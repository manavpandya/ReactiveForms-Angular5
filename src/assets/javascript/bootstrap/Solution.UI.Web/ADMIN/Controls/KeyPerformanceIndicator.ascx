<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KeyPerformanceIndicator.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.KeyPerformanceIndicator" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-center">
        <tbody>
            <tr>
                <th colspan="7">
                    <div class="main-title-left">
                        <img class="img-left" title="Key Performance Indicator" alt="Key Performance Indicator" src="/App_Themes/<%=Page.Theme %>/icon/key-performance-iIndicator.png" />
                        <h2>
                            Key Performance Indicator</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgkeyindicator','trKeyindicator','tempdiv');">
                            <img class="minimize" id="imgkeyindicator" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trKeyindicator">
                <td colspan="8" align="left">
                    <table cellspacing="1" cellpadding="0" border="0" width="100%" class="dashboard-center" style="margin:0;">
                        <asp:Literal ID="ltrIndicator" runat="server"></asp:Literal>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
