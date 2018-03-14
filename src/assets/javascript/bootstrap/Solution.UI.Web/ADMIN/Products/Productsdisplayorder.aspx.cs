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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Productsdisplayorder : BasePage
    {
        #region Declaration
        #region component
        CategoryComponent objCatComponent = new CategoryComponent();

        StoreComponent objStorecomponent = new StoreComponent();
        public static bool isDescendName = false;
        #endregion
        #endregion
        Int32 RowIndex = 0;
        private string StrFileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                {
                    return "";
                }
                else
                {
                    return (ViewState["FileName"].ToString());
                }
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                lblMessage.Text = "";

                if (!IsPostBack)
                {

                    FillCategoryList();

                    // FillCategoryList();
                }
            }
            catch (Exception ex) { CommonComponent.ErrorLog("Category2.aspx - Admin", ex.Message, ex.StackTrace); }
        }



        public void FillCategoryList()
        {
            DataSet dsCategoryList = new DataSet();
            //    dsCategoryList = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(ddlStore.SelectedValue), "", "", "Active");

          //  dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder from tb_category where StoreID=" + Convert.ToInt32(1) + " and isnull(Active,0)=1 and isnull(Deleted,0)=0");
            dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,isnull(DisplayOrder,0) as DisplayOrder from tb_category where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and CategoryID in (select distinct categoryid from tb_Productcategory where productid in (select productid from tb_product where isnull(deleted,0)=0 and isnull(active,0)=1))");


            if (dsCategoryList != null && dsCategoryList.Tables.Count > 0 && dsCategoryList.Tables[0].Rows.Count > 0)
            {



                grdParentCategory.DataSource = dsCategoryList.Tables[0];
                grdParentCategory.DataBind();
            }
            else
            {
                grdParentCategory.DataSource = null;
                grdParentCategory.DataBind();
            }
        }



        protected void grdParentCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lttitle = (Literal)e.Row.FindControl("lttitle");
                GridView gvCategory = (GridView)e.Row.FindControl("gvCategory");
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
                //((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                //((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

                if (hdnActive.Value != "")
                {
                    if (hdnActive.Value.ToString().ToLower() == "true")
                    {
                        ltrStatus.Text = "<span class=\"label label-success\">Active</span>";
                    }
                    else
                    {
                        ltrStatus.Text = "<span class=\"label label-warning\">In-Active</span>";
                    }
                }
                else
                {
                    ltrStatus.Text = "<span class=\"label label-warning\">In-Active</span>";
                }
                RowIndex = e.Row.RowIndex;
                ViewState["gvCategory"] = "0";
                Int32 CategoryID = Convert.ToInt32(grdParentCategory.DataKeys[e.Row.RowIndex].Value.ToString());

                DataSet dsChildCategoryList = new DataSet();

                //if (ddlStore.SelectedValue == "-1")
                //{
                //    dsChildCategoryList = CommonComponent.GetCommonDataSet("select tb_Product.ProductID,tb_Product.Name,tb_Product.DisplayOrder,isnull(Active,0) as active,tb_ProductCategory.CategoryID from tb_Product inner join tb_ProductCategory on tb_Product.ProductID=tb_ProductCategory.ProductID where CategoryID=" + CategoryID + " and isnull(tb_Product.Active,0)=1 and isnull(tb_Product.Deleted,0)=0");
                //}
                //else
                //{
                if (lttitle.Text.ToString().ToLower().IndexOf("sales outlet") > -1)
                {
                    dsChildCategoryList = CommonComponent.GetCommonDataSet("select p.ProductID, p.Name,p.DisplayOrder,isnull(p.Active,0) as active," + CategoryID + " as CategoryID from tb_Product p where p.StoreID=1 and isnull(Active,0) = 1 and isnull(Deleted,0) = 0  and p.productid in (Select tb_product.Productid from tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId= tb_ProductVariantValue.productId Where isnull(tb_product.Active,0)=1 AND  isnull(tb_product.deleted,0)=0 AND isnull(tb_product.storeid,0)=1 AND (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1) UNION select tb_Product.ProductID,tb_Product.Name,tb_Product.DisplayOrder,isnull(Active,0) as active,tb_ProductCategory.CategoryID from tb_Product inner join tb_ProductCategory on tb_Product.ProductID=tb_ProductCategory.ProductID where CategoryID=" + CategoryID + " and tb_product.storeid=1 and isnull(tb_Product.Active,0)=1 and isnull(tb_Product.Deleted,0)=0");
                }
                else
                {
                    dsChildCategoryList = CommonComponent.GetCommonDataSet("select tb_Product.ProductID,tb_Product.Name,tb_Product.DisplayOrder,isnull(Active,0) as active,tb_ProductCategory.CategoryID from tb_Product inner join tb_ProductCategory on tb_Product.ProductID=tb_ProductCategory.ProductID where CategoryID=" + CategoryID + " and tb_product.storeid=1 and isnull(tb_Product.Active,0)=1 and isnull(tb_Product.Deleted,0)=0");
                }
                //}
                // dsChildCategoryList = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(ddlStore.SelectedValue), "ParentCatName", lttitle.Text, "Active");
                //if (((DataRowView)e.Row.DataItem)["ParentCategoryID"].ToString() == "0") e.Row.Visible = false;
                if (e.Row.RowIndex == 0 && !IsPostBack)
                {
                    hdnrowid.Value = "divchild" + CategoryID.ToString();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapse('divchild" + CategoryID.ToString() + "', 'one');", true);
                }
                else
                {
                    //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapse('divchild" + CategoryID.ToString() + "',"+hdnrowid.Value.ToString()+");", true);
                }

                if (dsChildCategoryList != null && dsChildCategoryList.Tables.Count > 0 && dsChildCategoryList.Tables[0].Rows.Count > 0)
                {
                    ViewState["gvCategory"] = dsChildCategoryList.Tables[0].Rows.Count.ToString();
                    gvCategory.DataSource = dsChildCategoryList;
                    gvCategory.DataBind();
                }
                else
                {
                    ViewState["gvCategory"] = "0";
                    gvCategory.DataSource = null;
                    gvCategory.DataBind();

                }

            }

        }


        private bool checkSingleChildDuplicate(GridView gvtemp, String OriDO)
        {
            int count = 0;
            String Dorder = ",";
            int emptyDOrdr = 900;
            for (int i = 0; i < gvtemp.Rows.Count; i++)
            {
                GridViewRow row = (GridViewRow)gvtemp.Rows[i];
                TextBox DisplayOrder = ((TextBox)row.FindControl("txtDisplayOrder"));
                string Do = DisplayOrder.Text;

                if (String.IsNullOrEmpty(Do.ToString()))
                {
                    emptyDOrdr = emptyDOrdr + 1;
                    DisplayOrder.Text = emptyDOrdr.ToString();

                    Dorder = Dorder + emptyDOrdr + ",";

                }
                else if (Do == OriDO)
                {
                    count++;

                    if (count > 1)
                    {
                        return false;
                    }
                }

                else
                {
                    Dorder = Dorder + Do + ",";
                }

            }
            return true;
        }

        private bool checkChildDuplicate(GridView gvtemp)
        {
            String Dorder = ",";
            int emptyDOrdr = 900;
            for (int i = 0; i < gvtemp.Rows.Count; i++)
            {
                GridViewRow row = (GridViewRow)gvtemp.Rows[i];
                TextBox DisplayOrder = ((TextBox)row.FindControl("txtDisplayOrder"));
                string Do = DisplayOrder.Text;
                if (String.IsNullOrEmpty(Do.ToString()))
                {
                    emptyDOrdr = emptyDOrdr + 1;
                    DisplayOrder.Text = emptyDOrdr.ToString();

                    Dorder = Dorder + emptyDOrdr + ",";

                }
                else if (Dorder.IndexOf("," + Do + ",") > -1)
                {
                    return false;
                }

                else
                {
                    Dorder = Dorder + Do + ",";
                }

            }
            return true;

        }

        protected void grdParentCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ParentSave")
            {

                //GridView gvTemp = (GridView)sender;
                //GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                //System.Web.UI.HtmlControls.HtmlInputHidden hdnCategoryID = (System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnCategoryID");
                //Int32 CategoryID = Convert.ToInt32(hdnCategoryID.Value.ToString());
                //string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                //if (String.IsNullOrEmpty(displayorder))
                //{
                //    lblMessage.Text = "Please Enter DisplayOrder";
                //    return;

                //}
                //if (displayorder.Trim() == "")
                //{
                //    displayorder = "0";
                //}
                //int count = 0;
                //string Dorder = ",";
                //int emptyDOrdr = 900;
                //for (int i = 0; i < grdParentCategory.Rows.Count; i++)
                //{
                //    GridViewRow row = (GridViewRow)grdParentCategory.Rows[i];
                //    TextBox DD = ((TextBox)row.FindControl("txtDisplayOrder"));
                //    String Do = DD.Text;
                //    if (String.IsNullOrEmpty(Do.ToString()))
                //    {
                //        emptyDOrdr = emptyDOrdr + 1;


                //        Dorder = Dorder + emptyDOrdr + ",";

                //    }
                //    else if (Dorder.IndexOf("," + Do + ",") > -1)
                //    {
                //        count = count + 1;
                //    }

                //    else
                //    {
                //        Dorder = Dorder + Do + ",";
                //    }

                //}
                //if (count > 0)
                //{
                //    lblMessage.Text = "Duplicate DisplayOrder";
                //    lblMessage.Style.Add("color", "#FF0000");
                //    lblMessage.Style.Add("font-weight", "normal");
                //    //duplicate DOOrder
                //}
                //else
                //{
                //    CommonComponent.ExecuteCommonData("update tb_Category set DisplayOrder=" + displayorder + " where CategoryID=" + CategoryID + "");
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                //    lblMessage.Text = "";
                //}
            }
            else if (e.CommandName == "ChildSingleSave")
            {


            }
            else if (e.CommandName == "ChildAllSave")
            {


            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //LinkButton btnSave = (LinkButton)e.Row.FindControl("btnSave");
                //btnSave.OnClientClick = "return checkCount('ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                System.Web.UI.HtmlControls.HtmlAnchor lkbAllowAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbAllowAll");
                System.Web.UI.HtmlControls.HtmlAnchor lkbClearAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbClearAll");
                lkbClearAll.HRef = "javascript:selectAllGrid(false,'ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                lkbAllowAll.HRef = "javascript:selectAllGrid(true,'ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                ((ImageButton)e.Row.FindControl("btnSave")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                ((ImageButton)e.Row.FindControl("btnSave")).OnClientClick = "return checkCount('ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
                //((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                //((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                ((ImageButton)e.Row.FindControl("btnSingleSave")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.png";

                if (hdnActive.Value != "")
                {
                    if (hdnActive.Value.ToString().ToLower() == "true")
                    {
                        ltrStatus.Text = "<span class=\"label label-success\">Active</span>";
                    }
                    else
                    {
                        ltrStatus.Text = "<span class=\"label label-warning\">In-Active</span>";
                    }
                }
                else
                {
                    ltrStatus.Text = "<span class=\"label label-warning\">In-Active</span>";
                }


            }
            //if (gvCategory.Rows.Count > 0)
            //    trBottom.Visible = true;
            //else
            //    trBottom.Visible = false;

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    if (isDescendName == false)
            //    {
            //        ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
            //        lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
            //        lbName.AlternateText = "Ascending Order";
            //        lbName.ToolTip = "Ascending Order";
            //        lbName.CommandArgument = "DESC";
            //    }
            //    else
            //    {
            //        ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
            //        lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
            //        lbName.AlternateText = "Descending Order";
            //        lbName.ToolTip = "Descending Order";
            //        lbName.CommandArgument = "ASC";
            //    }
            //}

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCatid");

                Int32 CategoryID = 0, CategoryCount = 0, ProductCount = 0;
                Int32.TryParse(Convert.ToString(hdnCatid.Value), out CategoryID);


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

        protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ParentSave")
            {

            }
            else if (e.CommandName == "ChildSingleSave")
            {
                GridView gvTemp = (GridView)sender;
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                //Int32 Projectid = Convert.ToInt32(Request.QueryString["id"].ToString());
                HiddenField hdnCatid = (HiddenField)grrow.FindControl("hdnCatid");
                Int32 CategoryID = Convert.ToInt32(hdnCatid.Value.ToString());
                HiddenField hdnProductID = (HiddenField)grrow.FindControl("hdnProductID");
                Int32 ProductID = Convert.ToInt32(hdnProductID.Value.ToString());
                //Int32 ParentCatID = Convert.ToInt32(hdnParentCatID.Value.ToString());
                string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                if (String.IsNullOrEmpty(displayorder))
                {
                    lblMessage.Text = "Please Enter DisplayOrder";
                    return;

                }

                if (displayorder.Trim() == "")
                {
                    displayorder = "0";
                }
                if (checkSingleChildDuplicate(gvTemp, displayorder))
                {
                    lblMessage.Text = "";
                    CommonComponent.ExecuteCommonData("update tb_Product set DisplayOrder=" + displayorder + " where ProductID=" + ProductID + "");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }
                else
                {

                    lblMessage.Text = "Duplicate DisplayOrder";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid2", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }




            }
            else if (e.CommandName == "ChildAllSave")
            {
                GridView gvTemp = (GridView)sender;

                //Int32 Projectid = Convert.ToInt32(Request.QueryString["id"].ToString());


                // Int32 ParentCatID = Convert.ToInt32(hdnParentCatID.Value.ToString());




                if (checkChildDuplicate(gvTemp))
                {
                    for (int i = 0; i < gvTemp.Rows.Count; i++)
                    {

                        lblMessage.Text = "";
                        GridViewRow grrow = (GridViewRow)gvTemp.Rows[i];
                        CheckBox chkSelect = (CheckBox)gvTemp.Rows[i].FindControl("chkSelect");
                        HiddenField hdnCatid = (HiddenField)grrow.FindControl("hdnCatid");
                        Int32 CategoryID = Convert.ToInt32(hdnCatid.Value.ToString());
                        HiddenField hdnProductID = (HiddenField)grrow.FindControl("hdnProductID");
                        Int32 ProductID = Convert.ToInt32(hdnProductID.Value.ToString());
                        //HiddenField hdnParentCatID = (HiddenField)grrow.FindControl("hdnParentCatID");
                        string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                        if (displayorder.Trim() == "")
                        {
                            displayorder = "0";
                        }
                        if (chkSelect.Checked)
                        {
                            CommonComponent.ExecuteCommonData("update tb_product set DisplayOrder=" + displayorder + " where ProductID=" + ProductID + "");
                        }

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }
                else
                {

                    lblMessage.Text = "Duplicate DisplayOrder";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid2", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }

                //
            }

        }


        /// <summary>
        /// Method for set Image is active or not
        /// </summary>
        /// <param name="_Active">_Active</param>
        /// <returns></returns>
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
        /// Method for get data by entered keyword
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
        /// Method for get data by entered keyword
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            FillCategoryList();
        }


        protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvCategory.PageIndex = e.NewPageIndex;
            //FillCategoryList();
        }



        private bool CheckDisplayorder(DataTable dt)
        {
            int emptyDOrdr = 900;
            int DO = -1;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drr = dt.Select("ParentCategoryID=0");
                String Dorder = ",";
                if (drr.Length > 0)
                {
                    foreach (DataRow dr in drr)
                    {
                        if (String.IsNullOrEmpty(dr["DisplayOrder"].ToString()))
                        {
                            emptyDOrdr = emptyDOrdr + 1;
                            dr["DisplayOrder"] = emptyDOrdr;
                            dt.AcceptChanges();
                            Dorder = Dorder + dr["DisplayOrder"].ToString() + ",";

                        }
                        else if (Dorder.IndexOf("," + dr["DisplayOrder"].ToString() + ",") > -1)
                        {
                            return false;
                        }

                        else
                        {
                            Dorder = Dorder + dr["DisplayOrder"].ToString() + ",";
                        }
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int categoryID = Convert.ToInt32(dt.Rows[i]["CategoryID"].ToString());
                    Dorder = ",";
                    emptyDOrdr = 900;
                    drr = dt.Select("ParentCategoryID=" + categoryID);
                    if (drr.Length > 0)
                    {
                        foreach (DataRow dr in drr)
                        {
                            if (String.IsNullOrEmpty(dr["DisplayOrder"].ToString()))
                            {
                                emptyDOrdr = emptyDOrdr + 1;
                                dr["DisplayOrder"] = emptyDOrdr;
                                dt.AcceptChanges();
                                Dorder = Dorder + dr["DisplayOrder"].ToString() + ",";

                            }
                            else if (Dorder.IndexOf("," + dr["DisplayOrder"].ToString() + ",") > -1)
                            {
                                return false;
                            }

                            else
                            {
                                Dorder = Dorder + dr["DisplayOrder"].ToString() + ",";
                            }
                        }
                    }

                }
            }

            return true;

        }

        private bool InsertDataInDataBase(DataTable dt)
        {
            Boolean result = CheckDisplayorder(dt);
            if (result)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        CommonComponent.ExecuteCommonData("update tb_Category set DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + " where CategoryID=" + dt.Rows[i]["CategoryID"].ToString() + "");
                    }
                }
                else
                {
                    return false;

                    StrFileName = "";
                }
            }
            else
            {
                lblMessage.Text = "Duplicate Display Order";
                lblMessage.Style.Add("color", "#FF0000");
                lblMessage.Style.Add("font-weight", "normal");
                StrFileName = "";
                return false;
            }
            FillCategoryList();
            return true;
        }


    }
}