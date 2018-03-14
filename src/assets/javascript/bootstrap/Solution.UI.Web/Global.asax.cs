using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Data;

namespace Solution.UI.Web
{
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// Handles the Start event of the Application control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            AppLogic.ApplicationStart();
        }

        /// <summary>
        /// Handles the Start event of the Session control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            String StrReferrer;
            int ID = 0;
            int CategoryCount = 0;
            string Name = string.Empty;
            string parentName = string.Empty;
            string grandName = string.Empty;
            string strCurrentPath = string.Empty;
            string SENAME = "";
            //** Root URL like http://www.xyz.com/


            if (Request.RawUrl != null && Request.RawUrl.ToString() == "/" && Request.Url.Authority.ToString().ToLower().IndexOf("halfpricedrapes.us") <= -1)
            {
                Context.RewritePath("~/index.aspx");
            }
            else if (Request.RawUrl != null && Request.RawUrl.ToString().IndexOf("?") > -1 && Request.RawUrl.ToString().Substring(0, Request.RawUrl.ToString().IndexOf("?")) == "/" && Request.Url.Authority.ToString().ToLower().IndexOf("halfpricedrapes.us") <= -1)
            {
                Context.RewritePath("~/index.aspx");
            }


            //** Admin path - Rewrite is not apply
            if (Request.RawUrl.ToString().ToLower().Contains("/admin/") || Request.RawUrl.ToString().ToUpper().Contains("/REPLENISHMENTMANAGEMENT/"))
            {

                if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("halfpricedrapes.com/admin/login.aspx") > -1)
                {
                    Response.Redirect("http://www.halfpricedrapes.com/");
                    return;
                }


                if (!Request.Url.Authority.StartsWith("www.") && AppLogic.AppConfigBool("UseWWWRedirect"))
                {

                    Response.Redirect(Request.Url.Scheme + "://www." + Request.Url.Authority); //e.g  http://www.xyz.com
                }
                else
                {
                    Context.RewritePath(Request.RawUrl);
                    return;
                }
            }
            else
            {
                if (Request.Url != null && Request.Url.Authority.ToString().ToLower().IndexOf("halfpricedrapes.us") > -1 && Request.Url.ToString().ToLower().IndexOf("testmail.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("chargelogiccheckout.aspx") <= -1 && Request.Url.ToString().ToLower().IndexOf("chargelogicmerchantresource.html") <= -1 && (Request.Url.ToString().ToLower().IndexOf(".html") > -1 || Request.Url.ToString().ToLower().IndexOf(".aspx") > -1 || Request.Url.ToString().ToLower().IndexOf(".asmx") > -1))
                {
                    Response.Redirect("http://www.halfpricedrapes.com/");
                    return;
                }
            }

            if (Request.RawUrl.ToString().ToLower().Contains("/login/"))
            {
                Context.RewritePath(Request.RawUrl);
                return;
            }




            //** Check for Redirect logic. 

            if (!Request.Url.Authority.StartsWith("www.") && AppLogic.AppConfigBool("UseWWWRedirect"))
            {
                if (Request.RawUrl != null)
                {
                    Response.Redirect(Request.Url.Scheme + "://www." + Request.Url.Authority + Request.RawUrl); //e.g  http://www.xyz.com
                }
                else
                {
                    Response.Redirect(Request.Url.Scheme + "://www." + Request.Url.Authority); //e.g  http://www.xyz.com
                }

            }



            string FullPath = Server.UrlDecode(Request.RawUrl.ToLower());
            string AppPath = Request.Url.Scheme + "://" + Request.Url.Authority;

            if (AppLogic.AppConfigBool("UseLiveRewritePath"))
            {
                AppPath += ":" + Request.Url.Port + Request.ApplicationPath.ToLower();
            }
            else
            {
                AppPath += Request.ApplicationPath.ToLower();

            }

            if (FullPath.ToString().ToLower() == "ind.html" || FullPath.ToString().ToLower() == "/ind.html")
            {
                Context.RewritePath("~/Sitemap.aspx");
                return;
            }
            if (FullPath.ToString().ToLower() == "free-swatch.html" || FullPath.ToString().ToLower() == "/free-swatch.html")
            {
                Context.RewritePath("~/Free-Swatch.aspx?catid=0");
                return;
            }

            string strExtension = System.IO.Path.GetExtension(Request.Url.ToString().Replace("\"", "%22").Replace("<", "").Replace(">", "").Replace("|", "")).ToLower();

            //Return when extension are .aspx and url found in physical path.
            try
            {
                if (strExtension.StartsWith(".aspx") && System.IO.File.Exists(Server.MapPath(Request.Url.AbsolutePath)) && (!Request.Url.AbsolutePath.StartsWith("/Redirect.aspx")))
                {
                    return;
                }
            }
            catch
            {
            }

            if (Request.RawUrl.ToString().ToLower().IndexOf("itempage.aspx") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }
            if (Request.RawUrl.ToString().ToLower().IndexOf("itempagerating.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("itempageratingnew.aspx") > -1 || Request.RawUrl.ToString().ToLower().IndexOf("romanitempagerating.aspx") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }

            if (Request.RawUrl.ToString().ToLower().IndexOf("subcategory.aspx") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }
            if (Request.RawUrl.ToString().ToLower().IndexOf("productvideo.xml") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }
            if (Request.RawUrl.ToString().ToLower().IndexOf("ProductVideo.swf") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }

            if (Request.RawUrl.ToString().ToLower().IndexOf("category.aspx") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }


            if (Request.RawUrl.ToString().ToLower().IndexOf("subcategory.aspx") > -1)
            {
                Context.RewritePath("~/" + Request.RawUrl.ToString().Substring(Request.RawUrl.ToString().LastIndexOf("/") + 1, Request.RawUrl.ToString().Length - Request.RawUrl.ToString().LastIndexOf("/") - 1));
                return;

            }

            //Return when JS,CSS or .axd files are found
            if (strExtension.StartsWith(".axd") || strExtension == ".js" || strExtension == ".css")
                return;

            //retuen when Image or Xml  files are found
            if (strExtension == ".jpg" || strExtension == ".gif" || strExtension == ".jpeg" || strExtension == ".png"
                || strExtension == ".xml")
            { return; }

            //Counvert into Lower Case
            strCurrentPath = Request.Path.ToLower();


            //** Category 
            if (FullPath.Length > 1)
            {


                string Indexstr = FullPath.Substring(1);
                if (Indexstr.IndexOf("/") > -1 && (Indexstr.ToLower().IndexOf(".html") > -1 || Indexstr.ToLower().IndexOf(".aspx") > -1))
                {
                    FullPath = "/" + Indexstr.Replace("/", "-");
                }
            }

            string[] Catgories = FullPath.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int TotalCount = Catgories.Length;
            bool isProduct = false;//Set for Product details

            //** Check for product
            //if (TotalCount > 1 && Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().ToLower().EndsWith(".aspx"))
            //    isProduct = true;
            Int32 PID = 0;

            if (TotalCount > 0)
            {
                if (Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().ToLower().IndexOf("-review") > -1)
                {
                    PID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 productId FROM tb_Product WHERE Storeid=" + AppLogic.AppConfigs("StoreId").ToString() + " AND isnull(active,0)=1  and isnull(deleted,0)=0 AND  ProductUrl='" + Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().ToLower().Replace("-review", "") + "'"));
                    if (PID > 0)
                    {
                        isProduct = true;
                    }
                }
                else
                {

                    PID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 productId FROM tb_Product WHERE Storeid=" + AppLogic.AppConfigs("StoreId").ToString() + " AND isnull(active,0)=1  and isnull(deleted,0)=0 AND  ProductUrl='" + Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim() + "'"));
                    if (PID > 0)
                    {
                        isProduct = true;
                    }
                }
            }



            if (TotalCount > 2)
                grandName = Catgories[TotalCount - 3].ToString().Replace(".html", "");
            if (TotalCount > 1)
                parentName = Catgories[TotalCount - 2].ToString().Replace(".html", "");
            if (TotalCount > 0)
            {
                Name = Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().Replace(".html", "");
            }

            if (!isProduct)
            {


                ID = CommonComponent.GetCategoryID(Name, parentName, grandName, Convert.ToInt32(1), 0, 1); //Child name, Parent Name, Grand Name, StoreID, Optional CategoryId,3-select Query for Category Available or not
            }
            else
            {
                //string ProductName = Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().Replace(".aspx", "");
                //SENAME = ProductName.Substring(0, ProductName.LastIndexOf("-"));
                //ProductName = ProductName.Substring(ProductName.LastIndexOf("-") + 1);
                //Int32.TryParse(ProductName, out ID);

                Int32.TryParse(PID.ToString(), out ID);
            }

            //** Redirect Category or Product Page
            if (ID != 0)
            {
                if (!isProduct)
                {
                    if (parentName == "")
                    {
                        string CheckparentNotAvail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT COUNT(*) FROM dbo.tb_CategoryMapping WHERE CategoryID='" + ID + "' AND ParentCategoryID<>0"));

                        if (CheckparentNotAvail != null && CheckparentNotAvail != "" && CheckparentNotAvail != "0")
                        {
                            parentName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SeName FROM dbo.tb_Category WHERE CategoryID in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE CategoryID=" + ID + " AND ParentCategoryID<>0)"));
                            //Context.RewritePath("~/Rewriter.aspx");
                            //return;
                        }
                    }
                    CategoryCount = CommonComponent.GetCategoryID("", "", "", Convert.ToInt32(1), ID, 2);//Mode-2-Get Total count from Category 

                    // Get Parent CategoryID  from CatID
                    int ParentCatID = 0;

                    if (parentName != "" && parentName.Trim().Length > 0)
                    {
                        ParentCatID = CommonComponent.GetCategoryID("", parentName, "", Convert.ToInt32(1), 0, 4);
                    }

                    if (CategoryCount > 0)
                    {
                        // find more category then redirect to category page
                        //if (Request.RawUrl != null && Request.RawUrl.ToString().IndexOf("https://") > -1)
                        //{
                        //    string strSename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Sename FROM tb_category WHERE CategoryId=" + ID.ToString() + ""));
                        //    Context.RewritePath("http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + strSename.ToString() + ".html");
                        //}
                        //else
                        //{
                        Context.RewritePath("~/category.aspx?CatID=" + ID.ToString() + "&CatPID=" + ParentCatID);
                        //}
                    }
                    else
                    {
                        //Redirect to subcategory when last category find
                        if (AppLogic.AppConfigs("RomanCategoryID").ToString().ToString().IndexOf("," + ID.ToString() + ",") > -1)
                        {
                            //if (Request.RawUrl != null && Request.RawUrl.ToString().IndexOf("https://") > -1)
                            //{
                            //    string strSename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Sename FROM tb_category WHERE CategoryId=" + ID.ToString() + ""));
                            //    Response.Redirect("http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + strSename.ToString() + ".html");
                            //}
                            //else
                            //{
                            Context.RewritePath("~/RomanCategory.aspx?CatID=" + ID.ToString() + "&CatPID=" + ParentCatID);
                            //}
                        }
                        else
                        {
                            //if (Request.RawUrl != null && Request.RawUrl.ToString().IndexOf("https://") > -1)
                            //{
                            //    string strSename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Sename FROM tb_category WHERE CategoryId=" + ID.ToString() + ""));
                            //    Context.RewritePath("http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + strSename.ToString() + ".html");
                            //}
                            //else
                            //{
                            Context.RewritePath("~/SubCategory.aspx?CatID=" + ID.ToString() + "&CatPID=" + ParentCatID);
                            //}
                        }
                    }
                }
                else
                {
                    //string CheckProductId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ISNULL(ProductID,0) FROM dbo.tb_Product WHERE SEName='" + SENAME + "' AND MainCategory='" + parentName + "' AND ProductID='" + ID + "'"));
                    //if (CheckProductId != null && CheckProductId != "" && CheckProductId != "0")
                    //    Context.RewritePath("~/ItemPage.aspx?PID=" + ID.ToString());
                    //else
                    //    Context.RewritePath("~/Rewriter.aspx");

                    if (Catgories[TotalCount - 1].Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim().ToLower().IndexOf("-review") > -1)
                    {
                        //Context.RewritePath("~/ItemPageRating.aspx?PID=" + ID.ToString());
                        //String strCategoryid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 CategoryID FROM tb_ProductCategory WHERE Productid=" + ID.ToString() + ""));
                        //if (AppLogic.AppConfigs("RomanCategoryID").ToString().IndexOf("," + strCategoryid.ToString() + ",") > -1)
                        //{
                        //    Context.RewritePath("~/Roman-ItemPage.aspx?PID=" + ID.ToString());
                        //}
                        //else
                        //{
                        //    Context.RewritePath("~/ItemPage.aspx?PID=" + ID.ToString());
                        //}
                        if (Request.RawUrl != null)
                        {
                            Response.Redirect(Request.RawUrl.ToString().ToLower().Replace("-review.html", ".html"));
                        }
                        else
                        {
                            String strCategoryid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 CategoryID FROM tb_ProductCategory WHERE Productid=" + ID.ToString() + ""));
                            if (AppLogic.AppConfigs("RomanCategoryID").ToString().IndexOf("," + strCategoryid.ToString() + ",") > -1)
                            {
                                Context.RewritePath("~/Roman-ItemPage.aspx?PID=" + ID.ToString());
                            }
                            else
                            {
                                Context.RewritePath("~/ItemPage.aspx?PID=" + ID.ToString());
                            }
                        }
                    }
                    else
                    {
                        String strCategoryid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 CategoryID FROM tb_ProductCategory WHERE Productid=" + ID.ToString() + ""));
                        if (AppLogic.AppConfigs("RomanCategoryID").ToString().IndexOf("," + strCategoryid.ToString() + ",") > -1)
                        {
                            Context.RewritePath("~/Roman-ItemPage.aspx?PID=" + ID.ToString());
                        }
                        else
                        {
                            Context.RewritePath("~/ItemPage.aspx?PID=" + ID.ToString());
                        }
                    }

                }
                return;
            }

            if (strCurrentPath.Contains("/gi-"))
            {
                string[] strCarte = strCurrentPath.Split('-');
                strCurrentPath = strCurrentPath.Substring(strCurrentPath.IndexOf("/gi-"));
                string strCustomCartID = "";
                string id = strCurrentPath.Substring(strCurrentPath.LastIndexOf("-") + 1).Replace(".aspx", "");
                if (strCarte.Length > 1)
                {
                    strCustomCartID = strCarte[1].ToString();
                    int CustomCartID = 0;
                    int.TryParse(strCustomCartID, out CustomCartID);
                    string CheckProductId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(CustomCartID,0) FROM tb_ShoppingCartItems WHERE CustomCartID='" + CustomCartID.ToString() + "'  AND ProductID='" + id + "'"));
                    if (CheckProductId != null && CheckProductId != "" && CheckProductId != "0")
                    {
                        strCurrentPath = "GiftItemPage.aspx?ProductID=" + id.ToString() + "&CustomCartID=" + CustomCartID;
                        Context.RewritePath(strCurrentPath);
                        return;
                    }
                    else
                    {
                        strCurrentPath = "GiftItemPage.aspx?ProductID=" + id.ToString();
                        Context.RewritePath(strCurrentPath);
                        return;

                    }
                }
                else
                {
                    strCurrentPath = "GiftItemPage.aspx?ProductID=" + id.ToString();
                    Context.RewritePath(strCurrentPath);
                    return;
                }
                //strCurrentPath.Split('-')[1].ToString().Replace(".aspx", "");
            }

            //** Get Topic Details
            if (strCurrentPath.Contains("/") && strCurrentPath.ToString().ToLower().Contains(".html"))
            {
                DataSet dsTopic = new DataSet();
                string id = strCurrentPath.Substring(1);
                dsTopic = TopicComponent.GetTopicList(id.Split('.')[0].ToString().Replace("-", " "), Convert.ToInt32(1));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    Context.RewritePath("StaticPage.aspx?StaticPage=" + id.Split('.')[0].ToString().Replace("-", " "));
                    return;
                }
            }


            //Shop By Price

            //Name
            DataSet dsShopByPrice = new DataSet();
            //string ShopByPriceId = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 PriceRangeId  from tb_PriceRange where SEName='" + Name + "' and Status=1 and StoreID='" + Convert.ToString(AppLogic.AppConfigs("StoreID") + "'")));

            string ShopByPriceId = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 PriceRangeId  from tb_PriceRange where SEName='" + Name + "' and Status=1 and StoreID='1'"));

            if (ShopByPriceId != null && ShopByPriceId != "" && ShopByPriceId != "0")
            {
                Context.RewritePath("~/SubCategory.aspx?PriceID=" + ShopByPriceId.ToString());
                return;
            }


            //Redirect to New Arrivals page 
            if (Request.RawUrl.ToString().ToLower().IndexOf("new-arrivals") > -1)
            {
                Context.RewritePath("NewArrival.aspx");
                return;
            }
            else
            {
                if (strCurrentPath.Contains("/") && strCurrentPath.ToString().ToLower().Contains(".html"))
                {

                    string id = strCurrentPath.Substring(1);
                    string strname = id.Split('.')[0].ToString();
                    string strlink = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Sename FROM tb_ProductSearchType WHERE SEName='" + strname.ToString() + "'"));
                    if (!string.IsNullOrEmpty(strlink))
                    {
                        Context.RewritePath("~/ProductSearchList.aspx");
                        return;
                    }

                }
            }
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Error event of the Application control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Solution.Bussines.Components.ErrorHandlerComponent handler = new Solution.Bussines.Components.ErrorHandlerComponent();
            handler.HandleException();
        }

        /// <summary>
        /// Handles the End event of the Session control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the End event of the Application control
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}