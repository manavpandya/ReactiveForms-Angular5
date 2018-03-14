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
    public partial class ProductRomanShadeYardage : BasePage
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
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                BindStore();
                if (!string.IsNullOrEmpty(Request.QueryString["RomanShadeId"]))
                {
                    BindData(Convert.ToInt32(Request.QueryString["RomanShadeId"]));
                    lblTitle.Text = "Edit Product Roman Shade Yardage";
                }
            }
            txtShadeName.Focus();
        }

        /// <summary>
        /// Bind store dropdown
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail != null && storeDetail.Count > 0)
            {
                ddlStoreName.DataSource = storeDetail;
                ddlStoreName.DataTextField = "StoreName";
                ddlStoreName.DataValueField = "StoreID";
                ddlStoreName.DataBind();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["StoreId"]))
            {
                ddlStoreName.SelectedValue = Convert.ToString(Request.QueryString["StoreId"]);
            }
            else
            {
                ddlStoreName.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="StateID">int StateID</param>
        private void BindData(Int32 RomanShadeId)
        {
            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("Select ISNULL(Lined,0) as Lined,ISNULL(LinedInterlined,0) as LinedInterlined,ISNULL(BlackoutLining,0) as BlackoutLining,RomanShadeId,ISNULL(ShadeName,'') as ShadeName,ISNULL(StoreId,1) AS StoreId,ISNULL(WidthStandardAllowance,0) as WidthStandardAllowance,ISNULL(LengthStandardAllowance,0) as LengthStandardAllowance,ISNULL(Active,1) as Active,Isnull(DisplayOrder,0) as DisplayOrder,isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost,Isnull(MechanismOption,0) as MechanismOption,Isnull(Duties,0) as Duties from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + "");
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                txtShadeName.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["ShadeName"]);
                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["WidthStandardAllowance"].ToString().Trim()))
                    txtAddWidthStandardAllowance.Text = "0.00";
                else
                    txtAddWidthStandardAllowance.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["WidthStandardAllowance"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["LengthStandardAllowance"].ToString().Trim()))
                    txtAddLengthStandardAllowance.Text = "0.00";
                else
                    txtAddLengthStandardAllowance.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["LengthStandardAllowance"].ToString().Trim()), 2));
                txtDisplayOrder.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["DisplayOrder"]);
                chkActive.Checked = Convert.ToBoolean(dsSearchpro.Tables[0].Rows[0]["Active"]);

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["FabricPerYardCost"].ToString().Trim()))
                    txtfabricyard.Text = "0.00";
                else
                    txtfabricyard.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["FabricPerYardCost"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["ManufuacturingCost"].ToString().Trim()))
                    txtmenufacturercost.Text = "0.00";
                else
                    txtmenufacturercost.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["ManufuacturingCost"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["MechanismOption"].ToString().Trim()))
                    txtMechanism.Text = "0.00";
                else
                    txtMechanism.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["MechanismOption"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["Duties"].ToString().Trim()))
                    txtDuties.Text = "0.00";
                else
                    txtDuties.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["Duties"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["Lined"].ToString().Trim()))
                    txtLined.Text = "0.00";
                else
                    txtLined.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["Lined"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["LinedInterlined"].ToString().Trim()))
                    txtLinedInterlined.Text = "0.00";
                else
                    txtLinedInterlined.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["LinedInterlined"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["BlackoutLining"].ToString().Trim()))
                    txtBlackoutLining.Text = "0.00";
                else
                    txtBlackoutLining.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["BlackoutLining"].ToString().Trim()), 2));
            }
        }

        #region Button Click Events

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["romanproductType"] != null)
            {
                Response.Redirect("/Admin/Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&romanproductType=1");
            }
            else
            {
                Response.Redirect("ProductRomanShadeYardageList.aspx");
            }
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            AppConfig.StoreID = 1;
            if (txtShadeName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Shade Name.', 'Message','');});", true);
                txtShadeName.Focus();
                return;
            }

            decimal WidthStandardAllowance = 0;
            if (!string.IsNullOrEmpty(txtAddWidthStandardAllowance.Text.Trim()) && Convert.ToDecimal(txtAddWidthStandardAllowance.Text) > 0)
                WidthStandardAllowance = Convert.ToDecimal(txtAddWidthStandardAllowance.Text.Trim());

            decimal LengthStandardAllowance = 0;
            if (!string.IsNullOrEmpty(txtAddLengthStandardAllowance.Text.Trim()) && Convert.ToDecimal(txtAddLengthStandardAllowance.Text) > 0)
                LengthStandardAllowance = Convert.ToDecimal(txtAddLengthStandardAllowance.Text.Trim());

            decimal MechanismOption = 0;
            decimal.TryParse(txtMechanism.Text.ToString(), out MechanismOption);

            decimal Duties = 0;
            decimal.TryParse(txtDuties.Text.ToString(), out Duties);

            decimal Lined = 0;
            decimal.TryParse(txtLined.Text.ToString(), out Lined);
            decimal LinedInterlined = 0;
            decimal.TryParse(txtLinedInterlined.Text.ToString(), out LinedInterlined);
            decimal BlackoutLining = 0;
            decimal.TryParse(txtBlackoutLining.Text.ToString(), out BlackoutLining);

            Int32 Active = 0;
            if (chkActive.Checked)
                Active = 1;

            Int32 DisplayOrder = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()) && Convert.ToInt32(txtDisplayOrder.Text) > 0)
                DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());

            if (!string.IsNullOrEmpty(Request.QueryString["RomanShadeId"]) && Convert.ToString(Request.QueryString["RomanShadeId"]) != "0")
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductRomanShadeYardage where ShadeName='" + txtShadeName.Text.Trim().ToString() + "' and StoreId=" + ddlStoreName.SelectedValue + " and ISNULL(Active,0)=1 and isnull(Deleted,0)=0 AND RomanShadeId <> " + Convert.ToString(Request.QueryString["RomanShadeId"]) + ""));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shade Name already existss.', 'Message','');});", true);
                    return;
                }
                else
                {
                    CommonComponent.ExecuteCommonData("Update tb_ProductRomanShadeYardage set Lined=" + Lined + ",LinedInterlined=" + LinedInterlined + ",BlackoutLining=" + BlackoutLining + ",ShadeName='" + txtShadeName.Text.Trim().ToString().Replace("'", "''") + "',StoreId=" + ddlStoreName.SelectedValue + ",Active=" + Active + ",WidthStandardAllowance=" + WidthStandardAllowance + ",LengthStandardAllowance=" + LengthStandardAllowance + ",DisplayOrder=" + DisplayOrder + ",FabricPerYardCost=" + txtfabricyard.Text.ToString() + ",ManufuacturingCost=" + txtmenufacturercost.Text.ToString() + ",MechanismOption=" + MechanismOption + ",Duties=" + Duties + " where RomanShadeId = " + Convert.ToString(Request.QueryString["RomanShadeId"]) + "");
                    if (Request.QueryString["romanproductType"] != null)
                    {
                        if (Request.QueryString["ProductID"] != null)
                        {
                            Response.Redirect("/Admin/Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&romanproductType=1&ID=" + Request.QueryString["ProductID"].ToString());
                        }
                        else
                        {
                            Response.Redirect("/Admin/Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&romanproductType=1");
                        }
                    }
                    else
                    {
                        Response.Redirect("ProductRomanShadeYardageList.aspx?status=updated");
                    }
                }
            }
            else
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductRomanShadeYardage where ShadeName='" + txtShadeName.Text.Trim().ToString() + "' and StoreId=" + ddlStoreName.SelectedValue + " and ISNULL(Active,0)=1 AND isnull(Deleted,0)=0 "));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Shade Name already exists.', 'Message','');});", true);
                    return;
                }
                else
                {
                    Int32 SearchId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ProductRomanShadeYardage (ShadeName,StoreId,WidthStandardAllowance,LengthStandardAllowance,DisplayOrder,Active,CreatedOn,FabricPerYardCost,ManufuacturingCost,MechanismOption,Duties,Lined,LinedInterlined,BlackoutLining) values('" + txtShadeName.Text.Trim().ToString().Replace("'", "''") + "'," + ddlStoreName.SelectedValue + "," + WidthStandardAllowance + "," + LengthStandardAllowance + "," + DisplayOrder + "," + Active + ",Getdate()," + txtfabricyard.Text.ToString() + "," + txtmenufacturercost.Text.ToString() + "," + MechanismOption + "," + Duties + "," + Lined + "," + LinedInterlined + "," + BlackoutLining + ") Select Scope_identity();"));
                    if (SearchId > 0)
                    {
                        if (Request.QueryString["romanproductType"] != null)
                        {
                            if (Request.QueryString["ProductID"] != null)
                            {
                                Response.Redirect("/Admin/Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&romanproductType=1&ID=" + Request.QueryString["ProductID"].ToString());
                            }
                            else
                            {
                                Response.Redirect("/Admin/Products/Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&romanproductType=1");
                            }
                        }
                        else
                        {
                            Response.Redirect("ProductRomanShadeYardageList.aspx?status=inserted");
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] char</param>
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