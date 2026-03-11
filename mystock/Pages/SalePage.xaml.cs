using mystock.Components;
using mystock.Helper;
using mystock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace mystock.Pages
{
    /// <summary>
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {

        const int QTY = 50;

        ObservableCollection<SaleItem> billItems = new ObservableCollection<SaleItem>();
        ProductService productService = new ProductService();

        public SalePage()
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
                Alert.Warning(product.ProductName + "\nสินค้า มี 0 ชิ้น");

                return;
            }

            var exist = billItems.FirstOrDefault(x => x.ProductId == product.Id);

            if (exist != null)
            {
                exist.Qty += QTY;

                if (exist.Qty > product.Stock)
                {
                    Alert.Error("จำนวนเกิน STOCK");

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
                Alert.Info("ไม่มีรายการสินค้า!");

                return;
            }

            PayWindow pay = new PayWindow(billItems);

            if (pay.ShowDialog() == true)
            {
                Alert.Success("ชำระเงินสำเร็จ");

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
                Alert.Error("จำนวนเกิน STOCK");

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
                Alert.Error("จำนวนเกิน STOCK");

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
                Alert.Info("ไม่มีรายการสินค้า!");

                return;
            }


            AlertDialog2 dialog = new AlertDialog2("ต้องการยกเลิกรายการขายหรือไม่ ?", "question");
           
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                billItems.Clear();
                TotalText.Text = "0.00 บาท";
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

    
    }
}
