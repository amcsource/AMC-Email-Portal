<%@ Page Language="C#" AutoEventWireup="true" CodeFile="browser.aspx.cs" Inherits="Demo_browser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
//            var url = "http://www.liberty-izone.com/tools/radius/api";
//            var params = "type=1&username=sdf&password=sdf&query=%7B%22advert_pending%22%3A%22no%22%2C%22chap_challenge%22%3A%22%22%2C%22chap_id%22%3A%22%22%2C%22domain%22%3A%22%22%2C%22error%22%3A%22%22%2C%22error_orig%22%3A%22%22%2C%22host_ip%22%3A%220.0.0.0%22%2C%22identity%22%3A%22Liberty%2Btest%2BGateway%22%2C%22interface_name%22%3A%22bridge1%22%2C%22ip%22%3A%2293.190.117.106%22%2C%22link_login%22%3A%22http%3A%2F%2F93.190.117.106%2Flogin%3Fdst%22%2C%22link_logout%22%3A%22http%22%2C%22link_status%22%3A%22http%3A%2F%2F93.190.117.106%2Fstatus%22%2C%22logged_in%22%3A%22no%22%2C%22login_by%22%3A%22%22%2C%22plain_passwd%22%3A%22yes%22%2C%22popup%22%3A%22true%22%2C%22server_address%22%3A%2293.190.117.106%253A80%22%2C%22server_name%22%3A%22hotspot1%22%2C%22trial%22%3A%22yes%22%2C%22session_id%22%3A%22%22%2C%22ssl_login%22%3A%22no%22%2C%22mac%22%3A%22E0%253ACE%253AC3%253AF2%253AE4%253A33%22%2C%22link_login_only%22%3A%22http%3A%2F%2F93.190.117.106%2Flogin%22%2C%22link_orig%22%3A%22%2Bhttp%3A%2F%2Fwww.bromleycourthotel.co.uk%2Fwifi-login%2F%22%2C%22hostname%22%3A%2293.190.117.106%22%2C%22username%22%3A%22%22%2C%22pageid%22%3A%2218%22%7D";

//            if (window.XMLHttpRequest) {
//                http = new XMLHttpRequest();
//            }
//            else {
//                http = new ActiveXObject("Microsoft.XMLHTTP");
//            }

//            http.open("POST", url, true);
//            http.setRequestHeader("Content-type", "application/x-www-form-urlencoded;charset=UTF-8");
//            http.setRequestHeader("Content-length", params.length);
//            http.setRequestHeader("Connection", "close");

//            http.onreadystatechange = function () {//Call a function when the state changes.
//                //alert('in');
//                if (http.readyState == 4 && http.status == 200) {
//                    kid = http.responseText;
//                    //                    document.searchform.searchfield.value = kid;
//                    //                    document.searchform.submit()
//                    $("#response").append(kid);
//                }
//            }

            //            http.send(params);


            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0)      // If Internet Explorer, return version number
                alert(parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));
            else                 // If another browser, return 0
                alert('otherbrowser');

            return false;


        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="response"></div>
        <%--http://www.liberty-izone.com/tools/radius/api?type=1&username=sdf&password=sdf&query=%7B%22advert_pending%22%3A%22no%22%2C%22chap_challenge%22%3A%22%22%2C%22chap_id%22%3A%22%22%2C%22domain%22%3A%22%22%2C%22error%22%3A%22%22%2C%22error_orig%22%3A%22%22%2C%22host_ip%22%3A%220.0.0.0%22%2C%22identity%22%3A%22Liberty%2Btest%2BGateway%22%2C%22interface_name%22%3A%22bridge1%22%2C%22ip%22%3A%2293.190.117.106%22%2C%22link_login%22%3A%22http%3A%2F%2F93.190.117.106%2Flogin%3Fdst%22%2C%22link_logout%22%3A%22http%22%2C%22link_status%22%3A%22http%3A%2F%2F93.190.117.106%2Fstatus%22%2C%22logged_in%22%3A%22no%22%2C%22login_by%22%3A%22%22%2C%22plain_passwd%22%3A%22yes%22%2C%22popup%22%3A%22true%22%2C%22server_address%22%3A%2293.190.117.106%253A80%22%2C%22server_name%22%3A%22hotspot1%22%2C%22trial%22%3A%22yes%22%2C%22session_id%22%3A%22%22%2C%22ssl_login%22%3A%22no%22%2C%22mac%22%3A%22E0%253ACE%253AC3%253AF2%253AE4%253A33%22%2C%22link_login_only%22%3A%22http%3A%2F%2F93.190.117.106%2Flogin%22%2C%22link_orig%22%3A%22%2Bhttp%3A%2F%2Fwww.bromleycourthotel.co.uk%2Fwifi-login%2F%22%2C%22hostname%22%3A%2293.190.117.106%22%2C%22username%22%3A%22%22%2C%22pageid%22%3A%2218%22%7D--%>
    </div>
    </form>
</body>
</html>
