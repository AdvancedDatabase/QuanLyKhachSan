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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;

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
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_MonthlyRevenueReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters["@hotel"].Value = maKS;
                    cmd.Parameters["@dateBegin"].Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters["@dateEnd"].Value = dp_to.SelectedDate.Value.Date;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGrid.ItemsSource = ds.Tables[0].DefaultView;
                    }
                    conn.Close();
                }
            }
        }

        private void btn_yearReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_YearRevenueReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters["@hotel"].Value = maKS;
                    cmd.Parameters["@dateBegin"].Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters["@dateEnd"].Value = dp_to.SelectedDate.Value.Date;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGrid.ItemsSource = ds.Tables[0].DefaultView;
                    }
                    conn.Close();

                }
            }
        }

        private void btn_roomReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_RoomRevenueReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters["@hotel"].Value = maKS;
                    cmd.Parameters["@dateBegin"].Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters["@dateEnd"].Value = dp_to.SelectedDate.Value.Date;
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGrid.ItemsSource = ds.Tables[0].DefaultView;
                    }
                    conn.Close();
                }
            }
        }

        private void btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            using (PdfDocument document = new PdfDocument())
            {
                document.DocumentInformation.Author = maKS.ToString();
                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;

                //Set the standard
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello, World! ", font, PdfBrushes.Black, new PointF(0, 0));
                graphics.DrawString("From date: "+ dp_from.SelectedDate.Value, font, PdfBrushes.Black, new PointF(0, 50));
                graphics.DrawString("To date: "+ dp_to.SelectedDate.Value, font, PdfBrushes.Black, new PointF(0, 100));

                //Save the document
                if (string.IsNullOrEmpty(txb_fileName.Text))
                    document.Save("output.pdf");
                else
                    document.Save(txb_fileName.Text + ".pdf");
            }
        }
    }
}
