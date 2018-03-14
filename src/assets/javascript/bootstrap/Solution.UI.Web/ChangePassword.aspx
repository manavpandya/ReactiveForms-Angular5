<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="Solution.UI.Web.changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>
    <script type="text/javascript">
        function checkfields() {

            if (document.getElementById('<%=txtOldPassword.ClientID %>').value == '') {
                alert('Please enter Old Password.');
                document.getElementById('<%=txtOldPassword.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtpassword.ClientID %>').value == '') {
                alert('Please enter New Password.');
                document.getElementById('<%=txtpassword.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtpassword.ClientID %>').value.length < 6) {
                alert('Password length must be at least 6 character long.');
                document.getElementById('<%=txtpassword.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtconfirmpassword.ClientID %>').value == '') {
                alert('Please enter Confirm Password.');
                document.getElementById('<%=txtconfirmpassword.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtpassword.ClientID %>').value != document.getElementById('<%=txtconfirmpassword.ClientID %>').value) {
                alert('Confirm Password must be match with Password.');
                document.getElementById('<%=txtconfirmpassword.ClientID %>').focus();
                return false;
            }
            return true;
        }

    </script>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> > <span>
                <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal>
            </span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                        </span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a href="MyAccount.aspx" style="color: #B92127; text-decoration: underline;"
                           title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a> </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <asp:Panel ID="PnlChangePass" runat="server" DefaultButton="btnsubmit">
                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table-none"
                        id="mainTable">
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
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" Style="border: 0px;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">*</span>Old Password
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="77%">
                                    <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server"
                                        CssClass="contact-fild" OnPreRender="txtOldPassword_PreRender"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">*</span>New Password
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="77%">
                                    <asp:TextBox ID="txtpassword" TextMode="Password" runat="server" CssClass="contact-fild"
                                        OnPreRender="txtpassword_PreRender"></asp:TextBox>
                                    <img src="/images/help.jpg" alt="Password length must be at least 6 character long"
                                        title="Password length must be at least 6 character long" align="absmiddle" />
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">*</span>Confirm Password
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="77%">
                                    <asp:TextBox ID="txtconfirmpassword" TextMode="Password" runat="server"
                                        CssClass="contact-fild" OnPreRender="txtconfirmpassword_PreRender"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    &nbsp;
                                </td>
                                <td valign="middle" align="left">
                                    <asp:ImageButton ID="btnsubmit" runat="server" ToolTip="SUBMIT" ImageUrl="/images/submit.png"
                                        OnClientClick="return checkfields()" OnClick="btnsubmit_Click" />
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="btnCancel" runat="server" ToolTip="CANCEL" ImageUrl="/images/cancel.png"
                                        OnClientClick="javascript:window.location.href='/MyAccount.aspx';return false;" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="3">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
