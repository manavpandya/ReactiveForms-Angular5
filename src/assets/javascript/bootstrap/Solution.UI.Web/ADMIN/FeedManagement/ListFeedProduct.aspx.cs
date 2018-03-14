using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class ListFeedProduct : BasePage
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
                btnIsBase.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 62px; height: 23px; border:none;cursor:pointer;");
                btnFromIsBase.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/import-from-base.png) no-repeat transparent; width: 131px; height: 23px; border:none;cursor:pointer;");

                BindStore();
                IsBaseValue();
                ddlStore.SelectedValue = "1";
                ddlStore_SelectedIndexChanged(null, null);
                BindData("");
                ddlStore.SelectedValue = "1";
            }
        }

        /// <summary>
        /// Binds all Stores into Drop down List
        /// </summary>
        public void BindStore()
        {
            DataSet dsStore = new DataSet();
            dsStore = StoreComponent.GetStoreList();
            ddlStore.Items.Clear();
            if (dsStore != null && dsStore.Tables[0].Rows.Count != 0)
            {
                for (int StoreCount = 0; StoreCount < dsStore.Tables[0].Rows.Count; StoreCount++)
                    ddlStore.Items.Add(new ListItem(dsStore.Tables[0].Rows[StoreCount]["StoreName"].ToString(), dsStore.Tables[0].Rows[StoreCount]["StoreID"].ToString()));
            }
            BindStoreMethod();
        }

        /// <summary>
        /// Binds the Store Method
        /// </summary>
        public void BindStoreMethod()
        {
            if (Session["StoreID"] != null)
            {
                int SID = Convert.ToInt32(Session["StoreID"]);
                ListItem itm = ddlStore.Items.FindByValue(SID.ToString());
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Determines whether [is base value].
        /// </summary>
        protected void IsBaseValue()
        {
            if (ddlFeedName.SelectedValue != "0")
            {
                if (Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select IsBase from tb_feedmaster Where FeedId=" + ddlFeedName.SelectedValue + "")))
                {
                    btnIsBase.Visible = true;
                    ddlbasestore.Visible = false;
                    btnFromIsBase.Visible = false;
                }
                else
                {
                    ddlbasestore.Visible = false;
                    btnIsBase.Visible = false;
                    btnFromIsBase.Visible = true;
                }
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFeedName.Items.Clear();
            DataSet dsFeedFieldType = new DataSet();
            FeedComponent objFeedMaster = new FeedComponent();
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
                BindData("");
            }
        }

        /// <summary>
        /// Binds the Product Feed data into gridview
        /// </summary>
        /// <param name="SearchVal">string SearchVal</param>
        public void BindData(string SearchVal)
        {
            FeedComponent objFeedMaster = new FeedComponent();
            DataSet dsFeedMaster = new DataSet();
            if (!string.IsNullOrEmpty(ddlFeedName.SelectedValue) && (Convert.ToInt32(ddlFeedName.SelectedValue) > 0))
            {
                dsFeedMaster = objFeedMaster.GetFeedProduct(Convert.ToInt32(ddlFeedName.SelectedValue), SearchVal.ToString());
                gvFeedMaster.DataSource = dsFeedMaster;
                gvFeedMaster.DataBind();
                if (gvFeedMaster.Rows.Count > 0)
                { }
                else
                {
                    gvFeedMaster.DataSource = dsFeedMaster;
                    gvFeedMaster.DataBind();
                }
            }
            else
            {
                gvFeedMaster.DataSource = null;
                gvFeedMaster.DataBind();
            }
        }

        /// <summary>
        /// Feed Name Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlFeedName_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsBaseValue();
            txtSearch.Text = "";
            BindData("");
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                string StrSearch = txtSearch.Text.Trim().Replace("'", "''");
                BindData(StrSearch);
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
            ddlStore.SelectedIndex = 0;
            BindData("");
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
                    string DeleteValue = e.CommandArgument.ToString();
                    //  if Is Base Product
                    string[] strArray = e.CommandArgument.ToString().Split('-');
                    string ProductId = strArray[0];
                    string FeedId = strArray[2];
                    string FieldId = (string)CommonComponent.GetScalarCommonData("Select (cast(FieldId as varchar)+',') from tb_FeedFieldMaster Where FeedId=" + FeedId.ToString() + " for XML path('')");
                    if (Convert.ToBoolean(strArray[1]))
                    {
                        if (!string.IsNullOrEmpty(FieldId))
                        {
                            FieldId = FieldId.Substring(0, (FieldId.Length - 1));
                            CommonComponent.ExecuteCommonData("Delete From tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + " Where FieldId In (" + FieldId.ToString() + ")");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(FieldId))
                        {
                            FieldId = FieldId.Substring(0, (FieldId.Length - 1));
                            CommonComponent.ExecuteCommonData("Delete From tb_FeedFieldValues_" + ddlFeedName.SelectedValue.ToString() + " Where ProductId = " + ProductId.ToString() + " And FieldId In (" + FieldId.ToString() + ")");
                        }
                    }
                    BindData("");
                }
            }
            catch { }
        }

        /// <summary>
        /// Feed Master Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvFeedMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblproductid = (Label)e.Row.FindControl("lblProduct");
                Literal ltfedd = (Literal)e.Row.FindControl("ltfedd");

                ((HyperLink)e.Row.FindControl("hprlnkEdit")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("btnDelete")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

                DataSet dsnew = new DataSet();
                dsnew = CommonComponent.GetCommonDataSet("Select FeedID,FeedName from tb_feedmaster Where StoreID=" + ddlStore.SelectedValue.ToString() + "");
                string strtable = "";

                if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                {
                    strtable = "<table cellspacing=\"0\" cellpadding=\"2\" border=\"0\"><tr><td><table cellspacing=\"2\" cellpadding=\"2\" border=\"0\"><tr>";

                    for (int i = 0; i < dsnew.Tables[0].Rows.Count; i++)
                    {

                        if ((i + 1) % 10 == 0)
                        {
                            strtable += "</tr></table></td></tr><tr><td><table cellspacing=\"2\" cellpadding=\"2\" border=\"0\"><tr>";
                        }
                        DataSet dsRecord = new DataSet();
                        dsRecord = CommonComponent.GetCommonDataSet("Select Top 1 FieldID from tb_FeedFieldValues_" + dsnew.Tables[0].Rows[i]["FeedID"].ToString() + " Where ProductID=" + lblproductid.Text.ToString() + "");

                        if (dsRecord != null && dsRecord.Tables.Count > 0 && dsRecord.Tables[0].Rows.Count > 0)
                        {
                            Int32 TotalCount = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(DISTINCT FieldID) from tb_FeedFieldValues_" + dsnew.Tables[0].Rows[i]["FeedID"].ToString() + " Where FieldID in (Select FieldID FROM tb_FeedFieldMaster  Where FeedID=" + dsnew.Tables[0].Rows[i]["FeedID"].ToString() + " AND isnull(isRequired,0) = 1) AND isnull(FieldValue,'') <> '' AND ProductID=" + lblproductid.Text.ToString() + ""));
                            Int32 TotalCountmain = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(DISTINCT FieldID) FROM tb_FeedFieldMaster  Where FeedID=" + dsnew.Tables[0].Rows[i]["FeedID"].ToString() + " AND isnull(isRequired,0) = 1 AND isnull(DefautValue,'') = ''"));
                            if (TotalCountmain > TotalCount)
                            {
                                strtable += "<td style=' background-color :#002AFF;color:#fff;font-size:10px;border:solid 1px #002AFF;' title='" + Server.HtmlEncode(dsnew.Tables[0].Rows[i]["FeedName"].ToString()) + "'>" + dsnew.Tables[0].Rows[i]["FeedName"].ToString() + "</td>";
                            }
                            else
                            {
                                strtable += "<td style=' background-color :#568403;color:#fff;font-size:10px;border:solid 1px #568403;' title='" + Server.HtmlEncode(dsnew.Tables[0].Rows[i]["FeedName"].ToString()) + "'>" + dsnew.Tables[0].Rows[i]["FeedName"].ToString() + "</td>";
                            }
                        }
                        else
                        {
                            strtable += "<td style=' background-color :#c10000;color:#fff;font-size:10px;border:solid 1px #c10000;' title='" + Server.HtmlEncode(dsnew.Tables[0].Rows[i]["FeedName"].ToString()) + "'>" + dsnew.Tables[0].Rows[i]["FeedName"].ToString() + "</td>";
                        }
                    }
                    strtable += "</tr></table></td></tr></table>";
                }
                ltfedd.Text = strtable;
            }

        }

        /// <summary>
        ///  Feed Master Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvFeedMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFeedMaster.PageIndex = e.NewPageIndex;
            BindData("");
        }

        /// <summary>
        ///  Is Base Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnIsBase_Click(object sender, EventArgs e)
        {
            string str = "";

            FeedComponent objMng = new FeedComponent();
            DataSet dsFeed = new DataSet();
            dsFeed = objMng.GetFielDname(Convert.ToInt32(ddlFeedName.SelectedValue.ToString()));
            if (dsFeed != null && dsFeed.Tables.Count > 0 && dsFeed.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFeed.Tables[0].Rows.Count; i++)
                {
                    str = Convert.ToString(CommonComponent.GetScalarCommonData("EXEC [usp_ImportFeedData] @FEEDID=" + ddlFeedName.SelectedValue.ToString() + ",@STOREID = " + ddlStore.SelectedValue.ToString() + ",@BASEID = 0,@itemsnew='" + dsFeed.Tables[0].Rows[i]["ProductField"].ToString() + "'"));
                }
                if (str == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Some Record(s) are Missing, Please try again.');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Record(s) Inserted Successfully.');", true);
                }
                BindData("");
            }
        }

        /// <summary>
        ///  From Is Base Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFromIsBase_Click(object sender, EventArgs e)
        {
            string str = "";
            FeedComponent objMng = new FeedComponent();
            DataSet dsFeed = new DataSet();
            Int32 ii = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select FeedID from tb_feedmaster Where StoreID=" + ddlStore.SelectedValue.ToString() + " and IsBase=1"));
            dsFeed = objMng.GetFielDname(ii);

            if (dsFeed != null && dsFeed.Tables.Count > 0 && dsFeed.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsFeed.Tables[0].Rows.Count; i++)
                {
                    str = Convert.ToString(CommonComponent.GetScalarCommonData("EXEC [usp_ImportFeedData] @FEEDID=" + ii.ToString() + ",@STOREID = " + ddlStore.SelectedValue.ToString() + ",@BASEID = " + ddlFeedName.SelectedValue.ToString() + ",@itemsnew='" + dsFeed.Tables[0].Rows[i]["ProductField"].ToString() + "'"));
                }
                if (str == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('Some Record(s) are Missing, Please try again.');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg03", "alert('Record(s) Inserted Successfully.');", true);
                }
                BindData("");
            }
        }
    }
}