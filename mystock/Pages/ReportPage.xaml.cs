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
 
    public partial class ReportPage : Page
    {

        ProductService productService = new ProductService();
        public ReportPage()
        {
            InitializeComponent();

            LoadProducts();

        }

        void LoadProducts()
        {
            ProductGrid.ItemsSource = productService.GetProducts();
        }

        private void SellHistory_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("เปิดประวัติการขาย");
        }

        private void StockHistory_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("เปิดประวัติสต๊อก");
        }

        private void DailyReport_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("เปิดรายงานยอดขาย");
        }

        private void TopProduct_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("เปิดสินค้าขายดี");
        }
    }
}
