using System.Windows;

namespace DoAnNhom
{
    public partial class CustomMessengeBox : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;


        public CustomMessengeBox()
        {
            InitializeComponent();
        }

        public CustomMessengeBox(string message, string title,
                                 MessageBoxButton buttons, MessageBoxImage icon)
        {
            InitializeComponent();
            txtTitle.Text = string.IsNullOrWhiteSpace(title) ? "Thông báo" : title;
            txtMessage.Text = message ?? string.Empty;
            ConfigureButtons(buttons);
        }


        public static MessageBoxResult Show(string message)
            => Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.None);

        public static MessageBoxResult Show(string message, string title)
            => Show(message, title, MessageBoxButton.OK, MessageBoxImage.None);


        public static MessageBoxResult Show(string message, string title,
                                            MessageBoxButton buttons)
            => Show(message, title, buttons, MessageBoxImage.None);


        public static MessageBoxResult Show(string message, string title,
                                            MessageBoxButton buttons, MessageBoxImage icon)
        {
            var box = new CustomMessengeBox(message, title, buttons, icon);
            box.ShowDialog();
            return box.Result;
        }


        private void ConfigureButtons(MessageBoxButton buttons)
        {

            btnOK.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnYes.Visibility = Visibility.Collapsed;
            btnNo.Visibility = Visibility.Collapsed;

            switch (buttons)
            {
                case MessageBoxButton.OK:
                    btnOK.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.OKCancel:
                    btnOK.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNo:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNoCancel:
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                default:
                    btnOK.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }
    }
}