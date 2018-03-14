using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;


namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HeaderText : BasePage
    {
       
            #region Variable declaration
        tb_HeaderText Header = null;
        HeaderComponent objHeaderComp = null;
       
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["HeaderID"]) && Convert.ToString(Request.QueryString["HeaderID"]) != "0")
                {
                    Header = new tb_HeaderText();
                    objHeaderComp = new HeaderComponent();
                    //Display selected Header detail for edit mode
                    Header = objHeaderComp.getHeaderByID(Convert.ToInt32(Request.QueryString["HeaderID"]));
                    lblHeader.Text = "Edit Header";
                    ckeditordescription.Text = Header.HeaderText;
                    txtHeaderName.Text = Header.DisplayOrder.ToString();
                    
                    if(Header.Active==true)
                    {
                        chkselect.Checked = true;
                    }
                    else
                    {
                        chkselect.Checked = false;
                    }
                   
                  
                }
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Header = new tb_HeaderText();
            objHeaderComp = new HeaderComponent();
            if (!string.IsNullOrEmpty(Request.QueryString["HeaderID"]) && Convert.ToString(Request.QueryString["HeaderID"]) != "0")
            {
                Header = objHeaderComp.getHeaderByID(Convert.ToInt32(Request.QueryString["HeaderID"]));
                lblHeader.Text = "Edit Header";
                Header.HeaderText = ckeditordescription.Text.Trim();
                Header.DisplayOrder = Convert.ToInt32(txtHeaderName.Text.Trim());
                if (chkselect.Checked)
                {
                    Header.Active = true;
                }
                else
                {
                    Header.Active = false;
                }
             
                Int32 isupdated = objHeaderComp.Update(Header);
                if (isupdated > 0)
                {
                    Response.Redirect("HeaderTextList.aspx?status=updated");
                }
            }
            else
            {
                Header.DisplayOrder = Convert.ToInt32(txtHeaderName.Text.Trim());
                if(chkselect.Checked)
                {
                    Header.Active = true;
                }
                else
                {
                    Header.Active = false;
                }
               
              
                //Check Header name already exists or not
                //if (objHeaderComp.CheckDuplicate(Header))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Header Name already exists.', 'Message');});", true);
                //    return;
                //}

                
                Header.HeaderText = ckeditordescription.Text.Trim();
                //Header.SEDescription = txtSEDescription.Text.Trim();
                //Header.SEKeywords = txtSEKeywords.Text.Trim();
                //Header.SETitle = txtSETitle.Text.Trim();
                //string sename = CommonOperations.RemoveSpecialCharacter(txtTitle.Text.Trim().ToCharArray());
                //Header.SEName = sename;
                //Header.CreatedBy = Convert.ToInt32(Session["AdminID"].ToString());
                //Header.CreatedOn = System.DateTime.Now;
                //Header.ShowOnSiteMap = chkShowOnSiteMap.Checked;
                //Header.Deleted = false;
                Int32 isadded = objHeaderComp.CreateHeader(Header);

                if (isadded > 0)
                {
                    Response.Redirect("HeaderTextList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        /// Bind Store Details with dropdown
        /// </summary>
        private void bindstore()
        {
            //StoreComponent objStorecomponent = new StoreComponent();
            //var storeDetail = objStorecomponent.GetStore();
            //if (storeDetail.Count > 0 && storeDetail != null)
            //{
            //    drpStoreName.DataSource = storeDetail;
            //    drpStoreName.DataTextField = "StoreName";
            //    drpStoreName.DataValueField = "StoreID";
            //    drpStoreName.DataBind();
            //}

        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("HeaderTextList.aspx");
        }

        
    }
}