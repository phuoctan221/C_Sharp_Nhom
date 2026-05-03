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
            // Lấy danh sách tin từ database
            var danhSach = DatabaseHelper.LayTatCaTin();

            // Xóa danh sách cũ
            panelCards.Children.Clear();

            // Thêm card vào danh sách
            foreach (var tin in danhSach)
            {
                var card = new CardBatDongSan();
                card.SetData(tin);  // ⭐ QUAN TRỌNG: Phải gọi SetData
                panelCards.Children.Add(card);
            }
        }
    }
}