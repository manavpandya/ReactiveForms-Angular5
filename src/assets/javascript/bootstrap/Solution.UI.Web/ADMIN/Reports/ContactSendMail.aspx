<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactSendMail.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Reports.ContactSendMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
    <script type="text/javascript">    
        function ValidatePage() {
            if (document.getElementById('txtSubject') != null && document.getElementById('txtSubject').value == '') {
                //alert('Please Enter Subject.');
                jAlert('Please Enter Subject.', 'Message', 'txtSubject');
                //document.getElementById('txtSubject').focus();
                return false;
            }
            if (document.getElementById('txtmsgbody') != null && document.getElementById('txtmsgbody').value == '') {
                //alert('Please Enter Message.');
                jAlert('Please Enter Message.', 'Message', 'txtmsgbody');
                //document.getElementById('txtmsgbody').focus();
                return false;
            }
            document.getElementById("prepage").style.display = '';
            return true;
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
   
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_border">
        <tr>
            <td colspan="3" valign="middle">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="logo" width="80%">
                            <a href="#" title="Cash Register">
                                <img src="/App_Themes/<%=Page.Theme %>/images/logo.png"
                                  style="float: left; padding: 10px 0 0 10px;" /></a>
                        </td>
                        <td align="right" width="20%" valign="top" style="padding-right: 10px; padding-top: 10px;">
                            <a href="javascript:window.close();" title="Close" class="close">
                                <img src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" alt="Close" style="border: 0px"
                                    title="Close" /></a>
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
            <td colspan="2" align="left" style="color: #fff; height: 28px; background: #7d7d7d;
                border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                font-size: 12px; font-weight: bold;">
                <strong>Reply to Inquiry</strong>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td colspan="2" align="center" style="padding-top: 20px; color: Red; font-weight: bold;
                            font-size: 12px;">
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2" style="padding-left: 10px; padding-top: 5px; padding-bottom: 10px;
                            line-height: 1.6em; font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <div id="Email" runat="server">
                                <table style="color: #000000; width: 100%; font-size: 12px;" cellpadding="5" cellspacing="2">
                                    <tr style="text-align: left">
                                        <td align="left">
                                            Subject :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtSubject" runat="server" Width="400px" Style="border: 1px solid #BCC0C1;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="text-align: left">
                                        <td valign="top" align="left">
                                            Your Reply :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtmsgbody" runat="server" TextMode="MultiLine" Width="500px" Height="100px"
                                                Style="border: 1px solid #BCC0C1;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnSubmit" runat="server" CssClass="button" OnClientClick="return ValidatePage()"
                                                OnClick="btnSubmit_onclick" ToolTip="Send Email" AlternateText="Send Email" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
