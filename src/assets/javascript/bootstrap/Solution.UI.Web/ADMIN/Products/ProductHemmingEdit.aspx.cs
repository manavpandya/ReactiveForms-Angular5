using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductHemmingEdit : BasePage
    {
        Int32 chkallowedflg = 0;
        Int32 ProductID = 0;
        Int32 HemmingPercentage = 0;
        Int32 Qty84 = 0;
        Int32 Qty96 = 0;
        Int32 Qty108 = 0;
        Int32 Qty120 = 0;
        Int32 Qty84F = 0;
        Int32 Qty96F = 0;
        Int32 Qty108F = 0;
        Int32 Qty120F = 0;

        Int32 Qty84W = 0;
        Int32 Qty96W = 0;
        Int32 Qty108W = 0;
        Int32 Qty120W = 0;
        Int32 Qty84FW = 0;
        Int32 Qty96FW = 0;
        Int32 Qty108FW = 0;
        Int32 Qty120FW = 0;
        Int32 strAllowHeming = 0;
        string strparent = "";
        string iskyon = "";
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
        DataSet dsNew = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            if (!IsPostBack)
            {

                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");
                btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
                if (strAllowHeming == 1)
                {
                    chkonoff.Checked = true;
                    ViewState["hon"] = "1";
                }
                else
                {
                    chkonoff.Checked = false;
                    ViewState["hon"] = "0";
                }
                //FillGridSKU();


            }
            if (divsearch.Visible == true)
            {
                if (chkonoff.Checked)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgOnOff", "MakeCheckedall('true','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgOnOff", "MakeCheckedall('false','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
                }
            }


        }
        private void FillGridSKU()
        {

            DataSet dsGrid = new DataSet();
            dsGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 1,0,'" + txtSearch.Text.ToString() + "'");
            strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
            if (strAllowHeming == 1)
            {
                chkonoff.Checked = true;
            }
            else
            {
                chkonoff.Checked = false;
            }

            DataSet dsnewdata = new DataSet();

            if (dsGrid != null && dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {
                dsnewdata = dsGrid.Clone();



                for (int i = 0; i < dsGrid.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsnewdata.Tables[0].NewRow();
                    DataSet dsGridChild = new DataSet();

                    dsGridChild = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 2," + dsGrid.Tables[0].Rows[i]["ProductID"].ToString() + ",'" + txtSearch.Text.ToString() + "'");
                    Qty84 = 0;
                    Qty96 = 0;
                    Qty108 = 0;
                    Qty120 = 0;
                    Qty84F = 0;
                    Qty96F = 0;
                    Qty108F = 0;
                    Qty120F = 0;

                    Qty84W = 0;
                    Qty96W = 0;
                    Qty108W = 0;
                    Qty120W = 0;
                    Qty84FW = 0;
                    Qty96FW = 0;
                    Qty108FW = 0;
                    Qty120FW = 0;




                    if (dsGridChild != null && dsGridChild.Tables.Count > 0 && dsGridChild.Tables[0].Rows.Count > 0)
                    {
                        dsNew = dsGridChild;
                        if (!string.IsNullOrEmpty(dsGrid.Tables[0].Rows[i]["IsHamming"].ToString()) && (dsGrid.Tables[0].Rows[i]["IsHamming"].ToString().ToLower() == "true" || dsGrid.Tables[0].Rows[i]["IsHamming"].ToString().ToLower() == "1") && strAllowHeming == 1)
                        {
                            if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                            {
                                Int32 tempQty84 = 0;
                                Int32 tempQty96 = 0;
                                Int32 tempQty108 = 0;
                                Int32 tempQty120 = 0;


                                Int32 tempQty84W = 0;
                                Int32 tempQty96W = 0;
                                Int32 tempQty108W = 0;
                                Int32 tempQty120W = 0;

                                Int32 StoreQty = 0;
                                bool fx84 = false;
                                bool fx96 = false;
                                bool fx108 = false;
                                bool fx120 = false;
                                for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                                {
                                    //double Qty = Convert.ToDouble(); 
                                    Int32 Qty = 0;
                                    Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(100) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                                    StoreQty = Qty;
                                    Int32 Qty1 = 0;
                                    if (chkonoff.Checked)
                                    {

                                        Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString()) - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());

                                    }
                                    else
                                    {
                                        Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                                    }



                                    Qty = Qty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    if (Qty1 < 0)
                                    {
                                        Qty1 = 0;
                                    }
                                    if (Qty < 0)
                                    {
                                        Qty = 0;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());

                                    //    //Qty = Qty;
                                    //}

                                    //if (Convert.ToInt32(Qty) >= Quantity)
                                    //{

                                    if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {

                                        tempQty84 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        if (chkonoff.Checked)
                                        {
                                            tempQty84W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        }
                                        else
                                        {
                                            tempQty84W = StoreQty;
                                        }
                                        Qty84 = Qty + Qty96;
                                        Qty84F = Qty;

                                        Qty84W = Qty1 + Qty96W;
                                        Qty84FW = Qty1;
                                        fx84 = true;


                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        tempQty96 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());

                                        if (chkonoff.Checked)
                                        {
                                            tempQty96W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        }
                                        else
                                        {
                                            tempQty96W = StoreQty;
                                        }

                                        Qty96 = Qty + Qty108;
                                        Qty96F = Qty;
                                        Qty84 = Qty84F + Qty96;


                                        Qty96W = Qty1 + Qty108W;
                                        Qty96FW = Qty1;
                                        Qty84W = Qty84FW + Qty96W;
                                        fx96 = true;
                                        //Qty84 = Qty84
                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        tempQty108 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        if (chkonoff.Checked)
                                        {
                                            tempQty108W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        }
                                        else
                                        {
                                            tempQty108W = StoreQty;
                                        }
                                        Qty108 = Qty + Qty120;
                                        Qty108F = Qty;
                                        Qty96 = Qty96F + Qty108;
                                        Qty84 = Qty84F + Qty96;

                                        Qty108W = Qty1 + Qty120W;
                                        Qty108FW = Qty1;
                                        Qty96W = Qty96FW + Qty108W;
                                        Qty84W = Qty84FW + Qty96W;
                                        fx108 = true;
                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {

                                        Qty120 = Qty;
                                        Qty120F = Qty;
                                        Qty108 = Qty108F + Qty120;
                                        Qty96 = Qty96F + Qty108;
                                        Qty84 = Qty84F + Qty96;

                                        Qty120W = Qty1;
                                        Qty120FW = Qty1;
                                        Qty108W = Qty108FW + Qty120W;
                                        Qty96W = Qty96FW + Qty108W;
                                        Qty84W = Qty84FW + Qty96W;
                                        fx120 = true;
                                    }




                                }
                                if (tempQty84 < 0)
                                {
                                    if (Qty84 > (-(tempQty84)))
                                    {
                                        Qty84 = Qty84 + tempQty84;

                                    }
                                    else
                                    {
                                        Qty84 = 0;

                                    }
                                }
                                if (tempQty96 < 0)
                                {
                                    if (Qty96 > (-(tempQty96)))
                                    {
                                        Qty96 = Qty96 + tempQty96;

                                    }
                                    else
                                    {
                                        Qty96 = 0;

                                    }
                                }
                                if (tempQty108 < 0)
                                {
                                    if (Qty108 > (-(tempQty108)))
                                    {
                                        Qty108 = Qty108 + tempQty108;

                                    }
                                    else
                                    {
                                        Qty108 = 0;

                                    }
                                }
                                if (tempQty84W < 0)
                                {
                                    if (Qty84W > (-(tempQty84W)))
                                    {
                                        Qty84W = Qty84W + tempQty84W;

                                    }
                                    else
                                    {

                                        Qty84W = 0;
                                    }
                                }
                                if (tempQty96W < 0)
                                {
                                    if (Qty96W > (-(tempQty96W)))
                                    {
                                        Qty96W = Qty96W + tempQty96W;

                                    }
                                    else
                                    {

                                        Qty96W = 0;
                                    }
                                }
                                if (tempQty108W < 0)
                                {
                                    if (Qty108W > (-(tempQty108W)))
                                    {
                                        Qty108W = Qty108W + tempQty108W;

                                    }
                                    else
                                    {

                                        Qty108W = 0;
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                            {
                                bool fx84 = false;
                                bool fx96 = false;
                                bool fx108 = false;
                                bool fx120 = false;
                                for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                                {
                                    //double Qty = Convert.ToDouble(); 
                                    Int32 Qty = 0;
                                    Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(100) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                                    Int32 Qty1 = 0;
                                    //if ((hdnchkallowed.Value.ToString() == "1" || hdnchkallowed.Value.ToString().Trim().ToLower() == "true"))
                                    //{
                                    Qty = Qty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    //if (chkonoff.Checked)
                                    //{

                                    //   // Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString()) - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    //}
                                    //else
                                    //{
                                    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                                    //}
                                    //}
                                    //else
                                    //{
                                    //    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                                    //}
                                    //if (Convert.ToInt32(Qty) >= Quantity)
                                    //{

                                    if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        Qty84 = Qty;
                                        Qty84W = Qty1;
                                        fx84 = true;
                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        Qty96 = Qty;
                                        Qty96W = Qty1;
                                        fx96 = true;
                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        Qty108 = Qty;
                                        Qty108W = Qty1;
                                        fx108 = false;
                                    }
                                    else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsNew.Tables[0].Rows[ids]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                                    {
                                        Qty120 = Qty;
                                        Qty120W = Qty1;
                                        fx120 = true;
                                    }


                                }
                            }
                        }



                        dr["SKU"] = dsGrid.Tables[0].Rows[i]["SKU"].ToString();
                        dr["UPC"] = dsGrid.Tables[0].Rows[i]["UPC"].ToString();
                        dr["Name"] = dsGrid.Tables[0].Rows[i]["Name"].ToString();
                        dr["Inventory"] = dsGrid.Tables[0].Rows[i]["Inventory"].ToString();
                        dr["IsHamming"] = dsGrid.Tables[0].Rows[i]["IsHamming"].ToString();
                        dr["ProductInventoryID"] = dsGrid.Tables[0].Rows[i]["ProductInventoryID"].ToString();
                        dr["ProductID"] = dsGrid.Tables[0].Rows[i]["ProductID"].ToString();
                        dr["HemmingPercentage"] = dsGrid.Tables[0].Rows[i]["HemmingPercentage"].ToString();
                        dr["HammingSafetyQty"] = dsGrid.Tables[0].Rows[i]["HammingSafetyQty"].ToString();
                        dr["ItemType"] = "parent";
                        dr["salechannel"] = "0";
                        dr["Hpdsite"] = "0";
                        dr["Status"] = dsGrid.Tables[0].Rows[i]["Status"].ToString();
                        dr["discountinue"] = dsGrid.Tables[0].Rows[i]["discountinue"].ToString();
                        dsnewdata.Tables[0].Rows.Add(dr);
                        for (int j = 0; j < dsGridChild.Tables[0].Rows.Count; j++)
                        {
                            dr = dsnewdata.Tables[0].NewRow();
                            dr["SKU"] = dsGridChild.Tables[0].Rows[j]["SKU"].ToString();
                            dr["UPC"] = dsGridChild.Tables[0].Rows[j]["UPC"].ToString();
                            dr["Name"] = "";
                            dr["Inventory"] = dsGridChild.Tables[0].Rows[j]["Inventory"].ToString();
                            dr["IsHamming"] = 0;
                            dr["ProductInventoryID"] = 0;
                            dr["ProductID"] = dsGridChild.Tables[0].Rows[j]["VariantValueID"].ToString();
                            dr["HemmingPercentage"] = dsGridChild.Tables[0].Rows[j]["HemmingPercentage"].ToString();
                            dr["HammingSafetyQty"] = dsGridChild.Tables[0].Rows[j]["AddiHemingQty"].ToString();
                            dr["ItemType"] = "child";

                            if (Qty84W < 0)
                            {
                                Qty84W = 0;
                            }
                            if (Qty84 < 0)
                            {
                                Qty84 = 0;
                            }
                            if (Qty96W < 0)
                            {
                                Qty96W = 0;
                            }
                            if (Qty96 < 0)
                            {
                                Qty96 = 0;
                            }
                            if (Qty108W < 0)
                            {
                                Qty108W = 0;
                            }
                            if (Qty108 < 0)
                            {
                                Qty108 = 0;
                            }
                            if (Qty120W < 0)
                            {
                                Qty120W = 0;
                            }
                            if (Qty120 < 0)
                            {
                                Qty120 = 0;
                            }
                            if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-84") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty84.ToString();
                                dr["Hpdsite"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)")); Qty84W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-96") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty96.ToString();
                                dr["Hpdsite"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)"));// Qty96W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-108") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty108.ToString();
                                dr["Hpdsite"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)"));// Qty108W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-120") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty120.ToString();
                                dr["Hpdsite"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)")); //Qty120W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-108") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") > -1)
                            {
                                dr["salechannel"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',3)"));
                                

                                dr["Hpdsite"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)"));
                                if (dr["salechannel"].ToString() == "0" && dr["Hpdsite"] != null && dr["Hpdsite"].ToString() != "0" && Convert.ToInt32(dr["Hpdsite"].ToString()) > Convert.ToInt32(dsGridChild.Tables[0].Rows[j]["AddiHemingQty"].ToString()))
                                {
                                    dr["salechannel"] = Convert.ToString(Convert.ToInt32(dr["Hpdsite"].ToString()) - Convert.ToInt32(dsGridChild.Tables[0].Rows[j]["AddiHemingQty"].ToString()));
                                }
                            }
                            else
                            {
                                dr["salechannel"] = "";
                                dr["Hpdsite"] = "";
                            }
                            if (j == dsGridChild.Tables[0].Rows.Count - 1)
                            {
                                dr["border"] = "border-bottom:solid 1px #999999 !important;";
                            }
                            else
                            {
                                dr["border"] = "border-bottom:solid 1px #eeeeee !important;";
                            }
                            //dr["salechannel"] = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)"));
                            //dr["Hpdsite"] = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_Temp('" + dsGridChild.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsGridChild.Tables[0].Rows[j]["SKU"].ToString() + "',1)"));
                            dr["Status"] = dsGridChild.Tables[0].Rows[j]["Status"].ToString();
                            dr["discountinue"] = dsGridChild.Tables[0].Rows[j]["discountinue"].ToString();
                            dsnewdata.Tables[0].Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr["SKU"] = dsGrid.Tables[0].Rows[i]["SKU"].ToString();
                        dr["UPC"] = dsGrid.Tables[0].Rows[i]["UPC"].ToString();
                        dr["Name"] = dsGrid.Tables[0].Rows[i]["Name"].ToString();
                        dr["Inventory"] = dsGrid.Tables[0].Rows[i]["Inventory"].ToString();
                        dr["IsHamming"] = dsGrid.Tables[0].Rows[i]["IsHamming"].ToString();
                        dr["ProductInventoryID"] = dsGrid.Tables[0].Rows[i]["ProductInventoryID"].ToString();
                        dr["ProductID"] = dsGrid.Tables[0].Rows[i]["ProductID"].ToString();
                        dr["HemmingPercentage"] = dsGrid.Tables[0].Rows[i]["HemmingPercentage"].ToString();
                        dr["HammingSafetyQty"] = dsGrid.Tables[0].Rows[i]["HammingSafetyQty"].ToString();
                        dr["ItemType"] = dsGrid.Tables[0].Rows[i]["ItemType"].ToString();
                        dr["border"] = "border-bottom:solid 1px #999999 !important;";
                        dr["salechannel"] = "";// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsGrid.Tables[0].Rows[i]["UPC"].ToString() + "','" + dsGrid.Tables[0].Rows[i]["SKU"].ToString() + "',1)"));
                        dr["Hpdsite"] = "";// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_Temp('" + dsGrid.Tables[0].Rows[i]["UPC"].ToString() + "','" + dsGrid.Tables[0].Rows[i]["SKU"].ToString() + "',1)"));
                        dr["Status"] = dsGrid.Tables[0].Rows[i]["Status"].ToString();
                        dr["discountinue"] = dsGrid.Tables[0].Rows[i]["discountinue"].ToString();
                        dsnewdata.Tables[0].Rows.Add(dr);
                    }

                }
            }
            if (dsGrid != null && dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {
                grdoptionmainGroup.DataSource = dsnewdata;
                grdoptionmainGroup.DataBind();
                btnSave.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
                btnSave.Visible = false;
                grdoptionmainGroup.DataSource = null;
                grdoptionmainGroup.DataBind();
            }
        }
        private void FillGridSKUbySearch()
        {

            //DataSet dsGrid = new DataSet();
            //dsGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 1,0,'" + txtSearch.Text.ToString() + "'");
            //strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
            //if (strAllowHeming == 1)
            //{
            //    chkonoff.Checked = true;
            //}
            //else
            //{
            //    chkonoff.Checked = false;
            //}
            //if (dsGrid != null && dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            //{
            //    grdoptionmainGroup.DataSource = dsGrid;
            //    grdoptionmainGroup.DataBind();
            //    btnSave.Visible = true;
            //    btnExport.Visible = true;
            //}
            //else
            //{
            //    grdoptionmainGroup.DataSource = null;
            //    grdoptionmainGroup.DataBind();
            //    btnSave.Visible = false;
            //    btnExport.Visible = false;
            //}
            FillGridSKU();
        }
        protected void grdvaluelisting_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = "Available For Sale (After " + HemmingPercentage.ToString() + "% Global Safety Stock)";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden hdnHemmingPercentage = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnHemmingPercentage");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnAddiHemingQty = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnAddiHemingQty");
                TextBox txtinventoryonHand = (TextBox)e.Row.FindControl("txtinventoryonHand");
                TextBox txthpdwebsite = (TextBox)e.Row.FindControl("txthpdwebsite");
                TextBox txtsaleschannel = (TextBox)e.Row.FindControl("txtsaleschannel");
                TextBox txttotalinventory = (TextBox)e.Row.FindControl("txttotalinventory");
                TextBox txtUPC = (TextBox)e.Row.FindControl("txtUPC");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantValueID");

                Label ltlocation = (Label)e.Row.FindControl("ltlocation");
                Label ltlocationatl = (Label)e.Row.FindControl("ltlocationatl");
                Label ltlocationatlbulk = (Label)e.Row.FindControl("ltlocationatlbulk");

                Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=1 and VariantValueID=" + hdnVariantValueID.Value.ToString() + ""));

                ltlocation.Text = LwId.ToString();

                LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=15 and VariantValueID=" + hdnVariantValueID.Value.ToString() + ""));
                ltlocationatl.Text = LwId.ToString();

                LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=18 and VariantValueID=" + hdnVariantValueID.Value.ToString() + ""));
                ltlocationatlbulk.Text = LwId.ToString();

                Label ltSKU = (Label)e.Row.FindControl("ltSKU");


                if (Qty84W < 0)
                {
                    Qty84W = 0;
                }
                if (Qty84 < 0)
                {
                    Qty84 = 0;
                }
                if (Qty96W < 0)
                {
                    Qty96W = 0;
                }
                if (Qty96 < 0)
                {
                    Qty96 = 0;
                }
                if (Qty108W < 0)
                {
                    Qty108W = 0;
                }
                if (Qty108 < 0)
                {
                    Qty108 = 0;
                }
                if (Qty120W < 0)
                {
                    Qty120W = 0;
                }
                if (Qty120 < 0)
                {
                    Qty120 = 0;
                }
                if (ltSKU.Text.ToString().Trim().IndexOf("-84") > -1)
                {
                    txthpdwebsite.Text = Qty84W.ToString();
                    txtsaleschannel.Text = Qty84.ToString();
                }
                else if (ltSKU.Text.ToString().Trim().IndexOf("-96") > -1)
                {
                    txthpdwebsite.Text = Qty96W.ToString();
                    txtsaleschannel.Text = Qty96.ToString();
                }
                else if (ltSKU.Text.ToString().Trim().IndexOf("-108") > -1)
                {
                    txthpdwebsite.Text = Qty108W.ToString();
                    txtsaleschannel.Text = Qty108.ToString();
                }
                else if (ltSKU.Text.ToString().Trim().IndexOf("-120") > -1)
                {
                    txthpdwebsite.Text = Qty120W.ToString();
                    txtsaleschannel.Text = Qty120.ToString();
                }

                //if (chkallowedflg == 1)
                //{
                if (hdnHemmingPercentage.Value.ToString() != "" && Convert.ToDecimal(hdnHemmingPercentage.Value.ToString()) > Decimal.Zero)
                {
                    Decimal Qty = 0;
                    Qty = (Convert.ToDecimal(txtinventoryonHand.Text.ToString()) * Convert.ToDecimal(hdnHemmingPercentage.Value.ToString())) / Convert.ToDecimal(100);
                    txttotalinventory.Text = String.Format("{0:0}", Math.Floor(Qty));
                    //Qty = Qty - Convert.ToDecimal(hdnAddiHemingQty.Value.ToString());
                    //txthpdwebsite.Text = (Convert.ToInt32(txtinventoryonHand.Text.ToString()) - Convert.ToInt32(hdnAddiHemingQty.Value.ToString())).ToString();
                    //txttotalinventory.Text = String.Format("{0:0}", Qty);
                }
                else
                {
                    txttotalinventory.Text = String.Format("{0:0}", Math.Floor(Convert.ToDecimal(txtinventoryonHand.Text.ToString())));
                }
                //    else
                //    {
                //        Decimal Qty = 0;
                //        Qty = (Convert.ToDecimal(txtinventoryonHand.Text.ToString()) * Convert.ToDecimal(hdnHemmingPercentage.Value.ToString())) / Convert.ToDecimal(100);
                //        txttotalinventory.Text = String.Format("{0:0}", Qty);
                //        Qty = Qty - Convert.ToDecimal(hdnAddiHemingQty.Value.ToString());
                //        txtsaleschannel.Text = String.Format("{0:0}", Qty);
                //    }

                //    //if (hdnAddiHemingQty.Value.ToString() != "")
                //    //{
                //    //    Int32 Qty = 0;
                //    //    Qty = Convert.ToInt32(txtinventoryonHand.Text.ToString()) - Convert.ToInt32(hdnAddiHemingQty.Value.ToString());
                //    //    txthpdwebsite.Text = Qty.ToString();
                //    //}
                //    //else
                //    //{
                //    //    txthpdwebsite.Text = txtinventoryonHand.Text.ToString();
                //    //}

                //}
                //else
                //{
                //    if (hdnAddiHemingQty.Value.ToString() != "")
                //    {
                //        Int32 Qty = 0;
                //        Qty = Convert.ToInt32(txtinventoryonHand.Text.ToString()) - Convert.ToInt32(hdnAddiHemingQty.Value.ToString());
                //        txthpdwebsite.Text = Qty.ToString();
                //    }
                //    else
                //    {
                //        txthpdwebsite.Text = txtinventoryonHand.Text.ToString();
                //    }

                //}
                txtUPC.Attributes.Add("readonly", "true");
                txthpdwebsite.Attributes.Add("readonly", "true");
                txtsaleschannel.Attributes.Add("readonly", "true");
                txtinventoryonHand.Attributes.Add("readonly", "true");
                txttotalinventory.Attributes.Add("readonly", "true");


            }
        }
        protected void grdoptionmainGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //GridView grd = (GridView)e.Row.FindControl("grdvaluelisting");
                //if (grd != null)
                {
                    DataSet dsGrid = new DataSet();

                    CheckBox chkallowed = (CheckBox)e.Row.FindControl("chkallowed");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnchkallowed = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnchkallowed");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnProductID = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnProductID");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnHemmingPercentage = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnHemmingPercentage");

                    System.Web.UI.HtmlControls.HtmlInputHidden hdntype = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdntype");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnInventory = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnInventory");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnHemmingqty = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnHemmingqty");

                    TextBox txtinventoryonHand = (TextBox)e.Row.FindControl("txtinventoryonHand");
                    TextBox txtUPC = (TextBox)e.Row.FindControl("txtUPC");
                    TextBox txtsaleschannelsw = (TextBox)e.Row.FindControl("txtsaleschannelsw");
                    TextBox txthpdwebsitesw = (TextBox)e.Row.FindControl("txthpdwebsitesw");
                    TextBox txtsafetyHandsw = (TextBox)e.Row.FindControl("txtsafetyHandsw");
                    Label ltlocation = (Label)e.Row.FindControl("ltlocation");
                    Label ltlocationatl = (Label)e.Row.FindControl("ltlocationatl");
                    Label ltlocationatlbulk = (Label)e.Row.FindControl("ltlocationatlbulk");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnBorder = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnBorder");

                    if (hdntype.Value.ToString().ToLower() == "swatch" || hdntype.Value.ToString().ToLower() == "bedding" || (hdntype.Value.ToString().ToLower() == "drape" && txthpdwebsitesw.Text == ""))
                    {

                        Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=1 and ProductId=" + hdnProductID.Value.ToString() + ""));
                        ltlocation.Text = LwId.ToString();
                        LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=15 and ProductId=" + hdnProductID.Value.ToString() + ""));
                        ltlocationatl.Text = LwId.ToString();
                        LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=18 and ProductId=" + hdnProductID.Value.ToString() + ""));
                        ltlocationatlbulk.Text = LwId.ToString();

                    }
                    else if (hdntype.Value.ToString().ToLower() == "child")
                    {

                        Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=1 and VariantValueID=" + hdnProductID.Value.ToString() + ""));
                        ltlocation.Text = LwId.ToString();
                        LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=15 and VariantValueID=" + hdnProductID.Value.ToString() + ""));
                        ltlocationatl.Text = LwId.ToString();
                        LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=18 and VariantValueID=" + hdnProductID.Value.ToString() + ""));
                        ltlocationatlbulk.Text = LwId.ToString();

                    }
                    else
                    {
                        ltlocation.Text = "";
                        ltlocationatl.Text = "";
                        ltlocationatlbulk.Text = "";
                    }
                    Label ltSKU = (Label)e.Row.FindControl("ltSKU");

                    if (hdntype.Value.ToString().ToLower() == "parent")
                    {
                        strparent = ltSKU.Text.ToString();
                        txtUPC.Text = "";
                    }
                    if (hdntype.Value.ToString().ToLower() == "parent")
                    {
                        strparent = ltSKU.Text.ToString();
                    }
                    if (!string.IsNullOrEmpty(hdnBorder.Value.ToString()))
                    {
                        e.Row.Attributes.Add("style", hdnBorder.Value.ToString());
                    }
                    if (hdnHemmingPercentage.Value.ToString().Trim() != "")
                    {
                        HemmingPercentage = Convert.ToInt32(hdnHemmingPercentage.Value.ToString());
                    }
                    ProductID = Convert.ToInt32(hdnProductID.Value.ToString());
                    //dsGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 2," + ProductID.ToString() + ",'" + txtSearch.Text.ToString() + "'");
                    //dsNew = dsGrid;
                    //Qty84 = 0;
                    //Qty96 = 0;
                    //Qty108 = 0;
                    //Qty120 = 0;
                    //Qty84F = 0;
                    //Qty96F = 0;
                    //Qty108F = 0;
                    //Qty120F = 0;

                    //Qty84W = 0;
                    //Qty96W = 0;
                    //Qty108W = 0;
                    //Qty120W = 0;
                    //Qty84FW = 0;
                    //Qty96FW = 0;
                    //Qty108FW = 0;
                    //Qty120FW = 0;

                    if (hdntype.Value.ToString().ToLower() == "swatch" || hdntype.Value.ToString().ToLower() == "bedding" || (hdntype.Value.ToString().ToLower() == "drape" && txthpdwebsitesw.Text == ""))
                    {
                        if (chkonoff.Checked)
                        {
                            if ((hdnchkallowed.Value.ToString() == "1" || hdnchkallowed.Value.ToString().Trim().ToLower() == "true"))
                            {
                                //  txtsaleschannelsw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                // txthpdwebsitesw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));

                                Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                // Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }
                                txtsaleschannelsw.Text = sI.ToString();
                                txthpdwebsitesw.Text = Wi.ToString();

                            }
                            else
                            {
                                // txtsaleschannelsw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                // txthpdwebsitesw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()));

                                Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }
                                txtsaleschannelsw.Text = sI.ToString();
                                txthpdwebsitesw.Text = Wi.ToString();
                            }

                        }
                        else
                        {
                            //txtsaleschannelsw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                            //txthpdwebsitesw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()));
                            Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                            Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                            if (sI < 0)
                            {
                                sI = 0;
                            }
                            if (Wi < 0)
                            {
                                Wi = 0;
                            }
                            txtsaleschannelsw.Text = sI.ToString();
                            txthpdwebsitesw.Text = Wi.ToString();
                        }


                    }
                    else if (hdntype.Value.ToString().ToLower() == "child" && txthpdwebsitesw.Text.ToString() == "")
                    {
                        if (chkonoff.Checked)
                        {
                            if ((hdnchkallowed.Value.ToString() == "1" || hdnchkallowed.Value.ToString().Trim().ToLower() == "true"))
                            {

                                Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                //Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }
                                txtsaleschannelsw.Text = sI.ToString();
                                txthpdwebsitesw.Text = Wi.ToString();
                            }
                            else
                            {

                                Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }
                                txtsaleschannelsw.Text = sI.ToString();
                                txthpdwebsitesw.Text = Wi.ToString();

                                //txtsaleschannelsw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                                //txthpdwebsitesw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()));
                            }
                        }
                        else
                        {
                            //txtsaleschannelsw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                            //txthpdwebsitesw.Text = Convert.ToString(Convert.ToInt32(hdnInventory.Value.ToString()));

                            Int32 sI = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()) - Convert.ToInt32(hdnHemmingqty.Value.ToString()));
                            Int32 Wi = Convert.ToInt32(Convert.ToInt32(hdnInventory.Value.ToString()));
                            if (sI < 0)
                            {
                                sI = 0;
                            }
                            if (Wi < 0)
                            {
                                Wi = 0;
                            }
                            txtsaleschannelsw.Text = sI.ToString();
                            txthpdwebsitesw.Text = Wi.ToString();
                        }



                    }
                    else if (hdntype.Value.ToString().ToLower() == "child")
                    {
                        //chkallowed.Attributes.Add("class", strparent.ToString());
                        ltSKU.Attributes.Add("class", strparent.ToString());
                        txtsafetyHandsw.Attributes.Add("onkeyup", "AllowandlockQtyVariantChk('" + iskyon.ToString() + "');");

                    }

                    else
                    {
                        //txthpdwebsitesw.Text = "0";
                        //txtsaleschannelsw.Text = "0";
                        txthpdwebsitesw.Attributes.Add("style", "display:none;");
                        txtsaleschannelsw.Attributes.Add("style", "display:none;");
                        txtsafetyHandsw.Attributes.Add("style", "display:none;");
                        iskyon = chkallowed.ClientID.ToString();

                    }


                    if ((hdnchkallowed.Value.ToString() == "1" || hdnchkallowed.Value.ToString().Trim().ToLower() == "true"))
                    {
                        chkallowed.Checked = true;
                        chkallowedflg = 1;

                    }
                    else
                    {
                        chkallowed.Checked = false;
                        chkallowedflg = 0;
                    }
                    if (strAllowHeming == 0)
                    {
                        chkallowed.Attributes.Add("style", "display:none;");
                    }
                    else if (hdntype.Value.ToString().ToLower() == "child")
                    {
                        chkallowed.Attributes.Add("style", "display:none;");
                    }



                    //if (chkallowed.Checked == true && strAllowHeming == 1)
                    //{
                    //    if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    //    {
                    //        Int32 tempQty84 = 0;
                    //        Int32 tempQty96 = 0;
                    //        Int32 tempQty108 = 0;
                    //        Int32 tempQty120 = 0;


                    //        Int32 tempQty84W = 0;
                    //        Int32 tempQty96W = 0;
                    //        Int32 tempQty108W = 0;
                    //        Int32 tempQty120W = 0;

                    //        Int32 StoreQty = 0;

                    //        for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                    //        {
                    //            //double Qty = Convert.ToDouble(); 
                    //            Int32 Qty = 0;
                    //            Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(hdnHemmingPercentage.Value) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                    //            StoreQty = Qty;
                    //            Int32 Qty1 = 0;
                    //            if (chkonoff.Checked)
                    //            {
                    //                Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString()) - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());

                    //            }
                    //            else
                    //            {
                    //                Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                    //            }



                    //            Qty = Qty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //            if (Qty1 < 0)
                    //            {
                    //                Qty1 = 0;
                    //            }
                    //            if (Qty < 0)
                    //            {
                    //                Qty = 0;
                    //            }
                    //            //}
                    //            //else
                    //            //{
                    //            //    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());

                    //            //    //Qty = Qty;
                    //            //}

                    //            //if (Convert.ToInt32(Qty) >= Quantity)
                    //            //{

                    //            if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1)
                    //            {

                    //                tempQty84 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //                if (chkonoff.Checked)
                    //                {
                    //                    tempQty84W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //                }
                    //                else
                    //                {
                    //                    tempQty84W = StoreQty;
                    //                }
                    //                Qty84 = Qty + Qty96;
                    //                Qty84F = Qty;

                    //                Qty84W = Qty1 + Qty96W;
                    //                Qty84FW = Qty1;


                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1)
                    //            {
                    //                tempQty96 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());

                    //                if (chkonoff.Checked)
                    //                {
                    //                    tempQty96W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //                }
                    //                else
                    //                {
                    //                    tempQty96W = StoreQty;
                    //                }

                    //                Qty96 = Qty + Qty108;
                    //                Qty96F = Qty;
                    //                Qty84 = Qty84F + Qty96;


                    //                Qty96W = Qty1 + Qty108W;
                    //                Qty96FW = Qty1;
                    //                Qty84W = Qty84FW + Qty96W;

                    //                //Qty84 = Qty84
                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1)
                    //            {
                    //                tempQty108 = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //                if (chkonoff.Checked)
                    //                {
                    //                    tempQty108W = StoreQty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //                }
                    //                else
                    //                {
                    //                    tempQty108W = StoreQty;
                    //                }
                    //                Qty108 = Qty + Qty120;
                    //                Qty108F = Qty;
                    //                Qty96 = Qty96F + Qty108;
                    //                Qty84 = Qty84F + Qty96;

                    //                Qty108W = Qty1 + Qty120W;
                    //                Qty108FW = Qty1;
                    //                Qty96W = Qty96FW + Qty108W;
                    //                Qty84W = Qty84FW + Qty96W;
                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1)
                    //            {

                    //                Qty120 = Qty;
                    //                Qty120F = Qty;
                    //                Qty108 = Qty108F + Qty120;
                    //                Qty96 = Qty96F + Qty108;
                    //                Qty84 = Qty84F + Qty96;

                    //                Qty120W = Qty1;
                    //                Qty120FW = Qty1;
                    //                Qty108W = Qty108FW + Qty120W;
                    //                Qty96W = Qty96FW + Qty108W;
                    //                Qty84W = Qty84FW + Qty96W;
                    //            }




                    //        }
                    //        if (tempQty84 < 0)
                    //        {
                    //            if (Qty84 > (-(tempQty84)))
                    //            {
                    //                Qty84 = Qty84 + tempQty84;

                    //            }
                    //            else
                    //            {
                    //                Qty84 = 0;

                    //            }
                    //        }
                    //        if (tempQty96 < 0)
                    //        {
                    //            if (Qty96 > (-(tempQty96)))
                    //            {
                    //                Qty96 = Qty96 + tempQty96;

                    //            }
                    //            else
                    //            {
                    //                Qty96 = 0;

                    //            }
                    //        }
                    //        if (tempQty108 < 0)
                    //        {
                    //            if (Qty108 > (-(tempQty108)))
                    //            {
                    //                Qty108 = Qty108 + tempQty108;

                    //            }
                    //            else
                    //            {
                    //                Qty108 = 0;

                    //            }
                    //        }
                    //        if (tempQty84W < 0)
                    //        {
                    //            if (Qty84W > (-(tempQty84W)))
                    //            {
                    //                Qty84W = Qty84W + tempQty84W;

                    //            }
                    //            else
                    //            {

                    //                Qty84W = 0;
                    //            }
                    //        }
                    //        if (tempQty96W < 0)
                    //        {
                    //            if (Qty96W > (-(tempQty96W)))
                    //            {
                    //                Qty96W = Qty96W + tempQty96W;

                    //            }
                    //            else
                    //            {

                    //                Qty96W = 0;
                    //            }
                    //        }
                    //        if (tempQty108W < 0)
                    //        {
                    //            if (Qty108W > (-(tempQty108W)))
                    //            {
                    //                Qty108W = Qty108W + tempQty108W;

                    //            }
                    //            else
                    //            {

                    //                Qty108W = 0;
                    //            }
                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                    //    {
                    //        for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                    //        {
                    //            //double Qty = Convert.ToDouble(); 
                    //            Int32 Qty = 0;
                    //            Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(hdnHemmingPercentage.Value) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                    //            Int32 Qty1 = 0;
                    //            //if ((hdnchkallowed.Value.ToString() == "1" || hdnchkallowed.Value.ToString().Trim().ToLower() == "true"))
                    //            //{
                    //            Qty = Qty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //            if (chkonoff.Checked)
                    //            {
                    //                Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString()) - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                    //            }
                    //            else
                    //            {
                    //                Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                    //            }
                    //            //}
                    //            //else
                    //            //{
                    //            //    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                    //            //}
                    //            //if (Convert.ToInt32(Qty) >= Quantity)
                    //            //{

                    //            if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1)
                    //            {
                    //                Qty84 = Qty;
                    //                Qty84W = Qty1;
                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1)
                    //            {
                    //                Qty96 = Qty;
                    //                Qty96W = Qty1;
                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1)
                    //            {
                    //                Qty108 = Qty;
                    //                Qty108W = Qty1;
                    //            }
                    //            else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1)
                    //            {
                    //                Qty120 = Qty;
                    //                Qty120W = Qty1;
                    //            }

                    //        }
                    //    }
                    //}
                    //if (dsGrid != null && dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
                    //{
                    //    txtUPC.Text = "";
                    //    grd.DataSource = dsGrid;
                    //    grd.DataBind();
                    //}
                    //else
                    //{
                    //    grd.DataSource = null;
                    //    grd.DataBind();
                    //}
                    txtUPC.Attributes.Add("readonly", "true");
                    txtinventoryonHand.Attributes.Add("readonly", "true");
                }
            }
        }
        protected void grdoptionmainGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdoptionmainGroup.PageIndex = e.NewPageIndex;
            if (txtSearch.Text.ToString() != "")
            {
                FillGridSKUbySearch();
            }
            else
            {
                FillGridSKU();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hdnallsku.Value = "";
            hdnhemmingstatus.Value = "";
            hdnonoffstatus.Value = "";
            grdoptionmainGroup.PageIndex = 0;
            FillGridSKUbySearch();
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow grd in grdoptionmainGroup.Rows)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden hdnProductID = (System.Web.UI.HtmlControls.HtmlInputHidden)grd.FindControl("hdnProductID");
                CheckBox chkallowed = (CheckBox)grd.FindControl("chkallowed");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnProductInventoryID = (System.Web.UI.HtmlControls.HtmlInputHidden)grd.FindControl("hdnProductInventoryID");
                TextBox txtinventoryonHand = (TextBox)grd.FindControl("txtinventoryonHand");

                TextBox txtsafetyHandsw = (TextBox)grd.FindControl("txtsafetyHandsw");

                System.Web.UI.HtmlControls.HtmlInputHidden hdntype = (System.Web.UI.HtmlControls.HtmlInputHidden)grd.FindControl("hdntype");
                GridView grdd = (GridView)grd.FindControl("grdvaluelisting");
                Int32 chkAll = 0;
                if (chkallowed.Checked == true)
                {
                    chkAll = 1;
                }

                if (hdntype.Value.ToString().ToLower() == "swatch")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_product SET IsHamming=" + chkAll + ",HammingSafetyQty=" + txtsafetyHandsw.Text.ToString() + " WHERE ProductId=" + hdnProductID.Value.ToString() + "");
                }
                else if (hdntype.Value.ToString().ToLower() == "child")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET AddiHemingQty=" + txtsafetyHandsw.Text.ToString() + " WHERE VariantValueID=" + hdnProductID.Value.ToString() + "");
                }
                else if (hdntype.Value.ToString().ToLower() == "parent")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_product SET IsHamming=" + chkAll + " WHERE ProductId=" + hdnProductID.Value.ToString() + "");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_product SET IsHamming=" + chkAll + ",HammingSafetyQty=" + txtsafetyHandsw.Text.ToString() + " WHERE ProductId=" + hdnProductID.Value.ToString() + "");
                }
                //CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + txtinventoryonHand.Text.ToString() + "  WHERE ProductInventoryID=" + hdnProductInventoryID.Value.ToString() + "");

                //foreach (GridViewRow grr in grdd.Rows)
                //{
                //    System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)grr.FindControl("hdnVariantValueID");
                //    System.Web.UI.HtmlControls.HtmlInputHidden hdnProductVariantInventoryID = (System.Web.UI.HtmlControls.HtmlInputHidden)grr.FindControl("hdnProductVariantInventoryID");
                //    TextBox txtinventoryonHand1 = (TextBox)grr.FindControl("txtinventoryonHand");
                //    TextBox txtsafetyHand = (TextBox)grr.FindControl("txtsafetyHand");
                //    CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET AddiHemingQty=" + txtsafetyHand.Text.ToString() + " WHERE VariantValueID=" + hdnVariantValueID.Value.ToString() + "");
                //    //CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + txtinventoryonHand1.Text.ToString() + " WHERE ProductVariantInventoryID=" + hdnProductVariantInventoryID.Value.ToString() + "");
                //}
            }
            if (chkonoff.Checked)
            {


                CommonComponent.ExecuteCommonData("UPDATE tb_Appconfig SET ConfigValue='1' WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1");
                //string[] arrsku = hdnallsku.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //string[] arrskustatus = hdnhemmingstatus.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //string strnewSku = ",";
                //string strnewqty = ",";
                //for (int j = 0; j < arrsku.Length; j++)
                //{
                //    if (strnewSku.IndexOf("," + arrsku[j].ToString() + ",") <= -1)
                //    {
                //        strnewSku += arrsku[j].ToString() + ",";
                //        strnewqty += arrskustatus[j].ToString() + ",";
                //    }

                //}
                //arrsku = strnewSku.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //arrskustatus = strnewqty.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string[] arrsku = hdnallsku.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] arrskustatus = hdnhemmingstatus.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] arrskustatusreminder = hdnonoffstatus.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string strnewSku = ",";
                string strnewqty = ",";
                string arrskustatusreminder1 = ",";
                for (int j = 0; j < arrsku.Length; j++)
                {
                    if (strnewSku.IndexOf("," + arrsku[j].ToString() + ",") <= -1)
                    {
                        strnewSku += arrsku[j].ToString() + ",";
                        strnewqty += arrskustatus[j].ToString() + ",";
                        arrskustatusreminder1 += arrskustatusreminder[j].ToString() + ",";
                    }

                }
                arrsku = strnewSku.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                arrskustatus = strnewqty.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                arrskustatusreminder = arrskustatusreminder1.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (ViewState["hon"] != null && ViewState["hon"].ToString() != "1")
                {
                    ViewState["hon"] = "1";
                    string strall = "";
                    for (int j = 0; j < arrsku.Length; j++)
                    {
                        if (arrskustatusreminder[j].ToString().ToLower().Trim() != arrskustatus[j].ToString().ToLower().Trim())
                        {
                            strall += arrsku[j].ToString() + ",";
                        }
                    }
                    CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'" + strall.ToString() + "','',getdate(),'" + Request.UserHostAddress.ToString() + "','ON')");
                }

                if (arrsku.Length > 0 && arrskustatus.Length > 0 && arrsku.Length == arrskustatus.Length)
                {

                    for (int j = 0; j < arrsku.Length; j++)
                    {
                        if (arrskustatusreminder[j].ToString().ToLower().Trim() != arrskustatus[j].ToString().ToLower().Trim())
                        {
                            CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'" + arrsku[j].ToString() + "','" + arrskustatus[j].ToString() + "',getdate(),'" + Request.UserHostAddress.ToString() + "','ON')");
                        }
                    }
                }



                hdnallsku.Value = "";
                hdnhemmingstatus.Value = "";
                hdnonoffstatus.Value = "";
            }
            else
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_Appconfig SET ConfigValue='0' WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1");
                string[] arrsku = hdnallsku.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] arrskustatus = hdnhemmingstatus.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] arrskustatusreminder = hdnonoffstatus.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string strnewSku = ",";
                string strnewqty = ",";
                string arrskustatusreminder1 = ",";
                for (int j = 0; j < arrsku.Length; j++)
                {
                    if (strnewSku.IndexOf("," + arrsku[j].ToString() + ",") <= -1)
                    {
                        strnewSku += arrsku[j].ToString() + ",";
                        strnewqty += arrskustatus[j].ToString() + ",";
                        arrskustatusreminder1 += arrskustatusreminder[j].ToString() + ",";
                    }

                }
                arrsku = strnewSku.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                arrskustatus = strnewqty.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                arrskustatusreminder = arrskustatusreminder1.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (ViewState["hon"] != null && ViewState["hon"].ToString() != "0")
                {
                    ViewState["hon"] = "0";
                    string strall = "";
                    for (int j = 0; j < arrsku.Length; j++)
                    {
                        if (arrskustatusreminder[j].ToString().ToLower().Trim() != arrskustatus[j].ToString().ToLower().Trim())
                        {
                            strall += arrsku[j].ToString() + ",";
                        }
                    }
                    CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'" + strall.ToString() + "','',getdate(),'" + Request.UserHostAddress.ToString() + "','OFF')");
                }
                if (arrsku.Length > 0 && arrskustatus.Length > 0 && arrsku.Length == arrskustatus.Length)
                {
                    for (int j = 0; j < arrsku.Length; j++)
                    {
                        if (arrskustatusreminder[j].ToString().ToLower().Trim() != arrskustatus[j].ToString().ToLower().Trim())
                        {
                            CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'" + arrsku[j].ToString() + "','" + arrskustatus[j].ToString() + "',getdate(),'" + Request.UserHostAddress.ToString() + "','OFF')");
                        }
                    }
                        
                }


                hdnallsku.Value = "";
                hdnhemmingstatus.Value = "";
                hdnonoffstatus.Value = "";
            }
            //SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1

            if (chkonoff.Checked)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsave", "jAlert('Data Updated Successfully!','Success');MakeCheckedall('true','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsave", "jAlert('Data Updated Successfully!','Success');MakeCheckedall('false','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
            }
            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsuccess", "", true);
            if (txtSearch.Text.ToString() != "" && btnSave.Visible == true)
            {
                FillGridSKUbySearch();
            }
            else
            {
                if (btnSave.Visible == true)
                {
                    FillGridSKU();
                }
            }
        }
        protected void btnSavetemp_Click(object sender, ImageClickEventArgs e)
        {

            if (chkonoff.Checked)
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_Appconfig SET ConfigValue='1' WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1");
                if (ViewState["hon"] != null && ViewState["hon"].ToString() != "1")
                {
                    ViewState["hon"] = "1";
                    CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'','',getdate(),'" + Request.UserHostAddress.ToString() + "','ON')");
                }
            }
            else
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_Appconfig SET ConfigValue='0' WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1");
                if (ViewState["hon"] != null && ViewState["hon"].ToString() != "0")
                {
                    ViewState["hon"] = "0";
                    CommonComponent.ExecuteCommonData("insert into tb_skuhemminglog(AdminID,SKU,Hemming,CreatedOn,IPAddress,Globalhemming) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",'','',getdate(),'" + Request.UserHostAddress.ToString() + "','OFF')");
                }
            }

            if (txtSearch.Text.ToString() != "" && btnSave.Visible == true)
            {
                FillGridSKUbySearch();
            }
            else
            {
                if (btnSave.Visible == true)
                {
                    FillGridSKU();
                }
            }
            //SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1

            if (chkonoff.Checked)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsave", "MakeCheckedall('true','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsave", "MakeCheckedall('false','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
            }


        }
        private void WriteFile(String Text, string FileName)
        {

            StreamWriter writer = null;
            try
            {


                FileInfo info = new FileInfo(FileName);
                writer = info.AppendText();
                writer.Write(Text);

                if (writer != null)
                    writer.Close();
            }
            catch
            {
                if (writer != null)
                    writer.Close();
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                //string attachment = "attachment; filename=ProductHemminglist_Export.xls";
                //Response.ClearContent();
                //// Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=ProductHemminglist_Export.xls");
                //Response.ContentType = "application/ms-excel";
                //StringWriter stw = new StringWriter();
                //Response.Write(divhemming.InnerText.ToString());
                //Response.Flush();
                //Response.End();

                bool checkchild = false;
                foreach (GridViewRow gr in grdoptionmainGroup.Rows)
                {
                    object[] args = new object[12];
                    args[0] = ((Label)gr.FindControl("ltSKU")).Text.ToString();
                    args[1] = ((TextBox)gr.FindControl("txtUPC")).Text.ToString();
                    args[2] = ((TextBox)gr.FindControl("txtinventoryonHand")).Text.ToString();
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnProductID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdnProductID");

                    Label ltlocation = (Label)gr.FindControl("ltlocation");
                    Label ltlocationatl = (Label)gr.FindControl("ltlocationatl");
                    Label lblstatus = (Label)gr.FindControl("lblstatus");
                    Label ltlocationatlbulk = (Label)gr.FindControl("ltlocationatlbulk");

                    System.Web.UI.HtmlControls.HtmlInputHidden hdnType = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdntype");

                    System.Web.UI.HtmlControls.HtmlInputHidden hdncountinue = (System.Web.UI.HtmlControls.HtmlInputHidden)gr.FindControl("hdncountinue");
                    //if (hdnType.Value.ToString().Trim().ToLower() == "swatch")
                    //{
                    // Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=1 and ProductId=" + hdnProductID.Value.ToString() + ""));

                    args[3] = ltlocation.Text.ToString();

                    // LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=15 and ProductId=" + hdnProductID.Value.ToString() + ""));
                    args[4] = ltlocationatl.Text.ToString();
                    args[5] = ltlocationatlbulk.Text.ToString();
                    args[6] = ((TextBox)gr.FindControl("txtsafetyHandsw")).Text.ToString();
                    args[7] = ((TextBox)gr.FindControl("txtsaleschannelsw")).Text.ToString();
                    args[8] = ((TextBox)gr.FindControl("txthpdwebsitesw")).Text.ToString();
                    //}
                    //else
                    //{
                    //    args[3] = "";
                    //    args[4] = "";
                    //    args[5] = "";
                    //    args[6] = "";
                    //    args[7] = "";
                    //}

                    if (((CheckBox)gr.FindControl("chkallowed")).Checked == true)
                    {
                        args[9] = "Y";
                    }
                    else
                    {
                        args[9] = "N";
                    }

                    if (lblstatus != null)
                    {
                        args[10] = lblstatus.Text.ToString();
                    }
                    else
                    {
                        args[10] = "";
                    }
                    if (AppLogic.AppConfigs("iscustomdiscountinue") != null && AppLogic.AppConfigs("iscustomdiscountinue").ToString() == "1")
                    {
                        string strcust = ((Label)gr.FindControl("ltSKU")).Text.ToString();
                        if (strcust.ToString().ToLower().LastIndexOf("-cus") > -1)
                        {
                            bool iscon = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT isnull(Discontinue,0) FROM tb_ProductFabricCode WHERE isnull(Active,0)=1 and isnull(Code,'')<>'' and Code in (SELECT FabricCode FROM tb_product WHERE ProductId=" + hdnProductID.Value.ToString() + ")"));
                            if (iscon)
                            {
                                args[11] = "Y";
                            }
                            else
                            {
                                args[11] = "N";
                            }
                            
                        }
                        else
                        {
                            args[11] = hdncountinue.Value.ToString();
                        }
                        
                    }
                    else
                    {
                        args[11] = hdncountinue.Value.ToString();
                    }
                    
                    //args[4]
                    sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\"", args));
                    //if (checkchild == false)
                    //{
                    //    sb.AppendLine(" ,SKU,UPC,Inventory On Hand, Location,Safety Lock Qty,Avilable for Sales Partner,Avilable for HPD");

                    //    checkchild = true;
                    //}
                    //GridView grchild = (GridView)gr.FindControl("grdvaluelisting");
                    //if (grchild.Rows.Count > 0)
                    //{
                    //    foreach (GridViewRow gr1 in grchild.Rows)
                    //    {
                    //        object[] args1 = new object[9];

                    //        args1[0] = ((Label)gr1.FindControl("ltSKU")).Text.ToString();
                    //        args1[1] = ((TextBox)gr1.FindControl("txtUPC")).Text.ToString();
                    //        args1[2] = ((TextBox)gr1.FindControl("txtinventoryonHand")).Text.ToString();
                    //        System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantValueID = (System.Web.UI.HtmlControls.HtmlInputHidden)gr1.FindControl("hdnVariantValueID");
                    //        Label ltlocationchild = (Label)gr1.FindControl("ltlocation");
                    //        Label ltlocationatlcild = (Label)gr1.FindControl("ltlocationatl");

                    //        // Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=1 and VariantValueID=" + hdnVariantValueID.Value.ToString() + ""));

                    //        args1[3] = ltlocationchild.Text.ToString();

                    //        // LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=15 and VariantValueID=" + hdnVariantValueID.Value.ToString() + ""));
                    //        args1[4] = ltlocationatlcild.Text.ToString();


                    //        args1[5] = ((TextBox)gr1.FindControl("txtsafetyHand")).Text.ToString();
                    //        args1[6] = ((TextBox)gr1.FindControl("txtsaleschannel")).Text.ToString();
                    //        args1[7] = ((TextBox)gr1.FindControl("txthpdwebsite")).Text.ToString();

                    //        args1[8] = "";
                    //        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"", args1));

                    //    }
                    //}
                }


                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductHemmingList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,UPC,Inventory On Hand, Livermore,Atlanta,Atlanta Bulk,Safety Lock Qty,Avilable For Channel Sales,Avilable For HPD Website,Hemming Allowed,Status,Discontinue");
                    sb.AppendLine(FullString);

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }


            }
            catch
            {

            }

        }
        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            hdnallsku.Value = "";
            hdnhemmingstatus.Value = "";
            hdnonoffstatus.Value = "";
            txtSearch.Text = "";
            FillGridSKU();

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
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/") + StrFileName);
                    //StrFileName = Server.MapPath(AppLogic.AppConfigs("CategoryImportPath") + "CategoryCSV/ImportCSV/") + StrFileName;
                    FillMapping(uploadCSV.FileName);
                }
                else
                {
                    //lblMessage.Text = "Please upload appropriate file.";
                    //lblMessage.Style.Add("color", "#FF0000");
                    //lblMessage.Style.Add("font-weight", "normal");

                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    if (InsertDataInDataBase(dtCSV) && lblMessage.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMessage.Text = "Safety Lock Qty Imported Successfully";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                        lblMessage.Visible = true;
                        //  Response.Redirect("ProductHemmingEdit.aspx", false);
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
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/") + FileName);
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

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        private bool InsertDataInDataBase(DataTable dt)
        {


            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(dt.Rows[i]["Safety Lock Qty"].ToString()))
                    {
                        dt.Rows[i]["Safety Lock Qty"] = 0;
                        dt.AcceptChanges();
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["UPC"].ToString()))
                    {
                        try
                        {
                            Int32 qty = 0;
                            Int32.TryParse(dt.Rows[i]["Safety Lock Qty"].ToString(), out qty);

                            CommonComponent.ExecuteCommonData("update tb_product set HammingSafetyQty=" + qty + " where storeid=1 and SKU = '" + dt.Rows[i]["SKU"].ToString().Trim() + "' and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(deleted,0)=0");
                            CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set AddiHemingQty=" + qty + " where sku='" + dt.Rows[i]["sku"].ToString().Trim() + "' and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and ProductID in (select ProductID from tb_Product where StoreID=1 and isnull(deleted,0)=0)");


                        }
                        catch { }
                    }

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
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportHemmingCSV/") + FileName);
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
                        if (tempFieldName == "sku" || tempFieldName == "safety lock qty" || tempFieldName == "hemming allowed" || tempFieldName == "upc")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",safety lock qty,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",hemming allowed,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1)
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
                        lblMessage.Text = "Please Specify SKU,Safety Lock Qty,Hemming Allowed,UPC in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }

                }
                else
                {
                    lblMessage.Text = "Please Specify SKU,Safety Lock Qty,Hemming Allowed,UPC in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
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
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtpassword.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RequiredPass", "jAlert('Please Enter Password.','Required');", true);
                txtpassword.Focus();
                return;
            }

            string pass = Convert.ToString(CommonComponent.GetScalarCommonData("select configvalue from tb_appconfig where configname='HemmingpagePassword' and storeid=1"));
            if (!string.IsNullOrEmpty(pass))
            {
                if (txtpassword.Text.ToString().Trim() == pass.ToString().Trim())
                {

                    CommonComponent.ExecuteCommonData("insert into tb_hemminglog(AdminID,CreatedOn,IPAddress) values (" + Convert.ToInt32(Session["AdminID"].ToString()) + ",getdate(),'" + Request.UserHostAddress.ToString() + "')");
                    password.Visible = false;
                    divsearch.Visible = true;
                   // btnSave.Visible = true;
                    if (chkonoff.Checked)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgOnOff", "MakeCheckedall('true','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgOnOff", "MakeCheckedall('false','" + chkonoff.ClientID.ToString() + "');Getstatusall('" + chkonoff.ClientID.ToString() + "','" + divshippingtime.ClientID.ToString() + "','" + btnSavetemp.ClientID.ToString() + "','" + string.Empty.TrimEnd(',') + "');", true);
                    }
                }
                else
                {
                    // System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "IncorrectPass", "jAlert('Incorrect Password','Error');", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "IncorrectPass", "jAlert('Incorrect Password','Error');", true);
                }
            }

        }
    }
}