<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome to Admin Panel</title>
    <script type="text/javascript" src="/js/jquery-1.3.2.js"></script>
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/popup.js"></script>
    <script type="text/javascript">
        function checkLogin() {
            if (document.getElementById('txtUserName') != null && document.getElementById('txtUserName').value == '') {
                jAlert('Please Enter User Name.', 'Message', 'txtUserName');
                //document.getElementById('txtUserName').focus();
                return false;
            }
            else if (document.getElementById('txtPassword') != null && document.getElementById('txtPassword').value == '') {
                jAlert('Please Enter Password.', 'Message', 'txtPassword');
                //document.getElementById('txtPassword').focus();
                return false;

            }
            return true;
        }
    </script>
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

        function CheckForgotPassword() {
            if (document.getElementById('txtEmail') != null && document.getElementById('txtEmail').value == '') {
                jAlert('Please enter your Email.', 'Message', 'txtEmail');
                //document.getElementById('txtEmail').focus();
                return false;
            }
            else if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value != '' && !checkemail1(document.getElementById("txtEmail").value)) {
                jAlert('Please enter valid Email.', 'Message', 'txtEmail');
                //document.getElementById('txtEmail').focus();
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function ShowForGotPassword() {
            if (document.getElementById('txtEmail') != null) {
                document.getElementById('txtEmail').value = ''
                document.getElementById('txtEmail').focus();
            }
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();

            return false;
        }
        
    </script>
</head>
<body class="login-bg">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td>
                    <table width="1000" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 0 auto;
                        position: relative;">
                        <tbody>
                            <tr>
                                <td valign="top" align="center" class="header">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <div class="login-page-bg">
                                        <div class="login-page-logo">
                                            <img src="/App_Themes/<%=Page.Theme %>/images/logo.png" title="" alt="" />
                                        </div>
                                        <div class="login-page">
                                            <div class="login-page-right">
                                                <div class="login-page-right-content">
                                                    <div class="login-page-right-textbox">
                                                        <label>
                                                            User Name:</label>
                                                        <p>
                                                            <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-left.png" />
                                                            <span class="overlay_wrapper">
                                                                <asp:TextBox CssClass="login-page-right-textbox-input" ID="txtUserName" EnableViewState="false"
                                                                    runat="server"></asp:TextBox>
                                                            </span>
                                                            <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-right.png" />
                                                        </p>
                                                    </div>
                                                    <div class="login-page-right-textbox">
                                                        <label>
                                                            Password:</label>
                                                        <p>
                                                            <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-left.png" />
                                                            <span class="overlay_wrapper">
                                                                <asp:TextBox CssClass="login-page-right-textbox-input" ID="txtPassword" EnableViewState="false"
                                                                    TextMode="Password" runat="server"></asp:TextBox>
                                                            </span>
                                                            <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-right.png" /></p>
                                                    </div>
                                                    <div class="login-page-right-textbox">
                                                        <label>
                                                            &nbsp;</label>
                                                        <p>
                                                            <asp:ImageButton TabIndex="3" ID="btnLogin" runat="server" Style="float: left;" ImageUrl="/App_Themes/<%=Page.Theme %>/images/login.png"
                                                                OnClientClick="return checkLogin();" ValidationGroup="Login" OnClick="btnLogin_Click" />
                                                            <a href="javascript:void(0);" onclick="return ShowForGotPassword();" class="img-right">
                                                                Forgot Password?</a>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <img class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/login-page-bottom.png" />
                                        <div class="bottom-link">
                                            Copyright &copy;
                                            <%=System.DateTime.Now.Year.ToString() %>, Half Price Drapes, All rights reserved.</div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <div style="display: none">
        <input type="button" id="btnreadmore" />
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 400px; height: 200px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            height: 200px">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Forgot Password
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative;" ImageUrl="~/images/cancel.png"
                        runat="server" ToolTip="Close" OnClientClick="disablePopup();return false;"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table width="100%" border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                        <tr class="altrow">
                            <td>
                                <span class="star">*</span>Email :
                            </td>
                            <td>
                                <asp:TextBox runat="server" EnableViewState="false" ID="txtEmail" CssClass="order-textfield"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="oddrow">
                            <td>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSend" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/button/send-email.png"
                                    ValidationGroup="Login" OnClientClick="return CheckForgotPassword();" OnClick="btnSend_Click" />
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
