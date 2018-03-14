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
    /// Feed Component Class Contains Countr Feed related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class FeedComponent
    {
        #region  Key Function

        /// <summary>
        /// Function For Create Feed
        /// </summary>
        /// <param name="Feed">tb_FeedMaster Feed</param>
        /// <returns>Returns value of current inserted record</returns>
        public Int32 InsertFeed(tb_FeedMaster Feed)
        {
            Int32 isAdded = 0;
            try
            {
                FeedDAC objFeed = new FeedDAC();
                if (objFeed.CheckDuplicate(Feed) == 0)
                {
                    objFeed.Create(Feed);
                    isAdded = Feed.FeedID;
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
        /// Update Method for Feed
        /// </summary>
        /// <param name="Feed">tb_FeedMaster Feed</param>
        /// <returns>Returns FeedId of currently updated record</returns>
        public Int32 UpdateFeed(tb_FeedMaster Feed)
        {
            Int32 isUpdated = 0;
            try
            {
                FeedDAC objFeed = new FeedDAC();
                if (objFeed.CheckDuplicate(Feed) == 0)
                {
                    UpdateFeedMaster(Feed);
                    isUpdated = Feed.FeedID;
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
        ///  Get Feed Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return a Feed record for edit in table form</returns>
        public tb_FeedMaster getFeed(int Id)
        {
            FeedDAC objFeed = new FeedDAC();
            return objFeed.getFeed(Id);
        }

        /// <summary>
        /// Sub Method for Feed Master
        /// </summary>
        /// <param name="Feed">tb_FeedMaster Feed</param>
        /// <returns>Returns Object</returns>
        private tb_FeedMaster UpdateFeedMaster(tb_FeedMaster Feed)
        {
            FeedDAC objFeed = new FeedDAC();
            objFeed.Update(Feed);
            return Feed;
        }

        /// <summary>
        ///  For Delete Feed By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void delFeed(int Id)
        {
            FeedDAC objFeed = new FeedDAC();
            objFeed.Delete(Id);
        }

        #endregion

        #region Grid Function

        /// <summary>
        /// Variable And Properties
        /// </summary>
        #region Variable And Properties
        private static int _count;
        private static bool _newFilter = false;
        private static string _Filter = "";
        private static bool _afterDelete = false;

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
        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }
        #endregion

        /// <summary>
        /// Get total number of record of state table
        /// </summary>
        /// <param name="CName">String CName</param>
        /// <returns>Return count of rows for state table as int</returns>
        public static int GetCount(string CName)
        {
            return (_afterDelete ? _count : _count);
        }

        /// <summary>
        /// Get Feed Details
        /// </summary>
        /// <param name="SId">Int32 SId</param>
        /// <param name="SearchVal">string SearchVal</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedData(Int32 SId, string SearchVal)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFeedDetails(SId, SearchVal);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Details using FeedId
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetDatabyFeedId(Int32 FeedId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFeedDataByFeedId(FeedId);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Field Type Details
        /// </summary>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedFieldTypeMaster()
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = CommonComponent.GetCommonDataSet("Select * from tb_FeedFieldTypeMaster");
            return DSFeed;
        }
        #endregion

        #region Oprations for Feed Field Master

        /// <summary>
        /// Insert Feed Field Value
        /// </summary>
        /// <param name="tb_FeedFieldMaster">tb_FeedFieldMaster tb_FeedFieldMaster</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Id of inserted Feed Field</returns>
        public Int32 InsertFeedField(tb_FeedFieldMaster tb_FeedFieldMaster, Int32 StoreID)
        {
            int isAdded = 0;
            FeedDAC objFeed = new FeedDAC();
            try
            {
                if (objFeed.CheckDuplicateFeedField(tb_FeedFieldMaster) == 0)
                {
                    objFeed = new FeedDAC();
                    isAdded = objFeed.InsertFeedField(tb_FeedFieldMaster, StoreID);
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
        /// Get Feed Field Data
        /// </summary>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetDatabyFeedFieldId(Int32 FieldId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetDatabyFeedFieldIds(FieldId);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Field Value DataList
        /// </summary>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <param name="TypeId">Int32 TypeId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFieldValueById(Int32 FieldId, Int32 TypeId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFieldValueByIds(FieldId, TypeId);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Master details by Store
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedMasterByStore(Int32 StoreId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFeedMasterByStores(StoreId);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Details 
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedDetail(Int32 StoreId, Int32 FeedId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFeedDetailsforFieldData(StoreId, FeedId);
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Mapping Details
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedmapping(Int32 FeedId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = CommonComponent.GetCommonDataSet("SELECT dbo.tb_FeedMaster.FeedID FROM dbo.tb_FeedFieldMapping INNER JOIN dbo.tb_FeedMaster ON dbo.tb_FeedFieldMapping.RelatedFeedID = dbo.tb_FeedMaster.FeedID WHERE dbo.tb_FeedFieldMapping.RelatedFeedID=" + FeedId + "");
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Field Details
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset </returns>
        public DataSet GetFeedFieldDetails(Int32 FeedId)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = CommonComponent.GetCommonDataSet("SELECT dbo.tb_FeedFieldTypeMaster.TypeName, dbo.tb_FeedFieldMaster.* FROM dbo.tb_FeedFieldMaster INNER JOIN dbo.tb_FeedFieldTypeMaster ON dbo.tb_FeedFieldMaster.FieldTypeID = dbo.tb_FeedFieldTypeMaster.FieldTypeID WHERE dbo.tb_FeedFieldMaster.FeedID=" + FeedId + " Order by dbo.tb_FeedFieldMaster.DisplayOrder");
            return DSFeed;
        }

        /// <summary>
        /// Get Feed Values
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <returns>Returns String</returns>
        public string GetFielValues(Int32 FeedId, Int32 ProductID, Int32 FieldId)
        {
            string str = "";
            str = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(dbo.tb_FeedFieldValues_" + FeedId + ".FieldValue,'') as FieldValue FROM dbo.tb_FeedFieldMaster INNER JOIN dbo.tb_FeedFieldValues_" + FeedId + " ON dbo.tb_FeedFieldMaster.FieldID = dbo.tb_FeedFieldValues_" + FeedId + ".FieldID WHERE dbo.tb_FeedFieldMaster.FeedID=" + FeedId + " AND  dbo.tb_FeedFieldValues_" + FeedId + ".ProductID=" + ProductID + " AND dbo.tb_FeedFieldValues_" + FeedId + ".FieldID=" + FieldId + ""));
            if (str == null)
            {
                str = "";
            }
            return str;
        }

        /// <summary>
        /// Get Feed Default Values
        /// </summary>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <returns>Returns String</returns>
        public string GetFieldDefaulesValues(Int32 FieldId)
        {
            string str = "";
            str = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(DefautValue,'') as DefautValue FROM dbo.tb_FeedFieldMaster WHERE FieldID=" + FieldId + ""));
            if (str == null)
            {
                str = "";
            }
            return str;
        }

        /// <summary>
        /// Get Manufacture Details
        /// </summary>
        /// <param name="storeid">Int32 storeid</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetManufacture(Int32 storeid)
        {
            String Query = "select 'Manufacture-' + replace(SEName,'-','_') as link,ManufactureID,Name,Description,SEDescription,SEName,SEname + '_' + convert(varchar(20), manufactureid) as 'ImageName',DisplayOrder from tb_Manufacture where Deleted<>1 and Active=1 " + ((storeid == 0) ? "" : " and StoreID=" + storeid.ToString()) + " Order by Name";
            return CommonComponent.GetCommonDataSet(Query);
        }

        /// <summary>
        /// Get Field Values
        /// </summary>
        /// <param name="FieldID">Int32 FieldID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFieldValues(Int32 FieldID)
        {
            string strsql = "SELECT dbo.tb_FeedFieldTypeValues.FieldValues, dbo.tb_FeedFieldTypeValues.DisplayOrder, dbo.tb_FeedFieldTypeValues.FieldID";
            strsql += " FROM dbo.tb_FeedFieldMaster INNER JOIN";
            strsql += " dbo.tb_FeedFieldTypeValues ON dbo.tb_FeedFieldMaster.FieldID = dbo.tb_FeedFieldTypeValues.FieldID INNER JOIN";
            strsql += " dbo.tb_FeedFieldTypeMaster ON dbo.tb_FeedFieldTypeValues.TypeID = dbo.tb_FeedFieldTypeMaster.FieldTypeID WHERE dbo.tb_FeedFieldTypeValues.FieldID=" + FieldID.ToString() + " Order By dbo.tb_FeedFieldTypeValues.DisplayOrder";
            return CommonComponent.GetCommonDataSet(strsql);
        }

        /// <summary>
        /// Get Category Details
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCategoryByStoreId(Int32 StoreID)
        {
            return CommonComponent.GetCommonDataSet("Select c.CategoryID, Name, cm.ParentCategoryID, DisplayOrder, c.StoreID  from tb_FeedCategory c inner join tb_FeedCategoryMapping cm on c.CategoryID=cm.CategoryID where Deleted != 1 and Active = 1 and c.StoreID = " + StoreID + " and c.Storeid=cm.StoreID order by name asc");
        }

        /// <summary>
        /// Get Feed Base Record Value
        /// </summary>
        /// <param name="feedid">int feedid</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeeDBybase(int feedid)
        {
            return CommonComponent.GetCommonDataSet("SELECT FeedID FROM tb_FeedMaster WHERE IsBase=1 AND FeedID=" + feedid + "");
        }

        /// <summary>
        /// Get Feed List
        /// </summary>
        /// <param name="FeedID">int FeedID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedlist(int FeedID)
        {
            return CommonComponent.GetCommonDataSet("SELECT DISTINCT RelatedFeedID FROM tb_FeedFieldMapping WHERE BaseFeedID=" + FeedID + "");
        }

        /// <summary>
        /// Get Feed Related Field
        /// </summary>
        /// <param name="fieldid">int fieldid</param>
        /// <param name="feedid">int feedid</param>
        /// <param name="baseid">int baseid</param>
        public void GetFeedRelatedField(int fieldid, int feedid, int baseid)
        {
            string strsql = "";
            strsql += " INSERT INTO tb_FeedFieldValues_" + baseid + "(FieldID,FieldValue,ProductID) ";
            strsql += " SELECT  Mytable.FieldID, Mytable.FieldValue,Mytable.ProductID FROM (SELECT T.FieldID,F.FieldValue,F.ProductID,T.FeedID FROM tb_FeedFieldValues_" + feedid + " as F  ";
            strsql += " inner join ";
            strsql += " (SELECT tb_FeedFieldMaster.FeedID,tb_FeedFieldMaster.FieldID,tb_FeedFieldMaster.FieldName,tb_FeedFieldMapping.BaseFieldID FROM tb_FeedFieldMaster inner join tb_FeedFieldMapping on tb_FeedFieldMaster.FieldID= tb_FeedFieldMapping.RelatedFieldID ";
            strsql += " WHERE tb_FeedFieldMapping.BaseFieldID = " + fieldid + ") as T on f.FieldID= t.BaseFieldID  WHERE  t.BaseFieldID=" + fieldid + ") as Mytable WHERE Mytable.FieldID not in (SELECT FieldID FROM tb_FeedFieldValues_" + baseid + " WHERE FieldID in(SELECT RelatedFieldID FROM tb_FeedFieldMapping WHERE BaseFieldID = " + fieldid + " AND RelatedFeedID=" + baseid + ")) and FeedID=" + baseid + "";
            CommonComponent.ExecuteCommonData(strsql);
        }

        /// <summary>
        /// Get Count of FeedFieldValue
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <param name="FEEDID">Int32 FEEDID</param>
        /// <returns>Returns Count of FeedFieldValues</returns>
        public Int32 GetFielValuesByProductID(Int32 ProductID, Int32 FieldId, Int32 FEEDID)
        {
            Int32 str = 0;
            str = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT count(ValueID) as ValueID FROM FeedFieldValues_" + FEEDID + " WHERE FieldID=" + FieldId + " and ProductID=" + ProductID + ""));
            return str;
        }

        /// <summary>
        /// Get Feed Product
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <param name="FeedName">String FeedName</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedProduct(Int32 FeedId, String FeedName)
        {
            FeedDAC objFeed = new FeedDAC();
            DataSet DSFeed = new DataSet();
            DSFeed = objFeed.GetFeedProduct(FeedId, FeedName);
            return DSFeed;
        }

        /// <summary>
        /// Get Field Name
        /// </summary>
        /// <param name="FEEDID">Int32 FEEDID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFielDname(Int32 FEEDID)
        {
            return CommonComponent.GetCommonDataSet("SELECT FieldID,ProductField FROM tb_FeedProductBaseMapping  WHERE FeedID=" + FEEDID + "");
        }

        /// <summary>
        /// Get Feed List for Clone
        /// </summary>
        /// <param name="FeedID">int FeedID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedlistForClone(int FeedID)
        {
            return CommonComponent.GetCommonDataSet("SELECT * FROM tb_FeedFieldMaster WHERE FeedID=" + FeedID + "");
        }

        /// <summary>
        /// Get Feed Name for Clone
        /// </summary>
        /// <param name="FeedID">int FeedID</param>
        /// <param name="fieldName">string fieldName</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeednameForClone(int FeedID, string fieldName)
        {
            return CommonComponent.GetCommonDataSet("SELECT FieldName,FieldID FROM tb_FeedFieldMaster WHERE FeedID=" + FeedID + " AND FieldName='" + fieldName + "'");
        }

        /// <summary>
        /// Get Feed Name Exist
        /// </summary>
        /// <param name="FieldID">int FieldID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeednameExist(int FieldID)
        {
            return CommonComponent.GetCommonDataSet("SELECT ValueID FROM tb_FeedFieldTypeValues WHERE FieldID=" + FieldID + " ");
        }

        /// <summary>
        /// Get Feed Product Field 
        /// </summary>
        /// <param name="FieldID">int FieldID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedProductFieldExist(int FieldID)
        {
            return CommonComponent.GetCommonDataSet("SELECT ProductMappingID FROM tb_FeedProductBaseMapping WHERE FieldID=" + FieldID + " ");
        }

        /// <summary>
        /// Get Feed Mapping field Exist
        /// </summary>
        /// <param name="FieldID">int FieldID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedmappingFieldExist(int FieldID)
        {
            return CommonComponent.GetCommonDataSet("SELECT MappingID FROM tb_FeedFieldMapping WHERE RelatedFieldID=" + FieldID + " ");
        }

        /// <summary>
        /// Get Feed Data For Export
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetDataForExport(Int32 StoreId, Int32 FeedId)
        {
            return CommonComponent.GetCommonDataSet(@"SELECT DISTINCT ' '+ dbo.tb_FeedFieldMaster.FieldName as FieldName, dbo.tb_FeedFieldMaster.FieldID 
                                FROM dbo.tb_FeedFieldMaster  WHERE (dbo.tb_FeedFieldMaster.FeedID = " + FeedId + ")");
        }
        #endregion

    }
}
