using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for FlexiMail
/// </summary>
public class FlexiMail
{
    #region Constructors-Destructors
    public FlexiMail()
    {
        //set defaults 
        myEmail = new System.Net.Mail.MailMessage();
        _MailBodyManualSupply = false;
        _IsBodyHtml = true;
    }
    #endregion

    #region  Class Data
    private string _From;
    private string _FromName;
    private string _To;
    private string _ToList;
    private string _Subject;
    private string _CC = "";
    private string _CCList;
    private string _BCC = "";
    private string _TemplateDoc;
    private string[] _ArrValues;
    private string _BCCList;
    private bool _MailBodyManualSupply;
    private bool _IsBodyHtml;
    private string _MailBody;
    private string[] _Attachment;
    private System.Net.Mail.MailMessage myEmail;

    #endregion

    #region Propertie
    public string From
    {
        get { return _From; }
        set { _From = value; }
    }
    public string FromName
    {
        get { return _FromName; }
        set { _FromName = value; }
    }
    public string To
    {
        get { return _To; }
        set { _To = value; }
    }
    public string Subject
    {
        get { return _Subject; }
        set { _Subject = value; }
    }
    public string CC
    {
        get { return _CC; }
        set { _CC = value; }
    }
    public string BCC
    {
        get { return _BCC; }
        set { _BCC = value; }
    }
    public bool MailBodyManualSupply
    {
        get { return _MailBodyManualSupply; }
        set { _MailBodyManualSupply = value; }
    }
    public bool IsBodyHtml
    {
        get { return _IsBodyHtml; }
        set { _IsBodyHtml = value; }
    }
    public string MailBody
    {
        get { return _MailBody; }
        set { _MailBody = value; }
    }
    public string EmailTemplateFileName
    {
        get { return _TemplateDoc; }
        //FILE NAME OF TEMPLATE ( MUST RESIDE IN ../EMAILTEMPLAES/ FOLDER ) 
        set { _TemplateDoc = value; }
    }
    public string[] ValueArray
    {
        get { return _ArrValues; }
        //ARRAY OF VALUES TO REPLACE VARS IN TEMPLATE 
        set { _ArrValues = value; }
    }

    public string[] AttachFile
    {
        get { return _Attachment; }
        set { _Attachment = value; }
    }

    #endregion

    #region SEND EMAIL

    public void Send()
    {
        myEmail.IsBodyHtml = _IsBodyHtml;

        //set mandatory properties 
        if (_FromName == "")
            _FromName = _From;
        myEmail.From = new MailAddress(_From, _FromName);
        myEmail.Subject = _Subject;

        //---Set recipients in To List 
        _ToList = _To.Replace(";", ",");
        if (_ToList != "")
        {
            string[] arr = _ToList.Split(',');
            myEmail.To.Clear();
            if (arr.Length > 0)
            {
                foreach (string address in arr)
                {
                    myEmail.To.Add(new MailAddress(address));
                }
            }
            else
            {
                myEmail.To.Add(new MailAddress(_ToList));
            }
        }

        //---Set recipients in CC List 
        _CCList = _CC.Replace(";", ",");
        if (_CCList != "")
        {
            string[] arr = _CCList.Split(',');
            myEmail.CC.Clear();
            if (arr.Length > 0)
            {
                foreach (string address in arr)
                {
                    myEmail.CC.Add(new MailAddress(address));
                }
            }
            else
            {
                myEmail.CC.Add(new MailAddress(_CCList));
            }
        }

        //---Set recipients in BCC List 
        _BCCList = _BCC.Replace(";", ",");
        if (_BCCList != "")
        {
            string[] arr = _BCCList.Split(',');
            myEmail.Bcc.Clear();
            if (arr.Length > 0)
            {
                foreach (string address in arr)
                {
                    myEmail.Bcc.Add(new MailAddress(address));
                }
            }
            else
            {
                myEmail.Bcc.Add(new MailAddress(_BCCList));
            }
        }

        //set mail body 
        if (_MailBodyManualSupply)
        {
            myEmail.Body = _MailBody;
        }
        else
        {
            myEmail.Body = GetHtml(_TemplateDoc);
        }

        if (!myEmail.IsBodyHtml)
        {
            ////myEmail.BodyEncoding = System.Text.Encoding.UTF8;
            ////myEmail.HeadersEncoding = System.Text.Encoding.UTF8;
            ////myEmail.SubjectEncoding = System.Text.Encoding.UTF8;

            //myEmail.BodyEncoding = System.Text.Encoding.ASCII;
            //myEmail.HeadersEncoding = System.Text.Encoding.ASCII;
            //myEmail.SubjectEncoding = System.Text.Encoding.ASCII;

            //myEmail.Body = null;

            //var plainView = AlternateView.CreateAlternateViewFromString(_MailBody, myEmail.BodyEncoding, "text/plain");
            //plainView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
            //myEmail.AlternateViews.Add(plainView);


            //Send directly SMS
            if (!string.IsNullOrWhiteSpace(_To))
            {
                string phoneNumber = _To.Split('@')[0];
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    SMSHelper.SendSMS(phoneNumber, _MailBody);
                }
            }

        }
        else
        {
            // set attachment 
            if (_Attachment != null)
            {
                for (int i = 0; i < _Attachment.Length; i++)
                {
                    if (_Attachment[i] != null)
                        myEmail.Attachments.Add(new Attachment(_Attachment[i]));
                }

            }

            SmtpClient client = new SmtpClient();
            client.Host = System.Configuration.ConfigurationManager.AppSettings["host"];
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["username"], System.Configuration.ConfigurationManager.AppSettings["password"]);
            client.Send(myEmail);
        }

    }
    #endregion

    #region GetHtml
    public string GetHtml(string argTemplateDocument)
    {
        int i;
        StreamReader filePtr;
        string fileData = "";

        filePtr = File.OpenText(HttpContext.Current.Request.PhysicalApplicationPath + "\\EmailTemplate\\" + argTemplateDocument);
        fileData = filePtr.ReadToEnd();
        filePtr.Close();
        filePtr = null;

        if ((_ArrValues != null))
        {
            for (i = _ArrValues.GetLowerBound(0); i <= _ArrValues.GetUpperBound(0); i++)
            {
                fileData = fileData.Replace("@v" + i.ToString() + "@", (string)_ArrValues[i]);
            }
        }
        return fileData;

    }
    #endregion
}