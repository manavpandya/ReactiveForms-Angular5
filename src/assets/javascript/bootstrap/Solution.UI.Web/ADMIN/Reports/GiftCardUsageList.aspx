<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="GiftCardUsageList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.GiftCardUsageList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                <table>
                    <tr>
                        <td style="padding-left: 0px;">
                            Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstore" runat="server" CssClass="order-list" Width="170px"
                                OnSelectedIndexChanged="ddlstore_SelectedIndexChanged" AutoPostBack="true">
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
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
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
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Gift Card Usage List" alt="Gift Card Usage List" src="/App_Themes/<%=Page.Theme %>/Images/credit-card-list-icon.png" />
                                                <h2>
                                                    Gift Card Usage List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grdGiftCardUsage" runat="server" AutoGenerateColumns="False" DataKeyNames="GiftCardUsageID"
                                                BorderStyle="Solid" BorderWidth="1" CellSpacing="1" BorderColor="#E7E7E7" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2" onrowdatabound="grdGiftCardUsage_RowDataBound"  DataSourceID="_gridObjectDataSource"
                                               >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Credit Card Type" >
                                                        <HeaderTemplate>
                                                           Gift Card Name
                                                            <asp:ImageButton ID="lblName" runat="server" CommandArgument="DESC" CommandName="SerialNumber"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Order Number" >
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" Width="10%"/>
                                                        <ItemTemplate>
                                                             <a href="/Admin/Orders/Orders.aspx?id=<%# Eval("OrderNumber") %>" ><%# Eval("OrderNumber") %> </a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Amount" >
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblAmount" runat="server" Text='<%# String.Format ("{0:C}",Convert.ToDecimal( DataBinder.Eval(Container.DataItem,"Amount")))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Used By" >
                                                      <HeaderTemplate>
                                                           Used By
                                                            <asp:ImageButton ID="lblUsedBy" runat="server" CommandArgument="DESC" CommandName="CustomerName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblCustomerName" runat="server" Text='<%# bind("CustomerName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" >
                                                        <HeaderTemplate>
                                                            Store Name
                                                            <asp:ImageButton ID="lblstorename" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblstore" runat="server" Text='<%# bind("StoreName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Used On" >
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%"/>
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblUsedOn" runat="server" Text='<%# bind("CreatedOn") %>'></asp:Label>
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
                                             <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                TypeName="Solution.Bussines.Components.GiftCardUsageComponent" SelectMethod="GetDataByFilter"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
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
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
