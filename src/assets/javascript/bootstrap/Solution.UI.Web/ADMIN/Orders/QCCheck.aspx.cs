using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class QCCheck : BasePage
    {
       
             public static string ProductIconPath = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblmsg.Visible = true;
                txtUPC.Focus();
                btnGo.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif) no-repeat transparent;  width: 67px; height: 23px;border:none;cursor:pointer;");
            }
        }
     
        protected void btnGo_Click(object sender, EventArgs e)
        {
           
           lblmsg.Visible = false;
           DataSet ds = new DataSet();
            object StoreID=CommonComponent.GetScalarCommonData("SELECT StoreID from tb_Order where OrderNumber="+txtUPC.Text.ToString().Trim()+"");
            if (StoreID != null && StoreID!="")
            {
                ds = CommonComponent.GetCommonDataSet("select CASE WHEN tb_order.storeid=1 then '' when isnull(tb_product.Upc,'')='' then '' else isnull(tb_product.Upc,'') end as UPC, ImageName='',tb_OrderedShoppingCartItems.SKU as SKU,tb_OrderedShoppingCartItems.Quantity,tb_OrderedShoppingCartItems.ProductName as Name,tb_Product.ProductID,tb_Product.ImageName from tb_order inner join tb_OrderedShoppingCartItems on tb_Order.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID inner join tb_product on tb_OrderedShoppingCartItems.RefProductID=tb_Product.ProductID  where tb_Order.OrderNumber=" + txtUPC.Text.ToString().Trim() + " and tb_Order.StoreID=" + StoreID + " and isnull(tb_Order.deleted,0)=0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    gvQCCheck.Visible = false;
                    gvOrderDetails.Visible = true;


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        if(!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Upc"].ToString()))
                        {
                            ds.Tables[0].Rows[i]["upc"] = ds.Tables[0].Rows[i]["UPC"].ToString();
                            ds.Tables[0].AcceptChanges();

                            object image = CommonComponent.GetScalarCommonData("select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=1 and isnull(deleted,0)=0 and  StoreID=1 AND (tb_Product.UPC='" + ds.Tables[0].Rows[i]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + ds.Tables[0].Rows[i]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' ) UNION select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=0 and isnull(deleted,0)=0 and StoreID=1 AND (tb_Product.UPC='" + ds.Tables[0].Rows[i]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + ds.Tables[0].Rows[i]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' )");
                            if (image != null && image != "" )
                            {

                                ds.Tables[0].Rows[i]["ImageName"] = image.ToString();
                                ds.Tables[0].AcceptChanges();
                            }
                        }
                        else
                        {
                            DataSet dsUPC = new DataSet();
                            dsUPC = CommonComponent.GetCommonDataSet("select isnull(UPC,'') as UPC from tb_ProductVariantValue where sku='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' and productId in (SELECT productId FROM tb_Product WHERE Storeid=1 and isnull(Active,0)=1 and isnull(deleted,0)=0) UNION select isnull(UPC,'') as UPC from tb_ProductVariantValue where sku='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' and productId in (SELECT productId FROM tb_Product WHERE Storeid=1 and isnull(Active,0)=0 and isnull(deleted,0)=0)");
                            if (dsUPC != null && dsUPC.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsUPC.Tables[0].Rows[0]["UPC"].ToString()))
                                {
                                    ds.Tables[0].Rows[i]["upc"] = dsUPC.Tables[0].Rows[0]["UPC"].ToString();
                                    ds.Tables[0].AcceptChanges();

                                }
                                object image = CommonComponent.GetScalarCommonData("select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=1 and isnull(deleted,0)=0 and StoreID=1 AND (tb_Product.UPC='" + dsUPC.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + dsUPC.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' ) UNION select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=0 and isnull(deleted,0)=0 and StoreID=1 AND (tb_Product.UPC='" + dsUPC.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + dsUPC.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' )");
                                if (image != null && image != "")
                                {
                                    ds.Tables[0].Rows[i]["ImageName"] = image.ToString();
                                    ds.Tables[0].AcceptChanges();
                                }
                            }
                            else
                            {
                                DataSet dsUPC1 = new DataSet();
                                dsUPC1 = CommonComponent.GetCommonDataSet("select isnull(UPC,'') as UPC from tb_Product where sku='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  and ProductId=" + ds.Tables[0].Rows[i]["ProductID"].ToString() + "");
                                if (dsUPC1 != null && dsUPC1.Tables[0].Rows.Count > 0)
                                {
                                        if (!string.IsNullOrEmpty(dsUPC1.Tables[0].Rows[0]["UPC"].ToString()))
                                        {
                                            ds.Tables[0].Rows[i]["upc"] = dsUPC1.Tables[0].Rows[0]["UPC"].ToString();
                                            ds.Tables[0].AcceptChanges();

                                        }

                                        object image = CommonComponent.GetScalarCommonData("select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=1 and isnull(deleted,0)=0 and StoreID=1 AND (tb_Product.UPC='" + dsUPC1.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + dsUPC1.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' ) UNION select tb_product.ImageName from tb_Product LEFT OUTER JOIN tb_productvariantvalue on tb_Product.ProductId=tb_productvariantvalue.ProductId where isnull(active,0)=0 and isnull(deleted,0)=0 and StoreID=1 AND (tb_Product.UPC='" + dsUPC1.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_Product.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'  or tb_productvariantvalue.UPC='" + dsUPC1.Tables[0].Rows[0]["UPC"].ToString() + "' OR tb_productvariantvalue.SKU='" + ds.Tables[0].Rows[i]["SKU"].ToString() + "' )");
                                  if (image != null && image != "")
                                    {

                                        ds.Tables[0].Rows[i]["ImageName"] = image.ToString();
                                        ds.Tables[0].AcceptChanges();
                                    }

                                }
                            }
                        }
                        

                    }

                    gvOrderDetails.DataSource = ds;
                    gvOrderDetails.DataBind();

                }
            }
            else
            {
                gvQCCheck.Visible = true;
                gvOrderDetails.Visible = false;
                DataSet ds1 = new DataSet();
                ds1 = CommonComponent.GetCommonDataSet("select tb_Product.ProductId,tb_Product.Name,tb_Product.ImageName,tb_ProductVariantValue.SKU,tb_ProductVariantValue.UPC from tb_Product INNER JOIN tb_ProductVariantValue ON tb_Product.ProductID=tb_ProductVariantValue.ProductID where  tb_ProductVariantValue.UPC = '" + txtUPC.Text.ToString().Trim() + "' AND  isnull(tb_Product.Deleted,0)=0 and tb_Product.StoreID=1 ");
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {

                    gvQCCheck.DataSource = ds1;
                    gvQCCheck.DataBind();


                }
                else
                {
                    DataSet ds2 = new DataSet();
                    ds2 = CommonComponent.GetCommonDataSet("select ProductId,Name,ImageName,SKU,UPC from tb_Product where isnull(Deleted,0)=0 and StoreID=1 and UPC = '" + txtUPC.Text + "'");
                    if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                    {

                        gvQCCheck.DataSource = ds2;
                        gvQCCheck.DataBind();

                    }
                    else
                    {
                        gvQCCheck.DataSource = null;
                        gvQCCheck.DataBind();
                    }
                }

            }
               
       }

        /// <summary>
        /// Creates the folder for specified path.
        /// </summary>
        /// <param name="FPath">String FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        protected void gvQCCheck_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string ImagePathProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_AppConfig WHERE Storeid=1 and configname='ImagePathProduct' and isnull(deleted,0)=0"));
            ProductIconPath = string.Concat(ImagePathProduct, "Icon/");
          
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnImageName = (HiddenField)e.Row.FindControl("hdnImageName");
                Image imgName = (Image)e.Row.FindControl("imgName");
              

                if (!string.IsNullOrEmpty(hdnImageName.Value.ToString()) && File.Exists(Server.MapPath(ProductIconPath + hdnImageName.Value.ToString())))
                {
                    imgName.ImageUrl = ProductIconPath + hdnImageName.Value;

                }
                else
                {
                    imgName.ImageUrl = ProductIconPath + "image_not_available.jpg";
                }
                
            }
        }

        protected void gvOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string ImagePathProduct = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Configvalue FROM tb_AppConfig WHERE Storeid=1 and configname='ImagePathProduct' and isnull(deleted,0)=0"));
            ProductIconPath = string.Concat(ImagePathProduct, "Icon/");
           

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnImageName = (HiddenField)e.Row.FindControl("hdnImageName");
                Image imgName = (Image)e.Row.FindControl("imgName");


                if (!string.IsNullOrEmpty(hdnImageName.Value.ToString()) && File.Exists(Server.MapPath(ProductIconPath + hdnImageName.Value.ToString())))
                {
                    imgName.ImageUrl = ProductIconPath + hdnImageName.Value;

                }
                else
                {
                    imgName.ImageUrl = ProductIconPath + "image_not_available.jpg";
                }
                
            }
        }


        }
    }
