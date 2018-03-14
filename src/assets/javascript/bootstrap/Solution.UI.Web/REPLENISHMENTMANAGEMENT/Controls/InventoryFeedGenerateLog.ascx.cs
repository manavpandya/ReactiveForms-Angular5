using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.IO;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT.Controls
{
    public partial class InventoryFeedGenerateLog : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInventoryFeedLog();
            }
        }


        private void BindInventoryFeedLog()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("Exec usp_GetChannelPartnerFeedFields 3");
            if(ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                grdInventoryFeedLog.DataSource = ds.Tables[0];
                grdInventoryFeedLog.DataBind();
                grdInventoryFeedLog.UseAccessibleHeader = true;

                grdInventoryFeedLog.HeaderRow.TableSection = TableRowSection.TableHeader;
                grdInventoryFeedLog.HeaderRow.CssClass = "cf";
            }
            else
            {
                grdInventoryFeedLog.DataSource = null;
                grdInventoryFeedLog.DataBind();
            }
        }

        protected void grdInventoryFeedLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlAnchor aviewhistory = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("aviewhistory");
                  HiddenField hdnfilename = (HiddenField)e.Row.FindControl("hdnfilename");
                 System.Web.UI.HtmlControls.HtmlAnchor adownloadfile = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("adownloadfile");
                HiddenField hdnstoreid = (HiddenField)e.Row.FindControl("hdnstoreid");
                try
                {
                    Label lblgeneratedon = (Label)e.Row.FindControl("lblgeneratedon");
                    lblgeneratedon.Text = String.Format("{0:MM/dd/yyyy hh:mm tt}", Convert.ToDateTime(lblgeneratedon.Text));
                }
                catch { }
              
                if (hdnfilename != null && hdnfilename.Value != "" && hdnfilename.Value != "0")
                {
                    if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));
                    if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + hdnfilename.Value.ToString())))
                    {
                        adownloadfile.Visible = true;
                        adownloadfile.HRef = "~/REPLENISHMENTMANAGEMENT/Files/" + hdnfilename.Value.ToString();

                    }
                    else
                    {
                        adownloadfile.Visible = false;
                    }
                   
                }
                else { adownloadfile.Visible = false; }



                aviewhistory.HRef = "~/REPLENISHMENTMANAGEMENT/GenerateInventoryFeedHistoryLog.aspx?Storeid=" + hdnstoreid.Value.ToString() + "";
            }
        }
    }
}