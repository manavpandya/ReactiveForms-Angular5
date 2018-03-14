using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ReturnItem : BasePage
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
                if (Request.QueryString["ONo"] != null)
                {
                    string OrderNumber = SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString());
                    DataSet dsReturn = new DataSet();
                    dsReturn = CommonComponent.GetCommonDataSet("select isnull(rm.ordernumber,0) as NewOrderNumber,p.Name as PName,r.ReturnItemID,r.productid,r.ReturnImages,r.ReturnReason,r.ReturnNotes,isnull(r.IsReturn,0) as IsReturn,isnull(r.ReturnType,'') as ReturnType from tb_returnitem r  inner join tb_product p on p.productid=r.productid " +
                                                                " left outer join tb_rmamapping rm on rm.oldordernumber=r.orderednumber and rm.rmaid=r.returnitemid  " +
                                                                " where orderednumber=" + OrderNumber);
                    if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                    {
                        grdReturnItemList.DataSource = dsReturn;
                        grdReturnItemList.DataBind();
                    }
                    else
                    {
                        trRMATitle.Visible = false;
                        grdReturnItemList.DataSource = null;
                        grdReturnItemList.DataBind();
                    }
                }
            }
        }

        /// <summary>
        /// Return Item List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdReturnItemList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    Label lblImage = (Label)e.Row.FindControl("lblImage");
                    HiddenField hdIsReturn = (HiddenField)e.Row.FindControl("hdIsReturn");
                    Label lblNewOrderNumber = (Label)e.Row.FindControl("lblNewOrderNumber");
                    if (lblNewOrderNumber != null)
                    {
                        if (lblNewOrderNumber.Text.Trim() == "" || lblNewOrderNumber.Text.Trim() == "0")
                            lblNewOrderNumber.Text = "N.A.";
                        //else
                        //Response.Redirect("Orders.aspx?id=" + lblNewOrderNumber.Text.ToString() + "", true);
                    }
                    if (hdIsReturn != null)
                    {
                        HtmlContainerControl divStatus = (HtmlContainerControl)e.Row.FindControl("divStatus");
                        HtmlContainerControl divChangeStatus = (HtmlContainerControl)e.Row.FindControl("divChangeStatus");
                        HtmlContainerControl divrefund = (HtmlContainerControl)e.Row.FindControl("divrefund");
                        HtmlContainerControl divstore = (HtmlContainerControl)e.Row.FindControl("divstore");
                        HtmlInputHidden hdnStoreCredit = (HtmlInputHidden)e.Row.FindControl("hdnreturnid");
                        HtmlInputHidden hdnproductid = (HtmlInputHidden)e.Row.FindControl("hdnproductid");
                        HiddenField hdReturnType = (HiddenField)e.Row.FindControl("hdReturnType");
                        HtmlAnchor aChangeItem = (HtmlAnchor)e.Row.FindControl("aChangeItem");

                        if ((hdIsReturn.Value.ToString().Trim().ToLower() == "false" || hdIsReturn.Value.ToString().Trim().ToLower() == "0"))
                        {
                            if (hdReturnType.Value.ToString().Trim().ToLower() != "rr" && hdReturnType.Value.ToString().Trim().ToLower() != "sc")
                            {
                                divChangeStatus.Visible = true;
                                if (aChangeItem != null)
                                {
                                    aChangeItem.Attributes.Add("onclick", "OpenCenterWindow('ChangeRMAOrder.aspx?ProductID=" + hdnproductid.Value.ToString() + "&ONo=" + Server.UrlEncode(Request.QueryString["ONo"].ToString()) + "&RMA=" + hdnStoreCredit.Value.ToString() + "',880,600)");
                                }

                                divStatus.Visible = false;
                                divrefund.Visible = false;
                                divstore.Visible = false;
                                divstore.InnerHtml = "";
                            }
                            else
                            {
                                if (hdReturnType.Value.ToString().Trim().ToLower() == "sc")
                                {
                                    divChangeStatus.Visible = false;
                                    divstore.Visible = true;
                                    string stReturnImagesrGet = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ISNULL(SerialNumber,'') as SerialNumber FROM tb_GiftCard WHERE RMANumber='" + hdnStoreCredit.Value.ToString() + "'"));
                                    if (!String.IsNullOrEmpty(stReturnImagesrGet))
                                    {
                                        divstore.InnerHtml = "<a style='cursor:pointer;' href='javascript:void(0);' onclick='javascript:window.open(\"StoreCreditDetails.aspx?RMA=" + hdnStoreCredit.Value.ToString() + "\", \"\",\"height=400,width=850,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");'>Credit Details</a>";//
                                    }
                                    else
                                    {
                                        divstore.InnerHtml = "<a style='cursor:pointer;' href='javascript:void(0);' onclick='javascript:window.open(\"StoreCredit.aspx?ono=" + Request.QueryString["ono"].ToString() + "&Proid=" + hdnproductid.Value + "&RMA=" + hdnStoreCredit.Value.ToString() + "\", \"\",\"height=450,width=850,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");'>Store Credit</a>";//
                                    }
                                    divStatus.Visible = false;
                                    divrefund.Visible = false;
                                }
                                else
                                {
                                    divChangeStatus.Visible = false;
                                    divrefund.InnerHtml = "<a style='cursor:pointer;' onclick=\"window.parent.Tabdisplay(10);window.parent.chkHeight();window.parent.iframereload('ContentPlaceHolder1_frmRefund');window.parent.document.getElementById('prepage').style.display = 'none';\" > Refund </a>";
                                    divStatus.Visible = false;
                                    divstore.InnerHtml = "";
                                    divrefund.Visible = true;
                                    divstore.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            divChangeStatus.Visible = false;
                            divStatus.Visible = true;
                            divrefund.Visible = false;
                            divstore.Visible = false;
                            divstore.InnerHtml = "";
                        }
                    }
                    //if (lblImage != null)
                    //{
                    //    if (lblImage.Text.Trim() != "")
                    //    {
                    //        string[] img = lblImage.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //        lblImage.Text = "<Br/>";
                    //        for (int i = 0; i < img.Length; i++)
                    //        {
                    //            lblImage.Text += " <img src='/client/images/ReturnImages/" + img[i].ToString() + "' width='70px'/> ";

                    //        }
                    //        if (img.Length > 0)
                    //            lblImage.Visible = true;
                    //        else
                    //            lblImage.Visible = false;
                    //    }
                    //}
                }
                catch { }
            }
        }
    }
}