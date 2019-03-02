<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UltimateEditor.aspx.cs" Inherits="Demo_HtmlToPdf" %>

<%@ Register TagPrefix="kswc" Namespace="Karamasoft.WebControls.UltimateEditor" Assembly="UltimateEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <kswc:UltimateEditor ID="UltimateEditor1" runat="server" EditorSource="ClientSideEvents.xml" Width="400px" OnToolbarItemRender="UltimateEditor1_ToolbarItemRender"></kswc:UltimateEditor>
        </div>
        <br />
        <div class="tagSections" style="width: 26%; float: left;">
            <fieldset>
                <legend>Function Selector</legend>
                <div class="functionSelector_result optionSelector_result">
                    <ul class="functionSelector optionSelector placeholder">
                        <li>Current User</li>
                        <li>Current Date</li>
                        <li>Current Time</li>
                        <li>Current User Id</li>
                    </ul>
                </div>
            </fieldset>
        </div>
        <div>
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
        </div>
    </form>

    <script type="text/javascript" src="<%= URLRewrite.BasePath()%>/js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".functionSelector li").click(function () {
                var ultimateEditorObj = UltimateEditors['<%=UltimateEditor1.ClientID%>'];
                // Store the current range in the editor so that current range will not be lost once the editor loses the focus
                ultimateEditorObj.StoreCurrentRange();
                // Insert HTML fragment into the editor
                ultimateEditorObj.InsertHTML($(this).text());
            });
        });
    </script>
</body>
</html>
