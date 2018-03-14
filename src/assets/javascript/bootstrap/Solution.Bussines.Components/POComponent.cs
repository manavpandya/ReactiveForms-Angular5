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
    /// PO Component Class Contains Purchase Order related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class POComponent
    {

        public POComponent()
        {

        }

        #region Key Functions

       
        /// <summary>
        /// Get PO Shipping Status
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns Shipping Status in Sting</returns>
        public String GetPOShippingStatus(Int32 OrderNumber)
        {
            PODAC dac = new PODAC();
            String strShippingStatus = String.Empty;
            strShippingStatus = dac.GetPOShippingStatus(OrderNumber);
            return strShippingStatus;
        }

        #endregion

    }
}
