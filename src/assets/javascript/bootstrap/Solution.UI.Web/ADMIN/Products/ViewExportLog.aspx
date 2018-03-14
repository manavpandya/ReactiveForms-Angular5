﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewExportLog.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ViewExportLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none;">
    <form id="form1" runat="server">

        <div>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                style="padding: 2px;">
                <tbody>

                    <tr>
                        <th>
                            <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                                 Log Details
                            </div>
                            <div class="main-title-right">
                                <a href="javascript:void(0);" style="display: none;" class="show_hideMainDiv" runat="server" id="btnClose"
                                    onclick="window.close();">
                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                            </div>
                        </th>
                    </tr>

                    <tr>
                        <td style="padding: 2px;" align="right">
                            <table>
                                <tr>
                                    <td>Search By SKU:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsearch" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnsearch" runat="server" Style="background: url(/App_Themes/Gray/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border: none; cursor: pointer;" OnClick="btnsearch_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShowAll" Style="background: url(/App_Themes/Gray/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border: none; cursor: pointer;" runat="server" OnClick="btnShowAll_Click" />
                                    </td>
                                </tr>


                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style="padding: 2px">
                            <div id="poorderprint" style="border: 5px solid #e7e7e7; overflow-y: scroll; height: 450px; padding-top: 2px;">
                                <asp:GridView ID="gvhemminglog" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Record Found."
                                    class="order-table" Style="border: solid 1px #e7e7e7" ShowFooter="true" PageSize="30" AllowPaging="true" OnPageIndexChanging="gvhemminglog_PageIndexChanging">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <label style="color: Red">
                                            No records found ...</label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SKU
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblasin" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                UPC
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblsku" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Before(Discontinue)
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblbefore" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"beforeimport") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                After(Discontinue)
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblafter" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Discontinue") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                Type
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lbltype" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Type") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Created By
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblIP" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"adminname") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Created On
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lbldate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"createdon") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>


                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                    <FooterStyle CssClass=".order-table td" />
                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>
    </form>
</body>
</html>