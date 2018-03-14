<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDetail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ViewDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <div id="popupcontactdetails" style="z-index: 1000001; width: 500px; height: 213px;padding:2px 0 0 3px">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            font-size: 12px; font-family: Arial,Helvetica,sans-serif;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Credit Card Details
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupviewdetailclose" Style="position: relative;" ImageUrl="/App_Themes/Gray/images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();">
                    </asp:ImageButton>
                </td>
            </tr>
            <tr style="background-color: White">
                <td colspan="2" valign="top">
                    <div style="display: none" id="creditdetail" runat="server">
                        <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Customer Name
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblcustomername"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Card Name
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblcardname"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Card type
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblcardtype"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Card Number
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblcardnumber"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>CVC
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblcvc"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Expiry Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblmonth"></asp:Label>
                                    /
                                    <asp:Label runat="server" ID="lblyear"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display: block;" id="password" runat="server">
                        <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                            <tr>
                                <td valign="top" style="font-size: 11px;">
                                    <span class="required-red"></span>Password
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtpassword" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                                        Width="143" TextMode="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="oddrow">
                                <td colspan="2">
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnsubmit" OnClientClick="return checkFields(); return false;"
                                        OnClick="btnsubmit_Click" runat="server" ImageUrl="/App_Themes/Gray/images/next.png" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 80px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function checkFields() {
            if (document.getElementById('<%=txtpassword.ClientID %>') == null || document.getElementById('<%=txtpassword.ClientID %>').selectedIndex == "") {
                jAlert('Please Enter Password.', 'Required Information', '<%=txtpassword.ClientID %>');
                return false;
            }
            //document.getElementById("prepage").style.display = 'block';
            return true;
        }
    </script>
    </form>
</body>
</html>
