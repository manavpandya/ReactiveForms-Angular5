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
   public class GenerateInventoryFeedComponent
    { 
       
       
       #region Declaration

        public Int32 StoreID = 0;
        GenerateInventoryFeedDAC objInv = null;
        #endregion

       public GenerateInventoryFeedComponent()
        {

        }
       /// <summary>
       /// get Sales channel partner list
       /// </summary>
       public DataSet GetSalesPartnerList()
       {
           DataSet DSCommon = new DataSet();
           objInv = new GenerateInventoryFeedDAC();
           DSCommon = objInv.GetSalesPartnerList();
           return DSCommon;


       }

       /// <summary>
       /// get Sales channel partner Feed Template
       /// </summary>
       public DataSet GetChannelPartnerFeedTemplate(Int32 StoreID)
       {
           DataSet DSCommon = new DataSet();
           objInv = new GenerateInventoryFeedDAC();
           DSCommon = objInv.GetChannelPartnerFeedTemplate(StoreID);
           return DSCommon;
       }



       public void InsertFeedLog(String FileName,Int32 GeneratedBy,Int32 StoreID)
       {
           objInv = new GenerateInventoryFeedDAC();
           objInv.InsertFeedLog(FileName, GeneratedBy, StoreID);
       }

    }
}
