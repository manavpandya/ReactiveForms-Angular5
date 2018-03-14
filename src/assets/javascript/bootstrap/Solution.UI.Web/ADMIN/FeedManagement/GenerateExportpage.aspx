<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="GenerateExportpage.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.GenerateExportpage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
   <%-- <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>--%>
    <%--<script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery.ui.accordion.js"></script>--%>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display: none">
        <asp:Button ID="btnFinish" runat="server" OnClick="btnFinish_Click" />
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" />
        <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
        <input type="hidden" id="hdnfile" runat="server" value="" />
        <input type="hidden" id="hdnnew" runat="server" value="" />
        <input type="hidden" id="hdntab" runat="server" value="" />
        <input type="hidden" id="hdntotal" runat="server" value="" />
        <input type="hidden" id="hdnfeedid" runat="server" value="" />
        <input type="hidden" id="deletehdn" runat="server" value="" />
        <input type="hidden" id="hdntitle" runat="server" value="" />
    </div>
</asp:Content>
