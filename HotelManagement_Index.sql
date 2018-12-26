CREATE DATABASE QUANLYKHACHSAN_INDEX
GO

USE QUANLYKHACHSAN_INDEX
GO

-------------------------------------------------
--CREATE TABLE

CREATE TABLE KhachHang (
	maKH int				NOT NULL IDENTITY(1,1),
	hoTen nvarchar(50)		NOT NULL,
	tenDangNhap varchar(30) NOT NULL UNIQUE,
	matKhau varchar(30)		NOT NULL,
	soCMND varchar(10)		NOT NULL UNIQUE,
	diaChi nvarchar(100),
	soDienThoai varchar(10) NOT NULL UNIQUE,
	moTa nvarchar(255),
	email varchar(50)		NOT NULL UNIQUE,
	CONSTRAINT PK_khachhang PRIMARY KEY (maKH)
)
GO

CREATE TABLE NhanVien(
	maNV int				NOT NULL IDENTITY(1,1),
	hoTen nvarchar(50)		NOT NULL,
	tenDangNhap varchar(30) NOT NULL UNIQUE,
	matKhau varchar(30)		NOT NULL,
	maKS tinyint			NOT NULL,
	CONSTRAINT PK_nhanvien PRIMARY KEY (maNV)
)
GO

CREATE TABLE KhachSan(
	maKS tinyint			NOT NULL IDENTITY(1,1),
	tenKS nvarchar(50)		NOT NULL,
	soSao tinyint			NOT NULL,
	soNha varchar(12)		NOT NULL,
	duong nvarchar(50)		NOT NULL,
	quan nvarchar(20)		NOT NULL,
	thanhPho nvarchar(20)	NOT NULL,
	giaTB int,
	moTa nvarchar(255),
	CONSTRAINT PK_khachsan PRIMARY KEY (maKS)
)
GO

CREATE TABLE LoaiPhong(
	maLoaiPhong smallint	NOT NULL IDENTITY(1,1),
	tenLoaiPhong nvarchar(30) NOT NULL,
	maKS tinyint			NOT NULL,
	donGia int				NOT NULL,
	moTa nvarchar(255),
	slTrong smallint		NOT NULL,
	CONSTRAINT PK_loaiphong PRIMARY KEY (maLoaiPhong)
)
GO

CREATE TABLE Phong(
	maPhong smallint		NOT NULL IDENTITY(1,1),
	loaiPhong smallint		NOT NULL,
	soPhong char(5)			NOT NULL,
	CONSTRAINT PK_phong PRIMARY KEY (maPhong)
)
GO

CREATE TABLE TrangThaiPhong(
	maPhong smallint		NOT NULL,
	ngay datetime			NOT NULL,
	tinhTrang nvarchar(15)	NOT NULL,
	CONSTRAINT PK_trangthaiphong PRIMARY KEY (maPhong, ngay)
)
GO

CREATE TABLE DatPhong(
	maDP int				NOT NULL IDENTITY(1,1),
	maLoaiPhong smallint	NOT NULL,
	maPhong smallint		NOT NULL,
	maKH int				NOT NULL,
	ngayBatDau datetime		NOT NULL,
	ngayTraPhong datetime	NOT NULL,
	ngayDat datetime		NOT NULL,
	donGia int				NOT NULL,
	moTa nvarchar(255),
	tinhTrang nvarchar(13)	NOT NULL,
	CONSTRAINT PK_datphong PRIMARY KEY (maDP)
)
GO

CREATE TABLE HoaDon(
	maHD int				NOT NULL IDENTITY(1,1),
	ngayThanhToan datetime	NOT NULL,
	tongTien int			NOT NULL,
	maDP int				NOT NULL UNIQUE,
	CONSTRAINT PK_hoadon PRIMARY KEY (maHD)
)
GO


----------------------------------------------
-- FOREIGN KEY

ALTER TABLE LoaiPhong 
ADD CONSTRAINT FK_loaiphong_khachsan
FOREIGN KEY (maKS)
REFERENCES KhachSan(maKS)
GO

ALTER TABLE NhanVien 
ADD CONSTRAINT FK_nhanvien_khachsan
FOREIGN KEY (maKS)
REFERENCES KhachSan(maKS)
GO

ALTER TABLE Phong 
ADD CONSTRAINT FK_phong_loaiphong
FOREIGN KEY (loaiPhong)
REFERENCES LoaiPhong(maLoaiPhong)
GO

ALTER TABLE TrangThaiPhong 
ADD CONSTRAINT FK_trangthaiphong_phong
FOREIGN KEY (maPhong)
REFERENCES Phong(maPhong)
GO

ALTER TABLE DatPhong 
ADD CONSTRAINT FK_datphong_loaiphong
FOREIGN KEY (maLoaiPhong)
REFERENCES LoaiPhong(maLoaiPhong)
GO

ALTER TABLE DatPhong 
ADD CONSTRAINT FK_datphong_maphong
FOREIGN KEY (maPhong)
REFERENCES Phong(maPhong)
GO

ALTER TABLE DatPhong 
ADD CONSTRAINT FK_datphong_khachhang
FOREIGN KEY (maKH)
REFERENCES KhachHang(maKH)
GO

ALTER TABLE HoaDon 
ADD CONSTRAINT FK_hoadon_datphong
FOREIGN KEY (maDP)
REFERENCES DatPhong(maDP)
GO

-------------------------------------------------
--CREATE CHECK

ALTER TABLE TrangThaiPhong
ADD CONSTRAINT CHK_tinhtrangTTP
CHECK (tinhtrang in (N'đang sử dụng', N'đang bảo trì', N'còn trống'))
GO

ALTER TABLE DatPhong
ADD CONSTRAINT CHK_tinhtrangDatPhong
CHECK (tinhtrang in (N'đã xác nhận', N'chưa xác nhận'))
GO

ALTER TABLE DatPhong
ADD CONSTRAINT CHK_ngaytraphong
CHECK (ngayTraPhong > ngayBatDau)
GO

ALTER TABLE DatPhong
ADD CONSTRAINT CHK_ngaydat
CHECK (ngayDat <= ngayBatDau)
GO

ALTER TABLE KhachSan
ADD CONSTRAINT CHK_sosao
CHECK (soSao>=1 AND soSao<=5)
GO


---------------------------------------------
-- INDEX
CREATE INDEX idx_loaiphong_maks
ON LoaiPhong(maKS);

CREATE INDEX idx_loaiphong_maks_dongia
ON LoaiPhong(maKS, donGia);

CREATE INDEX idx_phong_loaiphong
ON Phong(loaiPhong);

CREATE INDEX idx_khachhang_tendangnhap_matkhau
ON KhachHang(tenDangNhap, matKhau);

CREATE INDEX idx_nhanvien_tendangnhap_matkhau
ON NhanVien(tenDangNhap, matKhau);

CREATE INDEX idx_khachsan_thanhpho
ON KhachSan(thanhPho);

CREATE INDEX idx_trangthaiphong_maphong
ON TrangThaiPhong(maPhong);

CREATE INDEX idx_datphong_maphong
ON DatPhong(maPhong);

CREATE INDEX idx_datphong_maKH
ON DatPhong(maKH);

CREATE INDEX idx_hoadon_maDP_tongtien
ON HoaDon(maDP, tongTien);

select count(*) from KhachHang
select count(*) from NhanVien
select count(*) from HoaDon
select count(*) from DatPhong
select count(*) from TrangThaiPhong
