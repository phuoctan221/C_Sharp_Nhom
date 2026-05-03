using DoAnNhom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            txtGia.Text = tin.Loai == "ChoThue" ? $"{tin.Gia:N0} đ/tháng" : $"{tin.Gia:N0} đ";
            txtDienTich.Text = $"{tin.DienTich} m²";
            txtDiaChi.Text = $"{tin.DiaChi}, {tin.QuanHuyen}, {tin.ThanhPho}";

            if (!string.IsNullOrEmpty(tin.HinhAnh))
            {
                var first = tin.HinhAnh.Split(';')[0];
                imgHinh.Source = new BitmapImage(new System.Uri(first, System.UriKind.RelativeOrAbsolute));
            }
        }

        private void BtnChiTiet_Click(object sender, RoutedEventArgs e)
        {
            if (Tin == null)
            {
                MessageBox.Show("Tin dữ liệu bị null!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MainWindow.Instance == null)
            {
                MessageBox.Show("Không tìm thấy MainWindow Instance!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow.Instance.NavigateToChiTiet(Tin.Id);
        }

        private void BtnYeuThich_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentUser == null)
            {
                CustomMessengeBox.Show("Vui lòng đăng nhập để lưu tin yêu thích!", "Thông báo",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (DatabaseHelper.LuuYeuThich(MainWindow.CurrentUser.Id, Tin.Id))
                CustomMessengeBox.Show("Đã lưu vào yêu thích!", "Thông báo",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
            else
                CustomMessengeBox.Show("Tin này đã có trong yêu thích!", "Thông báo",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
