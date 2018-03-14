<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListBulkPrinting.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ListBulkPrinting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" language="javascript">
        var t;
        function selectAll(on) {
            var allElts = document.forms['form1'].elements;
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }

        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Select at least one Record!', 'Message'); });
                return false;
            }
            else {
                return true;
            }
        }

        $(function () {
            $('#ContentPlaceHolder1_txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function validation() {
            if (document.getElementById('ContentPlaceHolder1_txtFromDate').value == '') {
                jAlert('Please Enter From Date.', 'Required Information', 'ContentPlaceHolder1_txtFromDate');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtToDate').value == '') {
                jAlert('Please Enter To Date.', 'Required Information', 'ContentPlaceHolder1_txtToDate');
                return false;
            }
            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtFromDate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtToDate').value);
            if (startDate > endDate) {
                jAlert('Please Select Valid Date.', 'Required Information', 'ContentPlaceHolder1_txtToDate');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <table>
                    <tr>
                        <td align="left">
                            Store : &nbsp;
                            <asp:DropDownList ID="ddlStore" runat="server" Width="175px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
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
                                                    <img class="img-left" title="Bulk Packing Slip Print" alt="Bulk Packing Slip Print"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/store-list-icon.png" />
                                                    <h2>
                                                        Bulk Packing Slip Print</h2>
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
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            To Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="left" style="width: 65px;">
                                                            Search By :
                                                        </td>
                                                        <td align="left" style="width: 110px;">
                                                            <asp:DropDownList Width="110px" ID="ddlIsPrinted" AutoPostBack="true" runat="server"
                                                                CssClass="order-list" OnSelectedIndexChanged="ddlIsPrinted_SelectedIndexChanged">
                                                                <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                                <asp:ListItem Value="Not Printed" Selected="True">Not Printed</asp:ListItem>
                                                                <asp:ListItem Value="Printed">Printed</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" style="width: 120px;">
                                                            <asp:DropDownList Width="120px" ID="ddlSearch" runat="server" CssClass="order-list">
                                                                <%--<asp:ListItem Text="Search By" Value="0"></asp:ListItem>--%>
                                                                <asp:ListItem Value="OrderNumber">Order Number</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnGo" runat="server" OnClientClick="return validation(); return false;"
                                                                ImageUrl="/App_Themes/gray/Images/search.gif" OnClick="btnGo_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnSearchall" runat="server" ImageUrl="/App_Themes/gray/Images/showall.png"
                                                                CommandName="ShowAll" OnClick="btnSearchall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow" runat="server" id="trtop">
                                            <td colspan="3" style="padding-left: 10px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="right">
                                                            <a id="A1" href="javascript:selectAll(true);">Check All</a> | <a id="A2" href="javascript:selectAll(false);">
                                                                Clear All</a><br />
                                                            <asp:ImageButton ID="ImageButton1" runat="server" Style="padding-top: 5px;" OnClientClick="return checkCount();"
                                                                OnClick="btnprintslip_Click" />
                                                            <div style="display: none;">
                                                                <asp:ImageButton ID="ImageButton2" runat="server" OnClick="ImgMultiCap_Click" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:GridView ID="grvBulkPrintingReport" runat="server" AutoGenerateColumns="False"
                                                    DataKeyNames="Ordernumber" EmptyDataText="No Record(s) Found." AllowSorting="True"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                                    CellPadding="2" CellSpacing="1" GridLines="None" Width="100%" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    ShowHeader="true" OnRowDataBound="grvBulkPrintingReport_RowDataBound" OnPageIndexChanging="grvBulkPrintingReport_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Order#">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBulkOrderNnumber" runat="server" Visible="false" Text='<%# Eval("OrderNumber")%>'></asp:Label>
                                                                <asp:Literal ID="ltrBulkOrderNnumber" runat="server" Text='<%# Eval("OrderNumber")%>'></asp:Literal>
                                                                <asp:Label runat="server" ID="OrderId" Style="display: none;" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'
                                                                    Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Store Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Total">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderTotal" runat="server" Text='<%# String.Format("{0:F}",DataBinder.Eval(Container.DataItem,"OrderTotal")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ship To Address">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShipAddr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippingAddress") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Items">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItems" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Is Shipped">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIsShipped" runat="server" Text='<%# String.Format("{0:d}",DataBinder.Eval(Container.DataItem,"ShippedOn")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Is Printed">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIsPrinted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IsPrinted") %>'></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Print It Now">
                                                            <ItemTemplate>
                                                                <input type="checkbox" runat="server" id="chkPrint" name='<%# "chk_" + DataBinder.Eval(Container.DataItem,"OrderNumber")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ShoppingCartID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShoppingCartID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShoppingCardID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td colspan="3" style="padding-left: 10px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="right">
                                                            <a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                                href="javascript:selectAll(false);">Clear All</a><br />
                                                            <asp:ImageButton ID="btnprintslip" runat="server" Style="padding-top: 5px;" OnClientClick="return checkCount();"
                                                                OnClick="btnprintslip_Click" />
                                                            <div style="display: none;">
                                                                <asp:ImageButton ID="ImgMultiCap" runat="server" OnClick="ImgMultiCap_Click" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
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
</asp:Content>
