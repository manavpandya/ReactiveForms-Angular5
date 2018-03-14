using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class EditShippingRange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

            if (!IsPostBack)
            {
                string id = "";
                string QStoreid = "";
                if (Request.QueryString["ID"] != null)
                    id = Request.QueryString["ID"].ToString();
                if (Request.QueryString["storeid"] != null)
                    QStoreid = Request.QueryString["storeid"].ToString();
                GetData(id, QStoreid);

            }
        }
        protected void GetData(string id, string QStoreid)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    int shippingrangeid = Convert.ToInt32(id);
                    int Storeid = Convert.ToInt32(QStoreid);
                    DataSet dsgrid = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where shippingpricerangeid=" + shippingrangeid + " and StoreID=" + Storeid + "");
                    if (dsgrid.Tables[0].Rows.Count > 0)
                    {
                        hdnrangeid.Value = dsgrid.Tables[0].Rows[0]["shippingpricerangeid"].ToString();
                        txtfromprice.Text = dsgrid.Tables[0].Rows[0]["FromPrice"].ToString();
                        txttoprice.Text = dsgrid.Tables[0].Rows[0]["ToPrice"].ToString();
                        txtprice.Text = dsgrid.Tables[0].Rows[0]["Price"].ToString();
                        if (Convert.ToBoolean(dsgrid.Tables[0].Rows[0]["Active"].ToString()) == true)
                        {
                            chactive.Checked = true;
                        }

                    }
                }
            }
            catch
            {

            }
        }

        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            string fromprice = txtfromprice.Text;
            string toprice = txttoprice.Text;
            string Price = txtprice.Text;
            var storeid = Convert.ToInt32(AppConfig.StoreID);
            string hdnfield = hdnrangeid.Value;

            if (!string.IsNullOrEmpty(fromprice) && !string.IsNullOrEmpty(hdnfield))
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

                                int id = Convert.ToInt32(hdnfield);
                                DataSet checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where shippingpricerangeid !=" + id + " and Active=1 and " + famount + " between fromprice and toprice");
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
                                            checkdata = CommonComponent.GetCommonDataSet("select * from  tb_ShippingPriceRange where shippingpricerangeid !=" + id + " and Active=1 and " + tamount + " between fromprice and toprice");
                                            if (checkdata.Tables[0].Rows.Count > 0)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please insert another value', 'Message');});", true);
                                            }
                                            else
                                            {
                                                if (chactive.Checked)
                                                {
                                                    CommonComponent.GetCommonDataSet("update tb_ShippingPriceRange set fromprice=" + famount + ",toprice=" + tamount + ",price=" + pamount + ",active=1,deleted=0,updatedby=1,updatedon=GETDATE() where shippingpricerangeid=" + id + "");
                                                }
                                                else
                                                {
                                                    CommonComponent.GetCommonDataSet("update tb_ShippingPriceRange set fromprice=" + famount + ",toprice=" + tamount + ",price=" + pamount + ",active=0,deleted=0,updatedby=1,updatedon=GETDATE() where shippingpricerangeid=" + id + "");
                                                }
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Updated Successfully..', 'Message');});", true);
                                                Response.Redirect("ProductShippingRange.aspx?status=updated");
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

        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Products/ProductShippingRange.aspx");
        }

    }
}