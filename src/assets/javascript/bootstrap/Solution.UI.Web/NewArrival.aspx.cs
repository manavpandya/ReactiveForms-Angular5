using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Solution.UI.Web
{
    public partial class NewArrival : System.Web.UI.Page
    {

        #region Declaration

    
        public string fullPath = string.Empty;
        string strView = "";
        Int32 itemCount = 6;
        Int32 TotalCount = 6;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(false);
            fullPath = Server.UrlDecode(Request.RawUrl.ToLower());
            string AppPath = Request.Url.Scheme + "://" + Request.Url.Authority;
            if (AppLogic.AppConfigBool("UseLiveRewritePath"))
                AppPath += ":" + Request.Url.Port + Request.ApplicationPath.ToLower();
            else
                AppPath += Request.ApplicationPath.ToLower();

            if (fullPath.Contains(AppPath))
            {
                int IndexPath = fullPath.LastIndexOf(AppPath);
                fullPath = fullPath.Substring(IndexPath + AppPath.Length);
            }

            fullPath = "/" + fullPath.Trim("/".ToCharArray());

            if (fullPath.Contains("*")) //hr
                fullPath = fullPath.Substring(0, fullPath.IndexOf("*"));
         
         
            if (!IsPostBack)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(AppLogic.AppConfigs("Live_Contant_Server_Path") + "/images/NewArrivalBanner/");
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            if (File.Exists(strfl))
                            {
                                FileInfo fl = new FileInfo(strfl);
                                imgBanner.Src = AppLogic.AppConfigs("Live_Contant_Server") + "/images/NewArrivalBanner/" + fl.Name.ToString();
                                break;
                            }
                        }
                    }
                }
                catch { }

                BindData();

                //if (ViewState["strFeaturedimg"] != null)
                //{

                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
                //}
                //if (ViewState["strFeaturedimglist"] != null)
                //{

                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);

                //}
            }
            else
            {
                //if (ViewState["strFeaturedimgpostback"] != null)
                //{

                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderpost", ViewState["strFeaturedimgpostback"].ToString(), true);
                //}
                //if (ViewState["strFeaturedimglistpostback"] != null)
                //{

                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlistpost", ViewState["strFeaturedimglistpostback"].ToString(), true);
                //}
            }
           
        }

        /// <summary>
        /// Method to bind Repeater with New Arrival Product
        /// </summary>
        private void BindData() 
        {
            DataSet dsNewArrival = CommonComponent.GetCommonDataSet("EXEC usp_Product_NewArrival 1"); //ProductComponent.GetAllNewArrivalProduct(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsNewArrival != null && dsNewArrival.Tables.Count > 0 && dsNewArrival.Tables[0].Rows.Count > 0)
            {
                dvMessage.Visible = false;
                topMiddle.Visible = true;

                TotalCount = Convert.ToInt32(dsNewArrival.Tables[0].Rows.Count);

                RptNewArrivalProduct.DataSource = dsNewArrival;
                RptNewArrivalProduct.DataBind();
              
               
            }
            else
            {
                if (dsNewArrival != null && dsNewArrival.Tables.Count > 0 && dsNewArrival.Tables[0].Rows.Count > 0)
                {
                    dvMessage.Visible = false;
                }
                else
                {
                    dvMessage.Visible = true;
                }

                topMiddle.Visible = false;

            }
        }

        /// <summary>
        /// New Arrival Product Item Data Bound to display 3 Record in each row
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void RptNewArrivalProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");

                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                HtmlImage outofStockDiv = (HtmlImage)e.Item.FindControl("outofStockDiv");
                HtmlAnchor innerAddtoCart = (HtmlAnchor)e.Item.FindControl("innerAddtoCart");
                HtmlImage imgAddToCart = (HtmlImage)e.Item.FindControl("imgAddToCart");
                Image imgName = (Image)e.Item.FindControl("imgName");

                Literal ltbottom = (Literal)e.Item.FindControl("ltbottom");
              
                #region Bind rating
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                DataSet dsComment = new DataSet();
                dsComment = ProductComponent.GetProductRating(Convert.ToInt32(ltrproductid.Text));
                Literal ltreviewDetail = (Literal)e.Item.FindControl("ltrating");
                Label ltrRatingCount = (Label)e.Item.FindControl("ltrRatingCount");

                Decimal rating = 0;
                HtmlGenericControl DivratingNew = (HtmlGenericControl)e.Item.FindControl("Divratinglist");
                HtmlGenericControl divSpace = (HtmlGenericControl)e.Item.FindControl("divSpace");


                decimal dd = 0;
                decimal ddnew = 0;

                if (dsComment != null && dsComment.Tables[0].Rows.Count > 0)
                {

                    rating = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]);
                    ltrRatingCount.Text = dsComment.Tables[0].Rows[0]["AvgRating"].ToString();
                    DivratingNew.Visible = true;
                    divSpace.Visible = false;
                    dd = Math.Truncate(Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]));
                    ddnew = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]) - dd;
                }
                else
                {
                    DivratingNew.Visible = false;
                    divSpace.Visible = true;
                }


                if (dd == Convert.ToDecimal(1))
                {
                   
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }
                    ltreviewDetail.Text += "<img src=\"/images/star-form1.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(2))
                {
                   
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }

                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(3))
                {
                  
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }


                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(4))
                {
                
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }
                }
                else if (dd == Convert.ToDecimal(5))
                {
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                }
                else
                {
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                }
                ltreviewDetail.Text += "";


                #endregion




                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                        {
                            lblTag.Text = "<img  title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "' />";
                        }
                    }
                }

                if ((e.Item.ItemIndex + 1) % itemCount == 0 && e.Item.ItemIndex != 0)
                {

                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                    itemCount = itemCount + 5;
                }
                else
                {
                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                }

                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");
                HtmlInputHidden ltrProductURL = (HtmlInputHidden)e.Item.FindControl("ltrProductURL");
                HtmlInputHidden hdnItemIndex = (HtmlInputHidden)e.Item.FindControl("hdnItemIndex");
                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");

                HtmlAnchor aProductlink = (HtmlAnchor)e.Item.FindControl("aProductlink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;
                Label lblPrice = (Label)e.Item.FindControl("lblPrice");

              

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);
                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";// "<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else ltrRegularPrice.Text += "<span>&nbsp;</span>";
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Starting Price: <span>" + SalePrice.ToString("C") + "</span>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";

                if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                {
                    
                    DataSet dsVariant = new DataSet();
                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantID FROM tb_ProductVariant WHERE ProductID=" + ltrproductid.Text.ToString() + "");
                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                    {
                        aProductlink.Visible = true;
                        aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                    }
                    else
                    {

                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        aProductlink.HRef = "/" + ltrProductURL.Value.ToString();

                        imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                    }
                }
                else
                {
                    aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                    aProductlink.Attributes.Remove("onclick");
                    aProductlink.Visible = true;
                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                  
                    //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                    imgAddToCart.Visible = false;
                    outofStockDiv.Visible = false;

                }

             
             
            }
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Icon Image Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

     
        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 65)
                Name = Name.Substring(0, 62) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Replace the '"' and '\' which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace("'", "&#39;").Replace('"', '-').Replace('\'', '-').ToString();
        }


        #region Remove ViewState From page
        /// <summary>
        /// Remove ViewState From page.
        /// </summary>
        /// <returns></returns>
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

     

    }
}