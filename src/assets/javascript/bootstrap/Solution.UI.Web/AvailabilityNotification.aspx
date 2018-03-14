<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AvailabilityNotification.aspx.cs"
    Inherits="Solution.UI.Web.AvailabilityNotification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            background: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2a2a2a;
        }
        a, img
        {
            border: none;
            outline: none;
        }
        .table_border
        {
            border: 1px solid #D5D4D4;
            margin: 0 auto;
        }
        .checkout-red
        {
            color: #fe0000;
        }
        .name-input
        {
            border: 1px solid #E4E4E4;
            height: 18px;
            width: 210px;
            font-size: 12px;
            padding: 1px 5px;
        }
        .logo
        {
            float: left;
            width: 480px;
            background: #fff;
            padding: 10px 0 10px;
            margin: 0;
        }
        .close-bg
        {
            float: right;
            width: 30px;
            padding: 15px 40px 12px 0;
            text-align: left;
            background: #fff;
            margin: 0;
        }
    </style>
    <script src="js/popup.js" type="text/javascript"></script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
        }
    </script>
    <script type="text/javascript">
        function checkfields() {

            if (document.getElementById('<%=txtFirstName.ClientID %>') != null && document.getElementById('<%=txtFirstName.ClientID %>').value == '') {
                alert('Please enter First Name.');
                document.getElementById('<%=txtFirstName.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtLastName.ClientID %>') != null && document.getElementById('<%=txtLastName.ClientID %>').value == '') {
                alert("Please enter Last Name.");
                document.getElementById('<%=txtLastName.ClientID %>').focus();
                return false;
            }

            if (document.getElementById('<%=txttelephone.ClientID %>') != null && document.getElementById('<%=txttelephone.ClientID %>').value == '') {
                alert("Please enter Telephone Number.");
                document.getElementById('<%=txttelephone.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value == '') {
                alert("Please enter Email.");
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
                alert('Please enter valid Email.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            document.getElementById("prepage").style.display = '';
            return true;
        }

    </script>
    <script type="text/javascript">
        function clearfields() {
            document.getElementById('<%=txtFirstName.ClientID %>').value = '';
            document.getElementById('<%=txtLastName.ClientID %>').value = '';
            document.getElementById('<%=txttelephone.ClientID %>').value = '';
            document.getElementById('<%=txtEmail.ClientID %>').value = '';
            document.getElementById('<%=txtFirstName.ClientID %>').focus();
            return false;
        }
    </script>
    <script type="text/javascript">
        function onKeyPressPhone(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
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
</head>
<body>
    <form id="form1" runat="server">
    <table width="550" border="0" cellpadding="0" cellspacing="0" class="table_border">
        <tr>
            <td colspan="3" valign="middle">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="logo">
                            <img src="/images/logo.png" style="float: left; padding: 10px 0 0 10px;" />
                        </td>
                        <td class="close-bg">
                            <a href="javascript:void(0);" onclick="window.parent.disablePopup();" title="Close"
                                class="close">
                                <img src="/images/Close.png" alt="Close" style="border: 0px" title="Close" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" style="color: #000000; height: 28px; background: #F1F1f1;
                border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                font-size: 12px; font-weight: bold;">
                <strong>Availability Notification</strong>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" border="0" cellpadding="4" cellspacing="0" style="width: 570px">
                    <tr>
                        <td colspan="3" align="right">
                            <span class="checkout-red">*</span>Required Fields
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 180px;">
                            <span class="checkout-red">*</span>First Name
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Last Name
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Telephone
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txttelephone" runat="server" CssClass="name-input" MaxLength="20"
                                onkeypress="return onKeyPressPhone(event);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Email
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSend" Style="padding: 0px 0px 0px 0px; border: 0px" runat="server"
                                alt="SEND" title="SEND" ImageUrl="/images/send.png" OnClientClick="return checkfields();"
                                OnClick="btnSend_Click" />
                            <asp:ImageButton ID="btnReset" Style="border: 0px" runat="server" alt="RESET" title="RESET"
                                ImageUrl="/images/reset.png" OnClientClick="return clearfields();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
