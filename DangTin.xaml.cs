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
        }

        private void BtnDangTin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề và giá.");
                return;
            }

            if (!decimal.TryParse(txtGia.Text, out decimal gia))
            {
                MessageBox.Show("Giá không hợp lệ.");
                return;
            }

            if (!decimal.TryParse(txtDienTich.Text, out decimal dienTich))
            {
                MessageBox.Show("Diện tích không hợp lệ.");
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
                MessageBox.Show("Đăng tin thành công!");
                var mw = Window.GetWindow(this) as MainWindow;
                mw?.NavigateToTrangChu();
            }
            else
            {
                MessageBox.Show("Đăng tin thất bại!");
            }
        }
    }
}