<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Sitemap.aspx.cs" Inherits="Solution.UI.Web.Sitemap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a> <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <div class="sitemap-navi">
                    <asp:Literal ID="ltrSiteMap" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
