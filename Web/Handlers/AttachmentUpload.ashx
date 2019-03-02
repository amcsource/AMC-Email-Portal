<%@ WebHandler Language="C#" Class="AttachmentUpload" %>

using System;
using System.Web;
using System.IO;

public class AttachmentUpload : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "multipart/form-data";
        context.Response.Expires = -1;


        try
        {
            HttpPostedFile postedFile = context.Request.Files["file"];
            string savepath = HttpContext.Current.Server.MapPath("~/TempAttachments/");
            var extension = Path.GetExtension(postedFile.FileName);

            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);

            var file = Guid.NewGuid().ToString() + extension;
            if (file != null)
            {
                var fileLocation = string.Format("{0}/{1}", savepath, file);
                postedFile.SaveAs(fileLocation);

                var fileWebLocation = URLRewrite.BasePath() + "/TempAttachments/" + file;
                context.Response.Write(fileWebLocation);
                context.Response.StatusCode = 200;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
        }
        
        //HttpPostedFile uploads = context.Request.Files["upload"];

        //string CKEditorFuncNum = context.Request["CKEditorFuncNum"];

        //string file = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(uploads.FileName);

        //uploads.SaveAs(context.Server.MapPath("~") + "\\Uploads\\" + file);

        //string url = URLRewrite.BasePath() + "/Uploads/" + file;

        //context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");

        //context.Response.End();

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}