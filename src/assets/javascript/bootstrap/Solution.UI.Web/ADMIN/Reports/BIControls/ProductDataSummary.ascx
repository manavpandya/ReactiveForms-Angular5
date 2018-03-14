<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductDataSummary.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.ProductDataSummary" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function AllTimeRevenueDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_ProductDataSummary_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_ProductDataSummary_txtToDate').datetimepicker({
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
            if ((sil[1]) == "ProductDataSummary") {
                document.getElementById("ProductDataSummary-blocker").style.display = "block";
                document.getElementById("ProductSummaryInventory-blocker").style.display = "block";
                document.getElementById("ProductSummaryPrice-blocker").style.display = "block";
                document.getElementById("ProductSalesSummary-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllTimeRevenue(sender, args) {
            document.getElementById("ProductDataSummary-blocker").style.display = "none";
            document.getElementById("ProductSummaryInventory-blocker").style.display = "none";
            document.getElementById("ProductSummaryPrice-blocker").style.display = "none";
            document.getElementById("ProductSalesSummary-blocker").style.display = "none";
        }
        prmAllTimeRevenue.add_beginRequest(BeginRequestHandlerAllTimeRevenue);
        prmAllTimeRevenue.add_endRequest(EndRequestHandlerAllTimeRevenue);

        function checkpostback() {
            var prmAllTimeRevenue1 = Sys.WebForms.PageRequestManager.getInstance();

            prmAllTimeRevenue1.add_beginRequest(drawChartAnimate);
            prmAllTimeRevenue1.add_endRequest(drawChartAnimate);
        }
    </script>
</div>
<div>
    <asp:HiddenField ID="hdnProductSummary" runat="server" Value="0" />

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="border-td">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                    class="content-table">
                    <tr>
                        <td class="border-td-sub">
                            <asp:UpdatePanel ID="upnlProductDataSummary" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(AllTimeRevenueDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="All Time Revenue" alt="All Time Revenue" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Product Data Summary</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-ProductDataSummary" title="Minimize" alt="Minimize" onclick="showorhide('ProductDataSummary');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="ProductDataSummary-row-filter" class="altrow">
                                            <td colspan="2">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore1" OnSelectedIndexChanged="ddlStore1_SelectedIndexChanged" runat="server" AutoPostBack="True"
                                                                CssClass="order-list" Style="width: 200px; border: solid 1px #CCC !important;">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>Product SKU / UPC :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtProductName" OnTextChanged="txtProductName_TextChanged" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td>Option :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlVariant" runat="server" OnSelectedIndexChanged="ddlVariant_SelectedIndexChanged" CssClass="select-box-pro" Style="width: 150px;" AutoPostBack="true">
                                                                <asp:ListItem Value="0">Select Variant</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                </table>
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnToday" runat="server" Visible="false" CssClass="btn-unchecked" ToolTip="Today" OnClick="btnToday_Click" Text="D" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" Visible="false" CssClass="btn-unchecked" ToolTip="Weekly" OnClick="btnWeek_Click" Text="W" />
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('alltimerevenue');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="ProductDataSummary-row-total">
                                        </tr>
                                        <tr id="ProductDataSummary-row-chart">
                                            <td style="position: relative; width: 50%; vertical-align: top;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <th>
                                                            <div style="margin-left: 1%; float: left">
                                                                <asp:Label ID="lblTotalOrderHeader" Font-Size="10pt" ForeColor="#ffffff" Font-Bold="false" runat="server" Text="Total Order of Product"></asp:Label>
                                                            </div>
                                                            <div style="float: right; margin-right: 1%">
                                                                <asp:Label ID="lblTotalOrder" Font-Size="11pt" ForeColor="#ffffff" Font-Bold="false"  runat="server" Text=""></asp:Label>
                                                            </div>

                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ProductSummaryOrderchartdiv" style="width: 100%; float: left; border: 1px solid #c7c7c7; min-height: 200px;">
                                                            </div>
                                                            <div id="ProductDataSummary-blocker" class="loader-div">
                                                                <div>Loading...</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="position: relative; width: 50%; vertical-align: top;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <th>
                                                            <div style="margin-left: 1%; float: left">
                                                                <asp:Label ID="lblSalesChannel" Font-Size="10pt" runat="server" ForeColor="#ffffff" Font-Bold="false" Text="Sales Channel"></asp:Label>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ProductSummaryBySalesChannel" style="width: 100%; float: left; border: 1px solid #c7c7c7; min-height: 200px;">
                                                            </div>
                                                            <div id="ProductSalesSummary-blocker" class="loader-div">
                                                                <div>Loading...</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="ProductDataSummary-row-chart1" style="display:none">
                                            <td style="position: relative; width: 50%; vertical-align: top;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <th>
                                                            <div style="margin-left: 1%; float: left">
                                                                <asp:Label ID="lblProductInventory" Font-Size="10pt" runat="server" ForeColor="#ffffff" Font-Bold="false" Text="Product Inventory Summary"></asp:Label>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ProductSummaryInventory" style="width: 100%; float: left; border: 1px solid #c7c7c7; min-height: 200px;">
                                                            </div>
                                                            <div id="ProductSummaryInventory-blocker" class="loader-div">
                                                                <div>Loading...</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="position: relative; width: 50%; vertical-align: top;">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <th>
                                                            <div style="margin-left: 1%; float: left">
                                                                <asp:Label ID="lblProductPriceSummary" Font-Size="10pt" ForeColor="#ffffff" Font-Bold="false" runat="server" Text="Product Price Summary"></asp:Label>
                                                            </div>
                                                            <div style="float: right; margin-right: 1%">
                                                                <asp:Label ID="Label2" Font-Size="11pt" ForeColor="#ffffff" Font-Bold="false"  runat="server" Text=""></asp:Label>
                                                            </div>

                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div id="ProductSummaryPrice" style="width: 100%; float: left; border: 1px solid #c7c7c7;min-height: 200px;">
                                                            </div>
                                                            <div id="ProductSummaryPrice-blocker" class="loader-div">
                                                                <div>Loading...</div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
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
