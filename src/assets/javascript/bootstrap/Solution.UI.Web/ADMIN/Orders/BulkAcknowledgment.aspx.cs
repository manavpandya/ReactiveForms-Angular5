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
    public partial class BulkAcknowledgment : BasePage
    {
        DataSet dsProducts = null;
        bool bApproved = false;
        Int32 IsOverStockProcess = 0;
        Int32 Storeid = 4;
        Boolean IsCouponDiscount = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(-2)));
                txtToDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date));
                Fillorder();
                imgprocessOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/processing-order.gif";
                imgshortshiplineOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/short-ship-line.gif";
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnUploadOrder.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload-orders-to-nav.gif) no-repeat transparent; width: 150px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");
            }

        }
        private void Fillorder()
        {
            DataSet dsorder = new DataSet();
            string sttoreid = " AND StoreId=4 ";
            ViewState["Storeid"] = "4";
            Int32 ordernumber = 0;
            if (txtSearch.Text.ToString().Trim() != "")
            {
                Int32.TryParse(txtSearch.Text.ToString(), out ordernumber);
                if (ordernumber > 0)
                {
                    sttoreid += " ANd (orderNumber=" + ordernumber + " Or Reforderid='" + ordernumber + "')";
                }
                else
                {
                    sttoreid += " AND Reforderid='" + txtSearch.Text.ToString().Trim().Replace("'", "''") + "'";
                }
            }
            if (txtToDate.Text != "" && txtFromDate.Text != "")
            {
                sttoreid += " AND cast(Orderdate as date) >=cast('" + txtFromDate.Text.ToString().Trim().Replace("'", "''") + "' as date) and  cast(Orderdate as date) <=cast('" + txtToDate.Text.ToString().Trim().Replace("'", "''") + "' as date) ";
            }
            //dsorder = CommonComponent.GetCommonDataSet("SELECT   OrderNumber,Orderdate,isnull(Reforderid,'') as Reforderid FROM tb_Order WHERE isnull(IsBackEnd,1)=1 AND isnull(BackEndGUID,'') = '' AND isnull(deleted,0)=0 " + sttoreid + " Order By Orderdate DESC");
            dsorder = CommonComponent.GetCommonDataSet("SELECT   OrderNumber,Orderdate,isnull(Reforderid,'') as Reforderid FROM tb_Order WHERE isnull(OrderStatus,'') <> 'CANCELED' and isnull(isNAVInserted,0)=1 AND isnull(isnavcompleted,0) = 0 AND isnull(deleted,0)=0 " + sttoreid + " Order By Orderdate DESC");
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                grdAllorder.DataSource = dsorder;
                grdAllorder.DataBind();
                trsavevendor.Visible = true;
                trTop.Visible = true;
                trbottom.Visible = true;
            }
            else
            {
                grdAllorder.DataSource = null;
                grdAllorder.DataBind();
                trsavevendor.Visible = false;
                trTop.Visible = false;
                trbottom.Visible = false;
            }
        }
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
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
                    Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
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

                            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                            CreateFolder(FPath.ToString());
                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            {
                                SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            }
                            else
                            {
                                if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                {
                                    try
                                    {
                                        DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                        bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                        bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                        bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                        bCodeControl.BarCodeHeight = 70;
                                        bCodeControl.ShowHeader = false;
                                        bCodeControl.ShowFooter = true;
                                        bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                        bCodeControl.Size = new System.Drawing.Size(250, 100);
                                        bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                    }
                                    catch
                                    {

                                    }
                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                    }
                                }
                            }

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

                            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                            CreateFolder(FPath.ToString());
                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                            {
                                SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                            }
                            else
                            {
                                if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                {
                                    try
                                    {
                                        DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                        bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                        bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                        bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                        bCodeControl.BarCodeHeight = 70;
                                        bCodeControl.ShowHeader = false;
                                        bCodeControl.ShowFooter = true;
                                        bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                        bCodeControl.Size = new System.Drawing.Size(250, 100);
                                        bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                    }
                                    catch
                                    {

                                    }
                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        SKU += "<br/><img width=\"160px\" src=\"" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                    }
                                }
                            }

                        }
                    }

                }
            }
            return Table.ToString();


        }
        private void BindProducts(Int32 OrderNumber, bool Approveid, GridView gvProducts)
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
                Literal ltr2 = (Literal)e.Row.FindControl("ltr2");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnshoppingcartid");
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
                if (Storeid == 4)
                {
                    ltr2.Visible = true;
                    DataSet dsProduct = new DataSet();
                    OrderComponent objOrder = new OrderComponent();
                    dsProduct = objOrder.GetProductList(Convert.ToInt32(hdnshoppingcartid.Value));
                    if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsProduct.Tables[0].Rows.Count; i++)
                        {

                            DataSet dsTemp = new DataSet();
                            dsTemp = CommonComponent.GetCommonDataSet("select isnull(Inventory,0) as Inventory,isnull(sku,'') as sku,isnull(upc,'') as UPC,storeid from tb_product where productid=" + dsProduct.Tables[0].Rows[i]["RefProductID"].ToString() + "");
                            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                            {
                                String Inv = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsTemp.Tables[0].Rows[0]["UPC"].ToString() + "','" + dsTemp.Tables[0].Rows[0]["sku"].ToString() + "'," + dsTemp.Tables[0].Rows[0]["storeid"].ToString() + ");"));
                                ltr2.Text = "<span style=\"color:#2A7FFF;font-weight:bold;\"> [Available Qty : " + dsTemp.Tables[0].Rows[0]["Inventory"].ToString() + "][Sales Channel Qty : " + Inv.ToString() + "]</span>";
                            }
                        }
                    }
                }
                else
                {
                    ltr2.Visible = false;
                }
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
                        ddlupgradesku.Visible = false;//ddlupgradesku.Visible = true;
                        btnEditsku.Visible = false;
                    }
                    else
                    {
                        ddlupgradesku.Visible = false;
                        if (ddlupgradesku.Items.Count > 0)
                        {
                            ddlupgradesku.SelectedIndex = 0;
                        }
                        btnEditsku.Visible = false; //btnEditsku.Visible = true;
                    }
                }

            }
        }

        protected void grdAllorder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvProducts = (GridView)e.Row.FindControl("gvProducts");
                Label lblorderNumber = (Label)e.Row.FindControl("lblorderNumber");
                BindProducts(Convert.ToInt32(lblorderNumber.Text.ToString()), false, gvProducts);
            }
        }
        protected void imgprocessOrder_Click(object sender, EventArgs e)
        {
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            //if (ViewState["Storeid"] != null)
            //{
            //    Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
            //}
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

            bool flg = false;
            foreach (GridViewRow gr1 in grdAllorder.Rows)
            {
                GridView gvProducts = (GridView)gr1.FindControl("gvProducts");
                Label lblorderNumber = (Label)gr1.FindControl("lblorderNumber");
                CheckBox chkselect = (CheckBox)gr1.FindControl("chkselect");
                if (chkselect.Checked == true)
                {
                    SQLAccess objSql = new SQLAccess();
                    foreach (GridViewRow gr in gvProducts.Rows)
                    {
                        Int32 strCheck = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT count(OrderNumber) FROM tb_Order WHERE isnull(IsOverStockProcess,0) = 0 AND OrderNumber=" + lblorderNumber.Text.ToString() + ""));
                        if (strCheck > 0)
                        {
                            flg = true;
                            Label lblaccepted = (Label)gr.FindControl("lblaccepted");
                            Label OrderItemID = (Label)gr.FindControl("OrderItemID");
                            DropDownList ddlacknowledgement = (DropDownList)gr.FindControl("ddlacknowledgement");
                            System.Web.UI.HtmlControls.HtmlInputHidden hdncustom = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdncustom");

                            objSql.ExecuteNonQuery("UPDATE tb_OrderedShoppingCartItems SET IsAccepted=" + ddlacknowledgement.SelectedValue.ToString() + " WHERE OrderedCustomCartID =" + hdncustom.Value.ToString() + " ");
                            transactionCommand.Append("<InvoiceLineId acknowledgement=\"" + ddlacknowledgement.SelectedItem.Text.ToString().ToLower() + "\">" + OrderItemID.Text.ToString() + "</InvoiceLineId>");
                        }
                    }

                    objSql.ExecuteNonQuery("UPDATE tb_Order SET IsOverStockProcess=1 WHERE OrderNumber=" + lblorderNumber.Text.ToString() + " ");
                }

            }
            transactionCommand.Append("</ProcessingOrders>");
            transactionCommand.Append("</Request>");
            if (flg == true)
            {
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
                    //imgprocessOrder.Visible = false;
                    //imgshortshiplineOrder.Visible = false;
                }
                catch
                {
                }
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
            //  String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + Storeid.ToString() + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            transactionCommand.Append("<supplierShipmentMessage xmlns=\"api.supplieroasis.com\">");


            bool flg = false;
            foreach (GridViewRow gr1 in grdAllorder.Rows)
            {
                GridView gvProducts = (GridView)gr1.FindControl("gvProducts");
                Label lblorderNumber = (Label)gr1.FindControl("lblorderNumber");
                CheckBox chkselect = (CheckBox)gr1.FindControl("chkselect");
                if (chkselect.Checked == true)
                {
                    foreach (GridViewRow gr in gvProducts.Rows)
                    {

                        String strCheck = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(RefSalesChannel,RefOrderId) FROM tb_Order WHERE isnull(IsOverStockProcess,0) = 1 AND OrderNumber=" + lblorderNumber.Text.ToString() + ""));
                        if (!String.IsNullOrEmpty(strCheck))
                        {

                            Label lblCustomCartID = (Label)gr.FindControl("lblCustomCartID");

                            string linenumber = "";
                            linenumber = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(LineNumber,0) from tb_OrderedShoppingCartItems where OrderedCustomCartID=" + lblCustomCartID.Text.ToString() + ""));



                            transactionCommand.Append("<supplierShipment>");
                            transactionCommand.Append("<salesChannelName>OSTK</salesChannelName>");


                            flg = true;
                            Label lblaccepted = (Label)gr.FindControl("lblaccepted");
                            Label OrderItemID = (Label)gr.FindControl("OrderItemID");
                            DropDownList ddlacknowledgement = (DropDownList)gr.FindControl("ddlacknowledgement");
                            System.Web.UI.HtmlControls.HtmlInputHidden hdncustom = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdncustom");
                            SQLAccess objSql = new SQLAccess();

                            objSql.ExecuteNonQuery("UPDATE tb_Order SET IsOverStockProcess=2 WHERE OrderNumber=" + lblorderNumber.Text.ToString() + "");
                            transactionCommand.Append("<salesChannelOrderNumber>" + strCheck.ToString() + "</salesChannelOrderNumber>");
                            transactionCommand.Append("<salesChannelLineNumber>" + linenumber + "</salesChannelLineNumber>");
                            transactionCommand.Append("<warehouse><code>Exclusive</code></warehouse>");
                            transactionCommand.Append("<shortShip><reasonCode><code>" + AppLogic.AppConfigs("OverStockReasonCode").ToString() + "</code></reasonCode></shortShip>");
                            transactionCommand.Append("</supplierShipment>");
                        }
                    }
                }

            }
            transactionCommand.Append("</supplierShipmentMessage>");

            if (flg == true)
            {
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
                    //imgprocessOrder.Visible = false;
                    //imgshortshiplineOrder.Visible = false;
                }
                catch
                {
                }
            }

        }

        private static string GetAuthorization(string User, string Password)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);

            return Convert.ToBase64String(utf8encoder.GetBytes(string.Format("{0}:{1}", User, Password)));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Fillorder();
        }
        protected void btnUploadOrder_Click(object sender, EventArgs e)
        {
            // for loop
            foreach (GridViewRow gr1 in grdAllorder.Rows)
            {
                GridView gvProducts = (GridView)gr1.FindControl("gvProducts");
                Label lblorderNumber = (Label)gr1.FindControl("lblorderNumber");
                CheckBox chkselect = (CheckBox)gr1.FindControl("chkselect");
                if (chkselect.Checked == true)
                {
                    Int32 strCheck = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT count(OrderNumber) FROM tb_Order WHERE isnull(isNAVInserted,0)=1 AND isnull(isnavcompleted,0) = 0 AND OrderNumber=" + lblorderNumber.Text.ToString() + ""));
                    if (strCheck > 0)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_order SET isNAVInserted=0,isnavcompleted=1,IsNavError=0,NAVError='' WHERE OrderNumber=" + lblorderNumber.Text.ToString() + "");
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order uploaded successfully, Please check in NAV after 5 minutes.','Message');", true);
            Fillorder();
        }
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            Fillorder();
        }

    }
}