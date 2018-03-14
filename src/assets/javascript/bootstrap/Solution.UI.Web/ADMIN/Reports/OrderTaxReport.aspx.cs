using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class OrderTaxReport : BasePage
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
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

                BindStore();
                BindState();
                Binddata();
            }

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
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
            //ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
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
        /// Bind All Stats in Drop down
        /// </summary>
        private void BindState()
        {
            DataSet dsState = new DataSet();
            //objSql = new SQLAccess();

            if (ddlStore.SelectedValue != "0")
            {
                //dsState = CommonComponent.GetCommonDataSet("select tb_Ecomm_State.StateName as StateName, tb_Ecomm_StateTax.StateID as StateID, tb_Ecomm_StateTax.StateTaxID as StateTaxID from tb_Ecomm_StateTax inner join tb_Ecomm_State on tb_Ecomm_State.StateID=tb_Ecomm_StateTax.StateID inner join tb_ecomm_TaxClass on tb_ecomm_StateTax.TaxClassID=tb_ecomm_TaxClass.TaxClassID where tb_Ecomm_StateTax.TaxRate>0 and tb_ecomm_TaxClass.StoreID=" + ddlStore.SelectedValue + " order by tb_Ecomm_State.StateName");
                dsState = CommonComponent.GetCommonDataSet("select tb_State.Name as StateName, tb_StateTax.StateID as StateID,tb_StateTax.StateTaxID as StateTaxID from tb_StateTax inner join tb_State ON tb_State.StateID=tb_StateTax.StateID inner join tb_TaxClass on tb_StateTax.TaxClassID=tb_TaxClass.TaxClassID where tb_StateTax.TaxRate>0 and tb_TaxClass.StoreID=" + ddlStore.SelectedValue + " order by tb_State.Name");
            }
            else
            {
                dsState = CommonComponent.GetCommonDataSet(" select tb_State.Name as StateName, tb_StateTax.StateID as StateID,tb_StateTax.StateTaxID as StateTaxID from tb_StateTax inner join tb_State on tb_State.StateID=tb_StateTax.StateID where tb_StateTax.TaxRate>0 order by Name ");
            }

            if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
            {
                ddlState.DataSource = dsState;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateID";
                ddlState.DataBind();
                ddlState.SelectedIndex = 0;
            }
            else
            {
                ddlState.Items.Clear();
            }

        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindState();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Binddata();
        }

        /// <summary>
        /// Binds Data for Order Tax Report
        /// </summary>
        private void Binddata()
        {
            string strSql = "select tb_Order.IsNew as IsNew,OrderNumber as OrderNumber, (select StoreName FROM tb_Store where tb_Store.StoreID=tb_Order.StoreID) as StoreName,OrderDate as OrderDate, OrderTax as OrderTax,OrderTotal,(ISNULL(tb_Order.ShippingLastName,'')+' '+isnull(tb_order.ShippingFirstName,'')+'<br />'+isnull(tb_Order.ShippingAddress1,'')+' '+isnull(tb_Order.ShippingAddress2,'')+' '+isnull(tb_Order.ShippingAddress2,'')+' '+isnull(tb_Order.ShippingSuite,'')+' '+isnull(tb_Order.ShippingCity,'')+' '+isnull(tb_Order.ShippingState,'')+' '+ISNULL(tb_Order.ShippingZip,'')+'<br /> '+isnull((select Name from tb_Country where tb_Country.Name=tb_Order.ShippingCountry),''))as  FullShippingAddress from tb_Order where tb_Order.Deleted=0";
            if (ddlStore.SelectedIndex > 0)
            {
                strSql += " AND tb_Order.Storeid=" + ddlStore.SelectedValue.ToString() + "";
            }
            if (ddlState.SelectedIndex > -1)
            {
                //strSql += " AND tb_Order.ShippingState=" + ddlState.SelectedValue.ToString() + "";
                strSql += " AND tb_Order.ShippingState IN (SELECT  NAME FROM dbo.tb_State WHERE StateID='" + ddlState.SelectedValue.ToString() + "')";
            }
            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

            }


            if (txtMailFrom.Text.ToString() != "")
            {
                strSql += " AND Convert(char(10),tb_Order.OrderDate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtMailTo.Text.ToString() != "")
            {

                strSql += " AND Convert(char(10),tb_Order.OrderDate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            strSql = strSql + " ORDER BY tb_order.OrderNumber desc";
            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql);
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grvordertax.DataSource = dsMail;
                grvordertax.DataBind();
                trdelete.Visible = true;
            }
            else
            {
                trdelete.Visible = false;
                grvordertax.DataSource = null;
                grvordertax.DataBind();
            }
        }

        /// <summary>
        ///  List Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvordertax_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvordertax.PageIndex = e.NewPageIndex;
            Binddata();
        }

        Decimal total = Decimal.Zero;
        Decimal totaltax = decimal.Zero;

        /// <summary>
        /// Order Tax Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvordertax_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ordertotal"));
                totaltax += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "OrderTax"));
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblamount = (Label)e.Row.FindControl("lblTotal");
                lblamount.Text = total.ToString("C").Replace("(", "-").Replace(")", "");

                Label lbltaxamount = (Label)e.Row.FindControl("lblTotaltax");
                lbltaxamount.Text = totaltax.ToString("C").Replace("(", "-").Replace(")", "");
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged1(object sender, EventArgs e)
        {
            Binddata();
        }
    }
}