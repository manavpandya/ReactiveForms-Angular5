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
using System.Collections;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
namespace Solution.Bussines.Components
{
    /// <summary>
    /// Credit Card Component Class Call Method from Credit card Data Class
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class CreditCardComponent
    {
        #region Declaration

        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private static int _count;
        public static bool _afterDelete = false;
        CreditCardDAC creditdac = new CreditCardDAC();
        List<CreditcardComponentEntity> lstcredit = new List<CreditcardComponentEntity>();
        public Int32 StoreID = 0;

        #endregion

        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public CreditCardComponent()
        {
            StoreID = AppConfig.StoreID;
        }

        #endregion

        #region  Key Function
        /// <summary>
        /// Get All Card Type By StoreID
        /// </summary>
        /// <param name="StoreID">int32 StoreID</param>
        /// <returns>Returns all Card Type by DataSet</returns>
        public DataSet GetAllCarTypeByStoreID(int StoreID)
        {
            CreditCardDAC objCreditcard = new CreditCardDAC();
            return objCreditcard.GetAllCarTypeByStoreID(StoreID);
        }

        /// <summary>
        /// Get Address For Customer By CustomerId & AddressType
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="AddressType">Int32 AddressType</param>
        /// <returns>Returns Address</returns>
        public DataSet GetAddressForByCustId_AddType(Int32 CustomerID, Int32 AddressType)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            DataSet DSAddress = new DataSet();
            DSAddress = objCreditCard.GetAddressForByCustId_AddType(CustomerID, AddressType);
            return DSAddress;
        }

        /// <summary>
        /// Get Credit Card Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerI</param>
        /// <returns>Returns Credit Card Details</returns>
        public DataSet GetCreditCardDetailByCustomerID(Int32 CustomerID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            DataSet DSCreditCard = new DataSet();
            DSCreditCard = objCreditCard.GetCreditCardDetailByCustomerID(CustomerID, StoreID);
            return DSCreditCard;
        }

        /// <summary>
        /// Get Credit Card Detail By CardID
        /// </summary>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns Credit Card Details</returns>
        public DataSet GetCreditCardDetailByCardID(Int32 CardID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            DataSet DSCreditCardDetail = new DataSet();
            DSCreditCardDetail = objCreditCard.GetCreditCardDetailByCardID(CardID);
            return DSCreditCardDetail;
        }

        /// <summary>
        /// Check IsDefault Credit Card for Customer or not
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns true or false</returns>
        public bool CheckIsDefaultCreditCard(Int32 CustomerID, Int32 CardID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            return objCreditCard.CheckIsDefaultCreditCard(CustomerID, CardID);
        }

        /// <summary>
        /// Delete Credit Card Detail By CardID
        /// </summary>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns 1 = Deleted or -1 = Not Deleted</returns>
        public Int32 DeleteCreditCardDetail(Int32 CardID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            return objCreditCard.DeleteCreditCardDetail(CardID);
        }

        /// <summary>
        /// Check Credit Card Is Exists or not in tb_CreditCardDetails
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="CardNumber">String CardNumber</param>
        /// <param name="CardTypeID">Int32 CardTypeID</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns True or False</returns>
        public bool CheckCreditCardIsExists(Int32 CustomerID, String CardNumber, Int32 CardTypeID, Int32 CardID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            return objCreditCard.CheckCreditCardIsExists(CustomerID, CardNumber, CardTypeID, CardID);
        }

        /// <summary>
        /// Insert Credit Card Details
        /// </summary>
        /// <param name="tb_CreditCardDetails">tb_CreditCardDetails tb_CreditCardDetails</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 InsertCreditCardDetails(tb_CreditCardDetails tb_CreditCardDetails, Int32 CustomerID)
        {
            int isAdded = 0;
            try
            {
                CreditCardDAC objCreditCard = new CreditCardDAC();
                isAdded = objCreditCard.InsertCreditCardDetails(tb_CreditCardDetails, CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (isAdded <= 0)
            {
                isAdded = -1;
            }
            return isAdded;
        }

        /// <summary>
        /// Update Customer CreditCardID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns True or False</returns>
        public bool UpdateCustomerCreditCardID(Int32 CustomerID, Int32 CardID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            return objCreditCard.UpdateCustomerCreditCardID(CustomerID, CardID);
        }

        /// <summary>
        /// Update CreditCard Detail
        /// </summary>
        /// <param name="tb_CreditCardDetails">tb_CreditCardDetails tb_CreditCardDetails</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCreditCardDetil(tb_CreditCardDetails tb_CreditCardDetails, Int32 CardID)
        {
            int isUpdated = 0;
            try
            {
                CreditCardDAC objCreditCard = new CreditCardDAC();
                isUpdated = objCreditCard.UpdateCreditCardDetil(tb_CreditCardDetails, CardID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (isUpdated <= 0)
            {
                isUpdated = -1;
            }
            return isUpdated;
        }

        /// <summary>
        /// Get Card Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns All Card Details</returns>
        public DataSet GetCardDetailByCustomerId(Int32 CustomerID)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            DataSet DSCardDetail = new DataSet();
            DSCardDetail = objCreditCard.GetCardDetailByCustomerId(CustomerID);
            return DSCardDetail;
        }


        /// <summary>
        /// Set One Click Default Values
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>Returns 1 = true</returns>
        public bool SetOneClickDefaultValues(tb_Customer tb_Customer)
        {
            CreditCardDAC objCreditCard = new CreditCardDAC();
            return objCreditCard.SetOneClickDefaultValues(tb_Customer);
        }

        /// <summary>
        /// Get the Data Source for Grid view after searching,sorting and on first time load
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns IQueryable</returns>      
        public IQueryable<CreditcardComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            IQueryable<CreditcardComponentEntity> results = from a in ctx.tb_CreditCardTypes
                                                            where a.Deleted.Value == false
                                                            select new CreditcardComponentEntity
                                                            {
                                                                CardTypeID = a.CardTypeID,
                                                                CardType = a.CardType,
                                                                Active = a.Active.Value,
                                                                StoreName = a.tb_Store.StoreName,
                                                                StoreID = a.tb_Store.StoreID
                                                            };
            if (pSearchValue != null)
            {
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.CardType.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.Where(a => a.CardType.Contains(pSearchValue.Trim())).AsQueryable();
                }
            }
            else
            {
                pSearchValue = "";
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.CardType.Contains(pSearchValue) && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.OrderBy(o => o.CardType);
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstcredit.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
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
                results = results.OrderBy(o => o.CardType);
            }
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results;
        }

        /// <summary>
        /// Get Property Value
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
        /// Get Total Count
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Total count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Get all values for specific credit card type
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns table tb_CreditCardTypes</returns>
        public tb_CreditCardTypes GetcreditcardType(int Id)
        {
            return creditdac.GetcreditcardType(Id);
        }

        /// <summary>
        /// Function for deleting credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns integer</returns>
        public Int32 Deletecredittypetype(tb_CreditCardTypes credit)
        {
            Int32 isUpdated = 0;
            try
            {
                creditdac.Update(credit);
                isUpdated = credit.CardTypeID;
                _afterDelete = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Function for updating credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns integer</returns>
        public Int32 Updatecredittypetype(tb_CreditCardTypes credit)
        {
            Int32 isUpdated = 0;
            try
            {
                creditdac.Update(credit);
                isUpdated = credit.CardTypeID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Function for creating Credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns Integer</returns>
        public Int32 Createcreditcardtype(tb_CreditCardTypes credit)
        {
            Int32 isAdded = 0;
            try
            {
                creditdac.Createcreditcardtype(credit);
                isAdded = credit.CardTypeID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Function for checking duplicate credit card type
        /// </summary>
        /// <param name="tblpay">tb_CreditCardTypes credit</param>
        /// <returns>Returns integer</returns>
        public bool CheckDuplicate(tb_CreditCardTypes credit)
        {
            if (creditdac.CheckDuplicate(credit) == 0)
            {
                return false;
            }
            else
                return true;
        }

        #endregion
    }

    /// <summary>
    /// Credit card Component Entity variant and Properties
    /// </summary>
    public class CreditcardComponentEntity
    {
        private int _CardTypeID;

        public int CardTypeID
        {
            get { return _CardTypeID; }
            set { _CardTypeID = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private string _CardType;

        public string CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }

        private string _Discription;

        public string Discription
        {
            get { return _Discription; }
            set { _Discription = value; }
        }

        private int _CreatedBy;

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private int _UpdatedBy;

        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }

        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        private DateTime _UpdatedOn;

        public DateTime UpdatedOn
        {
            get { return _UpdatedOn; }
            set { _UpdatedOn = value; }
        }

        public bool _Active;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        public bool _Deleted;

        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

    }

}
