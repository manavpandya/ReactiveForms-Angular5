using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Solution.Data;
using Solution.Bussines.Components;


namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GenerateCriteoXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode productsNode = doc.CreateElement("products");
            doc.AppendChild(productsNode);


            DataSet dsData = new DataSet();
            dsData = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Product  WHERE Isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1 and isnull(ItemType,'') <> 'Roman' and ProductId in (SELECT ProductID FROM tb_Productcategory WHERE CategoryId in (SELECT CategoryId  FROM tb_Category WHERE StoreId=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0))");

            String strPath = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE Configname ='ImagePathProduct' and  Storeid=1 and isnull(Deleted,0)=0"));
            String strcontactPath = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE Configname ='Live_Contant_Server' and  Storeid=1 and isnull(Deleted,0)=0"));
            String strServer = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE Configname ='Live_Server' and  Storeid=1 and isnull(Deleted,0)=0"));
            strcontactPath = strcontactPath + strPath;
            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                {



                    XmlNode productNode = doc.CreateElement("product");
                    XmlAttribute productAttribute = doc.CreateAttribute("id");
                    productAttribute.Value = dsData.Tables[0].Rows[i]["SKU"].ToString();
                    productNode.Attributes.Append(productAttribute);
                    productsNode.AppendChild(productNode);

                    XmlNode nameNode = doc.CreateElement("name");
                    nameNode.AppendChild(doc.CreateTextNode(dsData.Tables[0].Rows[i]["Name"].ToString()));
                    productNode.AppendChild(nameNode);

                    XmlNode smallimage = doc.CreateElement("smallimage");
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["ImageName"].ToString()))
                    {
                        smallimage.AppendChild(doc.CreateTextNode(strcontactPath + "icon/" + dsData.Tables[0].Rows[i]["ImageName"].ToString()));
                    }
                    else
                    {
                        smallimage.AppendChild(doc.CreateTextNode(""));
                    }

                    productNode.AppendChild(smallimage);


                    XmlNode bigimage = doc.CreateElement("bigimage");
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["ImageName"].ToString()))
                    {
                        bigimage.AppendChild(doc.CreateTextNode(strcontactPath + "medium/" + dsData.Tables[0].Rows[i]["ImageName"].ToString()));
                    }
                    else
                    {
                        bigimage.AppendChild(doc.CreateTextNode(""));
                    }
                    productNode.AppendChild(bigimage);

                    XmlNode producturl = doc.CreateElement("producturl");
                    producturl.AppendChild(doc.CreateTextNode(strServer + "/" + dsData.Tables[0].Rows[i]["producturl"].ToString()));
                    productNode.AppendChild(producturl);

                    XmlNode description = doc.CreateElement("description");
                    description.AppendChild(doc.CreateTextNode(Convert.ToString(System.Text.RegularExpressions.Regex.Replace(dsData.Tables[0].Rows[i]["description"].ToString(), @"<[^>]*>", String.Empty)).Replace("<p>", "").Replace("</p>", "").Replace("<br>", "").Replace("</br>", "").Replace("<br />", "")));
                    productNode.AppendChild(description);



                    Decimal price = Convert.ToDecimal(dsData.Tables[0].Rows[i]["price"].ToString());
                    Decimal retailprice = Convert.ToDecimal(dsData.Tables[0].Rows[i]["saleprice"].ToString());
                    if(retailprice > Decimal.Zero)
                    {
                        if(price > retailprice)
                        {
                            price = retailprice;
                        }
                    }
                    XmlNode priceNode = doc.CreateElement("price");
                    priceNode.AppendChild(doc.CreateTextNode(string.Format("{0:0.00}", Convert.ToDecimal(price.ToString()))));
                    productNode.AppendChild(priceNode);

                    XmlNode retailpriceNode = doc.CreateElement("retailprice");
                    retailpriceNode.AppendChild(doc.CreateTextNode(string.Format("{0:0.00}", Convert.ToDecimal(price.ToString()))));
                    productNode.AppendChild(retailpriceNode);


                    XmlNode discountNode = doc.CreateElement("discount");
                    discountNode.AppendChild(doc.CreateTextNode("0"));
                    productNode.AppendChild(discountNode);

                    XmlNode recommendableNode = doc.CreateElement("recommendable");
                    recommendableNode.AppendChild(doc.CreateTextNode("1"));
                    productNode.AppendChild(recommendableNode);

                    Int32 inventory = 0;
                    Int32.TryParse(dsData.Tables[0].Rows[i]["inventory"].ToString(), out inventory);
                    XmlNode instockNode = doc.CreateElement("instock");
                    if (inventory > 0)
                    {
                        instockNode.AppendChild(doc.CreateTextNode("1"));
                    }
                    else if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Ismadetomeasure"].ToString()) && Convert.ToBoolean(dsData.Tables[0].Rows[i]["Ismadetomeasure"].ToString()))
                    {
                        instockNode.AppendChild(doc.CreateTextNode("1"));
                    }
                    else if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Ismadetoorder"].ToString()) && Convert.ToBoolean(dsData.Tables[0].Rows[i]["Ismadetoorder"].ToString()))
                    {
                        instockNode.AppendChild(doc.CreateTextNode("1"));
                    }
                    else
                    {
                        instockNode.AppendChild(doc.CreateTextNode("0"));
                    }

                    productNode.AppendChild(instockNode);

                }
            }


            doc.Save(Server.MapPath("/catalog.xml"));
        }
    }
}