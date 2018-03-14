<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssemblerProductPopUp.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.AssemblerProductPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none">
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

        function keyRestrictforIntOnly(e, validchars) {
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

        function AddInventoryproduct(rowindex) {

            if (document.getElementById('grdProducts_txtQty_' + rowindex) != null) {

                var inventoryhtml = document.getElementById('grdProducts_lblInventory_' + rowindex).innerHTML;
                var qty = document.getElementById('grdProducts_txtQty_' + rowindex).value;
                if (parseInt(qty) > parseInt(inventoryhtml)) {
                    document.getElementById('grdProducts_txtQty_' + rowindex).value = inventoryhtml;
                }
                else {

                }
            }
        }
    </script>
    <script type="text/javascript">
        function loadVendorsku() {
            var id = window.opener.document.getElementById('<%=Request.QueryString["mode"]%>').value;
            var allSKu = ',' + id + ',';
            document.getElementById('<%= txtproductsku.ClientID %>').value = ',' + id + ',';
            var allCheckbox = document.getElementById('grdProducts').getElementsByTagName('input');
            for (var i = 0; i < allCheckbox.length; i++) {
                var allExists = allCheckbox[i];
                if (allExists.id.indexOf('_hdnVendorSKUID1_') > -1 && allSKu.indexOf(',' + allExists.value.replace(/^\s*\s*$/g, '') + ',') > -1) {
                    var checkboxid = allExists.id.replace('_hdnVendorSKUID1_', '_chkSelect_');

                    // document.getElementById(checkboxid).checked = true;
                }

            }
        }
    </script>
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/css/style.css"></script>
    <div id="dvProduct" runat="server">
        <table width="100%" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" class="order-table1">
            <tr>
                <td style="width: 8%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtFeaturesystem" CssClass="order-textfield" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 30%">
                    <asp:ImageButton ID="ibtnProductsearch" runat="server" ImageUrl="~/App_Themes/Gray/images/search.gif"
                        OnClientClick="return fvalidation();" OnClick="ibtnProductsearch_Click" />&nbsp;
                    <asp:ImageButton ID="ibtnProductshowall" runat="server" ImageUrl="~/App_Themes/Gray/images/showall.png"
                        OnClick="ibtnProductshowall_Click" />
                </td>
                <td style="width: 40%" align="center">
                    <asp:ImageButton ID="ibtnProductaddtoselectionlist" runat="server" ImageUrl="~/App_Themes/Gray/images/add-to-selection-list.png"
                        OnClick="ibtnProductaddtoselectionlist_Click" OnClientClick="return fcheckCount();" />&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnProductclose" runat="server" ImageUrl="~/App_Themes/Gray/images/pclose.png"
                        OnClientClick="return closeWin();" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display: none;">
                    (Max. 3 Vendor Select)
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblProducterror" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr id="trAddProducts1" runat="server" visible="false">
                <td colspan="4" align="left">
                    <b>Note :</b> Only Active Products with Available Inventory will be Listed.
                </td>
                <%--<td align="right" colspan="2" style="padding-right: 10px;">
                    <asp:Button ID="btnAddProducts2" runat="server" OnClientClick="return chkSelect();" Visible="false" />
                </td>--%>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="True"
                        PageSize="20" OnPageIndexChanging="grdProducts_PageIndexChanging" OnRowDataBound="grdProducts_RowDataBound"
                        CellPadding="2" CellSpacing="1" BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    Select
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("ProductID").ToString() == "True" ? "True": "False") %>' />
                                    <asp:HiddenField ID="hdnVendorSKUID" runat="server" Value='<%#Eval("ProductID") %>' />
                                    <asp:HiddenField ID="hdnVendorSKUID1" runat="server" Value='<%#Eval("SKU") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("Name")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assign Qty">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" Style="text-align: center;" Text="0" Width="40px"
                                        onkeypress="return keyRestrictforIntOnly(event,'0123456789');" CssClass="order-textfield"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inventory" Visible="True">
                                <ItemTemplate>
                                    <asp:Label ID="lblInventory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                        <AlternatingRowStyle CssClass="altrow" />
                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                    </asp:GridView>
                    <div style="display: none;">
                        <asp:TextBox ID="txtproductsku" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        function fvalidation() {
            var a = document.getElementById('<%=txtFeaturesystem.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message', '<%=txtFeaturesystem.ClientID %>'); });
                return false;
            }
            return true;
        }
        function closeWin() {
            window.close();
        }
    </script>
    <script language="javascript" type="text/javascript">
        function fcheckCount() {

            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                        var textboxid = myform.elements[i].id.replace('_chkSelect_', '_txtQty_');
                        var lblboxid = myform.elements[i].id.replace('_chkSelect_', '_lblInventory_');

                        if (document.getElementById(textboxid) != null && (document.getElementById(textboxid).value == "" || document.getElementById(textboxid).value == "0")) {

                            $(document).ready(function () { jAlert('Please enter valid quantity!', 'Message', textboxid); });
                            return false;
                        }
                    }
                }
            }

            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Record!', 'Message'); });
                return false;
            }
            //            else if (count > 3) {
            //                $(document).ready(function () { jAlert('Check Max. Three Record!', 'Message'); });
            //                return false;
            //            }
            return true;
        }
    </script>
    </form>
</body>
</html>
