<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top10BestPatterns.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.Top10BestPatterns" %>
<div>
    <table style="width:100%;" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Top 10 Best Selling Pattern"
                            title="Top 10 Best Selling Pattern" class="img-left" />
                        <h2>
                            Top 10 Best Selling Pattern</h2>
                    </div>
                    <div class="main-title-right">
                       <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgItemPattern','tr5TenItemsPattern','tblItemPattern');">
                            <img class="minimize" title="Minimize" id="imgItemPattern" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="tr5TenItemsPattern">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdTopPatternlist" runat="server" CssClass="dashboard-left" CellPadding="0"
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
                            <asp:TemplateField HeaderText="PatternName">
                                <ItemTemplate>
                                   <%-- <a href="/Admin/Products/Product.aspx?StoreID=<%# Eval("StoreID") %>&ID=<%# Eval("RefProductID") %>&Mode=edit"
                                        style="font-weight: normal; color: #6A6A6A; font-size: 11px" title='<%# Convert.ToString(Eval("ProductPattern")) %>'>--%>
                                        <%# SetItemName(Convert.ToString(Eval("ProductPattern")))%></a></ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sold">
                                <ItemTemplate>
                                    <%# Eval("SoldQty")%>
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
                    <table style="width:100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/TopSellingProductsPatternList.aspx?Storeid=<%=storeid%>" style="cursor: pointer;"><span>View List</span></a>
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