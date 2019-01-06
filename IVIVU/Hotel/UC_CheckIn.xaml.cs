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
    /// Interaction logic for UC_CheckIn.xaml
    /// </summary>
    public partial class UC_CheckIn : UserControl
    {
        public UC_CheckIn()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingList();
        }

        private void LoadBookingList()
        {
            using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
            using (SqlCommand cmd = new SqlCommand("SP_BOOKINGLIST", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@maKS", SqlDbType.Int);
                cmd.Parameters["@maKS"].Value = Login.maKS;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dg_BookingList.ItemsSource = dt.DefaultView;
            }
        }

        private static string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            (dg_BookingList.ItemsSource as DataView).RowFilter = string.Format("Convert([Mã đặt phòng], System.String) LIKE '%{0}%' OR " +
                "[Người đặt] LIKE '%{0}%' OR Convert([Mã phòng], System.String) LIKE '%{0}%' OR [Số phòng] LIKE '%{0}%'", EscapeLikeValue(txb_search.Text));
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            if (dg_BookingList.SelectedCells.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_ConfirmBooking", conn))
                {
                    int bookingID = int.Parse(((DataRowView)dg_BookingList.SelectedItem).Row["Mã đặt phòng"].ToString());

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maDP", SqlDbType.Int);
                    cmd.Parameters["@maDP"].Value = bookingID;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    LoadBookingList();
                    conn.Close();

                    MessageBox.Show("Nhận phòng thành công!", "Thông báo", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một đơn đặt phòng!", "Thông báo");
            }
        }
    }
}
