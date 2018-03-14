using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class Tradereport : BasePage
    {

        public static bool isDescendbtnemail = false;
        public static bool isDescendbtnname = false;
        public static bool isDescendbtnregdate = false;
        public static bool isDescendbtncouponcode = false;
        public static bool isDescendbtntotalorder = false;
        public static bool isDescendbtntotalamount = false;
        public static bool isDescendbtnlastorderdate = false;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.AddMonths(-1)));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");



                //GetEmail();
            }
        }


        /// <summary>
        ///  Mail Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvMailReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMailReport.PageIndex = e.NewPageIndex;
            if (hdnflag.Value.ToString() == "0")
            {
                GetEmail();
            }
            else if (hdnflag.Value.ToString() == "1")
            {
                getemailall();
            }
            else
            {
                GetEmail();
            }

        }


        /// <summary>
        /// Get Email By Store
        /// </summary>
        private void GetEmail()
        {



            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

            }



            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '" + Convert.ToDateTime(txtMailFrom.Text.ToString()) + "','" + Convert.ToDateTime(txtMailTo.Text.ToString()) + "','" + txtSearch.Text.ToString().Replace("'", "''") + "'");
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                Session["dataexport"] = (DataSet)dsMail;
                grvMailReport.DataSource = dsMail;
                grvMailReport.DataBind();

                btnexport.Visible = true;
            }
            else
            {
                Session["dataexport"] = null;
                grvMailReport.DataSource = null;
                grvMailReport.DataBind();
                btnexport.Visible = false;
            }



        }



        private void getemailall()
        {

            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '','',''");
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                Session["dataexport"] = (DataSet)dsMail;
                grvMailReport.DataSource = dsMail;
                grvMailReport.DataBind();

                btnexport.Visible = true;
            }
            else
            {
                Session["dataexport"] = null;
                grvMailReport.DataSource = null;
                grvMailReport.DataBind();
                btnexport.Visible = false;
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hdnflag.Value = "0";
            grvMailReport.PageIndex = 0;
            GetEmail();
        }

        protected void grvMailReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendbtnemail == false)
                {
                    ImageButton btnemail = (ImageButton)e.Row.FindControl("btnemail");
                    btnemail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnemail.AlternateText = "Ascending Order";
                    btnemail.ToolTip = "Ascending Order";
                    btnemail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnemail = (ImageButton)e.Row.FindControl("btnemail");
                    btnemail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnemail.AlternateText = "Descending Order";
                    btnemail.ToolTip = "Descending Order";
                    btnemail.CommandArgument = "ASC";
                }
                if (isDescendbtnname == false)
                {
                    ImageButton btnname = (ImageButton)e.Row.FindControl("btnname");
                    btnname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnname.AlternateText = "Ascending Order";
                    btnname.ToolTip = "Ascending Order";
                    btnname.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnname = (ImageButton)e.Row.FindControl("btnname");
                    btnname.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnname.AlternateText = "Descending Order";
                    btnname.ToolTip = "Descending Order";
                    btnname.CommandArgument = "ASC";
                }
                if (isDescendbtnregdate == false)
                {
                    ImageButton btnregdate = (ImageButton)e.Row.FindControl("btnregdate");
                    btnregdate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnregdate.AlternateText = "Ascending Order";
                    btnregdate.ToolTip = "Ascending Order";
                    btnregdate.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnregdate = (ImageButton)e.Row.FindControl("btnregdate");
                    btnregdate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnregdate.AlternateText = "Descending Order";
                    btnregdate.ToolTip = "Descending Order";
                    btnregdate.CommandArgument = "ASC";
                }
                if (isDescendbtncouponcode == false)
                {
                    ImageButton btncouponcode = (ImageButton)e.Row.FindControl("btncouponcode");
                    btncouponcode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btncouponcode.AlternateText = "Ascending Order";
                    btncouponcode.ToolTip = "Ascending Order";
                    btncouponcode.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btncouponcode = (ImageButton)e.Row.FindControl("btncouponcode");
                    btncouponcode.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btncouponcode.AlternateText = "Descending Order";
                    btncouponcode.ToolTip = "Descending Order";
                    btncouponcode.CommandArgument = "ASC";
                }
                if (isDescendbtntotalorder == false)
                {
                    ImageButton btntotalorder = (ImageButton)e.Row.FindControl("btntotalorder");
                    btntotalorder.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btntotalorder.AlternateText = "Ascending Order";
                    btntotalorder.ToolTip = "Ascending Order";
                    btntotalorder.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btntotalorder = (ImageButton)e.Row.FindControl("btntotalorder");
                    btntotalorder.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btntotalorder.AlternateText = "Descending Order";
                    btntotalorder.ToolTip = "Descending Order";
                    btntotalorder.CommandArgument = "ASC";
                }

                if (isDescendbtntotalamount == false)
                {
                    ImageButton btntotalamount = (ImageButton)e.Row.FindControl("btntotalamount");
                    btntotalamount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btntotalamount.AlternateText = "Ascending Order";
                    btntotalamount.ToolTip = "Ascending Order";
                    btntotalamount.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btntotalamount = (ImageButton)e.Row.FindControl("btntotalamount");
                    btntotalamount.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btntotalamount.AlternateText = "Descending Order";
                    btntotalamount.ToolTip = "Descending Order";
                    btntotalamount.CommandArgument = "ASC";
                }

                if (isDescendbtnlastorderdate == false)
                {
                    ImageButton btnlastorderdate = (ImageButton)e.Row.FindControl("btnlastorderdate");
                    btnlastorderdate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnlastorderdate.AlternateText = "Ascending Order";
                    btnlastorderdate.ToolTip = "Ascending Order";
                    btnlastorderdate.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnlastorderdate = (ImageButton)e.Row.FindControl("btnlastorderdate");
                    btnlastorderdate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnlastorderdate.AlternateText = "Descending Order";
                    btnlastorderdate.ToolTip = "Descending Order";
                    btnlastorderdate.CommandArgument = "ASC";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblregdate = (Label)e.Row.FindControl("lblregdate");
                Label lblorderdate = (Label)e.Row.FindControl("lblorderdate");

                if (!String.IsNullOrEmpty(lblregdate.Text))
                {
                    lblregdate.Text = (Convert.ToDateTime(lblregdate.Text)).ToString("MM/dd/yyyy");
                }

                if (!String.IsNullOrEmpty(lblorderdate.Text))
                {
                    lblorderdate.Text = (Convert.ToDateTime(lblorderdate.Text)).ToString("MM/dd/yyyy");
                }

            }
        }
        /// <summary>
        /// Sort Column in ASC or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn != null)
            {
                if (btn.CommandArgument == "ASC")
                {
                    grvMailReport.Sort(btn.CommandName.ToString(), SortDirection.Ascending);
                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btn.ID == "btnemail")
                    {
                        isDescendbtnemail = false;
                    }
                    else if (btn.ID == "btnname")
                    {
                        isDescendbtnname = false;
                    }
                    else if (btn.ID == "btnregdate")
                    {
                        isDescendbtnregdate = false;
                    }
                    else if (btn.ID == "btncouponcode")
                    {
                        isDescendbtncouponcode = false;
                    }
                    else if (btn.ID == "btntotalorder")
                    {
                        isDescendbtntotalorder = false;
                    }
                    else if (btn.ID == "btntotalamount")
                    {
                        isDescendbtntotalamount = false;
                    }
                    else if (btn.ID == "btnlastorderdate")
                    {
                        isDescendbtnlastorderdate = false;
                    }
                    btn.AlternateText = "Descending Order";
                    btn.ToolTip = "Descending Order";
                    btn.CommandArgument = "DESC";
                }
                else if (btn.CommandArgument == "DESC")
                {
                    grvMailReport.Sort(btn.CommandName.ToString(), SortDirection.Descending);
                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btn.ID == "btnemail")
                    {
                        isDescendbtnemail = true;
                    }
                    else if (btn.ID == "btnname")
                    {
                        isDescendbtnname = true;
                    }
                    else if (btn.ID == "btnregdate")
                    {
                        isDescendbtnregdate = true;
                    }
                    else if (btn.ID == "btncouponcode")
                    {
                        isDescendbtncouponcode = true;
                    }
                    else if (btn.ID == "btntotalorder")
                    {
                        isDescendbtntotalorder = true;
                    }
                    else if (btn.ID == "btntotalamount")
                    {
                        isDescendbtntotalamount = true;
                    }
                    else if (btn.ID == "btnlastorderdate")
                    {
                        isDescendbtnlastorderdate = true;
                    }
                    btn.AlternateText = "Ascending Order";
                    btn.ToolTip = "Ascending Order";
                    btn.CommandArgument = "ASC";
                }
            }
        }

        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }
        protected void DownloadProductExport()
        {
            if (ViewState["LastExportFileNametrade"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileNametrade"].ToString());
                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastExportFileNametrade"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('File not found.','Message');", true);
                return;
            }
        }

        protected void btnexport_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsorder = new DataSet();
            if (Session["dataexport"] == null)
            {
                if(hdnflag.Value.ToString()=="0")
                {
                    dsorder = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '" + Convert.ToDateTime(txtMailFrom.Text.ToString()) + "','" + Convert.ToDateTime(txtMailTo.Text.ToString()) + "','" + txtSearch.Text.ToString().Replace("'", "''") + "'");
                }
                else if (hdnflag.Value.ToString() == "1")
                {
                    dsorder = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '','',''");
                }
                else
                {
                    dsorder = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '" + Convert.ToDateTime(txtMailFrom.Text.ToString()) + "','" + Convert.ToDateTime(txtMailTo.Text.ToString()) + "','" + txtSearch.Text.ToString().Replace("'", "''") + "'");
                }
                
            }
            else
            {
                dsorder = (DataSet)Session["dataexport"];
            }

            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                SecurityComponent objsec = new SecurityComponent();

                string column = "";
                string columnnom = "";
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                    {
                        if (dsorder.Tables[0].Columns.Count - 1 == i)
                        {
                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                            columnnom += "{" + i.ToString() + "}";
                        }
                        else
                        {
                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                            columnnom += "{" + i.ToString() + "},";
                        }
                    }
                }


                sb.AppendLine(column);
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                    {
                        object[] args = new object[dsorder.Tables[0].Columns.Count];
                        for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }
                        sb.AppendLine(string.Format(columnnom, args));
                    }
                    if (!String.IsNullOrEmpty(sb.ToString()))
                    {
                        string FullString = sb.ToString();
                        sb.Remove(0, sb.Length);
                        sb.AppendLine(FullString);

                        DateTime dt = DateTime.Now;
                        string StrStorename = "TradeExport";

                        String FileName = StrStorename.ToString() + "_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                        if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                            Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                        ViewState["LastExportFileNametrade"] = null;
                        String FilePath = Server.MapPath("~/Admin/Files/" + FileName);

                        ViewState["LastExportFileNametrade"] = FileName.ToString();
                        WriteFile(sb.ToString(), FilePath);
                    }
                    if (ViewState["LastExportFileNametrade"] != null)
                    {
                        DownloadProductExport();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Data Exported Successfully.','Message');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                        return;
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }
        }

        protected void grvMailReport_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnshowall_Click(object sender, EventArgs e)
        {
            hdnflag.Value = "1";
            txtSearch.Text = "";
            txtMailFrom.Text = "";
            txtMailTo.Text = "";
            getemailall();

        }



    }
}