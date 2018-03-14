using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Memo
{
    public partial class MemoViewDetails : Solution.UI.Web.BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
            {
                Response.Redirect("/Admin/Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["MemoID"] != null)
                {

                    btnClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/close-v2.png";
                    btnSenmessage.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/submit.gif";
                    btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

                    FillMemodetails(Convert.ToInt32(Request.QueryString["MemoID"]));
                    FillMemodetailsReply(Convert.ToInt32(Request.QueryString["MemoID"]));
                }
            }
        }

        private void FillMemodetails(int MemoID)
        {
            string userID = "";
            MemoComponent objMemo = new MemoComponent();

            Object CreatedBy = CommonComponent.GetScalarCommonData("SELECT CreatedBy FROM tb_memo WHERE MemoID=" + MemoID + "");
            try
            {
                userID = Convert.ToString(CreatedBy);
            }
            catch { userID = "0"; }

            if (userID == Session["AdminID"].ToString())
            {
                object Email = CommonComponent.GetScalarCommonData("SELECT EmailID  FROM dbo.tb_Memo INNER JOIN  dbo.tb_Admin ON dbo.tb_Memo.AssignedTo=tb_Admin.AdminID WHERE CreatedBy=" + Session["AdminID"].ToString() + " AND MemoID=" + MemoID + "");
                btnClose.Visible = true;
                ltEmailTo.Text = Convert.ToString(Email);
            }
            else
            {
                object Email = CommonComponent.GetScalarCommonData("SELECT EmailID  FROM dbo.tb_Memo INNER JOIN  dbo.tb_Admin ON dbo.tb_Memo.CreatedBy=tb_Admin.AdminID WHERE AssignedTo=" + Session["AdminID"].ToString() + " AND MemoID=" + MemoID + "");
                ltEmailTo.Text = Convert.ToString(Email);
                btnClose.Visible = false;
            }

            DataSet dsMemo = new DataSet();
            dsMemo = objMemo.GetMemoByID(MemoID);
            if (dsMemo != null && dsMemo.Tables.Count > 0 && dsMemo.Tables[0].Rows.Count > 0)
            {
                lblSubject.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["Title"]);
                lblStatus.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["Status"]);
                if (lblStatus.Text.ToLower() == "close")
                {
                    btnClose.Visible = false;
                }
                lblCreateOn.Text = Convert.ToString(dsMemo.Tables[0].Rows[0]["CreatedOn"]);
            }
        }

        private void FillMemodetailsReply(int MemoID)
        {
            String Sql = null;
            DataSet dsMemo = new DataSet();
            String strreply = String.Empty;
            MemoComponent objMemo = new MemoComponent();
            dsMemo = objMemo.GetMemoReply(MemoID);
            if (dsMemo != null && dsMemo.Tables.Count > 0 && dsMemo.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsMemo.Tables[0].Rows.Count - 1; i++)
                {
                    #region Comment"
                    // dsMemo.Tables[0].Rows[i]["ReplyMadeBy"]
                    // dsMemo.Tables[0].Rows[i]["Description"]
                    // dsMemo.Tables[0].Rows[i]["CreatedOn"]
                    #endregion


                    if (Convert.ToString(dsMemo.Tables[0].Rows[i]["Type"]).ToLower() == "creator")
                    {
                        strreply += "<h3 class='clientmade'><a href=\"#\">" + " Reply made on : " + Convert.ToString(dsMemo.Tables[0].Rows[i]["CreatedOn"]) + " by  " + Convert.ToString(dsMemo.Tables[0].Rows[i]["ReplyMadeBy"]) + "</a> </h3>";
                    }
                    else
                        strreply += "<h3 class='adminmade'><a href=\"#\">" + " Reply made on : " + Convert.ToString(dsMemo.Tables[0].Rows[i]["CreatedOn"]) + " by  " + Convert.ToString(dsMemo.Tables[0].Rows[i]["ReplyMadeBy"]) + "</a> </h3>";


                    strreply += "<div>";
                    strreply += "<table cellspacing='0' cellpadding='0' border='0' class='order-table' style='font-family:Arial,sans-serif;font-size:12px;' width='100%' >";
                    strreply += "<tr>";
                    strreply += "<td>";
                    strreply += dsMemo.Tables[0].Rows[i]["Description"];
                    strreply += "</td>";
                    strreply += "</tr>";

                    if (dsMemo.Tables[0].Rows[i]["FileName"] != DBNull.Value && dsMemo.Tables[0].Rows[i]["FileName"].ToString() != "")
                    {
                        string filesavepath = string.Empty;
                        filesavepath = AppLogic.AppConfigs("MemoAttachmentPath").ToString();

                        if (System.IO.Directory.Exists(Server.MapPath(filesavepath.Replace("~", ""))))
                        {
                            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                            string flname = dsMemo.Tables[0].Rows[i]["FileName"].ToString();
                            if (System.IO.File.Exists(Server.MapPath(filesavepath.Replace("~", "")) + "/" + flname.ToString()))
                            {
                                strreply += "<tr>";
                                strreply += "<td>";
                                strreply += "<b><br /><br />Attachment:</b> <br />";
                                strreply += " <a href=\"" + url.ToString() + filesavepath.Replace("~", "") + "/" + flname + "\" target=\"_blank\">" + flname.ToString() + "</a><br />";

                                strreply += "</td>";
                                strreply += "</tr>";
                            }
                        }
                    }
                    strreply += "</table>";
                    strreply += "</div>";
                }
            }
            ltrReply.Text = strreply;
        }

        protected void imgsendmessage_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["MemoID"] != null)
            {
                Int32 isadded = 0;
                string userID = "";
                string Usertype = "";
                string Status = "";
                Object CreatedBy = CommonComponent.GetScalarCommonData("SELECT CreatedBy FROM tb_memo WHERE MemoID=" + Convert.ToInt32(Request.QueryString["MemoID"]) + "");
                try
                {
                    userID = Convert.ToString(CreatedBy);
                }
                catch { userID = "0"; }
                if (userID == Session["AdminID"].ToString())
                {
                    Usertype = "Creator";
                }
                else
                    Usertype = "Receiver";

                MemoComponent objMemoComp = new MemoComponent();
                tb_MemoReply memoRep = new tb_MemoReply();
                memoRep.MemoID = Convert.ToInt32(Request.QueryString["MemoID"]);
                memoRep.Description = txtDescription.Text.Trim();
                memoRep.UserID = Convert.ToInt32(Session["AdminID"].ToString());
                memoRep.Type = Usertype;
                memoRep.CreatedOn = System.DateTime.Now;
                isadded = objMemoComp.CreateMemoReply(memoRep);

                if (isadded > 0)
                {
                    if (Usertype.ToLower() == "receiver")
                    {
                        Status = "Waiting";

                    }
                    else
                        Status = "Open";

                    CommonComponent.ExecuteCommonData(" UPDATE dbo.tb_Memo SET Status='" + Status + "' WHERE MemoID=" + Convert.ToInt32(Request.QueryString["MemoID"]) + "");

                    String MailSubject = "Memo Updated - " + lblSubject.Text.Trim();
                    String MailBody = "<div><b>Memo Detail</b> <br/>" + txtDescription.Text.Trim() + "</div>";
                    CommonOperations.SendMail(ltEmailTo.Text, MailSubject, MailBody, Request.UserHostAddress.ToString(), true, null);

                    string filesavepath = string.Empty;
                    filesavepath = AppLogic.AppConfigs("MemoAttachmentPath").ToString();
                    string fulldocpath = Server.MapPath(filesavepath);

                    if (flUpload.HasFile)
                    {
                        if (!System.IO.Directory.Exists(fulldocpath.Replace("~", "")))
                            System.IO.Directory.CreateDirectory(fulldocpath.Replace("~", ""));

                        System.IO.FileInfo fl = new System.IO.FileInfo(flUpload.FileName.ToString());
                        flUpload.SaveAs(fulldocpath + isadded.ToString() + fl.Extension.ToString());
                        CommonComponent.ExecuteCommonData("UPDATE tb_MemoReply SET FileName='" + isadded.ToString() + fl.Extension.ToString() + "' WHERE ReplyID=" + isadded.ToString());
                    }
                    if (Request.QueryString["MemoID"] != null)
                    {
                        FillMemodetails(Convert.ToInt32(Request.QueryString["MemoID"]));
                        FillMemodetailsReply(Convert.ToInt32(Request.QueryString["MemoID"]));

                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MemoView.aspx");
        }

        protected void btnClose_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData(" UPDATE dbo.tb_Memo SET Status='Close' WHERE MemoID=" + Convert.ToInt32(Request.QueryString["MemoID"]) + "");
            btnClose.Visible = false;

            String MailSubject = "Memo Closed - " + lblSubject.Text.Trim();
            String MailBody = "<div><b>Memo Closed </b> <br/>" + txtDescription.Text.Trim() + "</div>";
            CommonOperations.SendMail(ltEmailTo.Text, MailSubject, MailBody, Request.UserHostAddress.ToString(), true, null);

            if (Request.QueryString["MemoID"] != null)
            {
                FillMemodetails(Convert.ToInt32(Request.QueryString["MemoID"]));
                FillMemodetailsReply(Convert.ToInt32(Request.QueryString["MemoID"]));
            }
        }

    }

}