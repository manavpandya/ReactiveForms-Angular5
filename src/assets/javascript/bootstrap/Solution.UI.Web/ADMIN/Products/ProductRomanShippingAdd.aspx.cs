using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductRomanShippingAdd : BasePage
    {
        decimal Fromwidth, Towidth, Cost = decimal.Zero;


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                btnSaveshipping.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancelshipping.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                //  bindstore();

                if (!string.IsNullOrEmpty(Request.QueryString["RomanshippingId"]) && Convert.ToString(Request.QueryString["RomanshippingId"]) != "0")
                {

                    DataSet dsromanshipping = CommonComponent.GetCommonDataSet("select * from tb_Roman_Shipping where RomanshippingId=" + Request.QueryString["RomanshippingId"]);
                    if (!string.IsNullOrEmpty(dsromanshipping.Tables[0].Rows[0]["Fromwidth"].ToString()))
                    {
                        decimal.TryParse(dsromanshipping.Tables[0].Rows[0]["Fromwidth"].ToString(), out Fromwidth);
                        txtfromwidth.Text = Fromwidth.ToString();
                    }
                    if (!string.IsNullOrEmpty(dsromanshipping.Tables[0].Rows[0]["Towidth"].ToString()))
                    {
                        decimal.TryParse(dsromanshipping.Tables[0].Rows[0]["Towidth"].ToString(), out Towidth);
                        txttowidth.Text = Towidth.ToString();
                    }
                    if (!string.IsNullOrEmpty(dsromanshipping.Tables[0].Rows[0]["Cost"].ToString()))
                    {
                        decimal.TryParse(dsromanshipping.Tables[0].Rows[0]["Cost"].ToString(), out Cost);
                        txtcost.Text = String.Format("{0:0.00}", Cost);
                    }


                    if (!string.IsNullOrEmpty(dsromanshipping.Tables[0].Rows[0]["Active"].ToString()) && Convert.ToBoolean(dsromanshipping.Tables[0].Rows[0]["Active"].ToString()))
                    {
                        chkIsActive.Checked = true;
                    }
                    else { chkIsActive.Checked = false; }
                    lblHeader.Text = "Edit Product Roman Shipping";

                }

            }

        }
        protected void btnSaveshipping_Click(object sender, ImageClickEventArgs e)
        {
            int RomanshippingId = 0;

            decimal.TryParse(Convert.ToString(txtfromwidth.Text), out Fromwidth);
            decimal.TryParse(Convert.ToString(txttowidth.Text), out Towidth);
            decimal.TryParse(Convert.ToString(txtcost.Text), out Cost);
            string Active = Convert.ToString(chkIsActive.Checked);

            if (Request.QueryString["RomanshippingId"] != null && Request.QueryString["RomanshippingId"].ToString() != "")
            {
                int.TryParse(Request.QueryString["RomanshippingId"].ToString(), out RomanshippingId);
                CommonComponent.ExecuteCommonData("UPDATE  tb_Roman_Shipping SET Fromwidth=" + "" + Fromwidth + ", Towidth=" + Towidth + ",Cost=" + Cost + ",Active='" + Active + "' where RomanshippingId=" + RomanshippingId);
                Response.Redirect("ProductRomanShipping.aspx?status=updated");

            }
            else
            {
                CommonComponent.ExecuteCommonData("insert into tb_Roman_Shipping (Fromwidth,Towidth,Cost,Active) values ("
                    + "" + Fromwidth + "," + Towidth + "," + Cost + ",'" + Active + "')");

                Response.Redirect("ProductRomanShipping.aspx?status=inserted");

            }
        }

        protected void btnCancelshipping_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductRomanShipping.aspx");

        }
        //private void bindstore()
        //{
        //    StoreComponent objStorecomponent = new StoreComponent();
        //    var storeDetail = objStorecomponent.GetStore();
        //    if (storeDetail.Count > 0 && storeDetail != null)
        //    {
        //        ddlStore.DataSource = storeDetail;
        //        ddlStore.DataTextField = "StoreName";
        //        ddlStore.DataValueField = "StoreID";
        //        ddlStore.DataBind();
        //        ddlStore.SelectedIndex = 0;
        //    }
        //    if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]) && Convert.ToString(Request.QueryString["StoreID"]) != "0")
        //    {
        //        ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();

        //    }
        //    else if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
        //    {
        //        ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
        //        AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
        //    }
        //    else
        //        AppConfig.StoreID = 1;
        //}
    }

}
