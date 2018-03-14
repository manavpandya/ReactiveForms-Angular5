using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddProductRating : BasePage
    {
        #region Declaration
        tb_ShippingServices tblShippingService = null;
        ShippingComponent objShipping = null;
        int StoreId = 0;
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
                txtSKU.Text = "";
                bindproduct();
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindproductwithSKU();
        }


        /// <summary>
        ///  Bind Store Details in dropdown
        /// </summary>
        private void bindproductwithSKU()
        {
            DataSet productdetail = new DataSet();
            string name = "";
            if (!String.IsNullOrEmpty(txtSKU.Text))
            {
                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet("select ProductID,Name,SKU from tb_product where StoreID=1 and isnull(Deleted,0)=0 and isnull(active,0)=1 and (sku like '%" + txtSKU.Text.Replace("'", "''") + "%' OR name  like '%" + txtSKU.Text.Replace("'", "''") + "%')  ");


                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlproduct.DataSource = ds;
                    ddlproduct.DataTextField = "Name";
                    ddlproduct.DataValueField = "ProductID";
                    ddlproduct.DataBind();
                    ddlproduct.Items.Insert(0, new ListItem("Select Product", "-1"));
                    ddlproduct.SelectedIndex = 0;

                }
                else
                {
                    ddlproduct.DataSource = null;
                    ddlproduct.DataBind();

                }


            }
            else
            {
                ddlproduct.DataSource = null;
                ddlproduct.DataBind();
            }





        }
        /// <summary>
        ///  Bind Store Details in dropdown
        /// </summary>
        private void bindproduct()
        {

            DataSet productdetail = new DataSet();
            productdetail = CommonComponent.GetCommonDataSet("select ProductID,Name from tb_product where StoreID=1 and isnull(Deleted,0)=0 and isnull(active,0)=1");
            if (productdetail != null && productdetail.Tables.Count > 0 && productdetail.Tables[0].Rows.Count > 0)
            {
                ddlproduct.DataSource = productdetail;
                ddlproduct.DataTextField = "Name";
                ddlproduct.DataValueField = "ProductID";
                ddlproduct.DataBind();
            }
            ddlproduct.Items.Insert(0, new ListItem("Select Product", "-1"));
            ddlproduct.SelectedIndex = 0;
        }





        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            objShipping = new ShippingComponent();
            tblShippingService = new tb_ShippingServices();



            ProductComponent objRating = new ProductComponent();
            string strResult = "";
            int approve = 0;
         
            if (chkapprove.Checked)
            {
                approve = 1;
            }
            else
            {
                approve = 0;
            }

            int verified = 0;
            if (chkVerified.Checked)
            {
                verified = 1;
            }
            else
            {
                verified = 0;
            }
            CommonComponent.ExecuteCommonData("insert into tb_Rating(StoreID,EmailID,name,ProductID,CustomerID,Rating,Comments,IsApproved, isVerified,createdon) values (1,'" + txtemail.Text.ToString() + "','" + txtName.Text.ToString() + "'," + ddlproduct.SelectedValue.ToString() + "," + Convert.ToInt32(Session["AdminID"].ToString()) + "," + ddlrating.SelectedValue.ToString() + ",'" + txtcomment.Text.ToString().Replace("'", "''") + "'," + approve + ", "+ verified +",'" + txtStartDate.Text.ToString() + "')");

            Response.Redirect("ProductRating.aspx?status=inserted");


        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductRating.aspx");
        }
    }
}