using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class Contact : Page
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public Contact()
        {
            InitializeComponent();
        }

        private void BtnGui_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtNoiDung.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO LienHe 
                               (HoTen, Email, SoDienThoai, NoiDung)
                               VALUES (@HoTen, @Email, @SDT, @NoiDung)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@SDT", txtSoDienThoai.Text.Trim());
                cmd.Parameters.AddWithValue("@NoiDung", txtNoiDung.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Gửi liên hệ thành công!");

            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            txtNoiDung.Clear();
        }
    }
}