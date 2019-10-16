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
        public static readonly string DaKy = "Đã ký";

        public static readonly string UrlGetInvoiceNumberByDate =
            WebConfigurationManager.AppSettings["UrlGetInvoiceNumberByDate"];



        //public static string UrlLogin = $"{Potocol}{Mst}.{UrlLoginApi}";
        //public static string UrlExecuteCommand = $"{Potocol}{Mst}.{UrlExecuteCommandApi}";
        //public static string UrlDeleteInvoice = $"{Potocol}{Mst}.{UrlDeleteInvoiceApi}";
        //public static string UrlAdd = $"{Potocol}{Mst}.{UrlAddApi}";
        //public static string UrlSave = $"{Potocol}{Mst}.{UrlSaveApi}";
    }
}