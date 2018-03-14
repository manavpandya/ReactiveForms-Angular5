<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductShippingRange.aspx.cs" MasterPageFile="~/ADMIN/Admin.Master" Inherits="Solution.UI.Web.ADMIN.Products.ProductShippingRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Products/AddShippingRange.aspx">
                    <img alt="Add Shipping Range" title="Add Shipping Range" src="/App_Themes/<%=Page.Theme %>/images/add-manufacture.png" /></a></span>
            </div>
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left; display: none;">
                    <table style="margin-top: 5px; float: left;">
                        <tr>
                            <td align="left">Store :
                                <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexchange" CssClass="order-list"
                                    Style="margin-left: 0px">
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
                                    <table width="100%" border="0" class="add-product" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <th width="100%">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Customer Availability Notification List" alt="Customer Availability Notification List" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>Shipping Price Range</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <table style="width: 100%; text-align: right">
                                                    <tr>
                                                        <td colspan="5" align="left" style="padding-right: 4px">Total Availability:
                                                            <asp:Label runat="server" ID="lbltotala"></asp:Label>
                                                        </td>
                                                        <td align="right" width="64%" valign="middle" style="display: none;">Search&nbsp;:
                                                        </td>
                                                        <td style="width: 2%; display: none;">
                                                            <asp:DropDownList ID="ddlSearch" runat="server" Width="115px" AutoPostBack="False"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="Name">Name</asp:ListItem>
                                                                <asp:ListItem Value="Email">Email</asp:ListItem>
                                                                <asp:ListItem Value="SKU" Selected="True">SKU</asp:ListItem>
                                                                <asp:ListItem Value="ProductName">Product Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle" width="2%" style="display: none;">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Style="width: 124px; vertical-align: text-top"
                                                                ValidationGroup="SearchGroup"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 2%; display: none;">
                                                            <asp:Button ID="btnSearch" runat="server" OnClientClick="return validation();" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td style="padding: 0px 0px; width: 2%; display: none;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" OnClientClick="javascript:chkHeight();"
                                                                CausesValidation="False" />
                                                            <div style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <td colspan="5" align="left" style="padding-right: 4px">Total Availability:
                                                            <asp:Label runat="server" ID="lbltotala"></asp:Label>
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <asp:GridView ID="grdshirange" AutoGenerateColumns="false" runat="server" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" DataKeyNames="ShippingPriceRangeID" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" ShowHeaderWhenEmpty="true"
                                                    OnRowDataBound="grdProduct_RowDataBound" OnRowCommand="grdProduct_RowCommand" OnPageIndexChanging="grdshirange_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:HiddenField ID="hdnShippingRangeID" runat="server" Value='<%#Eval("ShippingPriceRangeID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="FromPrice" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                $ From Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="lblFromPrice" runat="server" Text='<%# Bind("FromPrice") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ToPrice" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                $ To Price
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="lblToPrice" runat="server" Text='<%# Bind("ToPrice") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="left" Width="20%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                $ Price (Discount)
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ToPrice" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                Status
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="left" Width="20%"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hplEdit" runat="server" NavigateUrl='<%#"EditShippingRange.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "ShippingPriceRangeID") +"&storeId="+ DataBinder.Eval(Container.DataItem, "StoreID")%>'
                                                                    ImageUrl="/App_Themes/gray/Images/edit.gif"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" id="trBottom" runat="server">
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
