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
    public partial class Statistic : Window
    {
        public Statistic()
        {
            InitializeComponent();
        }

        private void btn_SttRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else if (string.IsNullOrEmpty(txb_minDay.Text))
                MessageBox.Show("Hãy nhập số ngày ít nhất!");
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_StatusRoomStatistic", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters.Add("@minOfDay", SqlDbType.Int);
                    cmd.Parameters["@hotel"].Value = Login.maKS;
                    cmd.Parameters["@dateBegin"].Value = dp_from.SelectedDate.Value.Date;
                    cmd.Parameters["@dateEnd"].Value = dp_to.SelectedDate.Value.Date;
                    cmd.Parameters["@minOfDay"].Value = int.Parse(txb_minDay.Text);
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

        private void btn_emptyRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dp_from.Text))
                MessageBox.Show("Hãy nhập ngày bắt đầu!");
            else if (string.IsNullOrEmpty(dp_to.Text))
                MessageBox.Show("Hãy nhập ngày kết thúc!");
            else
            {
                using (SqlConnection conn = new SqlConnection(Connection.connectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_EmptyRoomStatistics", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@hotel", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@dateBegin", SqlDbType.Date);
                    cmd.Parameters.Add("@dateEnd", SqlDbType.Date);
                    cmd.Parameters["@hotel"].Value = Login.maKS;
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
                document.DocumentInformation.Author = Login.maKS.ToString();
                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for a page
                PdfGraphics graphics = page.Graphics;

                //Set the standard
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello, World! ", font, PdfBrushes.Black, new PointF(0, 0));
                graphics.DrawString("From date: " + dp_from.SelectedDate.Value, font, PdfBrushes.Black, new PointF(0, 50));
                graphics.DrawString("To date: " + dp_to.SelectedDate.Value, font, PdfBrushes.Black, new PointF(0, 100));

                //Save the document
                if (string.IsNullOrEmpty(txb_fileName.Text))
                    document.Save("output.pdf");
                else
                    document.Save(txb_fileName.Text + ".pdf");
            }
        }
    }
}
