using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.IO;
namespace Solution.UI.Web.ADMIN.Settings
{
    /// <summary>
    /// Category List Page contains list of category
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public partial class categoryList : BasePage
    {
        #region Declaration
        #region component
        CategoryComponent objCatComponent = new CategoryComponent();

        StoreComponent objStorecomponent = new StoreComponent();
        public static bool isDescendName = false;
        #endregion
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["mode"] != null)
            {
                if (Request.QueryString["mode"].ToString().Equals("new"))
                {
                    lblMessage.Text = "Category Inserted Successfully";
                }
                else if (Request.QueryString["mode"].ToString().Equals("edit"))
                {
                    lblMessage.Text = "Category Updated Successfully";
                }
            }
            else
            {
                lblMessage.Text = "";
            }
            if (!IsPostBack)
            {
                CategoryComponent.Filter = "";
                CategoryComponent.NewFilter = false;
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif";
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                btnShowAll.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
                bindstore();
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                FillCategoryList();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue);
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
                AppConfig.StoreID = 1;
            }
        }

        /// <summary>
        /// Fills the Category List in Gridview.
        /// </summary>
        public void FillCategoryList()
        {
            DataSet dsCategoryList = new DataSet();
            dsCategoryList = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(ddlStore.SelectedValue), Convert.ToString(ddlSearch.SelectedValue), txtSearch.Text.Trim(), Convert.ToString(ddlStatus.SelectedValue));

            if (dsCategoryList != null && dsCategoryList.Tables.Count > 0 && dsCategoryList.Tables[0].Rows.Count > 0)
            {
                gvCategory.DataSource = dsCategoryList.Tables[0];
                gvCategory.DataBind();
            }
            else
            {
                gvCategory.DataSource = null;
                gvCategory.DataBind();
            }
            if (ddlStore.SelectedValue == "-1")
            {

                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Sorting Grid View Ascending or Descending
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    gvCategory.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }


                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gvCategory.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }

                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Category Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvCategory.Rows.Count > 0)
                trBottom.Visible = true;
            else
                trBottom.Visible = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendName == false)
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbName.AlternateText = "Ascending Order";
                    lbName.ToolTip = "Ascending Order";
                    lbName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbName.AlternateText = "Descending Order";
                    lbName.ToolTip = "Descending Order";
                    lbName.CommandArgument = "ASC";
                }
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCatid");
            //    Label lblParent = (Label)e.Row.FindControl("lblParent");
            //    HyperLink hplEdit = (HyperLink)e.Row.FindControl("hplEdit");

            //    DataSet dsPname = CategoryComponent.GetParentCategoryNamebyCategoryID(Convert.ToInt16(hdnCatid.Value));
            //    if (dsPname != null && dsPname.Tables.Count > 0 && dsPname.Tables[0].Rows.Count > 0)
            //    {
            //        int pCount = dsPname.Tables[0].Rows.Count;
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //        for (int i = 0; i < pCount; i++)
            //        {
            //            sb.Append(dsPname.Tables[0].Rows[i]["Name"].ToString() + ", ");

            //        }
            //        int length = sb.ToString().Length;
            //        string pName = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            //        lblParent.Text = pName.ToString();
            //    }
            //}
        }

        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">bool _Active</param>
        /// <returns>Returns the Image Path</returns>
        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedIndex > 0)
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());

            }
            else
            {
                AppConfig.StoreID = 1;
            }
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "";
            FillCategoryList();

        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            //if (txtSearch.Text.Trim() != null)
            //{
            //    gvCategory.DataSourceID = null;
            //    tb_Category ctxCategory = new tb_Category();
            //    ctxCategory.Name = txtSearch.Text.Trim();
            //    gvCategory.DataSource = objCatComponent.getcategorydetailsbyname(txtSearch.Text.Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            //    gvCategory.DataBind();
            //}
            //else
            //{
            //    bindgridviewwithcategory();
            //    gvCategory.DataBind();

            //}

            FillCategoryList();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = gvCategory.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gvCategory.Rows[i].FindControl("hdnCatid");
                CheckBox chk = (CheckBox)gvCategory.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    DeleteImage(hdn.Value);
                    objCatComponent.Deletecategory(Convert.ToInt16(hdn.Value));
                    lblMessage.Text = "Category Deleted Successfully";
                }
            }
            FillCategoryList();

        }

        /// <summary>
        ///  Delete1 Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete1_Click(object sender, EventArgs e)
        {
            int totalRowCount = gvCategory.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gvCategory.Rows[i].FindControl("hdnCatid");
                CheckBox chk = (CheckBox)gvCategory.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    DeleteImage(hdn.Value);
                    objCatComponent.Deletecategory(Convert.ToInt16(hdn.Value));
                    lblMessage.Text = "Category Deleted Successfully";
                }
            }
            FillCategoryList();

        }

        /// <summary>
        /// Deletes the Category image
        /// </summary>
        /// <param name="CategoryId">string CategoryId</param>
        private void DeleteImage(string CategoryId)
        {
            DataSet ds = objCatComponent.getCatdetailbycatid(Convert.ToInt32(CategoryId), Convert.ToInt16(AppConfig.StoreID));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ImageName"].ToString().Trim() != null && ds.Tables[0].Rows[0]["ImageName"].ToString().Trim() != "")
                {
                    string Path = AppLogic.AppConfigs("ImagePathCategory");
                    string imageName = ds.Tables[0].Rows[0]["ImageName"].ToString().Trim();
                    string bannerName = ds.Tables[0].Rows[0]["BannerImageName"].ToString().Trim();
                    string iconFile = Server.MapPath(Path) + "Icon/" + imageName;
                    string bannerFile = Server.MapPath(Path) + "Banner/" + bannerName;
                    string largeFile = Server.MapPath(Path) + "Large/" + imageName;
                    string medium = Server.MapPath(Path) + "Medium/" + imageName;
                    string micro = Server.MapPath(Path) + "Micro/" + imageName;
                    if (File.Exists(iconFile))
                        File.Delete(iconFile);
                    if (File.Exists(bannerFile))
                        File.Delete(bannerFile);
                    if (File.Exists(largeFile))
                        File.Delete(largeFile);
                    if (File.Exists(medium))
                        File.Delete(medium);
                    if (File.Exists(micro))
                        File.Delete(micro);
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            FillCategoryList();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            //CategoryComponent.CategoryID = 0;
            CategoryComponent.Filter = "";
            //CategoryComponent.NewFilter = false;
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlSearch.SelectedIndex = 0;
            gvCategory.PageIndex = 0;
            FillCategoryList();
        }

        /// <summary>
        /// Status Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCategoryList();
        }

        /// <summary>
        ///  Category Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategory.PageIndex = e.NewPageIndex;
            FillCategoryList();
        }
    }
}