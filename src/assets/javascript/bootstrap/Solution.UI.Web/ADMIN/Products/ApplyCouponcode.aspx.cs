using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ApplyCouponcode : BasePage
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

                btnApply.ImageUrl = "/App_Themes/" + Page.Theme + "/images/apply.png";
                bindcoupon();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindcoupon()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("select couponcode,couponid from tb_Coupons where storeid=1 and isnull(deleted,0)=0 and  cast(ExpirationDate as date)>=cast(GETDATE() as date)");

            if (ds != null && ds.Tables[0].Rows.Count>0 && ds.Tables.Count>0)
            {
                ddlCouponCode.DataSource = ds;
                ddlCouponCode.DataTextField = "couponcode";
                ddlCouponCode.DataValueField = "couponid";
                ddlCouponCode.DataBind();
            }
            ddlCouponCode.Items.Insert(0, "Select Coupon Code");
            ddlCouponCode.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Product Data
        /// </summary>
        public void GetData()
        {
            DataSet ds = new DataSet();
            if(rdobtnNewarrival.Checked==true)
            {
//                ds = CommonComponent.GetCommonDataSet(@"select productid,isnull(Name,'') as Name,isnull(sku,'') as sku,isnull(UPC,'') as UPC from tb_Product where StoreID=1 and  
//isnull(Active,0) = 1 and isnull(Deleted,0) = 0 and isnull(IsNewArrival,0)=1 and cast(IsNewArrivalFromDate as date)<=cast(GETDATE() as date) 
//and cast(IsNewArrivalFromDate as date)>=cast(GETDATE() as date)  and  isnull(Isfreefabricswatch,0) <> 1 and productid 
//not in(select tb_Giftcardproduct.productid from tb_Giftcardproduct) ");
                
                ds = CommonComponent.GetCommonDataSet("Exec usp_Product_GetBuy1Get1NewArrivalProducts 2");
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    trtop.Visible = true;
                    grdProductDetail.DataSource = ds;
                    grdProductDetail.DataBind();
                  
                }
                else
                {
                    trtop.Visible = false;
                    grdProductDetail.DataSource = null;
                    grdProductDetail.DataBind();
                }
            }
            else  if (rdobtnbuyonegetone.Checked == true)
            {
//                ds = CommonComponent.GetCommonDataSet(@"select p.ProductID,p.Name as Name,p.UPC as UPC,p.SKU as SKU  from tb_Product p 
// where p.StoreID=1 and isnull(Active,0) = 1 and isnull(Deleted,0) = 0 and isnull(Isfreefabricswatch,0) <> 1 and p.productid in 
//(Select tb_product.Productid from tb_ProductVariantValue INNER JOIN tb_product ON tb_product.ProductId= tb_ProductVariantValue.productId
//Where isnull(tb_ProductVariantValue.VarActive,0)=1 AND isnull(tb_product.Active,0)=1 AND  isnull(tb_product.deleted,0)=0 AND isnull(tb_product.storeid,0)
//=1 AND (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1
//) order by p.DisplayOrder ");

                ds = CommonComponent.GetCommonDataSet("Exec usp_Product_GetBuy1Get1NewArrivalProducts 1");

                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {
                    trtop.Visible = true;
                    grdProductDetail.DataSource = ds;
                    grdProductDetail.DataBind();
                    
                }
                else
                {
                    trtop.Visible = false;
                    grdProductDetail.DataSource = null;
                    grdProductDetail.DataBind();
                }
            }
            else
            {
                trtop.Visible = false;
                grdProductDetail.DataSource = null;
                grdProductDetail.DataBind();
            }
        }

        /// <summary>
        /// Radio Button Selected Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdobtnNewarrival_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// Radio Button Selected Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdobtnbuyonegetone_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// Button Apply Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnApply_Click(object sender, ImageClickEventArgs e)
        {
            string productid="";

           
            int totalRowCount = grdProductDetail.Rows.Count;
           
           
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdProductDetail.Rows[i].FindControl("hdnproductid");
                CheckBox chk = (CheckBox)grdProductDetail.Rows[i].FindControl("chkselect");
                if (chk.Checked == true)
                {
                        productid += hdn.Value.ToString() + ",";
               
                  
                }
            }

           
            if (!String.IsNullOrEmpty(productid))
            {
                if (ddlCouponCode.SelectedIndex != 0)
                {
                         CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + productid.ToString().Substring(0,productid.Length-1) + "' where couponid='" + ddlCouponCode.SelectedValue.ToString() + "' ");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg2", "jAlert('Coupon Code Successfully Applied.', 'Message');", true);
                        return;
             
                }
               
               
            }
           
        }

        //protected void btnApply_Click(object sender, ImageClickEventArgs e)
        //{
        //    string productid = ",";
           
        //    string existproductid = "";
        //    int totalRowCount = grdProductDetail.Rows.Count;
        //     existproductid = Convert.ToString(CommonComponent.GetScalarCommonData("select ValidforProduct from tb_Coupons where couponid=" + ddlCouponCode.SelectedValue + ""));
        //    if (!string.IsNullOrEmpty(existproductid))
        //    existproductid =","+existproductid +",";
        //    Int32 Flag = 0;
        //    for (int i = 0; i < totalRowCount; i++)
        //    {
        //        HiddenField hdn = (HiddenField)grdProductDetail.Rows[i].FindControl("hdnproductid");
        //        CheckBox chk = (CheckBox)grdProductDetail.Rows[i].FindControl("chkselect");
        //        if (chk.Checked == true)
        //        {
                


        //                if (existproductid.ToString().IndexOf("," + hdn.Value.ToString() + ",") <= -1 && productid.ToString().ToLower().IndexOf("," + hdn.Value.ToString() + ",") <= -1)
        //                {
        //                    productid += hdn.Value + ",";
        //                }
                  
        //            Flag = 1;
        //        }
        //    }

        //    if(Flag==0)
        //    {
                
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Atleast One Product..', 'Message');});", true);
        //        return;
        //    }
        //    if (!String.IsNullOrEmpty(productid) && productid.ToString().Length>2)
        //    {
        //        if (ddlCouponCode.SelectedIndex!=0)
        //        {
        //            if (!String.IsNullOrEmpty(existproductid))
        //            {
        //                CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct=ValidforProduct+'" + productid.ToString().Remove(productid.ToString().LastIndexOf(",")) + "' where couponid='" + ddlCouponCode.SelectedValue + "' ");
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg2", "jAlert('All Products have assigned coupon code..', 'Message');", true);
        //                return;
        //            }
        //            else
        //            {
        //                productid = productid.ToString().Substring(1, productid.Length - 1);
        //                CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + productid.ToString().Remove(productid.ToString().LastIndexOf(",")) + "' where couponid='" + ddlCouponCode.SelectedValue + "' ");
        //                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg2", "jAlert('All Products have assigned coupon code..', 'Message');", true);
        //                return;
        //            }
                        
                  
        //        }
        //        else
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select Coupon Code...', 'Message');});", true);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg2", "jAlert('All Products have already assigned coupon code..', 'Message');", true);
        //        return;
        //    }
        //}
        
    }
}