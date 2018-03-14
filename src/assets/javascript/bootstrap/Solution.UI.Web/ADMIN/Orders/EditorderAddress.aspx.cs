using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class EditorderAddress : BasePage
    {
        public string AddressType = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            imgClosee.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
            btnReset.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btnUpdateEmail.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/ChangeEmail.png";
            if (Request.QueryString["type"] != null && Request.QueryString["id"] != null)
            {
                if (Request.QueryString["type"].ToString().ToLower() == "bill")
                {
                    AddressType = "Billing Address Details";
                }
                else if (Request.QueryString["type"].ToString().ToLower() == "ship")
                {
                    AddressType = "Shipping Address Details";
                }
            }
            if (!IsPostBack)
            {
                FillCountry();
                if (Request.QueryString["type"] != null && Request.QueryString["id"] != null)
                {
                    BindAddress(Request.QueryString["type"].ToString(), Convert.ToInt32(Request.QueryString["id"].ToString()));
                }
            }
        }

        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlCountry.Items.Clear();


            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = dscountry.Tables[0];
                ddlCountry.DataTextField = "Name";
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlCountry.DataSource = null;
                ddlCountry.DataBind();

            }

            if (ddlCountry.Items.FindByText("United States") != null)
            {
                ddlCountry.Items.FindByText("United States").Selected = true;
            }

            ddlCountry_SelectedIndexChanged(null, null);

        }

        /// <summary>
        /// Binds the address.
        /// </summary>
        /// <param name="type">string type</param>
        /// <param name="ordernumber">int ordernumber.</param>
        private void BindAddress(string type, Int32 ordernumber)
        {
            DataSet dsAddress = new DataSet();
            dsAddress = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()));
            if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["Email"].ToString()))
                {
                    txtEmail.Text = dsAddress.Tables[0].Rows[0]["Email"].ToString();
                }
                if (Request.QueryString["type"].ToString().ToLower() == "bill")
                {
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingEmail"].ToString()))
                    {
                        txtEmailAdd.Text = dsAddress.Tables[0].Rows[0]["BillingEmail"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingFirstName"].ToString()))
                    {
                        txtFirstname.Text = dsAddress.Tables[0].Rows[0]["BillingFirstName"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingLastName"].ToString()))
                    {
                        txtLastname.Text = dsAddress.Tables[0].Rows[0]["BillingLastName"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingCompany"].ToString()))
                    {
                        txtCompany.Text = dsAddress.Tables[0].Rows[0]["BillingCompany"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                    {
                        txtAddress1.Text = dsAddress.Tables[0].Rows[0]["BillingAddress1"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                    {
                        txtAddress2.Text = dsAddress.Tables[0].Rows[0]["BillingAddress2"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingCity"].ToString()))
                    {
                        txtCity.Text = dsAddress.Tables[0].Rows[0]["BillingCity"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingSuite"].ToString()))
                    {
                        txtSuite.Text = dsAddress.Tables[0].Rows[0]["BillingSuite"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingCountry"].ToString()))
                    {
                        ddlCountry.ClearSelection();
                        ddlCountry.Items.FindByText(dsAddress.Tables[0].Rows[0]["BillingCountry"].ToString()).Selected = true;
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingState"].ToString()))
                    {
                        try
                        {
                            ddlState.ClearSelection();
                            ddlState.Items.FindByText(dsAddress.Tables[0].Rows[0]["BillingState"].ToString()).Selected = true;
                        }
                        catch
                        {
                            ddlState.Items.FindByText("Other").Selected = true;
                            txtOther.Text = dsAddress.Tables[0].Rows[0]["BillingState"].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingZip"].ToString()))
                    {
                        txtZipcode.Text = dsAddress.Tables[0].Rows[0]["BillingZip"].ToString();

                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        txtPhone.Text = dsAddress.Tables[0].Rows[0]["BillingPhone"].ToString();
                    }

                }
                else if (Request.QueryString["type"].ToString().ToLower() == "ship")
                {
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingEmail"].ToString()))
                    {
                        txtEmailAdd.Text = dsAddress.Tables[0].Rows[0]["ShippingEmail"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingFirstName"].ToString()))
                    {
                        txtFirstname.Text = dsAddress.Tables[0].Rows[0]["ShippingFirstName"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingLastName"].ToString()))
                    {
                        txtLastname.Text = dsAddress.Tables[0].Rows[0]["ShippingLastName"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    {
                        txtCompany.Text = dsAddress.Tables[0].Rows[0]["ShippingCompany"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    {
                        txtAddress1.Text = dsAddress.Tables[0].Rows[0]["ShippingAddress1"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    {
                        txtAddress2.Text = dsAddress.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    {
                        txtCity.Text = dsAddress.Tables[0].Rows[0]["ShippingCity"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    {
                        txtSuite.Text = dsAddress.Tables[0].Rows[0]["ShippingSuite"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    {
                        ddlCountry.ClearSelection();
                        ddlCountry.Items.FindByText(dsAddress.Tables[0].Rows[0]["ShippingCountry"].ToString()).Selected = true;
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingState"].ToString()))
                    {
                        try
                        {
                            ddlState.ClearSelection();
                            ddlState.Items.FindByText(dsAddress.Tables[0].Rows[0]["ShippingState"].ToString()).Selected = true;
                        }
                        catch
                        {
                            ddlState.Items.FindByText("Other").Selected = true;
                            txtOther.Text = dsAddress.Tables[0].Rows[0]["ShippingState"].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    {
                        txtZipcode.Text = dsAddress.Tables[0].Rows[0]["ShippingZip"].ToString();

                    }
                    if (!string.IsNullOrEmpty(dsAddress.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        txtPhone.Text = dsAddress.Tables[0].Rows[0]["ShippingPhone"].ToString();
                    }
                }
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string state = "";
            if (ddlState.SelectedValue.ToString() == "-11")
            {
                state = txtOther.Text.ToString();
            }
            else
            {
                state = ddlState.SelectedItem.Text.ToString();
            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "bill")
            {
                string Sql = "";
                Sql = "UPDATE tb_order SET   BillingEmail='" + txtEmailAdd.Text.ToString().Replace("'", "''") + "', BillingFirstName='" + txtFirstname.Text.ToString().Replace("'", "''") + "', BillingLastName='" + txtLastname.Text.ToString().Replace("'", "''") + "', BillingCompany='" + txtCompany.Text.ToString().Replace("'", "''") + "', BillingAddress1='" + txtAddress1.Text.ToString().Replace("'", "''") + "',";
                Sql += "BillingAddress2='" + txtAddress2.Text.ToString().Replace("'", "''") + "', BillingSuite='" + txtSuite.Text.ToString().Replace("'", "''") + "', BillingCity='" + txtCity.Text.ToString().Replace("'", "''") + "', BillingState='" + state.ToString().Replace("'", "''") + "', BillingZip='" + txtZipcode.Text.ToString().Replace("'", "''") + "', BillingCountry='" + ddlCountry.SelectedItem.Text.ToString().Replace("'", "''") + "', BillingPhone='" + txtPhone.Text.ToString().Replace("'", "''") + "'";
                Sql += " WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "";
                CommonComponent.ExecuteCommonData(Sql);

                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(17, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.location.href=window.parent.location.href;window.close();", true);
            }
            else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString().ToLower() == "ship")
            {
                string Sql = "";

                Sql = "UPDATE tb_order SET   ShippingEmail='" + txtEmailAdd.Text.ToString().Replace("'", "''") + "',  ShippingFirstName='" + txtFirstname.Text.ToString().Replace("'", "''") + "',  ShippingLastName='" + txtLastname.Text.ToString().Replace("'", "''") + "',  ShippingCompany='" + txtCompany.Text.ToString().Replace("'", "''") + "',  ShippingAddress1='" + txtAddress1.Text.ToString().Replace("'", "''") + "',";
                Sql += " ShippingAddress2='" + txtAddress2.Text.ToString().Replace("'", "''") + "',  ShippingSuite='" + txtSuite.Text.ToString().Replace("'", "''") + "',  ShippingCity='" + txtCity.Text.ToString().Replace("'", "''") + "',  ShippingState='" + state.ToString().Replace("'", "''") + "',  ShippingZip='" + txtZipcode.Text.ToString().Replace("'", "''") + "',  ShippingCountry='" + ddlCountry.SelectedItem.Text.ToString().Replace("'", "''") + "',  ShippingPhone='" + txtPhone.Text.ToString().Replace("'", "''") + "'";
                Sql += " WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "";
                CommonComponent.ExecuteCommonData(Sql);
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(18, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.location.href=window.parent.location.href;window.close();", true);
            }
        }

        /// <summary>
        /// Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlState.Items.Clear();
            if (ddlCountry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlCountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlState.DataSource = dsState.Tables[0];
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ddlState.SelectedIndex = 0;
                }
                else
                {
                    ddlState.DataSource = null;
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlState.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlState.SelectedIndex = 0;
                }
            }
            else
            {
                ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlState.Items.Insert(1, new ListItem("Other", "-11"));
                ddlState.SelectedIndex = 0;
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "OtherState();", true);
        }

        /// <summary>
        ///  Reset Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnReset_Click(object sender, ImageClickEventArgs e)
        {
            txtEmail.Text = "";
            txtEmailAdd.Text = "";
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtCity.Text = "";
            txtSuite.Text = "";
            txtZipcode.Text = "";
            txtPhone.Text = "";

            BindAddress(Request.QueryString["type"].ToString(), Convert.ToInt32(Request.QueryString["id"].ToString()));
        }

        /// <summary>
        ///  Update Email Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateEmail_Click(object sender, ImageClickEventArgs e)
        {
            string Sql = "";
            if (Request.QueryString["id"] != null)
            {
                Sql = "UPDATE tb_order SET Email='" + txtEmail.Text.ToString().Replace("'", "''") + "'";
                Sql += " WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "";
                CommonComponent.ExecuteCommonData(Sql);
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(16, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.location.href=window.parent.location.href;window.close();", true);
        }

    }
}