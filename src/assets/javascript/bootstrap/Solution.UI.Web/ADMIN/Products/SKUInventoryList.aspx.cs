using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.IO;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class SKUInventoryList : BasePage
    {
        ProductComponent objproduct = null;
        public static bool isDescendproductname = false;
        public static bool isDescendstname = false;
        public static bool isDescendproductcode = false;
        public static bool isDescendourprice = false;
        public static bool Issearch = false;
        public static string SearchBy = null;
        public static string SearchValue = null;
        int StoreID = 0;
        public static DataView DvProduct = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Insert"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('SKU inserted successfully.', 'Message');});", true);

                }
                else if (Request.QueryString["Update"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('SKU updated successfully.', 'Message');});", true);
                }
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";

                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";


                //btnAddNew.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/add-quantity-discount.png) no-repeat transparent; width: 154px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                // btndeleteQuantitytable.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                SearchBy = "";
                SearchValue = "";
                binddata();

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
                StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                StoreID = 1;
                ddlStore.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Bind Gift Card Product Data
        /// </summary>
        private void binddata()
        {
            DataSet dsproduct = new DataSet();
            txtToEmail.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where StoreID = " + ddlStore.SelectedValue.ToString() + "  and ConfigName = 'InventoryToEmail'"));
            txtBCCEmail.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where StoreID = " + ddlStore.SelectedValue.ToString() + "  and ConfigName = 'InventoryBCCEmail'"));


            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
            {
                DataSet dsData = CommonComponent.GetCommonDataSet("select * from tb_SKUInventory where ID = " + Request.QueryString["ID"].ToString());
                if (dsData != null && dsData.Tables.Count > 0 & dsData.Tables[0].Rows.Count > 0)
                {
                    txtSKU.Text = dsData.Tables[0].Rows[0]["SKU"].ToString();
                }
            }
            else
            {

            }

            objproduct = new ProductComponent();

            string strQuery = string.Empty;

            strQuery = "select * from tb_SKUInventory";

            if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {
                strQuery += " where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%'";
            }

            dsproduct = CommonComponent.GetCommonDataSet(strQuery);//objproduct.GetProductColorList(0, Issearch, SearchBy, SearchValue);

            if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
            {
                DvProduct = dsproduct.Tables[0].DefaultView;
                gvProductSize.DataSource = dsproduct;
                gvProductSize.DataBind();
                Issearch = false;
            }
            else
            {
                gvProductSize.DataSource = null;
                gvProductSize.DataBind();
            }
        }

        /// <summary>
        /// Sorting function for Grid view
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                DataView dv = new DataView();
                if (DvProduct != null)
                {
                    dv = DvProduct;
                    dv.Sort = lb.CommandName + " " + lb.CommandArgument;
                    if (lb.CommandArgument == "ASC")
                    {
                        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                        if (lb.ID == "lbSKU")
                        {
                            isDescendproductname = false;
                        }


                        lb.AlternateText = "Descending Order";
                        lb.ToolTip = "Descending Order";
                        lb.CommandArgument = "DESC";
                    }
                    else if (lb.CommandArgument == "DESC")
                    {
                        //          gvListQuantityDiscount.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                        lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                        if (lb.ID == "lbSKU")
                        {
                            isDescendproductname = true;
                        }


                        lb.AlternateText = "Ascending Order";
                        lb.ToolTip = "Ascending Order";
                        lb.CommandArgument = "ASC";
                    }
                    gvProductSize.DataSource = dv;
                    gvProductSize.DataBind();
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
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                Issearch = true;
                SearchBy = ddlSearch.SelectedItem.Value;
                SearchValue = txtSearch.Text.Trim();
            }
            else
            {
                SearchBy = "";
                SearchValue = "";
            }
            binddata();


        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnsearch_Click(object sender, ImageClickEventArgs e)
        {
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                Issearch = true;
                SearchBy = ddlSearch.SelectedItem.Value;
                SearchValue = txtSearch.Text.Trim();
            }
            else
            {
                SearchBy = "";
                SearchValue = "";
            }
            binddata();

        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnShowall_Click(object sender, ImageClickEventArgs e)
        {
            ddlStore.SelectedIndex = 0;
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            txtSearch.Text = "";
            SearchBy = "";
            SearchValue = "";
            binddata();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {


            int count = 0;
            int indx = 0;
            foreach (GridViewRow r in gvProductSize.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                Label lb = (Label)r.FindControl("ID");
                int ID = Convert.ToInt32(lb.Text.ToString());
                if (chk.Checked)
                {
                    count++;
                    //indx = ProductComponent.DeleteProductColor(Convert.ToInt32(ID));
                    CommonComponent.ExecuteCommonData("delete from tb_SKUInventory where ID = " + ID.ToString() + "");
                    count = 1;
                }
            }

            if (count == 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select at least one sku...');", true);
            }
            else if (indx == 1)
            {
                binddata();
            }
            else if (indx == -1)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('This Quantity Name is Assigned to Product,So First Delete From Product Table...');", true);
            }
            StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            txtSearch.Text = "";
            SearchBy = "";
            SearchValue = "";
            binddata();

        }

        /// <summary>
        /// Gift Card Product Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvProductSize_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                try
                {
                    int ColorID = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("SKUInventoryList.aspx?Mode=Edit&ID=" + ColorID);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Gift Card Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvProductSize_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvProductSize.Rows.Count > 0)
            {
                Productdata.Visible = true;
            }
            else
            {
                Productdata.Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {

                if (isDescendproductname == false)
                {
                    ImageButton lbSKU = (ImageButton)e.Row.FindControl("lbSKU");
                    lbSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbSKU.AlternateText = "Ascending Order";
                    lbSKU.ToolTip = "Ascending Order";
                    lbSKU.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbSKU = (ImageButton)e.Row.FindControl("lbSKU");
                    lbSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbSKU.AlternateText = "Descending Order";
                    lbSKU.ToolTip = "Descending Order";
                    lbSKU.CommandArgument = "ASC";
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");
                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
            }
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Product Image Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Color/Micro/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Color/Micro/image_not_available.jpg");
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Edit")
            {
                Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(SKU),0) as TotCnt From tb_SKUInventory Where SKU='" + txtSKU.Text.ToString().Trim() + "' And  ID <>" + Request.QueryString["ID"].ToString() + " "));
                if (ChkSKu > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU is already Exists', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                    txtSKU.Focus();
                    return;
                }
                else
                {
                    CommonComponent.ExecuteCommonData("update tb_SKUInventory set SKU = '" + txtSKU.Text.ToString().Trim().Replace("'", "''") + "' where ID = " + Request.QueryString["ID"].ToString() + "");

                    if (!string.IsNullOrWhiteSpace(txtToEmail.ToString().Trim().Replace("'", "''")))
                        CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue = '" + txtToEmail.Text.ToString().Trim().Replace("'", "''") + "' where ConfigName ='InventoryToEmail'");

                    if (!string.IsNullOrWhiteSpace(txtBCCEmail.ToString().Trim().Replace("'", "''")))
                        CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue = '" + txtBCCEmail.Text.ToString().Trim().Replace("'", "''") + "' where ConfigName ='InventoryBCCEmail'");

                    Response.Redirect("SKUInventoryList.aspx?Update=true");
                }
            }
            else
            {
                Int32 IsExists = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(count(1),0) as cCount from tb_SKUInventory where SKU = '" + txtSKU.Text.ToString().Trim().Replace("'", "''") + "'"));
                if (IsExists > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "IsExists", "$(document).ready( function() {jAlert('SKU is already Exists', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                    txtSKU.Focus();
                    return;
                }
                else
                {
                    CommonComponent.ExecuteCommonData("insert into tb_SKUInventory(SKU) values('" + txtSKU.Text.ToString().Trim().Replace("'", "''") + "')");
                    if (!string.IsNullOrWhiteSpace(txtToEmail.ToString().Trim().Replace("'", "''")))
                        CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue = '" + txtToEmail.Text.ToString().Trim().Replace("'", "''") + "' where ConfigName ='InventoryToEmail'");

                    if (!string.IsNullOrWhiteSpace(txtBCCEmail.ToString().Trim().Replace("'", "''")))
                        CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue = '" + txtBCCEmail.Text.ToString().Trim().Replace("'", "''") + "' where ConfigName ='InventoryBCCEmail'");
                    Response.Redirect("SKUInventoryList.aspx?Insert=true");
                }
            }
        }


        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("SKUInventoryList.aspx");
        }

    }
}