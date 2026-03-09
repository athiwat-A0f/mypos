using mystock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;


namespace mystock
{

    public partial class MainWindow : Window
    {
        const int QTY = 50;

        ObservableCollection<SaleItem> billItems = new ObservableCollection<SaleItem>();
        ProductService productService = new ProductService();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();

            SaleGrid.ItemsSource = billItems;
        }

        void LoadProducts()
        {
            ProductGrid.ItemsSource = productService.GetProducts();
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Product product = btn.Tag as Product;
        
            if (product.Stock <= 0)
            {
                MessageBox.Show(
                  product.ProductName + "\nสินค้า มี 0 ชิ้น",
                  "แจ้งเตือน",
                  MessageBoxButton.OK,
                  MessageBoxImage.Warning);

                return;
            }

            var exist = billItems.FirstOrDefault(x => x.ProductId == product.Id);

            if (exist != null)
            {
                exist.Qty += QTY;

                if (exist.Qty > product.Stock)
                {
                    MessageBox.Show(
                      "จำนวนเกิน STOCK",
                      "แจ้งเตือน",
                      MessageBoxButton.OK,
                      MessageBoxImage.Information);

                    exist.Qty = product.Stock;
                }
            }
            else
            {
                billItems.Add(new SaleItem
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Qty = QTY >= product.Stock ? product.Stock : QTY
                });
            }

            SaleGrid.Items.Refresh();

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal total = billItems.Sum(x => x.Price * x.Qty);

            TotalText.Text = total.ToString("N2") + " บาท";
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (billItems == null || billItems.Count == 0)
            {
                MessageBox.Show(
                "ไม่มีรายการสินค้า",
                "แจ้งเตือน",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                return;
            }

            PayWindow pay = new PayWindow(billItems);

            if (pay.ShowDialog() == true)
            {
                MessageBox.Show(
                "ชำระเงินสำเร็จ",
                "แจ้งเตือน",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                billItems.Clear();

                CalculateTotal();
                LoadProducts();
            }
        }

        private void Qty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void Qty_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            SaleItem item = txt.Tag as SaleItem;

            var row = productService.GetProducts().FirstOrDefault(x => x.Id == item.ProductId);

            int qty;

            if (!int.TryParse(txt.Text, out qty))
            {
                qty = 1;
            }

            if (qty > row.Stock)
            {
                MessageBox.Show(
                  "จำนวนเกิน STOCK",
                  "แจ้งเตือน",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information);

                qty = row.Stock;
            }

            if (qty <= 0)
            {
                qty = 1;
            }

            item.Qty = qty;

            SaleGrid.Items.Refresh();
            CalculateTotal();
        }

        private void IncreaseQty_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SaleItem item = btn.Tag as SaleItem;

            var row = productService.GetProducts().FirstOrDefault(x => x.Id == item.ProductId);

            item.Qty += QTY;

            if (item.Qty > row.Stock)
            {
                MessageBox.Show(
                  "จำนวนเกิน STOCK",
                  "แจ้งเตือน",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information);

                item.Qty = row.Stock;
            }

            SaleGrid.Items.Refresh();

            CalculateTotal();
        }


        private void DecreaseQty_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SaleItem item = btn.Tag as SaleItem;

            item.Qty -= QTY;

            if (item.Qty <= 0)
            {
                billItems.Remove(item);
            }

            SaleGrid.Items.Refresh();

            CalculateTotal();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (billItems == null || billItems.Count == 0)
            {
                MessageBox.Show(
                "ไม่มีรายการสินค้า",
                "แจ้งเตือน",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

                return;
            }

            var result = MessageBox.Show(
                "ต้องการยกเลิกรายการขายหรือไม่ ?",
                "แจ้งเตือน",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                billItems.Clear();
                TotalText.Text = "0.00 บาท";  // รีเซ็ตยอดเงิน
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            SaleItem item = btn.Tag as SaleItem;

            if (item != null)
            {
                billItems.Remove(item);
                CalculateTotal();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
             "ต้องการปิดโปรแกรม หรือไม่ ?",
             "แจ้งเตือน",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
             "เวอร์ชั่น 1.0.0",
             "ช่วยเหลือ",
             MessageBoxButton.OK,
             MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }

        }

    }
}
