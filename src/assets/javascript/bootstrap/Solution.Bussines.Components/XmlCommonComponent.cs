using System;
using StringWriter = System.IO.StringWriter;
using Encoding = System.Text.Encoding;
using StringBuilder = System.Text.StringBuilder;
using MemoryStream = System.IO.MemoryStream;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;
using XmlTextWriter = System.Xml.XmlTextWriter;
using UTF8Encoding = System.Text.UTF8Encoding;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// XmlCommon Component Class Contains XmlCommon related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class XmlCommonComponent
    {
        #region Constructor
        public XmlCommonComponent()
        {
        }
        #endregion

        #region SerializeObject

        /// <summary>
        /// SerializeObject
        /// </summary>
        /// <param name="pObject"> pObject - Object </param>
        /// <param name="objectType">objectType - Type</param>
        /// <returns>String - XMLizedString</returns>
        static public String SerializeObject(Object pObject, System.Type objectType)
        {
            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(objectType);
                XmlTextWriter XmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(XmlTextWriter, pObject);
                memoryStream = (MemoryStream)XmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch (Exception ex)
            {
                return GetExceptionDetail(ex, "\n");
            }
        }
        #endregion

        #region GetExceptionDetail
        /// <summary>
        /// Get Exception Details
        /// </summary>
        /// <param name="ex">Exception - ex</param>
        /// <param name="LineSeparator">string - LineSeparator</param>
        /// <returns>string - Exception Detail</returns>
        static public String GetExceptionDetail(Exception ex, String LineSeparator)
        {
            String ExDetail = "Exception=" + ex.Message + LineSeparator;
            while (ex.InnerException != null)
            {
                ExDetail += ex.InnerException.Message + LineSeparator;
                ex = ex.InnerException;
            }
            return ExDetail;
        }
        #endregion

        #region UTF8ByteArrayToString
        /// <summary>
        /// Convert UTF 8 byte Array to String
        /// </summary>
        /// <param name="characters">Characters</param>
        /// <returns>string - constructedString</returns>
        static public String UTF8ByteArrayToString(Byte[] characters)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return constructedString;
        }
        #endregion
    }
    internal class StringWriterWithEncoding : StringWriter
    {
        #region Variables
        private Encoding m_Encoding;
        #endregion

        #region Properties
        /// <summary>
        /// String Writer with Encoding
        /// </summary>
        /// <param name="sb">StringBuilder sb</param>
        /// <param name="encoding">Encoding encoding</param>
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base()
        {
            m_Encoding = encoding;
        }

        public override Encoding Encoding { get { return m_Encoding; } }
        #endregion
    }
}
