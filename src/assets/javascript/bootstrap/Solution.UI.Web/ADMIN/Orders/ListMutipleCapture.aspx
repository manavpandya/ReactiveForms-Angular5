<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListMutipleCapture.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ListMutipleCapture" %>

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
                return MultiCaptureClick();
            }
        }

        function MultiCaptureClick() {
            jConfirm('Are you sure you want to Capture this Record(s)?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('ContentPlaceHolder1_ImgMultiCap').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
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
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Multiple Capture" alt="Multiple Capture" src="/App_Themes/<%=Page.Theme %>/Images/store-list-icon.png" />
                                                <h2>
                                                    Multiple Capture</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table>
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
                                                    <td align="left" style="width: 180px;">
                                                        <asp:DropDownList Width="175px" ID="ddlSearch" runat="server" CssClass="order-list">
                                                            <asp:ListItem Text="Search By" Value="0"></asp:ListItem>
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
                                        <td>
                                            <span><a id="A1" href="javascript:selectAll(true);">Check All</a> | <a id="A2" href="javascript:selectAll(false);">
                                                Clear All</a> </span><span style="float: right;">
                                                    <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="return checkCount();"
                                                        OnClick="btnCaptureMultiTop_Click" />
                                                    <div style="display: none;">
                                                        <asp:ImageButton ID="ImageButton2" runat="server" OnClick="ImgMultiCap_Click" />
                                                    </div>
                                                </span>
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grvMultiCapture" runat="server" AutoGenerateColumns="False" DataKeyNames="Ordernumber"
                                                EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                ShowHeader="true" OnRowDataBound="grvMultiCapture_RowDataBound" OnPageIndexChanging="grvMultiCapture_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectCaptureMultiOrder" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order#">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMultiCapOrderNnumber" runat="server" Visible="false" Text='<%# Eval("OrderNumber")%>'></asp:Label>
                                                            <asp:Literal ID="ltrMultiCapOrderNnumber" runat="server" Text='<%# Eval("OrderNumber")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStoreName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="First Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FirstName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AVS Result">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAVSResult" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"AVSResult") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Shipping Method">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblShipMethod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippingMethod") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billing Address">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBillAddr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"BillingAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Shipping Address">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblShipAddr" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippingAddress") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Card Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCardType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CardType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Gateway">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPGateway" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"PaymentGateway") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="left" />
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
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemStyle HorizontalAlign="right" />
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
                                        <td>
                                            <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;">
                                                    <asp:ImageButton ID="btnCaptureMultiTop" runat="server" OnClientClick="return checkCount();"
                                                        OnClick="btnCaptureMultiTop_Click" />
                                                    <div style="display: none;">
                                                        <asp:ImageButton ID="ImgMultiCap" runat="server" OnClick="ImgMultiCap_Click" />
                                                    </div>
                                                </span>
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
