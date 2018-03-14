<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BulkPickingSlip.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.BulkPickingSlip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  
 
    <style type="text/css" media="print">
        .myprint
        {
            display: none;
        }
        .break
        {
            page-break-before: left;
            width: 10px;
            height: 1px;
            display: block;
        }
    </style>
    <style type="text/css">
#head {color:#ffffff}
p{padding:0px;margin:0px;line-height:15px;}
table{font-size:12px;}
table th{line-height:10px;}
.billaddtext{padding-top:0px;font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px;color:#000000;}
.style1 {font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-weight: bold;font-size: 12px;color:black;}
.style2 {font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:15px;}
.style3 {font-size: 12px; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;color:#000000;}
.style4 {font-family: Arial,Tahoma,verdana,Helvetica,sans-serif; font-size: 12px; }
.style5 {font-size: 12px;font-weight: bold;}
ul{margin:0px 0px 0px 15px;padding:0px;}
li{font-weight:normal;list-style:circle;margin-left:10px;}
.popup_header {background:none repeat scroll 0 0 #000000;
background:url(/Client/images/Header_bg_Kit.gif) center top repeat-x;
border:1px solid #000000;float:left;height:35px;margin:0;width:505px;padding-top:15px;*padding-top:5px;}
body {font-family:Arial,Tahoma,verdana,Helvetica,sans-serif;font-size:12px;
margin:0px;padding:0px;}

#header{width:505px;height:60px;
background:url(/Client/images/header_bg.jpg) center bottom repeat-x;padding:0px 0 0 0;
text-align:center;font-family:Arial,Tahoma,verdana,Helvetica,sans-serif;}
.stylecontact{
font-family:Verdana,Arial,Helvetica,sans-serif;
font-size:15px;
}
.logo{float:left;width:500px;padding:13px 0 0 0;text-align:center;margin-bottom:17px;}
</style>
</head>
<body style="background: none;" onload="javascript:var hh=$(document).height();window.parent.document.getElementById('frmPickingSlip').height =hh+'px';" onunload="javascript:var hh=$(document).height();window.parent.document.getElementById('frmPickingSlip').height =hh+'px';">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <div style='float: right;' id="idprint" runat="server">
    </div>
    <div id="ltBultprint" runat="server">
        <table border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="white"
            width="600px" style="border: solid 1px #d0d0d0;">
            <tr>
                <td align="center" height="15px" colspan="3">
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    ###logo###
                </td>
            </tr>
            <tr>
                <td align="center" height="15px" colspan="3">
                </td>
            </tr>
            <tr>
                <td width="282px" valign="top">
                    <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                        <tr height="25px">
                            <td height="19" align="left" colspan="2">
                                <span class="style3" style="font-size: 12px; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;
                                    padding-left: 5px"><strong>Order #: ###lblOrderId###</strong></span>
                            </td>
                        </tr>
                        <tr height="25px">
                            <td valign="top" align="left" style="padding-top: 5px; padding-left: 5px; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                <span class="style1">Order Date : </span>
                            </td>
                            <td class="style1" align="left" style="padding-top: 5px; font-weight: normal; vertical-align: top">
                                ###lblDate###
                            </td>
                        </tr>
                        <tr height="25px">
                            <td valign="top" class="style1" align="left" style="padding-left: 5px; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                <span class="style1">Payment Method :</span>
                            </td>
                            <td valign="top" class="style1" align="left" style="font-weight: normal; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                ###lblPaymentMethod###
                            </td>
                        </tr>
                        <tr height="25px">
                            <td valign="top" class="style1" align="left" style="padding-left: 5px; font-family: Verdana,Arial,Helvetica,sans-serif;">
                                <span class="style1">Name On Card :</span>
                            </td>
                            <td valign="top" class="style1" align="left" style="font-weight: normal; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                <p>
                                    ###lblName### &nbsp;</p>
                            </td>
                        </tr>
                        <tr height="25px">
                            <td valign="top" align="left" style="padding-left: 5px; padding-bottom: 1px; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                <span class="style1">Card Type :</span>
                            </td>
                            <td valign="top" class="style1" align="left" style="font-weight: normal; font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;">
                                ###lblCardType###
                            </td>
                        </tr>
                        <tr height="25px">
                            <td valign="top" class="style1" align="left" style="font-family: Arial,Tahoma,verdana,Helvetica,sans-serif;
                                padding-left: 5px;">
                                <strong>Card Number :</strong>
                            </td>
                            <td valign="top" class="style1" align="left" style="font-weight: normal; font-family: arial,tahoma,verdana,helvetica,sans-serif;">
                                ###lblCardNo###
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="1px" height="150px" valign="middle">
                    <hr style="border: solid 1px #eeeeee; height: 150px" />
                </td>
                <td align="right" style="padding-top: 0px; vertical-align: top; width: 248px; padding-right: 5px">
                    ###src###
                </td>
            </tr>
            <tr>
                <td colspan="3" height="20px">
                    <hr style="border: solid 1px #eeeeee" />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="padding-left: 10px; font-family: Verdana,Arial,Helvetica,sans-serif;"
                    colspan="3">
                    ###ltBilling###
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" height="40px">
                    &nbsp;
                </td>
            </tr>
            <tr align="left">
                <td colspan="3" align="left">
                    <center>
                        <table width="500px" cellpadding="0" cellspacing="0" border="0" style="font-family: Verdana,Arial,Helvetica,sans-serif;">
                            <tr>
                                <td style="height: 19px; padding-left: 3px" colspan="5">
                                    <center>
                                        ###lblMsg### ###ltCart###
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="style3" colspan="4" style="height: 18px; text-align: right"
                                    valign="top">
                                </td>
                                <td class="style3" style="width: 15%; height: 18px; text-align: right" valign="top">
                                </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr align="left">
                 
                <td colspan="3" align="left" style="padding:20px;">
                    Customer Note: ###Notes####
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 10px; padding-top: 20px">
                    <font style="padding: 2px 2px; text-align: left;font-size: 11px; font-family: Arial, Helvetica, sans-serif;
                        padding-left: 20px; color: #000000; line-height: 16px;">Thank You, </font>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 10px;">
                    <font style="padding: 2px 2px; text-align: left; font-size: 11px; font-family: Arial, Helvetica, sans-serif;
                        padding-left: 20px; color: #000000; line-height: 16px;">###Storename###</font>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="padding: 10px;">
                </td>
            </tr>
        </table>
        <span class="break"></span>
    </div>
    </form>
</body>
</html>
