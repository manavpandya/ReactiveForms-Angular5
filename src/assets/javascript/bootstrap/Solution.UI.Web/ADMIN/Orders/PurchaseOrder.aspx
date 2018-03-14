<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrder.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.PurchaseOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function checkvendor() {
            if (document.getElementById('ddlVendor') && document.getElementById('ddlVendor').options[document.getElementById('ddlVendor').selectedIndex].value == 0) {
                alert("Please select Vendor for Preview.");
                document.getElementById('ddlVendor').focus();
                return false;
            }
            return true;
        }

        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);

            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;

            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 104)
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

        function changesubtotal() {
            var tables = document.getElementById('gvCart');
            var trs = tables.getElementsByTagName('tr');
            for (var j = 0; j < trs.length; j++) {
                var theCells = trs[j].getElementsByTagName("td");
                if (theCells.length > 0) {
                    var txtq = theCells[2].getElementsByTagName("input")[0];
                    var txtp = theCells[3].getElementsByTagName("input")[0];
                    var spanr = theCells[4].getElementsByTagName("span")[0];

                    var st = 0;
                    var q = parseFloat(txtq.value);
                    var p = parseFloat(txtp.value);
                    st = p * q;
                    spanr.innerHTML = st.toFixed(2);
                }
            }
        }
    </script>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr>
                <th>
                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                        Generate Warehouse Purchase Order
                    </div>
                    <div class="main-title-right">
                        <a id="AtagBack" runat="server" style="margin-right: 60px;">
                            <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Purchase Order"
                                title="Go to Warehouse PO" />
                        </a>
                    </div>
                </th>
            </tr>
            <tr>
                <td style="padding: 2px">
                    <asp:Literal ID="litProducts" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="padding: 2px">
                    <div id="rdolist" style="border: 5px sollid #e7e7e7;">
                        <asp:GridView ID="grdCart" runat="server" AutoGenerateColumns="False" Width="100%"
                            class="order-table" Style="border: solid 1px #e7e7e7" OnRowDataBound="gvCart_RowDataBound">
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <EmptyDataTemplate>
                                <label style="color: Red; text-align: center;">
                                    No Record(s) Found !</label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Select" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                        <%--<asp:Label ID="lblPurchaseOrderQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PurchaseOrderQty") %>'></asp:Label>--%>
                                        <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        <asp:Label ID="lblQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                        <input type="hidden" id="vname" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>' />
                                        <input type="hidden" id="vvalue" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>' />
                                        <%--<asp:Label ID="lblShippingCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ShippingCartID") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <HeaderTemplate>
                                        &nbsp;Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="50%" HorizontalAlign="left" />
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <HeaderTemplate>
                                        &nbsp; SKU
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle />
                                    <ItemTemplate>
                                        <%-- <asp:TextBox ID="txtQuantity" Text=' <%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Quantity").ToString())- Convert.ToInt32(DataBinder.Eval(Container.DataItem,"PurchaseOrderQty").ToString()) %>'
                                            runat="server" Width="70px" onblur="changesubtotal();" Style="text-align: center"
                                            MaxLength="6" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtQuantity" class="aspNetDisabled from-textfield hasDatepicker"
                                            Text=' <%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Quantity").ToString()) %>'
                                            runat="server" Width="70px" onblur="changesubtotal();" Style="text-align: center"
                                            MaxLength="4" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                        <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" onblur="changesubtotal();"
                                            ID="txtPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SalePrice") %>'
                                            Width="80px" Style="text-align: center; display: none"></asp:TextBox>
                                        <asp:Label ID="lblSubTotal" runat="server" Text='<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem,"Quantity").ToString())*Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"SalePrice").ToString())).ToString("f2") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price">
                                    <HeaderTemplate>
                                        Price&nbsp;
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        $<%#    string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "SalePrice"))%>
                                        <asp:Label ID="lblPrice" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SalePrice") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%" HorizontalAlign="right" />
                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                            <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                    <table cellpadding="1" cellspacing="2" width="100%">
                        <tr>
                            <td style="width: 100px;">
                                Vendor :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="order-list" Width="250px"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mail Template :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMailTemplate" runat="server" CssClass="order-list" Width="250px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Special Instructions:
                            </td>
                            <td valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="ckeditor-table">
                                            <asp:TextBox TextMode="multiLine" class="order-list" ID="txtDescription" TabIndex="23"
                                                Width="248px" Height="55px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnPreview" runat="server" OnClientClick="return checkvendor();"
                                    OnClick="btnPreview_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
