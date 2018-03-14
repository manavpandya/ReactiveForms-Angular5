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
    public partial class RMARequestList : BasePage
    {
        DataSet dsorderlog = new DataSet();
        string OrderNumber = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                lblOrderNumber.Text = OrderNumber.ToString();
            }
            if (!IsPostBack)
            {
                BindData(OrderNumber);
            }
        }

        /// <summary>
        /// Binds the data for RMA Request List
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        public void BindData(string OrderNumber)
        {
           // dsorderlog = CommonComponent.GetCommonDataSet("select s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,ReturnItemID As id,ReturnItemID,'RMA-'+ convert(nvarchar(50),ReturnItemID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_ReturnItem.Deleted As 'Status',tb_ReturnItem.CreatedOn,isnull(tb_ReturnItem.isReturnrequest,0) as isReturnrequest from tb_ReturnItem join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumber inner join tb_store s on s.storeid=o.storeid  where tb_Product.storeid=" + AppLogic.AppConfigs("StoreId").ToString() + "  AND o.orderNumber=" + OrderNumber + " order by tb_ReturnItem.CreatedOn desc");

            dsorderlog = CommonComponent.GetCommonDataSet(@"SELECT s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,tb_Return.returnid As id,tb_Return.returnid,
'RMA-'+ convert(nvarchar(50),tb_Return.returnid) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,
o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_Return.Deleted As
 'Status',tb_Return.CreatedOn,isnull(tb_Return.isReturnrequest,0) as isReturnrequest FROM tb_ReturnItem
INNER JOIN tb_Return on tb_ReturnItem.ReturnID =tb_Return.ReturnID
join tb_Product on
  tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumbeR
  inner join tb_store s on
   s.storeid=o.storeid  where tb_Product.storeid=" + AppLogic.AppConfigs("StoreId").ToString() + @"  
   AND o.orderNumber=" + OrderNumber + " order by tb_Return.CreatedOn desc");



            if (dsorderlog != null && dsorderlog.Tables.Count > 0 && dsorderlog.Tables[0].Rows.Count > 0)
            {
                grdRMARequestList.DataSource = dsorderlog;
                grdRMARequestList.DataBind();
                creatermalinktr.Visible = false;
                trRMATitle.Visible = true;
            }
            else
            {
                DataSet dsOrder = new DataSet();
                dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(OrderNumber));
                if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                {
                    int CustomerID = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["CustomerID"].ToString());
                    creatermalink.HRef = "http://162.242.196.60/ReturnItem.aspx?CUSTID=" + CustomerID.ToString() + ""; ;
                    string lblStoreNameOrder = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(StoreName,'') as StoreName from tb_Store where StoreID=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + " and ISNULL(Deleted,0)=0"));
                    if (!string.IsNullOrEmpty(lblStoreNameOrder))
                    {
                        if (lblStoreNameOrder.ToString().ToLower().IndexOf("half price drapes yahoo") > -1)
                        {
                            creatermalink.HRef = "http://162.242.196.60/RMA/HPDYahoo/ReturnItem.aspx?custid=" + CustomerID.ToString() + "&strid=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + "";
                        }
                        else if (lblStoreNameOrder.ToString().ToLower().IndexOf("amazon") > -1)
                        {
                            creatermalink.HRef = "http://162.242.196.60/rma/amazon/returnitem.aspx?custid=" + CustomerID.ToString() + "&strid=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + "";
                        }
                        else if (lblStoreNameOrder.ToString().ToLower().IndexOf("sears") > -1)
                        {
                            creatermalink.HRef = "http://162.242.196.60/rma/sears/returnitem.aspx?custid=" + CustomerID.ToString() + "&strid=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + "";
                        }
                        else if (lblStoreNameOrder.ToString().ToLower().IndexOf("overstock") > -1)
                        {
                            creatermalink.HRef = "http://162.242.196.60/rma/overstock/returnitem.aspx?custid=" + CustomerID.ToString() + "&strid=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + "";
                        }
                        else // Main Store
                        {
                            creatermalink.HRef = "http://162.242.196.60/returnitem.aspx?custid=" + CustomerID.ToString() + "&strid=" + dsOrder.Tables[0].Rows[0]["StoreID"].ToString() + "";
                        }
                    }
                }
                creatermalinktr.Visible = true;
                trRMATitle.Visible = false;
                grdRMARequestList.DataSource = null;
                grdRMARequestList.DataBind();
            }
        }

        /// <summary>
        /// RMA Request List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdRMARequestList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                System.Data.DataRowView Row = (System.Data.DataRowView)e.Row.DataItem;
                System.Web.UI.HtmlControls.HtmlAnchor lkOrderNumber = e.Row.FindControl("lkOrderNumber") as System.Web.UI.HtmlControls.HtmlAnchor;
                System.Web.UI.WebControls.HiddenField hdOrderNumber = e.Row.FindControl("hdOrderNumber") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdReturnType = e.Row.FindControl("hdReturnType") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdStoreName = e.Row.FindControl("hdStoreName") as System.Web.UI.WebControls.HiddenField;
                System.Web.UI.WebControls.HiddenField hdnisReturnrequest = e.Row.FindControl("hdnisReturnrequest") as System.Web.UI.WebControls.HiddenField;
                if (lkOrderNumber != null)
                {
                    if (hdnisReturnrequest.Value.ToString() == "True")
                    {
                        if (hdReturnType.Value.ToString().Trim().ToLower() == "rr" && !(hdStoreName.Value.ToString().ToLower().Contains("ebay") || hdStoreName.Value.ToString().ToLower().Contains("amazon")))
                            lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?Id=" + hdOrderNumber.Value + "&refund=1";
                        else
                            lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?Id=" + hdOrderNumber.Value + "&return=1";
                    }
                    else
                    {
                        lkOrderNumber.HRef = "/Admin/Orders/orders.aspx?Id=" + hdOrderNumber.Value + "&fromreturn=1";
                    }
                }
            }
        }
    }
}