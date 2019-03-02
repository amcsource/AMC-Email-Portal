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
//using Aspose.Pdf;

public partial class Demo_HtmlToPdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //var htmlContent = String.Format("<body>Hello world: {0}</body>", DateTime.Now);
            //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            //htmlToPdf.GeneratePdf(htmlContent, null, "D:\\NReco\\PDFFile.pdf");


            BAL_AMCPE.EmailTemplates et2 = new BAL_AMCPE.EmailTemplates();
            et2.obj = et2.GetTemplateByID(435);

            string result = HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(et2.obj.Body.Replace("<br />", ""), "<(.|\n)*?>", ""));
            
            FlexiMail ml = new FlexiMail();
            ml.From = "info@menopausecentre.com.au";
            ml.To = "0404072602@sms.managedhealth.com.au";
            ml.CC = "";
            ml.BCC = "";
            ml.Subject = "Test";
            ml.MailBody = result;
            ml.IsBodyHtml = false;
            ml.MailBodyManualSupply = true;
            ml.AttachFile = null;

            ml.Send();




            //var htmlContent = String.Format("<body>{0}</body>", et.obj.Body);
            //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            //htmlToPdf.GeneratePdf(htmlContent, null, "D:\\NReco\\PDFFile.pdf");

            Aspose.Words.License licenseWord = new Aspose.Words.License();
            licenseWord.SetLicense(Server.MapPath("~/Aspose.Words.lic"));

            Aspose.Pdf.License license = new Aspose.Pdf.License();
            license.SetLicense(Server.MapPath("~/Aspose.PDF.lic"));
            

            ////aspose Mthod 1
            //BAL_AMCPE.EmailTemplates et2 = new BAL_AMCPE.EmailTemplates();
            //et2.obj = et2.GetTemplateByID(471);

            //string htmlString = et2.obj.Body;
            //System.Text.RegularExpressions.Regex.Replace(htmlString, @"<(.|\n)*?>", "");


            //// Instantiate an object PDF class
            //Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();
            //// add the section to PDF document sections collection
            //Aspose.Pdf.Generator.Section section = pdf.Sections.Add();

            ////Create text paragraphs containing HTML text

            ////var htmlContent = String.Format("<body>Hello world: {0}</body>", DateTime.Now);
            //var htmlContent = String.Format("<body>{0}</body>", et.obj.Body);
            //Aspose.Pdf.Generator.Text text2 = new Aspose.Pdf.Generator.Text(section, htmlContent);
            //// enable the property to display HTML contents within their own formatting
            //text2.IsHtmlTagSupported = true;
            ////Add the text paragraphs containing HTML text to the section
            //section.Paragraphs.Add(text2);
            //// Specify the URL which serves as images database
            //pdf.HtmlInfo.ImgUrl = "D:/pdftest/MemoryStream/";

            ////Save the pdf document
            //pdf.Save("D:/HTML2pdfASPOSE-2.pdf");


            ////aspose method 2
            //// For complete examples and data files, please go to https://github.com/aspose-pdf/Aspose.Pdf-for-.NET
            //// The path to the documents directory.
            //string dataDir = "D:/html/";

            //// Instantiate an object PDF class
            //Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();

            //// Add the section to PDF document sections collection
            //Aspose.Pdf.Generator.Section section = pdf.Sections.Add();

            //// Read the contents of HTML file into StreamReader object
            //StreamReader r = File.OpenText(dataDir + "index.html");

            //// Create text paragraphs containing HTML text
            //Aspose.Pdf.Generator.Text text2 = new Aspose.Pdf.Generator.Text(section, r.ReadToEnd());

            //// Enable the property to display HTML contents within their own formatting
            //text2.IsHtmlTagSupported = true;

            //text2.IfHtmlTagSupportedOverwriteHtmlFontNames = true;
            //text2.IfHtmlTagSupportedOverwriteHtmlFontSizes = true;

            //// Add the text paragraphs containing HTML text to the section
            //section.Paragraphs.Add(text2);

            //// Specify the URL which serves as images database
            //pdf.HtmlInfo.ImgUrl = dataDir + "images/";

            //// Following properties are added from Aspose.Pdf for .NET 8.4.0
            //pdf.HtmlInfo.BadHtmlHandlingStrategy = BadHtmlHandlingStrategy.TreatAsPlainText;
            //pdf.HtmlInfo.ShowUnknownHtmlTagsAsText = true;

            //// Save the Pdf document
            //pdf.Save(dataDir + "HTML2pdf_out.pdf");



            ////////////////////////////////
            ////HTML TO DOC
            //string myDir = @"D:/HTMLTODOC/";
            //string filename = DateTime.Now.ToString("HH_mm_ss");

            //Document doc = new Document(myDir + "template.html");
            ////doc.Save(myDir + filename + ".docx");

            ////doc = new Document(myDir + filename + ".docx");
            //doc.Save(myDir + filename + ".pdf");



    
            
            
            ////////////////////////////////////
            //HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            //// set the PageInfo,in Aspose.Pdf 1 inch = 72 points
            //loadOptions.PageInfo.Margin.Bottom = 10;
            //loadOptions.PageInfo.Margin.Top = 10;
            //loadOptions.PageInfo.Width = 600;
            //loadOptions.PageInfo.Height = 800;

            //string myDir = @"D:/HTMLTODOC/";
            //string filename = DateTime.Now.ToString("HH_mm_ss");
            //Aspose.Pdf.Document doc = new Aspose.Pdf.Document(myDir + "template-2.html", loadOptions);
            //doc.Save(myDir + filename + ".pdf");



            //////////////////////////

            // HTML string
            string myDir = @"D:/HTMLTODOC/";

            BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
            et.obj = et.GetTemplateByID(475);

            String html = et.obj.Body;

            //// instantiate HtmlLoadOptions object and set desrired properties.
            //HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
            //htmlLoadOptions.PageInfo.Margin.Bottom = 10;
            //htmlLoadOptions.PageInfo.Margin.Top = 10;
            //htmlLoadOptions.PageInfo.Margin.Left = 40;
            //htmlLoadOptions.PageInfo.Margin.Right = 40;
            //htmlLoadOptions.PageInfo.Width = Aspose.Pdf.PageSize.A4.Width;
            //htmlLoadOptions.PageInfo.Height = Aspose.Pdf.PageSize.A4.Height;

            ////Load HTML string
            ////Document doc = new Document(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)), htmlLoadOptions);
            //Aspose.Pdf.Document doc = new Aspose.Pdf.Document(myDir + "template-1.html", htmlLoadOptions);

            //doc.ProcessParagraphs();

            //// Resize contents of resultant PDF

            //int[] page_cnt1 = new int[doc.Pages.Count];

            //for (int i = 0; i < doc.Pages.Count; i++)

            //    page_cnt1[i] = i + 1;

            //PdfFileEditor pfe = new PdfFileEditor();
            //pfe.ResizeContents(doc, page_cnt1, PdfFileEditor.ContentsResizeParameters.PageResize(Aspose.Pdf.PageSize.A4.Width, Aspose.Pdf.PageSize.A4.Height));

            ////Save PDF file
            string filename = DateTime.Now.ToString("HH_mm_ss");
            //doc.Save(myDir + filename + ".pdf");

            

            // convert string to stream
            //byte[] byteArray = Encoding.UTF8.GetBytes(et.obj.Body);
            //MemoryStream stream = new MemoryStream(byteArray);


            //-----------WORKING METHOD 1 START-------------------------------------------

            Aspose.Words.Document wordDoc = new Aspose.Words.Document(myDir + "test-2.html");
            Aspose.Words.PageSetup ps = wordDoc.FirstSection.PageSetup;

            ps.LeftMargin = 0.5 * 72;

            ps.RightMargin = 0.5 * 72;

            ps.TopMargin = 0.5 * 72;

            ps.BottomMargin = 0.5 * 72;

            wordDoc.Save(myDir + filename + ".pdf");

            Aspose.Pdf.Document pdfDoc = new Document(myDir + filename + ".pdf");
            pdfDoc.Encrypt("123", "", Permissions.PrintDocument, CryptoAlgorithm.AESx128);
            pdfDoc.Save(myDir + filename + "1.pdf");

            //-----------WORKING METHOD 1 END-----------------------------------------------


            //////-----------WORKING METHOD 2 START-------------------------------------------
            
            //BAL_AMCPE.EmailTemplates el = new BAL_AMCPE.EmailTemplates();
            //var data = el.GetTemplateDetailByID(455);
            //byte[] byteArray = Encoding.UTF8.GetBytes("<html><head></head><body>" + data.Letter + "</body></html>");


            //using (MemoryStream stream = new MemoryStream(byteArray))
            //{
            //    Aspose.Words.Document wordDoc = new Aspose.Words.Document(stream);
            //    wordDoc.Save(myDir + filename + ".pdf");

            //    //Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(myDir + filename + ".docx");
            //    //pdfDoc.Encrypt("1", "1", 0, Aspose.Pdf.CryptoAlgorithm.AESx128);
            //    //pdfDoc.Save(myDir + filename + ".pdf");
            //}

            //////-----------WORKING METHOD 2 END-------------------------------------------

        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        //string result = HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace("Hi [field: 1.Patient First Name], due to a system error your account was double charged. This issue has been resolved and we have processed a refund. Please contact us if you have any further queries. We apologise for the inconvenience. AMC [field: 1.Patient Number] new line space some more testing  and more testing www.themc.com.au  hsuifsdhfiuhiuhiu@iuhiulo.com  04040 07343  4231423  sdf sd f gfd g fsd", "<(.|\n)*?>", ""));
        string result = HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(txtMessage.Text, "<(.|\n)*?>", ""));

        FlexiMail ml = new FlexiMail();
        ml.From = "info@menopausecentre.com.au";
        ml.To = "0404072602@sms.managedhealth.com.au";
        ml.CC = "";
        ml.BCC = "";
        ml.Subject = "Test";
        ml.MailBody = result;
        ml.IsBodyHtml = false;
        ml.MailBodyManualSupply = true;
        ml.AttachFile = null;

        ml.Send();
    }
}