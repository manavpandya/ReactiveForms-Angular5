using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Castle.Web.Controls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ManageQuestionAnswer : BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        public static bool isDescendName = false;
        public static bool isDescendDate = false;
        ProductComponent procomp = new ProductComponent();
        tb_Rating tb_rating = new tb_Rating();

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
                if (Request.QueryString["status"] != null)
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Q/A/R operation completed successfully.', 'Message','');});", true);
                    }
                }
                BindStore();
                BindGrid();
                btnapproverating.ImageUrl = "/App_Themes/" + Page.Theme + "/images/approveqar.png";
                btnDisapproverating.ImageUrl = "/App_Themes/" + Page.Theme + "/images/disapproveqar.png";
            }
            else
            {
                bool checkBool = false;
                int totalRowCount = grdQA.Rows.Count;
                for (int i = 0; i < totalRowCount; i++)
                {
                    HiddenField hdn = (HiddenField)grdQA.Rows[i].FindControl("hdnQARId");
                    HiddenField hdnQAR = (HiddenField)grdQA.Rows[i].FindControl("hdnQARValue");
                    CheckBox chk = (CheckBox)grdQA.Rows[i].FindControl("chkselect");
                    if (chk.Checked == true)
                    {
                        checkBool = true;
                        break;
                    }
                }
                if (checkBool == false)
                {                    
                    BindGrid();                    
                }
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                drpstore.DataSource = Storelist;
                drpstore.DataTextField = "StoreName";
                drpstore.DataValueField = "StoreID";
            }
            else
            {
                drpstore.DataSource = null;
            }
            drpstore.DataBind();
            drpstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                drpstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdQA.PageIndex = e.NewPageIndex;
            //grdQA.DataBind();

            Solution.Data.ProductDAC dac = new Solution.Data.ProductDAC();
            System.Data.DataSet results1 = new System.Data.DataSet();

            System.Data.DataTable dtTempTable = new System.Data.DataTable();
            dtTempTable.Columns.Add("QuestionId", typeof(int));
            dtTempTable.Columns.Add("ProductId", typeof(int));
            dtTempTable.Columns.Add("StoreID", typeof(int));
            dtTempTable.Columns.Add("ProductName", typeof(string));
            dtTempTable.Columns.Add("Question", typeof(string));
            dtTempTable.Columns.Add("QCreatedBy", typeof(string));
            dtTempTable.Columns.Add("QCreatedOn", typeof(string));
            dtTempTable.Columns.Add("QuestionIdOfAnswer", typeof(int));
            dtTempTable.Columns.Add("AnswerId", typeof(int));
            dtTempTable.Columns.Add("Answer", typeof(string));
            dtTempTable.Columns.Add("ACreatedBy", typeof(string));
            dtTempTable.Columns.Add("ACreatedOn", typeof(string));
            dtTempTable.Columns.Add("AnswerIdOfReply", typeof(int));
            dtTempTable.Columns.Add("ReplyId", typeof(int));
            dtTempTable.Columns.Add("Reply", typeof(string));
            dtTempTable.Columns.Add("RCreatedBy", typeof(string));
            dtTempTable.Columns.Add("RCreatedOn", typeof(string));
            dtTempTable.Columns.Add("IsApproved_Questions", typeof(int));
            dtTempTable.Columns.Add("IsApproved_Answer", typeof(int));
            dtTempTable.Columns.Add("IsApproved_Reply", typeof(int));
            dtTempTable.Columns.Add("Ischild_Reply", typeof(int));

            if (ddlQAstatus.SelectedValue.ToString() == "0")
            {
                results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 0);
                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), 0, "", "", "", 0, 0, "", "", "", 0, 0, 0, 0);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else if (ddlQAstatus.SelectedValue.ToString() == "1")
            {
                results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 1);
                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            System.Data.DataRow[] result = results1.Tables[1].Select("CTEReplyId = '" + results1.Tables[0].Rows[i]["ReplyId"] + "'");
                            dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ReplyId"]), Convert.ToString(results1.Tables[0].Rows[i]["Reply"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Reply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["Ischild_Reply"]));
                            foreach (System.Data.DataRow rowTemp in result)
                            {
                                if (Convert.ToString(rowTemp["ReplyId"]) != "")
                                {
                                    dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), rowTemp["ReplyId"], Convert.ToString(rowTemp["Reply"]), Convert.ToString(rowTemp["CreatedBy"]), Convert.ToString(rowTemp["CreatedDate"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), rowTemp["IsApproved"], rowTemp["Ischild"]);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else if (ddlQAstatus.SelectedValue.ToString() == "-1")
            {
                results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 2);
                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), 0, "", "", "", 0, 0, "", "", "", 2, 0, 0, 0);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

           

            results1.Tables.Clear();
            results1.Tables.Add(dtTempTable);
            
          
            System.Data.DataTable dtAll = new System.Data.DataTable();
            dtAll.Columns.Add("QARId", typeof(int));
            dtAll.Columns.Add("ProductId", typeof(int));
            dtAll.Columns.Add("StoreId", typeof(int));
            dtAll.Columns.Add("ProductName", typeof(string));
            dtAll.Columns.Add("Question", typeof(string));
            dtAll.Columns.Add("Answer", typeof(string));
            dtAll.Columns.Add("Reply", typeof(string));
            dtAll.Columns.Add("CreatedBy", typeof(string));
            dtAll.Columns.Add("CreatedOn", typeof(string));
            dtAll.Columns.Add("QAR", typeof(string));
            dtAll.Columns.Add("ApproveImage", typeof(string));
            dtAll.Columns.Add("DisapproveImage", typeof(string));

            if (results1.Tables.Count > 0)
            {
                try
                {
                    int QidCompare = 0;
                    int AidCompare = 0;
                    int RidCompare = 0;
                    for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                    {
                        System.Data.DataRow[] result = results1.Tables[0].Select("QuestionId = '" + results1.Tables[0].Rows[i]["QuestionId"] + "'");
                        ///===================Question====================  
                        if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 0)
                        {
                            dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                            continue;
                        }
                        else if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 2)
                        {
                            dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                            continue;
                        }

                        foreach (System.Data.DataRow rowQuestion in result)
                        {
                            if (QidCompare != Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]))
                            {
                                if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 0)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                }
                                else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 1)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "", "/App_Themes/gray/images/disapprove.png");
                                }
                                else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 2)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                }
                                ///===================Answer====================
                                foreach (System.Data.DataRow rowAnswer in result)
                                {

                                    if (AidCompare != Convert.ToInt32(rowAnswer["AnswerId"]))
                                    {
                                        if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 0)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                        }
                                        else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 1)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "", "/App_Themes/gray/images/disapprove.png");
                                        }
                                        else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 2)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "");
                                        }
                                        ///===================Reply====================
                                        foreach (System.Data.DataRow rowReply in result)
                                        {
                                            if (Convert.ToInt32(rowAnswer["AnswerId"]) == Convert.ToInt32(rowReply["AnswerId"]))
                                            {
                                                if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 0 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                                }
                                                else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 1 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "", "/App_Themes/gray/images/disapprove.png");
                                                }
                                                else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 2 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "");
                                                }
                                            }
                                            RidCompare = Convert.ToInt32(rowReply["ReplyId"]);
                                        }
                                        ///===================Reply====================
                                    }
                                    AidCompare = Convert.ToInt32(rowAnswer["AnswerId"]);
                                }
                                ///===================Answer====================
                            }
                            QidCompare = Convert.ToInt32(rowQuestion["QuestionId"]);
                        }
                        ///===================Question====================                       
                    }


                }
                catch (Exception ex)
                {
                }
                results1.Tables.Clear();
                results1.Tables.Add(dtAll);
                grdQA.DataSource = dtAll;
                grdQA.DataBind();
            }
        }

        public void BindGrid()
        {
            Solution.Data.ProductDAC dac = new Solution.Data.ProductDAC();
            System.Data.DataSet results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 0); //Mode 0 means status = pending
           
            System.Data.DataTable dtTempTable = new System.Data.DataTable();
            dtTempTable.Columns.Add("QuestionId", typeof(int));
            dtTempTable.Columns.Add("ProductId", typeof(int));            
            dtTempTable.Columns.Add("StoreID", typeof(int));
            dtTempTable.Columns.Add("ProductName", typeof(string));
            dtTempTable.Columns.Add("Question", typeof(string));
            dtTempTable.Columns.Add("QCreatedBy", typeof(string));
            dtTempTable.Columns.Add("QCreatedOn", typeof(string));
            dtTempTable.Columns.Add("QuestionIdOfAnswer", typeof(int));
            dtTempTable.Columns.Add("AnswerId", typeof(int));
            dtTempTable.Columns.Add("Answer", typeof(string));
            dtTempTable.Columns.Add("ACreatedBy", typeof(string));
            dtTempTable.Columns.Add("ACreatedOn", typeof(string));
            dtTempTable.Columns.Add("AnswerIdOfReply", typeof(int));
            dtTempTable.Columns.Add("ReplyId", typeof(int));
            dtTempTable.Columns.Add("Reply", typeof(string));
            dtTempTable.Columns.Add("RCreatedBy", typeof(string));
            dtTempTable.Columns.Add("RCreatedOn", typeof(string));
            dtTempTable.Columns.Add("IsApproved_Questions", typeof(int));
            dtTempTable.Columns.Add("IsApproved_Answer", typeof(int));
            dtTempTable.Columns.Add("IsApproved_Reply", typeof(int));
            dtTempTable.Columns.Add("Ischild_Reply", typeof(int));

            if (results1.Tables.Count > 0)
            {
                try
                {
                    for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                    {
                        //System.Data.DataRow[] result = results1.Tables[1].Select("CTEReplyId = '" + results1.Tables[0].Rows[i]["ReplyId"] + "'");
                        //dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ReplyId"]), Convert.ToString(results1.Tables[0].Rows[i]["Reply"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Reply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["Ischild_Reply"]));
                        //foreach (System.Data.DataRow rowTemp in result)
                        //{
                        //    if (Convert.ToString(rowTemp["ReplyId"]) != "")
                        //    {
                        //        dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), rowTemp["ReplyId"], Convert.ToString(rowTemp["Reply"]), Convert.ToString(rowTemp["CreatedBy"]), Convert.ToString(rowTemp["CreatedDate"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), rowTemp["IsApproved"], rowTemp["Ischild"]);
                        //    }                            
                        //}
                        dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), 0, "", "", "", 0, 0, "", "", "", 0, 0, 0, 0);
                    }
                }
                catch (Exception ex)
                {
                   
                }
            }

            results1.Tables.Clear();
            results1.Tables.Add(dtTempTable);

            System.Data.DataTable dtAll = new System.Data.DataTable();
            dtAll.Columns.Add("QARId", typeof(int));
            dtAll.Columns.Add("ProductId", typeof(int));
            dtAll.Columns.Add("StoreId", typeof(int));
            dtAll.Columns.Add("ProductName", typeof(string));
            dtAll.Columns.Add("Question", typeof(string));
            dtAll.Columns.Add("Answer", typeof(string));
            dtAll.Columns.Add("Reply", typeof(string));
            dtAll.Columns.Add("CreatedBy", typeof(string));
            dtAll.Columns.Add("CreatedOn", typeof(string));
            dtAll.Columns.Add("QAR", typeof(string));
            dtAll.Columns.Add("ApproveImage", typeof(string));
            dtAll.Columns.Add("DisapproveImage", typeof(string));

            if (results1.Tables.Count > 0)
            {
                try
                {
                    int QidCompare = 0;
                    int AidCompare = 0;
                    int RidCompare = 0;
                    for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                    {
                        System.Data.DataRow[] result = results1.Tables[0].Select("QuestionId = '" + results1.Tables[0].Rows[i]["QuestionId"] + "'");
                        ///===================Question====================  
                        if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 0)
                        {
                            dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                            continue;
                        }
                        else if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 2)
                        {
                            dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                            continue;
                        }

                        foreach (System.Data.DataRow rowQuestion in result)
                        {
                            if (QidCompare != Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]))
                            {
                                if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 0)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                }
                                else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 1)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "", "/App_Themes/gray/images/disapprove.png");
                                }
                                else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 2)
                                {
                                    dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                }
                                ///===================Answer====================
                                foreach (System.Data.DataRow rowAnswer in result)
                                {

                                    if (AidCompare != Convert.ToInt32(rowAnswer["AnswerId"]))
                                    {
                                        if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 0)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                        }
                                        else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 1)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "", "/App_Themes/gray/images/disapprove.png");
                                        }
                                        else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 2)
                                        {
                                            dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "");
                                        }
                                        ///===================Reply====================
                                        foreach (System.Data.DataRow rowReply in result)
                                        {
                                            if (Convert.ToInt32(rowAnswer["AnswerId"]) == Convert.ToInt32(rowReply["AnswerId"]))
                                            {
                                                if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 0 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                                }
                                                else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 1 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "", "/App_Themes/gray/images/disapprove.png");
                                                }
                                                else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 2 && Convert.ToString(rowReply["Reply"]) != "")
                                                {
                                                    dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "");
                                                }
                                            }
                                            RidCompare = Convert.ToInt32(rowReply["ReplyId"]);
                                        }
                                        ///===================Reply====================
                                    }
                                    AidCompare = Convert.ToInt32(rowAnswer["AnswerId"]);
                                }
                                ///===================Answer====================
                            }
                            QidCompare = Convert.ToInt32(rowQuestion["QuestionId"]);
                        }
                        ///===================Question====================                       
                    }


                }
                catch (Exception ex)
                {
                }
                results1.Tables.Clear();
                results1.Tables.Add(dtAll);
                grdQA.DataSource = dtAll;
                grdQA.DataBind();
            }
        }

        public void BindGridAscDesc(string SortType, string CommandName)
        {
            Solution.Data.ProductDAC dac = new Solution.Data.ProductDAC();
            System.Data.DataSet results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 0); //Mode 0 means status = pending
            System.Data.DataTable dtAll = new System.Data.DataTable();
            //dtAll = results1.Tables[0].Copy();
            ////dtAll.Merge(results1.Tables[1], true);
            ////dtAll.Merge(results1.Tables[2], true);

            dtAll.Columns.Add("QARId", typeof(int));
            dtAll.Columns.Add("ProductId", typeof(int));
            dtAll.Columns.Add("StoreId", typeof(int));
            dtAll.Columns.Add("ProductName", typeof(string));
            dtAll.Columns.Add("Question", typeof(string));
            dtAll.Columns.Add("Answer", typeof(string));
            dtAll.Columns.Add("Reply", typeof(string));
            dtAll.Columns.Add("CreatedBy", typeof(string));
            dtAll.Columns.Add("CreatedOn", typeof(string));
            dtAll.Columns.Add("QAR", typeof(string));

            if (results1.Tables.Count > 0)
            {
                try
                {
                    int QidCompare = 0;
                    int AidCompare = 0;
                    int RidCompare = 0;
                    for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                    {
                        System.Data.DataRow[] result = results1.Tables[0].Select("QuestionId = '" + results1.Tables[0].Rows[i]["QuestionId"] + "'");
                        ///===================Question====================                       
                        foreach (System.Data.DataRow rowQuestion in result)
                        {
                            if (QidCompare != Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]))
                            {
                                dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question");

                                ///===================Answer====================
                                foreach (System.Data.DataRow rowAnswer in result)
                                {

                                    if (AidCompare != Convert.ToInt32(rowAnswer["AnswerId"]))
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer");

                                        ///===================Reply====================
                                        foreach (System.Data.DataRow rowReply in result)
                                        {
                                            if (Convert.ToInt32(rowAnswer["AnswerId"]) == Convert.ToInt32(rowReply["AnswerId"]))
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply");
                                            }
                                            RidCompare = Convert.ToInt32(rowReply["ReplyId"]);
                                        }
                                        ///===================Reply====================
                                    }
                                    AidCompare = Convert.ToInt32(rowAnswer["AnswerId"]);
                                }
                                ///===================Answer====================
                            }
                            QidCompare = Convert.ToInt32(rowQuestion["QuestionId"]);
                        }
                        ///===================Question====================                       
                    }


                }
                catch (Exception ex)
                {
                }
                dtAll = results1.Tables[0];
                results1.Tables.Clear();
            }

            System.Data.DataView dv = dtAll.DefaultView;
            dv.Sort = CommandName + " " + SortType;
            grdQA.DataSource = dv.ToTable();
            grdQA.DataBind();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void drpstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdQA.PageIndex = 0;
            grdQA.DataBind();
            if (grdQA.Rows.Count > 0)
            {
                if (ddlQAstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

        /// <summary>
        /// Review Status Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlQAstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdQA.PageIndex = 0;
            //grdQA.DataBind();
            //if (grdQA.Rows.Count > 0)
            //{
            Solution.Data.ProductDAC dac = new Solution.Data.ProductDAC();
            if (ddlQAstatus.SelectedValue.ToString() == "0")
            {
                btnapproverating.Visible = true;
                btnDisapproverating.Visible = true;
                BindGrid();
            }
            else if (ddlQAstatus.SelectedValue.ToString() == "1")
            {
                btnapproverating.Visible = false;
                btnDisapproverating.Visible = true;

                System.Data.DataSet results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 1); //Mode 1 means status = Approved
                System.Data.DataTable dtAll = new System.Data.DataTable();

                System.Data.DataTable dtTempTable = new System.Data.DataTable();
                dtTempTable.Columns.Add("QuestionId", typeof(int));
                dtTempTable.Columns.Add("ProductId", typeof(int));
                dtTempTable.Columns.Add("StoreID", typeof(int));
                dtTempTable.Columns.Add("ProductName", typeof(string));
                dtTempTable.Columns.Add("Question", typeof(string));
                dtTempTable.Columns.Add("QCreatedBy", typeof(string));
                dtTempTable.Columns.Add("QCreatedOn", typeof(string));
                dtTempTable.Columns.Add("QuestionIdOfAnswer", typeof(int));
                dtTempTable.Columns.Add("AnswerId", typeof(int));
                dtTempTable.Columns.Add("Answer", typeof(string));
                dtTempTable.Columns.Add("ACreatedBy", typeof(string));
                dtTempTable.Columns.Add("ACreatedOn", typeof(string));
                dtTempTable.Columns.Add("AnswerIdOfReply", typeof(int));
                dtTempTable.Columns.Add("ReplyId", typeof(int));
                dtTempTable.Columns.Add("Reply", typeof(string));
                dtTempTable.Columns.Add("RCreatedBy", typeof(string));
                dtTempTable.Columns.Add("RCreatedOn", typeof(string));
                dtTempTable.Columns.Add("IsApproved_Questions", typeof(int));
                dtTempTable.Columns.Add("IsApproved_Answer", typeof(int));
                dtTempTable.Columns.Add("IsApproved_Reply", typeof(int));
                dtTempTable.Columns.Add("Ischild_Reply", typeof(int));

                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            System.Data.DataRow[] result = results1.Tables[1].Select("CTEReplyId = '" + Convert.ToString(results1.Tables[0].Rows[i]["ReplyId"]) + "'");
                            dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ReplyId"]), Convert.ToString(results1.Tables[0].Rows[i]["Reply"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Reply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["Ischild_Reply"]));
                            foreach (System.Data.DataRow rowTemp in result)
                            {
                                if (Convert.ToString(rowTemp["ReplyId"]) != "")
                                {
                                    dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), rowTemp["ReplyId"], Convert.ToString(rowTemp["Reply"]), Convert.ToString(rowTemp["CreatedBy"]), Convert.ToString(rowTemp["CreatedDate"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), rowTemp["IsApproved"], rowTemp["Ischild"]);
                                }

                            }                            
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                results1.Tables.Clear();
                results1.Tables.Add(dtTempTable);
                
                dtAll.Columns.Add("QARId", typeof(int));
                dtAll.Columns.Add("ProductId", typeof(int));
                dtAll.Columns.Add("StoreId", typeof(int));
                dtAll.Columns.Add("ProductName", typeof(string));
                dtAll.Columns.Add("Question", typeof(string));
                dtAll.Columns.Add("Answer", typeof(string));
                dtAll.Columns.Add("Reply", typeof(string));
                dtAll.Columns.Add("CreatedBy", typeof(string));
                dtAll.Columns.Add("CreatedOn", typeof(string));
                dtAll.Columns.Add("QAR", typeof(string));
                dtAll.Columns.Add("ApproveImage", typeof(string));
                dtAll.Columns.Add("DisapproveImage", typeof(string));

                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        int QidCompare = 0;
                        int AidCompare = 0;
                        int RidCompare = 0;
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            System.Data.DataRow[] result = results1.Tables[0].Select("QuestionId = '" + results1.Tables[0].Rows[i]["QuestionId"] + "'");
                            ///===================Question====================  
                            if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 0)
                            {
                                dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                continue;
                            }
                            else if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 2)
                            {
                                dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                continue;
                            }

                            foreach (System.Data.DataRow rowQuestion in result)
                            {
                                if (QidCompare != Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]))
                                {
                                    if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 0)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                    }
                                    else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 1)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "", "/App_Themes/gray/images/disapprove.png");
                                    }
                                    else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 2)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                    }
                                    ///===================Answer====================
                                    foreach (System.Data.DataRow rowAnswer in result)
                                    {

                                        if (AidCompare != Convert.ToInt32(rowAnswer["AnswerId"]))
                                        {
                                            if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 0)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                            }
                                            else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 1)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "", "/App_Themes/gray/images/disapprove.png");
                                            }
                                            else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 2)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "");
                                            }
                                            ///===================Reply====================
                                            foreach (System.Data.DataRow rowReply in result)
                                            {
                                                if (Convert.ToInt32(rowAnswer["AnswerId"]) == Convert.ToInt32(rowReply["AnswerId"]))
                                                {
                                                    if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 0 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                                    }
                                                    else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 1 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "", "/App_Themes/gray/images/disapprove.png");
                                                    }
                                                    else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 2 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "");
                                                    }
                                                }
                                                RidCompare = Convert.ToInt32(rowReply["ReplyId"]);
                                            }
                                            ///===================Reply====================
                                        }
                                        AidCompare = Convert.ToInt32(rowAnswer["AnswerId"]);
                                    }
                                    ///===================Answer====================
                                }
                                QidCompare = Convert.ToInt32(rowQuestion["QuestionId"]);
                            }
                            ///===================Question====================                       
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    results1.Tables.Clear();
                    results1.Tables.Add(dtAll);
                    grdQA.DataSource = dtAll;
                    grdQA.DataBind();
                }

            }
            else if (ddlQAstatus.SelectedValue.ToString() == "-1")
            {
                btnapproverating.Visible = true;
                btnDisapproverating.Visible = false;

                System.Data.DataSet results1 = dac.GetQAR(Convert.ToInt32(drpstore.SelectedValue), 2); //Mode 2 means status = Disapproved
                System.Data.DataTable dtAll = new System.Data.DataTable();

                System.Data.DataTable dtTempTable = new System.Data.DataTable();
                dtTempTable.Columns.Add("QuestionId", typeof(int));
                dtTempTable.Columns.Add("ProductId", typeof(int));
                dtTempTable.Columns.Add("StoreID", typeof(int));
                dtTempTable.Columns.Add("ProductName", typeof(string));
                dtTempTable.Columns.Add("Question", typeof(string));
                dtTempTable.Columns.Add("QCreatedBy", typeof(string));
                dtTempTable.Columns.Add("QCreatedOn", typeof(string));
                dtTempTable.Columns.Add("QuestionIdOfAnswer", typeof(int));
                dtTempTable.Columns.Add("AnswerId", typeof(int));
                dtTempTable.Columns.Add("Answer", typeof(string));
                dtTempTable.Columns.Add("ACreatedBy", typeof(string));
                dtTempTable.Columns.Add("ACreatedOn", typeof(string));
                dtTempTable.Columns.Add("AnswerIdOfReply", typeof(int));
                dtTempTable.Columns.Add("ReplyId", typeof(int));
                dtTempTable.Columns.Add("Reply", typeof(string));
                dtTempTable.Columns.Add("RCreatedBy", typeof(string));
                dtTempTable.Columns.Add("RCreatedOn", typeof(string));
                dtTempTable.Columns.Add("IsApproved_Questions", typeof(int));
                dtTempTable.Columns.Add("IsApproved_Answer", typeof(int));
                dtTempTable.Columns.Add("IsApproved_Reply", typeof(int));
                dtTempTable.Columns.Add("Ischild_Reply", typeof(int));

                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            //System.Data.DataRow[] result = results1.Tables[1].Select("CTEReplyId = '" + results1.Tables[0].Rows[i]["ReplyId"] + "'");
                            //dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ReplyId"]), Convert.ToString(results1.Tables[0].Rows[i]["Reply"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["RCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Reply"]), Convert.ToInt32(results1.Tables[0].Rows[i]["Ischild_Reply"]));
                            //foreach (System.Data.DataRow rowTemp in result)
                            //{
                            //    if (Convert.ToString(rowTemp["ReplyId"]) != "")
                            //    {
                            //        dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerId"]), Convert.ToString(results1.Tables[0].Rows[i]["Answer"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["ACreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["AnswerIdOfReply"]), rowTemp["ReplyId"], Convert.ToString(rowTemp["Reply"]), Convert.ToString(rowTemp["CreatedBy"]), Convert.ToString(rowTemp["CreatedDate"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]), Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Answer"]), rowTemp["IsApproved"], rowTemp["Ischild"]);
                            //    }

                            //}
                            dtTempTable.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionIdOfAnswer"]), 0, "", "", "", 0, 0, "", "", "", 2, 0, 0, 0);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                results1.Tables.Clear();
                results1.Tables.Add(dtTempTable);
                
                dtAll.Columns.Add("QARId", typeof(int));
                dtAll.Columns.Add("ProductId", typeof(int));
                dtAll.Columns.Add("StoreId", typeof(int));
                dtAll.Columns.Add("ProductName", typeof(string));
                dtAll.Columns.Add("Question", typeof(string));
                dtAll.Columns.Add("Answer", typeof(string));
                dtAll.Columns.Add("Reply", typeof(string));
                dtAll.Columns.Add("CreatedBy", typeof(string));
                dtAll.Columns.Add("CreatedOn", typeof(string));
                dtAll.Columns.Add("QAR", typeof(string));
                dtAll.Columns.Add("ApproveImage", typeof(string));
                dtAll.Columns.Add("DisapproveImage", typeof(string));

                if (results1.Tables.Count > 0)
                {
                    try
                    {
                        int QidCompare = 0;
                        int AidCompare = 0;
                        int RidCompare = 0;
                        for (int i = 0; i < results1.Tables[0].Rows.Count; i++)
                        {
                            System.Data.DataRow[] result = results1.Tables[0].Select("QuestionId = '" + results1.Tables[0].Rows[i]["QuestionId"] + "'");
                            ///===================Question====================  
                            if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 0)
                            {
                                dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                continue;
                            }
                            else if (Convert.ToInt32(results1.Tables[0].Rows[i]["IsApproved_Questions"]) == 2)
                            {
                                dtAll.Rows.Add(Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["ProductId"]), Convert.ToInt32(results1.Tables[0].Rows[i]["StoreId"]), Convert.ToString(results1.Tables[0].Rows[i]["ProductName"]), Convert.ToString(results1.Tables[0].Rows[i]["Question"]), "", "", Convert.ToString(results1.Tables[0].Rows[i]["QCreatedBy"]), Convert.ToString(results1.Tables[0].Rows[i]["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                continue;
                            }

                            foreach (System.Data.DataRow rowQuestion in result)
                            {
                                if (QidCompare != Convert.ToInt32(results1.Tables[0].Rows[i]["QuestionId"]))
                                {
                                    if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 0)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                    }
                                    else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 1)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "", "/App_Themes/gray/images/disapprove.png");
                                    }
                                    else if (Convert.ToInt32(rowQuestion["IsApproved_Questions"]) == 2)
                                    {
                                        dtAll.Rows.Add(Convert.ToInt32(rowQuestion["QuestionId"]), Convert.ToInt32(rowQuestion["ProductId"]), Convert.ToInt32(rowQuestion["StoreId"]), Convert.ToString(rowQuestion["ProductName"]), Convert.ToString(rowQuestion["Question"]), "", "", Convert.ToString(rowQuestion["QCreatedBy"]), Convert.ToString(rowQuestion["QCreatedOn"]), "Question", "/App_Themes/gray/images/approve.png", "");
                                    }
                                    ///===================Answer====================
                                    foreach (System.Data.DataRow rowAnswer in result)
                                    {

                                        if (AidCompare != Convert.ToInt32(rowAnswer["AnswerId"]))
                                        {
                                            if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 0)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                            }
                                            else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 1)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "", "/App_Themes/gray/images/disapprove.png");
                                            }
                                            else if (Convert.ToInt32(rowAnswer["IsApproved_Answer"]) == 2)
                                            {
                                                dtAll.Rows.Add(Convert.ToInt32(rowAnswer["AnswerId"]), Convert.ToInt32(rowAnswer["ProductId"]), Convert.ToInt32(rowAnswer["StoreId"]), Convert.ToString(rowAnswer["ProductName"]), "", Convert.ToString(rowAnswer["Answer"]), "", Convert.ToString(rowAnswer["ACreatedBy"]), Convert.ToString(rowAnswer["ACreatedOn"]), "Answer", "/App_Themes/gray/images/approve.png", "");
                                            }
                                            ///===================Reply====================
                                            foreach (System.Data.DataRow rowReply in result)
                                            {
                                                if (Convert.ToInt32(rowAnswer["AnswerId"]) == Convert.ToInt32(rowReply["AnswerId"]) )
                                                {
                                                    if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 0 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "/App_Themes/gray/images/disapprove.png");
                                                    }
                                                    else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 1 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "", "/App_Themes/gray/images/disapprove.png");
                                                    }
                                                    else if (Convert.ToInt32(rowReply["IsApproved_Reply"]) == 2 && Convert.ToString(rowReply["Reply"]) != "")
                                                    {
                                                        dtAll.Rows.Add(Convert.ToInt32(rowReply["ReplyId"]), Convert.ToInt32(rowReply["ProductId"]), Convert.ToInt32(rowReply["StoreId"]), Convert.ToString(rowReply["ProductName"]), "", "", Convert.ToString(rowReply["Reply"]), Convert.ToString(rowReply["RCreatedBy"]), Convert.ToString(rowReply["RCreatedOn"]), "Reply", "/App_Themes/gray/images/approve.png", "");
                                                    }
                                                }
                                                RidCompare = Convert.ToInt32(rowReply["ReplyId"]);
                                            }
                                            ///===================Reply====================
                                        }
                                        AidCompare = Convert.ToInt32(rowAnswer["AnswerId"]);
                                    }
                                    ///===================Answer====================
                                }
                                QidCompare = Convert.ToInt32(rowQuestion["QuestionId"]);
                            }
                            ///===================Question====================                       
                        }


                    }
                    catch (Exception ex)
                    {
                    }
                    results1.Tables.Clear();
                    results1.Tables.Add(dtAll);
                    grdQA.DataSource = dtAll;
                    grdQA.DataBind();
                }

            }
            trBottom.Visible = true;
            //}
        }

        /// <summary>
        /// Product Review Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdQA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ddlQAstatus.SelectedValue == "1")
            {
                //  grdProductReview.Columns[0].Visible = false;
                grdQA.Columns[6].Visible = false;
                grdQA.Columns[7].Visible = true;
                //  trBottom.Visible = false;
            }
            else if (ddlQAstatus.SelectedValue == "0")
            {
                // grdProductReview.Columns[0].Visible = false;
                grdQA.Columns[6].Visible = true;
                grdQA.Columns[7].Visible = true;
                //if (grdProductReview.Rows.Count > 0)
                //   // trBottom.Visible = false;
                //else
                //   // trBottom.Visible = false;
            }
            else if (ddlQAstatus.SelectedValue == "-1")
            {
                //   grdProductReview.Columns[0].Visible = false;
                grdQA.Columns[6].Visible = false;
                grdQA.Columns[7].Visible = true;
                //if (grdProductReview.Rows.Count > 0)
                //   // trBottom.Visible = false;
                //else
                //    //trBottom.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //if (isDescendName == false)
                //{
                //    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                //    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                //    lbName.AlternateText = "Ascending Order";
                //    lbName.ToolTip = "Ascending Order";
                //    lbName.CommandArgument = "DESC";
                //}
                //else
                //{
                //    ImageButton lbName = (ImageButton)e.Row.FindControl("lbName");
                //    lbName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                //    lbName.AlternateText = "Descending Order";
                //    lbName.ToolTip = "Descending Order";
                //    lbName.CommandArgument = "ASC";
                //}
                //if (isDescendDate == false)
                //{
                //    ImageButton lbDate = (ImageButton)e.Row.FindControl("lbDate");
                //    lbDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                //    lbDate.AlternateText = "Ascending Order";
                //    lbDate.ToolTip = "Ascending Order";
                //    lbDate.CommandArgument = "DESC";
                //}
                //else
                //{
                //    ImageButton lbDate = (ImageButton)e.Row.FindControl("lbDate");
                //    lbDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                //    lbDate.AlternateText = "Descending Order";
                //    lbDate.ToolTip = "Descending Order";
                //    lbDate.CommandArgument = "ASC";
                //}
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton btnapprove = (ImageButton)e.Row.FindControl("btnApprove");
            //    ImageButton btnUnApprove = (ImageButton)e.Row.FindControl("btnUnApprove");

            //    Rater rating = (Rater)e.Row.FindControl("Rater");
            //    btnapprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/approve.png";
            //    btnUnApprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/disapprove.png";
            //    //rating.ImageOnUrl = "/App_Themes/" + Page.Theme + "/images/star-form.jpg";
            //    //rating.ImageOffUrl = "/App_Themes/" + Page.Theme + "/images/star-form1.jpg";

            //    if (ddlQAstatus.SelectedValue == "0")
            //    {

            //        HiddenField hdn = (HiddenField)e.Row.FindControl("hdnQARId");
            //        LinkButton lnkviewImage = (LinkButton)e.Row.FindControl("lnkviewImage");



            //        //System.Data.DataSet dtImageRating = new System.Data.DataSet();
            //        //dtImageRating = CommonComponent.GetCommonDataSet("select * from tb_RatingImage where RatingID = " + hdn.Value + " and isnull(IsApproved,0)=0 and isnull(Deleted,0)=0");

            //        //if (dtImageRating != null && dtImageRating.Tables.Count > 0 && dtImageRating.Tables[0].Rows.Count > 0)
            //        //{
            //        //    Panel pnlImage = new Panel();
            //        //    pnlImage.ID = "pnl_" + hdn.Value;
            //        //    pnlImage.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //        //    string RatingIconPath = string.Concat(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("ImagePathProduct").ToString(), "RatingReview/");
            //        //    for (int i = 0; i < dtImageRating.Tables[0].Rows.Count; i++)
            //        //    {
            //        //        string imagePath = string.Empty;
            //        //        imagePath = string.Concat(RatingIconPath, Convert.ToString(dtImageRating.Tables[0].Rows[i]["ImageName"]));
            //        //        string RatingImageName = Convert.ToString(dtImageRating.Tables[0].Rows[i]["ImageName"]);
            //        //        string RatingImageID = Convert.ToString(dtImageRating.Tables[0].Rows[i]["RatingImageID"]);
            //        //        Panel pnlImageInner = new Panel();
            //        //        pnlImageInner.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            //        //        if (System.IO.Directory.Exists(Server.MapPath(RatingIconPath)))
            //        //        {

            //        //            if (System.IO.File.Exists(Server.MapPath(imagePath)))
            //        //            {

            //        //                Image ImgReview = new Image();
            //        //                ImgReview.ID = "Img_" + hdn.Value + "_" + Convert.ToString(i);
            //        //                ImgReview.ImageUrl = imagePath;
            //        //                ImgReview.Style.Add("float", "left");
            //        //                ImgReview.Style.Add("width", "99%");
            //        //                Button btnDelete = new Button();
            //        //                btnDelete.Text = "Delete";
            //        //                btnDelete.CommandArgument = Convert.ToString(RatingImageID);
            //        //                btnDelete.CommandName = "Delete";
            //        //                btnDelete.CssClass = "btndelete";
            //        //                btnDelete.OnClientClick = "return DeleteConfirmation('" + RatingImageID + "','" + RatingImageName + "');"; //RatingImageName
            //        //                //btnDelete.Click += new System.EventHandler(btnDelete_Click);

            //        //                pnlImageInner.ID = "innerpnl_" + RatingImageID;
            //        //                pnlImageInner.CssClass = "innerpnl";
            //        //                pnlImageInner.Controls.Add(ImgReview);
            //        //                pnlImageInner.Controls.Add(btnDelete);

            //        //                pnlImage.Controls.Add(pnlImageInner);
            //        //                pnlImage.Style.Add("display", "none");
            //        //                e.Row.Cells[8].Controls.Add(pnlImage);

            //        //                lnkviewImage.Visible = true;
            //        //                lnkviewImage.EnableViewState = false;
            //        //                lnkviewImage.OnClientClick = "return ShowImageModel('" + pnlImage.ID + "')";
            //        //            }
            //        //            else
            //        //            {

            //        //                //imagePath = string.Concat(RatingIconPath, Convert.ToString("image_not_available.jpg"));

            //        //                //Image ImgReview = new Image();
            //        //                //ImgReview.ID = "Img_" + hdn.Value + "_" + Convert.ToString(i);
            //        //                //ImgReview.ImageUrl = imagePath;
            //        //                //ImgReview.Style.Add("float", "left");
            //        //                //ImgReview.Style.Add("width", "100px");
            //        //                //Button btnDelete = new Button();
            //        //                //btnDelete.Text = "Delete";
            //        //                //btnDelete.CommandArgument = Convert.ToString(RatingImageID);
            //        //                //btnDelete.CommandName = "Delete";
            //        //                //btnDelete.CssClass = "btndelete";
            //        //                //btnDelete.OnClientClick = "return DeleteConfirmation('" + RatingImageID + "','" + RatingImageName + "');"; //RatingImageName
            //        //                //// btnDelete.Click += new System.EventHandler(btnDelete_Click);

            //        //                //pnlImageInner.ID = "innerpnl_" + RatingImageID;
            //        //                //pnlImageInner.CssClass = "innerpnl";
            //        //                //pnlImageInner.Controls.Add(ImgReview);
            //        //                //pnlImageInner.Controls.Add(btnDelete);

            //        //                //pnlImage.Controls.Add(pnlImageInner);
            //        //                //pnlImage.Style.Add("display", "none");
            //        //                //e.Row.Cells[8].Controls.Add(pnlImage);

            //        //            }
            //        //        }
            //        //    }
            //        //    //lnkviewImage.Visible = true;
            //        //    //lnkviewImage.EnableViewState = false;
            //        //    //lnkviewImage.OnClientClick = "return ShowImageModel('" + pnlImage.ID + "')";
            //        //}

            //}
            //}
        }

        /// <summary>
        /// Grid view Sorting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.AlternateText == "Ascending Order")
                {
                    BindGridAscDesc("ASC", lb.CommandName.ToString());
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = false;
                    }
                    else if (lb.ID == "lbDate")
                    {
                        isDescendDate = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.AlternateText == "Descending Order")
                {
                    BindGridAscDesc("DESC", lb.CommandName.ToString());
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "lbName")
                    {
                        isDescendName = true;
                    }
                    else if (lb.ID == "lbDate")
                    {
                        isDescendDate = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Product Review Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdQA_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        ///  Approve Rating Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnapproverating_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = grdQA.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdQA.Rows[i].FindControl("hdnQARId");
                HiddenField hdnQAR = (HiddenField)grdQA.Rows[i].FindControl("hdnQARValue");
                CheckBox chk = (CheckBox)grdQA.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    int QARId = Convert.ToInt32(hdn.Value);
                    if (hdnQAR.Value == "Question")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAQuestion_MVC SET IsApproved = 1 WHERE QuestionId =" + QARId + "");
                    }
                    else if (hdnQAR.Value == "Answer")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAAnswer_MVC SET IsApproved = 1 WHERE AnswerId =" + QARId + "");
                    }
                    else if (hdnQAR.Value == "Reply")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAReply_MVC SET IsApproved = 1 WHERE ReplyId =" + QARId + "");
                    }
                }
            }
            BindGrid();

            if (ddlQAstatus.SelectedValue == "1")
            {

                // grdProductReview.Columns[0].Visible = false;
                grdQA.Columns[6].Visible = false;
                trBottom.Visible = false;
            }
            else
            {
                // grdProductReview.Columns[0].Visible = true;
                grdQA.Columns[6].Visible = true;
                if (grdQA.Rows.Count > 0)
                    trBottom.Visible = true;
                else
                    trBottom.Visible = false;
            }
            // grdProductReview.DataBind();
            if (grdQA.Rows.Count > 0)
            {
                if (ddlQAstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            btnhdnDelete.CommandName = hdnCommandName.Value;
            string strComboValue = hdnCommandNameQAR.Value.Substring(hdnCommandNameQAR.Value.LastIndexOf(",") + 1);


            if (btnhdnDelete.CommandName == "Approve")
            {
                int QARId = Convert.ToInt32(hdnDelete.Value.Substring(0, hdnDelete.Value.LastIndexOf(",")));
                if (strComboValue == "Question")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAQuestion_MVC SET IsApproved = 1 WHERE QuestionId =" + QARId + "");
                    BindGrid();
                }
                else if (strComboValue == "Answer")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAAnswer_MVC SET IsApproved = 1 WHERE AnswerId =" + QARId + "");
                    BindGrid();
                }
                else if (strComboValue == "Reply")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAReply_MVC SET IsApproved = 1 WHERE ReplyId =" + QARId + "");
                    BindGrid();
                }
            }
            if (btnhdnDelete.CommandName == "Disapprove")
            {
                int QARId = Convert.ToInt32(hdnDelete.Value.Substring(0, hdnDelete.Value.LastIndexOf(",")));
                if (strComboValue == "Question")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAQuestion_MVC SET IsApproved = 2 WHERE QuestionId =" + QARId + "");
                    BindGrid();
                }
                else if (strComboValue == "Answer")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAAnswer_MVC SET IsApproved = 2 WHERE AnswerId =" + QARId + "");
                    BindGrid();
                }
                else if (strComboValue == "Reply")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_QAReply_MVC SET IsApproved = 2 WHERE ReplyId =" + QARId + "");
                    BindGrid();
                }
            }
        }

        protected void btnDisapproverating_Click(object sender, ImageClickEventArgs e)
        {
            int totalRowCount = grdQA.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdQA.Rows[i].FindControl("hdnQARId");
                HiddenField hdnQAR = (HiddenField)grdQA.Rows[i].FindControl("hdnQARValue");
                CheckBox chk = (CheckBox)grdQA.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                    int QARId = Convert.ToInt32(hdn.Value);
                    if (hdnQAR.Value == "Question")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAQuestion_MVC SET IsApproved = 2 WHERE QuestionId =" + QARId + "");
                    }
                    else if (hdnQAR.Value == "Answer")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAAnswer_MVC SET IsApproved = 2 WHERE AnswerId =" + QARId + "");
                    }
                    else if (hdnQAR.Value == "Reply")
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_QAReply_MVC SET IsApproved = 2 WHERE ReplyId =" + QARId + "");
                    }
                }
            }
            BindGrid();

            if (ddlQAstatus.SelectedValue == "1")
            {
                // grdProductReview.Columns[0].Visible = false;
                grdQA.Columns[6].Visible = false;
                trBottom.Visible = false;
            }
            else
            {
                //grdProductReview.Columns[0].Visible = true;
                grdQA.Columns[6].Visible = true;
                if (grdQA.Rows.Count > 0)
                    trBottom.Visible = true;
                else
                    trBottom.Visible = false;
            }
            //grdProductReview.DataBind();
            if (grdQA.Rows.Count > 0)
            {
                if (ddlQAstatus.SelectedValue.ToString() == "0")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "1")
                {
                    btnapproverating.Visible = false;
                    btnDisapproverating.Visible = true;
                }
                else if (ddlQAstatus.SelectedValue.ToString() == "-1")
                {
                    btnapproverating.Visible = true;
                    btnDisapproverating.Visible = false;

                }
                trBottom.Visible = true;
            }
        }

    }
}