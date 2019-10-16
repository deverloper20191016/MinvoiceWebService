using System;

namespace MinvoiceWebService.Models
{
    public class Master
    {
        public string Key { get; set; }
        public string CusCode { get; set; }
        public string Buyer { get; set; }
        public string CusName { get; set; }
        public string CusAddress { get; set; }
        public string CusTaxCode { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime ArisingDate { get; set; }
        public double Total { get; set; }
        public double DiscountAmount { get; set; }
        public double VatRate { get; set; }
        public double VatAmount { get; set; }
        public double Amount { get; set; }
        public string EmailDeliver { get; set; }
        public string CusBankNo { get; set; }
        public string CusBankName { get; set; }
        public string Currency { get; set; }
        public string ExRate { get; set; }
        public string SellBankNo { get; set; }
        public string SellBankName { get; set; }
        public string InvNote { get; set; }
        public string NumberOrDucument { get; set; }
        public DateTime DateOrDucument { get; set; }
        public string NoteOfDocument { get; set; }
    }

    public class Detail
    {
        public string No { get; set; }
        public string ProdName { get; set; }
        public string ProdCode { get; set; }
        public string ProdUnit { get; set; }
        public double ProdQuantity { get; set; }
        public double ProdPrice { get; set; }
        public double Amount { get; set; }
        public double DiscountAmount { get; set; }
        public double DiscountRate { get; set; }
        public double ProdVatAmount { get; set; }
        public double ProdVat { get; set; }
        public int Promotion { get; set; }
        public double TotalAmount { get; set; }
    }
    public class InvoiceCancel
    {
        public string Serial { get; set; }
        public string InvNo { get; set; }
    }
}