<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="Groups.aspx.cs" Inherits="Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">Groups</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
    <asp:Repeater ID="rptGroups" runat="server" 
        OnItemCommand="rptGroups_ItemCommand" onitemdatabound="rptGroups_ItemDataBound">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr>
                        <th scope="col" style="width:5%;">
                            ID
                        </th>
                        <th scope="col" style="width:60%;">
                            Group Name
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
                <td>
                    <%# Eval("Id") %>
                </td>
                <td>
                    <%# Eval("GroupName") %>
                </td>
                <td>
                    <%# Eval("CreatedBy") %>
                </td>
                <td>
                    <%# Eval("UpdatedBy") %>
                </td>
                <td>
                    <%--<a href='AddEditGroup.aspx?Id=<%# Eval("Id") %>' title="Edit"><i class="fa fa-pencil"></i></a>
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/i_delete.png" CommandName="Delete"
                        CommandArgument='<%# Eval("Id") %>' OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                    </asp:ImageButton>--%>
                    
                    <asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditGroup.aspx?Id=" + Eval("Id") %>'><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' OnClientClick='return confirm("Are you sure you want to delete this group, if deleted all associated permissions and users will be deleted as well?");'>
                        <i class="fa fa-times"></i>
                    </asp:LinkButton>
                    <asp:LinkButton ID="User" runat="server" PostBackUrl='<%# "AddEditGroupUser.aspx?return=groups&GroupId=" + Eval("Id") %>' ToolTip="Click to add user in this group"><i class="fa fa-user"></i></asp:LinkButton>
                    
                    
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Literal ID="ltlNoRecord" runat="server" Visible="false" Text="No Record found"></asp:Literal>
</asp:Content>
