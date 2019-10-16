using System;
using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;

namespace MinvoiceWebService.Converts
{
    public class CustomerJsonConvert
    {
        #region Customer

        public static JObject CreateJObjetcCustomer(Customer customer, bool opt)
        {
            var data = CreateJArrayMainDataCustomer(customer, opt);
            var jObject = new JObject
            {
                {"windowid", "WIN00009"},
                {"editmode", !opt ? 1 : 2},
                {"data", data}
            };
            return jObject;
        }

        private static JArray CreateJArrayMainDataCustomer(Customer customer, bool opt)
        {
            var jObject = CreateJObjectMainDataCustomer(customer, opt);
            var jArray = new JArray
            {
                jObject
            };
            return jArray;
        }

        private static JObject CreateJObjectMainDataCustomer(Customer customer, bool opt)
        {
            var details = CreateJArrayDetails(customer, opt);
            var jObject = new JObject
            {
                {"ma_dvcs", "VP"},
                {"ma_dt", customer.Code},
                {"ten_dt", customer.Name},
                {"dia_chi", customer.Address},
                {"dien_thoai", customer.Phone},
                {"fax", customer.Fax},
                {"ms_thue", customer.TaxCode},
                {"dai_dien", customer.Name},
                { "email", customer.Email},
                {"dien_giai", ""},
                {"details", details}
            };
            if (opt)
            {
                jObject.Add("dmdt_id", customer.dmdt_id);
                jObject.Add("nh_dt", false);
                jObject.Add("dt_me_id", customer.dt_me_id);
                jObject.Add("user_new", "ADMINISTRATOR");
                jObject.Add("date_new", DateTime.Now);
                jObject.Add("id", customer.dmdt_id);
                jObject.Add("$sub", "<div></div>");
            }
            else
            {
                jObject.Add("dt_me_id", null);
            }

            return jObject;
        }

        private static JArray CreateJArrayDetails(Customer customer, bool opt)
        {
            var jObject = CreateJObjectDetails(customer, opt);
            var jArray = new JArray
            {
                jObject
            };

            return jArray;
        }

        private static JObject CreateJObjectDetails(Customer customer, bool opt)
        {
            var data = CreateJArrayDataBank(customer, opt);
            var jObject = new JObject
            {
                {"tab_id", "TAB00014"},
                {"tab_table", "dmngh"},
                {"data", data}
            };

            return jObject;
        }

        private static JArray CreateJArrayDataBank(Customer customer, bool opt)
        {
            var jObject = CreateJObjectDataBank(customer, opt);
            var jArray = new JArray
            {
                jObject
            };
            return jArray;
        }

        private static JObject CreateJObjectDataBank(Customer customer, bool opt)
        {
            var jObject = new JObject
            {
                {"ma_dvcs", "VP"},
                {"so_tk", customer.BankNumber},
                {"tai_ngh", customer.BankName}
            };

            if (opt)
            {
                jObject.Add("dmdt_id", customer.dmdt_id);
                jObject.Add("user_new", "ADMINISTRA");
                jObject.Add("date_new", DateTime.Now);
                //jObject.Add("dmngh_id", "49cb20c0-2ba7-4b1f-bcee-578d8bcf17a7");
                //jObject.Add("id", "49cb20c0-2ba7-4b1f-bcee-578d8bcf17a7");
            }
            else
            {
                jObject.Add("id", null);
            }

            return jObject;
        }

        #endregion
    }
}