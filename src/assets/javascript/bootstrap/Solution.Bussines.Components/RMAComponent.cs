using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// RMA Component Class Contains Return Merchandise related Business Logic Functions 
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 

    public class RMAComponent
    {
        #region Declaration
        int _count;
        #endregion

        #region Key Function

        private int _ReturnItemID;
        private int _OrderedCustomerID;
        private int _OrderedNumber;
        private String _CustomerName;
        private int _CustomerEMail;
        private DateTime _OrderDate;
        private int _ProductID;
        private int _Quantity;
        private String _ReturnReason;
        private String _AdditionalInformation;
        private bool _Deleted;
        private DateTime _CreatedOn;
        private bool _isArrived;
        private String _ReturnNotes;
        private String _ReturnImages;
        private String _ReturnFee;
        private bool _IsReturn;
        private String _ReturnType;
        private String _RetunitemNotes;
        private bool _isReturnrequest;
        private int _MailLogID;

        public int ReturnItemID { get { return _ReturnItemID; } set { _ReturnItemID = value; } }
        public int OrderedCustomerID { get { return _OrderedCustomerID; } set { _OrderedCustomerID = value; } }
        public int OrderedNumber { get { return _OrderedNumber; } set { _OrderedNumber = value; } }
        public String CustomerName { get { return _CustomerName; } set { _CustomerName = value; } }
        public int CustomerEMail { get { return _CustomerEMail; } set { _CustomerEMail = value; } }
        public DateTime OrderDate { get { return _OrderDate; } set { _OrderDate = value; } }
        public int ProductID { get { return _ProductID; } set { _ProductID = value; } }
        public int Quantity { get { return _Quantity; } set { _Quantity = value; } }
        public String ReturnReason { get { return _ReturnReason; } set { _ReturnReason = value; } }
        public String AdditionalInformation { get { return _AdditionalInformation; } set { _AdditionalInformation = value; } }
        public bool Deleted { get { return _Deleted; } set { _Deleted = value; } }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public bool isArrived { get { return _isArrived; } set { _isArrived = value; } }
        public String ReturnNotes { get { return _ReturnNotes; } set { _ReturnNotes = value; } }
        public String ReturnImages { get { return _ReturnImages; } set { _ReturnImages = value; } }
        public String ReturnFee { get { return _ReturnFee; } set { _ReturnFee = value; } }
        public bool IsReturn { get { return _IsReturn; } set { _IsReturn = value; } }
        public String ReturnType { get { return _ReturnType; } set { _ReturnType = value; } }
        public String RetunitemNotes { get { return _RetunitemNotes; } set { _RetunitemNotes = value; } }
        public bool isReturnrequest { get { return _isReturnrequest; } set { _isReturnrequest = value; } }
        public int MailLogID { get { return _MailLogID; } set { _MailLogID = value; } }



        #endregion

        /// <summary>
        /// Get RMA Details by OrderedCustomerID 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Returns the RMA list by CustomerID</returns>
        public DataSet GetRetrunItemByID(int ID)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetRMAListByID(ID);
            return DSRMA;
        }

        /// <summary>
        /// Get Ordered Shopping Cart Items Form Return Item
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns the List of Shopping Cart Items for RMA</returns>
        public DataSet GetOrderedShoppingCartItemsFormRetrunItem(int ID, string ProductID)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetOrderedShoppingCartItemsFormRetrunItem(ID, ProductID);
            return DSRMA;
        }

        /// <summary>
        /// Get RMA Product By RMAID
        /// </summary>
        /// <param name="ReturnItemID">int ReturnItemID</param>
        /// <returns>Returns the list of products by ReturnItemID</returns>
        public DataSet GetRMAProductByRMAID(int ReturnItemID)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetRMAProductByRMAID(ReturnItemID);
            return DSRMA;
        }

        /// <summary>
        /// Get All Return Item
        /// </summary>
        /// <param name="StrID">String StrID</param>
        /// <param name="searchstr">String searchstr</param>
        /// <returns>Returns all return items list</returns>
        public DataSet GetAllReturnItem(String StrID, String searchstr)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetAllReturnItem(StrID, searchstr);
            return DSRMA;
        }

        /// <summary>
        /// Get All Return Item
        /// </summary>
        /// <param name="searchstr">String searchstr</param>
        /// <returns>Returns all return items list</returns>
        public DataSet GetAllReturnItem(String searchstr)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetAllReturnItem(searchstr);
            return DSRMA;
        }
        /// <summary>
        /// Get Ordered Shopping Cart Items Form Return Item
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns the List of Shopping Cart Items for RMA</returns>
        public DataSet GetOrderedShoppingCartItemsFormRetrunItem(int ID, string ProductID, int OrderedCustomCartID)
        {
            DataSet DSRMA = new DataSet();
            RMADAC dac = new RMADAC();
            DSRMA = dac.GetOrderedShoppingCartItemsFormRetrunItem(ID, ProductID, OrderedCustomCartID);
            return DSRMA;
        }
    }
}
