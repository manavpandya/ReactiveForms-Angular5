using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;


namespace Solution.UI.Web.ADMIN.Products
{
    public partial class EditSuggestedRetail : BasePage
    {
        string strHTMl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveValue.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save.gif";
            if (!IsPostBack)
            {
                GetSuggestedRetailPrice();
            }
        }

        private void GetSuggestedRetailPrice()
        {
            decimal suggestedretail = 0;
            suggestedretail = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select Configvalue from tb_appconfig where configname='ShadeSuggestedRetail' and storeid=1"));
            txtSuggestedRetail.Text = suggestedretail.ToString();
        }


        protected void btnSaveValue_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + (txtSuggestedRetail.Text) + "' where configname='ShadeSuggestedRetail' and storeid=1");
            DisplayShadeCalc();
            Page.ClientScript.RegisterStartupScript(btnSaveValue.GetType(), "@closemsg", "window.opener.document.getElementById('ContentPlaceHolder1_txtSuggestedRetail').value='" + txtSuggestedRetail.Text + "';window.opener.document.getElementById('ContentPlaceHolder1_divshadecalculator').innerHTML='" + strHTMl + "';window.close();", true);
        }

        private void DisplayShadeCalc()
        {
            DataSet dsWidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Shadewidth WHERE ProductId=" + Request.QueryString["ProductID"] + " order by value");

            DataSet dsLength = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ShadeLength WHERE ProductId=" + Request.QueryString["ProductID"] + " order by value");

            DataSet dsDetails = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ShadeDetail");

            Double shadesug = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeSuggestedRetail' and isnull(deleted,0)=0 and Storeid=1"));

            Double shademarkup = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));



            strHTMl = "";

            string strHTMLLength = "";

            string strLength = "";

            if (dsWidth != null && dsWidth.Tables.Count > 0 && dsWidth.Tables[0].Rows.Count > 0)
            {



                strHTMl += "<div><table>";

                strHTMl += "<tr><td class=\"headcol\">WIDTH ►</td>";

                strHTMLLength += "<tr><td class=\"headcol\">LENGTH ▼</td>";



                for (int i = 0; i < dsWidth.Tables[0].Rows.Count; i++)
                {

                    strHTMl += "<td class=\"long\">" + dsWidth.Tables[0].Rows[i]["value"].ToString() + "</td><td class=\"long\">Suggested</td><td class=\"long\">Our Price</td>";

                    strHTMLLength += "<td class=\"long\"></td><td class=\"long\">Retail</td><td class=\"long\">$</td>";

                    if (i == (dsWidth.Tables[0].Rows.Count - 1))
                    {

                        strHTMl += "</tr>";

                        strHTMLLength += "</tr>";

                    }





                }

                strHTMl = strHTMl + strHTMLLength;

                if (dsLength != null && dsLength.Tables.Count > 0 && dsLength.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsLength.Tables[0].Rows.Count; i++)
                    {

                        strHTMl += "<tr><td class=\"headcol\">" + dsLength.Tables[0].Rows[i]["value"].ToString() + "</td>";

                        for (int j = 0; j < dsWidth.Tables[0].Rows.Count; j++)
                        {

                            DataRow[] dr = dsDetails.Tables[0].Select("ShadeWidthID=" + dsWidth.Tables[0].Rows[j]["ShadeWidthID"].ToString() + " and ShadeLengthID=" + dsLength.Tables[0].Rows[i]["ShadeLengthID"].ToString() + "");

                            if (dr != null && dr.Length > 0)
                            {

                                foreach (DataRow dr1 in dr)
                                {

                                    strHTMl += "<td>" + dr1["value"].ToString() + "</td>";

                                    strHTMl += "<td>" + string.Format("{0:0.00}", Convert.ToDouble(Convert.ToDouble(dr1["value"].ToString()) * shadesug)) + "</td>";

                                    strHTMl += "<td>" + string.Format("{0:0.00}", Convert.ToDouble(Convert.ToDouble(dr1["value"].ToString()) * shademarkup)) + "</td>";

                                }



                            }



                        }

                        strHTMl += "</tr>";

                    }

                }

                strHTMl += "</table></div>";

                //<td class=\"long\">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td></tr>



            }
            
            //divshadecalculator.InnerHtml = strHTMl;

        }

    }
}