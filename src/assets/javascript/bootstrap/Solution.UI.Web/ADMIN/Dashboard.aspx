<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Dashboard" ViewStateMode="Enabled" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-1.2.6.min.js"></script>
    <script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <%-- <script src="/App_Themes/<%=Page.Theme %>/js/tabs.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function ShowDiv(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    document.getElementById(divid).style.display = '';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }
                }
            }
        }
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function tabactiveorders() {
            document.getElementById('acustomermap').className = 'Intabactive';
            document.getElementById('aordermap').className = 'tabactive';
            $('#CustomerChart').css('display', 'none');
            $('#OrderChart').css('display', '');
        }
        function tabactivecustomers() {
            document.getElementById('acustomermap').className = 'tabactive';
            document.getElementById('aordermap').className = 'Intabactive';
            $('#CustomerChart').css('display', '');
            $('#OrderChart').css('display', 'none');
        }
    </script>
    <script type="text/javascript">
        function TabdisplayDashboard(id) {

            for (var i = 1; i < 3; i++) {
                var divid = "divtab" + i.toString()
                var liid = "lic" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('lic' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <div class="content-row1" style="width: 100%;">
        <img title="Half Price Drapes Dashboard" alt="Half Price Drapes Dashboard" src="/App_Themes/<%=Page.Theme %>/icon/welcome.png">
        <h2 style="color: #6A6A6A;">
            Half Price Drapes Dashboard</h2>
        <span style="float: right; margin-top: 7px;" id="spanStore" runat="server">
            <asp:DropDownList ID="ddlStore" onchange="chkHeight();" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list"
                Style="width: 200px;">
            </asp:DropDownList>
        </span><span style="float: right;" id="spanDashboardSetting" runat="server"><a href="/admin/Settings/DashboardSetting.aspx">
            <img border='0' alt='' src="/App_Themes/<%=Page.Theme %>/images/Dashboard-Settings.png"
                style="margin-top: 3px; margin-right: 3px;" />
        </a></span>
    </div>
    <div class="content-row2">
        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard">
            <tbody>
                <tr>
                    <td width="22%" valign="top" id="tdLeft" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="divleftControls" runat="server">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td width="1%" id="tdLeftCenter" runat="server">
                        &nbsp;
                    </td>
                    <td width="53%" valign="top" id="tdCenter" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div id="divCenterControls" runat="server">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div style="float: left; width: 100%;">
                            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-center">
                                <tbody>
                                    <tr style="cursor: pointer;" id="countryorder">
                                        <td style="padding: 0px;">
                                            <div class="container">
                                                <table style="width: 100%;">
                                                    <tbody>
                                                        <tr class="table-th-bg">
                                                            <th style="padding: 0px;">
                                                                <div class="main-title-left">
                                                                    <img src="/App_Themes/Gray/icon/wordmap.png" alt="Country Map" title="Country Map"
                                                                        class="img-left">
                                                                    <h2>
                                                                        Country/State Wise Orders</h2>
                                                                </div>
                                                                <div class="main-title-right">
                                                                    <a onclick="ShowDiv('CountryTenOrder','toptenorder-target','content11');" href="javascript:void(0);"
                                                                        title="Minimize">
                                                                        <img src="/App_Themes/Gray/images/minimize.png" alt="Minimize" title="Minimize" id="CountryTenOrder"
                                                                            class="minimize"></a>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="toptenorder-target">
                                        <td style="padding: 0px;">
                                            <div class="content11">
                                                <table style="width: 100%; text-align: center;">
                                                    <tbody>
                                                        <tr id="CountryOrder">
                                                            <td colspan="3">
                                                                <div>
                                                                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>--%>
                                                                    <table cellspacing="1" cellpadding="0" style="border: 1px solid rgb(223, 223, 223);
                                                                        width: 100%; margin-bottom: 0pt ! important;" class="dashboard-left">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <a href="javascript:void(0);" id="aordermap" onclick="tabactiveorders();" class="tabactive">
                                                                                    Orders</a><a id="acustomermap" onclick="tabactivecustomers();" href="javascript:void(0);"
                                                                                        class="Intabactive">Customers</a>
                                                                            </td>
                                                                            <td align="right">
                                                                                Select Country: &nbsp;<asp:DropDownList ID="ddlCountryList" runat="server" AutoPostBack="true"
                                                                                    CssClass="order-list" OnSelectedIndexChanged="ddlCountryList_SelectedIndexChanged">
                                                                                    <asp:ListItem Value="United States">United States</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div id="OrderChart" align="center">
                                                                                </div>
                                                                                <div id="CustomerChart" align="center" style="width:100%">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--     </ContentTemplate>
                                                                            </asp:UpdatePanel>--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                    <td width="1%" id="tdCenterRight" runat="server">
                        &nbsp;
                    </td>
                    <td width="22%" valign="top" id="tdRight" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div id="divRightControls" runat="server">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
                top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
                height: 100%; width: 100%; z-index: 1000; display: none;">
                <table width="100%" style="padding-top: 25%;" align="center">
                    <tr>
                        <td align="center" style="color: #fff;" valign="middle">
                            <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                            <b>Loading ... ... Please wait!</b>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
