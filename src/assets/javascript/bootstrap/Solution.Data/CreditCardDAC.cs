using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// CreditCard Data Access Class Contains Credit Card related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class CreditCardDAC
    {
        #region Declaration
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions
        /// <summary>
        /// Get All Card Type By StoreID
        /// </summary>
        /// <param name="StoreID">int32 StoreID</param>
        /// <returns>Returns all Card Type by DataSet</returns>
        public DataSet GetAllCarTypeByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType_GetAllCardTypeByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Address For Customer By CustomerId & AddressType
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="AddressType">Int32 AddressType</param>
        /// <returns>Returns Address</returns>
        public DataSet GetAddressForByCustId_AddType(Int32 CustomerID, Int32 AddressType)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@AddressType", AddressType);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Credit Card Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Credit Card Details</returns>
        public DataSet GetCreditCardDetailByCustomerID(Int32 CustomerID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Credit Card Detail By CardID
        /// </summary>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns Credit Card Details</returns>
        public DataSet GetCreditCardDetailByCardID(Int32 CardID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Check IsDefault Credit Card for Customer or not
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns true or false</returns>
        public bool CheckIsDefaultCreditCard(Int32 CustomerID, Int32 CardID)
        {
            objSql = new SQLAccess();
            bool isExist = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            int cnt = Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
            if (cnt > 0)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// Delete Credit Card Detail By CardID
        /// </summary>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns 1 = Deleted or -1 = Not Deleted</returns>
        public Int32 DeleteCreditCardDetail(Int32 CardID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@Mode", 5);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
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
            objSql = new SQLAccess();
            bool isExist = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CardNumber", CardNumber);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@CardTypeID", CardTypeID);
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@Mode", 6);
            int cnt = Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
            if (cnt > 0)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// Insert Credit Card Details
        /// </summary>
        /// <param name="tb_CreditCardDetails">tb_CreditCardDetails tb_CreditCardDetails</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 InsertCreditCardDetails(tb_CreditCardDetails tb_CreditCardDetails, Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType_Insert";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@NameOnCard", tb_CreditCardDetails.NameOnCard);
            cmd.Parameters.AddWithValue("@CardNumber", tb_CreditCardDetails.CardNumber);
            cmd.Parameters.AddWithValue("@CardTypeID", tb_CreditCardDetails.CardTypeID);
            cmd.Parameters.AddWithValue("@CardVerificationCode", tb_CreditCardDetails.CardVerificationCode);
            cmd.Parameters.AddWithValue("@CardExpirationMonth", tb_CreditCardDetails.CardExpirationMonth);
            cmd.Parameters.AddWithValue("@CardExpirationYear", tb_CreditCardDetails.CardExpirationYear);
            cmd.Parameters.AddWithValue("@DefaultBillingAddressID", tb_CreditCardDetails.DefaultBillingAddressID);
            cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@Deleted", false);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update CreditCard Detail
        /// </summary>
        /// <param name="tb_CreditCardDetails">tb_CreditCardDetails tb_CreditCardDetails</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCreditCardDetil(tb_CreditCardDetails tb_CreditCardDetails, Int32 CardID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType_Insert";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@NameOnCard", tb_CreditCardDetails.NameOnCard);
            cmd.Parameters.AddWithValue("@CardNumber", tb_CreditCardDetails.CardNumber);
            cmd.Parameters.AddWithValue("@CardTypeID", tb_CreditCardDetails.CardTypeID);
            cmd.Parameters.AddWithValue("@CardVerificationCode", tb_CreditCardDetails.CardVerificationCode);
            cmd.Parameters.AddWithValue("@CardExpirationMonth", tb_CreditCardDetails.CardExpirationMonth);
            cmd.Parameters.AddWithValue("@CardExpirationYear", tb_CreditCardDetails.CardExpirationYear);
            cmd.Parameters.AddWithValue("@DefaultBillingAddressID", tb_CreditCardDetails.DefaultBillingAddressID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Customer CreditCardID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="CardID">Int32 CardID</param>
        /// <returns>Returns True or False</returns>
        public bool UpdateCustomerCreditCardID(Int32 CustomerID, Int32 CardID)
        {
            objSql = new SQLAccess();
            bool isUpdate = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@CardID", CardID);
            cmd.Parameters.AddWithValue("@Mode", 7);
            objSql.ExecuteNonQuery(cmd);
            int cnt = Convert.ToInt32(paramReturnval.Value);
            if (cnt == 1)
            {
                isUpdate = true;
            }
            return isUpdate;
        }

        /// <summary>
        /// Get Card Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns All Card Details</returns>
        public DataSet GetCardDetailByCustomerId(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 8);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Set One Click Default Values
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>returns 1 = true</returns>
        public bool SetOneClickDefaultValues(tb_Customer tb_Customer)
        {
            objSql = new SQLAccess();
            bool isUpdate = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CreditcardType";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", tb_Customer.CustomerID);
            cmd.Parameters.AddWithValue("@BillingAddressID", tb_Customer.BillingAddressID);
            cmd.Parameters.AddWithValue("@ShippingAddressID", tb_Customer.ShippingAddressID);
            cmd.Parameters.AddWithValue("@CardID", tb_Customer.CreditCardID);
            cmd.Parameters.AddWithValue("@Mode", 9);
            objSql.ExecuteNonQuery(cmd);
            int cnt = Convert.ToInt32(paramReturnval.Value);
            if (cnt == 1)
            {
                isUpdate = true;
            }
            return isUpdate;
        }

        /// <summary>
        /// Get the detail of Credit card
        /// </summary>
        /// <param name="Id">credit card id</param>
        /// <returns>Returns table tb_CreditCardTypes</returns>
        public tb_CreditCardTypes GetcreditcardType(int Id)
        {
            tb_CreditCardTypes credit = null;
            {
                credit = ctx.tb_CreditCardTypes.First(e => e.CardTypeID == Id);
            }
            return credit;
        }

        /// <summary>
        /// Function for updating credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns integer</returns>
        public void Update(tb_CreditCardTypes credit)
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Function for creating Credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns Integer</returns>
        public tb_CreditCardTypes Createcreditcardtype(tb_CreditCardTypes credit)
        {
            try
            {
                ctx.AddTotb_CreditCardTypes(credit);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return credit;
        }

        /// <summary>
        /// Function for checking duplicate credit card type
        /// </summary>
        /// <param name="credit">tb_CreditCardTypes credit</param>
        /// <returns>Returns Integer</returns>
        public Int32 CheckDuplicate(tb_CreditCardTypes credit)
        {
            Int32 isExists = 0;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(credit.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_CreditCardTypes
                          select new
                          {
                              creditcard = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.creditcard.CardType == credit.CardType
                        && a.creditcard.Deleted == false
                        && a.creditcard.CardTypeID != credit.CardTypeID
                        && a.store.StoreID == ID
                        select new { a.creditcard.CardTypeID }).Count();
            return isExists;
        }
        #endregion
    }
}
