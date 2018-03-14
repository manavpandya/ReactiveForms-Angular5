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
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class TaxRatesByZipCode : BasePage
    {
        #region Declaration
        TaxClassComponent objTaxClass = new TaxClassComponent();
        SortedList ZipCodeTaxTable = new SortedList();
        DataSet DsTaxClss = new DataSet();
        DataSet DsZipTax = new DataSet();
        public DataTable DtTableStateTax = new DataTable("StateTax");
        public ArrayList ArrlstTaxClass = new ArrayList();
        public static bool EmptyFlag = true;
        static string SearchText = "";
        static int StoreID = 1;
        static DataView DsGlobal = null;

        private string StrFileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                {
                    return "";
                }
                else
                {
                    return (ViewState["FileName"].ToString());
                }
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }
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
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                ibtnsearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/search.gif";
                ibtnShowall.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/Images/showall.png";
                btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                SearchText = "";
                txtSearch.Text = "";
                StoreID = 1;
                Bindstore();
            }
           // BindData(StoreID);
            if (SearchText != "")
                Search("ZipCode", txtSearch.Text);

            //RenderAll();
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
        /// Get Tax Rate By TaxClassID
        /// </summary>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <returns>Returns the Tax Rate by Tax ClassID</returns>
        private decimal GetRate(string ZipCode, int TaxClassID)
        {
            ZipCodeTaxTable = objTaxClass.GetArrayListForZipCode();
            for (int i = 0; i < ZipCodeTaxTable.Count; i++)
            {
                objTaxClass = (TaxClassComponent)ZipCodeTaxTable.GetByIndex(i);
                if (objTaxClass.ZipCode == ZipCode && objTaxClass.TaxClassIDForState == TaxClassID)
                {
                    return objTaxClass.TaxRate;
                }
            }
            return System.Decimal.Zero;
        }

        /// <summary>
        /// Bind Data in Data Source For Grid view according to Store Selection
        /// </summary>
        /// <param name="_StoreID">int _StoreID</param>
        public void BindData(int _StoreID)
        {
            lblMsg.Text = "";
            if (_StoreID < 1)
                return;
            DsTaxClss = TaxClassComponent.GetTaxClassByStoreID(_StoreID);
            ZipCodeTaxTable = objTaxClass.GetArrayListForZipCode();

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
            DsZipTax = objTaxClass.GetZipCode(2);
            if (DsZipTax != null && DsZipTax.Tables[0].Rows.Count > 0)
            {
                EmptyFlag = false;
                TaxClassComponent.DataSourceZipCode = DsZipTax.Tables[0].DefaultView;
                DsGlobal = TaxClassComponent.DataSourceZipCode;
            }
        }

        /// <summary>
        /// Bind Tax Class and Rate in Data Table according to Zip Code
        /// </summary>
        protected void BindZipTax()
        {
            DtTableStateTax.Columns.Clear();
            DtTableStateTax.Rows.Clear();

            DtTableStateTax.Columns.Add("ZipCode");
            DtTableStateTax.Columns.Add("ZipTaxID");
            if (ArrlstTaxClass.Count > 0)
            {
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                    DtTableStateTax.Columns.Add(TaxStructure.TaxName);
                }
            }
            DtTableStateTax.AcceptChanges();
            if (DsGlobal == null)
            {
                DsGlobal = TaxClassComponent.DataSourceZipCode;
            }
            if (DsGlobal != null && DsGlobal.Table != null && DsGlobal.ToTable().Rows.Count > 0)
            {
                foreach (DataRow gr in DsGlobal.ToTable().Rows)
                {
                    DataRow dr = DtTableStateTax.NewRow();
                    dr["ZipTaxID"] = gr["ID"].ToString();
                    dr["ZipCode"] = gr["ZipCode"].ToString();
                    for (int i = 0; i < ArrlstTaxClass.Count; i++)
                    {
                        TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                        decimal rate = GetRate((gr["ZipCode"].ToString()), TaxStructure.TaxClassID);
                        rate = Math.Round(rate, 2);
                        dr[TaxStructure.TaxName] = rate;
                    }
                    DtTableStateTax.Rows.Add(dr);
                }
            }
            else
            {
                EmptyFlag = true;
                DataRow dr = DtTableStateTax.NewRow();
                dr["ZipCode"] = "No record Found";
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    dr[1] = "";
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                    dr[TaxStructure.TaxName] = "No record Found";
                }
                DtTableStateTax.Rows.Add(dr);
            }
            DtTableStateTax.AcceptChanges();
            ViewState["ZipDataTable"] = DtTableStateTax;
        }

        /// <summary>
        /// Method For Generating Grid
        /// </summary>
        private void GenerateGrid()
        {
            gvZipCodeTaxRate.Columns.Clear();
            if (!EmptyFlag)
            {
                TemplateField BtnTmpField = new TemplateField();
                BtnTmpField.ItemTemplate =
                    new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Edit", "CommandEdit");
                BtnTmpField.HeaderTemplate =
                    new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Edit", "CommandEdit");
                BtnTmpField.EditItemTemplate =
                   new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Edit", "CommandEdit");
                BtnTmpField.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                gvZipCodeTaxRate.Columns.Add(BtnTmpField);
                BtnTmpField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                TemplateField tfZipCode = new TemplateField();
                tfZipCode.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                                             "Zip Code", "");
                tfZipCode.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item,
                                                              "ZipCode",
                                                             "ZipCode");
                tfZipCode.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem,
                                                              "ZipCode",
                                                             "");
                tfZipCode.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, "ZipCode", "Item");
                tfZipCode.FooterStyle.ForeColor = System.Drawing.Color.Red;

                tfZipCode.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(40);
                gvZipCodeTaxRate.Columns.Add(tfZipCode);
                tfZipCode.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                tfZipCode.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                tfZipCode.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                tfZipCode.FooterStyle.HorizontalAlign = HorizontalAlign.Center;

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

                        tftxrate.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer,
                                                                     TaxStructure.TaxName.ToString(),
                                                                      "Item");
                        tftxrate.FooterStyle.ForeColor = System.Drawing.Color.Red;
                        tftxrate.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(10);
                        gvZipCodeTaxRate.Columns.Add(tftxrate);
                        tftxrate.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                        tftxrate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        tftxrate.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                TemplateField BtnTmpField1 = new TemplateField();
                BtnTmpField1.ItemTemplate =
                    new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Delete", "CommandDelete");
                BtnTmpField1.HeaderTemplate =
                    new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "CommandDelete");
                BtnTmpField1.FooterTemplate =
                   new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, "Add New", "Command");
                BtnTmpField1.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                gvZipCodeTaxRate.Columns.Add(BtnTmpField1);

                BtnTmpField1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }

            else
            {
                TemplateField tfZipCode = new TemplateField();
                tfZipCode.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                                             "Zip Code", "");
                tfZipCode.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, "ZipCode", "Item");
                tfZipCode.FooterStyle.ForeColor = System.Drawing.Color.Red;
                gvZipCodeTaxRate.Columns.Add(tfZipCode);
                tfZipCode.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                tfZipCode.FooterStyle.HorizontalAlign = HorizontalAlign.Center;

                if (ArrlstTaxClass.Count > 0)
                {
                    for (int i = 0; i < ArrlstTaxClass.Count; i++)
                    {
                        TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                        TemplateField tftxrate = new TemplateField();
                        tftxrate.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header,
                                                                                             TaxStructure.TaxName.ToString(), "");

                        tftxrate.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer,
                                                                     TaxStructure.TaxName.ToString(),
                                                                      "Item");
                        tftxrate.FooterStyle.ForeColor = System.Drawing.Color.Red;
                        gvZipCodeTaxRate.Columns.Add(tftxrate);
                        tftxrate.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        tftxrate.FooterStyle.HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                TemplateField BtnTmpField1 = new TemplateField();
                BtnTmpField1.HeaderTemplate =
                                    new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "CommandDelete");
                BtnTmpField1.FooterTemplate =
                   new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, "Add New", "Command");
                BtnTmpField1.ItemStyle.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                gvZipCodeTaxRate.Columns.Add(BtnTmpField1);
                BtnTmpField1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                lblMsg.Text = "No Records Found....";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }

            gvZipCodeTaxRate.DataSource = (DataTable)ViewState["ZipDataTable"];
            gvZipCodeTaxRate.DataBind();
        }

        /// <summary>
        /// Render All
        /// </summary>
        private void RenderAll()
        {
            if (DsGlobal != null)
            {
                BindZipTax();
                DtTableStateTax.AcceptChanges();
                GenerateGrid();
            }
            else
            {
                BindZipTax();
                GenerateGrid();
                lblMsg.Text = "No Records Found...";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Method For Searching
        /// </summary>
        /// <param name="SearchField">string SearchField</param>
        /// <param name="SearchValue">string SearchValue</param>
        protected void Search(string SearchField, string SearchValue)
        {
            lblMsg.Text = "";
            DataView Dv = new DataView();
            BindData(StoreID);
            Dv = TaxClassComponent.SearchForZipCode(SearchField, SearchValue);
            TaxClassComponent.DataSourceZipCode = Dv;
            DsGlobal = TaxClassComponent.DataSourceZipCode;
            RenderAll();
            if (Dv == null)
            {
                lblMsg.Text = "No Records Found...";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
            SearchText = "";
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvZipCodeTaxRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvZipCodeTaxRate.PageIndex = e.NewPageIndex;
            BindData(StoreID);
            GenerateGrid();
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void gvZipCodeTaxRate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            lblMsg.Text = "";
            gvZipCodeTaxRate.EditIndex = -1;
            gvZipCodeTaxRate.DataSource = ((DataTable)ViewState["ZipDataTable"]);
            gvZipCodeTaxRate.DataBind();
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvZipCodeTaxRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Deleting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewDeleteEventArgs e</param>
        protected void gvZipCodeTaxRate_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                objTaxClass.StoreID = StoreID;
                GridViewRow row = gvZipCodeTaxRate.Rows[e.RowIndex];
                DataRow dr = ((DataTable)ViewState["ZipDataTable"]).Rows[e.RowIndex];
                Label txtID = (Label)row.FindControl("ZipCode");

                if (objTaxClass.DeleteZipTax(txtID.Text.ToString(), 4) != -1)
                {
                    ((DataTable)ViewState["ZipDataTable"]).Rows[e.RowIndex].Delete();
                    ((DataTable)ViewState["ZipDataTable"]).AcceptChanges();
                    gvZipCodeTaxRate.DataSource = (DataTable)ViewState["ZipDataTable"];
                    gvZipCodeTaxRate.DataBind();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code has been Deleted successfully...', 'Message','');});", true);
                }
                if (((DataTable)ViewState["ZipDataTable"]).Rows.Count == 0)
                {
                    EmptyFlag = true;
                    BindZipTax();
                    GenerateGrid();
                    gvZipCodeTaxRate.DataSource = (DataTable)ViewState["ZipDataTable"];
                    gvZipCodeTaxRate.DataBind();
                }
                txtSearch.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void gvZipCodeTaxRate_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblMsg.Text = "";
            gvZipCodeTaxRate.EditIndex = e.NewEditIndex;
            gvZipCodeTaxRate.DataSource = ((DataTable)ViewState["ZipDataTable"]);
            gvZipCodeTaxRate.DataBind();
            TextBox txtZipCode = (TextBox)gvZipCodeTaxRate.Rows[e.NewEditIndex].FindControl("ZipCode");
            ViewState["OldZipCode"] = txtZipCode.Text;
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void gvZipCodeTaxRate_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            objTaxClass.StoreID = StoreID;
            GridViewRow row = gvZipCodeTaxRate.Rows[e.RowIndex];
            DataRow dr = ((DataTable)ViewState["ZipDataTable"]).Rows[e.RowIndex];
            dr.BeginEdit();
            TextBox txtZipCode = (TextBox)row.FindControl("ZipCode");
            txtSearch.Text = "";
            if (txtZipCode.Text != "")
            {
                for (int i = 0; i < ArrlstTaxClass.Count; i++)
                {
                    TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                    TextBox txtTaxRate = (TextBox)row.FindControl(TaxStructure.TaxName.ToString());
                    objTaxClass.TaxClassIDForState = TaxStructure.TaxClassID;
                    objTaxClass.ZipCode = txtZipCode.Text;
                    objTaxClass.TaxRate = Convert.ToDecimal(txtTaxRate.Text.ToString());
                    if (objTaxClass.AddZipTax(ViewState["OldZipCode"].ToString(), 3) != -1)
                    {
                        dr[TaxStructure.TaxName] = Convert.ToDecimal(objTaxClass.TaxRate.ToString());
                        dr["ZipCode"] = txtZipCode.Text;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Updated Successfully..', 'Message','');});", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Already Exists...', 'Message','');});", true);
                    }
                }
                dr.EndEdit();
                dr.AcceptChanges();
                gvZipCodeTaxRate.EditIndex = -1;
                BindData(StoreID);
                RenderAll();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Must Be Entered...', 'Message','');});", true);
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }

            if (txtSearch.Text == "")
                BindData(StoreID);
            else
                Search("ZipCode", txtSearch.Text.Trim());
            gvZipCodeTaxRate.PageIndex = 0;
            RenderAll();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            SearchText = txtSearch.Text;
            if (txtSearch.Text != "")
            {
                Search("ZipCode", txtSearch.Text.Trim());
                RenderAll();
            }
            SearchText = "";
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            BindData(StoreID);
            RenderAll();
            SearchText = "";
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvZipCodeTaxRate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool stop = false;
            objTaxClass.StoreID = StoreID;
            if (e.CommandName.Equals("Insert"))
            {
                TextBox txtZipCode;
                DataTable dt = (DataTable)ViewState["ZipDataTable"];
                foreach (GridViewRow dr1 in gvZipCodeTaxRate.Rows)
                {
                    txtZipCode = (TextBox)gvZipCodeTaxRate.FooterRow.FindControl("ZipCode");
                    if (txtZipCode.Text != "")
                    {
                        if (txtZipCode.Text.Length < 11)
                        {
                            int indx = 0;
                            Random rm = new Random();
                            objTaxClass.ZipCode = txtZipCode.Text;
                            DataRow dr = dt.NewRow();
                            dr["ZipCode"] = objTaxClass.ZipCode;

                            for (int i = 0; i < ArrlstTaxClass.Count; i++)
                            {
                                TextBox txtTaxRate;
                                TaxClassStruct TaxStructure = (TaxClassStruct)ArrlstTaxClass[i];
                                txtTaxRate = (TextBox)gvZipCodeTaxRate.FooterRow.FindControl(TaxStructure.TaxName.ToString());
                                if (txtTaxRate.Text == "")
                                    txtTaxRate.Text = "0";
                                if (txtTaxRate.Text.Length < 11)
                                {
                                    objTaxClass.TaxClassIDForState = TaxStructure.TaxClassID;
                                    objTaxClass.TaxRate = Decimal.Parse(txtTaxRate.Text.Trim());
                                    dr[TaxStructure.TaxName] = objTaxClass.TaxRate;
                                    indx = objTaxClass.AddZipTax(Convert.ToString(0), 3);

                                    stop = true;
                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Tax Rate Length Should not be More than 10...', 'Message','');});", true);
                                }
                            }

                            if (indx != -1)
                            {
                                dt.Rows.Add(dr);
                                dt.AcceptChanges();
                                ViewState["ZipDataTable"] = dt;
                                EmptyFlag = false;
                                GenerateGrid();
                                BindData(StoreID);
                                RenderAll();
                                BindZipTax();
                                gvZipCodeTaxRate.DataSource = (DataTable)ViewState["ZipDataTable"];
                                gvZipCodeTaxRate.DataBind();
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Inserted Successfully...', 'Message','');});", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Already Exists...', 'Message','');});", true);
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Length Should not be More than 10...', 'Message','');});", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Zip Code Must Be Entered...', 'Message','');});", true);
                    }
                    if (stop)
                        break;
                }
            }
        }

        /// <summary>
        /// Zip Code Tax Rate Gridview Sorting Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewSortEventArgs e</param>
        protected void gvZipCodeTaxRate_Sorting(object sender, GridViewSortEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            string[] keys = img.ID.Split('_');
            DataTable dt = (DataTable)ViewState["ZipDataTable"];
            DataView dv = new DataView(dt);
            dv.Sort = keys[0] + " " + keys[1];

            gvZipCodeTaxRate.DataSource = dv;
            gvZipCodeTaxRate.DataBind();
        }

        /// <summary>
        /// Import button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMessage.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName);

                    FillMapping(uploadCSV.FileName);
                }
                else
                {


                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    if (InsertDataInDataBase(dtCSV) && lblMessage.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMessage.Text = "TaxRate Updated Successfully";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                        lblMessage.Visible = true;
                        return;

                    }


                }
                else
                {
                    lblMessage.Text += "Sorry file not found. Please retry uploading.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    lblMessage.Visible = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Bind Data with Gridview
        /// </summary>
        private void BindData()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {

            }
            else
                lblMessage.Text = "No data exists in file.";
            lblMessage.Style.Add("color", "#FF0000");
            lblMessage.Style.Add("font-weight", "normal");
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = ",";

                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();

                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "zipcode" || tempFieldName == "salestaxrate")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",zipcode,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",salestaxrate,") > -1)
                        {

                        }
                        else
                        {
                            lblMessage.Text = "File Does not contain all columns";
                            lblMessage.Style.Add("color", "#FF0000");
                            lblMessage.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {

                        BindData();
                    }
                    else
                    {
                        lblMessage.Text = "Please Specify Zipcode,SalestaxRate in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMessage.Text = "Please Specify Zipcode,SalestaxRate in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        private bool InsertDataInDataBase(DataTable dt)
        {


            if (dt.Rows.Count > 0)
            {

                Decimal dd = Decimal.Zero;
                string strstate = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strstate == "")
                    {
                        try
                        {
                            strstate = dt.Rows[i]["State"].ToString();
                        }
                        catch
                        {

                        }

                    }
                    if (String.IsNullOrEmpty(dt.Rows[i]["SalesTaxRate"].ToString()))
                    {
                        dt.Rows[i]["SalesTaxRate"] = 0;
                        dt.AcceptChanges();
                    }
                    else
                    {
                        if (Convert.ToDecimal(dt.Rows[i]["SalesTaxRate"].ToString()) > dd)
                        {
                            dd = Convert.ToDecimal(dt.Rows[i]["SalesTaxRate"].ToString());
                        }
                    }


                    try
                    {
                        CommonComponent.ExecuteCommonData("update tb_ZipTax set TaxRate='" + dt.Rows[i]["SalesTaxRate"].ToString() + "' where ZipCode='" + dt.Rows[i]["zipcode"].ToString() + "'");

                    }
                    catch { }


                }
                if (!string.IsNullOrEmpty(strstate))
                {
                    CommonComponent.ExecuteCommonData("update tb_StateTax set TaxRate='" + dd.ToString() + "' where StateID in (SELECT StateID FROM tb_State WHERE Abbreviation='" + strstate + "')");
                }
            }
            else
            {
                return false;

                StrFileName = "";
            }


            return true;
        }
        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                DataColumn columnID = new DataColumn();
                columnID.Caption = "Number";
                columnID.ColumnName = "Number";
                columnID.AllowDBNull = false;
                columnID.AutoIncrement = true;
                columnID.AutoIncrementSeed = 1;
                columnID.AutoIncrementStep = 1;
                dtCSV.Columns.Add(columnID);
                foreach (string FieldName in FieldNames)
                    dtCSV.Columns.Add(FieldName);
                while (csv.ReadNextRecord())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < FieldCount; i++)
                    {
                        string FieldName = FieldNames[i];
                        if (!dr.Table.Columns.Contains(FieldName))
                        { continue; }

                        dr[FieldName] = csv[i];
                    }
                    dtCSV.Rows.Add(dr);
                }
                dtCSV.AcceptChanges();
                return dtCSV;
            }
        }
    }
}