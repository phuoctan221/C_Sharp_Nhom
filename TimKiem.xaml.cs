using DoAnNhom.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom.Views
{
    public partial class TimKiem : Page
    {
        public TimKiem()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            var danhSach = DatabaseHelper.LayTatCaTin();
            HienThiKetQua(danhSach);
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string tuKhoa = txtTuKhoa.Text.Trim();
            string loai = GetComboBoxValue(cbLoai);
            string loaiBDS = GetComboBoxValue(cbLoaiBDS);
            string thanhPho = txtThanhPho.Text.Trim();

            decimal? giaTu = ParseDecimal(txtGiaTu.Text);
            decimal? giaDen = ParseDecimal(txtGiaDen.Text);
            decimal? dtTu = ParseDecimal(txtDienTichTu.Text);
            decimal? dtDen = ParseDecimal(txtDienTichDen.Text);

            var ketQua = DatabaseHelper.TimKiem(
                tuKhoa, loai, loaiBDS,
                giaTu, giaDen, dtTu, dtDen, thanhPho
            );

            HienThiKetQua(ketQua);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtTuKhoa.Text = "";
            cbLoai.SelectedIndex = 0;
            cbLoaiBDS.SelectedIndex = 0;
            txtGiaTu.Text = "";
            txtGiaDen.Text = "";
            txtDienTichTu.Text = "";
            txtDienTichDen.Text = "";
            txtThanhPho.Text = "";

            LoadAllData();
        }

        private string GetComboBoxValue(ComboBox cb)
        {
            if (cb.SelectedItem is ComboBoxItem item
                && item.Content.ToString() != "Tất cả")
                return item.Content.ToString();
            return "";
        }

        private decimal? ParseDecimal(string text)
        {
            if (decimal.TryParse(text.Trim(), out decimal result))
                return result;
            return null;
        }

        private void HienThiKetQua(List<TinDang> danhSach)
        {
            wrapList.Children.Clear();

            if (danhSach == null || danhSach.Count == 0)
            {
                txtKetQua.Text = "";
                pnlThongBao.Visibility = Visibility.Visible;
                return;
            }

            pnlThongBao.Visibility = Visibility.Collapsed;
            txtKetQua.Text = $"Tìm thấy {danhSach.Count} kết quả";

            foreach (var tin in danhSach)
            {
                var card = new CardBatDongSan();
                card.SetData(tin);
                wrapList.Children.Add(card);
            }
        }
    }
}