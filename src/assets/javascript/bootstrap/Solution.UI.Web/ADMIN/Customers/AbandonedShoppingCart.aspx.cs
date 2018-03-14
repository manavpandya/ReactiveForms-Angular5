using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Text;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;


namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class AbandonedShoppingCart : BasePage
    {
        #region Variable declaration
        public static bool isDescendEmail = false;
        public static bool isDescendName = false;
        System.Web.UI.WebControls.Literal ltrvartable = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblEmailMsg.Text = "";
            if (!IsPostBack)
            {

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                btnSend.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/send.gif) no-repeat transparent; width: 48px; height: 23px;border:none;cursor:pointer;");
                bindstore();
                GetCartDetail();
            }
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdAbandonedShoppingCart.PageIndex = 0;
            GetCartDetail();
        }

        /// <summary>
        /// Bind Store dropdown
        /// </summary>
        private void bindstore()
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
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grdAbandonedShoppingCart.PageIndex = 0;
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "Search Keyword";
            GetCartDetail();
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {

            if (grdAbandonedShoppingCart.Rows.Count > 0)
            {
                Int32 icountt = 0;
                foreach (GridViewRow gr in grdAbandonedShoppingCart.Rows)
                {

                    System.Web.UI.HtmlControls.HtmlInputHidden hdlallid = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdlallid");
                    CheckBox chkselect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkselect.Checked)
                    {
                        string[] temp = hdlallid.Value.ToString().Split('*');
                        SendMail(temp[0].ToString(), temp[1].ToString(), temp[2].ToString(), Convert.ToInt32(temp[3].ToString()));
                        icountt++;
                    }


                }
                if (icountt > 0)
                {
                    GetCartDetail();
                }

            }

        }


        /// <summary>
        /// Get the Cart Details
        /// </summary>
        public void GetCartDetail()
        {
            string strSql = "select s.ShoppingCartid,c.Email,c.FirstName,c.LastName,c.customerid,c.storeID,tb_Store.StoreName,(select count(1) from tb_ShoppingcartItems sc  where sc.ShoppingCartid  = s.ShoppingCartid) as 'Items',(select Top 1 IsMailSent from tb_ShoppingcartItems sc  where sc.ShoppingCartid  = s.ShoppingCartid) as IsMailSent,s.CreatedOn from tb_Shoppingcart s inner join tb_customer c on c.customerid = s.customerid inner join  tb_Store  on tb_Store.StoreID = c.storeID  where isnull(c.Email,'') <> '' ";
            if (ddlStore.SelectedIndex > 0)
            {
                strSql += " AND c.storeid=" + ddlStore.SelectedValue.ToString() + "";
            }
            //if (ddlSearch.SelectedIndex > 0 && txtSearch.Text.ToString().ToLower() != "search keyword")
            if (txtSearch.Text.ToString().ToLower() != "search keyword")
            {
                strSql += " AND " + ddlSearch.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }


            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql + " Order By s.CreatedOn DESC");
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grdAbandonedShoppingCart.DataSource = dsMail;
                grdAbandonedShoppingCart.DataBind();
                trdelete.Visible = true;
                btnSend.Visible = true;
            }
            else
            {
                trdelete.Visible = false;
                grdAbandonedShoppingCart.DataSource = null;
                grdAbandonedShoppingCart.DataBind();
                btnSend.Visible = false;
            }



        }

        /// <summary>
        /// Abandoned Shopping Cart Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdAbandonedShoppingCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAbandonedShoppingCart.PageIndex = e.NewPageIndex;
            GetCartDetail();
        }

        /// <summary>
        /// Delete Multiple record By Single click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grdAbandonedShoppingCart.Rows)
            {
                Label lblCustomerID = (Label)gr.FindControl("lblCustomerID");
                CheckBox chkRecord = (CheckBox)gr.FindControl("chkSelect");
                if (chkRecord.Checked)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_WishListItems where CustomerID=" + lblCustomerID.Text);
                    CommonComponent.ExecuteCommonData("Delete from tb_ShoppingcartItems  where Shoppingcartid=isnull((select  top 1 Shoppingcartid from tb_Shoppingcart where CustomerID= " + lblCustomerID.Text + "),0)");
                    CommonComponent.ExecuteCommonData("Delete from tb_Shoppingcart where CustomerID= " + lblCustomerID.Text);
                }
            }
            grdAbandonedShoppingCart.PageIndex = 0;
            GetCartDetail();
        }

        /// <summary>
        /// Abandoned Shopping Cart Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdAbandonedShoppingCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label IsMailSent = (Label)e.Row.FindControl("IsMailSent");
                if (!String.IsNullOrEmpty(IsMailSent.Text.ToString()) && (IsMailSent.Text.ToString().ToLower() == "true" || IsMailSent.Text.ToString() == "1"))
                {
                    IsMailSent.Text = "Yes";
                }
                else
                {
                    IsMailSent.Text = "No";
                }

                ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnEmail");
                btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Email-Reply.jpg";
            }
        }

        /// <summary>
        /// Abandoned Shopping Cart Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdAbandonedShoppingCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Email")
            {
                string[] temp = e.CommandArgument.ToString().Split('*');
                SendMail(temp[0].ToString(), temp[1].ToString(), temp[2].ToString(), Convert.ToInt32(temp[3].ToString()));
                GetCartDetail();
            }
        }

        /// <summary>
        /// Bind Cart Detail for mail
        /// </summary>
        /// <param name="ShoppingCartID">string ShoppingCartID</param>
        /// <param name="CustomerID">string CustomerID</param>
        public void BindCartDetail(string ShoppingCartID, string CustomerID)
        {
            try
            {
                StringBuilder sw = new StringBuilder(2000);

                sw.Append("<tr style='margin-bottom:15px;'>");
                sw.Append("<td style='margin-left:10px;font-weight:bold;font-family:Arial,Helvetica,sans-serif; font-size:13px;margin-top:15px;margin-bottom:15px;'>Your Shopping Cart</td>");
                sw.Append("</tr>");

                sw.Append("<tr>");
                sw.Append("<td >");

                sw.Append("<table style='padding-left: 10px; width:638px; ' class='popup_cantain' cellspacing='0' cellpadding='0' border='0' align='left' >");
                sw.Append("<tr>");
                sw.Append("<td>");
                string strSql = "select s.ShoppingCartid,c.Email,c.FirstName,c.customerid,sc.ProductID,dbo.tb_Product.Name,dbo.tb_Product.SKU, dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as Price,sc.Quantity,dbo.tb_Product.Deleted, dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory,c.StoreID from tb_Shoppingcart s inner join tb_customer c on c.customerid = s.customerid inner join tb_ShoppingcartItems  sc on sc.ShoppingCartid = s.ShoppingCartid INNER JOIN dbo.tb_Product ON sc.ProductID =dbo.tb_Product.ProductID AND sc.ShoppingCartid=" + ShoppingCartID + " and c.customerid=" + CustomerID + "";

                DataSet DsCItems = new DataSet();
                DsCItems = CommonComponent.GetCommonDataSet(strSql);

                if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                {
                    sw.Append("<div Class='datatable'>");
                    sw.Append("<table border='0' cellpadding='0' cellspacing='0' class='table' width='100%'> ");
                    sw.Append("<tbody>");
                    sw.Append("<tr>");
                    sw.Append("<th align='left' valign='middle' style='width:60%' ><b>Product</b></th>");
                    sw.Append("<th align='left' valign='middle' style='width:20%' ><b> SKU</b></th>");
                    sw.Append("<th align='center' valign='middle' style='width:10%'><b>Price</b></th>");
                    sw.Append("<th valign='middle' style='width: 20%;text-align:center;'><b>Quantity</b></th>");
                    sw.Append("</tr>");


                    for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                    {

                        sw.Append("<tr align='center'  valign='middle'>");
                        sw.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString());
                        sw.Append("</td>");
                        sw.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                        sw.Append("<td  align='left' >" + Math.Round(Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["price"].ToString()), 2) + "</td>");
                        sw.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                        sw.Append("</tr>");

                    }
                    sw.Append("</tbody>");
                    sw.Append("</table>");
                    sw.Append("</div>");
                    sw.Append("</td>");
                    sw.Append("</tr>");
                }
                else
                {
                    sw.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
                }

                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("</table>");

                sw.Append("</td>");
                sw.Append("</tr>");

                sw.Append("<tr align='left'>");
                sw.Append("<td style='text-align:center; padding-top:15px;margin-left:10px;font-family:Arial,Helvetica,sans-serif; font-size:13px;'>");
                sw.Append(" Click Here For View :-  <a href='http://www.halfpricedrapes.com/CheckoutCommon.aspx?&CID=" + CustomerID + "'>Your Shopping Cart</a>");
                sw.Append("</td>");
                sw.Append("</tr>");


                string strSqlForTopic = "Select * From tb_Topic where Deleted!=1 and TopicName = 'ShoppingCartDis'";
                DataSet DsCartTopic = new DataSet();
                DsCartTopic = CommonComponent.GetCommonDataSet(strSqlForTopic);

                sw.Append("<tr>");
                //sw.Append("<td Style='padding-top:20px;padding-left:140px;' ><img height='193' width='367' style='border-width:0px;'  src='" + AppLogic.AppConfig("LIVE_SERVER") + "/Client/images/gift_certificate25.jpg'/></td>");
                sw.Append("<td Style='padding-top:20px;' >");
                try
                {
                    sw.Append(DsCartTopic.Tables[0].Rows[0]["Description"].ToString());
                }
                catch
                { }
                sw.Append("</td>");
                sw.Append("</tr>");
            }
            catch { }
        }

        /// <summary>
        /// Bind Variant for product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns the Variant Details as a Literal Control</returns>
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

        /// <summary>
        /// This Function will send an E-Mail to Customer
        /// for Providing Registration Information of Customer
        /// </summary>
        /// <param name="ToAddress">string ToAddress</param>
        /// <param name="ShoppingCartID">string ShoppingCartID</param>
        /// <param name="CustomerID">string CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        private void SendMail(string ToAddress, string ShoppingCartID, string CustomerID, int StoreID)
        {

            try
            {
                CustomerComponent objCustomer = null;
                objCustomer = new CustomerComponent();
                DataSet dsMailTemplate = new DataSet();
                //dsMailTemplate = objCustomer.GetEmailTamplate("AbandonedShoppingCartEmail", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                dsMailTemplate = objCustomer.GetEmailTamplate("AbandonedShoppingCartEmail", StoreID);
                string Expiredate = DateTime.Now.Date.ToShortDateString();
                Expiredate = SecurityComponent.Encrypt(Expiredate);
                Expiredate = Server.UrlEncode(Expiredate);
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sw = new StringBuilder(2000);

                    string strSql = "select s.ShoppingCartid,c.Email,c.FirstName,c.customerid,sc.ProductID,dbo.tb_Product.Name,dbo.tb_Product.SKU, dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as ProductPrice,sc.Price as Price,sc.VariantNames,sc.VariantValues,sc.Quantity,dbo.tb_Product.Deleted, dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory,c.StoreID from tb_Shoppingcart s inner join tb_customer c on c.customerid = s.customerid inner join tb_ShoppingcartItems  sc on sc.ShoppingCartid = s.ShoppingCartid INNER JOIN dbo.tb_Product ON sc.ProductID =dbo.tb_Product.ProductID AND sc.ShoppingCartid=" + ShoppingCartID + " and c.customerid=" + CustomerID + "";

                    DataSet DsCItems = new DataSet();
                    DsCItems = CommonComponent.GetCommonDataSet(strSql);
                    sw.Append("<table border='0' cellpadding='0' cellspacing='0' class='datatable' width='100%'> ");
                    sw.Append("<tbody>");
                    if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                    {
                        sw.Append("<tr>");
                        sw.Append("<th align='left' valign='middle' style='width:60%' ><b>Product</b></th>");
                        sw.Append("<th align='left' valign='middle' style='width:20%' ><b> SKU</b></th>");
                        sw.Append("<th align='center' valign='middle' style='width:10%'><b>Price</b></th>");
                        sw.Append("<th valign='middle' style='width: 20%;text-align:center;'><b>Quantity</b></th>");
                        sw.Append("</tr>");


                        for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                        {

                            sw.Append("<tr align='center'  valign='middle'>");
                            if (DsCItems.Tables[0].Rows[i]["VariantNames"].ToString() != "" && DsCItems.Tables[0].Rows[i]["VariantValues"].ToString() != "")
                            {
                                BindVariantForProduct(DsCItems.Tables[0].Rows[i]["VariantNames"].ToString(), DsCItems.Tables[0].Rows[i]["VariantValues"].ToString());
                            }
                            else
                            {
                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                ltrvartable.Text = string.Empty;
                            }
                            sw.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString() + "<br />" + ltrvartable.Text + "");
                            sw.Append("</td>");
                            sw.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                            sw.Append("<td  align='left' >" + Math.Round(Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["price"].ToString()), 2) + "</td>");
                            sw.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                            sw.Append("</tr>");

                        }

                        sw.Append("<tr align='left'>");
                        sw.Append("<td colspan='4'  style='border:none;text-align:center; padding-top:15px;margin-left:10px;font-family:Arial,Helvetica,sans-serif; font-size:13px;'>");
                        sw.Append(" Click Here For View :-  <a href='http://www.halfpricedrapes.com/checkout/index?CID=" + CustomerID + "&Date=" + Expiredate + "'>Your Shopping Cart</a>");
                        sw.Append("</td>");
                        sw.Append("</tr>");



                    }
                    else
                    {
                        sw.Append("<tr>");
                        sw.Append("<td >");
                        sw.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
                        sw.Append("</td>");
                        sw.Append("</tr>");

                        sw.Append("<tr align='left'>");
                        sw.Append("<td colspan='4' style='border:none;text-align:center; padding-top:15px;margin-left:10px;font-family:Arial,Helvetica,sans-serif; font-size:13px;'>");
                        sw.Append(" Click Here For View :-  <a href='http://www.halfpricedrapes.com/checkout/index?CID=" + CustomerID + "&Date=" + Expiredate + "'>Your Shopping Cart</a>");
                        sw.Append("</td>");
                        sw.Append("</tr>");
                    }
                    sw.Append("</tbody>");
                    sw.Append("</table>");


                    string strSqlForTopic = "Select * From tb_Topic where Deleted!=1 and TopicName = 'ShoppingCartDis'";
                    DataSet DsCartTopic = new DataSet();
                    DsCartTopic = CommonComponent.GetCommonDataSet(strSqlForTopic);

                    sw.Append("<tr>");
                    sw.Append("<td Style='padding-top:10px;' >");
                    try
                    {
                        if (DsCartTopic != null && DsCartTopic.Tables[0].Rows.Count > 0)
                            sw.Append(DsCartTopic.Tables[0].Rows[0]["Description"].ToString());
                    }
                    catch
                    { }
                    sw.Append("</td>");
                    sw.Append("</tr>");


                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###CartDetail###", sw.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", StoreID.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    CommonOperations.SendMail(ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, null);
                    CommonComponent.ExecuteCommonData("update tb_ShoppingCartItems set IsMailSent=1 where ShoppingCartID=" + ShoppingCartID + "");
                    lblEmailMsg.Visible = true;
                    lblEmailMsg.Text = "Mail Send Successfully...";
                }
            }
            catch { }

        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            GetCartDetail();
            if (grdAbandonedShoppingCart.Rows.Count == 0)
                trdelete.Visible = false;
        }
    }
}