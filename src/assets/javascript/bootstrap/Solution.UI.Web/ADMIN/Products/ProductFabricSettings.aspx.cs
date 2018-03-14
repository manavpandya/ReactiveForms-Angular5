using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductFabricSettings : BasePage
    {
        ProductComponent objProduct = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                FillFabricVendor();
                lblMsg.Text = "";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnSavevendor.ImageUrl = "/App_Themes/" + Page.Theme + "/images/savevendor.gif";
                FillFabric();
                FillFabricType();
                if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
                {
                    BackLink.Visible = true;
                    BackLink.HRef = "Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFabricCode=FabricCode";
                }
                else if (Request.QueryString["ProductStoreID"] != null)
                {
                    BackLink.Visible = true;
                    BackLink.HRef = "Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFabricCode=FabricCode";
                }
            }
            btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnimport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/import-fabric-data.png) no-repeat transparent; width: 133px; height: 23px; border:none;cursor:pointer;");
        }

        private void FillFabric()
        {
            DataSet DsFabircType = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            if (Session["VendorLogin"] != null && Session["VendorLogin"].ToString() == "1" && Session["AdminvendorId"] != null && Session["AdminvendorId"].ToString() != "")
            {
                DsFabircType = CommonComponent.GetCommonDataSet("SELECT FabricTypeID,FabricTypename FROM tb_ProductFabricType WHERE isnull(Active,0)=1 AND FabricTypename in (SELECT isnull(FabricType,'') FROM tb_product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+FabricVendorIds+',') > 0 AND Storeid=1 And isnull(active,0)=1 and isnull(Deleted,0)=0)"); //objProduct.GetProductFabricDetails(0, 1);
            }
            else
            {
                DsFabircType = objProduct.GetProductFabricDetails(0, 1);
            }
            if (DsFabircType != null && DsFabircType.Tables.Count > 0 && DsFabircType.Tables[0].Rows.Count > 0)
            {
                ddlFabricType.DataSource = DsFabircType;
                ddlFabricType.DataValueField = "FabricTypeID";
                ddlFabricType.DataTextField = "FabricTypename";
                ddlFabricType.DataBind();
            }
            else
            {
                ddlFabricType.DataSource = null;
                ddlFabricType.DataBind();
            }
            ddlFabricType.Items.Insert(0, new ListItem("Select Fabric Category", "0"));
            ddlFabricType_SelectedIndexChanged(null, null);
        }
        private void FillFabricEdit()
        {

            ddlFabricType_SelectedIndexChanged(null, null);
        }
        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFabricType();
            //            ProductComponent objProduct = new ProductComponent();
            //            DataSet DsFabricCode = new DataSet();
            //            if (Session["VendorLogin"] != null)
            //            {
            //                DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty from tb_ProductFabricCode Where FabricTypeID =" + ddlFabricType.SelectedValue.ToString() + @" AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
            //  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @")  AND Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0)
            //      Union All  
            //  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0) AND tb_FabricVendorPortal.FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @"  
            //  Order by FabricCodeId"); // objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
            //            }
            //            else
            //            {
            //                DsFabricCode = objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
            //            }
            //            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            //            {
            //                grdFabricType.DataSource = DsFabricCode;
            //                grdFabricType.DataBind();
            //                trFabricDetails.Visible = true;
            //            }
            //            else
            //            {
            //                grdFabricType.DataSource = null;
            //                grdFabricType.DataBind();
            //                trFabricDetails.Visible = false;
            //            }
        }

        private void FillFabricType()
        {
            objProduct = new ProductComponent();
            DataSet dsFabricType = new DataSet();
            if (ddlFabricType.SelectedIndex > 0)
            {
                dsFabricType = objProduct.GetProductFabricType(Convert.ToInt32(ddlFabricType.SelectedValue));

                if (dsFabricType != null && dsFabricType.Tables.Count > 0 && dsFabricType.Tables[0].Rows.Count > 0)
                {
                    grdFabricType.DataSource = dsFabricType;
                    grdFabricType.DataBind();
                }
                else
                {
                    grdFabricType.DataSource = null;
                    grdFabricType.DataBind();
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txtSearch.Text))
                {
                    DataSet ds = new DataSet();
                    ds = CommonComponent.GetCommonDataSet("select distinct FabricTypeID from tb_ProductFabricCode where code like '%" + txtSearch.Text.Replace("'","''") + "%'");
                    string fabrictypeid = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 FabricTypeID from tb_ProductFabricCode where code like '%" + txtSearch.Text.Replace("'", "''") + "%'"));
                    if (!String.IsNullOrEmpty(fabrictypeid))
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i == 0)
                                {
                                    dsFabricType = objProduct.GetProductFabricType(Convert.ToInt32(ds.Tables[0].Rows[i]["FabricTypeID"].ToString()));
                                }
                                else
                                {
                                    DataSet dsTemp = objProduct.GetProductFabricType(Convert.ToInt32(ds.Tables[0].Rows[i]["FabricTypeID"].ToString()));
                                    if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                                    {
                                        dsFabricType.Merge(dsTemp);
                                    }
                                }

                            }
                        }


                        if (dsFabricType != null && dsFabricType.Tables.Count > 0 && dsFabricType.Tables[0].Rows.Count > 0)
                        {
                            grdFabricType.DataSource = dsFabricType;
                            grdFabricType.DataBind();
                        }
                        else
                        {
                            grdFabricType.DataSource = null;
                            grdFabricType.DataBind();
                        }
                    }
                    else
                    {
                        grdFabricType.DataSource = null;
                        grdFabricType.DataBind();
                    }
                }
                else
                {
                    grdFabricType.DataSource = null;
                    grdFabricType.DataBind();
                }


            }
        }

        private void FillFabricVendor()
        {
            VendorDAC objVendorDAC = new VendorDAC();
            DataSet dsdropshipsku = objVendorDAC.GetVendorList(0);
            ViewState["dsdropshipsku"] = dsdropshipsku;
        }
        #region Code for Fabric Type

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            objProduct = new ProductComponent();

            if (txtDisplayOrder.Text.Trim() == "")
            {
                txtDisplayOrder.Text = "999";
            }
            Int32 IsAdded = objProduct.Insert_Update_Delete_FabricType(0, txtFabricTypeName.Text.Trim(), Convert.ToInt32(txtDisplayOrder.Text), chkActive.Checked, 1);
            if (IsAdded > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeAdded", "jAlert('Fabric category added successfully.','Message');", true);
                FillFabric();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DuplicatedType", "jAlert('Fabric category already exists.','Message','ContentPlaceHolder1_txtFabricTypeName');", true);
                return;
            }
            if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
            {
                if (Request.QueryString["SearchFabricType"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFabricType=FabricType");
                }
                else if (Request.QueryString["SearchFabricCode"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFabricCode=FabricCode");
                }

            }
            else if (Request.QueryString["ProductStoreID"] != null)
            {
                if (Request.QueryString["SearchFabricType"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFabricType=FabricType");
                }
                else if (Request.QueryString["SearchFabricCode"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFabricCode=FabricCode");
                }

            }
        }
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void grdFabricType_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.Trim().ToLower() == "save")
            {
                objProduct = new ProductComponent();
                Int32 FabricTypeID = Convert.ToInt32(e.CommandArgument);
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricTypeName = (Literal)grrow.FindControl("ltrFabricTypeName");
                TextBox txtFabricTypeName = (TextBox)grrow.FindControl("txtFabricTypeName");

                System.Web.UI.HtmlControls.HtmlImage imgTypeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgTypeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");




                //if(chkActive.Checked==false)
                //{

                //    DataSet dsfabriccat = new DataSet();
                //    dsfabriccat = CommonComponent.GetCommonDataSet("select sku from tb_product  where isnull(FabricType,'')='" + txtFabricTypeName.Text.ToString() + "' and storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0");
                //    if(dsfabriccat!=null && dsfabriccat.Tables.Count>0 && dsfabriccat.Tables[0].Rows.Count>0)
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeUpdated", "jAlert('Fabric category is already assigned,you can not inactive.','Message');", true);
                //        return;
                //    }

                //}

                if (chkActive.Checked == false)
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Ismadetoready=1,Ismadetoorder=0,Ismadetomeasure=0,IsCustom=0 WHERE FabricType in (select FabricTypename from tb_ProductFabricType where FabricTypeID=" + FabricTypeID + ")");

                }


                Int32 IsUpdated = objProduct.Insert_Update_Delete_FabricType(FabricTypeID, txtFabricTypeName.Text.Trim(), Convert.ToInt32(txtdisplayorder.Text), chkActive.Checked, 2);

                if (IsUpdated > 0)
                {



                    ltrFabricTypeName.Visible = true;
                    txtFabricTypeName.Visible = false;


                    imgTypeActive.Visible = true;
                    chkActive.Visible = false;

                    //ltdisplayorder.Visible = true;
                    //txtdisplayorder.Visible = false;

                    imgSave.Visible = false;
                    imgcancel.Visible = false;
                    imgEdit.Visible = true;
                    FillFabric();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeUpdated", "jAlert('Fabric category updated successfully.','Message');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbTypeUpdated", "jAlert('Fabric category already exists.','Message');", true);
                }
            }
            else if (e.CommandName.Trim().ToLower() == "edit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricTypeName = (Literal)grrow.FindControl("ltrFabricTypeName");
                TextBox txtFabricTypeName = (TextBox)grrow.FindControl("txtFabricTypeName");

                System.Web.UI.HtmlControls.HtmlImage imgTypeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgTypeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                //Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                //TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");


                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");





                ltrFabricTypeName.Visible = false;
                txtFabricTypeName.Visible = true;






                string strScript = "javascript:if(document.getElementById('" + txtFabricTypeName.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric type.','Message','" + txtFabricTypeName.ClientID + "'); }return checkfabriccategory(" + grrow.RowIndex + ");";
                imgSave.OnClientClick = strScript;

                ltrFabricTypeName.Visible = false;
                txtFabricTypeName.Visible = true;
                txtFabricTypeName.Text = ltrFabricTypeName.Text;

                imgTypeActive.Visible = false;
                chkActive.Visible = true;

                //ltdisplayorder.Visible = false;
                //txtdisplayorder.Visible = true;
                //txtdisplayorder.Text = ltdisplayorder.Text;

                imgSave.Visible = true;
                imgcancel.Visible = true;
                imgEdit.Visible = false;
            }
            else if (e.CommandName.Trim().ToLower() == "exit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricTypeName = (Literal)grrow.FindControl("ltrFabricTypeName");
                TextBox txtFabricTypeName = (TextBox)grrow.FindControl("txtFabricTypeName");

                System.Web.UI.HtmlControls.HtmlImage imgTypeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgTypeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");





                ltrFabricTypeName.Visible = true;
                txtFabricTypeName.Visible = false;



                ltrFabricTypeName.Visible = true;
                txtFabricTypeName.Visible = false;

                imgTypeActive.Visible = true;
                chkActive.Visible = false;

                //ltdisplayorder.Visible = true;
                //txtdisplayorder.Visible = false;

                imgSave.Visible = false;
                imgcancel.Visible = false;
                imgEdit.Visible = true;
            }
            else if (e.CommandName.Trim().ToLower() == "remove")
            {
                objProduct = new ProductComponent();
                Int32 FabricTypeID = Convert.ToInt32(e.CommandArgument);



                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Ismadetoready=1,Ismadetoorder=0,Ismadetomeasure=0,IsCustom=0, FabricVendorIds='',FabricCode='',FabricType='' WHERE FabricType in (select FabricTypename from tb_ProductFabricType where FabricTypeID=" + FabricTypeID + ")");
                Int32 IsDeleted = objProduct.Insert_Update_Delete_FabricType(FabricTypeID, "", 0, Convert.ToBoolean(0), 3);
                if (IsDeleted > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeDeleted", "jAlert('Fabric category deleted successfully.','Message');", true);
                    FillFabric();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbTypeDeleted", "jAlert('Problem while deleting fabric type.','Message');", true);
                }
            }

        }
        protected void grdFabricType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "display:none;");
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes.Add("style", "border-right:none;");
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "border-left:none;");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "display:none;");
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes.Add("style", "border-right:none;");
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "border-left:none;");
                GridView grdFabricCode = (GridView)e.Row.FindControl("grdFabricCode");
                Int32 FabricTypeID = Convert.ToInt32(grdFabricType.DataKeys[e.Row.RowIndex].Value);
                DataSet dsFCode = new DataSet();
                objProduct = new ProductComponent();
                if (txtSearch.Text != "")
                {
                    dsFCode = objProduct.GetProductFabricCode(FabricTypeID, txtSearch.Text.Trim());
                }
                else
                {
                    dsFCode = objProduct.GetProductFabricCode(FabricTypeID);
                }
                ViewState["FabricTypeID"] = FabricTypeID.ToString();
                if (dsFCode != null && dsFCode.Tables.Count > 0 && dsFCode.Tables[0].Rows.Count > 0)
                {
                    Session["FabricVendorData"] = dsFCode;
                    grdFabricCode.DataSource = dsFCode;
                    grdFabricCode.DataBind();
                    //btnExport.Visible = true;
                    trsavevendor.Visible = true;
                }
                else
                {
                    Session["FabricVendorData"] = null;
                    grdFabricCode.DataSource = null;
                    grdFabricCode.DataBind();
                    //btnExport.Visible = false;
                    btnExport.Visible = true;
                }
            }
        }
        protected void grdFabricType_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion

        #region Code for Fabric Code

        protected void grdFabricCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Trim().ToLower() == "codeadd")
            {
                GridView gvTemp = (GridView)sender;
                Int32 FabricTypeID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnFooterFabricTypeID")).Value.ToString());
                objProduct = new ProductComponent();

                TextBox txtFooterFabricCode = (TextBox)gvTemp.FooterRow.FindControl("txtFooterFabricCode");
                TextBox txtFooterFabricName = (TextBox)gvTemp.FooterRow.FindControl("txtFooterFabricName");
                CheckBox chkActive = (CheckBox)gvTemp.FooterRow.FindControl("chkFooterActive");
                TextBox txtDisplayorder = (TextBox)gvTemp.FooterRow.FindControl("txtFooterDisplayorder");
                CheckBoxList chkvendorlist = (CheckBoxList)gvTemp.FooterRow.FindControl("chkvendorlistfooter");
                DropDownList ddlVendorListFooter = (DropDownList)gvTemp.FooterRow.FindControl("ddlVendorListFooter");



                TextBox txtfooterperyard = (TextBox)gvTemp.FooterRow.FindControl("txtfooterperyard");

                TextBox txtfooterMinQty = (TextBox)gvTemp.FooterRow.FindControl("txtfooterMinQty");


                TextBox txtfootersafetylock = (TextBox)gvTemp.FooterRow.FindControl("txtfootersafetylock");


                TextBox txtfooterdays = (TextBox)gvTemp.FooterRow.FindControl("txtfooterdays");


                CheckBox chkfooterdiscontinue = (CheckBox)gvTemp.FooterRow.FindControl("chkfooterdiscontinue");

                TextBox txtfooterupc = (TextBox)gvTemp.FooterRow.FindControl("txtfooterupc");
                //TextBox txtfooterMinwidthQty = (TextBox)gvTemp.FooterRow.FindControl("txtfooterMinwidthQty");
                //TextBox txtfootermaxwidthQty = (TextBox)gvTemp.FooterRow.FindControl("txtfootermaxwidthQty");
                //TextBox txtfooterminlengthQty = (TextBox)gvTemp.FooterRow.FindControl("txtfooterminlengthQty");
                //TextBox txtfootermaxlengthQty = (TextBox)gvTemp.FooterRow.FindControl("txtfootermaxlengthQty");

                //Int32 MinwidthQty = 0;
                //Int32 maxwidthQty = 0;
                //Int32 minlengthQty = 0;
                //Int32 maxlengthQty = 0;
                //if (!string.IsNullOrEmpty(txtfooterMinwidthQty.Text.ToString()))
                //{
                //    MinwidthQty = Convert.ToInt32(txtfooterMinwidthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtfootermaxwidthQty.Text.ToString()))
                //{
                //    maxwidthQty = Convert.ToInt32(txtfootermaxwidthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtfooterminlengthQty.Text.ToString()))
                //{
                //    minlengthQty = Convert.ToInt32(txtfooterminlengthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtfootermaxlengthQty.Text.ToString()))
                //{
                //    maxlengthQty = Convert.ToInt32(txtfootermaxlengthQty.Text.ToString());
                //}

                if (txtDisplayorder.Text.Trim() == "")
                {
                    txtDisplayorder.Text = "999";
                }
                Int32 UPCDup = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(code) from tb_ProductFabricCode where FabricUPC='" + txtfooterupc.Text.ToString().Trim() + "' and code<>'" + txtFooterFabricCode.Text.Trim() + "' and isnull(active,0)=1 and isnull(FabricUPC,'')<>''"));
                if (UPCDup > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "DuplicatedUPC", "jAlert('UPC already exists.','Message','ContentPlaceHolder1_txtFabricTypeName');", true);
                    return;

                }
                Int32 IsAdded = objProduct.Insert_Update_Delete_FabricCode(FabricTypeID, 0, txtFooterFabricCode.Text.Trim(), txtFooterFabricName.Text.Trim(), Convert.ToInt32(txtDisplayorder.Text), chkActive.Checked, 1);

                if (IsAdded > 0)
                {
                    string StrFabricVendor = "";
                    foreach (ListItem li in ddlVendorListFooter.Items)
                    {
                        if (li.Selected)
                        {
                            StrFabricVendor += li.Value + ",";
                        }
                    }
                    if (StrFabricVendor.Length > 1)
                    {
                        StrFabricVendor = StrFabricVendor.Substring(0, StrFabricVendor.Length - 1);
                    }
                    CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET FabricVendorIds='" + StrFabricVendor.ToString() + "',FabricUPC='" + txtfooterupc.Text.ToString().Trim().Replace("'", "''") + "' WHERE Code='" + txtFooterFabricCode.Text.ToString().Replace("'", "''") + "' AND FabricTypeID=" + FabricTypeID + "");
                    CommonComponent.ExecuteCommonData("UPDATE tb_Product SET FabricVendorIds='" + StrFabricVendor.ToString() + "' WHERE FabricCode='" + txtFooterFabricCode.Text.ToString().Replace("'", "''") + "'");

                    CommonComponent.ExecuteCommonData("update tb_ProductFabricCode set YardPrice=" + txtfooterperyard.Text + " , SafetyLock=" + txtfootersafetylock.Text + ",Discontinue='" + chkfooterdiscontinue.Checked + "' where Code='" + txtFooterFabricCode.Text.ToString().Replace("'", "''") + "' AND FabricTypeID=" + FabricTypeID + "");


                    string minqty = Convert.ToString(CommonComponent.GetScalarCommonData("select MinQty from tb_FabricVendorPortal where FabricCodeId=" + IsAdded.ToString() + ""));
                    if (!string.IsNullOrEmpty(minqty))
                    {
                        CommonComponent.ExecuteCommonData("update tb_FabricVendorPortal set MinQty=" + txtfooterMinQty.Text + " where FabricCodeId=" + IsAdded.ToString() + "");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("insert into tb_FabricVendorPortal (FabricTypeID,FabricCodeId,Code,MinQty,CreatedBy,CreatedOn) values (" + FabricTypeID + "," + IsAdded + ",'" + txtFooterFabricCode.Text.Trim() + "'," + txtfooterMinQty.Text + "," + Convert.ToInt32(Session["AdminID"]) + ",getdate())");
                    }


                    // CommonComponent.ExecuteCommonData("update tb_FabricVendorPortal set MinQty=" + txtfooterMinQty.Text + " where Code='" + txtFooterFabricCode.Text.ToString().Replace("'", "''") + "' AND FabricTypeID=" + FabricTypeID + "");

                    //Int32 fabcodeid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select FabricCodeId from tb_ProductFabricCode where FabricTypeID=" + FabricTypeID + ""));
                    //CommonComponent.ExecuteCommonData("update tb_ProductFabricWidth set NoOfDays=" + txtfooterdays.Text + " where  FabricCodeId=" + IsAdded.ToString() + "");

                    Int32 FabricWeightId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ProductFabricWidth (FabricCodeID,Active,CreatedBy,CreatedOn,NoOfDays) " +
                                                    " values(" + IsAdded.ToString() + ",1," + Convert.ToInt32(Session["AdminID"]) + ",getdate()," + txtfooterdays.Text + "); Select Scope_identity()"));


                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CodeAdded", "jAlert('Fabric code added successfully.','Message');", true);
                    FillFabricEdit();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "DuplicatedCode", "jAlert('Fabric code already exists.','Message','ContentPlaceHolder1_txtFabricTypeName');", true);
                    return;
                }
            }
            else if (e.CommandName.Trim().ToLower() == "codesave")
            {
                objProduct = new ProductComponent();
                Int32 FabricCodeID = Convert.ToInt32(e.CommandArgument);
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Int32 FabricTypeID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnFabricTypeID")).Value.ToString());

                Literal ltrFabricCode = (Literal)grrow.FindControl("ltrFabricCode");
                TextBox txtFabricCode = (TextBox)grrow.FindControl("txtFabricCode");

                Literal ltrFabricName = (Literal)grrow.FindControl("ltrFabricName");
                TextBox txtFabricName = (TextBox)grrow.FindControl("txtFabricName");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");
                CheckBoxList chkvendorlist = (CheckBoxList)grrow.FindControl("chkvendorlist");
                DropDownList ddlVendorList = (DropDownList)grrow.FindControl("ddlVendorList");

                Literal ltperyard = (Literal)grrow.FindControl("ltperyard");
                TextBox txtperyard = (TextBox)grrow.FindControl("txtperyard");

                Literal ltsafetylock = (Literal)grrow.FindControl("ltsafetylock");
                TextBox txtsaftylock = (TextBox)grrow.FindControl("txtsaftylock");

                Literal ltdays = (Literal)grrow.FindControl("ltdays");
                TextBox txtdays = (TextBox)grrow.FindControl("txtdays");


                Literal ltMinQty = (Literal)grrow.FindControl("ltMinQty");
                TextBox txtMinQty = (TextBox)grrow.FindControl("txtMinQty");

                System.Web.UI.HtmlControls.HtmlImage imgdiscontinue = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgdiscontinue");
                CheckBox chkdiscontinue = (CheckBox)grrow.FindControl("chkdiscontinue");

                Literal ltupc = (Literal)grrow.FindControl("ltupc");
                TextBox txtupc = (TextBox)grrow.FindControl("txtupc");


                //Literal ltMinwidthQty = (Literal)grrow.FindControl("ltMinwidthQty");
                //TextBox txtMinwidthQty = (TextBox)grrow.FindControl("txtMinwidthQty");

                //Literal ltmaxwidthQty = (Literal)grrow.FindControl("ltmaxwidthQty");
                //TextBox txtmaxwidthQty = (TextBox)grrow.FindControl("txtmaxwidthQty");

                //Literal ltMinlengthQty = (Literal)grrow.FindControl("ltMinlengthQty");
                //TextBox txtMinlengthQty = (TextBox)grrow.FindControl("txtMinlengthQty");


                //Literal ltMaxlengthQty = (Literal)grrow.FindControl("ltMaxlengthQty");
                //TextBox txtmaxlengthQty = (TextBox)grrow.FindControl("txtmaxlengthQty");


                string StrFabricVendor = "";
                foreach (ListItem li in ddlVendorList.Items)
                {
                    if (li.Selected)
                    {
                        StrFabricVendor += li.Value + ",";
                    }
                }
                if (StrFabricVendor.Length > 1)
                {
                    StrFabricVendor = StrFabricVendor.Substring(0, StrFabricVendor.Length - 1);
                }
                if (txtdisplayorder.Text.Trim() == "")
                {
                    txtdisplayorder.Text = "999";
                }
                Int32 UPCDup = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(code) from tb_ProductFabricCode where FabricUPC='" + txtupc.Text.ToString().Trim() + "' and code<>'" + txtFabricCode.Text.Trim() + "' and isnull(active,0)=1 and isnull(FabricUPC,'')<>''"));
                if (UPCDup > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "DuplicatedUPC", "jAlert('UPC already exists.','Message','ContentPlaceHolder1_txtFabricTypeName');", true);
                    return;

                }
                if (chkActive.Checked == false)
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Ismadetoready=1,Ismadetoorder=0,Ismadetomeasure=0,IsCustom=0 WHERE FabricCode in (SELECT ISNULL(Code,'') FROM tb_ProductFabricCode WHERE FabricCodeId=" + FabricCodeID + ")");

                }

                try
                {
                    string str = "insert into tb_ProductFabricCodeLog (FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder,CreatedBy,CreatedOn,FabricVendorIds,YardPrice,SafetyLock,Discontinue,Deleted ) select FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),FabricVendorIds,YardPrice,SafetyLock,Discontinue,0  from tb_ProductFabricCode where FabricCodeId=" + FabricCodeID.ToString() + " ";
                    CommonComponent.ExecuteCommonData(str);
                }
                catch
                {

                }

                try
                {

                    string str1 = "insert into tb_ProductFabricWidthLog (FabricwidthId,FabricCodeId,Width,DisplayOrder,CreatedBy,CreatedOn,Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,Deleted,MinWidth,MaxWidth,MinLength,MaxLength) select FabricwidthId,FabricCodeId,Width,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,0,MinWidth,MaxWidth,MinLength,MaxLength from tb_ProductFabricWidth where FabricCodeId=" + FabricCodeID.ToString() + " ";
                    CommonComponent.ExecuteCommonData(str1);
                }
                catch
                {

                }

                Int32 MinwidthQty = 0;
                Int32 maxwidthQty = 0;
                Int32 minlengthQty = 0;
                Int32 maxlengthQty = 0;
                //if (!string.IsNullOrEmpty(txtMinwidthQty.Text.ToString()))
                //{
                //    MinwidthQty = Convert.ToInt32(txtMinwidthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtmaxwidthQty.Text.ToString()))
                //{
                //    maxwidthQty = Convert.ToInt32(txtmaxwidthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtMinlengthQty.Text.ToString()))
                //{
                //    minlengthQty = Convert.ToInt32(txtMinlengthQty.Text.ToString());
                //}
                //if (!string.IsNullOrEmpty(txtmaxlengthQty.Text.ToString()))
                //{
                //    maxlengthQty = Convert.ToInt32(txtmaxlengthQty.Text.ToString());
                //}

                Int32 IsUpdated = objProduct.Insert_Update_Delete_FabricCode(FabricTypeID, FabricCodeID, txtFabricCode.Text.Trim(), txtFabricName.Text.Trim(), Convert.ToInt32(txtdisplayorder.Text), chkActive.Checked, 2);
                CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET FabricVendorIds='" + StrFabricVendor.ToString() + "',FabricUPC='" + txtupc.Text.ToString().Trim().Replace("'", "''") + "' WHERE FabricCodeId=" + FabricCodeID.ToString() + "");
                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET FabricVendorIds='" + StrFabricVendor.ToString() + "' WHERE FabricCode='" + txtFabricCode.Text.ToString().Replace("'", "''") + "'");
                CommonComponent.ExecuteCommonData("update tb_ProductFabricCode set YardPrice=" + txtperyard.Text + " , SafetyLock=" + txtsaftylock.Text + ",Discontinue='" + chkdiscontinue.Checked + "' where FabricCodeId=" + FabricCodeID.ToString() + "");
                CommonComponent.ExecuteCommonData("if(EXISTS(SELECT top 1 FabricCodeId FROM tb_ProductFabricWidth WHERE FabricCodeId=" + FabricCodeID.ToString() + "))begin update tb_ProductFabricWidth set  NoOfDays=" + txtdays.Text + " where  FabricCodeId=" + FabricCodeID.ToString() + " end else begin Insert into tb_ProductFabricWidth (FabricCodeID,Active,CreatedBy,CreatedOn,NoOfDays) " +
                                                    " values(" + FabricCodeID.ToString() + ",1," + Convert.ToInt32(Session["AdminID"]) + ",getdate()," + txtdays.Text + ");  end");


                Int32 FabricCodeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select FabricCodeId from tb_FabricVendorPortal where FabricCodeId=" + FabricCodeID.ToString() + ""));
                if (FabricCodeId != null && FabricCodeId > 0)
                {
                    CommonComponent.ExecuteCommonData("update tb_FabricVendorPortal set MinQty=" + txtMinQty.Text + " where FabricCodeId=" + FabricCodeID.ToString() + "");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("insert into tb_FabricVendorPortal (FabricTypeID,FabricCodeId,Code,MinQty,CreatedBy,CreatedOn) values (" + FabricTypeID + "," + FabricCodeID + ",'" + txtFabricCode.Text.Trim() + "'," + txtMinQty.Text + "," + Convert.ToInt32(Session["AdminID"]) + ",getdate())");
                }




                if (IsUpdated > 0)
                {
                    ltperyard.Visible = true;
                    txtperyard.Visible = false;

                    ltMinQty.Visible = true;
                    txtMinQty.Visible = false;

                    ltsafetylock.Visible = true;
                    txtsaftylock.Visible = false;

                    ltdays.Visible = true;
                    txtdays.Visible = false;

                    imgdiscontinue.Visible = true;
                    chkdiscontinue.Visible = false;


                    ltrFabricCode.Visible = true;
                    txtFabricCode.Visible = false;

                    ltrFabricName.Visible = true;
                    txtFabricName.Visible = false;

                    imgCodeActive.Visible = true;
                    chkActive.Visible = false;

                  //  ltMinwidthQty.Visible = true;
                  //  txtMinwidthQty.Visible = false;

                 //   ltmaxwidthQty.Visible = true;
                 //   txtmaxwidthQty.Visible = false;

                  //  ltMinlengthQty.Visible = true;
                  //  txtMinlengthQty.Visible = false;

                  //  ltMaxlengthQty.Visible = true;
                  //  txtmaxlengthQty.Visible = false;



                    //ltdisplayorder.Visible = true;
                    //txtdisplayorder.Visible = false;

                    imgSave.Visible = false;
                    imgcancel.Visible = false;
                    imgEdit.Visible = true;
                    ltupc.Visible = true;
                    txtupc.Visible = false;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CodeUpdated", "jAlert('Fabric code updated successfully.','Message');", true);
                    FillFabricEdit();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbCodeUpdated", "jAlert('Fabric code already exists.','Message');", true);
                }
            }
            else if (e.CommandName.Trim().ToLower() == "codeedit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                DropDownList ddlVendorList = (DropDownList)grrow.FindControl("ddlVendorList");
                Literal ltrFabricCode = (Literal)grrow.FindControl("ltrFabricCode");
                TextBox txtFabricCode = (TextBox)grrow.FindControl("txtFabricCode");

                Literal ltrFabricName = (Literal)grrow.FindControl("ltrFabricName");
                TextBox txtFabricName = (TextBox)grrow.FindControl("txtFabricName");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                //Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                //TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");

                Literal ltperyard = (Literal)grrow.FindControl("ltperyard");
                TextBox txtperyard = (TextBox)grrow.FindControl("txtperyard");

                Literal ltsafetylock = (Literal)grrow.FindControl("ltsafetylock");
                TextBox txtsaftylock = (TextBox)grrow.FindControl("txtsaftylock");

                Literal ltdays = (Literal)grrow.FindControl("ltdays");
                TextBox txtdays = (TextBox)grrow.FindControl("txtdays");


                Literal ltMinQty = (Literal)grrow.FindControl("ltMinQty");
                TextBox txtMinQty = (TextBox)grrow.FindControl("txtMinQty");
                Literal ltupc = (Literal)grrow.FindControl("ltupc");
                TextBox txtupc = (TextBox)grrow.FindControl("txtupc");

                System.Web.UI.HtmlControls.HtmlImage imgdiscontinue = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgdiscontinue");
                CheckBox chkdiscontinue = (CheckBox)grrow.FindControl("chkdiscontinue");

                //Literal ltMinwidthQty = (Literal)grrow.FindControl("ltMinwidthQty");
                //TextBox txtMinwidthQty = (TextBox)grrow.FindControl("txtMinwidthQty");

                //Literal ltmaxwidthQty = (Literal)grrow.FindControl("ltmaxwidthQty");
                //TextBox txtmaxwidthQty = (TextBox)grrow.FindControl("txtmaxwidthQty");

                //Literal ltMinlengthQty = (Literal)grrow.FindControl("ltMinlengthQty");
                //TextBox txtMinlengthQty = (TextBox)grrow.FindControl("txtMinlengthQty");


                //Literal ltMaxlengthQty = (Literal)grrow.FindControl("ltMaxlengthQty");
                //TextBox txtmaxlengthQty = (TextBox)grrow.FindControl("txtmaxlengthQty");

                //ltMinwidthQty.Visible = false;
                //txtMinwidthQty.Visible = true;

                //ltmaxwidthQty.Visible = false;
                //txtmaxwidthQty.Visible = true;

                //ltMinlengthQty.Visible = false;
                //txtMinlengthQty.Visible = true;

                //ltMaxlengthQty.Visible = false;
                //txtmaxlengthQty.Visible = true;


                ltperyard.Visible = false;
                txtperyard.Visible = true;


                ltMinQty.Visible = false;
                txtMinQty.Visible = true;



                ltsafetylock.Visible = false;
                txtsaftylock.Visible = true;

                ltdays.Visible = false;
                txtdays.Visible = true;

                imgdiscontinue.Visible = false;
                chkdiscontinue.Visible = true;

                ddlVendorList.Enabled = true;
                ltupc.Visible = false;
                txtupc.Visible = true;

                string strScript = "javascript:if(document.getElementById('" + txtFabricCode.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric code.','Message','" + txtFabricCode.ClientID + "'); return false;} if(document.getElementById('" + txtFabricName.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric Name.','Message','" + txtFabricName.ClientID + "'); return false;} return checkfabricCode(" + grrow.RowIndex + ");";
                imgSave.OnClientClick = strScript;

                ltrFabricCode.Visible = false;
                txtFabricCode.Visible = true;
                txtFabricCode.Text = ltrFabricCode.Text;

                ltrFabricName.Visible = false;
                txtFabricName.Visible = true;
                txtFabricName.Text = ltrFabricName.Text;

                imgCodeActive.Visible = false;
                chkActive.Visible = true;

                //ltdisplayorder.Visible = false;
                //txtdisplayorder.Visible = true;
                //txtdisplayorder.Text = txtdisplayorder.Text;

                imgSave.Visible = true;
                imgcancel.Visible = true;
                imgEdit.Visible = false;
            }
            else if (e.CommandName.Trim().ToLower() == "codeexit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricCode = (Literal)grrow.FindControl("ltrFabricCode");
                TextBox txtFabricCode = (TextBox)grrow.FindControl("txtFabricCode");

                Literal ltrFabricName = (Literal)grrow.FindControl("ltrFabricName");
                TextBox txtFabricName = (TextBox)grrow.FindControl("txtFabricName");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");

                Literal ltperyard = (Literal)grrow.FindControl("ltperyard");
                TextBox txtperyard = (TextBox)grrow.FindControl("txtperyard");

                Literal ltsafetylock = (Literal)grrow.FindControl("ltsafetylock");
                TextBox txtsaftylock = (TextBox)grrow.FindControl("txtsaftylock");

                Literal ltdays = (Literal)grrow.FindControl("ltdays");
                TextBox txtdays = (TextBox)grrow.FindControl("txtdays");
                Literal ltupc = (Literal)grrow.FindControl("ltupc");
                TextBox txtupc = (TextBox)grrow.FindControl("txtupc");

                Literal ltMinQty = (Literal)grrow.FindControl("ltMinQty");
                TextBox txtMinQty = (TextBox)grrow.FindControl("txtMinQty");


                //Literal ltMinwidthQty = (Literal)grrow.FindControl("ltMinwidthQty");
                //TextBox txtMinwidthQty = (TextBox)grrow.FindControl("txtMinwidthQty");

                //Literal ltmaxwidthQty = (Literal)grrow.FindControl("ltmaxwidthQty");
                //TextBox txtmaxwidthQty = (TextBox)grrow.FindControl("txtmaxwidthQty");

                //Literal ltMinlengthQty = (Literal)grrow.FindControl("ltMinlengthQty");
                //TextBox txtMinlengthQty = (TextBox)grrow.FindControl("txtMinlengthQty");


                //Literal ltMaxlengthQty = (Literal)grrow.FindControl("ltMaxlengthQty");
                //TextBox txtmaxlengthQty = (TextBox)grrow.FindControl("txtmaxlengthQty");


                //ltMinwidthQty.Visible = true;
                //txtMinwidthQty.Visible = false;

                //ltmaxwidthQty.Visible = true;
                //txtmaxwidthQty.Visible = false;

                //ltMinlengthQty.Visible = true;
                //txtMinlengthQty.Visible = false;

                //ltMaxlengthQty.Visible = true;
                //txtmaxlengthQty.Visible = false;



                txtupc.Visible = false;

                ltupc.Visible = true;

                System.Web.UI.HtmlControls.HtmlImage imgdiscontinue = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgdiscontinue");
                CheckBox chkdiscontinue = (CheckBox)grrow.FindControl("chkdiscontinue");




                ltperyard.Visible = true;
                txtperyard.Visible = false;


                ltMinQty.Visible = true;
                txtMinQty.Visible = false;

                ltsafetylock.Visible = true;
                txtsaftylock.Visible = false;

                ltdays.Visible = true;
                txtdays.Visible = false;

                imgdiscontinue.Visible = true;
                chkdiscontinue.Visible = false;

                ltrFabricCode.Visible = true;
                txtFabricCode.Visible = false;

                ltrFabricName.Visible = true;
                txtFabricName.Visible = false;

                imgCodeActive.Visible = true;
                chkActive.Visible = false;

                //ltdisplayorder.Visible = true;
                //txtdisplayorder.Visible = false;

                imgSave.Visible = false;
                imgcancel.Visible = false;
                imgEdit.Visible = true;
            }
            else if (e.CommandName.Trim().ToLower() == "coderemove")
            {
                objProduct = new ProductComponent();
                Int32 FabricCodeID = Convert.ToInt32(e.CommandArgument);



                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Ismadetoready=1,Ismadetoorder=0,Ismadetomeasure=0,IsCustom=0, FabricVendorIds='',FabricCode='' WHERE FabricCode in (SELECT ISNULL(Code,'') FROM tb_ProductFabricCode WHERE FabricCodeId=" + FabricCodeID + ")");
                Int32 IsDeleted = objProduct.Insert_Update_Delete_FabricCode(0, FabricCodeID, "", "", 0, Convert.ToBoolean(0), 3);



                try
                {

                    string str1 = "insert into tb_ProductFabricWidthLog (FabricwidthId,FabricCodeId,Width,DisplayOrder,CreatedBy,CreatedOn,Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,Deleted,MinWidth,MaxWidth,MinLength,MaxLength) select FabricwidthId,FabricCodeId,Width,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,1,MinWidth,MaxWidth,MinLength,MaxLength from tb_ProductFabricWidth where FabricCodeId=" + FabricCodeID.ToString() + " ";
                    CommonComponent.ExecuteCommonData(str1);
                }
                catch
                {

                }

                CommonComponent.ExecuteCommonData("delete from tb_ProductFabricWidth where FabricCodeId=" + FabricCodeID.ToString() + "");

                if (IsDeleted > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeDeleted", "jAlert('Fabric code deleted successfully.','Message');", true);
                    FillFabricEdit();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbTypeDeleted", "jAlert('Problem while deleting fabric code.','Message');", true);
                }
            }
        }
        protected void grdFabricCode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes.Add("style", "border-right:none;");
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "border-left:none;");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[e.Row.Cells.Count - 2].Attributes.Add("style", "border-right:none;");
                e.Row.Cells[e.Row.Cells.Count - 1].Attributes.Add("style", "border-left:none;");

                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FabricCodeId")) == 0)
                {
                    e.Row.Visible = false;
                }
                else
                {
                    GridView grdFabricWidth = (GridView)e.Row.FindControl("grdFabricWidth");
                    Int32 FabricCodeID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FabricCodeId"));
                    DataSet dsFWidth = new DataSet();
                    objProduct = new ProductComponent();
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnvendorids = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnvendorids");
                    CheckBoxList chkvendorlist = (CheckBoxList)e.Row.FindControl("chkvendorlist");
                    DropDownList ddlVendorList = (DropDownList)e.Row.FindControl("ddlVendorList");
                    ImageButton imgSave = (ImageButton)e.Row.FindControl("imgSave");
                    //  imgSave.OnClientClick = "return checkfabricCode("+e.Row.RowIndex+");";

                    // chkvendorlist.Items.Clear();
                    ddlVendorList.Items.Clear();
                    if (ViewState["dsdropshipsku"] != null)
                    {
                        DataSet dsVendor = new DataSet();
                        dsVendor = (DataSet)ViewState["dsdropshipsku"];
                        if (dsVendor != null && dsVendor.Tables.Count > 0 && dsVendor.Tables[0].Rows.Count > 0)
                        {
                            ddlVendorList.DataSource = dsVendor.Tables[0];
                            ddlVendorList.DataTextField = "Name";
                            ddlVendorList.DataValueField = "VendorID";
                            ddlVendorList.DataBind();
                        }
                    }
                    if (hdnvendorids != null && !string.IsNullOrEmpty(hdnvendorids.Value) && hdnvendorids.Value.ToString() != "")
                    {
                        string StrFabricVendor = "," + Convert.ToString(hdnvendorids.Value) + ",";
                        if (StrFabricVendor.Length > 0 && StrFabricVendor.ToString().ToLower().IndexOf(",") > -1)
                        {
                            for (int i = 0; i < ddlVendorList.Items.Count; i++)
                            {
                                // ddlVendorList.Items[i].Text = "&nbsp;" + ddlVendorList.Items[i].Text;
                                ddlVendorList.Items[i].Text = ddlVendorList.Items[i].Text;
                                if (StrFabricVendor.ToString().IndexOf("," + ddlVendorList.Items[i].Value.ToString() + ",") > -1)
                                {
                                    ddlVendorList.Items[i].Selected = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ddlVendorList.Items.Count; i++)
                        {
                            ddlVendorList.Items[i].Text = ddlVendorList.Items[i].Text;
                        }
                    }
                    dsFWidth = objProduct.GetProductFabricWidth(FabricCodeID);
                    ViewState["FabricCodeID"] = FabricCodeID.ToString();
                    if (dsFWidth != null && dsFWidth.Tables.Count > 0 && dsFWidth.Tables[0].Rows.Count > 0)
                    {
                        grdFabricWidth.DataSource = dsFWidth;
                        grdFabricWidth.DataBind();
                    }
                    else
                    {
                        grdFabricWidth.DataSource = null;
                        grdFabricWidth.DataBind();
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFooterFabricCode = (TextBox)e.Row.FindControl("txtFooterFabricCode");
                TextBox txtFooterFabricName = (TextBox)e.Row.FindControl("txtFooterFabricName");
                LinkButton lnkAdd = (LinkButton)e.Row.FindControl("lnkAdd");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnFooterFabricTypeID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnFooterFabricTypeID");
                hdnFooterFabricTypeID.Value = Convert.ToString(ViewState["FabricTypeID"]);
                string strScript = "javascript:if(document.getElementById('" + txtFooterFabricCode.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric code.','Message','" + txtFooterFabricCode.ClientID + "'); return false;}if(document.getElementById('" + txtFooterFabricName.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric name.','Message','" + txtFooterFabricName.ClientID + "'); return false;} return true;";
                lnkAdd.OnClientClick = strScript;
                DropDownList ddlVendorListFooter = (DropDownList)e.Row.FindControl("ddlVendorListFooter");
                CheckBoxList chkvendorlistfooter = (CheckBoxList)e.Row.FindControl("chkvendorlistfooter");
                ddlVendorListFooter.Items.Clear();
                if (ViewState["dsdropshipsku"] != null)
                {
                    DataSet dsVendor = new DataSet();
                    dsVendor = (DataSet)ViewState["dsdropshipsku"];
                    if (dsVendor != null && dsVendor.Tables.Count > 0 && dsVendor.Tables[0].Rows.Count > 0)
                    {
                        ddlVendorListFooter.DataSource = dsVendor.Tables[0];
                        ddlVendorListFooter.DataTextField = "Name";
                        ddlVendorListFooter.DataValueField = "VendorID";
                        ddlVendorListFooter.DataBind();
                    }
                    for (int i = 0; i < ddlVendorListFooter.Items.Count; i++)
                    {
                        ddlVendorListFooter.Items[i].Text = ddlVendorListFooter.Items[i].Text;
                    }
                }
            }
        }
        protected void grdFabricCode_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void grdFabricCode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion

        #region Code for Fabric Width

        protected void grdFabricWidth_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Trim().ToLower() == "addwidth")
            {
                GridView gvTemp = (GridView)sender;
                Int32 hdnFabricCodeID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)gvTemp.FooterRow.FindControl("hdnFooterFabricCodeID")).Value.ToString());
                objProduct = new ProductComponent();

                TextBox txtFooterFabricWidth = (TextBox)gvTemp.FooterRow.FindControl("txtFooterFabricWidth");
                CheckBox chkActive = (CheckBox)gvTemp.FooterRow.FindControl("chkFooterActive");
                TextBox txtDisplayorder = (TextBox)gvTemp.FooterRow.FindControl("txtDisplayorder");

                if (txtDisplayorder.Text.Trim() == "")
                {
                    txtDisplayorder.Text = "999";
                }
                Int32 IsAdded = objProduct.Insert_Update_Delete_FabricWidth(hdnFabricCodeID, 0, txtFooterFabricWidth.Text.Trim(), Convert.ToInt32(txtDisplayorder.Text), chkActive.Checked, 1);

                if (IsAdded > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CodeAdded", "jAlert('Fabric Width added successfully.','Message');", true);
                    FillFabric();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "DuplicatedCode", "jAlert('Fabric Width already exists.','Message','" + txtFooterFabricWidth.ClientID + "');", true);
                    return;
                }
            }
            else if (e.CommandName.Trim().ToLower() == "widthsave")
            {
                objProduct = new ProductComponent();
                Int32 FabricCodeID = Convert.ToInt32(e.CommandArgument);
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                Int32 hdnFabricWidthID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnFabricWidthID")).Value.ToString());
                Int32 hdnFabricCodeID = Convert.ToInt32(((System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnFabricCodeID")).Value.ToString());

                Literal ltrFabricWidth = (Literal)grrow.FindControl("ltrFabricWidth");
                TextBox txtFabricWidth = (TextBox)grrow.FindControl("txtFabricWidth");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");

                //if (txtdisplayorder.Text.Trim() == "")
                //{
                //    txtdisplayorder.Text = "999";
                //}

                Int32 IsUpdated = objProduct.Insert_Update_Delete_FabricWidth(hdnFabricCodeID, hdnFabricWidthID, txtFabricWidth.Text.Trim(), Convert.ToInt32(txtdisplayorder.Text), chkActive.Checked, 2);

                if (IsUpdated > 0)
                {
                    ltrFabricWidth.Visible = true;
                    txtFabricWidth.Visible = false;

                    imgCodeActive.Visible = true;
                    chkActive.Visible = false;

                    //ltdisplayorder.Visible = true;
                    //txtdisplayorder.Visible = false;

                    imgSave.Visible = false;
                    imgcancel.Visible = false;
                    imgEdit.Visible = true;

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "widthUpdated", "jAlert('Fabric width updated successfully.','Message');", true);
                    FillFabric();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbwidthUpdated", "jAlert('Fabric width already exists.','Message');", true);
                }
            }
            else if (e.CommandName.Trim().ToLower() == "widthedit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricWidth = (Literal)grrow.FindControl("ltrFabricWidth");
                TextBox txtFabricWidth = (TextBox)grrow.FindControl("txtFabricWidth");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                //Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                //TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");


                string strScript = "javascript:if(document.getElementById('" + txtFabricWidth.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric width.','Message','" + txtFabricWidth.ClientID + "'); return false;} return false;} return true;";
                imgSave.OnClientClick = strScript;

                ltrFabricWidth.Visible = false;
                txtFabricWidth.Visible = true;
                txtFabricWidth.Text = ltrFabricWidth.Text;

                imgCodeActive.Visible = false;
                chkActive.Visible = true;

                //ltdisplayorder.Visible = false;
                //txtdisplayorder.Visible = true;
                //txtdisplayorder.Text = txtdisplayorder.Text;

                imgSave.Visible = true;
                imgcancel.Visible = true;
                imgEdit.Visible = false;
            }
            else if (e.CommandName.Trim().ToLower() == "widthexit")
            {
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                Literal ltrFabricWidth = (Literal)grrow.FindControl("ltrFabricWidth");
                TextBox txtFabricWidth = (TextBox)grrow.FindControl("txtFabricWidth");

                System.Web.UI.HtmlControls.HtmlImage imgCodeActive = (System.Web.UI.HtmlControls.HtmlImage)grrow.FindControl("imgCodeActive");
                CheckBox chkActive = (CheckBox)grrow.FindControl("chkActive");

                //Literal ltdisplayorder = (Literal)grrow.FindControl("ltdisplayorder");
                //TextBox txtdisplayorder = (TextBox)grrow.FindControl("txtdisplayorder");

                ImageButton imgSave = (ImageButton)grrow.FindControl("imgSave");
                ImageButton imgEdit = (ImageButton)grrow.FindControl("imgEdit");
                ImageButton imgcancel = (ImageButton)grrow.FindControl("imgcancel");
                ImageButton imgDelete = (ImageButton)grrow.FindControl("imgDelete");

                ltrFabricWidth.Visible = true;
                txtFabricWidth.Visible = false;

                imgCodeActive.Visible = true;
                chkActive.Visible = false;

                //ltdisplayorder.Visible = true;
                //txtdisplayorder.Visible = false;

                imgSave.Visible = false;
                imgcancel.Visible = false;
                imgEdit.Visible = true;
            }
            else if (e.CommandName.Trim().ToLower() == "removewidth")
            {
                objProduct = new ProductComponent();
                Int32 FabricWidthID = Convert.ToInt32(e.CommandArgument);
                Int32 IsDeleted = objProduct.Insert_Update_Delete_FabricWidth(0, FabricWidthID, "", 0, Convert.ToBoolean(0), 3);
                if (IsDeleted > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "TypeDeleted", "jAlert('Fabric Width deleted successfully.','Message');", true);
                    FillFabric();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ProbTypeDeleted", "jAlert('Problem while deleting fabric Width.','Message');", true);
                }
            }
        }
        protected void grdFabricWidth_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FabricWidthID")) == 0)
                {
                    e.Row.Visible = false;
                }
                else
                {
                    Int32 FabricCodeID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FabricCodeId"));
                    ViewState["FabricCodeID"] = FabricCodeID.ToString();
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox txtFooterFabricWidth = (TextBox)e.Row.FindControl("txtFooterFabricWidth");
                LinkButton lnkAdd = (LinkButton)e.Row.FindControl("lnkAdd");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnFooterFabricCodeID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnFooterFabricCodeID");
                hdnFooterFabricCodeID.Value = Convert.ToString(ViewState["FabricCodeID"]);
                string strScript = "javascript:if(document.getElementById('" + txtFooterFabricWidth.ClientID + "').value.replace(/^\\s*\\s*$/g, '') == ''){jAlert('Please enter fabric Width.','Message','" + txtFooterFabricWidth.ClientID + "'); return false;} return true;";
                lnkAdd.OnClientClick = strScript;
            }
        }
        protected void grdFabricWidth_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void grdFabricWidth_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        #endregion
        protected void btnSavevendor_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow gr in grdFabricType.Rows)
            {
                GridView grdFabricCode = (GridView)gr.FindControl("grdFabricCode");
                foreach (GridViewRow grcode in grdFabricCode.Rows)
                {
                    string StrFabricVendor = "";
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnFabricCodeID = (System.Web.UI.HtmlControls.HtmlInputHidden)grcode.FindControl("hdnFabricCodeID");
                    Literal ltrFabricCode = (Literal)grcode.FindControl("ltrFabricCode");
                    CheckBoxList chkvendorlist = (CheckBoxList)grcode.FindControl("chkvendorlist");
                    DropDownList ddlVendorList = (DropDownList)grcode.FindControl("ddlVendorList");
                    if (hdnFabricCodeID != null && hdnFabricCodeID.Value.ToString() != "" && hdnFabricCodeID.Value.ToString() != "0")
                    {

                        foreach (ListItem li in ddlVendorList.Items)
                        {
                            if (li.Selected)
                            {
                                StrFabricVendor += li.Value + ",";
                            }
                        }
                        if (StrFabricVendor.Length > 1)
                        {
                            StrFabricVendor = StrFabricVendor.Substring(0, StrFabricVendor.Length - 1);
                        }
                        CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET FabricVendorIds='" + StrFabricVendor.ToString() + "' WHERE FabricCodeId=" + hdnFabricCodeID.Value.ToString() + "");
                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET FabricVendorIds='" + StrFabricVendor.ToString() + "' WHERE FabricCode='" + ltrFabricCode.Text.ToString().Replace("'", "''") + "'");
                    }

                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillFabricType();
        }

        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

            //if (Session["FabricVendorData"] != null)
            //{
            CommonComponent clsCommon = new CommonComponent();
            DataView dvCust = new DataView();

            DataSet ds = new DataSet();
            // ds = (DataSet)Session["FabricVendorData"];
            int FabricTypeID = 0;
            if (ddlFabricType.SelectedIndex > 0)
            {
                FabricTypeID = Convert.ToInt32(ddlFabricType.SelectedValue.ToString());
            }
            ds = CommonComponent.GetCommonDataSet("Exec usp_Product_ProductFabricDetails " + FabricTypeID + ",0,6,'" + txtSearch.Text.ToString().Replace("'", "''") + "'");

            if (ds == null)
            {
                if (Session["FabricVendorData"] != null)
                {
                    ds = (DataSet)Session["FabricVendorData"];
                }
            }

            DataSet Dsvendors = new DataSet();
            Dsvendors = CommonComponent.GetCommonDataSet("select vendorid,name from tb_vendor where isnull(active,0)=1 and isnull(deleted,0)=0 and vendorid in (3,5)");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("vendorname", typeof(string));
                ds.AcceptChanges();
                if (Dsvendors != null && Dsvendors.Tables.Count > 0 && Dsvendors.Tables[0].Rows.Count > 0)
                {

                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(ds.Tables[0].Rows[k]["FabricVendorIds"].ToString()))
                            {

                                DataRow[] dr = Dsvendors.Tables[0].Select("vendorid in (" + ds.Tables[0].Rows[k]["FabricVendorIds"].ToString() + ")");
                                if (dr.Length > 0)
                                {
                                    ds.Tables[0].Rows[k]["Vendorname"] = dr[0]["name"].ToString();
                                    ds.Tables[0].AcceptChanges();
                                }
                                else
                                {
                                    ds.Tables[0].Rows[k]["Vendorname"] = "Sierra";
                                    ds.Tables[0].AcceptChanges();
                                }
                            }
                        }
                        catch { }

                    }
                }
            }


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dvCust = ds.Tables[0].DefaultView;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (dvCust != null)
                {
                    for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dvCust.Table.Rows[i]["Code"].ToString()))
                        {
                            object[] args = new object[23];
                            args[0] = Convert.ToString(dvCust.Table.Rows[i]["Code"]);
                            args[1] = Convert.ToString(dvCust.Table.Rows[i]["Name"].ToString());
                            args[2] = Convert.ToString(dvCust.Table.Rows[i]["SafetyLock"].ToString());
                            args[3] = Convert.ToString(dvCust.Table.Rows[i]["Minqty"].ToString());

                            args[4] = Convert.ToString(dvCust.Table.Rows[i]["NoofDays"].ToString());

                            args[5] = Convert.ToString(dvCust.Table.Rows[i]["YardPrice"].ToString());
                            string Dis = "";
                            int Discontinue = 0;
                            Discontinue = Convert.ToInt32(dvCust.Table.Rows[i]["Discontinue"].ToString());
                            if (Discontinue == 0)
                            {
                                Dis = "No";
                            }
                            else
                            {
                                Dis = "Yes";
                            }
                            int act = 0;
                            string active = "";
                            act = Convert.ToInt32(dvCust.Table.Rows[i]["active"].ToString());
                            if (act == 0)
                            {
                                active = "No";
                            }
                            else
                            {
                                active = "Yes";
                            }
                            args[6] = Convert.ToString(Dis);
                            args[7] = Convert.ToString(active);
                            args[8] = Convert.ToString(dvCust.Table.Rows[i]["Vendorname"].ToString());

                            args[9] = Convert.ToString(dvCust.Table.Rows[i]["fabricupc"].ToString());
                            args[10] = Convert.ToString(dvCust.Table.Rows[i]["MinWidth"].ToString());
                            args[11] = Convert.ToString(dvCust.Table.Rows[i]["MaxWidth"].ToString());
                            args[12] = Convert.ToString(dvCust.Table.Rows[i]["MinLength"].ToString());
                            args[13] = Convert.ToString(dvCust.Table.Rows[i]["MaxLength"].ToString());

                            sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"", args));
                        }

                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "FabricVendorProductsData_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    //       sb.AppendLine("Fabric Code,Name,Safety Lock,Min. Alert Qty,Delivery Days,Per yard cost,Qty on hand1,Balance Qty1,Prod. Date1,Qty on hand2,Balance Qty2,Prod. Date2,Qty on hand3,Balance Qty3,Prod. Date3,Qty on hand4,Balance Qty4,Prod. Date4");
                    sb.AppendLine("Fabric Code,Name,Safety Lock,Min Alert Qty,Delivery Days,Per Yard Retail Price,Discontinue,Active,Vendor,UPC,MinWidth,MaxWidth,MinLength,MaxLength");
                    sb.AppendLine(FullString);

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }

            //}
        }
    }
}