<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="businessintelligence.aspx.cs" Inherits="Solution.UI.Web.ADMIN.businessintelligence" %>

<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeRevenue.ascx" TagPrefix="uc" TagName="AllTimeRevenue" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeOrderNumber.ascx" TagPrefix="uc" TagName="AllTimeOrderNumber" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeNewCustomer.ascx" TagPrefix="uc" TagName="AllTimeNewCustomer" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeAverageRevenue.ascx" TagPrefix="uc" TagName="AllTimeAverageRevenue" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeAverageOrders.ascx" TagPrefix="uc" TagName="AllTimeAverageOrders" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeAverageCustomer.ascx" TagPrefix="uc" TagName="AllTimeAverageCustomer" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/RevenuByCountryState.ascx" TagPrefix="uc" TagName="RevenuByCountryState" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/OrderRevenue_ByCountry.ascx" TagPrefix="uc" TagName="OrderRevenue_ByCountry" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllCustomerByCountry.ascx" TagPrefix="uc" TagName="AllCustomerByCountry" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeTop25Category.ascx" TagPrefix="uc" TagName="AllTimeTop25Category" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeTop25Brands.ascx" TagPrefix="uc" TagName="AllTimeTop25Brands" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeTop25Products.ascx" TagPrefix="uc" TagName="AllTimeTop25Products" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeTop3SellingMonth_ByRevenu.ascx" TagPrefix="uc" TagName="AllTimeTop3SellingMonth_ByRevenu" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top25SellingDays.ascx" TagPrefix="uc" TagName="Top25SellingDays" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top5SellingTimeByRevenue.ascx" TagPrefix="uc" TagName="Top5SellingTimeByRevenue" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeCustomerByOrder.ascx" TagPrefix="uc" TagName="AllTimeCustomerByOrder" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeStoreSalesReport.ascx" TagPrefix="uc" TagName="AllTimeStoreSalesReport" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeTop3SellingMonth_ByOrder.ascx" TagPrefix="uc" TagName="AllTimeTop3SellingMonth_ByOrder" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/OrderStatusReport.ascx" TagPrefix="uc" TagName="OrderStatusReport" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top10StateByOrder.ascx" TagPrefix="uc" TagName="Top10StateByOrder" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top10StateByRevenue.ascx" TagPrefix="uc" TagName="Top10StateByRevenue" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top25SellingDaysByOrder.ascx" TagPrefix="uc" TagName="Top25SellingDaysByOrder" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top5SellingTimeByOrder.ascx" TagPrefix="uc" TagName="Top5SellingTimeByOrder" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/AllTimeCustomerByRevenue.ascx" TagPrefix="uc" TagName="AllTimeCustomerByRevenue" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/Top10ProductByReview.ascx" TagPrefix="uc" TagName="Top10ProductByReview" %>
<%@ Register Src="~/ADMIN/Reports/BIControls/ProductDataSummary.ascx" TagPrefix="uc" TagName="ProductDataSummary" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .BIdashboard {
            float: left;
            width: 33%;
            padding-right: 5px;
        }

        .BIdashboard-pt {
            width: 100%;
            float: left;
            padding-top: 5px;
        }

        .ProductSummary {
            float: left;
            width: 100%;
            padding-top: 5px;
        }

        .btn-checked {
            background: #B62A18;
            color: #FFFFFF;
            border: 1px solid #8D2113 !important;
        }

        .btn-unchecked {
            color: #000000;
            border: 1px solid #d7d7d7 !important;
        }

            .btn-unchecked:hover {
                background: #B62A18;
                color: #FFFFFF;
                border: 1px solid #8D2113 !important;
            }

        .loader-div {
            background-color: #1d1d1d;
            height: 100%;
            left: 0;
            opacity: 0.5;
            position: absolute;
            top: 0;
            width: 100%;
            z-index: 1000;
            display: none;
        }

            .loader-div div {
                color: #fff;
                display: block;
                font-weight: bold;
                left: 0;
                margin: 0 auto;
                padding: 16% 0 0;
                right: 0;
                width: 50px;
                font-size: 16px;
            }
    </style>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function SearchValidation(controlname) {
            if (document.getElementById('ContentPlaceHolder1_' + controlname + '_txtFromDate').value != '') {
                if (document.getElementById('ContentPlaceHolder1_' + controlname + '_txtToDate').value != '') {
                    var startDate = new Date(document.getElementById('ContentPlaceHolder1_' + controlname + '_txtFromDate').value);
                    var endDate = new Date(document.getElementById('ContentPlaceHolder1_' + controlname + '_txtToDate').value);
                    if (startDate > endDate) {
                        jAlert('Please Select Valid Date.', 'Required Information');
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    jAlert('Please Enter End Date.', 'Required Information', 'ContentPlaceHolder1_' + controlname + '_txtToDate');
                    return false;
                }
            }
            else {
                jAlert('Please Enter Start Date.', 'Required Information', 'ContentPlaceHolder1_' + controlname + '_txtFromDate');
                return false;
            }
        }
        function showorhide(controlname) {
            if (document.getElementById('' + controlname + '-row-filter').style.display == "none") {
                $('#' + controlname + '-row-filter').show(200);
                $('#' + controlname + '-row-total').show(200);
                $('#' + controlname + '-row-chart').show(200);
                if (document.getElementById(controlname + '-row-chart1') != null) {
                    $('#' + controlname + '-row-chart1').show(200);
                }
                $('#img-' + controlname + '').attr('src', "/App_Themes/<%=Page.Theme %>/images/minimize.png");
            }
            else {
                $('#' + controlname + '-row-filter').hide(200);
                $('#' + controlname + '-row-total').hide(200);
                $('#' + controlname + '-row-chart').hide(200);
                if (document.getElementById(controlname + '-row-chart1') != null) {
                    $('#' + controlname + '-row-chart1').hide(200);
                }
                $('#img-' + controlname + '').attr('src', "/App_Themes/<%=Page.Theme %>/images/expand.gif");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1" style="width: 100%;">
        <img title="Acedepot Dashboard" alt="Acedepot Dashboard" src="/App_Themes/<%=Page.Theme %>/icon/welcome.png">
        <h2>Business Intelligence Dashboard </h2>
        <span style="float: right; margin-top: 7px;" id="spanStore" runat="server">
            <asp:DropDownList ID="ddlStore" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" runat="server" AutoPostBack="True"
                CssClass="order-list" Style="width: 200px; border: solid 1px #c7c7c7 !important;">
            </asp:DropDownList>
        </span>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="content-row2">
                <div id="sortable">
                    <div class="BIdashboard">
                        <div id="alltimerevenueDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeRevenue runat="server" ID="alltimerevenue" />
                        </div>
                        <div id="AllTimeAverageRevenueDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeAverageRevenue runat="server" ID="AllTimeAverageRevenue" />
                        </div>
                        <div id="RevenuByCountryStateDiv" runat="server" class="BIdashboard-pt">
                            <uc:RevenuByCountryState runat="server" ID="RevenuByCountryState" />
                        </div>
                        <div id="AllTimeTop25CategoryDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeTop25Category runat="server" ID="AllTimeTop25Category" />
                        </div>
                        <div id="AllTimeTop3SellingMonth_ByRevenuDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeTop3SellingMonth_ByRevenu runat="server" ID="AllTimeTop3SellingMonth_ByRevenu" />
                        </div>
                        <div id="AllTimeTop3SellingMonth_ByOrderDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeTop3SellingMonth_ByOrder runat="server" ID="AllTimeTop3SellingMonth_ByOrder" />
                        </div>
                        <div id="AllTimeCustomerByRevenueDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeCustomerByRevenue runat="server" ID="AllTimeCustomerByRevenue" />
                        </div>
                        <div id="Top10StateByRevenueDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top10StateByRevenue runat="server" ID="Top10StateByRevenue" />
                        </div>
                        <div id="Top10ProductByReviewDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top10ProductByReview runat="server" ID="Top10ProductByReview" />
                        </div>
                    </div>
                    <div class="BIdashboard">
                        <div id="AllTimeOrderNumberDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeOrderNumber runat="server" ID="AllTimeOrderNumber" />
                        </div>
                        <div id="AllTimeAverageOrdersDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeAverageOrders runat="server" ID="AllTimeAverageOrders" />
                        </div>
                        <div id="OrderRevenue_ByCountryDiv" runat="server" class="BIdashboard-pt">
                            <uc:OrderRevenue_ByCountry runat="server" ID="OrderRevenue_ByCountry" />
                        </div>
                        <div id="AllTimeTop25BrandsDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeTop25Brands runat="server" ID="AllTimeTop25Brands" />
                        </div>
                        <div id="Top25SellingDaysDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top25SellingDays runat="server" ID="Top25SellingDays" />
                        </div>
                        <div id="Top25SellingDaysByOrderDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top25SellingDaysByOrder runat="server" ID="Top25SellingDaysByOrder" />
                        </div>

                        <div id="AllTimeCustomerByOoder" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeCustomerByOrder runat="server" ID="AllTimeCustomerByOrder" />
                        </div>
                        <div id="Top10StateByOrderDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top10StateByOrder runat="server" ID="Top10StateByOrder" />
                        </div>
                    </div>
                    <div class="BIdashboard">
                        <div id="AllTimeNewCustomerDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeNewCustomer runat="server" ID="AllTimeNewCustomer" />
                        </div>
                        <div id="AllTimeAverageCustomerDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeAverageCustomer runat="server" ID="AllTimeAverageCustomer" />
                        </div>
                        <div id="AllCustomerByCountryDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllCustomerByCountry runat="server" ID="AllCustomerByCountry" />
                        </div>
                        <div id="AllTimeTop25ProductsDiv" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeTop25Products runat="server" ID="AllTimeTop25Products" />
                        </div>
                        <div id="Top5SellingTimeByRevenueDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top5SellingTimeByRevenue runat="server" ID="Top5SellingTimeByRevenue" />
                        </div>
                        <div id="Top5SellingTimeByOrderDiv" runat="server" class="BIdashboard-pt">
                            <uc:Top5SellingTimeByOrder runat="server" ID="Top5SellingTimeByOrder" />
                        </div>
                        <div id="StoreSalesReport" runat="server" class="BIdashboard-pt">
                            <uc:AllTimeStoreSalesReport runat="server" ID="AllTimeStoreSalesReport" />
                        </div>
                        <div id="OrderStatusReportdiv" runat="server" class="BIdashboard-pt">
                            <uc:OrderStatusReport runat="server" ID="OrderStatusReport" />
                        </div>

                    </div>

                    <div style="float: left; margin-top: 5px">
                        <asp:Label ID="Label1" runat="server" ForeColor="White" Text="0"></asp:Label>
                    </div>
                </div>
                <div id="ProductSummaryDiv" runat="server" class="ProductSummary">
                    <uc:ProductDataSummary runat="server" ID="ProductDataSummary" />
                </div>
                <div id="allcustomerbycountry-blocker" class="loader-div">
                    <div>Loading...</div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="ddlStore" EventName="SelectedIndexChanged" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
