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
    public partial class Admin : System.Web.UI.MasterPage
    {
        #region Declaration
        tb_Admin admin = null;
        string[] Rights = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
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
            //if (Request.QueryString["HType"] != null && Convert.ToString(Request.QueryString["HType"]).Trim().ToLower() == "no" && Request.Url.ToString().ToLower().IndexOf("productvariant.aspx") > -1)
            //{
            //    if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter1", "window.parent.location.href='/Admin/Login.aspx';", true); 

            //        return;
            //    }
            //}
            //else if (Request.Url.ToString().ToLower().IndexOf("customerphoneorder.aspx") > -1)
            //{
            //    if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter2", "window.parent.location.href='/Admin/Login.aspx';", true);

            //        return;
            //    }
            //}
              
            SetAdminRights();
            BindActiveImage();
            if (Session["AdminType"] != null && Session["AdminType"].ToString() != "3")
            {
                SetAdminPageRight();
            }
            
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
            if(!IsPostBack)
            {
                if (Session["AdminType"] != null && Session["AdminType"].ToString() == "3" && Request.Url.ToString().ToLower().IndexOf("multipleshippingsection.aspx") <=-1)
                {
                    Response.Redirect("/Admin/Orders/MultipleShippingSection.aspx");
                }
            }
            
        }

        /// <summary>
        /// Bind Active Image
        /// </summary>
        public void BindActiveImage()
        {

            string strurl = Request.RawUrl.ToString().ToLower();
            ViewState["Strimage"] = "";
            if (strurl.ToLower().Contains("/admin/dashboard.aspx"))
            {
                Image1.Src = "/App_Themes/gray/Images/dashboard-hover.png";
                ViewState["Strimage"] = "Image1";
                SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                SpaceDashboard.Visible = true;

            }
            else if (strurl.ToLower().Contains("/orders/") && Session["AdminType"] != null && Session["AdminType"].ToString() != "3")
            {
                if ((Rights != null && !Rights.Contains("1")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image2.Src = "/App_Themes/gray/Images/order-hover.png";
                ViewState["Strimage"] = "Image2";
            }
            else if (strurl.ToLower().Contains("/products/"))
            {
                if ((Rights != null && !Rights.Contains("2")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image3.Src = "/App_Themes/gray/Images/product-hover.png";
                ViewState["Strimage"] = "Image3";
            }
            else if (strurl.ToLower().Contains("/customers/"))
            {
                if ((Rights != null && !Rights.Contains("3")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image4.Src = "/App_Themes/gray/Images/customer-hover.png";
                ViewState["Strimage"] = "Image4";
            }
            else if (strurl.ToLower().Contains("/content/"))
            {
                if ((Rights != null && !Rights.Contains("4")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image5.Src = "/App_Themes/gray/Images/content-hover.png";
                ViewState["Strimage"] = "Image5";
            }
            else if (strurl.ToLower().Contains("/settings/"))
            {
                if ((Rights != null && !Rights.Contains("5")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image6.Src = "/App_Themes/gray/Images/settings-hover.png";
                ViewState["Strimage"] = "Image6";
            }
            else if (strurl.ToLower().Contains("/reports/"))
            {
                if ((Rights != null && !Rights.Contains("6")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image7.Src = "/App_Themes/gray/Images/reports-hover.png";
                ViewState["Strimage"] = "Image7";
            }
            else if (strurl.ToLower().Contains("/configuration/"))
            {
                if ((Rights != null && !Rights.Contains("7")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image8.Src = "/App_Themes/gray/Images/configuration-hover.png";
                ViewState["Strimage"] = "Image8";
            }

            else if (strurl.ToLower().Contains("/promotions/"))
            {
                if (Rights != null && !Rights.Contains("8"))
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                Image9.Src = "/App_Themes/gray/Images/promotions-hover.png";
                ViewState["Strimage"] = "Image9";
            }
            else if (strurl.ToLower().Contains("/feedmanagement/"))
            {
                if ((Rights != null && !Rights.Contains("10")) || Rights == null)
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                imgwebmail10.Src = "/App_Themes/gray/Images/feed-hover.png";
                ViewState["Strimage"] = "imgwebmail10";
            }
            else if (strurl.ToLower().Contains("/webmail/"))
            {
                if (Rights != null && !Rights.Contains("12"))
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                imgwebmail.Src = "/App_Themes/gray/Images/web-mail-hover.png";
                ViewState["Strimage"] = "imgwebmail";
            }
            else if (strurl.ToLower().Contains("/memo/"))
            {
                if (Rights != null && !Rights.Contains("13"))
                {
                    Response.Redirect("/admin/dashboard.aspx");
                    SpaceDashboard.Attributes.Add("style", "min-height:500px;");
                    SpaceDashboard.Visible = true;
                }
                imgmemo.Src = "/App_Themes/gray/Images/internal-memo-hover.png";

                ViewState["Strimage"] = "imgmemo";
            }

        }

        /// <summary>
        /// Bind Store dropdown
        /// </summary>
        private void bindstore()
        {
            DataTable dtStore = new DataTable();
            DataSet dsStore = new DataSet();
            dsStore = CommonComponent.GetCommonDataSet("SELECT StoreID,StoreName,DisplayOrder,Deleted FROM tb_Store WHERE deleted=0  order by DisplayOrder ASC");
            ltStoreListProduct.Text = "";
            ltStoreAddProduct.Text = "";
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
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 3)
                    {
                        EditSID = "/Admin/Products/ProductAmazon.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 4)
                    {
                        EditSID = "/Admin/Products/ProductOverStock.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 5)
                    {
                        EditSID = "/Admin/Products/ProductYahoo.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 6)
                    {
                        //Redirect to ProductSears page for eBay product
                        EditSID = "/Admin/Products/ProductYahoo.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 7)
                    {
                        //Redirect to ProductEBay page
                        EditSID = "/Admin/Products/ProductEBay.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 8)
                    {
                        //Redirect to ProductSears page
                        EditSID = "/Admin/Products/ProductSears.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else if (StoreID == 9)
                    {
                        //Redirect to ProductBuy page
                        EditSID = "/Admin/Products/ProductBuy.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    else
                    {
                        EditSID = "/Admin/Products/Product.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                        ltStoreAddProduct.Text += "<li><a href='" + EditSID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
                    SID = "/Admin/Products/ProductList.aspx?StoreId=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "";
                    ltStoreListProduct.Text += "<li><a href='" + SID + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    ltorderlist.Text += "<li><a href='/Admin/Orders/OrderList.aspx?Storeid=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    if (dsStore.Tables[0].Rows[j]["StoreID"].ToString() == "1")
                    {
                        ltrSalesOrderStorelist.Text += "<li><a href='/Admin/Orders/PhoneOrder.aspx?Storeid=" + dsStore.Tables[0].Rows[j]["StoreID"].ToString() + "'>" + Convert.ToString(dsStore.Tables[0].Rows[j]["StoreName"]) + "</a></li>";
                    }
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
                if (admin != null && admin.Rights != null)
                {
                    Rights = admin.Rights.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int cnt = 0; cnt < Rights.Length; cnt++)
                    {
                        switch (Convert.ToInt16(Rights[cnt]))
                        {
                            case 1:
                                Page.Master.FindControl("trOrder").Visible = true;
                                break;
                            case 2:
                                Page.Master.FindControl("trproduct").Visible = true;
                                break;
                            case 3:
                                Page.Master.FindControl("trcustomer").Visible = true;
                                break;
                            case 4:
                                Page.Master.FindControl("trcontent").Visible = true;
                                break;
                            case 5:
                                Page.Master.FindControl("trSetting").Visible = true;
                                break;
                            case 6:
                                Page.Master.FindControl("trreports").Visible = true;
                                break;
                            case 7:
                                Page.Master.FindControl("trconfiguration").Visible = true;
                                break;
                            case 8:
                                Page.Master.FindControl("trPromotions").Visible = true;
                                break;
                            case 10:
                                Page.Master.FindControl("trFeed").Visible = true;
                                break;
                            case 12:
                                Page.Master.FindControl("trwebmail").Visible = true;
                                break;
                            case 13:
                                Page.Master.FindControl("trMemo").Visible = true;
                                break;
                            case 14:
                                Page.Master.FindControl("liReplenishment").Visible = true;
                                break;
                        }
                    }
                }
            }
        }


        public void SetAdminPageRight()
        {
            if (Session["dtAdminRightsList"] != null)
            {
                DataTable dtright = new DataTable();
                dtright = (DataTable)Session["dtAdminRightsList"];
                if (dtright != null && dtright.Rows.Count > 0)
                {
                    for (int i = 0; i < dtright.Rows.Count; i++)
                    {
                        switch (Convert.ToString(dtright.Rows[i]["PageName"]))
                        {
                            case "Order List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liOrderList.Visible = true;
                                }
                                else
                                {
                                    liOrderList.Visible = false; ;
                                }
                                break;
                            case "Phone Orders":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liPhoneOrder.Visible = true;
                                }
                                else
                                {
                                    liPhoneOrder.Visible = false; ;
                                }
                                break;
                            case "Multiple Capture":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liMultiCapture.Visible = true;
                                }
                                else
                                {
                                    liMultiCapture.Visible = false; ;
                                }
                                break;
                            case "Warehouse P.O.":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liWareHousePO.Visible = true;
                                }
                                else
                                {
                                    liWareHousePO.Visible = false; ;
                                }
                                break;
                            case "Return Item List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liReturnItemList.Visible = true;
                                }
                                else
                                {
                                    liReturnItemList.Visible = false;
                                }
                                break;
                            case "Bulk Order Print":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liBulkOrderPrint.Visible = true;
                                }
                                else
                                {
                                    liBulkOrderPrint.Visible = false;
                                }
                                break;
                            case "Bulk Packing Slip Print":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liBulkOrderSlipPrint.Visible = true;
                                }
                                else
                                {
                                    liBulkOrderSlipPrint.Visible = false;
                                }
                                break;
                            case "Failed Transactions":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liFaildTransaction.Visible = true;
                                }
                                else
                                {
                                    liFaildTransaction.Visible = false;
                                }
                                break;
                            case "Product List/Add":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liProductList.Visible = true;
                                    //liAddProduct.Visible = true;
                                }
                                else
                                {
                                    liProductList.Visible = false;
                                    liAddProduct.Visible = false;
                                }
                                break;
                            case "Add Category":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liAddCategory.Visible = true;
                                }
                                else
                                {
                                    liAddCategory.Visible = false; ;
                                }
                                break;
                            case "Vendor/DropShipper List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liVendorDropShipperList.Visible = true;
                                }
                                else
                                {
                                    liVendorDropShipperList.Visible = false; ;
                                }
                                break;
                            case "DropShipper SKU List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liDropShipperSKUList.Visible = true;
                                }
                                else
                                {
                                    liDropShipperSKUList.Visible = false; ;
                                }
                                break;
                            case "Vendor/DropShipper Payment List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liVendorDropShipperPaymentList.Visible = true;
                                }
                                else
                                {
                                    liVendorDropShipperPaymentList.Visible = false; ;
                                }
                                break;
                            case "Warehouse List":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liWareHouseList.Visible = true;
                                }
                                else
                                {
                                    liWareHouseList.Visible = false; ;
                                }
                                break;
                            case "Product Reviews":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liProductReview.Visible = true;
                                }
                                else
                                {
                                    liProductReview.Visible = false; ;
                                }
                                break;
                            case "SEO":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    //liSEO.Visible = true;
                                }
                                else
                                {
                                    liSEO.Visible = false; ;
                                }
                                break;
                            case "Import/Export":
                                if (Convert.ToBoolean(dtright.Rows[i]["isListed"]))
                                {
                                    liImportExport.Visible = true;
                                }
                                else
                                {
                                    liImportExport.Visible = false; ;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}