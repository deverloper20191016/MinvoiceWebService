using MinvoiceWebService.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MinvoiceWebService.Services
{
    public class LoginService
    {
        private static JObject Login(string username, string password, string mst)
        {
            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            JObject json = new JObject
            {
                {"username",username },
                {"password",password },
                {"ma_dvcs","VP" }
            };

            string urlLogin = $"{CommonConstants.Potocol}{mst}.{CommonConstants.UrlLoginApi}";
            //var urlLogin = CommonConstants.UrlLogin;
            string token = client.UploadString(urlLogin, json.ToString());
            return JObject.Parse(token);
        }

        private static void CreateAuthorization(WebClient webClient, string username, string pass, string mst)
        {
            JObject tokenJson = Login(username, pass, mst);
            if (tokenJson.ContainsKey("token"))
            {
                string authorization = "Bear " + tokenJson["token"] + ";VP;vi";
                webClient.Headers[HttpRequestHeader.Authorization] = authorization;
            }
            else
            {
                throw new Exception(tokenJson["error"].ToString());
            }

        }

        private static void CreateAuthorizationIPos(WebClient webClient, string token, string mst)
        {
            string authorization = "Bear " + token;
            webClient.Headers[HttpRequestHeader.Authorization] = authorization;
        }

        public static WebClient SetupWebClient(string userName, string passWord, string mst)
        {
            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");

            CreateAuthorization(webClient, userName, passWord, mst);
            return webClient;
        }


        public static WebClient SetupWebClientIPos(string mst, string token)
        {
            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");

            CreateAuthorizationIPos(webClient, token, mst);
            return webClient;
        }

        public static HttpClient SetupHttpClient(string username, string pass, string mst)
        {
            JObject tokenJson = Login(username, pass, mst);
            string token = tokenJson["token"] + ";VP;vi";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bear", token);
            return client;

        }

    }
}