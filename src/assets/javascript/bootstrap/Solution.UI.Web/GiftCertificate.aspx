<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GiftCertificate.aspx.cs" Inherits="Solution.UI.Web.GiftCertificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right-sidebar">
        <div class="breadcrumb">
            <a title='Home' href='/index.aspx'>Home </a>> <span>
                <asp:Literal ID="ltbreadcrmbs" runat="server" Text="Gift Certificate"></asp:Literal></span>
        </div>
        <div class="title-box1">
            <h1>
                <asp:Literal ID="ltTitle" runat="server" Text="Gift Certificate"></asp:Literal></h1>
        </div>
        <div class="cat-row1" id="topMiddle" runat="server">
            <center>
                <span style="color: #7D888F; text-align: center;">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label></span></center>
            <asp:Repeater ID="rptGiftCerti" runat="server" OnItemDataBound="rptGiftCerti_ItemDataBound">
                <ItemTemplate>
                    <div class="cat-box" style="margin-bottom: 10px;" id="Probox" runat="server">
                        <div class="cat-display" id="proDisplay" runat="server" style="position: relative;">
                            <a href="/gi-<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%# Eval("ProductID")%>.aspx"
                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                <asp:Image ImageUrl='<%# GetIconImageProduct(Convert.ToString(Eval("ImageName"))) %>'
                                    ID="imgName" Width="180" Height="149" runat="server" /></a>
                        </div>
                        <p style="height: 20px;">
                            <a href="/gi-<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%# Eval("ProductID")%>.aspx"
                                title="<%# SetAttribute(Convert.ToString(DataBinder.Eval(Container.DataItem,"Tooltip")))%>">
                                <%# SetName(Convert.ToString(Eval("Name")))%></a>
                        </p>
                        <div class="sale-price" style="color: #000; font-size: 12px;">
                            <asp:Literal ID="ltrYourPrice" runat="server"></asp:Literal>
                            <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# Math.Round(Convert.ToDecimal(Eval("price")), 2)%>'></asp:Label>
                            <asp:Label ID="lblSalePrice" Visible="false" runat="server" Text='<%# Math.Round(Convert.ToDecimal(Eval("SalePrice")), 2)%>'></asp:Label>
                        </div>
                        <div class="add-to-cart">
                            <a href="javascript:void(0);" id="aFeaturedLink" runat="server" style="display: none;"
                                title="View More"><a href="/gi-<%# Convert.ToString(Eval("SEName")).ToLower()%>-<%# Eval("ProductID")%>.aspx" />
                                <img src="/images/View-Detail.jpg" alt="View Detail" title="View Detail" /></a></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
