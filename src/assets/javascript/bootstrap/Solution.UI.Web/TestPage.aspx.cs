using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Solution.Bussines.Components.Common;
using System.Data;
using Solution.Bussines.Components;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.Net;
using Solution.Data;

using System.Drawing.Imaging;

using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace Solution.UI.Web
{
    public partial class TestPage : System.Web.UI.Page
    {
        #region Private Properties
        private int CurrentPage
        {
            get
            {
                object objPage = ViewState["_CurrentPage"];
                int _CurrentPage = 0;
                if (objPage == null)
                {
                    _CurrentPage = 0;
                }
                else
                {
                    _CurrentPage = (int)objPage;
                }
                return _CurrentPage;
            }
            set { ViewState["_CurrentPage"] = value; }
        }
        private int fistIndex
        {
            get
            {

                int _FirstIndex = 0;
                if (ViewState["_FirstIndex"] == null)
                {
                    _FirstIndex = 0;
                }
                else
                {
                    _FirstIndex = Convert.ToInt32(ViewState["_FirstIndex"]);
                }
                return _FirstIndex;
            }
            set { ViewState["_FirstIndex"] = value; }
        }
        private int lastIndex
        {
            get
            {

                int _LastIndex = 0;
                if (ViewState["_LastIndex"] == null)
                {
                    _LastIndex = 0;
                }
                else
                {
                    _LastIndex = Convert.ToInt32(ViewState["_LastIndex"]);
                }
                return _LastIndex;
            }
            set { ViewState["_LastIndex"] = value; }
        }
        #endregion

        #region PagedDataSource
        PagedDataSource _PageDataSource = new PagedDataSource();
        #endregion

        #region Private Methods
        /// <summary>
        /// Build DataTable to bind Main Items List
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetDataTable()
        {
            DataTable dtItems = new DataTable();

            DataColumn dcName = new DataColumn();
            dcName.ColumnName = "title";
            dcName.DataType = System.Type.GetType("System.String");
            dtItems.Columns.Add(dcName);

            DataRow row;
            for (int i = 1; i <= 100; i++)
            {
                row = dtItems.NewRow();
                row["title"] = "Sample Row:&nbsp;I am putting here sample text for row " + i;
                dtItems.Rows.Add(row);

            }
            return dtItems;

        }

        /// <summary>
        /// Binding Main Items List
        /// </summary>
        private void BindItemsList()
        {

            DataTable dataTable = this.GetDataTable();
            _PageDataSource.DataSource = dataTable.DefaultView;
            _PageDataSource.AllowPaging = true;
            _PageDataSource.PageSize = 10;
            _PageDataSource.CurrentPageIndex = CurrentPage;
            ViewState["TotalPages"] = _PageDataSource.PageCount;

            this.lblPageInfo.Text = "Page " + (CurrentPage + 1) + " of " + _PageDataSource.PageCount;
            this.lbtnPrevious.Enabled = !_PageDataSource.IsFirstPage;
            this.lbtnNext.Enabled = !_PageDataSource.IsLastPage;
            this.lbtnFirst.Enabled = !_PageDataSource.IsFirstPage;
            this.lbtnLast.Enabled = !_PageDataSource.IsLastPage;

            this.dListItems.DataSource = _PageDataSource;
            this.dListItems.DataBind();
            this.doPaging();
        }

        /// <summary>
        /// Binding Paging List
        /// </summary>
        private void doPaging()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");

            fistIndex = CurrentPage - 5;

            if (CurrentPage > 5)
            {
                lastIndex = CurrentPage + 5;
            }
            else
            {
                lastIndex = 5;
            }
            if (lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                fistIndex = lastIndex - 5;
            }

            if (fistIndex < 0)
            {
                fistIndex = 0;
            }

            for (int i = fistIndex; i < lastIndex; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            this.dlPaging.DataSource = dt;
            this.dlPaging.DataBind();
        }
        #endregion
        //protected override void OnInit(EventArgs e)
        //{

        //    base.OnInit(e);

        //    Timer1.Tick += new EventHandler<EventArgs>(Timer1_Tick);


        //}
        //void Timer1_Tick(object sender, EventArgs e)
        //{


        //    //string str = "5/17/2012 12:00:00 AM";
        //    //DateTime dt2 = Convert.ToDateTime(str);
        //    DateTime dt1 = DateTime.Now;
        //    TimeSpan ts = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString());// dt1;//dt2.Subtract(dt1);
        //    Int32 hh = 24 - ts.Hours;
        //    Int32 mini = ts.Minutes;
        //    Int32 Second = ts.Seconds;
        //    if (mini > 0)
        //    {
        //        hh = hh - 1;
        //        mini = 60 - mini;
        //        Second = 60 - Second;
        //    }
        //    lblTime.Text = hh.ToString() + " " + mini.ToString() + " " + Second.ToString();

        //    UpdatePanel1.Update();

        //}
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                List<String> list = new List<string>();
                String a = "a";
                list.Add(a);
                String b = "b";
                list.Add(b);
                if(list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Response.Write(list[i].ToString());
                    }
                }


                //byte[] binaryDataResult = null;
                //DataSet dsinventory = new DataSet();
                //dsinventory = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Admin");
                //using (MemoryStream memStream = new MemoryStream())
                //{
                //    BinaryFormatter brFormatter = new BinaryFormatter();
                //    dsinventory.RemotingFormat = SerializationFormat.Binary;
                //    brFormatter.Serialize(memStream, dsinventory);
                //    binaryDataResult = memStream.ToArray();
                //}


                //DataSet DsInventory = new DataSet();
                //BinaryFormatter bformatter = new BinaryFormatter();
                //MemoryStream stream = new MemoryStream();
                //stream = new MemoryStream(binaryDataResult);
                //DsInventory = (DataSet)bformatter.Deserialize(stream);
                //stream.Close();

                //System.Drawing.Image imm = System.Drawing.Image.FromFile(Server.MapPath("/karanUPS-Package3_1ZX0360V0391046227_1672_706@2013482006-1.gif"));
                //System.Drawing.Bitmap bmp1 = new System.Drawing.Bitmap(imm, imm.Width, imm.Height);
                //bmp1.Save(Server.MapPath("/karanUPS-Package3_1ZX0360V0391046227_1672_706@2013482006-1-new.gif"), System.Drawing.Imaging.ImageFormat.Gif);
                //PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                //doc.Pages.Add(new PdfSharp.Pdf.PdfPage());
                //XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                //XImage img = XImage.FromFile(Server.MapPath("/karanUPS-Package3_1ZX0360V0391046227_1672_706@2013482006-1.gif"));

                //xgr.DrawImage(img, 0, 0);
                //doc.Save(Server.MapPath("/karanUPS-Package3_1ZX0360V0391046227_1672_706@2013482006-1.pdf"));
                //doc.Close();

                // CreatePDFFile(Server.MapPath("/karanUPS-Package3_1ZX0360V0391046227.gif"));
                //Response.Write(Server.UrlEncode(SecurityComponent.Encrypt("2431").ToString()));
                //Response.Write(Server.UrlEncode(SecurityComponent.Encrypt("3115").ToString()));
                //Response.Write(Convert.ToDecimal("123466665.00").ToString("C"));
                //CreatePDFFile(1090);

                //this.BindItemsList();
                //BindCategory();
                //string strName = "Monitor : 17 Inch Flat LCD (+179.99)";
                //if (strName.ToString().IndexOf("($") > -1)
                //{
                //    strName = strName.Substring(strName.ToString().IndexOf("($") + 2, strName.ToString().Length - strName.ToString().IndexOf("($") - 2);
                //}
                //else if (strName.ToString().IndexOf("(+") > -1)
                //{
                //    strName = strName.Substring(strName.ToString().IndexOf("(+") + 2, strName.ToString().Length - strName.ToString().IndexOf("(+") - 2);
                //}

                //Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
                //DataSet dsproduct = new DataSet();
                //dsproduct = objSql.GetDs("SELECT PRODUCTID,SKU FROM tb_Product WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND Storeid=1");
                //if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dsproduct.Tables[0].Rows.Count; i++)
                //    {
                //        string strBarcode = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 UPC FROM tb_UPCMASTER WHERE isnull(SKU,'')=''"));
                //        objSql.ExecuteNonQuery("UPDATE tb_Product SET UPC='" + strBarcode + "' WHERE SKU='" + dsproduct.Tables[0].Rows[i]["SKU"].ToString() + "'");
                //        objSql.ExecuteNonQuery("UPDATE tb_UPCMASTER SET SKU='" + dsproduct.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "' WHERE UPC = '" + strBarcode.ToString().Trim() + "'");
                //    }
                //}

                //ProductCount(13);

                //string str = "96W X 120L Pole Pocket (Made to Order Size)(+$89)";
                //Response.Write(str.Substring(str.IndexOf("(+$") + 3, str.LastIndexOf(")") - str.IndexOf("(+$") - 3));

                //StringBuilder frameHTML = new StringBuilder();
                ////set the source of the iframe to the path of the PDF
                //frameHTML.Append("<iframe src=" + Convert.ToString(Server.MapPath("/UPS-Packge1_1Z58F3E30396172082_126863_17793@201122614730-1.pdf")) + " ");
                ////set height and width of the frame
                //frameHTML.Append("width=200px height=200px");
                //frameHTML.Append("</iframe>");
                //divIframe.InnerHtml = frameHTML.ToString();
                // GetLNTOrder();
                //GetorderOverStock();
            }
        }

        //protected void BindCategory()
        //{
        //    string StrCategory = "";
        //    DataSet dsCategory = new DataSet();
        //    dsCategory = CategoryComponent.GetCategoryByStoreID(1, 3);
        //    StrCategory = LoadCategoryTree("1", dsCategory);
        //   // ltrCategory.Text = StrCategory.ToString();
        //}

        private void CreatePDFFile(string filename)
        {
            Document document = new Document();
            PdfWriter writer = null;
            try
            {

                FileInfo fl = new FileInfo(filename);
                string strname = fl.FullName.Replace(fl.Name.ToString(), "");
                writer = PdfWriter.GetInstance(document, new FileStream(strname + fl.Name.ToString().Replace(fl.Extension.ToString(), "") + "22.pdf".ToString(), FileMode.Create));

                document.Open();
                iTextSharp.text.Table table = new iTextSharp.text.Table(5);
                iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                float[] headerwidths = { 200, 80, 50 };
                aTable.Widths = headerwidths;
                aTable.WidthPercentage = 100;

                iTextSharp.text.Table aTable2 = new iTextSharp.text.Table(1);// 2 rows, 2 columns
                aTable2.Cellpadding = 3;
                aTable2.Cellspacing = 3;
                aTable2.BorderWidth = 0;
                aTable2.WidthPercentage = 100;
                iTextSharp.text.Cell cell = new iTextSharp.text.Cell();

                iTextSharp.text.Image img;

                try
                {

                    System.Drawing.Image im = System.Drawing.Image.FromFile(filename);
                    img = iTextSharp.text.Image.GetInstance(im, ImageFormat.Gif);

                }
                catch (Exception ex)
                {
                    img = iTextSharp.text.Image.GetInstance(filename);
                }


                iTextSharp.text.Image img4 = img;
                img4.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                cell.Add(img4);
                cell.BorderWidth = 0;
                aTable2.AddCell(cell);
                document.Add(aTable2);
                document.Close();
                writer.CloseStream = true;
                writer.Close();

                File.Move(filename, Server.MapPath("/config/" + fl.Name.ToString()));

            }
            catch
            {
            }
        }
        private void GetProductVariant()
        {

            DataSet dsvaraintid = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();

            dsvaraintid = objSql.GetDs("SELECT * FROM variantdata");
            if (dsvaraintid != null && dsvaraintid.Tables.Count > 0 && dsvaraintid.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsvaraintid.Tables[0].Rows.Count; i++)
                {
                    string[] headerdesign = Regex.Split(dsvaraintid.Tables[0].Rows[i]["Top header design"].ToString(), "\" \"", RegexOptions.IgnoreCase);
                    string[] sizedesign = Regex.Split(dsvaraintid.Tables[0].Rows[i]["Size"].ToString(), "\" \"", RegexOptions.IgnoreCase);
                    bool ismaster = false;
                    Int32 productId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productid FROM tb_Product WHERE SKU='" + dsvaraintid.Tables[0].Rows[i]["code"].ToString().Replace("'", "''") + "' AND Storeid=2"));

                    Int32 VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantID FROM tb_ProductVariant WHERE VariantName='" + headerdesign[0].ToString().Replace("\"", "").Trim() + "'  AND productid=" + productId + ""));

                    if (VariantID == 0)
                    {

                        VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO  tb_ProductVariant(VariantName,ProductID,IsParent,ParentId) VALUES ('" + headerdesign[0].ToString().Replace("\"", "").Trim() + "'," + productId.ToString() + ",1,0) SELECT SCOPE_IDENTITY();"));
                    }

                    Int32 VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantValue='" + headerdesign[1].ToString().Replace("\"", "").Trim() + "'  AND productid=" + productId + " AND VariantID=" + VariantID + ""));
                    if (VariantValueID == 0)
                    {
                        string PP = "0";
                        if (headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("($") > -1)
                        {
                            PP = headerdesign[1].ToString().Replace("\"", "").Trim().Substring(headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("($") + 2, headerdesign[1].ToString().Replace("\"", "").Trim().LastIndexOf(")") - headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("($") - 2);

                        }
                        else if (headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("(+$") > -1)
                        {
                            PP = headerdesign[1].ToString().Replace("\"", "").Trim().Substring(headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("(+$") + 3, headerdesign[1].ToString().Replace("\"", "").Trim().LastIndexOf(")") - headerdesign[1].ToString().Replace("\"", "").Trim().IndexOf("(+$") - 3);

                        }

                        VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue(VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID,SKU,UPC,Header,Inventory) VALUES (" + VariantID + ",'" + headerdesign[1].ToString().Replace("\"", "").Trim() + "'," + PP + ",0," + productId.ToString() + ",'','','',0) SELECT SCOPE_IDENTITY();"));
                    }





                    //SubVariant
                    VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantID FROM tb_ProductVariant WHERE VariantName='" + sizedesign[0].ToString().Replace("\"", "").Trim() + "' AND productid=" + productId + " AND isnull(parentid,0)=" + VariantValueID + ""));
                    if (VariantID == 0)
                    {
                        VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO  tb_ProductVariant(VariantName,ProductID,IsParent,ParentId) VALUES ('" + sizedesign[0].ToString().Replace("\"", "").Trim() + "'," + productId.ToString() + ",0," + VariantValueID + ") SELECT SCOPE_IDENTITY();"));
                    }
                    for (int k = 1; k < sizedesign.Length; k++)
                    {
                        VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantValue='" + sizedesign[k].ToString().Replace("\"", "").Trim() + "' AND productid=" + productId + " AND VariantID=" + VariantID + ""));
                        if (VariantValueID == 0)
                        {
                            string PP = "0";

                            if (sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") > -1)
                            {
                                PP = sizedesign[k].ToString().Replace("\"", "").Trim().Substring(sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") + 2, sizedesign[k].ToString().Replace("\"", "").Trim().LastIndexOf(")") - sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") - 2);

                            }
                            else if (sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") > -1)
                            {
                                PP = sizedesign[k].ToString().Replace("\"", "").Trim().Substring(sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") + 3, sizedesign[k].ToString().Replace("\"", "").Trim().LastIndexOf(")") - sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") - 3);

                            }
                            VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue(VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID,SKU,UPC,Header,Inventory) VALUES (" + VariantID + ",'" + sizedesign[k].ToString().Replace("\"", "").Trim() + "'," + PP + ",0," + productId.ToString() + ",'','','',0) SELECT SCOPE_IDENTITY();"));
                        }

                    }

                }
            }


        }
        private void GetProductVariantNew()
        {

            DataSet dsvaraintid = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();

            dsvaraintid = objSql.GetDs("SELECT * FROM variantdata_New");
            if (dsvaraintid != null && dsvaraintid.Tables.Count > 0 && dsvaraintid.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsvaraintid.Tables[0].Rows.Count; i++)
                {

                    string[] sizedesign = Regex.Split(dsvaraintid.Tables[0].Rows[i]["options"].ToString(), "\" \"", RegexOptions.IgnoreCase);
                    bool ismaster = false;
                    Int32 productId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productid FROM tb_Product WHERE SKU='" + dsvaraintid.Tables[0].Rows[i]["code"].ToString().Replace("'", "''") + "' AND Storeid=2"));

                    Int32 VariantID = 0;// Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantID FROM tb_ProductVariant WHERE VariantName='" + headerdesign[0].ToString().Replace("\"", "").Trim() + "'  AND productid=" + productId + ""));

                    Int32 VariantValueID = 0;




                    if (productId > 0)
                    {
                        //SubVariant
                        VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantID FROM tb_ProductVariant WHERE VariantName='" + sizedesign[0].ToString().Replace("\"", "").Trim().Replace("'", "''") + "' AND productid=" + productId + ""));
                        if (VariantID == 0)
                        {
                            VariantID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO  tb_ProductVariant(VariantName,ProductID,IsParent,ParentId) VALUES ('" + sizedesign[0].ToString().Replace("\"", "").Trim().Replace("'", "''") + "'," + productId.ToString() + ",0,0) SELECT SCOPE_IDENTITY();"));
                        }
                        for (int k = 1; k < sizedesign.Length; k++)
                        {
                            VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantValue='" + sizedesign[k].ToString().Replace("\"", "").Trim().Replace("'", "''") + "' AND productid=" + productId + " AND VariantID=" + VariantID + ""));
                            if (VariantValueID == 0)
                            {
                                string PP = "0";

                                if (sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") > -1)
                                {
                                    PP = sizedesign[k].ToString().Replace("\"", "").Trim().Substring(sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") + 2, sizedesign[k].ToString().Replace("\"", "").Trim().LastIndexOf(")") - sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("($") - 2);

                                }
                                else if (sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") > -1)
                                {
                                    PP = sizedesign[k].ToString().Replace("\"", "").Trim().Substring(sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") + 3, sizedesign[k].ToString().Replace("\"", "").Trim().LastIndexOf(")") - sizedesign[k].ToString().Replace("\"", "").Trim().IndexOf("(+$") - 3);

                                }
                                VariantValueID = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue(VariantID,VariantValue,VariantPrice,DisplayOrder,ProductID,SKU,UPC,Header,Inventory) VALUES (" + VariantID + ",'" + sizedesign[k].ToString().Replace("\"", "").Trim() + "'," + PP + ",0," + productId.ToString() + ",'','','',0) SELECT SCOPE_IDENTITY();"));
                            }

                        }
                    }

                }
            }


        }

        public Int32 ProductCount(Int32 CategoryId)
        {

            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("EXEC usp_CategoryProductCount " + CategoryId + ", 0");
            Int32 PCount = 0;
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                DataSet dsParent = new DataSet();

                dsParent = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsParent != null && dsParent.Tables.Count > 0 && dsParent.Tables[0].Rows.Count > 0)
                {
                    PCount = Convert.ToInt32(dsParent.Tables[0].Rows[0]["tt"].ToString());
                }
                Int32 catid = 0;
                Int32 parentcategid = 0;
                for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                {
                    if (catid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()) || parentcategid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString()))
                    {

                        PCount = LoadCategoryChild(Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()), PCount);

                    }
                    catid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString());
                    parentcategid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString());
                }
            }
            else
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    PCount = Convert.ToInt32(dsCategory.Tables[0].Rows[0]["tt"].ToString());
                }
            }
            return PCount;

        }
        public Int32 LoadCategoryChild(Int32 CategoryId, Int32 count)
        {

            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("EXEC usp_CategoryProductCount " + CategoryId + ", 0");

            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                DataSet dsParent = new DataSet();
                dsParent = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsParent != null && dsParent.Tables.Count > 0 && dsParent.Tables[0].Rows.Count > 0)
                {
                    count += Convert.ToInt32(dsParent.Tables[0].Rows[0]["tt"].ToString());
                }
                Int32 catid = 0;
                Int32 parentcategid = 0;
                for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                {
                    if (catid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()) || parentcategid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString()))
                    {


                        count = LoadCategoryChild(Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()), count);

                    }
                    catid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString());
                    parentcategid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString());
                }

            }
            else
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    count += Convert.ToInt32(dsCategory.Tables[0].Rows[0]["tt"].ToString());
                }

            }
            return count;
        }
        public Int32 LoadChildNode(int id, DataSet dsCategory)
        {
            int Cnt = 0;
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0 && dsCategory.Tables[0].Select("ParentCategoryID=" + id).Length > 0)
            {
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=" + id))
                {
                    Cnt++;
                    string ChildCatName = dr["Name"].ToString();
                    LoadChildNode(Convert.ToInt32(dr["CategoryID"].ToString()), dsCategory);
                }
            }
            return Cnt;
        }
        protected void lbtnNext_Click(object sender, EventArgs e)
        {

            CurrentPage += 1;
            this.BindItemsList();

        }
        protected void lbtnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            this.BindItemsList();

        }
        protected void dlPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("Paging"))
            {
                CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
                this.BindItemsList();
            }
        }
        protected void dlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
            if (lnkbtnPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Style.Add("fone-size", "14px");
                lnkbtnPage.Font.Bold = true;

            }
        }
        protected void lbtnLast_Click(object sender, EventArgs e)
        {

            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            this.BindItemsList();

        }
        protected void lbtnFirst_Click(object sender, EventArgs e)
        {

            CurrentPage = 0;
            this.BindItemsList();
        }
        private void CreatePDFFile(Int32 OrderNumber)
        {

            Document document = new Document();
            PdfWriter writer = null;
            try
            {
                DataSet dsOrder = new DataSet();
                dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(OrderNumber);
                if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                {
                    writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(AppLogic.AppConfigs("ImagePathInvoice").ToString() + "Invoice_" + OrderNumber.ToString() + ".pdf").ToString(), FileMode.Create));

                    document.Open();
                    iTextSharp.text.Table table = new iTextSharp.text.Table(5);
                    iTextSharp.text.Table aTable = new iTextSharp.text.Table(3);
                    float[] headerwidths = { 200, 80, 50 };
                    aTable.Widths = headerwidths;
                    aTable.WidthPercentage = 100;

                    iTextSharp.text.Table aTable2 = new iTextSharp.text.Table(1);// 2 rows, 2 columns
                    aTable2.Cellpadding = 3;
                    aTable2.Cellspacing = 3;
                    aTable2.BorderWidth = 0;
                    aTable2.WidthPercentage = 100;
                    iTextSharp.text.Cell cell = new iTextSharp.text.Cell();

                    iTextSharp.text.Image img;

                    img = iTextSharp.text.Image.GetInstance(AppLogic.AppConfigs("Live_Server") + "/images/logo.png");

                    iTextSharp.text.Image img4 = img;
                    img4.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;

                    cell.Add(img4);
                    cell.BorderWidth = 0;
                    aTable2.AddCell(cell);
                    document.Add(aTable2);

                    iTextSharp.text.Table aTable1 = new iTextSharp.text.Table(1, 2);
                    aTable1.WidthPercentage = 100;
                    aTable1.BorderWidth = 0;
                    aTable1.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                    iTextSharp.text.Cell cellContent;
                    Chunk chcontent;
                    // 
                    chcontent = new Chunk(Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString() + " ,"), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    Paragraph objp;

                    objp = new Paragraph("Dear ");
                    objp.Add(chcontent);
                    cellContent = new iTextSharp.text.Cell();
                    cellContent.BorderColor = new Color(0, 0, 255);
                    cellContent.Add(objp);
                    cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellContent.BorderWidth = 0;
                    aTable1.AddCell(cellContent);



                    cellContent = new iTextSharp.text.Cell("Your order has been received. Thank you for shopping with Security Camera Dealer. \r\nWe appreciate your business and are committed to deliver excellent customer care. \r\n \r\n");
                    cellContent.BorderColor = new Color(0, 0, 255);
                    cellContent.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellContent.BorderWidth = 0;
                    aTable1.AddCell(cellContent);
                    document.Add(aTable1);

                    iTextSharp.text.Table aTableOrderNumber = new iTextSharp.text.Table(1, 5);
                    aTableOrderNumber.WidthPercentage = 100;
                    aTableOrderNumber.BorderWidth = 0;
                    aTableOrderNumber.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                    iTextSharp.text.Cell cellorder;
                    chcontent = new Chunk(OrderNumber.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    objp = new Paragraph("Order Number: ");
                    objp.Add(chcontent);
                    cellorder = new iTextSharp.text.Cell(objp);
                    cellorder.BorderColor = new Color(0, 0, 255);
                    cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellorder.BorderWidth = 0;
                    aTableOrderNumber.AddCell(cellorder);

                    objp = new Paragraph("Order Date: ");
                    chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    objp.Add(chcontent);
                    cellorder = new iTextSharp.text.Cell(objp);
                    cellorder.BorderColor = new Color(0, 0, 255);
                    cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellorder.BorderWidth = 0;
                    aTableOrderNumber.AddCell(cellorder);


                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString()))
                    {
                        objp = new Paragraph("Shipping Method: ");
                        chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        objp.Add(chcontent);
                        cellorder = new iTextSharp.text.Cell(objp);
                        cellorder.BorderColor = new Color(0, 0, 255);
                        cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellorder.BorderWidth = 0;
                        aTableOrderNumber.AddCell(cellorder);
                    }

                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()))
                    {
                        objp = new Paragraph("Payment Method: ");
                        chcontent = new Chunk(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        objp.Add(chcontent);
                        cellorder = new iTextSharp.text.Cell(objp);
                        cellorder.BorderColor = new Color(0, 0, 255);
                        cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellorder.BorderWidth = 0;
                        aTableOrderNumber.AddCell(cellorder);
                    }

                    string creditcardNumber = "";
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()) && dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "creditcard")
                    {
                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()))
                        {
                            string CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                            if (CardNumber.Length > 4)
                            {
                                for (int i = 0; i < CardNumber.Length - 4; i++)
                                {
                                    creditcardNumber += "*";
                                }
                                creditcardNumber += CardNumber.ToString().Substring(CardNumber.Length - 4);
                            }
                            else
                            {
                                creditcardNumber = "";
                            }

                        }

                    }
                    else
                    {
                        creditcardNumber = "";
                    }
                    if (creditcardNumber != "")
                    {
                        objp = new Paragraph("Card Number: ");
                        chcontent = new Chunk(creditcardNumber.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                        objp.Add(chcontent);
                        cellorder = new iTextSharp.text.Cell(objp);
                        cellorder.BorderColor = new Color(0, 0, 255);
                        cellorder.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellorder.BorderWidth = 0;
                        aTableOrderNumber.AddCell(cellorder);
                    }
                    document.Add(aTableOrderNumber);
                    objp = new Paragraph("\n");
                    document.Add(objp);

                    iTextSharp.text.Table aTableAddress = new iTextSharp.text.Table(3, 11);
                    aTableAddress.WidthPercentage = 100;
                    aTableAddress.BorderWidth = 0;
                    aTableAddress.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                    iTextSharp.text.Cell cellAddress;
                    chcontent = new Chunk("Account", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    cellAddress = new iTextSharp.text.Cell(chcontent);
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 100F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);
                    chcontent = new Chunk("Billing Address", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    cellAddress = new iTextSharp.text.Cell(chcontent);

                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    chcontent = new Chunk("Shipping Address", FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    cellAddress = new iTextSharp.text.Cell(chcontent);
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);
                    objp = new Paragraph(" ");
                    document.Add(objp);

                    cellAddress = new iTextSharp.text.Cell("Name:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString()));
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString()));
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);




                    cellAddress = new iTextSharp.text.Cell("Company:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);


                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);



                    cellAddress = new iTextSharp.text.Cell("Address1:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);


                    cellAddress = new iTextSharp.text.Cell("Address2:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);



                    cellAddress = new iTextSharp.text.Cell("Suite:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);



                    cellAddress = new iTextSharp.text.Cell("City:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);



                    cellAddress = new iTextSharp.text.Cell("State:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingState"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingState"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);


                    cellAddress = new iTextSharp.text.Cell("Zip:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);



                    cellAddress = new iTextSharp.text.Cell("Country:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);
                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }

                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);


                    cellAddress = new iTextSharp.text.Cell("Phone:");
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.Width = 100F;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        cellAddress = new iTextSharp.text.Cell(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()));
                    }
                    else
                    {
                        cellAddress = new iTextSharp.text.Cell("-");
                    }
                    cellAddress.BorderColor = new Color(0, 0, 255);
                    cellAddress.Width = 200F;
                    cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellAddress.BorderWidth = 0;
                    aTableAddress.AddCell(cellAddress);
                    document.Add(aTableAddress);

                    //Bind Cart


                    iTextSharp.text.Table aTableCart = new iTextSharp.text.Table(5);
                    aTableCart.WidthPercentage = 100F;
                    float[] fl = { 300F, 85F, 60F, 65F, 75F };
                    aTableCart.Widths = fl;
                    iTextSharp.text.Cell cellcart;
                    cellcart = new iTextSharp.text.Cell("Product");
                    cellcart.Width = 200F;
                    cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellcart.BorderColor = new Color(164, 164, 164);
                    cellcart.BackgroundColor = new Color(218, 218, 218);
                    aTableCart.AddCell(cellcart);

                    cellcart = new iTextSharp.text.Cell("SKU");
                    cellcart.BorderColor = new Color(164, 164, 164);
                    cellcart.Width = 100F;
                    cellcart.BackgroundColor = new Color(218, 218, 218);
                    cellcart.VerticalAlignment = Element.ALIGN_TOP;
                    aTableCart.AddCell(cellcart);

                    cellcart = new iTextSharp.text.Cell("Price");
                    cellcart.BorderColor = new Color(164, 164, 164);
                    cellcart.Width = 80F;
                    cellcart.BackgroundColor = new Color(218, 218, 218);
                    cellcart.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellcart);

                    cellcart = new iTextSharp.text.Cell("Quantity");
                    cellcart.BorderColor = new Color(164, 164, 164);
                    cellcart.BackgroundColor = new Color(218, 218, 218);
                    cellcart.Width = 120F;
                    cellcart.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellcart);

                    cellcart = new iTextSharp.text.Cell("Sub Total");
                    cellcart.BorderColor = new Color(164, 164, 164);
                    cellcart.Width = 120F;
                    cellcart.BackgroundColor = new Color(218, 218, 218);
                    cellcart.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellcart.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellcart);

                    aTableCart.DefaultVerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.Padding = 3;
                    aTableCart.DefaultCellGrayFill = 3;
                    aTableCart.BorderWidth = 1;
                    aTableCart.BorderColor = new Color(218, 218, 218);
                    OrderComponent objOrderCart = new OrderComponent();
                    DataSet dsCart = new DataSet();
                    dsCart = objOrderCart.GetProductList(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()));
                    if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                        {
                            iTextSharp.text.Cell cellValue;
                            aTable.DefaultCell.GrayFill = 1.0f;
                            string Pname = "";
                            Pname += dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "\n";

                            string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            for (int j = 0; j < variantValue.Length; j++)
                            {
                                Pname += variantName[j].ToString() + " : " + variantValue[j].ToString() + "\n";
                            }
                            cellValue = new iTextSharp.text.Cell(Pname.ToString());
                            cellValue.BorderColor = new Color(164, 164, 164);
                            cellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                            aTableCart.AddCell(cellValue);

                            cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["SKU"].ToString());
                            cellValue.BorderColor = new Color(164, 164, 164);
                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellValue);

                            cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C"));
                            cellValue.BorderColor = new Color(164, 164, 164);
                            cellValue.VerticalAlignment = Element.ALIGN_RIGHT;
                            cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellValue);

                            cellValue = new iTextSharp.text.Cell(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                            cellValue.BorderColor = new Color(164, 164, 164);
                            cellValue.VerticalAlignment = Element.ALIGN_CENTER;
                            cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellValue);

                            cellValue = new iTextSharp.text.Cell(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["IndiTotal"].ToString()).ToString("C"));
                            cellValue.BorderColor = new Color(164, 164, 164);
                            cellValue.VerticalAlignment = Element.ALIGN_RIGHT;
                            cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                            aTableCart.AddCell(cellValue);

                        }
                    }

                    iTextSharp.text.Cell cellCartFooter;
                    aTable.DefaultCell.GrayFill = 1.0f;

                    cellCartFooter = new iTextSharp.text.Cell("Sub Total:");
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.Colspan = 4;
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderSubtotal"].ToString()).ToString("C"));
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell("Shipping:");
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.Colspan = 4;
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("C"));
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell("Discount:");
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.Colspan = 4;
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()).ToString("C"));
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell("Order Tax:");
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.Colspan = 4;
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()).ToString("C"));
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell("Total:");
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.Colspan = 4;
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);

                    cellCartFooter = new iTextSharp.text.Cell(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()).ToString("C"));
                    cellCartFooter.BorderColor = new Color(164, 164, 164);
                    cellCartFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellCartFooter.VerticalAlignment = Element.ALIGN_MIDDLE;
                    aTableCart.AddCell(cellCartFooter);
                    document.Add(aTableCart);


                    iTextSharp.text.Table aTableFooter = new iTextSharp.text.Table(1, 3);
                    aTableFooter.WidthPercentage = 100;
                    aTableFooter.BorderWidth = 0;
                    aTableFooter.DefaultHorizontalAlignment = Element.ALIGN_LEFT;

                    iTextSharp.text.Cell cellRegards;
                    cellRegards = new iTextSharp.text.Cell("Thank You,");
                    cellRegards.BorderColor = new Color(0, 0, 255);
                    cellRegards.Width = 100F;
                    cellRegards.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellRegards.BorderWidth = 0;
                    aTableFooter.AddCell(cellRegards);

                    chcontent = new Chunk(AppLogic.AppConfigs("storeName").ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, new Color(35, 31, 32)));
                    cellRegards = new iTextSharp.text.Cell(chcontent);
                    cellRegards.BorderColor = new Color(0, 0, 255);
                    cellRegards.Width = 100F;
                    cellRegards.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellRegards.BorderWidth = 0;
                    aTableFooter.AddCell(cellRegards);



                    Anchor anchorLink = new Anchor(AppLogic.AppConfigs("Live_Server").ToString());
                    anchorLink.Reference = AppLogic.AppConfigs("Live_Server").ToString();
                    anchorLink.Name = "top";
                    cellRegards = new iTextSharp.text.Cell(anchorLink);
                    cellRegards.BorderColor = new Color(0, 0, 255);
                    cellRegards.Width = 100F;
                    cellRegards.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellRegards.BorderWidth = 0;
                    aTableFooter.AddCell(cellRegards);

                    document.Add(aTableFooter);
                    document.Close();
                    writer.CloseStream = true;
                    writer.Close();
                }
            }
            catch
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //GetProductVariant();
            //GetorderOverStockGetrec();
            //GetProductVariantNew();
            //GetorderOverStockTime();


            // GetConfirmShipmentOverStock();

            //DataSet dsReaXml = new DataSet();
            //dsReaXml.ReadXml(Server.MapPath("/OverstockOrder/635024354776587252.xml"));
            //DataRow[] rd = dsReaXml.Tables[0].Select("convert(char(10),[Date],101)  >= cast('" + string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.AddDays(-11))) + "' as datetime)");
            //SQLAccess dbAccess = new SQLAccess();
            // DataSet dsStore = new DataSet();
            // dsStore = dbAccess.GetDs("Select StoreID,StoreName from tb_Store  where StoreName like '%overstock%' AND Deleted=0");
            //if (dsStore.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dsStore.Tables[0].Rows.Count; i++)
            //    {
            //        string MerchantKey = Convert.ToString(dbAccess.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='MerchantKey' AND Storeid=" + dsStore.Tables[0].Rows[i]["StoreID"].ToString() + ""));
            //        string AuthenticationKey = Convert.ToString(dbAccess.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthenticationKey' AND Storeid=" + dsStore.Tables[0].Rows[i]["StoreID"].ToString() + ""));
            //        String AuthServer = Convert.ToString(dbAccess.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + dsStore.Tables[0].Rows[i]["StoreID"].ToString() + ""));
            //        ImportoverStockOrderData(dsReaXml, Convert.ToInt32(dsStore.Tables[0].Rows[i]["StoreID"].ToString()));
            //    }
            //}
            //DataSet dsReaXml = new DataSet();
            //dsReaXml.ReadXml(Server.MapPath("/OverstockOrder/635024354776587252.xml"));

            //if (dsReaXml.Tables["InvoiceLine"] != null && dsReaXml.Tables["InvoiceLine"].Rows.Count > 0)
            //{
            //    SQLAccess objSql = new SQLAccess();
            //  //  using (SqlConnection connection = new
            //  //SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString()))
            //  //  {
            //  //      SqlDataAdapter adapter = new SqlDataAdapter();
            //  //      adapter.UpdateCommand = new SqlCommand("UPDATE tb_Product SET OptionSku=@OptionSku WHERE Storeid=4 AND SKU=@VendorSku;",
            //  //          connection);
            //  //      adapter.UpdateCommand.Parameters.Add("@OptionSku", SqlDbType.VarChar, 500, "OptionSku");
            //  //      adapter.UpdateCommand.Parameters.Add("@VendorSku", SqlDbType.VarChar, 500, "VendorSku");

            //  //      adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
            //  //      adapter.UpdateBatchSize = 2;
            //  //      adapter.Update(dsReaXml, "InvoiceLine");
            //  //  }
            //    for (int i = 0; i < dsReaXml.Tables["InvoiceLine"].Rows.Count; i++)
            //    {
            //        objSql.ExecuteNonQuery("UPDATE tb_Product SET OptionSku='" + dsReaXml.Tables["InvoiceLine"].Rows[i]["OptionSku"].ToString().Replace("'", "''") + "' WHERE Storeid=4 AND SKU='" + dsReaXml.Tables["InvoiceLine"].Rows[i]["VendorSku"].ToString().Replace("'", "''") + "'");
            //    }
            //}
            //GetorderOverStockGetCategories();
            //ImportSearsOrder();
            GetorderOverStock();
            //GetorderOverStockGetrec();
            //GetorderOverStock();

        }
        public void ImportoverStockOrderData(DataSet ds, Int32 StoreID)
        {


            #region Variables
            String iAmazonOrderNumber = "0";
            string strItemID = string.Empty;
            DateTime dtPurchase = DateTime.Now;
            string strCustomerEmail = string.Empty;
            string strCustomerName = string.Empty;
            string strCustomerPhone = string.Empty;
            string strSKU = string.Empty;
            string strProductName = string.Empty;
            int iQuantity = 0;
            decimal dItemPrice = 0M;
            decimal dItemTax = 0M;
            decimal dShippingPrice = 0M;
            decimal dShippingTax = 0M;
            string strShippingMethod = string.Empty;
            string strShippingName = string.Empty;
            string strShippingAddress1 = string.Empty;
            string strShippingAddress2 = string.Empty;
            string strShippingAddress3 = string.Empty;
            string strShippingCity = string.Empty;
            string strShippingState = string.Empty;
            string strShippingZip = string.Empty;
            string strShippingCountry = string.Empty;
            string strShippingPhone = string.Empty;
            decimal dTotalTax = 0M;
            string strOrderNotes = string.Empty;
            String PaymentDate = "";
            Decimal decTax1 = Decimal.Zero;
            Decimal decTax2 = Decimal.Zero;
            #endregion

            try
            {
                SQLAccess dbAccess = new SQLAccess();

                if (ds != null && ds.Tables.Count > 4 && ds.Tables["Order"].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables["Order"].Rows.Count; i++)
                    {
                        try
                        {

                            iAmazonOrderNumber = ds.Tables["Order"].Rows[i]["Id"].ToString();
                            bool bOrderExists = false;

                            string sql = "select 'True' from tb_Order where isnull(RefOrderID,'')='" + iAmazonOrderNumber + "' and storeid=" + StoreID;
                            bOrderExists = Convert.ToBoolean(dbAccess.ExecuteScalarQuery(sql));
                            strShippingMethod = ds.Tables["Order"].Rows[i]["ShipMethod"].ToString();
                            int orderSID = 0;
                            Int32 CustID = 0;
                            if (!bOrderExists)
                            {

                                DataRow[] drAddress = ds.Tables["Address"].Select("order_id=" + ds.Tables["Order"].Rows[i]["order_id"].ToString() + "");
                                if (drAddress != null && drAddress.Length > 0)
                                {
                                    strShippingName = Convert.ToString(drAddress[0]["Name"].ToString());
                                    strShippingAddress1 = Convert.ToString(drAddress[0]["Address1"].ToString());
                                    strShippingAddress2 = Convert.ToString(drAddress[0]["Address2"].ToString());
                                    strShippingAddress3 = "";
                                    strShippingCity = Convert.ToString(drAddress[0]["City"].ToString());
                                    strShippingZip = Convert.ToString(drAddress[0]["ZipCode"].ToString());
                                    strShippingCountry = "";
                                    strShippingPhone = "";
                                    strShippingState = Convert.ToString(dbAccess.ExecuteScalarQuery("select Name from tb_State where Abbreviation='" + Convert.ToString(drAddress[0]["State"].ToString()) + "'"));
                                    strShippingCountry = Convert.ToString(dbAccess.ExecuteScalarQuery("select isnull(Name,'') from tb_Country where TwoLetterISOCode='" + Convert.ToString(drAddress[0]["CountryCode"].ToString()) + "'"));

                                }

                                SQLAccess objdb = new SQLAccess();
                                CustID = Convert.ToInt32(objdb.ExecuteScalarQuery("INSERT INTO tb_Customer(StoreID,Email,FirstName,LastName,Active,Deleted,IsRegistered) VALUES (" + StoreID + ",'','" + strShippingName.Replace("'", "''") + "','',0,1,0); SELECT SCOPE_IDENTITY();"));

                                orderSID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("insert into tb_OrderedShoppingCart (CustomerID,CreatedOn) values(" + CustID + ",getdate()); SELECT SCOPE_IDENTITY();"));

                                DataRow[] drProduct = ds.Tables["InvoiceLine"].Select("order_Id=" + ds.Tables["Order"].Rows[i]["order_Id"].ToString() + "");
                                Decimal OrderTotal = 0;
                                Decimal OrderSubtotal = 0;
                                Decimal Shippingtotal = 0;
                                if (drProduct != null && drProduct.Length > 0)
                                {
                                    foreach (DataRow dr in drProduct)
                                    {

                                        Int32 ProductID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("select Productid from tb_product where (overstockproductid='" + dr["Id"].ToString() + "' or sku='" + dr["VendorSku"].ToString().Replace("'", "''") + "') and storeid=" + StoreID));
                                        if (ProductID <= 0)
                                        {
                                            ProductID = Convert.ToInt32(dbAccess.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,Description,MainCategory,Weight,Inventory,Price,SalePrice,StoreID,overstockproductid,deleted,Active) VALUES ('" + dr["name"].ToString().Replace("'", "''") + "','" + dr["VendorSku"].ToString().Replace("'", "''") + "','','',1,999," + dr["Cost"].ToString() + "," + dr["Cost"].ToString() + "," + StoreID + ",'" + dr["Id"].ToString().Replace("'", "''") + "',0,1); SELECT SCOPE_IDENTITY();"));
                                        }

                                        dbAccess.ExecuteNonQuery("insert into tb_OrderedShoppingCartItems (OrderedShoppingCartID,RefProductID,Quantity,Price,VariantNames,VariantValues,ProductName,SKU,OrderItemID) " +
                                           " values(" + orderSID + ",'" + ProductID + "','" + dr["Quantity"].ToString() + "'," + dr["Cost"].ToString() + ",'','','" + dr["name"].ToString().Replace("'", "''") + "','" + dr["VendorSku"].ToString().Replace("'", "''") + "','" + dr["Id"].ToString().Replace("'", "''") + "')");
                                        OrderSubtotal = OrderSubtotal + Convert.ToDecimal(Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Cost"].ToString()));
                                        Shippingtotal = Shippingtotal + Convert.ToDecimal(dr["ShipCost"].ToString());
                                    }

                                }

                                OrderTotal = OrderSubtotal + Shippingtotal;
                                Decimal OrderTax = 0;
                                Int32 OrderNumber = Convert.ToInt32(dbAccess.ExecuteScalarQuery(@"insert into tb_Order (GiftCertificateDiscountAmount,orderStatus,StoreID,CustomerID,ShoppingCardID,LastName,FirstName,Email,BillingLastName,BillingFirstName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingState,BillingZip,BillingCountry,BillingPhone,ShippingFirstName,ShippingLastName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingState,ShippingCity,ShippingZip,ShippingCountry,ShippingPhone,OrderTotal,OrderSubtotal,OrderShippingCosts,OrderTax,Deleted,RefOrderID,PaymentMethod,OrderDate,Isnew,LevelDiscountAmount,CouponDiscountAmount,TransactionStatus,CardType,CardVarificationCode,CardNumber,CardName,CardExpirationMonth,CardExpirationYear,Last4,paymentgateway,CustomDiscount,ShippingMethod,ShippedOn,ShippedVIA,QuantityDiscountAmount)
 
 values(0,'New'," + StoreID + "," + CustID + "," + orderSID + ",'','" + strShippingName.ToString().Replace("'", "''") + "','','','" + strShippingName.ToString().Replace("'", "''") + "','','" + strShippingAddress1.ToString().Replace("'", "''") + "','" + strShippingAddress2.ToString().Replace("'", "''") + "','','" + strShippingCity.ToString().Replace("'", "''") + "','" + strShippingState + "','" + strShippingZip.ToString().Replace("'", "''") + "','" + strShippingCountry.Replace("'", "''") + "','" + strShippingPhone + "','" + strShippingName.ToString().Replace("'", "''") + "','','','" + strShippingAddress1.ToString().Replace("'", "''") + "','" + strShippingAddress2.ToString().Replace("'", "''") + "','" + strShippingState.Replace("'", "''") + "','" + strShippingCity.ToString().Replace("'", "''") + "','" + strShippingZip.ToString().Replace("'", "''") + "','" + strShippingCountry.Replace("'", "''") + "','" + strShippingPhone + "','" + OrderTotal + "','" + OrderSubtotal + "','" + Shippingtotal + "','" + OrderTax + "',0,'" + iAmazonOrderNumber.ToString().Replace("'", "''").Trim() + "','CREDITCARD','" + String.Format("{0:MM/dd/yyyy hh:mm:ss ttt}", Convert.ToDateTime(ds.Tables["Order"].Rows[i]["Date"].ToString())) + "',1,0,0,'CAPTURED','','','','','','','','AUTHORIZENET',0,'" + strShippingMethod + "','','','0.0'); SELECT SCOPE_IDENTITY();"));


                                //dbAccess.ExecuteNonQuery("update tb_order SET OrderDate='" + dtPurchase + "' where OrderNumber='" + OrderNumber + "' and storeid=" + StoreID + "");

                            }

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }
        protected void btnSKU_Click(object sender, EventArgs e)
        {

            DataSet dsvaraintid = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();

            dsvaraintid = objSql.GetDs("SELECT * FROM " + txtTablename.Text.ToString() + " WHERE lower(isnull([on Site],''))<>''");
            if (dsvaraintid != null && dsvaraintid.Tables.Count > 0 && dsvaraintid.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsvaraintid.Tables[0].Rows.Count; i++)
                {
                    DataSet dsvaraintid1 = new DataSet();
                    dsvaraintid1 = objSql.GetDs("SELECT isnull(sku,'') as SKU,isnull(UPC,'') as UPC,isnull(Header,'') as Header,isnull([Overstock SKU],'') as [Overstock SKU] FROM " + txtTablename.Text.ToString() + " WHERE SKU like '%" + dsvaraintid.Tables[0].Rows[i]["sku"].ToString().Replace("'", "''") + "-%' AND  isnull([on Site],'')=''");
                    Int32 ProductId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT TOP 1 ProductId FROM tb_product WHERE isnull(sku,'')='" + dsvaraintid.Tables[0].Rows[i]["sku"].ToString().Replace("'", "''") + "' AND Storeid=2"));
                    if (dsvaraintid1 != null && dsvaraintid1.Tables.Count > 0 && dsvaraintid1.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsvaraintid1.Tables[0].Rows.Count; j++)
                        {
                            if (!string.IsNullOrEmpty(dsvaraintid1.Tables[0].Rows[j]["sku"].ToString()))
                            {
                                string strSKu = "";
                                if (dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().IndexOf("-") > -1)
                                {
                                    strSKu = dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().Substring(0, dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().LastIndexOf("-"));
                                }
                                else
                                {
                                    strSKu = dsvaraintid1.Tables[0].Rows[j]["sku"].ToString();
                                }
                                if (strSKu.ToString().ToLower() == dsvaraintid.Tables[0].Rows[i]["sku"].ToString().ToLower())
                                {
                                    if (!string.IsNullOrEmpty(dsvaraintid1.Tables[0].Rows[j]["Header"].ToString()))
                                    {
                                        objSql.ExecuteNonQuery("UPDATE tb_ProductVariantValue SET SKU='" + dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().Replace("'", "''") + "',UPC='" + dsvaraintid1.Tables[0].Rows[j]["UPC"].ToString().Replace("'", "''") + "',Header='" + dsvaraintid1.Tables[0].Rows[j]["Header"].ToString().Replace("'", "''") + "' WHERE Productid=" + ProductId + " AND  VariantValue like '%" + dsvaraintid1.Tables[0].Rows[j]["Header"].ToString().Replace("'", "''") + "%'");

                                        objSql.ExecuteNonQuery("UPDATE tb_ProductVariantValue SET SKU='" + dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().Replace("'", "''") + "',UPC='" + dsvaraintid1.Tables[0].Rows[j]["UPC"].ToString().Replace("'", "''") + "',Header='" + dsvaraintid1.Tables[0].Rows[j]["Header"].ToString().Replace("'", "''") + "' WHERE Productid=" + ProductId + " AND  replace(replace(replace(replace(lower(VariantValue),'w x 84l','wx84l'),'w x 96l','wx96l'),'w x 108l','wx108l'),'w x 120l','wx120l') like '%" + dsvaraintid1.Tables[0].Rows[j]["Header"].ToString().Replace("'", "''").ToLower() + "%'");
                                        Int32 ProductIdoverStock = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT TOP 1 ProductId FROM tb_product WHERE isnull(sku,'')='" + dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().Replace("'", "''") + "' AND Storeid=4"));
                                        if (ProductIdoverStock > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dsvaraintid1.Tables[0].Rows[i]["Overstock SKU"].ToString()))
                                            {
                                                objSql.ExecuteNonQuery("UPDATE tb_Product SET OptionSku='" + dsvaraintid1.Tables[0].Rows[i]["Overstock SKU"].ToString() + "',UPC='" + dsvaraintid1.Tables[0].Rows[j]["UPC"].ToString().Replace("'", "''") + "' WHERE Storeid=4 AND Productid=" + ProductIdoverStock.ToString() + "");
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(dsvaraintid1.Tables[0].Rows[i]["Overstock SKU"].ToString()))
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_Product(Name,SKU,Inventory,Storeid,price,Saleprice,Active,Deleted,Description,OptionSku,UPC) SELECT Name,'" + dsvaraintid1.Tables[0].Rows[j]["sku"].ToString().Replace("'", "''") + "',Inventory,4,price,Saleprice,Active,Deleted,Description,'" + dsvaraintid1.Tables[0].Rows[i]["Overstock SKU"].ToString() + "','" + dsvaraintid1.Tables[0].Rows[j]["UPC"].ToString().Replace("'", "''") + "' FROM tb_Product WHERE isnull(sku,'')='" + dsvaraintid.Tables[0].Rows[i]["sku"].ToString().Replace("'", "''") + "' AND Storeid=2 ");
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        private void GetorderOverStockTime()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");


            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>EBD659E4-EF74-45CD-A072-0E1A0F52AC59</MerchantKey>");
            //transactionCommand.Append("<AuthenticationKey>601509EC-5D21-4EA4-A594-E2042CFEF316</AuthenticationKey>");
            transactionCommand.Append("<GetOverstockTime />");
            transactionCommand.Append("</Request>");

            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.Headers.Add("SapiMethodName", "GetOverstockTime");
            // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
            }
            catch
            {
            }
        }

        private void GetConfirmShipmentOverStock()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>EBD659E4-EF74-45CD-A072-0E1A0F52AC59</MerchantKey>");
            transactionCommand.Append("<AuthenticationKey>6C206CF4-D2D5-4E5D-A935-378511E3B058</AuthenticationKey>");
            transactionCommand.Append("<ConfirmShipment>");
            transactionCommand.Append("<InvoiceLine>");
            transactionCommand.Append("<Id>232436310</Id>");
            transactionCommand.Append("<Quantity>3</Quantity>");
            transactionCommand.Append("<Shipment>");
            transactionCommand.Append("<CarrierCode>USPS</CarrierCode>");
            transactionCommand.Append("<ShipDate>2013-04-29</ShipDate>");
            transactionCommand.Append("<TrackingNumber>123456789</TrackingNumber>");
            transactionCommand.Append("<PartnerInvoiceNumber>86144944-1</PartnerInvoiceNumber>");
            transactionCommand.Append("</Shipment>");
            transactionCommand.Append("</InvoiceLine>");
            transactionCommand.Append("</ConfirmShipment>");
            transactionCommand.Append("</Request>");

            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.Headers.Add("SapiMethodName", "ConfirmShipment");
            // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
            }
            catch
            {
            }
        }
        //private void UpdaeoverStockQuantity()
        //{
        //    Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
        //    string MerchantKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='MerchantKey' AND Storeid=" + ddlStore.SelctedValue.ToString() + ""));
        //    string AuthenticationKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthenticationKey' AND Storeid=" + ddlStore.SelctedValue.ToString() + ""));
        //    String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + ddlStore.SelctedValue.ToString() + ""));


        //    ASCIIEncoding encoding = new ASCIIEncoding();
        //    StringBuilder transactionCommand = new StringBuilder(4096);
        //    DataSet ds = new DataSet();
        //    transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
        //    transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");


        //    transactionCommand.Append("<MerchantKey>" + MerchantKey + "</MerchantKey>");
        //    transactionCommand.Append("<AuthenticationKey>" + AuthenticationKey + "</AuthenticationKey>");
        //    transactionCommand.Append("<UpdateQuantity>");

        //    foreach (GrdiViewRow gr in grdProduc.Rows)
        //    {
        //        Label lblSku = (Label)gr.FindControl("lblSKU");
        //        Label lblInventory = (Label)gr.FindControl("lblInventory");
        //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //        if (chkSelect.Checked == true)
        //        {
        //            transactionCommand.Append("<Option>");
        //            transactionCommand.Append("<OptionSku>" + lblSku.Text.ToString() + "</OptionSku>");
        //            transactionCommand.Append("<Quantity>" + lblInventory.Text.ToString() + "</Quantity>");
        //            transactionCommand.Append("</Option>");
        //        }
        //    }
        //    transactionCommand.Append("</UpdateQuantity>");
        //    transactionCommand.Append("</Request>");

        //    System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
        //    byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
        //    myRequest.Method = "POST";
        //    myRequest.Timeout = 300000;
        //    myRequest.Headers.Add("SapiMethodName", "UpdateQuantity");
        //    // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
        //    myRequest.ContentType = "application/xml";
        //    myRequest.ContentLength = data.Length;
        //    Stream newStream = myRequest.GetRequestStream();
        //    // Send the data.
        //    newStream.Write(data, 0, data.Length);
        //    newStream.Close();
        //    // get the response
        //    WebResponse myResponse;
        //    String rawResponseString = String.Empty;
        //    try
        //    {
        //        myResponse = myRequest.GetResponse();
        //        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
        //        {
        //            rawResponseString = sr.ReadToEnd();
        //            // Close and clean up the StreamReader
        //            sr.Close();
        //        }
        //        XmlDocument xDoc = new XmlDocument();
        //        xDoc.LoadXml(rawResponseString);
        //        ds.ReadXml(new XmlNodeReader(xDoc));
        //        try
        //        {
        //            if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
        //            {
        //                Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
        //            }
        //            ds.WriteXml(Server.MapPath("/OverstockOrder/Updateproduct-" + DateTime.Now.Ticks.ToString() + ".xml"));
        //        }
        //        catch { }
        //        myResponse.Close();
        //    }
        //    catch
        //    {
        //    }
        //}
        private void GetorderOverStock()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");

            SQLAccess objdb = new SQLAccess();
            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            //transactionCommand.Append("<MerchantKey>3416EEFD-D935-49FC-A594-4841CD062B39</MerchantKey>");
            transactionCommand.Append("<MerchantKey>4D2A65B4-E203-48CB-9334-13C1A78AC0C7</MerchantKey>");
            transactionCommand.Append("<AuthenticationKey>8144B987-4D5D-4106-9208-801E747F4D7F</AuthenticationKey>"); //
            transactionCommand.Append("<GetOrders2 />");
            transactionCommand.Append("</Request>");
             String AuthServer = "https://sapi.overstock.com/api";
            //string MerchantKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='MerchantKey' AND Storeid=4"));
            //string AuthenticationKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthenticationKey' AND Storeid=4"));
            // String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=4"));
            //transactionCommand.Append("<MerchantKey>" + MerchantKey + "</MerchantKey>");
            //transactionCommand.Append("<AuthenticationKey>" + AuthenticationKey + "</AuthenticationKey>"); //
            //transactionCommand.Append("<GetOrders2 />");
            //transactionCommand.Append("</Request>");


            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());
      
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.Headers.Add("SapiMethodName", "GetOrders2");
            // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    Response.Write(rawResponseString);
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                Response.Write(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/order-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
            }
            catch
            {
            }
        }
        private void GetorderOverStockGetrec()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();

            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>4D2A65B4-E203-48CB-9334-13C1A78AC0C7</MerchantKey>");
            transactionCommand.Append("<GetCredentials />");
            //transactionCommand.Append("<MethodName>SapiMethodName: GetCredentials</MethodName>");
            transactionCommand.Append("</Request>");
            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());
            String AuthServer = "https://sapi.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            //myRequest.Headers.Add("content-type","text/xml");
            myRequest.Headers.Add("SapiMethodName", "GetCredentials");
            myRequest.Timeout = 300000;

            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/Auth" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
            }
            catch
            {
            }
        }
        private void GetorderOverStockGetCategories()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");


            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>EBD659E4-EF74-45CD-A072-0E1A0F52AC59</MerchantKey>");
            transactionCommand.Append("<AuthenticationKey>6C206CF4-D2D5-4E5D-A935-378511E3B058</AuthenticationKey>");
            // transactionCommand.Append("<GetCategories />");
            transactionCommand.Append("<GetProduct2>");
            transactionCommand.Append("<ProductSku>12283667</ProductSku>");
            transactionCommand.Append("</GetProduct2>");
            transactionCommand.Append("</Request>");

            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            myRequest.Method = "POST";
            myRequest.Timeout = 300000;
            myRequest.Headers.Add("SapiMethodName", "GetProduct2");
            // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            myRequest.ContentType = "application/xml";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/Category-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch { }
                myResponse.Close();
            }
            catch
            {
            }
        }


        //protected override void Render(HtmlTextWriter writer)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    StringWriter strngWriter = new StringWriter(builder);
        //    HtmlTextWriter mywriter = new HtmlTextWriter(strngWriter);
        //    base.Render(mywriter);
        //    string s = builder.ToString();
        //    s = s.Replace("<meta ", "\n<meta ");
        //    StringBuilder frameHTML = new StringBuilder();
        //    //set the source of the iframe to the path of the PDF
        //    frameHTML.Append("<iframe src=" + Convert.ToString(FilePath) + " ");
        //    //set height and width of the frame
        //    frameHTML.Append("width=" + width + "px height=" + height + "px");
        //    frameHTML.Append("</iframe>");

        //    //s = System.Text.RegularExpressions.Regex.Replace(s, "/client/images/", AppLogic.AppConfig("ContentServer") + "/client/images/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    //s = System.Text.RegularExpressions.Regex.Replace(s, AppLogic.AppConfig("imagespath"), AppLogic.AppConfig("ContentServer") + AppLogic.AppConfig("imagespath"), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    Response.Write(s);
        //}

        private void ImportSearsOrder()
        {
            //SQLAccess objdb = new SQLAccess();
            //DataSet DsSears = new DataSet();

            //string Orderstatuss = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='SearsOrderStatus' And Storeid=10"));//"173.192.80.91";
            //Int32 OrderDays = Convert.ToInt32(objdb.ExecuteScalerQuery("SELECT isnull(configValue,0) FROM tb_ecomm_AppConfig WHERE Configname='SearsOrderDays' And Storeid=10"));//"173.192.80.91";
            //string UserName = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='Searsusername' And Storeid=10"));//"173.192.80.91";
            //string password = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='SearsPassword' And Storeid=10"));//"173.192.80.91";

            //string startdate = DateTime.Now.Date.AddDays(OrderDays).ToString("yyyy-MM-dd");
            //string EndDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

            // DsSears.ReadXml(@"https://seller.marketplace.sears.com/SellerPortal/api/oms/purchaseorder/v3?email=" + UserName + "&password=" + password + "&fromdate=" + startdate + "&todate=" + EndDate + "&status=" + Orderstatuss + "");
            try
            {
                SQLAccess objdb = new SQLAccess();
                DataSet DsSears = new DataSet();
                //objdb = new SQLAccess();
                //string Orderstatuss = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_AppConfig WHERE Configname='SearsOrderStatus' And Storeid=8"));//"173.192.80.91";
                //objdb = new SQLAccess();
                //Int32 OrderDays = Convert.ToInt32(objdb.ExecuteScalerQuery("SELECT isnull(configValue,0) FROM tb_AppConfig WHERE Configname='SearsOrderDays' And Storeid=8"));//"173.192.80.91";
                //objdb = new SQLAccess();
                //string UserName = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_AppConfig WHERE Configname='Searsusername' And Storeid=8"));//"173.192.80.91";
                //objdb = new SQLAccess();
                //string password = Convert.ToString(objdb.ExecuteScalerQuery("SELECT configValue FROM tb_AppConfig WHERE Configname='SearsPassword' And Storeid=8"));//"173.192.80.91";

                string startdate = DateTime.Now.Date.AddDays(0).ToString("yyyy-MM-dd");
                string EndDate = DateTime.Now.Date.ToString("yyyy-MM-dd");

                //DsSears.ReadXml(@"https://seller.marketplace.sears.com/SellerPortal/api/oms/purchaseorder/v3?email=" + UserName + "&password=" + password + "&fromdate=" + startdate + "&todate=" + EndDate + "&status=" + Orderstatuss + "");
                DsSears.ReadXml(Server.MapPath("/Searsorder10may2013.xml"));
                if (DsSears != null && DsSears.Tables.Count > 0 && DsSears.Tables["purchase-order"].Rows.Count > 0)
                {

                    for (int i = 0; i < DsSears.Tables["purchase-order"].Rows.Count; i++)
                    {
                        objdb = new SQLAccess();
                        Int32 strOrderId = Convert.ToInt32(objdb.ExecuteScalarQuery("SELECT Count(RefOrderID) FROM tb_Order WHERE RefOrderID='" + DsSears.Tables["purchase-order"].Rows[i]["customer-order-confirmation-number"].ToString().Trim() + "'"));
                        if (strOrderId == 0)
                        {
                            Int32 CustID = 0;
                            DataRow[] drShipping = null;
                            DataRow[] drPolIne = null;
                            DataRow[] drPoHeader = null;
                            string strShippingMethod = "";
                            DataRow[] drPoDetails = null;

                            if (DsSears.Tables["shipping-detail"].Rows.Count > 0)
                            {
                                drShipping = (DataRow[])DsSears.Tables["shipping-detail"].Select("[purchase-order_id]=" + DsSears.Tables["purchase-order"].Rows[i]["purchase-order_id"].ToString() + "");


                                CustID = Convert.ToInt32(objdb.ExecuteScalarQuery("select top 1 CustomerID from tb_customer where Active=0 AND Email='" + DsSears.Tables["purchase-order"].Rows[i]["customer-email"].ToString() + "' and storeid=8"));
                                try
                                {
                                    strShippingMethod = drShipping[0]["shipping-method"].ToString();
                                    if (CustID == 0)
                                    {

                                        CustID = Convert.ToInt32(objdb.ExecuteScalarQuery("INSERT INTO tb_Customer(StoreID,Email,FirstName,LastName,Active,Deleted,IsRegistered,Phone) VALUES (8,'" + DsSears.Tables["purchase-order"].Rows[i]["customer-email"].ToString() + "','" + drShipping[0]["ship-to-name"].ToString().Replace("'", "''") + "','',0,1,0,'" + drShipping[0]["phone"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                    }
                                }
                                catch
                                {

                                }
                            }
                            objdb = new SQLAccess();
                            Int32 orderSID = Convert.ToInt32(objdb.ExecuteScalarQuery("insert into tb_OrderedShoppingCart (CustomerID,CreatedOn) values(" + CustID + ",getdate()); SELECT SCOPE_IDENTITY();"));

                            // orderSID++;
                            //objdb = new SQLAccess();
                            //objdb.ExecuteNonQuery("insert into tb_Ecomm_OrderedShoppingCart (OrderedShoppingCartID,CustomerID,CouponCode,CreatedOn) values(" + orderSID + "," + CustID + ",'',getdate())");

                            if (DsSears.Tables["po-line"].Rows.Count > 0)
                            {
                                drPolIne = DsSears.Tables["po-line"].Select("[purchase-order_id]=" + DsSears.Tables["purchase-order"].Rows[i]["purchase-order_id"].ToString() + "");
                            }
                            foreach (DataRow dritem in drPolIne)
                            {
                                if (dritem != null)
                                {
                                    drPoHeader = DsSears.Tables["po-line-header"].Select("[po-line_id]=" + dritem["po-line_id"].ToString() + "");
                                    decimal OrderSubtotal = 0;
                                    foreach (DataRow dritemHeader in drPoHeader)
                                    {
                                        objdb = new SQLAccess();
                                        Int32 ProductID = Convert.ToInt32(objdb.ExecuteScalarQuery("Select ProductID from tb_product where SKU='" + dritemHeader["item-id"].ToString().Replace("'", "''") + "' and storeid=8"));

                                        if (ProductID > 0)
                                        {

                                        }
                                        else
                                        {
                                            ProductID = Convert.ToInt32(objdb.ExecuteScalarQuery("insert into tb_Product (Name,SKU,Price,SalePrice,Active,Deleted,Inventory,StoreId) values('" + dritemHeader["item-name"].ToString().Replace("'", "''") + "','" + dritemHeader["item-id"].ToString().Replace("'", "''") + "','" + dritemHeader["selling-price-each"].ToString() + "','" + dritemHeader["selling-price-each"].ToString() + "',1,1,9999,8); SELECT SCOPE_IDENTITY();"));
                                        }
                                        decimal pricecomm = Convert.ToDecimal(dritemHeader["order-quantity"].ToString()) * Convert.ToDecimal(dritemHeader["commission"].ToString());
                                        decimal price1 = Convert.ToDecimal(dritemHeader["order-quantity"].ToString()) * Convert.ToDecimal(dritemHeader["selling-price-each"].ToString());
                                        // price1 = price1;
                                        objdb = new SQLAccess();



                                        objdb.ExecuteNonQuery("insert into tb_OrderedShoppingCartItems (OrderedShoppingCartID,RefProductID,Quantity,Price,VariantNames,VariantValues,ProductName,SKU,OrderItemID) " +
                                               " values(" + orderSID + ",'" + ProductID + "','" + dritemHeader["order-quantity"].ToString() + "'," + dritemHeader["selling-price-each"].ToString() + ",'','','" + dritemHeader["item-name"].ToString().Replace("'", "''") + "','" + dritemHeader["item-id"].ToString().Replace("'", "''") + "','" + dritemHeader["item-id"].ToString().Replace("'", "''") + "')");

                                        OrderSubtotal += price1;

                                        string Stringststate = Convert.ToString(objdb.ExecuteScalarQuery("SELECT  Name FROM tb_State WHERE Abbreviation='" + drShipping[0]["state"].ToString() + "'"));

                                        string CountryID = Convert.ToString(objdb.ExecuteScalarQuery("SELECT Name FROM tb_Country WHERE CountryID in (SELECT CountryID FROM tb_State WHERE Abbreviation='" + drShipping[0]["state"].ToString() + "')"));
                                        if (CountryID.ToString().Trim() == "")
                                        {
                                            CountryID = "";
                                        }

                                        //  decimal OrderSubtotal = Convert.ToDecimal(dtRecord.Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(dtRecord.Rows[i]["price"].ToString());
                                        //decimal Ordertotal = OrderSubtotal + Convert.ToDecimal(dtRecord.Rows[i]["Shipping_Cost"].ToString()) + Convert.ToDecimal(dtRecord.Rows[i]["Tax_Cost"].ToString());


                                        int year = Convert.ToInt32(DateTime.Now.Year.ToString());

                                        decimal totaldiscount = Convert.ToDecimal(DsSears.Tables["purchase-order"].Rows[i]["total-shipping-handling"].ToString()) + Convert.ToDecimal(DsSears.Tables["purchase-order"].Rows[i]["balance-due"].ToString()) + +Convert.ToDecimal(DsSears.Tables["purchase-order"].Rows[i]["sales-tax"].ToString());

                                        //drPoDetails = DsSears.Tables["po-line-detail"].Select("po-line_id=" + dritem["po-line_id"].ToString() + "");
                                        //string strshipstatus = "";
                                        //if (drPoDetails != null)
                                        //{
                                        //    strshipstatus = drPoDetails["po-line-status"].ToString();
                                        //}


                                        string dtOrder = string.Format("{0:MM/dd/yyyy hh:mm:ss tt}", Convert.ToDateTime(DsSears.Tables["purchase-order"].Rows[i]["po-date"].ToString() + " " + DsSears.Tables["purchase-order"].Rows[i]["po-time"].ToString())); //Convert.ToDateTime(DsSears.Tables["purchase-order"].Rows[i]["po-date"].ToString().Replace("'", "''"));
                                        DateTime ShipOrder = Convert.ToDateTime(DsSears.Tables["purchase-order"].Rows[i]["expected-ship-date"].ToString().Replace("'", "''"));


                                        Int32 OrderNumber = Convert.ToInt32(objdb.ExecuteScalarQuery(@"insert into tb_Order (StoreID,CustomerID,ShoppingCardID,LastName,FirstName,Email,BillingLastName,BillingFirstName,BillingCompany,BillingAddress1,BillingAddress2,BillingSuite,BillingCity,BillingState,BillingZip,BillingCountry,BillingPhone,ShippingFirstName,ShippingLastName,ShippingCompany,ShippingAddress1,ShippingAddress2,ShippingState,ShippingCity,ShippingZip,ShippingCountry,ShippingPhone,OrderTotal,OrderSubtotal,OrderShippingCosts,OrderTax,Deleted,RefOrderID,PaymentMethod,OrderDate,Isnew,LevelDiscountAmount,CouponDiscountAmount,TransactionStatus,CardType,CardVarificationCode,CardNumber,CardName,CardExpirationMonth,CardExpirationYear,Last4,paymentgateway,CustomDiscount,ShippingMethod,ShippedOn,ShippedVIA,QuantityDiscountAmount)

values(8," + CustID + "," + orderSID + ",'','" + drShipping[0]["ship-to-name"].ToString().Replace("'", "''") + "','" + DsSears.Tables["purchase-order"].Rows[i]["customer-email"].ToString() + "','','" + drShipping[0]["ship-to-name"].ToString().Replace("'", "''") + "','','" + drShipping[0]["address"].ToString().Replace("'", "''") + "','','','" + drShipping[0]["city"].ToString().Replace("'", "''") + "','" + Stringststate + "','" + drShipping[0]["zipcode"].ToString().Replace("'", "''") + "','" + CountryID + "','" + drShipping[0]["phone"] + "','" + drShipping[0]["ship-to-name"].ToString().Replace("'", "''") + "','','','" + drShipping[0]["address"].ToString().Replace("'", "''") + "','','" + Stringststate + "','" + drShipping[0]["city"].ToString().Replace("'", "''") + "','" + drShipping[0]["zipcode"].ToString().Replace("'", "''") + "','" + CountryID + "','" + drShipping[0]["phone"] + "','" + totaldiscount + "','" + OrderSubtotal + "','" + DsSears.Tables["purchase-order"].Rows[i]["total-shipping-handling"].ToString() + "','" + DsSears.Tables["purchase-order"].Rows[i]["sales-tax"].ToString() + "',0,'" + DsSears.Tables["purchase-order"].Rows[i]["customer-order-confirmation-number"].ToString().Replace("'", "''").Trim() + "',1,'" + dtOrder + "',1,0,0,'CAPTURED','','','','','','','','AUTHORIZENET',0,'" + strShippingMethod + "','" + ShipOrder.ToString() + "','','" + DsSears.Tables["purchase-order"].Rows[i]["total-commission"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                        //FillShipment(ProductID, strShippingMethod, "", DsSears.Tables["purchase-order"].Rows[i]["customer-order-confirmation-number"].ToString().Replace("'", "''").Trim(), ShipOrder);

                                        //objdb.ExecuteNonQuery("UPDATE tb_Ecomm_Product SET Inventory=isnull(Inventory,0)-" + dritemHeader["order-quantity"].ToString() + " WHERE SKU='" + dritemHeader["item-id"].ToString().Replace("'", "''") + "'");

                                    }

                                }
                            }
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                //WriteLog("Sears Error1: " + ex.Message.ToString() + ex.StackTrace.ToString());
            }

        }

        private void GetLNTOrder()
        {

            try
            {
                //string postString = string.Format("api_user:{0},api_key:{1},function_name:{2},order_id:{3},return_data_type:{4}", "EF_API", "9ef87dcbc2d9d7cc030635e48acdc77f", "GetOrderById", "7092079", "json");
                string postString = "\"login\":{\"api_user\":\"EF_API\",\"api_key\":\"9ef87dcbc2d9d7cc030635e48acdc77f\"},\"function_name\":\"GetOrderById\",\"parameters\":{\"order_id\":\"98277954\"},\"return_data_type\":\"json\"";
                byte[] arr = System.Text.Encoding.UTF8.GetBytes(postString);
                // Prepare web request...
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://client.torreycommerce.com/exclusivefabrics/API/Shop/Order");
                if (TextBox1.Text.ToString() == "1")
                {
                    myRequest.Credentials = new NetworkCredential("EF_API", "watercurtaincubepencil");//watercurtaincubepencil
                }
                // myRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                //myRequest.PreAuthenticate = true;

                myRequest.Headers.Add("API-Version:2");
                myRequest.Accept = "application/json";
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json";
                //myRequest.KeepAlive = true;
                myRequest.ContentLength = arr.Length;

                //myRequest.Headers.Add("api_key", "9ef87dcbc2d9d7cc030635e48acdc77f");
                //myRequest.Headers.Add("function_name", "GetOrderById");
                //myRequest.Headers.Add("order_id", "7092079");
                //myRequest.Headers.Add("return_data_type", "json");
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(arr, 0, arr.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                Encoding enc = System.Text.Encoding.GetEncoding(1252);
                StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);
                try
                {
                    string Response = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    response.Close();

                }
                catch
                {


                    loResponseStream.Close();
                    response.Close();
                }
            }
            catch (Exception Ex)
            {
                Response.Write(Ex.Message.ToString() + " " + Ex.StackTrace.ToString());

            }



        }

        protected void lntbtn_Click(object sender, EventArgs e)
        {
            GetLNTOrder();
        }

    }
}