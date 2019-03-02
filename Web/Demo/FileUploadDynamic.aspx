﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUploadDynamic.aspx.cs"
    Inherits="Demo_FileUploadDynamic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .Field
        {
            color: black;
            border: 1px solid #FFFFFF;
            background-color: #FFCC00;
        }
        .Find
        {
            color: blue;
            font: 10px Arial;
        }
    </style>
    <script type="text/javascript">
        var Fo = new ActiveXObject("Scripting.FileSystemObject");
        var StrOut = new String();
        var FileName = new String();
        var Extention = new String();

        function FindFile(FOo) {
            alert('ff');
            var FSo = new Enumerator(FOo.Files);
            for (i = 0; !FSo.atEnd(); FSo.moveNext()) {
                if (FileName == "*" || FSo.item().name.slice(0, FSo.item().name.lastIndexOf(".")).toLowerCase().indexOf(FileName) > -1)
                    if (Extention == "*" || FSo.item().name.slice(FSo.item().name.lastIndexOf(".") + 1).toLowerCase().indexOf(Extention) > -1) {
                        //StrOut += "<tr "+ ((i%2)? "":"bgcolor=#DDAA55")  +"><td width=50%><font class=find>" + FSo.item().name + "</font></td><td width=25%><font class=find>" + FSo.item().type + "</font></td><td width=50%><font class=find>"+ String(FSo.item().size/(1024*1024)).slice(0,3) +" MB</font></td></tr>";
                        StrOut += "<tr><td colspan='3'><input type='file' name='" + FSo.item().path + "' /></td></tr>";
                        i++
                    }
            }
        }

        function Scan() {
            
            FileName = (search.value.lastIndexOf(".") > -1) ? search.value.slice(0, search.value.lastIndexOf(".")) : (search.value.length > 0) ? search.value.toLowerCase() : "*"; //Get Searched File Name
            Extention = (search.value.lastIndexOf(".") > -1) ? search.value.slice(search.value.lastIndexOf(".") + 1).toLowerCase() : "*"; // Get Searched File Extention Name
            if (path.value.length > 0 && Fo.FolderExists(path.value)) {
                StrOut = "<table border=0 width=100% cellspacing=0>"
                FindFile(Fo.GetFolder(path.value));
                outPut.innerHTML = StrOut + "</table>";
            }
            else alert("Insert Correct Path Address");
        }
    </script>
</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <div>
        <table border="0" width="100%" cellspacing="0" style="border-collapse: collapse"
            cellpadding="2">
            <tr>
                <td dir="ltr" bgcolor="#FFCC00">
                    <b><font face="Arial" size="2">Named :
    </font></b>
                </td>
                <td dir="ltr" bgcolor="#FFCC00">
                    <input size="50" type="text" id="search" name="search" class="Field">
                </td>
            </tr>
            <tr>
                <td dir="ltr" bgcolor="#FFCC00">
                    <p dir="ltr">
                        <b><font face="Arial" size="2">Path : </font></b>
                </td>
                <td bgcolor="#FFCC00">
                    <input size="50" type="text" value="C:\" id="path" name="path" class="Field">
                </td>
            </tr>
            <tr>
                <td bgcolor="#FFCC00">
                    &nbsp;
                </td>
                <td bgcolor="#FFCC00">
                    <input type="button" value="Scan" onclick="Scan()" class="Field">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" bgcolor="#FFCC00">
                    <font face="arial" size="2"><b>Search Result</b></font>
                    <hr>
                </td>
            </tr>
            <tr>
                <td colspan="2" bgcolor="#FFCC00">
                    <div id="outPut">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
