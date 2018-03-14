<%@ Page Title="" Language="C#" MasterPageFile="~/RMA/HPDAmazon/HPDAmazon.Master" AutoEventWireup="true"
    CodeBehind="ViewRMARecentOrders.aspx.cs" Inherits="Solution.UI.Web.RMA.HPDAmazon.ViewRMARecentOrders" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal><a id="lnkBack" runat="server"
                    href="/RMA/HPDAmazon/ReturnItem.aspx" style="color: #B92127; text-decoration: underline;
                    float: right;margin-top:5px;" title="BACK">
                    <img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a></span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <div class="static-main-box">
                <asp:Label ID="lblMsg" runat="server" style="color:#B92127;"></asp:Label>
                <div class="paging-top" style="width: 100%; margin-bottom: 5px; font-size: 12px;
                    text-align: right;" id="paging" runat="server">
                    <p class="numbering" style="text-align: right;">
                        <asp:Literal ID="litPagesTop" runat="server"></asp:Literal>
                    </p>
                </div>
                <asp:Label ID="lblTable" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
