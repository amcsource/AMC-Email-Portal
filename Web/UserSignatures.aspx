<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="UserSignatures.aspx.cs" Inherits="UserSignatures" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">
        User Signatures</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
    <asp:Repeater ID="rptTemplates" runat="server" OnItemCommand="rptTemplates_ItemCommand">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr>
                        <th scope="col" style="width: 25%;">
                            User name
                        </th>
                        <th scope="col" style="width: 30%;">
                            Signature Name
                        </th>
                        <th scope="col" style="width: 18%;">
                            Created By
                        </th>
                        <th scope="col" style="width: 18%;">
                            Last Updated By
                        </th>
                        <th scope="col" style="width: 11%;">
                            Action
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("UserId") %>
                </td>
                <td>
                    <%# Eval("Name") %>
                </td>
                <td>
                    <%# Eval("CreatedBy") %>
                </td>
                <td>
                    <%# Eval("UpdatedBy") %>
                </td>
                <td>
                    <asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditUserSignature.aspx?Id=" + Eval("Id") %>'
                        ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>'
                        ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                            <i class="fa fa-times"></i>
                    </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <div id="norecord" class="message norecord" runat="server" visible="false">
        <asp:Literal ID="ltlNoRecord" runat="server" Text="No Record found"></asp:Literal>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
</asp:Content>
