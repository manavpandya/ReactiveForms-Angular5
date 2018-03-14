<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPasswordSend.aspx.cs" Inherits="Solution.UI.Web.ForgotPasswordSend" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" src="js/CreateAccountValidate.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/jquery-alerts-main.js"></script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
    <script src="/js/jquery-alerts.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkEmailAd() {
            document.getElementById('popup_container').style.visibility = 'visible';
        }
        function checyes() {
            document.getElementById("ContentPlaceHolder1_hdnyes").value = '1';
            document.getElementById("ContentPlaceHolder1_Button2").click();
        }


    </script>
    <div id="popup_container" style="position: fixed; visibility: hidden; z-index: 1000000; padding: 0px; margin: 0px; min-width: 604px; max-width: 604px; top: 275px;left: 487.5px;">
        <h1 id="popup_title">Confirmation</h1>
        <div id="popup_content" class="confirm2">
            <div id="popup_message" style="color:#641114">We have sent you a new password to your registered email address. Please check your mail and login to continue.</div>
            <div id="popup_panel"><a href="javascript:void(0);" id="popup_ok" onclick="javascript:checyes();" style="margin-left: 0;"><strong><span style="margin-left: 15px; margin-right: 15px;">Ok</span></strong></a> &nbsp;&nbsp;&nbsp; </div>
        </div>
    </div>

    <script type="text/javascript">
        function checkfields() {

            if (document.getElementById('<%=txtshowcode.ClientID %>').value == '') {
                alert('Please Enter Shown Code.');
                document.getElementById('<%=txtshowcode.ClientID %>').focus();
                return false;
            }
            return true;
        }

    </script>
    <script type="text/javascript">
        function ReloadCapthca() {
            document.getElementById('ContentPlaceHolder1_txtshowcode').value = '';

            var chars = "0123456789";
            var string_length = 8;
            var randomstring = '';
            for (var i = 0; i < string_length; i++) {
                var rnum = Math.floor(Math.random() * chars.length);
                randomstring += chars.substring(rnum, rnum + 1);
            }

            document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
            document.getElementById('ContentPlaceHolder1_txtshowcode').focus();
        }

        function checkfieldsforForgotpwd() {

            if (document.getElementById('<%=lblEmail.ClientID %>').value == '') {
                 alert('Please enter email address.');
                 document.getElementById('<%=lblEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=lblEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=lblEmail.ClientID %>').value)) {
                alert('Please enter valid email address.');
                document.getElementById('<%=lblEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtshowcode.ClientID %>').value == '') {
                alert('Please enter ShownCode.');
                document.getElementById('<%=txtshowcode.ClientID %>').focus();
                 return false;
             }
        return true;
    }
    </script>
    <div style="display: none">
        <input type="hidden" id="hdnyes" runat="server" value="0" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
    </div>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">My Account</a> > <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal>
        </span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <asp:Panel ID="pblforgot" runat="server" DefaultButton="btnsubmit">
                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table-none">
                        <tbody>
                            <tr>
                                <td align="right" colspan="3">
                                    <span class="required-red">*</span>Required Fields
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">&nbsp;&nbsp;</span>E-Mail
                                </td>
                                <td>:
                                </td>
                                <td>
                                   <%-- <asp:Label ID="lblEmail" runat="server"></asp:Label>--%>
                                    <asp:TextBox ID="lblEmail" EnableViewState="false" CssClass="login-field" Visible="true" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">&nbsp;&nbsp;</span>Verification
                                </td>
                                <td width="3%">:
                                </td>
                                <td width="77%">
                                    <img width="150px" height="40px" class="img-left" id="imgcapcha" alt="" src="/JpegImage.aspx?id=343343" />
                                    <%-- <a title="Reload" href="#" class="reload-icon" onclick="ReloadCapthca();">
                                                    <img class="reload-btn" title="Reload" alt="Reload" src="/images/reload-icon.png" /></a>--%>
                                    <input type="button" value="" title="Reload" id="btnreload" style="background: url(/images/reload-icon.jpg) no-repeat transparent; width: 31px; height: 29px; border: none; cursor: pointer; margin: 8px 0 0 5px;"
                                        onclick="ReloadCapthca();" />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">*</span>&nbsp;Enter Code Shown
                                </td>
                                <td width="3%">:
                                </td>
                                <td width="77%">
                                    <asp:TextBox ID="txtshowcode" runat="server" Width="120px" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">&nbsp;
                                </td>
                                <td valign="middle" align="left">
                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                        OnClientClick="return checkfieldsforForgotpwd();" OnClick="btnsubmit_Click" />
                                    &nbsp;&nbsp; <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ImageUrl="/images/cancel.png" OnClick="btnCancel_Click" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="3"></td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>