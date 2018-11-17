CREATE DATABASE QUANLYKHACHSAN
GO

USE QUANLYKHACHSAN
GO

-------------------------------------------------
--CREATE TABLE

CREATE TABLE KhachHang (
	maKH char(10) NOT NULL,
	hoTen nvarchar(50),--NOT NULL
	tenDangNhap varchar(30) NOT NULL UNIQUE,
	matKhau varchar(30),--NOT NULL
	soCMND varchar(10) NOT NULL, --UNIQUE
	diaChi nvarchar(100),
	soDienThoai varchar(10), --NOT NULL, UNIQUE
	moTa nvarchar(255),
	email varchar(50), --NOT NULL, UNIQUE
	CONSTRAINT PK_khachhang PRIMARY KEY (maKH)
)
GO

CREATE TABLE NhanVien(
	maNV char(10) NOT NULL,
	hoTen nvarchar(50),--NOT NULL
	tenDangNhap varchar(30) NOT NULL UNIQUE,
	matKhau varchar(30),--NOT NULL
	maKS tinyint NOT NULL,
	CONSTRAINT PK_nhanvien PRIMARY KEY (maNV)
)
GO

CREATE TABLE KhachSan(
	maKS tinyint NOT NULL,
	tenKS nvarchar(50) NOT NULL,
	soSao tinyint,--NOT NULL
	soNha varchar(12),--NOT NULL
	duong nvarchar(50),--NOT NULL
	quan nvarchar(20),--NOT NULL
	thanhPho nvarchar(20),--NOT NULL
	giaTB int,
	moTa nvarchar(255),
	CONSTRAINT PK_khachsan PRIMARY KEY (maKS)
)
GO

CREATE TABLE LoaiPhong(
	maLoaiPhong char(10) NOT NULL,
	tenLoaiPhong varchar(20),--NOT NULL
	maKS tinyint NOT NULL,
	donGia int NOT NULL,
	moTa nvarchar(255),
	slTrong smallint NOT NULL,
	CONSTRAINT PK_loaiphong PRIMARY KEY (maLoaiPhong)
)
GO

CREATE TABLE Phong(
	maPhong char(5) NOT NULL,
	loaiPhong char(10) NOT NULL,
	soPhong char(5) NOT NULL,
	CONSTRAINT PK_phong PRIMARY KEY (maPhong)
)
GO

CREATE TABLE TrangThaiPhong(
	maPhong char(5) NOT NULL,
	ngay datetime NOT NULL,
	tinhTrang nvarchar(15) NOT NULL,
	CONSTRAINT PK_trangthaiphong PRIMARY KEY (maPhong, ngay)
)
GO

CREATE TABLE DatPhong(
	maDP char(10) NOT NULL,
	maLoaiPhong char(10) NOT NULL,
	maKH char(10) NOT NULL,
	ngayBatDau datetime,--NOT NULL
	ngayTraPhong datetime,--NOT NULL
	ngayDat datetime,--NOT NULL
	donGia int,--NOT NULL
	moTa nvarchar(255),
	tinhTrang nvarchar(13) NOT NULL,
	CONSTRAINT PK_datphong PRIMARY KEY (maDP)
)
GO

CREATE TABLE HoaDon(
	maHD char(10) NOT NULL,
	ngayThanhToan datetime NOT NULL,
	tongTien int NOT NULL,
	maDP char(10) NOT NULL,--UNIQUE
	CONSTRAINT PK_hoadon PRIMARY KEY (maHD)
)
GO

---------------------------------------------
-- INDEX
/*CREATE INDEX idx_maks
ON LoaiPhong(maKS)

CREATE INDEX idx_maks_dongia
ON LoaiPhong(maKS, donGia)

CREATE INDEX idx_loaiphong
ON Phong(loaiPhong)
*/

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
--CREATE TABLE

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


------------------------------------------
-- PROCEDURE
/*
CREATE PROCEDURE SP_DatPhong @maLoaiPhong char(3), @maKH char(10), @ngayBatDau datetime, @ngayTraPhong datetime
AS
	IF (NOT EXISTS(SELECT * FROM KhachHang WHERE maKH=@maKH))
		RETURN -1
	IF (NOT EXISTS(SELECT * FROM LoaiPhong WHERE maLoaiPhong=@maLoaiPhong))
		RETURN -1
	IF ((SELECT slTrong FROM LoaiPhong WHERE maLoaiPhong=@maLoaiPhong)=0)
		RETURN -1
	DECLARE @maDP int, @ngayDat datetime, @donGia int, @moTa nvarchar(255)
	IF (NOT EXISTS(SELECT * FROM DatPhong))
		SET @maDP = 1
	ELSE
		SET @maDP = (SELECT MAX(maDP) FROM DatPhong) + 1
	SET @ngayDat = GETDATE()
	SET @donGia = (SELECT donGia FROM LoaiPhong WHERE maLoaiPhong=@maLoaiPhong)
	SET @moTa = (SELECT moTa FROM LoaiPhong WHERE maLoaiPhong=@maLoaiPhong)
	INSERT INTO DatPhong(maDP, maLoaiPhong, maKH, ngayBatDau, ngayTraPhong, ngayDat, donGia, moTa, tinhTrang) VALUES (@maDP , @maLoaiPhong, @maKH, @ngayBatDau, @ngayTraPhong, @ngayDat, @donGia, @moTa, N'chưa xác nhận')
	RETURN @maDP
*/