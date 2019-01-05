﻿using Ivivu;
using Ivivu.Model;
using Ivivu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ivivu.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<KhachSan> _HotelList;
        public ObservableCollection<KhachSan> HotelList { get=> _HotelList; set { _HotelList = value; OnPropertyChanged(); } }

        private List<int> _StarNumber;
        public List<int> StarNumber { get => _StarNumber; set { _StarNumber = value; OnPropertyChanged(); } }
        
        private String _City;
        public String City { get => _City; set { _City = value; OnPropertyChanged(); } }

        private String _NgayDatPhong;
        public String NgayDatPhong { get => _NgayDatPhong; set { _NgayDatPhong = value; OnPropertyChanged(); } }

        private String _NgayTraPhong;
        public String NgayTraPhong { get => _NgayTraPhong; set { _NgayTraPhong = value; OnPropertyChanged(); } }

        private int _Stars;
        public int Stars { get => _Stars; set { _Stars = value; OnPropertyChanged(); } }

        private int _Price;
        public int Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        private KhachSan _SelectedHotel;
        public KhachSan SelectedHotel
        {
            get => _SelectedHotel;
            set
            {
                _SelectedHotel = value;
                OnPropertyChanged();
                if (_SelectedHotel != null)
                {
                    
                }
            }
        }

        public ICommand LoginButton { get; set; }
        public ICommand SignupButton { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand LogoutButton { get; set; }
        //public List<String> LongListToTestComboVirtualization { get; set; }

        public MainViewModel()
        {
            StarNumber = new List<int>()
            {
                 1, 2, 3, 4, 5
            };

            City = "";
            NgayDatPhong = "";
            NgayTraPhong = "";
            Stars = 0;
            Price = 0;

            LoginButton = new RelayCommand<Button>((p) => { return true; }, (p) =>
            {
                LoginWindow login = new LoginWindow();
                MainWindow main = new MainWindow();
                login.ShowDialog();

                if (login == null)
                    return;
                var loginVM = login.DataContext as LoginViewModel;

                if (loginVM.IsLoggedin)
                {
                    HideButton(p);
                    OnPropertyChanged();
                    ShowButton(main.btn_Logout);
                }

            });
            LogoutButton = new RelayCommand<Button>((p) => 
            {
                LoginWindow login = new LoginWindow();

                if (login == null)
                    return false;
                var loginVM = login.DataContext as LoginViewModel;
                if (loginVM.IsLoggedin)
                    ShowButton(p);
                return true;
            }, (p) =>
            {
                LoginWindow login = new LoginWindow();

                if (login == null)
                    return;
                var loginVM = login.DataContext as LoginViewModel;

                loginVM.IsLoggedin = false;
                MessageBox.Show("Đăng xuất thành công");
                HideButton(p);
            });
            SignupButton = new RelayCommand<Button>((p) => { return true; }, (p) => { SignUpWindow signUp = new SignUpWindow(); signUp.ShowDialog(); });
            SearchCommand = new RelayCommand<Button>((p) =>
            {
                return true;
            }, (p) =>
            {
                //if (City == "")
                //    return;
                //NgayDatPhong = Convert.ToDateTime(NgayDatPhong).ToShortDateString();
                //DateTimeOffset tp = Convert.ToDateTime(NgayDatPhong);
                ////if (dp > tp)
                ////    MessageBox.Show("Ngày dặt phòng không được trước hơn ngày trả phòng");
                //if (NgayDatPhong == "")
                //    return;
                //if (NgayTraPhong == "")
                //    return;
                //if (Stars == 0)
                //    return;
                //if (Price == 0)
                //    return;
                LoadHotelList();
            });
            
        }

        void HideButton(Button p)       //ẩn button
        {
            p.Visibility = Visibility.Collapsed;
        }
        void ShowButton(Button p)       //hiện button
        {
            p.Visibility = Visibility.Visible;
        }
        void LoadHotelList()
        {
            if (string.IsNullOrEmpty(City) == false && Stars != 0 && Price != 0)    //tìm dựa trên 3 tiêu chí
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.thanhPho == City && ht.soSao == Stars && ht.giaTB >= Price));
            else if (string.IsNullOrEmpty(City) == true && Stars != 0 && Price != 0)//tìm dựa trên giá và số sao
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.soSao == Stars && ht.giaTB >= Price));
            else if (string.IsNullOrEmpty(City) == false && Stars == 0 && Price != 0)//tìm dựa trên địa điểm và giá
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.thanhPho == City && ht.giaTB >= Price));
            else if (string.IsNullOrEmpty(City) == false && Stars != 0 && Price == 0)//tìm dựa trên địa điểm và số sao
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.thanhPho == City && ht.soSao == Stars));
            else if (string.IsNullOrEmpty(City) == true && Stars == 0 && Price != 0)//tìm kiếm dựa trên giá cả
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.giaTB >= Price));
            else if (string.IsNullOrEmpty(City) == true && Stars != 0 && Price == 0)
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.soSao == Stars));
            else
                HotelList = new ObservableCollection<KhachSan>(DataProvider.Ins.DB.KhachSans.Where(ht => ht.thanhPho == City));
        }
    }
}
