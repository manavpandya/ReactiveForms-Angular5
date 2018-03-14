﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class CouponPopup : Solution.UI.Web.BasePage
    {
        #region Declaration

        CategoryComponent objCategory = new CategoryComponent();
        CustomerComponent objCustomer = new CustomerComponent();
        CouponComponent objcoupon = new CouponComponent();
        tb_Coupons tb_coupon = new tb_Coupons();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

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
                if (Request.QueryString["mode"] != null)
                {
                    string mode = Request.QueryString["mode"].ToString();
                    if (mode == "category")
                    {
                        btnSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                        btnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
                        btnAddtoselect.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-to-selection-list.png";
                        btnClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/pclose.png";
                        BindCategory();
                        dvProduct.Style.Add("display", "none");
                        dvCategory.Style.Add("display", "");
                        dvCustomer.Style.Add("display", "none");
                        dvCustomer.Visible = false;
                        dvCategory.Visible = true;
                        dvProduct.Visible = false;
                    }
                    else if (mode == "product")
                    {
                       
                        btnproductsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                        btnproductshowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
                        btnproductaddtoselect.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-to-selection-list.png";
                        btnproductclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/pclose.png";
                        btnSaveProducts.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";
                        BindProduct();
                        getselectedproducts();
                        dvProduct.Style.Add("display", "");
                        dvCategory.Style.Add("display", "none");
                        dvCategory.Visible = false;
                        dvProduct.Visible = true;
                        dvCustomer.Style.Add("display", "none");
                        dvCustomer.Visible = false;
                        
                    }
                    else if (mode == "customer")
                    {
                        btncustSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                        btncustshowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
                        btncustAddtoselection.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-to-selection-list.png";
                        btncustclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/pclose.png";
                        BindCustomer();
                        dvProduct.Style.Add("display", "none");
                        dvCategory.Style.Add("display", "none");
                        dvCategory.Visible = false;
                        dvProduct.Visible = false;
                        dvCustomer.Style.Add("display", " ");
                        dvCustomer.Visible = true;
                    }
                }
            }
        }

        #region For Category

        /// <summary>
        /// Category Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCategoryid");
                Label lblParent = (Label)e.Row.FindControl("lblParent");
                DataSet dsPname = CategoryComponent.GetParentCategoryNamebyCategoryID(Convert.ToInt16(hdnCatid.Value));

                if (dsPname != null && dsPname.Tables.Count > 0 && dsPname.Tables[0].Rows.Count > 0)
                {
                    int pCount = dsPname.Tables[0].Rows.Count;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < pCount; i++)
                    {
                        sb.Append(dsPname.Tables[0].Rows[i]["Name"].ToString() + ", ");
                    }
                    int length = sb.ToString().Length;
                    string pName = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
                    lblParent.Text = pName.ToString();
                }
            }
        }

        /// <summary>
        ///  Category Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCategory.PageIndex = e.NewPageIndex;
            BindCategory();
        }

        /// <summary>
        /// Binds the Category to the Category Gridview
        /// </summary>
        private void BindCategory()
        {
            if (Request.QueryString["StoreID"] != null)
            {
                DataSet dsCategory = new DataSet();
                if (txtsearch.Text == "")
                {
                    dsCategory = objCategory.getCategoryDetailsbyStoreId(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (txtsearch.Text != "" && txtsearch.Text != null)
                {
                    dsCategory = objCategory.getcategorydetailsbynameforsearch("%" + txtsearch.Text.Trim() + "%", Convert.ToInt32(Request.QueryString["StoreID"]));
                }

                


                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    grdCategory.DataSource = dsCategory;
                    grdCategory.DataBind();
                }
                else
                {
                    grdCategory.DataSource = null;
                    grdCategory.DataBind();
                }
                string[] s;
                string ids;
                string[] pers = { "" };
                if (Request.QueryString["couponid"] != null && Request.QueryString["couponid"] != "")
                {
                    int couponid = Convert.ToInt32(Request.QueryString["couponid"].ToString());
                    tb_coupon = objcoupon.Getcoupon(couponid);
                    ids = tb_coupon.ValidForCategory;
                    s = ids.Split(',');
                    try
                    {
                        if (Session["idspercantage"] != null && Session["idspercantage"].ToString() != "")
                        {
                            pers = Session["idspercantage"].ToString().Split(',');
                        }
                    }
                    catch
                    {

                    }
                    for (int i = 0; i < grdCategory.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdCategory.Rows[i].FindControl("chkSelect");
                        HiddenField hdncatid = (HiddenField)grdCategory.Rows[i].FindControl("hdnCategoryid");
                        TextBox txtpercentage = (TextBox)grdCategory.Rows[i].FindControl("txtpercentage");
                        for (int j = 0; j < s.Length; j++)
                        {
                            if (s[j].Trim() == hdncatid.Value)
                            {
                                chkSelect.Checked = true;
                                try
                                {
                                    if (pers.Length > 0)
                                    {
                                        txtpercentage.Text = pers[j].ToString();
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtsearch.Text.Trim() != "" && txtsearch.Text.Trim().Length > 0)
            {
                BindCategory();
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, ImageClickEventArgs e)
        {
            txtsearch.Text = "";
            BindCategory();
        }

        /// <summary>
        ///  Add to Select Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddtoselect_Click(object sender, ImageClickEventArgs e)
        {
            string ids;
            int TotalRowCount = grdCategory.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnCategoryid = null;
            TextBox txtpercentage = null;
            string Allqty = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Text.StringBuilder percantage = new System.Text.StringBuilder();
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdCategory.Rows[i].FindControl("chkSelect");
                hdnCategoryid = (HiddenField)grdCategory.Rows[i].FindControl("hdnCategoryid");
                txtpercentage = (TextBox)grdCategory.Rows[i].FindControl("txtpercentage");
                if (chkSelect.Checked == true)
                {
                    ids = hdnCategoryid.Value.ToString();
                    sb.Append(ids + ", ");
                    string sqty = txtpercentage.Text.ToString();
                    if (sqty == "")
                    {
                        sqty = "0";
                    }
                    percantage.Append(sqty.ToString() + ",");
                }
            }
            int length = sb.ToString().Length;
            ids = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            if (percantage.ToString().IndexOf(",") > -1)
            {
                Allqty = percantage.ToString().Remove(percantage.ToString().LastIndexOf(","));
            }
            Session["cids"] = null;
            Session["pids"] = null;
            Session["ids"] = ids;
            Session["idspercantage"] = Allqty;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }

        #endregion

        #region For Product

        /// <summary>
        ///  Product Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnproductsearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtproductsearch.Text != "")
                BindProduct();
        }

        /// <summary>
        ///  Product Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnproductshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtproductsearch.Text = "";
            BindProduct();
        }

        /// <summary>
        ///  Product Add to Select Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnproductaddtoselect_Click(object sender, ImageClickEventArgs e)
        {
            string pids;
            string ids = "";
            int TotalRowCount = grdProduct.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnProductid = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Session["selectedpid"] != null && Session["selectedpid"].ToString() != "")
            {
                ids = Session["selectedpid"].ToString();
                ids = ids.ToString().Replace(", ", ",").Replace(",  ", ",");

            }
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                hdnProductid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                if (chkSelect.Checked == true)
                {
                    pids = hdnProductid.Value.ToString();


                    if ((ids.ToString().ToLower().IndexOf("," + pids + ",") > -1))
                    {
                        
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ids))
                        {
                            ids = "," + pids + ",";
                        }
                        else
                        {
                            ids = ids + pids + ",";
                        }
                    }
                }
            }
            Session["selectedpid"] = ids;
            BindSelectedProduct();
        }

        /// <summary>
        ///  Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProduct.PageIndex = e.NewPageIndex;
            BindProduct();
        }

        private void BindSelectedProduct()
        {
           
            string ids="";
            if (Request.QueryString["couponid"] != null && Request.QueryString["couponid"] != "")
            {
                if (Session["selectedpid"] != null && Session["selectedpid"].ToString() != "")
                {
                    try
                    {
                        ids = Session["selectedpid"].ToString();
                        ids = ids.ToString().Remove(ids.ToString().LastIndexOf(","));
                        if (ids.Length > 2)
                        {
                            ids = ids.Substring(1, ids.ToString().Length - 1);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "updateids", "window.opener.document.getElementById('ContentPlaceHolder1_txtvalidforprod').innerHTML='" + ids + "';", true);
                if (!string.IsNullOrEmpty(ids) && ids.Length>2)
                {
                    DataSet dsselectedproduct = new DataSet();
                    dsselectedproduct = CommonComponent.GetCommonDataSet("select productid,isnull(sku,'') as sku,isnull(name,'') as name from tb_product where productid in (" + ids + ")");
                    if (dsselectedproduct != null && dsselectedproduct.Tables.Count > 0 && dsselectedproduct.Tables[0].Rows.Count > 0)
                    {
                        grdselectedproduct.DataSource = dsselectedproduct;
                        grdselectedproduct.DataBind();
                        btnSaveProducts.Visible = true;
                    }
                    else
                    {
                        grdselectedproduct.DataSource = null;
                        grdselectedproduct.DataBind();
                        btnSaveProducts.Visible = false;
                        Session["selectedpid"] = null;
                    }
                }
                else
                {
                    grdselectedproduct.DataSource = null;
                    grdselectedproduct.DataBind();
                    btnSaveProducts.Visible = false;
                    Session["selectedpid"] = null;
                }


               
            }
            else
            {
                grdselectedproduct.DataSource = null;
                grdselectedproduct.DataBind();
                btnSaveProducts.Visible = false;
                Session["selectedpid"] = null;
            }
        }


        private void getselectedproducts()
        {

            string ids = "";
            if (Request.QueryString["couponid"] != null && Request.QueryString["couponid"] != "")
            {
                int couponid = Convert.ToInt32(Request.QueryString["couponid"].ToString());

                if (Session["pids"] != null && Session["pids"].ToString()!="")
                {
                    ids = Session["pids"].ToString();
                }
                else
                {
                    tb_coupon = objcoupon.Getcoupon(couponid);

                    ids = tb_coupon.ValidforProduct;
                }
                              
                if (!string.IsNullOrEmpty(ids) && ids.Length>2)
                {
                     DataSet dsselectedproduct = new DataSet();
                    dsselectedproduct = CommonComponent.GetCommonDataSet("select productid,isnull(sku,'') as sku,isnull(name,'') as name from tb_product where productid in (" + ids + ")");
                    if (dsselectedproduct != null && dsselectedproduct.Tables.Count > 0 && dsselectedproduct.Tables[0].Rows.Count > 0)
                    {
                        grdselectedproduct.DataSource = dsselectedproduct;
                        grdselectedproduct.DataBind();
                        ids = ", " + ids + ",";
                        Session["selectedpid"] = ids;
                        btnSaveProducts.Visible = true;
                        string[] s;
                      
                        if (Session["selectedpid"] != null && Session["selectedpid"].ToString() != "")
                        {
                            ids = Session["selectedpid"].ToString();
                            ids = ids.ToString().Replace(", ", ",").Replace(",  ", ",");
                        }
                        s = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int rocount = grdProduct.Rows.Count;
                        for (int i = 0; i < rocount; i++)
                        {
                            CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                            HiddenField hdnprodid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                            for (int j = 0; j < s.Length; j++)
                            {
                                if (s[j].Trim() == hdnprodid.Value)
                                {
                                    chkSelect.Checked = true;
                                }
                            }
                        }
                       
                    }
                    else
                    {

                        grdselectedproduct.DataSource = null;
                        grdselectedproduct.DataBind();
                        Session["selectedpid"] = null;
                        btnSaveProducts.Visible = false;
                        
                    }
                }
                else
                {
                    grdselectedproduct.DataSource = null;
                    grdselectedproduct.DataBind();
                    Session["selectedpid"] = null;
                    btnSaveProducts.Visible = false;
                    
                }
               


              
            }
        }

        /// <summary>
        /// Binds the Products to the Product Grid view
        /// </summary>
        private void BindProduct()
        {
            DataSet dsProduct = null;


            if (rdosearch.SelectedValue.ToString() == "1")
            {
                if (Request.QueryString["StoreID"] != null && txtproductsearch.Text.Trim() == "" && txtproductsearch.Text.Trim().Length <= 0)
                {
                    dsProduct = ProductComponent.GetProductByStoreID(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (Request.QueryString["StoreID"] != null && txtproductsearch.Text.Trim() != "" && txtproductsearch.Text.Trim().Length >= 0)
                {
                    dsProduct = ProductComponent.GetProductBySearch(Convert.ToInt32(Request.QueryString["StoreID"]), txtproductsearch.Text.Trim());
                }

            }
            else
            {
                if (Request.QueryString["StoreID"] != null && txtproductsearch.Text.Trim() == "" && txtproductsearch.Text.Trim().Length <= 0)
                {
                    dsProduct = ProductComponent.GetProductByStoreID(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else if (Request.QueryString["StoreID"] != null && txtproductsearch.Text.Trim() != "" && txtproductsearch.Text.Trim().Length >= 0)
                {
                    //dsProduct = ProductComponent.GetProductBySearch(Convert.ToInt32(Request.QueryString["StoreID"]), txtproductsearch.Text.Trim());

                    dsProduct = CommonComponent.GetCommonDataSet("Exec usp_IndexPageConfig " + Convert.ToInt32(Request.QueryString["StoreID"]) + ",'" + txtproductsearch.Text.Trim() + "',0,0,6");
                }
            }
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdProduct.DataSource = dsProduct;
                grdProduct.DataBind();

                
            }
            else
            {
                grdProduct.DataSource = null;
                grdProduct.DataBind();
               
            }
            string[] s;
            string ids="";
            if (Session["selectedpid"] != null && Session["selectedpid"].ToString() != "")
            {
                ids = Session["selectedpid"].ToString();
                ids = ids.ToString().Replace(", ", ",").Replace(",  ", ",");
            }
            s = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int rocount = grdProduct.Rows.Count;
            for (int i = 0; i < rocount; i++)
            {
                CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                HiddenField hdnprodid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j].Trim() == hdnprodid.Value)
                    {
                        chkSelect.Checked = true;
                    }
                }
            }
           
        }

        #endregion

        #region For Customer

        /// <summary>
        /// CustomerSearch button Click Event
        /// </summary>
        protected void btncustSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtcustsearch.Text != "")
                BindCustomer();
        }

        /// <summary>
        /// CustomerShowall button Click Event
        /// </summary>
        protected void btncustshowall_Click(object sender, ImageClickEventArgs e)
        {
            txtcustsearch.Text = "";
            BindCustomer();
        }

        /// <summary>
        /// Customer Add to selection button Click Event
        /// </summary>
        protected void btncustAddtoselection_Click(object sender, ImageClickEventArgs e)
        {
            string cids;
            int TotalRowCount = grdCustomer.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnCustomerid = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < TotalRowCount; i++)
            {
                chkSelect = (CheckBox)grdCustomer.Rows[i].FindControl("chkSelect");
                hdnCustomerid = (HiddenField)grdCustomer.Rows[i].FindControl("hdncustomerid");
                if (chkSelect.Checked == true)
                {
                    cids = hdnCustomerid.Value.ToString();
                    sb.Append(cids + ", ");
                }
            }
            int length = sb.ToString().Length;
            cids = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            Session["cids"] = cids;
            Session["pids"] = null;
            Session["ids"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }

        /// <summary>
        /// Binds the customers to the customer gridview
        /// </summary>
        private void BindCustomer()
        {
            DataSet dsCustomer = null;
            if (Request.QueryString["StoreID"] != null && txtcustsearch.Text.Trim() == "" && txtcustsearch.Text.Trim().Length <= 0)
            {
                dsCustomer = objCustomer.GetCustomerDetailsbyStoreid(Convert.ToInt32(Request.QueryString["StoreID"]));
            }
            else if (Request.QueryString["StoreID"] != null && txtcustsearch.Text.Trim() != "" && txtcustsearch.Text.Trim().Length >= 0)
            {

                if (rdocustomer.SelectedIndex == 0)
                {
                    dsCustomer = objCustomer.GetCustomerDetailsbysearchvalue(Convert.ToInt32(Request.QueryString["StoreID"]), txtcustsearch.Text.Trim());
                }
                else
                {

                    dsCustomer = CommonComponent.GetCommonDataSet("SELECT * from tb_Customer where isnull(Deleted,0) != 1 and isnull(Active,0) = 1 and StoreID=" + Request.QueryString["StoreID"].ToString() + " and  Email in (SELECT items from dbo.Split('" + txtcustsearch.Text.Trim().Replace("'", "''") + "',',')) order by FirstName asc  ");
                }
               // dsCustomer = objCustomer.GetCustomerDetailsbysearchvalue(Convert.ToInt32(Request.QueryString["StoreID"]), txtcustsearch.Text.Trim());
            }

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                grdCustomer.DataSource = dsCustomer;
                grdCustomer.DataBind();
            }
            else
            {
                grdCustomer.DataSource = null;
                grdCustomer.DataBind();
            }
            int rocount = grdCustomer.Rows.Count;
            string[] s;
            string ids;
            if (Request.QueryString["couponid"] != null && Request.QueryString["couponid"] != "")
            {
                int couponid = Convert.ToInt32(Request.QueryString["couponid"].ToString());
                tb_coupon = objcoupon.Getcoupon(couponid);
                ids = tb_coupon.ValidforCustomer;
                s = ids.Split(',');
                for (int i = 0; i < rocount; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdCustomer.Rows[i].FindControl("chkSelect");
                    HiddenField hdnCustid = (HiddenField)grdCustomer.Rows[i].FindControl("hdncustomerid");
                    for (int j = 0; j < s.Length; j++)
                    {
                        if (s[j].Trim() == hdnCustid.Value)
                        {
                            chkSelect.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Customer Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomer.PageIndex = e.NewPageIndex;
            BindCustomer();
        }

        #endregion

        protected void grdselectedproduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    int pid = Convert.ToInt32(e.CommandArgument);
                    string ids = "";
                    if (Session["selectedpid"] != null && Session["selectedpid"].ToString()!="")
                    {
                      ids=  Session["selectedpid"].ToString();
                      ids = ids.ToString().Replace(", ", ",").Replace(",  ",",");
                     
                    }

                    if ( (ids.ToString().ToLower().IndexOf("," + pid + ",") > -1))
                    {
                        ids = ids.ToString().Replace(pid + ",", "");
                        Session["selectedpid"] = ids;
                        int rocount = grdProduct.Rows.Count;
                        for (int i = 0; i < rocount; i++)
                        {
                            CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                            HiddenField hdnprodid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                            for (int j = 0; j < grdProduct.Rows.Count; j++)
                            {
                                if (Convert.ToString(pid) ==hdnprodid.Value)
                                {
                                    chkSelect.Checked = false;
                                }
                            }
                        }

                        BindSelectedProduct();
                    }
                    else if ((ids.ToString().ToLower().IndexOf("," + pid ) > -1))
                    {
                        ids = ids.ToString().Replace(","+pid, "");
                        Session["selectedpid"] = ids;
                        int rocount = grdProduct.Rows.Count;
                        for (int i = 0; i < rocount; i++)
                        {
                            CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                            HiddenField hdnprodid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                            for (int j = 0; j < grdProduct.Rows.Count; j++)
                            {
                                if (Convert.ToString(pid) == hdnprodid.Value)
                                {
                                    chkSelect.Checked = false;
                                }
                            }
                        }
                        BindSelectedProduct();
                    }
                  

                   
                    

                }
            }
            catch { }
        }

        protected void btnSaveProducts_Click(object sender, ImageClickEventArgs e)
        {
            string pids="";
            int TotalRowCount = grdProduct.Rows.Count;
            CheckBox chkSelect = null;
            HiddenField hdnProductid = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < grdselectedproduct.Rows.Count; i++)
            {

                hdnProductid = (HiddenField)grdselectedproduct.Rows[i].FindControl("hdnProductid");
               
                    pids = hdnProductid.Value.ToString();
                    sb.Append(pids + ", ");
               
            }

            //if (Session["selectedpid"] != null && Session["selectedpid"].ToString() != "")
            //{
            //    pids = Session["selectedpid"].ToString();

            //}

            int length = sb.ToString().Length;
            if (length > 0)
            {
                pids = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            }
            Session["pids"] = pids;
            Session["cids"] = null;
            Session["ids"] = null;
            Session["selectedpid"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }
    }
}