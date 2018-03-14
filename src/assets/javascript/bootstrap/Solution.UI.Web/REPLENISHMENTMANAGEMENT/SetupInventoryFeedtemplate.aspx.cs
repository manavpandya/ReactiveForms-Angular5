using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.IO;
using System.Text;
using System.Data.SqlClient;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class SetupInventoryFeedtemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            if (!IsPostBack)
            {
                Session["configure"] = null;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "loadsetupprobox", "$('#setup-probox').fadeIn('slow');", true);
                HTmltempalete();
                ddlstore.Focus();
                GetStoreList();
                GetExistingStoreList();
            }
        }
        /// <summary>
        /// Get store List From Store master
        /// </summary>
        private void GetStoreList()
        {
            GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();

            DataSet dsStore = new DataSet();
            dsStore = objInv.GetSalesPartnerList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlstore.DataSource = dsStore;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "RepStoreID";
                ddlstore.DataBind();
            }
            else
            {
                ddlstore.DataSource = null;
                ddlstore.DataBind();
            }
            ddlstore.Items.Insert(0, new ListItem("Select Channel Partner", "0"));

        }
        private void HTmltempalete()
        {
            string strallhtml = "<div class=\"panel fa-border p-top-20\" id=\"div_##kau###\"><input type=\"hidden\" id=\"hdn_##kau###\" name=\"hdn_##kau###\" value=\"0\" />";
            strallhtml += "<div class=\"form-group\">";
            strallhtml += "<label class=\"control-label col-md-3\">Column Name</label>";
            strallhtml += "<div class=\"col-md-3\">";
            strallhtml += "<input type=\"text\" class=\"form-control\" name=\"column_name_##kau###\" id=\"column_name_##kau###\" >";
            strallhtml += "</div>";

            strallhtml += "<div class=\"col-md-3\">";
            strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnremove_##kau###\"  onclick=\"removecolumn(##kau###);\" style=\"display:none;\">Remove Column</button>";
            strallhtml += "</div>";

            strallhtml += "</div>";
            strallhtml += "<div class=\"form-group\">";
            strallhtml += "<label class=\"control-label col-md-3\">Attributes</label>";
            strallhtml += "<div class=\"col-md-6 icheck minimal\">";
            strallhtml += "<div class=\"row\">";
            strallhtml += "<label class=\"checkbox-inline\">";
            strallhtml += "<div class=\"row\">";
            strallhtml += "<div class=\"radio single-row\">";
            strallhtml += "<input tabindex=\"3\" type=\"radio\" onchange=\"javascript:checkparent(this);\"  name=\"demo-radio_##kau###\" id=\"demo-radio_1_##kau###\" value=\"Yes\" />";
            strallhtml += "<label>Required</label>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "</label>";
            strallhtml += "<label class=\"checkbox-inline\">";
            strallhtml += "<div class=\"row\">";
            strallhtml += "<div class=\"radio single-row\">";
            strallhtml += "<input tabindex=\"3\" type=\"radio\"  onchange=\"javascript:checkparent(this);\" name=\"demo-radio_##kau###\"  id=\"demo-radio_2_##kau###\" value=\"No\" />";
            strallhtml += "<label>Optional</label>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "</label>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "<div class=\"form-group\">";
            strallhtml += "<label class=\"control-label col-md-3\">Column Data</label>";
            strallhtml += "<div class=\"col-md-7\">";
            strallhtml += "<div class=\"form-control-static\">";
            strallhtml += "<div class=\"row\">";
            strallhtml += "<div class=\"col-sm-7\">";
            strallhtml += "<div class=\"col-xs-8\">";
            strallhtml += "<div class=\"row\">";
            strallhtml += "<select class=\"form-control\" name=\"select_##kau###\" id=\"select_##kau###\">";
            DataSet dspColumn = new DataSet();

            ReplenishmentFeedComponent objrep = new ReplenishmentFeedComponent();
            dspColumn = objrep.GetMappingColumnName();
            strallhtml += "<option value=\"\">Select Column Data</option>";
            //if (dspColumn != null && dspColumn.Tables.Count > 0 && dspColumn.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dspColumn.Tables[0].Rows.Count; i++)
            //    {
            //        strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[i]["columndata"].ToString() + "\">" + dspColumn.Tables[0].Rows[i]["columndata"].ToString() + "</option>";
            //    }

            //}
            strallhtml += "<option>SKU</option>";
            strallhtml += "<option>Channel Partner SKU</option>";
            strallhtml += "<option>GS1 ID</option>";
            strallhtml += "<option>Product Name/Description</option>";
            strallhtml += "<option>Price</option>";
            strallhtml += "<option>Quantity Available for Channel Partner</option>";
            strallhtml += "<option>Quantity On Order1</option>";
            strallhtml += "<option>Item Availability Date1</option>";
            strallhtml += "<option>Quantity On Order2</option>";
            strallhtml += "<option>Item Availability Date2</option>";
            strallhtml += "<option>Quantity On Order3</option>";
            strallhtml += "<option>Item Availability Date3</option>";
            strallhtml += "<option>Quantity On Order4</option>";
            strallhtml += "<option>Item Availability Date4</option>";
            strallhtml += "<option>Discontinued Status</option>";
            strallhtml += "<option>Active or Inactive Status</option>";
            strallhtml += "<option>OptionSku</option>";
            strallhtml += "</select>";
            strallhtml += "</div>";
            strallhtml += " </div>";
            strallhtml += "<div class=\"col-xs-4\" style=\"display:none;\">";
            strallhtml += "<a class=\"fancybox fancybox.iframe\" onclick=\"windowoprnincenter('Configurepopup.aspx?new=##kau###&id=0','600','350');\" href=\"javascript:void(0);\"> <button class=\"btn btn-orang\" type=\"button\">Configure</button></a>";
            strallhtml += "</div>";
            strallhtml += "<span class=\"help-block\" style=\"width:100% !important;float:left;\"><em>Dynamic Value</em></span> </div>";
            strallhtml += "<div class=\"col-sm-5\">";
            strallhtml += "<input type=\"text\" class=\"form-control\" name=\"txtstatic_##kau###\" id=\"txtstatic_##kau###\" onchange=\"changecolumndata('txtstatic_##kau###');\" onkeyup=\"changecolumndata('txtstatic_##kau###');\" />";
            strallhtml += "<span class=\"help-block\"><em>Static Value</em></span></div>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "</div>";
            strallhtml += "</div>";

            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">&nbsp;</label><div class=\"col-md-3\"><div class=\"radio single-row\"><input type=\"checkbox\" onchange=\"configuredatacolumn(##kau###);\" id=\"chkcolumn_##kau###\" name=\"chkcolumn_##kau###\" value=\"No\"> <label>Configure Data</label></div></div></div>";

            strallhtml += "<div id=\"divconfigure_##kau###\" style=\"display:none;\">";
            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">DB Value</label><div class=\"col-md-3\" style=\"margin-top:5px;\">New Value</div> </div>";
            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">True</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueyes_##kau###\" name=\"txtdbvalueyes_##kau###\" value=\"\" class=\"form-control\"></div> </div>";
            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">False</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueno_##kau###\" name=\"txtdbvalueno_##kau###\" value=\"\" class=\"form-control\"></div> </div>";
            strallhtml += "</div>";

            strallhtml += "</div>";



            strallhtml += "<div class=\"form-group\" id=\"divgroup_##kau###\">";
            strallhtml += "<label class=\"control-label col-md-3\">&nbsp;</label>";
            strallhtml += "<div class=\"col-md-9\">";
            strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnmore_##kau###\" onclick=\"addmorecolumn(##kau_new###);\">Add More Columns</button>";

            strallhtml += "</div>";
            strallhtml += "</div>";
            ltrgroupdata.Text = strallhtml.Replace("##kau###", "0").Replace("##kau_new###", "1");
            divcolumndata.InnerHtml = strallhtml;
        }
        private void GetExistingStoreList()
        {
            ReplenishmentFeedComponent objReplishment = new ReplenishmentFeedComponent();
            DataSet dsStore = new DataSet();
            dsStore = objReplishment.GetExistingStoreList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlexiststore.DataSource = dsStore;
                ddlexiststore.DataTextField = "StoreName";
                ddlexiststore.DataValueField = "StoreID";
                ddlexiststore.DataBind();
            }
            else
            {
                ddlexiststore.DataSource = null;
                ddlexiststore.DataBind();
            }
            ddlexiststore.Items.Insert(0, new ListItem("Select Existing Feed Template", "0"));
        }

        protected void mapping_btn_next_Click(object sender, EventArgs e)
        {
            string strallhtml = "";
            Int32 StoreIdall = 0;
            if (ddlstore.SelectedIndex > 0)
            {
                ltrstorename.Text = ddlstore.SelectedItem.Text.ToString();

                StoreIdall = Convert.ToInt32(ddlstore.SelectedValue.ToString());

            }
            else if (txtstorename.Text.ToString().Trim() != "")
            {
                ltrstorename.Text = txtstorename.Text.ToString().Trim();
                Int32 storeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT RepStoreID FROM tb_Replenishment_Store WHERE Storename ='" + txtstorename.Text.ToString().Replace("'", "''") + "' and isnull(deleted,0)=0 "));
                if (storeId > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "alreadyexists", "alert('Channel partner name already exists!!!'); $('#setup-probox').fadeIn('slow');", true);
                    return;
                }

            }
            else if (ddlexiststore.SelectedIndex > 0)
            {
                StoreIdall = Convert.ToInt32(ddlexiststore.SelectedValue.ToString());
                ltrstorename.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Storename FROM tb_Replenishment_Store WHERE RepStoreID ='" + ddlexiststore.SelectedValue.ToString().Replace("'", "''") + "' and isnull(deleted,0)=0 "));


            }
            ltrstorename1.Text = ltrstorename.Text;
            ltrstorename2.Text = ltrstorename.Text;
            if (StoreIdall > 0)
            {

                DataSet dsData = new DataSet();
                ReplenishmentFeedComponent objReplishment = new ReplenishmentFeedComponent();
                dsData = objReplishment.GetFieldDetails(StoreIdall);
                if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                {
                    DataSet dspColumn = new DataSet();

                    ReplenishmentFeedComponent objrep = new ReplenishmentFeedComponent();
                    dspColumn = objrep.GetMappingColumnName();
                    for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                    {


                        strallhtml += "<div class=\"panel fa-border p-top-20\" id=\"div_" + i.ToString() + "\"><input type=\"hidden\" id=\"hdn_" + i.ToString() + "\" name=\"hdn_" + i.ToString() + "\" value=\"" + dsData.Tables[0].Rows[i]["ReplenishmentFieldID"].ToString() + "\" /> ";
                        strallhtml += "<div class=\"form-group\">";
                        strallhtml += "<label class=\"control-label col-md-3\">Column Name</label>";
                        strallhtml += "<div class=\"col-md-3\">";
                        strallhtml += "<input type=\"text\" class=\"form-control\" value=\"" + dsData.Tables[0].Rows[i]["FieldName"].ToString() + "\" name=\"column_name_" + i.ToString() + "\" id=\"column_name_" + i.ToString() + "\" >";
                        strallhtml += "</div>";

                        strallhtml += "<div class=\"col-md-3\">";
                        strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnremove_" + i.ToString() + "\"  onclick=\"removecolumn(" + i.ToString() + ");\">Remove Column</button>";
                        strallhtml += "</div>";

                        strallhtml += "</div>";
                        strallhtml += "<div class=\"form-group\">";
                        strallhtml += "<label class=\"control-label col-md-3\">Attributes</label>";
                        strallhtml += "<div class=\"col-md-6 icheck minimal\">";
                        strallhtml += "<div class=\"row\">";
                        strallhtml += "<label class=\"checkbox-inline\">";
                        strallhtml += "<div class=\"row\">";
                        strallhtml += "<div class=\"radio single-row\">";

                        if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["IsRequired"].ToString()) && Convert.ToBoolean(dsData.Tables[0].Rows[i]["IsRequired"].ToString()) == true)
                        {
                            strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\" id=\"demo-radio_1_" + i.ToString() + "\" checked=\"CHECKED\" value=\"Yes\" />";
                        }
                        else
                        {
                            strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\" id=\"demo-radio_1_" + i.ToString() + "\" value=\"Yes\" />";
                        }


                        strallhtml += "<label>Required</label>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "</label>";
                        strallhtml += "<label class=\"checkbox-inline\">";
                        strallhtml += "<div class=\"row\">";
                        strallhtml += "<div class=\"radio single-row\">";
                        if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["IsRequired"].ToString()) && Convert.ToBoolean(dsData.Tables[0].Rows[i]["IsRequired"].ToString()) == false)
                        {
                            strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\"  id=\"demo-radio_2_" + i.ToString() + "\" checked=\"CHECKED\" value=\"No\" />";
                        }
                        else
                        {
                            strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\"  id=\"demo-radio_2_" + i.ToString() + "\" value=\"No\" />";
                        }
                        strallhtml += "<label>Optional</label>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "</label>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "<div class=\"form-group\">";
                        strallhtml += "<label class=\"control-label col-md-3\">Column Data</label>";
                        strallhtml += "<div class=\"col-md-7\">";
                        strallhtml += "<div class=\"form-control-static\">";
                        strallhtml += "<div class=\"row\">";
                        strallhtml += "<div class=\"col-sm-7\">";
                        strallhtml += "<div class=\"col-xs-8\">";
                        strallhtml += "<div class=\"row\">";
                        strallhtml += "<select class=\"form-control\" name=\"select_" + i.ToString() + "\" id=\"select_" + i.ToString() + "\">";

                        strallhtml += "<option value=\"\">Select Column Data</option>";
                        //if (dspColumn != null && dspColumn.Tables.Count > 0 && dspColumn.Tables[0].Rows.Count > 0)
                        //{
                        //    for (int k = 0; k < dspColumn.Tables[0].Rows.Count; k++)
                        //    {
                        //        if (dsData.Tables[0].Rows[i]["MappingColumn"].ToString().ToLower().Trim() == dspColumn.Tables[0].Rows[k]["columndata"].ToString().ToLower().Trim())
                        //        {
                        //            strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\" selected=\"true\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                        //        }
                        //        else
                        //        {
                        //         
                        //           // strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                        //        }

                        //    }

                        //}



                        strallhtml += "<option ###SKU###>SKU</option>";
                        strallhtml += "<option  ###Channel Partner SKU###>Channel Partner SKU</option>";
                        strallhtml += "<option ###GS1 ID###>GS1 ID</option>";
                        strallhtml += "<option ###Product Name/Description###>Product Name/Description</option>";
                        strallhtml += "<option ###Price###>Price</option>";
                        strallhtml += "<option ###Quantity Available for Channel Partner###>Quantity Available for Channel Partner</option>";
                        strallhtml += "<option ###Quantity On Order1###>Quantity On Order1</option>";
                        strallhtml += "<option ###Item Availability Date1###>Item Availability Date1</option>";
                        strallhtml += "<option ###Quantity On Order2###>Quantity On Order2</option>";
                        strallhtml += "<option ###Item Availability Date2###>Item Availability Date2</option>";
                        strallhtml += "<option ###Quantity On Order3###>Quantity On Order3</option>";
                        strallhtml += "<option ###Item Availability Date3###>Item Availability Date3</option>";
                        strallhtml += "<option ###Quantity On Order4###>Quantity On Order4</option>";
                        strallhtml += "<option ###Item Availability Date4###>Item Availability Date4</option>";
                        strallhtml += "<option ###Discontinued Status###>Discontinued Status</option>";
                        strallhtml += "<option ###Active or Inactive Status###>Active or Inactive Status</option>";
                        strallhtml += "<option ###OptionSku###>OptionSku</option>";

                        if (!String.IsNullOrEmpty(dsData.Tables[0].Rows[i]["MappingColumn"].ToString()))
                        {
                            strallhtml = strallhtml.Replace("###" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "###", "selected=\"true\"");
                            strallhtml = strallhtml.Replace("###SKU###", "").Replace("###SKU###", "").Replace("###Channel Partner SKU###", "").Replace("###GS1 ID###", "").Replace("###Product Name/Description###", "").Replace("###Price###", "").Replace("###Quantity Available for Channel Partner###", "").Replace("###Quantity On Order1###", "").Replace("###Item Availability Date1###", "").Replace("###Quantity On Order2###", "").Replace("###Item Availability Date2###", "").Replace("###Quantity On Order3###", "").Replace("###Item Availability Date3###", "").Replace("###Quantity On Order4###", "").Replace("###Item Availability Date4###", "").Replace("###Discontinued Status###", "").Replace("###Active or Inactive Status###", "").Replace("###OptionSku###", "");



                            //strallhtml += "<option value=\"" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "\" selected=\"true\">" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "</option>";

                        }
                        else
                        {
                            strallhtml = strallhtml.Replace("###SKU###", "").Replace("###SKU###", "").Replace("###Channel Partner SKU###", "").Replace("###GS1 ID###", "").Replace("###Product Name/Description###", "").Replace("###Price###", "").Replace("###Quantity Available for Channel Partner###", "").Replace("###Quantity On Order1###", "").Replace("###Item Availability Date1###", "").Replace("###Quantity On Order2###", "").Replace("###Item Availability Date2###", "").Replace("###Quantity On Order3###", "").Replace("###Item Availability Date3###", "").Replace("###Quantity On Order4###", "").Replace("###Item Availability Date4###", "").Replace("###Discontinued Status###", "").Replace("###Active or Inactive Status###", "").Replace("###OptionSku###", "");
                        }
                        //string[] arr1 = new string[] { "SKU", "Channel Partner SKU", "GS1 ID", "Product Name/Description", "Price", "Quantity Available for Channel Partner", "Quantity On Order", "Item Availability Date", "Discontinued Status", "Active or Inactive Status" };

                        //for (int k = 0; k < arr1.Length; k++)
                        //{
                        //    if (dsData.Tables[0].Rows[i]["MappingColumn"].ToString().ToLower().Trim() == arr1[k].ToString())
                        //    {
                        //        strallhtml += "<option value=\"" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "\" selected=\"true\">" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "</option>";
                        //    }
                        //    else
                        //    {
                        //        strallhtml += "<option value=\"" + arr1[k].ToString() + "\" >" + arr1[k].ToString() + "</option>";

                        //    }
                        //}


                        //strallhtml += "<option>SKU</option>";
                        //strallhtml += "<option>Channel Partner SKU</option>";
                        //strallhtml += "<option>GS1 ID</option>";
                        //strallhtml += "<option>Product Name/Description</option>";
                        //strallhtml += "<option>Price</option>";
                        //strallhtml += "<option>Quantity Available for Channel Partner</option>";
                        //strallhtml += "<option>Quantity On Order</option>";
                        //strallhtml += "<option>Item Availability Date</option>";
                        //strallhtml += "<option>Discontinued Status</option>";
                        //strallhtml += "<option>Active or Inactive Status</option>";
                        strallhtml += "</select>";
                        strallhtml += "</div>";
                        strallhtml += " </div>";
                        strallhtml += "<div class=\"col-xs-4\" style=\"display:none;\">";
                        strallhtml += "<a class=\"fancybox fancybox.iframe\" onclick=\"windowoprnincenter('Configurepopup.aspx?new=" + dsData.Tables[0].Rows[i]["ReplenishmentFieldID"].ToString() + "&id=" + dsData.Tables[0].Rows[i]["ReplenishmentFieldID"].ToString() + "','600','350');\" href=\"javascript:void(0);\"> <button class=\"btn btn-orang\" type=\"button\">Configure</button></a>";
                        strallhtml += "</div>";
                        strallhtml += "<span class=\"help-block\" style=\"width:100% !important;float:left;\"><em>Dynamic Value</em></span> </div>";
                        strallhtml += "<div class=\"col-sm-5\">";

                        strallhtml += "<input type=\"text\" class=\"form-control\" value=\"" + dsData.Tables[0].Rows[i]["StaticValue"].ToString() + "\" name=\"txtstatic_" + i.ToString() + "\" id=\"txtstatic_" + i.ToString() + "\" onchange=\"changecolumndata('txtstatic_" + i.ToString() + "');\" onkeyup=\"changecolumndata('txtstatic_" + i.ToString() + "');\">";
                        strallhtml += "<span class=\"help-block\"><em>Static Value</em></span></div>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        strallhtml += "</div>";
                        DataSet dsconfigure = new DataSet();
                        dsconfigure = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID=" + dsData.Tables[0].Rows[i]["ReplenishmentFieldID"].ToString() + "");

                        if (dsconfigure != null && dsconfigure.Tables.Count > 0 && dsconfigure.Tables[0].Rows.Count > 0)
                        {
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">&nbsp;</label><div class=\"col-md-3\"><div class=\"radio single-row\"><input type=\"checkbox\" checked onchange=\"configuredatacolumn(" + i.ToString() + ");\" id=\"chkcolumn_" + i.ToString() + "\" name=\"chkcolumn_" + i.ToString() + "\" value=\"Yes\"> <label>Configure Data</label></div></div> </div>";
                            strallhtml += "<div id=\"divconfigure_" + i.ToString() + "\">";
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">DB Value</label><div class=\"col-md-3\" style=\"margin-top:5px;\">New Value</div> </div>";
                            for (int icall = 0; icall < dsconfigure.Tables[0].Rows.Count; icall++)
                            {
                                if (!string.IsNullOrEmpty(dsconfigure.Tables[0].Rows[icall]["Dbvalue"].ToString()))
                                {
                                    if (Convert.ToBoolean(dsconfigure.Tables[0].Rows[icall]["Dbvalue"].ToString()))
                                    {
                                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">True</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueyes_" + i.ToString() + "\" name=\"txtdbvalueyes_" + i.ToString() + "\" value=\"" + dsconfigure.Tables[0].Rows[icall]["Assignedvalue"].ToString() + "\" class=\"form-control\"></div> </div>";

                                        //txtyes.Text = dsconfigure.Tables[0].Rows[i]["Assignedvalue"].ToString();
                                    }
                                    else
                                    {
                                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">False</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueno_" + i.ToString() + "\" name=\"txtdbvalueno_" + i.ToString() + "\" value=\"" + dsconfigure.Tables[0].Rows[icall]["Assignedvalue"].ToString() + "\" class=\"form-control\"></div> </div>";
                                        //txtno.Text = dsconfigure.Tables[0].Rows[i]["Assignedvalue"].ToString();
                                    }

                                }
                            }
                            strallhtml += "</div>";
                        }
                        else
                        {
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">&nbsp;</label><div class=\"col-md-3\"><div class=\"radio single-row\"><input type=\"checkbox\" onchange=\"configuredatacolumn(" + i.ToString() + ");\" id=\"chkcolumn_" + i.ToString() + "\" name=\"chkcolumn_" + i.ToString() + "\" value=\"No\"> <label>Configure Data</label></div></div> </div>";
                            strallhtml += "<div id=\"divconfigure_" + i.ToString() + "\" style=\"display:none;\">";
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">DB Value</label><div class=\"col-md-3\" style=\"margin-top:5px;\">New Value</div> </div>";
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">True</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueyes_" + i.ToString() + "\" name=\"txtdbvalueyes_" + i.ToString() + "\" value=\"\" class=\"form-control\"></div> </div>";
                            strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">False</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueno_" + i.ToString() + "\" name=\"txtdbvalueno_" + i.ToString() + "\" value=\"\" class=\"form-control\"></div> </div>";
                            strallhtml += "</div>";
                        }
                        strallhtml += "</div>";




                        strallhtml += "<div class=\"form-group\" id=\"divgroup_" + i.ToString() + "\">";
                        strallhtml += "<label class=\"control-label col-md-3\">&nbsp;</label>";
                        strallhtml += "<div class=\"col-md-9\">";
                        //if (i == dsData.Tables[0].Rows.Count - 1)
                        //{
                        //    strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnmore_" + i.ToString() + "\" onclick=\"addmorecolumn(" + dsData.Tables[0].Rows.Count.ToString() + ");\">Add More Columns</button>";
                        //}
                        //else
                        {
                            strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" style=\"display:none;\" id=\"btnmore_" + i.ToString() + "\">Add More Columns</button>";
                        }



                        strallhtml += "</div>";
                        strallhtml += "</div>";
                    }
                    Int32 tt = Convert.ToInt32(dsData.Tables[0].Rows.Count.ToString());
                    tt = tt + 1;
                    ltrgroupdata.Text = strallhtml + divcolumndata.InnerHtml.Replace("##kau###", dsData.Tables[0].Rows.Count.ToString()).Replace("##kau_new###", tt.ToString());
                }


            }


            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide();$('#mapping-probox').fadeIn('slow'); $('#sequence-probox').hide();$('#generate-probox').hide();$('#setup-wizard').removeClass();$('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass();$('#mapping-wizard').addClass('current active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('active-trail');$('#mapping-wizard').parent().addClass('active');$('#setup-wizard').parent().removeClass('active');", true);




        }
        protected void generate_btn_next_Click(object sender, EventArgs e)
        {
            string[] formkeys = Request.Form.AllKeys;
            Int32 diss = 1;
            DataTable dtnew = new DataTable();
            if (Session["datacolumn"] != null)
            {
                dtnew = (DataTable)Session["datacolumn"];
            }
            foreach (String s in formkeys)
            {
                if (s.ToLower().IndexOf("hdnname_") > -1)
                {
                    string strname = Request.Form[s].ToString();

                    if (dtnew != null && dtnew.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtnew.Rows.Count; i++)
                        {
                            if (dtnew.Rows[i]["ColumnName"].ToString().ToLower().Trim() == strname.ToString().ToLower().Trim())
                            {
                                dtnew.Rows[i]["Displayorder"] = diss;
                                dtnew.AcceptChanges();
                                diss += 1;
                            }
                        }
                    }


                }
            }
            Session["datacolumn"] = dtnew;




            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide();$('#mapping-probox').hide();$('#sequence-probox').hide();$('#generate-probox').fadeIn('slow');$('#setup-wizard').removeClass();$('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass();$('#mapping-wizard').addClass('currentpro active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('currentpro active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('current active-trail');$('#generate-wizard').parent().addClass('active');$('#sequence-wizard').parent().removeClass('active');", true);

        }

        protected void sequence_btn_next_Click(object sender, EventArgs e)
        {
            string[] formkeys = Request.Form.AllKeys;
            DataTable dt = new DataTable();
            DataColumn col1 = new DataColumn("Id", typeof(int));
            dt.Columns.Add(col1);
            DataColumn col2 = new DataColumn("ColumnName", typeof(string));
            dt.Columns.Add(col2);

            DataColumn col3 = new DataColumn("isRequired", typeof(bool));
            dt.Columns.Add(col3);
            DataColumn col4 = new DataColumn("Staticvalue", typeof(string));
            dt.Columns.Add(col4);

            DataColumn col5 = new DataColumn("Storeid", typeof(int));
            dt.Columns.Add(col5);
            DataColumn col6 = new DataColumn("Displayorder", typeof(int));
            dt.Columns.Add(col6);

            DataColumn col7 = new DataColumn("MappingColumn", typeof(string));
            dt.Columns.Add(col7);

            DataColumn col8 = new DataColumn("columnId", typeof(int));
            dt.Columns.Add(col8);

            DataColumn col14 = new DataColumn("isconfigure", typeof(int));
            dt.Columns.Add(col14);

            DataColumn col10 = new DataColumn("Dbvalueyes", typeof(bool));
            dt.Columns.Add(col10);
            DataColumn col11 = new DataColumn("Assignedvalueyes", typeof(string));
            dt.Columns.Add(col11);

            DataColumn col12 = new DataColumn("Dbvalueno", typeof(bool));
            dt.Columns.Add(col12);
            DataColumn col13 = new DataColumn("Assignedvalueno", typeof(string));
            dt.Columns.Add(col13);


            Int32 cl = 0;
            Int32 sv = 0;
            Int32 ir = 0;
            Int32 sid = 0;
            Int32 hid = 0;
            Int32 mpid = 0;
            DataRow dr = null;
            Int32 displayOrder = 1;
            foreach (String s in formkeys)
            {

                if (cl == 0 && sv == 0 && ir == 0 && hid == 0)
                {
                    dr = dt.NewRow();
                    dr["Storeid"] = 0;
                    displayOrder = 2;
                }
                if (s.ToLower().IndexOf("###kau###") <= -1)
                {

                    if (s.ToLower().IndexOf("column_name_") > -1)
                    {
                        dr["ColumnName"] = Request.Form[s].ToString();
                        Int32 Icount = 0;
                        Int32.TryParse(s.ToLower().Replace("column_name_", ""), out Icount);
                        dr["columnId"] = Icount;
                        cl = 1;
                        if (Request.Form[s.ToLower().Replace("column_name_", "chkcolumn_")] != null)
                        {
                            if (Request.Form[s.ToLower().Replace("column_name_", "chkcolumn_").ToString()].ToString() == "Yes")
                            {
                                dr["Dbvalueyes"] = 1;
                                dr["Dbvalueno"] = 0;
                                dr["isconfigure"] = 1;
                                dr["Assignedvalueyes"] = Request.Form[s.ToLower().Replace("column_name_", "txtdbvalueyes_")].ToString();
                                dr["Assignedvalueno"] = Request.Form[s.ToLower().Replace("column_name_", "txtdbvalueno_")].ToString();
                            }
                            else
                            {
                                dr["Assignedvalueyes"] = "";
                                dr["Assignedvalueno"] = "";
                                dr["Dbvalueyes"] = 1;
                                dr["Dbvalueno"] = 0;
                                dr["isconfigure"] = 0;
                            }
                        }
                    }
                    else if (s.ToLower().IndexOf("txtstatic_") > -1)
                    {
                        dr["Staticvalue"] = Request.Form[s].ToString();
                        sv = 1;


                    }
                    else if (s.ToLower().IndexOf("demo-radio_") > -1)
                    {
                        if (Request.Form[s].ToString() == "Yes")
                        {
                            dr["isRequired"] = Convert.ToBoolean(true);
                        }
                        else
                        {
                            dr["isRequired"] = Convert.ToBoolean(false);
                        }
                        ir = 1;
                    }

                    //else if (s.ToLower().IndexOf("demo-radio_2") > -1)
                    //{
                    //    if (Request.Form[s].ToString() == "Yes")
                    //    {
                    //        dr["isRequired"] = Convert.ToBoolean(false);
                    //    }
                    //    else
                    //    {
                    //        dr["isRequired"] = Convert.ToBoolean(false);
                    //    }
                    //    ir = 1;
                    //}
                    else if (s.ToLower().IndexOf("hdn_") > -1)
                    {
                        dr["Id"] = Request.Form[s].ToString();
                        hid = 1;
                    }
                    else if (s.ToLower().IndexOf("select_") > -1)
                    {
                        if (Request.Form[s].ToString().ToLower() == "select column data")
                        {
                            dr["MappingColumn"] = "";
                        }
                        else
                        {
                            dr["MappingColumn"] = Request.Form[s].ToString();
                        }

                        mpid = 1;
                    }
                    if (cl == 1 && sv == 1 && ir == 1 && hid == 1 && mpid == 1)
                    {
                        cl = 0;
                        sv = 0;
                        ir = 0;
                        hid = 0;
                        mpid = 0;
                        dr["Displayorder"] = displayOrder.ToString();
                        dt.Rows.Add(dr);
                    }

                    dt.AcceptChanges();
                }

            }
            ltcolumn.Text = "";
            string strscript = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                Session["datacolumn"] = dt;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ltcolumn.Text += "<li  class=\"dd-item\" id=\"li_" + i.ToString() + "\"><div class=\"dd-handle\" ><input type=\"hidden\" id=\"hdnname_" + i.ToString() + "\" name=\"hdnname_" + i.ToString() + "\" Value=\"" + dt.Rows[i]["ColumnName"].ToString() + "\">" + dt.Rows[i]["ColumnName"].ToString() + "</div></li>";
                    strscript = "$(\"li\").arrangeable({dragSelector: '.dd-handle'});";
                }
            }
            if (ddlstore.SelectedIndex > 0)
            {
                txtfilename.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId =" + ddlstore.SelectedValue.ToString() + " "));
            }

            else if (ddlexiststore.SelectedIndex > 0)
            {
                txtfilename.Text = Convert.ToString(ddlexiststore.SelectedItem.Text.ToString());
            }
            if (!string.IsNullOrEmpty(txtfilename.Text) && txtfilename.Text.ToString().IndexOf(".") > -1)
            {
                string path = System.IO.Path.GetExtension(txtfilename.Text.ToString());
                if (path.ToString().ToLower().IndexOf(".csv") > -1)
                {
                    rdocsv.Checked = true;
                }
                else
                {
                    rdoexcel.Checked = true;
                }
                txtfilename.Text = txtfilename.Text.ToString().Replace(path, "");
            }
            btnsave.Visible = true;


            ////////Start Rebind///////////////
            string strallhtml = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataSet dspColumn = new DataSet();
                ReplenishmentFeedComponent objrep = new ReplenishmentFeedComponent();
                dspColumn = objrep.GetMappingColumnName();
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    strallhtml += "<div class=\"panel fa-border p-top-20\" id=\"div_" + i.ToString() + "\"><input type=\"hidden\" id=\"hdn_" + i.ToString() + "\" name=\"hdn_" + i.ToString() + "\" value=\"" + dt.Rows[i]["id"].ToString() + "\" /> ";
                    strallhtml += "<div class=\"form-group\">";
                    strallhtml += "<label class=\"control-label col-md-3\">Column Name</label>";
                    strallhtml += "<div class=\"col-md-3\">";
                    strallhtml += "<input type=\"text\" class=\"form-control\" value=\"" + dt.Rows[i]["ColumnName"].ToString() + "\" name=\"column_name_" + i.ToString() + "\" id=\"column_name_" + i.ToString() + "\" >";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "<div class=\"form-group\">";
                    strallhtml += "<label class=\"control-label col-md-3\">Attributes</label>";
                    strallhtml += "<div class=\"col-md-6 icheck minimal\">";
                    strallhtml += "<div class=\"row\">";
                    strallhtml += "<label class=\"checkbox-inline\">";
                    strallhtml += "<div class=\"row\">";
                    strallhtml += "<div class=\"radio single-row\">";

                    if (!string.IsNullOrEmpty(dt.Rows[i]["IsRequired"].ToString()) && Convert.ToBoolean(dt.Rows[i]["IsRequired"].ToString()) == true)
                    {
                        strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\" id=\"demo-radio_1_" + i.ToString() + "\" checked=\"CHECKED\" value=\"Yes\" />";
                    }
                    else
                    {
                        strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\" id=\"demo-radio_1_" + i.ToString() + "\" value=\"Yes\" />";
                    }


                    strallhtml += "<label>Required</label>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "</label>";
                    strallhtml += "<label class=\"checkbox-inline\">";
                    strallhtml += "<div class=\"row\">";
                    strallhtml += "<div class=\"radio single-row\">";
                    if (!string.IsNullOrEmpty(dt.Rows[i]["IsRequired"].ToString()) && Convert.ToBoolean(dt.Rows[i]["IsRequired"].ToString()) == false)
                    {
                        strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\"  id=\"demo-radio_2_" + i.ToString() + "\" checked=\"CHECKED\" value=\"No\" />";
                    }
                    else
                    {
                        strallhtml += "<input tabindex=\"3\" type=\"radio\"  name=\"demo-radio_" + i.ToString() + "\"  id=\"demo-radio_2_" + i.ToString() + "\" value=\"No\" />";
                    }
                    strallhtml += "<label>Optional</label>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "</label>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "<div class=\"form-group\">";
                    strallhtml += "<label class=\"control-label col-md-3\">Column Data</label>";
                    strallhtml += "<div class=\"col-md-7\">";
                    strallhtml += "<div class=\"form-control-static\">";
                    strallhtml += "<div class=\"row\">";
                    strallhtml += "<div class=\"col-sm-7\">";
                    strallhtml += "<div class=\"col-xs-8\">";
                    strallhtml += "<div class=\"row\">";
                    strallhtml += "<select class=\"form-control\" name=\"select_" + i.ToString() + "\" id=\"select_" + i.ToString() + "\">";
                    strallhtml += "<option value=\"\">Select Column Data</option>";
                    //if (dspColumn != null && dspColumn.Tables.Count > 0 && dspColumn.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dspColumn.Tables[0].Rows.Count; k++)
                    //    {
                    //        if (dt.Rows[i]["MappingColumn"].ToString().ToLower().Trim() == dspColumn.Tables[0].Rows[k]["columndata"].ToString().ToLower().Trim())
                    //        {
                    //            strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\" selected=\"true\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                    //        }
                    //        else
                    //        {
                    //            strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                    //        }

                    //    }

                    //}
                    //if (dspColumn != null && dspColumn.Tables.Count > 0 && dspColumn.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dspColumn.Tables[0].Rows.Count; k++)
                    //    {
                    //        if (dt.Rows[i]["MappingColumn"].ToString().ToLower().Trim() == dspColumn.Tables[0].Rows[k]["columndata"].ToString().ToLower().Trim())
                    //        {
                    //            strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\" selected=\"true\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                    //        }
                    //        else
                    //        {
                    //            strallhtml += "<option value=\"" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "\">" + dspColumn.Tables[0].Rows[k]["columndata"].ToString() + "</option>";
                    //        }

                    //    }

                    //}
                    strallhtml += "<option ###SKU###>SKU</option>";
                    strallhtml += "<option  ###Channel Partner SKU###>Channel Partner SKU</option>";
                    strallhtml += "<option ###GS1 ID###>GS1 ID</option>";
                    strallhtml += "<option ###Product Name/Description###>Product Name/Description</option>";
                    strallhtml += "<option ###Price###>Price</option>";
                    strallhtml += "<option ###Quantity Available for Channel Partner###>Quantity Available for Channel Partner</option>";
                    strallhtml += "<option ###Quantity On Order1###>Quantity On Order1</option>";
                    strallhtml += "<option ###Item Availability Date1###>Item Availability Date1</option>";
                    strallhtml += "<option ###Quantity On Order2###>Quantity On Order2</option>";
                    strallhtml += "<option ###Item Availability Date2###>Item Availability Date2</option>";
                    strallhtml += "<option ###Quantity On Order3###>Quantity On Order3</option>";
                    strallhtml += "<option ###Item Availability Date3###>Item Availability Date3</option>";
                    strallhtml += "<option ###Quantity On Order4###>Quantity On Order4</option>";
                    strallhtml += "<option ###Item Availability Date4###>Item Availability Date4</option>";
                    strallhtml += "<option ###Discontinued Status###>Discontinued Status</option>";
                    strallhtml += "<option ###Active or Inactive Status###>Active or Inactive Status</option>";
                    strallhtml += "<option ###OptionSku###>OptionSku</option>";

                    if (!String.IsNullOrEmpty(dt.Rows[i]["MappingColumn"].ToString()))
                    {
                        strallhtml = strallhtml.Replace("###" + dt.Rows[i]["MappingColumn"].ToString() + "###", "selected=\"true\"");
                        strallhtml = strallhtml.Replace("###SKU###", "").Replace("###SKU###", "").Replace("###Channel Partner SKU###", "").Replace("###GS1 ID###", "").Replace("###Product Name/Description###", "").Replace("###Price###", "").Replace("###Quantity Available for Channel Partner###", "").Replace("###Quantity On Order1###", "").Replace("###Item Availability Date1###", "").Replace("###Quantity On Order2###", "").Replace("###Item Availability Date2###", "").Replace("###Quantity On Order3###", "").Replace("###Item Availability Date3###", "").Replace("###Quantity On Order4###", "").Replace("###Item Availability Date4###", "").Replace("###Discontinued Status###", "").Replace("###Active or Inactive Status###", "").Replace("###OptionSku###", "");



                        //strallhtml += "<option value=\"" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "\" selected=\"true\">" + dsData.Tables[0].Rows[i]["MappingColumn"].ToString() + "</option>";

                    }
                    else
                    {
                        strallhtml = strallhtml.Replace("###SKU###", "").Replace("###SKU###", "").Replace("###Channel Partner SKU###", "").Replace("###GS1 ID###", "").Replace("###Product Name/Description###", "").Replace("###Price###", "").Replace("###Quantity Available for Channel Partner###", "").Replace("###Quantity On Order1###", "").Replace("###Item Availability Date1###", "").Replace("###Quantity On Order2###", "").Replace("###Item Availability Date2###", "").Replace("###Quantity On Order3###", "").Replace("###Item Availability Date3###", "").Replace("###Quantity On Order4###", "").Replace("###Item Availability Date4###", "").Replace("###Discontinued Status###", "").Replace("###Active or Inactive Status###", "").Replace("###OptionSku###", "");
                    }

                    strallhtml += "</select>";
                    strallhtml += "</div>";
                    strallhtml += " </div>";
                    strallhtml += "<div class=\"col-xs-4\" style=\"display:none;\">";
                    strallhtml += "<a class=\"fancybox fancybox.iframe\" onclick=\"windowoprnincenter('Configurepopup.aspx?new=" + dt.Rows[i]["Id"].ToString() + "&id=" + dt.Rows[i]["Id"].ToString() + "','600','350');\" href=\"javascript:void(0);\" > <button class=\"btn btn-orang\" type=\"button\">Configure</button></a>";
                    strallhtml += "</div>";
                    strallhtml += "<span class=\"help-block\" style=\"width:100% !important;float:left;\"><em>Dynamic Value</em></span> </div>";
                    strallhtml += "<div class=\"col-sm-5\">";

                    strallhtml += "<input type=\"text\" class=\"form-control\" value=\"" + dt.Rows[i]["StaticValue"].ToString() + "\" name=\"txtstatic_" + i.ToString() + "\" id=\"txtstatic_" + i.ToString() + "\" onchange=\"changecolumndata('txtstatic_" + i.ToString() + "');\" onkeyup=\"changecolumndata('txtstatic_" + i.ToString() + "');\">";
                    strallhtml += "<span class=\"help-block\"><em>Static Value</em></span></div>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                    if (!string.IsNullOrEmpty(dt.Rows[i]["isconfigure"].ToString()) && dt.Rows[i]["isconfigure"].ToString() == "1")
                    {
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">&nbsp;</label><div class=\"col-md-3\"><div class=\"radio single-row\"><input type=\"checkbox\" checked onchange=\"configuredatacolumn(" + i.ToString() + ");\" id=\"chkcolumn_" + i.ToString() + "\" name=\"chkcolumn_" + i.ToString() + "\" value=\"Yes\"> <label>Configure Data</label></div></div> </div>";
                        strallhtml += "<div id=\"divconfigure_" + i.ToString() + "\">";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">DB Value</label><div class=\"col-md-3\" style=\"margin-top:5px;\">New Value</div> </div>";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">True</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueyes_" + i.ToString() + "\" name=\"txtdbvalueyes_" + i.ToString() + "\" value=\"" + dt.Rows[i]["Dbvalueyes"].ToString() + "\" class=\"form-control\"></div> </div>";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">False</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueno_" + i.ToString() + "\" name=\"txtdbvalueno_" + i.ToString() + "\" value=\"" + dt.Rows[i]["Dbvalueno"].ToString() + "\" class=\"form-control\"></div> </div>";
                        strallhtml += "</div>";
                    }
                    else
                    {
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">&nbsp;</label><div class=\"col-md-3\"><div class=\"radio single-row\"><input type=\"checkbox\" onchange=\"configuredatacolumn(" + i.ToString() + ");\" id=\"chkcolumn_" + i.ToString() + "\" name=\"chkcolumn_" + i.ToString() + "\" value=\"No\"> <label>Configure Data</label></div></div> </div>";
                        strallhtml += "<div id=\"divconfigure_" + i.ToString() + "\" style=\"display:none;\">";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">DB Value</label><div class=\"col-md-3\" style=\"margin-top:5px;\">New Value</div> </div>";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">True</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueyes_" + i.ToString() + "\" name=\"txtdbvalueyes_" + i.ToString() + "\" value=\"\" class=\"form-control\"></div> </div>";
                        strallhtml += "<div class=\"form-group\"><label class=\"control-label col-md-3\">False</label><div class=\"col-md-3\"><input type=\"text\" id=\"txtdbvalueno_" + i.ToString() + "\" name=\"txtdbvalueno_" + i.ToString() + "\" value=\"\" class=\"form-control\"></div> </div>";
                        strallhtml += "</div>";
                    }
                    strallhtml += "</div>";






                    strallhtml += "<div class=\"form-group\" id=\"divgroup_" + i.ToString() + "\">";
                    strallhtml += "<label class=\"control-label col-md-3\">&nbsp;</label>";
                    strallhtml += "<div class=\"col-md-9\">";
                    if (i == dt.Rows.Count - 1)
                    {
                        strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnmore_" + i.ToString() + "\" onclick=\"addmorecolumn(" + dt.Rows.Count.ToString() + ");\">Add More Columns</button>";
                    }
                    else
                    {
                        strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" style=\"display:none;\" id=\"btnmore_" + i.ToString() + "\">Add More Columns</button>";
                    }

                    strallhtml += "&nbsp;&nbsp; &nbsp;&nbsp;";
                    if (i == dt.Rows.Count - 1)
                    {
                        strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnremove_" + i.ToString() + "\"  onclick=\"removecolumn(" + i.ToString() + ");\" style=\"display:none;\">Remove Column</button>";
                    }
                    else
                    {
                        strallhtml += "<button class=\"btn btn-orang m-bot15\" type=\"button\" id=\"btnremove_" + i.ToString() + "\"  onclick=\"removecolumn(" + i.ToString() + ");\">Remove Column</button>";
                    }
                    strallhtml += "</div>";
                    strallhtml += "</div>";
                }

                ltrgroupdata.Text = strallhtml;
            }

            ///////////END////////////

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide(); $('#mapping-probox').hide();$('#sequence-probox').fadeIn('slow');$('#generate-probox').hide();$('#setup-wizard').removeClass(); $('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass(); $('#mapping-wizard').addClass('currentpro active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('current active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('active-trail');$('#sequence-wizard').parent().addClass('active');$('#mapping-wizard').parent().removeClass('active');$(document).ready(function () {" + strscript + "});", true);
        }
        protected void btndone_Click(object sender, EventArgs e)
        {

            Response.Redirect("dashboard.aspx", true);

        }
        //private void GetFieldTemplate()
        //{
        //    GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
        //    DataSet DsTemplate = new DataSet();
        //    DsTemplate = objInv.GetChannelPartnerFeedTemplate(Convert.ToInt32(Session["Storeidnew"].ToString()));
        //    if (DsTemplate != null && DsTemplate.Tables.Count > 0 && DsTemplate.Tables[0].Rows.Count > 0)
        //    {
        //        String strqueryProduct = "";
        //        String StrVariant = "";


        //        DataSet dsProductcol = new DataSet();
        //        dsProductcol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_Product'");
        //        if (dsProductcol != null && dsProductcol.Tables.Count > 0 && dsProductcol.Tables[0].Rows.Count > 0)
        //        {

        //            strqueryProduct = "select ";
        //            String Strpara = "";
        //            for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //            {
        //                Int32 IsRequired = 0;
        //                Int32 IsStatic = 0;
        //                string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //                if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsRequired = 1;
        //                }
        //                else
        //                {
        //                    IsRequired = 0;
        //                }

        //                if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsStatic = 1;
        //                }
        //                else
        //                {
        //                    IsStatic = 0;
        //                }

        //                String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //                String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                {
        //                    // strallhtml += "<option>SKU</option>";
        //                    //strallhtml += "<option>Channel Partner SKU</option>";
        //                    //strallhtml += "<option>GS1 ID</option>";
        //                    //strallhtml += "<option>Product Name/Description</option>";
        //                    //strallhtml += "<option>Price</option>";
        //                    //strallhtml += "<option>Quantity Available for Channel Partner</option>";
        //                    //strallhtml += "<option>Quantity On Order</option>";
        //                    //strallhtml += "<option>Item Availability Date</option>";
        //                    //strallhtml += "<option>Discontinued Status</option>";
        //                    //strallhtml += "<option>Active or Inactive Status</option>";
        //                    if (MappingColumn == "SKU")
        //                    {
        //                        MappingColumn = "SKU";
        //                    }
        //                    else if (MappingColumn == "GS1 ID")
        //                    {
        //                        MappingColumn = "productid";
        //                    }
        //                    else if (MappingColumn == "Product Name/Description")
        //                    {
        //                        MappingColumn = "Name";
        //                    }
        //                    else if (MappingColumn == "Price")
        //                    {
        //                        MappingColumn = "Price";
        //                    }
        //                    else if (MappingColumn == "Quantity On Order")
        //                    {
        //                        MappingColumn = "Inventory";
        //                    }
        //                    else if (MappingColumn == "Discontinued Status")
        //                    {
        //                        MappingColumn = "IsDiscontinued";
        //                    }
        //                    else if (MappingColumn == "Active or Inactive Status")
        //                    {
        //                        MappingColumn = "Active";
        //                    }
        //                    else if (MappingColumn == "Item Availability Date")
        //                    {
        //                        MappingColumn = "CreatedOn";
        //                    }


        //                    DataRow[] drr = dsProductcol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                    if (drr.Length > 0)
        //                    {
        //                        Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                    }
        //                    else
        //                    {
        //                        MappingColumn = "";
        //                        // Strpara += ' ' + " as " + FieldName + ",";
        //                        Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                    }



        //                }
        //                else if (IsStatic == 1)
        //                {
        //                    Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //                }




        //            }

        //            Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //            strqueryProduct = strqueryProduct + Strpara + " from tb_product ";
        //            String Strwhr = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + Session["Storeidnew"].ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') and storeid=1";
        //            strqueryProduct = strqueryProduct + Strwhr;



        //        }



        //        DataSet dsVariantCol = new DataSet();
        //        dsVariantCol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_ProductVariantValue'");
        //        if (dsVariantCol != null && dsVariantCol.Tables.Count > 0 && dsVariantCol.Tables[0].Rows.Count > 0)
        //        {
        //            StrVariant = "select ";


        //            String Strpara = "";
        //            for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //            {
        //                Int32 IsRequired = 0;
        //                Int32 IsStatic = 0;
        //                string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //                if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsRequired = 1;
        //                }
        //                else
        //                {
        //                    IsRequired = 0;
        //                }

        //                if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsStatic = 1;
        //                }
        //                else
        //                {
        //                    IsStatic = 0;
        //                }

        //                String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //                String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                {
        //                    // strallhtml += "<option>SKU</option>";
        //                    //strallhtml += "<option>Channel Partner SKU</option>";
        //                    //strallhtml += "<option>GS1 ID</option>";
        //                    //strallhtml += "<option>Product Name/Description</option>";
        //                    //strallhtml += "<option>Price</option>";
        //                    //strallhtml += "<option>Quantity Available for Channel Partner</option>";
        //                    //strallhtml += "<option>Quantity On Order</option>";
        //                    //strallhtml += "<option>Item Availability Date</option>";
        //                    //strallhtml += "<option>Discontinued Status</option>";
        //                    //strallhtml += "<option>Active or Inactive Status</option>";
        //                    if (MappingColumn == "Channel Partner SKU")
        //                    {
        //                        MappingColumn = "SKU";
        //                    }
        //                    else if (MappingColumn == "GS1 ID")
        //                    {
        //                        MappingColumn = "productid";
        //                    }

        //                    else if (MappingColumn == "Discontinued Status")
        //                    {
        //                        MappingColumn = "IsDiscontinued";
        //                    }
        //                    else if (MappingColumn == "Price")
        //                    {
        //                        MappingColumn = "VariantPrice";
        //                    }
        //                    else if (MappingColumn == "Active or Inactive Status")
        //                    {
        //                        MappingColumn = "VarActive";
        //                    }
        //                    else if (MappingColumn == "Quantity Available for Channel Partner")
        //                    {
        //                        MappingColumn = "Inventory";
        //                    }
        //                    else if (MappingColumn == "Item Availability Date")
        //                    {
        //                        MappingColumn = "BackOrderdate";
        //                    }



        //                    DataRow[] drr = dsVariantCol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                    if (drr.Length > 0)
        //                    {
        //                        Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                    }
        //                    else
        //                    {
        //                        MappingColumn = "";
        //                        Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                    }



        //                }
        //                else if (IsStatic == 1)
        //                {
        //                    Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //                }




        //            }


        //            Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //            StrVariant = " Union all " + StrVariant + Strpara + " from tb_productvariantvalue ";
        //            string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + Session["Storeidnew"].ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
        //            StrVariant = StrVariant + Strwhr2;


        //            //StrVariant = StrVariant + " Union all ";
        //            //string child = "";
        //            //child = "select ";
        //            //child = child + Strpara + " from tb_productvariantvalue ";
        //            //string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
        //            //StrVariant = StrVariant + child + Strwhr2;

        //        }



        //        String strquery = "";
        //        strquery = strqueryProduct + StrVariant;
        //        DataSet dsEcommProducts = new DataSet();
        //        dsEcommProducts = CommonComponent.GetCommonDataSet(strquery);
        //        if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0 && dsEcommProducts.Tables[0].Rows.Count > 0)
        //        {
        //            if (rdocsv.Checked)
        //            {
        //                GenerateCSV(dsEcommProducts);
        //            }
        //            else if (rdoexcel.Checked)
        //            {
        //                GenerateExcel(dsEcommProducts);
        //            }



        //        }
        //        else
        //        {
        //            if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0)
        //            {
        //                if (rdocsv.Checked)
        //                {
        //                    GenerateCSV(dsEcommProducts);
        //                }
        //                else if (rdoexcel.Checked)
        //                {
        //                    GenerateExcel(dsEcommProducts);
        //                }
        //            }
        //        }


        //    }
        //    else
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "alert('Template Data not Found.');", true);
        //        return;
        //    }


        //}
        private void GetFieldTemplate()
        {
            int storeid = 0;
            storeid = Convert.ToInt32(Session["Storeidnew"].ToString());
            GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
            DataSet DsTemplate = new DataSet();
            DsTemplate = objInv.GetChannelPartnerFeedTemplate(Convert.ToInt32(Session["Storeidnew"].ToString()));
            if (DsTemplate != null && DsTemplate.Tables.Count > 0 && DsTemplate.Tables[0].Rows.Count > 0)
            {
                bool kohldis = false;
                bool atgdis = false;
                bool hozzdis = false;

                kohldis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsKohlDis' and storeid=1"));
                atgdis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsAtgDis' and storeid=1"));
                hozzdis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsHozzDis' and storeid=1"));
                String strqueryProduct = "";
                String StrVariant = "";

                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    storeid = 4;
                }



                //DataSet dsProductcol = new DataSet();
                //dsProductcol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_Product'");
                //if (dsProductcol != null && dsProductcol.Tables.Count > 0 && dsProductcol.Tables[0].Rows.Count > 0)
                //{

                strqueryProduct = "select ";
                String Strpara = "";
                for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
                {
                    Int32 IsRequired = 0;
                    Int32 IsStatic = 0;
                    string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
                    if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
                    {
                        IsRequired = 1;
                    }
                    else
                    {
                        IsRequired = 0;
                    }

                    if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
                    {
                        IsStatic = 1;
                    }
                    else
                    {
                        IsStatic = 0;
                    }

                    String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
                    String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

                    if (!String.IsNullOrEmpty(MappingColumn))
                    {

                        if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
                        {
                            MappingColumn = "SKU";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "sku")
                        {
                            MappingColumn = "SKU";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
                        {
                            //if (storeid == 12)
                            //{
                            //    MappingColumn = "''''+cast(UPC as nvarchar(max))";
                            //}
                            //else
                            //{
                                MappingColumn = "UPC";
                          //  }
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
                        {
                            MappingColumn = "(select top 1 isnull(q.name,'')  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "price")
                        {
                            //MappingColumn = "cast(Price as nvarchar(max))";
                            MappingColumn = "(select top 1 cast(q.Price as nvarchar(max))  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
                        {
                            MappingColumn = "(select top 1 case when cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
                        {
                            //if (storeid == 14 || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep" || storeid == 12 || storeid == 9)
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                          //  }
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max)) else '' end  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1) as nvarchar(max))  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                           // }
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1) as nvarchar(max))  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                           // }
                        }



                        else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
                        {
                            MappingColumn = "(dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + "))"; //"cast(Inventory as nvarchar(max))";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
                        {
                           // MappingColumn = "case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end";
                           // MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) ='0' then 'false' else 'true' end from tb_Product where StoreID=" +storeid.ToString() + " and sku=tb_Product.sku)";
                            if (storeid == 14 && atgdis == true) //atg
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")) >0 then 'false' else  'true' end end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }
                            else if (storeid == 13 && hozzdis == true) //Houzz
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")) >0 then 'false' else  'true' end end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }
                            else
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else 'true' end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
                        {
                           // MappingColumn = " case when isnull(inventory,0) >0 then 'Yes' else 'No' end ";
                            //MappingColumn = " case when isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            if (storeid == 12 && kohldis == true) //kohl
                            {
                                MappingColumn = "case when (select top 1  cast(isnull(q.Discontinue,0) as nvarchar(max)) from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)='0' then 'Yes' else case when (select top 1  cast(isnull(q.Discontinue,0) as nvarchar(max)) from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)='1' and isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end end ";
                            }
                            else
                            {
                                MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            }

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
                        {
                            MappingColumn = "Active";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
                        {
                            MappingColumn = "(select top 1 isnull(q.optionsku,'')  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }



                        ////DataRow[] drr = dsProductcol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
                        ////if (drr.Length > 0)
                        ////{
                        ////    Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
                        ////}
                        ////else
                        //{
                        //    MappingColumn = "";
                        //    // Strpara += ' ' + " as " + FieldName + ",";
                        //    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        //}
                        if (!String.IsNullOrEmpty(MappingColumn))
                            Strpara += MappingColumn + " as [" + FieldName + "],";
                        else
                        {
                            MappingColumn = "";
                            Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        }



                    }
                    else if (IsStatic == 1)
                    {
                        if (StaticValue.ToLower().Trim() == "blank")
                        {
                            StaticValue = "";
                        }
                        Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
                    }




                }

                Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
                strqueryProduct = strqueryProduct + Strpara + " from tb_product ";
                String Strwhr = "";
                if(ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    Strwhr = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsOverStockRep,0)=1 and isnull(IsDisplayOnFeed,0)=1) and storeid=1 and ProductID not in (select ProductID from tb_ProductVariantValue) and isnull(Deleted,0)<>1";

                }
                else
                {
                    Strwhr = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsDisplayOnFeed,0)=1) and storeid=1 and ProductID not in (select ProductID from tb_ProductVariantValue) and isnull(Deleted,0)<>1";
                }
                
                strqueryProduct = strqueryProduct + Strwhr;



                //  }



                //DataSet dsVariantCol = new DataSet();
                //dsVariantCol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_ProductVariantValue'");
                //if (dsVariantCol != null && dsVariantCol.Tables.Count > 0 && dsVariantCol.Tables[0].Rows.Count > 0)
                //{
                StrVariant = "select ";


                Strpara = "";
                for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
                {
                    Int32 IsRequired = 0;
                    Int32 IsStatic = 0;
                    string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
                    if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
                    {
                        IsRequired = 1;
                    }
                    else
                    {
                        IsRequired = 0;
                    }

                    if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
                    {
                        IsStatic = 1;
                    }
                    else
                    {
                        IsStatic = 0;
                    }

                    String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
                    String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

                    if (!String.IsNullOrEmpty(MappingColumn))
                    {


                        int count = 0;



                        if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
                        {
                            MappingColumn = "SKU";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
                        {
                            //if (storeid == 12)
                            //{
                            //    MappingColumn = "''''+cast(UPC as nvarchar(max))";
                            //}
                            //else
                            //{
                                MappingColumn = "UPC";
                            //}

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "sku")
                        {
                            MappingColumn = "SKU";

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
                        {

                           // MappingColumn = "(select tb_Product.Name from tb_product where ProductID=tb_productvariantvalue.ProductID)";
                            MappingColumn = "(select top 1 isnull(tb_Product.Name,'') from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku )";

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "price")
                        {
                           // MappingColumn = "cast(VariantPrice as nvarchar(max))";
                            MappingColumn = "(select top 1 cast(Price as nvarchar(max))  from tb_Product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
                        {
                            //MappingColumn = " case when isnull(inventory,0) > 0 then 'Yes' else 'No' end ";
                           // MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            if (storeid == 12 && kohldis == true)
                            {
                                //  MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else case when  (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='1' then 'Yes' else 'No' end end ";
                                MappingColumn = "case when (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='0' then 'Yes' else case when  (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='1' and isnull((dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end end ";


                            }
                            else
                            {
                                MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            }
                           

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
                        {
                            MappingColumn = "VarActive";

                        }


                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
                        {
                            MappingColumn = "(select top 1 case when cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
                        {
                            //if (storeid == 14 || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep" || storeid == 12 || storeid == 9)
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max)) else '' end  from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                           //}
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                           // }
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                           // }
                        }
                        else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
                        {
                            MappingColumn = "(dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + "))"; //"cast(Inventory as nvarchar(max))";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
                        {
                           
                           // MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            if (storeid == 14 && atgdis == true) //atg
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")) >0 then 'false' else 'true' end end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }
                            else if (storeid == 13 && hozzdis == true) //Houzz
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")) >0 then 'false' else 'true' end end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }

                            else
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
                        {
                            MappingColumn = "(select top 1 isnull(optionsku,'')  from tb_Product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                        }


                        if (!String.IsNullOrEmpty(MappingColumn))
                            Strpara += MappingColumn + " as [" + FieldName + "],";
                        else
                        {
                            MappingColumn = "";
                            Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        }



                    }
                    else if (IsStatic == 1)
                    {
                        if (StaticValue.ToLower().Trim() == "blank")
                        {
                            StaticValue = "";
                        }
                        Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
                    }




                }


                Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
                StrVariant = " Union all " + StrVariant + Strpara + " from tb_productvariantvalue ";
                string Strwhr2 = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    Strwhr2 = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsOverStockRep,0)=1 and isnull(IsDisplayOnFeed,0)=1)  and productid  in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) ";

                }
                else
                {
                    Strwhr2 = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsDisplayOnFeed,0)=1)  and productid  in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) ";

                }
                
                StrVariant = StrVariant + Strwhr2;

                string kohlquery = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    kohlquery = "select 'Always IN' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,No, or Guaranteed' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? 1' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all SELECT  [FILE TYPE], [VENDOR SKU],  [AVAILABLE],  cast([QTY] as nvarchar(100)) as [QTY],  cast([NEXT AVAILABLE QTY] as nvarchar(100)) as [NEXT AVAILABLE QTY] ,  [NEXT AVAILABLE DATE],  [MANUFACTURER],  [MANUFACTURER SKU],  [DESCRIPTION],  [UNIT COST],  [UNIT COST 2],  [UNIT COST 3], [UNIT COST 4], [DISCONTINUED], [MERCH. DEPT.],   [UNIT OF MEASURE],  [MERCHANT SKU],  [MERCHANT],  [GS1 ID] FROM ( ";
                    //kohlquery = "select 'Always \"IN\"' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,\"No\", or \"Guaranteed\"' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? \"1\"' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all ";      
                }
                String strquery = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    strquery = kohlquery + strqueryProduct + StrVariant + ") as A";
                }
                else
                {
                    strquery = strqueryProduct + StrVariant;
                }

                
                DataSet dsEcommProducts = new DataSet();
                //dsEcommProducts = CommonComponent.GetCommonDataSet("SELECT DISTINCT C.* FROM ("+strquery+") as C");

               
                SqlDataAdapter Adpt = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
                SqlCommand cmd = new SqlCommand();
                try
                {
                    dsEcommProducts = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT DISTINCT C.* FROM (" + strquery + ") as C";
                    cmd.CommandTimeout = 600;
                    cmd.Connection = conn;

                    Adpt.SelectCommand = cmd;
                    Adpt.Fill(dsEcommProducts);
                }
                catch (Exception ex)
                {
                    dsEcommProducts = null;

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                finally
                {
                    if (conn != null)
                        if (conn.State == ConnectionState.Open) conn.Close();
                    cmd.Dispose();
                    Adpt.Dispose();
                }

                //Response.Write("SELECT DISTINCT C.* FROM (" + strquery + ") as C");
                if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0 && dsEcommProducts.Tables[0].Rows.Count > 0)
                {
                    if (rdocsv.Checked)
                    {
                        GenerateCSV(dsEcommProducts);
                    }
                    else if (rdoexcel.Checked)
                    {
                        GenerateExcel(dsEcommProducts);
                    }



                }
                else
                {
                    if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0)
                    {
                        if (rdocsv.Checked)
                        {
                            GenerateCSV(dsEcommProducts);
                        }
                        else if (rdoexcel.Checked)
                        {
                            GenerateExcel(dsEcommProducts);
                        }
                    }
                }


            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "alert('Template Data not Found.');", true);
                return;
            }


        }

        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }


        private void GenerateExcel(DataSet Dstt)
        {
            DataSet Ds = new DataSet();
            string overrepemail = "";
            overrepemail = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='OverStockRepEmailID' and storeid=1"));

            DataTable dttemp = Dstt.Tables[0].Clone();

            for (int i = 0; i < dttemp.Columns.Count; i++)
            {
                dttemp.Columns[i].DataType = typeof(string);
            }
            foreach (DataRow row in Dstt.Tables[0].Rows)
            {
                dttemp.ImportRow(row);
            }


            Ds.Tables.Add(dttemp);


            try
            {



                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    DataColumn col1 = new DataColumn("ColumnName", typeof(string));
                    dt1.Columns.Add(col1);
                    DataColumn col2 = new DataColumn("ColumnNo", typeof(int));
                    dt1.Columns.Add(col2);
                    DataColumn col3 = new DataColumn("flag", typeof(int));
                    dt1.Columns.Add(col3);
                    DataSet dsdefaultdata = new DataSet();
                    for (int j = 0; j < Ds.Tables[0].Columns.Count; j++)
                    {
                        dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                        if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                        {
                            DataRow drr = dt1.NewRow();

                            drr["ColumnName"] = Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                            drr["ColumnNo"] = j;
                            drr["flag"] = 1;
                            dt1.Rows.Add(drr);
                        }
                        else
                        {
                            DataRow drr = dt1.NewRow();
                            drr["ColumnName"] = Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                            drr["ColumnNo"] = j;
                            drr["flag"] = 0;
                            dt1.Rows.Add(drr);
                        }
                    }
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        for (int c = 0; c < Ds.Tables[0].Columns.Count; c++)
                        {

                            dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + Ds.Tables[0].Columns[c].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Session["Storeidnew"].ToString() + "))");
                            if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                            {


                                for (int kk = 0; kk < dsdefaultdata.Tables[0].Rows.Count; kk++)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) && dt1.Rows[c]["flag"].ToString() == "1" && !string.IsNullOrEmpty(Ds.Tables[0].Rows[i][dt1.Rows[c]["ColumnName"].ToString()].ToString()) && Convert.ToBoolean(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) == Convert.ToBoolean(Ds.Tables[0].Rows[i][dt1.Rows[c]["ColumnName"].ToString()].ToString()))
                                        {
                                            Ds.Tables[0].Rows[i][c] = dsdefaultdata.Tables[0].Rows[kk]["Assignedvalue"].ToString();
                                            Ds.Tables[0].AcceptChanges();


                                        }
                                    }
                                    catch
                                    {

                                    }

                                }


                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[i][c].ToString().Trim()))
                                {
                                    Ds.Tables[0].Rows[i][c] = (Ds.Tables[0].Rows[i][c].ToString().Trim().Replace("01/01/00", "").Replace("01/01/1900", "").Replace("1/1/1900", "").Replace("1/1/00", ""));
                                }
                                else
                                {
                                    Ds.Tables[0].Rows[i][c] = "";
                                }
                            }

                        }

                    }
                }
                DateTime dt = DateTime.Now;
                //  string StrStorename = "";
                //  StrStorename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Session["Storeidnew"].ToString() + ""));
                String FileName = "";

                //   FileName = StrStorename.ToString().Replace(".csv", "").Replace(".xls", "").Replace(".xlsx", "").ToString() + "_" + dt.Month + "_" + dt.Day + "_" + dt.Year + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + ".xls";
                //    FileName = ddlstore.SelectedItem.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".xls";
                if (!String.IsNullOrEmpty(txtfilename.Text))
                {
                    FileName = txtfilename.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".xls";
                }
                else
                {

                    FileName = ddlstore.SelectedItem.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".xls";
                }
                if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                {
                    for (int i = 1; i < 100; i++)
                    {
                        if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i + ".xls")))
                        {


                        }
                        else
                        {
                            FileName = FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i.ToString() + ".xls";
                            break;
                        }
                    }
                }

                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));
                //xlWorkBook.SaveAs(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();
                //releaseObject(xlWorkSheet);
                //releaseObject(xlWorkBook);
                //releaseObject(xlApp);
                 string overstockheader = "";

            overstockheader = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='ShowOverStockheader' and storeid=1"));
            Boolean ShowOverStockheader = false;
            Boolean.TryParse(overstockheader, out ShowOverStockheader);
                int[] iColumns = new int[Ds.Tables[0].Columns.Count];
                for (int i = 1; i < Ds.Tables[0].Columns.Count; i++)
                {
                    iColumns[i] = i;
                }
                if ((ddlstore.SelectedItem.Text.ToString().ToLower() == "over" || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk" || ddlstore.SelectedItem.Text.ToString().ToLower() == "over stock" || ddlstore.SelectedItem.Text.ToString().ToLower() == "overstock" || ddlstore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1) )
                {
                    if((ShowOverStockheader==false))
                    {
                        for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }
                            Ds.Tables[0].Columns[i].ColumnName = clo;
                            Ds.Tables[0].AcceptChanges();
                        }

                    }
                    
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {

                    DataRow dr = Ds.Tables[0].NewRow();
                    //for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        dr[i] = "MANDATORY COLUMNS ARE MARKED IN ORANGE, RECOMMENDED COLUMNS ARE MARKED IN BLUE.  ** Reference the Supplier Guide for details on Best Practice for your Inventory Updates.";
                    //    }
                    //    else
                    //    {
                    //        dr[i] = " ";
                    //    }

                    //}
                    //Ds.Tables[0].Rows.InsertAt(dr, 0);
                    //dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            dr[i] = "When you are done click \"File\" and then select \"Save As\".";
                        }
                        else if (i == 1)
                        {
                            dr[i] = " **Reference the Supplier Guide for details on Best Practice for your Inventory Updates.";
                        }
                        else
                        {
                            dr[i] = " ";
                        }

                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 0);
                    dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            dr[i] = "At the Bottom of the Save window make sure the \"Save As Type\" dropdown menu has Excel 97-2003 workbook (*.xls) or Excel workbook (*.xlsx) selected.";
                        }
                        else
                        {
                            dr[i] = " ";
                        }

                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 1);
                    dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        dr[i] = Ds.Tables[0].Columns[i].ColumnName.ToString();
                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 2);

                    Ds.Tables[0].AcceptChanges();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "MANDATORY COLUMNS ARE MARKED IN ORANGE.RECOMMENDED COLUMNS ARE MARKED IN BLUE.";
                        }

                        else
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }

                            Ds.Tables[0].Columns[i].ColumnName = clo;
                        }

                    }
                    Ds.Tables[0].AcceptChanges();
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    DataRow dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        dr[i] = Ds.Tables[0].Columns[i].ColumnName.ToString();
                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 0);
                    Ds.Tables[0].AcceptChanges();


                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = overrepemail;
                            //   Ds.Tables[0].Columns[i].ColumnName = "artmoyatheeff.com";
                        }
                        else if (i == 1)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "83290";
                        }
                        else if (i == 2)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "replenishment";
                        }

                        else
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }

                            Ds.Tables[0].Columns[i].ColumnName = clo;
                        }

                    }


                    Ds.Tables[0].AcceptChanges();

                }


                try
                {
                    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");

                    objExport.ExportDetails(Ds.Tables[0], iColumns, RKLib.ExportData.Export.ExportFormat.Excel, Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName));
                }
                catch { }


                ViewState["LastGeneratedFeedFileName"] = null;
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName);

                ViewState["LastGeneratedFeedFileName"] = FileName.ToString();


                if (ViewState["LastGeneratedFeedFileName"] != null)
                {
                    GenerateInventoryFeedComponent objinv = new GenerateInventoryFeedComponent();
                    if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                    {
                        objinv.InsertFeedLog(ViewState["LastGeneratedFeedFileName"].ToString(), Convert.ToInt32(Session["AdminID"].ToString()), Convert.ToInt32(ddlstore.SelectedValue.ToString()));
                    }
                    // DownloadProductExportExcel();
                    btndownloadnow.Visible = true;

                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Data Exported Successfully.');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Record(s) not Found.');", true);
                    return;
                }


            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('" + ex.Message.ToString() + "');", true);
            }


        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);

                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        private void GenerateCSV(DataSet Ds)
        {


            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            dsorder = Ds;
            SecurityComponent objsec = new SecurityComponent();

            string column = "";
            string columnnom = "";

            string overstockheader = "";

            overstockheader = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='ShowOverStockheader' and storeid=1"));
            string overrepemail = "";
            overrepemail = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='OverStockRepEmailID' and storeid=1"));
            Boolean ShowOverStockheader = false;
            Boolean.TryParse(overstockheader, out ShowOverStockheader);



            if (ddlstore.SelectedItem.Text.ToString().ToLower() != "over" && ddlstore.SelectedItem.Text.ToString().ToLower() != "ostk" && ddlstore.SelectedItem.Text.ToString().ToLower() != "over stock" && ddlstore.SelectedItem.Text.ToString().ToLower() != "overstock" && ddlstore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") <= -1)
            {
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Columns.Count > 0)
                {
                    for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                    {
                        if (dsorder.Tables[0].Columns.Count - 1 == i)
                        {
                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                            columnnom += "{" + i.ToString() + "}";
                        }
                        else
                        {

                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                            columnnom += "{" + i.ToString() + "},";
                        }
                    }
                }
            }
            else
            {
                if (ShowOverStockheader==false)
                {
                    if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Columns.Count > 0)
                    {
                        for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                        {
                            if (dsorder.Tables[0].Columns.Count - 1 == i)
                            {
                                // column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                                columnnom += "{" + i.ToString() + "}";
                            }
                            else
                            {

                                //  column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                                columnnom += "{" + i.ToString() + "},";
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                    {
                        if (dsorder.Tables[0].Columns.Count - 1 == i)
                        {
                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                            columnnom += "{" + i.ToString() + "}";
                        }
                        else
                        {

                            column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                            columnnom += "{" + i.ToString() + "},";
                        }
                    }
                }
            }

            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Record(s) not Found.');", true);
            //    return;
            //}

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                DataSet dsdefaultdata = new DataSet();
                DataTable dt = new DataTable();
                DataColumn col1 = new DataColumn("ColumnName", typeof(string));
                dt.Columns.Add(col1);
                DataColumn col2 = new DataColumn("ColumnNo", typeof(int));
                dt.Columns.Add(col2);
                DataColumn col3 = new DataColumn("flag", typeof(int));
                dt.Columns.Add(col3);
                //   DataRow dr = null;

                for (int j = 0; j < dsorder.Tables[0].Columns.Count; j++)
                {

                    dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                    if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = dt.NewRow();

                        drr["ColumnName"] = dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                        drr["ColumnNo"] = j;
                        drr["flag"] = 1;
                        dt.Rows.Add(drr);
                    }
                    else
                    {
                        DataRow drr = dt.NewRow();
                        drr["ColumnName"] = dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                        drr["ColumnNo"] = j;
                        drr["flag"] = 0;
                        dt.Rows.Add(drr);
                    }


                }

                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dt.Rows.Count; c++)
                    {

                        dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + dsorder.Tables[0].Columns[c].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Session["Storeidnew"].ToString() + "))");
                        if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                        {
                            bool chk = false;

                            for (int kk = 0; kk < dsdefaultdata.Tables[0].Rows.Count; kk++)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) && !string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][dt.Rows[c]["ColumnName"].ToString()].ToString()) && dt.Rows[c]["flag"].ToString() == "1" && Convert.ToBoolean(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) == Convert.ToBoolean(dsorder.Tables[0].Rows[i][dt.Rows[c]["ColumnName"].ToString()].ToString()))
                                    {
                                        chk = true;
                                        args[c] = _EscapeCsvField(dsdefaultdata.Tables[0].Rows[kk]["Assignedvalue"].ToString());
                                    }
                                }
                                catch
                                {

                                }

                            }


                            if (chk == false)
                            {
                                //args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                                if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString()))
                                {
                                    args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                                }
                                else
                                {
                                    args[c] = "";
                                }
                            }

                        }
                        else
                        {
                            //args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                            if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString()))
                            {
                                args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace("01/01/00", "").Replace("01/01/1900", "").Replace("1/1/1900", "").Replace("1/1/00", ""));
                            }
                            else
                            {
                                args[c] = "";
                            }
                        }

                        //if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower().IndexOf("discontinue") > -1)
                        //{
                        //    bool checkall;

                        //    bool.TryParse(dsorder.Tables[0].Rows[i][c].ToString(), out checkall);
                        //    if (checkall)
                        //    {
                        //        if (Session["Storeidnew"].ToString() == "4" || Session["Storeidnew"].ToString() == "11" || Session["Storeidnew"].ToString() == "12")
                        //        {
                        //            args[c] = "1";
                        //        }
                        //        else
                        //        {
                        //            args[c] = "YES";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (Session["Storeidnew"].ToString() == "4" || Session["Storeidnew"].ToString() == "11" || Session["Storeidnew"].ToString() == "12")
                        //        {
                        //            args[c] = "0";
                        //        }
                        //        else
                        //        {
                        //            args[c] = "";
                        //        }

                        //    }

                        //}
                        //else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower().IndexOf("status") > -1 || dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower().IndexOf("active") > -1)
                        //{
                        //    bool checkactive;
                        //    bool.TryParse(dsorder.Tables[0].Rows[i][c].ToString(), out checkactive);
                        //    if (checkactive)
                        //    {
                        //        args[c] = "Active";
                        //    }
                        //    else
                        //    {
                        //        args[c] = "Inactive";
                        //    }

                        //}
                        //else
                        //{
                        //args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        //}




                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                string FullString = sb.ToString();
                sb.Remove(0, sb.Length);
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    sb.AppendLine("MANDATORY COLUMNS ARE MARKED IN ORANGE, RECOMMENDED COLUMNS ARE MARKED IN BLUE.  ** Reference the Supplier Guide for details on Best Practice for your Inventory Updates.");
                    sb.AppendLine("When you are done click \"File\" and then select \"Save As\".");
                    sb.AppendLine("At the Bottom of the Save window make sure the \"Save As Type\" dropdown menu has Excel 97-2003 workbook (*.xls) or Excel workbook (*.xlsx) selected.");
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    object[] argsover = new object[3];
                    argsover[0] = overrepemail;
                    argsover[1] = "83290";
                    argsover[2] = "replenishment";
                    sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\"", argsover));
                }
                sb.AppendLine(FullString);

                DateTime dt = DateTime.Now;
                //  string StrStorename = "";
                //   StrStorename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Session["Storeidnew"].ToString() + ""));
                String FileName = "";

                //  FileName = StrStorename.ToString().Replace(".csv", "").Replace(".xls", "").Replace(".xlsx", "").ToString() + "_" + dt.Month + "_" + dt.Day + "_" + dt.Year + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + ".csv";

                if (!String.IsNullOrEmpty(txtfilename.Text))
                {
                    FileName = txtfilename.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                }
                else
                {

                    FileName = ddlstore.SelectedItem.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                }




                if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                {
                    for (int i = 1; i < 100; i++)
                    {
                        if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i + ".csv")))
                        {


                        }
                        else
                        {
                            FileName = FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i.ToString() + ".csv";
                            break;
                        }
                    }
                }

                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                ViewState["LastGeneratedFeedFileName"] = null;
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName);

                ViewState["LastGeneratedFeedFileName"] = FileName.ToString();
                WriteFile(sb.ToString(), FilePath);
            }
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                GenerateInventoryFeedComponent objinv = new GenerateInventoryFeedComponent();
                if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                {
                    objinv.InsertFeedLog(ViewState["LastGeneratedFeedFileName"].ToString(), Convert.ToInt32(Session["AdminID"].ToString()), Convert.ToInt32(Session["Storeidnew"].ToString()));
                }

                btndownloadnow.Visible = true;


                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Data Exported Successfully.');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Record(s) not Found.');", true);
                return;
            }


        }

        protected void DownloadProductExportExcel()
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + ViewState["LastGeneratedFeedFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                if (File.Exists(FilePath))
                {



                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "application/ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastGeneratedFeedFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('File not found.');", true);
                return;
            }
        }


        protected void DownloadProductExportCSV()
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + ViewState["LastGeneratedFeedFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastGeneratedFeedFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('File not found.');", true);
                return;
            }
        }
        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
        protected void btngeneratefeed_Click(object sender, EventArgs e)
        {
            //Response.Redirect("generateinventoryfeed.aspx");
            btndownloadnow.Visible = false;
            GetFieldTemplate();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide();$('#mapping-probox').hide();$('#sequence-probox').hide();$('#generate-probox').fadeIn('slow');$('#setup-wizard').removeClass();$('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass();$('#mapping-wizard').addClass('currentpro active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('currentpro active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('current active-trail');$('#generate-wizard').parent().addClass('active');$('#sequence-wizard').parent().removeClass('active');", true);
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            Session["Storeidnew"] = null;
            DataTable dtnew = new DataTable();
            string filename = ".csv";
            if (rdoexcel.Checked)
            {
                filename = ".xls";
            }
            if (Session["datacolumn"] != null)
            {
                Int32 Storeid = 0;
                if (ddlstore.SelectedIndex > 0)
                {

                    Storeid = Convert.ToInt32(ddlstore.SelectedValue.ToString());

                }

                else if (txtstorename.Text.ToString().Trim() != "")
                {
                    ltrstorename.Text = txtstorename.Text.ToString().Trim();
                    Storeid = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_Replenishment_Store(StoreName,Deleted) VALUES ('" + txtstorename.Text.ToString().Trim().Replace("'", "''") + "',0); SELECT SCOPE_IDENTITY();"));
                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Store(StoreName,Deleted) VALUES ('" + txtstorename.Text.ToString().Trim().Replace("'", "''") + "',1)");


                }
                else if (ddlexiststore.SelectedIndex > 0)
                {
                    Storeid = Convert.ToInt32(ddlexiststore.SelectedValue.ToString());



                }

                Session["Storeidnew"] = Storeid.ToString();
                string templateId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Storeid + ""));
                if (string.IsNullOrEmpty(templateId) || templateId.ToString() == "0")
                {
                    templateId = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_Replenishment_Feedtemplate(StoreId,Filename,Fileformat,Createdby) VALUES (" + Storeid + ",'" + txtfilename.Text.ToString().Trim().Replace("'", "''") + filename + "','" + filename + "'," + Session["AdminId"].ToString() + "); SELECT SCOPE_IDENTITY();"));
                }
                else
                {
                    CommonComponent.ExecuteCommonData("update tb_Replenishment_Feedtemplate SET Filename='" + txtfilename.Text.ToString().Trim().Replace("'", "''") + filename + " ', Fileformat='" + filename + "' WHERE InventoryFeedTemplateID=" + templateId + "");
                }


                dtnew = (DataTable)Session["datacolumn"];

                DataSet dsdelete = new DataSet();
                dsdelete = CommonComponent.GetCommonDataSet("SELECT ReplenishmentFieldID,FieldName FROM tb_Replenishment_FeedtemplateDetail WHERE InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + Storeid + ")");
                if (dsdelete != null && dsdelete.Tables.Count > 0 && dsdelete.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dsdelete.Tables[0].Rows.Count; k++)
                    {
                        DataRow[] dr = dtnew.Select("ID=" + dsdelete.Tables[0].Rows[k]["ReplenishmentFieldID"].ToString() + "");
                        if (dr != null && dr.Length > 0)
                        {

                        }
                        else
                        {

                            CommonComponent.ExecuteCommonData("DELETE FROM tb_Replenishment_FeedtemplateDetail WHERE ReplenishmentFieldID=" + dsdelete.Tables[0].Rows[k]["ReplenishmentFieldID"].ToString() + "");
                            CommonComponent.ExecuteCommonData("INSERT INTO tb_Replenishment_FeedtemplateDetailLog(FiledName,CreatedBy,StoreId,Type) VALUES ('" + dsdelete.Tables[0].Rows[k]["FieldName"].ToString().Replace("'", "''") + "'," + Session["AdminID"].ToString() + "," + Storeid + ",'Delete')");
                        }
                    }
                }
                if (dtnew != null && dtnew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        Int32 Static = 0;
                        if (!string.IsNullOrEmpty(dtnew.Rows[i]["Staticvalue"].ToString()))
                        {
                            Static = 1;
                        }
                        if (dtnew.Rows[i]["Id"].ToString() == "0")
                        {
                            Int32 ScopeIdentity = 0;
                            ScopeIdentity = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO tb_Replenishment_FeedtemplateDetail(InventoryFeedTemplateID,FieldName,IsRequired,IsStatic,StaticValue,DisplayOrder,MappingColumn,Createdby) VALUES (" + templateId + ",'" + dtnew.Rows[i]["Columnname"].ToString().Trim().Replace("'", "''") + "','" + dtnew.Rows[i]["isRequired"].ToString().Trim().Replace("'", "''") + "'," + Static + ",'" + dtnew.Rows[i]["Staticvalue"].ToString().Trim().Replace("'", "''") + "'," + dtnew.Rows[i]["Displayorder"].ToString().Trim() + ",'" + dtnew.Rows[i]["MappingColumn"].ToString().Trim().Replace("'", "''") + "'," + Session["AdminId"].ToString() + "); SELECT SCOPE_IDENTITY();"));

                            if (!string.IsNullOrEmpty(dtnew.Rows[i]["isconfigure"].ToString()) && dtnew.Rows[i]["isconfigure"].ToString() == "1")
                            {
                                CommonComponent.ExecuteCommonData("if(NOT EXISTS(SELECT ConfigureId FROM tb_Replenishment_configure WHERE  FieldID=" + ScopeIdentity.ToString() + ")) begin INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + ScopeIdentity.ToString() + ",1,'" + dtnew.Rows[i]["Assignedvalueyes"].ToString().Replace("'", "''") + "'); INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + ScopeIdentity.ToString() + ",0,'" + dtnew.Rows[i]["Assignedvalueno"].ToString().Replace("'", "''") + "') end else begin  update tb_Replenishment_configure SET Assignedvalue='" + dtnew.Rows[i]["Assignedvalueyes"].ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=1 and FieldID=" + ScopeIdentity.ToString() + "; update tb_Replenishment_configure SET Assignedvalue='" + dtnew.Rows[i]["Assignedvalueno"].ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=0 and FieldID=" + ScopeIdentity.ToString() + " end");
                            }
                            else
                            {
                                CommonComponent.ExecuteCommonData("DELETE FROM tb_Replenishment_configure WHERE  FieldID=" + ScopeIdentity.ToString() + "");
                            }

                            //try
                            //{
                            //    if (Session["configure"] != null)
                            //    {
                            //        DataTable dt = (DataTable)Session["configure"];
                            //        if (dt != null)
                            //        {
                            //            DataRow[] dr = dt.Select("id=" + dtnew.Rows[i]["columnId"].ToString() + "");
                            //            if (dr.Length > 0)
                            //            {
                            //                for (int drconfig = 0; drconfig < dr.Length; drconfig++)
                            //                {
                            //                    if (Convert.ToBoolean(dr[drconfig]["Dbvalue"].ToString()))
                            //                    {
                            //                        CommonComponent.ExecuteCommonData("INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + ScopeIdentity.ToString() + ",1,'" + dr[drconfig]["Assignedvalue"].ToString().Replace("'", "''") + "')");
                            //                    }
                            //                    else
                            //                    {
                            //                        CommonComponent.ExecuteCommonData("INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + ScopeIdentity.ToString() + ",0,'" + dr[drconfig]["Assignedvalue"].ToString().Replace("'", "''") + "')");
                            //                    }

                            //                }


                            //            }
                            //        }
                            //    }
                            //}
                            //catch
                            //{

                            //}

                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(dtnew.Rows[i]["isconfigure"].ToString()) && dtnew.Rows[i]["isconfigure"].ToString() == "1")
                            {
                                CommonComponent.ExecuteCommonData("if(NOT EXISTS(SELECT ConfigureId FROM tb_Replenishment_configure WHERE  FieldID=" + dtnew.Rows[i]["Id"].ToString() + ")) begin INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + dtnew.Rows[i]["Id"].ToString() + ",1,'" + dtnew.Rows[i]["Assignedvalueyes"].ToString().Replace("'", "''") + "'); INSERT INTO tb_Replenishment_configure(FieldID,Dbvalue,Assignedvalue) VALUES (" + dtnew.Rows[i]["Id"].ToString() + ",0,'" + dtnew.Rows[i]["Assignedvalueno"].ToString().Replace("'", "''") + "') end else begin  update tb_Replenishment_configure SET Assignedvalue='" + dtnew.Rows[i]["Assignedvalueyes"].ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=1 and FieldID=" + dtnew.Rows[i]["Id"].ToString() + "; update tb_Replenishment_configure SET Assignedvalue='" + dtnew.Rows[i]["Assignedvalueno"].ToString().Replace("'", "''") + "'  WHERE isnull(Dbvalue,0)=0 and FieldID=" + dtnew.Rows[i]["Id"].ToString() + " end");
                            }
                            else
                            {
                                CommonComponent.ExecuteCommonData("DELETE FROM tb_Replenishment_configure WHERE  FieldID=" + dtnew.Rows[i]["Id"].ToString() + "");
                            }

                            CommonComponent.ExecuteCommonData("UPDATE tb_Replenishment_FeedtemplateDetail SET MappingColumn='" + dtnew.Rows[i]["MappingColumn"].ToString().Trim().Replace("'", "''") + "', FieldName='" + dtnew.Rows[i]["Columnname"].ToString().Trim().Replace("'", "''") + "',IsRequired='" + dtnew.Rows[i]["isRequired"].ToString().Trim().Replace("'", "''") + "',IsStatic=" + Static + ",StaticValue='" + dtnew.Rows[i]["Staticvalue"].ToString().Trim().Replace("'", "''") + "',DisplayOrder=" + dtnew.Rows[i]["Displayorder"].ToString().Trim() + " WHERE ReplenishmentFieldID=" + dtnew.Rows[i]["Id"].ToString() + "");
                        }

                    }
                }
                btngeneratefeed.Visible = true;
                btnsave.Visible = false;
                btndone.Visible = true;
                //sequence - btn - back.Visible = false;
                final_btn_back.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide();$('#mapping-probox').hide();$('#sequence-probox').hide();$('#generate-probox').fadeIn('slow');$('#setup-wizard').removeClass();$('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass();$('#mapping-wizard').addClass('currentpro active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('currentpro active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('current active-trail');$('#generate-wizard').parent().addClass('active');$('#sequence-wizard').parent().removeClass('active');", true);
            }
        }
        protected void btndownloadnow_Click(object sender, EventArgs e)
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                if (ViewState["LastGeneratedFeedFileName"].ToString().ToLower().IndexOf(".csv") > -1)
                {
                    DownloadProductExportCSV();
                }
                else
                {
                    DownloadProductExportExcel();
                }

            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loanexttab", "$('#setup-probox').hide();$('#mapping-probox').hide();$('#sequence-probox').hide();$('#generate-probox').fadeIn('slow');$('#setup-wizard').removeClass();$('#setup-wizard').addClass('currentpro active-trail');$('#mapping-wizard').removeClass();$('#mapping-wizard').addClass('currentpro active-trail');$('#sequence-wizard').removeClass();$('#sequence-wizard').addClass('currentpro active-trail');$('#generate-wizard').removeClass();$('#generate-wizard').addClass('current active-trail');$('#generate-wizard').parent().addClass('active');$('#sequence-wizard').parent().removeClass('active');", true);
        }

    }
}