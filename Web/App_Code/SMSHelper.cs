using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for SMSHelper
/// </summary>
public class SMSHelper
{
	public static string SendSMS(string mobileNumber, string message)
    {
        string html = string.Empty;
        string url = string.Format("http://sms.theamc.com.au/api.php?md=send_sms&user=admin&pass=Notify&num={0}&msg={1}", mobileNumber, HttpUtility.UrlEncode(message));

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.AutomaticDecompression = DecompressionMethods.GZip;

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            html = reader.ReadToEnd();
        }
        
        return html;
    }
}