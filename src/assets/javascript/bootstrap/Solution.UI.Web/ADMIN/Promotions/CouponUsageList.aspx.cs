using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class CouponUsageList : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        int StoreID;
        CouponComponent couponcomp = new CouponComponent();
        Decimal total = Decimal.Zero;
        Decimal totaltax = decimal.Zero;

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
                if(Request.UrlReferrer != null)
                {
                    abacklink.HRef = Request.UrlReferrer.ToString();
                }
                BindStore();
                if (Request.QueryString["StoreID"] != null)
                {   
                    Int32.TryParse(Request.QueryString["StoreID"], out StoreID);
                }
                if (Request.QueryString["CouponCode"] != null)
                    Binddata(StoreID);
                ddlstore.SelectedValue = StoreID.ToString();
                btnexport1.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
              
            }
        }

        /// <summary>
        /// Binds the Store Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();
        }

        /// <summary>
        /// Binds Gridview according to the Store ID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        public void Binddata(int StoreID)
        {
            object op = CommonComponent.GetScalarCommonData("select count(*) OrderCount  from tb_Order where CouponCode='" + Request.QueryString["CouponCode"].ToString() + "' and isnull(Deleted,0)=0 and StoreID=" + ddlstore.SelectedValue);
            lblCount.Text = op.ToString();
            lttitle.Text = "Coupon Usage for " + Request.QueryString["CouponCode"].ToString();
            DataSet Ds = new DataSet();
            if (StoreID < 1)
            {

                Ds = CommonComponent.GetCommonDataSet("select OrderNumber,isnull(OrderTotal,0) as saleamount,OrderDate as useon,Email from tb_Order where CouponCode='" + Request.QueryString["CouponCode"].ToString() + "' and isnull(Deleted,0)=0");
            }
            else
            {
                Ds = CommonComponent.GetCommonDataSet("select OrderNumber,isnull(OrderTotal,0) as saleamount,OrderDate as useon,Email from tb_Order where CouponCode='" + Request.QueryString["CouponCode"].ToString() + "' and isnull(Deleted,0)=0and StoreID=" + ddlstore.SelectedValue);
  
            }
          
            gridcouponusage.DataSource = Ds;
            gridcouponusage.DataBind();
        }

       

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlstore.SelectedValue.ToString() == "0" || ddlstore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }

            StoreID = Convert.ToInt32(ddlstore.SelectedValue);
            Binddata(StoreID);
        }


     

        protected void gridcouponusage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "saleamount"));
                
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                lblTotal.Text = total.ToString("C").Replace("(", "-").Replace(")", "");

               
            }
        }
        protected void btnexport1_Click(object sender, EventArgs e)
        {
            DataView dvCust = new DataView();
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT tb_Coupons.CouponCode,tb_OrderedShoppingCartItems.SKU,ProductName,Avg(DiscountPrice) as [AvgSalePrice], sum(DiscountPrice * Quantity) TotalPrice,Sum(Quantity) as Totalqty FROM  tb_Coupons  INNER JOIN tb_Order on tb_Order.CouponCode=tb_Coupons.CouponCode INNER JOIN tb_OrderedShoppingCartItems on tb_OrderedShoppingCartItems.OrderedShoppingCartID=tb_Order.ShoppingCardID WHERE isnull(tb_Order.CouponCode,'')<>'' and isnull(tb_OrderedShoppingCartItems.DiscountPrice,0) > 0 and  tb_Order.StoreID=1 and isnull(tb_Order.Deleted,0)=0 and tb_Coupons.CouponCode='" + Request.QueryString["CouponCode"].ToString() + "'  GROUP BY tb_Coupons.CouponCode,tb_OrderedShoppingCartItems.SKU,ProductName ORDER by tb_Coupons.CouponCode");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dvCust = ds.Tables[0].DefaultView;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (dvCust != null)
                {
                    for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                    {
                        object[] args = new object[6];
                        args[0] = Convert.ToString(dvCust.Table.Rows[i]["CouponCode"]);
                        args[1] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
                        args[2] = Convert.ToString(dvCust.Table.Rows[i]["ProductName"]);
                        args[3] = Convert.ToString(dvCust.Table.Rows[i]["AvgSalePrice"]);
                       
                        args[4] = Convert.ToString(dvCust.Table.Rows[i]["Totalqty"]);
                        //args[5] = Convert.ToString(dvCust.Table.Rows[i]["TotalPrice"]);
                        Decimal ddd = Convert.ToDecimal(dvCust.Table.Rows[i]["AvgSalePrice"]) * Convert.ToDecimal(dvCust.Table.Rows[i]["Totalqty"]);
                        args[5] = string.Format("{0:0.00}", ddd);
 
                        
                       

                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "CouponList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("CouponCode,Item No.,SKU Description,Avg Sales Price,Total Sold (Qty.),Total Price");
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
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }
        }
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

    }
}