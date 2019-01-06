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
--exec SP_MonthlyRevenueReport 1, '2008-1-1', '2018-12-31'

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
	@maKS INT OUTPUT,
	@tenKS NVARCHAR(50) OUTPUT,
	@tenNV NVARCHAR(50) OUTPUT)
AS
BEGIN
	--KIỂM TRA TÍNH HỢP LỆ CỦA TÊN ĐĂNG NHẬP VÀ MẬT KHẨU
	SELECT @maKS = maKS, @tenNV = hoTen FROM NhanVien WHERE @tenDangNhap=tenDangNhap AND @matKhau=matKhau
	SELECT @tenKS = tenKS FROM KhachSan WHERE maKS=@maKS
	IF(@maKS > 0)
	BEGIN
		RETURN 1;
	END
	ELSE 
		RAISERROR(N'TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU KHÔNG ĐÚNG',16,1)
		RETURN -1;
END

--DROP PROCEDURE SP_LOGIN_ADMIN
GO

IF object_id('SP_BookingList', 'p') IS NOT NULL
	DROP PROC SP_BookingList

GO

--Xem danh sách đặt phòng đang chờ xác nhận
CREATE PROCEDURE SP_BookingList
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
	@maDP INT,
	@maHoaDon INT OUTPUT)
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
	SELECT @maHoaDon = MAX(maHD) FROM HoaDon
END

GO

--Tìm kiếm thông tin hóa đơn
create  PROC usp_timKiemThongTinHoaDon(@maKS int, @maKH int,@ngayLap datetime, @thanhTien int)
AS
BEGIN	
	create  table tab(
	 tHoTen nvarchar(50),
	 tCMND varchar(10),
	 tSDT varchar(10),
	 tngayBD date,
	 tngayKT date,
	 tmaLoaiphong int,
	 tTongTien int,
	 tNgayThanhToan date)

	DECLARE @hoTen nvarchar(50)
	DECLARE @CMND varchar(10)
	DECLARE @sdt varchar(10)
	declare @maHD int
	declare @ngayBD datetime
	declare @ngayKT datetime
	declare @tongtien int
	declare @ngaythanhtoan datetime

	DECLARE @maDP int SET @maDP=0		
	DECLARE @curmaHD int SET @curmaHD =0
	DECLARE @curmaDP int SET @curmaDP=0
	DECLARE @curtongTien int SET @curtongTien=0
	DECLARE @curngayThanhToan datetime SET @curngayThanhToan=''
	
	--Tìm theo thành tiền
	IF (@thanhTien!=0 and @maKH =0  and @ngayLap is null)
	BEGIN
		DECLARE curTimKiemTheoTongTien CURSOR--lấy các dòng có mã hóa đơn có số tiền tương ứng
		FOR SELECT maHD,maDP,tongTien,ngayThanhToan	FROM HoaDon	WHERE tongTien=@thanhTien
			
		OPEN curTimKiemTheoTongTien
		FETCH NEXT FROM curTimKiemTheoTongTien  INTO @curmaHD, @curmaDP,@curtongTien, @curngayThanhToan
		
		if @maKS = (select maKS from loaiphong where maloaiphong = (select maloaiphong from datphong where maDP=@curmaDP))
		begin
			WHILE (@@FETCH_STATUS=0)
			BEGIN
			--tìm tất Khách hàng có số tiền đó
				SELECT @hoten=KH.hoTen, @CMND=KH.soCMND, @sdt=KH.soDienThoai, @maHD= @curmaHD, @ngayBD=cast(DP.ngayBatDau as date), @ngayKT=cast(DP.ngayTraPhong as date),@tongtien= @curtongTien ,@ngaythanhtoan=cast(@curngayThanhToan as date)
				FROM KhachHang KH join (SELECT	* FROM dbo.DatPhong WHERE maDP=@curmaDP) DP	ON  KH.maKH=DP.maKH
				
				FETCH NEXT FROM curTimKiemTheoTongTien  INTO @curmaHD, @curmaDP,@curtongTien, @curngayThanhToan
				
				insert into tab(thoten,tcmnd, tsdt,tngaybd, tngaykt, ttongtien,tngaythanhtoan)values(@hoten,@cmnd,@sdt, @ngaybd,@ngaykt,@tongtien,@ngaythanhtoan)
			END
		end
		
		CLOSE curTimKiemTheoTongTien
		DEALLOCATE curTimKiemTheoTongTien
	END
	--Tìm theo ngày lập
	ELSE IF (@ngayLap is not null and @thanhTien=0 and @maKH =0)
	BEGIN
		DECLARE curTimKiemTheoNgayLap CURSOR FOR SELECT maHD,maDP,tongTien,ngayThanhToan FROM HoaDon WHERE cast (ngayThanhToan as date)=cast (@ngayLap as date)
			
		OPEN curTimKiemTheoNgayLap
		FETCH NEXT FROM curTimKiemTheoNgayLap  INTO @curmaHD, @curmaDP,@curtongTien, @curngayThanhToan
		
		if @maKS = (select maKS from loaiphong where maloaiphong = (select maloaiphong from datphong where maDP=@curmaDP))
		begin
			WHILE (@@FETCH_STATUS=0)
			BEGIN					
				--tìm tất Khách hàng có ngày lập hóa đơn đó
				SELECT @hoten=KH.hoTen, @cmnd=KH.soCMND, @sdt=KH.soDienThoai, @ngaybd=DP.ngayBatDau, @ngaykt=DP.ngayTraPhong, @tongtien= @curtongTien ,@ngaythanhtoan=@curngayThanhToan
				FROM KhachHang KH JOIN (SELECT * FROM  dbo.DatPhong WHERE maDP=@curmaDP) DP	ON  KH.maKH=DP.maKH
				
				insert into tab(thoten,tcmnd, tsdt,tngaybd, tngaykt, ttongtien,tngaythanhtoan)
				values(@hoten,@cmnd,@sdt, @ngaybd,@ngaykt,@tongtien,@ngaythanhtoan)
				
				FETCH NEXT FROM curTimKiemTheoNgayLap  INTO @curmaHD, @curmaDP,@curtongTien, @curngayThanhToan
			END
		end
		
		CLOSE curTimKiemTheoNgayLap
		DEALLOCATE curTimKiemTheoNgayLap
	END
	--Tìm theo mã KH
	ELSE IF (@maKH <>0 and @ngayLap is null and @thanhTien = 0)
	BEGIN	
		select @hoten=hoten, @CMND=soCMND, @sdt=soDienThoai	from khachhang	where maKh=@makh		
		declare @curngayBD datetime
		declare @curngayKT datetime
		declare @curmaloaiphong int
		
		declare curKHDP cursor	for select ngayBatDau,ngaytraPhong,maloaiphong,maDP	from DatPhong where maKH=@maKH
		
		OPEN curKHDP
		FETCH NEXT FROM curKHDP  INTO @curngaybd, @curngayKT, @curmaloaiphong, @curmaDP
		
		while (@@FETCH_STATUS=0)
		begin
			if @curmaloaiphong in (select maloaiphong from loaiphong where maKs=@maKS)
			begin				
				SELECT @ngaybd=DP.ngayBatDau, @ngaykt=DP.ngayTraPhong, @tongtien=HD.tongTien , @ngaythanhtoan=HD.ngayThanhToan
				FROM (SELECT * FROM	dbo.DatPhong WHERE maDP=@curmaDP) DP join  HoaDon HD ON  HD.maDP=DP.maDP
					
				insert into tab(thoten,tcmnd, tsdt,tngaybd, tngaykt, ttongtien,tngaythanhtoan)
				values(@hoten,@cmnd,@sdt, @ngaybd,@ngaykt,@tongtien,@ngaythanhtoan)
					
			end
			FETCH NEXT FROM curKHDP  INTO  @curngaybd, @curngayKT, @curmaloaiphong, @curmaDP
		end
		
		CLOSE curKHDP
		DEALLOCATE curKHDP
	END
	
	select * from tab
	if OBJECT_ID('dbo.tab', 'u') is  not null drop table tab
END
go

--Liệt kê danh sách các loại phòng tại ks
create proc proc_DSLoaiPhong(@maKS int)
as
begin
	select maloaiphong, tenloaiphong  from Loaiphong where maKs=@maKS
end

GO

--Kiểm tra tình trạng phòng
create proc proc_KTTinhTrangPhong(@maKS int, @maLoaiPhong int, @ngay datetime)
AS
BEGIN
	create  table tab1
	(tabmaLP smallint,
	tabtenLP varchar(30),
	tabmaP smallint,
	tabsoP char(5),
	tabngay datetime,
	tabtinhTrangP nvarchar(15))	
	
	IF (@maLoaiPhong in (SELECT maLoaiPhong from LoaiPhong where maKS=@maKS))
	BEGIN
		DECLARE @tenLoaiPhong varchar(30)
		DECLARE @donGia int 
		DECLARE @moTa nvarchar(255)
		DECLARE @slTrong int  
	
		select @tenLoaiPhong=tenLoaiPhong from LoaiPhong where maLoaiPhong=@maLoaiPhong
		DECLARE @curmaPhong int 
		DECLARE @cursoPhong char(5) 
		
		DECLARE curPhong CURSOR	FOR (SELECT maPhong,soPhong	FROM Phong	WHERE loaiPhong=@maLoaiPhong)		
		OPEN curPhong
		FETCH NEXT FROM curPhong INTO @curmaPhong,@cursoPhong
		
		--trả về đang bảo trì | đang sử dụng | còn trống	
		DECLARE @ngayTinhTrangP datetime 
		DECLARE @tinhTrangP nvarchar(15)	
		WHILE (@@FETCH_STATUS=0)
		BEGIN		
			IF (@ngay is not null or @ngay <>'')
			BEGIN
				set @tinhTrangP=''				
				if exists (select * from TrangThaiPhong where maPhong=@curmaphong)
				begin
					SELECT @ngayTinhTrangP=ngay, @tinhTrangP=tinhTrang FROM TrangThaiPhong	WHERE maPhong=@curmaPhong and cast(ngay as date)=cast(@ngay as date)
					if  (@tinhTrangP<>'')
					begin
						INSERT into tab1(tabmaLP,tabtenLP ,tabmaP ,tabsoP ,tabngay,tabtinhTrangP) VALUES(@maLoaiPhong,@tenLoaiPhong,@curmaPhong,@cursoPhong, @ngayTinhTrangP,@tinhTrangP)
					end
				end
			END			
			FETCH NEXT FROM curPhong INTO @curmaPhong,@cursoPhong
		END		
		CLOSE curPhong
		DEALLOCATE curPhong
	END	
	select * from tab1
	if OBJECT_ID('dbo.tab1', 'u') is  not null	drop table tab1
END
GO


--Chi tiết phòng 
create proc proc_ChiTietPhong(@maKS int, @maLoaiPhong int, @maPhong int, @ngay datetime, @tinhtrang nvarchar(15))
as
begin
	if @maKS in (select maKS from loaiphong where maloaiphong=@maloaiphong)
	begin
		declare @tenloaiphong nvarchar(30)
		declare @dongia int
		declare @mota nvarchar(255)		
		select @tenloaiphong=tenloaiphong, @dongia=dongia, @mota=mota from loaiphong where maKs=@maks and maloaiphong=@maloaiphong
		
		declare @sophong char(5)		
		select @sophong=sophong	from phong where loaiphong=@maloaiphong	
			
		select @tenloaiphong, @sophong, @dongia,@mota, @ngay, @tinhtrang
	end
end
go