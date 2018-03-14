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
    public partial class BulkOrderProcess : BasePage
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
                GetStoreList();
                Fillorder();

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnUploadOrder.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload-order.gif) no-repeat transparent; width: 63px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");
            }

        }

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <param name="ddlStore">DropDownlist ddlStore</param>
        private void GetStoreList()
        {
            ddlStore.Items.Clear();
            DataSet dsStore = new DataSet();

            dsStore = CommonComponent.GetCommonDataSet("select StoreID,StoreName from tb_store where StoreID in (3,4) and isnull(Deleted,0)=0 order by DisplayOrder");
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataTextField = "StoreName";

                ddlStore.DataBind();
            }
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }






        }
        private void Fillorder()
        {
            DataSet dsorder = new DataSet();
            string sttoreid = " AND StoreId=" + ddlStore.SelectedValue.ToString() + " ";
            ViewState["Storeid"] = ddlStore.SelectedValue.ToString();
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
            if (istrackingpartner.Checked)
            {

                dsorder = CommonComponent.GetCommonDataSet("SELECT top 50   OrderNumber,Orderdate,isnull(Reforderid,'') as Reforderid,StoreID FROM tb_Order  WHERE isnull(IsBackendProcessed,0)=1 and  isnull(deleted,0)=0  and isnull(OrderStatus,'')='Shipped' and OrderNumber in (select OrderNumber from tb_OrderShippedItems where isnull(TrackingNumber,'')<>'') " + sttoreid + " Order By Orderdate DESC");
            }
            else
            {
                dsorder = CommonComponent.GetCommonDataSet("SELECT top 50   OrderNumber,Orderdate,isnull(Reforderid,'') as Reforderid,StoreID FROM tb_Order  WHERE isnull(IsBackendProcessed,0)=0 and  isnull(deleted,0)=0  and isnull(OrderStatus,'')='Shipped' and OrderNumber in (select OrderNumber from tb_OrderShippedItems where isnull(TrackingNumber,'')<>'') " + sttoreid + " Order By Orderdate DESC");

            }

            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                grdAllorder.DataSource = dsorder;
                grdAllorder.DataBind();
                trsavevendor.Visible = true;
                trTop.Visible = true;
                trbottom.Visible = true;
                if (istrackingpartner.Checked)
                {
                    btnUploadOrder.Visible = false;
                }
                else
                {
                    btnUploadOrder.Visible = true;
                }
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


            dsProducts = CommonComponent.GetCommonDataSet("Exec usp_BulkOrderProcess " + OrderNumber + "");


            gvProducts.DataSource = dsProducts;
            gvProducts.DataBind();

        }
        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //if (ViewState["Storeid"] != null)
                //{
                //    Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
                //}
                //if (Storeid == 14 || Storeid == 4)
                //{
                //}
                //else
                //{
                //    e.Row.Cells[12].Visible = false;
                //}
                //if (IsCouponDiscount == false)
                //{
                //    e.Row.Cells[11].Visible = false;
                //}
                //e.Row.Cells[5].Attributes.Add("style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //                e.Row.Cells[5].Attributes.Add("style", "display:none");
                //                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                //                Label lblInventory = (Label)e.Row.FindControl("lblInventory");
                //                Label lblUpgradeSKU = (Label)e.Row.FindControl("lblUpgradeSKU");
                //                Label lblCustomCartID = (Label)e.Row.FindControl("lblCustomCartID");
                //                Label lblAssambly = (Label)e.Row.FindControl("lblAssambly");
                //                Label lblVariantname = (Label)e.Row.FindControl("lblVariantname");
                //                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                //                Label lblName = (Label)e.Row.FindControl("lblName");
                //                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                //                Label lblaccepted = (Label)e.Row.FindControl("lblaccepted");
                //                Label OrderItemID = (Label)e.Row.FindControl("OrderItemID");
                //                DropDownList ddlacknowledgement = (DropDownList)e.Row.FindControl("ddlacknowledgement");
                //                DropDownList ddlupgradesku = (DropDownList)e.Row.FindControl("ddlupgradesku");
                //                Label lblSKUupgrade = (Label)e.Row.FindControl("lblSKUupgrade");
                //                ImageButton btnEditsku = (ImageButton)e.Row.FindControl("btnEditsku");
                //                Label lblOrderQty = (Label)e.Row.FindControl("lblOrderQty");
                //                Label lblShippedQty = (Label)e.Row.FindControl("lblShippedQty");

                //                if (ViewState["Storeid"] != null)
                //                {
                //                    Storeid = Convert.ToInt32(ViewState["Storeid"].ToString());
                //                }

                //                lblShippedQty.Text = "0";
                //                if (ViewState["Storeid"] != null)
                //                {
                //                    Int32 OrderQty = 0;
                //                    Int32.TryParse(lblOrderQty.Text.ToString(), out OrderQty);

                //                    string StrInv = Convert.ToString(CommonComponent.GetScalarCommonData("Select Isnull(inventory,0) as inventory from tb_Product Where Productid=" + lblProductID.Text.ToString() + ""));
                //                    if (!string.IsNullOrEmpty(StrInv))
                //                    {
                //                        Int32 Inventory = 0;
                //                        Int32.TryParse(StrInv.ToString(), out Inventory);
                //                        if (Inventory > 0)
                //                        {
                //                            if (Inventory >= OrderQty)
                //                            {
                //                                lblShippedQty.Text = OrderQty.ToString();
                //                            }
                //                            else
                //                            {
                //                                lblShippedQty.Text = Inventory.ToString();
                //                            }
                //                        }
                //                        else { lblShippedQty.Text = "0"; }
                //                    }
                //                    else
                //                    {
                //                        string StrProVariInv = Convert.ToString(CommonComponent.GetScalarCommonData("Select Isnull(Inventory,0) as Inventory from tb_ProductVariantValue Where sku='" + lblSKU.Text.Trim().Replace("'", "''") + "' and Productid=" + lblProductID.Text.ToString() + ""));
                //                        if (!string.IsNullOrEmpty(StrProVariInv))
                //                        {
                //                            Int32 Inventory = 0;
                //                            Int32.TryParse(StrProVariInv.ToString(), out Inventory);
                //                            if (Inventory > 0)
                //                            {
                //                                if (Inventory >= OrderQty)
                //                                {
                //                                    lblShippedQty.Text = OrderQty.ToString();
                //                                }
                //                                else
                //                                {
                //                                    lblShippedQty.Text = Inventory.ToString();
                //                                }
                //                            }
                //                            else { lblShippedQty.Text = "0"; }
                //                        }
                //                    }
                //                }

                //                if (Storeid == 14 || Storeid == 4)
                //                {
                //                    if (lblaccepted != null && !string.IsNullOrEmpty(lblaccepted.Text.ToString().Trim()) && (lblaccepted.Text.ToString().Trim() == "1" || lblaccepted.Text.ToString().Trim().ToLower() == "true"))
                //                    {
                //                        ddlacknowledgement.SelectedValue = "1";
                //                    }
                //                    else
                //                    {
                //                        ddlacknowledgement.SelectedValue = "0";
                //                    }
                //                }
                //                else
                //                {
                //                    e.Row.Cells[12].Visible = false;
                //                }

                //                if (IsCouponDiscount == false)
                //                {
                //                    e.Row.Cells[11].Visible = false;
                //                }

                //                if (lblProductID != null)
                //                {


                //                    //string StrQuery = " SElect ProductAssemblyID,RefProductID, tb_ProductAssembly.ProductID,tb_product.name,tb_product.Sku,ISNULL(Quantity,0) as Quantity from tb_ProductAssembly " +
                //                    //                  " inner join tb_product on tb_ProductAssembly.ProductID=tb_product.ProductID " +
                //                    //                  " where RefProductID= " + lblProductID.Text.ToString() + " and ISNULL(tb_product.Active,1)=1 and ISNULL(Deleted,0)=0";
                //                    //DataSet dsAssamble = new DataSet();
                //                    //dsAssamble = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                //                    //if (dsAssamble != null && dsAssamble.Tables.Count > 0 && dsAssamble.Tables[0].Rows.Count > 0)
                //                    //{
                //                    //    for (int i = 0; i < dsAssamble.Tables[0].Rows.Count; i++)
                //                    //    {
                //                    //        lblAssambly.Text += dsAssamble.Tables[0].Rows[i]["Name"].ToString() + " - Qty (" + dsAssamble.Tables[0].Rows[i]["Quantity"].ToString() + ")<br />";
                //                    //    }
                //                    //}
                //                }

                //                if (Convert.ToInt32(dsProducts.Tables[0].Rows[e.Row.RowIndex]["StoreID"].ToString()) != 1)
                //                {
                //                    if (dsProducts.Tables[0].Rows[e.Row.RowIndex]["UPC"].ToString() == "")
                //                    {
                //                        if (!bApproved)
                //                        {
                //                            lblInventory.Text = @"
                //                <a onclick='javascript:OpenInventory(" + dsProducts.Tables[0].Rows[e.Row.RowIndex]["ProductID"].ToString() + ")' href='javascript:void(0);' >[Not Found]</a>";
                //                        }
                //                    }
                //                }
                //                if (dsProducts.Tables[0].Rows[e.Row.RowIndex]["MarryProducts"].ToString() == "")
                //                {
                //                    if (!bApproved)
                //                    {
                //                        lblUpgradeSKU.Text = @"
                //                <a onclick='javascript:OpenInventoryForSKU(" + dsProducts.Tables[0].Rows[e.Row.RowIndex]["ProductID"].ToString() + "," + dsProducts.Tables[0].Rows[e.Row.RowIndex]["StoreID"].ToString() + "," + lblCustomCartID.Text.ToString() + ")' href='javascript:void(0);' >[Not Found]</a>";
                //                    }
                //                }
                //                string strRef = "";
                //                string strRefSku = "";
                //                lblName.Text = lblName.Text.ToString() + BindVariant(lblVariantname.Text.ToString(), lblVariantValues.Text.ToString(), Convert.ToInt32(lblProductID.Text.ToString()), ref strRef, ref strRefSku);
                //                if (strRef != "")
                //                {
                //                    lblSKU.Text = strRef;
                //                }
                //                if (lblProductID != null)
                //                {
                //                    string[] strnm = lblVariantname.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //                    string[] strval = lblVariantValues.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //                    if (strnm.Length > 0)
                //                    {
                //                        for (int i = 0; i < strnm.Length; i++)
                //                        {
                //                            if (strnm[i].ToString().ToLower().IndexOf("select size") > -1 || strnm[i].ToString().ToLower().IndexOf("header design") > -1)
                //                            {
                //                                DataSet ds = new DataSet();
                //                                if (strRefSku != "")
                //                                {

                //                                }
                //                                else
                //                                {
                //                                    strRefSku = lblSKU.Text.ToString();
                //                                }
                //                                string strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ProductId,'0') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=1 AND SKU='" + strRefSku.ToString() + "'"));
                //                                if (!string.IsNullOrEmpty(strp))
                //                                {
                //                                    ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM tb_ProductVariantValue WHERE VariantID in (SELECT VariantID FROM tb_ProductVariant WHERE ParentId in (SELECT VariantValueID FROM tb_ProductVariantValue WHERE  VariantValue = '" + strval[i].ToString() + "' AND ProductId=" + strp + ")) AND isnull(SKU,'')<> ''");
                //                                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                                    {
                //                                        ddlupgradesku.DataSource = ds.Tables[0];
                //                                        ddlupgradesku.DataTextField = "SKU";
                //                                        ddlupgradesku.DataValueField = "SKU";
                //                                        ddlupgradesku.DataBind();
                //                                    }
                //                                    else
                //                                    {
                //                                        strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + strRefSku.ToString() + "'"));
                //                                        if (!string.IsNullOrEmpty(strp))
                //                                        {

                //                                            strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                //                                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                                            {
                //                                                ddlupgradesku.DataSource = ds.Tables[0];
                //                                                ddlupgradesku.DataTextField = "SKU";
                //                                                ddlupgradesku.DataValueField = "SKU";
                //                                                ddlupgradesku.DataBind();
                //                                            }
                //                                        }
                //                                        else
                //                                        {
                //                                            string strsku1 = "";

                //                                            if (strRefSku.IndexOf("-84") > -1)
                //                                            {
                //                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                //                                            }
                //                                            else if (strRefSku.IndexOf("-96") > -1)
                //                                            {
                //                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                //                                            }
                //                                            else if (strRefSku.IndexOf("-108") > -1)
                //                                            {
                //                                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                //                                                //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                                            }
                //                                            ds = new DataSet();

                //                                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                //                                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                                            {
                //                                                ddlupgradesku.DataSource = ds.Tables[0];
                //                                                ddlupgradesku.DataTextField = "SKU";
                //                                                ddlupgradesku.DataValueField = "SKU";
                //                                                ddlupgradesku.DataBind();
                //                                            }

                //                                        }
                //                                    }

                //                                }
                //                                else
                //                                {
                //                                    strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + strRefSku.ToString() + "'"));
                //                                    if (!string.IsNullOrEmpty(strp))
                //                                    {
                //                                        ds = new DataSet();
                //                                        strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                                        ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                //                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                                        {
                //                                            ddlupgradesku.DataSource = ds.Tables[0];
                //                                            ddlupgradesku.DataTextField = "SKU";
                //                                            ddlupgradesku.DataValueField = "SKU";
                //                                            ddlupgradesku.DataBind();
                //                                        }
                //                                    }
                //                                    else
                //                                    {
                //                                        string strsku1 = "";

                //                                        if (strRefSku.IndexOf("-84") > -1)
                //                                        {
                //                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                //                                        }
                //                                        else if (strRefSku.IndexOf("-96") > -1)
                //                                        {
                //                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                //                                        }
                //                                        else if (strRefSku.IndexOf("-108") > -1)
                //                                        {
                //                                            strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                //                                            //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                                        }
                //                                        ds = new DataSet();

                //                                        ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                //                                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                                        {
                //                                            ddlupgradesku.DataSource = ds.Tables[0];
                //                                            ddlupgradesku.DataTextField = "SKU";
                //                                            ddlupgradesku.DataValueField = "SKU";
                //                                            ddlupgradesku.DataBind();
                //                                        }

                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        strRefSku = lblSKU.Text.ToString();
                //                        string strp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(OptionSKU,'') FROM tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU='" + lblSKU.Text.ToString() + "'"));
                //                        if (!string.IsNullOrEmpty(strp))
                //                        {
                //                            DataSet ds = new DataSet();
                //                            strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND OptionSKU like '%" + strp.ToString() + "%'");
                //                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                            {
                //                                ddlupgradesku.DataSource = ds.Tables[0];
                //                                ddlupgradesku.DataTextField = "SKU";
                //                                ddlupgradesku.DataValueField = "SKU";
                //                                ddlupgradesku.DataBind();
                //                            }
                //                        }
                //                        else
                //                        {
                //                            string strsku1 = "";

                //                            if (strRefSku.IndexOf("-84") > -1)
                //                            {
                //                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-84", "-96").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-84", "-120").Replace("'", "''") + "'";
                //                            }
                //                            else if (strRefSku.IndexOf("-96") > -1)
                //                            {
                //                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-96", "-108").Replace("'", "''") + "','" + strRefSku.Replace("-96", "-120").Replace("'", "''") + "'";
                //                            }
                //                            else if (strRefSku.IndexOf("-108") > -1)
                //                            {
                //                                strsku1 = "'" + strRefSku.Replace("'", "''") + "','" + strRefSku.Replace("-108", "-120").Replace("'", "''") + "'";
                //                                //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                //                            }
                //                            DataSet ds = new DataSet();

                //                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU FROM  tb_product WHERE isnull(active,0)=1 and isnull(Deleted,0)=0 and Storeid=" + Storeid.ToString() + " AND SKU in ('" + strsku1 + "')");
                //                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //                            {
                //                                ddlupgradesku.DataSource = ds.Tables[0];
                //                                ddlupgradesku.DataTextField = "SKU";
                //                                ddlupgradesku.DataValueField = "SKU";
                //                                ddlupgradesku.DataBind();
                //                            }

                //                        }
                //                    }
                //                    ddlupgradesku.Items.Insert(0, new ListItem("None", ""));
                //                    if (lblSKUupgrade != null && lblSKUupgrade.Text.ToString().Trim() != "")
                //                    {
                //                        if (strRefSku != "")
                //                        {
                //                            lblSKU.Text = lblSKU.Text.Replace(strRefSku, lblSKUupgrade.Text.ToString().Trim());
                //                        }
                //                        try
                //                        {
                //                            ddlupgradesku.SelectedValue = lblSKUupgrade.Text.ToString();
                //                        }
                //                        catch
                //                        {
                //                        }
                //                        ddlupgradesku.Visible = false;//ddlupgradesku.Visible = true;
                //                        btnEditsku.Visible = false;
                //                    }
                //                    else
                //                    {
                //                        ddlupgradesku.Visible = false;
                //                        if (ddlupgradesku.Items.Count > 0)
                //                        {
                //                            ddlupgradesku.SelectedIndex = 0;
                //                        }
                //                        btnEditsku.Visible = false; //btnEditsku.Visible = true;
                //                    }
                //                }

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Fillorder();
        }
        protected void btnUploadOrder_Click(object sender, EventArgs e)
        {
            string strorders = "";
            // for loop
            foreach (GridViewRow gr1 in grdAllorder.Rows)
            {
                GridView gvProducts = (GridView)gr1.FindControl("gvProducts");
                Label lblorderNumber = (Label)gr1.FindControl("lblorderNumber");
                Label lblreforderid = (Label)gr1.FindControl("lblreforderid");

                CheckBox chkselect = (CheckBox)gr1.FindControl("chkselect");
                if (chkselect.Checked == true)
                {
                    if (ddlStore.SelectedValue.ToString() == "3")
                    {
                        strorders += lblorderNumber.Text.ToString() + ",";
                    }
                    else
                    {
                        GetConfirmShipmentOverStock(Convert.ToInt32(ddlStore.SelectedValue.ToString()), Convert.ToInt32(lblorderNumber.Text.ToString()), lblreforderid.Text.ToString());
                    }
                }
            }

            if (!string.IsNullOrEmpty(strorders))
            {
                strorders = strorders.Substring(0, strorders.ToString().Length - 1);
                if (ddlStore.SelectedValue.ToString() == "3")
                {
                    SendShippingFile(strorders);
                }

            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order uploaded successfully.','Message');", true);
            Fillorder();
        }


        private void GetConfirmShipmentOverStock(Int32 StoreId, Int32 OrderNumber, string Reforderid)
        {

             Reforderid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(RefSalesChannel,RefOrderId) FROM tb_Order WHERE  OrderNumber=" + OrderNumber + ""));
            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
(
delegate { return true; }
);
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            string OverstockUserName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockUserName' AND Storeid=" + StoreId + ""));
            string OverstockPassword = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockPassword' AND Storeid=" + StoreId.ToString() + ""));
            //String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + StoreId.ToString() + ""));
            string shippingmethodname = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ShippingMethod,'')  from tb_Order where ordernumber=" + OrderNumber + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            transactionCommand.Append("<supplierShipmentMessage xmlns=\"api.supplieroasis.com\">");
           
            
            DataSet DsCItems = new DataSet();
            DsCItems = CommonComponent.GetCommonDataSet("SELECT Isnull(tb_OrderedShoppingCartItems.Inventoryupdated,0) as Inventoryupdated,tb_OrderedShoppingCartItems.linenumber ,(isnull(tb_OrderedShoppingCartItems.WareHouseID,0)) as WareHouseID,(isnull(tb_OrderedShoppingCartItems.ShippedQty,0)) as ShippedQty,tb_Product.Name,tb_Product.SKU, tb_OrderedShoppingCartItems.Quantity,"
                                                        + "    tb_OrderedShoppingCartItems.OrderedCustomCartID, tb_OrderedShoppingCartItems.RefProductID,"
                                                        + "    tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues,"
                                                        + "    (isnull(s.TrackingNumber,tb_OrderedShoppingCartItems.TrackingNumber)) TrackingNumber,isnull(s.ShippedVia,tb_OrderedShoppingCartItems.ShippedVia) ShippedVia,"
                                                        + "   (isnull(s.Shipped,0)) as Shipped,isnull(s.Shipped,0) as ShippedProduct,"
                                                        + "    isnull(s.ShippedOn,tb_OrderedShoppingCartItems.ShippedOn) ShippedOn,  tb_Product.Description,tb_OrderedShoppingCartItems.Price As "
                                                        + "    SalePrice, isnull(s.ShippedNote,'') as ShippedNote,isnull(tb_OrderedShoppingCartItems.OrderItemID,'') as OrderItemID"
                                                        + "    FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems"
                                                        + "    left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,"
                                                        + "    ShippedNote from  tb_OrderShippedItems where OrderNumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  "
                                                        + "    inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber"
                                                        + "    as TrackingNumber,ShippedVia,ShippedOn from tb_Order ) o ON  o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID "
                                                        + "    on tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID Where OrderedShoppingCartID = (select ShoppingCardID FROM  tb_Order where OrderNumber=" + OrderNumber + ")");
            if (DsCItems != null && DsCItems.Tables.Count > 0 && DsCItems.Tables[0].Rows.Count > 0)
            {
                bool chkisShipp = false;
                for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["TrackingNumber"].ToString()) && !string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString()) && !string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString()))
                    {
                        chkisShipp = true;
                        transactionCommand.Append("<supplierShipment>");
                        transactionCommand.Append("<salesChannelName>OSTK</salesChannelName>");
                        transactionCommand.Append("<salesChannelOrderNumber>" + Reforderid.ToString() + "</salesChannelOrderNumber>");
                        transactionCommand.Append("<salesChannelLineNumber>" + DsCItems.Tables[0].Rows[i]["linenumber"].ToString() + "</salesChannelLineNumber>");
                        transactionCommand.Append("<warehouse><code>Exclusive</code></warehouse>");

                       
                        
                       
                        //if (DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString().ToLower().Trim() == "fedex")
                        //{
                        //    transactionCommand.Append("<supplierShipConfirmation><quantity>" + DsCItems.Tables[0].Rows[i]["ShippedQty"].ToString() + "</quantity><carrier><code>FEDX</code></carrier>");
                        //}
                        //else
                        //{
                            transactionCommand.Append("<supplierShipConfirmation><quantity>" + DsCItems.Tables[0].Rows[i]["ShippedQty"].ToString() + "</quantity><carrier><code>" + DsCItems.Tables[0].Rows[i]["ShippedVia"].ToString() + "</code></carrier>");
                           
                       // }
                        transactionCommand.Append("<trackingNumber>" + DsCItems.Tables[0].Rows[i]["TrackingNumber"].ToString() + "</trackingNumber>");
                        transactionCommand.Append("<shipDate>" + string.Format("{0:yyyy-MM-ddThh:mm:ss}", Convert.ToDateTime(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString())) + "</shipDate>");
                        transactionCommand.Append("<serviceLevel><code>" + shippingmethodname.ToString() + "</code></serviceLevel></supplierShipConfirmation>");
                        transactionCommand.Append("</supplierShipment>");
                       
                    }
                }

                transactionCommand.Append("</supplierShipmentMessage>");
                //transactionCommand.Append("</Request>");
                if (chkisShipp == true)
                {
                    //System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
                    //byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

                    ////String AuthServer = "https://sapiqa.overstock.com/api";
                    //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    //myRequest.Method = "POST";
                    //myRequest.Timeout = 300000;
                    //myRequest.Headers.Add("SapiMethodName", "ConfirmShipment");
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
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.supplieroasis.com/shipments?jobName=");
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
                            ds.WriteXml(Server.MapPath("/OverstockOrder/" + Reforderid.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".xml"));

                        }
                        catch { }
                        myResponse.Close();
                        CommonComponent.ExecuteCommonData("update tb_order set IsBackendProcessed=1 where ordernumber=" + OrderNumber + "");
                    }
                    catch (Exception ex)
                    {
                        StreamWriter sw = new StreamWriter(Server.MapPath("/OverstockOrder/" + Reforderid.ToString() + "-" + DateTime.Now.ToString() + "_1.txt"));
                        sw.WriteLine(DateTime.Now.Date.ToString());
                        sw.WriteLine(ex.Message.ToString() + " " + ex.StackTrace.ToString());
                        sw.Close();
                        sw.Dispose();
                    }
                }

            }


        }
        private static string GetAuthorization(string User, string Password)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);

            return Convert.ToBase64String(utf8encoder.GetBytes(string.Format("{0}:{1}", User, Password)));
        }
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            istrackingpartner.Checked = false;
            Fillorder();
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlStore.SelectedValue.ToString() == "4")
            {
                txtAllOrernumber.Visible = true;
                btnupdateoverstock.Visible = true;
            }
            else
            {
                txtAllOrernumber.Visible = false;
                btnupdateoverstock.Visible = false;
            }
            Fillorder();

        }


        #region "Amazon OrderShipping Insert/Update"
        private void CreateShippingMessage(string ProductID, int OrderNumber, int MessageID, MemoryStream ParentDoc, string orders)
        {
            string myString;

            string strresponse = "";
            //string Query = " SELECT  isnull(tb_Order.Reforderid,'') as Reforderid,case when isnull(tb_OrderedShoppingCartItems.OrderItemId,'')='' then isnull(tb_Product.amazonproductid,'') else isnull(tb_OrderedShoppingCartItems.OrderItemId,'') end as amazonproductid, tb_Order.ShippingTrackingNumber, tb_Order.ShippedVia,Convert(char(10),tb_Order.Shippedon,101) as  Shippedon,tb_Order.ShippingMethod,tb_OrderedShoppingCartItems.Quantity as Quantity,tb_OrderedShoppingCartItems.OrderedCustomCartID  " +
            //               " FROM         tb_Product INNER JOIN " +
            //               " tb_OrderedShoppingCartItems ON tb_Product.ProductID = tb_OrderedShoppingCartItems.ProductID INNER JOIN " +
            //               " tb_Order ON tb_OrderedShoppingCartItems.OrderedShoppingCartID = tb_Order.ShoppingCartID " +
            //               "  WHERE isnull(tb_OrderedShoppingCartItems.isupload,0)=0  AND tb_Order.Storeid=3 and isnull(tb_Order.Deleted,0)=0 and ordernumber in ('" + orders + "')";

            string[] strorlist = orders.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strorlist.Length > 0)
            {

                int iCount = 0;
                foreach (string st in strorlist)
                {
                    string Query = "SELECT  isnull(tb_Order.Reforderid,'') as Reforderid,case when isnull(tb_OrderedShoppingCartItems.OrderItemId,'')='' then isnull(tb_Product.amazonproductid,'') else isnull(tb_OrderedShoppingCartItems.OrderItemId,'') end as amazonproductid," +
           "tb_OrderShippedItems.TrackingNumber as ShippingTrackingNumber, tb_OrderShippedItems.ShippedVia,tb_OrderShippedItems.Shippedon as  Shippedon,tb_Order.ShippingMethod,tb_OrderedShoppingCartItems.Quantity as Quantity,tb_OrderedShoppingCartItems.OrderedCustomCartID " +
                                    "FROM         tb_Product INNER JOIN " +
                                    "tb_OrderedShoppingCartItems ON tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID INNER JOIN " +
                                    "tb_Order ON tb_OrderedShoppingCartItems.OrderedShoppingCartID = tb_Order.ShoppingCardID inner join tb_OrderShippedItems on tb_Order.OrderNumber=tb_OrderShippedItems.OrderNumber " +
                                   "WHERE tb_Order.Storeid=3 and isnull(tb_Order.Deleted,0)=0 and isnull(tb_OrderShippedItems.TrackingNumber,'')<>'' and tb_order.ordernumber in ('" + st.ToString() + "')";


                    DataSet Ds = new DataSet();

                    Ds = CommonComponent.GetCommonDataSet(Query);

                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                    {


                        for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["amazonproductid"].ToString()) && !string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString()) && !string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["Reforderid"].ToString()))
                            {
                                iCount += 1;
                                myString = "<Message>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<MessageID>" + iCount.ToString() + "</MessageID>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                //myString = "<OperationType>Update</OperationType>";
                                //this.AddStringToStream(ref myString, ParentDoc);


                                myString = "<OrderFulfillment>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<AmazonOrderID>" + Ds.Tables[0].Rows[i]["Reforderid"].ToString() + "</AmazonOrderID>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["Shippedon"].ToString()))
                                {
                                    myString = "<FulfillmentDate>" + String.Format(@"{0:yyyy-MM-dd\THH:mm:ss}", Convert.ToDateTime(Ds.Tables[0].Rows[i]["Shippedon"].ToString())) + "</FulfillmentDate>";
                                    //myString = "<FulfillmentDate>" + String.Format(@"{0:yyyy-MM-dd}", Convert.ToDateTime(Ds.Tables[0].Rows[i]["Shippedon"].ToString())) + "T09:00:00</FulfillmentDate>";
                                }
                                else
                                {
                                    // myString = "<FulfillmentDate>" + String.Format(@"{0:yyyy-MM-dd}", Convert.ToDateTime(DateTime.Now.Date.ToString())) + "T09:00:00</FulfillmentDate>";
                                    myString = "<FulfillmentDate>" + String.Format(@"{0:yyyy-MM-dd\THH:mm:ss}", Convert.ToDateTime(DateTime.Now.ToString())) + "</FulfillmentDate>";
                                }

                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<FulfillmentData>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["ShippedVia"].ToString()) && Ds.Tables[0].Rows[i]["ShippedVia"].ToString().IndexOf(",") > -1)
                                {
                                    string[] strVia = Ds.Tables[0].Rows[i]["ShippedVia"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (strVia != null && strVia.Length > 0)
                                    {
                                        if (strVia[0].ToString().ToLower().Trim() == "fedex")
                                        {
                                            myString = "<CarrierCode>FedEx</CarrierCode>";
                                        }
                                        else
                                        {
                                            myString = "<CarrierCode>" + strVia[0].ToString() + "</CarrierCode>";
                                        }
                                    }
                                    else
                                    {
                                        myString = "<CarrierCode></CarrierCode>";
                                    }
                                }
                                else
                                {
                                    if (Ds.Tables[0].Rows[i]["ShippedVia"].ToString().ToLower().Trim() == "fedex")
                                    {
                                        myString = "<CarrierCode>FedEx</CarrierCode>";
                                    }
                                    else
                                    {
                                        myString = "<CarrierCode>" + Convert.ToString(Ds.Tables[0].Rows[i]["ShippedVia"].ToString()) + "</CarrierCode>";
                                    }

                                }
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<ShippingMethod>" + Convert.ToString(Ds.Tables[0].Rows[i]["ShippingMethod"].ToString()) + "</ShippingMethod>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString()) && Ds.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString().IndexOf(",") > -1)
                                {
                                    string[] strTrackingNumber = Ds.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (strTrackingNumber != null && strTrackingNumber.Length > 0)
                                    {
                                        myString = "<ShipperTrackingNumber>" + Convert.ToString(strTrackingNumber[0].ToString()) + "</ShipperTrackingNumber>";
                                    }
                                    else
                                    {
                                        myString = "<ShipperTrackingNumber></ShipperTrackingNumber>";
                                    }
                                }
                                else
                                {
                                    myString = "<ShipperTrackingNumber>" + Convert.ToString(Ds.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString()) + "</ShipperTrackingNumber>";
                                }
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "</FulfillmentData>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                //if (Ds.Tables[0].Rows.Count > 1)
                                //{


                                myString = "<Item>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<AmazonOrderItemCode>" + Ds.Tables[0].Rows[i]["amazonproductid"].ToString() + "</AmazonOrderItemCode>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                //myString = "<MerchantFulfillmentItemID>40806703143250</MerchantFulfillmentItemID>";
                                //this.AddStringToStream(ref myString, ParentDoc);
                                myString = "<Quantity>" + Ds.Tables[0].Rows[i]["Quantity"].ToString() + "</Quantity>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "</Item>";
                                this.AddStringToStream(ref myString, ParentDoc);
                                //}
                                myString = "</OrderFulfillment>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "</Message>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                try
                                {

                                    CommonComponent.ExecuteCommonData("update tb_order set IsBackendProcessed=1 where ordernumber in ('" + st + "')");
                                }
                                catch
                                {
                                }
                            }
                        }

                    }

                }
            }

        }
        public System.IO.MemoryStream GetShippingFeed(string id, int orderNumber, int ProductIds, string orders)
        {
            System.IO.MemoryStream myDocument = new System.IO.MemoryStream();
            string myString;

            //Add the document header.
            //myString = "<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>";  //encoding="UTF-8"?
            myString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<AmazonEnvelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\">";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<DocumentVersion>1.01</DocumentVersion>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MerchantIdentifier>" + id + "</MerchantIdentifier>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "</Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MessageType>OrderFulfillment</MessageType>";
            this.AddStringToStream(ref myString, myDocument);


            this.CreateShippingMessage(ProductIds.ToString(), Convert.ToInt32(orderNumber), 0, myDocument, orders);
            //}
            //myString = "<Message>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<MessageID>1</MessageID>";
            //this.AddStringToStream(ref myString, myDocument);
            ////myString = "<OperationType>Update</OperationType>";
            ////this.AddStringToStream(ref myString, myDocument);

            //myString = "<OrderFulfillment>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<AmazonOrderID>106-8215336-8289853</AmazonOrderID>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<FulfillmentDate>2015-05-18T09:00:00</FulfillmentDate>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<FulfillmentData>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<CarrierCode>FedEx</CarrierCode>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<ShippingMethod>FreeEconomy</ShippingMethod>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<ShipperTrackingNumber>569020010248740</ShipperTrackingNumber>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "</FulfillmentData>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<Item>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<AmazonOrderItemCode>20435773636826</AmazonOrderItemCode>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "<Quantity>4</Quantity>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "</Item>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "</OrderFulfillment>";
            //this.AddStringToStream(ref myString, myDocument);

            //myString = "</Message>";
            //this.AddStringToStream(ref myString, myDocument);

            myString = "</AmazonEnvelope>";
            this.AddStringToStream(ref myString, myDocument);


            return myDocument;
        }
        public void SendShippingFile(string OrderNumbrs)
        {

            //   WriteAmazonLog("Amazon Shipping Started :" + DateTime.Now.ToString());
            //string Query = " SELECT  isnull(dbo.tb_Order.Reforderid,'') as Reforderid,case when isnull(dbo.tb_OrderedShoppingCartItems.OrderItemId,'')='' then isnull(dbo.tb_Product.amazonproductid,'') else isnull(dbo.tb_OrderedShoppingCartItems.OrderItemId,'') end as amazonproductid, dbo.tb_Order.ShippingTrackingNumber, dbo.tb_Order.ShippedVia,Convert(char(10),dbo.tb_Order.Shippedon,101) as  Shippedon,dbo.tb_Order.ShippingMethod,dbo.tb_OrderedShoppingCartItems.Quantity as Quantity,dbo.tb_OrderedShoppingCartItems.OrderedCustomCartID  " +
            //              " FROM         dbo.tb_Product INNER JOIN " +
            //              " dbo.tb_OrderedShoppingCartItems ON dbo.tb_Product.ProductID = dbo.tb_OrderedShoppingCartItems.ProductID INNER JOIN " +
            //              " dbo.tb_Order ON dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID = dbo.tb_Order.ShoppingCartID " +
            //              "  WHERE isnull(tb_OrderedShoppingCartItems.isupload,0)=0  AND dbo.tb_Order.Storeid=3 and isnull(dbo.tb_Order.Deleted,0)=0 and ordernumber in ('" + OrderNumbrs + "'))";


            //          string Query = "SELECT  isnull(tb_Order.Reforderid,'') as Reforderid,case when isnull(tb_OrderedShoppingCartItems.OrderItemId,'')='' then isnull(tb_Product.amazonproductid,'') else isnull(tb_OrderedShoppingCartItems.OrderItemId,'') end as amazonproductid," +
            //"tb_OrderShippedItems.TrackingNumber as ShippingTrackingNumber, tb_OrderShippedItems.ShippedVia,Convert(char(10),tb_OrderShippedItems.Shippedon,101) as  Shippedon,tb_Order.ShippingMethod,tb_OrderedShoppingCartItems.Quantity as Quantity,tb_OrderedShoppingCartItems.OrderedCustomCartID " +
            //                         "FROM         tb_Product INNER JOIN " +
            //                         "tb_OrderedShoppingCartItems ON tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID INNER JOIN " +
            //                         "tb_Order ON tb_OrderedShoppingCartItems.OrderedShoppingCartID = tb_Order.ShoppingCardID inner join tb_OrderShippedItems on tb_Order.OrderNumber=tb_OrderShippedItems.OrderNumber " +
            //                        "WHERE tb_Order.Storeid=3 and isnull(tb_Order.Deleted,0)=0 and isnull(tb_OrderShippedItems.TrackingNumber,'')<>'' and tb_order.ordernumber in ('" + OrderNumbrs + "')";




            //          DataSet Ds = new DataSet();

            //          Ds = CommonComponent.GetCommonDataSet(Query);

            //          if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            if (!string.IsNullOrEmpty(OrderNumbrs) && OrderNumbrs.ToString().Length > 2)
            {




                String accessKeyId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));


                //for (int iLoopIds = 0; iLoopIds < ProductIds.Count; iLoopIds++)
                //{
                try
                {
                    MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                    string ServiceURLAmz = Convert.ToString(CommonComponent.GetScalarCommonData("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                    mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                    //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                    //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                    MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, "Curtainsonbudget Amazon", "1.01", mwsConfig2);
                    // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                    MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                    sfrequest.Merchant = merchantId;
                    sfrequest.Marketplace = marketplaceId;



                    System.IO.MemoryStream stre = GetShippingFeed(merchantId, Convert.ToInt32(0), Convert.ToInt32(0), OrderNumbrs);
                    sfrequest.FeedContent = stre;




                    //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                    sfrequest.FeedContent.Position = 0;
                    sfrequest.FeedType = "_POST_ORDER_FULFILLMENT_DATA_";
                    sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                    MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                    if (!Directory.Exists(Server.MapPath("/AmazonFiles")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/AmazonFiles"));
                    }

                    //try
                    //{
                    //    XmlDocument doc = new System.Xml.XmlDocument();
                    //    doc.Load(stre);
                    //    doc.Save(Server.MapPath("/AmazonFiles/Order_shipdetails_" + DateTime.Now.Date.Ticks.ToString()+".xml"));
                    //}
                    //catch
                    //{

                    //}
                    string filename = Server.MapPath("/AmazonFiles/Order_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml");
                    StreamWriter objWrite = new StreamWriter(filename, false);
                    objWrite.Write(submitres.ToXML());
                    objWrite.Close();
                    objWrite.Dispose();

                    //InvokeSubmitFeed(mwsclient, sfrequest);


                }
                catch (Exception ex)
                {

                    //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                    //{
                    //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //    SendProductFile(str[str.Length - 1].ToString().Replace(")", ""), Convert.ToInt32(orderNumber), Convert.ToInt32(ProductIds[iLoopIds].ToString()));
                    //}
                    //lblMsg.Text = ex.Message.ToString();
                    //  WriteAmazonLog("Amazon Shipping :" + ex.Message.ToString() + " " + ex.StackTrace.ToString());
                    //  SendMail("Urgent!! Amazon Shipping Information Problem", ex.Message.ToString());
                }
            }
            else
            {
                //  WriteAmazonLog("Amazon Shipping No Record Found  :" + DateTime.Now.ToString());
            }
            // }

        }

        private void AddStringToStream(ref string StringToAdd, MemoryStream StreamToAddTo)
        {
            //Convert the string into a byte array and add
            //it to the stream.
            System.Text.UTF8Encoding myEncoding =
            new System.Text.UTF8Encoding();
            byte[] myBuffer = myEncoding.GetBytes(StringToAdd);
            StreamToAddTo.Write(myBuffer, 0, myBuffer.Length);
        }
        #endregion

        protected void btnupdateoverstock_Click(object sender, EventArgs e)
        {
            if(txtAllOrernumber.Text.ToString().Trim() != "")
            {
                CommonComponent.ExecuteCommonData("update tb_order set IsBackendProcessed=0 where StoreId=4 and ordernumber in (SELECT rtrim(ltrim(items)) FROM dbo.Split_Order('" + txtAllOrernumber.Text.ToString() + "',',') WHERE isnull(items,'')<>'') and isnull(IsBackendProcessed,0)=1");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder111", "jAlert('Order updated successfully.','Message');", true);
            }
        }
    }
}