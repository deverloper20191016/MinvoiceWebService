using System.Collections.Generic;
using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;

namespace MinvoiceWebService.Converts
{
    public class JsonConvert
    {
        public static JObject CreateJsonMinvoice(DataRequestObject dataRequestObject, Invoice invoice)
        {

            JObject jObject = new JObject
            {
                {"inv_invoiceNumber",dataRequestObject.InvoiceNumber },
                {"inv_invoiceSeries", dataRequestObject.KyHieu},
                {"inv_invoiceIssuedDate", invoice.Master.ArisingDate},
                {"inv_currencyCode" , invoice.Master.Currency},
                {"inv_exchangeRate" , invoice.Master.ExRate},
                {"inv_buyerDisplayName" ,invoice.Master.Buyer},
                {"ma_dt" ,invoice.Master.CusCode},
                {"inv_buyerLegalName" ,invoice.Master.CusName},
                {"inv_buyerTaxCode" ,invoice.Master.CusTaxCode},
                {"inv_buyerAddressLine", invoice.Master.CusAddress},
                {"inv_buyerEmail" ,invoice.Master.EmailDeliver},
                {"inv_buyerBankAccount" ,invoice.Master.CusBankNo},
                {"inv_buyerBankName" ,invoice.Master.CusBankName},
                {"inv_paymentMethodName", invoice.Master.PaymentMethod},
                {"inv_sellerBankAccount" ,invoice.Master.SellBankNo},
                {"inv_sellerBankName",invoice.Master.SellBankName},
                { "mau_hd",dataRequestObject.MauSo},
                {"inv_invoiceNote", invoice.Master.InvNote},
                {"sovb", invoice.Master.NumberOrDucument},
                {"ngayvb",invoice.Master.DateOrDucument },
                {"ghi_chu",invoice.Master.NoteOfDocument },
                {"inv_originalId",  dataRequestObject.InvOriginalId },
                {"inv_adjustmentType",dataRequestObject.TypeOfInvoice },
                {"so_hd_dc",dataRequestObject.InvoiceNumber },
                {"trang_thai_hd",dataRequestObject.TypeOfInvoice },
                {"so_benh_an", invoice.Master.Key },
                {"inv_TotalAmountWithoutVat",invoice.Master.Amount },
                {"inv_vatAmount",invoice.Master.VatAmount },
                {"inv_discountAmount",invoice.Master.DiscountAmount },
                {"inv_TotalAmount",invoice.Master.Total },
                {"inv_fromWarehouseName",invoice.Master.KhoXuat },
                {"inv_toWarehouseName",invoice.Master.KhoNhap },
                {"so_dien_thoai",invoice.Master.Sdt },
                {"cancu_so",invoice.Master.CanCuSo },
                {"hopdong_ngay",invoice.Master.HopDongNgay },
                {"hopdong_cua",invoice.Master.HopDongCua },
                {"nguoi_vanchuyen",invoice.Master.NguoiVanChuyen },
                {"hopdong",invoice.Master.HopDong },
                {"phuong_tien",invoice.Master.PhuongTien },
                {"ngay_nhap",invoice.Master.NgayNhap },
                {"ngay_xuat",invoice.Master.NgayXuat },
                {"ngay_ct",invoice.Master.NgayChungTu },
                {"lenh_dieu_dong",invoice.Master.LenhDieuDong },
                {"phong_ban",invoice.Master.PhongBan },
                {"so_hop_dong",invoice.Master.HopDong },
                {"ve_viec",invoice.Master.VeViec },
                {"nguoi_van_chuyen",invoice.Master.NguoiVanChuyen },
                {"kho_xuat",invoice.Master.KhoXuat },
                {"kho_nhap",invoice.Master.KhoNhap },
                {"dia_chi_giao_hang",invoice.Master.DiaChiGiaoHang },
                {"hop_dong_kte",invoice.Master.HopDongKinhTe },
                {"voi_tochuc",invoice.Master.ToChuc },
                {"inv_vatPercentage",invoice.Master.VATRate },
                {"key_api",invoice.Master.Key },

                {"field1",invoice.Master.Field1 },
                {"field2",invoice.Master.Field2 },
                {"field3",invoice.Master.Field3 },
                {"field4",invoice.Master.Field4 },
                {"field5",invoice.Master.Field5 },
                {"field6",invoice.Master.Field6 },
                {"field7",invoice.Master.Field7 },
                {"field8",invoice.Master.Field8 },
                {"field9",invoice.Master.Field9 },
                {"field10",invoice.Master.Field10 }

            };

            if (dataRequestObject.SignType.HasValue)
            {
                jObject.Add("sign_type", dataRequestObject.SignType.Value.ToString());
            }

            if (!string.IsNullOrEmpty(dataRequestObject.InvInvoiceCodeId))
            {
                jObject.Add("inv_InvoiceCode_id", dataRequestObject.InvInvoiceCodeId);
            }

            JArray jArrayDetails = CreateJarrayDetails(invoice.Details, invoice.Master);
            jObject.Add("details", jArrayDetails);

            JArray jArrayData = new JArray { jObject };

            JObject jObjectInvoice = new JObject
            {
                {"windowid","WIN00187" },
                {"editmode", dataRequestObject.Opt?2:1 },
                {"data",jArrayData }
            };

            return jObjectInvoice;
        }



        private static JArray CreateJarrayDetails(List<Detail> details, Master master)
        {
            JArray jArrayData = new JArray();
            foreach (var detail in details)
            {
                JObject jObjectData = new JObject
                {
                    {"stt_rec0", detail.No},
                    {"inv_itemCode", detail.ProdCode},
                    {"inv_itemName", detail.ProdName},
                    {"inv_unitCode", detail.ProdUnit},
                    {"inv_unitName", detail.ProdUnit},
                    {"inv_unitPrice", detail.ProdPrice},
                    {"inv_quantity", detail.ProdQuantity},
                    {"inv_TotalAmountWithoutVat", detail.Amount},
                    {"inv_vatAmount", detail.ProdVatAmount},
                    {"inv_TotalAmount", detail.TotalAmount},
                    {"inv_promotion", detail.Promotion},
                    {"inv_discountPercentage", detail.DiscountRate},
                    {"inv_discountAmount",detail.DiscountAmount},
                    {"ma_thue",!string.IsNullOrEmpty(detail.ProdVat.ToString()) ? detail.ProdVat.GetValueOrDefault().ToString("n0") : master.VATRate},
                    {"field1",detail.Field1 },
                    {"field2",detail.Field2 },
                    {"field3",detail.Field3 },
                    {"field4",detail.Field4 },
                    {"field5",detail.Field5 },
                    {"field6",detail.Field6 },
                    {"field7",detail.Field7 },
                    {"field8",detail.Field8 },
                    {"field9",detail.Field9 },
                    {"field10",detail.Field10 },
                    {"sl_thucnhap",detail.SoLuongThucNhap },
                    {"sl_thucxuat",detail.SoLuongThucXuat },
                    {"ma_lo",detail.MaLo },
                    {"ma_mau",detail.MaMau },
                    {"han_dung",detail.HanDung },
                    {"noi_san_xuat",detail.NoiSanXuat },
                };
                jArrayData.Add(jObjectData);
            }

            JObject jObject = new JObject
            {
                {"tab_id","TAB00188" },
                {"data",jArrayData }
            };

            JArray jArrayDetails = new JArray
            {
                jObject
            };

            return jArrayDetails;
        }
    }
}