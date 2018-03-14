<%@ Page Title="" Language="C#" MasterPageFile="~/RMA/Amazon/RMAAmazon.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="Solution.UI.Web.RMA.Amazon.OrderDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> >
        <asp:Literal ID="PrevPage" runat="server"></asp:Literal>
        > <span>Order Details</span></div>
  
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a id="divback" runat="server" style="color: #B92127; text-decoration: underline;"
                            title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a> </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div style="min-height: 200px;" class="static-big-main">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                <asp:Label ID="lblTable" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
