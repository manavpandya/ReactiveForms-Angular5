<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeRMAOrder.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.ChangeRMAOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change RMA Order </title>
    <style type="text/css" media="all">
        .lblfont {
            font-weight: bold;
            color: red;
        }

        .table-none-border {
            border: 1px solid #ececec;
        }

        .datatable table {
            border: 1px solid #eeeeee;
        }

        .datatable tr.alter_row {
            background-color: #f9f9f9;
        }

        .datatable a {
            color: #C72E1A;
            text-decoration: none;
        }

            .datatable a:hover {
                color: #C72E1A;
                text-decoration: underline;
            }

        .datatable td {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }

        .datatable th {
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
        function CheckAlert() {
            jConfirm1('Order Processed Successfully..', 'Success', function (r) {
                if (r == true) {
                    window.parent.location.href = window.parent.location.href;
                }
            });
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
            if (chk.checked)
                ele.removeAttribute("disabled");
            else
                ele.setAttribute("disabled", "disabled");
        }
        function dorefresh() {
            __doPostBack('btnUpdate', '');
        }

        function openCenteredCrossSaleWindow(x, lblid) {

            createCookie('prskus', document.getElementById(x).value, 1);
            var width = 1000;
            var height = 700;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = document.getElementById('HdnStoreID').value;
            var CustID = document.getElementById('HdnCustID').value;
            var rma = document.getElementById('HdnRMA').value;
            var pid = document.getElementById('HdnProductID').value;
            var ono = document.getElementById('HdnOrderNumber').value;
            var cartid = document.getElementById('HdnCartID').value;
            var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
            window.open('RMA_ProductSku.aspx?StoreID=' + StoreID + '&RMA=' +rma+ '&ONO='+ono+ '&ProductID='+pid+'&CartID='+cartid+'&clientid=' + x + "&lblID=" + lblid + "&CustID=" + CustID + "&subWind", "Mywindow", windowFeatures);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function ShoppingCartTotal() {
            var total = 0;
            if (document.getElementById('lblSubTotal').innerHTML != '') {
                total = parseFloat(document.getElementById('lblSubTotal').innerHTML);
            }
            if (document.getElementById('TxtShippingCost').value != '') {
                total += parseFloat(document.getElementById('TxtShippingCost').value);
            }
            if (document.getElementById('TxtTax').value != '') {
                total += parseFloat(document.getElementById('TxtTax').value);
            }
            if (document.getElementById('TxtDiscount').value != '') {
                total -= parseFloat(document.getElementById('TxtDiscount').value);
            }
            document.getElementById('lblTotal').innerHTML = parseFloat(total).toFixed(2);
            document.getElementById('hfTotal').value = document.getElementById('lblTotal').innerHTML;
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function loader() {
            chkHeight();
        }
    </script>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hfCustomer" runat="server" />
            <asp:HiddenField ID="hfGateway" runat="server" />
            <table width="100%" class="table-none-border">
                <tr>
                    <td align="left" style="padding: 10px;" colspan="2">Order # : <strong style="color: #4C4C4C;">
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>
                    </strong>
                    </td>
                    <%--  <td>
                    <a id="lnkOldOrders" runat="server" onclick="window.location=window.location;return false;"
                        class="lblfont" href="javascript:void(0);" style="float: right;">View Old Orders</a>
                </td>--%>
                </tr>
                <tr>
                    <td align="left" style="padding-bottom: 10px; text-align: right;" colspan="2">
                        <a href="javascript:void(0);" class="lblfont" id="lnkAddNew" runat="server">+Add Item</a>
                        &nbsp; <a onclick="window.location=window.location;return false;" class="lblfont"
                            href="javascript:void(0);" style="float: right;">Refresh Cart</a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table cellspacing="1" cellpadding="2" width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="GVShoppingCartItems" runat="server" AutoGenerateColumns="false"
                                        BorderStyle="Solid" CellSpacing="1" CellPadding="0" Width="100%" CssClass="table-noneforOrder"
                                        GridLines="None" OnRowCommand="GVShoppingCartItems_RowCommand" OnRowDataBound="GVShoppingCartItems_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                    <asp:Label ID="lblCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CustomCartID") %>'></asp:Label>
                                                    <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                    <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                                    <asp:Literal ID="ltrVariNamevalue" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    &nbsp; Product Name
                                                </HeaderTemplate>
                                                <HeaderStyle Height="28px" HorizontalAlign="left" />
                                                <ItemStyle HorizontalAlign="left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    &nbsp; SKU
                                                </HeaderTemplate>
                                                <HeaderStyle Height="28px" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Math.Round(Convert.ToDecimal(Eval("SalePrice")),2) %>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    &nbsp; Price
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="right" Height="28px" />
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Eval("Quantity") %>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    &nbsp;Quantity
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Height="28px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSubTotalGrid" runat="server" Text='<%# BindSubtotal(Convert.ToDecimal(Eval("SalePrice")),Convert.ToInt32( Eval("Quantity")))%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    SubTotal&nbsp;
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="right" Height="28px" />
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lkRemove" runat="server" Text="Remove" CommandName='remove' OnClientClick="return confirm('Are you sure to delete this Record ?');"
                                                        CommandArgument=' <%# Eval("ShoppingCartID") +"-"+ Eval("Productid") +"-"+ Eval("CustomCartID") %>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    Remove&nbsp;
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Height="28px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                            CssClass="list_table_cell_link" Height="24px" HorizontalAlign="Center" />
                                        <EditRowStyle CssClass="list_table_cell_link" />
                                        <PagerStyle HorizontalAlign="Right" Font-Size="9pt" ForeColor="Black" />
                                        <HeaderStyle BackColor="#F2F2F2" CssClass="list-table-title" Height="30px" HorizontalAlign="Center"
                                            Wrap="True" />
                                        <AlternatingRowStyle BackColor="#F2F2F2" BorderColor="White" BorderStyle="Solid"
                                            CssClass="list_table_cell_link" ForeColor="#444545" />
                                    </asp:GridView>
                                    <asp:Literal ID="litProducts" runat="Server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"></td>
                            </tr>
                            <tr runat="server" id="trNotes" visible="false">
                                <td colspan="3" align="left" valign="top">
                                    <table>
                                        <tr>
                                            <td valign="top">Notes:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtNotes" runat="server" CssClass="textfield_small" TextMode="MultiLine"
                                                    Width="450px" Height="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table-none-border">
                                        <tr style="background-color: #F2F2F2;">
                                            <td colspan="2" style="text-align: left; padding-left: 5px;" height="25px">
                                                <b>Order Details </b>
                                            </td>
                                        </tr>
                                        <tr class="table_bg">
                                            <td style="text-align: left; padding-left: 5px;" height="25px">ShippingMethod :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:DropDownList ID="ddlShippingMethod" Style="padding-left: 3px; width: 250px;"
                                                    AutoPostBack="true" runat="server" CssClass="select_box" OnSelectedIndexChanged="ddlShippingMethod_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 5px;" height="25px">SubTotal :
                                            </td>
                                            <td style="text-align: left;">$<asp:Label ID="lblSubTotal" Text="0.00" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hfsuto" runat="Server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr class="table_bg">
                                            <td style="text-align: left; padding-left: 5px;" height="25px">ShippingCost :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtShippingCost" onblur="ShoppingCartTotal();" onkeypress="return onKeyPressBlockNumbers(event)"
                                                    Text="0.00" Width="50" CssClass="textfield_small" runat="server"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="TxtShippingCost"
                                                    Display="Dynamic" ErrorMessage="Enter ShippingCost." Operator="DataTypeCheck"
                                                    SetFocusOnError="True" Type="Currency" ValidationGroup="Product" CssClass="error">Invalid ShippingCost</asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 129px; text-align: left; padding-left: 5px;" height="25px">Tax :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtTax" Width="50" onblur="ShoppingCartTotal();" onkeypress="return onKeyPressBlockNumbers(event)"
                                                    Text="0.00" CssClass="textfield_small" runat="server"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="TxtTax"
                                                    Display="Dynamic" ErrorMessage="Enter Tax." Operator="DataTypeCheck" SetFocusOnError="True"
                                                    Type="Currency" ValidationGroup="Product" CssClass="error">Invalid Tax</asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr class="table_bg">
                                            <td style="text-align: left; padding-left: 5px;" height="25px">Discount :
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="TxtDiscount" onblur="ShoppingCartTotal();" onkeypress="return onKeyPressBlockNumbers(event)"
                                                    Text="0.00" Width="50" CssClass="textfield_small" runat="server"></asp:TextBox>
                                                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="TxtDiscount"
                                                    Display="Dynamic" ErrorMessage="Enter Discount." Operator="DataTypeCheck" SetFocusOnError="True"
                                                    Type="Currency" ValidationGroup="Product" CssClass="error">Invalid Discount</asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 5px; padding-top: 5px;" height="25px">
                                                <b>Total :</b>
                                            </td>
                                            <td style="text-align: left;">
                                                <b>$<asp:Label ID="lblTotal" Text="0.00" runat="server"></asp:Label></b>
                                                <asp:HiddenField ID="hfTotal" Value="0" runat="Server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:HiddenField ID="HdnS_State" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnS_Zip" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnCustID" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnS_Country" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnStoreID" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnS_CountryID" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnProductID" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnRMA" Value="0" runat="Server" />
                                    <asp:HiddenField ID="HdnCartID" Value="0" runat="server" />
                                    <asp:HiddenField ID="HdnOrderNumber" Value="0" runat="Server" />

                                    <asp:Button ID="btnUpdate" runat="server" Text="Review" OnClick="btnUpdate_Click"
                                        OnClientClick="loader();" Style="background: url(../images/Change_ordersave.jpg) no-repeat; width: 80px; height: 23px; border: 0; border: 0; color: #ffffff; vertical-align: top; cursor: pointer; font-weight: bold;"
                                        ValidationGroup="Product" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSave" OnClientClick="loader();" runat="server" Text="Save" OnClick="btnSave_Click"
                                        Style="background: url(../images/Change_ordersave.jpg) no-repeat; width: 80px; height: 23px; border: 0; color: #ffffff; vertical-align: top; cursor: pointer; font-weight: bold;"
                                        ValidationGroup="Product" />
                                </td>
                                <td colspan="2" align="left">
                                    <input type="button" value="Close" onclick="javascript: window.close();" style="background: url(../images/Change_ordersave.jpg) no-repeat; width: 80px; height: 23px; border: 0; border: 0; color: #ffffff; vertical-align: top; cursor: pointer; font-weight: bold;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%">
                <tr>
                    <td align="center" style="color: #fff;">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
