using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductShippingRange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();

                if (Request.QueryString["status"] != null)
                {
                    if (Request.QueryString["status"].ToString() == "updated")
                    {
                        //if (Session["messagedisplay"] != null || Session["messagedisplay"] != "true")
                        //{
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Updated Successfully...!', 'Message');});", true);
                        //Session["messagedisplay"] = "true";
                        //}
                    }
                    else if (Request.QueryString["status"].ToString() == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Inserted Successfully...!', 'Message');});", true);
                    }
                }


                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");

                Binddata();
            }
        }
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
            //Store is selected dynamically from menu
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();
            else
                ddlStore.SelectedIndex = 0;

        }

        private void Binddata()
        {
            try
            {
                int Storeid = Convert.ToInt32(AppConfig.StoreID);
                DataSet dsgrid = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where isnull(deleted,0)=0 and StoreID=" + Storeid + "");
                grdshirange.DataSource = dsgrid;
                grdshirange.DataBind();
                lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
            }
            catch
            {
                grdshirange.DataSource = null;
                grdshirange.DataBind();
                lbltotala.Text = "0";
            }

        }

        protected void ddlStore_SelectedIndexchange(object sender, EventArgs e)
        {
            try
            {
                if (ddlStore.SelectedItem.Value != "")
                {
                    int storeid = Convert.ToInt32(ddlStore.SelectedItem.Value);
                    AppConfig.StoreID = storeid;
                    Binddata();
                }
            }
            catch
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnShowall_Click(object sender, EventArgs e)
        {

        }
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        public string SetImage(bool _Active)
        {
            string _ReturnUrl;
            if (_Active)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
        }

        protected void grdshirange_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdshirange.PageIndex = e.NewPageIndex;
            Binddata();
        }

    }
}