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

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Manufacture Component Class Contains Manufacture related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ManufactureComponent
    {
        #region Key Functions

        /// <summary>
        /// Get Manufacture Details
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Dataset</returns>
        public static DataSet GetManufactureByStoreID(Int32 StoreID)
        {
            ManufactureDAC dac = new ManufactureDAC();
            DataSet DSManufacture = new DataSet();
            DSManufacture = dac.GetManufactureByStoreID(StoreID);
            return DSManufacture;
        }

        /// <summary>
        /// Method for save manufacturer data into tb_manufacturer
        /// </summary>
        /// <param name="Manufacturer">tb_Manufacture Manufacturer</param>
        /// <returns>Returns tb_Manufacture table </returns>
        public static tb_Manufacture CreateManufacturer(tb_Manufacture Manufacturer)
        {
            ManufactureDAC dac = new ManufactureDAC();
            return dac.CreateManufacturer(Manufacturer);
        }

        /// <summary>
        /// Method to Get All Manufacture Details
        /// </summary>
        /// <returns>Return Dataset- All Manufacture</returns>

        public static DataSet GetAllManufactureDetail()
        {
            ManufactureDAC dac = new ManufactureDAC();
            return dac.GetAllManufactureDetail();
        }

        /// <summary>
        /// Method to delete manufacture from manufacture table
        /// </summary>
        /// <param name="manufactureid">int manufactureid</param>
        public static void Deletecategory(int manufactureid)
        {
            ManufactureDAC dac = new ManufactureDAC();
            dac.DeleteManufacture(manufactureid);
        }

        /// <summary>
        /// Method to check name is exist or not
        /// </summary>
        /// <param name="ManufactureName">string ManufactureName</param>
        /// <returns>Returns ManufactureID</returns>
        public static int CheckManufactureName(string ManufactureName, Int32 StrStoreId)
        {
            ManufactureDAC dac = new ManufactureDAC();
            return dac.CheckManufactureName(ManufactureName, StrStoreId);
        }

        /// <summary>
        /// Get Manufacture Details
        /// </summary>
        /// <param name="manufactureId">int ManufactureId</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Return Dataset- Manufacture Details</returns>
        public static DataSet GetManufactureByManID(int manufactureId, int StoreID)
        {
            ManufactureDAC dac = new ManufactureDAC();
            return dac.GetManufactureByManID(manufactureId, StoreID);
        }

        /// <summary>
        /// Get Manufacture Details
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Return Dataset- Manufacture Details</returns>
        public static DataSet GetManufactureByName(string Name, int StoreID)
        {
            ManufactureDAC dac = new ManufactureDAC();
            return dac.GetManufactureByName(Name, StoreID);
        }

        /// <summary>
        /// Update Manufacture
        /// </summary>
        /// <param name="Manufacture">tb_Manufacture Table</param>
        public static void Update(tb_Manufacture Manufacture)
        {
            ManufactureDAC dac = new ManufactureDAC();
            dac.Update(Manufacture);
        }

        #endregion
    }
}
