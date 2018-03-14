using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;

namespace Solution.Bussines.Entities
{
    public partial class RedTag_CCTV_Ecomm_DBEntities
    {
        public static RedTag_CCTV_Ecomm_DBEntities Context
        {
            get
            {
                string objectContextKey = HttpContext.Current.GetHashCode().ToString("x");
                if (!HttpContext.Current.Items.Contains(objectContextKey))
                {
                    HttpContext.Current.Items.Add(objectContextKey, new RedTag_CCTV_Ecomm_DBEntities());
                }
                return HttpContext.Current.Items[objectContextKey] as RedTag_CCTV_Ecomm_DBEntities;
            }
        }
    }
}
