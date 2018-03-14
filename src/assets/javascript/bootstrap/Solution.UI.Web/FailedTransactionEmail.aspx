﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailedTransactionEmail.aspx.cs"
    Inherits="Solution.UI.Web.FailedTransactionEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
</head>
<body>
    <table width="624" border="0" align="center" cellpadding="0" cellspacing="0" class="popup_docwidth">
        <tr>
            <td>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center">
                            <a href="#" >
                                <img style="margin: 5px;" src="<%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("LIVE_SERVER")%>/images/logo.png"
                                    alt="" border="0"  /></a>
                        </td>
                    </tr>
                </table>
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
                                        Dear Administrator,<br />
                                        <span style="padding-left: 20px">We have Received Failed Transaction Form
                                            <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName").ToString() %>
                                            . </span>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="10">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Order Number: <strong style="color: #F00" id="lblOrderId">
                                            <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal></span></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Order Date: <strong id="lblDate">
                                            <asp:Literal ID="ltrOrderdate" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <%--                                <tr >
                                    <td>
                                        <span id="lblDelMethod">Shipping Method:</span> <strong id="lblDeliveryMethod">
                                            <asp:Literal ID="ltrshippingMethod" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        Payment Method: <strong id="lblPaymentMethod">
                                            <asp:Literal ID="ltrpaymentMethod" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>
                                <%-- <tr id="trcard" runat="server" visible="false">
                                    <td>
                                        Card Number: <strong id="lblCardNo">
                                            <asp:Literal ID="ltrCardNumber" runat="server"></asp:Literal></strong>
                                    </td>
                                </tr>--%>
                                <tr id="tr1" runat="server">
                                    <td>
                                        Email: <strong id="Strong1">
                                            <asp:Literal ID="ltrCustEmail" runat="server"></asp:Literal></strong>
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
                                    <td>
                                        <asp:Literal ID="ltInvoiceSignature" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong>Failed Transaction Reason :</strong> &nbsp;<asp:Literal ID="ltrFailedReson"
                                            runat="server"></asp:Literal>
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
        <tr>
            <td class="pop_footer_link">
                &copy; 2012.
                <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("STOREPATH")%>
                All rights reserved.
            </td>
        </tr>
    </table>
</body>
</html>
