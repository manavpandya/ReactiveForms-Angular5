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

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class ItemwiseSalesReport : BasePage
    {
        public bool isAscend = false;
        public int TotalQty = 0;
        public decimal Totalprice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetStoreList(ddlStore);
                txtFromdate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtTodate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //GetOrderListByStoreId(1, pageSize);
                btnSearch_Click(null, null);
            }
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
        }

        /// <summary>
        /// Gets the Store List
        /// </summary>
        /// <param name="ddlStore">DropDownList ddlStore</param>
        private void GetStoreList(DropDownList ddlStore)
        {
            ddlStore.Items.Clear();
            DataSet dsStore = new DataSet();
            dsStore = StoreComponent.GetStoreListByMenu();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataTextField = "StoreName";

                ddlStore.DataBind();
            }
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All", "0"));
            ddlStore.SelectedValue = Convert.ToString("4");

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

            if (txtFromdate.Text.ToString() != "")
            {
                strSearch += " AND Convert(char(10),orderdate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtFromdate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtTodate.Text.ToString() != "")
            {
                strSearch += " AND Convert(char(10),orderdate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtTodate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtSearch.Text.ToString().Trim() != "")
            {
                strSearch += " AND (p.Name like '" + txtSearch.Text.ToString().Replace("'", "''") + "' OR p.SKu like '" + txtSearch.Text.ToString().Replace("'", "''") + "' OR p.optionSKu like '" + txtSearch.Text.ToString().Replace("'", "''") + "' OR p.UPC like '" + txtSearch.Text.ToString().Replace("'", "''") + "')";
            }
            if (ddlStore.SelectedValue != "0")
            {
                strSearch += " AND StoreID=" + ddlStore.SelectedValue.ToString() + " ";
            }

            grvItemwiseSalesRpt.PageIndex = 0;
            ViewState["SearchFilter"] = strSearch.ToString();
            GetOrderListByStoreId();
            if (grvItemwiseSalesRpt.Rows.Count > 0)
            {
                ImageButton blImage = (ImageButton)grvItemwiseSalesRpt.HeaderRow.FindControl("blImage");
                blImage.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                blImage.AlternateText = "Descending Order";
                blImage.ToolTip = "Descending Order";
                isAscend = true;
                blImage.CommandArgument = "DESC";
            }

        }

        /// <summary>
        /// Gets the Order List by StoreID
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        private void GetOrderListByStoreId()
        {
            DataSet dsProductDetails = new DataSet();
            string StrQuery = "";
            if (ViewState["SearchFilter"] != null)
            {
                StrQuery = "Select RefProductID,Sum(ISNULL(Quantity,0)) as Quantity,Sum(ISNULL(Quantity,0) * ISNULL(tb_OrderedShoppingCartItems.price,0)) as Totalprice,p.Name,p.SKU,ISNULL(p.OptionSku,'') as OptionSku,ISNULL(p.UPC,'') as UPC " +
                                        " from tb_OrderedShoppingCartItems Inner Join tb_product as p on p.ProductID=tb_OrderedShoppingCartItems.RefProductID " +
                                        " Where OrderedShoppingCartID in (Select ShoppingCardID from tb_order Where ISNULL(Deleted,0)=0 " + ViewState["SearchFilter"].ToString() + ") " +
                                        " Group by RefProductID,p.Name,p.SKU,ISNULL(p.OptionSku,''),ISNULL(p.UPC,'') " +
                                        " Order by Sum(ISNULL(Quantity,0)) desc";
            }
            else
            {
                StrQuery = "Select RefProductID,Sum(ISNULL(Quantity,0)) as Quantity,Sum(ISNULL(Quantity,0) * ISNULL(tb_OrderedShoppingCartItems.price,0)) as Totalprice,p.Name,p.SKU,ISNULL(p.OptionSku,'') as OptionSku,ISNULL(p.UPC,'') as UPC " +
                                       " from tb_OrderedShoppingCartItems Inner Join tb_product as p on p.ProductID=tb_OrderedShoppingCartItems.RefProductID " +
                                       " Where OrderedShoppingCartID in (Select ShoppingCardID from tb_order Where ISNULL(Deleted,0)=0) " +
                                       " Group by RefProductID,p.Name,p.SKU,ISNULL(p.OptionSku,''),ISNULL(p.UPC,'') " +
                                       " Order by Sum(ISNULL(Quantity,0)) desc";
            }

            dsProductDetails = CommonComponent.GetCommonDataSet(StrQuery.ToString());
            if (dsProductDetails != null && dsProductDetails.Tables.Count > 0 && dsProductDetails.Tables[0].Rows.Count > 0)
            {
                grvItemwiseSalesRpt.DataSource = dsProductDetails;
                grvItemwiseSalesRpt.DataBind();
                btnExport.Visible = true;
            }
            else
            {
                grvItemwiseSalesRpt.DataSource = null;
                grvItemwiseSalesRpt.DataBind();
                btnExport.Visible = false;
            }
            ViewState["ItemwiseData"] = dsProductDetails;
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            //{
            //    AppConfig.StoreID = 1;
            //}
            //else
            //{
            //    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            //}

            btnSearch_Click(null, null);
            //GetOrderListByStoreId();
        }

        protected void grvItemwiseSalesRpt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrOrderNumber = (Literal)e.Row.FindControl("ltrOrderNumber");
                Label lblProductId = (Label)e.Row.FindControl("lblProductId");
                Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                string StrSearch = "";
                if (ViewState["SearchFilter"] != null)
                {
                    StrSearch = ViewState["SearchFilter"].ToString();
                }

                String StrOrdernumber = Convert.ToString(CommonComponent.GetScalarCommonData("Select CAST(ISNULL(OrderNumber,0) as varchar(max))+',' from tb_order Where ISNULL(Deleted,0)=0 " + StrSearch.ToString() + " and ShoppingCardID in (SElect OrderedShoppingCartID from tb_OrderedShoppingCartItems Where RefProductID=" + lblProductId.Text.ToString() + ") for Xml path('')"));
                if (!string.IsNullOrEmpty(StrOrdernumber.ToString().Trim()))
                {
                    if (StrOrdernumber.ToString().IndexOf(",") > -1)
                    {
                        string[] StrOrdernum = StrOrdernumber.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (StrOrdernum.Length > 0)
                        {
                            for (int j = 0; j < StrOrdernum.Length; j++)
                            {
                                if (StrOrdernum.Length > j)
                                {
                                    ltrOrderNumber.Text += "<a style=\"font-weight: normal;text-decoration: underline; font-size: 11px;\" href=\"/Admin/Orders/Orders.aspx?id=" + StrOrdernum[j].ToString() + "\">" + StrOrdernum[j].ToString() + "</a>, ";
                                }
                            }
                        }
                        if (ltrOrderNumber.Text.ToString().Trim().Length > 1)
                        {
                            ltrOrderNumber.Text = ltrOrderNumber.Text.ToString().Substring(0, ltrOrderNumber.Text.Length - 2);
                        }
                    }
                }

                Int32 Qty = 0;
                int.TryParse(lblQuantity.Text.ToString().Trim(), out Qty);
                Totalprice += Convert.ToDecimal(lblPrice.Text.ToString());
                TotalQty += Qty;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isAscend == false)
                {
                    ImageButton blImage = (ImageButton)e.Row.FindControl("blImage");
                    if (ViewState["sortExpression"] == null || ViewState["sortExpression"].ToString().ToUpper() == "DESC")
                    {
                        blImage.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                        blImage.AlternateText = "Descending Order";
                        blImage.ToolTip = "Descending Order";
                        isAscend = true;
                        blImage.CommandArgument = "DESC";
                    }
                    else if (ViewState["sortExpression"] != null || ViewState["sortExpression"].ToString().ToUpper() == "ASC")
                    {
                        blImage.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                        blImage.AlternateText = "Ascending Order";
                        blImage.ToolTip = "Ascending Order";
                        isAscend = false;
                        blImage.CommandArgument = "ASC";
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGrandQtyTotal = (Label)e.Row.FindControl("lblGrandQtyTotal");
                Label lblFPrice = (Label)e.Row.FindControl("lblFPrice");
                lblGrandQtyTotal.Text = TotalQty.ToString();
                lblFPrice.Text = String.Format("{0:0.00}", Totalprice);

            }
        }

        /// <summary>
        /// Sorting Data Grid
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton blImage = (ImageButton)sender;
            if (blImage != null)
            {
                if (blImage.CommandArgument == "ASC")
                {
                    DataView dv = new DataView();
                    DataSet dt = new DataSet();
                    ViewState["sortExpression"] = "DESC";
                    if (ViewState["ItemwiseData"] != null)
                    {
                        dt = (DataSet)ViewState["ItemwiseData"];
                        dv = dt.Tables[0].DefaultView;
                        dv.Sort = blImage.CommandName.ToString() + " DESC";
                        dv.ToTable();
                        grvItemwiseSalesRpt.DataSource = dv;
                        grvItemwiseSalesRpt.DataBind();
                    }

                }
                else if (blImage.CommandArgument == "DESC")
                {
                    DataView dv = new DataView();
                    DataSet dt = new DataSet();
                    ViewState["sortExpression"] = "ASC";
                    if (ViewState["ItemwiseData"] != null)
                    {
                        dt = (DataSet)ViewState["ItemwiseData"];
                        dv = dt.Tables[0].DefaultView;
                        dv.Sort = blImage.CommandName.ToString() + " ASC";
                        dv.ToTable();
                        grvItemwiseSalesRpt.DataSource = dv;
                        grvItemwiseSalesRpt.DataBind();
                    }
                    isAscend = false;
                }
            }
        }


        /// <summary>
        /// Export Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            DateTime dt = DateTime.Now;
            String FileName = "SaleReport_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/text";
            //grvItemwiseSalesRpt.AllowPaging = false;
            //grvItemwiseSalesRpt.DataBind();



            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < grvItemwiseSalesRpt.Columns.Count; k++)
            {
                //add separator
                sb.Append(grvItemwiseSalesRpt.Columns[k].HeaderText + ',');
            }
            //append new line

            sb.Append("\r\n");
            for (int i = 0; i < grvItemwiseSalesRpt.Rows.Count; i++)
            {

                Label lblProductId = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblProductId");
                Label lblPName = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblPName");
                sb.Append(lblPName.Text + ',');
                Label lblSKU = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblSKU");
                sb.Append(lblSKU.Text + ',');
                Label lblOptionSku = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblOptionSku");
                sb.Append(lblOptionSku.Text + ',');
                Label lblUPC = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblUPC");
                sb.Append(lblUPC.Text + ',');
                Label lblQuantity = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblQuantity");
                sb.Append(lblQuantity.Text + ',');

                Label lblPrice = (Label)grvItemwiseSalesRpt.Rows[i].FindControl("lblPrice");
                sb.Append("$" + lblPrice.Text + ',');
               

                string StrSearch = "";
                if (ViewState["SearchFilter"] != null)
                {
                    StrSearch = ViewState["SearchFilter"].ToString();
                }
                string StrOrdernumber = string.Empty;
              
                StrOrdernumber = Convert.ToString(CommonComponent.GetScalarCommonData("Select CAST(ISNULL(OrderNumber,0) as varchar(max))+' " + "," + " ' from tb_order Where ISNULL(Deleted,0)=0 " + StrSearch.ToString() + " and ShoppingCardID in (SElect OrderedShoppingCartID from tb_OrderedShoppingCartItems Where RefProductID=" + lblProductId.Text.ToString() + ") for Xml path('')"));

                if (StrOrdernumber.IndexOfAny(new char[] { '"', ',' }) != -1)
                {
                    StringBuilder builder = new StringBuilder();
                    // if so append and with ""
                   // StrOrdernumber =StrOrdernumber.TrimEnd(',');
                    builder.AppendFormat("\"{0}\"", StrOrdernumber.Replace("\"", "\"\""));
                    StrOrdernumber = builder.ToString();
                }
                sb.Append(StrOrdernumber + ',');


                //for (int k = 0; k < grvItemwiseSalesRpt.Columns.Count; k++)
                //{
                //    //add separator

                //    sb.Append(grvItemwiseSalesRpt.Rows[i].Cells[k].Text + ',');
                //}
                //append new line
                sb.Append("\r\n");

            }
            sb.Append(" " + ',');
            sb.Append(" " + ',');
            sb.Append(" " + ',');
            sb.Append("Total: " + ',');
            Label lblGrandQtyTotal = (Label)grvItemwiseSalesRpt.FooterRow.FindControl("lblGrandQtyTotal");
            sb.Append(lblGrandQtyTotal.Text + ',');
            Label lblFPrice = (Label)grvItemwiseSalesRpt.FooterRow.FindControl("lblFPrice");
            sb.Append("$" + lblFPrice.Text + ',');
            sb.Append(" " + ',');
            sb.Append("\r\n");
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

        }

        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="FileName"></param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
    }
}