using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Memo
{
    public partial class MemoView : Solution.UI.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            {
                Response.Redirect("/Admin/Login.aspx");
            }

            if (!IsPostBack)
            {
                FillCalendar();

            }


            if (ViewState["ltrEventCalendar"] != null)
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCalender", ViewState["ltrEventCalendar"].ToString(), true);
            }
        }

        private void FillCalendar()
        {
            ViewState["DefaultDate"] = DateTime.Now;
            ltrEventCalendar.Text = "";
            DataSet Ds = new DataSet();

            //Ds = clsEventCalendarObj.GetAllEventCalander();

            MemoComponent objMemo = new MemoComponent();
            Ds = objMemo.GetMemoByUserID(Convert.ToInt32(Session["AdminID"].ToString()));


            DateTime DefaultDate = Convert.ToDateTime(ViewState["DefaultDate"].ToString());
            int ModalPopupLoop = 0;
            StringBuilder MakeCalendarString = new StringBuilder();
            MakeCalendarString.Append("");
            MakeCalendarString.Append(@"<script type='text/javascript'>

                    $(document).ready(function () {
                        $('#calendar').fullCalendar({
                            
                            theme: false,
                            header: {
                                left: 'prev,next today',
                                center: 'title',
                                right: 'month,agendaWeek,agendaDay'
                            },
                            editable: true,
                            events: [ ");

            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                do
                {
                    try
                    {
                        DateTime FromDate = Convert.ToDateTime(Ds.Tables[0].Rows[ModalPopupLoop]["StartDate"].ToString());
                        DateTime ToDate = Convert.ToDateTime(Ds.Tables[0].Rows[ModalPopupLoop]["StartDate"].ToString());
                        if (ModalPopupLoop == 0)
                        {
                            MakeCalendarString.Append("{" +
                                            "title: '" + Ds.Tables[0].Rows[ModalPopupLoop]["Title"].ToString().Replace("'", "`") + "' , " +
                                            "start: new Date(" + FromDate.Year + ", " + (FromDate.Month - 1) + ", " + FromDate.Day + ", " + FromDate.Hour + ", " + FromDate.Minute + ")," +
                                            "end: new Date(" + ToDate.Year + ", " + (ToDate.Month - 1) + ", " + ToDate.Day + ", " + ToDate.Hour + ", " + ToDate.Minute + ")," +
                                          "url:'javascript:ModalPopupShow(" + Ds.Tables[0].Rows[ModalPopupLoop]["MemoID"].ToString() + ");'," +
                                          "textColor: '#231F20', " +
                                            "color: '#D2D2D2', " +
                                            "allDay: false" +
                                        "}");
                        }
                        else
                        {
                            MakeCalendarString.Append(",{" +
                                            "title: '" + Ds.Tables[0].Rows[ModalPopupLoop]["Title"].ToString().Replace("'", "`") + "' , " +
                                            "start: new Date(" + FromDate.Year + ", " + (FromDate.Month - 1) + ", " + FromDate.Day + ", " + FromDate.Hour + ", " + FromDate.Minute + ")," +
                                            "end: new Date(" + ToDate.Year + ", " + (ToDate.Month - 1) + ", " + ToDate.Day + ", " + ToDate.Hour + ", " + ToDate.Minute + ")," +
                                        "url:'javascript:ModalPopupShow(" + Ds.Tables[0].Rows[ModalPopupLoop]["MemoID"].ToString() + ");'," +
                                        "textColor: '#231F20', " +
                                            "color: '#D2D2D2', " + "allDay: false" +
                                        "}");
                        }

                        ModalPopupLoop += 1;
                    }
                    catch { }
                }
                while (ModalPopupLoop != Ds.Tables[0].Rows.Count);
            }
            MakeCalendarString.Append(@"],
                        timeFormat: 'h(:m)tt'
                        });

                    });

                </script>");

            ltrEventCalendar.Text = MakeCalendarString.ToString();


            //     Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('hi');", false);
            //   Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "msgg", ltrEventCalendar.Text, false);

            ViewState["ltrEventCalendar"] = ltrEventCalendar.Text.ToString().Replace("<script type='text/javascript'>", "").Replace("</script>", "");

            ltrEventCalendar.Text = "";
        }

        protected void btnPopup_Click(object sender, EventArgs e)
        {
            //lblShowPassword.Text = hdnValue.Value;
            lblMemoTitle.Text = "";
            lblStatus.Text = "";
            lblStartDate.Text = "";
            lblDescription.Text = "";
            ltrMoreDetails.Text = "";

            if (hdnValue.Value != "")
            {
                MemoComponent objMemo = new MemoComponent();
                DataSet dsMemo = objMemo.GetMemoByID(Convert.ToInt32(hdnValue.Value));

                if (dsMemo != null && dsMemo.Tables.Count > 0 && dsMemo.Tables[0].Rows.Count > 0)
                {
                    lblMemoTitle.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["Title"]);
                    lblStatus.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["Status"]);
                    lblDescription.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["Description"]);
                    lblStartDate.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["StartDate"]);
                    ltrMoreDetails.Text = " <a href='/Admin/Memo/MemoViewDetails.aspx?MemoID=" + hdnValue.Value + "' style='float:right; color:#F93A21;'>View More..</a>";

                    //  lblEndDate.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["EndDate"]);
                }
                //GetMemoByID
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "centerPopup();loadPopup();", true);
        }
    }
}