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
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductVariantBuy1OnsalePopup : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSaveValue.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save.gif";
                txtFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));

                if (Request.QueryString["varitype"] != null && Request.QueryString["varitype"].ToString().ToLower() == "buy1")
                {
                    lblPagetitle.Text = "Buy 1 Get 1 free Product Date";
                    chkIsBuyonsale.Text = "Is Buy 1 Get 1 Free ";
                    tdonsaleprice.Visible = true;
                }
                else
                {
                    lblPagetitle.Text = "OnSale Product Date";
                    chkIsBuyonsale.Text = "Is OnSale ";
                    tdonsaleprice.Visible = true;
                }
                string ProductId = "", VariId = "";
                if (Request.QueryString["ProductID"] != null)
                {
                    ProductId = Request.QueryString["ProductID"].ToString();
                }
                if (Request.QueryString["VariId"] != null)
                {
                    VariId = Request.QueryString["VariId"].ToString();
                }
                BindData(ProductId, VariId, Request.QueryString["varitype"].ToString());
            }
        }

        protected void BindData(string ProductId, string VariantValuId, string varitype)
        {
            DataSet dsVariantVal = new DataSet();
            dsVariantVal = CommonComponent.GetCommonDataSet("Select Isnull(OnSalePrice,0) as OnSalePrice, ISNULL(Buy1Get1,0) as Buy1Get1,ISNULL(OnSale,0) as OnSale,Buy1Fromdate,Buy1Todate,OnSaleFromdate,OnSaleTodate,isnull(Buy1Price,0) as Buy1Price,isnull(variantvalue,'') as variantvalue from tb_ProductVariantValue Where productid=" + ProductId + " and VariantValueID=" + VariantValuId + "");
            if (dsVariantVal != null && dsVariantVal.Tables.Count > 0 && dsVariantVal.Tables[0].Rows.Count > 0)
            {
                if (varitype.ToString().IndexOf("buy1") > -1)
                {
                    if (string.IsNullOrEmpty(dsVariantVal.Tables[0].Rows[0]["Buy1Fromdate"].ToString()) || dsVariantVal.Tables[0].Rows[0]["Buy1Fromdate"].ToString().IndexOf("1900") > -1)
                    {
                        txtFromDate.Text = "";
                    }
                    else
                        txtFromDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dsVariantVal.Tables[0].Rows[0]["Buy1Fromdate"].ToString()));

                    if (string.IsNullOrEmpty(dsVariantVal.Tables[0].Rows[0]["Buy1Todate"].ToString()) || dsVariantVal.Tables[0].Rows[0]["Buy1Todate"].ToString().IndexOf("1900") > -1)
                    {
                        txtToDate.Text = "";
                    }
                    else
                        txtToDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dsVariantVal.Tables[0].Rows[0]["Buy1Todate"].ToString()));

                    chkIsBuyonsale.Checked = Convert.ToBoolean(dsVariantVal.Tables[0].Rows[0]["Buy1Get1"].ToString());

                    if (!String.IsNullOrEmpty(dsVariantVal.Tables[0].Rows[0]["variantvalue"].ToString()) && dsVariantVal.Tables[0].Rows[0]["variantvalue"].ToString().ToLower().IndexOf("custom size") > -1)
                    {
                        decimal Buy1Price = 0;
                        tdonsaleprice.Visible = false;
                        txtOnsalePrice.Text = Buy1Price.ToString("f2");

                    }
                    else
                    {
                        decimal Buy1Price = 0;
                        decimal.TryParse(dsVariantVal.Tables[0].Rows[0]["Buy1Price"].ToString(), out Buy1Price);
                        txtOnsalePrice.Text = Buy1Price.ToString("f2");
                    }



                }
                else
                {
                    decimal OnSalePrice = 0;
                    decimal.TryParse(dsVariantVal.Tables[0].Rows[0]["OnSalePrice"].ToString(), out OnSalePrice);
                    txtOnsalePrice.Text = OnSalePrice.ToString("f2");

                    if (string.IsNullOrEmpty(dsVariantVal.Tables[0].Rows[0]["OnSaleFromdate"].ToString()) || dsVariantVal.Tables[0].Rows[0]["OnSaleFromdate"].ToString().IndexOf("1900") > -1)
                    {
                        txtFromDate.Text = "";
                    }
                    else
                        txtFromDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dsVariantVal.Tables[0].Rows[0]["OnSaleFromdate"].ToString()));
                    if (string.IsNullOrEmpty(dsVariantVal.Tables[0].Rows[0]["OnSaleTodate"].ToString()) || dsVariantVal.Tables[0].Rows[0]["OnSaleTodate"].ToString().IndexOf("1900") > -1)
                    {
                        txtToDate.Text = "";
                    }
                    else
                        txtToDate.Text = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dsVariantVal.Tables[0].Rows[0]["OnSaleTodate"].ToString()));

                    chkIsBuyonsale.Checked = Convert.ToBoolean(dsVariantVal.Tables[0].Rows[0]["OnSale"].ToString());
                }
            }
        }

        protected void btnSaveValue_Click(object sender, ImageClickEventArgs e)
        {

            if (Request.QueryString["varitype"] != null && Request.QueryString["varitype"].ToString().ToLower() == "buy1")
            {
                Int32 dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(OnSale,0)) as OnSale  from tb_ProductVariantValue Where productid=" + Request.QueryString["ProductID"] + " and isnull(OnSale,0)=1"));
                if (dsVariantVal2 > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Product is already as onsale', 'Message');", true);
                    return;
                }
            }
            else
            {


                Int32 dsVariantVal1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(Buy1Get1,0)) as Buy1Get1  from tb_ProductVariantValue Where productid=" + Request.QueryString["ProductID"] + " and isnull(Buy1Get1,0)=1"));
                if (dsVariantVal1 > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Product is already as Buy 1 Get 1', 'Message');", true);
                    return;
                }


            }
            //string StrFDate = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtFromDate.Text.ToString()));
            //string StrTDate = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtToDate.Text.ToString()));
            DateTime Fromdate = new DateTime();
            DateTime Todate = new DateTime();

            if (chkIsBuyonsale.Checked)
            {
                if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
                {
                    if (Convert.ToDateTime(txtFromDate.Text.ToString()) >= Convert.ToDateTime(txtToDate.Text.ToString()))
                    {

                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        //return;
                    }
                }
                else
                {
                    if (txtFromDate.Text.ToString() == "" && txtToDate.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                    else if (txtToDate.Text.ToString() == "" && txtFromDate.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                }


                try
                {
                    Fromdate = Convert.ToDateTime(txtFromDate.Text.ToString());
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

                try
                {
                    Todate = Convert.ToDateTime(txtToDate.Text.ToString());
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }

            if (Request.QueryString["ProductID"] != null)
            {
                string ProductId = Request.QueryString["ProductID"].ToString();
                string StoreID = Request.QueryString["StoreID"].ToString();
                string VariId = Request.QueryString["VariId"].ToString();
                if (Request.QueryString["varitype"].ToString().ToLower() == "buy1")
                {
                    string StrValue = Convert.ToString(Request.QueryString["clientid"].ToString()).Replace("arelatedbuy1get1", "imgBuy1get1");

                    DataSet DsPrevalue = new DataSet();
                    DsPrevalue = CommonComponent.GetCommonDataSet("select isnull(Buy1Get1,0) as Buy1Get1,isnull(Buy1Fromdate,'') as Buy1Fromdate,isnull(Buy1Todate,'') as Buy1Todate,isnull(sku,'') as sku,isnull(upc,'') as upc,isnull(Buy1Price,0) as Buy1Price from tb_ProductVariantValue Where ProductID=" + ProductId + " and VariantValueID=" + VariId + "");

                    if (chkIsBuyonsale.Checked)
                    {
                        decimal Buy1Price = 0;
                        decimal.TryParse(txtOnsalePrice.Text.ToString(), out Buy1Price);
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set Buy1Get1=1, Buy1Fromdate='" + Fromdate + "',Buy1Todate='" + Todate + "',Buy1Price=" + Buy1Price + " Where ProductID=" + ProductId + " and VariantValueID=" + VariId + "");
                        try
                        {
                            if (DsPrevalue != null && DsPrevalue.Tables.Count > 0 && DsPrevalue.Tables[0].Rows.Count > 0)
                            {
                                bool BeforeIsBuy1Get1 = false;
                                Decimal beforebuy1price = 0;
                                DateTime BeforeBuy1fromdate;
                                DateTime beforeBuy1todate;
                                BeforeIsBuy1Get1 = Convert.ToBoolean(DsPrevalue.Tables[0].Rows[0]["Buy1Get1"].ToString());

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Fromdate"].ToString(), out BeforeBuy1fromdate);

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Todate"].ToString(), out beforeBuy1todate);
                                Decimal.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Price"].ToString(), out beforebuy1price);



                                if (BeforeIsBuy1Get1 != true || BeforeBuy1fromdate.Date != Fromdate.Date || beforeBuy1todate.Date != Todate.Date || Buy1Price != beforebuy1price)
                                {
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + DsPrevalue.Tables[0].Rows[0]["sku"].ToString() + "','" + DsPrevalue.Tables[0].Rows[0]["upc"].ToString() + "',1,'" + Fromdate + "','" + Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Manual'," + Session["AdminID"].ToString() + "," + Buy1Price + "," + beforebuy1price + "");
                                }

                            }
                        }
                        catch
                        {

                        }
                        Page.ClientScript.RegisterStartupScript(btnSaveValue.GetType(), "@closemsg", "jAlert('Date Added Successfully.', 'Success');window.opener.document.getElementById('" + StrValue.ToString() + "').style.display = '';window.opener.document.getElementById('" + StrValue.ToString() + "').src = '/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png';window.close();", true);
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set Buy1Get1=0,Buy1Price=0, Buy1Fromdate=null,Buy1Todate=null Where ProductID=" + ProductId + " and VariantValueID=" + VariId + "");
                        try
                        {
                            if (DsPrevalue != null && DsPrevalue.Tables.Count > 0 && DsPrevalue.Tables[0].Rows.Count > 0)
                            {
                                bool BeforeIsBuy1Get1 = false;

                                DateTime BeforeBuy1fromdate;
                                DateTime beforeBuy1todate;
                                Decimal beforebuy1price = 0;
                                BeforeIsBuy1Get1 = Convert.ToBoolean(DsPrevalue.Tables[0].Rows[0]["Buy1Get1"].ToString());

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Fromdate"].ToString(), out BeforeBuy1fromdate);

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Todate"].ToString(), out beforeBuy1todate);
                                Decimal.TryParse(DsPrevalue.Tables[0].Rows[0]["Buy1Price"].ToString(), out beforebuy1price);

                                if (BeforeIsBuy1Get1 != false || BeforeBuy1fromdate.Date != Fromdate.Date || beforeBuy1todate.Date != Todate || Decimal.Zero != beforebuy1price)
                                {
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + DsPrevalue.Tables[0].Rows[0]["sku"].ToString() + "','" + DsPrevalue.Tables[0].Rows[0]["upc"].ToString() + "',0,'" + Fromdate + "','" + Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Manual'," + Session["AdminID"].ToString() + ",0," + beforebuy1price + "");
                                }

                            }
                        }
                        catch { }


                        Page.ClientScript.RegisterStartupScript(btnSaveValue.GetType(), "@closemsg", "jAlert('Date Added Successfully.', 'Success');window.opener.document.getElementById('" + StrValue.ToString() + "').style.display = 'none';window.opener.document.getElementById('" + StrValue.ToString() + "').src = '/App_Themes/" + Page.Theme.ToString() + "/images/isInactive.png';window.close();", true);
                    }
                    //window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').innerHTML = 'true';window.opener.document.getElementById('arelatedbuy1get1').value = 'true';", true);
                }
                else
                {

                    DataSet DsPrevalue = new DataSet();
                    DsPrevalue = CommonComponent.GetCommonDataSet("select isnull(OnSale,0) as OnSale,isnull(OnSaleFromdate,'') as OnSaleFromdate,isnull(OnSaleTodate,'') as OnSaleTodate,isnull(OnSalePrice,0) as OnSalePrice,isnull(sku,'') as sku,isnull(upc,'') as upc from tb_ProductVariantValue Where ProductID=" + ProductId + " and VariantValueID=" + VariId + "");


                    decimal OnSalePrice = 0;
                    string StrValue = Convert.ToString(Request.QueryString["clientid"].ToString()).Replace("arelatedonsale", "imgonsale");
                    if (chkIsBuyonsale.Checked)
                    {
                        decimal.TryParse(txtOnsalePrice.Text.ToString(), out OnSalePrice);
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set OnSalePrice=" + OnSalePrice + ",OnSale=1, OnSaleFromdate='" + Fromdate + "',OnSaleTodate='" + Todate + "' Where ProductID= " + ProductId + " and VariantValueID=" + VariId + "");




                        try
                        {

                            if (DsPrevalue != null && DsPrevalue.Tables.Count > 0 && DsPrevalue.Tables[0].Rows.Count > 0)
                            {
                                bool BeforeOnSale = false;

                                DateTime Beforeonsalefromdate;
                                DateTime beforeonsaletodate;
                                BeforeOnSale = Convert.ToBoolean(DsPrevalue.Tables[0].Rows[0]["OnSale"].ToString());

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSaleFromdate"].ToString(), out Beforeonsalefromdate);

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSaleTodate"].ToString(), out beforeonsaletodate);


                                Decimal beforeonsaleprice = Decimal.Zero;
                                Decimal.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSalePrice"].ToString(), out beforeonsaleprice);
                                if (BeforeOnSale != true || Beforeonsalefromdate.Date != Fromdate.Date || beforeonsaletodate.Date != Todate || beforeonsaleprice != OnSalePrice)
                                {
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + DsPrevalue.Tables[0].Rows[0]["sku"].ToString() + "','" + DsPrevalue.Tables[0].Rows[0]["upc"].ToString() + "',1,'" + Fromdate + "','" + Todate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Manual'," + Session["AdminID"].ToString() + "," + OnSalePrice + "," + beforeonsaleprice + "");
                                }

                            }
                        }
                        catch { }






                        Page.ClientScript.RegisterStartupScript(btnSaveValue.GetType(), "@closemsg", "jAlert('Date Added Successfully.', 'Success');window.opener.document.getElementById('" + StrValue.ToString() + "').style.display = '';window.opener.document.getElementById('" + StrValue.ToString() + "').src = '/App_Themes/" + Page.Theme.ToString() + "/images/isActive.png';window.close();", true);
                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("Update tb_ProductVariantValue set OnSalePrice=0,OnSale=0, OnSaleFromdate=null,OnSaleTodate=null Where ProductID= " + ProductId + " and VariantValueID=" + VariId + "");
                        try
                        {
                            if (DsPrevalue != null && DsPrevalue.Tables.Count > 0 && DsPrevalue.Tables[0].Rows.Count > 0)
                            {
                                bool BeforeOnSale = false;

                                DateTime Beforeonsalefromdate;
                                DateTime beforeonsaletodate;
                                BeforeOnSale = Convert.ToBoolean(DsPrevalue.Tables[0].Rows[0]["OnSale"].ToString());

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSaleFromdate"].ToString(), out Beforeonsalefromdate);

                                DateTime.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSaleTodate"].ToString(), out beforeonsaletodate);


                                Decimal beforeonsaleprice = Decimal.Zero;
                                Decimal.TryParse(DsPrevalue.Tables[0].Rows[0]["OnSalePrice"].ToString(), out beforeonsaleprice);

                                if (BeforeOnSale != false || Beforeonsalefromdate.Date != Fromdate.Date || beforeonsaletodate.Date != Todate || beforeonsaleprice != OnSalePrice)
                                {
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + DsPrevalue.Tables[0].Rows[0]["sku"].ToString() + "','" + DsPrevalue.Tables[0].Rows[0]["upc"].ToString() + "',0,'" + Fromdate + "','" + Todate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Manual'," + Session["AdminID"].ToString() + "," + OnSalePrice + "," + beforeonsaleprice + "");
                                }

                            }
                        }
                        catch { }



                        Page.ClientScript.RegisterStartupScript(btnSaveValue.GetType(), "@closemsg", "jAlert('Date Added Successfully.', 'Success');window.opener.document.getElementById('" + StrValue.ToString() + "').style.display = 'none';window.opener.document.getElementById('" + StrValue.ToString() + "').src = '/App_Themes/" + Page.Theme.ToString() + "/images/isInactive.png';window.close();", true);
                    }
                }
            }
        }
    }
}