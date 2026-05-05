Tạo DATABASE vào SqlSever


-- Tạo database
CREATE DATABASE BatDongSanDB;
GO

-- Sử dụng database
USE BatDongSanDB;
GO

-- Bảng NguoiDung
CREATE TABLE NguoiDung (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL,
    SoDienThoai NVARCHAR(15),
    DiaChi NVARCHAR(255),
    NgayTao DATETIME DEFAULT GETDATE()
);

-- Bảng TinDang
CREATE TABLE TinDang (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(255) NOT NULL,
    Loai NVARCHAR(50),
    LoaiBDS NVARCHAR(50),
    Gia DECIMAL(18,2),
    DienTich FLOAT,
    DiaChi NVARCHAR(255),
    QuanHuyen NVARCHAR(100),
    ThanhPho NVARCHAR(100),
    MoTa NVARCHAR(MAX),
    HinhAnh NVARCHAR(255),
    NguoiDangId INT,
    NgayDang DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (NguoiDangId) REFERENCES NguoiDung(Id)
);

-- Bảng YeuThich
CREATE TABLE YeuThich (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NguoiDungId INT,
    TinDangId INT,
    NgayLuu DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (NguoiDungId) REFERENCES NguoiDung(Id),
    FOREIGN KEY (TinDangId) REFERENCES TinDang(Id)
);

-- Unique: mỗi user chỉ thích 1 tin 1 lần
ALTER TABLE YeuThich
ADD CONSTRAINT UQ_YeuThich UNIQUE (NguoiDungId, TinDangId);

/////////////////////////////////////

@Cập nhật thêm database

ALTER TABLE NguoiDung
ADD CONSTRAINT UQ_TenDangNhap UNIQUE (TenDangNhap);

ALTER TABLE NguoiDung
ADD VaiTro NVARCHAR(20) NOT NULL DEFAULT 'User';


///////////////////////////////////
INSERT INTO [BatDongSanDB].[dbo].[TinDang] 
    ([TieuDe], [Loai], [LoaiBDS], [Gia], [DienTich], [DiaChi], [QuanHuyen], [ThanhPho], [MoTa], [HinhAnh], [NguoiDangId], [NgayDang])
VALUES
-- Hà Nội
(N'Căn hộ 2PN view Hồ Tây', N'MuaBan', N'CanHo', 2500000000, 75, N'123 Lạc Long Quân', N'Tây Hồ', N'Hà Nội', N'Căn hộ sang trọng view Hồ Tây, nội thất cao cấp, an ninh 24/7', 'https://picsum.photos/seed/bds1/400/240', 1, GETDATE()),

(N'Nhà phố 4 tầng mặt tiền', N'MuaBan', N'NhaO', 8500000000, 120, N'45 Đội Cấn', N'Ba Đình', N'Hà Nội', N'Nhà phố 4 tầng, mặt tiền rộng 5m, phù hợp kinh doanh', 'https://picsum.photos/seed/bds2/400/240', 1, GETDATE()),

(N'Biệt thự vườn Vinhomes', N'MuaBan', N'BietThu', 15000000000, 300, N'Khu đô thị Vinhomes', N'Nam Từ Liêm', N'Hà Nội', N'Biệt thự vườn cao cấp, hồ bơi riêng, sân vườn rộng', 'https://picsum.photos/seed/bds3/400/240', 1, GETDATE()),

(N'Đất nền dự án Hoài Đức', N'MuaBan', N'Dat', 3200000000, 100, N'KĐT Hoài Đức', N'Hoài Đức', N'Hà Nội', N'Đất nền dự án sổ đỏ, hạ tầng hoàn chỉnh, giá tốt', 'https://picsum.photos/seed/bds4/400/240', 2, GETDATE()),

(N'Cho thuê căn hộ 1PN Cầu Giấy', N'ChoThue', N'CanHo', 10000000, 45, N'78 Trần Thái Tông', N'Cầu Giấy', N'Hà Nội', N'Căn hộ 1PN đầy đủ nội thất, gần trường ĐH, siêu thị', 'https://picsum.photos/seed/bds5/400/240', 2, GETDATE()),

(N'Cho thuê nhà nguyên căn Đống Đa', N'ChoThue', N'NhaO', 25000000, 80, N'12 Tôn Đức Thắng', N'Đống Đa', N'Hà Nội', N'Nhà nguyên căn 3 tầng, 3PN, phù hợp gia đình', 'https://picsum.photos/seed/bds6/400/240', 2, GETDATE()),

-- Hồ Chí Minh
(N'Căn hộ cao cấp Quận 1', N'MuaBan', N'CanHo', 5800000000, 90, N'88 Đinh Tiên Hoàng', N'Quận 1', N'Hồ Chí Minh', N'Căn hộ penthouse view sông Sài Gòn, nội thất dát vàng', 'https://picsum.photos/seed/bds7/400/240', 1, GETDATE()),

(N'Nhà phố Thủ Đức mới xây', N'MuaBan', N'NhaO', 4200000000, 100, N'56 Võ Văn Ngân', N'Thủ Đức', N'Hồ Chí Minh', N'Nhà phố mới xây 5 tầng, gần ĐHQG, tiện kinh doanh', 'https://picsum.photos/seed/bds8/400/240', 1, GETDATE()),

(N'Đất nền Quận 9 giá tốt', N'MuaBan', N'Dat', 2800000000, 120, N'KĐT Long Phước', N'Quận 9', N'Hồ Chí Minh', N'Đất nền sổ hồng riêng, đường 12m, tiện xây nhà ở', 'https://picsum.photos/seed/bds9/400/240', 2, GETDATE()),

(N'Cho thuê căn hộ Vinhomes Central', N'ChoThue', N'CanHo', 20000000, 65, N'208 Nguyễn Hữu Cảnh', N'Bình Thạnh', N'Hồ Chí Minh', N'Căn hộ 2PN Vinhomes Central Park, đầy đủ tiện ích', 'https://picsum.photos/seed/bds10/400/240', 2, GETDATE()),

(N'Biệt thự Phú Mỹ Hưng', N'MuaBan', N'BietThu', 25000000000, 500, N'KĐT Phú Mỹ Hưng', N'Quận 7', N'Hồ Chí Minh', N'Biệt thự siêu sang Phú Mỹ Hưng, 5PN, 3 phòng khách, hồ bơi', 'https://picsum.photos/seed/bds11/400/240', 1, GETDATE()),

(N'Cho thuê văn phòng Quận 3', N'ChoThue', N'NhaO', 45000000, 150, N'30 Lê Quý Đôn', N'Quận 3', N'Hồ Chí Minh', N'Văn phòng mặt tiền đường lớn, thích hợp công ty vừa và nhỏ', 'https://picsum.photos/seed/bds12/400/240', 2, GETDATE()),

-- Đà Nẵng
(N'Nhà phố gần biển Mỹ Khê', N'MuaBan', N'NhaO', 6500000000, 150, N'12 Trường Sa', N'Ngũ Hành Sơn', N'Đà Nẵng', N'Nhà phố cách biển 200m, thích hợp kinh doanh homestay', 'https://picsum.photos/seed/bds13/400/240', 1, GETDATE()),

(N'Căn hộ view biển Đà Nẵng', N'MuaBan', N'CanHo', 3500000000, 68, N'200 Võ Nguyên Giáp', N'Sơn Trà', N'Đà Nẵng', N'Căn hộ view biển trực diện, tặng nội thất cao cấp', 'https://picsum.photos/seed/bds14/400/240', 2, GETDATE()),

(N'Cho thuê nhà nguyên căn Hải Châu', N'ChoThue', N'NhaO', 15000000, 90, N'34 Trần Phú', N'Hải Châu', N'Đà Nẵng', N'Nhà 3 tầng trung tâm thành phố, gần chợ Hàn, Cầu Rồng', 'https://picsum.photos/seed/bds15/400/240', 1, GETDATE()),

(N'Đất nền Liên Chiểu sổ đỏ', N'MuaBan', N'Dat', 1800000000, 200, N'KĐT Liên Chiểu', N'Liên Chiểu', N'Đà Nẵng', N'Đất nền đường 10m, quy hoạch rõ ràng, pháp lý sạch', 'https://picsum.photos/seed/bds16/400/240', 2, GETDATE()),

-- Cần Thơ
(N'Nhà phố mặt tiền Ninh Kiều', N'MuaBan', N'NhaO', 3800000000, 110, N'67 Nguyễn Trãi', N'Ninh Kiều', N'Cần Thơ', N'Nhà phố mặt tiền đường lớn, tiện kinh doanh buôn bán', 'https://picsum.photos/seed/bds17/400/240', 1, GETDATE()),

(N'Cho thuê căn hộ Cần Thơ', N'ChoThue', N'CanHo', 7000000, 50, N'100 Mậu Thân', N'Ninh Kiều', N'Cần Thơ', N'Căn hộ studio, đầy đủ nội thất, gần bến Ninh Kiều', 'https://picsum.photos/seed/bds18/400/240', 2, GETDATE()),

-- Nha Trang
(N'Biệt thự biển Nha Trang', N'MuaBan', N'BietThu', 18000000000, 400, N'KĐT Bắc Vĩnh Hòa', N'Vĩnh Hòa', N'Nha Trang', N'Biệt thự nghỉ dưỡng ven biển, view đại dương, sân vườn rộng', 'https://picsum.photos/seed/bds19/400/240', 1, GETDATE()),

(N'Căn hộ du lịch Nha Trang Center', N'ChoThue', N'CanHo', 12000000, 55, N'20 Trần Phú', N'Lộc Thọ', N'Nha Trang', N'Căn hộ view biển, tiện nghi đầy đủ, phù hợp du lịch ngắn hạn', 'https://picsum.photos/seed/bds20/400/240', 2, GETDATE())