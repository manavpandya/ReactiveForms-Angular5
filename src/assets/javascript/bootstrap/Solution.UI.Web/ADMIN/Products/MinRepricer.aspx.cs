using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class MinRepricer : BasePage
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

                if (!string.IsNullOrEmpty(Request.QueryString["MinMasterID"]) && Convert.ToString(Request.QueryString["MinMasterID"]) != "0")
                {
                    DataSet Ds = new DataSet();
                    Ds = CommonComponent.GetCommonDataSet("GuiGetMinRePricerList 2,''," + Convert.ToInt32(Request.QueryString["MinMasterID"].ToString()) + "");
                    if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                    {
                        txtFieldName.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Fieldname"]);
                        txtPercentage.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Percentage"]);
                        if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["IsOveride"].ToString()) && Convert.ToBoolean(Ds.Tables[0].Rows[0]["IsOveride"].ToString()))
                        {
                            chkIsOverride.Checked = true;
                        }
                        else
                        {
                            chkIsOverride.Checked = false;
                        }
                        if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[0]["active"].ToString()) && Convert.ToBoolean(Ds.Tables[0].Rows[0]["active"].ToString()))
                        {
                            chkactive.Checked = true;
                        }
                        else
                        {
                            chkactive.Checked = false;
                        }

                    }
                }
            }
        }

        /// <summary>
        ///  Save Template Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["MinMasterID"]) && Convert.ToString(Request.QueryString["MinMasterID"]) != "0")
            {
                Decimal Percentage = Decimal.Zero;
                Decimal.TryParse(txtPercentage.Text.ToString(), out Percentage);
                CommonComponent.ExecuteCommonData("Exec GuiGetMinRePricerList 3,''," + Convert.ToInt32(Request.QueryString["MinMasterID"].ToString()) + ",'" + txtFieldName.Text.ToString().Replace("'", "''") + "'," + Percentage + "," + chkIsOverride.Checked + "," + chkactive.Checked + "");
                CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 5,'','ALL'," + Session["AdminID"].ToString() + "");
                Response.Redirect("MinRepricerList.aspx?status=updated");
            }
            else
            {
                Decimal Percentage = Decimal.Zero;
                Decimal.TryParse(txtPercentage.Text.ToString(), out Percentage);
                CommonComponent.ExecuteCommonData("Exec GuiGetMinRePricerList 4,''," + Convert.ToInt32(0) + ",'" + txtFieldName.Text.ToString().Replace("'", "''") + "'," + Percentage + "," + chkIsOverride.Checked + "," + chkactive.Checked + "");
                CommonComponent.ExecuteCommonData("Exec GuiInsertRepricerlog 4,'','ALL'," + Session["AdminID"].ToString() + "");
                Response.Redirect("MinRepricerList.aspx?status=inserted");
            }
        }

        /// <summary>
        ///  Cancel Template Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MinRepricerList.aspx");
        }


    }
}