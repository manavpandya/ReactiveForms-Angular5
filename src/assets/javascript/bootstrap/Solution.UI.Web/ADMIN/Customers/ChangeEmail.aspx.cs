using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Data;
using LumenWorks.Framework.IO.Csv;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class ChangeEmail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnupdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnshoworder.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtoldemail.Text.ToString()) && !String.IsNullOrEmpty(txtnewemail.Text.ToString()))
            {
                if (txtoldemail.Text.ToString().Trim().ToLower() == txtnewemail.Text.ToString().Trim().ToLower())
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "jAlert('Please Enter different Email.','Message','ContentPlaceHolder1_txtoldemail');", true);
                    return;
                }

                CommonComponent.ExecuteCommonData("Exec usp_changeemail '" + txtoldemail.Text.ToString().Trim() + "','" + txtnewemail.Text.ToString().Trim() + "',1");
                BindOldEmailOrders();
                BindNewEmailOrders();
                txtoldemail.Text = "";
                txtnewemail.Text = "";
                trupdate.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "jAlert('Email Merged Successfully.','Message','ContentPlaceHolder1_txtoldemail');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "jAlert('Please Enter Email.','Message','ContentPlaceHolder1_txtoldemail');", true);
            }
        }
        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtpassword.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RequiredPass", "jAlert('Please Enter Password.','Required');", true);
                txtpassword.Focus();
                return;
            }

            string pass = Convert.ToString(CommonComponent.GetScalarCommonData("select configvalue from tb_appconfig where configname='mergeemailpassword' and storeid=1"));
            if (!string.IsNullOrEmpty(pass))
            {
                if (txtpassword.Text.ToString().Trim() == pass.ToString().Trim())
                {


                    password.Visible = false;
                    divsearch.Visible = true;

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "IncorrectPass", "jAlert('Incorrect Password','Error');", true);
                }
            }

        }

        protected void btnshoworder_Click(object sender, EventArgs e)
        {
            lblmsg.Text = "";
            if (!String.IsNullOrEmpty(txtoldemail.Text.ToString()) && !String.IsNullOrEmpty(txtnewemail.Text.ToString()))
            {
                if (txtoldemail.Text.ToString().Trim().ToLower() == txtnewemail.Text.ToString().Trim().ToLower())
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "jAlert('Please Enter different Email.','Message','ContentPlaceHolder1_txtoldemail');", true);
                    return;
                }
                string ctype = getcustomertype(txtoldemail.Text.ToString());
                if (ctype.ToString().ToLower() != "registered")
                {
                    lblmsg.Text = "Customer Not Exist.";
                    grdold.DataSource = null;
                    grdold.DataBind();
                    grdnew.DataSource = null;
                    grdnew.DataBind();
                    grdold.Visible = false;
                    grdnew.Visible = false;
                    trupdate.Visible = false;
                    ltrold.Text = "";
                    ltrnew.Text = "";
                    return;
                }
                BindOldEmailOrders();
                BindNewEmailOrders();
                trupdate.Visible = true;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "jAlert('Please Enter Email.','Message','ContentPlaceHolder1_txtoldemail');", true);
            }


        }


        private void BindOldEmailOrders()
        {
            if (!String.IsNullOrEmpty(txtoldemail.Text.ToString()))
            {
                ltrold.Text = "Order Details for : <b>" + txtoldemail.Text.ToString() + "</b>";
                DataSet Dsold = new DataSet();
                Dsold = CommonComponent.GetCommonDataSet("Exec usp_changeemail '" + txtoldemail.Text.ToString().Trim() + "','" + txtnewemail.Text.ToString().Trim() + "',2");
                if (Dsold != null && Dsold.Tables.Count > 0 && Dsold.Tables[0].Rows.Count > 0)
                {
                    ltrold.Text += "     Total Order : <b>" + Dsold.Tables[0].Rows.Count + "</b>";
                    grdold.DataSource = Dsold.Tables[0];
                    grdold.DataBind();
                    grdold.Visible = true;
                    btnupdate.Visible = true;
                }
                else
                {
                    ltrold.Text += "     Total Order : <b>0</b>";
                    grdold.DataSource = null;
                    grdold.DataBind();
                    btnupdate.Visible = true;
                }
                string ctype = getcustomertype(txtoldemail.Text.ToString());
                if (ctype.ToString().ToLower() != "registered")
                {
                    ctype = "Customer Not Exist.";
                }
                ltrold.Text += "     Customer Type : <b>" + ctype + "</b>";

            }
        }

        private void BindNewEmailOrders()
        {
            if (!String.IsNullOrEmpty(txtnewemail.Text.ToString()))
            {
                ltrnew.Text = "Order Details for : <b>" + txtnewemail.Text.ToString() + "</b>";
                DataSet Dsnew = new DataSet();
                Dsnew = CommonComponent.GetCommonDataSet("Exec usp_changeemail '" + txtoldemail.Text.ToString().Trim() + "','" + txtnewemail.Text.ToString().Trim() + "',3");
                if (Dsnew != null && Dsnew.Tables.Count > 0 && Dsnew.Tables[0].Rows.Count > 0)
                {
                    ltrnew.Text += "     Total Order : <b>" + Dsnew.Tables[0].Rows.Count + "</b>";
                    grdnew.DataSource = Dsnew.Tables[0];
                    grdnew.DataBind();
                    grdnew.Visible = true;
                }
                else
                {
                    ltrnew.Text += "     Total Order : <b>0</b>";
                    grdnew.DataSource = null;
                    grdnew.DataBind();
                }


                string ctype = getcustomertype(txtnewemail.Text.ToString());
                ltrnew.Text += "     Customer Type : <b>" + ctype + "</b>";

            }
        }



        protected void grdold_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdold.PageIndex = e.NewPageIndex;
            BindOldEmailOrders();
        }
        protected void grdnew_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdnew.PageIndex = e.NewPageIndex;
            BindNewEmailOrders();
        }


        public string getcustomertype(string email)
        {
            string customertype = "";

            int count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(CustomerID) from tb_Customer where Email='" + email.ToString().Trim() + "' and isnull(Active,0)=1 and isnull(Deleted,0)=0 and isnull(IsRegistered,0)=1 and isnull(Email,'')<>''"));
            if (count > 0)
            {
                customertype = "Registered";
            }
            //else
            //{
            //    count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(CustomerID) from tb_Customer where Email='" + email.ToString().Trim() + "'  and isnull(Deleted,0)=0 and isnull(Email,'')<>''"));
            //    if (count > 0)
            //    {
            //        customertype = "Guest";
            //    }
            //}
            if (String.IsNullOrEmpty(customertype))
            {
                customertype = "Customer Not Exist";
            }
            return customertype;
        }
    }
}