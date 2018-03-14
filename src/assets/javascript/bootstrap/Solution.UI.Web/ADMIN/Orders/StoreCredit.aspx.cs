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
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class StoreCredit : Solution.UI.Web.BasePage
    {
        int couponsID = 0;
        StoreComponent stac = new StoreComponent();
        tb_Coupons tb_coupon = new tb_Coupons();
        CouponComponent couponcomp = new CouponComponent();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Text = "";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                BindStore();
                DateTime dt = DateTime.Now.Date.AddYears(1);
                txtExpDate.Text = dt.ToString();
                txtCouponCode.Text = GenerateRandom();
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    couponsID = Convert.ToInt32(Request.QueryString["ID"].ToString().Trim());
                    BindData(couponsID);
                    ddlStore.Enabled = false;
                    lblTitle.Text = "Update Coupon";
                }
                else
                {
                    DataSet dsCustom = new DataSet();
                    dsCustom = CommonComponent.GetCommonDataSet("SELECT OrderedCustomerID,CustomerEMail FROM tb_ReturnItem WHERE ReturnItemID=" + Convert.ToInt32(Request.QueryString["RMA"].ToString()) + "");
                    if (dsCustom.Tables[0].Rows.Count > 0)
                    {
                        txtEmail.Text = Convert.ToString(dsCustom.Tables[0].Rows[0]["CustomerEMail"]);
                        txtValidForCust.Text = Convert.ToString(dsCustom.Tables[0].Rows[0]["OrderedCustomerID"]);
                    }
                    lblTitle.Text = "Add Coupon";
                    DataSet dsorder = new DataSet();
                    dsorder = GetOrderDetails();
                    trOrderDetails.Visible = true;
                    grdRMARequestList.DataSource = dsorder;
                    grdRMARequestList.DataBind();
                }
            }
        }

        /// <summary>
        /// Binds the Data of Store Credit
        /// </summary>
        /// <param name="ID">int ID</param>
        public void BindData(int ID)
        {
            DataSet Ds = new DataSet();
            Ds = CommonComponent.GetCommonDataSet("Select * From tb_Coupons Where CouponsID=" + ID + "");
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                Decimal decGlobal = Decimal.Zero;
                txtCouponCode.Text = Ds.Tables[0].Rows[0]["CouponsCode"].ToString();
                txtDescription.Text = Ds.Tables[0].Rows[0]["Description"].ToString();
                string date = String.Format("{0:d}", Convert.ToDateTime(Ds.Tables[0].Rows[0]["ExpirationDate"].ToString()));
                txtExpDate.Text = Convert.ToDateTime(date.ToString()).ToString();

                Decimal.TryParse(Ds.Tables[0].Rows[0]["DiscountPercent"].ToString(), out decGlobal);
                txtDiscountPercent.Text = decGlobal.ToString("f2");
                if (Convert.ToBoolean(Ds.Tables[0].Rows[0]["ExpiresonFirstUseByAnyCustomer"].ToString()) == true)
                    rdbCoupons.SelectedValue = "ExpiresonFirstUseByAnyCustomer";
                else if (Convert.ToBoolean(Ds.Tables[0].Rows[0]["ExpiresAfterOneUsageByEachCustomer"].ToString()) == true)
                    rdbCoupons.SelectedValue = "ExpiresAfterOneUsageByEachCustomer";
                else
                {
                    rdbCoupons.SelectedValue = "ExpiredAfterNUses";
                    trUsers.Visible = true;
                    txtNUses.Text = Ds.Tables[0].Rows[0]["ExpiredAfterNUses"].ToString();
                }
                txtDiscoutAmnt.Text = Convert.ToDouble((Ds.Tables[0].Rows[0]["DiscountAmount"].ToString())).ToString();
                string TempValid = Ds.Tables[0].Rows[0]["ValidForCategory"].ToString();
                if (TempValid == "")
                { chkAllCategories.Checked = true; txtValidForCategory.Text = ""; txtValidForCategory.Enabled = false; }
                else if (TempValid == "0")
                { chkAllCategories.Checked = false; txtValidForCategory.Text = ""; }
                else
                { chkAllCategories.Checked = false; txtValidForCategory.Text = Ds.Tables[0].Rows[0]["ValidForCategory"].ToString(); }

                TempValid = Ds.Tables[0].Rows[0]["ValidForProduct"].ToString();
                if (TempValid == "")
                { chkAllProducts.Checked = true; txtValidForProducts.Text = ""; txtValidForProducts.Enabled = false; }
                else if (TempValid == "0")
                { chkAllProducts.Checked = false; txtValidForProducts.Text = ""; }
                else
                { chkAllProducts.Checked = false; txtValidForProducts.Text = Ds.Tables[0].Rows[0]["ValidForProduct"].ToString(); }

                TempValid = Ds.Tables[0].Rows[0]["ValidForCustomer"].ToString();
                if (TempValid == "")
                { chkAllCustomer.Checked = true; txtValidForCust.Text = ""; txtValidForCust.Enabled = false; }
                else if (TempValid == "0")
                { chkAllCustomer.Checked = false; txtValidForCust.Text = ""; }
                else
                { chkAllCustomer.Checked = false; txtValidForCust.Text = Ds.Tables[0].Rows[0]["ValidForCustomer"].ToString(); }
                ddlStore.SelectedValue = Ds.Tables[0].Rows[0]["StoreID"].ToString();
                int storeID = Convert.ToInt32(ddlStore.SelectedValue);
                Session["StoreID"] = storeID;

                Decimal.TryParse(Ds.Tables[0].Rows[0]["OrderTotal"].ToString(), out decGlobal);
                txtOrderTotal.Text = decGlobal.ToString("f2");
            }
        }

        /// <summary>
        /// Gets the Order Details for Store Credit
        /// </summary>
        /// <returns>Returns the Order Details as a Dataset</returns>
        public DataSet GetOrderDetails()
        {
            string StrQ1 = "";
            if (Request.QueryString["RMA"] != null)
            {
                Int32 OrderedCustomCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select OrderedCustomCartID from tb_ReturnItem where ReturnItemID=" + Convert.ToInt32(Request.QueryString["RMA"]) + ""));
                if (OrderedCustomCartID > 0)
                {
                    StrQ1 = " and tb_OrderedShoppingCartItems.OrderedCustomCartID=" + OrderedCustomCartID + "";
                }
            }
            DataSet dsorder = new DataSet();
            try
            {
                dsorder = CommonComponent.GetCommonDataSet(@" SELECT dbo.tb_Order.OrderNumber, dbo.tb_OrderedShoppingCartItems.Quantity, dbo.tb_OrderedShoppingCartItems.Price, dbo.tb_Order.OrderTotal, dbo.tb_Order.OrderShippingCosts, dbo.tb_Order.OrderTax, dbo.tb_Order.OrderSubtotal,dbo.tb_OrderedShoppingCartItems.RefProductID
                                                            FROM dbo.tb_Order INNER JOIN dbo.tb_OrderedShoppingCartItems ON dbo.tb_Order.ShoppingCardID = dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID WHERE  dbo.tb_Order.OrderNumber=" + SecurityComponent.Decrypt(Request.QueryString["ono"].ToString()) + " AND dbo.tb_OrderedShoppingCartItems.RefProductID=" + Request.QueryString["Proid"].ToString() + "" + StrQ1.ToString() + "");
                if (dsorder.Tables[0].Rows.Count > 0)
                {
                    decimal Producttoal = Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Price"].ToString()) * Convert.ToDecimal(dsorder.Tables[0].Rows[0]["Quantity"].ToString());
                    //txtDiscoutAmnt.Text = String.Format("{0:0.00}", Convert.ToDecimal(Producttoal.ToString()));
                }
            }
            catch { }
            return dsorder;
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
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
            //ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            //ddlStore.SelectedIndex = 1;
        }


        /// <summary>
        ///  Generate Random coupon Code Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void generaterandom_Click(object sender, EventArgs e)
        {
            txtCouponCode.Text = GenerateRandom();
        }


        /// <summary>
        /// Radio Button selected index Changed event of Coupon
        /// </summary>
        /// <param name="sender"> sender Object</param>
        /// <param name="e">Event Argument  e</param>
        protected void rdbCoupons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbCoupons.SelectedValue.ToString() == "ExpiredAfterNUses")
                trUsers.Visible = true;
            else
                trUsers.Visible = false;
        }

        /// <summary>
        /// Generates  random code for Coupon  Code.
        /// </summary>
        /// <returns>Returns the random code for Coupon  Code</returns>
        private string GenerateRandom()
        {
            string strRandom = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16);
            string strGet = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT SerialNumber FROM tb_GiftCard WHERE SerialNumber='" + strRandom.ToString() + "'"));
            if (!String.IsNullOrEmpty(strGet))
            {
                GenerateRandom();
            }
            else
            {
                return strRandom;
            }
            return strRandom;
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtDiscoutAmnt.Text.Trim() == "")
                txtDiscoutAmnt.Text = "0";
            if (txtDiscountPercent.Text.Trim() == "")
                txtDiscountPercent.Text = "0";
            Regex re = new Regex(@"^(100(?:\.0{1,2})?|0*?\.\d{1,2}|\d{1,2}(?:\.\d{1,2})?)$");
            if (txtCouponCode.Text == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Insert CouponCode ');", true);
                txtCouponCode.Focus();
            }
            else if (txtExpDate.Text.ToString() == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Insert ExpirationDate ');", true);
                txtExpDate.Focus();
            }
            if (txtDiscoutAmnt.Text.Trim() == "")
                txtDiscoutAmnt.Text = "0";
            if (txtDiscountPercent.Text.Trim() == "")
                txtDiscountPercent.Text = "0";
            if (txtDiscountPercent.Text == "" && txtDiscoutAmnt.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message', 'txtDiscoutAmnt');});", true);
                return;
            }
            else if (Convert.ToDecimal(txtDiscountPercent.Text) == decimal.Zero && Convert.ToDecimal(txtDiscoutAmnt.Text) == decimal.Zero)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Amount', 'Message', 'txtDiscoutAmnt');});", true);
                return;
            }
            else if (Convert.ToDecimal(txtDiscountPercent.Text) > decimal.Zero && Convert.ToDecimal(txtDiscoutAmnt.Text) > decimal.Zero)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Discount Percent or Discount Amount', 'Message', 'txtDiscoutAmnt');});", true);
                return;
            }
            else if (rdbCoupons.SelectedValue == "ExpiredAfterNUses" && txtNUses.Text == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Insert Value');", true);
                txtNUses.Focus();
            }
            else
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit" && ddlStore.SelectedValue == Convert.ToString(Session["StoreID"]))
                {
                    if (Convert.ToDateTime(txtExpDate.Text.ToString()) >= DateTime.Today)
                    {
                        Update();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Updated Successfully', 'Message');});", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Select Valid Expiration Date... ');", true);
                    }
                }
                else
                {
                    if (Convert.ToDateTime(txtExpDate.Text.ToString()) >= DateTime.Today)
                    {
                        Insert();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Added Successfully', 'Message');});", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Expiration Date  Must Be greater Than Today...');", true);
                    }
                }
            }
        }

        /// <summary>
        /// Insert the Coupon Code
        /// </summary>
        public void Insert()
        {
            tb_Coupons tb_coupon = new tb_Coupons();
            tb_coupon = SetValue();
            tb_coupon.CreatedOn = DateTime.Now;
            tb_coupon.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_coupon.Deleted = false;

            CommonComponent.ExecuteCommonData("INSERT INTO tb_GiftCard(StoreID,SerialNumber,CustomerID,OrderNumber,InitialAmount,Balance,ExpirationDate,EmailTo,Status,RMAnumber,CreatedOn) VALUES (" + ddlStore.SelectedValue.ToString() + ",'" + txtCouponCode.Text.ToString() + "'," + txtValidForCust.Text.ToString() + "," + SecurityComponent.Decrypt(Request.QueryString["ono"].ToString()) + ",'" + txtDiscoutAmnt.Text.ToString() + "','" + txtDiscoutAmnt.Text.ToString() + "','" + txtExpDate.Text.ToString() + "','" + txtEmail.Text.ToString() + "',1," + Convert.ToInt32(Request.QueryString["RMA"].ToString()) + ",getdate())");
            SendMail(txtEmail.Text.ToString());
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "window.opener.location.href=window.opener.location.href;window.close();", true);
        }

        /// <summary>
        /// Sends the Mail
        /// </summary>
        /// <param name="toEmail">string toEmail</param>
        private void SendMail(string toEmail)
        {
            int storeid = Convert.ToInt32(AppConfig.StoreID);
            try
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                string ToID = toEmail.ToString();
                CustomerComponent objCustomer = new CustomerComponent();
                DataSet dsMailTemplate = new DataSet();
                dsMailTemplate = objCustomer.GetEmailTamplate("StoreCreditEmailTemp", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###GiftCode###", txtCouponCode.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###ExpireDate###", txtExpDate.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###TotalCredit###", "$" + txtDiscoutAmnt.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###NOTES###", txtDescription.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");

                    CommonOperations.SendMail(ToID + ";" + AppLogic.AppConfigs("MailMe_ToAddress").ToString(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }


        /// <summary>
        /// Update/Edit Coupon details
        /// </summary>
        public void Update()
        {
            tb_Coupons tb_coupon = new tb_Coupons();
            tb_coupon = SetValue();
            couponsID = Convert.ToInt32(Request.QueryString["ID"].ToString().Trim());
            Int32 isupdated = couponcomp.Updatecoupon(tb_coupon);
            if (isupdated > 0)
            {
                Response.Redirect("/Admin/Promotions/CouponsList.aspx?status=updated");
            }
            else
            {
                txtExpDate.Text = "...";
            }
        }

        /// <summary>
        /// Set Values of Coupon  after Edit Data
        /// </summary>
        /// <returns>Returns tb_Coupons Table Object</returns>
        public tb_Coupons SetValue()
        {
            tb_coupon = new tb_Coupons();
            tb_coupon.CouponCode = txtCouponCode.Text.Trim();
            tb_coupon.Description = txtDescription.Text.Trim();
            tb_coupon.ExpirationDate = Convert.ToDateTime(txtExpDate.Text.ToString());
            if (rdbCoupons.SelectedValue.ToString() == "ExpiresonFirstUseByAnyCustomer")
            {
                tb_coupon.ExpiresonFirstUseByAnyCustomer = true;
                tb_coupon.ExpiredAfterNUses = 0;
            }
            else if (rdbCoupons.SelectedValue.ToString() == "ExpiresAfterOneUsageByEachCustomer")
            {
                tb_coupon.ExpiredAfterNUses = 0;
                tb_coupon.ExpiresAfterOneUsageByEachCustomer = true;
            }
            else
            {
                if (txtNUses.Text == "")
                    txtNUses.Text = "0";
                trUsers.Visible = true;
                tb_coupon.ExpiredAfterNUses = Convert.ToInt32(txtNUses.Text.Trim());
            }
            if (txtDiscountPercent.Text.Trim() == "")
                txtDiscountPercent.Text = "0";
            tb_coupon.DiscountPercent = Convert.ToDecimal(txtDiscountPercent.Text.Trim());
            tb_coupon.DiscountAllowIncludeFreeShipping = false;
            tb_coupon.RequiredMinimumOrderTotal = 0;
            if (txtOrderTotal.Text.Trim() == "")
                txtOrderTotal.Text = "0";
            tb_coupon.OrderTotal = Convert.ToDecimal(txtOrderTotal.Text.Trim());

            if (txtDiscoutAmnt.Text.Trim() == "")
                txtDiscoutAmnt.Text = "0";
            tb_coupon.DiscountAmount = Convert.ToDecimal(txtDiscoutAmnt.Text.Trim());

            if (!chkAllCategories.Checked && txtValidForCategory.Text.Trim() == "")
                tb_coupon.ValidForCategory = "0";

            else
                tb_coupon.ValidForCategory = txtValidForCategory.Text.Trim();

            if (!chkAllCustomer.Checked && txtValidForCust.Text.Trim() == "")
                tb_coupon.ValidforCustomer = "0";

            else
                tb_coupon.ValidforCustomer = txtValidForCust.Text.Trim();

            if (!chkAllProducts.Checked && txtValidForProducts.Text.Trim() == "")
                tb_coupon.ValidforProduct = "0";

            else
                tb_coupon.ValidforProduct = txtValidForProducts.Text.Trim();
            int _StoreID = 0;
            _StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
            tb_coupon.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
            return tb_coupon;
        }
    }
}