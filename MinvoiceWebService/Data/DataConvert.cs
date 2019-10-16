using System;
using System.Collections.Generic;
using System.Xml;
using MinvoiceWebService.Models;

namespace MinvoiceWebService.Data
{
    public class DataConvert
    {
        private static XmlDocument _xmlDocument;

        #region Invoice



        #endregion

        #region Invoice Cancel

        public static List<InvoiceCancel> GetInvoiceCancels(string xml)
        {
            List<InvoiceCancel> invoiceCancels = new List<InvoiceCancel>();
            try
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.LoadXml(xml);
                XmlNodeList invCancelNodeList = _xmlDocument.SelectNodes("Invoices/Inv");

                if (invCancelNodeList != null)
                {
                    foreach (XmlNode node in invCancelNodeList)
                    {
                        InvoiceCancel invCancel = new InvoiceCancel
                        {
                            Serial = node.SelectSingleNode("Serial")?.InnerText,
                            InvNo = node.SelectSingleNode("InvNo")?.InnerText
                        };
                        invoiceCancels.Add(invCancel);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return invoiceCancels;

        }
        #endregion

        #region Customer

        public static List<Customer> GetCustomers(string xml)
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.LoadXml(xml);
                XmlNodeList customerNodeList = _xmlDocument.SelectNodes("Customers/Customer");

                if (customerNodeList != null)
                {
                    foreach (XmlNode customerNodeItem in customerNodeList)
                    {
                        Customer customer = new Customer
                        {
                            Name = customerNodeItem.SelectSingleNode("Name")?.InnerText,
                            Code = customerNodeItem.SelectSingleNode("Code")?.InnerText,
                            TaxCode = customerNodeItem.SelectSingleNode("TaxCode")?.InnerText,
                            Address = customerNodeItem.SelectSingleNode("Address")?.InnerText,
                            BankAccountName = customerNodeItem.SelectSingleNode("BankAccountName")?.InnerText,
                            BankName = customerNodeItem.SelectSingleNode("BankName")?.InnerText,
                            BankNumber = customerNodeItem.SelectSingleNode("BankNumber")?.InnerText,
                            Email = customerNodeItem.SelectSingleNode("Email")?.InnerText,
                            Fax = customerNodeItem.SelectSingleNode("Fax")?.InnerText,
                            Phone = customerNodeItem.SelectSingleNode("Phone")?.InnerText,
                            ContactPerson = customerNodeItem.SelectSingleNode("ContactPerson")?.InnerText,
                            RepresentPerson = customerNodeItem.SelectSingleNode("RepresentPerson")?.InnerText,
                            CusType = !string.IsNullOrEmpty(customerNodeItem.SelectSingleNode("CusType")?.InnerText) ? Convert.ToInt32(customerNodeItem.SelectSingleNode("CusType")?.InnerText) : (int?)null
                        };

                        customers.Add(customer);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return customers;
        }

        #endregion
    }
}