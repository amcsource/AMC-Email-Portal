using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
//using Aspose.Pdf;
//using Aspose.Pdf.Generator;

//using Aspose.Words;

using iTextSharp.text.pdf;
using Aspose.Pdf;
using Aspose.Pdf.Facades;
using System.Drawing;
//using Aspose.Pdf;

public partial class Demo_HtmlToPdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Aspose.Words.License licenseWord = new Aspose.Words.License();
            licenseWord.SetLicense(Server.MapPath("~/Aspose.Words.lic"));

            Aspose.Pdf.License license = new Aspose.Pdf.License();
            license.SetLicense(Server.MapPath("~/Aspose.PDF.lic"));

            string myDir = @"D:/Docs/";

            ////Save PDF file
            string filename = DateTime.Now.ToString("HH_mm_ss");

            //Image to PDF
            // Instantiate Document Object
            Document doc = new Document();
            // Add a page to pages collection of document
            Aspose.Pdf.Page page = doc.Pages.Add();
            // Load the source image file to Stream object
            FileStream fs = new FileStream(myDir + "test.tiff", FileMode.Open, FileAccess.Read);
            byte[] tmpBytes = new byte[fs.Length];
            fs.Read(tmpBytes, 0, Convert.ToInt32(fs.Length));

            MemoryStream mystream = new MemoryStream(tmpBytes);
            // Instantiate BitMap object with loaded image stream
            Bitmap b = new Bitmap(mystream);

            // Set margins so image will fit, etc.
            page.PageInfo.Margin.Bottom = 0;
            page.PageInfo.Margin.Top = 0;
            page.PageInfo.Margin.Left = 0;
            page.PageInfo.Margin.Right = 0;

            page.CropBox = new Aspose.Pdf.Rectangle(0, 0, b.Width, b.Height);
            // Create an image object
            Aspose.Pdf.Image image1 = new Aspose.Pdf.Image();
            // Add the image into paragraphs collection of the section
            page.Paragraphs.Add(image1);
            // Set the image file stream
            image1.ImageStream = mystream;
            // Save resultant PDF file

            doc.Encrypt("123", "123", Permissions.PrintDocument, CryptoAlgorithm.RC4x128);
            doc.Save( myDir + filename + "_image.pdf");

            // Close memoryStream object
            mystream.Close();

            //Image to PDF end

            //Doc to PDF
            Aspose.Words.Document wordDoc = new Aspose.Words.Document(myDir + "Patient Portal.docx");
            Aspose.Words.PageSetup ps = wordDoc.FirstSection.PageSetup;

            ps.LeftMargin = 0.5 * 72;

            ps.RightMargin = 0.5 * 72;

            ps.TopMargin = 0.5 * 72;

            ps.BottomMargin = 0.5 * 72;
            wordDoc.Save(myDir + filename + ".pdf");

            Aspose.Pdf.Document pdfDoc = new Document(myDir + filename + ".pdf");
            pdfDoc.Encrypt("123", "123", Permissions.PrintDocument, CryptoAlgorithm.RC4x128);
            pdfDoc.Save(myDir + filename + "_doc.pdf");
            //Doc to PDF end

            //Just encrypt PDF
            Aspose.Pdf.Document pdfDoc2 = new Document(myDir + "normal_pdf.pdf");
            pdfDoc2.Encrypt("123", "123", Permissions.PrintDocument, CryptoAlgorithm.RC4x128);
            pdfDoc2.Save(myDir + "normal_pdf.pdf");
            //Just encrypt PDF END


        }
    }
}