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
    /// Country Component Class Contains Country related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CountryComponent
    {
        #region  Key Functions

        /// <summary>
        /// To Create New Country 
        /// </summary>
        /// <param name="country">tb_Country country</param>
        /// <returns>Returns value of current inserted record</returns>
        public Int32 CreateCountry(tb_Country country)
        {
            Int32 isAdded = 0;
            try
            {
                CountryDAC objCountry = new CountryDAC();
                if (objCountry.CheckDuplicate(country) == 0)
                {
                    objCountry.Create(country);
                    isAdded = country.CountryID;
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
        /// Update Method for Country
        /// </summary>
        /// <param name="country">tb_Country country</param>
        /// <returns>Returns CountryID of currently updated record</returns>
        public Int32 UpdateCountry(tb_Country country)
        {
            Int32 isUpdated = 0;
            try
            {
                CountryDAC objCountry = new CountryDAC();
                if (objCountry.CheckDuplicate(country) == 0)
                {
                    UpdateCountries(country);
                    isUpdated = country.CountryID;
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
        ///  Get Country Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return a country record for edit in table form</returns>
        public tb_Country getCountry(int Id)
        {
            CountryDAC objCountry = new CountryDAC();
            return objCountry.getCountry(Id);
        }

        /// <summary>
        /// Update Method for Country
        /// </summary>
        /// <param name="country">tb_Country country</param>
        /// <returns>Returns CountryID of currently updated record</returns>
        private tb_Country UpdateCountries(tb_Country country)
        {
            CountryDAC objCountry = new CountryDAC();
            objCountry.Update(country);
            return country;
        }

        /// <summary>
        ///  For Delete Country By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void delCountry(int Id)
        {
            CountryDAC objCountry = new CountryDAC();
            objCountry.Delete(Id);
        }

        /// <summary>
        ///  Method for Searching and Filtering functionality
        /// </summary>
        /// <param name="realIndex">int realIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="Filter">int Filter</param>
        /// <param name="ObjParameter">ObjectParameter ObjParameter</param>
        /// <returns>Returns Filtered Data</returns>
        public List<tb_Country> GetDataByFilter(int realIndex, int pageSize, string sortBy, string Filter, ObjectParameter ObjParameter)
        {
            List<tb_Country> objCountry = new List<tb_Country>();
            RedTag_CCTV_Ecomm_DBEntities objCountryEntity = new RedTag_CCTV_Ecomm_DBEntities();
            objCountry = objCountryEntity.usp_Country(realIndex, pageSize, sortBy, Filter, ObjParameter).ToList<tb_Country>();
            return objCountry;
        }

        //Client Side Functions
        /// <summary>
        /// Get Data of All Countries
        /// </summary>
        /// <returns>Returns All Countries Details</returns>
        public DataSet GetAllCountries()
        {
            CountryDAC objCountry = new CountryDAC();
            DataSet DSCountry = new DataSet();
            DSCountry = objCountry.GetAllCountries();
            return DSCountry;
        }

        #endregion

        #region Grid Function

        #region Variable And Properties
        private static int _count;
        private static bool _newFilter = false;
        private static string _Filter = "";

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

        private static int _CountryID = 0;
        public static int CountryID
        {
            get { return _CountryID; }
            set { _CountryID = value; }
        }

        private static bool _afterDelete = false;


        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }
        #endregion

        /// <summary>
        ///  Method for Searching and Filtering functionality
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="CName">string CName</param>
        /// <returns>Returns filtered Country table</returns>
        public static List<tb_Country> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName)
        {
            if (string.IsNullOrEmpty(sortBy))
                sortBy = CName + " Asc";
            else if (sortBy.EndsWith(" DESC"))
            {
                sortBy = sortBy.Replace(",", " DESC,");
            }
            ObjectParameter countParameter = new ObjectParameter("count", typeof(int));
            List<tb_Country> objCountry = new List<tb_Country>();
            CountryComponent obj = new CountryComponent();
            objCountry = obj.GetDataByFilter(startIndex, pageSize, sortBy, Filter, countParameter).ToList();
            _count = (int)countParameter.Value;
            return objCountry;
        }

        /// <summary>
        /// Get total number of record of state table
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <returns>Returns count of rows from state table as int</returns>
        public static int GetCount(string CName)
        {
            return (_afterDelete ? _count : _count);
        }

        /// <summary>
        /// Get Two letter Country Code by name
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns Country Code</returns>
        public string GetCountryCodeByName(string CountryName)
        {
            CountryDAC objCountry = new CountryDAC();
            return Convert.ToString(objCountry.GetCountryCodeByName(CountryName));
        }

        /// <summary>
        /// Get Three letter Country Code by name
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns Three Letter ISO Code for Country</returns>
        public string GetCountryThreeLetterISOCodeByName(string CountryName)
        {
            CountryDAC objCountry = new CountryDAC();
            return Convert.ToString(objCountry.GetCountryThreeLetterISOCodeByName(CountryName));
        }

        /// <summary>
        /// Get Two letter Country Code by name For Shipping Label
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns string Country Code</returns>
        public string GetCountryCodeByNameForShippingLabel(string CountryName)
        {
            CountryDAC objCountry = new CountryDAC();
            return Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(CountryName));
        }

        /// <summary>
        /// Get Country name for shipping label
        /// </summary>
        /// <param name="CountryID">Int32 CountryID</param>
        /// <returns>Returns Country name for shipping label</returns>
        public string GetCountryNameByCodeForShippingLabel(Int32 CountryID)
        {
            CountryDAC objCountry = new CountryDAC();
            return Convert.ToString(objCountry.GetCountryNameByCodeForShippingLabel(CountryID));
        }
        #endregion
    }
}
