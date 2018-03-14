<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="comparetable.aspx.cs" Inherits="Solution.UI.Web.comparetable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script src="/js/popup.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/css/general.css" type="text/css" media="screen" />
    <script type="text/javascript">
        function ShowModelForCompareProduct(id) {
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '550px';
            document.getElementById('frmdisplay1').width = '1080px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:1080px;height:550px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '1080px';
            document.getElementById('popupContact1').style.height = '550px';
            window.scrollTo(0, 0);
            document.getElementById('frmdisplay1').src = id;
            centerPopup1();
            loadPopup1();
        }
    </script>
    <style type="text/css">
        #blanket
        {
            background-color: #111;
            opacity: 0.65;
            filter: alpha(opacity=65);
            position: absolute;
            z-index: 9001;
            top: 0px;
            left: 0px;
            width: 100%;
        }
        #popUpDiv
        {
            position: absolute;
            width: 320px;
            height: 390px;
            z-index: 9002;
        }
        #backgroundPopup
        {
            display: none;
            position: fixed;
            _position: absolute;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background: #000000;
            border: 1px solid #cecece;
            z-index: 1;
        }
        #popupContact
        {
            display: none;
            position: fixed;
            _position: absolute;
            width: 269px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 2;
            padding: 12px;
            font-size: 13px;
        }
        #popupContact h1
        {
            text-align: left;
            color: #6FA5FD;
            font-size: 22px;
            font-weight: 700;
            border-bottom: 1px dotted #D3D3D3;
            padding-bottom: 2px;
            margin-bottom: 20px;
        }
        #popupContactClose
        {
            font-size: 14px;
            line-height: 14px;
            right: 6px;
            top: 4px;
            position: absolute;
            color: #6fa5fd;
            font-weight: 700;
            display: block;
        }
        #button
        {
            text-align: center;
            margin: 100px;
        }
        #btnreadmore
        {
            text-align: center;
            margin: 100px;
        }
        #btnhelpdescri
        {
            text-align: center;
            margin: 100px;
        }
        #popupContact1
        {
            display: none;
            _position: absolute;
            width: 750px;
            background: #FFFFFF;
            border: 2px solid #cecece;
            z-index: 2;
            padding: 12px;
            font-size: 13px;
        }
        #popupContact1 h1
        {
            text-align: left;
            color: #6FA5FD;
            font-size: 22px;
            font-weight: 700;
            border-bottom: 1px dotted #D3D3D3;
            padding-bottom: 2px;
            margin-bottom: 20px;
        }
        #popupContactClose1
        {
            font-size: 14px;
            line-height: 14px;
            right: 6px;
            top: 4px;
            position: absolute;
            color: #6fa5fd;
            font-weight: 700;
            display: block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="comparison-main" style="display: block;" id="comare_div" runat="server"
        visible="false">
        <div class="comparison-bg">
            <div style="padding: 0;" class="comparison-row">
                <h3>
                    Product compare list</h3>
                <a title="" href="javascript:void(0);" onclick="ShowModelForCompareProduct('/ProductCompare.aspx?p=ProductCompareDescription');"
                    runat="server" id="btncompare" style="float: right; margin-left: 10px;">
                    <img class="img-right" title="" style="margin-right: 3px;" alt="" src="/images/compare-now.png"></a>
                <a href="javascript:void(0);" onclick="DeleteProductData(-1,-1);" style="color: #B92127;
                    font-size: 11px; font-weight: bold; float: right; vertical-align: bottom; right: 100">
                    [RESET]</a>
            </div>
            <div id="compare_result">
                <div class="comparison-row">
                    <asp:Literal ID="ltrProduct" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <div id="popupContact1" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style='float: left; margin: 5px; background-color: transparent;  
            left: -19px; top: -23px; position: absolute;'>
            <img src='/images/popupclose.png' style='cursor: pointer;' onclick='javascript:disablePopup();'>
        </div>
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </form>
</body>
</html>
