using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class InventoryVariantWareHouse : BasePage
    {
        Int32 InventoryTotal = 0;
        Int32 ProductID = 0;
        Int32 VariantID = 0;
        Int32 VariantValueID = 0;
        DataSet dsWarehouse = new DataSet();
        string strlockId = "";
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null)
            {
                ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);
            }
            if (Request.QueryString["VariantID"] != null)
            {
                VariantID = Convert.ToInt32(Request.QueryString["VariantID"]);
            }
            if (Request.QueryString["VariantValueID"] != null)
            {
                VariantValueID = Convert.ToInt32(Request.QueryString["VariantValueID"]);
            }


            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                popupviewdetailclose.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                GetWarehouseList(Convert.ToInt32(Request.QueryString["PID"].ToString()), 2);
            }
        }

        /// <summary>
        /// Gets the Warehouse List
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Mode">int Mode</param>
        private void GetWarehouseList(int ProductID, int Mode)
        {
            dsWarehouse = new DataSet();
            ProductComponent objProdComp = new ProductComponent();
            dsWarehouse = objProdComp.GetVariantWarehouseList(Mode, ProductID, VariantID, VariantValueID);
            if (dsWarehouse != null && dsWarehouse.Tables[0].Rows.Count > 0)
            {
                grdWarehouse.DataSource = dsWarehouse;
                grdWarehouse.DataBind();
            }
            else
            {
                grdWarehouse.DataSource = null;
                grdWarehouse.DataBind();
            }
        }

        /// <summary>
        /// WareHouse Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtWarehouseInventory = (TextBox)e.Row.FindControl("txtInventory");
                Label lblPreferredLocation = (Label)e.Row.FindControl("lblPreferredLocation");
                RadioButton rdowarehouse = (RadioButton)e.Row.FindControl("rdowarehouse");

                try
                {
                    if (lblPreferredLocation != null && !string.IsNullOrEmpty(lblPreferredLocation.Text.ToString().Trim()) && lblPreferredLocation.Text.ToString() == "1")
                    {
                        rdowarehouse.Checked = true;
                    }
                }
                catch { }

                Int32 tt = 0;
                if (e.Row.RowIndex == 0)
                {
                    txtWarehouseInventory.Focus();
                }
                if (txtWarehouseInventory != null)
                {
                    bool ff = Int32.TryParse(txtWarehouseInventory.Text.Trim().ToString(), out tt);
                    if (ff)
                    {
                        InventoryTotal += tt;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                TextBox txtLockQuantity = (TextBox)e.Row.FindControl("txtLockQuantity");

                if (dsWarehouse != null && dsWarehouse.Tables.Count > 1 && dsWarehouse.Tables[1].Rows.Count > 0)
                {
                    txtLockQuantity.Text = dsWarehouse.Tables[1].Rows[0]["AddiHemingQty"].ToString();
                    txtallowmsg.Text = dsWarehouse.Tables[1].Rows[0]["AllowQuantityAvail"].ToString();
                    txtlockmsg.Text = dsWarehouse.Tables[1].Rows[0]["LockQuantityAvail"].ToString();
                }
                else
                {
                    txtLockQuantity.Text = "0";
                }


                if (lblTotal != null)
                    lblTotal.Text = InventoryTotal.ToString();
            }
        }

        /// <summary>
        /// Saves the Inventory in Warehouse
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="mode">int mode</param>
        protected void SaveInventoryInWarehouse(int ProductID, int mode)
        {
            ProductComponent objProductComp = new ProductComponent();
            int InventoryTotal = 0;
            foreach (GridViewRow row in grdWarehouse.Rows)
            {
                int Inventory = 0;
                RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
                Label lblTotal = (Label)row.FindControl("lblTotal");
                TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);
                bool PreferredLocation = false;
                String WareHouseLocation = "";
                if (rdowarehouse.Checked)
                {
                    PreferredLocation = true;
                    try
                    {
                        WareHouseLocation = lblWarehouse.Text;

                        DataSet dsProductDetail = CommonComponent.GetCommonDataSet("Select SKU,UPC from tb_ProductVariantValue where VariantValueID=" + VariantValueID + "");
                        if (dsProductDetail != null && dsProductDetail.Tables.Count > 0 && dsProductDetail.Tables[0].Rows.Count > 0 && !String.IsNullOrEmpty(dsProductDetail.Tables[0].Rows[0]["SKU"].ToString()) && !String.IsNullOrEmpty(dsProductDetail.Tables[0].Rows[0]["UPC"].ToString()))
                        {
                            String WareHouseCode = Convert.ToString(CommonComponent.GetScalarCommonData("Select Code from tb_WareHouse where WareHouseID=" + Convert.ToInt32(WareHouseLocation.ToString()) + ""));
                            CommonComponent.ExecuteCommonData("exec usp_Warehouse_CheckWarehouse '" + dsProductDetail.Tables[0].Rows[0]["SKU"].ToString().Trim() + "','" + dsProductDetail.Tables[0].Rows[0]["UPC"].ToString().Trim() + "','" + WareHouseCode.ToString().Trim() + "'");
                        }
                    }
                    catch
                    {

                    }

                }
                if (txtInventory != null)
                {
                    if (int.TryParse(txtInventory.Text.Trim(), out Inventory))
                    {
                        objProductComp.InsertUpdateVariantWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, Inventory, mode, VariantID, VariantValueID, Convert.ToInt32(Session["AdminID"]), Convert.ToInt32(Session["AdminID"]), PreferredLocation);
                        InventoryTotal += Inventory;
                    }
                    else
                    {
                        objProductComp.InsertUpdateVariantWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, VariantID, VariantValueID, Convert.ToInt32(Session["AdminID"]), Convert.ToInt32(Session["AdminID"]), PreferredLocation);
                    }
                }
                else
                {
                    objProductComp.InsertUpdateVariantWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, VariantID, VariantValueID, Convert.ToInt32(Session["AdminID"]), Convert.ToInt32(Session["AdminID"]), PreferredLocation);
                }

            }

            string LockQuantity = Convert.ToString(((TextBox)grdWarehouse.FooterRow.FindControl("txtLockQuantity")).Text);
            if (LockQuantity == "")
            {
                LockQuantity = "0";
            }
            CommonComponent.ExecuteCommonData("UPDATE dbo.tb_ProductVariantValue SET AddiHemingQty=" + LockQuantity + ",AllowQuantityAvail='" + txtallowmsg.Text.ToString().Replace("'", "''") + "',LockQuantityAvail='" + txtlockmsg.Text.ToString().Replace("'", "''") + "' WHERE ProductID=" + ProductID + " AND VariantValueID=" + VariantValueID);

            //Int32 inventoryUpdated = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(Inventory,0) from tb_Product where ProductID=" + ProductID + ""));
            //// objProductComp.UpdateProductinventory(ProductID, InventoryTotal, Convert.ToInt32(Request.QueryString["StoreID"]));

            //CommonComponent.ExecuteCommonData("update tb_Product set Inventory=" + InventoryTotal + " where ProductID=" + ProductID + " and StoreID=" + Convert.ToInt32(Request.QueryString["StoreID"]));

            //if (inventoryUpdated != InventoryTotal)
            //    CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
            string strCid = Request.QueryString["cid"].ToString().Replace("lnkEditInventory", "hdnmainqty");
            string strIid = Request.QueryString["cid"].ToString().Replace("lnkEditInventory", "txtLockinventory");
            string strlockid = Request.QueryString["cid"].ToString().Replace("lnkEditInventory", "hdnAddiHemingQty");
            string strIid1 = Request.QueryString["cid"].ToString().Substring(0, Request.QueryString["cid"].ToString().IndexOf("_lnkEdit"));
            //strIid = strIid + "_txtLockinventory";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlert", "javascript:window.parent.jAlert('Inventory Updated Successfully.','Message');window.parent.document.getElementById('" + strlockid + "').value='" + LockQuantity.ToString() + "'; window.parent.document.getElementById('" + strCid + "').value='" + InventoryTotal.ToString() + "';window.parent.AllowandlockQtyVariant('" + strIid.ToString() + "');window.parent.document.getElementById('ContentPlaceHolder1_hdngridid').value='" + strIid1 + "';window.parent.document.getElementById('ContentPlaceHolder1_btnReBindData').click();window.parent.disablePopup();$('#popup_ok').focus();", true);
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            SaveInventoryInWarehouse(Convert.ToInt32(Request.QueryString["PID"].ToString()), 2);
        }
    }
}