<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChartBusinessTrend.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.ChartBusinessTrend" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-center">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/chart-business-trend.png" alt="Chart/Business Trend"
                            title="Chart/Business Trend" class="img-left" />
                        <h2>
                            Chart/Business Trend</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgChart','trChart','tempDiv');">
                            <img class="minimize" id="imgChart" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trChart">
                <td>
                    <div id="tab-container" style="padding-bottom:0px;">
                        <ul class="menu">
                            <li id="lic1" class="active" onclick="TabdisplayDashboard(1);">Amount</li>
                            <li id="lic2" onclick="TabdisplayDashboard(2);">Orders</li>
                        </ul>
                        <span style="float: right; text-decoration: none;">
                            <asp:DropDownList ID="ddlOption" onchange="chkHeight();" runat="server" CssClass="order-list" Style="width: 100px;
                                font-family: Arial,Helvetica,sans-serif; font-size: 11px; text-decoration: none;"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                                <asp:ListItem Value="Last24hours">Last 24 hours</asp:ListItem>
                                <asp:ListItem Value="Last7days">Last 7 days</asp:ListItem>
                                <asp:ListItem Value="ThisMonth" Selected="True">This Month</asp:ListItem>
                                <asp:ListItem Value="LastMonth">Last Month</asp:ListItem>
                                <asp:ListItem Value="Quarterly">Quarterly</asp:ListItem>
                                <asp:ListItem Value="ThisYear">This Year</asp:ListItem>
                                <asp:ListItem Value="LastYear">Last Year</asp:ListItem>
                            </asp:DropDownList>
                        </span><span class="clear"></span>
                        <div class="tab-content general-tab" id="divtab2" style="display: none;">
                            <div class="tab-content-3">
                                <asp:Literal ID="ltrChartOrder" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="tab-content general-tab" id="divtab1" style="display: block;">
                            <div class="tab-content-3">
                                <asp:Literal ID="ltrChartAmount" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <table cellpadding="0" cellspacing="0" width="100%" border="0px" class="">
                            <tr>
                                <td align="center">
                                    <strong>Revenue</strong>
                                </td>
                                <td align="center" class="">
                                    <strong>Order Amount</strong>
                                </td>
                                <td align="center" class="">
                                    <strong>Tax</strong>
                                </td>
                                <td align="center" class="">
                                    <strong>Shipping</strong>
                                </td>
                                <td align="center" class="">
                                    <strong>Quantity</strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblRevenue" runat="server" Style="text-decoration: none;" Text="0.00"></asp:Label>
                                </td>
                                <td align="center" class="">
                                    <asp:Label ID="lblAuthorized" Style="text-decoration: none" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td align="center" class="">
                                    <asp:Label ID="lblTax" Style="text-decoration: none" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td align="center" class="">
                                    <asp:Label ID="lblShipping" Style="text-decoration: none" runat="server" Text="0.00"></asp:Label>
                                </td>
                                <td align="center" class="">
                                    <asp:Label ID="lblQuantity" Style="text-decoration: none" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                              
                        </table>
                        
                    </div>
                  
                </td>
            </tr>
        </tbody>
    </table>
    
</div>
