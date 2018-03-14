using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Data;
using System.Collections;
using System.Data;
using Solution.Bussines.Entities;
using System.Data.SqlClient;
using System.Configuration;

namespace Solution.Bussines.Components.AdminCommon
{
    /// <summary>
    /// AppLogic Component Class Contains AppLogic related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class AppConfig
    {

        #region Declaration

        public SortedList ListAppConfig;
        private int _AppConfigID;
        private string _ConfigName;
        private string _Configvalue;
        public static int _StoreID = 0;

        #endregion

        #region Properties

        public int AppConfigID
        {
            get { return _AppConfigID; }
        }

        public string ConfigName
        {
            get { return _ConfigName; }
        }

        public string ConfigValue
        {
            get { return _Configvalue; }
        }

        public static int StoreID
        {
            get { return _StoreID; }
            set
            {
                _StoreID = value;
                AppLogic.ApplicationStart();
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="AppConfigID">AppConfigID</param>
        /// <param name="ConfigName">ConfigName</param>
        /// <param name="Configvalue">Configvalue</param>
        public AppConfig(int AppConfigID, string ConfigName, string Configvalue)
        {
            _AppConfigID = AppConfigID;
            _ConfigName = ConfigName;
            _Configvalue = Configvalue;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppConfig()
        {
            DataSet dsAppConfig = new DataSet();
            ListAppConfig = new SortedList();

            if (StoreID == 0)
            {
                StoreID = Convert.ToInt32(ConfigurationManager.AppSettings["GeneralStoreID"]);
            }

            AppConfigDAC ObjAppConfig = new AppConfigDAC();
            dsAppConfig = ObjAppConfig.GetAppConfig(StoreID);

            if (dsAppConfig != null && dsAppConfig.Tables.Count > 0 && dsAppConfig.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAppConfig.Tables[0].Rows.Count; i++)
                {
                    if (dsAppConfig.Tables[0].Rows[i]["ConfigName"] != null)
                    {
                        ListAppConfig.Add(Convert.ToString(dsAppConfig.Tables[0].Rows[i]["ConfigName"]).ToLowerInvariant(), new AppConfig(Convert.ToInt32(dsAppConfig.Tables[0].Rows[i]["AppConfigID"].ToString()), Convert.ToString(dsAppConfig.Tables[0].Rows[i]["ConfigName"]), Convert.ToString(dsAppConfig.Tables[0].Rows[i]["ConfigValue"])));
                    }
                }

                ListAppConfig.Add("StoreID".ToLowerInvariant(), new AppConfig(dsAppConfig.Tables[0].Rows.Count + 1000000, "StoreID".ToLowerInvariant(), Convert.ToString(StoreID)));
            }
        }

        #endregion

        #region Key Functions

        /// <summary>
        /// Get ConfigValue from Config
        /// </summary>
        /// <param name="name">String Name</param>
        /// <returns>AppConfig Value</returns>
        public AppConfig this[string ConfigName]
        {
            get
            {
                return (AppConfig)ListAppConfig[ConfigName.ToLowerInvariant()];
            }
        }

        #endregion

    }
}
