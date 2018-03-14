<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SalesAgentOrderList.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.SalesAgentOrderList" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Top 10 Low Inventory Items"
                            title="Sales Agent Orders list" class="img-left" />
                        <h2>
                           Sales Agent Recent Orders</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgsalesagtn','trsalesagentList','tblsalesagnt');">
                            <img class="minimize" title="Minimize" id="imgsalesagtn" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trsalesagentList">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdsalesagentlist" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1 %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" Width="2%" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Number">
                                <ItemTemplate>
              
                                        <%#Eval("OrderNumber")%></a></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date">
                              
                                <ItemTemplate>
                                    <%# Eval("OrderDate")%>
                                    &nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" BorderWidth="0" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Agent Name">
                              
                                <ItemTemplate>
                                    <%# Eval("Name")%>
                                    &nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView> 
                </td>
            </tr>
            <tr id="tblsalesagnt">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <%--<td align="right" width="50%">
                                    <a title="View List" href="/Admin/Reports/LowInventory.aspx" style="cursor: pointer;"><span>View List</span></a>
                                    <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                                </td>--%>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>