using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.IO;
using System.Web.Configuration;
using System.Drawing;
using System.Text;

public partial class Emails_AutoProcessEmailJob : System.Web.UI.Page
{
    public int TemplateId, EmailId;
    public long EmailProcessId;
    public string templateName, user, from, to, cc, bcc, subject, body, letter, patientFileName, attachmentCategory, attachmentDescription, patientRecId, patientNumber, patientFullName, businessFilter, businessInclude, presType, userLetterFilePath = "", userLetterEncryptedFilePath = "", attachmentRecId;
    public bool hasBusinessAttachment, selectAllAttachments, storePatientFile, sendSMS, sendUnencryptedFile;

    // File Path variables
    string fileName, sourcePath, targetPath;

    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["PatientNumber"]))
            {
                Session["PatientNumber"] = Convert.ToString(Request.QueryString["PatientNumber"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["RecId"]))
            {
                Session["RecId"] = Convert.ToString(Request.QueryString["RecId"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["PresType"]))
            {
                Session["PresType"] = string.IsNullOrWhiteSpace(Convert.ToString(Request.QueryString["PresType"])) ? "" : Convert.ToString(Request.QueryString["PresType"]);
            }


            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
            {
                user = Convert.ToString(Request.QueryString["UserId"]).ToLower().Replace("amc\\", "");
                Session["UserId"] = Convert.ToString(Request.QueryString["UserId"]);
            }
            else
            {
                user = "SYSTEM";
                Session["UserId"] = "SYSTEM";
            }

            if (!string.IsNullOrEmpty(Request.QueryString["EmailProcessId"]))
            {
                EmailProcessId = Convert.ToInt64(Request.QueryString["EmailProcessId"]);
            }

            PatientActivity pa = new PatientActivity();
            if (string.IsNullOrEmpty(Request.QueryString["PatientNumber"]) || string.IsNullOrEmpty(Request.QueryString["RecId"]))
            {
                Response.Redirect("~/SessionExpired.aspx?reason=nopatient");
            }
            else
            {
                patientNumber = Convert.ToString(Request.QueryString["PatientNumber"]);
                patientRecId = Convert.ToString(Request.QueryString["RecId"]);
                patientFullName = pa.GetPatientNameByPatientNumber(patientNumber);
                presType = string.IsNullOrWhiteSpace(Convert.ToString(Request.QueryString["PresType"])) ? "" : Convert.ToString(Request.QueryString["PresType"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["TemplateId"]))
            {
                int templateId = Convert.ToInt32(Request.QueryString["TemplateId"]);
                TemplateId = templateId;
                GetTemplateDetailById(templateId);

                if (!string.IsNullOrWhiteSpace(to))
                {
                    ProcessEmailData();
                    SendMail(true);
                }
            }
            else
            {
                Response.Redirect("~/SessionExpired.aspx?reason=notemplate");
            }
        }
    }

    private void GetTemplateDetailById(int id)
    {
        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        BAL_AMCPE.EmailData ed = et.GetTemplateDetailByID(id);

        from = hdnFrom.Value = Helper.ProcessDataAutoSQLEmail(ed.From, patientRecId, patientNumber, presType);  //ed.From;
        to = hdnTo.Value = Helper.ProcessDataAutoSQLEmail(ed.To, patientRecId, patientNumber, presType); //ed.To;

        cc = hdnCc.Value = !string.IsNullOrWhiteSpace(ed.Cc) ? Helper.ProcessDataAutoSQLEmail(ed.Cc, patientRecId, patientNumber, presType) : ""; //ed.Cc;
        bcc = hdnBcc.Value = !string.IsNullOrWhiteSpace(ed.Bcc) ? Helper.ProcessDataAutoSQLEmail(ed.Bcc, patientRecId, patientNumber, presType) : ""; //ed.Bcc;
        subject = hdnSubject.Value = Helper.ProcessDataAutoSQLEmail(ed.Subject, patientRecId, patientNumber, presType); //ed.Subject;
        body = hdnBody.Value = Helper.ProcessDataAutoSQLEmail(ed.Body, patientRecId, patientNumber, presType); //ed.Body;
        letter = hdnLetter.Value = Helper.ProcessDataAutoSQLEmail(ed.Letter, patientRecId, patientNumber, presType);


        hasBusinessAttachment = Convert.ToBoolean(ed.AttachmentHasBusiness);
        hdnhasBusinessAttachment.Value = Convert.ToString(hasBusinessAttachment);

        selectAllAttachments = Convert.ToBoolean(ed.SelectAllAttachments);
        hdnselectAllAttachments.Value = Convert.ToString(selectAllAttachments);

        storePatientFile = Convert.ToBoolean(ed.StorePatientLetter);
        hdnstorePatientFile.Value = Convert.ToString(storePatientFile);

        if (storePatientFile)
        {
            if (!string.IsNullOrWhiteSpace(ed.PatientFileName))
                patientFileName = hdnPatientFileName.Value = Helper.ProcessDataAutoSQLEmail(ed.PatientFileName, patientRecId, patientNumber, presType);
            else
                patientFileName = hdnPatientFileName.Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentCategory))
                attachmentCategory = hdnAttachmentCategory.Value = Helper.ProcessDataAutoSQLEmail(ed.AttachmentCategory, patientRecId, patientNumber, presType);
            else
                attachmentCategory = hdnAttachmentCategory.Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentDescription))
                attachmentDescription = hdnAttachmentDescription.Value = Helper.ProcessDataAutoSQLEmail(ed.AttachmentDescription, patientRecId, patientNumber, presType);
            else
                attachmentDescription = hdnAttachmentDescription.Value = string.Empty;
        }
        else
        {
            patientFileName = hdnPatientFileName.Value = string.Empty;
            attachmentCategory = hdnAttachmentCategory.Value = string.Empty;
            attachmentDescription = hdnAttachmentDescription.Value = string.Empty;
        }

        sendSMS = Convert.ToBoolean(ed.SendAsSMS);
        hdnSendSMS.Value = Convert.ToString(sendSMS);

        sendUnencryptedFile = Convert.ToBoolean(ed.SendUnencryptedFile);
        hdnSendUnencryptedFile.Value = Convert.ToString(sendUnencryptedFile);

        businessFilter = hdnbusinessFilter.Value = ed.AttachmentBusinessFilter;
        businessInclude = hdnbusinessInclude.Value = ed.AttachmentBusinessInclude;

        hdnShowPromptDialog.Value = "0";
        hdnPromptFileUrl.Value = hdnPromptFileName.Value = "";

        if (!Convert.ToBoolean(sendSMS))
        {
            //check for attachment folder
            GetAttachmentFiles("~/Attachments/Templates/" + TemplateId.ToString(), TemplateId, hasBusinessAttachment, ed.AttachmentBusinessFilter, ed.AttachmentBusinessInclude, Convert.ToBoolean(ed.PromptForAttachments), Convert.ToBoolean(ed.AttachmentHasDirectory), ed.AttachmentDirectoryPath, ed.AttachmentDirectoryFilter, ed.AttachmentDirectoryInclude, Convert.ToBoolean(ed.IncludeInstructions), ed.InstructionFilter, Convert.ToBoolean(ed.CombineMultipleInstructions));
        }
    }

    private void GetAttachmentFiles(string filePath, int templateId = 0, bool hasBusinessAttachment = false, string businessFilter = "", string businessInclude = "", bool promptForAttachments = false, bool attachmentHasDirectory = false, string directoryPath = "", string directoryFilter = "", string directoryInclude = "", bool includeInstructions = false, string instructionFilter = "", bool combineInstruction = false)
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
            int fileCount = 1;

            List<Attachments> attachments = new List<Attachments>();
            List<Attachments> directoryAttachments = new List<Attachments>();

            if (Directory.Exists(sourcePath)) //email template Browsed Files
            {
                di = new DirectoryInfo(sourcePath);
                myfiles = di.GetFiles("*");
                foreach (var file in myfiles)
                {
                    if (!file.Name.ToLower().Contains("letter.docx"))
                    {
                        len = file.Length;
                        order = 0;
                        while (len >= 1024 && order + 1 < sizes.Length)
                        {
                            order++;
                            len = len / 1024;
                        }

                        string attachedFilePath = filePath.Replace("~", "");

                        //attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = sourcePath, FileWebURL = URLRewrite.BasePath() + "/Attachments/Templates/" + id + "/" + file.Name });
                        attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = sourcePath, FileWebURL = URLRewrite.BasePath() + attachedFilePath + "/" + file.Name, Include = true });


                        //attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = URLRewrite.BasePath() + "/Attachments/Templates/" + id + "/" + file.Name });

                        //insert url of every being attached in list
                        //attachURLs.Add(file.FullName);
                        hdnAttachURLs.Value += file.FullName + "|";
                    }
                }
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

                        businessFilter = Helper.ProcessDataAutoSQLEmail(businessFilter, patientRecId, patientNumber, presType);
                        //List<string> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                        attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                    }


                    string sourcePathFile = string.Empty;

                    if (attachFiles.Count > 0)
                    {
                        //foreach (string aF in attachFiles)
                        foreach (BAL_AMCPE.BAttachments.AttachmentFile aF in attachFiles)
                        {
                            sourcePathFile = WebConfigurationManager.AppSettings["Server03BusinessAttachments"] + aF.RecId;
                            di = new DirectoryInfo(sourcePathFile);
                            myfiles = di.GetFiles("*");

                            if (myfiles.Count() > 0)
                            {
                                if (businessInclude == "Include Latest")
                                    myfiles = myfiles.OrderByDescending(a => a.LastWriteTime).Take(1).ToArray();
                                else if (businessInclude == "Include Oldest")
                                    myfiles = myfiles.OrderBy(a => a.LastWriteTime).Take(1).ToArray();
                            }

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

                                string destFile = string.Empty;
                                string newFileName = string.Empty;

                                if (businessFilter.Contains("!"))
                                {
                                    newFileName = s.Name.Remove(s.Name.LastIndexOf(".")) + fileCount++ + Path.GetExtension(sourceFile);
                                    destFile = Path.Combine(savePath, newFileName);
                                }
                                else
                                {
                                    destFile = Path.Combine(savePath, s.Name);
                                }

                                File.Copy(sourceFile, destFile, true);

                                string fileName = businessFilter.Contains("!") ? newFileName : s.Name;
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

                                string attachedFilePath = filePath.Replace("~", "");

                                attachments.Add(new Attachments { Name = fileName, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fileName, Include = true });
                                hdnAttachURLs.Value += s.FullName + "|";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

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
                            //string newFileName = s.Name.Remove(s.Name.LastIndexOf(".")) + fileCount++ + Path.GetExtension(sourceFile);
                            //string destFile = Path.Combine(savePath, newFileName);
                            string destFile = Path.Combine(savePath, s.Name);
                            File.Copy(sourceFile, destFile, true);

                            string attachedFilePath = filePath.Replace("~", "");

                            if (promptForAttachments)
                            {
                                directoryAttachments.Add(new Attachments { Name = s.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + s.Name, Include = true });
                            }
                            else
                            {
                                attachments.Add(new Attachments { Name = s.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + s.Name, Include = true });
                                hdnAttachURLs.Value += s.FullName + "|";
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            try
            {
                if (includeInstructions) //Include attachments
                {
                    BAL_AMCPE.InstructionTemplates et = new BAL_AMCPE.InstructionTemplates();
                    DAL_AMCPE.InstructionTemplate ed = et.GetDocTemplateByID(Convert.ToInt32(TemplateId));

                    List<string> instructions = Helper.ProcessDataInstruction(instructionFilter, patientRecId, patientNumber);

                    int filecount = 1;

                    if (instructions.Count > 0)
                    {
                        var templates = et.GetInstructionTemplateByNames(string.Join(",", instructions));

                        if (templates.Count > 0)
                        {
                            string userTempDir = "~/TempFiles/" + user + "/";
                            string savePath1 = Server.MapPath(userTempDir);
                            if (!Directory.Exists(savePath1))
                            {
                                Directory.CreateDirectory(savePath1);
                            }

                            Aspose.Words.License license = new Aspose.Words.License();
                            license.SetLicense(Server.MapPath("~/Aspose.Words.lic"));
                            Aspose.Words.Document doc;

                            string templatePath = "";
                            System.IO.FileInfo[] allPDFs = null;
                            string Merged_pdf_filepath = "";

                            int count = 0;
                            filecount = templates.Count;
                            allPDFs = new System.IO.FileInfo[filecount];

                            foreach (var item in templates)
                            {
                                templatePath = item.TemplatePath;
                                if (File.Exists(templatePath))
                                {
                                    string path_pdf = "";

                                    FileInfo fileinfo = new FileInfo(templatePath);
                                    string filename = fileinfo.Name.Replace(".docx", "").Replace(".doc", "");

                                    string id = Guid.NewGuid().ToString().Replace("-", "");
                                    string DESTfile = Convert.ToString(Convert.ToString(patientFullName + "_" + filename)) + ".doc";

                                    string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;

                                    path_pdf = et.ReadDocFileToString_V2(templatePath, patientRecId, patientNumber, savePath, DESTfile, "");

                                    if (!string.IsNullOrWhiteSpace(path_pdf))
                                    {
                                        try
                                        {
                                            string OutputLocation = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                                            doc = new Aspose.Words.Document(path_pdf);
                                            doc.Save(OutputLocation);

                                            et.delete_tempfile();
                                            Session["fileinfo"] = path_pdf.Replace(".doc", ".pdf");

                                            FileInfo fileidnfo = new FileInfo(OutputLocation);
                                            string fileName = fileidnfo.Name;
                                            File.Copy(OutputLocation, savePath1 + "\\" + fileName, true);
                                            File.Delete(OutputLocation);
                                            FileInfo fileInfo2 = new FileInfo(savePath1 + "\\" + fileName);

                                            if (combineInstruction)
                                            {
                                                allPDFs[count] = fileInfo2;
                                                count++;
                                            }
                                            else
                                            {
                                                len = fileInfo2.Length;

                                                order = 0;
                                                while (len >= 1024 && order + 1 < sizes.Length)
                                                {
                                                    order++;
                                                    len = len / 1024;
                                                }

                                                attachments.Add(new Attachments { Name = fileInfo2.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath1, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fileInfo2.Name, Include = true });
                                                hdnAttachURLs.Value += fileInfo2.FullName + "|";
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }

                            if (combineInstruction)
                            {
                                //string pInstFile = DateTime.Now.Ticks.ToString() + ".pdf";
                                //Merged_pdf_filepath = et.MergeAllPDF(allPDFs, Server.MapPath("~/Output/") + pInstFile);

                                string pInstFile = patientFullName + "_Instructions_" + DateTime.Now.Ticks.ToString() + ".pdf";
                                Merged_pdf_filepath = et.MergeAllPDF(allPDFs, savePath1 + pInstFile);

                                FileInfo fInfo = new FileInfo(Merged_pdf_filepath);

                                len = fInfo.Length;
                                order = 0;
                                while (len >= 1024 && order + 1 < sizes.Length)
                                {
                                    order++;
                                    len = len / 1024;
                                }

                                //string id = Guid.NewGuid().ToString().Replace("-", "");
                                //string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;

                                attachments.Add(new Attachments { Name = fInfo.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath1, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fInfo.Name, Include = true });
                                hdnAttachURLs.Value += fInfo.FullName + "|";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            if (attachments.Count > 0)
                rptAttachments.DataSource = attachments;
            else
                rptAttachments.DataSource = null;
            rptAttachments.DataBind();

        }
        catch (Exception ex)
        { }
    }

    private void ProcessEmailData()
    {
        try
        {
            string ud = Server.MapPath("~/Attachments/Temp/" + user + "/");
            if (!Directory.Exists(ud))
            {
                Directory.CreateDirectory(ud);
            }
            if (!Directory.Exists(ud + "\\2"))
            {
                Directory.CreateDirectory(ud + "\\2");
            }

            fileName = "";
            targetPath = ud + "\\2";

            // save files in temp foldee now
            foreach (RepeaterItem row in rptAttachments.Items)
            {
                Literal ltrFileName = (Literal)row.FindControl("ltrFileName");
                HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
                HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                if (hdnIsDelete.Value == "0")
                {
                    fileName = ltrFileName.Text;
                    File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
                }
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile objHttpPostedFile = (HttpPostedFile)Request.Files[i];
                if (objHttpPostedFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(objHttpPostedFile.FileName);
                    objHttpPostedFile.SaveAs(Path.Combine(targetPath, fileName));
                }
            }


            // empty Temp before moving files into it.
            Array.ForEach(Directory.GetFiles(ud), File.Delete);

            // Move files from \\2 to temp folder so that we always get files from there.
            string[] files = Directory.GetFiles(targetPath);
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                fileName = Path.GetFileName(s);
                System.IO.File.Move(s, Path.Combine(ud, fileName));
            }

            // bind repeater with these stored files
            GetAttachmentFiles("~/Attachments/Temp/" + user + "/");
        }
        catch (Exception ex)
        {

        }
    }

    private void SendMail(bool sendMail)
    {
        EmailJobProcess ej = new EmailJobProcess();
        DAL_AMCPE.EmailJob job = ej.GetEmailJobById(EmailProcessId);

        int emailId = 0;
        try
        {
            // First identify if this email has any attachments
            bool emailHasAttachments = false;
            string userDirectory = "", userServerDirectory;
            List<string> attachmentUrls = new List<string>();

            if (!sendSMS)
            {
                foreach (RepeaterItem row in rptAttachments.Items)
                {
                    HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                    if (hdnIsDelete.Value == "0")
                    {
                        emailHasAttachments = true;
                        break;
                    }
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
            el.obj.Body = !sendSMS ? body : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(body.Replace("<br />", ""), "<(.|\n)*?>", ""));
            el.obj.Letter = letter;
            el.obj.SentBy = "amc\\" + user;
            el.obj.SentOn = DateTime.Now;
            //el.obj.HasAttachments = attachments.Count() > 0 ? true : false;
            el.obj.HasAttachments = emailHasAttachments;
            el.obj.EmailTemplateId = TemplateId;
            el.obj.IsDraft = !sendMail;
            el.obj.IsDeleted = false;
            el.obj.ActivityRecId = "";
            //el.obj.PatientRecId = "";
            //el.obj.PatientNumber = "";
            el.obj.PatientRecId = patientRecId;
            el.obj.PatientNumber = patientNumber;
            el.obj.PatientFullName = patientFullName;

            el.obj.StorePatientLetter = storePatientFile;
            el.obj.PatientFileName = patientFileName;
            el.obj.AttachmentCategory = attachmentCategory;
            el.obj.AttachmentDescription = attachmentDescription;
            el.obj.SendASSMS = sendSMS;

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
                    else
                    {
                        // this detects that folder is draft and email is again being draft.
                        Array.ForEach(Directory.GetFiles(saveServerPath), File.Delete);
                    }

                    sourcePath = Server.MapPath("~/Attachments/Temp/" + user + "/");
                    targetPath = saveServerPath;
                    foreach (RepeaterItem row in rptAttachments.Items)
                    {
                        HiddenField hdnInclude = (HiddenField)row.FindControl("hdnInclude");
                        if (Convert.ToBoolean(hdnInclude.Value))
                        {
                            Literal ltr = (Literal)row.FindControl("ltrFileName");
                            HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
                            HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                            fileName = ltr.Text;
                            File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
                            attachmentUrls.Add(Path.Combine(targetPath, fileName));
                        }
                    }
                }

                //Either create encrypted PDF or save as docx file
                //if (sendMail && Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
                if (sendMail && !string.IsNullOrWhiteSpace(el.obj.Letter) && !sendSMS)
                {
                    string templateData = Convert.ToString(el.obj.Letter);

                    string ptFileName = string.Empty;
                    if (storePatientFile)
                    {
                        ptFileName = patientFileName;
                    }
                    else
                    {
                        ptFileName = patientFullName + "_" + DateTime.Now.ToString("dd_MM_yy");
                    }

                    string filename = ptFileName + ".pdf";
                    string filenameToBeDeleted = ptFileName + "_1.pdf";
                    string htmlFile = ptFileName + ".html";

                    Aspose.Words.License licenseWord = new Aspose.Words.License();
                    licenseWord.SetLicense(Server.MapPath("~/Aspose.Words.lic"));

                    Aspose.Pdf.License licensePDF = new Aspose.Pdf.License();
                    licensePDF.SetLicense(Server.MapPath("~/Aspose.Pdf.lic"));


                    byte[] byteArray = Encoding.UTF8.GetBytes("<html><head></head><body>" + el.obj.Letter.Replace("[page-break]", "<br style='page-break-before:always; clear:both' />") + "</body></html>");
                    string pathToSaveServer = Server.MapPath("~/Attachments/Sent/" + user + "/" + emailId);

                    //Save the document as PDF
                    if (!Directory.Exists(pathToSaveServer))
                    {
                        Directory.CreateDirectory(pathToSaveServer);
                    }

                    //create html file on the fly
                    using (FileStream fs = new FileStream(pathToSaveServer + "\\" + htmlFile, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.WriteLine("<html><head></head><body>" + el.obj.Letter.Replace("[page-break]", "<br style='page-break-before:always; clear:both' />") + "</body></html>");
                        }
                    }

                    using (MemoryStream stream = new MemoryStream(byteArray))
                    {
                        //Stream method removed as creating issue with &nbsp; and other & tags, so replaced with html file
                        //Aspose.Words.Document wordDoc = new Aspose.Words.Document(stream);

                        Aspose.Words.Document wordDoc = new Aspose.Words.Document(pathToSaveServer + "\\" + htmlFile);


                        Aspose.Words.PageSetup ps = wordDoc.FirstSection.PageSetup;
                        ps.LeftMargin = 0.5 * 72;
                        ps.RightMargin = 0.5 * 72;
                        ps.TopMargin = 0.5 * 72;
                        ps.BottomMargin = 0.5 * 72;

                        wordDoc.Save(pathToSaveServer + "\\" + filenameToBeDeleted);


                        Aspose.Pdf.Document pdfDoc = new Aspose.Pdf.Document(pathToSaveServer + "\\" + filenameToBeDeleted);

                        if (storePatientFile) //save unencrypted letter pdf in patient record
                        {
                            try
                            {
                                string id = Guid.NewGuid().ToString().Replace("-", "");
                                string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;
                                string outputLocation = savePath + "\\" + filename;
                                if (!System.IO.Directory.Exists(savePath))
                                {
                                    System.IO.Directory.CreateDirectory(savePath);
                                }
                                pdfDoc.Save(savePath + "\\" + filename);

                                userLetterFilePath = savePath + "\\" + filename;

                                using (DAL_AMCPE.GMEEDevelopmentEntities ATCH = new DAL_AMCPE.GMEEDevelopmentEntities())
                                {
                                    ATCH.obj_sp_BulkPrint_Attachment("", "", patientRecId, @"amc\" + user, outputLocation.Replace("\\\\", "\\"), Path.GetFileName(outputLocation), id, attachmentDescription, attachmentCategory);
                                    attachmentRecId = id;
                                }
                            }
                            catch (Exception ex) { }
                        }

                        //delete any letter file if it already exists, added on 02.10.2017
                        try
                        {
                            if (attachmentUrls.Contains(pathToSaveServer + "\\" + filename))
                            {
                                attachmentUrls.Remove(pathToSaveServer + "\\" + filename);
                            }
                        }
                        catch (Exception ex)
                        { }

                        //pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.AESx128);
                        if (!sendUnencryptedFile)
                        {
                            //get patient dob and set as password
                            PatientActivity pAct = new PatientActivity();
                            string password = pAct.GetPatientDOBByPatientNumber(patientNumber);

                            pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.RC4x128);
                        }
                        pdfDoc.Save(pathToSaveServer + "\\" + filename);
                        userLetterEncryptedFilePath = pathToSaveServer + "\\" + filename;

                        try
                        {
                            File.Delete(pathToSaveServer + "\\" + filenameToBeDeleted);
                            File.Delete(pathToSaveServer + "\\" + htmlFile);
                        }
                        catch (Exception ex)
                        {

                        }

                    }


                    attachmentUrls.Add(Path.Combine(pathToSaveServer, filename));
                    emailHasAttachments = true;
                }

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
                    ml.MailBody = !sendSMS ? body : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(body.Replace("<br />", ""), "<(.|\n)*?>", "")); //body;
                    ml.MailBodyManualSupply = true;
                    ml.IsBodyHtml = !sendSMS;
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

                job.EmailFailed = false;
                job.EmailSent = true;
                job.SendDate = DateTime.Now;
            }
            else
            {
                // TODO: Show error message
                job.EmailFailed = true;
                job.EmailSent = false;
            }
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

            //flag email job to failure
            job.EmailFailed = true;
            job.EmailSent = false;

            //delete user letter file and remove entry from CRM
            DeleteUserLetterFile();
            using (DAL_AMCPE.GMEEDevelopmentEntities ATCH = new DAL_AMCPE.GMEEDevelopmentEntities())
            {
                ATCH.sp_BulkPrint_Attachment_Delete(patientRecId, attachmentRecId);
            }
        }


        // delete files from user temp folder
        DeleteFilesFromUserTempDirectory();

        ej.obj = job;
        ej.Save();
    }

    private void DeleteFilesFromUserTempDirectory()
    {
        string userTempDirectory = Server.MapPath("~/Attachments/Temp/" + user + "/");
        if (Directory.Exists(userTempDirectory))
            Array.ForEach(Directory.GetFiles(userTempDirectory), File.Delete);
    }

    private void DeleteUserLetterFile()
    {
        if (!string.IsNullOrWhiteSpace(userLetterFilePath) && File.Exists(userLetterFilePath))
        {
            File.Delete(userLetterFilePath);
        }

        if (!string.IsNullOrWhiteSpace(userLetterEncryptedFilePath) && File.Exists(userLetterEncryptedFilePath))
        {
            File.Delete(userLetterEncryptedFilePath);
        }
    }
}