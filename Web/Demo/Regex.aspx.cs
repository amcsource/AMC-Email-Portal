using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Demo_Regex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////string s = "This is text [Field: Contact.Id] string of [function: Current USer] demo";
        ////Regex regex = new Regex("[(*)]");
        ////var v = regex.Match(s);
        ////string result = v.Groups[0].ToString();
        ////Response.Write(result);


        ////string input = "This is text [Field: Contact.Id] string of [function: Current USer]"; // "(one)(two)(three)(four)(five)"; 
        //////string[] result = Regex.Split(input, @"\[([^]]*)\]");



        ////string[] result = input.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
        ////foreach (string s in result)
        ////    Response.Write(s + "<br />");


        /////////////////  WORKING //////////////////

        ////string regularExpressionPattern = @"\[(.*?)\]"; // return with brackets
        //string regularExpressionPattern = @"(?<=\[)(.*?)(?=\])"; // return without brackets

        //string inputText = "This is text [Field: Contact.Id] string of [function: Current User] demo.";

        //// returns reslut in Matches Collection
        ////Regex re = new Regex(regularExpressionPattern);

        ////foreach (Match m in re.Matches(inputText))
        ////{
        ////    Response.Write(m.Value + "<br/>");
        ////}


        //// returns result in string[]
        //string[] res = Regex.Matches(inputText, regularExpressionPattern)
        //                    .Cast<Match>()
        //                    .Select(m => m.Value)
        //                    .ToArray();

        //foreach (string s in res)
        //{
        //    Response.Write(s + "<br/>");
        //}
    }
    protected void btnValidateUrl_Click(object sender, EventArgs e)
    {
        bool isValid = false;
        if(txtUrl.Text.Contains("http"))
            isValid = Regex.IsMatch(txtUrl.Text, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        else
            isValid = Regex.IsMatch(txtUrl.Text, @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");

        if(isValid)
            lblMessage.Text = "Validated";
        else
            lblMessage.Text = "Not Validated";

        //if (Uri.IsWellFormedUriString(txtUrl.Text, UriKind.Absolute))
        //{
        //    lblMessage.Text = "Validated";
        //}
        //else
        //    lblMessage.Text = "Not Validated";
    }
}