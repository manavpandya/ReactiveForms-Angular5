using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddQAR : BasePage
    {
        #region Declaration
        tb_ShippingServices tblShippingService = null;
        ShippingComponent objShipping = null;
        int StoreId = 0;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            
            if (!IsPostBack)
            {
                bindproduct();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

            }
            imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

        }

        /// <summary>
        ///  Bind Store Details in dropdown
        /// </summary>
        private void bindproduct()
        {
            //DataSet productdetail = new DataSet();
            //productdetail = CommonComponent.GetCommonDataSet("select ProductID,Name from tb_product where StoreID=1 and isnull(Deleted,0)=0");
            //if (productdetail != null && productdetail.Tables.Count > 0 && productdetail.Tables[0].Rows.Count>0)
            //{
            //    ddlproduct.DataSource = productdetail;
            //    ddlproduct.DataTextField = "Name";
            //    ddlproduct.DataValueField = "ProductID";
            //    ddlproduct.DataBind();
            //}
            //ddlproduct.Items.Insert(0, new ListItem("Select Product", "-1"));
            //ddlproduct.SelectedIndex = 0;

            string strProductId = Request.QueryString["ID"];
            DataSet productdetail = new DataSet();
            productdetail = CommonComponent.GetCommonDataSet("select QuestionId,(select Name from tb_Product where ProductId = '" + strProductId + "') as ProductName,Question,SameQuestionToo,Role,CustomerId,CreatedBy,CreatedDate,CreatedTime,IsApproved from tb_QAQuestion_MVC where ProductId = '" + strProductId + "'");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (productdetail != null && productdetail.Tables.Count > 0 && productdetail.Tables[0].Rows.Count > 0)
            {
                sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
                sb.Append("<tr class=\"oddrow\">");
                sb.Append("<td>");
                sb.Append("<h1>Product Name:</h1>");
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append("<h1>" + productdetail.Tables[0].Rows[0]["ProductName"] + "</h1>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr class=\"oddrow\">");
                sb.Append("<td></td>");
                sb.Append("<td><span style=\"vertical-align: left; margin-right: 3px; float: left;\"><a href=\"javascript:void(0);\" onclick=\"AddQuestionBox()\">Add Question");
                sb.Append("</a>");
                sb.Append("<div class=\"txtAreaAddQusetion\" style=\"display:none;\">");
                sb.Append("<textarea ID=\"txtAreaAddQusetion\" name=\"txtAreaAddQusetion\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\"></textarea>");
                sb.Append(" ");
                sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"return SaveAddQuestionBox(" + strProductId + ");\">");
                sb.Append(" ");
                sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelAddQuestionBox()\">");
                sb.Append("</div>");
                sb.Append("</span>");
                sb.Append("</td>");
                sb.Append("</tr>");
                for (int i = 0; i < productdetail.Tables[0].Rows.Count; i++)
                {
                    //For Questions 
                    sb.Append("<tr class=\"oddrow\">");
                    sb.Append("<td colspan=\"2\"><h2 style=\"padding: 0 0 0 0px;\">Q" + (i + 1) + " :</h2><textarea onclick=\"addtext('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', this.value)\" ID=\"txtQuestion" + productdetail.Tables[0].Rows[i]["QuestionId"] + "\" name=\"txtQuestion" + productdetail.Tables[0].Rows[i]["QuestionId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetail.Tables[0].Rows[i]["Question"] + "</textarea>");
                    sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetail.Tables[0].Rows[i]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetail.Tables[0].Rows[i]["CreatedBy"]);
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("</td>");
                    sb.Append("<td>");

                    sb.Append("<div style=\"padding-right:750px;\" >");

                    if (Convert.ToInt32(productdetail.Tables[0].Rows[i]["IsApproved"]) == 0)
                    {
                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', 'Question', '1')\">");
                        sb.Append("Approve");
                        sb.Append(" ");
                        sb.Append("</a>");
                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', 'Question', '2')\">");
                        sb.Append("Disapprove");
                        sb.Append("</a>");
                        sb.Append(" ");
                    }
                    else if (Convert.ToInt32(productdetail.Tables[0].Rows[i]["IsApproved"]) == 1)
                    {
                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', 'Question', '2')\">");
                        sb.Append("Disapprove");
                        sb.Append("</a>");
                        sb.Append(" ");
                    }
                    else if (Convert.ToInt32(productdetail.Tables[0].Rows[i]["IsApproved"]) == 2)
                    {
                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', 'Question', '1')\">");
                        sb.Append("Approve");
                        sb.Append("</a>");
                        sb.Append(" ");
                    }

                    sb.Append("<a href=\"javascript:void(0);\" onclick=\"AdditionBox('txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "')\">");
                    sb.Append("Answer to Q" + (i + 1));
                    sb.Append("</a>");
                    sb.Append("</div>");

                    sb.Append("<div class=\"txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "\" style=\"display:none;\">");
                    sb.Append("<textarea ID=\"txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "\" name=\"txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\"></textarea>");
                    sb.Append(" ");
                    sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetail.Tables[0].Rows[i]["QuestionId"] + "','txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "', 'Answer')\">");
                    sb.Append(" ");
                    sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaAnswer" + productdetail.Tables[0].Rows[i]["QuestionId"] + "')\">");
                    sb.Append("</div>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    DataSet productdetailAns = new DataSet();
                    productdetailAns = CommonComponent.GetCommonDataSet("select AnswerId,Answer,InAccurate,Star,Role,CustomerId,CreatedBy,CreatedDate,CreatedTime,IsApproved from tb_QAAnswer_MVC where QuestionId = '" + productdetail.Tables[0].Rows[i]["QuestionId"] + "'");
                    if (productdetailAns != null && productdetailAns.Tables.Count > 0 && productdetailAns.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < productdetailAns.Tables[0].Rows.Count; j++)
                        {
                            // For Answers
                            sb.Append("<tr class=\"oddrow\">");
                            sb.Append("<td colspan=\"2\" style=\"padding: 0 0 0 200px;\"><h2 style=\"padding: 0 0 0 0px;\">A" + (j + 1) + " :</h2><textarea onclick=\"addtextAnswer('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', this.value)\" ID=\"txtAnswer" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "\" name=\"txtAnswer" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetailAns.Tables[0].Rows[j]["Answer"] + "</textarea>");
                            sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetailAns.Tables[0].Rows[j]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetailAns.Tables[0].Rows[j]["CreatedBy"]);
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td>");
                            sb.Append("</td>");
                            sb.Append("<td>");

                            sb.Append("<div style=\"padding-left:180px;\">");

                            if (Convert.ToInt32(productdetailAns.Tables[0].Rows[j]["IsApproved"]) == 0)
                            {
                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', 'Answer', '1')\">");
                                sb.Append("Approve");
                                sb.Append("</a>");
                                sb.Append(" ");
                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', 'Answer', '2')\">");
                                sb.Append("Disapprove");
                                sb.Append("</a>");
                                sb.Append(" ");
                            }
                            else if (Convert.ToInt32(productdetailAns.Tables[0].Rows[j]["IsApproved"]) == 1)
                            {
                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', 'Answer', '2')\">");
                                sb.Append("Disapprove");
                                sb.Append("</a>");
                                sb.Append(" ");
                            }
                            else if (Convert.ToInt32(productdetailAns.Tables[0].Rows[j]["IsApproved"]) == 2)
                            {
                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', 'Answer', '1')\">");
                                sb.Append("Approve");
                                sb.Append("</a>");
                                sb.Append(" ");
                            }

                            sb.Append("<a href=\"javascript:void(0);\" onclick=\"AdditionBox('txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "')\">");
                            sb.Append("Reply to A" + (j + 1));
                            sb.Append("</a>");
                            sb.Append("</div>");

                            sb.Append("<div class=\"txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "\" style=\"display:none;\">");
                            sb.Append("<textarea ID=\"txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "\" name=\"txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;margin-left:210px;\"></textarea>");
                            sb.Append(" ");
                            sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "','txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "', 'Reply')\">");
                            sb.Append(" ");
                            sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaReply" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "')\">");
                            sb.Append("</div>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            DataSet productdetailRep = new DataSet();
                            productdetailRep = CommonComponent.GetCommonDataSet("select ReplyId,AnswerId,Reply,Role,CustomerId,CreatedBy,CreatedDate,CreatedTime,IsApproved from tb_QAReply_MVC where AnswerId = '" + productdetailAns.Tables[0].Rows[j]["AnswerId"] + "'");
                            if (productdetailRep != null && productdetailRep.Tables.Count > 0 && productdetailRep.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < productdetailRep.Tables[0].Rows.Count; k++)
                                {
                                    // For Reply
                                    sb.Append("<tr class=\"oddrow\">");
                                    sb.Append("<td colspan=\"2\" style=\"padding: 0 0 0 400px;\"><h2 style=\"padding: 0 0 0 0px;\">R" + (k + 1) + " :</h2><textarea onclick=\"addtextReply('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', this.value)\" ID=\"txtReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "\" name=\"txtReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetailRep.Tables[0].Rows[k]["Reply"] + "</textarea>");
                                    sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetailRep.Tables[0].Rows[k]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetailRep.Tables[0].Rows[k]["CreatedBy"]);
                                    sb.Append("</td>");
                                    sb.Append("</tr>");

                                    sb.Append("<tr>");
                                    sb.Append("<td>");
                                    sb.Append("</td>");
                                    sb.Append("<td>");

                                    sb.Append("<div style=\"padding-left:420px;\">");
                                    if (Convert.ToInt32(productdetailRep.Tables[0].Rows[k]["IsApproved"]) == 0)
                                    {
                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', 'Reply', '1')\">");
                                        sb.Append("Approve");
                                        sb.Append("</a>");
                                        sb.Append(" ");
                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', 'Reply', '2')\">");
                                        sb.Append("Disapprove");
                                        sb.Append("</a>");
                                        sb.Append(" ");
                                    }
                                    else if (Convert.ToInt32(productdetailRep.Tables[0].Rows[k]["IsApproved"]) == 1)
                                    {
                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', 'Reply', '2')\">");
                                        sb.Append("Disapprove");
                                        sb.Append("</a>");
                                        sb.Append(" ");
                                    }
                                    else if (Convert.ToInt32(productdetailRep.Tables[0].Rows[k]["IsApproved"]) == 2)
                                    {
                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', 'Reply', '1')\">");
                                        sb.Append("Approve");
                                        sb.Append("</a>");
                                        sb.Append(" ");
                                    }

                                    sb.Append("<a href=\"javascript:void(0);\"  onclick=\"AdditionBox('txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "')\">");
                                    sb.Append("Reply to R" + (k + 1));
                                    sb.Append("</a>");
                                    sb.Append("</div>");

                                    sb.Append("<div class=\"txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "\" style=\"display:none;\" >");
                                    sb.Append("<textarea ID=\"txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "\" name=\"txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;margin-left:410px;\"></textarea>");
                                    sb.Append(" ");
                                    sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "','txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "', 'InnerReply')\">");
                                    sb.Append(" ");
                                    sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaInnerReply" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "')\">");
                                    sb.Append("</div>");
                                    sb.Append("</td>");
                                    sb.Append("</tr>");

                                    DataSet productdetailInnerRep = new DataSet();
                                    productdetailInnerRep = CommonComponent.GetCommonDataSet("select * from tb_QAReply_MVC where AnswerId = '" + productdetailRep.Tables[0].Rows[k]["ReplyId"] + "' and IsChild = 0");
                                    if (productdetailInnerRep != null && productdetailInnerRep.Tables.Count > 0 && productdetailInnerRep.Tables[0].Rows.Count > 0)
                                    {
                                        for (int m = 0; m < productdetailInnerRep.Tables[0].Rows.Count; m++)
                                        {
                                            // For InnerReply1
                                            sb.Append("<tr class=\"oddrow\">");
                                            sb.Append("<td colspan=\"2\" style=\"padding: 0 0 0 600px;\"><h2 style=\"padding: 0 0 0 0px;\">R" + (m + 1) + " :</h2><textarea onclick=\"addtextInnerReply('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', this.value)\" ID=\"txtInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "\" name=\"txtInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetailInnerRep.Tables[0].Rows[m]["Reply"] + "</textarea>");
                                            sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetailInnerRep.Tables[0].Rows[m]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetailInnerRep.Tables[0].Rows[m]["CreatedBy"]);
                                            sb.Append("</td>");
                                            sb.Append("</tr>");

                                            sb.Append("<tr>");
                                            sb.Append("<td>");
                                            sb.Append("</td>");
                                            sb.Append("<td>");

                                            sb.Append("<div style=\"padding-left:620px;\">");
                                            if (Convert.ToInt32(productdetailInnerRep.Tables[0].Rows[m]["IsApproved"]) == 0)
                                            {
                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                sb.Append("Approve");
                                                sb.Append("</a>");
                                                sb.Append(" ");
                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                sb.Append("Disapprove");
                                                sb.Append("</a>");
                                                sb.Append(" ");
                                            }
                                            else if (Convert.ToInt32(productdetailInnerRep.Tables[0].Rows[m]["IsApproved"]) == 1)
                                            {
                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                sb.Append("Disapprove");
                                                sb.Append("</a>");
                                                sb.Append(" ");
                                            }
                                            else if (Convert.ToInt32(productdetailInnerRep.Tables[0].Rows[m]["IsApproved"]) == 2)
                                            {
                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                sb.Append("Approve");
                                                sb.Append("</a>");
                                                sb.Append(" ");
                                            }

                                            sb.Append("<a href=\"javascript:void(0);\" onclick=\"AdditionBox('txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "')\">");
                                            sb.Append("Reply to R" + (m + 1));
                                            sb.Append("</a>");
                                            sb.Append("</div>");

                                            sb.Append("<div class=\"txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "\" style=\"display:none;\" >");
                                            sb.Append("<textarea ID=\"txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "\" name=\"txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;margin-left:410px;\"></textarea>");
                                            sb.Append(" ");
                                            sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "','txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "', 'InnerReply')\">");
                                            sb.Append(" ");
                                            sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaInnerReply" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "')\">");
                                            sb.Append("</div>");
                                            sb.Append("</td>");
                                            sb.Append("</tr>");


                                            DataSet productdetailInnerRep2 = new DataSet();
                                            productdetailInnerRep2 = CommonComponent.GetCommonDataSet("select * from tb_QAReply_MVC where AnswerId = '" + productdetailInnerRep.Tables[0].Rows[m]["ReplyId"] + "' and IsChild = 0");
                                            if (productdetailInnerRep2 != null && productdetailInnerRep2.Tables.Count > 0 && productdetailInnerRep2.Tables[0].Rows.Count > 0)
                                            {
                                                for (int n = 0; n < productdetailInnerRep2.Tables[0].Rows.Count; n++)
                                                {
                                                    // For InnerReply2
                                                    sb.Append("<tr class=\"oddrow\">");
                                                    sb.Append("<td colspan=\"2\" style=\"padding: 0 0 0 800px;\"><h2 style=\"padding: 0 0 0 0px;\">R" + (n + 1) + " :</h2><textarea onclick=\"addtextInnerReply('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', this.value)\" ID=\"txtInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "\" name=\"txtInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetailInnerRep2.Tables[0].Rows[n]["Reply"] + "</textarea>");
                                                    sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetailInnerRep2.Tables[0].Rows[n]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetailInnerRep2.Tables[0].Rows[n]["CreatedBy"]);
                                                    sb.Append("</td>");
                                                    sb.Append("</tr>");

                                                    sb.Append("<tr>");
                                                    sb.Append("<td>");
                                                    sb.Append("</td>");
                                                    sb.Append("<td>");

                                                    sb.Append("<div style=\"padding-left:875px;\">");
                                                    if (Convert.ToInt32(productdetailInnerRep2.Tables[0].Rows[n]["IsApproved"]) == 0)
                                                    {
                                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                        sb.Append("Approve");
                                                        sb.Append("</a>");
                                                        sb.Append(" ");
                                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                        sb.Append("Disapprove");
                                                        sb.Append("</a>");
                                                        sb.Append(" ");
                                                    }
                                                    else if (Convert.ToInt32(productdetailInnerRep2.Tables[0].Rows[n]["IsApproved"]) == 1)
                                                    {
                                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                        sb.Append("Disapprove");
                                                        sb.Append("</a>");
                                                        sb.Append(" ");
                                                    }
                                                    else if (Convert.ToInt32(productdetailInnerRep2.Tables[0].Rows[n]["IsApproved"]) == 2)
                                                    {
                                                        sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                        sb.Append("Approve");
                                                        sb.Append("</a>");
                                                        sb.Append(" ");
                                                    }

                                                    sb.Append("<a href=\"javascript:void(0);\"  onclick=\"AdditionBox('txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "')\">");
                                                    sb.Append("Reply to R" + (n + 1));
                                                    sb.Append("</a>");
                                                    sb.Append("</div>");

                                                    sb.Append("<div class=\"txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "\" style=\"display:none;\" >");
                                                    sb.Append("<textarea ID=\"txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "\" name=\"txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;margin-left:410px;\"></textarea>");
                                                    sb.Append(" ");
                                                    sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "','txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "', 'InnerReply')\">");
                                                    sb.Append(" ");
                                                    sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaInnerReply" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "')\">");
                                                    sb.Append("</div>");
                                                    sb.Append("</td>");
                                                    sb.Append("</tr>");


                                                    DataSet productdetailInnerRep3 = new DataSet();
                                                    productdetailInnerRep3 = CommonComponent.GetCommonDataSet("select * from tb_QAReply_MVC where AnswerId = '" + productdetailInnerRep2.Tables[0].Rows[n]["ReplyId"] + "' and IsChild = 0");
                                                    if (productdetailInnerRep3 != null && productdetailInnerRep3.Tables.Count > 0 && productdetailInnerRep3.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int p = 0; p < productdetailInnerRep3.Tables[0].Rows.Count; p++)
                                                        {
                                                            // For InnerReply3
                                                            sb.Append("<tr class=\"oddrow\">");
                                                            sb.Append("<td colspan=\"2\" style=\"padding: 0 0 0 1000px;\"><h2 style=\"padding: 0 0 0 0px;\">R" + (p + 1) + " :</h2><textarea onclick=\"addtextInnerReply('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', this.value)\" ID=\"txtInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "\" name=\"txtInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "\" CssClass=\"order-textfield\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;\">" + productdetailInnerRep3.Tables[0].Rows[p]["Reply"] + "</textarea>");
                                                            sb.Append("<Label ID=\"Label1\" runat=\"server\" CssClass=\"order-label\"/> on " + string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(productdetailInnerRep3.Tables[0].Rows[p]["Createddate"].ToString())) + " By <Label ID=\"Label4\" runat=\"server\" CssClass=\"order-label\" />" + productdetailInnerRep3.Tables[0].Rows[p]["CreatedBy"]);
                                                            sb.Append("</td>");
                                                            sb.Append("</tr>");


                                                            sb.Append("<tr>");
                                                            sb.Append("<td>");
                                                            sb.Append("</td>");
                                                            sb.Append("<td>");

                                                            sb.Append("<div style=\"padding-left:1090px;\">");
                                                            if (Convert.ToInt32(productdetailInnerRep3.Tables[0].Rows[p]["IsApproved"]) == 0)
                                                            {
                                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                                sb.Append("Approve");
                                                                sb.Append("</a>");
                                                                sb.Append(" ");
                                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                                sb.Append("Disapprove");
                                                                sb.Append("</a>");
                                                                sb.Append(" ");
                                                            }
                                                            else if (Convert.ToInt32(productdetailInnerRep3.Tables[0].Rows[p]["IsApproved"]) == 1)
                                                            {
                                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', 'InnerReply', '2')\">");
                                                                sb.Append("Disapprove");
                                                                sb.Append("</a>");
                                                                sb.Append(" ");
                                                            }
                                                            else if (Convert.ToInt32(productdetailInnerRep3.Tables[0].Rows[p]["IsApproved"]) == 2)
                                                            {
                                                                sb.Append("<a href=\"javascript:void(0);\" onclick=\"ApproveDisapproveBox('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', 'InnerReply', '1')\">");
                                                                sb.Append("Approve");
                                                                sb.Append("</a>");
                                                                sb.Append(" ");
                                                            }

                                                            sb.Append("<a href=\"javascript:void(0);\" onclick=\"AdditionBox('txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "')\">");
                                                            sb.Append("Reply to R" + (p + 1));
                                                            sb.Append("</a>");
                                                            sb.Append("</div>");

                                                            sb.Append("<div class=\"txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "\" style=\"display:none;\" >");
                                                            sb.Append("<textarea ID=\"txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "\" name=\"txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "\" TextMode=\"MultiLine\" Columns=\"5\" style=\"width:400px;margin-left:410px;\"></textarea>");
                                                            sb.Append(" ");
                                                            sb.Append("<input type=\"image\" title=\"Save\" src=\"/App_Themes/Gray/images/save.gif\" alt=\"Save\" onclick=\"SaveBox('" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "','txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "', 'InnerReply')\">");
                                                            sb.Append(" ");
                                                            sb.Append("<input type=\"image\" title=\"Cancel\" src=\"/App_Themes/Gray/images/cancel.gif\" alt=\"Cancel\" onclick=\"CancelBox('txtAreaInnerReply" + productdetailInnerRep3.Tables[0].Rows[p]["ReplyId"] + "')\">");
                                                            sb.Append("</div>");
                                                            sb.Append("</td>");
                                                            sb.Append("</tr>");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //sb.Append("<input type=\"hidden\" value=\"\" id=\"hdnAllQustionIds\" runat=\"server\" />");                
                sb.Append("</table>");
            }
            litTable.Text = sb.ToString();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            //Question----------------
            var val = this.hdnAllQustionIds.Value;
            var questionIdnQuestion = val.Split('~');
            string[] formkeys = Request.Form.AllKeys;
            for (int i = 0; i < questionIdnQuestion.Length; i++)
            {
                try
                {
                    string strval = "";
                    foreach (String s in formkeys)
                    {
                        if (questionIdnQuestion[i].Trim() != "")
                        {
                            if (s.Contains("txtQuestion" + questionIdnQuestion[i].Trim()))
                            {
                                strval = Request.Form[s.ToString()];
                                break;
                            }
                        }
                    }
                    if (strval.Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please add some text to Question' " + questionIdnQuestion[i].Trim() + ", 'Message','');});", true);
                    }
                    else
                    {
                        CommonComponent.GetScalarCommonData("update tb_QAQuestion_MVC set Question = '" + strval.Replace("'", "''") + "' where QuestionId = '" + questionIdnQuestion[i].Trim() + "'");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //Question----------------

            //Answer----------------
            var valAns = this.hdnAllAnswerIds.Value;
            var answerIdnAnswer = valAns.Split('~');
            string[] formkeysAnswer = Request.Form.AllKeys;
            for (int i = 0; i < answerIdnAnswer.Length; i++)
            {
                try
                {
                    string strval = "";
                    foreach (String s in formkeysAnswer)
                    {
                        if (answerIdnAnswer[i].Trim() != "")
                        {
                            if (s.Contains("txtAnswer" + answerIdnAnswer[i].Trim()))
                            {
                                strval = Request.Form[s.ToString()];
                                break;
                            }
                        }
                    }
                    if (strval.Trim() == "")
                    {                        
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please add some text to Answer' " + answerIdnAnswer[i].Trim() + ", 'Message','');});", true);
                    }
                    else
                    {
                        CommonComponent.GetScalarCommonData("update tb_QAAnswer_MVC set Answer = '" + strval.Replace("'", "''") + "' where AnswerId = '" + answerIdnAnswer[i].Trim() + "'");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //Answer----------------

            //Reply----------------
            var valRep = this.hdnAllReplyIds.Value;
            var replyIdnReply = valRep.Split('~');
            string[] formkeysReply = Request.Form.AllKeys;
            for (int i = 0; i < replyIdnReply.Length; i++)
            {
                try
                {
                    string strval = "";
                    foreach (String s in formkeysReply)
                    {
                        if (replyIdnReply[i].Trim() != "")
                        {
                            if (s.Contains("txtReply" + replyIdnReply[i].Trim()))
                            {
                                strval = Request.Form[s.ToString()];
                                break;
                            }
                        }
                    }
                    if (strval.Trim() == "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please add some text to Reply' " + replyIdnReply[i].Trim() + ", 'Message','');});", true);
                    }
                    else
                    {
                        CommonComponent.GetScalarCommonData("update tb_QAReply_MVC set Reply = '" + strval.Replace("'", "''") + "' where ReplyId = '" + replyIdnReply[i].Trim() + "'");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //Reply----------------

            //Inner Reply----------------
            var valInnerRep = this.hdnAllReplyIds.Value;
            var replyIdnInnerReply = valInnerRep.Split('~');
            string[] formkeysInnerReply = Request.Form.AllKeys;
            for (int i = 0; i < replyIdnInnerReply.Length; i++)
            {
                try
                {
                    string strval = "";
                    foreach (String s in formkeysInnerReply)
                    {
                        if (replyIdnInnerReply[i].Trim() != "")
                        {
                            if (s.Contains("txtInnerReply" + replyIdnInnerReply[i].Trim()))
                            {
                                strval = Request.Form[s.ToString()];
                                break;
                            }
                        }
                    }
                    CommonComponent.GetScalarCommonData("update tb_QAReply_MVC set Reply = '" + strval.Replace("'", "''") + "' where ReplyId = '" + replyIdnInnerReply[i].Trim() + "'");
                }
                catch (Exception ex)
                {

                }
            }
            //Inner Reply----------------

            Response.Redirect("ManageQuestionAnswer.aspx?status=inserted");


        }


        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgAddQustionSave_Click(object sender, ImageClickEventArgs e)
        {
            //Question----------------
            var val = this.hdnAllQustionIds.Value;
            var questionIdnQuestion = val.Split('~');
            string[] formkeys = Request.Form.AllKeys;
            for (int i = 0; i < questionIdnQuestion.Length; i++)
            {
                try
                {
                    string strval = "";
                    foreach (String s in formkeys)
                    {
                        if (s.Contains("txtQuestion" + questionIdnQuestion[i].Trim()))
                        {
                            strval = Request.Form[s.ToString()];
                            break;
                        }
                    }
                    CommonComponent.GetScalarCommonData("update tb_QAQuestion_MVC set Question = '" + strval.Replace("'", "''") + "' where QuestionId = '" + questionIdnQuestion[i].Trim() + "'");
                }
                catch (Exception ex)
                {

                }
            }
            //Question----------------

            Response.Redirect("ManageQuestionAnswer.aspx?status=inserted");
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ManageQuestionAnswer.aspx");
        }

    }
}