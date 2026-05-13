using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoAnNhom
{
    
    public partial class DangNhap : UserControl
    {
        private static readonly string connectionString =
        ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public DangNhap()
        {
            InitializeComponent();
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

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT Id, TenDangNhap, VaiTro 
                             FROM NguoiDung
                             WHERE TenDangNhap = @user
                             AND MatKhau = @pass";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                       MainWindow.CurrentUser = new NguoiDung
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TenDangNhap = reader["TenDangNhap"].ToString(),
                            VaiTro = reader["VaiTro"].ToString()
                        };

                        CustomMessengeBox.Show("Đăng nhập thành công!",
                            "Thông báo",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        MainWindow.Instance.UpdateMenuBar();
                        MainWindow.Instance.Navigate("TrangChu");
                    }
                    else
                    {
                        CustomMessengeBox.Show("Sai tài khoản hoặc mật khẩu!",
                            "Đăng nhập thất bại",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message);
            }
        }

        private void TxtDangKy_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.Instance.Navigate("DangKy");
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
            Window.GetWindow(this)?.Close();
        }
    }
}