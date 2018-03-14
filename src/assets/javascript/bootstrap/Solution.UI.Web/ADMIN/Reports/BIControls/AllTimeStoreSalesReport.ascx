<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeStoreSalesReport.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeStoreSalesReport" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function StoreSalesReportDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_AllTimeStoreSalesReport_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllTimeStoreSalesReport_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });

            });
        }
        var prmAllTimeStoreSalesReport = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeStoreSalesReport(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "AllTimeStoreSalesReport") {
                document.getElementById("StoreSalesReport-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeStoreSalesReport(sender, args) {
            document.getElementById("StoreSalesReport-blocker").style.display = "none";
        }
        prmAllTimeStoreSalesReport.add_beginRequest(BeginRequestHandlerAllTimeStoreSalesReport);
        prmAllTimeStoreSalesReport.add_endRequest(EndRequestHandlerAllTimeStoreSalesReport);
    </script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</div>
<div>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="border-td">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="content-table">
                    <tr>
                        <td class="border-td-sub">
                            <asp:UpdatePanel ID="upnlStoreSalesReport" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(StoreSalesReportDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Store Wise Sales Report" alt="Store Wise Sales Report" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Store Wise Sales Report</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-StoreSalesReport" title="Minimize" alt="Minimize" onclick="showorhide('StoreSalesReport')" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="StoreSalesReport-row-filter" class="altrow">
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllTimeStoreSalesReport');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="StoreSalesReport-row-total">
                                            <td></td>
                                        </tr>
                                        <tr id="StoreSalesReport-row-chart">
                                            <td align="center" style="position: relative;">
                                                <div id="StoreSalesReportchartdiv" style="min-height:297px;">
                                                </div>
                                                <div id="StoreSalesReport-blocker" class="loader-div">
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
