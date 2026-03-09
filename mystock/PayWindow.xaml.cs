using mystock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mystock
{
    /// <summary>
    /// Interaction logic for PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {
        DbService db = new DbService();

        ObservableCollection<SaleItem> billItems;
        decimal total;

        public PayWindow(ObservableCollection<SaleItem> items)
        {
            InitializeComponent();

            billItems = items;

            total = billItems.Sum(x => x.Price * x.Qty);

            TotalText.Text = total.ToString("N2");
            CashBox.Focus();
        }

        private void CashBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal cash;

            if (decimal.TryParse(CashBox.Text, out cash))
            {
                decimal change = cash - total;

                ChangeText.Text = change.ToString("N2");
            }
            else
            {
                ChangeText.Text = "0.00";
            }
        }

        private void CashBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // ไม่ใช่ตัวเลข
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    decimal cash;
                    decimal change;

                    decimal.TryParse(CashBox.Text, out cash);
                    decimal.TryParse(ChangeText.Text, out change);

                    if (cash < total)
                    {
                       MessageBox.Show(
                       "เงินไม่พอจ่าย กรุณาตรวจสอบ",
                       "แจ้งเตือน",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning);

                        return;
                    }

                    change = cash - total;

                    SqlCommand saleCmd = new SqlCommand(@"
                    INSERT INTO sales
                    (total, cash, change)
                    OUTPUT INSERTED.id
                    VALUES (@total,@cash,@change)",
                    conn, tran);

                    saleCmd.Parameters.AddWithValue("@total", total);
                    saleCmd.Parameters.AddWithValue("@cash", cash);
                    saleCmd.Parameters.AddWithValue("@change", change);

                    int saleId = (int)saleCmd.ExecuteScalar();

                    foreach (var item in billItems)
                    {
                        // insert sale item
                        SqlCommand itemCmd = new SqlCommand(@"
                        INSERT INTO sale_items
                        (sale_id, product_id, qty, price)
                        VALUES (@sale_id,@product_id,@qty,@price)",
                            conn, tran);

                        itemCmd.Parameters.AddWithValue("@sale_id", saleId);
                        itemCmd.Parameters.AddWithValue("@product_id", item.ProductId);
                        itemCmd.Parameters.AddWithValue("@qty", item.Qty);
                        itemCmd.Parameters.AddWithValue("@price", item.Price);
                        //itemCmd.Parameters.AddWithValue("@total", item.Total);

                        itemCmd.ExecuteNonQuery();

                        // ลด stock
                        SqlCommand stockCmd = new SqlCommand(
                        "UPDATE products SET stock = stock - @qty WHERE id=@id",
                        conn, tran);

                        stockCmd.Parameters.AddWithValue("@qty", item.Qty);
                        stockCmd.Parameters.AddWithValue("@id", item.ProductId);

                        stockCmd.ExecuteNonQuery();
                    }

                    tran.Commit();


                    billItems.Clear();
                    //CalculateTotal();
                    //LoadProducts();

                    this.DialogResult = true;
                    this.Close();
                    //MessageBox.Show("ขายสำเร็จ");
                }
                catch
                {
                    tran.Rollback();

                    MessageBox.Show(
                      "เกิดข้อผิดพลาด!",
                      "แจ้งเตือน",
                      MessageBoxButton.OK,
                      MessageBoxImage.Error);
                }
            }
        }
    }
}
