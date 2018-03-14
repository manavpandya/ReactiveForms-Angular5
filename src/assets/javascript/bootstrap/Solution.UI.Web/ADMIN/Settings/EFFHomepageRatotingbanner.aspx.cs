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
    public partial class EFFHomepageRatotingbanner : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                imgback.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/back.png";
                BindStore();
                GetBannerId();
            }
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
            if (Request.QueryString["id"] != null)
            {
                ddlStore.SelectedValue = Request.QueryString["id"].ToString();
            }
            else if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }
        private void GetBannerId()
        {
            Int32 Id = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 BannerTypeId FROM tb_RotatorHomebannerEFF WHERE StoreID=" + ddlStore.SelectedValue.ToString() + " AND isnull(IsActive,0)=1"));
            ltradio.Text = "<input type=\"radio\"  id=\"flat-radio\" name=\"flat-radio\" />";
            ltradio1.Text = "<input type=\"radio\"  id=\"flat-radio-1\" name=\"flat-radio\" />";
            ltradio2.Text = "<input type=\"radio\"  id=\"flat-radio-2\" name=\"flat-radio\" />";
            if (Id > 0)
            {
                if (Id == 1)
                {
                    ltradio.Text = "<input type=\"radio\" checked id=\"flat-radio\" name=\"flat-radio\" />";
                    ltradio1.Text = "<input type=\"radio\"  id=\"flat-radio-1\" name=\"flat-radio\" />";
                    ltradio2.Text = "<input type=\"radio\"  id=\"flat-radio-2\" name=\"flat-radio\" />";

                }
                else if (Id == 2)
                {
                    ltradio.Text = "<input type=\"radio\"  id=\"flat-radio\" name=\"flat-radio\" />";
                    ltradio1.Text = "<input type=\"radio\" checked id=\"flat-radio-1\" name=\"flat-radio\" />";
                    ltradio2.Text = "<input type=\"radio\"  id=\"flat-radio-2\" name=\"flat-radio\" />";
                }
                else if (Id == 3)
                {
                    ltradio.Text = "<input type=\"radio\"  id=\"flat-radio\" name=\"flat-radio\" />";
                    ltradio1.Text = "<input type=\"radio\"  id=\"flat-radio-1\" name=\"flat-radio\" />";
                    ltradio2.Text = "<input type=\"radio\" checked  id=\"flat-radio-2\" name=\"flat-radio\" />";
                }

            }
        }
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            GetBannerId();
        }
        protected void imgback_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("EFFHomePagebannerlist.aspx");
        }
    }
}