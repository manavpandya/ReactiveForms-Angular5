<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentCustomer.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.RecentCustomer" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th colspan="3">
                    <div class="main-title-left">
                        <img class="img-left" title="Recent Customer" alt="Recent Customer" src="/App_Themes/<%=Page.Theme %>/icon/customers-by-profitability.png" />
                        <h2>
                            Recent Customers</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgRecentCustomer','trRecentCustomerlist','trRecentCustomerView');">
                            <img class="minimize" id="imgRecentCustomer" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trRecentCustomerlist">
                <td colspan="3">
                    <asp:GridView ID="grdRecentCustomerlist" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Customer(s) Found."
                        EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red"
                        Style="margin-bottom: 0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="Customer">
                                <ItemTemplate>
                                    <a href="/Admin/Customers/Customer.aspx?mode=edit&CustID=<%# Eval("CustomerID") %>"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title="Click to Edit Customer">
                                        <%# Eval("CustomerName")%></a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <a href="mailto:<%# Eval("Email")%>" style="color: #6A6A6A; text-decoration: none; font-weight: normal;
                                        font-size: 11px;">
                                        <%# Eval("Email")%>
                                    </a>&nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trRecentCustomerView">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                    <a title="Add Customer" href="/Admin/Customers/Customer.aspx" style="cursor: pointer;">
                                        <span>Add Customer</span></a>
                                    <img class="img-right" title="Add Customer" alt="Add Customer" src="/App_Themes/<%=Page.Theme %>/Icon/Add_Product.gif" />
                                </td>
                                <td align="right" width="50%">
                                    <a title="Customer List" href="/Admin/Customers/CustomerList.aspx" style="cursor: pointer;">
                                        <span>Customer List</span></a>
                                    <img class="img-right" title="Customer List" alt="Customer List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
