using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class ViewCustomerDetail : BasePage
    {
         #region Declaration
        public int CustomerID = 0;
        decimal FinalOrderSubtotalTemp = 0;
        decimal FinalDiscountTemp = 0;
        decimal FinalShippingTemp = 0;
        decimal FinalTaxTemp = 0;
        decimal FinalOrdertotalTemp = 0;
        decimal TotalOrderNumberTemp = 0;
        DataSet dsCustomer = new DataSet();
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            storeid.Value = AppLogic.AppConfigs("storeid").ToString();
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["CustID"] != null && Convert.ToString(Request.QueryString["CustID"]).Trim() != "")
                    {
                        CustomerID = Convert.ToInt32(Request.QueryString["CustID"].ToString());
                        BindCustomerInfo();
                        RevenueInfo();
                        ProductsInfo();
                    }
                }
                catch { }
                if (Request.QueryString["Tab"] != null && Convert.ToString(Request.QueryString["Tab"]).Trim().ToLower() != "")
                {
                    if (Convert.ToString(Request.QueryString["Tab"]).Trim().ToLower() == "revenue")
                    {
                        privatenotes1.Attributes.Add("class", "active");
                       ordernotes1.Attributes.Add("class", "");
                       ordernotes.Attributes.Add("style", "display:none");
                      //  giftnotes.Attributes.Add("style", "display:none");
                        privatenotes.Attributes.Add("style", "display:block");
                    }
                }
            }

        }

        /// <summary>
        /// Function to Bind Customer Information
        /// </summary>
        public void BindCustomerInfo()
        {
            try
            {
                string strSql = "select c.CustomerID, (ISNULL(c.FirstName,'') +' ' +ISNULL(c.LastName,'')) as Name, ISNULL(c.Email,'') as Email,isnull(a.Company,'') as Company,isnull(a.Address1,'') as Address1,isnull(a.Address2,'') as Address2," +
                                " isnull(a.city,'') as city,isnull(a.State,'') as State,isnull(a.ZipCode,'') as ZipCode,isnull(a.Country,'') as Country,isnull(a.Phone,'') as Phone from tb_Customer c " +
                                " inner join tb_Address a on c.CustomerID = a.CustomerID where ISNULL(c.Deleted,0) = 0 and ISNULL(a.Deleted,0) = 0  and ISNULL(a.AddressType,0)  = 1 and c.CustomerID = " + CustomerID + "";


                dsCustomer = CommonComponent.GetCommonDataSet(strSql);
                if (dsCustomer == null || dsCustomer.Tables[0].Rows.Count == 0)
                {
                    strSql = "SELECT TOP 1 CustomerID, (ISNULL(FirstName,'') +' ' +ISNULL(LastName,'')) as Name, ISNULL(Email,'') as Email,isnull(billingcompany,'') as Company,isnull(billingphone,'') as phone,isnull(shippingAddress1,'') as Address1,isnull(shippingcity,'') as city,isnull(shippingState,'') as State,isnull(shippingZip,'') as ZipCode,isnull(shippingCountry,'') as Country FROM tb_Order WHERE CustomerId=" + CustomerID + " Order By OrderNumber DESC";


                    dsCustomer = CommonComponent.GetCommonDataSet(strSql);
                }
                if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CustomerID"].ToString()))
                    {
                      //  lblCustomderID.Text = dsCustomer.Tables[0].Rows[0]["CustomerID"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Company"].ToString()))
                    {
                       // lblCompanyName.Text = dsCustomer.Tables[0].Rows[0]["Company"].ToString();
                    }
                    else
                    {
                       // lblCompanyName.Text = "-";
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Name"].ToString()))
                    {
                       // lblName.Text = dsCustomer.Tables[0].Rows[0]["Name"].ToString();
                    }
                    else
                    {
                       // lblName.Text = "-";
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                    {
                      //  lblEmail.Text = dsCustomer.Tables[0].Rows[0]["Email"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()))
                    {//
                       // lblPhone.Text = dsCustomer.Tables[0].Rows[0]["Phone"].ToString();
                    }
                    else
                    {
                       // lblPhone.Text = "";
                    }
                    if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()) && !string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["city"].ToString()) && !string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["State"].ToString()) && !string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()) && !string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Country"].ToString()))
                    {
                        string CountrName = "";
                        CountrName = Convert.ToString(CommonComponent.GetScalarCommonData("select Name from tb_Country where CountryID = " + dsCustomer.Tables[0].Rows[0]["Country"].ToString() + ""));
                        if (!string.IsNullOrEmpty(CountrName))
                        {
                            //ltShipAddress.Text = dsCustomer.Tables[0].Rows[0]["Address1"].ToString() + "</br>" + dsCustomer.Tables[0].Rows[0]["city"].ToString() + "&nbsp;" + dsCustomer.Tables[0].Rows[0]["State"].ToString() + "&nbsp;" + dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString() + "</br>" + CountrName;
                        }
                        else
                        {
                           // ltShipAddress.Text = dsCustomer.Tables[0].Rows[0]["Address1"].ToString() + "</br>" + dsCustomer.Tables[0].Rows[0]["city"].ToString() + "&nbsp;" + dsCustomer.Tables[0].Rows[0]["State"].ToString() + "&nbsp;" + dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString() + "</br>" + dsCustomer.Tables[0].Rows[0]["Country"].ToString();
                        }

                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Function to Bind Revenue Details.
        /// </summary>
        public void RevenueInfo()
        {
            try
            {
                DataSet Ds = new DataSet();
                Ds = CommonComponent.GetCommonDataSet("select count(isnull(o.OrderNumber,0)) over(partition by o.CustomerID) as FinalOrderTotalCount,sum(isnull(o.OrderSubtotal,0)) over(partition by o.CustomerID) as FinalOrderSubtotal,sum(isnull(o.OrderTotal,0)) over(partition by o.CustomerID) as FinalOrdertotal,sum(isnull(o.OrderTax,0)) over(partition by o.CustomerID) as FinalTax,sum(isnull(o.CouponDiscountAmount,0)) over(partition by o.CustomerID) as FinalDiscount,sum(isnull(o.OrderShippingCosts,0)) over(partition by o.CustomerID) as FinalShipping, o.OrderNumber,ISNULL(o.OrderStatus,'') as OrderStatus,isnull(Convert(char(10),o.OrderDate,101),'') as Orderdate,ISNULL(o.OrderSubtotal,0) as OrderSubtotal,ISNULL(o.CouponDiscountAmount,0) as CouponDiscountAmount,ISNULL(o.OrderShippingCosts,0) as OrderShippingCosts,ISNULL(o.OrderTax,0) as OrderTax,ISNULL(o.OrderTotal,0) as OrderTotal from tb_Order o where ISNULL(o.Deleted,0) = 0 and o.CustomerID = " + CustomerID + "");
                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                    grdRevenue.DataSource = Ds;
                else grdRevenue.DataSource = null;
                grdRevenue.DataBind();
            }
            catch { }
        }

        /// <summary>
        ///   Gridview  Row Data Bound Event for Customer Wise Order Revenue
        /// </summary>
        protected void grdRevenue_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblFinalOrderSubtotalTemp = (Label)e.Row.FindControl("lblFinalOrderSubtotalTemp");
                FinalOrderSubtotalTemp = Convert.ToDecimal(lblFinalOrderSubtotalTemp.Text.ToString());

                Label lblFinalDiscountTemp = (Label)e.Row.FindControl("lblFinalDiscountTemp");
                FinalDiscountTemp = Convert.ToDecimal(lblFinalDiscountTemp.Text.ToString());

                Label lblFinalShippingTemp = (Label)e.Row.FindControl("lblFinalShippingTemp");
                FinalShippingTemp = Convert.ToDecimal(lblFinalShippingTemp.Text.ToString());

                Label lblFinalTaxTemp = (Label)e.Row.FindControl("lblFinalTaxTemp");
                FinalTaxTemp = Convert.ToDecimal(lblFinalTaxTemp.Text.ToString());

                Label lblFinalOrdertotalTemp = (Label)e.Row.FindControl("lblFinalOrdertotalTemp");
                FinalOrdertotalTemp = Convert.ToDecimal(lblFinalOrdertotalTemp.Text.ToString());

                Label lblTotalOrderNumberTemp = (Label)e.Row.FindControl("lblTotalOrderNumberTemp");
                TotalOrderNumberTemp = Convert.ToDecimal(lblTotalOrderNumberTemp.Text.ToString());


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFinalOrderSubtotal = (Label)e.Row.FindControl("lblFinalOrderSubtotal");
                lblFinalOrderSubtotal.Text = FinalOrderSubtotalTemp.ToString();

                Label lblFinalDiscount = (Label)e.Row.FindControl("lblFinalDiscount");
                lblFinalDiscount.Text = FinalDiscountTemp.ToString();

                Label lblFinalShipping = (Label)e.Row.FindControl("lblFinalShipping");
                lblFinalShipping.Text = FinalShippingTemp.ToString();

                Label lblFinalTax = (Label)e.Row.FindControl("lblFinalTax");
                lblFinalTax.Text = FinalTaxTemp.ToString();

                Label lblFinalOrdertotal = (Label)e.Row.FindControl("lblFinalOrdertotal");
                lblFinalOrdertotal.Text = FinalOrdertotalTemp.ToString();

                Label lblTotalOrderNumber = (Label)e.Row.FindControl("lblTotalOrderNumber");
                lblTotalOrderNumber.Text = TotalOrderNumberTemp.ToString();
            }
        }

        /// <summary>
        /// Function to Bind Products Details.
        /// </summary>
        public void ProductsInfo()
        {
            try
            {
                DataSet DsProduct = new DataSet();
                DsProduct = CommonComponent.GetCommonDataSet("select   o.storeId,o.OrderNumber,o.CustomerID, count(isnull(o.OrderNumber,0)) over(partition by o.CustomerID) as FinalOrderTotalCount,sum(isnull(o.OrderSubtotal,0)) over(partition by o.CustomerID) as FinalOrderSubtotal,sum(isnull(o.OrderTotal,0)) over(partition by o.CustomerID) as FinalOrdertotal,ISNULL(sc.RefProductID,0) as ProductID, ISNULL(sc.SKU,'') as SKU,ISNULL(sc.ProductName,'') as ProductName, " +
                                 " ISNULL(sc.Quantity,'') as Quantity,ISNULL(o.OrderSubtotal ,0) as OrderSubtotal,ISNULL(o.OrderTotal,'') as OrderTotal,ISNULL(sc.Price,0) as Price,ISNULL(sc.VariantValues,'') as VariantValues,ISNULL(sc.VariantNames,'') as VariantNames " +
                                 " from tb_Order o inner join tb_OrderedShoppingCartItems sc on o.ShoppingCardID = sc.OrderedShoppingCartID where isnull(o.deleted,0) = 0 and o.CustomerID  = " + CustomerID + "");
                if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                    grdProduct.DataSource = DsProduct;
                else grdProduct.DataSource = null;
                grdProduct.DataBind();

            }
            catch { }
        }

        /// <summary>
        ///   Gridview  Row Data Bound Event for Product
        /// </summary>
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblFinalOrderSubtotalTemp = (Label)e.Row.FindControl("lblFinalOrderSubtotalTemp");
                FinalOrderSubtotalTemp = Convert.ToDecimal(lblFinalOrderSubtotalTemp.Text.ToString());

                Label lblTotalOrderNumberTemp = (Label)e.Row.FindControl("lblTotalOrderNumberTemp");
                TotalOrderNumberTemp = Convert.ToDecimal(lblTotalOrderNumberTemp.Text.ToString());

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFinalOrderSubtotal = (Label)e.Row.FindControl("lblFinalOrderSubtotal");
                lblFinalOrderSubtotal.Text = FinalOrderSubtotalTemp.ToString();

                Label lblTotalOrderNumber = (Label)e.Row.FindControl("lblTotalOrderNumber");
                lblTotalOrderNumber.Text = TotalOrderNumberTemp.ToString();
            }
        }
    }
}