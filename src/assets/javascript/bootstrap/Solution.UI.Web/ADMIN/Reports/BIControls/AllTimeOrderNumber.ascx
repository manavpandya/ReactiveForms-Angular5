<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeOrderNumber.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeOrderNumber" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function OrderNumberDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_AllTimeOrderNumber_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllTimeOrderNumber_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
            });
        }
        var prmAllTimeOrderNumber = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeOrderNumber(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "AllTimeOrderNumber") {
                document.getElementById("ordernumber-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeOrderNumber(sender, args) {
            document.getElementById("ordernumber-blocker").style.display = "none";
        }
        prmAllTimeOrderNumber.add_beginRequest(BeginRequestHandlerAllTimeOrderNumber);
        prmAllTimeOrderNumber.add_endRequest(EndRequestHandlerAllTimeOrderNumber);
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
                            <asp:UpdatePanel ID="upnlOrderNUmber" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(OrderNumberDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="All Time Number of Order" alt="All Time Number of Order" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>All Time Number of Orders</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-ordernumber" title="Minimize" alt="Minimize" onclick="showorhide('ordernumber')" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="ordernumber-row-filter" class="altrow">
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
                                                        <%--<td>
                                                            <button type="button" id="btnToday" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">D</button>
                                                        </td>
                                                        <td>
                                                            <button type="button" id="btnWeek" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">W</button>
                                                        </td>
                                                        <td>
                                                            <button type="button" id="btnmonth" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">M</button>
                                                        </td>
                                                        <td>
                                                            <button type="button" id="btnQuarter" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">Q</button>
                                                        </td>
                                                        <td>
                                                            <button type="button" id="btnHalfyear" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">HY</button>
                                                        </td>
                                                        <td>
                                                            <button type="button" id="btnYear" runat="server" class="btn-unchecked" onclick="drawChartAnimate();">Y</button>
                                                        </td>--%>

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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllTimeOrderNumber');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="ordernumber-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <asp:Label ID="lblTotalOrders" Font-Size="15pt" runat="server" Text="0"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="ordernumber-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="OrderNumberchartdiv">
                                                </div>
                                                <div id="OrderNumberchartdivAnimate">
                                                </div>
                                                <div id="ordernumber-blocker" class="loader-div">
                                                        <div>Loading...</div>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnToday" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnWeek" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnmonth" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnQuarter" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnHalfyear" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnYear" eventname="Click"/>
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" eventname="Click"/>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>