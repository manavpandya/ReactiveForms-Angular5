using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Footer Component Class Contains Footer related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class FooterComponent
    {
        #region Key Functions
   
        /// <summary>
        /// Get Footer Content List
        /// </summary>
        /// <returns>Return Dataset</returns>
     
        public static DataSet GetFooterContentList()
        {
            FooterContentDAC dac = new FooterContentDAC();
            DataSet DSFooterContent = new DataSet();
            DSFooterContent = dac.GetFooterContentList();
            return DSFooterContent;
        }

        #endregion
    }
}
