<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Unsubscribe.aspx.cs" Inherits="Solution.UI.Web.Unsubscribe" %>

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
            <div class="static-main-box">
                <center>
                    <asp:Label ID="lblMsg" Text="" runat="server"></asp:Label></center>
                <br />
                <br />
                <a href="/" title="Keep Shopping">
                    <img alt="Keep Shopping" src="/images/keep-shopping.png" style="border: none;" /></a>
            </div>
        </div>
    </div>
</asp:Content>
