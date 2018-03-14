using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using Solution.Data;
using System.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Admin Rights Component Class Contains Admin Rights Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class AdminRightsComponent
    {
        #region Declartion

        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();


        #endregion

        /// <summary>
        /// Get Admin List
        /// </summary>
        /// <param name="AdminID">int AdminID</param>
        /// <returns>Returns Admin Details List</returns>
        public List<AdminEntity> GetAdminList(int AdminID)
        {
            List<AdminEntity> Admin = null;
            Admin = (from a in ctxRedtag.tb_Admin
                     where a.Active == true && a.Deleted == false
                     && a.AdminType == 1
                     select new AdminEntity
                     {
                         AdminID = a.AdminID,
                         FirstName = a.FirstName + " " + a.LastName + ":" + a.EmailID,
                         Rights = a.Rights,
                         Templatename = a.TemplateName,
                         EmailID = a.EmailID
                     }).ToList();
            if (AdminID != 0)
                Admin = Admin.Where(a => a.AdminID == AdminID).ToList();
            return Admin;
        }

        /// <summary>
        /// Get Admin Rights List
        /// </summary>
        /// <returns>Returns Admin Rights List</returns>
        public List<tb_AdminRights> GetRightList()
        {
            List<tb_AdminRights> lstRights = new List<tb_AdminRights>();
            lstRights = (from a in ctxRedtag.tb_AdminRights
                         where (System.Boolean?)a.Active ?? false == true
                         select a).Distinct().ToList();
            return lstRights;
        }
        /// <summary>
        /// Get Admin Rights List for Page wise
        /// </summary>
        /// <returns>Returns Admin Rights List for Page wise</returns>
        public DataSet GetAdminPageRightList(Int32 AdminID)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminPageRightList(AdminID);
        }

        /// <summary>
        /// Get Admin by ID
        /// </summary>
        /// <param name="AdminID">int AdminID</param>
        /// <returns>Returns tb_Admin object</returns>        
        public tb_Admin GetAdminByID(int AdminID)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminByID(AdminID);
        }

        /// <summary>
        /// Update Admin Rights
        /// </summary>
        /// <param name="admin">tb_Admin admin</param>
        /// <returns>Returns No of rows affected</returns>
        public int UpdateAdminRights(tb_Admin admin)
        {
            int RowsAffected = 0;
            AdminRightsDAC dac = new AdminRightsDAC();
            RowsAffected = dac.UpdateAdminRights(admin);
            return RowsAffected;
        }

        /// <summary>
        /// Insert Update Page Rights For Admin
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <param name="ID">Int32 ID</param>
        /// <param name="InnerRightsID">Int32 InnerRightsID</param>
        /// <param name="IsListed">Boolean IsListed</param>
        /// <param name="CreatedBy">Int32 CreatedBy</param>
        /// <returns>Returns the Identity Value</returns>
        public Int32 Insert_Update_PageRightsForAdmin(Int32 MainAdminID, Int32 CompareAdminID, Int32 ID, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.Insert_Update_PageRightsForAdmin(MainAdminID, CompareAdminID, ID, InnerRightsID, IsListed, CreatedBy);
        }

        /// <summary>
        /// Get All Page Rights By AdminID
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        public void GetAllPageRightsByAdminID(Int32 AdminID)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            dac.GetAllPageRightsByAdminID(AdminID);
        }
        /// <summary>
        /// Get Admin Rights For Page
        /// </summary>
        /// <param name="TabName">String TabName</param>
        /// <param name="PageName">String PageName</param>
        /// <returns>Returns a DataRow that contains Rights Information</returns>
        public static DataRow GetAdminRightsForPage(String TabName, String PageName)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminRightsForPage(TabName, PageName);
        }

        #region Admin Template
        public DataSet GetAdminTemplatePageRightList()
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminTemplatePageRightList();
        }
        public DataSet GetAdminTemplatePageName()
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminTemplatePageName();
        }

        public DataSet GetAdminTemplatePageDetailByName(String Name)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminTemplatePageDetailByName(Name);
        }

        public Int32 Insert_Update_TemplatePageRightsForAdmin(Int32 ID, String Name, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy, String Rights)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.Insert_Update_TemplatePageRightsForAdmin(ID, Name, InnerRightsID, IsListed, CreatedBy, Rights);
        }
        public bool CheckDuplicateTemplatename(tb_admin_Templaterights AdminTemplate)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            if (dac.CheckDuplicateTemplatename(AdminTemplate) == 0)
            {
                return false;
            }
            else
                return true;
        }

        #endregion
        public Int32 Insert_Update_AssignPageRightsForAdmin(Int32 MainAdminID, Int32 CompareAdminID, Int32 ID, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy, Int32 StoreID)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.Insert_Update_AssignPageRightsForAdmin(MainAdminID, CompareAdminID, ID, InnerRightsID, IsListed, CreatedBy, StoreID);
        }


        public DataSet GetAdminAssignPageRightList(Int32 AdminID)
        {
            AdminRightsDAC dac = new AdminRightsDAC();
            return dac.GetAdminAssignPageRightList(AdminID);
        }
    }

    public class AdminEntity
    {
        private int _AdminID;
        private string _FirstName;
        private string _LastName;
        private string _EmailID;
        private string _Rights;
        private string _Templatename;
        public int AdminID
        {
            get { return _AdminID; }
            set { _AdminID = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; }
        }
        public string Rights
        {
            get { return _Rights; }
            set { _Rights = value; }
        }
        public string Templatename
        {
            get { return _Templatename; }
            set { _Templatename = value; }
        }
    }
}
