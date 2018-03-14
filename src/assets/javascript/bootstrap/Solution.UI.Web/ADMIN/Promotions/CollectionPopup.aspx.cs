using System;
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
    public partial class CollectionPopup : BasePage
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

                    if (mode == "product")
                    {

                        btnproductsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                        btnproductshowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";
                        btnproductaddtoselect.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-to-selection-list.png";
                        btnproductclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/pclose.png";
                        btnSaveProducts.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";
                        BindProduct();
                        getselectedproducts();
                        dvProduct.Style.Add("display", "");

                        dvProduct.Visible = true;


                    }

                }
            }
        }



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
            if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
            {
                ids = Session["selectedcpid"].ToString();
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
            Session["selectedcpid"] = ids;

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

            string ids = "";
            if (Request.QueryString["CollectionId"] != null && Request.QueryString["CollectionId"] != "")
            {
                if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
                {
                    try
                    {
                        ids = Session["selectedcpid"].ToString();
                        ids = ids.ToString().Remove(ids.ToString().LastIndexOf(","));
                        if (ids.Length > 2)
                        {
                            ids = ids.Substring(1, ids.ToString().Length - 1);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "updateids", "window.opener.document.getElementById('ContentPlaceHolder1_txtvalidforprod').innerHTML='" + ids + "';", true);
                if (!string.IsNullOrEmpty(ids) && ids.Length > 2)
                {
                    DataSet dsselectedproduct = new DataSet();
                    dsselectedproduct = CommonComponent.GetCommonDataSet("select productid,isnull(sku,'') as sku,isnull(name,'') as name from tb_product where productid in (" + ids + ") and isnull(deleted,0)=0");
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
                        Session["selectedcpid"] = null;
                    }
                }
                else
                {
                    grdselectedproduct.DataSource = null;
                    grdselectedproduct.DataBind();
                    btnSaveProducts.Visible = false;
                    Session["selectedcpid"] = null;
                }



            }
            else
            {
                grdselectedproduct.DataSource = null;
                grdselectedproduct.DataBind();
                btnSaveProducts.Visible = false;
                Session["selectedcpid"] = null;
            }
        }


        private void getselectedproducts()
        {

            string ids = "";
            if (Request.QueryString["CollectionId"] != null && Request.QueryString["CollectionId"] != "")
            {
                int couponid = Convert.ToInt32(Request.QueryString["CollectionId"].ToString());

                if (Session["pidcolection"] != null && Session["pidcolection"].ToString() != "")
                {
                    ids = Session["pidcolection"].ToString();
                }
                else
                {
                    // tb_coupon = objcoupon.Getcoupon(couponid);

                    ids = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Productids,'') as Productids from tb_Collection where CollectionId=" + couponid + ""));
                    // ids = tb_coupon.ValidforProduct;
                }

                if (!string.IsNullOrEmpty(ids) && ids.Length > 2)
                {
                    DataSet dsselectedproduct = new DataSet();
                    dsselectedproduct = CommonComponent.GetCommonDataSet("select productid,isnull(sku,'') as sku,isnull(name,'') as name from tb_product where productid in (" + ids + ") and isnull(deleted,0)=0");
                    if (dsselectedproduct != null && dsselectedproduct.Tables.Count > 0 && dsselectedproduct.Tables[0].Rows.Count > 0)
                    {
                        grdselectedproduct.DataSource = dsselectedproduct;
                        grdselectedproduct.DataBind();
                        ids = ", " + ids + ",";
                        Session["selectedcpid"] = ids;
                        btnSaveProducts.Visible = true;
                        string[] s;

                        if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
                        {
                            ids = Session["selectedcpid"].ToString();
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
                        Session["selectedcpid"] = null;
                        btnSaveProducts.Visible = false;

                    }
                }
                else
                {
                    grdselectedproduct.DataSource = null;
                    grdselectedproduct.DataBind();
                    Session["selectedcpid"] = null;
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
            string ids = "";
            if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
            {
                ids = Session["selectedcpid"].ToString();
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



        protected void grdselectedproduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    int pid = Convert.ToInt32(e.CommandArgument);
                    string ids = "";
                    if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
                    {
                        ids = Session["selectedcpid"].ToString();
                        ids = ids.ToString().Replace(", ", ",").Replace(",  ", ",");

                    }

                    if ((ids.ToString().ToLower().IndexOf("," + pid + ",") > -1))
                    {
                        ids = ids.ToString().Replace(pid + ",", "");
                        Session["selectedcpid"] = ids;
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
                    else if ((ids.ToString().ToLower().IndexOf("," + pid) > -1))
                    {
                        ids = ids.ToString().Replace("," + pid, "");
                        Session["selectedcpid"] = ids;
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
            string pids = "";
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

            //if (Session["selectedcpid"] != null && Session["selectedcpid"].ToString() != "")
            //{
            //    pids = Session["selectedcpid"].ToString();

            //}

            int length = sb.ToString().Length;
            if (length > 0)
            {
                pids = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            }
            Session["pidcolection"] = pids;
            Session["cids"] = null;
            Session["ids"] = null;
            Session["selectedcpid"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.testi();window.close();", true);
        }
    }
}