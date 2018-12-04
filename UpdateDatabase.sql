USE QUANLYKHACHSAN
GO


UPDATE HoaDon
SET tongTien = (SELECT donGia FROM DatPhong WHERE maDP=HoaDon.maDP)*(SELECT DAY(ngayTraPhong-ngayBatDau) FROM DatPhong WHERE maDP=HoaDon.maDP),
ngayThanhToan= (SELECT ngayTraPhong FROM DatPhong WHERE maDP=HoaDon.maDP)
WHERE maDP>0;

GO

UPDATE KhachSan
SET giaTB = (SELECT AVG(donGia) FROM LoaiPhong WHERE maKS=KhachSan.maKS)
WHERE maKS>0;
