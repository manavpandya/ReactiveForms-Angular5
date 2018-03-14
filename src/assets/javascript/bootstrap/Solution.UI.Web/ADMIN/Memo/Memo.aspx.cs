using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Memo
{
    public partial class Memo : Solution.UI.Web.BasePage
    {
        #region Variable declaration

        tb_Memo memo = null;
        MemoComponent objMemoComp = null;

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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                lblStartDate.Text = System.DateTime.Now.ToShortDateString();
                BindUsers();
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            memo = new tb_Memo();
            objMemoComp = new MemoComponent();
            memo.Title = txtTitle.Text.Trim();
            memo.Status = "Open";
            memo.StartDate = System.DateTime.Now;
            memo.AssignedTo = Convert.ToInt32(ddlAssignTo.SelectedValue);
            memo.CreatedBy = Convert.ToInt32(Session["AdminID"].ToString());
            memo.CreatedOn = System.DateTime.Now;
            memo.Deleted = false;
            Int32 isadded = objMemoComp.CreateMemo(memo);

            if (isadded > 0)
            {
                tb_MemoReply memoRep = new tb_MemoReply();
                memoRep.MemoID = isadded;
                memoRep.Description = txtDescription.Text.Trim();
                memoRep.UserID = Convert.ToInt32(Session["AdminID"].ToString());
                memoRep.Type = "Creator";
                memoRep.CreatedOn = System.DateTime.Now;
                isadded = objMemoComp.CreateMemoReply(memoRep);

                if (isadded > 0)
                {
                    string filesavepath = string.Empty;
                    filesavepath = AppLogic.AppConfigs("MemoAttachmentPath").ToString();
                    string fulldocpath = Server.MapPath(filesavepath);

                    if (uploadMemoDocument.HasFile)
                    {
                        if (!System.IO.Directory.Exists(fulldocpath.Replace("~", "")))
                            System.IO.Directory.CreateDirectory(fulldocpath.Replace("~", ""));

                        System.IO.FileInfo fl = new System.IO.FileInfo(uploadMemoDocument.FileName.ToString());
                        uploadMemoDocument.SaveAs(fulldocpath + isadded.ToString() + fl.Extension.ToString());
                        CommonComponent.ExecuteCommonData("UPDATE tb_MemoReply SET FileName='" + isadded.ToString() + fl.Extension.ToString() + "' WHERE ReplyID=" + isadded.ToString());
                    }

                    String MailTo = "", MailSubject = "", MailBody = "";
                    MailTo = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(EmailID,'') from tb_admin where AdminID=" + Convert.ToInt32(ddlAssignTo.SelectedValue)));
                    MailSubject = "New Memo Added - " + txtTitle.Text.Trim();
                    MailBody = "<div><b>Memo Detail</b> <br/>" + txtDescription.Text.Trim() + "</div>";
                    CommonOperations.SendMail(MailTo.Trim(), MailSubject, MailBody, Request.UserHostAddress.ToString(), true, null);
                    Response.Redirect("MemoView.aspx");
                }
            }
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MemoView.aspx");
        }

        /// <summary>
        /// Bind Users
        /// </summary>
        private void BindUsers()
        {
            DataSet dsUsers = new DataSet();
            dsUsers = CommonComponent.GetCommonDataSet("Select AdminID,(FirstName+' '+LastName +' : '+ EmailID) AS Name From tb_admin where isnull(active,0)=1 and isnull(Deleted,0)=0 order by FirstName, LastName");

            if (dsUsers != null && dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
            {
                ddlAssignTo.DataSource = dsUsers;
                ddlAssignTo.DataTextField = "Name";
                ddlAssignTo.DataValueField = "AdminID";
                ddlAssignTo.DataBind();

            }
            ddlAssignTo.Items.Insert(0, new ListItem("Select Assign To", "0"));
            ddlAssignTo.SelectedIndex = 0;
        }

    }
}