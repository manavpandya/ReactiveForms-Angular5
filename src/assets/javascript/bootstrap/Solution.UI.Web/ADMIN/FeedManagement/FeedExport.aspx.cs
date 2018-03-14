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

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class FeedExport : BasePage
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
                btnGenerateCSV.ImageUrl = "/App_Themes/" + Page.Theme + "/images/generate.png";
                bindstore();
                ddlStore_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// Bind Stores into drop down
        /// </summary>
        private void bindstore()
        {
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
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                }
                else
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Binds the field category.
        /// </summary>
        protected void BindFieldCategory()
        {
            lblMessage.Text = "";
            btnGenerateCSV.Visible = false;
            trBottom.Visible = false;
            trTop.Visible = false;
            trFields.Visible = false;

            FeedComponent objFeedMaster = new FeedComponent();
            DataSet dsFeedMaster = new DataSet();
            if (!string.IsNullOrEmpty(ddlFeedName.SelectedValue) && (Convert.ToInt32(ddlFeedName.SelectedValue) > 0))
            {
                dsFeedMaster = objFeedMaster.GetDataForExport(Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(ddlFeedName.SelectedValue));
                ChklistFieldName.DataSource = dsFeedMaster;
                ChklistFieldName.DataTextField = "FieldName";
                ChklistFieldName.DataValueField = "FieldID";
                ChklistFieldName.DataBind();
                if (ChklistFieldName.Items.Count > 0)
                {
                    btnGenerateCSV.Visible = true;
                    trBottom.Visible = true;
                    trTop.Visible = true;
                    trFields.Visible = true;
                }
                else
                {
                    ChklistFieldName.DataSource = null;
                    ChklistFieldName.DataBind();
                    lblMessage.Text = "No Record Found ...";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('No Record Found ...');", true);
                }
            }
            else
            {
                ChklistFieldName.DataSource = null;
                ChklistFieldName.DataBind();
                lblMessage.Text = "No Record Found ...";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('No Record Found ...');", true);
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFeedName.Items.Clear();
            DataSet dsFeedFieldType = new DataSet();
            FeedComponent objFeedMaster = new FeedComponent();
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

            dsFeedFieldType = objFeedMaster.GetFeedMasterByStore(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsFeedFieldType != null && dsFeedFieldType.Tables.Count > 0 && dsFeedFieldType.Tables[0].Rows.Count > 0)
            {
                ddlFeedName.DataSource = dsFeedFieldType.Tables[0];
                ddlFeedName.DataValueField = "FeedID";
                ddlFeedName.DataTextField = "FeedName";
                ddlFeedName.DataBind();
            }
            else
            {
                ddlFeedName.Items.Insert(0, new ListItem("- No Feed Name Found -", "0"));
            }
            BindFieldCategory();
        }

        /// <summary>
        /// Feed Name Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlFeedName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFieldCategory();
        }

        #region functions for Export

        /// <summary>
        /// Replace \r\n with New Line Character
        /// </summary>
        /// <param name="sFieldValueToEscape">String sFieldValueToEscape</param>
        /// <returns>Returns the \r\n with New Line Character</returns>
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
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
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";//sFieldValueToEscape;
            }
        }

        /// <summary>
        /// This will write file with 
        /// given text 
        /// </summary>
        /// <param name="Text"> String Text </param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            try
            {
                FileInfo info = new FileInfo(FileName);
                writer = info.AppendText();
                writer.Write(Text);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        ///  Generate CSV Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGenerateCSV_Click(object sender, ImageClickEventArgs e)
        {
            DataRow rw;
            Int32 cnt = 0;
            string strfields = "", strFieldsNames = "";
            DataTable tblExport = new DataTable();
            bool flag = false;
            if ((DataTable)ViewState["CreateTable"] == null)
            {
                cnt = 0;
                if (ChklistFieldName.Items.Count > 0)
                {
                    tblExport.Columns.Add("ProductID");
                    for (int i = 0; i < ChklistFieldName.Items.Count; i++)
                    {
                        if (ChklistFieldName.Items[i].Selected)
                        {
                            flag = true;
                            tblExport.Columns.Add(ChklistFieldName.Items[i].Text.ToString().Trim(), typeof(string));
                            strfields += "{" + cnt + "},";
                            strFieldsNames += ChklistFieldName.Items[i].Text.ToString().Trim() + ",";
                            cnt++;
                        }
                    }
                }
                else
                {
                }
            }
            if (flag == false)
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please select at least one field.');", true);
                return;
            }
            if (strfields.Length > 1)
            {
                strfields = strfields.Substring(0, strfields.Length - 1);
                strFieldsNames = strFieldsNames.Substring(0, strFieldsNames.Length - 1);
            }
            string strSelect = " p.ProductId";
            string strFrom = "";
            strFrom += " Products p ";
            string strQuery = "with Products as ( SELECT  row_number() over (order by pr.ProductId) RowNum,pr.ProductId as ProductId FROM (SELECT  DISTINCT top 100 PERCENT dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".ProductId as ProductId FROM  dbo.tb_FeedFieldMaster INNER JOIN dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + " ON dbo.tb_FeedFieldMaster.FieldID = dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".FieldID WHERE dbo.tb_FeedFieldMaster.FeedId=" + ddlFeedName.SelectedValue.ToString() + " Order By ProductId) as pr)";
            if (tblExport != null && tblExport.Columns.Count > 0)
            {
                for (int j = 1; j < tblExport.Columns.Count; j++)
                {
                    strQuery += ",[" + tblExport.Columns[j].ColumnName.ToString().Replace(" ", "").Replace("'", "''") + "] as(SELECT DISTINCT top 100 PERCENT row_number() over (order by dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".ProductId) RowNum,dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".FieldValue as [" + tblExport.Columns[j].ColumnName.ToString() + "],dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".ProductId AS ppp FROM dbo.tb_FeedFieldMaster INNER JOIN dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + " ON dbo.tb_FeedFieldMaster.FieldID = dbo.tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + ".FieldID WHERE dbo.tb_FeedFieldMaster.FeedId=" + ddlFeedName.SelectedValue.ToString() + " and dbo.tb_FeedFieldMaster.FieldName='" + tblExport.Columns[j].ColumnName.ToString() + "' Order By PPP)";
                    strFrom += " full outer join [" + tblExport.Columns[j].ColumnName.ToString().Replace(" ", "").Replace("'", "''") + "] on p.ProductId = [" + tblExport.Columns[j].ColumnName.ToString().Replace(" ", "").Replace("'", "''") + "].ppp";
                    strSelect += ",[" + tblExport.Columns[j].ColumnName.ToString().Replace(" ", "").Replace("'", "''") + "].[" + tblExport.Columns[j].ColumnName.ToString().Replace("'", "''") + "]";
                }
            }
            strQuery = strQuery + " SELECT  " + strSelect + " FROM  " + strFrom + " Order By p.ProductId";

            DataSet dsExport = new DataSet();

            dsExport = CommonComponent.GetCommonDataSet(strQuery);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            object[] args = new object[cnt];
            if (dsExport != null && dsExport.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsExport.Tables[0].Rows.Count; i++)
                {
                    for (int c = 1; c < dsExport.Tables[0].Columns.Count; c++)
                    {
                        if (Convert.ToString(dsExport.Tables[0].Rows[i][c].ToString()).IndexOf(",") > -1 || Convert.ToString(dsExport.Tables[0].Rows[i][c].ToString()).IndexOf("'") > -1)
                        {
                            string strResponse = dsExport.Tables[0].Rows[i][c].ToString();
                            string DefautValue = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(DefautValue,'') as DefautValue FROM tb_FeedFieldMaster WHERE FieldName='" + dsExport.Tables[0].Columns[c].ColumnName.ToString() + "' AND FeedID=" + ddlFeedName.SelectedValue.ToString() + ""));
                            if (string.IsNullOrEmpty(strResponse))
                            {
                                strResponse = DefautValue;
                            }

                            strResponse = strResponse.Replace("\t", "");
                            strResponse = strResponse.Replace("\" />", "\"/>");
                            strResponse = strResponse.Replace("\" >", "\">");
                            strResponse = strResponse.Replace("©", "&copy;");
                            strResponse = strResponse.Replace("®", "&reg;");
                            strResponse = strResponse.Replace("™", "&trade;");
                            strResponse = strResponse.Replace("$", "");
                            strResponse = strResponse.Replace("~", "");
                            strResponse = strResponse.Replace("*", "");
                            strResponse = strResponse.Replace("^", "");

                            System.Text.RegularExpressions.Regex objRegExp = new System.Text.RegularExpressions.Regex("<(.|\n)+?>");
                            string strlimit = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(FieldLimit,0) as FieldLimit FROM tb_FeedFieldMaster WHERE FieldName='" + dsExport.Tables[0].Columns[c].ColumnName.ToString() + "' AND FeedID=" + ddlFeedName.SelectedValue.ToString() + ""));
                            if (!string.IsNullOrEmpty(strlimit))
                            {
                                if (Convert.ToInt32(strlimit) > 0)
                                {
                                    strResponse = objRegExp.Replace(strResponse, String.Empty);

                                    if (strResponse.Length > Convert.ToInt32(strlimit))
                                    {
                                        strResponse = _EscapeCsvField(strResponse.Substring(0, Convert.ToInt32(strlimit)).Replace("\r\n", ""));
                                        args[c - 1] = strResponse;
                                    }
                                    else
                                    {
                                        args[c - 1] = _EscapeCsvField(strResponse.Replace("\r\n", ""));
                                    }
                                }
                            }
                            else
                            {
                                strResponse = objRegExp.Replace(strResponse, String.Empty);
                                args[c - 1] = _EscapeCsvField(strResponse.Replace("\r\n", ""));
                            }
                        }
                        else
                        {
                            string strResponse = dsExport.Tables[0].Rows[i][c].ToString();
                            string DefautValue = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(DefautValue,'') as DefautValue FROM tb_FeedFieldMaster WHERE FieldName='" + dsExport.Tables[0].Columns[c].ColumnName.ToString() + "' AND FeedID=" + ddlFeedName.SelectedValue.ToString() + ""));
                            if (string.IsNullOrEmpty(strResponse))
                            {
                                strResponse = DefautValue;
                            }
                            strResponse = strResponse.Replace("\t", "");
                            strResponse = strResponse.Replace("\" />", "\"/>");
                            strResponse = strResponse.Replace("\" >", "\">");
                            strResponse = strResponse.Replace("©", "&copy;");
                            strResponse = strResponse.Replace("®", "&reg;");
                            strResponse = strResponse.Replace("™", "&trade;");
                            strResponse = strResponse.Replace("$", "");
                            strResponse = strResponse.Replace("~", "");
                            strResponse = strResponse.Replace("*", "");
                            strResponse = strResponse.Replace("^", "");

                            System.Text.RegularExpressions.Regex objRegExp = new System.Text.RegularExpressions.Regex("<(.|\n)+?>");
                            string strlimit = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(FieldLimit,0) as FieldLimit FROM tb_FeedFieldMaster WHERE FieldName='" + dsExport.Tables[0].Columns[c].ColumnName.ToString() + "' AND FeedID=" + ddlFeedName.SelectedValue.ToString() + ""));
                            if (!string.IsNullOrEmpty(strlimit))
                            {
                                if (Convert.ToInt32(strlimit) > 0)
                                {
                                    strResponse = objRegExp.Replace(strResponse, String.Empty);

                                    if (strResponse.Length > Convert.ToInt32(strlimit))
                                    {
                                        strResponse = _EscapeCsvField(strResponse.Substring(0, Convert.ToInt32(strlimit)).Replace("\r\n", ""));
                                        args[c - 1] = strResponse;
                                    }
                                    else
                                    {
                                        args[c - 1] = _EscapeCsvField(strResponse.Replace("\r\n", ""));
                                    }
                                }
                            }
                            else
                            {
                                strResponse = objRegExp.Replace(strResponse, String.Empty);
                                args[c - 1] = _EscapeCsvField(strResponse.Replace("\r\n", ""));
                            }
                        }
                    }
                    sb.AppendLine(string.Format(strfields, args));
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                string FullString = sb.ToString();
                sb.Remove(0, sb.Length);
                sb.AppendLine(strFieldsNames);
                sb.AppendLine(FullString);

                DateTime dt = DateTime.Now;
                String FileName = "Feed_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                Response.Clear();
                Response.ClearContent();
                String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('No Record Found ...');", true);
            }
        }
        #endregion
    }
}