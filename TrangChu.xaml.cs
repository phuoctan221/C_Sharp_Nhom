using DoAnNhom.Data;
using System.Windows.Controls;
using System.Diagnostics;

namespace DoAnNhom.Views
{
    public partial class TrangChu : Page
    {
        public TrangChu()
        {
            InitializeComponent();
            LoadData();
        }

        private void OpenInstagram(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.instagram.com",
                UseShellExecute = true
            });
        }
        private void OpenFacebook(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.facebook.com/phuoctannn",
                UseShellExecute = true
            });
        }
        private void LoadData()
        {
            var danhSach = DatabaseHelper.LayTatCaTin();
            panelCards.Children.Clear();

            foreach (var tin in danhSach)
            {
                var card = new CardBatDongSan();
                card.SetData(tin);  
                panelCards.Children.Add(card);
            }
        }
    }
}