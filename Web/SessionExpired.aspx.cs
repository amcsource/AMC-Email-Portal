using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SessionExpired : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Expire User session explicitly.
        Session["UserId"] = null; 

        if (!string.IsNullOrEmpty(Request.QueryString["reason"]))
        {
            if (Convert.ToString(Request.QueryString["reason"]).ToLower() == "denied")
            {
                lblMessage.Text = "Seems you have been visiting page which you are not authorised for." + "<br/>"
                                    + "Please login again from CRM with proper detail and try again";
            }
            else if (Convert.ToString(Request.QueryString["reason"]).ToLower() == "duplicateuser")
            {
                lblMessage.Text = "Seems you have been trying to access with other user name while you were already logged in." + "<br/>"
                                    + "You have been logged out now and Please login again from CRM with proper detail and try again";
            }
            else if (Convert.ToString(Request.QueryString["reason"]).ToLower() == "nouser")
            {
                lblMessage.Text = "No user is registered with the name you have been trying to log in." + "<br/>"
                                    + "Please login from CRM with proper detail and try again";
            }
            else if (Convert.ToString(Request.QueryString["reason"]).ToLower() == "nopatient")
            {
                //lblMessage.Text = "No patient has been selected to send mail." + "<br/>"
                //                    + "Please login from CRM again with Patient RecId and Patient Number and try again";

                lblMessage.Text = "To send an email, Please close this window and run the quick action from GMEE.";
            }
            else if (Convert.ToString(Request.QueryString["reason"]).ToLower() == "notemplate")
            {
                lblMessage.Text = "No email template has been selected to send mail." + "<br/>"
                                    + "Please login from CRM again and pass template name";
            }
        }
        else if (Session["UserId"] == null)
        {
            lblMessage.Text = "Seems your session has expired." + "<br/>"
                    + "Please login again from CRM and try again";
        }
    }
}