﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Data;
using LumenWorks.Framework.IO.Csv;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class FabricVendorPortal : BasePage
    {
        string StrFileName = "";

        public int currentgridfabriccode = 0;
        public int oldgridfabriccode = 0;
        public int gridcounter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillFabricType();
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                if (Session["VendorLogin"] != null && Session["VendorLogin"].ToString() == "1" && Session["AdminvendorId"] != null && Session["AdminvendorId"].ToString() != "")
                {
                    FillVEndorDetail();
                    trvendordetail.Visible = true;
                    trvendortitle.Visible = true;
                }
                else
                {
                    trvendordetail.Visible = false;
                    trvendortitle.Visible = false;
                }
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnimport1.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/iimport-fabric-inventory.png) no-repeat transparent; width: 200px; height: 23px; border:none;cursor:pointer;");
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["VendorLogin"] != null)
            {
                this.MasterPageFile = "~/ADMIN/Vendor.Master";
            }
        }
        private void FillVEndorDetail()
        {

            VendorComponent objVendor = new VendorComponent();
            tb_Vendor tbvendor = new tb_Vendor();
            tbvendor = objVendor.GetVendorByID(Convert.ToInt32(Session["AdminvendorId"].ToString()));

            ltvendordetail.Text = "";
            ltvendordetail.Text += "<table>";
            ltvendordetail.Text += "<tr>";
            string strname = "N/A";
            string strEmail = "N/A";
            if (!string.IsNullOrEmpty(tbvendor.Name))
            {
                strname = tbvendor.Name.ToString();
            }
            if (!string.IsNullOrEmpty(tbvendor.Email))
            {
                strEmail = tbvendor.Email.ToString();
            }
            ltvendordetail.Text += "<td style='font-size:16px;'><strong>Vendor Name :</strong>&nbsp;" + strname.ToString() + "";

            ltvendordetail.Text += "</td>";
            ltvendordetail.Text += "</tr>";

            //string strCity = "N/A";
            //if (!string.IsNullOrEmpty(tbvendor.City))
            //{
            //    strCity = tbvendor.City.ToString();
            //}

            //string strState = "N/A";
            //if (!string.IsNullOrEmpty(tbvendor.State))
            //{
            //    strState = tbvendor.State.ToString();
            //}
            //if (strCity != "" && strState != "")
            //{
            //ltvendordetail.Text += "<tr>";
            //ltvendordetail.Text += "<td><strong>City :</strong>&nbsp;" + strCity + "<strong>State :</strong>&nbsp;" + strState.ToString();
            //ltvendordetail.Text += "</td>";
            //ltvendordetail.Text += "</tr>";
            //}
            ltvendordetail.Text += "</table>";
        }

        /// <summary>
        /// Bind Fabric Type
        /// </summary>
        private void FillFabricType()
        {
            DataSet DsFabircType = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            if (Session["VendorLogin"] != null && Session["VendorLogin"].ToString() == "1" && Session["AdminvendorId"] != null && Session["AdminvendorId"].ToString() != "")
            {
                DsFabircType = CommonComponent.GetCommonDataSet("SELECT FabricTypeID,FabricTypename FROM tb_ProductFabricType WHERE isnull(Active,0)=1 AND FabricTypename in (SELECT isnull(FabricType,'') FROM tb_product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+FabricVendorIds+',') > 0 AND Storeid=1 And isnull(active,0)=1 and isnull(Deleted,0)=0)"); //objProduct.GetProductFabricDetails(0, 1);
            }
            else
            {
                DsFabircType = objProduct.GetProductFabricDetails(0, 1);
            }
            if (DsFabircType != null && DsFabircType.Tables.Count > 0 && DsFabircType.Tables[0].Rows.Count > 0)
            {
                ddlFabricType.DataSource = DsFabircType;
                ddlFabricType.DataValueField = "FabricTypeID";
                ddlFabricType.DataTextField = "FabricTypename";
                ddlFabricType.DataBind();
            }
            else
            {
                ddlFabricType.DataSource = null;
                ddlFabricType.DataBind();
            }
            ddlFabricType.Items.Insert(0, new ListItem("Select Fabric Category", "0"));
            ddlFabricType_SelectedIndexChanged(null, null);
        }

        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ProductComponent objProduct = new ProductComponent();
            DataSet DsFabricCode = new DataSet();
            if (Session["VendorLogin"] != null)
            {


                DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty,0 as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(YardPrice,0) as YardPrice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as Poqty,'' as Updatedon from tb_ProductFabricCode Where FabricTypeID =" + ddlFabricType.SelectedValue.ToString() + @" AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @")  AND Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0)
      Union All  
  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty,(select isnull(ConfigValue,0) from tb_AppConfig where ConfigName='VendorAddDays' and storeid=1) as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(YardPrice,'') as YardPrice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as Poqty,'' as updatedon From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0) AND tb_FabricVendorPortal.FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @"  
  Order by FabricCodeId"); // objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);

            }
            else
            {
                if (ddlFabricType.SelectedIndex == 0)
                {
                    DsFabricCode = objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 6);
                }
                else
                {
                    DsFabricCode = objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
                }
            }
            ltrCalenScript.Text = "";
            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            {
                //btnExport.Visible = true;
                btnExport.Visible = true;

                Session["FabricVendor"] = DsFabricCode;
                DataSet dss = new DataSet();
                dss = bindFabricCodeDetails(DsFabricCode);
                Session["FabricVendor1"] = dss;
                DataView dv = new DataView();
                dv = dss.Tables[0].DefaultView;
                dv.Sort = "FabricCodeId ASC";
                dv.ToTable();


                grdVendorPortal.DataSource = dv.ToTable();
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
                Session["FabricVendor"] = null;
                Session["FabricVendor1"] = null;
                grdVendorPortal.DataSource = null;
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = false;
            }
        }



        private DataSet bindFabricCodeDetails(DataSet ds)
        {


            DataSet Final = new DataSet();
            Final = ds.Copy();
            DataSet dsall = new DataSet();
            string query = " select FabricOrderId,tb_FabricVendorOrder.FabricVendorPortId, tb_FabricVendorOrder.FabricTypeID,tb_FabricVendorOrder.FabricCodeId," +
           " tb_FabricVendorOrder.Code,cast(ProductionDate as date) as ProductionDate,OrderDate," +
          " isnull( tb_FabricVendorOrder.QtyinYard,0) as QtyOnHand,isnull(tb_FabricVendorOrder.QtyBoookedinYard,0) as BookedQty,isnull(tb_FabricVendorOrder.BalanceOrder,0) as AvailQty,isnull(QtyBoookedNAV,0) as QtyBoookedNAV,isnull(Poqty,0) as Poqty,(case when isnull(tb_FabricVendorOrder.updatedon,'')='' then tb_FabricVendorOrder.createdon else tb_FabricVendorOrder.updatedon end) as updatedon " +
           " from tb_FabricVendorOrder  " +
             " left outer join tb_FabricVendorPortal on tb_FabricVendorOrder.FabricCodeId=tb_FabricVendorPortal.FabricCodeId ";

            dsall = CommonComponent.GetCommonDataSet(query);




            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (dsall != null && dsall.Tables.Count > 0 && dsall.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        int FabricCodeId = 0;
                        FabricCodeId = Convert.ToInt32(ds.Tables[0].Rows[i]["FabricCodeId"].ToString());
                        if (FabricCodeId > 0)
                        {
                            DataRow[] drr = dsall.Tables[0].Select("FabricCodeId =" + FabricCodeId, "FabricOrderId ASC");
                            DataRow[] drfinal = Final.Tables[0].Select("FabricCodeId =" + FabricCodeId);
                            if (drr.Length > 0)
                            {

                                if (drfinal.Length > 0)
                                {
                                    string fabriccode = drfinal[0]["code"].ToString();
                                    drfinal[0]["FabricOrderId"] = drr[0]["FabricOrderId"].ToString();
                                    drfinal[0]["code1"] = fabriccode;
                                    drfinal[0]["QtyOnHand"] = drr[0]["QtyOnHand"].ToString();
                                    drfinal[0]["BookedQty"] = drr[0]["BookedQty"].ToString();
                                    drfinal[0]["QtyBoookedNAV"] = drr[0]["QtyBoookedNAV"].ToString();
                                    drfinal[0]["ProductionDate"] = drr[0]["ProductionDate"].ToString();
                                    drfinal[0]["Poqty"] = drr[0]["Poqty"].ToString();
                                    drfinal[0]["updatedon"] = drr[0]["updatedon"].ToString();
                                    Final.Tables[0].AcceptChanges();


                                    for (int j = 1; j < drr.Length; j++)
                                    {
                                        DataRow gg = Final.Tables[0].NewRow();
                                        gg.ItemArray = drfinal[0].ItemArray.Clone() as object[];


                                        gg["FabricOrderId"] = drr[j]["FabricOrderId"].ToString();

                                        gg["code1"] = fabriccode + "_" + j;
                                        gg["QtyOnHand"] = drr[j]["QtyOnHand"].ToString();
                                        gg["BookedQty"] = drr[j]["BookedQty"].ToString();
                                        gg["QtyBoookedNAV"] = drr[j]["QtyBoookedNAV"].ToString();
                                        gg["ProductionDate"] = drr[j]["ProductionDate"].ToString();
                                        gg["Poqty"] = drr[j]["Poqty"].ToString();
                                        gg["updatedon"] = drr[j]["updatedon"].ToString();

                                        Final.Tables[0].Rows.Add(gg);
                                        Final.AcceptChanges();

                                    }



                                    if (drr.Length < 4)
                                    {
                                        Int32 tt = drr.Length;
                                        for (int k = 0; k < (4 - tt); k++)
                                        {
                                            DataRow gg = Final.Tables[0].NewRow();
                                            gg.ItemArray = drfinal[0].ItemArray.Clone() as object[];
                                            gg["FabricOrderId"] = 0;

                                            gg["code1"] = fabriccode + "_" + (tt + k);
                                            gg["QtyOnHand"] = 0;
                                            gg["BookedQty"] = 0;
                                            gg["QtyBoookedNAV"] = 0;
                                            gg["ProductionDate"] = "";
                                            gg["Poqty"] = 0;
                                            gg["updatedon"] = "";
                                            Final.Tables[0].Rows.Add(gg);
                                            Final.AcceptChanges();

                                        }
                                    }


                                }
                            }

                            else
                            {


                                string fabriccode = drfinal[0]["code"].ToString();
                                drfinal[0]["FabricOrderId"] = 0;
                                drfinal[0]["code1"] = fabriccode;
                                drfinal[0]["QtyOnHand"] = 0;
                                drfinal[0]["BookedQty"] = 0;
                                drfinal[0]["QtyBoookedNAV"] = 0;
                                drfinal[0]["ProductionDate"] = "";
                                drfinal[0]["Poqty"] = 0;
                                drfinal[0]["updatedon"] = "";
                                Final.Tables[0].AcceptChanges();

                                for (int j = 1; j <= 3; j++)
                                {

                                    DataRow gg = Final.Tables[0].NewRow();
                                    gg.ItemArray = drfinal[0].ItemArray.Clone() as object[];


                                    gg["FabricOrderId"] = 0;

                                    gg["code1"] = fabriccode + "_" + j;
                                    gg["QtyOnHand"] = 0;
                                    gg["BookedQty"] = 0;
                                    gg["QtyBoookedNAV"] = 0;
                                    gg["ProductionDate"] = "";
                                    gg["Poqty"] = 0;
                                    gg["updatedon"] = "";
                                    Final.Tables[0].Rows.Add(gg);
                                    Final.AcceptChanges();

                                }



                            }

                        }

                    }


                }

            }




            return Final;


        }



        protected void grdVendorPortal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[11].Attributes.Add("style", "display:none;");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox lbldays = (TextBox)e.Row.FindControl("lbldays");
                Label lblFabricCodeId = (Label)e.Row.FindControl("lblFabricCodeId");
                Label lblFabricVendorPortId = (Label)e.Row.FindControl("lblFabricVendorPortId");
                CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
                TextBox txtMinQty = (TextBox)e.Row.FindControl("txtMinQty");
                TextBox txtMaxOrderQty = (TextBox)e.Row.FindControl("txtMaxOrderQty");
                //TextBox txtQtyOnHand = (TextBox)e.Row.FindControl("txtQtyOnHand");
                TextBox txtBookedQty = (TextBox)e.Row.FindControl("txtBookedQty");
                Label lblFabricTypeID = (Label)e.Row.FindControl("lblFabricTypeID");
                Label lblName = (Label)e.Row.FindControl("lblName");
                TextBox txtminqty1 = (TextBox)e.Row.FindControl("txtminqty1");
                TextBox lblyardcost = (TextBox)e.Row.FindControl("lblyardcost");
                //TextBox txtAvailQty = (TextBox)e.Row.FindControl("txtAvailQty");
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";


                Label lblbalanceqty = (Label)e.Row.FindControl("lblbalanceqty");

                e.Row.Cells[11].Attributes.Add("style", "display:none;");
                System.Web.UI.HtmlControls.HtmlAnchor atagView = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("atagView");


                string days = Convert.ToString(CommonComponent.GetScalarCommonData("select noofdays from tb_ProductFabricWidth where fabriccodeid=" + lblFabricCodeId.Text.ToString() + ""));
                if (!string.IsNullOrEmpty(days))
                {
                    lbldays.Text = days.ToString();
                    ViewState["days"] = days.ToString();
                }
                else
                {
                    lbldays.Text = "0";
                }

                atagView.Attributes.Add("onclick", "ShowModelCredit('/ADMIN/Products/FabricVendorOrder_Popup.aspx?FabricTypeId=" + lblFabricTypeID.Text.ToString() + "&FabricCodeId=" + lblFabricCodeId.Text.ToString() + "');");

                if (lblFabricVendorPortId != null && lblFabricVendorPortId.Text.ToString() == "0")
                {
                    chkActive.Checked = false;
                }
                else
                {
                    chkActive.Checked = true;
                }

                //txtBookedQty.Attributes.Add("onkeyup", "ClacuOnHandQty(" + e.Row.RowIndex + ");");
                //txtAvailQty.Attributes.Add("onkeyup", "ClacuOnHandQty(" + e.Row.RowIndex + ");");

                //  txtQtyOnHand.Attributes.Add("readonly", "true");



                string Funname = "";
                Label lblCode = (Label)e.Row.FindControl("lblCode");
                //  string strCode = ((Label)e.Row.Parent.Parent.Parent.FindControl("lblCode")).Text;

                string strCode = lblCode.Text.ToString();

                //    TextBox txtQtyinYard = (TextBox)e.Row.FindControl("txtQtyinYard");


                Label lblFabricOrderId = (Label)e.Row.FindControl("lblFabricOrderId");
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                TextBox txtqtyonhand = (TextBox)e.Row.FindControl("txtqtyonhand");
                TextBox txtbookedqty = (TextBox)e.Row.FindControl("txtbookedqty");
                TextBox txtavailqty = (TextBox)e.Row.FindControl("txtavailqty");
                Label lblsku = (Label)e.Row.FindControl("lblsku");
                Label lblorder = (Label)e.Row.FindControl("lblorder");
                TextBox txtpoorderqty = (TextBox)e.Row.FindControl("txtpoorderqty");

                //TextBox txtOrderDate = (TextBox)e.Row.FindControl("txtOrderDate");
                TextBox txtProductionDate = (TextBox)e.Row.FindControl("txtProductionDate");
                TextBox txtbookupload = (TextBox)e.Row.FindControl("txtbookupload");



                Label lblproductiondate = (Label)e.Row.FindControl("lblproductiondate");


                txtbookedqty.Attributes.Add("readonly", "true");
                txtbookupload.Attributes.Add("readonly", "true");
                // txtqtyonhand.Attributes.Add("readonly", "true");
                //  if (String.IsNullOrEmpty(txtOrderDate.Text) || txtOrderDate.Text.ToString().ToLower().IndexOf("1900") > -1)
                //    txtOrderDate.Text = "";
                if (String.IsNullOrEmpty(txtProductionDate.Text) || txtProductionDate.Text.ToString().ToLower().IndexOf("1900") > -1)
                    txtProductionDate.Text = "";

                if (String.IsNullOrEmpty(lblproductiondate.Text) || lblproductiondate.Text.ToString().ToLower().IndexOf("1900") > -1)
                    lblproductiondate.Text = "";


                if (!String.IsNullOrEmpty(lblproductiondate.Text))
                {
                    lblproductiondate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(lblproductiondate.Text));
                }


                if (!String.IsNullOrEmpty(txtProductionDate.Text))
                {
                    txtProductionDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtProductionDate.Text));
                }

                txtqtyonhand.Attributes.Add("onkeyup", "ClacuOnHandQtynew('" + txtqtyonhand.ClientID + "','" + txtbookedqty.ClientID + "','" + txtbookupload.ClientID + "','" + txtavailqty.ClientID + "','" + txtpoorderqty.ClientID + "');");
                txtbookedqty.Attributes.Add("onkeyup", "ClacuOnHandQty1('" + txtqtyonhand.ClientID + "','" + txtbookedqty.ClientID + "','" + txtbookupload.ClientID + "','" + txtavailqty.ClientID + "');");
                txtbookupload.Attributes.Add("onkeyup", "ClacuOnHandQty1('" + txtqtyonhand.ClientID + "','" + txtbookedqty.ClientID + "','" + txtbookupload.ClientID + "','" + txtavailqty.ClientID + "');");
                txtavailqty.Attributes.Add("readonly", "true");
                txtpoorderqty.Attributes.Add("onkeyup", "ClacuOnHandQty('" + txtpoorderqty.ClientID + "','" + txtbookedqty.ClientID + "','" + txtbookupload.ClientID + "','" + txtavailqty.ClientID + "','" + txtqtyonhand.ClientID + "');");
                // lblsku.Text = strCode + "_" + (e.Row.RowIndex + 1);


                //txtpoorderqty.Text = txtqtyonhand.Text;
                if (lblsku.Text.ToString().IndexOf("_1") > -1 || lblsku.Text.ToString().IndexOf("_2") > -1 || lblsku.Text.ToString().IndexOf("_3") > -1)
                {


                    Funname += "<script type=\"text/javascript\">";
                    Funname += " $(function () {$('#" + txtProductionDate.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true});});";
                    Funname += "</script>";
                    ltrCalenScript.Text += Funname.ToString();
                }
                else
                {
                    txtProductionDate.Attributes.Add("style", "display:none");
                    txtpoorderqty.Attributes.Add("style", "display:none");
                    if (e.Row.RowIndex > 0)
                    {
                        e.Row.Attributes.Add("style", "border-top:solid 1px #6a6a6a;");
                    }

                }






                //DateTime fabricdate = Convert.ToDateTime(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=1 AND Configname='FabricCalculateDate'"));




                Int32 bookqty1 = 0, bookuploadqty1 = 0, qtyonhand1 = 0;
                Int32.TryParse(txtbookedqty.Text, out bookqty1);
                Int32.TryParse(txtbookupload.Text, out bookuploadqty1);
                Int32.TryParse(txtqtyonhand.Text, out qtyonhand1);

                txtavailqty.Text = Convert.ToString(qtyonhand1 - bookqty1 - bookuploadqty1);


                lblbalanceqty.Text = Convert.ToString(qtyonhand1 - bookqty1 - bookuploadqty1);

                //if (gridcounter >= 0 && gridcounter < 5)
                //{
                //    currentgridfabriccode = Convert.ToInt32(lblFabricCodeId.Text.ToString());

                //    if(gridcounter==0)
                //    {


                //    }
                //    else
                //    {

                //    }


                //    gridcounter++;



                //}
                //if (gridcounter >= 4)
                //{
                //    oldgridfabriccode = currentgridfabriccode;
                //    currentgridfabriccode = 0;
                //    gridcounter = 0;

                //}

                //if (lblsku.Text.ToString().IndexOf("_1") > -1 || lblsku.Text.ToString().IndexOf("_2") > -1 || lblsku.Text.ToString().IndexOf("_3") > -1)
                //{
                //    lblCode.Visible = false;
                //    lblName.Visible = false;
                //    txtminqty1.Visible = false;
                //    lbldays.Visible = false;
                //    lblyardcost.Visible = false;

                //}
                //else
                //{
                //    lblsku.Visible = false;


                //}


                if (lblsku.Text.ToString().IndexOf("_1") > -1 || lblsku.Text.ToString().IndexOf("_2") > -1 || lblsku.Text.ToString().IndexOf("_3") > -1)
                {
                    lblCode.Visible = false;
                    lblName.Visible = false;
                    txtminqty1.Visible = false;
                    lbldays.Visible = false;
                    lblyardcost.Visible = false;
                    lblorder.Visible = true;

                }
                else
                {
                    lblorder.Visible = false;
                    //txtpoorderqty.Visible = false;
                    //txtProductionDate.Visible = false;
                    txtProductionDate.Attributes.Add("style", "display:none");
                    txtpoorderqty.Attributes.Add("style", "display:none");


                }


                if (lblorder.Text.ToString().IndexOf("_1") > -1)
                {
                    lblorder.Text = "Order 1";
                }
                else if (lblorder.Text.ToString().IndexOf("_2") > -1)
                {
                    lblorder.Text = "Order 2";
                }
                else if (lblorder.Text.ToString().IndexOf("_3") > -1)
                {
                    lblorder.Text = "Order 3";
                }
                else if (lblorder.Text.ToString().IndexOf("_4") > -1)
                {
                    lblorder.Text = "Order 4";
                }



            }
        }

        protected void grdVendorPortal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                Label lblFabricVendorPortId = (Label)row.FindControl("lblFabricVendorPortId");
                Label lblFabricTypeID = (Label)row.FindControl("lblFabricTypeID");
                Label lblCode = (Label)row.FindControl("lblCode");
                if (e.CommandName == "_deleteLinkButton")
                {
                    if (lblCode.Visible == true)
                    {

                        try
                        {
                            if (Convert.ToInt32(lblFabricTypeID.Text.ToString()) > 0)
                            {
                                int FabricCodeId = Convert.ToInt32(e.CommandArgument);
                                CommonComponent.ExecuteCommonData("Delete from tb_ProductFabricCode where FabricCodeId=" + FabricCodeId + " and FabricTypeID=" + lblFabricTypeID.Text.ToString() + "");
                                // CommonComponent.ExecuteCommonData("Delete from tb_FabricVendorPortal where FabricCodeId=" + FabricCodeId + " and FabricTypeID=" + lblFabricTypeID.Text.ToString() + "");
                                ddlFabricType_SelectedIndexChanged(null, null);

                            }
                        }
                        catch (Exception ex)
                        { throw ex; }

                    }

                    Label lblFabricOrderId = (Label)row.FindControl("lblFabricOrderId");
                    int FabricOrderId = Convert.ToInt32(lblFabricOrderId.Text);
                    if (FabricOrderId > 0)
                    {
                        Label lblFabricCodeId = (Label)row.FindControl("lblFabricCodeId");


                        Int32 QtyinYard = 0, QtyBoookedinYard = 0, BalanceOrder = 0, QtyReceived = 0;
                        string StrInsert = "";

                        StrInsert = "UPDATE [tb_FabricVendorOrder] set [QtyinYard] = " + QtyinYard + ",[QtyBoookedinYard] = " + QtyBoookedinYard + ",[BalanceOrder] = " + BalanceOrder + ",[QtyReceived] = " + QtyReceived + ",[OrderDate] = NULL,[ProductionDate] = NULL WHERE FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                        CommonComponent.ExecuteCommonData(StrInsert.ToString());
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Record Deleted Successfully','Success');", true);
                    }
                }



                //try
                //{
                //    StrInsert = "INSERT INTO [tb_FabricVendorOrderLog]([FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn],[Deleted]) " +
                //        " Select [FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],ISNULL(QtyinYard,0) QtyinYard ,ISNULL(QtyBoookedinYard,0) QtyBoookedinYard,ISNULL(BalanceOrder,0) as BalanceOrder,[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn],1 from [tb_FabricVendorOrderLog] Where FabricOrderId=" + FabricOrderId + " Select SCOPE_IDENTITY();";
                //    FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));
                //}
                //catch { }

            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            //if (Convert.ToInt32(ddlFabricType.SelectedValue) > 0)
            //{
            if (grdVendorPortal.Rows.Count > 0)
            {
                string code = "";
                int counter = 0;
              
                for (int i = 0; i < grdVendorPortal.Rows.Count; i++)
                {
                    TextBox txtMinQty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtMinQty");
                    TextBox txtMaxOrderQty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtMaxOrderQty");
                    TextBox txtQtyOnHand = (TextBox)grdVendorPortal.Rows[i].FindControl("txtQtyOnHand");
                    TextBox txtBookedQty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtBookedQty");
                    TextBox txtAvailQty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtAvailQty");
                    TextBox txtpoorderqty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtpoorderqty");


                    Label lblFabricCodeId = (Label)grdVendorPortal.Rows[i].FindControl("lblFabricCodeId");
                    Label lblFabricVendorPortId = (Label)grdVendorPortal.Rows[i].FindControl("lblFabricVendorPortId");
                    Label lblCode = (Label)grdVendorPortal.Rows[i].FindControl("lblCode");
                    Label lblFabricTypeID = (Label)grdVendorPortal.Rows[i].FindControl("lblFabricTypeID");

                    Int32 MinQty = 0, MinOrderQty = 0, QtyOnHand = 0, BookedQty = 0, AvailQty = 0, balanceqty = 0;
                    int.TryParse(txtMinQty.Text.ToString(), out MinQty);

                    int.TryParse(txtMinQty.Text.ToString(), out MinQty);
                    int.TryParse(txtMaxOrderQty.Text.ToString(), out MinOrderQty);
                    int.TryParse(txtQtyOnHand.Text.ToString(), out QtyOnHand);
                    int.TryParse(txtBookedQty.Text.ToString(), out BookedQty);
                    int.TryParse(txtAvailQty.Text.ToString(), out AvailQty);


                    //   int.TryParse(lblbalanceqty.Text.ToString(), out balanceqty);


                    TextBox txtminqty1 = (TextBox)grdVendorPortal.Rows[i].FindControl("txtminqty1");
                    TextBox lbldays = (TextBox)grdVendorPortal.Rows[i].FindControl("lbldays");
                    TextBox lblyardcost = (TextBox)grdVendorPortal.Rows[i].FindControl("lblyardcost");

                    Int32 minqty1 = 0, days = 0;
                    decimal yardcost = 0;

                    int.TryParse(txtminqty1.Text.ToString(), out minqty1);
                    int.TryParse(lbldays.Text.ToString(), out days);
                    decimal.TryParse(lblyardcost.Text.ToString(), out yardcost);


                    string StrInsert = "";
                    string StrInsert1 = "";
                    string StrInsert2 = "";
                    Int32 FabricVendorPortId = 0;

                    if (lblCode.Visible == true)
                    {
                        if (lblFabricVendorPortId.Text.ToString().Trim() == "0")
                        {
                            FabricVendorPortId = Convert.ToInt32(lblFabricVendorPortId.Text.ToString().Trim());
                            StrInsert = "INSERT INTO [tb_FabricVendorPortal]([FabricTypeID],[FabricCodeId],[Code],[CreatedBy],[CreatedOn]) " +
                            " VALUES(" + lblFabricTypeID.Text.ToString() + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "'," + Session["AdminId"].ToString() + ",Getdate()); Select SCOPE_IDENTITY();";
                            FabricVendorPortId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));

                            string noofdays = (Convert.ToString(CommonComponent.GetScalarCommonData("select FabricCodeId from tb_ProductFabricWidth WHERE FabricCodeId=" + lblFabricCodeId.Text + "")));
                            if (!string.IsNullOrEmpty(noofdays))
                            {

                                try
                                {

                                    string str1 = "insert into tb_ProductFabricWidthLog (FabricwidthId,FabricCodeId,Width,DisplayOrder,CreatedBy,CreatedOn,Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,Deleted) select FabricwidthId,FabricCodeId,Width,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,0 from tb_ProductFabricWidth where FabricCodeId=" + lblFabricCodeId.Text + " ";
                                    CommonComponent.ExecuteCommonData(str1);
                                }
                                catch
                                {

                                }
                                StrInsert1 = "UPDATE tb_ProductFabricWidth SET updatedby=" + Session["AdminId"].ToString() + ",updatedon=getdate() WHERE FabricCodeId=" + lblFabricCodeId.Text + "";
                            }
                            else
                            {
                                StrInsert1 = "insert into tb_ProductFabricWidth(FabricCodeId,CreatedBy,CreatedOn,active,noofdays) values (" + lblFabricCodeId.Text + "," + Session["AdminId"].ToString() + ",Getdate(),1," + days + ")";
                            }


                            try
                            {
                                string str = "insert into tb_ProductFabricCodeLog (FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder,CreatedBy,CreatedOn,FabricVendorIds,YardPrice,SafetyLock,Discontinue,Deleted) select FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),FabricVendorIds,YardPrice,SafetyLock,Discontinue,0 from tb_ProductFabricCode where FabricCodeId=" + lblFabricCodeId.Text + " ";
                                CommonComponent.ExecuteCommonData(str);
                            }
                            catch
                            {

                            }
                            StrInsert2 = "UPDATE tb_ProductFabricCode set YardPrice=" + yardcost + " WHERE FabricCodeId=" + lblFabricCodeId.Text + "";
                            CommonComponent.ExecuteCommonData(StrInsert1.ToString());
                           // CommonComponent.ExecuteCommonData(StrInsert2.ToString());



                        }
                        else
                        {
                            // Update
                            FabricVendorPortId = Convert.ToInt32(lblFabricVendorPortId.Text.ToString().Trim());
                            //   StrInsert = "UPDATE [tb_FabricVendorPortal] set [MinQty] = " + MinQty + ",[MinOrderQty] = " + MinOrderQty + ",[QtyOnHand] = " + QtyOnHand + ",[BookedQty] = " + BookedQty + ",[AvailQty] = " + AvailQty + " WHERE FabricVendorPortId=" + lblFabricVendorPortId.Text.ToString() + "";

                            try
                            {
                                string StrInsertlog = "INSERT INTO [tb_FabricVendorPortalLog]([FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[MinQty],[MinOrderQty],[QtyOnHand],[BookedQty],[AvailQty],[CreatedBy],[CreatedOn],[UpdatedOn],[UpdatedBy]) " +
                       " select FabricVendorPortId,FabricTypeID,FabricCodeId,Code,MinQty,MinOrderQty,QtyOnHand,BookedQty,AvailQty," + Session["AdminId"].ToString() + ",Getdate(),Getdate()," + Session["AdminId"].ToString() + " from tb_FabricVendorPortal where fabriccodeid=" + lblFabricCodeId.Text + "";
                                CommonComponent.ExecuteCommonData(StrInsertlog.ToString());
                            }
                            catch { }


                           StrInsert = "update tb_FabricVendorPortal set MinQty=" + minqty1 + " WHERE FabricVendorPortId=" + lblFabricVendorPortId.Text.ToString() + "";



                            string noofdays = (Convert.ToString(CommonComponent.GetScalarCommonData("select FabricCodeId from tb_ProductFabricWidth WHERE FabricCodeId=" + lblFabricCodeId.Text + "")));
                            if (!string.IsNullOrEmpty(noofdays))
                            {




                                try
                                {

                                    string str1 = "insert into tb_ProductFabricWidthLog (FabricwidthId,FabricCodeId,Width,DisplayOrder,CreatedBy,CreatedOn,Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,Deleted) select FabricwidthId,FabricCodeId,Width,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),Active,QtyOnHand,NextOrderQty,AlertQty,AvailableDate,AllowQty,NoOfDays,0 from tb_ProductFabricWidth where FabricCodeId=" + lblFabricCodeId.Text + " ";
                                    CommonComponent.ExecuteCommonData(str1);
                                }
                                catch
                                {

                                }


                                StrInsert1 = "UPDATE tb_ProductFabricWidth SET updatedby=" + Session["AdminId"].ToString() + ",updatedon=getdate()  WHERE FabricCodeId=" + lblFabricCodeId.Text + "";
                            }
                            else
                            {
                                StrInsert1 = "insert into tb_ProductFabricWidth(FabricCodeId,CreatedBy,CreatedOn,active,noofdays) values (" + lblFabricCodeId.Text + "," + Session["AdminId"].ToString() + ",Getdate(),1," + days + ")";
                            }


                            try
                            {
                                string str = "insert into tb_ProductFabricCodeLog (FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder,CreatedBy,CreatedOn,FabricVendorIds,YardPrice,SafetyLock,Discontinue,Deleted) select FabricCodeId,FabricTypeID,Code,Name,Active,DisplayOrder," + Session["AdminId"].ToString() + ",Getdate(),FabricVendorIds,YardPrice,SafetyLock,Discontinue,0 from tb_ProductFabricCode where FabricCodeId=" + lblFabricCodeId.Text + " ";
                                CommonComponent.ExecuteCommonData(str);
                            }
                            catch
                            {

                            }
                            StrInsert2 = "UPDATE tb_ProductFabricCode set YardPrice=" + yardcost + " WHERE FabricCodeId=" + lblFabricCodeId.Text + "";
                           // CommonComponent.ExecuteCommonData(StrInsert.ToString());
                            CommonComponent.ExecuteCommonData(StrInsert1.ToString());
                          //  CommonComponent.ExecuteCommonData(StrInsert2.ToString());


                        }

                    }

                    Label lblsku = (Label)grdVendorPortal.Rows[i].FindControl("lblsku");
                    Label lblorder = (Label)grdVendorPortal.Rows[i].FindControl("lblorder");

                    TextBox txtqtyonhand = (TextBox)grdVendorPortal.Rows[i].FindControl("txtqtyonhand");
                    TextBox txtbookedqty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtbookedqty");
                    TextBox txtavailqty = (TextBox)grdVendorPortal.Rows[i].FindControl("txtavailqty");
                    TextBox txtProductionDate = (TextBox)grdVendorPortal.Rows[i].FindControl("txtProductionDate");
                    TextBox txtbookupload = (TextBox)grdVendorPortal.Rows[i].FindControl("txtbookupload");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnvendorqty = (System.Web.UI.HtmlControls.HtmlInputHidden)grdVendorPortal.Rows[i].FindControl("hdnvendorqty");

                    System.Web.UI.HtmlControls.HtmlInputHidden hdnvendorqty1 = (System.Web.UI.HtmlControls.HtmlInputHidden)grdVendorPortal.Rows[i].FindControl("hdnvendorqty1");

                    //  Label lblVendorOrderNum = (Label)grdVendorOrder.Rows[k].FindControl("lblVendorOrderNum");


                    //Label lblFabricCodeId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricCodeId");
                    //Label lblFabricVendorPortId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricVendorPortId");

                    Label lblFabricOrderId = (Label)grdVendorPortal.Rows[i].FindControl("lblFabricOrderId");


                    Label lblbalanceqty = (Label)grdVendorPortal.Rows[i].FindControl("lblbalanceqty");
                    Label lblqtyonhand = (Label)grdVendorPortal.Rows[i].FindControl("lblqtyonhand");
                    Label lblbookedqty = (Label)grdVendorPortal.Rows[i].FindControl("lblbookedqty");
                    Label lblbookupload = (Label)grdVendorPortal.Rows[i].FindControl("lblbookupload");
                    Label lblpoorderqty = (Label)grdVendorPortal.Rows[i].FindControl("lblpoorderqty");
                    Label lblproductiondate = (Label)grdVendorPortal.Rows[i].FindControl("lblproductiondate");

                    Int32 qtyonhand = 0, bookqty = 0, availqty = 0, uploadqty = 0, poqty = 0;

                    Int32 qtyonhand1 = 0, bookqty1 = 0, bookupload1 = 0, poqty1 = 0, balance1 = 0;
                    //HiddenField hdnupdateqty = (HiddenField)grdVendorOrder.Rows[k].FindControl("hdnupdateqty");


                    //if(hdnupdateqty.Value!=null && hdnupdateqty.Value!="0")
                    //{
                    //    CommonComponent.ExecuteCommonData("");
                    //}

                    if (lblsku.Visible == true && lblsku.Text.ToString().LastIndexOf("_1") > -1)
                    {
                        lblsku.Text = lblCode.Text + "_2";
                    }
                    else if (lblsku.Visible == true && lblsku.Text.ToString().LastIndexOf("_2") > -1)
                    {
                        lblsku.Text = lblCode.Text + "_3";
                    }
                    else if (lblsku.Visible == true && lblsku.Text.ToString().LastIndexOf("_3") > -1)
                    {
                        lblsku.Text = lblCode.Text + "_4";
                    }
                    if (lblsku.Visible == false)
                    {
                        if (lblCode.Text.ToString()!=code)
                        {
                            code = lblCode.Text.ToString();
                            counter = 1;

                        }
                        else
                        {
                            counter++;
                        }

                        lblsku.Text = lblCode.Text + "_" + counter;
                       // lblsku.Text = lblCode.Text + "_1";
                    }


                    //if (lblorder.Visible == true && lblorder.Text.ToString().LastIndexOf("_1") > -1)
                    //{
                    //    lblorder.Text = "Order 2";
                    //}
                    //else if (lblorder.Visible == true && lblorder.Text.ToString().LastIndexOf("_2") > -1)
                    //{
                    //    lblorder.Text = "Order 3";
                    //}
                    //else if (lblorder.Visible == true && lblorder.Text.ToString().LastIndexOf("_3") > -1)
                    //{
                    //    lblorder.Text = "Order 4";
                    //}
                    //if (lblorder.Visible == false)
                    //{
                    //    lblorder.Text = "Order 1";
                    //}


                    int.TryParse(txtqtyonhand.Text.ToString(), out qtyonhand);
                    int.TryParse(txtbookedqty.Text.ToString(), out bookqty);
                    int.TryParse(txtbookupload.Text.ToString(), out uploadqty);
                    int.TryParse(txtavailqty.Text.ToString(), out availqty);
                    int.TryParse(txtqtyonhand.Text.ToString(), out poqty);


                    int.TryParse(lblqtyonhand.Text.ToString(), out qtyonhand1);
                    int.TryParse(lblbookedqty.Text.ToString(), out bookqty1);
                    int.TryParse(lblbookupload.Text.ToString(), out bookupload1);
                    int.TryParse(lblbalanceqty.Text.ToString(), out balance1);
                    int.TryParse(lblpoorderqty.Text.ToString(), out poqty1);









                    StrInsert = "";
                    Int32 FabricOrderId = 0;
                    if (lblFabricOrderId.Text.ToString().Trim() == "0")
                    {
                        //StrInsert = "INSERT INTO [tb_FabricVendorOrder]([FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[ProductionDate],[CreatedBy],[CreatedOn],POQty) " +
                        //" VALUES(" + lblFabricVendorPortId.Text.ToString() + "," + ddlFabricType.SelectedValue + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "','" + lblsku.Text.ToString().Replace("'", "''") + "'," + qtyonhand + "," + bookqty + "," + availqty + ",'" + txtProductionDate.Text + "'," + Session["AdminId"].ToString() + ",Getdate()," + poqty + "); Select SCOPE_IDENTITY();";
                        //FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));

                        int FabricTypeID = 0;
                        if (ddlFabricType.SelectedIndex>0)
                        {
                            FabricTypeID = Convert.ToInt32(ddlFabricType.SelectedValue.ToString());
                        }
                        else
                        {
                            FabricTypeID = Convert.ToInt32(lblFabricTypeID.Text.ToString());
                        }
                        if (FabricTypeID<=0)
                        {
                            FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select FabricTypeID from tb_ProductFabricCode where Code='" + lblCode.Text.ToString() + "'"));
                        }
                        StrInsert = "INSERT INTO [tb_FabricVendorOrder]([FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[ProductionDate],[CreatedBy],[CreatedOn],POQty) " +
                        " VALUES(0," + FabricTypeID + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "','" + lblsku.Text.ToString().Replace("'", "''") + "'," + qtyonhand + "," + bookqty + "," + availqty + ",'" + txtProductionDate.Text + "'," + Session["AdminId"].ToString() + ",Getdate()," + poqty + "); Select SCOPE_IDENTITY();";
                        FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));
                    }
                    else
                    {

                        try
                        {



                            StrInsert = "INSERT INTO [tb_FabricVendorOrderLog]([FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn],QtyBoookedNAV,Poqty) " +
                                " select FabricOrderId,FabricVendorPortId,FabricTypeID,FabricCodeId,Code,VendorOrderNumber,QtyinYard,QtyBoookedinYard,BalanceOrder,OrderDate,ProductionDate,QtyReceived," + Session["AdminId"].ToString() + ",Getdate(),QtyBoookedNAV,Poqty from tb_FabricVendorOrder where FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                            CommonComponent.GetScalarCommonData(StrInsert.ToString());
                        }
                        catch { }


                        // Update
                        FabricOrderId = Convert.ToInt32(lblFabricOrderId.Text.ToString().Trim());



                        if (lblproductiondate.Text.ToString().IndexOf("1900") > -1)
                        {
                            lblproductiondate.Text = "";
                        }

                        if (qtyonhand == qtyonhand1 && availqty == balance1 && poqty == poqty1 && txtProductionDate.Text.ToString() == lblproductiondate.Text.ToString() && bookqty == bookqty1 && uploadqty == bookupload1)
                        {
                            StrInsert = "UPDATE [tb_FabricVendorOrder] set [QtyinYard] = " + qtyonhand + ", [BalanceOrder] = " + availqty + ",[ProductionDate] = '" + txtProductionDate.Text.Trim() + "',[POQty]=" + poqty + " WHERE FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                        }
                        else
                        {
                            StrInsert = "UPDATE [tb_FabricVendorOrder] set [QtyinYard] = " + qtyonhand + ", [BalanceOrder] = " + availqty + ",[ProductionDate] = '" + txtProductionDate.Text.Trim() + "',[POQty]=" + poqty + ",UpdatedOn=getdate() WHERE FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                        }
                        CommonComponent.ExecuteCommonData(StrInsert.ToString());

                        //CommonComponent.ExecuteCommonData("update tb_ProductFabricWidth set NoOfDays=" + days + " where FabricCodeId=" + lblFabricCodeId.Text.ToString() + "");

                        //CommonComponent.ExecuteCommonData("update tb_ProductFabricCode set YardPrice=" + yardqty + " where FabricCodeId=" + lblFabricCodeId.Text.ToString() + "");


                        //CommonComponent.ExecuteCommonData("update tb_FabricVendorPortal set MinQty=" + MinQty.Text + " where FabricVendorPortId=" + lblFabricVendorPortId.Text.ToString() + "");


                    }
                    if (hdnvendorqty.Value == "1")
                    {
                        CommonComponent.ExecuteCommonData("update tb_FabricVendorOrder set QtyBoookedinYard=0,QtyBoookedNAV=0 where FabricOrderId=" + FabricOrderId.ToString() + "");
                    }
                    if (hdnvendorqty1.Value == "1")
                    {
                        CommonComponent.ExecuteCommonData("update tb_FabricVendorOrder set  QtyBoookedNAV=0 where FabricOrderId=" + FabricOrderId.ToString() + "");
                    }








                    //string StrInsertlog = "INSERT INTO [tb_FabricVendorPortalLog]([FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[MinQty],[MinOrderQty],[QtyOnHand],[BookedQty],[AvailQty],[CreatedBy],[CreatedOn],[UpdatedOn],[UpdatedBy]) " +
                    //    " VALUES(" + FabricVendorPortId + "," + lblFabricTypeID.Text.ToString() + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "'," + MinQty + "," + MinOrderQty + "," + QtyOnHand + "," + BookedQty + "," + AvailQty + "," + Session["AdminId"].ToString() + ",Getdate(),Getdate()," + Session["AdminId"].ToString() + ")";
                    //CommonComponent.ExecuteCommonData(StrInsertlog.ToString());
                }
                btnSearch_Click(null, null);
                //ddlFabricType_SelectedIndexChanged(null, null);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Record Saved Successfully','Success');", true);
            }
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@validf", "jAlert('Please Select Fabric Category Name','Message');", true);
            //    return;
            //}
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdVendorPortal.PageIndex = 0;
            GetGrid();

        }


        private void GetGrid()
        {
            ProductComponent objProduct = new ProductComponent();
            DataSet DsFabricCode = new DataSet();

            if (ddlFabricType.SelectedIndex > 0)
            {
                if (Session["VendorLogin"] != null)
                {
                    DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty,0 as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as Updatedon from tb_ProductFabricCode Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @" and Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%' AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @"%' and Code like '%" + txtSearch.Text.ToString().Trim().Replace("'", "''") + @"%')  AND Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0)
      Union All  
  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty,(select isnull(ConfigValue,0) from tb_AppConfig where ConfigName='VendorAddDays' and storeid=1) as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as updatedon From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0) AND tb_FabricVendorPortal.FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @" and tb_FabricVendorPortal.Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%'  
  Order by FabricCodeId"); // objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
                }
                else
                {
                    DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty,0 as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as Updatedon from tb_ProductFabricCode Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @" and Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%' AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @" and Code like '%" + txtSearch.Text.ToString().Trim().Replace("'", "''") + @"%')   
      Union All  
  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty,(select isnull(ConfigValue,0) from tb_AppConfig where ConfigName='VendorAddDays' and storeid=1) as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as updatedon From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.FabricTypeID = " + ddlFabricType.SelectedValue.ToString() + @" and  tb_FabricVendorPortal.Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%'  
  Order by FabricCodeId");
                    // DsFabricCode = objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
                }
            }
            else
            {
                if (Session["VendorLogin"] != null)
                {
                    DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty,0 as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as Updatedon from tb_ProductFabricCode Where Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%' AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where Code like '%" + txtSearch.Text.ToString().Trim().Replace("'", "''") + @"%')  AND Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0)
      Union All  
  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty,(select isnull(ConfigValue,0) from tb_AppConfig where ConfigName='VendorAddDays' and storeid=1) as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as updatedon From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.Code in (SELECT isnull(FabricCode,'') FROM tb_Product WHERE patindex('%," + Session["AdminvendorId"].ToString() + @",%',','+tb_Product.FabricVendorIds+',') > 0) AND tb_FabricVendorPortal.Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%'  
  Order by FabricCodeId"); // objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
                }
                else
                {
                    DsFabricCode = CommonComponent.GetCommonDataSet(@"Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty,0 as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as Updatedon from tb_ProductFabricCode Where Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%' AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>''    
  and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where Code like '%" + txtSearch.Text.ToString().Trim().Replace("'", "''") + @"%')   
      Union All  
  Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty,(select isnull(ConfigValue,0) from tb_AppConfig where ConfigName='VendorAddDays' and storeid=1) as deliverydays,'' as Leadtime1,'' as order1qty,'' as Order1date,'' as Production1date,'' as Leadtime2,'' as order2qty,'' as Order2date,'' as Production2date,'' as Leadtime3,'' as order3qty,'' as Order3date,'' as Production3date,'' as Leadtime4,'' as order4qty,'' as Order4date,'' as Production4date,isnull(yardprice,0) as yardprice,0 as FabricOrderId,'' as code1,0 as QtyBoookedNAV,'' as ProductionDate,0 as poqty,'' as updatedon From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode ON tb_ProductFabricCode.FabricCodeId = tb_FabricVendorPortal.FabricCodeId Where  tb_FabricVendorPortal.Code like '%" + txtSearch.Text.ToString().Replace("'", "''") + @"%'  
  Order by FabricCodeId");
                    // DsFabricCode = objProduct.GetFabricVendorPortalDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
                }

            }
            ltrCalenScript.Text = "";
            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            {
                // btnExport.Visible = true;
                btnExport.Visible = true;
                Session["FabricVendor"] = DsFabricCode;
                DataSet dss = new DataSet();

                dss = bindFabricCodeDetails(DsFabricCode);
                Session["FabricVendor1"] = dss;
                DataView dv = new DataView();
                dv = dss.Tables[0].DefaultView;
                dv.Sort = "FabricCodeId ASC";
                dv.ToTable();


                grdVendorPortal.DataSource = dv.ToTable();
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
                Session["FabricVendor"] = null;
                Session["FabricVendor1"] = null;
                grdVendorPortal.DataSource = null;
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = false;
            }
        }

        //Export Product

        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }



        private void preparedataforexport()
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["FabricVendor"];
            DataSet dsfabric = ds.Clone();


            DataSet dsdetails = new DataSet();
            dsdetails = (DataSet)Session["FabricVendor1"];

            string order1qty = "";
            string order2qty = "";
            string order3qty = "";
            string order4qty = "";
            string code = "";
            string name = "";
            string Production2date = "";
            string Production3date = "";
            string Production4date = "";
            int flag = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                code = ds.Tables[0].Rows[i]["Code"].ToString();
                name = ds.Tables[0].Rows[i]["name"].ToString();

                DataRow[] dr = dsdetails.Tables[0].Select("code ='" + code + "'");
                if (dr.Length > 0)
                {
                    for (int j = 0; j < dr.Length; j++)
                    {
                        if (dr[j]["code1"].ToString() == code)
                        {
                            order1qty = dr[j]["QtyOnhand"].ToString();
                        }
                        else if (dr[j]["code1"].ToString().IndexOf("_1") > -1)
                        {
                            order2qty = dr[j]["QtyOnhand"].ToString();
                            if (!String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()) && dr[j]["Productiondate"].ToString().IndexOf("1/1/1900") > -1)
                            {
                                Production2date = "";
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()))
                                {
                                    Production2date = "";
                                }
                                else
                                {
                                    Production2date = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr[j]["Productiondate"].ToString()));
                                }
                            }

                        }
                        else if (dr[j]["code1"].ToString().IndexOf("_2") > -1)
                        {
                            order3qty = dr[j]["QtyOnhand"].ToString();
                            if (!String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()) && dr[j]["Productiondate"].ToString().IndexOf("1/1/1900") > -1)
                            {
                                Production3date = "";
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()))
                                {
                                    Production3date = "";
                                }
                                else
                                {
                                    Production3date = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr[j]["Productiondate"].ToString()));
                                }
                            }

                        }
                        else if (dr[j]["code1"].ToString().IndexOf("_3") > -1)
                        {
                            order4qty = dr[j]["QtyOnhand"].ToString();
                            if (!String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()) && dr[j]["Productiondate"].ToString().IndexOf("1/1/1900") > -1)
                            {
                                Production4date = "";
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(dr[j]["Productiondate"].ToString()))
                                {
                                    Production4date = "";
                                }
                                else
                                {
                                    Production4date = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dr[j]["Productiondate"].ToString()));
                                }

                            }

                        }
                    }


                    DataRow drr = dsfabric.Tables[0].NewRow();
                    drr["Code"] = code.ToString();
                    drr["Name"] = name.ToString();
                    drr["order1qty"] = order1qty.ToString();
                    drr["order2qty"] = order2qty.ToString();
                    drr["Production2date"] = Production2date.ToString();
                    drr["order3qty"] = order3qty.ToString();
                    drr["Production3date"] = Production3date.ToString();
                    drr["order4qty"] = order4qty.ToString();
                    drr["Production4date"] = Production4date.ToString();

                    dsfabric.Tables[0].Rows.Add(drr);
                    dsfabric.Tables[0].AcceptChanges();
                    order1qty = "";
                    order2qty = "";
                    order3qty = "";
                    order4qty = "";
                    code = "";
                    name = "";
                    Production2date = "";
                    Production3date = "";
                    Production4date = "";
                    flag = 0;


                }

                //Label lblqtyonhand = (Label)grdVendorPortal.Rows[i].FindControl("lblqtyonhand");
                //Label lblCode = (Label)grdVendorPortal.Rows[i].FindControl("lblCode");
                //Label lblorder = (Label)grdVendorPortal.Rows[i].FindControl("lblorder");
                //Label lblproductiondate = (Label)grdVendorPortal.Rows[i].FindControl("lblproductiondate");
                //Label lblName = (Label)grdVendorPortal.Rows[i].FindControl("lblName");

                //code = lblCode.Text;
                //name = lblName.Text;
                //if (lblorder.Visible==false)
                //{
                //    order1qty = lblqtyonhand.Text;
                //    flag++;
                //}
                //else if(lblorder.Text.IndexOf("Order 1")>-1)
                //{
                //    order2qty = lblqtyonhand.Text;
                //    flag++;
                //}
                //else if (lblorder.Text.IndexOf("Order 2") > -1)
                //{
                //    order3qty = lblqtyonhand.Text;
                //    flag++;
                //}
                //else if (lblorder.Text.IndexOf("Order 3") > -1)
                //{
                //    order4qty = lblqtyonhand.Text;
                //    flag++;
                //}


                // if (lblorder.Text.IndexOf("Order 1") > -1)
                //{
                //    Production2date = lblproductiondate.Text;
                //}
                //else if (lblorder.Text.IndexOf("Order 2") > -1)
                //{
                //    Production3date = lblproductiondate.Text;
                //}
                //else if (lblorder.Text.IndexOf("Order 3") > -1)
                //{
                //    Production4date = lblproductiondate.Text;
                //}






            }
            Session["FabricVendor"] = dsfabric;

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            preparedataforexport();
            if (Session["FabricVendor"] != null)
            {
                CommonComponent clsCommon = new CommonComponent();
                DataView dvCust = new DataView();

                DataSet ds = new DataSet();
                ds = (DataSet)Session["FabricVendor"];

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dvCust = ds.Tables[0].DefaultView;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (dvCust != null)
                    {
                        for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                        {
                            object[] args = new object[9];
                            args[0] = Convert.ToString(dvCust.Table.Rows[i]["Code"]);
                            args[1] = Convert.ToString(dvCust.Table.Rows[i]["Name"].ToString());
                            args[2] = Convert.ToString(dvCust.Table.Rows[i]["order1qty"].ToString());
                            args[3] = Convert.ToString(dvCust.Table.Rows[i]["order2qty"].ToString());

                            args[4] = Convert.ToString(dvCust.Table.Rows[i]["Production2date"].ToString());

                            args[5] = Convert.ToString(dvCust.Table.Rows[i]["order3qty"].ToString());
                            args[6] = Convert.ToString(dvCust.Table.Rows[i]["Production3date"].ToString());
                            args[7] = Convert.ToString(dvCust.Table.Rows[i]["order4qty"].ToString());
                            args[8] = Convert.ToString(dvCust.Table.Rows[i]["Production4date"].ToString());





                            sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"", args));
                        }
                    }

                    if (!String.IsNullOrEmpty(sb.ToString()))
                    {

                        DateTime dt = DateTime.Now;
                        String FileName = "FabricVendorProducts_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                        string FullString = sb.ToString();
                        sb.Remove(0, sb.Length);
                        //       sb.AppendLine("Fabric Code,Name,Safety Lock,Min. Alert Qty,Delivery Days,Per yard cost,Qty on hand1,Balance Qty1,Prod. Date1,Qty on hand2,Balance Qty2,Prod. Date2,Qty on hand3,Balance Qty3,Prod. Date3,Qty on hand4,Balance Qty4,Prod. Date4");
                        sb.AppendLine("Fabric Code,Name,Qty On Hand / Order,Qty on Order 1,Prod. Date 1,Qty on Order 2,Prod. Date 2,Qty on Order 3,Prod. Date 3");
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

            }
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMsg.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName);
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
                    if (InsertDataInDataBase(dtCSV) && lblMsg.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMsg.Text = "Product Imported Successfully";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                        lblMsg.Visible = true;
                        return;

                    }


                }
                else
                {
                    lblMsg.Text += "Sorry file not found. Please retry uploading.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                    lblMsg.Visible = true;
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

        private bool InsertDataInDataBase(DataTable dt)
        {


            if (dt.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("DELETE FROM tb_importfabriccode");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strqty1 = "0";


                    string strqty2 = "0";

                    string strqty3 = "0";

                    string strqty4 = "0";
                    string yardprice = "0";
                    string days = "0";
                    string minqty = "0";
                    string bal1 = "0";
                    string bal2 = "0";
                    string bal3 = "0";
                    string bal4 = "0";

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Qty on hand1"].ToString()))
                    {
                        strqty1 = dt.Rows[i]["Qty on hand1"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Qty on hand2"].ToString()))
                    {
                        strqty2 = dt.Rows[i]["Qty on hand2"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Qty on hand3"].ToString()))
                    {
                        strqty3 = dt.Rows[i]["Qty on hand3"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Qty on hand4"].ToString()))
                    {
                        strqty4 = dt.Rows[i]["Qty on hand4"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Per yard cost"].ToString()))
                    {
                        yardprice = dt.Rows[i]["Per yard cost"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Min. Alert Qty"].ToString()))
                    {
                        minqty = dt.Rows[i]["Min. Alert Qty"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Delivery Days"].ToString()))
                    {
                        days = dt.Rows[i]["Delivery Days"].ToString();
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Balance Qty1"].ToString()))
                    {
                        bal1 = dt.Rows[i]["Balance Qty1"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Balance Qty2"].ToString()))
                    {
                        bal2 = dt.Rows[i]["Balance Qty2"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Balance Qty3"].ToString()))
                    {
                        bal3 = dt.Rows[i]["Balance Qty3"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Balance Qty4"].ToString()))
                    {
                        bal4 = dt.Rows[i]["Balance Qty4"].ToString();
                    }

                    //if (String.IsNullOrEmpty(dt.Rows[i]["SalePrice"].ToString()))
                    //{
                    //    dt.Rows[i]["SalePrice"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["BaseCustomPrice"].ToString()))
                    //{
                    //    dt.Rows[i]["BaseCustomPrice"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["DisplayOrder"].ToString()))
                    //{
                    //    dt.Rows[i]["DisplayOrder"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["Weight"].ToString()))
                    //{
                    //    dt.Rows[i]["Weight"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //try
                    //{
                    //    CommonComponent.ExecuteCommonData("update tb_product set SalePrice=" + dt.Rows[i]["SalePrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and SKU = '" + dt.Rows[i]["SKU"].ToString() + "'");
                    //    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set VariantPrice=" + dt.Rows[i]["SalePrice"].ToString() + ", BaseCustomPrice=" + dt.Rows[i]["BaseCustomPrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where  SKU = '" + dt.Rows[i]["SKU"].ToString() + "' and ProductID in (select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0)");
                    //}
                    //catch { }




                    string strquery = "INSERT INTO tb_importfabriccode(Code, Name,  order1qty, balanceqty1, production1date, order2qty,balanceqty2,  production2date, order3qty,balanceqty3,  production3date, order4qty, balanceqty4, production4date,minqty, deliverydays,yardprice) VALUES (";
                    strquery += "'" + dt.Rows[i]["Fabric Code"].ToString().Replace("'", "''") + "','" + dt.Rows[i]["Name"].ToString().Replace("'", "''") + "'," + strqty1 + "," + bal1 + ",'" + dt.Rows[i]["Prod. Date1"].ToString().Replace("'", "''") + "'," + strqty2 + "," + bal2 + ",'" + dt.Rows[i]["Prod. Date2"].ToString().Replace("'", "''") + "'," + strqty3 + "," + bal3 + ",'" + dt.Rows[i]["Prod. Date3"].ToString().Replace("'", "''") + "'," + strqty4 + "," + bal4 + ",'" + dt.Rows[i]["Prod. Date4"].ToString().Replace("'", "''") + "'," + minqty + "," + days + "," + yardprice + "";
                    strquery += ")";
                    CommonComponent.ExecuteCommonData(strquery);
                }

                DataSet dsdat = new DataSet();

                dsdat = CommonComponent.GetCommonDataSet("SELECT * FROM tb_importfabriccode");
                if (dsdat != null && dsdat.Tables.Count > 0 && dsdat.Tables[0].Rows.Count > 0)
                {
                    CommonComponent.ExecuteCommonData("EXEC usp_importfabriccodes");
                }

            }
            else
            {
                return false;


            }


            return true;
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
                        if (tempFieldName == "fabric code" || tempFieldName == "name" || tempFieldName == "min. alert qty" || tempFieldName == "delivery days" || tempFieldName == "per yard cost" || tempFieldName == "qty on hand1" || tempFieldName == "balance qty1" || tempFieldName == "prod. date1" || tempFieldName == "qty on hand2" || tempFieldName == "balance qty2" || tempFieldName == "prod. date2" || tempFieldName == "qty on hand3" || tempFieldName == "balance qty3" || tempFieldName == "prod. date3" || tempFieldName == "qty on hand4" || tempFieldName == "balance qty4" || tempFieldName == "prod. date4")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",fabric code,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",min. alert qty,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",per yard cost,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",qty on hand1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",balance qty1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",prod. date1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",qty on hand2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",balance qty2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",prod. date2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",qty on hand3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",balance qty3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",prod. date3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",qty on hand4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",balance qty4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",prod. date4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "File Does not contain all columns";
                            lblMsg.Style.Add("color", "#FF0000");
                            lblMsg.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        if (ddlFabricType.SelectedIndex > 0)
                        {
                            ddlFabricType_SelectedIndexChanged(null, null);
                        }

                        //BindData();

                    }
                    else
                    {
                        //lblMsg.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    //lblMsg.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        protected void grdVendorPortal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdVendorPortal.PageIndex = e.NewPageIndex;
            GetGrid();
        }


    }
}