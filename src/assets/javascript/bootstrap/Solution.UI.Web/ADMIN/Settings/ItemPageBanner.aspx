<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ItemPageBanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.ItemPageBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function checkondelete() {
            jConfirm('Are you sure want to delete selected banner?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnCustDelete").value = id;
                    //alert(document.getElementById("ContentPlaceHolder1_hdnCustDelete").value);
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 4px; float: left; display: none;">
                        <tr>
                            <td style="text-align: right">
                                Store :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="true"
                                    CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Settings/EditItemPageBanner.aspx">
                    <img alt="Add Item Page Banner" title="Add Item Page Banner" src="/App_Themes/<%=Page.Theme %>/images/additempagebanner.png" /></a></span>
            </div>
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
                                                    <img class="img-left" title="Topic List" alt="Topic List" src="/App_Themes/<%=Page.Theme %>/images/topic-list-icon.png">
                                                    <h2>
                                                        Item Page Banner List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <span class="star" style="vertical-align: middle; text-align: center; padding-right: 20%;">
                                                    <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <fieldset>
                                                    <legend style="font-weight: bold; border: 1px Solid #848484; line-height: 30px;">Item
                                                        Page - Swatch Banner(s)</legend>
                                                    <asp:GridView ID="gvBigBannerType" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                        AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="100%" AllowPaging="True"
                                                        PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                        OnRowCommand="gvBigBannerType_RowCommand" OnRowDataBound="gvBigBannerType_RowDataBound"
                                                        OnRowDeleting="gvBigBannerType_RowDeleting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr No.">
                                                                <HeaderStyle />
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex+1 %>.
                                                                    <asp:HiddenField ID="hdnItemBannerTypeID" runat="server" Value='<%#Eval("ItemBannerTypeID") %>' />
                                                                    <asp:HiddenField ID="hdnItemBannerID" runat="server" Value='<%#Eval("ItemBannerID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText=" Banner Title">
                                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBannerTitle" runat="server" Text='<%# Bind("BannerTitle") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Banner Image">
                                                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgBannerImage" runat="server" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText=" Banner URL">
                                                                <HeaderStyle HorizontalAlign="Left" Width="27%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBannerUrl" runat="server" Text='<%# Bind("BannerUrl") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Active">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgIsActive" runat="server" CommandName="ActiveBanner" CommandArgument='<%# Eval("ItemBannerID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                        CommandArgument='<%# Eval("ItemBannerID") %>'></asp:ImageButton>
                                                                    <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="delete"
                                                                        CommandArgument='<%# Eval("ItemBannerID") %>' OnClientClick="return confirm('Are you sure want to delete selected banner?');">
                                                                    </asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <fieldset>
                                                    <legend style="font-weight: bold; border: 1px Solid #848484; line-height: 30px;">Item
                                                        Page - Small Banner</legend>
                                                    <asp:GridView ID="gvSmallBannerType" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                        AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="100%" AllowPaging="True"
                                                        PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                        OnRowCommand="gvSmallBannerType_RowCommand" OnRowDataBound="gvSmallBannerType_RowDataBound"
                                                        OnRowDeleting="gvSmallBannerType_RowDeleting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr No.">
                                                                <HeaderStyle />
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex+1 %>.
                                                                    <asp:HiddenField ID="hdnItemBannerTypeID" runat="server" Value='<%#Eval("ItemBannerTypeID") %>' />
                                                                    <asp:HiddenField ID="hdnItemBannerID" runat="server" Value='<%#Eval("ItemBannerID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText=" Banner Title">
                                                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBannerTitle" runat="server" Text='<%# Bind("BannerTitle") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Banner Image">
                                                                <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgBannerImage" runat="server" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText=" Banner URL">
                                                                <HeaderStyle HorizontalAlign="Left" Width="27%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBannerUrl" runat="server" Text='<%# Bind("BannerUrl") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Active">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgIsActive" runat="server" CommandName="ActiveBanner" CommandArgument='<%# Eval("ItemBannerID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                        CommandArgument='<%# Eval("ItemBannerID") %>'></asp:ImageButton>
                                                                    <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="delete"
                                                                        CommandArgument='<%# Eval("ItemBannerID") %>' OnClientClick="return confirm('Are you sure want to delete selected banner?');">
                                                                    </asp:ImageButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    </asp:GridView>
                                                </fieldset>
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
