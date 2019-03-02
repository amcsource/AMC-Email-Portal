<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoProcessEmailJob.aspx.cs" Inherits="Emails_AutoProcessEmailJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Repeater ID="rptAttachments" runat="server" Visible="false">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th style="width: 60%">Name
                        </th>
                        <th style="width: 20%">Size
                        </th>
                        <th style="width: 20%">Remove
                        </th>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%--<asp:HiddenField ID="hdnIsDelete" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />--%>
                        <%--<asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />--%>

                        <asp:HiddenField ID="hdnIsDelete" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />
                        <asp:HiddenField ID="hdnInclude" runat="server" Value='<%# Eval("Include") %>' />

                        <asp:HyperLink ID="hlFileUrl" runat="server" Target="_blank" NavigateUrl='<%# Eval("FileWebURL") %>' Width="500px">
                            <asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />
                        </asp:HyperLink>

                    </td>
                    <td>
                        <%--<%# Eval("Size") %>--%>
                        <asp:Literal runat="server" ID="ltrFileSize" Text='<%# Eval("Size") %>' />
                    </td>
                    <td>
                        <input type="button" id="lnkDelete" value="-" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>

        <asp:HiddenField ID="hdnTemplateId" runat="server" />
    <asp:HiddenField ID="hdnEmailId" runat="server" />
    <asp:HiddenField ID="hdnCurrentUser" runat="server" />
    <asp:HiddenField ID="hdnAction" runat="server" />
    <asp:HiddenField ID="hdnPatientNumber" runat="server" />
    <asp:HiddenField ID="hdnPatientRecId" runat="server" />
    <asp:HiddenField ID="hdnPatientFullName" runat="server" />
    <asp:HiddenField ID="hdnbusinessFilter" runat="server" />
    <asp:HiddenField ID="hdnbusinessInclude" runat="server" />
    <asp:HiddenField ID="hdnhasBusinessAttachment" runat="server" />
    <asp:HiddenField ID="hdnselectAllAttachments" runat="server" />
    <asp:HiddenField ID="hdnstorePatientFile" runat="server" />
    <asp:HiddenField ID="hdnSendSMS" runat="server" />
    <asp:HiddenField ID="hdnSendUnencryptedFile" runat="server" />

    <%--<asp:HiddenField ID="hdnAttachingMore" runat="server" />--%>
    <%--<asp:HiddenField ID="hdnUserDirectory" runat="server" />--%>
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnTo" runat="server" />
    <asp:HiddenField ID="hdnCc" runat="server" />
    <asp:HiddenField ID="hdnBcc" runat="server" />
    <asp:HiddenField ID="hdnSubject" runat="server" />
    <asp:HiddenField ID="hdnBody" runat="server" />
    <asp:HiddenField ID="hdnLetter" runat="server" />
    
    <asp:HiddenField ID="hdnPatientFileName" runat="server" />
    <asp:HiddenField ID="hdnAttachmentCategory" runat="server" />
    <asp:HiddenField ID="hdnAttachmentDescription" runat="server" />

    <asp:HiddenField ID="hdnAttachURLs" runat="server" />
    <asp:HiddenField ID="hdnShowPromptDialog" runat="server" Value="0" />
    <asp:HiddenField ID="hdnPromptFileUrl" runat="server" Value="" />
    <asp:HiddenField ID="hdnPromptFileName" runat="server" Value="" />
    </form>
</body>
</html>