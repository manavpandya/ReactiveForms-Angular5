using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Text;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class Wishlist : BasePage
    {
        #region Variable declaration
        public static bool isDescendEmail = false;
        public static bool isDescendName = false;
        System.Web.UI.WebControls.Literal ltrvartable = null;
        string strIds = ",";
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblEmailMsg.Text = "";
            if (!IsPostBack)
            {

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 58px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                GetCartDetail();
            }
            Page.Form.DefaultButton = btnSearch.UniqueID;

        }


        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdWishList.PageIndex = 0;
            GetCartDetail();
        }

        /// <summary>
        /// Show All Record by Selected Date
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grdWishList.PageIndex = 0;
            ddlSearch.SelectedIndex = 0;
            txtSearch.Text = "";
            GetCartDetail();
        }

        /// <summary>
        /// Delete Multiple record By Single click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow gr in grdWishList.Rows)
            {
                Label lblCustomerID = (Label)gr.FindControl("lblCustomerID");
                CheckBox chkRecord = (CheckBox)gr.FindControl("chkSelect");
                if (chkRecord.Checked)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_WishListItems where CustomerID=" + lblCustomerID.Text);
                }
            }
            grdWishList.PageIndex = 0;
            GetCartDetail();
        }


        /// <summary>
        /// Bind Store drop down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }



        /// <summary>
        /// Gets the cart detail.
        /// </summary>
        public void GetCartDetail()
        {
            string strSql = "SELECT   ROW_NUMBER() OVER (ORDER BY tb_Customer.CustomerID DESC) AS id, dbo.tb_Customer.CustomerID, isnull(dbo.tb_Customer.FirstName, " +
                                " 'Unregistered user') as FirstName , isnull(dbo.tb_Customer.LastName,'') as LastName, (select TOP 1 CreateDate from tb_WishListItems WHERE CustomerID=tb_Customer.CustomerID oRDER bY CreateDate desc) as CreateDate, isnull(dbo.tb_Customer.Email,'') as Email, dbo.tb_Customer.StoreID, " +
                             " dbo.tb_store.StoreName, isnull(dbo.tb_Customer.IsRegistered,0) as IsRegistered, isnull(dbo.tb_Customer.Active,0) as Active, isnull(dbo.tb_Customer.Deleted,0) as Deleted, count(dbo.tb_WishListItems.Quantity) as " +
                              " products,sum(dbo.tb_WishListItems.Quantity) as Quantity FROM  dbo.tb_Customer INNER JOIN dbo.tb_WishListItems ON dbo.tb_Customer.CustomerID = dbo.tb_WishListItems.CustomerID " +
                            " INNER JOIN dbo.tb_store ON dbo.tb_Customer.StoreID = dbo.tb_store.StoreID   where tb_Customer.Email  is not null";
            if (ddlStore.SelectedIndex > 0)
            {
                strSql += " and tb_Customer.storeid=" + ddlStore.SelectedValue.ToString() + "";
            }
            //if (ddlSearch.SelectedIndex > 0 && txtSearch.Text.ToString().ToLower() != "search keyword")
            if (txtSearch.Text.ToString().ToLower() != "")
            {
                strSql += " AND " + ddlSearch.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%' ";
            }
            strSql += " group by tb_Customer.CustomerID,dbo.tb_Customer.FirstName,dbo.tb_Customer.LastName," +
                             " dbo.tb_Customer.Email,tb_Customer.StoreID,dbo.tb_store.StoreName, dbo.tb_Customer.IsRegistered, dbo.tb_Customer.Active, dbo.tb_Customer.Deleted  " +
                             "order by  CreateDate desc";


            DataSet dsMail = new DataSet();
            dsMail = CommonComponent.GetCommonDataSet(strSql);

            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                grdWishList.DataSource = dsMail;
                grdWishList.DataBind();
                trdelete.Visible = true;
            }
            else
            {
                trdelete.Visible = false;
                grdWishList.DataSource = null;
                grdWishList.DataBind();
            }



        }


        /// <summary>
        /// Wish List Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdWishList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdWishList.PageIndex = e.NewPageIndex;
            GetCartDetail();
        }

        /// <summary>
        /// Wish List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdWishList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnEmail = (ImageButton)e.Row.FindControl("btnEmail");
                Label lblCustomerID = (Label)e.Row.FindControl("lblCustomerID");
                btnEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Email-Reply.jpg";
                btnEmail.OnClientClick = "return OpenViewCarttoSendMail(" + lblCustomerID.Text + ");";
                if(strIds.IndexOf(","+lblCustomerID.Text.ToString().Trim()+"," ) > -1)
                {
                    e.Row.Visible = false;
                }
                else
                {
                    strIds = strIds + lblCustomerID.Text.ToString().Trim() + ",";
                }
            }
        }

        /// <summary>
        /// Wish List Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdWishList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Email")
            {
                string[] temp = e.CommandArgument.ToString().Split('*');
                // SendMail(temp[0].ToString(), temp[1].ToString(), temp[2].ToString(), Convert.ToInt32(temp[3].ToString()));
            }
        }


        #region Bind Variant for product

        /// <summary>
        /// Display the Variant for Product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns the Literal controls which display product variant</returns>
        public System.Web.UI.WebControls.Literal BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            ltrvartable = new System.Web.UI.WebControls.Literal();
            if (varname.Length > 0)
            {

                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
            }
            if (sbvartable.ToString() != "")
            {
                ltrvartable.Text = sbvartable.ToString();
            }
            else
            {
                ltrvartable.Text = "";
            }
            return ltrvartable;
        }

        #endregion

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            GetCartDetail();
            if (grdWishList.Rows.Count == 0)
                trdelete.Visible = false;
        }
    }
}