<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ThankYou.aspx.cs" Inherits="Solution.UI.Web.ThankYou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <div>
                <asp:Literal ID="lblfriend" runat="server"></asp:Literal><br />
                <asp:Literal ID="ltReturnMerchandise" runat="server"></asp:Literal>
                <asp:Literal ID="lblCreateAccount" runat="server"></asp:Literal>
                <br />
                Enjoy your time with <span id="spnStoreName" runat="server"></span>
                <br />
                <br />
                <br />
            </div>
            <div>
                <table style="width: 100%">
                    <tbody>
                        <tr>
                            <td align="left">
                                <asp:ImageButton ID="btnkeepshopping" runat="server" alt="kEEP SHOPPING" title="kEEP SHOPPING"
                                    ImageUrl="/images/keep-shopping.png" OnClick="btnkeepshopping_Click" />
                            </td>
                            <td align="left">
                                <asp:ImageButton ID="btnCheckoutNow" runat="server" alt="CHECKOUT NOW" title="CHECKOUT NOW"
                                    ImageUrl="/images/checkout-now.png" OnClick="btnCheckoutNow_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
