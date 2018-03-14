<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="VendorSkuList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.VendorSkuList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
 
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(Boolean _Status)
        {
            if (_Status == true)
            {
                _ReturnUrl = "../Images/active.gif";

            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";

            }
            return _ReturnUrl;
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
    </script>
    <script language="javascript" type="text/javascript">
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
                $(document).ready(function () { jAlert('Select at least one Vendor!', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }

        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected Vendor(s) ?', 'Confirmation', function (r) {
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

        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Products/Vendorsku.aspx">
                    <img alt="Add DropShipper SKU" title="Add DropShipper SKU" src="/App_Themes/<%=Page.Theme %>/images/add-dropshipper-sku.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="DropShipper SKU List" alt="DropShipper SKU List" src="/App_Themes/<%=Page.Theme %>/Images/vendor-list-icon.png">
                                                    <h2>
                                                        DropShipper SKU List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.VendorComponent" SelectMethod="GetVendorSKUDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Search :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td align="right" style="padding-right: 0px; width: 23px;">
                                                            <asp:Button ID="btnSearchall" runat="server" OnClick="btnSearchall_Click" CommandName="ShowAll" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdVendor" runat="server" AutoGenerateColumns="False" DataKeyNames="VendorID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowCommand="grdVendor_RowCommand"
                                                    BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                    GridLines="None" Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" DataSourceID="_gridObjectDataSource" OnRowDataBound="grdVendor_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnVendorID" runat="server" Value='<%#Eval("VendorSKUID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vendor Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Vendor Name
                                                                <asp:ImageButton ID="btnVendorName" runat="server" CommandArgument="DESC" CommandName="Name"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVendorName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                DropShipper SKU
                                                                <asp:ImageButton ID="btnVendorSKU" runat="server" CommandArgument="DESC" CommandName="VendorSKU"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblvendorsku" runat="server" Text='<%# Bind("VendorSKU") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                                <asp:ImageButton ID="btnProductName" runat="server" CommandArgument="DESC" CommandName="ProductName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductname" runat="server" Text='<%# Bind("ProductName") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("VendorSKUID") %>'></asp:ImageButton>
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
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;">
                                                        <asp:Button ID="btnDeleteMultiple" runat="server" OnClick="btnDeleteMultiple_Click"
                                                            OnClientClick="return checkCount();" />
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
