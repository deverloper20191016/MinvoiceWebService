using System.Web.Services;
using MinvoiceWebService.Converts;
using MinvoiceWebService.Services;

namespace MinvoiceWebService
{
    /// <summary>
    /// Summary description for MinvoiceWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MinvoiceWebService : WebService
    {

        [WebMethod]
        public string GetInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string invoiceNumber)
        {
            var result = MinvoiceService.GetInvoice(mst, userName, passWord, mauSo, kyHieu, invoiceNumber);
            return result;
        }

        [WebMethod]
        public string CreateInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu, string xml)
        {
            var resutl = MinvoiceService.CreateInvoice(mst, userName, passWord, mauSo, kyHieu, "", xml, false);
            return resutl;
        }
        [WebMethod]
        public string CreateInvoiceVMD(string mst, string userName, string passWord, string mauSo, string kyHieu, string xml)
        {
            var resutl = MinvoiceService.CreateInvoice(mst, userName, passWord, mauSo, kyHieu, "", xml, false);
            return resutl;
        }
        [WebMethod]
        public string CreateInvoiceSaveSign(string mst, string userName, string passWord, string mauSo, string kyHieu, string xml, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xml);
            var resutl = MinvoiceService.CreateInvoiceSaveSign(mst, userName, passWord, mauSo, kyHieu, "", xmlConvertByTypeFont, false);
            return resutl;
        }

        [WebMethod]
        public string CreateInvoiceSse(string mst, string userName, string passWord, string mauSo, string kyHieu, string xml, int signType)
        {
            var resutl = MinvoiceService.CreateInvoiceSse(mst, userName, passWord, mauSo, kyHieu, "", xml, false, signType);
            return resutl;
        }


        [WebMethod]
        public string UpdateInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml)
        {
            var resutl = MinvoiceService.UpdateInvoice(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, true);
            return resutl;
        }

        // typeOfAdjustment 1: Điều chỉnh tăng; 2: Điều chỉnh giảm, 3: Điều chỉnh định danh

        [WebMethod]
        public string AdjustInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string invoiceNumber, string xml, int typeOfAdjustment)
        {
            var resutl = MinvoiceService.UpdateInvoice(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xml, false, 1, 2, typeOfAdjustment);
            return resutl;
        }


        [WebMethod]
        public string CancelInvoice(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string xml)
        {
            var result = MinvoiceService.CancelInvoice(mst, userName, passWord, xml);
            return result;
        }

        [WebMethod]
        public string UpdateCustomer(string xmlData, string mst, string username, string pass, string linkWs)
        {
            var result = CustomerService.UpdateCustomer(xmlData, mst, username, pass, linkWs);
            return result;
        }

        [WebMethod]
        public string GetInvoiceNumberByDate(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string tuNgay, string denNgay)
        {
            var result =
                MinvoiceService.GetInvoiceFromDateToDate(mst, userName, passWord, mauSo, kyHieu, tuNgay, denNgay);
            return result;
        }

        [WebMethod]
        public string DownloadPdf(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber)
        {
            var result = MinvoiceService.PreviewPdf(mst, userName, passWord, mauSo, kyHieu, invoiceNumber);
            return result;
        }

        [WebMethod]
        public string DownloadPdfBase64(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber)
        {
            var result = MinvoiceService.DownloadPdfBase64(mst, userName, passWord, mauSo, kyHieu, invoiceNumber);
            return result;
        }

        //Convert to font



        [WebMethod(MessageName = "CreateInvoiceConvertFont")]
        public string CreateInvoiceConvertFont(string mst, string userName, string passWord, string mauSo, string kyHieu, string xml, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xml);

            var resutl = MinvoiceService.CreateInvoice(mst, userName, passWord, mauSo, kyHieu, "", xmlConvertByTypeFont, false);
            return resutl;
        }


        [WebMethod(MessageName = "UpdateInvoiceConvertFont")]
        public string UpdateInvoiceConvertFont(string mst, string userName, string passWord, string mauSo, string kyHieu, string invoiceNumber, string xml, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xml);
            var resutl = MinvoiceService.UpdateInvoice(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xmlConvertByTypeFont, true);
            return resutl;
        }

        // typeOfAdjustment 1: Điều chỉnh tăng; 2: Điều chỉnh giảm, 3: Điều chỉnh định danh

        [WebMethod(MessageName = "AdjustInvoiceConvertFont")]
        public string AdjustInvoiceConvertFont(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string invoiceNumber, string xml, int typeOfAdjustment, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xml);
            var resutl = MinvoiceService.UpdateInvoice(mst, userName, passWord, mauSo, kyHieu, invoiceNumber, xmlConvertByTypeFont, false, 1, 2, typeOfAdjustment);
            return resutl;
        }


        [WebMethod(MessageName = "CancelInvoiceConvertFont")]
        public string CancelInvoiceConvertFont(string mst, string userName, string passWord, string mauSo, string kyHieu,
            string xml, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xml);
            var result = MinvoiceService.CancelInvoice(mst, userName, passWord, xmlConvertByTypeFont);
            return result;
        }

        [WebMethod(MessageName = "UpdateCustomerConvertFont")]
        public string UpdateCustomerConvertFont(string xmlData, string mst, string username, string pass, string linkWs, int typeFont)
        {
            string xmlConvertByTypeFont = Converter.ConvertToFont(typeFont, xmlData);
            var result = CustomerService.UpdateCustomer(xmlConvertByTypeFont, mst, username, pass, linkWs);
            return result;
        }


        #region Mobiphone

        //[WebMethod]
        //public string CreateInvoiceMobiphone(string xml)
        //{
        //    var a = DataConvert.GetMobiphoneXmlObject(xml);
        //    return "OK";
        //}
        #endregion

    }
}
