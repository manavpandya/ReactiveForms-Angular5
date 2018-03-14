<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="Solution.UI.Web.Client.Control.LeftMenu" %>
<div class="left-sidebar" id="hideleftmenu" runat="server">
    <div class="left-menu">
        <%=MenuData %>
    </div>
  
    <div class="best-sellers-main">
        <div class="best-sellers-title">
            BEST SELLERS</div>
        <div class="best-sellers-bg">
            <asp:Repeater ID="rptBestSeller" runat="server">
                <ItemTemplate>
                    <div id="ProboxBestSeller" runat="server" class="bs-box">
                        <div id="proDisplay" runat="server">
                            <div onclick="bs-dispaly">
                                <div>
                                    <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx">
                                        <img src='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>' width="168"
                                             alt="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>"
                                            title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>" /></a></div>
                            </div>
                            <h2>
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                    title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                    <%# SetName(Convert.ToString(Eval("Name")))%></a></h2>
                            <p>
                                <a href="/<%# Convert.ToString(Eval("MainCategory")).ToLower()%>/<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%#Eval("ProductID")%>.aspx"
                                    title="more details">more details</a></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
<div class="left-menu-box" style="display: none;">
    <div class="left-title">
        <h1>
            Shop By Price</h1>
    </div>
    <asp:Literal ID="ltShopbyPrice" runat="Server"></asp:Literal>
</div>
<div class="left-menu-box" style="display: none;">
    <div class="left-title">
        <h1>
            Customer Service</h1>
    </div>
    <ul>
        <li><a href="/shippinginformation" title="Shipping Info">Shipping Info</a></li>
        <li><a href="/returnsrepairs" title="Returns &amp; Repairs">Returns &amp; Repairs</a></li>
        <li><a href="/orderstatus.aspx" title="Track My Order">Track My Order</a></li>
        <li><a href="/faq" title="FAQ">FAQ</a></li>
        <li><a href="/customerfeedback" title="Customer Feedbacks">Customer Feedbacks</a></li>
    </ul>
</div>
<div class="left-menu-box" style="display: none;">
    <div class="left-title">
        <h1>
            Engraving Info</h1>
    </div>
    <ul>
        <li><a href="/engravingfonts" title="Engraving Fonts">Engraving Fonts</a></li>
        <li><a href="/engravingpolicy" title="Engraving Policy">Engraving Policy</a></li>
        <li><a href="/engravingfaqs" title="Engraving FAQs">Engraving FAQs</a></li>
        <li><a href="/engravedsamples" title="Engraved Samples">Engraved Samples</a></li>
    </ul>
</div>
