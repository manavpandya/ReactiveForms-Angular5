<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListReturnItem.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ListReturnItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script runat="server">
        string _ReturnUrlname;
        public string SetRegisterImage(bool _Value)
        {
            if (_Value == false)
            {
                _ReturnUrlname = "../Images/yes.png";
            }
            else
            {
                _ReturnUrlname = "../Images/no.png";
            }
            return _ReturnUrlname;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {

                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }
        }
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
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
                $(document).ready(function () { jAlert('Select at least one RMA !', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected RMA(s) ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px;" align="left">
                                Store : &nbsp;
                                <asp:DropDownList ID="ddlStore" runat="server" Width="175px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span>
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
                                                    <img class="img-left" title="Return Item List" alt="Return Item List" src="/App_Themes/<%=Page.Theme %>/Images/store-list-icon.png" />
                                                    <h2>
                                                        Return Item List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <table>
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search&nbsp;:
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList Width="125px" ID="ddlSearch" runat="server" CssClass="order-list">
                                                                <asp:ListItem Value="CustomerName">CustomerName</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield">
                                                            </asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:ImageButton ID="btnGo" runat="server" OnClientClick="return validation();" ImageUrl="/App_Themes/gray/Images/search.gif"
                                                                OnClick="btnGo_Click" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:ImageButton ID="btnSearchall" runat="server" ImageUrl="/App_Themes/gray/Images/showall.png"
                                                                CommandName="ShowAll" OnClick="btnSearchall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdReturnItemList" runat="server" AutoGenerateColumns="False" DataKeyNames="returnid"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    ShowHeader="true" OnRowDataBound="grdReturnItemList_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RMA No.">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <a style="text-decoration: none; cursor: pointer;" onclick='<%# "javascript:window.open(\"ReturnMerchandisePopUp.aspx?ID=" + DataBinder.Eval(Container.DataItem,"returnid")  +"\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");" %>'>
                                                                    <asp:Label ID="lblRMAID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RMANo") %>'></asp:Label>
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer ID">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomerID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Number">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdOrderNumber" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"OrderedNumber") %>' />
                                                                <asp:HiddenField ID="hdReturnType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ReturnType") %>' />
                                                                <asp:HiddenField ID="hdStoreName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"SToreName") %>' />
                                                                <asp:HiddenField ID="hdnisReturnrequest" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"isReturnrequest") %>' />
                                                                <%--href="/Admin/OrderManagement/orders.aspx?ONo=&refund=1"--%>
                                                                <a runat="server" id="lkOrderNumber" style="text-decoration: none">
                                                                    <asp:Label ID="OrderNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderedNumber") %>'></asp:Label>
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"CustomerName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Order Date">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderDate","{0:dd MMM yyyy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPro" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblquantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Return Status" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <img id="imagstatus" runat="server">
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Return Status" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReturnItemID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"returnid") %>'></asp:Label>
                                                                <asp:Label ID="lblReturnStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsReturn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Is Active">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <img src='<%# SetRegisterImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Status"))) %>'
                                                                    alt="" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;">
                                                        <asp:Button ID="btnDeleteMultiple" runat="server" OnClientClick="return checkCount();"
                                                            OnClick="btnDeleteMultiple_Click" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDeleteMultiple_Click" />
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
