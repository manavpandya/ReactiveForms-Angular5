using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Security Component Class Contains Security related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class SecurityComponent
    {
        #region Declaration

        static string EncryptionKey;

        #endregion

        #region Key Functions

       /// <summary>
       /// Encrypt Code
       /// </summary>
        /// <param name="s"> string value</param>
       /// <returns>Hash Code</returns>
        static public String GetMD5Hash(String s)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            Byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Encrypt Code
        /// </summary>
        /// <param name="strText">string value</param>
        /// <returns>Encrypt String</returns>
        public static string Encrypt(string strText)
        {
            Byte[] IV = { 12, 34, 56, 78, 90, 89, 25, 49 };
            try
            {

                //EncryptionKey =  Convert.ToString(ObjAppconfig.GetAppConfigByName("EncryptionKey"));
                EncryptionKey = System.Configuration.ConfigurationSettings.AppSettings["EncryptionKey"].ToString();
                Byte[] bykey = System.Text.Encoding.UTF8.GetBytes(EncryptionKey);
                Byte[] InputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write);
                cs.Write(InputByteArray, 0, InputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Decrypt Code
        /// </summary>
        /// <param name="strText">string value</param>
        /// <returns>Decrypt String</returns>
        public static string Decrypt(string strText)
        {
            Byte[] IV = { 12, 34, 56, 78, 90, 89, 25, 49 };
            Byte[] inputByteArray = new Byte[strText.Length];
            try
            {

                EncryptionKey = System.Configuration.ConfigurationSettings.AppSettings["EncryptionKey"].ToString();
                //  EncryptionKey = Convert.ToString(ObjAppconfig.GetAppConfigByName("EncryptionKey"));
                Byte[] bykey = System.Text.Encoding.UTF8.GetBytes(EncryptionKey);
                //Byte[] InputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bykey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

    }

}
