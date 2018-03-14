using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class ProductHead : System.Web.UI.UserControl
    {
        string StoreID = string.Empty;
        string URLMaterial = string.Empty;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            URLMaterial = Request.Url.Query;


            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    StoreID = Request.QueryString["StoreID"].ToString();
                    SetOnClientClickEvent(Convert.ToInt32(StoreID));
                    getOtherUrlImages(Convert.ToInt32(StoreID), true);
                }
            }
            MenubtnHPDOverstock.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHalfPriceFabrics.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHalfPriceShades.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDEBay.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDSears.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDBuycom.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDNewEgg.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDWayfailr.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDLNT.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
            MenubtnHPDBellacor.OnClientClick = "jAlert('Currently, Store is not live!','Information');return false;";
        }

        /// <summary>
        ///  Half Price Drapes Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPD_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPD.ImageUrl = "../images/MenubtnHPD-Red.png";
            GetandSetUrl("Product.aspx", "1");
            MenubtnHPD.ImageUrl = "../images/MenubtnHPD-Red.png";
            SetOnClientClickEvent(1);
        }

        /// <summary>
        ///  Half Price Drapes Yahoo Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDYahoo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDYahoo.ImageUrl = "../images/MenubtnHPDYahoo-Red.png";
            GetandSetUrl("ProductYahoo.aspx", "2");
            MenubtnHPDYahoo.ImageUrl = "../images/MenubtnHPDYahoo.png";
            SetOnClientClickEvent(2);
        }
        /// <summary>
        ///  Curtainsonbudget Amazon Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnCurtainsonbudgetAmazon_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnCurtainsonbudgetAmazon.ImageUrl = "../images/MenubtnCurtainsonbudgetAmazon-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "3");
            MenubtnCurtainsonbudgetAmazon.ImageUrl = "../images/MenubtnCurtainsonbudgetAmazon.png";
            SetOnClientClickEvent(3);
        }

        /// <summary>
        ///  HPD Overstock Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDOverstock_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDOverstock.ImageUrl = "../images/MenubtnHPDOverstock-Red.png";
            GetandSetUrl("ProductOverStock.aspx", "4");
            MenubtnHPDOverstock.ImageUrl = "../images/MenubtnHPDOverstock.png";
            SetOnClientClickEvent(4);
        }

        /// <summary>
        ///  Half Price Fabrics Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHalfPriceFabrics_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHalfPriceFabrics.ImageUrl = "../images/MenubtnHalfPriceFabrics-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "5");
            MenubtnHalfPriceFabrics.ImageUrl = "../images/MenubtnHalfPriceFabrics.png";
            SetOnClientClickEvent(5);
        }
        /// <summary>
        ///  Half Price Shades Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHalfPriceShades_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHalfPriceShades.ImageUrl = "../images/MenubtnHalfPriceShades-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "6");
            MenubtnHalfPriceShades.ImageUrl = "../images/MenubtnHalfPriceShades.png";
            SetOnClientClickEvent(6);
        }
        /// <summary>
        ///  HPD EBay Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDEBay_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDEBay.ImageUrl = "../images/MenubtnHPDEBay-Red.png";
            GetandSetUrl("ProductEBay.aspx", "7");
            MenubtnHPDEBay.ImageUrl = "../images/MenubtnHPDEBay.png";
            SetOnClientClickEvent(7);
        }

        /// <summary>
        ///  HPD Sears Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDSears_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDSears.ImageUrl = "../images/MenubtnHPDSears-Red.png";
            GetandSetUrl("ProductSears.aspx", "8");
            MenubtnHPDSears.ImageUrl = "../images/MenubtnHPDSears.png";
            SetOnClientClickEvent(8);
        }
        /// <summary>
        ///  HPD Buy.com Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDBuycom_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //MenubtnHPDBuycom.ImageUrl = "../images/MenubtnHPDBuycom-Red.png";
            //GetandSetUrl("ProductAmazon.aspx", "9");
            //MenubtnHPDBuycom.ImageUrl = "../images/MenubtnHPDBuycom.png";
            //SetOnClientClickEvent(9);
        }

        /// <summary>
        ///  HPD NewEgg Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDNewEgg_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //MenubtnHPDNewEgg.ImageUrl = "../images/MenubtnHPDNewEgg-Red.png";
            //GetandSetUrl("ProductAmazon.aspx", "10");
            //MenubtnHPDNewEgg.ImageUrl = "../images/MenubtnHPDNewEgg.png";
            //SetOnClientClickEvent(10);
        }
        /// <summary>
        ///  HPD Wayfailr Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDWayfailr_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDWayfailr.ImageUrl = "../images/MenubtnHPDWayfailr-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "11");
            MenubtnHPDWayfailr.ImageUrl = "../images/MenubtnHPDWayfailr.png";
            SetOnClientClickEvent(11);
        }
        /// <summary>
        ///  HPD LNT Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDLNT_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDLNT.ImageUrl = "../images/MenubtnHPDLNT-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "10");
            MenubtnHPDLNT.ImageUrl = "../images/MenubtnHPDLNT.png";
            SetOnClientClickEvent(12);
        }

        /// <summary>
        ///  HPD Bellacor Menu Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void MenubtnHPDBellacor_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MenubtnHPDBellacor.ImageUrl = "../images/MenubtnHPDBellacor-Red.png";
            GetandSetUrl("ProductAmazon.aspx", "9");
            MenubtnHPDBellacor.ImageUrl = "../images/MenubtnHPDBellacor.png";
            SetOnClientClickEvent(13);
        }
        protected void SetOnClientClickEvent(Int32 StoreID)
        {
            MenubtnHPD.OnClientClick = "";
            MenubtnHPDYahoo.OnClientClick = "";
            MenubtnCurtainsonbudgetAmazon.OnClientClick = "";
            MenubtnHPDOverstock.OnClientClick = "";
            MenubtnHalfPriceFabrics.OnClientClick = "";
            MenubtnHalfPriceShades.OnClientClick = "";
            MenubtnHPDEBay.OnClientClick = "";
            MenubtnHPDSears.OnClientClick = "";
            MenubtnHPDBuycom.OnClientClick = "";
            MenubtnHPDNewEgg.OnClientClick = "";
            MenubtnHPDWayfailr.OnClientClick = "";
            MenubtnHPDLNT.OnClientClick = "";
            MenubtnHPDBellacor.OnClientClick = "";
            try
            {
                if (StoreID == 1)
                {
                    MenubtnHPD.OnClientClick = "return false;";
                }
                else if (StoreID == 2)
                {
                    MenubtnHPDYahoo.OnClientClick = "return false;";
                }
                else if (StoreID == 3)
                {
                    MenubtnCurtainsonbudgetAmazon.OnClientClick = "return false;";
                }
                else if (StoreID == 4)
                {
                    MenubtnHPDOverstock.OnClientClick = "return false;";
                }
                else if (StoreID == 5)
                {
                    MenubtnHalfPriceFabrics.OnClientClick = "return false;";
                }
                else if (StoreID == 6)
                {
                    MenubtnHalfPriceShades.OnClientClick = "return false;";
                }
                else if (StoreID == 7)
                {
                    MenubtnHPDEBay.OnClientClick = "return false;";
                }
                else if (StoreID == 8)
                {
                    MenubtnHPDSears.OnClientClick = "return false;";
                }
                else if (StoreID == 9)
                {
                   // MenubtnHPDBuycom.OnClientClick = "return false;";
                    MenubtnHPDBellacor.OnClientClick = "return false;";
                }
                else if (StoreID == 10)
                {
                    MenubtnHPDLNT.OnClientClick = "return false;";
                }
                else if (StoreID == 11)
                {
                    MenubtnHPDWayfailr.OnClientClick = "return false;";
                }
                else if (StoreID == 12)
                {
                    //    MenubtnHPDLNT.OnClientClick = "return false;";  // KOHL
                }
                else if (StoreID == 13)
                {
                    //MenubtnHPDBellacor.OnClientClick = "return false;";
                }
            }
            catch { }
        }

        /// <summary>
        /// get Other Url Images function
        /// </summary>
        /// <param name="manuno">int manuno</param>
        /// <param name="Ispageload">bool Ispageload</param>
        private void getOtherUrlImages(int manuno, bool Ispageload)
        {
            try
            {
                StringBuilder js = new StringBuilder("");
                string currmenu = "0";
                MenubtnHPD.ImageUrl = "../images/MenubtnHPD.png";
                MenubtnHPDYahoo.ImageUrl = "../images/MenubtnHPDYahoo.png";
                MenubtnCurtainsonbudgetAmazon.ImageUrl = "../images/MenubtnCurtainsonbudgetAmazon.png";
                MenubtnHPDOverstock.ImageUrl = "../images/MenubtnHPDOverstock.png";
                MenubtnHalfPriceFabrics.ImageUrl = "../images/MenubtnHalfPriceFabrics.png";
                MenubtnHalfPriceShades.ImageUrl = "../images/MenubtnHalfPriceShades.png";
                MenubtnHPDEBay.ImageUrl = "../images/MenubtnHPDEBay.png";
                MenubtnHPDSears.ImageUrl = "../images/MenubtnHPDSears.png";
                MenubtnHPDBuycom.ImageUrl = "../images/MenubtnHPDBuycom.png";
                MenubtnHPDNewEgg.ImageUrl = "../images/MenubtnHPDNewEgg.png";
                MenubtnHPDWayfailr.ImageUrl = "../images/MenubtnHPDWayfailr.png";
                MenubtnHPDLNT.ImageUrl = "../images/MenubtnHPDLNT.png";
                MenubtnHPDBellacor.ImageUrl = "../images/MenubtnHPDBellacor.png";

                string pageurl = string.Empty;
                switch (manuno)
                {
                    case 1:
                        MenubtnHPD.ImageUrl = "../images/MenubtnHPD-red.png";
                        pageurl = "Product.aspx";
                        currmenu = "1";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu1", "currmenu=1;", true);
                        break;
                    case 2:
                        MenubtnHPDYahoo.ImageUrl = "../images/MenubtnHPDYahoo-red.png";
                        pageurl = "ProductYahoo.aspx";
                        currmenu = "2";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu2", "currmenu=2;", true);
                        break;
                    case 3:
                        MenubtnCurtainsonbudgetAmazon.ImageUrl = "../images/MenubtnCurtainsonbudgetAmazon-red.png";
                        pageurl = "ProductAmazon.aspx";
                        currmenu = "3";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu3", "currmenu=3;", true);
                        break;
                    case 4:
                        MenubtnHPDOverstock.ImageUrl = "../images/MenubtnHPDOverstock-red.png";
                        pageurl = "ProductOverStock.aspx";
                        currmenu = "4";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu4", "currmenu=4;", true);
                        break;
                    case 5:
                        MenubtnHalfPriceFabrics.ImageUrl = "../images/MenubtnHalfPriceFabrics-red.png";
                        pageurl = "ProductAmazon.aspx";
                        currmenu = "5";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu5", "currmenu=5;", true);
                        break;
                    case 6:
                        MenubtnHalfPriceShades.ImageUrl = "../images/MenubtnHalfPriceShades-red.png";
                        pageurl = "ProductAmazon.aspx";
                        currmenu = "6";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu6", "currmenu=6;", true);
                        break;
                    case 7:
                        MenubtnHPDEBay.ImageUrl = "../images/MenubtnHPDEBay-red.png";
                        pageurl = "ProductEBay.aspx";
                        currmenu = "7";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu7", "currmenu=7;", true);
                        break;
                    case 8:
                        MenubtnHPDSears.ImageUrl = "../images/MenubtnHPDSears-red.png";
                        pageurl = "ProductSears.aspx";
                        currmenu = "8";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu8", "currmenu=8;", true);
                        break;
                    case 9:
                        MenubtnHPDBuycom.ImageUrl = "../images/MenubtnHPDBellacor-red.png";
                        pageurl = "Product.aspx";
                        currmenu = "9";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu9", "currmenu=9;", true);
                        break;
                    case 10:
                        MenubtnHPDNewEgg.ImageUrl = "../images/MenubtnHPDLNT-red.png";
                        pageurl = "Product.aspx";
                        currmenu = "10";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu10", "currmenu=10;", true);
                        break;
                    case 11:
                        MenubtnHPDWayfailr.ImageUrl = "../images/MenubtnHPDWayfailr-red.png";
                        pageurl = "Product.aspx";
                        currmenu = "11";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu11", "currmenu=11;", true);
                        break;
                    case 12:
                        //MenubtnHPDLNT.ImageUrl = "../images/MenubtnHPDLNT-red.png"; // KOHL
                        //pageurl = "Product.aspx";
                        //currmenu = "12";
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu12", "currmenu=12;", true);
                        break;
                    case 13:
                        //MenubtnHPDBellacor.ImageUrl = "../images/MenubtnHPDBellacor-red.png";
                        //pageurl = "ProductAmazon.aspx";
                        //currmenu = "13";
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Curmenu13", "currmenu=13;", true);
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// function for Get and Set URL for Clone functionality
        /// </summary>
        /// <param name="PageName">string PageName</param>
        /// <param name="StoreId">string StoreId</param>
        private void GetandSetUrl(string PageName, string StoreId)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["Storeid"].ToString() != StoreId)
            {
                if (Request.QueryString["CloneID"] != null)
                {
                    Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["id"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                }
                else
                {
                    Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["id"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                }
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["Storeid"].ToString() == StoreId)
            {
                if (Request.QueryString["OLDID"] != null)
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                    }
                }
                else
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        try
                        {
                            Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                        }
                        catch (Exception EX)
                        {
                            Response.Redirect(PageName + "?Storeid=" + StoreId);
                        }
                    }
                }
            }
            else if (Request.QueryString["id"] == null && Request.QueryString["Storeid"] != null && Request.QueryString["Storeid"].ToString() != StoreId)
            {
                if (Request.QueryString["OLDID"] != null)
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                    }
                }
                else
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        try
                        {
                            Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                        }
                        catch (Exception ex)
                        {
                            Response.Redirect(PageName + "?Storeid=" + StoreId);
                        }
                    }
                }
            }
            else if (Request.QueryString["id"] == null && Request.QueryString["Storeid"] != null && Request.QueryString["Storeid"].ToString() == StoreId)
            {
                if (Request.QueryString["OLDID"] != null)
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["OLDID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                    }
                }
                else
                {
                    if (Request.QueryString["CloneID"] != null)
                    {
                        Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString() + "");
                    }
                    else
                    {
                        try
                        {
                            Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&OLDID=" + Request.QueryString["ID"].ToString() + "&CloneID=" + Request.QueryString["Storeid"].ToString() + "");
                        }
                        catch (Exception ex)
                        {
                            Response.Redirect(PageName + "?Storeid=" + StoreId);
                        }
                    }
                }
            }
            else if (Request.QueryString["OLDID"] != null)
            {
                Response.Redirect("" + PageName + "?Storeid=" + StoreId + "&Mode=edit&ID=" + Request.QueryString["OLDID"].ToString() + "");
            }
            else
            {
                //Response.Redirect(Request.Url.ToString());
                Response.Redirect(PageName + "?Storeid=" + StoreId);
            }
        }
    }
}