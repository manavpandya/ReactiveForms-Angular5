<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderHaming.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderHaming" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css" media="all">
        .table-none-border
        {
            border: 1px solid #ececec;
        }
        
        .datatable table
        {
            border: 1px solid #eeeeee;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable a
        {
            color: #C72E1A;
            text-decoration: none;
        }
        .datatable a:hover
        {
            color: #C72E1A;
            text-decoration: underline;
        }
        .datatable td
        {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
    </style>
    <script type="text/javascript">
        function AddHamingqty(rowindex) {
            if (document.getElementById('grdProducts_hdnHamingQtyariantvalue_' + rowindex) != null && document.getElementById('grdProducts_txtHamingQty_' + rowindex) != null) {
                var Orderqty = document.getElementById('grdProducts_hdnHamingQtyariantvalue_' + rowindex).value;
                var Hamingqty = document.getElementById('grdProducts_txtHamingQty_' + rowindex).value;
                if (Hamingqty == '' || (parseInt(Hamingqty) <= parseInt(0))) {
                    alert('Please enter valid quantity.');
                    document.getElementById('grdProducts_txtHamingQty_' + rowindex).value = Orderqty;
                }
                if (parseInt(Hamingqty) > parseInt(Orderqty)) {
                    alert('Please enter valid quantity.');
                    document.getElementById('grdProducts_txtHamingQty_' + rowindex).value = Orderqty;
                }
                else {
                }
            }
        }

        function keyRestrictEngraving(e, validchars) {
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

        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function ToggleText(chk, eleid) {
            var ele = document.getElementById(eleid);
            if (chk.checked) {
                ele.removeAttribute("disabled");
            }
            else {
                ele.setAttribute("disabled", "disabled");
                ele.value = "0";
            }
        }
    </script>
    <style type="text/css">
        .lblfont
        {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hfCustomer" runat="server" />
        <asp:HiddenField ID="hfGateway" runat="server" />
        <input type="hidden" id="hdnIsSalesManager" runat="server" value="0" />
        <table width="100%" class="table-none-border">
            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" CellPadding="5"
                        CellSpacing="0" ShowFooter="false" Width="100%" GridLines="Both" CssClass="table-noneforOrder"
                        OnRowDataBound="grdProducts_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Product Name
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label>
                                    <asp:Label ID="lblProductID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"RefProductID") %>'></asp:Label>
                                    <asp:Label ID="lblVariantNames" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                    <asp:Label ID="lblVariantValues" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                    <asp:Label ID="lblcustomcartid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <center>
                                        SKU
                                    </center>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <center>
                                        Upgrade SKU
                                    </center>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUpgradeSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKUupgrade") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <center>
                                        Ordered
                                        <br />
                                        Quantity</center>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" Width="9%" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <center>
                                        Hemming
                                        <br />
                                        Quantity</center>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtHamingQty" onkeypress="return keyRestrictEngraving(event,'1234567890.');"
                                        CssClass="from-textfield hasDatepicker" runat="server" Width="70px" Style="text-align: center;"></asp:TextBox>
                                    <asp:HiddenField ID="hdnHamingQtyariantvalue" runat="server" Value="0" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" Width="9%" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span style="text-align: Center;">Hemming Option</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlHamingName" CssClass="order-list" runat="server">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:ImageButton ID="imgupdatesku" runat="server" AlternateText="Update" OnClick="imgupdatesku_Click" />&nbsp;
                    <asp:ImageButton ID="imgupdatesku1" runat="server" AlternateText="Print Processing Order"
                        Visible="false" OnClick="imgupdatesku1_Click" />
                &nbsp;
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
