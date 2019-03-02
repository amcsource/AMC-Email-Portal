<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="SentEmails.aspx.cs" Inherits="Emails_SentEmails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">
        Sent</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>

    <div id="search">
        <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter search keyword"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" />
    </div>

    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" onitemdatabound="rptList_ItemDataBound">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr style="width: 100%">
                        <th scope="col" style="width: 15%">
                            Patient detail
                        </th>
                        <%--<th scope="col" style="width: 20">
                            From
                        </th>--%>
                        <th scope="col" style="width: 20%">
                            Template Name
                        </th>
                        <th scope="col" style="width: 37%">
                            Subject
                        </th>
                        <th scope="col" style="width: 10%">
                            Sent
                        </th>
                        <th scope="col" style="width: 3%">
                            &nbsp;
                        </th>
                        <th scope="col" style="width: 15%;">
                            Action
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# Eval("PatientFullName") %><br />
                    <%# Eval("PatientNumber") %>
                </td>
                <%--<td>
                    <%# Eval("From") %>
                </td>--%>
                <td>
                    <%# Eval("EmailTemplateName")%>
                </td>
                <td>
                    <%# Convert.ToString(Eval("Subject")).Length > 50 ? Convert.ToString(Eval("Subject")).Substring(0, 50) + " ..." : Eval("Subject") %>
                </td>
                <td>
                    <%# Eval("SentBy")%>, <%# Convert.ToDateTime(Eval("SentOn")).ToString("dd/MM/yyyy <br/> hh:mm tt") %>
                </td>
                <td>
                    <%# (Convert.ToBoolean(Eval("HasAttachments")) && !Convert.ToBoolean(Eval("SendASSMS"))) ? "<i class='fa fa-paperclip fa-lg' title='this email has attachments'>" : "" %>
                </td>
                <td>
                    <asp:LinkButton ID="View" runat="server" PostBackUrl='<%# "SendEmail.aspx?action=view&EmailId=" + Eval("Id") %>' ToolTip="View"><i class="fa fa-eye"></i></asp:LinkButton>
                    <asp:LinkButton ID="Send" runat="server" PostBackUrl='<%# "SendEmail.aspx?action=resend&EmailId=" + Eval("Id") %>' ToolTip="Resend"><i class="fa fa-location-arrow"></i></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl='<%# "SendEmail.aspx?action=forward&EmailId=" + Eval("Id") %>' ToolTip="Forward"><i class="fa fa-mail-forward"></i></asp:LinkButton>
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
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
</asp:Content>
