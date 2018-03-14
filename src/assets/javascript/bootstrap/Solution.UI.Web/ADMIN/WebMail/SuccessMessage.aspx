<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuccessMessage.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.WebMail.SuccessMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div id="content-width">
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Mail Log" alt="Mail Log" src="/App_Themes/<%=Page.Theme %>/icon/report_icon.gif" />
                                                    <h2>
                                                        Success Message</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table cellpadding="0" cellspacing="0" align="center">
                                                    <tr>
                                                        <td align="left" style="font-family: Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-size: 14px; color: Green">
                                                            <br />
                                                            Mail has been sent Successfully.<br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnsuccess" runat="server" CssClass="button" Text="Back to Inbox"
                                                                OnClick="btnsuccess_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
