using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoAnNhom
{
    public partial class DangNhap : Window
    {
        private const string USERNAME = "admin";
        private const string PASSWORD = "123";

        public DangNhap()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
        }

        private void Button_DangNhap(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                CustomMessengeBox.Show("Vui lòng nhập đầy đủ thông tin!",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (username == USERNAME && password == PASSWORD)
            {
                CustomMessengeBox.Show("Đăng nhập thành công!",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                CustomMessengeBox.Show("Sai tài khoản hoặc mật khẩu!",
                                "Đăng nhập thất bại",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            lblUsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
                lblUsernamePlaceholder.Visibility = Visibility.Visible;
        }
            
        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            lblPasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
                lblPasswordPlaceholder.Visibility = Visibility.Visible;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}