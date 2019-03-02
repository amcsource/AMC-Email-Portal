using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Demo_FileUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        File.Copy(@"\\192.168.0.4\\ArvindProjectNew\\Team\\Binay\\l.txt", @"E:\\Test\\abc.txt");
    }
}