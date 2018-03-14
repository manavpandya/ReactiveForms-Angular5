using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class CustomerQuoteView : BasePage
    {
        string strSql = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnBack.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/back.png";
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "view" && Request.QueryString["ID"] != null)
                {
                    DataTable dtCart = new DataTable("QuatedProducts");
                    dtCart.Columns.Add(new DataColumn("ProductID", typeof(Int32)));
                    dtCart.Columns.Add(new DataColumn("Name", typeof(String)));
                    dtCart.Columns.Add(new DataColumn("SKU", typeof(String)));
                    dtCart.Columns.Add(new DataColumn("Options", typeof(String)));
                    dtCart.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
                    dtCart.Columns.Add(new DataColumn("Price", typeof(Decimal)));
                    dtCart.Columns.Add(new DataColumn("Notes", typeof(String)));
                    dtCart.Columns.Add(new DataColumn("VariantNames", typeof(String)));
                    dtCart.Columns.Add(new DataColumn("VariantValues", typeof(String)));

                    string QuoteNumber = Convert.ToString(CommonComponent.GetScalarCommonData("Select QuoteNumber from tb_CustomerQuote Where CustomerQuoteID ='" + Request.QueryString["ID"].ToString() + "'"));
                    if (QuoteNumber.Length > 0)
                        lblQuoteNumber.Text = QuoteNumber;
                    else lblQuoteNumber.Text = "N/A";

                    strSql = "select cq.customerid,p.sku,cqi.name,cqi.price,cqi.quantity,cqi.options,cqi.productid,IsNull(cqi.notes,'') as Notes,cqi.VariantNames,cqi.VariantValues from tb_CustomerQuoteItems cqi  " +
                    " inner join tb_CustomerQuote cq  on cq.CustomerQuoteID=cqi.CustomerQuoteID " +
                    " inner join tb_Product p  on p.ProductID=cqi.ProductID " +
                    " where cq.CustomerQuoteID= " + Convert.ToInt32(Request.QueryString["ID"]);
                    DataSet dsCustQuote = CommonComponent.GetCommonDataSet(strSql);
                    foreach (DataRow dr in dsCustQuote.Tables[0].Rows)
                    {
                        DataRow DrTemp = dtCart.NewRow();
                        DrTemp["ProductID"] = dr["ProductID"];
                        DrTemp["Name"] = dr["Name"];
                        DrTemp["SKU"] = dr["SKU"];
                        DrTemp["Options"] = dr["Options"];
                        DrTemp["Quantity"] = dr["Quantity"];
                        DrTemp["Price"] = dr["Price"];
                        DrTemp["Notes"] = dr["Notes"];
                        DrTemp["VariantNames"] = dr["VariantNames"];
                        DrTemp["VariantValues"] = dr["VariantValues"];
                        dtCart.Rows.Add(DrTemp);
                    }
                    Session["QuotedProducts"] = dtCart;
                    if (dtCart.Rows.Count > 0)
                        SetCustomerInfo(Convert.ToInt32(dsCustQuote.Tables[0].Rows[0]["CustomerID"]));
                }

                BindProductData();
            }
        }


        /// <summary>
        /// Binds the product data.
        /// </summary>
        private void BindProductData()
        {

            if (Session["QuotedProducts"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["QuotedProducts"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvProductDisplay.DataSource = dt;
                    gvProductDisplay.DataBind();
                }
                else
                {
                    gvProductDisplay.DataSource = null;
                    gvProductDisplay.DataBind();
                }
            }
        }

        /// <summary>
        /// Sets the customer information.
        /// </summary>
        /// <param name="CustID">int CustID</param>
        private void SetCustomerInfo(Int32 CustID)
        {
            hfCustomerID.Value = CustID.ToString();
            Session["QuotedCustID"] = CustID.ToString();

            DataSet dsCustomer = new DataSet();
            dsCustomer = CommonComponent.GetCommonDataSet(@"select CustomerID, FirstName, LastName, Email, BillingAddressID, ShippingAddressID 
                        from tb_Customer where CustomerID=" + CustID);

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                lblCustomerName.Text = "" + dsCustomer.Tables[0].Rows[0]["FirstName"] + " " + dsCustomer.Tables[0].Rows[0]["LastName"];

                DataSet dsBillingAddress = new DataSet();
                dsBillingAddress = CommonComponent.GetCommonDataSet("select * from tb_Address where AddressID=" + dsCustomer.Tables[0].Rows[0]["BillingAddressID"]);
                if (dsBillingAddress != null && dsBillingAddress.Tables.Count > 0 && dsBillingAddress.Tables[0].Rows.Count > 0)
                {
                    lblBillingAddress.Text = "";
                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["FirstName"] + " " + dsBillingAddress.Tables[0].Rows[0]["LastName"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsBillingAddress.Tables[0].Rows[0]["Company"].ToString().Trim()))
                        lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["Company"] + "<br/>";

                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["Address1"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsBillingAddress.Tables[0].Rows[0]["Address2"].ToString().Trim()))
                        lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["Address2"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsBillingAddress.Tables[0].Rows[0]["Suite"].ToString().Trim()))
                        lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["Suite"] + "<br/>";

                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["City"] + "<br/>";
                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["State"] + "<br/>";
                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["ZipCode"] + "<br/>";

                    string BillingCountry = (string)(CommonComponent.GetScalarCommonData("select name from tb_Country where CountryID=" + dsBillingAddress.Tables[0].Rows[0]["Country"]));
                    lblBillingAddress.Text += BillingCountry + "<br/>";
                    lblBillingAddress.Text += dsBillingAddress.Tables[0].Rows[0]["Phone"] + "<br/>";
                }

                DataSet dsShippingAddress = new DataSet();
                dsShippingAddress = CommonComponent.GetCommonDataSet("select * from tb_Address where AddressID=" + dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]);
                if (dsShippingAddress != null && dsShippingAddress.Tables.Count > 0 && dsShippingAddress.Tables[0].Rows.Count > 0)
                {
                    lblShippingAddress.Text = "";
                    //lblShippingAddress.Text += "<b>Shipping Address : </b><br/>";
                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["FirstName"] + " " + dsShippingAddress.Tables[0].Rows[0]["LastName"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Company"].ToString().Trim()))
                        lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["Company"] + "<br/>";

                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["Address1"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Address2"].ToString().Trim()))
                        lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["Address2"] + "<br/>";

                    if (!String.IsNullOrEmpty(dsShippingAddress.Tables[0].Rows[0]["Suite"].ToString().Trim()))
                        lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["Suite"] + "<br/>";

                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["City"] + "<br/>";
                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["State"] + "<br/>";
                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["ZipCode"] + "<br/>";

                    string ShippingCountry = (string)(CommonComponent.GetScalarCommonData("select name from tb_Country where CountryID=" + dsShippingAddress.Tables[0].Rows[0]["Country"]));
                    lblShippingAddress.Text += ShippingCountry + "<br/>";
                    lblShippingAddress.Text += dsShippingAddress.Tables[0].Rows[0]["Phone"] + "<br/>";
                }
            }
            else
            {
                lblCustomerName.Text = "";
                hfCustomerID.Value = "";
                lblBillingAddress.Text = "";
                lblShippingAddress.Text = "";
            }
        }

        /// <summary>
        /// Back Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnBack_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Session["QuotedCustID"] = null;
            Session["QuotedProducts"] = null;
            Session["QuotedPids"] = null;
            Response.Redirect("CustomerQuoteList.aspx");
        }

        protected void gvProductDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblvarname = (Label)e.Row.FindControl("lblvarname");
                Label lblvarvalue = (Label)e.Row.FindControl("lblvarvalue");
                Label lblPName = (Label)e.Row.FindControl("lblPName");

                string[] variantName = lblvarname.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblvarvalue.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                
                for (int j = 0; j < variantValue.Length; j++)
                {
                    if (variantName.Length > j)
                    {
                        lblPName.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                    }
                }

            }
        }
    }
}