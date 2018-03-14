using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ViewminvendorPrice : BasePage
    {
        Decimal Total = Decimal.Zero;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.ImageUrl = "/App_Themes/gray/images/save.gif";
            btnCancel.ImageUrl = "/App_Themes/gray/images/cancel.gif";
            if (!IsPostBack)
            {
                binddata();
            }

        }
        public void binddata()
        {
            string asin = "";
            DataSet ds = new DataSet();
            if (Request.QueryString["asin"] != null)
            {
                asin = Request.QueryString["asin"].ToString();
                DataSet Dsdetails = new DataSet();
                Dsdetails = CommonComponent.GetCommonDataSet("select top 1 isnull(SkuId,'') as SkuId,isnull(ProductName,'') as ProductName,ASINId from ItemInfoes where ASINId='" + asin + "'");
                if (Dsdetails != null && Dsdetails.Tables.Count > 0 && Dsdetails.Tables[0].Rows.Count > 0)
                {
                    lblname.Text = Dsdetails.Tables[0].Rows[0]["ProductName"].ToString();
                    lblsku.Text = Dsdetails.Tables[0].Rows[0]["SkuId"].ToString();
                    lblasin.Text = Dsdetails.Tables[0].Rows[0]["ASINId"].ToString();

                }
                CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asin + "'");
                ds = CommonComponent.GetCommonDataSet("Exec GuiGetMinRePricerListByASIN 1,'" + asin + "'");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {


                    gvminprice.DataSource = ds;
                    gvminprice.DataBind();
                    GetCustomCalculation(asin);
                }
                else
                {
                    gvminprice.DataSource = null;
                    gvminprice.DataBind();
                }
            }
            else
            {
                gvminprice.DataSource = null;
                gvminprice.DataBind();
            }



        }

        protected void gvminprice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvminprice.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void gvminprice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton _editLinkButton = (ImageButton)e.Row.FindControl("_editLinkButton");

                ImageButton btnSave = (ImageButton)e.Row.FindControl("btnSave");

                ImageButton btnCancel = (ImageButton)e.Row.FindControl("btnCancel");

                _editLinkButton.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/edit-price.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/save.png";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/CloseIcon.png";
                Label lbltotal1 = (Label)e.Row.FindControl("lbltotal1");

                Decimal.TryParse(lbltotal1.Text.ToString().Replace("$", ""), out Total);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbltotal = (Label)e.Row.FindControl("lbltotal");
                lbltotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(Total));

            }
        }

        protected void gvminprice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow gvrow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                ImageButton btnSave = gvrow.FindControl("btnSave") as ImageButton;
                ImageButton btnCancel = gvrow.FindControl("btnCancel") as ImageButton;
                ImageButton btnEditPrice = gvrow.FindControl("_editLinkButton") as ImageButton;
                TextBox txtPercentage = gvrow.FindControl("txtPercentage") as TextBox;
                Label lblPercentage = gvrow.FindControl("lblPercentage") as Label;
                if (e.CommandName == "edit")
                {
                    try
                    {
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEditPrice.Visible = false;

                        txtPercentage.Visible = true;
                        lblPercentage.Visible = false;

                    }
                    catch
                    { }
                }
                else if (e.CommandName == "Cancel")
                {
                    btnEditPrice.Visible = true;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    txtPercentage.Visible = false;
                    lblPercentage.Visible = true;


                }
                else if (e.CommandName == "Save")
                {
                    String MinChildID = Convert.ToString(e.CommandArgument.ToString());
                    Decimal Percent = 0;

                    Decimal.TryParse(txtPercentage.Text, out Percent);

                    Decimal OriginalPercent = 0;
                    Decimal.TryParse(lblPercentage.Text, out OriginalPercent);
                    CommonComponent.ExecuteCommonData("update tb_MinRepricerChild set Percentage=" + Percent + " where MinChildID=" + MinChildID + "");
                    if (Percent != OriginalPercent)
                    {
                        if (Request.QueryString["asin"] != null)
                        {
                            string asin = Request.QueryString["asin"].ToString();
                            CommonComponent.ExecuteCommonData("update ItemInfoes set iscustom=1 where asinid='" + asin + "'");
                        }
                    }

                    if (Request.QueryString["asin"] != null)
                    {
                        string asin = Request.QueryString["asin"].ToString();
                        CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asin + "'");


                        CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 1,'" + asin + "',''," + Session["AdminID"].ToString() + "");
                    }
                    hdnflag.Value = "2";
                    binddata();
                    

                }
            }
        }

        private void GetCustomCalculation(string asin)
        {
            DataSet ds = CommonComponent.GetCommonDataSet("Exec GuiGetMinRePricerListByASIN 1,'" + asin + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                bool IsCustom = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCustom"].ToString());
                bool UseFixPrice = Convert.ToBoolean(ds.Tables[0].Rows[0]["UseFixPrice"].ToString());
                Decimal FixPrice = Convert.ToDecimal(ds.Tables[0].Rows[0]["FixPrice"].ToString());
                if (IsCustom)
                {
                    chkrevertparent.Visible = true;
                    
                }
                else
                {
                    chkrevertparent.Visible = false;
                    
                }
                if (UseFixPrice)
                {
                    chkusefixprice.Checked = true;
                    txtfixprice.Text = String.Format("{0:0.00}", Convert.ToDecimal(FixPrice));
                    tdchkusefixprice.Attributes.Add("Display", "''");
                    tdfixprice.Attributes.Add("Display", "''");
                }
                else
                {
                    chkusefixprice.Checked = false;
                    txtfixprice.Text = String.Format("{0:0.00}", Convert.ToDecimal(0));
                    tdchkusefixprice.Attributes.Add("Display", "none");
                    tdfixprice.Attributes.Add("Display", "none");
                }
                if (hdnflag.Value.ToString() == "1" || hdnflag.Value.ToString() == "2")
                {
                    int currentpage = 1;
                    if(Request.QueryString["cp"]!=null)
                    {
                        Int32.TryParse(Request.QueryString["cp"].ToString(), out currentpage);
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg11", "jAlert('Record Saved Successfully.','Message');ShowHidetd();window.parent.document.getElementById('ContentPlaceHolder1_hdncurrenttemp').value=" + currentpage + ";window.parent.document.getElementById('ContentPlaceHolder1_btntemp').click();window.close();", true);
                    hdnflag.Value = "0";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "ShowHidetd();", true);
                }

                

            }
        }

        protected void gvminprice_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvminprice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvminprice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["asin"] != null)
            {
                hdnflag.Value = "1";
                string asin = Request.QueryString["asin"].ToString();
                if (chkrevertparent.Checked)
                {
                    CommonComponent.ExecuteCommonData("update ItemInfoes set iscustom=0,UseFixPrice=0 where asinid='" + asin + "'");
                    CommonComponent.ExecuteCommonData("Delete From tb_MinRepricerChild where AsinID='" + asin + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asin + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 6,'" + asin + "',''," + Session["AdminID"].ToString() + "");
                }



                if (chkusefixprice.Checked)
                {
                    Decimal FixPrice = 0;
                    Decimal.TryParse(txtfixprice.Text, out FixPrice);
                    CommonComponent.ExecuteCommonData("update ItemInfoes set UseFixPrice=1,FixPrice=" + FixPrice + " where asinid='" + asin + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asin + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 7,'" + asin + "','" + FixPrice.ToString("C2") + "'," + Session["AdminID"].ToString() + "");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("update ItemInfoes set UseFixPrice=0 where asinid='" + asin + "'");
                    CommonComponent.ExecuteCommonData("Exec GuiCalculateMinRePricer '" + asin + "'");
                }
                binddata();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg11", "window.opener.document.getElementById('ContentPlaceHolder1_btntemp').click();window.close();", true);
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}