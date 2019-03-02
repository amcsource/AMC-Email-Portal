<%@ Page Language="C#" AutoEventWireup="true" CodeFile="directoryBrowse.aspx.cs" Inherits="Demo_directoryBrowse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Browse" onclick="Button1_Click" />
        <br />
        <asp:Button ID="Button2" runat="server" Text="Get Drives" onclick="Button2_Click" />
        <br />
        <asp:Button ID="Button3" runat="server" Text="Get Host Names" onclick="Button3_Click" />

    </div>
    </form>
</body>
</html>
