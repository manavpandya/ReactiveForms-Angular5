using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ViewOrderReport : BasePage
    {
        #region Declaration


        int pageSize = 20;
        int selectedStore = 0;
        DataSet dsOrder = new DataSet();
        OrderComponent ordcomp = new OrderComponent();

        Decimal hdnSubtotal1 = Decimal.Zero;
        Decimal hdnTotal1 = Decimal.Zero;
        Decimal HdnShippingCost1 = Decimal.Zero;
        Decimal hdnordertax1 = Decimal.Zero;
        Decimal hdnDiscount1 = Decimal.Zero;
        Decimal hdnRefund1 = Decimal.Zero;
        Decimal hdnAdjAmt1 = Decimal.Zero;


        Decimal hdnSubtotal1F = Decimal.Zero;
        Decimal hdnTotal1F = Decimal.Zero;
        Decimal HdnShippingCost1F = Decimal.Zero;
        Decimal hdnordertax1F = Decimal.Zero;
        Decimal hdnDiscount1F = Decimal.Zero;
        Decimal hdnRefund1F = Decimal.Zero;
        Decimal hdnAdjAmt1F = Decimal.Zero;

        Decimal hdncanceledOrder = Decimal.Zero;
        Decimal hdnvoidOrder = Decimal.Zero;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromdate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtTodate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //GetOrderListByStoreId(1, pageSize);
                //btnSearch_Click(null, null);

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["SearchFilter"] = null;
            string strSearch = "";
            if (txtFromdate.Text.ToString() != "" && txtTodate.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtTodate.Text.ToString()) >= Convert.ToDateTime(txtFromdate.Text.ToString()))
                {
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtFromdate.Text.ToString() == "" && txtTodate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtTodate.Text.ToString() == "" && txtFromdate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }

            ViewState["SearchFilter"] = strSearch.ToString();


            string strWhere = string.Empty;
            if (Convert.ToString(txtSearchText.Text).Trim() != "")
            {
                string strSearchText = txtSearchText.Text.ToString().Trim();
                int i = 0;
                strWhere = " and (";
                foreach (string strS in strSearchText.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(strS))
                    {
                        strWhere += GetSearchText(strS, ddlSearchLike.SelectedValue.ToString(), Convert.ToInt32(ddlParent.SelectedValue.ToString()));
                    }
                    i++;
                }
                strWhere += ")";
            }
            strWhere = strWhere.Replace("OR )", ")").Replace("OR)", ")");

            DataSet dsData = CommonComponent.GetCommonDataSet("Exec GetOrderReport '" + txtFromdate.Text.ToString() + "','" + txtTodate.Text.ToString() + "'," + Convert.ToInt32(ddlOption.SelectedValue.ToString()) + "," + Convert.ToInt32(ddlParent.SelectedValue.ToString()) + ",'" + strWhere + "'");
            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                ltReport.Text = GetDataForDisplay(dsData, Convert.ToInt32(ddlParent.SelectedValue.ToString()));
            }
        }

        /// <summary>
        /// Get Search Text for Where Condition
        /// </summary>
        /// <param name="strS">Search text</param>
        /// <param name="SearchLike">Search Like Type</param>
        /// <param name="Parent">Is Parent or Child</param>
        /// <returns></returns>
        private string GetSearchText(string strS, string SearchLike, int Parent)
        {
            string strQuery = string.Empty;
            switch (SearchLike)
            {
                case "Starts with":
                    if (Parent == 0)
                        strQuery += " tb_Product.SKU like ''" + strS.Trim() + "%'' OR ";
                    else
                        strQuery += " (tb_Product.SKU like ''" + strS.Trim() + "%'' OR tb_ProductVariantValue.SKU like ''" + strS.Trim() + "%'') OR ";
                    break;
                case "Contains":
                    if (Parent == 0)
                        strQuery += " tb_Product.SKU like ''%" + strS.Trim() + "%'' OR ";
                    else
                        strQuery += " (tb_Product.SKU like ''%" + strS.Trim() + "%'' OR tb_ProductVariantValue.SKU like ''%" + strS.Trim() + "%'') OR ";
                    break;
                case "Exact":
                    if (Parent == 0)
                        strQuery += " tb_Product.SKU =''" + strS.Trim() + "'' OR";
                    else
                        strQuery += " (tb_Product.SKU =''" + strS.Trim() + "'' OR tb_ProductVariantValue.SKU = ''" + strS.Trim() + "'') OR ";
                    break;
            }
            return strQuery;
        }

        /// <summary>
        /// Get Data for Display HTML
        /// </summary>
        /// <param name="DsData">Dataset of Records with All Data</param>
        /// <param name="IsParent">Is Parent or Child</param>
        /// <returns>HTML For Display</returns>
        private string GetDataForDisplay(DataSet DsData, int IsParent)
        {
            StringBuilder strb = new StringBuilder();
            string stryear = "";
            string stryearnew = "";
            DataSet dsyear = new DataSet();
            dsyear = CommonComponent.GetCommonDataSet("select Year,SKU,PSKU,RM,MO,CU,TotalOrder from TempOrderReportYear Order by SKU ASC ");
            if (DsData != null && DsData.Tables.Count > 0)
            {
                foreach (DataTable dTable in DsData.Tables)
                {
                    int iTotalRM = 0, iTotalMO = 0, iTotalCU = 0, iTotalO = 0;
                    if (dTable.Rows.Count > 0)
                    {
                        for (int j = 0; j < dTable.Columns.Count; j++)
                        {
                            string[] strgetyear = dTable.Columns[j].ColumnName.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strgetyear.Length > 1)
                            {
                                stryearnew = strgetyear[1].ToString();
                                if (stryear.IndexOf(stryearnew) <= -1)
                                {
                                    DataRow[] dr = dsyear.Tables[0].Select("year=" + stryearnew + "", "SKU ASC");
                                    if (dr.Length > 0)
                                    {
                                        strb.Append("<table onclick='funHideShow(\"" + stryearnew.ToString() + "\");'  class='table' cellspacing='1' cellpadding='0' style='width:100%;border-color:#ff0000;border-width:1px;border-style:Solid;width:100%;margin-bottom:0 !important; cursor: pointer;'>");
                                        strb.Append("<tr>");
                                        strb.Append("<td style='background-color:#ccc; font-size:18px; font-weight:bold; text-align:center;padding:10px;'>" + stryearnew);
                                        strb.Append("</td>");
                                        strb.Append("</tr>");
                                        strb.Append("</table>");
                                        strb.Append("<p>&nbsp;</p>");
                                        strb.Append("<table class='dashboard-left' cellspacing='1' cellpadding='0' style='width:100%;border-color:#DFDFDF;border-width:1px;border-style:Solid;width:100%;margin-bottom:0 !important;'>");
                                        strb.Append("<tr>");
                                        for (int k = 0; k < dsyear.Tables[0].Columns.Count; k++)
                                        {
                                            if (dsyear.Tables[0].Columns[k].ColumnName.ToLower() != "year")
                                            {
                                                if (IsParent == 0)
                                                {
                                                    if (dsyear.Tables[0].Columns[k].ColumnName != "PSKU")
                                                    {
                                                        if (k == 0)
                                                            strb.Append("<th style='width:20%;text-align:left;padding:5px;'>" + dsyear.Tables[0].Columns[k].ColumnName.ToString().Replace("_", " ").Replace("RM", "READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                                        else
                                                            strb.Append("<th style='width:20%;text-align:center;padding:5px;'>" + dsyear.Tables[0].Columns[k].ColumnName.ToString().Replace("_", " ").Replace("RM", "READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (k <= 1)
                                                        strb.Append("<th style='width:16%;text-align:left;padding:5px;'>" + dsyear.Tables[0].Columns[k].ColumnName.ToString().Replace("_", " ").Replace("RM", " READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                                    else
                                                        strb.Append("<th style='width:16%;text-align:center;padding:5px;'>" + dsyear.Tables[0].Columns[k].ColumnName.ToString().Replace("_", " ").Replace("RM", " READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                                }
                                            }
                                        }
                                        strb.Append("</tr>");

                                        int TotalRM = 0, TotalMO = 0, TotalCU = 0, TotalO = 0;
                                        foreach (DataRow dRow in dr)
                                        {
                                            strb.Append("<tr>");
                                            for (int i = 0; i < dsyear.Tables[0].Columns.Count; i++)
                                            {
                                                if (dsyear.Tables[0].Columns[i].ColumnName.ToLower() != "year")
                                                {
                                                    if (IsParent == 0)
                                                    {
                                                        if (dsyear.Tables[0].Columns[i].ColumnName != "PSKU")
                                                        {
                                                            if (i == 1)
                                                                strb.Append("<td style='width:20%; text-align:left;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                                            else
                                                                strb.Append("<td style='width:20%; text-align:center;padding:5px;'>" + dRow[i].ToString() + "</td>");

                                                            switch (dsyear.Tables[0].Columns[i].ColumnName.ToLower())
                                                            {
                                                                case "rm":
                                                                    TotalRM = TotalRM + Convert.ToInt32(dRow[i].ToString());
                                                                    break;
                                                                case "mo":
                                                                    TotalMO = TotalMO + Convert.ToInt32(dRow[i].ToString());
                                                                    break;
                                                                case "cu":
                                                                    TotalCU = TotalCU + Convert.ToInt32(dRow[i].ToString());
                                                                    break;
                                                                case "totalorder":
                                                                    TotalO = TotalO + Convert.ToInt32(dRow[i].ToString());
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i <= 2)
                                                            strb.Append("<td style='width:16%;text-align:left;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                                        else
                                                            strb.Append("<td style='width:16%;text-align:center;padding:5px;'>" + dRow[i].ToString() + "</td>");

                                                        switch (dsyear.Tables[0].Columns[i].ColumnName.ToLower())
                                                        {
                                                            case "rm":
                                                                TotalRM = TotalRM + Convert.ToInt32(dRow[i].ToString());
                                                                break;
                                                            case "mo":
                                                                TotalMO = TotalMO + Convert.ToInt32(dRow[i].ToString());
                                                                break;
                                                            case "cu":
                                                                TotalCU = TotalCU + Convert.ToInt32(dRow[i].ToString());
                                                                break;
                                                            case "totalorder":
                                                                TotalO = TotalO + Convert.ToInt32(dRow[i].ToString());
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            strb.Append("</tr>");
                                        }

                                        //strb.Append("</tr>");

                                        strb.Append("<tr>");
                                        if (IsParent == 1)
                                            strb.Append("<td colspan='2'  style='text-align:left;padding:5px;font-size:16px;'> ALL TOTAL");
                                        else
                                            strb.Append("<td style='text-align:left;padding:5px;font-size:16px;font-weight:bold;'> ALL TOTAL");
                                        strb.Append("</td>");
                                        strb.Append("<td style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + TotalRM.ToString());
                                        strb.Append("</td>");
                                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + TotalMO.ToString());
                                        strb.Append("</td>");
                                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + TotalCU.ToString());
                                        strb.Append("</td>");
                                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + TotalO.ToString());
                                        strb.Append("</td>");
                                        strb.Append("</tr>");
                                        strb.Append("</table>");
                                    }

                                    stryear = stryear + "," + stryearnew;
                                    break;
                                }
                            }
                        }

                        strb.Append("<table style='display:none;' id='tb_" + stryearnew.ToString() + "' class='dashboard-left' cellspacing='1' cellpadding='0' style='width:100%;border-color:#DFDFDF;border-width:1px;border-style:Solid;width:100%;margin-bottom:0 !important;'>");
                        strb.Append("<tr>");
                        for (int j = 0; j < dTable.Columns.Count; j++)
                        {
                            if (IsParent == 0)
                            {
                                if (dTable.Columns[j].ColumnName != "PSKU")
                                {
                                    if (j == 0)
                                        strb.Append("<th style='width:20%;text-align:left;padding:5px;'>" + dTable.Columns[j].ColumnName.ToString().Replace("_", " ").Replace("RM", "READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                    else
                                        strb.Append("<th style='width:20%;text-align:center;padding:5px;'>" + dTable.Columns[j].ColumnName.ToString().Replace("_", " ").Replace("RM", "READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                }
                            }
                            else
                            {
                                if (j <= 1)
                                    strb.Append("<th style='width:16%;text-align:left;padding:5px;'>" + dTable.Columns[j].ColumnName.ToString().Replace("_", " ").Replace("RM", " READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                                else
                                    strb.Append("<th style='width:16%;text-align:center;padding:5px;'>" + dTable.Columns[j].ColumnName.ToString().Replace("_", " ").Replace("RM", " READY MADE").Replace("MO", "MADE TO ORDER").Replace("CU", "CUSTOM").Replace("Orders", "TOTAL ORDERS").ToUpper() + "</th>");
                            }
                        }
                        strb.Append("</tr>");
                        foreach (DataRow dRow in dTable.Rows)
                        {
                            strb.Append("<tr>");
                            for (int i = 0; i < dTable.Columns.Count; i++)
                            {
                                if (IsParent == 0)
                                {
                                    if (dTable.Columns[i].ColumnName != "PSKU")
                                    {
                                        if (i == 0)
                                            strb.Append("<td style='width:20%; text-align:left;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                        else
                                            strb.Append("<td style='width:20%; text-align:center;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                    }
                                }
                                else
                                {
                                    if (i <= 1)
                                        strb.Append("<td style='width:16%;text-align:left;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                    else
                                        strb.Append("<td style='width:16%;text-align:center;padding:5px;'>" + dRow[i].ToString() + "</td>");
                                }

                                string CName = dTable.Columns[i].ColumnName.ToLower();

                                if (CName.Contains("_rm"))
                                    iTotalRM = iTotalRM + Convert.ToInt32(dRow[i].ToString());
                                else if (CName.Contains("_mo"))
                                    iTotalMO = iTotalMO + Convert.ToInt32(dRow[i].ToString());
                                else if (CName.Contains("_cu"))
                                    iTotalCU = iTotalCU + Convert.ToInt32(dRow[i].ToString());
                                else if (CName.Contains("_orders"))
                                    iTotalO = iTotalO + Convert.ToInt32(dRow[i].ToString());

                            }
                            strb.Append("</tr>");


                        }
                        strb.Append("<tr>");
                        if (IsParent == 1)
                            strb.Append("<td colspan='2'  style='text-align:left;padding:5px;font-size:16px;'> TOTAL");
                        else
                            strb.Append("<td style='text-align:left;padding:5px;font-size:16px;font-weight:bold;'> TOTAL");
                        strb.Append("</td>");
                        strb.Append("<td style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + iTotalRM.ToString());
                        strb.Append("</td>");
                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + iTotalMO.ToString());
                        strb.Append("</td>");
                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + iTotalCU.ToString());
                        strb.Append("</td>");
                        strb.Append("<td  style='text-align:center;padding:5px;font-weight:bold;font-size:16px;'>" + iTotalO.ToString());
                        strb.Append("</td>");
                        strb.Append("</tr>");
                        //strb.Append("</tr>");
                        strb.Append("</table>");
                    }
                }
            }
            return strb.ToString();
        }
    }
}