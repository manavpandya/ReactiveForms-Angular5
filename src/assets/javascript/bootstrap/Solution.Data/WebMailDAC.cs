using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;


namespace Solution.Data
{
    /// <summary>
    /// WebMail Data Access Class Contains WebMail related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class WebMailDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get All Treeview Folders for Fill up Treeview with Recursive loop
        /// </summary>
        /// <param name="FolderID"></param>
        /// <param name="Mode"></param>
        /// <returns>Returns all folder list as a Dataset</returns>
        public DataSet GetTreeviewFolder(Int32 FolderID, Int32 Mode, Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Inbox Message Count that are not Deleted
        /// </summary>
        /// <returns>Returns Message box count</returns>
        public Int32 GetAllInboxMessageCount(Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get All Deleted Message Count
        /// </summary>
        /// <returns>Returns count</returns>
        public Int32 GetAllDeletedMessageCount(Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get All Recent Message Count
        /// </summary>
        /// <returns>Returns count</returns>
        public Int32 GetAllRecentMessageCount(Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 9);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Other Message Count that are Not Deleted
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Other Message Count</returns>
        public Int32 GetOtherMessageCount(Int32 FolderID, Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 5);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get All Folders Details except currently Selected folder those are bind into Move to Dropdown
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder List</returns>
        public DataSet GetMoveToFolderList(Int32 FolderID,Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 6);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get Folder Name For ShowBody to display in Header
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <returns>Returns Folder Name </returns>
        public String GetFolderNameForShowBody(Int32 MailID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@MailID", MailID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Folder Name For Others to display in Header
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder Name</returns>
        public String GetFolderNameForOthers(Int32 FolderID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Email Message according to various condition
        /// </summary>
        /// <param name="ShowType">String ShowType</param>
        /// <param name="ID">Int32 ID</param>
        /// <returns>Returns Message Details as a Dataset</returns>
        public DataSet GetEmailMessage(String ShowType, String ID, Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";

            if (ShowType.ToString().Trim().ToLower().IndexOf("inbox") > -1)
            {
                cmd.Parameters.AddWithValue("@Mode", 3);
            }
            else if (ShowType.ToString().Trim().ToLower().IndexOf("sent items") > -1)
            {
                cmd.Parameters.AddWithValue("@Mode", 4);
            }
            else if (ShowType.ToString().Trim().ToLower().IndexOf("showbody") > -1)
            {
                cmd.Parameters.AddWithValue("@MailID", Convert.ToInt32(ID));
                cmd.Parameters.AddWithValue("@Mode", 5);
            }
            else if (ShowType.ToString().Trim().ToLower().IndexOf("deleted items") > -1)
            {
                cmd.Parameters.AddWithValue("@Mode", 6);
            }
            else if (ShowType.ToString().Trim().ToLower().IndexOf("spam") > -1)
            {
                cmd.Parameters.AddWithValue("@Mode", 7);
            }
            else if (ShowType.ToString().Trim().ToLower().IndexOf("recent email") > -1)
            {
                cmd.Parameters.AddWithValue("@Mode", 16);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FolderID", Convert.ToInt32(ID));
                cmd.Parameters.AddWithValue("@Mode", 8);
            }
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Move Message function
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <param name="MailID">Int32 MailID</param>
        /// <returns>Returns integer</returns>
        public Int32 MoveMessage(Int32 FolderID, Int32 MailID, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MoveToQueries";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@MailID", MailID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get Folder Name For Move To
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder Name</returns>
        public String GetFolderNameForMoveTo(Int32 FolderID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MoveToQueries";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get EmailIDs From Zip Code
        /// </summary>
        /// <param name="ZipCode">String ZipCode</param>
        /// <returns>Returns Email Ids into the form of For XML Path Output</returns>
        public String GetEmailIDsFromZipCode(String ZipCode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
            cmd.Parameters.AddWithValue("@Mode", 9);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Set Message Tag As Read
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        public void SetMessageTagAsRead(Int32 MailID,Int32 AgentID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@MailID", MailID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", 10);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Delete Email Message either Permanently or Temporarily  
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Return Boolean Value</returns>
        public bool DeleteEmailMessage(Int32 MailID, Int32 Mode,Int32 AgentID)
        {
            bool isDeletedTrue = false;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@MailID", MailID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int isdeleted = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));

            if (isdeleted > 0)
            {
                isDeletedTrue = true;
            }
            return isDeletedTrue;
        }

        /// <summary>
        /// Function  For "Delete", "Spam" and  "Mark As Unread" facility
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Returns Boolean Values</returns>
        public bool Delete_Spam_MarkAsUnread(Int32 MailID, Int32 Mode,Int32 AgentID)
        {
            bool isUpdateTrue = false;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_MailBoxSettings";
            cmd.Parameters.AddWithValue("@MailID", MailID);
            cmd.Parameters.AddWithValue("@AgentID", AgentID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int isUpdate = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));

            if (isUpdate > 0)
            {
                isUpdateTrue = true;
            }
            return isUpdateTrue;
        }

        /// <summary>
        /// Get Parent Folders For Create New Folder
        /// </summary>
        /// <returns>Returns Parent Folder List</returns>
        public DataSet GetParentFoldersForCreateNewFolder()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@Mode", 7);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Child Folders For Create New Folder
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns>Returns Child Folder List</returns>
        public DataSet GetChildFoldersForCreateNewFolder(Int32 FolderID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_TreeViewSettings";
            cmd.Parameters.AddWithValue("@FolderID", FolderID);
            cmd.Parameters.AddWithValue("@Mode", 8);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Create New Folder For Web Mail
        /// </summary>
        /// <param name="FolderName">String FolderName</param>
        /// <param name="FolderEmail">String FolderEmail</param>
        /// <param name="ParentFolderID">Int32 ParentFolderID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 CreateNewFolder(String FolderName, String FolderEmail, Int32 ParentFolderID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WebMail_CreateNewFolder";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@FolderName", FolderName);
            cmd.Parameters.AddWithValue("@FolderEmail", FolderEmail);
            cmd.Parameters.AddWithValue("@ParentFolderID", ParentFolderID);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }


        #endregion

    }
}
