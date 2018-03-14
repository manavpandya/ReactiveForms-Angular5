using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class NavInventoryDetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                {
                    GetDetails();
                    DataSet ds = new DataSet();
                    Int32 processid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1  isnull(ProccessId,0) from tb_NavInventoryFlag"));
                    if (processid == 1)
                    {
                        lblUpdatedOn.Text += " <b style='color:#ff0000;'>In Progress..</b>";
                        btnNavInventorySync.Visible = false;
                    }
                    else if (processid == 2)
                    {
                        lblUpdatedOn.Text += " <b style='color:green;'>Completed.</b>";
                    }
                    btnNavInventorySync.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/nav-inventory-sync.gif) no-repeat transparent; width: 185px; height: 40px; border:none;cursor:pointer;");
                }
                else
                {
                    Response.Redirect("/Login.aspx");
                }
            }

        }
        public void GetDetails()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("select top 1 isnull(UpdatedOn,'') as UpdatedOn,isnull(UpdatedBy,0) as UpdatedBy from tb_NavInventoryDetail order by UpdatedOn desc");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string AdminName = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(FirstName+'  '+LastName,'') as Name from tb_Admin where AdminId=" + ds.Tables[0].Rows[0]["UpdatedBy"].ToString() + ""));
                lblUpdatedBy.Text = AdminName;
                lblUpdatedOn.Text = ds.Tables[0].Rows[0]["UpdatedOn"].ToString();


            }
            else
            {
                lblUpdatedBy.Text = "N/A";
                lblUpdatedOn.Text = "N/A";
            }


        }



        protected void btnNavInventorySync_Click(object sender, EventArgs e)
        {
            if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
            {
                CommonComponent.ExecuteCommonData("update tb_NavInventoryFlag set Flag=1, AdminId=" + Session["AdminID"] + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Please check Inventory in Ecomm after 45 minutes.','Message');", true);
            }

        }
    }
}