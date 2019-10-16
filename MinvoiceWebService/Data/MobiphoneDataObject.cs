using System;
using System.Collections.Generic;

namespace MinvoiceWebService.Data
{
    public class MobiphoneXmlObject
    {
        public Header Header { get; set; }
        public HoaDonEntity HoaDonEntity { get; set; }
    }

    public class Header
    {
        public string LinkWebService { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }



    public class HoaDonEntity
    {
        public string MaLoaiHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public string dhoadonid { get; set; }
        public DateTime? NgayNhapVien { get; set; }
        public DateTime? NgayTaoHoaDon { get; set; }
        public DateTime? NgayXuatHoaDon { get; set; }
        public string BenBanMaDonVi { get; set; }
        public string BenBanMaSoThue { get; set; }
        public string BenBanTenDonVi { get; set; }
        public string BenBanDiaChi { get; set; }
        public string BenMuaMaSoThue { get; set; }
        public string BenMuaTenDonVi { get; set; }
        public string BenMuaHoTen { get; set; }
        public string BenMuaDiaChi { get; set; }
        public string BenMuaDienThoai { get; set; }
        public string BenMuaEmail { get; set; }
        public double? TienThueVat { get; set; }
        public double? TongTienHang { get; set; }
        public double? TongTienThanhToan { get; set; }
        public double? TienChietKhau { get; set; }
        public string TongTienThanhToanBangChu { get; set; }
        public string DongTienThanhToan { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string TyGia { get; set; }
        public string SoBienLai { get; set; }
        public string TamUng { get; set; }
        public string GhiChu { get; set; }
        public string NamHoc { get; set; }
        public bool IsSign { get; set; }
        public string TrangThaiDieuChinh { get; set; }
       
        public bool isSuDungBangKe { get; set; }
        public string KieuHoaDon { get; set; }
        public string TTamtNoTax { get; set; }
        public string TTvatNoTax { get; set; }
        public string TTnetNoTax { get; set; }
        public string TTamt0Tax { get; set; }
        public string TTvat0Tax { get; set; }
        public string TTnet0Tax { get; set; }
        public string TTamt5Tax { get; set; }
        public string TTvat5Tax { get; set; }
        public string TTnet5Tax { get; set; }
        public string TTamt10Tax { get; set; }
        public string TTvat10Tax { get; set; }
        public string TTnet10Tax { get; set; }
        public bool IsGiuLai { get; set; }

        public List<HangHoaEntity> HangHoaEntities { get; set; }
    }

    public class HangHoaEntity
    {
        public string SoThuTu { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public string DonViTinh { get; set; }
        public double? SoLuong { get; set; }
        public double? DonGia { get; set; }
        public double? ThanhTien { get; set; }
        public double? Vat { get; set; }
        public double? TienVat { get; set; }
        public double? ThanhTienSauThue { get; set; }
        public string HeSoMon { get; set; }
        public string HeSo { get; set; }
        public string DonViPhi { get; set; }
        public string DieuChinh { get; set; }
        public bool khuyenMai { get; set; }
    }

}