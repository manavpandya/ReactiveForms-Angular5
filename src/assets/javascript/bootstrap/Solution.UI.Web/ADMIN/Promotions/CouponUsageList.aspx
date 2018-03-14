<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CouponUsageList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.CouponUsageList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            <div>
               
                                                                            <asp:Button ID="btnexport1" OnClick="btnexport1_Click" runat="server" />
                                                                       
            </div>
            <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="javascript:void(0);" id="abacklink" runat="server">
                <img alt="Add Coupon" title="Back" src="/App_Themes/<%=Page.Theme %>/images/back.gif" /></a></span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Promotions/Coupon.aspx">
                <img alt="Add Coupon" title="Add Coupon" src="/App_Themes/<%=Page.Theme %>/images/add-coupon.png" /></a></span>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                <img class="img-left" alt="" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>
                                                    <asp:Literal ID="lttitle" runat="server" Text="Coupon Usage for"></asp:Literal></h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" colspan="2" >
                                                        Store Name : &nbsp;&nbsp;
                                                        <asp:DropDownList ID="ddlstore" runat="server" Width="180px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlstore_SelectedIndexChanged" CssClass="order-list">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                   
                                                           <td align="left" colspan="2" >
                                                               Total Records :  &nbsp;&nbsp;
                                                               <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
                                                               </td>
                                                   
                                                </tr>
                                              
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="gridcouponusage" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None"   ShowFooter="true"
                                                 CellPadding="2" CellSpacing="1" DataSourceID="" OnRowDataBound="gridcouponusage_RowDataBound">
                                               
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr No.">
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Order Number">
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <HeaderStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <a href="/Admin/Orders/Orders.aspx?id=<%#Eval("OrderNumber") %>">
                                                                  <asp:Label ID="lblorderno" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                            </a>
                                                          
                                                        </ItemTemplate>
                                                          
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Used by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbusedby" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Email") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Used on">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbusedon" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"useon") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate >
                                                                                     Total : 
                                                                                    </FooterTemplate>
                                                       
                                                    </asp:TemplateField>
                                                      

                                                       <asp:TemplateField HeaderText="Sale Amount">
                                                        <ItemStyle HorizontalAlign="right" />
                                                        <HeaderStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsaleamount" runat="server" Text='<%# String.Format("{0:C}",Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"saleamount"))) %>'></asp:Label>
                                                        </ItemTemplate>
                                                             <FooterTemplate>
                                                                                       <asp:Label ID="lblTotal" runat="server" />
                                                                                    </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                              
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                 <FooterStyle BackColor="#E1E1E1" ForeColor="#000" Font-Bold="true" HorizontalAlign="right"
                                                            BorderWidth="0" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
