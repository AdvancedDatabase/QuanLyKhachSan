using CrystalDecisions.CrystalReports.Engine;
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
    /// Interaction logic for UC_Report.xaml
    /// </summary>
    public partial class UC_Report : UserControl
    {
        public UC_Report()
        {
            InitializeComponent();
        }

        private void btn_monthlyReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("MonthlyReport", "SP_MonthlyRevenueReport");
        }

        private void btn_yearReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("YearReport", "SP_YearRevenueReport");
        }

        private void btn_roomReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("RoomReport", "SP_RoomRevenueReport");
        }

        private string GetPath(string fileName)
        {
            //MessageBox.Show(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            //MessageBox.Show(System.IO.Directory.GetCurrentDirectory());
            //MessageBox.Show(System.Environment.CurrentDirectory);
            string path = System.IO.Directory.GetCurrentDirectory();
            path = path.Substring(0, Math.Max(0, path.Length - 10));
            path += "\\Report_Statistic\\" + fileName + ".rpt";
            return path;
        }

        private void CallReport(string fileName, string spName)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(GetPath(fileName));
                //report.SetDatabaseLogon("sa", "password");//if your are using sqlAuthentication
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.Int).Value = Login.maKS;
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date).Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date).Value = dp_to.SelectedDate.Value.Date;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    rpt.SetDataSource(dt);
                    rpt.SetParameterValue("@hotel", Login.maKS);
                    rpt.SetParameterValue("@dateBegin", dp_from.SelectedDate.Value.Date);
                    rpt.SetParameterValue("@dateEnd", dp_to.SelectedDate.Value.Date);
                    rpt.SetParameterValue("hotelName", Login.hotelName);
                    crView_Report.ViewerCore.ReportSource = rpt;
                }
            }
        }
    }
}
