using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.IO;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class SetNewArrivalDisplayOrder : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnsave.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/save.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                btncancel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                DataSet DsOption = new DataSet();
                DsOption = CommonComponent.GetCommonDataSet("select isnull(OrderbyCustom,0) as OrderbyCustom,isnull(OrderbyName,0) as OrderbyName,isnull(OrderbyPrice,0) as OrderbyPrice from tb_custompagemaster where Pagename='newarrival'");
                if (DsOption != null && DsOption.Tables.Count > 0 && DsOption.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DsOption.Tables[0].Rows[0]["OrderbyCustom"].ToString()))
                    {
                        rdooption.SelectedValue = "custom";
                        hdnoptions.Value = "custom";
                    }
                    else if (Convert.ToBoolean(DsOption.Tables[0].Rows[0]["OrderbyName"].ToString()))
                    {
                        rdooption.SelectedValue = "name";
                        hdnoptions.Value = "name";
                    }
                    else if (Convert.ToBoolean(DsOption.Tables[0].Rows[0]["OrderbyPrice"].ToString()))
                    {
                        rdooption.SelectedValue = "price";
                        hdnoptions.Value = "price";
                    }
                    else
                    {
                        rdooption.SelectedValue = "custom";
                        hdnoptions.Value = "custom";
                    }
                }
                BindProducts();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidegrid", "SelectOption();expandcollapse();", true);
            }
        }

        private void BindProducts()
        {

            DataSet dsProducts = new DataSet();
            ProductComponent objProduct = new ProductComponent();

            dsProducts = CommonComponent.GetCommonDataSet("Exec GuiGetNewArrivalProduct 1");
            if (dsProducts != null && dsProducts.Tables.Count > 0 && dsProducts.Tables[0].Rows.Count > 0)
            {

                gridproducts.DataSource = dsProducts.Tables[0];
                gridproducts.DataBind();
                ltrrepeater.Text = "<ul class=\"rep-drag\">";
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {


                    ltrrepeater.Text += "<li style=\"width: 16.5%;margin: 0px auto;list-style-type: none;float: left; margin-right: 10px;\"><div class=\"col-sm-4 fp-display\">";
                    ltrrepeater.Text += " <div class=\"fp-box-div img-center free-swatch-hover\">";
                    ltrrepeater.Text += "<center><img Style=\"border:1px solid #ccc;text-align:center;\" src = '" + GetIconImageProduct(dsProducts.Tables[0].Rows[i]["ImageName"].ToString()) + "' ID=\"imgName22\" ToolTip='" + dsProducts.Tables[0].Rows[0]["Name"].ToString() + "' runat=\"server\" /> </center>";
                    ltrrepeater.Text += "<div class=\"btn-box-bg\"></div>";
                    ltrrepeater.Text += "</div>";
                    ltrrepeater.Text += "<div class=\"fp-display-title\" style=\"height:30px;\"><h2 style=\"line-height: 16px;\">" + SetName(dsProducts.Tables[0].Rows[i]["Name"].ToString()) + "</h2></div>";
                    ltrrepeater.Text += "<p class=\"fp-box-p\" style=\"margin-top: 6%;margin-left: 7%;\">";
                    //ltrrepeater.Text += "<input id=\"hdnCatid\" type=\"hidden\" value=" + dsProducts.Tables[0].Rows[i]["CategoryID"].ToString() + " ></input>";

                    ltrrepeater.Text += "<input id=\"hdnProductID\" type=\"hidden\" value=" + dsProducts.Tables[0].Rows[i]["ProductID"].ToString() + " ></input>";
                    //ltrrepeater.Text += "<br/>";
                    ltrrepeater.Text += "<span style=\"line-height:13px;font-size: 13px;\">Inventory: " + dsProducts.Tables[0].Rows[i]["Inventory"].ToString() + "</span>";
                   // ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Price: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["salePrice"].ToString())) + "</span>";
                    if (!string.IsNullOrEmpty(dsProducts.Tables[0].Rows[i]["salePrice"].ToString()))
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Price: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["salePrice"].ToString())) + "</span>";
                    }
                    else
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Price: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["price"].ToString())) + "</span>";
                    }


                    ltrrepeater.Text += "</p>";
                    ltrrepeater.Text += "</li>";
                }
                ltrrepeater.Text += "</ul>";
                // btnsave.Visible = true;
            }
            else
            {
                gridproducts.DataSource = null;
                gridproducts.DataBind();
                //btnsave.Visible = false;
            }


        }
        /// <summary>
        /// Get Product Image With Full Path
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>return Image with Full Path </returns>
        public string GetIconImageProduct(String img)
        {
            string imagepath = string.Empty;

            try
            {
                imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

                //   if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    imagepath = AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
                    return imagepath;
                }

                imagepath = string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
            }
            catch (Exception ex)
            {

            }
            return imagepath;
        }
        /// <summary>
        /// Set Name of Product or category
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Return Max. 62 Length of string</returns>
        public String SetName(String Name)
        {
            string name = string.Empty;
            try
            {
                if (Name.Length > 48)
                    name = Name.Substring(0, 45) + "...";
                else
                    name = Server.HtmlEncode(Name);
            }
            catch (Exception ex)
            {
            }
            return name;

        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
           


                if(rdooption.SelectedValue.ToString().ToLower()=="name")
                {
                    CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=0,Orderbyname=1,orderbyprice=0 where pagename='newarrival'");
                    hdnoptions.Value = "name";
                }
                else if (rdooption.SelectedValue.ToString().ToLower() == "price")
                {
                    CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=0,Orderbyname=0,orderbyprice=1 where pagename='newarrival'");
                    hdnoptions.Value = "price";
                }
                else
                {
                    CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=1,Orderbyname=0,orderbyprice=0 where pagename='newarrival'");
                    hdnoptions.Value = "custom";
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Saved successfully.', 'Message');});SelectOption();expandcollapse();", true);

            

        }

        protected void btncancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Dashboard.aspx");
        }
    }
}