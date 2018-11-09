

CREATE DATABASE QUANLYKHACHSAN
GO

USE QUANLYKHACHSAN
GO


-------------------------------------------------
--CREATE TABLE

CREATE TABLE KhachHang (
	maKH char(10),
	hoTen nvarchar(50),
	tenDangNhap varchar(30),
	matKhau varchar(30),
	soCMND varchar(10),
	diaChi nvarchar(100),
	soDienThoai varchar(10),
	moTa nvarchar(255),
	email varchar(50),
	CONSTRAINT PK_khachhang PRIMARY KEY (maKH)
)
GO

CREATE TABLE NhanVien(
	maNV char(10),
	hoTen nvarchar(50),
	tenDangNhap varchar(30),
	matKhau varchar(30),
	maKS tinyint,
	CONSTRAINT PK_nhanvien PRIMARY KEY (maNV)
)
GO

CREATE TABLE KhachSan(
	maKS tinyint,
	tenKS nvarchar(50),
	soSao tinyint,
	soNha varchar(12),
	duong nvarchar(50),
	quan nvarchar(20),
	thanhPho nvarchar(20),
	giaTB int,
	moTa nvarchar(255),
	CONSTRAINT PK_khachsan PRIMARY KEY (maKS)
)
GO

CREATE TABLE LoaiPhong(
	maLoaiPhong char(10),--
	tenLoaiPhong varchar(20),
	maKS tinyint,
	donGia int,
	moTa nvarchar(255),
	slTrong smallint,
	CONSTRAINT PK_loaiphong PRIMARY KEY (maLoaiPhong)
)
GO

CREATE TABLE Phong(
	maPhong char(5),
	loaiPhong char(10),
	soPhong char(5),
	CONSTRAINT PK_phong PRIMARY KEY (maPhong)
)
GO

CREATE TABLE TrangThaiPhong(
	maPhong char(5),
	ngay datetime,
	tinhTrang nvarchar(15),
	CONSTRAINT PK_trangthaiphong PRIMARY KEY (maPhong, ngay)
)
GO

CREATE TABLE DatPhong(
	maDP char(10),
	maLoaiPhong char(10),
	maKH char(10),
	ngayBatDau datetime,
	ngayTraPhong datetime,
	ngayDat datetime,
	donGia int,
	moTa nvarchar(255),
	tinhTrang nvarchar(13),
	CONSTRAINT PK_datphong PRIMARY KEY (maDP)
)
GO

CREATE TABLE HoaDon(
	maHD char(10),
	ngayThanhToan datetime,
	tongTien int,
	maDP char(10),
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