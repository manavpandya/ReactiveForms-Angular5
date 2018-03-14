<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbandonedShoppingCartitem.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Customers.AbandonedShoppingCartitem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Abandoned ShoppingCart Item</title>
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
    <script type="text/javascript">
        function chkselect(ids) {
            var allElts = document.getElementById("rdolist").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.id == ids) {

                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }

            return true;

        }
        function chkselect1(ids) {
            var allElts = document.getElementById("Div1").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.id == ids) {

                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }

            return true;

        }
        function chkselectProduct() {
            var allElts = document.getElementById("rdolist").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.checked == true) {

                        Chktrue++;
                    }

                }
            }

            if (Chktrue == 0) {
                alert('Please select product')
                return false;
            }
            return true;

        }
        function checkSearch() {

            if (document.getElementById("txtSearch").value == '') {
                alert('Please enter search keyword');
                document.getElementById("txtSearch").focus();
                return false;

            }
            return true;
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
                    <td style="text-align: right;">
                        <img id="imgMainDiv" runat="server" onclick="javascript:var result=window.opener.location.href;result =result.replace('potab=1','');result =result+'&potab=1'; window.opener.location.href=result; window.close();"
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
                            Shopping Cart Item(s)
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
