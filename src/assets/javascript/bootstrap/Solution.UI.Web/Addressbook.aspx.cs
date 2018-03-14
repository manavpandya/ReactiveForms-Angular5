using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web
{
    public partial class Addressbook : System.Web.UI.Page
    {
        #region Local Variables

        CustomerComponent objCustomer = null;
        tb_Customer tb_Customer = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            if (Session["CustID"] == null)
            {
                Response.Redirect("/Login.aspx", true);
            }

            EditAddress();
            if (!IsPostBack)
            {
                if (Session["CustID"] != null && Session["CustID"].ToString().Trim().Length > 0)
                {
                    GetCustomerAddressInfo();

                    //Session["FirstName"] = tb_Customer.FirstName.ToString();
                }
                Session["PageName"] = "Address Book";
                if (Request.QueryString["msg"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('" + Request.QueryString["msg"].ToString() + "');", true);
                }
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// Method for Detect that which button's Click event is fired for Delete and Edit Process
        /// </summary>
        private void EditAddress()
        {
            if (Session["CustID"] != null)
            {
                string[] formkeys = Request.Form.AllKeys;
                foreach (String s in formkeys)
                {
                    //To Move Address Details and Redirect to EditAddress page
                    if (s.Contains("bt_MoveToEditAddress"))
                    {
                        string[] p = s.Split(':');
                        MoveToEditAddress(p[1].ToString(), Convert.ToInt32(p[2].ToString()));
                    }
                    //For Delete Address From Address Table
                    if (s.Contains("bt_DeleteFromAddress"))
                    {
                        string[] p = s.Split(':');
                        DeleteAddress(Convert.ToInt32(p[1].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Move To EditAddress to Change Customer's Billing Or Shipping Address
        /// </summary>
        /// <param name="AddType">string AddType</param>
        /// <param name="CategoryID">int AddressID</param>
        private void MoveToEditAddress(string AddType, int AddressID)
        {
            //To Move Address Details and Redirect to EditAddress page
            Session["__ISREFRESH"] = null;
            string strurl = "editaddress.aspx?addtype=" + AddType + "&type=edit&addid=" + AddressID;
            Response.Redirect(strurl.ToLower());
        }

        /// <summary>
        /// To Delete Address using AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        private void DeleteAddress(Int32 AddressID)
        {
            objCustomer = new CustomerComponent();
            try
            {
                if (objCustomer.DeleteAddressByAddressID(AddressID))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Address is deleted successfully.');", true);
                    GetCustomerAddressInfo();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in deleting address, please try again.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Using Customer Identity retrieves Customers information
        /// </summary>
        public void GetCustomerAddressInfo()
        {
            objCustomer = new CustomerComponent();
            DataSet dsCustomerDetail = new DataSet();
            dsCustomerDetail = objCustomer.GetCustomerDetailByCustID(Convert.ToInt32(Session["CustID"]));
            if (dsCustomerDetail != null && dsCustomerDetail.Tables.Count > 0 && dsCustomerDetail.Tables[0].Rows.Count > 0)
            {
                String strBillAddress = "";
                String strShippAddress = "";

                if (Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) > 0)
                {
                    DataSet dsAddress = new DataSet();
                    dsAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"])));

                    if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
                    {
                        strBillAddress += "<tr>";
                        strBillAddress += "<td><strong>" + Convert.ToString(dsAddress.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsAddress.Tables[0].Rows[0]["LastName"]) + "</strong><br>";

                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) != "")
                        {
                            strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) + "<br>";
                        }
                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address1"]) + "<br>";

                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) != "")
                        {
                            strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) + "<br>";
                        }
                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) != "")
                        {
                            strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) + "<br>";
                        }

                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["City"]) + "<br>";
                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["State"]) + "<br>";
                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["ZipCode"]) + "<br>";
                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["CountryName"]) + "<br>";
                        strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Phone"]) + "<br>";
                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) != "")
                        {
                            strBillAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) + "<br>";
                        }
                        strBillAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' value='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' id='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' class='btn1' onClick='" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]) + "\").click();'>Edit</a>";
                        strBillAddress += "</td></tr>";
                        ltBilling.Text = strBillAddress.ToString().Trim();
                    }
                }

                if (Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) > 0)
                {
                    DataSet dsAddress = new DataSet();
                    dsAddress = objCustomer.GetAddressDetailByAddressID(Convert.ToInt32(Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"])));

                    if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
                    {
                        strShippAddress += "<tr>";
                        strShippAddress += "<td><strong>" + Convert.ToString(dsAddress.Tables[0].Rows[0]["FirstName"]) + " " + Convert.ToString(dsAddress.Tables[0].Rows[0]["LastName"]) + "</strong><br>";

                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) != "")
                        {
                            strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Company"]) + "<br>";
                        }
                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address1"]) + "<br>";

                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) != "")
                        {
                            strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Address2"]) + "<br>";
                        }
                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) != "")
                        {
                            strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Suite"]) + "<br>";
                        }

                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["City"]) + "<br>";
                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["State"]) + "<br>";
                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["ZipCode"]) + "<br>";
                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["CountryName"]) + "<br>";
                        strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Phone"]) + "<br>";
                        if (Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) != "")
                        {
                            strShippAddress += Convert.ToString(dsAddress.Tables[0].Rows[0]["Fax"]) + "<br>";
                        }
                        strShippAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' value='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' id='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' class='btn1' onClick='" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]) + "\").click();'>Edit</a>";
                        strShippAddress += "</td></tr>";
                        ltShipping.Text = strShippAddress.ToString().Trim();
                    }
                }

                // Get Other Address of Customer

                DataSet dsOtherAddress = new DataSet();

                dsOtherAddress = objCustomer.GetAddressDetailByCustID(Convert.ToInt32(Session["CustID"]));

                if (dsOtherAddress != null && dsOtherAddress.Tables.Count > 0 && dsOtherAddress.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsOtherAddress.Tables[0].Rows.Count; i++)
                    {
                        int AddressID = Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString());
                        int AddressType = Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressType"].ToString());

                        if (AddressID > 0 && AddressType == 0)
                        {
                            //Bind Customers Billing Address
                            //strBillAddress = "";
                            if (AddressID != Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["BillingAddressID"]))
                            {
                                strBillAddress += "<tr>";
                                strBillAddress += "<td><strong>" + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["FirstName"]) + " " + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["LastName"]) + "</strong><br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) + "<br>";
                                }
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address1"]) + "<br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) + "<br>";
                                }
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) + "<br>";
                                }

                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["City"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["State"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["ZipCode"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["CountryName"]) + "<br>";
                                strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Phone"]) + "<br>";
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) != "")
                                {
                                    strBillAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) + "<br>";
                                }
                                strBillAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:BillingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "\").click();'>Edit</a>";
                                strBillAddress += "&nbsp;&nbsp;<input type='submit' runat='server' style='display: none;' name='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick=\"if( confirm('Are you sure you want to delete this Billing Address?') ) { document.getElementById('bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "').click(); } else { return false;}\">Delete</a>";
                                strBillAddress += "</td></tr>";
                                ltBilling.Text = strBillAddress.ToString().Trim();
                            }
                        }
                        if (AddressID > 0 && AddressType == 1)
                        {
                            //Bind Customers Shipping Address
                            //strShippAddress = "";
                            if (AddressID != Convert.ToInt32(dsCustomerDetail.Tables[0].Rows[0]["ShippingAddressID"]))
                            {
                                strShippAddress += "<tr>";
                                strShippAddress += "<td><strong>" + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["FirstName"]) + " " + Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["LastName"]) + "</strong><br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Company"]) + "<br>";
                                }
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address1"]) + "<br>";

                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Address2"]) + "<br>";
                                }
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Suite"]) + "<br>";
                                }

                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["City"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["State"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["ZipCode"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["CountryName"]) + "<br>";
                                strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Phone"]) + "<br>";
                                if (Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) != "")
                                {
                                    strShippAddress += Convert.ToString(dsOtherAddress.Tables[0].Rows[i]["Fax"]) + "<br>";
                                }
                                strShippAddress += "<input type='submit' runat='server' style='display: none;' name='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick='document.getElementById(\"bt_MoveToEditAddress:ShippingAdd:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "\").click();'>Edit</a>";
                                strShippAddress += "&nbsp;&nbsp;<input type='submit' runat='server' style='display: none;' name='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' value='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' id='bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' class='btn1' onClick='" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "' width='70px' height='15px' ><a href='#' style='text-decoration:underline;font-weight:bold;font-size:12px;' onclick=\"if( confirm('Are you sure you want to delete this Shipping Address?') ) { document.getElementById('bt_DeleteFromAddress:" + Convert.ToInt32(dsOtherAddress.Tables[0].Rows[i]["AddressID"].ToString()) + "').click(); } else { return false;}\">Delete</a>";
                                strShippAddress += "</td></tr>";
                                ltShipping.Text = strShippAddress.ToString().Trim();
                            }
                        }
                    }
                }
            }
        }
    }
}