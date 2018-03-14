using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using Solution.Data;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductVariant : BasePage
    {
        public static string SearchProductTempPath = string.Empty;
        public static string SearchProductPath = string.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeIcon = Size.Empty;
        Int32 varaintid = 0;
        Int32 isparent = 0;
        Int32 pcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["HType"] != null && Convert.ToString(Request.QueryString["HType"]).Trim().ToLower() == "no")
            //{
            //    if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter3", "window.parent.location.href='/Admin/Login.aspx';", true);

            //        return;
            //    }
            //}
            if (Request.QueryString["HType"] != null && Convert.ToString(Request.QueryString["HType"]).Trim().ToLower() == "no")
            {
                String strScript = @"$(function () {
                                    $('#header').css('display', 'none');
                                    $('#footer').css('display', 'none');
                                    $('body').css('background', 'none');
                                    });";

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter", strScript.Trim(), true);
                btnGotoProduct.Visible = false;
                divcontentrow1.Visible = false;
                idVariantHeader.Visible = false;
            }
            if (Request.QueryString["ID"] != null && Convert.ToInt32(Request.QueryString["ID"]) > 0)
            {
                pcount = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(ProductId) FROM tb_Product WHERE ItemType='Roman' and ProductId=" + Request.QueryString["ID"].ToString() + ""));
            }
            if (!IsPostBack)
            {

                BindSize();
                lblMsg.Text = "";
                btnSaveOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancelOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";

                if (Request.QueryString["ID"] != null && Convert.ToInt32(Request.QueryString["ID"]) > 0)
                {
                    ProductComponent objProduct = new ProductComponent();
                    tb_Product tb_Product = new tb_Product();
                    tb_Product = objProduct.GetAllProductDetailsbyProductID(Convert.ToInt32(Request.QueryString["ID"]));
                    lblProductName.Text = Convert.ToString(tb_Product.Name);
                    FillGrid();
                }

            }

        }

        protected void btnSaveOption_Click(object sender, ImageClickEventArgs e)
        {

            decimal VariantPrice = 0;
            Int32 DisplayOrder = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);

            if (!string.IsNullOrEmpty(txtOptionName.Text.ToString()))
            {
                string VariantName = Convert.ToString(txtOptionName.Text.Trim());
                Int32 VariValue = Convert.ToInt32(ProductComponent.SaveProductVariant(Convert.ToInt32(Request.QueryString["ID"]), VariantName, "", VariantPrice, DisplayOrder, "", "", "", Convert.ToInt32(chkparent.Checked)));
                if (VariValue > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "jAlert('Option Added Successfully!','Message');", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "jAlert('Please Enter Option Name.','Message');", true);
            }

            ClearTextBox();
            FillGrid();
        }

        protected void btnCancelOption_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void btnReBindData_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grdoptionmainGroup.Rows)
            {
                GridView grdvalue = (GridView)gr.FindControl("grdvaluelisting");
                if (grdvalue.ClientID.ToString().ToLower() == hdngridid.Value.ToString().ToLower())
                {
                    foreach (GridViewRow gr2 in grdvalue.Rows)
                    {
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");

                        SQLAccess objSql = new SQLAccess();
                        //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                        Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    }

                }
                else
                {
                    foreach (GridViewRow gr1 in grdvalue.Rows)
                    {
                        GridView grdvalue1 = (GridView)gr1.FindControl("grdnamevaluelisting");
                        if (grdvalue1.ClientID.ToString().ToLower().Trim() == hdngridid.Value.ToString().ToLower().Trim())
                        {
                            foreach (GridViewRow gr2 in grdvalue1.Rows)
                            {
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");

                                SQLAccess objSql = new SQLAccess();
                                //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                                Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));

                            }
                        }
                    }
                }

            }
            hdngridid.Value = "";
            FillGrid();
        }
        private void ClearTextBox()
        {
            txtDisplayOrder.Text = "";
            txtOptionName.Text = "";
            chkparent.Checked = false;
        }

        private void FillGrid()
        {

            DataSet dsVaraint = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            dsVaraint = objSql.GetDs("SELECT * FROM tb_ProductVariant WHERE ProductID=" + Request.QueryString["ID"].ToString() + " AND isnull(ParentId,0)=0");

            //dsVaraint = (DataSet)ProductComponent.GetProductVariantByproductID(Convert.ToInt32(Request.QueryString["ID"].ToString()));
            if (dsVaraint != null && dsVaraint.Tables.Count > 0 && dsVaraint.Tables[0].Rows.Count > 0)
            {
                grdoptionmainGroup.DataSource = dsVaraint;
                grdoptionmainGroup.DataBind();
            }
            else
            {
                grdoptionmainGroup.DataSource = null;
                grdoptionmainGroup.DataBind();
            }
            if (Request.QueryString["HType"] != null && Convert.ToString(Request.QueryString["HType"]).Trim().ToLower() == "no")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto1", "var hgt = $(document).height();hgt = parseInt(hgt);window.parent.iframeAutoheightById('ContentPlaceHolder1_ifrmProductVariant',hgt);", true);
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgauto1", "$('#ContentPlaceHolder1_ifrmProductVariant').css('height', hgt.toString() + 'px');", true);

            }

        }

        /// <summary>
        /// Varaint name Listing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdoptionmainGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdvaluelisting = (GridView)e.Row.FindControl("grdvaluelisting");
                ViewState["grdvaluelisting"] = "0";
                Int32 Variantid = Convert.ToInt32(grdoptionmainGroup.DataKeys[e.Row.RowIndex].Value.ToString());
                DataSet dsListing = new DataSet();
                System.Web.UI.HtmlControls.HtmlInputHidden hdnparent = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnparent");
                Literal ltparentimage = (Literal)e.Row.FindControl("ltparentimage");
                CheckBox chkparent = (CheckBox)e.Row.FindControl("chkparent");
                Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
                dsListing = objSql.GetDs("select 1 as VarActive, 0 AS VariantValueID," + Variantid + " as VariantID,'' AS VariantValue,0 AS VariantPrice,0 AS DisplayOrder,0 AS ProductID,'' AS SKU,'' AS UPC,'' AS Header,0 as Inventory,'' as selectSKU,0 as AllowQuantity,0 as LockQuantity,0 as AddiHemingQty,0 as BasecustomPrice,0 as Buy1Get1 ,0 as OnSale,'' as FabricType,'' as FabricCode,'' as BackOrderdate ,0 as Weight,0 as updatebuy1 UNION SELECT isnull(VarActive,0) as VarActive, VariantValueID,VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID, SKU, UPC, Header,Inventory,isnull(RelatedProductid,'Select SKU') as selectSKU,isnull(AllowQuantity,0) as AllowQuantity,isnull(LockQuantity,0) as LockQuantity,ISNULL(AddiHemingQty,0) as AddiHemingQty,ISNULL(BasecustomPrice,0) as BasecustomPrice,ISNULL(Buy1Get1,0) as Buy1Get1,ISNULL(OnSale,0) as OnSale, FabricType,FabricCode ,Cast(BackOrderdate as Date) as BackOrderdate ,Isnull(Weight,0) as weight,case when cast(Buy1Todate as date) >= cast(getdate() as date) then 1 else 0 end as updatebuy1  FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["ID"].ToString() + " AND VariantID=" + Variantid + " Order By DisplayOrder");
                ViewState["varaintid"] = Variantid;
                if (!string.IsNullOrEmpty(hdnparent.Value.ToString()) && (hdnparent.Value.ToString() == "1" || hdnparent.Value.ToString().ToLower() == "true"))
                {
                    isparent = 1;
                    ltparentimage.Text = "<img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\" border=\"0\" />";
                    chkparent.Checked = true;
                }
                else
                {
                    isparent = 0;
                    chkparent.Checked = false;
                    ltparentimage.Text = "<img src=\"/App_Themes/" + Page.Theme.ToString() + "/images/isInactive.png\" border=\"0\" />";
                }
                if (dsListing != null && dsListing.Tables.Count > 0 && dsListing.Tables[0].Rows.Count > 0)
                {
                    ViewState["grdvaluelisting"] = dsListing.Tables[0].Rows.Count.ToString();
                    grdvaluelisting.DataSource = dsListing;
                    grdvaluelisting.DataBind();
                }
                else
                {
                    ViewState["grdvaluelisting"] = "0";
                    grdvaluelisting.DataSource = null;
                    grdvaluelisting.DataBind();
                }
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                ((ImageButton)e.Row.FindControl("imgEdit")).Visible = true;
            }
        }
        protected void grdoptionmainGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Save")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    //Int32 Projectid = Convert.ToInt32(Request.QueryString["id"].ToString());
                    Int32 Varaintid = Convert.ToInt32(e.CommandArgument.ToString());

                    string strGroupName = ((TextBox)grrow.FindControl("txttitle")).Text;
                    bool chkparent = ((CheckBox)grrow.FindControl("chkparent")).Checked;
                    string displayorder = ((TextBox)grrow.FindControl("txtdisplayorder")).Text;



                    if (displayorder.Trim() == "")
                    {
                        displayorder = "0";
                    }
                    SQLAccess objSql = new SQLAccess();
                    Int32 CountVariant = 0;
                    CountVariant = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(*) FROM tb_ProductVariant  WHERE VariantName='" + strGroupName.Replace("'", "''") + "' AND ProductId=" + Request.QueryString["id"].ToString() + " AND VariantID <> " + Varaintid + ""));
                    if (CountVariant > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Name Already Exists!','Message','" + ((TextBox)gvTemp.FooterRow.FindControl("txttitle")).ClientID.ToString() + "');", true);
                        return;
                    }

                    Int32 Id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariant SET VariantName='" + strGroupName.Replace("'", "''").Trim() + "',DisplayOrder=" + displayorder + ",isparent=" + Convert.ToInt32(chkparent) + " WHERE VariantID = " + Varaintid + ""));


                }
                else if (e.CommandName == "Remove")
                {
                    GridView gvTemp = (GridView)sender;
                    Int32 VariantId = Convert.ToInt32(e.CommandArgument.ToString());

                    SQLAccess objSql = new SQLAccess();

                    objSql.ExecuteNonQuery("DELETE FROM tb_ProductVariantValue WHERE VariantID in (SELECT VariantID FROM tb_ProductVariant WHERE ParentId in (SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantID=" + VariantId + ")) DELETE FROM  tb_ProductVariant WHERE ParentId in (SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantID=" + VariantId + ") DELETE FROM tb_ProductVariantValue WHERE VariantID=" + VariantId + "  DELETE FROM tb_ProductVariant WHERE VariantID=" + VariantId + "");




                }
                else if (e.CommandName == "Exit")
                {

                }
                FillGrid();
            }
            catch { }

        }
        protected void grdoptionmainGroup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ImageButton imgSave = (ImageButton)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("imgSave");
            ImageButton imgcancel = (ImageButton)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("imgcancel");

            ImageButton imgEdit = (ImageButton)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("imgEdit");
            ImageButton imgDelete = (ImageButton)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("imgDelete");
            Literal lttitle = (Literal)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("lttitle");
            TextBox txttitle = (TextBox)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("txttitle");

            Literal ltdisplayorder = (Literal)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("ltdisplayorder");
            Literal ltparentimage = (Literal)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("ltparentimage");
            CheckBox chkparent = (CheckBox)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("chkparent");
            TextBox txtdisplayorder = (TextBox)grdoptionmainGroup.Rows[e.NewEditIndex].FindControl("txtdisplayorder");

            string strScript = "javascript:if(document.getElementById('" + txttitle.ClientID + "').value == ''){jAlert('Please Enter Name.','Message','" + txttitle.ClientID + "'); return false;}";
            imgSave.OnClientClick = strScript;
            lttitle.Visible = false;
            txttitle.Visible = true;
            imgEdit.Visible = false;
            imgDelete.Visible = true;
            imgSave.Visible = true;
            imgcancel.Visible = true;
            ltparentimage.Visible = false;
            chkparent.Visible = true;
            txtdisplayorder.Visible = true;
            ltdisplayorder.Visible = false;
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "gridcollaspe", "expandcollapse('" + hdnrowid.Value.ToString() + "','one');", true);
        }

        /// <summary>
        /// Variant Value Listing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdvaluelisting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //GridView gvChkListMaster = (GridView)sender;
            //string strchecklistID = ((HtmlInputHidden)gvChkListMaster.Rows[e.RowIndex].FindControl("hdnchecklistID")).Value;
            //ChecklistMasterComponent objSql = new ChecklistMasterComponent();
            //Int32 Id = Convert.ToInt32(objSql.AddOrUpdateOrDeleteCheckListProject(0, Convert.ToInt32(strchecklistID), "", "", 0, DateTime.Now.ToString(), Convert.ToInt32(Session["AdminId"].ToString()), 3));
            //LoadleftControl();
            //if (Id < 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Problem While Deleting','Message','" + ((TextBox)gvChkListMaster.FooterRow.FindControl("txtTitle")).ClientID.ToString() + "');expandcollapse('" + hdnrowid.Value.ToString() + "','one');", true);
            //    return;
            //}
            //if (Request.QueryString["ProjectGroupId"] != null)
            //{
            //    GetAllGroup();
            //}
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "gridcollaspe", "expandcollapse('" + hdnrowid.Value.ToString() + "','one');", true);
        }
        protected void grdvaluelisting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gvChkListMaster = (GridView)sender;
            // GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
            ImageButton imgSave = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgSave");
            ImageButton imgEdit = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgEdit");
            ImageButton imgDelete = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgDelete");
            ImageButton imgcancel = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgcancel");

            Label lttitle = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("lttitle");
            TextBox txttitle = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txttitle");


            Literal ltsku = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltsku");
            TextBox txtsku = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtsku");

            Literal ltupc = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltupc");
            TextBox txtupc = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtupc");

            Literal ltheader = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltheader");
            TextBox txtheader = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtheader");

            Literal ltprice = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltprice");
            TextBox txtprice = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtprice");

            Literal ltdisplayorder = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltdisplayorder");
            TextBox txtdisplayorder = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtdisplayorder");

            Label ltinventory = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltinventory");
            TextBox txtinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtinventory");

            Label ltAllowInventory = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltAllowInventory");
            TextBox txtAllowinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtAllowinventory");

            Literal ltLockInventory = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltLockInventory");
            TextBox txtLockinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtLockinventory");

            Literal ltAdditionalHemingQty = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltAdditionalHemingQty");
            TextBox txtAdditionalHemingQty = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtAdditionalHemingQty");

            Literal ltBasecustomPrice = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltBasecustomPrice");
            TextBox txtBasecustomPrice = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtBasecustomPrice");



            Literal ltweight = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltweight");
            TextBox txtweightparent = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtweightparent");


            System.Web.UI.HtmlControls.HtmlInputHidden arelatedsku1 = (System.Web.UI.HtmlControls.HtmlInputHidden)gvChkListMaster.Rows[e.NewEditIndex].FindControl("hdnrelatedsku");
            hdnrelatedsku.Value = arelatedsku1.Value.ToString().Trim();

            //string strScript = "javascript:if(document.getElementById('" + txttitle.ClientID + "').value == ''){jAlert('Please Enter Name.','Message','" + txttitle.ClientID + "'); return false;} chkHeight();";
            imgSave.OnClientClick = "return CheckValidation('" + txttitle.ClientID.ToString() + "','Please enter option value.');";
            //imgSave.OnClientClick = strScript;
            lttitle.Visible = false;
            txttitle.Visible = true;
            imgEdit.Visible = false;
            imgDelete.Visible = true;
            imgSave.Visible = true;
            imgcancel.Visible = true;

            ltsku.Visible = false;
            txtsku.Visible = true;

            ltupc.Visible = false;
            txtupc.Visible = true;

            ltheader.Visible = false;
            txtheader.Visible = true;

            ltinventory.Visible = true;
            //txtinventory.Visible = false;

            ltAllowInventory.Visible = false;
            txtAllowinventory.Visible = true;
            txtAllowinventory.Attributes.Add("readonly", "true");

            ltLockInventory.Visible = false;
            txtLockinventory.Visible = true;

            ltdisplayorder.Visible = false;
            txtdisplayorder.Visible = true;

            ltprice.Visible = false;
            txtprice.Visible = true;

            ltBasecustomPrice.Visible = false;
            txtBasecustomPrice.Visible = true;

            ltAdditionalHemingQty.Visible = false;
            txtAdditionalHemingQty.Visible = true;

            ltweight.Visible = false;
            txtweightparent.Visible = true;

            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "gridcollaspe", "expandcollapse('" + hdnrowid.Value.ToString() + "','one');", true);
        }
        protected void grdvaluelisting_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Attributes.Add("style", "display:none;");

                if (pcount > 0)
                {
                    e.Row.Cells[1].Text = "Fabric&nbsp;Per&nbsp;Yard&nbsp;Cost($)";
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");

                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("style", "border-bottom: solid 1px #eee !important;");

                e.Row.Cells[5].Attributes.Add("style", "display:none;");
                if (pcount > 0)
                {
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");

                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");
                if (((DataRowView)e.Row.DataItem)["VariantValueID"].ToString() == "0") e.Row.Visible = false;
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                ((ImageButton)e.Row.FindControl("imgEdit")).Visible = true;

                GridView grdnamevaluelisting = (GridView)e.Row.FindControl("grdnamevaluelisting");
                System.Web.UI.HtmlControls.HtmlTableRow trsuboption = (System.Web.UI.HtmlControls.HtmlTableRow)e.Row.FindControl("trsuboption");
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedsku");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnrelatedskugrid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnrelatedsku");

                Label lttitle = (Label)e.Row.FindControl("lttitle");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantValueID");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedbuy1get1 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedbuy1get1");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedonsale = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedonsale");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnBuy1Get1 = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnBuy1Get1");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnonsale = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnonsale");

                System.Web.UI.HtmlControls.HtmlImage imgBuy1get1 = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgBuy1get1");
                System.Web.UI.HtmlControls.HtmlImage imgonsale = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgonsale");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedColor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedColor");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedfabric = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedfabric");
                if (!string.IsNullOrEmpty(lttitle.Text) && Convert.ToInt32(hdnVariantValueID.Value.ToString()) > 0)
                {
                    arelatedColor.Attributes.Add("onclick", "openCenteredCrossSaleWindowEdit(this.id,'','" + hdnVariantValueID.Value.ToString() + "');");
                    arelatedfabric.Attributes.Add("onclick", "openCenteredCrossSaleWindowEdit(this.id,'','" + hdnVariantValueID.Value.ToString() + "');");
                }
                System.Web.UI.HtmlControls.HtmlInputHidden hdnactive = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnactive");
                CheckBox Active = (CheckBox)e.Row.FindControl("chkactive");
                if (hdnactive.Value.ToString() == "1" || hdnactive.Value.ToString().ToLower() == "true")
                {
                    Active.Checked = true;

                }
                Active.Attributes.Add("onchange", "updateactiveoption('" + Active.ClientID.ToString() + "');");
                //if (!string.IsNullOrEmpty(hdnBuy1Get1.Value) && hdnBuy1Get1.Value.ToString() == "1")
                //{
                //    imgBuy1get1.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                //    imgBuy1get1.Attributes.Add("style", "display:''");
                //}
                //else { imgBuy1get1.Attributes.Add("style", "display:none"); }

                System.Web.UI.HtmlControls.HtmlInputHidden hdnupdatebuy1 = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnupdatebuy1");
                if (!string.IsNullOrEmpty(hdnBuy1Get1.Value) && (hdnBuy1Get1.Value.ToString() == "1" || hdnBuy1Get1.Value.ToString().ToLower().Trim() == "true"))
                {
                    if (!string.IsNullOrEmpty(hdnupdatebuy1.Value) && hdnupdatebuy1.Value.ToString() == "1")
                    {
                        imgBuy1get1.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                        imgBuy1get1.Attributes.Add("style", "display:''");
                    }
                    else
                    {
                        imgBuy1get1.Attributes.Add("style", "display:none");
                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0 where VariantValueID=" + Convert.ToInt32(hdnVariantValueID.Value.ToString()) + "");
                    }
                }
                else { imgBuy1get1.Attributes.Add("style", "display:none"); }


                if (!string.IsNullOrEmpty(hdnonsale.Value) && hdnonsale.Value.ToString() == "1")
                {
                    imgonsale.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                    imgonsale.Attributes.Add("style", "display:''");
                }
                else { imgonsale.Attributes.Add("style", "display:none"); }

                arelatedbuy1get1.Attributes.Add("onclick", "openCenteredCrossSaleWindowOnsaleBuy1('" + arelatedbuy1get1.ClientID.ToString() + "','" + hdnVariantValueID.Value.ToString() + "');");
                arelatedonsale.Attributes.Add("onclick", "openCenteredCrossSaleWindowOnsaleBuy1('" + arelatedonsale.ClientID.ToString() + "','" + hdnVariantValueID.Value.ToString() + "');");
                Label ltInventory = (Label)e.Row.FindControl("ltInventory");
                DataSet dswaregouse = new DataSet();
                dswaregouse = CommonComponent.GetCommonDataSet("SELECT  tb_WareHouse.Code,isnull(tb_WareHouseProductVariantInventory.inventory,0) as inventory FROM dbo.tb_WareHouse INNER JOIN dbo.tb_WareHouseProductVariantInventory ON dbo.tb_WareHouse.WareHouseID = dbo.tb_WareHouseProductVariantInventory.WareHouseID WHERE tb_WareHouseProductVariantInventory.ProductID=" + Request.QueryString["ID"].ToString() + " and tb_WareHouseProductVariantInventory.VariantValueID=" + hdnVariantValueID.Value.ToString() + " ");
                string str = "<br/>";
                if (dswaregouse != null && dswaregouse.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dswaregouse.Tables[0].Rows.Count; i++)
                    {
                        str += dswaregouse.Tables[0].Rows[i]["Code"].ToString() + " :" + dswaregouse.Tables[0].Rows[i]["inventory"].ToString() + "&nbsp;";
                    }
                }
                ltInventory.Text += str;
                Int32 isParentID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT COUNT(ParentId) FROM dbo.tb_ProductVariant WHERE ParentId=" + DataBinder.Eval(e.Row.DataItem, "VariantValueID").ToString() + " AND ProductID=" + DataBinder.Eval(e.Row.DataItem, "ProductID").ToString()));
                System.Web.UI.HtmlControls.HtmlAnchor lnkEditInventory = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lnkEditInventory");

                if (isParentID == 0)
                {
                    lnkEditInventory.Attributes.Add("onclick", "ShowModelUserRegisterPopup(" + DataBinder.Eval(e.Row.DataItem, "ProductID").ToString() + "," + e.Row.DataItemIndex.ToString() + "," + DataBinder.Eval(e.Row.DataItem, "VariantID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "VariantValueID").ToString() + ",'" + lnkEditInventory.ClientID.ToString() + "');");
                }
                else
                {
                    lnkEditInventory.Attributes.Add("Style", "cursor:default;text-decoration:none;color: #fff;");
                }

                if (hdnrelatedskugrid.Value.Trim().ToLower() == "select sku")
                {
                    objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                }
                else
                {
                    if (hdnrelatedskugrid.Value.Trim() == "")
                    {
                        objAnchor.InnerHtml = "select sku";
                        objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                    }
                    else
                    {
                        objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','" + hdnrelatedskugrid.Value.ToString() + "');");
                    }
                }
                if (hdnrelatedskugrid.Value.Trim() == "0")
                {
                    objAnchor.InnerHtml = "select sku";
                    objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                }
                hdnrelatedsku.Value = hdnrelatedskugrid.Value.ToString();
                Int32 Variantid = Convert.ToInt32(((DataRowView)e.Row.DataItem)["VariantValueID"].ToString());
                DataSet dsListing = new DataSet();

                Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
                dsListing = objSql.GetDs("select 1 as VarActive," + Variantid + " AS VariantValueID,0 as VariantID,'' AS VariantValue,0 AS VariantPrice,0 AS DisplayOrder,0 AS ProductID,'' AS SKU,'' AS UPC,'' AS Header,0 as Inventory,'' as selectSKU,0 as AllowQuantity, 0 as LockQuantity,0 as AddiHemingQty,0 as BasecustomPrice,0 as Buy1Get1 ,0 as OnSale,'' as BackOrderdate,0 as Weight,0 as updatebuy1  UNION SELECT isnull(VarActive,0) as VarActive,VariantValueID,VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID, SKU, UPC, Header,Inventory,isnull(RelatedProductid,'Select SKU') as selectSKU,isnull(AllowQuantity,0) as AllowQuantity,isnull(LockQuantity,0) as LockQuantity,ISNULL(AddiHemingQty,0) as AddiHemingQty,ISNULL(BasecustomPrice,0) as BasecustomPrice,ISNULL(Buy1Get1,0) as Buy1Get1,ISNULL(OnSale,0) as OnSale, Cast(BackOrderdate as Date) as BackOrderdate,isnull(Weight,0) as Weight,case when cast(Buy1Todate as date) >= cast(getdate() as date) then 1 else 0 end as updatebuy1 FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["ID"].ToString() + " AND VariantID in (SELECT VariantID FROM tb_ProductVariant WHERE parentid=" + Variantid + ") Order By DisplayOrder");
                ViewState["varaintidname"] = Variantid;
                if (dsListing != null && dsListing.Tables.Count > 0 && dsListing.Tables[0].Rows.Count > 0)
                {
                    ViewState["grdnamevaluelisting"] = dsListing.Tables[0].Rows.Count.ToString();
                    grdnamevaluelisting.DataSource = dsListing;
                    grdnamevaluelisting.DataBind();
                }
                else
                {
                    ViewState["grdnamevaluelisting"] = "0";
                    grdnamevaluelisting.DataSource = null;
                    grdnamevaluelisting.DataBind();
                }
                if (isparent == 1)
                {
                    grdnamevaluelisting.Visible = true;
                    trsuboption.Visible = true;
                }
                else
                {
                    trsuboption.Visible = false;
                    grdnamevaluelisting.Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtTitle = (TextBox)e.Row.FindControl("txtTitle");
                LinkButton lnkAdd = (LinkButton)e.Row.FindControl("lnkAdd");
                TextBox txtAllowInventory = (TextBox)e.Row.FindControl("txtAllowInventory");
                txtAllowInventory.Attributes.Add("readonly", "true");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantIDSub = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantIDSub");
                lnkAdd.OnClientClick = "return CheckValidation('" + txtTitle.ClientID.ToString() + "','Please enter option value.');";
                hdnVariantIDSub.Value = ViewState["varaintid"].ToString();
                if (ViewState["grdvaluelisting"] != null && Convert.ToInt32(ViewState["grdvaluelisting"].ToString()) == 1)
                {
                    e.Row.Cells[0].Attributes.Add("style", "border-right:none !important;");
                }
                e.Row.Visible = true;

                e.Row.Cells[5].Attributes.Add("style", "display:none;");
                if (pcount > 0)
                {
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");

                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");

            }
        }

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName, string StrTempImgName)
        {
            SearchProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/");
            SearchProductPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(SearchProductPath));
            if (!string.IsNullOrEmpty(FileName.ToString()))
            {
                try
                {

                    CreateImage("icon", FileName, StrTempImgName);
                    if (!Directory.Exists(Server.MapPath(SearchProductPath + "/large")))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath + "/large"));
                    try
                    {
                        if (File.Exists(Server.MapPath(SearchProductPath + "/large/" + FileName)))
                        {
                            File.Delete(Server.MapPath(SearchProductPath + "/large/" + FileName));
                            CommonOperations.DeleteFileFromContentServer(Server.MapPath(SearchProductPath + "/large/" + FileName));
                        }
                    }
                    catch
                    {

                    }

                    File.Copy(Server.MapPath(SearchProductTempPath + StrTempImgName), Server.MapPath(SearchProductPath + "/large/" + FileName), true);
                    CommonOperations.SaveOnContentServer(Server.MapPath(SearchProductPath + "/large/" + FileName));
                }
                catch (Exception ex)
                {
                }
                finally
                {

                }
            }
        }
        /// <summary>
        ///  Create Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName, string StrTempImgName)
        {
            try
            {
                string strFile = null;
                String strPath = "";
                strPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/") + StrTempImgName.ToString();
                strFile = Server.MapPath(strPath);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "icon":
                        strFilePath = Server.MapPath(SearchProductPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(SearchProductPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                }
                ResizePhoto(strFile, Size, strFilePath);
            }
            catch (Exception ex)
            {
                if (ex.Source == "System.Drawing")
                    lblMsg.Text = "<br />Error Saving " + Size + " Image..Please check that Directory exists..";
                else
                    lblMsg.Text += "<br />" + ex.Message;
            }

        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
            }
            catch (Exception ex)
            {
                lblMsg.Text += "<br />" + ex.Message;
            }
        }

        /// <summary>
        /// Resize Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            switch (Size)
            {
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
            }
            ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        private void BindSize()
        {
            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(77), Convert.ToInt32(77));
            }
        }

        /// <summary>
        /// Resize Images
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="FinWidth">int FinWidth</param>
        /// <param name="FinHeight">int FinHeight</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizeImage(string strFile, int FinWidth, int FinHeight, string strFilePath)
        {
            System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgVisol.Height;
            int resizedWidth = imgVisol.Width;

            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = FinHeight;
                    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = FinWidth;
                    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                }
            }
            else if (imgVisol.Width >= FinWidth && imgVisol.Height <= FinHeight)
            {
                resizedWidth = FinWidth;
                resizePercent = (FinWidth * 100) / imgVisol.Width;
                resizedHeight = (int)Math.Round((imgVisol.Height * resizePercent) / 100.0);
            }

            else if (imgVisol.Width <= FinWidth && imgVisol.Height >= FinHeight)
            {
                resizePercent = (FinHeight * 100) / imgVisol.Height;
                resizedHeight = FinHeight;
                resizedWidth = (int)Math.Round(resizePercent * imgVisol.Width / 100.0);
            }

            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);

            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgVisol.Width;
            int sourceHeight = imgVisol.Height;

            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(0, 0, sourceWidth, sourceHeight);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);
            GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);
            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();
        }

        /// <summary>
        /// Generate Image
        /// </summary>
        /// <param name="extBMP">Bitmap extBMP</param>
        /// <param name="DestFileName">string DestFileName</param>
        /// <param name="DefWidth">int DefWidth</param>
        /// <param name="DefHeight">int DefHeight</param>
        private void GenerateImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            Encoder Enc = Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(Encoder.Quality, (long)600);

            if (extBMP != null && extBMP.Width < (DefWidth) && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, startX, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null && extBMP.Width < (DefWidth))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, startX, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, 0, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);

            }
        }

        /// <summary>
        /// Get Encoder Information
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo Object</returns>
        private static ImageCodecInfo GetEncoderInfo(string resizeMimeType)
        {
            // Get image code for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
        }

        protected void grdvaluelisting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    GridView gvTemp = (GridView)sender;

                    Int32 Variantid = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnVariantIDSub")).Value.ToString()); //Convert.ToInt32(grdoptionmainGroup.DataKeys[0].Value.ToString());
                    string strvaluename = ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).Text;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    strvaluename = regex.Replace(strvaluename, " ");
                    strvaluename = strvaluename.Trim();
                    SQLAccess objSql = new SQLAccess();
                    Int32 CountVariant = 0;
                    CountVariant = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(*) FROM tb_ProductVariantValue WHERE VariantValue='" + strvaluename.Replace("'", "''").Trim() + "' AND ProductId=" + Request.QueryString["id"].ToString() + " AND VariantID=" + Variantid + ""));
                    if (CountVariant > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Name Already Exists!','Message','" + ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).ClientID.ToString() + "');", true);
                        return;
                    }
                    string strstrvaluesku = ((TextBox)gvTemp.FooterRow.FindControl("txtSku")).Text;
                    string hdnProductColorNewImg = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnProductColorNewImg")).Value.ToString());
                    string strstrvalueupc = ((TextBox)gvTemp.FooterRow.FindControl("txtUpc")).Text;
                    string strstrvalueheader = ((TextBox)gvTemp.FooterRow.FindControl("txtHeader")).Text;
                    string strInventory = ((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnmainqty")).Value;
                    string strLockInventory = ((TextBox)gvTemp.FooterRow.FindControl("txtLockinventory")).Text;
                    string strAdditionalHemingQty = ((TextBox)gvTemp.FooterRow.FindControl("txtAdditionalHemingQty")).Text;
                    string Fabrictype = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnProductfabrictype")).Value.ToString());
                    string FabricCode = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnProductfabriccode")).Value.ToString());

                    string strbackorderdateparent = ((TextBox)gvTemp.FooterRow.FindControl("txtbackorderdateparent")).Text;
                    if (string.IsNullOrEmpty(strbackorderdateparent))
                    {
                        strbackorderdateparent = null;
                    }
                    else
                    {
                        strbackorderdateparent = strbackorderdateparent.Replace("'", "''");
                    }

                    decimal weight = 0;
                    decimal.TryParse(((TextBox)gvTemp.FooterRow.FindControl("txtweightparent")).Text, out weight);


                    if (strInventory == "")
                    {
                        strInventory = "0";
                    }
                    if (strLockInventory == "")
                    {
                        strLockInventory = "0";
                    }
                    if (strAdditionalHemingQty == "")
                    {
                        strAdditionalHemingQty = "0";
                    }

                    string strAllowInventory = ((TextBox)gvTemp.FooterRow.FindControl("txtAllowinventory")).Text;
                    if (strAllowInventory == "")
                    {
                        strAllowInventory = "0";
                    }
                    string strprice = ((TextBox)gvTemp.FooterRow.FindControl("txtPrice")).Text;
                    string strDisplayorder = ((TextBox)gvTemp.FooterRow.FindControl("txtDisplayorder")).Text;
                    string relatedSKu = hdnrelatedsku.Value.ToString().Trim();
                    string StrBasecustomPrice = ((TextBox)gvTemp.FooterRow.FindControl("txtBasecustomPrice")).Text;

                    if (strInventory.Trim() == "")
                        strInventory = "0";
                    if (strprice.Trim() == "")
                        strprice = "0";
                    if (strDisplayorder.Trim() == "")
                        strDisplayorder = "0";
                    if (StrBasecustomPrice.Trim() == "")
                        StrBasecustomPrice = "0";

                    string StrVariPath = "";
                    if (!string.IsNullOrEmpty(hdnrelatedcolor.Value.Trim()))
                    {
                        string Strcolor = Convert.ToString(hdnrelatedcolor.Value.Trim());
                        StrVariPath = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where lower(ColorName)='" + Strcolor.ToString().ToLower() + "'"));
                        if (!string.IsNullOrEmpty(hdnProductColorNewImg))
                        {
                            if (!string.IsNullOrEmpty(hdnProductColorNewImg.ToString()) || hdnProductColorNewImg.ToString().IndexOf("/temp/") > -1)
                            {
                                StrVariPath = Convert.ToString(hdnProductColorNewImg);
                            }
                            //else
                            //{
                            //    StrVariPath = hdnProductColorNewImg.ToString();
                            //}
                        }
                    }
                    CheckBox Active = (CheckBox)gvTemp.FooterRow.FindControl("chkactive");

                    Int32 iSActive = 0;
                    if (Active.Checked)
                    {
                        iSActive = 1;
                    }


                    Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue(VarActive,VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID,SKU,UPC,Header,RelatedProductid,AllowQuantity,LockQuantity,VariImageName,AddiHemingQty,BasecustomPrice,FabricType,FabricCode,BackOrderdate,weight) VALUES (" + iSActive + "," + Variantid + ",'" + strvaluename.Replace("'", "''").Trim() + "'," + strprice + "," + strDisplayorder + "," + Request.QueryString["id"].ToString() + ",'" + strstrvaluesku.Replace("'", "''") + "','" + strstrvalueupc.Replace("'", "''") + "','" + strstrvalueheader.Replace("'", "''") + "','" + relatedSKu.Trim().Replace("'", "''").Replace("Select SKU", "") + "'," + strAllowInventory + "," + strLockInventory + ",'" + StrVariPath + "'," + strAdditionalHemingQty + "," + StrBasecustomPrice + ",'" + Fabrictype.Replace("'", "''") + "','" + FabricCode.Replace("'", "''") + "','" + strbackorderdateparent + "'," + weight + ") SELECT SCOPE_IDENTITY();"));
                    if (string.IsNullOrEmpty(strbackorderdateparent))
                    {
                        objSql.ExecuteNonQuery("update tb_ProductVariantValue SET BackOrderdate= NULL WHERE VariantValueID=" + id + "");
                    }
                    string strImageName = "";
                    if (id > 0 && !string.IsNullOrEmpty(hdnrelatedcolor.Value.Trim()) && !string.IsNullOrEmpty(hdnProductColorNewImg.ToString()))
                    {

                        if (!string.IsNullOrEmpty(hdnProductColorNewImg.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(strvaluename.ToCharArray()) + "_" + Request.QueryString["id"].ToString() + "_" + id.ToString() + ".jpg";
                            string path = string.Empty;
                            path = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/") + hdnProductColorNewImg.ToString());
                            if (File.Exists(path))
                            {
                                SaveImage(strImageName, hdnProductColorNewImg.ToString());
                                try
                                {
                                    File.Delete(path);
                                }
                                catch { }
                            }
                        }
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VariImageName='" + strImageName.ToString().Replace("'", "''") + "' Where VariantValueID=" + id.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + "");
                    }
                    hdnrelatedcolor.Value = "";
                    ((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnProductColorNewImg")).Value = "";
                    foreach (GridViewRow gr in gvTemp.Rows)
                    {

                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnAllowInventory");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnVariantValueID");
                        //  id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                        id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    }

                    FillGrid();
                    hdnrelatedsku.Value = "select sku";

                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIn(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }




                }
                else if (e.CommandName == "updateactive")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 Variantvalueid = Convert.ToInt32(e.CommandArgument.ToString());
                    CheckBox Active = (CheckBox)grrow.FindControl("chkactive");
                    Int32 isActive = 0;
                    if (Active.Checked)
                    {
                        isActive = 1;
                    }
                    CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VarActive='" + isActive + "' Where VariantValueID=" + Variantvalueid.ToString() + "");
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }


                }
                else if (e.CommandName == "Save")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 Variantvalueid = Convert.ToInt32(e.CommandArgument.ToString());
                    TextBox txttitle = (TextBox)grrow.FindControl("txttitle");

                    string strvaluename = ((TextBox)grrow.FindControl("txttitle")).Text;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    strvaluename = regex.Replace(strvaluename, " ");
                    strvaluename = strvaluename.Trim();
                    SQLAccess objSql = new SQLAccess();
                    Int32 CountVariant = 0;
                    CountVariant = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(*) FROM tb_ProductVariantValue WHERE VariantValue='" + strvaluename.Replace("'", "''").Trim() + "' AND ProductId=" + Request.QueryString["id"].ToString() + " AND VariantValueID <> " + Variantvalueid + ""));
                    if (CountVariant > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Name Already Exists!','Message','" + ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).ClientID.ToString() + "');", true);
                        return;
                    }

                    string hdnProductColorNewImg = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnProductColorNewImg")).Value.ToString());
                    string strstrvaluesku = ((TextBox)grrow.FindControl("txtsku")).Text;
                    string strstrvalueupc = ((TextBox)grrow.FindControl("txtupc")).Text;
                    string strstrvalueheader = ((TextBox)grrow.FindControl("txtheader")).Text;
                    string strInventory = ((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnmainqty")).Value;
                    string strLockInventory = ((TextBox)grrow.FindControl("txtLockinventory")).Text;
                    string strAllowInventory = ((TextBox)grrow.FindControl("txtAllowinventory")).Text;
                    string strbackorderdateparent = ((TextBox)grrow.FindControl("txtbackorderdateparent")).Text;



                    decimal weight = 0;
                    decimal.TryParse(((TextBox)grrow.FindControl("txtweightparent")).Text, out weight);
                    CheckBox Active = (CheckBox)grrow.FindControl("chkactive");

                    Int32 iSActive = 0;
                    if (Active.Checked)
                    {
                        iSActive = 1;
                    }

                    string strAdditionalHemingQty = ((TextBox)grrow.FindControl("txtAdditionalHemingQty")).Text;
                    string StrBasecustomPrice = ((TextBox)grrow.FindControl("txtBasecustomPrice")).Text;
                    string Fabrictype = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnProductfabrictype")).Value.ToString());
                    string FabricCode = Convert.ToString(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnProductfabriccode")).Value.ToString());
                    if (strInventory == "")
                    {
                        strInventory = "0";
                    }
                    if (strLockInventory == "")
                    {
                        strLockInventory = "0";
                    }

                    if (strAllowInventory == "")
                    {
                        strAllowInventory = "0";
                    }

                    if (strAdditionalHemingQty == "")
                        strAdditionalHemingQty = "0";
                    if (StrBasecustomPrice == "")
                        StrBasecustomPrice = "0";

                    string strprice = ((TextBox)grrow.FindControl("txtprice")).Text;
                    string strDisplayorder = ((TextBox)grrow.FindControl("txtdisplayorder")).Text;
                    string relatedSKu = hdnrelatedsku.Value.ToString().Trim();
                    if (strInventory.Trim() == "")
                        strInventory = "0";
                    if (strprice.Trim() == "")
                        strprice = "0";
                    if (strDisplayorder.Trim() == "")
                        strDisplayorder = "0";
                    string StrVariPath = "";

                    if (string.IsNullOrEmpty(strbackorderdateparent))
                    {
                        strbackorderdateparent = null;
                    }
                    else
                    {
                        strbackorderdateparent = strbackorderdateparent.Replace("'", "''");
                    }

                    if (!string.IsNullOrEmpty(hdnrelatedcolor.Value.Trim()))
                    {
                        string Strcolor = Convert.ToString(hdnrelatedcolor.Value.Trim());
                        StrVariPath = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where lower(ColorName)='" + Strcolor.ToString().ToLower() + "'"));
                        if (!string.IsNullOrEmpty(hdnProductColorNewImg))
                        {
                            if (!string.IsNullOrEmpty(hdnProductColorNewImg.ToString()) || hdnProductColorNewImg.ToString().IndexOf("/temp/") > -1)
                            {
                                StrVariPath = Convert.ToString(hdnProductColorNewImg);
                            }
                        }

                        string strImageName = "";
                        if (!string.IsNullOrEmpty(hdnrelatedcolor.Value.Trim()) && !string.IsNullOrEmpty(hdnProductColorNewImg.ToString()))
                        {
                            if (!string.IsNullOrEmpty(hdnProductColorNewImg.ToString().Trim()))
                            {
                                strImageName = RemoveSpecialCharacter(strvaluename.ToCharArray()) + "_" + Request.QueryString["id"].ToString() + "_" + Variantvalueid.ToString() + ".jpg";
                                string path = string.Empty;
                                path = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/") + hdnProductColorNewImg.ToString());
                                if (File.Exists(path))
                                {
                                    SaveImage(strImageName, hdnProductColorNewImg.ToString());
                                    try
                                    {
                                        File.Delete(path);
                                    }
                                    catch { }
                                }
                            }
                            CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VarActive=" + iSActive + ", VariImageName='" + strImageName.ToString().Replace("'", "''") + "' Where VariantValueID=" + Variantvalueid.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + "");
                        }
                        else { CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VarActive=" + iSActive + ", VariImageName='" + StrVariPath.ToString().Replace("'", "''") + "' Where VariantValueID=" + Variantvalueid.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""); }

                    }

                    //  Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET weight =" + weight + ", BackOrderdate ='" + strbackorderdateparent + "', FabricCode='" + FabricCode.Replace("'", "''") + "',FabricType='" + Fabrictype.Replace("'", "''") + "', BasecustomPrice=" + StrBasecustomPrice.ToString() + ",AddiHemingQty=" + strAdditionalHemingQty.ToString() + ", LockQuantity=" + strLockInventory.ToString() + ",AllowQuantity=" + strAllowInventory.ToString() + ", RelatedProductid='" + relatedSKu.Replace("'", "''").Trim().Replace("'", "''").Replace("Select SKU", "") + "', VariantValue='" + strvaluename.Replace("'", "''").Trim() + "',VariantPrice='" + strprice.Replace("'", "''") + "',DisplayOrder=" + strDisplayorder + ",SKU='" + strstrvaluesku.Replace("'", "''") + "',UPC='" + strstrvalueupc.Replace("'", "''") + "',Header='" + strstrvalueheader.Replace("'", "''") + "' WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET weight =" + weight + ", BackOrderdate ='" + strbackorderdateparent + "', FabricCode='" + FabricCode.Replace("'", "''") + "',FabricType='" + Fabrictype.Replace("'", "''") + "', BasecustomPrice=" + StrBasecustomPrice.ToString() + ",AddiHemingQty=" + strAdditionalHemingQty.ToString() + ", LockQuantity=" + strLockInventory.ToString() + ",AllowQuantity=0, RelatedProductid='" + relatedSKu.Replace("'", "''").Trim().Replace("'", "''").Replace("Select SKU", "") + "', VariantValue='" + strvaluename.Replace("'", "''").Trim() + "',VariantPrice='" + strprice.Replace("'", "''") + "',DisplayOrder=" + strDisplayorder + ",SKU='" + strstrvaluesku.Replace("'", "''") + "',UPC='" + strstrvalueupc.Replace("'", "''") + "',Header='" + strstrvalueheader.Replace("'", "''") + "' WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    if (string.IsNullOrEmpty(strbackorderdateparent))
                    {
                        objSql.ExecuteNonQuery("update tb_ProductVariantValue SET BackOrderdate= NULL WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + "");
                    }

                    hdnrelatedcolor.Value = "";

                    ((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnProductColorNewImg")).Value = "";
                    foreach (GridViewRow gr in gvTemp.Rows)
                    {
                        TextBox txttitlenew = (TextBox)gr.FindControl("txttitle");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnAllowInventory");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnVariantValueID");

                        if (txttitlenew.ClientID.ToString().ToLower() != txttitle.ClientID.ToString().ToLower())
                        {
                            //id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET  AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                            id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET  AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                        }
                    }

                    FillGrid();
                    hdnrelatedsku.Value = "select sku";
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }

                }
                else if (e.CommandName == "Remove")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 VariantvalueId = Convert.ToInt32(e.CommandArgument.ToString());
                    Int32 VariantID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnVariantID")).Value);

                    SQLAccess objSql = new SQLAccess();
                    objSql.ExecuteNonQuery("DELETE FROM tb_ProductVariantValue WHERE VariantID in (SELECT VariantID FROM tb_ProductVariant WHERE ParentId in (" + VariantvalueId + ")) DELETE FROM  tb_ProductVariant WHERE ParentId in (" + VariantvalueId + ") DELETE FROM tb_ProductVariantValue WHERE VariantvalueId=" + VariantvalueId + "; EXEC dbo.usp_InsertUpdateVariantWarehouse @ProductID = " + Convert.ToInt32(Request.QueryString["ID"]) + ",@VariantID = " + VariantID + ", @VariantValueID = " + VariantvalueId + ", @CreatedBy = " + Convert.ToInt32(Session["AdminID"]) + ",@UpdatedBy = " + Convert.ToInt32(Session["AdminID"]) + ",@Mode =" + 3);
                    FillGrid();

                    string strIid = gvTemp.ClientID.ToString() + "_txtLockinventory";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlert", "javascript:document.getElementById('" + strIid + "').value='0'; AllowandlockQtyVariant('" + strIid.ToString() + "');document.getElementById('" + strIid + "').value='';", true);
                    foreach (GridViewRow gr in grdoptionmainGroup.Rows)
                    {
                        GridView grdvalue = (GridView)gr.FindControl("grdvaluelisting");
                        if (grdvalue.ClientID.ToString().ToLower() == gvTemp.ClientID.ToString().ToLower())
                        {
                            foreach (GridViewRow gr2 in grdvalue.Rows)
                            {
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");


                                //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                                Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                            }

                        }
                        else
                        {
                            foreach (GridViewRow gr1 in grdvalue.Rows)
                            {
                                GridView grdvalue1 = (GridView)gr1.FindControl("grdnamevaluelisting");
                                if (grdvalue1.ClientID.ToString().ToLower().Trim() == gvTemp.ClientID.ToString().ToLower().Trim())
                                {
                                    foreach (GridViewRow gr2 in grdvalue1.Rows)
                                    {
                                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");


                                        //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                                        Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));

                                    }
                                }
                            }
                        }

                    }
                    FillGrid();
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }



                }
                else if (e.CommandName == "Exit")
                {
                    FillGrid();
                }

            }
            catch
            {
            }
        }
        /// <summary>
        /// Value Variant Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdnamevaluelisting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdnamevaluelisting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gvChkListMaster = (GridView)sender;
            // GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
            ImageButton imgSave = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgSave");
            ImageButton imgcancel = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgcancel");
            ImageButton imgEdit = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgEdit");
            ImageButton imgDelete = (ImageButton)gvChkListMaster.Rows[e.NewEditIndex].FindControl("imgDelete");
            Label lttitle = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("lttitle");
            TextBox txttitle = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txttitle");


            Literal ltsku = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltsku");
            TextBox txtsku = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtsku");

            Literal ltupc = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltupc");
            TextBox txtupc = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtupc");

            Literal ltheader = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltheader");
            TextBox txtheader = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtheader");

            Literal ltprice = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltprice");
            TextBox txtprice = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtprice");

            Literal ltdisplayorder = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltdisplayorder");
            TextBox txtdisplayorder = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtdisplayorder");

            Label ltinventory = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltinventory");
            TextBox txtinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtinventory");

            Label ltAllowInventory = (Label)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltAllowInventory");
            TextBox txtAllowinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtAllowinventory");

            Literal ltLockInventory = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltLockInventory");
            TextBox txtLockinventory = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtLockinventory");

            Literal ltAdditionalHemingQty = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltAdditionalHemingQty");
            TextBox txtAdditionalHemingQty = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtAdditionalHemingQty");

            Literal ltBasecustomPrice = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltBasecustomPrice");
            TextBox txtBasecustomPrice = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtBasecustomPrice");



            Literal ltweight = (Literal)gvChkListMaster.Rows[e.NewEditIndex].FindControl("ltweight");
            TextBox txtweight = (TextBox)gvChkListMaster.Rows[e.NewEditIndex].FindControl("txtweight");


            System.Web.UI.HtmlControls.HtmlInputHidden arelatedsku1 = (System.Web.UI.HtmlControls.HtmlInputHidden)gvChkListMaster.Rows[e.NewEditIndex].FindControl("hdnrelatedsku");
            hdnrelatedsku1.Value = arelatedsku1.Value.ToString().Trim();
            //string strScript = "javascript:if(document.getElementById('" + txttitle.ClientID + "').value == ''){jAlert('Please Enter Name.','Message','" + txttitle.ClientID + "'); return false;} chkHeight();";
            //imgSave.OnClientClick = strScript;
            // imgSave.OnClientClick = "return CheckValidation('" + txttitle.ClientID.ToString() + "','Please enter sub option value.');";
            imgSave.OnClientClick = "return CheckValidation('" + txttitle.ClientID.ToString() + "','Please enter sub option value.');";
            lttitle.Visible = false;
            txttitle.Visible = true;
            imgEdit.Visible = false;
            imgDelete.Visible = true;
            imgSave.Visible = true;
            imgcancel.Visible = true;

            ltsku.Visible = false;
            txtsku.Visible = true;

            ltupc.Visible = false;
            txtupc.Visible = true;

            ltheader.Visible = false;
            txtheader.Visible = true;

            ltinventory.Visible = true;
            // txtinventory.Visible = false;

            ltAllowInventory.Visible = false;
            txtAllowinventory.Visible = true;
            txtAllowinventory.Attributes.Add("readonly", "true");
            ltLockInventory.Visible = false;
            txtLockinventory.Visible = true;
            ltdisplayorder.Visible = false;
            txtdisplayorder.Visible = true;
            ltprice.Visible = false;
            txtprice.Visible = true;

            ltBasecustomPrice.Visible = false;
            txtBasecustomPrice.Visible = true;

            ltAdditionalHemingQty.Visible = false;
            txtAdditionalHemingQty.Visible = true;

            ltweight.Visible = false;
            txtweight.Visible = true;
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "gridcollaspe", "expandcollapse('" + hdnrowid.Value.ToString() + "','one');", true);
        }
        protected void grdnamevaluelisting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Attributes.Add("style", "display:none;");
                if (pcount > 0)
                {
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");
                    e.Row.Cells[1].Text = "Fabric&nbsp;Per&nbsp;Yard&nbsp;Cost($)";
                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("style", "border-bottom: solid 1px #eee !important;");
                e.Row.Cells[5].Attributes.Add("style", "display:none;");
                if (pcount > 0)
                {
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");

                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");
                if (e.Row.RowIndex == 0)
                {
                    e.Row.Visible = false;
                }

                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantValueID");
                System.Web.UI.HtmlControls.HtmlAnchor objAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedsku1");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedbuy1get1 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedbuy1get1");
                System.Web.UI.HtmlControls.HtmlAnchor arelatedonsale = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("arelatedonsale");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnrelatedskugrid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnrelatedsku");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnBuy1Get1 = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnBuy1Get1");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnonsale = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnonsale");

                System.Web.UI.HtmlControls.HtmlImage imgBuy1get1 = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgBuy1get1");
                System.Web.UI.HtmlControls.HtmlImage imgonsale = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgonsale");

                Literal ltrBuy1get1 = (Literal)e.Row.FindControl("ltrBuy1get1");
                Literal ltronsale = (Literal)e.Row.FindControl("ltronsale");
                Label ltInventory = (Label)e.Row.FindControl("ltInventory");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnactive = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnactive");
                CheckBox Active = (CheckBox)e.Row.FindControl("chkactive");
                if (hdnactive.Value.ToString() == "1" || hdnactive.Value.ToString().ToLower() == "true")
                {
                    Active.Checked = true;

                }


                Active.Attributes.Add("onchange", "updateactiveoption('" + Active.ClientID.ToString() + "');");
                DataSet dswaregouse = new DataSet();
                dswaregouse = CommonComponent.GetCommonDataSet("SELECT  tb_WareHouse.Code,isnull(tb_WareHouseProductVariantInventory.inventory,0) as inventory FROM dbo.tb_WareHouse INNER JOIN dbo.tb_WareHouseProductVariantInventory ON dbo.tb_WareHouse.WareHouseID = dbo.tb_WareHouseProductVariantInventory.WareHouseID WHERE tb_WareHouseProductVariantInventory.ProductID=" + Request.QueryString["ID"].ToString() + " and tb_WareHouseProductVariantInventory.VariantValueID=" + hdnVariantValueID.Value.ToString() + " ");
                string str = "<br/>";
                if (dswaregouse != null && dswaregouse.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dswaregouse.Tables[0].Rows.Count; i++)
                    {
                        str += dswaregouse.Tables[0].Rows[i]["Code"].ToString() + " :" + dswaregouse.Tables[0].Rows[i]["inventory"].ToString() + "&nbsp;";
                    }
                }
                ltInventory.Text += str;
                //if (!string.IsNullOrEmpty(hdnBuy1Get1.Value) && hdnBuy1Get1.Value.ToString() == "1")
                //{
                //    imgBuy1get1.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                //    imgBuy1get1.Attributes.Add("style", "display:''");
                //}
                //else { imgBuy1get1.Attributes.Add("style", "display:none"); }

                System.Web.UI.HtmlControls.HtmlInputHidden hdnupdatebuy1 = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnupdatebuy1");


                if (!string.IsNullOrEmpty(hdnBuy1Get1.Value) && (hdnBuy1Get1.Value.ToString() == "1" || hdnBuy1Get1.Value.ToString().ToLower().Trim() == "true"))
                {

                    if (!string.IsNullOrEmpty(hdnupdatebuy1.Value) && hdnupdatebuy1.Value.ToString() == "1")
                    {
                        imgBuy1get1.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                        imgBuy1get1.Attributes.Add("style", "display:''");
                    }
                    else
                    {
                        imgBuy1get1.Attributes.Add("style", "display:none");
                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0 where VariantValueID=" + Convert.ToInt32(hdnVariantValueID.Value.ToString()) + "");
                    }
                }
                else { imgBuy1get1.Attributes.Add("style", "display:none"); }


                if (!string.IsNullOrEmpty(hdnonsale.Value) && hdnonsale.Value.ToString() == "1")
                {
                    imgonsale.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png\"";
                    imgonsale.Attributes.Add("style", "display:''");
                }
                else { imgonsale.Attributes.Add("style", "display:none"); }

                System.Web.UI.HtmlControls.HtmlAnchor lnkEditInventory = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lnkEditInventory");
                lnkEditInventory.Attributes.Add("onclick", "ShowModelUserRegisterPopup(" + DataBinder.Eval(e.Row.DataItem, "ProductID").ToString() + "," + e.Row.DataItemIndex.ToString() + "," + DataBinder.Eval(e.Row.DataItem, "VariantID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "VariantValueID").ToString() + ",'" + lnkEditInventory.ClientID.ToString() + "');");

                if (hdnrelatedskugrid.Value.ToString().Trim().ToLower() == "select sku")
                {
                    objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                }
                else
                {
                    if (hdnrelatedskugrid.Value.ToString().Trim() == "")
                    {
                        objAnchor.InnerHtml = "select sku";
                        objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                    }
                    else
                    {
                        objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','" + hdnrelatedskugrid.Value.ToString() + "');");
                    }
                }
                if (hdnrelatedskugrid.Value.ToString().Trim() == "0")
                {
                    objAnchor.InnerHtml = "select sku";
                    objAnchor.Attributes.Add("onclick", "openCenteredCrossSaleWindow('" + objAnchor.ClientID.ToString() + "','');");
                }
                arelatedbuy1get1.Attributes.Add("onclick", "openCenteredCrossSaleWindowOnsaleBuy1('" + arelatedbuy1get1.ClientID.ToString() + "','" + hdnVariantValueID.Value.ToString() + "');");
                arelatedonsale.Attributes.Add("onclick", "openCenteredCrossSaleWindowOnsaleBuy1('" + arelatedonsale.ClientID.ToString() + "','" + hdnVariantValueID.Value.ToString() + "');");

                //if (hdnrelatedsku.Value.ToString().ToLower() == "select sku")
                //{
                hdnrelatedsku1.Value = hdnrelatedskugrid.Value.ToString();
                //}

                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;

                ((ImageButton)e.Row.FindControl("imgEdit")).Visible = true;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtTitle = (TextBox)e.Row.FindControl("txtTitle");
                LinkButton lnkAdd = (LinkButton)e.Row.FindControl("lnkAdd");
                TextBox txtAllowInventory = (TextBox)e.Row.FindControl("txtAllowInventory");
                txtAllowInventory.Attributes.Add("readonly", "true");

                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantIDSub = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantIDSub");
                lnkAdd.OnClientClick = "return CheckValidation('" + txtTitle.ClientID.ToString() + "','Please enter sub option value.');";
                hdnVariantIDSub.Value = ViewState["varaintidname"].ToString();
                if (ViewState["grdnamevaluelisting"] != null && Convert.ToInt32(ViewState["grdnamevaluelisting"].ToString()) == 1)
                {
                    e.Row.Cells[0].Attributes.Add("style", "border-right:none !important;");
                }

                e.Row.Visible = true;
                e.Row.Cells[5].Attributes.Add("style", "display:none;");
                if (pcount > 0)
                {
                    e.Row.Cells[2].Attributes.Add("style", "display:none;");

                }
                e.Row.Cells[4].Attributes.Add("style", "display:none;");
                e.Row.Cells[6].Attributes.Add("style", "display:none;");
            }
        }
        protected void grdnamevaluelisting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add1")
                {
                    GridView gvTemp = (GridView)sender;

                    Int32 VariantValueid = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnVariantIDSub")).Value.ToString()); //Convert.ToInt32(grdoptionmainGroup.DataKeys[0].Value.ToString());
                    string strvaluename = ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).Text;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    strvaluename = regex.Replace(strvaluename, " ");
                    strvaluename = strvaluename.Trim();
                    SQLAccess objSql = new SQLAccess();
                    Int32 CountVariant = 0;

                    Int32 Variantid = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT isnull(variantid,0) FROM tb_ProductVariant WHERE parentid=" + VariantValueid + ""));
                    if (Variantid <= 0)
                    {
                        Variantid = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO  tb_ProductVariant(VariantName,ProductID,IsParent,ParentId) VALUES ('Select Size'," + Request.QueryString["id"].ToString() + ",0," + VariantValueid + ") SELECT SCOPE_IDENTITY();"));
                    }
                    CountVariant = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(*) FROM tb_ProductVariantValue WHERE VariantValue='" + strvaluename.Replace("'", "''").Trim() + "' AND ProductId=" + Request.QueryString["id"].ToString() + " AND VariantID=" + Variantid + ""));
                    if (CountVariant > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Name Already Exists!','Message','" + ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).ClientID.ToString() + "');", true);
                        return;
                    }
                    string strstrvaluesku = ((TextBox)gvTemp.FooterRow.FindControl("txtSku")).Text;
                    string strstrvalueupc = ((TextBox)gvTemp.FooterRow.FindControl("txtUpc")).Text;
                    string strstrvalueheader = ((TextBox)gvTemp.FooterRow.FindControl("txtHeader")).Text;
                    string strInventory = ((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnmainqty")).Value;
                    string strLockInventory = ((TextBox)gvTemp.FooterRow.FindControl("txtLockinventory")).Text;
                    string StrBasecustomPrice = ((TextBox)gvTemp.FooterRow.FindControl("txtBasecustomPrice")).Text;
                    string strAdditionalHemingQty = ((TextBox)gvTemp.FooterRow.FindControl("txtAdditionalHemingQty")).Text;
                    string strbackorderdate = ((TextBox)gvTemp.FooterRow.FindControl("txtbackorderdate")).Text;
                    CheckBox Active = (CheckBox)gvTemp.FooterRow.FindControl("chkactive");

                    Int32 iSActive = 0;
                    if (Active.Checked)
                    {
                        iSActive = 1;
                    }
                    decimal weight = 0;
                    decimal.TryParse(((TextBox)gvTemp.FooterRow.FindControl("txtweight")).Text, out weight);
                    if (StrBasecustomPrice == "")
                        StrBasecustomPrice = "0";
                    if (strAdditionalHemingQty == "")
                        strAdditionalHemingQty = "0";

                    if (strInventory == "")
                    {
                        strInventory = "0";
                    }
                    if (strLockInventory == "")
                    {
                        strLockInventory = "0";
                    }
                    string strAllowInventory = ((TextBox)gvTemp.FooterRow.FindControl("txtAllowinventory")).Text;
                    if (strAllowInventory == "")
                    {
                        strAllowInventory = "0";
                    }
                    string strprice = ((TextBox)gvTemp.FooterRow.FindControl("txtPrice")).Text;
                    string strDisplayorder = ((TextBox)gvTemp.FooterRow.FindControl("txtDisplayorder")).Text;
                    string relatedSKu = hdnrelatedsku1.Value.ToString();

                    if (strInventory.Trim() == "")
                        strInventory = "0";
                    if (strprice.Trim() == "")
                        strprice = "0";
                    if (strDisplayorder.Trim() == "")
                        strDisplayorder = "0";

                    string StrVariPath = "";
                    if (!string.IsNullOrEmpty(hdnrelatedcolor.Value.Trim()))
                    {
                        string Strcolor = Convert.ToString(hdnrelatedcolor.Value.Trim());
                        StrVariPath = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where lower(ColorName)='" + Strcolor.ToString().ToLower() + "'"));
                    }
                    if (string.IsNullOrEmpty(strbackorderdate))
                    {
                        strbackorderdate = null;
                    }
                    else
                    {
                        strbackorderdate = strbackorderdate.Replace("'", "''");
                    }

                    Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue(VarActive,VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID,SKU,UPC,Header,RelatedProductid,AllowQuantity,LockQuantity,VariImageName,AddiHemingQty,BasecustomPrice,BackOrderdate,weight) VALUES (" + iSActive + "," + Variantid + ",'" + strvaluename.Replace("'", "''").Trim() + "'," + strprice + "," + strDisplayorder + "," + Request.QueryString["id"].ToString() + ",'" + strstrvaluesku.Replace("'", "''") + "','" + strstrvalueupc.Replace("'", "''") + "','" + strstrvalueheader.Replace("'", "''") + "','" + relatedSKu.Trim().Replace("'", "''").Replace("Select SKU", "") + "'," + strAllowInventory + "," + strLockInventory + ",'" + StrVariPath + "'," + strAdditionalHemingQty + "," + StrBasecustomPrice + ",'" + strbackorderdate + "'," + weight + ") SELECT SCOPE_IDENTITY();"));
                    if (string.IsNullOrEmpty(strbackorderdate))
                    {
                        objSql.ExecuteNonQuery("update tb_ProductVariantValue SET BackOrderdate= NULL WHERE VariantValueID=" + id + "");
                    }
                    foreach (GridViewRow gr in gvTemp.Rows)
                    {

                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnAllowInventory");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnVariantValueID");
                        //id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                        id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));

                    }


                    FillGrid();
                    hdnrelatedsku1.Value = "select sku";
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }

                }
                else if (e.CommandName == "updateactive1")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 Variantvalueid = Convert.ToInt32(e.CommandArgument.ToString());
                    CheckBox Active = (CheckBox)grrow.FindControl("chkactive");
                    Int32 isActive = 0;
                    if (Active.Checked)
                    {
                        isActive = 1;
                    }
                    CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VarActive='" + isActive + "' Where VariantValueID=" + Variantvalueid.ToString() + "");
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }

                }
                else if (e.CommandName == "Save1")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 Variantvalueid = Convert.ToInt32(e.CommandArgument.ToString());
                    TextBox txttitle = (TextBox)grrow.FindControl("txttitle");


                    string strvaluename = ((TextBox)grrow.FindControl("txttitle")).Text;
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    strvaluename = regex.Replace(strvaluename, " ");
                    strvaluename = strvaluename.Trim();
                    SQLAccess objSql = new SQLAccess();
                    Int32 CountVariant = 0;
                    CountVariant = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(*) FROM tb_ProductVariantValue WHERE Variantid in (SELECT isnull(Variantid,0) FROM tb_ProductVariant WHERE isnull(parentid,0)=" + Variantvalueid + ") AND VariantValue='" + strvaluename.Replace("'", "''") + "' AND ProductId=" + Request.QueryString["id"].ToString() + " AND VariantValueID <> " + Variantvalueid + ""));
                    if (CountVariant > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popupload", "jAlert('Name Already Exists!','Message','" + ((TextBox)gvTemp.FooterRow.FindControl("txtTitle")).ClientID.ToString() + "');", true);
                        return;
                    }
                    string strstrvaluesku = ((TextBox)grrow.FindControl("txtsku")).Text;
                    string strstrvalueupc = ((TextBox)grrow.FindControl("txtupc")).Text;
                    string strstrvalueheader = ((TextBox)grrow.FindControl("txtheader")).Text;
                    string strInventory = ((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnmainqty")).Value;
                    string strLockInventory = ((TextBox)grrow.FindControl("txtLockinventory")).Text;
                    string strAllowInventory = ((TextBox)grrow.FindControl("txtAllowinventory")).Text;
                    string stBasecustomPrice = ((TextBox)grrow.FindControl("txtBasecustomPrice")).Text;
                    string strAdditionalHemingQty = ((TextBox)grrow.FindControl("txtAdditionalHemingQty")).Text;
                    string strbackorderdate = ((TextBox)grrow.FindControl("txtbackorderdate")).Text;

                    decimal weight = 0;
                    decimal.TryParse(((TextBox)grrow.FindControl("txtweight")).Text, out weight);
                    CheckBox Active = (CheckBox)grrow.FindControl("chkactive");

                    Int32 iSActive = 0;
                    if (Active.Checked)
                    {
                        iSActive = 1;
                    }


                    if (stBasecustomPrice == "")
                        stBasecustomPrice = "0";
                    if (strAdditionalHemingQty == "")
                        strAdditionalHemingQty = "0";

                    if (strInventory == "")
                    {
                        strInventory = "0";
                    }
                    if (strLockInventory == "")
                    {
                        strLockInventory = "0";
                    }
                    if (strAllowInventory == "")
                    {
                        strAllowInventory = "0";
                    }
                    string strprice = ((TextBox)grrow.FindControl("txtprice")).Text;
                    string strDisplayorder = ((TextBox)grrow.FindControl("txtdisplayorder")).Text;

                    string relatedSKu = hdnrelatedsku1.Value.ToString();
                    if (strInventory.Trim() == "")
                        strInventory = "0";
                    if (strprice.Trim() == "")
                        strprice = "0";
                    if (strDisplayorder.Trim() == "")
                        strDisplayorder = "0";

                    if (string.IsNullOrEmpty(strbackorderdate))
                    {
                        strbackorderdate = null;
                    }
                    else
                    {
                        strbackorderdate = strbackorderdate.Replace("'", "''");
                    }


                    DataSet Dsprevalue = new DataSet();
                    Decimal beforePrice = decimal.Zero;
                    Decimal Price = decimal.Zero;

                    Decimal Basecustomprice = decimal.Zero;
                    Decimal beforeBasecustomprice = decimal.Zero;
                    string logupc = "";
                    string logsku = "";
                    Dsprevalue = CommonComponent.GetCommonDataSet("select isnull(BasecustomPrice,0) as BasecustomPrice,isnull(VariantPrice,0) as VariantPrice,isnull(upc,'') as upc,isnull(sku,'') as sku from tb_ProductVariantValue where  VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + "");
                    if (Dsprevalue != null && Dsprevalue.Tables.Count > 0 && Dsprevalue.Tables[0].Rows.Count > 0)
                    {


                        Decimal.TryParse(Dsprevalue.Tables[0].Rows[0]["VariantPrice"].ToString(), out beforePrice);
                        Decimal.TryParse(Dsprevalue.Tables[0].Rows[0]["BasecustomPrice"].ToString(), out beforeBasecustomprice);
                        Decimal.TryParse(strprice.Replace("'", "''"), out Price);
                        Decimal.TryParse(stBasecustomPrice.Replace("'", "''"), out Basecustomprice);
                        logupc = Dsprevalue.Tables[0].Rows[0]["upc"].ToString();
                        logsku = Dsprevalue.Tables[0].Rows[0]["sku"].ToString();



                    }




                    //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET VarActive=" + iSActive + ",weight =" + weight + " ,BasecustomPrice=" + stBasecustomPrice + ",AddiHemingQty=" + strAdditionalHemingQty + ",AllowQuantity=" + strAllowInventory + ",LockQuantity=" + strLockInventory + ", RelatedProductid='" + relatedSKu.Trim().Replace("Select SKU", "") + "',VariantValue='" + strvaluename.Replace("'", "''").Trim() + "',VariantPrice='" + strprice.Replace("'", "''") + "',DisplayOrder=" + strDisplayorder + ",SKU='" + strstrvaluesku.Replace("'", "''") + "',UPC='" + strstrvalueupc.Replace("'", "''") + "',Header='" + strstrvalueheader.Trim().Replace("'", "''") + "',BackOrderdate='" + strbackorderdate + "' WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET VarActive=" + iSActive + ",weight =" + weight + " ,BasecustomPrice=" + stBasecustomPrice + ",AddiHemingQty=" + strAdditionalHemingQty + ",AllowQuantity=0,LockQuantity=" + strLockInventory + ", RelatedProductid='" + relatedSKu.Trim().Replace("Select SKU", "") + "',VariantValue='" + strvaluename.Replace("'", "''").Trim() + "',VariantPrice='" + strprice.Replace("'", "''") + "',DisplayOrder=" + strDisplayorder + ",SKU='" + strstrvaluesku.Replace("'", "''") + "',UPC='" + strstrvalueupc.Replace("'", "''") + "',Header='" + strstrvalueheader.Trim().Replace("'", "''") + "',BackOrderdate='" + strbackorderdate + "' WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                    if (beforePrice != Price || beforeBasecustomprice != Basecustomprice)
                    {
                        CommonComponent.ExecuteCommonData("Exec GuiInsertPriceLog '" + logsku.Trim().Replace("'", "''") + "','" + logupc.Trim().Replace("'", "''") + "'," + Price + "," + beforePrice + "," + Basecustomprice + "," + beforeBasecustomprice + ",'Manual'," + Session["AdminID"].ToString() + "");
                    }
                    if (string.IsNullOrEmpty(strbackorderdate))
                    {



                        objSql.ExecuteNonQuery("update tb_ProductVariantValue SET BackOrderdate= NULL WHERE VariantValueID=" + Variantvalueid + " AND ProductID=" + Request.QueryString["id"].ToString() + "");
                    }

                    foreach (GridViewRow gr in gvTemp.Rows)
                    {
                        TextBox txttitlenew = (TextBox)gr.FindControl("txttitle");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnAllowInventory");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnVariantValueID");

                        if (txttitlenew.ClientID.ToString().ToLower() != txttitle.ClientID.ToString().ToLower())
                        {
                            //id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                            id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                        }
                    }

                    FillGrid();
                    hdnrelatedsku1.Value = "select sku";
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }

                }
                else if (e.CommandName == "Remove1")
                {
                    GridView gvTemp = (GridView)sender;
                    GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                    Int32 VariantvalueId = Convert.ToInt32(e.CommandArgument.ToString());
                    Int32 VariantID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnVariantID")).Value);

                    SQLAccess objSql = new SQLAccess();
                    objSql.ExecuteNonQuery("DELETE FROM tb_ProductVariantValue WHERE VariantvalueId=" + VariantvalueId + ";EXEC dbo.usp_InsertUpdateVariantWarehouse @ProductID = " + Convert.ToInt32(Request.QueryString["ID"]) + ",@VariantID = " + VariantID + ", @VariantValueID = " + VariantvalueId + ", @CreatedBy = " + Convert.ToInt32(Session["AdminID"]) + ",@UpdatedBy = " + Convert.ToInt32(Session["AdminID"]) + ",@Mode =" + 3);
                    FillGrid();

                    string strIid = gvTemp.ClientID.ToString() + "_txtLockinventory";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlert", "javascript:document.getElementById('" + strIid + "').value='0'; AllowandlockQtyVariant('" + strIid.ToString() + "');document.getElementById('" + strIid + "').value='';", true);
                    foreach (GridViewRow gr in grdoptionmainGroup.Rows)
                    {
                        GridView grdvalue = (GridView)gr.FindControl("grdvaluelisting");
                        if (grdvalue.ClientID.ToString().ToLower() == gvTemp.ClientID.ToString().ToLower())
                        {
                            foreach (GridViewRow gr2 in grdvalue.Rows)
                            {
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");
                                // Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                                Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                            }
                        }
                        else
                        {
                            foreach (GridViewRow gr1 in grdvalue.Rows)
                            {
                                GridView grdvalue1 = (GridView)gr1.FindControl("grdnamevaluelisting");
                                if (grdvalue1.ClientID.ToString().ToLower().Trim() == gvTemp.ClientID.ToString().ToLower().Trim())
                                {
                                    foreach (GridViewRow gr2 in grdvalue1.Rows)
                                    {
                                        System.Web.UI.HtmlControls.HtmlInputHidden hdnAllowInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnAllowInventory");
                                        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr2.FindControl("hdnVariantValueID");


                                        //Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=" + hdnAllowInventory.Value.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));
                                        Int32 id = Convert.ToInt32(objSql.ExecuteScalarQuery("UPDATE tb_ProductVariantValue SET AllowQuantity=0 WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + " AND ProductID=" + Request.QueryString["id"].ToString() + ""));

                                    }
                                }
                            }
                        }

                    }
                    try
                    {
                        Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIN(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + Request.QueryString["id"].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                        if (sprice > Decimal.Zero)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + Request.QueryString["id"].ToString() + "");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "salepricechange", "window.parent.document.getElementById('ContentPlaceHolder1_txtSalePrice').value='" + string.Format("{0:0.00}", sprice) + "';", true); ;
                        }
                    }
                    catch
                    {

                    }

                }
                else if (e.CommandName == "Exit1")
                {
                    FillGrid();
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String value after Remove Special Character</returns>
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);
            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            res = value;
            return res;
        }

    }
}