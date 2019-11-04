using System.Web.Configuration;

namespace MinvoiceWebService.Data
{
    public class CommonConstants
    {
        public static readonly string Potocol = WebConfigurationManager.AppSettings["Protocol"];
        //public static readonly string Mst = WebConfigurationManager.AppSettings["Mst"];
        public static readonly string UrlLoginApi = WebConfigurationManager.AppSettings["UrlLogin"];
        public static readonly string UrlExecuteCommandApi = WebConfigurationManager.AppSettings["UrlExecuteCommand"];
        public static readonly string UrlDeleteInvoiceApi = WebConfigurationManager.AppSettings["UrlDeleteInvoice"];
        public static readonly string UrlAddApi = WebConfigurationManager.AppSettings["UrlAdd"];
        public static readonly string UrlAddSignApi = WebConfigurationManager.AppSettings["UrlAddSign"];
        public static readonly string UrlAddSseApi = WebConfigurationManager.AppSettings["UrlAddSSE"];
        public static readonly string UrlSaveApi = WebConfigurationManager.AppSettings["UrlSave"];
        public static readonly string UrlGetInfo = WebConfigurationManager.AppSettings["UrlGetInfo"];
        public static readonly string UrlPreviewPdf = WebConfigurationManager.AppSettings["UrlPreviewPdf"];

        public static readonly string ChoKy = "Chờ ký";
        public static readonly string ChoDuyet = "Chờ duyệt";
        public static readonly string DaKy = "Đã ký";
        public static readonly string ChoNguoiMuaKy = "Chờ người mua ký";
        public static readonly string NguoiMuaDaKy = "Người mua đã ký";

        public static readonly string UrlGetInvoiceNumberByDate =
            WebConfigurationManager.AppSettings["UrlGetInvoiceNumberByDate"];

        // Cập nhật ngày 2019-10-23
        public static readonly string UrlGetInvoiceById = WebConfigurationManager.AppSettings["UrlGetInvoiceById"];
        public static readonly string UrlGetListInvoice = WebConfigurationManager.AppSettings["UrlGetListInvoice"];
        public static readonly string UrlGetTbph = WebConfigurationManager.AppSettings["UrlGetTbph"];

        public static readonly string UrlGetInvoiceByKeyApiBravo = WebConfigurationManager.AppSettings["UrlGetInvoiceByKeyApiBravo"];
        public static readonly string UrlGetInvoiceFkeyApiBravo = WebConfigurationManager.AppSettings["UrlGetInvoiceFkeyApiBravo"];
        public static readonly string UrlGetInvoiceByDateApiBravo = WebConfigurationManager.AppSettings["UrlGetInvoiceByDateApiBravo"];

        public static readonly string UserCreateInvoiceIPos = WebConfigurationManager.AppSettings["UserCreateInvoiceIPos"];

        // Cập nhật ngày 2019-10-25

        public static readonly string UrlSignInvoice = WebConfigurationManager.AppSettings["UrlSignInvoice"];
        public static readonly string UrlSubstituteInvoice = WebConfigurationManager.AppSettings["UrlSubstituteInvoice"];

        //public static string UrlLogin = $"{Potocol}{Mst}.{UrlLoginApi}";
        //public static string UrlExecuteCommand = $"{Potocol}{Mst}.{UrlExecuteCommandApi}";
        //public static string UrlDeleteInvoice = $"{Potocol}{Mst}.{UrlDeleteInvoiceApi}";
        //public static string UrlAdd = $"{Potocol}{Mst}.{UrlAddApi}";
        //public static string UrlSave = $"{Potocol}{Mst}.{UrlSaveApi}";
    }
}