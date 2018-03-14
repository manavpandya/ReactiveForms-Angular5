using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class WarehouseGeneratePO : BasePage
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

                if (Request.QueryString["PID"] != null)
                    BindCart(Request.QueryString["PID"].ToString());
                btnPreview.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/preview_warehouse_po.gif";
                btnGeneratePDFPO.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/generate-purchase-order.gif";
                btnGeneratePDFPO.Visible = false;
                lblstaticpdfcollection.Visible = false;
            }
            else
            {
                String[] formkeys = Request.Form.AllKeys;
                foreach (String s in formkeys)
                {
                    if (s.Contains("btnDownload-"))
                    {
                        try
                        {
                            String p = Request.Form.Get(s);
                            downloadfile(p);
                        }
                        catch (Exception ex)
                        {
                            // CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :-> cart() - bt_Delete  \r\n Date: " + System.DateTime.Now + "\r\n");
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Binds the Cart for Warehouse PO
        /// </summary>
        /// <param name="PId">string PId</param>
        private void BindCart(string PId)
        {
            if (PId.IndexOf(",") > -1)
            {
                PId = PId.Substring(0, PId.Length - 1);
            }
            DataSet dsBindCart = new DataSet();
            dsBindCart = CommonComponent.GetCommonDataSet("SELECT * FROM tb_WareHouseproduct WHERE productid in (" + PId + ")");
            if (dsBindCart != null && dsBindCart.Tables.Count > 0 && dsBindCart.Tables[0].Rows.Count > 0)
            {
                grdCart.DataSource = dsBindCart;
                grdCart.DataBind();
                BindVendors();
            }
            else
            {
                grdCart.DataSource = null;
                grdCart.DataBind();
            }
        }

        /// <summary>
        /// Binds the Vendors
        /// </summary>
        private void BindVendors()
        {
            ddlVendor.Items.Clear();
            int storeid = AppConfig.StoreID;
            if (storeid == 0)
                storeid = 1;
            DataSet dsVen = new DataSet();
            dsVen = CommonComponent.GetCommonDataSet("select VendorId AS id,tb_Vendor.* From tb_Vendor where ISNULL(tb_Vendor.Deleted,0) = 0 and ISNULL(tb_Vendor.IsDropshipper,0) = 1 order by tb_Vendor.VendorId");
            ddlVendor.DataSource = dsVen;
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataValueField = "id";
            ddlVendor.DataBind();
            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select Vendor", "0");
            li.Selected = true;
            ddlVendor.Items.Insert(0, li);
            ddlVendor.SelectedItem.Text = "Select Vendor";
        }

        /// <summary>
        /// Binds the vendors mail format.
        /// </summary>
        /// <param name="VendorId">int VendorId</param>
        private void BindVendorsMailFormat(Int32 VendorId)
        {
            Int32 storeid = 0;
            storeid = AppConfig.StoreID;
            if (storeid == 0)
                storeid = 1;
            DataSet dsVenMail = new DataSet();
            //dsVenMail = CommonComponent.GetCommonDataSet("select EmailTemplate as TemplateID,tb_EmailTemplate.Label as  Template from tb_vendor Inner Join tb_EmailTemplate on tb_vendor.EmailTemplate=tb_EmailTemplate.TemplateID where Vendorid=" + VendorId + " and EmailTemplate in (Select TemplateID from tb_EmailTemplate where StoreID=1)  and ISNULL(Active,0)=1 and ISNULL(deleted,0)=0 ");
            //dsVenMail = CommonComponent.GetCommonDataSet("Select TemplateID,Label as  Template from tb_EmailTemplate where StoreID=" + storeid.ToString() + "");
            dsVenMail = CommonComponent.GetCommonDataSet("Select TemplateID,Label as  Template from tb_EmailTemplate where StoreID=" + storeid.ToString() + " and ISNULL(IsPOTemplate,0)=1 ");
            ddlMailTemplate.DataTextField = "Template";
            ddlMailTemplate.DataValueField = "TemplateID";
            ddlMailTemplate.DataSource = dsVenMail;
            ddlMailTemplate.DataBind();
        }

        /// <summary>
        /// Vendor Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVendor.SelectedValue.ToString() != "0")
            {
                if (ddlVendor.SelectedItem.Text != "Shelter Logic/ Shelterworks")
                {
                    btnPreview.Visible = true;
                    lblmailtemplate.Visible = true;
                    ddlMailTemplate.Visible = true;
                    lblspeicalinstructions.Visible = true;
                    txtDescription.Visible = true;
                    btnGeneratePDFPO.Visible = false;
                    lblstaticpdfcollection.Visible = false;



                    try
                    {
                        //Int32 templateid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Vendorid,EmailTemplate from tb_vendor where Vendorid=" + ddlVendor.SelectedValue.ToString() + " and ISNULL(Active,1)=1 and ISNULL(deleted,0)=0 "));
                        //ddlMailTemplate.SelectedIndex = -1;
                        //ddlMailTemplate.Items.FindByValue(templateid.ToString()).Selected = true;
                        BindVendorsMailFormat(Convert.ToInt32(ddlVendor.SelectedValue));
                    }
                    catch { }
                }
                else
                {
                    btnPreview.Visible = false;
                    lblmailtemplate.Visible = false;
                    ddlMailTemplate.Visible = false;
                    lblspeicalinstructions.Visible = false;
                    txtDescription.Visible = false;
                    btnGeneratePDFPO.Visible = true;


                }
            }
            else
            {
                btnPreview.Visible = true;
                lblmailtemplate.Visible = true;
                ddlMailTemplate.Visible = true;
                lblspeicalinstructions.Visible = true;
                txtDescription.Visible = true;
                ddlMailTemplate.Items.Clear();
                btnGeneratePDFPO.Visible = false;
                lblstaticpdfcollection.Visible = false;

            }

        }

        /// <summary>
        ///  Preview Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPreview_Click(object sender, ImageClickEventArgs e)
        {


            if (ddlMailTemplate.SelectedIndex == -1)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgTest01", "alert('Please select Mail Template for Preview.');", true);
                return;
            }
            else
            {
                Session["PONotes"] = "";
                string Notes = string.Empty;
                if (!string.IsNullOrEmpty(txtDescription.Text.Trim()))
                    Notes = txtDescription.Text.Trim();

                Decimal AdditionalCost = 0, subtotal = 0;
                Decimal Adjustments = 0;
                Decimal Tax = 0;
                Decimal Shipping = 0;

                DataTable dtCart = new DataTable("VendorCart");
                dtCart.Columns.Add(new DataColumn("ProductID", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Name", typeof(String)));
                dtCart.Columns.Add(new DataColumn("SKU", typeof(String)));
                dtCart.Columns.Add(new DataColumn("Quantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Price", typeof(Decimal)));
                dtCart.Columns.Add(new DataColumn("PoQuantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("MaxQuantity", typeof(Int32)));
                dtCart.Columns.Add(new DataColumn("Customcartid", typeof(String)));

                bool checkquanttiydata = false;
                string lblerrorquantityitemname = string.Empty;

                foreach (GridViewRow row in grdCart.Rows)
                {

                    try
                    {
                        //Label lblProductID = (Label)row.FindControl("lblProductID");
                        //Label lblName = (Label)row.FindControl("lblProductName");
                        Label lblSKU = (Label)row.FindControl("lblSKU");
                        Label lblQty = (Label)row.FindControl("lblQty");
                        TextBox txtQty = (TextBox)row.FindControl("txtQuantity");
                        //Label lblPrice = (Label)row.FindControl("lblPrice");
                        ////TextBox txtPrice = (TextBox)row.FindControl("lblPrice");
                        Int32 pid = 0, qty = 0;
                        //Decimal Price = 0;
                        //Int32.TryParse(lblProductID.Text.Trim(), out pid);
                        Int32.TryParse(txtQty.Text.Trim(), out qty);
                        //Decimal.TryParse(lblPrice.Text, out Price);

                        if (qty <= 0)
                        {
                            lblerrorquantityitemname += lblSKU.Text + ",";
                            checkquanttiydata = true;
                            //string QtyMsg = "Please enter valid Quanity for " + lblName.Text + ". ";
                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('" + QtyMsg + "');", true);
                            //return;
                        }


                        //DataRow dr = dtCart.NewRow();
                        //dr["ProductID"] = pid;
                        //dr["Name"] = lblName.Text.Trim();
                        //dr["SKU"] = lblSKU.Text.Trim();
                        //dr["Quantity"] = qty;
                        //dr["Price"] = Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                        //subtotal += Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                        //dr["PoQuantity"] = qty;
                        //dr["MaxQuantity"] = qty;
                        //dr["Customcartid"] = "0";
                        //dtCart.Rows.Add(dr);
                    }
                    catch { }
                }


                if (checkquanttiydata == true)
                {
                    int lenentityitemerror = lblerrorquantityitemname.Length;
                    lblerrorquantityitemname = lblerrorquantityitemname.Substring(0, lenentityitemerror - 1);
                    string QtyMsg = "Please enter Quantity for following SKU(s): " + lblerrorquantityitemname;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('" + QtyMsg + "');", true);
                    return;
                }

                if (checkquanttiydata == false)
                {
                    foreach (GridViewRow row in grdCart.Rows)
                    {
                        try
                        {
                            Label lblProductID = (Label)row.FindControl("lblProductID");
                            Label lblName = (Label)row.FindControl("lblProductName");
                            Label lblSKU = (Label)row.FindControl("lblSKU");
                            Label lblQty = (Label)row.FindControl("lblQty");
                            TextBox txtQty = (TextBox)row.FindControl("txtQuantity");
                            Label lblPrice = (Label)row.FindControl("lblPrice");
                            //TextBox txtPrice = (TextBox)row.FindControl("lblPrice");
                            Int32 pid = 0, qty = 0;
                            Decimal Price = 0;
                            Int32.TryParse(lblProductID.Text.Trim(), out pid);
                            Int32.TryParse(txtQty.Text.Trim(), out qty);
                            Decimal.TryParse(lblPrice.Text, out Price);


                            DataRow dr = dtCart.NewRow();
                            dr["ProductID"] = pid;
                            dr["Name"] = lblName.Text.Trim();
                            dr["SKU"] = lblSKU.Text.Trim();
                            dr["Quantity"] = qty;
                            dr["Price"] = Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                            subtotal += Convert.ToDecimal(lblPrice.Text.Replace("$", ""));
                            dr["PoQuantity"] = qty;
                            dr["MaxQuantity"] = qty;
                            dr["Customcartid"] = "0";
                            dtCart.Rows.Add(dr);
                        }
                        catch { }
                    }
                }
                DataView dvVendor = dtCart.DefaultView;
                if (dvVendor.Count > 0)
                {
                    Session["VendorCart"] = null;
                    Session["VendorCart"] = dtCart;
                    Session["PONotes"] = Notes.ToString();
                    Response.Redirect("WareHouseVendormail.aspx?VendorID=" + ddlVendor.SelectedValue.ToString() + "&MailTemplate=" + ddlMailTemplate.SelectedValue.ToString() + "&AdditionalCost=" + AdditionalCost + "&Notes=1&Adjustments=" + Adjustments + "&Tax=" + Tax + "&Shipping=" + Shipping + "&Mode=view");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", "alert('Please select your Vendors for your Products.');", true);
                }
            }
        }


        /// <summary>
        /// Bind Product Details
        /// </summary>
        /// <param name="PId">string PId</param>
        /// <returns>Returns the Dataset of Product Details</returns>
        private DataSet GetProductBind(string PId)
        {
            if (PId.IndexOf(",") > -1)
            {
                PId = PId.Substring(0, PId.Length - 1);
            }

            DataSet dsBindCart = new DataSet();
            dsBindCart = CommonComponent.GetCommonDataSet("SELECT * FROM tb_WareHouseproduct WHERE productid in (" + PId + ")");
            return dsBindCart;
        }

        /// <summary>
        ///  Generate PDF for PO Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGeneratePDFPO_Click(object sender, ImageClickEventArgs e)
        {
            bool checkquanttiydata = true;
            System.DateTime podate = System.DateTime.Now;

            DataSet dsproductdet = new DataSet();

            if (ddlVendor.SelectedValue.ToString() != "0")
            {
                if (ddlVendor.SelectedItem.Text == "Shelter Logic/ Shelterworks")
                {




                    if (Request.QueryString["PID"] != null)
                        dsproductdet = GetProductBind(Request.QueryString["PID"].ToString());

                    int productid = 0;
                    int newquantity = 0;
                    double price = 0;
                    int grdrecord = 0;

                    checkquanttiydata = false;
                    string lblerrorquantityitemname = string.Empty;
                    for (int vproduct = 0; vproduct < dsproductdet.Tables[0].Rows.Count; vproduct++)
                    {

                        foreach (GridViewRow grcheck in grdCart.Rows)
                        {


                            Label lblsku = (Label)grcheck.FindControl("lblSKU");

                            string psku = lblsku.Text;
                            if (dsproductdet.Tables[0].Rows[vproduct]["SKU"].ToString() == psku)
                            {

                                Label lblname = (Label)grcheck.FindControl("lblProductName");
                                TextBox txtquantity = (TextBox)grcheck.FindControl("txtQuantity");
                                int qty = 0;
                                Int32.TryParse(txtquantity.Text.Trim(), out qty);
                                //if (int.Parse(txtquantity.Text) <= 0)
                                if (qty <= 0)
                                {

                                    lblerrorquantityitemname += lblsku.Text + ",";
                                    checkquanttiydata = true;
                                    //string QtyMsg = "Please enter valid Quanity for " + lblname.Text;
                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('" + QtyMsg + "');", true);
                                    lblstaticpdfcollection.Visible = false;
                                    //break;
                                }

                            }

                            if (checkquanttiydata)
                            {
                                //break;
                            }



                        }

                        if (checkquanttiydata)
                        {
                            //break;
                        }
                    }


                    if (checkquanttiydata == true)
                    {
                        int lenentityitemerror = lblerrorquantityitemname.Length;
                        lblerrorquantityitemname = lblerrorquantityitemname.Substring(0, lenentityitemerror - 1);
                        string QtyMsg = "Please enter Quantity for following SKU(s): " + lblerrorquantityitemname;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('" + QtyMsg + "');", true);
                    }

                    if (checkquanttiydata == false)
                    {
                        OrderComponent ordercomponent = new OrderComponent();

                        ordercomponent.InsertVendorPrurchaseOrderDetails(int.Parse(ddlVendor.SelectedValue), podate);

                        for (int vproduct = 0; vproduct < dsproductdet.Tables[0].Rows.Count; vproduct++)
                        {
                            foreach (GridViewRow grcheck in grdCart.Rows)
                            {
                                Label lblsku = (Label)grcheck.FindControl("lblSKU");

                                string psku = lblsku.Text;
                                if (dsproductdet.Tables[0].Rows[vproduct]["SKU"].ToString() == psku)
                                {

                                    TextBox txtquantity = (TextBox)grcheck.FindControl("txtQuantity");
                                    productid = int.Parse(dsproductdet.Tables[0].Rows[vproduct]["ProductID"].ToString());
                                    newquantity = int.Parse(txtquantity.Text);
                                    //quantity = int.Parse(dsproductdet.Tables[0].Rows[vproduct]["Inventory"].ToString());
                                    price = double.Parse(dsproductdet.Tables[0].Rows[vproduct]["Price"].ToString());
                                    ordercomponent.InsertVendorPrurchaseOrderItemDetails(int.Parse(ddlVendor.SelectedValue), productid, newquantity, price, podate);
                                    break;
                                }

                            }


                        }

                        WrittenPDF(podate);
                    }



                }
            }

        }


        /// <summary>
        /// Write the PDF for PO
        /// </summary>
        /// <param name="podate">DateTime podate.</param>
        protected void WrittenPDF(System.DateTime podate)
        {


            //ClearPDFData();
            double grandsubtotal = 0;
            double pricecheck = 0;
            string strprice = string.Empty;
            string dollorsing = "$";
            DataSet dsproductdet = new DataSet();
            DataSet dsselectvendor = new DataSet();
            string pdfilename = string.Empty;
            int pdata = 0;
            string porderno = string.Empty;
            int filecountno = 1;
            string fnameonly = string.Empty;

            OrderComponent Ocompnent = new OrderComponent();
            dsselectvendor = Ocompnent.SelectVendorPurchaseOrderDeatial(int.Parse(ddlVendor.SelectedValue), podate);

            if (Request.QueryString["PID"] != null)
                dsproductdet = Ocompnent.SelectPurchaserOrderDetailsVendor(int.Parse(ddlVendor.SelectedValue), podate);



            PlaceHolder laddcontorl = new PlaceHolder();

            int nofile = 1;

            //New Data added for more than 40 date: 22-Oct-2012
            decimal totalprecord = dsproductdet.Tables[0].Rows.Count;

            decimal rpcount = System.Math.Floor(totalprecord / 10);
            decimal modata = System.Math.Round(totalprecord % 10);
            if (modata > 0)
            {
                rpcount++;
            }



            try
            {
                //String pdffilepath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));

                String pdffilepath = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
                if (!System.IO.Directory.Exists(pdffilepath))
                {
                    System.IO.Directory.CreateDirectory(pdffilepath);
                }

                if (pdffilepath != string.Empty && pdffilepath != "")
                {

                    //New Data added for more than 40 date: 22-Oct-2012
                    for (decimal intrpcount = 1; intrpcount <= rpcount; intrpcount++)
                    {
                        fnameonly = string.Empty;
                        grandsubtotal = 0;
                        porderno = dsselectvendor.Tables[0].Rows[0]["PONumber"].ToString();
                        //pdfilename = "/" + "Resources/PDF Collection//" + "ShelterLogic" + "_"  + porderno + "_" + intrpcount + "_" + currentdate.Month + "_" + currentdate.Day.ToString() + "_" + currentdate.Year.ToString() + "_" + currentdate.Hour.ToString() + "_" + currentdate.Minute.ToString() + ".pdf";
                        if (dsproductdet.Tables[0].Rows.Count > 10)
                        {
                            if (intrpcount != 1)
                            {

                                filecountno++;
                                //pdfilename = "/" + "Resources/POFiles//" + "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                                //pdfilename = pdffilepath + "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                                //fnameonly = "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                                pdfilename = pdffilepath + "PO_" + porderno + "_" + filecountno + ".pdf";
                                fnameonly = "PO_" + porderno + "_" + filecountno + ".pdf";

                            }
                            else if (intrpcount == 1)
                            {


                                //pdfilename = "/" + "Resources/POFiles//" + "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                                //pdfilename = pdffilepath + "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                                pdfilename = pdffilepath + "PO_" + porderno + ".pdf";

                                fnameonly = "PO_" + porderno + ".pdf";
                            }

                        }
                        else
                        {
                            //pdfilename = "/" + "Resources/POFiles//" + "PO_SheleterLogic" + "_" + porderno + "_" + filecountno + ".pdf";
                            pdfilename = pdffilepath + "PO_" + porderno + ".pdf";
                            fnameonly = "PO_" + porderno + ".pdf";
                        }
                        string everthingpath = pdffilepath + "/Everythingcovers.pdf";

                        //PdfReader pdfReader = new PdfReader(Server.MapPath("~/Resources/POFiles/Everythingcovers.pdf"));
                        PdfReader pdfReader = new PdfReader(Server.MapPath(everthingpath));
                        string pdfactualpathname = Server.MapPath(pdfilename).ToString();
                        PdfStamper pdfStamper = new PdfStamper(pdfReader, new System.IO.FileStream(pdfactualpathname, FileMode.Create));
                        pdfStamper.FormFlattening = true;

                        AcroFields pdfForm = pdfStamper.AcroFields;

                        DataSet dsconfigdata = new DataSet();
                        //   dsconfigdata = CommonComponent.GetCommonDataSet(" select ConfigValue from tb_appconfig where configName in ('Shipping.CompanyName','Shipping.OriginContactName','Shipping.OriginAddress','Shipping.OriginAddress2','Shipping.OriginCity','Shipping.OriginCountry','Shipping.OriginState','Shipping.OriginZip','Shipping.OriginPhone') ");


                        for (int vdata = 0; vdata < dsselectvendor.Tables[0].Rows.Count; vdata++)
                        {

                            pdfForm.SetField("txtdate", System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                            pdfForm.SetField("txtpono", dsselectvendor.Tables[0].Rows[vdata]["PONumber"].ToString());
                        }

                        //pdfForm.SetField("txtsalesperson", AppLogic.AppConfigs("Shipping.CompanyName").ToString());
                        pdfForm.SetField("txtsalesperson", Session["AdminName"].ToString());
                        pdfForm.SetField("txtcompany", AppLogic.AppConfigs("Shipping.CompanyName").ToString());
                        //pdfForm.SetField("txtname", AppLogic.AppConfigs("Shipping.OriginContactName").ToString());
                        pdfForm.SetField("txtname", Session["AdminName"].ToString());
                        pdfForm.SetField("txtaddress", AppLogic.AppConfigs("Shipping.OriginAddress").ToString() + " " + AppLogic.AppConfigs("Shipping.OriginAddress2").ToString());
                        pdfForm.SetField("txtcity", AppLogic.AppConfigs("Shipping.OriginCity").ToString());
                        pdfForm.SetField("txtzip", AppLogic.AppConfigs("Shipping.OriginZip").ToString());
                        pdfForm.SetField("txtdayphone", AppLogic.AppConfigs("Shipping.OriginPhone").ToString());

                        DataSet dsvendordata = new DataSet();
                        dsvendordata = CommonComponent.GetCommonDataSet("select v.Name as VName,Address,City,State,c.Name as CountryName,Zipcode,Phone,email from tb_vendor as v,tb_Country as c where c.CountryID = v.Country and v.VendorID = '" + int.Parse(ddlVendor.SelectedValue) + "'");

                        if (dsvendordata.Tables[0].Rows.Count > 0)
                        {
                            pdfForm.SetField("txtshipto", dsvendordata.Tables[0].Rows[0]["VName"].ToString());
                            pdfForm.SetField("txtshipaddress", dsvendordata.Tables[0].Rows[0]["Address"].ToString());
                            pdfForm.SetField("txtshipcity", dsvendordata.Tables[0].Rows[0]["City"].ToString());
                            pdfForm.SetField("txshiptsate", dsvendordata.Tables[0].Rows[0]["State"].ToString());
                            pdfForm.SetField("txtshipzip", dsvendordata.Tables[0].Rows[0]["Zipcode"].ToString());
                            pdfForm.SetField("txtshiptophone", dsvendordata.Tables[0].Rows[0]["Phone"].ToString());
                            pdfForm.SetField("txtemail", dsvendordata.Tables[0].Rows[0]["email"].ToString());

                        }



                        string shortstatname = AppLogic.AppConfigs("Shipping.OriginState").ToString();
                        DataSet dsstatecheck = new DataSet();
                        dsstatecheck = CommonComponent.GetCommonDataSet("select Name from tb_state  where Abbreviation = '" + shortstatname + "'");

                        if (dsstatecheck.Tables[0].Rows.Count > 0)
                        {
                            pdfForm.SetField("txtstate", dsstatecheck.Tables[0].Rows[0]["Name"].ToString());
                        }





                        int tpdata = int.Parse(intrpcount.ToString()) * 10;

                        int newdatainc = pdata;


                        if (dsproductdet.Tables[0].Rows.Count <= tpdata)
                        {
                            tpdata = dsproductdet.Tables[0].Rows.Count;
                        }
                        else
                        {
                            tpdata = tpdata;
                        }
                        for (pdata = newdatainc; pdata < tpdata; pdata++)
                        {
                            int newpdata = pdata - newdatainc;
                            pdata = newpdata + newdatainc;

                            switch (newpdata)
                            {
                                case 0:
                                    pdfForm.SetField("txtdescitem1", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart1", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty1", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");
                                    pdfForm.SetField("txtitemprice1", strprice);

                                    double ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());

                                    pdfForm.SetField("txtitemtotal1", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 1:
                                    pdfForm.SetField("txtdescitem2", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart2", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty2", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice2", strprice);

                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal2", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 2:
                                    pdfForm.SetField("txtdescitem3", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart3", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty3", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");


                                    pdfForm.SetField("txtitemprice3", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal3", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 3:
                                    pdfForm.SetField("txtdescitem4", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart4", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty4", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice4", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal4", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 4:
                                    pdfForm.SetField("txtdescitem5", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart5", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty5", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice5", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal5", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 5:
                                    pdfForm.SetField("txtdescitem6", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart6", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty6", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice6", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal6", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 6:
                                    pdfForm.SetField("txtdescitem7", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart7", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty7", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice7", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal7", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 7:
                                    pdfForm.SetField("txtdescitem8", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart8", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty8", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice8", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal8", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 8:
                                    pdfForm.SetField("txtdescitem9", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart9", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty9", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());

                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice9", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal9", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                                case 9:
                                    pdfForm.SetField("txtdescitem10", dsproductdet.Tables[0].Rows[pdata]["Name"].ToString());
                                    pdfForm.SetField("txtitempart10", dsproductdet.Tables[0].Rows[pdata]["SKU"].ToString());
                                    pdfForm.SetField("txtitemqty10", dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString());


                                    pricecheck = 0;
                                    strprice = string.Empty;
                                    pricecheck = double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    strprice = pricecheck.ToString("0.00");

                                    pdfForm.SetField("txtitemprice10", strprice);
                                    ppricetotal = 0;
                                    ppricetotal = int.Parse(dsproductdet.Tables[0].Rows[pdata]["quantity"].ToString()) * double.Parse(dsproductdet.Tables[0].Rows[pdata]["Price"].ToString());
                                    pdfForm.SetField("txtitemtotal10", ppricetotal.ToString("0.00"));
                                    grandsubtotal = grandsubtotal + ppricetotal;
                                    break;
                            }
                        }

                        string strgrandsubtotal = grandsubtotal.ToString("0.00");
                        pdfForm.SetField("txtgrandsubtotal", strgrandsubtotal);
                        pdfForm.SetField("txttotal", strgrandsubtotal);

                        pdfStamper.FormFlattening = true;
                        pdfStamper.SetFullCompression();
                        pdfStamper.Close();
                        string filenameonly = fnameonly;
                        string Links = "";
                        Links = Links +
                        "<input style='display:none; padding-top: 10px;' type='submit' runat='server' value='" + pdfactualpathname + "' name='btnDownload-main-" + intrpcount + "'  id='btnDownload-main-" + intrpcount + "' onclick='submit' /> <a href='#' onclick=\"document.getElementById('btnDownload-main-" + intrpcount + "').click();\">" + filenameonly + "</a> <br/>";

                        Label l1 = new Label();
                        l1.Text = Links + "             ";
                        l1.ForeColor = System.Drawing.Color.Red;

                        PlaceHolder1.Controls.Add(l1);
                        nofile++;
                        PlaceHolder1.Visible = false;
                    }

                    lblstaticpdfcollection.Visible = false;
                    RemoveProductFromWarhouse();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@SaveMsg", "alert('Record Saved Successfully!'); window.location.href='WareHousePO.aspx';", true);


                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Please Configuare PDF File Path in Admin!');", true);
                }


            }
            catch
            {
            }
            finally
            {

            }
        }

        /// <summary>
        /// Download Files at Specified File Path
        /// <param name="filepath">string filepath</param>
        private void downloadfile(string filepath)
        {
            string pathdir = Convert.ToString(AppLogic.AppConfigs("POFilesPath"));
            //string strFile = filepath.Replace(Server.MapPath("/Resources/").ToString(), "");
            string strFile = filepath.Replace(Server.MapPath(pathdir).ToString(), "");
            //Response.Redirect("/Resources/" + strFile.Replace("\\", "/"));
            Response.Redirect(strFile.Replace("\\", "/"));
        }

        /// <summary>
        /// Removes the Product from Warehouse
        /// </summary>
        protected void RemoveProductFromWarhouse()
        {
            string pdatastr = Request.QueryString["PID"].ToString();
            string[] strproductid = pdatastr.Split(',');

            foreach (string pid in strproductid)
            {
                CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseproduct WHERE Productid =" + pid + "");
            }
        }
    }
}