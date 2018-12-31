﻿using System;
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

        private void CallReport(string fileName, string spName)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(@"E:\Year3\Term1\AdvancedDatabase\QuanLyKhachSan\IVIVU\Hotel\Report_Statistic\" + fileName + ".rpt");
                //"" + GetCurrentPath()[0] + "Reports\\" + rptName + ""
                //report.SetDatabaseLogon("sa", "password");//if your are using sqlAuthentication
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.Int);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters["@hotel"].Value = Login.maKS;
                    cmd.Parameters["@dateBegin"].Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters["@dateEnd"].Value = dp_to.SelectedDate.Value.Date;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    rpt.SetDataSource(dt);
                    rpt.SetParameterValue("@hotel", maKS);
                    rpt.SetParameterValue("@dateBegin", dp_from.SelectedDate.Value.Date);
                    rpt.SetParameterValue("@dateEnd", dp_to.SelectedDate.Value.Date);
                    crView_Report.ViewerCore.ReportSource = rpt;
                }
            }
        }
    }
}
