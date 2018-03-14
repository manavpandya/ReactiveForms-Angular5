using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class ProductPageConfiguration : BasePage
    {
        ConfigurationComponent objAppComp = new ConfigurationComponent();
        public string ShippingTimeDesc = "", ReturnPolicyDesc = "", RelatedDesc = "", RecentlyDesc = "", PriceMatchDesc = "";
        public string PrintDesc = "", TellAFriendDesc = "", MoreImageDesc = "";
        public string EstimateShippingDesc = "", PostReviewDesc = "", WishlistDesc = "", SocialDesc = "";
        public string ProductBarcodeDesc = "", OrderBarcodeDesc = "", PackingSlipDesc = "", ImageZoomDesc = "", BookMarkPageDesc = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindStore();
            }
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            GetConfigStatus();
            GetDescription();
        }

        private void BindStore()
        {
            DataSet dsStore = StoreComponent.GetStoreList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        public void GetConfigStatus()
        {
            string flagvalue = "", id = "", divid = "", btnid = imgbtnsave.ClientID.ToString();

            if (AppLogic.AppConfigs("SwitchItemShippingTime") != null && AppLogic.AppConfigs("SwitchItemShippingTime").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemShippingTime").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkshippingtime.ClientID.ToString() + ",";
                divid += divshippingtime.ClientID.ToString() + ",";
            }

            if (AppLogic.AppConfigs("SwitchItemReturnPolicy") != null && AppLogic.AppConfigs("SwitchItemReturnPolicy").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemReturnPolicy").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkreturnpolicy.ClientID.ToString() + ",";
                divid += divreturnpolicy.ClientID.ToString() + ",";
            }

            if (AppLogic.AppConfigs("SwitchItemPriceMatch") != null && AppLogic.AppConfigs("SwitchItemPriceMatch").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemPriceMatch").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkpricematch.ClientID.ToString() + ",";
                divid += divpricematch.ClientID.ToString() + ",";
            }

            if (AppLogic.AppConfigs("SwitchItemBookMarkPage") != null && AppLogic.AppConfigs("SwitchItemBookMarkPage").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemBookMarkPage").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkbookmarkpage.ClientID.ToString() + ",";
                divid += divbookmarkpage.ClientID.ToString() + ",";
            }

            if (AppLogic.AppConfigs("SwitchItemImageZoom") != null && AppLogic.AppConfigs("SwitchItemImageZoom").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemImageZoom").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkimagezoom.ClientID.ToString() + ",";
                divid += divimagezoom.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemRelatedProducts") != null && AppLogic.AppConfigs("SwitchItemRelatedProducts").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemRelatedProducts").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkrelatedpro.ClientID.ToString() + ",";
                divid += divrelatedPro.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemRecentlyViewedProducts") != null && AppLogic.AppConfigs("SwitchItemRecentlyViewedProducts").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemRecentlyViewedProducts").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkrecentlypro.ClientID.ToString() + ",";
                divid += divrecentlypro.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemSocialLink") != null && AppLogic.AppConfigs("SwitchItemSocialLink").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemSocialLink").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chksociallink.ClientID.ToString() + ",";
                divid += divsociallink.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemPrintThisPage") != null && AppLogic.AppConfigs("SwitchItemPrintThisPage").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemPrintThisPage").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkprintthispage.ClientID.ToString() + ",";
                divid += divprintthispage.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemTellAFriend") != null && AppLogic.AppConfigs("SwitchItemTellAFriend").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemTellAFriend").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chktellafrnd.ClientID.ToString() + ",";
                divid += divtellafrnd.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemViewMoreImages") != null && AppLogic.AppConfigs("SwitchItemViewMoreImages").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemViewMoreImages").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkviewmoreimages.ClientID.ToString() + ",";
                divid += divviewmoreimages.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemEstimateShipping") != null && AppLogic.AppConfigs("SwitchItemEstimateShipping").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemEstimateShipping").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkestimateshipping.ClientID.ToString() + ",";
                divid += divestimateshipping.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemProductReview") != null && AppLogic.AppConfigs("SwitchItemProductReview").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemProductReview").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkpostreview.ClientID.ToString() + ",";
                divid += divpostreview.ClientID.ToString() + ",";
            }


            if (AppLogic.AppConfigs("SwitchItemAddToWishlist") != null && AppLogic.AppConfigs("SwitchItemAddToWishlist").ToString() != "")
            {
                if (AppLogic.AppConfigs("SwitchItemAddToWishlist").ToLower().ToString() == "true")
                {
                    flagvalue += "true" + ",";
                }
                else
                {
                    flagvalue += "false" + ",";
                }
                id += chkaddtowishlist.ClientID.ToString() + ",";
                divid += divaddtowishlist.ClientID.ToString() + ",";
            }


            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "MakeCheckedall('" + flagvalue.TrimEnd(',') + "','" + id.TrimEnd(',') + "');Getstatusall('" + id.TrimEnd(',') + "','" + divid.TrimEnd(',') + "','" + btnid.TrimEnd(',') + "','" + string.Empty.TrimEnd(',') + "');", true);
        }

        protected void imgbtnsave_Click(object sender, ImageClickEventArgs e)
        {
            string flag = "false";
            bool f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6 = false, f7 = false, f8 = false, f9 = false, f10 = false, f11 = false, f12 = false, f13 = false, f14 = false;
            objAppComp = new ConfigurationComponent();

            if (chkshippingtime.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            Int32 isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemShippingTime", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f1 = true;
            }


            flag = "false";
            if (chkrelatedpro.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemRelatedProducts", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f2 = true;
            }


            flag = "false";
            if (chkrecentlypro.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemRecentlyViewedProducts", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f3 = true;
            }


            flag = "false";
            if (chksociallink.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }

            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemSocialLink", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f4 = true;
            }

            flag = "false";
            if (chkprintthispage.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }

            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemPrintThisPage", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f5 = true;
            }


            flag = "false";
            if (chktellafrnd.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemTellAFriend", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f6 = true;
            }


            flag = "false";
            if (chkviewmoreimages.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemViewMoreImages", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f7 = true;
            }


            flag = "false";
            if (chkestimateshipping.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemEstimateShipping", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f8 = true;
            }


            flag = "false";
            if (chkpostreview.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemProductReview", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f9 = true;
            }

            flag = "false";
            if (chkaddtowishlist.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemAddToWishlist", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f10 = true;
            }

            flag = "false";
            if (chkreturnpolicy.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemReturnPolicy", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f11 = true;
            }

            flag = "false";
            if (chkimagezoom.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemImageZoom", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f12 = true;
            }

            flag = "false";
            if (chkpricematch.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemPriceMatch", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f13 = true;
            }

            flag = "false";
            if (chkbookmarkpage.Checked)
            {
                flag = "true";
            }
            else
            {
                flag = "false";
            }
            isupdated = objAppComp.UpdateAppConfigvalue("SwitchItemBookMarkPage", flag, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));
            if (isupdated > 0)
            {
                f13 = true;
            }

            if (f1 == f2 == f3 == f4 == f5 == f6 == f7 == f8 == f9 == f10 == f11 == f12 == f13 == f14 == true)
            {
                GetConfigStatus();
            }
        }

        public void GetDescription()
        {
            DataSet ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemShippingTime", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ShippingTimeDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemReturnPolicy", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ReturnPolicyDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemPriceMatch", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                PriceMatchDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemBookMarkPage", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                BookMarkPageDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemImageZoom", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ImageZoomDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemRelatedProducts", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RelatedDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemRecentlyViewedProducts", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RecentlyDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemSocialLink", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SocialDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemPrintThisPage", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                PrintDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemTellAFriend", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                TellAFriendDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemViewMoreImages", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                MoreImageDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemEstimateShipping", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                EstimateShippingDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemProductReview", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                PostReviewDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }

            ds = new DataSet();
            ds = objAppComp.GetConfigDescription("SwitchItemAddToWishlist", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                WishlistDesc = ds.Tables[0].Rows[0]["Description"].ToString();
            }
        }
    }
}