<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintProductBarcode.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.PrintProductBarcode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/App_Themes/Gray/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">

        function validation() {
            if (document.getElementById("txtName") != null && document.getElementById("txtName").value.replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Name.');
                document.getElementById("txtName").focus();
                return false;
            }
            return true;
        }


        function checkQuantity() {
            //alert('1' + document.getElementById("txtQuantity").value);
            var qty = document.getElementById("txtQuantity").value;
            if (qty == '') {
                alert('Please enter Quantity');
                document.getElementById("txtQuantity").focus();
                return false;
            }
            else if (qty <= 0) {
                alert('Please enter valid Quantity');
                document.getElementById("txtQuantity").focus();
                return false;
            }
            return true;
        }

        function keyRestrictForInventory(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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

        function PrintBarcode(quantity) {
            if (document.getElementById('divBarcodePrint')) {
                var BrowserName = navigator.appName.toString();

                if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                    w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=10,Width=10,left=0,top=0,visible=false,alwaysLowered=yes');

                    w.document.write(document.getElementById("divBarcodePrint").innerHTML);
                    //  w.document.write(document.getElementById("trBarcode").innerHTML);
                    w.document.close();
                    w.print();
                    w.close();
                }
                else {
                    var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                    pri.document.open();
                    var contentAll = document.getElementById("divBarcodePrint").innerHTML;

                    //var contentAll = document.getElementById("trBarcode").innerHTML;
                    //for (var i = 0; i < quantity; i++) {
                    pri.document.write(contentAll);
                    //}
                    pri.document.close();
                    pri.focus();
                    pri.print();
                }
            }
            return false;
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table style="margin-top: 5px; float: left;" width="99%">
            <tr>
                <td style="width: 5%">
                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td align="left" colspan="5">
                                <b>Print bar-code Inventory label(2" x 2" labels)</b>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 10%">
                                Store :&nbsp;
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="false"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list"
                                    Style="margin-left: 0px">
                                </asp:DropDownList>
                            </td>
                            <td align="right" style="width: 5%">
                                Name&nbsp;or&nbsp;SKU&nbsp;or&nbsp;UPC
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="order-textfield"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 5%">
                                <asp:Button ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />
                            </td>
                        </tr>
                        <tr id="trSku" runat="server" visible="false">
                            <td align="right" style="width: 5%">
                                SKU :&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSku" runat="server" CssClass="order-list" Width="185px">
                                </asp:DropDownList>
                            </td>
                            <td align="right" style="width: 5%">
                                Quantity :&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="2" CssClass="order-textfield"
                                    Width="40px" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"></asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton ID="btnPrintBarcode" runat="server" Text="Print Barcode" OnClick="btnPrintBarcode_Click"
                                    OnClientClick="return checkQuantity();" />
                            </td>
                        </tr>
                        <tr id="trBarcode" runat="server" visible="false" align="center">
                            <td align="center" colspan="5">
                                <div id="divBarcodePrint">
                                    <asp:Literal ID="ltrbarcode" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint" width="210px"></iframe>
    </div>
    </form>
</body>
</html>
