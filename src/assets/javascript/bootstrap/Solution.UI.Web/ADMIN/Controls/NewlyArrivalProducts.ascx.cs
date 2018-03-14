using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Controls
{
    public partial class NewlyArrivalProducts : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillNewArrivalProduct();
        }
        private void FillNewArrivalProduct()
        {
            DataSet dsProduct = new DataSet();
            dsProduct = DashboardComponent.GetNewArrivalProduct();
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                grdNewArrivalProducts.DataSource = dsProduct.Tables[0];
                grdNewArrivalProducts.DataBind();
                tdviewReport.Visible = true;
                if (Convert.ToInt32(Solution.Bussines.Components.DashboardComponent.ControlStoreID) == 0)
                {
                    ltrAddProduct.Text = "<a title=\"Add Product\" href=\"/Admin/Products/Product.aspx?StoreID=1\"style=\"cursor: pointer;\"><span>Add Product</span></a>";
                    ltrProductList.Text = "<a title=\"Product List\" href=\"/Admin/Products/ProductList.aspx?StoreID=1\"style=\"cursor: pointer;\"><span>Product List</span></a>";
                }
                else
                {
                    ltrAddProduct.Text = "<a title=\"Add Product\" href=\"/Admin/Products/Product.aspx?StoreID=" + Solution.Bussines.Components.DashboardComponent.ControlStoreID.ToString() + "\"style=\"cursor: pointer;\"><span>Add Product</span></a>";
                    ltrProductList.Text = "<a title=\"Product List\" href=\"/Admin/Products/ProductList.aspx?StoreID=" + Solution.Bussines.Components.DashboardComponent.ControlStoreID.ToString() + "\"style=\"cursor: pointer;\"><span>Product List</span></a>";
                }
            }
            else
            {
                grdNewArrivalProducts.DataSource = null;
                grdNewArrivalProducts.DataBind();
                tdviewReport.Visible = false;
            }
        }
        protected void grdNewArrivalProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    //System.Web.UI.HtmlControls.HtmlInputHidden hdnStoreIId = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnStoreIId");
                    //System.Web.UI.HtmlControls.HtmlAnchor hlink = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("hlink");
                    //if (hdnStoreIId != null && hdnStoreIId.Value.ToString() == "1")
                    //{
                    //    //hlink.HRef = hlink.HRef.Replace("Product.aspx", "");
                    //}
                    //else if (hdnStoreIId != null && hdnStoreIId.Value.ToString() == "2")
                    //{
                    //    hlink.HRef = hlink.HRef.ToString().Replace("Product.aspx", "ProductAmazon.aspx");
                    //}
                    //else if (hdnStoreIId != null && (hdnStoreIId.Value.ToString() == "3" || hdnStoreIId.Value.ToString() == "4" || hdnStoreIId.Value.ToString() == "5" || hdnStoreIId.Value.ToString() == "6" || hdnStoreIId.Value.ToString() == "7"))
                    //{
                    //    hlink.HRef = hlink.HRef.ToString().Replace("Product.aspx", "ProductYahoo.aspx");
                    //}
                    //else if (hdnStoreIId != null && hdnStoreIId.Value.ToString() == "8")
                    //{
                    //    hlink.HRef = hlink.HRef.ToString().Replace("Product.aspx", "ProductSears.aspx");
                    //}
                    //else if (hdnStoreIId != null && hdnStoreIId.Value.ToString() == "9")
                    //{
                    //    hlink.HRef = hlink.HRef.ToString().Replace("Product.aspx", "ProductBuy.aspx");
                    //}
                }
                catch
                {
                }

            }
        }
    }
}