<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice_Sendmail.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.Invoice_Sendmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <script type="text/javascript" src="/js/jquery-1.3.2.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <style type="text/css">
        .datatable table {
            border: 1px solid #eeeeee;
        }

        .datatable tr.alter_row {
            background-color: #f9f9f9;
        }

        .datatable td {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }

        .datatable th {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }

        .receiptfont {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }

        .receiptlineheight {
            height: 15px;
        }

        .popup_cantain {
            font-size: 11px;
            font-family: Arial, Helvetica, sans-serif;
            color: #4C4C4C;
            text-decoration: none;
        }

            .popup_cantain a {
                font-size: 11px;
                font-family: Arial, Helvetica, sans-serif;
                color: #FE0000;
                text-decoration: none;
            }

                .popup_cantain a:hover {
                    font-size: 11px;
                    font-family: Arial, Helvetica, sans-serif;
                    color: #000;
                    text-decoration: underline;
                }

        .Printinvoice {
        }
    </style>
    <style type="text/css" media="print">
        .Printinvoice {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function SendInvoice() {
            if (document.getElementById('txtCustEmail') != null) {
                document.getElementById('txtCustEmail').value = window.parent.document.getElementById('ContentPlaceHolder1_ahrefMail').innerHTML;
                document.getElementById('hdmemail').value = window.parent.document.getElementById('ContentPlaceHolder1_ahrefMail').innerHTML;
                document.getElementById('txtCustEmail').focus();
                document.getElementById('hdntabid').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnTabid').value;
            }
            //$(window).scrollTop(0, 0);


            document.getElementById('btnreadmore').click();
            $('#popupContact').css('top', '0');
            //$('html, body').animate({ scrollTop: $('#popupContact').offset().top }, 'slow');
            return false;
        }
        function SendInvoiceValidation() {
            if (document.getElementById('txtCustEmail') == null && document.getElementById('txtCustEmail').value == '') {
                alert('Please enter Valid Email');
                document.getElementById('txtCustEmail').focus();
                return false;
            }
            return true;

        }
        //        function SendInvoice() {
        //            window.parent.chkHeight();
        //            document.getElementById('hdmemail').value = window.parent.document.getElementById('ContentPlaceHolder1_ahrefMail').innerHTML;
        //            document.getElementById('hdntabid').value = window.parent.document.getElementById('ContentPlaceHolder1_hdnTabid').value;
        //            document.getElementById('btnInvoice').click();
        //        }
    </script>
</head>
<body style="background: none; font-family: Arial, Helvetica, sans-serif; font-size: 11px; color: #2A2A2A;">
    <form id="form1" runat="server">
        <div class="body12">
            <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
            <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
            <table width="100%" cellspacing="0" cellpadding="0" align="center" class="table">
                <tbody>
                    <tr>
                        <td align="center" class="receiptfont" width="100%">
                            <div align="center" class="bkground123">
                                <table width="100%" cellspacing="0" cellpadding="0" align="center" class="popup_cantain">
                                    <tbody>
                                        <tr>
                                            <td style="height: 14px"></td>
                                        </tr>
                                        <tr class="Printinvoice">
                                            <td valign="middle" align="right">
                                                <a title="Print Invoice" href="javascript:window.print();">
                                                    <img title="Print Invoice" alt="Print Invoice" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>&nbsp;&nbsp;
                                          <%--  <a title="Send Invoice To Customer" href="javascript:void(0);" onclick="javascript:SendInvoice();">
                                                <img title="Send Invoice To Customer" alt="Send Invoice To Customer" src="<%=Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Live_Server") %>/App_Themes/<%=Page.Theme %>/button/send-invoice.png" /></a>--%>
                                            </td>
                                        </tr>
                                        <tr align="center" style="height: 85px">
                                            <td valign="middle" align="center" rowspan="1">
                                                <img alt="logo" border="0" runat="server" class="img_left" id="ImgStoreLogo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="center" rowspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left" class="receiptlineheight" style="height: 22px" rowspan="1">Dear <b><span id="lblName">
                                                <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                                            </span>,</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left" class="receiptlineheight" style="height: 20px" rowspan="1">Your order has been received. Thank you for shopping with
                                            <%=Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreName").ToString() %>.
                                            <br />
                                                We appreciate your business and are committed to deliver excellent customer care.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="center" style="height: 20px" rowspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="center" style="height: 20px" rowspan="1"></td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">Order Number:&nbsp; <b><span id="lblOrderId">
                                                            <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                        <td rowspan="5" align="center">
                                                            <img id="imgOrderBarcode" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trRefOrderNo" runat="server" visible="false">
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                                            <asp:Literal ID="ltrstore" runat="server" Text="StoreName"></asp:Literal>
                                                            : <b><span id="lblRef">
                                                                <asp:Literal ID="ltrRef" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">Order Date:&nbsp; <b><span id="lblDate">
                                                            <asp:Literal ID="ltrOrderdate" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                                            <span id="lblDelMethod">Shipping Method:</span> <b><span id="lblDeliveryMethod">
                                                                <asp:Literal ID="ltrshippingMethod" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">Payment Method: <b><span id="lblPaymentMethod">
                                                            <asp:Literal ID="ltrpaymentMethod" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                    </tr>
                                                    <tr id="trcard" runat="server" visible="false">
                                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">Card Number: <b><span id="lblCardNo">
                                                            <asp:Literal ID="ltrCardNumber" runat="server"></asp:Literal></span></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="left" style="height: 20px" rowspan="1"></td>
                                        </tr>
                                        <tr class="popup_cantain">
                                            <td valign="middle" align="left" rowspan="1">
                                                <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <span class="error" id="lblMsg"></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 20px"></td>
                    </tr>
                    <tr>
                        <td width="100%" align="left" style="height: 19px">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td style="height: 19px">
                                            <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" align="center"></td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 20px"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <table width="60%">
                                                <tr id="trordernotes" runat="server" visible="false">
                                                    <td valign="top" align="left" width="8%"><b>Order&nbsp;Notes </b>&nbsp;</td><td valign="top" align="left" width="2%">:</td>
                                                    <td valign="top">
                                                        <asp:Literal ID="ltordernotes" runat="server"></asp:Literal>
                                                    </td>

                                                </tr>
                                                <tr id="trcustnotes" runat="server" visible="false">
                                                     <td valign="top" align="left" width="5%"><b>Notes :</b>&nbsp;</td> 
                                                    <td valign="top" align="left" colspan="2">
                                                       <asp:Literal ID="ltcustomernotes" runat="server"></asp:Literal>
                                                    </td>
                                                     
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left" colspan="5" class="font-black04">
                                            <asp:Literal ID="ltInvoiceSignature" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr class="Printinvoice">
                        <td valign="middle" align="right">
                            <a title="Print Invoice" href="javascript:window.print();">
                                <img title="Print Invoice" alt="Print Invoice" src="/App_Themes/<%=Page.Theme %>/button/print.png" /></a>&nbsp;&nbsp;
                      <%--  <a title="Send Invoice To Customer" href="javascript:void(0);" onclick="javascript:SendInvoice();">
                            <img title="Send Invoice To Customer" alt="Send Invoice To Customer" src="<%=Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Live_Server") %>/App_Themes/<%=Page.Theme %>/button/send-invoice.png" /></a>--%>
                        </td>
                    </tr>
                </tbody>
            </table>
            <img src='/images/watermark_canceled.png' style='left:30%;top:40%;position:fixed;z-index:1000;' id="idcanceledtag" runat="server" alt="" visible="false" />
        </div>
        <div style="display: none;">
            <asp:Button ID="btnInvoice" runat="server" OnClick="btnInvoice_Click" />
            <input type="hidden" id="hdmemail" runat="server" value="" />
            <input type="hidden" id="hdntabid" runat="server" value="1" />
            <input type="button" id="btnreadmore" />
        </div>
        <div id="popupContact" style="z-index: 1000001; width: 900px; height: 600px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444; height: 598px">
                <tr style="background-color: #444; height: 25px;">
                    <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif; font-weight: bold;">&nbsp;Send Mail To Customer
                    </td>
                    <td align="right" valign="top">
                        <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="/App_Themes/Gray/Images/cancel-icon.png"
                            runat="server" ToolTip="Close" OnClientClick="disablePopup();return false;"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <table width="100%" border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                            <tr class="altrow">
                                <td style="width: 10%" valign="top">
                                    <span class="star">*</span>Email :
                                </td>
                                <td valign="top">
                                    <asp:TextBox runat="server" EnableViewState="false" ID="txtCustEmail" CssClass="order-textfield"
                                        Width="400px"></asp:TextBox>
                                    (Use (;) sign for more than one recipients)
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <span class="star">*</span>Subject :
                                </td>
                                <td valign="top">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="ckeditor-table">
                                                <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtEmailBody"
                                                    TabIndex="27" Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                <script type="text/javascript">
                                                    CKEDITOR.replace('<%= txtEmailBody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 380, toolbarStartupExpanded: false });
                                                    CKEDITOR.on('dialogDefinition', function (ev) {
                                                        if (ev.data.name == 'image') {
                                                            var btn = ev.data.definition.getContents('info').get('browse');
                                                            btn.hidden = false;
                                                            btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                        }
                                                        if (ev.data.name == 'link') {
                                                            var btn = ev.data.definition.getContents('info').get('browse');
                                                            btn.hidden = false;
                                                            btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                        }
                                                    });
                                                </script>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="oddrow">
                                <td></td>
                                <td>
                                    <asp:ImageButton ID="btnSendmail" OnClientClick="return SendInvoiceValidation();"
                                        runat="server" OnClick="btnSendmail_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="backgroundPopup" style="z-index: 1000000;">
        </div>
    </form>
</body>
</html>
