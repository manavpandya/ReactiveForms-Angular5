using System.Data;
using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.Common;
using System.IO;

namespace Solution.UI.Web
{
    public partial class Category : System.Web.UI.Page
    {
        #region Declaration

        CategoryComponent objCategory = new CategoryComponent();
        public string fullPath = string.Empty;

        #endregion



        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        /// 
        Int32 itemCount = 6;
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (AppLogic.AppConfigBool("UseLiveRewritePath"))
                fullPath += ":" + Request.Url.Port + Request.ApplicationPath.ToLower();
            else
                fullPath += Request.RawUrl.ToLower();

            if (!IsPostBack)
            {
                if (Request.QueryString["CatId"] != null)
                {
                    BindCategoryDetailWithSubCategory();
                    BindSubCategoryOfCategoryWithRepeater();
                    BindBestSeller();
                }
                breadcrumbs();
                if (Request.QueryString["CatPID"] != null)
                {
                    Session["HeaderCatid"] = Request.QueryString["CatPID"].ToString();
                }
                if (Session["HeaderCatid"] != null && Convert.ToInt32(Session["HeaderCatid"]) == 0)
                {
                    if (Request.QueryString["CatID"] != null)
                    {
                        Session["HeaderCatid"] = Request.QueryString["CatID"].ToString();
                    }
                }

                if (Request.QueryString["CatID"] != null)
                {
                    Session["HeaderSubCatid"] = Request.QueryString["CatID"].ToString();
                }
                else
                {
                    Session["HeaderSubCatid"] = null;
                }

            }
            string action = string.Empty;
            string str = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
            action = "/" + Request.Url.ToString().Replace(str, "");
            Context.Items["Original_Path"] = action;
            //String strScript = @"$('#divhalfpricedrapes').css('display', 'none');";

            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "HidecategoryFooter", strScript.Trim(), true);
        }

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            //if (Name.Length > 50)
            //    Name = Name.Substring(0, 47) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Set Category Path
        /// </summary>
        /// <param name="SEName">String SEName</param>
        /// <param name="ParentSEName">String ParentSEName</param>
        /// <returns>Return Category Path</returns>
        public String SetCategoryPath(String SEName, String ParentSEName)
        {
            string CategoryPath = "";
            if (ParentSEName.Length > 0)
                CategoryPath = "/" + ParentSEName + "/" + SEName;
            else CategoryPath = "/" + SEName;
            return CategoryPath;
        }

        #region Bind Breadcrumbs,Category,Sub Category

        /// <summary>
        /// Get BreadCrumbs
        /// </summary>
        private void breadcrumbs()
        {
            try
            {
                string strbreadkrum = ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["CatID"]), Convert.ToInt32(Request.QueryString["CatPID"]), 1, "", 3, 0);
                if (string.IsNullOrEmpty(strbreadkrum))
                {
                    strbreadkrum = " <a href='/' title='Home'>Home </a><img src='/images/breadcrumbs-bullet.png' alt='' title='' class='breadcrumbs-bullet' />";
                    if (Request.QueryString["CatPID"] != null && Request.QueryString["CatPID"].ToString() != "0")
                    {
                        strbreadkrum += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT '<a title=\"'+ Name +'\" href=\"/'+ Sename +'.html\">'+ Name +'</a><img src=\"/images/breadcrumbs-bullet.png\" alt=\"\" title=\"\" class=\"breadcrumbs-bullet\" />' FROm tb_category WHERE categoryid=" + Request.QueryString["CatPID"].ToString() + ""));
                    }
                    if (Request.QueryString["CatID"] != null && Request.QueryString["CatID"].ToString() != "0")
                    {
                        strbreadkrum += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT '<span> '+ Name +' </span> ' FROm tb_category WHERE categoryid=" + Request.QueryString["CatID"].ToString() + ""));
                    }
                    ltbreadcrmbs.Text = strbreadkrum;
                }
                else
                {
                    ltbreadcrmbs.Text = strbreadkrum;
                }
            }
            catch { }
        }

        /// <summary>
        /// Bind Category 
        /// </summary>
        private void BindSubCategoryOfCategoryWithRepeater()
        {
            DataSet dsSubCategory = CategoryComponent.GetSubCategoryByCategoryId(Convert.ToInt32(Request.QueryString["CatId"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsSubCategory != null && dsSubCategory.Tables.Count > 0 && dsSubCategory.Tables[0].Rows.Count > 0)
            {
                RepSubCategory.DataSource = dsSubCategory;
                dvMessage.Visible = false;
            }
            else
            {
                dvMessage.Visible = true;
                RepSubCategory.DataSource = null;
            }
            itemCount = 6;
            RepSubCategory.DataBind();
        }

        /// <summary>
        /// Bind Category With Subcategory
        /// </summary>
        private void BindCategoryDetailWithSubCategory()
        {
            DataSet dsCategory = objCategory.getCatdetailbycatid(Convert.ToInt32(Request.QueryString["CatId"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            try
            {
                String SETitle = "";
                String SEKeywords = "";
                String SEDescription = "";
                if (!string.IsNullOrEmpty(dsCategory.Tables[0].Rows[0]["SETitle"].ToString()))
                {
                    SETitle = dsCategory.Tables[0].Rows[0]["SETitle"].ToString();
                }
                else
                {
                    SETitle = AppLogic.AppConfigs("SiteSETitle").ToString();
                }

                if (!string.IsNullOrEmpty(dsCategory.Tables[0].Rows[0]["SEKeywords"].ToString()))
                {
                    SEKeywords = dsCategory.Tables[0].Rows[0]["SEKeywords"].ToString();
                }
                else
                {
                    SEKeywords = AppLogic.AppConfigs("SiteSEKeywords").ToString();
                }


                if (!string.IsNullOrEmpty(dsCategory.Tables[0].Rows[0]["SEDescription"].ToString()))
                {
                    SEDescription = dsCategory.Tables[0].Rows[0]["SEDescription"].ToString();
                }
                else
                {
                    SEDescription = AppLogic.AppConfigs("SiteSEDescription").ToString();
                }
                Master.HeadTitle(SETitle, SEKeywords, SEDescription);
            }
            catch { }
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                ltTitle.Text = Convert.ToString(dsCategory.Tables[0].Rows[0]["Name"]);
                //RptCategoryDescription.DataSource = dsCategory;
                RptCategoryDescription.DataSource = null;

                //if (Convert.ToString(dsCategory.Tables[0].Rows[0]["Description"]).Length > 0)
                //    divCatBanner.Visible = true;
                //else divCatBanner.Visible = false;

            }
            else
            {
                RptCategoryDescription.DataSource = null;
            }
            RptCategoryDescription.DataBind();
        }

        /// <summary>
        /// Sub Category Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void RepSubCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
                HtmlGenericControl CatDisplay = (HtmlGenericControl)e.Item.FindControl("CatDisplay");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                Literal ltbottom = (Literal)e.Item.FindControl("ltbottom");
                
                //if ((e.Item.ItemIndex + 1) % 5 == 0 && e.Item.ItemIndex != 0)
                //{
                //    //Catbox.Attributes.Add("style", "padding-right:0px;");
                //    ////CatDisplay.Attributes.Add("class", "cat-display-none");
                //    litControl.Text = "</ul><ul>";
                //}

                if ((e.Item.ItemIndex + 1) % itemCount == 0 && e.Item.ItemIndex != 0)
                {
                    //Probox.Attributes.Add("style", "margin-right:0px;");
                    ////proDisplay.Attributes.Add("class", "pro-display-none");
                    //if (RecordCount == itemCount)
                    //{
                    //    litControl.Text = "</ul><ul><li style='width:251px !important;'>";
                    //}
                    //else
                    //{
                   // litControl.Text = "</ul><ul><li>";
                    //}
                  //  ltbottom.Text = "</li>";
                    itemCount = itemCount + 5;
                }
                else
                {
                  //  litControl.Text = "<li>";
                  //   ltbottom.Text = "</li>";
                }

            }
        }

        #endregion

        #region Get Image

        /// <summary>
        /// Get Image Name with Parameter
        /// </summary>
        /// <param name="img">String img</param>
        /// <param name="CategoryId">String CategoryId</param>
        /// <returns>Return Image Path</returns>
        public String GetIconImageCategory(String img, String CategoryId)
        {
            try
            {
                String imagepath = String.Empty;
                imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") +  imagepath;
                }
                else
                {
                    return Getimage(Convert.ToInt32(CategoryId));
                }
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") +  AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            catch
            {
                
            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") +  AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Get Image Path
        /// </summary>
        /// <param name="id">Int32 id</param>        
        /// <returns>Return Image Path</returns>
        private string Getimage(Int32 id)
        {
            try
            {
                String imagepath = String.Empty;
                Solution.Data.SQLAccess objSql = new Data.SQLAccess();
                string strimage = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 Imagename FROM tb_product INNER JOIN tb_ProductCategory ON tb_product.Productid = tb_ProductCategory.Productid WHERE isnull(active,0)=1 and isnull(Deleted,0) =0  AND isnull(Imagename,'') <> '' AND tb_ProductCategory.CategoryID in (SELECT CategoryID FROM tb_CategoryMapping WHERE ParentCategoryID=" + id + ") Order by newid()"));
                imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + strimage;
                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") +imagepath))
                {
                    return imagepath;
                }
                else
                {
                    Int32 CatId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Top 1 CategoryID FROM tb_CategoryMapping WHERE ParentCategoryID=" + id + ""));
                    if (CatId > 0)
                    {
                        return Getimage(CatId);
                    }
                    else
                    {
                        return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
                    }
                }
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            catch
            {

            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }
        #endregion

        #region Remove ViewState From page

        /// <summary>
        /// Remove ViewState From page.
        /// </summary>
        /// <returns>Returns object</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Save Page State To Persistence Medium
        /// </summary>
        /// <param name="state">object state</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }
        #endregion

        /// <summary>
        /// Bind Best Seller
        /// </summary>
        private void BindBestSeller()
        {
            ///Bind Best Seller
            DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 4);
            if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
            {
                rptBestSeller.DataSource = dsBestSeller;
                rptBestSeller.DataBind();
            }

        }

        /// <summary>
        /// Best Seller Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptBestSeller_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl ProboxBestSeller = (HtmlGenericControl)e.Item.FindControl("ProboxBestSeller");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Label lblTagName = (Label)e.Item.FindControl("lblTagImageName");
                Literal lblTagImage = (Literal)e.Item.FindControl("lblTagImage");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");

                if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                {
                    ProboxBestSeller.Attributes.Add("style", "padding-right: 0px;");
                }
                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                        {
                            lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "'  />";
                        }
                    }
                }

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");

                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor abestLink = (HtmlAnchor)e.Item.FindControl("abestLink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "<p>" + Price.ToString("C") + "</p>";//"<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else
                {
                    ltrRegularPrice.Text += "<p>&nbsp;</p>";
                }
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "<p>" + SalePrice.ToString("C") + "</p>";//"<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else
                {
                    ltrYourPrice.Text += "<p>" + Price.ToString("C") + "</p>";
                }
                if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                {

                    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                    if (Count == 0)
                    {


                        abestLink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + ",'" + abestLink.ClientID.ToString() + "');");
                    }
                    else
                    {
                        abestLink.Attributes.Remove("onclick");
                        abestLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        //abestLink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                    }
                }
                else
                {
                    abestLink.Attributes.Remove("onclick");
                    abestLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                }


            }
        }

        /// <summary>
        /// Get Icon Image Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns String</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Replace the '"' and '\' which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }
    }
}