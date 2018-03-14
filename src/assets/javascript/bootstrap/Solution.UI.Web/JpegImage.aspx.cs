using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using Solution.Bussines.Components;

namespace Solution.UI.Web
{
    public partial class JpegImage : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                this.Session["CaptchaImageText"] = GenerateRandomCode();
            }
            // Create a CAPTCHA image using the text stored in the Session object.
            CaptchaComponent ci = new CaptchaComponent(this.Session["CaptchaImageText"].ToString(), 150, 40, "Century Schoolbook");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
        }

        /// <summary>
        /// Generates the Random Code
        /// </summary>
        /// <returns>Returns the Generated Random Code</returns>
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string randmString = string.Empty;
            for (int i = 0; i < 2; i++)
                randmString += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            for (int i = 0; i < 2; i++)
                randmString += random.Next(10).ToString();
            for (int i = 0; i < 2; i++)
                randmString += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            return randmString;
        }
    }
}