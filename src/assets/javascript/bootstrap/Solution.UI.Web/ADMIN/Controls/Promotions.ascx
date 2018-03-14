<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Promotions.ascx.cs" Inherits="Solution.UI.Web.ADMIN.Controls.Promotions" %>
<div>
    <table style="width:100%;" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Top 10 Coupon List"
                            title="Top 10 Coupon List" class="img-left" />
                        <h2>
                            Top 10 Coupon List</h2>
                    </div>
                    <div class="main-title-right">
                       <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgItemPromotions','tr5TenItemsPromotions','tblItemPromotions');">
                            <img class="minimize" title="Minimize" id="imgItemPromotions" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="tr5TenItemsPromotions">
                <td align="center" valign="middle">
                    <asp:GridView ID="grdTopCouponslist" runat="server" CssClass="dashboard-left" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;">
                        <Columns>
                                                    
                                                    <asp:TemplateField HeaderText="Coupon Code" HeaderStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            Coupon Code
                                                           
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                             <asp:HiddenField ID="hdncouponid" runat="server" Value='<%#Eval("CouponID") %>' />
                                                            <asp:Label ID="lbcouponcode" runat="server" Text='<%# bind("CouponCode") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                    <asp:TemplateField HeaderText="Ex. Date" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Ex. Date
                                                           
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                              <%# String.Format("{0:MM/dd/yyyy}", Eval("ExpirationDate"))%>
                                                          <%--  <asp:Label ID="lbexpiredate" runat="server" Text='<%# bind("ExpirationDate") %>'></asp:Label>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  
                                                    <asp:TemplateField HeaderText="Discount Amount" HeaderStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            Discount Amount
                                                           
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbdiscountamt" runat="server" Text='<%# String.Format("{0:F}", DataBinder.Eval(Container.DataItem,"DiscountAmount"))%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                                                   
                                                    
                                                </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="tblItem">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table style="width:100%">
                        <tbody>
                            <tr>
                                <td align="right" width="50%">
                                </td>
                                <td align="right" width="50%">
                                    <a title="View List" href="/Admin/Promotions/CouponsList.aspx?Storeid=<%=storeid%>&loginid=<%=loginid%>" style="cursor: pointer;"><span>View List</span></a>
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