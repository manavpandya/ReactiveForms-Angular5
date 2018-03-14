using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN
{
    public partial class EmailSchedulesList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ClearData();
                BindData();
                BindGroupName();
                BindDay();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                imgUpdate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update.png";
            }
        }


        protected void RptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void RptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int DailyScheduleID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "cmd_clone")
            {
                Label lblDayoftheWeek = (Label)e.Item.FindControl("lblDayoftheWeek");
                Label lblStartTime = (Label)e.Item.FindControl("lblStartTime");
                HiddenField hidGroupID = (HiddenField)e.Item.FindControl("hidGroupID");


                if (lblDayoftheWeek == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Week is required.', 'Message','');", true);
                    return;
                }

                if (lblStartTime == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Time is required.', 'Message','');", true);
                    return;
                }

                if (hidGroupID == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Group is required.', 'Message','');", true);
                    return;
                }

                int AdminID = 0;
                int.TryParse(Convert.ToString(Session["AdminID"]), out AdminID);

                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO tb_DailySchedules(StoreID,DayoftheWeek,StartTime,ScheduleGroupID,IsDeleted,IsApproved,CreatedBy,CreatedDate)");
                query.Append("VALUES(1,'" + lblDayoftheWeek.Text + "','" + lblStartTime.Text + "'," + hidGroupID.Value + ",0,1," + AdminID + ",'" + System.DateTime.Now + "')");

                Solution.Bussines.Components.CommonComponent.GetScalarCommonData(Convert.ToString(query));
                BindData();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record Clone successfully', 'Message','');", true);
                return;
            }

            if (e.CommandName == "cmd_delete")
            {

                StringBuilder query = new StringBuilder();
                query.Append("Update tb_DailySchedules SET IsDeleted = 1 Where DailyScheduleID = " + DailyScheduleID);

                Solution.Bussines.Components.CommonComponent.GetScalarCommonData(Convert.ToString(query));
                BindData();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record Deleted successfully', 'Message','');", true);
                return;

            }

            if (e.CommandName == "cmd_edit")
            {

                Label lblDayoftheWeek = (Label)e.Item.FindControl("lblDayoftheWeek");
                Label lblStartTime = (Label)e.Item.FindControl("lblStartTime");
                HiddenField hidGroupID = (HiddenField)e.Item.FindControl("hidGroupID");


                if (lblDayoftheWeek == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Week is required.', 'Message','');", true);
                    return;
                }

                if (lblStartTime == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Time is required.', 'Message','');", true);
                    return;
                }

                if (hidGroupID == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Group is required.', 'Message','');", true);
                    return;
                }

                ClearData();
                ddlDay.Items.FindByText(lblDayoftheWeek.Text).Selected = true;
                //ddlDay.SelectedItem.Text = lblDayoftheWeek.Text;
                txtTime.Text = lblStartTime.Text;
                ddlGroupName.SelectedValue = hidGroupID.Value;
                hidDailyScheduleID.Value = Convert.ToString(e.CommandArgument);
                imgSave.Visible = false;
                imgUpdate.Visible = true;

            }

            if (e.CommandName == "cmd_send")
            {
                HiddenField hidGroupID = (HiddenField)e.Item.FindControl("hidGroupID");

                if (hidGroupID == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Day of the Group is required.', 'Message','');", true);
                    return;
                }

                Label GroupName = (Label)e.Item.FindControl("lblGroup");


                #region mail sent start here




                if (GroupName.Text == "ReviewReminder")
                {
                    #region For ReviewReminder
                    DataSet dtReview = new DataSet();
                    dtReview = GetReviewEmailProduct();

                    DataSet dtTemplate = new DataSet();
                    dtTemplate = GetScheduleEmailTemplate(Convert.ToInt32(hidGroupID.Value));

                    if (dtReview != null && dtReview.Tables.Count > 0 && dtReview.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dtReview.Tables[0].Rows.Count; i++)
                        {
                            // tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl
                            string CustomerName = string.Empty;
                            string ProductUrl = string.Empty;
                            string ShippingEmail = string.Empty;
                            string CustomerEmail = string.Empty;

                            CustomerName = string.Concat(Convert.ToString(dtReview.Tables[0].Rows[i]["FirstName"]), " ", Convert.ToString(dtReview.Tables[0].Rows[i]["LastName"]));
                            ProductUrl = Convert.ToString(dtReview.Tables[0].Rows[i]["BackupProductUrl"]);
                            CustomerEmail = Convert.ToString(dtReview.Tables[0].Rows[i]["Email"]);
                            ShippingEmail = Convert.ToString(dtReview.Tables[0].Rows[i]["ShippingEmail"]);


                            if (dtTemplate != null && dtTemplate.Tables.Count > 0 && dtTemplate.Tables[0].Rows.Count > 0)
                            {
                                string Description = string.Empty;
                                string Subject = string.Empty;
                                string UserIP = string.Empty;
                                Description = Convert.ToString(dtTemplate.Tables[0].Rows[0]["EmailBody"]);
                                if (!string.IsNullOrEmpty(CustomerName))
                                {
                                    Description = Description.Replace("###customername###", CustomerName);
                                }
                                else
                                {
                                    Description = Description.Replace("###customername###", "");
                                }

                                if (!string.IsNullOrEmpty(ProductUrl))
                                {
                                    Description = Description.Replace("###ProductLink####", ProductUrl);
                                }
                                else
                                {
                                    Description = Description.Replace("###ProductLink####", "");
                                }

                                Subject = Convert.ToString(dtTemplate.Tables[0].Rows[0]["Subject"]);
                                UserIP = Request.UserHostAddress;
                                if (!string.IsNullOrEmpty(CustomerEmail))
                                {
                                    DataSet dtHeaderFooter = new DataSet();

                                    if (dtHeaderFooter != null && dtHeaderFooter.Tables.Count > 0 && dtHeaderFooter.Tables[0].Rows.Count > 0)
                                    {
                                        StringBuilder EmailBody = new StringBuilder();
                                        string TempHeaderBodyHeader = Convert.ToString(dtHeaderFooter.Tables[0].Rows[0]["EmailBody"]);
                                        string TempHeaderBodyContant = Convert.ToString(GetContentPart());
                                        string TempHeaderBodyFooter = Convert.ToString(dtHeaderFooter.Tables[0].Rows[1]["EmailBody"]);

                                        TempHeaderBodyContant = TempHeaderBodyContant.Replace("###ContentPart###", Description);

                                        EmailBody.Append(Convert.ToString(TempHeaderBodyHeader));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyContant));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyFooter));

                                      //  Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(CustomerEmail, Subject, Convert.ToString(EmailBody), UserIP, true, null);

                                    }

                                    
                                }

                                if (!string.IsNullOrEmpty(ShippingEmail))
                                {
                                    if (ShippingEmail.Equals(CustomerEmail) == false)
                                    {
                                        DataSet dtHeaderFooter = new DataSet();

                                        if (dtHeaderFooter != null && dtHeaderFooter.Tables.Count > 0 && dtHeaderFooter.Tables[0].Rows.Count > 0)
                                        {
                                            StringBuilder EmailBody = new StringBuilder();
                                            string TempHeaderBodyHeader = Convert.ToString(dtReview.Tables[0].Rows[0]["EmailBody"]);
                                            string TempHeaderBodyContant = Convert.ToString(GetContentPart());
                                            string TempHeaderBodyFooter = Convert.ToString(dtReview.Tables[0].Rows[1]["EmailBody"]);

                                            TempHeaderBodyContant = TempHeaderBodyContant.Replace("###ContentPart###", Description);

                                            EmailBody.Append(Convert.ToString(TempHeaderBodyHeader));
                                            EmailBody.Append(Convert.ToString(TempHeaderBodyContant));
                                            EmailBody.Append(Convert.ToString(TempHeaderBodyFooter));

                                          //  Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(ShippingEmail, Subject, Convert.ToString(EmailBody), UserIP, true, null);

                                        }

                                         
                                    }

                                }


                            }

                        }


                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + GroupName.Text + " Email Not Found.', 'Message','');", true);
                        return;
                    }
                    #endregion
                }
                else if (GroupName.Text == "Swatch") // need to change  WishlistShoppingCartEmail to Swatch replace 
                {
                    #region Swatch
                    DataSet dtSwatch = new DataSet();
                    dtSwatch = GetSwatchDetail();

                    DataSet dtTemplate = new DataSet();
                    dtTemplate = GetScheduleEmailTemplate(Convert.ToInt32(hidGroupID.Value));

                    if (dtSwatch != null && dtSwatch.Tables.Count > 0 && dtSwatch.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dtSwatch.Tables[0].Rows.Count; i++)
                        {
                            // tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl
                            string CustomerName = string.Empty;
                            string ProductUrl = string.Empty;
                            string ShippingEmail = string.Empty;
                            string CustomerEmail = string.Empty;

                            CustomerName = string.Concat(Convert.ToString(dtSwatch.Tables[0].Rows[i]["FirstName"]), " ", Convert.ToString(dtSwatch.Tables[0].Rows[i]["LastName"]));
                            ProductUrl = Convert.ToString(dtSwatch.Tables[0].Rows[i]["BackupProductUrl"]);
                            CustomerEmail = Convert.ToString(dtSwatch.Tables[0].Rows[i]["Email"]);
                            ShippingEmail = Convert.ToString(dtSwatch.Tables[0].Rows[i]["ShippingEmail"]);


                            if (dtTemplate != null && dtTemplate.Tables.Count > 0 && dtTemplate.Tables[0].Rows.Count > 0)
                            {
                                string Description = string.Empty;
                                string Subject = string.Empty;
                                string UserIP = string.Empty;
                                Description = Convert.ToString(dtTemplate.Tables[0].Rows[0]["EmailBody"]);
                                Description = Description.Replace("###customername###", CustomerName);
                                Description = Description.Replace("###ProductLink####", ProductUrl);
                                Subject = Convert.ToString(dtTemplate.Tables[0].Rows[0]["Subject"]);
                                UserIP = Request.UserHostAddress;
                                if (!string.IsNullOrEmpty(CustomerEmail))
                                {

                                    DataSet dtHeaderFooter = new DataSet();

                                    if (dtHeaderFooter != null && dtHeaderFooter.Tables.Count > 0 && dtHeaderFooter.Tables[0].Rows.Count > 0)
                                    {
                                        StringBuilder EmailBody = new StringBuilder();
                                        string TempHeaderBodyHeader = Convert.ToString(dtHeaderFooter.Tables[0].Rows[0]["EmailBody"]);
                                        string TempHeaderBodyContant = Convert.ToString(GetContentPart());
                                        string TempHeaderBodyFooter = Convert.ToString(dtHeaderFooter.Tables[0].Rows[1]["EmailBody"]);

                                        TempHeaderBodyContant = TempHeaderBodyContant.Replace("###ContentPart###", Description);

                                        EmailBody.Append(Convert.ToString(TempHeaderBodyHeader));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyContant));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyFooter));

                                        //Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(CustomerEmail, Subject, Convert.ToString(EmailBody), UserIP, true, null);

                                    }


                                  //  Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(CustomerEmail, Subject, Description, UserIP, true, null);
                                }

                               


                            }

                        }


                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + GroupName.Text + " Email Not Found.', 'Message','');", true);
                        return;
                    }
                    #endregion
                }
                else if (GroupName.Text == "AbandonedShoppingCartEmail")
                {
                    #region AbandonCart
                    DataSet dtAbandonCart = new DataSet();
                    dtAbandonCart = GetAbandonCart();

                    DataSet dtTemplate = new DataSet();
                    dtTemplate = GetScheduleEmailTemplate(Convert.ToInt32(hidGroupID.Value));

                    if (dtAbandonCart != null && dtAbandonCart.Tables.Count > 0 && dtAbandonCart.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAbandonCart.Tables[0].Rows.Count; i++)
                        {
                            string CartDetail = string.Empty;
                            int CustomerID = 0;
                            CustomerID = Convert.ToInt32(dtAbandonCart.Tables[0].Rows[i]["customerid"]);
                            CartDetail = GetCartProductWithHtml(CustomerID);


                            string CustomerEmail = string.Empty;
                            string StoreID = string.Empty;
                            string UserIP = string.Empty;
                            CustomerEmail = Convert.ToString(dtAbandonCart.Tables[0].Rows[i]["Email"]);
                            StoreID = Convert.ToString(dtAbandonCart.Tables[0].Rows[i]["StoreID"]);
                            UserIP = Request.UserHostAddress;

                            if (dtTemplate != null && dtTemplate.Tables.Count > 0 && dtTemplate.Tables[0].Rows.Count > 0)
                            {

                                String strBody = "";
                                String strSubject = "";
                                strBody = dtTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                                strSubject = dtTemplate.Tables[0].Rows[0]["Subject"].ToString();
                                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###CartDetail###", CartDetail.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###StoreID###", StoreID.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                                if (!string.IsNullOrEmpty(CustomerEmail))
                                {
                                    DataSet dtHeaderFooter = new DataSet();

                                    if (dtHeaderFooter != null && dtHeaderFooter.Tables.Count > 0 && dtHeaderFooter.Tables[0].Rows.Count > 0)
                                    {
                                        StringBuilder EmailBody = new StringBuilder();
                                        string TempHeaderBodyHeader = Convert.ToString(dtHeaderFooter.Tables[0].Rows[0]["EmailBody"]);
                                        string TempHeaderBodyContant = Convert.ToString(GetContentPart());
                                        string TempHeaderBodyFooter = Convert.ToString(dtHeaderFooter.Tables[0].Rows[1]["EmailBody"]);

                                        TempHeaderBodyContant = TempHeaderBodyContant.Replace("###ContentPart###", strBody);

                                        EmailBody.Append(Convert.ToString(TempHeaderBodyHeader));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyContant));
                                        EmailBody.Append(Convert.ToString(TempHeaderBodyFooter));

                                        // Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(CustomerEmail, strSubject, Convert.ToString(EmailBody), UserIP, true, null);

                                    }

                                    // Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(CustomerEmail, strSubject, strBody, UserIP, true, null);
                                }

                            }
                        }


                    }
                    else {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + GroupName.Text + " Email Not Found.', 'Message','');", true);
                        return;
                    }

                    #endregion
                }

                #endregion mail End

            }

        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            int AdminID = 0;
            int.TryParse(Convert.ToString(Session["AdminID"]), out AdminID);

            if (Convert.ToInt32(ddlDay.SelectedItem.Value) <= 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Day.', 'Message','');", true);
                return;
            }

            if (string.IsNullOrEmpty(txtTime.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Time.', 'Message','');", true);
                return;
            }


            if (Convert.ToInt32(ddlGroupName.SelectedItem.Value) < 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select From Group.', 'Message','');", true);
                return;
            }

            DateTime time = Convert.ToDateTime(txtTime.Text);
            StringBuilder query = new StringBuilder();
            query.Append("INSERT INTO tb_DailySchedules(StoreID,DayoftheWeek,StartTime,ScheduleGroupID,IsDeleted,IsApproved,CreatedBy,CreatedDate)");
            query.Append("VALUES(1,'" + ddlDay.SelectedItem.Text + "','" + time.ToShortTimeString() + "'," + ddlGroupName.SelectedItem.Value + ",0,1," + AdminID + ",'" + System.DateTime.Now + "')");

            Solution.Bussines.Components.CommonComponent.GetScalarCommonData(Convert.ToString(query));
            ClearData();
            BindData();
            BindGroupName();
            BindDay();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record inserted successfully', 'Message','');", true);
            return;


        }

        protected void imgUpdate_Click(object sender, ImageClickEventArgs e)
        {


            int AdminID = 0;
            int.TryParse(Convert.ToString(Session["AdminID"]), out AdminID);

            if (Convert.ToInt32(ddlDay.SelectedItem.Value) <= 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Day.', 'Message','');", true);
                return;
            }

            if (string.IsNullOrEmpty(txtTime.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Time.', 'Message','');", true);
                return;
            }


            if (Convert.ToInt32(ddlGroupName.SelectedItem.Value) < 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select From Group.', 'Message','');", true);
                return;
            }

            DateTime time = Convert.ToDateTime(txtTime.Text);
            StringBuilder query = new StringBuilder();
            query.Append("Update tb_DailySchedules SET DayoftheWeek= '" + ddlDay.SelectedItem.Text + "',StartTime = '" + time.ToShortTimeString() + "',ScheduleGroupID = " + ddlGroupName.SelectedItem.Value + ",CreatedBy = " + AdminID + ",CreatedDate = '" + System.DateTime.Now + "' Where DailyScheduleID = " + hidDailyScheduleID.Value);

            Solution.Bussines.Components.CommonComponent.GetScalarCommonData(Convert.ToString(query));
            ClearData();
            BindData();
            BindGroupName();
            BindDay();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('record Updated successfully', 'Message','');", true);
            return;

        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            ClearData();
        }

        #region Private Function
        private void BindData()
        {
            System.Data.DataSet dtDailySchedules = new System.Data.DataSet();
            dtDailySchedules = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("select tds.*,tst.Label,tst.TemplateID from tb_DailySchedules as tds inner join tb_ScheduleEmailTemplate as tst ON tst.TemplateID = tds.ScheduleGroupID AND isnull(tst.IsDeleted,0) = 0 AND ISNULL(tst.IsApproved,0) = 1 WHERE ISNULL(tds.IsApproved,0) = 1 AND ISNULL(tds.IsDeleted,0) = 0");

            if (dtDailySchedules != null && dtDailySchedules.Tables.Count > 0 && dtDailySchedules.Tables[0].Rows.Count > 0)
            {
                RptList.DataSource = dtDailySchedules;
                RptList.DataBind();
            }
            else
            {
                RptList.DataSource = dtDailySchedules;
                RptList.DataBind();
            }
        }
        private void BindGroupName()
        {

            System.Data.DataSet GroupName = new System.Data.DataSet();
            GroupName = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("select * from tb_ScheduleEmailTemplate where ISNULL(IsDeleted,0) = 0 AND ISNULL(IsApproved,0) = 1 AND ISNULL(IsShow,0) = 1");

            if (GroupName != null && GroupName.Tables.Count > 0 && GroupName.Tables[0].Rows.Count > 0)
            {
                ddlGroupName.DataSource = GroupName;
                ddlGroupName.DataTextField = "Label";
                ddlGroupName.DataValueField = "TemplateID";
                ddlGroupName.DataBind();
                ddlGroupName.Items.Insert(0, new ListItem("-- Select Group --", "0"));
            }
            else
            {
                ddlGroupName.DataSource = null;
                ddlGroupName.DataBind();
                ddlGroupName.Items.Insert(0, new ListItem("-- Select Group --", "0"));
            }

        }

        private void BindDay()
        {

            //System.Data.DataSet dtDay = new System.Data.DataSet();
            //dtDay = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("select * from tb_DayoftheWeek where ISNULL(IsDeleted,0) = 0 AND ISNULL(IsApproved,0) = 1");

            //if (dtDay != null && dtDay.Tables.Count > 0 && dtDay.Tables[0].Rows.Count > 0)
            //{

            //    ddlDay.DataSource = dtDay;
            //    ddlDay.DataTextField = "DayoftheWeek";
            //    ddlDay.DataValueField = "DayoftheWeekID";
            //    ddlDay.DataBind();
            //    ddlDay.Items.Insert(0, new ListItem("-- Select Day --", "0"));
            //}
            //else
            //{
            //    ddlDay.DataSource = null;
            //    ddlDay.DataBind();
            //    ddlDay.Items.Insert(0, new ListItem("-- Select Day --", "0"));
            //}

        }

        private void ClearData()
        {
            ddlDay.ClearSelection();
            ddlGroupName.ClearSelection();
            txtTime.Text = string.Empty;
        }

        private DataSet GetReviewEmailProduct()
        {
            StringBuilder SBReview = new StringBuilder();

            SBReview.Append(" SELECT tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl ");
            SBReview.Append(" FROM tb_OrderShippedItems");
            SBReview.Append(" inner join tb_Order ON tb_Order.OrderNumber = tb_OrderShippedItems.OrderNumber");
            SBReview.Append(" inner join tb_Customer ON tb_Customer.CustomerID = tb_Order.CustomerID");
            SBReview.Append(" inner join tb_Product ON tb_Product.ProductID = tb_OrderShippedItems.RefProductID");
            SBReview.Append(" WHERE ISNULL(tb_Customer.Email,'') <> '' AND ISNULL(tb_Order.ShippingEmail,'') <> ''");
            SBReview.Append(" AND NOT EXISTS (SELECT TR.ProductID FROM tb_Rating TR WHERE TR.ProductID = tb_OrderShippedItems.RefProductID)");
            SBReview.Append(" AND  DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) >= DATEADD(hh, -24, GETDATE()) AND DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) <= DATEADD(hh, 0, GETDATE())");
            SBReview.Append(" GROUP BY tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl,tb_OrderShippedItems.ShippedOn");
            SBReview.Append(" ORDER BY tb_OrderShippedItems.ShippedOn DESC");

            System.Data.DataSet dtReviewEmail = new System.Data.DataSet();
            dtReviewEmail = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBReview));


            return dtReviewEmail;

        }

        private DataSet GetWishlistShoppingCartEmail()
        {


            StringBuilder SBWishlistShoppingCart = new StringBuilder();

            SBWishlistShoppingCart.Append(" SELECT tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl ");
            SBWishlistShoppingCart.Append(" FROM tb_OrderShippedItems");
            SBWishlistShoppingCart.Append(" inner join tb_Order ON tb_Order.OrderNumber = tb_OrderShippedItems.OrderNumber");
            SBWishlistShoppingCart.Append(" inner join tb_Customer ON tb_Customer.CustomerID = tb_Order.CustomerID");
            SBWishlistShoppingCart.Append(" inner join tb_Product ON tb_Product.ProductID = tb_OrderShippedItems.RefProductID");
            SBWishlistShoppingCart.Append(" WHERE ISNULL(tb_Customer.Email,'') <> '' AND ISNULL(tb_Order.ShippingEmail,'') <> ''");
            SBWishlistShoppingCart.Append(" AND NOT EXISTS (SELECT TR.ProductID FROM tb_Rating TR WHERE TR.ProductID = tb_OrderShippedItems.RefProductID)");
            SBWishlistShoppingCart.Append(" AND  DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) >= DATEADD(hh, -24, GETDATE()) AND DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) <= DATEADD(hh, 0, GETDATE())");
            SBWishlistShoppingCart.Append(" GROUP BY tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl,tb_OrderShippedItems.ShippedOn");
            SBWishlistShoppingCart.Append(" ORDER BY tb_OrderShippedItems.ShippedOn DESC");

            System.Data.DataSet dtWishlistShoppingCart = new System.Data.DataSet();
            dtWishlistShoppingCart = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBWishlistShoppingCart));


            return dtWishlistShoppingCart;

        }

        private DataSet GetAbandonCart()
        {
            StringBuilder SBAbandonCart = new StringBuilder();

            SBAbandonCart.Append("  select c.Email,c.FirstName,c.customerid,c.StoreID  ");
            SBAbandonCart.Append("  from tb_Shoppingcart s ");
            SBAbandonCart.Append("  inner join tb_customer c on c.customerid = s.customerid  ");
            SBAbandonCart.Append("  inner join tb_ShoppingcartItems  sc on sc.ShoppingCartid = s.ShoppingCartid ");
            SBAbandonCart.Append("  INNER JOIN dbo.tb_Product ON sc.ProductID =dbo.tb_Product.ProductID ");
            SBAbandonCart.Append(" AND  DATEADD(hh, 0, s.CreatedOn) >= DATEADD(hh, -24, GETDATE()) AND DATEADD(hh, 0, s.CreatedOn) <= DATEADD(hh, 0, GETDATE()) ");
            SBAbandonCart.Append("  GROUP BY c.Email,c.FirstName,c.customerid,c.StoreID ");
            SBAbandonCart.Append("  ORDER BY c.customerid ");

            System.Data.DataSet dtAbandonCart = new System.Data.DataSet();
            dtAbandonCart = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBAbandonCart));

            return dtAbandonCart;
        }

        private DataSet GetAbandonCartDetail(int CustomerID)
        {
            StringBuilder SBAbandonCart = new StringBuilder();

            SBAbandonCart.Append("  select s.ShoppingCartid,c.Email,c.FirstName,c.customerid,sc.ProductID,dbo.tb_Product.Name,dbo.tb_Product.SKU, dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as ProductPrice,sc.Price as Price,sc.VariantNames,sc.VariantValues,sc.Quantity,dbo.tb_Product.Deleted, dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory,c.StoreID ");
            SBAbandonCart.Append(" from tb_Shoppingcart s ");
            SBAbandonCart.Append(" inner join tb_customer c on c.customerid = s.customerid ");
            SBAbandonCart.Append(" inner join tb_ShoppingcartItems  sc on sc.ShoppingCartid = s.ShoppingCartid ");
            SBAbandonCart.Append(" INNER JOIN dbo.tb_Product ON sc.ProductID =dbo.tb_Product.ProductID and c.customerid=" + CustomerID + " ");
            SBAbandonCart.Append(" Where c.customerid=" + CustomerID + " AND  DATEADD(hh, 0, s.CreatedOn) >= DATEADD(hh, -24, GETDATE()) AND DATEADD(hh, 0, s.CreatedOn) <= DATEADD(hh, 0, GETDATE()) ");
            SBAbandonCart.Append(" ORDER BY c.customerid ");


            System.Data.DataSet dtAbandonCart = new System.Data.DataSet();
            dtAbandonCart = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBAbandonCart));

            return dtAbandonCart;
        }

        private string GetCartProductWithHtml(int CustomerID)
        {
            StringBuilder sw = new StringBuilder();

            // string strSql = "select s.ShoppingCartid,c.Email,c.FirstName,c.customerid,sc.ProductID,dbo.tb_Product.Name,dbo.tb_Product.SKU, dbo.tb_Product.SEName, case when isnull(dbo.tb_Product.SalePrice,0) = 0 then  isnull(dbo.tb_Product.Price,0) else isnull(dbo.tb_Product.SalePrice,0) end as ProductPrice,sc.Price as Price,sc.VariantNames,sc.VariantValues,sc.Quantity,dbo.tb_Product.Deleted, dbo.tb_Product.ImageName, dbo.tb_Product.MainCategory,c.StoreID from tb_Shoppingcart s inner join tb_customer c on c.customerid = s.customerid inner join tb_ShoppingcartItems  sc on sc.ShoppingCartid = s.ShoppingCartid INNER JOIN dbo.tb_Product ON sc.ProductID =dbo.tb_Product.ProductID AND sc.ShoppingCartid=" + ShoppingCartID + " and c.customerid=" + CustomerID + "";

            string Expiredate = DateTime.Now.Date.ToShortDateString();
            Expiredate = Solution.Bussines.Components.SecurityComponent.Encrypt(Expiredate);

            DataSet DsCItems = new DataSet();
            DsCItems = GetAbandonCartDetail(CustomerID);

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
                    string RetString = string.Empty;
                    sw.Append("<tr align='center'  valign='middle'>");
                    if (DsCItems.Tables[0].Rows[i]["VariantNames"].ToString() != "" && DsCItems.Tables[0].Rows[i]["VariantValues"].ToString() != "")
                    {
                        RetString = BindVariantForProduct(DsCItems.Tables[0].Rows[i]["VariantNames"].ToString(), DsCItems.Tables[0].Rows[i]["VariantValues"].ToString());
                    }

                    sw.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString() + "<br />" + RetString + "");
                    sw.Append("</td>");
                    sw.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                    sw.Append("<td  align='left' >" + Math.Round(Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["price"].ToString()), 2) + "</td>");
                    sw.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Quantity"].ToString() + "</td>");
                    sw.Append("</tr>");

                }

                sw.Append("<tr align='left'>");
                sw.Append("<td colspan='4'  style='border:none;text-align:center; padding-top:15px;margin-left:10px;font-family:Arial,Helvetica,sans-serif; font-size:13px;'>");
                sw.Append(" Click Here For View :-  <a href='http://www.halfpricedrapes.com/CheckoutCommon.aspx?CID=" + CustomerID + "&Date=" + Expiredate + "'>Your Shopping Cart</a>");
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
                sw.Append(" Click Here For View :-  <a href='http://www.halfpricedrapes.com/CheckoutCommon.aspx?CID=" + CustomerID + "&Date=" + Expiredate + "'>Your Shopping Cart</a>");
                sw.Append("</td>");
                sw.Append("</tr>");
            }
            sw.Append("</tbody>");
            sw.Append("</table>");

            return Convert.ToString(sw);

        }

        public string BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            string retString = string.Empty;

            if (varname.Length > 0)
            {

                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
            }
            if (sbvartable.ToString() != "")
            {
                retString = sbvartable.ToString();
            }
            else
            {
                retString = string.Empty;
            }
            return retString;
        }

        private DataSet GetSwatchDetail()
        {
            StringBuilder SBSwatch = new StringBuilder();

            SBSwatch.Append(" SELECT tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl  ");
            SBSwatch.Append(" FROM tb_OrderShippedItems  ");
            SBSwatch.Append(" inner join tb_Order ON tb_Order.OrderNumber = tb_OrderShippedItems.OrderNumber  ");
            SBSwatch.Append(" inner join tb_Customer ON tb_Customer.CustomerID = tb_Order.CustomerID  ");
            SBSwatch.Append(" inner join tb_Product ON tb_Product.ProductID = tb_OrderShippedItems.RefProductID AND ItemType = 'Swatch'  ");
            SBSwatch.Append(" WHERE ISNULL(tb_Customer.Email,'') <> '' AND ISNULL(tb_Order.ShippingEmail,'') <> ''   ");
            SBSwatch.Append(" AND  DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) >= DATEADD(hh, -24, GETDATE()) AND DATEADD(hh, 0, tb_OrderShippedItems.ShippedOn) <= DATEADD(hh, 0, GETDATE())  ");
            SBSwatch.Append(" GROUP BY tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Order.ShippingEmail ,tb_Product.BackupProductUrl,tb_OrderShippedItems.ShippedOn  ");
            SBSwatch.Append(" ORDER BY tb_OrderShippedItems.ShippedOn DESC  ");

            System.Data.DataSet dtSBSwatch = new System.Data.DataSet();
            dtSBSwatch = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBSwatch));

            return dtSBSwatch;

        }

        private string GetContentPart()
        {
            string HeaderString = string.Empty;
            StringBuilder SBHeaderString = new StringBuilder();
            SBHeaderString.Append("<table width='680' cellspacing='0' cellpadding='0' border='0' align='center' class='mobile_content' style='margin:0 auto; background:#ffffff;'>");
            SBHeaderString.Append("<tbody>  ");
            SBHeaderString.Append("<tr>");
            SBHeaderString.Append("<td colspan='3'>");
            SBHeaderString.Append("###ContentPart###");
            SBHeaderString.Append("</td>");
            SBHeaderString.Append("</tr>");
            SBHeaderString.Append("</tbody>");
            SBHeaderString.Append("</table>");

            return HeaderString;

        }

        private DataSet GetHeaderFooterPart()
        {
        
            StringBuilder SBTemplate = new StringBuilder();
            SBTemplate.Append(" select * from tb_ScheduleEmailTemplate where isShow = 0 ");

            System.Data.DataSet dtTemplate = new System.Data.DataSet();
            dtTemplate = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBTemplate));


            return dtTemplate;

        
        }

        private DataSet GetScheduleEmailTemplate(int TemplateID)
        {

            StringBuilder SBTemplate = new StringBuilder();
            SBTemplate.Append("select * from tb_ScheduleEmailTemplate where TemplateID = " + TemplateID);

            System.Data.DataSet dtTemplate = new System.Data.DataSet();
            dtTemplate = Solution.Bussines.Components.CommonComponent.GetCommonDataSet(Convert.ToString(SBTemplate));

            return dtTemplate;
        }
        #endregion


    }
}