using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class QuanLyTin : UserControl
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public QuanLyTin()
        {
            InitializeComponent();
            LoadTin();
        }

        private void LoadTin()
        {
            if (MainWindow.CurrentUser == null)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query;

                if (MainWindow.CurrentUser.VaiTro == "Admin")
                {
                    query = @"SELECT 
                                Id,
                                TieuDe,
                                Loai,
                                Gia,
                                DienTich,
                                LEFT(HinhAnh, CHARINDEX(';', HinhAnh + ';') - 1) AS HinhAnh
                              FROM TinDang";
                }
                else
                {
                    query = @"SELECT 
                                Id,
                                TieuDe,
                                Loai,
                                Gia,
                                DienTich,
                                LEFT(HinhAnh, CHARINDEX(';', HinhAnh + ';') - 1) AS HinhAnh
                              FROM TinDang
                              WHERE NguoiDangId = @userId";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (MainWindow.CurrentUser.VaiTro != "Admin")
                {
                    cmd.Parameters.AddWithValue("@userId",
                        MainWindow.CurrentUser.Id);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgTin.ItemsSource = dt.DefaultView;
            }
        }

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DataRowView row)
            {
                int id = Convert.ToInt32(row["Id"]);
                MainWindow.Instance.NavigateToChiTiet(id);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is DataRowView row)
            {
                int id = Convert.ToInt32(row["Id"]);

                var result = CustomMessengeBox.Show(
                    "Bạn có chắc muốn xóa tin này?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn =
                            new SqlConnection(connectionString))
                        {
                            conn.Open();

                            SqlCommand cmd1 = new SqlCommand(
                                "DELETE FROM YeuThich WHERE TinDangId = @id", conn);
                            cmd1.Parameters.AddWithValue("@id", id);
                            cmd1.ExecuteNonQuery();

                            SqlCommand cmd2 = new SqlCommand(
                                "DELETE FROM TinDang WHERE Id = @id", conn);
                            cmd2.Parameters.AddWithValue("@id", id);
                            cmd2.ExecuteNonQuery();
                        }

                        CustomMessengeBox.Show(
                            "Đã xóa tin thành công!",
                            "Thành công",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        LoadTin();
                    }
                    catch (Exception ex)
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