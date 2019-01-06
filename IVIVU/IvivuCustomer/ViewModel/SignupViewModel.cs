using Ivivu.Model;
using Ivivu.ViewModel;
using IvivuCustomer.Model;
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
    class SignupViewModel : BaseViewModel
    {
        private bool IsSignedUp { get; set; }
        private String _FullName;
        public String FullName { get => _FullName; set { _FullName = value; OnPropertyChanged(); } }

        private String _UserName;
        public String UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private String _Password;
        public String Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private String _RetypePassword;
        public String RetypePassword { get => _RetypePassword; set { _RetypePassword = value; OnPropertyChanged(); } }

        private String _Address;
        public String Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private String _PhoneNumber;
        public String PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }

        private String _IdentityNumber;
        public String IdentityNumber { get => _IdentityNumber; set { _IdentityNumber = value; OnPropertyChanged(); } }

        private String _Email;
        public String Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private String _Description;
        public String Description { get => _Description; set { _Description = value; OnPropertyChanged(); } }

        public ICommand SignupCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RetypePasswordChangedCommand { get; set; }

        public SignupViewModel()
        {
            IsSignedUp = false;
            UserName = "";
            Password = "";
            RetypePassword = "";
            Address = "";
            PhoneNumber = "";
            IdentityNumber = "";
            Email = "";
            Description = "";

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            RetypePasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { RetypePassword = p.Password; });
            SignupCommand = new RelayCommand<Window>((p) =>
            {
                if (String.IsNullOrEmpty(UserName) == true || String.IsNullOrEmpty(Password) == true || String.IsNullOrEmpty(RetypePassword) == true || String.IsNullOrEmpty(Address) == true || String.IsNullOrEmpty(PhoneNumber) == true || String.IsNullOrEmpty(IdentityNumber) == true || String.IsNullOrEmpty(Email) == true)
                {
                    OnPropertyChanged();
                    return false;
                }
                return true;
            }
            , (p) =>
            {
                
                Signup(p);
            });
        }

        void Signup(Window signup)
        {
            if (signup == null)
                return;

            //kiểm tra thông tin trống
            //if (String.IsNullOrEmpty(UserName) == true || String.IsNullOrEmpty(Password) == true || String.IsNullOrEmpty(RetypePassword) == true || String.IsNullOrEmpty(Address) == true || String.IsNullOrEmpty(PhoneNumber) == true || String.IsNullOrEmpty(IdentityNumber) == true || String.IsNullOrEmpty(Email) == true)
            //{
            //    MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            //    return;
            //}

            //kiểm tra số điện thoại
            if(PhoneNumber.Length != 10)     //kiêm tra độ dài của số điện thoại
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập lại!");
                return;
            }
            int n;
            bool isNumeric = int.TryParse(PhoneNumber, out n);      //kiểm tra số điện thoại có chứa ký tự ngoài số không
            if(isNumeric == false)
            {
                MessageBox.Show("Số điện thoại không được chứa ký tự khác ngoài số");
                return;
            }

            //kiếm tra cmnd
            if (IdentityNumber.Length != 10)     //kiêm tra độ dài của số điện thoại
            {
                MessageBox.Show("Số CMND không hợp lệ. Vui lòng nhập lại!");
                return;
            }
            isNumeric = int.TryParse(IdentityNumber, out n);      //kiểm tra số điện thoại có chứa ký tự ngoài số không
            if (isNumeric == false)
            {
                MessageBox.Show("Số CMND không được chứa ký tự khác ngoài số");
                return;
            }

            //kiểm tra mật khẩu
            if (Password.Length < 8)
            {
                MessageBox.Show("Mật khẩu quá ngắn. Vui lòng nhập lại");
                return;
            }

            if(Password != RetypePassword)      //kiểm tra mật khảu có đúng không
            {
                MessageBox.Show("Mật khẩu và mật khẩu xác nhận không trùng nhau");
                return;
            }
            var checkUserName = DataProvider.Ins.DB.KhachHangs.Where(un => un.tenDangNhap == UserName).Count(); //kiểm tra sự tồn tại của username
            if (checkUserName > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại");
                return;
            }

            var checkEmail = DataProvider.Ins.DB.KhachHangs.Where(un => un.email == Email).Count(); //kiểm tra sự tồn tại của email
            if (checkUserName > 0)
            {
                MessageBox.Show("Email đã tồn tại");
                return;
            }
            SignUpWindow signupWindow = new SignUpWindow();

            if (signupWindow.DataContext == null)
                return;
            var signupVM = signupWindow.DataContext as SignupViewModel;
            int id = DataProvider.Ins.DB.KhachHangs.Count() + 1;

            var kh = new KhachHang()
            {
                maKH = id,
                hoTen = FullName,
                tenDangNhap = UserName,
                matKhau = Password,
                diaChi = Address,
                soDienThoai = PhoneNumber,
                soCMND = IdentityNumber,
                email = Email,
                moTa = Description
            };
            DataProvider.Ins.DB.KhachHangs.Add(kh);
            DataProvider.Ins.DB.SaveChanges();
            IsSignedUp = true;

            if (signupVM.IsSignedUp)
            {
                MessageBox.Show("Đăng ký thành công");
                signup.Close();
            }
        }
    }
}
