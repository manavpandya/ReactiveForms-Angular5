using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solution.Bussines.Components.Common
{

    /// <summary>
    /// AppLogic Component Class Contains AppLogic related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class AppLogic
    {

        #region Declaration

        static public AppConfig AppConfigTable;

        static public readonly String ro_OK = "OK";

        static public readonly String ro_TXModeAuthCapture = "AUTH CAPTURE";
        static public readonly String ro_TXModeAuthOnly = "AUTH";
        static public readonly String ro_TXStateAuthorized = "AUTHORIZED";
        static public readonly String ro_TXStateCaptured = "CAPTURED";
        static public readonly String ro_TXStateVoided = "VOIDED";
        static public readonly String ro_TXStateRefunded = "REFUNDED";
        static public readonly String ro_TXStateFraud = "FRAUD";
        static public readonly String ro_TXStateUnknown = "UNKNOWN";
        static public readonly String ro_TXStatePending = "PENDING";

        static public readonly String ro_PMCreditCard = "CREDITCARD";
        static public readonly String ro_PMPayPal = "PAYPAL";
        static public readonly String ro_PMPayPalExpress = "PAYPALEXPRESS";

        #endregion

        #region Key Function

        /// <summary>
        /// Get ConfigValue from Appconfig List
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns>Config Value</returns>
        public static String AppConfigs(String paramName)
        {
            try
            {
                if (AppConfigTable[paramName] != null)
                {
                    return AppConfigTable[paramName].ConfigValue;
                }
                else
                {
                    return "";
                }
            }
            catch (NullReferenceException ex)
            {
              
                return "";
            }
        }

        /// <summary>
        /// Initialize  Appconfig Data
        /// </summary>
        public static void ApplicationStart()
        {
            AppConfigTable = new AppConfig();
        }
   
        /// <summary>
        /// return Boolean According to Appconfig Parameter
        /// </summary>
        /// <param name="paramName">Appconfig ParameterName in String</param>
        /// <returns>return Boolean According to Appconfig Parameter</returns>
        public static bool AppConfigBool(String paramName)
        {
            String tmp = AppConfigs(paramName).ToUpperInvariant();
            if (tmp == "TRUE" || tmp == "YES" || tmp == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
