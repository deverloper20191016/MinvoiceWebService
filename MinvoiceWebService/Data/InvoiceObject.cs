using System.Collections.Generic;

namespace MinvoiceWebService.Data
{
    public class Invoice
    {
        public Master Master { get; set; }
        public List<Detail> Details { get; set; }
    }

    public class Master
    {
        /// <summary>
        /// so_benh_an: Key hóa đơn
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// ma_dt: Mã đối tượng
        /// </summary>
        public string CusCode { get; set; }

        /// <summary>
        /// inv_buyerDisplayName: Tên người mua
        /// </summary>
        public string Buyer { get; set; }

        /// <summary>
        /// inv_buyerLegalName: Tên đơn vị
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// inv_buyerAddressLine: Địa chỉ người mua
        /// </summary>
        public string CusAddress { get; set; }

        /// <summary>
        /// inv_buyerTaxCode: Mã số thuế người mua
        /// </summary>
        public string CusTaxCode { get; set; }

        /// <summary>
        /// inv_paymentMethodName: Hình thức thanh toán
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// inv_invoiceIssuedDate: Ngày hóa đơn
        /// </summary>
        public string ArisingDate { get; set; }

        /// <summary>
        /// inv_TotalAmount: Tổng tiền
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// inv_discountAmount: Tổng tiền chiết khấu
        /// </summary>
        public double DiscountAmount { get; set; }

        /// <summary>
        /// inv_vatAmount: Tổng tiền thuế
        /// </summary>
        public double VatAmount { get; set; }

        /// <summary>
        /// inv_TotalAmountWithoutVat: Tổng tiền trước thuế
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// inv_buyerEmail: Email người mua
        /// </summary>
        public string EmailDeliver { get; set; }

        /// <summary>
        /// inv_buyerBankAccount: Số tài khoản ngân hàng
        /// </summary>
        public string CusBankNo { get; set; }

        /// <summary>
        /// inv_buyerBankName: Tên ngân hàng
        /// </summary>
        public string CusBankName { get; set; }

        /// <summary>
        /// inv_currencyCode: Mã tiền tệ
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// inv_exchangeRate: Tỉ giá
        /// </summary>
        public string ExRate { get; set; }

        /// <summary>
        /// inv_sellerBankAccount: Tài khoản ngân hàng bên bán
        /// </summary>
        public string SellBankNo { get; set; }

        /// <summary>
        /// inv_sellerBankName: Tên ngân hàng bên bán
        /// </summary>
        public string SellBankName { get; set; }

        /// <summary>
        /// inv_invoiceNote: Ghi chú hóa đơn
        /// </summary>
        public string InvNote { get; set; }

        /// <summary>
        /// sovb: Số văn bản xóa bỏ
        /// </summary>
        public string NumberOrDucument { get; set; }

        /// <summary>
        /// ngayvb: Ngày xóa bỏ văn bản
        /// </summary>
        public string DateOrDucument { get; set; }

        /// <summary>
        /// ghi_chu: Ghi chú xóa bỏ hóa đơn
        /// </summary>
        public string NoteOfDocument { get; set; }

        /// <summary>
        /// so_dien_thoai: Số điện thoại
        /// </summary>
        public string Sdt { get; set; }

        /// <summary>
        /// inv_fromWarehouseName: Kho xuất
        /// kho_xuat
        /// </summary>
        public string KhoXuat { get; set; }

        /// <summary>
        /// inv_toWarehouseName: Kho đến
        /// kho_nhap
        /// </summary>
        public string KhoNhap { get; set; }

        /// <summary>
        /// cancu_so: Căn cứ số
        /// </summary>
        public string CanCuSo { get; set; }

        /// <summary>
        /// hopdong_ngay: Hợp đồng ngày
        /// </summary>
        public string HopDongNgay { get; set; }

        /// <summary>
        /// hopdong_cua: Hợp đồng của
        /// </summary>
        public string HopDongCua { get; set; }

        /// <summary>
        /// nguoi_vanchuyen: Người vận chuyển
        /// </summary>
        public string NguoiVanChuyen { get; set; }

        /// <summary>
        /// hopdong: Hợp đồng
        /// </summary>
        public string HopDong { get; set; }

        /// <summary>
        /// phuong_tien: Phương tiện
        /// </summary>
        public string PhuongTien { get; set; }

        /// <summary>
        /// ngay_nhap: Ngày nhập
        /// </summary>
        public string NgayNhap { get; set; }

        /// <summary>
        /// ngay_xuat: Ngày xuất
        /// </summary>
        public string NgayXuat { get; set; }

        /// <summary>
        /// ngay_ct: Ngày chứng từ
        /// </summary>
        public string NgayChungTu { get; set; }

        /// <summary>
        /// lenh_dieu_dong: Lệnh điều động
        /// </summary>
        public string LenhDieuDong { get; set; }

        /// <summary>
        /// phong_ban: Phòng ban
        /// </summary>
        public string PhongBan { get; set; }

        /// <summary>
        /// ve_viec: Về việc
        /// </summary>
        public string VeViec { get; set; }

        /// <summary>
        /// dia_chi_giao_hang: Địa chỉ giao hàng
        /// </summary>
        public string DiaChiGiaoHang { get; set; }

        /// <summary>
        /// hop_dong_kte: Hợp đồng kinh tế
        /// </summary>
        public string HopDongKinhTe { get; set; }

        /// <summary>
        /// voi_tochuc: Với tổ chức
        /// </summary>
        public string ToChuc { get; set; }

        /// <summary>
        /// sign_type: Loại ký: 0. Không, 1. Có, 2.Bỏ cách
        /// </summary>

        public string SignType { get; set; }

        public string VATRate { get; set; }
        public string ThanhTienBangChu { get; set; }

        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
    }

    public class Detail
    {
        /// <summary>
        /// stt_rec0: Số thứ tự
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// inv_itemName: Tên hàng hóa
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// inv_itemCode: Mã hàng hóa
        /// </summary>
        public string ProdCode { get; set; }

        /// <summary>
        /// inv_unitName: Đơn vị tính
        /// </summary>
        public string ProdUnit { get; set; }

        /// <summary>
        /// inv_quantity: Số lượng
        /// </summary>
        public double? ProdQuantity { get; set; }

        /// <summary>
        /// inv_unitPrice: Đơn giá
        /// </summary>
        public double? ProdPrice { get; set; }

        /// <summary>
        /// inv_TotalAmountWithoutVat: Tiền trước thuế
        /// </summary>
        public double? Amount { get; set; }

        /// <summary>
        /// inv_discountAmount: Tiền chiết khấu
        /// </summary>
        public double? DiscountAmount { get; set; }

        /// <summary>
        /// inv_discountPercentage: % Chiết khấu
        /// </summary>
        public double? DiscountRate { get; set; }

        /// <summary>
        /// inv_vatAmount: Tiền thuế
        /// </summary>
        public double? ProdVatAmount { get; set; }

        /// <summary>
        /// ma_thue: Mã thuế
        /// </summary>
        public double? ProdVat { get; set; }

        /// <summary>
        /// inv_promotion: Có khuyến mại
        /// </summary>
        public int Promotion { get; set; }

        /// <summary>
        /// inv_TotalAmount: Thành tiền cuối cùng
        /// </summary>
        public double? TotalAmount { get; set; }

        /// <summary>
        /// sl_thucnhap: Số lượng thực nhập
        /// </summary>
        public double? SoLuongThucNhap { get; set; }

        /// <summary>
        /// sl_thucxuat: Số lượng thực xuất
        /// </summary>
        public double? SoLuongThucXuat { get; set; }

        /// <summary>
        /// ma_lo: Mã lô
        /// </summary>
        public string MaLo { get; set; }

        /// <summary>
        /// ma_mau: Mã màu
        /// </summary>
        public string MaMau { get; set; }

        /// <summary>
        /// han_dung: Hạn sử dụng
        /// </summary>
        public string HanDung { get; set; }

        /// <summary>
        /// noi_san_xuat: Nơi sản xuất
        /// </summary>
        public string NoiSanXuat { get; set; }


        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        /// <summary>
        /// stt1: Số thứ tự
        /// </summary>
        public string No1 { get; set; }

        public string GiamTru { get; set; }
    }

    public class InvoiceCancel
    {
        public string InvSerial { get; set; }
        public string InvPattern { get; set; }
        public string InvNumber { get; set; }
        public string SoVb { get; set; }
        public string NgayVb { get; set; }
        public string GhiChu { get; set; }

    }

    public class Customer
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string BankAccountName { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string RepresentPerson { get; set; }
        public int? CusType { get; set; }
        public string dmdt_id { get; set; }
        public string dt_me_id { get; set; }
    }
}