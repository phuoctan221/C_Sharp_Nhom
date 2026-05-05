using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DoAnNhom.Data;

namespace DoAnNhom.Views
{
    public partial class ChiTiet : Page
    {
        private TinDang _tin;
        private int _currentId = 0;

        public ChiTiet()
        {
            InitializeComponent();
            this.Loaded += ChiTiet_Loaded;
        }

        public ChiTiet(int id) : this()
        {
            _currentId = id;
        }

        private void ChiTiet_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentId > 0)
                LoadChiTiet(_currentId);
        }

        private void LoadChiTiet(int id)
        {
            try
            {
                _tin = DatabaseHelper.LayTinTheoId(id);

                if (_tin == null)
                {
                    CustomMessengeBox.Show(
                        $"Không tìm thấy tin đăng có ID = {id}",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                BindData();
            }
            catch (Exception ex)
            {
                CustomMessengeBox.Show(
                    $"Lỗi tải dữ liệu: {ex.Message}",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BindData()
        {
            txtTieuDe.Text = _tin.TieuDe;

            txtGia.Text = _tin.Loai == "ChoThue"
                ? $"{_tin.Gia:N0} đ/tháng"
                : $"{_tin.Gia:N0} đ";

            txtDienTich.Text = $"• {_tin.DienTich} m²";
            txtLoai.Text = $"{_tin.Loai} - {_tin.LoaiBDS}";
            txtDiaChi.Text = $"{_tin.DiaChi}, {_tin.QuanHuyen}, {_tin.ThanhPho}";

            txtMoTa.Text = string.IsNullOrEmpty(_tin.MoTa)
                ? "Chưa có mô tả chi tiết cho bất động sản này."
                : _tin.MoTa;

            if (!string.IsNullOrEmpty(_tin.HinhAnh))
            {
                try
                {
                    var firstImage = _tin.HinhAnh.Split(
                        new[] { ';' },
                        StringSplitOptions.RemoveEmptyEntries)[0];

                    imgHinh.Source = new BitmapImage(
                        new Uri(firstImage, UriKind.RelativeOrAbsolute));
                }
                catch { }
            }
            CapNhatTrangThaiYeuThich();
        }

        private void CapNhatTrangThaiYeuThich()
        {
            if (MainWindow.CurrentUser == null || _tin == null)
            {
                btnYeuThich.Foreground = Brushes.White;
                return;
            }

            bool daThich = DatabaseHelper.KiemTraYeuThich(
                MainWindow.CurrentUser.Id,
                _tin.Id);

            btnYeuThich.Foreground = daThich
                ? Brushes.Red
                : Brushes.White;
        }

        private void BtnYeuThich_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentUser == null)
            {
                CustomMessengeBox.Show(
                    "Vui lòng đăng nhập để lưu tin yêu thích!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (_tin == null) return;

            bool daThich = DatabaseHelper.KiemTraYeuThich(
                MainWindow.CurrentUser.Id,
                _tin.Id);

            if (daThich)
            {
                DatabaseHelper.XoaYeuThich(
                    MainWindow.CurrentUser.Id,
                    _tin.Id);

                btnYeuThich.Foreground = Brushes.White;

                CustomMessengeBox.Show(
                    "Đã bỏ khỏi yêu thích!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                DatabaseHelper.LuuYeuThich(
                    MainWindow.CurrentUser.Id,
                    _tin.Id);

                btnYeuThich.Foreground = Brushes.Red;

                CustomMessengeBox.Show(
                    "Đã lưu vào yêu thích!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void BtnQuayLai_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.NavigateToTrangChu();
        }
    }
}