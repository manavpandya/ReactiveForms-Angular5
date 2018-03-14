<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefundOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.RefundOrder"
    ValidateRequest="false" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                }
                else {
                    return false;
                }
            }

            return true;
        }

        function checkValidation() {
            if (document.getElementById('txtAmount').value != null && document.getElementById('txtAmount').value == '') {
                jAlert('Please Enter Refund Amount.', 'Message', 'txtAmount');
                return false;
            }

            var amount = parseFloat (parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnAmount').value) - parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value)).toFixed(2);
             
            var amount1 = parseFloat(document.getElementById('txtAmount').value).toFixed(2);
            document.getElementById('hdnamtorder').value = parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnAmount').value);
            document.getElementById('hdnamtrefund').value = window.opener.document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value;
          
            if (parseFloat(amount1) > parseFloat(amount)) {

                jAlert('Please enter refund amount less then or equal to $' + amount, 'Message', 'txtAmount');
                return false;
            }
            chkHeight();
            return true;
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height();
             
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function resetField() {
            if (document.getElementById('txtAmount') != null) {
                document.getElementById('txtAmount').value = '';
                document.getElementById('txtAmount').focus();
            }
            if (document.getElementById('txtReason') != null) {
                document.getElementById('txtReason').value = '';
            }
            

            return false;
        }
        function amountSet() {
            document.getElementById('hdnamtorder').value = parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnAmount').value);
            document.getElementById('hdnamtrefund').value = window.opener.document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value;
            document.getElementById('lblTotal').innerHTML = (parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnAmount').value) - parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value)).toFixed(2);
            document.getElementById('lblRefundedamount').innerHTML = parseFloat(window.opener.document.getElementById('ContentPlaceHolder1_hdnrefundAmount').value).toFixed(2);
            
        }
    </script>
</head>
<body style="background: none;font-family:Arial,Helvetica,sans-serif;font-size:11px;" onload="amountSet();">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <table width="96%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
        font-size: 11px;margin:10px;background-color:#fbfbfb;">
        <tr style="background-color: #444; height: 30px;">
            <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                font-weight: bold;">
                &nbsp;Refund Order
            </td>
            <td align="right" valign="top">
                <asp:ImageButton ID="imgClose" Style="position: relative;" ImageUrl="~/images/cancel.png"
                    runat="server" ToolTip="Close" OnClientClick="window.close();return false;"></asp:ImageButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                    <tr>
                        <td align="left">
                            <b>Order Number : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltorderNumber" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <b>Order Total : </b>
                        </td>
                        <td align="left" style="color:Red;">
                            $<asp:Label ID="lblTotal" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td align="left">
                            <b>Refunded Amount : </b>
                        </td>
                        <td align="left">
                            $<asp:Label ID="lblRefundedamount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <b>Refund Amount : </b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAmount" runat="server" onkeypress="return isNumberKey(event);"
                                CssClass="order-textfield" Style="font-size: 12px;" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <b>Refund Reason : </b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" CssClass="order-textfield"
                                Style="font-size: 12px;" Width="200px" Height="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                        </td>
                        <td align="left">
                            <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" OnClientClick="return checkValidation();"
                                runat="server" />&nbsp;<asp:ImageButton ID="btnReset" OnClientClick="return resetField();"
                                    runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display:none;">
    <input type="hidden" id="hdnamtorder" runat="server" value="0" />
    <input type="hidden" id="hdnamtrefund" runat="server" value="0" />
    </div>
     <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 10%;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
