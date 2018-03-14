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
    public partial class InventoryWareHouse : BasePage
    {
        Int32 InventoryTotal = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
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
            DataSet dsWarehouse = new DataSet();
            ProductComponent objProdComp = new ProductComponent();
            dsWarehouse = objProdComp.GetWarehouseList(Mode, ProductID);
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
            string Productdelivertype = "";

            DataSet ds = CommonComponent.GetCommonDataSet("Select Isnull(ProductTypeDeliveryID,0) as ProductTypeDeliveryID, Isnull(ProductTypeID,0) as ProductTypeID from tb_Product where ProductId=" + ProductID + "");
            string ptypeID = "";
            string pdeliverytypeID = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                pdeliverytypeID = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_producttypedelivery where ProductTypeDeliveryID=" + ds.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString() + ""));
                ptypeID = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductType where ProductTypeID=" + ds.Tables[0].Rows[0]["ProductTypeID"].ToString() + ""));
            }
            if (pdeliverytypeID != null && pdeliverytypeID.ToLower() == "vendor" && ptypeID != null && ptypeID.ToString().Trim().ToLower() == "assembly product")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Inventory Cannot be Updated due to Assembly Product.');", true);
            }
            else
            {

                ProductComponent objProductComp = new ProductComponent();
                int InventoryTotal = 0;
                foreach (GridViewRow row in grdWarehouse.Rows)
                {
                    int Inventory = 0;
                    Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
                    RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                    Label lblTotal = (Label)row.FindControl("lblTotal");
                    TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);
                    bool PreferredLocation = false;

                    if (rdowarehouse.Checked)
                    {
                        PreferredLocation = true;
                    }
                    if (txtInventory != null)
                    {
                        if (int.TryParse(txtInventory.Text.Trim(), out Inventory))
                        {
                            objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, Inventory, mode, PreferredLocation);
                            InventoryTotal += Inventory;
                        }
                        else
                        {
                            objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, PreferredLocation);
                        }
                    }
                    else
                    {
                        objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, PreferredLocation);
                    }

                }
                Int32 inventoryUpdated = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(Inventory,0) from tb_Product where ProductID=" + ProductID + ""));

                objProductComp.UpdateProductinventory(ProductID, InventoryTotal, Convert.ToInt32(Request.QueryString["StoreID"]));

                if (inventoryUpdated != InventoryTotal)
                    CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlert", "javascript:window.parent.jAlert('Inventory Updated Successfully.','Message','ContentPlaceHolder1_txtSearch'); window.parent.document.getElementById('ContentPlaceHolder1_grdProduct_lblInventory_" + Request.QueryString["invId"].ToString() + "').innerHTML='" + InventoryTotal.ToString() + "';window.parent.document.getElementById('ContentPlaceHolder1_txtSearch').value='';window.parent.disablePopup();$('#popup_ok').focus();", true);
            }
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