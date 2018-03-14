<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgentMonthlysales.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.AgentMonthlysales" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Monthly Sales Agent wise" title="Monthly Sales Agent wise"
                            class="img-left">
                        <h2>
                            Monthly Sales Agent wise</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgmnhtlysales','trmnthlysales','trmnthlysaleslist');">
                            <img class="minimize" id="imgmnhtlysales" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trmnthlysales">
                <td>
                    <span style="float: right; text-decoration: none;">
                        <asp:DropDownList ID="ddlOption" onchange="chkHeight();" CssClass="order-list" Style="width: 90px;
                            font-family: Arial,Helvetica,sans-serif; font-size: 11px; text-decoration: none;"
                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOption_SelectedIndexChanged">
                        </asp:DropDownList>
                    </span>
                </td>
            </tr>
            <tr id="trmnthlysaleslist">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdsalesagentlistmnthly" runat="server" CssClass="dashboard-left"
                        CellPadding="0" Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1"
                        BorderStyle="Solid" BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found."
                        EditRowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red"
                        Style="margin-bottom: 0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex +1 %>.
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" Width="2%" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sales Agent">
                                <ItemTemplate>
                                    <%#Eval("Name")%></a></ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Sale">
                                <ItemTemplate>
                                    <%# Eval("totalsales")%>
                                    &nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemTemplate>
                                    $<%# String.Format("{0:0.00}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"OrderTotal")))%>
                                    &nbsp;
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
        </tbody>
    </table>
</div>
