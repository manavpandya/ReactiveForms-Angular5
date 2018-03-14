using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Net.Mail;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI.HtmlControls;
using System.IO;
using MWSMerchantFulfillmentService;
using Ionic.Zlib;
using Ionic.Zip;
using System.Text.RegularExpressions;
using PdfSharp.Drawing;


namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class MultipleShippingSection : BasePage
    {

        #region Declaration

        bool IsPrintLabelExists = false;
        string UPCPath = Convert.ToString(AppLogic.AppConfigs("ProductUPC"));
        OrderComponent objOrderComp = new OrderComponent();
        int checkCount, GridrowCount = 0;
        public static int pageIndex = 1;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["AmazonMerchantID"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonMerchantID'"));
                ViewState["AmazonServiceURL"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonServiceURL'"));
                ViewState["AmazonAccessKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonAccessKey'"));
                ViewState["AmazonSecretKey"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonSecretKey'"));
                ViewState["AmazonApplicationName"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonApplicationName'"));
                ViewState["AmazonDefaultMethod"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname='AmazonDefaultMethod'"));

                //ResizeImagewithpdf(@"C:\Users\karan\Desktop\images\temp", @"C:\Users\karan\Desktop\images", "0001.jpg", @"C:\Users\karan\Desktop\images");
                Session["GetOrderListByBatchIDs"] = null;
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                //btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");

                btnPrintLabel1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-label.gif";
                btnprintslip1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-packing-slip-shipping.gif";

                //if (Session["AmazonUserLogin"] != null && Session["AmazonUserLogin"].ToString() == "3")
                //{
                GetStoreList();
                this.GetCustomersPageWise(1);
                //byte[] AAAL= GetTransparentArrayFromFileWithDelete(Server.MapPath("/images/auto-cover.jpg"));
                //using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(AAAL)))
                //{
                //    image.Save(Server.MapPath("/images/output1.png"), System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                //}


                //using (FileStream fOutStream = new FileStream(Server.MapPath("/IMAGES/1222.PNG"),
                //                                   FileMode.Create, FileAccess.Write))
                //{

                //    byte[] tempBytes = new byte[4096];
                //    int i;
                //    while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                //    {
                //        fOutStream.Write(tempBytes, 0, i);
                //    }
                //}

                //}
            }
        }

        public static byte[] GetTransparentArrayFromFileWithDelete(string pathToFile)
        {
            byte[] newImage = new byte[0];
            using (Bitmap bmp = new Bitmap(pathToFile))
            {
                System.Drawing.Color pixel = bmp.GetPixel(0, 0);
                if (pixel.A != 0)
                {
                    // Make backColor transparent for myBitmap.
                    bmp.MakeTransparent(System.Drawing.Color.Transparent);

                    ImageConverter converter = new ImageConverter();
                    newImage = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    bmp.Dispose();
                }
                else
                {
                    FileStream fs = new FileStream(pathToFile, FileMode.OpenOrCreate, FileAccess.Read);
                    newImage = new byte[fs.Length];
                    fs.Read(newImage, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                }
            }
            try
            {
                File.Delete(pathToFile);
            }
            catch
            {
            }
            return newImage;
        }


        /// <summary>
        /// Get Store List
        /// </summary>
        /// <param name="ddlStore">DropDownlist Control</param>
        private void GetStoreList()
        {
            try
            {
                ddlStore.Items.Clear();
                DataSet dsStore = new DataSet();
                //dsStore = StoreComponent.GetStoreListByMenu();
                dsStore = CommonComponent.GetCommonDataSet("select * from tb_Store where deleted = 0 and StoreName like '%amazon%'");
                if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
                {
                    ddlStore.DataSource = dsStore;
                    ddlStore.DataValueField = "StoreID";
                    ddlStore.DataTextField = "StoreName";
                    ddlStore.DataBind();
                }
                else
                {
                    ddlStore.DataSource = null;
                    ddlStore.DataBind();
                }
            }
            catch
            {
            }
        }


        private void GetCustomersPageWise(int pageIndex)
        {


            //DateTime StartDate = System.DateTime.Now;
            //DateTime EndDate = System.DateTime.Now;


            DataSet DsGetOrderListByBatchIDs = new DataSet();
            if (Session["GetOrderListByBatchIDs"] == null)
            {
                if (int.Parse(ddlStore.SelectedValue) > 0)
                {
                    //DsGetOrderListByBatchIDs = CommonComponent.GetCommonDataSet("EXEC usp_Order_GetOrderListByBatchIDs ''," + pageIndex + "," + int.Parse(ddlPageSize.SelectedValue) + ",'','','" + txtMailFrom.Text.ToString() + "','" + txtMailTo.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ",'" + txtSearch.Text.ToString().Replace("'", "''") + "'");
                    //if (txtSearch.Text.ToString().ToLower() != "orderNo / reforder no")
                    //{
                    Int32 orNum;
                    Int32.TryParse(txtSearch.Text.ToString().Replace("'", "''"), out orNum);
                    //DsGetOrderListByBatchIDs = CommonComponent.GetCommonDataSet("EXEC usp_Order_GetOrderListByBatchIDs ''," + pageIndex + "," + int.Parse(ddlPageSize.SelectedValue) + ",'','','" + txtMailFrom.Text.ToString() + "','" + txtMailTo.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ",'" + Convert.ToString(txtSearch.Text.ToString().Replace("'", "''")) + "'," + orNum + "");

                    //if (ddlStatus.SelectedIndex > 0)
                    //{
                    if (ChkIsPrime.Checked)
                        DsGetOrderListByBatchIDs = CommonComponent.GetCommonDataSet("EXEC usp_Order_GetOrderListByBatchIDs ''," + pageIndex + "," + int.Parse(ddlPageSize.SelectedValue) + ",'','','" + txtMailFrom.Text.ToString() + "','" + txtMailTo.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ",'" + ddlStatus.SelectedValue.ToString().Replace("'", "''") + "','true','" + Convert.ToString(txtSearch.Text.ToString().Replace("'", "''")) + "'," + orNum + "");
                    else
                        DsGetOrderListByBatchIDs = CommonComponent.GetCommonDataSet("EXEC usp_Order_GetOrderListByBatchIDs ''," + pageIndex + "," + int.Parse(ddlPageSize.SelectedValue) + ",'','','" + txtMailFrom.Text.ToString() + "','" + txtMailTo.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ",'" + ddlStatus.SelectedValue.ToString().Replace("'", "''") + "','false','" + Convert.ToString(txtSearch.Text.ToString().Replace("'", "''")) + "'," + orNum + "");
                    //}
                    //else
                    //{
                    //    DsGetOrderListByBatchIDs = CommonComponent.GetCommonDataSet("EXEC usp_Order_GetOrderListByBatchIDs ''," + pageIndex + "," + int.Parse(ddlPageSize.SelectedValue) + ",'','','" + txtMailFrom.Text.ToString() + "','" + txtMailTo.Text.ToString() + "',1,'','" + Convert.ToString(txtSearch.Text.ToString().Replace("'", "''")) + "'," + orNum + "");
                    //}
                    //}
                }

            }
            else
            {
                DsGetOrderListByBatchIDs = (DataSet)Session["GetOrderListByBatchIDs"];
            }

            int recordCount = 0;
            if (DsGetOrderListByBatchIDs != null && DsGetOrderListByBatchIDs.Tables.Count > 0 && DsGetOrderListByBatchIDs.Tables[0].Rows.Count > 0)
            {
                // Session["GetOrderListByBatchIDs"] = (DataSet)DsGetOrderListByBatchIDs;
                recordCount = DsGetOrderListByBatchIDs.Tables[0].Rows.Count;
                this.PopulatePager(recordCount, pageIndex);
                DataSet dsnew = new DataSet();
                dsnew = DsGetOrderListByBatchIDs.Clone();
                string[] strcolors = { "#ec1f7b", "#e1ae23", "#C98BF0", "#098EAA", "#c5230d", "#c5230d", "#008000" };
                Int32 icolor = 6;
                if (recordCount == 1 && pageIndex <= 1)
                {
                    Int32 TotalShip = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(Sum(isnull(ShipQty,0)),0) from tb_AmazonlabelDetails WHERE OrderNumber=" + DsGetOrderListByBatchIDs.Tables[0].Rows[0]["OrderNumber"].ToString() + ""));
                    Int32 TotalORderedqty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT  isnull(Sum(isnull(Quantity,0)),0)  FROM tb_OrderedShoppingCartItems INNER JOIN tb_Order on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID  WHERE OrderNumber=" + DsGetOrderListByBatchIDs.Tables[0].Rows[0]["OrderNumber"].ToString() + ""));
                    if (TotalORderedqty > TotalShip)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Openwindowdata", "window.open('MultipleShippingPopupForLabel.aspx?Ono=" + DsGetOrderListByBatchIDs.Tables[0].Rows[0]["OrderNumber"].ToString() + "', '_newtab');", true);
                    }

                }


                foreach (DataRow dr1 in DsGetOrderListByBatchIDs.Tables[0].Select("RowNumber > " + ((pageIndex - 1) * int.Parse(ddlPageSize.SelectedValue)) + " and RowNumber <=" + (pageIndex * int.Parse(ddlPageSize.SelectedValue)) + ""))
                {
                    dsnew.Tables[0].ImportRow(dr1);
                }
                try
                {


                    string emailall = ",";
                    string emailcolorall = ",";
                    for (int i = 0; i < dsnew.Tables[0].Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[i]["ShippingEmail"].ToString()))
                        {
                            if (emailall.ToString().ToLower().IndexOf("," + dsnew.Tables[0].Rows[i]["ShippingEmail"].ToString().ToLower() + ",") > -1)
                            {
                                string[] emailallsplt = emailall.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] emailcolorallsplt = emailcolorall.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (emailallsplt.Length > 0)
                                {
                                    for (int ic = 0; ic < emailallsplt.Length; ic++)
                                    {
                                        if (emailallsplt[ic].ToString().ToLower().Trim() == dsnew.Tables[0].Rows[i]["ShippingEmail"].ToString().ToLower().Trim())
                                        {
                                            dsnew.Tables[0].Rows[i]["Storename"] = emailcolorallsplt[ic].ToString();
                                            dsnew.Tables[0].AcceptChanges();
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] dremail = dsnew.Tables[0].Select("ShippingEmail='" + dsnew.Tables[0].Rows[i]["ShippingEmail"].ToString() + "'");
                                if (dremail.Length > 1)
                                {

                                    emailall += dsnew.Tables[0].Rows[i]["ShippingEmail"].ToString() + ",";
                                    if (icolor < 0)
                                    {

                                        dsnew.Tables[0].Rows[i]["Storename"] = "#4c8fbd";
                                        emailcolorall += "#4c8fbd,";
                                    }
                                    else
                                    {
                                        emailcolorall += strcolors[icolor] + ",";
                                        dsnew.Tables[0].Rows[i]["Storename"] = strcolors[icolor].ToString();
                                    }
                                    dsnew.Tables[0].AcceptChanges();
                                    icolor--;
                                }
                                else
                                {
                                    dsnew.Tables[0].Rows[i]["Storename"] = "#4c8fbd";
                                    dsnew.Tables[0].AcceptChanges();
                                }
                            }

                        }
                        else
                        {
                            dsnew.Tables[0].Rows[i]["Storename"] = "#4c8fbd";
                            dsnew.Tables[0].AcceptChanges();
                        }
                    }
                }
                catch
                {

                }
                btnPrintLabel1.Visible = true;
                btnprintslip1.Visible = true;
                grvOrderlist.DataSource = dsnew.Tables[0].DefaultView;
                grvOrderlist.DataBind();
                trpagesize.Visible = true;

            }
            else
            {
                grvOrderlist.DataSource = null;
                grvOrderlist.DataBind();
                if (grvOrderlist.DataSource == null)
                {
                    trpagesize.Visible = false;
                }
                else
                {
                    trpagesize.Visible = true;
                }
                //trpagesize.Visible = false;
                btnPrintLabel1.Visible = false;
                btnprintslip1.Visible = false;
            }



        }

        protected void PageSize_Changed(object sender, EventArgs e)
        {
            this.GetCustomersPageWise(1);
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            this.GetCustomersPageWise(pageIndex);
        }

        private void PopulatePager(int recordCount, int currentPage)
        {

            double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(ddlPageSize.SelectedValue));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            ArrayList pages = new ArrayList();
            if (pageCount > 0)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();

        }



        /// <summary>
        /// Get Order Wise Product
        /// </summary>
        /// <param name="cartId">Order Cart Id</param>
        /// <param name="ltrProduct">Literal Control</param>
        private decimal GetProduct(Int32 cartId, Literal ltrProduct, HtmlInputHidden hndProductId, TextBox txtaddtionalprice)
        {
            DataSet dsProduct = new DataSet();
            DataSet dsDimensions = new DataSet();

            OrderComponent objOrder = new OrderComponent();
            dsProduct = objOrder.GetProductList(cartId);
            decimal FinalDimesions = 0;
            decimal productPrice = 0;
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsProduct.Tables[0].Rows.Count; i++)
                {
                    hndProductId.Value = Convert.ToString(dsProduct.Tables[0].Rows[i]["RefProductId"]);
                    string[] variantName = dsProduct.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] variantValue = dsProduct.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string strVariantname = "";
                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            strVariantname += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                        }
                    }
                    if (i == 0)
                    {
                        if (strVariantname != "")
                        {
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />" + strVariantname + " QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                        }
                        else
                        {
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                        }
                    }
                    else
                    {
                        if (i == 1)
                        {
                            ltrProduct.Text += "<div style='width: 20%; float: right; text-align: right;'> " +
                                         " <a class='order-no' onclick='showhideGrid(" + cartId + ");' href='javascript:void(0);'> " +
                             " <img class='minimize' title='minimize' id='imgminmize" + cartId + "' alt='close' " +
                             " src='/Admin/images/expand_1.png'></a>  </div>";

                            ltrProduct.Text += "<div style='display:none;' id='" + Convert.ToString(cartId) + "'>";
                        }

                        if (strVariantname != "")
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />" + strVariantname + " QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";
                        else
                            ltrProduct.Text += "<strong>" + dsProduct.Tables[0].Rows[i]["ProductName"] + "</strong><br />QTY: " + dsProduct.Tables[0].Rows[i]["Quantity"] + "<br />SKU: " + dsProduct.Tables[0].Rows[i]["SKu"] + "<br />";

                        if (i == dsProduct.Tables[0].Rows.Count - 1)
                            ltrProduct.Text += "</div>";

                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["Price"].ToString()))
                    {
                        decimal price = 0;
                        decimal.TryParse(dsProduct.Tables[0].Rows[i]["Price"].ToString(), out price);
                        productPrice += price;

                    }
                    if (dsProduct.Tables[0].Rows[i]["SKu"] != null && dsProduct.Tables[0].Rows[i]["SKu"].ToString() != "")
                    {
                        decimal Height = 0;
                        decimal Width = 0;
                        decimal Length = 0;
                        decimal total = 0;
                        dsDimensions = CommonComponent.GetCommonDataSet("SELECT top 1 ISNULL(Height,0) AS Height ,ISNULL(Width,0) AS Width,ISNULL(Length,0) AS Length FROM dbo.tb_Product WHERE SKU ='" + dsProduct.Tables[0].Rows[i]["SKu"].ToString() + "' and StoreId=" + ddlStore.SelectedValue.ToString() + " and isnull(Active,0)=1 and isnull(Deleted,0)=0");
                        if (dsDimensions != null && dsDimensions.Tables.Count > 0 && dsDimensions.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dsDimensions.Tables[0].Rows[0]["Height"].ToString()))
                                decimal.TryParse(dsDimensions.Tables[0].Rows[0]["Height"].ToString(), out Height);
                            if (!string.IsNullOrEmpty(dsDimensions.Tables[0].Rows[0]["Width"].ToString()))
                                decimal.TryParse(dsDimensions.Tables[0].Rows[0]["Width"].ToString(), out Width);
                            if (!string.IsNullOrEmpty(dsDimensions.Tables[0].Rows[0]["Length"].ToString()))
                                decimal.TryParse(dsDimensions.Tables[0].Rows[0]["Length"].ToString(), out Length);

                            total = Height * Width * Length;

                            FinalDimesions += total;
                        }
                    }

                }

                if (Session["ProductDetail"] != null)
                {
                    DataTable dtproductdetail = Session["ProductDetail"] as DataTable;
                    decimal addprice = 0;
                    if (dtproductdetail.Rows.Count > 0)
                    {
                        DataRow[] foundRows;
                        foundRows = dtproductdetail.Select("ShippingCartID =" + cartId + "");
                        for (int i = 0; i < foundRows.Length; i++)
                        {
                            ltrProduct.Text += "<strong>" + foundRows[i]["Name"] + "</strong><br />QTY: " + foundRows[i]["Quantity"] + "<br />SKU: " + foundRows[i]["SKu"] + "<br />";
                            decimal.TryParse(foundRows[i]["SalePrice"].ToString(), out addprice);
                            productPrice += addprice;
                        }
                    }

                }
                txtaddtionalprice.Text = string.Format("{0:0.00}", productPrice);
            }
            return FinalDimesions;

        }

        /// <summary>
        /// Generate Shipping LAble Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGeneratealllabel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GenerateAlllabel();
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Get All Shipping Lable
        /// </summary>
        public void GenerateAlllabel()
        {

            try
            {
                foreach (GridViewRow gvr in grvOrderlist.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        HtmlInputHidden hndProductId = (HtmlInputHidden)gvr.FindControl("hndProductId");
                        HtmlInputHidden hdnRefOrderNo = (HtmlInputHidden)gvr.FindControl("hdnRefOrderNo");

                        Label lblOrderNumber = (Label)gvr.FindControl("lblOrderNumber");
                        DropDownList ddlShippingMethod = (DropDownList)gvr.FindControl("ddlShippingMethod");
                        DropDownList ddlWareHouse = (DropDownList)gvr.FindControl("ddlWareHouse");
                        TextBox txtProWeight = (TextBox)gvr.FindControl("txtProWeight");
                        TextBox txtHeightgrid = (TextBox)gvr.FindControl("txtHeightgrid");
                        TextBox txtWidthgrid = (TextBox)gvr.FindControl("txtWidthgrid");
                        TextBox txtLengthgrid = (TextBox)gvr.FindControl("txtLengthgrid");
                        DropDownList ddlDimensions = (DropDownList)gvr.FindControl("ddlDimensions");

                        CheckBox chkMail = (CheckBox)gvr.FindControl("chkMail");
                        CheckBox chkinsured = (CheckBox)gvr.FindControl("chkinsured");
                        TextBox txtaddtionalprice = (TextBox)gvr.FindControl("txtaddtionalprice");
                        DropDownList ddlMailpieceShape = (DropDownList)gvr.FindControl("ddlMailpieceShape");
                        Int32 lblno = 2;
                        CheckBox chkgeneratelbl = (CheckBox)gvr.FindControl("chkgeneratelbl");
                        decimal productprice = 0;

                        decimal.TryParse(txtaddtionalprice.Text, out productprice);

                        decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;

                        if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                            decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                        if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                            decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                        if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                            decLength = Convert.ToDecimal(txtLengthgrid.Text);

                        if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                            decWidth = Convert.ToDecimal(txtWidthgrid.Text);

                        int chkQty = 0;
                        string ShippingLabelFileName = "";
                        string OrderNumber = Convert.ToString(lblOrderNumber.Text);
                        string ShipmentId = "";

                        #region AddproductDetail
                        TextBox txtaddsku = (TextBox)gvr.FindControl("txtaddsku");
                        TextBox txtaddname = (TextBox)gvr.FindControl("txtaddname");
                        TextBox txtaddqty = (TextBox)gvr.FindControl("txtaddqty");
                        TextBox txtaddprice = (TextBox)gvr.FindControl("txtaddprice");
                        System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)gvr.FindControl("hdnshoppingcartid");
                        decimal addprice = 0;
                        try
                        {
                            decimal.TryParse(txtaddprice.Text, out addprice);
                        }
                        catch { }
                        int addqty = 0;
                        int shoppingcartid = 0;
                        try
                        {
                            int.TryParse(txtaddqty.Text, out addqty);
                        }
                        catch { }
                        try
                        {
                            int.TryParse(hdnshoppingcartid.Value.ToString(), out shoppingcartid);
                        }
                        catch { }

                        #endregion

                        if (!string.IsNullOrEmpty(OrderNumber.ToString()))
                        {
                            if (ddlShippingMethod.SelectedItem != null && chkgeneratelbl != null && chkgeneratelbl.Checked == true)
                            {
                                if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text))
                                {


                                    CountryComponent objCountry = new CountryComponent();
                                    if (ddlShippingMethod.SelectedItem != null)
                                    {
                                        // MWSMerchantFulfillmentService.Model.GetShipmentResponse objShippent = new MWSMerchantFulfillmentService.Model.GetShipmentResponse();
                                        MWSMerchantFulfillmentService.Model.Shipment objShippent = new MWSMerchantFulfillmentService.Model.Shipment();
                                        MWSMerchantFulfillmentService.Model.CreateShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CreateShipmentResponse();
                                        if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text))
                                        {
                                            string sellerId = ViewState["AmazonMerchantID"].ToString();
                                            string mwsAuthToken = "";
                                            // The client application version
                                            string appVersion = "1.01";

                                            // The endpoint for region service and version (see developer guide)
                                            // ex: https://mws.amazonservices.com
                                            string serviceURL = ViewState["AmazonServiceURL"].ToString();
                                            MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                                            config.ServiceURL = serviceURL;
                                            // Set other client connection configurations here if needed
                                            // Create the client itself
                                            MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                                            MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
                                            DataTable dtOrder = new DataTable();
                                            dtOrder.Columns.Add("Qty", typeof(int));
                                            dtOrder.Columns.Add("AmazonItemId", typeof(String));
                                            dtOrder.Columns.Add("RefOrderId", typeof(String));
                                            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderNumber=" + OrderNumber + "");
                                            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                                {
                                                    DataRow dataRow = dtOrder.NewRow();
                                                    dataRow["qty"] = dtORderdetails.Tables[0].Rows[i]["Quantity"].ToString();
                                                    dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                                                    dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                                    dtOrder.Rows.Add(dataRow);
                                                }
                                            }
                                            string[] strlids = ddlShippingMethod.SelectedValue.ToString().Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            objlabel = obj.InvokeCreateShipment(hdnRefOrderNo.Value.ToString(), sellerId, "", strlids[0].ToString(), strlids[1].ToString(), decLength, decWidth, decHeight, decWeight, dtOrder);

                                            objShippent = objlabel.CreateShipmentResult.Shipment;
                                            // objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                            if (objShippent == null)
                                            {
                                                return;
                                            }
                                            string strddd = objlabel.CreateShipmentResult.Shipment.Label.FileContents.Contents.ToString();
                                            //  objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");

                                            //string strddd = objShippent.GetShipmentResult.Shipment.Label.FileContents.Contents.ToString();
                                            byte[] imageBytes = Base64DecodeString(strddd);
                                            string fileName = objlabel.CreateShipmentResult.Shipment.TrackingId.ToString();
                                            ShipmentId = objlabel.CreateShipmentResult.Shipment.ShipmentId.ToString();
                                            CommonComponent.ExecuteCommonData("Update tb_Order set ShipmentId='" + ShipmentId + "'  where  OrderNumber=" + OrderNumber + "");
                                            //  string fileName = objShippent.GetShipmentResult.Shipment.TrackingId.ToString();
                                            string imagepath = "";
                                            imagepath = "~/ShippingLabels/FEDEX/";// AppLogic.AppConfigs("FedEx.LabelSavePath");
                                            string fileFormat = AppLogic.AppConfigs("Shipping.LabelFormat").ToString();
                                            if (fileFormat.Length > 3)
                                            {
                                                fileFormat = "pdf";
                                            }
                                            if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("USPS") > -1)
                                            {
                                                imagepath = "~/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                                fileName = "USPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                 DateTime.Now.Year.ToString() +
                                 DateTime.Now.Month.ToString() +
                                 DateTime.Now.Day.ToString() +
                                 DateTime.Now.Hour.ToString() +
                                 DateTime.Now.Minute.ToString() +
                                 DateTime.Now.Second.ToString() + "-1." +
                                 fileFormat.ToLower();
                                            }
                                            else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("UPS") > -1)
                                            {
                                                imagepath = "~/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                                fileName = "UPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                    DateTime.Now.Year.ToString() +
                                    DateTime.Now.Month.ToString() +
                                    DateTime.Now.Day.ToString() +
                                    DateTime.Now.Hour.ToString() +
                                    DateTime.Now.Minute.ToString() +
                                    DateTime.Now.Second.ToString() + "-1." +
                                   fileFormat.ToLower();
                                            }
                                            else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("FEDEX") > -1)
                                            {
                                                imagepath = "~/ShippingLabels/FEDEX/";//Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                                fileName = "FedEx-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                            DateTime.Now.Year.ToString() +
                                            DateTime.Now.Month.ToString() +
                                            DateTime.Now.Day.ToString() +
                                            DateTime.Now.Hour.ToString() +
                                            DateTime.Now.Minute.ToString() +
                                            DateTime.Now.Second.ToString() + "-1." + fileFormat.ToLower();
                                            }

                                            ShippingLabelFileName = fileName;
                                            FileStream fstrm = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")), FileMode.CreateNew, FileAccess.Write);
                                            BinaryWriter writer = new BinaryWriter(fstrm);
                                            writer.Write(imageBytes);
                                            writer.Close();
                                            fstrm.Close();

                                            using (FileStream fInStream = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")),
                                FileMode.Open, FileAccess.Read))
                                            {
                                                using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                                                {
                                                    using (FileStream fOutStream = new FileStream(Server.MapPath(imagepath + "temp/" + fileName),
                                                    FileMode.Create, FileAccess.Write))
                                                    {

                                                        byte[] tempBytes = new byte[4096];
                                                        int i;
                                                        while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                                                        {
                                                            fOutStream.Write(tempBytes, 0, i);
                                                        }
                                                    }
                                                }
                                            }
                                            if (fileName.ToString().ToLower().IndexOf(".png") > -1)
                                            {
                                                ResizeImagewithpdf(imagepath + "temp", imagepath, fileName, imagepath);
                                            }
                                            else
                                            {
                                                System.IO.File.Copy(imagepath + "temp/" + fileName, imagepath + fileName, true);
                                            }

                                        }
                                        else
                                        {
                                            Session["ProductDetail"] = null;
                                            return;
                                        }
                                        decimal ShippingCost = 0;
                                        if (ddlShippingMethod.SelectedValue.ToString() != "")
                                        {
                                            int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                                            int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
                                            if (Index != -1 && Length != 0)
                                            {
                                                string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                                                Decimal.TryParse(strShippingCost, out ShippingCost);
                                            }
                                        }

                                        if (string.IsNullOrEmpty(ShippingLabelFileName) || ShippingLabelFileName.ToString().Trim() == "")
                                        {
                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('Error While Generating Shipping Label.','Message');", true);
                                            Session["ProductDetail"] = null;
                                            return;
                                        }
                                        if (!string.IsNullOrEmpty(Convert.ToString(ShippingLabelFileName.Trim())) && Convert.ToString(ShippingLabelFileName.Trim()).ToLower().IndexOf("error :") > -1)
                                        {
                                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('" + ShippingLabelFileName.ToString() + "','Message');", true);
                                            Session["ProductDetail"] = null;
                                            return;
                                        }
                                        if (!string.IsNullOrEmpty(ShippingLabelFileName.ToString()))
                                        {
                                            string StrShipping = Convert.ToString(ddlShippingMethod.SelectedItem.Text);
                                            if (!string.IsNullOrEmpty(StrShipping.ToString().Trim()) && StrShipping.ToLower().IndexOf("($") > -1)
                                            {
                                                StrShipping = StrShipping.Substring(0, StrShipping.IndexOf("($"));
                                            }
                                            string strdimension = Convert.ToString(Convert.ToString(decHeight) + " x " + Convert.ToString(decWidth) + " x " + Convert.ToString(decLength));

                                            CommonComponent.ExecuteCommonData("Update tb_Order set IsBackendProcessed=1,ShipmentId='" + ShipmentId + "', ShippingDimension ='" + strdimension + "', ShippingLabelFileName='" + ShippingLabelFileName.ToString().Replace(".png", ".pdf") + "',ShippingLabelMethod='" + ddlShippingMethod.SelectedItem.Text + "',ShippingLabelCost=" + ShippingCost + ",ShippingLabelWeight=" + decWeight + ",ShippingLabelPackageHeight=" + decHeight + ",ShippingLabelPackageWidth=" + decWidth + ",ShippingLabelPackageLength=" + decLength + "  where  OrderNumber=" + OrderNumber + "");
                                            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID,SKU FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderNumber=" + OrderNumber + "");
                                            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                                {
                                                    CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_PackageHeader " + OrderNumber + ",'" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.CarrierName.ToString() + "','','" + objlabel.CreateShipmentResult.Shipment.ShippingService.ShipDate.ToString() + "','" + dtORderdetails.Tables[0].Rows[i]["SKU"].ToString() + "'," + dtORderdetails.Tables[0].Rows[i]["Quantity"].ToString() + "");
                                                }
                                            }
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_AmazonlabelDetails(OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, ShipQty, TrackingNumber, CreatedOn) SELECT OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, Quantity, TrackingNumber, getdate()  FROM tb_OrderedShoppingCartItems INNER JOIN tb_Order on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID WHERE OrderNumber=" + OrderNumber + "");

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@UpdateMsg", "jAlert('Shipping Label Generated Successfully.','Success');", true);
                Session["ProductDetail"] = null;
                Session["GetOrderListByBatchIDs"] = null;
                this.GetCustomersPageWise(pageIndex);
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }
        public static byte[] Base64DecodeString(string inputStr)
        {
            byte[] decodedByteArray =
              Convert.FromBase64CharArray(inputStr.ToCharArray(),
                                            0, inputStr.Length);
            return (decodedByteArray);
        }
        /// <summary>
        /// GridView Row Command Event for Save and Generate Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvOrderlist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandSource.GetType() != typeof(GridView))
                {
                    GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "dimensions", "dimensions(" + row.RowIndex.ToString() + ",'');", true);

                    HtmlInputHidden hndProductId = (HtmlInputHidden)row.FindControl("hndProductId");
                    HtmlInputHidden hdnRefOrderNo = (HtmlInputHidden)row.FindControl("hdnRefOrderNo");
                    Label lblOrderNumber = (Label)row.FindControl("lblOrderNumber");
                    DropDownList ddlShippingMethod = (DropDownList)row.FindControl("ddlShippingMethod");
                    DropDownList ddlWareHouse = (DropDownList)row.FindControl("ddlWareHouse");
                    TextBox txtProWeight = (TextBox)row.FindControl("txtProWeight");
                    TextBox txtHeightgrid = (TextBox)row.FindControl("txtHeightgrid");
                    TextBox txtWidthgrid = (TextBox)row.FindControl("txtWidthgrid");
                    TextBox txtLengthgrid = (TextBox)row.FindControl("txtLengthgrid");
                    DropDownList ddlMailpieceShape = (DropDownList)row.FindControl("ddlMailpieceShape");
                    DropDownList ddlDimensions = (DropDownList)row.FindControl("ddlDimensions");

                    CheckBox chkMail = (CheckBox)row.FindControl("chkMail");
                    CheckBox chkinsured = (CheckBox)row.FindControl("chkinsured");
                    TextBox txtaddtionalprice = (TextBox)row.FindControl("txtaddtionalprice");

                    Int32 lblno = 2;
                    decimal productprice = 0;
                    try
                    {
                        decimal.TryParse(txtaddtionalprice.Text, out productprice);
                    }
                    catch { }
                    #region AddproductDetail
                    TextBox txtaddsku = (TextBox)row.FindControl("txtaddsku");
                    TextBox txtaddname = (TextBox)row.FindControl("txtaddname");
                    TextBox txtaddqty = (TextBox)row.FindControl("txtaddqty");
                    TextBox txtaddprice = (TextBox)row.FindControl("txtaddprice");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)row.FindControl("hdnshoppingcartid");
                    decimal addprice = 0;
                    try
                    {
                        decimal.TryParse(txtaddprice.Text, out addprice);
                    }
                    catch { }
                    int addqty = 0;
                    int shoppingcartid = 0;
                    try
                    {
                        int.TryParse(txtaddqty.Text, out addqty);
                    }
                    catch { }
                    try
                    {
                        int.TryParse(hdnshoppingcartid.Value.ToString(), out shoppingcartid);
                    }
                    catch { }

                    #endregion

                    if (e.CommandName == "Edit")
                    {
                        decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;

                        if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                            decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                        if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                            decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                        if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                            decLength = Convert.ToDecimal(txtLengthgrid.Text);

                        if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                            decWidth = Convert.ToDecimal(txtWidthgrid.Text);
                        int chkQty = 0;
                        string ShippingLabelFileName = "";
                        string OrderNumber = Convert.ToString(lblOrderNumber.Text);
                        string ShipmentId = "";
                        if (!string.IsNullOrEmpty(OrderNumber.ToString()))
                        {
                            if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text))
                            {
                                CountryComponent objCountry = new CountryComponent();
                                if (ddlShippingMethod.SelectedItem != null)
                                {
                                    //MWSMerchantFulfillmentService.Model.GetShipmentResponse objShippent = new MWSMerchantFulfillmentService.Model.GetShipmentResponse();
                                    MWSMerchantFulfillmentService.Model.Shipment objShippent = new MWSMerchantFulfillmentService.Model.Shipment();
                                    MWSMerchantFulfillmentService.Model.CreateShipmentResponse objlabel = new MWSMerchantFulfillmentService.Model.CreateShipmentResponse();
                                    if (!string.IsNullOrEmpty(ddlShippingMethod.SelectedItem.Text))
                                    {
                                        string sellerId = ViewState["AmazonMerchantID"].ToString();
                                        string mwsAuthToken = "";
                                        // The client application version
                                        string appVersion = "1.01";

                                        // The endpoint for region service and version (see developer guide)
                                        // ex: https://mws.amazonservices.com
                                        string serviceURL = ViewState["AmazonServiceURL"].ToString();
                                        MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
                                        config.ServiceURL = serviceURL;
                                        // Set other client connection configurations here if needed
                                        // Create the client itself
                                        MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(ViewState["AmazonAccessKey"].ToString(), ViewState["AmazonSecretKey"].ToString(), ViewState["AmazonApplicationName"].ToString(), appVersion, config);
                                        MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);

                                        DataTable dtOrder = new DataTable();
                                        dtOrder.Columns.Add("Qty", typeof(int));
                                        dtOrder.Columns.Add("AmazonItemId", typeof(String));
                                        dtOrder.Columns.Add("RefOrderId", typeof(String));
                                        DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderNumber=" + OrderNumber + "");
                                        if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                            {
                                                DataRow dataRow = dtOrder.NewRow();
                                                dataRow["qty"] = dtORderdetails.Tables[0].Rows[i]["Quantity"].ToString();
                                                dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                                                dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                                                dtOrder.Rows.Add(dataRow);
                                            }
                                        }

                                        string[] strlids = ddlShippingMethod.SelectedValue.ToString().Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        objlabel = obj.InvokeCreateShipment(hdnRefOrderNo.Value.ToString(), sellerId, "", strlids[0].ToString(), strlids[1].ToString(), decLength, decWidth, decHeight, decWeight, dtOrder);

                                        objShippent = objlabel.CreateShipmentResult.Shipment;
                                        if (objShippent == null)
                                        {
                                            return;
                                        }
                                        // objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                        string strddd = objlabel.CreateShipmentResult.Shipment.Label.FileContents.Contents.ToString();
                                        //objShippent = obj.InvokeGetShipment(sellerId, "537d8635-c5fc-4aa8-aa35-a9af0a80ab78");
                                        //string strddd = objShippent.GetShipmentResult.Shipment.Label.FileContents.Contents.ToString();

                                        byte[] imageBytes = Base64DecodeString(strddd);
                                        string fileName = objlabel.CreateShipmentResult.Shipment.TrackingId.ToString();
                                        ShipmentId = objlabel.CreateShipmentResult.Shipment.ShipmentId.ToString();
                                        CommonComponent.ExecuteCommonData("Update tb_Order set ShipmentId='" + ShipmentId + "'  where  OrderNumber=" + OrderNumber + "");
                                        // string fileName = objShippent.GetShipmentResult.Shipment.TrackingId.ToString();
                                        string imagepath = "";
                                        imagepath = "~/ShippingLabels/FEDEX/";// AppLogic.AppConfigs("FedEx.LabelSavePath");
                                        string fileFormat = AppLogic.AppConfigs("Shipping.LabelFormat").ToString();
                                        if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("USPS") > -1)
                                        {
                                            imagepath = "~/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                            fileName = "USPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                             DateTime.Now.Year.ToString() +
                             DateTime.Now.Month.ToString() +
                             DateTime.Now.Day.ToString() +
                             DateTime.Now.Hour.ToString() +
                             DateTime.Now.Minute.ToString() +
                             DateTime.Now.Second.ToString() + "-1." +
                             fileFormat.ToLower();
                                        }
                                        else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("UPS") > -1)
                                        {
                                            imagepath = "~/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                            fileName = "UPS-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                DateTime.Now.Year.ToString() +
                                DateTime.Now.Month.ToString() +
                                DateTime.Now.Day.ToString() +
                                DateTime.Now.Hour.ToString() +
                                DateTime.Now.Minute.ToString() +
                                DateTime.Now.Second.ToString() + "-1." +
                                fileFormat.ToLower();
                                        }
                                        else if (ddlShippingMethod.SelectedValue.ToString().ToUpper().IndexOf("FEDEX") > -1)
                                        {
                                            imagepath = "~/ShippingLabels/FEDEX/";//Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                            fileName = "FedEx-Package1_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(hdnshoppingcartid.Value.ToString())) ? "0" : hdnshoppingcartid.Value.ToString()) + "@" +
                                        DateTime.Now.Year.ToString() +
                                        DateTime.Now.Month.ToString() +
                                        DateTime.Now.Day.ToString() +
                                        DateTime.Now.Hour.ToString() +
                                        DateTime.Now.Minute.ToString() +
                                        DateTime.Now.Second.ToString() + "-1." + fileFormat.ToLower();
                                        }

                                        ShippingLabelFileName = fileName;
                                        FileStream fstrm = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")), FileMode.CreateNew, FileAccess.Write);
                                        BinaryWriter writer = new BinaryWriter(fstrm);
                                        writer.Write(imageBytes);
                                        writer.Close();
                                        fstrm.Close();

                                        using (FileStream fInStream = new FileStream(Server.MapPath(imagepath + fileName.Replace("." + fileFormat.ToLower(), ".gzip")),
                            FileMode.Open, FileAccess.Read))
                                        {
                                            using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                                            {
                                                using (FileStream fOutStream = new FileStream(Server.MapPath(imagepath + "temp/" + fileName),
                                                FileMode.Create, FileAccess.Write))
                                                {
                                                    byte[] tempBytes = new byte[4096];
                                                    int i;
                                                    while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                                                    {
                                                        fOutStream.Write(tempBytes, 0, i);
                                                    }
                                                }
                                            }
                                        }
                                        if (fileName.ToString().ToLower().IndexOf(".png") > -1)
                                        {
                                            ResizeImagewithpdf(imagepath + "temp", imagepath, fileName, imagepath);
                                        }
                                        else
                                        {
                                            System.IO.File.Copy(imagepath + "temp/" + fileName, imagepath + fileName);
                                        }

                                    }
                                    else
                                    {
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    decimal ShippingCost = 0;
                                    if (ddlShippingMethod.SelectedValue.ToString() != "")
                                    {
                                        int Index = ddlShippingMethod.SelectedItem.Text.IndexOf("($");
                                        int Length = ddlShippingMethod.SelectedItem.Text.LastIndexOf(")") - Index;
                                        if (Index != -1 && Length != 0)
                                        {
                                            string strShippingCost = ddlShippingMethod.SelectedItem.Text.Substring(Index + 2, Length - 2).Trim();
                                            Decimal.TryParse(strShippingCost, out ShippingCost);
                                        }
                                    }

                                    if (string.IsNullOrEmpty(ShippingLabelFileName) || ShippingLabelFileName.ToString().Trim() == "")
                                    {
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('Error While Generating Shipping Label.','Message');", true);
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(ShippingLabelFileName.Trim())) && Convert.ToString(ShippingLabelFileName.Trim()).ToLower().IndexOf("error :") > -1)
                                    {
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "jAlert('" + ShippingLabelFileName.ToString() + "','Message');", true);
                                        Session["ProductDetail"] = null;
                                        return;
                                    }
                                    if (!string.IsNullOrEmpty(ShippingLabelFileName.ToString()))
                                    {
                                        string StrShipping = Convert.ToString(ddlShippingMethod.SelectedItem.Text);
                                        if (!string.IsNullOrEmpty(StrShipping.ToString().Trim()) && StrShipping.ToLower().IndexOf("($") > -1)
                                        {
                                            StrShipping = StrShipping.Substring(0, StrShipping.IndexOf("($"));
                                        }
                                        string strdimension = Convert.ToString(Convert.ToString(decHeight) + " x " + Convert.ToString(decWidth) + " x " + Convert.ToString(decLength));

                                        CommonComponent.ExecuteCommonData("Update tb_Order set IsBackendProcessed=1,ShipmentId='" + ShipmentId + "', ShippingDimension ='" + strdimension + "', ShippingLabelFileName='" + ShippingLabelFileName.ToString() + "',ShippingLabelMethod='" + ddlShippingMethod.SelectedItem.Text + "',ShippingLabelCost=" + ShippingCost + ",ShippingLabelWeight=" + decWeight + ",ShippingLabelPackageHeight=" + decHeight + ",ShippingLabelPackageWidth=" + decWidth + ",ShippingLabelPackageLength=" + decLength + "  where  OrderNumber=" + OrderNumber + "");
                                        DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID,SKU FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderNumber=" + OrderNumber + "");
                                        if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                                            {
                                                CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_PackageHeader " + OrderNumber + ",'" + objlabel.CreateShipmentResult.Shipment.TrackingId.ToString() + "','" + objlabel.CreateShipmentResult.Shipment.ShippingService.CarrierName.ToString() + "','','" + objlabel.CreateShipmentResult.Shipment.ShippingService.ShipDate.ToString() + "','" + dtORderdetails.Tables[0].Rows[i]["SKU"].ToString() + "'," + dtORderdetails.Tables[0].Rows[i]["Quantity"].ToString() + "");

                                            }
                                        }
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_AmazonlabelDetails(OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, ShipQty, TrackingNumber, CreatedOn) SELECT OrderNumber, OrderedCustomcartId, ShipmentId, ShippingDimension, ShippingLabelFileName, ShippingLabelMethod, ShippingLabelCost, ShippingLabelWeight, ShippingLabelPackageHeight, ShippingLabelPackageWidth, ShippingLabelPackageLength, Quantity, TrackingNumber, getdate()  FROM tb_OrderedShoppingCartItems INNER JOIN tb_Order on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID WHERE OrderNumber=" + OrderNumber + "");
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@UpdateMsg", "jAlert('Shipping Label Generated Successfully.','Success');", true);

                                        this.GetCustomersPageWise(pageIndex);

                                    }
                                }
                            }
                        }
                    }

                    if (e.CommandName == "Print")
                    {
                        try
                        {
                            string strGenFile = "";
                            string StrMergeFile = "";
                            bool IsBatchOrder = false;
                            if (grvOrderlist.Rows.Count > 0)
                            {
                                string StrBatchId = "";
                                string StrOrderNmumder = "";
                                StrOrderNmumder += lblOrderNumber.Text.ToString() + ",";
                                if (StrOrderNmumder.Length > 0)
                                {
                                    string[] StrLabels = Regex.Split(StrOrderNmumder.ToString().Trim(), ",");
                                    litSlip.InnerHtml = "";
                                    for (int i = 0; i < StrLabels.Length; i++)
                                    {
                                        if (StrLabels[i].ToString().Trim() != "")
                                        {
                                            strGenFile = SlipOrder(Convert.ToInt32(StrLabels[i].ToLower()), Convert.ToInt32(StrLabels.Length), i);
                                            StrMergeFile = StrMergeFile + strGenFile + ",";
                                        }
                                    }


                                    string strPDf = GetFilesPDFprint(StrOrderNmumder, StrMergeFile);
                                    string BatchID = StrOrderNmumder;
                                    string BatchPdf = "/ShippingLabels/BatchPDF/";
                                    BatchPdf = Server.MapPath(BatchPdf);
                                    if (!System.IO.Directory.Exists(BatchPdf))
                                    {
                                        System.IO.Directory.CreateDirectory(BatchPdf);
                                    }
                                    Random rd = new Random();
                                    if (File.Exists(BatchPdf + "/" + BatchID + ".pdf"))
                                    {
                                        string pdfOpenPath = "PrintBarcode('/ShippingLabels/BatchPDF/" + BatchID.ToString() + ".pdf?" + rd.Next(1000).ToString() + "');";

                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msggg" + rd.Next(1000).ToString(), pdfOpenPath, true);
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {

                            // CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
                        }
                    }
                    if (e.CommandName == "Packaingslip")
                    {
                        try
                        {
                            Session["PrintOrderId"] = "";
                            Int32 Count = 0;
                            //CheckBox chkgeneratelbl = (CheckBox)row.FindControl("chkgeneratelbl");

                            //if (chkgeneratelbl.Checked)
                            //{
                                if (Count == 0)
                                    Session["PrintOrderId"] += lblOrderNumber.Text.ToString();
                                else
                                    Session["PrintOrderId"] += "," + lblOrderNumber.Text.ToString();

                                Count++;
                            //}
                            if (!string.IsNullOrEmpty(Session["PrintOrderId"].ToString()))
                            {
                                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "subscribescript", "window.open('PrintMultipleSlip.aspx?amzid=1', '','height=750,width=850,scrollbars=1');", true);
                            }
                        }
                        catch { }
                    }
                    if (e.CommandName == "RefreshShipping")
                    {
                        try
                        {
                            decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;
                            Int32 OrderNumber = 0;
                            OrderNumber = Convert.ToInt32(lblOrderNumber.Text);

                            if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                                decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                            if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                                decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                            if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                                decLength = Convert.ToDecimal(txtLengthgrid.Text);

                            if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                                decWidth = Convert.ToDecimal(txtWidthgrid.Text);

                            BindShippingMethod(ddlShippingMethod, OrderNumber, decWeight, decHeight, decWidth, decLength, ddlWareHouse, ddlMailpieceShape.SelectedItem.Text);
                        }
                        catch { }
                    }

                    if (e.CommandName == "Download")
                    {

                        string StrShipVia = Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(ShippedVia,'')+','  from tb_ordershippeditems where Ordernumber=" + lblOrderNumber.Text.ToString() + " FOR XML PATH('')"));
                        if (!string.IsNullOrEmpty(StrShipVia.ToString().Trim()))
                        {
                            String LabelFPath = "";
                            if (StrShipVia.ToString().ToUpper().IndexOf("USPS") > -1)
                            {
                                LabelFPath = "~/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                            }
                            if (StrShipVia.ToString().ToUpper().IndexOf("UPS") > -1)
                            {
                                LabelFPath = "~/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                            }
                            if (StrShipVia.ToString().ToUpper().IndexOf("FEDEX") > -1)
                            {
                                LabelFPath = " 	~/ShippingLabels/FEDEX/";// Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                            }
                            try
                            {

                                Label lblShippingLabelFileName = (Label)row.FindControl("lblShippingLabelFileName");

                                string lblShippingLabelFileName1 = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT DISTINCT ShippingLabelFileName+',' FROM tb_AmazonlabelDetails WHERE Ordernumber=" + lblOrderNumber.Text.ToString() + " FOR XML PATH('')"));
                                //CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                                //if (chkSelect.Checked)
                                {

                                    if (!string.IsNullOrEmpty(lblShippingLabelFileName1.ToString().Trim()))
                                    {
                                        if (lblShippingLabelFileName1.Trim().ToLower().Contains(","))
                                        {
                                            DownloadMoreLabels(LabelFPath, lblShippingLabelFileName1.Trim());
                                        }
                                        else
                                        {
                                            downloadfile(LabelFPath, lblShippingLabelFileName.Text.ToString().Trim());
                                        }
                                    }
                                    else
                                    {
                                        DownloadMoreLabelsFromShipping(LabelFPath, lblOrderNumber.Text.ToString());
                                    }
                                }
                            }
                            catch { Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgdownload", "jAlert('Shipping Label Not Found.','Message');", true); return; }
                        }
                        else { Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgdownload", "jAlert('Shipping Label Not Found.','Message');", true); return; }

                    }

                }
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Download Shipping  Lable Function
        /// </summary>
        /// <param name="LabelFPath"></param>
        /// <param name="OrderNo"></param>
        public void DownloadMoreLabelsFromShipping(string LabelFPath, String OrderNo)
        {
            try
            {
                if (Directory.Exists(Server.MapPath(LabelFPath)))
                {
                    DirectoryInfo di = new DirectoryInfo(Server.MapPath(LabelFPath));
                    FileInfo[] rgFiles = di.GetFiles("*_" + OrderNo.ToString().Trim() + "_*.*");
                    if (rgFiles.Length > 0)
                    {
                        string ShippingLabelFileName = "";
                        if (rgFiles.Length > 1)
                        {
                            for (int i = 0; i < rgFiles.Length; i++)
                            {
                                ShippingLabelFileName += rgFiles[i].ToString() + ",";
                            }
                            DownloadMoreLabels(LabelFPath, ShippingLabelFileName.ToString().Trim());
                        }
                        else
                        {
                            downloadfile(LabelFPath, rgFiles[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Download More Lable 
        /// </summary>
        /// <param name="LabelFPath"></param>
        /// <param name="ShippingLabelFileName"></param>
        public void DownloadMoreLabels(string LabelFPath, string ShippingLabelFileName)
        {
            try
            {
                string[] StrLabels = ShippingLabelFileName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (LabelFPath.ToString().Contains("~"))
                {
                    string[] aar = LabelFPath.Split('~');
                    LabelFPath = aar[1].ToString();
                    if (StrLabels.Length > 0)
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            for (int i = 0; i < StrLabels.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(StrLabels[i].ToString()))
                                {

                                    if (StrLabels[i].ToString().ToUpper().IndexOf("USPS") > -1)
                                    {
                                        LabelFPath = "~/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                    }
                                    if (StrLabels[i].ToString().ToUpper().IndexOf("UPS") > -1)
                                    {
                                        LabelFPath = "~/ShippingLabels/UPS/";//Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                    }
                                    if (StrLabels[i].ToString().ToUpper().IndexOf("FEDEX") > -1)
                                    {
                                        LabelFPath = " 	~/ShippingLabels/FEDEX/";// Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                    }

                                    string filePath = Server.MapPath(LabelFPath + StrLabels[i].ToString());
                                    if (System.IO.File.Exists(Server.MapPath(LabelFPath + "/" + StrLabels[i].ToString())))
                                    {
                                        zip.AddFile(filePath, "files");
                                    }
                                }
                            }
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedFile.zip");
                            Response.ContentType = "application/zip";
                            zip.Save(Response.OutputStream);
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Download Files from  Specified File Path
        /// </summary>
        /// <param name="filepath">string file  path</param>
        private void downloadfile(string LabelFPath, string filepath)
        {
            try
            {
                string[] aar = LabelFPath.Split('~');
                LabelFPath = aar[1].ToString();

                FileInfo file = new FileInfo(Server.MapPath(LabelFPath + "/" + filepath));
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = ReturnExtension(file.Extension.ToLower());
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);
                }
            }
            catch { }
            Response.End();
        }

        /// <summary>
        /// Returns the Extension
        /// </summary>
        /// <param name="fileExtension">string fileExtension</param>
        /// <returns>Returns the File Extension</returns>
        private string ReturnExtension(string fileExtension)
        {
            try
            {
                switch (fileExtension)
                {
                    case ".htm":
                    case ".html":
                    case ".log":
                        return "text/HTML";
                    case ".txt":
                        return "text/plain";
                    case ".doc":
                        return "application/ms-word";
                    case ".tiff":
                    case ".tif":
                        return "image/tiff";
                    case ".asf":
                        return "video/x-ms-asf";
                    case ".avi":
                        return "video/avi";
                    case ".zip":
                        return "application/zip";
                    case ".xls":
                    case ".csv":
                        return "application/vnd.ms-excel";
                    case ".gif":
                        return "image/gif";
                    case ".jpg":
                    case "jpeg":
                        return "image/jpeg";
                    case ".bmp":
                        return "image/bmp";
                    case ".wav":
                        return "audio/wav";
                    case ".mp3":
                        return "audio/mpeg3";
                    case ".mpg":
                    case "mpeg":
                        return "video/mpeg";
                    case ".rtf":
                        return "application/rtf";
                    case ".asp":
                        return "text/asp";
                    case ".pdf":
                        return "application/pdf";
                    case ".fdf":
                        return "application/vnd.fdf";
                    case ".ppt":
                        return "application/mspowerpoint";
                    case ".dwg":
                        return "image/vnd.dwg";
                    case ".msg":
                        return "application/msoutlook";
                    case ".xml":
                    case ".sdxl":
                        return "application/xml";
                    case ".xdp":
                        return "application/vnd.adobe.xdp+xml";
                    default:
                        return "application/octet-stream";
                }
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
                return "";

            }
        }



        /// <summary>
        /// Order List Gridview Row editing Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvOrderlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                grvOrderlist.PageIndex = 0;
            }
            catch
            {
            }
        }

        protected void btnPrintLabel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strGenFile = "";
                string StrMergeFile = "";
                bool IsBatchOrder = false;
                if (grvOrderlist.Rows.Count > 0)
                {
                    string StrBatchId = "";
                    string StrOrderNmumder = "";


                    for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                    {
                        Label lblShippingLabelFileName = (Label)grvOrderlist.Rows[i].FindControl("lblShippingLabelFileName");
                        Label lblOrderNumber = (Label)grvOrderlist.Rows[i].FindControl("lblOrderNumber");
                        CheckBox chkSelect = (CheckBox)grvOrderlist.Rows[i].FindControl("chkSelect");
                        if (chkSelect.Checked)
                        {
                            IsBatchOrder = true;
                            StrOrderNmumder += lblOrderNumber.Text.ToString() + ",";
                        }
                    }
                    if (IsBatchOrder == true && StrOrderNmumder.Length > 0)
                    {
                        string[] StrLabels = Regex.Split(StrOrderNmumder.ToString().Trim(), ",");
                        litSlip.InnerHtml = "";
                        for (int i = 0; i < StrLabels.Length; i++)
                        {
                            if (StrLabels[i].ToString().Trim() != "")
                            {
                                strGenFile = SlipOrder(Convert.ToInt32(StrLabels[i].ToLower()), Convert.ToInt32(StrLabels.Length), i);
                                StrMergeFile = StrMergeFile + strGenFile + ",";
                            }
                        }


                        string strPDf = GetFilesPDFprint(StrOrderNmumder, StrMergeFile);
                        string BatchID = StrOrderNmumder;
                        string BatchPdf = "/ShippingLabels/BatchPDF/";
                        BatchPdf = Server.MapPath(BatchPdf);
                        if (!System.IO.Directory.Exists(BatchPdf))
                        {
                            System.IO.Directory.CreateDirectory(BatchPdf);
                        }
                        if (File.Exists(BatchPdf + "/" + BatchID + ".pdf"))
                        {
                            string pdfOpenPath = "window.open('/ShippingLabels/BatchPDF/" + BatchID.ToString() + ".pdf','_blank');";
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msggg", pdfOpenPath, true);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgggErr", "jAlert('No Label Found!!','Message');", true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
            }
        }


        /// <summary>
        /// Generate File PDF
        /// </summary>
        /// <param name="BatchID"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetFilesPDFprint(string BatchID, string filename)
        {
            try
            {
                string PDFPath = "/ShippingLabels/PDFPrint/";
                PDFPath = Server.MapPath(PDFPath);

                string filePath = "/ShippingLabels/BatchPDF/";
                filePath = Server.MapPath(filePath);

                if (!System.IO.Directory.Exists(PDFPath))
                {
                    System.IO.Directory.CreateDirectory(PDFPath);
                }

                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }

                string print = "";
                MergeDocs(filePath + "/" + BatchID + ".pdf", filename, ref print);

                return "";
            }
            catch (Exception ex)
            {

                CommonComponent.ErrorLog(Request.Path.ToString(), ex.Message, ex.StackTrace);
                return "";
            }
        }

        /// <summary>
        /// Merge Documnets
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="shippinglabelfiles"></param>
        /// <param name="print"></param>
        private void MergeDocs(string filePath, string shippinglabelfiles, ref string print)
        {
            Document document = new Document();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                int n = 0;
                int rotation = 0;

                string[] AllshipinglablefilesArr = shippinglabelfiles.Split(',');

                print = "0";
                foreach (string filename in AllshipinglablefilesArr)
                {

                    if (File.Exists(filename))
                    {
                        print = "1";
                    }
                    else
                    {
                        continue;
                    }

                    string pdffilename = "";

                    if (filename != "" && filename.Length > 0)
                    {
                        if (filename.Contains(".gif"))
                            pdffilename = filename.Replace(".gif", ".pdf");
                        else if (filename.Contains(".jpg"))
                            pdffilename = filename.Replace(".jpg", ".pdf");
                        else if (filename.Contains(".jpeg"))
                            pdffilename = filename.Replace(".jpeg", ".pdf");
                        else if (filename.Contains(".png"))
                            pdffilename = filename.Replace(".png", ".pdf");
                        else if (filename.Contains(".pdf"))
                            pdffilename = filename;
                        PdfReader reader = new PdfReader(pdffilename);

                        n = reader.NumberOfPages;

                        int i = 0;
                        while (i < n)
                        {
                            i++;
                            document.SetPageSize(reader.GetPageSizeWithRotation(1));
                            document.NewPage();

                            if (i == 1)
                            {
                                Chunk fileRef = new Chunk(" ");
                                fileRef.SetLocalDestination(pdffilename);
                                document.Add(fileRef);
                            }

                            page = writer.GetImportedPage(reader, i);
                            rotation = reader.GetPageRotation(i);
                            if (rotation == 90 || rotation == 270)
                            {
                                cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                            }
                            else
                            {
                                cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { throw e; }
            finally { document.Close(); }
        }

        /// <summary>
        /// Get Order Slip
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="TotRec"></param>
        /// <param name="CurrRec"></param>
        /// <returns></returns>
        protected string SlipOrder(int OrderId, int TotRec, int CurrRec)
        {
            string strMergeFile = "";
            string strgetFile = "";
            try
            {
                string LabelPrintCopyPath = "";

                string PDFPath = "/ShippingLabels/PDFPrint/";
                PDFPath = Server.MapPath(PDFPath);
                LabelPrintCopyPath = "/ShippingLabels/CombineMethod/";
                LabelPrintCopyPath = Server.MapPath(LabelPrintCopyPath);

                if (!System.IO.Directory.Exists(PDFPath))
                {
                    System.IO.Directory.CreateDirectory(PDFPath);
                }

                if (!System.IO.Directory.Exists(LabelPrintCopyPath))
                {
                    System.IO.Directory.CreateDirectory(LabelPrintCopyPath);
                }
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + Convert.ToInt32(OrderId))), out StoreID);
                AppConfig.StoreID = StoreID;
                string StrQuery = "SElect distinct ISNULL(TrackingNumber,'') as TrackingNumber,ISNULL(ShippedVia,'') as ShippedVia from tb_OrderShippedItems where OrderNumber in (" + OrderId.ToString() + ") AND isnull(TrackingNumber,'') <> ''";
                DataSet dsTackingNo = CommonComponent.GetCommonDataSet(StrQuery);
                if (dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0)
                {
                    string type = Convert.ToString(dsTackingNo.Tables[0].Rows[0]["ShippedVia"].ToString());
                    String LabelFPath = "~/ShippingLabels/USPS/";
                    String LabelFPath1 = "~/ShippingLabels/UPS/";
                    String LabelFPath2 = "~/ShippingLabels/FEDEX/";
                    //if (type.ToString().ToUpper().IndexOf("USPS") > -1)
                    //{
                    //    LabelFPath = "~/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                    //}
                    //if (type.ToString().ToUpper().IndexOf("UPS") > -1)
                    //{
                    //    LabelFPath = "~/ShippingLabels/UPS/"; //Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                    //}
                    //if (type.ToString().ToUpper().IndexOf("FEDEX") > -1)
                    //{
                    //    LabelFPath = "~/ShippingLabels/FEDEX/"; //Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                    //}
                    string ShippingLabelFileName = "";// Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLabelFileName"].ToString());
                    string[] strFile = { "" };
                    for (int i = 1; i < 4; i++)
                    {

                        if (i == 1)
                        {
                            strFile = System.IO.Directory.GetFiles(Server.MapPath(LabelFPath), "*_" + OrderId.ToString() + "_*");
                        }
                        else if (i == 2)
                        {
                            strFile = System.IO.Directory.GetFiles(Server.MapPath(LabelFPath1), "*_" + OrderId.ToString() + "_*");
                        }
                        else if (i == 3)
                        {
                            strFile = System.IO.Directory.GetFiles(Server.MapPath(LabelFPath2), "*_" + OrderId.ToString() + "_*");
                        }

                        foreach (string str in strFile)
                        {
                            if (str.IndexOf("_" + OrderId.ToString() + "_") > -1)
                            {
                                FileInfo fl = new FileInfo(str);

                                //if (type.ToString().ToUpper().IndexOf("USPS") > -1)
                                //{
                                //    object objShippingCountry = CommonComponent.GetScalarCommonData("SELECT ShippingCountry FROM dbo.tb_Order  WHERE OrderNumber='" + OrderId.ToString() + "'");

                                //    if (objShippingCountry != null && Convert.ToString(objShippingCountry).ToLower() != "united states")
                                //    {
                                //        DirectoryInfo di = new DirectoryInfo(Server.MapPath(LabelFPath));
                                //        FileInfo[] rgFiles = di.GetFiles("*_" + OrderId.ToString() + "_*");
                                //        if (rgFiles.Length > 1)
                                //        {
                                //            strgetFile = ResizeImageandPDFInternational(Server.MapPath(LabelFPath), LabelPrintCopyPath, fl.Name.ToString(), PDFPath);
                                //            strMergeFile = strMergeFile + strgetFile + ",";
                                //        }
                                //        else
                                //        {
                                //            strgetFile = ResizeImagewithpdf(Server.MapPath(LabelFPath), LabelPrintCopyPath, fl.Name.ToString(), PDFPath);
                                //            strMergeFile = strMergeFile + strgetFile + ",";
                                //        }

                                //    }
                                //    else
                                //    {
                                //        strgetFile = ResizeImagewithpdf(Server.MapPath(LabelFPath), LabelPrintCopyPath, fl.Name.ToString(), PDFPath);
                                //        strMergeFile = strMergeFile + strgetFile + ",";
                                //    }
                                //}
                                //else

                                // strgetFile = ResizeImagewithpdf(Server.MapPath(LabelFPath), LabelPrintCopyPath, fl.Name.ToString(), PDFPath);
                                if (str.ToLower().IndexOf(".pdf") > -1)
                                {
                                    strgetFile = str.ToString();//ResizeImagewithpdf(Server.MapPath(LabelFPath), LabelPrintCopyPath, fl.Name.ToString(), PDFPath);
                                    strMergeFile = strMergeFile + strgetFile + ",";
                                }


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return strMergeFile;
        }


        /// <summary>
        /// Resize PDF Internations
        /// </summary>
        /// <param name="Orgpath"></param>
        /// <param name="destpath"></param>
        /// <param name="fileName"></param>
        /// <param name="PDFPath"></param>
        /// <returns></returns>
        public String ResizeImageandPDFInternational(string Orgpath, string destpath, string fileName, string PDFPath)
        {

            string strReturn = "";
            try
            {
                if (File.Exists(Orgpath + "/" + fileName))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(Orgpath + "/" + fileName))
                    {
                        Bitmap newBMP = new Bitmap(1800, 1200);
                        newBMP.SetResolution(300, 300);
                        Graphics g = Graphics.FromImage(newBMP);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 1800, 1200);
                        g.DrawImage(img, rect);
                        g.Dispose();
                        img.Dispose();
                        newBMP.Save(destpath + "/" + fileName);

                        try
                        {
                            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                            PdfSharp.Pdf.PdfPage PFNEW = new PdfSharp.Pdf.PdfPage();
                            PFNEW.Size = PdfSharp.PageSize.RA5;
                            XUnit xy = new XUnit(290F);
                            XUnit xy1 = new XUnit(435F);
                            PFNEW.Height = xy;
                            PFNEW.Width = xy1;
                            doc.Pages.Add(PFNEW);
                            XGraphics xgr = XGraphics.FromPdfPage(PFNEW);
                            FileInfo fl = new FileInfo(PDFPath + "/" + fileName);
                            string strname = fl.FullName.Replace(fl.Name.ToString(), "");
                            XImage img1 = XImage.FromFile(destpath + "/" + fileName);
                            xgr.DrawImage(img1, 0, 0);
                            doc.Save(fl.FullName.Replace(fl.Extension.ToString(), ".pdf"));

                            strReturn = fl.FullName.Replace(fl.Extension.ToString(), ".pdf");
                            img1.Dispose();
                            doc.Close();
                        }
                        catch { }
                    }
                }
            }
            catch { }
            return strReturn;
        }

        /// <summary>
        /// Resize Image with PDF
        /// </summary>
        /// <param name="Orgpath"></param>
        /// <param name="destpath"></param>
        /// <param name="fileName"></param>
        /// <param name="PDFPath"></param>
        /// <returns></returns>
        public String ResizeImagewithpdf(string Orgpath, string destpath, string fileName, string PDFPath)
        {
            string strReturn = "";
            try
            {
                if (File.Exists(Orgpath + "/" + fileName))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(Orgpath + "/" + fileName))
                    {

                        Bitmap newBMP = new Bitmap(1200, 1800);
                        newBMP.SetResolution(300, 300);
                        Graphics g = Graphics.FromImage(newBMP);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 1200, 1800);
                        g.DrawImage(img, rect);
                        g.Dispose();
                        img.Dispose();
                        newBMP.Save(destpath + fileName);
                        try
                        {
                            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                            PdfSharp.Pdf.PdfPage PFNEW = new PdfSharp.Pdf.PdfPage();
                            PFNEW.Size = PdfSharp.PageSize.RA5;
                            XUnit xy = new XUnit(435F);
                            XUnit xy1 = new XUnit(290F);
                            PFNEW.Height = xy;
                            PFNEW.Width = xy1;
                            doc.Pages.Add(PFNEW);
                            XGraphics xgr = XGraphics.FromPdfPage(PFNEW);
                            FileInfo fl = new FileInfo(PDFPath + fileName);
                            string strname = fl.FullName.Replace(fl.Name.ToString(), "");
                            XImage img1 = XImage.FromFile(destpath + fileName);
                            xgr.DrawImage(img1, 0, 0);
                            doc.Save(fl.FullName.Replace(fl.Extension.ToString(), ".pdf"));

                            strReturn = fl.FullName.Replace(fl.Extension.ToString(), ".pdf");
                            img1.Dispose();
                            doc.Close();


                        }
                        catch { }

                    }
                }
            }
            catch { }
            return strReturn;
        }

        protected void btnprintslip_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                Session["PrintOrderId"] = "";
                Int32 Count = 0;
                for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                {
                    Label lblShippingLabelFileName = (Label)grvOrderlist.Rows[i].FindControl("lblShippingLabelFileName");
                    Label lblOrderNumber = (Label)grvOrderlist.Rows[i].FindControl("lblOrderNumber");
                   CheckBox chkpackageSelect = (CheckBox)grvOrderlist.Rows[i].FindControl("chkgeneratelbl");
                    if (chkpackageSelect.Checked)
                    {
                        if (Count == 0)
                            Session["PrintOrderId"] += lblOrderNumber.Text.ToString();
                        else
                            Session["PrintOrderId"] += "," + lblOrderNumber.Text.ToString();

                        Count++;
                    }
                }

                if (!string.IsNullOrEmpty(Session["PrintOrderId"].ToString()))
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "subscribescript", "var w1=window.open('PrintMultipleSlip.aspx?amzid=1', '','height=750,width=850,scrollbars=1'); ", true); //w1.print();w1.close();
                }
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select atleast one order to print.','Message');", true);

            }
            catch { }
        }


        protected void grvOrderlist_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvOrderlist.PageIndex = e.NewPageIndex;

            this.GetCustomersPageWise(pageIndex);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["GetOrderListByBatchIDs"] = null;
            grvOrderlist.PageIndex = 0;
            GetCustomersPageWise(1);
        }

        protected void txtProWeight_TextChanged(object sender, EventArgs e)
        {

            TextBox txt = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txt.Parent.Parent;
            TextBox txtProWeight = (TextBox)gvRow.FindControl("txtProWeight");
            TextBox txtHeightgrid = (TextBox)gvRow.FindControl("txtHeightgrid");
            TextBox txtWidthgrid = (TextBox)gvRow.FindControl("txtWidthgrid");
            TextBox txtLengthgrid = (TextBox)gvRow.FindControl("txtLengthgrid");
            Label lblOrderNumber = (Label)gvRow.FindControl("lblOrderNumber");
            DropDownList ddlShippingMethod = (DropDownList)gvRow.FindControl("ddlShippingMethod");
            DropDownList ddlWareHouse = (DropDownList)gvRow.FindControl("ddlWareHouse");
            DropDownList ddlDimensions = (DropDownList)gvRow.FindControl("ddlDimensions");
            DropDownList ddlMailpieceShape = (DropDownList)gvRow.FindControl("ddlMailpieceShape");

            try
            {
                decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;
                Int32 OrderNumber = 0;
                OrderNumber = Convert.ToInt32(lblOrderNumber.Text);

                if (!string.IsNullOrEmpty(txtProWeight.Text.ToString()))
                    decWeight = Convert.ToDecimal(txtProWeight.Text.ToString());

                if (!string.IsNullOrEmpty(txtHeightgrid.Text.ToString()))
                    decHeight = Convert.ToDecimal(txtHeightgrid.Text);

                if (!string.IsNullOrEmpty(txtLengthgrid.Text.ToString()))
                    decLength = Convert.ToDecimal(txtLengthgrid.Text);

                if (!string.IsNullOrEmpty(txtWidthgrid.Text.ToString()))
                    decWidth = Convert.ToDecimal(txtWidthgrid.Text);

                BindShippingMethod(ddlShippingMethod, OrderNumber, decWeight, decHeight, decWidth, decLength, ddlWareHouse, ddlMailpieceShape.SelectedItem.Text);
            }
            catch { }
        }

        protected void btnlnkPrintallLabel_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void grvOrderlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                string strUserAgent = Request.UserAgent.ToString().ToLower();
                Literal ltripadONO = (Literal)e.Row.FindControl("ltripadONO");
                if (strUserAgent != null && strUserAgent.ToString().ToLower().IndexOf("ipad") > -1)
                {
                    if (e.Row.RowType == DataControlRowType.Header)
                    {

                        ImageButton btngeneratealllabel = (ImageButton)e.Row.FindControl("btngeneratealllabel");
                        btngeneratealllabel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save-and-generate-label.gif";

                        ImageButton btnlnkPrintalllabel = (ImageButton)e.Row.FindControl("btnlnkPrintalllabel");
                        btnlnkPrintalllabel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-label.gif";

                        e.Row.Cells[1].Visible = e.Row.Cells[2].Visible = false;
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ltripadONO.Visible = true;
                        e.Row.Cells[1].Visible = e.Row.Cells[2].Visible = false;
                        ltripadONO.Text = "Order# <a style='color:#b92127 !important;' class=\"order-no\" href=\"/Admin/Orders/Orders.aspx?id=" + ltripadONO.Text.ToString() + "\">" + ltripadONO.Text.ToString() + "</a><br /><br />";
                    }
                }

                if (e.Row.RowType == DataControlRowType.Header)
                {

                    ImageButton btngeneratealllabel = (ImageButton)e.Row.FindControl("btngeneratealllabel");
                    btngeneratealllabel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save-and-generate-label.gif";

                    ImageButton btnlnkPrintalllabel = (ImageButton)e.Row.FindControl("btnlnkPrintalllabel");
                    btnlnkPrintalllabel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-label.gif";


                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    ImageButton bntDownUSPS = (ImageButton)e.Row.FindControl("bntDownUSPS");
                    bntDownUSPS.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/downloadAmazon-pdf.jpg";

                    ImageButton btnSearchlabel = (ImageButton)e.Row.FindControl("btnSearchlabel");
                    btnSearchlabel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save-and-generate-label.gif";

                    ImageButton btnRemoveBatchOrder = (ImageButton)e.Row.FindControl("btnRemoveBatchOrder");
                    btnRemoveBatchOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/remove-from-batch.gif";

                    ImageButton btnRebindShippingMethods = (ImageButton)e.Row.FindControl("btnRebindShippingMethods");
                    btnRebindShippingMethods.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/refresh-icon.png";

                    GridrowCount++;
                    Literal ltr2 = (Literal)e.Row.FindControl("ltr2");

                    Literal ltr3 = (Literal)e.Row.FindControl("ltr3");
                    Literal ltr4 = (Literal)e.Row.FindControl("ltr4");
                    Literal ltrCustomerShipping = (Literal)e.Row.FindControl("ltrCustomerShipping");

                    Literal ltrTackingNo = (Literal)e.Row.FindControl("ltrTackingNo");
                    TextBox txtHeightgrid = (TextBox)e.Row.FindControl("txtHeightgrid");
                    TextBox txtWidthgrid = (TextBox)e.Row.FindControl("txtWidthgrid");
                    TextBox txtLengthgrid = (TextBox)e.Row.FindControl("txtLengthgrid");
                    TextBox txtProWeight = (TextBox)e.Row.FindControl("txtProWeight");
                    Label lblOrderNumber = (Label)e.Row.FindControl("lblOrderNumber");
                    Label lblGenerateLabelMsg = (Label)e.Row.FindControl("lblGenerateLabelMsg");
                    DropDownList ddlShippingMethod = (DropDownList)e.Row.FindControl("ddlShippingMethod");
                    DropDownList ddlWareHouse = (DropDownList)e.Row.FindControl("ddlWareHouse");
                    Label lblShippingLabelFileName = (Label)e.Row.FindControl("lblShippingLabelFileName");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");

                    HiddenField hdnchecklblgenerate = (HiddenField)e.Row.FindControl("hdnchecklblgenerate");
                    DropDownList ddlMailpieceShape = (DropDownList)e.Row.FindControl("ddlMailpieceShape");

                    HiddenField hdnDimensionValue = (HiddenField)e.Row.FindControl("hdnDimensionValue");
                    Label lblDimensionName = (Label)e.Row.FindControl("lblDimensionName");
                    System.Web.UI.HtmlControls.HtmlGenericControl divHWL = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("dimHWL");

                    HtmlInputHidden hdnRefOrderNo = (HtmlInputHidden)e.Row.FindControl("hdnRefOrderNo");
                    Literal ltrRefOrderNo = (Literal)e.Row.FindControl("ltrRefOrderNo");
                    Literal ltramazonlogo = (Literal)e.Row.FindControl("ltramazonlogo");
                    //ImageButton btnprintslip = (ImageButton)e.Row.FindControl("btnprintslip");
                    //btnprintslip.OnClientClick = "PrintBarcode('/Admin/Orders/PackingSlipMultiwarehouse.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(lblOrderNumber.Text.ToString())) + "'); return false;";
                    if (ChkIsPrime.Checked)
                    {
                        ltramazonlogo.Text = "<img src=\"/admin/images/amazon_prime.png\" class=\"logosize\" border=\"none\" />";
                    }
                    if (!string.IsNullOrEmpty(hdnRefOrderNo.Value.ToString()))
                    {
                        ltrRefOrderNo.Text = "<b> " + ltrRefOrderNo.Text.ToString() + "</b>";
                    }
                    else
                    {
                        ltrRefOrderNo.Visible = false;
                    }

                    Int32 OrderNumber = Convert.ToInt32(ltr2.Text.Trim());
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnshoppingcartid = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnshoppingcartid");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnorderStatus = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnorderStatus");

                    btnRemoveBatchOrder.Visible = false;

                    // Button bntDownUSPS = (Button)e.Row.FindControl("bntDownUSPS");
                    //bntDownUSPS.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");

                    btnRebindShippingMethods.Attributes.Add("onclick", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.UniqueID.ToString() + "');");
                    btnSearchlabel.Attributes.Add("onclick", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.UniqueID.ToString() + "');");

                    HtmlInputHidden hndProductId = (HtmlInputHidden)e.Row.FindControl("hndProductId");

                    TextBox txtaddtionalprice = (TextBox)e.Row.FindControl("txtaddtionalprice");

                    System.Web.UI.HtmlControls.HtmlGenericControl divaddproductdetail = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("divaddproductdetail");

                    Literal lblAmazonTrackingNo = (Literal)e.Row.FindControl("lblAmazonTrackingNo");
                    ImageButton btnPrintLabel = (ImageButton)e.Row.FindControl("btnPrintLabel");

                    #region Shipped Order Details

                    Label lblShippingMethod = (Label)e.Row.FindControl("lblShippingMethod");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelMethod = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelMethod");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelWeight = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelWeight");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageHeight = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageHeight");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageWidth = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageWidth");
                    System.Web.UI.HtmlControls.HtmlInputHidden hdnShippingLabelPackageLength = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnShippingLabelPackageLength");

                    #endregion

                    #region Shipping Address

                    HtmlInputHidden hdnAddress1 = (HtmlInputHidden)e.Row.FindControl("hdnAddress1");
                    HtmlInputHidden hdnAddress2 = (HtmlInputHidden)e.Row.FindControl("hdnAddress2");
                    HtmlInputHidden hdnSuite = (HtmlInputHidden)e.Row.FindControl("hdnSuite");
                    HtmlInputHidden hdnCity = (HtmlInputHidden)e.Row.FindControl("hdnCity");
                    HtmlInputHidden hdnState = (HtmlInputHidden)e.Row.FindControl("hdnState");
                    HtmlInputHidden hdnPhone = (HtmlInputHidden)e.Row.FindControl("hdnPhone");
                    HtmlInputHidden hdnCountry = (HtmlInputHidden)e.Row.FindControl("hdnCountry");
                    HtmlInputHidden hdnZip = (HtmlInputHidden)e.Row.FindControl("hdnZip");
                    HtmlInputHidden hdnCompany = (HtmlInputHidden)e.Row.FindControl("hdnCompany");
                    HtmlInputHidden hdnShippingMethod = (HtmlInputHidden)e.Row.FindControl("hdnShippingMethod");

                    HiddenField hdnOrderShippingCosts = (HiddenField)e.Row.FindControl("hdnOrderShippingCosts");
                    ltr4.Text += "<br />";
                    if (!string.IsNullOrEmpty(hdnCompany.Value.ToString()))
                    {
                        ltr4.Text += hdnCompany.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnAddress1.Value.ToString()))
                    {
                        ltr4.Text += hdnAddress1.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnAddress2.Value.ToString()))
                    {
                        ltr4.Text += hdnAddress2.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnSuite.Value.ToString()))
                    {
                        ltr4.Text += hdnSuite.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnCity.Value.ToString()))
                    {
                        ltr4.Text += hdnCity.Value.ToString() + ", " + hdnState.Value.ToString() + " " + hdnZip.Value.ToString() + "<br />";
                        ViewState["Zip"] = hdnZip.Value.ToString();
                    }
                    if (!string.IsNullOrEmpty(hdnCountry.Value.ToString()))
                    {
                        ltr4.Text += hdnCountry.Value.ToString() + "<br />";
                    }
                    if (!string.IsNullOrEmpty(hdnPhone.Value.ToString()))
                    {
                        ltr4.Text += hdnPhone.Value.ToString();
                    }
                    #endregion

                    ltr2.Text = "<a style='color:#b92127 !important;' class=\"order-no\" href=\"/Admin/Orders/Orders.aspx?id=" + ltr2.Text.ToString() + "\">" + ltr2.Text.ToString() + "</a><br />";
                    if (hdnShippingMethod != null && hdnShippingMethod.Value.ToString() != " ")
                        ltrCustomerShipping.Text = "Shipping : " + hdnShippingMethod.Value.ToString() + "($" + string.Format("{0:0.00}", Convert.ToDecimal(hdnOrderShippingCosts.Value.ToString())) + ")";
                    GetProduct(Convert.ToInt32(hdnshoppingcartid.Value), ltr3, hndProductId, txtaddtionalprice);
                    //BindWareHouse(ddlWareHouse);

                    string StrQuery = "SElect distinct ISNULL(TrackingNumber,'') as TrackingNumber,ISNULL(ShippedVia,'') as ShippedVia from tb_OrderShippedItems where OrderNumber in (" + lblOrderNumber.Text.ToString() + ") AND isnull(TrackingNumber,'') <> ''";
                    DataSet dsTackingNo = CommonComponent.GetCommonDataSet(StrQuery);
                    if (dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0)
                    {
                        if ((dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0))
                        {
                            divaddproductdetail.Visible = false;
                            checkCount++;
                            //  hdnchecklblgenerate.Value = "1";
                            btnSearchlabel.Visible = false;
                            bntDownUSPS.Visible = false;
                            btnRebindShippingMethods.Visible = false;
                            btnRemoveBatchOrder.Visible = false;
                            if (dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dsTackingNo.Tables[0].Rows[0]["ShippedVia"]).Trim()))
                            {
                                string StrShipVia = Convert.ToString(dsTackingNo.Tables[0].Rows[0]["ShippedVia"]).Trim();
                                String LabelFPath = "";
                                if (StrShipVia.ToUpper().IndexOf("USPS") > -1)
                                {
                                    LabelFPath = "~/ShippingLabels/USPS/"; //Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                }
                                if (StrShipVia.ToUpper().IndexOf("UPS") > -1)
                                {
                                    LabelFPath = "~/ShippingLabels/UPS/"; // Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                }
                                if (StrShipVia.ToUpper().IndexOf("FEDEX") > -1)
                                {
                                    LabelFPath = "~/ShippingLabels/FEDEX/"; //Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                }
                                string[] strFile = System.IO.Directory.GetFiles(Server.MapPath(LabelFPath), "*_" + lblOrderNumber.Text.ToString() + "_*");
                                lblShippingLabelFileName.Text = "";
                                foreach (string str in strFile)
                                {
                                    FileInfo fltemp = new FileInfo(str);
                                    if (str.IndexOf("_" + lblOrderNumber.Text.ToString() + "_") > -1 && fltemp.Extension.ToString().ToLower() == ".pdf")
                                    {
                                        FileInfo fl = new FileInfo(str);
                                        lblShippingLabelFileName.Text += fl.Name.ToString() + ",";
                                    }

                                }

                                try
                                {
                                    if (lblShippingLabelFileName.Text.ToString().Length > 0)
                                    {
                                        lblShippingLabelFileName.Text = lblShippingLabelFileName.Text.ToString().Substring(0, lblShippingLabelFileName.Text.ToString().Length - 1);
                                        chkSelect.Visible = true;
                                        IsPrintLabelExists = true;
                                        bntDownUSPS.Visible = true;

                                        string strAmazonlabelDetailsQuery = "select  DISTINCT ISNULL(TrackingNumber,'') as AmazonTrackingNumber, isnull(ShippingLabelFileName,'') as ShippingLabelFileName from tb_AmazonlabelDetails  where OrderNumber = " + lblOrderNumber.Text.ToString() + "";
                                        DataSet dsAmazonProduct = CommonComponent.GetCommonDataSet(strAmazonlabelDetailsQuery);
                                        if (dsAmazonProduct != null && dsAmazonProduct.Tables.Count > 0 && dsAmazonProduct.Tables[0].Rows.Count > 0)
                                        {
                                            string strtracking = "";
                                            string LabelFPath1 = "";
                                            for (int z = 0; z < dsAmazonProduct.Tables[0].Rows.Count; z++)
                                            {

                                                if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) && !string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString()))
                                                {
                                                    btnPrintLabel1.Visible = true;
                                                    if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("USPS") > -1)
                                                    {
                                                        LabelFPath1 = "/ShippingLabels/USPS/";// Convert.ToString(AppLogic.AppConfigs("USPS.LabelSavePath"));
                                                    }
                                                    if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("UPS") > -1)
                                                    {
                                                        LabelFPath1 = "/ShippingLabels/UPS/"; //Convert.ToString(AppLogic.AppConfigs("UPS.LabelSavePath"));
                                                    }
                                                    if (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString().ToUpper().IndexOf("FEDEX") > -1)
                                                    {
                                                        LabelFPath1 = "/ShippingLabels/FEDEX/"; //Convert.ToString(AppLogic.AppConfigs("FedEx.LabelSavePath"));
                                                    }
                                                    if (z == dsAmazonProduct.Tables[0].Rows.Count - 1)
                                                    {
                                                        strtracking += "<a href=\"" + LabelFPath1.ToString() + "" + dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString() + "\"   style='color:#000 !important;' target=\"_blank\">" + (dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) + "</a>";
                                                    }
                                                    else
                                                    {
                                                        strtracking += "<a href=\"" + LabelFPath1.ToString() + "" + dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString() + "\"   style='color:#000 !important;' target=\"_blank\">" + (dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) + "</a>,&nbsp;";
                                                    }

                                                }

                                                //if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()))
                                                //    strtracking += "<a href=\"" + dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString() + "\"   style='color:#000 !important;' target=\"_blank\">" + (dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString()) + "</a>&nbsp;<a onclick=\"return DeleteTrackingNo('" + dsAmazonProduct.Tables[0].Rows[z]["ID"].ToString() + "','" + dsAmazonProduct.Tables[0].Rows[z]["AmazonTrackingNumber"].ToString() + "');\" href=\"javascript:void(0);\">X</a>,&nbsp;";

                                                //if (!string.IsNullOrEmpty(dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString()))
                                                //    lblShippingLabelFileName.Text = (dsAmazonProduct.Tables[0].Rows[z]["ShippingLabelFileName"].ToString());
                                            }

                                            lblAmazonTrackingNo.Text = strtracking.ToString();

                                            btnPrintLabel.Visible = true;
                                            btnPrintLabel1.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        bntDownUSPS.Visible = false;
                                    }

                                }
                                catch
                                {
                                }
                            }

                            ddlShippingMethod.Visible = false;
                            if (string.IsNullOrEmpty(hdnShippingLabelMethod.Value.ToString().Trim()))
                            {
                                if (dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0)
                                {
                                    lblShippingMethod.Text = Convert.ToString(dsTackingNo.Tables[0].Rows[0]["ShippedVia"]);
                                }
                            }
                            else
                                lblShippingMethod.Text = hdnShippingLabelMethod.Value.ToString().Trim();
                            lblGenerateLabelMsg.Text = "Label Generated";

                            lblDimensionName.Visible = true;

                            txtProWeight.ReadOnly = true;
                            txtHeightgrid.ReadOnly = true;
                            txtWidthgrid.ReadOnly = true;
                            txtLengthgrid.ReadOnly = true;

                            lblGenerateLabelMsg.Visible = true;

                            txtHeightgrid.Text = Convert.ToString(hdnShippingLabelPackageHeight.Value);
                            txtWidthgrid.Text = Convert.ToString(hdnShippingLabelPackageWidth.Value);
                            txtLengthgrid.Text = Convert.ToString(hdnShippingLabelPackageLength.Value);

                            decimal decWeight = 0;
                            if (!string.IsNullOrEmpty(hdnShippingLabelWeight.Value.ToString()))
                                decWeight = Convert.ToDecimal(hdnShippingLabelWeight.Value.ToString());
                            txtProWeight.Text = Convert.ToString(string.Format("{0:F}", decWeight));

                            if (dsTackingNo != null && dsTackingNo.Tables.Count > 0 && dsTackingNo.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsTackingNo.Tables[0].Rows.Count; i++)
                                {
                                    string TrkNo = Convert.ToString(dsTackingNo.Tables[0].Rows[i]["TrackingNumber"]);
                                    if (!string.IsNullOrEmpty(TrkNo.ToString()))
                                    {
                                        if (i == 0)
                                            ltrTackingNo.Text = "<br /> <b>Tracking# : </b>" + TrkNo.ToString();
                                        else
                                            ltrTackingNo.Text += "<br />" + TrkNo.ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        divaddproductdetail.Visible = true;

                        // btnRemoveBatchOrder.Visible = true;
                        btnRemoveBatchOrder.Visible = false;
                        txtProWeight.Attributes.Add("onchange", "return Clacu(" + e.Row.RowIndex + ",'" + txtProWeight.UniqueID.ToString() + "');");
                        ddlShippingMethod.Visible = true;
                        btnSearchlabel.Visible = false;
                        bntDownUSPS.Visible = false;
                        btnRebindShippingMethods.Visible = true;
                        lblShippingMethod.Text = "";


                        string StrQueryProduct = "SELECT sum(A.Weight) as Weight,  Height, Length, Width  FROM (select  (SELECT isnull(tb_Product.Weight,0) FROM tb_Product INNER JOIN tb_ProductVariantValue on tb_Product.ProductID=tb_ProductVariantValue.ProductID WHERE tb_Product.StoreId=1 and isnull(tb_Product.Deleted,0)=0 and tb_ProductVariantValue.SKU=OSCI.SKU UNION SELECT isnull(tb_Product.Weight,0) FROM tb_Product WHERE tb_Product.StoreId=1 and isnull(tb_Product.Deleted,0)=0 and tb_Product.SKU=OSCI.SKU)  * ISNULL(OSCI.Quantity,1) as Weight,1 AS Height,13 as Length,11 as Width from tb_OrderedShoppingCartItems AS OSCI inner join tb_Order on tb_Order.ShoppingCardID=OSCI.OrderedShoppingCartID where tb_Order.Ordernumber=" + lblOrderNumber.Text.ToString() + ") AS A GROUP BY  Height, Length, Width";
                        DataSet dsProduct = CommonComponent.GetCommonDataSet(StrQueryProduct);
                        decimal decHeight = 0, decLength = 0, decWidth = 0, decWeight = 0;

                        if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Height"].ToString()))
                                decHeight = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Height"].ToString());
                            if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Width"].ToString()))
                                decWidth = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Width"].ToString());
                            if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Length"].ToString()))
                                decLength = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Length"].ToString());

                            if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Weight"].ToString()))
                                decWeight = Convert.ToDecimal(dsProduct.Tables[0].Rows[0]["Weight"].ToString());
                        }

                        txtHeightgrid.Text = Convert.ToString(decHeight);
                        txtWidthgrid.Text = Convert.ToString(decWidth);
                        txtLengthgrid.Text = Convert.ToString(decLength);
                        txtProWeight.Text = Convert.ToString(Math.Round(decWeight, 2));


                        //BindShippingMethod(ddlShippingMethod, OrderNumber, decWeight, decHeight, decWidth, decLength, ddlWareHouse, ddlMailpieceShape.SelectedItem.Text);
                    }

                    if (grvOrderlist.HeaderRow != null)
                    {
                        ImageButton btngeneratelabel = (ImageButton)grvOrderlist.HeaderRow.FindControl("btngeneratealllabel");
                        if (btngeneratelabel != null)
                        {
                            if (checkCount == GridrowCount) { btngeneratelabel.Visible = false; }
                            else { btngeneratelabel.Visible = false; }
                        }
                        ImageButton btnlnkPrintalllabel = (ImageButton)grvOrderlist.HeaderRow.FindControl("btnlnkPrintalllabel");
                        Literal ltrUseBr = (Literal)grvOrderlist.HeaderRow.FindControl("ltrUseBr");
                        if (btngeneratelabel != null)
                        {
                            if (checkCount > 0)
                            {
                                btnlnkPrintalllabel.Visible = false;
                                ltrUseBr.Visible = true;
                                ltrUseBr.Text = "<br>";
                            }
                            else { btnlnkPrintalllabel.Visible = false; }
                        }

                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (IsPrintLabelExists == true)
                    {
                        btnPrintLabel1.Visible = true;
                        if (grvOrderlist.Rows.Count > 0)
                        {
                            for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                            {
                                grvOrderlist.Rows[i].Cells[11].Visible = true;
                            }
                        }
                        grvOrderlist.HeaderRow.Cells[11].Visible = true;
                    }
                    else
                    {
                        btnPrintLabel1.Visible = false;
                        if (grvOrderlist.Rows.Count > 0)
                        {
                            for (int i = 0; i < grvOrderlist.Rows.Count; i++)
                            {
                                grvOrderlist.Rows[i].Cells[11].Visible = false;
                            }
                        }
                        grvOrderlist.HeaderRow.Cells[11].Visible = false;
                    }
                }


            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("Multishipping", ex.Message.ToString(), ex.StackTrace.ToString());
            }

        }


        /// <summary>
        /// Get Product Details From Shipping
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        protected DataSet GetProductDetailsFromShipping(Int32 OrderNumber)
        {
            string StrQuery = " Select ROW_NUMBER() OVER (ORDER BY OSCI.RefProductID) AS PackageId,OSCI.ProductName AS Name,OSCI.SKU,OSCI.Quantity,OSCI.Price AS SalePrice,OSCI.VariantNames,OSCI.VariantValues, " +
                                  " (case when (SElect ISNULL(weight,0) from tb_Product where ProductID=OSCI.RefProductID)=0 then isnull(OSCI.Weight,0) else (SElect ISNULL(weight,0) from tb_Product where ProductID=OSCI.RefProductID) end) as Weight,OSCI.OrderedCustomCartId as ShippingCartID,OSCI.RefProductID AS ProductId, (OSCI.OrderedShoppingCartID) as ShoppingCartID  " +
                                  " from tb_OrderedShoppingCartItems AS OSCI inner join tb_Order on tb_Order.ShoppingCardID=OSCI.OrderedShoppingCartID where tb_Order.Ordernumber=" + OrderNumber + "";
            DataSet DsProduct = new DataSet();

            DsProduct = CommonComponent.GetCommonDataSet(StrQuery.ToString());
            return DsProduct;

        }
        private void BindShippingMethod(DropDownList ddlShippingMethod, Int32 OrderNumber, decimal decWeight, decimal decHeight, decimal decWidth, decimal decLength, DropDownList ddlWareHouse, string mailreceipe)
        {
            ddlShippingMethod.Items.Clear();
            string OrgShippingZip = "";
            string OrgCountry = "";
            string strUSPSMessage = "";
            string strUPSMessage = "";
            decimal decTotalWeight = 0;
            DataTable ShippingTable = new DataTable();//*//
            ShippingTable.Columns.Add("sn", typeof(int));
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));
            ShippingTable.Columns.Add("Shippingmethod", typeof(String));
            ShippingTable.Columns.Add("ShippingServiceId", typeof(String));
            ShippingTable.Columns.Add("ShippingOfferId", typeof(String));
            ShippingTable.Columns.Add("Shippingcarrier", typeof(String));

            ShippingComponent objShipping = new ShippingComponent();
            CountryComponent objCountry = new CountryComponent();

            decimal Weight = decimal.Zero;
            Weight = Convert.ToDecimal(decWeight);

            if (Weight == 0)
            {
                Weight = 1;
            }

            DataTable dtOrder = new DataTable();


            dtOrder.Columns.Add("Qty", typeof(int));
            dtOrder.Columns.Add("AmazonItemId", typeof(String));
            dtOrder.Columns.Add("RefOrderId", typeof(String));
            DataSet dtORderdetails = CommonComponent.GetCommonDataSet("SELECT tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.OrderItemID,tb_Order.RefOrderID FROM tb_Order INNER JOIN tb_OrderedShoppingCartItems on  tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE OrderNumber=" + OrderNumber + "");
            if (dtORderdetails != null && dtORderdetails.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dtORderdetails.Tables[0].Rows.Count; i++)
                {
                    DataRow dataRow = dtOrder.NewRow();
                    dataRow["qty"] = dtORderdetails.Tables[0].Rows[i]["Quantity"].ToString();
                    dataRow["AmazonItemId"] = dtORderdetails.Tables[0].Rows[i]["OrderItemID"].ToString();
                    dataRow["RefOrderId"] = dtORderdetails.Tables[0].Rows[i]["RefOrderID"].ToString();
                    dtOrder.Rows.Add(dataRow);
                }

            }

            decTotalWeight = decWeight;
            string accessKey = ViewState["AmazonAccessKey"].ToString();

            // Developer AWS secret key
            string secretKey = ViewState["AmazonSecretKey"].ToString();

            // The client application name
            string appName = ViewState["AmazonApplicationName"].ToString();
            string sellerId = ViewState["AmazonMerchantID"].ToString();
            string mwsAuthToken = "";
            // The client application version
            string appVersion = "1.01";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = ViewState["AmazonServiceURL"].ToString();
            MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            MWSMerchantFulfillmentServiceClient client = new MWSMerchantFulfillmentServiceClient(accessKey, secretKey, appName, appVersion, config);
            MWSMerchantFulfillmentServiceSample obj = new MWSMerchantFulfillmentServiceSample(client);
            MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse res = new MWSMerchantFulfillmentService.Model.GetEligibleShippingServicesResponse();
            //res = obj.GetAllEligibleShippingServices(sellerId, "");
            res = obj.InvokeGetEligibleShippingServices(sellerId, "", Convert.ToDecimal(decLength.ToString()), Convert.ToDecimal(decWidth.ToString()), Convert.ToDecimal(decHeight.ToString()), decTotalWeight, dtOrder);
            for (int i = 0; i < res.GetEligibleShippingServicesResult.ShippingServiceList.Count(); i++)
            {
                string ShippingServiceId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceId;
                string ShippingServiceName = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceName;
                string ShippingServiceOfferId = res.GetEligibleShippingServicesResult.ShippingServiceList[i].ShippingServiceOfferId;
                string Amount1 = res.GetEligibleShippingServicesResult.ShippingServiceList[i].Rate.Amount.ToString();

                DataRow dataRow = ShippingTable.NewRow();
                dataRow["sn"] = (i + 1).ToString();
                dataRow["Shippingmethod"] = ShippingServiceName;
                dataRow["ShippingMethodName"] = " " + ShippingServiceName + "($" + string.Format("{0:0.00}", Convert.ToDecimal(Amount1)) + ")";
                dataRow["ShippingServiceId"] = ShippingServiceId + "~" + ShippingServiceOfferId;
                dataRow["ShippingOfferId"] = ShippingServiceOfferId;
                dataRow["Price"] = Amount1;
                dataRow["Shippingcarrier"] = res.GetEligibleShippingServicesResult.ShippingServiceList[i].CarrierName.ToString();
                ShippingTable.Rows.Add(dataRow);
            }
            if (ShippingTable.Rows.Count > 0)
            {
                ShippingTable.DefaultView.Sort = "Shippingmethod asc,Price asc";
            }

            if (ShippingTable != null && ShippingTable.Rows.Count > 0)
            {
                System.Web.UI.WebControls.ListItem itemMethod = null;
                string strall = "";
                ddlShippingMethod.DataSource = ShippingTable;
                ddlShippingMethod.DataValueField = "ShippingServiceId";
                ddlShippingMethod.DataTextField = "ShippingMethodName";
                ddlShippingMethod.DataBind();
                Int32 items = 0;
                if (ViewState["AmazonDefaultMethod"] != null && !string.IsNullOrEmpty(ViewState["AmazonDefaultMethod"].ToString()))
                {
                    foreach (DataRow drMethod in ShippingTable.DefaultView.ToTable().Rows)
                    {
                        if (ViewState["AmazonDefaultMethod"].ToString().ToLower().Trim() == drMethod["Shippingmethod"].ToString().ToLower().Trim())
                        {
                            try
                            {
                                ddlShippingMethod.SelectedIndex = items;
                            }
                            catch
                            { }
                        }
                        items++;
                        //itemMethod = new System.Web.UI.WebControls.ListItem(drMethod["ShippingMethodName"].ToString(), drMethod["ShippingServiceId"].ToString());

                    }
                }



            }
            else
            {
                ddlShippingMethod.DataSource = null;
                ddlShippingMethod.DataBind();
            }

        }
    }
}
