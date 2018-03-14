<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top25SellingDays.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.Top25SellingDays" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var prmTop25SellingDays = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerTop25SellingDays(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "Top25SellingDays") {
                document.getElementById("top25Selling-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerTop25SellingDays(sender, args) {
            document.getElementById("top25Selling-blocker").style.display = "none";
        }
        prmTop25SellingDays.add_beginRequest(BeginRequestHandlerTop25SellingDays);
        prmTop25SellingDays.add_endRequest(EndRequestHandlerTop25SellingDays);
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
                            <asp:UpdatePanel ID="upnltop25Sellingday" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(AllTimeRevenueDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 10 Selling Days" alt="Top 10 Selling Days" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 10 Selling Days by Revenue</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-topsellingday" title="Minimize" onclick="showorhide('topsellingday');" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="topsellingday-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>

                                                        <%--<td>
                                                            <asp:Button ID="btnToday" runat="server" ToolTip="Today" Text="D" Class="btn-checked" OnClick="btnToday_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" ToolTip="Weekly" Text="W" Class="btn-unchecked" OnClick="btnWeek_Click" />
                                                        </td>--%>
                                                        <td>
                                                            <asp:Button ID="btnmonth" runat="server" ToolTip="Current Month" Text="M" Class="btn-unchecked" OnClick="btnmonth_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnQuarter" runat="server" ToolTip="Quarter" Text="Q" Class="btn-unchecked" OnClick="btnQuarter_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnHalfyear" runat="server" ToolTip="Half Year" Text="HY" Class="btn-unchecked" OnClick="btnHalfyear_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnYear" runat="server" ToolTip="Year" Text="Y" Class="btn-checked" OnClick="btnYear_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="topsellingday-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <%--<asp:Label ID="ldlTop25SellingDays" Font-Size="15pt" runat="server" Text="0"></asp:Label>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="topsellingday-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="Top25SellingDaysDiv" style="min-height: 203px;">
                                                </div>
                                                <div id="top25Selling-blocker" class="loader-div">
                                                        <div>Loading...</div>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnmonth" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnQuarter" EventName="Click" />
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
