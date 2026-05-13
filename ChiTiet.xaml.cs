using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DoAnNhom.Data;

namespace DoAnNhom.Views
{
    public partial class ChiTiet : Page
    {
        private TinDang _tin;
        private int _currentId = 0;

        private List<string> imageList = new List<string>();
        private int currentIndex = 0;
        private DispatcherTimer timer;

        public ChiTiet()
        {
            InitializeComponent();
            Loaded += ChiTiet_Loaded;
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
            _tin = DatabaseHelper.LayTinTheoId(id);
            if (_tin == null) return;

            txtTieuDe.Text = _tin.TieuDe;
            txtGia.Text = _tin.Loai == "ChoThue"
                ? $"{_tin.Gia:N0} đ/tháng"
                : $"{_tin.Gia:N0} đ";

            txtDienTich.Text = $"• {_tin.DienTich} m²";
            txtLoai.Text = $"{_tin.Loai} - {_tin.LoaiBDS}";
            txtDiaChi.Text = $"{_tin.DiaChi}, {_tin.QuanHuyen}, {_tin.ThanhPho}";
            txtMoTa.Text = string.IsNullOrEmpty(_tin.MoTa)
                ? "Chưa có mô tả."
                : _tin.MoTa;

            txtLoaiBadge.Text = _tin.Loai == "ChoThue"
                ? "CHO THUÊ"
                : "MUA BÁN";

            LoadImages();
        }

        private async void LoadImages()
        {
            imageList.Clear();
            currentIndex = 0;

            if (!string.IsNullOrWhiteSpace(_tin.HinhAnh))
            {
                var arr = _tin.HinhAnh
                    .Replace("\r", "")
                    .Split(new[] { ';', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in arr)
                {
                    string url = item.Trim();
                    if (url.StartsWith("http"))
                        imageList.Add(url);
                }
            }

            if (imageList.Count > 0)
            {
                await ShowImage(imageList[0]);

                if (imageList.Count > 1)
                    StartAutoSlide();
            }
            else
            {
                imgHinh.Source = null;
            }
        }

        private async Task ShowImage(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var bytes = await client.GetByteArrayAsync(url);

                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        bitmap.Freeze();
                    }

                    imgHinh.Source = bitmap;
                }
            }
            catch
            {
                imgHinh.Source = null;
            }
        }

        private void StartAutoSlide()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += async (s, e) =>
            {
                currentIndex++;
                if (currentIndex >= imageList.Count)
                    currentIndex = 0;

                await ShowImage(imageList[currentIndex]);
            };
            timer.Start();
        }

        private void BtnQuayLai_Click(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
            MainWindow.Instance?.NavigateToTrangChu();
        }

        private void BtnYeuThich_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentUser == null) return;

            bool daThich = DatabaseHelper.KiemTraYeuThich(
                MainWindow.CurrentUser.Id,
                _tin.Id);

            if (daThich)
            {
                DatabaseHelper.XoaYeuThich(
                    MainWindow.CurrentUser.Id,
                    _tin.Id);
                btnYeuThich.Foreground = Brushes.White;
            }
            else
            {
                DatabaseHelper.LuuYeuThich(
                    MainWindow.CurrentUser.Id,
                    _tin.Id);
                btnYeuThich.Foreground = Brushes.Red;
            }
        }
    }
}