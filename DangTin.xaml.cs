using DoAnNhom.Data;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class DangTin : UserControl
    {
        public DangTin()
        {
            InitializeComponent();

            if (MainWindow.CurrentUser == null ||
                MainWindow.CurrentUser.VaiTro != "Admin")
            {
                CustomMessengeBox.Show(
                    "Bạn không có quyền truy cập!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                MainWindow.Instance.Navigate("TrangChu");
            }
        }

        private void BtnDangTin_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtTieuDe.Text) ||
                string.IsNullOrWhiteSpace(txtGia.Text))
            {
                CustomMessengeBox.Show(
                    "Vui lòng nhập tiêu đề và giá.",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                CustomMessengeBox.Show(
                    "Giá không hợp lệ.",
                    "Lỗi dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtDienTich.Text, out decimal dienTich))
            {
                CustomMessengeBox.Show(
                    "Diện tích không hợp lệ.",
                    "Lỗi dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (cbLoai.SelectedItem == null ||
                cbLoaiBDS.SelectedItem == null)
            {
                CustomMessengeBox.Show(
                    "Vui lòng chọn loại tin và loại BĐS.",
                    "Thiếu thông tin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var tin = new TinDang
            {
                TieuDe = txtTieuDe.Text.Trim(),
                Loai = (cbLoai.SelectedItem as ComboBoxItem)?.Content.ToString(),
                LoaiBDS = (cbLoaiBDS.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Gia = gia,
                DienTich = dienTich,
                DiaChi = txtDiaChi.Text.Trim(),
                QuanHuyen = txtQuanHuyen.Text.Trim(),
                ThanhPho = txtThanhPho.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                HinhAnh = txtHinhAnh.Text.Trim(),
                NguoiDangId = MainWindow.CurrentUser.Id
            };

            bool kq = DatabaseHelper.ThemTin(tin);

            if (kq)
            {
                CustomMessengeBox.Show(
                    "Đăng tin thành công!",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                MainWindow.Instance.Navigate("TrangChu");
            }
            else
            {
                CustomMessengeBox.Show(
                    "Đăng tin thất bại!",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}