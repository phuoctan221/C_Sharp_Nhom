using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DoAnNhom.Data;

namespace DoAnNhom
{
    public partial class YeuThich : Page
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public YeuThich()
        {
            InitializeComponent();
            this.Loaded += YeuThich_Loaded;
        }

        private void YeuThich_Loaded(object sender, RoutedEventArgs e)
        {
            LoadYeuThich();
        }

        private void LoadYeuThich()
        {
            try
            {
                if (MainWindow.CurrentUser == null)
                {
                    CustomMessengeBox.Show(
                        "Bạn chưa đăng nhập!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                wrapYeuThich.Children.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT t.Id, t.TieuDe, t.Gia,
                               t.DienTich, t.DiaChi,
                               t.QuanHuyen, t.ThanhPho,
                               t.Loai, t.LoaiBDS,
                               t.HinhAnh, t.MoTa,
                               t.NguoiDangId
                        FROM dbo.YeuThich y
                        INNER JOIN dbo.TinDang t
                            ON y.TinDangId = t.Id
                        WHERE y.NguoiDungId = @userId
                        ORDER BY y.Id DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId",
                        MainWindow.CurrentUser.Id);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        TextBlock empty = new TextBlock
                        {
                            Text = "Bạn chưa có tin yêu thích nào.",
                            Foreground = Brushes.White,
                            FontSize = 18,
                            FontWeight = FontWeights.SemiBold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 50, 0, 0)
                        };

                        wrapYeuThich.Children.Add(empty);
                        return;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        TinDang tin = new TinDang
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            TieuDe = row["TieuDe"].ToString(),
                            Gia = Convert.ToDecimal(row["Gia"]),
                            DienTich = Convert.ToDecimal(row["DienTich"]),
                            DiaChi = row["DiaChi"].ToString(),
                            QuanHuyen = row["QuanHuyen"].ToString(),
                            ThanhPho = row["ThanhPho"].ToString(),
                            Loai = row["Loai"].ToString(),
                            LoaiBDS = row["LoaiBDS"].ToString(),
                            HinhAnh = row["HinhAnh"].ToString(),
                            MoTa = row["MoTa"].ToString(),
                            NguoiDangId = Convert.ToInt32(row["NguoiDangId"])
                        };

                        CardBatDongSan card = new CardBatDongSan();
                        card.SetData(tin);

                        wrapYeuThich.Children.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessengeBox.Show(
                    "Lỗi load yêu thích:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}