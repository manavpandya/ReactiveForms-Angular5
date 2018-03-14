using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class TradeCustomerTemplate : BasePage
    {
        #region Local Variables
        StoreComponent objStorecomponent = null;
        CustomerComponent objCustomer = null;
        public Int32 CustomerID = 0;


        public bool DiscountProductStructure = false;
        public bool DiscountCategoryStructure = false;
        public bool DiscountBrandStructure = false;

        DataSet dsProduct = new DataSet();
        DataSet dsCategory = new DataSet();
        DataSet dsBrand = new DataSet();

        public DataTable dt;

        #endregion


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

                bindstore();



                if (Request.QueryString["Mode"] != null && Convert.ToString(Request.QueryString["Mode"]).Trim().ToLower() == "edit")
                {


                    lblHeader.Text = "Update Trade Customer Template";
                    FillTradeTemplateData();

                    ddlStoreName.Enabled = false;



                }
                else
                {
                    lblHeader.Text = "Add Trade Customer Template";


                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "trcoupcode", "if(document.getElementById('trcoupcode') != null){document.getElementById('trcoupcode').style.display='none'; }", true);

                }
                FillTradeTemplateData();
                BinCategoryDiscountDeatailbyCustID();
                BinProductDiscountDeatailbyCustID();


            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "SetUploadDocVisible();", true);
            }
            Page.MaintainScrollPositionOnPostBack = true;
            Page.Form.DefaultButton = imgSave.UniqueID;



        }


        private void FillTradeTemplateData()
        {
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit" && Request.QueryString["TempID"] != null && Request.QueryString["TempID"].ToString() != "")
            {
                Int32 TemplateID = Convert.ToInt32(Request.QueryString["TempID"].ToString());
                DataSet dsTrade = new DataSet();
                dsTrade = CommonComponent.GetCommonDataSet("select TradeTemplateID,isnull(TradeTempName,'') as TradeTempName,isnull(Active,0) as Active from tb_TradeTempMaster where TradeTemplateID=" + TemplateID + "");
                if (dsTrade != null && dsTrade.Tables.Count > 0 && dsTrade.Tables[0].Rows.Count > 0)
                {
                    txtTemplatename.Text = dsTrade.Tables[0].Rows[0]["TradeTempName"].ToString();
                    if (dsTrade.Tables[0].Rows[0]["Active"].ToString().ToLower() == "0" || dsTrade.Tables[0].Rows[0]["Active"].ToString().ToLower() == "true")
                    {
                        chkActiveTemplate.Checked = true;
                    }
                    else
                    {
                        chkActiveTemplate.Checked = false;
                    }



                }
            }
        }


        /// <summary>


        /// <summary>
        /// Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {


            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit" && Request.QueryString["TempID"] != null && Request.QueryString["TempID"].ToString() != "")
            {
                string tradeTemplatename = "";
                if (!String.IsNullOrEmpty(txtTemplatename.Text.ToString()))
                {
                    Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(TradeTemplateID) from tb_TradeTempMaster where isnull(deleted,0)=0 and TradeTempName='" + txtTemplatename.Text.ToString().Replace("'", "''") + "' and TradeTemplateID<>" + Request.QueryString["TempID"].ToString() + ""));
                    if (count <= 0)
                    {
                        CommonComponent.ExecuteCommonData("update tb_TradeTempMaster set TradeTempName='" + txtTemplatename.Text.ToString().Replace("'", "''") + "',Active='" + chkActiveTemplate.Checked + "',UpdatedBy=" + Convert.ToInt32(Session["AdminID"].ToString()) + ",UpdatedOn=getdate() where TradeTemplateID=" + Request.QueryString["TempID"].ToString() + "");
                        UpdateMemberShipDisCount(Convert.ToInt32(Request.QueryString["TempID"].ToString()));
                        CheckmembershipDiscount(Convert.ToInt32(Request.QueryString["TempID"].ToString()));
                        Response.Redirect("TradeTemplateList.aspx?status=updated", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Trade Template Name already exist with same Template Name.', 'Message');});", true);
                        return;
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Please Enter Template Name.', 'Message');});", true);
                    return;
                }






            }
            else
            {



                string tradeTemplatename = "";
                if (!String.IsNullOrEmpty(txtTemplatename.Text.ToString()))
                {
                    Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(TradeTemplateID) from tb_TradeTempMaster where isnull(deleted,0)=0 and  TradeTempName='" + txtTemplatename.Text.ToString().Replace("'", "''") + "'"));
                    if (count <= 0)
                    {
                        string Inserttemplate = @"INSERT INTO [dbo].[tb_TradeTempMaster]
           ([TradeTempName]
           ,[Active]
           ,[Deleted]
           ,[CreatedBy]
           ,[CreatedOn]) VALUES ('" + txtTemplatename.Text.ToString().Replace("'", "''") + "','" + chkActiveTemplate.Checked + "',0," + Convert.ToInt32(Session["AdminID"].ToString()) + ",getdate());SELECT SCOPE_IDENTITY();";
                        Int32 templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData(Inserttemplate));
                        if (templateid > 0)
                        {
                            InsertMemberShipDisCount(templateid);
                            CheckmembershipDiscount(templateid);
                            Response.Redirect("TradeTemplateList.aspx?status=inserted", true);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert2", "$(document).ready( function() {jAlert('Problem in inserting Trade Template, please try again.', 'Message');});", true);
                            return;
                        }

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Trade Template Name already exist with same Template Name.', 'Message');});", true);
                        return;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Please Enter Template Name.', 'Message');});", true);
                    return;
                }



                //if (CustomerAdded == -1)
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                //    return;
                //}
                //if (CustomerAdded > 0)
                //{
                //    if (ViewState["IsPasswordChanged"] != null && ViewState["IsPasswordChanged"].ToString() == "1")
                //    {
                //        try
                //        {
                //            SendMail();
                //        }
                //        catch { }
                //    }

                //    if (InsertBillingAddress(0, CustomerAdded) && InsertShippingAddress(0, CustomerAdded))
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Insert", "$(document).ready( function() {jAlert('Customer Inserted Successfully.', 'Message');});", true);
                //        if (Request.QueryString["rtn"] != null && Request.QueryString["rtn"].ToString() != "")
                //        {
                //            Response.Redirect("/Admin/Orders/Orders.aspx?id=" + Request.QueryString["rtn"].ToString() + "", true);
                //        }
                //        else if (Request.QueryString["Quoted"] != null)
                //        {
                //            if (Request.QueryString["Quoted"].ToString().Trim() == "0")
                //                Response.Redirect("CustomerQuote.aspx?CustID=" + CustomerAdded);
                //            else Response.Redirect("CustomerQuote.aspx?CustID=" + CustomerAdded + "Mode=edit&&ID=" + Request.QueryString["Quoted"].ToString().Trim());
                //        }
                //        else
                //        {

                //        }

                //        Response.Redirect("CustomerList.aspx?status=inserted", true);
                //    }
                //    else
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert1", "$(document).ready( function() {jAlert('Problem in inserting billing and shipping address, please try again.', 'Message');});", true);
                //        return;
                //    }
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert2", "$(document).ready( function() {jAlert('Problem in inserting customer, please try again.', 'Message');});", true);
                //    return;
                //}
            }
        }



        /// <summary>
        /// Cancel button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("TradeTemplateList.aspx", true);


            Session["TempDsCatDiscount"] = null;
            Session["TempDsProdDiscount"] = null;


        }


        /// <summary>
        /// Bind store dropdown
        /// </summary>
        private void bindstore()
        {
            objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail != null && storeDetail.Count > 0)
            {
                ddlStoreName.DataSource = storeDetail;
                ddlStoreName.DataTextField = "StoreName";
                ddlStoreName.DataValueField = "StoreID";
                ddlStoreName.DataBind();
            }
            ddlStoreName.SelectedIndex = 0;
        }


        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
        {

            AppConfig.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue.ToString());
        }






        protected void BindCategoryDetails(Int32 CustomerLevelID, Int32 StoreId)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            if (hdnCatWiseDiscountids != null && hdnCatWiseDiscountids.Value != "")
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT NAME FROM tb_category WHERE CategoryID IN ('" + hdnCatWiseDiscountids.Value + "')");
            }
            //dsCategory = objCustomer.GetMembershipDetails(CustomerLevelID, StoreId, "category", 2);
            //if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            //{
            //    grdCategory.DataSource = dsCategory;
            //    grdCategory.DataBind();
            //    DiscountCategoryStructure = true;
            //}
            //else
            //{
            //    DiscountCategoryStructure = false;
            //    grdCategory.DataSource = null;
            //    grdCategory.DataBind();
            //}
        }







        protected void grdProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProduct.PageIndex = e.NewPageIndex;
            BindProductDiscountDeatail();

        }
        protected void grdProduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit Percent")
            {
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        txtPercent.Text = lblPercent.Text.ToString();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lblPercent.Visible = false;
                        txtPercent.Visible = true;
                    }
                }
            }
            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lblPercent.Visible = true;
                        txtPercent.Visible = false;
                    }
                }
            }
            if (e.CommandName == "Delete")
            {
                string ProductId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    Label lblProductId = (Label)gr.FindControl("lblProductId");

                    if (lblProductId.Text == e.CommandArgument.ToString())
                    {
                        if (Session["TempDsProdDiscount"] != null)
                        {
                            DataTable dtProduct;
                            dtProduct = (DataTable)Session["TempDsProdDiscount"];
                            DataRow[] dr;
                            dr = dtProduct.Select("ProductId='" + ProductId + "'");
                            foreach (DataRow AssingDr in dr)
                            {
                                dtProduct.Rows.Remove(AssingDr);
                            }
                            dtProduct.AcceptChanges();
                            Session["TempDsProdDiscount"] = dtProduct;
                            grdProduct.DataSource = dtProduct;
                            grdProduct.DataBind();
                            
                            int templateid = 0;
                            templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 TradeTemplateID from tb_TradeTemplateDetail where TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + ""));
                            CommonComponent.ExecuteCommonData("Delete from  tb_MembershipDiscount where MembershipDiscountID in (select MembershipDiscountID from tb_MembershipDiscount inner join tb_TradeTemplateDetail on tb_MembershipDiscount.DiscountObjectID=tb_TradeTemplateDetail.DiscountObjectID where tb_MembershipDiscount.DiscountType=tb_TradeTemplateDetail.DiscountType and tb_TradeTemplateDetail.DiscountObjectID=" + lblProductId.Text.ToString() + " and tb_TradeTemplateDetail.DiscountType='product' and custid in (select CustomerID from tb_Customer where isnull(TradeTemplateID,0)=" + templateid + "))");
                            CommonComponent.ExecuteCommonData("Delete from tb_TradeTemplateDetail  where DiscountObjectID=" + lblProductId.Text.ToString() + " and DiscountType='product' and TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + "");
                        }
                    }
                }

            }
            if (e.CommandName == "Add")
            {
                string ProductId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdProduct.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    Label lblProductId = (Label)gr.FindControl("lblProductId");
                    if (lblProductId.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        Label lblEmail = (Label)gr.FindControl("lblEmailID");
                        Label lblName = (Label)gr.FindControl("lblName");

                        if (txtPercent.Text != "")
                        {
                            decimal DiscountPerc = 0;
                            try
                            {
                                DiscountPerc = Convert.ToDecimal(txtPercent.Text.Replace("%", ""));
                                if (Session["TempDsProdDiscount"] != null)
                                {
                                    DataTable dtproduct;
                                    dtproduct = (DataTable)Session["TempDsProdDiscount"];
                                    DataRow[] dr;
                                    dr = dtproduct.Select("ProductId='" + ProductId + "'");

                                    dr[0]["ProductDiscount"] = DiscountPerc;

                                    dtproduct.AcceptChanges();
                                    Session["TempDsProdDiscount"] = dtproduct;
                                    grdProduct.DataSource = dtproduct;
                                    grdProduct.DataBind();

                                }

                                int templateid = 0;
                                templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 TradeTemplateID from tb_TradeTemplateDetail where TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + ""));
                                CommonComponent.ExecuteCommonData("update tb_MembershipDiscount set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblProductId.Text.ToString() + " and DiscountType='product' and MembershipDiscountID in (select MembershipDiscountID from tb_MembershipDiscount inner join tb_TradeTemplateDetail on tb_MembershipDiscount.DiscountObjectID=tb_TradeTemplateDetail.DiscountObjectID where tb_MembershipDiscount.DiscountType=tb_TradeTemplateDetail.DiscountType and custid in (select CustomerID from tb_Customer where isnull(TradeTemplateID,0)=" + templateid + ")) ");
                                CommonComponent.ExecuteCommonData("Update tb_TradeTemplateDetail set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblProductId.Text.ToString() + " and TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + "");
                            }
                            catch
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter valid Discount ...', 'Message');});", true);
                                return;
                            }

                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            btnEdit.Visible = true;
                            lblPercent.Visible = true;
                            txtPercent.Visible = false;

                            // BindProductDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
                        }
                    }
                }
            }
        }


        protected void grdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCategory.PageIndex = e.NewPageIndex;
            BindCategoryDiscountDeatail();

            //BindCategoryDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
            // grdCategory.DataBind();
        }
        protected void grdCategory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void grdCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit Percent")
            {
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        txtPercent.Text = lblPercent.Text.ToString();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEdit.Visible = false;
                        lblPercent.Visible = false;
                        txtPercent.Visible = true;
                    }
                }
            }
            if (e.CommandName == "Stop")
            {
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblMembershipDiscountID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        btnSave.Visible = false;
                        btnCancel.Visible = false;
                        btnEdit.Visible = true;
                        lblPercent.Visible = true;
                        txtPercent.Visible = false;
                    }
                }
            }

            if (e.CommandName == "Delete")
            {
                string CategoryId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblCategoryID = (Label)gr.FindControl("lblCategoryID");
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    if (lblCategoryID.Text == e.CommandArgument.ToString())
                    {
                        if (Session["TempDsCatDiscount"] != null)
                        {
                            DataTable dtcategory;
                            dtcategory = (DataTable)Session["TempDsCatDiscount"];
                            DataRow[] dr;
                            dr = dtcategory.Select("CategoryId='" + CategoryId + "'");
                            foreach (DataRow AssingDr in dr)
                            {
                                dtcategory.Rows.Remove(AssingDr);
                            }
                            dtcategory.AcceptChanges();
                            Session["TempDsCatDiscount"] = dtcategory;
                            grdCategory.DataSource = dtcategory;
                            grdCategory.DataBind();
                            int templateid = 0;
                            templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 TradeTemplateID from tb_TradeTemplateDetail where TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + ""));
                            CommonComponent.ExecuteCommonData("Delete from  tb_MembershipDiscount where MembershipDiscountID in (select MembershipDiscountID from tb_MembershipDiscount inner join tb_TradeTemplateDetail on tb_MembershipDiscount.DiscountObjectID=tb_TradeTemplateDetail.DiscountObjectID where tb_MembershipDiscount.DiscountType=tb_TradeTemplateDetail.DiscountType and tb_TradeTemplateDetail.DiscountObjectID=" + lblCategoryID.Text.ToString() + " and tb_TradeTemplateDetail.DiscountType='category' and custid in (select CustomerID from tb_Customer where isnull(TradeTemplateID,0)=" + templateid + "))");
                            CommonComponent.ExecuteCommonData("Delete from tb_TradeTemplateDetail  where DiscountObjectID=" + lblCategoryID.Text.ToString() + " and DiscountType='category' and TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + "");
                        }
                    }
                }

            }
            if (e.CommandName == "Add")
            {

                string CategoryId = e.CommandArgument.ToString();
                foreach (GridViewRow gr in grdCategory.Rows)
                {
                    Label lblMembershipDiscountID = (Label)gr.FindControl("lblMembershipDiscountID");
                    Label lblCategoryID = (Label)gr.FindControl("lblCategoryID");

                    if (lblCategoryID.Text == e.CommandArgument.ToString())
                    {
                        Label lblPercent = (Label)gr.FindControl("lblDiscountPercent");
                        TextBox txtPercent = (TextBox)gr.FindControl("txtDiscountPercent");
                        ImageButton btnEdit = (ImageButton)gr.FindControl("btnEdit");
                        ImageButton btnSave = (ImageButton)gr.FindControl("btnSave");
                        ImageButton btnCancel = (ImageButton)gr.FindControl("btnCancel");
                        Label lblEmail = (Label)gr.FindControl("lblEmailID");
                        Label lblName = (Label)gr.FindControl("lblName");

                        if (txtPercent.Text != "")
                        {
                            decimal DiscountPerc = 0;
                            try
                            {
                                DiscountPerc = Convert.ToDecimal(txtPercent.Text.Replace("%", ""));
                                if (Session["TempDsCatDiscount"] != null)
                                {
                                    DataTable dtcategory;
                                    dtcategory = (DataTable)Session["TempDsCatDiscount"];
                                    DataRow[] dr;
                                    dr = dtcategory.Select("CategoryId='" + CategoryId + "'");

                                    dr[0]["CategoryDiscount"] = DiscountPerc;

                                    dtcategory.AcceptChanges();
                                    Session["TempDsCatDiscount"] = dtcategory;
                                    grdCategory.DataSource = dtcategory;
                                    grdCategory.DataBind();

                                }
                                int templateid = 0;
                                templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 TradeTemplateID from tb_TradeTemplateDetail where TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + ""));
                                CommonComponent.ExecuteCommonData("update tb_MembershipDiscount set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblCategoryID.Text.ToString() + " and DiscountType='category' and MembershipDiscountID in (select MembershipDiscountID from tb_MembershipDiscount inner join tb_TradeTemplateDetail on tb_MembershipDiscount.DiscountObjectID=tb_TradeTemplateDetail.DiscountObjectID where tb_MembershipDiscount.DiscountType=tb_TradeTemplateDetail.DiscountType and custid in (select CustomerID from tb_Customer where isnull(TradeTemplateID,0)="+templateid+")) ");
                                CommonComponent.ExecuteCommonData("Update tb_TradeTemplateDetail set Discount=" + DiscountPerc + " where DiscountObjectID=" + lblCategoryID.Text.ToString() + " and TradeTemplateDetailID=" + lblMembershipDiscountID.Text.ToString() + "");

                            }
                            catch
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter valid Discount ...', 'Message');});", true);
                                return;
                            }

                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            btnEdit.Visible = true;
                            lblPercent.Visible = true;
                            txtPercent.Visible = false;

                            //  BindCategoryDetails(Convert.ToInt32(Request.QueryString["CustomerLevelID"].ToString()), Convert.ToInt32(ddlStoreName.SelectedValue));
                        }
                    }
                }
                //  BinCategoryDiscountDeatailbyCustID();
            }
        }

        private void InsertMemberShipDisCount(int TemplateID)
        {
            bool result = false;
            #region Insert for Category Discount Details
            if (Session["TempDsCatDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsCatDiscount"];
                if (dt1 != null)
                {



                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {


                        //                        string query = @"INSERT INTO [dbo].[tb_TradeTemplateDetail]
                        //           (
                        //           [TradeTemplateID]
                        //           ,[Discount]
                        //           ,[DiscountObjectID]
                        //           ,[DiscountType]
                        //           ,[StoreID])
                        //     VALUES (" + TemplateID + "," + Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString()) + "," + Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString()) + ",'category'," + Convert.ToInt32(ddlStoreName.SelectedValue) + ")";

                        //                        CommonComponent.ExecuteCommonData(query);


                        CommonComponent.ExecuteCommonData("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + "," + TemplateID + ",1," + Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString()) + "," + Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString()) + ",'category'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");


                        //tb_MembershipDiscount.CustID = custid;
                        //// tb_MembershipDiscount.MembershipDiscountID = Convert.ToInt32(dt1.Rows[i]["MembershipDiscountID"].ToString());
                        //tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString());
                        //tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString());
                        //tb_MembershipDiscount.DiscountType = "category";
                        //tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        //tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        //tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);

                        //result = objCustomer.InsertMembershipDiscount(tb_MembershipDiscount);

                    }
                }

                Session["TempDsCatDiscount"] = null;
            }

            #endregion


            #region Insert for Product Discount Details
            if (Session["TempDsProdDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsProdDiscount"];
                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {


                        //                        string query = @"INSERT INTO [dbo].[tb_TradeTemplateDetail]
                        //           (
                        //           [TradeTemplateID]
                        //           ,[Discount]
                        //           ,[DiscountObjectID]
                        //           ,[DiscountType]
                        //           ,[StoreID])
                        //     VALUES (" + TemplateID + "," + Convert.ToDecimal(dt1.Rows[i]["ProductDiscount"].ToString()) + "," + Convert.ToInt32(dt1.Rows[i]["ProductId"].ToString()) + ",'product'," + Convert.ToInt32(ddlStoreName.SelectedValue) + ")";

                        //                        CommonComponent.ExecuteCommonData(query);

                        CommonComponent.ExecuteCommonData("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + "," + TemplateID + ",1," + Convert.ToDecimal(dt1.Rows[i]["ProductDiscount"].ToString()) + "," + Convert.ToInt32(dt1.Rows[i]["ProductId"].ToString()) + ",'product'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");


                        //tb_MembershipDiscount.CustID = custid;
                        //// tb_MembershipDiscount.MembershipDiscountID = Convert.ToInt32(dt1.Rows[i]["MembershipDiscountID"].ToString());
                        //tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["ProductDiscount"].ToString());
                        //tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["ProductId"].ToString());
                        //tb_MembershipDiscount.DiscountType = "product";
                        //tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        //tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        //tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        //result = objCustomer.InsertMembershipDiscount(tb_MembershipDiscount);

                    }
                }

                Session["TempDsProdDiscount"] = null;
            }
            #endregion
        }


        private void UpdateMemberShipDisCount(int TemplateID)
        {

            bool result = false;

            #region Update  for Category Discount Details
            if (Session["TempDsCatDiscount"] != null)
            {
                DataTable dt1;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dt1 = (DataTable)Session["TempDsCatDiscount"];

                if (dt1 != null)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        CommonComponent.ExecuteCommonData("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + "," + TemplateID + ",1," + Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString()) + "," + Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString()) + ",'category'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");


                        //tb_MembershipDiscount.CustID = custid;
                        //tb_MembershipDiscount.Discount = Convert.ToDecimal(dt1.Rows[i]["CategoryDiscount"].ToString());
                        //tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dt1.Rows[i]["CategoryId"].ToString());
                        //tb_MembershipDiscount.DiscountType = "category";
                        //tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        //tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        //tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        //result = objCustomer.UpdateMembershipDiscount(tb_MembershipDiscount);
                    }
                }
                Session["TempDsCatDiscount"] = null;
            }
            #endregion

            #region Update  for Product Discount Details
            if (Session["TempDsProdDiscount"] != null)
            {
                DataTable dtProduct;
                tb_MembershipDiscount tb_MembershipDiscount = new tb_MembershipDiscount();
                dtProduct = (DataTable)Session["TempDsProdDiscount"];

                if (dtProduct != null)
                {
                    for (int i = 0; i < dtProduct.Rows.Count; i++)
                    {

                        CommonComponent.ExecuteCommonData("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + "," + TemplateID + ",1," + Convert.ToDecimal(dtProduct.Rows[i]["ProductDiscount"].ToString()) + "," + Convert.ToInt32(dtProduct.Rows[i]["ProductId"].ToString()) + ",'product'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");

                        //tb_MembershipDiscount.CustID = custid;
                        //tb_MembershipDiscount.Discount = Convert.ToDecimal(dtProduct.Rows[i]["ProductDiscount"].ToString());
                        //tb_MembershipDiscount.DiscountObjectID = Convert.ToInt32(dtProduct.Rows[i]["ProductId"].ToString());
                        //tb_MembershipDiscount.DiscountType = "product";
                        //tb_MembershipDiscount.CreatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.CreatedOn = DateTime.Now;
                        //tb_MembershipDiscount.UpdatedBy = Convert.ToInt32(Session["AdminId"].ToString());
                        //tb_MembershipDiscount.UpdatedOn = DateTime.Now;
                        //tb_MembershipDiscount.StoreID = Convert.ToInt32(ddlStoreName.SelectedValue);
                        //result = objCustomer.UpdateMembershipDiscount(tb_MembershipDiscount);
                    }
                }
                Session["TempDsProdDiscount"] = null;
            }
            #endregion
        }
        private void BindCategoryDiscountDeatail()
        {
            if (Session["TempDsCatDiscount"] != null)
            {
                dt = (DataTable)Session["TempDsCatDiscount"];
                grdCategory.DataSource = dt;
                grdCategory.DataBind();
                // Session["TempDsCatDiscount"] = null;

            }

        }
        private void BindProductDiscountDeatail()
        {
            if (Session["TempDsProdDiscount"] != null)
            {
                dt = (DataTable)Session["TempDsProdDiscount"];
                grdProduct.DataSource = dt;
                grdProduct.DataBind();
                // Session["TempDsCatDiscount"] = null;

            }

        }

        private void BinCategoryDiscountDeatailbyCustID()
        {
            if (Request.QueryString["TempID"] != null && Convert.ToString(Request.QueryString["TempID"]).Trim().ToLower() != "")
            {

                int CustID = Convert.ToInt32(Request.QueryString["TempID"].ToString());
                string DiscountType = "category";
                int storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                dsCategory = CommonComponent.GetCommonDataSet("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + ", " + Request.QueryString["TempID"].ToString() + ",2,0,0,'category'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");

                // dsCategory = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 2);


                Session["TempDsCatDiscount"] = dsCategory.Tables[0];
                grdCategory.DataSource = dsCategory;
                grdCategory.DataBind();

            }
            else
            {
                Session["TempDsCatDiscount"] = null;
            }
        }
        private void BinProductDiscountDeatailbyCustID()
        {
            if (Request.QueryString["TempID"] != null && Convert.ToString(Request.QueryString["TempID"]).Trim().ToLower() != "")
            {
                int CustID = Convert.ToInt32(Request.QueryString["TempID"].ToString());
                string DiscountType = "product";
                int storeid = Convert.ToInt32(ddlStoreName.SelectedValue);
                // dsProduct = objCustomer.GetMembershipDetails(CustID, DiscountType, storeid, 3);
                dsProduct = CommonComponent.GetCommonDataSet("Exec usp_TradeTempalteDiscount " + Convert.ToInt32(ddlStoreName.SelectedValue) + ", " + Request.QueryString["TempID"].ToString() + ",3,0,0,'product'," + Convert.ToInt32(Session["AdminId"].ToString()) + "," + Convert.ToInt32(Session["AdminId"].ToString()) + "");


                Session["TempDsProdDiscount"] = dsProduct.Tables[0];
                grdProduct.DataSource = dsProduct;
                grdProduct.DataBind();

            }
            else
            {
                Session["TempDsProdDiscount"] = null;
            }
        }
        protected void btnCustDiscountDetailid_Click(object sender, EventArgs e)
        {

            // BindCategoryDetails(1,1);
            BindCategoryDiscountDeatail();
        }

        protected void btnProdDiscountDetailid_Click(object sender, EventArgs e)
        {
            BindProductDiscountDeatail();

        }


        private void CheckmembershipDiscount(int Tradeid)
        {
            CommonComponent.ExecuteCommonData("Exec usp_TradeTempalateCustomerDiscount " + Tradeid + "");
        }








    }
}