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
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class WareHousePO : BasePage
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
                Bindstore();
                if (ddlStore.SelectedIndex == 0)
                {
                    AppConfig.StoreID = 1;
                }
                else
                {
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                }
                btnGenerateMultiPlePO.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/generate-purchase-order.gif";
                btnGenerateVendorQuotePO.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/generate-po-from-quote.gif";
                btnGenerateVendorQuotePOtop.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/generate-po-from-quote.gif";
                btnRequestVendorQuote.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/request-new-quotes.gif";
                btnAddManualQuote.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-manual-quote.gif";
                btnRefreshQuote.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/refresh-quote.gif";

                BindData();
                bindOldPo();
                BindRequestQuoteDetails();
                lnkAddNew.Attributes.Add("onclick", "OpenCenterWindow('PopUpProduct.aspx?StoreID=" + ddlStore.SelectedValue + "',900,600);");
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void Bindstore()
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
            if (!string.IsNullOrEmpty(AppConfig.StoreID.ToString()))
                ddlStore.SelectedValue = AppConfig.StoreID.ToString();
            else
                ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Warehouse Product Data
        /// </summary>
        protected void BindData()
        {
            DataSet dsWarehouse = new DataSet();
            WarehouseComponent objWarehouse = new WarehouseComponent();
            dsWarehouse = objWarehouse.GetWarehouseProductListByStoreID(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsWarehouse != null && dsWarehouse.Tables.Count > 0 && dsWarehouse.Tables[0].Rows.Count > 0)
            {
                grdCustomer.DataSource = dsWarehouse;
                grdCustomer.DataBind();
                btnGenerateMultiPlePO.Visible = true;
                btnRequestVendorQuote.Visible = true;
                btnAddManualQuote.Visible = true;
                btnRefreshQuote.Visible = true;
                trCheckClearAll.Visible = true;
            }
            else
            {
                grdCustomer.DataSource = null;
                grdCustomer.DataBind();
                btnGenerateMultiPlePO.Visible = false;
                btnRequestVendorQuote.Visible = false;
                btnAddManualQuote.Visible = false;
                btnRefreshQuote.Visible = false;
                trCheckClearAll.Visible = false;
            }
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int gCustomerID = Convert.ToInt32(hdnCustDelete.Value);
            if (gCustomerID > 0)
            {
                CommonComponent.ExecuteCommonData("Delete From tb_WareHouseproduct where WareHouseId=" + gCustomerID + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
                BindData();
                hdnCustDelete.Value = "0";
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                return;
            }
        }

        /// <summary>
        /// Customer Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }

        /// <summary>
        ///  Generate Multi PO Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGenerateMultiPlePO_Click(object sender, ImageClickEventArgs e)
        {
            string Pids = "";
            int Cnt = 0;
            if (grdCustomer.Rows.Count > 0)
            {
                for (int i = 0; i < grdCustomer.Rows.Count; i++)
                {
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdCustomer.Rows[i].FindControl("chkSelect")).Checked);
                    if (chkSelect)
                    {
                        int ProductId = Convert.ToInt32(((Label)grdCustomer.Rows[i].FindControl("lblProductID")).Text.ToString());
                        Pids += ProductId + ",";
                        Cnt++;
                    }
                }
            }
            if (Cnt == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Select Product to Generate PO.!');", true);
                return;
            }
            // Take Order number after its code
            Response.Redirect("WarehouseGeneratePO.aspx?PId=" + Pids);
        }


        /// <summary>
        /// Bind Old PO Records
        /// </summary>
        private void bindOldPo()
        {
            DataSet DsOldPOrder = new DataSet();
            Int32 ONo = 0;
            //DsOldPOrder = CommonComponent.GetCommonDataSet("select convert(varchar(max),round(isnull(po.Adjustments,0),2)) as Adjustments,convert(varchar(max),round(isnull(po.Tax,0),2)) as Tax,convert(varchar(max),round(isnull(po.Shipping,0),2)) as Shipping,convert(varchar(max),round(isnull(po.additionalcost,0),2)) as additionalcost, convert(varchar(max),round(isnull(po.poamount,0),2)) as poamount,po.PONumber,po.VendorID,po.PODate,po.OrderNumber,v.Name  from tb_PurchaseOrder po  INNER JOIN tb_Vendor v ON po.VendorID = v.VendorID  where po.OrderNumber = " + ONo + " order by po.ponumber desc");
            DsOldPOrder = CommonComponent.GetCommonDataSet("select convert(varchar(max),round(isnull(po.Adjustments,0),2)) as Adjustments, " +
                                        "convert(varchar(max),round(isnull(po.Tax,0),2)) as Tax,  convert(varchar(max),round(isnull(po.Shipping,0),2)) as Shipping, " +
                                        "convert(varchar(max),round(isnull(po.additionalcost,0),2)) as additionalcost,   convert(varchar(max),round(isnull(po.poamount,0),2)) as poamount, " +
                                        "po.PONumber,po.VendorID,po.PODate,po.OrderNumber,v.Name  from tb_PurchaseOrder po  " +
                                        "LEFT OUTER JOIN tb_Vendor v ON po.VendorID = v.VendorID  " +
                                        "where po.OrderNumber = " + ONo + " and PONumber in (Select ISNULL(PONumber,0) from tb_PurchaseOrderItems  " +
                                        "where ProductID in (SELECT isnull(ProductID,0) FROM tb_Product WHERE Storeid=" + ddlStore.SelectedValue + "))  " +
                                        "order by po.ponumber desc");
            if (DsOldPOrder != null && DsOldPOrder.Tables[0].Rows.Count > 0)
            {
                gvOldPOrder.DataSource = DsOldPOrder;
                gvOldPOrder.DataBind();
                gvOldPOrder.Visible = true;
                lblPoOrdrs.Visible = true;

                Decimal TotalPOAmount = 0;
                //Decimal TotalClearPOAmount = objOldPOrder.GetClearAmountforOrder(ONo);
                Decimal TotalClearPOAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(sum(isnull(paidamount,0)),0) from tb_vendorpayment  " +
                                                                    " where VendorPaymentID in( " +
                                                                    " select VendorPaymentID from tb_vendorpaymentPurchaseOrder " +
                                                                    " where ponumber in (select ponumber from tb_purchaseorder " +
                                                                    " where ordernumber=" + ONo + "))"));
                Decimal TotalUnClearPOAmount = 0;
                foreach (DataRow dr in DsOldPOrder.Tables[0].Rows)
                {
                    TotalPOAmount += Convert.ToDecimal(dr["POAmount"].ToString());
                }
                TotalUnClearPOAmount = TotalPOAmount - TotalClearPOAmount;
                ltClearPOAmt.Text = "<b>Clear Amount </b>:$" + TotalClearPOAmount.ToString("f2") + "<br />";
                ltUnClearPOAmt.Text = "<b>UnClear Amount </b>:$" + TotalUnClearPOAmount.ToString("f2") + "<br />";
                ltTotalPOAmt.Text = "<b>Total PO Amount </b>:$" + TotalPOAmount.ToString("f2") + "<br />";
                trOldPO.Visible = true;
            }
            else
            {
                gvOldPOrder.Visible = false;
                lblPoOrdrs.Visible = false;
                ltClearPOAmt.Text = "";
                ltUnClearPOAmt.Text = "";
                ltTotalPOAmt.Text = "";
                trOldPO.Visible = false;
            }
        }

        #region Set Values

        /// <summary>
        /// Checks the pending Shipping Details
        /// </summary>
        /// <param name="pono">string pono</param>
        /// <returns>Returns the Link if not pending else Blank</returns>
        public String CheckPending(String pono)
        {
            String Result = "";
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(1,0) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(IsShipped,0)=0")) > 0)
            {
                Result = "<a  href=\"/WarhouseVendorProducts.aspx?pono=" + pono.Trim() + "\" target=\"_blank\" STYLE='COLOR:#212121;text-decoration:underline;'>Click Here</a>";
            }
            else
            { Result = ""; }
            return Result;
        }

        /// <summary>
        /// Checks the download PDF
        /// </summary>
        /// <param name="pono">string pono</param>
        /// <returns>Returns download link</returns>
        public string CheckDownloadPdf(string pono)
        {
            //string pdfdir = Server.MapPath("~/Resources/POFiles/");
            string pdfdir = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));

            string Result = "";

            string[] filePaths = Directory.GetFiles(pdfdir, "*.pdf");

            foreach (string filecheck in filePaths)
            {
                if (filecheck.Contains(pono))
                {

                    Result = "<a  href=\"/DownloadPDFRecord.aspx?pono=" + pono.Trim() + "\" target=\"_blank\" STYLE='COLOR:#212121;text-decoration:underline;'>Download PDF</a>";
                }
            }

            return Result;
        }

        /// <summary>
        /// Checks the Pending New
        /// </summary>
        /// <param name="pono">string pono</param>
        /// <returns>Returns the Shipping Status</returns>
        public String CheckPendingNew(String pono)
        {
            String Result = "";
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(1,0) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(IsShipped,0)=0")) > 0)
            {
                int CountToal = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ponumber) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(IsShipped,0)=1"));
                if (CountToal > 0)
                {
                    int CountToalnew = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ponumber) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(Ispaid,0)=1"));
                    if (CountToalnew > 0)
                    {
                        Result = "Partially Paid";
                    }
                    else
                    {
                        Result = "Partially Shipped";
                    }
                }
            }
            else
            {
                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(1,0) from  tb_PurchaseOrderItems where ponumber=" + pono.Trim() + " and isnull(IsPaid,0)=1")) > 0)
                {
                    Result = "Paid";
                }
                else
                {
                    Result = "Shipped";
                }
            }
            return Result;
        }

        /// <summary>
        /// Makes the Positive
        /// </summary>
        /// <param name="Adjustments">string  Adjustments</param>
        /// <returns>Returns  the positive string value</returns>
        public String MakePositive(string Adjustments)
        {
            try
            {
                Decimal Adjust = Convert.ToDecimal(Adjustments);
                if (Adjust < 0)
                    return "$" + (-Adjust) + "(-)";
                else if (Adjust == 0)
                    return "$0.00";
                else
                    return "$" + (Adjust) + "(+)";

            }
            catch { return "$0.00"; }
        }
        #endregion

        /// <summary>
        /// Binds the request quote details.
        /// </summary>
        private void BindRequestQuoteDetails()
        {
            DataSet DsRequestQuote = new DataSet();
            string StrQuery = " Select distinct p.ProductID,p.Name,p.SKU,ISNULL(p.Inventory,0) as Inventory from tb_VendorQuoteRequestDetails as Vreq " +
                               " inner join tb_Product p ON Vreq.ProductID = p.ProductID " +
                               " Where ISNULL(p.Deleted,0)=0 and ISNULL(p.Active,1)=1 " +
                               "and ISNULL(Vreq.VendorQuoteReqDetailsID,0) not in (Select ISNULL(VendorQuoteReqDetailsID,0) from tb_PurchaseOrderItems Where Vreq.ProductId =tb_PurchaseOrderItems.ProductID) ";
            DsRequestQuote = CommonComponent.GetCommonDataSet(StrQuery.ToString());
            if (DsRequestQuote != null && DsRequestQuote.Tables.Count > 0 && DsRequestQuote.Tables[0].Rows.Count > 0)
            {
                grvRequestedQuote.DataSource = DsRequestQuote;
                grvRequestedQuote.DataBind();
                btnGenerateVendorQuotePO.Visible = true;
                btnGenerateVendorQuotePOtop.Visible = true;
            }
            else
            {
                grvRequestedQuote.DataSource = null;
                grvRequestedQuote.DataBind();
                btnGenerateVendorQuotePO.Visible = false;
                btnGenerateVendorQuotePOtop.Visible = false;
            }
        }

        /// <summary>
        /// Old PO Order Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvOldPOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                System.Web.UI.WebControls.GridViewRow row = (System.Web.UI.WebControls.GridViewRow)(((System.Web.UI.WebControls.LinkButton)e.CommandSource).NamingContainer);
                System.Web.UI.HtmlControls.HtmlInputHidden hdnpo = (System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hdnpo");
                string strSql = "delete from tb_PurchaseOrderItems where PoNumber=" + Convert.ToInt32(hdnpo.Value);
                CommonComponent.ExecuteCommonData(strSql.ToString());
                strSql = "delete from tb_PurchaseOrder where PoNumber=" + Convert.ToInt32(hdnpo.Value);
                CommonComponent.ExecuteCommonData(strSql.ToString());
                bindOldPo();
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('PO Deleted Successfully..');", true);
            }
        }

        /// <summary>
        /// Download Files at Specified File Path
        /// </summary>
        /// <param name="filepath">string filepath</param>
        private void downloadfile(string filepath)
        {
            try
            {
                FileInfo file = new FileInfo(Server.MapPath(filepath));
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);

                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);
                }
            }
            catch { }
            Response.End();



        }

        /// <summary>
        /// Old PO Order Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvOldPOrder_RowEditing(object sender, GridViewEditEventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlInputHidden hdnpo = (System.Web.UI.HtmlControls.HtmlInputHidden)gvOldPOrder.Rows[e.NewEditIndex].FindControl("hdnpo");
            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Warehousemail WHERE Ponumber=" + hdnpo.Value + "");
            if (dsMail.Tables[0].Rows.Count > 0)
            {
                System.Net.Mail.AlternateView av = System.Net.Mail.AlternateView.CreateAlternateViewFromString(dsMail.Tables[0].Rows[0]["Body"].ToString(), null, "text/html");
                if (!String.IsNullOrEmpty(dsMail.Tables[0].Rows[0]["Filepath"].ToString()))
                {
                    try
                    {
                        if (System.IO.File.Exists(Server.MapPath(dsMail.Tables[0].Rows[0]["Filepath"].ToString())))
                        {
                            CommonOperations.SendMailAttachment(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av, Server.MapPath(dsMail.Tables[0].Rows[0]["Filepath"].ToString()).ToString());
                            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully..');", true);
                        }
                    }
                    catch
                    {
                        CommonOperations.SendMail(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av);
                        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully..');", true);
                    }
                }
                else
                {
                    CommonOperations.SendMail(dsMail.Tables[0].Rows[0]["EmailFrom"].ToString(), dsMail.Tables[0].Rows[0]["EmailTo"].ToString().Replace(",", ";"), dsMail.Tables[0].Rows[0]["EmailCC"].ToString(), dsMail.Tables[0].Rows[0]["EMailBCC"].ToString(), dsMail.Tables[0].Rows[0]["Subject"].ToString(), dsMail.Tables[0].Rows[0]["Body"].ToString(), Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Mail has been sent successfully..');", true);
                }
            }
        }

        /// <summary>
        ///  Customer Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomer.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        ///  Old PO Order Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvOldPOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOldPOrder.PageIndex = e.NewPageIndex;
            bindOldPo();
        }

        /// <summary>
        /// Old PO Order Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvOldPOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden hdnpo = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnpo");
                Literal ltrpdfdownloadcheck = (Literal)e.Row.FindControl("ltrpdfdownload");
                Label lblnamecheck = (Label)e.Row.FindControl("lblVName");
                Label lblRegDetailNo = (Label)e.Row.FindControl("lblRegDetailNo");

                string namecheck = lblnamecheck.Text;
                string povaluecheck = hdnpo.Value;

                string StrQru = "Select distinct cast(VendorQuoteReqDetailsID AS varchar(max)) + ',' from tb_PurchaseOrderItems where PONumber = " + hdnpo.Value.ToString() + " for XMl path ('')";
                string StrReqDetailValue = Convert.ToString(CommonComponent.GetScalarCommonData(StrQru.ToString()));

                if (!string.IsNullOrEmpty(StrReqDetailValue.ToString()) && StrReqDetailValue.Length > 0)
                {
                    StrReqDetailValue = StrReqDetailValue.Substring(0, StrReqDetailValue.Length - 1);
                    lblRegDetailNo.Text = StrReqDetailValue.ToString();
                }
                else { lblRegDetailNo.Text = "-"; }

                //Get PDF File according the porder
                //string pdfdir = Server.MapPath("~/Resources/POFiles/");
                string pdfstrpath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
                string pdfdir = Server.MapPath(pdfstrpath);

                string[] filePaths = Directory.GetFiles(pdfdir, "*.pdf");
                string Links = string.Empty;
                int intrpcount = 0;
                string Atag = "";
                foreach (string filecheck in filePaths)
                {
                    string filename = string.Empty;
                    filename = filecheck.Replace(Server.MapPath(pdfstrpath).ToString(), "");

                    if ((filecheck.ToLower().Contains("po_" + povaluecheck + ".") || filecheck.ToLower().Contains("po_" + povaluecheck + "_")) && lblnamecheck.Text == "Shelter Logic/ Shelterworks" && (filename.ToLower().Contains("po_" + povaluecheck + ".") || filename.ToLower().Contains("po_" + povaluecheck + "_")))
                    {

                        intrpcount++;
                        LinkButton lfile = new LinkButton();
                        //string filename = string.Empty;
                        //filename = filecheck.Replace(Server.MapPath("/Resources/POFiles/").ToString(), "");
                        string pdfacutalpath = Server.MapPath(filename).ToString();

                        Links = Links +
                        "<input style='display:none;' type='submit' runat='server' value='" + pdfacutalpath + "' name='btnDownload-main-" + intrpcount + "'  id='btnDownload-main-" + intrpcount + "' onclick='submit' /> <a href='javascript:void(0);' onclick=\"pdffiledownloa('" + filename + "');\">" + filename + "</a> <br/>";

                        Atag += " <a href='javascript:void(0);' onclick=\"pdffiledownloa('" + filename + "');\">" + filename + "</a> <br/>";

                        LinkButton lnkresend = (LinkButton)e.Row.FindControl("lnkResend");
                        lnkresend.Visible = false;

                    }
                    else if ((filecheck.ToLower().Contains("po_" + povaluecheck + ".") || filecheck.ToLower().Contains("po_" + povaluecheck + "_")) && (filename.ToLower().Contains("po_" + povaluecheck + ".") || filename.ToLower().Contains("po_" + povaluecheck + "_")))
                    {
                        intrpcount++;
                        LinkButton lfile = new LinkButton();
                        //string filename = string.Empty;
                        //filename = filecheck.Replace(Server.MapPath("/Resources/POFiles/").ToString(), "");
                        string pdfacutalpath = Server.MapPath(filename).ToString();
                        Links = Links +
                        "<input style='display:none;' type='submit' runat='server' value='" + pdfacutalpath + "' name='btnDownload-main-" + intrpcount + "'  id='btnDownload-main-" + intrpcount + "' onclick='submit' /> <a href='javascript:void(0);' onclick=\"pdffiledownloa('" + filename + "');\">" + filename + "</a> <br/>";
                        Atag += " <a href='javascript:void(0);' onclick=\"pdffiledownloa('" + filename + "');\">" + filename + "</a><br/>";
                    }
                }
                ltrpdfdownloadcheck.Text = Atag;
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            lnkAddNew.Attributes.Add("onclick", "OpenCenterWindow('PopUpProduct.aspx?StoreID=" + ddlStore.SelectedValue + "',900,600);");
            BindData();
            bindOldPo();
        }

        /// <summary>
        ///  Download File Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDownloadfile_Click(object sender, EventArgs e)
        {
            downloadfile(Convert.ToString(AppLogic.AppConfigs("POFilesPath")) + hdnfilename.Value.ToString());
            hdnfilename.Value = "";
        }

        /// <summary>
        ///  Request Vendor Quote Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRequestVendorQuote_Click(object sender, ImageClickEventArgs e)
        {
            string Pids = "";
            int Cnt = 0;
            if (grdCustomer.Rows.Count > 0)
            {
                for (int i = 0; i < grdCustomer.Rows.Count; i++)
                {
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdCustomer.Rows[i].FindControl("chkSelect")).Checked);
                    if (chkSelect)
                    {
                        int ProductId = Convert.ToInt32(((Label)grdCustomer.Rows[i].FindControl("lblProductID")).Text.ToString());
                        Pids += ProductId + ",";
                        Cnt++;
                    }
                }
            }
            if (Cnt == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Select Product to Generate Vendor Quote!');", true);
                return;
            }
            // Take Order number after its code
            Response.Redirect("VendorQuote.aspx?PId=" + Pids);
        }

        /// <summary>
        /// Requested Quote Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvRequestedQuote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                GridView grvQuoteReplyDetails = (GridView)e.Row.FindControl("grvQuoteReplyDetails");
                //Label lblProductOption = (Label)e.Row.FindControl("lblProductOption");
                Literal ltrProOption = (Literal)e.Row.FindControl("ltrProOption");

                //if (lblProductOption != null && lblProductOption.Text.ToString() != "")
                //{
                //    ltrProOption.Text = " &nbsp;Options : " + lblProductOption.Text.ToString();
                //}

                if (lblProductID != null)
                {
                    String StrQuery = "SELECT tb_VendorQuoteRequestDetails.VendorQuoteReqDetailsID,dbo.tb_VendorQuoteRequestDetails.VendorID, dbo.tb_VendorQuoteRequestDetails.VendorQuoteRequestID, dbo.tb_VendorQuoteRequestDetails.ProductId, " +
                        " dbo.tb_Vendor.Name AS VendorName, dbo.tb_VendorQuoteRequest.VendorQuoteRequestID AS VendorQuoteID, dbo.tb_VendorQuoteRequest.RequestedOn,  " +
                        " ISNULL(dbo.tb_VendorQuoteReply.VendorQuoteReplyID, 0) AS VendorQuoteReplyID, ISNULL(dbo.tb_VendorQuoteReply.RefProductID, 0) AS RefProductID, " +
                        " ISNULL(dbo.tb_VendorQuoteRequestDetails.Quantity, 0) AS RequestedQuantity, ISNULL(dbo.tb_VendorQuoteReply.Quantity, 0) AS QuoteQuantity,  " +
                        " ISNULL(dbo.tb_VendorQuoteReply.Price, 0) AS Price, dbo.tb_VendorQuoteReply.QuoteGivenOn, CASE WHEN ISNULL(dbo.tb_VendorQuoteReply.AvailableDays, '0') = 0 THEN '-' ELSE dbo.tb_VendorQuoteReply.AvailableDays END AS AvailableDays, ISNULL(dbo.tb_VendorQuoteReply.IsShipped, 0) AS IsShipped,  " +
                        " CASE WHEN ISNULL(dbo.tb_VendorQuoteReply.Notes, '') = '' THEN '-' ELSE dbo.tb_VendorQuoteReply.Notes END AS Notes, CASE WHEN ISNULL(dbo.tb_VendorQuoteReply.Location, '') = '' THEN '-' ELSE dbo.tb_VendorQuoteReply.Location END AS Location,  " +
                        " dbo.tb_VendorQuoteReply.ProductName, dbo.tb_VendorQuoteReply.ProductOption, dbo.tb_VendorQuoteRequestDetails.MailLogid " +
                    " FROM dbo.tb_VendorQuoteRequestDetails INNER JOIN " +
                       "  dbo.tb_Vendor ON dbo.tb_VendorQuoteRequestDetails.VendorID = dbo.tb_Vendor.VendorID INNER JOIN " +
                       "  dbo.tb_VendorQuoteRequest ON dbo.tb_VendorQuoteRequestDetails.VendorQuoteRequestID = dbo.tb_VendorQuoteRequest.VendorQuoteRequestID " +
                       "  LEFT OUTER JOIN dbo.tb_VendorQuoteReply ON dbo.tb_VendorQuoteRequestDetails.VendorQuoteRequestID = dbo.tb_VendorQuoteReply.VendorQuoteRequestID " +
                       "  and tb_VendorQuoteReply.RefProductID=tb_VendorQuoteRequestDetails.ProductId and tb_VendorQuoteReply.VendorID=tb_VendorQuoteRequestDetails.VendorID " +
                       "  WHERE (dbo.tb_VendorQuoteRequestDetails.ProductId = " + lblProductID.Text.ToString() + ") and tb_VendorQuoteRequestDetails.VendorQuoteReqDetailsID not in (Select ISNULL(VendorQuoteReqDetailsID,0) from tb_PurchaseOrderItems)";

                    if (grvQuoteReplyDetails != null)
                    {
                        DataSet dsGrdView = new DataSet();
                        dsGrdView = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                        if (dsGrdView != null && dsGrdView.Tables.Count > 0 && dsGrdView.Tables[0].Rows.Count > 0)
                        {
                            grvQuoteReplyDetails.DataSource = dsGrdView;
                            grvQuoteReplyDetails.DataBind();
                        }
                        else
                        {
                            grvQuoteReplyDetails.DataSource = null;
                            grvQuoteReplyDetails.DataBind();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Quote Reply Details Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvQuoteReplyDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblVendorQuoteReplyID = (Label)e.Row.FindControl("lblVendorQuoteReplyID");
                Label lblVendorQuoteID = (Label)e.Row.FindControl("lblVendorQuoteID");
                Literal ltrRequestNo = (Literal)e.Row.FindControl("ltrRequestNo");
                TextBox txtPOQty = (TextBox)e.Row.FindControl("txtPOQty");
                TextBox txtQuotedPrice = (TextBox)e.Row.FindControl("txtQuotedPrice");
                Label lblQuotedPrice = (Label)e.Row.FindControl("lblQuotedPrice");
                Label lblPOQty = (Label)e.Row.FindControl("lblPOQty");
                Label lblVendorID = (Label)e.Row.FindControl("lblVendorID");
                Label lblQuoteGivenOn = (Label)e.Row.FindControl("lblQuoteGivenOn");

                Label lblMailLogid = (Label)e.Row.FindControl("lblMailLogid");
                Literal ltrResendMail = (Literal)e.Row.FindControl("ltrResendMail");

                if (lblVendorQuoteReplyID != null && !string.IsNullOrEmpty(lblVendorQuoteReplyID.Text.ToString()) && Convert.ToInt32(lblVendorQuoteReplyID.Text) == 0)
                {
                    lblQuotedPrice.Text = "-";
                    txtQuotedPrice.Visible = false;
                    lblQuotedPrice.Visible = true;

                    lblPOQty.Text = "-";
                    lblPOQty.Visible = true;
                    txtPOQty.Visible = false;

                    lblQuoteGivenOn.Text = "-";
                }

                if (lblMailLogid != null && !string.IsNullOrEmpty(lblMailLogid.Text) && Convert.ToInt32(lblMailLogid.Text.ToString()) > 0)
                {
                    ltrResendMail.Text = "<a href=\"javascript:void(0)\" onclick=\"openCenteredWindow('VendorQuoteResendmail.aspx?qoute=1&Miallogid=" + lblMailLogid.Text.ToString() + "');\">Resend</a>";
                }
                if (lblVendorQuoteID != null && ltrRequestNo != null)
                {
                    ltrRequestNo.Text = "<a href='javascript:void(0);' onclick=\"window.open('VendorQuoteRequestPreview.aspx?VendorQuoteID=" + lblVendorQuoteID.Text.ToString() + "&VendorID=" + lblVendorID.Text.ToString() + "&ProductId=" + lblProductID.Text.ToString() + "','','width=700px,height=600px');return false;\" >View Details</a>";
                }
            }
        }

        /// <summary>
        /// Quote Reply Details Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grvQuoteReplyDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    GridViewRow Gv2Row = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                    ImageButton lbl = (ImageButton)(Gv2Row.FindControl("btndel"));
                    Label lblProductId = (Label)Gv2Row.FindControl("lblProductID");
                    Label lblVendorQuoteReplyID = (Label)Gv2Row.FindControl("lblVendorQuoteReplyID");
                    Label lblVendorQuoteID = (Label)Gv2Row.FindControl("lblVendorQuoteID");
                    Label lblVendorID = (Label)Gv2Row.FindControl("lblVendorID");
                    int i = Convert.ToInt32(e.CommandArgument.ToString());

                    CommonComponent.ExecuteCommonData("Delete from tb_VendorQuoteRequestDetails where VendorQuoteRequestID= " + lblVendorQuoteID.Text.ToString() + " and VendorID=" + lblVendorID.Text.ToString() + " and ProductId=" + lblProductId.Text.ToString() + "");
                    if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_VendorQuoteRequestDetails where VendorQuoteRequestID=" + lblVendorQuoteID.Text.ToString() + "")) == 0)
                    {
                        CommonComponent.ExecuteCommonData("Delete from tb_VendorQuoteRequest where VendorQuoteRequestID= " + lblVendorQuoteID.Text.ToString() + "");
                    }
                    CommonComponent.ExecuteCommonData("Delete from tb_VendorQuoteReply where VendorQuoteReplyID= " + lblVendorQuoteReplyID.Text.ToString() + "");

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@DeleteQuote", "jAlert('Quote Deleted Successfully!','Message');", true);
                    BindRequestQuoteDetails();
                }
            }
            catch { }
        }

        /// <summary>
        ///  Generate Vendor PO Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGenerateVendorQuotePO_Click(object sender, ImageClickEventArgs e)
        {
            bool QtyFlag = false;

            DataTable dtCart = new DataTable("QuoteOrderCart");
            Session["IgnorvID"] = null;
            Session["OldVendor"] = null;
            Session["vencount"] = null;
            Session["QuoteOrderCart"] = null;
            Session["veni"] = null;

            dtCart.Columns.Add(new DataColumn("Id", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("RequestQuoteId", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("RequestQuoteDetailId", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("ProductID", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("Name", typeof(String)));
            dtCart.Columns.Add(new DataColumn("ProductOption", typeof(String)));
            dtCart.Columns.Add(new DataColumn("SKU", typeof(String)));
            dtCart.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
            dtCart.Columns.Add(new DataColumn("VendorIds", typeof(String)));
            dtCart.Columns.Add(new DataColumn("Price", typeof(String)));
            dtCart.Columns.Add(new DataColumn("PurNotes", typeof(String)));

            //dtCart.Columns.Add(new DataColumn("PackageID", typeof(String)));
            int cnt = 0;

            if (grvRequestedQuote.Rows.Count > 0)
            {
                for (int i = 0; i < grvRequestedQuote.Rows.Count; i++)
                {
                    GridView gvMasterRow = (GridView)grvRequestedQuote.Rows[i].FindControl("grvQuoteReplyDetails");
                    if (gvMasterRow != null && gvMasterRow.Rows.Count > 0)
                    {
                        for (int j = 0; j < gvMasterRow.Rows.Count; j++)
                        {
                            Label lblName = (Label)(grvRequestedQuote.Rows[i].FindControl("lblName"));
                            Label lblProductID = (Label)(grvRequestedQuote.Rows[i].FindControl("lblProductID"));
                            Label lblProductOption = (Label)(grvRequestedQuote.Rows[i].FindControl("lblProductOption"));
                            Label lblSku = (Label)(grvRequestedQuote.Rows[i].FindControl("lblSku"));

                            TextBox txtPOQty = (TextBox)(gvMasterRow.Rows[j].FindControl("txtPOQty"));
                            TextBox txtQuotedPrice = (TextBox)(gvMasterRow.Rows[j].FindControl("txtQuotedPrice"));
                            Label lblQuoteQuantity = (Label)(gvMasterRow.Rows[j].FindControl("lblQuoteQuantity"));
                            Label lblVendorID = (Label)(gvMasterRow.Rows[j].FindControl("lblVendorID"));
                            Label lblVendorQuoteID = (Label)(gvMasterRow.Rows[j].FindControl("lblVendorQuoteID"));
                            Label lblVendorQuoteReqDetailsID = (Label)(gvMasterRow.Rows[j].FindControl("lblVendorQuoteReqDetailsID"));

                            bool IsValid = false;
                            if (txtPOQty != null && txtQuotedPrice != null)
                            {
                                if ((!string.IsNullOrEmpty(txtPOQty.Text) && Convert.ToInt32(txtPOQty.Text) > 0)
                                   && (!string.IsNullOrEmpty(txtQuotedPrice.Text) && Convert.ToDecimal(txtQuotedPrice.Text) > 0))
                                {
                                    QtyFlag = true;
                                    IsValid = true;
                                }
                            }

                            if (QtyFlag && IsValid)
                            {
                                if (lblQuoteQuantity != null && (Convert.ToInt32(txtPOQty.Text) > Convert.ToInt32(lblQuoteQuantity.Text)))
                                {
                                    string ProName = "";
                                    if (lblName.Text.ToString().Length > 15)
                                        ProName = lblName.Text.ToString().Substring(0, 15);
                                    else ProName = lblName.Text.ToString();

                                    string StrMsg = @"Following Products do not qualify for Purchase Order:\n " + ProName.ToString() + " ... Having PO Quantity is greater than Quoted Quantity.";
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg03", "alert('" + StrMsg.ToString() + "');", true);
                                    return;
                                }
                                DataRow dr = dtCart.NewRow();
                                cnt++;
                                dr["Id"] = Convert.ToInt32(cnt);
                                dr["RequestQuoteId"] = Convert.ToInt32(lblVendorQuoteID.Text.ToString());
                                dr["RequestQuoteDetailId"] = Convert.ToInt32(lblVendorQuoteReqDetailsID.Text.ToString());
                                dr["ProductID"] = Convert.ToInt32(lblProductID.Text.ToString());

                                string ProOption = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ProductOption,'') as ProductOption,VendorQuoteReqDetailsID from tb_VendorQuoteRequestDetails where VendorQuoteReqDetailsID=" + lblVendorQuoteReqDetailsID.Text.ToString() + " and ProductId=" + lblProductID.Text.ToString() + ""));
                                if (!string.IsNullOrEmpty(ProOption.ToString()))
                                    dr["Name"] = Convert.ToString(lblName.Text.ToString()) + "<br/> " + ProOption.ToString().Replace("\r\n", "<br/>");
                                else
                                    dr["Name"] = Convert.ToString(lblName.Text.ToString());
                                dr["ProductOption"] = Convert.ToString(ProOption.ToString());
                                dr["SKU"] = Convert.ToString(lblSku.Text.ToString());
                                dr["Quantity"] = Convert.ToInt32(txtPOQty.Text);
                                dr["VendorIds"] = Convert.ToInt32(lblVendorID.Text.ToString());
                                dr["Price"] = Convert.ToString(txtQuotedPrice.Text.ToString());
                                dr["PurNotes"] = "";
                                //dr["PackageID"] = dsven.Tables[0].Rows[i]["PackageID"].ToString();
                                dtCart.Rows.Add(dr);
                            }
                        }
                    }
                }
                if (QtyFlag == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter Valid Quantity and Price for Product.');", true);
                    return;
                }
            }
            Session["QuoteOrderCart"] = dtCart;
            Session["veni"] = null;
            if (dtCart.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter Valid Quantity and Price for Product.');", true);
                return;
            }
            else
            {
                Response.Redirect("QuotedPurchaseOrder.aspx");
            }
        }

        /// <summary>
        ///  Add Manual Quote Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddManualQuote_Click(object sender, ImageClickEventArgs e)
        {
            string Pids = "";
            int Cnt = 0;
            if (grdCustomer.Rows.Count > 0)
            {
                for (int i = 0; i < grdCustomer.Rows.Count; i++)
                {
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdCustomer.Rows[i].FindControl("chkSelect")).Checked);
                    if (chkSelect)
                    {
                        int ProductId = Convert.ToInt32(((Label)grdCustomer.Rows[i].FindControl("lblProductID")).Text.ToString());
                        Pids += ProductId + ",";
                        Cnt++;
                    }
                }
            }
            if (Cnt == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Select Product to Generate Vendor Quote!');", true);
                return;
            }

            Response.Redirect("AddManualVendorQuote.aspx?PId=" + Pids);
        }

        /// <summary>
        ///  Refresh Quote Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRefreshQuote_Click(object sender, ImageClickEventArgs e)
        {
            BindRequestQuoteDetails();
        }
    }
}