using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using Solution.Data;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class InsertEbaycategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text.ToString().Trim() != "")
            {
              
                GetCategoryAll();
            }
        }
        private void GetEbaycategory(CategoryTypeCollection objCaolection, string parentid)
        {
            foreach (CategoryType catty in objCaolection)
            {
                for (int i = 0; i < catty.CategoryParentID.Count; i++)
                {
                    if (catty.CategoryParentID[i].ToString() == parentid && catty.CategoryID.ToString() != parentid)
                    {
                         SQLAccess objsql = new  SQLAccess();
                        DataSet dsCategory = new DataSet();
                        string strparentid = Convert.ToString(objsql.ExecuteScalarQuery("SELECT ID FROM tb_ebayCategory WHERE ebayCategoryID=" + catty.CategoryID.ToString() + ""));
                        if (!string.IsNullOrEmpty(strparentid))
                        {
                            dsCategory = objsql.GetDs("SELECT ID FROM tb_ebayCategory WHERE ebayCategoryID=" + catty.CategoryID.ToString() + " AND ParentCategoryID =" + parentid + "");
                        }

                        if (dsCategory == null || dsCategory.Tables.Count == 0 || dsCategory.Tables[0].Rows.Count == 0)
                        {
                            objsql.ExecuteNonQuery("INSERT INTO tb_ebayCategory(Name,ParentCategoryID,ebayCategoryID) VALUES ('" + catty.CategoryName.ToString().Replace("'", "''") + "'," + parentid.ToString() + "," + catty.CategoryID.ToString() + ")");
                        }
                        if (catty.CategoryID.ToString() != parentid)
                        {
                            GetEbaycategory(objCaolection, catty.CategoryID.ToString());
                        }
                    }
                }
            }
        }
        public ApiContext GetContext()
        {

            ApiContext contexttemp = new ApiContext();

            //if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())))
            ////if (Convert.ToBoolean(true))
            //{
            //    // credentials for the call
            //    contexttemp.ApiCredential.ApiAccount.Developer = //AppLogic.AppConfigs("ebaySandboxDevKey");
            //    contexttemp.ApiCredential.ApiAccount.Application = //AppLogic.AppConfig("ebaySandboxAppKey");
            //    contexttemp.ApiCredential.ApiAccount.Certificate = //AppLogic.AppConfig("ebaySandboxCertiKey");
            //    contexttemp.ApiCredential.eBayToken = //AppLogic.AppConfig("ebaySandboxToken");
            //    // set the url
            //    contexttemp.SoapApiServerUrl = //AppLogic.AppConfig("ebaySandboxServerUrl");
            //    //contexttemp.Version = "485";
            //    //contexttemp.ApiCredential.eBayAccount.UserName = "TESTUSER_Kaushalam";
            //    //contexttemp.ApiCredential.eBayAccount.Password = "n$r7xRu";
            //}
            //else
            {
                //set the productin keys and token
                contexttemp.ApiCredential.ApiAccount.Developer = AppLogic.AppConfigs("ebayDevKey"); //"5a00a3e5-7a91-4b58-9316-b8296fbde684";//
                contexttemp.ApiCredential.ApiAccount.Application = AppLogic.AppConfigs("ebayAppKey");//"Kaushala-f941-4f25-b4e2-0c3e482c1ad5"; //
                contexttemp.ApiCredential.ApiAccount.Certificate = AppLogic.AppConfigs("ebayCertiKey");//"Kaushala-f941-4f25-b4e2-0c3e482c1ad5"; 
                contexttemp.ApiCredential.eBayToken = AppLogic.AppConfigs("ebayToken");
                //set the server url
                contexttemp.SoapApiServerUrl = AppLogic.AppConfigs("ebayServerUrl");//"https://api.ebay.com/wsapi"; //


            }
            return contexttemp;
        }
        private void GetCategoryAll()
        {
            ApiContext apiContext = GetContext();
            GetCategoriesCall apicall = new GetCategoriesCall(apiContext);
            StringCollection strId = new StringCollection();
            strId.Add(TextBox1.Text.ToString().Trim());

            apicall.CategoryParent = strId;
            apicall.CategorySiteID = "0";

            apicall.DetailLevelList.Add(eBay.Service.Core.Soap.DetailLevelCodeType.ReturnAll);
            apicall.ViewAllNodes = true;
            // apicall.LevelLimit = 4;
            CategoryTypeCollection newCategoryCollection = new CategoryTypeCollection();
            newCategoryCollection = apicall.GetCategories();
            foreach (CategoryType catty in newCategoryCollection)
            {
                 SQLAccess objsqlEbay = new  SQLAccess();
                DataSet dsCategory = new DataSet();
                dsCategory = objsqlEbay.GetDs("SELECT ID FROM tb_ebayCategory WHERE ebayCategoryID=" + catty.CategoryID.ToString() + "");
                if (dsCategory.Tables[0].Rows.Count == 0)
                {
                    objsqlEbay.ExecuteNonQuery("INSERT INTO tb_ebayCategory(Name,ParentCategoryID,ebayCategoryID) VALUES ('" + catty.CategoryName.ToString().Replace("'", "''") + "',0," + catty.CategoryID.ToString() + ")");
                }

                GetEbaycategory(newCategoryCollection, catty.CategoryID.ToString());
            }
        }
        
    }
}