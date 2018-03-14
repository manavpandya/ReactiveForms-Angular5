using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Solution.Data;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderEmail : BasePage
    {
        public int OrderNumber;
        public string OrderEmailID;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["ONo"] != null)
            {
                Int32 StoreID = 0;
                OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
            }

            if (Request.QueryString["Ono"] != null)
            {
                OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()));
            }
            if (Request.QueryString["OrderEmail"] != null)
            {
                OrderEmailID = Convert.ToString(Request.QueryString["OrderEmail"].ToString());
            }
            if (!IsPostBack)
            {
                OrderEmails();
            }
        }


        /// <summary>
        /// display Order Emails
        /// </summary>
        public void OrderEmails()
        {
            SQLAccess objsqlGetEmailForOrder = new SQLAccess();
            String strGetDataqry = "";
            String strPO = "";

            strGetDataqry = "select MailID,[From],[To],Subject,SentOn,IsAttachment,IsIncomming from tb_EmailList  where ";
            strGetDataqry += "((Subject like '%New Order # " + OrderNumber.ToString() + "%' or Subject like '%Receipt for Order # " + OrderNumber.ToString() + "%' or Subject like '%Purchase order number " + OrderNumber.ToString() + "%' or Subject like '%Order Cancelled - ONo: " + OrderNumber.ToString() + "%')";
            strGetDataqry += " or (Body like '%Order Number: <strong style=\"color: #F00\" id=\"lblOrderId\">                                              " + OrderNumber.ToString() + "%' or  Body like '%Order Number :</b></td><td>" + OrderNumber.ToString() + "%' or Body like '%Order Number :<b style=\"font-size:12px;\"><font style=\"font-size:20px;\">" + OrderNumber.ToString() + "%' or isnull(OrderNumber,0)=" + OrderNumber.ToString() + ")) and isnull(isDeleted,0)=0 and isnull(isSpam,0)=0 ";
            strGetDataqry += " UNION ";

            strGetDataqry = "select MailID,[From],[To],Subject,SentOn,IsAttachment,IsIncomming from tb_EmailList  where ";
            strGetDataqry += "((Subject like '%New Order # " + OrderNumber.ToString() + "%' or Subject like '%Receipt for Order # " + OrderNumber.ToString() + "%' or Subject like '%Purchase order number " + OrderNumber.ToString() + "%' or Subject like '%Order Cancelled - ONo: " + OrderNumber.ToString() + "%')";
            strGetDataqry += " or (Body like '%Order Number: <strong style=\"color: #F00\" id=\"lblOrderId\">                                              " + OrderNumber.ToString() + "%' or  Body like '%Order Number :</b></td><td>" + OrderNumber.ToString() + "%' or Body like '%Order Number :<b style=\"font-size:12px;\"><font style=\"font-size:20px;\">" + OrderNumber.ToString() + "%' or isnull(OrderNumber,0)=" + OrderNumber.ToString() + ")) and isnull(isDeleted,0)=0 and isnull(isSpam,0)=0 AND ([from] like '%" + OrderEmailID + "%' Or [To] like '%" + OrderEmailID + "%') ORDER BY SentOn DESC";

            DataSet dsGetEmailFromOrder = new DataSet();
            dsGetEmailFromOrder = CommonComponent.GetCommonDataSet(strGetDataqry);


            StringBuilder sbOrdEmail = new StringBuilder();
            sbOrdEmail.Append("");
            if (dsGetEmailFromOrder != null && dsGetEmailFromOrder.Tables.Count > 0 && dsGetEmailFromOrder.Tables[0].Rows.Count > 0)
            {
                sbOrdEmail.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"datatable\">");
                sbOrdEmail.Append("<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">");
                sbOrdEmail.Append("<th valign=\"middle\" align=\"left\" style=\"width: 20%\"><b>From Email</b></th>");
                sbOrdEmail.Append("<th valign=\"middle\" align=\"left\" style=\"width: 20%\"><b>To Email</b></th>");
                sbOrdEmail.Append("<th valign=\"middle\" align=\"left\" style=\"width: 45%\"><b>Subject Email</b></th>");
                sbOrdEmail.Append("<th valign=\"middle\" align=\"left\" style=\"width: 15%\"><b>Sent On</b></th>");
                sbOrdEmail.Append("</tr>");

                for (int i = 0; i < dsGetEmailFromOrder.Tables[0].Rows.Count; i++)
                {
                    sbOrdEmail.Append("<tr>");
                    sbOrdEmail.Append("<td valign=\"top\" align=\"left\">" + Convert.ToString(dsGetEmailFromOrder.Tables[0].Rows[i]["From"].ToString()) + "</td>");
                    sbOrdEmail.Append("<td valign=\"top\" align=\"left\">" + Convert.ToString(dsGetEmailFromOrder.Tables[0].Rows[i]["To"].ToString()) + "</td>");
                    sbOrdEmail.Append("<td valign=\"top\" align=\"left\"><a href =\"javascript:void(0);\" onclick=\"OpenCenterWindow('/Admin/WebMail/EmailInboxmaster.aspx?ShowType=ShowBody&ID=" + Convert.ToString(dsGetEmailFromOrder.Tables[0].Rows[i]["MailID"].ToString()) + "',1000,800)\">" + Convert.ToString(dsGetEmailFromOrder.Tables[0].Rows[i]["Subject"].ToString()) + "</a></td>");
                    sbOrdEmail.Append("<td valign=\"top\" align=\"left\">" + Convert.ToString(dsGetEmailFromOrder.Tables[0].Rows[i]["SentOn"].ToString()) + "</td></tr>");
                }
                sbOrdEmail.Append("</table>");
            }


            if (sbOrdEmail.ToString() != "")
            {
                ltrOrderEmails.Text = sbOrdEmail.ToString();
            }
            else
            {
                ltrOrderEmails.Text = "<span style='color:red;font-family: Arial,Helvetica,sans-serif;font-size: 13px;'>No email(s) found.</span>";
            }


        }
    }
}