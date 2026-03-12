using mystock.Components;
using System.Windows;
using mystock.Helper;


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

        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.ProductPage());
        }
        private void SellReport_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.SellReportPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AlertDialog2 dialog = new AlertDialog2("ต้องการออกจากระบบ หรือไม่ ?", "question");

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                MainMenu.Visibility = Visibility.Collapsed;
                MainFrame.Navigate(new Pages.LoginPage());
            }
        }


        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Alert.Info("เวอร์ชั่น 1.0.1");

        }


    }
}
