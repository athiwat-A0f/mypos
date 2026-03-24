using mystock.Helper;
using mystock.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace mystock.Pages
{

    public partial class StockPage : Page
    {
        DbService db = new DbService();

        Stock selectedStockProduct;

        StockService StockService = new StockService();

        public StockPage()
        {
            InitializeComponent();

            LoadProductsProduct();

        }

        void LoadProductsProduct()
        {
            StockGrid.ItemsSource = StockService.GetStocks();
        }

        private void ClearForm()
        {
            txtQty.Text = "";
            txtCost.Text = "";
            txtCostAvg.Text = "0";
            txtRemark.Text = "";
        }

        private void StockGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStockProduct = StockGrid.SelectedItem as Stock;

            if (selectedStockProduct == null) return;

            txtName.Text = selectedStockProduct.ProductName;
            txtPrice.Text = selectedStockProduct.Price.ToString();
            txtCost.Text = selectedStockProduct.Cost.ToString();

        }

        private void StockIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCost.Text))
            {
                txtCost.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQty.Text))
            {
                txtQty.Focus();
                return;
            }

            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {

                    string sql = @"INSERT INTO stock
                                (
                                    product_id,
                                    qty,
                                    cost,
                                    cost_avg,
                                    remark
                                )
                                VALUES
                                (
                                    @product_id,
                                    @qty,
                                    @cost,
                                    @cost_avg,
                                    @remark
                                )";

                    SqlCommand cmd = new SqlCommand(sql, conn, tran);

                    cmd.Parameters.AddWithValue("@product_id", selectedStockProduct.ProductId);
                    cmd.Parameters.AddWithValue("@qty", decimal.Parse(txtQty.Text));
                    cmd.Parameters.AddWithValue("@cost", decimal.Parse(txtCost.Text));
                    cmd.Parameters.AddWithValue("@cost_avg", decimal.Parse(txtCostAvg.Text));
                    cmd.Parameters.AddWithValue("@remark", txtRemark.Text);

                    cmd.ExecuteNonQuery();

                    string sql2 = @"UPDATE products 
                       SET stock = stock + @qty
                       WHERE id = @id";

                    SqlCommand cmd2 = new SqlCommand(sql2, conn, tran);

                    cmd2.Parameters.AddWithValue("@id", selectedStockProduct.ProductId);
                    cmd2.Parameters.AddWithValue("@qty", decimal.Parse(txtQty.Text));

                    cmd2.ExecuteNonQuery();

                    tran.Commit();

                    Alert.Success("บันทึกสำเร็จ");

                    LoadProductsProduct();
                    ClearForm();
                }
                catch
                {
                    tran.Rollback();

                    Alert.Error("เกิดข้อผิดพลาด!");
                }
            }
        }


        private void StockOut_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtCost.Text))
            {
                txtCost.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQty.Text))
            {
                txtQty.Focus();
                return;
            }

            using (var conn = db.GetConnection())
            {
                conn.Open();

                SqlTransaction tran = conn.BeginTransaction();

                try
                {

                    string sql = @"INSERT INTO stock
                                (
                                    product_id,
                                    qty,
                                    cost,
                                    cost_avg,
                                    remark,
                                    type
                                )
                                VALUES
                                (
                                    @product_id,
                                    @qty,
                                    @cost,
                                    @cost_avg,
                                    @remark,
                                    @type
                                )";

                    SqlCommand cmd = new SqlCommand(sql, conn, tran);

                    cmd.Parameters.AddWithValue("@product_id", selectedStockProduct.ProductId);
                    cmd.Parameters.AddWithValue("@qty", decimal.Parse(txtQty.Text));
                    cmd.Parameters.AddWithValue("@cost", decimal.Parse(txtCost.Text));
                    cmd.Parameters.AddWithValue("@cost_avg", decimal.Parse(txtCostAvg.Text));
                    cmd.Parameters.AddWithValue("@remark", txtRemark.Text);
                    cmd.Parameters.AddWithValue("@type", "out");

                    cmd.ExecuteNonQuery();

                    string sql2 = @"UPDATE products 
                       SET stock = stock - @qty
                       WHERE id = @id";

                    SqlCommand cmd2 = new SqlCommand(sql2, conn, tran);

                    cmd2.Parameters.AddWithValue("@id", selectedStockProduct.ProductId);
                    cmd2.Parameters.AddWithValue("@qty", decimal.Parse(txtQty.Text));

                    cmd2.ExecuteNonQuery();

                    tran.Commit();

                    Alert.Success("บันทึกสำเร็จ");

                    LoadProductsProduct();
                    ClearForm();
                }
                catch
                {
                    tran.Rollback();

                    Alert.Error("เกิดข้อผิดพลาด!");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            ClearForm();
        }

        private void OnValueChanged(object sender, TextChangedEventArgs e)
        {
            if (!decimal.TryParse(txtQty.Text, out decimal qty))
            {
                txtCostAvg.Text = "";
                return;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal cost))
            {
                txtCostAvg.Text = "";
                return;
            }

            if (qty == 0)
            {
                txtCostAvg.Text = "0";
                return;
            }

            decimal avg = cost / qty;

            txtCostAvg.Text = avg.ToString("N2");
        }
    }
}
