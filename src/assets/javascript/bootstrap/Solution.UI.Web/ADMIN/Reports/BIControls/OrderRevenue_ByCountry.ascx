<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderRevenue_ByCountry.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.OrderRevenue_ByCountry" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function orderrevenuebycountrystateDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_OrderRevenue_ByCountry_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_OrderRevenue_ByCountry_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
            });
        }
        var prmOrderRevenueCountry = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerOrderRevenueCountry(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1] + "_" + sil[2]) == "OrderRevenue_ByCountry") {
                document.getElementById("orderrevenue-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerOrderRevenueCountry(sender, args) {
            document.getElementById("orderrevenue-blocker").style.display = "none";
        }
        prmOrderRevenueCountry.add_beginRequest(BeginRequestHandlerOrderRevenueCountry);
        prmOrderRevenueCountry.add_endRequest(EndRequestHandlerOrderRevenueCountry);
    </script>
    <style>
        #OrderRevenuechartdiv span{
            display : none !important;
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
                            <asp:UpdatePanel ID="upnlOrderRevenueCountry" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(orderrevenuebycountrystateDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Order Revenue By Country" alt="Order Revenue By Country" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Order by Country/State</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-revenuebycountry" title="Minimize" alt="Minimize" onclick="showorhide('revenuebycountry');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="revenuebycountry-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>Country :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="select-box-pro" Style="width: 200px;" AutoPostBack="true">
                                                                <asp:ListItem Value="0">Year</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>State :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="select-box-pro" Style="width: 150px;" AutoPostBack="true">
                                                                <asp:ListItem Value="0">State</asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>
                                                </table>
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
                                                    <a href="javascript:void(0);" onclick="DirectBtnClick('ByDay');" title="Last Year">LY</a>
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('OrderRevenue_ByCountry');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="revenuebycountry-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <asp:Label ID="lblTotalOrders" Font-Size="15pt" runat="server" Text="0"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="revenuebycountry-row-chart">
                                            <td align="center" colspan="2" style="position: relative;">
                                                <div id="OrderRevenuechartdiv">
                                                </div>
                                                <div id="orderrevenue-blocker" class="loader-div">
                                                    <div style="margin-top: 12% !important;">Loading...</div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />--%>
                                    <asp:AsyncPostBackTrigger ControlID="btnmonth" EventName="Click" />

                                    <asp:AsyncPostBackTrigger ControlID="btnYear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnHalfyear" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnQuarter" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnToday" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnWeek" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>