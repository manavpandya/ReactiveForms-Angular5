using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class GenerateExportpage : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "if(document.getElementById('idtitle')){document.getElementById('idtitle').innerHTML=document.getElementById('ContentPlaceHolder1_hdntitle').value;}", true);
            }
        }

        /// <summary>
        /// Page Init Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Init(object sender, EventArgs e)
        {

            generateScript();
        }


        /// <summary>
        /// Binds All stores into drop down
        /// </summary>
        /// <param name="ddlStore">DropDownList ddlStore</param>
        public void BindStore(DropDownList ddlStore)
        {
            DataSet dsStore = new DataSet();
            dsStore = StoreComponent.GetStoreList();
            ddlStore.Items.Clear();
            if (dsStore != null && dsStore.Tables[0].Rows.Count != 0)
            {
                for (int StoreCount = 0; StoreCount < dsStore.Tables[0].Rows.Count; StoreCount++)
                    ddlStore.Items.Add(new ListItem(dsStore.Tables[0].Rows[StoreCount]["StoreName"].ToString(), dsStore.Tables[0].Rows[StoreCount]["StoreID"].ToString()));
            }
            if (Request.QueryString["StoreId"] != null)
            {
                ddlStore.SelectedValue = Convert.ToString(Request.QueryString["StoreId"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Generates the Script
        /// </summary>
        private void generateScript()
        {
            FeedComponent objFeed = new FeedComponent();
            DataSet dsFeed = new DataSet();
            if (Request.QueryString["Storeid"] != null)
            {
                dsFeed = objFeed.GetFeedMasterByStore(Convert.ToInt32(Request.QueryString["Storeid"].ToString()));
            }
            else
            {
                dsFeed = objFeed.GetFeedMasterByStore(1);
            }
            ContentPlaceHolder objplaceHoder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");

            if (dsFeed != null && dsFeed.Tables.Count > 0 && dsFeed.Tables[0].Rows.Count > 0)
            {
                Literal objDiv = new Literal();
                //objDiv.Text = "<div style=\"vertical-align: middle; padding: 10px 21px 4px 81px; height: 18px; float: left; width: 93%;\"></div>";
                // objDiv.Text += "<div class=\"tab_box2\" style=\"padding-top: 5px;\" >";
                objDiv.Text += "<div style=\"height: 85px;\">";
                string strscript = "<script type=\"text/javascript\" language=\"javascript\">function showtab(id,id1,title,ids){";
                strscript += " var mytool_array=ids.split(',');for (i=1; i<=id1; i++) {";
                strscript += " if(i == id){if(document.getElementById('img_'+i)){document.getElementById('img_'+i).src='/FeedManagement/Images/'+mytool_array[i-1]+'_hover.jpg';}}else{if(document.getElementById('img_'+i)){document.getElementById('img_'+i).src='/FeedManagement/Images/'+mytool_array[i-1]+'.jpg';}}";
                strscript += " }";
                strscript += "if(document.getElementById('idtitle')){document.getElementById('idtitle').innerHTML=title;}document.getElementById('ContentPlaceHolder1_hdntitle').value=title;document.getElementById('ContentPlaceHolder1_hdntab').value=id;document.getElementById('ContentPlaceHolder1_hdntotal').value=id1;for(i=0;i < id1; i++){var j = i + 1;var name ='ContentPlaceHolder1_pnl_'+j;if((id == j) && (document.getElementById(name))){document.getElementById(name).style.display='block';}else{if(document.getElementById(name)){document.getElementById(name).style.display='none';}}}}";
                strscript += " function uploadfile(id,panelname){";
                strscript += " if(document.getElementById('ContentPlaceHolder1_'+id) && document.getElementById('ContentPlaceHolder1_'+id).value == ''){alert('Please select file.'); return false;}document.getElementById('ContentPlaceHolder1_hdnfile').value=id;document.getElementById('ContentPlaceHolder1_hdnnew').value=panelname;__doPostBack('ctl00$ContentPlaceHolder1$btnUpload',''); return false;";
                strscript += " }";
                strscript += " function deletefile(id,panelname,deletename){";
                strscript += " document.getElementById('ContentPlaceHolder1_hdnfile').value=id;document.getElementById('ContentPlaceHolder1_hdnnew').value=panelname;document.getElementById('ContentPlaceHolder1_deletehdn').value=deletename;__doPostBack('ctl00$ContentPlaceHolder1$btnDelete',''); return false;";
                strscript += " }";
                strscript += " function saveData(name,id){";
                strscript += "document.getElementById('ContentPlaceHolder1_hdnnew').value=name;document.getElementById('ContentPlaceHolder1_hdnfeedid').value=id;__doPostBack('ctl00$ContentPlaceHolder1$btnFinish','');";
                strscript += " }";
                strscript += "</script>";

                Response.Write(strscript);
                // Start StoreWise Image Binding

                objDiv.Text += "<ul>";
                string strIds = "";

                for (int id = 0; id < dsFeed.Tables[0].Rows.Count; id++)
                {
                    strIds += dsFeed.Tables[0].Rows[id]["FeedID"].ToString() + ",";
                }
                if (strIds != "")
                {
                    strIds = strIds + "0";

                }
                for (int Tab = 0; Tab < dsFeed.Tables[0].Rows.Count; Tab++)
                {
                    if (Request.QueryString["FID"] != null && Request.QueryString["FID"].ToString() == dsFeed.Tables[0].Rows[Tab]["FeedID"].ToString())
                    {
                        hdntitle.Value = dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString();
                    }

                    int tab1 = Tab + 1;

                    objDiv.Text += "<li style=\"list-style:none;\">";
                    if (tab1 == 1)
                    {
                        objDiv.Text += "<img id=\"img_" + tab1.ToString() + "\" src=\"" + AppLogic.AppConfigs("FeedImagePath").ToString() + dsFeed.Tables[0].Rows[Tab]["FeedID"].ToString() + "_hover.jpg" + "\" alt=\"" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "\" title=\"" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "\" style=\"cursor:pointer;\" onclick=\"showtab(" + tab1 + "," + dsFeed.Tables[0].Rows.Count + ",'" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "','" + strIds + "');\" />";
                    }
                    else
                    {
                        objDiv.Text += "<img id=\"img_" + tab1.ToString() + "\" src=\"" + AppLogic.AppConfigs("FeedImagePath").ToString() + dsFeed.Tables[0].Rows[Tab]["imagename"].ToString() + "\" alt=\"" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "\" title=\"" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "\" style=\"cursor:pointer;\" onclick=\"showtab(" + tab1 + "," + dsFeed.Tables[0].Rows.Count + ",'" + dsFeed.Tables[0].Rows[Tab]["FeedName"].ToString() + "','" + strIds + "');\" />";
                    }
                    objDiv.Text += "</li>";
                }

                objDiv.Text += "</ul>";
                objDiv.Text += "</div>";
                // End StoreWise Image Binding

                //objDiv.Text += "<div class=\"static_display\">";
                //objDiv.Text += "<div class=\"content_box\">";

                // Start title class
                //objDiv.Text += "<div class=\"title\">";
                //objDiv.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                //objDiv.Text += "<tbody><tr>";
                //objDiv.Text += "<td align=\"left\" width=\"3\" valign=\"top\">";
                //objDiv.Text += "<img  title=\"\" alt=\"\" src=\"../images/left_round_img.gif\"></td>";
                //objDiv.Text += "<td align=\"left\" valign=\"top\" class=\"order_bg\">";
                //objDiv.Text += "<div class=\"heading_1\">";
                //objDiv.Text += "<span id=\"ContentPlaceHolder1_lblTitle\">Edit <span id='idtitle'>" + dsFeed.Tables[0].Rows[0]["FeedName"].ToString() + "</span> Product</span>";
                //objDiv.Text += "</div>";

                //objDiv.Text += "<div style='float:right;margin-top:5px;color:#fff'>Store: ";
                objplaceHoder.Controls.Add(objDiv);

                DropDownList objStore = new DropDownList();
                objStore.ID = "ddlStore";
                objStore.AutoPostBack = true;
                objStore.CssClass = "product-type";
                objStore.Width = Unit.Pixel(200);
                objStore.SelectedIndexChanged += new EventHandler(objStore_SelectedIndexChanged);
                BindStore(objStore);
                //objplaceHoder.Controls.Add(objStore);

                Literal objDiv1 = new Literal();
                //objDiv1.Text = " </div>";

                //objDiv1.Text += " </td>";
                //objDiv1.Text += " <td align=\"left\" width=\"6\" valign=\"top\">";
                //objDiv1.Text += " <img title=\"\" alt=\"\" src=\"../images/round_right_cor.gif\"></td>";
                //objDiv1.Text += " </tr>";
                //objDiv1.Text += "</tbody></table>";
                //objDiv1.Text += "</div>";
                // End title class

                objDiv1.Text += "<div class=\"content-row2\">";

                objDiv1.Text += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
                objDiv1.Text += "<tbody><tr>";
                objDiv1.Text += "<td valign=\"top\" align=\"left\" height=\"5\">";
                objDiv1.Text += "<img width=\"1\" height=\"5\" src=\"/App_Themes/" + Page.Theme + "/images/spacer.gif\">";
                objDiv1.Text += "</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<td valign=\"top\" align=\"left\" height=\"5\">";
                objDiv1.Text += "<img width=\"1\" height=\"5\" src=\"/App_Themes/" + Page.Theme + "/images/spacer.gif\">";
                objDiv1.Text += "</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr>";

                // objDiv1.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" bgcolor=\"#ffffff\" width=\"100%\"><tbody><tr> <td class=\"border-td\">";
                objDiv1.Text += "<td class=\"border-td\">";

                objDiv1.Text = "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" class=\"content-table\">";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<td class=\"border-td-sub\">";

                // table add-product Start
                objDiv1.Text += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"add-product\">";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<th>";
                objDiv1.Text += "<div class=\"main-title-left\" style=\"width: 98% !important\">";
                objDiv1.Text += "<img class=\"img-left\" title=\"Add Feed Master\" alt=\"Add Feed Master\" src=\"/App_Themes/" + Page.Theme + "/Images/admin-list-icon.png\">";
                objDiv1.Text += "<h2><span style=\"line-height: 25px;\">Edit " + dsFeed.Tables[0].Rows[0]["FeedName"].ToString() + " Product </span>";

                objDiv1.Text += "<span style=\"float: right;\">Store : ";
                objplaceHoder.Controls.Add(objDiv1);
                objplaceHoder.Controls.Add(objStore);
                objDiv1 = new Literal();
                objDiv1.Text = "</span>";
                objDiv1.Text += "</h2>";
                objDiv1.Text += "</div>";
                objDiv1.Text += "</th>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr class=\"altrow\">";
                objDiv1.Text += "<td align=\"right\" style=\"font-size:12px;\"><span class=\"star\">*</span>Required Field</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr class=\"even-row\">";
                objDiv1.Text += "<td>";

                objplaceHoder.Controls.Add(objDiv1);
                for (int i = 0; i < dsFeed.Tables[0].Rows.Count; i++)
                {
                    int tab1 = i + 1;
                    DataSet dsFeedMapping = new DataSet();
                    if (Convert.ToBoolean(dsFeed.Tables[0].Rows[i]["IsBase"].ToString()) == false)
                    {
                        dsFeedMapping = objFeed.GetFeedmapping(Convert.ToInt32(dsFeed.Tables[0].Rows[i]["FeedID"].ToString()));
                        if (dsFeedMapping == null || dsFeedMapping.Tables.Count == 0 || dsFeedMapping.Tables[0].Rows.Count == 0)
                        {
                            continue;
                        }
                    }

                    Panel pnl = new Panel();
                    pnl.ID = "pnl_" + tab1.ToString();

                    if (hdnnew.Value.ToString() == "")
                    {
                        if (Request.QueryString["FID"] != null && Request.QueryString["FID"].ToString() == dsFeed.Tables[0].Rows[i]["FeedID"].ToString())
                        {
                            hdnnew.Value = "pnl_" + tab1.ToString();
                        }
                    }
                    if (hdnfeedid.Value.ToString() == "")
                    {
                        if (Request.QueryString["FID"] != null && Request.QueryString["FID"].ToString() == dsFeed.Tables[0].Rows[i]["FeedID"].ToString())
                        {
                            hdnfeedid.Value = dsFeed.Tables[0].Rows[i]["FeedID"].ToString();
                        }
                    }
                    if (hdntab.Value.ToString() == "")
                    {
                        if (Request.QueryString["FID"] != null && Request.QueryString["FID"].ToString() == dsFeed.Tables[0].Rows[i]["FeedID"].ToString())
                        {
                            hdntab.Value = tab1.ToString();
                        }
                    }

                    // New Table Start
                    Literal lt = new Literal();
                    #region Main Loop Table
                    lt.Text += "<table width=\"100%\" style=\"border:1px solid #e7e7e7;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";

                    pnl.Controls.Add(lt);
                    int k = 0;
                    int jT = 1;
                    DataSet dsFeedField = new DataSet();
                    string strSave = "<script type=\"text/javascript\" language=\"javascript\">function Checkvalidation" + dsFeed.Tables[0].Rows[i]["FeedID"].ToString() + "(name,id){";
                    dsFeedField = objFeed.GetFeedFieldDetails(Convert.ToInt32(dsFeed.Tables[0].Rows[i]["FeedID"].ToString()));

                    if (dsFeedField != null && dsFeedField.Tables.Count > 0 && dsFeedField.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsFeedField.Tables[0].Rows.Count; j++)
                        {
                            if (k == 0)
                            {
                                Literal lttr = new Literal();
                                if (jT % 2 == 0)
                                {
                                    lttr.Text = "<tr class='altrow' id='tr_pnl_" + i.ToString() + "_" + j.ToString() + "' runat='server'>";

                                }
                                else
                                {
                                    lttr.Text = "<tr id='tr_pnl_" + i.ToString() + "_" + j.ToString() + "' runat='server'>";
                                }
                                pnl.Controls.Add(lttr);
                            }
                            k = k + 1;
                            Literal ltOpen = new Literal();

                            ltOpen.Text = "<td style='padding-left:10px;width:10%;font-size:12px;color:#000;padding-top:5px;text-transform:capitalize;' valign='top'>";
                            if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                            {
                                ltOpen.Text += "<span style=\"color: rgb(255, 0, 51);\">*</span>";
                            }
                            else
                            {
                                ltOpen.Text += "<span style=\"color: rgb(255, 0, 51);\">&nbsp;</span>";
                            }
                            pnl.Controls.Add(ltOpen);

                            Label lttdlbl = new Label();
                            if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "label")
                            {
                                lttdlbl.ID = "lblskip_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + j.ToString();
                            }
                            else
                            {
                                lttdlbl.ID = "lbl_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + j.ToString();
                            }
                            lttdlbl.ForeColor = System.Drawing.Color.Black;
                            lttdlbl.Text = dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + ":";
                            pnl.Controls.Add(lttdlbl);
                            Literal lttdClose = new Literal();
                            lttdClose.Text = "</td>";
                            pnl.Controls.Add(lttdClose);

                            Literal ltOpen1 = new Literal();
                            ltOpen1.Text = "<td style='padding-left:10px;width:40%;padding-top:5px;' valign='top'>";
                            pnl.Controls.Add(ltOpen1);

                            #region Asp Control Start
                            if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "textbox")
                            {
                                TextBox objtxt = new TextBox();

                                objtxt.CssClass = "order-textfield";
                                objtxt.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));

                                objtxt.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));

                                objtxt.MaxLength = Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldLimit"].ToString());
                                objtxt.ID = "txt_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "').value == ''))";
                                    strSave += "{";
                                    strSave += " alert('Please Enter " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "').focus(); return false;";
                                    strSave += "}";

                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                if (strVal != "")
                                {
                                    objtxt.Text = strVal;
                                }
                                else
                                {
                                    objtxt.Text = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                }
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    if (objtxt.Text.ToString() == "")
                                    {
                                        objtxt.Attributes.Add("style", "background-color:yellow;");
                                    }
                                }
                                pnl.Controls.Add(objtxt);

                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "calendar")
                            {
                                string Funname = "";
                                TextBox objtxt = new TextBox();
                                objtxt.CssClass = "from-textfield";
                                objtxt.Attributes.Add("style", "margin-right: 2px;");
                                objtxt.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));

                                objtxt.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));

                                objtxt.MaxLength = Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldLimit"].ToString());
                                objtxt.ID = "txt_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();

                                Funname += "<script type=\"text/javascript\">";
                                Funname += " $(function () {$('#ContentPlaceHolder1_" + objtxt.ID + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true});});";
                                Funname += "</script>";
                                Literal ltrCalenScript = new Literal();
                                ltrCalenScript.Text = Funname.ToString();

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "').value == ''))";
                                    strSave += "{";
                                    strSave += " alert('Please Enter " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objtxt.ID + "').focus(); return false;";
                                    strSave += "}";
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                if (strVal != "")
                                {
                                    objtxt.Text = strVal;
                                }
                                else
                                {
                                    objtxt.Text = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                }
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    if (objtxt.Text.ToString() == "")
                                    {
                                        objtxt.Attributes.Add("style", "background-color:yellow;");
                                    }
                                }
                                pnl.Controls.Add(objtxt);
                                pnl.Controls.Add(ltrCalenScript);

                                //<input type="text" style="width:70px;margin-right: 3px;" class="from-textfield hasDatepicker" id="ContentPlaceHolder1_txtOrderFrom" value="06/22/2012" name="ctl00$ContentPlaceHolder1$txtOrderFrom">
                                // <img class="ui-datepicker-trigger" src="/App_Themes/Gray/images/date-icon.png" alt="" title="">
                                //     pnl.Controls.Add(objCalendor);
                            }

                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "combobox")
                            {
                                DropDownList objddl = new DropDownList();
                                objddl.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objddl.CssClass = "product-type";

                                objddl.ID = "ddl_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                DataSet DsFieldvalues = new DataSet();
                                if ((dsFeedField.Tables[0].Rows[j]["FieldName"].ToString().ToLower().IndexOf("manufac") > -1) || (dsFeedField.Tables[0].Rows[j]["FieldName"].ToString().ToLower().IndexOf("manu fac") > -1) || (dsFeedField.Tables[0].Rows[j]["FieldName"].ToString().ToLower().IndexOf("brand") > -1))
                                {
                                    objddl.Items.Clear();
                                    objddl.DataTextField = "Name";
                                    objddl.DataValueField = "ManufactureID";
                                    DataSet dsManufacture = new DataSet();
                                    if (Request.QueryString["Storeid"] != null)
                                    {
                                        dsManufacture = objFeed.GetManufacture(Convert.ToInt32(Request.QueryString["Storeid"].ToString()));
                                    }
                                    else
                                    {
                                        dsManufacture = objFeed.GetManufacture(1);
                                    }

                                    // DataSet dsManufacture = objFeed.GetManufacture(1);
                                    objddl.DataSource = dsManufacture.Tables[0];
                                    objddl.DataBind();
                                    ListItem li = new ListItem("Select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "", "0");
                                    objddl.Items.Insert(0, li);
                                }
                                else
                                {
                                    DsFieldvalues = objFeed.GetFieldValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                    if (DsFieldvalues != null && DsFieldvalues.Tables.Count > 0 && DsFieldvalues.Tables[0].Rows.Count > 0)
                                    {
                                        objddl.Items.Clear();
                                        objddl.DataTextField = "FieldValues";
                                        objddl.DataValueField = "FieldValues";
                                        objddl.DataSource = DsFieldvalues.Tables[0];
                                        objddl.DataBind();

                                    }
                                    ListItem li = new ListItem("Select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "", "0");
                                    objddl.Items.Insert(0, li);
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        objddl.Items.FindByText(strVal.ToString()).Selected = true;
                                    }
                                    else
                                    {
                                        if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                        {
                                            objddl.Items.FindByText(objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()))).Selected = true;
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objddl.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objddl.ID + "').selectedIndex == 0))";
                                    strSave += "{";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objddl.ID + "').focus(); return false;";
                                    strSave += "}";

                                    if (objddl.SelectedIndex == 0)
                                    {
                                        objddl.Attributes.Add("style", "background-color:yellow");
                                    }
                                }

                                pnl.Controls.Add(objddl);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "category")
                            {
                                DropDownList objddl = new DropDownList();
                                objddl.CssClass = "product-type";
                                objddl.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));

                                objddl.ID = "ddl_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                DataSet DsFieldvalues = new DataSet();

                                DsFieldvalues = objFeed.GetFieldValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                if (DsFieldvalues != null && DsFieldvalues.Tables.Count > 0 && DsFieldvalues.Tables[0].Rows.Count > 0)
                                {
                                    SetCategory(objddl, Convert.ToString(DsFieldvalues.Tables[0].Rows[0]["FieldValues"].ToString()), objStore, dsFeedField.Tables[0].Rows[j]["FieldName"].ToString());
                                }
                                else
                                {

                                    SetCategory(objddl, "0", objStore, dsFeedField.Tables[0].Rows[j]["FieldName"].ToString());
                                }

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objddl.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objddl.ID + "').selectedIndex == 0))";
                                    strSave += "{";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objddl.ID + "').focus(); return false;";
                                    strSave += "}";
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        objddl.Items.FindByText(strVal).Selected = true;
                                    }
                                    else
                                    {
                                        if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                        {
                                            objddl.Items.FindByText(objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()))).Selected = true;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    if (objddl.SelectedIndex == 0)
                                    {
                                        objddl.Attributes.Add("style", "background-color:yellow");
                                    }
                                }

                                pnl.Controls.Add(objddl);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "label")
                            {
                                Label objlbl = new Label();
                                objlbl.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objlbl.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));
                                objlbl.ID = "lbl_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));

                                if (strVal != "")
                                {
                                    objlbl.Text = strVal.ToString();
                                }
                                else
                                {
                                    if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                    {
                                        objlbl.Text = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                    }
                                }


                                pnl.Controls.Add(objlbl);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "checkbox")
                            {
                                CheckBox objChk = new CheckBox();
                                objChk.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objChk.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));
                                objChk.ID = "chk_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objChk.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objChk.ID + "').checked == false))";
                                    strSave += "{";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objChk.ID + "').focus(); return false;";
                                    strSave += "}";

                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        objChk.Checked = Convert.ToBoolean(strVal.ToString());
                                    }
                                    else
                                    {
                                        if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                        {
                                            objChk.Checked = Convert.ToBoolean(objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())));
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                pnl.Controls.Add(objChk);

                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "radiobutton")
                            {
                                RadioButton objRadio = new RadioButton();
                                objRadio.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objRadio.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));
                                objRadio.ID = "rdo_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objRadio.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objRadio.ID + "').checked == false))";
                                    strSave += "{";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objRadio.ID + "').focus(); return false;";
                                    strSave += "}";
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        objRadio.Checked = Convert.ToBoolean(strVal.ToString());
                                    }
                                    else
                                    {
                                        if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                        {
                                            objRadio.Checked = Convert.ToBoolean(objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())));
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                pnl.Controls.Add(objRadio);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "textarea")
                            {
                                TextBox objArea = new TextBox();
                                objArea.CssClass = "description-textarea";
                                objArea.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objArea.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));
                                objArea.MaxLength = Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldLimit"].ToString());
                                objArea.ID = "txtarea_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                objArea.Attributes.Add("style", "border: 1px solid rgb(231, 231, 231); background: none repeat scroll 0px 0px rgb(231, 231, 231); visibility: hidden; display: none;");
                                string strscript1 = "";
                                strscript1 += "<script type=\"text/javascript\">";
                                strscript1 += "CKEDITOR.replace('ContentPlaceHolder1_txtarea_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString() + "', { baseHref: '<%= ResolveUrl(\"~/ckeditor/\") %>', height: " + dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString() + ",width: " + dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString() + " });";
                                strscript1 += "CKEDITOR.on('dialogDefinition', function (ev) {";
                                strscript1 += "if (ev.data.name == 'image') {";
                                strscript1 += "var btn = ev.data.definition.getContents('info').get('browse');";
                                strscript1 += "btn.hidden = false;";
                                strscript1 += "btn.onClick = function () { window.open(CKEDITOR.basePath +'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };";
                                strscript1 += "}";
                                strscript1 += "if (ev.data.name == 'link') {";
                                strscript1 += " var btn = ev.data.definition.getContents('info').get('browse');";
                                strscript1 += " btn.hidden = false;";
                                strscript1 += "btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };";
                                strscript1 += "}";
                                strscript1 += "});";
                                strscript1 += "</script>";
                                Literal ltScript = new Literal();
                                ltScript.Text = strscript1.ToString();
                                objArea.TextMode = TextBoxMode.MultiLine;
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + objArea.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + objArea.ID + "').value == ''))";
                                    strSave += "{";
                                    strSave += " alert('Please Enter " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objArea.ID + "').focus(); return false;";
                                    strSave += "}";
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        objArea.Text = strVal.ToString();
                                    }
                                    else
                                    {
                                        objArea.Text = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                    }
                                }
                                catch
                                {
                                }
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    if (objArea.Text.ToString() == "")
                                    {
                                        objArea.Attributes.Add("style", "background-color:yellow;");
                                    }
                                }
                                pnl.Controls.Add(objArea);
                                pnl.Controls.Add(ltScript);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "checkboxlist")
                            {
                                CheckBoxList objchkList = new CheckBoxList();
                                objchkList.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));
                                objchkList.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));
                                objchkList.ID = "chklist_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                DataSet DsFieldvalues = new DataSet();
                                DsFieldvalues = objFeed.GetFieldValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                if (DsFieldvalues != null && DsFieldvalues.Tables.Count > 0 && DsFieldvalues.Tables[0].Rows.Count > 0)
                                {
                                    objchkList.Items.Clear();
                                    objchkList.DataTextField = "FieldValues";
                                    objchkList.DataValueField = "FieldValues";
                                    objchkList.DataSource = DsFieldvalues.Tables[0];
                                    objchkList.DataBind();
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        if (strVal.IndexOf(",") > -1)
                                        {
                                            string[] strln = strVal.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            for (int c = 0; c < strln.Length; c++)
                                            {
                                                foreach (ListItem item in objchkList.Items)
                                                {
                                                    if (item.Value.ToString().ToLower() == strln[c].ToString().ToLower())
                                                    {
                                                        item.Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (strVal != "")
                                            {
                                                objchkList.SelectedValue = strVal.ToString();
                                            }
                                            else
                                            {
                                                if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                                {
                                                    objchkList.SelectedValue = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                                }
                                            }
                                        }
                                    }

                                }
                                catch
                                {
                                }

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]) && objchkList.Items.Count > 0)
                                {
                                    strSave += "if(document.getElementById('ContentPlaceHolder1_" + objchkList.ID + "'))";
                                    strSave += "{var listItemArray = document.getElementsByName('ContentPlaceHolder1_" + objchkList.ID + "');";
                                    strSave += "var isItemChecked = false;";
                                    strSave += "for (var i=0; i<listItemArray.length; i++){var listItem = listItemArray[i]; if(listItem.checked ){isItemChecked = true;}}if (isItemChecked == false){";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objchkList.ID + "').focus(); return false;}";
                                    strSave += "}";
                                }
                                pnl.Controls.Add(objchkList);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "radiobuttonlist")
                            {
                                RadioButtonList objradio = new RadioButtonList();

                                objradio.Width = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldWidth"].ToString()));

                                objradio.Height = Unit.Pixel(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldHeight"].ToString()));

                                objradio.ID = "rdolist_" + j.ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                DataSet DsFieldvalues = new DataSet();

                                DsFieldvalues = objFeed.GetFieldValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                if (DsFieldvalues != null && DsFieldvalues.Tables.Count > 0 && DsFieldvalues.Tables[0].Rows.Count > 0)
                                {
                                    objradio.Items.Clear();
                                    objradio.DataTextField = "FieldValues";
                                    objradio.DataValueField = "FieldValues";
                                    objradio.DataSource = DsFieldvalues.Tables[0];
                                    objradio.DataBind();
                                }
                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]) && objradio.Items.Count > 0)
                                {
                                    strSave += "if(document.getElementById('ContentPlaceHolder1_" + objradio.ID + "'))";
                                    strSave += "{var listItemArray = document.getElementsByName('ContentPlaceHolder1_" + objradio.ID + "');";
                                    strSave += "var isItemChecked = false;";
                                    strSave += "for (var i=0; i<listItemArray.length; i++){var listItem = listItemArray[i]; if(listItem.checked ){isItemChecked = true;}}if ( isItemChecked == false ){";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + objradio.ID + "').focus(); return false;}";
                                    strSave += "}";
                                }
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "")
                                    {
                                        if (strVal.IndexOf(",") > -1)
                                        {
                                            string[] strln = strVal.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            for (int c = 0; c < strln.Length; c++)
                                            {
                                                foreach (ListItem item in objradio.Items)
                                                {
                                                    if (item.Value.ToString().ToLower() == strln[c].ToString().ToLower())
                                                    {
                                                        item.Selected = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (strVal != "")
                                            {
                                                objradio.SelectedValue = strVal.ToString();
                                            }
                                            else
                                            {
                                                if (objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString())) != "")
                                                {
                                                    objradio.SelectedValue = objFeed.GetFieldDefaulesValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                pnl.Controls.Add(objradio);
                            }
                            else if (dsFeedField.Tables[0].Rows[j]["TypeName"].ToString().ToLower() == "fileupload")
                            {
                                FileUpload fl = new FileUpload();
                                fl.ID = "fl_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString();
                                pnl.Controls.Add(fl);
                                ImageButton btnUpl = new ImageButton();
                                btnUpl.OnClientClick = "return uploadfile('fl_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString() + "','pnl_" + tab1.ToString() + "');";

                                btnUpl.ImageUrl = "images/upload.gif";
                                pnl.Controls.Add(btnUpl);

                                Literal objImage = new Literal();
                                string strVal = objFeed.GetFielValues(Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FeedID"].ToString()), Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(dsFeedField.Tables[0].Rows[j]["FieldID"].ToString()));
                                try
                                {
                                    if (strVal != "" && (strVal.ToLower().IndexOf(".jpg") > -1 || strVal.ToLower().IndexOf(".bmp") > -1 || strVal.ToLower().IndexOf(".gif") > -1 || strVal.ToLower().IndexOf(".png") > -1 || strVal.ToLower().IndexOf(".jpeg") > -1))
                                    {
                                        string strValnew = strVal.Replace(AppLogic.AppConfigs("Live_Server").ToString(), "");
                                        if (System.IO.File.Exists(Server.MapPath(strValnew)))
                                        {
                                            objImage.Text = "<br/><img src='" + strVal + "' border='0' width='100px' height='100px' />";
                                            ViewState["File_" + fl.ID.ToString()] = strVal.ToString();
                                            pnl.Controls.Add(objImage);
                                            ImageButton btnDel = new ImageButton();
                                            btnDel.ID = "btndel_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString();
                                            btnDel.OnClientClick = "return deletefile('fl_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString() + "','pnl_" + tab1.ToString() + "','" + btnDel.ID.ToString() + "');";
                                            btnDel.ImageUrl = "images/delete.gif";
                                            pnl.Controls.Add(btnDel);
                                        }
                                    }
                                    else
                                    {
                                        if (strVal != "")
                                        {
                                            string strValnew = strVal.Replace(AppLogic.AppConfigs("Live_Server").ToString(), "");
                                            if (System.IO.File.Exists(Server.MapPath(strValnew)))
                                            {
                                                objImage.Text = "<br/><a href='" + strVal + "' target='_blank' style='color:#000;text-decoration:underline;'>View File</a>";
                                                pnl.Controls.Add(objImage);
                                                ImageButton btnDel = new ImageButton();
                                                btnDel.ID = "btndel_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString();
                                                btnDel.OnClientClick = "return deletefile('fl_" + dsFeedField.Tables[0].Rows[j]["FieldID"].ToString() + "_" + dsFeedField.Tables[0].Rows[j]["FeedID"].ToString() + "','pnl_" + tab1.ToString() + "','" + btnDel.ID.ToString() + "');";
                                                btnDel.ImageUrl = "images/delete.gif";
                                                pnl.Controls.Add(btnDel);
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }

                                if (Convert.ToBoolean(dsFeedField.Tables[0].Rows[j]["isRequired"]))
                                {
                                    strSave += "if((document.getElementById('ContentPlaceHolder1_" + fl.ID + "')) && (document.getElementById('ContentPlaceHolder1_" + fl.ID + "').value == ''))";
                                    strSave += "{";
                                    strSave += " alert('Please select " + dsFeedField.Tables[0].Rows[j]["FieldName"].ToString() + "');document.getElementById('ContentPlaceHolder1_" + fl.ID + "').focus(); return false;";
                                    strSave += "}";
                                }
                            }
                            #endregion Asp Control Ends

                            Literal lttdClose1 = new Literal();
                            lttdClose1.Text = "</td>";
                            if (j + 1 == dsFeedField.Tables[0].Rows.Count)
                            {
                                if ((j + 1) % 2 != 0)
                                {
                                    lttdClose1.Text = "<td colspan='2'>&nbsp;</td>";
                                }
                            }

                            pnl.Controls.Add(lttdClose1);

                            if (k % 2 == 0)
                            {
                                jT = jT + 1;
                                Literal lttr = new Literal();

                                lttr.Text = "</tr><tr><td colspan='4' style='height:10px;'></td></tr>";

                                pnl.Controls.Add(lttr);
                                k = 0;
                            }
                        }

                        Literal ltSavebtn = new Literal();
                        ltSavebtn.Text = "<tr><td style=\"width:100%;text-align:center\" colspan=\"4\">&nbsp;</td></tr>";
                        ltSavebtn.Text += "<tr class=\"altrow\"><td colspan='4' style='width:100%;text-align:center'>";
                        pnl.Controls.Add(ltSavebtn);
                        strSave += " saveData(name,id); return false;}";
                        strSave += "</script>";
                        Response.Write(strSave.ToString());
                        ImageButton btnSave = new ImageButton();
                        btnSave.OnClientClick = "return Checkvalidation" + dsFeed.Tables[0].Rows[i]["FeedID"].ToString() + "('" + pnl.ID.ToString() + "'," + dsFeed.Tables[0].Rows[i]["FeedID"].ToString() + ");";
                        btnSave.ID = "btn_pnl_" + dsFeed.Tables[0].Rows[i]["FeedID"].ToString();
                        btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                        pnl.Controls.Add(btnSave);

                        Literal ltcancelbtn = new Literal();
                        ltcancelbtn.Text = "&nbsp;";
                        pnl.Controls.Add(ltcancelbtn);
                        ImageButton btnCancel = new ImageButton();
                        btnCancel.OnClientClick = "javascript:window.location.href='ListFeedProduct.aspx'; return false;";
                        btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                        pnl.Controls.Add(btnCancel);

                        Literal ltSavebtnc = new Literal();
                        ltSavebtnc.Text = "</td></tr>";
                        pnl.Controls.Add(ltSavebtnc);
                        pnl.DefaultButton = btnSave.ID.ToString();
                    }
                    Literal lt111 = new Literal();
                    lt111.Text = "</table>";

                    #endregion Main Loop Table End

                    ////lt111.Text += "</td>";
                    ////lt111.Text += "</tr>";

                    //<tr class="oddrow">
                    //<td>
                    //<table width="100%" border="0" cellpadding="0" cellspacing="0">
                    //<td>
                    //</td>
                    //<td style="width: 80%">
                    //<asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                    //OnClientClick="return CheckValidations();" OnClick="btnSave_Click" />
                    //<asp:ImageButton ID="btncancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                    //OnClick="btncancel_Click" />
                    //</td>
                    //</table>
                    //</td>
                    //</tr>

                    //lt111Cl.Text += "</table>";
                    //// table add-product Close

                    ////lt111.Text += "</td>";
                    ////lt111.Text += "</tr>";
                    ////lt111.Text += "</table>";

                    // close Table Main
                    pnl.Controls.Add(lt111);
                    if (Request.QueryString["FID"] != null && Request.QueryString["FID"].ToString() != dsFeed.Tables[0].Rows[i]["FeedID"].ToString())
                    {
                        pnl.Attributes.Add("style", "display:none");
                    }
                    objplaceHoder.Controls.Add(pnl);
                }

                Literal lt111Cl = new Literal();
                lt111Cl.Text += "</td>";
                lt111Cl.Text += "</tr>";
                lt111Cl.Text += "</table>";
                // table add-product Close

                lt111Cl.Text += "</td>";
                lt111Cl.Text += "</tr>";
                lt111Cl.Text += "</table>";

                lt111Cl.Text += "</td></tr></table></div>";
                //lt111Cl.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                //lt111Cl.Text += "<tbody><tr>";
                //lt111Cl.Text += "<td width=\"5\" valign=\"top\">";
                //lt111Cl.Text += "<img title=\"\" alt=\"\" src=\"../images/left_round_cor.gif\"></td>";
                //lt111Cl.Text += "<td height=\"5\" class=\"bottom_bg\">";
                //lt111Cl.Text += "</td>";
                //lt111Cl.Text += "<td align=\"right\" width=\"5\" valign=\"top\">";
                //lt111Cl.Text += "<img title=\"\" alt=\"\" src=\"../images/right_round_cor.gif\"></td>";
                //lt111Cl.Text += "</tr>";
                //lt111Cl.Text += "</tbody></table>";
                //lt111Cl.Text += "</div></div></div>";
                objplaceHoder.Controls.Add(lt111Cl);
            }
            else
            {
                Literal objDiv = new Literal();
                objDiv.Text = "<div style=\"vertical-align: middle; padding: 10px 21px 4px 81px; height: 18px; float: left; width: 93%;\"></div>";
                objDiv.Text += "<div class=\"content-row1\" style=\"height: 85px;\">";

                DropDownList objStore = new DropDownList();
                objStore.ID = "ddlStore";
                objStore.AutoPostBack = true;
                objStore.CssClass = "product-type";
                objStore.Width = Unit.Pixel(200);
                objStore.SelectedIndexChanged += new EventHandler(objStore_SelectedIndexChanged);
                BindStore(objStore);

                Literal objDiv1 = new Literal();
                objDiv1.Text += "<div class=\"content-row2\">";

                objDiv1.Text += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
                objDiv1.Text += "<tbody><tr>";
                objDiv1.Text += "<td valign=\"top\" align=\"left\" height=\"5\">";
                objDiv1.Text += "<img width=\"1\" height=\"5\" src=\"/App_Themes/" + Page.Theme + "/images/spacer.gif\">";
                objDiv1.Text += "</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<td valign=\"top\" align=\"left\" height=\"5\">";
                objDiv1.Text += "<img width=\"1\" height=\"5\" src=\"/App_Themes/" + Page.Theme + "/images/spacer.gif\">";
                objDiv1.Text += "</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr>";

                objDiv1.Text += "<td class=\"border-td\">";
                objDiv1.Text = "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" class=\"content-table\">";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<td class=\"border-td-sub\">";
                // table add-product Start
                objDiv1.Text += "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"add-product\">";
                objDiv1.Text += "<tr>";
                objDiv1.Text += "<th>";
                objDiv1.Text += "<div class=\"main-title-left\" style=\"width: 98% !important\">";
                //objDiv1.Text += "<img class=\"img-left\" alt=\"Add Feed Master\" src=\"/App_Themes/" + Page.Theme + "/Images/admin-list-icon.png\">";
                objDiv1.Text += "<h2><span style=\"line-height: 25px;\"></span>";

                objDiv1.Text += "<span style=\"float: right;\">Store : ";
                objplaceHoder.Controls.Add(objDiv1);
                objplaceHoder.Controls.Add(objStore);
                objDiv1 = new Literal();
                objDiv1.Text = "</span>";
                objDiv1.Text += "</h2>";
                objDiv1.Text += "</div>";
                objDiv1.Text += "</th>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr class=\"altrow\">";
                objDiv1.Text += "<td align=\"right\" style=\"font-size:12px;\"><span class=\"star\">*</span>Required Field</td>";
                objDiv1.Text += "</tr>";
                objDiv1.Text += "<tr class=\"even-row\">";
                objDiv1.Text += "<td>";



                Literal lt111Cl = new Literal();
                lt111Cl.Text += "</td>";
                lt111Cl.Text += "</tr>";
                lt111Cl.Text += "</table>";
                // table add-product Close

                lt111Cl.Text += "</td>";
                lt111Cl.Text += "</tr>";
                lt111Cl.Text += "</table>";
                lt111Cl.Text += "</td></tr></table></div>";

                //objDiv.Text += "<div class=\"static_display\">";
                //objDiv.Text += "<div class=\"content_box\">";
                //objDiv.Text += "<div class=\"title\">";

                //objDiv.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
                //objDiv.Text += "<tbody><tr>";
                //objDiv.Text += "<td align=\"left\" width=\"3\" valign=\"top\">";
                //objDiv.Text += "<img class=\"img_left\" title=\"\" alt=\"\" src=\"../images/left_round_img.gif\" style=\"float:right\"></td>";
                //objDiv.Text += "<td align=\"left\" valign=\"top\" class=\"order_bg\">";
                //objDiv.Text += "<div class=\"heading_1\">";
                //objDiv.Text += "<span id=\"ContentPlaceHolder1_lblTitle\"></span>";
                //objDiv.Text += "</div>";

                //objDiv.Text += "<div style='float:right;margin-top:0px;'>Store: ";
                //objplaceHoder.Controls.Add(objDiv);

                //DropDownList objStore = new DropDownList();
                //objStore.ID = "ddlStore";

                //objStore.AutoPostBack = true;
                //objStore.CssClass = "product-type";
                //objStore.Width = Unit.Pixel(200);
                //objStore.SelectedIndexChanged += new EventHandler(objStore_SelectedIndexChanged);
                //BindStore(objStore);
                //objplaceHoder.Controls.Add(objStore);

                //Literal objDiv1 = new Literal();
                //objDiv1.Text = "</div>";
                //objDiv1.Text += "</td>";
                //objDiv1.Text += "<td align=\"left\" width=\"6\" valign=\"top\">";
                //objDiv1.Text += " <img title=\"\" alt=\"\" src=\"../images/round_right_cor.gif\"></td>";
                //objDiv1.Text += "</tr>";
                //objDiv1.Text += "</tbody></table>";
                //objDiv1.Text += "</div></div></div>";

                objDiv1.Text += "</div></div>";
                objplaceHoder.Controls.Add(objDiv1);
            }
        }


        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        void objStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContentPlaceHolder objplaceHoder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            string str = "";
            DropDownList ddlStore = (DropDownList)objplaceHoder.FindControl("ddlStore");

            FeedComponent objFeed = new FeedComponent();
            DataSet dsFeed = new DataSet();

            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }


            if (ddlStore.SelectedIndex != -1)
            {
                dsFeed = objFeed.GetFeedMasterByStore(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            }
            else
            {
                dsFeed = objFeed.GetFeedMasterByStore(1);
            }
            if (dsFeed != null && dsFeed.Tables.Count > 0 && dsFeed.Tables[0].Rows.Count > 0)
            {
                Response.Redirect("GenerateExportpage.aspx?Storeid=" + ddlStore.SelectedValue.ToString() + "&ID=" + Request.QueryString["ID"].ToString() + "&FID=" + dsFeed.Tables[0].Rows[0]["FEEDID"].ToString());
            }
            else
            {
                Response.Redirect("GenerateExportpage.aspx?Storeid=" + ddlStore.SelectedValue.ToString() + "&ID=" + Request.QueryString["ID"].ToString() + "&FID=" + Request.QueryString["FID"].ToString());
            }
        }


        /// <summary>
        /// Sets the Category
        /// </summary>
        /// <param name="ddlCategory">DropDownList ddlCategory</param>
        /// <param name="rootCtegory">String rootCtegory</param>
        /// <param name="ddlStore">DropDownList ddlStore</param>
        /// <param name="TitleName">String TitleName</param>
        public void SetCategory(DropDownList ddlCategory, string rootCtegory, DropDownList ddlStore, string TitleName)
        {
            ddlCategory.Items.Clear();
            int count = 1;
            FeedComponent ObjCategory = new FeedComponent();
            ListItem LT2 = new ListItem();
            DataRow[] drCatagories = null;
            DataSet dsCategory = new DataSet();
            string strName = "";
            string strNm = "";
            dsCategory = ObjCategory.GetCategoryByStoreId(Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            if (Convert.ToInt32(rootCtegory) > 0)
            {
                drCatagories = dsCategory.Tables[0].Select("Categoryid=" + rootCtegory + " and StoreID=1");
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = selDR["Name"].ToString();
                    strNm = selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    count++;
                }
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + rootCtegory + " and StoreID=1");
            }
            else
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + rootCtegory + " and StoreID=1");
            }
            if (dsCategory != null && drCatagories.Length > 0)
            {
                if (Convert.ToInt32(rootCtegory) > 0)
                {
                    foreach (DataRow selDR in drCatagories)
                    {
                        LT2 = new ListItem();
                        strName = strNm + " > " + selDR["Name"].ToString();
                        LT2.Text = strName;
                        LT2.Value = selDR["CategoryID"].ToString();
                        ddlCategory.Items.Add(LT2);
                        SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count, ddlCategory, dsCategory, strName);
                    }
                }
                else
                {
                    foreach (DataRow selDR in drCatagories)
                    {
                        LT2 = new ListItem();
                        LT2.Text = selDR["Name"].ToString();
                        strName = selDR["Name"].ToString();
                        LT2.Value = selDR["CategoryID"].ToString();
                        ddlCategory.Items.Add(LT2);
                        SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count, ddlCategory, dsCategory, strName);
                    }
                }
            }
            ListItem LT = new ListItem();
            LT.Text = "Select " + TitleName + "";
            LT.Value = "0";
            ddlCategory.Items.Insert(0, LT);
        }

        /// <summary>
        /// Sets the Child Category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        /// <param name="ddlCategory">DropDownList ddlCategory</param>
        /// <param name="dsCategory">DataSet dsCategory</param>
        /// <param name="strName">String strName</param>
        public void SetChildCategory(int ID, int Number, DropDownList ddlCategory, DataSet dsCategory, string strName)
        {
            int count = Number;
            string st = "...";
            string strnameT = "";
            strnameT = strName;
            for (int i = 0; i < count; i++)
            {
                st += st;
            }
            DataRow[] drCatagories = null;

            drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString() + " and StoreID=1");
            ListItem LT2;
            int innercount = 0;
            if (drCatagories.Length > 0)
            {
                innercount++;
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    strName = strnameT + " > " + selDR["Name"].ToString();

                    LT2.Text = strName;//st + "|" + (count + 1) + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), innercount + Number, ddlCategory, dsCategory, strnameT + " > " + selDR["Name"].ToString());
                }
            }
        }

        /// <summary>
        ///  Finish Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder objplaceHoder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            string str = "";
            Panel pnls = (Panel)objplaceHoder.FindControl(hdnnew.Value.ToString());
            string update = "";
            string insertintofield = "";
            string insertintovalue = "";
            bool flag = false;
            string lblname = "";
            string updateSet = "";
            int fieldid = 0;
            FeedComponent objFeed = new FeedComponent();
            System.Text.RegularExpressions.Regex objRegExp = new System.Text.RegularExpressions.Regex("<(.|\n)+?>");
            string TempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");


            foreach (Control ctrl in pnls.Controls)
            {
                if (ctrl.ID != null && lblname == "")
                {
                    if (ctrl.GetType() == typeof(Label))
                    {
                        if (ctrl.ID.ToString().ToLower().IndexOf("lbl_") > -1)
                        {
                            string[] strFieldId = ctrl.ID.ToString().Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Label lbl = (Label)ctrl;
                            fieldid = Convert.ToInt32(strFieldId[1].ToString());
                            if (objFeed.GetFielValuesByProductID(Convert.ToInt32(Request.QueryString["ID"].ToString()), Convert.ToInt32(strFieldId[1].ToString()), Convert.ToInt32(hdnfeedid.Value.ToString())) > 0)
                            {
                                flag = false;

                            }
                            else
                            {

                                flag = true;
                            }
                            lblname = ctrl.ID.ToString();
                        }
                    }
                }
                if (ctrl.GetType() != typeof(Label))
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        TextBox txtctrl = (TextBox)ctrl;
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + objRegExp.Replace(txtctrl.Text.ToString(), string.Empty).Replace("'", "''") + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + objRegExp.Replace(txtctrl.Text.ToString(), string.Empty).Replace("'", "''") + "',";
                        }
                    }
                    else if (ctrl.GetType() == typeof(DropDownList))
                    {
                        DropDownList ddlctrl = (DropDownList)ctrl;
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + objRegExp.Replace(ddlctrl.SelectedItem.Text.ToString(), string.Empty).Replace("'", "''") + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + objRegExp.Replace(ddlctrl.SelectedItem.Text.ToString(), string.Empty).Replace("'", "''") + "',";
                        }
                    }
                    else if (ctrl.GetType() == typeof(CheckBox))
                    {
                        CheckBox chkctrl = (CheckBox)ctrl;
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + Convert.ToString(chkctrl.Checked) + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + Convert.ToString(chkctrl.Checked) + "',";
                        }
                    }
                    else if (ctrl.GetType() == typeof(RadioButton))
                    {
                        RadioButton rdoctrl = (RadioButton)ctrl;
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + Convert.ToString(rdoctrl.Checked) + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + Convert.ToString(rdoctrl.Checked) + "',";
                        }
                    }
                    else if (ctrl.GetType() == typeof(CheckBoxList))
                    {
                        CheckBoxList chklistctrl = (CheckBoxList)ctrl;
                        string strcheck = "";

                        for (int k = 0; k < chklistctrl.Items.Count; k++)
                        {
                            if (chklistctrl.Items[k].Selected == true)
                            {
                                strcheck += objRegExp.Replace(chklistctrl.Items[k].Text.ToString(), string.Empty).ToString() + ",";
                            }
                        }
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + Convert.ToString(strcheck) + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + Convert.ToString(strcheck) + "',";
                        }
                    }
                    //else if (ctrl.GetType() == typeof(eWorld.UI.CalendarPopup))
                    //{
                    //    eWorld.UI.CalendarPopup calctrl = (eWorld.UI.CalendarPopup)ctrl;
                    //    string strcheck = "";

                    //    if (flag == false)
                    //    {
                    //        updateSet += "[FieldValue]='" + Convert.ToString(calctrl.PostedDate.ToString()) + "',";
                    //    }
                    //    else
                    //    {
                    //        insertintofield += "[FieldValue],";
                    //        insertintovalue += "'" + Convert.ToString(calctrl.PostedDate.ToString()) + "',";
                    //    }
                    //}
                    else if (ctrl.GetType() == typeof(RadioButtonList))
                    {
                        RadioButtonList rdolistctrl = (RadioButtonList)ctrl;
                        string strcheck = "";

                        for (int k = 0; k < rdolistctrl.Items.Count; k++)
                        {
                            if (rdolistctrl.Items[k].Selected == true)
                            {
                                strcheck += objRegExp.Replace(rdolistctrl.Items[k].Text.ToString(), string.Empty).ToString() + ",";
                            }
                        }
                        if (flag == false)
                        {
                            updateSet += "[FieldValue]='" + Convert.ToString(strcheck) + "',";
                        }
                        else
                        {
                            insertintofield += "[FieldValue],";
                            insertintovalue += "'" + Convert.ToString(strcheck) + "',";
                        }
                    }
                    else if (ctrl.GetType() == typeof(FileUpload))
                    {
                        FileUpload filectrl = (FileUpload)ctrl;
                        if (ViewState["File_" + ctrl.ID.ToString()] != null && ViewState["File_" + ctrl.ID.ToString().ToString()].ToString().ToLower().IndexOf("/large/") <= -1)
                        {
                            System.IO.FileInfo flinfo = new System.IO.FileInfo(ViewState["File_" + ctrl.ID.ToString()].ToString());
                            System.IO.File.Copy(Server.MapPath(TempPath + ViewState["File_" + ctrl.ID.ToString()].ToString()), Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/") + Request.QueryString["ID"].ToString() + "_" + hdnfeedid.Value.ToString() + flinfo.Extension.ToString()), true);
                            if (flag == false)
                            {
                                updateSet += "[FieldValue]='" + AppLogic.AppConfigs("Live_Server") + "/" + string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/") + Request.QueryString["ID"].ToString() + "_" + hdnfeedid.Value.ToString() + flinfo.Extension.ToString() + "',";
                            }
                            else
                            {
                                insertintofield += "[FieldValue],";
                                insertintovalue += "'" + AppLogic.AppConfigs("Live_Server") + "/" + string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/") + Request.QueryString["ID"].ToString() + "_" + hdnfeedid.Value.ToString() + flinfo.Extension.ToString() + "',";
                            }
                            try
                            {
                                System.IO.File.Delete(Server.MapPath(TempPath + ViewState["File_" + ctrl.ID.ToString()].ToString()));
                            }
                            catch
                            {
                            }

                        }
                        else
                        {
                            if (flag == false)
                            {
                                if (ViewState["File_" + ctrl.ID.ToString()] != null)
                                {
                                    updateSet += "[FieldValue]='" + ViewState["File_" + ctrl.ID.ToString().ToString()].ToString() + "',";
                                }
                                else
                                {
                                    updateSet += "[FieldValue]='',";
                                }
                            }
                            else
                            {

                                insertintofield += "[FieldValue],";
                                if (ViewState["File_" + ctrl.ID.ToString()] != null)
                                {
                                    insertintovalue += "'" + ViewState["File_" + ctrl.ID.ToString()].ToString() + "',";
                                }
                                else
                                {
                                    insertintovalue += "'',";
                                }
                            }
                        }
                    }
                    if (updateSet != "" && updateSet.IndexOf(",") > -1)
                    {
                        updateSet = updateSet.Substring(0, updateSet.Length - 1);
                    }
                    if (updateSet != "")
                    {
                        update = " UPDATE FeedFieldValues_" + hdnfeedid.Value.ToString() + " SET  " + updateSet + " WHERE ProductID=" + Request.QueryString["ID"].ToString() + " AND FieldID=" + fieldid.ToString() + "";
                        CommonComponent.ExecuteCommonData(update);
                        DataSet objBase = new DataSet();

                        objBase = objFeed.GetFeeDBybase(Convert.ToInt32(hdnfeedid.Value.ToString()));
                        if (objBase != null && objBase.Tables.Count > 0 && objBase.Tables[0].Rows.Count > 0)
                        {
                            DataSet objRelatdDeed = new DataSet();
                            objRelatdDeed = objFeed.GetFeedlist(Convert.ToInt32(hdnfeedid.Value.ToString()));
                            if (objRelatdDeed != null && objRelatdDeed.Tables.Count > 0 && objRelatdDeed.Tables[0].Rows.Count > 0)
                            {
                                for (int iFeed = 0; iFeed < objRelatdDeed.Tables[0].Rows.Count; iFeed++)
                                {
                                    objFeed.GetFeedRelatedField(Convert.ToInt32(fieldid.ToString()), Convert.ToInt32(hdnfeedid.Value.ToString()), Convert.ToInt32(objRelatdDeed.Tables[0].Rows[iFeed]["RelatedFeedID"].ToString()));
                                }
                            }
                        }

                        updateSet = "";
                        fieldid = 0;
                        flag = false;
                        lblname = "";
                    }
                    else
                    {
                        if (insertintofield != "" && insertintovalue != "")
                        {
                            insertintofield += "ProductId,FieldID";

                        }
                        if (insertintovalue != "" && insertintofield != "")
                        {
                            insertintovalue += Request.QueryString["ID"].ToString() + "," + fieldid.ToString() + "";

                        }
                        if (insertintovalue != "" && insertintofield != "")
                        {
                            CommonComponent.ExecuteCommonData("INSERT INTO FeedFieldValues_" + hdnfeedid.Value.ToString() + "(" + insertintofield + ") VALUES (" + insertintovalue + ")");
                            DataSet objBase = new DataSet();

                            objBase = objFeed.GetFeeDBybase(Convert.ToInt32(hdnfeedid.Value.ToString()));
                            if (objBase != null && objBase.Tables.Count > 0 && objBase.Tables[0].Rows.Count > 0)
                            {
                                DataSet objRelatdDeed = new DataSet();
                                objRelatdDeed = objFeed.GetFeedlist(Convert.ToInt32(hdnfeedid.Value.ToString()));
                                if (objRelatdDeed != null && objRelatdDeed.Tables.Count > 0 && objRelatdDeed.Tables[0].Rows.Count > 0)
                                {
                                    for (int iFeed = 0; iFeed < objRelatdDeed.Tables[0].Rows.Count; iFeed++)
                                    {
                                        objFeed.GetFeedRelatedField(Convert.ToInt32(fieldid.ToString()), Convert.ToInt32(hdnfeedid.Value.ToString()), Convert.ToInt32(objRelatdDeed.Tables[0].Rows[iFeed]["RelatedFeedID"].ToString()));
                                    }
                                }
                            }
                            flag = false;
                            update = "";
                            insertintofield = "";
                            insertintovalue = "";
                            fieldid = 0;
                            lblname = "";
                        }

                    }
                }
            }
            if (hdntab.Value.ToString() != "")
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "alert('Data Saved Successfully');", true);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "for(i=0;i < " + hdntotal.Value.ToString() + "; i++){var j = i + 1;var name ='ContentPlaceHolder1_pnl_'+j;if(" + hdntab.Value.ToString() + " == j){document.getElementById(name).style.display='block';document.getElementById('idtitle').innerHTML=document.getElementById('ContentPlaceHolder1_hdntitle').value;}else{document.getElementById(name).style.display='none';}}", true);
            }
        }

        /// <summary>
        ///  Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder objplaceHoder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            Panel pnls = (Panel)objplaceHoder.FindControl(hdnnew.Value.ToString());
            foreach (Control ctrl in pnls.Controls)
            {
                if ((ctrl.GetType() == typeof(FileUpload)) && (ctrl.ID.ToLower() == hdnfile.Value.ToString().ToLower()))
                {
                    FileUpload fl = (FileUpload)ctrl;
                    if (fl.HasFile)
                    {
                        fl.SaveAs(Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/") + fl.FileName.ToString()));
                        ViewState["File_" + hdnfile.Value.ToString()] = fl.FileName.ToString();
                    }
                }
            }
            if (hdntab.Value.ToString() != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "for(i=0;i < " + hdntotal.Value.ToString() + "; i++){var j = i + 1;var name ='ContentPlaceHolder1_pnl_'+j;if(" + hdntab.Value.ToString() + " == j){document.getElementById(name).style.display='block';document.getElementById('idtitle').innerHTML=document.getElementById('ContentPlaceHolder1_hdntitle').value;}else{document.getElementById(name).style.display='none';}}", true);
            }
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (hdnfile.Value.ToString() != "")
            {
                try
                {
                    if (ViewState["File_" + hdnfile.Value.ToString()] != null)
                    {
                        string strFile = ViewState["File_" + hdnfile.Value.ToString()].ToString().Replace(AppLogic.AppConfigs("Live_Server").ToString(), "");
                        if (System.IO.File.Exists(Server.MapPath(strFile)))
                        {
                            System.IO.File.Delete(Server.MapPath(strFile));
                            ViewState["File_" + hdnfile.Value.ToString()] = null;
                            ContentPlaceHolder objplaceHoder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                            Panel pnls = (Panel)objplaceHoder.FindControl(hdnnew.Value.ToString());
                            ImageButton btndl = (ImageButton)pnls.FindControl(deletehdn.Value.ToString());
                            if (btndl != null)
                            {
                                btndl.Visible = false;
                            }
                            FeedComponent objfeed = new FeedComponent();
                            string[] str = hdnfile.Value.ToString().Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string strupdate = "UPDATE FeedFieldValues_" + hdnfeedid.Value.ToString() + " SET FieldValue='' WHERE FieldID=" + str[1].ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + "";
                            CommonComponent.ExecuteCommonData(strupdate);
                            if (hdntab.Value.ToString() != "")
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('File Deleted Successfully.');for(i=0;i < " + hdntotal.Value.ToString() + "; i++){var j = i + 1;var name ='ContentPlaceHolder1_pnl_'+j;if(" + hdntab.Value.ToString() + " == j){document.getElementById(name).style.display='block';}else{document.getElementById(name).style.display='none';}}", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('File Deleted Successfully.');", true);
                            }
                        }

                    }
                }
                catch
                {
                }
            }
        }
    }
}