using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Diagnostics;
using System.Security;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;

public partial class Emails_pdf : System.Web.UI.Page
{
    public string filke = "", hh="";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        hh =Convert.ToString(Session["fileinfo"]);
        //Spire.PdfViewer.Forms.PdfDocumentViewer cc = new Spire.PdfViewer.Forms.PdfDocumentViewer(); 
        //string pdfDoc = @"D:\michelle\e-iceblue\Spire.Office.pdf";
        //if (1==1)
        //{
        //    cc.LoadFromFile(@"C:\Users\admin\Documents\visual studio 2010\Projects\ConsoleApplication1\ConsoleApplication1\pdf\1.pdf");
        //}
       
        // Instantiate HTML SaveOptions object
        //Aspose.Words.Document doc = new Aspose.Words.Document(@"C:\Users\admin\Documents\visual studio 2010\Projects\ConsoleApplication1\ConsoleApplication1\pdf\mynew.doc");

        //// Microsoft.Office.Interop.Word.Document pdfDocument = new Microsoft.Office.Interop.Word.Document(@"F:\ExternalTestsData\35940.pdf");
        //string outHtmlFile = @"d:\aaaa.html";
        //// Create HtmlSaveOption with tested feature


        //// Create HtmlSaveOption with tested feature
        //Aspose.Words.Saving.HtmlSaveOptions saveOptions = new Aspose.Words.Saving.HtmlSaveOptions();


      
        //saveOptions.SaveFormat=Aspose.Words.SaveFormat.Html;
        //saveOptions.PrettyFormat = true; ;
      
       

     
        //// Save the output in HTML format
        //string sourceFile = @"F:\ExternalTestsData\36297_36189.pdf";
        Aspose.Pdf.Document testDoc = new Aspose.Pdf.Document(hh);

        Aspose.Pdf.HtmlSaveOptions options = new Aspose.Pdf.HtmlSaveOptions();
        // This is main setting that allows work and testing of tested feature
        options.RasterImagesSavingMode = Aspose.Pdf.HtmlSaveOptions.RasterImagesSavingModes.AsExternalPngFilesReferencedViaSvg;//


      ///  options.CustomResourceSavingStrategy = new Aspose.Pdf.HtmlSaveOptions.ResourceSavingStrategy(Custom_processor_of_embedded_images);

        // Get clean test directory
      
        // Do conversion
        testDoc.Save(hh.Replace(".pdf",".html"), options);
       
         //HttpContext.Current.Response.Write("<script> window.print(); </script>");

         //Get the File Name. Remove space characters from File Name.
       // string fileName = @"C:\Users\admin\Documents\visual studio 2010\Projects\ConsoleApplication1\ConsoleApplication1\pdf\1.pdf";



        Response.Clear();
        Response.ClearHeaders();
        Response.ClearHeaders();
       

       // Response.WriteFile(hh.Replace(".pdf", ".html"));

        string path = hh.Replace(".pdf", ".html");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
        //Save the PDF file.
     //   string inputPath = Server.MapPath("~/Sample PDF/") + "Mudassar Khan.pdf";


     //   //Set the HTML file path.
     //   string outputPath = Server.MapPath("~/Output/") + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".html";

     //   ProcessStartInfo startInfo = new ProcessStartInfo();

     //   //Set the PDF File Path and HTML File Path as arguments.
     //   startInfo.Arguments = string.Format("{0} {1}", inputPath, outputPath);

     //   //Set the Path of the PdfToHtml exe file.
     //   startInfo.FileName = Server.MapPath("~/PdfToHtml/pdftohtml.exe");

     //   //Hide the Command window.
     //   startInfo.WindowStyle = ProcessWindowStyle.Hidden;
     //   startInfo.CreateNoWindow = true;
     //   startInfo.UserName = "admin";
     //   startInfo.UseShellExecute = false;
     ////   startInfo.Arguments = String.Format("/h /t \"{0}\" \"{1}\"", @"C:\Users\admin\Documents\visual studio 2010\Projects\ConsoleApplication1\ConsoleApplication1\pdf\1.pdf", DefaultPrinterName());//file.FullName: full path of PDF file                          startInfo.CreateNoWindow = true;
     //   startInfo.ErrorDialog = true;
    
     //   SecureString pwd = new SecureString();
     //   foreach (char c in "ds".ToCharArray())
     //       pwd.AppendChar(c);
     //   startInfo.Password = pwd;

     //    //Execute the PdfToHtml exe file.
     //    using (Process process = Process.Start(startInfo))
     //    {
     //        process.WaitForExit();
     //    }





    }
}