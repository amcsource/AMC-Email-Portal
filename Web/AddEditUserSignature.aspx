<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="AddEditUserSignature.aspx.cs" Inherits="AddEditUserSignature" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome
        {
            margin-left: 224px !important;
            width: 76% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">
        Add/Edit Master Template</h2>
    <div class="formdata">
        <div class="form-elements">
            <label for="name">
                Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtName" runat="server" CssClass="inputtext" MaxLength="250"
                placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter name"
                CssClass="error-long" Display="Dynamic" ControlToValidate="txtName" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">
                User Name <span class="red">(required)</span></label>
            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="inputselect"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select user name"
                CssClass="error-long" Display="Dynamic" ControlToValidate="ddlUsers" ForeColor="Red" InitialValue="0"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>Signature <span class="red">(required)</span></label>
            <asp:TextBox ID="txtSignature" runat="server" name="header" TextMode="MultiLine"
                CssClass="textarea"></asp:TextBox>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Signature" CssClass="submit-button"
                ValidationGroup="Template" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" OnClick="btnCancel_Click" />
        </div>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="//cdn.ckeditor.com/4.4.3/full/ckeditor.js"></script>
    <script type="text/javascript">

        CKEDITOR.plugins.addExternal('lineheight', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/lineheight/', 'plugin.js');
        CKEDITOR.plugins.addExternal('wordcount', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/wordcount/', 'plugin.js');
        CKEDITOR.plugins.addExternal('notification', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/notification/', 'plugin.js');
        CKEDITOR.plugins.addExternal('undo', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/undo/', 'plugin.js');
        CKEDITOR.plugins.addExternal('htmlwriter', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/htmlwriter/', 'plugin.js');

        CKEDITOR.replace('ctl00$cphBody$txtSignature', {
            toolbar: [
                { name: 'source', items: ['Source'] },
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-',
                'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
	            },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] },
            //'/',
	            {name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
	            { name: 'colors', items: ['TextColor', 'BGColor'] },
	            { name: 'tools', items: ['Maximize', 'ShowBlocks'] }
            ],
            contentsCss: '<%= URLRewrite.BasePath() %>/css/ckeditor.css',
            extraPlugins: 'lineheight,wordcount,notification,undo,htmlwriter',
            font_names: 'Arial/Arial, Helvetica, sans-serif;' +
                        'Calibri;' +
	                    'Comic Sans MS/Comic Sans MS, cursive;' +
	                    'Courier New/Courier New, Courier, monospace;' +
	                    'Georgia/Georgia, serif;' +
	                    'Lucida Sans Unicode/Lucida Sans Unicode, Lucida Grande, sans-serif;' +
	                    'Tahoma/Tahoma, Geneva, sans-serif;' +
	                    'Times New Roman/Times New Roman, Times, serif;' +
	                    'Trebuchet MS/Trebuchet MS, Helvetica, sans-serif;' +
	                    'Verdana/Verdana, Geneva, sans-serif',
            font_defaultLabel: 'Calibri',
            line_height: "5px; 10px; 15px; 17px;",
            fontSize_sizes: '8/10.5px;11/16px;16/21px;20/26.5px;',
            disableNativeSpellChecker: false,
            enterMode: CKEDITOR.ENTER_BR,
            wordcount: { showParagraphs: false, showWordCount: false, showCharCount: true, countSpacesAsChars: true, countHTML: false, maxWordCount: -1, maxCharCount: -1},
            filebrowserUploadUrl: '<%= URLRewrite.BasePath()%>/Handlers/Upload.ashx'
        });
        
    </script>
</asp:Content>