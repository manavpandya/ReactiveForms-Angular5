using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class QuantityDiscountList1 : BasePage
    {
        #region Declaration
        public static bool isDescendQuantityDiscount = false;
        public static bool isDescendstname = false;
        public static bool isDescendLowQuantity = false;
        public static bool isDescendHighQuantity = false;
        public static bool isDescendDiscount = false;
        tb_QauntityDiscount tblQuantityDiscount = null;
        QuantityDiscountComponent objQuantityDiscount = null;
        static int discountID = 0;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                btnAddNew.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/add-quantity-discount.png) no-repeat transparent; width: 154px; height: 23px; border:none;cursor:pointer;");
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                btndeleteQuantitytable.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                String strStatus = Convert.ToString(Request.QueryString["status"]);
                if (strStatus == "inserted")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Quantity Discount inserted successfully.', 'Message','');});", true);
                }
                else if (strStatus == "updated")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Quantity Discount updated successfully.', 'Message','');});", true);
                }
                bindstore();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
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
            ddlStore.Items.Insert(0, new ListItem("All Store", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Sorting function For Gridview
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    gvQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbQuantityDiscount")
                    {
                        isDescendQuantityDiscount = false;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = false;
                    }
                    else if (lb.ID == "lbLowQuantity")
                    {
                        isDescendLowQuantity = false;
                    }
                    else if (lb.ID == "lbHighQuantity")
                    {
                        isDescendHighQuantity = false;
                    }
                    else
                    {
                        isDescendDiscount = false;
                    }


                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbQuantityDiscount")
                    {
                        isDescendQuantityDiscount = true;
                    }
                    else if (lb.ID == "lbstname")
                    {
                        isDescendstname = true;
                    }
                    else if (lb.ID == "lbLowQuantity")
                    {
                        isDescendLowQuantity = true;
                    }
                    else if (lb.ID == "lbHighQuantity")
                    {
                        isDescendHighQuantity = true;
                    }
                    else
                    {
                        isDescendDiscount = true;
                    }

                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            gvQuantityDiscount.PageIndex = 0;
            gvQuantityDiscount.DataBind();
            TRUpdateQtyDiscount.Visible = false;
            TRUpdateQtyDiscount1.Visible = false;
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            gvQuantityDiscount.PageIndex = 0;
            gvQuantityDiscount.DataBind();
            TRUpdateQtyDiscount.Visible = false;
            TRUpdateQtyDiscount1.Visible = false;
        }

        /// <summary>
        /// Quantity Discount Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvQuantityDiscount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendQuantityDiscount == false)
                {
                    ImageButton lbQuantityDiscount = (ImageButton)e.Row.FindControl("lbQuantityDiscount");
                    lbQuantityDiscount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbQuantityDiscount.AlternateText = "Ascending Order";
                    lbQuantityDiscount.ToolTip = "Ascending Order";
                    lbQuantityDiscount.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbQuantityDiscount = (ImageButton)e.Row.FindControl("lbQuantityDiscount");
                    lbQuantityDiscount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbQuantityDiscount.AlternateText = "Descending Order";
                    lbQuantityDiscount.ToolTip = "Descending Order";
                    lbQuantityDiscount.CommandArgument = "ASC";
                }
                if (isDescendstname == false)
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstname.AlternateText = "Ascending Order";
                    lbstname.ToolTip = "Ascending Order";
                    lbstname.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstname = (ImageButton)e.Row.FindControl("lbstname");
                    lbstname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstname.AlternateText = "Descending Order";
                    lbstname.ToolTip = "Descending Order";
                    lbstname.CommandArgument = "ASC";
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Quantity Discount List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvListQuantityDiscount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvListQuantityDiscount.Rows.Count > 0)
            {
                TRUpdateQtyDiscount1.Visible = true;
            }
            else
            {
                TRUpdateQtyDiscount1.Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (isDescendLowQuantity == false)
                {
                    ImageButton lbLowQuantity = (ImageButton)e.Row.FindControl("lbLowQuantity");
                    lbLowQuantity.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbLowQuantity.AlternateText = "Ascending Order";
                    lbLowQuantity.ToolTip = "Ascending Order";
                    lbLowQuantity.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbLowQuantity = (ImageButton)e.Row.FindControl("lbLowQuantity");
                    lbLowQuantity.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbLowQuantity.AlternateText = "Descending Order";
                    lbLowQuantity.ToolTip = "Descending Order";
                    lbLowQuantity.CommandArgument = "ASC";
                }
                if (isDescendHighQuantity == false)
                {
                    ImageButton lbHighQuantity = (ImageButton)e.Row.FindControl("lbHighQuantity");
                    lbHighQuantity.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbHighQuantity.AlternateText = "Ascending Order";
                    lbHighQuantity.ToolTip = "Ascending Order";
                    lbHighQuantity.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbHighQuantity = (ImageButton)e.Row.FindControl("lbHighQuantity");
                    lbHighQuantity.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbHighQuantity.AlternateText = "Descending Order";
                    lbHighQuantity.ToolTip = "Descending Order";
                    lbHighQuantity.CommandArgument = "ASC";
                }
                if (isDescendDiscount == false)
                {
                    ImageButton lbDiscount = (ImageButton)e.Row.FindControl("lbDiscount");
                    lbDiscount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbDiscount.AlternateText = "Ascending Order";
                    lbDiscount.ToolTip = "Ascending Order";
                    lbDiscount.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbDiscount = (ImageButton)e.Row.FindControl("lbDiscount");
                    lbDiscount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbDiscount.AlternateText = "Descending Order";
                    lbDiscount.ToolTip = "Descending Order";
                    lbDiscount.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Quantity Discount Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvQuantityDiscount_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Select")
            {
                TRUpdateQtyDiscount.Visible = true;
                TRUpdateQtyDiscount1.Visible = true;
                try
                {
                    discountID = Convert.ToInt32(e.CommandArgument.ToString());
                    //foreach (GridViewRow gr in gvQuantityDiscount.Rows)
                    //{
                    //    HiddenField lblID = (HiddenField)gr.FindControl("hdnQuantityDiscountID");
                    //    if (lblID.Value == e.CommandArgument.ToString())
                    //    {
                    //        Label lbName = (Label)gr.FindControl("lblName");
                    //        //lblUpdate.Text = "Update Quantity Discount" + " : " + lbName.Text.Trim();
                    //        // btnDelete.Visible = true;
                    //    }
                    //}
                    BindgvQuantityDiscount(e.CommandArgument.ToString());
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message;
                    lblMsg.CssClass = "error";
                    //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->gvDiscount_RowCommand \r\n Date: " + System.DateTime.Now + "\r\n");
                }
            }
            if (e.CommandName == "EditQuantityDiscount")
            {
                TRUpdateQtyDiscount.Visible = false;
                TRUpdateQtyDiscount1.Visible = false;
                foreach (GridViewRow dr in gvQuantityDiscount.Rows)
                {
                    HiddenField lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    if (lblID.Value == e.CommandArgument.ToString())
                    {
                        Label lbName = (Label)dr.FindControl("lblName");
                        TextBox txtName = (TextBox)dr.FindControl("txtName");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lbName.Visible = false;
                        txtName.Visible = true;
                    }
                }
            }

            if (e.CommandName == "Add")
            {
                foreach (GridViewRow dr in gvQuantityDiscount.Rows)
                {
                    HiddenField lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    if (lblID.Value == e.CommandArgument.ToString())
                    {
                        TextBox txtName = (TextBox)dr.FindControl("txtName");
                        Label lbName = (Label)dr.FindControl("lblName");
                        Label lbStoreID = (Label)dr.FindControl("lblStoreID");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");

                        string SpaceCkeck = txtName.Text;
                        string[] SpaceCkeck1 = SpaceCkeck.Split(' ');

                        if (txtName.Text != "" && SpaceCkeck1[0] != "")
                        {
                            objQuantityDiscount = new QuantityDiscountComponent();
                            tblQuantityDiscount = new tb_QauntityDiscount();

                            tblQuantityDiscount = objQuantityDiscount.GetQauntityDiscount(Convert.ToInt32(lblID.Value.Trim()));
                            string nametemp = tblQuantityDiscount.Name;
                            tblQuantityDiscount.Name = txtName.Text.Trim();

                            tblQuantityDiscount.UpdatedOn = DateTime.Now;
                            tblQuantityDiscount.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                            if (objQuantityDiscount.CheckDuplicateforupdate(tblQuantityDiscount, nametemp))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Quantity Discount with same Name already exists, please specify another Name...', 'Message','');});", true);
                                return;
                            }
                            Int32 isupdated = objQuantityDiscount.UpdateQauntityDiscount(tblQuantityDiscount);
                            if (isupdated > 0)
                            {
                                btnSave.Visible = false;
                                btnCancel.Visible = false;
                                btnEdit.Visible = true;
                                lbName.Visible = true;
                                txtName.Visible = false;
                                gvQuantityDiscount.DataBind();
                            }
                            else
                            {
                                lblMsg.Text = "Quantity Discounts with given name already exists...";
                                lblMsg.CssClass = "error";
                            }
                        }
                        else
                        {
                            if (txtName.Text == "")
                            {
                                lblMsg.Text = "Please enter quantity discount name...";
                                lblMsg.CssClass = "error";
                            }
                            else
                            {
                                lblMsg.Text = "Please enter valid quantity discount name...";
                                lblMsg.CssClass = "error";
                            }
                        }
                    }
                }
            }

            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow dr in gvQuantityDiscount.Rows)
                {
                    HiddenField lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    if (lblID.Value == e.CommandArgument.ToString())
                    {
                        TextBox txtName = (TextBox)dr.FindControl("txtName");
                        DropDownList ddlStore = (DropDownList)dr.FindControl("drpdwnStore");
                        Label lbName = (Label)dr.FindControl("lblName");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lbName.Visible = true;
                        txtName.Visible = false;


                    }
                }
            }


            //if (e.CommandName == "edit")
            //{
            //    try
            //    {
            //        int QuantityDiscountID = Convert.ToInt32(e.CommandArgument);
            //        Response.Redirect("QuantityDiscount.aspx?QuantityDiscountID=" + QuantityDiscountID);
            //    }
            //    catch
            //    { }
            //}
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

            gvQuantityDiscount.PageIndex = 0;
            gvQuantityDiscount.DataBind();
            trBottom.Visible = false;
            TRUpdateQtyDiscount.Visible = false;
            TRUpdateQtyDiscount1.Visible = false;
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            Delete("Quantity Discount");
        }

        /// <summary>
        /// Bind the Quantity Discount Grid view
        /// </summary>
        /// <param name="ID">String ID</param>
        private void BindgvQuantityDiscount(string ID)
        {
            objQuantityDiscount = new QuantityDiscountComponent();

            List<tb_QuantityDiscountTable> lstobj = new List<tb_QuantityDiscountTable>();
            lstobj = objQuantityDiscount.GetQauntityDiscountnew(Convert.ToInt32(ID));
            if (lstobj != null && lstobj.Count > 0)
            {
                gvListQuantityDiscount.DataSource = lstobj;
                gvListQuantityDiscount.DataBind();
            }
            else
            {
                lblMsg.Text = "No Discount Information exists... ";
                lblMsg.CssClass = "error";
                DataTable dtSingleLine = new DataTable("QuantityDiscount");
                dtSingleLine.Columns.Add(new DataColumn("LowQuantity"));
                dtSingleLine.Columns.Add(new DataColumn("HighQuantity"));
                dtSingleLine.Columns.Add(new DataColumn("DiscountPercent"));
                dtSingleLine.Columns.Add(new DataColumn("CreatedOn"));
                dtSingleLine.Columns.Add(new DataColumn("QuantityDiscountID"));
                dtSingleLine.Columns.Add(new DataColumn("QuantityDiscountTableID"));
                DataRow NewRow = dtSingleLine.NewRow();
                NewRow["LowQuantity"] = 0;
                NewRow["HighQuantity"] = 0;
                NewRow["DiscountPercent"] = 0;
                NewRow["CreatedOn"] = DateTime.MinValue;
                NewRow["QuantityDiscountID"] = 0;
                NewRow["QuantityDiscountTableID"] = ID;


                dtSingleLine.Rows.Add(NewRow);
                gvListQuantityDiscount.DataSource = dtSingleLine;
                gvListQuantityDiscount.DataBind();
                gvListQuantityDiscount.Rows[0].Visible = false;
                //gvListQuantityDiscount.DataSource = null;
                //     TRUpdateQtyDiscount.Visible = true;
                TRUpdateQtyDiscount1.Visible = true;
            }
            gvListQuantityDiscount.DataBind();
        }

        /// <summary>
        /// Quantity Discount List Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvListQuantityDiscount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            objQuantityDiscount = new QuantityDiscountComponent();
            if (e.CommandName == "Select")
            {
                foreach (GridViewRow dr in gvListQuantityDiscount.Rows)
                {
                    HiddenField lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    Label lblQuantityDiscountTableID = (Label)dr.FindControl("lblQuantityDiscountTableID");
                    if (lblQuantityDiscountTableID.Text == e.CommandArgument.ToString())
                    {
                        Label lbLow = (Label)dr.FindControl("lblLowQuantity");
                        Label lbHigh = (Label)dr.FindControl("lblHighQuantity");
                        Label lbDiscount = (Label)dr.FindControl("lblDiscount");
                        TextBox txtLow = (TextBox)dr.FindControl("txtAddLowQuantity");
                        TextBox txtHigh = (TextBox)dr.FindControl("txtAddHighQuantity");
                        TextBox txtDiscount = (TextBox)dr.FindControl("txtAddDiscount");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lbLow.Visible = false;
                        lbHigh.Visible = false;
                        lbDiscount.Visible = false;
                        txtLow.Visible = true;
                        txtHigh.Visible = true;
                        txtDiscount.Visible = true;
                    }
                }
            }

            if (e.CommandName == "Add")
            {
                foreach (GridViewRow dr in gvListQuantityDiscount.Rows)
                {
                    HiddenField lblQuantityDiscountID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    Label lblQuantityDiscountTableID = (Label)dr.FindControl("lblQuantityDiscountTableID");
                    if (lblQuantityDiscountTableID.Text == e.CommandArgument.ToString())
                    {
                        Label lbLow = (Label)dr.FindControl("lblLowQuantity");
                        Label lbHigh = (Label)dr.FindControl("lblHighQuantity");
                        Label lbDiscount = (Label)dr.FindControl("lblDiscount");
                        TextBox txtLow = (TextBox)dr.FindControl("txtAddLowQuantity");
                        TextBox txtHigh = (TextBox)dr.FindControl("txtAddHighQuantity");
                        TextBox txtDiscount = (TextBox)dr.FindControl("txtAddDiscount");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        if (Convert.ToInt32(txtLow.Text.Trim()) < Convert.ToInt32(txtHigh.Text.Trim()))
                        {

                            int result = objQuantityDiscount.UpdateQuantityDiscountTable(Convert.ToInt32(lblQuantityDiscountTableID.Text.Trim()), Convert.ToInt32(lblQuantityDiscountID.Value.Trim()), Convert.ToInt32(txtLow.Text.Trim()), Convert.ToInt32(txtHigh.Text.Trim()), Convert.ToDecimal(txtDiscount.Text.Trim()));
                            if (result != -1)
                            {
                                btnSave.Visible = false;
                                btnCancel.Visible = false;
                                btnEdit.Visible = true;
                                lbLow.Visible = true;
                                lbHigh.Visible = true;
                                lbDiscount.Visible = true;
                                txtLow.Visible = false;
                                txtHigh.Visible = false;
                                txtDiscount.Visible = false;
                                BindgvQuantityDiscount(lblQuantityDiscountID.Value);
                            }
                            else
                            {
                                lblMsg.Text = "Given Information Already Exists...";
                                lblMsg.CssClass = "error";
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('High Value Should Be Greater than Low Value');", true);
                        }
                    }
                }
            }


            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow dr in gvListQuantityDiscount.Rows)
                {
                    HiddenField lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                    Label lblQuantityDiscountTableID = (Label)dr.FindControl("lblQuantityDiscountTableID");

                    if (lblQuantityDiscountTableID.Text == e.CommandArgument.ToString())
                    {
                        Label lbLow = (Label)dr.FindControl("lblLowQuantity");
                        Label lbHigh = (Label)dr.FindControl("lblHighQuantity");
                        Label lbDiscount = (Label)dr.FindControl("lblDiscount");
                        TextBox txtLow = (TextBox)dr.FindControl("txtAddLowQuantity");
                        TextBox txtHigh = (TextBox)dr.FindControl("txtAddHighQuantity");
                        TextBox txtDiscount = (TextBox)dr.FindControl("txtAddDiscount");
                        ImageButton btnEdit = (ImageButton)dr.FindControl("_editLinkButton");
                        ImageButton btnSave = (ImageButton)dr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)dr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lbLow.Visible = true;
                        lbHigh.Visible = true;
                        lbDiscount.Visible = true;
                        txtLow.Visible = false;
                        txtHigh.Visible = false;
                        txtDiscount.Visible = false;
                    }
                }
            }

            if (e.CommandName == "Footer Add")
            {
                HiddenField lblID = new HiddenField();
                foreach (GridViewRow dr in gvListQuantityDiscount.Rows)
                {
                    lblID = (HiddenField)dr.FindControl("hdnQuantityDiscountID");
                }
                TextBox txtLow = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewLowQuantity");
                TextBox txtHigh = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewHighQuantity");
                TextBox txtDiscount = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewDiscount");
                ImageButton btnSave = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnSave");
                ImageButton btnCancel = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnCancel");

                if (Convert.ToInt32(txtLow.Text.Trim()) < Convert.ToInt32(txtHigh.Text.Trim()))
                {
                    int result = objQuantityDiscount.AddQuantityDiscountTable(Convert.ToInt32(lblID.Value), Convert.ToInt32(txtLow.Text.Trim()), Convert.ToInt32(txtHigh.Text.Trim()), Convert.ToDecimal(txtDiscount.Text.Trim()));
                    if (result != -1)
                    {
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        txtLow.Visible = false;
                        txtHigh.Visible = false;
                        txtDiscount.Visible = false;
                        BindgvQuantityDiscount(lblID.Value);
                    }
                    else
                    {
                        lblMsg.Text = "Record Already Exists...";
                        lblMsg.CssClass = "error";
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('High Value Should Be Greater than Low Value');", true);
                }

            }
            if (e.CommandName == "Footer Stop")
            {
                TextBox txtLow = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewLowQuantity");
                TextBox txtHigh = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewHighQuantity");
                TextBox txtDiscount = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewDiscount");
                ImageButton btnSave = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnSave");
                ImageButton btnCancel = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnCancel");
                btnCancel.Visible = false;
                btnSave.Visible = false;
                txtLow.Visible = false;
                txtHigh.Visible = false;
                txtDiscount.Visible = false;
                txtDiscount.Text = "";
                txtHigh.Text = "";
                txtLow.Text = "";
            }
        }

        /// <summary>
        /// Adds the new discount row command
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">CommandEventArgs e</param>
        protected void AddNewDiscountRowCommand(object sender, CommandEventArgs e)
        {
            if (gvListQuantityDiscount.Rows.Count > 0 && (gvListQuantityDiscount.Rows[0].Cells[0].FindControl("lblQuantityDiscountTableID") as Label).Text == "0")
                gvListQuantityDiscount.Rows[0].Visible = false;
            TextBox txtLow = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewLowQuantity");
            TextBox txtHigh = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewHighQuantity");
            TextBox txtDiscount = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtNewDiscount");
            ImageButton btnSave = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnSave");
            ImageButton btnCancel = (ImageButton)gvListQuantityDiscount.FooterRow.FindControl("btnCancel");
            btnCancel.Visible = true;
            btnSave.Visible = true;
            txtLow.Visible = true;
            txtHigh.Visible = true;
            txtDiscount.Visible = true;
        }

        /// <summary>
        /// Deletes the Quantity discount
        /// </summary>
        /// <param name="Value">string Value</param>
        public void Delete(string Value)
        {
            objQuantityDiscount = new QuantityDiscountComponent();
            if (Value == "Quantity Discount")
            {
                int count = 0;
                int indx = 0;
                foreach (GridViewRow r in gvQuantityDiscount.Rows)
                {
                    CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                    HiddenField lb = (HiddenField)r.FindControl("hdnQuantityDiscountID");
                    int ID = Convert.ToInt32(lb.Value.ToString());
                    if (chk.Checked)
                    {
                        count++;
                        indx = objQuantityDiscount.DeleteQauntityDiscount(ID);
                    }
                }


                if (count == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select at least one record...');", true);
                }
                else if (indx == 1)
                {
                    //lblUpdate.Text = "";
                    //BindQuantityDiscountForStoreID();
                    gvQuantityDiscount.DataBind();
                }
                else if (indx == -1)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('This Quantity Name is Assigned to Product, So First Delete From Product Table...');", true);
                }
            }
            else
            {
                int count = 0;

                if (gvListQuantityDiscount.Rows.Count != 1)
                {
                    foreach (GridViewRow r in gvListQuantityDiscount.Rows)
                    {
                        CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                        Label lb = (Label)r.FindControl("lblQuantityDiscountTableID");
                        int ID = Convert.ToInt32(lb.Text.ToString());
                        if (chk.Checked)
                        {
                            count++;
                            int indx = objQuantityDiscount.DeleteQuantityDiscountTable(ID);
                        }
                    }
                    if (count == 0)
                    {
                        lblMsg.Text = "Please select at least one record...";
                        lblMsg.CssClass = "error";
                    }
                    else
                    {
                        BindgvQuantityDiscount(discountID.ToString());
                    }
                }
                else
                {
                    foreach (GridViewRow r in gvListQuantityDiscount.Rows)
                    {
                        CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                        if (chk.Checked)
                        {
                            ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('This is last Record So you can not delete');", true);
                        }
                        else
                        {
                            lblMsg.Text = "Please Select At least one Record...";
                            lblMsg.CssClass = "error";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sorting  function for grid view
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sortingnew(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    gvListQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    gvListQuantityDiscount.DataBind();
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbLowQuantity")
                    {
                        isDescendLowQuantity = false;
                    }
                    else if (lb.ID == "lbHighQuantity")
                    {
                        isDescendHighQuantity = false;
                    }
                    else
                    {
                        isDescendDiscount = false;
                    }


                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvListQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbLowQuantity")
                    {
                        isDescendLowQuantity = true;
                    }
                    else if (lb.ID == "lbHighQuantity")
                    {
                        isDescendHighQuantity = true;
                    }
                    else
                    {
                        isDescendDiscount = true;
                    }

                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Delete Quantity Table Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndeleteQuantitytable_Click(object sender, EventArgs e)
        {
            Delete("Quantity Discount Table");
        }

    }
}