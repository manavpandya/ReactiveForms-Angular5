<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dealoftheday.ascx.cs"
    Inherits="Solution.UI.Web.Controls.Dealoftheday" %>
<asp:UpdatePanel ID="uppanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="deal_ending_box" style="margin-top: 4px;">
            <p>
                Deal ending in:
                <asp:Label ID="lblTime" runat="server" Text=""></asp:Label>
                <br />
                <b style="font-size: 12px; font-weight: bold; color: #000000;">Coupons are not valid
                    for Deal of the Day Items. </b>
            </p>
        </div>
        <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick" Enabled="false">
        </asp:Timer>
    </ContentTemplate>
</asp:UpdatePanel>
