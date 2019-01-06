using Ivivu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivivu.ViewModel
{
    public class ReceiptViewModel:BaseViewModel
    {
        private String _FullName;
        public String FullName { get => _FullName; set { _FullName = value; OnPropertyChanged(); } }

        private String _Address;
        public String Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private String _PhoneNumber;
        public String PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        

        private String _HotelName;
        public String HotelName { get => _HotelName; set { _HotelName = value; OnPropertyChanged(); } }

        private String _RetypePassword;
        public String RetypePassword { get => _RetypePassword; set { _RetypePassword = value; OnPropertyChanged(); } }
        
        private int _Stars;
        public int Stars { get => _Stars; set { _Stars = value; OnPropertyChanged(); } }

        private String _City;
        public String City { get => _City; set { _City = value; OnPropertyChanged(); } }

        private String _RoomTypeName;
        public String RoomTypeName { get => _RoomTypeName; set { _RoomTypeName = value; OnPropertyChanged(); } }

        private int _Total;
        public int Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }

        private String _ngayNhanPhong;
        public String ngayNhanPhong { get => _ngayNhanPhong; set { _ngayNhanPhong = value; OnPropertyChanged(); } }

        private String _ngayTraPhong;
        public String ngayTraPhong { get => _ngayTraPhong; set { _ngayTraPhong = value; OnPropertyChanged(); } }

        public ReceiptViewModel()
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

            OrderRoomWindow book = new OrderRoomWindow();
            var bookVM = book.DataContext as OrderRoomViewModel;


            FullName = DataProvider.Ins.DB.KhachHangs.Where(kh => kh.maKH == MAKH).SingleOrDefault().hoTen;
            Address = DataProvider.Ins.DB.KhachHangs.Where(kh => kh.maKH == MAKH).SingleOrDefault().diaChi;
            PhoneNumber = DataProvider.Ins.DB.KhachHangs.Where(kh => kh.maKH == MAKH).SingleOrDefault().soDienThoai;

            HotelName = DataProvider.Ins.DB.KhachSans.Where(ks => ks.maKS == MAKS).SingleOrDefault().tenKS;
            Stars = DataProvider.Ins.DB.KhachSans.Where(ks => ks.maKS == MAKS).SingleOrDefault().soSao;
            City = DataProvider.Ins.DB.KhachSans.Where(ks => ks.maKS == MAKS).SingleOrDefault().thanhPho;

            RoomTypeName = DataProvider.Ins.DB.LoaiPhongs.Where(lp => lp.maLoaiPhong == maPhong).SingleOrDefault().tenLoaiPhong;
            Total = DataProvider.Ins.DB.LoaiPhongs.Where(lp => lp.maLoaiPhong == maPhong).SingleOrDefault().donGia;

            ngayNhanPhong = bookVM.NgayDatPhong;
            ngayTraPhong = bookVM.NgayTraPhong;

        }
    }
}
