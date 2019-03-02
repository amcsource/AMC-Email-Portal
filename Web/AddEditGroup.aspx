<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="AddEditGroup.aspx.cs" Inherits="AddEditGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">Add/Edit Group</h2>
    <div class="formdata">
        <div class="form-elements">
            <label for="name">
                Group Name <span class="red">(required)</span></label>
                <asp:TextBox ID="txtGroupName" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfGroupName" runat="server" ErrorMessage="Please enter group name" Display="Dynamic" CssClass="error-long"
             ControlToValidate="txtGroupName" ForeColor="Red" ValidationGroup="Group"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="category">
                Permissions</label>
            <asp:CheckBoxList ID="chkPermissions" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CssClass="checkBoxList">
                <asp:ListItem>Can Create Group</asp:ListItem>
                <asp:ListItem>Can Edit Group</asp:ListItem>
                <asp:ListItem>Can Delete Group</asp:ListItem>
                <asp:ListItem>Can Edit Other Group</asp:ListItem>
                <asp:ListItem>Can Delete Other Group</asp:ListItem>

                <%--<asp:ListItem>Can Create User</asp:ListItem>
                <asp:ListItem>Can Edit User</asp:ListItem>
                <asp:ListItem>Can Delete User</asp:ListItem>
                <asp:ListItem>Can Edit Other User</asp:ListItem>
                <asp:ListItem>Can Delete Other User</asp:ListItem>--%>

                <%--<asp:ListItem>Can Create SQL Query</asp:ListItem>
                <asp:ListItem>Can Edit SQL Query</asp:ListItem>
                <asp:ListItem>Can Delete SQL Query</asp:ListItem>
                <asp:ListItem>Can Edit Other SQL Query</asp:ListItem>
                <asp:ListItem>Can Delete Other SQL Query</asp:ListItem>--%>

                <asp:ListItem>Can Create Template</asp:ListItem>
                <asp:ListItem>Can Edit Template</asp:ListItem>
                <asp:ListItem>Can Delete Template</asp:ListItem>
                <asp:ListItem>Can Edit Other Template</asp:ListItem>
                <asp:ListItem>Can Delete Other Template</asp:ListItem>

                <asp:ListItem>Can Send Email</asp:ListItem>
                <asp:ListItem>Can Delete Email</asp:ListItem>
                <asp:ListItem>Can Delete Other Email</asp:ListItem>
            </asp:CheckBoxList>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Group" CssClass="submit-button" onclick="btnSubmit_Click" ValidationGroup="Group" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" onclick="btnCancel_Click" />
        </div>
    </div>

    <asp:HiddenField ID="hdnId" runat="server" />
</asp:Content>
