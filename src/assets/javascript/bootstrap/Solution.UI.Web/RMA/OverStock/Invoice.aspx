<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="Solution.UI.Web.RMA.OverStock.Invoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <%-- <style type="text/css">
    .datatable table{border:1px solid #eeeeee;  }
    .datatable tr.alter_row{background-color:#f9f9f9;}
    .datatable td{padding:2px 2px; text-align:left; border:1px solid #eeeeee; font:11px/14px Verdana, Arial, Helvetica, sans-serif; color:#4c4c4c; line-height:16px;}
    .datatable th{padding:2px 3px; text-align:left; border:1px solid #eeeeee; font:11px/14px  Verdana, Arial, Helvetica, sans-serif; font-weight:bold; color:#4c4c4c; line-height:16px;}
    .receiptfont{font:11px/14px Verdana, Arial, Helvetica, sans-serif; color:#4c4c4c;}
    .receiptlineheight{  height: 15px;}
    .Printinvoice {display:block;}
    </style>--%>
    <style type="text/css" media="print">
        .Printinvoice
        {
            display: none;
        }
    </style>
    <style type="text/css">
        
         body { margin:0px; padding:0px; font-family:Arial, Helvetica, sans-serif; }
        .popup_docwidth { width:642px; border:1px solid #DFDFDF; background-color:#FFFFFF; margin:0 auto; font-family:Arial, Helvetica, sans-serif; }
        .pop_header_row2 { float:left; width:640px; text-align:center; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:28px; color:#2A2A2A; background:#fff; border-top:1px solid #CFCFCF; border-bottom:1px solid #CFCFCF; text-transform:uppercase; }
        .pop_border { width:100%; }
        .pop_border th { background:#E4E4E4; }
        .pop_border td { border:1px solid #DFDFDF; padding:5px; }
        .pop_header_row2 a { color:#2A2A2A; text-decoration:none; }
        .pop_header_row2 a:hover { color:#2A2A2A; text-decoration:underline; }
        .popup_cantain { font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #2A2A2A; text-decoration: none; line-height: 25px; }
        .popup_cantain a { font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #FE0000; text-decoration: none; }
        .popup_cantain a:hover {
        font-family:; font-size:12px; color:#000; text-decoration:underline; }
        .user_bg { width:300px; height:60px; padding-top:7px; padding-left:7px; background:url(images/user_bg.gif) no-repeat left top; }
        .style3 { font-weight: bold }
        .forgot_textfild { color:#2A2A2A; width:150px; }
        .pop_bottom_link { background:#F1F1F1; font-family:Arial, Helvetica, sans-serif; font-size:11px; text-align:center; line-height:30px; height:30px; color:#000000; text-decoration:none; }
        .pop_bottom_link a { color:#000000; text-decoration:none; }
        .pop_bottom_link a:hover { color:#000000; text-decoration:underline; }
        .pop_footer_link { height: 30px; line-height: 30px; font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #2A2A2A; text-align:center }
        .pop_border_new1 { width:100%; line-height:20px; }
        .pop_border_new1 th { background:#fff; }
        .pop_border_new1 td { border:1px solid #fff; padding:2px 5px; }
        .pop_border_new { width:100%; line-height:20px; }
        .pop_border_new th { background:#E4E4E4; }
        .pop_border_new td { border:1px solid #DFDFDF; padding:2px 5px; }
         .pop_border_new2 { width:100%;  font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #2A2A2A; text-decoration: none; line-height: 25px; }
        .pop_border_new2 th { background:#CFCFCF; font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #2A2A2A; text-decoration: none; line-height: 25px;}
        .pop_border_new2 td { border:1px solid #CFCFCF; padding:2px 5px; font-family:Arial, Helvetica, sans-serif; font-size:12px; color: #2A2A2A; text-decoration: none; line-height: 25px;}
     </style>
    <%-- <style type="text/css" media="print">
        .form
        {
            page-break-after: always;
        }
    </style>--%>
</head>
<body>
    <table width="624" cellspacing="0" cellpadding="0" align="center" class="popup_docwidth"
        style="border: none;">
        <tr class="Printinvoice">
            <td valign="middle" align="left">
                <a title="PRINT" href="javascript:window.print();" style="margin-top: 5px;">
                    <img title="PRINT" alt="PRINT" src="/images/print.png" border="0" /></a>
            </td>
        </tr>
    </table>
    <table width="624" border="0" align="center" cellpadding="0" cellspacing="0" class="popup_docwidth">
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <a href="#" title="">
                                <img alt="logo" border="0" style="margin: 5px;" runat="server" id="ImgStoreLogo" />
                            </a>
                        </td>
                        <td width="35" align="center" id="trFacebook" runat="server">
                            <a title="Facebook" target="_blank" href="/">
                                <img src="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/images/fb-icon.png"
                                    alt="Facebook" border="0" title="Facebook"></a>
                        </td>
                        <td width="35" align="center" id="trTwitter" runat="server">
                            <a title="Twitter" target="_blank" href="/">
                                <img src="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/images/twitter-icon.png"
                                    alt="Twitter" border="0" title="Twitter" /></a>
                        </td>
                        <td width="35" align="center" id="trPinterest" runat="server">
                            <a title="Pinterest" target="_blank" href="/">
                                <img src="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/images/pinterest-icon.png"
                                    alt="Pinterest" border="0" title="Pinterest"></a>
                        </td>
                        <td width="35" align="center" id="trGooglePlus" runat="server">
                            <a title="Google Plus" target="_blank" href="/">
                                <img src="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/images/google-plus-icon.png"
                                    alt="Google Plus" border="0" title="Google Plus"></a>
                        </td>
                        <td width="20">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trHeaderMenu" runat="server">
            <td align="center" class="pop_header_row2">
                <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/index.aspx"
                    title="Home">Home</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/aboutus"
                        title="About Us">About Us</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/TechnicalSupport"
                            title="Technical Support">Technical Support</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/contactUs.aspx"
                                title="Contact Us">Contact Us</a>&nbsp;&nbsp;|&nbsp;&nbsp;<a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/blog"
                                    title="Blog">Blog</a>
            </td>
        </tr>
        <tr id="trStoreBanner" runat="server">
            <td>
                 
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="20">
                            &nbsp;
                        </td>
                        <td width="600">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="popup_cantain">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Dear <strong>
                                            <asp:Literal ID="ltrName" runat="server"></asp:Literal></strong>,<br />
                                        Your order has been received. Thank you for shopping with
                                        <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName").ToString() %>.
                                        <br />
                                        We appreciate your business and are committed to delivering excellent customer care.
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Order Number: <strong style="color: #F00" id="lblOrderId">
                                            <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Order Date: <strong id="lblDate">
                                            <asp:Literal ID="ltrOrderdate" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblDelMethod">Shipping Method:</span> <strong id="lblDeliveryMethod">
                                            <asp:Literal ID="ltrshippingMethod" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Payment Method: <strong id="lblPaymentMethod">
                                            <asp:Literal ID="ltrpaymentMethod" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <tr id="trcard" runat="server" visible="false">
                                    <td>
                                        Card Number: <strong id="lblCardNo">
                                            <asp:Literal ID="ltrCardNumber" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltrAddress" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Product Information :</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Congratulations! Your order has been prioritized and is being processed now. You
                                        will receive another email containing your tracking information.
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <br />
                                        Please note: If you are purchasing a large order it may be delivered separately.
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="ltInvoiceSignature" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="20">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <span class="error" id="lblMsg"></span>
            </td>
        </tr>
        <tr id="footerLink">
            <td class="pop_bottom_link">
                <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/index.aspx"
                    title="Home">Home</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/aboutus"
                        title="About Us">About Us</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/technicalSupport"
                            title="Tech &amp; Setup">Tech &amp; Setup</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/returnpolicy"
                                title="Return Policy">Return Policy</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/shippingpolicy"
                                    title="Shipping Policy">Shipping Policy</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/warranty"
                                        title="Warranty">Warranty</a> | <a href="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/Contactus.aspx"
                                            title="Contact Us">Contact Us</a>
            </td>
        </tr>
        <tr>
            <td class="pop_footer_link">
                &copy; 2012.
                <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("STOREPATH")%>
                All rights reserved.
            </td>
        </tr>
    </table>
</body>
<%--<body>
    <form id="theForm" runat="server">
    <div class="body12">
        <table width="100%" cellspacing="0" cellpadding="0" align="center" class="table">
            <tbody>
                <tr>
                    <td align="center" class="receiptfont">
                        <div align="center" class="bkground123">
                            <table width="100%" cellspacing="0" cellpadding="0" align="center" class="signup-row">
                                <tbody>
                                    <tr>
                                        <td style="height: 14px" colspan="3">
                                        </td>
                                    </tr>
                                    <tr class="Printinvoice">
                                        <td valign="middle" align="left" rowspan="1">
                                             <a title="Print Invoice" href="javascript:window.print();">
                                            <img title="Print Invoice" alt="Print Invoice" src="/images/print.png" border="0" /></a>
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 85px">
                                        <td valign="middle" align="center" rowspan="1">
                                            <img alt="logo" border="0" src="/images/logo.png" class="img_left" id="Image1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" rowspan="1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" class="receiptlineheight" style="height: 22px" rowspan="1">
                                            Dear <b><span id="lblName">
                                                <asp:Literal ID="ltrName" runat="server"></asp:Literal>
                                            </span>,</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" class="receiptlineheight" style="height: 20px" rowspan="1">
                                            Your order has been received. Thank you for shopping with <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName").ToString() %>.
                                            <br />
                                             We appreciate your business and are committed to delivering excellent customer care.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" style="height: 20px" rowspan="1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" style="height: 20px" rowspan="1">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                            Order Number:&nbsp; <b><span id="lblOrderId">
                                                <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></span></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                            Order Date:&nbsp; <b><span id="lblDate">
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
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                            Payment Method: <b><span id="lblPaymentMethod">
                                                <asp:Literal ID="ltrpaymentMethod" runat="server"></asp:Literal></span></b>
                                        </td>
                                    </tr>
                                    <tr id="trcard" runat="server" visible="false">
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                            Card Number: <b><span id="lblCardNo">
                                                <asp:Literal ID="ltrCardNumber" runat="server"></asp:Literal></span></b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="height: 20px" rowspan="1">
                                        </td>
                                    </tr>
                                    <tr>
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
                    <td align="center" style="height: 20px">
                    </td>
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
                                    <td width="100%" align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
            </tbody>
        </table>
    </div>
    </form>
</body>--%>
</html>

