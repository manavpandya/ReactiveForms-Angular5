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

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class CustomerList : BasePage
    {

        #region Declaration

        CustomerComponent objCustomer = null;
        tb_Customer tb_Customer = null;
        public static bool isDescendCustName = false;
        public static bool isDescendCustID = false;
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
                CustomerComponent._count = 0;
                txtisostback.Text = "false";
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer updated successfully.', 'Message','');});", true);
                    }
                }
                isDescendCustName = false;
                isDescendCustID = false;
                isDescendStoreName = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

                bindstore();
            }
            else
            {
                txtisostback.Text = "true";
            }

        }


        /// <summary>
        /// Bind Store Method
        /// </summary>
        private void bindstore()
        {
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
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                }
                else
                {
                    AppConfig.StoreID = 1;
                    ddlStore.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and search value
            grdCustomer.PageIndex = 0;
            grdCustomer.DataBind();
        }

        /// <summary>
        /// Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param> 
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtEmail.Text = "";
            txtphone.Text = "";
            txtzipcode.Text = "";
            //ddlStore.SelectedIndex = 0;
            grdCustomer.PageIndex = 0;
            grdCustomer.DataBind();
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
            grdCustomer.DataBind();
        }

        /// <summary>
        /// Customer Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.ToString().Trim().ToLower() == "delete")
            {
                //int gCustomerID = Convert.ToInt32(e.CommandArgument);
                //objCustomer = new CustomerComponent();
                //tb_Customer = new tb_Customer();
                //int IsDeleted = 0;
                //tb_Customer = objCustomer.GetCustomerDataByID(gCustomerID);
                //tb_Customer.Deleted = true;
                //IsDeleted = objCustomer.UpdateCustomerList(tb_Customer);
                //if (IsDeleted > 0)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
                //    grdCustomer.DataBind();
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                //    return;
                //}

            }
            if (e.CommandName.ToString().Trim().ToLower() == "edit")
            {
                int gCustomerID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("Customer.aspx?mode=edit&CustID=" + gCustomerID.ToString(), true);
            }
        }

        /// <summary>
        /// Customer Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdCustomer.Rows.Count > 0)
            {
                btnExport.Visible = true;
                t1.Visible = true;
                t2.Visible = true;
                t3.Visible = true;
                t4.Visible = true;
            }

            else
            {
                btnExport.Visible = false;
                t1.Visible = false;
                t2.Visible = false;
                t3.Visible = false;
                t4.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendCustID == false)
                {
                    ImageButton btnCustID = (ImageButton)e.Row.FindControl("btnCustID");
                    btnCustID.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnCustID.AlternateText = "Ascending Order";
                    btnCustID.ToolTip = "Ascending Order";
                    btnCustID.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnCustID = (ImageButton)e.Row.FindControl("btnCustID");
                    btnCustID.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnCustID.AlternateText = "Descending Order";
                    btnCustID.ToolTip = "Descending Order";
                    btnCustID.CommandArgument = "ASC";
                }
                if (isDescendCustName == false)
                {
                    ImageButton btnCustName = (ImageButton)e.Row.FindControl("btnCustName");
                    btnCustName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnCustName.AlternateText = "Ascending Order";
                    btnCustName.ToolTip = "Ascending Order";
                    btnCustName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnCustName = (ImageButton)e.Row.FindControl("btnCustName");
                    btnCustName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnCustName.AlternateText = "Descending Order";
                    btnCustName.ToolTip = "Descending Order";
                    btnCustName.CommandArgument = "ASC";
                }
                if (isDescendStoreName == false)
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnStoreName.AlternateText = "Ascending Order";
                    btnStoreName.ToolTip = "Ascending Order";
                    btnStoreName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnStoreName.AlternateText = "Descending Order";
                    btnStoreName.ToolTip = "Descending Order";
                    btnStoreName.CommandArgument = "ASC";
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
                    grdCustomer.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnCustID")
                    {
                        isDescendCustID = false;
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
                    grdCustomer.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnCustID")
                    {
                        isDescendCustID = true;
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
        /// Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int gCustomerID = Convert.ToInt32(hdnCustDelete.Value);
            objCustomer = new CustomerComponent();
            tb_Customer = new tb_Customer();
            int IsDeleted = 0;
            tb_Customer = objCustomer.GetCustomerDataByID(gCustomerID);
            tb_Customer.Deleted = true;
            IsDeleted = objCustomer.UpdateCustomerList(tb_Customer);
            if (IsDeleted > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
                grdCustomer.DataBind();
                hdnCustDelete.Value = "0";
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                return;
            }
        }

        /// <summary>
        /// Export Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
                {
                    if (Convert.ToDateTime(txtToDate.Text.ToString()) >= Convert.ToDateTime(txtFromDate.Text.ToString()))
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                }
                else
                {
                    if (txtFromDate.Text.ToString() == "" && txtToDate.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                    else if (txtToDate.Text.ToString() == "" && txtFromDate.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }

                }
            }
            catch { }
            CommonComponent clsCommon = new CommonComponent();
            DataView dvCust = new DataView();
            string whereclause = "";
            if (ddlStore.SelectedIndex == 0 || ddlStore.SelectedIndex == -1)
            {

                if(!string.IsNullOrEmpty(txtSearch.Text))
                {
                    whereclause = whereclause + " and (tb_Customer.FirstName like '%" + txtSearch.Text.ToString() + "%' OR tb_Customer.LastName like '%" + txtSearch.Text.ToString() + "%' Or ShippingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or ShippingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' ";

                    whereclause = whereclause + " or tb_Customer.FirstName+' '+tb_Customer.LastName like '%" + txtSearch.Text.ToString() + "%'   Or ShippingAddress.FirstName+' '+ShippingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.FirstName+' '+BillingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' or tb_Customer.LastName+' '+tb_Customer.FirstName like '%" + txtSearch.Text.ToString() + "%' Or ShippingAddress.LastName+' '+ShippingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.LastName+' '+BillingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' )";
                }
                if (!string.IsNullOrEmpty(txtEmail.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Email like '%" + txtEmail.Text.ToString() + "%' OR tb_Customer.Email like '%" + txtEmail.Text.ToString() + "%' OR ShippingAddress.Email like '%" + txtEmail.Text.ToString() + "%')";
                }
                if (!string.IsNullOrEmpty(txtphone.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Phone like '%" + txtphone.Text.ToString() + "%'   OR ShippingAddress.Phone like '%" + txtphone.Text.ToString() + "%')";
                }
                if (!string.IsNullOrEmpty(txtzipcode.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Zipcode like '%" + txtzipcode.Text.ToString() + "%'   OR ShippingAddress.Zipcode like '%" + txtzipcode.Text.ToString() + "%')";
                }
                if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
                {
                    whereclause = whereclause + " and cast(tb_customer.CreatedOn as date)>=cast('" + txtFromDate.Text.ToString() + "' as date) and cast(tb_customer.CreatedOn as date) <= cast('" + txtToDate.Text.ToString() + "' as date)";
                }

                dvCust = clsCommon.GetCustomerExport(whereclause , ddlStore.SelectedValue).Tables[0].DefaultView;
            }
            else
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    whereclause = whereclause + " and (tb_Customer.FirstName like '%" + txtSearch.Text.ToString() + "%' OR tb_Customer.LastName like '%" + txtSearch.Text.ToString() + "%' Or ShippingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or ShippingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' ";

                    whereclause = whereclause + " or tb_Customer.FirstName+' '+tb_Customer.LastName like '%" + txtSearch.Text.ToString() + "%'   Or ShippingAddress.FirstName+' '+ShippingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.FirstName+' '+BillingAddress.LastName like '%" + txtSearch.Text.ToString() + "%' or tb_Customer.LastName+' '+tb_Customer.FirstName like '%" + txtSearch.Text.ToString() + "%'   Or ShippingAddress.LastName+' '+ShippingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' Or BillingAddress.LastName+' '+BillingAddress.FirstName like '%" + txtSearch.Text.ToString() + "%' )";
                }
                if (!string.IsNullOrEmpty(txtEmail.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Email like '%" + txtEmail.Text.ToString() + "%' OR tb_Customer.Email like '%" + txtEmail.Text.ToString() + "%' OR ShippingAddress.Email like '%" + txtEmail.Text.ToString() + "%')";
                }
                if (!string.IsNullOrEmpty(txtphone.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Phone like '%" + txtphone.Text.ToString() + "%'   OR ShippingAddress.Phone like '%" + txtphone.Text.ToString() + "%')";
                }
                if (!string.IsNullOrEmpty(txtzipcode.Text))
                {
                    whereclause = whereclause + " and (BillingAddress.Zipcode like '%" + txtzipcode.Text.ToString() + "%'   OR ShippingAddress.Zipcode like '%" + txtzipcode.Text.ToString() + "%')";
                }
                if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
                {
                    whereclause = whereclause + " and cast(tb_customer.CreatedOn as date)>=cast('" + txtFromDate.Text.ToString() + "' as date) and cast(tb_customer.CreatedOn as date) <= cast('" + txtToDate.Text.ToString() + "' as date)";
                }
                dvCust = clsCommon.GetCustomerExport(whereclause, ddlStore.SelectedValue).Tables[0].DefaultView;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (dvCust != null && dvCust.Table.Rows.Count > 0)
            {
                for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                {
                    object[] args = new object[26];
                    args[0] = Convert.ToString(dvCust.Table.Rows[i]["CustomerID"]);
                    args[1] = Convert.ToString(dvCust.Table.Rows[i]["Email"]);
                    args[2] = Convert.ToString(dvCust.Table.Rows[i]["FirstName"]) + " " + Convert.ToString(dvCust.Table.Rows[i]["LastName"]);
                    args[3] = Convert.ToString(dvCust.Table.Rows[i]["Active"]);
                    args[4] = Convert.ToString(dvCust.Table.Rows[i]["BillingAddressName"]);
                    args[5] = Convert.ToString(dvCust.Table.Rows[i]["BillingCompany"]);
                    args[6] = Convert.ToString(dvCust.Table.Rows[i]["BillingAddress1"]);
                    args[7] = Convert.ToString(dvCust.Table.Rows[i]["BillingAddress2"]);
                    args[8] = Convert.ToString(dvCust.Table.Rows[i]["BillingSuite"]);
                    args[9] = Convert.ToString(dvCust.Table.Rows[i]["BillingCity"]);
                    args[10] = Convert.ToString(dvCust.Table.Rows[i]["BillingCountry"]);
                    args[11] = Convert.ToString(dvCust.Table.Rows[i]["BillingState"]);
                    args[12] = Convert.ToString(dvCust.Table.Rows[i]["BillingZipCode"]);
                    args[13] = Convert.ToString(dvCust.Table.Rows[i]["BillingPhone"]);
                    args[14] = Convert.ToString(dvCust.Table.Rows[i]["BillingFax"]);

                    args[15] = Convert.ToString(dvCust.Table.Rows[i]["ShippingAddressName"]);
                    args[16] = Convert.ToString(dvCust.Table.Rows[i]["ShippingCompany"]);
                    args[17] = Convert.ToString(dvCust.Table.Rows[i]["ShippingAddress1"]);
                    args[18] = Convert.ToString(dvCust.Table.Rows[i]["ShippingAddress2"]);
                    args[19] = Convert.ToString(dvCust.Table.Rows[i]["ShippingSuite"]);
                    args[20] = Convert.ToString(dvCust.Table.Rows[i]["ShippingCity"]);
                    args[21] = Convert.ToString(dvCust.Table.Rows[i]["ShippingCountry"]);
                    args[22] = Convert.ToString(dvCust.Table.Rows[i]["ShippingState"]);
                    args[23] = Convert.ToString(dvCust.Table.Rows[i]["ShippingZipCode"]);
                    args[24] = Convert.ToString(dvCust.Table.Rows[i]["ShippingPhone"]);
                    args[25] = Convert.ToString(dvCust.Table.Rows[i]["ShippingFax"]);

                    sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\",\"{19}\",\"{20}\",\"{21}\",\"{22}\",\"{23}\",\"{24}\",\"{25}\"", args));
                }
            }

            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                string FullString = sb.ToString();
                sb.Remove(0, sb.Length);
                sb.AppendLine("CustomerID,Email,Name,Active,BillingAddressName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingCountry,BillingState,BillingZipCode,BillingPhone,BillingFax,ShippingAddressName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingSuite,ShippingCity,ShippingCountry,ShippingState,ShippingZipCode,ShippingPhone,ShippingFax");
                sb.AppendLine(FullString);

                DateTime dt = DateTime.Now;
                String FileName = "Customers_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('No Record Found to Export.', 'Message');", true);
                return;
            }

        }

        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
    }
}