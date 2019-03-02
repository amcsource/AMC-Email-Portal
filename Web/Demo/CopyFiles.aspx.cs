using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Demo_CopyFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string sourcePath = @TextBox1.Text;
        string targetPath = Server.MapPath("~/EmailAttachments");
        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        var di = new DirectoryInfo(sourcePath);
        FileInfo[] myfiles = new FileInfo[] { };
        myfiles = di.GetFiles("*");
        try
        {
            foreach (var s in myfiles)
            {
                string fileName = Path.GetFileName(s.FullName);
                string sourceFile = Path.Combine(sourcePath, fileName);
                string destFile = Path.Combine(targetPath, fileName);
                File.Copy(sourceFile, destFile, true);
            }
            Literal1.Text = "Success";
        }
        catch (Exception ex)
        {
            Literal1.Text = "Fail: " + ex.Message;
        }

    }
}