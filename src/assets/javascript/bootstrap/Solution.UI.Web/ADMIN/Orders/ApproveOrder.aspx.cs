using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Text;
using Solution.Data;
using System.IO;
using System.Net;
using System.Xml;
using System.Net.Security;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ApproveOrder : BasePage
    {
        DataSet dsProducts = null;
        bool bApproved = false;
        Int32 IsOverStockProcess = 0;
        Int32 Storeid = 0;
        Boolean IsCouponDiscount = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgLogo.Src = AppLogic.AppConfigs("LIVE_SERVER").TrimEnd("/".ToCharArray()) + "/Client/images/logo_white_bg.gif";
                ImageButton3.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update-order.png";
                btnApprove.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/approve-order.png";
                imgprocessOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/processingorder.jpg";
                imgshortshiplineOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/shortshipline.jpg";
                imgupdatesku.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update.png";
                if (Request.QueryString["Ono"] != null)
                {
                    OrderComponent ObjOrder1 = new OrderComponent();
                    DataSet dsOrder = new DataSet();
                    dsOrder = ObjOrder1.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["ONO"].ToString()));
                    ViewState["Storeid"] = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["storeID"].ToString());
                    //  ImageButton3.Visible = false;
                    bool.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["IsApproved"]), out bApproved);
                    Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["IsOverStockProcess"]), out IsOverStockProcess);

                    if (ViewState["Storeid"] != null && ViewState["Storeid"].ToString() == "4" || ViewState["Storeid"].ToString() == "14")
                    {
                        //imgprocessOrder.Visible = true;
                        imgshortshiplineOrder.Visible = true;
                        imgprocessOrder.Visible = false;
                        //  imgshortshiplineOrder.Visible = false;
                    }
                    else
                    {
                        imgprocessOrder.Visible = false;
                        imgshortshiplineOrder.Visible = false;
                    }
                    if (IsOverStockProcess != 0)
                    {
                        imgprocessOrder.Visible = false;
                        // imgshortshiplineOrder.Visible = false;
                    }
                    if (bApproved)
                    {
                        BindProducts(Convert.ToInt32(Request.QueryString["Ono"]), true);
                        btnApprove.Visible = false;

                    }
                    else
                    {

                        BindProducts(Convert.ToInt32(Request.QueryString["Ono"]), false);
                    }
                    // form1.Visible = !bApproved;
                    Binddata(Request.QueryString["Ono"].ToString());
                    if (!bApproved)
                    {
                        lblAddProduct.Text = @"<a id='lnkAddNew' style='font-weight:bold'
                onclick='OpenUpdateProductBrowser(" + dsOrder.Tables[0].Rows[0]["storeID"] + "," + dsOrder.Tables[0].Rows[0]["ShoppingCardID"] + "," + Request.QueryString["Ono"].ToString() + ")' href='javascript:void(0);' ><img src='/App_Themes/" + Page.Theme.ToString() + "/images/add-item.png' alt='' /></a>";
                    }
                    else
                    {
                        lblAddProduct.Text = "";
                    }

                    string count = Convert.ToString(CommonComponent.GetScalarCommonData("select ordernumber from tb_order where isnull(isnavinserted,0)=1 and isnull(isnavcompleted,0)=0 and PaymentMethod='CREDITCARD' and ordernumber=" + Request.QueryString["Ono"].ToString() + ""));
                    if (!String.IsNullOrEmpty(count))
                    {
                        ImageButton3.OnClientClick = "if (confirm('Order is not imported into NAV, Are You sure want to change?')) { return true; } else { return false; }";
                    }


                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto", "window.parent.document.getElementById('ContentPlaceHolder1_frmUpdateOrder').height = document['body'].offsetHeight + 20;", true);
                }
            }
        }

        public String BindVariant(String VarinatNames, String VariantValues, Int32 productId, ref string SKU, ref string strRefSku)
        {
            string[] Names = VarinatNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] Values = VariantValues.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int iLoopValues = 0;
            StringBuilder Table = new StringBuilder();
            if (iLoopValues < Values.Length && Names.Length == Values.Length)
            {
                for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                {
                    Table.Append("<br/>" + Names[iLoopValues].ToString().Replace("Estimated Delivery", "Estimated Ship Date") + ": " + Values[iLoopValues]);
                    SQLAccess objSql = new SQLAccess();
                    DataSet dsoption = new DataSet();
                    dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + productId + " AND VariantValue='" + Values[iLoopValues].ToString().Replace("'", "''") + "'");
                    if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                        {
                            SKU += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                            strRefSku = dsoption.Tables[0].Rows[0]["SKU"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                        {

                            //String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                            //CreateFolder(FPath.ToString());
                            //if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //{
                            //    SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            //}
                            //else
                            //{
                            //    if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //    {
                            //        try
                            //        {
                            //            DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                            //            bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                            //            bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                            //            bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                            //            bCodeControl.BarCodeHeight = 70;
                            //            bCodeControl.ShowHeader = false;
                            //            bCodeControl.ShowFooter = true;
                            //            bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                            //            bCodeControl.Size = new System.Drawing.Size(250, 100);
                            //            bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                            //        }
                            //        catch
                            //        {

                            //        }
                            //        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //        {
                            //            SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            //        }
                            //    }
                            //}

                        }
                    }
                }
            }
            else
            {
                for (iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                {
                    Table.Append("<br/> - " + Values[iLoopValues]);
                    SQLAccess objSql = new SQLAccess();
                    DataSet dsoption = new DataSet();
                    dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + productId + " AND VariantValue='" + Values[iLoopValues].ToString().Replace("'", "''") + "'");
                    if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                        {
                            SKU += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                            strRefSku = dsoption.Tables[0].Rows[0]["SKU"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                        {

                            //String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                            //CreateFolder(FPath.ToString());
                            //if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //{
                            //    SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            //}
                            //else
                            //{
                            //    if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //    {
                            //        try
                            //        {
                            //            DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                            //            bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                            //            bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                            //            bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                            //            bCodeControl.BarCodeHeight = 70;
                            //            bCodeControl.ShowHeader = false;
                            //            bCodeControl.ShowFooter = true;
                            //            bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                            //            bCodeControl.Size = new System.Drawing.Size(250, 100);
                            //            bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                            //        }
                            //        catch
                            //        {

                            //        }
                            //        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            //        {
                            //            SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            //        }
                            //    }
                            //}

                        }
                    }

                }
            }
            return Table.ToString();


        }
        private void BindProducts(Int32 OrderNumber, bool Approveid)
        {
            LockProductComponent objLock = new LockProductComponent();
            dsProducts = new DataSet();
            if (Approveid == false)
            {
                dsProducts = objLock.GetOrderCartNew(OrderNumber);
            }
            else
            {
                dsProducts = objLock.GetOrderCartNewafterApprove(OrderNumber);
            }


            for (int k = 0; k < dsProducts.Tables[0].Rows.Count; k++)
            {
                decimal CouponDiscount = 0;
                if (!string.IsNullOrEmpty(dsProducts.Tables[0].Rows[k]["DiscountPrice"].ToString()))
                {
                    decimal.TryParse(dsProducts.Tables[0].Rows[k]["DiscountPrice"].ToString(), out CouponDiscount);
                    if (CouponDiscount > Decimal.Zero)
                    {
                        IsCouponDiscount = true;
                        break;
                    }
                }
            }

            gvProducts.DataSource = dsProducts;
            gvProducts.DataBind();
            //if (dsProducts.Tables[0].Rows.Count > 0)
            //{
            //    decimal orgtotalp = 0;
            //    for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
            //    {
            //        orgtotalp += Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["Price"].ToString()) * Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["Ordered Quantity"].ToString());
            //    }
            //    ViewState["OrgTotal"] = orgtotalp.ToString("f2");
            //}
            //else
            //{
            //    ViewState["OrgTotal"] = 0;
            //}
        }

        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["Storeid"] != null)
                {
                    Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
                }
                if (Storeid == 14 || Storeid == 4)
                {
                }
                else
                {
                    e.Row.Cells[12].Visible = false;
                }
                if (IsCouponDiscount == false)
                {
                    e.Row.Cells[11].Visible = false;
                }
                e.Row.Cells[5].Attributes.Add("style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Attributes.Add("style", "display:none");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblInventory = (Label)e.Row.FindControl("lblInventory");
                Label lblUpgradeSKU = (Label)e.Row.FindControl("lblUpgradeSKU");
                Label lblCustomCartID = (Label)e.Row.FindControl("lblCustomCartID");
                Label lblAssambly = (Label)e.Row.FindControl("lblAssambly");
                Label lblVariantname = (Label)e.Row.FindControl("lblVariantname");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblName = (Label)e.Row.FindControl("lblName");
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                Label lblaccepted = (Label)e.Row.FindControl("lblaccepted");
                Label OrderItemID = (Label)e.Row.FindControl("OrderItemID");
                DropDownList ddlacknowledgement = (DropDownList)e.Row.FindControl("ddlacknowledgement");
                DropDownList ddlupgradesku = (DropDownList)e.Row.FindControl("ddlupgradesku");
                Label lblSKUupgrade = (Label)e.Row.FindControl("lblSKUupgrade");
                ImageButton btnEditsku = (ImageButton)e.Row.FindControl("btnEditsku");
                Label lblOrderQty = (Label)e.Row.FindControl("lblOrderQty");
                Label lblShippedQty = (Label)e.Row.FindControl("lblShippedQty");
                ImageButton btnEditprice = (ImageButton)e.Row.FindControl("btnEditprice");
                ImageButton btnEditdiscountprice = (ImageButton)e.Row.FindControl("btnEditdiscountprice");
                Label lblRelatedproductID = (Label)e.Row.FindControl("lblRelatedproductID");
                Label lblIsProductType = (Label)e.Row.FindControl("lblIsProductType");
                Label lblSubtotalrow = (Label)e.Row.FindControl("lblSubtotalrow");
                Label lblUpgradeDiscountPrice = (Label)e.Row.FindControl("lblUpgradeDiscountPrice");
                Label lblUpgradePrice = (Label)e.Row.FindControl("lblUpgradePrice");

                Decimal dprice = Decimal.Zero;
                Decimal pUpgradePrice = Decimal.Zero;
                try
                {


                    if (lblUpgradeDiscountPrice != null && !string.IsNullOrEmpty(lblUpgradeDiscountPrice.Text))
                    {
                        dprice = Convert.ToDecimal(lblUpgradeDiscountPrice.Text);
                        pUpgradePrice = Convert.ToDecimal(lblUpgradePrice.Text);
                        if (dprice > Decimal.Zero)
                        {
                            dprice = dprice * Convert.ToDecimal(lblOrderQty.Text);
                            lblSubtotalrow.Text = String.Format("{0:0.00}", dprice);
                        }
                        else
                        {
                            pUpgradePrice = pUpgradePrice * Convert.ToDecimal(lblOrderQty.Text);
                            lblSubtotalrow.Text = String.Format("{0:0.00}", pUpgradePrice);
                        }
                    }
                    else
                    {
                        pUpgradePrice = Convert.ToDecimal(lblUpgradePrice.Text);
                        pUpgradePrice = pUpgradePrice * Convert.ToDecimal(lblOrderQty.Text);
                        lblSubtotalrow.Text = String.Format("{0:0.00}", pUpgradePrice);
                    }
                }
                catch
                {

                }

                if (ViewState["Storeid"] != null)
                {
                    Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
                }

                lblShippedQty.Text = "0";
                if (ViewState["Storeid"] != null)
                {
                    Int32 OrderQty = 0;
                    Int32.TryParse(lblOrderQty.Text.ToString(), out OrderQty);

                    string StrInv = Convert.ToString(CommonComponent.GetScalarCommonData("Select Isnull(inventory,0) as inventory from tb_Product Where Productid=" + lblProductID.Text.ToString() + ""));
                    if (!string.IsNullOrEmpty(StrInv))
                    {
                        Int32 Inventory = 0;
                        Int32.TryParse(StrInv.ToString(), out Inventory);
                        if (Inventory > 0)
                        {
                            if (Inventory >= OrderQty)
                            {
                                lblShippedQty.Text = OrderQty.ToString();
                            }
                            else
                            {
                                lblShippedQty.Text = Inventory.ToString();
                            }
                        }
                        else { lblShippedQty.Text = "0"; }
                    }
                    else
                    {
                        string StrProVariInv = Convert.ToString(CommonComponent.GetScalarCommonData("Select Isnull(Inventory,0) as Inventory from tb_ProductVariantValue Where sku='" + lblSKU.Text.Trim().Replace("'", "''") + "' and Productid=" + lblProductID.Text.ToString() + ""));
                        if (!string.IsNullOrEmpty(StrProVariInv))
                        {
                            Int32 Inventory = 0;
                            Int32.TryParse(StrProVariInv.ToString(), out Inventory);
                            if (Inventory > 0)
                            {
                                if (Inventory >= OrderQty)
                                {
                                    lblShippedQty.Text = OrderQty.ToString();
                                }
                                else
                                {
                                    lblShippedQty.Text = Inventory.ToString();
                                }
                            }
                            else { lblShippedQty.Text = "0"; }
                        }
                    }

                    int qty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(ShippedQty,0) as ShippedQty from tb_orderedshoppingcartitems where OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + " and RefProductID=" + lblProductID.Text.ToString() + ""));
                    if (qty > 0)
                    {
                        lblShippedQty.Text = qty.ToString();
                    }

                }

                if (Storeid == 14 || Storeid == 4)
                {
                    if (lblaccepted != null && !string.IsNullOrEmpty(lblaccepted.Text.ToString().Trim()) && (lblaccepted.Text.ToString().Trim() == "1" || lblaccepted.Text.ToString().Trim().ToLower() == "true"))
                    {
                        ddlacknowledgement.SelectedValue = "1";
                    }
                    else
                    {
                        ddlacknowledgement.SelectedValue = "0";
                    }
                }
                else
                {
                    e.Row.Cells[12].Visible = false;
                }

                if (IsCouponDiscount == false)
                {
                    e.Row.Cells[11].Visible = false;
                    if (lblIsProductType.Text == "0")
                    {


                        btnEditprice.Visible = false;
                    }
                    else
                    {
                        if (lblRelatedproductID.Text == "0")
                        {
                            btnEditprice.Visible = true;
                        }
                        else
                        {
                            btnEditprice.Visible = false;
                        }
                    }


                }
                else
                {
                    if (lblIsProductType.Text == "0")
                    {
                        btnEditdiscountprice.Visible = false;
                        btnEditprice.Visible = false;
                    }
                    else
                    {
                        if (lblRelatedproductID.Text == "0")
                        {
                            btnEditdiscountprice.Visible = true;
                        }
                        else
                        {
                            btnEditdiscountprice.Visible = false;
                        }
                        btnEditprice.Visible = false;
                    }

                }



                if (lblProductID != null)
                {


                    //string StrQuery = " SElect ProductAssemblyID,RefProductID, tb_ProductAssembly.ProductID,tb_product.name,tb_product.Sku,ISNULL(Quantity,0) as Quantity from tb_ProductAssembly " +
                    //                  " inner join tb_product on tb_ProductAssembly.ProductID=tb_product.ProductID " +
                    //                  " where RefProductID= " + lblProductID.Text.ToString() + " and ISNULL(tb_product.Active,1)=1 and ISNULL(Deleted,0)=0";
                    //DataSet dsAssamble = new DataSet();
                    //dsAssamble = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                    //if (dsAssamble != null && dsAssamble.Tables.Count > 0 && dsAssamble.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < dsAssamble.Tables[0].Rows.Count; i++)
                    //    {
                    //        lblAssambly.Text += dsAssamble.Tables[0].Rows[i]["Name"].ToString() + " - Qty (" + dsAssamble.Tables[0].Rows[i]["Quantity"].ToString() + ")<br />";
                    //    }
                    //}
                }

                if (Convert.ToInt32(dsProducts.Tables[0].Rows[e.Row.RowIndex]["StoreID"].ToString()) != 1)
                {
                    if (dsProducts.Tables[0].Rows[e.Row.RowIndex]["UPC"].ToString() == "")
                    {
                        if (!bApproved)
                        {
                            lblInventory.Text = @"
                <a onclick='javascript:OpenInventory(" + dsProducts.Tables[0].Rows[e.Row.RowIndex]["ProductID"].ToString() + ")' href='javascript:void(0);' >[Not Found]</a>";
                        }
                    }
                }
                if (dsProducts.Tables[0].Rows[e.Row.RowIndex]["MarryProducts"].ToString() == "")
                {
                    if (!bApproved)
                    {
                        lblUpgradeSKU.Text = @"
                <a onclick='javascript:OpenInventoryForSKU(" + dsProducts.Tables[0].Rows[e.Row.RowIndex]["ProductID"].ToString() + "," + dsProducts.Tables[0].Rows[e.Row.RowIndex]["StoreID"].ToString() + "," + lblCustomCartID.Text.ToString() + ")' href='javascript:void(0);' >[Not Found]</a>";
                    }
                }
                string strRef = "";
                string strRefSku = "";
                lblName.Text = lblName.Text.ToString() + BindVariant(lblVariantname.Text.ToString(), lblVariantValues.Text.ToString(), Convert.ToInt32(lblProductID.Text.ToString()), ref strRef, ref strRefSku);
                if (strRef != "")
                {
                    lblSKU.Text = strRef;
                }
                if (lblProductID != null)
                {
                    string[] strnm = lblVariantname.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strval = lblVariantValues.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (strnm.Length > 0)
                    {
                        for (int i = 0; i < strnm.Length; i++)
                        {
                            if (strnm[i].ToString().ToLower().IndexOf("select size") > -1 || strnm[i].ToString().ToLower().IndexOf("header design") > -1)
                            {
                                DataSet ds = new DataSet();
                                if (strRefSku != "")
                                {

                                }
                                else
                                {
                                    strRefSku = lblSKU.Text.ToString();
                                }
                                string strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ProductId,'0') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=1 AND SKU='" + strRefSku.ToString() + "'"));
                                if (!string.IsNullOrEmpty(strp))
                                {
                                    ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM tb_ProductVariantValue WHERE VariantID in (SELECT VariantID FROM tb_ProductVariant WHERE ParentId in (SELECT VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue = '" + strval[i].ToString() + "' AND ProductId=" + strp + ")) AND isnull(SKU,'')<> ''");
                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        ddlupgradesku.DataSource = ds.Tables[0];
                                        ddlupgradesku.DataTextField = "SKU";
                                        ddlupgradesku.DataValueField = "SKU";
                                        ddlupgradesku.DataBind();
                                    }
                                    else
                                    {
                                        strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + strRefSku.ToString() + "'"));
                                        if (!string.IsNullOrEmpty(strp))
                                        {

                                            strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                ddlupgradesku.DataSource = ds.Tables[0];
                                                ddlupgradesku.DataTextField = "SKU";
                                                ddlupgradesku.DataValueField = "SKU";
                                                ddlupgradesku.DataBind();
                                            }
                                        }
                                        else
                                        {
                                            string strsku1 = "";

                                            if (strRefSku.IndexOf("-84") > -1)
                                            {
                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                                            }
                                            else if (strRefSku.IndexOf("-96") > -1)
                                            {
                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                                            }
                                            else if (strRefSku.IndexOf("-108") > -1)
                                            {
                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                                                //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                                            }
                                            ds = new DataSet();

                                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                ddlupgradesku.DataSource = ds.Tables[0];
                                                ddlupgradesku.DataTextField = "SKU";
                                                ddlupgradesku.DataValueField = "SKU";
                                                ddlupgradesku.DataBind();
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + strRefSku.ToString() + "'"));
                                    if (!string.IsNullOrEmpty(strp))
                                    {
                                        ds = new DataSet();
                                        strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                                        ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            ddlupgradesku.DataSource = ds.Tables[0];
                                            ddlupgradesku.DataTextField = "SKU";
                                            ddlupgradesku.DataValueField = "SKU";
                                            ddlupgradesku.DataBind();
                                        }
                                    }
                                    else
                                    {
                                        string strsku1 = "";

                                        if (strRefSku.IndexOf("-84") > -1)
                                        {
                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                                        }
                                        else if (strRefSku.IndexOf("-96") > -1)
                                        {
                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                                        }
                                        else if (strRefSku.IndexOf("-108") > -1)
                                        {
                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                                            //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                                        }
                                        ds = new DataSet();

                                        ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            ddlupgradesku.DataSource = ds.Tables[0];
                                            ddlupgradesku.DataTextField = "SKU";
                                            ddlupgradesku.DataValueField = "SKU";
                                            ddlupgradesku.DataBind();
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strRefSku = lblSKU.Text.ToString();
                        string strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + lblSKU.Text.ToString() + "'"));
                        if (!string.IsNullOrEmpty(strp))
                        {
                            DataSet ds = new DataSet();
                            strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                ddlupgradesku.DataSource = ds.Tables[0];
                                ddlupgradesku.DataTextField = "SKU";
                                ddlupgradesku.DataValueField = "SKU";
                                ddlupgradesku.DataBind();
                            }
                        }
                        else
                        {
                            string strsku1 = "";

                            if (strRefSku.IndexOf("-84") > -1)
                            {
                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                            }
                            else if (strRefSku.IndexOf("-96") > -1)
                            {
                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                            }
                            else if (strRefSku.IndexOf("-108") > -1)
                            {
                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                                //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                            }
                            DataSet ds = new DataSet();

                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                ddlupgradesku.DataSource = ds.Tables[0];
                                ddlupgradesku.DataTextField = "SKU";
                                ddlupgradesku.DataValueField = "SKU";
                                ddlupgradesku.DataBind();
                            }

                        }
                    }
                    ddlupgradesku.Items.Insert(0, new ListItem("None", ""));
                    if (lblSKUupgrade != null && lblSKUupgrade.Text.ToString().Trim() != "")
                    {
                        if (strRefSku != "")
                        {
                            lblSKU.Text = lblSKU.Text.Replace(strRefSku, lblSKUupgrade.Text.ToString().Trim());
                        }
                        try
                        {
                            ddlupgradesku.SelectedValue = lblSKUupgrade.Text.ToString();
                        }
                        catch
                        {
                        }
                        ddlupgradesku.Visible = true;
                        btnEditsku.Visible = false;
                    }
                    else
                    {
                        ddlupgradesku.Visible = false;
                        if (ddlupgradesku.Items.Count > 0)
                        {
                            ddlupgradesku.SelectedIndex = 0;
                        }
                        btnEditsku.Visible = true;
                    }
                }

            }
        }
        protected void imgupdatesku_Click(object sender, EventArgs e)
        {
            Int32 iccc = 0;
            foreach (GridViewRow gvr in gvProducts.Rows)
            {
                Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");
                DropDownList ddlupgradesku = (DropDownList)gvr.FindControl("ddlupgradesku");
                if (ddlupgradesku.Visible == true)
                {
                    iccc++;
                    CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET SKUupgrade='" + ddlupgradesku.SelectedValue.ToString() + "' WHERE OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + "");
                }

            }
            if (iccc > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsucces", "jAlert('Record Updated Successfully.','Message');", true);
            }
        }
        protected void btnApproveupdate_Click(object sender, EventArgs e)
        {




            decimal dAdjustmentAmount = decimal.Zero;
            SQLAccess objSql = new SQLAccess();
            decimal subtotalnew = 0;
            foreach (GridViewRow gvr in gvProducts.Rows)
            {
                Int32 LockQty = 0, iLockQty, iOrderQty;
                Label lblName = (Label)gvr.FindControl("lblName");
                Label lblUpgradeSKU = (Label)gvr.FindControl("lblUpgradeSKU");
                Label lblProductID = (Label)gvr.FindControl("lblProductID");
                TextBox txtLockQty = (TextBox)gvr.FindControl("txtLockQty");
                Label lblLockQty = (Label)gvr.FindControl("lblLockQty");
                Label lblOrderQty = (Label)gvr.FindControl("lblOrderQty");

                Int32.TryParse(txtLockQty.Text, out LockQty);
                Int32.TryParse(lblLockQty.Text, out iLockQty);
                Int32.TryParse(lblOrderQty.Text, out iOrderQty);

                Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");

                Int32 UpgradeQty = 0, iUpgradeQty;
                Decimal UpgradePrice = 0;
                Decimal Price = 0;
                Decimal UpgradeDiscountPrice = 0;
                TextBox txtUpgradeQty = (TextBox)gvr.FindControl("txtUpgradeQty");
                Label lblUpgradeQty = (Label)gvr.FindControl("lblUpgradeQty");
                Label lblPrice = (Label)gvr.FindControl("lblPrice");
                TextBox txtUpgradePrice = (TextBox)gvr.FindControl("txtUpgradePrice");
                Label lblUpgradeInventory = (Label)gvr.FindControl("lblUpgradeInventory");
                Label lblRelatedproductID = (Label)gvr.FindControl("lblRelatedproductID");


                Int32.TryParse(txtUpgradeQty.Text, out UpgradeQty);
                Decimal.TryParse(txtUpgradePrice.Text, out UpgradePrice);
                Decimal.TryParse(lblPrice.Text, out Price);
                Int32.TryParse(lblUpgradeInventory.Text, out iUpgradeQty);
                string PName = lblName.Text.Trim().Replace("\"", "").Replace("'", "");
                TextBox txtUpgradeDiscountPrice = (TextBox)gvr.FindControl("txtUpgradeDiscountPrice");
                Decimal.TryParse(txtUpgradeDiscountPrice.Text, out UpgradeDiscountPrice);
                Label lblUpgradeDiscountPrice = (Label)gvr.FindControl("lblUpgradeDiscountPrice");

                Int32 ProductID = Convert.ToInt32(lblProductID.Text.Trim());

                TextBox txtEditSku = (TextBox)gvr.FindControl("txtEditSku");
                if (txtUpgradePrice.Visible == true)
                {
                    CommonComponent.ExecuteCommonData("update tb_OrderedShoppingCartItems set Price=" + UpgradePrice + " where OrderedCustomCartID=" + lblCustomCartID.Text + "");
                    try
                    {
                        OrderComponent objOrderlog = new OrderComponent();
                        objOrderlog.InsertOrderlog(29, Convert.ToInt32(Request.QueryString["ONO"].ToString()), lblCustomCartID.Text.ToString(), Convert.ToInt32(Session["AdminID"].ToString()));
                    }
                    catch { }
                }
                else if (txtUpgradeDiscountPrice.Visible == true)
                {
                    CommonComponent.ExecuteCommonData("update tb_OrderedShoppingCartItems set DiscountPrice=" + UpgradeDiscountPrice + " where OrderedCustomCartID=" + lblCustomCartID.Text + "");
                    try
                    {
                        OrderComponent objOrderlog = new OrderComponent();
                        objOrderlog.InsertOrderlog(29, Convert.ToInt32(Request.QueryString["ONO"].ToString()), lblCustomCartID.Text.ToString(), Convert.ToInt32(Session["AdminID"].ToString()));
                    }
                    catch { }
                }

                if (txtEditSku.Visible == true && !String.IsNullOrEmpty(txtEditSku.Text))
                {
                    CommonComponent.ExecuteCommonData("update tb_OrderedShoppingCartItems set SKU='" + txtEditSku.Text.ToString().Trim().Replace("'", "''") + "' where OrderedCustomCartID=" + lblCustomCartID.Text + "");
                    try
                    {
                        OrderComponent objOrderlog = new OrderComponent();
                        objOrderlog.InsertOrderlog(30, Convert.ToInt32(Request.QueryString["ONO"].ToString()), lblCustomCartID.Text.ToString(), Convert.ToInt32(Session["AdminID"].ToString()));
                    }
                    catch { }
                }


            }
            Decimal SwatchPrice = 0;
            int count = 0;
            count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(*) from tb_OrderedShoppingCartItems where isnull(IsProductType,0)=0 and OrderedShoppingCartID in (select ShoppingCardID from tb_order where ordernumber=" + Request.QueryString["Ono"].ToString() + ")"));
            if (count > 0)
            {
                SwatchPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(OrderTotal,0)-isnull(OrderSubtotal,0)-isnull(OrderTax,0)-isnull(OrderShippingCosts,0) from tb_Order where OrderNumber=" + Request.QueryString["Ono"].ToString() + ""));
            }
            else
            {
                SwatchPrice = 0;
            }
            CommonComponent.ExecuteCommonData("Exec usp_Product_UpdateTotal " + Request.QueryString["Ono"].ToString() + "," + SwatchPrice + "");

            BindProducts(Convert.ToInt32(Request.QueryString["Ono"]), false);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "jAlert('Record Updated Successfully.','Message');window.parent.location.href=window.parent.location.href;", true);
            ImageButton3.Visible = false;


            //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", " window.opener.location.reload(true);", true);




        }
        public DataSet GetInvoiceProducts(Int32 OrderID)
        {
            SQLAccess dbAccess = new SQLAccess();
            string Query = "select osi.Refproductid as Productid,isnull(osi.name,mp.name) as Productname,mp.SKU,osi.VariantNames,osi.VariantValues,osi.Quantity as quantity," +
                " osi.Price as price,isnull(osi.MarryproductQuantity,0) as UpgradeProductQty,p.ProductID as UpgradeProductID," +
                //" isnull(round(case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end,2),0) as UpgradePrice," +
                " isnull(lp.UpgradePrice,0) as UpgradePrice, " +
                " (select 'PO-'+convert(nvarchar(10),poi.PONumber)+',' from tb_PurChaseOrderItems poi inner join tb_PurchaseOrder po on poi.PONumber=po.PONumber " +
                " and poi.Productid=osi.refProductID and po.OrderNumber=o.OrderNumber for xml path('')) as PONumber from tb_orderedshoppingcartitems osi  inner join tb_order o on o.shoppingcardid=osi.orderedshoppingcartid " +
                " left join tb_product mp on mp.productid=osi.Refproductid left join tb_lockproducts lp on lp.OrderNumber=o.OrderNumber and " +
                " lp.Productid=osi.RefProductid AND lp.upgradeQuantity >= 0 " +
                //" and  isnull(lp.isCompleted,0)=1 " +
                " left join tb_product p on p.SKU = osi.marryproducts and p.StoreID=o.StoreID " +
                " where o.ordernumber=" + OrderID;
            DataSet myDataSet = new DataSet();
            myDataSet = dbAccess.GetDs(Query);
            return myDataSet;
        }

        protected void Binddata(string ONo)
        {
            //OrderComponent objOrder = new OrderComponent();
            //DataSet dsOrder = new DataSet();
            //dsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(ONo));
            //int StoreID = 0;

            ////lnkApprove.Attributes.Add("onclick", "window.open('ApproveOrder.aspx?ONo=" + ONo + "','','width=700,height=700,scrollbars=1'); ");
            ////lnkApprove.HRef = "ApproveOrder.aspx?Invoice=1&ONo=" + ONo;
            ////if (dsOrder.Tables[0].Rows[0]["IsApproved"] != null)
            ////{
            ////    if (dsOrder.Tables[0].Rows[0]["IsApproved"].ToString().ToLower() == "true")
            ////        lnkApprove.Visible = false;
            ////    else lnkApprove.Visible = true;

            ////}
            ////else lnkApprove.Visible = true;

            //Decimal ReturnedStarck = 0, ReturnFee = 0;
            //Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["ReturnedItem"]), out ReturnedStarck);
            //Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["ReturnedFee"]), out ReturnFee);
            //if (ReturnedStarck > 0)
            //{
            //    lblReturnedStarck.Text = ReturnedStarck.ToString("f2");
            //    trReturnedStarck.Visible = true;
            //}
            //else
            //    trReturnedStarck.Visible = false;

            //if (ReturnFee > 0)
            //{
            //    lblReturnFee.Text = ReturnFee.ToString("f2");
            //    trReturnFee.Visible = true;
            //}
            //else
            //    trReturnFee.Visible = false;



            //Int32.TryParse(dsOrder.Tables[0].Rows[0]["StoreID"].ToString(), out StoreID);
            //AppConfig.StoreID = StoreID;



            //Decimal custDist = 0;
            ////Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["CustomDiscount"]), out custDist);
            //Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["CustomDiscount"]), out custDist);

            //Decimal custDistNew = 0;
            ////Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["CustomDiscount"]), out custDist);
            //// Decimal.TryParse(Convert.ToString(txtcustomdis.Text.ToString()), out custDistNew);

            //ViewState["LevelDiscountAmount"] = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
            //ViewState["CouponDiscountAmount"] = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString());
            //Decimal Discount = custDist + Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()), 2);
            ////Decimal Discount = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CustomDiscount"].ToString()) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()), 2);

            //lblAmount.Text = Discount.ToString("f2");
            //if (ViewState["customdiscount"] == null)
            //{
            //    ViewState["customdiscount"] = Discount.ToString("f2");
            //    txtcustomdis.Text = Discount.ToString("f2");
            //}

            //Decimal CustomDiscount = 0;
            //Decimal.TryParse(txtcustomdis.Text, out CustomDiscount);

            //if (ViewState["LevelDiscountAmount"] != null)
            //{
            //    CustomDiscount = Math.Round((CustomDiscount - Convert.ToDecimal(ViewState["LevelDiscountAmount"])), 2);
            //}
            //if (ViewState["CouponDiscountAmount"] != null)
            //{
            //    CustomDiscount = Math.Round((CustomDiscount - Convert.ToDecimal(ViewState["CouponDiscountAmount"])), 2);
            //}
            //// Discount = Discount + CustomDiscount;
            //Decimal GiftCardDiscount = Decimal.Zero;
            //Decimal.TryParse(dsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString(), out GiftCardDiscount);
            //if (GiftCardDiscount != Decimal.Zero)
            //{ lblGiftCardDiscount.Text = Math.Round(GiftCardDiscount, 2).ToString(); TrGiftCard.Visible = true; }

            //decimal OrderTax = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()), 2);
            //lblOrderTax.Text = OrderTax.ToString();
            //Decimal decAdjustments = Decimal.Zero;
            //Decimal.TryParse(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"].ToString(), out decAdjustments);
            //lblAdjustments.Text = decAdjustments.ToString("f2");
            //decimal total = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2);
            //lblTotal.Text = (total + decAdjustments).ToString("f2");

            ////AddCartItem(ONo);
            //decimal ShipCost = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()), 2);
            //lblShoppingCost.Text = ShipCost.ToString();
            ////if (ViewState["OrgTotal"] != null)
            ////{
            ////    lblOrgSubTotal.Text = ViewState["OrgTotal"].ToString();//dsOrder.Tables[0].Rows[0]["OrderSubTotal"].ToString();
            ////}
            ////else
            ////{
            //lblOrgSubTotal.Text = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderSubTotal"]).ToString("f2");
            ////}
            //if (Convert.ToDecimal(lblSubTotal.Text) > 0)
            //{
            //    decimal diff = Convert.ToDecimal(lblSubTotal.Text) - Convert.ToDecimal(lblOrgSubTotal.Text);
            //    lblAdjustments.Text = diff.ToString("f2");
            //}
            //decimal alltotal = Convert.ToDecimal(total) + Convert.ToDecimal(lblAdjustments.Text.ToString().Replace("$", ""));
            //lblTotal.Text = (alltotal - Convert.ToDecimal(txtcustomdis.Text)).ToString("f2");
            //ViewState["customdiscount"] = CustomDiscount.ToString("f2");

            //Decimal.TryParse(dsOrder.Tables[0].Rows[0]["OrderSubTotal"].ToString(), out decAdjustments);

        }
        protected void imgprocessOrder_Click(object sender, EventArgs e)
        {
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            if (ViewState["Storeid"] != null)
            {
                Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
            }
            string MerchantKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='MerchantKey' AND Storeid=" + Storeid.ToString() + ""));
            string AuthenticationKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthenticationKey' AND Storeid=" + Storeid.ToString() + ""));
            String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + Storeid.ToString() + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>" + MerchantKey + "</MerchantKey>");
            transactionCommand.Append("<AuthenticationKey>" + AuthenticationKey + "</AuthenticationKey>");
            transactionCommand.Append("<ProcessingOrders>");

            foreach (GridViewRow gr in gvProducts.Rows)
            {

                Label lblaccepted = (Label)gr.FindControl("lblaccepted");
                Label OrderItemID = (Label)gr.FindControl("OrderItemID");
                DropDownList ddlacknowledgement = (DropDownList)gr.FindControl("ddlacknowledgement");
                System.Web.UI.HtmlControls.HtmlInputHidden hdncustom = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdncustom");
                SQLAccess objSql = new SQLAccess();
                objSql.ExecuteNonQuery("UPDATE tb_OrderedShoppingCartItems SET IsAccepted=" + ddlacknowledgement.SelectedValue.ToString() + " WHERE OrderedCustomCartID =" + hdncustom.Value.ToString() + " UPDATE tb_Order SET IsOverStockProcess=1 WHERE OrderNumber=" + Request.QueryString["ONO"].ToString() + "");
                transactionCommand.Append("<InvoiceLineId acknowledgement=\"" + ddlacknowledgement.SelectedItem.Text.ToString().ToLower() + "\">" + OrderItemID.Text.ToString() + "</InvoiceLineId>");

            }
            transactionCommand.Append("</ProcessingOrders>");
            transactionCommand.Append("</Request>");

            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.Headers.Add("SapiMethodName", "ProcessingOrders");
            // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/OrdeProcessing-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Processing Order Successfully...','Message');", true);
                imgprocessOrder.Visible = false;
                imgshortshiplineOrder.Visible = false;
            }
            catch
            {
            }

        }
        protected void imgshortshiplineOrder_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = new
          RemoteCertificateValidationCallback
(
delegate { return true; }
);
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            if (ViewState["Storeid"] != null)
            {
                Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
            }
            string OverstockUserName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockUserName' AND Storeid=" + Storeid + ""));
            string OverstockPassword = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockPassword' AND Storeid=" + Storeid.ToString() + ""));
            // String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + Storeid.ToString() + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            transactionCommand.Append("<supplierShipmentMessage xmlns=\"api.supplieroasis.com\">");

            String strCheck = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(RefSalesChannel,RefOrderId) FROM tb_Order WHERE isnull(IsOverStockProcess,0) = 1 AND OrderNumber=" + Convert.ToInt32(Request.QueryString["Ono"].ToString()) + ""));
            if (!String.IsNullOrEmpty(strCheck))
            {
                foreach (GridViewRow gr in gvProducts.Rows)
                {
                    Label lblCustomCartID = (Label)gr.FindControl("lblCustomCartID");

                    string linenumber = "";
                    linenumber = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(LineNumber,0) from tb_OrderedShoppingCartItems where OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + ""));



                    transactionCommand.Append("<supplierShipment>");
                    transactionCommand.Append("<salesChannelName>OSTK</salesChannelName>");
                    Label lblaccepted = (Label)gr.FindControl("lblaccepted");
                    Label OrderItemID = (Label)gr.FindControl("OrderItemID");
                    DropDownList ddlacknowledgement = (DropDownList)gr.FindControl("ddlacknowledgement");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdncustom = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdncustom");
                    SQLAccess objSql = new SQLAccess();
                    objSql.ExecuteNonQuery("UPDATE tb_Order SET IsOverStockProcess=2 WHERE OrderNumber=" + Request.QueryString["ONO"].ToString() + "");
                    transactionCommand.Append("<salesChannelOrderNumber>" + strCheck.ToString() + "</salesChannelOrderNumber>");
                    transactionCommand.Append("<salesChannelLineNumber>" + linenumber + "</salesChannelLineNumber>");
                    transactionCommand.Append("<warehouse><code>Exclusive</code></warehouse>");
                    transactionCommand.Append("<shortShip><reasonCode><code>" + AppLogic.AppConfigs("OverStockReasonCode").ToString() + "</code></reasonCode></shortShip>");
                    transactionCommand.Append("</supplierShipment>");

                }
            }
            transactionCommand.Append("</supplierShipmentMessage>");

            //System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            //byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            //myRequest.Method = "POST";
            //myRequest.Timeout = 300000;
            //myRequest.Headers.Add("SapiMethodName", "ShortshipLine");
            //// myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            //myRequest.ContentType = "application/xml";
            //myRequest.ContentLength = data.Length;
            //Stream newStream = myRequest.GetRequestStream();
            //// Send the data.
            //newStream.Write(data, 0, data.Length);
            //newStream.Close();
            //// get the response
            //WebResponse myResponse;
            //String rawResponseString = String.Empty;

            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            //String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.supplieroasis.com/shipments");
            myRequest.Method = "POST";

            myRequest.Headers.Add("Authorization", string.Format("Basic {0}", GetAuthorization(OverstockUserName, OverstockPassword)));
            myRequest.ContentType = "application/xml; charset=UTF-8";

            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/OrdeProcessing-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Order Short ship Line Successfully...','Message');", true);
                imgprocessOrder.Visible = false;
                imgshortshiplineOrder.Visible = false;
            }
            catch
            {
            }

        }
        private static string GetAuthorization(string User, string Password)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);

            return Convert.ToBase64String(utf8encoder.GetBytes(string.Format("{0}:{1}", User, Password)));
        }
        private void UpdaeoverStockQuantity()
        {

        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {

            LockProductComponent objLock = new LockProductComponent();

            foreach (GridViewRow gvr in gvProducts.Rows)
            {
                Int32 LockQty = 0, iLockQty, iShippedqty;
                Label lblName = (Label)gvr.FindControl("lblName");

                Label lblOrderQty = (Label)gvr.FindControl("lblOrderQty");
                Label lblUpgradeSKU = (Label)gvr.FindControl("lblUpgradeSKU");
                Label lblProductID = (Label)gvr.FindControl("lblProductID");
                TextBox txtLockQty = (TextBox)gvr.FindControl("txtLockQty");
                Label lblLockQty = (Label)gvr.FindControl("lblLockQty");

                Label lblShippedQty = (Label)gvr.FindControl("lblShippedQty");

                //Int32.TryParse(lblShippedQty.Text, out iShippedqty);

                Int32.TryParse(txtLockQty.Text, out LockQty);
                Int32.TryParse(lblLockQty.Text, out iLockQty);

                Int32 UpgradeQty = 0, iUpgradeQty;
                Decimal UpgradePrice = 0;
                TextBox txtUpgradeQty = (TextBox)gvr.FindControl("txtUpgradeQty");
                TextBox txtUpgradePrice = (TextBox)gvr.FindControl("txtUpgradePrice");
                Label lblUpgradeInventory = (Label)gvr.FindControl("lblUpgradeInventory");
                Int32.TryParse(txtUpgradeQty.Text, out UpgradeQty);
                Decimal.TryParse(txtUpgradePrice.Text, out UpgradePrice);
                Int32.TryParse(lblUpgradeInventory.Text, out iUpgradeQty);
                string PName = lblName.Text.Trim().Replace("\"", "").Replace("'", "");


                if (LockQty > iLockQty)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter " + iLockQty + " or Less Lock Quantity for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    txtLockQty.Focus();
                    return;
                }

                else if (UpgradeQty > iUpgradeQty)
                {
                    if (iUpgradeQty == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter Zero Upgrade Quantity for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                        txtUpgradeQty.Text = "0";
                    }
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter " + iUpgradeQty + " or Less Upgrade Quantity for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);

                    txtUpgradeQty.Focus();
                    return;
                }

                else if (LockQty == Convert.ToInt32(lblOrderQty.Text.Trim()) && UpgradeQty > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter Zero as Upgrade Quantity for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    txtUpgradeQty.Focus();
                    return;
                }
                else if ((Convert.ToInt32(UpgradeQty) > (iLockQty - LockQty)) && UpgradeQty > 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid Upgrade Quantity for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    txtUpgradeQty.Focus();
                    return;
                }
                else if (UpgradeQty > 0 && UpgradePrice <= 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid Upgrade Price for " + PName + ".','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    txtUpgradePrice.Focus();
                    return;
                }
            }

            decimal dAdjustmentAmount = decimal.Zero;
            foreach (GridViewRow gvr in gvProducts.Rows)
            {
                Int32 LockQty = 0, iLockQty, iOrderQty;
                Label lblName = (Label)gvr.FindControl("lblName");
                Label lblUpgradeSKU = (Label)gvr.FindControl("lblUpgradeSKU");
                Label lblProductID = (Label)gvr.FindControl("lblProductID");
                TextBox txtLockQty = (TextBox)gvr.FindControl("txtLockQty");
                Label lblLockQty = (Label)gvr.FindControl("lblLockQty");
                Label lblOrderQty = (Label)gvr.FindControl("lblOrderQty");

                Int32.TryParse(txtLockQty.Text, out LockQty);
                Int32.TryParse(lblLockQty.Text, out iLockQty);
                Int32.TryParse(lblOrderQty.Text, out iOrderQty);

                Label lblCustomCartID = (Label)gvr.FindControl("lblCustomCartID");

                Int32 UpgradeQty = 0, iUpgradeQty;
                Decimal UpgradePrice = 0;
                Decimal Price = 0;

                TextBox txtUpgradeQty = (TextBox)gvr.FindControl("txtUpgradeQty");
                Label lblUpgradeQty = (Label)gvr.FindControl("lblUpgradeQty");
                Label lblPrice = (Label)gvr.FindControl("lblPrice");
                TextBox txtUpgradePrice = (TextBox)gvr.FindControl("txtUpgradePrice");
                Label lblUpgradeInventory = (Label)gvr.FindControl("lblUpgradeInventory");


                Int32.TryParse(txtUpgradeQty.Text, out UpgradeQty);
                Decimal.TryParse(txtUpgradePrice.Text, out UpgradePrice);
                Decimal.TryParse(lblPrice.Text, out Price);
                Int32.TryParse(lblUpgradeInventory.Text, out iUpgradeQty);
                string PName = lblName.Text.Trim().Replace("\"", "").Replace("'", "");


                Int32 ProductID = Convert.ToInt32(lblProductID.Text.Trim());
                int PInventory = objLock.GetInventoryforProduct(ProductID, AppConfig.StoreID);
                int UInventory = objLock.GetInventoryforProduct(lblUpgradeSKU.Text.Trim(), AppConfig.StoreID);
                if (PInventory < LockQty)
                {
                    BindProducts(Convert.ToInt32(Request.QueryString["Ono"]), false);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter " + PInventory + " or less Lock Quantity for " + PName + "','Message'); $('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    return;
                }
                else if (UInventory < UpgradeQty)
                {
                    BindProducts(Convert.ToInt32(Request.QueryString["Ono"]), false);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter " + UInventory + " or less Upgrade Quantity for " + PName + "','Message'); $('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');", true);
                    return;
                }

                if (LockQty != 0 || UpgradeQty != 0)
                {
                    objLock = new LockProductComponent();

                    Int32 UpgradeQuantity = 0;

                    string MarryProducts = "";
                    if (UpgradeQty > 0)
                    {
                        MarryProducts = lblUpgradeSKU.Text.Trim();
                        UpgradeQuantity = UpgradeQty;

                    }
                    else
                    {
                        MarryProducts = "";
                        UpgradeQuantity = 0;
                        UpgradePrice = 0;
                    }
                    Int32 MarkQuantity = 0;
                    //objLock.MarkQuantity = 0;
                    int result = objLock.AddLockProduct(ProductID, LockQty, Convert.ToInt32(Request.QueryString["Ono"]), MarryProducts, false, MarkQuantity, UpgradeQuantity, Convert.ToInt32(lblCustomCartID.Text.Trim()), UpgradePrice);
                    if (result != 0)
                    {
                        objLock.AddQuantity(ProductID, -(LockQty));
                        if (iUpgradeQty > 0)
                            objLock.AddQuantitywithSKU(lblUpgradeSKU.Text.Trim(), -(UpgradeQty), AppConfig.StoreID);
                    }
                    if (UpgradeQty > 0)
                        dAdjustmentAmount += (UpgradeQty * UpgradePrice) - (iOrderQty * Price);




                }
            }
            objLock.SetApproveOrder(Convert.ToInt32(Request.QueryString["Ono"]), dAdjustmentAmount, 0);


            SQLAccess objSql = new SQLAccess();
            objSql.ExecuteNonQuery("INSERT INTO tb_Timestamplog(Createdby,type,refnumber,orderNumber) VALUES (" + Session["AdminID"].ToString() + ",1," + Convert.ToInt32(Request.QueryString["Ono"]) + "," + Convert.ToInt32(Request.QueryString["Ono"]) + ")");

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Order Approved Successfully...','Message');$('html, body').animate({ scrollTop: $('#btnApprove').offset().top }, 'slow');window.parent.location.href=window.parent.location.href;", true);


        }
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {


            //lblSKU.Visible = false;
            //btnEditsku.Visible = false;
            //txtEditSku.Visible = true;
            //ddlupgradesku.Visible = false;


        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ImageButton3.Visible = true;
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                TextBox txtUpgradePrice = (TextBox)row.FindControl("txtUpgradePrice");
                Label lblUpgradePrice = (Label)row.FindControl("lblUpgradePrice");
                ImageButton btnEditprice = (ImageButton)row.FindControl("btnEditprice");
                ImageButton btnEditdiscountprice = (ImageButton)row.FindControl("btnEditdiscountprice");


                TextBox txtUpgradeDiscountPrice = (TextBox)row.FindControl("txtUpgradeDiscountPrice");
                Label lblUpgradeDiscountPrice = (Label)row.FindControl("lblUpgradeDiscountPrice");
                DropDownList ddlupgradesku = (DropDownList)row.FindControl("ddlupgradesku");
                TextBox txtEditSku = (TextBox)row.FindControl("txtEditSku");
                Label lblSKU = (Label)row.FindControl("lblSKU");
                ImageButton btnEditsku = (ImageButton)row.FindControl("btnEditsku");
                if (e.CommandName == "EditPrice")
                {
                    txtUpgradePrice.Visible = true;
                    lblUpgradePrice.Visible = false;
                    btnEditprice.Visible = false;
                }
                else if (e.CommandName == "EditDiscountPrice")
                {
                    txtUpgradeDiscountPrice.Visible = true;
                    lblUpgradeDiscountPrice.Visible = false;
                    btnEditdiscountprice.Visible = false;
                }
                else if (e.CommandName == "EditSKu")
                {


                    lblSKU.Visible = false;
                    btnEditsku.Visible = false;
                    txtEditSku.Visible = true;
                    ddlupgradesku.Visible = false;
                }
            }
        }
    }
}