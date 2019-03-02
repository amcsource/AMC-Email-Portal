<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="SendBulkEmail.aspx.cs" Inherits="Emails_SendBulkEmail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Bulk Email</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
    </div>
    <div id="messageNotSent" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessageNotSent" runat="server"></asp:Label>
    </div>

    <div id="filter">
        <div class="search">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter search keyword" style="width: 60%;"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" onclick="btnSearch_Click" />
        </div>
        <div class="pagesize">
            <asp:DropDownList ID="ddlPatientType" runat="server" Width="200px">
                <asp:ListItem Value="0">Please Select Flag Type</asp:ListItem>
                <asp:ListItem Value="New">New Contacts</asp:ListItem>
                <asp:ListItem Value="Generic">Flagged Records</asp:ListItem>
                <asp:ListItem Value="Generic2">Flagged Records 2</asp:ListItem>
                <asp:ListItem Value="Receipts">Flagged Receipts</asp:ListItem>
                <asp:ListItem Value="SMS1">Flagged SMS1</asp:ListItem>
                <asp:ListItem Value="SMS2">Flagged SMS2</asp:ListItem>
                <asp:ListItem Value="12MonthPathEmail">12 Month Path Email</asp:ListItem>
				<asp:ListItem Value="12MonthPathEmail">12 Month Path Email DHEA</asp:ListItem>
            </asp:DropDownList>

            <asp:DropDownList ID="ddlPageSize" runat="server">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="0">All</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btnFilter" runat="server" Text="Filter" onclick="btnFilter_Click" />
        </div>
    </div>

    <div id="filterPatient">
        <div class="templates">
            <label>Choose Template</label>
            <asp:DropDownList ID="ddlTemplates" runat="server">
                <%--<asp:ListItem Value="0">Select template</asp:ListItem>
                <asp:ListItem Value="1">Template 1</asp:ListItem>
                <asp:ListItem Value="2">Template 2</asp:ListItem>--%>
            </asp:DropDownList>
        </div>
    </div>

    <asp:Repeater ID="rptPatients" runat="server">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th scope="col" style="width:5%;">
                                <asp:CheckBox ID="chkEmailAll" runat="server" CssClass="chkEmailAll" />
                            </th>
                            <th scope="col" style="width:5%;">
                                #
                            </th>
                            <th scope="col" style="width:25%;">
                                Patient Name/Number
                            </th>
                            <th scope="col" style="width:20%;">
                                Source
                            </th>
                            <th scope="col" style="width:20%;">
                                Stage
                            </th>
                            <th scope="col" style="width: 20%;">
                                Email
                            </th>
                            <th scope="col" style="width: 10%;">
                                Phone
                            </th>
                        </tr>
                    </thead>
                    <tbody class="records">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkEmail" runat="server" CssClass="chkEmail" />
                    </td>
                    <td>
                        <%# Container.ItemIndex + 1 %>
                    </td>
                    <td>
                        <asp:HiddenField ID="hdnPatientRecId" runat="server" Value='<%# Eval("PatientRecId") %>' />
                        <asp:Literal ID="ltlpatientName" runat="server" Text='<%# Eval("PatientName") %>'></asp:Literal> / <asp:Literal ID="ltlpatientNumber" runat="server" Text='<%# Eval("PatientNumber") %>'></asp:Literal>
                    </td>
                    <td>
                        <%# Eval("Source") %>
                    </td>
                    <td>
                        <%# Eval("Stage") %>
                    </td>
                    <td>
                        <%# Eval("Email") %>
                    </td>
                    <td>
                        <%# Eval("Phone") %>
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
    <div id="norecord" class="message norecord" runat="server" visible="false">
        <asp:Label ID="ltlNoRecord" runat="server" Text="No Record found"></asp:Label>
    </div>

    <div class="formdata" id="dvSendEmail" runat="server">
        <div class="formbuttons">
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="submit-button btnSend" OnClick="btnSend_Click" />
        </div>
    </div>

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
                </tr>
            </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:HiddenField ID="hdnIsDelete" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />
                    <asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltrFileSize" Text='<%# Eval("Size") %>' />
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {

            $(".btnSend").attr("disabled", "disabled");

            $(".chkEmail").on("click", function () {
                var c = 0;
                var AllCurrentCheckbox = 0;

                $(".chkEmail").each(function () {
                    if ($(this).find("input[type=checkbox]").is(':checked')) {
                        c = c + 1;
                    }
                    AllCurrentCheckbox = AllCurrentCheckbox + 1;
                });

                if (c == AllCurrentCheckbox) {
                    $("input[id*='chkEmailAll']").prop('checked', true);
                }
                else {
                    $("input[id*='chkEmailAll']").prop('checked', false);
                }
                if (c > 0) {
                    $(".btnSend").removeAttr("disabled");
                }
                else {
                    $(".btnSend").attr("disabled", "disabled");
                }
            });

            $("input[id*='chkEmailAll']").change(function () {
                if ($(this).prop('checked')) {
                    $('.records').find("input[type='checkbox']").each(function () {
                        $(this).prop('checked', true);
                        $(".btnSend").removeAttr("disabled");
                    });
                }
                else {
                    $('.records').find("input[type='checkbox']").each(function () {
                        $(this).prop('checked', false);
                        $(".btnSend").attr("disabled", "disabled");
                    });
                }
                return true;
            });

            $(".btnSend").click(function () {
                if ($("#<%= ddlTemplates.ClientID %>").val() == "0") {
                    alert('Select template first');
                    return false;
                }
                else {
                    $(this).val('Please wait...');
                }
            });

        });
    </script>
</asp:Content>