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

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new Pages.LoginPage()); //ProductPage());
        }

        public void ShowMenu()
        {
            MainMenu.Visibility = Visibility.Visible;
        }

        private void MenuSale_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.SalePage());
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.ProductPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
             "ต้องการออกจากระบบ หรือไม่ ?",
             "แจ้งเตือน",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainMenu.Visibility = Visibility.Collapsed;
                MainFrame.Navigate(new Pages.LoginPage());
            }
        }


        private void Help_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
             "เวอร์ชั่น 1.0.1",
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
