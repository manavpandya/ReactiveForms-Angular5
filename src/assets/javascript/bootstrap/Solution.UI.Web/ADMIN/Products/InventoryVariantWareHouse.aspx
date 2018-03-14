<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryVariantWareHouse.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.InventoryVariantWareHouse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/App_Themes/Gray/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">

        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }

        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'radio') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                window.parent.jAlert('Select at least one Warehouse Preferred Location!', 'Message');
                return false;
            }
            else {
                return ChkLockQuantity();
            }
        }

        function ChkLockQuantity() {
            var allElets = document.getElementById("grdWarehouse").getElementsByTagName("input")
            //grdWarehouse_txtLockQuantity
            var i;
            var TotalQty = 0;
            var LockQty = parseInt(document.getElementById("grdWarehouse_txtLockQuantity").value.replace(/^\s*\s*$/g, ''));
            for (var i = 0; i < allElets.length; i++) {
                var elt = allElets[i];
                if (elt.type == "text" && elt.id != "grdWarehouse_txtLockQuantity") {
                    if (document.getElementById(elt.id).value.replace(/^\s*\s*$/g, '') != "") {
                        TotalQty = parseInt(TotalQty) + parseInt(document.getElementById(elt.id).value.replace(/^\s*\s*$/g, ''));
                    }
                }
            }
            if (LockQty == "") {
                LockQty = 0;
            }
            if (TotalQty == "") {
                TotalQty = 0;
            }
//            if (parseInt(LockQty) > parseInt(TotalQty)) {
//                window.parent.jAlert("Total Quantity can not be greater than Lock Quantity.", "Message");
//                document.getElementById("grdWarehouse_txtLockQuantity").focus();
//                return false;
//            }
            return true;
        }
        
    </script>
    <script type="text/javascript">
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
        function SetTotalWeight() {

            var totalWeight = 0;
            var Weight = 0;
            for (var i = 0; i < 20; i++) {
                if (document.getElementById('grdWarehouse_txtInventory_' + i)) {
                    if (document.getElementById('grdWarehouse_txtInventory_' + i).value == '') {
                        Weight = 0;
                    }
                    else {
                        Weight = parseInt(document.getElementById('grdWarehouse_txtInventory_' + i).value);
                    }
                    totalWeight += Weight;
                }
            }
            document.getElementById('grdWarehouse_lblTotal').innerHTML = totalWeight;

        }


    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server" defaultbutton="btnSave">
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr style="background-color: rgb(68, 68, 68); height: 25px;">
                <td align="left" valign="middle" style="color: rgb(255, 255, 255); font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Warehouse Inventory
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupviewdetailclose" runat="server" OnClientClick="javascript:window.parent.disablePopup(); return false;"
                        Style="margin: 5px 5px 0 0;" />
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <asp:GridView ID="grdWarehouse" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                        BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                        CssClass="content-table" Width="100%" DataKeyNames="WareHouseID" EmptyDataText="No Records Found!"
                        RowStyle-ForeColor="Black" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                        AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                        ViewStateMode="Enabled" ShowHeaderWhenEmpty="true" OnRowDataBound="grdWarehouse_RowDataBound"
                        ShowFooter="True">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Preferred&nbsp;Location</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rdowarehouse" runat="server" OnClick="javascript:SelectSingleRadiobutton(this.id)"
                                        GroupName="WarehouseSelect" />
                                    <asp:Label ID="lblPreferredLocation" runat="server" Visible="false" Text='<%#Bind("PreferredLocation") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                <FooterTemplate>
                                    &nbsp;
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Warehouse Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblWarehouse" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="80%"></ItemStyle>
                                <FooterTemplate>
                                    <b>Partner Hemming Qty:&nbsp;</b><asp:TextBox ID="txtLockQuantity" CssClass="order-textfield"
                                        Style="width: 50px; text-align: center;" runat="server" onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');return ret;"
                                        MaxLength="5" TabIndex="7"></asp:TextBox>&nbsp;&nbsp;&nbsp; <b>Total Inventory:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" CssClass="footerBorder" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inventory">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtInventory" CssClass="order-textfield" Style="width: 50px; text-align: center;"
                                        runat="server" Text='<%#Bind("Inventory") %>' onKeyPress="var ret=keyRestrictForInventory(event,'0123456789');SetTotalWeight();return ret;"
                                        onblur="SetTotalWeight();" MaxLength="5" TabIndex="7" onkeyup="SetTotalWeight();"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="true" CssClass="footerBorderinventory" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WarehouseID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWarehouseID" runat="server" Text='<%#Bind("WarehouseID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" BackColor="White" />
                        <AlternatingRowStyle CssClass="altrow" BackColor="#FBFBFB" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding: 5px; text-align: left;">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left">
                                Allow Quantity Availability:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtallowmsg" runat="server" CssClass="order-textfield" Style="width: 400px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 5px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td align="left">
                                Lock Quantity Availability:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtlockmsg" runat="server" CssClass="order-textfield" Style="width: 400px;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding: 5px; text-align: center;">
                    <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return checkCount();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
