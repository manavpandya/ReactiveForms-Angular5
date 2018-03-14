<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KPIMeter.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.KPIMeter" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/kpi-meter.png" alt="KPI Meter" title="KPI Meter"
                            class="img-left">
                        <h2>
                            KPI Meter</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgKPI','trKPilist','trKPiGraph');">
                            <img class="minimize" id="imgKPI" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trKPilist">
                <td>
                    <asp:DropDownList ID="ddlView" onchange="chkHeight();" CssClass="order-list" Style="width: 90px; font-family: Arial,Helvetica,sans-serif;
                        font-size: 11px; text-decoration: none;" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlView_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlOption" onchange="chkHeight();" CssClass="order-list" Style="width: 90px; font-family: Arial,Helvetica,sans-serif;
                        font-size: 11px; text-decoration: none;" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="trKPiGraph">
                <td align="center" style="padding: 5px 0px 0px 0px">
                    <asp:Literal ID="ltrRevenue" runat="server"></asp:Literal>
                    <asp:Literal ID="ltrQuantity" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
</div>
