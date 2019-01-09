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
    /// Interaction logic for Detail.xaml
    /// </summary>
    public partial class Detail : Window
    {
        public Detail()
        {
            InitializeComponent();
        }

        public Detail(string _tenLoaiPhong, string _soPhong, int _donGia, string _moTa, DateTime _ngay, string _tinhTrang) : this()
        {
            InitializeComponent();
            tb_tenLoaiPhong.Text = _tenLoaiPhong;
            tb_soPhong.Text = _soPhong;
            tb_donGia.Text = _donGia.ToString();
            tb_moTa.Text = _moTa;
            tb_ngay.Text = _ngay.ToString();
            tb_tinhTrang.Text = _tinhTrang;

        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
