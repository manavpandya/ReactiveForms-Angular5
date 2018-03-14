using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Bussines.Components.Common;


namespace Solution.Bussines.Components
{
    public class ImportReplenishmentComponent
    {

        #region Declaration

        public Int32 StoreID = 0;
        ImportReplenishmentDAC objRepLog = null;
        #endregion

        public ImportReplenishmentComponent()
        {


        }
        /// <summary>
        /// Insert Replenishment File Detail
        /// </summary>
        /// <param name="FileName">String FileName</param>
        /// <param name="CreatedBy">Int32 CreatedBy</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Returns Identity Values</returns>
        public Int32 InsertReplenishmentFileLOg(String FileName,Int32 CreatedBy,Int32 Mode)
        {
            objRepLog = new ImportReplenishmentDAC();
            return objRepLog.InsertReplenishmentFileLOg(FileName, CreatedBy,Mode);
        }

        /// <summary>
        /// Update Replenishment File Name
        /// </summary>
        /// <param name="FileName">String FileName</param>
        /// <param name="FileID">Int32 FileID</param>
        /// <param name="Mode">Int32 Mode</param>
        public Int32 UpdateReplenishmentFileName(String FileName, Int32 FileID, Int32 Mode)
        {
            objRepLog = new ImportReplenishmentDAC();
            return objRepLog.UpdateReplenishmentFileName(FileName, FileID, Mode);
        }

        /// <summary>
        /// get last updated Replenishment File data
        /// </summary>
        
        public  DataSet GetLastFileModified()
        {   DataSet DSCommon = new DataSet();
           objRepLog = new ImportReplenishmentDAC();
            DSCommon = objRepLog.GetLastFileModified();
            return DSCommon;
           
            
        }

    }
}
