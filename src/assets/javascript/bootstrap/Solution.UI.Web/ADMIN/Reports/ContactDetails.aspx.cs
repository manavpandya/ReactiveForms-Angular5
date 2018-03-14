using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;


namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class ContactDetails : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetContactDetails();
            }
        }

        /// <summary>
        /// Get Contact Details for Site Owner
        /// </summary>
        private void GetContactDetails()
        {
            if (Request.QueryString["MID"] != null)
            {
                DataSet ds = new DataSet();
               
                ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ContactUs WHERE ContactUsID=" + Request.QueryString["MID"] + "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ltname.Text = Convert.ToString(ds.Tables[0].Rows[0]["Name"].ToString());
                    ltemail.Text = Convert.ToString(ds.Tables[0].Rows[0]["Email"].ToString());
                    ltAddress.Text = Convert.ToString(ds.Tables[0].Rows[0]["Address"].ToString());
                    ltCity.Text = Convert.ToString(ds.Tables[0].Rows[0]["City"].ToString());
                    ltCountry.Text = Convert.ToString(ds.Tables[0].Rows[0]["Country"].ToString());
                    ltState.Text = Convert.ToString(ds.Tables[0].Rows[0]["State"].ToString());
                    ltZipCode.Text = Convert.ToString(ds.Tables[0].Rows[0]["ZipCode"].ToString());
                    ltPhone.Text = Convert.ToString(ds.Tables[0].Rows[0]["PhoneNumber"].ToString());
                    ltFax.Text = Convert.ToString(ds.Tables[0].Rows[0]["Country"].ToString());
                    ltCountry.Text = Convert.ToString(ds.Tables[0].Rows[0]["Country"].ToString());
                    ltMessage.Text = Convert.ToString(ds.Tables[0].Rows[0]["Message"].ToString());
                    LtSubjectStatus.Text = Convert.ToString(ds.Tables[0].Rows[0]["SubjectStatus"].ToString());


                    if (Convert.ToString(ds.Tables[0].Rows[0]["City"]) == "")
                    {
                        trCity.Visible = false;
                    }
                    else
                    {
                        trCity.Visible = true;
                    }

                    if (Convert.ToString(ds.Tables[0].Rows[0]["Address"]) == "")
                    {
                        trAddress.Visible = false;
                    }
                    else
                    {
                        trAddress.Visible = true;
                    }


                    if (Convert.ToString(ds.Tables[0].Rows[0]["State"]) == "")
                    {
                        trState.Visible = false;
                    }
                    else
                    {
                        trState.Visible = true;
                    }


                    if (Convert.ToString(ds.Tables[0].Rows[0]["PhoneNumber"]) == "")
                    {
                        trPhone.Visible = false;
                    }
                    else
                    {
                        trPhone.Visible = true;
                    }
                }
            }
        }
    }
}