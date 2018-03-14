using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class CustomerQuoteHandler : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["CustQuoteID"] != null)
                {
                    DataSet dsIsRevised = new DataSet();
                    dsIsRevised = CommonComponent.GetCommonDataSet("select top 1 isnull(Ordernumber,0) as ordernumber,ISNULL(IsRevised,0) As IsRevised from dbo.tb_CustomerQuote where CustomerQuoteid=" + SecurityComponent.Decrypt(Request.QueryString["CustQuoteID"].ToString().Replace(" ", "+")));
                    if (dsIsRevised != null && dsIsRevised.Tables.Count > 0 && dsIsRevised.Tables[0].Rows.Count > 0)
                    {
                        bool IsRevised = Convert.ToBoolean(dsIsRevised.Tables[0].Rows[0]["IsRevised"]);
                        if (!IsRevised)
                        {
                            int ordernumber = Convert.ToInt32(CommonComponent.GetScalarCommonData(" select top 1 isnull(Ordernumber,0) from dbo.tb_CustomerQuote where CustomerQuoteid=" + SecurityComponent.Decrypt(Request.QueryString["CustQuoteID"].ToString().Replace(" ", "+"))));
                            if (ordernumber > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Customer Quote already Fulfilled. Please request another Quote.');window.location='/index.aspx'", true);
                            }
                            else
                            {
                                Session["CustomerQuoteID"] = Request.QueryString["CustQuoteID"].ToString().Replace(" ", "+");
                                AddToCartforCustomerQuote(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["CustQuoteID"].ToString().Replace(" ", "+"))));
                                Session["PaymentMethod"] = "creditcard";
                                Response.Redirect("checkoutcustomerquote.aspx");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Msg", "alert('Quote has been Revised, Use latest Quote sent in mail by Administrator.');window.location='/index.aspx'", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Msg", "alert('Quote has been Revised, Use latest Quote sent in mail by Administrator.');window.location='/index.aspx'", true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the ShoppingCart
        /// </summary>
        /// <param name="CustID">int CustID</param>
        /// <returns>Returns a Shopping Cart Details as a Dataset</returns>
        private DataSet GetShoppingcart(Int32 CustID)
        {
            DataSet DsCItems = new DataSet();
            DsCItems = CommonComponent.GetCommonDataSet(@"SELECT tb_Product.Name,tb_Product.SEName,tb_Product.SEName,tb_Product.SKU,
                        tb_Product.Name + ISNull(Convert(nvarchar(max),SUBSTRING(tb_Product.Description,0,180)),'') as Description,
                        tb_ShoppingCartItems.Price As SalePrice, tb_ShoppingCartItems.Quantity,tb_ShoppingCartItems.ProductID, 
                        tb_ShoppingCartItems.ShoppingCartID, tb_Product.Name AS ProductName,tb_ShoppingCartItems.VariantNames,
                        tb_ShoppingCartItems.VariantValues 
                        FROM tb_Product INNER JOIN tb_ShoppingCartItems ON tb_Product.ProductID = tb_ShoppingCartItems.ProductID 
                        Where tb_Product.StoreID=" + AppLogic.AppConfigs("StoreID")
                        + " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustID + ")");

            return DsCItems;
        }

        /// <summary>
        /// Add to Cart for Customer Quote
        /// </summary>
        /// <param name="CustomerQuoteID">int CustomerQuoteID</param>
        protected void AddToCartforCustomerQuote(Int32 CustomerQuoteID)
        {
            Int32 CustID = 0;
            CustID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select CustomerID from tb_CustomerQuote where CustomerQuoteID=" + CustomerQuoteID));
            bool flag = CommonOperations.RegisterCart(CustID, false);
            String Name = (String)CommonComponent.GetScalarCommonData("select email from tb_customer where customerid=" + CustID);
            Session["UserName"] = Name;
            Session["CustID"] = CustID;
            Session["IsAnonymous"] = "false";
            Session["FirstName"] = Name;
            Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));

            CommonComponent.ExecuteCommonData("Delete From tb_ShoppingCartItems Where ShoppingCartID=(Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustID + ")");
            CommonComponent.ExecuteCommonData("Delete From tb_ShoppingCart Where CustomerID=" + CustID);

            DataSet dsCustQuote = new DataSet();
            dsCustQuote = CommonComponent.GetCommonDataSet(@"select cq.customerid,p.sku,isnull(p.weight,1) as weight,cqi.name,cqi.price,cqi.quantity,isnull(cqi.options,'') as options,
                            cqi.productid from tb_CustomerQuoteItems cqi inner join tb_CustomerQuote cq  on cq.CustomerQuoteID=cqi.CustomerQuoteID  
                            inner join tb_Product p  on p.ProductID=cqi.ProductID where cq.CustomerQuoteID=" + CustomerQuoteID);

            if (dsCustQuote != null && dsCustQuote.Tables.Count > 0 && dsCustQuote.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCustQuote.Tables[0].Rows.Count; i++)
                {
                    Int32 SCartID = -1;
                    SCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + Convert.ToInt32(Session["CustID"])));
                    if (SCartID == 0)
                    {
                        SCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_shoppingcart(customerid)values(" + CustID + "); select scope_identity();"));
                    }
                    if (SCartID > 0)
                    {
                        CommonComponent.ExecuteCommonData("insert into tb_ShoppingCartItems (ShoppingCartID,ProductID,Price,Quantity,Weight,CategoryID,VariantNames,VariantValues)"
                            + " values (" + SCartID + "," + dsCustQuote.Tables[0].Rows[i]["ProductID"] + "," + dsCustQuote.Tables[0].Rows[i]["Price"]
                            + "," + dsCustQuote.Tables[0].Rows[i]["Quantity"] + "," + dsCustQuote.Tables[0].Rows[i]["Weight"] + ",0,'','" + dsCustQuote.Tables[0].Rows[i]["options"] + "' )");
                    }
                }
            }
        }
    }
}