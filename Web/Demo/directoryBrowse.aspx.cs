using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.ComponentModel;

public partial class Demo_directoryBrowse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GetDirectories();
    }

    private void GetDirectories()
    {
        string dir;
        if (Request.Form["dir"] == null || Request.Form["dir"].Length <= 0)
            dir = "D:\\";
        else
            dir = Request.Form["dir"];
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
        Response.Write("<ul class=\"jqueryFileTree\"\n");
        foreach (System.IO.DirectoryInfo di_child in di.GetDirectories())
            Response.Write("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + dir + di_child.Name + "/\">" + di_child.Name + "</a></li>\n");
        foreach (System.IO.FileInfo fi in di.GetFiles())
        {
            string ext = "";
            if (fi.Extension.Length > 1)
                ext = fi.Extension.Substring(1).ToLower();

            Response.Write("\t<li class=\"file ext_" + ext + "\"><a href=\"#\" rel=\"" + dir + fi.Name + "\">" + fi.Name + "</a></li>\n");
        }
        Response.Write("</ul>");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //FolderBrowserDialog fbd = new FolderBrowserDialog();
        //DialogResult result = fbd.ShowDialog();

        //OpenFileDialog fdlg = new OpenFileDialog();
        //fdlg.Title = "C# Corner Open File Dialog";
        //fdlg.InitialDirectory = @"c:\";
        //fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
        //fdlg.FilterIndex = 2;
        //fdlg.RestoreDirectory = true;
        //if (fdlg.ShowDialog() == DialogResult.OK)
        //{
        //    TextBox1.Text = fdlg.FileName;
        //}
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            Response.Write(d.Name + "\n");
            //Response.Write("Drive {0}", d.Name);
            //Response.Write("  File type: {0}", d.DriveType);
            //if (d.IsReady == true)
            //{
            //    Response.Write("  Volume label: {0}", d.VolumeLabel);
            //    Response.Write("  File system: {0}", d.DriveFormat);
            //    Response.Write(
            //        "  Available space to current user:{0, 15} bytes",
            //        d.AvailableFreeSpace);

            //    Response.Write(
            //        "  Total available space:          {0, 15} bytes",
            //        d.TotalFreeSpace);

            //    Response.Write(
            //        "  Total size of drive:            {0, 15} bytes ",
            //        d.TotalSize);
            //}
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        //NetworkBrowser nb = new NetworkBrowser();
        //foreach (string pc in nb.getNetworkComputers())
        //{
        //    cmbNetworkComputers.Items.Add(pc);
        //}
    }
}