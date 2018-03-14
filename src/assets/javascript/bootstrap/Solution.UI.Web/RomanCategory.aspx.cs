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
    public partial class RomanCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/RomanCategoryBanner/"));
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            if (File.Exists(strfl))
                            {
                                FileInfo fl = new FileInfo(strfl);
                              //  imgBanner.Src = "/images/RomanCategoryBanner/" + fl.Name.ToString();
                                break;
                            }
                        }
                    }
                }
                catch { }
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
                BindSubCategoryofMainCategory();
                breadcrumbs();
            }

        }

        private void BindSubCategoryofMainCategory()
        {
            // DataSet  dsProduct = ProductComponent.GetProductDetailsByOrderPrice(Convert.ToInt32(AppLogic.AppConfigs("RomanCategoryID")), Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "Low to High");
            DataSet dsProduct = CommonComponent.GetCommonDataSet("select isnull(p.description,'') as description, p.ProductID,p.Name,p.ImageName,isnull(p.ProductURL,'') as ProductURL,p.Price,(Case When (p.SalePrice Is Not Null And p.SalePrice!=0) Then p.SalePrice Else p.Price End) As SalePrice,p.DisplayOrder " +
                                                                    " ,p.MainCategory,p.SEName,isnull(p.Inventory,0) as Inventory, " +
                                                                    " isnull(IsFreeEngraving,0) as IsFreeEngraving, p.TagName,case when isnull(p.Tooltip,'')='' then p.Name else p.Tooltip end as Tooltip,isnull(p.OptionalAccessories,'') as OptionalAccessories   from tb_Product p inner join " +
                                                                    " tb_ProductCategory pc on p.ProductID=pc.ProductID where p.StoreID=cast(" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " as varchar(50)) and pc.CategoryID=cast(" + Request.QueryString["CatID"].ToString() + " as varchar(50)) and isnull(Active,0) = 1 and isnull(Deleted,0) = 0 and ItemType='Roman'  and isnull(IsRoman,0) = 1   order by p.DisplayOrder");

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                RepRomanCategory.DataSource = dsProduct;
            }
            else
            {
                RepRomanCategory.DataSource = null;
            }
            RepRomanCategory.DataBind();
        }

        protected void RepRomanCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
                HtmlGenericControl catDisplay = (HtmlGenericControl)e.Item.FindControl("catDisplay");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                Literal ColorOption = (Literal)e.Item.FindControl("ColorOption");
                Label lblProductID = (Label)e.Item.FindControl("lblProductID");

                HtmlGenericControl divVariantOption = (HtmlGenericControl)e.Item.FindControl("divVariantOption");
                HtmlGenericControl divWithoutVariantOption = (HtmlGenericControl)e.Item.FindControl("divWithoutVariantOption");
                HtmlGenericControl ColorMessage = (HtmlGenericControl)e.Item.FindControl("ColorMessage");
                HtmlGenericControl ViewMoreOption = (HtmlGenericControl)e.Item.FindControl("ViewMoreOption");
                HtmlGenericControl divForSinglecolorLine = (HtmlGenericControl)e.Item.FindControl("divForSinglecolorLine");

                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                Literal ltshiprange = (Literal)e.Item.FindControl("ltshiprange");


                if (Request.RawUrl != null && Request.RawUrl.ToString().ToLower().IndexOf("cellular") > -1)
                {
                    ltshiprange.Text = "Made to Measure.Ship in 3-5 Business Days.";
                }

                //if ((e.Item.ItemIndex + 1) % 2== 0 && e.Item.ItemIndex != 0)
                if ((e.Item.ItemIndex) % 2 == 0 && e.Item.ItemIndex != 0)
                {
                    //Catbox.Attributes.Add("style", "margin-right:0px;");
                    litControl.Text = "</div> <div class=\"shades-cat-row1\">";
                }

                RepeaterItem row = e.Item;
                if (row.ItemIndex <= 1)
                {
                    if (row.ItemIndex % 2 == 0)
                    {
                        Catbox.Attributes.Add("class", "shades-cat-pt1");
                        if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                        {
                            string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                            if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                            {
                                if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                                {
                                    lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "RomanLeft' />";
                                }
                            }
                        }
                    }
                    if (row.ItemIndex % 2 == 1)
                    {
                        Catbox.Attributes.Add("class", "shades-cat-pt2");
                        if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                        {
                            string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                            if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                            {
                                if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                                {
                                    lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "RomanRight' />";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (row.ItemIndex % 2 == 0)
                    {
                        Catbox.Attributes.Add("class", "shades-cat-pt1");
                        if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                        {
                            string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                            if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                            {
                                if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                                {
                                    lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "RomanLeft' />";
                                }
                            }
                        }
                    }
                    if (row.ItemIndex % 2 == 1)
                    {
                        Catbox.Attributes.Add("class", "shades-cat-pt2");
                        if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                        {
                            string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                            if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                            {
                                if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                                {
                                    lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "RomanRight' />";
                                }
                            }
                        }
                    }
                }


                String strPath = AppLogic.AppConfigs("ImagePathProductVariant");
                DataSet dscolorOption = CommonComponent.GetCommonDataSet("select p.ProductURL,pvv.VariantValueID,pvv.VariantID, pvv.VariantValue, pvv.VariImageName,pv.VariantName from tb_ProductVariantValue pvv inner join tb_ProductVariant pv on pv.VariantID = pvv.VariantID " +
                                                        " inner join tb_Product p on p.ProductID = pv.ProductID where pv.VariantName like '%color%' and pvv.VariImageName is not null and p.ProductID = " + lblProductID.Text.ToString() + "");
                if (dscolorOption != null && dscolorOption.Tables.Count > 0 && dscolorOption.Tables[0].Rows.Count > 0)
                {
                    Int32 iCheck = 0;

                    for (int i = 0; i < dscolorOption.Tables[0].Rows.Count; i++)
                    {
                        if (iCheck == 10)
                        {
                            break;
                        }
                        string strImagePath = strPath.Trim() + "/color/" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariImageName"]);
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + strImagePath.Trim()))
                        {
                            iCheck++;
                            ViewMoreOption.Visible = true;
                            ColorMessage.Visible = true;
                            divVariantOption.Visible = true;
                            divWithoutVariantOption.Visible = false;
                            //////ColorOption.Text += "<a href='javascript:void(0);'  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"border;solid 1px #dddddd;width:33px;height:33px;\"></a>";
                            ////ColorOption.Text += "<a onclick=\"variantlink('" + dscolorOption.Tables[0].Rows[i]["ProductURL"].ToString() + "','" + dscolorOption.Tables[0].Rows[i]["VariantValueID"].ToString() + "');\" href=\"javascript:void(0);\"  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"border;solid 1px #dddddd;width:33px;height:33px;\"></a>";
                            //ColorOption.Text += "<a  href=\"/Roman-ItemPage.aspx?" + dscolorOption.Tables[0].Rows[i]["VariantName"].ToString() + "=" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "\"  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"width:33px;height:33px;\"></a>";
                            if (i <= 0)
                            {
                                divForSinglecolorLine.Visible = true;
                                divVariantOption.Attributes.Add("style", "min-height:38px !important;float: left;");
                                //ColorOption.Text += "<a href='javascript:void(0);'  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"border;solid 1px #dddddd;width:33px;height:33px;\"></a>";
                                ColorOption.Text += "<a href=\"javascript:void(0);\" onclick=\"variantlink('" + dscolorOption.Tables[0].Rows[i]["ProductURL"].ToString() + "','" + dscolorOption.Tables[0].Rows[i]["VariantValueID"].ToString() + "');\"  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"width:33px;height:33px;border:solid 1px #d7d7d7;\"></a>";

                            }
                            else
                            {
                                divForSinglecolorLine.Visible = true;
                                divVariantOption.Attributes.Add("style", "min-height:38px !important;float: left;");
                                //ColorOption.Text += "<a href='javascript:void(0);'  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"border;solid 1px #dddddd;width:33px;height:33px;\"></a>";
                                ColorOption.Text += "<a href=\"javascript:void(0);\" onclick=\"variantlink('" + dscolorOption.Tables[0].Rows[i]["ProductURL"].ToString() + "','" + dscolorOption.Tables[0].Rows[i]["VariantValueID"].ToString() + "');\"  title='" + dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString() + "'><img  src=\"" + AppLogic.AppConfigs("Live_Contant_Server") +  strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dscolorOption.Tables[0].Rows[i]["VariantValue"].ToString()) + "\" style=\"width:33px;height:33px;border:solid 1px #d7d7d7;\"></a>";
                            }
                        }

                    }
                    if (iCheck == 0)
                    {
                        ViewMoreOption.Visible = false;
                        ColorMessage.Visible = false;
                        divWithoutVariantOption.Attributes.Add("style", "padding-top:110px;background: none;");
                        divWithoutVariantOption.Visible = true;
                        divVariantOption.Visible = false;
                        //ColorOption.Text = "";
                    }
                }
                else
                {
                    ViewMoreOption.Visible = false;
                    ColorMessage.Visible = false;
                    divWithoutVariantOption.Attributes.Add("style", "padding-top:110px;background: none;");
                    divWithoutVariantOption.Visible = true;
                    divVariantOption.Visible = false;
                    ColorOption.Text = "";
                }

            }
        }

        public String SetName(String Name)
        {
            //if (Name.Length > 30)
            //    Name = Name.Substring(0, 30) + "...";
            return Server.HtmlEncode(Name);
        }
        public String SetAttribute(String Name)
        {
            return Name.Replace("'", "&#39;").Replace('"', '-').Replace('\'', '-').ToString();
            //return Name.Replace('"', '-').Replace('\'', '-').Replace("'", "&#39;").ToString();
        }

        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") +imagepath))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hdnLink.Value.ToString() != "")
            {

                if (hdnVariant.Value.ToString() != "")
                {
                    Session["VID"] = hdnVariant.Value.ToString();
                    Session["PVID"] = "0";
                    hdnVariant.Value = "";
                }
                else
                {
                    Session["VID"] = null;
                    Session["PVID"] = null;
                }
                Response.Redirect(hdnLink.Value.ToString());
            }
        }
        private void breadcrumbs()
        {
            try
            {
                string strbreadkrum = "";
                if (string.IsNullOrEmpty(strbreadkrum))
                {
                    strbreadkrum = " <a href='/' title='Home'>Home </a><img src='/images/breadcrumbs-bullet.png' alt='' title='' class='breadcrumbs-bullet'>";
                    if (Request.QueryString["CatPID"] != null && Request.QueryString["CatPID"].ToString() != "0")
                    {
                        strbreadkrum += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT '<a title=\"'+ Name +'\" href=\"/'+ Sename +'.html\">'+ Name +'</a><img src=\"/images/breadcrumbs-bullet.png\" alt=\"\" title=\"\" class=\"breadcrumbs-bullet\">' FROm tb_category WHERE categoryid=" + Request.QueryString["CatPID"].ToString() + ""));
                    }
                    if (Request.QueryString["CatID"] != null && Request.QueryString["CatID"].ToString() != "0")
                    {
                        strbreadkrum += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT '<span> '+ Name +' </span> ' FROm tb_category WHERE categoryid=" + Request.QueryString["CatID"].ToString() + ""));
                    }
                    ltbreadcrmbs.Text = strbreadkrum;
                }
                
            }

            catch
            {

            }
        }

    }
}