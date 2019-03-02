using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.IO;
using BAL_AMCPE;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using System.Net;
using System.Drawing;

public partial class Emails_SendBulkEmail : System.Web.UI.Page
{
    public int pageSize = 25, PageRange = 5;
    public string sortExpression = "order by PatientName asc", searchKeyword = "", patientType = "0";
    public int TemplateId, EmailId;
    public string templateName, user, from, to, cc, bcc, subject, body, letter, patientFileName, attachmentCategory, attachmentDescription, patientRecId, patientNumber, patientFullName, businessFilter, businessInclude;
    public bool hasBusinessAttachment, selectAllAttachments, storePatientFile, sendSMS;

    public List<Attachments> attachments, patientAttachments;

    // File Path variables
    string fileName, sourcePath, targetPath;

    public int totalMessagesToBeSent = 0, totalMessagesSent = 0, totalMessagesNotSentToMissing = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TemplateId = EmailId = 0;
            user = "";
            from = to = cc = bcc = subject = body = letter = patientFileName = attachmentCategory = attachmentDescription = "";

            patientRecId = patientNumber = "";
            businessFilter = businessInclude = "";
            hasBusinessAttachment = selectAllAttachments = storePatientFile = sendSMS = false;

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
            hdnPatientFullName.Value = patientFullName;
            hdnbusinessFilter.Value = businessFilter;
            hdnbusinessInclude.Value = businessInclude;
            hdnhasBusinessAttachment.Value = Convert.ToString(hasBusinessAttachment);
            hdnselectAllAttachments.Value = Convert.ToString(selectAllAttachments);
            hdnstorePatientFile.Value = Convert.ToString(storePatientFile);
            hdnSendSMS.Value = Convert.ToString(sendSMS);

            //hdnAttachingMore.Value = Convert.ToString(attachingMore);
            hdnFrom.Value = from;
            hdnTo.Value = to;
            hdnCc.Value = cc;
            hdnBcc.Value = bcc;
            hdnSubject.Value = subject;
            hdnBody.Value = body;
            hdnLetter.Value = letter;
            hdnPatientFileName.Value = patientFileName;
            hdnAttachmentCategory.Value = attachmentCategory;
            hdnAttachmentDescription.Value = attachmentDescription; 

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
            patientFullName = hdnPatientFullName.Value;
            businessFilter = hdnbusinessFilter.Value;
            businessInclude = hdnbusinessInclude.Value;
            hasBusinessAttachment = Convert.ToBoolean(hdnhasBusinessAttachment.Value);
            selectAllAttachments = Convert.ToBoolean(hdnselectAllAttachments.Value);
            storePatientFile = Convert.ToBoolean(hdnstorePatientFile.Value);
            sendSMS = Convert.ToBoolean(hdnSendSMS.Value);

            from = hdnFrom.Value;
            to = hdnTo.Value;
            cc = hdnCc.Value;
            bcc = hdnBcc.Value;
            subject = hdnSubject.Value;
            body = hdnBody.Value;
            letter = hdnLetter.Value;
            patientFileName = hdnPatientFileName.Value;
            attachmentCategory = hdnAttachmentCategory.Value;
            attachmentDescription = hdnAttachmentDescription.Value;
        }
    }

    private void GetEmailTemplatesByCategoryId(int categoryId)
    {
        EmailTemplates et = new EmailTemplates();
        ddlTemplates.DataSource = et.GetEmailTemplatesByCategoryID(categoryId);
        ddlTemplates.DataTextField = "Name";
        ddlTemplates.DataValueField = "Id";
        ddlTemplates.DataBind();
        ddlTemplates.Items.Insert(0, new ListItem("Please choose template", "0"));
    }

    private void BindData()
    {
        searchKeyword = txtSearch.Text;
        pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        patientType = ddlPatientType.SelectedValue;

        BAL_AMCPE.BulkEmail et = new BAL_AMCPE.BulkEmail();
        var data = et.GetPatientsForBulkEmail(patientType, CurrentPage + 1, pageSize, sortExpression, searchKeyword);
        if (data.Count > 0)
        {
            rptPatients.DataSource = data;
            norecord.Visible = false;
            dvSendEmail.Visible = true;
            TotalPages = Convert.ToInt32(data.FirstOrDefault().TotalPages);
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

    protected void btnSend_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        BAL_AMCPE.EmailData ed = et.GetTemplateDetailByID(Convert.ToInt32(ddlTemplates.SelectedValue));

        TemplateId = ed.Id;
        totalMessagesToBeSent = totalMessagesSent = totalMessagesNotSentToMissing = 0;
        
        hasBusinessAttachment = Convert.ToBoolean(ed.AttachmentHasBusiness);
        selectAllAttachments = Convert.ToBoolean(ed.SelectAllAttachments);
        storePatientFile = Convert.ToBoolean(ed.StorePatientLetter);
        sendSMS = Convert.ToBoolean(ed.SendAsSMS);

        businessFilter = ed.AttachmentBusinessFilter;
        businessInclude = ed.AttachmentBusinessInclude;

        ////check for attachment folder
        //GetAttachmentFiles("~/Attachments/Templates/" + TemplateId.ToString(), TemplateId, hasBusinessAttachment, ed.AttachmentBusinessFilter, ed.AttachmentBusinessInclude, Convert.ToBoolean(ed.PromptForAttachments), Convert.ToBoolean(ed.AttachmentHasDirectory), ed.AttachmentDirectoryPath, ed.AttachmentDirectoryFilter, ed.AttachmentDirectoryInclude);

        CheckBox chkEmailAll = (CheckBox)rptPatients.Controls[0].Controls[0].FindControl("chkEmailAll");
        if (chkEmailAll.Checked)
        {
            BAL_AMCPE.BulkEmail bulkEmail = new BAL_AMCPE.BulkEmail();
            var data = bulkEmail.GetPatientsForBulkEmail(ddlPatientType.SelectedValue, 1, 0, sortExpression, txtSearch.Text);
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    totalMessagesToBeSent++;
                    
                    patientRecId = item.PatientRecId;
                    patientNumber = item.PatientNumber;
                    patientFullName = item.PatientName;

                    //check for attachment folder
                    if (!sendSMS)
                    {
                        GetAttachmentFiles("~/Attachments/Templates/" + TemplateId.ToString(), TemplateId, hasBusinessAttachment, ed.AttachmentBusinessFilter, ed.AttachmentBusinessInclude, Convert.ToBoolean(ed.PromptForAttachments), Convert.ToBoolean(ed.AttachmentHasDirectory), ed.AttachmentDirectoryPath, ed.AttachmentDirectoryFilter, ed.AttachmentDirectoryInclude);
                    }
                    UpdateTemplateWithPatientDetail(TemplateId, ed);
                    if (!string.IsNullOrWhiteSpace(to))
                    {
                        SendMail(true);
                    }
                    else
                    {
                        totalMessagesNotSentToMissing++;
                    }
                }
            }
        }
        else
        {
            foreach (RepeaterItem i in rptPatients.Items)
            {
                CheckBox chkEmail = (CheckBox)i.FindControl("chkEmail");
                if (chkEmail.Checked)
                {
                    totalMessagesToBeSent++;
                    
                    patientRecId = ((HiddenField)i.FindControl("hdnPatientRecId")).Value;
                    patientNumber = ((Literal)i.FindControl("ltlPatientNumber")).Text;
                    patientFullName = ((Literal)i.FindControl("ltlPatientName")).Text;

                    //check for attachment folder
                    if (!sendSMS)
                    {
                        GetAttachmentFiles("~/Attachments/Templates/" + TemplateId.ToString(), TemplateId, hasBusinessAttachment, ed.AttachmentBusinessFilter, ed.AttachmentBusinessInclude, Convert.ToBoolean(ed.PromptForAttachments), Convert.ToBoolean(ed.AttachmentHasDirectory), ed.AttachmentDirectoryPath, ed.AttachmentDirectoryFilter, ed.AttachmentDirectoryInclude);
                    }

                    UpdateTemplateWithPatientDetail(TemplateId, ed);
                    if (!string.IsNullOrWhiteSpace(to))
                    {
                        SendMail(true);
                    }
                    else
                    {
                        totalMessagesNotSentToMissing++;
                    }
                }
            }
        }

        if (totalMessagesToBeSent > 0)
        {
            message.Visible = true;
            lblMessage.Text = totalMessagesSent + "/" + totalMessagesToBeSent + " emails sent.";
        }
        if (totalMessagesNotSentToMissing > 0)
        {
            messageNotSent.Visible = true;
            lblMessageNotSent.Text = totalMessagesNotSentToMissing + "/" + totalMessagesToBeSent + " emails could not be sent because of To field missing.";
        }

        BindData();
    }

    private void UpdateTemplateWithPatientDetail(int templateId, EmailData ed)
    {
        from = Helper.ProcessData(ed.From, patientRecId, patientNumber); //ed.From;
        to = Helper.ProcessData(ed.To, patientRecId, patientNumber); //ed.To;
        cc = !string.IsNullOrWhiteSpace(ed.Cc) ? Helper.ProcessData(ed.Cc, patientRecId, patientNumber) : ""; //ed.Cc;
        bcc = !string.IsNullOrWhiteSpace(ed.Bcc) ? Helper.ProcessData(ed.Bcc, patientRecId, patientNumber) : ""; //ed.Bcc;
        subject = Helper.ProcessData(ed.Subject, patientRecId, patientNumber); //ed.Subject;
        body = Helper.ProcessData(ed.Body, patientRecId, patientNumber); //ed.Body;
        letter = Helper.ProcessData(ed.Letter, patientRecId, patientNumber);

        if (storePatientFile)
        {
            if (!string.IsNullOrWhiteSpace(ed.PatientFileName))
                patientFileName = Helper.ProcessData(ed.PatientFileName, patientRecId, patientNumber);
            else
                patientFileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentCategory))
                attachmentCategory = Helper.ProcessData(ed.AttachmentCategory, patientRecId, patientNumber);
            else
                attachmentCategory = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentDescription))
                attachmentDescription = Helper.ProcessData(ed.AttachmentDescription, patientRecId, patientNumber);
            else
                attachmentDescription = string.Empty;
        }
        else
        {
            patientFileName = attachmentCategory = attachmentDescription = string.Empty;
        }

        ////Get patient specific attachments
        //patientAttachments = new List<Attachments>();
        //if (attachments.Count > 0)
        //{
        //    patientAttachments.AddRange(attachments);
        //}

        

        //if (patientAttachments.Count > 0)
        //    rptAttachments.DataSource = patientAttachments;
        //else
        //    rptAttachments.DataSource = null;
        //rptAttachments.DataBind();
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
            int fileCount = 1;

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
                    //businessFilter = Helper.Between(businessFilter, "%", "%");
                    //businessFilter = Helper.ProcessData(businessFilter, patientRecId, patientNumber);
                    ////List<string> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);
                    //List<BAL_AMCPE.BAttachments.AttachmentFile> attachFiles = attach.GetPatientBAttachments(patientRecId, businessFilter, businessInclude);

                    string sourcePathFile = string.Empty;

                    if (attachFiles.Count > 0)
                    {
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
            el.obj.Body = !sendSMS ? body : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(body.Replace("<br />", ""), "<(.|\n)*?>", ""));
            el.obj.Letter = letter;
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


                    byte[] byteArray = Encoding.UTF8.GetBytes("<html><head></head><body>" + el.obj.Letter + "</body></html>");
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


                    //get patient dob and set as password
                    PatientActivity pAct = new PatientActivity();
                    string password = pAct.GetPatientDOBByPatientNumber(patientNumber);

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
                                using (DAL_AMCPE.GMEEDevelopmentEntities ATCH = new DAL_AMCPE.GMEEDevelopmentEntities())
                                {
                                    ATCH.obj_sp_BulkPrint_Attachment("", "", patientRecId, @"amc\" + user, outputLocation.Replace("\\\\", "\\"), Path.GetFileName(outputLocation), id, attachmentDescription, attachmentCategory);
                                }
                            }
                            catch (Exception ex) { }
                        }

                        //pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.AESx128);
                        pdfDoc.Encrypt(password, password, Aspose.Pdf.Permissions.PrintDocument, Aspose.Pdf.CryptoAlgorithm.RC4x128);
                        pdfDoc.Save(pathToSaveServer + "\\" + filename);

                        try
                        {
                            File.Delete(pathToSaveServer + "\\" + filenameToBeDeleted);
                            File.Delete(pathToSaveServer + "\\" + htmlFile);
                        }
                        catch(Exception ex)
                        {

                        }

                    }

                    attachmentUrls.Add(Path.Combine(pathToSaveServer, filename));
                    emailHasAttachments = true;
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
                    ml.MailBody = !sendSMS ? body : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(body.Replace("<br />", ""), "<(.|\n)*?>", ""));
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