using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AssemblerProductPopUp : Solution.UI.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["mode"] != null)
                {
                    BindFeature();
                    ViewState["SelectedSKUs"] = "";
                    ViewState["SelectedQty"] = "";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "loadVendorsku();", true);
                }

            }
        }
        protected void ibtnProductsearch_Click(object sender, ImageClickEventArgs e)
        {
            //if (txtProduct.Text != "")
            BindProduct();
        }
        protected void ibtnProductshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtFeaturesystem.Text = "";
            BindProduct();
        }
        protected void ibtnProductaddtoselectionlist_Click(object sender, ImageClickEventArgs e)
        {

            int Cnt = 0;
            if (grdProducts.Rows.Count > 0)
            {
                for (int i = 0; i < grdProducts.Rows.Count; i++)
                {
                    // int ProductId = Convert.ToInt32(((Label)grdProducts.Rows[i].FindControl("lblProductID")).Text.ToString());
                    HiddenField hdnProductid = (HiddenField)grdProducts.Rows[i].FindControl("hdnVendorSKUID");
                    String SKUs = Convert.ToString(((Label)grdProducts.Rows[i].FindControl("lblSKU")).Text.ToString());
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdProducts.Rows[i].FindControl("chkSelect")).Checked);
                    TextBox txtQty = (TextBox)grdProducts.Rows[i].FindControl("txtQty");
                    if (chkSelect && !string.IsNullOrEmpty(SKUs.ToString()))
                    {
                        AddtoList(SKUs, txtQty.Text.ToString());
                        Cnt++;
                    }
                }
                if (Cnt > 0)
                {
                    string skus = ViewState["SelectedSKUs"].ToString();
                    if (skus.Length > 1)
                        skus = skus.TrimEnd(",".ToCharArray());

                    string Qtys = ViewState["SelectedQty"].ToString();
                    if (Qtys.Length > 1)
                        Qtys = Qtys.TrimEnd(",".ToCharArray());

                    ScriptManager.RegisterClientScriptBlock(ibtnProductaddtoselectionlist, ibtnProductaddtoselectionlist.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["mode"].ToString() + "').value = '" + skus + "';window.opener.document.getElementById('" + Request.QueryString["qtymode"].ToString() + "').value = '" + Qtys + "';window.opener.document.getElementById('ContentPlaceHolder1_btnvendorlist').click();window.close();", true);
                }
            }
        }

        private void BindFeature()
        {
            if (Request.QueryString["StoreID"] != null)
            {
                BindProduct();
            }
        }

        private void BindProduct()
        {
            DataSet dsProduct = null;
            VendorComponent objVendor = new VendorComponent();
            //By Store ID
            if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() == "" && txtFeaturesystem.Text.Trim().Length <= 0)
            {
                dsProduct = objVendor.GetProductList(Convert.ToInt32(Request.QueryString["StoreID"].ToString()));
            }
            else if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() != "" && txtFeaturesystem.Text.Trim().Length > 0)
            {

                dsProduct = CommonComponent.GetCommonDataSet("SELECT ProductID,Name,SKU,'1' AS Quantity, Inventory FROM tb_Product WHERE (Name LIKE '%" + txtFeaturesystem.Text.Trim().Replace("'", "''") + "%' OR SKU LIKE '%" + txtFeaturesystem.Text.Trim().Replace("'", "''") + "%') AND isnull(Deleted,0)=0 AND isnull(Active,0)=1 AND Storeid =" + Request.QueryString["StoreID"].ToString() + "");

                //dsProduct = objVendor.GetProductListSearched("%" + txtFeaturesystem.Text.Trim() + "%", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));
            }

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdProducts.DataSource = dsProduct;
                grdProducts.DataBind();
                trAddProducts1.Visible = true;
            }
            else
            {
                grdProducts.DataSource = null;
                grdProducts.DataBind();
            }
        }
        protected void grdProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProducts.PageIndex = e.NewPageIndex;
            BindProduct();
        }
        protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                txtQty.Attributes.Add("onkeyup", "AddInventoryproduct(" + e.Row.RowIndex.ToString() + ");");

            }
        }

        private void AddtoList(string lblSKU, string txtQty)
        {
            try
            {
                string list = ",";
                if (ViewState["SelectedSKUs"] != null)
                {
                    list = ViewState["SelectedSKUs"].ToString();
                    if (!string.IsNullOrEmpty(lblSKU) && !list.Contains("," + lblSKU + ","))
                    {
                        list += lblSKU + ",";
                    }
                }
                else ViewState["SelectedSKUs"] = "";

                ViewState["SelectedSKUs"] = list;

                string listQty = ",";
                if (ViewState["SelectedQty"] != null)
                {
                    listQty = ViewState["SelectedQty"].ToString();
                    if (!string.IsNullOrEmpty(txtQty) && !listQty.Contains("," + txtQty + ","))
                    {
                        listQty += txtQty + ",";
                    }
                }
                else ViewState["SelectedQty"] = "";

                ViewState["SelectedQty"] = listQty;

            }
            catch { }
        }
    }
}