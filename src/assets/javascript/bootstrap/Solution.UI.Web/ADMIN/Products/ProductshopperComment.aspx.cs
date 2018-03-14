using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductshopperComment : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            System.Data.DataSet dtComment = new System.Data.DataSet();
            dtComment = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("select * from tb_ProductshopperComment WHERE isnull(IsApproved,0)=0");

            if (dtComment != null && dtComment.Tables.Count > 0 && dtComment.Tables[0].Rows.Count > 0)
            {
                RptList.DataSource = dtComment;
                RptList.DataBind();
            }
            else {
                RptList.DataSource = dtComment;
                RptList.DataBind();
            }

        }


        protected void RptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                
                int ProductshopperCommentID = Convert.ToInt32(e.CommandArgument);
                //ApprovedDate
                Solution.Bussines.Components.CommonComponent.ExecuteCommonData("update tb_ProductshopperComment set IsApproved = 1, ApprovedDate='" + DateTime.Now + "' where ProductshopperCommentID=" + ProductshopperCommentID + "");
                BindData();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Comment Approve successfully', 'Message','');", true);
                return;
            }

            if (e.CommandName == "Disapprove")
            {
                
                int ProductshopperCommentID = Convert.ToInt32(e.CommandArgument);
                Solution.Bussines.Components.CommonComponent.ExecuteCommonData("update tb_ProductshopperComment set IsApproved = 1, disapproveddate='" + DateTime.Now + "' where ProductshopperCommentID=" + ProductshopperCommentID + "");
                BindData();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Comment Disapprove successfully', 'Message','');", true);
                return;
            }
        }

        protected void RptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnapprove = (ImageButton)e.Item.FindControl("btnApprove");
                ImageButton btnUnApprove = (ImageButton)e.Item.FindControl("btnUnApprove");

                btnapprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/approve.png";
                btnUnApprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/disapprove.png";
            }
        }

    }
}