<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Regex.aspx.cs" Inherits="Demo_Regex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtUrl" runat="server" Width="250px"></asp:TextBox>
        <br />
        <asp:Button ID="btnValidateUrl" runat="server" Text="Validate Url" 
            onclick="btnValidateUrl_Click" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" Text="Result"></asp:Label>
    
    </div>
    </form>
</body>
</html>
