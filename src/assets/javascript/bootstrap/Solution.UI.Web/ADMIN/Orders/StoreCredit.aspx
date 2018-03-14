<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreCredit.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.StoreCredit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Cerdit</title>
    <script src="../../js/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtExpDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
    </script>
    <script type="text/javascript">
        function CheckValid() {
            if (document.getElementById('txtCouponCode') != null && document.getElementById('txtCouponCode').value == '') {
                jAlert('Please Enter Coupon Code', 'Message', 'txtCouponCode');
                return false;
            }
            if (document.getElementById('txtExpDate') != null && document.getElementById('txtExpDate').value == '') {
                jAlert('Please Enter Expiration Date', 'Message', 'txtExpDate');
                return false;
            }
            //            if ((document.getElementById('txtDiscountPercent') != null && document.getElementById('txtDiscountPercent').value == '') && (document.getElementById('txtDiscoutAmnt') != null && document.getElementById('txtDiscoutAmnt').value == '')) {
            //                jAlert('Please Enter Discount Percent [Or] Discount Amount', 'Message', 'txtDiscountPercent');
            //                return false;
            //            }
            if (document.getElementById('txtDiscoutAmnt') != null && document.getElementById('txtDiscoutAmnt').value == '') {
                jAlert('Please Enter Discount Amount', 'Message', 'txtDiscoutAmnt');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
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
        function onKeyPressBlockNumbersForPercentage(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
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
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="../../App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <table width="100%" cellspacing="2" cellpadding="2" border="0" style="line-height: 20px;"
                            class="table_none">
                            <tbody>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="lblMsg" runat="server" CssClass="font-red"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right" width="100%">
                                        <span class="star">*</span>Required Field
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px; padding-top: 10px; width: 50px;">
                                        <span class="star">*</span>Store Name :
                                    </td>
                                    <td style="padding-top: 10px;">
                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">&nbsp;</span>Description :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" Columns="5" TextMode="MultiLine"
                                            Height="50px" Width="300px" CssClass="textboxcommonstyle"></asp:TextBox><asp:Label runat="server"
                                                ID="lblCustomerName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">*</span>Coupon Code :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCouponCode" runat="server" CssClass="textboxcommonstyle" Width="200px"></asp:TextBox>&nbsp;<asp:LinkButton
                                            ID="generaterandom" runat="server" Style="color: #FE0000; text-decoration: underline;"
                                            Text="Generate Coupon Code" OnClick="generaterandom_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">*</span>Expiration Date :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExpDate" runat="server" CssClass="from-textfield" Width="70px"
                                            Style="margin-right: 3px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">*</span>Discount Percent :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="textboxcommonstyle"
                                            Width="41px" Text="0" MaxLength="3" onkeypress="return onKeyPressBlockNumbersForPercentage(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">*</span>Store Credit Amount :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiscoutAmnt" runat="server" onkeypress="return onKeyPressBlockNumbers(event);"
                                            CssClass="textboxcommonstyle" MaxLength="6" Width="50px" Text="0.00"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">*</span>Coupon Valid For :
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdbCoupons" runat="server" AutoPostBack="True" CssClass="select_box1"
                                            OnSelectedIndexChanged="rdbCoupons_SelectedIndexChanged">
                                            <asp:ListItem Value="ExpiresonFirstUseByAnyCustomer" Selected="true"> Expires On First Use By Any Customer</asp:ListItem>
                                            <asp:ListItem Value="ExpiresAfterOneUsageByEachCustomer"> Expires After One Usage By Each Customer</asp:ListItem>
                                            <asp:ListItem Value="ExpiredAfterNUses"> Expired After N Uses</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr runat="server" id="trUsers" visible="false">
                                    <td align="left" valign="middle" style="height: 28px;">
                                        <span class="star">&nbsp;</span>Enter Value :
                                    </td>
                                    <td align="left" class="dark_bg" style="height: 28px">
                                        <asp:TextBox ID="txtNUses" runat="server" CssClass="textboxcommonstyle"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">&nbsp;</span>Valid For Customer(s) :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValidForCust" runat="server" CssClass="textboxcommonstyle"></asp:TextBox>
                                        <asp:TextBox ID="txtEmail" runat="server" Width="200px" CssClass="textboxcommonstyle"></asp:TextBox>
                                        <asp:CheckBox ID="chkAllCustomer" runat="server" onchange="javascript:DisableBox(this,'ctl00_ContentPlaceHolder1_txtValidForCust');"
                                            CssText="All Customers" />
                                        (Enter Coma seperated list of Customer ids)
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">&nbsp;</span>Valid For Product(s) :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValidForProducts" runat="server" CssClass="textboxcommonstyle"></asp:TextBox>
                                        <asp:CheckBox ID="chkAllProducts" runat="server" onchange="javascript:DisableBox(this,'ctl00_ContentPlaceHolder1_txtValidForProducts');"
                                            CssText="All Products" />
                                        (Enter Coma seperated list of Product ids)
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">&nbsp;</span>Valid For Category(s) :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValidForCategory" runat="server" CssClass="textboxcommonstyle"></asp:TextBox>
                                        <asp:CheckBox ID="chkAllCategories" runat="server" onchange="javascript:DisableBox(this,'ctl00_ContentPlaceHolder1_txtValidForCategory');"
                                            CssText="All Categories" />
                                        (Enter Coma seperated list of Category ids)
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td valign="top" align="left" style="padding-left: 20px;">
                                        <span class="star">&nbsp;</span>MinOrderTotal :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOrderTotal" runat="server" onkeypress="return onKeyPressBlockNumbers(event);"
                                            CssClass="textboxcommonstyle" Width="292px" Height="18px" Text="0.00"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return CheckValid();"
                                            ImageUrl="~/Admin/images/save.jpg" OnClick="btnSave_Click" />
                                        &nbsp; &nbsp;
                                        <asp:ImageButton ID="btnCancel" runat="server" OnClientClick="javascript:window.close();return false;"
                                            CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr id="trOrderDetails" runat="server" visible="false">
                                    <td align="left" colspan="2" width="100%" style="line-height: 20px; background-color: #f3f3f3;
                                        font-weight: bold; font-size: 12px">
                                        Order Details :
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" width="100%">
                                        <asp:GridView ID="grdRMARequestList" runat="server" CssClass="table-noneforOrder"
                                            AutoGenerateColumns="False" BorderWidth="0" CellPadding="5" CellSpacing="5" Width="100%"
                                            GridLines="None" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Order Total($)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblordertotal" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTotal")), 2) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Order SubTotal($)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsubordertotal" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderSubTotal")), 2) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Shipping Cost($)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblshippingcost" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderShippingCosts")), 2) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Order Tax($)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblordertax" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTax")), 2) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Price($)
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblprice" runat="server" Text='<%#Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")), 2) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Quantity
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblqty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
