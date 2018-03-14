using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class viewpricequotedetail : BasePage
    {

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bindsalesperson();
                GetPricequoteDetails();
                btnsend.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/send.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
              
            }
        }

        private void GetPricequoteDetails()
        {
            if (Request.QueryString["PriceQuoteid"] != null)
            {
                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Pricequote WHERE PriceQuoteid=" + Request.QueryString["PriceQuoteid"] + "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (!(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Firstname"].ToString())) && !(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Lastname"].ToString())))
                    {
                        ltname.Text = ds.Tables[0].Rows[0]["Firstname"].ToString() + " " + ds.Tables[0].Rows[0]["Lastname"].ToString();
                        lblfirstname.Text = ds.Tables[0].Rows[0]["Firstname"].ToString();
                        lbllastname.Text = ds.Tables[0].Rows[0]["Lastname"].ToString();
                    }

                    if(!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Email"].ToString()))
                    ltemail.Text = Convert.ToString(ds.Tables[0].Rows[0]["Email"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Address"].ToString()))
                    ltAddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["Address"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["City"].ToString()))
                    ltCity.Text = Convert.ToString(ds.Tables[0].Rows[0]["City"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["State"].ToString()))
                    ltState.Text = Convert.ToString(ds.Tables[0].Rows[0]["State"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ZipCode"].ToString()))
                    ltZipCode.Text = Convert.ToString(ds.Tables[0].Rows[0]["ZipCode"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Phone"].ToString()))
                    ltPhone.Text = Convert.ToString(ds.Tables[0].Rows[0]["Phone"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Instruction"].ToString()))
                    ltMessage.Text = Convert.ToString(ds.Tables[0].Rows[0]["Instruction"].ToString());

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProductID"].ToString()))
                    hdnproductid.Value = ds.Tables[0].Rows[0]["ProductID"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Header"].ToString()))
                        ltpricequotedetail.Text = "<b>Header</b> : " + " " + ds.Tables[0].Rows[0]["Header"].ToString() + " &nbsp; &nbsp;"
                                                   + "  <b>Width</b> : " + " " + ds.Tables[0].Rows[0]["Width"].ToString() + "&nbsp; &nbsp; "
                                                   + "  <b>Length</b> : " + " " + ds.Tables[0].Rows[0]["Length"].ToString() + " &nbsp; &nbsp;"
                                                   + "  <b>Options</b> : " + " " + ds.Tables[0].Rows[0]["Options"].ToString() + "&nbsp;&nbsp;"
                                                   + "  <b>Quantity</b> : " + " " + ds.Tables[0].Rows[0]["Quantity"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Numberofwindows"].ToString()))
                        ltrNumberofwindow.Text= ds.Tables[0].Rows[0]["Numberofwindows"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PurposeofDrapery"].ToString()))
                        ltrPurposeofDrapery.Text = ds.Tables[0].Rows[0]["PurposeofDrapery"].ToString();


                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IfFunctioning"].ToString()))
                        ltrFunctoning.Text = ds.Tables[0].Rows[0]["IfFunctioning"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["WindowWidth"].ToString()))
                        ltrwindowwidth.Text= ds.Tables[0].Rows[0]["WindowWidth"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TopofWindowtoFloor"].ToString()))
                        ltrTopwidowfloor.Text = ds.Tables[0].Rows[0]["TopofWindowtoFloor"].ToString();


                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CeilingHeight"].ToString()))
                        ltrceilingheight.Text= ds.Tables[0].Rows[0]["CeilingHeight"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DraperyStyle"].ToString()))
                       ltrdraperystyle.Text = ds.Tables[0].Rows[0]["DraperyStyle"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["LiningOption"].ToString()))
                        ltrliningoption.Text = ds.Tables[0].Rows[0]["LiningOption"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsYourRod"].ToString()))
                        ltrisyourrod.Text = ds.Tables[0].Rows[0]["IsYourRod"].ToString();

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Haveyouordered"].ToString()))
                        ltrhaveyouordered.Text = ds.Tables[0].Rows[0]["Haveyouordered"].ToString();

             

                   

                    
                }
            }
        }


        private void Bindsalesperson()
        {
            AdminRightsComponent objAdminRightComponent = null;
            objAdminRightComponent = new AdminRightsComponent();
            ddlsalesperson.DataSource = objAdminRightComponent.GetAdminList(0);
            ddlsalesperson.DataTextField = "FirstName";
            ddlsalesperson.DataValueField = "EmailID";
            ddlsalesperson.DataBind();
            ddlsalesperson.SelectedIndex = 0;
        }

        protected void btnsend_Click(object sender, EventArgs e)
        {

            if (hdnproductid != null && hdnproductid.Value != "")
            {
                int ProductID = 0;
                int.TryParse(Convert.ToString(hdnproductid.Value), out  ProductID);
                string[] Array = ddlsalesperson.SelectedItem.Text.ToString().Split(':');
                string Assignname = Array[0];
                if (Request.QueryString["PriceQuoteid"] != null)
                {
                    CommonComponent.ExecuteCommonData("update tb_pricequote set Assignname ='" + Assignname + "' where PriceQuoteid = " + Request.QueryString["PriceQuoteid"]);
                }
                Int32 ReturnMailid = SendEmail(ddlsalesperson.SelectedValue, ProductID);
                if (ReturnMailid > 0)
                {
                   
                     
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('E-Mail has been sent successfully.');window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();", true);


                     
                }
                else { Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while sending Availability Notification.');window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();", true); }
            }
        }
           

        

        private Int32 SendEmail(String ToAddress, int ProductID)
        {
            int MailID = 0;
            CustomerComponent objCustomer = new CustomerComponent();
            ProductComponent objProduct = new ProductComponent();
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("CustomerSizeQuote", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            tb_Product tb_product = new tb_Product();
            tb_product = objProduct.GetAllProductDetailsbyProductID(ProductID);
            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();


                strSubject = Regex.Replace(strSubject, "###FIRSTNAME###", lblfirstname.Text + " " + lbllastname.Text, RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", lblfirstname.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LASTNAME###", lbllastname.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###EMAIL###", ltemail.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###TELEPHONE###", ltPhone.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###MESSAGE###", ltMessage.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###PRICEQUOTEDETAIL###", ltpricequotedetail.Text.ToString(), RegexOptions.IgnoreCase);
              



                strBody = Regex.Replace(strBody, "###PRODUCTLINK###", "/" + tb_product.MainCategory.ToString().Trim() + "/" + tb_product.SEName.ToString().Trim() + "-" + tb_product.ProductID.ToString().Trim() + ".aspx", RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PRODUCTNAME###", tb_product.Name.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###SKU###", Convert.ToString(tb_product.SKU).Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                MailID = CommonOperations.SendMailWithReplyTo(ltemail.Text, ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }

            return MailID;
        }
    }
}