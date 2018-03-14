using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Solution.Bussines.Components;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class viewamazonrepricevendor : BasePage
    {
        public Decimal YourPrice = 0;
        public Decimal YourShipping = 0;
        public Decimal Yourtotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            binddata();
        }
        public void binddata()
        {
            string asin = "";
            DataSet ds = new DataSet();
            if (Request.QueryString["asin"] != null)
            {
                asin = Request.QueryString["asin"].ToString();
                DataSet Dsdetails = new DataSet();
                Dsdetails = CommonComponent.GetCommonDataSet("select top 1 isnull(SkuId,'') as SkuId,isnull(ProductName,'') as ProductName,ASINId from ItemInfoes where ASINId='" + asin + "'");
                if(Dsdetails!=null && Dsdetails.Tables.Count>0 && Dsdetails.Tables[0].Rows.Count>0)
                {
                    lblname.Text = Dsdetails.Tables[0].Rows[0]["ProductName"].ToString();
                    lblsku.Text = Dsdetails.Tables[0].Rows[0]["SkuId"].ToString();
                    lblasin.Text = Dsdetails.Tables[0].Rows[0]["ASINId"].ToString();

                }

                ds = CommonComponent.GetCommonDataSet("Exec GuiGetAmazonRepriceVendorDetails '" + asin + "',1");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataSet dsYour = CommonComponent.GetCommonDataSet("select isnull(YourPrice,0) as YourPrice,isnull(ShippingPrice1,0) as ShippingPrice1,isnull(YourPrice,0)+isnull(ShippingPrice1,0) as total from ItemInfoes where ASINId='" + asin + "'");
                    if (dsYour != null && dsYour.Tables.Count > 0 && dsYour.Tables[0].Rows.Count > 0)
                    {
                        YourPrice = Convert.ToDecimal(dsYour.Tables[0].Rows[0]["YourPrice"].ToString());
                        YourShipping = Convert.ToDecimal(dsYour.Tables[0].Rows[0]["ShippingPrice1"].ToString());
                        Yourtotal = Convert.ToDecimal(dsYour.Tables[0].Rows[0]["total"].ToString());
                    }

                    gvhemminglog.DataSource = ds;
                    gvhemminglog.DataBind();
                }
                else
                {
                    gvhemminglog.DataSource = null;
                    gvhemminglog.DataBind();
                }
            }
            else
            {
                gvhemminglog.DataSource = null;
                gvhemminglog.DataBind();
            }


            string newstring = "";
            int fl = 0;

            newstring = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Vendordetail,'') from ItemInfoes where ASINId='" + asin + "'"));
            if (String.IsNullOrEmpty(newstring))
            {
                for (fl = 0; fl <= 5;fl++ )
                {
                    if (String.IsNullOrEmpty(newstring))
                    {
                        newstring = Getvendordetails(asin);
                       
                    }
                }
                   
                amazonframe.InnerHtml = newstring;


                if (!String.IsNullOrEmpty(newstring))
                {
                    CommonComponent.ExecuteCommonData("update ItemInfoes set Vendordetail='" + newstring.ToString().Replace("'", "''") + "' where ASINId='" + asin + "'");
                }

            }
            else
            {
                amazonframe.InnerHtml = newstring;
            }

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

        protected void gvhemminglog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvhemminglog.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void gvhemminglog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblprice = (Label)e.Row.FindControl("lblprice");
                Label lblshipping = (Label)e.Row.FindControl("lblshipping");
                Label lbltotal = (Label)e.Row.FindControl("lbltotal");
                Decimal vendorPrice = Convert.ToDecimal(lblprice.Text.ToString());
                Decimal vendorshipping = Convert.ToDecimal(lblshipping.Text.ToString());
                Decimal vendortotal = Convert.ToDecimal(lbltotal.Text.ToString());
                if (vendorPrice == YourPrice && vendorshipping == YourShipping && vendortotal == Yourtotal)
                {
                    e.Row.CssClass = "vendorcolor";
                }


            }
        }
    }
}