using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class CategoryPublish : BasePage
    {
        string srliveserver = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
            if (!IsPostBack)
            {
                GetCategoryList();
                btngo.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif";
                btnshowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
              //  btnUpdate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update.png";

            }
        }


        /// <summary>
        /// Show Admin Rights List
        /// </summary>
        private void GetCategoryList()
        {

            DataSet dsCategoryList = new DataSet();
            dsCategoryList = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(1), Convert.ToString("Name"), txtcategory.Text.Trim(), Convert.ToString(1));
            //dsCategoryList.Tables[0].DefaultView.Sort = "Name";
            DataView dv = dsCategoryList.Tables[0].DefaultView;
            dv.Sort = "Name ASC";
            dv.ToTable();
            if (dv.ToTable() != null && dv.ToTable().Rows.Count > 0)
            {
                chklcategory.Items.Clear();
                chklcategory.DataSource = dv.ToTable();
                chklcategory.DataTextField = "Name";
                chklcategory.DataValueField = "CategoryID";
                chklcategory.DataBind();
                chkdiv.Visible = true;
                btnUpdate.Visible = true;
                lblnoresult.Visible = false;
            }
            else
            {
                chklcategory.Items.Clear();
                chkdiv.Visible = false;
                btnUpdate.Visible = false;
                chklcategory.DataSource = null;
                chklcategory.DataBind();
                txtcategory.Text = null;
                lblnoresult.Visible = true;
            }
        }

        protected void btngo_Click(object sender, ImageClickEventArgs e)
        {
            GetCategoryList();
            txtcategory.Text = null;
        }

        protected void btnshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtcategory.Text = null;
            GetCategoryList();
        }

        protected void btnUpdate_Click(object sender, ImageClickEventArgs e)
        {
            string category = string.Empty;
            for (int cnt = 0; cnt < chklcategory.Items.Count; cnt++)
            {
                if (chklcategory.Items[cnt].Selected)
                {
                    try 
                    {
                        if(chklcategory.Items[cnt].Text.ToString().ToLower().IndexOf(" fabric") > -1)
                        {
                            var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + chklcategory.Items[cnt].Value.ToString().Trim() + "&catchupdate=1");
                            // Create a 'HttpWebRequest' object for the specified url. 
                            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                            // Set the user agent as if we were a web browser
                            myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                            var stream = myHttpWebResponse.GetResponseStream();

                            myHttpWebResponse.Close();
                        }
                        else if (chklcategory.Items[cnt].Text.ToString().ToLower().IndexOf("swatch") > -1)
                        {
                            var myUri = new Uri(srliveserver + "/swatch/category?catid=" + chklcategory.Items[cnt].Value.ToString().Trim() + "&catchupdate=1");
                            // Create a 'HttpWebRequest' object for the specified url. 
                            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                            // Set the user agent as if we were a web browser
                            myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                            var stream = myHttpWebResponse.GetResponseStream();

                            myHttpWebResponse.Close();
                        }
                        //else if (chklcategory.Items[cnt].Text.ToString().ToLower().IndexOf("home decor") > -1)
                        //{
                        //    var myUri = new Uri(srliveserver + "/homedecor/category?catid=" + chklcategory.Items[cnt].Value.ToString().Trim() + "&catchupdate=1");
                        //    // Create a 'HttpWebRequest' object for the specified url. 
                        //    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        //    // Set the user agent as if we were a web browser
                        //    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        //    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        //    var stream = myHttpWebResponse.GetResponseStream();

                        //    myHttpWebResponse.Close();
                        //}
                        else
                        {
                            var myUri = new Uri(srliveserver + "/category/category?catid=" + chklcategory.Items[cnt].Value.ToString().Trim() + "&catchupdate=1");
                            // Create a 'HttpWebRequest' object for the specified url. 
                            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                            // Set the user agent as if we were a web browser
                            myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                            var stream = myHttpWebResponse.GetResponseStream();

                            myHttpWebResponse.Close();
                        }
                        
                    }
                    catch
                    {

                    }
                    
                }
                //category += chklcategory.Items[cnt].Value.ToString().Trim() + ",";
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Category data published sucessfully.', 'Message');", true);
            chklcategory.ClearSelection();

            //if (category.Length > 1 && category.Contains(","))
            //    category = category.Substring(0, category.Length - 1);

        }

        protected void btnhomepagebnnaer_Click(object sender, EventArgs e)
        {
            try
            {
                CommonComponent.ExecuteCommonData("EXEC ExpireBannerdetails");


                var myUri1 = new Uri("http://www.halfpricedrapes.us/Admin/Settings/SmallHomePagebannerlist_MVC.aspx?autobanner=1");
                // Create a 'HttpWebRequest' object for the specified url. 
                var myHttpWebRequest1 = (HttpWebRequest)WebRequest.Create(myUri1);
                // Set the user agent as if we were a web browser
                myHttpWebRequest1.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                var myHttpWebResponse1 = (HttpWebResponse)myHttpWebRequest1.GetResponse();
                var stream1 = myHttpWebResponse1.GetResponseStream();

                myHttpWebResponse1.Close();

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
}