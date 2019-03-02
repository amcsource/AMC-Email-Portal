<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Queries.aspx.cs" Inherits="Queries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">SQL Queries</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
        <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" onitemdatabound="rptList_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th scope="col" style="width: 5%;">
                                ID
                            </th>
                            <th scope="col" style="width: 60%;">
                                Name
                            </th>
                            <th scope="col" style="width: 12%;">
                                Created By
                            </th>
                            <th scope="col" style="width: 13%;">
                                Last Updated By
                            </th>
                            <th scope="col" style="width: 10%;">
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
                        <%# Eval("Name") %>
                    </td>
                    <td>
                        <%# Eval("CreatedBy") %>
                    </td>
                    <td>
                        <%# Eval("UpdatedBy") %>
                    </td>
                    <td>
                       <%-- <a href='AddEditQuery.aspx?Id=<%# Eval("Id") %>' title="Edit"><i class="fa fa-pencil"></i></a>
                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/i_delete.png" CommandName="Delete"
                            CommandArgument='<%# Eval("Id") %>' OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                        </asp:ImageButton>--%>

                        <asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditQuery.aspx?Id=" + Eval("Id") %>' ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
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
        <asp:Label ID="ltlNoRecord" runat="server" Visible="false" Text="No Record found" CssClass="no-record"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>