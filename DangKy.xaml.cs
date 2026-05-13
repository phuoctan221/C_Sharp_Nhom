using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom.Views
{
    public partial class DangKy : UserControl
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public DangKy()
        {
            InitializeComponent();
        }

        // ================= PLACEHOLDER =================

        private void txtTenDangNhap_GotFocus(object sender, RoutedEventArgs e)
        {
            lblTenDangNhap.Visibility = Visibility.Collapsed;
        }

        private void txtTenDangNhap_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                lblTenDangNhap.Visibility = Visibility.Visible;
        }

        private void txtHoTen_GotFocus(object sender, RoutedEventArgs e)
        {
            lblHoTen.Visibility = Visibility.Collapsed;
        }

        private void txtHoTen_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                lblHoTen.Visibility = Visibility.Visible;
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            lblEmail.Visibility = Visibility.Collapsed;
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                lblEmail.Visibility = Visibility.Visible;
        }

        private void txtMatKhau_GotFocus(object sender, RoutedEventArgs e)
        {
            lblMatKhau.Visibility = Visibility.Collapsed;
        }

        private void txtMatKhau_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhau.Password))
                lblMatKhau.Visibility = Visibility.Visible;
        }

        // ================= ĐĂNG KÝ =================

        private void BtnDangKy_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Password))
            {
                CustomMessengeBox.Show(
                    "Vui lòng nhập đầy đủ thông tin!",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // ✅ Kiểm tra username trùng
                string checkUserSql = "SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap=@TenDangNhap";
                SqlCommand checkUserCmd = new SqlCommand(checkUserSql, conn);
                checkUserCmd.Parameters.AddWithValue("@TenDangNhap", txtTenDangNhap.Text.Trim());

                int userExists = (int)checkUserCmd.ExecuteScalar();
                if (userExists > 0)
                {
                    CustomMessengeBox.Show(
                        "Tên đăng nhập đã tồn tại!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // ✅ Kiểm tra email trùng
                string checkEmailSql = "SELECT COUNT(*) FROM NguoiDung WHERE Email=@Email";
                SqlCommand checkEmailCmd = new SqlCommand(checkEmailSql, conn);
                checkEmailCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                int emailExists = (int)checkEmailCmd.ExecuteScalar();
                if (emailExists > 0)
                {
                    CustomMessengeBox.Show(
                        "Email đã được sử dụng!",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // ✅ Insert tài khoản mới (mặc định User)
                string insertSql = @"INSERT INTO NguoiDung
                                     (TenDangNhap, HoTen, Email, MatKhau, VaiTro)
                                     VALUES (@TenDangNhap,@HoTen,@Email,@MatKhau,'User')";

                SqlCommand insertCmd = new SqlCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@TenDangNhap", txtTenDangNhap.Text.Trim());
                insertCmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                insertCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                insertCmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Password.Trim());

                insertCmd.ExecuteNonQuery();
            }

            CustomMessengeBox.Show(
                "Đăng ký thành công! Vui lòng đăng nhập.",
                "Thành công",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            MainWindow.Instance.Navigate("DangNhap");
        }
    }
}