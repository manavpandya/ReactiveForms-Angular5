using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddShippingRange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            string fromprice = txtfromprice.Text;
            string toprice = txttoprice.Text;
            string Price = txtprice.Text;
            var storeid = Convert.ToInt32(AppConfig.StoreID);


            if (!string.IsNullOrEmpty(fromprice) && !string.IsNullOrEmpty(toprice))
            {
                if (!string.IsNullOrEmpty(Price))
                {
                    if (Convert.ToDecimal(Price) < Convert.ToDecimal(fromprice))
                    {
                        if (Convert.ToDecimal(Price) < Convert.ToDecimal(toprice))
                        {

                            try
                            {
                                decimal famount = Convert.ToDecimal(Math.Round(decimal.Parse(fromprice), 2).ToString());
                                decimal tamount = Convert.ToDecimal(Math.Round(decimal.Parse(toprice), 2).ToString());
                                decimal pamount = Convert.ToDecimal(Math.Round(decimal.Parse(Price), 2).ToString());

                                DataSet checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where Active=1 and " + famount + " between fromprice and toprice");
                                if (checkdata.Tables[0].Rows.Count > 0)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please insert another value', 'Message');});", true);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(toprice))
                                    {
                                        try
                                        {
                                            checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where Active=1 and " + tamount + " between fromprice and toprice");
                                            if (checkdata.Tables[0].Rows.Count > 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please insert another value', 'Message');});", true);
                                            }
                                            else
                                            {
                                                if (chactive.Checked)
                                                {
                                                    CommonComponent.GetCommonDataSet("insert into tb_ShippingPriceRange(storeid,fromprice,toprice,price,active,deleted,createdby,createdon) values(" + storeid + "," + famount + "," + tamount + "," + pamount + ",1,0,1,GETDATE())");
                                                }
                                                else
                                                {
                                                    CommonComponent.GetCommonDataSet("insert into tb_ShippingPriceRange(storeid,fromprice,toprice,price,active,deleted,createdby,createdon) values(" + storeid + "," + famount + "," + tamount + "," + pamount + ",0,0,1,GETDATE())");
                                                }
                                                Response.Redirect("ProductShippingRange.aspx?status=inserted");
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                            catch
                            { }

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please insert less discount price', 'Message');});", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please insert less discount price', 'Message');});", true);
                    }
                }
            }

        }

        [WebMethod]
        public static string checkdata(string fromprice, string toprice)
        {
            string fromnotexist = "false";
            if (!string.IsNullOrEmpty(fromprice))
            {
                try
                {
                    DataSet checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where " + Convert.ToDecimal(fromprice) + " between fromprice and toprice");
                    if (checkdata.Tables[0].Rows.Count > 0)
                    {
                        fromnotexist = "false";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(toprice))
                        {
                            try
                            {
                                checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where " + Convert.ToDecimal(toprice) + " between fromprice and toprice");
                                if (checkdata.Tables[0].Rows.Count > 0)
                                {
                                    fromnotexist = "false";
                                }
                                else
                                {
                                    fromnotexist = "true";
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                catch
                { }
            }
            return fromnotexist;

        }

        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Products/ProductShippingRange.aspx");
        }
    }
}