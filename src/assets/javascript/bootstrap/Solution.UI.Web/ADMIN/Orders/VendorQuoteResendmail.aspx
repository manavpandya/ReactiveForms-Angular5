<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorQuoteResendmail.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.VendorQuoteResendmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr>
                <td style="border: 1px solid #e7e7e7;">
                    <table cellpadding="1" cellspacing="2" width="100%">
                        <tr valign="middle">
                            <td style="width: 60px; height: 30px;">
                                <span class="star">*</span>Email :
                            </td>
                            <td valign="middle" style="width: 225px; height: 30px;">
                                <asp:TextBox ID="txtFrom" CssClass="order-textfield" runat="server"></asp:TextBox>
                            </td>
                            <td valign="middle" align="left">
                                <asp:ImageButton ID="btnSendReciept" runat="server" Text="Send Mail" CssClass="button"
                                    OnClientClick="if(document.getElementById('txtFrom').value==''){alert('Please enter E-Mail.');document.getElementById('txtFrom').focus();return false;}"
                                    OnClick="btnSendReciept_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2" align="left">
                                <asp:Literal ID="ltDesc" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="display: none">
            <asp:Literal ID="ltsubject" runat="server"></asp:Literal>
            <asp:Literal ID="ltEmail" runat="server"></asp:Literal>
            <asp:Literal ID="ltsfrom" runat="server"></asp:Literal>
        </div>
    </div>
    </form>
</body>
</html>
