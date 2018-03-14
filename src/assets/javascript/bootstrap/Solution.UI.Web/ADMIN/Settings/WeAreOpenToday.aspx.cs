using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class WeAreOpenToday : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindData();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

            }
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {


                if (string.IsNullOrEmpty(date_timepicker_start.Text))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please enter Start Date.', 'Message','');", true);

                    return;
                }
                else if (string.IsNullOrEmpty(date_timepicker_end.Text))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please enter End Date.', 'Message','');", true);
                    return;
                }


                DateTime StartDate = Convert.ToDateTime(date_timepicker_start.Text);
                DateTime EndDate = Convert.ToDateTime(date_timepicker_end.Text);

                // equal to
                if (StartDate == EndDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Start Date and End Date Equal To.', 'Message','');", true);
                    return;
                }
                // greater than
                if (StartDate > EndDate)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Start Date and End Date Greater Than.', 'Message','');", true);
                    return;
                }

                int AdminID = 0;
                int.TryParse(Convert.ToString(Session["AdminID"]), out AdminID);
                WeAreTodayComponent.InsertWeareToday(StartDate, EndDate, AdminID);
                BindData();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record inserted successfully', 'Message','');", true);
                return;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('"+ex.InnerException.Message.ToString()+"', 'Message','');", true);
                    return;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('"+ ex.Message.ToString()+"', 'Message','');", true);
                    return;
                }

            }
        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WeAreOpenToday.aspx");
        }

        public void BindData()
        {
            RptList.DataSource = WeAreTodayComponent.GetWeareToday();
            RptList.DataBind();
        }

        protected void RptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((ImageButton)e.Item.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }

        protected void RptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteDate") // check command is DeleteDate
            {
                int returnID = 0;
                int WearetodayID = Convert.ToInt32(e.CommandArgument);
               returnID =  WeAreTodayComponent.DeleteWeareToday(WearetodayID);
               BindData();
               if (returnID > 0)
               {
                   Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record Deleted successfully', 'Message','');", true);
                   return;
               }
            }
        }


    }
}