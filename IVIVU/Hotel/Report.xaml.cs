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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for Statistic.xaml
    /// </summary>
    public partial class Report : Window
    {
        int maKS;
        public Report()
        {
            InitializeComponent();
            maKS = Login.maKS;
        }

        private void btn_monthlyReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("MonthlyReport");
        }

        private void btn_yearReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("YearReport");
        }

        private void btn_roomReport_Click(object sender, RoutedEventArgs e)
        {
            CallReport("RoomReport");
        }

        private void CallReport(string name)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(@"E:\Year3\Term1\AdvancedDatabase\QuanLyKhachSan\IVIVU\Hotel\" + name + ".rpt");
                rpt.SetParameterValue("@hotel", maKS);
                rpt.SetParameterValue("@dateBegin", dp_from.SelectedDate.Value.Date);
                rpt.SetParameterValue("@dateEnd", dp_to.SelectedDate.Value.Date);
                rpt.SetParameterValue("name", "TuanDoan");
                crView_Report.ViewerCore.ReportSource = rpt;
            }
        }
    }
}
