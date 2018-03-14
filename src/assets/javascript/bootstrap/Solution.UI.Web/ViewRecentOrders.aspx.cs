using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace Solution.UI.Web
{
    public partial class viewrecentorders : System.Web.UI.Page
    {

        #region Local Variables
        Int32 PageNo = 1;
        Int32 PageSize = 1;
        bool viewAll = false;
        Int32 PageCount = 0;
        System.Web.UI.WebControls.Literal ltrvartable = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            ltbrTitle.Text = "View Recent And Open Orders";
            ltTitle.Text = "View Recent And Open Orders";
            Session["PageOlderOrder"] = "false";
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
            }
            else
            {
                Response.Redirect("/login.aspx", true);
            }

            if (!IsPostBack)
            {
                if (Session["CustID"] != null)
                {
                    GetOrderDetails();
                }
                else
                {
                    Response.Redirect("/");
                }
            }
        }

        /// <summary>
        /// Gets the Recent Order Details
        /// </summary>
        private void GetOrderDetails()
        {
            //for Paging
            OrderComponent objOrder = new OrderComponent();
            DataSet DsOrderPaging = new DataSet();
            DsOrderPaging = objOrder.GetViewOrderPaging(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            // end for Paging 

            DataSet DsOrder = new DataSet();
            DsOrder = objOrder.GetOrderNobyCustomerID(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            {
                StringBuilder Table = new StringBuilder();
                DataRow[] dr = DsOrder.Tables[0].Select("isnull(isnew,0) = 1"); //("isnull(IsNew,0) = 0 AND isnull(ShippedVIA,0) = 1"); for old order
                if (dr != null && dr.Length > 0)
                {
                    foreach (DataRow drOrderNums in dr)
                    {
                        DataSet dsOrder = new DataSet();
                        dsOrder = objOrder.GetViewRecentOrderByOrderID(Convert.ToInt32(drOrderNums["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        Table.Append("<table style='float:left;' width='100%' align='left' border='0' cellpadding='0' cellspacing='0' class='table_none'  >");
                        Table.Append("<tr>");
                        Table.Append("<td valign='top' style=\"border-right: 1px solid #DFDFDF;width:230px;align:left;\">");
                        Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table-none'  style='border:0px;'>");
                        Table.Append("<tr>");
                        Table.Append("<td>Order Date");
                        Table.Append("<br/><p style=\"font-size:14px;padding-right:0px; font-weight:bold;width:230px;\">" + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString()).ToLongDateString() + "</p>");
                        Table.Append("<br/><br/><a href=\"/orderdetails.aspx?ono=" + Convert.ToString(drOrderNums["OrderNumber"].ToString()) + "\" >View Order Details</a> | <a href='invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Convert.ToString(drOrderNums["OrderNumber"].ToString()))) + "' target=\"_blank\"\">View Invoice</a>  ");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr>");
                        Table.Append("<td>Order Number: " + Convert.ToString(drOrderNums["OrderNumber"].ToString() + ""));
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr>");
                        Table.Append("<td>Shipping Method: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString() + ""));
                        Table.Append("</td>");
                        Table.Append("</tr>");
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
                            if (dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString().ToLower() == "new")
                            {
                                Table.Append("<tr>");
                                Table.Append("<td>Order Status: Pending");
                                Table.Append("</td>");
                                Table.Append("</tr>");
                            }
                            else
                            {
                                Table.Append("<tr>");
                                Table.Append("<td>Order Status: " + dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString() + "");
                                Table.Append("</td>");
                                Table.Append("</tr>");

                            }

                        }


                        Table.Append("</table>");

                        Table.Append("</td>");
                        Table.Append("<td style='padding-top:5px;width:98%;border-right: 1px solid #DFDFDF;' vAlign='top'>");

                        DataSet DsCartDetail = new DataSet();
                        DsCartDetail = objOrder.GetCartItemForViewRecentOrder(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        if (DsCartDetail != null && DsCartDetail.Tables.Count > 0 && DsCartDetail.Tables[0].Rows.Count > 0)
                        {
                            Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table_border_none' >");
                            Table.Append("<tr style='vAlign:top;'>");
                            Table.Append("<td colspan='2' vAlign='top' style='border-bottom:1px solid #DFDFDF;'>");
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
                                    //Table.Append("</td>");
                                    ////Table.Append("<td align='left' width='83%' valign='top'><a  href='/gi-" + Item.m_ProductID + "-" + Item.m_ProductSEName.ToLower() + ".aspx'>" + Item.m_ProductName + "</a>");
                                    //Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                                    //Table.Append("</td>");

                                   // Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='" + GetProductUrl(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString(), DsCartDetail.Tables[0].Rows[i]["SEName"].ToString(), DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString(), DsCartDetail.Tables[0].Rows[i]["CustomCartID"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                    if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\">");
                                        Table.Append("</td>");
                                    }
                                    else
                                    {
                                        Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                        Table.Append("</td>");
                                    }
                                    //Table.Append("<td align='left'  width='83%' valign='top' ><a  href='" + GetProductUrl(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString(), DsCartDetail.Tables[0].Rows[i]["SEName"].ToString(), DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString(), DsCartDetail.Tables[0].Rows[i]["CustomCartID"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                    if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<td align='left'  width='83%' valign='top' >" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                        Table.Append("</td>");
                                    }
                                    else
                                    {
                                        Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                        Table.Append("</td>");
                                    }
                                   
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
                                    //Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                    //Table.Append("</td>");
                                    //Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                    //Table.Append("</td>");

                                    //Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='" + GetProductUrl(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString(), DsCartDetail.Tables[0].Rows[i]["SEName"].ToString(), DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString(), DsCartDetail.Tables[0].Rows[i]["CustomCartID"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                    if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\">");
                                        Table.Append("</td>");
                                    }
                                    else
                                    {
                                        Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                        Table.Append("</td>");
                                    }
                                   // Table.Append("<td align='left'  width='83%' valign='top' ><a  href='" + GetProductUrl(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString(), DsCartDetail.Tables[0].Rows[i]["SEName"].ToString(), DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString(), DsCartDetail.Tables[0].Rows[i]["CustomCartID"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                    if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<td align='left'  width='83%' valign='top' >" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                        Table.Append("</td>");
                                    }
                                    else
                                    {
                                        Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                        Table.Append("</td>");
                                    }
                                }
                                Table.Append("</tr>");

                            }
                            Table.Append("</table>");

                        }

                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("</table>");

                    }
                    lblTable.Text = Table.ToString();
                }
            }
            else
                lblMsg.Text = "Order does not exist for this Customer.";
        }

        /// <summary>
        /// Gets the Micro Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Micro Image Path</returns>
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
        /// Bind Variant for Product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns Variant Details for Product as a Literal Control</returns>
        public System.Web.UI.WebControls.Literal BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            ltrvartable = new System.Web.UI.WebControls.Literal();
            if (varname.Length > 0)
            {

                // sbvartable.Append("<table cellpadding='0' cellspacing='0' width='100%'><tr><td>");
                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
                // sbvartable.AppendLine("</td></tr></table>");
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

        /// <summary>
        /// Bind Paging with Next and Previous link in SubCategory 's Products and Brand 's Products
        /// </summary>
        /// <param name="PageNumber">int PageNumber </param>
        /// <param name="NumberOfPages">int NumberOfPages</param>
        /// <returns>Returns Paging With Next and Previous link</returns>
        private String BindPageNumbers(int PageNumber, int NumberOfPages)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            String bre = "ViewRecentOrders";


            string pageurl = "/" + bre + "*";
            if (PageNumber > NumberOfPages)
                PageNumber = NumberOfPages;

            int CurrentOffset = (int)(PageNumber - 0.51) / 3;
            int FinalOffset = (int)(NumberOfPages - 0.51) / 3;



            if (viewAll == true)
                sb.AppendLine(" <span class='active'>Pages</span> ");
            else
                sb.AppendLine(" <a  href=\"" + pageurl + "All\">All Pages</a>");

            // sb.AppendLine("<");
            if (PageNumber > 1)
            {
                sb.AppendLine("<a  href=\"" + pageurl + (PageNumber - 1) + "\"> Previous</a>... ");
            }

            sb.Append(" [ ");

            if (viewAll == true)
            {
                for (int i = ((CurrentOffset * 3) + 1); i <= ((CurrentOffset + 1) * 3); i++)
                {
                    if (i > NumberOfPages)
                        break;
                    sb.AppendLine("<a href=\"" + pageurl + i + "\">" + i + "</a>" + " | ");
                }
            }
            else
            {
                for (int i = ((CurrentOffset * 3) + 1); i <= ((CurrentOffset + 1) * 3); i++)
                {
                    if (i > NumberOfPages)
                        break;
                    if (i != PageNumber)
                    {
                        sb.AppendLine("<a  href=\"" + pageurl + i + "\">" + i + "</a>" + " | ");
                    }
                    else
                    {
                        sb.AppendLine("<a class='active'><span>" + " " + i + "</span></a>" + " | ");
                    }
                }
            }
            if (PageNumber < NumberOfPages)
            {
                sb.AppendLine(" ...<a  href=\"" + pageurl + (PageNumber + 1) + "\">Next  </a> ");
            }

            //    sb.AppendLine(">");
            return sb.ToString();



        }

        /// <summary>
        /// Gets the Product URL
        /// </summary>
        /// <param name="mainCategory">String mainCategory</param>
        /// <param name="Sename">String Sename</param>
        /// <param name="ProductId">String ProductId</param>
        /// <param name="CustomCartID">String CustomCartID</param>
        /// <returns>String.</returns>
        public String GetProductUrl(String mainCategory, String Sename, String ProductId, string CustomCartID)
        {
            string Url = "";
            int GiftCardProductID = 0;
            GiftCardProductID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(GiftCardProductID,0) FROM dbo.tb_GiftCardProduct Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + ProductId.ToString() + ""));
            if (GiftCardProductID > 0)
            {
                Url = "/gi-" + CustomCartID + "-";
                if (Sename != "")
                {
                    Url += Sename.ToString();
                }
            }
            else
            {
                if (mainCategory != "")
                {
                    Url = "/" + mainCategory.ToString();
                }
                if (Sename != "")
                {
                    Url += "/" + Sename.ToString();
                }
            }
            if (ProductId != "")
            {
                Url += "-" + ProductId.ToString() + ".aspx";
            }
            return Url.ToString();
        }
    }
}
