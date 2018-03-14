<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Orders.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.Orders" ValidateRequest="false" %>

<asp:Content ID="cntOrders" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/tabs.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function checkbacklist()
        {
            if('<%=ViewState["urlreffer"]%>' != null)
            {
                window.history.back('<%=ViewState["urlreffer"]%>');
            }
            else {
                window.location.href = '/admin/Orderlist.aspx?Storeid=1';
            }
        }
    function ConfirmDeleteDocument(strmsg,cntrlnm) {
            jConfirmDynemicButton(strmsg, 'Confirmation', 'Yes', 'No', function (r) {
                if (r == true) {
                    //document.getElementById(cntrlnm).onclick = function () { return true; };
                    //alert('ctl00$'+cntrlnm.replace(/_/g,'$'));
                    //__doPostBack('ctl00$'+cntrlnm.replace(/_/g,'$'), '');
                     document.getElementById(cntrlnm.replace('LinkButton1','LinkButton2')).click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }

        function checkemailsend()
        {
        Tabdisplay(12);
         iframeAutoheight(document.getElementById("ContentPlaceHolder1_frmfullInvoice"));
        // iframereload('ContentPlaceHolder1_frmfullInvoice');
         var ifr = document.getElementById("ContentPlaceHolder1_frmfullInvoice");
         ifr.contentWindow.SendInvoice();
            return false;
        }

        function ShowModelCredit(id) {
            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '237px';
            document.getElementById('frmdisplay1').width = '509px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:507px;height:217px;");
            document.getElementById('popupContact1').style.width = '507px';
            document.getElementById('popupContact1').style.height = '217px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
        }
        function checkfileupload() {
            if (document.getElementById('ContentPlaceHolder1_fuUplodDoc')) {
                var fup = document.getElementById('ContentPlaceHolder1_fuUplodDoc');
                var fileName = fup.value;
                if (fileName == '') {
                    jAlert('Please Select File.', 'Message');
                    return false;
                }

            }
            return true;
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function checkcomment() {
            if (document.getElementById('ContentPlaceHolder1_txtComment') != null && document.getElementById('ContentPlaceHolder1_txtComment').value == '') {
                jAlert('Please Enter Comment.', 'Message', 'ContentPlaceHolder1_txtComment');
                // document.getElementById('ContentPlaceHolder1_txtComment').focus();
                return false;
            }
            else {
                chkHeight();
            }
            return true;
        }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        function isNumberKeyCard(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function ResetCreditcard() {
            if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null) {
                document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex = 0;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlMonth') != null) {
                document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex = 0;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlYear') != null) {
                document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex = 0;
            }
            if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null) {
                document.getElementById('ContentPlaceHolder1_txtCardNumber').value = '';
            }
            if (document.getElementById('ContentPlaceHolder1_txtCSC') != null) {
                document.getElementById('ContentPlaceHolder1_txtCSC').value = '';
            }
            if (document.getElementById('ContentPlaceHolder1_txtNameOnCard') != null) {
                document.getElementById('ContentPlaceHolder1_txtNameOnCard').value = '';
            }
            return false;
        }
        function checkcreditcard() {

            var CardType = '';
            if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null) {
                CardType = document.getElementById('ContentPlaceHolder1_ddlCardType').options[document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex].text;
            }
            if (document.getElementById('ContentPlaceHolder1_txtNameOnCard') != null && document.getElementById('ContentPlaceHolder1_txtNameOnCard').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Name of Card.', 'Required Information', 'ContentPlaceHolder1_txtNameOnCard');
                // document.getElementById('ContentPlaceHolder1_txtNameOnCard').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlCardType') != null && document.getElementById('ContentPlaceHolder1_ddlCardType').selectedIndex == 0) {
                jAlert('Please Select Card Type.', 'Required Information', 'ContentPlaceHolder1_ddlCardType');
                //document.getElementById('ContentPlaceHolder1_ddlCardType').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please Enter Card Number.', 'Required Information', 'ContentPlaceHolder1_txtCardNumber');
                // document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && (/^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCardNumber').value) == false && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.indexOf('*') < -1)) {
                jAlert('Please Enter valid Numeric Card Number', 'Required Information', 'ContentPlaceHolder1_txtCardNumber');
                //document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
                return false;
            }

            else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 15) {
                jAlert('Credit Card Number must be 15 digit long', 'Required Information', 'ContentPlaceHolder1_txtCardNumber');
                //document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
                return false;
            }

            else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCardNumber') != null && document.getElementById('ContentPlaceHolder1_txtCardNumber').value != '' && document.getElementById('ContentPlaceHolder1_txtCardNumber').value.length != 16) {
                jAlert('Credit Card Number must be 16 digit long', 'Required Information', 'ContentPlaceHolder1_txtCardNumber');
                //document.getElementById('ContentPlaceHolder1_txtCardNumber').focus();
                return false;
            }

            else if (document.getElementById('ContentPlaceHolder1_ddlMonth') != null && document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex == 0) {
                jAlert('Please Select Month.', 'Required Information', 'ContentPlaceHolder1_ddlMonth');
                //document.getElementById('ContentPlaceHolder1_ddlMonth').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex == 0) {
                jAlert('Please Select Year', 'Required Information', 'ContentPlaceHolder1_ddlYear');
                //document.getElementById('ContentPlaceHolder1_ddlYear').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlYear') != null && document.getElementById('ContentPlaceHolder1_ddlMonth') != null) {
                var objDate = new Date();
                var year = document.getElementById('ContentPlaceHolder1_ddlYear').options[document.getElementById('ContentPlaceHolder1_ddlYear').selectedIndex].value;
                var month = document.getElementById('ContentPlaceHolder1_ddlMonth').options[document.getElementById('ContentPlaceHolder1_ddlMonth').selectedIndex].value;

                if ((year > objDate.getFullYear()) || (year == objDate.getFullYear() && month >= objDate.getMonth() + 1)) {

                    //                    if (document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.replace(/^\s+|\s+$/g, "") == '') {
                    //                        jAlert('Please Enter Card Verification Code.', 'Required Information', 'ContentPlaceHolder1_txtCSC');
                    //                        //document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                    //                        return false;
                    //                    }
                    //                    else 

                    if (document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.replace(/^\s+|\s+$/g, "") != '' && /^-?\d+$/.test(document.getElementById('ContentPlaceHolder1_txtCSC').value) == false) {
                        jAlert('Please enter valid Numeric Card Verification Code', 'Required Information', 'ContentPlaceHolder1_txtCSC');
                        //document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                        return false;
                    }
                    else if ((CardType.toLowerCase() == 'amex' || CardType.toLowerCase() == 'american express') && document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.length != 4 && document.getElementById('ContentPlaceHolder1_txtCSC').value != '') {
                        jAlert('Card Verification Code must be 4 digit long', 'Required Information', 'ContentPlaceHolder1_txtCSC');
                        //document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                        return false;
                    }
                    else if (CardType.toLowerCase() != 'amex' && CardType.toLowerCase() != 'american express' && document.getElementById('ContentPlaceHolder1_txtCSC') != null && document.getElementById('ContentPlaceHolder1_txtCSC').value.length != 3 && document.getElementById('ContentPlaceHolder1_txtCSC').value != '') {
                        jAlert('Card Verification Code must be 3 digit long', 'Required Information', 'ContentPlaceHolder1_txtCSC');
                        //document.getElementById('ContentPlaceHolder1_txtCSC').focus();
                        return false;
                    }

                    return true;
                }
                else {
                    jAlert('Please Enter Valid Expiration Date.', 'Required Information', 'ContentPlaceHolder1_ddlYear');
                    // document.getElementById('ContentPlaceHolder1_ddlYear').focus();
                    return false;
                }

                return true;
            }


            return true;
        }
        function ShowCreditpopup() {

            
            if ('<%=ViewState["CardType"]%>' != null && '<%=ViewState["CardType"]%>' != '') {
                document.getElementById('ContentPlaceHolder1_ddlCardType').value = '<%=ViewState["CardType"]%>';
            }
            
            if ("<%=ViewState["CardName"]%>" != null && "<%=ViewState["CardName"]%>" != '') {
                document.getElementById('ContentPlaceHolder1_txtNameOnCard').value = "<%=ViewState["CardName"]%>";
            }
            if ('<%=ViewState["CardNumber"]%>' != null && '<%=ViewState["CardNumber"]%>' != '') {
                document.getElementById('ContentPlaceHolder1_txtCardNumber').value = '<%=ViewState["CardNumber"]%>';
            }
            if ('<%=ViewState["CardMonth"]%>' != null && '<%=ViewState["CardMonth"]%>' != '') {
                document.getElementById('ContentPlaceHolder1_ddlMonth').value = '<%=ViewState["CardMonth"]%>';
            }
            if ('<%=ViewState["CardYear"]%>' != null && '<%=ViewState["CardYear"]%>' != '') {
                document.getElementById('ContentPlaceHolder1_ddlYear').value = '<%=ViewState["CardYear"]%>';
            }
            if ('<%=ViewState["CardVarificationCode"]%>' != null && '<%=ViewState["CardVarificationCode"]%>' != '') {
                document.getElementById('ContentPlaceHolder1_txtCSC').value = '<%=ViewState["CardVarificationCode"]%>';
            }

            window.scrollTo(0, 0);

            document.getElementById('btnreadmore').click();

            return false;
        }

        function ShowAddressModel(id) {



            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '400px';
            document.getElementById('frmdisplay1').width = '750px';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:750px;height:400px;");
            document.getElementById('popupContact1').style.width = '750px';
            document.getElementById('popupContact1').style.height = '400px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = '/Admin/Orders/EditorderAddress.aspx?' + id;

        }
    </script>
    <script type="text/javascript">
        function ShowDiv(imgid, divid) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
            }
        }

        function Tabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 1; i < 16; i++) {
                var divid = "divtab" + i.toString()
                var liid = "li" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('li' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }
        }

        function iframeAutoheight(iframe) {
            var height = iframe.contentWindow.document.body.scrollHeight;

            iframe.height = height + 30;
        }
        function iframereload(iframe) {
            //chkHeight();
            document.getElementById(iframe).src = document.getElementById(iframe).src;

        }

        function getColor() {
            if (document.getElementById('ContentPlaceHolder1_ddlStatus') != null) {
                //                document.getElementById('ContentPlaceHolder1_ddlColor').length = 1;
                //                document.getElementById('ContentPlaceHolder1_ddlColor').options[0] = null;

                document.getElementById('ContentPlaceHolder1_ddlColor').options[0].text = document.getElementById('ContentPlaceHolder1_ddlStatus').options[document.getElementById('ContentPlaceHolder1_ddlStatus').selectedIndex].value;
                document.getElementById('ContentPlaceHolder1_ddlColor').options[0].value = document.getElementById('ContentPlaceHolder1_ddlStatus').options[document.getElementById('ContentPlaceHolder1_ddlStatus').selectedIndex].value;

            }
            return false;
        }
    </script>
    <script type="text/javascript">

        //Functions for Update Order
        function OpenInventory(PID) {
            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));

            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            // window.open('ProductSearch.aspx?upgrade=1&PID=' + PID, "Mywindow", windowFeatures);

            window.open('ProductSearch.aspx?upgrade=1&PID=' + PID, '', windowFeatures);
        }

        function OpenInventoryForSKU(PID, SID) {
            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));

            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('ProductSearch.aspx?SKU=1&PID=' + PID + '&SID=' + SID, '', windowFeatures);
        }

        function OpenInventoryForUpgradeSKU(PID, SID, OPCID) {
            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('ProductSearch.aspx?upgrade=1&OPCID=' + OPCID + '&SID=' + SID + '&PID=' + PID, '', windowFeatures);
        }

        function OpenInventoryForUpgradeSKU(PID, SID, OPCID, ParenetProductID) {
            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('ProductSearch.aspx?upgrade=1&OPCID=' + OPCID + '&SID=' + SID + '&PID=' + PID + '&ParenetProductID=' + ParenetProductID, '', windowFeatures);
        }

        function OpenUpdateProductBrowser(StoreID, OrderedShoppingCartID) {
            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('UpdateProductBrowser.aspx?StoreID=' + StoreID + '&OSCID=' + OrderedShoppingCartID + '', '', windowFeatures);
        }

        function OpenInventoryForNormalSKU(PID, SID, OCCID) {

            var width = 700;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('ProductSearch.aspx?normal=1&OCCID=' + OCCID + '&SID=' + SID + '&PID=' + PID, '', windowFeatures);
        }

        function confirmRemove() {
            return confirm('Are you sure you want to Remove this Item from Order?');
        }

        function ReloadParent() {
            parent.location.reload();
        }

        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

    </script>
    <style type="text/css">
        body
        {
            background: url("<%="/App_Themes/" + Page.Theme.ToString() %>/images/body-bg-order.jpg") repeat-x scroll left top #FCFCFC;
        }
    </style>
    <div class="content-row1">
        <table width="100%" class="table">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" border="0" class="table" style="width: auto;">
                        <tbody>
                            <tr>
                                <td align="left" valign="middle" id="tdPrevious" runat="server">
                                    <asp:ImageButton ID="imgPrevious" runat="server" ToolTip="Previous Order" />
                                </td>
                                <td align="left" valign="middle">
                                    <strong style="color: #6A6A6A;">Order #
                                        <asp:Literal ID="ltrOrderNumber" runat="Server"></asp:Literal>
                                        <asp:Literal ID="ltrRefOrderNumber" runat="Server"></asp:Literal>
                                        <asp:Literal ID="ltrDate" runat="Server"></asp:Literal>
                                    </strong>
                                </td>
                                <td align="left" valign="middle" id="tdNext" runat="server">
                                    <asp:ImageButton ID="imgNext" runat="server" ToolTip="Next Order" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td align="right">
                    <strong style="color: #6A6A6A;">
                        <%-- <asp:Label runat="server" ID="lblStoreName" Text="StoreName" Style="float: right;
                            margin-top: 10px; padding-right: 5px;"></asp:Label>--%>
                        <asp:Literal ID="ltStoreName" runat="Server"></asp:Literal>
                    </strong>
                </td>
            </tr>
        </table>
        &nbsp;</div>
    <div class="content-row2">
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
            <tbody>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img alt="" height="5" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <div id="tab-container-1">
                            <ul class="menu">
                                <li class="active" id="li1" onclick="Tabdisplay(1);iframereload('ContentPlaceHolder1_frmUpdateOrder');">
                                    ORDER DETAIL</li>
                                <%--<li id="li2" class="" onclick="Tabdisplay(2);iframereload('ContentPlaceHolder1_frmPO');">
                                    PURCHASE ORDER</li>--%>
                                <%--<li id="li2" class="" onclick="Tabdisplay(2);chkHeight();iframereload('ContentPlaceHolder1_frmInvoice');document.getElementById('prepage').style.display = 'none';">
                                    RECEIPT</li>--%>
                                <li id="li12" class="" onclick="Tabdisplay(12);chkHeight();iframereload('ContentPlaceHolder1_frmfullInvoice');document.getElementById('prepage').style.display = 'none';">
                                    SALES ORDER</li>
                                <li id="li3" class="" onclick="Tabdisplay(3);chkHeight();iframereload('ContentPlaceHolder1_frmPackingSlip');document.getElementById('prepage').style.display = 'none';">
                                    PACKING SLIP</li>
                                <li id="li13" style="display:none;" class="" onclick="Tabdisplay(13);chkHeight();iframereload('frmPickingSlip');document.getElementById('prepage').style.display = 'none';">
                                    PICKING SLIP</li>
                                <%-- <li id="li5" class="" onclick="Tabdisplay(5);chkHeight();iframereload('ContentPlaceHolder1_frmOrderMail');document.getElementById('prepage').style.display = 'none';">
                                    ORDER EMAILS</li>--%>
                                <li id="li6" class=""  onclick="Tabdisplay(6);chkHeight();iframereload('ContentPlaceHolder1_frmShippingLabel');document.getElementById('prepage').style.display = 'none';">
                                    SHIPPING LABEL</li>
                                <li id="li7"  style="display:none;"  class="" onclick="Tabdisplay(7);chkHeight();iframereload('ContentPlaceHolder1_frmChangeOrder');document.getElementById('prepage').style.display = 'none';">
                                    CHANGE ORDER</li>
                                <li id="li11" class="" style="display: none;" onclick="Tabdisplay(11);chkHeight();iframereload('ContentPlaceHolder1_frmPurchaseOrder');document.getElementById('prepage').style.display = 'none';">
                                    PURCHASE ORDER</li>
                                <% if (IsRefund)
                                   { %>
                                <li id="li10" class="" title="Refund" onclick="Tabdisplay(10);chkHeight();iframereload('ContentPlaceHolder1_frmRefund');document.getElementById('prepage').style.display = 'none';">
                                    REFUND</li>
                                <%} %>
                                <li id="li8" class=""  style="display:none;"  onclick="Tabdisplay(8);chkHeight();iframereload('ContentPlaceHolder1_frmReguest');document.getElementById('prepage').style.display = 'none';">
                                    RETURN</li>
                              <li id="li9" class="" style="display:none;"    title="RMA Request(s)" onclick="Tabdisplay(9);chkHeight();iframereload('ContentPlaceHolder1_frmRMAReguest');document.getElementById('prepage').style.display = 'none';">
                                    RMA Request(s)</li>
                                <li id="li14" class="" onclick="Tabdisplay(14);chkHeight();iframereload('ContentPlaceHolder1_frmShipping');document.getElementById('prepage').style.display = 'none';">
                                    SHIPPING</li>
                                <li id="li15" class="" style="display: none;" onclick="Tabdisplay(15);chkHeight();iframereload('ContentPlaceHolder1_frmHaming');document.getElementById('prepage').style.display = 'none';">
                                    <a class="blink_me" style="color: #B92127;">Hemming Option</a></li>
                                <li id="li4" class="" onclick="Tabdisplay(4);chkHeight();iframereload('ContentPlaceHolder1_frmorderLog');document.getElementById('prepage').style.display = 'none';">
                                    ORDER LOG</li>
                            </ul>
                            <div style="float: right; padding-top: 10px;">
                                 <asp:ImageButton ID="btnUploadCancelOrder" runat="server" Visible="false" ToolTip="Upload Cancel Order"
                                    OnClick="btnUploadCancelOrder_Click" OnClientClick="javascript:if(confirm('Are you sure want to cancel uploaded order?')){return true;}else{ return false;}"></asp:ImageButton>
                              
                                <asp:ImageButton ID="btnUploadOrder" runat="server" Visible="false" ToolTip="Upload Order"
                                    OnClick="btnUploadOrder_Click"></asp:ImageButton>
                               <%-- <a title="Export Order" href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_btnExport').click();">
                                    <img title="Export Order" alt="Export Order" src="/App_Themes/<%=Page.Theme %>/images/export-order.png" /></a>
                                <a title="Export Product" href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_btnExportproduct').click();">
                                    <img title="Export Product" alt="Export" src="/App_Themes/<%=Page.Theme %>/images/export-product.png" /></a>--%>
                                <a title="Back" onclick="chkHeight();" href="OrderList.aspx">
                                    <img title="Back" alt="Back" src="/App_Themes/<%=Page.Theme %>/button/back.png" /></a>
                                <%--<a title="Complete Order" href="javascript:void(0);">
                                    <img title="Complete Order" alt="Complete Order" src="/App_Themes/<%=Page.Theme %>/button/complete-order.png" /></a>--%>
                                <%--<a title="Email To Customer" href="javascript:void(0);" onclick="javascript:chkHeight();document.getElementById('ContentPlaceHolder1_btnSendEmail').click();">--%>
                                <a title="Email To Customer" href="javascript:void(0);" onclick="javascript:checkemailsend();">
                                    <img title="Email To Customer" alt="Email To Customer" src="/App_Themes/<%=Page.Theme %>/button/send-email.png" /></a>
                                <a title="Hold" href="javascript:void(0);" onclick="javascript:chkHeight();document.getElementById('ContentPlaceHolder1_btnHold').click();"
                                    id="ahold" runat="server" visible="false">
                                    <img title="Hold" alt="Hold" src="/App_Themes/<%=Page.Theme %>/button/hold.png" /></a>
                            </div>
                            <span class="clear"></span>
                            <div class="tab-content general-tab" style="margin-top: 0px;" id="divtab1">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" width="40%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="border-td-sub">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/order-total.png" alt="Order Total" title="Order Total"
                                                                                                                    class="img-left" />
                                                                                                                <h2>
                                                                                                                    Order Total</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgOrderTotal','trOrderTotal');"
                                                                                                                    title="Minimize">
                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgOrderTotal" alt="Minimize"
                                                                                                                        title="Minimize" class="minimize" /></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr id="trOrderTotal">
                                                                                                        <td>
                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <td valign="top">
                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="ordertotal-table">
                                                                                                                                <tbody>
                                                                                                                                    <tr>
                                                                                                                                        <td height="15">
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td width="32%" class="order-total-first" valign="top">
                                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="86%">
                                                                                                                                                <tbody>
                                                                                                                                                    <tr>
                                                                                                                                                        <td valign="top">
                                                                                                                                                            <span>Order Total</span>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td height="5">
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <strong>
                                                                                                                                                                <asp:Literal ID="ltrOrderTotal" runat="server"></asp:Literal>
                                                                                                                                                            </strong>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            &nbsp;
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </tbody>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                        <td width="3%">
                                                                                                                                            &nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td width="32%" valign="top">
                                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="92%" class="order-total-second">
                                                                                                                                                <tbody>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <span>Order Status</span>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td height="3">
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:Literal ID="ltrorderTranStatus" runat="server"></asp:Literal>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                            <asp:Literal ID="ltrProcessingStatus" runat="server"></asp:Literal>
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </tbody>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                        <td width="35%">
                                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="81%">
                                                                                                                                                <tbody>
                                                                                                                                                    <tr id="trcapture" runat="server" visible="false">
                                                                                                                                                        <td align="right">
                                                                                                                                                            <asp:Button ID="btnCapture" runat="server" Visible="false" OnClick="btnCapture_Click"
                                                                                                                                                                OnClientClick="chkHeight();" />
                                                                                                                                                            <asp:Button ID="btnRefund" runat="server" Visible="false" OnClick="btnRefund_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr id="trrefund" runat="server" visible="false">
                                                                                                                                                        <td align="right" style="padding-top: 2px;">
                                                                                                                                                            <asp:Button ID="btnForceRefund" runat="server" Visible="false" OnClick="btnForceRefund_Click"
                                                                                                                                                                OnClientClick="chkHeight();" />
                                                                                                                                                            <br />
                                                                                                                                                            <asp:Button ID="btnCancelOrder" runat="server" Visible="false" OnClick="btnCancelOrder_Click"
                                                                                                                                                                OnClientClick="chkHeight();" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr id="trvoid" runat="server" visible="false">
                                                                                                                                                        <td align="right" style="padding-top: 2px;">
                                                                                                                                                            <asp:Button ID="btnVoid" runat="server" Visible="false" OnClick="btnVoid_Click" OnClientClick="chkHeight();" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </tbody>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </tbody>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td height="45">
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="order-total-row">
                                                                                                                        <td>
                                                                                                                            <strong>Customer Name:</strong> <a id="ahrefname" runat="server"></a><strong>&nbsp;&nbsp;&nbsp;Email:</strong>
                                                                                                                            <a id="ahrefMail" runat="server"></a>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="order-total-row">
                                                                                                                        <td>
                                                                                                                            <strong>Shipping &amp; Handling:</strong> <span>
                                                                                                                                <asp:Literal ID="ltrShipping" runat="server"></asp:Literal>
                                                                                                                            </span>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="order-total-row">
                                                                                                                        <td>
                                                                                                                            <strong>Sales Agent:</strong> <span>
                                                                                                                                <asp:Label ID="salesAgent" runat="server" Text="Label"></asp:Label>
                                                                                                                            </span>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td height="4">
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr class="order-total-row" runat="server" id="IPAddressRow">
                                                                                                                        <td>
                                                                                                                            This order was placed via ONLINE via IP Address <b>
                                                                                                                                <asp:Literal ID="ltrIP" runat="server"></asp:Literal>
                                                                                                                            </b>
                                                                                                                            <img title="" alt="" src="/App_Themes/<%=Page.Theme %>/icon/ip-address.png" />
                                                                                                                            <asp:ImageButton ID="btnaAllowIP" runat="server" AlternateText="Allow This IP" Visible="false"
                                                                                                                                OnClick="btnaAllowIP_Click" />
                                                                                                                            <asp:ImageButton ID="btnBlockIP" runat="server" AlternateText="Block This IP" Visible="false"
                                                                                                                                OnClick="btnBlockIP_Click" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td height="5">
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td align="left" width="1%">
                                                            &nbsp;
                                                        </td>
                                                        <td align="right" width="59%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="border-td-sub">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/order-notes.png" alt="Order Notes" title="Order Notes"
                                                                                                                    class="img-left">
                                                                                                                <h2>
                                                                                                                    Order Notes</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgOrderNotes','trOrderNotes');"
                                                                                                                    title="Minimize">
                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgOrderNotes" alt="Minimize"
                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr id="trOrderNotes">
                                                                                                        <td>
                                                                                                            <div id="tab-container" style="height: 185px;">
                                                                                                                <ul class="menu">
                                                                                                                    <li class="active" id="ordernotes">Order Notes</li>
                                                                                                                    <li id="privatenotes" class="">Customer Notes</li>
                                                                                                                    <li id="yearly" class="">Order Documents</li>
                                                                                                                </ul>
                                                                                                                <span class="clear"></span>
                                                                                                                <div class="tab-content order-notes" id="divOrderNote" style="display: block; height: 130px;">
                                                                                                                    <div class="tab-content-1" style="height: 121px; overflow-y: auto;">
                                                                                                                        <asp:Literal ID="ltrOrderNotes" runat="server"></asp:Literal>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="tab-content private-notes" id="divCustomerNote" style="display: none;
                                                                                                                    height: 130px;">
                                                                                                                    <div class="tab-content-1" style="height: 121px; overflow-y: auto;">
                                                                                                                        <asp:Literal ID="ltrPrivate" runat="server"></asp:Literal></div>
                                                                                                                </div>
                                                                                                                <div class="tab-content yearly" id="divUploadDoc" style="display: none; height: 130px;">
                                                                                                                    <div class="tab-content-1" style="height: 121px; overflow-y: auto;">
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr class="oddrow" id="trUploadDoc" runat="server">
                                                                                                                                <td>
                                                                                                                                </td>
                                                                                                                                <td valign="middle">
                                                                                                                                    <span>
                                                                                                                                        <asp:FileUpload ID="fuUplodDoc" runat="server" />
                                                                                                                                        <asp:ImageButton Style="vertical-align: middle" ID="btnUpload" runat="server" AlternateText="Upload"
                                                                                                                                            OnClientClick="return checkfileupload();" OnClick="btnUpload_Click" ImageUrl="~/App_Themes/Gray/images/upload.gif" />
                                                                                                                                    </span>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr class="altrow" id="trDelete" runat="server">
                                                                                                                                <td>
                                                                                                                                </td>
                                                                                                                                <td style="border: none">
                                                                                                                                    <div style="height: 80px; width: 410px; overflow-y: auto; margin-top: 5px;">
                                                                                                                                        <asp:GridView ID="grdOrderDoc" runat="server" Font-Size="11px" Style="margin-top: 0px;
                                                                                                                                            line-height: 15px" OnRowCommand="grdOrderDoc_RowCommand" AutoGenerateColumns="false"
                                                                                                                                            ShowHeader="true" CellPadding="0" CellSpacing="1" GridLines="Both" BorderColor="#d7d7d7">
                                                                                                                                            <Columns>
                                                                                                                                                <asp:TemplateField HeaderText="<b>&nbsp;Document</b>" HeaderStyle-Width="30%">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        &nbsp;<a target="_blank" style="text-decoration: none; color: Red;" href='<%=Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Order.DocumentPath") %><%=Request["Id"] %>/<%#Eval("Name") %>'>
                                                                                                                                                            <%#Eval("Name") %></a>
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                    <ItemStyle BorderStyle="None" Font-Size="12px" />
                                                                                                                                                </asp:TemplateField>
                                                                                                                                                <%-- <asp:TemplateField>
                                                                                                                                                <ItemStyle Width="2%" BorderStyle="None" />
                                                                                                                                                <ItemTemplate>
                                                                                                                                                </ItemTemplate>
                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                                <asp:TemplateField HeaderText="<b>&nbsp;Delete</b>" HeaderStyle-Width="10%">
                                                                                                                                                    <ItemTemplate>
                                                                                                                                                        &nbsp;<asp:LinkButton ID="LinkButton1" ForeColor="#D5321C" Text="Delete" runat="server"
                                                                                                                                                            CommandArgument='<%#Eval("Name") %>' OnClientClick="javascript:return ConfirmDeleteDocument('Are you sure want to delete this document?',this.id);"
                                                                                                                                                            CommandName="DeleteDoc"></asp:LinkButton>
                                                                                                                                                        <asp:Button ID="LinkButton2" Style="display: none;" runat="server" CommandArgument='<%#Eval("Name") %>'
                                                                                                                                                            CommandName="DeleteDoc" />
                                                                                                                                                    </ItemTemplate>
                                                                                                                                                    <ItemStyle BorderStyle="None" Font-Size="12px" Height="25px" />
                                                                                                                                                </asp:TemplateField>
                                                                                                                                            </Columns>
                                                                                                                                        </asp:GridView>
                                                                                                                                    </div>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10" align="left" valign="top">
                                            <img height="10" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" width="33%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="border-td-sub">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/billing-address.png" alt="Billing Address"
                                                                                                                    title="Billing Address" class="img-left">
                                                                                                                <h2>
                                                                                                                    Billing Address</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgBillingAdd','trBillingadd');"
                                                                                                                    title="Minimize">
                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgBillingAdd" alt="Minimize"
                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr id="trBillingadd">
                                                                                                        <td align="left" valign="top">
                                                                                                            <div style="min-height: 135px;">
                                                                                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="address-table">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td align="left" width="80%" valign="top">
                                                                                                                                <asp:Literal ID="ltrBillingAddress" runat="server"></asp:Literal>
                                                                                                                            </td>
                                                                                                                            <td align="center" width="20%" valign="top">
                                                                                                                                <a title="Edit" id="aeditBill" runat="server" href="javascript:void(0);">[ Edit ]</a>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" width="32%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="border-td-sub">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/shipping-address.png" alt="Shipping Address"
                                                                                                                    title="Shipping Address" class="img-left">
                                                                                                                <h2>
                                                                                                                    Shipping Address</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgShippingAdd','trShippingAdd');"
                                                                                                                    title="Minimize">
                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgShippingAdd" alt="Minimize"
                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr id="trShippingAdd">
                                                                                                        <td>
                                                                                                            <div style="min-height: 135px;">
                                                                                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="address-table">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td align="left" width="80%" valign="top">
                                                                                                                                <asp:Literal ID="ltrShippingAddress" runat="server"></asp:Literal>
                                                                                                                            </td>
                                                                                                                            <td align="center" width="20%" valign="top">
                                                                                                                                <a title="Edit" id="aeditShip" runat="server" href="javascript:void(0);">[ Edit ]</a>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                        <td align="left" width="33%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="left" valign="top" class="border-td-sub">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <th>
                                                                                                            <div class="main-title-left">
                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/payment-information.png" alt="Payment Information"
                                                                                                                    title="Payment Information" class="img-left">
                                                                                                                <h2>
                                                                                                                    Payment Information</h2>
                                                                                                            </div>
                                                                                                            <div class="main-title-right">
                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgPayment','trpayment');" title="Minimize">
                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgPayment" alt="Minimize"
                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                        </th>
                                                                                                    </tr>
                                                                                                    <tr id="trpayment">
                                                                                                        <td>
                                                                                                            <div style="min-height: 135px;">
                                                                                                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="address-table">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td align="left" width="70%" valign="top">
                                                                                                                                <asp:Literal ID="ltrPayment" runat="server"></asp:Literal>
                                                                                                                                <asp:Literal ID="lttransactionresult" runat="server"></asp:Literal>
                                                                                                                            </td>
                                                                                                                            <td align="center" width="30%" valign="top">
                                                                                                                                <a href="javascript:void(0);" onclick="ShowCreditpopup();" id="acreditcard" runat="server"
                                                                                                                                    visible="false">Enter Credit Card Details</a>
                                                                                                                                <div id="creditcarddetails" runat="server">
                                                                                                                                    <a href="javascript:void(0);" onclick="ShowModelCredit('/ADMIN/Orders/ViewDetail.aspx?Id=<%=Request.QueryString["id"] %>');"
                                                                                                                                        visible="false">[ View Detail ]</a>
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10" align="left" valign="top">
                                            <img height="10" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td class="border-td-sub">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <th>
                                                                                            <div class="main-title-left">
                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/items-ordered.png" alt="Items Ordered"
                                                                                                    title="Items Ordered" class="img-left">
                                                                                                <h2>
                                                                                                    Items Ordered</h2>
                                                                                            </div>
                                                                                            <div class="main-title-right">
                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgProductlist','trProductlist');"
                                                                                                    title="Minimize">
                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgProductlist" alt="Minimize"
                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="trProductlist">
                                                                                        <td align="center" valign="top">
                                                                                            <iframe id="frmUpdateOrder" runat="server" bordercolor="white" frameborder="0" marginheight="0"
                                                                                                marginwidth="0" width="100%" scrolling="no"></iframe>
                                                                                            <asp:Literal ID="ltrProductlist" runat="server" Visible="false"></asp:Literal>
                                                                                            <table width="100%" style="border: 0px solid rgb(204, 204, 204); padding: 5px; display: none;">
                                                                                                <tr>
                                                                                                    <td colspan="2" align="right">
                                                                                                        <asp:ImageButton ID="btnAdjustCapture" runat="server" AlternateText="Capture Order"
                                                                                                            OnClick="btnAdjustCapture_Click" CausesValidation="false" Visible="false" />
                                                                                                        <asp:ImageButton ID="btnRefund_UpdateOrder" runat="server" AlternateText="Refund Order"
                                                                                                            OnClick="btnRefund_UpdateOrder_Click" CausesValidation="false" Visible="false" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <div class="table_border">
                                                                                                            <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" CellPadding="5"
                                                                                                                CellSpacing="0" OnRowCommand="grdProducts_RowCommand" OnRowDataBound="grdProducts_RowDataBound"
                                                                                                                ShowFooter="true" Width="100%" GridLines="Both" CssClass="table-noneforOrder">
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField Visible="false">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                                                                            <asp:Label ID="lblOrderedCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                                                                                                            <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                                                                            <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <HeaderTemplate>
                                                                                                                            Product Name
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <HeaderTemplate>
                                                                                                                            SKU
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <HeaderTemplate>
                                                                                                                            <center>
                                                                                                                                Ordered
                                                                                                                                <br />
                                                                                                                                Quantity</center>
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="center" Width="9%" />
                                                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <HeaderStyle HorizontalAlign="right" />
                                                                                                                        <HeaderTemplate>
                                                                                                                            <span style="text-align: right;">Price</span>
                                                                                                                        </HeaderTemplate>
                                                                                                                        <ItemTemplate>
                                                                                                                            $<asp:Label ID="lblPrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")).ToString("f2") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <%--   <FooterTemplate>
                                                                                                                            $<asp:Label Font-Bold="true" ID="lblTotalPrice" runat="Server"></asp:Label>
                                                                                                                        </FooterTemplate>--%>
                                                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" align="right">
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10" align="left" valign="top">
                                            <img height="10" width="1" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <td align="right" width="59%" valign="top">
                                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td align="left" width="40%" valign="top">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td class="border-td-sub">
                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <th>
                                                                                                                            <div class="main-title-left">
                                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/comments-history.png" alt="Comments History"
                                                                                                                                    title="Comments History" class="img-left">
                                                                                                                                <h2>
                                                                                                                                    Comments History</h2>
                                                                                                                            </div>
                                                                                                                            <div class="main-title-right">
                                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgComments','ContentPlaceHolder1_trComments');"
                                                                                                                                    title="Minimize">
                                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgComments" alt="Minimize"
                                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr id="trComments" runat="server">
                                                                                                                        <td>
                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="comments-history-table">
                                                                                                                                <tbody>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            Add order comments
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td>
                                                                                                                                            Order Status
                                                                                                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="listmenu" AutoPostBack="true"
                                                                                                                                                onchange="return getColor();" Width="178px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                                                                                            </asp:DropDownList>
                                                                                                                                            &nbsp;&nbsp;&nbsp;Color
                                                                                                                                            <asp:DropDownList ID="ddlColor" runat="server" CssClass="listmenu" AutoPostBack="false"
                                                                                                                                                Width="200px">
                                                                                                                                            </asp:DropDownList>
                                                                                                                                        </td>
                                                                                                                                        <td align="right" colspan="2" style="padding-right: 20px;">
                                                                                                                                            <asp:Button ID="btnUpdateState" runat="server" OnClientClick="chkHeight();" ToolTip="Update order Status"
                                                                                                                                                OnClick="btnUpdateState_Click" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td colspan="3">
                                                                                                                                            Comments<br />
                                                                                                                                            <asp:TextBox ID="txtComment" runat="server" CssClass="textarea" TextMode="MultiLine"
                                                                                                                                                Height="80px" Width="55%"></asp:TextBox>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td colspan="3">
                                                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                                                                                <tbody>
                                                                                                                                                    <tr>
                                                                                                                                                        <td align="left" width="50%" valign="middle">
                                                                                                                                                            <asp:CheckBox ID="chkNotify" runat="server" Text="&nbsp;Notify Customer by Email"
                                                                                                                                                                TextAlign="Right" />&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkfrontend" runat="server"
                                                                                                                                                                    Text="&nbsp;Visible on Frontend" TextAlign="Right" />
                                                                                                                                                        </td>
                                                                                                                                                        <td align="right" width="50%" valign="top" rowspan="2">
                                                                                                                                                            <asp:Button ID="btnShareComment" OnClientClick="return checkcomment();" runat="server"
                                                                                                                                                                ToolTip="Submit Comment" OnClick="btnShareComment_Click" />
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </tbody>
                                                                                                                                            </table>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <asp:Literal ID="ltrCommentAll" runat="server"></asp:Literal>
                                                                                                                                </tbody>
                                                                                                                            </table>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                        <td align="left" width="1%">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td align="right" width="20%" valign="top">
                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td align="left" width="100%" valign="top" class="border-td">
                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                                                <tbody>
                                                                                                    <tr>
                                                                                                        <td class="border-td-sub" align="right">
                                                                                                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                                                                                                                <tbody>
                                                                                                                    <tr>
                                                                                                                        <th>
                                                                                                                            <div class="main-title-left">
                                                                                                                                <img src="/App_Themes/<%=Page.Theme %>/icon/order-totals.png" alt="Order Totals"
                                                                                                                                    title="Order Totals" class="img-left">
                                                                                                                                <h2>
                                                                                                                                    Order Totals</h2>
                                                                                                                            </div>
                                                                                                                            <div class="main-title-right">
                                                                                                                                <a href="javascript:void(0);" onclick="ShowDiv('imgOderTotals','trOrderTotals');"
                                                                                                                                    title="Minimize">
                                                                                                                                    <img src="/App_Themes/<%=Page.Theme %>/images/minimize.png" id="imgOderTotals" alt="Minimize"
                                                                                                                                        title="Minimize" class="minimize"></a></div>
                                                                                                                        </th>
                                                                                                                    </tr>
                                                                                                                    <tr id="trOrderTotals">
                                                                                                                        <td align="right" style="background: #FFFEF3;">
                                                                                                                            <table style="width: 100%; text-align: right; border-top: none;" class="order-totals-table">
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right; width: 80%;">
                                                                                                                                        Sub Total:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        Discount Amount:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblDiscount" runat="server" Text='' CssClass="font-black04"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        Shipping:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblShoppingCost" runat="server" CssClass="font-black04"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        Order Tax:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblOrderTax" runat="server" CssClass="font-black04"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        Total:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblTotal" runat="server" CssClass="font-black04"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        Adjustment Amount:
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <span>$</span><asp:Label ID="lblAdjustmentAmount" runat="server" CssClass="font-black04"></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: right;">
                                                                                                                                        <b>Final Total:</b>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <b><span>$</span><asp:Label ID="lblFinalTotal" runat="server" CssClass="font-black04"></asp:Label>
                                                                                                                                        </b>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </table>
                                                                                                                            <asp:Literal ID="ltrorderTotals" runat="server"></asp:Literal>
                                                                                                                            <br />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </tbody>
                                                                                                            </table>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--<div class="tab-content po-tab" id="divtab2" style="margin-top: 0px; display: none;">
                                <iframe id="frmPO" runat="server" frameborder="0" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>--%>
                            <div class="tab-content invoice-tab" id="divtab2" style="margin-top: 0px; display: none;">
                                <iframe id="frmInvoice" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content invoice-tab" id="divtab12" style="margin-top: 0px; display: none;">
                                <iframe id="frmfullInvoice" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content packing-slip-tab" id="divtab3" style="margin-top: 0px; display: none;">
                                <iframe id="frmPackingSlip" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content packing-slip-tab" id="divtab13" style="margin-top: 0px; display: none;">
                                <iframe id="frmPickingSlip" frameborder="0" height="600px" src='BulkPickingSlip.aspx?Ono=<%=Request["id"] %>'
                                    marginheight="0" marginwidth="0" width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-log-tab" id="divtab4" style="margin-top: 0px; display: none;">
                                <iframe id="frmorderLog" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab5" style="margin-top: 0px; display: none;">
                                <iframe id="frmOrderMail" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" height="400px" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content Shipping-Label-tab" id="divtab6" style="margin-top: 0px;
                                display: none;">
                                <iframe id="frmShippingLabel" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" style="min-height: 1000px;" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab7" style="margin-top: 0px; display: none;">
                                <iframe id="frmChangeOrder" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab10" style="margin-top: 0px; display: none;">
                                <iframe id="frmRefund" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab11" style="margin-top: 0px; display: none;">
                                <iframe id="frmPurchaseOrder" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab8" style="margin-top: 0px; display: none;">
                                <iframe id="frmReguest" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content order-mail-tab" id="divtab9" style="margin-top: 0px; display: none;">
                                <iframe id="frmRMAReguest" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content Shipping-Label-tab" id="divtab14" style="margin-top: 0px;
                                display: none;">
                                <iframe id="frmShipping" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" style="min-height: 1000px;" scrolling="auto"></iframe>
                            </div>
                            <div class="tab-content Shipping-Label-tab" id="divtab15" style="margin-top: 0px;
                                display: none;">
                                <iframe id="frmHaming" frameborder="0" runat="server" marginheight="0" marginwidth="0"
                                    width="100%" style="min-height: 1000px;" scrolling="auto"></iframe>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnSendEmail" runat="server" ToolTip="Send Email" OnClick="btnSendEmail_Click" />
        <asp:Button ID="btnExport" runat="server" ToolTip="Send Email" OnClick="btnExport_Click" />
        <asp:Button ID="btnExportproduct" runat="server" ToolTip="Send Email" OnClick="btnExportproduct_Click" />
        <asp:Button ID="btnHold" runat="server" ToolTip="Send Email" OnClick="btnHold_Click" />
        <input type="hidden" id="hdnTabid" runat="server" value="1" />
        <input type="hidden" id="hdnAmount" runat="server" value="0" />
        <input type="hidden" id="hdnrefundAmount" runat="server" value="0" />
        <input type="hidden" id="hdnOrderEmail" runat="server" value="" />
        <input type="hidden" id="hdnHamingTab" runat="server" value="0" />
    </div>
    <div style="display: none">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 750px; height: 250px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            font-size: 12px;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Credit Card Details
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="~/images/cancel.png"
                        runat="server" ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                        <tr>
                            <td valign="top" style="font-size: 11px;">
                                <span class="required-red">*</span>Name on Card
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtNameOnCard" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                                    Width="143"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 11px;">
                                <span class="required-red">*</span>Card Type
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlCardType" runat="server" CssClass="order-list" Style="width: 143px;
                                    font-size: 11px;">
                                    <asp:ListItem Text="Select Card Type" Value="Select Card Type">Select Card Type</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 11px;">
                                <span class="required-red">*</span>Card Number
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="16" Width="143" CssClass="order-textfield"
                                    Style="font-size: 11px;" onkeypress="return isNumberKeyCard(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 11px;">
                                <span class="required-red">*</span>Expiration Date
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="order-list" Style="width: 70px;
                                    font-size: 11px;">
                                    <asp:ListItem Value="0">Month</asp:ListItem>
                                    <asp:ListItem Value="1">Jan(01)</asp:ListItem>
                                    <asp:ListItem Value="2">Feb(02)</asp:ListItem>
                                    <asp:ListItem Value="3">Mar(03)</asp:ListItem>
                                    <asp:ListItem Value="4">Apr(04)</asp:ListItem>
                                    <asp:ListItem Value="5">May(05)</asp:ListItem>
                                    <asp:ListItem Value="6">June(06)</asp:ListItem>
                                    <asp:ListItem Value="7">July(07)</asp:ListItem>
                                    <asp:ListItem Value="8">Aug(08)</asp:ListItem>
                                    <asp:ListItem Value="9">Sept(09)</asp:ListItem>
                                    <asp:ListItem Value="10">Oct(10)</asp:ListItem>
                                    <asp:ListItem Value="11">Nov(11)</asp:ListItem>
                                    <asp:ListItem Value="12">Dec(12)</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlYear" runat="server" CssClass="order-list" Style="width: 70px;
                                    font-size: 11px;">
                                    <asp:ListItem Text="Year" Value="Year">Year</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 11px;">
                                <span class="required-red">*</span>Card&nbsp;security&nbsp;code&nbsp;(CSC)
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCSC" runat="server" CssClass="order-textfield" Style="width: 40px;
                                    font-size: 11px;" onkeypress="return isNumberKeyCard(event)" MaxLength="4"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="oddrow">
                            <td colspan="2">
                            </td>
                            <td colspan="2" align="left">
                                <asp:ImageButton ID="btnSave" OnClientClick="return checkcreditcard();" OnClick="btnSave_Click"
                                    runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" />&nbsp;<asp:ImageButton
                                        ID="btnReset" OnClientClick="return  ResetCreditcard();" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/reser-filter.gif" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="popupContact1" style="z-index: 1000001; width: 750px; height: 350px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="350px" width="750" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
