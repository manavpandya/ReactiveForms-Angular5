using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class CustomerQuoteList : BasePage
    {
        #region Declaration

        CustomerQuoteComponent objCustomer = null;
        tb_CustomerQuote tb_Customer = null;
        public static bool isDescendCustName = false;
        public static bool isDescendCustID = false;
        public static bool isDescendQuoteNo = false;
        public static bool isDescendStoreName = false;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer Quote inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer Quote updated successfully.', 'Message','');});", true);
                    }
                }
                int loginid = 0;
                if (Request.QueryString["loginid"] != "" && Request.QueryString["loginid"] != null)
                {
                    loginid = Convert.ToInt32(Request.QueryString["loginid"]);
                  
                    hdnLoginID.Text = Convert.ToString(loginid);

                }
                isDescendCustName = false;
                isDescendCustID = false;
                isDescendStoreName = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDeleteQuote.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                bindstore();
            }

        }

        /// <summary>
        /// Bind All Stores into Drop Down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            //var storeDetail = objStorecomponent.GetStore();
            //if (storeDetail.Count > 0 && storeDetail != null)
            //{
            DataSet storeDetail = new DataSet();
            storeDetail = CommonComponent.GetCommonDataSet("Select top 1 * from tb_Store where ISNULL(Deleted,0)=0");
            if (storeDetail != null && storeDetail.Tables.Count > 0 && storeDetail.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = storeDetail.Tables[0];
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //try
            //{
            //    if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
            //    {
            //        ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
            //        AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            //    }
            //    else
            //    {
            //        AppConfig.StoreID = 1;
            //        ddlStore.SelectedIndex = 0;
            //    }
            //}
            //catch
            //{
            //    ddlStore.SelectedIndex = 0;
            //}
            if (!String.IsNullOrEmpty(Request.QueryString["Storeid"]))
            {
                if (Convert.ToInt32(Request.QueryString["Storeid"].ToString()) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(Request.QueryString["Storeid"]);
                }
                else
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
        }


        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and search value
            grdCustomerQuote.PageIndex = 0;
            grdCustomerQuote.DataBind();
            if (grdCustomerQuote.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            grdCustomerQuote.PageIndex = 0;
            grdCustomerQuote.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            grdCustomerQuote.DataBind();
            if (grdCustomerQuote.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Customer Quote Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdCustomerQuote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Trim().ToLower() == "downloadpdf")
            {
                string QuoteNumber = Convert.ToString(e.CommandArgument);
                string strstr = Convert.ToString(CommonComponent.GetScalarCommonData("select Top 1 QuoteNumber from tb_CustomerQuote WHERE QuoteNumber='" + QuoteNumber.ToString() + "-1' Order By CustomerQuoteID"));
                if (!string.IsNullOrEmpty(strstr))
                {
                    downloadfile(Server.MapPath("~/Resources/QuoteFiles/Quote_" + strstr.ToString() + ".pdf"));
                }
                else
                {
                    downloadfile(Server.MapPath("~/Resources/QuoteFiles/Quote_" + QuoteNumber.ToString() + ".pdf"));
                }
                
            }
        }

        /// <summary>
        /// Customer Quote Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCustomerQuote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdCustomerQuote.Rows.Count > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRevised = (Label)e.Row.FindControl("lblRevised");
                Label lblQuoteNumber = (Label)e.Row.FindControl("lblQuoteNumber");
                Label lblCustomerID = (Label)e.Row.FindControl("lblCustomerID");
                Label lblStoreId = (Label)e.Row.FindControl("lblStoreId");

                Button dnwid = ((Button)e.Row.FindControl("dnwid"));
                System.Web.UI.HtmlControls.HtmlAnchor tagQuote = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("tagQuoteNumber");
                System.Web.UI.HtmlControls.HtmlImage ImageQuote = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("ImgQuoteNumber");

                System.Web.UI.HtmlControls.HtmlAnchor orderno = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("orderno");
                HiddenField hdnOrderNo = (HiddenField)e.Row.FindControl("hdnOrderNo");
                Label lblOrderno = (Label)e.Row.FindControl("lblOrderno");
                if (hdnOrderNo != null && hdnOrderNo.Value != "" && hdnOrderNo.Value != "0")
                {
                    orderno.Visible = true;
                    orderno.HRef = "/Admin/Orders/Orders.aspx?id=" + hdnOrderNo.Value.ToString();
                }
                else { lblOrderno.Visible = true; }


                int QuoteNumber = 0;
                if (!string.IsNullOrEmpty(lblQuoteNumber.Text.ToString().Trim()))
                {
                    Int32.TryParse(lblQuoteNumber.Text.ToString().Trim(), out QuoteNumber);
                    string filepath = Convert.ToString(Server.MapPath("~/Resources/QuoteFiles/Quote_" + QuoteNumber.ToString() + ".pdf"));
                    System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                    if (file.Exists)
                    {
                        dnwid.Visible = true;
                        dnwid.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/download-pdf.jpg) no-repeat transparent; width: 31px; height: 31px; border:none;cursor:pointer;");
                    }
                }

                if (!Convert.ToBoolean(lblRevised.Text))
                {
                    tagQuote.HRef = "CustomerQuote.aspx?Mode=revise&ID=" + lblQuoteNumber.Text.Trim().ToString() + "&CustId=" + lblCustomerID.Text.ToString() + "&StoreId=" + lblStoreId.Text.ToString().Trim() + "&saleorder=1";
                    ImageQuote.Src = "/App_Themes/" + Page.Theme + "/images/revise.png";
                    ImageQuote.Alt = "revise";
                }
                else
                {
                    tagQuote.Visible = false;
                    ImageQuote.Visible = false;
                }
                System.Web.UI.HtmlControls.HtmlGenericControl divcomment = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("divcomment");
                Label lblQuoteid = (Label)e.Row.FindControl("lblQuoteid");
                if (lblQuoteid != null && lblQuoteid.Text != "")
                {
                    string Message = Convert.ToString(CommonComponent.GetScalarCommonData("select Isnull(Message,'') as message   From tb_Contactus where Quoteid=" + lblQuoteid.Text));

                    if (Message != null && Message.ToString() != "")
                        divcomment.Visible = true;
                    else
                        divcomment.Visible = false;
                }
                else
                {
                    divcomment.Visible = false;
                }
            }
        }

        /// <summary>
        /// Sorting Grid View Ascending or Descending
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
                    grdCustomerQuote.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnQuoteNumber")
                    {
                        isDescendQuoteNo = false;
                    }
                    else if (lb.ID == "btnCustName")
                    {
                        isDescendCustName = false;
                    }
                    else
                    {
                        isDescendStoreName = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdCustomerQuote.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnQuoteNumber")
                    {
                        isDescendQuoteNo = true;
                    }
                    else if (lb.ID == "btnCustName")
                    {
                        isDescendCustName = true;
                    }
                    else
                    {
                        isDescendStoreName = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Delete Quote Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteQuote_Click(object sender, EventArgs e)
        {
            CustomerQuoteComponent CustomerQuoteComponent = new CustomerQuoteComponent();
            tb_CustomerQuote CustomerQuote = new tb_CustomerQuote();
            int totalRowCount = grdCustomerQuote.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdCustomerQuote.Rows[i].FindControl("hdnCustomerQuoteID");
                HiddenField hdnOrderNo = (HiddenField)grdCustomerQuote.Rows[i].FindControl("hdnOrderNo");

                CheckBox chk = (CheckBox)grdCustomerQuote.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    if (string.IsNullOrEmpty(hdnOrderNo.Value.ToString()) || hdnOrderNo.Value == "0")
                    {
                        try
                        {
                            CustomerQuoteComponent.DeleteCustomerQuotesID(Convert.ToInt32(hdn.Value));

                        }
                        catch { }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Can not delete this, Order already placed for this quote.', 'Message');});", true);
                    }
                }

            }
            grdCustomerQuote.DataBind();
            if (grdCustomerQuote.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Returns the extension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>Returns the extension of files</returns>
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }


        /// <summary>
        /// Download files from the specified filepath.
        /// </summary>
        /// <param name="filepath">string filepath.</param>
        private void downloadfile(string filepath)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                if (file.Exists)
                {
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();
                    Response.ContentType = ReturnExtension(file.Extension.ToLower());
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    System.IO.FileStream sourceFile = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No PDF file available for this Quote Number.', 'Message');});", true);
                }
            }
            catch (Exception ex) { }
        }

        //protected void grdCustomerQuote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdCustomerQuote.PageIndex = e.NewPageIndex;
        //    grdCustomerQuote.DataBind();
        //}
    }
}