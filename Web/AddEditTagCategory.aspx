<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddEditTagCategory.aspx.cs" Inherits="AddEditTagCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Add/Edit Tag Category</h2>
    <div class="formdata">
        <div class="form-elements">
            <label for="name">
                Category Name <span class="red">(required)</span></label>
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqftxtCategoryName" runat="server" ErrorMessage="Please enter category name" Display="Dynamic" CssClass="error-long"
             ControlToValidate="txtCategoryName" ForeColor="Red" ValidationGroup="TagCategory"></asp:RequiredFieldValidator>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Category" CssClass="submit-button" onclick="btnSubmit_Click" ValidationGroup="TagCategory" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" onclick="btnCancel_Click" />
        </div>
    </div>
    <asp:HiddenField ID="hdnCategoryId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>