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
    public partial class ProductBaseMapping : BasePage
    {
        public static DataSet DsMapppedFields = new DataSet();
        DataSet DsCommon = new DataSet();
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
        /// Binds All Stores into Drop down
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
        /// Binds the Store Method
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
        /// Binds the Product Base Mapping data into  Gridview
        /// </summary>
        public void BindData()
        {
            lblMsg.Text = "";
            strSql = "Select ISNULL(FeedID,0) as FeedID, FeedName from tb_FeedMaster Where IsBase=1 and StoreID=" + ddlStore.SelectedValue;
            DsCommon = CommonComponent.GetCommonDataSet(strSql);

            if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(Convert.ToString(DsCommon.Tables[0].Rows[0]["FeedID"]), out FeedID);
            }

            if (FeedID > 0)
            {
                ViewState["FeedID"] = FeedID;
                lblBaseFeed.Text = Convert.ToString(DsCommon.Tables[0].Rows[0]["FeedName"]);

                strSql = @"Select ffm.* From tb_FeedMaster f, tb_FeedFieldMaster ffm 
                Where f.FeedID=ffm.FeedID and f.IsBase=1 and f.FeedID=" + FeedID + " and f.StoreID=" + ddlStore.SelectedValue
                    + " and ffm.FieldID not in (Select FieldID from tb_FeedProductBaseMapping where FeedID=" + FeedID + ")"
                    + " order by ffm.DisplayOrder";

                DsCommon = CommonComponent.GetCommonDataSet(strSql);
                lbBaseSchema.DataSource = DsCommon.Tables[0];
                lbBaseSchema.DataTextField = "FieldName";
                lbBaseSchema.DataValueField = "FieldID";
                lbBaseSchema.DataBind();

                if (DsCommon != null && DsCommon.Tables.Count > 0 && DsCommon.Tables[0].Rows.Count > 0)
                { }
                else lblMsg.Text = "No Any Fields Added or Remaining to Map for Base Feed!";
            }
            else
            {
                ViewState["FeedID"] = null;
                lbBaseSchema.Items.Clear();
                lblMsg.Text = "No any Base Feed Added/Defined for this Store!";
                lblBaseFeed.Text = "N/A";
                return;
            }

            if (FeedID > 0)
            {
                strSql = @"Select Column_Name,Data_Type From information_schema.columns 
                    Where table_name = 'tb_product'
                    and Column_Name not in (Select ProductField from tb_FeedProductBaseMapping where FeedID="
                            + FeedID + ") Order by ordinal_position";
            }
            else
            {
                strSql = @"Select Column_Name,Data_Type From information_schema.columns 
                    Where table_name = 'tb_product' Order by ordinal_position";
            }
            DsCommon = CommonComponent.GetCommonDataSet(strSql);


            try
            {
                if (DsCommon.Tables[0].Select("ProductField='Product_URL'").Length > 0)
                { }
                else
                {
                    DataRow dr = DsCommon.Tables[0].NewRow();
                    dr["Column_Name"] = "Product_URL";
                    dr["Data_Type"] = "nvarchar";
                    DsCommon.Tables[0].Rows.Add(dr);
                }
            }
            catch
            {
                DataRow dr = DsCommon.Tables[0].NewRow();
                dr["Column_Name"] = "Product_URL";
                dr["Data_Type"] = "nvarchar";
                DsCommon.Tables[0].Rows.Add(dr);
            }

            lbProductSchema.DataSource = DsCommon.Tables[0];
            lbProductSchema.DataTextField = "Column_Name";
            lbProductSchema.DataValueField = "Column_Name";
            lbProductSchema.DataBind();
        }

        /// <summary>
        /// Binds the Mapped Fields
        /// </summary>
        public void BindMappedFields()
        {
            strSql = @"Select fpbm.*,ffm.FieldName from tb_FeedProductBaseMapping fpbm, tb_FeedFieldMaster ffm 
                    where ffm.FieldID=fpbm.FieldID and fpbm.FeedID=" + Convert.ToInt32(ViewState["FeedID"]) + " order by ffm.DisplayOrder";
            DsMapppedFields = CommonComponent.GetCommonDataSet(strSql);
            grdMappedFields.DataSource = DsMapppedFields.Tables[0];
            grdMappedFields.DataBind();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (lbBaseSchema.SelectedValue.ToString().Trim() != "" && lbProductSchema.SelectedValue.ToString().Trim() != "")
            {
                strSql = @"Insert into tb_FeedProductBaseMapping (FeedID, FieldID, ProductField) values 
            (" + Convert.ToInt32(ViewState["FeedID"]) + "," + lbBaseSchema.SelectedValue + ",'" + lbProductSchema.SelectedValue + "')";
                CommonComponent.ExecuteCommonData(strSql);
                BindData();
                BindMappedFields();
                Page.ClientScript.RegisterStartupScript(typeof(string), "Save", "<script>alert('Fields Mapped Successfully!');</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(typeof(string), "Save", "<script>alert('Select [Product Schema] and [Base Schema] for Field(s) Mapping.');</script>");
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btncancel_Click(object sender, ImageClickEventArgs e)
        {
            BindData();
            BindMappedFields();
        }

        /// <summary>
        /// Sorting Gridview Column by ACS or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Mapped Fields Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdMappedFields_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                Int32 ProductMappingID = Convert.ToInt32(e.CommandArgument);
                strSql = "Delete From tb_FeedProductBaseMapping where ProductMappingID=" + ProductMappingID;
                CommonComponent.ExecuteCommonData(strSql);
                BindData();
                BindMappedFields();
            }
        }

        /// <summary>
        /// Mapped Fields Gridview Row Data Bound Event
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
    }
}