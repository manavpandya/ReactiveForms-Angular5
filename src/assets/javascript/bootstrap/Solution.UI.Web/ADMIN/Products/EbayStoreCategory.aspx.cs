using System;
 
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
 
using System.Xml;
using System.Text;
 
using System.IO;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using System.Net;
using System.Data;
using System.Data.Sql;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Solution.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
namespace Solution.UI.Web
{
    public partial class EbayStoreCategory : System.Web.UI.Page
    {
          protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //            string str = @"<?xml version='1.0' encoding='UTF-8'?>
            //<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
            //  <soapenv:Body>
            //    <SetStoreCategoriesResponse xmlns='urn:ebay:apis:eBLBaseComponents'>
            //      <Timestamp>2011-02-21T12:14:22.606Z</Timestamp>
            //      <Ack>Success</Ack>
            //      <CorrelationID>def6f689-0249-436c-8e7e-b594a0c84131</CorrelationID>
            //      <Version>707</Version>
            //      <Build>E707_CORE_BUNDLED_12695071_R1</Build>
            //      <TaskID>0</TaskID>
            //      <Status>Complete</Status>
            //      <CustomCategory>
            //        <CustomCategory>
            //          <CategoryID>411452819</CategoryID>
            //          <Name>TestCategory3</Name>
            //        </CustomCategory>
            //      </CustomCategory>
            //    </SetStoreCategoriesResponse>
            //  </soapenv:Body>
            //</soapenv:Envelope>";
           // str = str.Substring(str.IndexOf("<Status>") + 8, str.IndexOf("</Status>") - str.IndexOf("<Status>") - 8);

            LoadTree();
            //Setcategory();
        }
    }
    private void LoadTree()
    {
        GetStore();
    }
    private void GetStore()
    {
        //set your credentials for the call 

        GetStoreCall call = new GetStoreCall();

        call.ApiContext = GetContext();
        //get just the store categories
        call.CategoryStructureOnly = true;
        call.Execute();
        trvCategories.Nodes.Clear();
        TreeNode rootnode1 = new TreeNode();
        rootnode1.Text = "Root Category";
        rootnode1.Value = "0";
        trvCategories.Nodes.Add(rootnode1);
        //iterate through the top level categories
        foreach (StoreCustomCategoryType cat in call.Store.CustomCategories)
        {
            TreeNode rootnode = new TreeNode();
           
            rootnode.Value = cat.CategoryID.ToString();
           
            rootnode.Text = cat.Name;
             SQLAccess objsql = new  SQLAccess();
            DataSet dsCategory = new DataSet();
            dsCategory = objsql.GetDs("SELECT ID FROM tb_ebayStoreCategory WHERE ebayStoreCategoryID=" + cat.CategoryID.ToString() + " AND ParentStoreCategoryID=0");
            if (dsCategory.Tables[0].Rows.Count == 0)
            {
                objsql.ExecuteNonQuery("INSERT INTO tb_ebayStoreCategory(Name,ParentStoreCategoryID,ebayStoreCategoryID) VALUES ('" + cat.Name.ToString().Replace("'", "''") + "',0," + cat.CategoryID.ToString() + ")");
            }
            rootnode.ShowCheckBox = true;
            //rootnode.Value = cat.CategoryID;
            //rootnode.Checked = true;
            trvCategories.Nodes.Add(rootnode);
            GetChildCategories(cat, rootnode);
        }

    }
    private void GetChildCategories(StoreCustomCategoryType cat, TreeNode nd)
    {
        //get the category name, ID and whether it is a leaf


        //continue the recursion for each of the child categories
        foreach (StoreCustomCategoryType childcat in cat.ChildCategory)
        {
            long id = childcat.CategoryID;
            string name = childcat.Name;
            // bool leaf = (cat.ChildCategory.Count == 0);

            //Console.WriteLine("id = " + id + " name = " + name + " leaf= " + leaf);

            //condition to end the recursion
            //if (leaf)
            //{
            //    return;
            //}
            SQLAccess objsql = new SQLAccess();
            DataSet dsCategory = new DataSet();
            string strparentid = Convert.ToString(objsql.ExecuteScalarQuery("SELECT ID FROM tb_ebayStoreCategory WHERE ebayStoreCategoryID=" + cat.CategoryID.ToString() + ""));

            dsCategory = objsql.GetDs("SELECT ID FROM tb_ebayStoreCategory WHERE ebayStoreCategoryID=" + childcat.CategoryID.ToString() + " AND ParentStoreCategoryID =" + strparentid + "");
            if (dsCategory.Tables[0].Rows.Count == 0)
            {
                objsql.ExecuteNonQuery("INSERT INTO tb_ebayStoreCategory(Name,ParentStoreCategoryID,ebayStoreCategoryID) VALUES ('" + childcat.Name.ToString().Replace("'", "''") + "'," + strparentid.ToString() + "," + childcat.CategoryID.ToString() + ")");
            }
            TreeNode tnchild = new TreeNode();
            tnchild.Text = name;
            tnchild.Value = id.ToString();
            tnchild.ShowCheckBox = true;
            //tnchild.ToolTip = rootid;
            nd.ChildNodes.Add(tnchild);
            GetChildCategories(childcat, nd);
        }
    }
    public ApiContext GetContext()
    {

        ApiContext contexttemp = new ApiContext();

        //if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfig("UseEbaySandBox").ToString())))
        ////if (Convert.ToBoolean(true))
        //{
        //    // credentials for the call
        //    contexttemp.ApiCredential.ApiAccount.Developer = AppLogic.AppConfig("ebaySandboxDevKey");
        //    contexttemp.ApiCredential.ApiAccount.Application = AppLogic.AppConfig("ebaySandboxAppKey");
        //    contexttemp.ApiCredential.ApiAccount.Certificate = AppLogic.AppConfig("ebaySandboxCertiKey");
        //    contexttemp.ApiCredential.eBayToken = AppLogic.AppConfig("ebaySandboxToken");
        //    // set the url
        //    contexttemp.SoapApiServerUrl = AppLogic.AppConfig("ebaySandboxServerUrl");
        //    //contexttemp.Version = "485";
        //    //contexttemp.ApiCredential.eBayAccount.UserName = "TESTUSER_Kaushalam";
        //    //contexttemp.ApiCredential.eBayAccount.Password = "n$r7xRu";
        //}
        //else
        //{
            //set the productin keys and token
            contexttemp.ApiCredential.ApiAccount.Developer = AppLogic.AppConfigs("ebayDevKey"); ;
            contexttemp.ApiCredential.ApiAccount.Application = AppLogic.AppConfigs("ebayAppKey");
            contexttemp.ApiCredential.ApiAccount.Certificate = AppLogic.AppConfigs("ebayCertiKey");
            contexttemp.ApiCredential.eBayToken = AppLogic.AppConfigs("ebayToken");
            //set the server url
            contexttemp.SoapApiServerUrl = AppLogic.AppConfigs("ebayServerUrl");


        //}
        return contexttemp;
    }
    public void Setcategory()
    {
        /****Start*****/


        foreach (TreeNode tn in trvCategories.Nodes)
        {
            if (tn.Checked == true)
            {

                SetStoreCategoriesCall setcategoryCall = new SetStoreCategoriesCall();

                setcategoryCall.ApiContext = GetContext();

                StoreCustomCategoryType objCustom = new StoreCustomCategoryType();
                objCustom.Name = "TestCategory3"; //Getting from textbox
                //objCustom.CategoryID = 8463820;
                objCustom.Order = 1;
                StoreCustomCategoryTypeCollection objCustomCollection = new StoreCustomCategoryTypeCollection();

                objCustomCollection.Add(objCustom);
                //setcategoryCall.Version = "485";
                long categoryid = Convert.ToInt32(tn.Value.ToString());
                setcategoryCall.DestinationParentCategoryID = categoryid;
                setcategoryCall.ItemDestinationCategoryID = 0;
                setcategoryCall.Version = "455"; // 707
                setcategoryCall.Action = StoreCategoryUpdateActionCodeType.Add;

                long cateid = setcategoryCall.SetStoreCategories(StoreCategoryUpdateActionCodeType.Add, 0, categoryid, objCustomCollection);
                string strResponse = setcategoryCall.SoapResponse.ToString();
                string categoryId = "";
                try
                {
                    string strStatus = strResponse.Substring(strResponse.IndexOf("<Status>") + 8, strResponse.IndexOf("</Status>") - strResponse.IndexOf("<Status>") - 8);
                    if (strStatus.ToLower() == "complete")
                    {
                        categoryId = strResponse.Substring(strResponse.IndexOf("<CategoryID>") + 12, strResponse.IndexOf("</CategoryID>") - strResponse.IndexOf("<CategoryID>") - 12);

                        SQLAccess objsql = new  SQLAccess();
                        DataSet dsCategory = new DataSet();
                        int strparentid = Convert.ToInt32(objsql.ExecuteScalarQuery("SELECT ID FROM tb_ebayStoreCategory WHERE ebayStoreCategoryID=" + categoryid.ToString() + ""));
                        dsCategory = objsql.GetDs("SELECT ID FROM tb_ebayStoreCategory WHERE ebayStoreCategoryID=" + categoryId.ToString() + " AND ParentStoreCategoryID =" + strparentid + "");
                        if (dsCategory.Tables[0].Rows.Count == 0)
                        {
                            objsql.ExecuteNonQuery("INSERT INTO tb_ebayStoreCategory(Name,ParentStoreCategoryID,ebayStoreCategoryID) VALUES ('" + objCustom.Name.ToString().Replace("'", "''") + "'," + strparentid + "," + categoryId.ToString() + ")");
                        }
                    }

                }
                catch
                {
                }
                if (tn.Value == "0")
                {
                    break;
                }


            }

        }


    }
    protected void btnAddCate_Click(object sender, EventArgs e)
    {
        Setcategory();
    }
    }
}