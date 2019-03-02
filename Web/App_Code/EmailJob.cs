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
public class EmailJob : IJob
{
	public EmailJob()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void Execute(IJobExecutionContext context)
    {
        try
        {
            bool processEmailJob = false;

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ProcessEmailJob"]))
            {
                processEmailJob = Convert.ToBoolean(ConfigurationManager.AppSettings["ProcessEmailJob"]);
            }

            if (processEmailJob)
            {
                EmailJobProcess ej = new EmailJobProcess();
                List<DAL_AMCPE.EmailJob> jobs = ej.GetEmailJobs();
                if (jobs.Count > 0)
                {
                    foreach (var job in jobs)
                    {
                        string uri = Convert.ToString(ConfigurationManager.AppSettings["JobURL"]);  //"http://localhost/healthstrong/SendWeeklyReport.aspx";
                        uri = uri + "?PatientNumber=" + job.PatientNumber + "&RecId=" + job.PatientRecId + "&EmailProcessId=" + job.Id + "&TemplateId=" + job.TemplateId + "&PresType=" + job.Prescription;
                        WebRequest req = HttpWebRequest.Create(uri);
                        WebResponse res = req.GetResponse();
                    }
                }
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