using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ProductSearch : BasePage
    {
        DataSet dsCat = new DataSet();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                txtSearch.Focus();
                if (Request.QueryString["Upgrade"] != null || Request.QueryString["SKU"] != null)
                {
                    if (Request.QueryString["PID"] != null)
                        BindProductVariantDetails();
                }

                imgLogo.Src = AppLogic.AppConfigs("LIVE_SERVER").TrimEnd("/".ToCharArray()) + "/images/logo.png";
                imgMainDiv.Src = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                btnGo.ImageUrl = "/App_Themes/" + Page.Theme + "/images/search.gif";
                btnupdate.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnVarProsave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid();
        }

        /// <summary>
        /// Function for Fill product Data Grid
        /// </summary>
        private void FillGrid()
        {
            DataSet dsProduct = new DataSet();

            if (Request.QueryString["Upgrade"] != null || Request.QueryString["Normal"] != null)
            {
                String strSql = "SELECT ProductId,SKU,Inventory,Name,(case when(isnull(SalePrice,0)=0) then price else SalePrice end) as price FROM  tb_Product WHERE (isPack!='1' or isPack is null) AND (StoreID = " + Convert.ToInt32(Request.QueryString["SID"]) + ") AND (Active = 1) and ProductID not in(Select ProductID from tb_GiftCardProduct) ";
                strSql += (!string.IsNullOrEmpty(ddlSearch.SelectedValue)) ? " and (" + ddlSearch.SelectedValue + " like '%" + txtSearch.Text.Trim().ToString().Replace("'", "''") + "%')" : string.Empty;
                dsProduct = CommonComponent.GetCommonDataSet(strSql);
            }
            else if (Request.QueryString["SKU"] != null && Request.QueryString["cid"] != null)
            {
                String strSql = "SELECT ProductId,SKU,Inventory,Name,(case when(isnull(SalePrice,0)=0) then price else SalePrice end) as price FROM  tb_Product WHERE  (StoreID = " + Convert.ToInt32(Request.QueryString["SID"]) + ") AND (Active = 1) and ProductID not in(Select ProductID from tb_GiftCardProduct) ";
                strSql += (!string.IsNullOrEmpty(ddlSearch.SelectedValue)) ? " and (" + ddlSearch.SelectedValue + " like '%" + txtSearch.Text.Trim().ToString().Replace("'", "''") + "%')" : string.Empty;
                dsProduct = CommonComponent.GetCommonDataSet(strSql);
            }
            

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                gvOldPOrder.DataSource = dsProduct;
                gvOldPOrder.DataBind();
                lblMsg.Text = "";
                gvOldPOrder.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                gvOldPOrder.DataSource = null;
                gvOldPOrder.Visible = false;
                btnSave.Visible = false;
                lblMsg.Text = "No Record(s) Found.";
            }
        }

        /// <summary>
        ///  Old PO Order Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvOldPOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOldPOrder.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow dr in gvOldPOrder.Rows)
            {
                System.Web.UI.HtmlControls.HtmlInputRadioButton objRdo = (System.Web.UI.HtmlControls.HtmlInputRadioButton)dr.FindControl("rdcheckid");
                Label lblPrice = (Label)dr.FindControl("lblPrice");
                Label lblinventroy = (Label)dr.FindControl("lblACost");
                Label lblSKU = (Label)dr.FindControl("lblVName");
                Label lblPOName = (Label)dr.FindControl("lblPOName");

                Label lblProductID = (Label)dr.FindControl("ProductId");

                if (objRdo.Checked == true)
                {
                    if (Request.QueryString["setValue"] != null && Request.QueryString["Upgrade"] != null)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "set", " window.opener.document.getElementById('" + Request.QueryString["setValue"].ToString() + "').value='" + lblProductID.Text.Trim() + "';  window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lblVProductID').innerHTML='" + lblSKU.Text + "'; window.close(); ", true);
                        return;
                    }
                    else if (Request.QueryString["VVID"] != null && Request.QueryString["Upgrade"] != null)
                    {
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set VariantProductID=" + lblProductID.Text + " where VariantValueID=" + Request.QueryString["VVID"].ToString());
                    }
                    else if (Request.QueryString["Upgrade"] != null)
                    {
                        CommonComponent.ExecuteCommonData(@"update tb_OrderedPackageShoppingCartItems set UpgradeProductID=" + lblProductID.Text + ", UpgradeSKU=" + SQuote(lblSKU.Text) + ", UpgradeQuantity=Quantity,UpgradePrice=" + lblPrice.Text +
                        @" where OrderedPackageCartID=" + Request.QueryString["OPCID"].ToString());

                        //Replace Variant Values when Upgrade
                        CommonComponent.ExecuteCommonData(@"update OSCI set VariantValues=replace(VariantValues,OPSCI.Name,'" + lblPOName.Text.Trim() + @"') 
                        from tb_OrderedShoppingCartItems OSCI, tb_OrderedPackageShoppingCartItems OPSCI 
                        where OSCI.OrderedCustomCartID=OPSCI.OrderedCustomCartID and OrderedPackageCartID=" + Request.QueryString["OPCID"].ToString());
                    }
                    else if (Request.QueryString["SKU"] != null && Request.QueryString["cid"] != null)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET upgradeprice='" + lblPrice.Text.ToString().Replace("$", "") + "', marryproducts=" + SQuote(lblSKU.Text) + " WHERE OrderedCustomCartID=" + Request.QueryString["cid"] + "");
                    }
                    else if (Request.QueryString["Normal"] != null)
                    {
                        CommonComponent.ExecuteCommonData(@"update tb_OrderedShoppingCartItems set UpgradeProductID=" + lblProductID.Text + ", UpgradeSKU=" + SQuote(lblSKU.Text) + ", UpgradeQuantity=Quantity,NewPrice=" + lblPrice.Text +
                        @" where OrderedCustomCartID=" + Request.QueryString["OCCID"].ToString());
                    }

                    lblMsg.Text = "Record Updated Successfully.";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PageRefresh", "javascript:var result=window.opener.location.href;result =result.replace('potab=1','');result =result+'&potab=1'; window.opener.location.href=result; window.close();", true);
                    return;
                }
            }

            if (Request.QueryString["PID"] != null)
            {
                foreach (GridViewRow dr1 in GVVariantProduct.Rows)
                {
                    System.Web.UI.HtmlControls.HtmlInputRadioButton objRdo1 = (System.Web.UI.HtmlControls.HtmlInputRadioButton)dr1.FindControl("rdcheckid");
                    Label lblPrice1 = (Label)dr1.FindControl("lblPrice");
                    Label lblinventroy1 = (Label)dr1.FindControl("lblACost");
                    Label lblSKU1 = (Label)dr1.FindControl("lblVName");
                    Label lblPOName = (Label)dr1.FindControl("lblPOName");

                    Label lblProductID1 = (Label)dr1.FindControl("ProductId");

                    if (objRdo1.Checked == true)
                    {
                        if (Request.QueryString["setValue"] != null && Request.QueryString["Upgrade"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", " window.opener.document.getElementById('" + Request.QueryString["setValue"].ToString() + "').value='" + lblProductID1.Text.Trim() + "';  window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lblVProductID').innerHTML='" + lblSKU1.Text + "'; window.close(); ", true);
                            return;
                        }
                        else if (Request.QueryString["VVID"] != null && Request.QueryString["Upgrade"] != null)
                        {
                            CommonComponent.ExecuteCommonData(" update tb_ProductVariantValue set VariantProductID=" + lblProductID1.Text + " where VariantValueID=" + Request.QueryString["VVID"].ToString());
                        }
                        else if (Request.QueryString["Upgrade"] != null)
                        {
                            CommonComponent.ExecuteCommonData(@"update tb_OrderedPackageShoppingCartItems set UpgradeProductID=" + lblProductID1.Text + ", UpgradeSKU=" + SQuote(lblSKU1.Text) + ", UpgradeQuantity=Quantity,UpgradePrice=" + lblPrice1.Text +
                            @" where OrderedPackageCartID=" + Request.QueryString["OPCID"].ToString());

                            //Replace Variant Values when Upgrade
                            CommonComponent.ExecuteCommonData(@"update OSCI set VariantValues=replace(VariantValues,OPSCI.Name,'" + lblPOName.Text.Trim() + @"') 
                            from tb_OrderedShoppingCartItems OSCI, tb_OrderedPackageShoppingCartItems OPSCI 
                            where OSCI.OrderedCustomCartID=OPSCI.OrderedCustomCartID and OrderedPackageCartID=" + Request.QueryString["OPCID"].ToString());
                        }
                        else if (Request.QueryString["Normal"] != null)
                        {
                            CommonComponent.ExecuteCommonData(@"update tb_OrderedShoppingCartItems set UpgradeProductID=" + lblProductID1.Text + ", UpgradeSKU=" + SQuote(lblSKU1.Text) + ", UpgradeQuantity=Quantity,NewPrice=" + lblPrice1.Text +
                            @" where OrderedCustomCartID=" + Request.QueryString["OCCID"].ToString());
                        }

                        lblMsg.Text = "Record Updated Successfully.";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageRefresh", "javascript:var result=window.opener.location.href;result =result.replace('potab=1','');result =result+'&potab=1'; window.opener.location.href=result; window.close();", true);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// Binds the product variant details.
        /// </summary>
        private void BindProductVariantDetails()
        {
            Int32 VariantProductID = Convert.ToInt32(Request.QueryString["PID"].ToString());
            DataSet dtProductVariant = new DataSet();
            dtProductVariant = CommonComponent.GetCommonDataSet("select distinct tb_product.Name,tb_product.ProductId,tb_product.SKU,tb_product.Price,tb_product.Inventory  from dbo.tb_ProductVariantValue " +
                                " inner join tb_product on tb_product.ProductID=tb_ProductVariantValue .VariantProductID " +
                                " Where VariantID in (select VariantID from dbo.tb_ProductVariantValue Where ProductID=" + Convert.ToInt32(Request.QueryString["ParenetProductID"]) + " and variantproductID=" + VariantProductID + ") " +
                                " and VariantProductID not in (" + VariantProductID + ")");

            if (dtProductVariant != null && dtProductVariant.Tables.Count > 0 && dtProductVariant.Tables[0].Rows.Count > 0)
            {
                GVVariantProduct.DataSource = dtProductVariant;
                GVVariantProduct.DataBind();
                btnSave.Visible = true;
            }
        }

        /// <summary>
        ///  Variant Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void GVVariantProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOldPOrder.PageIndex = e.NewPageIndex;
            BindProductVariantDetails();
        }

        /// <summary>
        ///  Insert Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            if (validation())
            {
                if (Convert.ToBoolean(CommonComponent.ExecuteCommonData("INSERT INTO dbo.tb_Product (StoreID ,Name ,SKU ,Description ,Price ,SalePrice ,Weight ,Inventory ,CreatedOn ,Active ,Deleted) " +
                      " VALUES  (" + Request.QueryString["SID"].ToString() + ",'" + txtProductName.Text.Trim() + "','RSC-'+ CONVERT(VARCHAR(100)," + Request.QueryString["VVID"].ToString() + ") ,'" + txtDescription.Text.Trim() + "'," + Convert.ToDecimal(txtprice.Text.Trim()) + "," + Convert.ToDecimal(txtprice.Text.Trim()) + ",1,999,GetDate(),1,1)")))
                {
                    pnlAddpro.Visible = false;
                    lblMsg.Text = "Product Inserted Successfully...";
                }
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            pnlAddpro.Visible = false;
        }

        /// <summary>
        ///  Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnupdate_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToBoolean(CommonComponent.ExecuteCommonData("update tb_product set name='" + txtProductName.Text.Trim() + "',price=" + Convert.ToDecimal(txtprice.Text.Trim()) + ",SalePrice=" + Convert.ToDecimal(txtprice.Text.Trim()) + " where SKU='RSC-" + Request.QueryString["VVID"].ToString() + "' and deleted=1")))
            {
                lblMsg.Text = "Product Updated Successfully...";
                pnlAddpro.Visible = false;
            }
        }

        /// <summary>
        /// Validate that product is Exists or not
        /// </summary>
        /// <returns>true if Exists, false otherwise</returns>
        private bool validation()
        {
            bool flag = true;
            DataSet dspro = new DataSet();
            dspro = CommonComponent.GetCommonDataSet("select name,price from tb_product where SKU='RSC-" + Request.QueryString["VVID"].ToString() + "' and deleted=1");

            if (dspro != null && dspro.Tables.Count > 0 && dspro.Tables[0].Rows.Count > 0)
            {
                lblMsg.Text = "Product Already Exists...";
                txtProductName.Text = dspro.Tables[0].Rows[0]["Name"].ToString();
                txtprice.Text = dspro.Tables[0].Rows[0]["Price"].ToString();
                btnVarProsave.Visible = false;
                btnupdate.Visible = true;
                flag = false;
            }
            return flag;
        }

        /// <summary>
        ///  Add Variant Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddVariantProduct_Click(object sender, EventArgs e)
        {
            txtDescription.Text = "";
            txtInventory.Text = "";
            txtprice.Text = "";
            txtProductName.Text = "";
            txtsaleprice.Text = "";
            txtweight.Text = "";
            pnlAddpro.Visible = true;
            btnVarProsave.Visible = true;
            btnupdate.Visible = false;
        }

        /// <summary>
        /// Change the string,Replace the single Quote to Space
        /// </summary>
        /// <param name="s">string s</param>
        /// <returns>Change the string, Replace the single Quote to Space</returns>
        public String SQuote(String s)
        {
            int len = s.Length + 25;
            System.Text.StringBuilder tmpS = new System.Text.StringBuilder(len);
            tmpS.Append("N'");
            tmpS.Append(s.Replace("'", "''"));
            tmpS.Append("'");
            return tmpS.ToString();
        }
    }
}