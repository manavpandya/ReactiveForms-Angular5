<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesAgentOwnOpenQuotes.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.SalesAgentOwnOpenQuotes" %>
<div>
    <table style="width:100%;" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Sales Agent own Open Quotes"
                            title="Sales Agent own Open Quotes" class="img-left" />
                        <h2>
                            Sales Agent own Open Quotes</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgItemOpenQuotes','tr5TenItemsOpenQuotes','tblItemOpenQuotes');">
                            <img class="minimize" title="Minimize" id="imgItemOpenQuotes" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="tr5TenItemsOpenQuotes">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdSalesAgentQuotelist" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1 %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CustomerName">
                                <ItemTemplate>
                                   <%-- <a href="/Admin/Products/Product.aspx?StoreID=<%# Eval("StoreID") %>&ID=<%# Eval("RefProductID") %>&Mode=edit"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title='<%# Convert.ToString(Eval("ProductPattern")) %>'>--%>
                                        <%# SetItemName(Convert.ToString(Eval("FirstName")))%></a></ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="QuoteNumber">
                                <ItemTemplate>
                                    <%# Eval("QuoteNumber")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
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
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Orders/CustomerQuoteList.aspx?Storeid=<%=storeid%>&loginid=<%=loginid%>" style="cursor: pointer;"><span>View List</span></a>
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