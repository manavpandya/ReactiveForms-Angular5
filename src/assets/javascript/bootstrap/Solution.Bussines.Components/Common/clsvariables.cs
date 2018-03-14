using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solution.Bussines.Components.Common
{
   public static class clsvariables
   {
       #region Declaration
       public static System.Collections.Hashtable ht = new System.Collections.Hashtable();

       public static string PathBTemp() { return getparam("PathBTemp"); }
       public static string PathBMicroImage() { return getparam("PathBMicroImage"); }
       public static string PathBMicroImageNotAvailable() { return getparam("PathBMicroImageNotAvailable"); }
       public static string PathBIconImage() { return getparam("PathBIconImage"); }
       public static string PathBIconImageNotAvailable() { return getparam("PathBIconImageNotAvailable"); }

       public static string PathCTempBanner() { return getparam("PathCTempBanner"); }
       public static string PathCTemp() { return getparam("PathCTemp"); }
       public static string PathCBannerImage() { return getparam("PathCBannerImage"); }
       public static string PathCMicroImage() { return getparam("PathCMicroImage"); }
       public static string PathCIconImage() { return getparam("PathCIconImage"); }
       public static string PathCLargeImage() { return getparam("PathCLargeImage"); }
       public static string PathCMediumImage() { return getparam("PathCMediumImage"); }
       public static string PathCLargeImageNotAvailable() { return getparam("PathCLargeImageNotAvailable"); }
       public static string PathCIconImageNotAvailable() { return getparam("PathCIconImageNotAvailable"); }
       public static string PathCMediumImageNotAvailable() { return getparam("PathCMediumImageNotAvailable"); }
       public static string PathCMicroImageNotAvailable() { return getparam("PathCMicroImageNotAvailable"); }
       #endregion

       /// <summary>
       /// Load All Path
       /// </summary>
       public static void LoadAllPath()
       {
           
           ht = new System.Collections.Hashtable();
           
           ht.Add("PathBIconImage", "Brand/icon/");
           ht.Add("PathBIconImageNotAvailable", "Brand/icon/image_not_available.jpg");
           ht.Add("PathBTemp", "Brand/Temp/");
           ht.Add("PathBMicroImage", "Brand/Micro/");
           ht.Add("PathBMicroImageNotAvailable", "Brand/Micro/image_not_available.jpg");

           ht.Add("PathCMicroImage", "Category/Micro/");
           ht.Add("PathCIconImage", "Category/icon/");
           ht.Add("PathCLargeImage", "Category/Large/");
           ht.Add("PathCMediumImage", "Category/Medium/");
           ht.Add("PathCTemp", "Category/Temp/");
           ht.Add("PathCBannerImage", "Category/Banner/");
           ht.Add("PathCTempBanner", "Category/Temp/Banner/");
           ht.Add("PathCMicroImageNotAvailable", "Category/Micro/image_not_available.jpg");
           ht.Add("PathCIconImageNotAvailable", "Category/icon/image_not_available.jpg");
           ht.Add("PathCLargeImageNotAvailable", "Category/Large/image_not_available.jpg");
           ht.Add("PathCMediumImageNotAvailable", "Category/Medium/image_not_available.jpg");

           //product path

           //ht.add("pathpmicroimage", "product/micro/");
           //ht.add("pathpiconimage", "product/icon/");
           //ht.add("pathplargeimage", "product/large/");
           //ht.add("pathpmediumimage", "product/medium/");
           //ht.add("pathptemp", "product/temp/");

       }

       /// <summary>
       /// Get Param
       /// </summary>
       /// <param name="param">string param</param>
       /// <returns>string</returns>
       private static string getparam(string param)
       {
           try
           {
               
               return AppLogic.AppConfigs("AdminImagesPath") + ht[param];
           }
           catch { return ""; }
       }

    }
}
