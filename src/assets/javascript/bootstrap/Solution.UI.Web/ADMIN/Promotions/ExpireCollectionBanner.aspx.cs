using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class ExpireCollectionBanner : BasePage
    {
        public int isadded = 0;
        public int isupdated = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["ImageCollectionExbannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathCollectionBanner' AND storeid=1 AND isnull(Deleted,0)=0");
                ViewState["ImageCollectionLogobannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathCollectionLogo' AND storeid=1 AND isnull(Deleted,0)=0");
                txtEventName.Focus();
                bindcoupon();
                // GetData();
                if (!string.IsNullOrEmpty(Request.QueryString["CollectionID"]) && Convert.ToString(Request.QueryString["CollectionID"]) != "0")
                {
                    FillEventlist(Convert.ToInt32(Request.QueryString["CollectionID"]));
                    lblTitle.Text = "Edit Collection Banner";
                    lblTitle.ToolTip = "Edit Collection Banner";
                }

            }
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
            btndelete.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif";
            btndeletlogo.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif";
            for (int i = 0; i < chkCouponCode.Items.Count; i++)
            {
                chkCouponCode.Items[i].Attributes.Add("onclick", "MutExChkList(this)");
            }
            // btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            //btnShowAll.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
            btnpro.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-product.png";
        }


        private void FillEventlist(Int32 eid)
        {
            hdnevent.Value = eid.ToString();

            DataSet dsevents = new DataSet();
            dsevents = CommonComponent.GetCommonDataSet("select * from tb_Collection where CollectionId=" + eid + "");
            if (dsevents != null && dsevents.Tables.Count > 0 && dsevents.Tables[0].Rows.Count > 0)
            {
                txtEventName.Text = dsevents.Tables[0].Rows[0]["name"].ToString();
                //string startdate = String.Format("{0:d}", Convert.ToDateTime(dsevents.Tables[0].Rows[0]["startdate"].ToString()));
                //txtStartDate.Text = startdate;
                //string enddate = String.Format("{0:d}", Convert.ToDateTime(dsevents.Tables[0].Rows[0]["enddate"].ToString()));
                //txtEndDate.Text = enddate;
                TxtbannerTitle.Text = dsevents.Tables[0].Rows[0]["Title"].ToString();
                TxtBannerURL.Text = dsevents.Tables[0].Rows[0]["BannerURL"].ToString();
                ddlTarget.Text = Convert.ToString(dsevents.Tables[0].Rows[0]["LinkTarget"].ToString());
                txtUrlname.Text = Convert.ToString(dsevents.Tables[0].Rows[0]["Urlname"].ToString());
                string filename = Convert.ToString(dsevents.Tables[0].Rows[0]["BannerName"].ToString());
                string logofilename = Convert.ToString(dsevents.Tables[0].Rows[0]["Bannerlogo"].ToString());
                string Position = Convert.ToString(dsevents.Tables[0].Rows[0]["Position"].ToString());
                SalesEventDescription.Text = Server.HtmlDecode(dsevents.Tables[0].Rows[0]["Description"].ToString());
                txtpagetitle.Text = dsevents.Tables[0].Rows[0]["PageTitle"].ToString();
                txtSETitle.Text = dsevents.Tables[0].Rows[0]["SEOTitle"].ToString();
                txtSEDescription.Text = dsevents.Tables[0].Rows[0]["MetaDescription"].ToString();
                txtSEKeyword.Text = dsevents.Tables[0].Rows[0]["MetaKeyword"].ToString();
                if (!string.IsNullOrEmpty(dsevents.Tables[0].Rows[0]["IsActive"].ToString()) && Convert.ToBoolean(dsevents.Tables[0].Rows[0]["IsActive"].ToString()))
                {
                    chkactive.Checked = true;
                }
                try
                {
                    ddldescposition.SelectedValue = dsevents.Tables[0].Rows[0]["DescriptionPosition"].ToString();
                }
                catch { }

                if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString()));
                }
                if (File.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + filename.ToString())))
                {
                    Random rd = new Random();
                    imgBanner.Src = ViewState["ImageCollectionExbannrpath"].ToString() + filename.ToString() + "?" + rd.Next(10000).ToString();
                    btndelete.Visible = true;

                }

                if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString()));
                }
                if (!String.IsNullOrEmpty(logofilename) && File.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + logofilename.ToString())))
                {
                    Random rd = new Random();
                    imglogo.Src = ViewState["ImageCollectionLogobannrpath"].ToString() + logofilename.ToString() + "?" + rd.Next(10000).ToString();
                    btndeletlogo.Visible = true;

                }
                try
                {
                    if (!string.IsNullOrEmpty(Position))
                    {
                        ddlposition.SelectedValue = Position;
                    }
                }
                catch { }
                string couponcode = Convert.ToString(dsevents.Tables[0].Rows[0]["CouponCode"].ToString());
                if (!String.IsNullOrEmpty(couponcode))
                {
                    if (chkCouponCode.Items.Count > 0)
                    {
                        for (int y = 0; y < chkCouponCode.Items.Count; y++)
                        {
                            if (chkCouponCode.Items[y].Value.ToString().Trim() == couponcode.ToString().Trim())
                            {
                                chkCouponCode.Items[y].Selected = true;
                            }
                        }
                    }

                }
                string validproducts = "";
                validproducts = Convert.ToString(dsevents.Tables[0].Rows[0]["Productids"].ToString());
                txtvalidforprod.Text = validproducts;
                //HiddenField hdnProductid = null;
                //if (!String.IsNullOrEmpty(validproducts))
                //{
                //    String[] pid = validproducts.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //    if (pid.Length > 0)
                //    {

                //        if (grdProductDetail.Rows.Count > 0)
                //        {
                //            for (int k = 0; k < pid.Length; k++)
                //            {
                //                for (int i = 0; i < grdProductDetail.Rows.Count; i++)
                //                {
                //                    CheckBox chkselect = (CheckBox)grdProductDetail.Rows[i].FindControl("chkselect");

                //                    hdnProductid = (HiddenField)grdProductDetail.Rows[i].FindControl("hdnProductid");

                //                    if (pid[k].ToString().Trim() == hdnProductid.Value.ToString())
                //                    {
                //                        chkselect.Checked = true;

                //                    }





                //                }

                //            }


                //        }

                //    }

                //}

            }

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindcoupon()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("select couponcode,couponid from tb_Coupons where storeid=1 and isnull(deleted,0)=0 and  cast(ExpirationDate as date)>=cast(GETDATE() as date) order by couponcode asc");

            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    chkCouponCode.Items.Add(new ListItem(ds.Tables[0].Rows[i]["couponcode"].ToString(), ds.Tables[0].Rows[i]["couponid"].ToString()));

            }


        }

        /// <summary>
        /// Get Product Data
        /// </summary>
        public void GetData()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("select productid,isnull(Name,'') as Name,isnull(sku,'') as sku,isnull(UPC,'') as UPC from tb_Product where StoreID=1 and  isnull(Active,0) = 1 and isnull(Deleted,0) = 0 ");
            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
            {

                grdProductDetail.DataSource = ds;
                grdProductDetail.DataBind();

            }
            else
            {

                grdProductDetail.DataSource = null;
                grdProductDetail.DataBind();
            }
        }
        protected void btndelete_Click(object sender, ImageClickEventArgs e)
        {
            string strname = "";
            if (!string.IsNullOrEmpty(imgBanner.Src.ToString()) && imgBanner.Src.ToString().IndexOf("?") > -1)
            {
                strname = imgBanner.Src.ToString().Substring(0, imgBanner.Src.ToString().IndexOf("?"));
            }
            else
            {
                strname = imgBanner.Src.ToString();
            }

            if (File.Exists(Server.MapPath(strname.ToString())))
            {
                File.Delete(Server.MapPath(strname.ToString()));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(strname.ToString()));
                imgBanner.Src = "";
                btndelete.Visible = false;
            }



        }
        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String value after Remove Special Character</returns>
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);
            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            res = value;
            return res;
        }
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            //if (Convert.ToDateTime(txtEndDate.Text.ToString()) <= DateTime.Today)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Expiration Date  Must Be greater Then Today...', 'Message');});", true);
            //    return;
            //}


            if (fileuploadlogo.HasFile)
            {
                bool checksize = CheckImageSize(fileuploadlogo, 280, 100);
                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('Please upload Logo of Size width <=280 x height<=100','Message')", true);
                    return;
                }
            }
            if (txtUrlname.Text.ToString() == "")
            {
                txtUrlname.Text = RemoveSpecialCharacter(txtEventName.Text.ToString().ToCharArray());
            }
            Int32 isactive = 0;
            if (chkactive.Checked)
            {
                isactive = 1;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["CollectionId"]) && Convert.ToString(Request.QueryString["CollectionId"]) != "0")
            {
                isadded = 0;
                isupdated = 1;
                if (!String.IsNullOrEmpty(chkCouponCode.Text))
                {
                    CommonComponent.ExecuteCommonData("update tb_Collection set isactive=" + isactive + ", Urlname='" + txtUrlname.Text.ToString().Replace("'", "''") + "', Name='" + txtEventName.Text.ToString().Replace("'", "''") + "' ,CouponCode='" + chkCouponCode.Text.ToString() + "',Title='" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget.SelectedValue.ToString() + "',Description='" + SalesEventDescription.Text.Trim().Replace("'", "''") + "',PageTitle='" + txtpagetitle.Text.Trim().Replace("'", "''") + "',SEOTitle='" + txtSETitle.Text.Trim().Replace("'", "''") + "',MetaDescription='" + txtSEDescription.Text.Trim().Replace("'", "''") + "',MetaKeyword='" + txtSEKeyword.Text.Trim().Replace("'", "''") + "',DescriptionPosition='" + ddldescposition.SelectedValue.ToString() + "' where CollectionId=" + Convert.ToString(Request.QueryString["CollectionId"]) + "");

                }
                else
                {
                    CommonComponent.ExecuteCommonData("update tb_Collection set isactive=" + isactive + ",Urlname='" + txtUrlname.Text.ToString().Replace("'", "''") + "', Name='" + txtEventName.Text.ToString().Replace("'", "''") + "',CouponCode='',Title='" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget.SelectedValue.ToString() + "',Description='" + SalesEventDescription.Text.Trim().Replace("'", "''") + "',PageTitle='" + txtpagetitle.Text.Trim().Replace("'", "''") + "',SEOTitle='" + txtSETitle.Text.Trim().Replace("'", "''") + "',MetaDescription='" + txtSEDescription.Text.Trim().Replace("'", "''") + "',MetaKeyword='" + txtSEKeyword.Text.Trim().Replace("'", "''") + "',DescriptionPosition='" + ddldescposition.SelectedValue.ToString() + "' where CollectionId=" + Convert.ToString(Request.QueryString["CollectionId"]) + "");
                }

                insertproduct(Convert.ToInt32(Convert.ToString(Request.QueryString["CollectionId"])));
            }
            else
            {

                isadded = 1;
                isupdated = 0;

                Int32 EventID = 0;
                if (!String.IsNullOrEmpty(chkCouponCode.Text))
                {
                    EventID = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_Collection (isactive,Name,CouponCode,Title,BannerURL,LinkTarget,Urlname,Description,PageTitle,SEOTitle,MetaDescription,MetaKeyword,DescriptionPosition) values(" + isactive + ",'" + txtEventName.Text.ToString() + "','" + chkCouponCode.Text.ToString() + "','" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "','" + TxtBannerURL.Text.ToString().Replace("'", "''") + "','" + ddlTarget.SelectedValue.ToString() + "','" + txtUrlname.Text.ToString().Replace("'", "''") + "','" + SalesEventDescription.Text.Trim().Replace("'", "''") + "','" + txtpagetitle.Text.Trim().Replace("'", "''") + "','" + txtSETitle.Text.Trim().Replace("'", "''") + "','" + txtSEDescription.Text.Trim().Replace("'", "''") + "','" + txtSEKeyword.Text.Trim().Replace("'", "''") + "','" + ddldescposition.SelectedValue.ToString() + "')SELECT SCOPE_IDENTITY();"));

                }
                else
                {
                    EventID = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_Collection (isactive,Name,CouponCode,Title,BannerURL,LinkTarget,Urlname,Description,PageTitle,SEOTitle,MetaDescription,MetaKeyword,DescriptionPosition) values(" + isactive + ",'" + txtEventName.Text.ToString() + "','','" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "','" + TxtBannerURL.Text.ToString().Replace("'", "''") + "','" + ddlTarget.SelectedValue.ToString() + "','" + txtUrlname.Text.ToString().Replace("'", "''") + "','" + SalesEventDescription.Text.Trim().Replace("'", "''") + "','" + txtpagetitle.Text.Trim().Replace("'", "''") + "','" + txtSETitle.Text.Trim().Replace("'", "''") + "','" + txtSEDescription.Text.Trim().Replace("'", "''") + "','" + txtSEKeyword.Text.Trim().Replace("'", "''") + "','" + ddldescposition.SelectedValue.ToString() + "')SELECT SCOPE_IDENTITY();"));
                }

                if (EventID > 0)
                {
                    string strEventUrl = string.Empty;
                    strEventUrl = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Urlname,'') as Urlname from tb_Collection where CollectionId=" + EventID + ""));
                    if (!string.IsNullOrEmpty(strEventUrl))
                        CommonComponent.GetScalarCommonData("insert into tb_PageRedirect(StoreID,OldUrl,NewUrl) values(1,'/collection.aspx?url=" + strEventUrl.ToString().Replace("'", "''") + "','dynamicpages/collection?url=" + strEventUrl.ToString().Replace("'", "''") + "')");
                }

                insertproduct(EventID);
            }

        }

        private Boolean CheckImageSize(FileUpload ff, int width, int height)
        {



            try
            {
                if (!Directory.Exists(Server.MapPath("/Resources/temp")))
                {
                    Directory.CreateDirectory(Server.MapPath("/Resources/temp"));

                }
                FileInfo file = new FileInfo(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));
                //if (File.Exists(file))
                //{
                //    File.Delete(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                //}
                if (file.Exists)
                {
                    file.Delete();
                }

                ff.SaveAs(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                if (img.Width <= width && img.Height <= height)
                {
                    img.Dispose();
                    return true;

                }
                else
                {
                    img.Dispose();
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }
        public void insertproduct(Int32 eventid)
        {
            string pids = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            HiddenField hdnProductid = null;
            //for (int i = 0; i < grdProductDetail.Rows.Count; i++)
            //{
            //    CheckBox chkselect = (CheckBox)grdProductDetail.Rows[i].FindControl("chkselect");
            //    if (chkselect.Checked == true)
            //    {
            //        hdnProductid = (HiddenField)grdProductDetail.Rows[i].FindControl("hdnProductid");

            //        pids = hdnProductid.Value.ToString();
            //        sb.Append(pids + ", ");
            //    }



            //}
            try
            {


                if (FileUploadExpireEvent.HasFile)
                {


                    FileInfo fl = new FileInfo(FileUploadExpireEvent.FileName.ToString());

                    if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp")))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp"));
                    }
                    if (File.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp/" + fl.Name.ToString())))
                    {
                        File.Delete(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp/" + fl.Name.ToString()));
                    }
                    FileUploadExpireEvent.SaveAs(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp/" + fl.Name.ToString()));
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/temp/" + fl.Name.ToString()));
                    if (img.Width == 1590 && img.Height == 310)
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "imagesize", "alert('Banner Size should be 1590 X 310.');", true);
                        return;
                    }


                }
            }
            catch
            {

            }




            //int length = sb.ToString().Length;
            //if (length > 0)
            //{
            //    pids = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
            //}
            CommonComponent.ExecuteCommonData("update tb_Collection set Productids='" + txtvalidforprod.Text.ToString().Trim() + "',Position='" + ddlposition.SelectedValue.ToString() + "' where CollectionId=" + eventid + "");

            //if(!String.IsNullOrEmpty(txtvalidforprod.Text.ToString()))
            //{
            string ProIDS = ",";

            DataSet dspids = new DataSet();
            dspids = CommonComponent.GetCommonDataSet("select isnull(Productids,'') as Productids from tb_Collection where couponcode='" + chkCouponCode.SelectedValue.ToString().Trim() + "' and isnull(Productids,'')<>''");
            if (dspids != null && dspids.Tables.Count > 0 && dspids.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dspids.Tables[0].Rows.Count; i++)
                {
                    string[] aa = dspids.Tables[0].Rows[i][0].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (aa.Length > 0)
                    {
                        for (int k = 0; k < aa.Length; k++)
                        {
                            if (ProIDS.ToString().ToLower().IndexOf("," + aa[k].ToString().Trim().ToLower() + ",") <= -1)
                            {
                                ProIDS = ProIDS + aa[k].ToString().Trim().ToLower() + ",";
                            }
                        }

                    }
                }


                if (!String.IsNullOrEmpty(ProIDS) && ProIDS.Length > 2)
                {
                    ProIDS = ProIDS.ToString().Remove(ProIDS.ToString().LastIndexOf(","));
                    ProIDS = ProIDS.ToString().Substring(1, ProIDS.ToString().Length - 1);
                    CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + ProIDS.ToString().Trim() + "' where couponid=" + chkCouponCode.SelectedValue.ToString());
                }


            }
            else
            {
                CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + txtvalidforprod.Text.ToString().Trim() + "' where couponid=" + chkCouponCode.SelectedValue.ToString());

            }
            // }
            //else
            //{
            //    CommonComponent.ExecuteCommonData("update tb_Coupons set ValidforProduct='" + txtvalidforprod.Text.ToString().Trim() + "' where couponid=" + chkCouponCode.SelectedValue.ToString());
            //}




            if (FileUploadExpireEvent.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadExpireEvent.FileName.ToString());
                filename = eventid.ToString() + "" + fl.Extension.ToString();
                if (File.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename)))
                {
                    if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString())))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString()));
                    }
                    if (File.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename)))
                    {
                        File.Delete(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                        FileUploadExpireEvent.SaveAs(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                        try
                        {
                            Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                    }
                }
                else
                {
                    if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString())))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString()));
                    }
                    FileUploadExpireEvent.SaveAs(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                    try
                    {
                        Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                        objcompress.compressimage(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImageCollectionExbannrpath"].ToString() + "/" + filename));
                }
                CommonComponent.ExecuteCommonData("update tb_Collection set BannerName='" + filename.ToString() + "' where CollectionId=" + eventid + "");
            }



            //////logo


            if (fileuploadlogo.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(fileuploadlogo.FileName.ToString());
                filename = eventid.ToString() + "" + fl.Extension.ToString();
                if (File.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename)))
                {
                    if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString())))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString()));
                    }
                    if (File.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename)))
                    {
                        File.Delete(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                        fileuploadlogo.SaveAs(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                        try
                        {
                            Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                    }
                }
                else
                {
                    if (!Directory.Exists(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString())))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString()));
                    }
                    fileuploadlogo.SaveAs(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                    try
                    {
                        Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                        objcompress.compressimage(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImageCollectionLogobannrpath"].ToString() + "/" + filename));
                }
                CommonComponent.ExecuteCommonData("update tb_Collection set Bannerlogo='" + filename.ToString() + "' where CollectionId=" + eventid + "");
            }


            // Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('Event Added successfully.','Message')", true);

            if (isadded > 0)
            {
                if (Request.QueryString["txtse"] != null && Request.QueryString["st"] != null)
                {
                    Response.Redirect("CollectionList.aspx?status=inserted&st=" + Request.QueryString["st"].ToString() + "&txtse=" + Server.UrlEncode(Request.QueryString["txtse"].ToString()) + "");

                }
                else
                {
                    Response.Redirect("CollectionList.aspx?status=inserted");
                }

            }

            if (isupdated > 0)
            {
                if (Request.QueryString["txtse"] != null && Request.QueryString["st"] != null)
                {
                    Response.Redirect("CollectionList.aspx?status=updated&st=" + Request.QueryString["st"].ToString() + "&txtse=" + Server.UrlEncode(Request.QueryString["txtse"].ToString()) + "");

                }
                else
                {
                    Response.Redirect("CollectionList.aspx?status=updated");
                }

            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtsearch.Text))
            {
                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet("select productid,isnull(Name,'') as Name,isnull(sku,'') as sku,isnull(UPC,'') as UPC from tb_Product where StoreID=1 and  isnull(Active,0) = 1 and isnull(Deleted,0) = 0 and sku like '" + txtsearch.Text.ToString().Replace("'", "''") + "%' ");
                if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                {

                    grdProductDetail.DataSource = ds;
                    grdProductDetail.DataBind();

                }
                else
                {

                    grdProductDetail.DataSource = null;
                    grdProductDetail.DataBind();
                }
            }

        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            GetData();
        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {

            if (Request.QueryString["txtse"] != null && Request.QueryString["st"] != null)
            {
                Response.Redirect("CollectionList.aspx?st=" + Request.QueryString["st"].ToString() + "&txtse=" + Server.UrlEncode(Request.QueryString["txtse"].ToString()) + "");

            }
            else
            {
                Response.Redirect("CollectionList.aspx");
            }

        }

        protected void btndeletlogo_Click(object sender, ImageClickEventArgs e)
        {
            string strname = "";
            if (!string.IsNullOrEmpty(imglogo.Src.ToString()) && imglogo.Src.ToString().IndexOf("?") > -1)
            {
                strname = imglogo.Src.ToString().Substring(0, imglogo.Src.ToString().IndexOf("?"));
            }
            else
            {
                strname = imglogo.Src.ToString();
            }

            if (File.Exists(Server.MapPath(strname.ToString())))
            {
                File.Delete(Server.MapPath(strname.ToString()));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(strname.ToString()));
                imglogo.Src = "";
                btndeletlogo.Visible = false;
                if (Request.QueryString["CollectionId"] != null && !String.IsNullOrEmpty(Request.QueryString["CollectionId"].ToString()))
                {
                    CommonComponent.ExecuteCommonData("update tb_Collection set Bannerlogo='' where CollectionId=" + Request.QueryString["CollectionId"].ToString() + "");
                }
            }
        }
    }
}