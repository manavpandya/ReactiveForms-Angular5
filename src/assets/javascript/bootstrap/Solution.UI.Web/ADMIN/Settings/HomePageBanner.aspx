<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="HomePageBanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.HomePageBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table">
                <tr>
                    <td class="border-td-sub">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="add-product">
                            <tbody>
                                <tr>
                                    <th colspan="2">
                                        <div class="main-title-left">
                                            <img class="img-left" title="Home Page Banner" alt="Home Page Banner" src="/App_Themes/<%=Page.Theme %>/Images/home-page-banner-icon.png">
                                            <h2>
                                                <asp:Label runat="server" Text="Home Page Banner" ID="lblTitle"></asp:Label></h2>
                                        </div>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="vertical-align: top; padding-left: 15px; float: left;"><a href="EditHomePageBanner.aspx">
                                          Add Home Page Banner</a></span>
                                    </td>
                                </tr>
                                <tr>
                                    <tr>
                                        <td class="border">
                                            <center>
                                                <asp:Label ID="lblMsg" runat="Server"></asp:Label>
                                                <asp:DataList ID="DLHomePageBanner" runat="server" Width="98%" OnItemDataBound="DLHomePageBanner_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="border: 1px solid #e2e2e2; width: 100%">
                                                            <td>
                                                                &nbsp;&nbsp;<b>Title :</b> <a href="EditHomePageBanner.aspx?ID=<%#Eval("BannerID") %>">
                                                                    <%#Eval("Title")%>
                                                                </a><span style="text-align: right; float: right;"><a href="EditHomePageBanner.aspx?ID=<%#Eval("BannerID") %>">
                                                                    Edit </a>&nbsp;&nbsp;</span>
                                                            </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;&nbsp;<b>Banner URL :</b>
                                                                    <%#Eval("BannerURL")%>
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none;">
                                                                <td>
                                                                    &nbsp;&nbsp;<b>Title :</b>
                                                                    <%#Eval("Title")%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <center>
                                                                        &nbsp;&nbsp;<asp:Literal ID="LtBannerImage" runat="server"></asp:Literal>
                                                                        <asp:Label ID="lblBannerID" runat="server" Text='<%#Eval("BannerID") %>' Visible="false"></asp:Label>
                                                                    </center>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </center>
                                        </td>
                                    </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
