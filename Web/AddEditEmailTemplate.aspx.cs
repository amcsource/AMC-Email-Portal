using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Web.Services;
using System.IO;

public partial class AddEditEmailTemplate : System.Web.UI.Page
{
    public int Id, responseCode;
    public string user, fileName, targetPath;
    public static string filePrefix = "[Browsed]";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Id = 0;
            user = "";

            BindData();
            BindTagCategories();

            user = Convert.ToString(Session["UserId"]).Replace("amc\\", "");
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]);
                GetTemplateById(Id);
            }
            else
            {
                rptAttachments.DataSource = null;
                rptAttachments.DataBind();
            }

            hdnId.Value = Id.ToString();
            hdnCurrentUser.Value = user;
        }
        else
        {
            Id = Convert.ToInt32(hdnId.Value);
            user = hdnCurrentUser.Value;
        }

        // Enable Submit button according user permission
        if (Id == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateTemplate;
    }

    private void BindData()
    {
        // commented as ahmz asked to remove this
        //BAL_AMCPE.SQLQueries sq = new SQLQueries();
        //rptQueryList.DataSource = sq.GetSQLQueries();
        //rptQueryList.DataBind();

        BAL_AMCPE.MasterTemplates mt = new BAL_AMCPE.MasterTemplates();
        ddlMasterTemplates.DataSource = mt.GetMasterTemplates();
        ddlMasterTemplates.DataTextField = "Name";
        ddlMasterTemplates.DataValueField = "Id";
        ddlMasterTemplates.DataBind();
        ddlMasterTemplates.Items.Insert(0, new ListItem("No Template", "0"));

        BAL_AMCPE.EmailTemplateCategory tc = new BAL_AMCPE.EmailTemplateCategory();
        ddlEmailTemplateCategories.DataSource = tc.GetEmailTemplateCategories();
        ddlEmailTemplateCategories.DataTextField = "CategoryName";
        ddlEmailTemplateCategories.DataValueField = "Id";
        ddlEmailTemplateCategories.DataBind();
        ddlEmailTemplateCategories.Items.Insert(0, new ListItem("No Category", "0"));
    }

    private void BindTagCategories()
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        var data = tc.GetTagCategories();
        if (data.Count > 0)
            ddlTagSelectorTables.DataSource = data;
        else
        {
            ddlTagSelectorTables.DataSource = null;
        }
        ddlTagSelectorTables.DataTextField = "CategoryName";
        ddlTagSelectorTables.DataValueField = "Id";
        ddlTagSelectorTables.DataBind();
    }

    private void GetTemplateById(int id)
    {
        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        et.obj = et.GetTemplateByID(id);
        txtTemplateName.Text = et.obj.Name;
        txtFrom.Text = et.obj.From;
        txtTo.Text = et.obj.To;
        txtCc.Text = et.obj.Cc;
        txtBcc.Text = et.obj.Bcc;
        txtSubject.Text = et.obj.Subject;
        ddlMasterTemplates.SelectedValue = !string.IsNullOrEmpty(Convert.ToString(et.obj.TemplateId)) ? Convert.ToString(et.obj.TemplateId) : "0";
        ddlEmailTemplateCategories.SelectedValue = !string.IsNullOrEmpty(Convert.ToString(et.obj.EmailTemplateCategoryId)) ? Convert.ToString(et.obj.EmailTemplateCategoryId) : "0";
        txtTemplateBody.Text = txtTemplateBodySMS.Text = et.obj.Body;
        chkBusiness.Checked = Convert.ToBoolean(et.obj.AttachmentHasBusiness);
        txtFilterBusiness.Text = et.obj.AttachmentBusinessFilter;
        rdoListBusiness.SelectedValue = et.obj.AttachmentBusinessInclude;
        chkDirectory.Checked = Convert.ToBoolean(et.obj.AttachmentHasDirectory);
        txtDirectoryPath.Text = et.obj.AttachmentDirectoryPath;
        txtFilterDirectory.Text = et.obj.AttachmentDirectoryFilter;
        rdoListDirectory.SelectedValue = et.obj.AttachmentDirectoryInclude;
        chkPrompt.Checked = Convert.ToBoolean(et.obj.PromptForAttachments);
        chkSelectAll.Checked = Convert.ToBoolean(et.obj.SelectAllAttachments);
        
        //chkLetter.Checked = Convert.ToBoolean(et.obj.RequireLetter);
        chkPatientLetter.Checked = Convert.ToBoolean(et.obj.StorePatientLetter);
        txtPatientFileName.Text = et.obj.PatientFileName;
        txtAttachmentCategory.Text = et.obj.AttachmentCategory;
        txtAttachmentDescription.Text = et.obj.AttachmentDescription;

        chkSMS.Checked = Convert.ToBoolean(et.obj.SendASSMS);
        chkUnecryptedFile.Checked = Convert.ToBoolean(et.obj.SendUnEncryptedPatientLetter);
        
        txtTemplateLetter.Text = et.obj.Letter;

        chkInstruction.Checked = Convert.ToBoolean(et.obj.IncludeInstructions);
        txtInstruction.Text = et.obj.InstructionFilter;
        chkCombineInstructions.Checked = Convert.ToBoolean(et.obj.CombineMultipleInstructions);



        //if (chkLetter.Checked)
        //if (Convert.ToBoolean(et.obj.RequireLetter))
        //{
        //    hlLetter.NavigateUrl = "TemplateWordEditor.aspx?TemplateId=" + id;

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
        //            Session["TemplateWordEditor"] = data;
        //        }
        //    }
        //}
        //else
        //{
        //    hlLetter.NavigateUrl = "TemplateWordEditor.aspx?TemplateId=0";
        //}



        //check for attachment folder
        GetAttachmentFiles("~/Attachments/Templates/" + id.ToString());

        // Enable Submit button according user permission
        if ((Convert.ToString(Session["UserId"]).ToLower() == et.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditTemplate) || (Convert.ToString(Session["UserId"]).ToLower() != et.obj.CreatedBy.ToLower() && PermissionSession.UserPermission.CanEditOtherTemplate))
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;

    }

    private void GetAttachmentFiles(string filePath)
    {
        hdnAttachURLs.Value = "";
        string sourcePath = Server.MapPath(filePath);
        if (Directory.Exists(sourcePath))
        {
            var di = new DirectoryInfo(sourcePath);
            FileInfo[] myfiles = new FileInfo[] { };

            myfiles = di.GetFiles();

            //EmailAttachments ea = new EmailAttachments();
            //List<string> browsedFiles = ea.GetBrowsedAttachmentsByTemplateId(Id);
            //myfiles = di.GetFiles().Where(a => browsedFiles.Contains(a.Name)).ToArray();


            //myfiles = di.GetFiles().Where(a => a.Name.StartsWith(filePrefix)).ToArray();
            List<Attachments> attachments = new List<Attachments>();
            string[] sizes = { "Bytes", "KB", "MB", "GB" };
            double len;
            int order;
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

                    attachments.Add(new Attachments { Name = file.Name, Size = String.Format("{0:0.##} {1}", len, sizes[order]), FileURL = sourcePath });

                    //insert url of every being attached in list
                    //attachURLs.Add(file.FullName);
                    hdnAttachURLs.Value += file.FullName + "|";
                }
            }
            if (attachments.Count > 0)
                rptAttachments.DataSource = attachments;
            else
                rptAttachments.DataSource = null;
            rptAttachments.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();

        if (Id == 0)
        {
            et.obj = new DAL_AMCPE.EmailTemplate();
            et.obj.CreatedBy = Convert.ToString(Session["UserId"]);
            et.obj.CreatedOn = DateTime.Now;
        }
        else
        {
            et.obj = et.GetTemplateByID(Id);
            et.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
            et.obj.UpdatedOn = DateTime.Now;

            ProcessAttachments();
        }
        et.obj.Name = txtTemplateName.Text.Trim();
        et.obj.From = txtFrom.Text.Trim();
        et.obj.To = txtTo.Text.Trim();
        et.obj.Cc = txtCc.Text.Trim();
        et.obj.Bcc = txtBcc.Text.Trim();
        et.obj.Subject = txtSubject.Text.Trim();

        if (ddlMasterTemplates.SelectedValue != "0")
            et.obj.TemplateId = Convert.ToInt32(ddlMasterTemplates.SelectedValue);
        else
            et.obj.TemplateId = null;

        if (ddlEmailTemplateCategories.SelectedValue != "0")
            et.obj.EmailTemplateCategoryId = Convert.ToInt32(ddlEmailTemplateCategories.SelectedValue);
        else
            et.obj.EmailTemplateCategoryId = null;

        if (!chkSMS.Checked)
            et.obj.Body = txtTemplateBody.Text.Trim();
        else
            et.obj.Body = txtTemplateBodySMS.Text.Trim();

        et.obj.AttachmentHasBusiness = chkBusiness.Checked;
        et.obj.AttachmentBusinessFilter = txtFilterBusiness.Text;
        et.obj.AttachmentBusinessInclude = rdoListBusiness.SelectedValue;

        et.obj.AttachmentHasDirectory = chkDirectory.Checked;
        et.obj.AttachmentDirectoryPath = txtDirectoryPath.Text;
        et.obj.AttachmentDirectoryFilter = txtFilterDirectory.Text;
        et.obj.AttachmentDirectoryInclude = rdoListDirectory.SelectedValue;
        
        et.obj.PromptForAttachments = chkPrompt.Checked;
        et.obj.SelectAllAttachments = chkSelectAll.Checked;
        
        //et.obj.RequireLetter = ((Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"]))) ? true : false); //chkLetter.Checked;

        et.obj.Letter = txtTemplateLetter.Text.Trim();

        et.obj.StorePatientLetter = chkPatientLetter.Checked;
        et.obj.PatientFileName = txtPatientFileName.Text.Trim();
        et.obj.AttachmentCategory = txtAttachmentCategory.Text.Trim();
        et.obj.AttachmentDescription = txtAttachmentDescription.Text.Trim();

        et.obj.SendASSMS = chkSMS.Checked;
        et.obj.SendUnEncryptedPatientLetter = chkUnecryptedFile.Checked;

        et.obj.IncludeInstructions = chkInstruction.Checked;
        et.obj.InstructionFilter = txtInstruction.Text;
        et.obj.CombineMultipleInstructions = chkCombineInstructions.Checked;

        et.obj.DeletedBy = "";
        //et.obj.DeletedOn = DateTime.Now;
        et.obj.IsDeleted = false;

        responseCode = et.Save();

        if (responseCode == -1)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
        else if (responseCode == 0)
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
        else
        {
            string sourcePath, fileName;
            targetPath = Server.MapPath("~/Attachments/Templates/" + et.obj.Id.ToString());

            //if (Directory.Exists(targetPath))
            //{
            //    Directory.Delete(targetPath, true);
            //    System.Threading.Thread.Sleep(500); // Intentionally put this delay so that if code runs fast then it does not create issue with file access like files is already in use.
            //}

            //Commented on 21.01.2016 Not needed
            //if (chkDirectory.Checked && Convert.ToInt32(hdnIsAttachmentAdded.Value) == 1)
            //{
            //    sourcePath = txtDirectoryPath.Text;

            //    BAL_AMCPE.EmailAttachments ea = new EmailAttachments();
            //    ea.Delete(et.obj.Id);

            //    if (Directory.Exists(sourcePath))
            //    {
            //        //string[] files = new string[] { };
            //        //if(!string.IsNullOrWhiteSpace(txtFilterDirectory.Text))
            //        //    files = Directory.GetFiles(sourcePath, "*", chkSelectAll.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            //        //else
            //        //    files = Directory.GetFiles(sourcePath, txtFilterDirectory.Text, chkSelectAll.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            //        var di = new DirectoryInfo(sourcePath);
            //        FileInfo[] myfiles = new FileInfo[] { };
            //        string file = "";

            //        try
            //        {
            //            if (string.IsNullOrWhiteSpace(txtFilterDirectory.Text))
            //                //myfiles = di.GetFiles("*", chkSelectAll.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            //                myfiles = di.GetFiles("*");
            //            else
            //                //myfiles = di.GetFiles(txtFilterDirectory.Text, chkSelectAll.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            //                myfiles = di.GetFiles(txtFilterDirectory.Text);

            //            if (rdoListDirectory.SelectedValue == "Include Latest")
            //                file = myfiles.OrderByDescending(a => a.LastWriteTime).Select(a => a.FullName).First();
            //            else if (rdoListDirectory.SelectedValue == "Include Oldest")
            //                file = myfiles.OrderBy(a => a.LastWriteTime).Select(a => a.FullName).First();
            //        }
            //        catch (UnauthorizedAccessException) { }

            //        if (file == "")
            //        {
            //            foreach (var s in myfiles)
            //            {
            //                SaveEmailAttachment(s.FullName, et.obj.Id, sourcePath, targetPath, false);
            //            }
            //        }
            //        else
            //        {
            //            SaveEmailAttachment(file, et.obj.Id, sourcePath, targetPath, false);
            //        }
            //    }
            //}

            // save files from Browse files
            // save files in temp folder now
            //commented on 07.11.2015
            //foreach (RepeaterItem row in rptAttachments.Items)
            //{
            //    Literal ltrFileName = (Literal)row.FindControl("ltrFileName");
            //    HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
            //    HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
            //    if (hdnIsDelete.Value == "0")
            //    {
            //        fileName = ltrFileName.Text;
            //        File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, filePrefix + fileName.Replace(filePrefix, "")), true);
            //        SaveEmailAttachment(Path.Combine(targetPath, filePrefix + fileName.Replace(filePrefix, "")), et.obj.Id, "", targetPath);
            //    }
            //}

            //for (int i = 0; i < Request.Files.Count; i++)
            //{
            //    HttpPostedFile objHttpPostedFile = (HttpPostedFile)Request.Files[i];
            //    if (objHttpPostedFile.ContentLength > 0)
            //    {
            //        fileName = Path.GetFileName(objHttpPostedFile.FileName);
            //        objHttpPostedFile.SaveAs(Path.Combine(targetPath, filePrefix + fileName.Replace(filePrefix, "")));
            //        SaveEmailAttachment(Path.Combine(targetPath, filePrefix + fileName.Replace(filePrefix, "")), et.obj.Id, "", targetPath);
            //    }
            //}

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            else
            {
                Array.ForEach(Directory.GetFiles(targetPath), File.Delete);
            }
            //if (Directory.Exists(targetPath))
            //{
            //    Directory.Delete(targetPath, true);
            //    System.Threading.Thread.Sleep(500); // Intentionally put this delay so that if code runs fast then it does not create issue with file access like files is already in use.
            //}

            foreach (RepeaterItem row in rptAttachments.Items)
            {
                Literal ltrFileName = (Literal)row.FindControl("ltrFileName");
                HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
                HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                if (hdnIsDelete.Value == "0")
                {
                    fileName = ltrFileName.Text;
                    File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
                    //SaveEmailAttachment(Path.Combine(targetPath, fileName), et.obj.Id, "", targetPath, true);
                }
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile objHttpPostedFile = (HttpPostedFile)Request.Files[i];
                if (objHttpPostedFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(objHttpPostedFile.FileName);
                    objHttpPostedFile.SaveAs(Path.Combine(targetPath, fileName));
                    //SaveEmailAttachment(Path.Combine(targetPath, fileName), et.obj.Id, "", targetPath, true);
                }
            }




            // delete files from user temp folder
            Helper.DeleteFilesFromUserTempDirectory("~/Attachments/Temp/" + user + "/");

            //if (chkLetter.Checked && Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
            //if (Session["TemplateWordEditor"] != null && !string.IsNullOrWhiteSpace(Convert.ToString(Session["TemplateWordEditor"])))
            //{
            //    using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            //    {
            //        tx.Create();
            //        string data = Convert.ToString(Session["TemplateWordEditor"]);
            //        tx.Load(data, TXTextControl.StringStreamType.RichTextFormat);

            //        string fileTemplateName = targetPath + "\\Letter.docx";
            //        tx.Save(fileTemplateName, TXTextControl.StreamType.WordprocessingML);

            //        Session["TemplateWordEditor"] = null;
            //    }
            //}

            Session["dataction"] = "s";
            Response.Redirect("EmailTemplates.aspx");
        }
    }

    private void SaveEmailAttachment(string fileName, int emailtemplateId, string sourcePath, string targetPath, bool isBrowsed)
    {
        // Use static Path methods to extract only the file name from the path.


        fileName = Path.GetFileName(fileName);
        string destFile = Path.Combine(targetPath, fileName);
        if (sourcePath != "")
        {
            string sourceFile = Path.Combine(sourcePath, fileName);
            File.Copy(sourceFile, destFile, true);
        }
        BAL_AMCPE.EmailAttachments ea = new EmailAttachments();
        ea.obj = new DAL_AMCPE.EmailAttachment();
        ea.obj.Name = fileName;
        ea.obj.Url = destFile;
        ea.obj.EmailTemplateId = emailtemplateId;
        ea.obj.CreatedBy = Convert.ToString(Session["UserId"]);
        ea.obj.CreatedOn = DateTime.Now;
        ea.obj.UpdatedBy = "";
        //ea.obj.UpdatedOn = DateTime.Now;
        ea.obj.DeletedBy = "";
        //ea.obj.DeletedOn = DateTime.Now;
        ea.obj.IsDeleted = false;
        ea.obj.IsBrowsed = isBrowsed;
        ea.Save();
    }

    private void ProcessAttachments()
    {
        fileName = "";
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailTemplates.aspx");
    }

    [WebMethod]
    public static string GetColumnsByTableName(string tableName)
    {
        BAL_AMCPE.TagCategory tc = new BAL_AMCPE.TagCategory();
        List<ColumnName> columns = tc.GetTagSQLNamesByTagCategoryId(Convert.ToInt32(tableName));

        string result = "";
        if (columns != null)
            foreach (ColumnName item in columns)
            {
                result += "<li>" + item.name + "</li>";
            }
        return result;
    }


    //public static string GetColumnsByTableName(string tableName)
    //{
    //    BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
    //    List<ColumnName> columns = et.GetColumnNamesByTableName(tableName);
    //    string result = "";
    //    if (columns != null)
    //        foreach (ColumnName item in columns)
    //        {
    //            result += "<li>" + item.name + "</li>";
    //        }
    //    return result;
    //}
}