<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductHead.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.ProductHead" %>
<style type="text/css">
    .tab_box2
    {
        float: left;
        padding: 4px 0 0 0;
        width: 100%;
    }
    .tab_box2 ul
    {
        list-style: none outside none;
        margin: 0;
        padding: 0 0 0 0%;
    }
    .tab_box2 ul li
    {
        float: left;
        padding: 0 1px 0 0;
    }
</style>
<script type="text/javascript">
    var currmenu = 0;
</script>
<div runat="server" id="tblHeader">
    <ul>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPD" ToolTip="Half Price Drapes" AlternateText="Half Price Drapes" onmouseover="if(currmenu!=1)this.src='../images/MenubtnHPD-red.png';"
                onmouseout="if(currmenu!=1)this.src='../images/MenubtnHPD.png';" OnClick="MenubtnHPD_Click"
                OnClientClick="currmenu=1;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDYahoo" ToolTip="Half Price Drapes Yahoo"
                AlternateText="Half Price Drapes Yahoo" OnClick="MenubtnHPDYahoo_Click" onmouseover="if(currmenu!=2)this.src='../images/MenubtnHPDYahoo-red.png';"
                onmouseout="if(currmenu!=2)this.src='../images/MenubtnHPDYahoo.png';" OnClientClick="currmenu=2;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnCurtainsonbudgetAmazon" ToolTip="Curtainsonbudget Amazon"
                AlternateText="Curtainsonbudget Amazon" OnClick="MenubtnCurtainsonbudgetAmazon_Click"
                onmouseover="if(currmenu!=3)this.src='../images/MenubtnCurtainsonbudgetAmazon-red.png';"
                onmouseout="if(currmenu!=3)this.src='../images/MenubtnCurtainsonbudgetAmazon.png';"
                OnClientClick="currmenu=3;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDOverstock" ToolTip="HPD Overstock"
                AlternateText="HPD Overstock" OnClick="MenubtnHPDOverstock_Click" onmouseover="if(currmenu!=4)this.src='../images/MenubtnHPDOverstock-red.png';"
                onmouseout="if(currmenu!=4)this.src='../images/MenubtnHPDOverstock.png';" OnClientClick="currmenu=4;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHalfPriceFabrics" ToolTip="Half Price Fabrics"
                AlternateText="Half Price Fabrics" OnClick="MenubtnHalfPriceFabrics_Click" onmouseover="if(currmenu!=5)this.src='../images/MenubtnHalfPriceFabrics-red.png';"
                onmouseout="if(currmenu!=5)this.src='../images/MenubtnHalfPriceFabrics.png';"
                OnClientClick="currmenu=5;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHalfPriceShades" ToolTip="Half Price Shades"
                AlternateText="Half Price Shades" OnClick="MenubtnHalfPriceShades_Click" onmouseover="if(currmenu!=6)this.src='../images/MenubtnHalfPriceShades-red.png';"
                onmouseout="if(currmenu!=6)this.src='../images/MenubtnHalfPriceShades.png';"
                OnClientClick="currmenu=6;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDEBay" ToolTip="HPD EBay" AlternateText="HPD EBay"
                OnClick="MenubtnHPDEBay_Click" onmouseover="if(currmenu!=7)this.src='../images/MenubtnHPDEBay-red.png';"
                onmouseout="if(currmenu!=7)this.src='../images/MenubtnHPDEBay.png';" OnClientClick="currmenu=7;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDSears" ToolTip="HPD Sears" AlternateText="HPD Sears"
                OnClick="MenubtnHPDSears_Click" onmouseover="if(currmenu!=8)this.src='../images/MenubtnHPDSears-red.png';"
                onmouseout="if(currmenu!=8)this.src='../images/MenubtnHPDSears.png';" OnClientClick="currmenu=8;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDBuycom" ToolTip=" HPD Buy.com" AlternateText="HPD Buy.com"
                OnClick="MenubtnHPDBuycom_Click" onmouseover="if(currmenu!=9)this.src='../images/MenubtnHPDBuycom-red.png';"
                onmouseout="if(currmenu!=9)this.src='../images/MenubtnHPDBuycom.png';" OnClientClick="currmenu=9;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDNewEgg" ToolTip="HPD NewEgg" AlternateText="HPD NewEgg"
                OnClick="MenubtnHPDNewEgg_Click" onmouseover="if(currmenu!=10)this.src='../images/MenubtnHPDNewEgg-red.png';"
                onmouseout="if(currmenu!=10)this.src='../images/MenubtnHPDNewEgg.png';" OnClientClick="currmenu=10;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDWayfailr" ToolTip="HPD Wayfair" AlternateText="HPD Wayfair"
                OnClick="MenubtnHPDWayfailr_Click" onmouseover="if(currmenu!=11)this.src='../images/MenubtnHPDWayfailr-red.png';"
                onmouseout="if(currmenu!=11)this.src='../images/MenubtnHPDWayfailr.png';" OnClientClick="currmenu=11;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDLNT" ToolTip="HPD LNT" AlternateText="HPD LNT"
                OnClick="MenubtnHPDLNT_Click" onmouseover="if(currmenu!=12)this.src='../images/MenubtnHPDLNT-red.png';"
                onmouseout="if(currmenu!=12)this.src='../images/MenubtnHPDLNT.png';" OnClientClick="currmenu=12;" />
        </li>
        <li>
            <asp:ImageButton runat="server" ID="MenubtnHPDBellacor" ToolTip="HPD Bellacor" AlternateText="HPD Bellacor"
                OnClick="MenubtnHPDBellacor_Click" onmouseover="if(currmenu!=13)this.src='../images/MenubtnHPDBellacor-red.png';"
                onmouseout="if(currmenu!=13)this.src='../images/MenubtnHPDBellacor.png';" OnClientClick="currmenu=13;" />
        </li>
    </ul>
</div>
