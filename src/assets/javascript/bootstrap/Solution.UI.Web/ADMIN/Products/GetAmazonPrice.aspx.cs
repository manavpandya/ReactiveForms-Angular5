
using Solution.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Xml;
using MWSClientCsRuntime;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Net;



namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GetAmazonPrice : BasePage
    {

        DataTable DtSorting = new DataTable();

        SQLAccess ObjSql = new SQLAccess();
        public int currentPage = 1;
        public static bool isDescendProductName = false;
        public static bool isDescendSKU = false;
        public static bool isDescendInventory = false;
        public static bool isDescendYourPrice = false;
        public static bool isDescendMinprice = false;
        public static bool isDescendMaxprice = false;
        public static bool isDescendislowest = false;
        public static bool isDescendThresoldPrice = false;
        string timerstring = string.Empty;
        bool counter = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnsearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowAll.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                AddCoulms();
                string dd = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(LastSyncOn,'') as LastSyncOn from tb_repricerupdateinv order by LastSyncOn desc"));
                if (!string.IsNullOrEmpty(dd))
                {
                    lbllastsync.Text = "Last Inventory Sync on " + dd + " from Amazon.";
                }
                else
                {
                    lbllastsync.Text = "N/A";
                }


                //  btnShowAll_Click(null, null);
                BindGridPaging();
            }
        }

        public void AddCoulms()
        {
            if (DtSorting.Columns.Count == 0)
            {
                DtSorting.Columns.Add("SortField", typeof(string));
                DtSorting.Columns.Add("SortValue", typeof(string));
                DtSorting.AcceptChanges();
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            ViewState["CurrentPage"] = 1;
            BindGridPaging();
            //BindGrid(txtsearch.Text.Trim().Replace("'", "''"));
        }

        public string Getvendordetails(string asin)
        {
            string newstring = "";
            if (String.IsNullOrEmpty(newstring))
            {



                try
                {
                    var myUri = new Uri("http://www.amazon.com/gp/offer-listing/" + asin);
                    // Create a 'HttpWebRequest' object for the specified url. 
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    // Set the user agent as if we were a web browser
                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    var stream = myHttpWebResponse.GetResponseStream();
                    var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    string html = reader.ReadToEnd();
                    DataTable dt = new System.Data.DataTable();
                    if (html.IndexOf("<div id=\"olpOfferList\" class=\"a-section olpOfferList\">") > -1)
                    {
                        string[] strdrop = System.Text.RegularExpressions.Regex.Split(html, "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        string stt = "";
                        if (strdrop.Length > 1)
                        {
                            if (strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">") > -1)
                            {
                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">"));
                            }
                            else if (strdrop[1].IndexOf("<!-- MarkCF -->") > -1)
                            {
                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<!-- MarkCF -->"));
                            }
                            else
                            {
                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("</script>"));
                            }



                            stt = stt.Replace("a-button-stack", "a-button-stack hidediv");
                            stt = stt.Replace("olpBadgeContainer", "olpBadgeContainer hidediv");

                            stt = stt.Replace("Buying Options", "");
                            string[] allhref = System.Text.RegularExpressions.Regex.Split(html, "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                            dt = new System.Data.DataTable();
                            DataColumn col1 = new DataColumn("name", typeof(string));
                            dt.Columns.Add(col1);

                            DataColumn col2 = new DataColumn("lengthh", typeof(int));
                            dt.Columns.Add(col2);
                            if (allhref.Length > 0)
                            {
                                for (int i = 1; i < allhref.Length; i++)
                                {


                                    string strrt = allhref[i].ToString().Substring(0, allhref[i].ToString().IndexOf("\""));

                                    DataRow dr = dt.NewRow();
                                    dr["name"] = strrt;
                                    dr["lengthh"] = strrt.ToString().Length;
                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                    // stt = stt.Replace(strrt, "javascript:void(0);");
                                }
                            }

                            if (dt.Rows.Count > 0)
                            {


                                DataView dv = dt.DefaultView;
                                dv.Sort = "lengthh DESC";
                                dv.ToTable();
                                for (int i = 0; i < dv.ToTable().Rows.Count; i++)
                                {


                                    stt = stt.Replace(dv.ToTable().Rows[i][0].ToString(), "javascript:void(0);");
                                }
                            }
                            // amazonframe.InnerHtml = stt;
                            newstring = stt;

                        }

                    }






                    /////////////

                    if (html.IndexOf("<ul class=\"a-pagination\">") > -1)
                    {
                        string[] strUrl = System.Text.RegularExpressions.Regex.Split(html, "<ul class=\"a-pagination\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (strUrl.Length > 1 && strUrl[1].ToString().IndexOf("</ul>") > -1)
                        {
                            strUrl = System.Text.RegularExpressions.Regex.Split(strUrl[1].ToString(), "</ul>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        }
                        if (strUrl.Length >= 1 && strUrl[0].ToString().IndexOf("href=\"") > -1)
                        {

                            strUrl = System.Text.RegularExpressions.Regex.Split(strUrl[0].ToString(), "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            for (int iurl = 1; iurl < strUrl.Length; iurl++)
                            {


                                string strurlnew = strUrl[iurl].ToString().Substring(0, strUrl[iurl].ToString().IndexOf("\""));
                                if (strurlnew.IndexOf("_next") <= -1 && strurlnew != "#")
                                {


                                    myUri = new Uri("http://www.amazon.com" + strurlnew);
                                    // Create a 'HttpWebRequest' object for the specified url. 
                                    myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                                    // Set the user agent as if we were a web browser
                                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                                    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                    stream = myHttpWebResponse.GetResponseStream();
                                    reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                                    html = reader.ReadToEnd();
                                    dt = new System.Data.DataTable();
                                    if (html.IndexOf("<div id=\"olpOfferList\" class=\"a-section olpOfferList\">") > -1)
                                    {
                                        string[] strdrop = System.Text.RegularExpressions.Regex.Split(html, "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                        string stt = "";
                                        if (strdrop.Length > 1)
                                        {
                                            if (strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">") > -1)
                                            {
                                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">"));
                                            }
                                            else if (strdrop[1].IndexOf("<!-- MarkCF -->") > -1)
                                            {
                                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<!-- MarkCF -->"));
                                            }
                                            else
                                            {
                                                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("</script>"));
                                            }
                                            stt = stt.Replace("a-button-stack", "a-button-stack hidediv");
                                            stt = stt.Replace("olpBadgeContainer", "olpBadgeContainer hidediv");

                                            stt = stt.Replace("Buying Options", "");
                                            string[] allhref = System.Text.RegularExpressions.Regex.Split(html, "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                                            dt = new System.Data.DataTable();
                                            DataColumn col1 = new DataColumn("name", typeof(string));
                                            dt.Columns.Add(col1);

                                            DataColumn col2 = new DataColumn("lengthh", typeof(int));
                                            dt.Columns.Add(col2);
                                            if (allhref.Length > 0)
                                            {
                                                for (int i = 1; i < allhref.Length; i++)
                                                {


                                                    string strrt = allhref[i].ToString().Substring(0, allhref[i].ToString().IndexOf("\""));

                                                    DataRow dr = dt.NewRow();
                                                    dr["name"] = strrt;
                                                    dr["lengthh"] = strrt.ToString().Length;
                                                    dt.Rows.Add(dr);
                                                    dt.AcceptChanges();
                                                    // stt = stt.Replace(strrt, "javascript:void(0);");
                                                }
                                            }

                                            if (dt.Rows.Count > 0)
                                            {


                                                DataView dv = dt.DefaultView;
                                                dv.Sort = "lengthh DESC";
                                                dv.ToTable();
                                                for (int i = 0; i < dv.ToTable().Rows.Count; i++)
                                                {


                                                    stt = stt.Replace(dv.ToTable().Rows[i][0].ToString(), "javascript:void(0);");
                                                }
                                            }
                                            // amazonframe.InnerHtml += stt;
                                            newstring += stt;

                                        }

                                    }
                                }
                            }
                        }

                    }





                    // Release resources of response object.
                    myHttpWebResponse.Close();
                }
                catch (WebException ex)
                {
                    //using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                    //    html = sr.ReadToEnd();
                }
            }

            return newstring;
        }


        private void GetPricefromamazon(DataSet DsProduct)
        {
            try
            {
                string SellerId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string MarketplaceId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));
                string AccessKeyId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                string SecretKeyId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string MWSAuthToken = "";
                string ApplicationVersion = "1.0";
                string ApplicationName = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));


                for (int i = 0; i < DsProduct.Tables[0].Rows.Count; i++)
                {
                    string strbuff = string.Empty;
                    decimal total2 = 0;
                    decimal total3 = 0;
                    decimal total6 = 0;
                    decimal total7 = 0;
                    string condition = "";

                    string ShippingPrice1 = "";
                    string YourPrice = "";
                    string LowPrice = "";
                    string FulfilledBy = "";
                    string ShippingPrice2 = "";



                    MarketplaceWebServiceProductsConfig config1 = new MarketplaceWebServiceProductsConfig();
                    config1.ServiceURL = "https://mws.amazonservices.com";
                    MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient client = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient(ApplicationName,
                                                                                            ApplicationVersion,
                                                                                            AccessKeyId,
                                                                                            SecretKeyId,
                                                                                            config1);





                    MarketplaceWebServiceProducts.Model.GetMyPriceForASINRequest request2 = new MarketplaceWebServiceProducts.Model.GetMyPriceForASINRequest();
                    request2.SellerId = SellerId;
                    request2.MarketplaceId = MarketplaceId;
                    request2.MWSAuthToken = MWSAuthToken;
                    ASINListType asinListType1 = new ASINListType();
                    asinListType1.ASIN.Add(DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim());
                    request2.ASINList = asinListType1;
                    GetMyPriceForASINResponse response2 = client.GetMyPriceForASIN(request2);


                    if (response2 != null && response2.IsSetGetMyPriceForASINResult())
                    {
                        List<GetMyPriceForASINResult> getMyPriceForASINResult = response2.GetMyPriceForASINResult;
                        foreach (GetMyPriceForASINResult myPriceForASIN in getMyPriceForASINResult)
                        {
                            MarketplaceWebServiceProducts.Model.Product product = myPriceForASIN.Product;
                            if (product != null && product.IsSetOffers())
                            {
                                if (product.Offers.IsSetOffer())
                                {
                                    condition = product.Offers.Offer[0].ItemCondition;
                                    ShippingPrice1 = Convert.ToString(product.Offers.Offer[0].BuyingPrice.Shipping.Amount);
                                    YourPrice = Convert.ToString(product.Offers.Offer[0].BuyingPrice.ListingPrice.Amount);
                                    total6 = Convert.ToDecimal(product.Offers.Offer[0].BuyingPrice.Shipping.Amount + product.Offers.Offer[0].BuyingPrice.ListingPrice.Amount);
                                }
                            }
                        }
                    }


                    //GetLowestOfferListingsForASINRequest request3 = new GetLowestOfferListingsForASINRequest();
                    //request3.SellerId = SellerId;
                    //request3.MWSAuthToken = MWSAuthToken;
                    //request3.MarketplaceId = MarketplaceId;
                    //ASINListType asinList = new ASINListType();
                    //asinList.ASIN.Add(DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim());
                    //request3.ASINList = asinList;
                    //GetLowestOfferListingsForASINResponse response3 = client.GetLowestOfferListingsForASIN(request3);

                    //if (response3 != null && response3.IsSetGetLowestOfferListingsForASINResult())
                    //{
                    //    List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList = response3.GetLowestOfferListingsForASINResult;
                    //    foreach (GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in getLowestOfferListingsForASINResultList)
                    //    {
                    //        if (getLowestOfferListingsForASINResult.IsSetProduct())
                    //        {
                    //            MarketplaceWebServiceProducts.Model.Product product = getLowestOfferListingsForASINResult.Product;
                    //            bool first = true;
                    //            if (product != null && product.IsSetLowestOfferListings())
                    //            {
                    //                LowestOfferListingList lowestOfferListingList = product.LowestOfferListings;
                    //                foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList.LowestOfferListing)
                    //                {
                    //                    if (first)
                    //                    {

                    //                        total2 = lowestOfferListing.Price.ListingPrice.Amount;
                    //                        total3 = lowestOfferListing.Price.Shipping.Amount;
                    //                        total7 = Convert.ToDecimal(lowestOfferListing.Price.ListingPrice.Amount + lowestOfferListing.Price.Shipping.Amount);
                    //                        first = false;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        break;
                    //    }
                    //}







                    //if (response3 != null && response3.IsSetGetLowestOfferListingsForASINResult())
                    //{
                    //    CommonComponent.ExecuteCommonData("Delete from infovendor where ASINId='" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");

                    //    List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList = response3.GetLowestOfferListingsForASINResult;
                    //    foreach (GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in getLowestOfferListingsForASINResultList)
                    //    {
                    //        if (getLowestOfferListingsForASINResult.IsSetProduct())
                    //        {
                    //            MarketplaceWebServiceProducts.Model.Product product = getLowestOfferListingsForASINResult.Product;

                    //            if (product != null && product.IsSetLowestOfferListings())
                    //            {
                    //                int y = 0;
                    //                LowestOfferListingList lowestOfferListingList = product.LowestOfferListings;
                    //                foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList.LowestOfferListing)
                    //                {
                    //                    y++;

                    //                    Decimal vendorprice = lowestOfferListing.Price.ListingPrice.Amount;
                    //                    Decimal vendorshippoing = lowestOfferListing.Price.Shipping.Amount;
                    //                    string vendorname = "Vendor" + y;

                    //                    CommonComponent.ExecuteCommonData("insert into infovendor (ASINId,vendorid,price,shipping) values ('" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "','" + vendorname + "'," + vendorprice + "," + vendorshippoing + ")");
                    //                }
                    //            }
                    //        }

                    //    }
                    //}


                    //try
                    //{
                    //    string allstring = "";
                    //    var myUri = new Uri("http://www.amazon.com/gp/offer-listing/" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString());
                    //    // Create a 'HttpWebRequest' object for the specified url. 
                    //    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    //    // Set the user agent as if we were a web browser
                    //    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    //    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    //    var stream = myHttpWebResponse.GetResponseStream();
                    //    var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    //    string html = reader.ReadToEnd();
                    //    DataTable dt = new System.Data.DataTable();
                    //    if (html.IndexOf("<div id=\"olpOfferList\" class=\"a-section olpOfferList\">") > -1)
                    //    {
                    //        string[] strdrop = System.Text.RegularExpressions.Regex.Split(html, "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //        string stt = "";
                    //        if (strdrop.Length > 1)
                    //        {
                    //            if (strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">") > -1)
                    //            {
                    //                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">"));
                    //            }
                    //            else if (strdrop[1].IndexOf("<!-- MarkCF -->") > -1)
                    //            {
                    //                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<!-- MarkCF -->"));
                    //            }
                    //            else
                    //            {
                    //                stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("</script>"));
                    //            }



                    //            stt = stt.Replace("a-button-stack", "a-button-stack hidediv");
                    //            stt = stt.Replace("olpBadgeContainer", "olpBadgeContainer hidediv");

                    //            stt = stt.Replace("Buying Options", "");
                    //            string[] allhref = System.Text.RegularExpressions.Regex.Split(html, "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    //            dt = new System.Data.DataTable();
                    //            DataColumn col1 = new DataColumn("name", typeof(string));
                    //            dt.Columns.Add(col1);

                    //            DataColumn col2 = new DataColumn("lengthh", typeof(int));
                    //            dt.Columns.Add(col2);
                    //            if (allhref.Length > 0)
                    //            {
                    //                for (int ik = 1; ik < allhref.Length; ik++)
                    //                {


                    //                    string strrt = allhref[ik].ToString().Substring(0, allhref[ik].ToString().IndexOf("\""));

                    //                    DataRow dr = dt.NewRow();
                    //                    dr["name"] = strrt;
                    //                    dr["lengthh"] = strrt.ToString().Length;
                    //                    dt.Rows.Add(dr);
                    //                    dt.AcceptChanges();
                    //                    // stt = stt.Replace(strrt, "javascript:void(0);");
                    //                }
                    //            }

                    //            if (dt.Rows.Count > 0)
                    //            {


                    //                DataView dv = dt.DefaultView;
                    //                dv.Sort = "lengthh DESC";
                    //                dv.ToTable();
                    //                for (int ii = 0; ii < dv.ToTable().Rows.Count; ii++)
                    //                {


                    //                    stt = stt.Replace(dv.ToTable().Rows[ii][0].ToString(), "javascript:void(0);");
                    //                }
                    //            }



                    //            allstring = stt;





                    //        }






                    //    }


                    //if (html.IndexOf("<ul class=\"a-pagination\">") > -1)
                    //{
                    //    string[] strUrl = System.Text.RegularExpressions.Regex.Split(html, "<ul class=\"a-pagination\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //    if (strUrl.Length > 1 && strUrl[1].ToString().IndexOf("</ul>") > -1)
                    //    {
                    //        strUrl = System.Text.RegularExpressions.Regex.Split(strUrl[1].ToString(), "</ul>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //    }
                    //    if (strUrl.Length >= 1 && strUrl[0].ToString().IndexOf("href=\"") > -1)
                    //    {

                    //        strUrl = System.Text.RegularExpressions.Regex.Split(strUrl[0].ToString(), "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //        for (int iurl = 1; iurl < strUrl.Length; iurl++)
                    //        {


                    //            string strurlnew = strUrl[iurl].ToString().Substring(0, strUrl[iurl].ToString().IndexOf("\""));
                    //            if (strurlnew.IndexOf("_next") <= -1 && strurlnew != "#")
                    //            {


                    //                myUri = new Uri("http://www.amazon.com" + strurlnew);
                    //                // Create a 'HttpWebRequest' object for the specified url. 
                    //                myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    //                // Set the user agent as if we were a web browser
                    //                myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    //                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    //                stream = myHttpWebResponse.GetResponseStream();
                    //                reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    //                html = reader.ReadToEnd();
                    //                dt = new System.Data.DataTable();
                    //                if (html.IndexOf("<div id=\"olpOfferList\" class=\"a-section olpOfferList\">") > -1)
                    //                {
                    //                    string[] strdrop = System.Text.RegularExpressions.Regex.Split(html, "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    //                    string stt = "";
                    //                    if (strdrop.Length > 1)
                    //                    {
                    //                        if (strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">") > -1)
                    //                        {
                    //                            stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<div class=\"a-text-center a-spacing-large\">"));
                    //                        }
                    //                        else if (strdrop[1].IndexOf("<!-- MarkCF -->") > -1)
                    //                        {
                    //                            stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("<!-- MarkCF -->"));
                    //                        }
                    //                        else
                    //                        {
                    //                            stt = "<div id=\"olpOfferList\" class=\"a-section olpOfferList\">" + strdrop[1].Substring(0, strdrop[1].IndexOf("</script>"));
                    //                        }
                    //                        stt = stt.Replace("a-button-stack", "a-button-stack hidediv");
                    //                        stt = stt.Replace("olpBadgeContainer", "olpBadgeContainer hidediv");

                    //                        stt = stt.Replace("Buying Options", "");
                    //                        string[] allhref = System.Text.RegularExpressions.Regex.Split(html, "href=\"", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    //                        dt = new System.Data.DataTable();
                    //                        DataColumn col1 = new DataColumn("name", typeof(string));
                    //                        dt.Columns.Add(col1);

                    //                        DataColumn col2 = new DataColumn("lengthh", typeof(int));
                    //                        dt.Columns.Add(col2);
                    //                        if (allhref.Length > 0)
                    //                        {
                    //                            for (int iu = 1; iu < allhref.Length; iu++)
                    //                            {


                    //                                string strrt = allhref[iu].ToString().Substring(0, allhref[iu].ToString().IndexOf("\""));

                    //                                DataRow dr = dt.NewRow();
                    //                                dr["name"] = strrt;
                    //                                dr["lengthh"] = strrt.ToString().Length;
                    //                                dt.Rows.Add(dr);
                    //                                dt.AcceptChanges();
                    //                                // stt = stt.Replace(strrt, "javascript:void(0);");
                    //                            }
                    //                        }

                    //                        if (dt.Rows.Count > 0)
                    //                        {


                    //                            DataView dv = dt.DefaultView;
                    //                            dv.Sort = "lengthh DESC";
                    //                            dv.ToTable();
                    //                            for (int im = 0; im < dv.ToTable().Rows.Count; im++)
                    //                            {


                    //                                stt = stt.Replace(dv.ToTable().Rows[im][0].ToString(), "javascript:void(0);");
                    //                            }
                    //                        }
                    //                        allstring += stt;

                    //                    }

                    //                }
                    //            }
                    //        }
                    //    }

                    //}

                    int fl = 0;
                    string allstring = "";

                    for (fl = 0; fl <= 5; fl++)
                    {
                        if (String.IsNullOrEmpty(allstring))
                        {
                            allstring = Getvendordetails(DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim());
                        }
                    }



                    //if (!String.IsNullOrEmpty(allstring))
                    //{
                    CommonComponent.ExecuteCommonData("update ItemInfoes set Vendordetail='" + allstring.ToString().Replace("'", "''") + "' where ASINId='" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");
                    // }
                    DataTable dtt = new DataTable();
                    dtt.Columns.Add("Price", typeof(string));
                    dtt.Columns.Add("Shipping", typeof(string));
                    dtt.Columns.Add("Total", typeof(Decimal));
                    dtt.AcceptChanges();

                    try
                    {
                        string[] strprices = System.Text.RegularExpressions.Regex.Split(allstring, "<div class=\"a-row a-spacing-mini olpOffer\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (strprices.Length > 0)
                        {
                            for (int k = 1; k < strprices.Length; k++)
                            {
                                DataRow dr = dtt.NewRow();

                                string price = "";
                                string shipping = "";
                                if (strprices[k].ToString().IndexOf("a-column a-span2") > -1)
                                {
                                    string[] pp = System.Text.RegularExpressions.Regex.Split(strprices[k], "<span ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                    if (pp.Length > 0)
                                    {
                                        for (int j = 1; j < pp.Length; j++)
                                        {

                                            if (pp[j].ToString().IndexOf("class=\"a-size-large a-color-price olpOfferPrice a-text-bold") > -1)
                                            {
                                                price = pp[j].ToString().Replace("class=\"a-size-large a-color-price olpOfferPrice a-text-bold\">", "").Replace("<p class=\"olpShippingInfo\">", "").Replace("</span>", "").Replace("$", "").Replace(System.Environment.NewLine, "").Trim();

                                            }
                                            else if (pp[j].ToString().IndexOf("class=\"a-color-secondary") > -1)
                                            {
                                                shipping = pp[j].ToString().Replace("class=\"a-color-secondary\">", "").Replace("</span>", "").Replace("$", "");
                                                if (shipping.ToString().ToLower().IndexOf("free shipping") > -1)
                                                {
                                                    shipping = "0";
                                                    break;
                                                }

                                            }
                                            else if (pp[j].ToString().IndexOf("class=\"olpShippingPrice\">") > -1)
                                            {
                                                shipping = pp[j].ToString().Replace("class=\"olpShippingPrice\">", "").Replace("</span>", "").Replace("$", "");
                                                // string[] bb = System.Text.RegularExpressions.Regex.Split(shipping, "class=\"olpShippingPrice\">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                                // if (bb.Length > 0)
                                                {
                                                    shipping = shipping.ToString().Replace("$", "").Replace(System.Environment.NewLine, "").Trim();
                                                    break;
                                                }

                                            }


                                        }
                                    }

                                    if (!String.IsNullOrEmpty(price) && !String.IsNullOrEmpty(shipping))
                                    {
                                        dr["Price"] = price;
                                        dr["Shipping"] = shipping;
                                        dr["Total"] = Convert.ToDecimal(price) + Convert.ToDecimal(shipping);
                                        dtt.Rows.Add(dr);
                                        dtt.AcceptChanges();
                                    }

                                }

                            }
                        }

                        if (dtt.Rows.Count > 0)
                        {
                            CommonComponent.ExecuteCommonData("Delete from infovendor where ASINId='" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");

                            for (int u = 0; u < dtt.Rows.Count; u++)
                            {


                                Decimal vendorprice = Convert.ToDecimal(dtt.Rows[u]["Price"].ToString());
                                Decimal vendorshippoing = Convert.ToDecimal(dtt.Rows[u]["Shipping"].ToString());
                                string vendorname = "Vendor" + (u + 1);

                                CommonComponent.ExecuteCommonData("if(NOT EXISTS(SELECT Top 1 ASINID FROM  infovendor WHERE ASINId='" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "' and Price='" + vendorprice + "' and shipping='" + vendorshippoing + "')) begin insert into infovendor (ASINId,vendorid,price,shipping) values ('" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "','" + vendorname + "'," + vendorprice + "," + vendorshippoing + ") end");
                            }

                            DataView dv = dtt.DefaultView;
                            dv.Sort = "Total ASC";
                            dv.ToTable();
                            LowPrice = dv.ToTable().Rows[0]["Price"].ToString();
                            ShippingPrice2 = dv.ToTable().Rows[0]["Shipping"].ToString();
                            total7 = Convert.ToDecimal(LowPrice) + Convert.ToDecimal(ShippingPrice2);

                        }
                    }
                    catch
                    {
                        LowPrice = "";
                        ShippingPrice2 = "";
                    }




                    // Release resources of response object.
                    //    myHttpWebResponse.Close();
                    //}
                    //catch (WebException ex)
                    //{
                    //    //using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                    //    //    html = sr.ReadToEnd();
                    //}






                    if (String.IsNullOrEmpty(LowPrice) && String.IsNullOrEmpty(ShippingPrice2))
                    {

                        GetLowestOfferListingsForASINRequest request3 = new GetLowestOfferListingsForASINRequest();
                        request3.SellerId = SellerId;
                        request3.MWSAuthToken = MWSAuthToken;
                        request3.MarketplaceId = MarketplaceId;
                        ASINListType asinList = new ASINListType();
                        asinList.ASIN.Add(DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim());
                        request3.ASINList = asinList;
                        GetLowestOfferListingsForASINResponse response3 = client.GetLowestOfferListingsForASIN(request3);

                        if (response3 != null && response3.IsSetGetLowestOfferListingsForASINResult())
                        {
                            List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList = response3.GetLowestOfferListingsForASINResult;
                            foreach (GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in getLowestOfferListingsForASINResultList)
                            {
                                if (getLowestOfferListingsForASINResult.IsSetProduct())
                                {
                                    MarketplaceWebServiceProducts.Model.Product product = getLowestOfferListingsForASINResult.Product;
                                    bool first = true;
                                    if (product != null && product.IsSetLowestOfferListings())
                                    {
                                        LowestOfferListingList lowestOfferListingList = product.LowestOfferListings;
                                        foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList.LowestOfferListing)
                                        {
                                            if (first)
                                            {

                                                total2 = lowestOfferListing.Price.ListingPrice.Amount;
                                                total3 = lowestOfferListing.Price.Shipping.Amount;
                                                total7 = Convert.ToDecimal(lowestOfferListing.Price.ListingPrice.Amount + lowestOfferListing.Price.Shipping.Amount);
                                                if (total2 == 0)
                                                {
                                                    //LowPrice = "NA";
                                                    LowPrice = Convert.ToString(0);
                                                }
                                                else
                                                {
                                                    LowPrice = Convert.ToString(total2);
                                                }
                                                ShippingPrice2 = Convert.ToString(total3);
                                                first = false;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }


                        if (response3 != null && response3.IsSetGetLowestOfferListingsForASINResult())
                        {
                            CommonComponent.ExecuteCommonData("Delete from infovendor where ASINId='" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");

                            List<GetLowestOfferListingsForASINResult> getLowestOfferListingsForASINResultList = response3.GetLowestOfferListingsForASINResult;
                            foreach (GetLowestOfferListingsForASINResult getLowestOfferListingsForASINResult in getLowestOfferListingsForASINResultList)
                            {
                                if (getLowestOfferListingsForASINResult.IsSetProduct())
                                {
                                    MarketplaceWebServiceProducts.Model.Product product = getLowestOfferListingsForASINResult.Product;

                                    if (product != null && product.IsSetLowestOfferListings())
                                    {
                                        int y = 0;
                                        LowestOfferListingList lowestOfferListingList = product.LowestOfferListings;
                                        foreach (LowestOfferListingType lowestOfferListing in lowestOfferListingList.LowestOfferListing)
                                        {
                                            y++;

                                            Decimal vendorprice = lowestOfferListing.Price.ListingPrice.Amount;
                                            Decimal vendorshippoing = lowestOfferListing.Price.Shipping.Amount;
                                            string vendorname = "Vendor" + y;

                                            CommonComponent.ExecuteCommonData("insert into infovendor (ASINId,vendorid,price,shipping) values ('" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "','" + vendorname + "'," + vendorprice + "," + vendorshippoing + ")");
                                        }
                                    }
                                }

                            }
                        }

                    }

                    if (Convert.ToDecimal(total6) > total7)
                    {
                        FulfilledBy = "No";
                    }
                    else
                    {
                        FulfilledBy = "Yes";
                    }




                    CommonComponent.ExecuteCommonData("update ItemInfoes set Condition='" + condition + "', ShippingPrice1=" + ShippingPrice1 + ",YourPrice=" + YourPrice + ",LowPrice=" + LowPrice + ",FulfilledBy='" + FulfilledBy + "',ShippingPrice2=" + ShippingPrice2 + " where id=" + DsProduct.Tables[0].Rows[i]["id"].ToString() + "");
                    CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiUpdateAmazonRepriceThresoldPrice_Page '" + DsProduct.Tables[0].Rows[i]["ASINId"].ToString().Trim() + "'");
                }
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete2", "jAlert('Price Not Refreshed,Contact administrator','Message');", true);
            }
        }



        public void generatePager(int totalRowCount, int pageSize, int currentPage)
        {
            int totalLinkInPage = 5;
            int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

            int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
            int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

            if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
            {
                lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
                startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
            }

            List<ListItem> pageLinkContainer = new List<ListItem>();

            if (startPageLink != 1)
                pageLinkContainer.Add(new ListItem("First", "1", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new ListItem("Last", totalPageCount.ToString(), currentPage != totalPageCount));

            dlPager.DataSource = pageLinkContainer;
            dlPager.DataBind();
        }


        public void BindGridPaging()
        {
            String Condition = "";
            if (drpautoprice.SelectedValue.ToString() != "")
            {
                Condition = "  isnull(IsServiceupdate,0)=" + drpautoprice.SelectedValue.ToString() + " ";
            }
            if (drplowest.SelectedValue.ToString() != "")
            {
                if (String.IsNullOrEmpty(Condition))
                {
                    Condition += "  isnull(FulfilledBy,'''')='" + drplowest.SelectedValue.ToString() + "' ";

                }
                else
                {
                    Condition += " and isnull(FulfilledBy,'''')='" + drplowest.SelectedValue.ToString() + "' ";
                }

            }

            int pageSize = 25;
            int _TotalRowCount = 0;

            DataSet DsProduct = new DataSet();

            SqlDataAdapter Adpt = new SqlDataAdapter();

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
            SqlCommand cmd = new SqlCommand("GuiGetAmazonRepriceDetails", conn);
            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                int startRowNumber = ((currentPage - 1) * pageSize) + 1;
                cmd.Parameters.AddWithValue("@Search", txtsearch.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("@StartIndex", startRowNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@SortString", lblOrder.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("@Condition", Condition);
                SqlParameter parTotalCount = new SqlParameter("@TotalCount", SqlDbType.Int);
                parTotalCount.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parTotalCount);
                cmd.CommandTimeout = 10000;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(DsProduct);
                _TotalRowCount = Convert.ToInt32(parTotalCount.Value);
                ViewState["GridData"] = (DataSet)DsProduct;
                grdamazon.DataSource = DsProduct;
                grdamazon.DataBind();

                generatePager(_TotalRowCount, pageSize, currentPage);
            }
            catch (Exception ex)
            {

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
                Adpt.Dispose();

            }




        }
        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                currentPage = Convert.ToInt32(e.CommandArgument);
                ViewState["CurrentPage"] = currentPage;
                BindGridPaging();
            }
        }



        protected void grdamazon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region set property for sorting
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendProductName == false)
                {
                    ImageButton btnsortname = (ImageButton)e.Row.FindControl("btnsortname");
                    btnsortname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortname.AlternateText = "Ascending Order";
                    btnsortname.ToolTip = "Ascending Order";
                    btnsortname.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortname = (ImageButton)e.Row.FindControl("btnsortname");
                    btnsortname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortname.AlternateText = "Descending Order";
                    btnsortname.ToolTip = "Descending Order";
                    btnsortname.CommandArgument = "ASC";
                }
                if (isDescendSKU == false)
                {
                    ImageButton btnsortsku = (ImageButton)e.Row.FindControl("btnsortsku");
                    btnsortsku.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortsku.AlternateText = "Ascending Order";
                    btnsortsku.ToolTip = "Ascending Order";
                    btnsortsku.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortsku = (ImageButton)e.Row.FindControl("btnsortsku");
                    btnsortsku.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortsku.AlternateText = "Descending Order";
                    btnsortsku.ToolTip = "Descending Order";
                    btnsortsku.CommandArgument = "ASC";
                }
                if (isDescendMaxprice == false)
                {
                    ImageButton btnsortmaxprice = (ImageButton)e.Row.FindControl("btnsortmaxprice");
                    btnsortmaxprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortmaxprice.AlternateText = "Ascending Order";
                    btnsortmaxprice.ToolTip = "Ascending Order";
                    btnsortmaxprice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortmaxprice = (ImageButton)e.Row.FindControl("btnsortmaxprice");
                    btnsortmaxprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortmaxprice.AlternateText = "Descending Order";
                    btnsortmaxprice.ToolTip = "Descending Order";
                    btnsortmaxprice.CommandArgument = "ASC";
                }
                if (isDescendMinprice == false)
                {
                    ImageButton btnsortminprice = (ImageButton)e.Row.FindControl("btnsortminprice");
                    btnsortminprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortminprice.AlternateText = "Ascending Order";
                    btnsortminprice.ToolTip = "Ascending Order";
                    btnsortminprice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortminprice = (ImageButton)e.Row.FindControl("btnsortminprice");
                    btnsortminprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortminprice.AlternateText = "Descending Order";
                    btnsortminprice.ToolTip = "Descending Order";
                    btnsortminprice.CommandArgument = "ASC";
                }
                if (isDescendislowest == false)
                {
                    ImageButton btnsortislowest = (ImageButton)e.Row.FindControl("btnsortislowest");
                    btnsortislowest.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortislowest.AlternateText = "Ascending Order";
                    btnsortislowest.ToolTip = "Ascending Order";
                    btnsortislowest.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortislowest = (ImageButton)e.Row.FindControl("btnsortislowest");
                    btnsortislowest.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortislowest.AlternateText = "Descending Order";
                    btnsortislowest.ToolTip = "Descending Order";
                    btnsortislowest.CommandArgument = "ASC";
                }

                if (isDescendYourPrice == false)
                {
                    ImageButton btnsortyourprice = (ImageButton)e.Row.FindControl("btnsortyourprice");
                    btnsortyourprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortyourprice.AlternateText = "Ascending Order";
                    btnsortyourprice.ToolTip = "Ascending Order";
                    btnsortyourprice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortyourprice = (ImageButton)e.Row.FindControl("btnsortyourprice");
                    btnsortyourprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortyourprice.AlternateText = "Descending Order";
                    btnsortyourprice.ToolTip = "Descending Order";
                    btnsortyourprice.CommandArgument = "ASC";
                }

                if (isDescendThresoldPrice == false)
                {
                    ImageButton btnsortformulaprice = (ImageButton)e.Row.FindControl("btnsortformulaprice");
                    btnsortformulaprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnsortformulaprice.AlternateText = "Ascending Order";
                    btnsortformulaprice.ToolTip = "Ascending Order";
                    btnsortformulaprice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnsortformulaprice = (ImageButton)e.Row.FindControl("btnsortformulaprice");
                    btnsortformulaprice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnsortformulaprice.AlternateText = "Descending Order";
                    btnsortformulaprice.ToolTip = "Descending Order";
                    btnsortformulaprice.CommandArgument = "ASC";
                }
                OrderBy objOrderBy = (OrderBy)ViewState["Sort"];

                if (objOrderBy == null)
                {

                    objOrderBy = new OrderBy();

                }

                if (objOrderBy.hshOrderExpressions.Count > 0)
                {

                    var orderbyClause = objOrderBy.hshOrderPosition.Cast<DictionaryEntry>().OrderBy(entry => entry.Value);
                    Literal ltrsortsku = (Literal)e.Row.FindControl("ltrsortsku");
                    Literal ltrsortname = (Literal)e.Row.FindControl("ltrsortname");
                    Literal ltrsortmaxprice = (Literal)e.Row.FindControl("ltrsortmaxprice");
                    Literal ltrsortminprice = (Literal)e.Row.FindControl("ltrsortminprice");
                    Literal ltrsortislowest = (Literal)e.Row.FindControl("ltrsortislowest");
                    Literal ltrsortyourprice = (Literal)e.Row.FindControl("ltrsortyourprice");
                    Literal ltrsortformulaprice = (Literal)e.Row.FindControl("ltrsortformulaprice");

                    foreach (DictionaryEntry clause in orderbyClause)
                    {

                        string key = Convert.ToString(clause.Key);
                        if (key.ToUpper() == "SKUID")
                        {
                            ltrsortsku.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "PRODUCTNAME")
                        {
                            ltrsortname.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "MAXPRICE")
                        {
                            ltrsortmaxprice.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "MINPRICE")
                        {
                            ltrsortminprice.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "FULFILLEDBY")
                        {
                            ltrsortislowest.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "YOURPRICE")
                        {
                            ltrsortyourprice.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }
                        else if (key.ToUpper() == "THRESOLDPRICE")
                        {
                            ltrsortformulaprice.Text = "<a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"12\" alt=\"Remove Filter\" title=\"Remove Filter\" height=\"10\" style=\"cursor:pointer;\" src=\"/images/delete_ico.png\"></a>";
                        }


                    }



                }



            }
            #endregion
            if (e.Row.RowType == DataControlRowType.Header)
            {
                timerstring = "";
                ViewState["timerstring"] = timerstring;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // timerstring += "});";

                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Scrollable" + e.Row.RowIndex.ToString() + "", "$(function(){" + ViewState["timerstring"].ToString() + "}); ", true);//$('#ContentPlaceHolder1_grdamazon').Scrollable(); 
                // Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Scrollable_" + e.Row.RowIndex.ToString() + "", "var $z = jQuery.noConflict();$z(function(){$z('#ContentPlaceHolder1_grdamazon').Scrollable(); });", true);//

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label lblSkuID = (Label)e.Row.FindControl("lblSkuID");
                    Label lblASINId = (Label)e.Row.FindControl("lblASINId");
                    System.Web.UI.HtmlControls.HtmlAnchor avendor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("avendor");
                    Label lblYourPrice = (Label)e.Row.FindControl("lblYourPrice");
                    Label lblThresoldPrice = (Label)e.Row.FindControl("lblThresoldPrice");
                    Label lblFulfilledBy = (Label)e.Row.FindControl("lblFulfilledBy");
                    Label lblminprice = (Label)e.Row.FindControl("lblminprice");
                    TextBox txtminprice = (TextBox)e.Row.FindControl("txtminprice");
                    System.Web.UI.HtmlControls.HtmlAnchor aminprice = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("aminprice");
                    HiddenField hdncustom = (HiddenField)e.Row.FindControl("hdncustom");
                    System.Web.UI.HtmlControls.HtmlGenericControl timer = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("timer");
                    ImageButton updateprice = (ImageButton)e.Row.FindControl("updateprice");
                    ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                    Label lbllastupdate = (Label)e.Row.FindControl("lbllastupdate");
                    Label lbllogdate = (Label)e.Row.FindControl("lbllogdate");
                    //if (counter==false)
                    //{
                    lbllastupdate.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 LogDate from tb_repricerlogdetails where Asinid='" + lblASINId.Text.ToString() + "' and StatusID=3 order by LogDate Desc"));
                    if (!String.IsNullOrEmpty(lbllastupdate.Text))
                    {
                        lbllastupdate.Text = "Last Updated On :" + lbllastupdate.Text;
                    }


                    if (!string.IsNullOrEmpty(lbllogdate.Text) && Convert.ToInt32(lbllogdate.Text) > 0)
                    {
                        updateprice.Attributes.Add("style", "display:none;");
                        _editLinkButton.Attributes.Add("style", "display:none;");

                        timerstring += "$('#" + timer.ClientID.ToString() + "').countdowntimer({minutes : " + lbllogdate.Text.ToString() + ",size : 'lg',timeUp : timeisUp" + timer.ClientID.ToString() + "});function timeisUp" + timer.ClientID.ToString() + "() {$('#" + updateprice.ClientID.ToString() + "').show();$('#" + _editLinkButton.ClientID.ToString() + "').show();$('#" + timer.ClientID.ToString() + "').hide();} ";
                        ViewState["timerstring"] = timerstring;

                    }
                    else
                    {
                        timer.Attributes.Add("style", "display:none;");
                    }
                    //  }

                    // avendor.Attributes.Add("onclick", "OpenCenterWindow('/Admin/Products/viewamazonrepricevendor.aspx?asin=" + lblASINId.Text.ToString() + "',900,600);");
                    avendor.HRef = "/Admin/Products/viewamazonrepricevendor.aspx?asin=" + lblASINId.Text.ToString();
                    aminprice.HRef = "/Admin/Products/viewminvendorprice.aspx?asin=" + lblASINId.Text.ToString() + "&cp=" + currentPage;
                    if (hdncustom.Value.ToString().ToLower() == "true" || hdncustom.Value.ToString().ToLower() == "1")
                    {
                        aminprice.Attributes.Add("style", "text-decoration:none;color:green");
                        lblminprice.ForeColor = System.Drawing.Color.Green;
                    }

                    Label lblamazonstatus = (Label)e.Row.FindControl("lblamazonstatus");
                    if (lblamazonstatus.Text.ToString().ToLower() == "active")
                    {
                        lblamazonstatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblamazonstatus.ForeColor = System.Drawing.Color.Red;
                    }
                    Label lblwebsitestatus = (Label)e.Row.FindControl("lblwebsitestatus");
                    if (lblwebsitestatus.Text.ToString().ToLower() == "active")
                    {
                        lblwebsitestatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblwebsitestatus.ForeColor = System.Drawing.Color.Red;
                    }

                    Decimal vendorlowest = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Exec GuiGetAmazonRepriceVendorDetails '" + lblASINId.Text.ToString() + "',2"));
                    Label lblvendorlowest = (Label)e.Row.FindControl("lblvendorlowest");
                    if (vendorlowest > Decimal.Zero)
                    {
                        lblvendorlowest.Text = String.Format("{0:0.00}", Convert.ToDecimal(vendorlowest));
                    }
                    else
                    {
                        lblvendorlowest.Text = String.Format("{0:0.00}", Convert.ToDecimal(lblYourPrice.Text.ToString().Replace("$", "").Trim()));
                    }


                    ImageButton btnSave = (ImageButton)e.Row.FindControl("btnSave");

                    ImageButton btnCancel = (ImageButton)e.Row.FindControl("btnCancel");
                    ImageButton getprice = (ImageButton)e.Row.FindControl("getprice");

                    CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + lblASINId.Text.ToString() + "'");
                    Decimal MinPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(MinPrice,0) as Price from ItemInfoes where  isnull(ASINId,'')='" + lblASINId.Text.ToString() + "'"));
                    lblminprice.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(MinPrice.ToString()));
                    txtminprice.Text = String.Format("{0:0.00}", Convert.ToDecimal(MinPrice.ToString()));


                    CommonComponent.ExecuteCommonData("Exec GuiUpdateAmazonRepriceThresoldPrice_Page '" + lblASINId.Text.ToString() + "'");
                    Decimal ThresoldP = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(ThresoldPrice,0) as Price from ItemInfoes where  isnull(ASINId,'')='" + lblASINId.Text.ToString() + "'"));
                    lblThresoldPrice.Text = "$" + String.Format("{0:0.00}", Convert.ToDecimal(ThresoldP.ToString()));
                    //  Decimal websitePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(websitePrice,0) as Price from ItemInfoes where  isnull(ASINId,'')='" + lblASINId.Text.ToString() + "'"));

                    _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/edit-price.gif";
                    btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/save.png";
                    btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/CloseIcon.png";

                    updateprice.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/updatepriceinamazon.png";
                    getprice.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/refresh.png";
                    System.Web.UI.HtmlControls.HtmlImage imagstatus = e.Row.FindControl("imagstatus") as System.Web.UI.HtmlControls.HtmlImage;
                    if (lblFulfilledBy.Text.ToString().ToLower() == "yes")
                    {
                        imagstatus.Src = "/Admin/images/isActive.png";
                    }
                    else
                    {
                        imagstatus.Src = "/Admin/images/isInactive.png";
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message.ToString() + "" + ex.StackTrace.ToString());
                }
            }

        }
        /// <summary>
        /// Get Image
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public String GetIconImageProduct(String img, string StoreId)
        {
            try
            {
                string imagepathfull = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ConfigValue FROM tb_appConfig WHERE isnull(deleted,0)=0 and ConfigName='ImagePathProduct' and StoreId=1"));
                String imagepath = String.Empty;
                //if (ViewState["ImagePathProduct"] != null)
                //{
                //    imagepath = ViewState["ImagePathProduct"].ToString() + "medium/" + img;
                //}
                //else
                {
                    imagepath = imagepathfull + "micro/" + img;
                }

                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath;
                }


                return string.Concat(imagepathfull + "micro/image_not_available.jpg");
            }
            catch (Exception Ex)
            {

                //throw Ex;
            }
            return null;
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtsearch.Text = "";
            grdamazon.PageIndex = 0;
            currentPage = 1;
            ViewState["CurrentPage"] = 1;
            BindGridPaging();

        }

        protected void grdamazon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdamazon.PageIndex = e.NewPageIndex;
            //BindGrid(txtsearch.Text.Trim().Replace("'", "''"));
        }

        protected void grdamazon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow gvrow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                ImageButton btnSave = gvrow.FindControl("btnSave") as ImageButton;
                ImageButton btnCancel = gvrow.FindControl("btnCancel") as ImageButton;
                ImageButton btnEditPrice = gvrow.FindControl("_editLinkButton") as ImageButton;

                TextBox txtSkuID = (TextBox)gvrow.FindControl("txtSkuID");
                Label lblSkuID = (Label)gvrow.FindControl("lblSkuID");

                TextBox txtCondition = (TextBox)gvrow.FindControl("txtCondition");
                Label lblCondition = (Label)gvrow.FindControl("lblCondition");

                TextBox txtProductName = (TextBox)gvrow.FindControl("txtProductName");
                Label lblProductName = (Label)gvrow.FindControl("lblProductName");

                TextBox txtASINId = (TextBox)gvrow.FindControl("txtASINId");
                Label lblASINId = (Label)gvrow.FindControl("lblASINId");

                TextBox txtYourPrice = (TextBox)gvrow.FindControl("txtYourPrice");
                Label lblYourPrice = (Label)gvrow.FindControl("lblYourPrice");

                TextBox txtShippingPrice1 = (TextBox)gvrow.FindControl("txtShippingPrice1");
                Label lblShippingPrice1 = (Label)gvrow.FindControl("lblShippingPrice1");

                TextBox txtLowPrice = (TextBox)gvrow.FindControl("txtLowPrice");
                Label lblLowPrice = (Label)gvrow.FindControl("lblLowPrice");

                TextBox txtShippingPrice2 = (TextBox)gvrow.FindControl("txtShippingPrice2");
                Label lblShippingPrice2 = (Label)gvrow.FindControl("lblShippingPrice2");

                TextBox txtFulfilledBy = (TextBox)gvrow.FindControl("txtFulfilledBy");
                Label lblFulfilledBy = (Label)gvrow.FindControl("lblFulfilledBy");
                TextBox txtThresoldPrice = (TextBox)gvrow.FindControl("txtThresoldPrice");

                Label lblThresoldPrice = (Label)gvrow.FindControl("lblThresoldPrice");
                Label lblThresoldsubstract = (Label)gvrow.FindControl("lblThresoldsubstract");
                TextBox txtThresoldsubstract = (TextBox)gvrow.FindControl("txtThresoldsubstract");



                TextBox txtmaxprice = (TextBox)gvrow.FindControl("txtmaxprice");
                Label lblmaxprice = (Label)gvrow.FindControl("lblmaxprice");
                Label lblminprice = (Label)gvrow.FindControl("lblminprice");
                TextBox txtminprice = (TextBox)gvrow.FindControl("txtminprice");


                CheckBox chkuseformula = (CheckBox)gvrow.FindControl("chkuseformula");

                CheckBox chkIsServiceupdate = (CheckBox)gvrow.FindControl("chkIsServiceupdate");
                if (e.CommandName.ToLower() == "getprice")
                {


                    String Skuid = Convert.ToString(e.CommandArgument.ToString());
                    DataSet DsProduct = CommonComponent.GetCommonDataSet("Exec GuiGetAmazonRepriceDetails_SKU '" + Skuid + "'");
                    if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                    {

                        GetPricefromamazon(DsProduct);

                    }
                    if (ViewState["CurrentPage"] != null)
                    {
                        currentPage = Convert.ToInt32(ViewState["CurrentPage"].ToString());
                    }
                    else
                    {
                        currentPage = 1;
                        ViewState["CurrentPage"] = currentPage;
                    }
                    BindGridPaging();


                    //if (!String.IsNullOrEmpty(Skuid))
                    //{
                    //    BindGrid(Skuid);
                    //    BindGrid("");
                    //}
                    //else
                    //{
                    //    BindGrid("");
                    //}
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Price Refreshed Successfully.','Message');", true);
                }
                if (e.CommandName.ToLower() == "sort")
                {




                }
                if (e.CommandName.ToLower() == "updateprice")
                {
                    int formulaPrice = 0;
                    if (chkuseformula.Checked)
                    {
                        formulaPrice = 1;
                    }
                    else
                    {
                        formulaPrice = 0;

                    }
                    String Skuid = Convert.ToString(e.CommandArgument.ToString());
                    updatePrice(Skuid, formulaPrice, lblASINId.Text.ToString());

                    if (ViewState["CurrentPage"] != null)
                    {
                        currentPage = Convert.ToInt32(ViewState["CurrentPage"].ToString());
                    }
                    else
                    {
                        currentPage = 1;
                        ViewState["CurrentPage"] = currentPage;
                    }
                    BindGridPaging();

                }
                else if (e.CommandName == "edit")
                {
                    try
                    {
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEditPrice.Visible = false;

                        txtYourPrice.Visible = true;
                        lblYourPrice.Visible = false;
                        txtShippingPrice1.Visible = true;
                        lblShippingPrice1.Visible = false;

                        lblThresoldsubstract.Visible = false;
                        txtThresoldsubstract.Visible = true;


                        txtmaxprice.Visible = true;
                        lblmaxprice.Visible = false;
                        lblminprice.Visible = true;
                        //txtminprice.Visible = true;
                        chkuseformula.Enabled = true;
                        chkIsServiceupdate.Enabled = true;
                    }
                    catch
                    { }
                }

                else if (e.CommandName == "Cancel")
                {
                    btnEditPrice.Visible = true;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;


                    txtSkuID.Text = lblSkuID.Text.ToString();
                    txtCondition.Text = lblCondition.Text.ToString();
                    txtProductName.Text = lblProductName.Text.ToString();
                    txtASINId.Text = lblASINId.Text.ToString();
                    txtYourPrice.Text = lblYourPrice.Text.ToString().Replace("$", "").Trim();
                    txtShippingPrice1.Text = lblShippingPrice1.Text.ToString().Replace("$", "").Trim();
                    txtLowPrice.Text = lblLowPrice.Text.ToString().Replace("$", "").Trim();

                    txtShippingPrice2.Text = lblShippingPrice2.Text.ToString().Replace("$", "").Trim();
                    txtFulfilledBy.Text = lblFulfilledBy.Text.ToString();
                    txtThresoldPrice.Text = lblThresoldPrice.Text.ToString().Replace("$", "").Trim();
                    txtThresoldsubstract.Text = lblThresoldsubstract.Text.ToString().Replace("$", "").Trim();

                    txtmaxprice.Text = lblmaxprice.Text.ToString().Replace("$", "").Trim();
                    txtminprice.Text = lblminprice.Text.ToString().Replace("$", "").Trim();
                    chkuseformula.Enabled = false;
                    chkIsServiceupdate.Enabled = false;

                    txtYourPrice.Visible = false;
                    lblYourPrice.Visible = true;
                    txtShippingPrice1.Visible = false;
                    lblShippingPrice1.Visible = true;

                    txtFulfilledBy.Visible = false;
                    lblFulfilledBy.Visible = false;

                    lblThresoldsubstract.Visible = true;
                    txtThresoldsubstract.Visible = false;


                    txtmaxprice.Visible = false;
                    lblmaxprice.Visible = true;
                    lblminprice.Visible = true;
                    // txtminprice.Visible = false;
                }
                else if (e.CommandName == "Save")
                {
                    String ID = Convert.ToString(e.CommandArgument.ToString());
                    Decimal itemPrice = 0;
                    Decimal itemShippingPrice = 0;
                    Decimal itemThresoldPrice = 0;
                    Decimal lowestPrice = 0;
                    Decimal lowestShipingPrice = 0;
                    Decimal itemTsubhresoldPrice = 0;
                    string FulfilledBy = "";
                    string skuid = "";
                    Decimal MinPrice = 0;
                    Decimal MaxPrice = 0;
                    Decimal.TryParse(txtYourPrice.Text, out itemPrice);
                    Decimal.TryParse(txtShippingPrice1.Text, out itemShippingPrice);
                    Decimal.TryParse(txtThresoldPrice.Text, out itemThresoldPrice);
                    Decimal.TryParse(txtLowPrice.Text, out lowestPrice);
                    Decimal.TryParse(txtShippingPrice2.Text, out lowestShipingPrice);
                    Decimal.TryParse(txtThresoldsubstract.Text, out itemTsubhresoldPrice);

                    Decimal OriginalYourPrice = Decimal.Zero;
                    Decimal OriginalShipping = Decimal.Zero;
                    Decimal OriginalTotal = Decimal.Zero;
                    OriginalYourPrice = Convert.ToDecimal(lblYourPrice.Text.ToString().Replace("$", "").Trim());
                    OriginalShipping = Convert.ToDecimal(lblShippingPrice1.Text.ToString().Replace("$", "").Trim());
                    OriginalTotal = OriginalYourPrice + OriginalShipping;


                    FulfilledBy = txtFulfilledBy.Text;
                    skuid = txtSkuID.Text;
                    Decimal.TryParse(txtminprice.Text, out MinPrice);
                    Decimal.TryParse(txtmaxprice.Text, out MaxPrice);
                    bool UseFormulaPrice = false;
                    Decimal Formulaprice = Decimal.Zero;


                    if (itemPrice + itemShippingPrice < MinPrice && MinPrice > 0)
                    {
                        itemPrice = MinPrice - itemShippingPrice;
                    }
                    else if (itemPrice + itemShippingPrice > MaxPrice && MaxPrice > 0)
                    {
                        itemPrice = MaxPrice - itemShippingPrice;
                    }


                    if (chkuseformula.Checked)
                    {
                        UseFormulaPrice = true;

                        Formulaprice = Convert.ToDecimal(itemPrice) - Convert.ToDecimal(itemTsubhresoldPrice);
                        if (Formulaprice < MinPrice && MinPrice > 0)
                        {
                            Formulaprice = MinPrice;
                        }
                        else if (Formulaprice > MaxPrice && MaxPrice > 0)
                        {
                            Formulaprice = MaxPrice;
                        }


                    }
                    else
                    {
                        Formulaprice = Convert.ToDecimal(itemPrice);
                        UseFormulaPrice = false;
                    }



                    Decimal TotalYours = Decimal.Zero;
                    Decimal TotalAmazon = Decimal.Zero;

                    TotalAmazon = lowestPrice + lowestShipingPrice;

                    if (chkuseformula.Checked)
                    {
                        TotalYours = Formulaprice + itemShippingPrice;
                    }
                    else
                    {
                        TotalYours = itemPrice + itemShippingPrice;
                    }

                    //if (TotalYours > TotalAmazon)
                    //{
                    //    FulfilledBy = "No";
                    //}
                    //else
                    //{
                    //    FulfilledBy = "Yes";
                    //}



                    ChangeUser(Convert.ToInt32(ID), itemPrice, itemShippingPrice, Formulaprice, lowestPrice, lowestShipingPrice, itemTsubhresoldPrice, FulfilledBy, skuid, MinPrice, MaxPrice, UseFormulaPrice, lblASINId.Text.ToString().Trim(), OriginalYourPrice, OriginalShipping, OriginalTotal, chkIsServiceupdate.Checked);
                    if (ViewState["CurrentPage"] != null)
                    {
                        currentPage = Convert.ToInt32(ViewState["CurrentPage"].ToString());
                    }
                    else
                    {
                        currentPage = 1;
                        ViewState["CurrentPage"] = currentPage;
                    }

                    CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 2,'" + lblASINId.Text.ToString().Trim() + "',''," + Session["AdminID"].ToString() + "");
                    BindGridPaging();


                }

            }
        }


        public void ChangeUser(int Id, Decimal itemPrice, Decimal itemShippingPrice, Decimal Formulaprice, Decimal lowestPrice, Decimal lowestShipingPrice, Decimal itemTsubhresoldPrice, string FulfilledBy, string skuid, Decimal MinPrice, Decimal MaxPrice, bool UseFormulaPrice, string asinid, Decimal OriginalPrice, Decimal OriginalShipping, Decimal OriginalTotal, bool IsServiceupdate)
        {


            ObjSql.ExecuteNonQuery("update ItemInfoes set ShippingPrice1=" + itemShippingPrice + ",YourPrice=" + itemPrice + ",FulfilledBy='" + FulfilledBy + "',ThresoldPrice=" + Formulaprice + ",LowPrice=" + lowestPrice + ",ShippingPrice2=" + lowestShipingPrice + ", Thresoldsubstract=" + itemTsubhresoldPrice + ",MinPrice=" + MinPrice + ",MaxPrice=" + MaxPrice + ",UseFormulaPrice='" + UseFormulaPrice + "',IsServiceupdate='" + IsServiceupdate + "' where id=" + Id + "");
            if (itemShippingPrice != OriginalShipping || itemPrice != OriginalPrice || itemPrice + itemShippingPrice != OriginalTotal)
            {
                CommonComponent.ExecuteCommonData("update infovendor set price=" + itemPrice + ",shipping=" + itemShippingPrice + " where price=" + OriginalPrice + " and shipping=" + OriginalShipping + " and ASINId='" + asinid + "'");
            }
            CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asinid + "'");
            CommonComponent.ExecuteCommonData("Exec GuiUpdateAmazonRepriceThresoldPrice_Page '" + asinid + "'");
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Price Updated Successfully.','Message');", true);


        }

        private void updatePrice(string SKU, int formulaprice, string asinid)
        {

            string SellerId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
            string MarketplaceId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));
            string AccessKeyId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
            string SecretKeyId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));

            string ApplicationVersion = "1.0";
            string ApplicationName = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));

            Decimal Price = 0;
            if (formulaprice == 1)
            {
                Price = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Exec GuiGetAmazonRepriceVendorDetails '" + SKU + "',3"));
            }
            else
            {
                Price = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select top 1 isnull(YourPrice,0) from ItemInfoes where SkuId='" + SKU + "'"));
            }

            string strhours = "Feed_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".xml";


            string filePath = Server.MapPath("/admin/XMLfile/" + strhours);
            XmlTextWriter xw = new XmlTextWriter(filePath, System.Text.Encoding.GetEncoding("iso-8859-1"));

            // use indenting.
            xw.WriteStartDocument();
            xw.Formatting = Formatting.Indented;

            xw.WriteStartElement("AmazonEnvelope");
            xw.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            xw.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "amzn-envelope.xsd");

            xw.WriteStartElement("Header");
            xw.WriteElementString("DocumentVersion", "1.01");
            xw.WriteElementString("MerchantIdentifier", SellerId);
            xw.WriteEndElement();

            xw.WriteElementString("MessageType", "Price");
            xw.WriteElementString("PurgeAndReplace", "false");

            xw.WriteStartElement("Message");
            xw.WriteElementString("MessageID", "1");
            xw.WriteStartElement("Price");
            xw.WriteElementString("SKU", SKU.Trim().Replace("'", "''"));

            xw.WriteStartElement("StandardPrice");
            xw.WriteAttributeString("currency", "USD");

            xw.WriteString(Convert.ToString(Price));
            xw.WriteEndElement();

            xw.WriteEndElement();
            xw.WriteEndElement();
            xw.Close();


            MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
            config.ServiceURL = "https://mws.amazonservices.com";

            MarketplaceWebServiceClient client = new MarketplaceWebServiceClient(
                                                             AccessKeyId,
                                                             SecretKeyId,
                                                             ApplicationName,
                                                             ApplicationVersion,
                                                             config);

            SubmitFeedRequest request = new SubmitFeedRequest();
            request.Merchant = SellerId;

            request.Marketplace = MarketplaceId;
            request.FeedContent = System.IO.File.Open(Server.MapPath("/admin/XMLfile/" + strhours), System.IO.FileMode.Open, System.IO.FileAccess.Read);

            request.ContentMD5 = MarketplaceWebServiceClient.CalculateContentMD5(request.FeedContent);
            request.FeedContent.Position = 0;
            request.FeedType = "_POST_PRODUCT_PRICING_DATA_";
            SubmitFeedResponse response = client.SubmitFeed(request);
            if (response.IsSetSubmitFeedResult())
            {
                SubmitFeedResult submitFeedResult = response.SubmitFeedResult;
                if (submitFeedResult.IsSetFeedSubmissionInfo())
                {
                    FeedSubmissionInfo feedSubmissionInfo = submitFeedResult.FeedSubmissionInfo;
                    if (feedSubmissionInfo.IsSetFeedSubmissionId())
                    {

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete1", "jAlert('Price Changes uploaded successfully, Please check in Amazon after 15 minutes.','Message');", true);
                        if (formulaprice == 1)
                        {
                            string gg = Price.ToString("C2") + "-Formula";
                            CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 3,'" + asinid + "','" + gg + "'," + Session["AdminID"].ToString() + "");
                        }
                        else
                        {
                            string gg = Price.ToString("C2") + "-YourPrice";
                            CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 3,'" + asinid + "','" + gg + "'," + Session["AdminID"].ToString() + "");
                        }

                    }
                    else
                    {

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete2", "jAlert('Unsuccessfull','Message');", true);
                    }
                }
            }
        }

        protected void grdamazon_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdamazon_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        /// <summary>
        /// Sort Column in ASC or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string Sortorder = "";
            if (btn != null)
            {
                if (btn.CommandArgument == "ASC")
                {
                    Sortorder = "Asc";

                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btn.ID == "btnsortname")
                    {
                        isDescendProductName = false;
                    }
                    else if (btn.ID == "btnsortsku")
                    {
                        isDescendSKU = false;
                    }
                    else if (btn.ID == "btnsortmaxprice")
                    {
                        isDescendMaxprice = false;
                    }
                    else if (btn.ID == "btnsortminprice")
                    {
                        isDescendMinprice = false;
                    }
                    else if (btn.ID == "btnsortislowest")
                    {
                        isDescendislowest = false;
                    }
                    else if (btn.ID == "btnsortyourprice")
                    {
                        isDescendYourPrice = false;
                    }
                    else if (btn.ID == "btnsortformulaprice")
                    {
                        isDescendThresoldPrice = false;
                    }
                    btn.AlternateText = "Descending Order";
                    btn.ToolTip = "Descending Order";
                    btn.CommandArgument = "DESC";
                }
                else if (btn.CommandArgument == "DESC")
                {
                    Sortorder = "Desc";

                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btn.ID == "btnsortname")
                    {
                        isDescendProductName = true;
                    }
                    else if (btn.ID == "btnsortsku")
                    {
                        isDescendSKU = true;
                    }
                    else if (btn.ID == "btnsortmaxprice")
                    {
                        isDescendMaxprice = true;
                    }
                    else if (btn.ID == "btnsortminprice")
                    {
                        isDescendMinprice = true;
                    }
                    else if (btn.ID == "btnsortislowest")
                    {
                        isDescendislowest = true;
                    }
                    else if (btn.ID == "btnsortyourprice")
                    {
                        isDescendYourPrice = true;
                    }
                    else if (btn.ID == "btnsortformulaprice")
                    {
                        isDescendThresoldPrice = true;
                    }
                    btn.AlternateText = "Ascending Order";
                    btn.ToolTip = "Ascending Order";
                    btn.CommandArgument = "ASC";
                }
            }

            OrderBy objOrderBy = (OrderBy)ViewState["Sort"];

            if (objOrderBy == null)
            {

                objOrderBy = new OrderBy();

            }

            objOrderBy.AddOrderBy(btn.CommandName, Sortorder);

            ViewState["Sort"] = objOrderBy;

            string order = objOrderBy.GetOrderByClause();

            //LoadProducts(order);

            lblOrder.Text = order.Trim().Length > 0 ? "" + order : "";
            ltrSortExpression.Text = objOrderBy.BindliteralSort();
            // grdamazon.SortExpression = lblOrder.Text;
            if (ViewState["GridData"] != null)
            {
                DataSet Ds = new DataSet();
                Ds = (DataSet)ViewState["GridData"];
                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = Ds.Tables[0];
                    DataView dv = new DataView(dt);
                    dv.Sort = order.ToString().Replace("order by", "");

                    grdamazon.DataSource = dv;
                    grdamazon.DataBind();
                }

            }
            else
            {
                BindGridPaging();
            }
        }


        protected void grdamazon_Sorting(object sender, GridViewSortEventArgs e)
        {


        }

        protected void btnremove_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnremove.Value.ToString()))
            {
                OrderBy objOrderBy = (OrderBy)ViewState["Sort"];

                if (objOrderBy == null)
                {

                    objOrderBy = new OrderBy();

                }

                objOrderBy.DeleteOrderBy(hdnremove.Value.ToString());

                ViewState["Sort"] = objOrderBy;

                string order = objOrderBy.GetOrderByClause();
                lblOrder.Text = order.Trim().Length > 0 ? "" + order : "";
                ltrSortExpression.Text = objOrderBy.BindliteralSort();
                // grdamazon.SortExpression = lblOrder.Text;
                if (ViewState["GridData"] != null)
                {
                    DataSet Ds = new DataSet();
                    Ds = (DataSet)ViewState["GridData"];
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = Ds.Tables[0];
                        DataView dv = new DataView(dt);
                        dv.Sort = order.ToString().Replace("order by", "");

                        grdamazon.DataSource = dv;
                        grdamazon.DataBind();
                    }

                }
                else
                {
                    BindGridPaging();
                }
                hdnremove.Value = "";
            }
        }

        protected void btntemp_Click(object sender, EventArgs e)
        {
            if (hdncurrenttemp.Value.ToString() != "0")
            {
                currentPage = Convert.ToInt32(hdncurrenttemp.Value.ToString());
                hdncurrenttemp.Value = "0";
            }
            BindGridPaging();
        }
        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtpassword.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RequiredPass", "jAlert('Please Enter Password.','Required');", true);
                txtpassword.Focus();
                return;
            }

            string pass = Convert.ToString(CommonComponent.GetScalarCommonData("select configvalue from tb_appconfig where configname='amazonrepricerPassword' and storeid=1"));
            if (!string.IsNullOrEmpty(pass))
            {
                if (txtpassword.Text.ToString().Trim() == pass.ToString().Trim())
                {


                    password.Visible = false;
                    divsearch.Visible = true;
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Scrollable", "$(document).ready(function () {$('#ContentPlaceHolder1_grdamazon').Scrollable();});", true);
                    if (ViewState["timerstring"] != null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Scrollable", "$(function(){" + ViewState["timerstring"].ToString() + "});  ", true);
                    }

                    //  Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "Scrollable_rt", "var $z = jQuery.noConflict();$z(function(){$z('#ContentPlaceHolder1_grdamazon').Scrollable(); });", true);//
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "IncorrectPass", "jAlert('Incorrect Password','Error');", true);
                }
            }

        }

    }


    [Serializable]

    public class OrderBy
    {

        public Hashtable hshOrderExpressions { get; set; } // To store the column info on which gridview has to be sorted

        public Hashtable hshOrderPosition { get; set; }  //To store the sort order



        public OrderBy()
        {

            hshOrderExpressions = new Hashtable();

            hshOrderPosition = new Hashtable();

        }

        public string GetOrderByClause()
        {

            string orderby = string.Empty;

            try
            {

                if (hshOrderExpressions.Count > 0)
                {

                    var orderbyClause = hshOrderPosition.Cast<DictionaryEntry>().OrderBy(entry => entry.Value);

                    foreach (DictionaryEntry clause in orderbyClause)
                    {

                        string key = Convert.ToString(clause.Key);

                        orderby += key + " " + hshOrderExpressions[key] + ",";

                    }



                }

                if (orderby.Length > 0)
                {

                    orderby = " order by " + orderby.Substring(0, orderby.Length - 1);

                }



            }

            catch (Exception ex)
            {

                throw ex;

            }

            return orderby;

        }

        public string BindliteralSort()
        {
            string orderby = string.Empty;

            try
            {

                if (hshOrderExpressions.Count > 0)
                {

                    var orderbyClause = hshOrderPosition.Cast<DictionaryEntry>().OrderBy(entry => entry.Value);
                    orderby = "<div class=\"pro-compare-row2\">";
                    foreach (DictionaryEntry clause in orderbyClause)
                    {
                        orderby += "<div id=\"c-2\" class=\"comp-pro-box\"><span><h4>";
                        string key = Convert.ToString(clause.Key);

                        orderby += key + " " + hshOrderExpressions[key] + "</h4></span><a class=\"comp-close\" onclick=\"DeleteSortData('" + key + "');\" id=\"delete_compare\"><img width=\"16\" height=\"16\" src=\"/images/delete_ico.png\"></a></div>";

                    }
                    orderby += "</div>";


                }

                //if (orderby.Length > 0)
                //{

                //    orderby = " order by " + orderby.Substring(0, orderby.Length - 1);

                //}



            }

            catch (Exception ex)
            {

                throw ex;

            }

            return orderby;
        }


        public bool DeleteOrderBy(string orderBy)
        {
            bool isAddedSuccessfully = false;

            try
            {

                if (hshOrderExpressions.Count > 0)
                {

                    if (hshOrderExpressions[orderBy] != null)
                    {
                        hshOrderExpressions.Remove(orderBy);
                        hshOrderPosition.Remove(orderBy);

                    }



                }


                isAddedSuccessfully = true;



            }

            catch (Exception ex)
            {

                throw ex;

            }

            return isAddedSuccessfully;
        }


        public bool AddOrderBy(string orderBy, string Sortorder)
        {

            bool isAddedSuccessfully = false;

            try
            {

                if (hshOrderExpressions.Count > 0)
                {

                    if (hshOrderExpressions[orderBy] != null)
                    {

                        //hshOrderExpressions[orderBy] = Convert.ToString(hshOrderExpressions[orderBy]) == "Asc" ? "Desc" : "Asc";
                        hshOrderExpressions[orderBy] = Sortorder;

                    }

                    else
                    {

                        hshOrderExpressions[orderBy] = Sortorder;

                    }



                    hshOrderPosition[orderBy] = hshOrderPosition.Count + 1;

                }

                else
                {

                    hshOrderExpressions[orderBy] = Sortorder;

                    hshOrderPosition[orderBy] = 1;

                }

                isAddedSuccessfully = true;



            }

            catch (Exception ex)
            {

                throw ex;

            }

            return isAddedSuccessfully;

        }

    }
}