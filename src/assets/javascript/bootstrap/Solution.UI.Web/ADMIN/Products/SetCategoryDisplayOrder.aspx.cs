using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class SetCategoryDisplayOrder : BasePage
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
                    btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                    DataSet dscategory = new DataSet();
                    dscategory = CommonComponent.GetCommonDataSet("SELECT tb_Category.CategoryID,Name FROm tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID WHERE isnull(tb_CategoryMapping.ParentCategoryID,0)=0 and Storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and tb_CategoryMapping.CategoryID in (SELECT ParentCategoryID  FROM tb_CategoryMapping) ORder By tb_Category.DisplayOrder ASC");
                    if (dscategory != null && dscategory.Tables.Count > 0 && dscategory.Tables[0].Rows.Count > 0)
                    {
                        ddlCategory.DataSource = dscategory;
                        ddlCategory.DataTextField = "Name";
                        ddlCategory.DataValueField = "CategoryID";
                    }
                    else
                    {
                        ddlCategory.DataSource = null;
                    }
                    ddlCategory.DataBind();
                    //ddlCategory.Items.Insert(0, new ListItem("All Products", "0"));
                    ddlCategory.SelectedIndex = 0;

                    // FillCategoryList();


                }
            }
            catch (Exception ex) { CommonComponent.ErrorLog("Category2.aspx - Admin", ex.Message, ex.StackTrace); }
        }


        /// <summary>
        /// Method for Bind Store with dropdown ddlStore
        /// </summary>


        /// <summary>
        /// Fill Category List
        /// </summary>
        public void FillCategoryList()
        {
            DataSet dsCategoryList = new DataSet();
            //  dsCategoryList = CategoryComponent.GetAllCategoriesWithsearch(1, "", "", "Active");
            if (ddlcateggosystatus.SelectedValue.ToString() == "")
            {
                dsCategoryList = CommonComponent.GetCommonDataSet("select distinct categoryid,name,0 as ParentCategoryID,isnull(DisplayOrder,0) as DisplayOrder,isnull(ChildCatCount,0) as ChildCatCount,isnull(Active,0) as Active from tb_Category where   isnull(Deleted,0)=0 and StoreID=1 and (  CategoryID in (select CategoryID from tb_CategoryMapping where isnull(ParentCategoryID,0)<>0 and isnull(ParentCategoryID,0)=" + ddlCategory.SelectedValue.ToString() + "))  and isnull(ChildCatCount,0)>=0 order by DisplayOrder");
            }
            else
            {
                dsCategoryList = CommonComponent.GetCommonDataSet("select distinct categoryid,name,0 as ParentCategoryID,isnull(DisplayOrder,0) as DisplayOrder,isnull(ChildCatCount,0) as ChildCatCount,isnull(Active,0) as Active from tb_Category where isnull(active,0)=" + ddlcateggosystatus.SelectedValue.ToString() + " and isnull(Deleted,0)=0 and StoreID=1 and (  CategoryID in (select CategoryID from tb_CategoryMapping where isnull(ParentCategoryID,0)<>0 and isnull(ParentCategoryID,0)=" + ddlCategory.SelectedValue.ToString() + "))  and isnull(ChildCatCount,0)>=0 order by DisplayOrder");
            }


            if (dsCategoryList != null && dsCategoryList.Tables.Count > 0 && dsCategoryList.Tables[0].Rows.Count > 0)
            {

                DataRow[] drr = dsCategoryList.Tables[0].Select("ParentCategoryID=0 OR ParentCategoryID <> 0");
                DataTable dt = new DataTable();
                dt = dsCategoryList.Tables[0].Clone();

                foreach (DataRow dr in drr)
                {
                    dt.Rows.Add(dr.ItemArray);
                }
                dt.AcceptChanges();
                grdParentCategory.DataSource = dt;
                grdParentCategory.DataBind();
            }
            else
            {
                grdParentCategory.DataSource = null;
                grdParentCategory.DataBind();
            }
        }

        /// <summary>
        /// Sort Grid view in Asc or desc order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sorting(object sender, EventArgs e)
        {
            //    ImageButton lb = (ImageButton)sender;
            //    if (lb != null)
            //    {
            //        if (lb.CommandArgument == "ASC")
            //        {
            //            //DataTable dtSortTable = gvCategory.DataSource as DataTable;

            //            //if (dtSortTable != null)
            //            //{
            //            //    DataView dvSortedView = new DataView(dtSortTable);

            //            //    dvSortedView.Sort = lb.CommandName.ToString() + " " + SortDirection.Ascending;
            //            //    dvSortedView.ToTable();
            //            //    gvCategory.DataSource = dvSortedView;
            //            //    gvCategory.DataBind();
            //            //}
            //            gvCategory.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
            //            lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
            //            if (lb.ID == "lbName")
            //            {
            //                isDescendName = false;
            //            }


            //            lb.AlternateText = "Descending Order";
            //            lb.ToolTip = "Descending Order";
            //            lb.CommandArgument = "DESC";
            //        }
            //        else if (lb.CommandArgument == "DESC")
            //        {

            //            gvCategory.Sort(lb.CommandName.ToString(), SortDirection.Descending);
            //            //DataTable dtSortTable = gvCategory.DataSource as DataTable;

            //            //if (dtSortTable != null)
            //            //{
            //            //    DataView dvSortedView = new DataView(dtSortTable);

            //            //    dvSortedView.Sort = lb.CommandName.ToString() + " " + SortDirection.Descending;
            //            //    dvSortedView.ToTable();
            //            //    gvCategory.DataSource = dvSortedView;
            //            //    gvCategory.DataBind();
            //            //}
            //            lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
            //            if (lb.ID == "lbName")
            //            {
            //                isDescendName = true;
            //            }

            //            lb.AlternateText = "Ascending Order";
            //            lb.ToolTip = "Ascending Order";
            //            lb.CommandArgument = "ASC";
            //        }
            //    }
        }

        protected void grdParentCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lttitle = (Literal)e.Row.FindControl("lttitle");
                GridView gvCategory = (GridView)e.Row.FindControl("gvCategory");
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");
                Literal ltrrepeater = (Literal)e.Row.FindControl("ltrrepeater");
                //((ImageButton)e.Row.FindControl("btnSave")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.png";
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
                //   dsChildCategoryList = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(1), "ParentCatName", lttitle.Text, "Active");

                dsChildCategoryList = CommonComponent.GetCommonDataSet("SELECT name,isnull(tb_Category.active,0) as active,tb_Category.DisplayOrder,tb_Category.CategoryID,ParentCategoryID,Sename,ImageName FROM tb_Category INNER JOIN tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID WHERE tb_CategoryMapping.ParentCategoryID=" + CategoryID + " and isnull(active,0)=1 and name not like '%63inch%' and isnull(deleted,0)=0 and Storeid=1 order by tb_Category.DisplayOrder");

                //if (((DataRowView)e.Row.DataItem)["ParentCategoryID"].ToString() == "0") e.Row.Visible = false;
                //if (e.Row.RowIndex == 0 && !IsPostBack)
                //{
                //    hdnrowid.Value = "divchild" + CategoryID.ToString();
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapse('divchild" + CategoryID.ToString() + "', 'one');", true);
                //}
                //else
                //{
                //    //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapse('divchild" + CategoryID.ToString() + "',"+hdnrowid.Value.ToString()+");", true);
                //}

                if (dsChildCategoryList != null && dsChildCategoryList.Tables.Count > 0 && dsChildCategoryList.Tables[0].Rows.Count > 0)
                {
                    ViewState["gvCategory"] = dsChildCategoryList.Tables[0].Rows.Count.ToString();
                    gvCategory.DataSource = dsChildCategoryList;
                    gvCategory.DataBind();
                    ltrrepeater.Text = "<ul class=\"rep-drag\">";
                    for (int i = 0; i < dsChildCategoryList.Tables[0].Rows.Count; i++)
                    {
                        ltrrepeater.Text += "<li style=\"width: 16.5%;margin: 0px auto;list-style-type: none;float: left; margin-right: 10px;\"><div class=\"col-sm-4 fp-display\">";
                        ltrrepeater.Text += " <div class=\"fp-box-div img-center free-swatch-hover\">";
                        ltrrepeater.Text += "<center><img height=\"310\" Style=\"border:1px solid #ccc;text-align:center;\" src = '" + GetIconImageProduct(dsChildCategoryList.Tables[0].Rows[i]["ImageName"].ToString()) + "' ID=\"imgName22\" ToolTip='" + dsChildCategoryList.Tables[0].Rows[0]["Name"].ToString() + "' runat=\"server\" /></center>";
                        ltrrepeater.Text += "<div class=\"btn-box-bg\"></div>";
                        ltrrepeater.Text += "</div>";
                        ltrrepeater.Text += "<div class=\"fp-display-title\" style=\"height:40px;\"><h2>" + dsChildCategoryList.Tables[0].Rows[i]["Name"].ToString() + "</h2></div>";
                        ltrrepeater.Text += "<p class=\"fp-box-p\"><span style=\"font-size:13px;display:none;\">Dispaly Order : </span><input id=\"txtdisplayorder\" type=\"text\" style=\"width:20px;margin-top: 10px;display:none;height:14px;text-align: center;\" value=" + dsChildCategoryList.Tables[0].Rows[i]["DisplayOrder"].ToString() + "></input>";
                        ltrrepeater.Text += "<input id=\"hdnCatid\" type=\"hidden\" value=" + dsChildCategoryList.Tables[0].Rows[i]["CategoryID"].ToString() + " ></input>";
                        ltrrepeater.Text += "<input id=\"hdnParentCatID\" type=\"hidden\" value=" + dsChildCategoryList.Tables[0].Rows[i]["ParentCategoryID"].ToString() + " ></input></p>";
                        ltrrepeater.Text += "</li>";
                    }
                    ltrrepeater.Text += "</ul>";
                }
                else
                {
                    ViewState["gvCategory"] = "0";
                    gvCategory.DataSource = null;
                    gvCategory.DataBind();

                }

            }

        }
        public string GetIconImageProduct(String img)
        {
            string imagepath = string.Empty;

            try
            {
                imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

                if (File.Exists(Server.MapPath(imagepath)))
                {
                    imagepath = AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
                    return imagepath;
                }

                imagepath = string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            catch
            {
            }
            return imagepath;
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

                GridView gvTemp = (GridView)sender;
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);

                System.Web.UI.HtmlControls.HtmlInputHidden hdnCategoryID = (System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdnCategoryID");

                System.Web.UI.HtmlControls.HtmlInputHidden hdncategoryName = (System.Web.UI.HtmlControls.HtmlInputHidden)grrow.FindControl("hdncategoryName");

                Int32 CategoryID = Convert.ToInt32(hdnCategoryID.Value.ToString());
                string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                if (String.IsNullOrEmpty(displayorder))
                {
                    lblMessage.Text = "Please Enter Display Order";
                    return;

                }
                if (displayorder.Trim() == "")
                {
                    displayorder = "0";
                }
                int count = 0;
                string Dorder = ",";
                int emptyDOrdr = 900;
                for (int i = 0; i < grdParentCategory.Rows.Count; i++)
                {
                    GridViewRow row = (GridViewRow)grdParentCategory.Rows[i];
                    TextBox DD = ((TextBox)row.FindControl("txtDisplayOrder"));
                    String Do = DD.Text;
                    if (String.IsNullOrEmpty(Do.ToString()))
                    {
                        emptyDOrdr = emptyDOrdr + 1;


                        Dorder = Dorder + emptyDOrdr + ",";

                    }
                    else if (Dorder.IndexOf("," + Do + ",") > -1)
                    {
                        count = count + 1;
                    }

                    else
                    {
                        Dorder = Dorder + Do + ",";
                    }

                }
                if (count > 0)
                {
                    lblMessage.Text = "Duplicate Display Order";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                    //duplicate DOOrder
                }
                else
                {
                    CommonComponent.ExecuteCommonData("update tb_Category set DisplayOrder=" + displayorder + " where CategoryID=" + CategoryID + "");
                    string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                    if (hdncategoryName.Value.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                    lblMessage.Text = "";

                    try
                    {
                        var myUri = new Uri(srliveserver + "/home/index?publish=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    catch
                    {

                    }
                }
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


                ImageButton btnSave = (ImageButton)e.Row.FindControl("btnSave");
                btnSave.OnClientClick = "return checkCount('ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                System.Web.UI.HtmlControls.HtmlAnchor lkbAllowAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbAllowAll");
                System.Web.UI.HtmlControls.HtmlAnchor lkbClearAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbClearAll");
                lkbClearAll.HRef = "javascript:selectAllGrid(false,'ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                lkbAllowAll.HRef = "javascript:selectAllGrid(true,'ContentPlaceHolder1_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "draggrid", "draggrid();", true);

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
        protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ParentSave")
            {

            }
            else if (e.CommandName == "ChildSingleSave")
            {
                string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                GridView gvTemp = (GridView)sender;
                GridViewRow grrow = (GridViewRow)(((System.Web.UI.Control)e.CommandSource).NamingContainer);
                //Int32 Projectid = Convert.ToInt32(Request.QueryString["id"].ToString());
                HiddenField hdnCatid = (HiddenField)grrow.FindControl("hdnCatid");
                Int32 CategoryID = Convert.ToInt32(hdnCatid.Value.ToString());
                HiddenField hdnParentCatID = (HiddenField)grrow.FindControl("hdnParentCatID");
                HiddenField hdncategoryName = (HiddenField)grrow.FindControl("hdncategoryName");

                Int32 ParentCatID = Convert.ToInt32(hdnParentCatID.Value.ToString());
                string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                if (String.IsNullOrEmpty(displayorder))
                {
                    lblMessage.Text = "Please Enter Display Order";
                    return;

                }

                if (displayorder.Trim() == "")
                {
                    displayorder = "0";
                }
                if (checkSingleChildDuplicate(gvTemp, displayorder))
                {
                    lblMessage.Text = "";
                    CommonComponent.ExecuteCommonData("update tb_Category set DisplayOrder=" + displayorder + " where CategoryID=" + CategoryID + "");

                    if (hdncategoryName.Value.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    try
                    {
                        var myUri = new Uri(srliveserver + "/home/index?publish=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    catch
                    {

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);

                }
                else
                {
                    lblMessage.Text = "Duplicate Display Order";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
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
                        string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                        lblMessage.Text = "";
                        GridViewRow grrow = (GridViewRow)gvTemp.Rows[i];
                        CheckBox chkSelect = (CheckBox)gvTemp.Rows[i].FindControl("chkSelect");
                        HiddenField hdnCatid = (HiddenField)grrow.FindControl("hdnCatid");
                        Int32 CategoryID = Convert.ToInt32(hdnCatid.Value.ToString());
                        HiddenField hdnParentCatID = (HiddenField)grrow.FindControl("hdnParentCatID");
                        string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                        HiddenField hdncategoryName = (HiddenField)grrow.FindControl("hdncategoryName");
                        if (displayorder.Trim() == "")
                        {
                            displayorder = "0";
                        }
                        if (chkSelect.Checked)
                        {
                            CommonComponent.ExecuteCommonData("update tb_Category set DisplayOrder=" + displayorder + " where CategoryID=" + CategoryID + "");
                            if (hdncategoryName.Value.ToString().ToLower().IndexOf(" fabric") > -1)
                            {
                                var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                                // Create a 'HttpWebRequest' object for the specified url. 
                                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                                // Set the user agent as if we were a web browser
                                myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                var stream = myHttpWebResponse.GetResponseStream();

                                myHttpWebResponse.Close();
                            }
                            else
                            {
                                var myUri = new Uri(srliveserver + "/category/category?catid=" + CategoryID.ToString().Trim() + "&catchupdate=1");
                                // Create a 'HttpWebRequest' object for the specified url. 
                                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                                // Set the user agent as if we were a web browser
                                myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                var stream = myHttpWebResponse.GetResponseStream();

                                myHttpWebResponse.Close();
                            }
                            try
                            {
                                var myUri = new Uri(srliveserver + "/home/index?publish=1");
                                // Create a 'HttpWebRequest' object for the specified url. 
                                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                                // Set the user agent as if we were a web browser
                                myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                var stream = myHttpWebResponse.GetResponseStream();

                                myHttpWebResponse.Close();
                            }
                            catch
                            {

                            }
                        }

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }
                else
                {
                    lblMessage.Text = "Duplicate Display Order";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }

                //
            }

        }


        /// <summary>
        /// Method for get data by entered keyword
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, EventArgs e)
        {


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


        /// <summary>
        /// Grid Page Index Changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvCategory.PageIndex = e.NewPageIndex;
            //FillCategoryList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillCategoryList();
        }
    }
}