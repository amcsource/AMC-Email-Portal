using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.IO;
using BAL_AMCPE;
using System.Text.RegularExpressions;
using System.Text;
using iTextSharp.text.pdf;
using System.Net;
using System.Drawing;
using WordToPDF;

public partial class Emails_SendEmail_WithDots : System.Web.UI.Page
{
    public int TemplateId, EmailId;
    public string user, from, to, cc, bcc, subject, body, letter, patientFileName, attachmentCategory, attachmentDescription, action, patientRecId, patientNumber, patientFullName, businessFilter, businessInclude;
    public bool hasBusinessAttachment, selectAllAttachments, storePatientFile, sendSMS, sendUnencryptedFile;
    //public bool attachingMore;
    //public static List<string> attachURLs = new List<string>();

    // File Path variables
    string fileName, sourcePath, targetPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        string eventArgs = Convert.ToString(Request["__EVENTARGUMENT"]);
        string valTarget = Convert.ToString(Request["__EVENTTARGET"]);

        if (!IsPostBack)
        {
            TemplateId = EmailId = 0;
            user = "";
            action = "";
            from = to = cc = bcc = subject = body = letter = patientFileName = attachmentCategory = attachmentDescription = "";

            patientRecId = patientNumber = "";
            businessFilter = businessInclude = "";
            hasBusinessAttachment = selectAllAttachments = storePatientFile = sendSMS = sendUnencryptedFile = false;

            //attachingMore = false;
            //if (!string.IsNullOrEmpty(Request.QueryString["User"]))
            //{
            //    user = Request.QueryString["User"].Replace("amc\\", "");
            //    Session["CurrentUser"] = user;
            //}
            //else
            //{
            //    //TODO: redirect to email list
            //}


            if (!string.IsNullOrEmpty(Request.QueryString["PatientNumber"]))
            {
                Session["PatientNumber"] = Convert.ToString(Request.QueryString["PatientNumber"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["RecId"]))
            {
                Session["RecId"] = Convert.ToString(Request.QueryString["RecId"]);
            }


            // added to give direct access to this page
            if (!string.IsNullOrEmpty(Request.QueryString["UserId"]))
            {
                user = Convert.ToString(Request.QueryString["UserId"]).ToLower().Replace("amc\\", "");
                Session["UserId"] = Convert.ToString(Request.QueryString["UserId"]);
            }
            else
            {
                user = Convert.ToString(Session["UserId"]).ToLower().Replace("amc\\", "");
            }

            PatientActivity pa = new PatientActivity();
            // added to give direct access to this page
            if (Session["PatientNumber"] == null && Session["RecId"] == null)
            {
                if (string.IsNullOrEmpty(Request.QueryString["PatientNumber"]) && string.IsNullOrEmpty(Request.QueryString["RecId"]))
                {
                    Response.Redirect("~/SessionExpired.aspx?reason=nopatient");
                }
                else
                {
                    patientNumber = Convert.ToString(Request.QueryString["PatientNumber"]);
                    patientRecId = Convert.ToString(Request.QueryString["RecId"]);
                    patientFullName = pa.GetPatientNameByPatientNumber(patientNumber);
                }
            }
            else
            {
                patientNumber = Convert.ToString(Session["PatientNumber"]);
                patientRecId = Convert.ToString(Session["RecId"]);
                patientFullName = pa.GetPatientNameByPatientNumber(patientNumber);
            }


            //if (Session["PatientNumber"] == null && Session["RecId"] == null)
            //{
            //    Response.Redirect("~/SessionExpired.aspx?reason=nopatient");
            //}
            //else
            //{
            //    patientNumber = Convert.ToString(Session["PatientNumber"]);
            //    patientRecId = Convert.ToString(Session["RecId"]);
            //}


            if (!string.IsNullOrEmpty(Request.QueryString["TemplateId"]))
            {
                TemplateId = Convert.ToInt32(Request.QueryString["TemplateId"]);
                GetTemplateDetailById(TemplateId);
                chooseTemplate.Visible = false;
                divEmail.Visible = true;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["EmailId"]))
            {
                EmailId = Convert.ToInt32(Request.QueryString["EmailId"]);
                GetEmailDetailById(EmailId);
                chooseTemplate.Visible = false;
                divEmail.Visible = true;
            }
            else
            {
                chooseTemplate.Visible = true;
                divEmail.Visible = false;

                EmailTemplateCategory etc = new EmailTemplateCategory();
                ddlEmailTemplateCategories.DataSource = etc.GetEmailTemplateCategories();
                ddlEmailTemplateCategories.DataTextField = "CategoryName";
                ddlEmailTemplateCategories.DataValueField = "Id";
                ddlEmailTemplateCategories.DataBind();
                ddlEmailTemplateCategories.Items.Insert(0, new ListItem("All Email Template Categories", "0"));

                GetEmailTemplatesByCategoryId(Convert.ToInt32(ddlEmailTemplateCategories.SelectedValue));
            }

            // set hidden variables so that we may get values at post back
            hdnTemplateId.Value = TemplateId.ToString();
            hdnEmailId.Value = EmailId.ToString();
            hdnCurrentUser.Value = user;
            hdnAction.Value = action;
            hdnPatientNumber.Value = Convert.ToString(patientNumber);
            hdnPatientRecId.Value = patientRecId;
            hdnPatientFullName.Value = patientFullName;
            hdnbusinessFilter.Value = businessFilter;
            hdnbusinessInclude.Value = businessInclude;
            hdnhasBusinessAttachment.Value = Convert.ToString(hasBusinessAttachment);
            hdnselectAllAttachments.Value = Convert.ToString(selectAllAttachments);
            hdnstorePatientFile.Value = Convert.ToString(storePatientFile);
            hdnSendSMS.Value = Convert.ToString(sendSMS);
            hdnSendUnencryptedFile.Value = Convert.ToString(sendUnencryptedFile);

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

            // delete any files from user temp folder which might be left as garbage or when email was not sent.
            DeleteFilesFromUserTempDirectory();

        }
        else
        {
            TemplateId = Convert.ToInt32(hdnTemplateId.Value);
            EmailId = Convert.ToInt32(hdnEmailId.Value);
            user = hdnCurrentUser.Value;
            action = hdnAction.Value;
            patientNumber = hdnPatientNumber.Value;
            patientRecId = hdnPatientRecId.Value;
            patientFullName = hdnPatientFullName.Value;
            businessFilter = hdnbusinessFilter.Value;
            businessInclude = hdnbusinessInclude.Value;
            hasBusinessAttachment = Convert.ToBoolean(hdnhasBusinessAttachment.Value);
            selectAllAttachments = Convert.ToBoolean(hdnselectAllAttachments.Value);
            storePatientFile = Convert.ToBoolean(hdnstorePatientFile.Value);
            sendSMS = Convert.ToBoolean(hdnSendSMS.Value);
            sendUnencryptedFile = Convert.ToBoolean(hdnSendUnencryptedFile.Value);

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

            //if (Session["CurrentUser"] != null)
            //    user = Convert.ToString(Session["CurrentUser"]);
            //else
            //{
            //    //TODO: redirect to email list
            //}

            //attachingMore = Convert.ToBoolean(hdnAttachingMore.Value);
        }

        if (eventArgs == "" && valTarget == "cphBody_btnPreview")
        {
            SendMail(true);
        }
    }

    private void GetTemplateDetailById(int id)
    {
        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        BAL_AMCPE.EmailData ed = et.GetTemplateDetailByID(id);

        txtFrom.Text = from = hdnFrom.Value = Helper.ProcessData(ed.From, patientRecId, patientNumber);  //ed.From;
        txtTo.Text = to = hdnTo.Value = Helper.ProcessData(ed.To, patientRecId, patientNumber); //ed.To;
        if (txtTo.Text == "")
        {
            btnSend.Enabled = btnDraft.Enabled = false;
        }
        else
        {
            btnSend.Enabled = btnDraft.Enabled = true;
        }

        txtCc.Text = cc = hdnCc.Value = !string.IsNullOrWhiteSpace(ed.Cc) ? Helper.ProcessData(ed.Cc, patientRecId, patientNumber) : ""; //ed.Cc;
        txtBcc.Text = bcc = hdnBcc.Value = !string.IsNullOrWhiteSpace(ed.Bcc) ? Helper.ProcessData(ed.Bcc, patientRecId, patientNumber) : ""; //ed.Bcc;
        txtSubject.Text = subject = hdnSubject.Value = Helper.ProcessData(ed.Subject, patientRecId, patientNumber); //ed.Subject;
        txtTemplateBody.Text = txtTemplateBodySMS.Text = body = hdnBody.Value = Helper.ProcessData(ed.Body, patientRecId, patientNumber); //ed.Body;
        txtTemplateLetter.Text = letter = hdnLetter.Value = Helper.ProcessData(ed.Letter, patientRecId, patientNumber);


        //chkLetter.Checked = Convert.ToBoolean(ed.RequireLetter);

        ////if (chkLetter.Checked)
        //if (Convert.ToBoolean(ed.RequireLetter))
        //{
        //    hlLetter.NavigateUrl = "~/TemplateWordEditor.aspx?TemplateId=" + id + "&page=mail";

        //    //Load template data
        //    string fileTemplateData = Server.MapPath("~/Attachments/Templates/" + id + "/Letter.docx");
        //    if (File.Exists(fileTemplateData))
        //    {
        //        //Load from saved file and mail merge and then load into editor
        //        using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
        //        {
        //            tx.Create();
        //            tx.Load(fileTemplateData, TXTextControl.StreamType.WordprocessingML);

        //            string data;
        //            tx.Save(out data, TXTextControl.StringStreamType.RichTextFormat);
        //            data = Helper.ProcessData(data, patientRecId, patientNumber); //populate with exact data
        //            Session["TemplateWordEditor"] = data;
        //        }
        //    }
        //}
        //else
        //{
        //    hlLetter.NavigateUrl = "~/TemplateWordEditor.aspx?TemplateId=0&page=mail";
        //}

        hasBusinessAttachment = Convert.ToBoolean(ed.AttachmentHasBusiness);
        hdnhasBusinessAttachment.Value = Convert.ToString(hasBusinessAttachment);

        selectAllAttachments = Convert.ToBoolean(ed.SelectAllAttachments);
        hdnselectAllAttachments.Value = Convert.ToString(selectAllAttachments);

        storePatientFile = Convert.ToBoolean(ed.StorePatientLetter);
        hdnstorePatientFile.Value = Convert.ToString(storePatientFile);

        if (storePatientFile)
        {
            patientFile.Visible = true;
            if (!string.IsNullOrWhiteSpace(ed.PatientFileName))
                txtPatientFileName.Text = patientFileName = hdnPatientFileName.Value = Helper.ProcessData(ed.PatientFileName, patientRecId, patientNumber);
            else
                txtPatientFileName.Text = patientFileName = hdnPatientFileName.Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentCategory))
                txtAttachmentCategory.Text = attachmentCategory = hdnAttachmentCategory.Value = Helper.ProcessData(ed.AttachmentCategory, patientRecId, patientNumber);
            else
                txtAttachmentCategory.Text = attachmentCategory = hdnAttachmentCategory.Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(ed.AttachmentDescription))
                txtAttachmentDescription.Text = attachmentDescription = hdnAttachmentDescription.Value = Helper.ProcessData(ed.AttachmentDescription, patientRecId, patientNumber);
            else
                txtAttachmentDescription.Text = attachmentDescription = hdnAttachmentDescription.Value = string.Empty;
        }
        else
        {
            patientFile.Visible = false;
            txtPatientFileName.Text = patientFileName = hdnPatientFileName.Value = string.Empty;
            txtAttachmentCategory.Text = attachmentCategory = hdnAttachmentCategory.Value = string.Empty;
            txtAttachmentDescription.Text = attachmentDescription = hdnAttachmentDescription.Value = string.Empty;
        }

        sendSMS = Convert.ToBoolean(ed.SendAsSMS);
        hdnSendSMS.Value = Convert.ToString(sendSMS);

        sendUnencryptedFile = Convert.ToBoolean(ed.SendUnencryptedFile);
        hdnSendUnencryptedFile.Value = Convert.ToString(sendUnencryptedFile);

        if (!sendSMS)
        {
            tabs.Visible = true;
            sms.Visible = false;
        }
        else
        {
            tabs.Visible = false;
            sms.Visible = true;
        }

        businessFilter = hdnbusinessFilter.Value = ed.AttachmentBusinessFilter;
        businessInclude = hdnbusinessInclude.Value = ed.AttachmentBusinessInclude;

        hdnShowPromptDialog.Value = "0";
        hdnPromptFileUrl.Value = hdnPromptFileName.Value = "";

        if (!Convert.ToBoolean(sendSMS))
        {
            //check for attachment folder
            GetAttachmentFiles("~/Attachments/Templates/" + TemplateId.ToString(), TemplateId, hasBusinessAttachment, ed.AttachmentBusinessFilter, ed.AttachmentBusinessInclude, Convert.ToBoolean(ed.PromptForAttachments), Convert.ToBoolean(ed.AttachmentHasDirectory), ed.AttachmentDirectoryPath, ed.AttachmentDirectoryFilter, ed.AttachmentDirectoryInclude, Convert.ToBoolean(ed.IncludeInstructions), ed.InstructionFilter, Convert.ToBoolean(ed.CombineMultipleInstructions));
        }

        //if (Convert.ToBoolean(ed.PromptForAttachments))
        //    tblMoreAttachments.Visible = true;
        //else
        //    tblMoreAttachments.Visible = false;

        //chooseTemplate.Visible = false;
        divEmail.Visible = true;
    }

    private void GetEmailDetailById(int id)
    {
        BAL_AMCPE.Emails em = new BAL_AMCPE.Emails();
        DAL_AMCPE.Email ed = em.GetEmailByEmailId(id);
        txtFrom.Text = ed.From;
        txtTo.Text = ed.To;
        txtCc.Text = ed.Cc;
        txtBcc.Text = ed.Bcc;
        txtSubject.Text = ed.Subject;

        storePatientFile = Convert.ToBoolean(ed.StorePatientLetter);
        hdnstorePatientFile.Value = Convert.ToString(storePatientFile);

        if (storePatientFile)
        {
            patientFile.Visible = true;
            txtPatientFileName.Text = patientFileName = hdnPatientFileName.Value = ed.PatientFileName;
            txtAttachmentCategory.Text = attachmentCategory = hdnAttachmentCategory.Value = ed.AttachmentCategory;
            txtAttachmentDescription.Text = attachmentDescription = hdnAttachmentDescription.Value = ed.AttachmentDescription;
        }
        else
        {
            patientFile.Visible = false;
            txtPatientFileName.Text = patientFileName = hdnPatientFileName.Value = string.Empty;
            txtAttachmentCategory.Text = attachmentCategory = hdnAttachmentCategory.Value = string.Empty;
            txtAttachmentDescription.Text = attachmentDescription = hdnAttachmentDescription.Value = string.Empty;
        }

        sendSMS = Convert.ToBoolean(ed.SendASSMS);
        hdnSendSMS.Value = Convert.ToString(sendSMS);

        sendUnencryptedFile = Convert.ToBoolean(ed.SendUnEncryptedPatientLetter);
        hdnSendUnencryptedFile.Value = Convert.ToString(sendUnencryptedFile);

        if (!sendSMS)
        {
            txtTemplateBody.Text = ed.Body;
            tabs.Visible = true;
            sms.Visible = false;
        }
        else
        {
            txtTemplateBodySMS.Text = ed.Body;
            tabs.Visible = false;
            sms.Visible = true;
        }

        txtTemplateLetter.Text = ed.Letter;

        //chkLetter.Checked = Convert.ToBoolean(ed.LetterRequired);

        //hdnTemplateId.Value = Convert.ToInt32(ed.EmailTemplateId).ToString();
        TemplateId = Convert.ToInt32(ed.EmailTemplateId);

        string emailUser = ed.SentBy.Replace("amc\\", "");


        //if (chkLetter.Checked)
        //if (Convert.ToBoolean(ed.LetterRequired))
        //{
        if (!string.IsNullOrEmpty(Request.QueryString["action"]))
        {
            action = hdnAction.Value = Convert.ToString(Request.QueryString["action"]).ToLower();

            //Load template data
            string fileUrl = "";
            string fileTemplateData = "";

            if (action == "senddraft")
            {
                fileUrl = "~/Attachments/Drafts/" + emailUser + "/" + EmailId + "/Letter.docx";
                fileTemplateData = Server.MapPath(fileUrl);
            }
            else if (action == "resend" || action == "forward")
            {
                fileUrl = "~/Attachments/Sent/" + emailUser + "/" + EmailId + "/Letter.docx";
                fileTemplateData = Server.MapPath(fileUrl);
            }

            //hlLetter.NavigateUrl = "~/TemplateWordEditor.aspx?url=" + fileUrl + "&page = mail";

            //if (File.Exists(fileTemplateData))
            //{
            //    //Load from saved file and mail merge and then load into editor
            //    using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            //    {
            //        tx.Create();
            //        tx.Load(fileTemplateData, TXTextControl.StreamType.WordprocessingML);

            //        string data;
            //        tx.Save(out data, TXTextControl.StringStreamType.RichTextFormat);
            //        data = Helper.ProcessData(data, patientRecId, patientNumber); //populate with exact data
            //        Session["TemplateWordEditor"] = data;
            //    }
            //}
        }
        //}
        //else
        //{
        //    hlLetter.NavigateUrl = "~/TemplateWordEditor.aspx?TemplateId=0";
        //}


        //check for attachment folder
        if (!string.IsNullOrEmpty(Request.QueryString["action"]) && Convert.ToBoolean(ed.HasAttachments))
        {
            action = hdnAction.Value = Convert.ToString(Request.QueryString["action"]).ToLower();

            if (action == "senddraft")
                GetAttachmentFiles("~/Attachments/Drafts/" + emailUser + "/" + EmailId.ToString());
            else if (action == "resend" || action == "forward")
                GetAttachmentFiles("~/Attachments/Sent/" + emailUser + "/" + EmailId.ToString());
        }

        //check if email is opened in view mode
        if (!string.IsNullOrEmpty(Request.QueryString["action"]))
        {
            action = hdnAction.Value = Convert.ToString(Request.QueryString["action"]).ToLower();
            if (action == "view")
            {
                if (Convert.ToBoolean(ed.HasAttachments))
                {
                    if (Convert.ToBoolean(ed.IsDraft))
                        GetAttachmentFiles("~/Attachments/Drafts/" + emailUser + "/" + EmailId.ToString());
                    else
                        GetAttachmentFiles("~/Attachments/Sent/" + emailUser + "/" + EmailId.ToString());
                }

                btnSend.Visible = btnDraft.Visible = false;
            }
            else
                btnSend.Visible = btnDraft.Visible = true;
        }

        tblMoreAttachments.Visible = true;

        chooseTemplate.Visible = false;
        divEmail.Visible = true;
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

                        businessFilter = Helper.ProcessData(businessFilter, patientRecId, patientNumber);
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

                                string sourceFile = Path.Combine(sourcePathFile, s.Name.Replace("..", ".").Replace("._", "."));

                                string destFile = string.Empty;
                                string newFileName = string.Empty;

                                if (businessFilter.Contains("!"))
                                {
                                    newFileName = s.Name.Remove(s.Name.Replace("..", ".").Replace("._", ".").LastIndexOf(".")) + fileCount++ + Path.GetExtension(sourceFile);
                                    destFile = Path.Combine(savePath, newFileName);
                                }
                                else
                                {
                                    destFile = Path.Combine(savePath, s.Name.Replace("..", ".").Replace("._", "."));
                                }

                                File.Copy(sourceFile, destFile, true);

                                string fileName = businessFilter.Contains("!") ? newFileName : s.Name.Replace("..", ".");
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
                                            doc.Save(Path.Combine(savePath, fileName.Replace("..", "").Replace("._", ".")));

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
                                            pdfDoc.Save(Path.Combine(savePath, fileName.Replace("..", ".").Replace("._", ".")));

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
                                            pdfDoc.Save(destFile.Replace("..", "."));
                                        }
                                    }
                                }

                                string attachedFilePath = filePath.Replace("~", "");

                                attachments.Add(new Attachments { Name = fileName.Replace("..", ".").Replace("._", "."), Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fileName.Replace("..", ".").Replace("._", "."), Include = true });
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

                            string sourceFile = Path.Combine(sourcePathFile, s.Name.Replace("..", ".").Replace("._", "."));
                            //string newFileName = s.Name.Remove(s.Name.LastIndexOf(".")) + fileCount++ + Path.GetExtension(sourceFile);
                            //string destFile = Path.Combine(savePath, newFileName);
                            string destFile = Path.Combine(savePath, s.Name.Replace("..", ".").Replace("._", "."));
                            File.Copy(sourceFile, destFile, true);

                            string attachedFilePath = filePath.Replace("~", "");

                            if (promptForAttachments)
                            {
                                directoryAttachments.Add(new Attachments { Name = s.Name.Replace("..", ".").Replace("._", "."), Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + s.Name.Replace("..", ".").Replace("._", "."), Include = true });
                            }
                            else
                            {
                                attachments.Add(new Attachments { Name = s.Name.Replace("..", ".").Replace("._", "."), Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + s.Name.Replace("..", ".").Replace("._", "."), Include = true });
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
                    DAL_AMCPE.InstructionTemplate ed = et.GetDocTemplateByID(Convert.ToInt32(ddlTemplates.SelectedValue));

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
                                            doc.Save(OutputLocation.Replace("..", ".").Replace("._", "."));

                                            et.delete_tempfile();
                                            Session["fileinfo"] = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");

                                            FileInfo fileidnfo = new FileInfo(OutputLocation);
                                            string fileName = fileidnfo.Name.Replace("..", ".").Replace("._", ".");
                                            File.Copy(OutputLocation, savePath1 + "\\" + fileName.Replace("..", ".").Replace("._", "."), true);
                                            File.Delete(OutputLocation);
                                            FileInfo fileInfo2 = new FileInfo(savePath1 + "\\" + fileName.Replace("..", ".").Replace("._", "."));

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

                                                attachments.Add(new Attachments { Name = fileInfo2.Name.Replace("..", ".").Replace("._", "."), Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath1, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + fileInfo2.Name.Replace("..", ".").Replace("._", "."), Include = true });
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
                                Merged_pdf_filepath = et.MergeAllPDF(allPDFs, savePath1 + pInstFile.Replace("..", ".").Replace("._", "."));

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

                                string f_name = fInfo.Name.Replace("..", ".").Replace("._", ".");
                                attachments.Add(new Attachments { Name = f_name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = savePath1, FileWebURL = URLRewrite.BasePath() + "/TempFiles" + "/" + user + "/" + f_name, Include = true });
                                hdnAttachURLs.Value += fInfo.FullName + "|";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


            if (promptForAttachments && directoryAttachments.Count > 0)
            {
                hdnShowPromptDialog.Value = "1";
                rptPromptAttachments.DataSource = directoryAttachments;
            }
            else
            {
                hdnShowPromptDialog.Value = "0";
                rptPromptAttachments.DataSource = null;
            }
            rptPromptAttachments.DataBind();

            if (attachments.Count > 0)
                rptAttachments.DataSource = attachments;
            else
                rptAttachments.DataSource = null;
            rptAttachments.DataBind();

        }
        catch (Exception ex)
        { }
    }

    //protected void btnAdd_Click(object sender, EventArgs e)
    //{
    //    attachingMore = true;
    //    GetAttachmentFiles();
    //}

    protected void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            ProcessEmailData();
            SendMail(true);

            // do not open preview popup 
            //// open preview window
            //ltrFrom.Text = from;
            //ltrTo.Text = to;
            //ltrCc.Text = cc;
            //ltrBcc.Text = bcc;
            //ltrSubject.Text = subject;
            //ltrBody.Text = body;
            //ScriptManager.RegisterStartupScript(this, GetType(), "Show Modal Popup", "showmodalpopup();", true);
        }
        catch (Exception)
        {

        }
    }


    protected void btnPreview_Click(object sender, EventArgs e)
    {
        ltrFrom.Text = txtFrom.Text;
        ltrTo.Text = txtTo.Text;
        ltrCc.Text = txtCc.Text;
        ltrBcc.Text = txtBcc.Text;
        ltrSubject.Text = txtSubject.Text;
        ltrBody.Text = txtTemplateBody.Text;
        ScriptManager.RegisterStartupScript(this, GetType(), "Show Modal Popup", "showmodalpopup();", true);
    }

    protected void btnDraft_Click(object sender, EventArgs e)
    {
        ProcessEmailData();
        SendMail(false);

        //ProcessEmailFields();
        //SaveEmail(true);
        //Response.Redirect("Drafts.aspx");
    }

    private void DeleteFilesFromUserTempDirectory()
    {
        string userTempDirectory = Server.MapPath("~/Attachments/Temp/" + user + "/");
        if (Directory.Exists(userTempDirectory))
            Array.ForEach(Directory.GetFiles(userTempDirectory), File.Delete);
    }

    private void SendMail(bool sendMail)
    {
        int emailId = 0;
        try
        {
            // First identify if this email has any attachments
            bool emailHasAttachments = false;
            string userDirectory = "", userServerDirectory;
            List<string> attachmentUrls = new List<string>();
            DateTime dt = DateTime.Now;

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

                if (hdnPromptFileUrl.Value != "")
                {
                    emailHasAttachments = true;
                }
            }

            //foreach (RepeaterItem row in rptPromptAttachments.Items)
            //{
            //    HiddenField hdnInclude = (HiddenField)row.FindControl("hdnInclude");
            //    if (hdnInclude.Value == "1")
            //    {
            //        emailHasAttachments = true;
            //        break;
            //    }
            //}

            // No need to check this now
            //if (emailHasAttachments || Request.Files.Count > 0)
            //    emailHasAttachments = true;
            //else
            //    emailHasAttachments = false;


            // Now save Email in DB and get Id of email sent so that Attachments can be saved in folder on behalf of email id

            Emails el = new Emails();
            if (EmailId == 0 || action == "resend" || action == "forward")
                el.obj = new DAL_AMCPE.Email();
            else
                el.obj = el.GetEmailByEmailId(EmailId);

            el.obj.From = txtFrom.Text; //from;
            el.obj.To = txtTo.Text; //to;
            el.obj.Cc = txtCc.Text; //cc;
            el.obj.Bcc = txtBcc.Text; //bcc;
            el.obj.Subject = txtSubject.Text; //subject;
            el.obj.Body = !sendSMS ? txtTemplateBody.Text : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(txtTemplateBodySMS.Text.Replace("<br />", ""), "<(.|\n)*?>", "")); //txtTemplateBody.Text; //body;
            el.obj.Letter = txtTemplateLetter.Text; //letter;

            el.obj.SentBy = "amc\\" + user;
            el.obj.SentOn = dt;
            //el.obj.HasAttachments = attachments.Count() > 0 ? true : false;
            el.obj.HasAttachments = emailHasAttachments;
            el.obj.EmailTemplateId = TemplateId;
            el.obj.IsDraft = !sendMail;

            el.obj.StorePatientLetter = storePatientFile;
            el.obj.PatientFileName = patientFileName;
            el.obj.AttachmentCategory = attachmentCategory;
            el.obj.AttachmentDescription = attachmentDescription;
            el.obj.SendASSMS = sendSMS;
            el.obj.SendUnEncryptedPatientLetter = sendUnencryptedFile;

            el.obj.IsDeleted = false;

            //el.obj.LetterRequired = ((Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"]))) ? true : false); //chkLetter.Checked;

            // manage task
            if (EmailId == 0 && !sendMail) // Create New Task
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                SaveTask("new", guid, txtSubject.Text, dt);

                el.obj.ActivityRecId = guid;
                el.obj.PatientRecId = patientRecId;
                el.obj.PatientNumber = patientNumber;
                el.obj.PatientFullName = patientFullName;
            }
            else if (EmailId > 0 && !sendMail) // Add or update task
            {
                if (string.IsNullOrWhiteSpace(el.obj.ActivityRecId)) // Create new task
                {
                    string guid = Guid.NewGuid().ToString().Replace("-", "");
                    SaveTask("new", guid, txtSubject.Text, dt);

                    el.obj.ActivityRecId = guid;
                }
                else // update task
                {
                    SaveTask("edit", el.obj.ActivityRecId, txtSubject.Text, dt);
                }
            }
            else if (EmailId > 0 && sendMail && !(action == "resend" || action == "forward")) // Create new task
            {
                string guid = Guid.NewGuid().ToString().Replace("-", "");
                SaveTask("new", guid, txtSubject.Text, dt);

                el.obj.ActivityRecId = guid;

                //commented on 26.12.2017
                //as draft email is being sent so patient record should not be updated
                //el.obj.PatientRecId = patientRecId;
                //el.obj.PatientNumber = patientNumber;
                //el.obj.PatientFullName = patientFullName;
            }
            //else if (EmailId > 0 && sendMail) // Complete task
            //{
            //    SaveTask("complete", el.obj.ActivityRecId, txtSubject.Text, dt);
            //}
            else
            {
                el.obj.ActivityRecId = "";
                el.obj.PatientRecId = patientRecId;
                el.obj.PatientNumber = patientNumber;
                el.obj.PatientFullName = patientFullName;
            }

            emailId = el.Save();

            if (emailId > 0)
            {
                // Save files in Sent folder if email has attachments
                if (emailHasAttachments)
                {
                    if (sendMail)
                        userDirectory = "~/Attachments/Sent/" + user + "/";
                    else
                        userDirectory = "~/Attachments/Drafts/" + user + "/";

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

                    //sourcePath = Server.MapPath("~/Attachments/Templates/" + TemplateId.ToString() + "/"); //get file path from template attachments directory directly

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
                            File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName.Replace("..", ".").Replace("._", ".")), true);
                            attachmentUrls.Add(Path.Combine(targetPath, fileName.Replace("..", ".").Replace("._", ".")));
                        }
                    }

                    //attach selected files from prompt dialog
                    if (hdnPromptFileUrl.Value != "" && hdnPromptFileName.Value != "")
                    {
                        string[] fileUrls = hdnPromptFileUrl.Value.Split('$');
                        string[] fileNames = hdnPromptFileName.Value.Split('$');
                        int length = fileUrls.Length;
                        for (int i = 0; i < length; i++)
                        {
                            //skip file if it contains letter generated file in case email is being resent or forwarded
                            if (!fileNames[i].ToLower().Contains(patientFullName.ToLower() + "_"))
                            {
                                File.Copy(Path.Combine(fileUrls[i], fileNames[i]), Path.Combine(targetPath, fileNames[i].Replace("..", ".").Replace("._", ".")), true);
                                attachmentUrls.Add(Path.Combine(targetPath, fileNames[i].Replace("..", ".").Replace("._", ".")));
                            }
                        }
                    }
                }

                //Either create encrypted PDF or save as docx file
                //if (sendMail && Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
                if (sendMail && !string.IsNullOrWhiteSpace(el.obj.Letter) && !sendSMS && !(action == "resend" || action == "forward"))
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
                                pdfDoc.Save(savePath + "\\" + filename.Replace("..", ".").Replace("._", "."));
                                using (DAL_AMCPE.GMEEDevelopmentEntities ATCH = new DAL_AMCPE.GMEEDevelopmentEntities())
                                {
                                    ATCH.obj_sp_BulkPrint_Attachment("", "", patientRecId, @"amc\" + user, outputLocation.Replace("\\\\", "\\"), Path.GetFileName(outputLocation), id, attachmentDescription, attachmentCategory);
                                }
                            }
                            catch (Exception ex) { }
                        }

                        //delete any letter file if it already exists, added on 02.10.2017
                        try
                        {
                            if (attachmentUrls.Contains(pathToSaveServer + "\\" + filename.Replace("..", ".").Replace("._", ".")))
                            {
                                attachmentUrls.Remove(pathToSaveServer + "\\" + filename.Replace("..", ".").Replace("._", "."));
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
                        pdfDoc.Save(pathToSaveServer + "\\" + filename.Replace("..", ".").Replace("._", "."));

                        try
                        {
                            File.Delete(pathToSaveServer + "\\" + filenameToBeDeleted);
                            File.Delete(pathToSaveServer + "\\" + htmlFile);
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    attachmentUrls.Add(Path.Combine(pathToSaveServer, filename.Replace("..", ".").Replace("._", ".")));
                    emailHasAttachments = true;


                    ////get patient dob and set as password
                    //PatientActivity pAct = new PatientActivity();
                    //string password = pAct.GetPatientDOBByPatientNumber(patientNumber);

                    //byte[] USER = Encoding.ASCII.GetBytes(password);
                    //PdfReader reader = new PdfReader(pathToSaveServer + "\\" + filename);
                    //PdfStamper stamper = new PdfStamper(reader, new FileStream(pathToSaveServer + "\\" + filename, FileMode.OpenOrCreate));
                    //stamper.SetEncryption(USER, null, PdfWriter.AllowPrinting, PdfWriter.ENCRYPTION_AES_128);
                    //stamper.Close();


                    //string templateData = Convert.ToString(Session["TemplateWordEditor"]); //Helper.ProcessData(Convert.ToString(Session["TemplateWordEditor"]), patientRecId, patientNumber);
                    //using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
                    //{
                    //    tx.Create();

                    //    // load the data from text control
                    //    tx.Load(templateData, TXTextControl.StringStreamType.RichTextFormat);

                    //    TXTextControl.SaveSettings saveSettings = new TXTextControl.SaveSettings();
                    //    //get patient dob and set as password
                    //    PatientActivity pAct = new PatientActivity();
                    //    saveSettings.UserPassword = pAct.GetPatientDOBByPatientNumber(patientNumber);

                    //    //Save the document as PDF
                    //    string pathToSaveServer = Server.MapPath("~/Attachments/Sent/" + user + "/" + emailId);
                    //    if (!Directory.Exists(pathToSaveServer))
                    //    {
                    //        Directory.CreateDirectory(pathToSaveServer);
                    //    }

                    //    string pdfName = patientFullName + "_" + DateTime.Now.ToString("dd_MM_yy") + ".pdf";
                    //    tx.Save(pathToSaveServer + "\\" + pdfName, TXTextControl.StreamType.AdobePDF, saveSettings);
                    //    attachmentUrls.Add(Path.Combine(pathToSaveServer, pdfName));

                    //    emailHasAttachments = true;
                    //}
                }

                //else if (!sendMail && Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
                //{
                //    using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
                //    {
                //        tx.Create();
                //        string data = Convert.ToString(Session["TemplateWordEditor"]);
                //        tx.Load(data, TXTextControl.StringStreamType.RichTextFormat);

                //        string pathToSaveServer = Server.MapPath("~/Attachments/Drafts/" + user + "/" + emailId);
                //        if (!Directory.Exists(pathToSaveServer))
                //        {
                //            Directory.CreateDirectory(pathToSaveServer);
                //        }
                //        string fileTemplateName = pathToSaveServer + "\\Letter.docx";
                //        tx.Save(fileTemplateName, TXTextControl.StreamType.WordprocessingML);
                //    }
                //}

                //Session["TemplateWordEditor"] = null;


                //Update email attachment flag
                Emails el2 = new Emails();
                el2.obj = el2.GetEmailByEmailId(emailId);
                el2.obj.HasAttachments = emailHasAttachments;
                el2.Save();


                if (sendMail)
                {
                    //Now send mail with attachments(if any)
                    FlexiMail ml = new FlexiMail();
                    ml.From = txtFrom.Text; //from; // TODO: handle name other than email id also like AMC info@amc.com
                    ml.To = txtTo.Text;
                    ml.CC = txtCc.Text;
                    ml.BCC = (txtBcc.Text).Trim(',');
                    ml.Subject = txtSubject.Text;
                    ml.MailBody = !sendSMS ? txtTemplateBody.Text : HttpUtility.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(txtTemplateBodySMS.Text.Replace("<br />", ""), "<(.|\n)*?>", ""));
                    ml.MailBodyManualSupply = true;
                    ml.IsBodyHtml = !sendSMS;
                    if (emailHasAttachments)
                        ml.AttachFile = attachmentUrls.ToArray();
                    else
                        ml.AttachFile = null;

                    //string[] attachments = hdnAttachURLs.Value.TrimEnd('|').Split('|'); //new string[hdnAttachURLs.Value.TrimEnd('|').Split('|').Count()];
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

            // delete files from user temp folder
            DeleteFilesFromUserTempDirectory();


            Session["dataction"] = "s";
            if (sendMail)
                Response.Redirect("SentEmails.aspx?PatientNumber=" + Convert.ToString(Session["PatientNumber"]), false);
            else
                Response.Redirect("Drafts.aspx", false);
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

            Session["dataction"] = "e";
            Session["dataerror"] = ex.Message;
            if (sendMail)
                Response.Redirect("SentEmails.aspx?PatientNumber=" + Convert.ToString(Session["PatientNumber"]));
            else
                Response.Redirect("Drafts.aspx");
        }

    }

    private void CopyFolder(string sourceFolder, string destFolder)
    {
        string[] files = Directory.GetFiles(sourceFolder);
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destFolder, name);
            File.Copy(file, dest);
        }
        string[] folders = Directory.GetDirectories(sourceFolder);
        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder);
            CopyFolder(folder, dest);
        }
    }

    private void SaveEmail(bool saveAsDraft)
    {
        try
        {
            string[] attachments = hdnAttachURLs.Value.TrimEnd('|').Split('|');
            // Save data in Emails table
            Emails el = new Emails();
            el.obj = new DAL_AMCPE.Email();
            el.obj.From = from;
            el.obj.To = to;
            el.obj.Cc = cc;
            el.obj.Bcc = bcc;
            el.obj.Subject = subject;
            el.obj.Body = body;
            el.obj.Letter = letter;
            el.obj.SentBy = "amc\\" + user;
            el.obj.SentOn = DateTime.Now;
            el.obj.HasAttachments = attachments.Count() > 0 ? true : false;
            el.obj.IsDraft = saveAsDraft;
            el.obj.IsDeleted = false;
            el.Save();

            // TODO:
            //// Save attached files with email in User specific folder
            //foreach (string s in attachments)
            //{
            //    string fileName = Path.GetFileName(s);
            //    string targetPath = Server.MapPath("~/SentEmailAttachments");
            //    System.IO.File.Copy(s, Path.Combine(targetPath, fileName), true);
            //}

            //// Delete attachments from Temp folder
            //Directory.Delete(userDirectory, true);
        }
        catch (Exception ex)
        {

        }
    }

    private void ProcessEmailFields()
    {
        try
        {
            from = hdnFrom.Value = Helper.ProcessData(txtFrom.Text, patientRecId, patientNumber);
            to = hdnTo.Value = Helper.ProcessData(txtTo.Text, patientRecId, patientNumber);
            cc = hdnCc.Value = !string.IsNullOrWhiteSpace(txtCc.Text) ? Helper.ProcessData(txtCc.Text, patientRecId, patientNumber) : "";
            bcc = hdnBcc.Value = !string.IsNullOrWhiteSpace(txtBcc.Text) ? Helper.ProcessData(txtBcc.Text, patientRecId, patientNumber) : "";
            subject = hdnSubject.Value = Helper.ProcessData(txtSubject.Text, patientRecId, patientNumber);
            body = hdnBody.Value = Helper.ProcessData(txtTemplateBody.Text, patientRecId, patientNumber);
            letter = hdnLetter.Value = Helper.ProcessData(txtTemplateLetter.Text, patientRecId, patientNumber);
        }
        catch (Exception ex)
        {

        }
    }

    private void ProcessEmailData()
    {
        try
        {
            //removed as no preview is needed now so all fields have been processed already
            //// find exact value of different email paramanetes according patient recid 
            //ProcessEmailFields();

            string ud = Server.MapPath("~/Attachments/Temp/" + user + "/");
            if (!Directory.Exists(ud))
            {
                Directory.CreateDirectory(ud);
            }
            if (!Directory.Exists(ud + "\\2"))
            {
                Directory.CreateDirectory(ud + "\\2");
            }
            else
            {
                //// delete any existing files from user temp folder
                //Array.ForEach(Directory.GetFiles(ud), File.Delete);

                //// Move files from temp folder to /2 folder
                //string[] files = Directory.GetFiles(ud);


                //// Copy the files and overwrite destination files if they already exist.
                //foreach (string s in files)
                //{
                //    // Use static Path methods to extract only the file name from the path.
                //    fileName = Path.GetFileName(s);
                //    System.IO.File.Move(s, Path.Combine(ud + "\\2", fileName));
                //}
            }

            fileName = "";
            //sourcePath = Server.MapPath("~/Attachments/Templates/" + TemplateId.ToString() + "/");
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
                    File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName.Replace("..", ".").Replace("._", ".")), true);
                }
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile objHttpPostedFile = (HttpPostedFile)Request.Files[i];
                if (objHttpPostedFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(objHttpPostedFile.FileName);
                    objHttpPostedFile.SaveAs(Path.Combine(targetPath, fileName.Replace("..", ".").Replace("._", ".")));
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
                System.IO.File.Move(s, Path.Combine(ud, fileName.Replace("..", ".").Replace("._", ".")));
            }

            // bind repeater with these stored files
            GetAttachmentFiles("~/Attachments/Temp/" + user + "/");
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SentEmails.aspx?PatientNumber=" + Convert.ToString(Session["PatientNumber"]));
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

    protected void ddlEmailTemplateCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
        int templateCategoryId = Convert.ToInt32(ddlEmailTemplateCategories.SelectedValue);
        GetEmailTemplatesByCategoryId(templateCategoryId);
    }

    protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        TemplateId = Convert.ToInt32(ddlTemplates.SelectedValue);
        hdnTemplateId.Value = TemplateId.ToString();
        GetTemplateDetailById(TemplateId);
    }

    private void SaveTask(string status, string recId, string subject, DateTime? dt)
    {
        PatientActivity pa = new PatientActivity();

        if (status == "new")
        {
            pa.objActivity = new DAL_AMCPE.Activity();
            pa.objActivity.RecId = recId;
            pa.objActivity.CreatedBy = pa.objActivity.LastModBy = Convert.ToString(Session["UserId"]);
            pa.objActivity.CreatedDateTime = pa.objActivity.LastModDateTime = DateTime.Now;
            pa.objActivity.StartDateTime = DateTime.Now;
            pa.objActivity.Purpose = "Your Email has been saved to Drafts";
            pa.objActivity.Subject = "Your Email has been saved to Drafts";
            pa.objActivity.Notes = "Your email has been saved to Drafts. Please access the Patient Email Portal to continue editing your email - " +
                                   "(" + subject + " - AND " + Convert.ToDateTime(dt).ToString("dd/MM/yyyy hh:mm tt");
            pa.objActivity.ActivityResult = "Task - " + user.ToUpper();
            pa.objActivity.ActivityType = "Task";
            pa.objActivity.Status = "In Progress";
            pa.objActivity.ParentLink_RecID = patientRecId;
            pa.objActivity.xpDisplayName = patientFullName;

            pa.objActivity.GeographyName = "APAC";
            pa.objActivity.ParentLink_Category = "Contact";
            pa.Save(0);
        }
        else if (status == "edit")
        {
            pa.objActivity = pa.GetActivityByRecId(recId);
            pa.objActivity.LastModBy = Convert.ToString(Session["UserId"]);
            pa.objActivity.LastModDateTime = DateTime.Now;
            pa.objActivity.Notes = pa.objActivity.Notes + Environment.NewLine + "Your email has been saved to Drafts. Please access the Patient Email Portal to continue editing your email - " +
                                   "(" + subject + " - AND " + Convert.ToDateTime(dt).ToString("dd/MM/yyyy hh:mm tt") + ")";

            pa.Save(1);
        }
        else if (status == "complete")
        {
            pa.objActivity = pa.GetActivityByRecId(recId);
            pa.objActivity.LastModBy = Convert.ToString(Session["UserId"]);
            pa.objActivity.LastModDateTime = DateTime.Now;
            pa.objActivity.Notes = pa.objActivity.Notes + Environment.NewLine + "Email has been succesfully sent - " +
                                   "(" + Convert.ToString(Session["UserId"]) + " - AND - " + Convert.ToDateTime(dt).ToString("dd/MM/yyyy hh:mm tt") + ")";
            pa.objActivity.Status = "Completed";

            pa.Save(1);
        }

    }
}