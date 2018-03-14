<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="OrderExporttoBackend.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.OrderExporttoBackend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style>
        .pageccc td
        {
            border-style: none !important;
        }
    </style>
    <script type="text/javascript">

        $(function () {

            $('#ContentPlaceHolder1_txtFromdate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtTodate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function chkSelect() {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert("Please select at least one Order.", "Message");
                return false;
            }
        }

        function SearchValidation() {
            if (document.getElementById('ContentPlaceHolder1_txtFromdate').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtFromdate');
                //document.getElementById('ContentPlaceHolder1_txtMailFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtTodate').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtTodate');
                return false;
            }

            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtMFromdate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtTodate').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtTodate');
                return false;
            }
            return true;
        }


        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        function SelectAll(on) {
            var allElts = document.getElementById('divGrid').getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <table>
                    <tr>
                        <td align="left">
                            Store :&nbsp;<asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list"
                                Width="175px" AutoPostBack="true" Style="margin-top: 5px;" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Bulk Order Print" alt="Bulk Order Print" src="/App_Themes/<%=Page.Theme %>/Images/mail-log-icon.png" />
                                                    <h2>
                                                        Order Export to Back End</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                            </td>
                                            <td align="right" colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">
                                                            From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtFromdate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtTodate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:DropDownList ID="ddlsearchBy" runat="server" CssClass="order-list" Width="175px"
                                                                AutoPostBack="false" Style="margin-top: 5px;">
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Exported</asp:ListItem>
                                                                <asp:ListItem Value="0">Not Exported</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                                                OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right">
                                                        </td>
                                                        <td align="right" style="display: none;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="trExport" runat="server">
                                            <td align="right" colspan="2" style="padding-left: 10px;">
                                                <a title="Export Order" href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_btnExport').click();">
                                                    <img title="Export Order" alt="Export Order" src="/App_Themes/<%=Page.Theme %>/images/export-order.png" /></a>
                                                <a title="Export Product" href="javascript:void(0);" onclick="javascript:document.getElementById('ContentPlaceHolder1_btnExportproduct').click();">
                                                    <img title="Export Product" alt="Export" src="/App_Themes/<%=Page.Theme %>/images/export-product.png" /></a>
                                            </td>
                                        </tr>
                                        <tr id="trTop" runat="server">
                                            <td align="left" style="padding-left: 10px;">
                                                <a id="a1" href="javascript:SelectAll(true);">Check All</a>&nbsp; | <a id="a2" href="javascript:SelectAll(false);">
                                                    Clear All</a>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grvBulkPrint" runat="server" CssClass="order-table" BorderStyle="Solid"
                                                        BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                        Width="100%" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" OnRowDataBound="grvBulkPrint_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkPrint" runat="server" />
                                                                    <asp:Label ID="lblOrderID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'
                                                                        Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="OrderDate" HeaderText="Order Date">
                                                                <HeaderTemplate>
                                                                    Order Date
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr1" runat="server" Text='<%#String.Format("{0:dd&nbsp;MMM,&nbsp;yyyy&nbsp;hh:mm:ss&nbsp;ttt}",Convert.ToDateTime(Eval("OrderDate")))%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order No. / Product Details">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" id="tblordernodetails"
                                                                        runat="server">
                                                                        <tr>
                                                                            <td align="left" valign="top" style="border: none;">
                                                                                <asp:Label runat="server" ID="OrderId" Style="display: none;" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                                                <asp:Literal ID="ltr2" runat="server" Text='<%# Eval("OrderNumber")%>'></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ordered from (Store)">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr3" runat="server" Text='<%# Eval("StoreName")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer Name / ZipCode" HeaderStyle-Width="18%">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr4" runat="server" Text='<%# "<a href=\"/Admin/Customers/Customer.aspx?mode=edit&CustID="+ Eval("CustomerID") +"\" title=\"\" class=\"order-no\">"+ Eval("CustName") +"</a>" %>'></asp:Literal>
                                                                    <input type="hidden" id="hdnshoppingcartid" runat="server" value='<%# Eval("ShoppingCardID")%>' />
                                                                    <input type="hidden" id="hdnAddress1" runat="server" value='<%# Eval("ShippingAddress1")%>' />
                                                                    <input type="hidden" id="hdnAddress2" runat="server" value='<%# Eval("ShippingAddress2")%>' />
                                                                    <input type="hidden" id="hdnSuite" runat="server" value='<%# Eval("ShippingSuite")%>' />
                                                                    <input type="hidden" id="hdnCity" runat="server" value='<%# Eval("ShippingCity")%>' />
                                                                    <input type="hidden" id="hdnState" runat="server" value='<%# Eval("ShippingState")%>' />
                                                                    <input type="hidden" id="hdnPhone" runat="server" value='<%# Eval("ShippingPhone")%>' />
                                                                    <input type="hidden" id="hdnCountry" runat="server" value='<%# Eval("ShippingCountry")%>' />
                                                                    <input type="hidden" id="hdnZip" runat="server" value='<%# Eval("ShippingZip")%>' />
                                                                    <input type="hidden" id="hdnCompany" runat="server" value='<%# Eval("ShippingCompany")%>' />
                                                                    <input type="hidden" id="hdnShippingMethod" runat="server" value='<%# Eval("ShippingMethod")%>' />
                                                                    <input type="hidden" id="hdnOrderTotalNew" runat="server" value='<%# Eval("OrderTotal")%>' />
                                                                    <asp:TextBox ID="txtCustomername" runat="server" CssClass="order-textfield" Width="234px"
                                                                        Visible="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Total" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr5" runat="server" Text='<%#String.Format("{0:C}",Convert.ToDecimal(Eval("OrderListTotal")))%>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Shipped On" HeaderStyle-Width="10%" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltrShip" runat="server" Text='<%# String.Format("{0:d}",DataBinder.Eval(Container.DataItem,"ShippedOn")) %>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr6" runat="server" Text='<%# Eval("TransactionStatus")%>'></asp:Literal>
                                                                    <input type="hidden" id="hdnorderStatus" runat="server" value='<%# Eval("orderStatus")%>' />
                                                                    <input type="hidden" id="hdnSubtotal" runat="server" value='<%# Eval("OrderSubtotal")%>' />
                                                                    <input type="hidden" id="hdnTotal" runat="server" value='<%# Eval("FullOrderTotal")%>' />
                                                                    <input type="hidden" id="HdnShippingCost" runat="server" value='<%# Eval("OrderShippingCosts")%>' />
                                                                    <input type="hidden" id="hdnordertax" runat="server" value='<%# Eval("OrderTax")%>' />
                                                                    <input type="hidden" id="hdnDiscount" runat="server" value='<%# Eval("CustomDiscount")%>' />
                                                                    <input type="hidden" id="hdnRefund" runat="server" value='<%# Eval("RefundAmount")%>' />
                                                                    <input type="hidden" id="hdnAdjAmt" runat="server" value='<%# Eval("AdjustmentAmount")%>' />
                                                                    <input type="hidden" id="hdnSubtotalF" runat="server" value='<%# Eval("OrderSubtotalF")%>' />
                                                                    <input type="hidden" id="HdnShippingCostF" runat="server" value='<%# Eval("OrderShippingCostsF")%>' />
                                                                    <input type="hidden" id="hdnordertaxF" runat="server" value='<%# Eval("OrderTaxF")%>' />
                                                                    <input type="hidden" id="hdnDiscountF" runat="server" value='<%# Eval("CustomDiscountF")%>' />
                                                                    <input type="hidden" id="hdnRefundF" runat="server" value='<%# Eval("RefundAmountF")%>' />
                                                                    <input type="hidden" id="hdnAdjAmtF" runat="server" value='<%# Eval("AdjustmentAmountF")%>' />
                                                                    <input type="hidden" id="hdnlvelDiscountF" runat="server" value='<%# Eval("LevelDiscountAmountF")%>' />
                                                                    <input type="hidden" id="hdncoponDiscountF" runat="server" value='<%# Eval("CouponDiscountAmountF")%>' />
                                                                    <input type="hidden" id="hdnQtyDiscountAmountF" runat="server" value='<%# Eval("QuantityDiscountAmountF")%>' />
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="false" Visible="false"
                                                                        CssClass="status-list">
                                                                        <asp:ListItem Text="All Types" Value="All"></asp:ListItem>
                                                                        <asp:ListItem Text="New" Value="New" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Shipped" Value="Shipped"></asp:ListItem>
                                                                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                                                        <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                                                                        <asp:ListItem Text="Canceled" Value="Canceled"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exported Date" HeaderStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltrPrinted" runat="server" Text='<%# Eval("ExportedDate")%>'></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltr7" runat="server" Text=''></asp:Literal>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="altrow" VerticalAlign="top" />
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging pageccc" BorderStyle="None" />
                                                        <RowStyle CssClass="odd-row" VerticalAlign="top" />
                                                        <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trBottom" runat="server">
                                            <td align="left" style="padding-left: 10px;">
                                                <a id="aAllowAll" href="javascript:SelectAll(true);">Check All</a>&nbsp; | <a id="aClearAll"
                                                    href="javascript:SelectAll(false);">Clear All</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <asp:Button ID="btnExport" runat="server" ToolTip="Send Email" OnClientClick="return chkSelect();"
            OnClick="btnExport_Click" />
        <asp:Button ID="btnExportproduct" runat="server" ToolTip="Send Email" OnClientClick="return chkSelect();"
            OnClick="btnExportproduct_Click" />
    </div>
</asp:Content>
