<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Drafts.aspx.cs" Inherits="Emails_Drafts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Drafts</h2>
        <div id="message" class="message" runat="server" visible="false">
            <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
        </div>
        <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr style="width:100%">
                            <th scope="col" style="width:20%">
                                From
                            </th>
                            <th scope="col" style="width:60%">
                                Subject
                            </th>
                            <th scope="col" style="width:10%">
                                Saved on
                            </th>
                            <th scope="col" style="width:3%">
                                &nbsp;
                            </th>
                            <th scope="col" style="width:7%;">
                                Action
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("From") %>
                    </td>
                    <td>
                        <%# Convert.ToString(Eval("Subject")).Length > 50 ? Convert.ToString(Eval("Subject")).Substring(0, 50) + " ..." : Eval("Subject") %>
                    </td>
                    <td>
                        <%# Convert.ToDateTime(Eval("SentOn")).ToString("dd/MM/yyyy <br/> hh:mm tt") %>
                    </td>
                    <td>
                        <%# Convert.ToBoolean(Eval("HasAttachments")) == true ? "<i class='fa fa-paperclip fa-lg' title='this email has attachments'>" : "" %>
                    </td>
                    <td>
                        <%--<a href='SendDraftMail.aspx?Id=<%# Eval("Id") %>' title="Edit"><i class="fa fa-external-link"></i></a>
                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/i_delete.png" CommandName="Delete"
                            CommandArgument='<%# Eval("Id") %>' OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                        </asp:ImageButton>--%>

                        <asp:LinkButton ID="Send" runat="server" PostBackUrl='<%# "SendEmail.aspx?action=senddraft&EmailId=" + Eval("Id") %>' ToolTip="Send"><i class="fa fa-location-arrow"></i></asp:LinkButton>
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

        <asp:Panel ID="pnlPagination" runat="server" Visible="false">
        <div class="pagination">
            <div class="left">
                <asp:ImageButton ID="lnkFirst" runat="server" ImageUrl="../images/paging/btn_first.gif"
                    OnClick="lnkFirst_Click" />
                <asp:ImageButton ID="lnkPrev" runat="server" ImageUrl="../images/paging/btn_prev.gif"
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
                <asp:ImageButton ID="lnkNext" runat="server" ImageUrl="../images/paging/btn_next.gif"
                    OnClick="lnkNext_Click" />
                <asp:ImageButton ID="lnkLast" runat="server" ImageUrl="../images/paging/btn_last.gif"
                    OnClick="lnkLast_Click" />
            </div>
        </div>
    </asp:Panel>
        <asp:Label ID="ltlNoRecord" runat="server" Visible="false" Text="No Record found"
        CssClass="no-record"></asp:Label>

        <asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>