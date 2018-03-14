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
    /// Error Handler Component Class Contains Error related Details in Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ErrorHandlerComponent
    {
        public const int UseEventLog = 1;
        public const int UseFile = 2;
        public const int UseEmail = 4;
        int m_notifyMode = 0;
        FileNotificationSettingsComponent m_fileSettings = new FileNotificationSettingsComponent();
        EventLogSettingsComponent m_logSettings = new EventLogSettingsComponent();
        /// <summary>
        /// Get Error Handler Component
        /// </summary>
        public ErrorHandlerComponent()
        {
            if (ConfigurationSettings.AppSettings["errorNotifier_NotifyMode"] == null)
            {
                NotifyMode = UseFile;
            }
            else
            {
                NotifyMode = int.Parse(ConfigurationSettings.AppSettings["errorNotifier_NotifyMode"]);
            }
        }

        public int NotifyMode
        {
            get { return m_notifyMode; }
            set { m_notifyMode = value; }
        }

        public FileNotificationSettingsComponent FileSettings
        {
            get { return m_fileSettings; }
            set { m_fileSettings = value; }
        }

        public EventLogSettingsComponent LogSettings
        {
            get { return m_logSettings; }
            set { m_logSettings = value; }
        }

        /// <summary>
        /// Handle Exception
        /// </summary>
        public void HandleException()
        {
            Exception e = HttpContext.Current.Server.GetLastError();

            if (e == null)
                return;

            e = e.GetBaseException();

            if (e != null)
                HandleException(e);
        }

        /// <summary>
        /// Format Exception Description
        /// </summary>
        /// <param name="e">Exception e</param>
        public void HandleException(Exception e)
        {
            FormatExceptionDescription(e);
        }

        /// <summary>
        /// Error Handler for Rewriter.aspx Page
        /// </summary>
        /// <param name="e">Exception e</param>
        protected virtual void FormatExceptionDescription(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            HttpContext context = HttpContext.Current;
            if (e != null)
            {
                CommonComponent.ErrorLog(context.Request.Url.ToString(), e.Message, e.StackTrace);
                if (context.Request.Url.ToString().ToLower().IndexOf("/admin/") > -1)
                {
                }
                else
                {
                    context.Response.Redirect("~/Rewriter.aspx");
                }
            }
        }

        /// <summary>
        /// Write To File
        /// </summary>
        /// <param name="sText">string sText</param>
        void WriteToFile(string sText)
        {
            string sPath = HttpContext.Current.Server.MapPath(FileSettings.Filename);
            try
            {
                FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(sText);
                writer.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Log Event
        /// </summary>
        /// <param name="sText">string sText</param>
        void LogEvent(string sText)
        {
            try
            {
                if (!EventLog.SourceExists(LogSettings.EventSource))
                {
                    EventLog.CreateEventSource(LogSettings.EventSource, "Application");
                }

                EventLog log = new EventLog();
                log.Source = LogSettings.EventSource;
                log.WriteEntry(sText, EventLogEntryType.Error);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Clear Logged File Errors
        /// </summary>
        public void ClearLoggedFileErrors()
        {
            try
            {
                File.Delete(HttpContext.Current.Server.MapPath(FileSettings.Filename));
            }
            catch (Exception)
            {
            }
        }
    }
}
