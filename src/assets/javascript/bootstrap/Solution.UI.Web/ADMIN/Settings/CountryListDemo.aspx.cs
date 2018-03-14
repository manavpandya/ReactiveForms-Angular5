using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class CountryListDemo : System.Web.UI.Page
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
                CountryComponent.Filter = "";
            }
        }

        /// <summary>
        /// Page OnInit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            //  ContactListPageData.Page = this;
            base.OnInit(e);
        }

        /// <summary>
        /// Country Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void _CountryGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCountry")
            {
                //
                try
                {
                    CountryComponent cust = new CountryComponent();
                    tb_Country ctx = null;
                    int CountryId = Convert.ToInt32(e.CommandArgument);
                    ctx = cust.getCountry(CountryId);
                    ctx.Deleted = true;
                    cust.UpdateCountry(ctx);
                    if (CountryComponent.CountryID == 0)
                        RefreshGrid(true);
                    else
                        this.Response.Redirect("CountryList.aspx", false);
                }
                catch
                { }
            }
            else if (e.CommandName == "edit")
            {
                try
                {
                    int CountryId = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("Country.aspx?CountryID=" + CountryId);
                }
                catch
                { }
            }
            else if (e.CommandName == "add")
            {
                try
                {
                    Response.Redirect("Country.aspx");
                }
                catch
                { }
            }
        }

        /// <summary>
        /// Refresh Gridview Function
        /// </summary>
        /// <param name="afterDelete">Boolean afterDelete</param>
        private void RefreshGrid(bool afterDelete = false)
        {
            CountryComponent.AfterDelete = afterDelete;
            _CountryGridView.DataBind();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CountryComponent.CountryID = 0;
            CountryComponent.Filter = txtCountry.Text;
            CountryComponent.NewFilter = true;
            _CountryGridView.DataBind();
            _CountryGridView.PageIndex = 0;
        }

        /// <summary>
        /// Gets the Data by Filter
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <returns>Returns tb_Country User Class Object</returns>
        public static List<tb_Country> GetDataByFilter(int startIndex, int pageSize, string sortBy)
        {
            List<tb_Country> objCountry = new List<tb_Country>();
            return objCountry;
        }


    }
}