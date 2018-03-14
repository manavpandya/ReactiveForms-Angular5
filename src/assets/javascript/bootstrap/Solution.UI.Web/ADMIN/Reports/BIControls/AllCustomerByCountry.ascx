<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllCustomerByCountry.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.BIControls.AllCustomerByCountry" %>
<div>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function allcustomerbycountryDatetime() {
            $(function () {
                $('#ContentPlaceHolder1_AllCustomerByCountry_txtFromDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
                $('#ContentPlaceHolder1_AllCustomerByCountry_txtToDate').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "both",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                });
            });
        }
        var prmAllCustomerByCountry = Sys.WebForms.PageRequestManager.getInstance();
        function BeginRequestHandlerAllCustomerByCountry(sender, args) {
            var spl1 = args.get_postBackElement().id;
            var sil = spl1.split("_");
            if ((sil[1]) == "AllCustomerByCountry") {
                document.getElementById("allcustomerbycountry-blocker").style.display = "block";
            }
        }
        function EndRequestHandlerAllCustomerByCountry(sender, args) {
            document.getElementById("allcustomerbycountry-blocker").style.display = "none";
        }
        prmAllCustomerByCountry.add_beginRequest(BeginRequestHandlerAllCustomerByCountry);
        prmAllCustomerByCountry.add_endRequest(EndRequestHandlerAllCustomerByCountry);
    </script>
    <style>
        #CustomerCountrychartdiv span{
            display:none !important;
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
                            <asp:UpdatePanel ID="upnlNewCustomer" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(allcustomerbycountryDatetime);
                                    </script>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Customer by Country" alt="Customer by Country" src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>Customers by Country/State</h2>
                                                </div>
                                                <div class="main-title-right">
                                                    <a title="Minimize" href="javascript:void(0);">
                                                        <img class="minimize" id="img-customerbycountry" title="Minimize" alt="Minimize" onclick="showorhide('customerbycountry');" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr id="customerbycountry-row-filter" class="altrow">
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
                                                            <asp:Button ID="btnToday" runat="server" ToolTip="Today" CssClass="btn-unchecked" Text="D" OnClick="btnToday_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnWeek" runat="server" ToolTip="Weekly" CssClass="btn-unchecked" Text="W" OnClick="btnWeek_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnmonth" runat="server" ToolTip="Current Month" CssClass="btn-unchecked" Text="M" OnClick="btnmonth_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnQuarter" runat="server" ToolTip="Quarter" CssClass="btn-unchecked" Text="Q" OnClick="btnQuarter_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnHalfyear" runat="server" ToolTip="Half Year" CssClass="btn-unchecked" Text="HY" OnClick="btnHalfyear_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnYear" runat="server" ToolTip="Year" Text="Y" CssClass="btn-checked" OnClick="btnYear_Click" />
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
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return SearchValidation('AllCustomerByCountry');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="customerbycountry-row-total">
                                            <td>
                                                <div style="margin-left: 10px">
                                                    <asp:Label ID="ldlTotalCustomer" Font-Size="15pt" runat="server" Text="0"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="customerbycountry-row-chart">
                                            <td align="center" colspan="2" style="position:relative;">
                                                <div id="CustomerCountrychartdiv">
                                                </div>
                                                <div id="allcustomerbycountry-blocker" class="loader-div">
                                                        <div style="margin-top: 12% !important;">Loading...</div>
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
