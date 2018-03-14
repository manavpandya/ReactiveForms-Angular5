using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = System.Drawing.Image;
using System.IO;
using System.Configuration;
using System.Web.Security;

namespace Solution.UI.Web.ckeditor
{
    public partial class ImageBrowser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDirectoryList();
                ChangeDirectory(null, null);

                NewDirectoryButton.OnClientClick = "var name = prompt('Enter name of folder:'); if (name == null || name == '') return false; document.getElementById('" + NewDirectoryName.ClientID + "').value = name;";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "FocusImageList", "window.setTimeout(\"document.getElementById('" + ImageList.ClientID + "').focus();\", 1);", true);
        }

        protected void BindDirectoryList()
        {
            ImageFolder = "";
            string[] directories = Directory.GetDirectories(FileImageFolder);
            directories = Array.ConvertAll<string, string>(directories, delegate(string directory) { return directory.Substring(directory.LastIndexOf('\\') + 1); });
            DirectoryList.DataSource = directories;
            DirectoryList.DataBind();
            DirectoryList.Items.Insert(0, "Root");
        }

        protected void ChangeDirectory(object sender, EventArgs e)
        {
            DeleteDirectoryButton.Enabled = (DirectoryList.SelectedIndex != 0);
            ImageFolder = (DirectoryList.SelectedIndex == 0) ? "" : DirectoryList.SelectedValue + "/";
            SearchTerms.Text = "";
            BindImageList();
            SelectImage(null, null);
        }

        protected void DeleteFolder(object sender, EventArgs e)
        {
            Directory.Delete(FileImageFolder, true);
            BindDirectoryList();
            ChangeDirectory(null, null);
        }

        protected void CreateFolder(object sender, EventArgs e)
        {
            string name = UniqueDirectory(NewDirectoryName.Value);
            Directory.CreateDirectory(FileImageFolderRoot + name);
            BindDirectoryList();
            DirectoryList.SelectedValue = name;
            ChangeDirectory(null, null);
        }

        protected void BindImageList()
        {
            ImageList.Items.Clear();
            string[] files = Directory.GetFiles(FileImageFolder, "*" + SearchTerms.Text.Replace(" ", "*") + "*");

            foreach (string file in files)
            {
                if (IsImage(file))
                    ImageList.Items.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }

            if (files.Length > 0)
                ImageList.SelectedIndex = 0;
        }

        protected void Search(object sender, EventArgs e)
        {
            BindImageList();
        }

        protected void SelectImage(object sender, EventArgs e)
        {
            RenameImageButton.Enabled = (ImageList.Items.Count != 0);
            DeleteImageButton.Enabled = (ImageList.Items.Count != 0);

            if (ImageList.Items.Count == 0)
            {
                Image1.ImageUrl = "";
                return;
            }

            Image1.ImageUrl = ImageFolder + ImageList.SelectedValue + "?" + new Random().Next(1000);
            Image img = Image.FromFile(FileImageFolder + ImageList.SelectedValue);
            img.Dispose();

            int pos = ImageList.SelectedItem.Text.LastIndexOf('.');
            if (pos == -1)
                return;
            RenameImageButton.OnClientClick = "var name = prompt('Enter new filename:','" + ImageList.SelectedItem.Text.Substring(0, pos) + "'); if (name == null || name == '') return false; document.getElementById('" + NewImageName.ClientID + "').value = name + '" + ImageList.SelectedItem.Text.Substring(pos) + "';";
            OkButton.OnClientClick = "window.top.opener.CKEDITOR.dialog.getCurrent().setValueOf('info', 'txtUrl', encodeURI('" + ImageFolder + ImageList.SelectedValue.Replace("'", "\\'") + "')); window.top.close(); window.top.opener.focus();";
        }

        protected void RenameImage(object sender, EventArgs e)
        {
            string filename = UniqueFilename(NewImageName.Value);
            File.Move(FileImageFolder + ImageList.SelectedValue, FileImageFolder + filename);
            BindImageList();
            ImageList.SelectedValue = filename;
            SelectImage(null, null);
        }

        protected void DeleteImage(object sender, EventArgs e)
        {
            File.Delete(FileImageFolder + ImageList.SelectedValue);
            BindImageList();
            SelectImage(null, null);
        }

        protected void Upload(object sender, EventArgs e)
        {
            if (IsImage(UploadedImageFile.FileName))
            {
                string filename = UniqueFilename(UploadedImageFile.FileName);
                UploadedImageFile.SaveAs(FileImageFolder + filename);

                Byte[] data = UploadedImageFile.FileBytes;
                FileStream file = File.Create(FileImageFolder + filename);
                file.Write(data, 0, data.Length);
                file.Close();

                BindImageList();
                ImageList.SelectedValue = filename;
                SelectImage(null, null);
            }
        }

        protected void Clear(object sender, EventArgs e)
        {
            Session.Remove("viewstate");
        }

        //util methods
        private bool IsImage(string file)
        {
            return file.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase);
        }

        protected ImageCodecInfo GetImageCodec(string file)
        {
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
            {
                foreach (string ext in codec.FilenameExtension.Split(';'))
                {
                    if (file.EndsWith(ext.Substring(1), StringComparison.CurrentCultureIgnoreCase))
                        return codec;
                }
            }

            return null;
        }

        protected string UniqueFilename(string filename)
        {
            string newfilename = filename;

            for (int i = 1; File.Exists(FileImageFolder + newfilename); i++)
            {
                newfilename = filename.Insert(filename.LastIndexOf('.'), "(" + i + ")");
            }

            return newfilename;
        }

        protected string UniqueDirectory(string directoryname)
        {
            string newdirectoryname = directoryname;

            for (int i = 1; Directory.Exists(FileImageFolderRoot + newdirectoryname); i++)
            {
                newdirectoryname = directoryname + "(" + i + ")";
            }

            return newdirectoryname;
        }

        //properties
        protected string ImageFolderRoot
        {
            get { return ResolveUrl(ConfigurationManager.AppSettings["ImageRoot"]); }
        }

        protected string FileImageFolderRoot
        {
            get { return Server.MapPath(ImageFolderRoot); }
        }

        protected string ImageFolder
        {
            get { return ImageFolderRoot + ViewState["folder"]; }
            set { ViewState["folder"] = value; }
        }

        protected string FileImageFolder
        {
            get { return Server.MapPath(ImageFolder); }
        }
    }
}