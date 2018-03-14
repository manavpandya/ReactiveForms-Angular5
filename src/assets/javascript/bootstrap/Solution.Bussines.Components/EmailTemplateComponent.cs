using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using Solution.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Email Template Component Class Contains Email Template related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public class EmailTemplateComponent
    {
        #region Variabel Declaration
        private static int _count;
        #endregion

        #region Key Functions

        /// <summary>
        /// Get Email Template Entity List
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="pSearchBy">string SearchBy</param>
        /// <param name="pSearchValue">string SearchValue</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Email Template Entity List</returns>
        public IQueryable<EmailTemplateEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string SearchBy, string SearchValue, int StoreID)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                IQueryable<EmailTemplateEntity> results = from a in ctx.tb_EmailTemplate
                                                          from b in ctx.tb_Store
                                                          where a.tb_Store.StoreID == b.StoreID
                                                          && ((System.Boolean?)a.tb_Store.Deleted ?? false) == false
                                                          select new EmailTemplateEntity
                                                          {
                                                              TemplateID = a.TemplateID,
                                                              Label = a.Label,
                                                              Subject = a.Subject,
                                                              StoreID = a.tb_Store.StoreID,
                                                              StoreName = a.tb_Store.StoreName,
                                                              EmailTo = a.EmailTo,
                                                              EmailCC = a.EmailCC
                                                          };

                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }
                if (StoreID != -1)
                {
                    if (SearchBy == "Label")
                    {
                        //search by Label
                        results = results.Where(a => a.Label.Contains(SearchValue) && a.StoreID == StoreID).AsQueryable();
                    }
                    else if (SearchBy == "Subject")
                    {
                        //Search by Subject
                        results = results.Where(a => a.Subject.Contains(SearchValue) && a.StoreID == StoreID).AsQueryable();
                    }
                }
                else
                {
                    if (SearchBy == "Label")
                    {
                        //search by Label
                        results = results.Where(a => a.Label.Contains(SearchValue)).AsQueryable();
                    }
                    else if (SearchBy == "Subject")
                    {
                        //Search by Subject
                        results = results.Where(a => a.Subject.Contains(SearchValue)).AsQueryable();
                    }
                }

                _count = results.Count();
                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {
                    System.Reflection.PropertyInfo property = results.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), true);
                    }
                    else if (SortingOption.Length == 2)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), false);
                    }
                }
                else
                {
                    //Default sorting by Label
                    results = results.OrderBy(o => o.Label);
                }
                results = results.Skip(startIndex).Take(pageSize);
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get Total number of records used for paging
        /// </summary>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="StoreID">int StoreID</param>        
        /// <returns>Returns Total number of records</returns>
        public static int GetCount(string SearchBy, string SearchValue, int StoreID)
        {
            return _count;
        }

        /// <summary>
        /// Create Email Template
        /// </summary>
        /// <param name="template">tb_EmailTemplate template</param>
        /// <returns>Returns ID of created Email Template</returns>
        public Int32 CreateEmailTemplate(tb_EmailTemplate template)
        {
            Int32 isAdded = 0;
            try
            {
                EmailTemplateDAC dac = new EmailTemplateDAC();
                dac.Create(template);
                isAdded = template.TemplateID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Get Email Template by ID
        /// </summary>
        /// <param name="TemplateID">int TemplateID</param>
        /// <returns>Returns EmailTemplate by List </returns>
        public tb_EmailTemplate GetEmailTemplateByID(int TemplateID)
        {
            EmailTemplateDAC dac = new EmailTemplateDAC();
            return dac.GetEmailTemplateByID(TemplateID);
        }

        /// <summary>
        /// Update Email template
        /// </summary>
        /// <param name="template">tb_EmailTemplate template</param>
        /// <returns>Returns No of Rows Affected</returns>
        public Int32 UpdateEmailTemplate(tb_EmailTemplate template)
        {
            int RowsAffected = 0;
            try
            {
                EmailTemplateDAC dac = new EmailTemplateDAC();
                RowsAffected = dac.Update(template);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Check whether Email Template already exists or not
        /// </summary>
        /// <param name="EmailTemplate">tb_EmailTemplate EmailTemplate</param>
        /// <returns>count to identify whether Email Template already exists or not: count>0 already exists</returns>
        public bool CheckDuplicate(tb_EmailTemplate template)
        {
            EmailTemplateDAC dac = new EmailTemplateDAC();
            if (dac.CheckDuplicate(template) == 0)
            {
                return false;
            }
            else
                return true;
        }
        #endregion
    }

    /// <summary>
    /// Variable And Properties
    /// </summary>
    public class EmailTemplateEntity
    {
        private string _Label;
        private string _Subject;
        private Int32 _StoreID;
        private int _TemplateID;
        private string _StoreName;
        private string _EmailTo;
        private string _EmailCC;
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
        public int TemplateID
        {
            get { return _TemplateID; }
            set { _TemplateID = value; }
        }
        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        public Int32 StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string EmailTo
        {
            get { return _EmailTo; }
            set { _EmailTo = value; }
        }
        public string EmailCC
        {
            get { return _EmailCC; }
            set { _EmailCC = value; }
        }
    }
}
