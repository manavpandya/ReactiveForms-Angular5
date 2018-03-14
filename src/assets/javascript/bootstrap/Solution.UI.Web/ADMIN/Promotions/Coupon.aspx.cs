using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class Coupon : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        tb_Coupons tb_coupon = new tb_Coupons();
        CouponComponent couponcomp = new CouponComponent();
        int StoreId = 0;

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
                BindStore();
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btncat.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-category.png";
                btncust.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-customer.png";
                btnpro.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-product.png";
                if (!string.IsNullOrEmpty(Request.QueryString["CouponID"]) && Convert.ToString(Request.QueryString["CouponID"]) != "0")
                {
                    FillCoupon(Convert.ToInt32(Request.QueryString["CouponID"]));
                    lblTitle.Text = "Edit Coupon";
                    lblTitle.ToolTip = "Edit Coupon";
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
                ddlStore.DataSource = Storelist;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
            }
            else
            {
                ddlStore.DataSource = null;
            }
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("Select Store", "0"));
        }

        /// <summary>
        /// Fills the detail of coupon according to the CouponID
        /// </summary>
        /// <param name="couponid">int couponid</param>
        private void FillCoupon(int couponid)
        {
            hdncoupon.Value = couponid.ToString();
            tb_coupon = couponcomp.Getcoupon(couponid);
            ddlStore.Enabled = false;
            txtdescription.Text = tb_coupon.Description;
            txtcode.Text = tb_coupon.CouponCode;
            string date = String.Format("{0:d}", Convert.ToDateTime(tb_coupon.ExpirationDate));
            txtexpire.Text = date;
            txtdiscountper.Text = Math.Round(Convert.ToDecimal(tb_coupon.DiscountPercent), 2).ToString();
            txtdiscountamt.Text = Math.Round(Convert.ToDecimal(tb_coupon.DiscountAmount), 2).ToString();
            chkshipdiscnt.Checked = Convert.ToBoolean(tb_coupon.DiscountAllowIncludeFreeShipping);
            Session["idspercantage"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(CategoryPercentage,'') FROM tb_Coupons WHERE CouponID=" + couponid + ""));
            Session["ids"] = tb_coupon.ValidForCategory.ToString();
            DataSet DsCoupon = new DataSet();
            DsCoupon = CommonComponent.GetCommonDataSet("select isnull(IsCategoryCoupon,0) as IsCategoryCoupon, isnull(IsValidforBuy1get1,0) as IsValidforBuy1get1,isnull(IsValidforNewArrival,0) as IsValidforNewArrival,isnull(IsValidforSalesClearance,0) as IsValidforSalesClearance,isnull(CouponStartdate,ExpirationDate) as CouponStartdate,isnull(Iscouponactive,0) as Iscouponactive,isnull(isadmincoupon,0) as isadmincoupon,isnull(IsShippingFeed,0) as IsShippingFeed,isnull(ShippingFeedCode,'') as ShippingFeedCode from tb_Coupons WHERE CouponID=" + couponid + "");
            if (DsCoupon != null && DsCoupon.Tables.Count > 0 && DsCoupon.Tables[0].Rows.Count > 0)
            {
                chkbuy1get1.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["IsValidforBuy1get1"].ToString());
                chknewarrival.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["IsValidforNewArrival"].ToString());
                chksalesclearance.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["IsValidforSalesClearance"].ToString());
                chckactive.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["Iscouponactive"].ToString());
                chkadminonly.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["isadmincoupon"].ToString());
                txtfromdate.Text = String.Format("{0:d}", Convert.ToDateTime(DsCoupon.Tables[0].Rows[0]["CouponStartdate"].ToString()));
                chkcategorycoupon.Checked = Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["IsCategoryCoupon"].ToString());
                if (Convert.ToBoolean(DsCoupon.Tables[0].Rows[0]["IsShippingFeed"].ToString()))
                {
                    chkfeedvisible.Checked = true;
                    trshippingfeedcode.Attributes.Add("style","display:''");
                    txtshippingfeedcode.Text = Convert.ToString(DsCoupon.Tables[0].Rows[0]["ShippingFeedCode"].ToString());
                }
                else
                {
                    chkfeedvisible.Checked = false;
                    trshippingfeedcode.Attributes.Add("style", "display:none");
                    
                }
                
            }
            
            if (tb_coupon.ExpiresonFirstUseByAnyCustomer == true)
            {
                rdiocouponvalidfor.SelectedIndex = 0;
            }
            else if (tb_coupon.ExpiresAfterOneUsageByEachCustomer == true)
            {
                rdiocouponvalidfor.SelectedIndex = 1;
            }
            else
            {
                rdiocouponvalidfor.SelectedIndex = 2;
            }
            if (rdiocouponvalidfor.SelectedIndex == 2)
            {
                usesrow.Visible = true;
                txtentervalue.Text = tb_coupon.ExpiredAfterNUses.ToString();
            }
            if (tb_coupon.ValidForCategory == "")
            {
                chkvalidforcat.Checked = true;
                txtvalidforcat.Text = tb_coupon.ValidForCategory;
                txtvalidforcat.Enabled = false;
            }
            else
            {
                chkvalidforcat.Checked = false;
                txtvalidforcat.Text = tb_coupon.ValidForCategory;
            }
            if (tb_coupon.ValidforCustomer == "")
            {
                chkvalidforcust.Checked = true;
                txtvalidforcust.Text = tb_coupon.ValidforCustomer;
                txtvalidforcust.Enabled = false;
            }
            else
            {
                chkvalidforcust.Checked = false;
                txtvalidforcust.Text = tb_coupon.ValidforCustomer;
            }
            if (tb_coupon.ValidforProduct == "")
            {
                chkvalidforprod.Checked = true;
                txtvalidforprod.Text = tb_coupon.ValidforProduct;
                txtvalidforprod.Enabled = false;
            }
            else
            {
                chkvalidforprod.Checked = false;
                txtvalidforprod.Text = tb_coupon.ValidforProduct;
            }
            if (chkcategorycoupon.Checked)
            {
                //txtdiscountper.Attributes.Add("readonly", "true");
                //txtvalidforprod.Attributes.Add("readonly", "true");
                //btnpro.Attributes.Add("disabled", "disabled");
                //chkvalidforprod.Checked = false;
                //chkvalidforprod.Attributes.Add("disabled", "true");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "DisabledProductdata", "document.getElementById('ContentPlaceHolder1_txtdiscountper').value = '1';$('#ContentPlaceHolder1_txtdiscountper').attr(\"readonly\", \"true\");$('#ContentPlaceHolder1_txtvalidforprod').attr(\"readonly\", \"true\");$('#ContentPlaceHolder1_chkvalidforcat').removeAttr(\"checked\");document.getElementById('ContentPlaceHolder1_chkvalidforcat').disabled = true;document.getElementById('ContentPlaceHolder1_btnpro').disabled = true;", true);
            }
            txtminorder.Text = Math.Round(Convert.ToDecimal(tb_coupon.RequiredMinimumOrderTotal), 2).ToString();
            StoreId = tb_coupon.tb_StoreReference.Value.StoreID;
            ddlStore.SelectedValue = StoreId.ToString();
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["st"] != null && Request.QueryString["txt"] != null)
            {
                Response.Redirect("CouponsList.aspx?st=" + Request.QueryString["st"].ToString() + "&txt=" + Request.QueryString["txt"].ToString() + "");
            }
            else
            {
                Response.Redirect("CouponsList.aspx");
            }

        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["CouponID"]) && Convert.ToString(Request.QueryString["CouponID"]) != "0")
            {
                tb_coupon = couponcomp.Getcoupon(Convert.ToInt32(Convert.ToString(Request.QueryString["CouponID"])));
                if (txtdiscountamt.Text.Trim() == "")
                    txtdiscountamt.Text = "0";
                if (txtdiscountper.Text.Trim() == "")
                    txtdiscountper.Text = "0";
                if (txtdiscountper.Text == "" && txtdiscountamt.Text == "")
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                    return;

                }


                else if (Convert.ToDecimal(txtdiscountper.Text) == decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) == decimal.Zero)
                {

                    if (chkshipdiscnt.Checked && Convert.ToDecimal(txtdiscountper.Text) == decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) == decimal.Zero)
                    {
                        txtdiscountper.Text = "0";
                        txtdiscountamt.Text = "0";
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                        return;
                    }

                }

                else if (Convert.ToDecimal(txtdiscountper.Text) > decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) > decimal.Zero)
                {
                    if (chkshipdiscnt.Checked && Convert.ToDecimal(txtdiscountper.Text) == decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) == decimal.Zero)
                    {
                        txtdiscountper.Text = "0";
                        txtdiscountamt.Text = "0";
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                        return;
                    }
                }
                else if (Convert.ToDateTime(txtexpire.Text.ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Expiration Date  Must Be greater Then or equal to Today...', 'Message');});", true);
                    return;
                }


                tb_coupon.Description = txtdescription.Text.Trim();
                tb_coupon.CouponCode = txtcode.Text.Trim();
                tb_coupon.ExpirationDate = Convert.ToDateTime(txtexpire.Text.Trim());
                tb_coupon.DiscountAmount = Convert.ToDecimal(txtdiscountamt.Text.Trim());
                tb_coupon.DiscountPercent = Convert.ToDecimal(txtdiscountper.Text.Trim());
                tb_coupon.DiscountAllowIncludeFreeShipping = Convert.ToBoolean(chkshipdiscnt.Checked);
                if (rdiocouponvalidfor.SelectedIndex == 0)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = true;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = false;
                    tb_coupon.ExpiredAfterNUses = 0;
                }
                else if (rdiocouponvalidfor.SelectedIndex == 1)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = false;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = true;
                    tb_coupon.ExpiredAfterNUses = 0;
                }
                else if (rdiocouponvalidfor.SelectedIndex == 2)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = false;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = false;
                    if (txtentervalue.Text != null && txtentervalue.Text != "")
                        tb_coupon.ExpiredAfterNUses = Convert.ToInt32(txtentervalue.Text);
                    else
                        tb_coupon.ExpiredAfterNUses = 0;
                }
                if (!chkvalidforcat.Checked && txtvalidforcat.Text.Trim() == "")
                    tb_coupon.ValidForCategory = "";
                else
                    tb_coupon.ValidForCategory = txtvalidforcat.Text.Trim();
                if (!chkvalidforcust.Checked && txtvalidforcust.Text.Trim() == "")
                    tb_coupon.ValidforCustomer = "";
                else
                    tb_coupon.ValidforCustomer = txtvalidforcust.Text.Trim();
                if (!chkvalidforprod.Checked && txtvalidforprod.Text.Trim() == "")
                    tb_coupon.ValidforProduct = "";
                else
                    tb_coupon.ValidforProduct = txtvalidforprod.Text.Trim();
                if (txtminorder.Text != null && txtminorder.Text != "")
                {
                    tb_coupon.RequiredMinimumOrderTotal = Convert.ToDecimal(txtminorder.Text.Trim());
                }
                else
                    tb_coupon.RequiredMinimumOrderTotal = 0;
                tb_coupon.Deleted = false;
                tb_coupon.UpdatedOn = DateTime.Now;
                tb_coupon.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tb_coupon.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);

                Int32 isupdated = couponcomp.Updatecoupon(tb_coupon);
                Int32 IsValidforBuy1get1 = 0;
                Int32 IsValidforNewArrival = 0;
                Int32 IsValidforSalesClearance = 0;
                if (chkbuy1get1.Checked)
                {
                    IsValidforBuy1get1 = 1;
                }
                if (chknewarrival.Checked)
                {
                    IsValidforNewArrival = 1;
                }
                if (chksalesclearance.Checked)
                {
                    IsValidforSalesClearance = 1;
                }
                Int32 Active = 0;
                if (chckactive.Checked)
                {
                    Active = 1;
                }
                Int32 ischkadminonly = 0;
                if (chkadminonly.Checked)
                {
                    ischkadminonly = 1;
                }
                Int32 IsCategoryCoupon = 0;
                if (chkcategorycoupon.Checked)
                {
                    IsCategoryCoupon = 1;
                }
                string strcode = "";
                Int32 chkfeedvisiblestat = 0;
                if(chkfeedvisible.Checked)
                {
                    chkfeedvisiblestat = 1;
                    strcode = txtshippingfeedcode.Text.ToString();
                }

                CommonComponent.ExecuteCommonData("UPDATE tb_Coupons SET  IsShippingFeed=" + chkfeedvisiblestat + ",ShippingFeedCode='" + strcode.Replace("'", "''") + "', IsCategoryCoupon=" + IsCategoryCoupon + ", IsValidforBuy1get1=" + IsValidforBuy1get1 + ",IsValidforNewArrival=" + IsValidforNewArrival + ",IsValidforSalesClearance=" + IsValidforSalesClearance + ",CouponStartdate='" + txtfromdate.Text.ToString() + "',Iscouponactive=" + Active + ",isadmincoupon=" + ischkadminonly + " WHERE CouponID=" + isupdated + "");
                if (Session["idspercantage"] != null && isupdated > 0)
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Coupons SET CategoryPercentage='" + Session["idspercantage"].ToString() + "' WHERE CouponID=" + isupdated + "");
                    Session["idspercantage"] = null;
                }

                if (isupdated > 0)
                {
                    if (Request.QueryString["st"] != null && Request.QueryString["txt"] != null)
                    {
                        Response.Redirect("CouponsList.aspx?status=updated&st=" + Request.QueryString["st"].ToString() + "&txt=" + Request.QueryString["txt"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("CouponsList.aspx?status=updated");
                    }
                }
            }
            else
            {
                if (txtdiscountamt.Text.Trim() == "")
                    txtdiscountamt.Text = "0";
                if (txtdiscountper.Text.Trim() == "")
                    txtdiscountper.Text = "0";
                if (txtdiscountper.Text == "" && txtdiscountamt.Text == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                    return;
                }
                else if (Convert.ToDecimal(txtdiscountper.Text) == decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) == decimal.Zero)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                    return;
                }
                else if (Convert.ToDecimal(txtdiscountper.Text) > decimal.Zero && Convert.ToDecimal(txtdiscountamt.Text) > decimal.Zero)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message');});", true);
                    return;
                }
                else if (Convert.ToDateTime(txtexpire.Text.ToString()) < Convert.ToDateTime(DateTime.Now.Date))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Expiration Date  Must Be greater Then Today...', 'Message');});", true);
                    return;
                }
                tb_coupon.Description = txtdescription.Text.Trim();
                tb_coupon.CouponCode = txtcode.Text.Trim();
                tb_coupon.ExpirationDate = Convert.ToDateTime(txtexpire.Text.Trim());
                tb_coupon.DiscountAmount = Convert.ToDecimal(txtdiscountamt.Text.Trim());
                tb_coupon.DiscountPercent = Convert.ToDecimal(txtdiscountper.Text.Trim());
                if (rdiocouponvalidfor.SelectedIndex == 0)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = true;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = false;
                    tb_coupon.ExpiredAfterNUses = 0;
                }
                else if (rdiocouponvalidfor.SelectedIndex == 1)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = false;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = true;
                    tb_coupon.ExpiredAfterNUses = 0;
                }
                else if (rdiocouponvalidfor.SelectedIndex == 2)
                {
                    tb_coupon.ExpiresonFirstUseByAnyCustomer = false;
                    tb_coupon.ExpiresAfterOneUsageByEachCustomer = false;
                    if (txtentervalue.Text != null && txtentervalue.Text != "")
                        tb_coupon.ExpiredAfterNUses = Convert.ToInt32(txtentervalue.Text);
                    else
                        tb_coupon.ExpiredAfterNUses = 0;
                }
                tb_coupon.ValidForCategory = txtvalidforcat.Text.Trim();
                tb_coupon.ValidforCustomer = txtvalidforcust.Text.Trim();
                tb_coupon.ValidforProduct = txtvalidforprod.Text.Trim();
                if (txtminorder.Text != null && txtminorder.Text != "")
                {
                    tb_coupon.RequiredMinimumOrderTotal = Convert.ToDecimal(txtminorder.Text.Trim());
                }
                else
                    tb_coupon.RequiredMinimumOrderTotal = 0;
                tb_coupon.OrderTotal = 0;
                tb_coupon.DiscountAllowIncludeFreeShipping = false;
                //
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tb_coupon.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                //
                if (couponcomp.CheckDuplicate(tb_coupon))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Coupon Code with same name already exists, please specify another name...', 'Message');});", true);
                    return;
                }
                tb_coupon.CreatedOn = DateTime.Now;
                tb_coupon.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_coupon.Deleted = false;
                Int32 isadded = couponcomp.Createcoupon(tb_coupon);

                Int32 IsValidforBuy1get1 = 0;
                Int32 IsValidforNewArrival = 0;
                Int32 IsValidforSalesClearance = 0;
                if (chkbuy1get1.Checked)
                {
                    IsValidforBuy1get1 = 1;
                }
                if (chknewarrival.Checked)
                {
                    IsValidforNewArrival = 1;
                }
                if (chksalesclearance.Checked)
                {
                    IsValidforSalesClearance = 1;
                }
                Int32 Active = 0;
                if (chckactive.Checked)
                {
                    Active = 1;
                }
                Int32 ischkadminonly = 0;
                if (chkadminonly.Checked)
                {
                    ischkadminonly = 1;
                }

                string strcode = "";
                Int32 chkfeedvisiblestat = 0;
                if (chkfeedvisible.Checked)
                {
                    chkfeedvisiblestat = 1;
                    strcode = txtshippingfeedcode.Text.ToString();
                }

                CommonComponent.ExecuteCommonData("UPDATE tb_Coupons SET IsShippingFeed=" + chkfeedvisiblestat + ",ShippingFeedCode='" + strcode.Replace("'","''") + "', IsValidforBuy1get1=" + IsValidforBuy1get1 + ",IsValidforNewArrival=" + IsValidforNewArrival + ",IsValidforSalesClearance=" + IsValidforSalesClearance + ",CouponStartdate='" + txtfromdate.Text.ToString() + "',Iscouponactive=" + Active + ",isadmincoupon=" + ischkadminonly + " WHERE CouponID=" + isadded + "");


                if (Session["idspercantage"] != null && isadded > 0)
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Coupons SET CategoryPercentage='" + Session["idspercantage"].ToString() + "' WHERE CouponID=" + isadded + "");



                    Session["idspercantage"] = null;
                }
                if (isadded > 0)
                {
                    if (Request.QueryString["st"] != null && Request.QueryString["txt"] != null)
                    {
                        Response.Redirect("CouponsList.aspx?status=inserted&st=" + Request.QueryString["st"].ToString() + "&txt=" + Request.QueryString["txt"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("CouponsList.aspx?status=inserted");
                    }
                }
            }
        }

        /// <summary>
        /// Selected Index Changed Event of Radio button list
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdiocouponvalidfor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdiocouponvalidfor.SelectedIndex == 2)
                usesrow.Visible = true;
            else
                usesrow.Visible = false;
        }

        /// <summary>
        ///  Redirect Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnredirect_Click(object sender, EventArgs e)
        {
            if (Session["ids"] != null)
            {
                chkvalidforcat.Checked = false;
                txtvalidforcat.Enabled = true;
                txtvalidforcat.Text = Session["ids"].ToString();
            }
            if (Session["pids"] != null)
            {
                chkvalidforprod.Checked = false;
                txtvalidforprod.Enabled = true;
                txtvalidforprod.Text = Session["pids"].ToString();
            }
            if (Session["cids"] != null)
            {
                chkvalidforcust.Checked = false;
                txtvalidforcust.Enabled = true;
                txtvalidforcust.Text = Session["cids"].ToString();
            }
        }
    }
}