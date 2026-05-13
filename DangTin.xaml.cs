using DoAnNhom.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class DangTin : UserControl
    {
        private int? editingId = null;

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

        public DangTin(int id) : this()
        {
            editingId = id;
            txtTitle.Text = "CHỈNH SỬA TIN BẤT ĐỘNG SẢN";
            btnDangTin.Content = "CẬP NHẬT";
            LoadTin(id);
        }

        private void LoadTin(int id)
        {
            var tin = DatabaseHelper.GetTinById(id);
            if (tin == null) return;

            txtTieuDe.Text = tin.TieuDe;
            txtGia.Text = tin.Gia.ToString();
            txtDienTich.Text = tin.DienTich.ToString();
            txtDiaChi.Text = tin.DiaChi;
            txtQuanHuyen.Text = tin.QuanHuyen;
            txtThanhPho.Text = tin.ThanhPho;
            txtMoTa.Text = tin.MoTa;
            txtHinhAnh.Text = tin.HinhAnh;

            foreach (ComboBoxItem item in cbLoai.Items)
            {
                if (item.Content.ToString() == tin.Loai)
                {
                    cbLoai.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in cbLoaiBDS.Items)
            {
                if (item.Content.ToString() == tin.LoaiBDS)
                {
                    cbLoaiBDS.SelectedItem = item;
                    break;
                }
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
                Id = editingId ?? 0,
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

            bool kq;
            if (editingId.HasValue)
                kq = DatabaseHelper.CapNhatTin(tin);
            else
                kq = DatabaseHelper.ThemTin(tin);

            if (kq)
            {
                CustomMessengeBox.Show(
                    editingId.HasValue
                        ? "Cập nhật tin thành công!"
                        : "Đăng tin thành công!",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                MainWindow.Instance.MainContent.Content = new QuanLyTin();
            }
            else
            {
                CustomMessengeBox.Show(
                    "Thao tác thất bại!",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}