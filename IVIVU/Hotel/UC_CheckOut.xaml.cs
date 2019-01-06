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
    /// Interaction logic for UC_CheckOut.xaml
    /// </summary>
    public partial class UC_CheckOut : UserControl
    {
        public static int bookingID, roomID, cost, billID;
        public static string customer, roomNumber;
        public static DateTime datetFrom, dateTo, bookingDate;

        public UC_CheckOut()
        {
            InitializeComponent();
        }

        private void uc_CheckOut_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUnpaidList();
        }

        private void LoadUnpaidList()
        {
            using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
            using (SqlCommand cmd = new SqlCommand("SP_UnpaidList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@maKS", SqlDbType.Int);
                cmd.Parameters["@maKS"].Value = Login.maKS;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dg_unpaidList.ItemsSource = dt.DefaultView;
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
            (dg_unpaidList.ItemsSource as DataView).RowFilter = string.Format("Convert([Mã đặt phòng], System.String) LIKE '%{0}%' OR " +
                "[Người đặt] LIKE '%{0}%' OR Convert([Mã phòng], System.String) LIKE '%{0}%' OR [Số phòng] LIKE '%{0}%'", EscapeLikeValue(txb_search.Text));
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            if (dg_unpaidList.SelectedCells.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_Payment", conn))
                {
                    bookingID = int.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Mã đặt phòng"].ToString());
                    roomID = int.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Mã phòng"].ToString());
                    cost = int.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Đơn giá (đồng)"].ToString());
                    customer = ((DataRowView)dg_unpaidList.SelectedItem).Row["Người đặt"].ToString();
                    roomNumber = ((DataRowView)dg_unpaidList.SelectedItem).Row["Số phòng"].ToString();
                    datetFrom = DateTime.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Ngày bắt đầu"].ToString());
                    dateTo = DateTime.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Ngày trả phòng"].ToString());
                    bookingDate = DateTime.Parse(((DataRowView)dg_unpaidList.SelectedItem).Row["Ngày đặt"].ToString());

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@maDP", SqlDbType.Int).Value = bookingID;
                    cmd.Parameters.Add("@maHoaDon", SqlDbType.Int);
                    cmd.Parameters["@maHoaDon"].Direction = ParameterDirection.Output;
                    conn.Open();
                    cmd.ExecuteScalar();
                    billID = (int)cmd.Parameters["@maHoaDon"].Value;
                    LoadUnpaidList();

                    conn.Close();

                    Bill bill = new Bill();
                    bill.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một đơn đặt phòng!", "THÔNG BÁO");
            }
        }
    }
}
