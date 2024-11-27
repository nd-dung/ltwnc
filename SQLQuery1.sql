INSERT INTO [dbo].[tbl_QuyenHan] ( [sTenQuyen])
VALUES ( 'Admin'),
       ( 'User');

INSERT INTO [dbo].[tbl_TaiKhoan] ([sTaiKhoan], [sMatKhau], [iMaQuyen])
VALUES ( 'admin@gmail.com', '123456', 1);
     

INSERT INTO [dbo].[tbl_NguoiDung] ([sTenND], [sEmail], [sSDT], [dNgaySinh], [sGioiTinh], [iMaTaiKhoan])
VALUES ( 'Nguyen Van A', 'nguyenvana@example.com', '0123456789', '1990-01-01', 'Nam', 7)


	  