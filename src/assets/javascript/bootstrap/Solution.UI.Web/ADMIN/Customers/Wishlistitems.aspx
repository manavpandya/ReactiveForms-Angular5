<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wishlistitems.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.Wishlistitems" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Wish List Cart</title>
    <style type="text/css" media="print">
        .Printinvoice
        {
            display: none;
        }
    </style>
    <link href="../../App_Themes/gray/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
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
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <td>
                        <img id="imgLogo" runat="server" />
                    </td>
                    <td style="text-align: right;" class="Printinvoice">
                        <img id="imgMainDiv" runat="server" onclick="javascript:window.close();"
                            class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png"
                            style="cursor: pointer" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding: 5px;">
                    </td>
                </tr>
                <tr>
                    <th colspan="2">
                        <div class="main-title-left" style="color: #fff; text-align: left;">
                           Wish List Item(s)
                        </div>
                    </th>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%">
                        <asp:Literal ID="ltCart" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="Printinvoice">
                    <td>
                        <asp:ImageButton ImageUrl="/App_Themes/<%=Page.Theme %>/images/print.png" runat="server"
                            ID="btnPrint" OnClientClick="javascript:window.print();" OnClick="btnPrint_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div>
    </div>
    </form>
</body>
</html>
