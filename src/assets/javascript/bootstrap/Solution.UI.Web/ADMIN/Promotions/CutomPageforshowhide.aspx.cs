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

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class CutomPageforshowhide : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnsave.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/save.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            btncancel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            if(!IsPostBack)
            {
                BindProducts();
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

            string[] formkeys = Request.Form.AllKeys;
            CommonComponent.ExecuteCommonData("update tb_product SET Isnodisplaycpage=0 WHERE isnull(deleted,0)=0 and isnull(storeid,0)=1");
            foreach (String s in formkeys)
            {
                //To Move Address Details and Redirect to EditAddress page

               
                if (s.Contains("chknot_"))
                {
                      CommonComponent.ExecuteCommonData("update tb_product SET Isnodisplaycpage=1 WHERE ProductId="+ s.ToString().Replace("chknot_","")+" ");
                }
                
               
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Success", "jAlert('Record saved successfully','Success');", true);
            BindProducts();
        }
        private void BindProducts()
        {
       
            DataSet dsProducts = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            dsProducts = CommonComponent.GetCommonDataSet("exec GuiallcustomProductforhide");

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
                   // Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(QtyOnHand,'0') FROM tb_FabricVendorPortal WHERE Code='" + dsProducts.Tables[0].Rows[i]["FabricCode"].ToString() + "'"));
                  
                    //if (!string.IsNullOrEmpty(dsProducts.Tables[0].Rows[i]["salePrice"].ToString()))
                    //{
                    //    ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">SKU: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["salePrice"].ToString())) + "</span>";
                    //}
                    //else
                    //{
                    //    ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Price: $" + string.Format("{0:0.00}", Convert.ToDecimal(dsProducts.Tables[0].Rows[i]["price"].ToString())) + "</span>";
                    //}

                    ltrrepeater.Text += "<span style=\"line-height: 24px;font-size: 12px;\"><b>SKU</b>:"+ dsProducts.Tables[0].Rows[i]["SKU"].ToString()  + "</span>";
                    if (!string.IsNullOrEmpty(dsProducts.Tables[0].Rows[i]["FabricCode"].ToString()))
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height:24px;font-size: 12px;\"><b>Fabric SKU</b>: " + dsProducts.Tables[0].Rows[i]["FabricCode"].ToString() + " [<b style=\"color:#ff0000\">" + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 isnull(QtyinYard,'0'),Code FROM tb_FabricVendorOrder WHERE VendorOrderNumber ='" + dsProducts.Tables[0].Rows[i]["FabricCode"].ToString() + "_1'")) + "</b>]</span>";
                    }
                    else
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height:13px;font-size: 13px;\"></span>";
                    }
                    
                    if (Convert.ToBoolean(dsProducts.Tables[0].Rows[i]["Isnodisplaycpage"].ToString()))
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Is Not Display: <input type=\"checkbox\" id=\"chknot_" + dsProducts.Tables[0].Rows[i]["ProductID"].ToString() + "\" checked name=\"chknot_" + dsProducts.Tables[0].Rows[i]["ProductID"].ToString() + "\" value=\"yes\" /></span>";
                    }
                    else
                    {
                        ltrrepeater.Text += "<br/><span style=\"line-height: 24px;font-size: 12px;\">Is Not Display: <input type=\"checkbox\" id=\"chknot_" + dsProducts.Tables[0].Rows[i]["ProductID"].ToString() + "\" name=\"chknot_" + dsProducts.Tables[0].Rows[i]["ProductID"].ToString() + "\" value=\"no\" /></span>";
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
                tr1.Visible = false;
                tr2.Visible = false;
                btnsave.Visible = false;
            }


        }
    }
}