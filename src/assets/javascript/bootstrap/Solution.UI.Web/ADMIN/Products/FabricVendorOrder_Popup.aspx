<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FabricVendorOrder_Popup.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.FabricVendorOrder_Popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function ClacuOnHandQty(OrderQtyId, BookId, BalQtyId) {
            var QtyOnHand = 0;
            if (document.getElementById(BookId) != null && document.getElementById(OrderQtyId) != null) {
                var BookedQty = document.getElementById(BookId).value;
                var OrderQty = document.getElementById(OrderQtyId).value;

                if (OrderQty == '') {
                    OrderQty = 0;
                    document.getElementById(OrderQtyId).value = 0;
                }
                if (BookedQty == '') {
                    BookedQty = 0;
                    document.getElementById(BookId).value = 0;
                }

                if (isNaN(QtyOnHand)) {
                    QtyOnHand = 0;
                }
                QtyOnHand = parseInt(OrderQty) - parseInt(BookedQty);
                document.getElementById(BalQtyId).value = QtyOnHand;
            }
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
    </script>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <asp:Literal ID="ltrCalenScript" runat="server"></asp:Literal>
    <div id="popupcontactdetails" style="z-index: 1000001; width: 780px; height: 499px;
        padding: 2px 0 0 3px">
        <table width="99.5%" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
            font-size: 12px; font-family: Arial,Helvetica,sans-serif;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;Vendor Fabric Order
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupviewdetailclose" Style="position: relative;" ImageUrl="/App_Themes/Gray/images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();">
                    </asp:ImageButton>
                </td>
            </tr>
            <tr style="background-color: White">
                <td colspan="2" valign="top">
                    <div class="content-row1" style="float: left; padding: 2px; height: 13px;">
                        <div class="create-new-order" style="float: left;">
                            <asp:Label ID="lblFebrictypeName" runat="server"></asp:Label></div>
                    </div>
                    <div class="content-row2">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td height="10" align="left" valign="top">
                                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                                </td>
                            </tr>
                            <tr>
                                <td class="border-td">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                        class="content-table">
                                        <tr>
                                            <td class="border-td-sub">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr>
                                                        <td id="tdMainDiv">
                                                            <div id="divMain" class="slidingDivMainDiv">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                                    <tr class="even-row" style="display: none;">
                                                                        <td align="center">
                                                                            <fieldset>
                                                                                <legend><b>Select Fabric Category</b></legend>
                                                                                <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr class="oddrow">
                                                                                        <td width="13%">
                                                                                            <span class="star">*</span>Fabric Category Name :
                                                                                        </td>
                                                                                        <td width="88%" align="left">
                                                                                            <asp:DropDownList ID="ddlFabricType" runat="server" class="product-type" OnSelectedIndexChanged="ddlFabricType_SelectedIndexChanged"
                                                                                                AutoPostBack="true">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="altrow" id="trFabricDetails" runat="server" visible="false">
                                                                        <td align="center">
                                                                            <div id="div4" class="slidingDivDesc">
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td valign="top" align="left" colspan="2">
                                                                                            <asp:GridView ID="grdVendorPortal" runat="server" AutoGenerateColumns="False" BorderColor="#e7e7e7"
                                                                                                BorderStyle="Solid" BorderWidth="1px" DataKeyNames="FabricCodeId" EmptyDataText="No Record(s) Found."
                                                                                                AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                ViewStateMode="Enabled" Width="100%" GridLines="None" OnRowDataBound="grdVendorPortal_RowDataBound"
                                                                                                AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast" OnRowCommand="grdVendorPortal_RowCommand">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField Visible="false">
                                                                                                        <HeaderTemplate>
                                                                                                            &nbsp;Active
                                                                                                        </HeaderTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblFabricVendorPortId" runat="server" Visible="true" Text='<%# Bind("FabricVendorPortId") %>'></asp:Label>
                                                                                                            &nbsp;<asp:Label ID="lblFabricCodeId" runat="server" Visible="true" Text='<%# Bind("FabricCodeId") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <HeaderTemplate>
                                                                                                            &nbsp;Fabric Code
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemTemplate>
                                                                                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                         <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>&nbsp;-&nbsp;<asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:GridView ID="grdVendorOrder" runat="server" AutoGenerateColumns="False" BorderColor="#e7e7e7"
                                                                                                                            BorderStyle="Solid" BorderWidth="1px" DataKeyNames="FabricCodeId" EmptyDataText="No Record(s) Found."
                                                                                                                            AllowSorting="false" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                                            ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="50"
                                                                                                                            PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grdVendorOrder_RowDataBound">
                                                                                                                            <Columns>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Vendor Order#
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblFabricVendorPortId" runat="server" Visible="false" Text='<%# Bind("FabricVendorPortId") %>'></asp:Label>
                                                                                                                                        &nbsp;<asp:Label ID="lblFabricCodeId" runat="server" Visible="false" Text='<%# Bind("FabricCodeId") %>'></asp:Label>
                                                                                                                                        <asp:Label ID="lblFabricOrderId" runat="server" Visible="false" Text='<%# Bind("FabricOrderId") %>'></asp:Label>
                                                                                                                                        <asp:Label ID="lblVendorOrderNum" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Qty Order In Yards
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtQtyinYard" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                                                            onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                                                            Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyinYard"))  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Qty Booked
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtQtyBoookedinYard" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                                                            onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                                                            Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyBoookedinYard"))  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Balance On Order
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtBalanceOrder" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                                                            onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                                                            Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"BalanceOrder"))  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Ordered Date
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtOrderDate" runat="server" Width="70px" CssClass="from-textfield"
                                                                                                                                            Style="text-align: left; margin-right: 3px;" Text='<%#  DataBinder.Eval(Container.DataItem,"OrderDate")  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Production Date
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtProductionDate" runat="server" Width="70px" CssClass="from-textfield"
                                                                                                                                            Style="text-align: left; margin-right: 3px;" Text='<%#  DataBinder.Eval(Container.DataItem,"ProductionDate")  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField>
                                                                                                                                    <HeaderTemplate>
                                                                                                                                        &nbsp;Received Quantity
                                                                                                                                    </HeaderTemplate>
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtQtyReceived" runat="server" Width="60px" CssClass="order-textfield"
                                                                                                                                            onkeypress="return keyRestrictforIntOnly(event,'0123456789');" Style="text-align: center;"
                                                                                                                                            Text='<%#  Convert.ToInt32(DataBinder.Eval(Container.DataItem,"QtyReceived"))  %>'
                                                                                                                                            MaxLength="6"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                                <asp:TemplateField HeaderText="Operations">
                                                                                                                                    <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="_delete"
                                                                                                                                            CommandArgument='<%# Eval("FabricOrderId") %>' message='<%# Eval("FabricOrderId") %>'
                                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){return true;}else{return false;}"
                                                                                                                                            ImageUrl="~/App_Themes/Gray/images/delete-icon.png"></asp:ImageButton>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </asp:TemplateField>
                                                                                                                            </Columns>
                                                                                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                                                        </asp:GridView>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidatePage();"
                                                                                                OnClick="btnSave_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="left" valign="top">
                                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
