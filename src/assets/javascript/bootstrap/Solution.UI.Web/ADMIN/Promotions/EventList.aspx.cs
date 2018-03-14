using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class EventList : Solution.UI.Web.BasePage
    {
        #region Declaration

        CouponComponent couponcomp = new CouponComponent();
        StoreComponent stac = new StoreComponent();
        public static bool isDescendcoupon = false;
        public static bool isDescendStore = false;
        public static bool isDescendexpiredate = false;
        public static bool isDescenddiscount = false;
        public static bool isDescenddiscountamt = false;
        public static bool isDescendstartdate = false;


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
                isDescendcoupon = false;
                isDescendStore = false;
                isDescendexpiredate = false;
                isDescenddiscount = false;
                isDescenddiscountamt = false;
                isDescendstartdate = false;
                BindStore();
                if (Request.QueryString["st"] != null)
                {
                    ddlSearch.SelectedValue = Request.QueryString["st"].ToString();
                }
                if (Request.QueryString["txtse"] != null)
                {
                    txtSearch.Text = Request.QueryString["txtse"].ToString();

                }
                BindEventList();

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Event Details inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Event Details updated successfully.', 'Message');});", true);
                    }
                }
                int loginid = 0;
                if (Request.QueryString["loginid"] != "" && Request.QueryString["loginid"] != null)
                {
                    loginid = Convert.ToInt32(Request.QueryString["loginid"]);

                    hdnLoginID.Text = Convert.ToString(loginid);

                }
            }
        }


        private void BindEventList()
        {
            DataSet dsevents = new DataSet();
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                dsevents = CommonComponent.GetCommonDataSet("select * from tb_Event order by createdon desc");
            }
            else
            {
                dsevents = CommonComponent.GetCommonDataSet("select * from tb_Event where name like '%" + txtSearch.Text.Replace("'", "''") + "%'");
            }

            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                gridcoupon.DataSource = dsevents.Tables[0];
                gridcoupon.DataBind();
            }
            else
            {
                gridcoupon.DataSource = null;
                gridcoupon.DataBind();
            }
            int numVisible = 0;
            foreach (GridViewRow row in gridcoupon.Rows)
            {
                if (row.Visible == true)
                {
                    numVisible += 1;
                }
            }

            if (numVisible > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
                gridcoupon.DataSource = null;
                gridcoupon.DataBind();
            }
        }
        /// <summary>
        /// Binds the Store Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
            else
                ddlstore.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Storeid"]))
            {

                if (Convert.ToInt32(Request.QueryString["Storeid"].ToString()) > 0)
                {
                    ddlstore.SelectedValue = (Request.QueryString["Storeid"].ToString());
                }
                else
                {
                    ddlstore.SelectedIndex = 0;
                }

            }
        }

        /// <summary>
        /// Coupon Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gridcoupon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int couponid = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("ExpireEventBanner.aspx?EventID=" + couponid + "&st=" + ddlSearch.SelectedValue.ToString() + "&txtse=" + Server.UrlEncode(txtSearch.Text.ToString()) + "");
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstore.SelectedValue.ToString() == "0" || ddlstore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            AppConfig.StoreID = 1;
            gridcoupon.PageIndex = 0;
            gridcoupon.DataBind();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("EventList.aspx?st=" + ddlSearch.SelectedValue.ToString() + "&txtse=" + Server.UrlEncode(txtSearch.Text.ToString()) + "");
            //DataSet dsevents = new DataSet();
            //dsevents = CommonComponent.GetCommonDataSet("select * from tb_Event where name like '%" + txtSearch.Text + "%'");
            //if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            //{
            //    gridcoupon.DataSource = dsevents.Tables[0];
            //    gridcoupon.DataBind();
            //}
            //else
            //{
            //    gridcoupon.DataSource = null;
            //    gridcoupon.DataBind();
            //}
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnshowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlSearch.SelectedIndex = 0;
            Response.Redirect("EventList.aspx");
            //BindEventList();
        }

        /// <summary>
        /// Coupon Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gridcoupon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (gridcoupon.Rows.Count > 0)
            //    trBottom.Visible = true;
            //else
            //    trBottom.Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkedit = (ImageButton)e.Row.FindControl("lnkbtnedit");
                lnkedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                System.Web.UI.HtmlControls.HtmlAnchor alinkdisplay = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("alinkdisplay");
                HiddenField hdnurlname = (HiddenField)e.Row.FindControl("hdnurlname");
                HiddenField hdneventid = (HiddenField)e.Row.FindControl("hdneventid");

                alinkdisplay.HRef = "SalesEventDisplayOrder.aspx?id=" + hdneventid.Value.ToString() + "&url=" + hdnurlname.Value.ToString();
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendcoupon == false)
                {
                    ImageButton lbcoupon = (ImageButton)e.Row.FindControl("sortcode");
                    lbcoupon.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbcoupon.AlternateText = "Ascending Order";
                    lbcoupon.ToolTip = "Ascending Order";
                    lbcoupon.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbcoupon = (ImageButton)e.Row.FindControl("sortcode");
                    lbcoupon.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbcoupon.AlternateText = "Descending Order";
                    lbcoupon.ToolTip = "Descending Order";
                    lbcoupon.CommandArgument = "ASC";
                }
                if (isDescendStore == false)
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("sortstore");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbstore.AlternateText = "Ascending Order";
                    lbstore.ToolTip = "Ascending Order";
                    lbstore.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbstore = (ImageButton)e.Row.FindControl("sortstore");
                    lbstore.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbstore.AlternateText = "Descending Order";
                    lbstore.ToolTip = "Descending Order";
                    lbstore.CommandArgument = "ASC";
                }

                if (isDescendexpiredate == false)
                {
                    ImageButton lbexpire = (ImageButton)e.Row.FindControl("sortexpire");
                    lbexpire.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbexpire.AlternateText = "Ascending Order";
                    lbexpire.ToolTip = "Ascending Order";
                    lbexpire.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbexpire = (ImageButton)e.Row.FindControl("sortexpire");
                    lbexpire.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbexpire.AlternateText = "Descending Order";
                    lbexpire.ToolTip = "Descending Order";
                    lbexpire.CommandArgument = "ASC";
                }

                if (isDescendstartdate == false)
                {
                    ImageButton lbexpirestart = (ImageButton)e.Row.FindControl("sortexpirestart");
                    lbexpirestart.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbexpirestart.AlternateText = "Ascending Order";
                    lbexpirestart.ToolTip = "Ascending Order";
                    lbexpirestart.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbexpirestart = (ImageButton)e.Row.FindControl("sortexpirestart");
                    lbexpirestart.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbexpirestart.AlternateText = "Descending Order";
                    lbexpirestart.ToolTip = "Descending Order";
                    lbexpirestart.CommandArgument = "ASC";
                }


            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lbexpiredate = (Label)e.Row.FindControl("lbexpiredate");
                Label lbstartdate = (Label)e.Row.FindControl("lbstartdate");
                lbexpiredate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(lbexpiredate.Text));
                lbstartdate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(lbstartdate.Text));
                Literal ltStatusColor = (Literal)e.Row.FindControl("ltStatusColor");

                string todaydate = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //int result = string.Compare(lbexpiredate.Text, todaydate);




                if (Convert.ToDateTime(lbexpiredate.Text) >= Convert.ToDateTime(todaydate) && Convert.ToDateTime(todaydate) >= Convert.ToDateTime(lbstartdate.Text))
                {
                    if (ddlSearch.SelectedValue.ToString() == "")
                    {

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "0")
                    {
                        e.Row.Visible = false;

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "1")
                    {

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "2")
                    {
                        e.Row.Visible = false;
                    }
                    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:green'></div>&nbsp;";
                }
                else if (Convert.ToDateTime(lbstartdate.Text) > Convert.ToDateTime(todaydate) && Convert.ToDateTime(lbexpiredate.Text) > Convert.ToDateTime(todaydate))
                {
                    if (ddlSearch.SelectedValue.ToString() == "")
                    {

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "0")
                    {
                        e.Row.Visible = false;

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "1")
                    {
                        e.Row.Visible = false;
                    }
                    else if (ddlSearch.SelectedValue.ToString() == "2")
                    {

                    }
                    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:yellow'></div>&nbsp;";
                }
                else
                {
                    if (ddlSearch.SelectedValue.ToString() == "")
                    {

                    }
                    else if (ddlSearch.SelectedValue.ToString() == "0")
                    {


                    }
                    else if (ddlSearch.SelectedValue.ToString() == "1")
                    {
                        e.Row.Visible = false;
                    }
                    else if (ddlSearch.SelectedValue.ToString() == "2")
                    {
                        e.Row.Visible = false;
                    }
                    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";
                }
            }
        }

        /// <summary>
        /// Sorting Event of Gridview
        /// </summary>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    gridcoupon.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "sortcode")
                    {
                        isDescendcoupon = false;
                    }
                    else if (lb.ID == "sortstore")
                    {
                        isDescendStore = false;
                    }
                    else if (lb.ID == "sortexpirestart")
                    {
                        isDescendstartdate = false;
                    }
                    else if (lb.ID == "sortexpire")
                    {
                        isDescendexpiredate = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    gridcoupon.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "sortcode")
                    {
                        isDescendcoupon = true;
                    }
                    else if (lb.ID == "sortstore")
                    {
                        isDescendStore = true;
                    }
                    else if (lb.ID == "sortexpirestart")
                    {
                        isDescendstartdate = false;
                    }
                    else if (lb.ID == "sortexpire")
                    {
                        isDescendexpiredate = true;
                    }


                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            int totalRowCount = gridcoupon.Rows.Count;
            for (int tt = 0; tt < totalRowCount; tt++)
            {
                HiddenField hdn = (HiddenField)gridcoupon.Rows[tt].FindControl("hdneventid");
                CheckBox chk = (CheckBox)gridcoupon.Rows[tt].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    string ProIDS = ",";

                    string couponid = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(couponcode,'') as CouponCode from tb_event where eventid=" + hdn.Value.ToString() + ""));
                    // string Productids = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Productids,'') as Productids from tb_event where eventid=" + hdn.Value.ToString() + ""));
                    CommonComponent.ExecuteCommonData("delete from tb_Event where EventId=" + hdn.Value.ToString() + "");
                    if (!String.IsNullOrEmpty(couponid))
                    {
                        DataSet dspids = new DataSet();
                        dspids = CommonComponent.GetCommonDataSet("select isnull(Productids,'') as Productids from tb_event where couponcode='" + couponid.ToString().Trim() + "' and cast(Enddate as date) >=cast(getdate() as date) and isnull(Productids,'')<>''");
                        if (dspids != null && dspids.Tables.Count > 0 && dspids.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dspids.Tables[0].Rows.Count; i++)
                            {
                                string[] aa = dspids.Tables[0].Rows[i][0].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (aa.Length > 0)
                                {
                                    for (int k = 0; k < aa.Length; k++)
                                    {
                                        if (ProIDS.ToString().ToLower().IndexOf("," + aa[k].ToString().Trim().ToLower() + ",") <= -1)
                                        {
                                            ProIDS = ProIDS + aa[k].ToString().Trim().ToLower() + ",";
                                        }
                                    }

                                }
                            }


                            if (!String.IsNullOrEmpty(ProIDS) && ProIDS.Length > 2)
                            {
                                ProIDS = ProIDS.ToString().Remove(ProIDS.ToString().LastIndexOf(","));
                                ProIDS = ProIDS.ToString().Substring(1, ProIDS.ToString().Length - 1);
                                CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + ProIDS.ToString().Trim() + "' where couponid=" + couponid.ToString());
                            }
                            else
                            {
                                CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='0' where couponid=" + couponid.ToString());
                            }

                        }
                        else
                        {
                            CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='0' where couponid=" + couponid.ToString());

                        }

                    }


                }
            }
            BindEventList();
        }

        protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindEventList();
            Response.Redirect("EventList.aspx?st=" + ddlSearch.SelectedValue.ToString() + "&txtse=" + Server.UrlEncode(txtSearch.Text.ToString()) + "");
        }
    }
}