using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
namespace Solution.UI.Web
{
    public partial class CustomQuoteSize : System.Web.UI.Page
    {
        #region Local Variables

        int ProductID = 0;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["ProductID"] != null && Convert.ToString(Request.QueryString["ProductID"]) != "")
            {
                ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);
            }
            if (!IsPostBack)
            {
                BindState();
                BindCustomContent();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgload", "document.getElementById('hdnheaderqoute').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnheaderqoute').value;document.getElementById('hdnwidthqoute').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnwidthqoute').value;document.getElementById('hdnlengthqoute').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnlengthqoute').value;document.getElementById('hdnoptionhqoute').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnoptionhqoute').value;document.getElementById('hdnquantityqoute').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnquantityqoute').value; if(window.parent.document.getElementById('ContentPlaceHolder1_hdncord') != null){document.getElementById('hdncord').value = window.parent.document.getElementById('ContentPlaceHolder1_hdncord').value;}", true);
            }

            Page.Form.DefaultButton = btnSend.UniqueID;
        }

        public void BindState()
        {
            ddlstate.Items.Clear();

            DataSet dsState = new DataSet();
            dsState = CommonComponent.GetCommonDataSet("Select stateid,Name From tb_State where isnull(Deleted,0)=0 ");
            if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
            {
                ddlstate.DataSource = dsState;
                ddlstate.DataTextField = "Name";
                ddlstate.DataValueField = "StateID";
                ddlstate.DataBind();
            }
            else
            {
                ddlstate.DataSource = null;
                ddlstate.DataBind();
            }
            //ListItem lt1 = new ListItem("Select State", "0");
            //ddlstate.Items.Insert(0, lt1);
            //ListItem lt = new ListItem("Other", "-11");
            //ddlstate.Items.Insert(1, lt);
        }

        public void BindCustomContent()
        {
            try
            {
                DataSet dsTopic = new DataSet();
                dsTopic = CommonComponent.GetCommonDataSet("select Description from tb_topic where TopicName = 'customsizequoteforPopup'");
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Description"].ToString()))
                    {
                        trContent.Visible = true;
                        ltrCustomContent.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='width:100%;'>");
                    }
                    else
                        trContent.Visible = false;

                }
                else trContent.Visible = false;
            }
            catch { }
        }

        /// <summary>
        /// Sending Availability Notification Email
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (txtFirstName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter First Name.');", true);
                txtFirstName.Focus();
                return;
            }
            else if (txtLastName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Last Name.');", true);
                txtLastName.Focus();
                return;
            }
            //else if (txtAddress.Text == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter address.');", true);
            //    txtAddress.Focus();
            //    return;
            //}

            //else if (txtCity.Text == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter city.');", true);
            //    txtCity.Focus();
            //    return;
            //}

            //else if (txtZip.Text == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter zip code.');", true);
            //    txtZip.Focus();
            //    return;
            //}

            else if (txttelephone.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your phone number.');", true);
                txttelephone.Focus();
                return;
            }
            else if (txtEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txtEmail.Focus();
                return;
            }
            else if (!reValidEmail.IsMatch(txtEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                txtEmail.Focus();
                return;
            }
            string MailAddress = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT EmailID FROM tb_ContactEmail  WHERE Subject='Custom quote Request'"));// AppLogic.AppConfigs("ContactMail_ToAddress") + ";" + AppLogic.AppConfigs("customquote_mail");
            if (string.IsNullOrEmpty(MailAddress))
            {
                MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");
            }
            CommonComponent.ExecuteCommonData("Insert into tb_Pricequote (Numberofwindows,PurposeofDrapery,IfFunctioning,WindowWidth,TopofWindowtoFloor,CeilingHeight,DraperyStyle,LiningOption,IsYourRod,Haveyouordered,Firstname,Lastname,Email,Instruction,Zipcode,City,Address,State,Phone,ProductId,Header,Width,Length,Options,ToEmail) "
              + "values ( '" + txtnowindows.Text.ToString().Replace("'", "''") + "' ,'" + ddlpurposeofdrapery.SelectedValue + "' ,'" + ddlfunctioning.SelectedValue + "' , '" + txtwindowwidth.Text.ToString().Replace("'", "''") + "','" + txtwindowtofloor.Text.ToString().Replace("'", "''") + "', '" + txtCeilingheight.Text.ToString().Replace("'", "''") + "'"
              + ",'" + ddldraperystyle.SelectedValue + "' ,'" + ddlliningoption.SelectedValue + "' ,'" + ddlisreadyrod.SelectedValue + "' ,'" + ddlhaveuordered.SelectedValue + "'"
              + ",'" + txtFirstName.Text.ToString().Replace("'", "''") + "','" + txtLastName.Text.ToString().Replace("'", "''") + "','" + txtEmail.Text.ToString().Replace("'", "''") + "','" + txtInstruction.Text.ToString().Replace("'", "''") + "','" + txtZip.Text.ToString().Replace("'", "''") + "','" + txtCity.Text.ToString().Replace("'", "''") + "','" + 
              txtAddress.Text.ToString().Replace("'", "''") + "','" + ddlstate.SelectedItem.Text.ToString() + "','" + txttelephone.Text.ToString().Replace("'", "''") + "' ," + ProductID + ",'" + hdnheaderqoute.Value.ToString() + "','" + hdnwidthqoute.Value.ToString() + "','" +
              hdnlengthqoute.Value.ToString() + "','" + hdnoptionhqoute.Value.ToString() + "','" + MailAddress.ToString()+ "')"); //," + hdnquantityqoute.Value.ToString() + "
           
            if (MailAddress != string.Empty)
            {
                Int32 ReturnMailid = SendEmail(MailAddress);
                if (ReturnMailid > 0)
                {
                    txtnowindows.Text = "";
                    txtwindowwidth.Text = "";
                    txtwindowtofloor.Text = "";
                    txtCeilingheight.Text = "";
                    ddlpurposeofdrapery.ClearSelection();
                    ddlfunctioning.ClearSelection();
                    ddldraperystyle.ClearSelection();
                    ddlliningoption.ClearSelection();
                    ddlisreadyrod.ClearSelection();
                    ddlhaveuordered.ClearSelection();

                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtEmail.Text = "";
                    txtInstruction.Text = "";
                    txtZip.Text = "";
                    txtCity.Text = "";
                    txtAddress.Text = "";
                    ddlstate.SelectedValue = "1";
                    txttelephone.Text = "";
                    txtFirstName.Focus();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('E-Mail has been sent successfully.');window.parent.disablePopup();", true);
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs04", "jAlert('E-Mail has been sent successfully.');window.opener.disablePopup();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while sending Availability Notification.');window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();", true);
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs05", "jAlert('Problem while sending mail.');window.opener.disablePopup();", true);
                    return;
                }
            }

        }

        /// <summary>
        /// Send Email function
        /// </summary>
        /// <param name="ToAddress">String ToAddress</param>
        private Int32 SendEmail(String ToAddress)
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


                strSubject = Regex.Replace(strSubject, "###FIRSTNAME###", txtFirstName.Text + " " + txtLastName.Text, RegexOptions.IgnoreCase);


                strBody = Regex.Replace(strBody, "###NumberofWindows###", txtnowindows.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PurposeofDrapery###", ddlpurposeofdrapery.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###IfFunctioning###", ddlfunctioning.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###WindowWidth###", txtwindowwidth.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###TopofWindowtoFloor###", txtwindowtofloor.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###CeilingHeight###", txtCeilingheight.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###DraperyStyle###", ddldraperystyle.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LiningOption###", ddlliningoption.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###IsYourRod###", ddlisreadyrod.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###Haveyouordered###", ddlhaveuordered.SelectedValue.ToString().Trim(), RegexOptions.IgnoreCase);




                strBody = Regex.Replace(strBody, "###FIRSTNAME###", txtFirstName.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LASTNAME###", txtLastName.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###EMAIL###", txtEmail.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###TELEPHONE###", txttelephone.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PRODUCTLINK###", "/" + tb_product.ProductURL.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PRODUCTNAME###", tb_product.Name.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###SKU###", Convert.ToString(tb_product.SKU).Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###MESSAGE###", txtInstruction.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                string strcord = "";
                if (!string.IsNullOrEmpty(hdncord.Value.ToString()))
                {

                    strcord = "<b>Cord/Mount</b> : " + hdncord.Value.ToString() + "&nbsp; &nbsp;";
                }
                string strtt = "<b>Header</b> : " + " " + hdnheaderqoute.Value.ToString() + " &nbsp; &nbsp;"
                                                   + "  <b>Width</b> : " + " " + hdnwidthqoute.Value.ToString() + "&nbsp; &nbsp; "
                                                   + "  <b>Length</b> : " + " " + hdnlengthqoute.Value.ToString() + " &nbsp; &nbsp;"
                                                   + "  <b>Options</b> : " + " " + hdnoptionhqoute.Value.ToString() + "&nbsp;&nbsp;" + strcord
                                                   + "  <b>Quantity</b> : " + " " + hdnquantityqoute.Value.ToString();
                strBody = Regex.Replace(strBody, "###PRICEQUOTEDETAIL###", strtt.ToString(), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                MailID = CommonOperations.SendMailWithReplyTo(txtEmail.Text, ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                //CommonComponent.ExecuteCommonData("update tb_Pricequote set ToEmail='" + ToAddress.ToString() + "' where =" + );
            }

            return MailID;
        }
    }
}