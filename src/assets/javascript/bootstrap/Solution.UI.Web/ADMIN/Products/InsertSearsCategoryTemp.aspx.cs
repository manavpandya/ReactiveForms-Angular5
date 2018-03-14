using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class InsertSearsCategoryTemp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            InsertCategoryLI(txtHTML.Text.ToString(), 0);
        }
        private void InsertCategoryLI(string StrHTmll, Int32 Categoryid)
        {
            string str = StrHTmll.ToString();
            str = str.Replace(System.Environment.NewLine, "");
            string[] strli = System.Text.RegularExpressions.Regex.Split(str, "<li", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            int parentid = 0;
            int mainid = 0;
            int ii = 0;
            string strName1 = "";
            string strIds = "0";
            int j1 = 0;
            int j2 = 0;
            foreach (string strName in strli)
            {

                if (strName.IndexOf("class=\"\">") > -1 || strName.IndexOf("<a href=\"#\">") > -1 || strName.IndexOf("<a class=\"\"") > -1)
                {
                    if (strName.IndexOf("<ul") > -1)
                    {

                        //if (j1 == j2)
                        //{
                        //    mainid = Convert.ToInt32(strIds);


                        //}
                        //else
                        //{
                        //    j1 = 0;
                        //    j2 = 0;
                        //}

                        //j2++;

                    }
                    if (strName.IndexOf("class=\"\">") > -1)
                    {
                        strName1 = strName.Substring(strName.IndexOf("class=\"\">") + 9, strName.IndexOf("</a>") - strName.IndexOf("class=\"\">") - 9);
                    }
                    else if (strName.IndexOf("<a href=\"#\">") > -1)
                    {
                        strName1 = strName.Substring(strName.IndexOf("href=\"#\">") + 9, strName.IndexOf("</a>") - strName.IndexOf("href=\"#\">") - 9);
                    }
                    else if (strName.IndexOf("<a class=\"\"") > -1)
                    {
                        strName1 = strName.Substring(strName.IndexOf("href=\"#\">") + 9, strName.IndexOf("</a>") - strName.IndexOf("href=\"#\">") - 9);
                    }
                    strIds = strName.Substring(strName.IndexOf("id=\"") + 4, strName.IndexOf("\">") - strName.IndexOf("id=\"") - 4);
                    if (strIds.Length > 20)
                    {
                        strIds = strIds.Replace("node_", "");
                        strIds = strIds.Substring(0, strIds.IndexOf("\""));
                    }
                    else
                    {
                        strIds = strIds.Replace("node_", "");
                    }
                    //if (ii == 0)
                    //{
                    //    mainid = Convert.ToInt32(strIds);
                    //}
                    // string idss = strName.Substring(strName.IndexOf("taxonomy_models_item_class_") + 27, strName.IndexOf("\"") - strName.IndexOf("taxonomy_models_item_class_") - 27);


                    SQLAccess objS = new  SQLAccess();
                    Int32 cat = Convert.ToInt32(objS.ExecuteScalarQuery("INSERT INTO tb_Category(StoreID,Name,Active,Deleted,SearsCategoryID) VALUES (" + txtstoreid.Text.ToString() + ",'" + strName1 + "',1,0," + strIds + "); SELECT SCOPE_IDENTITY();"));

                    Int32 parentid44 = Convert.ToInt32(objS.ExecuteScalarQuery("SELECT isnull(categoryid,0) FROM  tb_Category WHERE SearsCategoryID =" + parentid + ""));
                    objS.ExecuteNonQuery("INSERT INTO tb_CategoryMapping(CategoryID,ParentCategoryID) VALUES (" + cat + "," + parentid44 + ")");
                    j1++;
                    if (strName1.ToString().ToLower() == "printers")
                    {

                    }
                    if (strName.IndexOf("<ul") > -1)
                    {
                        parentid = Convert.ToInt32(strIds);

                    }
                    else
                    {
                        if (strName.IndexOf("jstree-last") > -1 && (strName.IndexOf("jstree-leaf\"") > -1 || strName.IndexOf("taxonomy_models_item_class_" + strIds.ToString() + "\"") > -1))
                        {
                            string strProductid = "";

                            string[] strli1 = System.Text.RegularExpressions.Regex.Split(strName, "</ul>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            Int32 idsdd = 0;
                            if (strli1.Length == 2)
                            {
                                idsdd = Convert.ToInt32(objS.ExecuteScalarQuery("SELECT isnull(SearsCategoryID,0) FROM tb_Category WHERE Categoryid in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE  categoryid in (SELECT isnull(categoryid,0) FROM  tb_Category WHERE SearsCategoryID =" + parentid + "))"));
                                if (idsdd != 0)
                                {
                                    parentid = idsdd;
                                }
                            }
                            else if (strli1.Length >= 3)
                            {
                                strProductid = Convert.ToString(objS.ExecuteScalarQuery("SELECT isnull(SearsCategoryID,0) FROM tb_Category WHERE CategoryId in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE Categoryid in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE  categoryid in (SELECT isnull(categoryid,0) FROM  tb_Category WHERE SearsCategoryID =" + parentid + ")))"));
                                if (strProductid == "" || strProductid == "0")
                                {

                                    idsdd = Convert.ToInt32(objS.ExecuteScalarQuery("SELECT isnull(SearsCategoryID,0) FROM tb_Category WHERE Categoryid in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE  categoryid in (SELECT isnull(categoryid,0) FROM  tb_Category WHERE SearsCategoryID =" + parentid + "))"));
                                    if (idsdd != 0)
                                    {
                                        parentid = idsdd;
                                    }
                                }
                                else
                                {
                                    parentid = Convert.ToInt32(strProductid);

                                }
                            }


                        }
                        else if (strName.IndexOf("jstree-last") > -1)
                        {
                            parentid = Convert.ToInt32(objS.ExecuteScalarQuery("SELECT SearsCategoryID FROM tb_Category WHERE Categoryid in (SELECT ParentCategoryID FROM tb_CategoryMapping WHERE  categoryid in (SELECT isnull(categoryid,0) FROM  tb_Category WHERE SearsCategoryID =" + parentid + "))"));

                        }
                        else if (strName.IndexOf("jstree-leaf") > -1)
                        {

                        }
                        //else if (strName.IndexOf("jstree-last") > -1)
                        //{
                        //    parentid = mainid;
                        //}
                        else
                        {
                            parentid = mainid;
                        }
                    }


                    //if (strName.IndexOf("<ul") > -1)
                    //{
                    //    GetCategoryname(strName, Convert.ToInt32(cat));
                    //}
                    ii++;
                }

            }
        }
        private void GetCategoryname(string strHTML, Int32 categoryId)
        {
            string[] strUL = System.Text.RegularExpressions.Regex.Split(strHTML, "<ul", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            foreach (string str in strUL)
            {
                if (str.IndexOf("<li") > -1)
                {
                    InsertCategoryLI(str, categoryId);
                }
            }
        }
    }
}