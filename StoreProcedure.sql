USE QUANLYKHACHSAN
GO

-- Thong ke doanh thu theo thang
CREATE PROCEDURE SP_MonthlyRevenueReport(
	@hotel TINYINT,
	@dateBegin DATE,
	@dateEnd DATE)
AS
BEGIN
	IF (@dateBegin IS NOT NULL AND @dateEnd IS NOT NULL)
	BEGIN
		SELECT Year(hd.ngayThanhToan) AS N'Năm', MONTH(hd.ngayThanhToan) AS N'Tháng', SUM(CONVERT(BIGINT, hd.tongTien)) AS N'Doanh thu'
		FROM ((HoaDon hd JOIN DatPhong dp ON hd.maDP=dp.maDP)
			JOIN LoaiPhong lp ON dp.maLoaiPhong=lp.maLoaiPhong)
			JOIN KhachSan ks ON lp.maKS=ks.maKS
		WHERE ks.maKS = @hotel AND (hd.ngayThanhToan BETWEEN @dateBegin AND @dateEnd)
		GROUP BY MONTH(hd.ngayThanhToan), Year(hd.ngayThanhToan)
		ORDER BY Year(hd.ngayThanhToan), MONTH(hd.ngayThanhToan)
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END

--drop procedure SP_MonthlyRevenueReport
--exec SP_MonthlyRevenueReport 84, '2017-1-1', '2018-12-31'

GO

-- Thong ke doanh thu theo nam
CREATE PROCEDURE SP_YearRevenueReport(
	@hotel TINYINT,
	@dateBegin DATE,
	@dateEnd DATE)
AS
BEGIN
	IF (@dateBegin IS NOT NULL AND @dateEnd IS NOT NULL)
	BEGIN
		SELECT Year(hd.ngayThanhToan) AS N'Năm', SUM(CONVERT(BIGINT, hd.tongTien)) AS N'Doanh thu'
		FROM ((HoaDon hd JOIN DatPhong dp ON hd.maDP=dp.maDP)
			JOIN LoaiPhong lp ON dp.maLoaiPhong=lp.maLoaiPhong)
			JOIN KhachSan ks ON lp.maKS=ks.maKS
		WHERE ks.maKS = @hotel AND (hd.ngayThanhToan BETWEEN @dateBegin AND @dateEnd)
		GROUP BY Year(hd.ngayThanhToan)
		ORDER BY Year(hd.ngayThanhToan)
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END

--exec SP_YearRevenueReport 84, '2017-1-1', '2018-12-31'

GO

-- Thong ke doanh thu theo tung loai phong
CREATE PROCEDURE SP_RoomRevenueReport(
	@hotel TINYINT,
	@dateBegin DATE,
	@dateEnd DATE)
AS
BEGIN
	IF (@dateBegin IS NOT NULL AND @dateEnd IS NOT NULL)
	BEGIN
		SELECT lp.maLoaiPhong AS N'Mã loại phòng', lp.tenLoaiPhong AS N'Loại phòng', SUM(CONVERT(BIGINT, hd.tongTien)) AS N'Doanh thu'
		FROM ((HoaDon hd JOIN DatPhong dp ON hd.maDP=dp.maDP)
			JOIN LoaiPhong lp ON dp.maLoaiPhong=lp.maLoaiPhong)
			JOIN KhachSan ks ON lp.maKS=ks.maKS
		WHERE ks.maKS = @hotel AND (hd.ngayThanhToan BETWEEN '2018-1-1' AND '2018-12-31')
		GROUP BY lp.maLoaiPhong, lp.tenLoaiPhong
		ORDER BY lp.tenLoaiPhong
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END

--exec SP_RoomRevenueReport 84, '2017-1-1', '2018-12-31'

GO

-- Thong ke tinh trang phong
CREATE PROCEDURE SP_StatusRoomStatistic(
	@hotel TINYINT,
	@dateBegin DATE,
	@dateEnd DATE,
	@minOfDay INT)
AS
BEGIN
	IF (@dateBegin IS NOT NULL AND @dateEnd IS NOT NULL AND @minOfDay IS NOT NULL)
	BEGIN
		SELECT p.maPhong AS N'Mã phòng', COUNT(p.maPhong) AS N'Số ngày bảo trì'
		FROM (TrangThaiPhong ttp JOIN Phong p ON ttp.maPhong=p.maPhong)
			JOIN LoaiPhong lp ON p.loaiPhong=lp.maLoaiPhong
		WHERE lp.maKS = @hotel AND ttp.tinhTrang LIKE N'đang bảo trì' AND
			(ttp.ngay BETWEEN @dateBegin AND @dateEnd)
		GROUP BY p.maPhong
		HAVING COUNT(p.maPhong)>=@minOfDay
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END
--EXEC SP_StatusRoomStatistic 84, '2008-1-1', '2018-12-31', 15

GO

-- Thong ke so luong phong trong theo tung loai phong
CREATE PROCEDURE SP_EmptyRoomStatistics(
	@hotel TINYINT,
	@dateBegin DATE,
	@dateEnd DATE)
AS
BEGIN
	IF (@dateBegin IS NOT NULL AND @dateEnd IS NOT NULL)
	BEGIN
		SELECT lp.maLoaiPhong AS N'Mã loại phòng', COUNT(lp.maLoaiPhong) AS N'Số phòng trống'
		FROM (TrangThaiPhong ttp JOIN Phong p ON ttp.maPhong=p.maPhong)
			JOIN LoaiPhong lp ON p.loaiPhong=lp.maLoaiPhong
		WHERE lp.maKS = 84 AND ttp.tinhTrang LIKE N'còn trống' AND
			(ttp.ngay BETWEEN @dateBegin AND @dateEnd)
		GROUP BY lp.maLoaiPhong
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END
--EXEC SP_EmptyRoomStatistics 84, '2018-1-1', '2018-1-31'

GO

create procedure sp_LogIn_Admin(
	@tenDangNhap nvarchar(30),
	@matKhau nvarchar(30),
	@maKS int=0 output)
as
begin
	--Kiểm tra tên đăng nhập không được null?
	if(len(@tenDangNhap)=0)
	begin
		raiserror(N'Tên đăng nhập không được phép rỗng',16,1)
		return 0;
	end	
	--Kiểm tra mật khẩu không được null
	if( len(@matKhau)=0)
	begin 
		raiserror(N'Mật khẩu không được phép rỗng',16,1)
		return 0;
	end
	--Kiểm tra tính hợp lệ của tên đăng nhập và mật khẩu
	select @maKS=maKS from NhanVien where @tenDangNhap=tenDangNhap and @matKhau=matKhau
	if(@maKS > 0)
	begin
		return @maKS;
	end
	else 
		raiserror(N'Tên đăng nhập hoặc mật khẩu không đúng',16,1)
		return 0;
end

--drop procedure sp_LogIn_Admin

