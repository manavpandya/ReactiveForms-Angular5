using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.Collections;
namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class SetupInventoryFeeddata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                GetGropuCode();
            }
        }
        
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            divbutton.Visible = false;
            ReplenishmentFeedComponent objrep = new ReplenishmentFeedComponent();
            //DataSet dsReplishment = new DataSet();
            //dsReplishment = objrep.GetSearchData(ddlsubcategory.SelectedValue.ToString(), txtsku_upc.Text.ToString(), txtfabriccode.Text.ToString(), from.Text.ToString(), to.Text.ToString());
            //if (dsReplishment != null && dsReplishment.Tables.Count > 0 && dsReplishment.Tables[0].Rows.Count > 0)
            //{


            DataSet dsSku = new DataSet();

            string varaintsearchvalue = "";
            if (ddlsubcategory.SelectedIndex == 1 || ddlsubcategory.SelectedIndex == 0 || ddlsubcategory.SelectedValue.ToString() == "0")
            {
                if (txtfabriccode.Text.ToString() != "")
                {
                    varaintsearchvalue += " AND tb_ProductVariantValue.Productid in (SELECT Productid FROM tb_product WHERE Storeid=1 and isnull(deleted,0)=0 and NavFabric like '%" + txtfabriccode.Text.ToString().Replace("'", "''") + "%')";
                }
            }
            else
            {
                if (txtfabriccode.Text.ToString() != "")
                {
                    varaintsearchvalue += " AND tb_ProductVariantValue.Productid in (SELECT Productid FROM tb_product WHERE Storeid=1 and isnull(deleted,0)=0 and NavFabric like '%" + txtfabriccode.Text.ToString().Replace("'", "''") + "%' and NavProductGroup='" + ddlsubcategory.SelectedValue.ToString().Replace("'", "''") + "')";
                }
                else
                {
                    varaintsearchvalue += " AND tb_ProductVariantValue.Productid in (SELECT Productid FROM tb_product WHERE Storeid=1 and isnull(deleted,0)=0 and NavProductGroup ='" + ddlsubcategory.SelectedValue.ToString().Replace("'", "''") + "')";
                }

            }
            string skusearch = "";
            string varskusearch = "";
            if (txtsku_upc.Text.ToString() != "")
            {
                skusearch += " AND (SKU like '%" + txtsku_upc.Text.ToString().Replace("'", "''") + "%' OR UPC like '%" + txtsku_upc.Text.ToString().Replace("'", "''") + "%')";
            }
            if (txtsku_upc.Text.ToString() != "")
            {
                varskusearch += " AND (tb_ProductVariantValue.SKU like '%" + txtsku_upc.Text.ToString().Replace("'", "''") + "%' OR tb_ProductVariantValue.UPC like '%" + txtsku_upc.Text.ToString().Replace("'", "''") + "%')";
            }
            string daterange = "";
            if (from.Text.ToString() != "" && to.Text.ToString() != "")
            {
                daterange += " AND  cast(createdOn as date) >= cast('" + from.Text.ToString() + "' as date) and cast(createdOn as date) <= cast('" + to.Text.ToString() + "' as date)";
            }


            dsSku = CommonComponent.GetCommonDataSet(@" SELECT   replace(replace(tb_ProductVariantValue.SKU,'-108','-97108~'),'-120','-98120~') as SKU,tb_ProductVariantValue.UPC as UPC, isnull(tb_ProductVariantValue.Displayorder,0) as displayorder  FROM tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId=tb_ProductVariantValue.ProductID 
		WHERE isnull(StoreId,0)=1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and isnull(tb_ProductVariantValue.SKU,'')<>'' and isnull(tb_ProductVariantValue.UPC,'')<>'' " + varaintsearchvalue + varskusearch + @"
		 UNION 
		 SELECT     tb_Product.SKU as SKU,tb_Product.UPC as UPC,9999 as displayorder FROM tb_product WHERE     isnull(Deleted,0)=0 and storeid in (1) and isnull(tb_Product.SKU,'') <> '' and isnull(tb_Product.UPC,'')<>'' " + skusearch + daterange + @"
	 Order By SKU,displayorder");


            DataTable dtmain = new DataTable();

            string getstring = "";
            dtmain = dsSku.Tables[0];
            string strstorename = "";
            ArrayList strStoreName = new ArrayList();

            ArrayList strstoreidarry = new ArrayList();
            if (dsSku != null && dsSku.Tables.Count > 0 && dsSku.Tables[0].Rows.Count > 0)
            {



                DataSet dsStore = new DataSet();
                dsStore = CommonComponent.GetCommonDataSet("select * from tb_Replenishment_Store where isnull(Deleted,0)<>1 Order BY isnull(StoreName,'') Asc");

                if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsStore.Tables[0].Rows.Count; i++)
                    {

                        if (dsStore.Tables[0].Rows[i]["RepStoreID"].ToString()=="1")
                        {
                            dtmain.Columns.Add(dsStore.Tables[0].Rows[i]["StoreName"].ToString().Trim().Replace("ECOMMERCE", "").Replace("E-COMMERCE", "").Replace("Master", ""), typeof(string));
                            strStoreName.Add(dsStore.Tables[0].Rows[i]["StoreName"].ToString().Trim().Replace("ECOMMERCE", "").Replace("E-COMMERCE", "").Replace("Master", ""));
                        }
                        else
                        {
                            dtmain.Columns.Add(dsStore.Tables[0].Rows[i]["StoreName"].ToString().Replace("Half Price Drapes", "").Replace("HPD", ""), typeof(string));
                            strStoreName.Add(dsStore.Tables[0].Rows[i]["StoreName"].ToString().Replace("Half Price Drapes", "").Replace("HPD", ""));
                        }
                      
                        strstoreidarry.Add(dsStore.Tables[0].Rows[i]["RepStoreID"].ToString());

                    }
                    // ltrCart.Text += "</tr>";
                    string strskku = ",";
                    for (int i = 0; i < dtmain.Rows.Count; i++)
                    {
                        string sku = dtmain.Rows[i]["UPC"].ToString().Trim();
                        dtmain.Rows[i]["SKU"] = dtmain.Rows[i]["SKU"].ToString().Replace("-97108~", "-108").Replace("-98120~", "-120");
                        dtmain.AcceptChanges();
                        if (strskku.ToString().ToLower().IndexOf("," + dtmain.Rows[i]["SKU"].ToString().ToLower() + ",") > -1)
                        {
                            dtmain.Rows.RemoveAt(i);
                            dtmain.AcceptChanges();
                            i--;
                            continue;
                        }
                        else
                        {
                            strskku += dtmain.Rows[i]["SKU"].ToString() + ",";
                        }



                        getstring = Convert.ToString(CommonComponent.GetScalarCommonData("exec usp_Replenishment_getstring '" + sku + "' "));

                        if (!String.IsNullOrEmpty(getstring))
                        {
                            string[] strarray = getstring.Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < strarray.Length; j++)
                            {
                                string[] strResult = strarray[j].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                for (int k = 0; k < strResult.Length; k++)
                                {
                                    string strstatus = strResult[0];
                                    string strstoreid = strResult[1];
                                    strstorename = strResult[2];
                                    bool active = false;
                                   // dtmain.Rows[i][strstorename.Replace("Half Price Drapes", "").Replace("HPD", "")] = strstatus.ToString();
                                    try
                                    {
                                        dtmain.Rows[i][strstorename.Replace("Half Price Drapes", "")] = strstatus.ToString();
                                    }
                                    catch { }
                                }

                            }

                            for (int l = 3; l < dtmain.Columns.Count; l++)
                            {
                                if (dtmain.Rows[i][l] == DBNull.Value)
                                {
                                    dtmain.Rows[i][l] = "";

                                }

                            }
                        }

                        else
                        {
                            for (int l = 3; l < dtmain.Columns.Count; l++)
                            {
                                dtmain.Rows[i][l] = "";
                            }
                        }


                    }

                }

            }
            else
            {
                divallcheckbox.InnerHtml = "";
                divbutton.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgunsuccess", "alert('No Result Found.');", true);
                lblMsg.Text = "No Result Found.";
                return;
            }
            string strMain = "";
            strMain += "<table class=\"table table-bordered table-striped table-condensed cf table-td-whitespace\"><thead class=\"cf\">";

            strMain += "<tr>";

            strMain += "<th>Select All <div class=\"icheck minimal\" id=\"chkdiv_0\" onclick=\"checkallcheckbox('chk_0');\"><input type=\"checkbox\" onchange=\"checkallcheckbox('chk_0');\" id=\"chk_0\" name=\"chk_0\"  /></div></th>";
            for (int j = 0; j < dtmain.Columns.Count; j++)
            {
                //if (j == 0)
                //{
                //    strMain += "<th>" + dtmain.Columns[j].ColumnName.ToString() + "<div class=\"checkbox icheck minimal\"></div></th>";
                //}
                //else if (j == 1)
                //{
                //    strMain += "<th>" + dtmain.Columns[j].ColumnName.ToString() + "<div class=\"checkbox icheck minimal\"></div></th>";
                //}
                if (j == 0)
                {
                    strMain += "<th  style=\"vertical-align:top;\">" + dtmain.Columns[j].ColumnName.ToString() + "</th>";
                }
                else if (j == 1)
                {
                    strMain += "<th style=\"vertical-align:top;\">" + dtmain.Columns[j].ColumnName.ToString() + "</th>";
                }
                else if (j != 2)
                {
                    string strsId = "";
                    if (strStoreName.Count > 0)
                    {
                        for (int icount = 0; icount < strStoreName.Count; icount++)
                        {
                            if (dtmain.Columns[j].ColumnName.ToString().ToLower().Trim() == strStoreName[icount].ToString().ToLower().Trim())
                            {
                                strsId = strstoreidarry[icount].ToString();
                                break;
                            }

                        }
                    }
                    strMain += "<th>" + dtmain.Columns[j].ColumnName.ToString() + " <div class=\"icheck minimal\" onclick=\"testtt('chk_" + (j + 1).ToString() + "');\" id=\"chkdiv_" + (j + 1).ToString() + "\"><input type=\"checkbox\" onchange=\"testtt('chk_" + (j + 1).ToString() + "');\" id=\"chk_" + (j + 1).ToString() + "\"  name=\"chk_" + (j + 1).ToString() + "\"></div></th>";
                }

            }
            strMain += "</thead></tr><tbody>";
            for (int i = 0; i < dtmain.Rows.Count; i++)
            {
                strMain += "<tr id=\"trchk_" + i.ToString() + "\">";
                strMain += "<td data-title=\"Select All\" style=\"text-align:center\"><div class=\"icheck minimal\" id=\"chkdiv_0_" + i.ToString() + "\" onclick=\"checkallcheckbox('chk_0_" + i.ToString() + "');\"><input tabindex=\"3\" onchange=\"checkallcheckbox('chk_0_" + i.ToString() + "');\" id=\"chk_0_" + i.ToString() + "\" type=\"checkbox\"  name=\"chk_0_" + i.ToString() + "\"></div></td>";
                for (int j = 0; j < dtmain.Columns.Count; j++)
                {
                    if (j != 2)
                    {
                        if (j == 0 || j == 1)
                        {
                            strMain += "<td data-title=\"" + dtmain.Rows[i][j].ToString() + "\">" + dtmain.Rows[i][j].ToString() + "</td>";
                        }
                        else
                        {
                            string strsId = "";
                            if (strStoreName.Count > 0)
                            {
                                for (int icount = 0; icount < strStoreName.Count; icount++)
                                {
                                    if (dtmain.Columns[j].ColumnName.ToString().ToLower().Trim() == strStoreName[icount].ToString().ToLower().Trim())
                                    {
                                        strsId = strstoreidarry[icount].ToString();
                                        break;
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(dtmain.Rows[i][j].ToString()))
                            {
                                strMain += "<td data-title=\"" + dtmain.Rows[i][j].ToString() + "\" style=\"text-align:center\"><div class=\"icheck minimal\" onclick=\"checkallcheckboxchild('chk_" + (j + 1).ToString() + "_" + i.ToString() + "','chk_" + (j + 1).ToString() + "');\" id=\"chkdiv_" + (j + 1).ToString() + "_" + i.ToString() + "\"><input tabindex=\"3\" Checked=\"checked\" onchange=\"checkallcheckboxchild('chk_" + (j + 1).ToString() + "_" + i.ToString() + "','chk_" + (j + 1).ToString() + "');\" id=\"chk_" + (j + 1).ToString() + "_" + i.ToString() + "\" type=\"checkbox\"  name=\"chk_" + (j + 1).ToString() + "_" + i.ToString() + "\"><input type=\"hidden\" id=\"hdnsku_" + (j + 1).ToString() + "_" + i.ToString() + "\" name=\"hdnsku_" + (j + 1).ToString() + "_" + i.ToString() + "\" value=\"" + dtmain.Rows[i][0].ToString() + "\" /><input type=\"hidden\" id=\"hdnstoreid_" + (j + 1).ToString() + "_" + i.ToString() + "\" name=\"hdnstoreid_" + (j + 1).ToString() + "_" + i.ToString() + "\" value=\"" + strsId.ToString() + "\" /></div></td>";
                            }
                            else
                            {
                                strMain += "<td data-title=\"" + dtmain.Rows[i][j].ToString() + "\" style=\"text-align:center\"><div class=\"icheck minimal\" onclick=\"checkallcheckboxchild('chk_" + (j + 1).ToString() + "_" + i.ToString() + "','chk_" + (j + 1).ToString() + "');\" id=\"chkdiv_" + (j + 1).ToString() + "_" + i.ToString() + "\"><input tabindex=\"3\" onchange=\"checkallcheckboxchild('chk_" + (j + 1).ToString() + "_" + i.ToString() + "','chk_" + (j + 1).ToString() + "');\" id=\"chk_" + (j + 1).ToString() + "_" + i.ToString() + "\" type=\"checkbox\" name=\"chk_" + (j + 1).ToString() + "_" + i.ToString() + "\" ><input type=\"hidden\" id=\"hdnsku_" + (j + 1).ToString() + "_" + i.ToString() + "\" name=\"hdnsku_" + (j + 1).ToString() + "_" + i.ToString() + "\" value=\"" + dtmain.Rows[i][0].ToString() + "\" /><input type=\"hidden\" id=\"hdnstoreid_" + (j + 1).ToString() + "_" + i.ToString() + "\" name=\"hdnstoreid_" + (j + 1).ToString() + "_" + i.ToString() + "\" value=\"" + strsId.ToString() + "\" /></div></td>";
                            }
                        }
                    }



                }
                strMain += "</tr>";
            }
            strMain += "</tbody></table>";
            //divallcheckbox. = strMain;
            divallcheckbox.InnerHtml = strMain;
            if (dtmain != null && dtmain.Rows.Count > 0)
            {
                divbutton.Visible = true;
            }

            //}
        }
        [System.Web.Services.WebMethod]
        public static string GetData(string SKU, String StoreId, Int32 IsActive)
        {
            string resp = string.Empty;


            return "";
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //string[] formkeys = Request.Form.AllKeys;
            //Int32 sku = 0;
            //Int32 sid = 0;
            //Int32 hid = 0;
            //String[] strSKu = { "" };
            //String[] strstoreId = { "" };
            //String[] stractive = { "" };
            //foreach (String s in formkeys)
            //{

            //    if (s.ToLower().IndexOf("chk_0") <= -1)
            //    {
            //        if (s.ToLower().IndexOf("chk_") > -1)
            //        {


            //            if (s.ToLower().IndexOf("chk_") > -1)
            //            {

            //                string strall = Request.Form[s.ToString()].ToString();

            //            }
            //            else if (s.ToLower().IndexOf("hdnsku_") > -1)
            //            {
            //                sku = 1;
            //            }
            //            else if (s.ToLower().IndexOf("hdnstoreid_") > -1)
            //            {

            //            }
            //            //if (sku == 1 && sid == 1 && hid == 1)
            //            //{
            //            //    sku = 0;
            //            //    sid = 0;
            //            //    hid = 0;
            //            //}
            //        }
            //    }
            //    else
            //    {

            //    }
            //}
        }
        private void GetGropuCode()
        {
            DataSet dscode = new DataSet();
            dscode = CommonComponent.GetCommonDataSet(" SELECT 'ALL' as ProductGroupCode UNION  SELECT DISTINCT ProductGroupCode as ProductGroupCode FROM tb_NavProductGroupCode WHERE isnull(Active,0)=1 Order By  ProductGroupCode");

            if (dscode != null && dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                ddlsubcategory.DataSource = dscode;
                ddlsubcategory.DataTextField = "ProductGroupCode";
                ddlsubcategory.DataValueField = "ProductGroupCode";
                ddlsubcategory.DataBind();
            }
            else
            {
                ddlsubcategory.DataSource = null;

                ddlsubcategory.DataBind();
            }
            ddlsubcategory.Items.Insert(0, new ListItem("Select Class or Group", "0"));


        }

    }
}