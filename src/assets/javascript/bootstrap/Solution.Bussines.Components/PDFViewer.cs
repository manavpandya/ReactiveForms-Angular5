using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Solution.Bussines.Components
{
    public class PDFViewer : WebControl
    {
        private string filepath;
        private string width;
        private string height;

        public string FilePath
        {
            get
            {
                return filepath;
            }
            set
            {
                filepath = value;
            }
        }

        public string FrameWidth
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public string FrameHeight
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        //protected override void RenderContents(HtmlTextWriter writer)
        //{
        //    StringBuilder frameHTML = new StringBuilder();
        //    //set the source of the iframe to the path of the PDF
        //    frameHTML.Append("<iframe src=" + Convert.ToString(FilePath) + " ");
        //    //set height and width of the frame
        //    frameHTML.Append("width=" + width + "px height=" + height + "px");
        //    frameHTML.Append("</iframe>");
        //    //add the html content back to the page
        //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
        //    writer.Write(frameHTML.ToString());
        //    writer.RenderEndTag();
        //}

    }
}
