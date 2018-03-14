using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class ImageSize : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        ConfigurationComponent cfg = new ConfigurationComponent();

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
                btnsave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnclose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                BindData(drpstorename.SelectedValue);
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                drpstorename.DataSource = Storelist;
                drpstorename.DataTextField = "StoreName";
                drpstorename.DataValueField = "StoreID";
            }
            else
            {
                drpstorename.DataSource = null;
            }
            drpstorename.DataBind();
            drpstorename.Items.Insert(0, new ListItem("Select Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                drpstorename.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
        }

        /// <summary>
        /// Bind The data to the Text Boxes
        /// </summary>
        /// <param name="Storeid">int StoreID</param>
        public DataSet BindData(string Storeid)
        {
            DataSet dssize = cfg.GetImagesize(Convert.ToInt32(Storeid));
            GetValue(dssize, Storeid);
            return dssize;
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsave_Click(object sender, ImageClickEventArgs e)
        {
            lblMsg.Text = "";
            Hashtable ht = new Hashtable();
            if (Session["ht"] != null)
                ht = (Hashtable)Session["ht"];
            if (ht != null && drpstorename.SelectedIndex != -1)
            {
                UpdateThis(ht, "CategoryMicroHeight", drpstorename.SelectedValue, txtCategoryMicroHeight.Text);
                UpdateThis(ht, "CategoryMicroWidth", drpstorename.SelectedValue, txtCategoryMicroWidth.Text);

                UpdateThis(ht, "CategoryIconHeight", drpstorename.SelectedValue, txtCategoryIconHeight.Text);
                UpdateThis(ht, "CategoryIconWidth", drpstorename.SelectedValue, txtCategoryIconWidth.Text);
                UpdateThis(ht, "ProductIconHeight", drpstorename.SelectedValue, txtProductIconHeight.Text);
                UpdateThis(ht, "ProductIconWidth", drpstorename.SelectedValue, txtProductIconwidth.Text);
                UpdateThis(ht, "ProductLargeHeight", drpstorename.SelectedValue, txtProductLargeHeight.Text);
                UpdateThis(ht, "ProductLargeWidth", drpstorename.SelectedValue, txtProductLargeWidth.Text);
                UpdateThis(ht, "ProductMediumHeight", drpstorename.SelectedValue, txtProductMediumHeight.Text);
                UpdateThis(ht, "ProductMediumWidth", drpstorename.SelectedValue, txtProductMediumWidth.Text);
                UpdateThis(ht, "ProductMicroHeight", drpstorename.SelectedValue, txtProductMicroHeight.Text);
                UpdateThis(ht, "ProductMicroWidth", drpstorename.SelectedValue, txtProductMicroWidth.Text);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Image Size Updated Successfully.', 'Message');});", true);
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void drpstorename_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if (drpstorename.SelectedIndex != -1)
                ds = BindData(drpstorename.SelectedValue);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ClearData();
            }
        }

        /// <summary>
        /// Update the image Details
        /// </summary>
        /// <param name="ht">Hashtable ht</param>
        /// <param name="Imagename">string Imagename</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Imagesize">string Imagesize</param>
        private void UpdateThis(Hashtable ht, string Imagename, string StoreID, string Imagesize)
        {
            ConfigurationComponent.UpdateImageSize(Imagename, Convert.ToInt32(StoreID), Imagesize, 2);
        }

        /// <summary>
        /// Gets the Image Detail
        /// </summary>
        /// <param name="Ds">Dataset Ds</param>
        /// <param name="StoreID">string StoreID</param>
        public void GetValue(DataSet Ds, string StoreID)
        {

            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                for (int cnt = 0; cnt < Ds.Tables[0].Rows.Count; cnt++)
                {
                    ht.Add((string.IsNullOrEmpty(Ds.Tables[0].Rows[cnt]["StoreID"].ToString())
                    ? "1" : Ds.Tables[0].Rows[cnt]["StoreID"].ToString()) + Ds.Tables[0].Rows[cnt]["ImageName"].ToString(),
                    Ds.Tables[0].Rows[cnt]["ImageSize"].ToString());
                }
                Session["ht"] = ht;
                if (ht[StoreID + "CategoryMicroHeight"] != null)
                    txtCategoryMicroHeight.Text = (ht[StoreID + "CategoryIconHeight"] == null) ? "" : ht[StoreID + "CategoryMicroHeight"].ToString();
                else
                    txtCategoryMicroHeight.Text = "";

                if (ht[StoreID + "CategoryMicroWidth"] != null)
                    txtCategoryMicroWidth.Text = (ht[StoreID + "CategoryIconWidth"] == null) ? "" : ht[StoreID + "CategoryMicroWidth"].ToString();
                else
                    txtCategoryMicroWidth.Text = "";

                if (ht[StoreID + "CategoryIconHeight"] != null)
                    txtCategoryIconHeight.Text = (ht[StoreID + "CategoryIconHeight"] == null) ? "" : ht[StoreID + "CategoryIconHeight"].ToString();
                else txtCategoryIconHeight.Text = "";

                if (ht[StoreID + "CategoryIconWidth"] != null)
                    txtCategoryIconWidth.Text = (ht[StoreID + "CategoryIconWidth"] == null) ? "" : ht[StoreID + "CategoryIconWidth"].ToString();
                else txtCategoryIconWidth.Text = "";

                if (ht[StoreID + "ProductIconHeight"] != null)
                    txtProductIconHeight.Text = (ht[StoreID + "ProductIconHeight"] == null) ? "" : ht[StoreID + "ProductIconHeight"].ToString();
                else txtProductIconHeight.Text = "";

                if (ht[StoreID + "ProductIconWidth"] != null)
                    txtProductIconwidth.Text = (ht[StoreID + "ProductIconWidth"] == null) ? "" : ht[StoreID + "ProductIconWidth"].ToString();
                else txtProductIconwidth.Text = "";

                if (ht[StoreID + "ProductLargeHeight"] != null)
                    txtProductLargeHeight.Text = (ht[StoreID + "ProductLargeHeight"] == null) ? "" : ht[StoreID + "ProductLargeHeight"].ToString();
                else
                    txtProductLargeHeight.Text = "";

                if (ht[StoreID + "ProductLargeWidth"] != null)
                    txtProductLargeWidth.Text = (ht[StoreID + "ProductLargeWidth"] == null) ? "" : ht[StoreID + "ProductLargeWidth"].ToString();
                else txtProductLargeWidth.Text = "";

                if (ht[StoreID + "ProductMediumHeight"] != null)
                    txtProductMediumHeight.Text = (ht[StoreID + "ProductMediumHeight"] == null) ? "" : ht[StoreID + "ProductMediumHeight"].ToString();
                else txtProductMediumHeight.Text = "";

                if (ht[StoreID + "ProductMediumWidth"] != null)
                    txtProductMediumWidth.Text = (ht[StoreID + "ProductMediumWidth"] == null) ? "" : ht[StoreID + "ProductMediumWidth"].ToString();
                else
                    txtProductMediumWidth.Text = "";

                if (ht[StoreID + "ProductMicroHeight"] != null)
                    txtProductMicroHeight.Text = (ht[StoreID + "ProductMicroHeight"] == null) ? "" : ht[StoreID + "ProductMicroHeight"].ToString();
                else txtProductMicroHeight.Text = "";

                if (ht[StoreID + "ProductMicroWidth"] != null)
                    txtProductMicroWidth.Text = (ht[StoreID + "ProductMicroWidth"] == null) ? "" : ht[StoreID + "ProductMicroWidth"].ToString();
                else txtProductMicroWidth.Text = "";
            }
        }


        /// <summary>
        /// Clear the value of al text box
        /// </summary>
        private void ClearData()
        {
            txtCategoryMicroHeight.Text = "";
            txtCategoryMicroWidth.Text = "";

            txtCategoryIconHeight.Text = "";
            txtCategoryIconWidth.Text = "";
            txtProductIconwidth.Text = "";
            txtProductIconHeight.Text = "";
            txtProductLargeHeight.Text = "";
            txtProductLargeWidth.Text = "";
            txtProductMediumHeight.Text = "";
            txtProductMediumWidth.Text = "";
            txtProductMicroHeight.Text = "";
            txtProductMicroWidth.Text = "";
        }

        /// <summary>
        ///  Close Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnclose_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/ADMIN/Dashboard.aspx");
        }
    }
}