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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductvariantFabric : BasePage
    {
        public static string SearchProductTempPath = string.Empty;
        public static string SearchProductPath = string.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeIcon = Size.Empty;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddToSelectionlist.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

            if (!IsPostBack)
            {
                FillFabricType();


                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "edit")
                {
                    string StrVariId = Convert.ToString(Request.QueryString["VariId"]);
                    string StrProductID = Convert.ToString(Request.QueryString["ProductID"]);
                    BindOldData(StrVariId, StrProductID);
                }
            }
        }
        private void FillFabricType()
        {
            DataSet DsFabircType = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            DsFabircType = CommonComponent.GetCommonDataSet("SELECT FabricTypeID,FabricTypename FROM dbo.tb_ProductFabricType WHERE ISNULL(Active,0)=1 AND ISNULL(FabricTypename,'')<>'' ORDER BY ISNULL(DisplayOrder,999) ASC"); //objProduct.GetProductFabricDetails(0, 1);
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
            ddlFabricType.Items.Insert(0, new ListItem("Select Fabric Type", ""));
            ddlFabricType_SelectedIndexChanged(null, null);
        }
        protected void BindOldData(string VariantValueId, string ProductId)
        {
            try
            {
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("Select FabricType,FabricCode from tb_ProductVariantValue Where Productid=" + ProductId + " and VariantValueid=" + VariantValueId + "");
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsVariant.Tables[0].Rows[0]["FabricType"].ToString()))
                    {
                        ddlFabricType.ClearSelection();
                        ddlFabricType.Items.FindByText(Convert.ToString(dsVariant.Tables[0].Rows[0]["FabricType"].ToString())).Selected = true;
                        ddlFabricType_SelectedIndexChanged(null, null);
                    }
                    if (!string.IsNullOrEmpty(dsVariant.Tables[0].Rows[0]["FabricCode"].ToString()))
                    {
                        try
                        {
                            ddlFabricCode.ClearSelection();
                            ddlFabricCode.Items.FindByText(Convert.ToString(dsVariant.Tables[0].Rows[0]["FabricCode"].ToString())).Selected = true;
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {

            }
            
        }



        /// <summary>
        ///  Add to Color Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnAddToSelectionlist_Click(object sender, ImageClickEventArgs e)
        {

            string StrId = "";
            string StrColorImgId = "";
            string strfabrictype = "";
            string strfabriccode = "";


            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "edit")
            {
                if (Request.QueryString["VariId"] != null)
                {
                    string StrVariId = Convert.ToString(Request.QueryString["VariId"]);
                    string Ftype = "";
                    string Code = "";
                    if (ddlFabricType.SelectedIndex > 0)
                    {
                        Ftype = ddlFabricType.SelectedItem.Text.ToString();
                    }
                    if (ddlFabricCode.SelectedIndex > 0)
                    {
                        Code = ddlFabricCode.SelectedItem.Text.ToString();
                    }
                    CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET FabricType='" + Ftype.Replace("'", "''") + "',FabricCode='" + Code.Replace("'", "''") + "' WHERE VariantValueid=" + StrVariId + "");
                }
            }
            Page.ClientScript.RegisterClientScriptBlock(btnAddToSelectionlist.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString().Replace("arelatedfabric", "hdnProductfabrictype") + "').value = '" + ddlFabricType.SelectedItem.Text.ToString() + "';window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString().Replace("arelatedfabric", "hdnProductfabriccode") + "').value = '" + ddlFabricCode.SelectedItem.Text.ToString() + "';window.close();", true);


        }

        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFabricCode.Items.Clear();
            ProductComponent objProduct = new ProductComponent();

            DataSet DsFabricCode = new DataSet();
            if (ddlFabricType.SelectedIndex > 0)
            {
                DsFabricCode = objProduct.GetProductFabricDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
            }
            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            {
                ddlFabricCode.DataSource = DsFabricCode;
                ddlFabricCode.DataValueField = "FabricCodeId";
                ddlFabricCode.DataTextField = "Code";
                ddlFabricCode.DataBind();
            }
            else
            {
                ddlFabricCode.DataSource = null;
                ddlFabricCode.DataBind();
            }
            ddlFabricCode.Items.Insert(0, new ListItem("Select Fabric Code", ""));
        }




    }
}