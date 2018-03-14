<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReferAFriendAmazonProduct.aspx.cs" Inherits="Solution.UI.Web.ReferAFriendAmazonProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href='http://fonts.googleapis.com/css?family=Carrois+Gothic|Telex|Oxygen' rel='stylesheet' type='text/css' />
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

            if (document.getElementById('<%=txtYourname.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert('Please enter your name.');
                document.getElementById('<%=txtYourname.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtYourEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter your email");
                document.getElementById('<%=txtYourEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtYourEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtYourEmail.ClientID %>').value.replace(/^\s+|\s+$/g, ''))) {
                alert('Please enter valid email.');
                document.getElementById('<%=txtYourEmail.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtReEnterEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter re-enter email.");
                document.getElementById('<%=txtReEnterEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtReEnterEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtReEnterEmail.ClientID %>').value.replace(/^\s+|\s+$/g, ''))) {
                alert('Please enter valid re-enter email.');
                document.getElementById('<%=txtReEnterEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtYourEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') != document.getElementById('<%=txtReEnterEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '')) {
                alert('Re-enter email must be same with your email.');
                document.getElementById('<%=txtReEnterEmail.ClientID %>').focus();
                return false;
            }

            if (document.getElementById('<%=txtEmail1.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert("Please enter Email1");
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail1.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtEmail1.ClientID %>').value.replace(/^\s+|\s+$/g, ''))) {
                alert('Please enter valid Email1.');
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtYourEmail.ClientID %>').value.replace(/^\s+|\s+$/g, '') == document.getElementById('<%=txtEmail1.ClientID %>').value.replace(/^\s+|\s+$/g, '')) {
                alert('Your email and Email1 can not be same.');
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail2.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtEmail2.ClientID %>').value.replace(/^\s+|\s+$/g, ''))) {
                alert('Please enter valid Email2.');
                document.getElementById('<%=txtEmail2.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail3.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtEmail3.ClientID %>').value.replace(/^\s+|\s+$/g, ''))) {
                alert('Please enter valid Email3.');
                document.getElementById('<%=txtEmail3.ClientID %>').focus();
                return false;
            }

            document.getElementById("prepage").style.display = '';
            return true;
        }

    </script>
    <script type="text/javascript">
        function windowloadurl() {
//            if (document.getElementById('hdnurl') != null && document.getElementById('hdnurl').value == '') {
//                document.getElementById('hdnurl').value = document.referrer;
//            }

            document.getElementById('imgMain').src = 'http://ecx.images-amazon.com/images/I/' + '<%=Request.QueryString["id"]%>' + '.jpg';
        }
        function clearfields() {
            document.getElementById('<%=txtYourname.ClientID %>').value = '';
            document.getElementById('<%=txtYourEmail.ClientID %>').value = '';
            document.getElementById('<%=txtReEnterEmail.ClientID %>').value = '';
            document.getElementById('<%=txtEmail1.ClientID %>').value = '';
            document.getElementById('<%=txtEmail2.ClientID %>').value = '';
            document.getElementById('<%=txtEmail3.ClientID %>').value = '';
            document.getElementById('<%=txtYourname.ClientID %>').focus();
            return false;
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
                            <a href="#" title="Cash Register">
                                <img src="/images/store_3.png"  style="float: left; padding: 10px 0 0 10px;" id="" /></a>
                        </td>
                        <td class="close-bg">
                            <a href="javascript:window.close();" title="Close" class="close">
                                <img src="/images/remove.png" alt="Close" style="border: 0px" title="Close" /></a>
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
            <td colspan="2" align="left" style="color: #000; height: 28px; background: #F1F1F1;
                border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                font-size: 12px; font-weight: bold;">
                <strong>Refer A Friend</strong>
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
                            <span class="checkout-red">*</span>Your Name
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtYourname" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Your Email
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtYourEmail" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Re-enter Email &nbsp;
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtReEnterEmail" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            &nbsp;&nbsp;Please enter your friend's email addresses<br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="checkout-red">*</span>Email 1
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail1" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;Email 2
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail2" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;&nbsp;Email 3
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail3" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            &nbsp;&nbsp;The email that will be sent will contain your name
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td>
                            <asp:ImageButton ID="btnSend" Style="padding: 0px 10px 0px 0px; border: 0px" runat="server"
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
    <%--  <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 100px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>--%>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <div style="border: 1px solid #ccc;">
            <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                <tr>
                    <td>
                        <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                            width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                            border-radius: 10px;">
                            <center>
                                <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
