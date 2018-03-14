using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Entities;
using System.Data.Objects;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// State Component Class Contains State related Business Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class StateComponent
    {

        #region Declarations

        #region List Class for StateList
        /// <summary>
        /// Created dynamic table for Listing State Data
        /// </summary>
        public class StateTable
        {
            public int _StateID;
            public int _CountryID;
            public string _Name;
            public string _abbreviation;
            public bool _Deleted;
            public int _DisplayOrder;
            public int _CreatedBy;
            public DateTime _CreatedOn;
            public int _UpdatedBy;
            public DateTime _UpdatedOn;
            public string _CountryName;

            public int StateID
            {
                get { return _StateID; }
                set { _StateID = value; }
            }
            public int CountryID
            {
                get { return _CountryID; }
                set { _CountryID = value; }
            }
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
            public string Abbreviation
            {
                get { return _abbreviation; }
                set { _abbreviation = value; }
            }
            public bool Deleted
            {
                get { return _Deleted; }
                set { _Deleted = value; }
            }
            public int DisplayOrder
            {
                get { return _DisplayOrder; }
                set { _DisplayOrder = value; }
            }
            public int CreatedBy
            {
                get { return _CreatedBy; }
                set { _CreatedBy = value; }
            }
            public DateTime CreatedOn
            {
                get { return _CreatedOn; }
                set { _CreatedOn = value; }
            }
            public int UpdatedBy
            {
                get { return _UpdatedBy; }
                set { _UpdatedBy = value; }
            }
            public DateTime UpdatedOn
            {
                get { return _UpdatedOn; }
                set { _UpdatedOn = value; }
            }
            public string CountryName
            {
                get { return _CountryName; }
                set { _CountryName = value; }
            }
        }
        #endregion

        #endregion

        #region  Key Functions

        /// <summary>
        /// Function For Create State
        /// </summary>
        /// <param name="state">tb_State state</param>
        /// <returns>Identity value for inserted record</returns>
        public Int32 CreateState(tb_State state)
        {
            Int32 isAdded = 0;
            try
            {
                StateDAC ObjState = new StateDAC();
                if (ObjState.CheckDuplicate(state) == 0)
                {
                    ObjState.Create(state);
                    isAdded = state.StateID;
                }
                else
                {
                    isAdded = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Get All Country List for Fill Country Dropdown
        /// </summary>
        /// <returns> Return a table that contains Country List </returns>
        public List<tb_Country> GetAllCountry()
        {
            StateDAC ObjCountry = new StateDAC();
            List<tb_Country> returnCountryList = ObjCountry.GetAllCountry();
            return returnCountryList;
        }


        /// <summary>
        /// Update Method for State
        /// </summary>
        /// <param name="state">tb_State state</param>
        /// <returns>Returns StateID of current updated record</returns>
        public Int32 UpdateState(tb_State state)
        {
            Int32 isUpdated = 0;
            try
            {
                StateDAC ObjState = new StateDAC();
                if (ObjState.CheckDuplicate(state) == 0)
                {
                    UpdateStates(state);
                    isUpdated = state.StateID;
                }
                else
                {
                    isUpdated = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }


        /// <summary>
        /// Get State Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return a state record for edit in table form</returns>
        public tb_State getState(int Id)
        {
            StateDAC ObjState = new StateDAC();
            return ObjState.getState(Id);
        }

        /// <summary>
        /// Sub Method for Update State
        /// </summary>
        /// <param name="state">tb_State state</param>
        /// <returns>tb_State table</returns>
        private tb_State UpdateStates(tb_State state)
        {

            StateDAC ObjState = new StateDAC();
            ObjState.Update(state);

            return state;
        }


        /// <summary>
        /// For Delete State By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void delState(int Id)
        {
            StateDAC ObjState = new StateDAC();
            ObjState.Delete(Id);
        }

        /// <summary>
        /// Method Data for Searching and Filtering functionality
        /// </summary>
        /// <param name="realIndex">int realIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="Filter">int Filter</param>
        /// <param name="ObjParameter">ObjectParameter ObjParameter</param>
        /// <returns>filter state Data for display.</returns>
        public List<tb_State> GetDataByFilter(int realIndex, int pageSize, string sortBy, string Filter, ObjectParameter ObjParameter)
        {
            List<tb_State> objState = new List<tb_State>();
            RedTag_CCTV_Ecomm_DBEntities objStateEntity = new RedTag_CCTV_Ecomm_DBEntities();
            // objState = objStateEntity.usp_State(realIndex, pageSize, sortBy, Filter, ObjParameter).ToList();
            return objState;
        }


        // For Client side Functions
        /// <summary>
        /// <param name="CountryID">Int32 CountryID</param>
        /// Get All States
        /// </summary>
        /// <returns>Returns List of All States as a Dataset</returns>
        public DataSet GetAllState(Int32 CountryID)
        {
            StateDAC objState = new StateDAC();
            DataSet DSState = new DataSet();
            DSState = objState.GetAllStates(CountryID);
            return DSState;
        }

        /// <summary>
        /// Get Two letter State code By Name
        /// </summary>
        /// <param name="CountryName">string StateName</param>
        /// <returns>Returns string State Code</returns>
        public string GetStateCodeByName(string StateName)
        {
            StateDAC objState = new StateDAC();
            return Convert.ToString(objState.GetStateCodeByName(StateName));
        }

        #endregion

        #region Grid Function

        /// <summary>
        /// Variable And Properties
        /// </summary>
        #region Variable And Properties

        private static int _count;
        private static bool _newFilter = false;
        private static string _Filter = "";
        private static int _StateId = 0;

        public static bool NewFilter
        {
            get { return _newFilter; }
            set { _newFilter = value; }
        }

        public static string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public static int StateID
        {
            get { return _StateId; }
            set { _StateId = value; }
        }

        private static bool _afterDelete = false;

        /// <summary>
        /// Gets or sets the after delete flag.
        /// </summary>
        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }
        #endregion

        /// <summary>
        ///  Methods for Searching and Filtering functionality
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="CName">int CName</param>
        /// <returns>Returns filtered state table</returns>
        public static List<StateTable> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName)
        {
            int realIndex = (_newFilter ? 0 : startIndex);
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            List<StateTable> objState = new List<StateTable>();

            if (Convert.ToString(Filter) != "")
            {
                // For Filter Parameter is not null or blank
                var GetStateList = (from statelist in ctx.tb_State
                                    from countrylist in ctx.tb_Country
                                    where countrylist.CountryID == statelist.CountryID
                                    && statelist.Name.Contains(Filter)
                                    && ((System.Boolean?)statelist.Deleted ?? false) == false

                                    select new StateTable
                                    {
                                        StateID = statelist.StateID,
                                        Name = statelist.Name,
                                        CountryID = statelist.CountryID ?? 0,
                                        CountryName = countrylist.Name,
                                        Abbreviation = statelist.Abbreviation,
                                        DisplayOrder = statelist.DisplayOrder ?? 999,
                                        Deleted = statelist.Deleted ?? false,
                                        CreatedBy = statelist.CreatedBy ?? 0,
                                        CreatedOn = statelist.CreatedOn ?? DateTime.Now,
                                        UpdatedBy = statelist.UpdatedBy ?? 0,
                                        UpdatedOn = statelist.UpdatedOn ?? DateTime.Now
                                    }).OrderBy(s => s.Name);
                objState = GetStateList.ToList<StateTable>();
            }
            else
            {
                // For Filter Parameter is  null or blank
                var GetStateList = (from statelist in ctx.tb_State
                                    from countrylist in ctx.tb_Country
                                    where countrylist.CountryID == statelist.CountryID
                                    && ((System.Boolean?)statelist.Deleted ?? false) == false
                                    select new StateTable
                                    {
                                        StateID = statelist.StateID,
                                        Name = statelist.Name,
                                        CountryID = statelist.CountryID ?? 0,
                                        CountryName = countrylist.Name,
                                        Abbreviation = statelist.Abbreviation,
                                        DisplayOrder = statelist.DisplayOrder ?? 999,
                                        Deleted = statelist.Deleted ?? false,
                                        CreatedBy = statelist.CreatedBy ?? 0,
                                        CreatedOn = statelist.CreatedOn ?? DateTime.Now,
                                        UpdatedBy = statelist.UpdatedBy ?? 0,
                                        UpdatedOn = statelist.UpdatedOn ?? DateTime.Now
                                    }).OrderBy(s => s.Name);
                objState = GetStateList.ToList<StateTable>();
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    objState = objState.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<StateTable>();
                }
                else if (SortingOption.Length == 2)
                {
                    objState = objState.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<StateTable>();
                }
            }
            objState = objState.Skip(startIndex).Take(pageSize).ToList<StateTable>();
            return objState;
        }
        
        /// <summary>
        /// For getting Parameter value form passed parameter
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Property Value</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get total number of record of state table
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <returns>Return count of rows for state table as int</returns>
        public static int GetCount(string CName)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;


            if (Convert.ToString(Filter) != "")
            {
                int GetCnt = (from statelist in ctx.tb_State
                              from countrylist in ctx.tb_Country
                              where countrylist.CountryID == statelist.CountryID
                              && statelist.Name.Contains(Filter)
                              && ((System.Boolean?)statelist.Deleted ?? false) == false
                              select statelist).Count();
                return GetCnt;
            }
            else
            {
                var GetCnt = (from statelist in ctx.tb_State
                              from countrylist in ctx.tb_Country
                              where countrylist.CountryID == statelist.CountryID
                              && ((System.Boolean?)statelist.Deleted ?? false) == false
                              select statelist).Count();
                return GetCnt;
            }
        }

        #endregion

    }
}
