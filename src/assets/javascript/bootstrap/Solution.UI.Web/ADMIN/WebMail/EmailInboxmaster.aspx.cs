using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.WebMail
{
    public partial class EmailInboxmaster : BasePage
    {
        #region Common Variables

        public static bool isDescendFrom = false;
        public static bool isDescendSubject = false;
        public static bool isDescendDate = false;
        public static string EmailAttachmentPath = string.Empty;
        static int pageNo = 1;
        static DataView dsGlobal = null;
        string storeID = "0";
        String ShowType = String.Empty;
        String IDs = String.Empty;
        String Types = String.Empty;
        int pageSize = 20;
        const int pageDispCount = 10;
        private DataTable dt = new DataTable();

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            pageSize = Convert.ToInt32(ddlpageNo.SelectedValue);
            gvMailLog.AllowPaging = true;
            gvMailLog.PageSize = Convert.ToInt32(ddlpageNo.SelectedValue);

            if (Request.QueryString["ShowType"] != null && Convert.ToString(Request.QueryString["ShowType"]) != "")
            {
                ShowType = Convert.ToString(Request.QueryString["ShowType"]);
            }
            if (Request.QueryString["ID"] != null && Convert.ToString(Request.QueryString["ID"]) != "")
            {
                IDs = Convert.ToString(Request.QueryString["ID"]);
            }
            if (Request.QueryString["Type"] != null && Convert.ToString(Request.QueryString["Type"]) != "")
            {
                Types = Convert.ToString(Request.QueryString["Type"]);
            }
            if (ShowType.ToString().ToLower() == "inbox" || ShowType.ToString().ToLower() == "sent items" || ShowType.ToString().ToLower().IndexOf("recent email") > -1)
            {
                if (ShowType.ToString().ToLower() == "inbox")
                {
                    gvMailLog.Columns[3].Visible = true;
                    gvMailLog.Columns[4].Visible = false;
                }
                else
                {
                    gvMailLog.Columns[3].Visible = false;
                    gvMailLog.Columns[4].Visible = true;
                }
                lblPageTitle.Text = ShowType;
            }
            else if (ShowType == "Compose")
            {
                trEmailList.Attributes.Add("style", "display:none");
                trReFwd.Attributes.Add("style", "display:''");
                trEmailDetails.Attributes.Add("style", "display:none");
                lblPageTitle.Text = "New Mail";
                btndiscard.Visible = false;
                btnCancel.Visible = true;
                sparflinks.Visible = false;
                divpageitem.Visible = false;
                trforwardedAttachments.Attributes.Add("style", "display:none");
                if (Request.QueryString["OrderId"] != null)
                {
                    btnCancel.OnClientClick = "javascript:window.close(); return false;";
                }
                else
                {
                    btnCancel.OnClientClick = "javascript:window.history.back(); return false;";
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "HideMoveTo", "if(window.parent.document.getElementById('tdMoveto')){window.parent.document.getElementById('tdMoveto').style.display='none';}", true);
            }
            else if (ShowType == "ShowBody")
            {
                // Mode =1
                //String IName = Convert.ToString(objsql.ExecuteScalerQuery("select (Case When (FolderName Is Not Null And FolderName!='') Then FolderName Else FolderEmail End) As FolderName from tb_Ecomm_EmailFolders where FolderID=(Select FolderID from tb_Ecomm_EmailList where MailID=" + IDs + ")"));
                String IName = WebMailComponent.GetFolderNameForShowBody(Convert.ToInt32(IDs));
                int qmailid = 0;
                try
                {
                    int.TryParse(Convert.ToString(Request.QueryString["ID"]), out qmailid);
                    hdmMailIdss.Value = qmailid.ToString();
                }
                catch { }
                if (IName != "")
                {
                    lblPageTitle.Text = IName;
                }
                else
                {
                    lblPageTitle.Text = "Unknown";
                }
            }
            else if (ShowType.ToString().ToLower().IndexOf("deleted items") > -1)
            {
                lblPageTitle.Text = "Deleted Items";
            }
            else if (ShowType.ToString().ToLower().IndexOf("spam") > -1)
            {
                lblPageTitle.Text = "Spam";
            }
            else if (Types == "btnGo")
            {
                lblPageTitle.Text = "Search Result For : " + Convert.ToString(Request.QueryString["SearchValue"]) + "";
            }
            else
            {
                // Mode=2
                //String IName = Convert.ToString(objsql.ExecuteScalerQuery("Select (Case When (FolderName Is Not Null And FolderName!='') Then FolderName Else FolderEmail End) As FolderName from tb_Ecomm_EmailFolders where FolderID=" + Convert.ToInt32(ShowType) + ""));
                String IName = WebMailComponent.GetFolderNameForOthers(Convert.ToInt32(ShowType));
                lblPageTitle.Text = IName;
            }


            if (!IsPostBack)
            {
                EmailAttachmentPath = AppLogic.AppConfigs("WebMailAttachmentPath");
                isDescendFrom = false;
                isDescendSubject = false;
                isDescendDate = false;


                if (Request.QueryString["Email"] != null)
                {
                    txtrefwdto.Text = Request.QueryString["Email"].ToString();
                }
                ddlpageNo.SelectedValue = pageSize.ToString();
                //gvMailLog.PageSize = AdminEcomm.Admin.DataBase.Common.clsPageSize.PageSize;

                btnDelete.Attributes.Add("OnClick", "javascript:return chkselect();");
                Button6.Attributes.Add("OnClick", "javascript:return chkselect();");
                ViewState["Filter"] = null;

                // BindStore();
                if (ShowType != "Compose" && Types != "btnGo")
                {
                    BindData();
                }
                if (Types != "" && Types == "MoveTo")
                {
                    Movetodata(IDs);
                }
                else if (Types != "" && Types == "btnGo")
                {
                    if (Request.QueryString["SearchField"] != null && Convert.ToString(Request.QueryString["SearchField"]) != "")
                    {
                        GoButtonResult(Convert.ToString(Request.QueryString["SearchField"]), Convert.ToString(Request.QueryString["SearchValue"]));
                    }
                }
            }
            else
            {
                String[] formkeys = Request.Form.AllKeys;
                foreach (String s in formkeys)
                {
                    //For Delete Product from Cart and Bind Cart again
                    if (s.Contains("btnDownload-") || s.Contains("btnDownload-mainattach-"))
                    {
                        try
                        {
                            String p = Request.Form.Get(s);
                            downloadfile(p);
                        }
                        catch (Exception ex)
                        {
                            //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :-> cart() - bt_Delete  \r\n Date: " + System.DateTime.Now + "\r\n");
                        }
                    }
                }
            }
        }


        #region DownloadFile

        /// <summary>
        /// Download File from file path
        /// </summary>
        /// <param name="filepath">string filepath</param>
        private void downloadfile(string filepath)
        {
            try
            {

                FileInfo file = new FileInfo(filepath);
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = ReturnExtension(file.Extension.ToLower());
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    FileStream sourceFile = new FileStream(filepath, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);

                }
            }
            catch { }
            Response.End();
        }

        /// <summary>
        /// Returns the Extension from File
        /// </summary>
        /// <param name="fileExtension">string fileExtension</param>
        /// <returns>Returns Extension as a String</returns>
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }
        #endregion

        #region Get Searching,MoveTo,Go and Show All Results

        /// <summary>
        /// Search Button Click event
        /// </summary>
        /// <param name="SearchField">String SearchField</param>
        /// <param name="SearchValue">String SearchValue</param>
        public void GoButtonResult(String SearchField, String SearchValue)
        {
            if (SearchField.Length != 0 && SearchValue.Length != 0)
            {
                string strEmailid = "";
                string emailnames = "";
                if (SearchField.ToString().ToLower() == "zipcode")
                {

                    //emailnames = Convert.ToString(objSqlNew.ExecuteScalerQuery("SELECT ''''+ Email +''',' FROM tb_ecomm_order WHERE ShippingZip like '%" + SearchValue.ToString() + "%' OR BillingZip like '%" + SearchValue.ToString() + "%' FOR XML PATH('')"));
                    emailnames = WebMailComponent.GetEmailIDsFromZipCode(SearchValue.ToString().Trim());
                    if (emailnames.IndexOf(",") > -1)
                    {
                        emailnames = emailnames.Substring(0, emailnames.Length - 1);
                    }
                }

                string str = string.Empty;
                if (Request.QueryString["deletd"] != null && Request.QueryString["deletd"].ToString().ToLower() == "1")
                {
                    str = "Select ROW_NUMBER() OVER (ORDER BY MailID ASC) AS id, MailID,[From],[To],Subject,Body,SentOn,FolderID,IsAttachment,IsIncomming,AttachmentName,Createdon,isnull(cc,'') as cc,isnull(bcc,'') as bcc,isnull(isread,1) as isread ,isnull(isDeleted,0) as isDeleted from tb_EmailList where isnull(isDeleted,0)=1 AND ";
                }
                else if (Request.QueryString["spm"] != null && Request.QueryString["spm"].ToString().ToLower() == "1")
                {
                    str = "Select ROW_NUMBER() OVER (ORDER BY MailID ASC) AS id, MailID,[From],[To],Subject,Body,SentOn,FolderID,IsAttachment,IsIncomming,AttachmentName,Createdon,isnull(cc,'') as cc,isnull(bcc,'') as bcc,isnull(isread,1) as isread ,isnull(isDeleted,0) as isDeleted from tb_EmailList where isnull(isSpam,0)=1 AND ";
                }
                else
                {
                    str = "Select ROW_NUMBER() OVER (ORDER BY MailID ASC) AS id, MailID,[From],[To],Subject,Body,SentOn,FolderID,IsAttachment,IsIncomming,AttachmentName,Createdon,isnull(cc,'') as cc,isnull(bcc,'') as bcc,isnull(isread,1) as isread ,isnull(isDeleted,0) as isDeleted from tb_EmailList where isnull(isDeleted,0)=0 AND isnull(isSpam,0)=0 AND ";
                }
                if (SearchField == "OrderNumber")
                {
                    int io = 0;
                    try
                    {
                        io = Convert.ToInt32(SearchValue);
                    }
                    catch
                    {
                    }
                    //str = SearchField + " = '" + Convert.ToInt32(SearchValue.ToString()) + "'";
                    str += "(Subject like '%Receipt for Order #" + SearchValue.ToString() + "%' or Subject like '%New Order #" + SearchValue.ToString() + "%' or Subject like '%Distributor for Order No:" + SearchValue.ToString() + "%' or Subject like '%Receipt for Order No:" + SearchValue.ToString() + "%' or Subject like '%Order No:" + SearchValue.ToString() + "%'  or Subject like '%Cancelled - ONo: " + SearchValue.ToString() + "%')";
                    str += " or isnull(OrderNumber,0)=" + io.ToString() + " or (Body like '%>" + SearchValue.ToString() + "</td>%' or Body like '%&nbsp;&nbsp;&nbsp;&nbsp;Receipt For Order - <strong>#" + SearchValue.ToString() + "%' or Body like '%Order Number : 293%' or Body like '%Order Number : <b>" + SearchValue.ToString() + "</b>%' or Body like '%<span id=\"lblOrderId\">" + SearchValue.ToString() + "</span>%' or Body like '%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Receipt For Order - <strong>#" + SearchValue.ToString() + "%' or Body like '%Order Number:&nbsp; <b>" + SearchValue.ToString() + "</b>%' or Body like '%Order Number : <b>" + SearchValue.ToString() + "</b>%')";
                }
                else if (SearchField.ToLower() == "subject")
                {
                    //str = SearchField + " = '" + Convert.ToInt32(SearchValue.ToString()) + "'";
                    str += "(Subject like '%" + SearchValue.ToString().Replace("'", "''") + "%')";
                    // str += " or (Body like '%&nbsp;&nbsp;&nbsp;&nbsp;Receipt For Order - <strong>#" + SearchValue.ToString() + "%' or Body like '%Order Number : 293%' or Body like '%Order Number : <b>" + SearchValue.ToString() + "</b>%' or Body like '%<span id=\"lblOrderId\">" + SearchValue.ToString() + "</span>%' or Body like '%&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Receipt For Order - <strong>#" + SearchValue.ToString() + "%' or Body like '%Order Number:&nbsp; <b>" + SearchValue.ToString() + "</b>%' or Body like '%Order Number : <b>" + SearchValue.ToString() + "</b>%')";
                }
                else if (SearchField.ToLower() == "body")
                {
                    //str = SearchField + " = '" + Convert.ToInt32(SearchValue.ToString()) + "'";

                    str += "(Body like '%" + SearchValue.ToString().Replace("'", "''") + "%')";
                }

                else if (SearchField == "CustomerName")
                {
                    str += "([From] like '%" + Convert.ToString(SearchValue.ToString()) + "%' or [To] like '%" + Convert.ToString(SearchValue.ToString()) + "%' or Body like '%" + Convert.ToString(SearchValue.ToString()) + "%' or Subject like '%" + Convert.ToString(SearchValue.ToString()) + "%')";
                }
                else if (SearchField.ToLower() == "zipcode")
                {
                    str += "([From] in (" + Convert.ToString(emailnames.ToString()) + ") or [To] in (" + Convert.ToString(emailnames.ToString()) + "))";
                }
                else if (SearchField == "EmailID" || SearchField == "CustomerName")
                {
                    //str += "([From] like '%" + SearchValue.ToString() + "%' or [To] like '%" + SearchValue.ToString() + "%' or Subject like '%" + SearchValue.ToString() + "%' or Body like '%" + SearchValue.ToString() + "%')";
                    str += "([From] like '%" + SearchValue.ToString() + "%' or [To] like '%" + SearchValue.ToString() + "%')";
                }
                DataSet dssearchresults = new DataSet();
                dssearchresults = CommonComponent.GetCommonDataSet(str);

                if (dssearchresults != null && dssearchresults.Tables.Count > 0 && dssearchresults.Tables[0].Rows.Count > 0)
                {
                    gvMailLog.DataSource = dssearchresults.Tables[0];
                    gvMailLog.DataBind();
                    dt = dssearchresults.Tables[0];
                    managePaging(dssearchresults.Tables[0]);
                    Session["dataviewRecord"] = dssearchresults;

                    trBottom.Visible = true;
                    trTop.Visible = true;
                    divpageitem.Visible = true;
                }
                else
                {
                    gvMailLog.DataSource = null;
                    gvMailLog.DataBind();
                    Session["dataviewRecord"] = null;
                    lblMsg.Text = "No Records Found..." + "<br/><br/>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "msg", "if(window.parent.document.getElementById('tdMoveto')){window.parent.document.getElementById('tdMoveto').style.display='none';}", true);
                    trBottom.Visible = false;
                    trTop.Visible = false;
                    divpageitem.Visible = false;
                }
            }
        }

        /// <summary>
        /// Move Email Message function
        /// </summary>
        /// <param name="folID">String folID</param>
        public void Movetodata(String folID)
        {
            int cnt = 0;
            string strids = "0";
            String foldernames = "";
            //if (Request.QueryString["moveid"] != null)
            //{
            //    //foldernames = Convert.ToString(objsql.ExecuteScalerQuery("Select Case When isnull(FolderName,'')!='' Then FolderName Else FolderEmail End As FolderName FROM tb_Ecomm_EmailFolders where FolderID=" + Request.QueryString["moveid"] + ""));
            //    foldernames = WebMailComponent.GetFolderNameForMoveTo(Convert.ToInt32(Request.QueryString["moveid"]));
            //}
            //else
            //{
            //    //foldernames = Convert.ToString(objsql.ExecuteScalerQuery("Select Case When isnull(FolderName,'')!='' Then FolderName Else FolderEmail End As FolderName FROM tb_Ecomm_EmailFolders where FolderID=" + folID + ""));
            //    if (folID.ToString().Trim().ToLower().IndexOf("sent items") > -1 || folID.ToString().Trim().ToLower().IndexOf("deleted items") > -1)
            //    {
            //        foldernames = folID;
            //    }
            //    else
            //    {
            //        foldernames = WebMailComponent.GetFolderNameForMoveTo(Convert.ToInt32(folID));
            //    }
            //}
            if (Request.QueryString["ids"] != null)
            {
                strids = "," + Request.QueryString["ids"].ToString() + ",";
            }
            for (int i = 0; i < gvMailLog.Rows.Count; i++)
            {
                Label lblmailidg = (Label)gvMailLog.Rows[i].FindControl("lblMailID");

                if (strids.IndexOf("," + lblmailidg.Text.ToString() + ",") > -1)
                {
                    cnt++;
                    if (Request.QueryString["moveid"] != null)
                    {
                        if (folID != "" && folID.ToString().Trim().ToLower().IndexOf("sent items") > -1)
                        {
                            //bool ismoved = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList set FolderID=" + Request.QueryString["moveid"] + ",IsIncomming=0 where MailID=" + lblmailidg.Text + "");
                            int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(Request.QueryString["moveid"]), Convert.ToInt32(lblmailidg.Text), 1);
                        }
                        else if (folID != "" && folID.ToString().Trim().ToLower().IndexOf("deleted items") > -1)
                        {
                            int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(Request.QueryString["moveid"]), Convert.ToInt32(lblmailidg.Text), 4);
                        }
                        else if (folID != "" && folID.ToString().Trim().ToLower().IndexOf("spam") > -1)
                        {
                            int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(Request.QueryString["moveid"]), Convert.ToInt32(lblmailidg.Text), 5);
                        }
                        else
                        {
                            //int ismoved = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList set FolderID=" + Request.QueryString["moveid"] + " where MailID=" + lblmailidg.Text + "");
                            int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(Request.QueryString["moveid"]), Convert.ToInt32(lblmailidg.Text), 2);
                        }
                    }
                    //else
                    //{
                    //    if (foldernames != "" && (foldernames.ToString().Trim().ToLower().IndexOf("sent items") > -1 || foldernames.ToString().Trim().ToLower().IndexOf("deleted items") > -1))
                    //    {
                    //        //bool ismoved = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList set FolderID=" + Request.QueryString["moveid"] + ",IsIncomming=0 where MailID=" + lblmailidg.Text + "");
                    //        //bool ismoved = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList set FolderID=" + folID + ",IsIncomming=0 where MailID=" + lblmailidg.Text + "");
                    //        int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(folID), Convert.ToInt32(lblmailidg.Text), 1);
                    //    }
                    //    else
                    //    {
                    //        //bool ismoved = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList set FolderID=" + folID + " where MailID=" + lblmailidg.Text + "");
                    //        int ismoved = WebMailComponent.MoveMessage(Convert.ToInt32(folID), Convert.ToInt32(lblmailidg.Text), 2);
                    //    }
                    //}
                }
            }
            if (cnt > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Added", "$(document).ready( function() {jAlert('Record(s) has been moved to specified folder.', 'Message');});", true);
                BindData();
                return;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "MoveTo", "$(document).ready( function() {jAlert('Please select atleast one record.', 'Message');});", true);
                //ddlmoveto.SelectedValue = "0";
                return;
            }
        }

        /// <summary>
        /// Show all data
        /// </summary>
        public void ShowAlldata()
        {
            BindData();
        }
        #endregion


        /// <summary>
        /// Bing Gridview with the Mail List
        /// </summary>
        public void BindData()
        {
            DataSet dsMaillog = new DataSet();
            dsMaillog = WebMailComponent.GetEmailMessage(ShowType, Convert.ToString(IDs), Convert.ToInt32(Session["AdminID"]));
            if (dsMaillog != null && dsMaillog.Tables.Count > 0 && dsMaillog.Tables[0].Rows.Count > 0)
            {
                gvMailLog.DataSource = dsMaillog.Tables[0];
                gvMailLog.DataBind();
                Session["dataviewRecord"] = dsMaillog;
                dt = dsMaillog.Tables[0];
                managePaging(dsMaillog.Tables[0]);
                if (ShowType == "ShowBody")
                {
                    ShowBody(Convert.ToInt32(IDs));
                }
                trBottom.Visible = true;
                trTop.Visible = true;
                divpageitem.Visible = true;
            }
            else
            {
                gvMailLog.DataSource = null;
                gvMailLog.DataBind();
                lblMsg.Text = "No Records Found..." + "<br/><br/>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "msg", "if(window.parent.document.getElementById('tdMoveto')){window.parent.document.getElementById('tdMoveto').style.display='none';}", true);
                trBottom.Visible = false;
                trTop.Visible = false;
                divpageitem.Visible = false;
            }
            if (ShowType == "ShowBody")
            {
                divpageitem.Visible = false;
            }
            else
            {
                if (gvMailLog.Rows.Count > 0)
                {
                    divpageitem.Visible = true;
                }
                else
                {
                    divpageitem.Visible = false;
                }
            }
        }

        /// <summary>
        /// Message Gridview paging dropdown selected index change event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlpageNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowType != "Compose" && Types != "btnGo")
            {
                pageSize = Convert.ToInt32(ddlpageNo.SelectedValue.ToString());
                BindData();
            }
        }


        /// <summary>
        /// Delete button Click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        /// <summary>
        /// Delete Mail button Click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeletemail_Click(object sender, EventArgs e)
        {
            Deletemail();
        }

        /// <summary>
        /// Spam button Click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSpamemail_Click(object sender, EventArgs e)
        {
            Spammail();
        }

        /// <summary>
        /// Mark as Unread button Click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnMarkUnread_Click(object sender, EventArgs e)
        {
            MarkAsUnread();
        }


        #region Delete,DeleteMail,SpamMail,MarkAsUnreadMail Functions

        /// <summary>
        /// Delete all the MailLogs which are selected
        /// </summary>
        public void Delete()
        {
            int count = 0;
            foreach (GridViewRow row in gvMailLog.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                Label lb = (Label)row.FindControl("lblMailID");
                Label lblIsDeleted = (Label)row.FindControl("lblIsDeleted");
                if (chk.Checked)
                {
                    if (Convert.ToBoolean(lblIsDeleted.Text.ToString()))
                    {
                        //bool indx = objsql.ExecuteNonQuery("Delete From tb_Ecomm_EmailList where MailID=" + Convert.ToInt32(lb.Text.Trim()) + "");
                        bool indx = WebMailComponent.DeleteEmailMessage(Convert.ToInt32(lb.Text.Trim()), 11, Convert.ToInt32(Session["AdminID"]));
                    }
                    else
                    {
                        //bool indx = objsql.ExecuteNonQuery("UPDATE tb_Ecomm_EmailList SET isDeleted=1,IsRead=1 where MailID=" + Convert.ToInt32(lb.Text.Trim()) + "");
                        bool indx = WebMailComponent.DeleteEmailMessage(Convert.ToInt32(lb.Text.Trim()), 12, Convert.ToInt32(Session["AdminID"]));
                    }
                    count++;
                }
            }
            if (count > 0)
            {
                BindData();
            }
        }

        /// <summary>
        /// Delete mail function
        /// </summary>
        public void Deletemail()
        {
            int count = 0;
            //mode=13
            //bool indx = objsql.ExecuteNonQuery("Delete From tb_Ecomm_EmailList where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + "");
            //bool indx = objsql.ExecuteNonQuery("if not exists(select mailid FROM tb_Ecomm_EmailList where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + " and isnull(isDeleted,0) = 1) begin UPDATE tb_Ecomm_EmailList SET isDeleted=1,IsRead=1 where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + " end else begin DELETE FROM tb_Ecomm_EmailList WHERE MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + " end ");
            bool indx = WebMailComponent.Delete_Spam_MarkAsUnread(Convert.ToInt32(hdmMailIdss.Value.ToString()), 13, Convert.ToInt32(Session["AdminID"]));
            hdmMailIdss.Value = "0";
            if (Convert.ToString(Request.QueryString["ShowType"]) == "ShowBody")
            {
                //Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msgclose", "window.close();", true);
                //  Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msgclose", "if(window.opener.document.getElementById('lkbOrderEmail')){window.opener.document.getElementById('lkbOrderEmail').click();}window.close();", true);
                Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msgclose", "window.opener.location.href=window.opener.location.href; window.close();", true);
                //if(window.parent.document.getElementById('tdMoveto')){window.parent.document.getElementById('tdMoveto').style.display='none';}
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msg", "window.location.href = window.location.href;", true);
            }
        }


        /// <summary>
        /// Spam mail function
        /// </summary>
        public void Spammail()
        {
            int count = 0;
            //mode=14
            // bool indx = objsql.ExecuteNonQuery("Delete From tb_Ecomm_EmailList where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + "");
            // bool indx = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList Set IsSpam=1,IsRead=0 where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + "");
            bool indx = WebMailComponent.Delete_Spam_MarkAsUnread(Convert.ToInt32(hdmMailIdss.Value.ToString()), 14, Convert.ToInt32(Session["AdminID"]));
            hdmMailIdss.Value = "0";
            //Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msg", "window.location.href = window.location.href;", true);

            if (Convert.ToString(Request.QueryString["ShowType"]) == "ShowBody")
            {
                Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msgclose", "window.opener.location.href=window.opener.location.href; window.close();", true);
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msg", "window.location.href = window.location.href;", true);
            }


        }

        /// <summary>
        /// Mark as unread function
        /// </summary>
        public void MarkAsUnread()
        {
            int count = 0;
            //mode=15
            // bool indx = objsql.ExecuteNonQuery("Delete From tb_Ecomm_EmailList where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + "");
            //bool indx = objsql.ExecuteNonQuery("Update tb_Ecomm_EmailList Set IsRead=1 where MailID=" + Convert.ToInt32(hdmMailIdss.Value.ToString()) + "");
            bool indx = WebMailComponent.Delete_Spam_MarkAsUnread(Convert.ToInt32(hdmMailIdss.Value.ToString()), 15, Convert.ToInt32(Session["AdminID"]));
            hdmMailIdss.Value = "0";
            //Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msg", "window.location.href = window.location.href;", true);
            if (Convert.ToString(Request.QueryString["ShowType"]) == "ShowBody")
            {
                Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msgclose", "window.opener.location.href=window.opener.location.href; window.close();", true);
            }
            else
            {
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "if(window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnloadtreeagain')){window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnloadtreeagain').click();} window.location.href = window.location.href;", true);

                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "window.location.href = window.location.href;", true);
            }
        }
        #endregion


        /// <summary>
        /// Sorting Grid View Ascending or Descending
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    //gvMailLog.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    DataView dtgrid = new DataView();
                    dtgrid = dt.DefaultView;
                    dtgrid.Sort = lb.CommandName.ToString() + " " + lb.CommandArgument.ToString();
                    dt = dtgrid.ToTable();
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnFrom")
                    {
                        isDescendFrom = false;
                    }
                    else if (lb.ID == "btnSubject")
                    {
                        isDescendSubject = false;
                    }
                    else
                    {
                        isDescendDate = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    //gvMailLog.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    DataView dtgrid = new DataView();
                    dtgrid = dt.DefaultView;
                    dtgrid.Sort = lb.CommandName.ToString() + " " + lb.CommandArgument.ToString();
                    dt = dtgrid.ToTable();
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnFrom")
                    {
                        isDescendFrom = true;
                    }
                    else if (lb.ID == "btnSubject")
                    {
                        isDescendSubject = true;
                    }
                    else
                    {
                        isDescendDate = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
                gvMailLog.DataSource = dt;
                gvMailLog.DataBind();

                managePaging(dt);
            }
        }


        #region Grid View Events


        /// <summary>
        ///  Mail Log Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvMailLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMailLog.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Mail Log Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvMailLog_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowSubject")
            {
                int MailIDg = Convert.ToInt32(e.CommandArgument);
                if (Session["dataviewRecord"] != null)
                {
                    DataSet dsFilter = new DataSet();
                    dsFilter = (DataSet)Session["dataviewRecord"];
                    DataView dvfilter = dsFilter.Tables[0].DefaultView;

                    dvfilter.RowFilter = "MailID=" + MailIDg;
                    hdmMailIdss.Value = MailIDg.ToString();
                    dvfilter.ToTable();
                    if (dvfilter.Count > 0)
                    {
                        //System.Web.UI.WebControls.Literal lrtto = new System.Web.UI.WebControls.Literal();
                        //System.Web.UI.WebControls.Literal lrtfrom = new System.Web.UI.WebControls.Literal();
                        //lrtto.Text = dvfilter[0][3].ToString();
                        //lrtfrom.Text = dvfilter[0][2].ToString();

                        StringBuilder sbemailshow = new StringBuilder();
                        System.Web.UI.WebControls.Literal lrtshow = new System.Web.UI.WebControls.Literal();
                        StringBuilder sbShowAttachments = new StringBuilder();
                        StringBuilder sbForwardAttachment = new StringBuilder();
                        String strAttchpath = Server.MapPath(EmailAttachmentPath + "MailID_" + MailIDg.ToString());

                        if (Directory.Exists(strAttchpath))
                        {
                            String[] GetAllFileNames = Directory.GetFiles(strAttchpath);

                            for (int i = 0; i < GetAllFileNames.Length; i++)
                            {
                                FileInfo fi = new FileInfo(GetAllFileNames[i].ToString());

                                double FileSize = 0;
                                String FileSizePrint = "";
                                FileSize = fi.Length;
                                FileSizePrint = FileSize.ToString() + "byte";
                                if (FileSize > 1024)
                                {
                                    FileSize = Math.Round(FileSize / 1024, 2);
                                    FileSizePrint = FileSize.ToString() + "KB";
                                }
                                else if (FileSize > (1024 * 1204))
                                {
                                    FileSize = Math.Round(FileSize / (1024 * 1204), 2);
                                    FileSizePrint = FileSize.ToString() + "MB";
                                }

                                if (fi.Name != "Thumbs.db")
                                {
                                    if (i == 0)
                                    {
                                        sbShowAttachments.Append("<b>&nbsp;Attachments :</b><br/><br/>");
                                    }
                                    string strdwnloadtemp = EmailAttachmentPath + "MailID_" + MailIDg.ToString() + "/" + fi.Name.ToString();
                                    //sbShowAttachments.AppendLine((i + 1) + ". <a href=\"" + downloadfile("/AttachmentsFiles/" + "MailID_" + MailIDg.ToString() + "/" + fi.Name.ToString()) + "\" style='color: #696A6A;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none'>" + fi.Name.ToString() + "</a>&nbsp;&nbsp;&nbsp;");
                                    sbShowAttachments.AppendLine("<input style='display:none;' type='submit' runat='server' value='" + Server.MapPath(strdwnloadtemp).ToString().Replace("'", "''") + "' name='btnDownload-main-" + i + "'  id='btnDownload-main-" + i + "' onclick='submit' /> <a href='#' style='color: #212121;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none' onclick=\"document.getElementById('btnDownload-main-" + i + "').click();\">" + (i + 1) + ". " + fi.Name.ToString() + "(" + FileSizePrint.ToString() + ")" + "</a>&nbsp;&nbsp;&nbsp;");
                                    sbForwardAttachment.AppendLine("<input type='checkbox' id='chkfwdattach-" + i + "' checked='checked' onclick='Getbtnvalue();' onchange='Getbtnvalue();'/> <input style='display:none;' type='submit' runat='server' value='" + Server.MapPath(strdwnloadtemp).ToString().Replace("'", "''") + "' name='btnDownload-mainattach-" + i + "'  id='btnDownload-mainattach-" + i + "' onclick='submit' /> <a href='#' style='color: #212121;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none' onclick=\"document.getElementById('btnDownload-mainattach-" + i + "').click();\">" + (i + 1) + ". " + fi.Name.ToString() + "(" + FileSizePrint.ToString() + ")" + "</a>&nbsp;&nbsp;&nbsp;");
                                    //Links = Links + "<input style='display:none;' type='submit' runat='server' value='" + strdwnloadtemp + "' name='btnDownload-main-" + i + "'  id='btnDownload-main-" + i + "' onclick='submit' /> <a href='#' onclick=\"document.getElementById('btnDownload-main-" + i + "').click();\">" + fi.Name.ToString() + "</a>&nbsp;&nbsp;&nbsp;";
                                }
                            }
                        }
                        if (sbShowAttachments.ToString() == "")
                        {
                            sbShowAttachments.Append("No Attachment(s) Found.");
                        }
                        if (sbForwardAttachment.ToString() == "")
                        {
                            sbForwardAttachment.Append("No Attachment(s) Found.");
                        }
                        ltrFwdAttach.Text = sbForwardAttachment.ToString();

                        lrtshow.Text = Server.HtmlDecode(dvfilter[0][5].ToString());
                        sbemailshow.Append("<div id='getprint'><table cellpadding='5' cellspacing='0' width='100%'>");
                        sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White; font-size: 16px'>");
                        sbemailshow.AppendLine("<td id='tdsubject' style='display:none;'>" + dvfilter[0][4].ToString() + "</td>");
                        sbemailshow.AppendLine("<td id='tdto' style='display:none;'>" + dvfilter[0][3].ToString() + "</td>");
                        sbemailshow.AppendLine("<td id='tdmailid' style='display:none;'>" + dvfilter[0][1].ToString() + "</td>");
                        sbemailshow.AppendLine("<td id='tdmailCc' style='display:none;'>" + dvfilter[0][12].ToString() + "</td>");
                        sbemailshow.AppendLine("<td id='tdmailBcc' style='display:none;'>" + dvfilter[0][13].ToString() + "</td>");
                        sbemailshow.AppendLine("</tr>");
                        sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><span id='spafrom' style='font-size: 13px; float: left'>&nbsp;From: " + Server.HtmlEncode(dvfilter[0][2].ToString()) + "</span> <span id='spaSentOn' style='font-size: 13px;float: right;margin-right:20px;vertical-align:text-top'>Date: " + Convert.ToDateTime(dvfilter[0][6].ToString()).ToString("f") + "</span></td>");
                        sbemailshow.AppendLine("</tr>");



                        sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><span id='spacc' style='font-size: 13px; float: left'>&nbsp;To: " + Server.HtmlEncode(dvfilter[0]["To"].ToString()) + "</span> </td>");
                        sbemailshow.AppendLine("</tr>");

                        sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><span id='spabcc' style='font-size: 13px; float: left'>&nbsp;Cc: " + Server.HtmlEncode(dvfilter[0]["cc"].ToString()) + "</span> </td>");
                        sbemailshow.AppendLine("</tr>");

                        sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><span id='spasubject' style='font-size: 13px; float: left'>&nbsp;Bcc: " + Server.HtmlEncode(dvfilter[0]["bcc"].ToString()) + "</span> </td>");
                        sbemailshow.AppendLine("</tr>");
                        sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><span id='spasubject' style='font-size: 13px; float: left'>&nbsp;Subject: " + dvfilter[0]["subject"].ToString() + "</span> </td>");
                        sbemailshow.AppendLine("</tr>");

                        sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td>" + sbShowAttachments.ToString() + "</td>");
                        sbemailshow.AppendLine("</tr>");
                        sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White;'>");
                        sbemailshow.AppendLine("<td><div id='divbody' style='padding:10px;background-color:white;border:solid 1px #ddd;height:400px;overflow:auto;'>" + lrtshow.Text + "</div></td>");
                        sbemailshow.AppendLine("</tr>");
                        sbemailshow.AppendLine("</table></div>");
                        ltrsubjectshow.Text = sbemailshow.ToString(); // dvfilter[0][5].ToString();
                        //trEmailList.Visible = false;
                        //trEmailDetails.Visible = true;
                        // Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "if(window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnloadtreeagain')){window.parent.document.getElementById('ctl00_ContentPlaceHolder1_btnloadtreeagain').click();}", true);
                        trEmailList.Attributes.Add("style", "display:none");
                        trReFwd.Attributes.Add("style", "display:none");
                        trEmailDetails.Attributes.Add("style", "display:''");
                        divpageitem.Visible = false;

                        //objSql.ExecuteNonQuery("UPDATE tb_Ecomm_EmailList SET isread=0 WHERE MailID=" + MailIDg + "");
                        WebMailComponent.SetMessageTagAsRead(MailIDg, Convert.ToInt32(Session["AdminID"]));

                    }
                    else
                    {
                        ltrsubjectshow.Text = "";
                        //trEmailList.Visible = true;
                        //trEmailDetails.Visible = false;
                        trEmailList.Attributes.Add("style", "display:''");
                        divpageitem.Visible = true;
                        trEmailDetails.Attributes.Add("style", "display:none");
                        trReFwd.Attributes.Add("style", "display:none");
                    }
                }
            }
        }

        /// <summary>
        /// Mail Log Gridview Row Created Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvMailLog_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.Pager)
            {
                e.Row.Visible = false;
            }
        }

        /// <summary>
        /// Mail Log Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvMailLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "font-weight:normal");
                e.Row.Cells[4].Attributes.Add("style", "font-weight:normal");
                e.Row.Cells[5].Attributes.Add("style", "font-weight:normal");
                e.Row.Cells[8].Attributes.Add("style", "font-weight:normal");
                e.Row.Cells[9].Attributes.Add("style", "font-weight:normal");
                if (isDescendFrom == false)
                {
                    ImageButton btnFrom = (ImageButton)e.Row.FindControl("btnFrom");
                    btnFrom.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnFrom.AlternateText = "Ascending Order";
                    btnFrom.ToolTip = "Ascending Order";
                    btnFrom.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnFrom = (ImageButton)e.Row.FindControl("btnFrom");
                    btnFrom.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnFrom.AlternateText = "Descending Order";
                    btnFrom.ToolTip = "Descending Order";
                    btnFrom.CommandArgument = "ASC";
                }
                if (isDescendSubject == false)
                {
                    ImageButton btnSubject = (ImageButton)e.Row.FindControl("btnSubject");
                    btnSubject.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnSubject.AlternateText = "Ascending Order";
                    btnSubject.ToolTip = "Ascending Order";
                    btnSubject.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnSubject = (ImageButton)e.Row.FindControl("btnSubject");
                    btnSubject.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnSubject.AlternateText = "Descending Order";
                    btnSubject.ToolTip = "Descending Order";
                    btnSubject.CommandArgument = "ASC";
                }
                if (isDescendDate == false)
                {
                    ImageButton btnDate = (ImageButton)e.Row.FindControl("btnDate");
                    btnDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnDate.AlternateText = "Ascending Order";
                    btnDate.ToolTip = "Ascending Order";
                    btnDate.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnDate = (ImageButton)e.Row.FindControl("btnDate");
                    btnDate.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnDate.AlternateText = "Descending Order";
                    btnDate.ToolTip = "Descending Order";
                    btnDate.CommandArgument = "ASC";
                }
            }

            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "Highlight(this);");

                e.Row.Attributes.Add("onmouseout", "UnHighlight(this);");
                int i = e.Row.RowIndex;
                i++;
                if (i % 2 == 0)
                {
                    e.Row.Attributes.Add("style", "background: none repeat scroll 0 0 #FBFBFB;");
                }
                HiddenField hdnattchg = (HiddenField)e.Row.FindControl("hdnattch");
                LinkButton lnksubject = (LinkButton)e.Row.FindControl("lnksubject");
                LinkButton lblFromEmail = (LinkButton)e.Row.FindControl("lblFromEmail");
                Image imgattchg = (Image)e.Row.FindControl("imgattach");
                System.Web.UI.HtmlControls.HtmlImage objImg = (System.Web.UI.HtmlControls.HtmlImage)e.Row.FindControl("imgmsg");
                Label lblReadIdD = (Label)e.Row.FindControl("lblReadIdD");
                Label lblSentOn = (Label)e.Row.FindControl("lblSentOn");

                if (Convert.ToBoolean(lblReadIdD.Text) == true)
                {
                    objImg.Src = "/Admin/images/read.png";
                    lblFromEmail.Font.Bold = true;
                    lnksubject.Font.Bold = true;
                    lblSentOn.Font.Bold = true;
                    objImg.Alt = "Unread";

                }
                else
                {
                    objImg.Src = "/Admin/images/unread.png";
                    lblFromEmail.Font.Bold = false;
                    lnksubject.Font.Bold = false;
                    lblSentOn.Font.Bold = false;
                    objImg.Alt = "Read";
                }
                if (Convert.ToBoolean(hdnattchg.Value) == true)
                {
                    imgattchg.Visible = true;
                    imgattchg.ImageUrl = "~/Admin/images/attachment1.png";
                }
                else
                {
                    imgattchg.Visible = false;
                    imgattchg.ImageUrl = "";
                }
            }
        }
        #endregion


        /// <summary>
        /// Show Body function that call when Order Email message body have to show
        /// </summary>
        /// <param name="pMailID">int pMailID</param>
        public void ShowBody(int pMailID)
        {
            EmailAttachmentPath = AppLogic.AppConfigs("WebMailAttachmentPath");
            if (Session["dataviewRecord"] != null)
            {
                DataSet dsFilter = new DataSet();
                dsFilter = (DataSet)Session["dataviewRecord"];
                DataView dvfilter = dsFilter.Tables[0].DefaultView;
                if (dvfilter.Count > 0)
                {
                    //System.Web.UI.WebControls.Literal lrtto = new System.Web.UI.WebControls.Literal();
                    //System.Web.UI.WebControls.Literal lrtfrom = new System.Web.UI.WebControls.Literal();
                    //lrtto.Text = dvfilter[0][3].ToString();
                    //lrtfrom.Text = dvfilter[0][2].ToString();

                    StringBuilder sbemailshow = new StringBuilder();
                    System.Web.UI.WebControls.Literal lrtshow = new System.Web.UI.WebControls.Literal();
                    StringBuilder sbShowAttachments = new StringBuilder();
                    StringBuilder sbForwardAttachment = new StringBuilder();
                    String strAttchpath = Server.MapPath(EmailAttachmentPath + "MailID_" + pMailID.ToString());

                    if (Directory.Exists(strAttchpath))
                    {
                        String[] GetAllFileNames = Directory.GetFiles(strAttchpath);

                        for (int i = 0; i < GetAllFileNames.Length; i++)
                        {
                            FileInfo fi = new FileInfo(GetAllFileNames[i].ToString());

                            double FileSize = 0;
                            String FileSizePrint = "";
                            FileSize = fi.Length;
                            FileSizePrint = FileSize.ToString() + "byte";
                            if (FileSize > 1024)
                            {
                                FileSize = Math.Round(FileSize / 1024, 2);
                                FileSizePrint = FileSize.ToString() + "KB";
                            }
                            else if (FileSize > (1024 * 1204))
                            {
                                FileSize = Math.Round(FileSize / (1024 * 1204), 2);
                                FileSizePrint = FileSize.ToString() + "MB";
                            }

                            if (fi.Name != "Thumbs.db")
                            {
                                if (i == 0)
                                {
                                    sbShowAttachments.Append("<b>&nbsp;Attachments :</b><br/><br/>");
                                }
                                string strdwnloadtemp = EmailAttachmentPath + "MailID_" + pMailID.ToString() + "/" + fi.Name.ToString();
                                //sbShowAttachments.AppendLine((i + 1) + ". <a href=\"" + downloadfile("/AttachmentsFiles/" + "MailID_" + MailIDg.ToString() + "/" + fi.Name.ToString()) + "\" style='color: #696A6A;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none'>" + fi.Name.ToString() + "</a>&nbsp;&nbsp;&nbsp;");
                                sbShowAttachments.AppendLine("<input style='display:none;' type='submit' runat='server' value='" + Server.MapPath(strdwnloadtemp).ToString().Replace("'", "''") + "' name='btnDownload-main-" + i + "'  id='btnDownload-main-" + i + "' onclick='submit' /> <a href='#' style='color: #212121;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none' onclick=\"document.getElementById('btnDownload-main-" + i + "').click();\">" + (i + 1) + ". " + fi.Name.ToString() + "(" + FileSizePrint.ToString() + ")" + "</a>&nbsp;&nbsp;&nbsp;");
                                sbForwardAttachment.AppendLine("<input type='checkbox' id='chkfwdattach-" + i + "' checked='checked' onclick='Getbtnvalue();' onchange='Getbtnvalue();'/><input style='display:none;' type='submit' runat='server' value='" + Server.MapPath(strdwnloadtemp).ToString().Replace("'", "''") + "' name='btnDownload-mainattach-" + i + "'  id='btnDownload-mainattach-" + i + "' onclick='submit' /> <a href='#' style='color: #212121;font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;line-height: normal; padding: 2px;Text-decoration:none' onclick=\"document.getElementById('btnDownload-mainattach-" + i + "').click();\">" + (i + 1) + ". " + fi.Name.ToString() + "(" + FileSizePrint.ToString() + ")" + "</a>&nbsp;&nbsp;&nbsp;");
                                //Links = Links + "<input style='display:none;' type='submit' runat='server' value='" + strdwnloadtemp + "' name='btnDownload-main-" + i + "'  id='btnDownload-main-" + i + "' onclick='submit' /> <a href='#' onclick=\"document.getElementById('btnDownload-main-" + i + "').click();\">" + fi.Name.ToString() + "</a>&nbsp;&nbsp;&nbsp;";
                            }
                        }
                    }
                    if (sbShowAttachments.ToString() == "")
                    {
                        sbShowAttachments.Append("No Attachment(s) Found.");
                    }
                    if (sbForwardAttachment.ToString() == "")
                    {
                        sbForwardAttachment.Append("No Attachment(s) Found.");
                    }
                    ltrFwdAttach.Text = sbForwardAttachment.ToString();

                    lrtshow.Text = Server.HtmlDecode(dvfilter[0][5].ToString());
                    sbemailshow.Append("<div id='getprint'><table cellpadding='5' cellspacing='0' width='100%'>");
                    sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White; font-size: 16px'>");
                    sbemailshow.AppendLine("<td id='tdsubject' style='display:none;'>" + dvfilter[0][4].ToString() + "</td>");
                    sbemailshow.AppendLine("<td id='tdto' style='display:none;'>" + dvfilter[0][3].ToString() + "</td>");
                    sbemailshow.AppendLine("<td id='tdmailid' style='display:none;'>" + dvfilter[0][1].ToString() + "</td>");
                    sbemailshow.AppendLine("<td id='tdmailCc' style='display:none;'>" + dvfilter[0][12].ToString() + "</td>");
                    sbemailshow.AppendLine("<td id='tdmailBcc' style='display:none;'>" + dvfilter[0][13].ToString() + "</td>");
                    sbemailshow.AppendLine("</tr>");
                    sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><span id='spafrom' style='font-size: 13px; float: left'>&nbsp;From: " + dvfilter[0][2].ToString() + "</span> <span id='spaSentOn' style='font-size: 13px;float: right;margin-right:20px;vertical-align:text-top'>Date: " + Convert.ToDateTime(dvfilter[0][6].ToString()).ToString("f") + "</span></td>");
                    sbemailshow.AppendLine("</tr>");



                    sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><span id='spacc' style='font-size: 13px; float: left'>&nbsp;To: " + dvfilter[0]["To"].ToString() + "</span> </td>");
                    sbemailshow.AppendLine("</tr>");

                    sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><span id='spabcc' style='font-size: 13px; float: left'>&nbsp;Cc: " + dvfilter[0]["cc"].ToString() + "</span> </td>");
                    sbemailshow.AppendLine("</tr>");

                    sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><span id='spasubject' style='font-size: 13px; float: left'>&nbsp;Bcc: " + dvfilter[0]["bcc"].ToString() + "</span> </td>");
                    sbemailshow.AppendLine("</tr>");
                    sbemailshow.AppendLine("<tr style='background-color: White; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><span id='spasubject' style='font-size: 13px; float: left'>&nbsp;Subject: " + dvfilter[0]["subject"].ToString() + "</span> </td>");
                    sbemailshow.AppendLine("</tr>");

                    sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td>" + sbShowAttachments.ToString() + "</td>");
                    sbemailshow.AppendLine("</tr>");
                    sbemailshow.AppendLine("<tr style='background-color: #F2F2F2; border: 1px solid White;'>");
                    sbemailshow.AppendLine("<td><div id='divbody' style='padding:10px;background-color:white;border:solid 1px #ddd;height:500px;overflow:auto;'>" + lrtshow.Text + "</div></td>");
                    sbemailshow.AppendLine("</tr>");
                    sbemailshow.AppendLine("</table></div>");
                    ltrsubjectshow.Text = sbemailshow.ToString(); // dvfilter[0][5].ToString();
                    //trEmailList.Visible = false;
                    //trEmailDetails.Visible = true;
                    trEmailList.Attributes.Add("style", "display:none");
                    trReFwd.Attributes.Add("style", "display:none");
                    trEmailDetails.Attributes.Add("style", "display:''");
                }
                else
                {
                    ltrsubjectshow.Text = "";
                    //trEmailList.Visible = true;
                    //trEmailDetails.Visible = false;
                    trEmailList.Attributes.Add("style", "display:''");
                    trEmailDetails.Attributes.Add("style", "display:none");
                    trReFwd.Attributes.Add("style", "display:none");
                }

            }
        }


        #region Grid View Paging

        /// <summary>
        /// Dynamic Gridview paging
        /// </summary>
        /// <param name="_dt">DataTable _dt</param>
        protected void managePaging(DataTable _dt)
        {
            if (_dt.Rows.Count > 0)
            {
                plcPaging.Controls.Clear();
                // Variable declaration
                int numberOfPages;
                int numberOfRecords = dt.Rows.Count;
                int currentPage = (gvMailLog.PageIndex);
                StringBuilder strSummary = new StringBuilder();


                // If number of records is more then the page size (specified in global variable)
                // Just to check either gridview have enough records to implement paging
                if (numberOfRecords > pageSize)
                {
                    // Calculating the total number of pages
                    numberOfPages = (int)Math.Ceiling((double)numberOfRecords / (double)pageSize);
                }
                else
                {
                    numberOfPages = 1;
                }


                // Creating a small summary for records.
                strSummary.Append("Displaying <b>");

                // Creating X f X Records
                int floor = (currentPage * pageSize) + 1;
                strSummary.Append(floor.ToString());
                strSummary.Append("</b>-<b>");
                int ceil = ((currentPage * pageSize) + pageSize);

                //let say you have 26 records and you specified 10 page size, 
                // On the third page it will return 30 instead of 25 as that is based on pageSize
                // So this check will see if the ceil value is increasing the number of records. Consider numberOfRecords
                if (ceil > numberOfRecords)
                {
                    strSummary.Append(numberOfRecords.ToString());
                }
                else
                {
                    strSummary.Append(ceil.ToString());
                }

                // Displaying Total number of records Creating X of X of About X records.
                strSummary.Append("</b> of About <b>");
                strSummary.Append(numberOfRecords.ToString());
                strSummary.Append("</b> Records</br>");


                litPagingSummary.Text = strSummary.ToString();


                //Variable declaration 
                //these variables will used to calculate page number display
                int pageShowLimitStart = 1;
                int pageShowLimitEnd = 1;



                // Just to check, either there is enough pages to implement page number display logic.
                if (pageDispCount > numberOfPages)
                {
                    pageShowLimitEnd = numberOfPages; // Setting the end limit to the number of pages. Means show all page numbers
                }
                else
                {
                    if (currentPage > 4) // If page index is more then 4 then need to less the page numbers from start and show more on end.
                    {
                        //Calculating end limit to show more page numbers
                        pageShowLimitEnd = currentPage + (int)(Math.Floor((decimal)pageDispCount / 2));
                        //Calculating Start limit to hide previous page numbers
                        pageShowLimitStart = currentPage - (int)(Math.Floor((decimal)pageDispCount / 2));
                    }
                    else
                    {
                        //Simply Displaying the 10 pages. no need to remove / add page numbers
                        pageShowLimitEnd = pageDispCount;
                    }
                }

                // Since the pageDispCount can be changed and limit calculation can cause < 0 values 
                // Simply, set the limit start value to 1 if it is less
                if (pageShowLimitStart < 1)
                    pageShowLimitStart = 1;


                //Dynamic creation of link buttons

                // First Link button to display with paging
                LinkButton objLbFirst = new LinkButton();
                objLbFirst.Click += new EventHandler(objLb_Click);
                objLbFirst.Text = ""; //First
                objLbFirst.Width = 16;
                objLbFirst.Height = 16;
                objLbFirst.ToolTip = "First";
                objLbFirst.Attributes.Add("style", "background: url(/Admin/images/Actions-go-first-view-icon.png) no-repeat scroll 0% 0% transparent;width: 16px; height: 16px; border: 0pt none;");
                objLbFirst.ID = "lb_FirstPage";
                objLbFirst.CommandName = "pgChange";
                objLbFirst.EnableViewState = true;
                objLbFirst.CommandArgument = "1";

                //Previous Link button to display with paging
                LinkButton objLbPrevious = new LinkButton();
                objLbPrevious.Click += new EventHandler(objLb_Click);
                objLbPrevious.Text = "";
                objLbPrevious.Width = 16;
                objLbPrevious.ToolTip = "Previous";
                objLbPrevious.Height = 16;
                objLbPrevious.Attributes.Add("style", "background: url(/Admin/images/Actions-go-previous-view-icon.png) no-repeat scroll 0% 0% transparent;width: 16px; height: 16px; border: 0pt none;");
                objLbPrevious.ID = "lb_PreviousPage";
                objLbPrevious.CommandName = "pgChange";
                objLbPrevious.EnableViewState = true;
                objLbPrevious.CommandArgument = currentPage.ToString();


                //of course if the page is the 1st page, then there is no need of First or Previous
                if (currentPage == 0)
                {
                    objLbFirst.Enabled = false;
                    objLbPrevious.Enabled = false;
                }
                else
                {
                    objLbFirst.Enabled = true;
                    objLbPrevious.Enabled = true;
                }


                //Adding control in a place holder

                plcPaging.Controls.Add(objLbFirst);
                plcPaging.Controls.Add(new LiteralControl("&nbsp;   &nbsp;")); // Just to give some space 
                plcPaging.Controls.Add(objLbPrevious);
                plcPaging.Controls.Add(new LiteralControl("&nbsp;   &nbsp;"));


                // Creating page numbers based on the start and end limit variables.
                for (int i = pageShowLimitStart; i <= pageShowLimitEnd; i++)
                {
                    if ((Page.FindControl("lb_" + i.ToString()) == null) && i <= numberOfPages)
                    {
                        LinkButton objLb = new LinkButton();
                        objLb.Click += new EventHandler(objLb_Click);
                        objLb.Text = i.ToString();
                        if ((currentPage + 1) == i)
                        {

                            objLb.Attributes.Add("style", "color:#ff8000;");

                        }
                        objLb.ID = "lb_" + i.ToString();
                        objLb.CommandName = "pgChange";
                        objLb.EnableViewState = true;
                        objLb.CommandArgument = i.ToString();

                        if ((currentPage + 1) == i)
                        {
                            objLb.Enabled = false;
                        }


                        plcPaging.Controls.Add(objLb);
                        plcPaging.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    }
                }

                // Last Link button to display with paging
                LinkButton objLbLast = new LinkButton();
                objLbLast.Click += new EventHandler(objLb_Click);
                objLbLast.Text = "";
                objLbLast.Width = 16;
                objLbLast.Height = 16;
                objLbLast.ToolTip = "Last";
                objLbLast.Attributes.Add("style", "background: url(/Admin/images/Actions-go-last-view-icon.png) no-repeat;width: 16px; height: 16px;");
                objLbLast.ID = "lb_LastPage";
                objLbLast.CommandName = "pgChange";
                objLbLast.EnableViewState = true;
                objLbLast.CommandArgument = numberOfPages.ToString();

                // Next Link button to display with paging
                LinkButton objLbNext = new LinkButton();
                objLbNext.Click += new EventHandler(objLb_Click);
                objLbNext.Text = "";
                objLbNext.Width = 16;
                objLbNext.Height = 16;
                objLbNext.ToolTip = "Next";
                objLbNext.Attributes.Add("style", "background: url(/Admin/images/Actions-go-next-view-icon.png) no-repeat scroll 0% 0% transparent;width: 16px; height: 16px; border: 0pt none;");
                objLbNext.ID = "lb_NextPage";
                objLbNext.CommandName = "pgChange";
                objLbNext.EnableViewState = true;
                objLbNext.CommandArgument = (currentPage + 2).ToString();

                //of course if the page is the last page, then there is no need of last or next
                if ((currentPage + 1) == numberOfPages)
                {
                    objLbLast.Enabled = false;
                    objLbNext.Enabled = false;
                }
                else
                {
                    objLbLast.Enabled = true;
                    objLbNext.Enabled = true;
                }


                // Adding Control to the place holder
                plcPaging.Controls.Add(new LiteralControl("of <b>" + numberOfPages.ToString() + "</b>"));
                plcPaging.Controls.Add(new LiteralControl("&nbsp;  &nbsp;"));
                plcPaging.Controls.Add(objLbNext);
                plcPaging.Controls.Add(new LiteralControl("&nbsp;  &nbsp;"));
                plcPaging.Controls.Add(objLbLast);
                plcPaging.Controls.Add(new LiteralControl("&nbsp;  &nbsp;"));
            }

        }

        /// <summary>
        /// sub function of Dynamic gridview paging
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        void objLb_Click(object sender, EventArgs e)
        {
            plcPaging.Controls.Clear();
            LinkButton objlb = (LinkButton)sender;
            gvMailLog.PageIndex = (int.Parse(objlb.CommandArgument.ToString()) - 1);
            gvMailLog.DataSource = dt;
            gvMailLog.DataBind();
            managePaging(dt);
        }
        #endregion


        #region Clear Extra ViewStates

        /// <summary>
        /// Restores view-state information from a previous page request that was saved by the <see cref="M:System.Web.UI.Control.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the control state to be restored.</param>
        protected override void LoadViewState(object savedState)
        {
            object[] myState = (object[])savedState;
            if (myState[0] != null)
                base.LoadViewState(myState[0]);

            if (myState[1] != null)
            {
                dt = (DataTable)myState[1];
                gvMailLog.DataSource = dt;
                gvMailLog.DataBind();

                managePaging(dt);
            }

        }

        /// <summary>
        /// Saves any server control view-state changes that have occurred since the time the page was posted back to the server.
        /// </summary>
        /// <returns>Returns the server control's current view state. If there is no view state associated with the control, this method returns null.</returns>
        protected override object SaveViewState()
        {
            object baseState = base.SaveViewState();
            return new object[] { baseState, dt };
        }

        /// <summary>
        /// Loads any saved view-state information to the <see cref="T:System.Web.UI.Page" /> object.
        /// </summary>
        /// <returns>The saved view state.</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// These Method maintain the Session and ViewState
        /// </summary>
        /// <param name="state">object state</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }
        #endregion


        /// <summary>
        /// Send mail button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsend_Click(object sender, EventArgs e)
        {
            Exception ex = SendMail();

            if (ex == null || ex.Message.ToString() == "")
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ShowType"] != null)
                {
                    if (Request.QueryString["ID"].ToString().ToLower() != "compose")
                    {
                        Response.Redirect("/Admin/WebMail/SuccessMessage.aspx?Id=" + Request.QueryString["ID"].ToString() + "&ShowType=" + Request.QueryString["ShowType"].ToString() + "");
                    }
                    else
                    {
                        if (Request.QueryString["OrderID"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Mail has been sent successfully', 'Message');window.close();});", true);
                        }
                        else
                        {
                            Response.Redirect("/Admin/WebMail/SuccessMessage.aspx?Id=" + Request.QueryString["ID"].ToString() + "&ShowType=" + Request.QueryString["ShowType"].ToString() + "");
                        }
                    }
                }
                else
                {
                    Response.Redirect("/Admin/WebMail/SuccessMessage.aspx");
                }
            }
            else
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "SendMailfailure", "<script> alert('" + ex.Message + "')");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "SendMailfailure", "$(document).ready( function() {jAlert('" + ex.Message.ToString() + "', 'Message');});", true);
            }

        }

        /// <summary>
        /// Order Mail Send to Admin  & Customer for give the Information About Order
        /// </summary>
        public Exception SendMail()
        {
            Exception exSendMail = new Exception();
            try
            {
                EmailAttachmentPath = AppLogic.AppConfigs("WebMailAttachmentPath");
                //AlternateView av = AlternateView.CreateAlternateViewFromString(txtrefwdtobody.Text.ToString(), null, "text/html");

                string connectAttachments = String.Empty;
                string AttachOnlyName = String.Empty;
                string strFileName = string.Empty;
                string AttachmentFilePath = Server.MapPath("~/" + EmailAttachmentPath);
                string FwdAttachments = string.Empty;
                int OrderIDs = 0;
                if (!Directory.Exists(AttachmentFilePath + "/Temp"))
                {
                    Directory.CreateDirectory(AttachmentFilePath + "/Temp");
                }

                if (hdnfwdattach.Value.ToString() != "")
                {
                    string Getval = hdnfwdattach.Value.ToString();
                    string[] strGetbtnval = Getval.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int ai = 0; ai < strGetbtnval.Length; ai++)
                    {
                        FileInfo fitemp = new FileInfo(strGetbtnval[ai].ToString());
                        if (ai == 0)
                        {
                            AttachOnlyName += fitemp.Name;
                            FwdAttachments += strGetbtnval[ai].ToString();
                        }
                        else
                        {
                            AttachOnlyName += ";" + fitemp.Name;
                            FwdAttachments += ";" + strGetbtnval[ai].ToString();
                        }
                    }
                }

                if (AttachOnlyName != "")
                {
                    AttachOnlyName += ";";
                }
                if (FileUpload1.HasFile)
                {
                    strFileName = AttachmentFilePath + "\\Temp\\" + FileUpload1.FileName;
                    connectAttachments += strFileName;
                    AttachOnlyName += FileUpload1.FileName;
                    FileUpload1.SaveAs(strFileName);
                    FileUpload1.Dispose();
                }
                if (FileUpload2.HasFile)
                {
                    strFileName = AttachmentFilePath + "\\Temp\\" + FileUpload2.FileName;
                    connectAttachments += ";" + strFileName;
                    AttachOnlyName += ";" + FileUpload2.FileName;
                    FileUpload2.SaveAs(strFileName);
                    FileUpload2.Dispose();
                }
                if (FileUpload3.HasFile)
                {
                    strFileName = AttachmentFilePath + "\\Temp\\" + FileUpload3.FileName;
                    connectAttachments += ";" + strFileName;
                    AttachOnlyName += ";" + FileUpload3.FileName;
                    FileUpload3.SaveAs(strFileName);
                    FileUpload3.Dispose();
                }
                if (FileUpload4.HasFile)
                {
                    strFileName = AttachmentFilePath + "\\Temp\\" + FileUpload4.FileName;
                    connectAttachments += ";" + strFileName;
                    AttachOnlyName += ";" + FileUpload4.FileName;
                    FileUpload4.SaveAs(strFileName);
                    FileUpload4.Dispose();
                }
                //AdminEcomm.Admin.Client.AppConfigs.StoreId = 1;

                if (Request.QueryString["OrderID"] != null)
                {
                    OrderIDs = Convert.ToInt32(SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["OrderID"])));
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(txtrefwdtobody.Text.ToString(), null, "text/html");
                exSendMail = CommonOperations.SendMailWithAttachmentForEmailManagement(txtrefwdto.Text.ToString(), txtrefwdsubject.Text.ToString(), Server.HtmlEncode(txtrefwdtobody.Text.ToString()), txtrefwdtobody.Text.ToString(), Request.UserHostAddress.ToString(), Convert.ToBoolean(chkishtml.Checked), connectAttachments.ToString(), AttachOnlyName.ToString(), txtrefwdcc.Text.ToString(), txtrefwdbcc.Text.ToString(), 0, AttachmentFilePath, FwdAttachments, OrderIDs, av, Convert.ToInt32(Session["AdminID"]));
            }
            catch (Exception ex)
            {

            }
            return exSendMail;
        }

        /// <summary>
        /// Mail Log Gridview Sorting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewSortEventArgs e</param>
        protected void gvMailLog_Sorting(object sender, GridViewSortEventArgs e)
        {

        }
    }
}