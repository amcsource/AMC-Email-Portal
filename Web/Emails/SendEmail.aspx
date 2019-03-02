<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true"
    CodeFile="SendEmail.aspx.cs" Inherits="Emails_SendEmail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome {
            margin-left: 10px !important;
            width: 97% !important;
        }

        .previewEmail {
            overflow: auto;
        }

        .sms-text {
            width: 74% !important;
            height: 150px !important;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <h2 class="mainheading">Send Email</h2>
    <div class="formdata" id="chooseTemplate" runat="server" visible="false">
        <div class="form-elements">
            <label for="name">
                Choose Email Template Category</label>
            <asp:DropDownList ID="ddlEmailTemplateCategories" runat="server" CssClass="inputselect"
                AutoPostBack="true" OnSelectedIndexChanged="ddlEmailTemplateCategories_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div class="form-elements">
            <label for="name">
                Choose Template <span class="red">(required)</span></label>
            <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="inputselect" AutoPostBack="true"
                OnSelectedIndexChanged="ddlTemplates_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Choose template"
                CssClass="error-long" Display="Dynamic" ControlToValidate="ddlTemplates" ForeColor="Red"
                InitialValue="0"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="formdata" id="divEmail" runat="server">
        <div class="form-elements">
            <label for="name">
                From <span class="red">(required)</span></label>
            <asp:TextBox ID="txtFrom" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfFrom" runat="server" ErrorMessage="Please enter From address"
                CssClass="error-long" Display="Dynamic" ControlToValidate="txtFrom" ForeColor="Red"
                ValidationGroup="Template"></asp:RequiredFieldValidator>
        </div>
        <div class="form-elements">
            <label for="name">
                To <span class="red">(required)</span></label>
            <asp:TextBox ID="txtTo" runat="server" CssClass="inputtext"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rqfTo" runat="server" ErrorMessage="Please enter To address"
                CssClass="error-long" Display="Dynamic" ControlToValidate="txtTo" ForeColor="Red"
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



        <div id="patientFile" runat="server">
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
        
        <div class="body-editor">
            <div id="sms" runat="server" clientidmode="Static">
                <div class="form-elements">
                    <label>SMS</label>
                    <asp:TextBox ID="txtTemplateBodySMS" runat="server" name="header" TextMode="MultiLine" CssClass="textarea inputselect sms-text"></asp:TextBox>
                </div>
            </div>

            <div id="tabs" runat="server" clientidmode="Static">
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
                    <div class="email-attachments">
                        <table id="tblAttachments">
                            <asp:Repeater ID="rptAttachments" runat="server">
                                <HeaderTemplate>
                                    <thead>
                                        <tr>
                                            <%--<th>
                                            #
                                        </th>--%>
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
                                        <%--<td>
                                        <%# Container.ItemIndex + 1 %>
                                    </td>--%>
                                        <td>
                                            <asp:HiddenField ID="hdnIsDelete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />
                                            <asp:HiddenField ID="hdnInclude" runat="server" Value='<%# Eval("Include") %>' />
                                            <%--<%# Eval("Name") %>--%>
                                            <asp:HyperLink ID="hlFileUrl" runat="server" Target="_blank" NavigateUrl='<%# Eval("FileWebURL") %>'
                                                Width="500px">
                                                <asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />
                                            </asp:HyperLink>
                                        </td>
                                        <td>
                                            <%--<%# Eval("Size") %>--%>
                                            <asp:Literal runat="server" ID="ltrFileSize" Text='<%# Eval("Size") %>' />
                                        </td>
                                        <td>
                                            <input type="button" id="lnkDelete" value="-" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <table id="tblMoreAttachments" runat="server">
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
                    <div class="form-elements">
                        <%--<input type="button" class="BtnPlus submit-button" value="Add More" />--%>
                        <asp:FileUpload ID="fuAttachment" runat="server" Visible="false" />
                    </div>
                </div>

                <div id="tabs-letter">
                    <%--<asp:HyperLink ID="hlLetter" runat="server" CssClass="link-letter-tab" NavigateUrl="~/TemplateWordEditor.aspx?TemplateId=0" Target="_blank">Manage Word Template</asp:HyperLink>--%>
                    <asp:TextBox ID="txtTemplateLetter" runat="server" name="header" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="formbuttons">
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="submit-button processingMail"
                ValidationGroup="Template" OnClick="btnSend_Click" />
            <asp:Button ID="btnDraft" runat="server" Text="Draft" CssClass="draft-button processingMail"
                OnClick="btnDraft_Click" />
            <asp:Button ID="btnPreview" runat="server" Text="Preview" CssClass="submit-button"
                OnClick="btnPreview_Click" Visible="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="reset-button" OnClick="btnCancel_Click" />
        </div>
    </div>
    <div id="previewEmail" style="display: none">
        <div class="formdata">
            <div class="form-elm">
                <label>
                    From:</label>
                <span>
                    <asp:Literal ID="ltrFrom" runat="server"></asp:Literal></span>
            </div>
            <div class="form-elm">
                <label>
                    To:</label>
                <span>
                    <asp:Literal ID="ltrTo" runat="server"></asp:Literal></span>
            </div>
            <div class="form-elm">
                <label>
                    Cc:</label>
                <span>
                    <asp:Literal ID="ltrCc" runat="server"></asp:Literal></span>
            </div>
            <div class="form-elm">
                <label>
                    Bcc:</label>
                <span>
                    <asp:Literal ID="ltrBcc" runat="server"></asp:Literal></span>
            </div>
            <div class="form-elm">
                <label>
                    Subject:</label>
                <span>
                    <asp:Literal ID="ltrSubject" runat="server"></asp:Literal></span>
            </div>
            <div class="form-elm">
                <label>
                    Body:</label>
                <span>
                    <asp:Literal ID="ltrBody" runat="server"></asp:Literal></span>
            </div>
        </div>
    </div>
    <div id="dvPromptAttachment" style="display: none">
        <div class="formdata">
            <asp:Panel ID="pnlsearch" runat="server" DefaultButton="btnSearch">
                <div id="search">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter search keyword"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear" />
                </div>
            </asp:Panel>
            <table class="promptAttachments">
                <asp:Repeater ID="rptPromptAttachments" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th style="width: 20%">&nbsp;
                                </th>
                                <th style="width: 60%">Name
                                </th>
                                <th style="width: 20%">Size
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkFile" runat="server" CssClass="chkPrompt" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hdnFileUrl" runat="server" Value='<%# Eval("FileURL") %>' />
                                <asp:HiddenField ID="hdnInclude" runat="server" Value="0" />
                                <asp:HyperLink ID="hlFileUrl" runat="server" Target="_blank" NavigateUrl='<%# Eval("FileWebURL") %>'
                                    Width="500px">
                                    <asp:Literal runat="server" ID="ltrFileName" Text='<%# Eval("Name") %>' />
                                </asp:HyperLink>
                            </td>
                            <td class="fileSize">
                                <asp:Literal runat="server" ID="ltrFileSize" Text='<%# Eval("Size") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphScript" runat="Server">
    <script src="//code.jquery.com/ui/1.11.1/jquery-ui.js"></script>
    <script src="//cdn.ckeditor.com/4.4.3/full/ckeditor.js"></script>
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
	            '/',
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
	            '/',
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


        CKEDITOR.on('instanceReady', function (ev) {
            <%--ev.editor.setData('<span style="font-family:Calibri;">' + $('#<%= txtTemplateBody.ClientID %>').val() + '</span>');
            ev.editor.setData('<span style="font-family:Calibri;">' + $('#<%= txtTemplateLetter.ClientID %>').val() + '</span>');--%>
        });

        
        //        CKEDITOR.on('instanceReady', function (e) {
        //            e.editor.dataProcessor.htmlFilter.addRules({
        //                elements: {
        //                    p: function (e) { e.attributes.style = 'font-size:16px; line-height:17px; font-family:calibri;'; }
        //                }
        //            });
        //        });

        $(document).ready(function () {
            $("#tabs").tabs();



            $("#tabs").bind("tabsactivate", function (event, ui) {
                //if (ui.newTab.index() == 2) {
                //    var url = $(".link-letter-tab").prop('href');
                //    window.open(url, "_blank", "fullscreen=yes,toolbar=no,scrollbars=yes,resizable=no");
                //    $("#tabs").tabs({ active: 0 });
                //}
            });

            if ($(".check-Letter input[type='checkbox']").is(':checked')) {
                $(".link-letter").show();
            }
            else {
                $(".link-letter").hide();
            }

            //$(".check-Letter").click(function () {
            //    $(".link-letter").toggle();
            //});

            //$(".link-letter").click(function () {
            //    var url = $(this).prop('href');
            //    window.open(url, "_blank", "fullscreen=yes,toolbar=no,scrollbars=yes,resizable=no");
            //    return false;
            //});

            // attache files dynamically
            $("[id*=tblAttachments] [id*=lnkDelete]").click(function () {
                if (confirm("Do you want to remove this file?")) {
                    //Determine the GridView row within whose LinkButton was clicked.
                    var row = $(this).closest("tr");
                    row.attr("style", "display:none").find("input:hidden").val("1");

                }
                return false;
            });


            $(".BtnPlus").on("click", addRow);

            $("#<%= tblMoreAttachments.ClientID %>").on("click", ".BtnMinus", function () {
                if (confirm("Do you want to remove this file?")) {
                    var par = $(this).parent().parent();
                    par.remove();
                }
            });


            $(".processingMail").click(function () {
                $(this).val('Please wait...');
            });

            //            $("#<%= btnSend.ClientID %>").click(function () {
            //                if ($("#<%= txtTo.ClientID %>").val() == "") {
            //                    alert('Please enter To address first');
            //                    return false;
            //                }
            //            });

            if ($("#<%= hdnShowPromptDialog.ClientID %>").val() == "1") {
                $("#dvPromptAttachment").dialog({
                    title: "Select Attachments",
                    width: 750,
                    height: 450,
                    modal: true,
                    buttons: {
                        Close: function () {
                            $(this).dialog('close');
                        }
                    },
                    beforeClose: function (event, ui) {
                        var fileUrls = '', fileNames = '';

                        $("#dvPromptAttachment tbody tr").each(function () {
                            var checkbox = $(this).find(".chkPrompt input[type='checkbox']");
                            if ((checkbox).is(":checked")) {
                                var fileUrl = $(this).find("input[id*='hdnFileUrl']").val();
                                fileUrls = fileUrls + fileUrl + "$";

                                var fileName = $(this).find("a[id*='hlFileUrl']").text();
                                fileNames = fileNames + fileName + "$";

                                var fileNameWithUrl = $(this).find("a[id*='hlFileUrl']").attr("href");
                                var fileSize = $(this).find(".fileSize").text();
                                addFileRow(fileName, fileSize, fileNameWithUrl);
                            }
                        });

                        if (fileUrls != '') {
                            fileUrls = fileUrls.slice(0, -1);
                            $("#<%= hdnPromptFileUrl.ClientID %>").val(fileUrls);
                        }
                        if (fileNames != '') {
                            fileNames = fileNames.slice(0, -1);
                            $("#<%= hdnPromptFileName.ClientID %>").val(fileNames);
                        }
                    }
                });
            }

            $("#<%= btnSearch.ClientID %>").click(function () {
                var searchKeyword = $("#<%= txtSearch.ClientID %>").val().toLowerCase();
                $(".promptAttachments").find("a[id*='hlFileUrl']").each(function () {
                    if ($(this).text().toLowerCase().indexOf(searchKeyword) >= 0) {
                        $(this).closest("tr").show();
                    }
                    else {
                        $(this).closest("tr").hide();
                    }
                });
            });

            $("#<%= btnClear.ClientID %>").click(function () {
                $("#<%= txtSearch.ClientID %>").val('');
                $(".promptAttachments tr").show();
            });


            //            $(".chkPrompt input[type='checkbox']").change(function () {
            //                var includeFile = $(this).closest("tr").find("input[id*='hdnInclude']");
            //                var fileUrl = $(this).closest("tr").find("input[id*='hdnFileUrl']");
            //                var fileName = $(this).closest("tr").find("input[id*='hlFileUrl']");

            //                if ($(this).is(":checked")) {
            //                    includeFile.val("1");
            //                }
            //                else {
            //                    includeFile.val("0");
            //                }
            //            });
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

        function addFileRow(fileName, fileSize, fileUrl) {
            var html = '<tr>';
            if (fileUrl == '') {
                html = html + '<td style="width: 60%">' + fileName + '</td>';
            }
            else {
                html = html + '<td style="width: 60%">' + "<a href='" + fileUrl + "' target='_blank' style='width:500px;'>" + fileName + '</a></td>';
            }

            html = html + '<td style="width: 20%">' + fileSize + '</td>' +
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

                //                elem.parent().next().html(size); //add size
                //                elem.parent().nextAll().eq(1).find(".BtnMinus").css("display", "block"); //show delete button
                //                elem.css("display", "none");
                //                addRow(); //show next row

                //Upload attached file
                var fileData = evt.files[i]; // Getting the properties of file from file field
                var formData = new window.FormData(); // Creating object of FormData class
                formData.append("file", fileData); // Appending parameter named file with properties of file_field to form_data

                $.ajax({
                    url: '<%= URLRewrite.BasePath()%>/Handlers/AttachmentUpload.ashx',
                    data: formData,
                    processData: false,
                    contentType: false,
                    async: false,
                    type: 'POST',
                    success: function (data) {
                        //elem.parent().append("<a href='" + data + "' target='_blank' style='width:500px;'>" + evt.files[i].name + "</a>"); //show file name with url
                        addFileRow(fileData.name, size, data); //show file name with url
                        elem.closest("tr").hide();
                        //console.log(data);
                    },
                    error: function (errorData) {
                        //elem.parent().append(evt.files[0].name); //show file name without url
                        addFileRow(fileData.name, size, ''); //show file name without url
                    }
                });
            }
            addRow(); //show next row

        }

        function showmodalpopup() {
            $("#previewEmail").dialog({
                title: "Email Preview",
                width: 750,
                height: 450,
                modal: true,
                buttons: {
                    Send: function () {
                        __doPostBack("<%= btnPreview.ClientID %>", '');
                        $(this).dialog("close");
                    },
                    Close: function () {
                        $(this).dialog('close');
                    }
                }
            });
        };
    </script>
    <asp:HiddenField ID="hdnTemplateId" runat="server" />
    <asp:HiddenField ID="hdnEmailId" runat="server" />
    <asp:HiddenField ID="hdnCurrentUser" runat="server" />
    <asp:HiddenField ID="hdnAction" runat="server" />
    <asp:HiddenField ID="hdnPatientNumber" runat="server" />
    <asp:HiddenField ID="hdnPatientRecId" runat="server" />
    <asp:HiddenField ID="hdnPatientFullName" runat="server" />
    <asp:HiddenField ID="hdnbusinessFilter" runat="server" />
    <asp:HiddenField ID="hdnbusinessInclude" runat="server" />
    <asp:HiddenField ID="hdnhasBusinessAttachment" runat="server" />
    <asp:HiddenField ID="hdnselectAllAttachments" runat="server" />
    <asp:HiddenField ID="hdnstorePatientFile" runat="server" />
    <asp:HiddenField ID="hdnSendSMS" runat="server" />
    <asp:HiddenField ID="hdnSendUnencryptedFile" runat="server" />
    <%--<asp:HiddenField ID="hdnAttachingMore" runat="server" />--%>
    <%--<asp:HiddenField ID="hdnUserDirectory" runat="server" />--%>
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnTo" runat="server" />
    <asp:HiddenField ID="hdnCc" runat="server" />
    <asp:HiddenField ID="hdnBcc" runat="server" />
    <asp:HiddenField ID="hdnSubject" runat="server" />
    <asp:HiddenField ID="hdnBody" runat="server" />
    <asp:HiddenField ID="hdnLetter" runat="server" />
    
    <asp:HiddenField ID="hdnPatientFileName" runat="server" />
    <asp:HiddenField ID="hdnAttachmentCategory" runat="server" />
    <asp:HiddenField ID="hdnAttachmentDescription" runat="server" />

    <asp:HiddenField ID="hdnAttachURLs" runat="server" />
    <asp:HiddenField ID="hdnShowPromptDialog" runat="server" Value="0" />
    <asp:HiddenField ID="hdnPromptFileUrl" runat="server" Value="" />
    <asp:HiddenField ID="hdnPromptFileName" runat="server" Value="" />
</asp:Content>
