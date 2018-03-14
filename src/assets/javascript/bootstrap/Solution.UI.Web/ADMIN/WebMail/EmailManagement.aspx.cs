using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.WebMail
{
    public partial class EmailManagement : BasePage
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
                BindTreeView();

                if (Request.QueryString["ShowType"] != null && Convert.ToString(Request.QueryString["ID"]) != "")
                {
                    if (Request.QueryString["ShowType"] == "ShowBody")
                    {
                        ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + Request.QueryString["ShowType"].ToString() + "&ID=" + Request.QueryString["ID"].ToString() + "");
                    }
                }
            }
        }

        /// <summary>
        /// Binds the Tree View
        /// </summary>
        public void BindTreeView()
        {
            trvFolders.Nodes.Clear();
            DataSet DsVirtualFolder = new DataSet();
            DsVirtualFolder = WebMailComponent.GetTreeviewFolder(0, 1, Convert.ToInt32(Session["AdminID"]));

            //(Case When (SalePrice Is Not Null And SalePrice!=0) Then SalePrice Else Price End) As SalePrice
            if (DsVirtualFolder != null && DsVirtualFolder.Tables.Count > 0 && DsVirtualFolder.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < DsVirtualFolder.Tables[0].Rows.Count; i++)
                {
                    TreeNode tempnode = new TreeNode(DsVirtualFolder.Tables[0].Rows[i]["FolderName"].ToString(), DsVirtualFolder.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Emailm.png");
                    BindChildFolder(tempnode, Convert.ToInt32(DsVirtualFolder.Tables[0].Rows[i]["FolderID"]));
                    tempnode.NavigateUrl = "javascript:void(0);";
                    trvFolders.Nodes.Add(tempnode);
                }
                trvFolders.Attributes.Add("onclick", "javascript:treeNodeConfirmation(event);");
                trvFolders.ExpandAll();
            }
        }

        /// <summary>
        /// Binds the Child Folder
        /// </summary>
        /// <param name="parentnode">TeeNode parentnode</param>
        /// <param name="id">int id</param>
        public void BindChildFolder(TreeNode parentnode, int id)
        {
            DataSet dsGetChild = new DataSet();
            //dsGetChild = objsql.GetDs("select fol.FolderID,(Case When (fol.FolderName Is Not Null And fol.FolderName!='') Then fol.FolderName Else fol.FolderEmail End) As FolderName,fol.Createdon,fol.IsDeleted from tb_Ecomm_EmailFolders as fol,tb_Ecomm_EmailFolderRelation as Relfol where fol.FolderID=ChildFolderID and Relfol.ParentFolderID= " + id + " and fol.IsDeleted=0 Order By FolderName");

            dsGetChild = WebMailComponent.GetTreeviewFolder(id, 2, Convert.ToInt32(Session["AdminID"]));

            if (dsGetChild != null && dsGetChild.Tables.Count > 0 && dsGetChild.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsGetChild.Tables[0].Rows.Count; i++)
                {
                    TreeNode tempchild;
                    Int32 TotalCount = 0;
                    if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower() == "inbox")
                    {
                        // mode =3
                        //TotalCount = Convert.ToInt32(objSql.ExecuteScalerQuery("SELECT Count(*) FROM tb_Ecomm_EmailList WHERE isnull(IsRead,1)=1 and isnull(IsIncomming,0)=1 AND isnull(FolderID,0)<>0 and isnull(isDeleted,0)<>1"));
                        TotalCount = WebMailComponent.GetAllInboxMessageCount(Convert.ToInt32(Session["AdminID"]));

                    }
                    else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("deleted item") > -1)
                    {
                        // mode = 4
                        // TotalCount = Convert.ToInt32(objSql.ExecuteScalerQuery("SELECT Count(*) FROM tb_Ecomm_EmailList WHERE isnull(IsRead,1)=1 and isnull(IsIncomming,0)=1 AND isnull(FolderID,0)<>0 and isnull(isDeleted,0)=1"));
                        TotalCount = WebMailComponent.GetAllDeletedMessageCount(Convert.ToInt32(Session["AdminID"]));

                    }
                    else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("recent email") > -1)
                    {
                        TotalCount = WebMailComponent.GetAllRecentMessageCount(Convert.ToInt32(Session["AdminID"]));
                        //TotalCount = Convert.ToInt32(objSql.ExecuteScalerQuery("SELECT Count(*) FROM tb_Ecomm_EmailList WHERE isnull(IsRead,1)=1 and isnull(IsIncomming,0)=1 AND isnull(FolderID,0)<>0 and isnull(isDeleted,0)<>1 and CONVERT(CHAR(10),Createdon,101) >= CONVERT(CHAR(10),DATEADD(DAY,-1,GETDATE()),101) AND CONVERT(CHAR(10),Createdon,101) <= CONVERT(CHAR(10),GETDATE(),101)"));
                    }
                    else
                    {
                        if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("sent item") > -1)
                        {

                        }
                        else
                        {
                            // mode =5
                            //TotalCount = Convert.ToInt32(objSql.ExecuteScalerQuery("SELECT Count(*) FROM tb_Ecomm_EmailList WHERE isnull(IsRead,1)=1 and isnull(IsIncomming,0)=1 AND  isnull(isDeleted,0)<>1 and isnull(FolderID,0)=" + dsGetChild.Tables[0].Rows[i]["FolderID"].ToString() + ""));
                            TotalCount = WebMailComponent.GetOtherMessageCount(Convert.ToInt32(dsGetChild.Tables[0].Rows[i]["FolderID"]), Convert.ToInt32(Session["AdminID"]));
                        }
                    }
                    if (TotalCount > 0)
                    {
                        if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower() == "inbox")
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Email-Inbox-icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("sent item") > -1)
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/send-items.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("deleted item") > -1)
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Recycle-Bin-full-icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("recent email") > -1)
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Recent-Mail-Icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("spam") > -1)
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/webmail-spam-icon.png");
                        }
                        else
                        {
                            tempchild = new TreeNode("<b>" + dsGetChild.Tables[0].Rows[i]["FolderName"].ToString() + " (" + TotalCount.ToString() + ")</b>", dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/folder-icon.png");
                        }
                    }
                    else
                    {
                        if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower() == "inbox")
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Email-Inbox-icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("sent item") > -1)
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/send-items.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("deleted item") > -1)
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Recycle-Bin-full-icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("recent email") > -1)
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/Recent-Mail-Icon.png");
                        }
                        else if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]).ToLower().IndexOf("spam") > -1)
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/webmail-spam-icon.png");
                        }
                        else
                        {
                            tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString(), "/Admin/Images/folder-icon.png");
                        }
                    }

                    if (!IsPostBack)
                    {
                        if (Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderName"]) == "Inbox")
                        {
                            ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderID"]) + "&ID=" + Convert.ToString(dsGetChild.Tables[0].Rows[i]["FolderID"]) + "");
                            BindMoveTo(Convert.ToInt32(dsGetChild.Tables[0].Rows[i]["FolderID"]));
                        }
                    }
                    parentnode.ChildNodes.Add(tempchild);
                    BindChildFolder(tempchild, Convert.ToInt32(dsGetChild.Tables[0].Rows[i]["FolderID"]));
                }
            }
        }

        /// <summary>
        /// Move Messages into Specific folders
        /// </summary>
        /// <param name="vals">int vals</param>
        public void BindMoveTo(Int32 vals)
        {
            //mode =6
            //String strFolderName= objsql.ExecuteScalerQuery("Select (Case When (FolderName Is Not Null And FolderName!='') Then FolderName Else FolderEmail End) As FolderName from tb_Ecomm_EmailFolders
            DataSet dsGetmovelist = new DataSet();// objsql.GetDs("Select FolderID,(Case When (FolderName Is Not Null And FolderName!='') Then FolderName Else FolderEmail End) As FolderName,Createdon from tb_Ecomm_EmailFolders where IsDeleted=0 and FolderName!='Email Management' and FolderID!= " + vals + " ORDER BY FolderName");
            dsGetmovelist = WebMailComponent.GetMoveToFolderList(Convert.ToInt32(vals), Convert.ToInt32(Session["AdminID"]));

            if (dsGetmovelist != null && dsGetmovelist.Tables.Count > 0 && dsGetmovelist.Tables[0].Rows.Count > 0)
            {
                ddlmoveto.DataSource = dsGetmovelist.Tables[0];
                ddlmoveto.DataTextField = "FolderName";
                ddlmoveto.DataValueField = "FolderID";
                ddlmoveto.DataBind();
                ListItem ls = new ListItem("Select Folder", "0");
                ddlmoveto.Items.Insert(0, ls);
            }
        }

        /// <summary>
        ///  Compose Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkCompose_Click(object sender, EventArgs e)
        {
            trvFolders.Nodes[0].Select();
            ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=Compose&ID=Compose");
        }

        /// <summary>
        ///  Go Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (trvFolders.SelectedNode != null && trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("deleted items") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?Type=btnGo&deletd=1&SearchField=" + ddlsearchby.SelectedValue + "&SearchValue=" + txtsearchby.Text + "");
            }
            else if (trvFolders.SelectedNode != null && trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("spam") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?Type=btnGo&spm=1&SearchField=" + ddlsearchby.SelectedValue + "&SearchValue=" + txtsearchby.Text + "");
            }
            else
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?Type=btnGo&SearchField=" + ddlsearchby.SelectedValue + "&SearchValue=" + txtsearchby.Text + "");
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtsearchby.Text = "";
            ddlsearchby.SelectedIndex = 0;
        }

        /// <summary>
        ///  Refresh Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/Statistic/Emailmanagement.aspx");
        }

        /// <summary>
        /// Move to Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlmoveto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlmoveto.SelectedValue != "0")
            {
                //ifrmcontent.Attributes.Add("src", "/Admin/Statistic/EmailInboxmaster.aspx?ShowType=Compose&ID=Compose");

                if (trvFolders.SelectedNode.Text.ToString().Trim().ToLower().IndexOf("sent items") > -1 || trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("deleted items") > -1)
                {
                    ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Text.ToString() + "&ID=" + trvFolders.SelectedNode.Text.ToString() + "&Type=MoveTo&moveid=" + ddlmoveto.SelectedValue + "&ids=" + allids.Value);
                }
                else
                {
                    ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Value.ToString() + "&ID=" + trvFolders.SelectedNode.Value.ToString() + "&Type=MoveTo&moveid=" + ddlmoveto.SelectedValue + "&ids=" + allids.Value);
                }
            }
        }

        /// <summary>
        ///  Folders Treeview Selected Node Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void trvFolders_SelectedNodeChanged(object sender, EventArgs e)
        {
            txtsearchby.Text = "";
            ddlsearchby.SelectedIndex = 0;
            if (trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("sent items") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Text.ToString() + "&ID=" + trvFolders.SelectedNode.Text.ToString() + "");
            }
            else if (trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("spam") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Text.ToString() + "&ID=" + trvFolders.SelectedNode.Text.ToString() + "");
            }
            else if (trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("deleted items") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Text.ToString() + "&ID=" + trvFolders.SelectedNode.Text.ToString() + "");
            }
            else if (trvFolders.SelectedNode.Text.ToString().ToLower().IndexOf("recent email") > -1)
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Text.ToString() + "&ID=" + trvFolders.SelectedNode.Text.ToString() + "");
            }
            else
            {
                ifrmcontent.Attributes.Add("src", "/Admin/WebMail/EmailInboxmaster.aspx?ShowType=" + trvFolders.SelectedNode.Value.ToString() + "&ID=" + trvFolders.SelectedNode.Value.ToString() + "");
            }
            BindMoveTo(Convert.ToInt32(trvFolders.SelectedNode.Value));

        }

        /// <summary>
        /// Load Tree Again  Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnloadtreeagain_Click(object sender, EventArgs e)
        {
            BindTreeView();
        }
    }
}