using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.ShippingMethods;
using System.Text;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class ShippingCalculation : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                FillCountry();
            }
            ddlcountry.Focus();
            Page.Form.DefaultButton = btnSubmit.UniqueID;
        }

        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlcountry.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlcountry.DataSource = dscountry.Tables[0];
                ddlcountry.DataTextField = "Name";
                ddlcountry.DataValueField = "CountryID";
                ddlcountry.DataBind();
            }
            else
            {
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
            }
            ddlcountry.Items.Insert(0, new ListItem("Select Country", "0"));

            if (ddlcountry.Items.FindByText("United States") != null)
            {
                ddlcountry.Items.FindByText("United States").Selected = true;
            }
        }

        #region Bind ShippingMethod

        /// <summary>
        /// Submit Button Click Event for Get Shipping Method by ZipCode
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {

            DataSet dsWeight = new DataSet();
            dsWeight = ProductComponent.GetproductImagename(Convert.ToInt32(Request.QueryString["ProductId"].ToString()));
            decimal objdecimal = decimal.Zero;
            if (dsWeight != null && dsWeight.Tables.Count > 0 && dsWeight.Tables[0].Rows.Count > 0)
            {
                objdecimal = Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString());

            }
            CountryComponent objCountry = new CountryComponent();
            ltrshipping.Text = "";
            ltrshipping.Text = "<div>";
            ltrshipping.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" class=\"table\">";
            ltrshipping.Text += "<tbody><tr>";
            ltrshipping.Text += "<th width=\"60%\" style='color:#FFFFFF;background-color:#E86520'>";
            ltrshipping.Text += "Shipping Name";
            ltrshipping.Text += "</th>";
            ltrshipping.Text += "<th align=\"right\" width=\"15%\" style='color:#FFFFFF;background-color:#E86520' >";
            ltrshipping.Text += "Charge";
            ltrshipping.Text += "</th>";
            ltrshipping.Text += "<th align=\"right\" width=\"25%\" style='color:#FFFFFF;background-color:#E86520' >";
            ltrshipping.Text += "Order&nbsp;Total";
            ltrshipping.Text += "</th>";
            ltrshipping.Text += "<th align=\"right\" width=\"25%\" style='color:#FFFFFF;background-color:#E86520' >";
            ltrshipping.Text += "Estimated&nbsp;Days";
            ltrshipping.Text += "</th>";
            ltrshipping.Text += "</tr>";
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(ddlcountry.SelectedValue.ToString()));

            BindShippingMethod(CountryCode, "", txtZipCode.Text.ToString(), objdecimal);


            ltrshipping.Text += "</table>";
            ltrshipping.Text += "</div>";
        }

        /// <summary>
        /// Bind Shipping Method By ZipCode
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        private void BindShippingMethod(string Country, string State, string ZipCode, decimal Weight)
        {
            string strUSPSMessage = "";
            string strUPSMessage = "";
            string strFedexSMessage = "";
            lblMsg.Text = "";



            if (ZipCode == "" || Country == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select Country and Enter Zip Code";
                return;
            }

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));



            string CountryCode = Country;


            if (Weight == 0)
            {
                Weight = 1;

            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("EstimatedDays", typeof(DateTime));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));
            UPSTable.Columns.Add("EstimatedDays", typeof(DateTime));


            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));
            USPSTable.Columns.Add("EstimatedDays", typeof(DateTime));


            DataTable FedexTable = new DataTable();
            FedexTable.Columns.Add("ShippingMethodName", typeof(String));
            FedexTable.Columns.Add("Price", typeof(decimal));
            FedexTable.Columns.Add("EstimatedDays", typeof(DateTime));

            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), State, ZipCode, Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    USPSTable = objRate.EndiciaGetRatesEstimatedDays(ZipCode, CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                {
                    FedexTable = FedexMethod(Convert.ToDecimal(Weight), State, ZipCode, CountryCode, ref strFedexSMessage);


                }
                if (UPSTable != null && UPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(UPSTable);
                }
                if (USPSTable != null && USPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(USPSTable);
                }
                if (FedexTable != null && FedexTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(FedexTable);
                }

                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    bool IsFreeShipping = false;
                    IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) FROM tb_CustomerLevel inner JOIN dbo.tb_Customer ON tb_Customer.CustomerLevelID=tb_CustomerLevel.CustomerLevelID WHERE tb_Customer.CustomerID=" + Convert.ToInt32(Session["CustID"].ToString()) + ""));
                    if (IsFreeShipping)
                    {
                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            String strFreeShipping = "Standard Shipping($0.00)";
                            DataRow dataRow = ShippingTable.NewRow();
                            dataRow["ShippingMethodName"] = strFreeShipping;
                            dataRow["Price"] = 0;
                            dataRow["EstimatedDays"] = System.DateTime.Now.Date.ToShortDateString();
                            ShippingTable.Rows.Add(dataRow);
                        }
                    }
                }
                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    //*rdoShipping.DataSource = dvShipping.ToTable();
                    //*
                    DataTable dtAllShipping = dvShipping.ToTable();

                    for (int i = 0; i <= dtAllShipping.Rows.Count - 1; i++)
                    {

                        // dtAllShipping.Rows[i]["ShippingMethodName"];
                        //     dtAllShipping.Rows[i]["Price"];
                        //   dtAllShipping.Rows[i]["EstimatedDays"];

                        ltrshipping.Text += "<tr>";
                        ltrshipping.Text += "<td align=\"left\" valign=\"top\"  >";
                        ltrshipping.Text += dtAllShipping.Rows[i]["ShippingMethodName"].ToString();
                        ltrshipping.Text += "</td>";
                        ltrshipping.Text += "<td align=\"right\" valign=\"top\"  >";
                        ltrshipping.Text += Convert.ToDecimal(dtAllShipping.Rows[i]["Price"]).ToString("C");
                        ltrshipping.Text += "</td>";


                        ltrshipping.Text += "<td align=\"right\" valign=\"top\"  >";
                        ltrshipping.Text += (Convert.ToDecimal(dtAllShipping.Rows[i]["Price"].ToString()) + Convert.ToDecimal(hdnprice.Value.ToString())).ToString("C");
                        ltrshipping.Text += "</td>";

                        if (dtAllShipping.Rows[i]["EstimatedDays"] != null)
                        {
                            try
                            {
                                ltrshipping.Text += "<td align=\"centre\" valign=\"top\"  >";
                                ltrshipping.Text += (Convert.ToDateTime(dtAllShipping.Rows[i]["EstimatedDays"].ToString())).ToShortDateString();
                                ltrshipping.Text += "</td>";
                            }
                            catch
                            {
                            }
                        }

                        ltrshipping.Text += "</tr>";

                    }
                }


                if (strUSPSMessage != "" && strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage + strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUSPSMessage != "")
                {
                    lblMsg.Text = strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage;
                    lblMsg.Visible = true;
                }
            }
        }


        /// <summary>
        /// Fedex Method Bind
        /// </summary>
        /// <param name="Weight">Decimal Weight</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Country">String Country</param>
        /// <param name="StrMessage">Return Error Message </param>
        /// <returns>Returns the All Fedex Method as a Dataset</returns>
        private DataTable FedexMethod(decimal Weight, string State, string ZipCode, string Country, ref string StrMessage)
        {

            //   string GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, CountryCode.ToString(), Convert.ToInt32(Session["CustID"]), true));

            if (ZipCode == "" || Country == "")
            {
                return null;
            }
            StringBuilder tmpFixedShipping = new StringBuilder(4096);
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("EstimatedDays", typeof(DateTime));
            string GetFedexrate = "";
            Fedex obj = new Fedex();
            //      StringBuilder tmpRealTimeShipping = new StringBuilder(4096);

            if (Weight > decimal.Zero)
            {
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(Weight), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));
            }
            else
                GetFedexrate = Convert.ToString(obj.FedexGetRates(Convert.ToDecimal(1), "", "", "", State, ZipCode, Country.ToString(), Convert.ToInt32(Session["CustID"]), true));

            //FedEx Methods

            tmpRealTimeShipping.Append((string)GetFedexrate);
            string strResult = GetFedexrate;

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "FEDEX");
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {

                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion
            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString() == "")
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());


                    DataSet dsShipping = new DataSet();
                    string shippingQuery = " SELECT AdditionalPrice,DATEADD(DAY,isnull(EstimatedDays,0),GETDATE()) as EstimatedDays FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='FEDEX' AND isnull(tb_ShippingMethods.Active,0)=1 AND isnull(tb_ShippingMethods.Deleted,0)=0 and Name='" + strMethodname[0].ToString() + "'";
                    dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                    if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                    {
                        dataRow["EstimatedDays"] = Convert.ToDateTime(dsShipping.Tables[0].Rows[0]["EstimatedDays"].ToString()).ToShortDateString();
                    }
                    else
                        dataRow["EstimatedDays"] = null;


                    ShippingTable.Rows.Add(dataRow);
                }
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());

                    DataSet dsShipping = new DataSet();
                    string shippingQuery = " SELECT AdditionalPrice,DATEADD(DAY,isnull(EstimatedDays,0),GETDATE()) as EstimatedDays FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='FEDEX' AND isnull(tb_ShippingMethods.Active,0)=1 AND isnull(tb_ShippingMethods.Deleted,0)=0 and Name='" + strMethodname[0].ToString() + "'";
                    dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                    if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                    {
                        dataRow["EstimatedDays"] = Convert.ToDateTime(dsShipping.Tables[0].Rows[0]["EstimatedDays"].ToString()).ToShortDateString();
                    }
                    else
                        dataRow["EstimatedDays"] = null;

                    ShippingTable.Rows.Add(dataRow);
                }
            }

            return ShippingTable;

        }

        /// <summary>
        /// UPS Method Bind
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        /// <returns>Returns the Dataset of UPS Methods</returns>
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, ref string StrMessage)
        {
            if (ZipCode == "" || Country == "")
            {
                return null;
            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("EstimatedDays", typeof(DateTime));



            UPS obj = new UPS(AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2"), AppLogic.AppConfigs("Shipping.OriginCity"), AppLogic.AppConfigs("Shipping.OriginState"), AppLogic.AppConfigs("Shipping.OriginZip"), AppLogic.AppConfigs("Shipping.OriginCountry"));
            obj.DestinationCountryCode = Country;
            obj.DestinationStateProvince = State;
            obj.DestinationZipPostalCode = ZipCode;
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            if (Weight > decimal.Zero)
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(Weight), Convert.ToDecimal(0), true));
            }
            else
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(1), Convert.ToDecimal(0), true));
            }
            string strResult = tmpRealTimeShipping.ToString();

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), ServiceName);
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {

                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion

            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString().ToLower().IndexOf("error") > -1)
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());


                    DataSet dsShipping = new DataSet();
                    string shippingQuery = " SELECT AdditionalPrice,DATEADD(DAY,isnull(EstimatedDays,0),GETDATE()) as EstimatedDays FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='UPS' AND isnull(tb_ShippingMethods.Active,0)=1 AND isnull(tb_ShippingMethods.Deleted,0)=0 and Name='" + strMethodname[0].ToString() + "'";
                    dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                    if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                    {
                        dataRow["EstimatedDays"] = Convert.ToDateTime(dsShipping.Tables[0].Rows[0]["EstimatedDays"].ToString()).ToShortDateString();
                    }
                    else
                        dataRow["EstimatedDays"] = null;

                    ShippingTable.Rows.Add(dataRow);



                }
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());

                    DataSet dsShipping = new DataSet();
                    string shippingQuery = " SELECT AdditionalPrice,DATEADD(DAY,EstimatedDays,GETDATE()) as EstimatedDays FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='UPS' AND isnull(tb_ShippingMethods.Active,0)=1 AND isnull(tb_ShippingMethods.Deleted,0)=0 and Name='" + strMethodname[0].ToString() + "'";
                    dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                    if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                    {
                        dataRow["EstimatedDays"] = Convert.ToDateTime(dsShipping.Tables[0].Rows[0]["EstimatedDays"].ToString()).ToShortDateString();
                    }
                    else
                        dataRow["EstimatedDays"] = DBNull.Value;

                    ShippingTable.Rows.Add(dataRow);


                }

            }

            return ShippingTable;
        }


        #endregion
    }
}