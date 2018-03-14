using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class testcat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }
        protected void btngo_Click(object sender, EventArgs e)
        {
            DataTable tableexport = new DataTable();
            tableexport.Columns.Add(new DataColumn("Category", typeof(String)));
            tableexport.Columns.Add(new DataColumn("SubCategory", typeof(String)));
            tableexport.Columns.Add(new DataColumn("SKU", typeof(String)));
            tableexport.Columns.Add(new DataColumn("UPC", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Name", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Price", typeof(double)));
            tableexport.Columns.Add(new DataColumn("SalePrice", typeof(double)));
            tableexport.Columns.Add(new DataColumn("BaseCustomPrice", typeof(double)));
            tableexport.Columns.Add(new DataColumn("Size", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Inventory", typeof(Int32)));
            tableexport.Columns.Add(new DataColumn("Colors", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Style", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Fabric", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Header", typeof(String)));
            tableexport.Columns.Add(new DataColumn("FabricCode", typeof(String)));
            tableexport.Columns.Add(new DataColumn("Buy1Get1", typeof(String)));
            tableexport.Columns.Add(new DataColumn("OnSale", typeof(String)));
           

            if (!string.IsNullOrEmpty(txtcat.Text.ToString()))
            {
                DataSet dsparent = new DataSet();
                dsparent = CommonComponent.GetCommonDataSet("select tb_Category.CategoryID,tb_Category.Name from  tb_Category where CategoryID in ('" + txtcat.Text.ToString() + "') and isnull(tb_Category.Active,0)=1 and tb_Category.Name not like 'shop all%'");
                if (dsparent != null && dsparent.Tables.Count > 0 && dsparent.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsparent.Tables[0].Rows.Count; j++)
                    {


                        DataSet dscat = new DataSet();
                        dscat = CommonComponent.GetCommonDataSet("select tb_Category.CategoryID,tb_Category.Name from tb_CategoryMapping inner join tb_Category on tb_Category.CategoryID=tb_CategoryMapping.CategoryID where ParentCategoryID =" + dsparent.Tables[0].Rows[j]["categoryid"].ToString() + " and isnull(tb_Category.Active,0)=1 and tb_Category.Name not like 'shop all%'");
                        if (dscat != null && dscat.Tables.Count > 0 && dscat.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dscat.Tables[0].Rows.Count; i++)
                            {
                                DataSet dsProduct = new DataSet();
                                dsProduct = CommonComponent.GetCommonDataSet("select productid, name,isnull(sku,'') as sku,isnull(upc,'') as upc,isnull(price,0) as price,isnull(saleprice,0) as saleprice,isnull(Colors,'') as Colors,isnull(style,'') as style ,isnull(Fabric,'') as Fabric,isnull(Header,'') as Header,isnull(inventory,0) as inventory,isnull(FabricCode,'') as FabricCode,0 as BasecustomPrice from tb_product where productid in (select distinct ProductID from tb_ProductCategory where CategoryID=" + dscat.Tables[0].Rows[i]["CategoryID"].ToString() + ") and isnull(active,0)=1 and isnull(tb_product.Deleted,0)=0 and isnull(tb_product.ItemType,'') <> 'swatch'");
                                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                                {

                                    for (int k = 0; k < dsProduct.Tables[0].Rows.Count; k++)
                                    {

                                        DataRow dr = tableexport.NewRow();
                                        dr["Category"] = _EscapeCsvField(dsparent.Tables[0].Rows[j]["Name"].ToString());
                                        dr["SubCategory"] = _EscapeCsvField(dscat.Tables[0].Rows[i]["Name"].ToString());
                                        dr["SKU"] = dsProduct.Tables[0].Rows[k]["sku"].ToString();
                                        dr["UPC"] = dsProduct.Tables[0].Rows[k]["UPC"].ToString();
                                        dr["Name"] = _EscapeCsvField(dsProduct.Tables[0].Rows[k]["Name"].ToString());
                                        dr["Price"] = dsProduct.Tables[0].Rows[k]["Price"].ToString();
                                        dr["SalePrice"] = dsProduct.Tables[0].Rows[k]["SalePrice"].ToString();
                                        dr["BasecustomPrice"] = dsProduct.Tables[0].Rows[k]["BasecustomPrice"].ToString();
                                        dr["Size"] = "";
                                        dr["Inventory"] = dsProduct.Tables[0].Rows[k]["Inventory"].ToString();
                                        dr["Colors"] = dsProduct.Tables[0].Rows[k]["Colors"].ToString();
                                        dr["style"] = dsProduct.Tables[0].Rows[k]["style"].ToString();
                                        dr["Fabric"] = dsProduct.Tables[0].Rows[k]["Fabric"].ToString();
                                        dr["Header"] = dsProduct.Tables[0].Rows[k]["Header"].ToString();
                                        dr["FabricCode"] = dsProduct.Tables[0].Rows[k]["FabricCode"].ToString();
                                        dr["Buy1Get1"] = "No";
                                        dr["OnSale"] = "No";
                                        tableexport.Rows.Add(dr);
                                        tableexport.AcceptChanges();

                                        DataSet dsvariant = new DataSet();
                                        dsvariant = CommonComponent.GetCommonDataSet("select isnull(sku,'') as sku,isnull(upc,'') as upc,isnull(VariantPrice,0) as price ,0 as SalePrice,isnull(BasecustomPrice,0) as BasecustomPrice,isnull(Weight,0) as [Weight],isnull(inventory,0) as inventory,isnull(VariantValue,'') as VariantValue,isnull(FabricCode,'') as FabricCode,(case when isnull(buy1get1,0)=1 and cast(buy1Todate as date)>=cast('" + DateTime.Now + "' as date) then 'Yes' else 'No' end) as buy1get1,(case when isnull(onsale,0)=1 and cast(onsaleTodate as date)>=cast('" + DateTime.Now + "' as date) then 'Yes' else 'No' end)  as onsale from tb_ProductVariantValue where  ProductID=" + dsProduct.Tables[0].Rows[k]["productid"].ToString() + "");
                                        if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                                        {
                                            for(int y=0;y<dsvariant.Tables[0].Rows.Count;y++)
                                            {
                                                 dr = tableexport.NewRow();
                                                 dr["Category"] = "";
                                                 dr["SubCategory"] = "";
                                                 dr["SKU"] = dsvariant.Tables[0].Rows[y]["sku"].ToString();
                                                 dr["UPC"] = dsvariant.Tables[0].Rows[y]["UPC"].ToString();
                                                 dr["Name"] = _EscapeCsvField(dsProduct.Tables[0].Rows[k]["Name"].ToString());
                                                dr["Price"] = dsvariant.Tables[0].Rows[y]["Price"].ToString();
                                                dr["SalePrice"] = dsvariant.Tables[0].Rows[y]["SalePrice"].ToString();
                                                dr["BasecustomPrice"] = dsvariant.Tables[0].Rows[y]["BasecustomPrice"].ToString();
                                                dr["Size"] = dsvariant.Tables[0].Rows[y]["VariantValue"].ToString(); 
                                                dr["Inventory"] = dsvariant.Tables[0].Rows[y]["Inventory"].ToString();
                                                dr["Colors"] = dsProduct.Tables[0].Rows[k]["Colors"].ToString();
                                                dr["style"] = dsProduct.Tables[0].Rows[k]["style"].ToString();
                                                dr["Fabric"] = dsProduct.Tables[0].Rows[k]["Fabric"].ToString();
                                                dr["Header"] = dsProduct.Tables[0].Rows[k]["Header"].ToString();
                                                dr["FabricCode"] = dsvariant.Tables[0].Rows[y]["FabricCode"].ToString();
                                                dr["Buy1Get1"] = dsvariant.Tables[0].Rows[y]["buy1get1"].ToString();
                                                dr["OnSale"] = dsvariant.Tables[0].Rows[y]["onsale"].ToString();
                                                tableexport.Rows.Add(dr);
                                                tableexport.AcceptChanges();
                                            }
                                        }

                                    }


                                }
                            }
                        }




                    }
                }


            }

            if (tableexport != null && tableexport.Rows.Count > 0)
            {
               
                DataView dvCust = new DataView();
               // if (tableexport != null && tableexport.Rows.Count > 0)
                {
                    dvCust = tableexport.DefaultView;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (dvCust != null)
                    {
                        for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                        {
                            object[] args = new object[16];
                            args[0] = Convert.ToString(dvCust.Table.Rows[i]["Category"]);
                            args[1] = Convert.ToString(dvCust.Table.Rows[i]["SubCategory"].ToString());
                            args[2] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
                            args[3] = Convert.ToString(dvCust.Table.Rows[i]["UPC"]);
                            args[4] = Convert.ToString(dvCust.Table.Rows[i]["Name"]);
                            args[5] = Convert.ToString(dvCust.Table.Rows[i]["Price"]);
                            args[6] = Convert.ToString(dvCust.Table.Rows[i]["SalePrice"]);
                            args[7] = Convert.ToString(dvCust.Table.Rows[i]["BasecustomPrice"]);
                            args[8] = Convert.ToString(dvCust.Table.Rows[i]["Size"]);
                            args[9] = Convert.ToString(dvCust.Table.Rows[i]["Inventory"]);
                            args[10] = Convert.ToString(dvCust.Table.Rows[i]["Colors"]);
                            args[11] = Convert.ToString(dvCust.Table.Rows[i]["style"]);
                            args[12] = Convert.ToString(dvCust.Table.Rows[i]["Fabric"]);
                            args[13] = Convert.ToString(dvCust.Table.Rows[i]["Header"]);
                            args[14] = Convert.ToString(dvCust.Table.Rows[i]["Buy1Get1"]);
                            args[15] = Convert.ToString(dvCust.Table.Rows[i]["OnSale"]);
                            sb.AppendLine(string.Format("{0},{1},\"{2}\",\"{3}\",{4},\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"", args));
                        }
                    }

                    if (!String.IsNullOrEmpty(sb.ToString()))
                    {

                        DateTime dt = DateTime.Now;
                        String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                        string FullString = sb.ToString();
                        sb.Remove(0, sb.Length);
                        sb.AppendLine("Category,SubCategory,SKU,UPC,Name,Price,SalePrice,BaseCustomPrice,Size,Inventory,Colors,Style,Fabric,Header,Buy1Get1,OnSale");
                        sb.AppendLine(FullString);

                        if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                            Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                        String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                        WriteFile(sb.ToString(), FilePath);
                        Response.ContentType = "text/csv";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                        Response.TransmitFile(FilePath);
                        Response.End();
                    }
                }

            }
        }
    }
}