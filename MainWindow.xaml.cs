using DoAnNhom.Data;
using DoAnNhom.Views;
using System.Windows;

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
            menuBar.RefreshUser();

            NavigateToTrangChu();
        }

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

                case "DangKy":                 
                    NavigateToDangKy();
                    break;

                case "QuanLyTin":
                    NavigateToQuanLyTin();
                    break;

                case "QuanLyTaiKhoan":
                    NavigateToQuanLyTaiKhoan();
                    break;

                case "YeuThich":
                    NavigateToYeuThich();
                    break;

                default:
                    NavigateToTrangChu();
                    break;
            }
        }


        public void NavigateToTrangChu()
        {
            MainContent.Navigate(new TrangChu());
        }

        public void NavigateToTimKiem()
        {
            MainContent.Navigate(new TimKiem());
        }

        public void NavigateToDangTin()
        {
            MainContent.Navigate(new DangTin());
        }

        public void NavigateToDangNhap()
        {
            MainContent.Navigate(new DangNhap());
        }

        public void NavigateToDangKy()        
        {
            MainContent.Navigate(new DangKy());
        }

        public void NavigateToQuanLyTin()
        {
            MainContent.Navigate(new QuanLyTin());
        }

        public void NavigateToQuanLyTaiKhoan()
        {
            MainContent.Navigate(new QuanLyTaiKhoan());
        }

        public void NavigateToYeuThich()
        {
            MainContent.Navigate(new YeuThich());
        }

        public void NavigateToChiTiet(int bdsId)
        {
            if (bdsId > 0)
                MainContent.Navigate(new ChiTiet(bdsId));
        }

        public void UpdateMenuBar()
        {
            menuBar.RefreshUser();
        }
    }
}