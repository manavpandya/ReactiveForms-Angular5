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
    public partial class viewcodedetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                binddata();
                txtMaxlength.Attributes.Add("disabled", "disabled");
                txtMaxwidth.Attributes.Add("disabled", "disabled");
                txtMinwidth.Attributes.Add("disabled", "disabled");
                txtMinlength.Attributes.Add("disabled", "disabled");

            }

        }
        public void binddata()
        {
            DataSet ds = new DataSet();
            string code = Convert.ToString(CommonComponent.GetScalarCommonData("select Code from tb_ProductFabricCode where fabriccodeid=" + Request.QueryString["fabriccodeid"].ToString() + ""));

            if (!string.IsNullOrEmpty(code))
            {
                string type = Convert.ToString(CommonComponent.GetScalarCommonData("select FabricTypeID from tb_ProductFabricCode where fabriccodeid=" + Request.QueryString["fabriccodeid"].ToString() + ""));
                if (!string.IsNullOrEmpty(type))
                {
                    string fabrictype = Convert.ToString(CommonComponent.GetScalarCommonData("select FabricTypename from tb_ProductFabricType where FabricTypeID=" + type.ToString() + ""));
                    if (!string.IsNullOrEmpty(fabrictype))
                    {
                        lblcode.Text = code;
                        lblfabrictype.Text = fabrictype.ToString();
                    }
                }
                ds = CommonComponent.GetCommonDataSet("select sku,name,productid,isnull(Active,0) as Active,Inventory,case when isnull(Ismadetomeasure,0)=1 then 'Yes' when isnull(Ismadetoorder,0)=1 then 'Yes' else 'No' end as Iscustom,isnull(ProductFabricDays,0) as ProductFabricDays,0 as isRomanp from tb_product where FabricCode='" + code + "' and storeid=1 and isnull(deleted,0)=0 UNION  select tb_product.sku,tb_product.name,tb_product.productid,isnull(Active,0) as Active,tb_product.Inventory,case when isnull(VarActive,0)=1 then 'Yes' else 'No' end as Iscustom,0 as ProductFabricDays,1 as isRomanp from tb_product INNER JOIN tb_ProductVariantValue on tb_ProductVariantValue.ProductId=tb_product.ProductId  where tb_ProductVariantValue.SKU='" + code + "' and storeid=1 and isnull(deleted,0)=0 and isnull(ItemType,'')='Roman'");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvfabric.DataSource = ds;
                    gvfabric.DataBind();
                    btnSave.Visible = true;
                }
                else
                {
                    gvfabric.DataSource = null;
                    gvfabric.DataBind();
                    btnSave.Visible = false;
                }
                ds = CommonComponent.GetCommonDataSet("SELECT isnull(MinWidth,0) as MinWidth,isnull(MaxWidth,0) as MaxWidth,isnull(MinLength,0) as MinLength,isnull(MaxLength,0) as MaxLength FROM tb_ProductFabricWidth WHERE FabricCodeId=" + Request.QueryString["fabriccodeid"].ToString() + "");
                try
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txtMinwidth.Text = ds.Tables[0].Rows[0]["MinWidth"].ToString();
                        txtMaxwidth.Text = ds.Tables[0].Rows[0]["MaxWidth"].ToString();
                        txtMinlength.Text = ds.Tables[0].Rows[0]["MinLength"].ToString();
                        txtMaxlength.Text = ds.Tables[0].Rows[0]["MaxLength"].ToString();
                    }
                }
                catch
                {

                }

                Int32 MinwidthQty = 0;
                Int32 maxwidthQty = 0;
                Int32 minlengthQty = 0;
                Int32 maxlengthQty = 0;
                if (!string.IsNullOrEmpty(txtMinwidth.Text.ToString()))
                {
                    MinwidthQty = Convert.ToInt32(txtMinwidth.Text.ToString());
                    if (MinwidthQty == 0)
                    {
                        txtMinwidth.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMinWidth' and Storeid=1"));
                    }
                }
                else
                {
                    txtMinwidth.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMinWidth' and Storeid=1"));
                }
                if (!string.IsNullOrEmpty(txtMaxwidth.Text.ToString()))
                {
                    maxwidthQty = Convert.ToInt32(txtMaxwidth.Text.ToString());
                    if (maxwidthQty == 0)
                    {
                        txtMaxwidth.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMaxWidth' and Storeid=1"));
                    }
                }
                else
                {
                    txtMaxwidth.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMaxWidth' and Storeid=1"));
                }
                if (!string.IsNullOrEmpty(txtMinlength.Text.ToString()))
                {
                    minlengthQty = Convert.ToInt32(txtMinlength.Text.ToString());
                    if (minlengthQty == 0)
                    {
                        txtMinlength.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMinLength' and Storeid=1"));
                    }
                }
                else
                {
                    txtMinlength.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMinLength' and Storeid=1"));
                }
                if (!string.IsNullOrEmpty(txtMaxlength.Text.ToString()))
                {
                    maxlengthQty = Convert.ToInt32(txtMaxlength.Text.ToString());
                    if (maxlengthQty == 0)
                    {
                        txtMaxlength.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMaxLength' and Storeid=1"));
                    }
                }
                else
                {
                    txtMaxlength.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ConfigValue,'0') FROM tb_AppConfig WHERE ConfigName='FabricMaxLength' and Storeid=1"));
                }
            }

        }
        protected void btnsaverange_Click(object sender, ImageClickEventArgs e)
        {
            Int32 MinwidthQty = 0;
            Int32 maxwidthQty = 0;
            Int32 minlengthQty = 0;
            Int32 maxlengthQty = 0;
            if (!string.IsNullOrEmpty(txtMinwidth.Text.ToString()))
            {
                MinwidthQty = Convert.ToInt32(txtMinwidth.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtMaxwidth.Text.ToString()))
            {
                maxwidthQty = Convert.ToInt32(txtMaxwidth.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtMinlength.Text.ToString()))
            {
                minlengthQty = Convert.ToInt32(txtMinlength.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtMaxlength.Text.ToString()))
            {
                maxlengthQty = Convert.ToInt32(txtMaxlength.Text.ToString());
            }
            CommonComponent.ExecuteCommonData("update tb_ProductFabricWidth set  MinWidth=" + MinwidthQty + ",MaxWidth=" + maxwidthQty + ",MinLength=" + minlengthQty + ",MaxLength=" + maxlengthQty + " where  FabricCodeId=" + Request.QueryString["fabriccodeid"].ToString() + "");
            CommonComponent.ExecuteCommonData("INSERT INTO tb_Measurement(createdby,createdon,MinWidth,MaxWidth,MinLength,MaxLength,fabricodeid,logtype) values (" + Session["AdminID"].ToString() + ", getdate()," + MinwidthQty + "," + maxwidthQty + "," + minlengthQty + "," + maxlengthQty + "," + Request.QueryString["fabriccodeid"].ToString() + ",0)");


            Page.ClientScript.RegisterStartupScript(Page.GetType(), "alertmessage", "alert('Record Saved Successfully.');clicksavebuttonnew();", true);

        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow gr in gvfabric.Rows)
            {
                Label lblproductid = (Label)gr.FindControl("lblproductid");
                TextBox txtdays = (TextBox)gr.FindControl("txtdays");
                if (lblproductid != null && txtdays != null)
                {
                    Int32 dd = 0;
                    if (txtdays.Text.ToString() != "")
                    {
                        Int32.TryParse(txtdays.Text.ToString(), out dd);
                        CommonComponent.ExecuteCommonData("update tb_product SET ProductFabricDays=" + dd.ToString() + " WHERE ProductId=" + lblproductid.Text.ToString() + "");
                        CommonComponent.ExecuteCommonData("INSERT INTO tb_Measurement(createdby,createdon,productid,fabricodeid,logtype) values (" + Session["AdminID"].ToString() + ", getdate()," + lblproductid.Text.ToString() + "," + Request.QueryString["fabriccodeid"].ToString() + ",1)");
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "alertmessage", "alert('Record Saved Successfully.');clicksavebuttonnew();", true);

        }
        protected void gvfabric_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblstatus = (Label)e.Row.FindControl("lblstatus");
                Label lblisRomanp = (Label)e.Row.FindControl("lblisRomanp");
                TextBox txtdays = (TextBox)e.Row.FindControl("txtdays");
                if (lblisRomanp.Text.ToString().ToLower() == "1")
                {
                    txtdays.Visible = false;
                }
                if (lblstatus.Text.ToString().ToLower() == "true")
                {
                    lblstatus.Text = "Active";
                }
                else
                {
                    lblstatus.Text = "In Active";
                }
            }
        }
    }
}