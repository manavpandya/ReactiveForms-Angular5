<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopTenOrders.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.TopTenOrders" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-center">
        <tbody>
            <tr>
                <th colspan="4">
                    <div class="main-title-left">
                        <img class="img-left" title="Top 10 Orders" alt="Top 10 Orders" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                        <h2>
                            Top 10 Orders</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgTopTenOrder','trTopTenOrder','tempDiv');">
                            <img class="minimize" id="imgTopTenOrder" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trTopTenOrder">
                <td colspan="3">
                    <asp:GridView ID="grdTopTenOrders" runat="server" CellPadding="0" CssClass="dashboard-left"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" BorderStyle="Solid" BorderColor="#DFDFDF" CellSpacing="1"   GridLines="None"
                        EmptyDataText="No Order(s) Found." EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row"
                        OnRowDataBound="grdTopTenOrders_RowDataBound" EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Number">
                                <ItemTemplate>
                                    <div style="float:left;padding-left:4px;"><img src="/admin/images/<%# Eval("StoreID")%>.png"  /></div>
                                    <div style="float:left;padding-left:4px;"><a href="Orders/Orders.aspx?id=<%# Eval("OrderNumber")%>" style="font-weight: normal;
                                        text-decoration: underline; font-size: 11px;">
                                        <%# Eval("OrderNumber")%></a></div>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date">
                                <ItemTemplate>
                                    <%# String.Format("{0:MM/dd/yyyy}", Eval("OrderDate"))%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title="Click to Edit Customer">
                                        <%# Eval("CustomerName")%></a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# Eval("Email")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Total">
                                <ItemTemplate>
                                    <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
        </tbody>
    </table>
</div>
