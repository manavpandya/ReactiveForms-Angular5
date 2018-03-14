<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchLog.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.SearchLog" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/search-log.png" alt="Top 10 Search Terms" title="Top 10 Search Terms"
                            class="img-left" />
                        <h2>
                           Top 10 Search Terms</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgSearchLog','trSearchlog','tblSearchLog');">
                            <img class="minimize" title="Minimize" id="imgSearchLog" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trSearchlog">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdSearchLog" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" Style="margin-bottom: 0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1 %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Search Terms">
                                <ItemTemplate>
                                <asp:Label ID="lblSearchTerm" runat="server" Text='<%# SetItemName(Convert.ToString(Eval("SearchTerm")))%>' ToolTip='<%# Eval("SearchTerm")%>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Created On">
                                <ItemTemplate>
                                    <%# Eval("CreatedOn")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="tblSearchLog">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/SearchLogList.aspx" style="cursor: pointer;"><span>View List</span></a>
                                    <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
