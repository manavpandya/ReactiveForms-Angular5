<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="Solution.UI.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ForLoginPopupFacebook() {
            var url = window.location.pathname;
            var myPageName = url.substring(url.lastIndexOf('/') + 1);
            if (myPageName == '')
                myPageName = 'index.aspx';
            var FacebookUrl = 'Facebook.aspx?RquestPageName=' + myPageName;
            window.location.href = FacebookUrl;
        }

        function ForLoginGoogle() {
            var url = window.location.pathname;
            var myPageName = url.substring(url.lastIndexOf('/') + 1);
            if (myPageName == '')
                myPageName = 'index.aspx';
            var GoogleUrl = 'GoogleLogin.aspx?googleauth=true&RquestPageName=' + myPageName;
            window.location.href = GoogleUrl;
        }

        function ForLoginTwitter() {
            var url = window.location.pathname;
            var myPageName = url.substring(url.lastIndexOf('/') + 1);
            if (myPageName == '')
                myPageName = 'index.aspx';
            var TwitterUrl = 'Twitter.aspx?RquestPageName=' + myPageName;
            window.location.href = TwitterUrl;
        }
    </script>
    <script type="text/javascript">
        function checkfieldsforlogin() {


            if (document.getElementById('<%=txtusername.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert('Please enter email address.');
                document.getElementById('<%=txtusername.ClientID %>').value = '';
                document.getElementById('<%=txtusername.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtusername.ClientID %>').value.replace(/^\s+|\s+$/g, '') != '' && !checkemail1(document.getElementById('<%=txtusername.ClientID %>').value)) {
                alert('Please enter valid email address.');
                document.getElementById('<%=txtusername.ClientID %>').value = '';
                document.getElementById('<%=txtusername.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtpassword.ClientID %>').value.replace(/^\s+|\s+$/g, '') == '') {
                alert('Please enter password.');
                document.getElementById('<%=txtpassword.ClientID %>').value = '';
                document.getElementById('<%=txtpassword.ClientID %>').focus();
                return false;
            }
            return true;
        }

        function checkfieldsforForgotpwd() {

            if (document.getElementById('<%=txtEmail.ClientID %>').value == '') {
                alert('Please enter email address.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
                alert('Please enter valid email address.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
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
    </script>
    <div class="breadcrumbs">
        <a href="/" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span></div>
        <div class="static-big-main" style="text-align: center;">
            <asp:Panel ID="pnlLogin" runat="server" Style="text-align: center;" DefaultButton="btnLogin">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" style="margin-bottom: 10px;">
                    <tbody>
                        <tr>
                            <td width="33%" align="left" valign="top" class="td-broder">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                    <tbody>
                                        <tr>
                                            <th colspan="2">
                                                Returning Customers
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                Enter your user name and password below to sign into your <strong>
                                                    <%=storePath.ToString() %></strong> account.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-right: 4px;">
                                                User Name :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtusername" runat="server" EnableViewState="false" CssClass="login-field"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                Password :
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtpassword" runat="server" CssClass="login-field" EnableViewState="false"
                                                    TextMode="Password"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="btnLogin" runat="server" alt="LOGIN" title="LOGIN" ImageUrl="/images/login.png"
                                                    OnClick="btnLogin_Click" OnClientClick="return checkfieldsforlogin();" />
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td colspan="2" align="center">
                                                <table cellpadding="0" cellspacing="0" align="center">
                                                    <tr>
                                                        <td style="padding: 0px;">
                                                            <a href="javascript:void(0);" title="Facebook" onclick="return ForLoginPopupFacebook();">
                                                                <img src="/images/fb-button.jpg" style="padding-right: 6px;" alt="Facebook" title="Facebook"
                                                                    class="img-left" /></a> <a href="javascript:void(0);" title="Google" onclick="return ForLoginGoogle();">
                                                                        <img src="images/google-button.jpg" width="80" height="29" alt="Google" title="Google"
                                                                            class="img-left" /></a> <a href="javascript:void(0);" title="Twitter" onclick="return ForLoginTwitter();">
                                                                                <img src="images/twitter-button.jpg" style="padding-left: 6px" width="80" height="29"
                                                                                    alt="Twitter" title="Twitter" class="img-left" /></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="30" align="left" valign="middle">
                                                &nbsp;
                                            </td>
                                            <td height="30" align="left" valign="middle">
                                                <asp:LinkButton ID="lkbForgetpwd" ToolTip="Forgot your password" runat="server" Text="Forgot your password?"
                                                    OnClick="lkbForgetpwd_Click">Forgot your password?</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td width="10">
                                &nbsp;
                            </td>
                            <td width="32%" id="pnlGuest" runat="server" align="left" valign="top" class="td-broder">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Continue Checkout as a Guest Customers
                                            </th>
                                        </tr>
                                        <tr>
                                            <td height="75">
                                                You are not required to have an account to Shop with <strong>
                                                    <%=storePath.ToString() %></strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="middle">
                                                <a href="/checkoutcommon.aspx" runat="server" id="GuestCheckOut" title="GUEST CHECKOUT">
                                                    <img src="/images/guest-checkout.png" alt="GUEST CHECKOUT" />
                                                </a>
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
                            <td width="10">
                                &nbsp;
                            </td>
                            <td width="33%" id="tdCreateAcc" runat="server" align="left" valign="top" class="td-broder">
                                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                    <tbody>
                                        <tr>
                                            <th>
                                                Creating an Account
                                            </th>
                                        </tr>
                                        <tr>
                                            <td height="75">
                                                Creating an account provides access to tools including saved addresses, order history
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="middle">
                                                <asp:ImageButton ID="btnCreateAccount" runat="server" alt="CREATE AN ACCOUNT" title="CREATE AN ACCOUNT"
                                                    ImageUrl="/images/create-an-account.png" OnClick="btnCreateAccount_Click" />
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
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlForgotPassword" runat="server" Visible="false" Style="text-align: center;"
                DefaultButton="btnSubmit">
                <table border="0px" align="center" width="100%" style="margin-bottom: 10px; float: left;">
                    <tr>
                        <td>
                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td width="33%" align="left" valign="top">
                                        </td>
                                        <td width="33%" align="left" valign="top" class="td-broder">
                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                                <tr>
                                                    <th colspan="2">
                                                        User Verification
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="line-height: 20px; border: 0px;">
                                                        You can recover your lost account information using the form below. Please enter
                                                        your valid E-Mail Address (the one you used for registration),your account information
                                                        will be mailed to you shortly.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Email :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail" EnableViewState="false" runat="server" CssClass="login-field"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnSubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="~/images/submit.png"
                                                            OnClick="btnSubmit_Click" OnClientClick="return checkfieldsforForgotpwd();" />
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="btnCancel" runat="server" alt="CANCEL" title="CANCEL" ImageUrl="~/images/cancel.png"
                                                            OnClick="btnCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="33%" align="left" valign="top">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
