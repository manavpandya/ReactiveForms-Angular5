<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/ADMIN/Admin.Master"
    CodeBehind="CustomerQuoteView.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.CustomerQuoteView" %>

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
                                                    <img class="img-left" title="Add Topic" alt="Add Topic" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="View Customer Quote"></asp:Label></h2>
                                                </div>
                                                <div style="float: right;">
                                                    <asp:ImageButton ID="btnBack" runat="server" OnClick="btnBack_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/button/back.png"
                                                        CausesValidation="false" Style="float: right; padding-right: 10px; padding-top: 0px" />
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            Quote Number:
                                                        </td>
                                                        <td>
                                                            <asp:Label Font-Bold="true" ID="lblQuoteNumber" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="left" valign="top" style="width: 100px; height: 30px; line-height: 22px;">
                                                            Customer Name :
                                                        </td>
                                                        <td align="left" style="line-height: 22px;" valign="Top">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="2" valign="top" style="padding: 0px; padding-bottom: 5px;">
                                                                        <b>
                                                                            <asp:Label ID="lblCustomerName" runat="server"></asp:Label></b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 200px;">
                                                                        <b>Billing Address</b><br />
                                                                        <asp:Label ID="lblBillingAddress" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 200px;">
                                                                        <b>Shipping Address</b><br />
                                                                        <asp:Label ID="lblShippingAddress" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:HiddenField ID="hfCustomerID" runat="server" />
                                                            <%--<asp:LinkButton ID="btnChangeCust" runat="server" OnClick="btnChangeCust_OnClick" Text="Choose Customer" style="font-weight: bold; font-size: 13px; color: #212121">
                                            </asp:LinkButton>--%>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" valign="middle" style="width: 109px; vertical-align: top;">
                                                            Products :
                                                        </td>
                                                        <td align="left">
                                                            <asp:GridView ID="gvProductDisplay" runat="server" CssClass="product_table" PageSize="20"
                                                                Width="100%" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                CellPadding="2" CellSpacing="1" EmptyDataRowStyle-ForeColor="Red" GridLines="None"
                                                                BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7" 
                                                                onrowdatabound="gvProductDisplay_RowDataBound">
                                                                <FooterStyle />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                        <HeaderStyle Height="25px" BackColor=" #E7E7E7" />
                                                                        <HeaderTemplate>
                                                                            <strong>ID</strong>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            Product Name
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle BackColor=" #E7E7E7" Width="25%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                             <asp:Label ID="lblvarname" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                            <asp:Label ID="lblvarvalue" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Height="25px" HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <center>
                                                                                SKU</center>
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle BackColor=" #E7E7E7" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle BackColor=" #E7E7E7" />
                                                                        <HeaderTemplate>
                                                                            <center>
                                                                                Quantity</center>
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="5%" />
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle HorizontalAlign="Right" BackColor=" #E7E7E7" Width="8%" />
                                                                        <HeaderTemplate>
                                                                            Price
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Price", "{0:c}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle CssClass="vendor" HorizontalAlign="Center" BackColor=" #E7E7E7" Width="7%" />
                                                                        <HeaderTemplate>
                                                                            Notes
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNotes" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Notes") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50">
                                                                </PagerSettings>
                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
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
