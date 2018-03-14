using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;
using StringBuilder = System.Text.StringBuilder;
using System.Text.RegularExpressions;
using Solution.Bussines.Entities;


namespace Solution.UI.Web
{
    public partial class GiftItemPage : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            string url = "";
            if (Request.UrlReferrer != null)
            {
                url = Request.UrlReferrer.ToString();
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ProductID"] != null && Request.QueryString["ProductID"].ToString() != "")
                {
                    BindData(Convert.ToInt32(Request.QueryString["ProductID"].ToString()));

                    if (Request.QueryString["CustomCartID"] != null)
                    {
                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(Quantity,0) FROM tb_ShoppingCartItems WHERE CustomCartID='" + Request.QueryString["CustomCartID"].ToString() + "'  AND ProductID='" + Request.QueryString["ProductID"].ToString() + "'"));
                        txtQty.Text = strQty;
                        //trRecipientName.Visible = false;
                        //trRecipientEmail.Visible = false;
                        //trRecipientMessage.Visible = false;
                        btnAddToCart.ToolTip = "UPDATE CART";
                        BindGiftDetails(Request.QueryString["ProductID"].ToString());
                    }
                    else
                    {

                        txtQty.Text = "1";

                    }
                }
            }
        }

        /// <summary>
        /// Binds the Data for Gift Card Product
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        private void BindData(Int32 ProductId)
        {
            DataSet DsProduct = new DataSet();
            ProductComponent objproduct = new ProductComponent();
            DsProduct = objproduct.GetProductByIDForGift(ProductId, AppLogic.AppConfigs("StoreId").ToString());
            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                Decimal Price = Decimal.Zero;
                Decimal SalePrice = Decimal.Zero;
                if (DsProduct.Tables[0].Rows[0]["SalePrice"] != null && DsProduct.Tables[0].Rows[0]["SalePrice"].ToString() != "")
                {
                    SalePrice = Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString()), 2);
                }
                if (DsProduct.Tables[0].Rows[0]["Price"] != null && DsProduct.Tables[0].Rows[0]["Price"].ToString() != "")
                {
                    Price = Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString()), 2);
                }
                ltbreadcrmbs.Text += "<span>" + Server.HtmlEncode(DsProduct.Tables[0].Rows[0]["Name"].ToString().Trim()) + "</span>";
                litProductMainImage.Text = "<img  style=\"border:1px solid #DADADA;\" alt='" + Server.HtmlEncode(DsProduct.Tables[0].Rows[0]["Tooltip"].ToString()) + "' title='" + Server.HtmlEncode(DsProduct.Tables[0].Rows[0]["Tooltip"].ToString()) + "' src='" + GetIconImageProduct(DsProduct.Tables[0].Rows[0]["ImageName"].ToString()) + "' />";
                if (SalePrice == Decimal.Zero)
                {
                    litSalePrice.Text = Price.ToString();
                    hdnprice.Value = Price.ToString();
                }
                else
                {
                    litSalePrice.Text = SalePrice.ToString();
                    hdnprice.Value = SalePrice.ToString();
                }
            }

        }

        /// <summary>
        /// Gets the Icon Image Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the  Icon Image Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Binds the Gift Product Details.
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        protected void BindGiftDetails(string ProductId)
        {
            if (Session["MyDataSet"] != null)
            {
                DataSet MyDataset = new DataSet();
                MyDataset = (DataSet)Session["MyDataSet"];
                if (MyDataset != null && MyDataset.Tables[0].Rows.Count > 0)
                {
                    if (MyDataset.Tables[0].Select("productId=" + ProductId + "").Length > 0)
                    {
                        txtRecipientName.Text = MyDataset.Tables[0].Rows[0]["EmailName"].ToString();
                        txtRecipientEmail.Text = MyDataset.Tables[0].Rows[0]["EmailTo"].ToString();
                        txtMessage.Text = MyDataset.Tables[0].Rows[0]["EmailMessage"].ToString();
                    }
                }
            }
        }

        /// <summary>
        ///  Add to Cart Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddToCart_Click(object sender, ImageClickEventArgs e)
        {
            ShoppingCartComponent objShopping = new ShoppingCartComponent();

            bool IsRestricted = true;
            IsRestricted = CheckCustomerIsRestricted();
            int Qty = Convert.ToInt32(txtQty.Text);
            hdnQuantity.Value = txtQty.Text;
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }
            }


            if (hdnQuantity.Value.ToString().Trim() != "" && Convert.ToInt32(hdnQuantity.Value.ToString()) > 0)
            {
                if (Request.QueryString["ProductID"] != null)
                {
                    if (CheckInventory(Convert.ToInt32(Request.QueryString["ProductID"].ToString()), Convert.ToInt32(hdnQuantity.Value.ToString())))
                    {

                        DataSet MyDataset = new DataSet();
                        System.Text.RegularExpressions.Regex reEmail2 = new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                        if (Session["MyDataSet"] == null)
                        {
                            MyDataset.Tables.Add("Mytable");
                            DataColumn col3 = new DataColumn("productId", typeof(string));
                            MyDataset.Tables[0].Columns.Add(col3);
                            DataColumn col4 = new DataColumn("EmailName", typeof(string));
                            MyDataset.Tables[0].Columns.Add(col4);
                            DataColumn col5 = new DataColumn("EmailTo", typeof(string));
                            MyDataset.Tables[0].Columns.Add(col5);
                            DataColumn col6 = new DataColumn("EmailMessage", typeof(string));
                            MyDataset.Tables[0].Columns.Add(col6);
                            DataColumn col7 = new DataColumn("Quantity", typeof(string));
                            MyDataset.Tables[0].Columns.Add(col7);
                            MyDataset.AcceptChanges();
                        }
                        else
                        {
                            MyDataset = (DataSet)Session["MyDataSet"];
                        }

                        string recipientname = txtRecipientName.Text.ToString().Trim();
                        string emailto = txtRecipientEmail.Text.ToString().Trim();
                        string emailmsg = txtMessage.Text.ToString().Trim();
                        string giftcardid = Request.QueryString["ProductID"].ToString();
                        int TotalQty = Convert.ToInt32(txtQty.Text.ToString().Trim());



                        string retval = string.Empty;



                        if (MyDataset != null && MyDataset.Tables.Count > 0)
                        {
                            if (recipientname.Trim().Length > 0 && emailto.Trim().Length > 0)
                            {
                                if (!reEmail2.IsMatch(emailto.Trim()))
                                {
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msgemail", "alert('Please enter valid recipient's E-Mail Address.');document.getElementById('ContentPlaceHolder1_txtRecipientEmail').focus();", true);
                                    return;
                                }
                                else
                                {

                                    try
                                    {
                                        DataRow[] resultQty = MyDataset.Tables[0].Select("productId=" + Convert.ToString(Request.QueryString["ProductID"]) + " and EmailTo='" + txtRecipientEmail.Text.ToString().Trim().Replace("'", "''") + "'");
                                        if (resultQty.Length > 0)
                                        {
                                            int RowID = MyDataset.Tables[0].Rows.IndexOf(resultQty[0]);
                                            if (Request.QueryString["CustomCartID"] == null)
                                            {
                                                int TempQty = Convert.ToInt32(TotalQty + Convert.ToInt32(resultQty[0]["Quantity"]));
                                                MyDataset.Tables[0].Rows[RowID]["Quantity"] = TempQty.ToString();
                                                TotalQty = TempQty;
                                                MyDataset.AcceptChanges();
                                            }
                                            else
                                            {
                                                MyDataset.Tables[0].Rows[RowID]["EmailName"] = recipientname.ToString().Trim().Replace("'", "''");
                                                MyDataset.Tables[0].Rows[RowID]["EmailTo"] = emailto.ToString().Trim();
                                                MyDataset.Tables[0].Rows[RowID]["EmailMessage"] = emailmsg.ToString().Trim().Replace("'", "''");
                                                MyDataset.Tables[0].Rows[RowID]["Quantity"] = Convert.ToString(TotalQty);
                                                MyDataset.AcceptChanges();
                                            }
                                        }
                                        else
                                        {
                                            DataRow dr = MyDataset.Tables[0].NewRow();
                                            dr["productId"] = giftcardid.ToString();
                                            dr["EmailName"] = recipientname.ToString().Trim().Replace("'", "''");
                                            dr["EmailTo"] = emailto.ToString().Trim();
                                            dr["EmailMessage"] = emailmsg.ToString().Trim().Replace("'", "''");
                                            dr["Quantity"] = TotalQty.ToString();
                                            MyDataset.Tables[0].Rows.Add(dr);
                                            MyDataset.AcceptChanges();
                                        }
                                    }
                                    catch { }
                                }
                            }
                            Session["MyDataSet"] = MyDataset;
                        }

                        decimal price = Convert.ToDecimal(hdnprice.Value);
                        string strResult = objShopping.AddGiftItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.QueryString["ProductID"]), TotalQty, price, "", "", "Name,Email", txtRecipientName.Text.ToString().Trim().Replace(",", " ") + "," + txtRecipientEmail.Text.ToString());
                        if (strResult.ToLower() == "success")
                        {
                            if (Session["NoOfCartItems"] == null)
                            {
                                Session["NoOfCartItems"] = Qty.ToString();
                            }
                            Response.Redirect("/addtoCart.aspx");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msgqty", "alert('Not Sufficient Inventory.');document.getElementById('ContentPlaceHolder1_txtQty').focus();", true);
                    }
                }
            }
        }


        /// <summary>
        /// Check Customer is restricted or not.
        /// </summary>
        /// <returns>Returns True or False</returns>
        private bool CheckCustomerIsRestricted()
        {
            bool IsRestrictedCust = false;
            IsRestrictedCust = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT RestrictedIPID FROM tb_RestrictedIP WHERE IPAddress='" + Request.UserHostAddress.ToString() + "'"));
            return IsRestrictedCust;
        }

        /// <summary>
        /// Add Customer for Cart item Temporary
        /// </summary>
        private void AddCustomer()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Customer objCust = new tb_Customer();
            Int32 CustID = -1;
            CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Session["CustID"] = CustID.ToString();
            System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustID.ToString());
            custCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(custCookie);
        }

        /// <summary>
        /// Checks the Inventory for Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>true if sufficient Inventory ,false otherwise</returns>
        private bool CheckInventory(Int32 ProductID, Int32 Qty)
        {
            Qty = Convert.ToInt32(txtQty.Text);
            DataSet dscount = new DataSet();
            dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
            if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}