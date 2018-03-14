using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class CountryDemo : System.Web.UI.Page
    {
        private int Id = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CountryID"]))
            {

                Id = int.Parse(Request.QueryString["CountryID"]);
                if (!IsPostBack)
                    LoadCountry();
            }

        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CountryComponent countryInsert = new CountryComponent();
            tb_Country ctxCountry = null;
            if (Id > 0)
            {
                ctxCountry = countryInsert.getCountry(Id);
                ctxCountry.Name = txtCountry.Text;
                ctxCountry.TwoLetterISOCode = txtTwolatterISOCode.Text;
                ctxCountry.ThreeLetterISOCode = txtThreelatterISOCode.Text;
                ctxCountry.NumericISOCode = txtNumericISOCode.Text;
                ctxCountry.DisplayOrder = Convert.ToInt32(txtDispalyOrder.Text);
                countryInsert.UpdateCountry(ctxCountry);
            }
            else
            {
                ctxCountry = new tb_Country();
                ctxCountry.Name = txtCountry.Text;
                ctxCountry.TwoLetterISOCode = txtTwolatterISOCode.Text;
                ctxCountry.ThreeLetterISOCode = txtThreelatterISOCode.Text;
                ctxCountry.NumericISOCode = txtNumericISOCode.Text;
                ctxCountry.DisplayOrder = Convert.ToInt32(txtDispalyOrder.Text);

                countryInsert.CreateCountry(ctxCountry);
            }
            Response.Redirect("CountryList.aspx");


        }

        /// <summary>
        /// Loads the Country for update Record
        /// </summary>
        private void LoadCountry()
        {
            try
            {
                CountryComponent LoadCountry = new CountryComponent();
                tb_Country tbCountry = LoadCountry.getCountry(Id);
                txtCountry.Text = tbCountry.Name;
                txtTwolatterISOCode.Text = tbCountry.TwoLetterISOCode;
                txtThreelatterISOCode.Text = tbCountry.ThreeLetterISOCode;
                txtNumericISOCode.Text = tbCountry.NumericISOCode;
                txtDispalyOrder.Text = Convert.ToString(tbCountry.DisplayOrder);
            }
            catch
            {
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("CountryList.aspx");
        }
    }
}