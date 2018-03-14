using Solution.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Solution.Bussines.Components
{
    public class WeAreTodayComponent
    {

        /// <summary>
        /// Insert We are Today Details
        /// </summary>
        /// <param name="StartDate">string Option</param>
        /// <param name="EndDate">Int32 ProductId</param>
        public static void InsertWeareToday(DateTime StartDate, DateTime EndDate, int CreateBy)
        {
            WeAreTodayDAC dac = new WeAreTodayDAC();
            dac.InsertWeareToday(StartDate, EndDate, CreateBy);

        }

        public static DataSet GetWeareToday()
        {
             WeAreTodayDAC dac = new WeAreTodayDAC();
             return dac.GetWeareToday();
            
        }

        public static int DeleteWeareToday(int WearetodayID)
        {
            WeAreTodayDAC dac = new WeAreTodayDAC();
            return dac.DeleteWeareToday(WearetodayID);
        }
    }
}
