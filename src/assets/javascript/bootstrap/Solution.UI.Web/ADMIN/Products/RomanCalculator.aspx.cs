using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class RomanCalculator : System.Web.UI.Page
    {
        ProductComponent objProduct = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
                {
                    trlngthfrom.Visible = true;
                    trlngthprice.Visible = true;
                    trWidthfrom.Visible = false;
                    trWidthprice.Visible = false;
                }
                else
                {
                    trlngthfrom.Visible = false;
                    trlngthprice.Visible = false;
                    trWidthfrom.Visible = true;
                    trWidthprice.Visible = true;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["fabriccodeid"]))
                {
                    hdnfabcodeid.Value = Request.QueryString["fabriccodeid"];
                    hdnfabcode.Value = Convert.ToString(CommonComponent.GetScalarCommonData("select code from tb_ProductFabricCode where FabricCodeId=" + hdnfabcodeid.Value + "  and isnull(Active,0)=1"));
                }
                FillRoman();
                BindData();
            }
        }

        private void FillRoman()
        {
            DataSet DsRomanType = new DataSet();
            ProductComponent objProduct = new ProductComponent();

            DsRomanType = CommonComponent.GetCommonDataSet("SELECT RomanTypeID,TypeName FROM tb_RomanTypecalculator WHERE isnull(Active,0)=1 AND isnull(Deleted,0)=0");
            //}
            //else
            //{
            //    DsFabircType = objProduct.GetProductFabricDetails(0, 1);
            //}
            if (DsRomanType != null && DsRomanType.Tables.Count > 0 && DsRomanType.Tables[0].Rows.Count > 0)
            {
                ddlRoman.DataSource = DsRomanType;
                ddlRoman.DataValueField = "RomanTypeID";
                ddlRoman.DataTextField = "TypeName";
                ddlRoman.DataBind();
            }
            else
            {
                ddlRoman.DataSource = null;
                ddlRoman.DataBind();
            }
            ddlRoman.Items.Insert(0, new ListItem("Select Roman Type", "0"));
            
        }
        protected void ddlRoman_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Boolean lengthfrom = false;
            Boolean lengthto = false;
            decimal Widthfrom = 0;

            decimal WidthTo = 0;
            decimal WidthPrice = 0;

            decimal Lengthfrom = 0;
            decimal LengthTo = 0;
            decimal LengthPRice = 0;
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
            {
                Lengthfrom = Convert.ToDecimal(txtlengthfrom.Text);
                LengthTo = Convert.ToDecimal(txtlengthto.Text);
                LengthPRice = Convert.ToDecimal(txtlengthprice.Text);
            }
            else
            {
                Widthfrom = Convert.ToDecimal(txtwidthfrom.Text);
                WidthTo = Convert.ToDecimal(txtwidthto.Text);
                WidthPrice = Convert.ToDecimal(txtWidthPrice.Text);
            }

            string RomanType = ddlRoman.SelectedItem.Text;

            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
            {
                DataSet dswidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_RomanPriceLengthcalculation where RomanLengthId <> " + hdnRomanLengthId.Value + " and FabricCodeId= " + hdnfabcodeid.Value + " and TypeName='" + RomanType + "' and LengthFrom <='" + Lengthfrom + "' and LengthTo >='" + Lengthfrom + "'");
                if (dswidth != null && dswidth.Tables.Count > 0 && dswidth.Tables[0].Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Range", "jAlert('Range is already Exists','Message');", true);

                    return;
                }
                else
                {
                    dswidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_RomanPriceLengthcalculation where RomanLengthId <> " + hdnRomanLengthId.Value + " and FabricCodeId= " + hdnfabcodeid.Value + " and TypeName='" + RomanType + "' and LengthFrom<='" + lengthto + "' and LengthTo >='" + lengthto + "'");
                    if (dswidth != null && dswidth.Tables.Count > 0 && dswidth.Tables[0].Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Range", "jAlert('Range is already Exists','Message');", true);
                        return;
                    }
                }
                if (hdnRomanLengthId.Value != "0")
                {
                    CommonComponent.ExecuteCommonData("update tb_RomanPriceLengthcalculation set TypeName='" + ddlRoman.SelectedItem.Text + "',LengthFrom=" + txtlengthfrom.Text + ",LengthTo=" + txtlengthto.Text + ",Price=" + txtlengthprice.Text + " where RomanLengthId=" + hdnRomanLengthId.Value);


                }
                else
                {
                    CommonComponent.ExecuteCommonData("insert into  tb_RomanPriceLengthcalculation (TypeName,LengthFrom,LengthTo,Price,Deleted,FabricCode,FabricCodeId) values ('" + RomanType + "'," + Lengthfrom + "," + LengthTo + "," + LengthPRice + ",0,'" + hdnfabcode.Value + "'," + hdnfabcodeid.Value + ")");
                }

            }
            else
            {
                DataSet dswidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_RomanPricewidthcalculation where RomanWidhId <> " + hdnRomanWidhId.Value + " and FabricCodeId= " + hdnfabcodeid.Value + " and TypeName='" + RomanType + "' and WidthFrom<='" + Widthfrom + "' and WidthTo >='" + Widthfrom + "'");
                if (dswidth != null && dswidth.Tables.Count > 0 && dswidth.Tables[0].Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Range", "jAlert('Range is already Exists','Message');", true);
                    return;
                }
                else
                {
                    dswidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_RomanPricewidthcalculation where RomanWidhId <> " + hdnRomanWidhId.Value + " and FabricCodeId= " + hdnfabcodeid.Value + " and TypeName='" + RomanType + "' and WidthFrom<='" + WidthTo + "' and WidthTo >='" + WidthTo + "'");
                    if (dswidth != null && dswidth.Tables.Count > 0 && dswidth.Tables[0].Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Range", "jAlert('Range is already Exists','Message');", true);
                        return;
                    }
                }
                if (hdnRomanWidhId.Value != "0")
                {
                    CommonComponent.ExecuteCommonData(" update tb_RomanPricewidthcalculation set TypeName='" + ddlRoman.SelectedItem.Text + "',WidthFrom=" + txtwidthfrom.Text + ",WidthTo=" + txtwidthto.Text + ",Price=" + txtWidthPrice.Text + " where RomanWidhId=" + hdnRomanWidhId.Value);
                }
                else
                {
                    CommonComponent.ExecuteCommonData("insert into  tb_RomanPricewidthcalculation (TypeName,WidthFrom,WidthTo,Price,Deleted,FabricCode,FabricCodeId) values ('" + RomanType + "'," + Widthfrom + "," + WidthTo + "," + WidthPrice + ",0,'" + hdnfabcode.Value + "'," + hdnfabcodeid.Value + ")");
                }
            }

            txtlengthfrom.Text = "";
            txtlengthto.Text = "";
            txtlengthprice.Text = "";
            txtwidthfrom.Text = "";
            txtwidthto.Text = "";
            txtWidthPrice.Text = "";
            ddlRoman.SelectedIndex = 0;
            hdnRomanLengthId.Value = "0";
            hdnRomanWidhId.Value = "0";
            BindData();
        }
        protected void BindData()
        {
            string strSearchVal = "";


            DataSet dsSearchpro = new DataSet();
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
            {
                dsSearchpro = CommonComponent.GetCommonDataSet("select tb_ProductFabricCode.FabricCodeId,tb_ProductFabricCode.Code, tb_RomanPriceLengthcalculation.RomanLengthId,tb_RomanPriceLengthcalculation.TypeName,tb_RomanPriceLengthcalculation.price,tb_RomanPriceLengthcalculation.LengthFrom,tb_RomanPriceLengthcalculation.LengthTo,0 as RomanWidhId,'0' as Widthfrom,'0' as WidthTo,'0' as Price1  from tb_ProductFabricCode inner join tb_RomanPriceLengthcalculation on tb_RomanPriceLengthcalculation.FabricCodeId= tb_ProductFabricCode.FabricCodeId  where tb_ProductFabricCode.FabricCodeId=" + hdnfabcodeid.Value);
            }
            else
            {
                dsSearchpro = CommonComponent.GetCommonDataSet("select tb_ProductFabricCode.FabricCodeId,tb_ProductFabricCode.Code,  tb_RomanPricewidthcalculation.RomanWidhId, 0 as RomanLengthId, tb_RomanPricewidthcalculation.TypeName,tb_RomanPricewidthcalculation.price as price1,tb_RomanPricewidthcalculation.Widthfrom,tb_RomanPricewidthcalculation.WidthTo,'0' as LengthFrom ,'0' as LengthTo,'0' as Price from tb_ProductFabricCode  inner join tb_RomanPricewidthcalculation on tb_RomanPricewidthcalculation.FabricCodeId= tb_ProductFabricCode.FabricCodeId  where tb_ProductFabricCode.FabricCodeId=" + hdnfabcodeid.Value);
            }
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                grdSearchProductType.DataSource = dsSearchpro.Tables[0];
                grdSearchProductType.DataBind();
            }
            else
            {
                grdSearchProductType.DataSource = null;
                grdSearchProductType.DataBind();
            }
        }

        protected void BindData(string RomanType)
        {
            string strSearchVal = "";


            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("select tb_ProductFabricCode.FabricCodeId,tb_ProductFabricCode.Code, tb_RomanPriceLengthcalculation.*,tb_RomanPricewidthcalculation.* from tb_ProductFabricCode inner join tb_RomanPriceLengthcalculation on tb_RomanPriceLengthcalculation.FabricCodeId= tb_ProductFabricCode.FabricCodeId inner join tb_RomanPricewidthcalculation on tb_RomanPricewidthcalculation.FabricCodeId= tb_ProductFabricCode.FabricCodeId  where tb_ProductFabricCode.FabricCodeId=" + hdnfabcodeid.Value + "and tb_RomanPricewidthcalculation.TypeName='" + RomanType + "'");
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                grdSearchProductType.DataSource = dsSearchpro.Tables[0];
                grdSearchProductType.DataBind();
            }
            else
            {
                grdSearchProductType.DataSource = null;
                grdSearchProductType.DataBind();
            }
        }

        protected void grdSearchProductType_DataBound(object sender, EventArgs e)
        {


        }

        protected void grdSearchProductType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "edit")
            //{
            //    foreach (GridViewRow dr in grdSearchProductType.Rows)
            //    {
            //        Label lblTypeName = (Label)dr.FindControl("lblTypeName");
            //        Label lblWidthfrom = (Label)dr.FindControl("lblWidthfrom");
            //        Label lblWidthTo = (Label)dr.FindControl("lblWidthTo");
            //        Label lblPrice1 = (Label)dr.FindControl("lblPrice1");
            //        Label lblLengthFrom = (Label)dr.FindControl("lblLengthFrom");
            //        Label lblLengthTo = (Label)dr.FindControl("lblLengthTo");
            //        Label lblPrice = (Label)dr.FindControl("lblPrice");
            //        Label lblFabricCodeId = (Label)dr.FindControl("lblFabricCodeId");
            //        Label lblRomanLengthId = (Label)dr.FindControl("lblRomanLengthId");
            //        Label lblRomanWidhId = (Label)dr.FindControl("lblRomanWidhId");

            //        txtlengthfrom.Text = lblLengthFrom.Text;
            //        txtlengthto.Text = lblLengthTo.Text;
            //        txtlengthprice.Text = lblPrice.Text;
            //        txtwidthfrom.Text = lblWidthfrom.Text;
            //        txtwidthto.Text = lblWidthTo.Text;
            //        txtWidthPrice.Text = lblPrice1.Text;
            //        hdnRomanLengthId.Value = lblRomanLengthId.Text;
            //        hdnRomanWidhId.Value = lblRomanWidhId.Text;

            //        ddlRoman.ClearSelection();
            //        ddlRoman.Items.FindByText(Convert.ToString(lblTypeName.Text)).Selected = true;

            //        //btnSave.Visible = false;
            //        //btnupdate.Visible = true;

            //    }


            //}
        }

        protected void grdSearchProductType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Label lblTypeName = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblTypeName");
            Label lblWidthfrom = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblWidthfrom");
            Label lblWidthTo = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblWidthTo");
            Label lblPrice1 = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblPrice1");
            Label lblLengthFrom = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblLengthFrom");
            Label lblLengthTo = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblLengthTo");
            Label lblPrice = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblPrice");
            Label lblFabricCodeId = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblFabricCodeId");
            Label lblRomanLengthId = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblRomanLengthId");
            Label lblRomanWidhId = (Label)grdSearchProductType.Rows[e.NewEditIndex].FindControl("lblRomanWidhId");

            txtlengthfrom.Text = lblLengthFrom.Text;
            txtlengthto.Text = lblLengthTo.Text;
            txtlengthprice.Text = lblPrice.Text;
            txtwidthfrom.Text = lblWidthfrom.Text;
            txtwidthto.Text = lblWidthTo.Text;
            txtWidthPrice.Text = lblPrice1.Text;
            hdnRomanLengthId.Value = lblRomanLengthId.Text;
            hdnRomanWidhId.Value = lblRomanWidhId.Text;

            ddlRoman.ClearSelection();
            ddlRoman.Items.FindByText(Convert.ToString(lblTypeName.Text)).Selected = true;

        }

        protected void btnCancle_Click(object sender, ImageClickEventArgs e)
        {
            txtlengthfrom.Text = "";
            txtlengthto.Text = "";
            txtlengthprice.Text = "";
            txtwidthfrom.Text = "";
            txtwidthto.Text = "";
            txtWidthPrice.Text = "";
            ddlRoman.SelectedIndex = 0;                               
            hdnRomanLengthId.Value = "0";
            hdnRomanWidhId.Value = "0";
        }

        protected void btnupdate_Click(object sender, ImageClickEventArgs e)
        {

            //btnupdate.Visible = false;
            //btnSave.Visible = true;

            txtlengthfrom.Text = null;
            txtlengthto.Text = null;
            txtlengthprice.Text = null;
            txtwidthfrom.Text = null;
            txtwidthto.Text = null;
            txtWidthPrice.Text = null;
            ddlRoman.SelectedValue = Convert.ToString(0);
            BindData();

        }

        protected void grdSearchProductType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                else
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                }
                else
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                }
            }

        }

        protected void grdSearchProductType_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSearchProductType.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void grdSearchProductType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "length")
            {
                Label lblRomanLengthId = (Label)grdSearchProductType.Rows[e.RowIndex].FindControl("lblRomanLengthId");
                CommonComponent.ExecuteCommonData("DELETE FROM tb_RomanPriceLengthcalculation  WHERE RomanLengthId=" + lblRomanLengthId.Text + "");
            }
            else
            { 
             Label lblRomanWidhId = (Label)grdSearchProductType.Rows[e.RowIndex].FindControl("lblRomanWidhId");
             CommonComponent.ExecuteCommonData("DELETE FROM tb_RomanPricewidthcalculation  WHERE RomanWidhId=" + lblRomanWidhId.Text + "");
            }
            BindData();
        }
    }
}