﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeCustomerByRevenue.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeCustomerByRevenue" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function CustomerByRevenueDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_AllTimeCustomerByRevenue_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllTimeCustomerByRevenue_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });

            });
        }

        var prmAllTimeCustomerByRevenue = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeCustomerByRevenue(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "AllTimeCustomerByRevenue") {
                document.getElementById("CustomerRevenue-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeCustomerByRevenue(sender, args) {
            document.getElementById("CustomerRevenue-blocker").style.display = "none";
        }
        prmAllTimeCustomerByRevenue.add_beginRequest(BeginRequestHandlerAllTimeCustomerByRevenue);
        prmAllTimeCustomerByRevenue.add_endRequest(EndRequestHandlerAllTimeCustomerByRevenue);
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
                            <asp:UpdatePanel ID="upnlCustomerRevenue" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(CustomerByRevenueDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 10 Customer by Revenue" alt="Top 10 Customer by Revenue" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 10 Customer by Revenue</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-CustomerRevenue" title="Minimize" alt="Minimize" onclick="showorhide('CustomerRevenue')" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="CustomerRevenue-row-filter" class="altrow">
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
                                                        <td id="dateTo-name" align="left" style="width: 19px;">To :
                                                        </td>
                                                        <td id="dateTo-value" align="left" style="width: 100px;">
                                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllTimeCustomerByRevenue');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="CustomerRevenue-row-total">
                                            <td>
                                                
                                            </td>
                                        </tr>
                                        <tr id="CustomerRevenue-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="CustomerRevenuechartdiv" style="min-height:297px;">
                                                </div>
                                                <div id="CustomerRevenue-blocker" class="loader-div">
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