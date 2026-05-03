using System.Windows;
using DoAnNhom.Views;
using DoAnNhom.Data;

namespace DoAnNhom
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static NguoiDung CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            CurrentUser = null;

            // Cập nhật trạng thái user trên MenuBar
            menuBar.RefreshUser();

            NavigateToTrangChu();
        }

        // Xử lý event từ MenuBar
        private void MenuBar_NavigateRequested(object sender, NavigateRequestedEventArgs e)
        {
            Navigate(e.ViewName);
        }

        public void Navigate(string viewName)
        {
            switch (viewName)
            {
                case "TrangChu":
                    NavigateToTrangChu();
                    break;
                case "TimKiem":
                    NavigateToTimKiem();
                    break;
                case "DangTin":
                    NavigateToDangTin();
                    break;
                case "DangNhap":
                    NavigateToDangNhap();
                    break;
                case "YeuThich":
                    NavigateToYeuThich();
                    break;
                default:
                    NavigateToTrangChu();
                    break;
            }
        }

        public void NavigateToTrangChu() => MainContent.Navigate(new TrangChu());
        public void NavigateToTimKiem() => MainContent.Navigate(new TimKiem());
        public void NavigateToDangTin() => MainContent.Navigate(new DangTin());
        public void NavigateToDangNhap() => MainContent.Navigate(new DangNhap());
        public void NavigateToYeuThich() => MainContent.Navigate(new YeuThich());

        public void NavigateToChiTiet(int bdsId)
        {
            if (bdsId > 0)
                MainContent.Navigate(new ChiTiet(bdsId));
        }

        // Cập nhật MenuBar khi đăng nhập/đăng xuất từ nơi khác
        public void UpdateMenuBar()
        {
            menuBar.RefreshUser();
        }
    }
}