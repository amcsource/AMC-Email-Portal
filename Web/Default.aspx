<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" media="screen" />
    <style>
        .section
        {
            background: none; 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <header class="header">
        <div class="header_left">
            <a href="">
                <img src="<%= URLRewrite.BasePath()%>/images/logo.png"></a>
        </div>
        <div class="navbar_right">
            Welcome <span class="activeuser">
                <asp:Literal ID="ltlUserName" runat="server"></asp:Literal></span>
        </div>
        <div class="clear"></div>
    </header>
    <div class="section">
        <div class="center">
            <div class="action">
                <asp:ImageButton ID="imgGroups" runat="server" ImageUrl="~/images/actions/managegroup.png"
                    Enabled="false" PostBackUrl="~/Groups.aspx" />
            </div>
            <div class="action">
                <asp:ImageButton ID="imgTemplates" runat="server" ImageUrl="~/images/actions/Manage-Templates.png"
                    Enabled="false" PostBackUrl="~/MasterTemplates.aspx" />
            </div>
            <%--commented as ahmz said to remove this tab, but do not remove permanemtly for future reference--%>
            <%--<div class="action">
                <asp:ImageButton ID="imgQueries" runat="server" ImageUrl="~/images/actions/Manage-Queries.png" Enabled="false" PostBackUrl="~/Queries.aspx" />
            </div>--%>
            <div class="action">
                <asp:ImageButton ID="imgEmails" runat="server" ImageUrl="~/images/actions/sendemails.png"
                    Enabled="false" PostBackUrl="~/Emails/SendEmail.aspx" />
            </div>
            <div class="clr"></div>
        </div>
        
    </div>
    </form>
</body>
</html>
