using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace Hotel
{
    /// <summary>
    /// Interaction logic for UC_FindBill.xaml
    /// </summary>
    public partial class UC_FindBill : UserControl
    {
        public UC_FindBill()
        {
            InitializeComponent();
        }

        private void BtnDatVe(object sender, RoutedEventArgs e)
        {
            if (tb_MaKH.Text == "" && tb_tong.Text == "" && string.IsNullOrEmpty(dp_date.Text))
            {
                MessageBox.Show("Bạn chưa chọn thông tin cần tìm!!!");
            }
            else if ((tb_MaKH.Text != "" && tb_tong.Text != "") || (tb_tong.Text != "" && !string.IsNullOrEmpty(dp_date.Text)) || (tb_MaKH.Text != "" && !string.IsNullOrEmpty(dp_date.Text)) || (tb_MaKH.Text != "" && tb_tong.Text != "" && !string.IsNullOrEmpty(dp_date.Text)))
            {
                MessageBox.Show("Bạn vui lòng chỉ chọn một thông tin cần tìm kiếm.");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("usp_timKiemThongTinHoaDon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maKS", SqlDbType.Int).Value = Login.maKS;
                    if (tb_MaKH.Text == "") cmd.Parameters.Add("@maKH", SqlDbType.Int).Value = 0;
                    else cmd.Parameters.Add("@maKH", SqlDbType.Int).Value = int.Parse(tb_MaKH.Text);
                    if (!string.IsNullOrEmpty(dp_date.Text)) cmd.Parameters.Add("@ngayLap", SqlDbType.Date).Value = dp_date.SelectedDate.Value.Date;
                    else cmd.Parameters.Add("@ngayLap", SqlDbType.DateTime).Value = DBNull.Value;
                    if (tb_tong.Text == "") cmd.Parameters.Add("@thanhTien", SqlDbType.Int).Value = 0;
                    else cmd.Parameters.Add("@thanhTien", SqlDbType.Int).Value = int.Parse(tb_tong.Text);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataGrid1.Columns[0].Header = "Họ tên";
                    dataGrid1.Columns[1].Header = "CMND";
                    dataGrid1.Columns[2].Header = "SĐT";
                    dataGrid1.Columns[3].Header = "Ngày đặt phòng";
                    dataGrid1.Columns[4].Header = "Ngày trả phòng";
                    dataGrid1.Columns[5].Visibility = System.Windows.Visibility.Hidden;
                    dataGrid1.Columns[6].Header = "Tổng tiền";
                    dataGrid1.Columns[7].Header = "Ngày thanh toán";

                    dataGrid1.Columns[0].Width = 200;

                    conn.Close();

                }
                tb_MaKH.Text = "";
                tb_tong.Text = "";
                dp_date.Text = "";
            }
        }

        private void Tb_tong_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
