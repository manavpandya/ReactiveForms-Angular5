<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="swatchmessage.aspx.cs" Inherits="Solution.UI.Web.swatchmessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta name="robots" content="noindex, nofollow" />
    <link href="/css/style.css?6566" rel="stylesheet" type="text/css" />
     <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript">
        window.onload = ShowSwatchMessage;
        
        function ShowSwatchMessage() {

            $.ajax(
                        {
                            type: "POST",
                            url: "/TestMail.aspx/GeLimitMessage",
                            data: "{PId: 1}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "true",
                            cache: "false",
                            success: function (msg) {

                                

                                document.getElementById("divmsggg").innerHTML = msg.d;
                                // $('#diestimatedate').attr('style', 'display:block;');

                            },
                            Error: function (x, e) {
                            }
                        });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:220px;" id="divmsggg">
    
    </div>
    </form>
</body>
</html>
