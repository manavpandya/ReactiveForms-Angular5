using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class ListMaillog : BasePage
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
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                
                BindStore();
                GetEmail();
            }
        }
        protected void btnresend_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsnew = new DataSet();
            Int32 checkmail = 0;
            foreach (GridViewRow gr in grvMailReport.Rows)
            {
                Label lblMailId = (Label)gr.FindControl("lblmailID");
                CheckBox chkRecord = (CheckBox)gr.FindControl("chkSelect");
                if (chkRecord.Checked)
                {
                    try
                    {


                        dsnew = CommonComponent.GetCommonDataSet("SELECT * FROM tb_MailLog WHERE MailID=" + lblMailId.Text.ToString() + "");
                        if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        {

                            string strremove = "";
                            try
                            {
                                if (dsnew.Tables[0].Rows[0]["body"].ToString().ToLower().IndexOf("problem in sending mail") > -1)
                                {
                                    strremove = dsnew.Tables[0].Rows[0]["body"].ToString().Replace(dsnew.Tables[0].Rows[0]["body"].ToString().Substring(dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Problem in sending mail"), dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Message Body:") - dsnew.Tables[0].Rows[0]["body"].ToString().IndexOf("Problem in sending mail") + 13), "");
                                }
                                else
                                {
                                    strremove = dsnew.Tables[0].Rows[0]["body"].ToString();
                                }
                            }
                            catch
                            {
                                strremove = dsnew.Tables[0].Rows[0]["body"].ToString();
                            }

                            AlternateView av = AlternateView.CreateAlternateViewFromString(strremove, null, "text/html");
                            Solution.Bussines.Components.Common.CommonOperations.SendMail(dsnew.Tables[0].Rows[0]["ToEmail"].ToString(), dsnew.Tables[0].Rows[0]["Subject"].ToString().Replace("Failure :", ""), strremove.ToString(), dsnew.Tables[0].Rows[0]["IPAddress"].ToString(), true, av);

                        }
                        checkmail++;
                    }
                    catch
                    {

                    }
                }

                
            }
            if (checkmail > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsuccess", "alert('Mail has been sent successfully.');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsuccess", "alert('Mail Sending Problem.');", true);
            }
            
            
            //Problem in sending mail
        }
        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            GetEmail();
        }

        /// <summary>
        ///  Mail Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvMailReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMailReport.PageIndex = e.NewPageIndex;
            GetEmail();
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Get Email By Store
        /// </summary>
        private void GetEmail()
        {
            string strSql = "SELECT tb_MailLog.*,tb_Store.Storename FROM tb_MailLog INNER JOIN tb_Store ON tb_Store.StoreID = tb_MailLog.StoreID WHERE ISNULL(tb_MailLog.Deleted,0) = 0 ";
            if (ddlStore.SelectedIndex > 0)
            {
                strSql += " AND tb_MailLog.Storeid=" + ddlStore.SelectedValue.ToString() + "";
            }
            if (ddlSearch.SelectedIndex > 0 && txtSearch.Text.ToString().ToLower() != "search keyword" && txtSearch.Text.ToString().ToLower() != "searchkeyword")
            {
                strSql += " AND " + ddlSearch.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }

            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

            }


            if (txtMailFrom.Text.ToString() != "")
            {
                strSql += " AND Cast(MailDate as date) >= Cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            if (txtMailTo.Text.ToString() != "")
            {

                strSql += " AND Cast(MailDate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            strSql = strSql + " order by tb_MailLog.MailDate DESC";
            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql);
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grvMailReport.DataSource = dsMail;
                grvMailReport.DataBind();
                trdelete.Visible = true;
                btnresend.Visible = true;
            }
            else
            {
                trdelete.Visible = false;
                grvMailReport.DataSource = null;
                grvMailReport.DataBind();
                btnresend.Visible = false;
            }



        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grvMailReport.PageIndex = 0;
            GetEmail();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grvMailReport.PageIndex = 0;
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "Search Keyword";
            GetEmail();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grvMailReport.Rows)
            {
                Label lblMailId = (Label)gr.FindControl("lblmailID");
                CheckBox chkRecord = (CheckBox)gr.FindControl("chkSelect");
                if (chkRecord.Checked)
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_MailLog SET Deleted=1 WHERE MailID=" + lblMailId.Text.ToString() + "");
                }
            }
            grvMailReport.PageIndex = 0;
            GetEmail();
        }
    }
}