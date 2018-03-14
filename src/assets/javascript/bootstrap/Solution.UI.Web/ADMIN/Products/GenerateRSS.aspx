<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="GenerateRSS.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.GenerateRSS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Generate RSS" alt="Generate RSS" src="/App_Themes/<%=Page.Theme %>/Images/seo-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Generate RSS"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="height: 10px;">
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td style="width: 10%">
                                                            <span class="star">&nbsp;&nbsp;</span>Store:
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlStore" CssClass="add-product-list" Width="190px"
                                                                Height="20px" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 10%">
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:ImageButton ID="btnGenerate" runat="server" AlternateText="Generate" ToolTip="Generate"
                                                                OnClick="btnGenerate_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/button/generate.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 100px;">
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
    </div>
</asp:Content>
