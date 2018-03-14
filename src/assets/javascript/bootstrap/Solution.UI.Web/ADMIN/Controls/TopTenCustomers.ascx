<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopTenCustomers.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.TopTenCustomers" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th colspan="3">
                    <div class="main-title-left">
                        <img class="img-left" title="Top 10 Customers By Profitability" alt="Top 10 Customers By Profitability"
                            src="/App_Themes/<%=Page.Theme %>/icon/customers-by-profitability.png" />
                        <h2>
                            Top&nbsp;10&nbsp;Customers&nbsp;by&nbsp;Profitability</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgCustomer','trCustomerlist','trCustomerView');">
                            <img class="minimize" id="imgCustomer" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trCustomerlist">
                <td colspan="3">
                    <asp:GridView ID="grdCustomerlist" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Customer(s) Found."
                        EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red"
                        Style="margin-bottom: 0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer">
                                <ItemTemplate>
                                    <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title="Click to Edit Customer">
                                        <%# Eval("CustName")%></a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Profit&nbsp;">
                                <ItemTemplate>
                                    <%#String.Format("{0:C}", Convert.ToDecimal(Eval("ProfiteTotal")))%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trCustomerView">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/CustomerProfitableReport.aspx?Storeid=<%=storeid%>" style="cursor: pointer;">
                                        <span>View List</span></a>
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
