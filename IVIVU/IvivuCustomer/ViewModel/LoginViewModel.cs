using Ivivu.Model;
using Ivivu.ViewModel;
using Ivivu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IvivuCustomer.Model;

namespace Ivivu.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public bool IsLoggedin { get; set; }
        public int maKH { get; set; }
        private String _UserName;
        public String UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private String _Password;
        public String Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        
        public LoginViewModel()
        {
            IsLoggedin = false;
            Password = "";
            UserName = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        void Login(Window login)
        {
            if (login == null)
                return;
            LoginWindow loginWindow = new LoginWindow();

            if (loginWindow.DataContext == null)
                return;
            var loginVM = loginWindow.DataContext as LoginViewModel;

            var checkLogin = DataProvider.Ins.DB.KhachHangs.Where(kh => kh.tenDangNhap == UserName && kh.matKhau == Password).Count();
            if (checkLogin > 0)
                IsLoggedin = true;
            if (loginVM.IsLoggedin)
            {
                MessageBox.Show("Đăng nhập thành công");
                login.Close();
                KhachHang kh = DataProvider.Ins.DB.KhachHangs.Where(a => a.tenDangNhap == UserName).Single();
                maKH = kh.maKH;
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng");
            }
        }
    }
}
