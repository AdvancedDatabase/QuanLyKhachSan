--USE QUANLYKHACHSAN
USE QUANLYKHACHSAN_INDEX
GO


IF object_id('SP_MonthlyRevenueReport', 'p') IS NOT NULL
	DROP PROC SP_MonthlyRevenueReport

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
--exec SP_MonthlyRevenueReport 1, '2018-1-1', '2018-12-31'

GO


IF object_id('SP_YearRevenueReport', 'p') IS NOT NULL
	DROP PROC SP_YearRevenueReport

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

--exec SP_YearRevenueReport 1, '1/1/2017', '12/31/2018'
GO


IF object_id('SP_RoomRevenueReport', 'p') IS NOT NULL
	DROP PROC SP_RoomRevenueReport

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
		WHERE ks.maKS = @hotel AND (hd.ngayThanhToan BETWEEN @dateBegin AND @dateEnd)
		GROUP BY lp.maLoaiPhong, lp.tenLoaiPhong
		ORDER BY lp.maLoaiPhong, lp.tenLoaiPhong
	END
	ELSE
	BEGIN 
		RAISERROR(N'Vui lòng điền đầy đủ thông tin!',16,1)
		RETURN 0
	END
END

--drop procedure SP_RoomRevenueReport
--exec SP_RoomRevenueReport 84, '2017-1-1', '2018-12-31'

GO

IF object_id('SP_StatusRoomStatistic', 'p') IS NOT NULL
	DROP PROC SP_StatusRoomStatistic

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

IF object_id('SP_EmptyRoomStatistics', 'p') IS NOT NULL
	DROP PROC SP_EmptyRoomStatistics

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

IF object_id('SP_LogIn_Admin', 'p') IS NOT NULL
	DROP PROC sp_LogIn_Admin

GO

CREATE PROCEDURE SP_LogIn_Admin(
	@tenDangNhap NVARCHAR(30),
	@matKhau NVARCHAR(30),
	@maKS INT=0 OUTPUT)
AS
BEGIN
	--KIỂM TRA TÊN ĐĂNG NHẬP KHÔNG ĐƯỢC NULL?
	IF(LEN(@tenDangNhap)=0)
	BEGIN
		RAISERROR(N'TÊN ĐĂNG NHẬP KHÔNG ĐƯỢC PHÉP RỖNG',16,1)
		RETURN 0;
	END	
	--KIỂM TRA MẬT KHẨU KHÔNG ĐƯỢC NULL
	IF( LEN(@matKhau)=0)
	BEGIN 
		RAISERROR(N'MẬT KHẨU KHÔNG ĐƯỢC PHÉP RỖNG',16,1)
		RETURN 0;
	END
	--KIỂM TRA TÍNH HỢP LỆ CỦA TÊN ĐĂNG NHẬP VÀ MẬT KHẨU
	SELECT @maKS=MAKS FROM NHANVIEN WHERE @tenDangNhap=TENDANGNHAP AND @matKhau=MATKHAU
	IF(@maKS > 0)
	BEGIN
		RETURN @maKS;
	END
	ELSE 
		RAISERROR(N'TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU KHÔNG ĐÚNG',16,1)
		RETURN 0;
END

--DROP PROCEDURE SP_LOGIN_ADMIN


IF object_id('SP_BOOKINGLIST', 'p') IS NOT NULL
	DROP PROC SP_BOOKINGLIST

GO

--Xem danh sách đặt phòng đang chờ xác nhận
CREATE PROCEDURE SP_BOOKINGLIST
	@MAKS INT
AS
BEGIN
	SELECT DP.MADP AS N'Mã đặt phòng', KH.hoTen AS N'Người đặt', P.maPhong AS N'Mã phòng', P.soPhong AS N'Số phòng', DP.NGAYBATDAU AS N'Ngày bắt đầu', DP.NGAYTRAPHONG AS N'Ngày trả phòng', DP.NGAYDAT AS N'Ngày đặt', DP.DONGIA AS N'Đơn giá (đồng)', DP.TINHTRANG AS N'Tình trạng'
	FROM ((DatPhong DP JOIN LoaiPhong LP ON DP.maLoaiPhong=LP.maLoaiPhong) 
		JOIN KhachHang KH ON KH.maKH=DP.maKH)
		JOIN Phong P ON P.maPhong = DP.maPhong
	WHERE UPPER(DP.TINHTRANG) LIKE N'CHƯA XÁC NHẬN' AND LP.maKS=@MAKS
END

GO

IF object_id('SP_ConfirmBooking', 'p') IS NOT NULL
	DROP PROC SP_ConfirmBooking

GO

-- Xác nhận đặt phòng
CREATE PROCEDURE SP_ConfirmBooking(
	@maDP INT)
AS
BEGIN
	UPDATE DatPhong
	SET tinhTrang=N'Đã xác nhận'
	WHERE maDP=@maDP
END

GO

IF object_id('SP_UnpaidList', 'p') IS NOT NULL
	DROP PROC SP_UnpaidList
GO
--Xem danh sách đặt phòng chờ thanh toán
CREATE PROCEDURE SP_UnpaidList
	@MAKS INT
AS
BEGIN
	SELECT DP.MADP AS N'Mã đặt phòng', KH.hoTen AS N'Người đặt', P.maPhong AS N'Mã phòng', P.soPhong AS N'Số phòng', DP.NGAYBATDAU AS N'Ngày bắt đầu', DP.NGAYTRAPHONG AS N'Ngày trả phòng', DP.NGAYDAT AS N'Ngày đặt', DP.DONGIA AS N'Đơn giá (đồng)', DP.TINHTRANG AS N'Tình trạng'
	FROM ((DatPhong DP JOIN LoaiPhong LP ON DP.maLoaiPhong=LP.maLoaiPhong) 
		JOIN KhachHang KH ON KH.maKH=DP.maKH)
		JOIN Phong P ON P.maPhong = DP.maPhong
	WHERE UPPER(DP.tinhTrang) LIKE N'ĐÃ XÁC NHẬN' AND LP.maKS=@MAKS AND DP.maDP NOT IN (
		SELECT maDP FROM HoaDon)
END

GO	

IF object_id('SP_Payment', 'p') IS NOT NULL
	DROP PROC SP_Payment
GO
CREATE PROCEDURE SP_Payment(
	@maDP INT)
AS
BEGIN
	DECLARE @ngayThanhToan DATETIME
	DECLARE @tongTien INT
	DECLARE @ngaybd DATETIME
	DECLARE @maPhong INT
	DECLARE @maLoaiPhong INT
	DECLARE @donGia INT = 0

	SELECT @ngaybd=DP.ngayBatDau FROM DatPhong DP WHERE DP.maDP=@maDP 
	--Lấy ngày thanh toán bằng ngày trả phòng
	SELECT @ngayThanhToan=DP.ngayTraPhong FROM DatPhong DP WHERE DP.maDP=@maDP
	--Lấy mã phòng của phòng đã đặt
	SELECT @maPhong=DP.maPhong FROM DatPhong DP	WHERE DP.maDP=@maDP
	--Lấy mã loại phòng đã đặt
	SET @maLoaiPhong=(SELECT P.loaiPhong FROM Phong P WHERE @maPhong=maPhong)
	--Lấy đơn giá của phòng đã đặt
	SELECT @donGia=LP.donGia FROM LoaiPhong LP WHERE LP.maLoaiPhong = @maLoaiPhong
	--SET trạng thái của phòng đã đặt thành trống
	UPDATE TrangThaiPhong SET tinhTrang=N'còn trống' WHERE maPhong=@maPhong
	--Tăng số lượng phòng trống của loại phòng đã đặt lên 1
	UPDATE LoaiPhong SET slTrong = slTrong + 1 WHERE maLoaiPhong=@maLoaiPhong
	--Tính tổng tiền
	SET @tongtien=@donGia * (DATEDIFF(day,@ngaybd,@ngayThanhToan))

	IF(exists( SELECT* FROM DatPhong DP WHERE DP.maDP=@maDP and DP.tinhTrang=N'Đã xác nhận'))
	BEGIN
		INSERT INTO HoaDon VALUES(@ngayThanhToan, @tongTien, @maDP)
	END
END

GO

/*
select * 
from DatPhong dp join LoaiPhong lp on dp.maLoaiPhong=lp.maLoaiPhong
where year(ngayBatDau)=2018 and lp.maKS=1 and not exists (
	select * from HoaDon hd where dp.maDP=hd.maDP)

exec SP_ConfirmBooking 1801
exec SP_Payment 1801

select * from NhanVien where tenDangNhap like 'TerranceJacobs%'
select * from HoaDon where maDP=1801

SELECT Year(hd.ngayThanhToan) AS N'Năm', SUM(CONVERT(BIGINT, hd.tongTien)) AS N'Doanh thu'
		FROM ((HoaDon hd JOIN DatPhong dp ON hd.maDP=dp.maDP)
			JOIN LoaiPhong lp ON dp.maLoaiPhong=lp.maLoaiPhong)
			JOIN KhachSan ks ON lp.maKS=ks.maKS
		WHERE ks.maKS = 1 AND (hd.ngayThanhToan BETWEEN '2018-1-1' AND '2018-12-31')
		GROUP BY hd.ngayThanhToan
*/