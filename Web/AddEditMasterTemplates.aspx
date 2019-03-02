<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="AddEditMasterTemplates.aspx.cs" Inherits="AddEditMasterTemplates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome
        {
            float: right;
            margin-right: 10px !important;
            width: 76.5% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">
        Add/Edit Master Template</h2>
    <div class="formdata" style="width: 73%">
        <div class="form-elements">
            <label for="name">
                Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtTemplateName" runat="server" CssClass="inputtext" MaxLength="250"
                placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter template name"
                CssClass="error-long" Display="Dynamic" ControlToValidate="txtTemplateName" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label>
                Header</label>
            <asp:TextBox ID="txtTemplateHeader" runat="server" name="header" TextMode="MultiLine"
                CssClass="textarea"></asp:TextBox>
        </div>
        <div class="form-elements">
            <label>
                Footer</label>
            <asp:TextBox ID="txtTemplateFooter" runat="server" name="footer" TextMode="MultiLine"
                CssClass="textarea"></asp:TextBox>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Template" CssClass="submit-button"
                ValidationGroup="Template" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="tagSections">
        <fieldset>
            <legend>Function Selector</legend>
            <div class="functionSelector_result optionSelector_result">
                <ul class="functionSelector optionSelector placeholder">
                    <li>Current User</li>
                    <li>Current Date</li>
                    <li>Current Time</li>
                    <li>User Signature</li>
                </ul>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />
    <script src="//cdn.ckeditor.com/4.4.3/full/ckeditor.js"></script>
    <script type="text/javascript">

        CKEDITOR.plugins.addExternal('lineheight', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/lineheight/', 'plugin.js');
        CKEDITOR.plugins.addExternal('wordcount', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/wordcount/', 'plugin.js');
        CKEDITOR.plugins.addExternal('notification', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/notification/', 'plugin.js');
        CKEDITOR.plugins.addExternal('undo', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/undo/', 'plugin.js');
        CKEDITOR.plugins.addExternal('htmlwriter', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/htmlwriter/', 'plugin.js');

        CKEDITOR.replace('ctl00$cphBody$txtTemplateHeader', {
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
            wordcount: { showParagraphs: false, showWordCount: false, showCharCount: true, countSpacesAsChars: true, countHTML: false, maxWordCount: -1, maxCharCount: -1 }
        });



        CKEDITOR.replace('ctl00$cphBody$txtTemplateFooter', {
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
            wordcount: { showParagraphs: false, showWordCount: false, showCharCount: true, countSpacesAsChars: true, countHTML: false, maxWordCount: -1, maxCharCount: -1 }
        });

//        CKEDITOR.on('instanceReady', function (e) {
//            e.editor.dataProcessor.htmlFilter.addRules({
//                elements: {
//                    p: function (e) { e.attributes.style = 'font-size:16px; line-height:17px; font-family:calibri;'; }
//                }
//            });
//        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {

            var activeElmt = "", isInput = false;

            //adding on focus handler
            CKEDITOR.instances['cphBody_txtTemplateFooter'].on('focus', function () {
                activeElmt = $(this);
                isInput = false;
            });

            $(".formdata").delegate("input[type='text']", "focus", function () {
                activeElmt = $(this);
                isInput = true;
            });


            $(document).on('click', '.placeholder li', function (e) { //$(".placeholder li").click(function () {
                if (activeElmt != "") {
                    if ($(this).closest("ul").hasClass("functionSelector")) {
                        if (isInput) {
                            activeElmt.val(activeElmt.val() + " [function: " + $(this).text() + "]");
                        }
                        else {
                            CKEDITOR.instances['cphBody_txtTemplateFooter'].insertHtml("[function: " + $(this).text() + "]");
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>