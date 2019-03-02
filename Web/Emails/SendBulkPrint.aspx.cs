using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BAL_AMCPE;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using WordToPDF;
using System.Xml;
using DAL_AMCPE;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using Neodynamic;
using Aspose.Words;
//using Aspose.Pdf.InteractiveFeatures;
using System.Text.RegularExpressions;
using Microsoft.Office.Tools.Word;
using System.Drawing;


public partial class SendBulkPrint : System.Web.UI.Page
{
    /// <summary>
    /// /////////////////////////////////
    /// </summary>
    /// 

    string dd = "";
    [DllImport("Kernel32.dll")]
    static extern IntPtr CreateFile(string filename,
    [MarshalAs(UnmanagedType.U4)]FileAccess fileaccess,
    [MarshalAs(UnmanagedType.U4)]FileShare fileshare, int securityattributes,
    [MarshalAs(UnmanagedType.U4)]FileMode creationdisposition, int flags,
    IntPtr template);
    // //////////
    public int pageSize = 25, PageRange = 5;
    public string sortExpression = "order by PatientName asc, purpose asc", searchKeyword = "", patientType = "0";
    public int TemplateId, EmailId;
    public string templateName, templatePath, user, from, to, cc, bcc, subject, body, patientRecId, XSORecID, presType, patientNumber, patientFullName, businessFilter, businessInclude;
    public bool hasBusinessAttachment, selectAllAttachments;

    public List<Attachments> attachments, patientAttachments;

    // File Path variables
    string fileName, sourcePath, targetPath;

    public int totalMessagesToBeSent = 0, totalMessagesSent = 0, totalMessagesNotSentToMissing = 0;

    Timer timeoutTimer;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //  Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();
            if (!IsPostBack)
            {
                Aspose.Pdf.Facades.PdfFileEditor pdfEditor = new Aspose.Pdf.Facades.PdfFileEditor();
                //pdfEditor.Concatenate(new string[] { @"D:\Attachments\5f4956110e7642c1a87f46d3b8816cb2\Ahmz_unkown_Pharmacy Work_Sheet_Template.pdf" }, "E:\\Project\\AMC_Email\\Web\\Output\\rohit.pdf");

                //pdfEditor.Concatenate(@"E:\Project\AMC_Email\Web\Output\636044359915836690.pdf", @"E:\Project\AMC_Email\Web\Output\636044358592360992.pdf", @"E:\Project\AMC_Email\Web\Output\rohit.pdf");
                //pdfEditor.Concatenate(@"E:\Project\AMC_Email\Web\Output\rohit.pdf", @"E:\Project\AMC_Email\Web\Output\636042103304480786.pdf", @"E:\Project\AMC_Email\Web\Output\rohit.pdf");


                TemplateId = EmailId = 0;
                user = "";
                from = to = cc = bcc = subject = body = "";

                patientRecId = patientNumber = presType = "";
                businessFilter = businessInclude = "";
                hasBusinessAttachment = selectAllAttachments = false;

                message.Visible = messageNotSent.Visible = false;

                if (Session["CurrentPageBE"] != null)
                {
                    CurrentPage = Convert.ToInt16(Session["CurrentPageBE"]);
                    Session["CurrentPageBE"] = null;
                }

                if (!string.IsNullOrWhiteSpace(Convert.ToString(Session["UserId"])))
                {
                    user = Convert.ToString(Session["UserId"]).ToLower().Replace("amc\\", "");
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
                {
                    user = Convert.ToString(Request.QueryString["UserId"]).ToLower().Replace("amc\\", "");
                    Session["UserId"] = Convert.ToString(Request.QueryString["UserId"]);
                }
                else
                    Response.Redirect("~/SessionExpired.aspx");


                GetEmailTemplatesByCategoryId(0);

                BindData();

                // set hidden variables so that we may get values at post back
                hdnTemplateId.Value = TemplateId.ToString();
                hdnEmailId.Value = EmailId.ToString();
                hdnCurrentUser.Value = user;
                hdnPatientNumber.Value = Convert.ToString(patientNumber);
                hdnPatientRecId.Value = patientRecId;
                hdnPresType.Value = presType;
                hdnPatientFullName.Value = patientFullName;
                hdnbusinessFilter.Value = businessFilter;
                hdnbusinessInclude.Value = businessInclude;
                hdnhasBusinessAttachment.Value = Convert.ToString(hasBusinessAttachment);
                hdnselectAllAttachments.Value = Convert.ToString(selectAllAttachments);

                //hdnAttachingMore.Value = Convert.ToString(attachingMore);
                hdnFrom.Value = from;
                hdnTo.Value = to;
                hdnCc.Value = cc;
                hdnBcc.Value = bcc;
                hdnSubject.Value = subject;
                hdnBody.Value = body;

                //// delete any files from user temp folder which might be left as garbage or when email was not sent.
                //DeleteFilesFromUserTempDirectory();
            }
            else
            {
                TemplateId = Convert.ToInt32(hdnTemplateId.Value);
                EmailId = Convert.ToInt32(hdnEmailId.Value);
                user = hdnCurrentUser.Value;
                patientNumber = hdnPatientNumber.Value;
                patientRecId = hdnPatientRecId.Value;
                presType = hdnPresType.Value;
                patientFullName = hdnPatientFullName.Value;
                businessFilter = hdnbusinessFilter.Value;
                businessInclude = hdnbusinessInclude.Value;
                hasBusinessAttachment = Convert.ToBoolean(hdnhasBusinessAttachment.Value);
                selectAllAttachments = Convert.ToBoolean(hdnselectAllAttachments.Value);

                from = hdnFrom.Value;
                to = hdnTo.Value;
                cc = hdnCc.Value;
                bcc = hdnBcc.Value;
                subject = hdnSubject.Value;
                body = hdnBody.Value;
            }
        }
        catch (System.IO.FileNotFoundException ex)
        {
            Response.Write(ex.Message + ex.InnerException + ex.StackTrace + ex.Source);
        }
    }

    private void GetEmailTemplatesByCategoryId(int categoryId)
    {
        BAL_AMCPE.DocumentTemplates etc1 = new BAL_AMCPE.DocumentTemplates();
        ddlTemplates.DataSource = etc1.GetAllDocTemplates();
        ddlTemplates.DataTextField = "TemplateName";
        ddlTemplates.DataValueField = "Id";
        ddlTemplates.DataBind();
        ddlTemplates.Items.Insert(0, new ListItem("Please choose template", "0"));




        //    EmailTemplates et = new EmailTemplates();
        //    ddlTemplates.DataSource = et.GetEmailTemplatesByCategoryID(categoryId);
        //    ddlTemplates.DataTextField = "Name";
        //    ddlTemplates.DataValueField = "Id";
        //    ddlTemplates.DataBind();
        //    ddlTemplates.Items.Insert(0, new ListItem("Please choose template", "0"));
    }

    private void BindData()
    {



        searchKeyword = txtSearch.Text;
        pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        patientType = ddlPatientType.SelectedValue;
        /////////////////////////

        //prescription_parent = ddlxpres.SelectedValue;
        //BAL_AMCPE.DocumentTemplates etc3 = new BAL_AMCPE.DocumentTemplates();
        //List<xsoPrescription> hhh = etc3.GetTemplateReplaceKw(patientType);

        //ListtoDataTableConverter converter = new ListtoDataTableConverter();
        //DataSet ds = new DataSet();
        //ds.Tables.Add(converter.ToDataTable(hhh));


        /////////////////
        BAL_AMCPE.DocumentTemplates et = new BAL_AMCPE.DocumentTemplates();
        var data = et.GetPatientsForBulkPrint(patientType, CurrentPage + 1, pageSize, sortExpression, searchKeyword);
        if (data.Count > 0)
        {
            rptPatients.DataSource = data;
            norecord.Visible = false;
            dvSendEmail.Visible = true;
            TotalPages = Convert.ToInt32(data.ToList().Count);
        }
        else
        {
            rptPatients.DataSource = null;
            norecord.Visible = true;
            dvSendEmail.Visible = false;
            TotalPages = 0;
        }
        rptPatients.DataBind();
        if (TotalPages > 1)
            BindRptPagination(TotalPages);
        else
            pnlPagination.Visible = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        BindData();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        CurrentPage = 0;
        BindData();
    }
    public Int32 TotalPages
    {
        get
        {
            object o = ViewState["_TotalPage"];
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (Int32)o;
            }
        }
        set { ViewState["_TotalPage"] = value; }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.DocumentTemplates et = new BAL_AMCPE.DocumentTemplates();
        DAL_AMCPE.DocumentTemplate ed = et.GetDocTemplateByID(Convert.ToInt32(ddlTemplates.SelectedValue));

        TemplateId = ed.Id;
        templatePath = ed.TemplatePath;

        int filecount = 1;
        System.IO.FileInfo[] allPDFs = null;
        string Merged_pdf_filepath = "";
        if (File.Exists(templatePath))
        {

            //  SendToPrinter(path.Replace(".doc", ".pdf"));
            int count = 0;
            CheckBox chkEmailAll = (CheckBox)rptPatients.Controls[0].Controls[0].FindControl("chkEmailAll");
            string parent_type = Convert.ToString("xf" + ddlPatientType.SelectedValue);

            BAL_AMCPE.DocumentTemplates cc = new BAL_AMCPE.DocumentTemplates();
            Word2Pdf objWorPdf;
            GMEEDevelopmentEntities ATCH = new GMEEDevelopmentEntities();

            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense(Server.MapPath("~/Aspose.Words.lic"));
            Aspose.Words.Document doc;

            if (chkEmailAll.Checked)
            {
                BAL_AMCPE.DocumentTemplates bulkEmail = new BAL_AMCPE.DocumentTemplates();
                var data = bulkEmail.GetPatientsForBulkPrint(ddlPatientType.SelectedValue, 1, 0, sortExpression, txtSearch.Text);

                if (data.Count > 0)
                {
                    filecount = data.Count;
                    allPDFs = new System.IO.FileInfo[filecount];

                    foreach (var item in data)
                    {
                        string path_pdf = "";
                        totalMessagesToBeSent++;

                        patientRecId = item.PatientRecId;
                        patientNumber = item.PatientNumber;
                        patientFullName = item.PatientName;
                        XSORecID = item.XSORecID;
                        presType = item.PresType;

                        //patientRecId = "AB050B0622174793ACDAB3918CF76E44";
                        //patientNumber = "97457"; // ((Literal)i.FindControl("ltlPatientNumber")).Text;
                        //patientFullName = "Deborah Reiher"; //((Literal)i.FindControl("ltlPatientName")).Text;
                        //XSORecID = "96176AF662DB43D8BC537D0494C896AD"; // ((HiddenField)i.FindControl("hdnXSORecID")).Value;
                        //presType = "Prescription1"; // ((HiddenField)i.FindControl("hdnPresType")).Value;
                        

                        FileInfo fileinfo = new FileInfo(templatePath);
                        string filename = fileinfo.Name.Replace(".docx", "").Replace(".doc", "");

                        string id = Guid.NewGuid().ToString().Replace("-", "");
                        string DESTfile = Convert.ToString(Convert.ToString(patientFullName + "_" + filename)) + ".doc";

                        string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;
                        //Dictionary<string, string> replace_dict = et.ReadDocFileToString_V2(templatePath, patientRecId, patientNumber, savePath, DESTfile, presType);
                        //path_pdf = et.ReadMsWord(templatePath, savePath, DESTfile, replace_dict);

                        //18.07.2016
                        path_pdf = et.ReadDocFileToString_V2(templatePath, patientRecId, patientNumber, savePath, DESTfile, presType);

                        if (!string.IsNullOrWhiteSpace(path_pdf))
                        {
                            try
                            {
                                string OutputLocation = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                                doc = new Aspose.Words.Document(path_pdf);
                                doc.Save(OutputLocation);

                                //objWorPdf = new Word2Pdf();
                                //objWorPdf.InputLocation = path_pdf;
                                //objWorPdf.OutputLocation = path_pdf.Replace(".doc", ".pdf");
                                //objWorPdf.Word2PdfCOnversion();

                                et.delete_tempfile();
                                Session["fileinfo"] = path_pdf.Replace(".doc", ".pdf");

                                FileInfo fileidnfo = new FileInfo(OutputLocation);
                                allPDFs[count] = fileidnfo;

                                ATCH.obj_sp_BulkPrint_Attachment(XSORecID, parent_type, patientRecId, @"amc\" + user, OutputLocation, Path.GetFileName(OutputLocation), id, "", "N/A");
                                count++;
                            }
                            catch (Exception ex)
                            { 
                                
                            }
                        }
                    }

                    Merged_pdf_filepath = et.MergeAllPDF(allPDFs, Server.MapPath("~/Output/") + DateTime.Now.Ticks.ToString() + ".pdf");
                    Aspose.Pdf.Document testDoc = new Aspose.Pdf.Document(Merged_pdf_filepath);
                }
                //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('File Updated for " + count + " users Successsfully!');", true);
            }
            else
            {
                filecount = rptPatients.Items.Count;
                allPDFs = new System.IO.FileInfo[filecount];
                foreach (RepeaterItem i in rptPatients.Items)
                {
                    CheckBox chkEmail = (CheckBox)i.FindControl("chkEmail");
                    if (chkEmail.Checked)
                    {
                        string path_pdf = "";
                        totalMessagesToBeSent++;

                        //patientRecId = "9B66132FED754B829FD635B3B038BEF0";
                        //patientNumber = "655506"; // ((Literal)i.FindControl("ltlPatientNumber")).Text;
                        //patientFullName = "Anna Attard"; //((Literal)i.FindControl("ltlPatientName")).Text;
                        //XSORecID = "300C106F36F34D59A3B41D57AD0BFF02"; // ((HiddenField)i.FindControl("hdnXSORecID")).Value;
                        //presType = "Prescription1"; // ((HiddenField)i.FindControl("hdnPresType")).Value;

                        patientRecId = ((HiddenField)i.FindControl("hdnPatientRecId")).Value;
                        patientNumber = ((Literal)i.FindControl("ltlPatientNumber")).Text;
                        patientFullName = ((Literal)i.FindControl("ltlPatientName")).Text;
                        XSORecID = ((HiddenField)i.FindControl("hdnXSORecID")).Value;
                        presType = ((HiddenField)i.FindControl("hdnPresType")).Value;

                        FileInfo fileinfo = new FileInfo(templatePath);
                        string filename = fileinfo.Name.Replace(".docx", "").Replace(".doc", "");

                        string id = Guid.NewGuid().ToString().Replace("-", "");
                        string DESTfile = Convert.ToString(Convert.ToString(patientFullName + "_" + filename)) + ".doc";

                        string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;
                        //Dictionary<string, string> replace_dict = et.ReadDocFileToString(templatePath, patientRecId, patientNumber, savePath, DESTfile, presType);
                        //path_pdf = et.ReadMsWord(templatePath, savePath, DESTfile, replace_dict);

                        //18.07.2016
                        path_pdf = et.ReadDocFileToString_V2(templatePath, patientRecId, patientNumber, savePath, DESTfile, presType);

                        if (!string.IsNullOrWhiteSpace(path_pdf))
                        {
                            try
                            {
                                string OutputLocation = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                                doc = new Aspose.Words.Document(path_pdf);
                                doc.Save(OutputLocation);

                                //objWorPdf = new Word2Pdf();
                                //objWorPdf.InputLocation = path_pdf;
                                //objWorPdf.OutputLocation = path_pdf.Replace(".doc", ".pdf");
                                //objWorPdf.Word2PdfCOnversion();

                                et.delete_tempfile();
                                Session["fileinfo"] = path_pdf.Replace(".doc", ".pdf");
                                FileInfo fileidnfo = new FileInfo(OutputLocation);
                                allPDFs[count] = fileidnfo;

                                ATCH.obj_sp_BulkPrint_Attachment(XSORecID, parent_type, patientRecId, @"amc\" + user, OutputLocation, Path.GetFileName(OutputLocation), id, "", "N/A");
                                count++;
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex.Message);
                            }
                        }
                    }
                }

                Merged_pdf_filepath = et.MergeAllPDF(allPDFs, Server.MapPath("~/Output/") + DateTime.Now.Ticks.ToString() + ".pdf");
                Aspose.Pdf.Document testDoc1 = new Aspose.Pdf.Document(Merged_pdf_filepath);
            }

            BindData();
            btnSend.Text = "Print";

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "attachment;filename=Print.pdf");
            response.WriteFile(Merged_pdf_filepath);

            //Response.AddHeader("Refresh", "5; " + Request.RawUrl);
            Response.AddHeader("Refresh", "3");
            
            //Response.AddHeader("http-equiv", "Refresh");
            //Response.AddHeader("content", "5");


            //response.Flush();
            //Response.End();

            //Response.Redirect("~/Emails/SendBulkPrint.aspx");
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Template File Not Found!');", true);
        }
    }

    private void timeoutTimer_Tick(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl);
    }

    public static void PrintWebControl(string Script)
    {
        StringWriter stringWrite = new StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);

        Page pg = new Page();
        pg.EnableEventValidation = false;
        if (Script != string.Empty)
        {
            pg.ClientScript.RegisterStartupScript(pg.GetType(), "PrintJavaScript", Script);
        }
        HtmlForm frm = new HtmlForm();
        pg.Controls.Add(frm);
        frm.Attributes.Add("runat", "server");

        pg.DesignerInitialize();
        pg.RenderControl(htmlWrite);
        string strHTML = Script.ToString();

        // HttpContext.Current.Response.Clear();
        // HttpContext.Current.Response.Write(strHTML);

        HttpContext.Current.Response.Write("<script> window.open('http://localhost:51577/Web/Emails/pdf.aspx?file='" + Script + "'',); </script>");
        // HttpContext.Current.Response.End();

    }
    private void UpdateTemplateWithPatientDetail(int templateId, EmailData ed)
    {
        from = Helper.ProcessData(ed.From, patientRecId, patientNumber); //ed.From;
        to = Helper.ProcessData(ed.To, patientRecId, patientNumber); //ed.To;
        cc = !string.IsNullOrWhiteSpace(ed.Cc) ? Helper.ProcessData(ed.Cc, patientRecId, patientNumber) : ""; //ed.Cc;
        bcc = !string.IsNullOrWhiteSpace(ed.Bcc) ? Helper.ProcessData(ed.Bcc, patientRecId, patientNumber) : ""; //ed.Bcc;
        subject = Helper.ProcessData(ed.Subject, patientRecId, patientNumber); //ed.Subject;
        body = Helper.ProcessData(ed.Body, patientRecId, patientNumber); //ed.Body;

        //Get patient specific attachments
        patientAttachments = new List<Attachments>();
        if (attachments.Count > 0)
        {
            patientAttachments.AddRange(attachments);
        }

        try
        {
            if (templateId > 0 && hasBusinessAttachment == true) //Business objects
            {
                BAttachments attach = new BAttachments();


                string[] occPer = businessFilter.Split('%');

                List<BAL_AMCPE.BAttachments.AttachmentFile> attachFiles = new List<BAttachments.AttachmentFile>();

                if (businessFilter.Contains("!"))
                {
                    List<BAL_AMCPE.BAttachments.AttachmentFile> attachFile = Helper.ProcessDataBusiness(businessFilter.Replace("!", ""), patientRecId, patientNumber);
                    if (attachFile.Count > 0)
                    {
                        attachFiles.AddRange(attachFile);
                    }
                }
                else
                {
                    if (occPer.Count() - 1 == 2)
                    {
                        businessFilter = Helper.Between(businessFilter, "%", "%");
                    }

                    businessFilter = Helper.ProcessData(businessFilter, patientRecId, patientNumber);
                    //List<string> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                    attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                }

                //Old logic, commented on 31.07.2017
                ////businessFilter = Helper.Between(businessFilter, "%", "%");
                //businessFilter = Helper.ProcessData(businessFilter, patientRecId, patientNumber);
                ////List<string> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                //List<BAL_AMCPE.BAttachments.AttachmentFile> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);

                string sourcePathFile = string.Empty;

                if (attachFiles.Count > 0)
                {
                    DirectoryInfo di;
                    FileInfo[] myfiles;
                    string[] sizes = { "Bytes", "KB", "MB", "GB" };
                    double len;
                    int order;

                    //foreach (string aF in attachFiles)
                    foreach (BAL_AMCPE.BAttachments.AttachmentFile aF in attachFiles)
                    {
                        sourcePathFile = WebConfigurationManager.AppSettings["Server03BusinessAttachments"] + aF.RecId;
                        di = new DirectoryInfo(sourcePathFile);
                        myfiles = di.GetFiles("*");

                        string userTempDir = "~/TempFiles/" + user + "/";
                        string savePath = Server.MapPath(userTempDir);
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }

                        Aspose.Words.License licenseWord = new Aspose.Words.License();
                        licenseWord.SetLicense(Server.MapPath("~/Aspose.Words.lic"));

                        Aspose.Pdf.License license = new Aspose.Pdf.License();
                        license.SetLicense(Server.MapPath("~/Aspose.PDF.lic"));

                        //get patient dob and set as password
                        PatientActivity pAct = new PatientActivity();
                        string password = pAct.GetPatientDOBByPatientNumber(patientNumber);

                        foreach (var s in myfiles)
                        {
                            len = s.Length;
                            order = 0;
                            while (len >= 1024 && order + 1 < sizes.Length)
                            {
                                order++;
                                len = len / 1024;
                            }

                            string sourceFile = Path.Combine(sourcePathFile, s.Name);
                            string destFile = Path.Combine(savePath, s.Name);
                            File.Copy(sourceFile, destFile, true);

                            string fileName = s.Name;
                            if (Convert.ToBoolean(aF.Encrypt))
                            {
                                if (fileName.ToLower().EndsWith(".tiff") || fileName.ToLower().EndsWith(".doc") || fileName.ToLower().EndsWith(".docx") || fileName.ToLower().EndsWith(".pdf"))
                                {
                                    if (fileName.ToLower().EndsWith(".tiff"))
                                    {
                                        //Image to PDF
                                        // Instantiate Document Object
                                        Aspose.Pdf.Document doc = new Aspose.Pdf.Document();
                                        // Add a page to pages collection of document
                                        Aspose.Pdf.Page page = doc.Pages.Add();
                                        // Load the source image file to Stream object
                                        FileStream fs = new FileStream(destFile, FileMode.Open, FileAccess.Read);
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

                                        doc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.RC4x128);
                                        fileName = fileName.Replace(".tiff", "") + ".pdf";
                                        doc.Save(Path.Combine(savePath, fileName));

                                        try
                                        {
                                            //delete .tiff file
                                            File.Delete(destFile);
                                        }
                                        catch (Exception) { }

                                        // Close memoryStream object
                                        mystream.Close();

                                        //Image to PDF end
                                    }
                                    else if (fileName.ToLower().EndsWith(".doc") || fileName.ToLower().EndsWith(".docx"))
                                    {
                                        //Doc to PDF
                                        Aspose.Words.Document wordDoc = new Aspose.Words.Document(destFile);
                                        Aspose.Words.PageSetup ps = wordDoc.FirstSection.PageSetup;

                                        ps.LeftMargin = 0.5 * 72;

                                        ps.RightMargin = 0.5 * 72;

                                        ps.TopMargin = 0.5 * 72;

                                        ps.BottomMargin = 0.5 * 72;

                                        string fileNameDelete = fileName.Replace(".docx", "").Replace(".doc", "") + "_delete.pdf";
                                        wordDoc.Save(Path.Combine(savePath, fileNameDelete));

                                        Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(Path.Combine(savePath, fileNameDelete));
                                        pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.RC4x128);

                                        fileName = fileName.Replace(".docx", "").Replace(".doc", "") + ".pdf";
                                        pdfDoc.Save(Path.Combine(savePath, fileName));

                                        //delete temp and doc/docx file
                                        try
                                        {
                                            File.Delete(destFile);
                                            File.Delete(Path.Combine(savePath, fileNameDelete));
                                        }
                                        catch (Exception ex) { }

                                        //Doc to PDF end
                                    }
                                    else if (fileName.ToLower().EndsWith(".pdf"))
                                    {
                                        Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(destFile);
                                        pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.RC4x128);
                                        pdfDoc.Save(destFile);
                                    }
                                }
                            }

                            
                            patientAttachments.Add(new Attachments { Name = fileName, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fileName, Include = true });
                            hdnAttachURLs.Value += s.FullName + "|";
                        }
                    }


                    //foreach (BAL_AMCPE.BAttachments.AttachmentFile aF in attachFiles)
                    //{
                    //    sourcePathFile = WebConfigurationManager.AppSettings["Server03BusinessAttachments"] + aF.RecId;
                    //    di = new DirectoryInfo(sourcePathFile);
                    //    myfiles = di.GetFiles("*");

                    //    //string savePath = Server.MapPath("~/TempFiles/");
                    //    string userTempDir = "~/TempFiles/" + user + "/";
                    //    string savePath = Server.MapPath(userTempDir);
                    //    if (!Directory.Exists(savePath))
                    //    {
                    //        Directory.CreateDirectory(savePath);
                    //    }

                    //    foreach (var s in myfiles)
                    //    {
                    //        len = s.Length;
                    //        order = 0;
                    //        while (len >= 1024 && order + 1 < sizes.Length)
                    //        {
                    //            order++;
                    //            len = len / 1024;
                    //        }

                    //        string sourceFile = Path.Combine(sourcePathFile, s.Name);
                    //        string destFile = Path.Combine(savePath, s.Name);
                    //        File.Copy(sourceFile, destFile, true);

                    //        patientAttachments.Add(new Attachments { Name = s.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, Include = true });
                    //        hdnAttachURLs.Value += s.FullName + "|";
                    //    }
                    //}
                }
            }
        }
        catch (Exception ex)
        { }

        if (patientAttachments.Count > 0)
            rptAttachments.DataSource = patientAttachments;
        else
            rptAttachments.DataSource = null;
        rptAttachments.DataBind();
    }

    private void GetAttachmentFiles(string filePath, int templateId = 0, bool hasBusinessAttachment = false, string businessFilter = "", string businessInclude = "", bool promptForAttachments = false, bool attachmentHasDirectory = false, string directoryPath = "", string directoryFilter = "", string directoryInclude = "")
    {
        try
        {
            hdnAttachURLs.Value = "";
            string sourcePath = Server.MapPath(filePath);
            FileInfo[] myfiles = new FileInfo[] { };
            DirectoryInfo di;
            string[] sizes = { "Bytes", "KB", "MB", "GB" };
            double len;
            int order;

            attachments = new List<Attachments>();

            if (Directory.Exists(sourcePath)) //email template Browsed Files
            {
                di = new DirectoryInfo(sourcePath);
                myfiles = di.GetFiles("*");

                foreach (var file in myfiles)
                {
                    len = file.Length;
                    order = 0;
                    while (len >= 1024 && order + 1 < sizes.Length)
                    {
                        order++;
                        len = len / 1024;
                    }
                    attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = sourcePath, FileWebURL = "", Include = true });

                    //insert url of every being attached in list
                    //attachURLs.Add(file.FullName);
                    hdnAttachURLs.Value += file.FullName + "|";
                }
            }

            try
            {
                if (attachmentHasDirectory) //Directory Objects
                {
                    string sourcePathFile = string.Empty;

                    sourcePathFile = directoryPath;
                    di = new DirectoryInfo(sourcePathFile);
                    myfiles = di.GetFiles(directoryFilter);

                    //string savePath = Server.MapPath("~/TempFiles/");
                    string userTempDir = "~/TempFiles/" + user + "/";
                    string savePath = Server.MapPath(userTempDir);
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    if (myfiles.Count() > 0)
                    {
                        if (directoryInclude == "Include Latest")
                            myfiles = myfiles.OrderByDescending(a => a.LastWriteTime).Take(1).ToArray();
                        else if (directoryInclude == "Include Oldest")
                            myfiles = myfiles.OrderBy(a => a.LastWriteTime).Take(1).ToArray();
                    }

                    foreach (var s in myfiles)
                    {
                        len = s.Length;
                        if (s.Length > 0)
                        {
                            order = 0;
                            while (len >= 1024 && order + 1 < sizes.Length)
                            {
                                order++;
                                len = len / 1024;
                            }

                            string sourceFile = Path.Combine(sourcePathFile, s.Name);
                            string destFile = Path.Combine(savePath, s.Name);
                            File.Copy(sourceFile, destFile, true);

                            string attachedFilePath = filePath.Replace("~", "");

                            attachments.Add(new Attachments { Name = s.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + s.Name, Include = true });
                            hdnAttachURLs.Value += s.FullName + "|";
                        }
                    }
                }
            }
            catch (Exception ex) { }


            if (attachments.Count > 0)
                rptAttachments.DataSource = attachments;
            else
                rptAttachments.DataSource = null;
            rptAttachments.DataBind();

        }
        catch (Exception ex)
        {

        }
    }



    //private void GetAttachmentFiles(string filePath, int templateId = 0, bool hasBusinessAttachment = false, string businessFilter = "", string businessInclude = "")
    //{
    //    hdnAttachURLs.Value = "";
    //    string sourcePath = Server.MapPath(filePath);
    //    if (Directory.Exists(sourcePath))
    //    {
    //        var di = new DirectoryInfo(sourcePath);
    //        FileInfo[] myfiles = new FileInfo[] { };
    //        myfiles = di.GetFiles("*");
    //        List<Attachments> attachments = new List<Attachments>();
    //        string[] sizes = { "Bytes", "KB", "MB", "GB" };
    //        double len;
    //        int order;
    //        foreach (var file in myfiles)
    //        {
    //            len = file.Length;
    //            order = 0;
    //            while (len >= 1024 && order + 1 < sizes.Length)
    //            {
    //                order++;
    //                len = len / 1024;
    //            }
    //            attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = sourcePath, Include = true });

    //            //insert url of every being attached in list
    //            //attachURLs.Add(file.FullName);
    //            hdnAttachURLs.Value += file.FullName + "|";
    //        }

    //        if (attachments.Count > 0)
    //            rptAttachments.DataSource = attachments;
    //        else
    //            rptAttachments.DataSource = null;
    //        rptAttachments.DataBind();
    //    }
    //}

    private void SendMail(bool sendMail)
    {
        int emailId = 0;
        try
        {
            // First identify if this email has any attachments
            bool emailHasAttachments = false;
            string userDirectory = "", userServerDirectory;
            List<string> attachmentUrls = new List<string>();

            foreach (RepeaterItem row in rptAttachments.Items)
            {
                HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                if (hdnIsDelete.Value == "0")
                {
                    emailHasAttachments = true;
                    break;
                }
            }

            // Now save Email in DB and get Id of email sent so that Attachments can be saved in folder on behalf of email id
            Emails el = new Emails();
            el.obj = new DAL_AMCPE.Email();
            el.obj.From = from;
            el.obj.To = to;
            el.obj.Cc = cc;
            el.obj.Bcc = bcc;
            el.obj.Subject = subject;
            el.obj.Body = body;
            el.obj.SentBy = "amc\\" + user;
            el.obj.SentOn = DateTime.Now;
            el.obj.HasAttachments = emailHasAttachments;
            el.obj.EmailTemplateId = TemplateId;
            el.obj.IsDraft = !sendMail;
            el.obj.IsDeleted = false;
            el.obj.ActivityRecId = "";
            el.obj.PatientRecId = patientRecId;
            el.obj.PatientNumber = patientNumber;
            el.obj.PatientFullName = patientFullName;

            emailId = el.Save();

            if (emailId > 0)
            {
                // Save files in Sent folder if email has attachments
                if (emailHasAttachments)
                {
                    if (sendMail)
                        userDirectory = "~/Attachments/Sent/" + user + "/";

                    fileName = "";
                    userServerDirectory = Server.MapPath(userDirectory);

                    if (!Directory.Exists(userServerDirectory))
                    {
                        Directory.CreateDirectory(userServerDirectory);
                    }
                    string savePath = userDirectory + emailId.ToString() + "/";
                    string saveServerPath = Server.MapPath(savePath);
                    if (!Directory.Exists(saveServerPath))
                        Directory.CreateDirectory(saveServerPath);
                    //else
                    //{
                    //    // this detects that folder is draft and email is again being draft.
                    //    Array.ForEach(Directory.GetFiles(saveServerPath), File.Delete);
                    //}

                    sourcePath = Server.MapPath("~/Attachments/Temp/" + user + "/");
                    targetPath = saveServerPath;
                    foreach (RepeaterItem row in rptAttachments.Items)
                    {
                        Literal ltr = (Literal)row.FindControl("ltrFileName");
                        HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
                        HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                        fileName = ltr.Text;
                        File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
                        attachmentUrls.Add(Path.Combine(targetPath, fileName));
                    }
                }

                //if (hasBusinessAttachment)
                //{
                //    BAttachments attach = new BAttachments();
                //    List<string> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);

                //    string sourcePath = string.Empty;

                //    if (attachFiles.Count > 0)
                //    {
                //        foreach (string aF in attachFiles)
                //        {
                //            sourcePath = WebConfigurationManager.AppSettings["Server03BusinessAttachments"] + aF;

                //            if (sendMail)
                //                userDirectory = "~/Attachments/Sent/" + user + "/";

                //            fileName = "";
                //            userServerDirectory = Server.MapPath(userDirectory);

                //            if (!Directory.Exists(userServerDirectory))
                //            {
                //                Directory.CreateDirectory(userServerDirectory);
                //            }
                //            string savePath = userDirectory + emailId.ToString() + "/";
                //            string saveServerPath = Server.MapPath(savePath);
                //            if (!Directory.Exists(saveServerPath))
                //                Directory.CreateDirectory(saveServerPath);
                //            //else
                //            //{
                //            //    // this detects that folder is draft and email is again being draft.
                //            //    Array.ForEach(Directory.GetFiles(saveServerPath), File.Delete);
                //            //}

                //            targetPath = saveServerPath;

                //            if (Directory.Exists(sourcePath))
                //            {
                //                string[] files = new string[] { };

                //                if (!Directory.Exists(targetPath))
                //                {
                //                    Directory.CreateDirectory(targetPath);
                //                }


                //                var di = new DirectoryInfo(sourcePath);
                //                FileInfo[] myfiles = new FileInfo[] { };
                //                string file = "";

                //                try
                //                {
                //                    myfiles = di.GetFiles("*", SearchOption.AllDirectories);
                //                }
                //                catch (UnauthorizedAccessException) { }

                //                if (file == "") // it means Include all option is selected and all files need to be selected
                //                {
                //                    foreach (var s in myfiles)
                //                    {
                //                        emailHasAttachments = true;

                //                        // Use static Path methods to extract only the file name from the path.
                //                        fileName = Path.GetFileName(s.FullName);
                //                        string sourceFile = Path.Combine(sourcePath, fileName);
                //                        string destFile = Path.Combine(targetPath, fileName);
                //                        File.Copy(sourceFile, destFile, true);

                //                        attachmentUrls.Add(Path.Combine(targetPath, fileName));
                //                    }
                //                }
                //                else
                //                {
                //                    emailHasAttachments = true;

                //                    fileName = Path.GetFileName(file);
                //                    string sourceFile = Path.Combine(sourcePath, fileName);
                //                    string destFile = Path.Combine(targetPath, fileName);
                //                    File.Copy(sourceFile, destFile, true);

                //                    attachmentUrls.Add(Path.Combine(targetPath, fileName));
                //                }
                //            }
                //        }
                //    }
                //}


                //Update email attachment flag
                Emails el2 = new Emails();
                el2.obj = el2.GetEmailByEmailId(emailId);
                el2.obj.HasAttachments = emailHasAttachments;
                el2.Save();

                if (sendMail)
                {
                    //Now send mail with attachments(if any)
                    FlexiMail ml = new FlexiMail();
                    ml.From = from; // TODO: handle name other than email id also like AMC info@amc.com
                    //ml.FromName = "";
                    ml.To = to;
                    ml.CC = cc;
                    ml.BCC = (bcc).Trim(',');
                    ml.Subject = subject;
                    ml.MailBody = body;
                    ml.MailBodyManualSupply = true;
                    if (emailHasAttachments)
                        ml.AttachFile = attachmentUrls.ToArray();
                    else
                        ml.AttachFile = null;

                    ml.Send();
                }

                // finally save email attachments with urls and name in DB 

                el.objAttachment = new List<DAL_AMCPE.SentEmailAttachment>();
                foreach (string s in attachmentUrls)
                {
                    el.objAttachment.Add(new DAL_AMCPE.SentEmailAttachment()
                    {
                        Name = Path.GetFileName(s),
                        Url = s,
                        EmailId = emailId
                    });
                }
                el.SaveEmailAttachments();

            }
            else
            {
                // TODO: Show error message
            }

            //// delete files from user temp folder
            //DeleteFilesFromUserTempDirectory();

            //Session["dataction"] = "s";
            totalMessagesSent++;

            ResetPatientFlag();

            //Response.Redirect("SentEmails.aspx?PatientNumber=" + patientNumber, false);
        }
        catch (Exception ex)
        {
            //Mark email as deleted when there is some error.
            Emails el2 = new Emails();
            el2.obj = el2.GetEmailByEmailId(emailId);
            if (el2.obj != null)
            {
                el2.obj.IsDeleted = true;
                el2.Save();
            }
        }
    }

    private void DeleteFilesFromUserTempDirectory()
    {
        string userTempDirectory = Server.MapPath("~/Attachments/Temp/" + user + "/");
        if (Directory.Exists(userTempDirectory))
            Array.ForEach(Directory.GetFiles(userTempDirectory), File.Delete);
    }

    private void ResetPatientFlag()
    {
        BulkEmail.UpdatePatientFlag(ddlPatientType.SelectedValue, patientRecId);
    }

    #region Pagination
    public Int16 CurrentPage
    {
        get
        {
            object o = ViewState["_CurrentPage"];
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (Int16)o;
            }
        }
        set { ViewState["_CurrentPage"] = value; }
    }


    protected void BindRptPagination(Int32 intPageCount)
    {
        pnlPagination.Visible = true;
        lblTotalPages.Text = intPageCount.ToString();


        //Show Page range
        //set page start-index and end-index to show numbers
        Int32 StartPageRange = CurrentPage - (CurrentPage % PageRange) + 1;
        Int32 EndPageIndex = default(Int16);
        if (StartPageRange + PageRange - 1 < intPageCount)
        {
            EndPageIndex = StartPageRange + PageRange - 1;
        }
        else
        {
            EndPageIndex = intPageCount;
        }
        ArrayList aryLst = new ArrayList();
        Int32 i = 0;
        for (i = StartPageRange; i <= EndPageIndex; i++)
        {
            aryLst.Add(i);
        }
        rptPagination.DataSource = aryLst;
        rptPagination.DataBind();

        if (i - 1 == (StartPageRange + PageRange - 1) & i - 1 < intPageCount)
        {
            lnkEllipses.Visible = true;
            lnkEllipses.CommandArgument = i.ToString();
        }
        else
        {
            lnkEllipses.Visible = false;
        }

        //set last pageindex to LastPage link
        lnkLast.CommandArgument = (intPageCount - 1).ToString();
        if (CurrentPage == 0)
        {
            lnkFirst.Enabled = false;
            lnkPrev.Enabled = false;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
        }
        else if (CurrentPage == intPageCount - 1)
        {
            lnkNext.Enabled = false;
            lnkLast.Enabled = false;
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
        }
        else
        {
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;

        }
    }
    protected void rptPagination_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName.ToLower() == "page")
        {
            CurrentPage = Convert.ToInt16(Convert.ToInt16(e.CommandArgument) - 1);
            BindData();
        }
    }
    protected void lnkNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage += 1;
        BindData();
    }
    protected void lnkPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage -= 1;
        BindData();
    }
    protected void lnkFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = 0;
        BindData();
    }
    protected void lnkLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        CurrentPage = Convert.ToInt16(Convert.ToInt16(lblTotalPages.Text) - 1);
        BindData();
    }
    protected void lnkEllipses_Click(object sender, System.EventArgs e)
    {
        CurrentPage = (short)((Convert.ToInt32(CurrentPage) / PageRange + 1) * PageRange);
        BindData();
    }
    #endregion
}