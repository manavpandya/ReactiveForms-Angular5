<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesAgentOwnSales.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.SalesAgentOwnSales" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th colspan="4">
                    <div class="main-title-left">
                        <img class="img-left" title="Sales Agent own sales" alt="Sales Agent own sales" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                        <h2>
                           Sales Agent own sales</h2>
                    </div>
                    <div class="main-title-right">
                     <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgTopTenOrderSalesAgent','trTopTenOrderSalesAgent','tempDivSalesAgent');">
                            <img class="minimize" id="imgTopTenOrderSalesAgent" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trTopTenOrderSalesAgent">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdTopTenOrders" runat="server" CellPadding="0" CssClass="dashboard-left"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" BorderStyle="Solid" BorderColor="#DFDFDF" CellSpacing="1"   GridLines="None"
                        EmptyDataText="No Order(s) Found." EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row"
                        EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#" Visible="false">
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
                           
                          
                            <asp:TemplateField HeaderText="Order Total">
                                <ItemTemplate>
                                    <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" BorderWidth="0" />
                            </asp:TemplateField>
                         
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="tblItem">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%"></td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/SalesAgentOwnSalesReport.aspx?Storeid=<%=storeid%>" style="cursor: pointer;"><span>View List</span></a>
                                    <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>

        </tbody>
    </table>
</div>
