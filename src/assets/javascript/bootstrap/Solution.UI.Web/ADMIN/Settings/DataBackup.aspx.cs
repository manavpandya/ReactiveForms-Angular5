using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class DataBackup : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnBackupDatabase.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Backup.png";
        }

        /// <summary>
        ///  Backup Database Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnBackupDatabase_Click(object sender, ImageClickEventArgs e)
        {
            string DatabaseBackupPath = "";
            DatabaseBackupPath = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ConfigValue FROM dbo.tb_AppConfig WHERE ConfigName='DatabaseBackupPath'"));

            String Filename = DatabaseBackupPath + @"\" + "DB_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + "_" + Convert.ToString(DateTime.Now.Millisecond) + ".bak";
            object i = CommonComponent.ExecuteDatabaseBackup(Filename);

            if (Convert.ToInt32(i) != 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Database backup has been taken successfully', 'Message');});", true);

            }

        }
    }
}