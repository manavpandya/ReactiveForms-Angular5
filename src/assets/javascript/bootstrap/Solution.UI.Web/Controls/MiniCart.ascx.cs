using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text;
using Solution.Bussines.Components.Common;
using StringBuilder = System.Text.StringBuilder;
using StringWriter = System.IO.StringWriter;
using DataSet = System.Data.DataSet;
using DataRow = System.Data.DataRow;
using System.IO;

namespace Solution.UI.Web.Controls
{
    public partial class MiniCart : System.Web.UI.UserControl
    {
        #region Variables
        public int CustomerID;
        public String TotalItems;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
                CustomerID = Convert.ToInt32(Session["CustID"]);

            if (Session["NoOfCartItems"] != null && Session["NoOfCartItems"].ToString() != "")
                TotalItems = Session["NoOfCartItems"].ToString();
            int CustID = 0;

            if (Session["CustID"] != null && Convert.ToString(Session["CustID"]) != "")
                CustID = Convert.ToInt32(Session["CustID"]);
            BindMiniCart();

        }

        /// <summary>
        /// Bind Shopping Cart Details for Perticular User without CustomerID
        /// </summary>
        public void BindMiniCart()
        {
            if (Session["CustID"] != "" && Session["CustID"] != "")
                CustomerID = Convert.ToInt32(Session["CustID"]);
            clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(CustomerID));
            litMiniCart.Text = objMiniCart.GetMiniCart().Replace("/CheckoutCommon.aspx", "/checkoutcommon.aspx");
        }

        /// <summary>
        /// Bind Shopping Cart Details for Perticular User with CustomerID
        /// </summary>
        /// <param name="CustID">string CustID</param>
        public void BindMiniCartInDiv(String CustID)
        {

            clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(CustID));
            //ECommerceSite.Client.clsMiniCart objMiniCart = new ECommerceSite.Client.clsMiniCart(Convert.ToInt32(CustID));
            string strMiniCart = objMiniCart.GetMiniCart();


            //if (OutStockflag)
            //{
            //    strMiniCart += "<div style='color:cc0000;display:none;'>Not Enough Inventory </div>";
            //}
            //Response.Clear();
            //Response.Write(strMiniCart);
            //Response.End();
        }
    }
}