<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="TradeInquiries.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Customers.TradeInquiries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">

        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/Images/customer-list-icon.png" />
                                                    <h2>Trade Inquiries List</h2>
                                                </div>
                                            </th>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="even-row">
                    <td>
                        <asp:GridView ID="grdTradeInquiries" runat="server" AutoGenerateColumns="False" DataKeyNames="TradeApplicationID"
                            EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                            GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" CssClass="content-table "
                            PagerSettings-Mode="NumericFirstLast"
                            CellPadding="2" CellSpacing="1"
                            BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7" OnRowDataBound="grdTradeInquiries_RowDataBound" OnRowCommand="grdTradeInquiries_RowCommand" OnPageIndexChanging="grdTradeInquiries_PageIndexChanging">
                            <Columns>


                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Company Name
                                    </HeaderTemplate>
                                    <ItemTemplate>

                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# Eval("CompanyName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Business Address
                                                               
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStreet" runat="server" Text='<%# Eval("Street")%>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City")%>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lblState" runat="server" Text='<%# Eval("State")%>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lblZipCode" runat="server" Text='<%# Eval("ZipCode")%>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lblWebsite" runat="server" Text='<%# Eval("Website")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Customer Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Email
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Phone Number
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhoneNo" runat="server" Text='<%# Eval("Phone1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Request Date
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor=" #E7E7E7" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Download PDF
                                    </HeaderTemplate>
                                    <HeaderStyle BackColor=" #E7E7E7" />
                                    <ItemTemplate>
                                        <asp:HiddenField ID="DocumentFile" Value='<%# DataBinder.Eval(Container.DataItem,"DocumentFile") %>' runat="server" />

                                        <input type="hidden" id="mainid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"TradeApplicationID") %>' />
                                        <asp:Button ToolTip="Download PDF" CommandName="downloadpdf" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"DocumentFile") %>'
                                            ID="dnwid" Visible="false" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        View
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" style="color: #212121;" onclick="OpenCenterWindow('/admin/Reports/MailDetail.aspx?MID=<%# DataBinder.Eval(Container.DataItem,"MailID") %>',1000,600);">
                                            <img src="/App_Themes/<%=Page.Theme %>/images/view-details.png" border="0" /></a>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="middle" HorizontalAlign="Center" CssClass="border" />
                                </asp:TemplateField>

                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom" PageButtonCount="50"></PagerSettings>
                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                            <AlternatingRowStyle CssClass="altrow" />
                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                            <HeaderStyle Font-Bold="false" />
                        </asp:GridView>
                    </td>
                </tr>

            </table>

        </div>
    </div>

</asp:Content>
