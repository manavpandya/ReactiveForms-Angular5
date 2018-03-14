using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;


namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class Configurepopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "0")
                {
                    GetExistingdata(Convert.ToInt32(Request.QueryString["id"].ToString()));
                }
                else
                {
                    if (Session["configure"] != null && Request.QueryString["new"] != null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)Session["configure"];
                        if (dt != null)
                        {
                            if (Request.QueryString["new"] != null)
                            {
                                DataRow[] dr = dt.Select("id=" + Request.QueryString["new"].ToString() + "");
                                if (dr != null && dr.Length > 0)
                                {
                                    for (int i = 0; i < dr.Length; i++)
                                    {
                                        if (Convert.ToBoolean(dr[i]["Dbvalue"].ToString()))
                                        {
                                            txtyes.Text = dr[i]["Assignedvalue"].ToString();
                                        }
                                        else
                                        {
                                            txtno.Text = dr[i]["Assignedvalue"].ToString();
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
            }
        }
        private void GetExistingdata(Int32 FieldID)
        {
            DataSet dsconfigure = new DataSet();
            dsconfigure = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID=" + FieldID + "");
            if (dsconfigure != null && dsconfigure.Tables.Count > 0 && dsconfigure.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsconfigure.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsconfigure.Tables[0].Rows[i]["Dbvalue"].ToString()))
                    {
                        if (Convert.ToBoolean(dsconfigure.Tables[0].Rows[i]["Dbvalue"].ToString()))
                        {
                            txtyes.Text = dsconfigure.Tables[0].Rows[i]["Assignedvalue"].ToString();
                        }
                        else
                        {
                            txtno.Text = dsconfigure.Tables[0].Rows[i]["Assignedvalue"].ToString();
                        }

                    }
                }
            }
        }

        protected void mapping_btn_next_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "0")
            {

                CommonComponent.ExecuteCommonData("if(NOT EXISTS(SELECT ConfigureId FROM tb_Replenishment_configure WHERE  FieldID=" + Request.QueryString["id"].ToString() + ")) begin INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + Request.QueryString["id"].ToString() + ",1,'" + txtyes.Text.ToString().Replace("'", "''") + "'); INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + Request.QueryString["id"].ToString() + ",0,'" + txtno.Text.ToString().Replace("'", "''") + "') end else begin  update tb_Replenishment_configure SET Assignedvalue='" + txtyes.Text.ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=1 and FieldID=" + Request.QueryString["id"].ToString() + "; update tb_Replenishment_configure SET Assignedvalue='" + txtno.Text.ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=0 and FieldID=" + Request.QueryString["id"].ToString() + " end");

            }
            else
            {
                DataTable dt = new DataTable();
                if (Session["configure"] != null)
                {

                    dt = (DataTable)Session["configure"];
                    if (dt != null)
                    {
                        if (Request.QueryString["new"] != null)
                        {
                            DataRow[] dr = dt.Select("id=" + Request.QueryString["new"].ToString() + "");
                            if (dr != null && dr.Length > 0)
                            {
                                for (int i = 0; i < dr.Length; i++)
                                {
                                    if (Convert.ToBoolean(dr[i]["Dbvalue"].ToString()))
                                    {
                                        dr[i]["Assignedvalue"] = txtyes.Text.ToString();
                                    }
                                    else
                                    {
                                        dr[i]["Assignedvalue"] = txtno.Text.ToString();
                                    }
                                }
                                dt.AcceptChanges();
                                Session["configure"] = dt;
                            }
                            else
                            {
                                if (Request.QueryString["new"] != null)
                                {
                                    DataRow dr1 = dt.NewRow();
                                    dr1["FieldID"] = 0;
                                    dr1["id"] = Request.QueryString["new"].ToString();
                                    dr1["Dbvalue"] = 1;
                                    dr1["Assignedvalue"] = txtyes.Text.ToString();
                                    dt.Rows.Add(dr1);

                                    dr1 = dt.NewRow();
                                    dr1["FieldID"] = 0;
                                    dr1["id"] = Request.QueryString["new"].ToString();
                                    dr1["Dbvalue"] = 0;
                                    dr1["Assignedvalue"] = txtno.Text.ToString();

                                    dt.Rows.Add(dr1);
                                    Session["configure"] = dt;
                                }
                            }

                        }
                    }
                }
                else
                {
                    DataColumn col1 = new DataColumn("FieldID", typeof(int));
                    dt.Columns.Add(col1);
                    DataColumn col2 = new DataColumn("id", typeof(int));
                    dt.Columns.Add(col2);
                    DataColumn col3 = new DataColumn("Dbvalue", typeof(bool));
                    dt.Columns.Add(col3);
                    DataColumn col4 = new DataColumn("Assignedvalue", typeof(string));
                    dt.Columns.Add(col4);
                    if (Request.QueryString["new"] != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FieldID"] = 0;
                        dr["id"] = Request.QueryString["new"].ToString();
                        dr["Dbvalue"] = 1;
                        dr["Assignedvalue"] = txtyes.Text.ToString();
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr["FieldID"] = 0;
                        dr["id"] = Request.QueryString["new"].ToString();
                        dr["Dbvalue"] = 0;
                        dr["Assignedvalue"] = txtno.Text.ToString();

                        dt.Rows.Add(dr);
                        Session["configure"] = dt;
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "success", "alert('Record Saved Successfully.');window.close();", true);
        }
    }
}