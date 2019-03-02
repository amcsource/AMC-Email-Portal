<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AddEditTagSQL.aspx.cs" Inherits="AddEditTagSQL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <h2 class="mainheading">Add/Edit Tag SQL</h2>
    <div class="formdata" style="width: 73%">
        <div class="form-elements">
            <label for="name">
                Tag Category <span class="red">(required)</span></label>
            <asp:DropDownList ID="ddlTagCategory" runat="server" CssClass="inputselect">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose category" CssClass="error"
                Display="Dynamic" ControlToValidate="ddlTagCategory" ForeColor="Red" ValidationGroup="TagQuery" InitialValue="0"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtName" runat="server" CssClass="inputtext" MaxLength="250" placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter name" CssClass="error"
                Display="Dynamic" ControlToValidate="txtName" ForeColor="Red" ValidationGroup="TagQuery"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>Query <span class="red">(required)</span></label>
            <asp:TextBox ID="txtTaqQuery" runat="server" name="header" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rqfQuery" runat="server" ErrorMessage="Please enter query" CssClass="error"
                Display="Dynamic" ControlToValidate="txtTaqQuery" ForeColor="Red" ValidationGroup="TagQuery"></asp:RequiredFieldValidator>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Query" CssClass="submit-button"
                ValidationGroup="TagQuery" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="tagSections">
        <fieldset>
            <legend>Variable Selector</legend>
            <div class="functionSelector_result optionSelector_result">
                <ul class="functionSelector optionSelector placeholder">
                    <li>Patient Number</li>
                    <li>Patient RecID</li>
                    <li>Current User</li>
                    <li>PresType</li>
                </ul>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" Runat="Server">
    <script src="js/custom.js" type="text/javascript"></script>
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
                        //activeElmt.val(activeElmt.val() + "'@" + $(this).text() + "'");
                        activeElmt.insertAtCaret("'@" + $(this).text() + "'");
                    }
                }
            }
        });
    </script>
</asp:Content>