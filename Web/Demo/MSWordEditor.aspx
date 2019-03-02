<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="MSWordEditor.aspx.cs" Inherits="Demo_MSWordEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cke_chrome {
            margin-left: 10px !important;
            width: 97% !important;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" />
</asp:Content>

<%@ Register Assembly="TXTextControl.Web, Version=24.0.400.500, Culture=neutral, PublicKeyToken=6b83fe9a75cfb638" Namespace="TXTextControl.Web" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>


    <br />
        <br />
        <div class="show-editor" style="color: black;">Show Editor</div>
        <br />
        <br />
        <div style="height: 0px; display: none;" class="word-editor">
            <cc1:TextControl Style="border-bottom: 1px solid #999999;" ID="TextControl1" runat="server" Dock="Fill" />
        </div>

    <div class="body-editor">
        <div id="tabs">
            <ul>
                <li><a href="#tabs-body">Body</a></li>
                <li><a href="#tabs-attachment">Attachment</a></li>
                <li><a href="#tabs-letter">Letter</a></li>
            </ul>
            <div id="tabs-body">
                <div class="form-elements">
                    <asp:TextBox ID="txtTemplateBody" runat="server" name="header" TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
                </div>
            </div>
            <div id="tabs-attachment">
            </div>
            <div id="tabs-letter">
            </div>
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
            $("#tabs").tabs();
            $("#tabs").bind("tabsactivate", function (event, ui) {
                if (ui.newTab.index() == 1) {

                }
            });

            var tableName = "";
            var activeElmt = "", isInput = false;


            $(".show-editor").click(function () {
                $(".word-editor").height('600').show();
            });

            GetTableColumns(tableName);


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

            $(".formdata").delegate("input[type='text']", "focus", function () {
                activeElmt = $(this);
                isInput = true;
            });


            $(document).on('click', '.placeholder li', function (e) { //$(".placeholder li").click(function () {
                if (activeElmt != "") {
                    if ($(this).closest("ul").hasClass("tagSelector")) {
                        tableName = "";
                        if (isInput) {
                            activeElmt.val(activeElmt.val() + " [field: " + tableName + "." + $(this).text() + "]");
                        }
                        else {
                            CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[field: " + tableName + "." + $(this).text() + "]");
                        }
                    }
                    else if ($(this).closest("ul").hasClass("functionSelector")) {
                        if (isInput) {
                            activeElmt.val(activeElmt.val() + " [function: " + $(this).text() + "]");
                        }
                        else {
                            CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[function: " + $(this).text() + "]");
                        }
                    }
                    else if ($(this).closest("ul").hasClass("querySelector")) {
                        if (!isInput) {
                            CKEDITOR.instances['cphBody_txtTemplateBody'].insertHtml("[query: " + $(this).text() + "]");
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








        });



    </script>
</asp:Content>

