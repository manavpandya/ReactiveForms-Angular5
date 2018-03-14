using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class TradeInquiries : BasePage
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
                GetData();
            }
        }
        /// <summary>
        /// Get All TradeApplication Data
        /// </summary>
        public void GetData()
        {
            string strSql;
            strSql = "select *,(FirstName1+' '+LastName1) as CustomerName from tb_TradeApplication Order By CreatedOn DESC";

            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet(strSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdTradeInquiries.DataSource = ds;
                grdTradeInquiries.DataBind();
            }
            else
            {
                grdTradeInquiries.DataSource = null;
                grdTradeInquiries.DataBind();
            }


        }
        /// <summary>
        /// Gridview's RowdataBound Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdTradeInquiries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField DocumentFile = (HiddenField)e.Row.FindControl("DocumentFile");
                if (!string.IsNullOrEmpty(DocumentFile.Value))
                {


                    Button dnwid = ((Button)e.Row.FindControl("dnwid"));
                    string filepath = Convert.ToString(AppLogic.AppConfigs("ContentServerPhysicalPath").ToString() + AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + DocumentFile.Value);
                    System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                    if (file.Exists)
                    {
                        dnwid.Visible = true;
                        dnwid.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/download-pdf.jpg) no-repeat transparent; width: 31px; height: 31px; border:none;cursor:pointer;");
                    }
                }
            }
        }
        /// <summary>
        /// Gridview's RowCommand Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdTradeInquiries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Trim().ToLower() == "downloadpdf")
            {
                string DocumentFile = Convert.ToString(e.CommandArgument);
                if (!string.IsNullOrEmpty(DocumentFile))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "windowopen", "window.open('http://www.halfpricedrapes.com" + AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + DocumentFile.ToString() + "','_blank');", true);
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "windowopen", "window.open('http://www.halfpricedrapes.com" + AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + "Testkaushalam.txt','_blank');", true);
                    //downloadfile("http://www.halfpricedrapes.com/" + AppLogic.AppConfigs("OnlineApplication.DocumentPath").ToString() + DocumentFile.ToString());
                }


            }
        }

        /// <summary>
        /// Returns the extension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>Returns the extension of files</returns>
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }
        /// <summary>
        /// Download files from the specified filepath.
        /// </summary>
        /// <param name="filepath">string filepath.</param>
        private void downloadfile(string filepath)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                //if (file.Exists)
                //{
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Clear();
                Response.ContentType = ReturnExtension(file.Extension.ToLower());
                Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                System.IO.FileStream sourceFile = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open);
                long FileSize;
                FileSize = sourceFile.Length;
                byte[] getContent = new byte[(int)FileSize];
                sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                sourceFile.Close();
                Response.BinaryWrite(getContent);
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No file available for this Trade Application.', 'Message');});", true);
                //}
            }
            catch (Exception ex) { }
        }

        protected void grdTradeInquiries_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTradeInquiries.PageIndex = e.NewPageIndex;
            GetData();
        }
    }
}