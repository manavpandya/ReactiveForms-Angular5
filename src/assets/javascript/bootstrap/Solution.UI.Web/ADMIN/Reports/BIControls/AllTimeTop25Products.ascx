﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTimeTop25Products.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllTimeTop25Products" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Top25ProductsDateTime() {
            $(function () {
                $('#ContentPlaceHolder1_AllTimeTop25Products_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllTimeTop25Products_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
            });
        }
        var prmAllTimeTop25Products = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllTimeTop25Products(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "AllTimeTop25Products") {
                document.getElementById("top25products-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeTop25Products(sender, args) {
            document.getElementById("top25products-blocker").style.display = "none";
        }
        prmAllTimeTop25Products.add_beginRequest(BeginRequestHandlerAllTimeTop25Products);
        prmAllTimeTop25Products.add_endRequest(EndRequestHandlerAllTimeTop25Products);
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
                            <asp:UpdatePanel ID="upnltop25products" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(Top25ProductsDateTime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="All Time Top 25 Product" alt="All Time Top 25 Product" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 10 Products by Quantity</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-topproducts" title="Minimize" alt="Minimize" onclick="showorhide('topproducts');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="topproducts-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>

                                                        <td>
                                                            <asp:Button ID="btnToday" runat="server" ToolTip="Today" Text="D" Class="btn-unchecked" OnClick="btnToday_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" ToolTip="Weekly" Text="W" Class="btn-unchecked" OnClick="btnWeek_Click" />
                                                        </td>
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllTimeTop25Products');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="topproducts-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <%--<asp:Label ID="ldlTotalTop25Products" Font-Size="15pt" runat="server" Text="0"></asp:Label>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="topproducts-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="Top25Productchartdiv">
                                                </div>
                                                <div id="top25products-blocker" class="loader-div">
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
