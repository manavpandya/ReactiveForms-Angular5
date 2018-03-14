<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StaticPage.aspx.cs" Inherits="Solution.UI.Web.StaticPage" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="litBreadTitle" runat="server"></asp:Literal></span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <p>
                <asp:Literal ID="ltPage" runat="server"></asp:Literal></p>
        </div>
    </div>
         <div id="backgroundPopup" style="z-index: 1000000;">
             </div>

</asp:Content>
