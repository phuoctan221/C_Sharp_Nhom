using DoAnNhom.Data;
using System.Windows.Controls;

namespace DoAnNhom.Views
{
    public partial class TrangChu : Page
    {
        public TrangChu()
        {
            InitializeComponent();
            LoadData();
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