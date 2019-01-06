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
    /// Interaction logic for ListRoom.xaml
    /// </summary>
    public partial class ListRoom : UserControl
    {
        public ListRoom()
        {
            InitializeComponent();
        }

        private void ListRoom_load(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
            using (SqlCommand cmd = new SqlCommand("proc_DSLoaiPhong", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@maKS", SqlDbType.Int).Value = Login.maKS;
                conn.Open();
                cmd.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                loaiPhong.ItemsSource = dt.DefaultView;
                loaiPhong.SelectedItem = 1;
                loaiPhong.DisplayMemberPath = "tenloaiphong";
                loaiPhong.SelectedValuePath = "maloaiphong";
            }
        }

        private void LoaiPhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void BtnLietKe(object sender, RoutedEventArgs e)
        {
            if (loaiPhong.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn loại phòng!!!", "Lỗi");
            }
            else if (string.IsNullOrEmpty(dp_dateCheckRoom.Text))
            {
                MessageBox.Show("Bạn chưa chọn ngày!!!", "Lỗi");
            }
            else if (loaiPhong.Text == "" && string.IsNullOrEmpty(dp_dateCheckRoom.Text))
            {
                MessageBox.Show("Bạn chưa chọn thông tin!!!", "Lỗi");
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("proc_KTTinhTrangPhong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maKS", SqlDbType.Int).Value = Login.maKS;
                    cmd.Parameters.Add("@maLoaiPhong", SqlDbType.Int).Value = loaiPhong.SelectedValue;
                    cmd.Parameters.Add("@ngay", SqlDbType.Date).Value = dp_dateCheckRoom.SelectedDate.Value.Date;
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGrid2.ItemsSource = dt.DefaultView;
                    DataColumn a = dataGrid2.SelectedItem as DataColumn;

                    dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Hidden;
                    dataGrid2.Columns[2].Visibility = System.Windows.Visibility.Hidden;
                    dataGrid2.Columns[1].Header = "Tên loại phòng";
                    dataGrid2.Columns[3].Header = "Số phòng";
                    dataGrid2.Columns[4].Header = "Ngày";
                    dataGrid2.Columns[5].Header = "Tình trạng";
                    dataGrid2.Columns[1].Width = 300;
                    dataGrid2.Columns[3].Width = 300;
                    dataGrid2.Columns[4].Width = 300;
                    dataGrid2.Columns[5].Width = 200;
                    conn.Close();
                }
            }
        }

        private void BtnChiTiet(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid2.SelectedItem as DataRowView;
            if (row != null)
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("proc_ChiTietPhong", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maKS", SqlDbType.Int).Value = Login.maKS;
                    cmd.Parameters.Add("@maLoaiPhong", SqlDbType.Int).Value = row.Row.ItemArray[0];
                    cmd.Parameters.Add("@maPhong", SqlDbType.Int).Value = row.Row.ItemArray[2];
                    cmd.Parameters.Add("@ngay", SqlDbType.DateTime).Value = row.Row.ItemArray[4];
                    cmd.Parameters.Add("@tinhtrang", SqlDbType.NVarChar).Value = row.Row.ItemArray[5];
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    string soPhong = dt.Rows[0][1].ToString();
                    string tenLoaiPhong = dt.Rows[0][0].ToString();
                    int donGia = int.Parse(dt.Rows[0][2].ToString());
                    string moTa = dt.Rows[0][3].ToString();
                    DateTime ngay = DateTime.Parse(dt.Rows[0][4].ToString());
                    string tinhTrang = dt.Rows[0][5].ToString();
                    //Hiển thị chi tiết phòng
                    Detail detail = new Detail(tenLoaiPhong, soPhong, donGia, moTa, ngay, tinhTrang);
                    detail.Show();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn phòng!!!", "Chú ý");
            }
        }
    }
}
