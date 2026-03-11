using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace mystock.Components
{
    public partial class AlertDialog : Window
    {
        public AlertDialog(string message, string type = "info")
        {
            InitializeComponent();

            txtMessage.Text = message;

            switch (type)
            {
                case "success":
                    txtIcon.Text = "✔";
                    txtTitle.Text = "สำเร็จ";
                    Header.Background = Brushes.Green;
                    break;

                case "warning":
                    txtIcon.Text = "⚠";
                    txtTitle.Text = "แจ้งเตือน";
                    Header.Background = Brushes.Orange;
                    break;

                case "error":
                    txtIcon.Text = "✖";
                    txtTitle.Text = "ผิดพลาด";
                    Header.Background = Brushes.Red;
                    break;

                default:
                    txtIcon.Text = "ℹ";
                    txtTitle.Text = "ข้อมูล";
                    Header.Background = Brushes.DodgerBlue;
                    break;
            }

            Loaded += AlertDialog_Loaded;
        }

        private void AlertDialog_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
            this.BeginAnimation(Window.OpacityProperty, fade);

            DoubleAnimation scaleAnim = new DoubleAnimation(0.8, 1, TimeSpan.FromMilliseconds(200));
            scale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, scaleAnim);
            scale.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}