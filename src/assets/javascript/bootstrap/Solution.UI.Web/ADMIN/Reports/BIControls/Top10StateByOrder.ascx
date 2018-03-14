<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top10StateByOrder.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.Top10StateByOrder" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Top10StateByOrderDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_Top10StateByOrder_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_Top10StateByOrder_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });

            });

        }
        var prmAllTimeRevenue = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeRevenue(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "Top10StateByOrder") {
                document.getElementById("top10statebyorder-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeRevenue(sender, args) {
            document.getElementById("top10statebyorder-blocker").style.display = "none";
        }
        prmAllTimeRevenue.add_beginRequest(BeginRequestHandlerAllTimeRevenue);
        prmAllTimeRevenue.add_endRequest(EndRequestHandlerAllTimeRevenue);
    </script>
    <script type="text/javascript">
        //google.load('visualization', '1', { packages: ['treemap'] });
        //google.setOnLoadCallback(top10statebyorder);
        //Sys.Application.add_load(top10statebyorder);
        //var Option = {
        //    minColor: '#f00',midColor: '#ddd',
        //    maxColor: '#0d0',headerHeight: 15,fontColor: 'black',showScale: true,
        //    generateTooltip: showFullTooltip
        //}
        //function top10statebyorder() {
        //    var data = google.visualization.arrayToDataTable(
        //        [['Location', 'Parent', 'Market trade volume (size)', 'Market increase/decrease (color)'],
        //            ['Global',null,0,0],['United States','Global',0,0],
        //            ['Canada','Global',0,0],['Austria','Global',0,0],['India','Global',0,0],
        //            ['Massachusetts','United States',542,10],['New York','United States',36,31],
        //            ['New Jersey','United States',29,12],['Illinois','United States',10,-23],
        //            ['California','United States',5,-11],['canada','Canada',4,-2],['def','Canada',1,-13],['Alberta','Canada',1,4],
        //            ['asd','Canada',1,-5],['austriother','Austria',1,4],['Gujarat','India',1,-12]]);
        //            tree = new google.visualization.TreeMap(document.getElementById('top10statebyorder'));
        //            tree.draw(data,Option );
        //            function showFullTooltip(row, size, value) {return '<div style="background:#fd9; padding:10px; border-style:solid">(Total Number of Orders): ' + value + ' </div>';}};
    </script>
    <style type="text/css">
        #chartdiv {
            width: 100%;
        }

            #chartdiv div {
                position: relative;
                /*width: 100% !important;*/
            }
    </style>
</div>
<div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="border-td">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="content-table">
                    <tr>
                        <td class="border-td-sub">
                            <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>--%>
                            <asp:UpdatePanel ID="upnltop10stateorder" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(Top10StateByOrderDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 10 State by Revenue" alt="All Time Revenue" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 10 State by Order</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-Top10StateByOrder" title="Minimize" alt="Minimize" onclick="showorhide('Top10StateByOrder');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="Top10StateByOrder-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnToday" runat="server" CssClass="btn-unchecked" ToolTip="Today" OnClick="btnToday_Click" Text="D" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" CssClass="btn-unchecked" ToolTip="Weekly" OnClick="btnWeek_Click" Text="W" />
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('Top10StateByOrder');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="Top10StateByOrder-row-total">
                                            <td>
                                                <%--<div style="margin-left: 10px; visibility: hidden">
                                                    <asp:Label ID="lblTotalRevenue" Font-Size="15pt" margin-left="5px" runat="server" Text="0"></asp:Label>
                                                </div>--%>
                                            </td>
                                        </tr>
                                        <tr id="Top10StateByOrder-row-chart">
                                            <td align="center" colspan="2" style="position: relative;">
                                                <div id="top10statebyorder">
                                                </div>
                                                <div id="top10statebyorder-blocker" class="loader-div">
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
    <div style="display: none;">
        <asp:Literal ID="ltalltimerevenue" runat="server"></asp:Literal>
    </div>

</div>
