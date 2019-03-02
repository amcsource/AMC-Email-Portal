<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="Doctors.aspx.cs" Inherits="Doctors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr>
                        <th scope="col" style="width: 5%;">
                            ID
                        </th>
                        <th scope="col" style="width: 25%;">
                            Name
                        </th>
                        <th scope="col" style="width: 10%;">
                            Created By
                        </th>
                        <th scope="col" style="width: 12%;">
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
                    <%# Eval("DoctorName") %>
                </td>
                <td>
                    <%# Eval("CreatedBy") %>
                </td>
                <td>
                    <%# Eval("UpdatedBy") %>
                </td>
                <td>
                    <asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditDoctor.aspx?Id=" + Eval("Id") %>'
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
    <asp:Panel ID="pnlPagination" runat="server" Visible="false">
        <div class="pagination">
            <div class="left">
                <asp:ImageButton ID="lnkFirst" runat="server" ImageUrl="images/paging/btn_first.gif"
                    OnClick="lnkFirst_Click" />
                <asp:ImageButton ID="lnkPrev" runat="server" ImageUrl="images/paging/btn_prev.gif"
                    OnClick="lnkPrev_Click" />
                <asp:Repeater ID="rptPagination" runat="server" OnItemCommand="rptPagination_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPageN" runat="server" CommandName="Page" CommandArgument='<%#Container.DataItem%>'
                            Text='<%#Container.DataItem%>' Enabled='<%#  Convert.ToInt16(Container.DataItem) == (CurrentPage+1) ? false:true %>'
                            CssClass='<%#  Convert.ToInt16(Container.DataItem) == (CurrentPage+1) ? "gray":"green" %>'></asp:LinkButton>
                        <asp:Label ID="lblSpacer" runat="server" Text='<%# Convert.ToInt32(Container.DataItem) == TotalPages ? "" : "|" %>'
                            CssClass="spacer"></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:LinkButton ID="lnkEllipses" runat="server" CommandName="PageRange" Text="..."
                    OnClick="lnkEllipses_Click" Visible="false"></asp:LinkButton>
                <div class="pageText">
                    <span>of &nbsp;</span>
                    <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                </div>
                <asp:ImageButton ID="lnkNext" runat="server" ImageUrl="images/paging/btn_next.gif"
                    OnClick="lnkNext_Click" />
                <asp:ImageButton ID="lnkLast" runat="server" ImageUrl="images/paging/btn_last.gif"
                    OnClick="lnkLast_Click" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
</asp:Content>
