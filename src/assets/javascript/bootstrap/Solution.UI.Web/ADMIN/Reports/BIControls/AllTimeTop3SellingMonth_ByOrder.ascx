<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeTop3SellingMonth_ByOrder.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeTop3SellingMonth_ByOrder" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var prmAllTimeTop3SellingMonth_ByOrder = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeTop3SellingMonth_ByOrder(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1] + "_" + sil[2]) == "AllTimeTop3SellingMonth_ByOrder") {
                document.getElementById("Top3SellingMonthOrder-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeTop3SellingMonth_ByOrder(sender, args) {
            document.getElementById("Top3SellingMonthOrder-blocker").style.display = "none";
        }
        prmAllTimeTop3SellingMonth_ByOrder.add_beginRequest(BeginRequestHandlerAllTimeTop3SellingMonth_ByOrder);
        prmAllTimeTop3SellingMonth_ByOrder.add_endRequest(EndRequestHandlerAllTimeTop3SellingMonth_ByOrder);
    </script>
</div>
<div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="border-td">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="content-table">
                    <tr>
                        <td class="border-td-sub">
                            <asp:UpdatePanel ID="upnltop3monthorder" runat="server">
                                <ContentTemplate>
                                    <%--<script type="text/javascript">
                                        Sys.Application.add_load(AllTimeRevenueDatetime);
                                    </script>--%>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 3 Selling Month by Order" alt="Top 3 Selling Month by Order" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 3 Selling Month by Order</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-topsellingmonthorder" title="Minimize" onclick="showorhide('topsellingmonthorder');" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="topsellingmonthorder-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <%--<td>
                                                            <asp:Button ID="btnToday" runat="server" ToolTip="Today" Text="D" Class="btn-checked" OnClick="btnToday_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" ToolTip="Weekly" Text="W" Class="btn-unchecked" OnClick="btnWeek_Click" />
                                                        </td>--%>
                                                        <%--<td>
                                                            <asp:Button ID="btnmonth" runat="server" ToolTip="Current Month" Text="M" Class="btn-unchecked" OnClick="btnmonth_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnQuarter" runat="server" ToolTip="Quarter" Text="Q" Class="btn-unchecked" OnClick="btnQuarter_Click" />
                                                        </td>--%>
                                                        <td>
                                                            <asp:Button ID="btnHalfyear" runat="server" Visible="false" ToolTip="Half Year" Text="HY" Class="btn-unchecked" OnClick="btnHalfyear_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnYear" runat="server" ToolTip="Year" Text="Y" Class="btn-checked" OnClick="btnYear_Click" />
                                                        </td>
                                                        <%--<td id="dateFrom-name" align="left" style="width: 40px;">From :
                                                        </td>
                                                        <td id="dateFrom-value" align="left" style="width: 100px;">
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td id="dateTo-name" align="left" style="width: 40px;">To :
                                                        </td>
                                                        <td id="dateTo-value" align="left" style="width: 100px;">
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation();" />
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                        <tr id="topsellingmonthorder-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="Top3SellingMonthOrderDiv" style="min-height: 211px;">
                                                </div>
                                                <div id="Top3SellingMonthOrder-blocker" class="loader-div">
                                                        <div>Loading...</div>
                                                 </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnHalfyear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnYear" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>