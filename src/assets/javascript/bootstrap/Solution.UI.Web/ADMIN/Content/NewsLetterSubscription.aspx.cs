using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Content
{
    public partial class NewsLetterSubscription : BasePage
    {
        #region Variable declaration
        public static bool isDescendEmail = false;
        public static bool isDescendStoreName = false;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                bindstore();
            }
        }

        /// <summary>
        /// Bind Store dropdown
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            NewsSubscribtionComponent NewsComponent = new NewsSubscribtionComponent();
            tb_NewsSubscription newsSubscription = new tb_NewsSubscription();
            int totalRowCount = grdNewsSubScription.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdNewsSubScription.Rows[i].FindControl("hdnNewsSubscriptionID");
                CheckBox chk = (CheckBox)grdNewsSubScription.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    NewsComponent.delNews(Convert.ToInt32(hdn.Value));
                }
            }
            grdNewsSubScription.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
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


            grdNewsSubScription.DataBind();
        }

        /// <summary>
        /// News Subscription Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdNewsSubScription_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdNewsSubScription.Rows.Count > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
            }


            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendEmail == false)
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnEmailID");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnEmail.AlternateText = "Ascending Order";
                    btnEmail.ToolTip = "Ascending Order";
                    btnEmail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnEmailID");
                    btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnEmail.AlternateText = "Descending Order";
                    btnEmail.ToolTip = "Descending Order";
                    btnEmail.CommandArgument = "ASC";
                }
                if (isDescendStoreName == false)
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnStoreName.AlternateText = "Ascending Order";
                    btnStoreName.ToolTip = "Ascending Order";
                    btnStoreName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnStoreName = (ImageButton)e.Row.FindControl("btnStoreName");
                    btnStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnStoreName.AlternateText = "Descending Order";
                    btnStoreName.ToolTip = "Descending Order";
                    btnStoreName.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Sort GridView in ASC or DESC order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btnSorting = (ImageButton)sender;
            if (btnSorting != null)
            {
                if (btnSorting.CommandArgument == "ASC")
                {
                    grdNewsSubScription.Sort(btnSorting.CommandName.ToString(), SortDirection.Ascending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btnSorting.ID == "btnEmailID")
                    {
                        isDescendEmail = false;
                    }
                    else if (btnSorting.ID == "btnStoreName")
                    {
                        isDescendStoreName = false;
                    }

                    btnSorting.AlternateText = "Descending Order";
                    btnSorting.ToolTip = "Descending Order";
                    btnSorting.CommandArgument = "DESC";
                }
                else if (btnSorting.CommandArgument == "DESC")
                {
                    grdNewsSubScription.Sort(btnSorting.CommandName.ToString(), SortDirection.Descending);
                    btnSorting.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btnSorting.ID == "btnEmailID")
                    {
                        isDescendEmail = true;
                    }
                    else if (btnSorting.ID == "btnStoreName")
                    {
                        isDescendStoreName = true;
                    }

                    btnSorting.AlternateText = "Ascending Order";
                    btnSorting.ToolTip = "Ascending Order";
                    btnSorting.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Search Button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdNewsSubScription.DataBind();
        }

        /// <summary>
        /// ShowAll button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            ddlStore.SelectedIndex = 0;
            txtSearch.Text = "";
            grdNewsSubScription.DataBind();
        }

        /// <summary>
        /// Export Button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            string whereclause;
            CommonComponent clsCommon = new CommonComponent();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=NewsSubScriptionList_Export.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Email ID,Date");
            DataSet dsNews = new DataSet();
            if (ddlStore.SelectedIndex == 0 || ddlStore.SelectedIndex == -1)
            {
                if (txtSearch.Text != "" && txtSearch.Text != null)
                {
                    whereclause = " Email like '%" + txtSearch.Text + "%'";
                    dsNews = CommonComponent.GetCommonDataSet("select Email,CreatedOn from tb_NewsSubscription where " + whereclause);
                }
                else
                {
                    dsNews = CommonComponent.GetCommonDataSet("select Email,CreatedOn from tb_NewsSubscription");
                }
            }
            else
            {
                if (txtSearch.Text != "" && txtSearch.Text != null)
                {
                    whereclause = " and Email like '%" + txtSearch.Text + "%'";
                    dsNews = CommonComponent.GetCommonDataSet("select Email,CreatedOn from tb_NewsSubscription where StoreID=" + ddlStore.SelectedValue + whereclause);
                }
                else
                {
                    dsNews = CommonComponent.GetCommonDataSet("select Email,CreatedOn from tb_NewsSubscription where StoreID=" + ddlStore.SelectedValue);
                }

            }

            object[] args = new object[3];

            for (int i = 0; i < dsNews.Tables[0].Rows.Count; i++)
            {
                args[0] = dsNews.Tables[0].Rows[i]["Email"].ToString();
                args[1] = dsNews.Tables[0].Rows[i]["CreatedOn"].ToString();
                sb.AppendLine(string.Format("{0},\"{1}\"", args));
            }


            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
    }
}