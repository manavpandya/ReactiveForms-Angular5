using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text;
using Solution.Bussines.Components.Common;
using StringBuilder = System.Text.StringBuilder;
using StringWriter = System.IO.StringWriter;
using DataSet = System.Data.DataSet;
using DataRow = System.Data.DataRow;
using System.IO;
namespace Solution.UI.Web.Client.Control
{
    /// <summary>
    /// Left Menu Control for display left side menu on Client Side
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 

    public partial class LeftMenu : System.Web.UI.UserControl
    {
        #region Declaration

        protected string MenuData = string.Empty;
        CategoryComponent objCategorycomponent = new CategoryComponent();
        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
        string storeID = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());

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

                SetPriceRange();
                BindBestSeller();

            }
            BindLeftMenu();


        }

        /// <summary>
        /// Add '...', if String length is more than 40 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 40 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 40)
                Name = Name.Substring(0, 35) + "...";
            return Server.HtmlEncode(Name);
        }

        #region Get Icon Image

        /// <summary>
        /// Get Icon Image for Category
        /// </summary>
        /// <param name="img">string img</param>
        /// <returns>Return Image With Full Path</returns>
        public String GetIconImageCategory(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">string img</param>
        /// <returns>Return Image With Full Path</returns>
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

        #endregion

        /// <summary>
        /// Bind Best Sellers Details
        /// </summary>
        private void BindBestSeller()
        {
            ///Bind Best Seller
            //  DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 3);
            DataSet dsBestSeller = CommonComponent.GetCommonDataSet("select top 3 ProductID,Name,isnull(Price,0) as Price,isnull(SalePrice,0) as SalePrice, isnull(tb_Product.Description,'''') as Description, " +
                                                             " ImageName,isnull(DisplayOrder,0) as DisplayOrder,SKU, MainCategory, SEName,TagName,case when isnull(tb_Product.Tooltip,'''')='''' then tb_Product.Name else tb_Product.Tooltip end as Tooltip ,  " +
                                                             " isnull(tb_Product.Inventory,0) as Inventory,isnull(IsFreeEngraving,0) as IsFreeEngraving   from tb_Product   " +
                                                             " where isnull(Active,0)=1 and StoreID=1 and isnull(Deleted,0)=0 and  isnull(IsBestSeller,0)=1 and tb_Product.productid not in(select tb_Giftcardproduct.productid from tb_Giftcardproduct)");
            if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
            {

                rptBestSeller.DataSource = dsBestSeller;
                rptBestSeller.DataBind();
            }

            //Bind New arrival
            //DataSet dsFeatured = ProductComponent.DisplyProductByOption("IsFeatured", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 12);
            //if (dsFeatured != null && dsFeatured.Tables.Count > 0 && dsFeatured.Tables[0].Rows.Count > 0)
            //{
            //    rptFeaturedProduct.DataSource = dsFeatured;
            //    rptFeaturedProduct.DataBind();
            //}
        }

        /// <summary>
        /// Method for Bind Left Menu from Category Table up to One Level
        /// </summary>
        private void BindLeftMenu()
        {
            #region get parent menu
            int StoreID = 1;// Convert.ToInt32(AppLogic.AppConfigs("StoreID"));
            var sQueryparentMenu1 = (from cat in ctxRedtag.tb_Category
                                     join catMap in ctxRedtag.tb_CategoryMapping
                                     on cat.CategoryID equals catMap.CategoryID
                                     where cat.Active == true && cat.Deleted == false && catMap.ParentCategoryID == 0
                                     select cat).OrderBy(a => a.DisplayOrder);

            var sQueryparentMenu = (from a in sQueryparentMenu1
                                    where a.tb_Store.StoreID == StoreID
                                    select a);

            StringBuilder sb = new StringBuilder();

            sb.Append("<h2>Shop By Categories</h2>");

            sb.Append("<ul>");
            foreach (var parentMenu in sQueryparentMenu)
            {
                #region main parent menu
                //sb.Append("<li>");
                //sb.Append("<a href='/" + parentMenu.SEName + "' title= '" + parentMenu.Name + "' >");
                //sb.Append(parentMenu.Name);
                //sb.Append("</a>");

                string currentcatid = "";
                string Parentcatid = "";
                Parentcatid = "/" + parentMenu.SEName.ToString();
                string strrquest = Request.RawUrl.ToString();

                bool Itempag = false;
                string MainCatname = "";

                try
                {
                    if (strrquest.Contains(".aspx"))
                    {


                        string SENAME = "";
                        int PID = 0;
                        string ProductName = strrquest.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().Replace(".aspx", "");
                        if (ProductName.Contains("-"))
                        {
                            SENAME = ProductName.Substring(0, ProductName.LastIndexOf("-"));
                            ProductName = ProductName.Substring(ProductName.LastIndexOf("-") + 1);
                            Int32.TryParse(ProductName, out PID);

                            strrquest = Convert.ToString(CommonComponent.GetScalarCommonData("select  dbo.[fn_GetProductMainSenameCatPath](''," + PID + ",0," + StoreID + ")  from tb_Product where ProductID=" + PID + " and StoreID=" + StoreID + " "));

                        }
                        if (PID > 0)
                        {
                            string[] strcat = strrquest.Split(':');

                            if (strcat.Length > 1)
                            {
                                strrquest = "/" + strcat[0].ToString();
                                MainCatname = strcat[1].ToString();
                            }
                            else
                            {
                                strrquest = "/" + strcat[0].ToString();
                                MainCatname = strcat[0].ToString();
                            }
                        }
                        else
                        {

                            string[] strcat = strrquest.Split('/');
                            string strMain = "";
                            if (strcat.Length > 0)
                            {
                                strMain = strcat[1].Replace("/", "");
                                try
                                {
                                    strrquest = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SEName FROM dbo.tb_Category WHERE CategoryID in(SELECT TOP 1 ParentCategoryID FROM dbo.tb_CategoryMapping WHERE CategoryID IN(SELECT CategoryID FROM dbo.tb_Category WHERE SEName='" + strMain + "' AND storeid='" + StoreID + "'))"));
                                }
                                catch { }

                            }
                            if (strrquest != "")
                                strrquest = "/" + strrquest;
                            else
                                strrquest = "/" + strMain;

                            MainCatname = strMain;

                        }
                    }
                    else if (Request.RawUrl.Contains('/'))
                    {
                        string[] strcat = Request.RawUrl.Split('/');
                        if (strcat.Length > 2)
                        {
                            strrquest = "/" + strcat[1].ToString();

                            MainCatname = strcat[strcat.Length - 1].ToString();
                        }
                    }

                }
                catch { }


                if (strrquest == Parentcatid)
                {
                    sb.Append("<li>");
                    sb.Append("<a class='active1' style='float:left;color:#EF6501;' href='/" + parentMenu.SEName + "'>");
                    sb.Append(parentMenu.Name);
                    sb.Append("</a>");

                    #region bind sub menu of parent menu

                    var sQuerySubmenu = (from cat in ctxRedtag.tb_Category
                                         join catmap in ctxRedtag.tb_CategoryMapping
                                             on cat.CategoryID equals catmap.CategoryID
                                         where cat.Active == true && cat.Deleted == false && catmap.ParentCategoryID == parentMenu.CategoryID
                                         select cat).OrderBy(a => a.DisplayOrder);
                    //sb.Append("<ul>");
                    if (sQuerySubmenu.Count() > 0)
                    {
                        sb.Append("<ul>");
                        foreach (var subMenu in sQuerySubmenu)
                        {
                            if (subMenu.SEName.ToLower() == MainCatname.ToLower())
                            {
                                sb.Append("<li><a style='color:#EF6501;' href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                            }
                            else
                            {
                                sb.Append("<li><a style='' href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                            }


                            sb.Append("&nbsp;" + subMenu.Name);
                            sb.Append("</a></li>");
                        }
                        sb.Append("</ul>");
                    }
                    //sb.Append("</ul>");
                    #endregion
                }
                else
                {
                    sb.Append("<li>");
                    sb.Append("<a href='/" + parentMenu.SEName + "'>");
                    sb.Append(parentMenu.Name);
                    sb.Append("</a>");
                }
                #endregion

                //#region bind sub menu of parent menu

                //var sQuerySubmenu = (from cat in ctxRedtag.tb_Category
                //                     join catmap in ctxRedtag.tb_CategoryMapping
                //                         on cat.CategoryID equals catmap.CategoryID
                //                     where cat.Active == true && cat.Deleted == false && catmap.ParentCategoryID == parentMenu.CategoryID
                //                     select cat).OrderBy(a => a.DisplayOrder);
                ////sb.Append("<ul>");
                //if (sQuerySubmenu.Count() > 0)
                //{
                //    sb.Append("<ul>");
                //    foreach (var subMenu in sQuerySubmenu)
                //    {
                //        //sb.Append("<li><a href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                //        sb.Append("<li><a style='background-position:16px' href='/" + parentMenu.SEName + "/" + subMenu.SEName + "' title='" + subMenu.Name + "'>");
                //        sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;" + subMenu.Name);
                //        sb.Append("</a></li>");
                //    }
                //    sb.Append("</ul>");
                //}
                ////sb.Append("</ul>");
                //#endregion

                sb.Append("</li>");
            }
            sb.Append("</ul>");
            MenuData = sb.ToString();
            #endregion
        }

        /// <summary>
        /// Set Price Range for Product
        /// </summary>
        private void SetPriceRange()
        {

            DataSet dsPriceRange = new DataSet();
            dsPriceRange = objCategorycomponent.GetAllPriceRange(storeID);
            if (dsPriceRange != null && dsPriceRange.Tables[0].Rows.Count > 0)
            {
                StringBuilder sw = new StringBuilder();

                sw.AppendLine("<ul>");
                int indexno = 0;
                foreach (DataRow selDR in dsPriceRange.Tables[0].Rows)
                {
                    if (Convert.ToBoolean(objCategorycomponent.CheckProductforPriceRange(selDR["MinPrice"].ToString(), selDR["MaxPrice"].ToString(), storeID.ToString())))
                    {
                        if (dsPriceRange.Tables[0].Rows.Count == (indexno + 1))
                            sw.Append("<li>");
                        else
                            sw.Append("<li>");
                        if (Request.QueryString["PrId"] != null && Request.QueryString["PrId"] != "" && Request.QueryString["PrId"] == selDR["PriceRangeId"].ToString())
                            sw.Append("<a  class='active' href='/" + Server.UrlEncode(selDR["SEName"].ToString()) + "' title= '" + selDR["PriceRangeName"].ToString() + "'>" + selDR["PriceRangeName"].ToString() + "</a>");
                        else
                            sw.Append("<a  href='/" + Server.UrlEncode(selDR["SEName"].ToString()) + "' title= '" + selDR["PriceRangeName"].ToString() + "'>" + selDR["PriceRangeName"].ToString() + "</a>");
                        sw.Append("</li>");
                        indexno++;
                    }
                }
                sw.AppendLine("</ul>");
                ltShopbyPrice.Text += sw.ToString();
            }
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