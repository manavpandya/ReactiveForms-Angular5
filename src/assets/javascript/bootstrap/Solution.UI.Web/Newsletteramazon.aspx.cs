using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Text.RegularExpressions;
using System.Data;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Net.Mail;

namespace Solution.UI.Web
{
    public partial class Newsletteramazon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SnedNewsLetter();
        }
        private void SnedNewsLetter()
        {
            NewsSubscribtionEntity objsubscriber = new NewsSubscribtionEntity();
            if (Request.Form["semail"] != null && Request.Form["semail"].ToString().Length > 0)
            {
                Regex re = new Regex(@"^[a-zA-Z0-9][-\+\w\.]*@([a-zA-Z0-9][\w\-]*\.)+[a-zA-Z]{2,4}$");
                if (Request.Form["semail"].ToString().Trim() == "" || Request.Form["semail"].ToString().Trim() == "Enter your E-Mail Address")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgEmail", "alert('Please enter E-Mail Address.');", true);
                    //txtSubscriber.Focus();
                }
                else if (!re.IsMatch(Request.Form["semail"].ToString().Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgValidEmail", "alert('Enter valid E-Mail Address.');", true);
                }
                else
                {
                    objsubscriber.Email = Request.Form["semail"].ToString().Trim();
                    try
                    {
                        NewsSubscribtionComponent obj = new NewsSubscribtionComponent();
                        #region Mail Send For Subscribed Newsletter
                        DataSet DSNews = new DataSet();
                        DSNews = obj.GetNewsSubscribtionList(Request.Form["semail"].ToString().Trim(), Convert.ToInt32(3));

                        if (DSNews != null && DSNews.Tables.Count > 0 && DSNews.Tables[0].Rows.Count > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgAlready", "alert('You have been already subscribed.');", true);
                            //txtSubscriber.Text = "Enter your E-Mail Address";
                            //txtNewsZipCode.Text = "Enter Zip";
                            return;
                        }
                        else
                        {
                            NewsSubscribtionComponent counS = new NewsSubscribtionComponent();
                            tb_NewsSubscription tb_NewsSubscription = new Bussines.Entities.tb_NewsSubscription();
                            tb_NewsSubscription.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", 3);
                            tb_NewsSubscription.Email = Request.Form["semail"].ToString();
                            tb_NewsSubscription.CreatedOn = DateTime.Now;
                            Int32 isupdated = counS.CreateNewsSubscription(tb_NewsSubscription);

                            CustomerComponent objCustomer = new CustomerComponent();
                            DataSet dsMailTemplate = new DataSet();
                            dsMailTemplate = objCustomer.GetEmailTamplate("NewsSubscription", Convert.ToInt32(3));

                            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                            {
                                String strBody = "";
                                String strSubject = "";
                                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                                string STORENAME = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STORENAME'"));
                                string LIVE_SERVER = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='Live_Server'"));
                                string STOREPATH = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STOREPATH'"));

                                strSubject = Regex.Replace(strSubject, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);
                                strSubject = Regex.Replace(strSubject, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", LIVE_SERVER.ToString(), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###StoreID###","3", RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###EMAILID###", Server.UrlEncode(Server.HtmlEncode(SecurityComponent.Encrypt(Request.Form["semail"].ToString().Trim()))), RegexOptions.IgnoreCase);
                                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                                CommonOperations.SendMail(Request.Form["semail"].ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                                Response.Redirect("http://www.curtainsonbudget.com/info/Newsletter_ThankYou");
                                //txtSubscriber.Text = "Enter your E-Mail Address";
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgSorry", "alert('Sorry...unable to send confirmation mail.');", true);
                        //txtSubscriber.Text = "Enter your E-Mail Address";
                        // txtNewsZipCode.Text = "Enter Zip";
                    }
                }
            }
        }
        protected void btnSubscriber_Click(object sender, EventArgs e)
        {
            
        }
    }
}