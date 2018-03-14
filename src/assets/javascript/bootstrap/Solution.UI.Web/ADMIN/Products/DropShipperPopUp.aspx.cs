using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class DropShipperPopUp : Solution.UI.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["mode"] != null)
                {


                    BindDropshipper();
                    BindFeature();
                    ViewState["SelectedSKUs"] = "";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "loadVendorsku();", true);
                }

            }
        }


        private void BindDropshipper()
        {
            VendorDAC objVendorDAC = new VendorDAC();
            DataSet dsdropshipsku = objVendorDAC.GetVendorList(1);
            if (dsdropshipsku != null && dsdropshipsku.Tables.Count > 0 && dsdropshipsku.Tables[0].Rows.Count > 0)
            {
                ddldropshippersku.DataSource = dsdropshipsku.Tables[0];
                ddldropshippersku.DataTextField = "Name";
                ddldropshippersku.DataValueField = "VendorID";
            }
            else
            {
                ddldropshippersku.DataSource = null;
            }
            ddldropshippersku.Items.Insert(0, new ListItem("Select Drop Shipper", "0"));
            ddldropshippersku.DataBind();
        }
        protected void ddldropshippersku_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (ddldropshippersku.SelectedIndex != 0)
            // {
            BindProduct();

            // }
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
            DataSet dsVendor = null;
            VendorComponent objVendor = new VendorComponent();
            //By Store ID

            if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() == "" && txtFeaturesystem.Text.Trim().Length <= 0 && ddldropshippersku.SelectedIndex != 0)
            {
                dsVendor = objVendor.GetDropShipperListbyvendor(Convert.ToInt32(ddldropshippersku.SelectedValue.ToString()));
            }

            else if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() == "" && txtFeaturesystem.Text.Trim().Length <= 0)
            {
                dsVendor = objVendor.GetDropShipperList();
            }
            else if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() != "" && txtFeaturesystem.Text.Trim().Length >= 0)
            {
                dsVendor = objVendor.GetDropShipperListSearched("%" + txtFeaturesystem.Text.Trim() + "%");
            }



            if (dsVendor != null && dsVendor.Tables.Count > 0 && dsVendor.Tables[0].Rows.Count > 0)
            {
                grdProducts.DataSource = dsVendor;
                grdProducts.DataBind();
            }
            else
            {
                grdProducts.DataSource = null;
                grdProducts.DataBind();
            }


        }

        protected void ibtnFeaturesystemsearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFeaturesystem.Text != "")
                BindProduct();
        }
        protected void ibtnfeaturesystemshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtFeaturesystem.Text = "";
            BindProduct();
        }

        protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");


                if (txtvendorsku != null && txtvendorsku.Text != "")
                {
                    string[] strsplit = txtvendorsku.Text.ToString().Split(',');

                    if (strsplit.Length > 0)
                    {
                        for (int i = 0; i <= strsplit.Length - 1; i++)
                        {
                            if (lblSKU.Text.ToLower() == strsplit[i].ToString().ToLower())
                            {
                                chkSelect.Checked = true;
                            }
                        }

                    }

                }

                //   if (txtWarehouseInventory != null)
                //    InventoryTotal += int.TryParse(txtWarehouseInventory.Text.Trim().ToString(), out InventoryTotal) ? InventoryTotal : 0;

                //string[] str = "1,1";

            }

        }
        protected void ibtnFeaturesystemaddtoselectionlist_Click(object sender, ImageClickEventArgs e)
        {
            //int TotalRowCount = grdProducts.Rows.Count;
            //CheckBox chkSelect = null;
            //HiddenField hdnProductid = null;
            //for (int i = 0; i < TotalRowCount; i++)
            //{
            //    chkSelect = (CheckBox)grdProducts.Rows[i].FindControl("chkSelect");
            //    hdnProductid = (HiddenField)grdProducts.Rows[i].FindControl("hdnVendorSKUID");
            //    if (chkSelect.Checked == true)
            //    {
            //        ProductComponent.UpdateProductFeature(Convert.ToInt32(hdnProductid.Value.Trim()), true, Convert.ToInt32(Request.QueryString["StoreID"]));
            //    }
            //    else if (chkSelect.Checked == false)
            //    {
            //        ProductComponent.UpdateProductFeature(Convert.ToInt32(hdnProductid.Value.Trim()), false, Convert.ToInt32(Request.QueryString["StoreID"]));
            //    }
            //}
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);

            int Cnt = 0;
            if (grdProducts.Rows.Count > 0)
            {
                for (int i = 0; i < grdProducts.Rows.Count; i++)
                {
                    // int ProductId = Convert.ToInt32(((Label)grdProducts.Rows[i].FindControl("lblProductID")).Text.ToString());
                    HiddenField hdnProductid = (HiddenField)grdProducts.Rows[i].FindControl("hdnVendorSKUID");
                    String SKUs = Convert.ToString(((Label)grdProducts.Rows[i].FindControl("lblSKU")).Text.ToString());
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdProducts.Rows[i].FindControl("chkSelect")).Checked);

                    if (chkSelect && !string.IsNullOrEmpty(SKUs.ToString()))
                    {
                        AddtoList(SKUs);
                        Cnt++;
                    }
                }
                if (Cnt > 0)
                {
                    string skus = ViewState["SelectedSKUs"].ToString();
                    if (skus.Length > 1)
                        skus = skus.TrimEnd(",".ToCharArray());
                    //ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('ContentPlaceHolder1_hfSubTotal').value='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();window.close();", true);
                    ScriptManager.RegisterClientScriptBlock(ibtnFeaturesystemaddtoselectionlist, ibtnFeaturesystemaddtoselectionlist.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["mode"].ToString() + "').value = '" + skus + "';window.opener.document.getElementById('ContentPlaceHolder1_btnvendorlist').click();window.close();", true);
                    //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "<script>window.close();</script>", true);

                }
            }
        }
        protected void grdProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProducts.PageIndex = e.NewPageIndex;
            BindProduct();
        }
        private void AddtoList(string lblSKU)
        {
            try
            {
                string list = "";
                if (ViewState["SelectedSKUs"] != null)
                {
                    list = ViewState["SelectedSKUs"].ToString();
                    if (!string.IsNullOrEmpty(lblSKU) && !list.Contains(lblSKU + ","))
                    {
                        list += lblSKU + ",";
                    }
                }
                else ViewState["SelectedSKUs"] = "";

                ViewState["SelectedSKUs"] = list;
            }
            catch { }
        }

    }
}