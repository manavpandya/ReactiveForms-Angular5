using LumenWorks.Framework.IO.Csv;
using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OrderShippingImport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFileName = "";
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMessage.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName);
                    //StrFileName = Server.MapPath(AppLogic.AppConfigs("CategoryImportPath") + "CategoryCSV/ImportCSV/") + StrFileName;
                    FillMapping(uploadCSV.FileName);
                    ViewState["FileName"] = uploadCSV.FileName.ToString();
                }
                else
                {
                    //lblMessage.Text = "Please upload appropriate file.";
                    //lblMessage.Style.Add("color", "#FF0000");
                    //lblMessage.Style.Add("font-weight", "normal");

                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    //if (InsertDataInDataBase(dtCSV) && lblMessage.Text == "")
                    //{
                    // contVerify.Visible = false;
                    //lblMessage.Text = "Product Imported Successfully";
                    //lblMessage.Style.Add("color", "#FF0000");
                    //lblMessage.Style.Add("font-weight", "normal");
                    lblMessage.Visible = true;
                    return;

                    //}


                }
                else
                {
                    lblMessage.Text += "Sorry file not found. Please retry uploading.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    lblMessage.Visible = true;
                }
            }
            catch { }
        }
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();

            DataTable dtTemp = new DataTable();
            DataColumn col1 = new DataColumn("ID", typeof(string));
            dtTemp.Columns.Add(col1);
            DataColumn col2 = new DataColumn("USPSAddressdetails", typeof(string));
            dtTemp.Columns.Add(col2);
            DataColumn col3 = new DataColumn("MatchOrders", typeof(string));
            dtTemp.Columns.Add(col3);
            DataColumn col4 = new DataColumn("MatchShippingdetails", typeof(string));
            dtTemp.Columns.Add(col4);
            DataColumn col5 = new DataColumn("USPSTrackingNumber", typeof(string));
            dtTemp.Columns.Add(col5);
            DataColumn col7 = new DataColumn("USPSTrackingDate", typeof(string));
            dtTemp.Columns.Add(col7);

            DataColumn col6 = new DataColumn("IsMulti", typeof(int));
            dtTemp.Columns.Add(col6);

            DataColumn col8 = new DataColumn("ShoppingCardID", typeof(int));
            dtTemp.Columns.Add(col8);

            DataColumn col9 = new DataColumn("Transactionstatus", typeof(string));
            dtTemp.Columns.Add(col9);

            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = ",";

                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();

                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "account number" || tempFieldName == "id" || tempFieldName == "tracking number" || tempFieldName == "delivery date" || tempFieldName == "destination name" || tempFieldName == "destination address" || tempFieldName == "destination city" || tempFieldName == "destination zip" || tempFieldName == "reference id")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",account number,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",reference id,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",id,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",tracking number,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery date,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",destination name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",destination address,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",destination city,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",destination zip,") > -1)
                        {

                        }
                        else
                        {
                            lblMessage.Text = "File Does not contain all columns";
                            lblMessage.Style.Add("color", "#FF0000");
                            lblMessage.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        gvOrders.DataSource = null;
                        gvOrders.DataBind();

                        DataTable dtCSV = LoadCSV(FileName);

                        for (int i = 0; i < dtCSV.Rows.Count; i++)
                        {
                            Int32 OrderNumber = 0;
                            DataSet dsOrderdetails = new DataSet();
                            string zipcode = "";
                            if (dtCSV.Rows[i]["destination zip"].ToString().IndexOf("-") > 0)
                            {
                                if (dtCSV.Rows[i]["destination zip"].ToString().Substring(dtCSV.Rows[i]["destination zip"].ToString().IndexOf("-")) == "-")
                                {
                                    zipcode = dtCSV.Rows[i]["destination zip"].ToString().Substring(0, dtCSV.Rows[i]["destination zip"].ToString().Length - 1);
                                }
                                else if (dtCSV.Rows[i]["destination zip"].ToString().Substring(dtCSV.Rows[i]["destination zip"].ToString().IndexOf("-"), 1) == "-")
                                {
                                    zipcode = dtCSV.Rows[i]["destination zip"].ToString().Substring(0, dtCSV.Rows[i]["destination zip"].ToString().IndexOf("-"));

                                }
                                else
                                {
                                    zipcode = dtCSV.Rows[i]["destination zip"].ToString();
                                }
                            }
                            else
                            {
                                zipcode = dtCSV.Rows[i]["destination zip"].ToString();
                            }

                            Int32.TryParse(dtCSV.Rows[i]["reference id"].ToString(), out OrderNumber);
                            //if (string.IsNullOrEmpty(dtCSV.Rows[i]["destination address"].ToString().Trim()) && string.IsNullOrEmpty(zipcode))
                            //{
                            //    continue;
                            //}
                            dsOrderdetails = CommonComponent.GetCommonDataSet("select isnull(TransactionStatus,'') as Transactionstatus,Ordernumber,ShoppingCardID,ShippingCity,ShippingAddress1,ShippingFirstName+' '+ShippingLastName as fname,ShippingZip,ShippingState  from tb_order where Isnull(ShippingTrackingNumber,'') = '' and Isnull(isMailSent,0) =0 and ISNULL(Deleted,0)=0  and OrderNumber=" + OrderNumber + " and Isnull(isMailSent,0) =0");
                            if (dsOrderdetails != null && dsOrderdetails.Tables.Count > 0 && dsOrderdetails.Tables[0].Rows.Count > 0)
                            {
                                Int32 Count = dsOrderdetails.Tables[0].Rows.Count;
                                for (int j = 0; j < dsOrderdetails.Tables[0].Rows.Count; j++)
                                {
                                    DataRow dr = dtTemp.NewRow();
                                    dr["ID"] = dtCSV.Rows[i]["ID"].ToString().Replace("'", "''");
                                    dr["USPSAddressdetails"] = dtCSV.Rows[i]["destination name"].ToString() + "<br />" + dtCSV.Rows[i]["destination address"].ToString() + " <br/>" + dtCSV.Rows[i]["Destination State"].ToString() + " <br/>" + dtCSV.Rows[i]["destination city"].ToString() + " <br/>" + dtCSV.Rows[i]["destination zip"].ToString();
                                    dr["MatchOrders"] = dsOrderdetails.Tables[0].Rows[j]["Ordernumber"].ToString();
                                    dr["MatchShippingdetails"] = dsOrderdetails.Tables[0].Rows[j]["fname"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingAddress1"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingState"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingCity"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingZip"].ToString();
                                    dr["USPSTrackingNumber"] = Convert.ToString(dtCSV.Rows[i]["Tracking Number"].ToString().Replace("'", ""));
                                    if (string.IsNullOrEmpty(dtCSV.Rows[i]["Delivery Date"].ToString()))
                                    {
                                        dr["USPSTrackingDate"] = "";
                                    }
                                    else
                                    {
                                        dr["USPSTrackingDate"] = dtCSV.Rows[i]["Delivery Date"].ToString();
                                    }

                                    if (Count > 1)
                                    {
                                        dr["IsMulti"] = 1;
                                    }
                                    else
                                    {
                                        dr["IsMulti"] = 0;
                                    }
                                    dr["ShoppingCardID"] = dsOrderdetails.Tables[0].Rows[j]["ShoppingCardID"].ToString();
                                    dr["Transactionstatus"] = dsOrderdetails.Tables[0].Rows[j]["Transactionstatus"].ToString();
                                    dtTemp.Rows.Add(dr);
                                }



                            }
                            else
                            {
                                dsOrderdetails = CommonComponent.GetCommonDataSet("select isnull(TransactionStatus,'') as Transactionstatus,Ordernumber,ShoppingCardID,ShippingCity,ShippingAddress1,ShippingFirstName+' '+ShippingLastName as fname,ShippingZip,ShippingState  from tb_order where Isnull(ShippingTrackingNumber,'') = '' and Isnull(isMailSent,0) =0 and ISNULL(Deleted,0)=0  and ShippingFirstName+' '+ShippingLastName='" + dtCSV.Rows[i]["destination name"].ToString().Replace("'", "''") + "' and ShippingAddress1 like '%" + dtCSV.Rows[i]["destination address"].ToString().Replace("'", "''") + "%' AND ShippingCity='" + dtCSV.Rows[i]["destination city"].ToString().Replace("'", "''") + "' and ShippingZip='" + zipcode + "' and Isnull(isMailSent,0) =0");
                                if (dsOrderdetails != null && dsOrderdetails.Tables.Count > 0 && dsOrderdetails.Tables[0].Rows.Count > 0)
                                {
                                    Int32 Count = dsOrderdetails.Tables[0].Rows.Count;
                                    for (int j = 0; j < dsOrderdetails.Tables[0].Rows.Count; j++)
                                    {
                                        DataRow dr = dtTemp.NewRow();
                                        dr["ID"] = dtCSV.Rows[i]["ID"].ToString();
                                        dr["USPSAddressdetails"] = dtCSV.Rows[i]["destination name"].ToString() + "<br />" + dtCSV.Rows[i]["destination address"].ToString() + " <br/>" + dtCSV.Rows[i]["Destination State"].ToString() + " <br/>" + dtCSV.Rows[i]["destination city"].ToString() + " <br/>" + dtCSV.Rows[i]["destination zip"].ToString();
                                        dr["MatchOrders"] = dsOrderdetails.Tables[0].Rows[j]["Ordernumber"].ToString();
                                        dr["MatchShippingdetails"] = dsOrderdetails.Tables[0].Rows[j]["fname"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingAddress1"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingState"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingCity"].ToString() + "<br />" + dsOrderdetails.Tables[0].Rows[j]["ShippingZip"].ToString();
                                        dr["USPSTrackingNumber"] = Convert.ToString(dtCSV.Rows[i]["Tracking Number"].ToString().Replace("'", ""));
                                        if (string.IsNullOrEmpty(dtCSV.Rows[i]["Delivery Date"].ToString()))
                                        {
                                            dr["USPSTrackingDate"] = "";
                                        }
                                        else
                                        {
                                            dr["USPSTrackingDate"] = dtCSV.Rows[i]["Delivery Date"].ToString();
                                        }

                                        if (Count > 1)
                                        {
                                            dr["IsMulti"] = 1;
                                        }
                                        else
                                        {
                                            dr["IsMulti"] = 0;
                                        }
                                        dr["ShoppingCardID"] = dsOrderdetails.Tables[0].Rows[j]["ShoppingCardID"].ToString();
                                        dr["Transactionstatus"] = dsOrderdetails.Tables[0].Rows[j]["Transactionstatus"].ToString();
                                        dtTemp.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        if (dtTemp.Rows.Count > 0)
                        {
                            trBottom.Visible = true;
                            btnSave.Visible = true;
                        }
                        gvOrders.DataSource = dtTemp;
                        gvOrders.DataBind();



                    }
                    else
                    {
                        lblMessage.Text = "Please Specify Account Number,ID,Reference ID,Tracking number,delivery date,destination name,destination address,destination city,destination zip, in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }




                }
                else
                {
                    lblMessage.Text = "Please Specify Account Number,ID,Reference ID,Tracking number,delivery date,destination name,destination address,destination city,destination zip, in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                DataColumn columnID = new DataColumn();
                columnID.Caption = "Number";
                columnID.ColumnName = "Number";
                columnID.AllowDBNull = false;
                columnID.AutoIncrement = true;
                columnID.AutoIncrementSeed = 1;
                columnID.AutoIncrementStep = 1;
                dtCSV.Columns.Add(columnID);
                foreach (string FieldName in FieldNames)
                    dtCSV.Columns.Add(FieldName);
                while (csv.ReadNextRecord())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < FieldCount; i++)
                    {
                        string FieldName = FieldNames[i];
                        if (!dr.Table.Columns.Contains(FieldName))
                        { continue; }

                        dr[FieldName] = csv[i];
                    }
                    dtCSV.Rows.Add(dr);
                }
                dtCSV.AcceptChanges();
                return dtCSV;
            }
        }

        protected void gvOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal lblordernumber = (Literal)e.Row.FindControl("lblordernumber");
                HiddenField hdnmulti = (HiddenField)e.Row.FindControl("hdnmulti");
                if (hdnmulti.Value.ToString() == "1")
                {
                    e.Row.Attributes.Add("style", "color:#ff0000 !important;");
                }
                lblordernumber.Text = "<a href='/Admin/Orders/Orders.aspx?id=" + lblordernumber.Text.ToString() + "'>" + lblordernumber.Text.ToString() + "</a>";
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow gr in gvOrders.Rows)
            {
                HiddenField hdnProductID = (HiddenField)gr.FindControl("hdnProductID");
                HiddenField hdndate = (HiddenField)gr.FindControl("hdndate");
                HiddenField hdnShoppingCardID = (HiddenField)gr.FindControl("hdnShoppingCardID");
                Literal lbltrackingInfo = (Literal)gr.FindControl("lbltrackingInfo");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked)
                {
                    if (hdndate.Value.ToString().Trim() != "")
                    {
                        CommonComponent.ExecuteCommonData("INSERT INTO tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,ProductName) SELECT " + hdnProductID.Value.ToString() + ",RefProductID,'" + lbltrackingInfo.Text + "','USPS',1,'" + hdndate.Value.ToString().Trim() + "',Quantity,ProductName FROM tb_OrderedShoppingCartItems WHERE OrderedShoppingCartID=" + hdnShoppingCardID.Value.ToString() + "");
                        CommonComponent.ExecuteCommonData("UPDATE TB_ORDER SET isMailSent=0,IsBackShip=1,OrderStatus='Shipped',ShippingTrackingNumber='" + lbltrackingInfo.Text.ToString() + "',ShippedVIA='USPS',ShippedOn='" + hdndate.Value.ToString().Trim() + "' WHERE OrderNumber=" + hdnProductID.Value.ToString() + "");
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("INSERT INTO tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedQty,ProductName) SELECT " + hdnProductID.Value.ToString() + ",RefProductID,'" + lbltrackingInfo.Text + "','USPS',1,Quantity,ProductName FROM tb_OrderedShoppingCartItems WHERE OrderedShoppingCartID=" + hdnShoppingCardID.Value.ToString() + "");
                        CommonComponent.ExecuteCommonData("UPDATE TB_ORDER SET isMailSent=0,IsBackShip=1,OrderStatus='Shipped',ShippingTrackingNumber='" + lbltrackingInfo.Text.ToString() + "',ShippedVIA='USPS'  WHERE OrderNumber=" + hdnProductID.Value.ToString() + "");
                    }
                }





            }
            if (ViewState["FileName"] != null)
            {

                FillMapping(ViewState["FileName"].ToString());
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "successmsg", "jAlert('Shipping info. imported successfully for selected order.','Success');", true);
            }
        }
    }
}