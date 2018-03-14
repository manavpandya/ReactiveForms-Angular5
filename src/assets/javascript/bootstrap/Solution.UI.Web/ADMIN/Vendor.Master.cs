using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data.SqlClient;
using System.Data;
using StringBuilder = System.Text.StringBuilder;
using System.IO;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN
{
    public partial class Vendor : System.Web.UI.MasterPage
    {
        #region Declaration
        tb_Admin admin = null;
        string[] Rights = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string httpurl = Request.Url.ToString();
            if (httpurl.ToLower().StartsWith("http://"))
            {
                if (AppLogic.AppConfigBool("UseSSLAdmin"))
                {
                    httpurl = httpurl.Replace("http", "https").ToString();
                    Response.Redirect(httpurl);
                }
            }
            SetAdminRights();
            BindActiveImage();
            SetAdminPageRight();
            if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            {
                Response.Redirect("/Admin/Login.aspx");
            }
            if (Request.RawUrl.Contains("/Dashboard.aspx"))
            {
                SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                SpaceDashboard.Visible = true;
            }
            bindstore();
            litDate.Text = DateTime.Now.ToLongDateString();
        }

        /// <summary>
        /// Bind Active Image
        /// </summary>
        public void BindActiveImage()
        {

            string strurl = Request.RawUrl.ToString().ToLower();
            ViewState["Strimage"] = "";
            Image3.Src = "/App_Themes/gray/Images/product-hover.png";
            //if (strurl.ToLower().Contains("/admin/dashboard.aspx"))
            //{
            //    Image1.Src = "/App_Themes/gray/Images/dashboard-hover.png";
            //    ViewState["Strimage"] = "Image1";
            //    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //    SpaceDashboard.Visible = true;

            //}
            //else if (strurl.ToLower().Contains("/orders/"))
            //{
            //    if ((Rights != null && !Rights.Contains("1")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image2.Src = "/App_Themes/gray/Images/order-hover.png";
            //    ViewState["Strimage"] = "Image2";
            //}
            //else if (strurl.ToLower().Contains("/products/"))
            //{
            //    if ((Rights != null && !Rights.Contains("2")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image3.Src = "/App_Themes/gray/Images/product-hover.png";
            //    ViewState["Strimage"] = "Image3";
            //}
            //else if (strurl.ToLower().Contains("/customers/"))
            //{
            //    if ((Rights != null && !Rights.Contains("3")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image4.Src = "/App_Themes/gray/Images/customer-hover.png";
            //    ViewState["Strimage"] = "Image4";
            //}
            //else if (strurl.ToLower().Contains("/content/"))
            //{
            //    if ((Rights != null && !Rights.Contains("4")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image5.Src = "/App_Themes/gray/Images/content-hover.png";
            //    ViewState["Strimage"] = "Image5";
            //}
            //else if (strurl.ToLower().Contains("/settings/"))
            //{
            //    if ((Rights != null && !Rights.Contains("5")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image6.Src = "/App_Themes/gray/Images/settings-hover.png";
            //    ViewState["Strimage"] = "Image6";
            //}
            //else if (strurl.ToLower().Contains("/reports/"))
            //{
            //    if ((Rights != null && !Rights.Contains("6")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image7.Src = "/App_Themes/gray/Images/reports-hover.png";
            //    ViewState["Strimage"] = "Image7";
            //}
            //else if (strurl.ToLower().Contains("/configuration/"))
            //{
            //    if ((Rights != null && !Rights.Contains("7")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image8.Src = "/App_Themes/gray/Images/configuration-hover.png";
            //    ViewState["Strimage"] = "Image8";
            //}

            //else if (strurl.ToLower().Contains("/promotions/"))
            //{
            //    if (Rights != null && !Rights.Contains("8"))
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    Image9.Src = "/App_Themes/gray/Images/promotions-hover.png";
            //    ViewState["Strimage"] = "Image9";
            //}
            //else if (strurl.ToLower().Contains("/feedmanagement/"))
            //{
            //    if ((Rights != null && !Rights.Contains("10")) || Rights == null)
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    imgwebmail10.Src = "/App_Themes/gray/Images/feed-hover.png";
            //    ViewState["Strimage"] = "imgwebmail10";
            //}
            //else if (strurl.ToLower().Contains("/webmail/"))
            //{
            //    if (Rights != null && !Rights.Contains("12"))
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    imgwebmail.Src = "/App_Themes/gray/Images/web-mail-hover.png";
            //    ViewState["Strimage"] = "imgwebmail";
            //}
            //else if (strurl.ToLower().Contains("/memo/"))
            //{
            //    if (Rights != null && !Rights.Contains("13"))
            //    {
            //        Response.Redirect("/admin/dashboard.aspx");
            //        SpaceDashboard.Attributes.Add("style", "min-height:500px;");
            //        SpaceDashboard.Visible = true;
            //    }
            //    imgmemo.Src = "/App_Themes/gray/Images/internal-memo-hover.png";

            //    ViewState["Strimage"] = "imgmemo";
            //}

        }

        /// <summary>
        /// Bind Store dropdown
        /// </summary>
        private void bindstore()
        {
            DataTable dtStore = new DataTable();
            DataSet dsStore = new DataSet();
            dsStore = CommonComponent.GetCommonDataSet("SELECT StoreID,StoreName,DisplayOrder,Deleted FROM tb_Store WHERE deleted=0  order by DisplayOrder ASC");
            //ltStoreListProduct.Text = "";
            //ltStoreAddProduct.Text = "";
            ltorderlist.Text = "";
            ltrSalesOrderStorelist.Text = "";
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {

                for (int j = 0; j < dsStore.Tables[0].Rows.Count; j++)
                {
                    int StoreID = 0;
                    String SID = string.Empty;
                    String EditSID = string.Empty;
                    StoreID = Convert.ToInt32(dsStore.Tables[0].Rows[j]["StoreID"].ToString());
                    if (StoreID == 2)
                    {
                        //Redirect to amazon
                        EditSID = "/Admin/Products/ProductYahoo.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        //ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 3)
                    {
                        EditSID = "/Admin/Products/ProductAmazon.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                       // ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 4)
                    {
                        EditSID = "/Admin/Products/ProductOverStock.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                       // ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 5)
                    {
                        EditSID = "/Admin/Products/ProductYahoo.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                       // ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 6)
                    {
                        //Redirect to ProductSears page for eBay product
                        EditSID = "/Admin/Products/ProductYahoo.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                      //  ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 7)
                    {
                        //Redirect to ProductEBay page
                        EditSID = "/Admin/Products/ProductEBay.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                       // ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 8)
                    {
                        //Redirect to ProductSears page
                        EditSID = "/Admin/Products/ProductSears.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                       // ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 9)
                    {
                        //Redirect to ProductBuy page
                        EditSID = "/Admin/Products/ProductBuy.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        //ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else
                    {
                        EditSID = "/Admin/Products/Product.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        //ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    SID = "/Admin/Products/ProductList.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                  //  ltStoreListProduct.Text += "<li><a href='" + SID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    ltorderlist.Text += "<li><a href='/Admin/Orders/OrderList.aspx?Storeid=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    ltrSalesOrderStorelist.Text += "<li><a href='/Admin/Orders/PhoneOrder.aspx?Storeid=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                }
            }
        }

        /// <summary>
        /// Display only those modules to Admin for which contains Rights
        /// </summary>
        private void SetAdminRights()
        {
            if (Session["AdminID"] != null)
            {
                AdminRightsComponent objAdminRightsComp = new AdminRightsComponent();
                admin = objAdminRightsComp.GetAdminByID(Convert.ToInt32(Session["AdminID"]));
                Page.Master.FindControl("trproduct").Visible = true;
                
            }
        }


        public void SetAdminPageRight()
        {
             
        }
    }
}