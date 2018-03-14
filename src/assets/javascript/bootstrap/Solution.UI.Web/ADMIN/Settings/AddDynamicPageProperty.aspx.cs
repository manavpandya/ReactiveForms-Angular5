using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AddDynamicPageProperty : BasePage
    {
        public int isadded = 0;
        public int isupdated = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]) && Convert.ToString(Request.QueryString["ID"]) != "0")
                {
                    FillDetails(Convert.ToInt32(Request.QueryString["ID"]));
                    lblTitle.Text = "Edit Dynamic Page Property";
                    lblTitle.ToolTip = "Edit Dynamic Page Property";
                }
                else
                {
                    DataSet Dsnew = new DataSet();
                    Dsnew = CommonComponent.GetCommonDataSet("select distinct isnull(pagename,'') as PageName,isnull(PageValue,'') as PageValue from tb_dynamicpagemaster where PageValue not in (select PageName from tb_DynamicPageProperty)");
                    if(Dsnew!=null && Dsnew.Tables.Count>0 && Dsnew.Tables[0].Rows.Count>0)
                    {
                        ddlpagename.DataSource = Dsnew.Tables[0];
                        ddlpagename.DataTextField = "PageName";
                        ddlpagename.DataValueField = "PageValue";
                        ddlpagename.DataBind();
                    }
                    else
                    {
                        ddlpagename.DataSource = null;
                        ddlpagename.Items.Clear();
                        ddlpagename.DataBind();
                    }
                    ddlpagename.Items.Insert(0, new ListItem("--Select--", "0"));

                }

            }
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
        }


        private void FillDetails(Int32 id)
        {
            DataSet dsevents = new DataSet();
            dsevents = CommonComponent.GetCommonDataSet("select * from tb_DynamicPageProperty where id=" + id + "");
            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                ddlpagename.SelectedValue = dsevents.Tables[0].Rows[0]["PageName"].ToString();
                txtpagetitle.Text = dsevents.Tables[0].Rows[0]["Title"].ToString();
                txtDescription.Text = Server.HtmlDecode(dsevents.Tables[0].Rows[0]["Description"].ToString());
                ddldescposition.SelectedValue = dsevents.Tables[0].Rows[0]["DescriptionPosition"].ToString();
                txtSETitle.Text = Convert.ToString(dsevents.Tables[0].Rows[0]["SEOTitle"].ToString());
                txtSEKeyword.Text = Convert.ToString(dsevents.Tables[0].Rows[0]["MetaKeyword"].ToString());
                txtSEDescription.Text = Convert.ToString(dsevents.Tables[0].Rows[0]["MetaDescription"].ToString());
                ddlpagename.Enabled = false;
            }
        }

        protected void btndelete_Click(object sender, ImageClickEventArgs e)
        {
        }
        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String value after Remove Special Character</returns>
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);
            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            res = value;
            return res;
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]) && Convert.ToString(Request.QueryString["ID"]) != "0")
            {
                CommonComponent.ExecuteCommonData("update tb_DynamicPageProperty set PageName='" + ddlpagename.SelectedValue.ToString() + "',Title='" + txtpagetitle.Text.Trim().ToString().Replace("'", "''") + "',Description='" + txtDescription.Text.Trim().ToString().Replace("'", "''") + "',DescriptionPosition='" + ddldescposition.SelectedValue.ToString() + "',SEOTitle='" + txtSETitle.Text.Trim().ToString().Replace("'", "''") + "',MetaKeyword='" + txtSEKeyword.Text.Trim().ToString().Replace("'", "''") + "',MetaDescription='" + txtSEDescription.Text.Trim().ToString().Replace("'", "''") + "' where ID=" + Request.QueryString["ID"].ToString() + "");
                Response.Redirect("DynamicPagePropertyList.aspx?status=updated");
            }
            else
            {
                string PageID =Convert.ToString(CommonComponent.GetScalarCommonData("select ID from tb_DynamicPageProperty where PageName='" + ddlpagename.SelectedValue.ToString() + "'"));
                if (PageID != null && PageID != "")
                { CommonComponent.ExecuteCommonData("update tb_DynamicPageProperty set PageName='" + ddlpagename.SelectedValue.ToString() + "',Title='" + txtpagetitle.Text.Trim().ToString().Replace("'", "''") + "',Description='" + txtDescription.Text.Trim().ToString().Replace("'", "''") + "',DescriptionPosition='" + ddldescposition.SelectedValue.ToString() + "',SEOTitle='" + txtSETitle.Text.Trim().ToString().Replace("'", "''") + "',MetaKeyword='" + txtSEKeyword.Text.Trim().ToString().Replace("'", "''") + "',MetaDescription='" + txtSEDescription.Text.Trim().ToString().Replace("'", "''") + "' where ID=" + PageID + ""); }
                else
                { CommonComponent.ExecuteCommonData("insert into tb_DynamicPageProperty(PageName,Title,Description,DescriptionPosition,SEOTitle,MetaKeyword,MetaDescription) values('" + ddlpagename.SelectedValue.ToString() + "','" + txtpagetitle.Text.Trim().ToString().Replace("'", "''") + "','" + txtDescription.Text.Trim().ToString().Replace("'", "''") + "','" + ddldescposition.SelectedValue.ToString() + "','" + txtSETitle.Text.Trim().ToString().Replace("'", "''") + "','" + txtSEKeyword.Text.Trim().ToString().Replace("'", "''") + "','" + txtSEDescription.Text.Trim().ToString().Replace("'", "''") + "')"); }
                Response.Redirect("DynamicPagePropertyList.aspx?status=inserted");
            }
        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("DynamicPagePropertyList.aspx");
        }
    }
}