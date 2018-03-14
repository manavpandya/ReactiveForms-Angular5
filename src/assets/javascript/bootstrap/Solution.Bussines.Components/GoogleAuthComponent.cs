using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using System.Web.Security;
using Solution.Bussines.Components.Common;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Google Login related Component Class Contains AUTHORIZE related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public static class GoogleAuthComponent
    {
        public static string _consumerKey = AppLogic.AppConfigs("GoogleConsumerKey");
        public static string _consumerSecret = AppLogic.AppConfigs("GoogleConsumerSecret");

        /// <summary>
        /// Used by Google oAuth process to retrieve tokens.
        /// </summary>
        /// <param name="code">string code</param>
        /// <param name="refresh_token">string refresh_token</param>
        /// <param name="ReturnURL">string ReturnURL</param>
        /// <returns>Returns String</returns>
        public static GoogleTokens GoogleTokensGet(string code, string refresh_token, string ReturnURL)
        {
            string grant_type = "authorization_code";
            string code_or_refresh_token = "code=" + System.Web.HttpUtility.UrlEncode(code);

            if (refresh_token != null)
            {
                grant_type = "refresh_token";
                code_or_refresh_token = "refresh_token=" + System.Web.HttpUtility.UrlEncode(refresh_token);
            }

            string json = AuthUtilitiesComponent.WebRequest(AuthUtilitiesComponent.Method.POST, "https://accounts.google.com/o/oauth2/token",
                code_or_refresh_token
                + "&client_id=" + _consumerKey.ToString()
                + "&client_secret=" + _consumerSecret.ToString()
                + "&redirect_uri=" + System.Web.HttpUtility.UrlEncode(ReturnURL)
                + "&grant_type=" + grant_type
                );
            return Json.Deserialise<GoogleTokens>(json);
        }
    }

    /// <summary>
    /// Google Access Token 
    /// </summary>
    
    public class GoogleTokens
    {
        [DataMember]
        public string access_token;
        [DataMember]
        public int expires_in;
        [DataMember]
        public string token_type;
        [DataMember]
        public string refresh_token;
    }

    public class GoogleData
    {
        [DataMember]
        public Data data;
    }
    public class Data
    {
        [DataMember]
        public string email;
    }
}
