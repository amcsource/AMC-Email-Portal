
using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;

using System;
using System.Linq;
using System.Text;
using GemBox.Document;
using GemBox.Document.Tables;
using System.Text.RegularExpressions;
using System.Diagnostics;


using System.IO;
using Code7248.word_reader;
using System.Data;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices.ComTypes;

using System.Runtime.InteropServices;
using DAL_AMCPE;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using PdfToText;




namespace BAL_AMCPE
{
    public class DocumentTemplates
    {
        private Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
        private Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document(@"C:\Users\admin\AppData\Roaming\Skype\My Skype Received Files\Pharmacy Work Sheet Template Prescription1(2) - Copy - Copy.doc");
        [DllImport("Kernel32.dll")]
        static extern IntPtr CreateFile(string filename,
        [MarshalAs(UnmanagedType.U4)]FileAccess fileaccess,
        [MarshalAs(UnmanagedType.U4)]FileShare fileshare, int securityattributes,
        [MarshalAs(UnmanagedType.U4)]FileMode creationdisposition, int flags,
        IntPtr template);
        /// <summary>
        /// ///////////////////
        /// </summary>
        public DocumentTemplate obj;
        public int TotalPages { get; set; }

        public void cc(string documentLocation)
        {
            var app1 = new Microsoft.Office.Interop.Word.Application();
            var doc1 = app.Documents.Open(documentLocation);

            string rangeText = doc1.Range().Text;

            doc1.Save();
            doc1.Close();

            Marshal.ReleaseComObject(doc);
            Marshal.ReleaseComObject(app);
        }
        public List<DocumentTemplate> GetEmailTemplates()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.IsDeleted == false).ToList();
            }
        }
        public DAL_AMCPE.DocumentTemplate GetDocTemplateByID(int id)
        {
            id = Convert.ToInt16(id);
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.Id == id).SingleOrDefault();
            }
        }
        public string GetDocTemplatePathByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.Id == id).Select(c => c.TemplatePath).SingleOrDefault().ToString();
            }
        }
        public List<xsoPrescription> GetTemplateReplaceKw(string prescription_parent)
        {

            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                if (prescription_parent == "0")
                { return DB.xsoPrescriptions.Take(10).ToList(); }
                else
                { return DB.xsoPrescriptions.Take(10).Where(c => c.ParentLink_Category == prescription_parent).ToList(); }


            }
        }

        public List<PatientsBulkEmail1> GetPatientsForBulkPrint(string patientType, int pageIndex, int pageSize, string sortExpression, string searchKeyword)
        {
          
            using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
            {
                return DB.GetPatientsForBulkPrint(patientType, pageIndex, pageSize, sortExpression, searchKeyword).Select(a => new PatientsBulkEmail1()
                {
                    PatientNumber = a.PatientNumber,
                    PatientName = a.PatientName,
                    PatientRecId = a.PatientRecId,
                    Source = a.Source,
                    Stage = a.Stage,
                    Email = a.Email,
                    Phone = a.Phone
                }).ToList();

            }
        }

        public List<DocumentTemplate> GetDocTemsplateById(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                if (id == 0)
                    return DB.DocumentTemplates.Where(a => a.IsDeleted == false).OrderBy(a => a.TemplateName).ToList();
                else
                    return DB.DocumentTemplates.Where(a => a.IsDeleted == false && a.Id == id).OrderBy(a => a.TemplateName).ToList();
            }
        }
        public List<DAL_AMCPE.DocumentTemplate> GetAllDocTemplates()
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.IsDeleted == false).OrderBy(a => a.TemplateName).ToList();
            }
        }
        public List<DocumentTemplate> GetDocumentTemplatesList(int pageIndex, int pageSize, string sortExpression, string searchKeyword, int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                if (searchKeyword == "")
                { return DB.DocumentTemplates.ToList(); }
                else { return DB.DocumentTemplates.Where(d => d.TemplateName == searchKeyword).ToList(); }

            }
        }

        public List<procGetEmailTemplates_Result> GetEmailTemplates(int pageIndex, int pageSize, string sortExpression, string searchKeyword)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.procGetEmailTemplates(pageIndex, pageSize, sortExpression, searchKeyword).ToList();
            }
        }

        public DocumentTemplate GetDocumentTemplateByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.IsDeleted == false && a.Id == id).SingleOrDefault();
            }
        }

        public DocumentTemplate GetTemplateByName(string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return DB.DocumentTemplates.Where(a => a.IsDeleted == false && a.TemplateName == name).SingleOrDefault();
            }
        }

        public EmailData GetTemplateDetailByID(int id)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                return (from a in DB.EmailTemplates
                        where a.IsDeleted == false && a.Id == id
                        select new EmailData()
                        {
                            Id = a.Id,
                            From = a.From,
                            To = a.To,
                            Cc = a.Cc,
                            Bcc = a.Bcc,
                            Subject = a.Subject,
                            Body = a.TemplateId != null ? (a.MasterTemplate.Header + a.Body + a.MasterTemplate.Footer) : a.Body,
                            PromptForAttachments = a.PromptForAttachments,
                            AttachmentHasBusiness = a.AttachmentHasBusiness,
                            AttachmentBusinessFilter = a.AttachmentBusinessFilter,
                            AttachmentBusinessInclude = a.AttachmentBusinessInclude,
                            SelectAllAttachments = a.SelectAllAttachments,
                            AttachmentHasDirectory = a.AttachmentHasDirectory,
                            AttachmentDirectoryPath = a.AttachmentDirectoryPath,
                            AttachmentDirectoryFilter = a.AttachmentDirectoryFilter,
                            AttachmentDirectoryInclude = a.AttachmentDirectoryInclude
                        }).SingleOrDefault();
            }
        }


        public int Save(DAL_AMCPE.DocumentTemplate obj)
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    if (DoesAleardyExist(obj.Id, obj.TemplateName))
                        return -1;
                    else
                    {
                        if (obj.Id == 0)
                        {
                            DB.DocumentTemplates.AddObject(obj);
                        }
                        else
                        {
                            DB.DocumentTemplates.Attach(obj);
                            DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                        }
                        DB.SaveChanges();
                        return obj.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public int AddTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.DocumentTemplates.AddObject(obj);
                    DB.SaveChanges();
                    return obj.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateTemplate(DAL_AMCPE.DocumentTemplate ob)
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.DocumentTemplates.Attach(ob);
                    DB.ObjectStateManager.ChangeObjectState(ob, System.Data.EntityState.Deleted);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteTemplate()
        {
            try
            {
                using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
                {
                    DB.DocumentTemplates.DeleteObject(obj);
                    DB.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Deleted);
                    DB.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ColumnName> GetColumnNamesByTableName(string tableName)
        {
            try
            {
                using (GMEEDevelopmentEntities DB = new GMEEDevelopmentEntities())
                {
                    return (from l in DB.procGetColumnNamesByTableName(tableName)
                            select new ColumnName
                            {
                                // name = l.name.Trim()
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool DoesAleardyExist(int id, string name)
        {
            using (AMCPatientEmailEntities DB = new AMCPatientEmailEntities())
            {
                DAL_AMCPE.DocumentTemplate data;
                if (id == 0)
                {
                    data = (from a in DB.DocumentTemplates
                            where a.TemplateName == name && a.IsDeleted == false
                            select a).SingleOrDefault();
                }
                else
                {
                    data = (from a in DB.DocumentTemplates
                            where a.TemplateName == name && a.Id != id && a.IsDeleted == false
                            select a).SingleOrDefault();
                }

                if (data != null)
                    return true;
                else
                    return false;

            }
        }
        public string copy_clone(string sourceFile, string targetPath, string fileName)
        {

           

            // Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            //if (System.IO.Directory.Exists(sourcePath))
            //{
            //    string[] files = System.IO.Directory.GetFiles(sourcePath);

            //    // Copy the files and overwrite destination files if they already exist.
            //    foreach (string s in files)
            //    {
            //        // Use static Path methods to extract only the file name from the path.
            //        fileName = System.IO.Path.GetFileName(s);
            //        destFile = System.IO.Path.Combine(targetPath, fileName);
            //        System.IO.File.Copy(s, destFile, true);
            //    }
            //}
            //else
            //{

            //}

            // Keep console window open in debug mode.
            return destFile;
        }
      public static string GetText(string strfilename)
    {
        string strRetval = "";
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
        if (File.Exists(strfilename))
        {
            try
            {
                using (StreamReader sr = File.OpenText(strfilename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        strBuilder.AppendLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
              //  SendErrorMail(ex);
            }
            finally
            {
                if (System.IO.File.Exists(strfilename))
                    System.IO.File.Delete(strfilename);
            }
        }

        if (strBuilder.ToString().Trim() != "")
            strRetval = strBuilder.ToString();
        else
            strRetval = "";

        return strRetval;
    }

    public static string SaveAsText()
    {
        string fileName = "";
        object miss = System.Reflection.Missing.Value;
        Microsoft.Office.Interop.Word.Document doc = null;
        try
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            fileName = @"C:\h.txt";
            doc = wordApp.Documents.Open(@"C:\Users\admin\AppData\Roaming\Skype\rt.doc", false);
            doc.SaveAs(fileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDOSText);

        }
        catch (Exception ex)
        {

           // SendErrorMail(ex);
        }
        finally
        {
            if (doc != null)
            {
                doc.Close(ref miss, ref miss, ref miss);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                doc = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        return fileName;
    }
    public static string my_doc_string = "";
    public Dictionary<string,string> ReadDocFileToString(string org_file_full_path, string recId, string patientNumber)
    {
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
        object Missing = System.Type.Missing;
        my_doc_string = "";
        app = new Microsoft.Office.Interop.Word.Application();

        doc = app.Documents.Open(org_file_full_path, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);

        FindReplaceAnywhereDummy(app, "", "");

        return Helper.ProcessDataCopy(my_doc_string, recId, patientNumber);
         
    }
        
        public string ReadMsWord(string org_file_full_path, string dest_folder, string dest_file,Dictionary<string,string> d)
        {

         
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            object Missing = System.Type.Missing;
       
           


            string path = copy_clone(org_file_full_path, dest_folder, dest_file);

        
           
           
           
            

            try
            {
               
                app = new Microsoft.Office.Interop.Word.Application();

                doc = app.Documents.Open(path, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
            



                foreach (KeyValuePair<string, string> pair in d)
                {
                    FindReplaceAnywhere(app, pair.Key, pair.Value);
                }


                // FindReplaceAnywhere(app, find_text, replace_text);
                
              



            }
            finally
            {
                try
                {
                    if (doc != null) ((microsoft.office.interop.word._document)doc).close(true, missing, missing);
                }
                finally { }
                if (app != null) ((microsoft.office.interop.word._application)app).quit(true, missing, missing);
            }
         
            return path;


        }
        public static void FindReplaceAnywhere(Microsoft.Office.Interop.Word.Application app, string findText, string replaceText)
        {
            // http://forums.asp.net/p/1501791/3739871.aspx
            var doc = app.ActiveDocument;

            // Fix the skipped blank Header/Footer problem
            //    http://msdn.microsoft.com/en-us/library/aa211923(office.11).aspx
            Microsoft.Office.Interop.Word.WdStoryType lngJunk = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;

            // Iterate through all story types in the current document
            foreach (Microsoft.Office.Interop.Word.Range rngStory in doc.StoryRanges)
            {

                // Iterate through all linked stories
                var internalRangeStory = rngStory;

                do
                {
                    searchAndReplaceInStory(internalRangeStory, findText, replaceText);

                    try
                    {
                        //   6 , 7 , 8 , 9 , 10 , 11 -- http://msdn.microsoft.com/en-us/library/aa211923(office.11).aspx
                        switch (internalRangeStory.StoryType)
                        {
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesHeaderStory: // 6
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryHeaderStory:   // 7
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesFooterStory: // 8
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryFooterStory:   // 9
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageHeaderStory: // 10
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageFooterStory: // 11

                                if (internalRangeStory.ShapeRange.Count > 0)
                                {
                                    foreach (Microsoft.Office.Interop.Word.Shape oShp in internalRangeStory.ShapeRange)
                                    {
                                        if (oShp.TextFrame.HasText != 0)
                                        {
                                            searchAndReplaceInStory(oShp.TextFrame.TextRange, findText, replaceText);
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch
                    {
                        // On Error Resume Next
                    }

                    // ON ERROR GOTO 0 -- http://www.harding.edu/fmccown/vbnet_csharp_comparison.html

                    // Get next linked story (if any)
                    internalRangeStory = internalRangeStory.NextStoryRange;
                } while (internalRangeStory != null); // http://www.harding.edu/fmccown/vbnet_csharp_comparison.html
            }

        }
      public static  string hh = string.Empty;
        private static void searchAndReplaceInStory(Microsoft.Office.Interop.Word.Range rngStory, string strSearch, string strReplace)
        {
            object Missing = System.Type.Missing;
            rngStory.Find.ClearFormatting();
            rngStory.Find.Replacement.ClearFormatting();
            rngStory.Find.Text = strSearch;
            rngStory.Find.Replacement.Text = strReplace;
            rngStory.Find.Wrap = WdFindWrap.wdFindContinue;
            hh =hh+ rngStory.Text;
            object arg1 = Missing; // Find Pattern
            object arg2 = Missing; //MatchCase
            object arg3 = Missing; //MatchWholeWord
            object arg4 = Missing; //MatchWildcards
            object arg5 = Missing; //MatchSoundsLike
            object arg6 = Missing; //MatchAllWordForms
            object arg7 = Missing; //Forward
            object arg8 = Missing; //Wrap
            object arg9 = Missing; //Format
            object arg10 = Missing; //ReplaceWith
            object arg11 = WdReplace.wdReplaceAll; //Replace
            object arg12 = Missing; //MatchKashida
            object arg13 = Missing; //MatchDiacritics
            object arg14 = Missing; //MatchAlefHamza
            object arg15 = Missing; //MatchControl

            rngStory.Find.Execute(ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8, ref arg9, ref arg10, ref arg11, ref arg12, ref arg13, ref arg14, ref arg15);
        }
  
        private static void searchAndReplaceInStoryDummy(Microsoft.Office.Interop.Word.Range rngStory, string strSearch, string strReplace)
        {
            my_doc_string =my_doc_string+ rngStory.Text;
                  }
        public static void FindReplaceAnywhereDummy(Microsoft.Office.Interop.Word.Application app, string findText, string replaceText)
        {
            // http://forums.asp.net/p/1501791/3739871.aspx
            var doc = app.ActiveDocument;

            // Fix the skipped blank Header/Footer problem
            //    http://msdn.microsoft.com/en-us/library/aa211923(office.11).aspx
            Microsoft.Office.Interop.Word.WdStoryType lngJunk = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;

            // Iterate through all story types in the current document
            foreach (Microsoft.Office.Interop.Word.Range rngStory in doc.StoryRanges)
            {

                // Iterate through all linked stories
                var internalRangeStory = rngStory;

                do
                {
                    searchAndReplaceInStoryDummy(internalRangeStory, findText, replaceText);

                    try
                    {
                        //   6 , 7 , 8 , 9 , 10 , 11 -- http://msdn.microsoft.com/en-us/library/aa211923(office.11).aspx
                        switch (internalRangeStory.StoryType)
                        {
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesHeaderStory: // 6
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryHeaderStory:   // 7
                            case Microsoft.Office.Interop.Word.WdStoryType.wdEvenPagesFooterStory: // 8
                            case Microsoft.Office.Interop.Word.WdStoryType.wdPrimaryFooterStory:   // 9
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageHeaderStory: // 10
                            case Microsoft.Office.Interop.Word.WdStoryType.wdFirstPageFooterStory: // 11

                                if (internalRangeStory.ShapeRange.Count > 0)
                                {
                                    foreach (Microsoft.Office.Interop.Word.Shape oShp in internalRangeStory.ShapeRange)
                                    {
                                        if (oShp.TextFrame.HasText != 0)
                                        {
                                            searchAndReplaceInStoryDummy(oShp.TextFrame.TextRange, findText, replaceText);
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    catch
                    {
                        // On Error Resume Next
                    }

                    // ON ERROR GOTO 0 -- http://www.harding.edu/fmccown/vbnet_csharp_comparison.html

                    // Get next linked story (if any)
                    internalRangeStory = internalRangeStory.NextStoryRange;
                } while (internalRangeStory != null); // http://www.harding.edu/fmccown/vbnet_csharp_comparison.html
            }

        }
   
    }
    public class DocData
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplatePath { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

    }

    public class PatientsBulkEmail1
    {
        public string PatientNumber { get; set; }
        public string PatientName { get; set; }
        public string PatientRecId { get; set; }
        public string Source { get; set; }
        public string Stage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalPages { get; set; }
    }
}
