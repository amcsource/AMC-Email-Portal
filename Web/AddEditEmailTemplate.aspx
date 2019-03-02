<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="AddEditEmailTemplate.aspx.cs" Inherits="AddEditEmailTemplate" ValidateRequest="false" %>

<%--<%@ Register Assembly="TXTextControl.Web, Version=24.0.400.500, Culture=neutral, PublicKeyToken=6b83fe9a75cfb638" Namespace="TXTextControl.Web" TagPrefix="cc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome {
            margin-left: 10px !important;
            width: 97% !important;
        }

        .sms-text {
            width: 74% !important;
            height: 150px !important;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <h2 class="mainheading">Email Templates</h2>
    <div class="formdata" style="width: 73%">
        <div class="form-elements">
            <label>
                Name <span class="red">(required)</span></label>
            <asp:TextBox ID="txtTemplateName" runat="server" CssClass="inputtext" MaxLength="250"
                placeholder="Max 250 chars are allowed"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTemplateName" runat="server" ErrorMessage="Please enter template name"
                CssClass="error" Display="Dynamic" ControlToValidate="txtTemplateName" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">
                From <span class="red">(required)</span></label>
            <asp:TextBox ID="txtFrom" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfFrom" runat="server" ErrorMessage="Please enter From address"
                CssClass="error" Display="Dynamic" ControlToValidate="txtFrom" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">
                To <span class="red">(required)</span></label>
            <asp:TextBox ID="txtTo" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTo" runat="server" ErrorMessage="Please enter To address"
                CssClass="error" Display="Dynamic" ControlToValidate="txtTo" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">
                Cc</label>
            <asp:TextBox ID="txtCc" runat="server" CssClass="inputtext"></asp:TextBox>
        </div>
        <div class="form-elements">
            <label for="name">
                Bcc</label>
            <asp:TextBox ID="txtBcc" runat="server" CssClass="inputtext"></asp:TextBox>
        </div>
        <div class="form-elements">
            <label for="name">
                Subject</label>
            <asp:TextBox ID="txtSubject" runat="server" CssClass="inputtext"></asp:TextBox>
        </div>
        <div class="form-elements">
            <label for="name">
                Choose Master Template</label>
            <asp:DropDownList ID="ddlMasterTemplates" runat="server" CssClass="inputselect">
            </asp:DropDownList>
        </div>
        <div class="form-elements">
            <label for="name">
                Choose Email Template Category</label>
            <asp:DropDownList ID="ddlEmailTemplateCategories" runat="server" CssClass="inputselect">
            </asp:DropDownList>
        </div>
        <div class="form-elements">
            <label for="name" style="padding: 0px;">Store Letter in Patient's File</label>
            <asp:CheckBox ID="chkPatientLetter" runat="server" CssClass="patientLetter"></asp:CheckBox>
            &nbsp;            
        </div>

        <div class="patientFile">
            <div class="form-elements">
                <label>Patient File Name</label>
                <asp:TextBox ID="txtPatientFileName" runat="server" CssClass="inputtext"></asp:TextBox>
                &nbsp;            
            </div>
            <div class="form-elements">
                <label>Attachment Category</label>
                <asp:TextBox ID="txtAttachmentCategory" runat="server" CssClass="inputtext"></asp:TextBox>
                &nbsp;            
            </div>
            <div class="form-elements">
                <label>Attachment Description</label>
                <asp:TextBox ID="txtAttachmentDescription" runat="server" CssClass="inputtext"></asp:TextBox>
                &nbsp;            
            </div>
        </div>

        <div class="form-elements">
                <label for="name" style="padding: 0px;">Send letter as unencrypted file</label>
                <asp:CheckBox ID="chkUnecryptedFile" runat="server" CssClass=""></asp:CheckBox>
                &nbsp;            
            </div>

        <div class="form-elements">
            <label for="name" style="padding: 0px;">Send as SMS</label>
            <asp:CheckBox ID="chkSMS" runat="server" CssClass="sms"></asp:CheckBox>
            &nbsp;
        </div>

        <div class="body-editor">
            <div id="sms">
                <div class="form-elements">
                    <label>SMS</label>
                    <asp:TextBox ID="txtTemplateBodySMS" runat="server" name="header" TextMode="MultiLine" CssClass="textarea inputselect sms-text"></asp:TextBox>
                </div>
            </div>
            
            <div id="tabs">
                <ul>
                    <li><a href="#tabs-body">Body</a></li>
                    <li><a href="#tabs-attachment">Attachment</a></li>
                    <li><a href="#tabs-letter">Letter</a></li>
                </ul>
                <div id="tabs-body">
                    <div class="form-elements">
                        <asp:TextBox ID="txtTemplateBody" runat="server" name="header" TextMode="MultiLine"
                            CssClass="textarea"></asp:TextBox>
                    </div>
                </div>
                <div id="tabs-attachment">
                    <fieldset>
                        <legend>Include Attachments from</legend>
                        <div class="form-elements">
                            <asp:CheckBox ID="chkBusiness" runat="server" Text="Business Object" CssClass="attachmentOption" />
                        </div>
                        <div class="form-elements">
                            <label>Filter by name</label>
                            <asp:TextBox ID="txtFilterBusiness" runat="server" name="header" CssClass="inputtext" Text="*"></asp:TextBox>
                            <asp:RadioButtonList ID="rdoListBusiness" runat="server" RepeatDirection="Horizontal" CssClass="attachOptionList">
                                <asp:ListItem Value="Include All" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Include Latest"></asp:ListItem>
                                <asp:ListItem Value="Include Oldest"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <br />
                        <div class="form-elements">
                            <label>
                                <asp:CheckBox ID="chkDirectory" runat="server" Text="Directory" CssClass="attachmentOption directory" /></label>
                            <asp:TextBox ID="txtDirectoryPath" runat="server" name="header" CssClass="inputtext"></asp:TextBox>
                            <%--<asp:Button ID="btnBrowseDirectoy" runat="server" Text="..." CssClass="browseDirectory" />--%>
                        </div>
                        <div class="form-elements">
                            <label>
                                Filter by name</label>
                            <asp:TextBox ID="txtFilterDirectory" runat="server" name="header" CssClass="inputtext"
                                Text="*"></asp:TextBox>
                            <asp:RadioButtonList ID="rdoListDirectory" runat="server" RepeatDirection="Horizontal"
                                CssClass="attachOptionList">
                                <asp:ListItem Value="Include All" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Include Latest"></asp:ListItem>
                                <asp:ListItem Value="Include Oldest"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <br />
                        <div class="form-elements">
                            <label>
                                Browse file</label>
                            <div style="float: right; width: 74%;">
                                <table id="tblAttachments" class="templateAttachments">
                                    <asp:Repeater ID="rptAttachments" runat="server">
                                        <HeaderTemplate>
                                            <thead>
                                                <tr>
                                                    <th style="width: 60%">Name
                                                    </th>
                                                    <th style="width: 20%">Size
                                                    </th>
                                                    <th style="width: 20%">Remove
                                                    </th>
                                                </tr>
                                            </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hdnIsDelete" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />
                                                    <%--<%# Eval("Name") %>--%>
                                                    <asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />
                                                </td>
                                                <td>
                                                    <%--<%# Eval("Size") %>--%>
                                                    <asp:Literal runat="server" ID="ltrFileSize" Text='<%# Eval("Size") %>' />
                                                </td>
                                                <td>
                                                    <input type="button" class="BtnMinus" id="lnkDelete" value="-" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                                <table id="tblMoreAttachments" runat="server" class="templateAttachments">
                                    <tr class="rowstyle">
                                        <td style="width: 60%">
                                            <asp:FileUpload ID="fileUpload1" Multiple="Multiple" onchange="readURL(this);" runat="server" />
                                        </td>
                                        <td id="size" style="width: 20%"></td>
                                        <td style="width: 20%">
                                            <input type="button" class="BtnMinus" value="-" style="display: none" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <br />
                        <div class="form-elements">
                            <label>
                                <asp:CheckBox ID="chkInstruction" runat="server" Text="Instruction" CssClass="attachmentOption directory" /></label>
                                <asp:TextBox ID="txtInstruction" runat="server" name="header" CssClass="inputtext"></asp:TextBox>

                                <asp:CheckBox ID="chkCombineInstructions" runat="server" Text="If multiple instructions - combine" CssClass="attachmentOption" />
                        </div>
                    </fieldset>
                    <div class="attachOptions">
                        <span class="attachmentOption">
                            <asp:CheckBox ID="chkPrompt" runat="server" Text="Prompt to select attachments" /></span>
                        <span class="attachmentOption">
                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select all attachments" /></span>
                    </div>
                    <asp:HiddenField ID="hdnIsAttachmentAdded" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnAttachURLs" runat="server" />
                    <asp:HiddenField ID="hdnCurrentUser" runat="server" />
                </div>
                <div id="tabs-letter">
                    <%--<asp:HyperLink ID="hlLetter" runat="server" CssClass="link-letter-tab" NavigateUrl="~/TemplateWordEditor.aspx?TemplateId=0" Target="_blank">Manage Word Template</asp:HyperLink>--%>

                    <asp:TextBox ID="txtTemplateLetter" runat="server" name="header" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                </div>
            </div>
        </div>
        
        <div class="formbuttons">
            <asp:Button ID="btnSubmit" runat="server" Text="Save Template" CssClass="submit-button"
                ValidationGroup="Template" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" OnClick="btnCancel_Click" />
        </div>
    </div>
    <div class="tagSections">
        <fieldset>
            <legend>Tag Selector</legend>
            <div class="form-elements">
                <label>
                    Category:</label>
                <asp:DropDownList ID="ddlTagSelectorTables" runat="server" CssClass="inputselect tagTableSelect">
                </asp:DropDownList>
            </div>
            <div class="tagSelector_result optionSelector_result">
                <ul class="tagSelector optionSelector placeholder">
                </ul>
            </div>
        </fieldset>
        <fieldset>
            <legend>Function Selector</legend>
            <div class="functionSelector_result optionSelector_result">
                <ul class="functionSelector optionSelector placeholder">
                    <li>Current User</li>
                    <li>Current Date</li>
                    <li>Current Time</li>
                    <li>Current User Id</li>
                </ul>
            </div>
        </fieldset>
        <fieldset>
            <legend>Miscellaneous Selector</legend>
            <div class="miscellaneousSelector_result optionSelector_result">
                <ul class="miscellaneousSelector optionSelector placeholder">
                    <li data-content="<img src='[img_src]' alt='doctor sign' />">Doctor Signature</li>
                </ul>
            </div>
        </fieldset>

        <%--commented as ahmz asked to remove this--%>
        <%--<fieldset>
            <legend>Query Selector</legend>
            <div class="querySelector_result optionSelector_result">
                <ul class="querySelector optionSelector placeholder">
                    <asp:Repeater ID="rptQueryList" runat="server">
                        <ItemTemplate>
                            <li>
                                <%# Eval("Name") %></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </fieldset>--%>
    </div>
    <asp:HiddenField ID="hdnId" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script type="text/javascript" src="//code.jquery.com/ui/1.11.1/jquery-ui.js"></script>
    <script type="text/javascript" src="//cdn.ckeditor.com/4.4.3/full/ckeditor.js"></script>
    <script type="text/javascript">

        CKEDITOR.plugins.addExternal('lineheight', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/lineheight/', 'plugin.js');
        CKEDITOR.plugins.addExternal('wordcount', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/wordcount/', 'plugin.js');
        CKEDITOR.plugins.addExternal('notification', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/notification/', 'plugin.js');
        CKEDITOR.plugins.addExternal('undo', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/undo/', 'plugin.js');
        CKEDITOR.plugins.addExternal('htmlwriter', '<%= URLRewrite.BasePath() %>/ckeditor/plugins/htmlwriter/', 'plugin.js');

        CKEDITOR.replace('ctl00$cphBody$txtTemplateBody', {
            //uiColor: '#A996C1',
            height: '375px',
            toolbar: [
                { name: 'source', items: ['Source'] },
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            {
	                name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-',
                    'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
	            },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] },
            //'/',
	            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
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


        CKEDITOR.replace('ctl00$cphBody$txtTemplateLetter', {
            //uiColor: '#A996C1',
            height: '375px',
            toolbar: [
                { name: 'source', items: ['Source'] },
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	            {
	                name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-',
                    'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
	            },
	            { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
	            { name: 'insert', items: ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'] },
            //'/',
	            { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'] },
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


        //Capatilize first char    
        //        CKEDITOR.on('instanceReady', function (event) {
        //            var editor = event.editor;
        //            if (typeof (editor) !== 'undefined') {
        //                editor.focus();

        //                editor.document.getBody().on('keydown', function (event) {
        //                    var keyCode = event.data.getKeystroke();
        //                    if (keyCode >= 65 /*a*/ && keyCode <= 90 /*&& isFirstLetter(editor)*/) {
        //                        // insert 'A' instead of 'a'
        //                        var char = String.fromCharCode(keyCode);
        //                        editor.insertText(char);
        //                        event.data.preventDefault();
        //                    }
        //                });
        //            }
        //        });

        //        function isFirstLetter(editor) {
        //            var range, walker, node;

        //            range = editor.getSelection().getRanges()[0];
        //            range.setStartAt(editor.document.getBody(), CKEDITOR.POSITION_AFTER_START);
        //            walker = new CKEDITOR.dom.walker(range);
        //            walker.guard = function (node) {
        //                console.log(node);
        //                console.log(node.TextNode);
        //            };

        //            while (node = walker.previous()) { }
        //        }





        //        CKEDITOR.on('instanceReady', function (e) {
        //            e.editor.dataProcessor.htmlFilter.addRules({
        //                elements: {
        //                    p: function (e) { e.attributes.style = 'font-size:16px; line-heigth:17px; font-family:calibri;'; }
        //                }
        //            });
        //        });


        //        CKEDITOR.on('instanceReady', function (event) {
        //            var editor = event.editor;
        //            if (typeof (editor) !== 'undefined') {
        //                editor.focus();
        //                var element = editor.document.getBody()
        //                var range = editor.createRange();
        //                if (range) {
        //                    range.moveToElementEditablePosition(element, false);
        //                    range.select();
        //                }
        //            }
        //        });


    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            var activeTabIndex = 0;

            $("#tabs").tabs();
            $("#tabs").bind("tabsactivate", function (event, ui) {
                activeTabIndex = ui.newTab.index();

                if (ui.newTab.index() == 1) {
                    $("#<%= hdnIsAttachmentAdded.ClientID %>").val("1");
                }
                //else if (ui.newTab.index() == 2) {
                //    var url = $(".link-letter-tab").prop('href');
                //    window.open(url, "_blank", "fullscreen=yes,toolbar=no,scrollbars=yes,resizable=no");
                //    $( "#tabs" ).tabs({ active: 0 });
                //}
            });

            //if ($(".check-Letter input[type='checkbox']").is(':checked')) {
            //    $(".link-letter").show();
            //}
            //else {
            //    $(".link-letter").hide();
            //}


            //$(".check-Letter").click(function () {
            //    $(".link-letter").toggle();
            //});

            //$(".link-letter").click(function () {
            //    var url = $(this).prop('href');
            //    window.open(url, "_blank", "fullscreen=yes,toolbar=no,scrollbars=yes,resizable=no");
            //    return false;
            //});


            if ($(".patientLetter input[type='checkbox']").is(':checked')) {
                $(".patientFile").show();
            }
            else {
                $(".patientFile").hide();
            }

            $(".patientLetter").click(function () {
                $(".patientFile").toggle();
            });

            if ($(".sms input[type='checkbox']").is(':checked')) {
                $("#sms").show();
                $("#tabs").hide();
            }
            else {
                $("#sms").hide();
                $("#tabs").show();
            }

            $(".sms").click(function () {
                $("#sms").toggle();
                $("#tabs").toggle();
            });
            

            var tableName = $("#<%= ddlTagSelectorTables.ClientID %> option:selected").val();
            var activeElmt = "", isInput = false;

            GetTableColumns(tableName);

            $("#<%= ddlTagSelectorTables.ClientID %>").on('change', function () {
                tableName = $(this).val();
                GetTableColumns(tableName);
            });

            function GetTableColumns(tableName) {
                $.ajax({
                    type: "POST",
                    url: "AddEditEmailTemplate.aspx/GetColumnsByTableName",
                    data: "{'tableName':'" + tableName + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('.tagSelector').html(msg.d);
                    }
                });
            }

            $(document).ajaxStart(function () {
                $('.tagSelector').html("<b>Please wait...</b>");
            });


            //adding on focus handler
            CKEDITOR.instances['cphBody_txtTemplateBody'].on('focus', function () {
                activeElmt = $(this);
                isInput = false;
            });
            CKEDITOR.instances['cphBody_txtTemplateLetter'].on('focus', function () {
                activeElmt = $(this);
                isInput = false;
            });


            $(".formdata").delegate("input[type='text'], textarea", "focus", function () {
                activeElmt = $(this);
                isInput = true;
            });


            $(document).on('click', '.placeholder li', function (e) { //$(".placeholder li").click(function () {
                if (activeElmt != "") {
                    if ($(this).closest("ul").hasClass("tagSelector")) {
                        tableName = $("#<%= ddlTagSelectorTables.ClientID %> option:selected").val();
                            if (isInput) {
                                activeElmt.val(activeElmt.val() + " [field: " + tableName + "." + $(this).text() + "]");
                            }
                            else {
                                if (activeTabIndex == 0) {
                                    CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[field: " + tableName + "." + $(this).text() + "]");
                                }
                                if (activeTabIndex == 2) {
                                    CKEDITOR.instances['cphBody_txtTemplateLetter'].insertHtml("[field: " + tableName + "." + $(this).text() + "]");
                                }
                            }
                        }
                        else if ($(this).closest("ul").hasClass("functionSelector")) {
                            if (isInput) {
                                activeElmt.val(activeElmt.val() + " [function: " + $(this).text() + "]");
                            }
                            else {
                                if (activeTabIndex == 0) {
                                    CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[function: " + $(this).text() + "]");
                                }
                                if (activeTabIndex == 2) {
                                    CKEDITOR.instances['cphBody_txtTemplateLetter'].insertHtml("[function: " + $(this).text() + "]");
                                }
                            }
                        }
                        else if ($(this).closest("ul").hasClass("querySelector")) {
                            if (!isInput) {
                                if (activeTabIndex == 0) {
                                    CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[query: " + $(this).text() + "]");
                                }
                                if (activeTabIndex == 2) {
                                    CKEDITOR.instances['cphBody_txtTemplateLetter'].insertHtml("[query: " + $(this).text() + "]");
                                }
                            }
                        }
                        else if ($(this).closest("ul").hasClass("miscellaneousSelector")) {
                            if (!isInput) {
                                if (activeTabIndex == 0) {
                                    //CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml($(this).data("content"));
                                    CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[misc: " + $(this).text() + "]");
                                }
                                if (activeTabIndex == 2) {
                                    CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[misc: " + $(this).text() + "]");
                                }
                            }
                        }
                    }
            });


            $("#tabs-attachment .attachmentOption").each(function () {
                toggleAttachmentFields($(this).find("input[type = 'checkbox']"));
            });

            $(".attachmentOption input[type='checkbox']").click(function () {
                toggleAttachmentFields($(this));
            });

            function toggleAttachmentFields(elem) {
                if (elem.is(":checked")) {
                    elem.closest("div").next().find("input").each(function () {
                        $(this).removeAttr("disabled");
                    });
                    if (elem.parent().hasClass('directory')) {
                        elem.closest("label").siblings("input").each(function () {
                            $(this).removeAttr("disabled");
                        });
                    }
                }
                else {
                    elem.closest("div").next().find("input").each(function () {
                        $(this).attr("disabled", "disabled");
                    });
                    if (elem.parent().hasClass('directory')) {
                        elem.closest("label").siblings("input").each(function () {
                            $(this).attr("disabled", "disabled");
                        });
                    }
                }
            }




            $("#tblAttachments").on("click", ".BtnMinus", function () {
                if (confirm("Do you want to remove this file?")) {
                    var par = $(this).parent().parent();
                    par.find('input[id*="hdnIsDelete"]').val('1');
                    par.hide();
                }
            });

            $("#<%= tblMoreAttachments.ClientID %>").on("click", ".BtnMinus", function () {
                if (confirm("Do you want to remove this file?")) {
                    var par = $(this).parent().parent();
                    par.remove();
                }
            });

        });

        var ID = 2;
        function addRow() {
            var html =
                    '<tr>' +
                    '<td style="width: 60%"><input type="file" multiple="multiple" onchange="readURL(this);" name="fileUpload' + ID + '" /></td>' +
                    '<td style="width: 20%"></td>' +
                    '<td style="width: 20%"><input type="button" class="BtnMinus" value="-" style="display:none" /></td>' +
                    '</tr>'
            $(html).prependTo($("#<%= tblMoreAttachments.ClientID %>"))
            ID++;
        };

        function addFileRow(fileName, fileSize) {
            var html =
                    '<tr>' +
                    '<td style="width: 60%">' + fileName + '</td>' +
                    '<td style="width: 20%">' + fileSize + '</td>' +
                    '<td style="width: 20%"><input type="button" class="BtnMinus" value="-" /></td>' +
                    '</tr>'
            $(html).appendTo($("#<%= tblMoreAttachments.ClientID %>"))
        };

        function readURL(evt) {
            var elem = $(evt);
            var fileCount = evt.files.length;
            for (var i = 0; i < fileCount; i++) {
                var size = evt.files[i].size;
                if (size <= 1024)
                    size = (size).toFixed(2) + " Bytes";
                else if ((size / 1024) <= 1024)
                    size = (size / 1024).toFixed(2) + " KB";
                else
                    size = (size / (1024 * 1024)).toFixed(2) + " MB";

                addFileRow(evt.files[i].name, size);

                elem.closest("tr").hide();
            }
            addRow();
        }

    </script>
</asp:Content>
