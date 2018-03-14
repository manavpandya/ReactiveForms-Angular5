using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ReturnMerchandisePopUp : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                BindData(Convert.ToInt32(Request.QueryString["ID"].ToString()));
            }
        }

        /// <summary>
        /// Binds the data for Return Merchandise
        /// </summary>
        /// <param name="RMAID">int RMAID</param>
        private void BindData(int RMAID)
        {
            DataSet ds = new DataSet();
            RMAComponent objRMA = new RMAComponent();
            ds = objRMA.GetRMAProductByRMAID(RMAID);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                lblOrderNum.Text = ds.Tables[0].Rows[0]["OrderedNumber"].ToString();
                lblCustomerName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                lblEmail.Text = ds.Tables[0].Rows[0]["CustomerEMail"].ToString();
                lblInvoiceDate.Text = ds.Tables[0].Rows[0]["orderdate"].ToString();
               
                lblReturnResult.Text = ds.Tables[0].Rows[0]["ReturnReason"].ToString();
                lblAnyAvailableInfo.Text = ds.Tables[0].Rows[0]["AdditionalInformation"].ToString();


                for(int k=0;k<ds.Tables[0].Rows.Count;k++)
                {
                    lblProductName.Text += ds.Tables[0].Rows[k]["Name"].ToString() +",";
                    lblMerchandiseCode.Text += ds.Tables[0].Rows[k]["SKU"].ToString() +",";
                    lblQuantity.Text += ds.Tables[0].Rows[k]["Quantity"].ToString() +",";
                }

                

            }
        }
    }
}