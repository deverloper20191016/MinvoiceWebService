namespace MinvoiceWebService.Data
{
    public class DataRequestObject
    {
        public string Mst { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string XmlData { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Loại thao tác
        /// false: Thêm mới
        /// true: Cập nhật/Điều chỉnh
        /// </summary>
        public bool Opt { get; set; }

        /// <summary>
        /// Loại điều chỉnh
        /// 1: Điều chỉnh hóa đơn chưa ký; 
        /// 2: Điều chỉnh hóa đơn đã ký
        /// </summary>
        public int TypeUpdate { get; set; }

        /// <summary>
        /// inv_adjustmentType của hóa đơn
        /// 1: Gốc
        /// 5: Điều chỉnh định danh
        /// 19: Điều chỉnh tăng
        /// 21: Điều chỉnh giảm
        /// </summary>
        public int TypeOfInvoice { get; set; } = 1;
        public string InvOriginalId { get; set; }

        /// <summary>
        /// Loại ký
        /// 0: Không ký
        /// 1: Ký
        /// 2: Bỏ cách
        /// </summary>
        public int? SignType { get; set; }

        public string InvInvoiceCodeId { get; set; }

        /// <summary>
        /// inv_adjustmentType của hóa đơn gốc
        /// 1: Gốc
        /// 5: Điều chỉnh định danh
        /// 19: Điều chỉnh tăng
        /// 21: Điều chỉnh giảm
        /// </summary>
        public int TypeOfInvoiceParent { get; set; }

    }
}