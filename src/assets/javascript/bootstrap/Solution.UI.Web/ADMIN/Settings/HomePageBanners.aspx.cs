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
    public partial class HomePageBanners : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                ViewState["HomeImagebannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathHomePageBanner' AND storeid=1 AND isnull(Deleted,0)=0");
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
            dsSections = CommonComponent.GetCommonDataSet("select isnull(Section1,0) as Section1,isnull(Section2,0) as Section2,isnull(Section3,0) as Section3,isnull(Section4,0) as Section4,isnull(Section5,0) as Section5,isnull(DOSection1,0) as DOSection1,isnull(DOSection2,0) as DOSection2,isnull(DOSection3,0) as DOSection3,isnull(DOSection4,0) as DOSection4,isnull(DOSection5,0) as DOSection5 from tb_HomePageBannerSection");
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


                if (Convert.ToBoolean(dsSections.Tables[0].Rows[0]["Section3"].ToString()))
                {
                    chksection3.Checked = true;
                }
                else
                {
                    chksection3.Checked = false;
                }

                if (Convert.ToBoolean(dsSections.Tables[0].Rows[0]["Section4"].ToString()))
                {
                    chksection4.Checked = true;
                }
                else
                {
                    chksection4.Checked = false;
                }

                if (Convert.ToBoolean(dsSections.Tables[0].Rows[0]["Section5"].ToString()))
                {
                    chksection5.Checked = true;
                }
                else
                {
                    chksection5.Checked = false;
                }


                txtDOSection1.Text = dsSections.Tables[0].Rows[0]["DOSection1"].ToString();
                txtDOSection2.Text = dsSections.Tables[0].Rows[0]["DOSection2"].ToString();
                txtDOSection3.Text = dsSections.Tables[0].Rows[0]["DOSection3"].ToString();
                txtDOSection4.Text = dsSections.Tables[0].Rows[0]["DOSection4"].ToString();
                txtDOSection5.Text = dsSections.Tables[0].Rows[0]["DOSection5"].ToString();
            }



            DataSet DsBanners = new DataSet();
            DsBanners = CommonComponent.GetCommonDataSet("select bannerid,isnull(Title,'') as Title,isnull(BannerURL,'') as BannerURL,isnull(LinkTarget,'') as LinkTarget,isnull(imagename,'') as imagename from tb_HomePageBannerDetails");
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
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 3)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img3.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden3.Value = imagename.ToString();
                        }

                        TxtbannerTitle3.Text = title.ToString();
                        TxtBannerURL3.Text = url;
                        ddlTarget3.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 4)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img4.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden4.Value = imagename.ToString();
                        }

                        TxtbannerTitle4.Text = title.ToString();
                        TxtBannerURL4.Text = url;
                        ddlTarget4.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 5)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img5.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden5.Value = imagename.ToString();
                        }

                        TxtbannerTitle5.Text = title.ToString();
                        TxtBannerURL5.Text = url;
                        ddlTarget5.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 6)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img6.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden6.Value = imagename.ToString();
                        }

                        TxtbannerTitle6.Text = title.ToString();
                        TxtBannerURL6.Text = url;
                        ddlTarget6.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 7)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img7.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden7.Value = imagename.ToString();
                        }

                        TxtbannerTitle7.Text = title.ToString();
                        TxtBannerURL7.Text = url;
                        ddlTarget7.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 8)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img8.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden8.Value = imagename.ToString();
                        }

                        TxtbannerTitle8.Text = title.ToString();
                        TxtBannerURL8.Text = url;
                        ddlTarget8.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 9)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img9.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden9.Value = imagename.ToString();
                        }

                        TxtbannerTitle9.Text = title.ToString();
                        TxtBannerURL9.Text = url;
                        ddlTarget9.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 10)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img10.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden10.Value = imagename.ToString();
                        }

                        TxtbannerTitle10.Text = title.ToString();
                        TxtBannerURL10.Text = url;
                        ddlTarget10.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 11)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img11.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden11.Value = imagename.ToString();
                        }

                        TxtbannerTitle11.Text = title.ToString();
                        TxtBannerURL11.Text = url;
                        ddlTarget11.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 12)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img12.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden12.Value = imagename.ToString();
                        }

                        TxtbannerTitle12.Text = title.ToString();
                        TxtBannerURL12.Text = url;
                        ddlTarget12.SelectedValue = target.ToString();
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 13)
                    {

                        if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString())))
                        {
                            Random rd = new Random();
                            img13.Src = ViewState["HomeImagebannrpath"].ToString() + "/" + imagename.ToString() + "?" + rd.Next(10000).ToString();
                            Hidden13.Value = imagename.ToString();
                        }

                        TxtbannerTitle13.Text = title.ToString();
                        TxtBannerURL13.Text = url;
                        ddlTarget13.SelectedValue = target.ToString();
                    }

                }
            }
        }


        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {

            int errorcounter = 0;
            int errorsection1 = 0;
            int errorsection2 = 0;
            int errorsection3 = 0;
            int errorsection4 = 0;
            int errorsection5 = 0;


            int DOSection1 = 0;
            int DOSection2 = 0;
            int DOSection3 = 0;
            int DOSection4 = 0;
            int DOSection5 = 0;


            ////// save banner 0

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle0.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL0.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget0.SelectedValue.ToString() + "' where bannerid=0");



            if (FileUpload0.HasFile)
            {
                string filename = "";




                if (CheckImageSize(FileUpload0, 780, 250))
                {
                    FileInfo fl = new FileInfo(FileUpload0.FileName.ToString());
                    filename = "0" + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload0.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden0.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload0.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden0.Value = filename.ToString();
                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=0");
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


            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle1.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL1.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget1.SelectedValue.ToString() + "' where bannerid=1");



            if (FileUpload1.HasFile)
            {
                string filename = "";




                if (CheckImageSize(FileUpload1, 780, 250))
                {
                    FileInfo fl = new FileInfo(FileUpload1.FileName.ToString());
                    filename = "1" + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload1.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden1.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload1.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden1.Value = filename.ToString();
                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=1");
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

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle2.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL2.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget2.SelectedValue.ToString() + "' where bannerid=2");



            if (FileUpload2.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload2, 780, 250))
                {
                    FileInfo f2 = new FileInfo(FileUpload2.FileName.ToString());
                    filename = "2" + "" + f2.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload2.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden2.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload2.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden2.Value = filename.ToString();

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=2");
                    trf2.Visible = false;

                }
                else
                {
                    trf2.Visible = true;
                    errorcounter++;
                    errorsection1++;
                }
            }



            ///////save banner 3
            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle3.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL3.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget3.SelectedValue.ToString() + "' where bannerid=3");



            if (FileUpload3.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload3, 780, 250))
                {

                    FileInfo f3 = new FileInfo(FileUpload3.FileName.ToString());
                    filename = "3" + "" + f3.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload3.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden3.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload3.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden3.Value = filename.ToString();

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=3");
                    trf3.Visible = false;

                }
                else
                {
                    trf3.Visible = true;
                    errorcounter++;
                    errorsection1++;
                }
            }


            /////save banner 4

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle4.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL4.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget4.SelectedValue.ToString() + "' where bannerid=4");



            if (FileUpload4.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload4, 780, 355))
                {
                    FileInfo f4 = new FileInfo(FileUpload4.FileName.ToString());
                    filename = "4" + "" + f4.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload4.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden4.Value = filename.ToString();


                    }
                    else
                    {
                        FileUpload4.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        Hidden4.Value = filename.ToString();
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=4");
                    trf4.Visible = false;

                }
                else
                {
                    trf4.Visible = true;
                    errorcounter++;
                    errorsection2++;
                }
            }


            /////save banner 5

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle5.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL5.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget5.SelectedValue.ToString() + "' where bannerid=5");



            if (FileUpload5.HasFile)
            {
                string filename = "";


                if (CheckImageSize(FileUpload5, 780, 355))
                {
                    FileInfo f5 = new FileInfo(FileUpload5.FileName.ToString());
                    filename = "5" + "" + f5.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {

                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload5.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden5.Value = filename.ToString();
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        FileUpload5.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden5.Value = filename.ToString();
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=5");
                    trf5.Visible = false;

                }
                else
                {
                    trf5.Visible = true;
                    errorcounter++;
                    errorsection2++;
                }
            }

            /////save banner 6

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle6.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL6.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget6.SelectedValue.ToString() + "' where bannerid=6");



            if (FileUpload6.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload6, 510, 500))
                {
                    FileInfo f6 = new FileInfo(FileUpload6.FileName.ToString());
                    filename = "6" + "" + f6.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden6.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload6.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        FileUpload6.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden6.Value = filename.ToString();
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=6");
                    trf6.Visible = false;

                }
                else
                {
                    trf6.Visible = true;
                    errorcounter++;
                    errorsection3++;
                }
            }

            /////save banner 7

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle7.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL7.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget7.SelectedValue.ToString() + "' where bannerid=7");



            if (FileUpload7.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload7, 510, 220))
                {
                    FileInfo f7 = new FileInfo(FileUpload7.FileName.ToString());
                    filename = "7" + "" + f7.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden7.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload7.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        FileUpload7.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        Hidden7.Value = filename.ToString();
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=7");
                    trf7.Visible = false;

                }
                else
                {
                    trf7.Visible = true;
                    errorcounter++;
                    errorsection3++;
                }
            }



            /////save banner 8

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle8.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL8.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget8.SelectedValue.ToString() + "' where bannerid=8");



            if (FileUpload8.HasFile)
            {
                string filename = "";


                if (CheckImageSize(FileUpload8, 510, 750))
                {
                    FileInfo f8 = new FileInfo(FileUpload8.FileName.ToString());
                    filename = "8" + "" + f8.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden8.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload8.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden8.Value = filename.ToString();
                        FileUpload8.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=8");
                    trf8.Visible = false;

                }
                else
                {
                    trf8.Visible = true;
                    errorcounter++;
                    errorsection3++;
                }
            }


            /////save banner 9

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle9.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL9.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget9.SelectedValue.ToString() + "' where bannerid=9");



            if (FileUpload9.HasFile)
            {
                string filename = "";



                if (CheckImageSize(FileUpload9, 510, 250))
                {
                    FileInfo f9 = new FileInfo(FileUpload9.FileName.ToString());
                    filename = "9" + "" + f9.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden9.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload9.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden9.Value = filename.ToString();
                        FileUpload9.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=9");
                    trf9.Visible = false;

                }
                else
                {
                    trf9.Visible = true;
                    errorcounter++;
                    errorsection3++;
                }
            }

            /////save banner 10

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle10.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL10.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget10.SelectedValue.ToString() + "' where bannerid=10");



            if (FileUpload10.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload10, 510, 470))
                {
                    FileInfo f10 = new FileInfo(FileUpload10.FileName.ToString());
                    filename = "10" + "" + f10.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden10.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload10.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden10.Value = filename.ToString();
                        FileUpload10.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=10");
                    trf10.Visible = false;

                }
                else
                {
                    trf10.Visible = true;
                    errorcounter++;
                    errorsection3++;
                }
            }

            /////save banner 11

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle11.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL11.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget11.SelectedValue.ToString() + "' where bannerid=11");



            if (FileUpload11.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload11, 1590, 200))
                {
                    FileInfo f11 = new FileInfo(FileUpload11.FileName.ToString());
                    filename = "11" + "" + f11.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden11.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload11.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden11.Value = filename.ToString();
                        FileUpload11.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=11");
                    trf11.Visible = false;

                }
                else
                {
                    trf11.Visible = true;
                    errorcounter++;
                    errorsection4++;
                }
            }


            /////save banner 12

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle12.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL12.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget12.SelectedValue.ToString() + "' where bannerid=12");



            if (FileUpload12.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload12, 1050, 300))
                {
                    FileInfo f12 = new FileInfo(FileUpload12.FileName.ToString());
                    filename = "12" + "" + f12.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden12.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload12.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden12.Value = filename.ToString();
                        FileUpload12.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=12");
                    trf12.Visible = false;

                }
                else
                {
                    trf12.Visible = true;
                    errorcounter++;
                    errorsection5++;
                }
            }



            /////save banner 13

            CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set Title='" + TxtbannerTitle13.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL13.Text.ToString().Replace("'", "''") + "',LinkTarget='" + ddlTarget13.SelectedValue.ToString() + "' where bannerid=13");



            if (FileUpload13.HasFile)
            {
                string filename = "";

                if (CheckImageSize(FileUpload13, 510, 300))
                {
                    FileInfo f13 = new FileInfo(FileUpload13.FileName.ToString());
                    filename = "13" + "" + f13.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename)))
                    {
                        Hidden13.Value = filename.ToString();
                        File.Delete(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));
                        FileUpload13.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));


                    }
                    else
                    {
                        Hidden13.Value = filename.ToString();
                        FileUpload13.SaveAs(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["HomeImagebannrpath"].ToString() + "/" + filename));

                    }
                    CommonComponent.ExecuteCommonData("update tb_HomePageBannerDetails set imagename='" + filename + "' where bannerid=13");
                    trf13.Visible = false;

                }
                else
                {
                    trf13.Visible = true;
                    errorcounter++;
                    errorsection5++;
                }
            }


            //////////update topics

            string banner0 = "";
            string banner1 = "";
            string banner2 = "";
            string banner3 = "";
            string banner4 = "";
            string banner5 = "";
            string banner6 = "";
            string banner7 = "";
            string banner8 = "";
            string banner9 = "";
            string banner10 = "";
            string banner11 = "";
            string banner12 = "";
            string banner13 = "";


            Random rd = new Random();

            DataSet DsBanners = new DataSet();
            DsBanners = CommonComponent.GetCommonDataSet("select bannerid,isnull(Title,'') as Title,isnull(BannerURL,'') as BannerURL,isnull(LinkTarget,'') as LinkTarget,isnull(imagename,'') as imagename from tb_HomePageBannerDetails");
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

                        banner0 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 1)
                    {

                        banner1 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 2)
                    {

                        banner2 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"  /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 3)
                    {

                        banner3 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 4)
                    {

                        banner4 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 5)
                    {

                        banner5 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 6)
                    {

                        banner6 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 7)
                    {

                        banner7 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 8)
                    {

                        banner8 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 9)
                    {

                        banner9 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 10)
                    {

                        banner10 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 11)
                    {

                        banner11 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 12)
                    {

                        banner12 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                    else if (Convert.ToInt32(DsBanners.Tables[0].Rows[i]["bannerid"].ToString()) == 13)
                    {

                        banner13 = "<a href=\"" + url + "\" title=\"" + Server.HtmlEncode(title.Trim()) + "\" target=\"" + target + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner") + "/" + imagename.ToString() + "?" + rd.Next(1000).ToString() + "\"  alt=\"" + Convert.ToString(title).Trim() + "\"  Title=\"" + Server.HtmlEncode(Convert.ToString(title).Trim()) + "\"   /></a>";
                    }
                }

            }


            bool section1 = false;
            bool section2 = false;
            bool section3 = false;
            bool section4 = false;
            bool section5 = false;

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

            if (chksection3.Checked)
            {
                section3 = true;
            }
            else
            {
                section3 = false;
            }

            if (chksection4.Checked)
            {
                section4 = true;
            }
            else
            {
                section4 = false;
            }

            if (chksection5.Checked)
            {
                section5 = true;
            }
            else
            {
                section5 = false;
            }


            Int32.TryParse(txtDOSection1.Text, out DOSection1);
            Int32.TryParse(txtDOSection2.Text, out DOSection2);
            Int32.TryParse(txtDOSection3.Text, out DOSection3);
            Int32.TryParse(txtDOSection4.Text, out DOSection4);
            Int32.TryParse(txtDOSection5.Text, out DOSection5);


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


            DataTable dtsection1 = new DataTable();
            DataColumn col11 = new DataColumn("sectionid", typeof(string));
            dtsection1.Columns.Add(col11);
            DataColumn col21 = new DataColumn("displayorder", typeof(string));
            dtsection1.Columns.Add(col21);
            DataColumn col31 = new DataColumn("active", typeof(string));
            dtsection1.Columns.Add(col31);
            dtsection1.AcceptChanges();


            DataRow dr3 = null;
            dr3 = dtsection1.NewRow();
            dr3["sectionid"] = 3;
            dr3["displayorder"] = DOSection3;
            dr3["active"] = section3;
            dtsection1.Rows.Add(dr3);
            dtsection1.AcceptChanges();

            DataRow dr4 = null;
            dr4 = dtsection1.NewRow();
            dr4["sectionid"] = 4;
            dr4["displayorder"] = DOSection4;
            dr4["active"] = section4;
            dtsection1.Rows.Add(dr4);
            dtsection1.AcceptChanges();


            DataRow dr5 = null;
            dr5 = dtsection1.NewRow();
            dr5["sectionid"] = 5;
            dr5["displayorder"] = DOSection5;
            dr5["active"] = section5;
            dtsection1.Rows.Add(dr5);
            dtsection1.AcceptChanges();
            String Homepagebanner = "";
            string HomePageEvents = "";
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
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
                                    Homepagebanner += "<div class=\"row\">";



                                    Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
                                    Homepagebanner += "<div class=\"row\">";
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                                    Homepagebanner += banner0;
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                                    Homepagebanner += banner1;
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";


                                    Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
                                    Homepagebanner += "<div class=\"row\">";
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                                    Homepagebanner += banner2;
                                    Homepagebanner += " </div>";
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                                    Homepagebanner += banner3;
                                    Homepagebanner += "</div>";

                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";

                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                }


                            }
                            else if (drtemp[i]["sectionid"].ToString() == "2")
                            {
                                if (chksection2.Checked)
                                {
                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
                                    Homepagebanner += "<div class=\"row\">";
                                    Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
                                    Homepagebanner += "<div class=\"row\">";

                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                                    Homepagebanner += banner4;
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
                                    Homepagebanner += "<div class=\"row\">";

                                    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                                    Homepagebanner += banner5;
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                    Homepagebanner += "</div>";
                                }
                            }
                            
                        }
                   

                    }

                }
            






            drtemp = dtsection1.Select("", "displayorder asc");
            if (drtemp != null && drtemp.Length > 0)
            {
                for (int i = 0; i < drtemp.Length; i++)
                {

                    if (drtemp[i]["active"].ToString().ToLower() == "true" || drtemp[i]["active"].ToString().ToLower() == "1")
                    {
                        //if (drtemp[i]["sectionid"].ToString() == "1")
                        //{
                        //    if (chksection1.Checked)
                        //    {
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
                        //        Homepagebanner += "<div class=\"row\">";



                        //        Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
                        //        Homepagebanner += "<div class=\"row\">";
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                        //        Homepagebanner += banner0;
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                        //        Homepagebanner += banner1;
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";


                        //        Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
                        //        Homepagebanner += "<div class=\"row\">";
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                        //        Homepagebanner += banner2;
                        //        Homepagebanner += " </div>";
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                        //        Homepagebanner += banner3;
                        //        Homepagebanner += "</div>";

                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";

                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //    }


                        //}
                        //else if (drtemp[i]["sectionid"].ToString() == "2")
                        //{
                        //    if (chksection2.Checked)
                        //    {
                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
                        //        Homepagebanner += "<div class=\"row\">";
                        //        Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
                        //        Homepagebanner += "<div class=\"row\">";

                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
                        //        Homepagebanner += banner4;
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
                        //        Homepagebanner += "<div class=\"row\">";

                        //        Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
                        //        Homepagebanner += banner5;
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //        Homepagebanner += "</div>";
                        //    }
                        //}
                        if (drtemp[i]["sectionid"].ToString() == "3")
                        {

                            if (chksection3.Checked)
                            {
                                HomePageEvents += "<div class=\"col-sm-12 view-banner-row1\">";
                                HomePageEvents += "<div class=\"row\">";
                                HomePageEvents += "<div class=\"col-sm-4\">";
                                HomePageEvents += "<div class=\"view-banner-row1-pt\">";
                                HomePageEvents += banner6;
                                HomePageEvents += " </div>";
                                HomePageEvents += "<div class=\"view-banner-row1-pt\">";
                                HomePageEvents += banner7;
                                HomePageEvents += " </div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "<div class=\"col-sm-4\">";
                                HomePageEvents += "<div class=\"view-banner-row1-pt\">";
                                HomePageEvents += banner8;
                                HomePageEvents += " </div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "<div class=\"col-sm-4\">";
                                HomePageEvents += "<div class=\"view-banner-row1-pt\">";
                                HomePageEvents += banner9;
                                HomePageEvents += "</div>";
                                HomePageEvents += "<div class=\"view-banner-row1-pt\">";
                                HomePageEvents += banner10;
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";


                            }
                        }
                        else if (drtemp[i]["sectionid"].ToString() == "4")
                        {
                            if (chksection4.Checked)
                            {
                                HomePageEvents += "<div class=\"col-sm-12 view-banner-row2\">";
                                HomePageEvents += "<div class=\"row\">";
                                HomePageEvents += "<div class=\"col-sm-12\">";
                                HomePageEvents += "<div class=\"view-banner-row2-pt\">";
                                HomePageEvents += banner11;
                                HomePageEvents += " </div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";

                            }
                        }
                        else if (drtemp[i]["sectionid"].ToString() == "5")
                        {
                            if (chksection5.Checked)
                            {
                                HomePageEvents += "<div class=\"col-sm-12 view-banner-row3\">";
                                HomePageEvents += "<div class=\"row\">";

                                HomePageEvents += "<div class=\"col-sm-8\">";
                                HomePageEvents += "<div class=\"view-banner-row3-pt\">";
                                HomePageEvents += banner12;
                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";

                                HomePageEvents += "<div class=\"col-sm-4\">";
                                HomePageEvents += "<div class=\"view-banner-row3-pt\">";
                                HomePageEvents += banner13;
                                HomePageEvents += " </div>";
                                HomePageEvents += "</div>";

                                HomePageEvents += "</div>";
                                HomePageEvents += "</div>";
                            }
                        }
                    }

                }

            }













            //if (chksection1.Checked)
            //{
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
            //    Homepagebanner += "<div class=\"row\">";



            //    Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
            //    Homepagebanner += "<div class=\"row\">";
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
            //    Homepagebanner += banner0;
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
            //    Homepagebanner += banner1;
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";


            //    Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
            //    Homepagebanner += "<div class=\"row\">";
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
            //    Homepagebanner += banner2;
            //    Homepagebanner += " </div>";
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
            //    Homepagebanner += banner3;
            //    Homepagebanner += "</div>";

            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";

            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //}
            //if (chksection2.Checked)
            //{
            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-bg\">";
            //    Homepagebanner += "<div class=\"row\">";
            //    Homepagebanner += "<div class=\"col-md-6 banner-offer-left\">";
            //    Homepagebanner += "<div class=\"row\">";

            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-left-row\">";
            //    Homepagebanner += banner4;
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "<div class=\"col-md-6 banner-offer-right\">";
            //    Homepagebanner += "<div class=\"row\">";

            //    Homepagebanner += "<div class=\"col-sm-12 banner-offer-right-row\">";
            //    Homepagebanner += banner5;
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //    Homepagebanner += "</div>";
            //}

            CommonComponent.ExecuteCommonData("update tb_topic set Description='" + Homepagebanner + "' where Title='HomePageBanner' and StoreID=1");



            //if (chksection3.Checked)
            //{
            //    HomePageEvents += "<div class=\"col-sm-12 view-banner-row1\">";
            //    HomePageEvents += "<div class=\"row\">";
            //    HomePageEvents += "<div class=\"col-sm-4\">";
            //    HomePageEvents += "<div class=\"view-banner-row1-pt\">";
            //    HomePageEvents += banner6;
            //    HomePageEvents += " </div>";
            //    HomePageEvents += "<div class=\"view-banner-row1-pt\">";
            //    HomePageEvents += banner7;
            //    HomePageEvents += " </div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "<div class=\"col-sm-4\">";
            //    HomePageEvents += "<div class=\"view-banner-row1-pt\">";
            //    HomePageEvents += banner8;
            //    HomePageEvents += " </div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "<div class=\"col-sm-4\">";
            //    HomePageEvents += "<div class=\"view-banner-row1-pt\">";
            //    HomePageEvents += banner9;
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "<div class=\"view-banner-row1-pt\">";
            //    HomePageEvents += banner10;
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";


            //}


            //if (chksection4.Checked)
            //{
            //    HomePageEvents += "<div class=\"col-sm-12 view-banner-row2\">";
            //    HomePageEvents += "<div class=\"row\">";
            //    HomePageEvents += "<div class=\"col-sm-12\">";
            //    HomePageEvents += "<div class=\"view-banner-row2-pt\">";
            //    HomePageEvents += banner11;
            //    HomePageEvents += " </div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";

            //}


            //if (chksection5.Checked)
            //{
            //    HomePageEvents += "<div class=\"col-sm-12 view-banner-row3\">";
            //    HomePageEvents += "<div class=\"row\">";

            //    HomePageEvents += "<div class=\"col-sm-8\">";
            //    HomePageEvents += "<div class=\"view-banner-row3-pt\">";
            //    HomePageEvents += banner12;
            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";

            //    HomePageEvents += "<div class=\"col-sm-4\">";
            //    HomePageEvents += "<div class=\"view-banner-row3-pt\">";
            //    HomePageEvents += banner13;
            //    HomePageEvents += " </div>";
            //    HomePageEvents += "</div>";

            //    HomePageEvents += "</div>";
            //    HomePageEvents += "</div>";
            //}






            CommonComponent.ExecuteCommonData("update tb_topic set Description='" + HomePageEvents + "' where Title='HomePageEventBanner' and StoreID=1");





            CommonComponent.ExecuteCommonData("update tb_HomePageBannerSection set Section1='" + section1 + "',Section2='" + section2 + "',Section3='" + section3 + "',Section4='" + section4 + "',Section5='" + section5 + "',DOSection1=" + DOSection1 + ",DOSection2=" + DOSection2 + ",DOSection3=" + DOSection3 + ",DOSection4=" + DOSection4 + ",DOSection5=" + DOSection5 + "");




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
                if (errorsection3 > 0)
                {
                    msg += "Section3,";
                }
                if (errorsection4 > 0)
                {
                    msg += "Section4,";
                }
                if (errorsection5 > 0)
                {
                    msg += "Section5,";
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