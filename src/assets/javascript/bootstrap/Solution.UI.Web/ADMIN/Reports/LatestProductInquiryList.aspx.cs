using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class LatestProductInquiryList : Solution.UI.Web.BasePage
    {
        public static bool isDescendSubject = false;
        public static bool isDescendEmail = false;
        public static bool isDescendIPAddress = false;
        public static bool isDescendSentOn = false;
        public static bool isDescendInventory = false;
        //static int pageNo = 1;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 62px; height: 23px; border:none;cursor:pointer;vertical-align:top;");
                btnSendMail.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/send-mail.png";
                //btnDeleteConfig.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                ddlStore.SelectedValue = "1";
                txtDateFrom.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                txtDateTo.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
                fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 2, txtDateFrom.Text, txtDateTo.Text);
            }
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnSubject")
                    {
                        isDescendSubject = false;
                    }
                    else if (lb.ID == "btnEmail")
                    {
                        isDescendEmail = false;
                    }
                    else if (lb.ID == "btnIpAddress")
                    {
                        isDescendIPAddress = false;
                    }
                    else if (lb.ID == "btnSentOn")
                    {
                        isDescendSentOn = false;
                    }
                    else if (lb.ID == "btnInventory")
                    {
                        isDescendInventory = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnSubject")
                    {
                        isDescendSubject = true;
                    }
                    else if (lb.ID == "btnEmail")
                    {
                        isDescendEmail = true;
                    }
                    else if (lb.ID == "btnIpAddress")
                    {
                        isDescendIPAddress = true;
                    }
                    else if (lb.ID == "btnSentOn")
                    {
                        isDescendSentOn = true;
                    }
                    else if (lb.ID == "btnInventory")
                    {
                        isDescendInventory = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Fills the grid for Product Inquiry List
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="Mode">int Mode</param>
        /// <param name="SearchFrom">string SearchFrom</param>
        /// <param name="SearchTo">string SearchTo</param>
        /// <param name="OrderByColumn">string OrderByColumn</param>
        /// <param name="OrderBy">string OrderBy</param>
        protected void fillGrid(int StoreId = 1, int Mode = 1, string SearchFrom = null, string SearchTo = null, string OrderByColumn = null, string OrderBy = null)
        {
            DataSet ds = new DataSet();
            ProductComponent productComponent = new ProductComponent();
            ds = productComponent.getLatestProductInquiry(StoreId, Mode, SearchFrom, SearchTo, OrderByColumn, OrderBy);

            if (ds != null)
            {
                grdLatestProductInquiry.DataSource = ds.Tables[0];
                grdLatestProductInquiry.DataBind();
            }
            if (grdLatestProductInquiry.Rows.Count == 0)
            {
                trBottom.Visible = false;
            }
            else
            {
                trBottom.Visible = true;
            }
            //grdLatestProductInquiry.PageIndex = pageNo;

        }

        /// <summary>
        ///  Latest Product Inquiry Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdLatestProductInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLatestProductInquiry.PageIndex = e.NewPageIndex;
            //pageNo = grdLatestProductInquiry.PageIndex;
            fillGrid(Convert.ToInt32(ddlStore.SelectedValue));
        }

        /// <summary>
        /// Latest Product Inquiry Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdLatestProductInquiry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (txtDateFrom.Text.Trim() != "" || txtDateTo.Text.Trim() != "")
            {
                fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 2, txtDateFrom.Text.Trim(), txtDateTo.Text.Trim(), e.CommandName.ToString(), e.CommandArgument.ToString());
            }
            else
            {
                fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 1, null, null, e.CommandName.ToString(), e.CommandArgument.ToString());
            }
        }

        /// <summary>
        /// Latest Product Inquiry Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdLatestProductInquiry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendSubject == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnSubject");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnSubject");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }

                if (isDescendEmail == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnEmail");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnEmail");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }

                if (isDescendIPAddress == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnIpAddress");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnIpAddress");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }

                if (isDescendSentOn == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnSentOn");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnSentOn");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }

                if (isDescendInventory == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnInventory");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnInventory");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtDateFrom != null && txtDateFrom.Text.Trim() != "")
            {
                fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 2, txtDateFrom.Text.Trim(), txtDateTo.Text.Trim());
            }
            else
            {
                fillGrid(Convert.ToInt32(ddlStore.SelectedValue));
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(txtDateFrom.Text.Trim()) > Convert.ToDateTime(txtDateTo.Text.Trim()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please enter 'Date To' grater than 'Date From' ', 'Message');", true);
                return;
            }
            fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 2, txtDateFrom.Text, txtDateTo.Text);
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            fillGrid(Convert.ToInt32(ddlStore.SelectedValue));
            //txtDateFrom.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
            //txtDateTo.Text = String.Format("{0:MM/dd/yyyy}", DateTime.Now);
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            // int StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //Store is selected dynamically from menu
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string IDs = "";

            foreach (GridViewRow r in grdLatestProductInquiry.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                if (chk.Checked)
                {
                    Label lb = (Label)r.FindControl("lblAvailabilityID");
                    if (IDs != "")
                    {
                        IDs += "," + lb.Text;
                    }
                    else
                        IDs = lb.Text;
                }
            }
            if (IDs != "")
            {
                ProductComponent productComponent = new ProductComponent();
                productComponent.deleteProductInquiry(Convert.ToInt32(ddlStore.SelectedValue), IDs);

                if (txtDateFrom.Text.Trim() != "" || txtDateTo.Text.Trim() != "")
                {
                    fillGrid(Convert.ToInt32(ddlStore.SelectedValue), 2, txtDateFrom.Text.Trim(), txtDateTo.Text.Trim(), null, null);
                }
                else
                {
                    fillGrid(Convert.ToInt32(ddlStore.SelectedValue));
                }
            }
        }

        /// <summary>
        /// Set Status of customer
        /// </summary>
        /// <param name="_Active">_Active Object</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
            {
                _ReturnUrl = "../images/isActive.png";

            }
            else
            {
                _ReturnUrl = "../images/isInactive.png";

            }
            return _ReturnUrl;
        }

        /// <summary>
        ///  Send Mail Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendMail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SendMail();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1212", "jAlert('Mail(s) sent successfully.', 'Message');selectAll(false);", true);
            }
            catch { }
        }

        /// <summary>
        /// Gets the Icon Image Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Icon Image Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Send Email function
        /// </summary>
        private void SendMail()
        {
            if (grdLatestProductInquiry.Rows.Count > 0)
            {
                CustomerComponent objCustomer = new CustomerComponent();
                ProductComponent objProduct = new ProductComponent();
                DataSet dsMailTemplate = new DataSet();
                dsMailTemplate = objCustomer.GetEmailTamplate("ProductAvailabilityNotification", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                tb_Product tb_product = new tb_Product();

                for (int i = 0; i < grdLatestProductInquiry.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdLatestProductInquiry.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        HiddenField hdnProductID = (HiddenField)grdLatestProductInquiry.Rows[i].FindControl("hdnProductID");
                        HiddenField hdnCustomerName = (HiddenField)grdLatestProductInquiry.Rows[i].FindControl("hdnCustomerName");
                        Label lblEmail = (Label)grdLatestProductInquiry.Rows[i].FindControl("lblEmail");
                        tb_product = objProduct.GetAllProductDetailsbyProductID(Convert.ToInt32(hdnProductID.Value));
                        if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                        {
                            String strBody = "";
                            String strSubject = "";
                            strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                            strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                            String strProductLink = "/" + tb_product.MainCategory.ToString().Trim() + "/" + tb_product.SEName.ToString().Trim() + "-" + tb_product.ProductID.ToString().Trim() + ".aspx";
                            strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###CUSTOMERNAME###", hdnCustomerName.Value.ToString().Trim(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###PRODUCTLINK###", strProductLink.ToString().Trim(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###INVENTORY###", Convert.ToString(tb_product.Inventory), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###IMAGELINK###", GetIconImageProduct(Convert.ToString(tb_product.ImageName)), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###PRODUCTLINK###", strProductLink.ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###PRODUCTNAME###", tb_product.Name.ToString().Trim(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###SKU###", Convert.ToString(tb_product.SKU).Trim(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                            strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                            AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                            if (lblEmail.Text.ToString().Trim() != "")
                            {
                                int MailID = CommonOperations.SendMail(lblEmail.Text.ToString(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                            }
                        }
                    }
                }
            }
            else
            {

            }
        }
    }
}