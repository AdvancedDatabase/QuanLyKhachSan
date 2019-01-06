using CrystalDecisions.CrystalReports.Engine;
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
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class Bill : Window
    {
        public Bill()
        {
            InitializeComponent();
        }

       
        private void billWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReportDocument rpt = new ReportDocument();
            rpt.Load(@"E:\Year3\Term1\AdvancedDatabase\QuanLyKhachSan\IVIVU\Hotel\Report_Statistic\" + "Bill.rpt");
            rpt.SetParameterValue("maDP", UC_CheckOut.bookingID);
            rpt.SetParameterValue("khachHang", UC_CheckOut.customer);
            rpt.SetParameterValue("phong", UC_CheckOut.roomNumber);
            rpt.SetParameterValue("donGia", UC_CheckOut.cost);
            rpt.SetParameterValue("ngayBD", UC_CheckOut.datetFrom);
            rpt.SetParameterValue("ngayKT", UC_CheckOut.dateTo);
            rpt.SetParameterValue("maHD", UC_CheckOut.billID);
            rpt.SetParameterValue("hotelName", Login.hotelName);

            crView_Statistic.ViewerCore.ReportSource = rpt;
        }
    }
}
