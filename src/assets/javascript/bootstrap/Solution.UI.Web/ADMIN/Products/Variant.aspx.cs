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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Variant : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = "";
                btnSaveOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancelOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnAddOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-option.png";
                btnDeleteOptionValue.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
                btnUpdateVariantValue.ImageUrl = "/App_Themes/" + Page.Theme + "/images/update.png";
                btnAddOptionValue.ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-option-value.png";
                btnDeleteOption.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";

                btnUpdateVariantName.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Update-option-name.png";

                if (Request.QueryString["ID"] != null && Convert.ToInt32(Request.QueryString["ID"]) > 0)
                {
                    ProductComponent objProduct = new ProductComponent();
                    tb_Product tb_Product = new tb_Product();
                    tb_Product = objProduct.GetAllProductDetailsbyProductID(Convert.ToInt32(Request.QueryString["ID"]));
                    lblProductName.Text = Convert.ToString(tb_Product.Name);
                    CheckProductVariant(Convert.ToInt32(Request.QueryString["ID"]));
                }
                btnDeleteOption.Visible = false;
            }
            Page.MaintainScrollPositionOnPostBack = true;

        }

        /// <summary>
        /// Check Product Variant if Already Exists
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        private void CheckProductVariant(Int32 ProductID)
        {
            btnAddOptionValue.Visible = false;
            ProductComponent objProduct = new ProductComponent();
            DataSet DsProductVariant = new DataSet();
            DsProductVariant = objProduct.GetProductVariantcount(ProductID);
            if (DsProductVariant != null && DsProductVariant.Tables.Count > 0 && DsProductVariant.Tables[0].Rows.Count > 0)
            {
                dltVariant.DataSource = DsProductVariant.Tables[0].DefaultView;
                btnAddOptionValue.Visible = true;
            }
            else dltVariant.DataSource = null;
            dltVariant.DataBind();

        }

        /// <summary>
        /// Variant Repeater Item Command Event
        /// </summary>
        /// <param name="source">object source</param>
        /// <param name="e">DataListCommandEventArgs e</param>
        protected void dltVariant_ItemCommand(object source, DataListCommandEventArgs e)
        {
            trAddOption.Visible = false;
            trSavecancel.Visible = false;
            btnDeleteOption.Visible = true;

            Int32 VariantID = 0;
            string VariantName = string.Empty;
            VariantName = ((System.Web.UI.WebControls.LinkButton)(e.CommandSource)).Text;
            txtEditVariantName.Text = VariantName;
            hdnOptionName.Value = VariantName;

            if (e.CommandName != null && e.CommandName != string.Empty)
            {
                VariantID = Convert.ToInt32(e.CommandName);
                if (VariantID > 0)
                {
                    hdnVariantId.Value = VariantID.ToString();
                    BindVariantValue(VariantID);
                }
            }
        }

        /// <summary>
        /// Function for Bind Option Value
        /// </summary>
        /// <param name="VarintId">int VarintId</param>
        public void BindVariantValue(Int32 VarintId)
        {
            DataSet dsVariantValue = new DataSet();
            dsVariantValue = ProductComponent.GetProductVariantValues(Convert.ToInt32(Request.QueryString["ID"]), VarintId);
            if (dsVariantValue != null && dsVariantValue.Tables.Count > 0 && dsVariantValue.Tables[0].Rows.Count > 0)
            {
                grdVariantValue.DataSource = dsVariantValue;
                grdVariantValue.DataBind();
                trCheckall.Visible = true;
            }
            else
            {
                grdVariantValue.DataSource = null;
                grdVariantValue.DataBind();
                trCheckall.Visible = false;
            }
            trVariantValue.Visible = true;
            hdnOption.Value = VarintId.ToString();
            btnDeleteOption.Visible = true;
        }

        /// <summary>
        ///  Add Option Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnAddOption_Click(object sender, ImageClickEventArgs e)
        {
            hdnOption.Value = "0";
            trAddOption.Visible = true;
            trSavecancel.Visible = true;
            ddlOptionName.Visible = false;
            txtOptionName.Visible = true;
            trVariantValue.Visible = false;
            trCheckall.Visible = false;
            btnDeleteOption.Visible = false;
            txtOptionName.Focus();
        }

        /// <summary>
        ///  Delete Option Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnDeleteOption_Click(object sender, ImageClickEventArgs e)
        {
            if (hdnVariantId.Value != "0" && Request.QueryString["ID"] != null)
            {
                DataSet dsVariantUPC = new DataSet();
                try
                {
                    dsVariantUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') AS UPC FROM dbo.tb_ProductVariantValue WHERE ProductID=" + Convert.ToInt32(Request.QueryString["ID"]));
                }
                catch { }
                if (Convert.ToInt32(ProductComponent.DeleteVariantOption(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(hdnVariantId.Value))) > 0)
                {
                    hdnOption.Value = "0";
                    try
                    {
                        if (dsVariantUPC != null && dsVariantUPC.Tables.Count > 0 && dsVariantUPC.Tables[0].Rows.Count > 0)
                        {
                            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                            for (int v = 0; v < dsVariantUPC.Tables[0].Rows.Count; v++)
                            {
                                if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsVariantUPC.Tables[0].Rows[v]["UPC"].ToString().Trim() + ".png")))
                                {
                                    File.Delete(Server.MapPath(FPath + "/UPC-" + dsVariantUPC.Tables[0].Rows[v]["UPC"].ToString().Trim() + ".png"));
                                }
                            }
                        }
                    }
                    catch { }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg08", "jAlert('Option Deleted Successfully!','Message');", true);
                }
                btnDeleteOption.Visible = false;
                CheckProductVariant(Convert.ToInt32(Request.QueryString["ID"]));
            }
            btnAddOptionValue_Click(null, null);
            trAddOption.Visible = false;
            trSavecancel.Visible = false;
        }

        /// <summary>
        ///  Save Option Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSaveOption_Click(object sender, ImageClickEventArgs e)
        {
            string VariantValue = Convert.ToString(txtOptionValue.Text.Trim().Replace("'", "''"));
            decimal VariantPrice = 0;
            if (!string.IsNullOrEmpty(txtOptionPrice.Text))
                VariantPrice = Convert.ToDecimal(txtOptionPrice.Text.Trim());

            Int32 DisplayOrder = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);

            if (hdnOption.Value.ToString() == "0" && txtOptionName.Visible == true)
            {
                if (!string.IsNullOrEmpty(txtOptionName.Text.ToString()) && !string.IsNullOrEmpty(txtOptionValue.Text.ToString()))
                {
                    string VariantName = Convert.ToString(txtOptionName.Text.Trim().Replace("'", "''"));
                    Int32 VariValue = Convert.ToInt32(ProductComponent.SaveProductVariant(Convert.ToInt32(Request.QueryString["ID"]), VariantName, VariantValue, VariantPrice, DisplayOrder, txtOptionSKU.Text.ToString().Trim(), txtOptionHeader.Text.Trim(), txtOptionUPC.Text.Trim(), 0));
                    if (VariValue > 0)
                    {
                        try
                        {
                            GenerateBarcode(txtOptionUPC.Text.Trim());
                        }
                        catch { }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "jAlert('Option Added Successfully!','Message');", true);
                    }
                    CheckProductVariant(Convert.ToInt32(Request.QueryString["ID"]));
                }
                else
                {
                    if (string.IsNullOrEmpty(txtOptionName.Text.ToString()))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg04", "jAlert('Please Enter Option Name!','Message','" + txtOptionName.ClientID.Trim() + "');", true);
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOptionValue.Text.ToString()))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg05", "jAlert('Please Enter Option Value!','Message','" + txtOptionValue.ClientID.Trim() + "');", true);
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtOptionValue.Text.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg06", "jAlert('Please Enter Option Value!','Message','" + txtOptionValue.ClientID.Trim() + "');", true);
                    return;
                }
                else
                {
                    string VariantName = Convert.ToString(ddlOptionName.SelectedItem.Text.Trim());
                    Int32 VariantId = Convert.ToInt32(ddlOptionName.SelectedValue);
                    Int32 VariValue = Convert.ToInt32(ProductComponent.InsertProductVariantValue(VariantId, Convert.ToInt32(Request.QueryString["ID"]), VariantName, VariantValue, VariantPrice, DisplayOrder, txtOptionSKU.Text.ToString().Trim(), txtOptionHeader.Text.Trim(), txtOptionUPC.Text.Trim()));
                    if (VariValue > 0)
                    {
                        try
                        {
                            GenerateBarcode(txtOptionUPC.Text.Trim());
                        }
                        catch { }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg07", "jAlert('Option Value Added Successfully!','Message');", true);
                    }

                    BindVariantValue(Convert.ToInt32(ddlOptionName.SelectedValue));
                }
            }
            trAddOption.Visible = false;
            trSavecancel.Visible = false;
            ClearTextBox();
        }

        /// <summary>
        ///  Cancel Option Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnCancelOption_Click(object sender, ImageClickEventArgs e)
        {
            trAddOption.Visible = false;
            btnDeleteOption.Visible = false;
            trSavecancel.Visible = false;
        }

        /// <summary>
        ///  Add Option Value Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddOptionValue_Click(object sender, ImageClickEventArgs e)
        {
            trVariantValue.Visible = false;
            btnDeleteOption.Visible = false;
            trCheckall.Visible = false;
            hdnOption.Value = "1";
            ddlOptionName.Visible = true;
            txtOptionName.Visible = false;
            trAddOption.Visible = true;
            trSavecancel.Visible = true;

            ProductComponent objProduct = new ProductComponent();
            DataSet DsProductVariant = new DataSet();
            DsProductVariant = objProduct.GetProductVariantcount(Convert.ToInt32(Request.QueryString["ID"]));
            if (DsProductVariant != null && DsProductVariant.Tables.Count > 0 && DsProductVariant.Tables[0].Rows.Count > 0)
            {
                ddlOptionName.DataSource = DsProductVariant.Tables[0].DefaultView;
                ddlOptionName.DataTextField = "VariantName";
                ddlOptionName.DataValueField = "VariantID";
                btnAddOptionValue.Visible = true;
            }
            else
            {
                ddlOptionName.DataSource = null;
                btnAddOptionValue.Visible = false;
            }
            ddlOptionName.DataBind();
        }

        /// <summary>
        ///  Delete Option Value Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnDeleteOptionValue_Click(object sender, ImageClickEventArgs e)
        {
            int VariantValueID = 0;
            int VariantId = 0;
            int ProdId = 0;
            int Cnt = 0;
            if (grdVariantValue.Rows.Count > 0)
            {
                foreach (GridViewRow r in grdVariantValue.Rows)
                {
                    CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                    Label lblVariantValueID = (Label)r.FindControl("lblVariantValueID");
                    Label lblVariantID = (Label)r.FindControl("lblVariantID");
                    TextBox txtgrdoptionupc = (TextBox)r.FindControl("txtgrdoptionupc");
                    VariantValueID = Convert.ToInt32(lblVariantValueID.Text.ToString());
                    VariantId = Convert.ToInt32(lblVariantID.Text.ToString());
                    ProdId = Convert.ToInt32(Request.QueryString["ID"].ToString().Trim());
                    if (chk.Checked)
                    {
                        if (Convert.ToInt32(ProductComponent.DeleteVariantValues(ProdId, VariantId, VariantValueID)) > 0)
                        {
                            try
                            {
                                String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + txtgrdoptionupc.Text.Trim() + ".png")))
                                {
                                    File.Delete(Server.MapPath(FPath + "/UPC-" + txtgrdoptionupc.Text.Trim() + ".png"));
                                }
                            }
                            catch { }
                            Cnt++;
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "jAlert('" + Cnt.ToString() + " Option Value(s) Deleted Successfully!','Message');", true);
                BindVariantValue(VariantId);
            }
        }

        /// <summary>
        /// Function for Clear TextBoxes
        /// </summary>
        private void ClearTextBox()
        {
            txtDisplayOrder.Text = "";
            txtOptionName.Text = "";
            txtOptionPrice.Text = "";
            txtOptionValue.Text = "";
            txtOptionSKU.Text = "";
            txtOptionHeader.Text = "";
            txtOptionUPC.Text = "";
            txtOptionValueDescription.Text = "";
        }

        /// <summary>
        ///  Update variant Value Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnUpdateVariantValue_Click(object sender, ImageClickEventArgs e)
        {
            int VariantValueID = 0;
            int VariantId = 0;
            int ProdId = 0;
            int Cnt = 0;
            if (grdVariantValue.Rows.Count > 0)
            {
                foreach (GridViewRow r in grdVariantValue.Rows)
                {
                    CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                    Label lblVariantValueID = (Label)r.FindControl("lblVariantValueID");
                    Label lblVariantID = (Label)r.FindControl("lblVariantID");
                    TextBox txtgrddisplayorder = (TextBox)r.FindControl("txtgrddisplayorder");
                    TextBox txtgrdOptionPrice = (TextBox)r.FindControl("txtgrdOptionPrice");
                    TextBox txtgrdoptionsku = (TextBox)r.FindControl("txtgrdoptionsku");
                    TextBox txtgrdoptionheader = (TextBox)r.FindControl("txtgrdoptionheader");
                    TextBox txtgrdoptionupc = (TextBox)r.FindControl("txtgrdoptionupc");
                    VariantValueID = Convert.ToInt32(lblVariantValueID.Text.ToString());
                    VariantId = Convert.ToInt32(lblVariantID.Text.ToString());
                    ProdId = Convert.ToInt32(Request.QueryString["ID"]);
                    try
                    {
                        GenerateBarcode(txtgrdoptionupc.Text.Trim());
                    }
                    catch { }
                    if (chk.Checked)
                    {
                        decimal Price = 0;
                        if (!string.IsNullOrEmpty(txtgrdOptionPrice.Text))
                            Price = Convert.ToDecimal(txtgrdOptionPrice.Text);
                        int Display = 0;
                        if (!string.IsNullOrEmpty(txtgrddisplayorder.Text))
                            Display = Convert.ToInt32(txtgrddisplayorder.Text);

                        if (Convert.ToInt32(ProductComponent.UpdateProductVariantValue(VariantId, ProdId, VariantValueID, Price, Display, txtgrdoptionsku.Text.Trim(), txtgrdoptionheader.Text.Trim(), txtgrdoptionupc.Text.Trim())) > 0)
                        {
                            Cnt++;
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "jAlert('" + Cnt.ToString() + " Option Value(s) Updated Successfully!','Message');", true);
                BindVariantValue(VariantId);
            }
        }

        /// <summary>
        ///  Update variant Name Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnUpdateVariantName_OnClick(object sender, ImageClickEventArgs e)
        {
            string strSql = "Update tb_ProductVariant set VariantName='" + txtEditVariantName.Text.Trim().Replace("'", "''") + "' where VariantID=" + hdnVariantId.Value;
            CommonComponent.ExecuteCommonData(strSql);
            CheckProductVariant(Convert.ToInt32(Request.QueryString["ID"]));
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgUpdateOptionName", "jAlert('Option Name Updated Successfully!','Message');", true);
        }

        #region Generate Barcode From OrderNo. By Girish

        /// <summary>
        /// Generates the Barcode
        /// </summary>
        /// <param name="UPCCode">string UPCCode</param>
        private void GenerateBarcode(String UPCCode)
        {
            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
            CreateFolder(FPath.ToString());
            if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png")))
            {
                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                bCodeControl.BarCode = UPCCode.Trim();
                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                bCodeControl.BarCodeHeight = 70;
                bCodeControl.ShowHeader = false;
                bCodeControl.ShowFooter = true;
                bCodeControl.FooterText = "UPC-" + UPCCode.Trim();
                bCodeControl.Size = new System.Drawing.Size(250, 100);
                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"));
            }
        }

        /// <summary>
        /// Creates the folder at Specified path
        /// </summary>
        /// <param name="FPath">string FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        #endregion
    }
}