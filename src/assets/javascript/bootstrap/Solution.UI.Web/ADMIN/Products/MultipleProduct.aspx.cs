using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class MultipleProduct : Solution.UI.Web.BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
    
            if (!IsPostBack)
            {
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            int updateCount = 0;
            string strProductName = "";
            string strErrorMsg = "";
            decimal price = 0, saleprice = 0, tempprice = 0, tempsaleprice = 0;
            string[] ProductIDs = ((Session["ProductIDs"]).ToString()).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string PID in ProductIDs)
            {
                ProductComponent objProdComponent = new ProductComponent();
                DataSet dsProduct = ProductComponent.GetProductByProductID(Convert.ToInt32(PID));
                price = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Price"].ToString());
                saleprice = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["SalePrice"].ToString());
                strProductName = dsProduct.Tables[0].Rows[0]["Name"].ToString();

                string strUpdateQuery = "";
                //for SEDescription
                if (ddlSEDescription.SelectedValue.ToString() == "Append")
                    strUpdateQuery = strUpdateQuery + " ,SEDescription = CONVERT(varchar(MAX), SEDescription) + CONVERT(varchar(MAX), '" + txtSEDescription1.Text + "')";
                else if (ddlSEDescription.SelectedValue.ToString() == "Replace")
                    strUpdateQuery = strUpdateQuery + " ,SEDescription = REPLACE(SEDescription, '" + txtSEDescription1.Text + "', '" + txtSEDescription2.Text + "') ";
                else if (ddlSEDescription.SelectedValue.ToString() == "New")
                    strUpdateQuery = strUpdateQuery + " ,SEDescription = '" + txtSEDescription1.Text + "' ";

                //for SEKeywords
                if (ddlSEKeyword.SelectedValue.ToString() == "Append")
                    strUpdateQuery = strUpdateQuery + " ,SEKeywords  = CONVERT(varchar(MAX), SEKeywords ) + CONVERT(varchar(MAX), '" + txtSEKeywords1.Text + "')";
                else if (ddlSEKeyword.SelectedValue.ToString() == "Replace")
                    strUpdateQuery = strUpdateQuery + " ,SEKeywords  = REPLACE(SEKeywords , '" + txtSEKeywords1.Text + "', '" + txtSEKeywords2.Text + "') ";
                else if (ddlSEKeyword.SelectedValue.ToString() == "New")
                    strUpdateQuery = strUpdateQuery + " ,SEKeywords  = '" + txtSEKeywords1.Text + "' ";

                //for SETitle
                if (ddlSETitle.SelectedValue.ToString() == "Append")
                    strUpdateQuery = strUpdateQuery + " ,SETitle   = CONVERT(varchar(MAX), SETitle) + CONVERT(varchar(MAX), '" + txtSETitle1.Text + "')";
                else if (ddlSETitle.SelectedValue.ToString() == "Replace")
                    strUpdateQuery = strUpdateQuery + " ,SETitle   = REPLACE(SETitle  , '" + txtSETitle1.Text + "', '" + txtSETitle2.Text + "') ";
                else if (ddlSETitle.SelectedItem.Text.ToLower().Contains("new"))
                    strUpdateQuery = strUpdateQuery + " ,SETitle   = '" + txtSETitle1.Text + "' ";

                //for price
                tempprice = price;
                if (ddlPrice.SelectedItem.Text.ToLower().Contains("percentage increment"))
                {
                    tempprice = price + (price * Convert.ToDecimal(Convert.ToDecimal(txtPrice.Text) / 100));
                }
                else if (ddlPrice.SelectedItem.Text.ToLower().Contains("percentage decrement"))
                {
                    tempprice = price - (price * Convert.ToDecimal(Convert.ToDecimal(txtPrice.Text) / 100));
                }
                else if (ddlPrice.SelectedItem.Text.ToLower().Contains("amount increment"))
                {
                    tempprice = price + Convert.ToDecimal(txtPrice.Text);
                }
                else if (ddlPrice.SelectedItem.Text.ToLower().Contains("amount decrement"))
                {
                    tempprice = price - Convert.ToDecimal(txtPrice.Text);
                }
                else if (ddlPrice.SelectedItem.Text.ToLower().Contains("new"))
                {
                    tempprice = Convert.ToDecimal(txtPrice.Text);
                }

                //for SalePrice
                tempsaleprice = saleprice;
                if (ddlSalesPrice.SelectedItem.Text.ToLower().Contains("percentage increment"))
                {
                    tempsaleprice = saleprice + (saleprice * Convert.ToDecimal(Convert.ToDecimal(txtSalesPrice.Text) / 100));
                }
                else if (ddlSalesPrice.SelectedItem.Text.ToLower().Contains("percentage decrement"))
                {
                    tempsaleprice = saleprice - (saleprice * Convert.ToDecimal(Convert.ToDecimal(txtSalesPrice.Text) / 100));

                }
                else if (ddlSalesPrice.SelectedItem.Text.ToLower().Contains("amount increment"))
                {
                    tempsaleprice = saleprice + Convert.ToDecimal(txtSalesPrice.Text);
                }
                else if (ddlSalesPrice.SelectedItem.Text.ToLower().Contains("amount decrement"))
                {
                    tempsaleprice = saleprice - Convert.ToDecimal(txtSalesPrice.Text);
                }
                else if (ddlSalesPrice.SelectedItem.Text.ToLower().Contains("new"))
                {
                    tempsaleprice = Convert.ToDecimal(txtSalesPrice.Text);
                }

                if (tempprice >= tempsaleprice && tempprice >= 0 && tempsaleprice >= 0)
                {
                    strUpdateQuery = strUpdateQuery + " ,Price=" + tempprice;
                    strUpdateQuery = strUpdateQuery + " ,SalePrice=" + tempsaleprice;
                }
                //for Inventory
                if (ddlInventory.SelectedItem.Text.ToLower().Contains("increment"))
                    strUpdateQuery = strUpdateQuery + " ,Inventory=Inventory +" + Convert.ToDecimal(txtInventory.Text);
                else if (ddlInventory.SelectedItem.Text.ToLower().Contains("decrement"))
                    strUpdateQuery = strUpdateQuery + " ,Inventory=Inventory -" + Convert.ToDecimal(txtInventory.Text);
                else if (ddlInventory.SelectedItem.Text.ToLower().Contains("new"))
                    strUpdateQuery = strUpdateQuery + " ,Inventory=" + Convert.ToDecimal(txtInventory.Text);
                strUpdateQuery = strUpdateQuery.Trim().TrimStart(",".ToCharArray());

                // string AllProductIDs = Convert.ToString(Session["ProductIDs"]);
                if (!string.IsNullOrEmpty(strUpdateQuery))
                {
                    int RowsAffected = ProductComponent.UpdateMultipleProduct(strUpdateQuery, Convert.ToInt32(PID));
                    if (RowsAffected > 0)
                    {
                        updateCount = updateCount + 1;
                    }
                    else
                    {
                        strErrorMsg += strProductName + " , ";
                    }
                }
                else
                {
                    strErrorMsg += strProductName + " , ";
                }

            }
            if (!string.IsNullOrEmpty(strErrorMsg))
            {
                trErrorMsg.Visible = true;
                lblErrorMsg.Text = "Does not update following Product(s): " + strErrorMsg + "<br/>" +
                    "Total updated Product(s): " + updateCount;
                //Session["ProductIDs"] = null;
            }
            else
            {
                Response.Redirect("ProductList.aspx?Update=true");
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductList.aspx?StoreID=1");
        }


    }
}