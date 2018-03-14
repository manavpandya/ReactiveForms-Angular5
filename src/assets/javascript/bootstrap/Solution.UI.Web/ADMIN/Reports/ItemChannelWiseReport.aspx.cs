using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;


namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class ItemChannelWiseReport : BasePage
    {
        public int active = 0;
        ProductComponent objproduct = new ProductComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  GetItemChannelWiseReport();
                // GetItemChannelWiseReportnew();
                GetdatafromTempTable();

            }
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
            btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Response.Clear();
            //Response.Buffer = true;
            //DateTime dt = DateTime.Now;
            //String FileName = "SKUReport_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
            //Response.Charset = "";
            //Response.ContentType = "application/text";
            ////grvItemwiseSalesRpt.AllowPaging = false;
            ////grvItemwiseSalesRpt.DataBind();



            //StringBuilder sb = new StringBuilder();
            //for (int k = 0; k < grditemchannel.Columns.Count; k++)
            //{
            //    //add separator
            //    sb.Append(grditemchannel.Columns[k].HeaderText + ',');
            //}
            ////append new line

            //sb.Append("\r\n");
            //for (int i = 0; i < grditemchannel.Rows.Count; i++)
            //{
            //    for (int k = 0; k < grditemchannel.Columns.Count; k++)
            //    {
            //        sb.Append("1,");
            //    }
            //}
            //Response.Output.Write(sb.ToString());
            //Response.Flush();
            //Response.End();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Int32 Columncount = 0;

            if (grditemchannel.Rows.Count > 0)
            {
                Columncount = grditemchannel.Rows[0].Cells.Count;
            }
            string strcolumn = "";
            string strcolumnName = "";



            for (int j = 0; j < Columncount; j++)
            {
                if (j == Columncount - 1)
                {
                    strcolumn += "\"{" + j.ToString() + "}\"";
                    if (grditemchannel.HeaderRow.Cells[j].Text.ToString().IndexOf("<a") > -1)
                    {
                        strcolumnName += grditemchannel.HeaderRow.Cells[j].Text.ToString().Substring(0, grditemchannel.HeaderRow.Cells[j].Text.ToString().IndexOf("<a"));
                    }
                    else
                    {
                        strcolumnName += grditemchannel.HeaderRow.Cells[j].Text.ToString();
                    }


                }
                else
                {
                    strcolumn += "\"{" + j.ToString() + "}\",";
                    if (grditemchannel.HeaderRow.Cells[j].Text.ToString().IndexOf("<a") > -1)
                    {
                        strcolumnName += grditemchannel.HeaderRow.Cells[j].Text.ToString().Substring(0, grditemchannel.HeaderRow.Cells[j].Text.ToString().IndexOf("<a")) + ",";
                    }
                    else
                    {
                        strcolumnName += grditemchannel.HeaderRow.Cells[j].Text.ToString() + ",";
                    }

                }

            }
            object[] args = new object[Columncount];
            for (int i = 0; i < grditemchannel.Rows.Count; i++)
            {
                for (int k = 0; k < Columncount; k++)
                {
                    args[k] = grditemchannel.Rows[i].Cells[k].Text.ToString();
                }
                sb.AppendLine(string.Format(strcolumn, args));

            }





            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                string FullString = sb.ToString();
                sb.Remove(0, sb.Length);
                sb.AppendLine(strcolumnName);
                sb.AppendLine(FullString);

                DateTime dt = DateTime.Now;
                String FileName = "Master_SKU_per_Channel_Partner_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }



        }
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
        public void GetItemChannelWiseReportnew()
        {

            DataSet dsSku = new DataSet();

            dsSku = objproduct.GetItemchannelReport(txtSearch.Text);


            DataTable dtmain = new DataTable();

            string getstring = "";
            dtmain = dsSku.Tables[0];
            string strstorename = "";
            if (dsSku != null && dsSku.Tables.Count > 0 && dsSku.Tables[0].Rows.Count > 0)
            {
                //ltrCart.Text = "<table cellspacing=\"1\" cellpadding=\"2\" style=\"border-color:#E7E7E7;border-width:1px;border-style:Solid;width:100%;\">";
                //ltrCart.Text += "<tbody><tr>";
                //ltrCart.Text += "<th width=\"10%\">";
                //ltrCart.Text += "SKU";
                //ltrCart.Text += "</th>";


                DataSet dsStore = new DataSet();
                dsStore = CommonComponent.GetCommonDataSet("select * from tb_Store where isnull(Deleted,0)<>1 and Storeid<>1 Order BY case when patindex('%EBay%',StoreName) > 0 then 1 when patindex('%Sears%',StoreName) > 0 then 2 else 0 end");
                if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsStore.Tables[0].Rows.Count; i++)
                    {
                        // ltrCart.Text += "<th align=\"left\" width=\"10%\">";
                        // ltrCart.Text += dsStore.Tables[0].Rows[i]["StoreName"].ToString();
                        // ltrCart.Text += "</th>";

                        dtmain.Columns.Add(dsStore.Tables[0].Rows[i]["StoreName"].ToString().Replace("Half Price Drapes", "").Replace("HPD", ""), typeof(string));


                    }
                    // ltrCart.Text += "</tr>";
                    string strskku = ",";
                    for (int i = 0; i < dtmain.Rows.Count; i++)
                    {
                        string sku = dtmain.Rows[i]["Ecommerce UPC"].ToString().Trim();
                        dtmain.Rows[i]["Ecommerce SKU"] = dtmain.Rows[i]["Ecommerce SKU"].ToString().Replace("-97108~", "-108").Replace("-98120~", "-120");
                        dtmain.AcceptChanges();
                        if (strskku.ToString().ToLower().IndexOf("," + dtmain.Rows[i]["Ecommerce SKU"].ToString().ToLower() + ",") > -1)
                        {
                            dtmain.Rows.RemoveAt(i);
                            dtmain.AcceptChanges();
                            i--;
                            continue;
                        }
                        else
                        {
                            strskku += dtmain.Rows[i]["Ecommerce SKU"].ToString() + ",";
                        }

                        //ltrCart.Text += "<tr  class=\"grid_paging\">";
                        //ltrCart.Text += "<td align=\"left\" valign=\"top\">";
                        // ltrCart.Text += sku;
                        //ltrCart.Text += "</td>";

                        getstring = Convert.ToString(CommonComponent.GetScalarCommonData("exec usp_getstring '" + sku + "' "));

                        if (!String.IsNullOrEmpty(getstring))
                        {
                            string[] strarray = getstring.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < strarray.Length; j++)
                            {
                                string[] strResult = strarray[j].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                for (int k = 0; k < strResult.Length; k++)
                                {
                                    string strstatus = strResult[0];
                                    string strstoreid = strResult[1];
                                    strstorename = strResult[2];

                                    bool active = false;
                                    //  if (strstatus == "1") { active = true; }
                                    // bool active = Convert.ToBoolean(strstatus);
                                    //if (active == true)
                                    //{
                                    dtmain.Rows[i][strstorename.Replace("Half Price Drapes", "").Replace("HPD", "")] = strstatus.ToString();

                                    // ltrCart.Text += "<td  style='color:green;' align=\"center\" valign=\"top\">";
                                    //ltrCart.Text += "ACTIVE";

                                    //}
                                    //else
                                    //{
                                    //    dtmain.Rows[i][strstorename] = "IN-ACTIVE";
                                    //    // ltrCart.Text += "<td  style='color:red;' align=\"center\" valign=\"top\">";
                                    //    // ltrCart.Text += "IN-ACTIVE";

                                    //}

                                }

                            }

                            for (int l = 3; l < dtmain.Columns.Count; l++)
                            {
                                if (dtmain.Rows[i][l] == DBNull.Value)
                                {
                                    dtmain.Rows[i][l] = "-";

                                }

                            }




                        }

                        else
                        {
                            for (int l = 3; l < dtmain.Columns.Count; l++)
                            {
                                dtmain.Rows[i][l] = "-";
                            }
                        }

                        //  DataRow[] Rows = dtmain.Select("SKU='" +  + "'");
                    }
                    //for (int i = 0; i < dsSku.Tables[0].Rows.Count; i++)
                    //{
                    //    ltrCart.Text += "<tr  class=\"grid_paging\">";
                    //    ltrCart.Text += "<td align=\"left\" valign=\"top\">";
                    //    ltrCart.Text += dsSku.Tables[0].Rows[i]["SKU"].ToString();
                    //    ltrCart.Text += "</td>";
                    //    for (int j = 0; j < dsStore.Tables[0].Rows.Count; j++)
                    //    {

                    //        DataSet dsTemp = new DataSet();
                    //        dsTemp = CommonComponent.GetCommonDataSet("select ISNULL(Active,0) as Active from tb_Product where StoreID=" + Convert.ToInt32(dsStore.Tables[0].Rows[j]["StoreID"].ToString()) + " and SKU like '%" + dsSku.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "%'");
                    //        if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    //        {
                    //            bool active = Convert.ToBoolean(dsTemp.Tables[0].Rows[0][0].ToString());
                    //            if (active == true)
                    //            {
                    //                ltrCart.Text += "<td  style='color:green;' align=\"center\" valign=\"top\">";
                    //                ltrCart.Text += "ACTIVE";
                    //            }
                    //            else
                    //            {
                    //                ltrCart.Text += "<td  style='color:red;' align=\"center\" valign=\"top\">";
                    //                ltrCart.Text += "IN-ACTIVE";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            ltrCart.Text += "<td   align=\"center\" valign=\"top\">";
                    //            ltrCart.Text += "N/A";
                    //        }
                    //        ltrCart.Text += "</td>";
                    //    }

                    //    ltrCart.Text += "</tr>";
                    //}

                }
                //ltrCart.Text += "</table>";
            }
            else
            {

            }



            if (dtmain != null && dtmain.Rows.Count > 0)
            {
                //dtmain.Columns[0].ColumnName='Ecommerce SKU';
                //dtmain.Columns[1].ColumnName='Ecommerce UPC';
                dtmain.Columns.RemoveAt(2);
                dtmain.AcceptChanges();
                DataSet dstable = new DataSet();
                dstable = CommonComponent.GetCommonDataSet("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='tb_salesSKUcompare_temp'");
                if (dstable != null && dstable.Tables.Count > 0 && dstable.Tables[0].Rows.Count > 0)
                {
                    CommonComponent.ExecuteCommonData("DROP TABLE tb_salesSKUcompare_temp");
                }
                //else
                //{
                if (dtmain != null && dtmain.Rows.Count > 0)
                {
                    string tablecreate = "create table tb_salesSKUcompare_temp(";
                    for (int j = 0; j < dtmain.Columns.Count; j++)
                    {
                        tablecreate += "[" + dtmain.Columns[j].ColumnName.ToString() + "] nvarchar(1000),";
                    }
                    tablecreate = tablecreate.Substring(0, tablecreate.Length - 1);
                    tablecreate += ")";
                    CommonComponent.ExecuteCommonData(tablecreate);
                }
                //}

                using (SqlConnection Conn = new SqlConnection(Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"])))
                {
                    if (Conn != null)
                    {
                        if (Conn.State == ConnectionState.Open)
                        {
                            Conn.Close();
                        }
                    }
                    Conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Conn))
                    {
                        for (int j = 0; j < dtmain.Columns.Count; j++)
                        {
                            bulkCopy.ColumnMappings.Add(dtmain.Columns[j].ColumnName.ToString(), dtmain.Columns[j].ColumnName.ToString());
                        }

                        bulkCopy.DestinationTableName = "tb_salesSKUcompare_temp";
                        bulkCopy.WriteToServer(dtmain);
                    }
                }
                DataSet dsload = new DataSet();
                dsload = CommonComponent.GetCommonDataSet("SELECT * FROM tb_salesSKUcompare_temp");
                if (dsload != null && dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                {
                    grditemchannel.DataSource = dsload;
                    grditemchannel.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    grditemchannel.DataSource = null;
                    grditemchannel.DataBind();
                    btnExport.Visible = false;
                }

            }
            else
            {

            }

        }
        private void GetdatafromTempTable()
        {
            DataSet dsload = new DataSet();
            if (txtSearch.Text.ToString() != "")
            {
                dsload = CommonComponent.GetCommonDataSet("SELECT * FROM tb_salesSKUcompare_temp WHERE [Ecommerce SKU] like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'");
            }
            else
            {
                dsload = CommonComponent.GetCommonDataSet("SELECT * FROM tb_salesSKUcompare_temp");
            }

            if (dsload != null && dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                if (hdncelid.Value.ToString() != "")
                {
                    DataView dv = dsload.Tables[0].DefaultView;

                    string strclmn = dsload.Tables[0].Columns[Convert.ToInt32(hdncelid.Value.ToString())].ColumnName.ToString();
                    dv.Sort = "[" + strclmn + "] " + hdnsortexpression.Value.ToString();
                    grditemchannel.DataSource = dv.ToTable();
                    grditemchannel.DataBind();
                    hdncelid.Value = "";
                    hdnsortexpression.Value = "ASC";
                }
                else
                {
                    grditemchannel.DataSource = dsload;
                    grditemchannel.DataBind();
                }



                btnExport.Visible = true;
            }
            else
            {
                grditemchannel.DataSource = null;
                grditemchannel.DataBind();
                btnExport.Visible = false;
            }
        }
        public void GetItemChannelWiseReport()
        {
            DataSet dsSku = new DataSet();
            if (txtSearch.Text != "")
            {
                dsSku = CommonComponent.GetCommonDataSet("SELECT top 10  tb_ProductVariantValue.SKU  FROM tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId=tb_ProductVariantValue.ProductID WHERE isnull(StoreId,0)=1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and isnull(tb_ProductVariantValue.SKU,'')<>'' and tb_ProductVariantValue.SKU Like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%' UNION SELECT  tb_Product.SKU FROM tb_product WHERE   isnull(Deleted,0)=0 and  tb_Product.SKU Like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'  and ProductId not in (SELECT tb_ProductVariantValue.ProductID FROM tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId=tb_ProductVariantValue.ProductID WHERE isnull(StoreId,0)=1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and isnull(tb_ProductVariantValue.SKU,'')<>'')");
            }
            else
            {
                // dsSku = objproduct.GetItemchannelReport(txtSearch.Text);

                dsSku = CommonComponent.GetCommonDataSet("SELECT top 10  tb_ProductVariantValue.SKU  FROM tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId=tb_ProductVariantValue.ProductID WHERE isnull(StoreId,0)=1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and isnull(tb_ProductVariantValue.SKU,'')<>'' UNION SELECT top 10 tb_Product.SKU FROM tb_product WHERE   isnull(Deleted,0)=0   and ProductId not in (SELECT tb_ProductVariantValue.ProductID FROM tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId=tb_ProductVariantValue.ProductID WHERE isnull(StoreId,0)=1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and isnull(tb_ProductVariantValue.SKU,'')<>'')");
                //bind();
            }

            if (dsSku != null && dsSku.Tables.Count > 0 && dsSku.Tables[0].Rows.Count > 0)
            {
                ltrCart.Text = "<table cellspacing=\"1\" cellpadding=\"2\" style=\"border-color:#E7E7E7;border-width:1px;border-style:Solid;width:100%;\">";
                ltrCart.Text += "<tbody><tr>";
                ltrCart.Text += "<th width=\"10%\">";
                ltrCart.Text += "SKU";
                ltrCart.Text += "</th>";


                DataSet dsStore = new DataSet();
                dsStore = CommonComponent.GetCommonDataSet("select * from tb_Store where isnull(Deleted,0)<>1 order by DisplayOrder");
                if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsStore.Tables[0].Rows.Count; i++)
                    {
                        ltrCart.Text += "<th align=\"left\" width=\"10%\">";
                        ltrCart.Text += dsStore.Tables[0].Rows[i]["StoreName"].ToString();
                        ltrCart.Text += "</th>";
                    }
                    ltrCart.Text += "</tr>";
                    for (int i = 0; i < dsSku.Tables[0].Rows.Count; i++)
                    {
                        ltrCart.Text += "<tr  class=\"grid_paging\">";
                        ltrCart.Text += "<td align=\"left\" valign=\"top\">";
                        ltrCart.Text += dsSku.Tables[0].Rows[i]["SKU"].ToString();
                        ltrCart.Text += "</td>";
                        for (int j = 0; j < dsStore.Tables[0].Rows.Count; j++)
                        {

                            DataSet dsTemp = new DataSet();
                            dsTemp = CommonComponent.GetCommonDataSet("select ISNULL(Active,0) as Active from tb_Product where StoreID=" + Convert.ToInt32(dsStore.Tables[0].Rows[j]["StoreID"].ToString()) + " and SKU like '%" + dsSku.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "%'");
                            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                            {
                                bool active = Convert.ToBoolean(dsTemp.Tables[0].Rows[0][0].ToString());
                                if (active == true)
                                {
                                    ltrCart.Text += "<td  style='color:green;' align=\"center\" valign=\"top\">";
                                    ltrCart.Text += "ACTIVE";
                                }
                                else
                                {
                                    ltrCart.Text += "<td  style='color:red;' align=\"center\" valign=\"top\">";
                                    ltrCart.Text += "IN-ACTIVE";
                                }
                            }
                            else
                            {
                                ltrCart.Text += "<td   align=\"center\" valign=\"top\">";
                                ltrCart.Text += "N/A";
                            }
                            ltrCart.Text += "</td>";
                        }

                        ltrCart.Text += "</tr>";
                    }

                }
                ltrCart.Text += "</table>";
            }
            else
            {

            }

        }


        public void bind()
        {
            DataSet dsStore = new DataSet();
            dsStore = CommonComponent.GetCommonDataSet("select * from tb_Store where isnull(Deleted,0)<>1 order by DisplayOrder");

        }

        protected void grditemchannel_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Attributes.Add("style", "display:none;");
                try
                {

                    string strimage = "";
                    if (hdnsortexpression.Value.ToString().ToString().ToLower() == "asc")
                    {
                        strimage = "order-date-up.png";
                    }
                    else
                    {
                        strimage = "order-date.png";
                    }

                    if (hdncelid.Value.ToString() == "2")
                    {
                        e.Row.Cells[2].Text = e.Row.Cells[2].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(2,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = e.Row.Cells[2].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(2,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "3")
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(3,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[3].Text = e.Row.Cells[3].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(3,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "4")
                    {
                        e.Row.Cells[4].Text = e.Row.Cells[4].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(4,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[4].Text = e.Row.Cells[4].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(4,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "5")
                    {
                        e.Row.Cells[5].Text = e.Row.Cells[5].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(5,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[5].Text = e.Row.Cells[5].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(5,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "6")
                    {
                        e.Row.Cells[6].Text = e.Row.Cells[6].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(6,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[6].Text = e.Row.Cells[6].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(6,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "7")
                    {
                        e.Row.Cells[7].Text = e.Row.Cells[7].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(7,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[7].Text = e.Row.Cells[7].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(7,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "8")
                    {
                        e.Row.Cells[8].Text = e.Row.Cells[8].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(8,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[8].Text = e.Row.Cells[8].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(8,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "9")
                    {
                        e.Row.Cells[9].Text = e.Row.Cells[9].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(9,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[9].Text = e.Row.Cells[9].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(9,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "10")
                    {
                        e.Row.Cells[10].Text = e.Row.Cells[10].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(10,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[10].Text = e.Row.Cells[10].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(10,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }
                    if (hdncelid.Value.ToString() == "11")
                    {

                        e.Row.Cells[11].Text = e.Row.Cells[11].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(11,'" + hdnsortexpression.Value.ToString() + "');\"><img src=\"/App_Themes/Gray/icon/" + strimage + "\" /></a>";
                    }
                    else
                    {
                        e.Row.Cells[11].Text = e.Row.Cells[11].Text.ToString() + " " + "<a href='javascript:void(0);' onclick=\"sortingfunction(11,'ASC');\"><img src=\"/App_Themes/Gray/icon/order-date-up.png\" /></a>";
                    }

                }
                catch { }

            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                e.Row.Cells[0].Attributes.Add("align", "left"); e.Row.Cells[0].Attributes.Add("valign", "top");


                //e.Row.Cells[e.Row.DataItemIndex].ForeColor = System.Drawing.Color.Green;
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text.ToLower().Trim() == "yes")
                    {
                        e.Row.Cells[i].Attributes.Add("style", "color:green");
                    }
                    else if (e.Row.Cells[i].Text.ToLower().Trim() == "no")
                    {
                        e.Row.Cells[i].Attributes.Add("style", "color:red");

                    }

                }







            }


        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ltrCart.Text = "";
            GetdatafromTempTable();
        }

        protected void btnShowall_Click(object sender, EventArgs e)
        {
            ltrCart.Text = "";
            txtSearch.Text = "";
            GetdatafromTempTable();

        }
        protected void btnnavuplod_Click(object sender, EventArgs e)
        {
            GetItemChannelWiseReportnew();
        }

        protected void btnSorting_Click(object sender, EventArgs e)
        {
            GetdatafromTempTable();
        }


    }
}