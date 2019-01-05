using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "CheckIn":
                    txb_title.Text = "Nhận phòng";
                    usc = new UC_CheckIn();
                    GridMain.Children.Add(usc);
                    break;
                case "CheckOut":
                    txb_title.Text = "Trả phòng";
                    usc = new UC_CheckOut();
                    GridMain.Children.Add(usc);
                    break;
                case "Statistic":
                    txb_title.Text = "Thống kê";
                    usc = new UC_Statistic();
                    GridMain.Children.Add(usc);
                    break;
                case "Report":
                    txb_title.Text = "Báo cáo";
                    usc = new UC_Report();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }
    }
}
