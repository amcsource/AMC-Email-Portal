<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddEditDoctor.aspx.cs" Inherits="AddEditDoctor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Add/Edit Doctor</h2>
    <div class="formdata" style="width: 100%">
        <div class="form-elements">
            <label for="name">Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtName" runat="server" CssClass="inputtext" MaxLength="200" placeholder="Max 200 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter name" CssClass="error"
                Display="Dynamic" ControlToValidate="txtName" ForeColor="Red" ValidationGroup="Doctor"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>Signature Image <span class="red">(required)</span></label>
            <asp:FileUpload ID="fileImage" runat="server" />
            <br />
            <asp:Image ID="imgDocImage" runat="server" Width="100" Visible="false" />
                <asp:RequiredFieldValidator ID="rqfImage" runat="server" ErrorMessage="Please select image" CssClass="error"
                Display="Dynamic" ControlToValidate="fileImage" ForeColor="Red" ValidationGroup="Doctor"></asp:RequiredFieldValidator>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Doctor" CssClass="submit-button"
                ValidationGroup="Doctor" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    
    <asp:HiddenField ID="hdnId" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
    <script src="js/custom.js" type="text/javascript"></script>
</asp:Content>