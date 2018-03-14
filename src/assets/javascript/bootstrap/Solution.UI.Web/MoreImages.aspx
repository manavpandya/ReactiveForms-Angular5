<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoreImages.aspx.cs" Inherits="Solution.UI.Web.MoreImages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/css/popup_gallery.css" />
    <script type="text/javascript" src="/js/jquery-main.js"></script>
    <script type="text/javascript" src="/js/tutorial.js"></script>
</head>
<body style="background: none; padding: 10px 0;">
   <%-- <div style="float: right; padding-right: 10px; vertical-align: top;position:absolute;top:-20px;right:-20px;">
        <a href="javascript:void(0);" onclick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
            title="">
            <img src="/images/remove-minicart.png" alt="" /></a></div>--%>


    <div id="divMain" runat="Server">
        <%-- <div id="wrapper" style="border:solid 1px #c8c8c8;margin-top:30px;">
            <div class="close">
                </div>
            <h1>
                 
            </h1>
            <div id="img">
                <asp:Literal ID="ltimg" runat="server" Visible="false"></asp:Literal>
            </div>
            <ul id="gallery" class="jcarousel-skin-tango">
                <%= strMoreImg.ToString()%>
            </ul>
        </div> --%>
        <div id="wrapper" style="border: solid 1px #c8c8c8;">
            <ul id="gallery">
                <%= strMoreImg.ToString()%>
            </ul>
        </div>
    </div>
    <div id="popupoverlay">
        <div style="position: absolute; left: 0pt; top: 0pt; width: 100%; height: 100%; background: url(/images/ajax-loader.gif) no-repeat center;
            z-index: 10001;" id="lodingid" runat="server">
        </div>
    </div>
    <script type="text/javascript">

        window.onload = pgcomplateload;
        function wnloaddisplay() {
            document.getElementById("popupoverlay").style.display = 'none';
        }
        function pgcomplateload() {


            if (navigator.appName.indexOf('Explorer') > -1) {
                var t = setTimeout("wnloaddisplay()", 4000);
            }
            else {
                wnloaddisplay();
            }
        }
    
    </script>
</body>
</html>
