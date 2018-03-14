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
    /// Event Log Settings Component Class Contains Event Log Settings related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class EventLogSettingsComponent
    {
        string m_sEventSource;

        /// <summary>
        /// Error Notified Settings
        /// </summary>
        public EventLogSettingsComponent()
        {
            m_sEventSource = ConfigurationSettings.AppSettings["errorNotifier_EventLogSource"];
            if (m_sEventSource == null)
                m_sEventSource = "ASP.NET App Error";
        }

        public string EventSource
        {
            get { return m_sEventSource; }
            set { m_sEventSource = value; }
        }
    }
}
