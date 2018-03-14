<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeTop3SellingMonth_ByRevenu.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeTop3SellingMonth_ByRevenu" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var prmAllTimeTop3SellingMonth_ByRevenu = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeTop3SellingMonth_ByRevenu(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1] + "_" + sil[2]) == "AllTimeTop3SellingMonth_ByRevenu") {
                document.getElementById("Top3SellingMonthRevenu-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeTop3SellingMonth_ByRevenu(sender, args) {
            document.getElementById("Top3SellingMonthRevenu-blocker").style.display = "none";
        }
        prmAllTimeTop3SellingMonth_ByRevenu.add_beginRequest(BeginRequestHandlerAllTimeTop3SellingMonth_ByRevenu);
        prmAllTimeTop3SellingMonth_ByRevenu.add_endRequest(EndRequestHandlerAllTimeTop3SellingMonth_ByRevenu);
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
                            <asp:UpdatePanel ID="upnltop3monthrevenue" runat="server">
                                <ContentTemplate>
                                    <%--<script type="text/javascript">
                                        Sys.Application.add_load(AllTimeRevenueDatetime);
                                    </script>--%>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 3 Selling Month by Revenue" alt="Top 3 Selling Month by Revenue" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 3 Selling Month by Revenue</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-topsellingmonthrevenu" title="Minimize" onclick="showorhide('topsellingmonthrevenu');" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="topsellingmonthrevenu-row-filter" class="altrow">
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
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                        <tr id="topsellingmonthrevenu-row-chart">
                                            <td align="center" colspan="2" style="position: relative;">
                                                <div id="Top3SellingMonthRevenueDiv" style="min-height: 211px;">
                                                </div>
                                                <div id="Top3SellingMonthRevenu-blocker" class="loader-div">
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
