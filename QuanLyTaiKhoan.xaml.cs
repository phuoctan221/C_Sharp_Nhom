using DoAnNhom.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class QuanLyTaiKhoan : Page
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public QuanLyTaiKhoan()
        {
            InitializeComponent();

            if (MainWindow.CurrentUser == null ||
                MainWindow.CurrentUser.VaiTro != "Admin")
            {
                CustomMessengeBox.Show(
                    "Bạn không có quyền truy cập!",
                    "Truy cập bị từ chối",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                MainWindow.Instance.Navigate("TrangChu");
                return;
            }

            LoadUsers();
        }

        private void LoadUsers()
        {
            List<NguoiDung> list = new List<NguoiDung>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM NguoiDung";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NguoiDung
                        {
                            Id = (int)reader["Id"],
                            TenDangNhap = reader["TenDangNhap"].ToString(),
                            HoTen = reader["HoTen"].ToString(),
                            Email = reader["Email"].ToString(),
                            VaiTro = reader["VaiTro"].ToString()
                        });
                    }
                }
            }

            dgUser.ItemsSource = list;
        }

        // ✅ THÊM TÀI KHOẢN → DÙNG CHUNG SuaTaiKhoan
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainContent.Navigate(
                new SuaTaiKhoan(0)); // 0 = thêm mới
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is NguoiDung user)
            {
                MainWindow.Instance.MainContent.Navigate(
                    new SuaTaiKhoan(user.Id));
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is NguoiDung user)
            {
                // ✅ Không cho xóa chính mình
                if (user.Id == MainWindow.CurrentUser.Id)
                {
                    CustomMessengeBox.Show(
                        "Bạn không thể xóa chính mình!",
                        "Không hợp lệ",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // ✅ Không cho xóa Admin chính (Id = 1 ví dụ)
                if (user.VaiTro == "Admin" && user.Id == 1)
                {
                    CustomMessengeBox.Show(
                        "Không thể xóa Admin chính của hệ thống!",
                        "Không hợp lệ",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var result = CustomMessengeBox.Show(
                    $"Bạn có chắc muốn xóa tài khoản '{user.TenDangNhap}'?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string sql = "DELETE FROM NguoiDung WHERE Id=@Id";
                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@Id", user.Id);
                            cmd.ExecuteNonQuery();
                        }

                        CustomMessengeBox.Show(
                            "Xóa tài khoản thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        LoadUsers();
                    }
                    catch (System.Exception ex)
                    {
                        CustomMessengeBox.Show(
                            "Lỗi khi xóa:\n" + ex.Message,
                            "Lỗi",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}