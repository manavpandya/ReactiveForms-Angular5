using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ImportProduct : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnCancel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                Bindcolm();
            }

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            // int StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //Store is selected dynamically from menu
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
            {
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            }
            else
            {
                ddlStore.SelectedIndex = 0;
                AppConfig.StoreID = 1;
            }

        }

        /// <summary>
        /// Bind Columns For Binding Columns in CheckBoxList
        /// </summary>
        public void Bindcolm()
        {
            CommonComponent clsCommon = new CommonComponent();
            DataSet Ds = new DataSet();
            Ds = clsCommon.GetProductColumns(1, Int32.Parse(ddlStore.SelectedValue.ToString()));
            chklistCat.DataSource = Ds;
            chklistCat.DataTextField = "column_name";
            chklistCat.DataValueField = "column_name";
            chklistCat.DataBind();
            chklistCat.Items.Add(new ListItem("Distributor", "Distributor"));
            chklistCat.Items.Add(new ListItem("ProductType", "ProductType"));
            chklistCat.Items.Add(new ListItem("Manufacture", "Manufacture"));
            chklistCat.Items.Add(new ListItem("TaxName", "TaxName"));
        }

        /// <summary>
        /// Function for Remove comma From String
        /// </summary>
        /// <param name="sFieldValueToEscape">String sFieldValueToEscape</param>
        /// <returns>return String</returns>
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return sFieldValueToEscape.Replace("\"", "\"\"");//sFieldValueToEscape;
            }
        }
        
        /// <summary>
        /// Writes the File
        /// </summary>
        /// <param name="Text">string Text</param>
        /// <param name="FileName">string FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);
            if (writer != null)
                writer.Close();
        }

        /// <summary>
        ///  Export Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue == "")
                return;

            string Query = "";
            string StrExcel = "";
            for (int i = 0; i < chklistCat.Items.Count; i++)
            {
                if (chklistCat.Items[i].Selected == true)
                {
                    if (chklistCat.Items[i].Text.ToString().ToLower() != "distributor" && chklistCat.Items[i].Text.ToString().ToLower() != "producttype" && chklistCat.Items[i].Text.ToString().ToLower() != "manufacture" && chklistCat.Items[i].Text.ToString().ToLower() != "taxname")
                    {

                        Query += "tb_product." + chklistCat.Items[i].Text.ToString() + ",";
                        StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                    }
                    else if (chklistCat.Items[i].Text.ToString().ToLower() != "producttype" && chklistCat.Items[i].Text.ToString().ToLower() != "manufacture" && chklistCat.Items[i].Text.ToString().ToLower() != "taxname")
                    {
                        Query += " d.Name as Distributor,";
                        StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                    }
                    else if (chklistCat.Items[i].Text.ToString().ToLower() != "manufacture" && chklistCat.Items[i].Text.ToString().ToLower() != "taxname")
                    {
                        Query += " pt.Name as ProductType,";
                        StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                    }
                    else if (chklistCat.Items[i].Text.ToString().ToLower() != "taxname")
                    {
                        Query += " M.Name as Manufacture,";
                        StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                    }
                    else
                    {
                        Query += " T.TaxName as TaxName,";
                        StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                    }
                }
            }
            if (Query.ToString().IndexOf(",") > -1)
            {
                Query = Query.Substring(0, Query.Length - 1);
                StrExcel = StrExcel.Substring(0, StrExcel.Length - 1);
            }
            CommonComponent clsCommon = new CommonComponent();
            //string Query = "select column_name from information_schema.columns where table_name = 'tb_Product' order by ordinal_position";

            DataSet Ds = new DataSet();
            Ds = clsCommon.GetProductExport(2, Query, Int32.Parse(ddlStore.SelectedValue.ToString()));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Ds != null)
            {
                //Decimal Price = Decimal.Zero;
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string strValue = "";
                    for (int iTotal = 0; iTotal < chklistCat.Items.Count; iTotal++)
                    {
                        if (chklistCat.Items[iTotal].Selected == true)
                        {
                            string strResponse = Ds.Tables[0].Rows[i][chklistCat.Items[iTotal].Text.ToString()].ToString();
                            strResponse = strResponse.Replace("\t", "");
                            strResponse = strResponse.Replace("\" />", "\"/>");
                            strResponse = strResponse.Replace("\" >", "\">");
                            strValue += _EscapeCsvField(strResponse.ToString().Replace("\r\n", "")) + ",";
                            //StrExcel += chklistCat.Items[i].Text.ToString() + ",";
                        }
                    }
                    if (strValue.ToString().IndexOf(",") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.Length - 1);
                    }
                    sb.AppendLine(strValue);
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(StrExcel.ToString());


                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = "Products_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));
                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);

                    Response.Clear();
                    Response.ClearContent();
                    //Page.RegisterStartupScript("Success", "<script type='text/javascript'>alert('CSV file generated successfully..');</script>");
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                return;
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/Dashboard.aspx");
        }

    }
}