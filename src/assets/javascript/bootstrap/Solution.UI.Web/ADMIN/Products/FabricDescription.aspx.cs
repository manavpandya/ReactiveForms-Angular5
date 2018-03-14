using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;


namespace Solution.UI.Web.ADMIN.Products
{
    public partial class FabricDescription : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/gray/images/save.gif";
                if (Request.QueryString["fabriccodeid"] != null && Request.QueryString["fabriccodeid"].ToString() != "")
                {
                    string name = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(name,'') from tb_ProductFabricCode where FabricCodeId=" + Request.QueryString["fabriccodeid"].ToString() + ""));
                    fabricname.Text = name;
                    string Desc = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(FabricDescription,'') from tb_ProductFabricCode where FabricCodeId=" + Request.QueryString["fabriccodeid"].ToString() + ""));
                    if (!String.IsNullOrEmpty(Desc))
                    {
                        ckeditordescription.Text = Desc;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["fabriccodeid"] != null && Request.QueryString["fabriccodeid"].ToString() != "")
            {

                CommonComponent.ExecuteCommonData("update tb_ProductFabricCode set FabricDescription='" + ckeditordescription.Text.ToString().Replace("'","''") + "' where FabricCodeId=" + Request.QueryString["fabriccodeid"].ToString() + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CodeAdded", "jAlert('Description Saved successfully.','Message');", true);
            }
        }

        
    }
}