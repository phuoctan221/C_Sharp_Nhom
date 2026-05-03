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
            if (MainWindow.CurrentUser != null)
            {
                MainWindow.CurrentUser = null;
                RefreshUser();
                RaiseNavigate("TrangChu");
            }
        }

        public void RefreshUser()
        {
            if (MainWindow.CurrentUser != null)
            {
                txtUser.Text = "Xin chào, " + MainWindow.CurrentUser.HoTen;
                btnDangNhap.Visibility = Visibility.Collapsed;
                btnDangXuat.Visibility = Visibility.Visible;
            }
            else
            {
                txtUser.Text = "";
                btnDangNhap.Visibility = Visibility.Visible;
                btnDangXuat.Visibility = Visibility.Collapsed;
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