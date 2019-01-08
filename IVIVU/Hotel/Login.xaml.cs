using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public static int maKS;
        public static string hotelName, employeeName;
        public Login()
        {
            InitializeComponent();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txb_username.Text))
                MessageBox.Show("Hãy nhập tên đăng nhập!");
            else if (string.IsNullOrEmpty(passbox.Password))
                MessageBox.Show("Hãy nhập mật khẩu!");
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                    using (SqlCommand cmd = new SqlCommand("SP_Login_Admin", conn))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@tenDangNhap", SqlDbType.NVarChar).Value = txb_username.Text;
                        cmd.Parameters.Add("@matKhau", SqlDbType.NVarChar).Value = passbox.Password;
                        cmd.Parameters.Add("@maKS", SqlDbType.Int);
                        cmd.Parameters.Add("@tenKS", SqlDbType.NVarChar, 50);
                        cmd.Parameters.Add("@tenNV", SqlDbType.NVarChar, 50);
                        cmd.Parameters["@maKS"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@tenKS"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@tenNV"].Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteScalar();
                        maKS = (int)cmd.Parameters["@maKS"].Value;
                        hotelName = (string)cmd.Parameters["@tenKS"].Value;
                        employeeName = (string)cmd.Parameters["@tenNV"].Value;
                        conn.Close();
                    }
                    Main window = new Main();
                    this.Hide();
                    window.ShowDialog();
                    this.Show();
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.Message, "Thông báo", MessageBoxButton.OK);
                }
            }
        }
    }
}
