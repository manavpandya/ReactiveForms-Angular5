<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditShadeMarkup.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.EditShadeMarkup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                return true;
            return false;
        }

        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

       <%-- $(function () {
            $('#txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });--%>


        function SearchValidation() {

            if (document.getElementById('txtShadeMarkUp').value == '') {
                jAlert('Please Enter Shade MarkUp Value.', 'Required Information', 'txtShadeMarkUp');
                return false;
            }


            return true;
        }
    </script>
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            <asp:Label ID="lblPagetitle" Text="Shade MarkUp" runat="server"></asp:Label>
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td style="height: 10px;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr style="height: 30px;">
                               
                                <td valign="middle" align="center">
                                 Shade MarkUp:
                                </td>
                                <td valign="middle" align="left">
                                    <asp:TextBox ID="txtShadeMarkUp"  MaxLength="10" runat="server" class="order-textfield" ReadOnly="false"
                                                                                                        Width="80px" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                </td>
                                
                                <td valign="middle" align="left">
                                    <asp:ImageButton ID="btnSaveValue" runat="server" OnClick="btnSaveValue_Click" OnClientClick="return SearchValidation();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px;">
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>