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
    public partial class QuantityDiscount1 : BasePage
    {

        int discountID = 0;
        DataTable tableQuantityDiscount = new DataTable("dtQuantityDiscount");
        DataColumn myDataColumn;

        tb_QauntityDiscount tblQuantityDiscount = null;
        QuantityDiscountComponent objQuantityDiscount = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (Request.QueryString["ProductStoreID"] != null)
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(Request.QueryString["ProductStoreID"].ToString()));
                }
                GenerateTable();
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
        /// Quantity Discount Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvListQuantityDiscount_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            tableQuantityDiscount = (DataTable)ViewState["tbDiscount"];

            if (e.CommandName.Equals("Add New"))
            {

                TextBox txtLow1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddLowQuantity");
                TextBox txtHigh1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddHighQuantity");
                if (Convert.ToInt32(txtLow1.Text.Trim()) > Convert.ToInt32(txtHigh1.Text.Trim()) || Convert.ToInt32(txtHigh1.Text.Trim()) < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('High Value Should Be Greater than Low Value', 'Message','');});", true);
                    return;
                }
                TextBox txtDiscount1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddDiscount");
                if (Convert.ToInt32(txtLow1.Text.Trim()) < Convert.ToInt32(txtHigh1.Text.Trim()))
                {

                    DataRow dr1 = tableQuantityDiscount.NewRow();
                    dr1["LowQuantity"] = Convert.ToInt32(txtLow1.Text.Trim());
                    dr1["HighQuantity"] = Convert.ToInt32(txtHigh1.Text.Trim());
                    dr1["DiscountPercent"] = Convert.ToDecimal(txtDiscount1.Text.Trim());
                    tableQuantityDiscount.Rows.Add(dr1);
                    ViewState["tbDiscount"] = tableQuantityDiscount;
                    BindData();
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('High value should be greater than Low value...');", true);
                }
            }
        }

        /// <summary>
        /// Quantity Discount Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvListQuantityDiscount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            tableQuantityDiscount = (DataTable)ViewState["tbDiscount"];
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                DetailsView dv = (DetailsView)e.Row.FindControl("DetailsView1");
                dv.DataSource = tableQuantityDiscount;
                dv.DataBind();
            }
        }

        /// <summary>
        /// Generates the table for Store Dynamic Data for Quantity discount
        /// </summary>
        public void GenerateTable()
        {
            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "LowQuantity";
            tableQuantityDiscount.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Int32");
            myDataColumn.ColumnName = "HighQuantity";
            tableQuantityDiscount.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = System.Type.GetType("System.Decimal");
            myDataColumn.ColumnName = "DiscountPercent";
            tableQuantityDiscount.Columns.Add(myDataColumn);
            ViewState["tbDiscount"] = tableQuantityDiscount;
            BindData();
        }

        /// <summary>
        /// Binds the data of Quantity Discount
        /// </summary>
        public void BindData()
        {
            if (tableQuantityDiscount.Rows.Count == 0)
            {
                gvListQuantityDiscount.DataSource = tableQuantityDiscount;
                EmptyGridFix(gvListQuantityDiscount);
                gvListQuantityDiscount.DataBind();
            }
            else
            {
                gvListQuantityDiscount.DataSource = tableQuantityDiscount;
                gvListQuantityDiscount.DataBind();
            }
        }

        /// <summary>
        /// Empty the grid
        /// </summary>
        /// <param name="grdView">GridView grdview</param>
        protected void EmptyGridFix(GridView grdView)
        {
            if (grdView.Rows.Count == 0 && grdView.DataSource != null)
            {
                DataTable dt = null;
                if (grdView.DataSource is DataSet)
                {
                    dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
                }
                else if (grdView.DataSource is DataTable)
                {
                    dt = ((DataTable)grdView.DataSource).Clone();
                }
                if (dt == null)
                {
                    return;
                }

                dt.Rows.Add(dt.NewRow());
                grdView.DataSource = dt;
                grdView.DataBind();

                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }
            if (grdView.Rows.Count == 1 &&
                grdView.DataSource == null)
            {
                bool bIsGridEmpty = true;
                for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
                {
                    if (grdView.Rows[0].Cells[i].Text != string.Empty)
                    {
                        bIsGridEmpty = false;
                    }
                }
                if (bIsGridEmpty)
                {
                    grdView.Rows[0].Visible = false;
                    grdView.Rows[0].Controls.Clear();
                }
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Insert();
        }

        /// <summary>
        /// Inserts a Record of Quantity Discount
        /// </summary>
        public void Insert()
        {
            int LowQuantity;
            int HighQuantity;

            for (int i = 0; i < tableQuantityDiscount.Rows.Count; i++)
            {
                LowQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[0].ToString());
                HighQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[1].ToString());
                if (LowQuantity > HighQuantity || HighQuantity < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('High Value Should Be Greater than Low Value', 'Message','');});", true);
                    return;
                }
            }
            TextBox txtLow1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddLowQuantity");
            TextBox txtHigh1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddHighQuantity");
            if (txtHigh1.Text.Trim() != "" && txtLow1.Text.Trim() != "")
            {
                if (Convert.ToInt32(txtLow1.Text.Trim()) > Convert.ToInt32(txtHigh1.Text.Trim()) || Convert.ToInt32(txtHigh1.Text.Trim()) < 1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('High Value Should Be Greater than Low Value', 'Message','');});", true);
                    return;
                }
            }
            objQuantityDiscount = new QuantityDiscountComponent();
            tblQuantityDiscount = new tb_QauntityDiscount();
            int QuantityDiscountID;

            decimal DiscountPercent;


            bool flag = false;
            bool FlagForDiscount = false;



            tableQuantityDiscount = (DataTable)ViewState["tbDiscount"];

            TextBox txtDiscount1 = (TextBox)gvListQuantityDiscount.FooterRow.FindControl("txtAddDiscount");
            if (txtLow1.Text != "" && txtHigh1.Text != "" && txtDiscount1.Text != "")
            {
                FlagForDiscount = false;
            }
            else
            {
                FlagForDiscount = true;
            }
            if (!FlagForDiscount)
            {
                tblQuantityDiscount.Name = txtName.Text.Trim();

                if (objQuantityDiscount.CheckDuplicate(tblQuantityDiscount))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Quantity Discount with same Name already exists, please specify another Name...', 'Message','');});", true);
                    return;
                }

                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tblQuantityDiscount.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                tblQuantityDiscount.CreatedOn = DateTime.Now;
                tblQuantityDiscount.CreatedBy = Convert.ToInt32(Session["AdminID"]);

                discountID = objQuantityDiscount.CreateQauntityDiscount(tblQuantityDiscount);

                if (discountID != -1)
                {
                    for (int i = 0; i < tableQuantityDiscount.Rows.Count; i++)
                    {
                        QuantityDiscountID = discountID;
                        LowQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[0].ToString());
                        HighQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[1].ToString());
                        DiscountPercent = Convert.ToDecimal(tableQuantityDiscount.Rows[i].ItemArray[2].ToString());

                        if (objQuantityDiscount.AddQuantityDiscountTable(QuantityDiscountID, LowQuantity, HighQuantity, DiscountPercent) != -1)
                        {
                            flag = true;
                        }

                    }
                    QuantityDiscountID = discountID;
                    LowQuantity = Convert.ToInt32(txtLow1.Text.Trim());
                    HighQuantity = Convert.ToInt32(txtHigh1.Text.Trim());

                    DiscountPercent = Convert.ToDecimal(txtDiscount1.Text.Trim());
                    if (objQuantityDiscount.AddQuantityDiscountTable(QuantityDiscountID, LowQuantity, HighQuantity, DiscountPercent) != -1)
                    {
                        flag = true;
                    }

                    if (flag)
                    {
                        if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
                        {
                            Response.Redirect("../Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit");
                        }
                        else if (Request.QueryString["ProductStoreID"] != null)
                        {
                            Response.Redirect("../Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "");
                        }
                        else
                        {
                            Response.Redirect("QuantityDiscountList.aspx?Insert=true");
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Quantity discount with same name already exits, please specify another name...";
                        lblMsg.CssClass = "error";
                    }
                }
                else
                {
                    lblMsg.Text = "Quantity discount with same name already exits, please specify another name...";
                    lblMsg.CssClass = "error";
                }
            }
            else
            {
                if (tableQuantityDiscount.Rows.Count != 0)
                {
                    tblQuantityDiscount.Name = txtName.Text.Trim();

                    if (objQuantityDiscount.CheckDuplicate(tblQuantityDiscount))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Quantity Discount with same Name already exists, please specify another Name...', 'Message','');});", true);
                        return;
                    }

                    int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                    tblQuantityDiscount.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                    tblQuantityDiscount.CreatedOn = DateTime.Now;
                    tblQuantityDiscount.CreatedBy = Convert.ToInt32(Session["AdminID"]);

                    discountID = objQuantityDiscount.CreateQauntityDiscount(tblQuantityDiscount);

                    if (discountID != -1)
                    {
                        for (int i = 0; i < tableQuantityDiscount.Rows.Count; i++)
                        {
                            QuantityDiscountID = discountID;
                            LowQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[0].ToString());
                            HighQuantity = Convert.ToInt32(tableQuantityDiscount.Rows[i].ItemArray[1].ToString());
                            DiscountPercent = Convert.ToDecimal(tableQuantityDiscount.Rows[i].ItemArray[2].ToString());
                            if (objQuantityDiscount.AddQuantityDiscountTable(QuantityDiscountID, LowQuantity, HighQuantity, DiscountPercent) != -1)
                            {
                                flag = true;
                            }
                        }
                    }

                    if (flag)
                    {
                        Response.Redirect("QuantityDiscountList.aspx?Insert=true");
                    }
                    else
                    {

                        lblMsg.Text = "Quantity discount with same name already exits, please specify another name...";
                        lblMsg.CssClass = "error";

                    }
                }
                else
                {
                    lblMsg.Text = "Please insert values for Discount table...";
                    lblMsg.CssClass = "error";
                }
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
            {
                Response.Redirect("../Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit");
            }
            else if (Request.QueryString["ProductStoreID"] != null)
            {
                Response.Redirect("../Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "");
            }
            else
            {
                Response.Redirect("QuantityDiscountList.aspx");
            }
        }
    }
}