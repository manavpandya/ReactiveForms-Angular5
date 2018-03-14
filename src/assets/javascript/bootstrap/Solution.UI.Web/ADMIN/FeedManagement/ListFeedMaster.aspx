<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListFeedMaster.aspx.cs" EnableViewState="true" Inherits="Solution.UI.Web.ADMIN.FeedManagement.ListFeedMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function SearchValidation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 2px;" cellpadding="3" cellspacing="3">
                        <tr>
                            <td style="padding-left: 0px; width: 30%" valign="top" align="left">
                                Store : &nbsp;
                                <asp:DropDownList ID="ddlStore" runat="server" Width="200px" AutoPostBack="true"
                                    CssClass="order-list" 
                                    onselectedindexchanged="ddlStore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="FeedMaster.aspx">
                    <img alt="Add Feed" title="Add Feed" src="/App_Themes/<%=Page.Theme %>/images/create-feed.png" /></a></span>
            </div>
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
                                                    <img src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png" alt="Feed Master List"
                                                        title="Feed Master List" class="img-left">
                                                    <h2>
                                                        Feed Master List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 70%; padding-right: 0px;" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td valign="top" style="text-align: left">
                                                                        Search :&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                                            Width="250px"></asp:TextBox><br />
                                                                        <span style="padding-left: 50px;">(eg. Feed Name) </span>
                                                                    </td>
                                                                    <td valign="top" style="padding-right: 0px; line-height: 22px;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                                                            OnClick="btnSearch_Click" />
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" /><br />
                                                                        <span style="float: right;padding-right: 5px; font-size: 12px;
                                                                            font-family: Arial,Helvetica,sans-serif;">
                                                                            <asp:Label ID="lblTotcount" runat="server"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdFeedMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="FeedID"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    BorderWidth="1px" BorderColor="#E7E7E7" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="50"
                                                    PagerSettings-Mode="NumericFirstLast" CellPadding="3" CellSpacing="1" OnRowEditing="grdFeedMaster_RowEditing"
                                                    OnPageIndexChanging="grdFeedMaster_PageIndexChanging" OnRowDataBound="grdFeedMaster_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                <strong>Select </strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFeedId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FeedId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                                            <HeaderTemplate>
                                                                Sr No.
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <HeaderTemplate>
                                                                Feed Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFeedName" runat="server" Text='<%# Eval("FeedName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                            <HeaderTemplate>
                                                                Store Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# Eval("StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                                            <HeaderTemplate>
                                                                Is Base
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgIsActive" runat="server" AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBase")).ToString() %>'
                                                                    ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBase")).ToString() %>'
                                                                    ImageUrl='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBase"))) %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" Width="90px" />
                                                            <HeaderTemplate>
                                                                Created On
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedon" runat="server" Text='<%# Eval("Createdon") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                                            <HeaderTemplate>
                                                                Edit
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
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
        </div>
    </div>
</asp:Content>
