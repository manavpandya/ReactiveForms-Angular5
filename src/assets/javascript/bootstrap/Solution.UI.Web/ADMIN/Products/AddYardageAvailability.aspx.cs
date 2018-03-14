using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddYardageAvailability : BasePage
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
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                BindFabricType();
            }
            ddlFabricType.Focus();
        }

        /// <summary>
        /// Bind store dropdown
        /// </summary>
        private void BindFabricType()
        {
            DataSet dsFabric = new DataSet();
            dsFabric = CommonComponent.GetCommonDataSet("Select FabricTypeID,FabricTypename from tb_ProductFabricType Where ISNULL(Active,0)=1 Order by ISNULL(DisplayOrder,999)");
            if (dsFabric != null && dsFabric.Tables.Count > 0 && dsFabric.Tables[0].Rows.Count > 0)
            {
                ddlFabricType.DataSource = dsFabric;
                ddlFabricType.DataTextField = "FabricTypename";
                ddlFabricType.DataValueField = "FabricTypeID";
                ddlFabricType.DataBind();
            }
            ddlFabricType.Items.Insert(0, new ListItem("Select Fabric Type", "0"));
            if (dsFabric.Tables[0].Rows.Count > 1)
            {
                ddlFabricType.SelectedIndex = 1;
            }
            else
            {
                ddlFabricType.SelectedIndex = 0;
            }
            ddlFabricType_SelectedIndexChanged(null, null);
        }

        protected void grdProductStyleType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAvailableDate = (Label)e.Row.FindControl("lblAvailableDate");
                TextBox txtAvailableDate = (TextBox)e.Row.FindControl("txtAvailableDate");

                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                ((ImageButton)e.Row.FindControl("btnSave")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.png";
                ((ImageButton)e.Row.FindControl("btnCancel")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/CloseIcon.png";

                if (!string.IsNullOrEmpty(lblAvailableDate.Text))
                {
                    if (lblAvailableDate.Text.ToString().ToLower().IndexOf("1900") > -1)
                        txtAvailableDate.Text = "";
                    else
                        txtAvailableDate.Text = SetShortDate(lblAvailableDate.Text.ToString());
                }
            }
        }

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="StateID">int StateID</param>
        private void BindData(Int32 FabricTypeID)
        {
            DataSet dsSearchpro = new DataSet();
            string StrQuery = "Select 0  as NoOfDays,FabricCodeID,code,0 as AllowQty,'' as Width,0 as QtyOnHand,0 as NextOrderQty,0 as AlertQty,'' as AvailableDate from tb_ProductFabricCode where FabricTypeID = " + FabricTypeID + "" +
                            " AND FabricCodeId not in (Select tb_ProductFabricWidth.FabricCodeID from tb_ProductFabricWidth INNER JOIN tb_ProductFabricCode ON   tb_ProductFabricWidth.FabricCodeID = tb_ProductFabricCode.FabricCodeID " +
                            " where tb_ProductFabricCode.FabricTypeID = " + FabricTypeID + " ) AND ISNULL(tb_ProductFabricCode.Active,0)=1 " +
                            " union " +
                            " Select ISNULL(NoOfDays,0) as NoOfDays,tb_ProductFabricCode.FabricCodeID,code,ISNULL(AllowQty,0) as AllowQty,ISNULL(Width,'') as Width,ISNULL(QtyOnHand,0) as QtyOnHand,ISNULL(NextOrderQty,0) as NextOrderQty,ISNULL(AlertQty,0) as AlertQty,AvailableDate " +
                            " from tb_ProductFabricWidth INNER JOIN tb_ProductFabricCode ON   tb_ProductFabricWidth.FabricCodeID = tb_ProductFabricCode.FabricCodeID " +
                            " where tb_ProductFabricCode.FabricTypeID = " + FabricTypeID + " AND ISNULL(tb_ProductFabricCode.Active,0)=1 ";

            dsSearchpro = CommonComponent.GetCommonDataSet(StrQuery);
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                trFabricDetails.Visible = true;
                grdProductStyleType.DataSource = dsSearchpro;
                grdProductStyleType.DataBind();
                imgSave.Visible = true;
                trTop.Visible = true;
                trBottom.Visible = true;
            }
            else
            {
                trFabricDetails.Visible = true;
                grdProductStyleType.DataSource = null;
                grdProductStyleType.DataBind();
                imgSave.Visible = false;
                trTop.Visible = false;
                trBottom.Visible = false;
            }
            if (ddlFabricType.SelectedIndex == 0)
            {
                trFabricDetails.Visible = false;
            }
        }

        /// <summary>
        /// Set Short Date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <returns>Short Date String</returns>
        public string SetShortDate(string Date)
        {
            try
            {
                DateTime date = Convert.ToDateTime(Date);
                return string.Format("{0:MM/dd/yyyy}", date);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlFabricType.SelectedIndex > 0 && grdProductStyleType.Rows.Count > 0)
            {
                decimal QtyOnHand = 0, NextOrderQty = 0, AlertQty = 0;
                int AllowQty = 0, NoOfDays = 0;
                string Width = "";
                string AvailableDate = "";
                int Cnt = 0;
                for (int i = 0; i < grdProductStyleType.Rows.Count; i++)
                {
                    Label lblCode = (Label)grdProductStyleType.Rows[i].FindControl("lblCode");
                    Label lblFabricCodeId = (Label)grdProductStyleType.Rows[i].FindControl("lblFabricCodeId");
                    CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                    TextBox txtWidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtWidth");
                    TextBox txtQtyOnHand = (TextBox)grdProductStyleType.Rows[i].FindControl("txtQtyOnHand");
                    TextBox txtAllowQty = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAllowQty");
                    TextBox txtNextOrderQty = (TextBox)grdProductStyleType.Rows[i].FindControl("txtNextOrderQty");
                    TextBox txtAlertQty = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAlertQty");
                    TextBox txtAvailableDate = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAvailableDate");
                    TextBox txtNoOfDays = (TextBox)grdProductStyleType.Rows[i].FindControl("txtNoOfDays");

                    int FabricCodeID = Convert.ToInt32(lblFabricCodeId.Text.ToString());

                    Int32.TryParse(txtNoOfDays.Text.ToString(), out NoOfDays);

                    if (!string.IsNullOrEmpty(txtWidth.Text.ToString()))
                        Width = Convert.ToString(txtWidth.Text.ToString());

                    if (!string.IsNullOrEmpty(txtQtyOnHand.Text.ToString()))
                        QtyOnHand = Convert.ToDecimal(txtQtyOnHand.Text.ToString());

                    if (!string.IsNullOrEmpty(txtAllowQty.Text.ToString()))
                        AllowQty = Convert.ToInt32(txtAllowQty.Text.ToString());

                    if (!string.IsNullOrEmpty(txtNextOrderQty.Text.ToString()))
                        NextOrderQty = Convert.ToDecimal(txtNextOrderQty.Text.ToString());

                    Int32.TryParse(txtAllowQty.Text.ToString(), out AllowQty);
                    if (AllowQty == 0)
                    {
                        AllowQty = Convert.ToInt32(QtyOnHand + NextOrderQty);
                    }

                    if (!string.IsNullOrEmpty(txtAvailableDate.Text.Trim().ToString()))
                        AvailableDate = Convert.ToString(txtAvailableDate.Text);
                    else
                        AvailableDate = null;

                    if (ChkActive.Checked)
                    {
                        Cnt++;
                        Int32 Totcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Totcnt from tb_ProductFabricWidth where FabricCodeID = " + FabricCodeID + ""));
                        if (Totcnt > 0)
                        {
                            CommonComponent.ExecuteCommonData("Update tb_ProductFabricWidth set NoOfDays=" + NoOfDays + ",AllowQty=" + AllowQty + ",Width='" + Width.Replace("'", "''") + "',QtyOnHand=" + QtyOnHand + ",NextOrderQty=" + NextOrderQty + ",AlertQty=" + AlertQty + ",AvailableDate ='" + AvailableDate + "' where FabricCodeID=" + FabricCodeID + "");
                        }
                        else
                        {
                            Int32 FabricWeightId = Convert.ToInt32(CommonComponent.ExecuteCommonData("Insert into tb_ProductFabricWidth (FabricCodeID,Width,QtyOnHand,NextOrderQty,AlertQty,Active,AvailableDate,CreatedBy,CreatedOn,AllowQty,NoOfDays) " +
                                                        " values(" + FabricCodeID + ",'" + Width.Replace("'", "''") + "'," + QtyOnHand + "," + NextOrderQty + "," + AlertQty + ",1,'" + AvailableDate + "'," + Convert.ToInt32(Session["AdminID"]) + ",getdate()," + AllowQty + "," + NoOfDays + "); Select Scope_identity()"));
                        }
                    }
                }
                if (Cnt == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "jAlert('Select At least One Record(s)!','Message');", true);
                    return;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "jAlert('Record Updated Successfully','Message');", true);
                }
                BindData(Convert.ToInt32(ddlFabricType.SelectedValue));
            }
            else
            {
                grdProductStyleType.DataSource = null;
                grdProductStyleType.DataBind();
                imgSave.Visible = false;
                trTop.Visible = false;
                trBottom.Visible = false;
            }
        }

        /// <summary>
        /// Fabric Type Selected
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFabricType.SelectedIndex > 0)
            {
                BindData(Convert.ToInt32(ddlFabricType.SelectedValue));
            }
            else
            {
                trFabricDetails.Visible = false;
                grdProductStyleType.DataSource = null;
                grdProductStyleType.DataBind();
                imgSave.Visible = false;
                trTop.Visible = false;
                trBottom.Visible = false;
            }
        }

        protected void grdProductStyleType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                Label lblCode = (Label)row.FindControl("lblCode");
                CheckBox ChkActive = (CheckBox)row.FindControl("chkActive");
                TextBox txtWidth = (TextBox)row.FindControl("txtWidth");
                TextBox txtQtyOnHand = (TextBox)row.FindControl("txtQtyOnHand");
                ImageButton btnEditPrice = row.FindControl("_editLinkButton") as ImageButton;

                TextBox txtNextOrderQty = (TextBox)row.FindControl("txtNextOrderQty");
                TextBox txtAlertQty = (TextBox)row.FindControl("txtAlertQty");
                TextBox txtAvailableDate = (TextBox)row.FindControl("txtAvailableDate");
                TextBox txtAllowQty = (TextBox)row.FindControl("txtAllowQty");
                TextBox txtNoOfDays = (TextBox)row.FindControl("txtNoOfDays");

                if (e.CommandName == "DeleteAdmin")
                {
                    try
                    {
                        int FabricCodeID = Convert.ToInt32(e.CommandArgument);
                        CommonComponent.ExecuteCommonData("Update tb_ProductFabricWidth set Width=0,QtyOnHand=0,NextOrderQty=0,AlertQty=0,AllowQty=0,AvailableDate = NULL where FabricCodeID=" + FabricCodeID + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "alert('Record Deleted Successfully');", true);
                        BindData(Convert.ToInt32(ddlFabricType.SelectedValue));
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                else if (e.CommandName == "Save")
                {
                    decimal QtyOnHand = 0, NextOrderQty = 0, AlertQty = 0;
                    int AllowQty = 0, NoOfDays = 0;
                    string Width = "";
                    string AvailableDate = "";
                    int FabricCodeID = Convert.ToInt32(e.CommandArgument);

                    Int32.TryParse(txtNoOfDays.Text.ToString(), out NoOfDays);

                    if (!string.IsNullOrEmpty(txtWidth.Text.ToString()))
                        Width = Convert.ToString(txtWidth.Text.ToString());

                    if (!string.IsNullOrEmpty(txtQtyOnHand.Text.ToString()))
                        QtyOnHand = Convert.ToDecimal(txtQtyOnHand.Text.ToString());

                    if (!string.IsNullOrEmpty(txtNextOrderQty.Text.ToString()))
                        NextOrderQty = Convert.ToDecimal(txtNextOrderQty.Text.ToString());

                    if (!string.IsNullOrEmpty(txtAllowQty.Text.ToString()))
                        AllowQty = Convert.ToInt32(txtAllowQty.Text.ToString());

                    Int32.TryParse(txtAllowQty.Text.ToString(), out AllowQty);
                    if (AllowQty == 0)
                    {
                        AllowQty = Convert.ToInt32(QtyOnHand + NextOrderQty);
                    }

                    if (!string.IsNullOrEmpty(txtAlertQty.Text.ToString()))
                        AlertQty = Convert.ToDecimal(txtAlertQty.Text.ToString());

                    if (!string.IsNullOrEmpty(txtAvailableDate.Text.Trim().ToString()))
                        AvailableDate = Convert.ToString(txtAvailableDate.Text);
                    else
                        AvailableDate = null;

                    Int32 Totcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Totcnt from tb_ProductFabricWidth where FabricCodeID = " + FabricCodeID + ""));
                    if (Totcnt > 0)
                    {
                        CommonComponent.ExecuteCommonData("Update tb_ProductFabricWidth set NoOfDays=" + NoOfDays + ",AllowQty=" + AllowQty + ",Width='" + Width.Replace("'", "''") + "',QtyOnHand=" + QtyOnHand + ",NextOrderQty=" + NextOrderQty + ",AlertQty=" + AlertQty + ",AvailableDate ='" + AvailableDate + "' where FabricCodeID=" + FabricCodeID + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "jAlert('Record Updated Successfully','Message');", true);
                    }
                    else
                    {
                        Int32 FabricWeightId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ProductFabricWidth (FabricCodeID,Width,QtyOnHand,NextOrderQty,AlertQty,Active,AvailableDate,CreatedBy,CreatedOn,AllowQty,NoOfDays) " +
                                                     " values(" + FabricCodeID + ",'" + Width.Replace("'", "''") + "'," + QtyOnHand + "," + NextOrderQty + "," + AlertQty + ",1,'" + AvailableDate + "'," + Convert.ToInt32(Session["AdminID"]) + ",getdate()," + AllowQty + "," + NoOfDays + "); Select Scope_identity()"));
                        if (FabricWeightId > 0)
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "jAlert('Record Added Successfully','Message');", true);
                        else
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "jAlert('Error While Saving Record','Message');", true);
                    }

                    BindData(Convert.ToInt32(ddlFabricType.SelectedValue));
                }
            }
        }
    }
}