<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="UpdateTemplates.aspx.cs" Inherits="UpdateTemplates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">UpdateTemplates</h2>
    <div id="message" class="message" runat="server" visible="false">
        <asp:Label ID="lblMessage" runat="server" Text="this is text"></asp:Label>
    </div>
    
    <div id="filter">
        <div class="search">
            <asp:TextBox Visible="false" ID="txtSearch" runat="server" placeholder="Enter search keyword" style="width: 60%;"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Update" onclick="btnSearch_Click" />
        </div>
        <div class="pagesize">
        

           <asp:DropDownList ID="ddlDocTemplate" runat="server"  style="width: 25%;"
                onselectedindexchanged="ddlDocTemplate_SelectedIndexChanged">

                      </asp:DropDownList>

           <asp:DropDownList ID="ddlxpres" runat="server"  style="width: 25%;"
                onselectedindexchanged="ddlxpres_SelectedIndexChanged">
                    <asp:ListItem Text="All Prescription" Value="0" />
               <asp:ListItem Text="Print WS" Value="PrintWS" />
                  <asp:ListItem Text="Print Receipt" Value="PrintReceipt" />
                     <asp:ListItem Text="Generic Print1" Value="GenericPrint1" />
                        <asp:ListItem Text="Generic Print2" Value="GenericPrint2" />
                           <asp:ListItem Text="Generic Print3" Value="GenericPrint3" />
            </asp:DropDownList>
            
            <asp:DropDownList ID="ddlPageSize" runat="server" style="width: 25%;">
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="0">All</asp:ListItem>
            </asp:DropDownList>
          
        
            <asp:Button ID="btnFilter"  runat="server" Text="Filter" onclick="btnFilter_Click" />
        </div>
    </div>
    <asp:CheckBoxList  ID="cblCustomerList" runat="server" onclick="readCheckBoxList()"   >
              
            </asp:CheckBoxList>
    <asp:Repeater ID="rptTemplates" runat="server" OnItemCommand="rptTemplates_ItemCommand" onitemdatabound="rptTemplates_ItemDataBound">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <%--<th scope="col" style="width:5%;">
                                ID
                            </th>--%>
                            <th scope="col" style="width:60%;">
                                Template Name
                            </th>
                            <th scope="col" style="width:10%;">
                                Template Path
                            </th>
                            <th scope="col" style="width:10%;">
                                Activated
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
                    <%--<td>
                        <%# Eval("Id") %>
                    </td>--%>
                    <td>
                        <%# Eval("TemplateName")%>
                    </td>
                    <td>
                        <%# Eval("TemplatePath")%>
                    </td>
                    <td>
                        <%# Eval("IsActive") %>
                    </td>
                    <td>
                        <%--<asp:LinkButton ID="Edit" runat="server" PostBackUrl='<%# "AddEditEmailTemplate.aspx?Id=" + Eval("Id") %>' ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>--%>
                        <asp:LinkButton ID="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("Id") %>' ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
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
    <div id="norecord" class="message norecord" runat="server" visible="false">
        <asp:Label ID="ltlNoRecord" runat="server" Text="No Record found"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>