using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class SetProductDisplayOrder : BasePage
    {
        #region Declaration

        public static bool isDescendName = false;

        #region component

        CategoryComponent objCatComponent = new CategoryComponent();
        StoreComponent objStorecomponent = new StoreComponent();
        ProductComponent productcomp = new ProductComponent();

        #endregion

        #endregion

        Int32 RowIndex = 0;
        Int32 parentrowid = 0;
        bool allfinal = false;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                lnkbtnsearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnsave.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme + "/images/save.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btncancel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme + "/images/cancel.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
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

                }
                if (!IsPostBack)
                {
                    CategoryComponent.Filter = "";
                    CategoryComponent.NewFilter = false;
                    // bindstore();
                    //bindcategory();
                    //FillCategoryList();
                    FillRowCategoryList();
                    AppConfig.StoreID = Convert.ToInt32(1);
                }
            }
            catch (Exception ex) { CommonComponent.ErrorLog("Category2.aspx - Admin", ex.Message, ex.StackTrace); }
        }


        public void FillRowCategoryList()
        {

            txtcategoryinsert.Text = "";
            DataSet dsCategoryList = new DataSet();
            //if (ddlStore.SelectedValue == "-1")
            //{
            //    //mode=1
            //    //dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder from tb_category where  isnull(Active,0)=1 and isnull(Deleted,0)=0");
            //    dsCategoryList = productcomp.GetCategoryDetails(0, 0, 5);
            //}
            //else
            //{
            //mode=1
            //dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder from tb_category where StoreID=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + " and isnull(Active,0)=1 and isnull(Deleted,0)=0");
            //dsCategoryList = productcomp.GetCategoryDetails(0, Convert.ToInt32(1), 5);
            // dsCategoryList = CommonComponent.GetCommonDataSet("Exec GuiGetProductDisplayOrder 0,1,5");
            dsCategoryList = CommonComponent.GetCommonDataSet("Exec GuiGetProductDisplayOrder 1,0, " + ddlCategory.SelectedValue.ToString() + "," + ddlcateggosystatus.SelectedValue.ToString() + ",5");
            //}

            if (dsCategoryList != null && dsCategoryList.Tables.Count > 0 && dsCategoryList.Tables[0].Rows.Count > 0)
            {

                grdRowParentCategory.DataSource = dsCategoryList.Tables[0];
                grdRowParentCategory.DataBind();
            }
            else
            {
                grdRowParentCategory.DataSource = null;
                grdRowParentCategory.DataBind();
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
                HiddenField hdnPcategory = (HiddenField)e.Row.FindControl("hdnPcategory");
                Literal ltrrepeater = (Literal)e.Row.FindControl("ltrrepeater");
                Int32 CategoryID = Convert.ToInt32(hdnPcategory.Value.ToString());
                System.Web.UI.HtmlControls.HtmlContainerControl divproductlist = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("divproductlist");
                divproductlist.Attributes.Add("onclick", "javascript:expandcollapse('divchild" + CategoryID.ToString() + "','" + parentrowid.ToString() + "');");
                if (hdnActive.Value != "")
                {
                    if (hdnActive.Value.ToString().ToLower() == "true")
                    {
                        ltrStatus.Text = "<div style=\"color:#468847;font-size:14px;font-weight:bold;text-transform:uppercase;\">Active</div>";
                    }
                    else
                    {
                        ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                    }
                }
                else
                {
                    ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                }
                RowIndex = e.Row.RowIndex;
                ViewState["gvCategory"] = "0";

                // Int32 CategoryID = Convert.ToInt32(grdParentCategory.DataKeys[e.Row.RowIndex].Value.ToString());

                DataSet dsChildCategoryList = new DataSet();

                //if (ddlStore.SelectedValue == "-1")
                //{
                //    //mode=2
                //    //dsChildCategoryList = CommonComponent.GetCommonDataSet("select tb_Product.ProductID,tb_Product.Name,tb_ProductCategory.DisplayOrder,isnull(Active,0) as active,tb_ProductCategory.CategoryID from tb_Product inner join tb_ProductCategory on tb_Product.ProductID=tb_ProductCategory.ProductID where CategoryID=" + CategoryID + " and isnull(tb_Product.Active,0)=1 and isnull(tb_Product.Deleted,0)=0 order by tb_ProductCategory.DisplayOrder");
                //    dsChildCategoryList = productcomp.GetCategoryDetails(Convert.ToInt32(CategoryID), 0, 2);
                //}
                //else
                //{
                //mode=2
                //dsChildCategoryList = CommonComponent.GetCommonDataSet("select tb_Product.ProductID,tb_Product.Name,tb_ProductCategory.DisplayOrder,isnull(Active,0) as active,tb_ProductCategory.CategoryID from tb_Product inner join tb_ProductCategory on tb_Product.ProductID=tb_ProductCategory.ProductID where CategoryID=" + CategoryID + " and tb_product.storeid=" + ddlStore.SelectedValue + " and isnull(tb_Product.Active,0)=1 and isnull(tb_Product.Deleted,0)=0 order by tb_ProductCategory.DisplayOrder");
                // dsChildCategoryList = productcomp.GetCategoryDetails(Convert.ToInt32(CategoryID), Convert.ToInt32(1), 2);


                if (lttitle.Text.ToString().ToLower().IndexOf("final sale") > -1 && allfinal == false)
                {
                    CommonComponent.ExecuteCommonData("EXEC usp_FinalSaelCategoryProduct");
                    allfinal = true;
                }

                dsChildCategoryList = CommonComponent.GetCommonDataSet("Exec GuiGetProductDisplayOrder 1,0, " + CategoryID + ",0,2");
                //}
                if (e.Row.RowIndex == 0 && !IsPostBack)
                {
                    hdnrowid.Value = "divchild" + CategoryID.ToString();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapse('divchild" + CategoryID.ToString() + "', 'one');", true);
                }
                else
                {
                }
                if (dsChildCategoryList != null && dsChildCategoryList.Tables.Count > 0 && dsChildCategoryList.Tables[0].Rows.Count > 0)
                {
                    ViewState["gvCategory"] = dsChildCategoryList.Tables[0].Rows.Count.ToString();
                    gvCategory.DataSource = dsChildCategoryList;
                    gvCategory.DataBind();
                    ltrrepeater.Text = "<ul class=\"rep-drag\">";
                    for (int i = 0; i < dsChildCategoryList.Tables[0].Rows.Count; i++)
                    {
                        string tag = "";
                        if (!string.IsNullOrEmpty(dsChildCategoryList.Tables[0].Rows[i]["ImageName"].ToString()))
                        {
                            string ImgName = GetIconImageProduct(dsChildCategoryList.Tables[0].Rows[i]["ImageName"].ToString());

                            string tagname = dsChildCategoryList.Tables[0].Rows[i]["TagName"].ToString();

                            if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                            {

                                if (tagname != null && !string.IsNullOrEmpty(tagname.ToString().Trim()) && tagname.ToString().ToLower().IndexOf("bestseller") > -1)
                                {

                                    tag = "<img title='Best Seller' src=\"/images/BestSeller_new.png\" alt=\"Best Seller\" class='newarrival' />";

                                }
                                else if (tagname != null && !string.IsNullOrEmpty(tagname.ToString().Trim()) && tagname.ToString().ToLower().IndexOf("newarrival") > -1)
                                {
                                    string Strnew = "select count(ProductID) from tb_Product where StoreID=1 and isnull(active,0)=1 and isnull(Deleted,0)=0  and isnull(IsNewArrival,0)=1 and productid=" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + " and (cast(IsNewArrivalFromDate as date) <=  cast(GETDATE() as date) and cast(IsNewArrivalToDate as date) >=cast(GETDATE() as date))";
                                    Int32 Intnew = Convert.ToInt32(CommonComponent.GetScalarCommonData(Strnew.ToString()));
                                    if (Intnew > 0)
                                    {
                                        tag = "<img title='" + tagname.ToString().Trim() + "' src=\"/images/" + tagname.ToString().Trim() + ".png\" alt=\"" + tagname.ToString().Trim() + "\" class='newarrival' />";
                                    }
                                }
                                else if (!string.IsNullOrEmpty(tagname.ToString().Trim()))
                                {

                                    tag = "<img title='" + tagname.ToString().Trim() + "' src=\"/images/" + tagname.ToString().Trim() + ".png\" alt=\"" + tagname.ToString().Trim() + "\" class='newarrival' />";
                                }
                                //else
                                //{
                                string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue INNER JOIN tb_product on tb_product.ProductId=tb_ProductVariantValue.ProductId Where isnull(VarActive,0)=1 and tb_product.productid=" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 and (isnull(tb_product.Ismadetoready,0)=1 or isnull(tb_product.Ismadetoorder,0)=1)";
                                Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                if (Intcnt > 0)
                                {
                                    tag += "<img  title='Sale' src=\"/images/onsale.png\" alt=\"Sale\" class='newarrival onsaleth' />";


                                }
                                else
                                {
                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue INNER JOIN tb_product on tb_product.ProductId=tb_ProductVariantValue.ProductId Where isnull(VarActive,0)=1 and tb_product.productid=" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + " and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and isnull(IsShowBuy1Get1,0)=1 and  (isnull(Ismadetoorder,0)=1 Or Isnull(tb_ProductVariantValue.Inventory,0) >= 2)";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        tag = "<img title=\"Buy One Get One Free\" src=\"/images/sales-offer-img.png\" alt=\"Buy One Get One Free\" class=\"sales-offer-img\">";
                                    }
                                }


                                int salecount = 0;
                                salecount = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ProductID) from tb_Product where StoreID=1 and isnull(active,0)=1 and isnull(Deleted,0)=0 and ProductID not in (select distinct ProductID from tb_ProductVariantValue ) and isnull(IsSaleclearance,0)=1 and productid=" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + ""));
                                if (salecount > 0)
                                {
                                    tag += "<img  title='Sale' src=\"/images/onsale.png\" alt=\"Sale\" class='newarrival onsaleth' />";

                                }
                                //}

                            }


                        }



                        ltrrepeater.Text += "<li style=\"width: 16.5%;margin: 0px auto;list-style-type: none;float: left; margin-right: 10px;\"><div class=\"col-sm-4 fp-display\">";
                        ltrrepeater.Text += " <div class=\"fp-box-div img-center free-swatch-hover\">";
                        if (!String.IsNullOrEmpty(tag))
                        {
                            ltrrepeater.Text += "<center><img Style=\"border:1px solid #ccc;text-align:center;\" src = '" + GetIconImageProduct(dsChildCategoryList.Tables[0].Rows[i]["ImageName"].ToString()) + "' ID=\"imgName22\" ToolTip='" + dsChildCategoryList.Tables[0].Rows[0]["Name"].ToString() + "' runat=\"server\" /></center>" + tag + "";
                        }
                        else
                        {
                            ltrrepeater.Text += "<center><img Style=\"border:1px solid #ccc;text-align:center;\" src = '" + GetIconImageProduct(dsChildCategoryList.Tables[0].Rows[i]["ImageName"].ToString()) + "' ID=\"imgName22\" ToolTip='" + dsChildCategoryList.Tables[0].Rows[0]["Name"].ToString() + "' runat=\"server\" /> </center>";
                        }

                        ltrrepeater.Text += "<div class=\"btn-box-bg\"></div>";
                        ltrrepeater.Text += "</div>";
                        //if (!String.IsNullOrEmpty(tag))
                        //{
                        //    ltrrepeater.Text += "<div class=\"fp-display-title\" style=\"height:30px;padding-top:5px;\"><h2 style=\"line-height: 16px;\">" + SetName(dsChildCategoryList.Tables[0].Rows[i]["Name"].ToString()) + "</h2></div>";
                        //}
                        //else
                        {
                            ltrrepeater.Text += "<div class=\"fp-display-title\" style=\"height:30px;\"><h2 style=\"line-height: 16px;\">" + SetName(dsChildCategoryList.Tables[0].Rows[i]["Name"].ToString()) + "</h2></div>";
                        }
                        ltrrepeater.Text += "<p class=\"fp-box-p\" style=\"margin-top: 6%;margin-left: 7%;\">";
                        ltrrepeater.Text += "<input id=\"hdnCatid\" type=\"hidden\" value=" + dsChildCategoryList.Tables[0].Rows[i]["CategoryID"].ToString() + " ></input>";
                        ltrrepeater.Text += "<input id=\"hdnProductID\" type=\"hidden\" value=" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + " ></input>";
                        //ltrrepeater.Text += "<br/>";
                        ltrrepeater.Text += "<span style=\"line-height:13px;font-size: 13px;\">Inventory: " + dsChildCategoryList.Tables[0].Rows[i]["Inventory"].ToString() + "</span>";
                        ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Price: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsChildCategoryList.Tables[0].Rows[i]["Price"].ToString())) + "</span>";
                        ltrrepeater.Text += "<br/><span style=\"font-size:12px;float:left;text-decoration:none;display:none;\">Dispaly Order : </span><input id=\"txt-displayorder-" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + "\" type=\"text\" onchange=\"DisplayOrderChange(" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + ")\" style=\"width:55px;font-size:12px;height: 14px;text-align: center;float: left;margin-left: 1%;\" value=\"" + dsChildCategoryList.Tables[0].Rows[i]["DisplayOrder"].ToString() + "\"></input><span id=\"spn-displayorder-" + dsChildCategoryList.Tables[0].Rows[i]["ProductID"].ToString() + "\" style=\"display:none;margin-left: 1%;float: left;margin-bottom: 7%;margin-top: 1%;FONT-SIZE:12px;text-decoration:none;\">" + dsChildCategoryList.Tables[0].Rows[i]["DisplayOrder"].ToString() + "</span>";

                        ltrrepeater.Text += "</p>";
                        ltrrepeater.Text += "</li>";
                    }
                    ltrrepeater.Text += "</ul>";
                }
                else
                {
                    ViewState["gvCategory"] = "0";
                    gvCategory.DataSource = null;
                    gvCategory.DataBind();
                    ltrrepeater.Text = " <span style=\"color: Red; font-size: 12px; text-align: center;\">No Record(s) Found !</span>";
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
                LinkButton btnSave = (LinkButton)e.Row.FindControl("btnSave");
                btnSave.OnClientClick = "return checkCount('ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                System.Web.UI.HtmlControls.HtmlAnchor lkbAllowAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbAllowAll");
                System.Web.UI.HtmlControls.HtmlAnchor lkbClearAll = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbClearAll");
                lkbClearAll.HRef = "javascript:selectAllGrid(false,'ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
                lkbAllowAll.HRef = "javascript:selectAllGrid(true,'ContentPlaceHolder1_grdRowParentCategory_grdParentCategory_gvCategory_" + RowIndex.ToString() + "');";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");

                if (hdnActive.Value != "")
                {
                    if (hdnActive.Value.ToString().ToLower() == "true")
                    {
                        ltrStatus.Text = "<div style=\"color:#468847;font-size:14px;font-weight:bold;text-transform:uppercase;\">Active</div>";
                    }
                    else
                    {
                        ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                    }
                }
                else
                {
                    ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCatid");
                Int32 CategoryID = 0;
                Int32.TryParse(Convert.ToString(hdnCatid.Value), out CategoryID);
            }
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
                HiddenField hdnCatid = (HiddenField)grrow.FindControl("hdnCatid");
                Int32 CategoryID = Convert.ToInt32(hdnCatid.Value.ToString());
                HiddenField hdnProductID = (HiddenField)grrow.FindControl("hdnProductID");
                Int32 ProductID = Convert.ToInt32(hdnProductID.Value.ToString());
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
                    //mode=3
                    //CommonComponent.ExecuteCommonData("update tb_Product set DisplayOrder=" + displayorder + " where ProductID=" + ProductID + "");
                    //productcomp.UpdateDisplayOrder(Convert.ToString(displayorder), Convert.ToInt32(ProductID), 3);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid", "jAlert('Record Updated Successfully'); expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }
                else
                {
                    lblMessage.Text = "Duplicate Display Order";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "showcurrentgrid2", "expandcollapse('" + hdnrowid.Value.ToString() + "', 'one');", true);
                }
            }
            else if (e.CommandName == "ChildAllSave")
            {
                GridView gvTemp = (GridView)sender;
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
                        string displayorder = ((TextBox)grrow.FindControl("txtDisplayOrder")).Text;
                        if (displayorder.Trim() == "")
                        {
                            displayorder = "0";
                        }
                        if (chkSelect.Checked)
                        {
                            //mode=3
                            //CommonComponent.ExecuteCommonData("update tb_product set DisplayOrder=" + displayorder + " where ProductID=" + ProductID + "");
                            // productcomp.UpdateDisplayOrder(Convert.ToString(displayorder), Convert.ToInt32(ProductID), 3);
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
        public void FillCategoryList()
        {
            txtcategoryinsert.Text = "";

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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Sort Grid view in Asc or desc order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sorting(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Method for get data by entered keyword
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkbtnsearch_Click(object sender, EventArgs e)
        {
            DataSet dsCategoryList = new DataSet();
            //            if (ddlStore.SelectedValue == "-1")
            //            {
            //                //mode=1
            //                dsCategoryList = CommonComponent.GetCommonDataSet(@"SELECT tb_category.CategoryID,Name,StoreID,isnull(Active,0) as Active,tb_category.DisplayOrder ,ChildCatCount FROM  tb_category  WHERE  CategoryID in (select ParentCategoryID from tb_category INNER JOIN tb_CategoryMapping on tb_CategoryMapping.CategoryID=tb_category.CategoryID  where  isnull(Active,0)=1 and isnull(Deleted,0)=0  and name like '%" + txtcategoryinsert.Text.ToString().Replace("'", "''") + @"%') and isnull(Active,0)=1 and isnull(Deleted,0)=0 
            //UNION
            //SELECT tb_category.CategoryID,Name,StoreID,isnull(Active,0) as Active,tb_category.DisplayOrder ,ChildCatCount FROM  tb_CategoryMapping INNER JOIN tb_category on tb_CategoryMapping.categoryid=tb_category.categoryid  WHERE   tb_CategoryMapping.ParentCategoryID=0 and  tb_CategoryMapping.CategoryID in (select CategoryID from tb_category   where  isnull(Active,0)=1 and isnull(Deleted,0)=0  and name like '%" + txtcategoryinsert.Text.ToString().Replace("'", "''") + @"%') and isnull(Active,0)=1 and isnull(Deleted,0)=0 ");
            //                //dsCategoryList = productcomp.GetCategoryDetails(0, 0, 1);
            //            }
            //            else
            //            {
            //mode=1
            //dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder ,ProductCount from tb_category where StoreID=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + " and isnull(Active,0)=1 and isnull(Deleted,0)=0 and isnull(ProductCount,0)<>0 and name like '%" + txtcategoryinsert.Text.Trim() + "%'");

            //            SELECT tb_category.CategoryID,Name,StoreID,isnull(Active,0) as Active,tb_category.DisplayOrder ,ChildCatCount FROM  tb_category  WHERE  CategoryID in (select ParentCategoryID from tb_category INNER JOIN tb_CategoryMapping on tb_CategoryMapping.CategoryID=tb_category.CategoryID  where  isnull(Active,0)=1 and isnull(Deleted,0)=0  and name like '%" + txtcategoryinsert.Text.ToString().Replace("'", "''") + @"%') and isnull(Active,0)=1 and isnull(Deleted,0)=0 and Storeid=1" + @" 
            //UNION
            if (ddlcateggosystatus.SelectedValue == "")
            {
                dsCategoryList = CommonComponent.GetCommonDataSet(@"SELECT tb_category.CategoryID,Name,StoreID,isnull(Active,0) as Active,tb_category.DisplayOrder ,ChildCatCount FROM  tb_CategoryMapping INNER JOIN tb_category on tb_CategoryMapping.categoryid=tb_category.categoryid  WHERE   tb_CategoryMapping.ParentCategoryID=0 and  tb_CategoryMapping.CategoryID in (select CategoryID from tb_category   where   isnull(Deleted,0)=0  and name like '%" + txtcategoryinsert.Text.ToString().Replace("'", "''") + @"%') and tb_CategoryMapping.ParentCategoryID in (SELECT CategoryID FROM tb_CategoryMapping where isnull(ParentCategoryID,0)="+ddlCategory.SelectedValue.ToString()+")  and isnull(Deleted,0)=0 and Storeid=1");
            }
            else
            {
                dsCategoryList = CommonComponent.GetCommonDataSet(@"SELECT tb_category.CategoryID,Name,StoreID,isnull(Active,0) as Active,tb_category.DisplayOrder ,ChildCatCount FROM  tb_CategoryMapping INNER JOIN tb_category on tb_CategoryMapping.categoryid=tb_category.categoryid  WHERE   tb_CategoryMapping.ParentCategoryID=0 and  tb_CategoryMapping.CategoryID in (select CategoryID from tb_category   where  isnull(Active,0)=" + ddlcateggosystatus.SelectedValue.ToString() + " and isnull(Deleted,0)=0  and name like '%" + txtcategoryinsert.Text.ToString().Replace("'", "''") + @"%') and isnull(Active,0)=" + ddlcateggosystatus.SelectedValue.ToString() + " and tb_CategoryMapping.ParentCategoryID in (SELECT CategoryID FROM tb_CategoryMapping where isnull(ParentCategoryID,0)=" + ddlCategory.SelectedValue.ToString() + ")  and isnull(Deleted,0)=0 and Storeid=1");
            }

            //dsCategoryList = productcomp.GetCategoryDetails(0, Convert.ToInt32(ddlStore.SelectedValue.ToString()), 1);
            // }

            if (dsCategoryList != null && dsCategoryList.Tables.Count > 0 && dsCategoryList.Tables[0].Rows.Count > 0)
            {
                grdRowParentCategory.DataSource = dsCategoryList.Tables[0];
                grdRowParentCategory.DataBind();
            }
            else
            {
                grdRowParentCategory.DataSource = null;
                grdRowParentCategory.DataBind();
            }
        }
        /// <summary>
        /// Get Product Image With Full Path
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>return Image with Full Path </returns>
        public string GetIconImageProduct(String img)
        {
            string imagepath = string.Empty;

            try
            {
                imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

                //   if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    imagepath = AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
                    return imagepath;
                }

                imagepath = string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
            }
            catch (Exception ex)
            {

            }
            return imagepath;
        }
        /// <summary>
        /// Method for get data by entered keyword
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete1_Click(object sender, EventArgs e)
        {
        }
        protected void grdRowParentCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lttitle = (Literal)e.Row.FindControl("lttitle");
                GridView grdParentCategory = (GridView)e.Row.FindControl("grdParentCategory");
                Literal ltrStatus = (Literal)e.Row.FindControl("ltrStatus");
                HiddenField hdnActive = (HiddenField)e.Row.FindControl("hdnActive");

                parentrowid = e.Row.RowIndex;
                if (hdnActive.Value != "")
                {
                    if (hdnActive.Value.ToString().ToLower() == "true")
                    {
                        ltrStatus.Text = "<div style=\"color:#468847;font-size:14px;font-weight:bold;text-transform:uppercase;\">Active</div>";
                    }
                    else
                    {
                        ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                    }
                }
                else
                {
                    ltrStatus.Text = "<div style=\"color:#f89406;font-size:14px;font-weight:bold;text-transform:uppercase;\">In-Active</div>";
                }
                RowIndex = e.Row.RowIndex;
                ViewState["grdParentCategory"] = "0";
                Int32 CategoryID = Convert.ToInt32(grdRowParentCategory.DataKeys[e.Row.RowIndex].Value.ToString());

                DataSet dsCategoryList = new DataSet();
                //if (ddlStore.SelectedValue == "-1")
                //{
                //    //mode=1
                //    //dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder from tb_category where  isnull(Active,0)=1 and isnull(Deleted,0)=0");
                //    dsCategoryList = productcomp.GetCategoryDetails(CategoryID, 0, 6);
                //}
                //else
                //{
                //mode=1
                //dsCategoryList = CommonComponent.GetCommonDataSet("select CategoryID,Name,StoreID,isnull(Active,0) as Active,DisplayOrder from tb_category where StoreID=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()) + " and isnull(Active,0)=1 and isnull(Deleted,0)=0");
                // dsCategoryList = productcomp.GetCategoryDetails(CategoryID, Convert.ToInt32(1), 6);
                dsCategoryList = CommonComponent.GetCommonDataSet("Exec GuiGetProductDisplayOrder 1,0, " + CategoryID + ",0,6");
                //}

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
                if (e.Row.RowIndex == 0 && !IsPostBack)
                {
                    hdnProwid.Value = "divRowchild" + CategoryID.ToString();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "expandcollapseRow('divRowchild" + CategoryID.ToString() + "', 'one');", true);
                }
                else
                {
                }

            }
        }

        protected void grdRowParentCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnCancle_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Set Name of Product or category
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Return Max. 62 Length of string</returns>
        public String SetName(String Name)
        {
            string name = string.Empty;
            try
            {
                if (Name.Length > 48)
                    name = Name.Substring(0, 45) + "...";
                else
                    name = Server.HtmlEncode(Name);
            }
            catch (Exception ex)
            {
            }
            return name;

        }
        protected void btnDivClick_Click(object sender, EventArgs e)
        {
            FillRowCategoryList();

            divCollsapnMain.Attributes.Add("onclick", hdnCollspanMain.Value.ToString());
            divCollsapnChild.Attributes.Add("onclick", hdnCollspanChild.Value.ToString());
            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Grid", "document.getElementById('ContentPlaceHolder1_divCollsapnMain').click();document.getElementById('ContentPlaceHolder1_divCollsapnChild').click();");
        }

        protected void lnkbtnsearch_Click1(object sender, EventArgs e)
        {
            lnkbtnsearch_Click(null, null);
        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/dashboard.aspx");

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillRowCategoryList();
        }
    }
}