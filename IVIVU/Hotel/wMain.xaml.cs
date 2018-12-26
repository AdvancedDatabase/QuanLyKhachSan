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
    /// Interaction logic for wMain.xaml
    /// </summary>
    public partial class wMain : Window
    {
        public wMain()
        {
            InitializeComponent();
        }

        private void btn_book_Click(object sender, RoutedEventArgs e)
        {
            BookRoom window = new BookRoom();
            this.Hide();
            window.ShowDialog();
            this.Show();
        }

        private void btn_Report_Click(object sender, RoutedEventArgs e)
        {
            Report window = new Report();
            this.Hide();
            window.ShowDialog();
            this.Show();
        }

        private void btn_Statistic_Click(object sender, RoutedEventArgs e)
        {
            Statistic window = new Statistic();
            this.Hide();
            window.ShowDialog();
            this.Show();
        }
    }
}
