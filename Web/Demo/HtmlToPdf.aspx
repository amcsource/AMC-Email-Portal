﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HtmlToPdf.aspx.cs" Inherits="Demo_HtmlToPdf" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br />
        <asp:Button ID="btnSend" Text="Send" runat="server" OnClick="btnSend_Click" />
    </div>
    </form>
</body>
</html>
