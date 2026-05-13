using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class SuaTaiKhoan : Page
    {
        private int userId; // 0 = thêm mới
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public SuaTaiKhoan(int id = 0)
        {
            InitializeComponent();
            userId = id;

            if (userId == 0)
            {
                txtTitle.Text = "THÊM TÀI KHOẢN";
                btnAction.Content = "THÊM";
            }
            else
            {
                txtTitle.Text = "SỬA TÀI KHOẢN";
                btnAction.Content = "CẬP NHẬT";
                LoadUser();
            }
        }

        private void LoadUser()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM NguoiDung WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtTenDangNhap.Text = reader["TenDangNhap"].ToString();
                    txtHoTen.Text = reader["HoTen"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtSoDienThoai.Text = reader["SoDienThoai"].ToString();
                    txtDiaChi.Text = reader["DiaChi"].ToString();

                    foreach (ComboBoxItem item in cbVaiTro.Items)
                    {
                        if (item.Content.ToString() ==
                            reader["VaiTro"].ToString())
                        {
                            cbVaiTro.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cbVaiTro.SelectedItem == null)
            {
                CustomMessengeBox.Show(
                    "Vui lòng nhập đầy đủ thông tin!",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (userId == 0)
                    {
                        string insertSql = @"INSERT INTO NguoiDung
                                             (TenDangNhap, HoTen, Email, MatKhau, SoDienThoai, DiaChi, VaiTro)
                                             VALUES (@TenDangNhap,@HoTen,@Email,@MatKhau,@SoDienThoai,@DiaChi,@VaiTro)";

                        SqlCommand cmd = new SqlCommand(insertSql, conn);
                        cmd.Parameters.AddWithValue("@TenDangNhap", txtTenDangNhap.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@MatKhau", txtMatKhau.Password.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@VaiTro",
                            (cbVaiTro.SelectedItem as ComboBoxItem).Content.ToString());

                        cmd.ExecuteNonQuery();

                        CustomMessengeBox.Show(
                            "Thêm tài khoản thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        string updateSql = @"UPDATE NguoiDung SET
                                             TenDangNhap=@TenDangNhap,
                                             HoTen=@HoTen,
                                             Email=@Email,
                                             SoDienThoai=@SoDienThoai,
                                             DiaChi=@DiaChi,
                                             VaiTro=@VaiTro
                                             WHERE Id=@Id";

                        SqlCommand cmd = new SqlCommand(updateSql, conn);
                        cmd.Parameters.AddWithValue("@TenDangNhap", txtTenDangNhap.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@VaiTro",
                            (cbVaiTro.SelectedItem as ComboBoxItem).Content.ToString());
                        cmd.Parameters.AddWithValue("@Id", userId);

                        cmd.ExecuteNonQuery();

                        CustomMessengeBox.Show(
                            "Cập nhật tài khoản thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }

                MainWindow.Instance.Navigate("QuanLyTaiKhoan");
            }
            catch (System.Exception ex)
            {
                CustomMessengeBox.Show(
                    "Lỗi:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}