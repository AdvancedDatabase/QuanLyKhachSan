using Ivivu.Model;
using Ivivu.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ivivu.ViewModel
{
    public class OrderRoomViewModel:BaseViewModel
    {
        private String _NgayDatPhong;
        public String NgayDatPhong { get => _NgayDatPhong; set { _NgayDatPhong = value; OnPropertyChanged(); } }

        private String _NgayTraPhong;
        public String NgayTraPhong { get => _NgayTraPhong; set { _NgayTraPhong = value; OnPropertyChanged(); } }
        public ICommand BookingCommand { get; set; }
        public OrderRoomViewModel()
        {
            BookingCommand = new RelayCommand<Button>((p) => {
                if (string.IsNullOrEmpty(NgayDatPhong) == true || string.IsNullOrEmpty(NgayTraPhong) == true)
                    return false;
                DateTime ngayDatPhong = DateTime.Parse(NgayDatPhong);
                DateTime ngayTraPhong = DateTime.Parse(NgayTraPhong);
                if (ngayDatPhong < DateTime.Now)
                    return false;
                if (ngayDatPhong > ngayTraPhong)
                    return false;
                return true;
            }, (p) =>
            {
                LoginWindow login = new LoginWindow();
                var loginVM = login.DataContext as LoginViewModel;
                int MAKH = loginVM.maKH;

                MainWindow main = new MainWindow();
                var mainVM = main.DataContext as MainViewModel;
                int MAKS = mainVM.maKS;

                RoomType room = new RoomType();
                var roomVM = room.DataContext as RoomTypeViewModel;
                int maPhong = roomVM.maLP;

                DateTime ngayDatPhong = DateTime.Parse(NgayDatPhong);
                DateTime ngayTraPhong = DateTime.Parse(NgayTraPhong);

                DataProvider.Ins.DB.Dat_Phong(MAKS, maPhong, MAKH, ngayDatPhong, ngayTraPhong);
                MessageBox.Show("Đặt phòng thành công");

                ReceiptWindow receipt = new ReceiptWindow();
                receipt.ShowDialog();
            });
            
        }
    }
}
