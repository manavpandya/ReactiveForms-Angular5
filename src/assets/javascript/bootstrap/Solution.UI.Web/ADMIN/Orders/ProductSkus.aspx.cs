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
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ProductSkus : BasePage
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
        string strScriptreadymade = "";
        string strScriptmadetomeasure = "";
        static DataView dsGlobal = null;
        public string[] strspt;
        public string clientid = string.Empty;
        StringBuilder Table = null;
        Int32 TotalQuantityValue = 0;
        int Yardqty = 0;
        double actualYard = 0;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
            btnSearch.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
            btnSelectItem.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

            ImgColse2.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";
            // imgMainClose.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
            if (Request.QueryString["StoreID"] != null && Request.QueryString["clientid"].ToString() != null && !string.IsNullOrEmpty(Request.QueryString["clientid"].ToString()))
            {
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
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('ContentPlaceHolder1_hfSubTotal').value='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + skus + "';window.opener.document.getElementById('ContentPlaceHolder1_HdnSubTotal').innerHTML = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML=parseFloat(parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML)+parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtShippingCost').value)+parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtTax').value)-parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtDiscount').value)).toFixed(2);window.opener.document.getElementById('ContentPlaceHolder1_hfTotal').value=window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML;window.opener.document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();window.close();", true);
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
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();window.close();", true);
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
        /// Binds the Data for Product SKU
        /// </summary>
        public void BindData()
        {
            string SKUs = txtSearch.Text.Replace("'", "''").TrimEnd(",".ToCharArray()).TrimStart(",".ToCharArray());
            string Name = txtSearch.Text.Replace("'", "''").TrimEnd(",".ToCharArray()).TrimStart(",".ToCharArray());
            DataSet ds = new DataSet();
            string sql = "select name,ProductId,SKU,Price,case when isnull(SalePrice,0)=0 then Price else SalePrice end as SalePrice from tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId") + " and ProductID in (select ProductID from tb_ProductCategory)   and (Sku like '%" + SKUs.ToString().Trim() + "%' or name like '%" + Name.ToString().Trim() + "%') and ISNULL(Deleted,0)=0 and ISNULL(Active,0)=1";
            ds = CommonComponent.GetCommonDataSet(sql.ToString());
            gvListProducts.DataSource = null;
            gvListProducts.DataBind();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gvListProducts.DataSource = ds.Tables[0];
                    gvListProducts.DataBind();
                    //btnSelectItem.Visible = true;
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
        /// Gets the URL From SKU
        /// </summary>
        /// <param name="skus">string skus</param>
        /// <returns>Returns the Product URL</returns>
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
        /// <param name="objDiv">THtmlContainerControl objDiv</param>
        /// <param name="txtQty">TExtBox txtQty</param>
        /// <param name="imgButton">ImageButton imgButton</param>
        /// <param name="price">decimal price</param>
        /// <param name="hdnst">HtmlInputHidden hdnst</param>
        /// <param name="lblvariantprice">Label lblvariantprice.</param>
        /// <param name="RowIndex">int RowIndex</param>
        /// <returns>true if bind, false otherwise</returns>
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
        /// Product List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvListProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[0].Attributes.Add("style", "display:none");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk1 = (CheckBox)e.Row.FindControl("chkSelect");
                Label lbl = (Label)e.Row.FindControl("lblSKU1");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                TextBox txtQty = (TextBox)e.Row.FindControl("TxtQty");
                TextBox txtProQty = (TextBox)e.Row.FindControl("txtProQty");
                TextBox txtMadetoMeasureQty = (TextBox)e.Row.FindControl("txtMadetoMeasureQty");
                Literal ltvariant = (Literal)e.Row.FindControl("ltvariant");
                Literal ltYourPrice = (Literal)e.Row.FindControl("ltYourPrice");
                TextBox TxtQty = (TextBox)e.Row.FindControl("TxtQty");
                e.Row.Cells[1].Attributes.Add("style", "display:none");
                e.Row.Cells[0].Attributes.Add("style", "display:none");

                System.Web.UI.HtmlControls.HtmlAnchor avariantid = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("avariantid");
                System.Web.UI.HtmlControls.HtmlContainerControl divvariantid = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("divvariant");
                System.Web.UI.HtmlControls.HtmlAnchor divinnerclose = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("divinnerclose");
                System.Web.UI.HtmlControls.HtmlContainerControl divAttributes = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("divAttributes");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnVariantStatus");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnitemprice = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnitemprice");
                System.Web.UI.HtmlControls.HtmlContainerControl liready = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("liready");
                if (ltYourPrice != null)
                {
                    ltYourPrice.Text = "<span style='color: red; font-weight: bold;'>" + string.Format("{0:0.00}", Convert.ToDecimal(hdnitemprice.Value.ToString())) + "</span>";

                }
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Row.FindControl("lblSalePrice");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnActual = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnActual");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnprice = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnprice");
                System.Web.UI.HtmlControls.HtmlInputHidden hdncustomprice = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdncustomprice");

                System.Web.UI.HtmlControls.HtmlAnchor areadymade = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("areadymade");
                System.Web.UI.HtmlControls.HtmlAnchor amadetomeasure = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("amadetomeasure");

                System.Web.UI.HtmlControls.HtmlGenericControl divcustom = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("divcustom");
                System.Web.UI.HtmlControls.HtmlGenericControl licustom = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("licustom");

                Label lblvariantprice = (Label)e.Row.FindControl("lblvariantprice");

                ImageButton btnAddtocartReady = (ImageButton)e.Row.FindControl("btnAddtocartReady");
                btnAddtocartReady.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

                ImageButton btnAddtocartcustom = (ImageButton)e.Row.FindControl("btnAddtocartcustom");
                btnAddtocartcustom.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

                ImageButton btnSelectGrid = (ImageButton)e.Row.FindControl("btnSelectGrid");
                btnSelectGrid.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";
                Button btntempClick = (Button)e.Row.FindControl("btntempClick");
                DropDownList ddlcustomstyle = (DropDownList)e.Row.FindControl("ddlcustomstyle");
                DropDownList ddlcustomlength = (DropDownList)e.Row.FindControl("ddlcustomlength");
                DropDownList ddlcustomoptin = (DropDownList)e.Row.FindControl("ddlcustomoptin");
                DropDownList ddlcustomwidth = (DropDownList)e.Row.FindControl("ddlcustomwidth");

                Literal ltrcustomstyle = (Literal)e.Row.FindControl("ltrcustomstyle");
                Literal ltroptions = (Literal)e.Row.FindControl("ltroptions");
                ddlcustomlength.Attributes.Add("onchange", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                ddlcustomoptin.Attributes.Add("onchange", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                ddlcustomstyle.Attributes.Add("onchange", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                ddlcustomwidth.Attributes.Add("onchange", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                //dlcustomqty.Attributes.Add("onchange", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                txtMadetoMeasureQty.Attributes.Add("onkeyup", "ChangeCustomprice(" + Convert.ToInt32(lblProductID.Text.ToString()) + ", " + Convert.ToInt32(e.Row.RowIndex.ToString()) + ");");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnCurrTabvalue = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnCurrTabvalue");
                hdnVariProductId.Value = lblProductID.Text.ToString();
                string ProStoreId = "1";
                if (Request.QueryString["StoreID"] != null) { ProStoreId = Convert.ToString(Request.QueryString["StoreID"]); }
                Boolean IsRoman = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select ISNULL(IsRoman,0) as IsRoman from tb_product where Productid=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and StoreId=" + ProStoreId + ""));

                if (IsRoman == true)
                {
                    txtQty.Attributes.Add("onkeyup", "PriceChangeondropdownforroman(" + e.Row.RowIndex.ToString() + ");");
                }
                else
                {
                    txtQty.Attributes.Add("onkeyup", "PriceChangeondropdown(" + e.Row.RowIndex.ToString() + ");");
                }
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@pageloadevent", "PriceChangeondropdown(" + e.Row.RowIndex.ToString() + ");", true);
                hdnVariQuantity.Value = txtQty.Text;

                hdnVariPrice.Value = hdnitemprice.Value.ToString();

                Literal ltcustomPrice = (Literal)e.Row.FindControl("ltcustomPrice");
                Label lblAllowQty = (Label)e.Row.FindControl("lblAllowQty");
                Label lblQtyOnHand = (Label)e.Row.FindControl("lblQtyOnHand");
                Label lblNextOrderQty = (Label)e.Row.FindControl("lblNextOrderQty");
                Label lblAvailaDate = (Label)e.Row.FindControl("lblAvailaDate");
                txtProQty.Visible = true;

                // New Code for Variant
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + Convert.ToInt32(lblProductID.Text.ToString()) + " AND isnull(ParentId,0)=0 Order By DisplayOrder");
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    txtProQty.Visible = false;
                    ltvariant.Visible = true;
                    hdnVariantStatus.Value = "1";

                    decimal price = 0;
                    decimal salePrice = 0;
                    if (!string.IsNullOrEmpty(lblPrice.Text.ToString()))
                        price = Convert.ToDecimal(lblPrice.Text.ToString());
                    if (!string.IsNullOrEmpty(lblSalePrice.Text.ToString()))
                        salePrice = Convert.ToDecimal(lblSalePrice.Text.ToString());

                    if (price > decimal.Zero)
                    {
                        if (salePrice > decimal.Zero)
                        {
                            if (price > salePrice)
                            {
                                hdnprice.Value = salePrice.ToString();
                                ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + salePrice.ToString("C") + "</strong>";
                                hdncustomprice.Value = salePrice.ToString();
                            }
                        }
                        else if (salePrice == decimal.Zero)
                        {
                            hdnprice.Value = price.ToString();
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(price).ToString("C") + "</strong>";
                            hdncustomprice.Value = price.ToString();
                        }
                        else
                        {
                            hdnprice.Value = price.ToString();
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(price).ToString("C") + "</strong>";
                            hdncustomprice.Value = price.ToString();
                        }
                        hdnActual.Value = price.ToString();
                    }
                    else { hdnprice.Value = "0"; }

                    string StrVasriValue = Convert.ToString(BindReadyMade(Convert.ToInt32(lblProductID.Text.ToString()), Convert.ToInt32(e.Row.RowIndex.ToString()), btnAddtocartReady, IsRoman, areadymade));
                    if (!string.IsNullOrEmpty(StrVasriValue))
                        ltvariant.Text = StrVasriValue.ToString();
                    else
                        ltvariant.Text = "";
                }
                else
                {
                    hdnVariantStatus.Value = "0";
                    // avariantid.InnerHtml = "";
                    if (IsRoman == true)
                    {
                        btnAddtocartReady.OnClientClick = "return chkAddtocart(" + e.Row.RowIndex.ToString() + ",0);";
                    }
                    else
                    {
                        btnAddtocartReady.OnClientClick = "return chkAddtocart(" + e.Row.RowIndex.ToString() + ",1);";
                    }
                    liready.Style.Add("display", "none");
                    TxtQty.Attributes.Remove("onkeyup");
                }
                BindJaScriptforMeasure(btnAddtocartcustom, Convert.ToInt32(lblProductID.Text.ToString()), Convert.ToInt32(e.Row.RowIndex.ToString()));
                DataSet dsproduct = new DataSet();
                dsproduct = CommonComponent.GetCommonDataSet("Select ISNULL(Ismadetoorder,0)  as Ismadetoorder,ISNULL(Ismadetoready,0)  as Ismadetoready,ISNULL(Ismadetomeasure,0) as Ismadetomeasure from tb_product where Productid=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and StoreId=" + ProStoreId + "");
                if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoready"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoready"].ToString()))
                    {
                        //areadymade.InnerHtml = "MADE TO ORDER";
                        //areadymade.Title = "MADE TO ORDER";
                    }
                    else if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoorder"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoorder"].ToString()))
                    {
                        areadymade.InnerHtml = "MADE TO ORDER";
                        areadymade.Title = "MADE TO ORDER";
                    }
                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString()))
                    {
                        amadetomeasure.InnerHtml = "MADE TO MEASURE";
                        amadetomeasure.Title = "MADE TO MEASURE";
                    }
                }
                //string madetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(Ismadetoorder,0)  as Ismadetoorder from tb_product where Productid=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and StoreId=" + ProStoreId + ""));
                //if (!string.IsNullOrEmpty(madetoorder.ToString()) && Convert.ToBoolean(madetoorder.ToString()))
                //{
                //    areadymade.InnerHtml = "MADE TO ORDER";
                //    areadymade.Title = "MADE TO ORDER";
                //}
                //if (!string.IsNullOrEmpty(madetoorder.ToString()) && Convert.ToBoolean(madetoorder.ToString()))
                //{
                //    amadetomeasure.InnerHtml = "MADE TO MEASURE";
                //    areadymade.Title = "MADE TO MEASURE";
                //}

                string isCustom = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(isCustom,0)  as isCustom from tb_product where Productid=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and StoreId=" + ProStoreId + ""));
                if (!string.IsNullOrEmpty(isCustom.ToString()) && !Convert.ToBoolean(isCustom.ToString()))
                {
                    divcustom.Visible = false;
                    licustom.Visible = false;
                }
                else
                {
                    /// If Product is Made to Measure
                    DataSet DSFabricDetails = CommonComponent.GetCommonDataSet("Select FabricCode,FabricType  from tb_product where Productid=" + Convert.ToInt32(lblProductID.Text.ToString()) + " and StoreId=" + AppConfig.StoreID + "");
                    if (DSFabricDetails != null && DSFabricDetails.Tables.Count > 0 && DSFabricDetails.Tables[0].Rows.Count > 0)
                    {
                        string FabricCode = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricCode"]);
                        string FabricType = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricType"]);
                        Int32 FabricTypeID = 0;
                        if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                        {
                            FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                            if (FabricTypeID > 0)
                            {
                                DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                Int32 AllowQty = 0, QtyOnHand = 0, NextOrderQty = 0;

                                if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AllowQty"].ToString()))
                                    {
                                        AllowQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["AllowQty"]);
                                        lblAllowQty.Text = "Allow Quantity : <span style='color:red;'>" + AllowQty.ToString() + "</span>";
                                    }
                                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                    {
                                        QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);
                                        lblQtyOnHand.Text = "Quantity On Hand : <span style='color:red;'>" + QtyOnHand.ToString() + "</span>";
                                    }
                                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                    {
                                        NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                        lblNextOrderQty.Text = "Next Order Quantity : <span style='color:red;'>" + NextOrderQty.ToString() + "</span>";
                                    }

                                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && !dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().Contains("1900"))
                                    {
                                        try
                                        {
                                            lblAvailaDate.Text = "Available Date : <span style='color:red;'>" + string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"])) + "</span>";
                                        }
                                        catch { }
                                    }
                                }

                            }
                        }
                    }
                    GetStyle(ddlcustomstyle, Convert.ToInt32(lblProductID.Text.ToString()), e.Row.RowIndex);
                    Getoptions(Convert.ToInt32(lblProductID.Text.ToString()), ddlcustomoptin, e.Row.RowIndex);
                }
                //if (GetVarinatByProductID(Convert.ToInt32(lblProductID.Text.ToString()), divAttributes, txtQty, btnSelectGrid, Convert.ToDecimal(hdnitemprice.Value.ToString()), hdnVariantStatus, lblvariantprice, Convert.ToInt32(e.Row.RowIndex.ToString()), btntempClick))
                //{
                //    hdnVariantStatus.Value = "1";
                //}
                //else
                //{
                //    hdnVariantStatus.Value = "0";
                //    avariantid.InnerHtml = "";
                //}
                if (IsRoman == true)
                {
                    avariantid.Attributes.Add("onclick", "ShowVariantdiv('" + divvariantid.ClientID.ToString() + "');$('html, body').animate({ scrollTop: $('#" + divAttributes.ClientID.ToString() + "').offset().top }, 'slow');PriceChangeondropdownforroman(" + e.Row.RowIndex.ToString() + ");" + strScriptreadymade + strScriptmadetomeasure + "");
                }
                else
                {
                    avariantid.Attributes.Add("onclick", "ShowVariantdiv('" + divvariantid.ClientID.ToString() + "');$('html, body').animate({ scrollTop: $('#" + divAttributes.ClientID.ToString() + "').offset().top }, 'slow');" + strScriptreadymade + strScriptmadetomeasure + "");
                }
                strScriptreadymade = "";
                strScriptmadetomeasure = "";
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
        ///  List Gridview Page Index Changing Event
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
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("btndel");
                string[] variantName = lblVariantNames.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] variantValue = lblVariantValues.Text.ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                Label lblDiscountPrice = (Label)e.Row.FindControl("lblDiscountPrice");
                Label lblOrginalDiscountPrice = (Label)e.Row.FindControl("lblOrginalDiscountPrice");
                Label lblSalePrice = (Label)e.Row.FindControl("lblSalePrice");
                Label lblQtyGrid = (Label)e.Row.FindControl("lblQtyGrid");
                Label lblRelatedproductID = (Label)e.Row.FindControl("lblRelatedproductID");
                if (lblRelatedproductID != null && lblRelatedproductID.Text.ToString().Trim() != "0")
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
                decimal DiscountPrice = 0, OrgiPrice = 0;
                decimal DicPrice = 0, TempDis = 0;
                if (HdnCustID.Value != null && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                {
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString().Trim().ToString(), out DiscountPrice);
                    decimal.TryParse(lblSalePrice.Text.ToString().Trim().ToString(), out OrgiPrice);
                    lblHeaderDiscount.Text = "(" + DiscountPrice.ToString("f2") + "%)";

                    if (DiscountPrice > 0)
                    {
                        if (DiscountPrice > 0 && DiscountPrice <= 99)
                        {
                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                            DicPrice = OrgiPrice - TempDis;
                            lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                            grdSelected.Columns[4].Visible = true;
                            tdDiscountPrice.Visible = true;
                        }
                        else if (DiscountPrice >= 100)
                        {
                            TempDis = Convert.ToDecimal((OrgiPrice * DiscountPrice) / 100);
                            DicPrice = TempDis;
                            lblOrginalDiscountPrice.Text = DicPrice.ToString("f2");
                            grdSelected.Columns[4].Visible = true;
                            tdDiscountPrice.Visible = true;
                        }
                        else
                        {
                            lblOrginalDiscountPrice.Text = "0.00";
                            grdSelected.Columns[4].Visible = true;
                            tdDiscountPrice.Visible = true;
                        }
                    }
                    else
                    {
                        lblOrginalDiscountPrice.Text = "0.00";
                        grdSelected.Columns[4].Visible = false;
                        tdDiscountPrice.Visible = false;
                    }
                }
                else { grdSelected.Columns[4].Visible = false; tdDiscountPrice.Visible = false; lblOrginalDiscountPrice.Text = "0.00"; }

                if (lblGridPrice != null && !string.IsNullOrEmpty(lblGridPrice.Text))
                {
                    if (DicPrice > 0)
                    {
                        decimal GrdSubTot = DicPrice * Convert.ToDecimal(lblQtyGrid.Text.ToString());
                        lblGridPrice.Text = GrdSubTot.ToString("f2");
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
        ///  Select Item GridView Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
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
                    TextBox TxtQuantity = (TextBox)r.FindControl("txtProQty");

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
                                //strScript = @"if you want to select Variant then Click on [ Variant ]";
                                strScript = @"Please Select Variant For below Product: out side \n ";

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
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "jConfirm('if you want to select Variant then Click on [ Variant ]','Information',function(result) {if(result){return false;} else {document.getElementById('hdnVariProductId').value=" + lblProductID.Text + "; document.getElementById('hdnVariQuantity').value=" + TxtQuantity.Text + "; document.getElementById('hdnVariPrice').value=" + hdnitemprice.Value + "; document.getElementById('btnAddtocartwithvariant').click();}});", true);
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
        /// Add to lists the specified LBL SKU.
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
                    int Cnt = 0;
                    int i = Convert.ToInt32(e.CommandArgument.ToString());
                    if (grdSelected.Rows.Count > 0)
                    {
                        foreach (GridViewRow rn in grdSelected.Rows)
                        {
                            Label lblRelatedproductID = (Label)rn.FindControl("lblRelatedproductID");
                            if (lblRelatedproductID != null && lblRelatedproductID.Text.ToString() == "0")
                            {
                                Cnt++;
                            }
                        }
                    }
                    if (grdSelected.Rows.Count > 0 && Cnt != 1)
                    {
                        Int32 OrderNumber = Convert.ToInt32(Request.QueryString["Ono"].ToString());
                        Label lblProductId = (Label)grdSelected.Rows[i].FindControl("lblChnageOrderProductId");
                        Label lblVariantNames = (Label)grdSelected.Rows[i].FindControl("lblVariantNames");
                        Label lblVariantValues = (Label)grdSelected.Rows[i].FindControl("lblVariantValues");
                        Int32 ProductId = Convert.ToInt32(lblProductId.Text.ToString());
                        RemoveDataFromAddToCart(OrderNumber, ProductId, lblVariantNames.Text.ToString(), lblVariantValues.Text.ToString());
                        CartGridDataBind();
                        BindData();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Msg", "jAlert('You can not delete this record, One record is require.','Required Information')", true);
                        return;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Bind Shopping Cart details by Customer Id
        /// </summary>
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
        /// <param name="CustID">int CustID</param>
        /// <returns>Returns the Shopping Cart List as a Dataset</returns>
        private DataSet GetShoppingcart(Int32 CustID)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet DsCItems = new DataSet();
            DsCItems = objOrder.GetCartIDByCustIDPhoneOrder(CustID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            return DsCItems;
        }

        /// <summary>
        /// Add Product to Shopping Cart
        /// </summary>
        /// <param name="ProductID">Product Id</param>
        /// <param name="Qty">Product Quantity</param>
        protected void AddTocart(Int32 ProductID, Int32 Qty, Decimal itemPrice, string vValueid, string vNameid)
        {
            int pInventory = 0;
            bool outofstock = false;
            bool isDropshipProduct = false;
            string StrErrorMsg = "";
            int Yardqty = 0;
            double actualYard = 0;
            String strVariantNames = String.Empty;
            String strVariantValues = String.Empty;
            Decimal price = Decimal.Zero;
            Int32 finalQty = Qty;
            price += itemPrice;
            Int32 Isorderswatch1 = 0;
            string varinatvaluetemp = "";
            if (vValueid.Length > 0 && vNameid.Length > 0)
            {
                strVariantNames = vNameid.ToString();
                strVariantValues = vValueid.ToString();
            }
            Isorderswatch1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + ProductID.ToString() + " and ItemType='Swatch' "));
            if (Isorderswatch1 == 1)
            {
                price = 0;
            }
            // check Available Quantity
            //isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + ProductID.ToString() + ""));
            //string ProductType = "0";
            //if (ViewState["IsCustom"] != null)
            //{
            //    ProductType = "2";
            //    ViewState["IsCustom"] = null;
            //}
            //else
            //{
            //    ProductType = Convert.ToString(CommonComponent.GetScalarCommonData("Select (case when ISNULL(IsRoman,0)=1 then 3 else 1 end) As ProductType from tb_Product where isnull(Isfreefabricswatch,0) <> 1 AND Productid=" + ProductID + " and StoreID=1"));
            //    if (ProductType == "")
            //    {
            //        ProductType = "0";
            //    }
            //}
            //  string ProductType = Convert.ToString(CommonComponent.GetScalarCommonData("Select (case when ISNULL(IsCustom,0) = 1 then 2 when ISNULL(IsRoman,0)=1 then 3 else 1 end) As ProductType from tb_Product where Productid=" + ProductID + " and StoreID=" + AppLogic.AppConfigs("Storeid").ToString() + ""));
            isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + ProductID.ToString() + ""));
            string ProductType = "0";
            if (ViewState["IsCustom"] != null)
            {
                ProductType = "2";
                ViewState["IsCustom"] = null;
            }
            else
            {
                ProductType = Convert.ToString(CommonComponent.GetScalarCommonData("Select (case when ISNULL(IsRoman,0)=1 then 3 else 1 end) As ProductType from tb_Product where  ItemType<>'Swatch'  AND Productid=" + ProductID + " and StoreID=" + AppLogic.AppConfigs("Storeid").ToString() + ""));
                if (ProductType == "")
                {
                    ProductType = "0";
                }
            }
            DataSet ds = ProductComponent.GetProductDetailByID(Convert.ToInt32(ProductID), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2") // Made to Measure
                {
                    string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
                    string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
                    Int32 FabricTypeID = 0; if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                    {
                        FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                        if (FabricTypeID > 0)
                        {
                            DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                            Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                            Int32 OrderQty = Convert.ToInt32(Qty);

                            if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                    QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                    NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                            }
                            Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) FROM tb_ShoppingCartItems " +
                                        " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(HdnCustID.Value.ToString()) + " Order By ShoppingCartID) " +
                                        " AND ProductID=" + Convert.ToInt32(ProductID) + " AND VariantNames like '" + strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + strVariantValues.ToString().Replace("'", "''") + "%'"));
                            OrderQty = OrderQty + ShoppingCartQty;

                            TotalQty = QtyOnHand + NextOrderQty;

                            try
                            {
                                string Style = "";
                                double Width = 0;
                                double Length = 0;
                                string Options = "";
                                string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (strNmyard.Length > 0)
                                {
                                    if (strValyeard.Length == strNmyard.Length)
                                    {
                                        for (int j = 0; j < strNmyard.Length; j++)
                                        {
                                            if (strNmyard[j].ToString().ToLower() == "width")
                                            {
                                                if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
                                                {
                                                    double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Width);
                                                }
                                                else
                                                {
                                                    Width = Convert.ToDouble(strValyeard[j].ToString());
                                                }
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "length")
                                            {
                                                if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
                                                {
                                                    double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Length);
                                                }
                                                else { Length = Convert.ToDouble(strValyeard[j].ToString()); }
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "options")
                                            {
                                                Options = Convert.ToString(strValyeard[j].ToString());
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "header")
                                            {
                                                Style = Convert.ToString(strValyeard[j].ToString());
                                            }
                                        }
                                    }
                                }
                                //                string resp = "";
                                //                if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "" && Options != "")
                                //                {
                                //                    DataSet dsYard = new DataSet();
                                //                    dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                //                    if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                //                    {
                                //                        resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                //                        actualYard = Convert.ToDouble(resp.ToString());
                                //                    }
                                //                }
                                //                if (resp != "")
                                //                {
                                //                    OrderQty = Convert.ToInt32(OrderQty) * Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                //                }
                                //            }
                                //            catch
                                //            {
                                //            }
                                //            Yardqty = OrderQty;
                                //            if (TotalQty > 0 && OrderQty > TotalQty && isDropshipProduct == false)
                                //            {
                                //                outofstock = true;
                                //            }
                                //            else
                                //            {
                                //                if (OrderQty > QtyOnHand)
                                //                {
                                //                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && !dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().Contains("1900"))
                                //                    {
                                //                        Int32 TotalDays = 0;
                                //                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                                //                        {
                                //                            TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                                //                        }
                                //                        DateTime dtnew = Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()).AddDays(TotalDays);
                                //                        ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                //                    }

                                //                }
                                //                else
                                //                {
                                //                    Int32 TotalDays = 0;
                                //                    if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                                //                    {
                                //                        TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                                //                    }
                                //                    DateTime dtnew = DateTime.Now.Date.AddDays(TotalDays);
                                //                    ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                                string resp = "";
                                if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "")
                                {
                                    DataSet dsYard = new DataSet();
                                    dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                    if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                    {
                                        resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                        actualYard = Convert.ToDouble(resp.ToString());
                                    }
                                }
                                //if (resp != "")
                                //{
                                //    OrderQty = Convert.ToInt32(OrderQty) * Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                //}
                            }
                            catch
                            {
                            }
                            Yardqty = OrderQty;
                            if (!string.IsNullOrEmpty(strVariantValues.ToString().Trim()) && strVariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                            {
                                OrderQty = OrderQty * 2;
                            }

                            if (pInventory > 0 && OrderQty > pInventory && isDropshipProduct == false)
                            {
                                TotalQuantityValue = pInventory;
                            }

                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID + " "));

                            if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                            {
                                if (isDropshipProduct == false)
                                {
                                    outofstock = true;
                                }
                                ViewState["AvailableDate"] = StrVendor;
                            }
                            else
                            {
                                ViewState["AvailableDate"] = StrVendor;
                            }

                        }
                    }
                }
                else
                {
                    int customerid = 0;
                    customerid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select CustomerID from tb_Order where OrderNumber=" + Convert.ToInt32(Request.QueryString["ono"].ToString()) + " and isnull(tb_Order.Deleted,0)=0"));

                    //  if (CheckInvontryVar(ProductID, Qty, strVariantValues, strVariantNames, Convert.ToInt32(Request.QueryString["ono"].ToString())))
                    if (CheckInventory(Convert.ToInt32(ProductID), customerid, Convert.ToInt32(Qty), strVariantValues.ToString(), strVariantNames, ProductType))
                    { }
                    else
                    {
                        if (isDropshipProduct == false)
                        {
                            outofstock = true;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "1" && ViewState["AvailableDate"] == null) // Made to Measure
            {
                DateTime dtnew = DateTime.Now.Date.AddDays(12);
                ViewState["AvailableDate"] = Convert.ToString(dtnew);
            }
            if (outofstock)
            {
                if (TotalQuantityValue > 0)
                {
                    StrErrorMsg = @"Not Sufficient Inventory \n Available in our Warehouse : <span style=\""color:#641114 !important; font-weight:bold;\"">" + TotalQuantityValue.ToString() + "</span>";
                }
                else
                {
                    StrErrorMsg = "Not Sufficient Inventory";
                }
            }
            if (outofstock == false)
            {
                decimal LengthStdAllow = 0;

                String[] Names = strVariantNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                String[] Values = strVariantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string VariantNameId = "";
                string VariantValueId = "";
                string avail = "";
                if (strVariantNames != null && !string.IsNullOrEmpty(strVariantNames.ToString()))
                {
                    if (Names.Length > 0)
                    {
                        if (Values.Length == Names.Length)
                        {
                            //int RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(ProductID) + " and StoreId=1"));
                            int RomanShadeId = 0; //Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + " and StoreId=" + AppConfig.StoreID + ""));
                            for (int pp = 0; pp < Names.Length; pp++)
                            {
                                if (Names[pp].ToString().ToLower().IndexOf("roman shade design") > -1)
                                {
                                    RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + Values[pp].ToString().Trim() + "' AND isnull(Active,0)=1"));
                                    break;
                                }

                            }
                            decimal VariValue = 0, WidthStdAllow = 0;
                            for (int k = 0; k < Names.Length; k++)
                            {
                                VariantNameId = VariantNameId + Names[k].ToString() + ",";
                                VariantValueId = VariantValueId + Values[k].ToString() + ",";

                                // Roman Yardage ---------------------------------
                                //                if (RomanShadeId > 0 && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "3")
                                //                {
                                //                    if (Names[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                //                    {
                                //                        if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                //                        {
                                //                            decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out WidthStdAllow);
                                //                        }
                                //                        else
                                //                        {
                                //                            decimal.TryParse(Values[k].ToString(), out WidthStdAllow);
                                //                        }
                                //                    }
                                //                    if (Names[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                //                    {
                                //                        if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                //                        {
                                //                            decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out VariValue);
                                //                        }
                                //                        else
                                //                        {
                                //                            decimal.TryParse(Values[k].ToString(), out VariValue);
                                //                        }
                                //                        decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                //                        if (tempWidthStdAllow > 0)
                                //                        {
                                //                            LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                //                            LengthStdAllow = Math.Round(LengthStdAllow, 2);
                                //                        }
                                //                    }
                                //                }
                                //                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2")
                                //                {
                                //                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                //                    try
                                //                    {
                                //                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                //                    }
                                //                    catch
                                //                    {
                                //                    }
                                //                }
                                //                if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType == "1")
                                //                {
                                //                    if (avail == "")
                                //                    {
                                //                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductID + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                //                        if (avail == "")
                                //                        {
                                //                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductID + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }

                                //}

                                if (RomanShadeId > 0 && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "3")
                                {
                                    if (Names[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                    {
                                        //if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                        //{
                                        //    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out WidthStdAllow);
                                        //}
                                        //else
                                        //{
                                        //    decimal.TryParse(Values[k].ToString(), out WidthStdAllow);
                                        //}

                                        if (Values[k].ToString().IndexOf("/") > -1)
                                        {
                                            string strwidth = Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("/"));
                                            strwidth = strwidth.Replace("/", "");
                                            decimal tt = Convert.ToDecimal(strwidth);
                                            strwidth = Convert.ToString(Values[k].ToString()).Replace(strwidth + "/", "");
                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                            decimal WidthStdAllow1 = tt;
                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;
                                        }
                                        else
                                        {
                                            decimal WidthStdAllow1 = 0;
                                            decimal.TryParse(Values[k].ToString(), out WidthStdAllow1);
                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;

                                        }


                                    }
                                    if (VariValue > Convert.ToDecimal(1))
                                    {
                                        if (Names[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                        {
                                            //if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                            //{
                                            //    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out VariValue);
                                            //}
                                            //else
                                            //{
                                            //    decimal.TryParse(Values[k].ToString(), out VariValue);
                                            //}
                                            if (Values[k].ToString().IndexOf("/") > -1)
                                            {
                                                string strwidth = Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("/"));
                                                strwidth = strwidth.Replace("/", "");
                                                decimal tt = Convert.ToDecimal(strwidth);
                                                strwidth = Convert.ToString(Values[k].ToString()).Replace(strwidth + "/", "");
                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                decimal VariValue1 = tt;
                                                VariValue = VariValue + VariValue1;
                                            }
                                            else
                                            {
                                                decimal VariValue1 = 0;
                                                decimal.TryParse(Values[k].ToString(), out VariValue1);
                                                VariValue = VariValue + VariValue1;

                                            }

                                            decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                            //Page.ClientScript.RegisterStartupScript(pageNo.GetType(), "yardrequired", "alert('" + tempWidthStdAllow.ToString() + " " + WidthStdAllow.ToString() + " " + VariValue.ToString() + "');", true);
                                            string strFabricname = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShadeName from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + ""));
                                            if (tempWidthStdAllow > 0)
                                            {
                                                if (strFabricname.ToString().ToLower().Trim() == "casual")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "relaxed")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "soft fold")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));

                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "front slat")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(1.75 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                LengthStdAllow = Math.Round(LengthStdAllow, 2);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Names[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                        {
                                            if (Values[k].ToString().IndexOf("/") > -1)
                                            {
                                                string strwidth = Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("/"));
                                                strwidth = strwidth.Replace("/", "");
                                                decimal tt = Convert.ToDecimal(strwidth);
                                                strwidth = Convert.ToString(Values[k].ToString()).Replace(strwidth + "/", "");
                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                decimal VariValue1 = tt;
                                                VariValue = VariValue + VariValue1;
                                            }
                                            else
                                            {
                                                decimal VariValue1 = 0;
                                                decimal.TryParse(Values[k].ToString(), out VariValue1);
                                                VariValue = VariValue + VariValue1;

                                            }
                                        }
                                    }
                                }
                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2")
                                {
                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                    try
                                    {
                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType == "1")
                                {
                                    if (avail == "")
                                    {
                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductID + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                        if (avail == "")
                                        {
                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductID + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (LengthStdAllow > 0)
                {
                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LengthStdAllow)));
                    actualYard = Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(LengthStdAllow.ToString())));

                    //LengthStdAllow = LengthStdAllow * Convert.ToDecimal(Qty);
                    VariantNameId = VariantNameId + "Yardage Required,";
                    varinatvaluetemp = VariantValueId;
                    VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
                    DataSet dsvariant = new DataSet();
                    dsvariant = CommonComponent.GetCommonDataSet("SELECT isnull(Quantity,0) as Quantity,VariantValues FROM tb_ChangeOrderShoppingCartItems  WHERE OrderedShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_Order WHERE OrderNumber=" + Request.QueryString["ono"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + ProductID + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                    /// SELECT isnull(Quantity,0) FROM tb_ShoppingCartItems  WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Convert.ToInt32(HdnCustID.Value.ToString()) + " order By ShoppingCartID DESC) AND ProductID=" + ProductID + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'

                    Int32 ExistQty = 0;
                    if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                    {
                        ExistQty = Convert.ToInt32(dsvariant.Tables[0].Rows[0]["Quantity"].ToString());
                        VariantValueId = Convert.ToString(dsvariant.Tables[0].Rows[0]["VariantValues"].ToString());
                    }

                    ExistQty = ExistQty + Qty;

                    Yardqty = Yardqty * ExistQty;
                    LengthStdAllow = LengthStdAllow * Convert.ToDecimal(ExistQty);
                    varinatvaluetemp = varinatvaluetemp + LengthStdAllow.ToString() + ",";
                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + ProductID.ToString() + ",'" + strVariantNames.ToString().Replace("~hpd~", "-") + "','" + strVariantValues.ToString().Replace("~hpd~", "-") + "'"));

                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                    {
                        if (isDropshipProduct == false)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('Not Sufficient Inventory','Sorry!');", true);
                            return;
                        }
                    }
                }
                if (ViewState["AvailableDate"] != null)
                {
                    avail = Convert.ToString(ViewState["AvailableDate"]);
                    avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                }
                if (avail != "")
                {
                    VariantNameId = VariantNameId + "Estimated Delivery,";
                    VariantValueId = VariantValueId + avail.ToString() + ",";
                }

                decimal VariPrice = 0;
                foreach (string Value in Values)
                {
                    if (Value.ToString().IndexOf("(+$") > -1)
                    {
                        String CPrice = String.Empty;
                        CPrice = Value.Trim().Substring(Value.IndexOf("(+$") + 3);
                        CPrice = CPrice.Replace("(", "").Replace(")", "").Replace("+", "").Replace("$", "").Replace(",", "").Replace("\t", "").Replace(" ", "");
                        Decimal TempPrice = Decimal.Zero;
                        Decimal.TryParse(CPrice, out TempPrice);
                        VariPrice += TempPrice;
                        //price += TempPrice;
                    }
                    else if (Value.ToString().IndexOf("($") > -1)
                    {
                        String CPrice = String.Empty;
                        CPrice = Value.Trim().Substring(Value.IndexOf("($") + 2);
                        CPrice = CPrice.Replace("(", "").Replace(")", "").Replace("+", "").Replace("$", "").Replace(",", "").Replace("\t", "").Replace(" ", "");
                        Decimal TempPrice = Decimal.Zero;
                        Decimal.TryParse(CPrice, out TempPrice);
                        VariPrice += TempPrice;
                        //price += TempPrice;
                    }
                }

                if (VariPrice > 0)
                    price = VariPrice;
                if (Isorderswatch1 == 1)
                {
                    price = 0;
                }
                //if (CheckInvontryVar(ProductID, Qty, strVariantValues, strVariantNames, Convert.ToInt32(Request.QueryString["CustId"].ToString())))
                //{
                OrderComponent objorder = new OrderComponent();
                bool Result = false;

                string PName = "";
                DataSet dsChkProQty = new DataSet();
                dsChkProQty = CommonComponent.GetCommonDataSet("select ci.Direction,ci.Avail,ci.ProductID,p.Name,p.SKU,ci.[Quantity],ci.[Price] as SalePrice, " +
                                                               " isnull(ci.[VariantNames],'') as 'VariantNames', isnull(ci.[VariantValues],'') as 'VariantValues',ci.createdon  " +
                                                               "  from tb_ChangeOrderShoppingCartItems ci join tb_Product p on ci.ProductID=p.ProductID " +
                                                               "  where OrderNumber=" + Request.QueryString["ono"].ToString() + " order by createdon");

                if (dsChkProQty.Tables[0] != null && dsChkProQty.Tables[0].Rows.Count >= 1)
                {
                    for (Int32 i = 0; i < dsChkProQty.Tables[0].Rows.Count; i++)
                    {
                        PName = Convert.ToString(dsChkProQty.Tables[0].Rows[0]["name"]);
                        if (dsChkProQty.Tables[0].Rows[i]["ProductID"].ToString() == Convert.ToString(ProductID)
                            && (dsChkProQty.Tables[0].Rows[i]["VariantNames"].ToString() == Convert.ToString(VariantNameId) && dsChkProQty.Tables[0].Rows[i]["VariantValues"].ToString() == Convert.ToString(VariantValueId)))
                        {
                            finalQty += Convert.ToInt32(dsChkProQty.Tables[0].Rows[i]["Quantity"].ToString());
                        }
                    }
                }

                decimal DiscountPrice = 0, DicPrice = 0;
                if (HdnCustID.Value != null && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null))
                {
                    decimal.TryParse(Session["CustCouponCodeDiscount"].ToString().Trim().ToString(), out DiscountPrice);
                }
                if (DiscountPrice > 0)
                {
                    if (DiscountPrice >= 100)
                    {
                        decimal TempDis = Convert.ToDecimal((price * DiscountPrice) / 100);
                        DicPrice = TempDis;
                    }
                    if (DiscountPrice > 0)
                    {
                        decimal TempDis = Convert.ToDecimal((price * DiscountPrice) / 100);
                        DicPrice = price - TempDis;
                    }
                    DicPrice = Math.Round(DicPrice, 2);
                }



                if (finalQty == Qty)
                {
                    Result = Convert.ToBoolean(objorder.InsertCartItemChangeOrder(Convert.ToInt32(Request.QueryString["ono"].ToString()), ProductID, finalQty, price, VariantNameId, VariantValueId, DicPrice));
                }
                else
                {

                    Result = Convert.ToBoolean(objorder.UpdateCartItemChangeOrder(Convert.ToInt32(Request.QueryString["ono"].ToString()), ProductID, finalQty, VariantNameId, VariantValueId));
                  
                }
                if (ProductType == "3")
                {
                    //
                    CommonComponent.ExecuteCommonData("update tb_ChangeOrderShoppingCartItems set VariantValues='" + varinatvaluetemp + "' WHERE OrderNumber=" + Convert.ToInt32(Request.QueryString["ono"].ToString()) + " and ProductID=" + ProductID + " and VariantNames='" + VariantNameId.Replace("'", "''") + "' and VariantValues='" + VariantValueId.Replace("'", "''") + "' ");
                }
                if (Result == true)
                {
                    
                    string Query = "";


                    if (DiscountPrice > 0)
                    {
                        Query = "update tb_ChangeOrderedShoppingCart set SubTotal=(select sum(ISNULL(DiscountPrice,0)*Quantity) from tb_ChangeOrderShoppingCartItems where OrderNumber=" + Request.QueryString["ono"].ToString() + "),total=total-subtotal+(select sum(ISNULL(DiscountPrice,0)*Quantity) from tb_ChangeOrderShoppingCartItems where OrderNumber=" + Request.QueryString["ono"].ToString() + ") where OrderNumber=" + Request.QueryString["ono"].ToString();
                    }
                    else
                    {
                        Query = "update tb_ChangeOrderedShoppingCart set SubTotal=(select sum(Price*Quantity) from tb_ChangeOrderShoppingCartItems where OrderNumber=" + Request.QueryString["ono"].ToString() + "),total=total-subtotal+(select sum(Price*Quantity) from tb_ChangeOrderShoppingCartItems where OrderNumber=" + Request.QueryString["ono"].ToString() + ") where OrderNumber=" + Request.QueryString["ono"].ToString();
                    }
                    CommonComponent.ExecuteCommonData(Query);
                    CartGridDataBind();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('" + StrErrorMsg.ToString() + "','Sorry!');", true);
                }
                //}
                //else
                //{
                //    string strResult = "Not enough inventory for product " + PName.ToString();
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('" + strResult.ToString() + "','Sorry!');", true);
                //}
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@mgs01", "jAlert('" + StrErrorMsg.ToString() + "','Sorry!');", true);
                return;
            }
        }



        private bool CheckInventory(Int32 ProductID, Int32 CustomerId, Int32 Qty, string strVariantValues, string strVariantNames, string type)
        {
            if (!string.IsNullOrEmpty(strVariantValues.ToString().Trim()) && strVariantValues.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
            {
                Qty = Qty * 2;
            }

            Int32 AssemblyProduct = 0, ReturnQty = 0;
            AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where ProductId=" + ProductID + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
            if (AssemblyProduct > 0)
            {
                ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + ProductID + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + CustomerId + ",1"));
                TotalQuantityValue = ReturnQty;
                if (ReturnQty <= 0)
                {
                    return false;
                }
                else if (Qty > ReturnQty)
                {
                    return false;
                }
                else
                { return true; }
            }
            else
            {
                Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(sum(ISNULL(Quantity,0)),0) FROM tb_ChangeOrderShoppingCartItems " +
                                                     " WHERE OrderedShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_Order WHERE OrderNumber=" + Request.QueryString["ono"].ToString() + " order By ShoppingCartID DESC) " +
                                                     " AND ProductID=" + ProductID + " AND VariantNames like '" + strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + strVariantValues.Replace("'", "''").Replace("~hpd~", "-") + "%'"));
                Qty = Qty + ShoppingCartQty;
                DataSet dscount = new DataSet();
                Int32 alinv = 0;
                if (type == "1")
                {
                    string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (strValyeard.Length > 0)
                    {
                        if (strValyeard.Length == strNmyard.Length)
                        {
                            for (int j = 0; j < strNmyard.Length; j++)
                            {
                                if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                {
                                    if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                    {
                                        string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                        dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                                        alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                    }
                                    else
                                    {
                                        string strvalue = strValyeard[j].ToString().Trim();
                                        dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                                        alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {

                    dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
                }
                if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {

                    if (type == "1")
                    {
                        string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (strValyeard.Length > 0)
                        {
                            if (strValyeard.Length == strNmyard.Length)
                            {
                                for (int j = 0; j < strNmyard.Length; j++)
                                {
                                    if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                    {
                                        if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                        {
                                            string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                            int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                            TotalQuantityValue = Convert.ToInt32(CntInv);
                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProductID.ToString() + ""));
                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder) && TotalQuantityValue < Qty)
                                            {
                                                string resp = "";

                                                DataSet dsYard = new DataSet();
                                                string Length = "";
                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                {
                                                    Length = "84";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                {
                                                    Length = "96";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                {
                                                    Length = "108";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                {
                                                    Length = "120";
                                                }
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());

                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }

                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));

                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
                                                if (TotalQuantityValue < Qty)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string strvalue = strValyeard[j].ToString().Trim();
                                            int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                            TotalQuantityValue = Convert.ToInt32(CntInv);
                                            string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProductID.ToString() + ""));
                                            if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder) && TotalQuantityValue < Qty)
                                            {
                                                string resp = "";

                                                DataSet dsYard = new DataSet();
                                                string Length = "";
                                                if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                                                {
                                                    Length = "84";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                                                {
                                                    Length = "96";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                                                {
                                                    Length = "108";
                                                }
                                                else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                                                {
                                                    Length = "120";
                                                }
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }
                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));
                                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
                                                if (TotalQuantityValue < Qty)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    return true;

                                                }


                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }
                    else
                    {
                        int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                        TotalQuantityValue = Convert.ToInt32(CntInv);
                        if (type == "3")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
            }
        }



        /// <summary>
        /// Checks the inventory for variant.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Qty">int Qty</param>
        /// <param name="varinatvalue">string varinatvalue</param>
        /// <param name="VariantName">string VariantName</param>
        /// <param name="CustomerId">int CustomerId</param>
        /// <returns>true if Exists, false otherwise</returns>
        public bool CheckInvontryVar(Int32 ProductID, Int32 Qty, string varinatvalue, string VariantName, Int32 OrderNumber)
        {
            Int32 AssemblyProduct = 0, ReturnQty = 0;
            AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where Productid=" + ProductID + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
            if (AssemblyProduct > 0)
            {
                ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInv_Quote_ChangeOrder " + ProductID + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + ",0,1," + OrderNumber + ",1"));
                if (ReturnQty <= 0)
                {
                    return false;
                }
                else if (Qty > ReturnQty)
                {
                    return false;
                }
                else
                { return true; }
            }
            else
            {
                Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(SUM(ISNULL(Quantity,0)),0) FROM tb_ChangeOrderShoppingCartItems " +
                                                     "Where OrderNumber=" + OrderNumber + " AND ProductID=" + ProductID + " AND VariantNames='" + VariantName + "' AND VariantValues='" + varinatvalue + "'"));
                Qty = Qty + ShoppingCartQty;
                DataSet dscount = new DataSet();
                dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
                if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Bind the Cart Data into GridView
        /// </summary>
        private void CartGridDataBind()
        {
            if (Request.QueryString["Ono"] != null)
            {
                DataSet dsProduct = new DataSet();
                dsProduct = ChangeOrderComponent.GetChangeOrderCartByOrderNumber(Convert.ToInt32(Request.QueryString["Ono"].ToString()));
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    grdSelected.DataSource = dsProduct;
                    grdSelected.DataBind();
                    trSelectedproduct.Visible = true;
                    CalculateSubTotalPrice();
                    lbMsg.Text = "";
                    if (!string.IsNullOrEmpty(HdnCustID.Value) && Convert.ToInt32(HdnCustID.Value) > 0 && (Session["CustCouponCode"] != null && Session["CustCouponCodeDiscount"] != null) && grdSelected.Rows.Count > 0)
                    {
                        for (int i = 0; i < grdSelected.Rows.Count; i++)
                        {
                            Label lblCustomerCartId = (Label)grdSelected.Rows[i].FindControl("lblCustomerCartId");
                            Label lblProductID = (Label)grdSelected.Rows[i].FindControl("lblChnageOrderProductId");
                            Label lblVariantNames = (Label)grdSelected.Rows[i].FindControl("lblVariantNames");
                            Label lblVariantValues = (Label)grdSelected.Rows[i].FindControl("lblVariantValues");
                            Label lblOrginalDiscountPrice = (Label)grdSelected.Rows[i].FindControl("lblOrginalDiscountPrice");
                            decimal DiscountPrice = 0;
                            decimal.TryParse(lblOrginalDiscountPrice.Text.ToString().Trim(), out DiscountPrice);
                            CommonComponent.ExecuteCommonData("Update tb_ChangeOrderShoppingCartItems set DiscountPrice=" + DiscountPrice + " Where OrderNumber=" + Convert.ToInt32(Request.QueryString["Ono"].ToString()) + " and ProductID=" + lblProductID.Text.Trim() + " and VariantNames='" + lblVariantNames.Text.ToString() + "' and VariantValues='" + lblVariantValues.Text.ToString() + "'");
                        }
                    }
                }
                else
                {
                    grdSelected.DataSource = null;
                    grdSelected.DataBind();
                    trSelectedproduct.Visible = false;
                    lbMsg.Text = "Your Shopping Cart is Empty.";
                }
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
        /// Calculates the sub total price.
        /// </summary>
        private void CalculateSubTotalPrice()
        {
            lblTotal.Text = "0";
            if (grdSelected.Rows.Count > 0)
            {
                for (int i = 0; i < grdSelected.Rows.Count; i++)
                {
                    Label lblGridPrice = (Label)grdSelected.Rows[i].FindControl("lblGridPrice");
                    if (lblGridPrice != null && !string.IsNullOrEmpty(lblGridPrice.Text))
                    {
                        lblTotal.Text = Convert.ToString(Convert.ToDecimal(lblTotal.Text) + Convert.ToDecimal(lblGridPrice.Text));
                    }
                }
            }
        }

        /// <summary>
        /// Removes the data from add to cart.
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="VariantNames">String VariantNames</param>
        /// <param name="VariantValues">string VariantValues</param>
        private void RemoveDataFromAddToCart(Int32 OrderNumber, Int32 ProductID, String VariantNames, String VariantValues)
        {
            ChangeOrderComponent objchangeorder = new ChangeOrderComponent();
            bool Result = false;
            Result = Convert.ToBoolean(objchangeorder.DeleteChangeOrderCartItem(OrderNumber, ProductID, VariantNames, VariantValues));
            CartGridDataBind();
        }

        /// <summary>
        /// Clears the shopping Cart Button Event.
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
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('ContentPlaceHolder1_hfSubTotal').value='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML='" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML=parseFloat(parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').innerHTML)+parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtShippingCost').value)+parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtTax').value)-parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_TxtDiscount').value)).toFixed(2);window.opener.document.getElementById('ContentPlaceHolder1_hfTotal').value=window.opener.document.getElementById('ContentPlaceHolder1_lblTotal').innerHTML;window.opener.document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();window.close();", true);
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
                        ScriptManager.RegisterClientScriptBlock(btnSubmit, btnSubmit.GetType(), "Msg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_lblSubTotal').value = '" + lblTotal.Text.Trim() + "';window.opener.document.getElementById('ContentPlaceHolder1_btnshoppingcartitems').click();window.close();", true);
                    }
                }
            }
            catch { }

        }

        /// <summary>
        /// Gets the rounded sub total.
        /// </summary>
        /// <param name="Price">int Price</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns the Rounded sub total.</returns>
        public static Decimal GetSubTotal(Decimal Price, Int32 Qty)
        {
            return Math.Round((Price * Qty), 2);
        }

        /// <summary>
        ///  List Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvListProducts_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            gvListProducts.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// List Product Gridview Row Editing Event
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
        ///  Add to Cart With Variant Button Click Event
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

        /// <summary>
        /// Temp Button click for ShowVariant div
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btntempClick_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow1 = ((Button)sender).NamingContainer as GridViewRow;


            string strScc = "";
            foreach (GridViewRow clickedRow in gvListProducts.Rows)
            {
                //GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;

                CheckBox chk1 = (CheckBox)clickedRow.FindControl("chkSelect");
                Label lbl = (Label)clickedRow.FindControl("lblSKU1");
                Label lblProductID = (Label)clickedRow.FindControl("lblProductID");
                TextBox txtQty = (TextBox)clickedRow.FindControl("TxtQty");
                System.Web.UI.HtmlControls.HtmlAnchor avariantid = (System.Web.UI.HtmlControls.HtmlAnchor)clickedRow.FindControl("avariantid");
                System.Web.UI.HtmlControls.HtmlContainerControl divvariantid = (System.Web.UI.HtmlControls.HtmlContainerControl)clickedRow.FindControl("divvariant");
                System.Web.UI.HtmlControls.HtmlAnchor divinnerclose = (System.Web.UI.HtmlControls.HtmlAnchor)clickedRow.FindControl("divinnerclose");
                System.Web.UI.HtmlControls.HtmlContainerControl divAttributes = (System.Web.UI.HtmlControls.HtmlContainerControl)clickedRow.FindControl("divAttributes");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnVariantStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)clickedRow.FindControl("hdnVariantStatus");
                System.Web.UI.HtmlControls.HtmlInputHidden hdnitemprice = (System.Web.UI.HtmlControls.HtmlInputHidden)clickedRow.FindControl("hdnitemprice");
                Label lblvariantprice = (Label)clickedRow.FindControl("lblvariantprice");

                ImageButton btnAddtocartReady = (ImageButton)clickedRow.FindControl("btnAddtocartReady");
                btnAddtocartReady.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

                ImageButton btnAddtocartcustom = (ImageButton)clickedRow.FindControl("btnAddtocartcustom");
                btnAddtocartcustom.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

                ImageButton btnSelectGrid = (ImageButton)clickedRow.FindControl("btnSelectGrid");
                btnSelectGrid.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";
                Button btntempClick = (Button)clickedRow.FindControl("btntempClick");
                if (clickedRow1.RowIndex == clickedRow.RowIndex)
                {
                    strScc = "ShowVariantdiv('" + divvariantid.ClientID.ToString() + "');$('html, body').animate({ scrollTop: $('#" + divAttributes.ClientID.ToString() + "').offset().top }, 'slow');";
                }
                //if (GetVarinatByProductID(Convert.ToInt32(lblProductID.Text.ToString()), divAttributes, txtQty, btnSelectGrid, Convert.ToDecimal(hdnitemprice.Value.ToString()), hdnVariantStatus, lblvariantprice, Convert.ToInt32(clickedRow.RowIndex.ToString()), btntempClick))
                //{
                //    hdnVariantStatus.Value = "1";
                //}
                //else
                //{
                //    hdnVariantStatus.Value = "0";
                //    avariantid.InnerHtml = "";
                //}

                avariantid.Attributes.Add("onclick", "ShowVariantdiv('" + divvariantid.ClientID.ToString() + "');$('html, body').animate({ scrollTop: $('#" + divAttributes.ClientID.ToString() + "').offset().top }, 'slow');");

                divinnerclose.Attributes.Add("onclick", "HideVariantdiv('" + divvariantid.ClientID.ToString() + "');");
                if (Request.Browser.ToString().ToLower().IndexOf("firefox") > -1)
                {
                    clickedRow.Cells[5].Attributes.Add("style", "position:relative;");
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

            if (strScc != "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgReload", strScc, true);
            }
            hdnVariantValueid.Value = "";
        }

        protected string BindReadyMade(Int32 ProductId, Int32 RowIndex, ImageButton imgButton, bool IsRoman, System.Web.UI.HtmlControls.HtmlAnchor areadymade)
        {
            string strvarinat = "";
            string strWidthHeightvarinat = "";
            string strOptionvarinat = "";
            string strLiftvarinat = "";
            string strMountvarinat = "";
            Int32 CntReadymade = 0;
            if (areadymade.Visible == true)
            {
                CntReadymade = 1;
            }
            if (ProductId > 0)
            {
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + ProductId + " AND isnull(ParentId,0)=0 AND (ISNULL(VariantName,'') Not like '%Width%' AND ISNULL(VariantName,'') Not like '%length%') Order By DisplayOrder");
                string Stronchangevalue = "";
                if (IsRoman == true)
                {
                    Stronchangevalue = "onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\"";
                }
                else
                {
                    Stronchangevalue = "onchange=\"PriceChangeondropdown(" + RowIndex + ");\"";
                }

                strWidthHeightvarinat += "<div class=\"readymade-detail-pt1\">";
                strWidthHeightvarinat += "<div id=\"divvariantname-0\" class=\"readymade-detail-left\">Select Width</div>";
                strWidthHeightvarinat += "<input type=\"hidden\" value=\"Width\" id=\"hdnvariantname-0_" + RowIndex + "\">";
                strWidthHeightvarinat += "<div id=\"divSelectvariant-0\" style=\"border: 1px solid #E7E7E7; height: 57px; overflow-y: auto;padding: 0px 4px 8px;\" class=\"readymade-detail-right\">";
                Int32 ic = 0;
                strWidthHeightvarinat += "<div style=\"float: left;\">";
                for (int iWidth = 12; iWidth < 73; iWidth++)
                {
                    ic++;
                    if (iWidth == 12)
                    {
                        strWidthHeightvarinat += "<input type=\"radio\" value=\"" + iWidth.ToString() + "\" " + Stronchangevalue + " name=\"Selectvariant-0_" + RowIndex + "\" id=\"Selectvariant-000" + ic.ToString() + "\" checked=\"checked\" title=\"000" + ic.ToString() + "\">" + iWidth.ToString() + "<br />";
                    }
                    else
                    {
                        strWidthHeightvarinat += "<input type=\"radio\" value=\"" + iWidth.ToString() + "\" " + Stronchangevalue + " name=\"Selectvariant-0_" + RowIndex + "\" id=\"Selectvariant-000" + ic.ToString() + "\"  title=\"000" + ic.ToString() + "\">" + iWidth.ToString() + "<br />";
                    }
                }
                strWidthHeightvarinat += "</div>";

                strWidthHeightvarinat += "<div style=\"float: left;\">";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"0\" id=\"SelectvariantExtraWidthValue-0-1\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " checked=\"checked\" style=\"margin-left: 10px; width: auto ! important;\">00<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"1/8\" id=\"SelectvariantExtraWidthValue-0-2\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">1/8<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"1/4\" id=\"SelectvariantExtraWidthValue-0-3\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">1/4<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"3/8\" id=\"SelectvariantExtraWidthValue-0-4\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">3/8<br />";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"1/2\" id=\"SelectvariantExtraWidthValue-0-5\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">1/2<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"5/8\" id=\"SelectvariantExtraWidthValue-0-6\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">5/8<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"3/4\" id=\"SelectvariantExtraWidthValue-0-7\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">3/4<br />";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"7/8\" id=\"SelectvariantExtraWidthValue-0-8\" " + Stronchangevalue + " name=\"SelectvariantExtraWidthValue-select exact width_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">7/8<br />";

                strWidthHeightvarinat += "</div>";

                strWidthHeightvarinat += "</div>";
                strWidthHeightvarinat += "</div>";

                strWidthHeightvarinat += "<div class=\"readymade-detail-pt1\">";
                strWidthHeightvarinat += "<div id=\"divvariantname-9999999\" class=\"readymade-detail-left\">Select Length</div>";
                strWidthHeightvarinat += "<input type=\"hidden\" value=\"Length\" id=\"hdnvariantname-9999999_" + RowIndex + "\">";
                strWidthHeightvarinat += "<div id=\"divSelectvariant-9999999\" style=\"border: 1px solid #E7E7E7; height: 57px; overflow-y: auto;padding: 0px 4px 8px;\" class=\"readymade-detail-right\">";
                Int32 iL = 0;
                strWidthHeightvarinat += "<div style=\"float: left;\">";
                for (int ilength = 12; ilength < 73; ilength++)
                {
                    iL++;
                    if (ilength == 12)
                    {
                        strWidthHeightvarinat += "<input type=\"radio\" value=\"" + ilength.ToString() + "\" " + Stronchangevalue + " name=\"Selectvariant-9999999_" + RowIndex + "\" id=\"Selectvariant-9999999" + iL.ToString() + "\" checked=\"checked\" title=\"9999999" + iL.ToString() + "\">" + ilength.ToString() + "<br />";
                    }
                    else
                    {
                        strWidthHeightvarinat += "<input type=\"radio\" value=\"" + ilength.ToString() + "\" " + Stronchangevalue + " name=\"Selectvariant-9999999_" + RowIndex + "\" id=\"Selectvariant-9999999" + iL.ToString() + "\" title=\"9999999" + iL.ToString() + "\">" + ilength.ToString() + "<br />";
                    }
                }
                strWidthHeightvarinat += "</div>";

                strWidthHeightvarinat += "<div style=\"float: left;\">";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"0\" id=\"SelectvariantExtraLengthValue-9999999-1\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += "checked=\"checked\" style=\"margin-left: 10px; width: auto ! important;\">00<br>";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"1/8\" id=\"SelectvariantExtraLengthValue-9999999-2\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += "style=\"margin-left: 10px; width: auto ! important;\">1/8<br>";
                strWidthHeightvarinat += "<input type=\"radio\" value=\"1/4\" id=\"SelectvariantExtraLengthValue-9999999-3\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">1/4<br>";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"3/8\" id=\"SelectvariantExtraLengthValue-9999999-4\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">3/8<br>";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"1/2\" id=\"SelectvariantExtraLengthValue-9999999-5\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">1/2<br>";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"5/8\" id=\"SelectvariantExtraLengthValue-9999999-6\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">5/8<br>";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"3/4\" id=\"SelectvariantExtraLengthValue-9999999-7\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">3/4<br>";
                strWidthHeightvarinat += " <input type=\"radio\" value=\"7/8\" id=\"SelectvariantExtraLengthValue-9999999-8\" " + Stronchangevalue + " name=\"SelectvariantExtraLengthValue-select length_" + RowIndex + "\"";
                strWidthHeightvarinat += " style=\"margin-left: 10px; width: auto ! important;\">7/8<br>";

                strWidthHeightvarinat += "</div>";
                strWidthHeightvarinat += "</div>";
                strWidthHeightvarinat += "</div>";

                strOptionvarinat += "<div class=\"readymade-detail-pt1\">";
                strOptionvarinat += "<div id=\"divvariantname-200000\" class=\"readymade-detail-left\">Options</div>";
                strOptionvarinat += "<input type=\"hidden\" value=\"Options\" id=\"hdnvariantname-200000_" + RowIndex + "\">";
                strOptionvarinat += "<div id=\"divSelectvariant-200000\" style=\"border: 1px solid #E7E7E7; height: 57px; overflow-y: auto;padding: 0px 4px 8px;\" class=\"readymade-detail-right\">";
                strOptionvarinat += "<input type=\"radio\" value=\"Lined\" name=\"Selectvariant-200000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-200001\" checked=\"checked\" title=\"200001\">Lined<br />";
                strOptionvarinat += "<input type=\"radio\" value=\"Lined & Interlined\" name=\"Selectvariant-200000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-200002\" title=\"200002\">Lined & Interlined<br />";
                strOptionvarinat += "<input type=\"radio\" value=\"Blackout Lining\" name=\"Selectvariant-200000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-200003\" title=\"200003\">Blackout Lining<br />";
                strOptionvarinat += "</div>";
                strOptionvarinat += "</div>";

                strMountvarinat += "<div class=\"readymade-detail-pt1\">";
                strMountvarinat += "<div id=\"divvariantname-100000\" class=\"readymade-detail-left\">Mount</div>";
                strMountvarinat += "<input type=\"hidden\" value=\"Mount\" id=\"hdnvariantname-100000_" + RowIndex + "\">";
                strMountvarinat += "<div id=\"divSelectvariant-100000\" style=\"border: 1px solid #E7E7E7; height: 57px; overflow-y: auto;padding: 0px 4px 8px;\" class=\"readymade-detail-right\">";
                strMountvarinat += "<input type=\"radio\" value=\"Inside\" name=\"Selectvariant-100000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-100001\" checked=\"checked\" title=\"100001\">Inside<br />";
                strMountvarinat += "<input type=\"radio\" value=\"Outside\" name=\"Selectvariant-100000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-100002\" title=\"100002\">Outside<br />";
                strMountvarinat += "</div>";
                strMountvarinat += "</div>";
                strLiftvarinat = "";

                //strLiftvarinat += "<div class=\"readymade-detail-pt1\">";
                //strLiftvarinat += "<div id=\"divvariantname-9000000\" class=\"readymade-detail-left\">Lift</div>";
                //strLiftvarinat += "<input type=\"hidden\" value=\"Standard\" id=\"hdnvariantname-9000000_" + RowIndex + "\">";
                //strLiftvarinat += "<div id=\"divSelectvariant-9000000\" style=\"border: 1px solid #E7E7E7; height: 57px; overflow-y: auto;padding: 0px 4px 8px;\" class=\"readymade-detail-right\">";
                //strLiftvarinat += "<input type=\"radio\" value=\"Standard\" name=\"Selectvariant-9000000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-90000001\" checked=\"checked\" title=\"90000001\">Standard<br />";
                //strLiftvarinat += "<input type=\"radio\" value=\"Continuous Cord Loop\" name=\"Selectvariant-9000000_" + RowIndex + "\" " + Stronchangevalue + " id=\"Selectvariant-90000002\" title=\"90000002\">Continuous Cord Loop (+$" + string.Format("{0:0.00}", Convert.ToDecimal(AppLogic.AppConfigs("RomanLiftdefaultPrice").ToString())) + ")<br />";
                //strLiftvarinat += "</div>";
                //strLiftvarinat += "</div>";

                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    //strScriptVar += "function chkvariantvalues_" + ProductId.ToString() + "(RowIndex){";
                    //imgButton.OnClientClick = "return chkvariantvalues_" + ProductId.ToString() + "('" + RowIndex + "');";

                    strScriptVar += "function chkvariantvalues_" + ProductId.ToString() + "(){";
                    //imgButton.OnClientClick = "return chkvariantvalues_" + ProductId.ToString() + "();";
                    //imgButton.OnClientClick = "return chkAddtocart(" + RowIndex + ");";
                    if (IsRoman == true)
                    {
                        imgButton.OnClientClick = "return chkAddtocart(" + RowIndex + ",0);";
                    }
                    else { imgButton.OnClientClick = "return chkAddtocart(" + RowIndex + ",1);"; }
                    strvarinat += "<div id=\"divVariant_New\">";
                    bool checkmade = false;
                    bool isCustom = false;

                    Int32 Sid = 0;
                    for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    {
                        if (dsVariant.Tables[0].Rows.Count > 2 && i == 2 && IsRoman == true)
                        {
                            strvarinat += strWidthHeightvarinat + strOptionvarinat + strMountvarinat;
                        }
                        else if (dsVariant.Tables[0].Rows.Count == 2 && i == 1 && IsRoman == true)
                        {
                            strvarinat += strWidthHeightvarinat + strOptionvarinat + strMountvarinat;
                        }
                        else if (dsVariant.Tables[0].Rows.Count == 1 && i == 0 && IsRoman == true)
                        {
                            strvarinat += strWidthHeightvarinat + strOptionvarinat + strMountvarinat;
                        }

                        if (IsRoman == false && (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1 || dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                        {
                            continue;
                        }

                        bool isChild = false;
                        string strvarinatchild = "";
                        checkmade = false;
                        strvarinat += "<div class=\"readymade-detail-pt1\" >";
                        strvarinat += "<div class=\"readymade-detail-left\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" id=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                        DataSet dsVariantvalue = new DataSet();
                        dsVariantvalue = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + ProductId + " AND VariantID=" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + " Order By DisplayOrder");
                        if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                        {
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1 || dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1)
                            {
                                strvarinat += "  <div class=\"readymade-detail-right\" style=\"border: 1px solid #E7E7E7;height: 57px;overflow-y: auto;padding: 0px 4px 8px;\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'>###Wi_Div###";
                            }
                            else
                            {
                                strvarinat += "  <div class=\"readymade-detail-right\" style=\"border: 1px solid #E7E7E7;height: 57px;overflow-y: auto;padding: 0px 4px 8px;\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'>";
                            }

                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header") > -1)
                            {
                                //strScriptVar += "<input type=\"radio\" style=\"display:none;\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "<br />";
                            }
                            else
                            {
                                // strScriptVar += "<input type=\"radio\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "<br />";
                            }
                            //strvarinat += "<select ####onchange#### name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header") > -1)
                            {
                                //strvarinat += "<option value=\"0\" style=\"display:none;\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                            }
                            else
                            {
                                //strvarinat += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                            }

                            strScriptVar += "if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null &&  document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex==0)" + System.Environment.NewLine;
                            strScriptVar += "{" + System.Environment.NewLine;
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("select") > -1)
                            {
                                strScriptVar += "jAlert('Please " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                            }
                            else
                            {
                                strScriptVar += "jAlert('Please Select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                            }
                            strScriptVar += "return false;" + System.Environment.NewLine;
                            strScriptVar += "}" + System.Environment.NewLine;

                            Int32 iChk = 0;
                            for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                            {
                                string StrVarValueId = Convert.ToString(dsVariantvalue.Tables[0].Rows[j]["VariantValueId"]);
                                string StrQry = "";
                                Int32 Intcnt = 0;
                                string StrBuy1onsale = "";
                                bool IsOnSale = false;
                                decimal OnsalePrice = 0;

                                if (CntReadymade == 1)
                                {
                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (cast(Buy1Fromdate as date) <= cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        StrBuy1onsale = " (Buy 1 Get 1 Free)";
                                    }

                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        IsOnSale = true;
                                        OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + ProductId + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                        StrBuy1onsale += " (On Sale)";
                                    }
                                }


                                string strPrice = "";
                                if (IsOnSale == true && OnsalePrice > decimal.Zero)
                                {
                                    strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(OnsalePrice.ToString())) + ")";
                                }
                                else
                                {
                                    if (Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString()) > Decimal.Zero)
                                    {
                                        strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString())) + ")";
                                    }
                                }

                                if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                {
                                    checkmade = true;
                                    isCustom = true;
                                }
                                else
                                {

                                    if (iChk == 0)
                                    {
                                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("fabric color") > -1)
                                        {
                                            strvarinat += "<input type=\"radio\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####onchange#### checked=\"checked\" id=\"Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale+ "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale  + "<br />";
                                        }
                                        else
                                        {
                                            strvarinat += "<input type=\"radio\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####onchange#### checked=\"checked\" id=\"Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br />";
                                        }
                                    }
                                    else
                                    {
                                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("fabric color") > -1)
                                        {
                                            strvarinat += "<input type=\"radio\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####onchange#### id=\"Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + "<br />";
                                        }
                                        else
                                        {
                                            strvarinat += "<input type=\"radio\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####onchange#### id=\"Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br />";
                                        }
                                    }
                                    iChk++;
                                    //strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + strPrice + "</option>";
                                }
                                DataSet dsVariantparent = new DataSet();
                                dsVariantparent = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + ProductId + " AND isnull(ParentId,0)=" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "");
                                if (isChild == false && dsVariantparent.Tables.Count > 0 && dsVariantparent.Tables[0].Rows.Count > 0)
                                {
                                    strvarinat = strvarinat.Replace("####selectded####", " selected");
                                    if (strScriptreadymade == "")
                                    {
                                        if (IsRoman == true)
                                        {
                                            strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",1);";
                                        }
                                        else
                                        {
                                            strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",0);";
                                        }
                                        // strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + "," + IsRoman + ");";
                                    }
                                    if (IsRoman == true)
                                    {
                                        strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",1);\"");
                                    }
                                    else
                                    {
                                        strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",0);\"");
                                    }

                                    // strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + "," + IsRoman + ");\"");
                                    Sid = Convert.ToInt32(dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString());
                                    strvarinatchild = "";
                                    strvarinatchild += "<div class=\"readymade-detail-pt1\" >";
                                    strvarinatchild += "<div class=\"readymade-detail-left\" id=\"divvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</div><input type=\"hidden\" id=\"hdnvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "\" />";
                                    strvarinatchild += "  <div class=\"readymade-detail-right\" style=\"border: 1px solid #E7E7E7;height: 57px;overflow-y: auto;padding: 0px 4px 8px;\" id='divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'>";
                                    // strvarinatchild += "<select onchange=\"PriceChangeondropdown(" + RowIndex + ");\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" >";

                                    strScriptVar += "if(document.getElementById('Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "') != null &&  document.getElementById('Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "').selectedIndex==0)" + System.Environment.NewLine;
                                    strScriptVar += "{" + System.Environment.NewLine;
                                    if (dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString().ToLower().IndexOf("select") > -1)
                                    {
                                        strScriptVar += "jAlert('Please " + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                                    }
                                    else
                                    {
                                        strScriptVar += "jAlert('Please Select " + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString().Replace("'", @"\'") + "','Required information','Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "');" + System.Environment.NewLine;
                                    }
                                    strScriptVar += "return false;" + System.Environment.NewLine;
                                    strScriptVar += "}" + System.Environment.NewLine;


                                    //strvarinatchild += "<input type=\"radio\" value=\"0\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "<br />";
                                    //strvarinatchild += "<option value=\"0\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</option>";
                                    DataSet dsVariantvaluechild = new DataSet();
                                    dsVariantvaluechild = CommonComponent.GetCommonDataSet("SELECT *,isnull(AllowQuantity,0) as aQty,isnull(LockQuantity,0) as lQty FROM tb_ProductVariantValue WHERE ProductID=" + ProductId + " AND VariantID=" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + " order by DisplayOrder");
                                    if (dsVariantvaluechild != null && dsVariantvaluechild.Tables.Count > 0 && dsVariantvaluechild.Tables[0].Rows.Count > 0)
                                    {
                                        Int32 iChk1 = 0;
                                        for (int k = 0; k < dsVariantvaluechild.Tables[0].Rows.Count; k++)
                                        {
                                            IsOnSale = false;
                                            OnsalePrice = 0;
                                            if (CntReadymade == 1)
                                            {
                                                StrBuy1onsale = "";
                                                StrVarValueId = Convert.ToString(dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString());
                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    StrBuy1onsale = " (Buy 1 Get 1 Free)";
                                                }

                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    IsOnSale = true;
                                                    OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + ProductId + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1 order by DisplayOrder"));
                                                    StrBuy1onsale += " (On Sale)";
                                                }
                                            }

                                            strPrice = "";
                                            if (IsOnSale == true && OnsalePrice > decimal.Zero)
                                            {
                                                strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(OnsalePrice.ToString())) + ")";
                                            }
                                            else
                                            {
                                                if (Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString()) > Decimal.Zero)
                                                {
                                                    strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString())) + ")";
                                                }
                                            }
                                            if (dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                            {
                                                checkmade = true;
                                                isCustom = true;
                                            }
                                            else
                                            {
                                                int AllwQty = 0;
                                                Int32.TryParse(dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString(), out AllwQty);
                                                int LockQty = 0;
                                                Int32.TryParse(dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString(), out LockQty);
                                                string StrAlowLockqty = "";

                                                if (AllwQty > 0 && LockQty > 0)
                                                    StrAlowLockqty = "Allow Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString() + "</span>   Lock Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString() + "</span>";
                                                else if (AllwQty > 0)
                                                    StrAlowLockqty = "Allow Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString() + "</span>";
                                                else if (LockQty > 0)
                                                    StrAlowLockqty = "Lock Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString() + "</span>";
                                                else
                                                    StrAlowLockqty = "";

                                                if (iChk1 == 0)
                                                {
                                                    //if (IsRoman == true)
                                                    //{
                                                    //    strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    //}
                                                    //else
                                                    //{
                                                    //    strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    //}

                                                    if (IsRoman == true)
                                                    {
                                                        strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    }
                                                    else
                                                    {
                                                        strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");PriceChangeondropdownChangeAllow(" + RowIndex + "," + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    }
                                                }
                                                else
                                                {
                                                    //if (IsRoman == true)
                                                    //{
                                                    //    strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    //}
                                                    //else
                                                    //{
                                                    //    strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    //}

                                                    if (IsRoman == true)
                                                    {
                                                        strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    }
                                                    else
                                                    {
                                                        strvarinatchild += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");PriceChangeondropdownChangeAllow(" + RowIndex + "," + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                                    }
                                                }
                                                iChk1++;
                                                //strvarinatchild += "<option value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + strPrice + "</option>";
                                            }
                                        }
                                    }
                                    //strvarinatchild += "</select></div>";

                                    strvarinatchild += "</div>";
                                    strvarinatchild += "</div>";
                                    isChild = true;
                                }
                                else
                                {
                                    strvarinat = strvarinat.Replace("####selectded####", "");
                                    //if (isChild)
                                    //{
                                    //    if (Sid > 0)
                                    //    {
                                    //        strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + Sid.ToString() + "'," + RowIndex + "," + ProductId + "," + IsRoman + ");\"");
                                    //    }
                                    //    else
                                    //    {
                                    //        strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + "," + IsRoman + ");\"");
                                    //    }
                                    //    if (strScriptreadymade == "")
                                    //    {
                                    //        strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + "," + IsRoman + ");";
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //    if (IsRoman == true)
                                    //    {
                                    //        strvarinat = strvarinat.Replace("####onchange####", "onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\"");
                                    //    }
                                    //    else
                                    //    {
                                    //        strvarinat = strvarinat.Replace("####onchange####", "");
                                    //    }
                                    //}


                                    if (isChild)
                                    {
                                        if (Sid > 0)
                                        {
                                            if (IsRoman == true)
                                            {
                                                strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + Sid.ToString() + "'," + RowIndex + "," + ProductId + ",1);\"");
                                            }
                                            else
                                            {
                                                strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + Sid.ToString() + "'," + RowIndex + "," + ProductId + ",0);\"");
                                            }

                                        }
                                        else
                                        {
                                            if (IsRoman == true)
                                            {
                                                strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",1);\"");
                                            }
                                            else
                                            {
                                                strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",0);\"");
                                            }
                                        }
                                        if (strScriptreadymade == "")
                                        {
                                            if (IsRoman == true)
                                            {
                                                strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",1);";
                                            }
                                            else
                                            {
                                                strScriptreadymade = "sendData('Selectvariant-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'," + RowIndex + "," + ProductId + ",0);";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (IsRoman == true)
                                        {
                                            strvarinat = strvarinat.Replace("####onchange####", "onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\"");
                                        }
                                        else
                                        {
                                            strvarinat = strvarinat.Replace("####onchange####", "");
                                        }
                                    }
                                }
                            }
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1)
                            {
                                strvarinat = strvarinat.Replace("###Wi_Div###", "<div style=\"float:left;\">");
                                strvarinat += "</div><div style=\"float:left;\">";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\" checked=\"checked\" name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-1\" value=\"0\">00<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/8\">1/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/4\">1/4<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"3/8\">3/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/2\">1/2<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\" name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"5/8\">5/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"3/4\">3/4<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraWidthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"7/8\">7/8<br />";
                                strvarinat += "</div></div>";
                            }
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1)
                            {
                                strvarinat = strvarinat.Replace("###Wi_Div###", "<div style=\"float:left;\">");
                                strvarinat += "</div><div style=\"float:left;\">";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\" checked=\"checked\" name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-1\" value=\"0\">00<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/8\">1/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/4\">1/4<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"3/8\">3/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"1/2\">1/2<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\" name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"5/8\">5/8<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"3/4\">3/4<br />";
                                strvarinat += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "\" id=\"SelectvariantExtraLengthValue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "-2\" value=\"7/8\">7/8<br />";
                                strvarinat += "</div></div>";
                            }
                            //else { strvarinat += "</select><span><a title=\"Learn More\" href=\"javascript:void(0);\">Learn More</a></span></div>"; }
                            //else { strvarinat += "</select></div>"; }
                            else { strvarinat += "</div>"; }
                        }
                        strvarinat += "</div>";
                        strvarinat += strvarinatchild;
                        if (checkmade == true && isChild == false)
                        {
                        }
                        else
                        {
                        }
                    }
                    strvarinat += "</div>";
                    strScriptVar += "chkHeight(); return true;}";
                }
                else
                {
                    if (IsRoman == true)
                    {
                        imgButton.OnClientClick = "return chkAddtocart(" + RowIndex + ",0);";
                        strvarinat += "<div id=\"divVariant_New\">";
                        strvarinat += strWidthHeightvarinat + strOptionvarinat + strMountvarinat;
                        strvarinat += "</div>";
                    }
                    else { imgButton.OnClientClick = "return chkAddtocart(" + RowIndex + ",1);"; }
                }
            }
            else { return null; }
            if (IsRoman == true)
            {
                strvarinat = strvarinat + strLiftvarinat;
            }
            return strvarinat.ToString().Replace("###Wi_Div###", "");
        }

        protected void BindJaScriptforMeasure(ImageButton btnAddtocartcustom, Int32 ProductId, Int32 RowIndex)
        {
            strScriptVar += "function chkvariantvaluesmeasure_" + ProductId.ToString() + "(RowIndex){";
            btnAddtocartcustom.OnClientClick = "return chkAddtocartForMadetoMeasure(" + RowIndex + ");";

            strScriptVar += "if(document.getElementById('gvListProducts_ddlcustomstyle_" + RowIndex + "') != null &&  document.getElementById('gvListProducts_ddlcustomstyle_" + RowIndex + "').selectedIndex==0)" + System.Environment.NewLine;
            strScriptVar += "{" + System.Environment.NewLine;
            strScriptVar += "jAlert('Please select one','Required information','gvListProducts_ddlcustomstyle_" + RowIndex + "');" + System.Environment.NewLine;
            strScriptVar += "return false;" + System.Environment.NewLine;
            strScriptVar += "}" + System.Environment.NewLine;

            strScriptVar += "if(document.getElementById('gvListProducts_ddlcustomwidth_" + RowIndex + "') != null &&  document.getElementById('gvListProducts_ddlcustomwidth_" + RowIndex + "').selectedIndex==0)" + System.Environment.NewLine;
            strScriptVar += "{" + System.Environment.NewLine;
            strScriptVar += "jAlert('Please Select Width','Required information','gvListProducts_ddlcustomwidth_" + RowIndex + "');" + System.Environment.NewLine;
            strScriptVar += "return false;" + System.Environment.NewLine;
            strScriptVar += "}" + System.Environment.NewLine;

            strScriptVar += "if(document.getElementById('gvListProducts_ddlcustomlength_" + RowIndex + "') != null &&  document.getElementById('gvListProducts_ddlcustomlength_" + RowIndex + "').selectedIndex==0)" + System.Environment.NewLine;
            strScriptVar += "{" + System.Environment.NewLine;
            strScriptVar += "jAlert('Please Select Length','Required information','gvListProducts_ddlcustomlength_" + RowIndex + "');" + System.Environment.NewLine;
            strScriptVar += "return false;" + System.Environment.NewLine;
            strScriptVar += "}" + System.Environment.NewLine;

            strScriptVar += "if(document.getElementById('gvListProducts_ddlcustomoptin_" + RowIndex + "') != null &&  document.getElementById('gvListProducts_ddlcustomoptin_" + RowIndex + "').selectedIndex==0)" + System.Environment.NewLine;
            strScriptVar += "{" + System.Environment.NewLine;
            strScriptVar += "jAlert('Please Select Options','Required information','gvListProducts_ddlcustomoptin_" + RowIndex + "');" + System.Environment.NewLine;
            strScriptVar += "return false;" + System.Environment.NewLine;
            strScriptVar += "}" + System.Environment.NewLine;

            //strScriptVar += "if(document.getElementById('gvListProducts_dlcustomqty_" + RowIndex + "') != null &&  document.getElementById('gvListProducts_dlcustomqty_" + RowIndex + "').selectedIndex==0)" + System.Environment.NewLine;
            //strScriptVar += "{" + System.Environment.NewLine;
            //strScriptVar += "jAlert('Please Select Quantity (Panels)','Required information','gvListProducts_dlcustomqty_" + RowIndex + "');" + System.Environment.NewLine;
            //strScriptVar += "return false;" + System.Environment.NewLine;
            //strScriptVar += "}" + System.Environment.NewLine;

            strScriptVar += "if(document.getElementById('gvListProducts_txtMadetoMeasureQty_" + RowIndex + "') != null && (document.getElementById('gvListProducts_txtMadetoMeasureQty_" + RowIndex + "').value =='' || document.getElementById('gvListProducts_txtMadetoMeasureQty_" + RowIndex + "').value == 0))" + System.Environment.NewLine;
            strScriptVar += "{" + System.Environment.NewLine;
            strScriptVar += "jAlert('Please Enter Valid Quantity (Panels)','Required information','gvListProducts_txtMadetoMeasureQty_" + RowIndex + "');" + System.Environment.NewLine;
            strScriptVar += "return false;" + System.Environment.NewLine;
            strScriptVar += "}" + System.Environment.NewLine;

            strScriptVar += "chkHeight(); return true;}";
        }

        private void GetStyle(DropDownList ddlcustomstyle, Int32 productId, Int32 RowIndex)
        {
            string strcustomstyle = "";
            DataSet dsstyle = new DataSet();
            if (ViewState["Custom_Style"] == null)
            {
                dsstyle = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductSearchType WHERE isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND SearchType=6 Order By displayorder");
            }
            else
            {
                dsstyle = (DataSet)ViewState["Custom_Style"];
            }
            ddlcustomstyle.Items.Clear();
            if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
            {
                ViewState["Custom_Style"] = dsstyle;
                ddlcustomstyle.DataSource = dsstyle;
                ddlcustomstyle.DataTextField = "SearchValue";
                ddlcustomstyle.DataValueField = "SearchValue";
                ddlcustomstyle.DataBind();
                ddlcustomstyle.Items.Insert(0, new ListItem("Header", "0"));

                //for (int i = 0; i < dsstyle.Tables[0].Rows.Count; i++)
                //{
                //    if (i == 0)
                //        strcustomstyle += "<input type=\"radio\" onchange=\"ChangeCustomprice(" + productId.ToString() + "," + RowIndex + ");\" checked=\"checked\" style=\"margin-left: 10px;width: auto !important;\"  name=\"ddlcustomstyle" + RowIndex.ToString() + "\" id=\"rdocustomstyle-" + dsstyle.Tables[0].Rows[i]["SearchId"].ToString() + "\" value=\"" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + " <br />";
                //    else
                //        strcustomstyle += "<input type=\"radio\" onchange=\"ChangeCustomprice(" + productId.ToString() + "," + RowIndex + ");\" style=\"margin-left: 10px;width: auto !important;\"  name=\"ddlcustomstyle" + RowIndex.ToString() + "\" id=\"rdocustomstyle-" + dsstyle.Tables[0].Rows[i]["SearchId"].ToString() + "\" value=\"" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + " <br />";
                //    strScriptmadetomeasure = "ChangeCustomprice(" + productId.ToString() + "," + RowIndex + ");";
                //}
            }
            else
            {
                ddlcustomstyle.DataSource = null;
                ddlcustomstyle.DataBind();
                //strcustomstyle += "<input type=\"radio\" style=\"margin-left: 10px;width: auto !important;\"  name=\"ddlcustomstyle" + RowIndex.ToString() + "\" id=\"rdocustomstyle-0\" value=\"None\">None<br />";
            }
            /// ltrcustomstyle.Text = strcustomstyle.ToString();
        }

        /// <summary>
        /// Add to cart for Ready 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddVariant_Click(object sender, EventArgs e)
        {
            String strVariantNames = String.Empty;
            String strVariantValues = String.Empty;

            if (!string.IsNullOrEmpty(hdnVariProductId.Value) && !string.IsNullOrEmpty(hdnVariQuantity.Value) && !string.IsNullOrEmpty(hdnVariPrice.Value))
            {
                if (!string.IsNullOrEmpty(hdnVariName.Value.Trim()) && !string.IsNullOrEmpty(hdnVarivalue.Value.Trim()))
                {
                    string[] Names = Regex.Split(hdnVariName.Value.ToString().Trim(), "divvariantname=");
                    string[] Values = Regex.Split(hdnVarivalue.Value.ToString().Trim(), "divvariantname=");
                    for (int i = 0; i < Names.Length; i++)
                    {
                        if (Names.Length > 0 && !string.IsNullOrEmpty(Names[i].ToString().Trim()))
                        {
                            strVariantNames += Names[i].ToString().Trim();
                        }
                        if (Values.Length > 0 && !string.IsNullOrEmpty(Values[i].ToString().Trim()))
                        {
                            strVariantValues += Values[i].ToString().Trim();
                        }
                    }
                }
                AddTocart(Convert.ToInt32(hdnVariProductId.Value), (hdnVariQuantity.Value == "" ? 1 : Convert.ToInt32(hdnVariQuantity.Value)), Convert.ToDecimal(hdnVariPrice.Value.ToString()), strVariantValues, strVariantNames);
                CartGridDataBind();
                BindData();
            }
        }


        /// <summary>
        /// Add to cart for Made to Measure 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddVariantMeasure_Click(object sender, EventArgs e)
        {
            String strVariantNames = String.Empty;
            String strVariantValues = String.Empty;
            ViewState["IsCustom"] = "2";
            if (!string.IsNullOrEmpty(hdnVariProductId.Value) && !string.IsNullOrEmpty(hdnVariQuantity.Value) && !string.IsNullOrEmpty(hdnVariPrice.Value))
            {
                if (!string.IsNullOrEmpty(hdnforMeasureVName.Value.Trim()) && !string.IsNullOrEmpty(hdnforMeasureVValue.Value.Trim()))
                {
                    string[] Names = Regex.Split(hdnforMeasureVName.Value.ToString().Trim(), "divvariantname=");
                    string[] Values = Regex.Split(hdnforMeasureVValue.Value.ToString().Trim(), "divvariantname=");
                    for (int i = 0; i < Names.Length; i++)
                    {
                        if (Names.Length > 0 && !string.IsNullOrEmpty(Names[i].ToString().Trim()))
                        {
                            strVariantNames += Names[i].ToString().Trim();
                        }
                        if (Values.Length > 0 && !string.IsNullOrEmpty(Values[i].ToString().Trim()))
                        {
                            strVariantValues += Values[i].ToString().Trim();
                        }
                    }
                }
                Decimal inv = 0;
                if (hdnVariQuantity.Value == "")
                {
                    inv = 1;
                }
                else
                {
                    inv = Convert.ToDecimal(hdnVariQuantity.Value.ToString());
                }
                Decimal Price = Convert.ToDecimal(hdnVariPrice.Value.ToString()) / inv;
                AddTocart(Convert.ToInt32(hdnVariProductId.Value), (hdnVariQuantity.Value == "" ? 1 : Convert.ToInt32(hdnVariQuantity.Value)), Convert.ToDecimal(hdnVariPrice.Value.ToString()), strVariantValues, strVariantNames);
                CartGridDataBind();
                BindData();
                hdnVariPrice.Value = "0";
            }
        }

        /// <summary>
        /// Checks the Inventory.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="CustomerId">int CustomerId</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns true if sufficient Product Available, false otherwise</returns>
        private bool CheckInventory(Int32 ProductID, Int32 CustomerId, Int32 Qty)
        {
            Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) FROM tb_ShoppingCartItems " +
                                                 " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + CustomerId + ") " +
                                                 " AND ProductID=" + ProductID + ""));
            Qty = Qty + ShoppingCartQty;
            DataSet dscount = new DataSet();
            dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
            if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Getoptions(Int32 Productid, DropDownList ltroptions, Int32 RowIndex)
        {
            DataSet dsstyle = new DataSet();
            dsstyle = CommonComponent.GetCommonDataSet(@"SELECT ProductOptionsId,case when isnull(tb_ProductOptionsPrice.AdditionalPrice,0)=0 then tb_ProductOptionsPrice.Options else tb_ProductOptionsPrice.Options+'($'+cast(Round(tb_ProductOptionsPrice.AdditionalPrice,2) as varchar(10))+')'  end as name, tb_ProductOptionsPrice.ProductId, tb_ProductSearchType.DisplayOrder
                                                        FROM dbo.tb_ProductOptionsPrice INNER JOIN dbo.tb_ProductSearchType ON dbo.tb_ProductOptionsPrice.Options = dbo.tb_ProductSearchType.SearchValue WHERE tb_ProductOptionsPrice.ProductId=" + Productid.ToString() + @" Order By tb_ProductSearchType.DisplayOrder");
            if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
            {
                ltroptions.Items.Clear();
                ltroptions.DataSource = dsstyle;
                ltroptions.DataTextField = "Name";
                ltroptions.DataValueField = "Name";
                ltroptions.DataBind();
                ltroptions.Items.Insert(0, new ListItem("Options", "0"));
            }
            else
            {
            }
        }
    }
}