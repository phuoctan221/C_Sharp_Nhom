using DoAnNhom.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DoAnNhom
{
    public partial class CardBatDongSan : UserControl
    {
        public TinDang Tin { get; set; }

        public CardBatDongSan()
        {
            InitializeComponent();
        }

        public void SetData(TinDang tin)
        {
            Tin = tin;

            txtTieuDe.Text = tin.TieuDe;
            txtGia.Text = tin.Loai == "ChoThue"
                ? $"{tin.Gia:N0} đ/tháng"
                : $"{tin.Gia:N0} đ";

            txtDienTich.Text = $"{tin.DienTich} m²";
            txtDiaChi.Text = $"{tin.DiaChi}, {tin.QuanHuyen}, {tin.ThanhPho}";

            if (!string.IsNullOrEmpty(tin.HinhAnh))
            {
                var first = tin.HinhAnh.Split(';')[0];
                imgHinh.Source = new BitmapImage(
                    new System.Uri(first, System.UriKind.RelativeOrAbsolute));
            }

            KiemTraTrangThaiYeuThich();
        }

        private void KiemTraTrangThaiYeuThich()
        {
            if (MainWindow.CurrentUser == null || Tin == null)
            {
                btnYeuThich.Foreground = Brushes.White;
                return;
            }

            bool daThich = DatabaseHelper.KiemTraYeuThich(
                MainWindow.CurrentUser.Id,
                Tin.Id);

            btnYeuThich.Foreground = daThich
                ? Brushes.Red
                : Brushes.White;
        }

        private void BtnChiTiet_Click(object sender, RoutedEventArgs e)
        {
            if (Tin == null)
            {
                MessageBox.Show("Tin dữ liệu bị null!",
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MainWindow.Instance?.NavigateToChiTiet(Tin.Id);
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

            bool daThich = DatabaseHelper.KiemTraYeuThich(
                MainWindow.CurrentUser.Id,
                Tin.Id);

            if (daThich)
            {
                DatabaseHelper.XoaYeuThich(
                    MainWindow.CurrentUser.Id,
                    Tin.Id);

                btnYeuThich.Foreground = Brushes.White;
            }
            else
            {
                DatabaseHelper.LuuYeuThich(
                    MainWindow.CurrentUser.Id,
                    Tin.Id);

                btnYeuThich.Foreground = Brushes.Red;
            }
        }
    }
}