using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Demo_FileAccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName;
        string[] files = System.IO.Directory.GetFiles(@"E:\SubDir");

        // Copy the files and overwrite destination files if they already exist.
        foreach (string s in files)
        {
            // Use static Path methods to extract only the file name from the path.
            fileName = System.IO.Path.GetFileName(s);
            Response.Write(fileName + "<br>");
        }
    }
}