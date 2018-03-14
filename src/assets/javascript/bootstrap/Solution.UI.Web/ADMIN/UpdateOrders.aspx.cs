using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
namespace Solution.UI.Web.ADMIN
{
    public partial class UpdateOrders : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            DataSet ds = CommonComponent.GetCommonDataSet("select * from tb_Order where isnull(CardNumber,'')<>'' and ISNULL(Last4,'')=''");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    String cardnumber = SecurityComponent.Decrypt(ds.Tables[0].Rows[i]["CardNumber"].ToString());
                    if (cardnumber.Length > 4)
                    {
                        string Last4 = cardnumber.ToString().Substring(cardnumber.ToString().Length - 4);

                        CommonComponent.ExecuteCommonData("update tb_order set Last4='" + Last4 + "' where OrderNumber=" + Convert.ToInt32(ds.Tables[0].Rows[i]["OrderNumber"].ToString()) + "");
                    }

                }
                ScriptManager.RegisterClientScriptBlock(btnUpdate, btnUpdate.GetType(), "Msg", "jAlert('Orders Updated Successfully.','Required Information')", true);
            }
        }
    }
}