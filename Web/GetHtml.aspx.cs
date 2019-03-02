using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL_AMCPE;

public partial class GetHtml : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (!string.IsNullOrWhiteSpace(Request.QueryString["EmailId"]))
            //{
            //    Emails el = new Emails();
            //    int emailId = Convert.ToInt32(Request.QueryString["EmailId"]);
            //    var data = el.GetEmailByEmailId(emailId);
            //    if (data != null)
            //    {
            //        ltlText.Text = data.Letter;
            //    }
            //}
            //else if (!string.IsNullOrWhiteSpace(Request.QueryString["TemplateId"]))
            //{
            //    EmailTemplates et = new EmailTemplates();
            //    int templateId = Convert.ToInt32(Request.QueryString["TemplateId"]);
            //    var data = et.GetTemplateDetailByID(templateId);
            //    if (data != null)
            //    {
            //        ltlText.Text = data.Letter;
            //    }
            //}
        }
    }
}