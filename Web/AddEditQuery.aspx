<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="AddEditQuery.aspx.cs" Inherits="AddEditQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">
        Add/Edit SQL Query</h2>
    <div class="formdata" style="width: 73%">
        <div class="form-elements">
            <label for="name">
                Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtName" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter name"
                CssClass="error" Display="Dynamic" ControlToValidate="txtName" ForeColor="Red"
                ValidationGroup="SQLQuery"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>
                Query <span class="red">(required)</span></label>
            <asp:TextBox ID="txtSQLQuery" runat="server" name="header" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfQuery" runat="server" ErrorMessage="Please enter query"
                CssClass="error" Display="Dynamic" ControlToValidate="txtSQLQuery" ForeColor="Red"
                ValidationGroup="SQLQuery"></asp:RequiredFieldValidator>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Query" CssClass="submit-button"
                ValidationGroup="SQLQuery" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="tagSections">
        <fieldset>
            <legend>Variable Selector</legend>
            <div class="functionSelector_result optionSelector_result">
                <ul class="functionSelector optionSelector placeholder">
                    <li>Patient Number</li>
                    <li>Patient RecID</li>
                </ul>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        var activeElmt = "", isInput = false;

        $(".formdata").delegate("textarea", "focus", function () {
            activeElmt = $(this);
            isInput = true;
        });

        $(document).on('click', '.placeholder li', function (e) {
            if (activeElmt != "") {
                if ($(this).closest("ul").hasClass("functionSelector")) {
                    if (isInput) {
                        activeElmt.val(activeElmt.val() + "'@" + $(this).text()+"'");
                    }
                }
            }
        });
    </script>
</asp:Content>