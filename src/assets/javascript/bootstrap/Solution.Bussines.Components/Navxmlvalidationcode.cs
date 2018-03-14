using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
namespace Solution.Bussines.Components
{
    public class Navxmlvalidationcode
    {
        private string _xmlPath;
        private string _Error;
        public string XmlPath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }
        public string XmlError
        {
            get { return _Error; }
            set { _Error = value; }
        }
        public string XMlvalidation()
        {

            FileInfo fl = new FileInfo(XmlPath);
            if (fl.Extension.ToString().ToLower() != ".xml")
            {
                XmlError = "Please assign xml file.";
            }
            else
            {
                if (File.Exists(XmlPath))
                {
                    DataSet dsXML = new DataSet();
                    dsXML.ReadXml(XmlPath);
                    if (dsXML != null && dsXML.Tables.Count > 0 && dsXML.Tables[0].Rows.Count > 0)
                    {
                        Websitedata.AuthHeader obAuth = new Websitedata.AuthHeader();
                        obAuth.Username = "admin@hpd.com";
                        obAuth.Password = "HPD#2703";
                        Websitedata.Acctivateorder objOrder = new Websitedata.Acctivateorder();
                        objOrder.AuthHeaderValue = obAuth;
                        DataSet dsTableinfo = new DataSet();
                        DataSet dscolumninfo = new DataSet();
                        String StrTable = "";
                        StrTable = dsXML.Tables[0].Rows[0][1].ToString();
                        string[] AryTables = { "" };

                        AryTables = StrTable.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string strmisingcolumn = ",";
                        string strmisingcolumn1 = ",";
                        foreach (string strtable in AryTables)
                        {
                           // dsTableinfo = objOrder.GetTableInformation(Convert.ToString(strtable));
                            if (dsTableinfo != null && dsTableinfo.Tables.Count > 0 && dsTableinfo.Tables[0].Rows.Count > 0)
                            {
                                //dsTableinfo = new DataSet();
                                //dsTableinfo = objOrder.GetTableInformation(Convert.ToString(dsXML.Tables[0].Rows[0][2].ToString()));
                                //if (dsTableinfo != null && dsTableinfo.Tables.Count > 0 && dsTableinfo.Tables[0].Rows.Count > 0)
                                //{
                                if (dsXML.Tables[1] != null && dsXML.Tables[1].Rows.Count > 0)
                                {
                                    if (dsXML.Tables[1].Columns.Count == 9)
                                    {
                                        dscolumninfo = new DataSet();


                                        bool checkcol = false;



                                     //   dscolumninfo = objOrder.GetTablecolumnInformation(Convert.ToString(strtable));
                                        for (int j = 0; j < dsXML.Tables[1].Rows.Count; j++)
                                        {
                                            if (dscolumninfo != null && dscolumninfo.Tables.Count > 0 && dscolumninfo.Tables[0].Rows.Count > 0)
                                            {
                                                checkcol = false;
                                                for (int i = 0; i < dscolumninfo.Tables[0].Rows.Count; i++)
                                                {
                                                    if (dsXML.Tables[1].Rows[j][0].ToString().ToLower().Trim() == "nan")
                                                    {
                                                        checkcol = true;
                                                        if (strmisingcolumn.IndexOf("," + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",") <= -1)
                                                        {
                                                            strmisingcolumn += dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",";
                                                        }
                                                    }
                                                    else if ((dscolumninfo.Tables[0].Rows[i]["COLUMN_NAME"].ToString().ToLower().Trim() == dsXML.Tables[1].Rows[j][0].ToString().ToLower().Trim()) && (dscolumninfo.Tables[0].Rows[i]["DATA_TYPE"].ToString().ToLower().Trim() == dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim()) && (dscolumninfo.Tables[0].Rows[i]["CHARACTER_MAXIMUM_LENGTH"].ToString().ToLower().Trim() == dsXML.Tables[1].Rows[j][2].ToString().ToLower().Trim()))
                                                    {
                                                        checkcol = true;
                                                        //if (strmisingcolumn.IndexOf("," + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",") > -1)
                                                        //{
                                                        //    strmisingcolumn = strmisingcolumn.Replace("," + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",", ",");
                                                        //}
                                                        if (strmisingcolumn.IndexOf("," + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",") <= -1)
                                                        {
                                                            strmisingcolumn += dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",";
                                                        }
                                                    }

                                                }
                                            }

                                            if (checkcol == false)
                                            {
                                                //XmlError = "Source column name, data type and limit missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower().Trim();
                                                //break;

                                                if (strmisingcolumn1.IndexOf("," + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",") <= -1)
                                                {
                                                    strmisingcolumn1 += dsXML.Tables[1].Rows[j][0].ToString().ToLower() + ",";
                                                }

                                            }

                                        }

                                        ///Insert record

                                        //for (int j = 0; j < dsXML.Tables[1].Rows.Count; j++)
                                        //{
                                        //    if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "nvarachar" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "varachar" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "ntext" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "char" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "nchar" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "text")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "text" || dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "blob" || dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "code" || dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "option")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }
                                        //    else if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "int" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "bigint" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "smallint" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "tinyint")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "integer")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }
                                        //    else if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "decimal" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "float" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "money" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "numeric" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "smallmoney")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "decimal")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }
                                        //    else if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "decimal" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "float" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "money" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "numeric")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "decimal")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }
                                        //    else if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "time" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "smalldatetime" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "timestamp" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "datetime" || dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "date")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "date" || dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "dateformula")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }
                                        //    else if (dsXML.Tables[1].Rows[j][1].ToString().ToLower().Trim() == "bit")
                                        //    {

                                        //        if (dsXML.Tables[1].Rows[j][4].ToString().ToLower().Trim() == "boolean")
                                        //        {

                                        //        }
                                        //        else
                                        //        {
                                        //            XmlError = "Source column data type  missmatch . " + dsXML.Tables[1].Rows[j][0].ToString().ToLower() + " Row No: " + (j + 1).ToString();
                                        //            break;
                                        //        }

                                        //    }

                                        //}





                                        //}

                                    }
                                    else
                                    {
                                        XmlError = "Column count less than  '" + dsXML.Tables[1].Columns.Count + "'";
                                    }
                                }
                                else
                                {
                                    XmlError = "Field not found in file.";
                                }
                                //}
                                //else
                                //{
                                //    XmlError = "Customer destinatin table not found. - " + dsXML.Tables[0].Rows[0][2].ToString();
                                //}
                            }
                            else
                            {
                                XmlError = "Customer source table not found. - " + dsXML.Tables[0].Rows[0][1].ToString();
                            }
                        }
                        if (strmisingcolumn.Length > 1)
                        {
                            string[] strcolumn = strmisingcolumn.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            foreach (string str in strcolumn)
                            {
                                if (strmisingcolumn1.IndexOf("," + str + ",") > -1)
                                {
                                    strmisingcolumn1 = strmisingcolumn1.Replace("," + str + ",", ",");
                                }
                            }
                            if (strmisingcolumn1.Length > 1)
                            {
                                XmlError = "Source column " + strmisingcolumn1.Substring(1, strmisingcolumn1.Length - 1) + " missmatch in given table. ";
                            }
                            else
                            {
                                XmlError = "OK";
                            }
                        }
                        else if (string.IsNullOrEmpty(XmlError))
                        {
                            XmlError = "OK";
                        }
                    }
                    else
                    {
                        XmlError = "No record found in file.";
                    }
                }
                else
                {
                    XmlError = "File not found on " + XmlPath + " path";
                }

            }

            return XmlError;
        }
    }
}
