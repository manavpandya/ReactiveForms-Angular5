using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using GridGenerator;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class TaxRatesByState : BasePage
    {
        #region Declaration
        TaxClassComponent objTaxClass = new TaxClassComponent();
        SortedList StateTaxTable = new SortedList();
        DataSet DsState = new DataSet();
        DataSet DsTaxClss = new DataSet();
        static int storeID = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["StoreID"].ToString());
        public ArrayList ArrlstTaxClass = new ArrayList();
        public DataTable DtTableStateTax = new DataTable("StateTax");
        public struct TaxClassStruct
        {
            public int TaxClassID;
            public string TaxName;
        }

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                Bindstore();
            }
            if (txtSearch.Text == "")
                BindData(storeID);
            else
                Search("Name", txtSearch.Text.Trim());
            RenderAll();
        }

        /// <summary>
        /// Bind Store 
        /// </summary>
        private void Bindstore()
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
                ddlStore.Items.Insert(0, new ListItem("Select Store", "-1"));
            }
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
        }

        /// <summary>
        /// Fill Data For Grid view
        /// </summary>
        protected void Filldata()
        {
            DtTableStateTax.Columns.Clear();
            DtTableStateTax.Rows.Clear();
            DtTableStateTax.Columns.Add("StateID");
            DtTableStateTax.Columns.Add("State Name");
            DtTableStateTax.Columns.Add("Abbreviation");
            if (ArrlstTaxClass.Count > 0)
            {
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                    DtTableStateTax.Columns.Add(TaxStructure.TaxName);
                }
            }
            DtTableStateTax.AcceptChanges();
            DataView dv = TaxClassComponent.DataSourceState;
            foreach (DataRow gr in dv.ToTable().Rows)
            {
                DataRow dr = DtTableStateTax.NewRow();
                dr["StateID"] = gr["StateID"].ToString();
                dr["State Name"] = gr["Name"].ToString();
                dr["Abbreviation"] = gr["Abbreviation"].ToString();
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                    decimal rate = GetRate(Convert.ToInt32(gr["StateID"].ToString()), TaxStructure.TaxClassID);
                    rate = Math.Round(rate, 2);
                    dr[TaxStructure.TaxName] = rate;
                }
                DtTableStateTax.Rows.Add(dr);
            }
            DtTableStateTax.AcceptChanges();
            ViewState["DataTable"] = DtTableStateTax;
        }

        /// <summary>
        /// Get Rate For Tax Class
        /// </summary>
        /// <param name="StateID">int StateID</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <returns>Returns the Rate of Tax</returns>
        private decimal GetRate(int StateID, int TaxClassID)
        {
            for (int i = 0; i < StateTaxTable.Count; i++)
            {
                TaxClassComponent objTaxClass = (TaxClassComponent)StateTaxTable.GetByIndex(i);
                if (objTaxClass.StateID == StateID && objTaxClass.TaxClassIDForState == TaxClassID)
                {
                    return objTaxClass.TaxRate;
                }
            }
            return System.Decimal.Zero;
        }

        /// <summary>
        /// Bind Data in Data table
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void BindData(int StoreID)
        {
            if (StoreID < 1)
                return;

            DsTaxClss = TaxClassComponent.GetTaxClassByStoreID(StoreID);
            StateTaxTable = objTaxClass.GetArrayList();
            DsState = objTaxClass.GetState(1);
            if (DsState != null && DsState.Tables != null)
                TaxClassComponent.DataSourceState = DsState.Tables[0].DefaultView;

            if (DsTaxClss != null && DsTaxClss.Tables[0] != null)
            {
                ArrlstTaxClass.Clear();
                for (int i = 0; i < DsTaxClss.Tables[0].Rows.Count; i++)
                {
                    TaxClassStruct TaxStructure = new TaxClassStruct();
                    TaxStructure.TaxClassID = Convert.ToInt32(DsTaxClss.Tables[0].Rows[i]["TaxClassID"].ToString());
                    TaxStructure.TaxName = DsTaxClss.Tables[0].Rows[i]["TaxName"].ToString();
                    ArrlstTaxClass.Add(TaxStructure);
                }
            }
        }

        /// <summary>
        /// Render All function for Grid View Binding
        /// </summary>
        private void RenderAll()
        {
            Filldata();
            DtTableStateTax.AcceptChanges();
            GenerateGrid();
        }

        /// <summary>
        /// Generate Grid Dynamically
        /// </summary>
        private void GenerateGrid()
        {
            gvStateTaxRate.Columns.Clear();

            TemplateField tfStateID = new TemplateField();
            tfStateID.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                         "State ID", "");

            tfStateID.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                          "StateID",
                                                         "StateID");
            gvStateTaxRate.Columns.Add(tfStateID);

            tfStateID.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(10);

            tfStateID.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            tfStateID.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

            TemplateField tfStateName = new TemplateField();
            tfStateName.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                          "State Name", "");

            tfStateName.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                           "State Name",
                                                          "State Name");


            gvStateTaxRate.Columns.Add(tfStateName);
            tfStateName.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            tfStateName.ItemStyle.HorizontalAlign = HorizontalAlign.Left;

            TemplateField tfAbbreviation = new TemplateField();
            tfAbbreviation.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                         "Abbreviation", "");

            tfAbbreviation.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                          "Abbreviation",
                                                         "Abbreviation");

            gvStateTaxRate.Columns.Add(tfAbbreviation);
            tfAbbreviation.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            tfAbbreviation.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;



            if (ArrlstTaxClass.Count > 0)
            {
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];

                    TemplateField tftxrate = new TemplateField();
                    tftxrate.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                                 TaxStructure.TaxName.ToString(), "");


                    tftxrate.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                                   TaxStructure.TaxName.ToString(),
                                                                "");
                    tftxrate.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem,
                                                                 TaxStructure.TaxName.ToString(),
                                                                  "");
                    tftxrate.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(12);
                    gvStateTaxRate.Columns.Add(tftxrate);

                    tftxrate.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    tftxrate.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                }
            }
            TemplateField BtnTmpField = new TemplateField();
            BtnTmpField.ItemTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Edit", "CommandEdit");
            BtnTmpField.HeaderTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Edit", "CommandEdit");
            BtnTmpField.EditItemTemplate =
                new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Edit", "CommandEdit");

            gvStateTaxRate.Columns.Add(BtnTmpField);

            gvStateTaxRate.DataSource = (DataTable)ViewState["DataTable"];
            gvStateTaxRate.DataBind();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                Search("Name", txtSearch.Text.Trim());
                RenderAll();
            }
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            BindData(storeID);
            RenderAll();
        }

        /// <summary>
        /// State Tax Rate Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvStateTaxRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.Image btn2 = (Image)e.Row.FindControl("update_button");
                    btn2.ImageUrl = "../images/save_icon.jpg";
                }
            }
            catch { }
        }

        /// <summary>
        /// State Tax Rate Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void gvStateTaxRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvStateTaxRate.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.DataItemIndex.ToString());
            DataRow dr = ((DataTable)ViewState["DataTable"]).Rows[id];
            dr.BeginEdit();
            Label txtID = (Label)row.FindControl("StateID");

            for (int i = 0; i < ArrlstTaxClass.Count; i++)
            {
                TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                TextBox rate = (TextBox)row.FindControl(TaxStructure.TaxName.ToString());
                if (rate.Text.ToString().Trim().Length == 0)
                    rate.Text = "0";
                objTaxClass.TaxClassIDForState = TaxStructure.TaxClassID;
                objTaxClass.StateID = Convert.ToInt32(txtID.Text);
                objTaxClass.TaxRate = Convert.ToDecimal(rate.Text.ToString());
                dr[TaxStructure.TaxName] = Convert.ToDecimal(objTaxClass.TaxRate.ToString());
                objTaxClass.EditStateTax(2);
            }
            dr.EndEdit();
            dr.AcceptChanges();
            gvStateTaxRate.EditIndex = -1;
            gvStateTaxRate.DataSource = (DataTable)ViewState["DataTable"];
            gvStateTaxRate.DataBind();
        }

        /// <summary>
        /// State Tax Rate Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void gvStateTaxRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStateTaxRate.EditIndex = -1;
            gvStateTaxRate.DataBind();
        }

        /// <summary>
        /// State Tax Rate Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvStateTaxRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStateTaxRate.PageIndex = e.NewPageIndex;
            BindData(storeID);
            GenerateGrid();
        }

        /// <summary>
        /// State Tax Rate Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void gvStateTaxRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStateTaxRate.EditIndex = e.NewEditIndex;
            gvStateTaxRate.DataBind();
        }

        /// <summary>
        /// State Tax Rate Gridview Row Deleting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewDeleteEventArgs e</param>
        protected void gvStateTaxRate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvStateTaxRate.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.DataItemIndex.ToString());

            DataRow dr = ((DataTable)ViewState["DataTable"]).Rows[id];
            Label txtID = (Label)row.FindControl("StateID");
            for (int i = 0; i < ArrlstTaxClass.Count; i++)
            {
                TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                if (objTaxClass.DeleteStateTax(Convert.ToInt32(txtID.Text.ToString()), 3) != -1)
                {
                    dr[TaxStructure.TaxName] = "0";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Tax rates have been reset successfully.', 'Message','');});", true);
                }
            }
            ((DataTable)ViewState["DataTable"]).AcceptChanges();
            gvStateTaxRate.EditIndex = -1;
            gvStateTaxRate.DataSource = (DataTable)ViewState["DataTable"];
            gvStateTaxRate.DataBind();
        }

        /// <summary>
        /// Search Function
        /// </summary>
        /// <param name="SearchField">string SearchField</param>
        /// <param name="SearchValue">string SearchValue</param>
        protected void Search(string SearchField, string SearchValue)
        {
            try
            {
                DataView DtviewSearchState = new DataView();
                BindData(storeID);
                DtviewSearchState = TaxClassComponent.SearchForState(SearchField, SearchValue);
                TaxClassComponent.DataSourceState = DtviewSearchState;
                if (DtviewSearchState == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Records Found..', 'Message','');});", true);
                }
            }
            catch
            {
                RenderAll();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            storeID = Convert.ToInt32(ddlStore.SelectedValue);
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            if (txtSearch.Text == "")
                BindData(storeID);
            else
                Search("Name", txtSearch.Text.Trim());
            gvStateTaxRate.PageIndex = 0;
            RenderAll();
        }
    }
}