using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web
{
    public partial class RMA_ProductSkus : BasePage
    {
        #region General Variable
        tb_Product objProduct = null;
        tb_Category objCategory = null;
        DataSet dsCategory = new DataSet();
        DataSet dsCat = new DataSet();
        DataSet ds = new DataSet();
        String SelectedSKUs = String.Empty;
        public String strScriptVar = String.Empty;
        static int pageSize = 0;
        static int pageNo = 1;
        int TotCat = 0;
        int TotPro = 0;
        private int StoreID = 1;

        static DataView dsGlobal = null;
        public string[] strspt;
        public string clientid = string.Empty;
        StringBuilder Table = null;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["StoreID"] != null && Request.QueryString["clientid"].ToString() != null && !string.IsNullOrEmpty(Request.QueryString["clientid"].ToString()))
            {
                StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
                btnSearch.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
                btnSelectItem.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";
                ImgColse2.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";

                clientid = Request.QueryString["clientid"].ToString();

                if (!Page.IsPostBack)
                {
                    HttpCookie myCookie = new HttpCookie("prskus");
                    myCookie = Request.Cookies["prskus"];
                    if (myCookie != null)
                    {
                        string skuss = myCookie.Value.ToString();
                        if (!string.IsNullOrEmpty(skuss))
                        {
                            ViewState["SelectedSKUs"] = skuss;
                            strspt = skuss.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        }
                        else
                            ViewState["SelectedSKUs"] = "";
                    }
                    else
                    {
                        lbMsg.Text = "Cookies is not Enable in this computer please Enable cookies to work properly.";
                        ViewState["SelectedSKUs"] = "";
                    }
                }

            }
            else
                ViewState["SelectedSKUs"] = "";
            if (!IsPostBack)
            {
                HdnCustID.Value = Request.QueryString["CustID"].ToString();
                BindData();
                CartGridDataBind();
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(clientid))
                {
                    //BindCart(Convert.ToInt32(HdnCustID.Value));
                    string skus = ViewState["SelectedSKUs"].ToString();
                    if (skus.Length > 1)
                        skus = skus.TrimEnd(",".ToCharArray());
                    string cid = Request.QueryString["clientid"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Request.QueryString["lblID"].ToString()))
                    {
                        if (Request.QueryString["marrypro"] != null)
                        {
                            int ii = 0;
                            foreach (GridViewRow gr in grdSelected.Rows)
                            {
                                string strsku = (gr.FindControl("lblSKU") as Label).Text.Trim();
                                if (!string.IsNullOrEmpty(strsku))
                                    ii = ii + 1;
                            }

                            if (ii > 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "jAlert('You can select only one product for marry product.','Required Information')", true);
                                return;
                            }

                        }

                        skus = HdnProductSKu.Value.ToString();
                        string skus1 = skus;
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('hfsuto').value='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblSubTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + skus + "';window.opener.document.getElementById('hfsuto').innerHTML = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblTotal').innerHTML=parseFloat(parseFloat(window.opener.document.getElementById('lblSubTotal').innerHTML)+parseFloat(window.opener.document.getElementById('TxtShippingCost').value)+parseFloat(window.opener.document.getElementById('TxtTax').value)-parseFloat(window.opener.document.getElementById('TxtDiscount').value)).toFixed(2);window.opener.document.getElementById('hfTotal').value=window.opener.document.getElementById('lblTotal').innerHTML;window.opener.document.getElementById('btnUpdate').click();window.close();", true);
                        Page.ClientScript.RegisterClientScriptBlock(pageNo.GetType(), "msg", "<script>window.close();</script>", true);
                    }
                    else
                    {
                        if (Request.QueryString["marrypro"] != null)
                        {
                            int ii = 0;
                            foreach (GridViewRow gr in grdSelected.Rows)
                            {
                                string strsku = (gr.FindControl("lblSKU") as Label).Text.Trim();
                                if (!string.IsNullOrEmpty(strsku))
                                    ii = ii + 1;
                            }
                            if (ii > 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "jAlert('You can select only one product for marry product.','Required Information')", true);
                                return;
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblSubTotal').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('btnUpdate').click();window.close();", true);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Category Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Binds the data of RMA Product SKU
        /// </summary>
        public void BindData()
        {
            string SKUs = txtSearch.Text.Replace("'", "''").TrimEnd(",".ToCharArray()).TrimStart(",".ToCharArray());
            DataSet ds = new DataSet();
            string sql = "SElect [ProductID],[Name],[SKU],[Price],ISNULL(SalePrice,0) AS SalePrice from tb_Product Where StoreId=" + StoreID + " and Sku like '%" + SKUs.ToString().Trim() + "%'";
            ds = CommonComponent.GetCommonDataSet(sql.ToString());
            gvListProducts.DataSource = null;
            gvListProducts.DataBind();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gvListProducts.DataSource = ds.Tables[0];
                    gvListProducts.DataBind();
                    btnSelectItem.Visible = true;
                    cleartdid.Visible = true;
                }
                else
                {
                    gvListProducts.DataSource = null;
                    gvListProducts.DataBind();
                    btnSelectItem.Visible = false;
                    cleartdid.Visible = false;
                }

                //if (ds.Tables[0].Rows.Count > 0)
                //{ trBottom.Visible = true; cleartdid.Visible = true; trProduct.Visible = true; }
                //else
                //{ trBottom.Visible = false; cleartdid.Visible = false; trProduct.Visible = false; }

                foundPro.Text = ds.Tables[0].Rows.Count.ToString();
                // DivBottom.Visible = true;
            }
            else
            {
                foundPro.Text = "0";
                gvListProducts.DataSource = null;
                gvListProducts.DataBind();
                btnSelectItem.Visible = false;
                btnSelectItem.Visible = false;
                //DivBottom.Visible = false;
                cleartdid.Visible = false;
            }

        }

        /// <summary>
        ///  List Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvListProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListProducts.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Gets the URL
        /// </summary>
        /// <param name="skus">string skus</param>
        /// <returns>Returns the URL as a string</returns>
        private string GetURL(string skus)
        {
            try
            {
                string pagename = "editEcomProduct.aspx";
                if (Request.QueryString["StoreID"].ToString().Trim() == "6")
                    pagename = "EditYahooProduct.aspx";
                else if (Request.QueryString["StoreID"].ToString().Trim() == "1")
                    pagename = "editEcomProduct.aspx";

                skus = "'" + skus.Replace(",", "','") + "'";

                DataSet ds = CommonComponent.GetCommonDataSet("select SKU,'<a class=\"skulinks\" href=\"" + pagename + "?Mode=edit&ID='+Convert(varchar(20),ProductID)+ '&StoreID=' + Convert(varchar(20),StoreID) + '\">' + SKU + '</a>' + ', ' as SKUURL from tb_Product where SKU in (" + skus + ") " + ((Request.QueryString["StoreID"] != null) ? (" and Storeid=" + Request.QueryString["StoreID"].ToString()) : "") + "  and StoreID is not null");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    skus = string.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        skus += dr["SKUURL"].ToString();
                }
            }
            catch { }
            return skus.Trim().TrimEnd(",".ToCharArray());

        }

        /// <summary>
        /// Manufacture Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlManufacture_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// Gets the Variant by Product ID
        /// </summary>
        /// <param name="productID">int productID</param>
        /// <param name="objDiv">HtmlContainerControl objDiv</param>
        /// <param name="txtQty">TextBox txtQty</param>
        /// <param name="imgButton">ImageButton imgButton</param>
        /// <param name="price">Decimal price</param>
        /// <param name="hdnst">HtmlInputHidden hdnst</param>
        /// <param name="lblvariantprice">Label lblvariantprice</param>
        /// <param name="RowIndex">int RowIndex</param>
        /// <returns>true if Exists, false otherwise</returns>
        private bool GetVarinatByProductID(int productID, System.Web.UI.HtmlControls.HtmlContainerControl objDiv, TextBox txtQty, ImageButton imgButton, decimal price, System.Web.UI.HtmlControls.HtmlInputHidden hdnst, Label lblvariantprice, Int32 RowIndex)
        {
            DataSet dsVariant = new DataSet();
            dsVariant = ProductComponent.GetProductVariantByproductID(productID);
            objDiv.Controls.Clear();
            if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
            {
                string strVName = "";
                Literal ltrText;
                ltrText = new Literal();
                strScriptVar += "function addvariant_" + productID.ToString() + "(price,divid,hdnst,lblprice){";
                strScriptVar += "if(document.getElementById('" + txtQty.ClientID.ToString() + "') != null && (document.getElementById('" + txtQty.ClientID.ToString() + "').value=='' || document.getElementById('" + txtQty.ClientID.ToString() + "').value=='0'))" + System.Environment.NewLine;
                strScriptVar += "{" + System.Environment.NewLine;
                strScriptVar += "jAlert('Please Enter valid Quantity','Required Information','" + txtQty.ClientID.ToString() + "');" + System.Environment.NewLine;
                strScriptVar += "return false;" + System.Environment.NewLine;
                strScriptVar += "}" + System.Environment.NewLine;

                imgButton.OnClientClick = "return  addvariant_" + productID.ToString() + "('" + price + "','" + objDiv.ClientID.ToString() + "','" + hdnst.ClientID + "','" + lblvariantprice.ClientID.ToString() + "');";
                ltrText.Text = "<table cellpadding='0' cellspacing='4' border='0'>";
                objDiv.Controls.Add(ltrText);
                // Bind Variants with Engraving font
                dsVariant = ProductComponent.GetProductVariantByproductIDandEngraving(productID, 2);
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    {
                        ltrText = new Literal();
                        Int32 MaxLenEngraving = Convert.ToInt32(dsVariant.Tables[0].Rows[i]["VariantValue"].ToString());
                        ltrText.ID = "ltr_" + productID.ToString() + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                        ltrText.Text = "<tr><td><span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span> :</td><td>";
                        ltrText.Text += "<input type=\"text\" class=\"order-textfield\" style=\"width:105px;\" name=\"txt1_kau_" + productID + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" id=\"txt1_kau_" + productID + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" maxlength=\"" + MaxLenEngraving + "\" style=\"width: 202px; text-indent: 2px;\" /> &nbsp;";
                        objDiv.Controls.Add(ltrText);

                        strScriptVar += "if(document.getElementById('txt1_kau_" + productID + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null &&  document.getElementById('txt1_kau_" + productID + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + @"').value.replace(/^\s+|\s+$/g, '') == '')" + System.Environment.NewLine;
                        strScriptVar += "{" + System.Environment.NewLine;
                        strScriptVar += "jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','txt1_kau_" + productID + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                        strScriptVar += "return false;" + System.Environment.NewLine;
                        strScriptVar += "}" + System.Environment.NewLine;

                        DataSet dsfont = new DataSet();
                        dsfont = CommonComponent.GetCommonDataSet("Select * from tb_FontTable Where Status=1 Order by FontName");
                        if (dsfont != null && dsfont.Tables.Count > 0 && dsfont.Tables[0].Rows.Count > 0)
                        {
                            DropDownList rrselect_kau = new DropDownList();
                            rrselect_kau.ID = "rrselect_kau_" + productID.ToString() + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                            rrselect_kau.AutoPostBack = false;
                            rrselect_kau.CssClass = "product-type";
                            rrselect_kau.DataSource = dsfont;
                            rrselect_kau.DataTextField = "FontName";
                            rrselect_kau.DataValueField = "FontName";
                            rrselect_kau.Attributes.Add("style", "width: 160px; margin-right: 13px;");
                            rrselect_kau.DataBind();
                            rrselect_kau.Items.Insert(0, new ListItem("Choose Engraving Fonts", "0"));

                            strScriptVar += "if(document.getElementById('gvListProducts_" + rrselect_kau.ClientID.ToString() + "_" + RowIndex.ToString() + "') != null &&  document.getElementById('gvListProducts_" + rrselect_kau.ClientID.ToString() + "_" + RowIndex.ToString() + "').selectedIndex==0)" + System.Environment.NewLine;
                            strScriptVar += "{" + System.Environment.NewLine;
                            strScriptVar += "jAlert('Please select Choose Engraving Fonts','Required information','gvListProducts_" + rrselect_kau.ClientID.ToString() + "_" + RowIndex.ToString() + "');" + System.Environment.NewLine;
                            strScriptVar += "return false;" + System.Environment.NewLine;
                            strScriptVar += "}" + System.Environment.NewLine;

                            objDiv.Controls.Add(rrselect_kau);
                        }
                        ltrText = new Literal();
                        ltrText.Text = "</td></tr>";
                        objDiv.Controls.Add(ltrText);
                        ltrText.Text += "</td></tr>";
                    }
                }
                // Bind Other Variants
                dsVariant = ProductComponent.GetProductVariantByproductIDandEngraving(productID, 1);
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    {
                        DataTable dtVariant = new DataTable();
                        dtVariant = dsVariant.Tables[0].Clone();
                        if (strVName != dsVariant.Tables[0].Rows[i]["VariantName"].ToString())
                        {
                            DataRow[] dr = dsVariant.Tables[0].Select("VariantName ='" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", "''") + "'");
                            foreach (DataRow dr1 in dr)
                            {
                                object[] drAll = dr1.ItemArray;
                                dtVariant.Rows.Add(drAll);
                            }

                            ltrText = new Literal();
                            ltrText.ID = "ltr_" + productID.ToString() + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                            ltrText.Text = "<tr><td><span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span> :</td><td>";
                            objDiv.Controls.Add(ltrText);

                            DropDownList ddlvaraint = new DropDownList();
                            ddlvaraint.ID = "ddlVariant_" + productID.ToString() + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                            ddlvaraint.AutoPostBack = false;
                            ddlvaraint.CssClass = "product-type";
                            ddlvaraint.DataSource = dtVariant;
                            ddlvaraint.DataTextField = "priceVariantValue";
                            ddlvaraint.DataValueField = "VariantValueID";
                            ddlvaraint.Attributes.Add("onchange", "return changeprice('" + objDiv.ClientID.ToString() + "','" + lblvariantprice.ClientID.ToString() + "','" + hdnst.ClientID.ToString() + "','" + price.ToString() + "','" + txtQty.ClientID.ToString() + "');");

                            strScriptVar += "if(document.getElementById('gvListProducts_" + ddlvaraint.ClientID.ToString() + "_" + RowIndex.ToString() + "') != null &&  document.getElementById('gvListProducts_" + ddlvaraint.ClientID.ToString() + "_" + RowIndex.ToString() + "').selectedIndex==0)" + System.Environment.NewLine;
                            strScriptVar += "{" + System.Environment.NewLine;
                            //strScriptVar += "jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required Information','ddlVariant_" + productID.ToString() + "_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                            strScriptVar += "jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','gvListProducts_" + ddlvaraint.ClientID.ToString() + "_" + RowIndex.ToString() + "');" + System.Environment.NewLine;
                            strScriptVar += "return false;" + System.Environment.NewLine;
                            strScriptVar += "}" + System.Environment.NewLine;

                            ddlvaraint.DataBind();
                            ddlvaraint.Items.Insert(0, new ListItem("Select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString(), "0"));
                            if (ViewState["VariantValueId"] != null)
                            {
                                for (int j = 0; j < ddlvaraint.Items.Count; j++)
                                {
                                    if (ViewState["VariantValueId"].ToString().IndexOf("," + ddlvaraint.Items[j].Value.ToString() + ",") > -1)
                                    {
                                        ddlvaraint.SelectedValue = ddlvaraint.Items[j].Value.ToString();
                                    }
                                }
                            }
                            else
                            {
                                ddlvaraint.SelectedIndex = 0;
                            }
                            objDiv.Controls.Add(ddlvaraint);
                            ltrText = new Literal();
                            ltrText.Text = "</td></tr>";
                            objDiv.Controls.Add(ltrText);
                        }

                        strVName = dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                    }
                }
                strScriptVar += "chkHeight(); return true;}";
                ltrText = new Literal();
                ltrText.Text = "</table>";
                objDiv.Controls.Add(ltrText);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter" /> object and calls on the child controls of the <see cref="T:System.Web.UI.Page" /> to render.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the page content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                System.IO.StringWriter stringWriter = new System.IO.StringWriter(stringBuilder);
                System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stringWriter);
                base.Render(htmlWriter);
                string yourHtml = stringBuilder.ToString();//.Replace(stringBuilder.ToString().IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=") + ,""); // ***** Parse and Modify This *****
                yourHtml = yourHtml.Replace("href=\"javascript:__doPostBack('gvListProducts','Page$", "onclick=\"chkHeight();\" href=\"javascript:__doPostBack('gvListProducts','Page$");
                yourHtml = yourHtml.Replace("href=\"javascript:__doPostBack(&#39;gvListProducts&#39;,&#39;Page$", "onclick=\"chkHeight();\" href=\"javascript:__doPostBack(&#39;gvListProducts&#39;,&#39;Page$");

                writer.Write(yourHtml);
            }
            catch
            {
            }
        }

        /// <summary>
        /// List Product Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvListProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)e.Row.FindControl("chkSelect");
                Label lbl = (Label)e.Row.FindControl("lblSKU1");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                TextBox txtQty = (TextBox)e.Row.FindControl("TxtQty");
                System.Web.UI.HtmlControls.HtmlAnchor avariantid = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("avariantid");
                System.Web.UI.HtmlControls.HtmlContainerControl divvariantid = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("divvariant");
                System.Web.UI.HtmlControls.HtmlAnchor divinnerclose = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("divinnerclose");
                System.Web.UI.HtmlControls.HtmlContainerControl divAttributes = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("divAttributes");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantStatus");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnitemprice = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnitemprice");
                Label lblvariantprice = (Label)e.Row.FindControl("lblvariantprice");
                ImageButton btnSelectGrid = (ImageButton)e.Row.FindControl("btnSelectGrid");
                btnSelectGrid.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

                if (GetVarinatByProductID(Convert.ToInt32(lblProductID.Text.ToString()), divAttributes, txtQty, btnSelectGrid, Convert.ToDecimal(hdnitemprice.Value.ToString()), hdnVariantStatus, lblvariantprice, Convert.ToInt32(e.Row.RowIndex.ToString())))
                {
                    hdnVariantStatus.Value = "1";
                }
                else
                {
                    hdnVariantStatus.Value = "0";
                    avariantid.InnerHtml = "";
                }
                avariantid.Attributes.Add("onclick", "ShowVariantdiv('" + divvariantid.ClientID.ToString() + "');$('html, body').animate({ scrollTop: $('#" + divAttributes.ClientID.ToString() + "').offset().top }, 'slow');");
                divinnerclose.Attributes.Add("onclick", "HideVariantdiv('" + divvariantid.ClientID.ToString() + "');");
                if (Request.Browser.ToString().ToLower().IndexOf("firefox") > -1)
                {
                    e.Row.Cells[5].Attributes.Add("style", "position:relative;");
                }

                if (strspt != null && strspt.Length > 0)
                {
                    for (int i = 0; i < strspt.Length; i++)
                    {
                        if (lbl.Text.ToString().Trim() == strspt[i].ToString())
                        {
                            chk1.Checked = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            pageNo = 1;
            gvListProducts.PageIndex = 0;
            BindData();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gvListProducts.PageIndex = 0;
                txtSearch.Text = "";
                BindData();
            }
            catch { }
        }

        /// <summary>
        ///  Selected Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdSelected_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


        }
       
        /// <summary>
        /// Selected Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdSelected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblName = e.Row.FindControl("lblName") as Label;
                Label lblGridPrice = (Label)e.Row.FindControl("lblGridPrice");
                ImageButton btndel = (ImageButton)e.Row.FindControl("btndel");
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");

                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblProductType = (Label)e.Row.FindControl("lblProductType");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("btndel");
                string[] variantName = lblVariantNames.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblVariantValues.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (lblProductType.Text.ToString().ToLower().Trim() == "child")
                {
                    imgDelete.Visible = false;
                }

                for (int j = 0; j < variantValue.Length; j++)
                {
                    if (variantName.Length > j)
                    {
                        lblName.Text += "<br />" + variantName[j].ToString() + " : " + variantValue[j].ToString();
                    }
                }

                btndel.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";

                if (lblTotal.Text == "")
                    lblTotal.Text = "0";
                if (lblGridPrice != null && !string.IsNullOrEmpty(lblGridPrice.Text))
                {
                    if (lblProductType.Text.ToString().ToLower().Trim() != "child")
                    {
                        lblTotal.Text = Convert.ToString(Convert.ToDecimal(lblTotal.Text) + Convert.ToDecimal(lblGridPrice.Text));
                    }
                }
                if (lblSKU != null && !string.IsNullOrEmpty(lblSKU.Text))
                {
                    if (HdnProductSKu.Value == "")
                        HdnProductSKu.Value = lblSKU.Text;
                    else
                        HdnProductSKu.Value = HdnProductSKu.Value + "," + lblSKU.Text;
                }
            }
        }

        /// <summary>
        ///  Selected Item Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSelectItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int count = 0;
                string strScript = "";
                foreach (GridViewRow r in gvListProducts.Rows)
                {
                    CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                    Label lblSKU = (Label)r.FindControl("lblSKU1");
                    Label lblName1 = (Label)r.FindControl("lblName1");
                    Label lblProductID = (Label)r.FindControl("lblProductID");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnitemprice = (System.Web.UI.HtmlControls.HtmlInputHidden)r.FindControl("hdnitemprice");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)r.FindControl("hdnVariantStatus");
                    TextBox TxtQuantity = (TextBox)r.FindControl("TxtQty");

                    if (chk.Checked && !string.IsNullOrEmpty(lblSKU.Text))
                    {
                        if (Convert.ToInt32(hdnVariantStatus.Value.ToString()) != 0 && Convert.ToInt32(hdnVariantStatus.Value.ToString()) == 2)
                        {
                            string[] strkey = Request.Form.AllKeys;
                            string VariantValueId = "";
                            string VariantNameId = "";
                            foreach (string strkeynew in strkey)
                            {
                                if (strkeynew.ToString().ToLower().IndexOf("ddlvariant") > -1 && strkeynew.ToString().ToLower().IndexOf("$ddlvariant_" + lblProductID.Text.ToString() + "_") > -1)
                                {
                                    VariantValueId += Request.Form[strkeynew].ToString() + ",";
                                    VariantNameId += strkeynew.ToString().Substring(strkeynew.ToString().LastIndexOf("_") + 1, strkeynew.ToString().Length - strkeynew.ToString().LastIndexOf("_") - 1) + ",";
                                }
                            }
                            AddTocart(Convert.ToInt32(lblProductID.Text), (TxtQuantity.Text == "" ? 1 : Convert.ToInt32(TxtQuantity.Text)), Convert.ToDecimal(hdnitemprice.Value.ToString()), VariantValueId, VariantNameId);
                        }
                        else if (Convert.ToInt32(hdnVariantStatus.Value.ToString()) == 1)
                        {
                            if (strScript == "")
                            {
                                // strScript = @"Please Select Variant For below Product: \n ";
                                strScript = @"if you want to select Variant then Click on [ Variant ]";
                            }
                            strScript += " - " + lblName1.Text.ToString().Replace("'", @"\'") + @" \n ";
                        }
                        else
                        {
                            AddTocart(Convert.ToInt32(lblProductID.Text), (TxtQuantity.Text == "" ? 1 : Convert.ToInt32(TxtQuantity.Text)), Convert.ToDecimal(hdnitemprice.Value.ToString()), "", "");
                        }
                        if (strScript != "")
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "jAlert('" + strScript.ToString() + "','Information');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "jConfirm('if you want to select Variant then Click on [ Variant ]','Information',function(result) {if(result){return false;} else {document.getElementById('hdnVariProductId').value=" + lblProductID.Text + "; document.getElementById('hdnVariQuantity').value=" + TxtQuantity.Text + "; document.getElementById('hdnVariPrice').value=" + hdnitemprice.Value + "; document.getElementById('btnAddtocartwithvariant').click();}});", true);
                        }
                        addtolist(lblSKU.Text);
                        count++;
                    }
                }
                if (count > 0)
                {
                    CartGridDataBind();
                }
                BindData();
            }
            catch { }
        }

        /// <summary>
        /// Saves Selected SKUs into ViewState
        /// </summary>
        /// <param name="lblSKU">string lblSKU</param>
        private void addtolist(string lblSKU)
        {
            try
            {
                string list = ViewState["SelectedSKUs"].ToString();

                if (!string.IsNullOrEmpty(lblSKU) && !list.Contains(lblSKU + ","))
                {
                    list += lblSKU + ",";
                }
                ViewState["SelectedSKUs"] = list;
            }
            catch { }
        }
        
        /// <summary>
        /// Selected Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdSelected_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delMe")
                {
                    RemoveDataFromAddToCart(Convert.ToInt32(e.CommandArgument.ToString()));
                    CartGridDataBind();
                    BindData();

                }
            }
            catch { }
        }

        /// <summary>
        /// Bind Shopping Cart details by Customer Id
        /// </summary>
        /// <param name="CustID">Customer Id</param>
        private String BindCart()
        {
            Decimal NetPrice = Decimal.Zero;
            Decimal SubTotal = Decimal.Zero;
            Decimal OrderTotal = Decimal.Zero;
            Table = new StringBuilder();
            Decimal QtyDiscount = Decimal.Zero;
            Int32 CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
            Int32 CustID = CustomerID;
            OrderComponent objOrder = new OrderComponent();
            DataSet DsCItems = new DataSet();
            DsCItems = ShoppingCartComponent.GetPhoneOrderCartDetailByCustomerID(CustID);

            int NumberOfItems = 0;

            //Bind Shopping Cart Details
            if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
            {
                Table.Append("  <table width='100%' border='0' align='left' cellpadding='0' cellspacing='0' class='table'>");
                Table.Append("<tr>");
                Table.Append("<th width=\"30%\" align=\"left\" valign=\"top\">Product</th>");
                Table.Append("<th width=\"11%\" align=\"left\" valign=\"top\">SKU</th>");
                Table.Append("<th width=\"9%\" align=\"right\" valign=\"top\">Price</th>");
                Table.Append("<th width=\"10%\" align=\"center\" valign=\"top\">Quantity</th>");
                Table.Append("<th width=\"10%\" align=\"right\" valign=\"top\">Sub Total</th>");
                Table.Append("</tr>");
                NetPrice = 0;
                String Name = String.Empty;
                for (Int32 CartItemNo = 0; CartItemNo < DsCItems.Tables[0].Rows.Count; CartItemNo++)
                {
                    NumberOfItems = NumberOfItems + Convert.ToInt32(DsCItems.Tables[0].Rows[CartItemNo]["Quantity"].ToString());
                    NetPrice = Convert.ToDecimal(DsCItems.Tables[0].Rows[CartItemNo]["SalePrice"].ToString()) * Convert.ToInt32(DsCItems.Tables[0].Rows[CartItemNo]["Quantity"].ToString());
                    SubTotal += NetPrice;
                    Table.Append("<tr>");
                    Table.Append("<td align='left' valign='top'> " + DsCItems.Tables[0].Rows[CartItemNo]["Name"].ToString() + "</a>");
                    Table.Append("</td>");
                    Table.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[CartItemNo]["SKU"].ToString());
                    Table.Append("</td>");
                    Table.Append("<td align='right' valign='top'>$" + DsCItems.Tables[0].Rows[CartItemNo]["SalePrice"].ToString());
                    Table.Append("</td>");
                    Table.Append("<td align='right' valign='top'>$" + Convert.ToInt32(DsCItems.Tables[0].Rows[CartItemNo]["Quantity"].ToString()));
                    Table.Append("</td>");
                    Table.Append("<td align='right' valign='top' >$" + NetPrice + "</td>");
                    Table.Append("</tr>");
                }
                Table.Append("</table>");
                return Table.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Get Shopping Cart Details by Customer Id
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        private DataSet GetShoppingcart(Int32 CustID)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet DsCItems = new DataSet();
            DsCItems = objOrder.GetCartIDByCustIDPhoneOrder(CustID, Convert.ToInt32(StoreID));
            return DsCItems;
        }

        /// <summary>
        /// Add Product to Shopping Cart
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="Qty">Product Quantity</param>
        protected void AddTocart(Int32 ProductID, Int32 Qty, Decimal itemPrice, string vValueid, string vNameid)
        {

            String strVariantNames = String.Empty;
            String strVariantValues = String.Empty;
            Decimal price = Decimal.Zero;
            Int32 finalQty = Qty;
            price += itemPrice;

            ////Check Inventory of Product in Database and Add product in Shopping Cart
            //Int32 SCartID = -1;
            //SCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + Convert.ToInt32(HdnCustID.Value.ToString()) + ""));
            //if (SCartID != -1)
            //{
            #region Variant
            try
            {
                if (vValueid.Length > 0 && vNameid.Length > 0)
                {
                    string[] VariName = vNameid.Split(',');
                    string[] VariValue = vValueid.Split(',');

                    for (int k = 0; k < VariName.Length; k++)
                    {
                        if (!string.IsNullOrEmpty(VariValue[k]) && VariName[k].ToString().Replace(",", "").ToLower().IndexOf("engraving fonts") > -1)
                        {
                            strVariantNames += VariName[k].ToString().Replace(",", "") + ",";
                            strVariantValues += VariValue[k].ToString().Replace(",", "") + ",";
                        }
                        else
                        {
                            DataSet dsvariant = new DataSet();
                            if (!string.IsNullOrEmpty(VariValue[k]))
                            {
                                int Variantid = Convert.ToInt32(VariName[k].ToString().Replace(",", ""));
                                // checking for Engraving Variant
                                dsvariant = CommonComponent.GetCommonDataSet("SELECT dbo.tb_ProductVariant.VariantName, dbo.tb_ProductVariantValue.VariantValue,dbo.tb_ProductVariant.VariantID,dbo.tb_ProductVariantValue.VariantValueID, dbo.tb_ProductVariantValue.ProductID, isnull(dbo.tb_ProductVariantValue.VariantPrice,0) as VariantPrice,  " +
                                                                               " case when ISNULL(replace(replace(isnull(dbo.tb_ProductVariantValue.VariantValue,''),'+','$'),'$$','$'),'') like '%$%'    " +
                                                                               " then replace(replace(isnull(dbo.tb_ProductVariantValue.VariantValue,''),'+','$'),'$$','$')   " +
                                                                               " WHEN isnull(dbo.tb_ProductVariantValue.VariantPrice,0) > 0  " +
                                                                               " then dbo.tb_ProductVariantValue.VariantValue+'($'+ CAST(isnull(dbo.tb_ProductVariantValue.VariantPrice,0) as nvarchar(20))+')'  " +
                                                                               " else dbo.tb_ProductVariantValue.VariantValue end as priceVariantValue   " +
                                                                               " FROM dbo.tb_ProductVariant INNER JOIN      " +
                                                                               " dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID      " +
                                                                               " WHERE  dbo.tb_ProductVariant.ProductID = " + ProductID + " and dbo.tb_ProductVariant.VariantID=" + Variantid + " and dbo.tb_ProductVariantValue.VariantValue <>'Inscription' And (tb_ProductVariant.Variantname like '%Engraving%' and tb_ProductVariant.Variantname like '%character%') ORDER BY dbo.tb_ProductVariant.VariantName,dbo.tb_ProductVariantValue.DisplayOrder");

                                if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                                {
                                    String VariantValueid = Convert.ToString(VariValue[k].ToString().Replace(",", ""));
                                    foreach (DataRow dr in dsvariant.Tables[0].Rows)
                                    {
                                        strVariantNames += dr["VariantName"].ToString() + ",";
                                        strVariantValues += VariantValueid.ToString() + ",";
                                    }
                                }
                                else // Go for Variant
                                {

                                    int VariantValueid = Convert.ToInt32(VariValue[k].ToString().Replace(",", ""));
                                    dsvariant = CommonComponent.GetCommonDataSet("SELECT dbo.tb_ProductVariant.VariantName, dbo.tb_ProductVariantValue.VariantValue,dbo.tb_ProductVariant.VariantID,dbo.tb_ProductVariantValue.VariantValueID, dbo.tb_ProductVariantValue.ProductID, isnull(dbo.tb_ProductVariantValue.VariantPrice,0) as VariantPrice,  " +
                                                                                " case when ISNULL(replace(replace(isnull(dbo.tb_ProductVariantValue.VariantValue,''),'+','$'),'$$','$'),'') like '%$%'    " +
                                                                                " then replace(replace(isnull(dbo.tb_ProductVariantValue.VariantValue,''),'+','$'),'$$','$')   " +
                                                                                " WHEN isnull(dbo.tb_ProductVariantValue.VariantPrice,0) > 0  " +
                                                                                " then dbo.tb_ProductVariantValue.VariantValue+'($'+ CAST(isnull(dbo.tb_ProductVariantValue.VariantPrice,0) as nvarchar(20))+')'  " +
                                                                                " else dbo.tb_ProductVariantValue.VariantValue end as priceVariantValue   " +
                                                                                " FROM dbo.tb_ProductVariant INNER JOIN      " +
                                                                                " dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID      " +
                                                                                " WHERE  dbo.tb_ProductVariant.ProductID = " + ProductID + " and dbo.tb_ProductVariant.VariantID=" + Variantid + " and dbo.tb_ProductVariantValue.VariantValueID=" + VariantValueid + " ORDER BY dbo.tb_ProductVariant.VariantName,dbo.tb_ProductVariantValue.DisplayOrder");

                                    foreach (DataRow dr in dsvariant.Tables[0].Rows)
                                    {
                                        strVariantNames += dr["VariantName"].ToString() + ",";
                                        strVariantValues += dr["priceVariantValue"].ToString() + ",";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            #endregion

            String[] Names = strVariantNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            String[] Values = strVariantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string Value in Values)
            {
                if (Value.Contains("("))
                {
                    String CPrice = String.Empty;
                    CPrice = Value.Substring(Value.IndexOf('(') + 1);
                    CPrice = CPrice.Replace("(", "").Replace(")", "").Replace("+", "").Replace("$", "");
                    Decimal TempPrice = Decimal.Zero;
                    Decimal.TryParse(CPrice, out TempPrice);
                    price += TempPrice;
                }
            }

            DataSet Ds = new DataSet();
            ShoppingCartComponent objShopping = new ShoppingCartComponent();
            string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(ProductID), Qty, price, "", "", strVariantNames, strVariantValues,0);
            if (strResult.ToString().ToLower() == "success")
            {
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('" + strResult.ToString() + "','Sorry!');", true);
            }
            //}
        }

        /// <summary>
        ///Binds the  Cart Data into Grid
        /// </summary>
        private void CartGridDataBind()
        {
            lblTotal.Text = "0";
            HdnProductSKu.Value = "";
            Int32 CustomerID = Convert.ToInt32(HdnCustID.Value.ToString());
            DataSet dsGridCart = new DataSet();
            //Bind Shopping Cart Details
            Int32 CartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + Convert.ToInt32(HdnCustID.Value.ToString()) + ""));
            dsGridCart = ShoppingCartComponent.GetPhoneOrderCartDetailByCustomerID(CustomerID);
            if (dsGridCart != null && dsGridCart.Tables[0].Rows.Count > 0)
            {
                grdSelected.DataSource = dsGridCart.Tables[0];
                grdSelected.DataBind();
                trSelectedproduct.Visible = true;
                lbMsg.Text = "";
            }
            else
            {
                grdSelected.DataSource = null;
                grdSelected.DataBind();
                trSelectedproduct.Visible = false;
                lbMsg.Text = "Your Shopping Cart is Empty.";
            }
        }

        /// <summary>
        /// Removes the Data From Add to Cart
        /// </summary>
        /// <param name="CustomCartID">int CustomCartID</param>
        private void RemoveDataFromAddToCart(Int32 CustomCartID)
        {
            CommonComponent.ExecuteCommonData("delete from tb_ShoppingCartItems Where CustomCartID=" + CustomCartID + "");
            CartGridDataBind();

        }

        /// <summary>
        /// Removes All Data from Add to Cart
        /// </summary>
        /// <param name="ShoppingCartID">int ShoppingCartID</param>
        private void RemoveAllDataFromAddToCart(Int32 ShoppingCartID)
        {
            CommonComponent.ExecuteCommonData("delete from tb_ShoppingCartItems Where ShoppingCartID=" + ShoppingCartID + "");
            CartGridDataBind();
        }
        
        /// <summary>
        /// Clears the Shopping Cart
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void ClearShopping(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(clientid))
                {
                    //BindCart(Convert.ToInt32(HdnCustID.Value));
                    string skus = ViewState["SelectedSKUs"].ToString();
                    if (skus.Length > 1)
                        skus = skus.TrimEnd(",".ToCharArray());
                    string cid = Request.QueryString["clientid"].ToString().Trim();
                    if (!string.IsNullOrEmpty(Request.QueryString["lblID"].ToString()))
                    {
                        if (Request.QueryString["marrypro"] != null)
                        {
                            int ii = 0;
                            foreach (GridViewRow gr in grdSelected.Rows)
                            {
                                string strsku = (gr.FindControl("lblSKU") as Label).Text.Trim();
                                if (!string.IsNullOrEmpty(strsku))
                                    ii = ii + 1;
                            }
                            if (ii > 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "jAlert('You can select only one product for marry product.','Required information')", true);
                                return;
                            }
                        }
                        skus = HdnProductSKu.Value.ToString();
                        string skus1 = skus;
                        String StrDivCartItems = BindCart();
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('hfsuto').value='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblSubTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblTotal').innerHTML=parseFloat(parseFloat(window.opener.document.getElementById('lblSubTotal').innerHTML)+parseFloat(window.opener.document.getElementById('TxtShippingCost').value)+parseFloat(window.opener.document.getElementById('TxtTax').value)-parseFloat(window.opener.document.getElementById('TxtDiscount').value)).toFixed(2);window.opener.document.getElementById('hfTotal').value=window.opener.document.getElementById('lblTotal').innerHTML;window.opener.document.getElementById('btnUpdate').click();window.close();", true);
                        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "msg", "window.close();", true);
                    }
                    else
                    {
                        if (Request.QueryString["marrypro"] != null)
                        {
                            int ii = 0;
                            foreach (GridViewRow gr in grdSelected.Rows)
                            {
                                string strsku = (gr.FindControl("lblSKU") as Label).Text.Trim();
                                if (!string.IsNullOrEmpty(strsku))
                                    ii = ii + 1;
                            }
                            if (ii > 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "jAlert('You can select only one product for marry product.','Required Information')", true);
                                return;
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('lblSubTotal').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('btnUpdate').click();window.close();", true);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the Sub Total with Round Value
        /// </summary>
        /// <param name="Price">Decimal Price</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns the Sub Total with Round Decimal Value</returns>
        public static Decimal GetSubTotal(Decimal Price, Int32 Qty)
        {
            return Math.Round((Price * Qty), 2);
        }

        /// <summary>
        ///  List Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvListProducts_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            gvListProducts.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// Card Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void gvListProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int count = 0;
                string strScript = "";
                Label lblSKU = (Label)gvListProducts.Rows[e.NewEditIndex].FindControl("lblSKU1");
                Label lblName1 = (Label)gvListProducts.Rows[e.NewEditIndex].FindControl("lblName1");
                Label lblProductID = (Label)gvListProducts.Rows[e.NewEditIndex].FindControl("lblProductID");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnitemprice = (System.Web.UI.HtmlControls.HtmlInputHidden)gvListProducts.Rows[e.NewEditIndex].FindControl("hdnitemprice");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)gvListProducts.Rows[e.NewEditIndex].FindControl("hdnVariantStatus");
                TextBox TxtQuantity = (TextBox)gvListProducts.Rows[e.NewEditIndex].FindControl("TxtQty");
                if (Convert.ToInt32(hdnVariantStatus.Value.ToString()) != 0 && Convert.ToInt32(hdnVariantStatus.Value.ToString()) == 2)
                {
                    string[] strkey = Request.Form.AllKeys;
                    string VariantValueId = "";
                    string VariantNameId = "";
                    foreach (string strkeynew in strkey)
                    {
                        if (strkeynew.ToString().ToLower().IndexOf("txt1_kau_") > -1 && strkeynew.ToString().ToLower().IndexOf("txt1_kau_" + lblProductID.Text.ToString() + "_") > -1)
                        {
                            VariantValueId += Request.Form[strkeynew].ToString() + ",";
                            VariantNameId += strkeynew.ToString().Substring(strkeynew.ToString().LastIndexOf("_") + 1, strkeynew.ToString().Length - strkeynew.ToString().LastIndexOf("_") - 1) + ",";
                        }
                        if (strkeynew.ToString().ToLower().IndexOf("rrselect_kau_") > -1 && strkeynew.ToString().ToLower().IndexOf("rrselect_kau_" + lblProductID.Text.ToString() + "_") > -1)
                        {
                            VariantValueId += Request.Form[strkeynew].ToString() + ",";
                            VariantNameId += "Engraving Fonts,";
                        }
                        if (strkeynew.ToString().ToLower().IndexOf("ddlvariant") > -1 && strkeynew.ToString().ToLower().IndexOf("$ddlvariant_" + lblProductID.Text.ToString() + "_") > -1)
                        {
                            VariantValueId += Request.Form[strkeynew].ToString() + ",";
                            VariantNameId += strkeynew.ToString().Substring(strkeynew.ToString().LastIndexOf("_") + 1, strkeynew.ToString().Length - strkeynew.ToString().LastIndexOf("_") - 1) + ",";
                        }
                    }
                    AddTocart(Convert.ToInt32(lblProductID.Text), (TxtQuantity.Text == "" ? 1 : Convert.ToInt32(TxtQuantity.Text)), Convert.ToDecimal(hdnitemprice.Value.ToString()), VariantValueId, VariantNameId);
                }
                else if (Convert.ToInt32(hdnVariantStatus.Value.ToString()) == 1)
                {
                    if (strScript == "")
                    {
                        strScript = @"Please Select Variant For below Product: \n ";
                    }
                    strScript += " - " + lblName1.Text.ToString().Replace("'", @"\'") + @" \n ";
                }
                else
                {
                    AddTocart(Convert.ToInt32(lblProductID.Text), (TxtQuantity.Text == "" ? 1 : Convert.ToInt32(TxtQuantity.Text)), Convert.ToDecimal(hdnitemprice.Value.ToString()), "", "");
                }
                if (strScript != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "jAlert('" + strScript.ToString() + "','Information');", true);
                }
                addtolist(lblSKU.Text);
                CartGridDataBind();
                BindData();
            }
            catch { }
        }

        /// <summary>
        ///  Add to Cart with Variant Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddtocartwithvariant_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnVariProductId.Value) && !string.IsNullOrEmpty(hdnVariQuantity.Value) && !string.IsNullOrEmpty(hdnVariPrice.Value))
            {
                AddTocart(Convert.ToInt32(hdnVariProductId.Value), (hdnVariQuantity.Value == "" ? 1 : Convert.ToInt32(hdnVariQuantity.Value)), Convert.ToDecimal(hdnVariPrice.Value.ToString()), "", "");
                CartGridDataBind();
                BindData();
            }
        }
    }
}