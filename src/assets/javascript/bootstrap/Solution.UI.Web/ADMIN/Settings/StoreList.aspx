<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="StoreList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.StoreList" %>

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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Settings/Store.aspx">
                    <img alt="Add Store" title="Add Store" src="/App_Themes/<%=Page.Theme %>/images/add-store.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt=""/>
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt=""/>
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
                                                    <img class="img-left" title="Store List" alt="Store List" src="/App_Themes/<%=Page.Theme %>/Images/store-list-icon.png" />
                                                    <h2>
                                                        Store List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.StoreComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table>
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search&nbsp;:
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" Width="160px" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%;padding-right:0px;">
                                                            <asp:ImageButton ID="btnGo" runat="server" OnClick="btnGo_Click" OnClientClick="return validation();"
                                                                ImageUrl="/App_Themes/gray/Images/search.gif" />
                                                        </td>
                                                        <td style="width: 5%;padding-right:0px;">
                                                            <asp:ImageButton ID="btnSearchall" runat="server" OnClick="btnSearchall_Click" ImageUrl="/App_Themes/gray/Images/showall.png"
                                                                CommandName="ShowAll" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdStore" runat="server" AutoGenerateColumns="False" DataKeyNames="StoreID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnRowCommand="grdStore_RowCommand"
                                                    BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                    GridLines="None" Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                    PagerSettings-Mode="NumericFirstLast" DataSourceID="_gridObjectDataSource" OnRowDataBound="grdStore_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Store ID">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreId" runat="server" Text='<%# Bind("StoreId") %>' Style="float: center"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Store Name
                                                                <asp:ImageButton ID="btnStoreName" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("StoreName") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ImageUrl="~/ADMIN/Images/Edit.gif"
                                                                    ToolTip="Edit" CommandName="edit" CommandArgument='<%# Eval("StoreID") %>'></asp:ImageButton>
                                                                <asp:ImageButton Visible="false" runat="server" ID="_DeleteLinkButton" ImageUrl="~/ADMIN/Images/Delete.gif"
                                                                    ToolTip="Delete" CommandName="DeleteStoreConfig" CommandArgument='<%# Eval("StoreID") %>'
                                                                    message="Are you sure want to delete current Store?" OnClientClick='return confirm(this.getAttribute("message"))'>
                                                                </asp:ImageButton>
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
