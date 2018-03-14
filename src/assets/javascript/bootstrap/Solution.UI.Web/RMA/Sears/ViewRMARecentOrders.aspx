<%@ Page Title="" Language="C#" MasterPageFile="~/RMA/Sears/RMASears.Master" AutoEventWireup="true" CodeBehind="ViewRMARecentOrders.aspx.cs" Inherits="Solution.UI.Web.RMA.Sears.ViewRMARecentOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <script type="text/javascript">
        function ShowModelForNotification(id) {
            //document.getElementById('header-part').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '670px';
            document.getElementById('frmdisplay1').width = '670px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:670px;height:670px;background: none repeat scroll 0 0 rgba(3, 3, 3, 0.3) !important; padding: 10px !important;border:none !important;");
            document.getElementById('popupContact1').style.width = '670px';
            document.getElementById('popupContact1').style.height = '670px';
            // window.scrollTo(0, 0);
            //document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
            centerPopup1();
            loadPopup1();
        }
    </script>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal><a id="lnkBack" runat="server"
                    href="/RMA/Sears/ReturnItem.aspx" style="color: #B92127; text-decoration: underline; float: right; margin-top: 5px;"
                    title="BACK">
                    <img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a></span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <div class="static-main-box">
                <asp:Label ID="lblMsg" runat="server" Style="color: #B92127;"></asp:Label>
                <div class="paging-top" style="width: 100%; margin-bottom: 5px; font-size: 12px; text-align: right;"
                    id="paging" runat="server">
                    <p class="numbering" style="text-align: right;">
                        <asp:Literal ID="litPagesTop" runat="server"></asp:Literal>
                    </p>
                </div>
                <asp:Label ID="lblTable" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="blanket" style="display: none;">
    </div>
    <div id="popUpDiv" style="display: none; position: absolute;">
        <div style="background-color: #cccccc; margin-top: 8px; padding-right: 3px; padding-top: 4px;">
            <a href="javascript:void(0)" onclick="popup('popUpDiv')" style="float: right; margin-right: 3px;">
                <img src="/images/cancel.png" id="closeimage" />
            </a>
        </div>
    </div>
    <div id="popupContact1" style="z-index: 1000001; top: 30px; padding: 0px; width: 750px;">
        <div style="background: none repeat scroll 0 0 white;">
            <table border="0" cellspacing="0" cellpadding="0" class="table_border">
                <tr>
                    <td align="left">
                        <iframe id="frmdisplay1" frameborder="0" height="650" width="580" scrolling="auto"></iframe>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>