<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeAverageOrders.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeAverageOrders" %>
<div>

    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function avgOrderDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_AllTimeAverageOrders_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllTimeAverageOrders_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
            });
        }
        var prmAllTimeAverageOrders = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeAverageOrders(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if (sil[1] == "AllTimeAverageOrders") {
                document.getElementById("avgOrder-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeAverageOrders(sender, args) {
            document.getElementById("avgOrder-blocker").style.display = "none";
        }
        prmAllTimeAverageOrders.add_beginRequest(BeginRequestHandlerAllTimeAverageOrders);
        prmAllTimeAverageOrders.add_endRequest(EndRequestHandlerAllTimeAverageOrders);
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
                            <asp:UpdatePanel ID="upnlAvgOrder" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(avgOrderDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="All Time Average Order" alt="All Time Average Order" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>All Time Average Orders</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-avgorder" title="Minimize" alt="Minimize" onclick="showorhide('avgorder');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="avgorder-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnToday" runat="server" CssClass="btn-unchecked" ToolTip="Today" Text="D" OnClick="btnToday_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" CssClass="btn-unchecked" ToolTip="Weekly" Text="W" OnClick="btnWeek_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnmonth" runat="server" CssClass="btn-unchecked" ToolTip="Current Month" Text="M" OnClick="btnmonth_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnQuarter" runat="server" CssClass="btn-unchecked" ToolTip="Quarter" Text="Q" OnClick="btnQuarter_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnHalfyear" runat="server" CssClass="btn-unchecked" ToolTip="Half Year" Text="HY" OnClick="btnHalfyear_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnYear" runat="server" CssClass="btn-checked" ToolTip="Year" Text="Y" OnClick="btnYear_Click" />
                                                        </td>
                                                        <td id="dateFrom-name" align="left" style="width: 36px;">From :
                                                        </td>
                                                        <td id="dateFrom-value" align="left" style="width: 100px;">
                                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td id="dateTo-name" align="left" style="width: 20px;">To :
                                                        </td>
                                                        <td id="dateTo-value" align="left" style="width: 100px;">
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllTimeAverageOrders');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="avgorder-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <asp:Label ID="lblOrderAverage" Font-Size="15pt" runat="server" Text="0"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="avgorder-row-chart">
                                            <td align="center" colspan="2" style="position: relative;">
                                                <div id="AverageOrderdiv">
                                                </div>
                                                
                                                <div id="avgOrder-blocker" class="loader-div">
                                                    <div>Loading...</div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnToday" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnWeek" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnmonth" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnQuarter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnHalfyear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnYear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>