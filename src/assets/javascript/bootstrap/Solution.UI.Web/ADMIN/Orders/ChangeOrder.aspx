<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ChangeOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                <td align="left" style="padding-bottom: 10px;">
                    <%--Order # : <strong style="color: #4C4C4C;">
                        <asp:Label ID="lblOrderNo" runat="server"></asp:Label>  </strong>--%>
                </td>
                <td>
                    <a id="lnkOldOrders" runat="server" onclick="window.location=window.location;return false;"
                        class="lblfont" href="javascript:void(0);" style="float: right;">View Old Orders</a>
                </td>
            </tr>
            <tr>
                <td align="left" style="padding-bottom: 10px; text-align: right;" colspan="2">
                    <a href="javascript:void(0);" class="lblfont" id="lnkAddNew" runat="server">+Add Item</a>
                    &nbsp;&nbsp;&nbsp;<a onclick="window.location=window.location;return false;" class="lblfont"
                        href="javascript:void(0);" style="float: right;">Refresh Cart</a>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" CellPadding="5"
                        CellSpacing="0" OnRowDataBound="grdProducts_RowDataBound" ShowFooter="false"
                        Width="100%" GridLines="Both" CssClass="table-noneforOrder" OnRowCommand="grdProducts_RowCommand">
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                    <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                    <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                    <asp:Label ID="lblRelatedproductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RelatedproductID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Product Name
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
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
                                <HeaderStyle HorizontalAlign="right" />
                                <HeaderTemplate>
                                    <span style="text-align: right;">Price</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    $<asp:Label ID="lblPrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")).ToString("f2") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="right" />
                                <HeaderTemplate>
                                    <span style="text-align: right;">Discount Price
                                        <asp:Label ID="lblHeaderDiscount" runat="server"></asp:Label>
                                    </span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    $<asp:Label ID="lblOrginalDiscountPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"DiscountPrice")),2) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="right" />
                                <HeaderTemplate>
                                    <span style="text-align: right;">Sub Total</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    $<asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <span style="text-align: Center;">Delete</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                        ToolTip="Delete" OnClientClick="javascript:if(confirm('Are you sure,you want to delete this Product ?')){ }else{return false;}"
                                        CommandArgument='<%# Container.DataItemIndex %>' CommandName="delMe" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left" style="padding-bottom: 10px; text-align: right;" colspan="2">
                    <a href="javascript:void(0);" id="lnkAddNewBottom" class="lblfont" runat="server">+Add
                        Item</a> &nbsp;&nbsp;&nbsp;<a onclick="window.location=window.location;return false;"
                            class="lblfont" href="javascript:void(0);" style="float: right;">Refresh Cart</a>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" class="table-noneforOrder" style="font-size: 12px;">
            <tr>
                <th>
                </th>
                <th>
                    Change To
                </th>
                <th>
                    Original
                </th>
            </tr>
            <tr>
                <td>
                    Delivery Method
                </td>
                <td>
                    <%= litOrgShippingMethod.Text %>
                </td>
                <td>
                    <asp:Literal ID="litOrgShippingMethod" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    SubTotal
                </td>
                <td>
                    <asp:Literal ID="litCurSubtotal" runat="Server"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="litOrgSubTotal" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Discount
                </td>
                <td>
                    <asp:Literal ID="litCurDiscount" runat="Server"></asp:Literal><br />
                </td>
                <td rowspan="2">
                    <asp:Literal ID="litOrgDiscount" runat="Server"></asp:Literal><br />
                    <asp:Literal ID="litOrgCustomDiscount" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Other Discount
                </td>
                <td>
                    <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" onclick="javascript:ToggleText(this,'txtDiscount');" />
                    <asp:TextBox ID="txtDiscount" onkeypress="return keyRestrictEngraving(event,'1234567890.');"
                        CssClass="from-textfield hasDatepicker" runat="Server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Tax
                </td>
                <td>
                    <asp:Literal ID="litCurOrgTax" runat="Server"></asp:Literal>
                    <asp:Literal ID="litOrgTax" runat="Server" Visible="False"></asp:Literal>
                    <%--<asp:TextBox ID="txtOrgTax" onkeypress="return keyRestrictEngraving(event,'1234567890.');"
                        CssClass="from-textfield hasDatepicker" runat="server" Visible="false"></asp:TextBox>--%>
                </td>
                <td>
                    <%= litOrgTax.Text %>
                </td>
            </tr>
            <tr>
                <td>
                    Shipping Cost
                </td>
                <td>
                    <asp:Literal ID="litCurShippingCost" runat="Server"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="litOrgShippingCost" runat="Server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    Use Fixed Shipping Cost
                </td>
                <td>
                    <asp:CheckBox ID="chkShipping" runat="server" Checked="false" onclick="javascript:ToggleText(this,'txtShipping');" />&nbsp;<asp:TextBox
                        ID="txtShipping" runat="Server" onkeypress="return keyRestrictEngraving(event,'1234567890.');"
                        CssClass="from-textfield hasDatepicker" Enabled="false"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Total
                </td>
                <td>
                    <asp:Literal ID="litCurTotal" runat="Server"></asp:Literal>&nbsp;
                </td>
                <td>
                    <asp:Literal ID="litOrgTotal" runat="Server"></asp:Literal>
                    <asp:Literal ID="litTranState" runat="Server"></asp:Literal>
                    <asp:HiddenField ID="txthOldTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Button ID="btnUpdate" runat="server" CssClass="button" Style="background: url(../images/Change_ordersave.jpg) no-repeat;
                        width: 80px; height: 22px; border: 0; color: #ffffff; vertical-align: top; cursor: pointer;
                        font-size: 12px; font-weight: bold; padding-bottom: 4px;" Text="Review" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnSave" runat="server" CssClass="button" Style="background: url(../images/Change_ordersave.jpg) no-repeat;
            width: 80px; height: 22px; border: 0; color: #ffffff; vertical-align: top; cursor: pointer;
            font-size: 12px; font-weight: bold; padding-bottom: 4px;" Text="Save" OnClick="btnSave_Click"
            Visible="false" />
        &nbsp;
        <asp:Button ID="btnSaveAndProcess" runat="server" CssClass="button" Style="background: url(../images/Change_order.jpg) no-repeat;
            width: 250px; height: 22px; border: 0; color: #ffffff; font-weight: bold; vertical-align: top;
            cursor: pointer; font-size: 12px; padding-bottom: 4px;" Text="Save" OnClick="btnSaveAndProcess_Click" />
        &nbsp;</div>
    </form>
</body>
</html>
