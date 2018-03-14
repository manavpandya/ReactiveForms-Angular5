<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Top25SellingDaysByOrder.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.Top25SellingDaysByOrder" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
   <script type="text/javascript">
       var prmTop25SellingDaysByOrder = Sys.WebForms.PageRequestManager.getInstance();
       function BeginRequestHandlerTop25SellingDaysByOrder(sender, args) {
           var spl1 = args.get_postBackElement().id;
           var sil = spl1.split("_");
           if ((sil[1]) == "Top25SellingDaysByOrder") {
               document.getElementById("top25SellingByOrder-blocker").style.display = "block";
           }
       }
       function EndRequestHandlerTop25SellingDaysByOrder(sender, args) {
           document.getElementById("top25SellingByOrder-blocker").style.display = "none";
       }
       prmTop25SellingDaysByOrder.add_beginRequest(BeginRequestHandlerTop25SellingDaysByOrder);
       prmTop25SellingDaysByOrder.add_endRequest(EndRequestHandlerTop25SellingDaysByOrder);
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
                            <asp:UpdatePanel ID="upnltop25sellorder" runat="server">
                                <ContentTemplate>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Top 10 Selling Days By Order" alt="Top 10 Selling Days By Order" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Top 10 Selling Days by Order</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-topsellingdaybyorder" title="Minimize" onclick="showorhide('topsellingdaybyorder');" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="topsellingdaybyorder-row-filter" class="altrow">
                                            <td>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
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
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="topsellingdaybyorder-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <%--<asp:Label ID="ldlTop25SellingDays" Font-Size="15pt" runat="server" Text="0"></asp:Label>--%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="topsellingdaybyorder-row-chart">
                                            <td align="center" colspan="2" style="position: relative;">
                                                <div id="Top25SellingDaysByOrderDiv" style="min-height: 203px;">
                                                </div>
                                                <div id="top25SellingByOrder-blocker" class="loader-div">
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
