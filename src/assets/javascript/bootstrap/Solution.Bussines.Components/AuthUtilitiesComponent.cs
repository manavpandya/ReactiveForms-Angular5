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
using System.Collections.Specialized;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Google Login related Component Class Contains AUTHORIZE related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public static class AuthUtilitiesComponent
    {
        public enum Method { GET, POST, PUT, DELETE };

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Method method</param>
        /// <param name="url">string url</param>
        /// <param name="postData">string postData</param>
        /// <returns>Returns The web server response.</returns>
        public static string WebRequest(Method method, string url, string postData)
        {
            return WebRequest(method, url, postData, null);
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Method method</param>
        /// <param name="url">string url</param>
        /// <param name="postData">string postData</param>
        /// <returns>Returns The web server response.</returns>
        public static string WebRequest(Method method, string url, string postData, List<KeyValuePair<string, string>> headers)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value);
                }
            }
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            if (method == Method.POST || method == Method.DELETE)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                //POST the data.
                using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                    requestWriter.Close();
                }
            }
            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">HttpWebRequest webRequest [The request object.]</param>
        /// <returns>Returns The response data.</returns>
        public static string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    responseReader = new StreamReader(ex.Response.GetResponseStream());
                    //Read the response.
                    string innerResponseData = responseReader.ReadToEnd();
                    if (innerResponseData.Trim().Length > 0)
                    {
                        responseData = innerResponseData;
                    }
                }

                if (responseReader != null)
                {
                    responseReader.Close();
                }
                if (webRequest != null)
                {
                    try
                    {
                        webRequest.GetResponse().Close();
                    }
                    catch { }
                }

                throw new Exception(responseData);
            }
            catch (Exception ex)
            {
                //TODO: Improve error handling
                responseData = ex.Message;

                if (responseReader != null)
                {
                    responseReader.Close();
                }
                if (webRequest != null)
                {
                    try
                    {
                        webRequest.GetResponse().Close();
                    }
                    catch { }
                }
            }
            //Release variables.
            responseReader = null;
            webRequest = null;
            return responseData;
        }
    }

    //Generic UserData for Forms Auth Cookie
    [DataContract]
    public class UserData
    {
        [DataMember]
        public string id;
        [DataMember]
        public string username;
        [DataMember]
        public string name;
        [DataMember]
        public string serviceType;
    }
}
