<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Demo_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" Runat="Server">
<!DOCTYPE html>
<html>
<head>
<meta charset=utf-8 />
<title>JS Bin</title>
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
  <script src="http://ckeditor.com/apps/ckeditor/4.1/ckeditor.js?mlc5o1"></script>
</head>
<body>
  <input type="button" value="Insert Text $${FIRSTNAME}" onclick="InsertText();" />
  <textarea cols="100" id="Body" name="Body" rows="18" tabindex="7" title="Enter body content..."><strong>Prasad</strong></textarea>
  <script type="text/javascript" language="javascript">
//      $(function () {
//          if (CKEDITOR.instances['Body']) {
//              delete CKEDITOR.instances['Body'];
//          }
          CKEDITOR.config.height = '500';
          CKEDITOR.config.width = '500';
          CKEDITOR.config.autoGrow_maxHeight = '500';
          CKEDITOR.config.enterMode = CKEDITOR.ENTER_BR;
          CKEDITOR.config.shiftEnterMode = CKEDITOR.ENTER_P;
          CKEDITOR.config.scayt_autoStartup = false;
          CKEDITOR.replace('Body',
    {
        uiColor: '#fdd1ad',
        toolbar:
        [
            ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print', 'Scayt'],
            ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
            '/',
            ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
            ['Link', 'Unlink', 'Anchor'],
            ['Image', 'Flash', 'MediaEmbed', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
            '/',
            ['Styles', 'Format', 'Font', 'FontSize'],
            ['TextColor', 'BGColor'],
            ['Maximize', 'ShowBlocks']
        ]
    });
      //});

      function InsertText() {
          CKEDITOR.instances.Body.insertHtml('<b>$${FIRSTNAME}</b>');
      }
  </script>
</body>
</html>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphScript" Runat="Server">
</asp:Content>

