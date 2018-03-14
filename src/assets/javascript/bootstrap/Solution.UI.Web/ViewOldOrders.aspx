<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ViewOldOrders.aspx.cs" Inherits="Solution.UI.Web.viewoldorders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function reorder(ono) {
            document.getElementById('ContentPlaceHolder1_hdnortder').value = ono;

            document.getElementById('ContentPlaceHolder1_btnReorder').click();
        }
    </script>
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
                <asp:Label ID="lblTable" runat="server"></asp:Label>
            </div>
        </div>
        <div style="display: none;">
            <asp:ImageButton ID="btnReorder" runat="server" ImageUrl="~/images/reorder.png" OnClick="btnReorder_Click" />
            <input type="hidden" id="hdnortder" runat="server" value="0" />
        </div>
    </div>
</asp:Content>
