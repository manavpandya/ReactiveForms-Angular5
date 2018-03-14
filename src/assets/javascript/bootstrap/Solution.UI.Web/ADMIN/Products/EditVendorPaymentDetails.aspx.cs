using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class EditVendorPaymentDetails : Solution.UI.Web.BasePage
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnAddPurchaseOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Select-Purchase-Order.gif";
                BindVendorName();
                btnAddPurchaseOrder.OnClientClick = "OpenCenterWindow('PurchaseOrderSelection.aspx?VID=" + hfVPID.Value + "',900,600)";
            }
        }

        /// <summary>
        /// Binds the name of the vendor into Drop Down
        /// </summary>
        public void BindVendorName()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SElect VendorID,Name from tb_Vendor where ISNULL(Active,1)=1 and ISNULL(Deleted,0)=0");
            ddlVendorName.DataSource = ds;
            ddlVendorName.DataValueField = "VendorID";
            ddlVendorName.DataTextField = "Name";
            ddlVendorName.DataBind();
            hfVPID.Value = ddlVendorName.SelectedValue;
        }

        /// <summary>
        /// Vendor Name Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlVendorName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPurchaseorder.Text))
            {
                Session["PurchaseOrder"] = null;
                Session["OldPurOr"] = null;
            }
            else
                Session["OldPurOr"] = txtPurchaseorder.Text.Trim();
            hfVPID.Value = ddlVendorName.SelectedValue;
            btnAddPurchaseOrder.OnClientClick = "OpenCenterWindow('PurchaseOrderSelection.aspx?VID=" + hfVPID.Value + "',900,600)";
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToDecimal(hPaidAmount.Value.ToString().Replace("$", "")) > 0)
                lblPaidAmount.Text = hPaidAmount.Value.ToString().Replace("$", "");
            if (Convert.ToDecimal(hfPA.Value.ToString().Replace("$", "")) > 0)
                lblPOAmount.Text = hfPA.Value.ToString();
            if (Convert.ToDecimal(hfPrevPaid.Value.ToString().Replace("$", "")) > 0)
                lblPreviousPaid.Text = hfPrevPaid.Value;
            
            string strHPA = hfPA.Value.Replace("$", "");

            if (Convert.ToDecimal(lblPaidAmount.Text.Trim()) > 0)
            {
                if (!string.IsNullOrEmpty(txtTransactionDate.Text.ToString()))
                {
                    if (Convert.ToDecimal(strHPA) >= Convert.ToDecimal(lblPaidAmount.Text.Trim()))
                    {
                        Insert();
                    }
                    else
                    {
                        if (Session["PONumber"] != null && Session["PONumber"].ToString() != "")
                            lblPurchaseorder.Text = Session["PONumber"].ToString();
                        lblPOAmount.Text = strHPA.ToString();
                        lblMsg.Text = "Please enter Paid Amount Less than PO Amount";
                        lblMsg.Visible = true;
                    }
                }
                else
                {
                    lblMsg.Text = "Please Enter Transaction Date ";
                    lblMsg.Visible = true;
                }
            }
            else
            {
                if (Session["PONumber"] != null && Session["PONumber"].ToString() != "")
                    lblPurchaseorder.Text = Session["PONumber"].ToString();
                lblMsg.Text = "Paid Amount Must be greate than Zero. ";
                lblMsg.Visible = true;
            }
        }
      
        /// <summary>
        /// Insert method for inserting the vendor data in the database
        /// </summary>
        public void Insert()
        {
            Int32 VendorID = Convert.ToInt32(ddlVendorName.SelectedValue.ToString());
            decimal depoamount = Convert.ToDecimal(hfPA.Value.Replace("$", ""));
            decimal decpaidamount = Convert.ToDecimal(lblPaidAmount.Text);
            lblPOAmount.Text = depoamount.ToString();
            if (((Convert.ToDecimal(depoamount) > Convert.ToDecimal(lblPaidAmount.Text.Trim())) && (rdpayment.Items[0].Selected == false)) || ((Convert.ToDecimal(depoamount) == Convert.ToDecimal(lblPaidAmount.Text.Trim())) && (rdpayment.Items[0].Selected == true)))
            {
                Int32 VendorPaymentID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert Into tb_VendorPayment(VendorID,PaidBy,TransactionReference,TransactionDate,PurchaseOrders,PoAmount,PaidAmount,Note) values(" + ddlVendorName.SelectedValue + ",'" + txtPaidBy.Text.Trim().Replace("'", "''") + "','" + txtTransactionRef.Text.Trim().Replace("'", "''") + "','" + txtTransactionDate.Text + "','" + txtPurchaseorder.Text.Trim() + "'," + hfPA.Value.Replace("$", "") + "," + lblPaidAmount.Text.Trim() + ",'" + txtNote.Text.Trim().Replace("'", "''") + "') Select SCOPE_IDENTITY();"));
                if (VendorPaymentID > 0)
                {
                    BindPONumber();

                    if (Convert.ToDecimal(depoamount) == Convert.ToDecimal(lblPaidAmount.Text.Trim()))
                    {
                        CommonComponent.ExecuteCommonData("update tb_PurchaseOrder set PaymentComplete=1 where vendorid=" + VendorID + " and PONumber in (" + txtPurchaseorder.Text.Trim().Replace("PO-", "") + ")");
                        Response.Redirect("VendorPaymentList.aspx?Insert=true");
                    }
                    else
                    {
                        if (rdpayment.Items[1].Selected == true)
                        {
                            string[] strPONumber = txtPurchaseorder.Text.Replace("PO-", "").Split(',');
                            string query = "";
                            for (int i = 0; i < strPONumber.Length; i++)
                            {
                                query += " update po " +
                                        " set po.paymentcomplete=1 " +
                                        " from tb_PurchaseOrder po " +
                                        "    where po.ponumber=" + strPONumber[i] + " and  " +
                                        " isnull(po.poamount,0)=(select isnull(sum(isnull(paidamount,0)),0) from tb_vendorpaymentPurchaseOrder where ponumber=" + strPONumber[i] + ")";
                            }
                            CommonComponent.ExecuteCommonData(query);
                            Response.Redirect("VendorPaymentList.aspx?Insert=true");
                        }
                        else
                        {
                            if (Session["PONumber"] != null && Session["PONumber"].ToString() != "")
                                lblPurchaseorder.Text = Session["PONumber"].ToString();
                            lblMsg.Text = "Please Select Partial Payment";
                            lblMsg.Visible = true;
                        }
                    }
                }
                else
                {
                    if (Session["PONumber"] != null && Session["PONumber"].ToString() != "")
                        lblPurchaseorder.Text = Session["PONumber"].ToString();
                    lblMsg.Text = "Vendor with same name already exists, please specify another name...";
                }
            }
            else
            {
                if (Session["PONumber"] != null && Session["PONumber"].ToString() != "")
                    lblPurchaseorder.Text = Session["PONumber"].ToString();
                if (Convert.ToDecimal(depoamount) == Convert.ToDecimal(lblPaidAmount.Text.Trim()))
                    lblMsg.Text = "Please Select Full Payment.";
                else
                    lblMsg.Text = "Please Select Partial Payment.";
                lblMsg.Visible = true;
            }
        }

        /// <summary>
        /// Binds the PO number for Payment.
        /// </summary>
        public void BindPONumber()
        {
            string strVendorPayID = "";
            string[] strPONumber = txtPurchaseorder.Text.Replace("PO-", "").Split(',');

            if (Request.QueryString["ID"] != null)
                strVendorPayID = Request.QueryString["ID"].ToString();
            else
                strVendorPayID = Convert.ToString((Int32)CommonComponent.GetScalarCommonData("select max(VendorPaymentID) as 'Lastvpid' from tb_vendorPayment"));
            CommonComponent.ExecuteCommonData("delete from tb_vendorpaymentPurchaseOrder where  VendorPaymentID=" + strVendorPayID + " ");
            Hashtable ht = new Hashtable();
            if (Session["POCart"] != null)
                ht = (Hashtable)Session["POCart"];

            for (int i = 0; i < strPONumber.Length; i++)
            {
                if (ht.Keys.Count > 0)
                    CommonComponent.ExecuteCommonData("insert into tb_vendorpaymentPurchaseOrder(VendorPaymentID,PONumber,PaidAmount) values(" + strVendorPayID + "," + strPONumber[i] + "," + ht["" + strPONumber[i].ToString().Trim() + ""].ToString() + ")");
                else
                    CommonComponent.ExecuteCommonData("insert into tb_vendorpaymentPurchaseOrder(VendorPaymentID,PONumber,PaidAmount) values(" + strVendorPayID + "," + strPONumber[i] + "," + lblPaidAmount.Text.Trim() + ")");
            }
            Session["POCart"] = null;
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("VendorPaymentList.aspx");
        }
    }
}