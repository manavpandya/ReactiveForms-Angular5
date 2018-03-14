using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Collections;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class PurchaseOrderSelection : BasePage
    {
        string[] strSelPurchaseOr = new string[100];

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAddSelectedItems.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.gif";
                btnSaveChanges.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";
                btnClose.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                BindData();
            }
        }

        /// <summary>
        /// Binds the Order Selection Data
        /// </summary>
        public void BindData()
        {
            DataSet dsPurchaseOrder = new DataSet();
            if (Session["OldPurOr"] == null)
            {
                if (Session["PurchaseOrder"] != null && Session["PurchaseOrder"].ToString() != "")
                {
                    Session["OldPurOr"] = Session["PurchaseOrder"].ToString();
                }
            }
            else
            {
                Session["PurchaseOrder"] = Session["OldPurOr"];
            }
            if (Session["PurchaseOrder"] != null)
            {
                strSelPurchaseOr = Session["PurchaseOrder"].ToString().Replace("PO-", "").Split(',');
            }
            dsPurchaseOrder = CommonComponent.GetCommonDataSet("select sum(isnull(vpo.PaidAmount,0)) as PaidAmount,po.PODate,ISNULL(po.AdditionalCost,0) as AdditionalCost, isnull(po.POAmount,0) as 'POAmount',po.PONumber,po.VendorID, po.OrderNumber,po.PaymentComplete from tb_PurchaseOrder po left outer join dbo.tb_vendorpaymentPurchaseOrder VPO on vpo.PONumber=po.PONumber where po.VendorID=" + Request.QueryString["VID"].ToString() + " and isnull(po.PaymentComplete,0)=0 group by po.PODate,po.AdditionalCost,po.POAmount,po.PONumber,po.VendorID, po.OrderNumber,po.PaymentComplete Order by po.PONumber Desc");
            if (dsPurchaseOrder != null && dsPurchaseOrder.Tables.Count > 0 && dsPurchaseOrder.Tables[0].Rows.Count > 0)
            {
                GridVenPruchase.DataSource = dsPurchaseOrder;
                GridVenPruchase.DataBind();
                trAddSeletedItems.Visible = true;
                trTotal.Visible = true;
            }
            else
            {
                GridVenPruchase.DataSource = null;
                trAddSeletedItems.Visible = false;
                trTotal.Visible = false;
                GridVenPruchase.DataBind();
            }

            double decPOAmou = 0;
            double decPaidAmou = 0;
            double decremaing = 0;
            string strOr = "";

            Hashtable ht = new Hashtable();
            if (Session["POCart"] != null)
                ht = (Hashtable)Session["POCart"];
            foreach (GridViewRow dr in GridVenPruchase.Rows)
            {
                CheckBox chkSelect = (CheckBox)dr.FindControl("chkSelect");
                Label lblPOA = (Label)dr.FindControl("lblPAmount");
                Label lblPONumber = (Label)dr.FindControl("lblPONum");
                Label lblPaidAmount = (Label)dr.FindControl("lblPaidAmount");
                TextBox lblRem = (TextBox)dr.FindControl("lblRemaingAmount");
                Label lblTPAmount = (Label)dr.FindControl("lblTPAmount");
                Label lblTotalPAmount = (Label)dr.FindControl("lblTotalPaidAmount");
                Label lblTotalRemAmount = (Label)dr.FindControl("lblTotalRemaining");

                if (Session["PurchaseOrder"] != null)
                {
                    string[] strpo = Session["PurchaseOrder"].ToString().Replace("PO-", "").Split(',');

                    for (int i = 0; i < strpo.Length; i++)
                    {
                        if (strpo[i].Trim() == lblPONumber.Text.Trim())
                        {
                            chkSelect.Checked = true;

                            if (ht[strpo[i].Trim()] != null)
                                lblRem.Text = ht[strpo[i].Trim()].ToString();

                            decPOAmou = Convert.ToDouble(decPOAmou) + Convert.ToDouble(lblPOA.Text.Trim());
                            strOr = strOr + " PO-" + lblPONumber.Text.Trim() + ",";
                            decremaing = decremaing + Convert.ToDouble(lblRem.Text.Trim());
                            decPaidAmou = decPaidAmou + Convert.ToDouble(lblPaidAmount.Text.Trim());
                        }
                    }
                }
            }
            lblCost.Text = "$" + Convert.ToDecimal(decPOAmou).ToString("f2");
            lblorder.Text = strOr.ToString();
            lblReAmount.Text = "$" + Convert.ToDecimal(decremaing).ToString("f2");
            lblPaid.Text = "$" + Convert.ToDecimal(decPaidAmou).ToString("f2");
        }

        /// <summary>
        /// Vendor Purchase Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void GridVenPruchase_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string strpo = "";
                decimal TotalAmount = 0;
                decimal RemainingAmount = 0;
                decimal PaidAmount = 0;
                decimal firstPaidA = 0;
                decimal FirstPOAmou = 0;
                decimal FirestRem = 0;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblPAmount = (Label)e.Row.FindControl("lblPAmount");
                    Label lblPaidAmount = (Label)e.Row.FindControl("lblPaidAmount");
                    TextBox lblRem = (TextBox)e.Row.FindControl("lblRemaingAmount");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    Label lblPONumber = (Label)e.Row.FindControl("lblPONum");
                    strpo = lblPONumber.Text.Trim();
                    Hashtable ht = new Hashtable();
                    if (Session["POCart"] != null)
                        ht = (Hashtable)Session["POCart"];

                    lblRem.Text = Convert.ToDecimal(Convert.ToDecimal(lblPAmount.Text.Trim()) - Convert.ToDecimal(lblPaidAmount.Text.Trim())).ToString("f2");
                    TotalAmount = TotalAmount + Convert.ToDecimal(lblPAmount.Text.Trim());
                    RemainingAmount = RemainingAmount + Convert.ToDecimal(lblRem.Text.Trim());
                    PaidAmount = PaidAmount + Convert.ToDecimal(lblPaidAmount.Text.Trim());
                    for (int i = 0; i < strSelPurchaseOr.Length; i++)
                    {
                        if (strSelPurchaseOr[i].Trim() == strpo.Trim())
                        {
                            if (ht[strSelPurchaseOr[i].Trim()] != null)
                                lblRem.Text = ht[strSelPurchaseOr[i].Trim()].ToString();
                            firstPaidA = firstPaidA + Convert.ToDecimal(lblPaidAmount.Text.Trim());
                            FirstPOAmou = FirstPOAmou + Convert.ToDecimal(lblPAmount.Text.Trim());
                            FirestRem = FirestRem + Convert.ToDecimal(lblRem.Text.Trim());
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTPAmount = (Label)e.Row.FindControl("lblTPAmount");
                    Label lblTotalPAmount = (Label)e.Row.FindControl("lblTotalPaidAmount");
                    Label lblTotalRemAmount = (Label)e.Row.FindControl("lblTotalRemaining");

                    if (lblTPAmount != null)
                        lblTPAmount.Text = Convert.ToDecimal(firstPaidA).ToString("f2");

                    if (lblTotalPAmount != null)
                        lblTotalPAmount.Text = Convert.ToDecimal(FirstPOAmou).ToString("f2");

                    if (lblTotalRemAmount != null)
                        lblTotalRemAmount.Text = Convert.ToDecimal(FirestRem).ToString("f2");
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        ///  Close Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnclose_Click(object sender, ImageClickEventArgs e)
        {
            Session["OldPurOr"] = lblorder.Text;
            Session["POCart"] = (Hashtable)ViewState["POCart"];
            Session["PONumber"] = lblorder.Text.Trim();
            lblCost.Text = lblCost.Text.Replace("$", "");
            //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msgClose", "<script>window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value='';window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML='0.00';</script>", true);

            ScriptManager.RegisterClientScriptBlock(btnSaveChanges, btnSaveChanges.GetType(), "Msg", "window.opener.document.getElementById('ContentPlaceHolder1_lblPurchaseorder').innerHTML='" + lblorder.Text.Trim() + "'; " +
               " window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value = window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value +'" + lblorder.Text.Trim() + "';" +
               " window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML='$" + lblCost.Text.Trim() + "';" +
               " window.opener.document.getElementById('ContentPlaceHolder1_hfPA').value=window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML; " +
               "window.opener.document.getElementById('ContentPlaceHolder1_hfPrevPaid').value='" + lblPaid.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblPreviousPaid').innerHTML='" + lblPaid.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblPaidAmount').innerHTML=window.opener.document.getElementById('ContentPlaceHolder1_hPaidAmount').value= '$'+ (parseFloat(" + lblReAmount.Text.Trim().Replace("$", "") + ")).toFixed(2) ; window.close();", true);
        }

        /// <summary>
        ///  Add Selected Items Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddSelectedItems_Click(object sender, ImageClickEventArgs e)
        {
            double decPOAmou = 0;
            double decremaing = 0;
            bool blflag = false;
            string strOr = "";
            lblorder.Text = "";
            double decPaidAmount = 0;
            Hashtable ht = new Hashtable();

            foreach (GridViewRow dr in GridVenPruchase.Rows)
            {
                CheckBox chkSelect = (CheckBox)dr.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    Label lblPOA = (Label)dr.FindControl("lblPAmount");
                    Label lblONumber = (Label)dr.FindControl("lblONumber");
                    Label lblPONumber = (Label)dr.FindControl("lblPONum");
                    TextBox lblRem = (TextBox)dr.FindControl("lblRemaingAmount");
                    Label lblPaidAmount = (Label)dr.FindControl("lblPaidAmount");
                    ht.Add(lblPONumber.Text.Trim(), lblRem.Text.Trim());
                    decPOAmou = Convert.ToDouble(decPOAmou) + Convert.ToDouble(lblPOA.Text.Trim().Replace("$", ""));
                    decremaing = decremaing + Convert.ToDouble(lblRem.Text.Trim());
                    strOr = strOr + " PO-" + lblPONumber.Text.Trim();
                    decPaidAmount = decPaidAmount + Convert.ToDouble(lblPaidAmount.Text.Trim().Replace("$", ""));
                    strOr += ",";
                    blflag = true;
                }
            }

            if (blflag)
            {
                lblCost.Text = "$" + Convert.ToDecimal(decPOAmou).ToString("f2");
                lblReAmount.Text = "$" + Convert.ToDecimal(decremaing).ToString("f2");
                lblPaid.Text = "$" + Convert.ToDecimal(decPaidAmount).ToString("f2");
                lblorder.Text = strOr.ToString().Substring(0, strOr.Length - 1);
                if (Session["OldPurOr"] != null)
                    Session["OldPurOr"] = lblorder.Text;
                btnSaveChanges.Visible = true;
                btnAddSelectedItems.Visible = false;
                ViewState["POCart"] = null;
                ViewState["POCart"] = ht;
            }
            else
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select at least One Order.');", true);
        }

        /// <summary>
        ///  Save Changes Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSaveChanges_Click(object sender, ImageClickEventArgs e)
        {
            Session["POCart"] = (Hashtable)ViewState["POCart"];
            Session["PONumber"] = lblorder.Text.Trim();
            lblCost.Text = lblCost.Text.Replace("$", "");
            //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msgsave", "<script>window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value='';window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML='0.00';</script>", true);
            ScriptManager.RegisterClientScriptBlock(btnSaveChanges, btnSaveChanges.GetType(), "Msg", "window.opener.document.getElementById('ContentPlaceHolder1_lblPurchaseorder').innerHTML='" + lblorder.Text.Trim() + "'; " +
               " window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value = window.opener.document.getElementById('ContentPlaceHolder1_txtPurchaseorder').value +'" + lblorder.Text.Trim() + "';" +
               " window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML='$" + lblCost.Text.Trim() + "';" +
               " window.opener.document.getElementById('ContentPlaceHolder1_hfPA').value=window.opener.document.getElementById('ContentPlaceHolder1_lblPOAmount').innerHTML; " +
               "window.opener.document.getElementById('ContentPlaceHolder1_lblPreviousPaid').innerHTML='" + lblPaid.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_hfPrevPaid').value='" + lblPaid.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblPaidAmount').innerHTML=window.opener.document.getElementById('ContentPlaceHolder1_hPaidAmount').value= '$'+ (parseFloat(" + lblReAmount.Text.Trim().Replace("$", "") + ")).toFixed(2) ; window.close();", true);
        }
    }
}