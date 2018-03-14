using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components;
using Castle.Web.Controls;
using System.Configuration;

namespace Solution.UI.Web
{
    public partial class Testimonials : System.Web.UI.Page
    {
        Int32 TotalTestimonialCount = 0;
        int rowIndex = 1;
        DataSet ds = new DataSet();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              //  AppLogic.ApplicationStart();
             ///   AppConfig.StoreID = Convert.ToInt32(ConfigurationManager.AppSettings["GeneralStoreID"]);
                // CommonOperations.RedirectWithSSL(false);
                GetTestimonials();
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            TestimonialComponent objRating = new TestimonialComponent();
            string strResult = "";
            strResult = objRating.AddOrUpdateTestimonialsRating(txtname.Text, txtCity.Text, txtCountry.Text, txtcomment.Text, txtEmail.Text, Convert.ToInt32(ddlrating.SelectedValue.ToString()), 1, Request.UserHostAddress.ToString());
            txtcomment.Text = "";
            txtname.Text = "";
            txtEmail.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            ddlrating.SelectedIndex = 0;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Testimonial has been waiting for approval');", true);
        }

        /// <summary>
        /// Gets the testimonials.
        /// </summary>
        public void GetTestimonials()
        {
            DataSet dsTestimonials = TestimonialComponent.GetTestimonials(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTestimonials != null && dsTestimonials.Tables.Count > 0 && dsTestimonials.Tables[0].Rows.Count > 0)
            {
                TotalTestimonialCount = dsTestimonials.Tables[0].Rows.Count;
                dtlTestimonial.DataSource = dsTestimonials;
                dtlTestimonial.DataBind();

            }
        }

        /// <summary>
        /// Testimonial Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void dtlTestimonial_ItemDataBound1(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Rater rating = (Rater)e.Item.FindControl("Rater");
                Literal ltno = (Literal)e.Item.FindControl("ltrNoCount");
                Literal ltYes = (Literal)e.Item.FindControl("ltrYesCount");
                Literal ltTot = (Literal)e.Item.FindControl("ltrtotalCount");
                LinkButton lnkyes = (LinkButton)e.Item.FindControl("lnkyes");
                LinkButton lnkno = (LinkButton)e.Item.FindControl("lnkno");
                HtmlAnchor aYes = (HtmlAnchor)e.Item.FindControl("aYes");
                Label hdnId = (Label)e.Item.FindControl("hdnId");
                HtmlGenericControl divusefulcnt = (HtmlGenericControl)e.Item.FindControl("divusefulcnt");

                HtmlAnchor aNo = (HtmlAnchor)e.Item.FindControl("aNo");
                aYes.Attributes.Add("onclick", "javascript:__doPostBack('ctl00$ContentPlaceHolder1$dtlTestimonial$ctl" + e.Item.ItemIndex.ToString("D2") + "$lnkyes','');");
                aNo.Attributes.Add("onclick", "javascript:__doPostBack('ctl00$ContentPlaceHolder1$dtlTestimonial$ctl" + e.Item.ItemIndex.ToString("D2") + "$lnkno','');");

                int YesCount = 0;
                int NoCount = 0;
                int.TryParse(ltno.Text, out NoCount);
                int.TryParse(ltYes.Text, out YesCount);
                ltYes.Text = Convert.ToString(YesCount);
                ltTot.Text = Convert.ToString(YesCount + NoCount);

                if (ltYes.Text == "0" && ltTot.Text == "0")
                {
                    HtmlControl div = (HtmlControl)e.Item.FindControl("divReview");
                    div.Visible = false;
                }
                rating.ImageOnUrl = "/images/star-form.jpg";
                rating.ImageOffUrl = "/images/star-form1.jpg";
                if (TotalTestimonialCount == rowIndex)
                {
                    divusefulcnt.Attributes.Add("style", "border-bottom:none !important;");
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgdivborder", "document.getElementById('" + divusefulcnt.ClientID + "').setAttribute('style','border-bottom:none!important;');", true);
                }


                rowIndex++;
            }
        }

        /// <summary>
        /// Testimonial Repeater Item Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListCommandEventArgs e</param>
        protected void dtlTestimonial_ItemCommand1(object source, DataListCommandEventArgs e)
        {
            TestimonialComponent objTestimonialComp = new TestimonialComponent();
            if (e.CommandName == "Yes")
            {
                int TestimonialID = Convert.ToInt32(e.CommandArgument);
                objTestimonialComp.UpdateYesCount(TestimonialID);
            }
            if (e.CommandName == "No")
            {
                int TestimonialID = Convert.ToInt32(e.CommandArgument);
                objTestimonialComp.UpdateNoCount(TestimonialID);
            }
            GetTestimonials();
        }
    }
}