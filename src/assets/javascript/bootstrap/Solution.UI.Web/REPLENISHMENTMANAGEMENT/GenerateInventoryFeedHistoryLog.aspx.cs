using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.IO;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{

    public partial class GenerateInventoryFeedHistoryLog : System.Web.UI.Page
    {
        
        public static string Storename = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["Storeid"] != null && Request.QueryString["Storeid"].ToString() != "")
                {
                    Storename = Convert.ToString(CommonComponent.GetScalarCommonData("select replace(REPLACE(isnull(storename,''),'HPD',''),'Half Price Drapes','') as name from tb_Replenishment_Store where RepStoreID=" + Request.QueryString["Storeid"] + ""));
                }
                BindInventoryFeedLog();
            }
        }

        private void BindInventoryFeedLog()
        {
            DataSet ds = new DataSet();
            if (Request.QueryString["Storeid"] != null && Request.QueryString["Storeid"].ToString() != "")
            {
                ds = CommonComponent.GetCommonDataSet("Exec usp_GetChannelPartnerFeedFields 4," + Request.QueryString["Storeid"].ToString() + "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    grdInventoryFeedLogHistory.DataSource = ds.Tables[0];
                    grdInventoryFeedLogHistory.DataBind();
                    grdInventoryFeedLogHistory.UseAccessibleHeader = true;

                    grdInventoryFeedLogHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
                    grdInventoryFeedLogHistory.HeaderRow.CssClass = "cf";
                }
                else
                {
                    grdInventoryFeedLogHistory.DataSource = null;
                    grdInventoryFeedLogHistory.DataBind();
                }

            }

        }

        protected void grdInventoryFeedLogHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlAnchor adownloadfile = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("adownloadfile");
                HiddenField hdnfilename = (HiddenField)e.Row.FindControl("hdnfilename");
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
            }
        }
    }
}