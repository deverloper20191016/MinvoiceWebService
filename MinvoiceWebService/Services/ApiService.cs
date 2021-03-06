﻿using System;
using System.Linq;
using System.Web.WebSockets;
using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;

namespace MinvoiceWebService.Services
{
    public class ApiService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">Tài khoản</param>
        /// <param name="passWord">Mật khẩu</param>
        /// <param name="serial">Ký hiệu hóa đơn</param>
        /// <param name="pattern">Mẫu số hóa đơn</param>
        /// <param name="invNumber">Số hóa đơn</param>
        /// <param name="mst">Mã số thuế</param>
        /// <returns></returns>
        public static JArray GetInvoice(string userName, string passWord, string serial, string pattern,
            string invNumber, string mst)
        {
            var webClient = LoginService.SetupWebClient(userName, passWord, mst);

            var json = "{\"command\":\"CM00023\" , parameter:{\"ma_dvcs\":\"" + "VP" + "\",\"mau_hd\":\"" + pattern +
                       "\",\"inv_invoiceSeries\":\"" + serial + "\",\"inv_invoiceNumber\":\"" +
                       invNumber + "\"}}";

            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteCommandApi}";
            //var url = CommonConstants.UrlExecuteCommand;
            var rs = webClient.UploadString(url, json);
            var result = JArray.Parse(rs);
            return result;
        }

        /// <summary>
        /// Lấy hóa đơn theo Id
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="mst"></param>
        /// <param name="id">Id của hóa đơn</param>
        /// <returns></returns>
        public static JObject GetInvoiceById(string userName, string passWord, string mst, string id)
        {
            var webClient = LoginService.SetupWebClient(userName, passWord, mst);
            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetInvoiceById}{id}";
            var rs = webClient.DownloadString(url);
            var result = JObject.Parse(rs);
            return result;
        }

        public static JObject GetListInvoice(string userName, string passWord, string mst, string listId)
        {
            var webClient = LoginService.SetupWebClient(userName, passWord, mst);
            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlGetListInvoice}";
            var json = new JObject
            {
                {"data", listId }
            };
            var rs = webClient.UploadString(url, json.ToString());
            if (rs.Equals("null"))
            {
                return new JObject
                {
                    {"error","Không tìm thấy hóa đơn" }
                };
            }

            var result = JObject.Parse(rs);
            return result;

        }

        public static JArray GetInvoiceByKey(string userName, string passWord, string serial, string pattern,
            string key, string mst)
        {
            // test
            var webClient = LoginService.SetupWebClient(userName, passWord, mst);

            var json = "{\"command\":\"CM00024\" , parameter:{\"ma_dvcs\":\"" + "VP" + "\",\"mau_hd\":\"" + pattern +
                       "\",\"inv_invoiceSeries\":\"" + serial + "\",\"so_benh_an\":\"" +
                       key + "\"}}";

            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteCommandApi}";
            //var url = CommonConstants.UrlExecuteCommand;
            var rs = webClient.UploadString(url, json);
            var result = JArray.Parse(rs);
            return result;
        }

        public static string GetInvInvoiceCodeId(string mst, string userName, string passWord, string mauSo, string kyHieu)
        {
            var webClient = LoginService.SetupWebClient(userName, passWord, mst);

            var json = "{\"command\":\"CM00021\" , parameter:{\"username\":\"" + userName + "\"}}";
            var url = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlExecuteCommandApi}";

            try
            {
                var response = webClient.UploadString(url, json);
                var resultArray = JArray.Parse(response);
                if (resultArray.Count > 0)
                {
                    return resultArray.FirstOrDefault(x =>
                             x["mau_so"].ToString().Equals(mauSo) && x["ky_hieu"].ToString().Equals(kyHieu))?[
                             "ctthongbao_id"]
                         .ToString();

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}