<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="GroupUsers.aspx.cs" Inherits="GroupUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">Group Users</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
    
    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" onitemdatabound="rptList_ItemDataBound">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr>
                        <%--<th scope="col">
                            ID
                        </th>--%>
                        <th scope="col">
                            Group Name
                        </th>
                        <th scope="col">
                            User
                        </th>
                         <th scope="col">
                            Created By
                        </th>
                        <th scope="col">
                            Last Updated By
                        </th>
                        <th scope="col" style="width: 95px;">
                            Action
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <%--<td>
                    <%# Eval("Id") %>
                </td>--%>
                <td>
                    <%# Eval("GroupName") %>
                </td>
                <td>
                    <%# Eval("UserId") %>
                </td>
                <td>
                    <%# Eval("CreatedBy") %>
                </td>
                <td>
                    <%# Eval("UpdatedBy") %>
                </td>
                <td>
                    <asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditGroupUser.aspx?return=groupuser&Id=" + Eval("Id") %>' ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                        <i class="fa fa-times"></i>
                    </asp:LinkButton>

                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Label ID="lblMessage2" runat="server" Text="Some error occurred, please try again" Visible="false"></asp:Label>
    <asp:Label ID="ltlNoRecord" runat="server" Visible="false" Text="No Record found" CssClass="no-record"></asp:Label>
    
    <asp:HiddenField ID="hdnGroupId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
</asp:Content>
