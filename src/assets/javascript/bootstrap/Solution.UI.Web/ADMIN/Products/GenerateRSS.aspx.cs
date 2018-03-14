using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using StringBuilder = System.Text.StringBuilder;
using File = System.IO.File;
using StreamWriter = System.IO.StreamWriter;
using StringWriter = System.IO.StringWriter;
using XmlTextWriter = System.Xml.XmlTextWriter;
using XmlReader = System.Xml.XmlReader;
using XmlNodeType = System.Xml.XmlNodeType;
using SgmlReader = Sgml.SgmlReader;
using SqlCommand = System.Data.SqlClient.SqlCommand;
using SqlDataAdapter = System.Data.SqlClient.SqlDataAdapter;
using Formatting = System.Xml.Formatting;
using SqlDbType = System.Data.SqlDbType;
using CommandType = System.Data.CommandType;
using StreamReader = System.IO.StreamReader;
using WhitespaceHandling = System.Xml.WhitespaceHandling;
using StringReader = System.IO.StringReader;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GenerateRSS : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent objStorecomponent = new StoreComponent();
        ProductComponent ObjProduct = new ProductComponent();

        DataSet dsCategory = null;
        StringBuilder sitemap = new StringBuilder();
        String catCSSClass = String.Empty;
        String storeID = String.Empty;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnGenerate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/generate.png";
                bindstore();
            }
            btnGenerate.Attributes.Add("onclick", "return confirm('This will Delete Previously generated Products.xml file, Are you Sure?');");
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            String StoreID = "";
            if (ddlStore.SelectedIndex > 0)
                StoreID = ddlStore.SelectedValue.ToString();
            else
                StoreID = AppLogic.AppConfigs("StoreID").ToString();
        }

        /// <summary>
        /// Generate Xml file Button click event
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Argument</param>
        protected void btnGenerate_Click(object sender, ImageClickEventArgs e)
        {
            if (System.IO.File.Exists(Server.MapPath("~/Products.xml")))
                System.IO.File.Delete(Server.MapPath("~/Products.xml"));
            CreateRSS(AppLogic.AppConfigs("Live_Server").TrimEnd("/".ToCharArray()) + "/", AppLogic.AppConfigs("RSSTitle"), AppLogic.AppConfigs("RssDescription"), Convert.ToInt32(ddlStore.SelectedValue.ToString()));
        }

        /// <summary>
        /// Create Rss file in the Xml Format
        /// </summary>
        /// <param name="DomainName">string DomainName</param>
        /// <param name="Title">string Title</param>
        /// <param name="Description">string Description</param>
        /// <param name="StoreID">int StoreID</param>
        private void CreateRSS(string DomainName, string Title, string Description, int StoreID)
        {
            StringWriter streamw = new StringWriter();
            XmlTextWriter w = new XmlTextWriter(streamw);
            w.Formatting = Formatting.Indented;
            w.WriteStartDocument();

            DataSet Ds = new DataSet();
            Ds = ObjProduct.GenerateProductRSS(DomainName, ddlStore.SelectedValue.ToString());

            w.WriteProcessingInstruction("xml-stylesheet", " type=\"text/css\" href=\"#s1\" ");

            w.WriteStartElement("rss");
            w.WriteAttributeString("version", "2.0");
            w.WriteAttributeString("xmlns:g", "http://base.google.com/ns/1.0");
            w.WriteStartElement("channel");

            AddXmlEl(w, "title", Title);

            AddXmlEl(w, "link", DomainName);

            AddXmlEl(w, "description", Description);
            string itemDescription = string.Empty;
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                w.WriteStartElement("item", null);

                AddXmlEl(w, "g:id", Ds.Tables[0].Rows[i]["sku"].ToString());

                itemDescription = "<table cellspancing='3' width='100%'><tr><td valign='top' width='60' height='10'><img src='" + Ds.Tables[0].Rows[i]["ImageLink"].ToString().ToLower().Replace("large", "icon") + "' height='60' style='width: 100px;float:left;'></td>";
                itemDescription += "<td valign='top' height='10'>" + FilterTextFromHtml(Ds.Tables[0].Rows[i]["description"].ToString()) + "</td></tr></table>";
                AddXmlEl(w, "description", itemDescription);
                AddXmlEl(w, "title", Ds.Tables[0].Rows[i]["name"].ToString());
                AddXmlEl(w, "link", Ds.Tables[0].Rows[i]["url"].ToString());
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i]["saleprice"].ToString()))
                    AddXmlEl(w, "g:price", Ds.Tables[0].Rows[i]["saleprice"].ToString());
                else
                    AddXmlEl(w, "g:price", Ds.Tables[0].Rows[i]["price"].ToString());

                AddXmlEl(w, "g:weight", Ds.Tables[0].Rows[i]["Weight"].ToString());
                AddXmlEl(w, "g:model_number", Ds.Tables[0].Rows[i]["SKU"].ToString());
                AddXmlEl(w, "g:product_type", Ds.Tables[0].Rows[i]["ProductType"].ToString());
                AddXmlEl(w, "g:condition", "new");
                string ImageName = Ds.Tables[0].Rows[i]["productid"] + "_" + Ds.Tables[0].Rows[i]["sename"];
                AddXmlEl(w, "g:image_link", Ds.Tables[0].Rows[i]["ImageLink"].ToString());
                AddXmlEl(w, "g:payment_accepted", "Visa");
                AddXmlEl(w, "g:payment_accepted", "MasterCard");
                AddXmlEl(w, "g:payment_accepted", "Discover");
                AddXmlEl(w, "g:payment_accepted", "AmericanExpress");

                w.WriteEndElement();
            }
            w.WriteEndElement();
            w.WriteEndElement();
            w.WriteEndDocument();
            w.Close();
            String tmpstr = streamw.ToString().Replace("encoding=\"utf-16\"?>", "encoding=\"utf-8\"?>");
            bool Result = false;
            using (StreamWriter swrite = File.CreateText(Server.MapPath("~/Products.xml")))
            {
                swrite.Write(tmpstr.Trim());
                Result = true;
            }
            if (Result)
                Page.RegisterStartupScript("Msg", "<script type='text/javascript' lang='javascript'>alert('RSS file created Successfully!');</script>");
            else
                Page.RegisterStartupScript("Msg", "<script type='text/javascript' lang='javascript'>alert('Error creating RSS file. Please retry!');</script>");
        }
        
        /// <summary>
        /// Add Xml Element in the XmlTextWriter
        /// </summary>
        /// <param name="w">XmlTextWriter w</param>
        /// <param name="ElementName">string ElementName</param>
        /// <param name="ElementValue">string ElementValue</param>
        private void AddXmlEl(XmlTextWriter w, string ElementName, string ElementValue)
        {
            w.WriteElementString(ElementName, ElementValue);
        }

        /// <summary>
        /// Rules for the XML file generation
        /// </summary>
        /// <param name="html">string html</param>
        /// <returns>XmlReader object</returns>
        private XmlReader GetDocReader(string html)
        {
            SgmlReader r = new SgmlReader();
            r.WhitespaceHandling = WhitespaceHandling.None;
            r.DocType = @"HTML";
            r.InputStream = new StringReader(html);
            return r;
        }

        /// <summary>
        /// Filters the Text from HTML
        /// </summary>
        /// <param name="html">string HTML</param>
        /// <returns>Returns the output value as a string format which contains HTML.</returns>
        private string FilterTextFromHtml(string html)
        {
            XmlReader xml = GetDocReader(html);
            string Output = string.Empty;
            bool continueExec = true;
            try
            {
                continueExec = xml.Read();
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("GenerateRSS.aspx", ex.Message, ex.StackTrace);
            }
            while (continueExec)
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Text:
                        Output += xml.Value;
                        break;
                    case XmlNodeType.Element:
                        if (xml.Name.ToLower() == "img")
                            break;
                        if (xml.Name.ToLower() == "br")
                            break;
                        if (xml.Name.ToLower() == "span")
                            break;
                        if (xml.Name.ToLower() == "table")
                        {
                            Output += "</td></tr><tr><td colspan='2' valign='top'><table width='60%'>";
                            break;
                        }
                        if (xml.Name.ToLower() == "th")
                        {
                            Output += "<th align='left'>";
                            break;
                        }
                        Output += "<" + xml.Name.ToLower() + ">";
                        break;
                    case XmlNodeType.EndElement:
                        if (xml.Name.ToLower() == "br")
                            break;
                        if (xml.Name.ToLower() == "span")
                            break;
                        if (xml.Name.ToLower() == "p")
                        {
                            if (!Output.EndsWith("<p>"))
                                Output += "<br/>";
                            Output = Output.Replace("<p>", "");
                            break;
                        }
                        Output += "</" + xml.Name + ">";
                        break;

                }
                try
                {
                    continueExec = xml.Read();
                }
                catch (Exception ex)
                {
                    CommonComponent.ErrorLog("GenerateRSS.aspx", ex.Message, ex.StackTrace);
                }
            }
            return Output;
        }
    }
}