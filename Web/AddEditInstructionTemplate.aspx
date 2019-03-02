<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="AddEditInstructionTemplate.aspx.cs" Inherits="AddEditInstructionTemplate" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome
        {
            margin-left: 10px !important;
            width: 97% !important;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <h2 class="mainheading">
        Instruction Templates</h2>
      <div class="formdata">
        <div class="form-elements">
            <label for="name">
                Template Name <span class="red">(required)</span></label>
                <asp:TextBox ID="txtTemplateName" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqftxtCategoryName" runat="server" ErrorMessage="Please enter category name" Display="Dynamic" CssClass="error-long"
             ControlToValidate="txtTemplateName" ForeColor="Red" ValidationGroup="TagCategory"></asp:RequiredFieldValidator>
        </div>
         <div class="form-elements">
            <label for="name">
                Template Path <span class="red">(required)</span></label>
                <asp:TextBox ID="txtTemplatePath" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter path" Display="Dynamic" CssClass="error-long"
             ControlToValidate="txtTemplatePath" ForeColor="Red" ValidationGroup="TagCategory"></asp:RequiredFieldValidator>
        </div>

        <div class="form-elements">
            <label for="name">
                Is Active <span class="red">(required)</span></label>
                <asp:CheckBox ID="checkboxactive" runat="server" CssClass="inputcheckbox" ></asp:CheckBox>
          
        </div>
      <%--   <div class="form-elements">
            <label for="name">
                Is Delete <span class="red">(required)</span></label>
            --%>
                <asp:CheckBox ID="checkboxdelete"   Visible="false" runat="server" CssClass="inputcheckbox" ></asp:CheckBox>
          
       <%-- </div>--%>
        

        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Template" CssClass="submit-button" onclick="btnSubmit_Click" ValidationGroup="TagCategory" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" onclick="btnCancel_Click" />
        </div>
    </div>

    <asp:HiddenField ID="hdnid" runat="server" />
</asp:Content>
