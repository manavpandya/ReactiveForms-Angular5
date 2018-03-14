using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web
{
    public partial class Sitemap : System.Web.UI.Page
    {

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        /// 
        DataSet dsproduct = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            ltTitle.Text = "Sitemap";
            ltbrTitle.Text = "Sitemap";
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("StoreId")))
                {
                    Int32 Storeid = Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString());
                    dsproduct = CommonComponent.GetCommonDataSet("SELECT ProductID as Id,'/'+ProductURL as link,Name FROM tb_product WHERE  Storeid=1 and isnull(Active,0)=1 and isnull(deleted,0)=0 and ProductID in(select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))");
                    BindSearch(Storeid);

                }
            }
        }

        /// <summary>
        /// Binds the Search Details By StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindSearch(Int32 StoreID)
        {
            string StrSiteMap = string.Empty;
            DataSet dsCategory = new DataSet();
            dsCategory = CategoryComponent.GetCategoryByStoreID(StoreID, 3);
            StrSiteMap = "<ul>";
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                string StrChildValu = "";
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=0"))
                {
                    StrSiteMap += "<li ><a title='" + dr["Name"].ToString().Trim() + "' href='/" + dr["SEName"].ToString().ToLower() + ".html'>" + dr["Name"].ToString().Trim() + "</a>";
                    String strIds = Convert.ToString(CommonComponent.GetScalarCommonData("Select cast(ProductID as nvarchar(max))+',' from tb_ProductCategory where CategoryID =" + dr["CategoryID"].ToString() + " FOR XML PATH('')"));

                    if (strIds.Length > 1)
                    {
                        strIds = strIds + "0";
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drProduct = dsproduct.Tables[0].Select("ID in (" + strIds + ")");
                            if (drProduct.Length > 0)
                            {
                                StrSiteMap += "<ul>";
                                foreach (DataRow drtemp in drProduct)
                                {
                                    StrSiteMap += "<li><a title='" + Server.HtmlEncode(drtemp["Name"].ToString().Trim()) + "' href=\"" + drtemp["Link"].ToString().ToLower() + "\">" + drtemp["Name"].ToString().Trim() + "</a>";
                                    StrSiteMap += "</li>";
                                }
                                StrSiteMap += "</ul>";
                            }
                        }
                    }


                    StrChildValu = Convert.ToString(WriteSubCategory(dr["CategoryID"].ToString().Trim(), dr["SEName"].ToString(), dsCategory, ""));
                   // StrChildValu = Convert.ToString(WriteSubCategory("1720", dr["SEName"].ToString(), dsCategory, ""));
                    if (!string.IsNullOrEmpty(StrChildValu.ToString()))
                        StrSiteMap += StrChildValu;

                    StrSiteMap += "</li>";
                }
            }

            #region GetTopic for Site Map
            DataSet dsTopic = new DataSet();
            dsTopic = CommonComponent.GetCommonDataSet("SElect * from tb_topic Where Deleted=0 and StoreId=" + StoreID + " And ISNULL(ShowOnSiteMap,0)=1");
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTopic.Tables[0].Rows)
                {
                    StrSiteMap += "<li ><a title='" + dr["Title"].ToString().Trim() + "' href=\"" + dr["TopicName"].ToString().ToLower() + ".html\">" + dr["Title"].ToString().Trim() + "</a></li>";



                }
            }
            #endregion
            StrSiteMap += "</ul>";

            if (!string.IsNullOrEmpty(StrSiteMap.ToString()))
                ltrSiteMap.Text = StrSiteMap.ToString();
        }

        /// <summary>
        /// Writes the Sub Category
        /// </summary>
        /// <param name="CatID">String CatID</param>
        /// <param name="CatName">String CatName</param>
        /// <param name="dsCategory">DatSet dsCategory</param>
        /// <returns>Returns the Sub Category Data as a HTML String</returns>
        private string WriteSubCategory(String CatID, String CatName, DataSet dsCategory, string strchild)
        {
            string StrChild = strchild;
            if (!string.IsNullOrEmpty(CatName))
                CatName += "/";
            string ChildCatName = string.Empty;
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0 && dsCategory.Tables[0].Select("ParentCategoryID=" + CatID).Length > 0)
            {
                StrChild += "<ul>";
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=" + CatID))
                {
                    ChildCatName = dr["SEName"].ToString();
                    StrChild += "<li ><a title='" + dr["Name"].ToString().Trim() + "' href=\"" + ChildCatName + ".html\">" + dr["Name"].ToString().Trim() + "</a>";


                    String strIds = Convert.ToString(CommonComponent.GetScalarCommonData("Select cast(ProductID as nvarchar(max))+',' from tb_ProductCategory where CategoryID =" + dr["CategoryID"].ToString() + " FOR XML PATH('')"));

                    if (strIds.Length > 1)
                    {
                        strIds = strIds + "0";
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drProduct = dsproduct.Tables[0].Select("ID in (" + strIds + ")");
                            if (drProduct.Length > 0)
                            {
                                StrChild += "<ul>";
                                foreach (DataRow drtemp in drProduct)
                                {
                                    StrChild += "<li><a title='" + Server.HtmlEncode(drtemp["Name"].ToString().Trim()) + "' href=\"" + drtemp["Link"].ToString().ToLower() + "\">" + drtemp["Name"].ToString().Trim() + "</a>";
                                    StrChild += "</li>";
                                }
                                StrChild += "</ul>";
                            }
                        }
                    }
                    StrChild = WriteSubCategory(dr["CategoryID"].ToString(), dr["Name"].ToString(), dsCategory, StrChild);
                    StrChild += "</li>";
                }
                StrChild += "</ul>";
            }
           
            return StrChild.ToString();
        }
     

    }
}