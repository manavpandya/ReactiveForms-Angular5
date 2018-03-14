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
    public partial class CouponsList : Solution.UI.Web.BasePage
    {
        #region Declaration

        CouponComponent couponcomp = new CouponComponent();
        StoreComponent stac = new StoreComponent();
        public static bool isDescendcoupon = false;
        public static bool isDescendStore = false;
        public static bool isDescendexpiredate = false;
        public static bool isDescenddiscount = false;
        public static bool isDescenddiscountamt = false;
        Int32 isrow = 0;

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
                BindStore();
                if(Request.QueryString["st"] != null)
                {
                    ddlSearch.SelectedValue = Request.QueryString["st"].ToString();
                }
                if (Request.QueryString["txt"] != null)
                {
                    txtSearch.Text = Request.QueryString["txt"].ToString();
                }
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnshowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btndelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Coupon Code inserted successfully.', 'Message');});", true);
                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Coupon Code updated successfully.', 'Message');});", true);
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
                Response.Redirect("Coupon.aspx?CouponID=" + couponid + "&st=" + ddlSearch.SelectedValue.ToString() + "&txt=" + txtSearch.Text.ToString() + "");
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

            gridcoupon.PageIndex = 0;
            gridcoupon.DataBind();
            //if (gridcoupon.Rows.Count == isrow && isrow > 0)
            //{
            //    trBottom.Visible = true;
            //}
            //else
            //{
            //    trBottom.Visible = false;
            //}
        }


        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridcoupon.PageIndex = 0;
            gridcoupon.DataBind();
            //if (gridcoupon.Rows.Count == isrow && isrow > 0)
            //{
            //    trBottom.Visible = true;
            //}
            //else
            //{
            //    trBottom.Visible = false;
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
            ddlstore.SelectedIndex = 0;
            gridcoupon.PageIndex = 0;
            gridcoupon.DataBind();
            //if (gridcoupon.Rows.Count == isrow && isrow > 0)
            //{
            //    trBottom.Visible = true;
            //}
            //else
            //{
            //    trBottom.Visible = false;
            //}
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
                if (isDescenddiscount == false)
                {
                    ImageButton lbdisc = (ImageButton)e.Row.FindControl("sortdiscount");
                    lbdisc.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    lbdisc.AlternateText = "Ascending Order";
                    lbdisc.ToolTip = "Ascending Order";
                    lbdisc.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton lbdi = (ImageButton)e.Row.FindControl("sortdiscount");
                    lbdi.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    lbdi.AlternateText = "Descending Order";
                    lbdi.ToolTip = "Descending Order";
                    lbdi.CommandArgument = "ASC";
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

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lbexpiredate = (Label)e.Row.FindControl("lbexpiredate");

                lbexpiredate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(lbexpiredate.Text));
                Literal ltStatusColor = (Literal)e.Row.FindControl("ltStatusColor");
                HiddenField hdncouponid = (HiddenField)e.Row.FindControl("hdncouponid");
                string todaydate = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //int result = string.Compare(lbexpiredate.Text, todaydate);
                Label lbupdateddate = (Label)e.Row.FindControl("lbupdateddate");
                Label lbtype = (Label)e.Row.FindControl("lbtype");
                Label lbStatus = (Label)e.Row.FindControl("lbStatus");
                Label lbupdatedby = (Label)e.Row.FindControl("lbupdatedby");
                DataSet ds = new DataSet();
                string strStartdate = "";
                if (ddlSearch.SelectedValue.ToString() == "2")
                {

                    ds = CommonComponent.GetCommonDataSet("SELECT isnull(IsValidforBuy1get1,0) as IsValidforBuy1get1,isnull(IsValidforNewArrival,0) as IsValidforNewArrival,isnull(Iscouponactive,0) as Iscouponactive ,isnull(IsValidforSalesClearance,0) as IsValidforSalesClearance,isnull(tb_Coupons.UpdatedOn,tb_Coupons.CreatedOn) as UpdatedOn,tb_Admin.FirstName+' '+tb_Admin.LastName as Updatedby,CouponStartdate FROM tb_Coupons LEFT OUTER JOIN tb_Admin on isnull(tb_Coupons.UpdatedBy,tb_Coupons.CreatedBy)=tb_Admin.AdminID WHERE CouponID=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CouponID")) + " and cast(isnull(CouponStartdate,getdate()) as date)> cast(getdate() as date) and  cast(isnull(ExpirationDate,getdate()) as date)> cast(getdate() as date)");
                }
                else
                {
                    ds = CommonComponent.GetCommonDataSet("SELECT isnull(IsValidforBuy1get1,0) as IsValidforBuy1get1,isnull(IsValidforNewArrival,0) as IsValidforNewArrival,isnull(Iscouponactive,0) as Iscouponactive ,isnull(IsValidforSalesClearance,0) as IsValidforSalesClearance,isnull(tb_Coupons.UpdatedOn,tb_Coupons.CreatedOn) as UpdatedOn,tb_Admin.FirstName+' '+tb_Admin.LastName as Updatedby,CouponStartdate FROM tb_Coupons LEFT OUTER JOIN tb_Admin on isnull(tb_Coupons.UpdatedBy,tb_Coupons.CreatedBy)=tb_Admin.AdminID WHERE CouponID=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CouponID")) + "");

                }
                //else if (ddlSearch.SelectedValue.ToString() == "0")
                //{
                //    ds = CommonComponent.GetCommonDataSet("SELECT isnull(IsValidforBuy1get1,0) as IsValidforBuy1get1,isnull(IsValidforNewArrival,0) as IsValidforNewArrival,isnull(Iscouponactive,0) as Iscouponactive ,isnull(IsValidforSalesClearance,0) as IsValidforSalesClearance,isnull(UpdatedOn,CreatedOn) as UpdatedOn FROM tb_Coupons WHERE CouponID=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CouponID")) + " and isnull(Iscouponactive,0)=0");
                //}
                //else if (ddlSearch.SelectedValue.ToString() == "1")
                //{
                //    ds = CommonComponent.GetCommonDataSet("SELECT isnull(IsValidforBuy1get1,0) as IsValidforBuy1get1,isnull(IsValidforNewArrival,0) as IsValidforNewArrival,isnull(Iscouponactive,0) as Iscouponactive ,isnull(IsValidforSalesClearance,0) as IsValidforSalesClearance,isnull(UpdatedOn,CreatedOn) as UpdatedOn FROM tb_Coupons WHERE CouponID=" + Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CouponID")) + " and isnull(Iscouponactive,0)=1");
                //}


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["IsValidforBuy1get1"].ToString() == "1" || Convert.ToBoolean(ds.Tables[0].Rows[0]["IsValidforBuy1get1"].ToString()) == true)
                    {
                        lbtype.Text = "<span style=\"color:darkblue\">Buy 1 Get 1</span>";
                    }
                    else if (ds.Tables[0].Rows[0]["IsValidforNewArrival"].ToString() == "1" || Convert.ToBoolean(ds.Tables[0].Rows[0]["IsValidforNewArrival"].ToString()) == true)
                    {
                        lbtype.Text = "<span style=\"color:orange\">New Arrival</span>";
                    }
                    else if (ds.Tables[0].Rows[0]["IsValidforSalesClearance"].ToString() == "1" || Convert.ToBoolean(ds.Tables[0].Rows[0]["IsValidforSalesClearance"].ToString()) == true)
                    {
                        lbtype.Text = "<span style=\"color:blue\">Sales Clearance</span>";
                    }
                    else
                    {
                        lbtype.Text = "General";
                    }
                    //if (ds.Tables[0].Rows[0]["Iscouponactive"].ToString() == "1" || Convert.ToBoolean(ds.Tables[0].Rows[0]["Iscouponactive"].ToString()) == true)
                    //{
                    //    lbStatus.Text = "<span style=\"color:green\">Active</span>";
                    //}
                    //else
                    //{
                    //    lbStatus.Text = "<span style=\"color:red\">In Active</span>";
                    //}
                    lbupdateddate.Text = ds.Tables[0].Rows[0]["UpdatedOn"].ToString();
                    lbupdatedby.Text = ds.Tables[0].Rows[0]["Updatedby"].ToString();
                    try
                    {
                        strStartdate = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["CouponStartdate"].ToString()));
                    }
                    catch { }
                    // isrow++;

                }
                else
                {
                    if (ddlSearch.SelectedValue.ToString() == "2")
                    {
                        e.Row.Visible = false;
                    }
                    else
                    {
                        //  isrow++;
                    }
                }
                //else
                //{
                //    e.Row.Visible = false;
                //}
                //if (result == 0)
                //{
                //    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:green'></div>&nbsp;";
                //}
                //else if (result == -1)
                //{
                //    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";

                //}
                //else if (result == 1)
                //{
                //    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:green'></div>&nbsp;";

                //}


                try
                {

                    if (Convert.ToDateTime(lbexpiredate.Text) >= Convert.ToDateTime(todaydate) && Convert.ToDateTime(DateTime.Now.Date) >= Convert.ToDateTime(strStartdate))
                    {
                        Int32 cid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Iscouponactive,0) FROM tb_Coupons WHERE CouponID=" + hdncouponid.Value.ToString() + ""));
                        if (cid == 1)
                        {
                            ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:green'></div>&nbsp;";
                            if (ddlSearch.SelectedValue.ToString() == "")
                            {

                            }
                            else if (ddlSearch.SelectedValue.ToString() == "0")
                            {
                                e.Row.Visible = false;
                                //  isrow--;
                            }
                            else if (ddlSearch.SelectedValue.ToString() == "1")
                            {

                            }
                            else
                            {

                            }
                            lbStatus.Text = "<span style=\"color:green\">Active</span>";
                        }
                        else
                        {
                            ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";
                            if (ddlSearch.SelectedValue.ToString() == "")
                            {

                            }
                            else if (ddlSearch.SelectedValue.ToString() == "0")
                            {

                            }
                            else if (ddlSearch.SelectedValue.ToString() == "1")
                            {
                                e.Row.Visible = false;
                                //  isrow--;
                            }
                            else
                            {

                            }
                            lbStatus.Text = "<span style=\"color:red\">In Active</span>";
                        }



                    }
                    else
                    {
                        ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";
                        if (ddlSearch.SelectedValue.ToString() == "")
                        {

                        }
                        else if (ddlSearch.SelectedValue.ToString() == "0")
                        {

                        }
                        else if (ddlSearch.SelectedValue.ToString() == "1")
                        {
                            e.Row.Visible = false;
                            // isrow--;
                        }
                        else
                        {

                        }
                        lbStatus.Text = "<span style=\"color:red\">In Active</span>";
                    }
                }
                catch { }
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
                    else if (lb.ID == "sortexpire")
                    {
                        isDescendexpiredate = false;
                    }
                    else if (lb.ID == "sortdiscount")
                    {
                        isDescenddiscount = false;
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
                    else if (lb.ID == "sortexpire")
                    {
                        isDescendexpiredate = true;
                    }
                    else if (lb.ID == "sortdiscount")
                    {
                        isDescenddiscount = true;
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
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)gridcoupon.Rows[i].FindControl("hdncouponid");
                CheckBox chk = (CheckBox)gridcoupon.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    tb_Coupons tblcoupon = null;
                    tblcoupon = couponcomp.Getcoupon(Convert.ToInt16(hdn.Value));
                    tblcoupon.Deleted = true;
                    couponcomp.Deletecoupon(tblcoupon);
                }
            }
            gridcoupon.DataBind();
        }
    }
}