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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class FabricVendorOrder_Popup : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["FabricTypeId"] != null && Request.QueryString["FabricCodeId"] != null)
                {
                    string FabricTypeId = Convert.ToString(Request.QueryString["FabricTypeId"]);
                    string FabricCodeId = Convert.ToString(Request.QueryString["FabricCodeId"]);

                    if (!string.IsNullOrEmpty(FabricTypeId))
                    {
                        lblFebrictypeName.Text = "Fabric Type : <b>" + Convert.ToString(CommonComponent.GetScalarCommonData("Select Isnull(FabricTypename,'') from tb_ProductFabricType where FabricTypeID=" + FabricTypeId + "")) + "</b>";
                    }

                    FillFabricType(FabricTypeId);

                    btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                }
            }
        }

        /// <summary>
        /// Bind Fabric Type
        /// </summary>
        private void FillFabricType(string FabricTypeId)
        {
            DataSet DsFabircType = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            DsFabircType = objProduct.GetProductFabricDetails(0, 1);
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

            ddlFabricType.SelectedValue = FabricTypeId.ToString();

            ddlFabricType_SelectedIndexChanged(null, null);
        }

        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductComponent objProduct = new ProductComponent();
            DataSet DsFabricCode = new DataSet();
            if (Request.QueryString["FabricTypeId"] != null && Request.QueryString["FabricCodeId"] != null)
            {
                string StrQry = "Select 0 as FabricVendorPortId, FabricTypeID,FabricCodeId,Code,Name,0 as MinQty,0 as MinOrderQty,0 as QtyOnHand,0 as BookedQty,0 as AvailQty from tb_ProductFabricCode Where FabricTypeID = " + Convert.ToInt32(ddlFabricType.SelectedValue) + " AND ISNULL(Active,0)=1 AND ISNULL(Code,'')<>'' and FabricCodeid = " + Convert.ToInt32(Request.QueryString["FabricCodeId"].ToString()) + " " +
                                " and FabricCodeId not in (Select  FabricCodeId from  tb_FabricVendorPortal Where FabricTypeID = " + Convert.ToInt32(ddlFabricType.SelectedValue) + " and FabricCodeid=" + Convert.ToInt32(Request.QueryString["FabricCodeId"].ToString()) + ")  " +
                                " Union All  "+
                                " Select tb_FabricVendorPortal.FabricVendorPortId,tb_FabricVendorPortal.FabricTypeID,tb_FabricVendorPortal.FabricCodeId,tb_FabricVendorPortal.Code,tb_ProductFabricCode.Name,ISNULL(MinQty,0) as MinQty,ISNULL(MinOrderQty,0) as MinOrderQty,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(BookedQty,0) as BookedQty,ISNULL(AvailQty,0) as AvailQty From  tb_FabricVendorPortal INNER JOIN tb_ProductFabricCode on tb_ProductFabricCode.FabricCodeId=tb_FabricVendorPortal.FabricCodeId Where tb_FabricVendorPortal.FabricTypeID = " + Convert.ToInt32(ddlFabricType.SelectedValue) + " and tb_FabricVendorPortal.FabricCodeid= " + Convert.ToInt32(Request.QueryString["FabricCodeId"].ToString()) + " " +
                                " Order by FabricCodeId";

                //DsFabricCode = objProduct.GetFabricVendorPortalDetailsForPopup(Convert.ToInt32(ddlFabricType.SelectedValue), Convert.ToInt32(Request.QueryString["FabricCodeId"].ToString()), 4);
                DsFabricCode = CommonComponent.GetCommonDataSet(StrQry.ToString());
            }
            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            {
                grdVendorPortal.DataSource = DsFabricCode;
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = true;
            }
            else
            {
                grdVendorPortal.DataSource = null;
                grdVendorPortal.DataBind();
                trFabricDetails.Visible = false;
            }
        }

        protected void grdVendorPortal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblFabricCodeId = (Label)e.Row.FindControl("lblFabricCodeId");
                Label lblFabricVendorPortId = (Label)e.Row.FindControl("lblFabricVendorPortId");

                GridView grdVendorOrder = (GridView)e.Row.FindControl("grdVendorOrder");

                ProductComponent objProduct = new ProductComponent();
                DataSet DsFabricCode = new DataSet();

                DsFabricCode = CommonComponent.GetCommonDataSet("Select FabricOrderId,FabricVendorPortId, FabricTypeID,FabricCodeId,Code,'' as Name,ISNULL(QtyinYard,0) as QtyinYard,ISNULL(QtyBoookedinYard,0) QtyBoookedinYard,ISNULL(BalanceOrder,0) as BalanceOrder, OrderDate,ProductionDate,ISNULL(QtyReceived,0) as QtyReceived from tb_FabricVendorOrder Where FabricTypeID = " + ddlFabricType.SelectedValue + " and  FabricCodeId=" + lblFabricCodeId.Text.ToString() + " Order by FabricCodeId");

                if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
                {

                    DataSet DsFabricCodeTemp = new DataSet();
                    if (DsFabricCode.Tables[0].Rows.Count < 4)
                    {
                        DsFabricCodeTemp = objProduct.GetFabricVendorPortalCodeDetails(Convert.ToInt32(ddlFabricType.SelectedValue), Convert.ToInt16(lblFabricCodeId.Text.ToString()), 3);
                        string vendororder = "";
                        for (int i = 0; i < DsFabricCode.Tables[0].Rows.Count; i++)
                        {
                            vendororder += "'" + DsFabricCode.Tables[0].Rows[i]["Code"].ToString() + "',";
                        }
                        if (vendororder.Length > 0)
                        {
                            vendororder = vendororder.Substring(0, vendororder.Length - 1);
                        }
                        if (DsFabricCodeTemp != null && DsFabricCodeTemp.Tables.Count > 0 && DsFabricCodeTemp.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] dr = DsFabricCodeTemp.Tables[0].Select("Code in (" + vendororder + ")");
                            //foreach(DataRow dr1 in dr)
                            //{
                            Int32 tt = DsFabricCode.Tables[0].Rows.Count;
                            for (int i = 0; i < (4 - tt); i++)
                            {
                                DsFabricCode.Tables[0].Rows.Add(dr[0].ItemArray);
                                
                            }
                            DsFabricCode.Tables[0].AcceptChanges();


                        }
                    }

                    grdVendorOrder.DataSource = DsFabricCode;
                    grdVendorOrder.DataBind();
                    if (DsFabricCode.Tables[0].Rows[0]["FabricOrderId"].ToString() == "0")
                    {
                        for (int i = 0; i < grdVendorOrder.Rows.Count; i++)
                        {
                            grdVendorOrder.Rows[i].Cells[grdVendorOrder.Columns.Count - 1].Visible = false;
                        }
                        grdVendorOrder.HeaderRow.Cells[grdVendorOrder.Columns.Count - 1].Visible = false;
                    }
                    else
                    {
                        for (int i = 0; i < grdVendorOrder.Rows.Count; i++)
                        {
                            grdVendorOrder.Rows[i].Cells[grdVendorOrder.Columns.Count - 1].Visible = true;
                        }
                        grdVendorOrder.HeaderRow.Cells[grdVendorOrder.Columns.Count - 1].Visible = true;
                    }
                }
                else
                {
                    DsFabricCode = objProduct.GetFabricVendorPortalCodeDetails(Convert.ToInt32(ddlFabricType.SelectedValue), Convert.ToInt16(lblFabricCodeId.Text.ToString()), 3);
                    if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
                    {
                        grdVendorOrder.DataSource = DsFabricCode;
                        grdVendorOrder.DataBind();

                        if (DsFabricCode.Tables[0].Rows[0]["FabricOrderId"].ToString() == "0")
                        {
                            for (int i = 0; i < grdVendorOrder.Rows.Count; i++)
                            {
                                grdVendorOrder.Rows[i].Cells[grdVendorOrder.Columns.Count - 1].Visible = false;
                            }
                            grdVendorOrder.HeaderRow.Cells[grdVendorOrder.Columns.Count - 1].Visible = false;
                        }
                        else
                        {
                            for (int i = 0; i < grdVendorOrder.Rows.Count; i++)
                            {
                                grdVendorOrder.Rows[i].Cells[grdVendorOrder.Columns.Count - 1].Visible = true;
                            }
                            grdVendorOrder.HeaderRow.Cells[grdVendorOrder.Columns.Count - 1].Visible = true;
                        }
                    }
                }
            }
        }

        protected void grdVendorPortal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "_delete")
            {
                int FabricOrderId = Convert.ToInt32(e.CommandArgument);
                for (int i = 0; i < grdVendorPortal.Rows.Count; i++)
                {
                    GridView grdVendorOrder = (GridView)grdVendorPortal.Rows[i].FindControl("grdVendorOrder");
                    Label lblCode = (Label)grdVendorPortal.Rows[i].FindControl("lblCode");
                    if (grdVendorOrder.Rows.Count > 0)
                    {
                        for (int k = 0; k < grdVendorOrder.Rows.Count; k++)
                        {
                            Label lblFabricOrderId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricOrderId");
                            if (lblFabricOrderId != null && Convert.ToInt32(lblFabricOrderId.Text.ToString()) == FabricOrderId)
                            {
                                Label lblFabricCodeId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricCodeId");
                                Label lblFabricVendorPortId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricVendorPortId");

                                Int32 QtyinYard = 0, QtyBoookedinYard = 0, BalanceOrder = 0, QtyReceived = 0;
                                string StrInsert = "";

                                StrInsert = "UPDATE [tb_FabricVendorOrder] set [QtyinYard] = " + QtyinYard + ",[QtyBoookedinYard] = " + QtyBoookedinYard + ",[BalanceOrder] = " + BalanceOrder + ",[QtyReceived] = " + QtyReceived + ",[OrderDate] = NULL,[ProductionDate] = NULL WHERE FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                                CommonComponent.ExecuteCommonData(StrInsert.ToString());
                                try
                                {
                                    StrInsert = "INSERT INTO [tb_FabricVendorOrderLog]([FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn],[Deleted]) " +
                                        " Select [FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],ISNULL(QtyinYard,0) QtyinYard ,ISNULL(QtyBoookedinYard,0) QtyBoookedinYard,ISNULL(BalanceOrder,0) as BalanceOrder,[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn],1 from [tb_FabricVendorOrderLog] Where FabricOrderId=" + FabricOrderId + " Select SCOPE_IDENTITY();";
                                    FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));
                                }
                                catch { }
                            }
                        }
                    }
                }
                ddlFabricType_SelectedIndexChanged(null, null);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Record Deleted Successfully','Success');", true);
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToInt32(ddlFabricType.SelectedValue) > 0)
            {
                if (grdVendorPortal.Rows.Count > 0)
                {
                    for (int i = 0; i < grdVendorPortal.Rows.Count; i++)
                    {
                        GridView grdVendorOrder = (GridView)grdVendorPortal.Rows[i].FindControl("grdVendorOrder");
                        Label lblCode = (Label)grdVendorPortal.Rows[i].FindControl("lblCode");
                        if (grdVendorOrder.Rows.Count > 0)
                        {
                            for (int k = 0; k < grdVendorOrder.Rows.Count; k++)
                            {
                                TextBox txtQtyinYard = (TextBox)grdVendorOrder.Rows[k].FindControl("txtQtyinYard");
                                TextBox txtQtyBoookedinYard = (TextBox)grdVendorOrder.Rows[k].FindControl("txtQtyBoookedinYard");
                                TextBox txtBalanceOrder = (TextBox)grdVendorOrder.Rows[k].FindControl("txtBalanceOrder");
                                TextBox txtQtyReceived = (TextBox)grdVendorOrder.Rows[k].FindControl("txtQtyReceived");
                                Label lblVendorOrderNum = (Label)grdVendorOrder.Rows[k].FindControl("lblVendorOrderNum");
                                TextBox txtOrderDate = (TextBox)grdVendorOrder.Rows[k].FindControl("txtOrderDate");
                                TextBox txtProductionDate = (TextBox)grdVendorOrder.Rows[k].FindControl("txtProductionDate");

                                Label lblFabricCodeId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricCodeId");
                                Label lblFabricVendorPortId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricVendorPortId");

                                Label lblFabricOrderId = (Label)grdVendorOrder.Rows[k].FindControl("lblFabricOrderId");

                                Int32 QtyinYard = 0, QtyBoookedinYard = 0, BalanceOrder = 0, QtyReceived = 0;
                                int.TryParse(txtQtyinYard.Text.ToString(), out QtyinYard);
                                int.TryParse(txtQtyBoookedinYard.Text.ToString(), out QtyBoookedinYard);
                                int.TryParse(txtBalanceOrder.Text.ToString(), out BalanceOrder);
                                int.TryParse(txtQtyReceived.Text.ToString(), out QtyReceived);

                                string StrInsert = "";
                                Int32 FabricOrderId = 0;
                                if (lblFabricOrderId.Text.ToString().Trim() == "0")
                                {
                                    StrInsert = "INSERT INTO [tb_FabricVendorOrder]([FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn]) " +
                                    " VALUES(" + lblFabricVendorPortId.Text.ToString() + "," + ddlFabricType.SelectedValue + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "','" + lblVendorOrderNum.Text.ToString().Replace("'", "''") + "'," + QtyinYard + "," + QtyBoookedinYard + "," + BalanceOrder + ",'" + txtOrderDate.Text + "','" + txtProductionDate.Text + "'," + QtyReceived + "," + Session["AdminId"].ToString() + ",Getdate()); Select SCOPE_IDENTITY();";
                                    FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));
                                }
                                else
                                {
                                    // Update
                                    FabricOrderId = Convert.ToInt32(lblFabricOrderId.Text.ToString().Trim());
                                    StrInsert = "UPDATE [tb_FabricVendorOrder] set [QtyinYard] = " + QtyinYard + ",[QtyBoookedinYard] = " + QtyBoookedinYard + ",[BalanceOrder] = " + BalanceOrder + ",[QtyReceived] = " + QtyReceived + ",[OrderDate] = '" + txtOrderDate.Text + "',[ProductionDate] = '" + txtProductionDate.Text + "' WHERE FabricOrderId=" + lblFabricOrderId.Text.ToString() + "";
                                    CommonComponent.ExecuteCommonData(StrInsert.ToString());
                                }

                                try
                                {
                                    StrInsert = "INSERT INTO [tb_FabricVendorOrderLog]([FabricOrderId],[FabricVendorPortId],[FabricTypeID],[FabricCodeId],[Code],[VendorOrderNumber],[QtyinYard],[QtyBoookedinYard],[BalanceOrder],[OrderDate],[ProductionDate],[QtyReceived],[CreatedBy],[CreatedOn]) " +
                                        " VALUES(" + FabricOrderId + "," + lblFabricVendorPortId.Text.ToString() + "," + ddlFabricType.SelectedValue + "," + lblFabricCodeId.Text + ",'" + lblCode.Text.ToString() + "','" + lblVendorOrderNum.Text.ToString().Replace("'", "''") + "'," + QtyinYard + "," + QtyBoookedinYard + "," + BalanceOrder + ",'" + txtOrderDate.Text + "','" + txtProductionDate.Text + "'," + QtyReceived + "," + Session["AdminId"].ToString() + ",Getdate()); Select SCOPE_IDENTITY();";
                                    FabricOrderId = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrInsert.ToString()));
                                }
                                catch { }
                            }
                        }
                    }
                    ddlFabricType_SelectedIndexChanged(null, null);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "jAlert('Record Saved Successfully','Success');window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@validf", "jAlert('Please Select Fabric Category Name','Message');", true);
                return;
            }
        }

        protected void grdVendorOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Funname = "";
                string strCode = ((Label)e.Row.Parent.Parent.Parent.FindControl("lblCode")).Text;

                Label lblFabricCodeId = (Label)e.Row.FindControl("lblFabricCodeId");
                Label lblFabricOrderId = (Label)e.Row.FindControl("lblFabricOrderId");
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                TextBox txtQtyinYard = (TextBox)e.Row.FindControl("txtQtyinYard");
                TextBox txtQtyBoookedinYard = (TextBox)e.Row.FindControl("txtQtyBoookedinYard");
                TextBox txtBalanceOrder = (TextBox)e.Row.FindControl("txtBalanceOrder");
                Label lblVendorOrderNum = (Label)e.Row.FindControl("lblVendorOrderNum");
                TextBox txtOrderDate = (TextBox)e.Row.FindControl("txtOrderDate");
                TextBox txtProductionDate = (TextBox)e.Row.FindControl("txtProductionDate");

                if (String.IsNullOrEmpty(txtOrderDate.Text) || txtOrderDate.Text.ToString().ToLower().IndexOf("1900") > -1)
                    txtOrderDate.Text = "";
                if (String.IsNullOrEmpty(txtProductionDate.Text) || txtProductionDate.Text.ToString().ToLower().IndexOf("1900") > -1)
                    txtProductionDate.Text = "";

                txtQtyinYard.Attributes.Add("onkeyup", "ClacuOnHandQty('" + txtQtyinYard.ClientID + "','" + txtQtyBoookedinYard.ClientID + "','" + txtBalanceOrder.ClientID + "');");
                txtQtyBoookedinYard.Attributes.Add("onkeyup", "ClacuOnHandQty('" + txtQtyinYard.ClientID + "','" + txtQtyBoookedinYard.ClientID + "','" + txtBalanceOrder.ClientID + "');");
                txtBalanceOrder.Attributes.Add("readonly", "true");

                lblVendorOrderNum.Text = strCode + "_" + (e.Row.RowIndex + 1);

                Funname += "<script type=\"text/javascript\">";
                Funname += " $(function () {$('#" + txtOrderDate.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true});});";

                Funname += " $(function () {$('#" + txtProductionDate.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true});});";
                Funname += "</script>";

                ltrCalenScript.Text += Funname.ToString();

                if (lblFabricOrderId != null && lblFabricOrderId.Text.ToString() == "0")
                {
                    Int32 MaxOrdQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Isnull(MinOrderQty,0) from tb_FabricVendorPortal Where FabricCodeId= " + lblFabricCodeId.Text.ToString() + " and FabricTypeID= " + ddlFabricType.SelectedValue + ""));
                    if (MaxOrdQty > 0 && lblFabricOrderId.Text.ToString() == "0")
                    {
                        txtQtyinYard.Text = MaxOrdQty.ToString();
                    }
                }
            }
        }
    }
}