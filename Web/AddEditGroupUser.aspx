<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddEditGroupUser.aspx.cs" Inherits="AddEditGroupUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Add Edit Group User</h2>
    <div class="formdata">
       <%-- <asp:Label ID="lblMessage" runat="server" Text="Some error occurred, please try again" Visible="false"></asp:Label>--%>
        <div class="form-elements">
            <label for="name">Group <span class="red">(required)</span></label>
            <asp:DropDownList ID="ddlGroups" runat="server" CssClass="inputselect">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rqfGroupName" runat="server" ErrorMessage="Please choose group" CssClass="error-long"
                Display="Dynamic" InitialValue="0" ControlToValidate="ddlGroups" ForeColor="Red" ValidationGroup="GroupUser"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>User Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtUserName" runat="server" name="header" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqfUserName" runat="server" ErrorMessage="Please enter user name" CssClass="error-long"
                Display="Dynamic" ControlToValidate="txtUserName" ForeColor="Red" ValidationGroup="GroupUser"></asp:RequiredFieldValidator>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save User" CssClass="submit-button"
                ValidationGroup="GroupUser" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />
    <asp:HiddenField ID="hdnGroupId" runat="server" />
    <asp:HiddenField ID="hdnReturnTo" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>