using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.RMA.HPDAmazon
{
    public partial class ReturnItem : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnsubmit.UniqueID;
            if (!IsPostBack)
            {
                txtOrderNumber.Focus();
                UserControl userleft = (UserControl)Page.Master.FindControl("leftmenu");
                if (userleft != null)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl hideleftmenu = (System.Web.UI.HtmlControls.HtmlGenericControl)userleft.FindControl("hideleftmenu");
                    if (hideleftmenu != null)
                    {
                        hideleftmenu.Visible = false;
                    }
                }
                if (Request.QueryString["strid"] != null)
                {
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["strid"].ToString());
                    AppLogic.ApplicationStart();
                }
            }
            if (Request.QueryString["CUSTID"] != null)
            {
                Session["CID"] = Request.QueryString["CUSTID"].ToString();
                Response.Redirect("ViewRMARecentOrders.aspx");
            }
        }

        /// <summary>
        /// Submit Return Item details
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtCodeshow.Text != this.Session["CaptchaImageText"].ToString())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CaptchaAlert", "<script type='text/javascript'>alert('Incorrect Verification Code, try again.');</script>");
                this.txtCodeshow.Text = "";
                txtCodeshow.Focus();
            }
            else
            {
                string str = "";
                int InputCount = 0; string WhereClause = "";
                int custid = 0;
                string storid = "1";
                if (Request.QueryString["strid"] != null)
                {
                    storid = Request.QueryString["strid"].ToString();
                }
                else
                {
                    try
                    {
                        int iorder = Convert.ToInt32(txtOrderNumber.Text.Trim());
                        storid = Convert.ToString(CommonComponent.GetScalarCommonData("select ISNULL(StoreId,0) as StoreId from tb_order where (ordernumber=" + txtOrderNumber.Text.Trim() + " or RefOrderID='" + txtOrderNumber.Text.Trim() + "')"));
                    }
                    catch
                    {
                        storid = Convert.ToString(CommonComponent.GetScalarCommonData("select ISNULL(StoreId,0) as StoreId from tb_order where (RefOrderID='" + txtOrderNumber.Text.Trim() + "')"));
                    }
                }
                if (txtOrderNumber.Text.Trim() != "")
                {
                    try
                    {
                        int iorder = Convert.ToInt32(txtOrderNumber.Text.Trim());
                        WhereClause += " and (ordernumber=" + txtOrderNumber.Text.Trim() + " or RefOrderID='" + txtOrderNumber.Text.Trim() + "')";
                        InputCount = InputCount + 1;
                    }
                    catch
                    {
                        WhereClause += " and (RefOrderID='" + txtOrderNumber.Text.Trim() + "')";
                        InputCount = InputCount + 1;
                    }
                }
                if (txtZipCode.Text.Trim() != "")
                {
                    if (txtZipCode.Text.Trim().IndexOf("-") > -1)
                    {
                        WhereClause += " and (BillingZip like '%" + txtZipCode.Text.Trim() + "%' or ShippingZip like '%" + txtZipCode.Text.Trim() + "%')";
                    }
                    else
                    {
                        str = " as a,(SELECT Top 1 items FROM split((SELECT Top 1 ShippingZip FROM tb_order WHERE  ShippingZip like '%" + txtZipCode.Text.Trim() + "%'  " + WhereClause + " ) ,'-') order by items ASC) as b,(SELECT Top 1 items FROM split((SELECT Top 1 ShippingZip FROM tb_order WHERE  ShippingZip like '%" + txtZipCode.Text.Trim() + "%' " + WhereClause + ") ,'-') order by items DESC) as c,(SELECT Top 1 items FROM split((SELECT Top 1 BillingZip FROM tb_order WHERE  BillingZip like '%" + txtZipCode.Text.Trim() + "%' " + WhereClause + ") ,'-') order by items ASC) as d,(SELECT Top 1 items FROM split((SELECT Top 1 BillingZip FROM tb_order WHERE  BillingZip like '%" + txtZipCode.Text.Trim() + "%' " + WhereClause + ") ,'-') order by items DESC) as e";
                        WhereClause += " and (a.ShippingZip like '%' + b.items + '%' or  a.ShippingZip like '%' + c.items + '%' or  a.billingZip like '%' + d.items + '%' or   a.billingZip like '%' + e.items + '%')  and  ( b.items ='" + txtZipCode.Text.Trim() + "' Or C.items ='" + txtZipCode.Text.Trim() + "' Or d.items ='" + txtZipCode.Text.Trim() + "' Or e.items ='" + txtZipCode.Text.Trim() + "')";
                    }
                    InputCount = InputCount + 1;
                }
                if (InputCount > 1)
                {
                    DataSet ds = new DataSet();
                    ds = CommonComponent.GetCommonDataSet("select DISTINCT CustomerID from tb_order  where storeid=" + storid + " " + WhereClause);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
                    {
                        txtCodeshow.Text = "";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", " alert('Search found more than one customer which having same criteria, Please enter your Order Number.'); ", true);
                        txtOrderNumber.Focus();
                        return;
                    }
                    custid = Convert.ToInt32(CommonComponent.GetScalarCommonData(" select top 1 CustomerID from tb_order where storeid=" + storid + " " + WhereClause));
                }
                if (custid > 0)
                {
                    Session["CID"] = custid.ToString();
                    AppConfig.StoreID = Convert.ToInt32(storid);
                    AppLogic.ApplicationStart();
                    Response.Redirect("ViewRMARecentOrders.aspx");
                }
                else
                {
                    txtCodeshow.Text = "";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", " alert('Please enter valid Order Information.'); ", true);
                    txtOrderNumber.Focus();
                }
            }
        }
    }
}