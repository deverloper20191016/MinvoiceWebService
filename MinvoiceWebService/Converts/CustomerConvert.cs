using System;
using System.Collections.Generic;
using System.Xml;
using MinvoiceWebService.Data;

namespace MinvoiceWebService.Converts
{
    public class CustomerConvert
    {
        private static XmlDocument _xmlDocument;
        public static List<Customer> GetCustomers(string url)
        {
            url = url.Replace("&", "&amp;");
            _xmlDocument = new XmlDocument();
            List<Customer> customers = new List<Customer>();
            try
            {
                _xmlDocument.LoadXml(url);
                XmlNodeList customerNodeList = _xmlDocument.SelectNodes("Customers/Customer");
                customers = ConvertXmlNodeListToCustomerList(customerNodeList);
                return customers;
            }
            catch (Exception ex)
            {

            }
            return customers;
        }

        private static List<Customer> ConvertXmlNodeListToCustomerList(XmlNodeList xmlNodeList)
        {
            List<Customer> customers = new List<Customer>();
            foreach (XmlNode customerNodeItem in xmlNodeList)
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
            return customers;
        }
    }
}