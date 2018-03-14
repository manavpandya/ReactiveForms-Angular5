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
    public partial class BaseRelatedMapping : BasePage
    {
        public static DataSet DsMapppedFields = new DataSet();
        DataSet DsCommon = new DataSet();
        DataSet DsBaseFeed = new DataSet();
        DataSet DsRelatedFeed = new DataSet();
        String strSql = "";
        Int32 FeedID = 0;

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
                BindStore();
                ddlStore_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// Binds the stores into drop down.
        /// </summary>
        public void BindStore()
        {
            DataSet ds = new DataSet();
            ds = StoreComponent.GetStoreList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    ddlStore.Items.Add(new ListItem(ds.Tables[0].Rows[i]["StoreName"].ToString(), ds.Tables[0].Rows[i]["StoreID"].ToString()));
            }
            BindStoreMethod();
        }

        /// <summary>
        /// Binds the store method.
        /// </summary>
        public void BindStoreMethod()
        {
            if (Session["StoreID"] != null)
            {
                int SID = Convert.ToInt32(Session["StoreID"]);
                ListItem itm = ddlStore.Items.FindByValue(SID.ToString());
                ddlStore.SelectedIndex = ddlStore.Items.IndexOf(itm);
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlRelatedFeed.Items.Count > 0 && Convert.ToInt32(ddlRelatedFeed.SelectedValue.ToString()) > 0)
            {
                if (lbBaseSchema.SelectedValue.ToString().Trim() != "" && lbRelatedSchema.SelectedValue.ToString().Trim() != "")
                {
                    strSql = @"Insert into tb_FeedFieldMapping (BaseFeedID, BaseFieldID, RelatedFeedID, RelatedFieldID) values 
                             (" + Convert.ToInt32(ViewState["FeedID"]) + "," + lbBaseSchema.SelectedValue + "," + ddlRelatedFeed.SelectedValue + ",'" + lbRelatedSchema.SelectedValue + "')";

                    CommonComponent.ExecuteCommonData(strSql);
                    ddlRelatedFeed_SelectedIndexChanged(null, null);
                    Page.ClientScript.RegisterStartupScript(typeof(string), "Save", "<script>alert('Fields Mapped Successfully!');</script>");
                }
                else
                    Page.ClientScript.RegisterStartupScript(typeof(string), "Save", "<script>alert('Select [Product Schema] and [Base Schema] for Field(s) Mapping.');</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(typeof(string), "Save", "<script>alert('Select Related Feed.');</script>");
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btncancel_Click(object sender, ImageClickEventArgs e)
        {

        }

        /// <summary>
        /// Mapping Fields Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdMappedFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                Int32 MappingID = Convert.ToInt32(e.CommandArgument);
                strSql = "Delete From tb_FeedFieldMapping where MappingID=" + MappingID;
                CommonComponent.ExecuteCommonData(strSql);
                ddlRelatedFeed_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// Mapping Fields Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdMappedFields_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("btnRemove")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
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

            BindData();
            BindMappedFields();
        }

        /// <summary>
        /// Related Feed Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlRelatedFeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            #region Related Feed

            if (Convert.ToInt32(ViewState["FeedID"]) > 0)
            {
                strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                    Where f.FeedID=ffm.FeedID and f.FeedID=" + ddlRelatedFeed.SelectedValue + " and f.StoreID="
                        + ddlStore.SelectedValue + " and ffm.FieldID not in (Select RelatedFieldID from tb_FeedFieldMapping where BaseFeedID="
                        + Convert.ToInt32(ViewState["FeedID"]) + " and RelatedFeedID=" + ddlRelatedFeed.SelectedValue + ") order by ffm.DisplayOrder";
                DsCommon = CommonComponent.GetCommonDataSet(strSql);
                lbRelatedSchema.DataSource = DsCommon.Tables[0];
                lbRelatedSchema.DataTextField = "FieldName";
                lbRelatedSchema.DataValueField = "FieldID";
                lbRelatedSchema.DataBind();
            }
            else
            {
                lbBaseSchema.Items.Clear();
            }

            if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
            { }
            else lblMsg.Text += "No Any Fields Added or Remaining to Map for Related Feed!<br/>";

            #endregion Related Feed

            #region Base Feed

            strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                        Where f.FeedID=ffm.FeedID and f.IsBase=1 and f.FeedID=" + Convert.ToInt32(ViewState["FeedID"]) + " and f.StoreID=" + ddlStore.SelectedValue
                    + " and ffm.FieldID not in (Select BaseFieldID from tb_FeedFieldMapping where BaseFeedID=" + Convert.ToInt32(ViewState["FeedID"]) + " and RelatedFeedID="
                    + ddlRelatedFeed.SelectedValue + ") order by ffm.DisplayOrder";


            DsCommon = CommonComponent.GetCommonDataSet(strSql);
            lbBaseSchema.DataSource = DsCommon.Tables[0];
            lbBaseSchema.DataTextField = "FieldName";
            lbBaseSchema.DataValueField = "FieldID";
            lbBaseSchema.DataBind();

            if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
            { }
            else lblMsg.Text += "No Any Fields Added or Remaining to Map for Base Feed!<br/>";

            #endregion Base Feed

            BindMappedFields();
        }

        /// <summary>
        /// Binds the Feed data into Gridview.
        /// </summary>
        public void BindData()
        {
            lblMsg.Text = "";

            strSql = "Select ISNULL(FeedID,0) as FeedID, FeedName from tb_FeedMaster Where IsBase=1 and StoreID=" + ddlStore.SelectedValue;
            DsBaseFeed = CommonComponent.GetCommonDataSet(strSql);

            if (DsBaseFeed != null && DsBaseFeed.Tables.Count > 0 && DsBaseFeed.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(Convert.ToString(DsBaseFeed.Tables[0].Rows[0]["FeedID"]), out FeedID);
            }

            #region Bind Related Feed
            strSql = "Select ISNULL(FeedID,0) as FeedID, FeedName from tb_FeedMaster Where IsBase<>1 and StoreID=" + ddlStore.SelectedValue + " order by FeedName";
            DsRelatedFeed = CommonComponent.GetCommonDataSet(strSql);
            ddlRelatedFeed.DataSource = DsRelatedFeed.Tables[0];
            ddlRelatedFeed.DataTextField = "FeedName";
            ddlRelatedFeed.DataValueField = "FeedID";
            ddlRelatedFeed.DataBind();
            if (DsRelatedFeed != null && DsRelatedFeed.Tables.Count > 0 && DsRelatedFeed.Tables[0].Rows.Count > 0)
            {
                if (FeedID > 0)
                {
                    strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                    Where f.FeedID=ffm.FeedID and f.FeedID=" + ddlRelatedFeed.SelectedValue + " and f.StoreID="
                            + ddlStore.SelectedValue + " and ffm.FieldID not in (Select RelatedFieldID from tb_FeedFieldMapping where BaseFeedID="
                            + FeedID + " and RelatedFeedID=" + ddlRelatedFeed.SelectedValue + ") order by ffm.DisplayOrder";

                    DsCommon = CommonComponent.GetCommonDataSet(strSql);
                    lbRelatedSchema.DataSource = DsCommon.Tables[0];
                    lbRelatedSchema.DataTextField = "FieldName";
                    lbRelatedSchema.DataValueField = "FieldID";
                    lbRelatedSchema.DataBind();
                }
                else
                {
                    lbRelatedSchema.Items.Clear();
                }


                if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
                { }
                else lblMsg.Text += "No Any Fields Added or Remaining to Map for Related Feed!<br/>";
            }
            else
            {
                lblMsg.Text += "No any Related Feed Added for this Store!<br/>";
            }
            #endregion Bind Related Feed

            #region Bind Base Feed

            if (FeedID > 0)
            {
                Int32.TryParse(Convert.ToString(DsBaseFeed.Tables[0].Rows[0]["FeedID"]), out FeedID);
                lblBaseFeed.Text = Convert.ToString(DsBaseFeed.Tables[0].Rows[0]["FeedName"]);

                ViewState["FeedID"] = FeedID;
                if (DsRelatedFeed != null && DsRelatedFeed.Tables.Count > 0 && DsRelatedFeed.Tables[0].Rows.Count > 0)
                {
                    strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                        Where f.FeedID=ffm.FeedID and f.IsBase=1 and f.FeedID=" + FeedID + " and f.StoreID=" + ddlStore.SelectedValue
                            + " and ffm.FieldID not in (Select BaseFieldID from tb_FeedFieldMapping where BaseFeedID=" + FeedID + " and RelatedFeedID="
                            + ddlRelatedFeed.SelectedValue + ") order by ffm.DisplayOrder";
                }
                else
                {
                    strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                        Where f.FeedID=ffm.FeedID and f.IsBase=1 and f.FeedID=" + FeedID + " and f.StoreID=" + ddlStore.SelectedValue
                            + " order by ffm.DisplayOrder";
                }

                DsCommon = CommonComponent.GetCommonDataSet(strSql);
                lbBaseSchema.DataSource = DsCommon.Tables[0];
                lbBaseSchema.DataTextField = "FieldName";
                lbBaseSchema.DataValueField = "FieldID";
                lbBaseSchema.DataBind();

                if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
                { }
                else lblMsg.Text += "No Any Fields Added or Remaining to Map for Base Feed!<br/>";
            }
            else
            {
                lbBaseSchema.Items.Clear();
                lblMsg.Text += "No any Base Feed Added/Defined for this Store!";
                lblBaseFeed.Text = "N/A";
                return;
            }
            #endregion Bind Base Feed

        }

        /// <summary>
        /// Binds the mapped fields into Gridview.
        /// </summary>
        public void BindMappedFields()
        {
            if (ViewState["FeedID"] != null && Convert.ToString(ViewState["FeedID"]) != "" && ddlRelatedFeed.SelectedValue != null && ddlRelatedFeed.SelectedValue != "")
            {
                strSql = @"Select ffmp.MappingID,ffm1.FieldName as BaseFieldName, ffm2.FieldName as RelatedFieldName 
                From tb_FeedFieldMapping ffmp, tb_FeedFieldMaster ffm1, tb_FeedFieldMaster ffm2
                Where ffmp.BaseFieldID=ffm1.FieldID and ffmp.RelatedFieldID=ffm2.FieldID
                and ffmp.BaseFeedID=ffm1.FeedID and ffmp.RelatedFeedID=ffm2.FeedID
                and ffmp.BaseFeedID=" + Convert.ToInt32(ViewState["FeedID"]) + " and ffmp.RelatedFeedID=" + ddlRelatedFeed.SelectedValue;
                DsMapppedFields = CommonComponent.GetCommonDataSet(strSql);
                grdMappedFields.DataSource = DsMapppedFields.Tables[0];
                grdMappedFields.DataBind();

            }
            else
            {
                grdMappedFields.DataSource = null;
                grdMappedFields.DataBind();
            }
        }
    }
}