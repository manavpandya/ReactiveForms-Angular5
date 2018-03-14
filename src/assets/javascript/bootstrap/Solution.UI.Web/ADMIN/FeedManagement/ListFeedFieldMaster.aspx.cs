using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class ListFeedFieldMaster : BasePage
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
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnImportdata.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 62px; height: 23px; border:none;cursor:pointer;");

                if (Request.QueryString["status"] != null)
                {
                    if (Convert.ToString(Request.QueryString["status"]).ToLower() == "insert")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "qinsert", "$(document).ready( function() {jAlert('Record inserted successfully.', 'Message');});", true);
                    }
                    else if (Convert.ToString(Request.QueryString["status"]).ToLower() == "update")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "qupdate", "$(document).ready( function() {jAlert('Record updated successfully.', 'Message');});", true);
                    }
                }

                BindStore();
                BindData();
                ddlStore_SelectedIndexChanged(null, null);
                ddlStoreFrom_SelectedIndexChanged(null, null);
            }
        }


        /// <summary>
        /// Bind the Total Store Available in the Database
        /// </summary>
        public void BindStore()
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
            if (ddlStore.Items.Count > 0)
            {
                try
                {
                    if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                    {
                        ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                    }
                    else
                    {
                        ddlStore.SelectedIndex = 0;
                    }
                }
                catch
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
            BindStoreMethod();
        }

        /// <summary>
        /// Bind Store Method
        /// </summary>
        public void BindStoreMethod()
        {
            if (Session["StoreID"] != null)
            {
                int SID = Convert.ToInt32(Session["StoreID"]);
                ListItem itm = ddlStore.Items.FindByValue(SID.ToString());
                ddlStore.SelectedIndex = 0;
            }
            if (ddlStore.Items.Count > 0)
            {
                ArrayList textList = new ArrayList();
                ArrayList valueList = new ArrayList();
                foreach (ListItem li in ddlStore.Items)
                {
                    textList.Add(li.Text);
                }
                textList.Sort();
                foreach (object item in textList)
                {
                    string value = ddlStore.Items.FindByText(item.ToString()).Value;
                    valueList.Add(value);
                }
                ddlStoreFrom.Items.Clear();

                for (int i = 0; i < textList.Count; i++)
                {
                    ListItem objItem = new ListItem(textList[i].ToString(), valueList[i].ToString());
                    ddlStoreFrom.Items.Add(objItem);
                }
            }
        }

        /// <summary>
        /// Binds the Feed Mater data into gridview.
        /// </summary>
        public void BindData()
        {
            FeedComponent objFeedMaster = new FeedComponent();
            DataSet dsFeedMaster = new DataSet();
            if (!string.IsNullOrEmpty(ddlFeedName.SelectedValue) && (Convert.ToInt32(ddlFeedName.SelectedValue) > 0))
            {
                dsFeedMaster = objFeedMaster.GetFeedDetail(Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(ddlFeedName.SelectedValue));
                gvFeedMaster.DataSource = dsFeedMaster;
                gvFeedMaster.DataBind();
            }
            else
            {
                gvFeedMaster.DataSource = null;
                gvFeedMaster.DataBind();
            }
        }

        /// <summary>
        ///  Feed  Mater Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvFeedMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFeedMaster.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Feed Mater Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvFeedMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblTypeName = (Label)e.Row.FindControl("lblTypeName");
            Label lblFieldID = (Label)e.Row.FindControl("lblFieldID");
            Label lblFieldTypeID = (Label)e.Row.FindControl("lblFieldTypeID");
            HtmlAnchor tabaddvalue = (HtmlAnchor)e.Row.FindControl("tabaddvalue");
            if (lblTypeName != null)
            {
                if ((lblTypeName.Text.ToString().ToLower() == "combobox") || (lblTypeName.Text.ToString().ToLower() == "radiobuttonlist") || (lblTypeName.Text.ToString().ToLower() == "checkboxlist") || (lblTypeName.Text.ToString().ToLower() == "label"))
                {
                    tabaddvalue.Visible = true;
                    tabaddvalue.HRef = "javascript:void(0);";
                    tabaddvalue.Attributes.Add("onclick", "javascript:window.open('PopupAddValue.aspx?FieldId=" + lblFieldID.Text.ToString() + "&TypeId=" + lblFieldTypeID.Text.ToString() + "','More_Details','width=700,height=500,scrollbars=yes,left=150,top=100');");
                }
                else
                {
                    tabaddvalue.Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("hlEdit")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("btndel")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }


        /// <summary>
        /// Feed Master Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvFeedMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    Int32 ValueID = Convert.ToInt32(e.CommandArgument.ToString());
                    CommonComponent.ExecuteCommonData("Delete From tb_FeedFieldMaster Where FieldId=" + ValueID + "");
                    BindData();
                }
            }
            catch { }
        }

        /// <summary>
        /// StoreFrom Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStoreFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFeedNameStore.Items.Clear();
            DataSet dsFeedFieldType = new DataSet();
            FeedComponent objFeedMaster = new FeedComponent();
            if (ddlStoreFrom.Items.Count > 0)
            {
                if (ddlStoreFrom.SelectedValue != "0")
                {
                    dsFeedFieldType = objFeedMaster.GetFeedMasterByStore(Convert.ToInt32(ddlStoreFrom.SelectedValue));
                    if (dsFeedFieldType != null && dsFeedFieldType.Tables[0].Rows.Count > 0)
                    {
                        ddlFeedNameStore.DataSource = dsFeedFieldType.Tables[0];
                        ddlFeedNameStore.DataValueField = "FeedID";
                        ddlFeedNameStore.DataTextField = "FeedName";
                        ddlFeedNameStore.DataBind();
                    }
                }
            }
        }

        /// <summary>
        ///  Important Data Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnImportdata_Click(object sender, EventArgs e)
        {
            FeedComponent objFeed = new FeedComponent();
            DataSet dsFeed = new DataSet();
            if (ddlFeedNameStore.Items.Count > 0)
            {
                dsFeed = objFeed.GetFeedlistForClone(Convert.ToInt32(ddlFeedNameStore.SelectedValue.ToString()));
                if (dsFeed != null && dsFeed.Tables.Count > 0 && dsFeed.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFeed.Tables[0].Rows.Count; i++)
                    {
                        DataSet dsField = new DataSet();
                        dsField = objFeed.GetFeednameForClone(Convert.ToInt32(ddlFeedName.SelectedValue.ToString()), dsFeed.Tables[0].Rows[i]["FieldName"].ToString());
                        if (dsField != null && dsField.Tables.Count > 0 && dsField.Tables[0].Rows.Count > 0)
                        {
                            Int32 filedid = Convert.ToInt32(dsField.Tables[0].Rows[0]["FieldID"].ToString());
                            DataSet DsValues = new DataSet();
                            DsValues = objFeed.GetFeednameExist(Convert.ToInt32(dsField.Tables[0].Rows[0]["FieldID"].ToString()));

                            if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedName.SelectedValue + "")))
                            {
                                if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedNameStore.SelectedValue + "")))
                                {
                                    if (DsValues != null && DsValues.Tables[0].Rows.Count == 0)
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO  tb_FeedFieldTypeValues(TypeID,FieldID,FieldValues,DisplayOrder)SELECT TypeID," + filedid + ",FieldValues,DisplayOrder from tb_FeedFieldTypeValues WHERE FieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + "");
                                    }
                                    DataSet DsProductBase = new DataSet();
                                    DsProductBase = objFeed.GetFeedProductFieldExist(Convert.ToInt32(dsField.Tables[0].Rows[0]["FieldID"].ToString()));
                                    if (DsProductBase != null && DsProductBase.Tables[0].Rows.Count == 0)
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO  tb_FeedProductBaseMapping(FeedID,FieldID,ProductField)SELECT " + ddlFeedName.SelectedValue.ToString() + " as FeedId," + filedid + ",ProductField  from tb_FeedProductBaseMapping WHERE FeedID=" + ddlFeedNameStore.SelectedValue.ToString() + " AND FieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + " ");
                                    }
                                }
                                else
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please select Base feed.');document.getElementById('ContentPlaceHolder1_ddlFeedNameStore').focus();", true);
                                    return;
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedNameStore.SelectedValue + "")))
                                {

                                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please select Base feed.');document.getElementById('ContentPlaceHolder1_ddlFeedName').focus();", true);
                                    return;
                                }
                                else
                                {
                                    DataSet Dsmapping = new DataSet();
                                    Dsmapping = objFeed.GetFeedmappingFieldExist(Convert.ToInt32(dsField.Tables[0].Rows[0]["FieldID"].ToString()));
                                    if (Dsmapping != null && Dsmapping.Tables[0].Rows.Count == 0)
                                    {
                                        string strId = "";
                                        string strFeedId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT FeedID from tb_FeedMaster WHERE StoreID=" + ddlStore.SelectedValue.ToString() + " and IsBase=1"));

                                        if (!string.IsNullOrEmpty(strFeedId))
                                        {
                                            strId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT FieldID from tb_FeedProductBaseMapping WHERE ProductField in(SELECT ProductField from tb_FeedProductBaseMapping WHERE FieldID in (SELECT BaseFieldID from tb_FeedFieldMapping WHERE RelatedFieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + ")) and FeedID=" + strFeedId + ""));
                                            if (!string.IsNullOrEmpty(strId))
                                            {
                                                CommonComponent.ExecuteCommonData("INSERT INTO tb_FeedFieldMapping(BaseFeedID,BaseFieldID,RelatedFeedID,RelatedFieldID) VALUES (" + strFeedId + "," + strId + "," + ddlFeedName.SelectedValue.ToString() + "," + filedid + ")");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Int32 filedid = 0;

                            if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedName.SelectedValue + "")))
                            {
                                if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedNameStore.SelectedValue + "")))
                                {
                                    filedid = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO  tb_FeedFieldMaster([FeedID] ,[FieldName],[FieldTypeID],[FieldDescription],[FieldLimit],[FieldHeight],[FieldWidth],[isRequired],[DisplayOrder])SELECT " + ddlFeedName.SelectedValue.ToString() + ",[FieldName],[FieldTypeID],[FieldDescription],[FieldLimit],[FieldHeight],[FieldWidth],[isRequired],[DisplayOrder] from tb_FeedFieldMaster WHERE FeedID=" + ddlFeedNameStore.SelectedValue.ToString() + " AND FieldName='" + dsFeed.Tables[0].Rows[i]["FieldName"].ToString() + "' SELECT SCOPE_IDENTITY();"));
                                    CommonComponent.ExecuteCommonData("INSERT INTO  tb_FeedFieldTypeValues(TypeID,FieldID,FieldValues,DisplayOrder)SELECT TypeID," + filedid + ",FieldValues,DisplayOrder from tb_FeedFieldTypeValues WHERE FieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + "");
                                    CommonComponent.ExecuteCommonData("INSERT INTO  tb_FeedProductBaseMapping(FeedID,FieldID,ProductField)SELECT " + ddlFeedName.SelectedValue.ToString() + " as FeedId," + filedid + ",ProductField  from tb_FeedProductBaseMapping WHERE FeedID=" + ddlFeedNameStore.SelectedValue.ToString() + " AND FieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + " ");
                                }
                                else
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please select Base feed.');document.getElementById('ContentPlaceHolder1_ddlFeedNameStore').focus();", true);
                                    return;
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_Feedmaster Where FeedId=" + ddlFeedNameStore.SelectedValue + "")))
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Please select Base feed.');document.getElementById('ContentPlaceHolder1_ddlFeedName').focus();", true);
                                    return;
                                }
                                else
                                {
                                    filedid = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO  tb_FeedFieldMaster([FeedID] ,[FieldName],[FieldTypeID],[FieldDescription],[FieldLimit],[FieldHeight],[FieldWidth],[isRequired],[DisplayOrder])SELECT " + ddlFeedName.SelectedValue.ToString() + ",[FieldName],[FieldTypeID],[FieldDescription],[FieldLimit],[FieldHeight],[FieldWidth],[isRequired],[DisplayOrder] from tb_FeedFieldMaster WHERE FeedID=" + ddlFeedNameStore.SelectedValue.ToString() + " AND FieldName='" + dsFeed.Tables[0].Rows[i]["FieldName"].ToString() + "' SELECT SCOPE_IDENTITY();"));
                                    CommonComponent.ExecuteCommonData("INSERT INTO  tb_FeedFieldTypeValues(TypeID,FieldID,FieldValues,DisplayOrder)SELECT TypeID," + filedid + ",FieldValues,DisplayOrder from tb_FeedFieldTypeValues WHERE FieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + "");

                                    string strId = "";
                                    string strFeedId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT FeedID from tb_FeedMaster WHERE StoreID=" + ddlStore.SelectedValue.ToString() + " and IsBase=1"));

                                    if (!string.IsNullOrEmpty(strFeedId))
                                    {
                                        strId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT FieldID from tb_FeedProductBaseMapping WHERE ProductField in(SELECT ProductField from tb_FeedProductBaseMapping WHERE FieldID in (SELECT BaseFieldID from tb_FeedFieldMapping WHERE RelatedFieldID=" + dsFeed.Tables[0].Rows[i]["FieldId"].ToString() + ")) and FeedID=" + strFeedId + ""));
                                        if (!string.IsNullOrEmpty(strId))
                                        {
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_FeedFieldMapping(BaseFeedID,BaseFieldID,RelatedFeedID,RelatedFieldID) VALUES (" + strFeedId + "," + strId + "," + ddlFeedName.SelectedValue.ToString() + "," + filedid + ")");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    BindData();
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text.Trim() != "")
                {
                    string StrSearch = txtSearch.Text.Trim().Replace("'", "''");
                    FeedComponent objFeedMaster = new FeedComponent();
                    string WhrCluse = "";
                    if (ddlSearch.SelectedValue == "FieldName")
                        WhrCluse = "tb_FeedFieldMaster.FieldName like '%" + StrSearch + "%'";
                    else
                        WhrCluse = "tb_FeedFieldTypeMaster.TypeName like '%" + StrSearch + "%'";
                    if (!string.IsNullOrEmpty(ddlFeedName.SelectedValue) && (Convert.ToInt32(ddlFeedName.SelectedValue) > 0))
                    {
                        if (Convert.ToInt32(ddlStore.SelectedValue) > 0)
                            gvFeedMaster.DataSource = GetFeedDetailsforSearch(WhrCluse, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(ddlFeedName.SelectedValue));
                        else
                            gvFeedMaster.DataSource = GetFeedDetailsforSearch(WhrCluse, 0, Convert.ToInt32(ddlFeedName.SelectedValue));
                    }
                    else
                    {
                        if (ddlStore.SelectedIndex != 0)
                            gvFeedMaster.DataSource = GetFeedDetailsforSearch(WhrCluse, Convert.ToInt32(ddlStore.SelectedValue), 0);
                        else
                            gvFeedMaster.DataSource = GetFeedDetailsforSearch(WhrCluse, 0, 0);
                    }
                    gvFeedMaster.DataBind();
                }
            }
            catch (Exception ex)
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Search Term is not in correct formate.', 'Message');});", true);
                return;
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            //ddlStore.SelectedIndex = 0;
            BindData();
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFeedName.Items.Clear();
            txtSearch.Text = "";
            ddlSearch.SelectedIndex = 0;
            DataSet dsFeedFieldType = new DataSet();
            FeedComponent objFeedMaster = new FeedComponent();

            dsFeedFieldType = objFeedMaster.GetFeedMasterByStore(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsFeedFieldType != null && dsFeedFieldType.Tables.Count > 0 && dsFeedFieldType.Tables[0].Rows.Count > 0)
            {
                ddlFeedName.DataSource = dsFeedFieldType.Tables[0];
                ddlFeedName.DataValueField = "FeedID";
                ddlFeedName.DataTextField = "FeedName";
                ddlFeedName.DataBind();
            }
            else
            {
                ddlFeedName.Items.Insert(0, new ListItem("- No Feed Name found -", "0"));
            }

            BindData();
            if (ddlStore.Items.Count > 0)
            {
                ArrayList textList = new ArrayList();
                ArrayList valueList = new ArrayList();


                foreach (ListItem li in ddlStore.Items)
                {
                    textList.Add(li.Text);
                }

                textList.Sort();

                foreach (object item in textList)
                {
                    string value = ddlStore.Items.FindByText(item.ToString()).Value;
                    valueList.Add(value);
                }
                ddlStoreFrom.Items.Clear();

                for (int i = 0; i < textList.Count; i++)
                {
                    ListItem objItem = new ListItem(textList[i].ToString(), valueList[i].ToString());
                    ddlStoreFrom.Items.Add(objItem);
                }
                if (ddlStoreFrom.Items.Count > 0)
                {
                    ddlStoreFrom_SelectedIndexChanged(null, null);
                }
            }
        }


        /// <summary>
        /// Gets the feed details for search.
        /// </summary>
        /// <param name="WhrCluse">String WhrCluse</param>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="FeedId">int FeedId</param>
        /// <returns>DataSet.</returns>
        public DataSet GetFeedDetailsforSearch(String WhrCluse, Int32 StoreId, Int32 FeedId)
        {
            if (FeedId != 0)
            {
                if (StoreId != 0)
                    return CommonComponent.GetCommonDataSet("Select tb_FeedFieldMaster.*,tb_FeedMaster.FeedName,tb_FeedFieldTypeMaster.TypeName from tb_FeedFieldMaster Inner Join tb_FeedMaster on tb_FeedMaster.FeedId=tb_FeedFieldMaster.FeedId Inner Join tb_FeedFieldTypeMaster on tb_FeedFieldMaster.FieldTypeID=tb_FeedFieldTypeMaster.FieldTypeID Where " + WhrCluse + " And tb_FeedFieldMaster.StoreId=" + StoreId + " And tb_FeedMaster.FeedId=" + FeedId + "");
                else
                    return CommonComponent.GetCommonDataSet("Select tb_FeedFieldMaster.*,tb_FeedMaster.FeedName,tb_FeedFieldTypeMaster.TypeName from tb_FeedFieldMaster Inner Join tb_FeedMaster on tb_FeedMaster.FeedId=tb_FeedFieldMaster.FeedId Inner Join tb_FeedFieldTypeMaster on tb_FeedFieldMaster.FieldTypeID=tb_FeedFieldTypeMaster.FieldTypeID Where " + WhrCluse + " And tb_FeedMaster.FeedId=" + FeedId + "");
            }
            else
            {
                if (StoreId != 0)
                    return CommonComponent.GetCommonDataSet("Select tb_FeedFieldMaster.*,tb_FeedMaster.FeedName,tb_FeedFieldTypeMaster.TypeName from tb_FeedFieldMaster Inner Join tb_FeedMaster on tb_FeedMaster.FeedId=tb_FeedFieldMaster.FeedId Inner Join tb_FeedFieldTypeMaster on tb_FeedFieldMaster.FieldTypeID=tb_FeedFieldTypeMaster.FieldTypeID Where " + WhrCluse + " And tb_FeedFieldMaster.StoreId=" + StoreId + "");
                else
                    return CommonComponent.GetCommonDataSet("Select tb_FeedFieldMaster.*,tb_FeedMaster.FeedName,tb_FeedFieldTypeMaster.TypeName from tb_FeedFieldMaster Inner Join tb_FeedMaster on tb_FeedMaster.FeedId=tb_FeedFieldMaster.FeedId Inner Join tb_FeedFieldTypeMaster on tb_FeedFieldMaster.FieldTypeID=tb_FeedFieldTypeMaster.FieldTypeID Where " + WhrCluse + "");
            }
        }

        /// <summary>
        /// Feed Name Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlFeedName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindData();
        }
    }
}