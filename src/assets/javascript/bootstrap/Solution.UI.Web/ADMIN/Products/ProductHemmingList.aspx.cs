using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductHemmingList : BasePage
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

        DataSet dsNew = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.Png) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                // FillGridSKU();
                BindHemmingNew();
                // drphamming.SelectedValue = "0";
            }
        }
        public void BindHemmingNew()
        {
            DataSet dsGrid = new DataSet();
            dsGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 1,0,'" + txtSearch.Text.ToString() + "'");
            Int32 strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
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
                                        fx108 = true;
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
                        dsnewdata.Tables[0].Rows.Add(dr);
                        for (int j = 0; j < dsGridChild.Tables[0].Rows.Count; j++)
                        {
                            dr = dsnewdata.Tables[0].NewRow();
                            dr["SKU"] = dsGridChild.Tables[0].Rows[j]["SKU"].ToString();
                            dr["UPC"] = dsGridChild.Tables[0].Rows[j]["UPC"].ToString();
                            dr["Name"] = "";
                            dr["Inventory"] = dsGridChild.Tables[0].Rows[j]["Inventory"].ToString();
                            dr["IsHamming"] = dsGrid.Tables[0].Rows[i]["IsHamming"].ToString();
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
                                dr["Hpdsite"] = Qty84W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-96") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty96.ToString();
                                dr["Hpdsite"] = Qty96W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-108") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty108.ToString();
                                dr["Hpdsite"] = Qty108W.ToString();
                            }
                            else if (dsGridChild.Tables[0].Rows[j]["SKU"].ToString().IndexOf("-120") > -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-cus") <= -1 && dsGridChild.Tables[0].Rows[j]["SKU"].ToString().ToLower().IndexOf("-63") <= -1)
                            {
                                dr["salechannel"] = Qty120.ToString();
                                dr["Hpdsite"] = Qty120W.ToString();
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
                        dsnewdata.Tables[0].Rows.Add(dr);
                    }

                }
            }
            string withhemming = "";
            string withouthemming = "";
            withhemming += "<table  cellspacing=\"0\" style=\"border-color:#999999;border-width:1px;border-style:Solid;border-collapse:collapse;\" class=\"checklist-main border-right-all\" ><tr><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;SKU</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">UPC</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Inventory On Hand</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;Livermore</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;Atlanta</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Safety Lock Qty</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Avilable for Channel Sales</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Avilable for HPD</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Hemming Allowed</th></tr><tr><td colspan=\"9\" align=\"left\" style=\"font-weight:bold;font-size:14px;\">With Hemming</td></tr>";
            withouthemming += "<table  cellspacing=\"0\" style=\"border-color:#999999;border-width:1px;border-style:Solid;border-collapse:collapse;\" class=\"checklist-main border-right-all\" ><tr><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;SKU</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">UPC</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Inventory On Hand</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;Livermore</th><th valign=\"middle\" align=\"left\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">&nbsp;Atlanta</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Safety Lock Qty</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Avilable for Channel Sales</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Avilable for HPD</th><th valign=\"middle\" align=\"center\" style=\"width:10%;background-color: #EEEEEE;\" scope=\"col\" class=\"table-title\">Hemming Allowed</th></tr><tr><td colspan=\"9\" align=\"left\" style=\"font-weight:bold;font-size:14px;\">Without Hemming</td></tr>";

            if (dsnewdata != null && dsnewdata.Tables.Count > 0 && dsnewdata.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsnewdata.Tables[0].Rows.Count; i++)
                {

                    if (dsnewdata.Tables[0].Rows[i]["IsHamming"].ToString() == "1" || dsnewdata.Tables[0].Rows[i]["IsHamming"].ToString().ToLower() == "true")
                    {

                        if (!string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["border"].ToString()))
                        {


                            withhemming += "<tr style=\"" + dsnewdata.Tables[0].Rows[i]["border"].ToString() + "\">";
                        }
                        else
                        {
                            withhemming += "<tr>";
                        }


                        withhemming += "<td align=\"left\" style=\"width:10%;\">";
                        withhemming += "<span>" + dsnewdata.Tables[0].Rows[i]["SKU"].ToString() + " </span>";
                        withhemming += "</td>";


                        withhemming += "<td align=\"left\" style=\"width:10%;\">";
                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "parent")
                        {
                            withhemming += "<span></span>";
                        }
                        else
                        {
                            withhemming += "<span>" + dsnewdata.Tables[0].Rows[i]["UPC"].ToString() + " </span>";
                        }

                        withhemming += "</td>";

                        withhemming += "<td align=\"center\" style=\"width:10%;\">";
                        withhemming += "<span>" + dsnewdata.Tables[0].Rows[i]["Inventory"].ToString() + " </span>";
                        withhemming += "</td>";


                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch")
                        {
                            Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=1 and ProductId=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span>" + LwId.ToString() + " </span>";
                            withhemming += "</td>";
                            LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=15 and ProductId=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span>" + LwId.ToString() + " </span>";
                            withhemming += "</td>";


                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child")
                        {
                            Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=1 and VariantValueID=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span>" + LwId.ToString() + " </span>";
                            withhemming += "</td>";
                            LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=15 and VariantValueID=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span>" + LwId.ToString() + " </span>";
                            withhemming += "</td>";
                        }
                        else
                        {
                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";

                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";
                        }


                        //withhemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                        //withhemming += "<span></span>";
                        //withhemming += "</td>";

                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch")
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString() + " </span>";
                            withhemming += "</td>";
                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child")
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString() + " </span>";
                            withhemming += "</td>";
                        }
                        else
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";
                        }





                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch" || (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "drape" && dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString() == ""))
                        {
                            if (chkonoff.Checked)
                            {

                                Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }

                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + sI.ToString() + " </span>";
                                withhemming += "</td>";
                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + Wi.ToString() + " </span>";
                                withhemming += "</td>";



                            }
                            else
                            {
                                Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }

                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + sI.ToString() + " </span>";
                                withhemming += "</td>";
                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + Wi.ToString() + " </span>";
                                withhemming += "</td>";
                            }


                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child" && dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString() == "")
                        {
                            if (chkonoff.Checked)
                            {


                                Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }

                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + sI.ToString() + " </span>";
                                withhemming += "</td>";
                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + Wi.ToString() + " </span>";
                                withhemming += "</td>";

                            }
                            else
                            {


                                Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                                Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()));
                                if (sI < 0)
                                {
                                    sI = 0;
                                }
                                if (Wi < 0)
                                {
                                    Wi = 0;
                                }

                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + sI.ToString() + " </span>";
                                withhemming += "</td>";
                                withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                withhemming += "<span>" + Wi.ToString() + " </span>";
                                withhemming += "</td>";

                            }



                        }
                        else
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + Convert.ToString(Convert.ToInt32(string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["salechannel"].ToString()) ? "0" : dsnewdata.Tables[0].Rows[i]["salechannel"].ToString())) + " </span>";
                            withhemming += "</td>";

                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + Convert.ToString(Convert.ToInt32(string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString()) ? "0" : dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString())) + " </span>";
                            withhemming += "</td>";
                        }






                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch" || dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "parent")
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>Y</span>";
                            withhemming += "</td>";
                        }
                        else
                        {
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";
                        }



                        withhemming += "</tr>";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["border"].ToString()))
                        {


                            withouthemming += "<tr style=\"" + dsnewdata.Tables[0].Rows[i]["border"].ToString() + "\">";
                        }
                        else
                        {
                            withouthemming += "<tr>";
                        }

                        withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                        withouthemming += "<span>" + dsnewdata.Tables[0].Rows[i]["SKU"].ToString() + " </span>";
                        withouthemming += "</td>";


                        withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "parent")
                        {

                            withouthemming += "<span></span>";
                        }
                        else
                        {
                            withouthemming += "<span>" + dsnewdata.Tables[0].Rows[i]["UPC"].ToString() + " </span>";
                        }
                        withouthemming += "</td>";

                        withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                        withouthemming += "<span>" + dsnewdata.Tables[0].Rows[i]["Inventory"].ToString() + " </span>";
                        withouthemming += "</td>";

                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch")
                        {
                            Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=1 and ProductId=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span>" + LwId.ToString() + " </span>";
                            withouthemming += "</td>";
                            LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductInventory WHERE WareHouseID=15 and ProductId=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span>" + LwId.ToString() + " </span>";
                            withouthemming += "</td>";


                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child")
                        {
                            Int32 LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=1 and VariantValueID=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span>" + LwId.ToString() + " </span>";
                            withouthemming += "</td>";
                            LwId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT isnull(Inventory,0) FROM tb_WareHouseProductVariantInventory WHERE WareHouseID=15 and VariantValueID=" + dsnewdata.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span>" + LwId.ToString() + " </span>";
                            withouthemming += "</td>";
                        }
                        else
                        {
                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";
                        }


                        //withouthemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                        //withouthemming += "<span> </span>";
                        //withouthemming += "</td>";

                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch")
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString() + " </span>";
                            withouthemming += "</td>";
                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child")
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString() + " </span>";
                            withouthemming += "</td>";
                        }
                        else
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";
                        }


                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch")
                        {
                            Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                            Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()));
                            if (sI < 0)
                            {
                                sI = 0;
                            }
                            if (Wi < 0)
                            {
                                Wi = 0;
                            }



                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + sI.ToString() + " </span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + Wi.ToString() + " </span>";
                            withouthemming += "</td>";

                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "child" && dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString() == "")
                        {
                            Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                            Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()));
                            if (sI < 0)
                            {
                                sI = 0;
                            }
                            if (Wi < 0)
                            {
                                Wi = 0;
                            }



                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + sI.ToString() + " </span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + Wi.ToString() + " </span>";
                            withouthemming += "</td>";



                        }
                        else if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "drape" && dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString() == "")
                        {
                            Int32 sI = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()) - Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["HammingSafetyQty"].ToString()));
                            Int32 Wi = Convert.ToInt32(Convert.ToInt32(dsnewdata.Tables[0].Rows[i]["Inventory"].ToString()));
                            if (sI < 0)
                            {
                                sI = 0;
                            }
                            if (Wi < 0)
                            {
                                Wi = 0;
                            }



                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + sI.ToString() + " </span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + Wi.ToString() + " </span>";
                            withouthemming += "</td>";



                        }
                        else
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + Convert.ToString(Convert.ToInt32(string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["salechannel"].ToString()) ? "0" : dsnewdata.Tables[0].Rows[i]["salechannel"].ToString())) + " </span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + Convert.ToString(Convert.ToInt32(string.IsNullOrEmpty(dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString()) ? "0" : dsnewdata.Tables[0].Rows[i]["Hpdsite"].ToString())) + " </span>";
                            withouthemming += "</td>";
                        }






                        if (dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "swatch" || dsnewdata.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "parent")
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>N</span>";
                            withouthemming += "</td>";
                        }
                        else
                        {
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";
                        }

                        withouthemming += "</tr>";
                    }

                }
            }
            withhemming += "</table>";
            withouthemming += "</table>";
            withhemming += withouthemming;
            lthamming.Text = withhemming;

        }
        public void Bindhemming()
        {
            lthamming.Text = "";
            int iproductid = 0;
            int jHemmingPercentage = 0;
            decimal totalinventory = decimal.Zero;
            int hpdwebsite = 0;
            int saleschannel = 0;
            bool isdrphamming = false;
            bool ishamming = false;
            string status = string.Empty;
            string withhemming = "";
            string withouthemming = "";
            if (drphamming.SelectedValue == "1")
            {
                isdrphamming = true;
            }
            DataSet dsGrid, dschildGrid = new DataSet();
            dsGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 1,0,'" + txtSearch.Text.ToString() + "'");

            Int32 strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
            if (strAllowHeming == 1)
            {
                chkonoff.Checked = true;
            }
            else
            {
                chkonoff.Checked = false;
            }
            if (dsGrid != null && dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsGrid.Tables[0].Rows.Count; i++)
                {

                    int.TryParse(dsGrid.Tables[0].Rows[i]["ProductId"].ToString(), out iproductid);
                    bool.TryParse(dsGrid.Tables[0].Rows[i]["IsHamming"].ToString(), out ishamming);
                    dschildGrid = CommonComponent.GetCommonDataSet("EXEC usp_UpdateHemmingLogic 2," + iproductid.ToString() + ",'" + txtSearch.Text.ToString() + "'");
                    if (dschildGrid != null && dschildGrid.Tables.Count > 0 && dschildGrid.Tables[0].Rows.Count > 0)
                    {
                        if (ishamming == true && strAllowHeming == 1)
                        {
                            withhemming += "<tr>";

                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span>" + dsGrid.Tables[0].Rows[i]["SKU"].ToString() + " </span>";
                            withhemming += "</td>";


                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";

                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + dsGrid.Tables[0].Rows[i]["Inventory"].ToString() + " </span>";
                            withhemming += "</td>";


                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";

                            withhemming += "<td align=\"left\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";


                            withhemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";

                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";




                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";



                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span></span>";
                            withhemming += "</td>";

                            if (ishamming == true) { status = "Y"; } else { status = "N"; }
                            withhemming += "<td align=\"center\" style=\"width:10%;\">";
                            withhemming += "<span>" + status + "</span>";
                            withhemming += "</td>";


                            withhemming += "</tr>";
                        }
                        else
                        {
                            withouthemming += "<tr>";

                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span>" + dsGrid.Tables[0].Rows[i]["SKU"].ToString() + " </span>";
                            withouthemming += "</td>";


                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + dsGrid.Tables[0].Rows[i]["Inventory"].ToString() + " </span>";
                            withouthemming += "</td>";


                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";


                            withouthemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";

                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span></span>";
                            withouthemming += "</td>";




                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + saleschannel + " </span>";
                            withouthemming += "</td>";



                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + hpdwebsite + " </span>";
                            withouthemming += "</td>";

                            if (ishamming == true) { status = "Y"; } else { status = "N"; }
                            withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                            withouthemming += "<span>" + status + " </span>";
                            withouthemming += "</td>";

                            withouthemming += "</tr>";
                        }
                        for (int j = 0; j < dschildGrid.Tables[0].Rows.Count; j++)
                        {

                            int.TryParse(dschildGrid.Tables[0].Rows[j]["HemmingPercentage"].ToString(), out  jHemmingPercentage);
                            if (jHemmingPercentage != 0 && jHemmingPercentage > 0)
                            {
                                HemmingPercentage = jHemmingPercentage;
                                totalinventory = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(HemmingPercentage) * Convert.ToDecimal(dschildGrid.Tables[0].Rows[j]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                            }
                            else
                            {

                                totalinventory = Convert.ToDecimal(dschildGrid.Tables[0].Rows[j]["Inventory"].ToString());
                            }
                            ProductID = iproductid;

                            dsNew = dschildGrid;
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

                            if (ishamming == true && strAllowHeming == 1)
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
                                    for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                                    {

                                        Int32 Qty = 0;
                                        Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(HemmingPercentage) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
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
                                        if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1)
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


                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1)
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

                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1)
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
                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1)
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


                                    if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-84") > -1)
                                    {
                                        hpdwebsite = Qty84W;
                                        saleschannel = Qty84;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-96") > -1)
                                    {
                                        hpdwebsite = Qty96W;
                                        saleschannel = Qty96;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-108") > -1)
                                    {
                                        hpdwebsite = Qty108W;
                                        saleschannel = Qty108;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-120") > -1)
                                    {
                                        hpdwebsite = Qty120W;
                                        saleschannel = Qty120;
                                    }
                                    else
                                    {
                                        hpdwebsite = 0;
                                        saleschannel = 0;
                                    }


                                    withhemming += "<tr>";

                                    withhemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["SKU"].ToString() + " </span>";
                                    withhemming += "</td>";


                                    withhemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["UPC"].ToString() + " </span>";
                                    withhemming += "</td>";

                                    withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["Inventory"].ToString() + " </span>";
                                    withhemming += "</td>";


                                    withhemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["Name"].ToString() + " </span>";
                                    withhemming += "</td>";

                                    withhemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["Name"].ToString() + " </span>";
                                    withhemming += "</td>";


                                    withhemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                                    withhemming += "<span>" + String.Format("{0:0}", Math.Floor(totalinventory)) + " </span>";
                                    withhemming += "</td>";

                                    withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withhemming += "<span>" + dsNew.Tables[0].Rows[j]["AddiHemingQty"].ToString() + " </span>";
                                    withhemming += "</td>";




                                    withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withhemming += "<span>" + saleschannel + " </span>";
                                    withhemming += "</td>";



                                    withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withhemming += "<span>" + hpdwebsite + " </span>";
                                    withhemming += "</td>";

                                    if (ishamming == true) { status = "Y"; } else { status = "N"; }
                                    withhemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withhemming += "<span></span>";
                                    withhemming += "</td>";


                                    withhemming += "</tr>";

                                }



                            }
                            else
                            {

                                if (dsNew != null && dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                                {



                                    for (int ids = 0; ids < dsNew.Tables[0].Rows.Count; ids++)
                                    {

                                        Int32 Qty = 0;
                                        Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(HemmingPercentage) * Convert.ToDecimal(dsNew.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));

                                        Qty = Qty - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        Int32 Qty1 = 0;
                                        //if (chkonoff.Checked)
                                        //{
                                        //    Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString()) - Convert.ToInt32(dsNew.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                        //}
                                        //else
                                        //{
                                        Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());
                                        //}


                                        // Qty1 = Convert.ToInt32(dsNew.Tables[0].Rows[ids]["Inventory"].ToString());


                                        if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1)
                                        {
                                            Qty84 = Qty;
                                            Qty84W = Qty1;


                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1)
                                        {
                                            Qty96 = Qty;
                                            Qty96W = Qty1;


                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1)
                                        {
                                            Qty108 = Qty;
                                            Qty108W = Qty1;


                                        }
                                        else if (dsNew.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1)
                                        {
                                            Qty120 = Qty;
                                            Qty120W = Qty1;

                                        }
                                        else
                                        {
                                            hpdwebsite = 0;
                                            saleschannel = 0;
                                        }



                                    }

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

                                    if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-84") > -1)
                                    {
                                        hpdwebsite = Qty84W;
                                        saleschannel = Qty84;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-96") > -1)
                                    {
                                        hpdwebsite = Qty96W;
                                        saleschannel = Qty96;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-108") > -1)
                                    {
                                        hpdwebsite = Qty108W;
                                        saleschannel = Qty108;
                                    }
                                    else if (dsNew.Tables[0].Rows[j]["SKU"].ToString().Trim().IndexOf("-120") > -1)
                                    {
                                        hpdwebsite = Qty120W;
                                        saleschannel = Qty120;
                                    }



                                    withouthemming += "<tr>";

                                    withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["SKU"].ToString() + " </span>";
                                    withouthemming += "</td>";


                                    withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["UPC"].ToString() + " </span>";
                                    withouthemming += "</td>";

                                    withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["Inventory"].ToString() + " </span>";
                                    withouthemming += "</td>";


                                    withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["Name"].ToString() + " </span>";
                                    withouthemming += "</td>";

                                    withouthemming += "<td align=\"left\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["Name"].ToString() + " </span>";
                                    withouthemming += "</td>";


                                    withouthemming += "<td align=\"center\" style=\"width:10%;display:none;\">";
                                    withouthemming += "<span>" + String.Format("{0:0}", Math.Floor(totalinventory)) + " </span>";
                                    withouthemming += "</td>";

                                    withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + dsNew.Tables[0].Rows[j]["AddiHemingQty"].ToString() + " </span>";
                                    withouthemming += "</td>";




                                    withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + saleschannel + " </span>";
                                    withouthemming += "</td>";



                                    withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withouthemming += "<span>" + hpdwebsite + " </span>";
                                    withouthemming += "</td>";

                                    if (ishamming == true) { status = "Y"; } else { status = "N"; }
                                    withouthemming += "<td align=\"center\" style=\"width:10%;\">";
                                    withouthemming += "<span></span>";
                                    withouthemming += "</td>";

                                    withouthemming += "</tr>";
                                }


                            }



                        }
                    }

                }

            }

            else
            {

            }

            withhemming += "</table>";
            withouthemming += "</table>";
            withhemming += withouthemming;
            lthamming.Text = withhemming;
        }

        protected void drphamming_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindHemmingNew();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindHemmingNew();
        }

        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindHemmingNew();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string attachment = "attachment; filename=ProductHemminglist_Export.xls";
                Response.ClearContent();
                // Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ProductHemminglist_Export.xls");
                Response.ContentType = "application/ms-excel";
                StringWriter stw = new StringWriter();
                Response.Write(lthamming.Text.ToString());
                Response.Flush();
                Response.End();
            }
            catch
            {

            }

        }

    }
}