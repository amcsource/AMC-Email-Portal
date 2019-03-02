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
using System.Windows.Forms;
using System.Threading;

public partial class Demo_BrowseDirectory : System.Web.UI.Page
{

    Thread newThread;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string toDocumentFilePath = "@E:\\Test";//Network shared drive path;
        //        string UName = "admin";
        //        string PWD = "India@123";
        //        string domain = "";

        //        using (BethesdaConsentFormWCFSvc.UNCAccessWithCredentials unc = new BethesdaConsentFormWCFSvc.UNCAccessWithCredentials())
        //        {
        //            if (unc.NetUseWithCredentials(toDocumentFilePath, UName, domain, PWD))
        //            {
        //                 System.IO.File.Copy(@"\\JAISHROTRIY-PC\Users\admin", toDocumentFilePath, true);
        //             }
        //        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        newThread = new Thread(new ThreadStart(ThreadMethod));
        newThread.IsBackground = true;
        newThread.SetApartmentState(ApartmentState.STA);

        newThread.Start();
    }

    public void mm(string mm)
    {

        Page.Response.Write(mm);
        TextBox1.Text = mm;
    }

    public Boolean flag = true;
    void ThreadMethod()
    {
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        dialog.RootFolder = Environment.SpecialFolder.Desktop;
        dialog.SelectedPath = "C:\\";
        //if (flag == true)
        //{
        //    flag = false;
        //    newThread.Abort();
        //}
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            mm(dialog.SelectedPath.ToString());

        }
    }
}