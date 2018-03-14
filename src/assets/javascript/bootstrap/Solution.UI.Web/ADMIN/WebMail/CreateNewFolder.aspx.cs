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
    public partial class CreateNewFolder : BasePage
    {

        #region Global Declaration

        int iValue = 0;

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
                BindTreeView();
                //popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
            }
        }

        /// <summary>
        /// Gets the Nodes
        /// </summary>
        /// <param name="tn">TreeNode tn</param>
        /// <param name="ii">int ii</param>
        /// <returns>Returns the Node Index</returns>
        private int GetNodes(TreeNode tn, int ii)
        {
            iValue = ii;
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode trnode1 in tn.ChildNodes)
                {
                    if (trnode1.Checked)
                    {
                        iValue = Convert.ToInt32(trnode1.Value.ToString());
                        return iValue;
                    }
                    else
                    {
                        GetNodes(trnode1, iValue);
                    }
                }
            }
            else
            {
                if (tn.Checked)
                {
                    iValue = Convert.ToInt32(tn.Value.ToString());
                    return iValue;
                }
            }

            return iValue;
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            bool isrelchk = false;
            foreach (TreeNode trnode in trvfolderlist.Nodes)
            {

                if (trnode.Checked)
                {
                    //bool isadded = objsql.ExecuteNonQuery("if NOT Exists(Select * from tb_Ecomm_EmailFolders where (FolderName = '" + txtdisplayname.Text.Replace("'", "''") + "' or FolderEmail = '" + txtemailid.Text.Replace("'", "''") + "')) Begin Insert into tb_Ecomm_EmailFolders (FolderName,CreatedOn,IsDeleted,FolderEmail,ParentFolderID) values('" + txtdisplayname.Text.Replace("'", "''") + "','" + DateTime.Now.Date.ToString() + "',0,'" + txtemailid.Text.Replace("'", "''") + "'," + Convert.ToInt32(trnode.Value) + ") END");
                    int isAdded = WebMailComponent.CreateNewFolder(txtdisplayname.Text, txtemailid.Text, Convert.ToInt32(trnode.Value));
                    if (isAdded > 0)
                    {
                        isrelchk = true;
                    }
                }
                else
                {
                    int NodeVAlue = GetNodes(trnode, 0);
                    if (NodeVAlue > 0)
                    {
                        //bool isadded = objsql.ExecuteNonQuery("if NOT Exists(Select * from tb_Ecomm_EmailFolders where (FolderName = '" + txtdisplayname.Text.Replace("'", "''") + "' or FolderEmail = '" + txtemailid.Text.Replace("'", "''") + "')) Begin Insert into tb_Ecomm_EmailFolders (FolderName,CreatedOn,IsDeleted,FolderEmail,ParentFolderID) values('" + txtdisplayname.Text.Replace("'", "''") + "','" + DateTime.Now.Date.ToString() + "',0,'" + txtemailid.Text.Replace("'", "''") + "'," + Convert.ToInt32(NodeVAlue) + ") END");
                        int isAdded = WebMailComponent.CreateNewFolder(txtdisplayname.Text, txtemailid.Text, Convert.ToInt32(NodeVAlue));
                        if (isAdded > 0)
                        {
                            isrelchk = true;
                        }
                    }
                }
            }

            if (isrelchk)
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "Added", "<script>alert('Folder Created Successfully.')</script>");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Added", "$(document).ready( function() {jAlert('Folder Created Successfully.', 'Message');});", true);

                trvfolderlist.Nodes.Clear();
                BindTreeView();
                //trvfolderlist.ExpandAll();
            }
            else
            {
                //ClientScript.RegisterStartupScript(Page.GetType(), "Problem", "<script>alert('Folder Name or EmailID already Exists.')</script>");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Problem", "$(document).ready( function() {jAlert('Folder Name or EmailID already Exists.', 'Message');});", true);
                return;
            }

        }

        /// <summary>
        /// Binds the Tree View
        /// </summary>
        public void BindTreeView()
        {
            DataSet DsVirtualFolder = new DataSet();
            DsVirtualFolder = WebMailComponent.GetParentFoldersForCreateNewFolder();

            if (DsVirtualFolder != null && DsVirtualFolder.Tables.Count > 0 && DsVirtualFolder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < DsVirtualFolder.Tables[0].Rows.Count; i++)
                {
                    TreeNode tempnode = new TreeNode(DsVirtualFolder.Tables[0].Rows[i]["FolderName"].ToString(), DsVirtualFolder.Tables[0].Rows[i]["FolderID"].ToString());
                    tempnode.ShowCheckBox = true;
                    tempnode.NavigateUrl = "javascript:void(0);";
                    BindChildFolder(tempnode, Convert.ToInt32(DsVirtualFolder.Tables[0].Rows[i]["FolderID"]));
                    trvfolderlist.Nodes.Add(tempnode);
                }
            }
        }

        /// <summary>
        /// Binds the Child Folder
        /// </summary>
        /// <param name="parentnode">TeeeNode parentnode</param>
        /// <param name="id">int id</param>
        public void BindChildFolder(TreeNode parentnode, int id)
        {
            DataSet dsGetChild = new DataSet();
            dsGetChild = WebMailComponent.GetChildFoldersForCreateNewFolder(id);// objsql.GetDs("select fol.FolderID,(Case When (fol.FolderName Is Not Null And fol.FolderName!='') Then ' '+fol.FolderName Else ' '+fol.FolderEmail End) As FolderName,fol.Createdon,fol.IsDeleted from tb_Ecomm_EmailFolders as fol,tb_Ecomm_EmailFolderRelation as Relfol where fol.FolderID=ChildFolderID and Relfol.ParentFolderID= " + id + " and fol.IsDeleted=0 order by (Case When (FolderName Is Not Null And FolderName!='') Then ' '+FolderName Else ' '+FolderEmail End) ASC");

            if (dsGetChild != null && dsGetChild.Tables.Count > 0 && dsGetChild.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsGetChild.Tables[0].Rows.Count; i++)
                {
                    TreeNode tempchild = new TreeNode(dsGetChild.Tables[0].Rows[i]["FolderName"].ToString(), dsGetChild.Tables[0].Rows[i]["FolderID"].ToString());
                    tempchild.ShowCheckBox = true;
                    tempchild.CollapseAll();
                    tempchild.NavigateUrl = "javascript:void(0);";
                    BindChildFolder(tempchild, Convert.ToInt32(dsGetChild.Tables[0].Rows[i]["FolderID"]));
                    parentnode.ChildNodes.Add(tempchild);
                }
            }
        }
    }
}