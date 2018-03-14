using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderIncompleteList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");

                BindIncompleteOrderlist();

            }
        }


        public void BindIncompleteOrderlist()
        {
            string strsearch = string.Empty;
            DataSet dsOrder = new DataSet();
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                strsearch = " And tb_Order.OrderNumber like '%" + txtSearch.Text.Replace("'", "''") + "%' ";

            }


            dsOrder = CommonComponent.GetCommonDataSet("select orderdate,OrderNumber,SalesAgentID ,OrderTotal,(tb_Admin.FirstName + ' '+tb_Admin.LastName) as  CreatedBy,tb_Order.CustomerID,tb_Order.StoreId,ShoppingCardID,(tb_Customer.FirstName + ' '+tb_customer.LastName) as Name,tb_Customer.Email  from tb_Order" +
                                              " left outer join tb_admin on tb_admin.AdminID =tb_Order.SalesAgentID"
                                              + " left outer join tb_Customer on tb_Customer.CustomerID =tb_Order.CustomerID"
                                              + " where tb_Order.IsPhoneOrder=1 and tb_Order.Deleted=1 and isnull(tb_order.isIncompleteOrder,0)=1" + strsearch);

            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {

                grdIncompleteOrder.DataSource = dsOrder;
                grdIncompleteOrder.DataBind();

            }

        }


        protected void grdIncompleteOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName.ToString().Trim().ToLower() == "add")
            {
                string ShoppingCardid = e.CommandArgument.ToString();

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);

                string strCustomerid = ((Label)row.FindControl("lblCustomerid")).Text;
                string strOrdernumber = ((Label)row.FindControl("lblOrderno")).Text;

                Int32 Cardid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ShoppingCart (CustomerID,CreatedOn) select " + strCustomerid + " ,getdate()  SELECT SCOPE_IDENTITY();"));
                DataSet dsshoppingcartitems = new DataSet();

                dsshoppingcartitems = CommonComponent.GetCommonDataSet("select * from tb_OrderedShoppingCartItems where OrderedShoppingCartID =" + ShoppingCardid);

                string ProductID = ""; string Price = "";
                string Quantity = ""; string Weight = ""; string VariantNames = ""; string VariantValues = ""; string RelatedproductID = "";
                string YardQuantity = ""; string Actualyard = ""; string Notes = ""; string DiscountPrice = "";


                RemoveCart(Convert.ToInt32(strCustomerid));

                if (dsshoppingcartitems != null && dsshoppingcartitems.Tables.Count > 0 && dsshoppingcartitems.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsshoppingcartitems.Tables[0].Rows.Count; i++)
                    {


                         ProductID = dsshoppingcartitems.Tables[0].Rows[i]["RefProductID"].ToString();
                         Price = dsshoppingcartitems.Tables[0].Rows[i]["Price"].ToString();
                         Quantity = dsshoppingcartitems.Tables[0].Rows[i]["Quantity"].ToString();
                         Weight = dsshoppingcartitems.Tables[0].Rows[i]["Weight"].ToString();
                        //  string  CategoryID  = dsshoppingcartitems.Tables[0].Rows[i]["ProductID"].ToString();
                         VariantNames = dsshoppingcartitems.Tables[0].Rows[i]["VariantNames"].ToString().Replace("'", "''");
                         VariantValues = dsshoppingcartitems.Tables[0].Rows[i]["VariantValues"].ToString().Replace("'", "''");
                         RelatedproductID = dsshoppingcartitems.Tables[0].Rows[i]["RelatedproductID"].ToString();
                        //   string  IsProductType  = dsshoppingcartitems.Tables[0].Rows[i]["ProductID"].ToString();
                         YardQuantity = dsshoppingcartitems.Tables[0].Rows[i]["YardQuantity"].ToString();
                         Actualyard = dsshoppingcartitems.Tables[0].Rows[i]["Actualyard"].ToString();
                         Notes = dsshoppingcartitems.Tables[0].Rows[i]["Notes"].ToString();
                         DiscountPrice = dsshoppingcartitems.Tables[0].Rows[i]["DiscountPrice"].ToString();
                         Int32 IsProductType =Convert.ToInt32(dsshoppingcartitems.Tables[0].Rows[i]["IsProductType"].ToString());
                         Int32 IsVendorOrderId = Convert.ToInt32(dsshoppingcartitems.Tables[0].Rows[i]["IsVendorOrderId"].ToString());
                        Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ShoppingCartItems  (ShoppingCartID,ProductID,Price,Quantity,Weight,VariantNames,VariantValues,"
                        + " RelatedproductID,YardQuantity,Actualyard,Notes,DiscountPrice,IsProductType,IsVendorOrderId)"
                        + " select " + Cardid + ", " + ProductID + " , " + Price + " ," + Quantity + " ," + Weight + " ,'" + VariantNames + "','" + VariantValues + "' ," + RelatedproductID + " , "
                        + YardQuantity + " ," + Actualyard + " ,'" + Notes + "' ," + DiscountPrice + "," + IsProductType + "," + IsVendorOrderId + ""));

                    }

                    Response.Redirect(String.Format("/admin/orders/Phoneorder.aspx?Ono={0}&CustID={1}&saleorder={2}", strOrdernumber, strCustomerid,1));
                }
            }
        }


        private void RemoveCart(Int32 CustId)
        {
            CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + CustId + ")");
        }
        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindIncompleteOrderlist();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindIncompleteOrderlist();
        }


    }
}