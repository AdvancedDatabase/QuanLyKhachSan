using Ivivu.Model;
using Ivivu.ViewModel;
using IvivuCustomer.Model;
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
    class RoomTypeViewModel:BaseViewModel
    {
        private ObservableCollection<LoaiPhong> _RoomTypeList;
        public ObservableCollection<LoaiPhong> RoomTypeList { get => _RoomTypeList; set { _RoomTypeList = value; OnPropertyChanged(); } }

        private LoaiPhong _SelectdRoomType;
        public LoaiPhong SelectedRoomType
        {
            get => _SelectdRoomType;
            set
            {
                _SelectdRoomType = value;
                OnPropertyChanged();
            }
        }


        public int maLP { get; set; }


        public ICommand BookRoom { get; set; }

        public RoomTypeViewModel()
        {
            LoadRoomTypeList();
            BookRoom = new RelayCommand<Button>((a) =>
            {
                if (SelectedRoomType == null)
                    return false;
                ShowButton(a);
                OnPropertyChanged();
                return true;
            }, (a) =>
            {
                //KhachSan ksan = DataProvider.Ins.DB.KhachSans.Where(ks => ks.tenKS == SelectedHotel.tenKS).SingleOrDefault();
                //maKS = ksan.maKS;
                //RoomType roomtype = new RoomType();
                //roomtype.ShowDialog();
                //OnPropertyChanged();
                LoginWindow login = new LoginWindow();
                var loginVM = login.DataContext as LoginViewModel;
                if(loginVM.IsLoggedin)
                {
                    LoaiPhong phong = DataProvider.Ins.DB.LoaiPhongs.Where(lp=> lp.maLoaiPhong == SelectedRoomType.maLoaiPhong).SingleOrDefault();
                    maLP = phong.maLoaiPhong;
                    OrderRoomWindow book = new OrderRoomWindow();
                    book.ShowDialog();
                }
                else
                {
                    login.ShowDialog();
                }
            });
        }
        void LoadRoomTypeList()
        {
            MainWindow main = new MainWindow();
            var mainvm = main.DataContext as MainViewModel;

            RoomTypeList = new ObservableCollection<LoaiPhong>(DataProvider.Ins.DB.LoaiPhongs.Where(rt => rt.maKS == mainvm.maKS));
        }
        void ShowButton(Button btn)
        {
            btn.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
