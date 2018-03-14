using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class EditFeedFieldMaster : BasePage
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btncancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                bindstore();
                bindFeedFieldType();
                ddlStore_SelectedIndexChanged(null, null);
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    BindData(Convert.ToInt32(Request.QueryString["ID"].ToString().Trim()));
                    lblTitle.Text = "Update Feed Field Master";
                }
                else lblTitle.Text = "Add Feed Field Master";
                if (Request.QueryString["StoreId"] != null && Request.QueryString["FeedId"] != null)
                {
                    try
                    {
                        ddlStore.SelectedValue = Request.QueryString["StoreId"].ToString();
                        ddlStore_SelectedIndexChanged(null, null);
                    }
                    catch { }
                    try
                    {
                        ddlFeed.SelectedValue = Request.QueryString["FeedId"].ToString();
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Binds the Data
        /// </summary>
        /// <param name="ID">int ID</param>
        public void BindData(int ID)
        {
            DataSet dsFeedField = new DataSet();
            FeedComponent objfield = new FeedComponent();
            dsFeedField = objfield.GetDatabyFeedFieldId(ID);
            if (dsFeedField != null && dsFeedField.Tables.Count > 0 && dsFeedField.Tables[0].Rows.Count > 0)
            {
                txtFieldName.Text = dsFeedField.Tables[0].Rows[0]["FieldName"].ToString();
                txtFeedDesc.Text = dsFeedField.Tables[0].Rows[0]["FieldDescription"].ToString();
                txtLimit.Text = dsFeedField.Tables[0].Rows[0]["FieldLimit"].ToString();
                txtHeight.Text = dsFeedField.Tables[0].Rows[0]["FieldHeight"].ToString();
                txtWidth.Text = dsFeedField.Tables[0].Rows[0]["FieldWidth"].ToString();
                chkIsBase.Checked = Convert.ToBoolean(dsFeedField.Tables[0].Rows[0]["isRequired"].ToString());
                txtDisplayOrder.Text = dsFeedField.Tables[0].Rows[0]["DisplayOrder"].ToString();
                ddlFeedType.SelectedValue = dsFeedField.Tables[0].Rows[0]["FieldTypeID"].ToString();
                ddlStore.SelectedValue = dsFeedField.Tables[0].Rows[0]["storeId"].ToString();
                ddlStore_SelectedIndexChanged(null, null);
                ddlFeed.SelectedValue = dsFeedField.Tables[0].Rows[0]["FeedID"].ToString();
                txtDefaultValue.Text = dsFeedField.Tables[0].Rows[0]["DefautValue"].ToString();

                if ((ddlFeedType.SelectedItem.Text.ToString().ToLower() == "combobox") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "radiobuttonlist") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "checkboxlist") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "label"))
                {
                    if (Request.QueryString["ID"] != null)
                    {
                        ltMore.Text = " <a href=\"javascript:void(0);\" onclick=\"javascript:window.open('PopupAddValue.aspx?FieldId=" + Request.QueryString["ID"].ToString() + "&TypeId=" + ddlFeedType.SelectedValue + "','More_Details','width=700,height=500,scrollbars=yes,left=150,top=100');\">Add Value</a> ";
                    }
                }

                if (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "category")
                {
                    trRootCate.Visible = true;
                    string CateRootId = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(FieldValues,'') AS FieldValues from tb_FeedFieldTypeValues Where TypeID =" + ddlFeedType.SelectedValue + " And FieldID=" + Request.QueryString["ID"].ToString() + ""));
                    if (!string.IsNullOrEmpty(CateRootId))
                    {
                        txtCateRootId.Text = CateRootId.ToString();
                    }
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Bind Stores into Drop down
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
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }

            int SID = Convert.ToInt32(AppLogic.AppConfigs("StoreId"));
            ListItem itm = ddlStore.Items.FindByValue(SID.ToString());
            ddlStore.SelectedIndex = ddlStore.Items.IndexOf(itm);
        }


        /// <summary>
        /// Binds the Type of the Feed Field
        /// </summary>
        private void bindFeedFieldType()
        {
            DataSet dsFieldType = new DataSet();

            FeedComponent objFeed = new FeedComponent();
            dsFieldType = objFeed.GetFeedFieldTypeMaster();

            if (dsFieldType != null && dsFieldType.Tables.Count > 0 && dsFieldType.Tables[0].Rows.Count > 0)
            {
                ddlFeedType.DataSource = dsFieldType;
                ddlFeedType.DataTextField = "TypeName";
                ddlFeedType.DataValueField = "FieldTypeID";
                ddlFeedType.DataBind();
                ddlFeedType.Items.Insert(0, new ListItem("Select Feed", "0"));
            }
            else
            {
                ddlFeedType.Items.Clear();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFeed.Items.Clear();
            DataSet dsFeedMaster = new DataSet();
            FeedComponent objFeed = new FeedComponent();

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

            if (ddlStore.SelectedValue != "0")
            {
                dsFeedMaster = objFeed.GetFeedData(Convert.ToInt32(ddlStore.SelectedValue), "");
                if (dsFeedMaster != null && dsFeedMaster.Tables.Count > 0 && dsFeedMaster.Tables[0].Rows.Count > 0)
                {
                    ddlFeed.DataSource = dsFeedMaster;
                    ddlFeed.DataTextField = "FeedName";
                    ddlFeed.DataValueField = "FeedId";
                    ddlFeed.DataBind();

                }
                else
                {
                    ddlFeed.DataSource = null;
                    ddlFeed.DataBind();
                }
                ddlFeed.Items.Insert(0, new ListItem("Select Feed", "0"));
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "$(document).ready( function() {jAlert('Select Store Name.', 'Message');});", true);
                return;
            }

        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            int flag = 0;
            if (trRootCate.Visible)
            {
                if (string.IsNullOrEmpty(txtCateRootId.Text.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@asd", "$(document).ready( function() {jAlert('Enter Category Root Id.', 'Message','ContentPlaceHolder1_txtCateRootId');});", true);
                    return;
                }
                else { flag = 1; }
            }
            else { flag = 1; }

            if (flag == 1)
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    Int32 Cnt = 0;
                    if (!string.IsNullOrEmpty(ltMore.Text))
                    {
                        Cnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(ISNULL(FieldValues,0)) as TotCnt from tb_FeedFieldTypeValues Where TypeID =" + ddlFeedType.SelectedValue + " And FieldID=" + Request.QueryString["Id"].ToString() + ""));
                        if (Cnt > 0)
                        {
                            if (!string.IsNullOrEmpty(txtCateRootId.Text.ToString()) && txtCateRootId.Visible == true)
                            {
                                CommonComponent.ExecuteCommonData("Update tb_FeedFieldTypeValues set FieldValues = '" + txtCateRootId.Text.Trim().Replace("''", "'") + "' Where FieldID=" + Request.QueryString["Id"].ToString() + " and TypeID=" + ddlFeedType.SelectedValue + "");
                                txtCateRootId.Text = "";
                            }
                            Int32 Result = Convert.ToInt32(InsertFeedField());
                            if (Result > 0)
                            {
                                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Insert", "alert('Record Updated Successfully.');window.location.href='ListFeedFieldMaster.aspx';", true);
                                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Insert", "$(document).ready( function() {jAlert('Record Updated Successfully.', 'Message');window.location.href='ListFeedFieldMaster.aspx';});", true);
                                Response.Redirect("/Admin/FeedManagement/ListFeedFieldMaster.aspx?status=update");
                            }
                        }
                        else
                        {
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Insert", "alert('Please Assign Value of Feed Type : " + ddlFeedType.SelectedItem.Text.ToString() + "');", true);
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@chkInsert", "$(document).ready( function() {jAlert('Please Assign Value of Feed Type : " + ddlFeedType.SelectedItem.Text.ToString() + "', 'Message');});", true);
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtCateRootId.Text.ToString()) && txtCateRootId.Visible == true)
                        {
                            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(ISNULL(FieldValues,0)) as TotCnt from tb_FeedFieldTypeValues Where TypeID =" + ddlFeedType.SelectedValue + " And FieldID=" + Request.QueryString["Id"].ToString() + "")) == 0)
                            {
                                CommonComponent.ExecuteCommonData("Insert into tb_FeedFieldTypeValues(FieldID,TypeID,FieldValues) Values(" + Request.QueryString["Id"].ToString() + "," + ddlFeedType.SelectedValue + ",'" + txtCateRootId.Text.Trim().Replace("''", "'") + "')");
                            }
                            else
                            {
                                CommonComponent.ExecuteCommonData("Update tb_FeedFieldTypeValues set FieldValues = '" + txtCateRootId.Text.Trim().Replace("''", "'") + "' Where FieldID=" + Request.QueryString["Id"].ToString() + " and TypeID=" + ddlFeedType.SelectedValue + "");
                            }
                        }
                        Int32 Result = Convert.ToInt32(InsertFeedField());
                        if (Result > 0)
                        {
                            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Insert", "alert('Record Updated Successfully.');window.location.href='ListFeedFieldMaster.aspx';", true);
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedupdate", "$(document).ready( function() {jAlert('Record Updated Successfully.', 'Message');window.location.href='ListFeedFieldMaster.aspx';});", true);
                            Response.Redirect("/Admin/FeedManagement/ListFeedFieldMaster.aspx?status=update");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedupdate", "$(document).ready( function() {jAlert('Problem while updating record', 'Message');});", true);
                            return;
                        }
                    }
                }
                else
                {
                    Int32 Result = Convert.ToInt32(InsertFeedField());
                    if (Result > 0)
                    {
                        if (!string.IsNullOrEmpty(txtCateRootId.Text.ToString()) && txtCateRootId.Visible == true)
                        {
                            CommonComponent.ExecuteCommonData("Insert into tb_FeedFieldTypeValues(FieldID,TypeID,FieldValues) Values(" + Result.ToString() + "," + ddlFeedType.SelectedValue + ",'" + txtCateRootId.Text.Trim().Replace("''", "'") + "')");
                            txtCateRootId.Text = "";
                        }
                        // Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Insert", "alert('Record Inserted Successfully.');", true);
                        // Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Record Inserted successfully.', 'Message');});", true);
                        Response.Redirect("/Admin/FeedManagement/ListFeedFieldMaster.aspx?status=insert");
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Msg", "alert('Field name already exists, please specify another field name...');", true);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Field name already exists, please specify another field name.', 'Message');});", true);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Insert Feed Field
        /// </summary>
        /// <returns>Returns the Identity Value</returns>
        private Int32 InsertFeedField()
        {
            int Result = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            FeedComponent objFeed = new FeedComponent();
            tb_FeedFieldMaster tb_FeedField = new tb_FeedFieldMaster();
            tb_FeedField.FieldName = txtFieldName.Text.Trim().ToString().Replace("''", "'");
            tb_FeedField.isRequired = chkIsBase.Checked;

            Int32 FieldTypeID = Convert.ToInt32(ddlFeedType.SelectedValue.ToString());
            tb_FeedField.tb_FeedFieldTypeMasterReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_FeedFieldTypeMaster", "FieldTypeID", Convert.ToInt32(FieldTypeID));

            tb_FeedField.FieldDescription = txtFeedDesc.Text.Trim().ToString().Replace("''", "'");

            if (!string.IsNullOrEmpty(txtDefaultValue.Text))
            {
                if (ddlFeedType.SelectedItem.Text.ToString().ToLower().Trim() == "calendar")
                {
                    DateTime dt = new DateTime();
                    try
                    {
                        dt = Convert.ToDateTime(txtDefaultValue.Text.Trim().ToString());
                    }
                    catch { dt = DateTime.Now; }
                    tb_FeedField.DefautValue = Convert.ToString(dt);
                }
                else
                {
                    tb_FeedField.DefautValue = txtDefaultValue.Text.Trim().ToString().Replace("''", "'");
                }
            }
            else
            {
                tb_FeedField.DefautValue = "";
            }

            if (!string.IsNullOrEmpty(txtWidth.Text.Trim()))
                tb_FeedField.FieldWidth = Convert.ToInt32(txtWidth.Text.Trim());
            else
                tb_FeedField.FieldWidth = 100;
            if (!string.IsNullOrEmpty(txtHeight.Text.Trim()))
                tb_FeedField.FieldHeight = Convert.ToInt32(txtHeight.Text.Trim());
            else
                tb_FeedField.FieldHeight = 100;
            if (!string.IsNullOrEmpty(txtLimit.Text.Trim()))
                tb_FeedField.FieldLimit = Convert.ToInt32(txtLimit.Text.Trim());
            else
                tb_FeedField.FieldLimit = 100;

            if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()))
                tb_FeedField.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());

            Int32 FeedID = Convert.ToInt32(ddlFeed.SelectedValue);
            tb_FeedField.tb_FeedMasterReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_FeedMaster", "FeedID", Convert.ToInt32(FeedID));

            Int32 StoreId = Convert.ToInt32(ddlStore.SelectedValue);
            tb_FeedField.StoreId = StoreId;
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
            {
                tb_FeedField.FieldID = Convert.ToInt32(Request.QueryString["Id"]);
            }
            else
            {

            }
            Result = objFeed.InsertFeedField(tb_FeedField, Convert.ToInt32(ddlStore.SelectedValue));
            return Result;

        }

        /// <summary>
        /// Feed Type Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlFeedType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "category")
            {
                trRootCate.Visible = true;
            }
            else { trRootCate.Visible = false; }

            if ((ddlFeedType.SelectedItem.Text.ToString().ToLower() == "combobox") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "radiobuttonlist") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "checkboxlist") || (ddlFeedType.SelectedItem.Text.ToString().ToLower() == "label"))
            {
                if (Request.QueryString["ID"] != null)
                {
                    ltMore.Text = " <a href=\"javascript:void(0);\" onclick=\"window.open('PopupAddValue.aspx?FieldId=" + Request.QueryString["ID"].ToString() + "&TypeId=" + ddlFeedType.SelectedValue + "','More Details','width=700,height=500,scrollbars=yes,left=150,top=100');\">Add Value</a> ";
                }
            }
            else { ltMore.Text = ""; }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btncancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/FeedManagement/ListFeedFieldMaster.aspx");
        }
    }
}