using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class PackingSlipMultiwarehouse : BasePage
    {
        Int32 StoreID = 0;
        int OrderNumber = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {

                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);

                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                GetSlip();
            }
        }

        private void GetSlip()
        {
            string url = "";
            DataSet dsPreferred = new DataSet();
            DataSet dswarehouse = new DataSet();
            dswarehouse = CommonComponent.GetCommonDataSet("SELECT WareHouseID FROM tb_WareHouse WHERE isnull(Active,0)=1 AND isnull(Deleted,0)=0 ");

            //string[] stringArray = new string[Convert.ToInt32(dswarehouse.Tables[0].Rows.Count.ToString())];
            string warehouseid = "";
            string warehouseproductid = "";
            string warehouseproductidTemp = "";
            string warehouseproductidTempIds = "";
            Int32 Ilenght = 1;
            if (dswarehouse != null && dswarehouse.Tables.Count > 0 && dswarehouse.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < dswarehouse.Tables[0].Rows.Count; k++)
                {
                    dsPreferred = CommonComponent.GetCommonDataSet(@"SELECT isnull(tb_WareHouseProductInventory.WareHouseID,0) as WareHouseID,dbo.tb_OrderedShoppingCartItems.RefProductID
FROM         dbo.tb_OrderedShoppingCartItems INNER JOIN
                      dbo.tb_Order ON dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID = dbo.tb_Order.ShoppingCardID INNER JOIN
                      dbo.tb_WareHouseProductInventory ON dbo.tb_OrderedShoppingCartItems.RefProductID = dbo.tb_WareHouseProductInventory.ProductID
                      WHERE tb_Order.orderNumber=" + OrderNumber.ToString() + " AND isnull(tb_WareHouseProductInventory.Inventory,0) > 0 AND isnull(tb_WareHouseProductInventory.WareHouseID,0)= " + dswarehouse.Tables[0].Rows[k]["WareHouseID"].ToString() + " order By tb_WareHouseProductInventory.WareHouseID");
                    if (dsPreferred != null && dsPreferred.Tables.Count > 0 && dsPreferred.Tables[0].Rows.Count > 0)
                    {
                        warehouseid = warehouseid + dswarehouse.Tables[0].Rows[k]["WareHouseID"].ToString() + ",";
                        Ilenght++;
                        if (warehouseproductid == "")
                        {
                            warehouseproductid += "0,";
                        }
                        else
                        {
                            warehouseproductid += ",";
                        }
                        for (int p = 0; p < dsPreferred.Tables[0].Rows.Count; p++)
                        {
                            warehouseproductid = warehouseproductid + dsPreferred.Tables[0].Rows[p]["RefProductID"].ToString() + "~";
                            warehouseproductidTemp = warehouseproductidTemp + dsPreferred.Tables[0].Rows[p]["RefProductID"].ToString() + ",";
                        }
                    }
                }
                if (warehouseid != "")
                {
                    Ilenght++;
                    warehouseid = "0," + warehouseid;
                }
            }

            string[] strAll = new string[Ilenght];
            string[] strAllprodcut = new string[Ilenght];
            if (warehouseid != "")
            {
                strAll = warehouseid.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                strAllprodcut = warehouseproductid.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            if (strAll != null && strAll.Length > 0)
            {

                warehouseproductidTemp = warehouseproductidTemp + "0";
                string strproduct = Convert.ToString(CommonComponent.GetScalarCommonData(@"SELECT cast (dbo.tb_OrderedShoppingCartItems.RefProductID as nvarchar(500))+'~'
  FROM dbo.tb_OrderedShoppingCartItems INNER JOIN
                      dbo.tb_Order ON dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID = dbo.tb_Order.ShoppingCardID  WHERE tb_Order.orderNumber=" + OrderNumber.ToString() + @" AND tb_OrderedShoppingCartItems.RefProductID not in (" + warehouseproductidTemp + ") FOR XML PATH('')"));
                if (strproduct.Length > 1)
                {
                    strAll[0] = "0";
                    strAllprodcut[0] = strproduct;
                }
            }
            else
            {

                warehouseproductidTemp = warehouseproductidTemp + "0";
                string strproduct = Convert.ToString(CommonComponent.GetScalarCommonData(@"SELECT cast (dbo.tb_OrderedShoppingCartItems.RefProductID as nvarchar(500))+'~'
   FROM dbo.tb_OrderedShoppingCartItems INNER JOIN
                      dbo.tb_Order ON dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID = dbo.tb_Order.ShoppingCardID  WHERE tb_Order.orderNumber=" + OrderNumber.ToString() + @" AND tb_OrderedShoppingCartItems.RefProductID not in (" + warehouseproductidTemp + ") FOR XML PATH('')"));
                if (strproduct.Length > 1)
                {
                    strAll[0] = "0";
                    strAllprodcut[0] = strproduct;
                }
            }

            if (strAll != null && strAll.Length > 0)
            {
                bool chk = false;
                for (int i = 0; i < strAll.Length; i++)
                {

                    if (strAllprodcut[i].ToString() != "0")
                    {
                        if (StoreID == 4)
                        {
                            url = "http://www.halfpricedrapes.us/Admin/Orders/OverStockPackingSlip.aspx?PId=" + strAllprodcut[i].ToString() + "&WareHouseId=" + strAll[i].ToString() + "&ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                        }
                        else if (StoreID == 3)
                        {
                            url = "http://www.halfpricedrapes.us/Admin/Orders/AmazonPackingSlip.aspx?PId=" + strAllprodcut[i].ToString() + "&WareHouseId=" + strAll[i].ToString() + "&ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));                            
                        }
                        else
                        {
                            url = "http://www.halfpricedrapes.us/Admin/Orders/PackingSlip.aspx?PId=" + strAllprodcut[i].ToString() + "&WareHouseId=" + strAll[i].ToString() + "&ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                        }
                        WebRequest NewWebReq = WebRequest.Create(url);
                        WebResponse newWebRes = NewWebReq.GetResponse();
                        string format = newWebRes.ContentType;
                        Stream ftprespstrm = newWebRes.GetResponseStream();
                        StreamReader reader;
                        reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                        string strbiody = reader.ReadToEnd().ToString();
                        ltMultiwarehouse.Text += "<div style='page-break-after: always;'>";
                        string[] stroverstock = Regex.Split(strbiody, "\"body12\"", RegexOptions.IgnoreCase);
                        if (stroverstock.Length > 0)
                        {
                            string strdata = stroverstock[1].ToString();
                            strdata = strdata.Substring(strdata.IndexOf("<table") - 6);
                            strdata = strdata.Substring(0, strdata.LastIndexOf("</table>") + 8);
                            //if (strAll.Length - 1 != i && chk == true)
                            //{
                                strdata = strdata.Replace("class=\"Printinvoice\"", "style=\"display:none;\"");
                            //}



                            ltMultiwarehouse.Text += strdata;
                        }
                        chk = true;
                        string strpackageslip = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT OrderNumber FROM tb_Order WHERE OrderNumber=" + OrderNumber.ToString() + " and (isnull(Transactionstatus,'')='canceled' or isnull(Orderstatus,'')='canceled')"));
                        if (!string.IsNullOrEmpty(strpackageslip))
                        {
                            ltMultiwarehouse.Text += "<img src=\"" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/images/watermark_canceled.png\" style=\"left:30%;top:30%;position:fixed;z-index:1000;\">";
                        }
                        
                        ltMultiwarehouse.Text += "</div>";
                    }
                }
            }
            else
            {
                if (StoreID == 4)
                {
                    url = "http://www.halfpricedrapes.us/Admin/Orders/OverStockPackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                }
                else if (StoreID == 3)
                {
                    url = "http://www.halfpricedrapes.us/Admin/Orders/AmazonPackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                }
                else
                {
                    url = "http://www.halfpricedrapes.us/Admin/Orders/PackingSlip.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                }
                WebRequest NewWebReq = WebRequest.Create(url);
                WebResponse newWebRes = NewWebReq.GetResponse();
                string format = newWebRes.ContentType;
                Stream ftprespstrm = newWebRes.GetResponseStream();
                StreamReader reader;
                reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                string strbiody = reader.ReadToEnd().ToString();
                ltMultiwarehouse.Text += "<div style='page-break-after: always;'>";
                string[] stroverstock = Regex.Split(strbiody, "\"body12\"", RegexOptions.IgnoreCase);
                if (stroverstock.Length > 0)
                {
                    string strdata = stroverstock[1].ToString();
                    strdata = strdata.Substring(strdata.IndexOf("<table") - 6);
                    strdata = strdata.Substring(0, strdata.LastIndexOf("</table>") + 8);
                    strdata = strdata.Replace("class=\"Printinvoice\"", "style=\"display:none;\"");
                    ltMultiwarehouse.Text += strdata;
                }

                ltMultiwarehouse.Text += "</div>";
            }

        }
    }
}