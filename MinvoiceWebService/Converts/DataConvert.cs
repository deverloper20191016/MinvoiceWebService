using MinvoiceWebService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MinvoiceWebService.Converts
{
    public class DataConvert
    {
        private static XmlDocument _xmlDocument;

        public static List<Invoice> GetListInvoiceByXml(string xml)
        {
            xml = xml.Replace("&", "&amp;");

            List<Invoice> invoices = new List<Invoice>();
            _xmlDocument = new XmlDocument();
            try
            {
                _xmlDocument.LoadXml(xml);
                XmlNodeList xmlNodeListInvoice = _xmlDocument.SelectNodes("Invoices/Invoice");
                if (xmlNodeListInvoice != null)
                {
                    foreach (XmlNode xmlNodeInvoice in xmlNodeListInvoice)
                    {
                        Invoice invoice = GetInvocie(xmlNodeInvoice);
                        invoices.Add(invoice);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return invoices;
        }

        public static Invoice GetInvocie(XmlNode xmlNodeInvoice)
        {
            Invoice invoice = new Invoice();
            _xmlDocument = new XmlDocument();
            try
            {
                XmlNode xmlNodeMaster = xmlNodeInvoice.SelectSingleNode("Master");
                XmlNodeList xmlNodeListDetails = xmlNodeInvoice.SelectNodes("Details/Detail");

                Master master = GetMaster(xmlNodeMaster);
                List<Detail> details = GetDetails(xmlNodeListDetails);

                if (master.Total <= 0 && master.Amount <= 0 && master.VatAmount <= 0)
                {
                    master.Amount = 0;
                    master.VatAmount = 0;
                    master.DiscountAmount = 0;
                    master.Total = 0;

                    foreach (Detail detail in details)
                    {
                        master.Amount += detail.Amount.GetValueOrDefault(0);
                        master.VatAmount += detail.ProdVatAmount.GetValueOrDefault(0);
                        master.DiscountAmount += detail.DiscountAmount.GetValueOrDefault(0);
                        master.Total += detail.TotalAmount.GetValueOrDefault(0);
                    }
                }



                invoice.Master = master;
                invoice.Details = details;
                return invoice;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private static Master GetMaster(XmlNode xmlNodeMaster)
        {
            Master master = new Master
            {
                InvInvoiceCodeId = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("InvoiceCodeId")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("InvoiceCodeId")?.InnerText
                    : null,

                VATRate = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("VATRate")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("VATRate")?.InnerText
                    : null,
                Key = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Key")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("Key")?.InnerText
                    : null,
                CusCode = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusCode")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("CusCode")?.InnerText
                    : null,
                Buyer = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Buyer")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("Buyer")?.InnerText
                    : null,
                CusName = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusName")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("CusName")?.InnerText
                    : null,
                CusAddress = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusAddress")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("CusAddress")?.InnerText
                    : null,
                CusTaxCode = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusTaxCode")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("CusTaxCode")?.InnerText
                    : null,
                PaymentMethod = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("PaymentMethod")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("PaymentMethod")?.InnerText
                    : "Tiền mặt/Chuyển khoản",
                ArisingDate = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("ArisingDate")?.InnerText)
                    ? xmlNodeMaster.SelectSingleNode("ArisingDate")?.InnerText
                    : null
            };
            string totalText = xmlNodeMaster.SelectSingleNode("Total")?.InnerText;
            if (totalText != null)
            {
                master.Total = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Total")?.InnerText)
                    ? double.Parse(totalText)
                    : 0;
            }
            string discountAmountText = xmlNodeMaster.SelectSingleNode("DiscountAmount")?.InnerText;
            if (discountAmountText != null)
            {
                master.DiscountAmount =
                    !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("DiscountAmount")?.InnerText)
                        ? double.Parse(discountAmountText)
                        : 0;
            }

            string vatAmountText = xmlNodeMaster.SelectSingleNode("VatAmount")?.InnerText;
            if (vatAmountText != null)
            {
                master.VatAmount = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("VatAmount")?.InnerText)
                    ? double.Parse(vatAmountText)
                    : 0;
            }
            string amountText = xmlNodeMaster.SelectSingleNode("Amount")?.InnerText;
            if (amountText != null)
            {
                master.Amount = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Amount")?.InnerText)
                    ? double.Parse(amountText)
                    : 0;
            }
            master.EmailDeliver = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("EmailDeliver")?.InnerText) ? xmlNodeMaster
                    .SelectSingleNode("EmailDeliver")?.InnerText : null;
            master.CusBankNo = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusBankNo")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("CusBankNo")?.InnerText : null;
            master.CusBankName = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CusBankName")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("CusBankName")?.InnerText : null;
            master.Currency = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Currency")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Currency")?.InnerText : "VND";
            master.ExRate = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("ExRate")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("ExRate")?.InnerText : "1";
            master.SellBankNo = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("SellBankNo")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("SellBankNo")?.InnerText : null;
            master.SellBankName = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("SellBankName")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("SellBankName")?.InnerText : null;
            master.InvNote = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("InvNote")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("InvNote")?.InnerText : null;
            master.NumberOrDucument = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NumberOrDucument")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NumberOrDucument")?.InnerText : null;
            master.DateOrDucument = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("DateOrDucument")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("DateOrDucument")?.InnerText : null;
            master.NoteOfDocument = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NoteOfDocument")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NoteOfDocument")?.InnerText : null;
            master.Sdt = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("SoDienThoai")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("SoDienThoai")?.InnerText : null;

            master.NgayChungTu = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NgayChungTu")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NgayChungTu")?.InnerText : null;

            master.LenhDieuDong = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("LenhDieuDong")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("LenhDieuDong")?.InnerText : null;

            master.PhongBan = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("PhongBan")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("PhongBan")?.InnerText : null;

            master.KhoXuat = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("KhoXuat")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("KhoXuat")?.InnerText : null;

            master.KhoNhap = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("KhoNhap")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("KhoNhap")?.InnerText : null;

            master.DiaChiGiaoHang = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("DiaChiGiaoHang")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("DiaChiGiaoHang")?.InnerText : null;

            master.HopDongKinhTe = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("HopDongKinhTe")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("HopDongKinhTe")?.InnerText : null;

            master.CanCuSo = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("CanCuSo")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("CanCuSo")?.InnerText : null;

            master.HopDongNgay = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("HopDongNgay")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("HopDongNgay")?.InnerText : null;

            master.HopDongCua = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("HopDongCua")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("HopDongCua")?.InnerText : null;

            master.NguoiVanChuyen = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NguoiVanChuyen")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NguoiVanChuyen")?.InnerText : null;

            master.HopDong = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("HopDong")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("HopDong")?.InnerText : null;

            master.VeViec = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("VeViec")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("VeViec")?.InnerText : null;

            master.PhuongTien = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("PhuongTien")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("PhuongTien")?.InnerText : null;

            master.NgayNhap = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NgayNhap")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NgayNhap")?.InnerText : null;

            master.NgayXuat = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("NgayXuat")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("NgayXuat")?.InnerText : null;

            master.ToChuc = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("ToChuc")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("ToChuc")?.InnerText : null;

            master.ThanhTienBangChu = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("ThanhTienBangChu")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("ThanhTienBangChu")?.InnerText : null;

            master.Field1 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field1")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field1")?.InnerText : null;
            master.Field2 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field2")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field2")?.InnerText : null;
            master.Field3 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field3")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field3")?.InnerText : null;
            master.Field4 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field4")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field4")?.InnerText : null;
            master.Field5 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field5")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field5")?.InnerText : null;
            master.Field6 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field6")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field6")?.InnerText : null;
            master.Field7 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field7")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field7")?.InnerText : null;
            master.Field8 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field8")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field8")?.InnerText : null;
            master.Field9 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field9")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field9")?.InnerText : null;
            master.Field10 = !string.IsNullOrEmpty(xmlNodeMaster.SelectSingleNode("Field10")?.InnerText) ? xmlNodeMaster
                .SelectSingleNode("Field10")?.InnerText : null;

            return master;
        }

        private static List<Detail> GetDetails(XmlNodeList xmlNodeListDetails)
        {
            List<Detail> details = new List<Detail>();
            foreach (XmlNode xmlNodeListDetail in xmlNodeListDetails)
            {
                Detail detail = new Detail
                {
                    No = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("No")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("No")?.InnerText
                        : null,
                    ProdName = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("ProdName")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("ProdName")?.InnerText
                        : null,
                    ProdCode = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("ProdCode")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("ProdCode")?.InnerText
                        : null,
                    ProdUnit = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("ProdUnit")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("ProdUnit")?.InnerText
                        : null,
                    Field1 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field1")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field1")?.InnerText
                        : null,
                    Field2 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field2")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field2")?.InnerText
                        : null,
                    Field3 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field3")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field3")?.InnerText
                        : null,
                    Field4 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field4")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field4")?.InnerText
                        : null,
                    Field5 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field5")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field5")?.InnerText
                        : null,
                    Field6 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field6")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field6")?.InnerText
                        : null,
                    Field7 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field7")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field7")?.InnerText
                        : null,
                    Field8 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field8")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field8")?.InnerText
                        : null,
                    Field9 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field9")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("ProdUnit")?.InnerText
                        : null,
                    Field10 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("Field10")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("Field10")?.InnerText
                        : null,
                    MaLo = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("MaLo")?.InnerText)
                    ? xmlNodeListDetail.SelectSingleNode("MaLo")?.InnerText
                    : null,
                    MaMau = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("MaMau")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("MaMau")?.InnerText
                        : null,
                    HanDung = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("HanDung")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("HanDung")?.InnerText
                        : null,
                    NoiSanXuat = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("NoiSanXuat")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("NoiSanXuat")?.InnerText
                        : null,
                    No1 = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("No1")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("No1")?.InnerText
                        : null,
                    GiamTru = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("GiamTru")?.InnerText)
                        ? xmlNodeListDetail.SelectSingleNode("GiamTru")?.InnerText
                        : null,
                    LoaiDieuChinh = !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("LoaiDieuChinh")?.InnerText)
                        ? int.Parse(xmlNodeListDetail.SelectSingleNode("LoaiDieuChinh")?.InnerText)
                        : (int?)null,
                };


                string prodQuantityText = xmlNodeListDetail.SelectSingleNode("ProdQuantity")?.InnerText;
                if (prodQuantityText != null)
                {
                    detail.ProdQuantity =
                          !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("ProdQuantity")?.InnerText)
                              ? double.Parse(prodQuantityText)
                              : (double?)null;
                }

                string soLuongThucNhapText = xmlNodeListDetail.SelectSingleNode("SoLuongThucNhap")?.InnerText;
                if (soLuongThucNhapText != null)
                {
                    detail.SoLuongThucNhap =
                        !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("SoLuongThucNhap")?.InnerText)
                            ? double.Parse(soLuongThucNhapText)
                            : (double?)null;
                }

                string soLuongThucXuatText = xmlNodeListDetail.SelectSingleNode("SoLuongThucXuat")?.InnerText;
                if (soLuongThucXuatText != null)
                {
                    detail.SoLuongThucXuat =
                        !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("SoLuongThucXuat")?.InnerText)
                            ? double.Parse(soLuongThucXuatText)
                            : (double?)null;
                }



                string prodPriceText = xmlNodeListDetail.SelectSingleNode("ProdPrice")?.InnerText;
                if (prodPriceText != null)
                {
                    detail.ProdPrice =
                        !string.IsNullOrEmpty(xmlNodeListDetail.SelectSingleNode("ProdPrice")?.InnerText)
                            ? double.Parse(prodPriceText)
                            : (double?)null;
                }
                string amountText = xmlNodeListDetail.SelectSingleNode("Amount")?.InnerText;
                if (amountText != null)
                {
                    detail.Amount = !string.IsNullOrEmpty(amountText)
                          ? double.Parse(amountText)
                          : (double?)null;
                }
                string discountAmountText = xmlNodeListDetail.SelectSingleNode("DiscountAmount")?.InnerText;
                if (discountAmountText != null)
                {
                    detail.DiscountAmount =
                          !string.IsNullOrEmpty(discountAmountText)
                              ? double.Parse(discountAmountText)
                              : (double?)null;
                }
                string discountRateText = xmlNodeListDetail.SelectSingleNode("DiscountRate")?.InnerText;
                if (discountRateText != null)
                {
                    detail.DiscountRate =
                          !string.IsNullOrEmpty(discountRateText)
                              ? double.Parse(discountRateText)
                              : (double?)null;
                }
                string prodVatAmountText = xmlNodeListDetail.SelectSingleNode("ProdVatAmount")?.InnerText;
                if (prodVatAmountText != null)
                {
                    detail.ProdVatAmount =
                          !string.IsNullOrEmpty(prodVatAmountText)
                              ? double.Parse(prodVatAmountText)
                              : (double?)null;
                }
                string prodVatText = xmlNodeListDetail.SelectSingleNode("ProdVat")?.InnerText;
                if (prodVatText != null)
                {
                    detail.ProdVat = !string.IsNullOrEmpty(prodVatText)
                        ? double.Parse(prodVatText)
                        : (double?)null;
                }
                string innerText = xmlNodeListDetail.SelectSingleNode("Promotion")?.InnerText;
                if (innerText != null)
                {
                    detail.Promotion =
                        !string.IsNullOrEmpty(innerText)
                            ? int.Parse(innerText)
                            : 0;
                }
                string totalAmountText = xmlNodeListDetail.SelectSingleNode("TotalAmount")?.InnerText;
                if (totalAmountText != null)
                {
                    detail.TotalAmount =
                        !string.IsNullOrEmpty(totalAmountText)
                            ? double.Parse(totalAmountText)
                            : (double?)null;
                }


                string thueDBText = xmlNodeListDetail.SelectSingleNode("ThueDB")?.InnerText;
                if (thueDBText != null)
                {
                    detail.ThueDB = !string.IsNullOrEmpty(thueDBText)
                        ? double.Parse(thueDBText)
                        : (double?)null;
                }


                string total_ThueDBText = xmlNodeListDetail.SelectSingleNode("Total_ThueDB")?.InnerText;
                if (total_ThueDBText != null)
                {
                    detail.Total_ThueDB = !string.IsNullOrEmpty(total_ThueDBText)
                        ? double.Parse(total_ThueDBText)
                        : (double?)null;
                }


                details.Add(detail);
            }
            return details;
        }


        #region Invocie Cancel

        public static List<InvoiceCancel> GetInvoiceCancels(string xmlData)
        {
            _xmlDocument = new XmlDocument();
            _xmlDocument.LoadXml(xmlData);
            List<InvoiceCancel> invoiceCancels = new List<InvoiceCancel>();


            XmlNodeList invoiceList = _xmlDocument.SelectNodes("Invoices/Inv");
            if (invoiceList != null)
            {
                invoiceCancels.AddRange(from XmlNode node in invoiceList select GetInvoiceCancel(node));
            }

            return invoiceCancels;
        }

        public static InvoiceCancel GetInvoiceCancel(XmlNode node)
        {
            InvoiceCancel invoiceCancel = new InvoiceCancel
            {
                InvPattern = node.SelectSingleNode("InvPattern")?.InnerText,
                InvSerial = node.SelectSingleNode("InvSerial")?.InnerText,
                InvNumber = node.SelectSingleNode("InvNumber")?.InnerText,
                SoVb = node.SelectSingleNode("SoVb")?.InnerText,
                NgayVb = node.SelectSingleNode("NgayVb")?.InnerText,
                GhiChu = node.SelectSingleNode("GhiChu")?.InnerText
            };
            return invoiceCancel;
        }

        #endregion

        #region Invoice Mobiphone


        public static MobiphoneXmlObject GetMobiphoneXmlObject(string xml)
        {
            xml = xml.Replace("&", "&amp;");

            MobiphoneXmlObject invoices = new MobiphoneXmlObject();
            _xmlDocument = new XmlDocument();
            try
            {
                _xmlDocument.LoadXml(xml);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(_xmlDocument.NameTable);
                nsmgr.AddNamespace("inv", "http://thaison.vn/inv");
                nsmgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");


                XmlNode xmlNodeHeader = _xmlDocument["soapenv:Envelope"]?["soapenv:Header"]?["inv:Authentication"];
                //_xmlDocument.SelectSingleNode("soapenv:Envelope/soapenv:Header/inv:Authentication", nsmgr);

                XmlNode xmlNodeHoaDonEntity = _xmlDocument["soapenv:Envelope"]?["soapenv:Body"]?["XuatHoaDonDienTu"]?["hoaDonEntity"];
                // _xmlDocument.SelectSingleNode("soapenv:Envelope/soapenv:Body", nsmgr);

                Header header = GetHeader(xmlNodeHeader);
                HoaDonEntity hoaDonEntity = GetHoaDonEntity(xmlNodeHoaDonEntity);

                invoices.Header = header;
                invoices.HoaDonEntity = hoaDonEntity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return invoices;
        }

        private static Header GetHeader(XmlNode xmlNodeHeader)
        {
            Header header = new Header();
            if (xmlNodeHeader?.Attributes != null)
            {
                string linkWebService = xmlNodeHeader.Attributes["xmlns:inv"].InnerText;
                string userName = xmlNodeHeader["inv:userName"]?.InnerText;
                string passWord = xmlNodeHeader["inv:password"]?.InnerText;
                header.LinkWebService = linkWebService;
                header.Username = userName;
                header.Password = passWord;
            }
            return header;
        }


        private static HoaDonEntity GetHoaDonEntity(XmlNode xmlNodeHoaDonEntity)
        {
            try
            {
                HoaDonEntity hoaDonEntity = GetInfoHoaDonEntity(xmlNodeHoaDonEntity);
                XmlNodeList xmlNodeListDetails = xmlNodeHoaDonEntity["HangHoas"]?.ChildNodes;
                List<HangHoaEntity> hangHoaEntities = GetHangHoaEntities(xmlNodeListDetails);
                hoaDonEntity.HangHoaEntities = hangHoaEntities;
                return hoaDonEntity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private static HoaDonEntity GetInfoHoaDonEntity(XmlNode xmlNodeHoaDonEntity)
        {
            HoaDonEntity hoaDonEntity = new HoaDonEntity
            {
                MaLoaiHoaDon = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["MaLoaiHoaDon"]?.InnerText)
                    ? xmlNodeHoaDonEntity["MaLoaiHoaDon"]?.InnerText
                    : null,

                MauSo = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["MauSo"]?.InnerText)
                    ? xmlNodeHoaDonEntity["MauSo"]?.InnerText
                    : null,

                KyHieu = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["KyHieu"]?.InnerText)
                    ? xmlNodeHoaDonEntity["KyHieu"]?.InnerText
                    : null,

                TenLoaiHoaDon = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TenLoaiHoaDon"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TenLoaiHoaDon"]?.InnerText
                    : null,

                dhoadonid = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["dhoadonid"]?.InnerText)
                    ? xmlNodeHoaDonEntity["dhoadonid"]?.InnerText
                    : null,

                NgayNhapVien = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["NgayNhapVien"]?.InnerText)
                    ? DateTime.Parse(xmlNodeHoaDonEntity["NgayNhapVien"]?.InnerText)
                    : (DateTime?)null,

                NgayTaoHoaDon = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["NgayTaoHoaDon"]?.InnerText)
                    ? DateTime.Parse(xmlNodeHoaDonEntity["NgayTaoHoaDon"]?.InnerText)
                    : (DateTime?)null,

                NgayXuatHoaDon = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["NgayXuatHoaDon"]?.InnerText)
                    ? DateTime.Parse(xmlNodeHoaDonEntity["NgayXuatHoaDon"]?.InnerText)
                    : (DateTime?)null,

                BenBanMaDonVi = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenBanMaDonVi"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenBanMaDonVi"]?.InnerText
                    : null,

                BenBanMaSoThue = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenBanMaSoThue"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenBanMaSoThue"]?.InnerText
                    : null,

                BenBanTenDonVi = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenBanTenDonVi"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenBanTenDonVi"]?.InnerText
                    : null,

                BenBanDiaChi = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenBanDiaChi"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenBanDiaChi"]?.InnerText
                    : null,

                BenMuaMaSoThue = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaMaSoThue"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaMaSoThue"]?.InnerText
                    : null,

                BenMuaTenDonVi = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaTenDonVi"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaTenDonVi"]?.InnerText
                    : null,

                BenMuaHoTen = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaHoTen"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaHoTen"]?.InnerText
                    : null,

                BenMuaDiaChi = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaDiaChi"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaDiaChi"]?.InnerText
                    : null,

                BenMuaDienThoai = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaDienThoai"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaDienThoai"]?.InnerText
                    : null,

                BenMuaEmail = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["BenMuaEmail"]?.InnerText)
                    ? xmlNodeHoaDonEntity["BenMuaEmail"]?.InnerText
                    : null,

                TongTienThanhToanBangChu = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TongTienThanhToanBangChu"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TongTienThanhToanBangChu"]?.InnerText
                    : null,

                DongTienThanhToan = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["DongTienThanhToan"]?.InnerText)
                    ? xmlNodeHoaDonEntity["DongTienThanhToan"]?.InnerText
                    : null,

                HinhThucThanhToan = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["HinhThucThanhToan"]?.InnerText)
                    ? xmlNodeHoaDonEntity["HinhThucThanhToan"]?.InnerText
                    : null,

                TyGia = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TyGia"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TyGia"]?.InnerText
                    : null,

                SoBienLai = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["SoBienLai"]?.InnerText)
                    ? xmlNodeHoaDonEntity["SoBienLai"]?.InnerText
                    : null,

                TamUng = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TamUng"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TamUng"]?.InnerText
                    : null,

                GhiChu = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["GhiChu"]?.InnerText)
                    ? xmlNodeHoaDonEntity["GhiChu"]?.InnerText
                    : null,

                NamHoc = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["NamHoc"]?.InnerText)
                    ? xmlNodeHoaDonEntity["NamHoc"]?.InnerText
                    : null,


                TrangThaiDieuChinh = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TrangThaiDieuChinh"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TrangThaiDieuChinh"]?.InnerText
                    : null,

                KieuHoaDon = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["KieuHoaDon"]?.InnerText)
                    ? xmlNodeHoaDonEntity["KieuHoaDon"]?.InnerText
                    : null,

                TTamtNoTax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTamtNoTax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTamtNoTax"]?.InnerText
                    : null,

                TTvatNoTax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTvatNoTax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTvatNoTax"]?.InnerText
                    : null,

                TTnetNoTax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTnetNoTax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTnetNoTax"]?.InnerText
                    : null,


                TTamt0Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTamt0Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTamt0Tax"]?.InnerText
                    : null,

                TTvat0Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTvat0Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTvat0Tax"]?.InnerText
                    : null,

                TTnet0Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTnet0Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTnet0Tax"]?.InnerText
                    : null,

                TTamt5Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTamt5Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTamt5Tax"]?.InnerText
                    : null,

                TTvat5Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTvat5Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTvat5Tax"]?.InnerText
                    : null,

                TTnet5Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTnet5Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTnet5Tax"]?.InnerText
                    : null,

                TTamt10Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTamt10Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTamt10Tax"]?.InnerText
                    : null,

                TTvat10Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTvat10Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTamt10Tax"]?.InnerText
                    : null,

                TTnet10Tax = !string.IsNullOrEmpty(xmlNodeHoaDonEntity["TTnet10Tax"]?.InnerText)
                    ? xmlNodeHoaDonEntity["TTnet10Tax"]?.InnerText
                    : null,
            };

            string isGiuLaiText = xmlNodeHoaDonEntity["IsGiuLai"]?.InnerText;
            hoaDonEntity.IsGiuLai = !string.IsNullOrEmpty(isGiuLaiText) && bool.Parse(isGiuLaiText);

            string isSignText = xmlNodeHoaDonEntity["IsSign"]?.InnerText;
            hoaDonEntity.IsSign = !string.IsNullOrEmpty(isSignText) && bool.Parse(isSignText);

            string isSuDungBangKeText = xmlNodeHoaDonEntity["isSuDungBangKe"]?.InnerText;
            hoaDonEntity.isSuDungBangKe = !string.IsNullOrEmpty(isSuDungBangKeText) && bool.Parse(isSuDungBangKeText);

            string tienThueVatText = xmlNodeHoaDonEntity["TienThueVat"]?.InnerText;
            hoaDonEntity.TienThueVat = !string.IsNullOrEmpty(tienThueVatText)
                ? double.Parse(tienThueVatText)
                : 0;

            string tongTienHangText = xmlNodeHoaDonEntity["TongTienHang"]?.InnerText;
            hoaDonEntity.TongTienHang = !string.IsNullOrEmpty(tongTienHangText)
                ? double.Parse(tongTienHangText)
                : 0;


            string tongTienThanhToanText = xmlNodeHoaDonEntity["TongTienThanhToan"]?.InnerText;
            hoaDonEntity.TongTienThanhToan = !string.IsNullOrEmpty(tongTienThanhToanText)
                ? double.Parse(tongTienThanhToanText)
                : 0;

            string tienChietKhauText = xmlNodeHoaDonEntity["TienChietKhau"]?.InnerText;
            hoaDonEntity.TienChietKhau = !string.IsNullOrEmpty(tienChietKhauText)
                ? double.Parse(tienChietKhauText)
                : 0;


            return hoaDonEntity;
        }

        private static List<HangHoaEntity> GetHangHoaEntities(XmlNodeList xmlNodeListDetails)
        {
            List<HangHoaEntity> hangHoaEntities = new List<HangHoaEntity>();

            foreach (XmlNode xmlNodeListDetail in xmlNodeListDetails)
            {
                HangHoaEntity hangHoa = new HangHoaEntity
                {
                    SoThuTu = !string.IsNullOrEmpty(xmlNodeListDetail["SoThuTu"]?.InnerText)
                        ? xmlNodeListDetail["SoThuTu"]?.InnerText
                        : null,
                    MaHang = !string.IsNullOrEmpty(xmlNodeListDetail["MaHang"]?.InnerText)
                        ? xmlNodeListDetail["MaHang"]?.InnerText
                        : null,
                    TenHang = !string.IsNullOrEmpty(xmlNodeListDetail["TenHang"]?.InnerText)
                        ? xmlNodeListDetail["TenHang"]?.InnerText
                        : null,
                    DonViTinh = !string.IsNullOrEmpty(xmlNodeListDetail["DonViTinh"]?.InnerText)
                        ? xmlNodeListDetail["DonViTinh"]?.InnerText
                        : null,
                    HeSoMon = !string.IsNullOrEmpty(xmlNodeListDetail["HeSoMon"]?.InnerText)
                        ? xmlNodeListDetail["HeSoMon"]?.InnerText
                        : null,
                    HeSo = !string.IsNullOrEmpty(xmlNodeListDetail["HeSo"]?.InnerText)
                        ? xmlNodeListDetail["HeSo"]?.InnerText
                        : null,
                    DonViPhi = !string.IsNullOrEmpty(xmlNodeListDetail["DonViPhi"]?.InnerText)
                        ? xmlNodeListDetail["DonViPhi"]?.InnerText
                        : null,
                    DieuChinh = !string.IsNullOrEmpty(xmlNodeListDetail["DieuChinh"]?.InnerText)
                        ? xmlNodeListDetail["DieuChinh"]?.InnerText
                        : null
                };

                string khuyenMaiText = xmlNodeListDetail["khuyenMai"]?.InnerText;
                hangHoa.khuyenMai = !string.IsNullOrEmpty(khuyenMaiText) && bool.Parse(khuyenMaiText);

                string soLuongText = xmlNodeListDetail["SoLuong"]?.InnerText;
                hangHoa.SoLuong = !string.IsNullOrEmpty(soLuongText) ?
                    double.Parse(soLuongText)
                    : (double?)null;

                string donGiaText = xmlNodeListDetail["DonGia"]?.InnerText;
                hangHoa.DonGia =
                    !string.IsNullOrEmpty(donGiaText)
                        ? double.Parse(donGiaText)
                        : (double?)null;

                string thanhTienText = xmlNodeListDetail["ThanhTien"]?.InnerText;
                hangHoa.ThanhTien = !string.IsNullOrEmpty(thanhTienText)
                    ? double.Parse(thanhTienText)
                    : (double?)null;

                string vatText = xmlNodeListDetail["Vat"]?.InnerText;
                hangHoa.Vat =
                    !string.IsNullOrEmpty(vatText)
                        ? double.Parse(vatText)
                        : (double?)null;

                string tienVatText = xmlNodeListDetail["TienVat"]?.InnerText;
                hangHoa.TienVat = !string.IsNullOrEmpty(tienVatText)
                    ? double.Parse(tienVatText)
                    : (double?)null;

                string thanhTienSauThueText = xmlNodeListDetail["ThanhTienSauThue"]?.InnerText;
                hangHoa.ThanhTienSauThue =
                    !string.IsNullOrEmpty(thanhTienSauThueText)
                        ? double.Parse(thanhTienSauThueText)
                        : (double?)null;

                hangHoaEntities.Add(hangHoa);
            }
            return hangHoaEntities;
        }
        #endregion
    }
}