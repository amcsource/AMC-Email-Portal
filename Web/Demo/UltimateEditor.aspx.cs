using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Karamasoft.WebControls.UltimateEditor;

public partial class Demo_HtmlToPdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
            et.obj = et.GetTemplateByID(471);
            UltimateEditor1.EditorHtml = et.obj.Body;
        }
    }

    protected void UltimateEditor1_ToolbarItemRender(object sender, ToolbarItemRenderEventArgs e)
    {
        if (e.ToolbarItem.ID == "InsertPageBreak")
        {
            e.ToolbarItem.OnBeforeClick = "AppendToHistory('InsertPageBreak', 'OnBeforeClick'); InsertPageBreakBeforeClick();";
            e.ToolbarItem.OnAfterClick = "AppendToHistory('InsertPageBreak', 'OnAfterClick');";
        }
        else if (e.ToolbarItem.ID == "InsertHTML")
        {
            DropDownItem ddi;

            ddi = new DropDownItem();
            ddi.ItemValue = "";
            ddi.ItemText = "Insert HTML";
            e.ToolbarItem.DropDown.Add(ddi);

            ddi = new DropDownItem();
            ddi.ItemValue = "John Doe<br>Support Team<br>";
            ddi.ItemText = "Signature";
            e.ToolbarItem.DropDown.Add(ddi);

            ddi = new DropDownItem();
            ddi.ItemValue = "Acme Corporation<br>123 Main St<br>Los Angeles, 90210<br>";
            ddi.ItemText = "Company Address";
            e.ToolbarItem.DropDown.Add(ddi);

            ddi = new DropDownItem();
            ddi.ItemValue = "<img src='Acme.gif' alt='Acme Corporation'>";
            ddi.ItemText = "Company Logo";
            e.ToolbarItem.DropDown.Add(ddi);

            e.ToolbarItem.OnBeforeChange = "AppendToHistory('InsertHTML', 'OnBeforeChange'); InsertHTMLBeforeChange();";
            e.ToolbarItem.OnAfterChange = "AppendToHistory('InsertHTML', 'OnAfterChange'); InsertHTMLAfterChange();";
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string html = UltimateEditor1.EditorHtml;
        //string source = UltimateEditor1.EditorSource;

        string pdfFile = @"D:KarmaSoftUE\EditorContent2.pdf";
        HtmlToPdf htmlToPdf = new HtmlToPdf(UltimateEditor1);
        string htmlStr = htmlToPdf.HtmlString;

        //htmlToPdf.PdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
        //htmlToPdf.PdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
        //htmlToPdf.PdfConverter.PdfDocumentOptions.ShowHeader = true;
        //htmlToPdf.PdfConverter.PdfDocumentOptions.ShowFooter = true;
        htmlToPdf.PdfConverter.PdfDocumentOptions.LeftMargin = 5;
        htmlToPdf.PdfConverter.PdfDocumentOptions.RightMargin = 5;
        htmlToPdf.PdfConverter.PdfDocumentOptions.TopMargin = 5;
        htmlToPdf.PdfConverter.PdfDocumentOptions.BottomMargin = 5;
        htmlToPdf.PdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
        //htmlToPdf.PdfConverter.PdfHeaderOptions.HeaderText = "Sample header: " + TxtURL.Text;
        ///htmlToPdf.PdfConverter.PdfHeaderOptions.HeaderTextColor = System.Drawing.Color.Blue;
        htmlToPdf.PdfConverter.PdfHeaderOptions.HeaderSubtitleText = string.Empty;
        htmlToPdf.PdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
        htmlToPdf.PdfConverter.PdfFooterOptions.FooterText = "Sample footer";
        htmlToPdf.PdfConverter.PdfFooterOptions.FooterTextColor = System.Drawing.Color.Blue;
        htmlToPdf.PdfConverter.PdfFooterOptions.DrawFooterLine = false;
        //htmlToPdf.PdfConverter.PdfFooterOptions.PageNumberText = "Page";
        //htmlToPdf.PdfConverter.PdfFooterOptions.ShowPageNumber = true;

        htmlToPdf.PdfConverter.SavePdfFromHtmlStringToFile(htmlStr, pdfFile, Page.Request.Url.ToString());
    }
}