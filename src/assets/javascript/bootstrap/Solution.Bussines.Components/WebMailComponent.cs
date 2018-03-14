using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// WebMail Component Class Contains WebMail related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class WebMailComponent
    {
        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes variables when component file is loading
        /// </summary>
        public WebMailComponent()
        {

        }

        #endregion

        #region  Key Function

        /// <summary>
        /// Get All Treeview Folders for Fill up Treeview with Recursive loop
        /// </summary>
        /// <param name="FolderID"></param>
        /// <param name="Mode"></param>
        /// <returns>Returns all folder list as a Dataset</returns>
        public static DataSet GetTreeviewFolder(Int32 FolderID, Int32 Mode, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetTreeviewFolder(FolderID, Mode, AgentID);
        }

        /// <summary>
        /// Get All Inbox Message Count that are not Deleted
        /// </summary>
        /// <returns>Returns Message box count</returns>
        public static Int32 GetAllInboxMessageCount(Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetAllInboxMessageCount(AgentID);
        }

        /// <summary>
        /// Get All Deleted Message Count
        /// </summary>
        /// <returns>Returns count</returns>
        public static Int32 GetAllDeletedMessageCount(Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetAllDeletedMessageCount(AgentID);
        }

        /// <summary>
        /// Get All Recent Mali Message Count
        /// </summary>
        /// <returns>Returns count</returns>
        public static Int32 GetAllRecentMessageCount(Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetAllRecentMessageCount(AgentID);
        }

        /// <summary>
        /// Get Other Message Count that are Not Deleted
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Other Message Count</returns>
        public static Int32 GetOtherMessageCount(Int32 FolderID, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetOtherMessageCount(FolderID, AgentID);
        }

        /// <summary>
        /// Get All Folders Details except currently Selected folder those are bind into Move to Dropdown
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder List</returns>
        public static DataSet GetMoveToFolderList(Int32 FolderID, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetMoveToFolderList(FolderID, AgentID);
        }


        /// <summary>
        /// Get Folder Name For ShowBody to display in Header
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <returns>Returns Folder Name </returns>
        public static String GetFolderNameForShowBody(Int32 MailID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetFolderNameForShowBody(MailID);
        }


        /// <summary>
        /// Get Folder Name For Others to display in Header
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder Name</returns>
        public static String GetFolderNameForOthers(Int32 FolderID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetFolderNameForOthers(FolderID);
        }

        /// <summary>
        /// Get Email Message according to various condition
        /// </summary>
        /// <param name="ShowType">String ShowType</param>
        /// <param name="ID">Int32 ID</param>
        /// <returns>Returns Message Details as a Dataset</returns>
        public static DataSet GetEmailMessage(String ShowType, String ID, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetEmailMessage(ShowType, ID, AgentID);
        }

        /// <summary>
        /// Move Message function
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <param name="MailID">Int32 MailID</param>
        /// <returns>Returns integer</returns>
        public static Int32 MoveMessage(Int32 FolderID, Int32 MailID, Int32 Mode)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.MoveMessage(FolderID, MailID, Mode);
        }


        /// <summary>
        /// Get Folder Name For Move To
        /// </summary>
        /// <param name="FolderID">Int32 FolderID</param>
        /// <returns>Returns Folder Name</returns>
        public static String GetFolderNameForMoveTo(Int32 FolderID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetFolderNameForMoveTo(FolderID);
        }

        /// <summary>
        /// Get EmailIDs From Zip Code
        /// </summary>
        /// <param name="ZipCode">String ZipCode</param>
        /// <returns>Returns Email Ids into the form of For XML Path Output</returns>
        public static String GetEmailIDsFromZipCode(String ZipCode)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetEmailIDsFromZipCode(ZipCode);
        }

        /// <summary>
        /// Set Message Tag As Read
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        public static void SetMessageTagAsRead(Int32 MailID, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            objWebMail.SetMessageTagAsRead(MailID, AgentID);
        }

        /// <summary>
        /// Delete Email Message either Permanently or Temporarily  
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Return Boolean Value</returns>
        public static bool DeleteEmailMessage(Int32 MailID, Int32 Mode, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.DeleteEmailMessage(MailID, Mode, AgentID);
        }


        /// <summary>
        /// Function  For "Delete", "Spam" and  "Mark As Unread" facility
        /// </summary>
        /// <param name="MailID">Int32 MailID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Returns Boolean Values</returns>
        public static bool Delete_Spam_MarkAsUnread(Int32 MailID, Int32 Mode, Int32 AgentID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.Delete_Spam_MarkAsUnread(MailID, Mode, AgentID);
        }
        #endregion

        /// <summary>
        /// Get Parent Folders For Create New Folder
        /// </summary>
        /// <returns>Returns Parent Folder List</returns>
        public static DataSet GetParentFoldersForCreateNewFolder()
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetParentFoldersForCreateNewFolder();
        }

        /// <summary>
        /// Get Child Folders For Create New Folder
        /// </summary>
        /// <param name="FolderID"></param>
        /// <returns>Returns Child Folder List</returns>
        public static DataSet GetChildFoldersForCreateNewFolder(Int32 FolderID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.GetChildFoldersForCreateNewFolder(FolderID);
        }

        /// <summary>
        /// Create New Folder For Web Mail
        /// </summary>
        /// <param name="FolderName">String FolderName</param>
        /// <param name="FolderEmail">String FolderEmail</param>
        /// <param name="ParentFolderID">Int32 ParentFolderID</param>
        /// <returns>Returns Identity Value</returns>
        public static Int32 CreateNewFolder(String FolderName, String FolderEmail, Int32 ParentFolderID)
        {
            WebMailDAC objWebMail = new WebMailDAC();
            return objWebMail.CreateNewFolder(FolderName, FolderEmail, ParentFolderID);
        }
    }
}
