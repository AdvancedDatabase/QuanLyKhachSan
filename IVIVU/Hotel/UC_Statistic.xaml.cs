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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for UC_Statistic.xaml
    /// </summary>
    public partial class UC_Statistic : UserControl
    {
        public UC_Statistic()
        {
            InitializeComponent();
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

        private void CallStatistic(string name)
        {
            if (name == "StatusRoomStatistic" && string.IsNullOrEmpty(txb_minDay.Text))
            {
                MessageBox.Show("Hãy nhập số ngày tối thiểu");
                return;
            }
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                ReportDocument rpt = new ReportDocument();
                rpt.Load(GetPath(name));
                //report.SetDatabaseLogon("sa", "password");//if your are using sqlAuthentication
                rpt.SetParameterValue("@hotel", Login.maKS);
                rpt.SetParameterValue("@dateBegin", dp_from.SelectedDate.Value.Date);
                rpt.SetParameterValue("@dateEnd", dp_to.SelectedDate.Value.Date);
                rpt.SetParameterValue("hotelName", Login.hotelName);
                if (name == "StatusRoomStatistic")
                    rpt.SetParameterValue("@minOfDay", int.Parse(txb_minDay.Text));
                crView_Statistic.ViewerCore.ReportSource = rpt;
            }
        }

        private void btn_SttRoom_Click(object sender, RoutedEventArgs e)
        {
            CallStatistic("StatusRoomStatistic");
        }

        private void btn_emptyRoom_Click(object sender, RoutedEventArgs e)
        {
            CallStatistic("EmptyRoomStatistic");
        }
    }
}
