using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class featureCategory : Solution.UI.Web.BasePage //System.Web.UI.Page
    {
        #region "Declaration"
        CategoryComponent objCategory = new CategoryComponent();
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
                if (Request.QueryString["mode"] != null)
                {
                    string mode = Request.QueryString["mode"].ToString();
                    if (mode == "category")
                    {
                        BindCategory();
                        dvProduct.Style.Add("display", "none");
                        dvCategory.Style.Add("display", "");
                        dvBestseller.Style.Add("display", "none");
                        dvBestseller.Visible = false;
                        dvCategory.Visible = true;
                        dvProduct.Visible = false;
                        dvNewArrival.Visible = false;
                        dvNewArrival.Style.Add("display", "none");
                    }
                    else if (mode == "feature")
                    {
                        BindFeature();
                        dvProduct.Style.Add("display", "");
                        dvCategory.Style.Add("display", "none");
                        dvCategory.Visible = false;
                        dvProduct.Visible = true;
                        dvBestseller.Style.Add("display", "none");
                        dvBestseller.Visible = false;
                        dvNewArrival.Visible = false;
                        dvNewArrival.Style.Add("display", "none");
                    }
                    else if (mode == "best")
                    {
                        BindBest();
                        dvProduct.Style.Add("display", "none");
                        dvCategory.Style.Add("display", "none");
                        dvCategory.Visible = false;
                        dvProduct.Visible = false;
                        dvBestseller.Style.Add("display", " ");
                        dvBestseller.Visible = true;
                        dvNewArrival.Visible = false;
                        dvNewArrival.Style.Add("display", "none");
                    }
                    else if (mode == "new")
                    {
                        BindNewArrival();
                        dvProduct.Style.Add("display", "none");
                        dvCategory.Style.Add("display", "none");
                        dvCategory.Visible = false;
                        dvProduct.Visible = false;
                        dvBestseller.Style.Add("display", "none");
                        dvBestseller.Visible = false;
                        dvNewArrival.Visible = true;
                        dvNewArrival.Style.Add("display", "");
                    }
                }
            }

        }

        #region "New Arrival"

        /// <summary>
        /// Binds the New Arrival
        /// </summary>
        private void BindNewArrival()
        {
            DataSet dsProduct = null;
            //By Store ID
            if (Request.QueryString["StoreID"] != null && txtNewarrival.Text.Trim() == "" && txtNewarrival.Text.Trim().Length <= 0)
            {
                dsProduct = ProductComponent.GetProductByStoreID(Convert.ToInt32(Request.QueryString["StoreID"]));
            }
            else if (Request.QueryString["StoreID"] != null && txtNewarrival.Text.Trim() != "" && txtNewarrival.Text.Trim().Length >= 0)
            {
                dsProduct = ProductComponent.GetIndexPageConfig(Convert.ToInt32(Request.QueryString["StoreID"]), "%" + txtNewarrival.Text.Trim() + "%");
            }
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdNewArrival.DataSource = dsProduct;
                grdNewArrival.DataBind();
            }
            else
            {
                grdNewArrival.DataSource = null;
                grdNewArrival.DataBind();
            }

        }

        /// <summary>
        ///  New Arrival Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdNewArrival_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNewArrival.PageIndex = e.NewPageIndex;
            BindNewArrival();

        }

        /// <summary>
        ///  New Arrival Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnNewarrivalsearch_Click(object sender, ImageClickEventArgs e)
        {
            BindNewArrival();
        }

        /// <summary>
        ///  New Arrival Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnNewarrivalshow_Click(object sender, ImageClickEventArgs e)
        {
            txtNewarrival.Text = "";
            BindNewArrival();
        }

        /// <summary>
        ///  New Arrival Add to Selection Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnNewaddtoselection_Click(object sender, ImageClickEventArgs e)
        {
            int TotalRowCount = grdNewArrival.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnProductid = null;
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdNewArrival.Rows[i].FindControl("chkSelect");
                hdnProductid = (HiddenField)grdNewArrival.Rows[i].FindControl("hdnProductid");
                if (chkSelect.Checked == true)
                {
                    ProductComponent.UpdateProductNewArrival(Convert.ToInt32(hdnProductid.Value.Trim()), true, Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (chkSelect.Checked == false)
                {
                    ProductComponent.UpdateProductNewArrival(Convert.ToInt32(hdnProductid.Value.Trim()), false, Convert.ToInt32(Request.QueryString["StoreID"]));
                }



            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }
        #endregion

        #region "For Update Best Seller"

        /// <summary>
        /// Binds the Best Seller
        /// </summary>
        private void BindBest()
        {
            DataSet dsProduct = null;
            //By Store ID
            if (Request.QueryString["StoreID"] != null && txtBestkey.Text.Trim() == "" && txtBestkey.Text.Trim().Length <= 0)
            {
                dsProduct = ProductComponent.GetProductByStoreID(Convert.ToInt32(Request.QueryString["StoreID"]));
            }
            else if (Request.QueryString["StoreID"] != null && txtBestkey.Text.Trim() != "" && txtBestkey.Text.Trim().Length >= 0)
            {
                dsProduct = ProductComponent.GetIndexPageConfig(Convert.ToInt32(Request.QueryString["StoreID"]), "%" + txtBestkey.Text.Trim() + "%");
            }
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdBestproduct.DataSource = dsProduct;
                grdBestproduct.DataBind();
            }
            else
            {
                grdBestproduct.DataSource = null;
                grdBestproduct.DataBind();
            }

        }

        /// <summary>
        ///  Best Seller Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdBestproduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdBestproduct.PageIndex = e.NewPageIndex;
            BindBest();
        }

        /// <summary>
        ///  Best Seller Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnbestSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindBest();
        }

        /// <summary>
        ///  Best Seller Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnbestshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtBestkey.Text = "";
            BindBest();
        }

        /// <summary>
        ///  Best Seller Add to Selection Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnBestAddtoselection_Click(object sender, ImageClickEventArgs e)
        {
            int TotalRowCount = grdBestproduct.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnProductid = null;
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdBestproduct.Rows[i].FindControl("chkSelect");
                hdnProductid = (HiddenField)grdBestproduct.Rows[i].FindControl("hdnProductid");
                if (chkSelect.Checked == true)
                {
                    ProductComponent.UpdateProductBestSeller(Convert.ToInt32(hdnProductid.Value.Trim()), true, Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (chkSelect.Checked == false)
                {
                    ProductComponent.UpdateProductBestSeller(Convert.ToInt32(hdnProductid.Value.Trim()), false, Convert.ToInt32(Request.QueryString["StoreID"]));
                }



            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }

        #endregion

        #region "For Update Feature System"

        /// <summary>
        /// Binds the Featured System
        /// </summary>
        private void BindFeature()
        {
            if (Request.QueryString["StoreID"] != null)
            {
                BindProduct();
            }
        }

        /// <summary>
        /// Binds the Product into Drop Down
        /// </summary>
        private void BindProduct()
        {
            DataSet dsProduct = null;
            //By Store ID
            if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() == "" && txtFeaturesystem.Text.Trim().Length <= 0)
            {
                dsProduct = ProductComponent.GetProductByStoreID(Convert.ToInt32(Request.QueryString["StoreID"]));
            }
            else if (Request.QueryString["StoreID"] != null && txtFeaturesystem.Text.Trim() != "" && txtFeaturesystem.Text.Trim().Length >= 0)
            {
                dsProduct = ProductComponent.GetIndexPageConfig(Convert.ToInt32(Request.QueryString["StoreID"]), "%" + txtFeaturesystem.Text.Trim() + "%");
            }

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdFeaturesystem.DataSource = dsProduct;
                grdFeaturesystem.DataBind();
            }
            else
            {
                grdFeaturesystem.DataSource = null;
                grdFeaturesystem.DataBind();
            }
        }

        /// <summary>
        ///  Feature System Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdFeaturesystem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdFeaturesystem.PageIndex = e.NewPageIndex;
            BindProduct();
        }

        /// <summary>
        ///  Feature System Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnFeaturesystemsearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFeaturesystem.Text != "")
                BindProduct();
        }

        /// <summary>
        ///  Feature System Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnfeaturesystemshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtFeaturesystem.Text = "";
            BindProduct();
        }

        /// <summary>
        ///  Feature System Add to Selection List Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnFeaturesystemaddtoselectionlist_Click(object sender, ImageClickEventArgs e)
        {
            int TotalRowCount = grdFeaturesystem.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnProductid = null;
            int Cnt = 0;
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdFeaturesystem.Rows[i].FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Cnt++;
                }
            }
            if (!string.IsNullOrEmpty(hdnTotFeaturecnt1.Value) && hdnTotFeaturecnt1.Value != "0" && Convert.ToInt32(hdnTotFeaturecnt1.Value) > 0)
            {
                int TotFeaCnt = Convert.ToInt32(hdnTotFeaturecnt1.Value) + Cnt;
                if (TotFeaCnt > 18)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "MsgValidate", "jAlert('You can not select more than 18 Product(s)!','Message');", true);
                    return;
                }
            }

            if (Cnt > 18)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MsgValidate", "jAlert('You can not select more than 18 Product(s)!','Message');", true);
                return;
            }

            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdFeaturesystem.Rows[i].FindControl("chkSelect");
                hdnProductid = (HiddenField)grdFeaturesystem.Rows[i].FindControl("hdnProductid");
                if (chkSelect.Checked == true)
                {
                    ProductComponent.UpdateProductFeature(Convert.ToInt32(hdnProductid.Value.Trim()), true, Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                //else if (chkSelect.Checked == false)
                //{
                //    ProductComponent.UpdateProductFeature(Convert.ToInt32(hdnProductid.Value.Trim()), false, Convert.ToInt32(Request.QueryString["StoreID"]));
                //}
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);

        }

        #endregion

        #region "For Update Feature Category"

        /// <summary>
        /// Method for bind gridview with active category
        /// </summary>
        private void BindCategory()
        {
            if (Request.QueryString["StoreID"] != null)
            {
                DataSet dsCategory = new DataSet();
                if (txtsearch.Text == "")
                {
                    dsCategory = objCategory.getCategoryDetailsbyStoreId(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (txtsearch.Text != "" && txtsearch.Text != null)
                {
                    dsCategory = objCategory.getcategorydetailsbynameforsearch("%" + txtsearch.Text.Trim() + "%", Convert.ToInt32(Request.QueryString["StoreID"]));
                }

                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    grdCategory.DataSource = dsCategory;
                    grdCategory.DataBind();
                }
                else
                {
                    grdCategory.DataSource = null;
                    grdCategory.DataBind();
                }
            }
        }

        /// <summary>
        ///  Category Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCategory.PageIndex = e.NewPageIndex;
            BindCategory();
        }

        /// <summary>
        /// Search Button click event to search category name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtsearch.Text.Trim() != "" && txtsearch.Text.Trim().Length > 0)
            {
                BindCategory();
            }
        }

        /// <summary>
        /// Show All Button click event to show all category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnShowall_Click(object sender, ImageClickEventArgs e)
        {
            txtsearch.Text = "";
            BindCategory();
        }

        /// <summary>
        /// Add to select category as a feature category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnAddtoselect_Click(object sender, ImageClickEventArgs e)
        {
            int TotalRowCount = grdCategory.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnCategoryid = null;
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdCategory.Rows[i].FindControl("chkSelect");
                hdnCategoryid = (HiddenField)grdCategory.Rows[i].FindControl("hdnCategoryid");
                if (chkSelect.Checked == true)
                {
                    CategoryComponent.UpdateCategoryFeature(Convert.ToInt32(hdnCategoryid.Value.Trim()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")), true);
                }
                else if (chkSelect.Checked == false)
                {
                    CategoryComponent.UpdateCategoryFeature(Convert.ToInt32(hdnCategoryid.Value.Trim()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")), false);
                }

            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }

        /// <summary>
        /// Category Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCategoryid");
                Label lblParent = (Label)e.Row.FindControl("lblParent");
                DataSet dsPname = CategoryComponent.GetParentCategoryNamebyCategoryID(Convert.ToInt16(hdnCatid.Value));

                if (dsPname != null && dsPname.Tables.Count > 0 && dsPname.Tables[0].Rows.Count > 0)
                {
                    int pCount = dsPname.Tables[0].Rows.Count;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < pCount; i++)
                    {
                        sb.Append(dsPname.Tables[0].Rows[i]["Name"].ToString() + ", ");

                    }
                    int length = sb.ToString().Length;
                    string pName = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
                    lblParent.Text = pName.ToString();
                }
            }
        }
        #endregion
    }
}