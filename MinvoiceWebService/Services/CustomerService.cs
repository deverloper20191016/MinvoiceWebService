using System;
using System.Collections.Generic;
using MinvoiceWebService.Converts;
using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;

namespace MinvoiceWebService.Services
{
    public class CustomerService
    {
        #region Customer
        private static string GetIdOfCustomerByCode(string code, out string dmdtId, out string dtMeId, string username, string pass, string mst)
        {
            var webClient = LoginService.SetupWebClient(username, pass, mst);
            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteCommandApi}";

            string json = "{\"command\":\"CM00022\" , parameter:{\"ma_dt\":\"" + code + "\"}}";

            dmdtId = "";
            dtMeId = "";
            string codeResult = "";
            try
            {
                var rs = webClient.UploadString(url, json);
                JArray jArray = JArray.Parse(rs);

                if (jArray.Count > 0)
                {
                    dmdtId = jArray[0]["dmdt_id"].ToString();
                    dtMeId = jArray[0]["dt_me_id"].ToString();
                    codeResult = jArray[0]["ma_dt"].ToString();
                }
            }
            catch (Exception e)
            {
                codeResult = e.Message;
            }
            return codeResult;
        }

        public static string UpdateCustomer(string xmlData, string mst, string username, string pass, string linkWs)
        {
            JObject jObjectResult = new JObject();
            try
            {
                List<Customer> customers = CustomerConvert.GetCustomers(xmlData);
                var webClient = LoginService.SetupWebClient(username, pass, mst);
                var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlSaveApi}";

                foreach (var customer in customers)
                {
                    string dmdtId;
                    string dtMeId;
                    string code = GetIdOfCustomerByCode(customer.Code, out dmdtId, out dtMeId, username, pass, mst);
                    customer.dmdt_id = dmdtId;
                    customer.dt_me_id = dtMeId;
                    var customerJObject = CustomerJsonConvert.CreateJObjetcCustomer(customer, !string.IsNullOrEmpty(code));
                    string dataRequest = customerJObject.ToString();
                    try
                    {
                        try
                        {
                            var rs = webClient.UploadString(url, dataRequest);
                            JObject jObject = JObject.Parse(rs);
                            if (jObject.ContainsKey("ok"))
                            {
                                jObjectResult.Add($"OK_{customer.Code}", jObject["ok"].ToString());
                            }
                            else
                            {
                                if (jObject.ContainsKey("error"))
                                {
                                    jObjectResult.Add($"ERROR_{customer.Code}", jObject["error"].ToString());
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            jObjectResult.Add($"ERROR_{customer.Code}", $"ERROR: {e.Message}");
                        }
                    }
                    catch (Exception e)
                    {
                        jObjectResult.Add($"ERROR_{customer.Code}", $"ERROR: {e.Message}");
                    }
                }

                return jObjectResult.ToString();
            }
            catch (Exception e)
            {
                jObjectResult.Add("ERROR", $"ERROR: {e.Message}");
                return jObjectResult.ToString();
            }
        }
        #endregion

    }
}