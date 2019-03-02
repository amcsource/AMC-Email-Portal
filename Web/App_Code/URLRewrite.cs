using System;
using System.Collections.Generic;
using System.Web;
/// <summary>
/// Summary description for URLRewrite
/// </summary>
public class URLRewrite
{
    public URLRewrite()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// Function to get the base virtual path of the current Site.
    /// </summary>
    /// <returns>String, Current Root path of the Site.</returns>
    public static String BasePath()
    {
        return String.Format("http://{0}{1}", HttpContext.Current.Request.ServerVariables["HTTP_HOST"], (HttpContext.Current.Request.ApplicationPath.Equals("/") ? String.Empty : HttpContext.Current.Request.ApplicationPath));
    }
}