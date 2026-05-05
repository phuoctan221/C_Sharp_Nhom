using System.Windows;
using System.Windows.Controls;

namespace DoAnNhom
{
    public partial class MenuBar : UserControl
    {
        // Event để MainWindow biết cần chuyển trang
        public static readonly RoutedEvent NavigateRequestedEvent =
            EventManager.RegisterRoutedEvent(
                "NavigateRequested",
                RoutingStrategy.Bubble,
                typeof(NavigateRequestedEventHandler),
                typeof(MenuBar));

        public event NavigateRequestedEventHandler NavigateRequested
        {
            add { AddHandler(NavigateRequestedEvent, value); }
            remove { RemoveHandler(NavigateRequestedEvent, value); }
        }

        public MenuBar()
        {
            InitializeComponent();
        }

        // Lấy MainWindow
        private MainWindow Main
        {
            get { return Application.Current.MainWindow as MainWindow; }
        }

        // Gửi event lên MainWindow
        private void RaiseNavigate(string viewName)
        {
            RaiseEvent(new NavigateRequestedEventArgs(NavigateRequestedEvent, viewName));
        }

        // Các nút click
        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("TrangChu");
        }

        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("TimKiem");
        }

        private void BtnDangTin_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("DangTin");
        }

        private void BtnQuanLyTin_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("QuanLyTin");
        }

        private void BtnYeuThich_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("YeuThich");
        }

        private void BtnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            RaiseNavigate("DangNhap");
        }

        private void BtnDangXuat_Click(object sender, RoutedEventArgs e)
        {
            var result = CustomMessengeBox.Show(
            "Bạn có chắc muốn đăng xuất?",
            "Xác nhận",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow.CurrentUser = null;
                RefreshUser();
                MainWindow.Instance.Navigate("TrangChu");
            }
        }

        public void RefreshUser()
        {
            if (MainWindow.CurrentUser != null)
            {
                txtUser.Text = $"Xin chào, {MainWindow.CurrentUser.TenDangNhap}";
                btnDangNhap.Visibility = Visibility.Collapsed;
                btnDangXuat.Visibility = Visibility.Visible;

                if (MainWindow.CurrentUser.VaiTro == "Admin")
                {
                    btnDangTin.Visibility = Visibility.Visible;
                    btnQuanLyTin.Visibility = Visibility.Visible;
                    btnYeuThich.Visibility = Visibility.Collapsed;
                }
                else // User
                {
                    btnDangTin.Visibility = Visibility.Collapsed;
                    btnQuanLyTin.Visibility = Visibility.Collapsed;
                    btnYeuThich.Visibility = Visibility.Visible;
                }
            }
            else
            {
                txtUser.Text = "";
                btnDangNhap.Visibility = Visibility.Visible;
                btnDangXuat.Visibility = Visibility.Collapsed;

                btnDangTin.Visibility = Visibility.Collapsed;
                btnQuanLyTin.Visibility = Visibility.Collapsed;
                btnYeuThich.Visibility = Visibility.Collapsed;
            }
        }
    }

    public delegate void NavigateRequestedEventHandler(
        object sender, NavigateRequestedEventArgs e);

    public class NavigateRequestedEventArgs : RoutedEventArgs
    {
        public string ViewName { get; }

        public NavigateRequestedEventArgs(RoutedEvent routedEvent, string viewName)
            : base(routedEvent)
        {
            ViewName = viewName;
        }
    }
}