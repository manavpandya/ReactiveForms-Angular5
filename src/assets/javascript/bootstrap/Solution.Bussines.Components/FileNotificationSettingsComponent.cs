using System;
using System.Web;
using System.Web.Mail;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// File Notification Settings Component Class Contains File Notification related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class FileNotificationSettingsComponent
    {
        string m_sFileName;

        /// <summary>
        /// File Notification Settings Component
        /// </summary>
        public FileNotificationSettingsComponent()
        {
            m_sFileName = ConfigurationSettings.AppSettings["errorNotifier_Filename"];
            if (m_sFileName == null)
                m_sFileName = "error.txt";
        }

        /// <summary>
        /// File Name
        /// </summary>
        public string Filename
        {
            get { return m_sFileName; }
            set { m_sFileName = value; }
        }
    }
}
