<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetHtml.aspx.cs" Inherits="GetHtml" EnableViewState="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
		@page {
			size: A4;
			margin: 0;
		}
		
		body
		{
			width: 900px;
		}
		
		/*body table {
			width: 100%;
		}*/
		
		@media print {
			body {
				margin: 0;
				border: initial;
				border-radius: initial;
				width: initial;
				min-height: initial;
				box-shadow: initial;
				background: initial;
				page-break-after: always;
			}
		}
	</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="ltlText" runat="server"></asp:Literal>
    </form>
</body>
</html>
