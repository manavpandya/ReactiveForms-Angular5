using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using StringBuilder = System.Text.StringBuilder;
using File = System.IO.File;
using StreamWriter = System.IO.StreamWriter;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GenerateGoogleXML : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent objStorecomponent = new StoreComponent();
        TopicComponent ObjTopic = new TopicComponent();

        DataSet dsCategory = null;
        StringBuilder sitemap = new StringBuilder();
        String catCSSClass = String.Empty;
        String storeID = String.Empty;

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
                btnGenerate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/generate.png";
                bindstore();
            }
            btnGenerate.Attributes.Add("onclick", "return confirm('This will Delete Previously generated Sitemap.xml file, Are you Sure?');");

        }

        /// <summary>
        ///  Generate Google XML Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGenerate_Click(object sender, ImageClickEventArgs e)
        {
            if (System.IO.File.Exists(Server.MapPath("~/Sitemap.xml")))
                System.IO.File.Delete(Server.MapPath("~/Sitemap.xml"));

            storeID = ddlStore.SelectedValue.ToString();
            sitemap.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sitemap.Append(" <urlset \n");
            sitemap.Append(" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" \n");
            sitemap.Append(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" \n");
            sitemap.Append(" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 \n");
            sitemap.Append(" http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\"> \n");
            sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + "</loc><changefreq>weekly</changefreq><priority>1.00</priority></url>");
            GetProducts();
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
        }

        /// <summary>
        /// Gets the Product Detail for Google XML
        /// </summary>
        public void GetProductDetail()
        {
            try
            {
                String Query = "select ProductUrl from tb_Product where isnull(tb_Product.Deleted,0) <> 1 and isnull(tb_Product.Active,0) = 1 and tb_Product.StoreID=" + ddlStore.SelectedValue.ToString() + " Order By ProductID ASC";
                DataSet dsProduct = new DataSet();
                dsProduct = CommonComponent.GetCommonDataSet(Query);

                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0 && dsProduct.Tables[0] != null)
                {
                    foreach (DataRow dr in dsProduct.Tables[0].Rows)
                    {
                       // String str = CommonOperations.SetSEName(dr["SEName"].ToString());
                        sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + dr["ProductUrl"].ToString() + "</loc><changefreq>weekly</changefreq><priority>0.65</priority></url>");
                    }
                }
            }
            catch { }

        }

        /// <summary>
        /// Get Products and Product Categories
        /// </summary>
        public void GetProducts()
        {
            try
            {
                dsCategory = CommonComponent.GetCommonDataSet("Select c.CategoryID,Name,SEName,cm.ParentCategoryID,DisplayOrder From tb_Category c inner join tb_CategoryMapping cm on c.CategoryID=cm.CategoryID Where active=1 and Deleted=0  And c.StoreID=" + ddlStore.SelectedValue.ToString() + " Order By DisplayOrder ASC");
                GenerateSitemap();
                GetProductDetail();
                sitemap.Append("</urlset>");
                StreamWriter sw = new StreamWriter(System.IO.File.Open(Server.MapPath("/images/Sitemap.xml"), System.IO.FileMode.Create));
                sw.Write(sitemap.ToString());
                sw.Close();
                CommonOperations.SaveOnContentServer(Server.MapPath("/images/Sitemap.xml"));
                Page.RegisterStartupScript("Msg", "<script type='text/javascript' lang='javascript'>alert('Google XML file created Successfully!');</script>");
            }
            catch { }
        }

        /// <summary>
        /// Generate SiteMap file Xml for the Topic
        /// </summary>
        private void GenerateSitemap()
        {
            DataSet DSTopic = new DataSet();
            DSTopic = ObjTopic.GetTopicListByStoreID(Convert.ToInt32(ddlStore.SelectedValue));
            if (DSTopic != null && DSTopic.Tables.Count > 0 && DSTopic.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DSTopic.Tables[0].Rows)
                {
                    if (dr["Title"].ToString().ToLower() != "size chart")
                    {
                        sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + dr["TopicName"].ToString() + ".html</loc><changefreq>weekly</changefreq><priority>0.80</priority></url>");
                    }
                }
            }
           // sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/OrderStatus.aspx</loc><changefreq>weekly</changefreq><priority>0.80</priority></url>");
            sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/Contactus.html</loc><changefreq>weekly</changefreq><priority>0.80</priority></url>");

            if (dsCategory.Tables[0].Select("ParentCategoryID=0").Length > 0)
            {
                foreach (DataRow PCatDR in dsCategory.Tables[0].Select("ParentCategoryID=0", "DisplayOrder"))
                {
                    sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + PCatDR["SEName"].ToString().Trim() + ".html</loc><changefreq>weekly</changefreq><priority>0.80</priority></url>");
                    WriteSubCategory(PCatDR["CategoryID"].ToString().Trim(), PCatDR["SEName"].ToString(), 1, true);
                }
            }
        }

        /// <summary>
        /// Append the Xml text in the Xml file
        /// </summary>
        /// <param name="CatID">string CatID</param>
        /// <param name="CatName">string CatName</param>
        /// <param name="CategoryLevel">int CategoryLevel</param>
        /// <param name="isRoot">Boolean isRoot</param>
        private void WriteSubCategory(String CatID, String CatName, int CategoryLevel, bool isRoot)
        {
            if (isRoot)
                CatName = "";
            else
                CatName += "/";
            string SubCatName = string.Empty;
            if (dsCategory.Tables[0].Select("ParentCategoryID=" + CatID).Length > 0)
            {
                foreach (DataRow CatRW in dsCategory.Tables[0].Select("ParentCategoryID=" + CatID))
                {
                    switch (CategoryLevel)
                    {
                        case 1:
                            catCSSClass = "category";
                            break;
                        case 2:
                            catCSSClass = "sub_category";
                            break;
                        case 3:
                            catCSSClass = "item";
                            break;
                        default:
                            return;
                    }
                    SubCatName = CatName + CatRW["SEName"].ToString();
                    sitemap.Append("<url><loc>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + SubCatName + ".html</loc><changefreq>weekly</changefreq><priority>0.90</priority></url>");
                    if (CategoryLevel <= 3)
                        WriteSubCategory(CatRW["CategoryID"].ToString().Trim(), SubCatName, CategoryLevel + 1, false);
                }
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            String StoreID = "";
            if (ddlStore.SelectedIndex > 0)
                StoreID = ddlStore.SelectedValue.ToString();
            else
                StoreID = AppLogic.AppConfigs("StoreID").ToString();
        }
    }
}