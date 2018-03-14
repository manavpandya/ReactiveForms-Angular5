<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="VendorPaymentList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.VendorPaymentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="EditVendorPaymentDetails.aspx">
                    <img alt="Add Vendor/DropShipper Payment" title="Add Vendor/DropShipper Payment" src="/App_Themes/<%=Page.Theme %>/images/add-vendor-dropshipper-payment.png" /></a></span>
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
                                                    <img class="img-left" title="Vendor/DropShipper Payment List" alt="Vendor/DropShipper Payment List" src="/App_Themes/<%=Page.Theme %>/Images/vendor-list-icon.png">
                                                    <h2>
                                                        Vendor/DropShipper Payment List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.VendorComponent" SelectMethod="GetDataByFilterForVendorPayment"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCountForVendorPayment">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlVendor" DbType="Int32" Name="VendorID" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="ddlSearch" DbType="String" Name="SearchBy" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 12%;">
                                                            Vendor/DropShipper Name:
                                                        </td>
                                                        <td style="width: 48%">
                                                            <asp:DropDownList ID="ddlVendor" runat="server" AutoPostBack="true" CssClass="order-list">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:DropDownList ID="ddlSearch" AutoPostBack="False" Width="175px" runat="server"
                                                                CssClass="order-list">
                                                                <asp:ListItem Value="Name">Vendor/DropShipper Name</asp:ListItem>
                                                                <asp:ListItem Value="PaidBy">Paid By</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td style="width: 5%; padding-right: 0px;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdVendorPayment" runat="server" AutoGenerateColumns="False" DataKeyNames="VendorID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    DataSourceID="_gridObjectDataSource" 
                                                    onrowdatabound="grdVendorPayment_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Vendor/DropShipper Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Vendor/DropShipper Name
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
                                                                Paid&nbsp;By
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPaidBy" runat="server" Text='<%# Bind("PaidBy") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Transaction Reference" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTranRef" runat="server" Text='<%# Bind("TransactionReference") %>'
                                                                    Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Transaction Date
                                                                 <asp:ImageButton ID="btnTransDate" runat="server" CommandArgument="DESC" CommandName="TransactionDate"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTransDate" runat="server" Text='<%# Bind("TransactionDate","{0:MM/dd/yyyy}") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="	Purchase Orders" HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("PurchaseOrders") %>' Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                	PO Amount
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPoAmount" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"POAmount")),2) %>' Style="text-align: right;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="	Paid Amount" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTPaidAmt" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"PaidAmount")),2) %>' Style="text-align: right;"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("VendorID") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <%--<tr class="altrow" runat="server" id="trBottom">
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
                                        </tr>--%>
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
