using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;
using System.Web.Services;
using System.IO;
using System.Data.Entity;
using System.IO;
using System.Linq;

public partial class AddEditDocumentTemplate : System.Web.UI.Page
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

          //  BindData();
          //  BindTagCategories();

            user = Convert.ToString(Session["UserId"]).Replace("amc\\", "");
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]);
                GetDocTemplateById(Id);
            }
            else
            {
               // rptAttachments.DataSource = null;
               // rptAttachments.DataBind();
            }

            hdnid.Value = Id.ToString();
           
        }
        else
        {
            Id = Convert.ToInt32(hdnid.Value);
           
        }

        // Enable Submit button according user permission
        if (Id == 0)
            btnSubmit.Enabled = PermissionSession.UserPermission.CanCreateTemplate;
    }

    

   

    private void GetDocTemplateById(int id)
    {
        DAL_AMCPE.DocumentTemplate et = new DAL_AMCPE.DocumentTemplate();
        BAL_AMCPE.DocumentTemplates et1 = new BAL_AMCPE.DocumentTemplates();
        //BAL_AMCPE.EmailTemplates et = new BAL_AMCPE.EmailTemplates();
        et = et1.GetDocTemplateByID(id);
        txtTemplateName.Text = et.TemplateName;
        txtTemplatePath.Text = et.TemplatePath;
        checkboxactive.Checked = et.IsActive;
        checkboxdelete.Checked = false;

    }

    private void GetAttachmentFiles(string filePath)
    {
       
        string sourcePath = Server.MapPath(txtTemplatePath.Text);
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
               /// hdnAttachURLs.Value += file.FullName + "|";
            }
           
             //   rptAttachments.DataSource = null;
            //rptAttachments.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            DAL_AMCPE.DocumentTemplate et = new DAL_AMCPE.DocumentTemplate();

            if (Id == 0)
            {
                //et.obj = new DAL_AMCPE.DocumentTemplate();
                //et.obj.TemplatePath = Convert.ToString(Session["UserId"]);
                //et.obj.CreatedOn = DateTime.Now;
            }
            else
            {
                //et.obj = et.GetTemplateByID(Id);
                //et.obj.UpdatedBy = Convert.ToString(Session["UserId"]);
                //et.obj.UpdatedOn = DateTime.Now;

               // ProcessAttachments();
            }

            et.TemplateName = txtTemplateName.Text.Trim();
            et.TemplatePath = txtTemplatePath.Text.Trim();
            et.Id = Id;


            //et.obj.DeletedOn = DateTime.Now;
            et.IsDeleted = checkboxdelete.Checked;
            et.IsActive = checkboxactive.Checked;
            BAL_AMCPE.DocumentTemplates et1 = new BAL_AMCPE.DocumentTemplates();
            responseCode = et1.Save(et);
          

            if (responseCode == -1)
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Record could not be saved, it exists with same name already')", true);
            else if (responseCode == 0)
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "", "alert('Some error occurred, please try again')", true);
            else
            {
                string sourcePath, fileName;
                targetPath = Server.MapPath("~/Attachments/Docmnt/" + et.Id.ToString());

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

                //foreach (RepeaterItem row in rptAttachments.Items)
                //{
                //    Literal ltrFileName = (Literal)row.FindControl("ltrFileName");
                //    HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
                //    HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
                //    if (hdnIsDelete.Value == "0")
                //    {
                //        fileName = ltrFileName.Text;
                //        File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
                //        //SaveEmailAttachment(Path.Combine(targetPath, fileName), et.obj.Id, "", targetPath, true);
                //    }
                //}

                //for (int i = 0; i < Request.Files.Count; i++)
                //{
                //    HttpPostedFile objHttpPostedFile = (HttpPostedFile)Request.Files[i];
                //    if (objHttpPostedFile.ContentLength > 0)
                //    {
                //        fileName = Path.GetFileName(objHttpPostedFile.FileName);
                //        objHttpPostedFile.SaveAs(Path.Combine(targetPath, fileName));
                //        //SaveEmailAttachment(Path.Combine(targetPath, fileName), et.obj.Id, "", targetPath, true);
                //    }
                //}




                // delete files from user temp folder
               // Helper.DeleteFilesFromUserTempDirectory("~/Attachments/Docmnt/" + user + "/");

               // Session["dataction"] = "s";
                Response.Redirect("DocumentTemplates.aspx");
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.InnerException+ex.Message+ex.StackTrace+ex.Source);
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
        string ud = Server.MapPath("~/Attachments/Docmnt/" + user + "/");
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
        //foreach (RepeaterItem row in rptAttachments.Items)
        //{
        //    Literal ltrFileName = (Literal)row.FindControl("ltrFileName");
        //    HiddenField ltrFileUrl = (HiddenField)row.FindControl("hdnFileUrl");
        //    HiddenField hdnIsDelete = (HiddenField)row.FindControl("hdnIsDelete");
        //    if (hdnIsDelete.Value == "0")
        //    {
        //        fileName = ltrFileName.Text;
        //        File.Copy(Path.Combine(ltrFileUrl.Value, fileName), Path.Combine(targetPath, fileName), true);
        //    }
        //}

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
        GetAttachmentFiles("~/Attachments/Docmnt/" + user + "/");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DocumentTemplates.aspx");
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