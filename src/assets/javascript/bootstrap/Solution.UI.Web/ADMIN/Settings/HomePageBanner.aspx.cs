using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Data;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HomePageBanner : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            AppConfig.StoreID = 1;
            lblTitle.Text = "Home Page Banner";
            BindHomePageBannerDatalist();
        }

        /// <summary>
        /// Binds the Home Page Banner List
        /// </summary>
        private void BindHomePageBannerDatalist()
        {
            DLHomePageBanner.DataSource = null;

            DataTable dtHomePageBanner = new DataTable();
            dtHomePageBanner = HomeBannerComponent.GetHomeBanner(Convert.ToInt32(AppLogic.AppConfigs("StoreID"))).Tables[0];
            if (dtHomePageBanner != null && dtHomePageBanner.Rows.Count > 0)
            {
                DLHomePageBanner.DataSource = dtHomePageBanner;
                DLHomePageBanner.DataBind();
            }
        }

        /// <summary>
        /// DL Home Page Banner Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void DLHomePageBanner_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
            {
                try
                {
                    Literal LtBannerImage = (Literal)e.Item.FindControl("LtBannerImage");
                    Label lblBannerID = (Label)e.Item.FindControl("lblBannerID");
                    if (!String.IsNullOrEmpty(lblBannerID.Text.ToString()))
                    {
                        if (System.IO.File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Home/" + lblBannerID.Text.ToString() + ".jpg")))
                        {
                            LtBannerImage.Text = "<img src=" + AppLogic.AppConfigs("ImagePathBanner") + "Home/" + lblBannerID.Text.ToString() + ".jpg />";
                        }
                    }

                }
                catch (Exception)
                {

                }
            }
        }
    }
}