using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderHaming : BasePage
    {
        DataSet dsHaming = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string OrderNumber = string.Empty;
                if (Request.QueryString["ONo"] != null)
                {
                    OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                }
                imgupdatesku.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update.png";
                imgupdatesku1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-processing-order.png";
                BindData(OrderNumber);
                imgupdatesku1.OnClientClick = "OpenCenterWindow('OrderhamingPrint.aspx?Ono=" + OrderNumber.ToString() + "',680,700);return false;";
            }
        }

        protected void BindData(string OrderNumber)
        {
            try
            {

                if (Session["StrtempOrderedCustomCartID"] != null)
                {
                    if (Session["StrtempOrderedCustomCartID"].ToString().IndexOf(",") > -1)
                    {
                        Session["StrtempOrderedCustomCartID"] = Session["StrtempOrderedCustomCartID"] + "0";
                    }
                    DataSet dsproducts = new DataSet();
                    dsproducts = CommonComponent.GetCommonDataSet("Select * from tb_OrderedShoppingCartItems Where OrderedCustomCartID in (" + Session["StrtempOrderedCustomCartID"].ToString() + ") and OrderedShoppingCartID in (Select ShoppingCardID from tb_order Where OrderNumber=" + OrderNumber.ToString() + ")");
                    dsHaming = CommonComponent.GetCommonDataSet("Select * from tb_OrderHaming WHERE OrderNumber=" + SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()).ToString() + "");
                    if (dsproducts != null && dsproducts.Tables.Count > 0 && dsproducts.Tables[0].Rows.Count > 0)
                    {
                        grdProducts.DataSource = dsproducts;
                        grdProducts.DataBind();
                    }
                    else
                    {
                        grdProducts.DataSource = null;
                        grdProducts.DataBind();
                    }
                }
            }
            catch
            {
            }
        }

        protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlHamingName");
                TextBox txtHamingQty = (TextBox)e.Row.FindControl("txtHamingQty");
                Label lblcustomcartid = (Label)e.Row.FindControl("lblcustomcartid");
                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblName = (Label)e.Row.FindControl("lblName");
                string[] strnm = lblVariantNames.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] strval = lblVariantValues.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                txtHamingQty.Attributes.Add("onkeyup", "AddHamingqty(" + e.Row.RowIndex.ToString() + ");");

                if (strnm.Length > 0)
                {
                    for (int i = 0; i < strnm.Length; i++)
                    {
                        lblName.Text = lblName.Text.ToString() + "<br />" + strnm[i].ToString() + ":" + strval[i].ToString();
                    }
                }
                Int32 hmgQty = 0;
                if (Session["StrtempOrderedCustomCartID"] != null)
                {
                    string stt = "," + Session["StrtempOrderedCustomCartID"].ToString();
                    if (stt.IndexOf("," + lblcustomcartid.Text.ToString() + "") > -1)
                    {
                        ddl.Items.Insert(0, new ListItem("None", ""));
                        string[] strData = Session["vriantData"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] strQty = Session["vriantDataQty"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (strData.Length > 0)
                        {
                            //foreach (string stnm in strData)
                            for (int i = 0; i < strData.Length; i++)
                            {
                                hmgQty++;
                                ddl.Items.Insert(hmgQty, new ListItem(strData[i].ToString(), strData[i].ToString().Substring(0, strData[i].ToString().LastIndexOf("-"))));
                                txtHamingQty.Text = strQty[i].ToString();
                            }
                        }
                    }
                    if (dsHaming != null && dsHaming.Tables.Count > 0 && dsHaming.Tables[0].Rows.Count > 0)
                    {

                        DataRow[] dr = dsHaming.Tables[0].Select("OrderedCustomCartID = " + lblcustomcartid.Text.ToString() + "");
                        if (dr.Length > 0)
                        {
                            foreach (DataRow dr1 in dr)
                            {
                                txtHamingQty.Text = dr1["HamingQty"].ToString();
                                try
                                {
                                    ddl.SelectedValue = dr1["HamingName"].ToString();
                                }
                                catch
                                {

                                }
                            }
                            if (imgupdatesku1.Visible == false)
                            {
                                imgupdatesku1.Visible = true;
                            }
                        }
                    }
                    HiddenField hdnHamingQtyariantvalue = (HiddenField)e.Row.FindControl("hdnHamingQtyariantvalue");
                    hdnHamingQtyariantvalue.Value = txtHamingQty.Text.ToString();
                }
            }
        }

        protected void imgupdatesku_Click(object sender, ImageClickEventArgs e)
        {
            int cnt = 0;
            string OrderNumber = string.Empty;
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
            }
            foreach (GridViewRow gr in grdProducts.Rows)
            {
                DropDownList ddl = (DropDownList)gr.FindControl("ddlHamingName");
                TextBox txtHamingQty = (TextBox)gr.FindControl("txtHamingQty");
                Label lblcustomcartid = (Label)gr.FindControl("lblcustomcartid");
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                Label lblQty = (Label)gr.FindControl("lblQty");

                Label lblVariantValues = (Label)gr.FindControl("lblVariantValues");
                Label lblVariantNames = (Label)gr.FindControl("lblVariantNames");

                string StrVarintName = Convert.ToString(lblVariantNames.Text);
                string StrVariantValues = Convert.ToString(lblVariantValues.Text);

                DataSet dsvaraint = new DataSet();
                string[] StrVname = StrVarintName.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StrVvalue = StrVariantValues.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ddl.SelectedIndex > 0 && ddl.SelectedValue.ToString().ToLower() != "none" && !string.IsNullOrEmpty(txtHamingQty.Text) && Convert.ToInt32(txtHamingQty.Text.ToString()) > 0)
                {
                    cnt++;
                    //if (StrVname.Length > 0 && StrVvalue.Length > 0)
                    //{
                    //    for (int k = 0; k < StrVvalue.Length; k++)
                    //    {
                    //        if (!string.IsNullOrEmpty(StrVvalue[k].ToString()))
                    //        {
                    //            if (StrVname[k].ToString().ToLower().IndexOf("header design") > -1)
                    //            {
                    //                Int32 VariantValueID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantValue = '" + StrVvalue[k].ToString() + "' AND ProductId=" + lblProductID.Text.ToString() + ""));
                    //                dsvaraint = CommonComponent.GetCommonDataSet("SELECT VariantValueID,tb_ProductVariantValue.VariantValue,tb_ProductVariantValue.Inventory FROM tb_ProductVariantValue   WHERE tb_ProductVariantValue.variantid in (SELECT variantid FROM tb_ProductVariant WHERE ParentId=" + VariantValueID + ")");

                    //            }
                    //        }
                    //    }
                    //}

                    CommonComponent.ExecuteCommonData("EXEC usp_Orderhaming 0," + SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()).ToString() + "," + lblcustomcartid.Text.ToString() + "," + lblProductID.Text.ToString() + "," + lblQty.Text.ToString() + "," + txtHamingQty.Text.ToString() + ",'" + ddl.SelectedValue.ToString() + "'," + Session["AdminID"].ToString() + "");
                    if (imgupdatesku1.Visible == false)
                    {
                        imgupdatesku1.Visible = true;
                    }
                    // Update Variant Inventory and WH inv

                    try
                    {
                        //if (Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) from tb_OrderHaming where ISNULL(UpdatedBy,'')='' and ISNULL(Updatedon,'')='' and ordernumber=" + OrderNumber + " and OrderedCustomCartID=" + lblcustomcartid.Text.ToString() + "")) > 0)
                        //{
                        //DataRow[] dr = dsvaraint.Tables[0].Select("VariantValue like '%" + ddl.SelectedValue.ToString() + "L%'");
                        //if (dr.Length > 0)
                        //{
                        //CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set Inventory=Inventory-" + txtHamingQty.Text.ToString() + " Where VariantValueID =" + dr[0]["VariantValueID"].ToString() + " and VariantID in (Select VariantID fROM tb_ProductVariant Where ProductID=" + lblProductID.Text.ToString() + ")");

                        CommonComponent.ExecuteCommonData("Update tb_Product set Inventory=Inventory-" + txtHamingQty.Text.ToString() + " WHERE isnull(UPC,'') <> '' AND isnull(UPC,'') IN (SELECT isnull(UPC,'') FROM tb_product WHERE SKU='" + ddl.SelectedValue.ToString() + "')");
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set Inventory=Inventory-" + txtHamingQty.Text.ToString() + " Where SKU='" + ddl.SelectedValue.ToString().Replace("'", "''") + "' ");
                        DataSet dswarehouseproduct = new DataSet();
                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=Inventory-" + txtHamingQty.Text.ToString() + " WHERE VariantValueID in (SELECT VariantValueID FROM tb_ProductVariantValue WHERE SKU='" + ddl.SelectedValue.ToString().Replace("'", "''") + "')");

                        dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(Inventory,0)) as inventory,WareHouseID,ProductID FROM tb_WareHouseProductVariantInventory WHERE ProductID in (SELECT ProductID FROM tb_ProductVariantValue WHERE SKU='" + ddl.SelectedValue.ToString().Replace("'", "''") + "') GROUP BY WareHouseID,ProductID");
                        Int32 TotalInv = 0;
                        if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                        {
                            for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                            {
                                TotalInv += Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());
                                CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + " AND ProductID =" + dswarehouseproduct.Tables[0].Rows[m]["ProductID"].ToString() + "");
                            }
                            CommonComponent.ExecuteCommonData("Update tb_Product set Inventory=" + TotalInv.ToString() + " WHERE ProductID in (SELECT ProductID FROM tb_ProductVariantValue WHERE SKU='" + ddl.SelectedValue.ToString().Replace("'", "''") + "')");
                        }
                        //Int32 Hamingqty = 0;
                        //DataSet dsWarehouse = CommonComponent.GetCommonDataSet("Select * from tb_WareHouseProductInventory Where ProductID in (SELECT ProductID FROM tb_ProductVariantValue WHERE VariantValue='" + ddl.SelectedValue.ToString().Replace("'", "''") + "')");
                        //Int32.TryParse(txtHamingQty.Text.ToString(), out Hamingqty);
                        //if (Hamingqty > 0 && dsWarehouse != null && dsWarehouse.Tables.Count > 0 && dsWarehouse.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int k = 0; k < dsWarehouse.Tables[0].Rows.Count; k++)
                        //    {
                        //        Int32 WHQty = Convert.ToInt32(dsWarehouse.Tables[0].Rows[k]["Inventory"].ToString());
                        //        Int32 ProductInventoryID = Convert.ToInt32(dsWarehouse.Tables[0].Rows[k]["ProductInventoryID"].ToString());
                        //        if (WHQty > 0 && Hamingqty > 0)
                        //        {
                        //            Int32 TempHamingqty = WHQty - Hamingqty;
                        //            if (TempHamingqty == 0 || TempHamingqty > 0)
                        //            {
                        //                Hamingqty = 0;
                        //            }
                        //            if (TempHamingqty < 0)
                        //            {
                        //                Hamingqty = 0 - TempHamingqty;
                        //                TempHamingqty = 0;
                        //            }

                        //            CommonComponent.ExecuteCommonData("Update tb_WareHouseProductInventory set Inventory=" + TempHamingqty + " Where ProductInventoryID=" + ProductInventoryID + " and ProductID = " + lblProductID.Text.ToString() + "");
                        //        }
                        //    }
                        //}
                        // Open below comment
                        //CommonComponent.ExecuteCommonData("Update tb_Product set Inventory=Inventory-" + txtHamingQty.Text.ToString() + " Where ProductID=" + lblProductID.Text.ToString() + "");
                        // Use VariantValueID =" + dr[0]["VariantValueID"].ToString() + " for Inv Upate in tb_WareHouseProductVariantInventory table
                        //}
                        //}
                        BindData(OrderNumber);
                    }
                    catch { }
                }
                if (cnt == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "altsmg", "alert('Please select [Haming Option] or enter valid [Quantity].');", true);
                    return;
                }
            }
        }

        protected void imgupdatesku1_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}