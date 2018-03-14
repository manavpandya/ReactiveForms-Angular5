<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ImportPriceCSV.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ImportPriceCSV" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
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
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Import CSV" alt="Import CSV" src="/App_Themes/<%=Page.Theme %>/Images/product_export.png" />
                                                    <h2>
                                                        Import Price, Inventory, Weight CSV</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="width: 70%">
                                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 30%" valign="top">
                                                <a href="/Resources/halfpricedraps/ProductCSV/ImportCSV/ImportProductSample.csv" style="float: right">
                                                    Demo CSV File</a>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="width: 100%" valign="middle" colspan="2">
                                                Import CSV&nbsp;:
                                                <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5;
                                                    color: #000000;" />
                                                <asp:ImageButton ID="btnUpload" AlternateText="Upload" ImageAlign="Top" 
                                                    runat="server" OnClick="btnUpload_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table style="width: auto;" cellspacing="0" cellpadding="0" border="0" class="table"
                                                    id="contVerify" runat="server" visible="false">
                                                    <tbody>
                                                        <tr>
                                                            <td height="15px" align="left" style="width: auto;">
                                                                <asp:CheckBoxList ID="chkFields" runat="server" RepeatDirection="Horizontal">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                            <td style="width: auto;">
                                                                <asp:ImageButton ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:GridView ID="grdCSV" runat="server" AutoGenerateColumns="True" AllowPaging="false"
                                                    BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                    GridLines="None" Width="100%" HorizontalAlign="Center" OnPageIndexChanging="grdCSV_PageIndexChanging">
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
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
