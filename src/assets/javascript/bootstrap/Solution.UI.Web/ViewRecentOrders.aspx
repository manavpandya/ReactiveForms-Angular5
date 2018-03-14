<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewRecentOrders.aspx.cs" Inherits="Solution.UI.Web.viewrecentorders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> > <span>
                <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a href="MyAccount.aspx" style="color: #B92127; text-decoration: underline;"
                             title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a> </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div style="min-height: 200px;" class="static-big-main">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                <%--  <div class="numbering" style="width: 100%; margin-bottom: 5px; font-size: 12px; text-align: right;"
                    id="paging" runat="server">--%>
                <p style="text-align: right;">
                    <asp:Literal ID="litPagesTop" runat="server"></asp:Literal>
                </p>
                <%--  </div>--%>
                <asp:Label ID="lblTable" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
