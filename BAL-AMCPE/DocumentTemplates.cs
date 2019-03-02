using System;
using System.Collections.Generic;

using System.Linq;

using GemBox.Document;
using System.IO;
using Microsoft.Office.Interop.Word;

using System.Runtime.InteropServices;
using DAL_AMCPE;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;


namespace BAL_AMCPE
{
    public class DocumentTemplates
    {
        private Microsoft.Office.Interop.Word.Application app; //= new Microsoft.Office.Interop.Word.Application();
        private Microsoft.Office.Interop.Word.Document doc; // = new Microsoft.Office.Interop.Word.Document();
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
                return DB.GetPatientsForBulkPrint1(patientType, pageIndex, pageSize, sortExpression, searchKeyword).Select(a => new PatientsBulkEmail1()
                {
                    PatientNumber = a.PatientNumber,
                    PatientName = a.PatientName,
                    PatientRecId = a.PatientRecId,
                    Source = a.Source,
                    Stage = a.Stage,
                    Email = a.Email,
                    Phone = a.Phone,
                    XSORecID = a.XSORecID,
                    PresType = a.PresType
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
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            System.IO.File.Copy(sourceFile, destFile, true);
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
        public string path = "", path1 = "";
        public static string hh = string.Empty;
        //public string regExpression = @"(?<=\[)(.*?)(?=\])"; // return without brackets
        public string regExpression = @"(?<=\[)f:(\s*)(\d+)"; // return without brackets

        public List<string> doc_tags = new List<string>();

        //Step1
        public Dictionary<string, string> ReadDocFileToString(string org_file_full_path, string recId, string patientNumber, string dest_folder, string dest_file, string presType = "")
        {
            object Missing = System.Type.Missing;
            try
            {

                ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
                my_doc_string = "";
                path = copy_clone(org_file_full_path, dest_folder, "my" + dest_file);

                app = new Microsoft.Office.Interop.Word.Application();

                doc = app.Documents.Open(path, Missing, false, Missing, Missing, Missing, false, Missing, Missing, Missing);

                FindReplaceAnywhereDummy(app, "", "");
            }
            finally
            {
                try
                {
                    if (doc != null) ((Microsoft.Office.Interop.Word._Document)doc).Close(true, Missing, Missing);
                }
                finally { }
                if (app != null) ((Microsoft.Office.Interop.Word._Application)app).Quit(true, Missing, Missing);


            }
            return Helper.ProcessData_V2(doc_tags, recId, patientNumber, presType);

        }

        public string ReadDocFileToString_V2(string org_file_full_path, string recId, string patientNumber, string dest_folder, string dest_file, string presType = "")
        {
            try
            {
                my_doc_string = "";
                path = copy_clone(org_file_full_path, dest_folder, dest_file);


                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
                {
                    // Insert other code here.
                    //string txt = wordDoc.MainDocumentPart.Document.InnerText;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        my_doc_string = sr.ReadToEnd();
                    }


                    string[] result = Regex.Matches(my_doc_string, regExpression) // return without brackets
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

                    foreach (string s in result)
                    {
                        doc_tags.Add(s);
                    }

                    Dictionary<string, string> my_dict = Helper.ProcessData_V2(doc_tags, recId, patientNumber, presType);
                    string findText = string.Empty;
                    string replaceText = string.Empty;

                    ////replace with both bracket
                    //foreach (KeyValuePair<string, string> pair in my_dict)
                    //{
                    //    //findText = "[" + pair.Key + "]";
                    //    findText = pair.Key;
                    //    replaceText = pair.Value;

                    //    my_doc_string = my_doc_string.Replace("[" + findText + "]", replaceText);
                        
                    //}

                    //replace without right bracket
                    foreach (KeyValuePair<string, string> pair in my_dict)
                    {
                        //findText = "[" + pair.Key + "]";
                        findText = pair.Key;
                        replaceText = pair.Value;

                        my_doc_string = my_doc_string.Replace(findText, replaceText);
                        //my_doc_string = my_doc_string.Replace("[" + findText, replaceText);
                    }
                    //replace remaining brackets
                    my_doc_string = my_doc_string.Replace("[", "").Replace("]", "");


                    //Regex regexText = new Regex("[f: 4]");
                    //my_doc_string = regexText.Replace(my_doc_string, "Rohit!");
                    //my_doc_string = my_doc_string.Replace("[f: 4]", "Rohit");

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(my_doc_string);
                    }
                }

                //FindReplaceAnywhereDummy(app, "", "");
            }
            catch (Exception ex)
            {

            }
            return path;
        }

        //Step 2
        public void FindReplaceAnywhereDummy(Microsoft.Office.Interop.Word.Application app, string findText, string replaceText)
        {
            var doc = app.ActiveDocument;

            Microsoft.Office.Interop.Word.WdStoryType lngJunk = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;

            foreach (Microsoft.Office.Interop.Word.Range rngStory in doc.StoryRanges)
            {
                var internalRangeStory = rngStory;

                do
                {
                    if (internalRangeStory != null && internalRangeStory.Text.Contains("[f:"))
                    {
                        string[] result = Regex.Matches(internalRangeStory.Text, regExpression) // return without brackets
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();

                        foreach (string s in result)
                        {
                            doc_tags.Add(s);
                        }

                        //searchAndReplaceInStoryDummy(internalRangeStory, findText, replaceText);
                    }
                    try
                    {
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
                                            if (oShp.TextFrame.TextRange.Text.Contains("[f:"))
                                            {
                                                string[] result = Regex.Matches(internalRangeStory.Text, regExpression) // return without brackets
                                                               .Cast<Match>()
                                                               .Select(m => m.Value)
                                                               .ToArray();

                                                foreach (string s in result)
                                                {
                                                    doc_tags.Add(s);
                                                }
                                            }
                                            //searchAndReplaceInStoryDummy(oShp.TextFrame.TextRange, findText, replaceText);
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

                    }
                    internalRangeStory = internalRangeStory.NextStoryRange;
                } while (internalRangeStory != null);
            }

        }

        //Step 2.1
        private static void searchAndReplaceInStoryDummy(Microsoft.Office.Interop.Word.Range rngStory, string strSearch, string strReplace)
        {
            my_doc_string = my_doc_string + rngStory.Text;
        }

        //Step 3
        public string ReadMsWord(string org_file_full_path, string dest_folder, string dest_file, Dictionary<string, string> d)
        {


            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            object Missing = System.Type.Missing;

            path1 = copy_clone(org_file_full_path, dest_folder, dest_file);

            try
            {
                //Word.Application wordApp = Word.ApplicationClass();



                app = new Microsoft.Office.Interop.Word.Application();

                doc = app.Documents.Open(path1, Missing, false, Missing, Missing, Missing, false, Missing, Missing, Missing);

                //Vikas
                FindAndReplaceInDoc(app, d);

                //Yogesh
                //foreach (KeyValuePair<string, string> pair in d)
                //{
                //    FindReplaceAnywhere(app, pair.Key, pair.Value);
                //    //FindAndReplaceInDoc(app, pair.Key, pair.Value);
                //}
            }
            finally
            {
                try
                {
                    if (doc != null) ((Microsoft.Office.Interop.Word._Document)doc).Close(true, Missing, Missing);
                }
                finally { }
                if (app != null) ((Microsoft.Office.Interop.Word._Application)app).Quit(true, Missing, Missing);


            }


            return path1;


        }

        //Step 4
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


        //Step 5
        private static void searchAndReplaceInStory(Microsoft.Office.Interop.Word.Range rngStory, string strSearch, string strReplace)
        {
            object Missing = System.Type.Missing;
            rngStory.Find.ClearFormatting();
            rngStory.Find.Replacement.ClearFormatting();
            rngStory.Find.Text = strSearch;
            rngStory.Find.Replacement.Text = strReplace;
            rngStory.Find.Wrap = WdFindWrap.wdFindContinue;
            hh = hh + rngStory.Text;
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

        //Step 6
        public string delete_tempfile()
        {
            if (System.IO.File.Exists(path))
                File.Delete(path);

            if (System.IO.File.Exists(path1))
                File.Delete(path1);

            //File.Delete(path1);


            return my_doc_string;
        }
        public string MergeAllPDF(FileInfo[] allPDFs, string outputFileName)
        {
            var resultDocument = new TallComponents.PDF.Document();

            TallComponents.PDF.Document mergedDocument = new TallComponents.PDF.Document();

            // Keep a list of input streams to close when done.
            List<FileStream> streams = new List<FileStream>();



            foreach (FileInfo fileInfo in allPDFs)
            {
                if (fileInfo != null)
                {
                    // Open a stream and add it to the list of streams
                    var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

                    streams.Add(stream);

                    // Open a document
                    var document = new TallComponents.PDF.Document(stream);

                    // Append all pages to the target document. The target document will now hold 
                    // references to the input document, so its stream should not yet be closed.
                    resultDocument.Pages.AddRange(document.Pages.CloneToArray());
                }
            }

            // Create a file for writing the result.

            using (var output = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                // Write the result. During writing, access to the input files will be needed, 
                // so these should not yet be closed.
                resultDocument.Write(output);
            }

            // Close all input streams.
            streams.ForEach(stream => stream.Close());


            return outputFileName;
        }


        //23.06.2016

        private void FindAndReplaceInDoc(Microsoft.Office.Interop.Word.Application app, Dictionary<string, string> d)
        {
            ////Method 1
            //object matchCase = false;
            //object matchWholeWord = true;
            //object matchWildCards = false;
            //object matchSoundsLike = false;
            //object matchAllwordForms = false;
            //object forward = true;
            //object format = true;
            //object matchKashida = false;
            //object matchDiacritics = false;
            //object matchAlefHamza = false;
            //object matchControl = false;
            //object read_only = false;
            //object visible = true;
            //object replace = 2;
            //object wrap = 1;

            //object findText = "";
            //object replaceWithText = "";

            //foreach (KeyValuePair<string, string> pair in d)
            //{
            //    findText = pair.Key;
            //    replaceWithText = pair.Value;

            //    app.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord,
            //    ref matchWildCards, ref matchSoundsLike,
            //    ref matchAllwordForms, ref forward,
            //    ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida, ref matchDiacritics, ref matchAlefHamza,
            //    ref matchControl);
            //}


            ////Method 2
            // http://forums.asp.net/p/1501791/3739871.aspx
            var doc = app.ActiveDocument;

            // Fix the skipped blank Header/Footer problem
            //    http://msdn.microsoft.com/en-us/library/aa211923(office.11).aspx
            Microsoft.Office.Interop.Word.WdStoryType lngJunk = doc.Sections[1].Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;

            int storyRanges = doc.StoryRanges.Count;

            Dictionary<string, string> my_dict = new Dictionary<string, string>();

            foreach (Microsoft.Office.Interop.Word.Range rngStory in doc.StoryRanges)
            {

                // Iterate through all linked stories
                var internalRangeStory = rngStory;

                do
                {

                    if (internalRangeStory != null && internalRangeStory.Text.Contains("[f:"))
                    {
                        string[] result = Regex.Matches(internalRangeStory.Text, regExpression) // return without brackets
                                   .Cast<Match>()
                                   .Select(m => m.Value)
                                   .ToArray();


                        my_dict.Clear();
                        foreach (string s in result)
                        {
                            string queryResult = "";
                            if (d.ContainsKey("[" + s + "]"))
                            {
                                queryResult = d["[" + s + "]"];
                                if (!my_dict.ContainsKey("[" + s + "]"))
                                {
                                    my_dict.Add("[" + s + "]", queryResult);
                                }
                            }
                        }

                        searchAndReplaceInStory_V2(internalRangeStory, my_dict);
                    }

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
                                    int shapeRange = internalRangeStory.ShapeRange.Count;

                                    foreach (Microsoft.Office.Interop.Word.Shape oShp in internalRangeStory.ShapeRange)
                                    {
                                        if (oShp.TextFrame.HasText != 0)
                                        {
                                            if (oShp.TextFrame.TextRange.Text.Contains("[f:"))
                                            {

                                                string[] result = Regex.Matches(internalRangeStory.Text, regExpression) // return without brackets
                                                                       .Cast<Match>()
                                                                       .Select(m => m.Value)
                                                                       .ToArray();


                                                my_dict.Clear();
                                                foreach (string s in result)
                                                {
                                                    string queryResult = "";
                                                    if (d.ContainsKey("[" + s + "]"))
                                                    {
                                                        queryResult = d["[" + s + "]"];
                                                        if (!my_dict.ContainsKey("[" + s + "]"))
                                                        {
                                                            my_dict.Add("[" + s + "]", queryResult);
                                                        }

                                                    }
                                                }

                                                searchAndReplaceInStory_V2(oShp.TextFrame.TextRange, my_dict);

                                                //searchAndReplaceInStory_V2(oShp.TextFrame.TextRange, d);
                                            }
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

        private static void searchAndReplaceInStory_V2(Microsoft.Office.Interop.Word.Range rngStory, Dictionary<string, string> d)
        {
            object Missing = System.Type.Missing;
            rngStory.Find.ClearFormatting();
            rngStory.Find.Replacement.ClearFormatting();
            rngStory.Find.Wrap = WdFindWrap.wdFindContinue;
            hh = hh + rngStory.Text;
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

            foreach (KeyValuePair<string, string> pair in d)
            {
                rngStory.Find.Text = pair.Key;
                rngStory.Find.Replacement.Text = pair.Value;

                rngStory.Find.Execute(ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8, ref arg9, ref arg10, ref arg11, ref arg12, ref arg13, ref arg14, ref arg15);
            }
        }

        private void FindAndReplaceInDoc(Microsoft.Office.Interop.Word.Application app, object findText, object replaceWithText)
        {

            object Missing = System.Type.Missing;
            object arg1 = findText; // Find Pattern
            object arg2 = Missing; //MatchCase
            object arg3 = Missing; //MatchWholeWord
            object arg4 = Missing; //MatchWildcards
            object arg5 = Missing; //MatchSoundsLike
            object arg6 = Missing; //MatchAllWordForms
            object arg7 = Missing; //Forward
            object arg8 = Missing; //Wrap
            object arg9 = Missing; //Format
            object arg10 = replaceWithText; //ReplaceWith
            object arg11 = WdReplace.wdReplaceAll; //Replace
            object arg12 = Missing; //MatchKashida
            object arg13 = Missing; //MatchDiacritics
            object arg14 = Missing; //MatchAlefHamza
            object arg15 = Missing; //MatchControl

            app.Selection.Find.Execute(ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8, ref arg9, ref arg10, ref arg11, ref arg12, ref arg13, ref arg14, ref arg15);


            //object matchCase = false;
            //object matchWholeWord = true;
            //object matchWildCards = true;
            //object matchSoundsLike = false;
            //object matchAllwordForms = false;
            //object forward = true;
            //object format = true;
            //object matchKashida = false;
            //object matchDiacritics = false;
            //object matchAlefHamza = false;
            //object matchControl = false;
            //object read_only = false;
            //object visible = true;
            //object replace = 2;
            //object wrap = 1;

            //app.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord, 
            //    ref matchWildCards, ref matchSoundsLike, 
            //    ref matchAllwordForms, ref forward, 
            //    ref wrap, ref format, ref replaceWithText, ref replace, ref matchKashida, ref matchDiacritics, ref matchAlefHamza,
            //    ref matchControl);
        }

        //public void ReplaceTextInWordDoc()
        //{
        //    Microsoft.Office.Interop.Word.Application wordNew = new Microsoft.Office.Interop.Word.Application();
        //    Microsoft.Office.Interop.Word.Document docNew = new Microsoft.Office.Interop.Word.Document();

        //    object missing = System.Type.Missing;

        //    try
        //    {
        //        object fileName = @"C:\TT\change.doc";
        //        docNew = wordNew.Documents.Open(ref fileName,
        //            ref missing, ref missing, ref missing, ref missing,
        //            ref missing, ref missing, ref missing, ref missing,
        //            ref missing, ref missing, ref missing, ref missing,
        //            ref missing, ref missing, ref missing);

        //        docNew.Activate();

        //        foreach (Microsoft.Office.Interop.Word.Range tmpRange in docNew.StoryRanges)
        //        {

        //            tmpRange.Find.Text = "<DATE>";
        //            tmpRange.Find.Replacement.Text = DateTime.Now.ToString("MM/dd/yyyy");
        //            tmpRange.Find.Replacement.ParagraphFormat.Alignment =
        //                Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;



        //            tmpRange.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
        //            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

        //            tmpRange.Find.Execute(ref missing, ref missing, ref missing,
        //                ref missing, ref missing, ref missing, ref missing,
        //                ref missing, ref missing, ref missing, ref replaceAll,
        //                ref missing, ref missing, ref missing, ref missing);
        //        }

        //        docNew.Save();

        //        docNew.Close(ref missing, ref missing, ref missing);
        //        wordNew.Application.Quit(ref missing, ref missing, ref missing);
        //    }
        //    catch (Exception ex)
        //    {
        //        docNew.Close(ref missing, ref missing, ref missing);
        //        wordNew.Application.Quit(ref missing, ref missing, ref missing);
        //    }
        //}
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
        public string XSORecID { get; set; }
        public string PresType { get; set; }
    }
}
