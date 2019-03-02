<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ProcessEmailJob.aspx.cs" Inherits="Emails_ProcessEmailJob" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .session-expired
        {
            margin: 150px auto;
            width: 800px;
            height: 200px;
            font-size: 23px;
            text-align: center;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:Repeater ID="rptAttachments" runat="server" Visible="false">
        <HeaderTemplate>
            <thead>
                <tr>
                    <th style="width: 60%">
                        Name
                    </th>
                    <th style="width: 20%">
                        Size
                    </th>
                    <th style="width: 20%">
                        Remove
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

    <div class="session-expired">
        <asp:Label ID="lblMessage" runat="server" Text="Please wait while email is being sent."></asp:Label>
    </div>

    <div id="EmailConfirmation" style="display: none">
        <div class="formdata">
            To filed is missing so email could not be sent.
        </div>
    </div>

    <asp:HiddenField ID="hdnAttachURLs" runat="server" />
    <asp:HiddenField ID="hdnMailSent" runat="server" Value="1" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="//code.jquery.com/ui/1.11.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#<%= hdnMailSent.ClientID %>").val() == "1") {
                $("#EmailConfirmation").dialog(
                {
                    title: "To field missing",
                    width: 700,
                    height: 250,
                    modal: true
                    }
                });
            }
        });
    </script>
</asp:Content>