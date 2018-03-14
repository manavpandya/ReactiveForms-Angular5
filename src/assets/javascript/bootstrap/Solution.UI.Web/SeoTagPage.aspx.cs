using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web
{
    public partial class SeoTagPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCategory();
            }

        }
        private void FillCategory()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT name,CategoryId,Description FROM tb_category_seo Order By name");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcategory.DataSource = ds;
                ddlcategory.DataValueField = "CategoryId";
                ddlcategory.DataTextField = "Name";
                ddlcategory.DataBind();
                ViewState["Category"] = ds;
                GetDescription();
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["Searchkeyword"] = txtSearch.Text.ToString();
            grvMailReport.DataSource = null;
            grvMailReport.DataBind();
            if (ViewState["Category"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["Category"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    DataTable dt = new DataTable();
                    DataColumn col1 = new DataColumn("Keywords", typeof(string));
                    dt.Columns.Add(col1);

                    DataColumn col2 = new DataColumn("index", typeof(string));
                    dt.Columns.Add(col2);

                    DataRow[] dr = ds.Tables[0].Select("CategoryId=" + ddlcategory.SelectedValue.ToString() + "");
                    if (dr.Length > 0)
                    {
                        txtdescription.InnerHtml = System.Text.RegularExpressions.Regex.Replace(dr[0]["Description"].ToString().Trim(), @"<[^>]*>", String.Empty);
                        ViewState["desc"] = dr[0]["Description"].ToString();
                    }
                    else
                    {
                        ViewState["desc"] = null;
                    }
                    //string[] strall = System.Text.RegularExpressions.Regex.Split(System.Text.RegularExpressions.Regex.Replace(dr[0]["Description"].ToString().Trim(), @"<[^>]*>", String.Empty), txtSearch.Text.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    string str = dr[0]["Description"].ToString();
                    //if (strall.Length > 0)
                    //{

                    for (int index = 0; index < txtSearch.Text.ToString().Length; index++)
                    {
                        index = str.IndexOf(txtSearch.Text.ToString(), index);
                        if (index > -1)
                        {
                            DataRow dr1 = dt.NewRow();
                            dr1["Keywords"] = txtSearch.Text.ToString();
                            dr1["index"] = (index).ToString();
                            dt.Rows.Add(dr1);
                            dt.AcceptChanges();


                        }


                    }

                    //for (int i = 1; i < strall.Length; i++)
                    //{




                    //}
                    grvMailReport.DataSource = dt;
                    grvMailReport.DataBind();

                    // }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "highlight", "highlight('" + txtSearch.Text.ToString() + "');", true);
            }


        }
        private void GetDescription()
        {
            if (ViewState["Category"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["Category"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select("CategoryId=" + ddlcategory.SelectedValue.ToString() + "");
                    if (dr.Length > 0)
                    {
                        txtdescription.InnerHtml = System.Text.RegularExpressions.Regex.Replace(dr[0]["Description"].ToString().Trim(), @"<[^>]*>", String.Empty);
                    }
                }
            }
        }

        protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDescription();
        }

        protected void grvMailReport_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CheckBox chkSelect = (CheckBox)grvMailReport.Rows[e.NewEditIndex].FindControl("chkSelect");
            TextBox txtSearch1 = (TextBox)grvMailReport.Rows[e.NewEditIndex].FindControl("txtSearch");
            DropDownList ddlTag = (DropDownList)grvMailReport.Rows[e.NewEditIndex].FindControl("ddlTag");
            Label lblId = (Label)grvMailReport.Rows[e.NewEditIndex].FindControl("lblId");
            if (ViewState["Category"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["Category"];
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select("CategoryId=" + ddlcategory.SelectedValue.ToString() + "");
                    if (dr.Length > 0)
                    {
                        txtdescription.InnerHtml = dr[0]["Description"].ToString().Trim();
                        Response.Write(dr[0]["Description"].ToString().Trim().Substring(0, Convert.ToInt32(lblId.Text.ToString()) - Convert.ToInt32(txtSearch1.Text.ToString())));
                    }
                }
            }
        }

        protected void grvMailReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}