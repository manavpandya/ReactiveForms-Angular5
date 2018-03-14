using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Data;
using Solution.Bussines.Components;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.Common;
using System.IO;


namespace Solution.UI.Web
{
    public partial class comparetable : System.Web.UI.Page
    {
        string Comparestr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["pid"] != null && Request.QueryString["cid"] != null)
            {
                if (Request.QueryString["cid"].ToString() == "1")
                {
                    AddProductData(Request.QueryString["pid"].ToString());
                }
                else if (Request.QueryString["cid"].ToString() == "-1" && Request.QueryString["pid"].ToString() == "-1")
                { 
                    Session["CmpProductID"] = null;
                }
                else
                {
                    DeleteProductData(Request.QueryString["pid"].ToString());
                }
                if (Session["CmpProductID"] != null)
                {
                    Getdata(Session["CmpProductID"].ToString());
                    GetCompareProduct();
                }
            }
        }
        private void DeleteProductData(string ProIDs)
        {
            if (Session["CmpProductID"] != null)
            {
                if (Session["CmpProductID"].ToString().Contains(ProIDs.ToString().ToString() + ","))
                {
                    string newvar = "";
                    string proid = Session["CmpProductID"].ToString().TrimEnd(',');
                    string[] arr = proid.Split(',');
                    if (ProIDs != "")
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i] != ProIDs.ToString())
                            {
                                newvar += arr[i].ToString() + ",";
                            }
                        }
                        Session["CmpProductID"] = "";
                        Session["CmpProductID"] = newvar;

                    }
                }
            }
        }
        private void AddProductData(string ProIDs)
        {
            if (Session["CmpProductID"] != null)
            {
                if (!Session["CmpProductID"].ToString().Contains(ProIDs.ToString().ToString() + ","))
                {
                    Session["CmpProductID"] += ProIDs.ToString() + ",";
                }
            }
            else
            {
                Session["CmpProductID"] += ProIDs.ToString() + ",";
            }
        }
        public void Getdata(string ProIDs)
        {
            string proid = ProIDs.TrimEnd(',');
            string[] arr = proid.Split(',');
            DataSet dsproduct = new DataSet();
            if (ProIDs != "")
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].ToString() != "")
                    {
                        dsproduct = ProductComponent.GetProductDetailByID(Convert.ToInt32(arr[i].ToString()));
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {
                            Comparestr += @"<div id='c-" + (i + 1).ToString() + "' class='comparison-box'>";
                            Comparestr += "<div class='comparison-img'>";
                            Comparestr += "<img width='50' height='50' title='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' alt='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' src='" + GetIconImageProduct(dsproduct.Tables[0].Rows[0]["ImageName"].ToString()) + "'></div>";
                            Comparestr += "<div class='comparison-right'><h4>" + SetNameForCompare(dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "</h4></div>";
                            Comparestr += "<div style='float:left; position:absolute; bottom:3px; right:3px; cursor:pointer;' onclick='DeleteProductData(" + (i + 1).ToString() + "," + arr[i].ToString() + ");' id='delete_compare'>";
                            Comparestr += "<img width='16' height='16' src='/images/delete_ico.png'>";
                            Comparestr += "</div>";
                            Comparestr += "</div>";
                        }
                    }
                }
                if (arr.Length >= 1)
                {
                    if (arr.Length > 1)
                    {
                        btncompare.Visible = true;
                    }
                    else
                    {
                        btncompare.Visible = false;
                    }
                    comare_div.Visible = true;
                }
                else
                {
                    comare_div.Visible = false;
                }
            }
            else
            {
                comare_div.Visible = false;
            }
        }


        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }
        public String SetName(String Name)
        {
            if (Name.Length > 35)
                Name = Name.Substring(0, 32) + "...";
            return Server.HtmlEncode(Name);
        }

        public String SetNameForCompare(String Name)
        {
            if (Name.Length > 45)
                Name = Name.Substring(0, 42) + "...";
            return Server.HtmlEncode(Name);
        }
        public void GetCompareProduct()
        {
            if (!string.IsNullOrEmpty(Comparestr))
            {
                ltrProduct.Text = Comparestr;
            }
            else
            {
                ltrProduct.Text = "";
            }
        }
    }
}