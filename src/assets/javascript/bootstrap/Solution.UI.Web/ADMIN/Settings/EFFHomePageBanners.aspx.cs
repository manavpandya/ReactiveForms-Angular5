using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class EFFHomePageBanners : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                ViewState["HomeImagebannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='EFFImagePathHomePageBanner' AND storeid=1 AND isnull(Deleted,0)=0");
                if (!Directory.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString())))
                {
                    Directory.CreateDirectory(Server.MapPath(ViewState["HomeImagebannrpath"].ToString()));

                }
                BindBanners();
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

                if (img.Width == width && img.Height == height)
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


        private void BindBanners()
        {

            DataSet dsSections = new DataSet();
            dsSections = CommonComponent.GetCommonDataSet("select isnull(Section1,0) as Section1,isnull(Section2,0) as Section2,isnull(DOSection1,0) as DOSection1,isnull(DOSection2,0) as DOSection2 from tb_HomePageBannerSectionEFF");
            if (dsSections != null && dsSections.Tables.Count > 0 && dsSections.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToBoolean(dsSections.Tables[0].Rows[0]["Section1"].ToString()))
                {
                    chksection1.Checked = true;
                }
                else
                {
                    chksection1.Checked = false;
                }

                if (Convert.ToBoolean(dsSections.Tables[0].Rows[0]["Section2"].ToString()))
                {
                    chksection2.Checked = true;
                }
                else
                {
                    chksection2.Checked = false;
                }





                txtDOSection1.Text = dsSections.Tables[0].Rows[0]["DOSection1"].ToString();
                txtDOSection2.Text = dsSections.Tables[0].Rows[0]["DOSection2"].ToString();

            }



            DataSet DsBanners = new DataSet();
            DsBanners = CommonComponent.GetCommonDataSet("select bannerid,isnull(Title,'') as Title,isnull(BannerURL,'') as BannerURL,isnull(LinkTarget,'') as LinkTarget,isnull(imagename,'') as imagename from tb_HomePageBannerDetailsEFF");
            if (DsBanners != null && DsBanners.Tables.Count > 0 && DsBanners.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < DsBanners.Tables[0].Rows.Count; i++)
                {
                    string title = "";
                    string url = "";
                    string target = "";
                    string imagename = "";

                    title = DsBanners.Tables[0].Rows[i]["Title"].ToString();
                    url = DsBanners.Tables[0].Rows[i]["BannerURL"].ToString();
                    target = DsBanners.Tables[0].Rows[i]["LinkTarget"].ToString();
                    imagename = DsBanners.Tables[0].Rows[i]["imagename"].ToString();
                    if (String.IsNullOrEmpty(url))
                    {
                        url = "javascript:void(0);";
                    }

                    if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 0)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img0.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden1.Value = imagename.ToString();
                        }

                        TxtbannerTitle0.Text = title.ToString();
                        TxtBannerURL0.Text = url;
                        ddlTarget0.SelectedValue = target.ToString();
                    }

                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 1)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img1.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden1.Value = imagename.ToString();
                        }

                        TxtbannerTitle1.Text = title.ToString();
                        TxtBannerURL1.Text = url;
                        ddlTarget1.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 2)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img2.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden2.Value = imagename.ToString();
                        }

                        TxtbannerTitle2.Text = title.ToString();
                        TxtBannerURL2.Text = url;
                        ddlTarget2.SelectedValue = target.ToString();
                    }


                }
            }
        }


        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {

            int errorcounter = 0;
            int errorsection1 = 0;
            int errorsection2 = 0;



            int DOSection1 = 0;
            int DOSection2 = 0;



            ////// save banner 0

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set Title='" + TxtbannerTitle0.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL0.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget0.SelectedValue.ToString() + "' where bannerid=0");



            if (FileUpload0.HasFile)
            {
                string filename = "";




                if (CheckImageSize(FileUpload0, 770, 355))
                {
                    FileInfo fl = new FileInfo(FileUpload0.FileName.ToString());
                    filename = "0" + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload0.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden0.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload0.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden0.Value = filename.ToString();
                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set imagename='" + filename + "' where bannerid=0");
                    trf0.Visible = false;

                }
                else
                {
                    trf0.Visible = true;
                    errorcounter++;
                    errorsection1++;
                }

            }


            ///////save banner 1


            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set Title='" + TxtbannerTitle1.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL1.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget1.SelectedValue.ToString() + "' where bannerid=1");



            if (FileUpload1.HasFile)
            {
                string filename = "";




                if (CheckImageSize(FileUpload1, 770, 355))
                {
                    FileInfo fl = new FileInfo(FileUpload1.FileName.ToString());
                    filename = "1" + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload1.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden1.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload1.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden1.Value = filename.ToString();
                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set imagename='" + filename + "' where bannerid=1");
                    trf1.Visible = false;

                }
                else
                {
                    trf1.Visible = true;
                    errorcounter++;
                    errorsection1++;
                }

            }




            ////////save banner2

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set Title='" + TxtbannerTitle2.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL2.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget2.SelectedValue.ToString() + "' where bannerid=2");



            if (FileUpload2.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload2, 1570, 200))
                {
                    FileInfo f2 = new FileInfo(FileUpload2.FileName.ToString());
                    filename = "2" + "" + f2.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload2.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden2.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload2.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden2.Value = filename.ToString();

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetailsEFF set imagename='" + filename + "' where bannerid=2");
                    trf2.Visible = false;

                }
                else
                {
                    trf2.Visible = true;
                    errorcounter++;
                    errorsection2++;
                }
            }






            //////////update topics

            string banner0 = "";
            string banner1 = "";
            string banner2 = "";



            Random rd = new Random();

            DataSet DsBanners = new DataSet();
            DsBanners = CommonComponent.GetCommonDataSet("select bannerid,isnull(Title,'') as Title,isnull(BannerURL,'') as BannerURL,isnull(LinkTarget,'') as LinkTarget,isnull(imagename,'') as imagename from tb_HomePageBannerDetailsEFF");
            if (DsBanners != null && DsBanners.Tables.Count > 0 && DsBanners.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < DsBanners.Tables[0].Rows.Count; i++)
                {
                    string title = "";
                    string url = "";
                    string target = "";
                    string imagename = "";

                    title = DsBanners.Tables[0].Rows[i]["Title"].ToString();
                    url = DsBanners.Tables[0].Rows[i]["BannerURL"].ToString();
                    target = DsBanners.Tables[0].Rows[i]["LinkTarget"].ToString();
                    imagename = DsBanners.Tables[0].Rows[i]["imagename"].ToString();
                    if (String.IsNullOrEmpty(url))
                    {
                        url = "javascript:void(0);";
                    }

                    if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 0)
                    {

                        banner0 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("EFFImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 1)
                    {

                        banner1 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("EFFImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 2)
                    {

                        banner2 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("EFFImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"  /></a>";
                    }

                }

            }


            bool section1 = false;
            bool section2 = false;


            if (chksection1.Checked)
            {
                section1 = true;
            }
            else
            {
                section1 = false;
            }


            if (chksection2.Checked)
            {
                section2 = true;
            }
            else
            {
                section2 = false;
            }




            Int32.TryParse(txtDOSection1.Text, out DOSection1);
            Int32.TryParse(txtDOSection2.Text, out DOSection2);



            DataTable dtsection = new DataTable();
            DataColumn col1 = new DataColumn("sectionid", typeof(string));
            dtsection.Columns.Add(col1);
            DataColumn col2 = new DataColumn("displayorder", typeof(string));
            dtsection.Columns.Add(col2);
            DataColumn col3 = new DataColumn("active", typeof(string));
            dtsection.Columns.Add(col3);
            dtsection.AcceptChanges();


            DataRow dr1 = null;
            dr1 = dtsection.NewRow();
            dr1["sectionid"] = 1;
            dr1["displayorder"] = DOSection1;
            dr1["active"] = section1;
            dtsection.Rows.Add(dr1);
            dtsection.AcceptChanges();


            DataRow dr2 = null;
            dr2 = dtsection.NewRow();
            dr2["sectionid"] = 2;
            dr2["displayorder"] = DOSection2;
            dr2["active"] = section2;
            dtsection.Rows.Add(dr2);
            dtsection.AcceptChanges();




            String Homepagebanner = "";

            DataRow[] drtemp = dtsection.Select("", "displayorder asc");
            if (drtemp != null && drtemp.Length > 0)
            {
                for (int i = 0; i < drtemp.Length; i++)
                {

                    if (drtemp[i]["active"].ToString().ToLower() == "true" || drtemp[i]["active"].ToString().ToLower() == "1")
                    {
                        if (drtemp[i]["sectionid"].ToString() == "1")
                        {
                            if (chksection1.Checked)
                            {






                                Homepagebanner += "<div class=\"col-md-12 padding-none\">";

                                Homepagebanner += "<div class=\"col-md-6 padding-none banner-offer-left\">";
                                Homepagebanner += banner0;
                                Homepagebanner += "</div>";
                                Homepagebanner += "<div class=\"col-md-6 padding-none banner-offer-right\">";
                                Homepagebanner += banner1;
                                Homepagebanner += "</div>";
                                Homepagebanner += "</div>";


                            }


                        }
                        else if (drtemp[i]["sectionid"].ToString() == "2")
                        {
                            if (chksection2.Checked)
                            {


                                Homepagebanner += "<div class=\"col-md-12 padding-none view-banner-main\">";
                                Homepagebanner += "<div class=\"row\">";
                                Homepagebanner += "<div class=\"col-sm-12 view-banner-row2\">";
                                Homepagebanner += "<div class=\"view-banner-row2-pt\">";


                                Homepagebanner += banner2;
                                Homepagebanner += "</div>";
                                Homepagebanner += "</div>";
                                Homepagebanner += "</div>";
                                Homepagebanner += "</div>";

                            }
                        }

                    }


                }

            }





            CommonComponent.ExecuteCommonData("update tb_topic set Description='" + Homepagebanner + "' where Title='EFFHomePageBanner' and StoreID=1");




            CommonComponent.ExecuteCommonData("update tb_HomePageBannerSectionEFF set Section1='" + section1 + "',Section2='" + section2 + "',DOSection1=" + DOSection1 + ",DOSection2=" + DOSection2 + "");




            if (errorcounter > 0)
            {

                string msg = "";
                if (errorsection1 > 0)
                {
                    msg += " Section1,";
                }
                if (errorsection2 > 0)
                {
                    msg += "Section2,";
                }

                if (msg.Length > 1)
                {
                    msg = msg.Substring(0, msg.Length - 1);
                }
                msg = "Please check Error Messages in " + msg;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('" + msg + "','Message')", true);

            }
            else
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('Record updated successfully.','Message')", true);
            }
            BindBanners();

        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/admin/dashboard.aspx");
        }
    }
}