<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="PartnerExport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.PartnerExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 5px; float: left">
                        <tr>
                            <td align="left">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"></span>
            </div>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table style="width: 100%; border: 0; bgcolor: #FFFFFF;" cellpadding="0" cellspacing="0"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Partner Export" alt="Partner Export" src="/App_Themes/<%=Page.Theme %>/Images/product_export.png" />
                                                <h2>
                                                    Partner Export
                                                </h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td style="width: 5%" align="center">
                                                        Store :
                                                    </td>
                                                    <td style="width: 15%" align="left">
                                                        <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="true"
                                                            CssClass="order-list" Style="margin-left: 0px" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export" OnClientClick="return checkCount();"
                                                            OnClick="btnExport_Click" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow" id="trupload" runat="server" visible="false">
                                                    <td style="width: 5%" align="center">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" valign="top" colspan="2">
                                                        <asp:Button ID="btnUpload" runat="server" ToolTip="Upload" OnClick="btnUpload_Click" />
                                                        &nbsp;
                                                        <asp:Button ID="btnDownload" runat="server" ToolTip="Download" OnClick="btnDownload_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
