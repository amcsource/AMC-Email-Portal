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


public partial class Emails_AutoProcessPrintJob : System.Web.UI.Page
{
    public int TemplateId;
    public long EmailProcessId;
    public string templateName, templatePath, patientRecId, patientNumber, XSORecID, presType, patientFullName;

    // File Path variables
    string fileName, sourcePath, targetPath;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProcessSheets();
        }
    }

    private void ProcessSheets()
    {
        BAL_AMCPE.PrintJobProcess ej = new BAL_AMCPE.PrintJobProcess();
        List<DAL_AMCPE.PrintJob> jobs = ej.GetPrintJobs();
        if (jobs.Count > 0)
        {
            System.IO.FileInfo[] allPDFs = null;
            List<string> filesToDelete = new List<string>();
            
            BAL_AMCPE.DocumentTemplates et = new BAL_AMCPE.DocumentTemplates();
            
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string targetPath = WebConfigurationManager.AppSettings["Server03PrintDocument"] + date;

            Aspose.Words.License license = new Aspose.Words.License();
            license.SetLicense(Server.MapPath("~/Aspose.Words.lic"));
            Aspose.Words.Document doc;

            var records = jobs.GroupBy(a => a.Filename).ToList();

            string fileName = "";

            foreach (var items in records)
            {
                int filecount = items.Count();
                int count = 0;
                allPDFs = new System.IO.FileInfo[filecount];

                foreach (var job in items)
                {
                    ej.obj = job;
                    
                    fileName = job.Filename;

                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        DAL_AMCPE.DocumentTemplate ed = et.GetDocTemplateByID(job.DocumentTemplateId);

                        TemplateId = ed.Id;
                        templatePath = ed.TemplatePath;
                        string path_pdf = "";

                        patientRecId = job.PatientRecId;
                        patientNumber = job.PatientNumber;
                        
                        FileInfo fileinfo = new FileInfo(templatePath);
                        string filename = fileinfo.Name.Replace(".docx", "").Replace(".doc", "");

                        string id = Guid.NewGuid().ToString().Replace("-", "");
                        string DESTfile = Convert.ToString(job.PatientFullName + "_" + job.DocumentTemplateName) + ".doc";
                        string savePath = WebConfigurationManager.AppSettings["Server03SavePrintDocument"] + id;

                        path_pdf = et.ReadDocFileToString_V2(templatePath, patientRecId, patientNumber, savePath, DESTfile);

                        if (!string.IsNullOrWhiteSpace(path_pdf))
                        {
                            try
                            {
                                string OutputLocation = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                                doc = new Aspose.Words.Document(path_pdf);
                                doc.Save(OutputLocation);

                                et.delete_tempfile();
                                //Session["fileinfo"] = path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf");

                                FileInfo fileidnfo = new FileInfo(OutputLocation);
                                allPDFs[count] = fileidnfo;

                                //Update record in PharmacyWorksheet table
                                ej.obj.Process = false;
                                ej.obj.ProcessedOn = DateTime.Now;
                                ej.obj.IsProcessed = true;

                                ej.Save();

                                count++;

                                if (!job.SaveInCRM)
                                {
                                    filesToDelete.Add(path_pdf.Replace(".docx", ".pdf").Replace(".doc", ".pdf"));
                                }
                            }
                            catch (Exception ex)
                            {
                                //Update record in PharmacyWorksheet table
                                ej.obj.Process = true;
                                ej.obj.IsProcessed = false;
                                ej.obj.ProcessFailed = true;
                                ej.obj.ProcessFailedCount = Convert.ToInt16(Convert.ToInt16(ej.obj.ProcessFailedCount) + 1);
                                ej.obj.ProcessFailedReason = ex.Message + "\n" + ex.InnerException.Message.ToString();

                                ej.Save();

                                //if(job.SaveInCRM)
                                //{
                                //    //commented in case of testing
                                //    //TODO: rasie task
                                //    PatientActivity pa = new PatientActivity();
                                //    pa.objActivity = new DAL_AMCPE.Activity();
                                //    //pa.objActivity.RecId = job.ActivityRecId;
                                //    pa.objActivity.ActivityResult = "Task - EXCEPTIONS";
                                //    pa.objActivity.Notes = "Worksheet failed to process - Please alert IT and action manually";
                                //    pa.objActivity.Subject = "URGENT - WORKSHEET HAS NOT BEEN PROCESSED";
                                //    pa.objActivity.Purpose = "URGENT - WORKSHEET NOT SENT TO PHARMACY";
                                //    pa.objActivity.StartDateTime = DateTime.Now;
                                //    pa.objActivity.ActivityType = "Task";
                                //    pa.objActivity.xfPrescriptionType = "";
                                //    pa.objActivity.LastModBy = "SYSTEM";
                                //    pa.Save(0);
                                //}
                            }
                        }
                    }
                    else
                    {
                        //Update record in PharmacyWorksheet table
                        ej.obj.Process = true;
                        ej.obj.IsProcessed = false;
                        ej.obj.ProcessFailed = true;
                        ej.obj.ProcessFailedCount = Convert.ToInt16(Convert.ToInt16(ej.obj.ProcessFailedCount) + 1);
                        ej.obj.ProcessFailedReason = "File could not be processed as file name not found";

                        ej.Save();

                        //if (job.SaveInCRM)
                        //{
                        //    //commented in case of testing
                        //    //TODO: rasie task
                        //    PatientActivity pa = new PatientActivity();
                        //    pa.objActivity = new DAL_AMCPE.Activity();
                        //    //pa.objActivity.RecId = job.ActivityRecId;
                        //    pa.objActivity.ActivityResult = "Task - EXCEPTIONS";
                        //    pa.objActivity.Notes = "Worksheet failed to process - Please alert IT and action manually";
                        //    pa.objActivity.Subject = "URGENT - WORKSHEET HAS NOT BEEN PROCESSED [Filename not found]";
                        //    pa.objActivity.Purpose = "URGENT - WORKSHEET NOT SENT TO PHARMACY";
                        //    pa.objActivity.StartDateTime = DateTime.Now;
                        //    pa.objActivity.ActivityType = "Task";
                        //    pa.objActivity.xfPrescriptionType = "";
                        //    pa.objActivity.LastModBy = "SYSTEM";
                        //    pa.Save(0);
                        //}
                    }
                }

                //Save allPDFs at current date folder
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                //et.MergeAllPDF(allPDFs, targetPath + "\\" + fileName + "-" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".pdf");
                et.MergeAllPDF(allPDFs, targetPath + "\\" + fileName + ".pdf");
            }

            
            //TODO: Delete individual files from filesToDelete
            try
            {
                if (filesToDelete.Count() > 0)
                {
                    for (int i = 0; i < filesToDelete.Count(); i++)
                    {
                        if (File.Exists(filesToDelete[i]))
                        {
                            File.Delete(filesToDelete[i]);
                        }
                    }

                }
            }
            catch (Exception ex) { }
        }
    }
}