using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace DoAnNhom.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["BatDongSanConn"].ConnectionString;

        public static NguoiDung DangNhap(string email, string matKhau)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT Id, HoTen, Email, MatKhau, 
                               SoDienThoai, DiaChi, VaiTro
                               FROM NguoiDung
                               WHERE Email=@Email AND MatKhau=@MatKhau";

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
                                DiaChi = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                VaiTro = reader.IsDBNull(6) ? "User" : reader.GetString(6)
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

                string sql = @"INSERT INTO NguoiDung
                               (HoTen, Email, MatKhau, SoDienThoai, DiaChi, VaiTro)
                               VALUES (@HoTen,@Email,@MatKhau,@SDT,@DiaChi,@VaiTro)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@HoTen", nd.HoTen);
                    cmd.Parameters.AddWithValue("@Email", nd.Email);
                    cmd.Parameters.AddWithValue("@MatKhau", nd.MatKhau);
                    cmd.Parameters.AddWithValue("@SDT", nd.SoDienThoai ?? "");
                    cmd.Parameters.AddWithValue("@DiaChi", nd.DiaChi ?? "");
                    cmd.Parameters.AddWithValue("@VaiTro", nd.VaiTro ?? "User");

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<TinDang> LayTatCaTin()
        {
            List<TinDang> list = new List<TinDang>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM TinDang
                               ORDER BY NgayDang DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(DocTin(reader));
                    }
                }
            }

            return list;
        }

        public static TinDang GetTinById(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM TinDang WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return DocTin(reader);
                    }
                }
            }

            return null;
        }

        public static TinDang LayTinTheoId(int id)
        {
            return GetTinById(id);
        }

        public static bool ThemTin(TinDang tin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO TinDang
                               (TieuDe, Loai, LoaiBDS, Gia, DienTich,
                                DiaChi, QuanHuyen, ThanhPho, MoTa,
                                HinhAnh, NguoiDangId)
                               VALUES
                               (@TieuDe,@Loai,@LoaiBDS,@Gia,@DienTich,
                                @DiaChi,@QuanHuyen,@ThanhPho,@MoTa,
                                @HinhAnh,@NguoiDangId)";

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

        public static bool CapNhatTin(TinDang tin)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE TinDang SET
                               TieuDe=@TieuDe,
                               Loai=@Loai,
                               LoaiBDS=@LoaiBDS,
                               Gia=@Gia,
                               DienTich=@DienTich,
                               DiaChi=@DiaChi,
                               QuanHuyen=@QuanHuyen,
                               ThanhPho=@ThanhPho,
                               MoTa=@MoTa,
                               HinhAnh=@HinhAnh
                               WHERE Id=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", tin.Id);
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

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool LuuYeuThich(int userId, int tinId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO YeuThich (NguoiDungId, TinDangId)
                               VALUES (@UserId,@TinId)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TinId", tinId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static void XoaYeuThich(int userId, int tinId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"DELETE FROM YeuThich
                               WHERE NguoiDungId=@UserId
                               AND TinDangId=@TinId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TinId", tinId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool KiemTraYeuThich(int userId, int tinId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT COUNT(*) FROM YeuThich
                               WHERE NguoiDungId=@UserId
                               AND TinDangId=@TinId";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TinId", tinId);

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        private static TinDang DocTin(SqlDataReader reader)
        {
            return new TinDang
            {
                Id = (int)reader["Id"],
                TieuDe = reader["TieuDe"].ToString(),
                Loai = reader["Loai"].ToString(),
                LoaiBDS = reader["LoaiBDS"].ToString(),
                Gia = (decimal)reader["Gia"],
                DienTich = (decimal)reader["DienTich"],
                DiaChi = reader["DiaChi"]?.ToString(),
                QuanHuyen = reader["QuanHuyen"]?.ToString(),
                ThanhPho = reader["ThanhPho"]?.ToString(),
                MoTa = reader["MoTa"]?.ToString(),
                HinhAnh = reader["HinhAnh"]?.ToString(),
                NguoiDangId = (int)reader["NguoiDangId"],
                NgayDang = (DateTime)reader["NgayDang"]
            };
        }
        public static List<TinDang> TimKiem(
            string tuKhoa,
            string loai,
            string loaiBDS,
            decimal? giaTu,
            decimal? giaDen,
            decimal? dtTu,
            decimal? dtDen,
            string thanhPho)
        {
            List<TinDang> list = new List<TinDang>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT * FROM TinDang WHERE 1=1 ";

                if (!string.IsNullOrEmpty(tuKhoa))
                    sql += " AND (TieuDe LIKE @TuKhoa OR DiaChi LIKE @TuKhoa OR MoTa LIKE @TuKhoa)";

                if (!string.IsNullOrEmpty(loai))
                    sql += " AND Loai = @Loai";

                if (!string.IsNullOrEmpty(loaiBDS))
                    sql += " AND LoaiBDS = @LoaiBDS";

                if (giaTu.HasValue)
                    sql += " AND Gia >= @GiaTu";

                if (giaDen.HasValue)
                    sql += " AND Gia <= @GiaDen";

                if (dtTu.HasValue)
                    sql += " AND DienTich >= @DtTu";

                if (dtDen.HasValue)
                    sql += " AND DienTich <= @DtDen";

                if (!string.IsNullOrEmpty(thanhPho))
                    sql += " AND ThanhPho LIKE @ThanhPho";

                sql += " ORDER BY NgayDang DESC";

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
                        cmd.Parameters.AddWithValue("@ThanhPho", "%" + thanhPho + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new TinDang
                            {
                                Id = (int)reader["Id"],
                                TieuDe = reader["TieuDe"].ToString(),
                                Loai = reader["Loai"].ToString(),
                                LoaiBDS = reader["LoaiBDS"].ToString(),
                                Gia = (decimal)reader["Gia"],
                                DienTich = (decimal)reader["DienTich"],
                                DiaChi = reader["DiaChi"]?.ToString(),
                                QuanHuyen = reader["QuanHuyen"]?.ToString(),
                                ThanhPho = reader["ThanhPho"]?.ToString(),
                                MoTa = reader["MoTa"]?.ToString(),
                                HinhAnh = reader["HinhAnh"]?.ToString(),
                                NguoiDangId = (int)reader["NguoiDangId"],
                                NgayDang = (DateTime)reader["NgayDang"]
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}