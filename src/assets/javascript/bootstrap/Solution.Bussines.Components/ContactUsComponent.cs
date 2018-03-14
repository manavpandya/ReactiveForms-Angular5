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
    /// Contact Us Component Class Contains Contact Us Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ContactUsComponent
    {
        #region Declaration

        public Int32 StoreID = 0;
        ContactUsDAC objContactus = null;
        #endregion

        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public ContactUsComponent()
        {
            StoreID = 1;// AppConfig.StoreID;
        }

        #endregion

        #region  Key Function
        /// <summary>
        /// Insert Contact Us Details into table
        /// </summary>
        /// <param name="tb_ContactUs">tb_ContactUs tb_ContactUs</param>
        /// <returns>Returns New Generated Row Id from tb_ContactUs table</returns>
        public Int32 InsertContactUsDetail(tb_ContactUs tb_ContactUs)
        {
            int isAdded = 0;
            try
            {
                objContactus = new ContactUsDAC();
                isAdded = objContactus.InsertContactUsDetail(tb_ContactUs);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }
        #endregion
    }
}
