<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="aboutus.aspx.cs" Inherits="Solution.UI.Web.aboutus" %>

<%@ Register Src="~/Controls/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrmbs">
        <a href="/" title="Home">Home </a>
        <img src="images/breadcrmbs-bullet.png" alt="breadcrmbs" title="breadcrmbs">
        <span>
            <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></div>
    <div id="content-right">
        <div class="title">
            <img src="images/title-top.png" width="758" height="4" alt="title" title="title"
                class="img-left">
            <h1>
                <asp:Literal ID="ltTitle" runat="server"></asp:Literal></h1>
            <img src="images/title-bottom.png" width="758" height="4" alt="title" title="title"
                class="img-left">
        </div>
        <div class="static-main">
            <div>
                
            </div>
        </div>
    </div>
    <uc1:LeftMenu ID="leftmenu" runat="server" />
    </span>
</asp:Content>
