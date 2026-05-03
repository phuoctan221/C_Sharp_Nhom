using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DoAnNhom.Data
{
    public static class DatabaseHelper
    {

        // 🔧 Đổi chuỗi kết nối cho đúng máy bạn
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        // ===== Người dùng =====
        public static NguoiDung DangNhap(string email, string matKhau)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, HoTen, Email, MatKhau, SoDienThoai, DiaChi FROM NguoiDung WHERE Email=@Email AND MatKhau=@MatKhau";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new NguoiDung
                            {
                                Id = reader.GetInt32(0),
                                HoTen = reader.GetString(1),
                                Email = reader.GetString(2),
                                MatKhau = reader.GetString(3),
                                SoDienThoai = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                DiaChi = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static bool DangKy(NguoiDung nd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO NguoiDung (HoTen, Email, MatKhau, SoDienThoai, DiaChi) VALUES (@HoTen,@Email,@MatKhau,@SDT,@DiaChi)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", nd.HoTen);
                    cmd.Parameters.AddWithValue("@Email", nd.Email);
                    cmd.Parameters.AddWithValue("@MatKhau", nd.MatKhau);
                    cmd.Parameters.AddWithValue("@SDT", nd.SoDienThoai ?? "");
                    cmd.Parameters.AddWithValue("@DiaChi", nd.DiaChi ?? "");
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ===== Tin đăng =====
        public static List<TinDang> LayTatCaTin()
        {
            List<TinDang> list = new List<TinDang>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, TieuDe, Loai, LoaiBDS, Gia, DienTich, DiaChi, QuanHuyen, ThanhPho, MoTa, HinhAnh, NguoiDangId, NgayDang FROM TinDang ORDER BY NgayDang DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TinDang
                        {
                            Id = reader.GetInt32(0),
                            TieuDe = reader.GetString(1),
                            Loai = reader.GetString(2),
                            LoaiBDS = reader.GetString(3),
                            Gia = reader.GetDecimal(4),
                            DienTich = reader.GetDecimal(5),
                            DiaChi = reader.IsDBNull(6) ? "" : reader.GetString(6),
                            QuanHuyen = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            ThanhPho = reader.IsDBNull(8) ? "" : reader.GetString(8),
                            MoTa = reader.IsDBNull(9) ? "" : reader.GetString(9),
                            HinhAnh = reader.IsDBNull(10) ? "" : reader.GetString(10),
                            NguoiDangId = reader.GetInt32(11),
                            NgayDang = reader.GetDateTime(12)
                        });
                    }
                }
            }
            return list;
        }

        public static List<TinDang> TimKiem(string tuKhoa, string loai, string loaiBDS, decimal? giaTu, decimal? giaDen, decimal? dtTu, decimal? dtDen, string thanhPho)
        {
            List<TinDang> list = new List<TinDang>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT Id, TieuDe, Loai, LoaiBDS, Gia, DienTich, DiaChi, QuanHuyen, ThanhPho, MoTa, HinhAnh, NguoiDangId, NgayDang
                               FROM TinDang WHERE 1=1 ";
                if (!string.IsNullOrEmpty(tuKhoa))
                    sql += " AND (TieuDe LIKE @TuKhoa OR DiaChi LIKE @TuKhoa OR MoTa LIKE @TuKhoa) ";
                if (!string.IsNullOrEmpty(loai))
                    sql += " AND Loai = @Loai ";
                if (!string.IsNullOrEmpty(loaiBDS))
                    sql += " AND LoaiBDS = @LoaiBDS ";
                if (giaTu.HasValue)
                    sql += " AND Gia >= @GiaTu ";
                if (giaDen.HasValue)
                    sql += " AND Gia <= @GiaDen ";
                if (dtTu.HasValue)
                    sql += " AND DienTich >= @DtTu ";
                if (dtDen.HasValue)
                    sql += " AND DienTich <= @DtDen ";
                if (!string.IsNullOrEmpty(thanhPho))
                    sql += " AND ThanhPho = @ThanhPho ";
                sql += " ORDER BY NgayDang DESC ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(tuKhoa))
                        cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                    if (!string.IsNullOrEmpty(loai))
                        cmd.Parameters.AddWithValue("@Loai", loai);
                    if (!string.IsNullOrEmpty(loaiBDS))
                        cmd.Parameters.AddWithValue("@LoaiBDS", loaiBDS);
                    if (giaTu.HasValue)
                        cmd.Parameters.AddWithValue("@GiaTu", giaTu.Value);
                    if (giaDen.HasValue)
                        cmd.Parameters.AddWithValue("@GiaDen", giaDen.Value);
                    if (dtTu.HasValue)
                        cmd.Parameters.AddWithValue("@DtTu", dtTu.Value);
                    if (dtDen.HasValue)
                        cmd.Parameters.AddWithValue("@DtDen", dtDen.Value);
                    if (!string.IsNullOrEmpty(thanhPho))
                        cmd.Parameters.AddWithValue("@ThanhPho", thanhPho);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TinDang
                            {
                                Id = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                Loai = reader.GetString(2),
                                LoaiBDS = reader.GetString(3),
                                Gia = reader.GetDecimal(4),
                                DienTich = reader.GetDecimal(5),
                                DiaChi = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                QuanHuyen = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                ThanhPho = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                MoTa = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                HinhAnh = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                NguoiDangId = reader.GetInt32(11),
                                NgayDang = reader.GetDateTime(12)
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static TinDang LayTinTheoId(int id)
        {
            if (id <= 0) return null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT Id, TieuDe, Loai, LoaiBDS, Gia, DienTich, DiaChi, 
                                  QuanHuyen, ThanhPho, MoTa, HinhAnh, NguoiDangId, NgayDang 
                           FROM TinDang WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new TinDang
                                {
                                    Id = reader.GetInt32(0),
                                    TieuDe = reader.GetString(1),
                                    Loai = reader.GetString(2),
                                    LoaiBDS = reader.GetString(3),
                                    Gia = reader.GetDecimal(4),
                                    DienTich = reader.GetDecimal(5),
                                    DiaChi = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                    QuanHuyen = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    ThanhPho = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                    MoTa = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                    HinhAnh = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                    NguoiDangId = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                                    NgayDang = reader.GetDateTime(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi DatabaseHelper.LayTinTheoId: " + ex.Message, "Lỗi CSDL");
            }
            return null;
        }

        public static bool ThemTin(TinDang tin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO TinDang (TieuDe, Loai, LoaiBDS, Gia, DienTich, DiaChi, QuanHuyen, ThanhPho, MoTa, HinhAnh, NguoiDangId)
                               VALUES (@TieuDe,@Loai,@LoaiBDS,@Gia,@DienTich,@DiaChi,@QuanHuyen,@ThanhPho,@MoTa,@HinhAnh,@NguoiDangId)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TieuDe", tin.TieuDe);
                    cmd.Parameters.AddWithValue("@Loai", tin.Loai);
                    cmd.Parameters.AddWithValue("@LoaiBDS", tin.LoaiBDS);
                    cmd.Parameters.AddWithValue("@Gia", tin.Gia);
                    cmd.Parameters.AddWithValue("@DienTich", tin.DienTich);
                    cmd.Parameters.AddWithValue("@DiaChi", tin.DiaChi ?? "");
                    cmd.Parameters.AddWithValue("@QuanHuyen", tin.QuanHuyen ?? "");
                    cmd.Parameters.AddWithValue("@ThanhPho", tin.ThanhPho ?? "");
                    cmd.Parameters.AddWithValue("@MoTa", tin.MoTa ?? "");
                    cmd.Parameters.AddWithValue("@HinhAnh", tin.HinhAnh ?? "");
                    cmd.Parameters.AddWithValue("@NguoiDangId", tin.NguoiDangId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<TinDang> LayTinCuaNguoiDung(int nguoiDungId)
        {
            List<TinDang> list = new List<TinDang>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT Id, TieuDe, Loai, LoaiBDS, Gia, DienTich, DiaChi, QuanHuyen, ThanhPho, MoTa, HinhAnh, NguoiDangId, NgayDang
                               FROM TinDang WHERE NguoiDangId=@Id ORDER BY NgayDang DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", nguoiDungId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TinDang
                            {
                                Id = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                Loai = reader.GetString(2),
                                LoaiBDS = reader.GetString(3),
                                Gia = reader.GetDecimal(4),
                                DienTich = reader.GetDecimal(5),
                                DiaChi = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                QuanHuyen = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                ThanhPho = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                MoTa = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                HinhAnh = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                NguoiDangId = reader.GetInt32(11),
                                NgayDang = reader.GetDateTime(12)
                            });
                        }
                    }
                }
            }
            return list;
        }

        // ===== Yêu thích =====
        public static bool LuuYeuThich(int nguoiDungId, int tinDangId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO YeuThich (NguoiDungId, TinDangId) VALUES (@NguoiDungId,@TinDangId)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                    cmd.Parameters.AddWithValue("@TinDangId", tinDangId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool XoaYeuThich(int nguoiDungId, int tinDangId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM YeuThich WHERE NguoiDungId=@NguoiDungId AND TinDangId=@TinDangId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                    cmd.Parameters.AddWithValue("@TinDangId", tinDangId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<TinDang> LayYeuThich(int nguoiDungId)
        {
            List<TinDang> list = new List<TinDang>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT t.Id, t.TieuDe, t.Loai, t.LoaiBDS, t.Gia, t.DienTich, t.DiaChi, t.QuanHuyen, t.ThanhPho, t.MoTa, t.HinhAnh, t.NguoiDangId, t.NgayDang
                               FROM YeuThich y JOIN TinDang t ON y.TinDangId = t.Id
                               WHERE y.NguoiDungId = @NguoiDungId ORDER BY y.NgayLuu DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TinDang
                            {
                                Id = reader.GetInt32(0),
                                TieuDe = reader.GetString(1),
                                Loai = reader.GetString(2),
                                LoaiBDS = reader.GetString(3),
                                Gia = reader.GetDecimal(4),
                                DienTich = reader.GetDecimal(5),
                                DiaChi = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                QuanHuyen = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                ThanhPho = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                MoTa = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                HinhAnh = reader.IsDBNull(10) ? "" : reader.GetString(10),
                                NguoiDangId = reader.GetInt32(11),
                                NgayDang = reader.GetDateTime(12)
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
