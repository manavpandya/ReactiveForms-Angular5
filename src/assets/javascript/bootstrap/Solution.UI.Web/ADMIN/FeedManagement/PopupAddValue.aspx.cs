using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class PopupAddValue : BasePage
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
                btnSubmit.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                if (Request.QueryString["FieldId"] != null && Request.QueryString["TypeId"] != null)
                {
                    hdnValueId.Value = "0";
                    BindData();
                }

                txtValue.Focus();
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        protected void BindData()
        {
            hdnValueId.Value = "0";
            DataSet dsFieldValue = new DataSet();
            FeedComponent objfeedfield = new FeedComponent();
            dsFieldValue = objfeedfield.GetFieldValueById(Convert.ToInt32(Request.QueryString["FieldId"].ToString()), Convert.ToInt32(Request.QueryString["TypeId"].ToString()));
            if (dsFieldValue != null && dsFieldValue.Tables.Count > 0 && dsFieldValue.Tables[0].Rows.Count > 0)
            {
                grdSelected.DataSource = dsFieldValue;
                grdSelected.DataBind();
                DataView dv = new DataView();
                dv = dsFieldValue.Tables[0].DefaultView;
                dv.Sort = "DisplayOrder ASC";
                dv.ToTable();
                CommonComponent.ExecuteCommonData("Update tb_FeedFieldMaster set DefautValue='" + dv.Table.Rows[0]["FieldValues"].ToString() + "' WHERE FieldID=" + Request.QueryString["FieldId"].ToString() + "");
            }
            else
            {
                grdSelected.DataSource = null;
                grdSelected.DataBind();
            }
        }

        /// <summary>
        /// Selected Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdSelected_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    Int32 ValueID = Convert.ToInt32(e.CommandArgument.ToString());
                    CommonComponent.ExecuteCommonData("Delete From tb_FeedFieldTypeValues Where ValueID=" + ValueID + "");
                    BindData();
                }
                if (e.CommandName == "EditMe")
                {
                    Int32 ValueID = Convert.ToInt32(e.CommandArgument.ToString());
                    hdnValueId.Value = ValueID.ToString();
                    DataSet dsEdit = new DataSet();
                    dsEdit = CommonComponent.GetCommonDataSet("SElect * From tb_FeedFieldTypeValues Where ValueID=" + ValueID + "");
                    if (dsEdit != null && dsEdit.Tables[0].Rows.Count > 0)
                    {
                        txtValue.Text = dsEdit.Tables[0].Rows[0]["FieldValues"].ToString();
                        txtDisplayOrder.Text = dsEdit.Tables[0].Rows[0]["DisplayOrder"].ToString();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        ///  Selected Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdSelected_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSelected.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtValue.Text.ToString()) && !String.IsNullOrEmpty(txtDisplayOrder.Text.ToString()))
            {
                if (hdnValueId.Value == "0" || string.IsNullOrEmpty(hdnValueId.Value))
                {
                    if (Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ISNULL(FieldValues,0)) as Totcnt from tb_FeedFieldTypeValues Where TypeID=" + Request.QueryString["TypeId"].ToString() + " And FieldId=" + Request.QueryString["FieldId"].ToString() + " And FieldValues='" + txtValue.Text.Trim().Replace("''", "'") + "'")) == 0)
                    {
                        CommonComponent.ExecuteCommonData("Insert into tb_FeedFieldTypeValues(FieldID,TypeID,FieldValues,DisplayOrder) Values(" + Request.QueryString["FieldId"].ToString() + "," + Request.QueryString["TypeId"].ToString() + ",'" + txtValue.Text.Trim().Replace("''", "'") + "'," + txtDisplayOrder.Text + ")");
                        txtValue.Text = "";
                        txtDisplayOrder.Text = "";
                    }
                    else { Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Field Value Already Exists ..');", true); }
                    BindData();
                }
                else
                {
                    if (Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(ISNULL(FieldValues,0)) as Totcnt from tb_FeedFieldTypeValues Where TypeID=" + Request.QueryString["TypeId"].ToString() + " And FieldId=" + Request.QueryString["FieldId"].ToString() + " And FieldValues='" + txtValue.Text.Trim().Replace("''", "'") + "' And ValueId <> " + hdnValueId.Value.ToString() + "")) == 0)
                    {
                        CommonComponent.ExecuteCommonData("Update tb_FeedFieldTypeValues set FieldValues='" + txtValue.Text + "',DisplayOrder=" + txtDisplayOrder.Text + " Where ValueId=" + hdnValueId.Value.ToString() + "");
                        txtValue.Text = "";
                        txtDisplayOrder.Text = "";
                    }
                    else { Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg02", "alert('Field Value Already Exists ..');", true); }
                    BindData();
                }
                txtValue.Focus();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Enter Field Value and Display Order');", true);
                return;
            }
        }

        /// <summary>
        /// Selected Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdSelected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("btnEdit")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("btndel")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
            }
        }
    }
}