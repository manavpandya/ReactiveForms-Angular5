using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Data;

namespace Solution.UI.Web
{
    public partial class OrderStatus : System.Web.UI.Page
    {
        System.Web.UI.WebControls.Literal ltrvartable = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            lblMsg.Text = "";
            if (!IsPostBack)
            {

                lblMsg.Text = "";
                txtOrderNumber.Focus();
                txtOrderNumber.Text = "";
                txtEmail.Text = "";
            }

        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {

            String SelectQuery = "";
            SelectQuery = "select o.ordernumber, o.Email  from  tb_Order  o  where o.deleted = 0 and O.ordernumber = " + txtOrderNumber.Text.Trim() + " and o.StoreID = " + AppLogic.AppConfigs("StoreID") + " and o.Email ='" + txtEmail.Text.Trim() + "'";
            DataSet dscustomer = new DataSet();
            dscustomer = CommonComponent.GetCommonDataSet(SelectQuery);
            if (dscustomer != null && dscustomer.Tables.Count > 0 && dscustomer.Tables[0].Rows.Count > 0)
            {
                GetOrderDetails();
                tblOrderDetail.Visible = false;
            }
            else
            {
                lblMsg.Text = "Order Number or Email is not Valid!";
            }


        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/");

        }
        
        /// <summary>
        /// Gets the Order Details
        /// </summary>
        private void GetOrderDetails()
        {
            StringBuilder Table = new StringBuilder();
            OrderComponent objOrder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objOrder.GetViewRecentOrderByOrderID(Convert.ToInt32(txtOrderNumber.Text.Trim()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                Table.Append("<table style='float:left;' width='100%' align='left' border='0' cellpadding='0' cellspacing='0' class='table_none'  >");
                Table.Append("<tr>");
                Table.Append("<td valign='top' style=\"border-right: 1px solid #E5E5E5;width:230px;align:left;\">");
                Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table_none'  style='border:0px;'>");
                Table.Append("<tr>");
                Table.Append("<td>Order Date");
                Table.Append("<br/><p style=\"font-size:14px;padding-right:0px; font-weight:bold;width:230px;\">" + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString()).ToLongDateString() + "</p>");
                Table.Append("</td>");
                Table.Append("</tr>");
                Table.Append("<tr>");
                Table.Append("<td>Order Number: " + Convert.ToString(txtOrderNumber.Text.Trim() + ""));
                Table.Append("</td>");
                Table.Append("</tr>");
                //Table.Append("<tr>");
                //Table.Append("<td>Shipping Method: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString() + ""));
                //Table.Append("</td>");
                //Table.Append("</tr>");

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()))
                {
                    Table.Append("<tr>");
                    Table.Append("<td>Tracking Number: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"]));
                    Table.Append("</td>");
                    Table.Append("</tr>");
                    //int pCount = dsOrder.Tables[0].Rows.Count;
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //for (int i = 0; i < pCount; i++)
                    //{
                    //    sb.Append(dsOrder.Tables[0].Rows[i]["ShippingTrackingNumber"].ToString() + ", ");

                    //}
                    //int length = sb.ToString().Length;
                    //string TrackingNo = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
                    //Table.Append("<tr>");
                    //Table.Append("<td>Tracking Number: "+ TrackingNo.ToString()+"");
                    //Table.Append("</td>");
                    //Table.Append("</tr>");

                }

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippedVia"].ToString()))
                {
                    Table.Append("<tr>");
                    Table.Append("<td>Shipped Via: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippedVia"]));
                    Table.Append("</td>");
                    Table.Append("</tr>");
                }


                Table.Append("<tr>");
                Table.Append("<td>Recipient: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString() + ""));
                Table.Append("</td>");
                Table.Append("</tr>");
                Table.Append("<tr>");
                Table.Append("<td>Order Total: $" + Convert.ToString(Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2)) + "");
                Table.Append("</td>");
                Table.Append("</tr>");

                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString()))
                {
                    Table.Append("<tr>");
                    if (dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString().ToLower() == "new")
                    {
                        Table.Append("<td>Order Status: Pending");
                    }
                    else
                    {
                        Table.Append("<td>Order Status: " + dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString() + "");
                    }
                    Table.Append("</td>");
                    Table.Append("</tr>");
                }


                Table.Append("</table>");

                Table.Append("</td>");
                Table.Append("<td style='padding-top:5px;width:98%;border-right: 1px solid rgb(229, 229, 229);' vAlign='top'>");

                DataSet DsCartDetail = new DataSet();
                DsCartDetail = objOrder.GetCartItemForViewRecentOrder(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (DsCartDetail != null && DsCartDetail.Tables.Count > 0 && DsCartDetail.Tables[0].Rows.Count > 0)
                {
                    Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table_border_none' >");
                    Table.Append("<tr style='vAlign:top;'>");
                    Table.Append("<td colspan='2' vAlign='top' style='border-bottom:1px solid #E5E5E5;'>");
                    Table.Append("<p style='font-size:14px;font-weight:bold;width:83%;'>Ordered Items</p>");
                    Table.Append("</td>");
                    Table.Append("</tr>");
                    Table.Append("<tr style='valign:top;'>");

                    for (Int32 i = 0; i < DsCartDetail.Tables[0].Rows.Count; i++)
                    {


                        if ((DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == null) || (DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == ""))
                        {

                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                            {
                                BindVariantForProduct(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString(), DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString());
                            }
                            else
                            {
                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                ltrvartable.Text = string.Empty;
                            }
                            //Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                            Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                            Table.Append("</td>");
                            ////Table.Append("<td align='left' width='83%' valign='top'><a  href='/gi-" + Item.m_ProductID + "-" + Item.m_ProductSEName.ToLower() + ".aspx'>" + Item.m_ProductName + "</a>");
                            //Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                            Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");

                            Table.Append("</td>");
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                            {
                                BindVariantForProduct(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()), Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()));
                            }
                            else
                            {
                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                ltrvartable.Text = string.Empty;
                            }
                            Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                            Table.Append("</td>");
                            Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                            Table.Append("</td>");
                        }
                        Table.Append("</tr>");

                    }
                    Table.Append("</table>");

                }

                Table.Append("</td>");
                Table.Append("</tr>");
                Table.Append("</table>");
                lblTable.Text = Table.ToString();
            }


            else
                lblMsg.Text = "Order Number or Email is not Valid!";
        }
        
        /// <summary>
        /// Gets the Micro Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Micro Image Path</returns>
        public String GetMicroImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            if (img != "")
            {
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }
        
        /// <summary>
        /// Binds the Variant for Product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns the Litral Controls that contains HTML</returns>
        public System.Web.UI.WebControls.Literal BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            ltrvartable = new System.Web.UI.WebControls.Literal();
            if (varname.Length > 0)
            {

                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
            }
            if (sbvartable.ToString() != "")
            {
                ltrvartable.Text = sbvartable.ToString();
            }
            else
            {
                ltrvartable.Text = "";
            }
            return ltrvartable;
        }
    }
}