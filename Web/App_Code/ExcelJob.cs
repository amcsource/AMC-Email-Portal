using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using BAL_AMCPE;
using DAL_AMCPE;

/// <summary>
/// Summary description for EmailJob
/// </summary>
public class ExcelJob : IJob
{
    public void Execute(IJobExecutionContext context)
    {
        try
        {
            bool processJob = false;

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ExcelJob"]))
            {
                processJob = Convert.ToBoolean(ConfigurationManager.AppSettings["ExcelJob"]);
            }

            if (processJob)
            {
                string uri = Convert.ToString(ConfigurationManager.AppSettings["JobExcelURL"]);
                WebRequest req = HttpWebRequest.Create(uri);
                WebResponse res = req.GetResponse();
            }
        }
        catch (WebException ex)
        {
            FlexiMail ml = new FlexiMail();
            ml.From = Convert.ToString(ConfigurationManager.AppSettings["FROM"]);
            ml.FromName = "AMC Patient Email";
            ml.To = "rohitashw.choudhary@dotsquares.com";
            ml.CC = "";
            ml.BCC = "";
            ml.Subject = "Error in Send Weekly Report";
            ml.EmailTemplateFileName = "";
            ml.AttachFile = null;
            ml.MailBodyManualSupply = true;
            ml.MailBody = "Email Job could not be processed.<br />" + ex.Message;
            ml.Send();
        }
    }
}