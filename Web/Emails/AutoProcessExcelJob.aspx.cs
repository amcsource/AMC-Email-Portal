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
using System.Xml;
using DAL_AMCPE;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

public partial class Emails_AutoProcessExcelJob : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProcessSheets();
        }
    }

    private void ProcessSheets()
    {
        BAL_AMCPE.ExcelJobProcess ej = new BAL_AMCPE.ExcelJobProcess();
        List<DAL_AMCPE.ExcelJob> jobs = ej.GetExcelJobs();
        if (jobs.Count > 0)
        {
            string fileName = "";
            DataTable dtRecords;
            string[] attachment = new string[1];

            foreach (var job in jobs)
            {
                ej.obj = job;

                if (!string.IsNullOrWhiteSpace(job.Filename))
                    fileName = job.Filename + "_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss");
                else
                    fileName = "AMC_Excel_Report" + "_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss");

                dtRecords = BAL_AMCPE.Utils.DbUtility.GetDataTable(job.ProcName, new BAL_AMCPE.Connections.SqlCon(), null, CommandType.StoredProcedure, false);
                if (dtRecords.Rows.Count > 0)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        try
                        {
                            //Create the worksheet 
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                            int column = 1;

                            //set columns
                            foreach (DataColumn col in dtRecords.Columns)
                            {
                                ws.Cells[1, column++].Value = col.ColumnName;
                                //ws.Column(column).Width = 25;
                                //ws.Column(column).Style.WrapText = true;
                            }

                            using (ExcelRange rng = ws.Cells[1, 1, 1, (column - 1)])
                            {
                                rng.Style.Font.Bold = true;
                                //rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid
                                //rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(69, 27, 123));  //Set color to dark Maroon
                                //rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            }

                            //set rows
                            for (int i = 1; i <= dtRecords.Rows.Count; i++)
                            {
                                for (column = 1; column <= dtRecords.Columns.Count; column++)
                                {
                                    ws.Cells[i + 1, column].Value = Convert.ToString(dtRecords.Rows[i - 1][column - 1]); //converted explicitely to string so that it can be left alighend
                                    ws.Cells[i + 1, column].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                }
                            }

                            ws.View.FreezePanes(2, 1); //freezes first row

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();


                            string excelName = fileName + ".xlsx";

                            var sitePath = System.Web.HttpContext.Current.Server.MapPath("~/TempFiles/");
                            System.IO.FileStream file = System.IO.File.Create(sitePath + excelName);
                            byte[] dataExcel = pck.GetAsByteArray();
                            file.Write(dataExcel, 0, dataExcel.Length);
                            file.Close();

                            //Now send mail with attachments(if any)
                            FlexiMail ml = new FlexiMail();
                            ml.FromName = "AMC";
                            ml.From = "info@menopausecentre.com.au"; //from; // TODO: handle name other than email id also like AMC info@amc.com
                            ml.To = job.EmailId;
                            ml.CC = "";
                            ml.BCC = "";
                            ml.Subject = job.EmailSubject;
                            ml.MailBody = job.EmailBody;
                            ml.MailBodyManualSupply = true;
                            ml.IsBodyHtml = true;

                            attachment[0] = sitePath + excelName;
                            ml.AttachFile = attachment;

                            ml.Send();

                            //Update record in ExcelJob table
                            ej.obj.Process = false;
                            ej.obj.ProcessedOn = DateTime.Now;
                            ej.obj.IsProcessed = true;
                            ej.Save();
                        }
                        catch (Exception ex)
                        {
                            //Update record in ExcelJob table
                            ej.obj.Process = true;
                            ej.obj.ProcessFailed = true;
                            ej.obj.ProcessFailedCount = Convert.ToInt16(ej.obj.ProcessFailedCount ?? 0 + 1);
                            ej.obj.ProcessFailedReason = ex.Message;
                            ej.obj.IsProcessed = false;
                            ej.Save();
                        }
                    }

                    ////TODO: Delete temp file
                    //if (System.IO.File.Exists(attachment[0]))
                    //    File.Delete(attachment[0]);
                }
            }
        }
    }
}