<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateWordEditor.aspx.cs" Inherits="TemplateWordEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/style.css" media="screen" />

    <%@ Register Assembly="TXTextControl.Web, Version=24.0.400.500, Culture=neutral, PublicKeyToken=6b83fe9a75cfb638" Namespace="TXTextControl.Web" TagPrefix="cc1" %>
</head>
<body style="background-color: white;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>


        <div class="formdata" style="width: 73%; float: left;">
            <div style="height: 1000px; border: 1px solid #ccc;" class="sample">
                <cc1:TextControl Style="border-bottom: 1px solid #999999;" ID="TextControl1" runat="server" Dock="Fill" />
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
        </div>
        <br />
        <div style="float:left;">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="submit-button" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel reset-button" />
        </div>
    </form>
    <script type="text/javascript" src="<%= URLRewrite.BasePath()%>/js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            if (window.location.href.indexOf("page=mail") > -1) {
                $(".tagSections").css('visibility', 'hidden');
            }

            $(".cancel").click(function () {
                if (confirm('Are you sure you would like to exit out of the letter')) {
                    window.close();
                }
                else {
                    return false;
                }
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



            $(document).on('click', '.placeholder li', function (e) {
                var html = "";

                if ($(this).closest("ul").hasClass("tagSelector")) {
                    tableName = $("#<%= ddlTagSelectorTables.ClientID %> option:selected").val();
                        html = "[field: " + tableName + "." + $(this).text() + "] ";
                    }
                    else if ($(this).closest("ul").hasClass("functionSelector")) {
                        html = "[function: " + $(this).text() + "] ";
                    }
                    else if ($(this).closest("ul").hasClass("querySelector")) {
                        html = "[query: " + $(this).text() + "] ";
                    }

                var sel = TXTextControl.selection
                sel.getBounds(function (e) {
                    //console.log(e);

                    var bounds = { "start": e.start, "length": 0 }
                    sel.setBounds(bounds)

                    var encoded = btoa(html);
                    sel.load(TXTextControl.StreamType.PlainText, encoded);
                });
            });
        });
    </script>
</body>
</html>
