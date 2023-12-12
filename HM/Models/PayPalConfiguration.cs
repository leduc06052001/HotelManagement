using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HM.Models
{
    public class PayPalConfiguration
    {
        public static class PaypalConfiguration
        {
            // Các biến lưu cliendID và clientSecert  
            public readonly static string ClientId;
            public readonly static string ClientSecret;

            static PaypalConfiguration()
            {
                var config = GetConfig();
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }
            // Lấy thông tin từ web.config  
            public static Dictionary<string, string> GetConfig()
            {
                return PayPal.Api.ConfigManager.Instance.GetProperties();
            }
            private static string GetAccessToken()
            {
                // Nhận 1 accesstoken từ paypal  
                string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
                return accessToken;
            }
            public static APIContext GetAPIContext()
            {
                // Trả về dối tượng apicontext bằng cách gọi accesstoken 
                APIContext apiContext = new APIContext(GetAccessToken());
                apiContext.Config = GetConfig();
                return apiContext;
            }
        }

    }
}