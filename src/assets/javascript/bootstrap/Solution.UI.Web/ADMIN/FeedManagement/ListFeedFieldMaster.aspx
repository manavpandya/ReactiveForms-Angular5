<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ListFeedFieldMaster.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.ListFeedFieldMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function SearchValidation() {
            if (document.getElementById("<%=txtSearch.ClientID %>") != null && document.getElementById("<%=txtSearch.ClientID %>").value == '') {
                jAlert('Please Enter Search Value.', 'Required Information', '<%=txtSearch.ClientID %>');
                return false;
            }
            return true;
        }
        function storeValidation() {
            if (document.getElementById("<%=ddlStoreFrom.ClientID %>") != null && document.getElementById("<%=ddlStoreFrom.ClientID %>").selectedIndex == -1) {
                jAlert('Please Select Store From.', 'Required Information', '<%=ddlStoreFrom.ClientID %>');
                return false;
            }
            else if (document.getElementById("<%=ddlFeedNameStore.ClientID %>") != null && document.getElementById("<%=ddlFeedNameStore.ClientID %>").selectedIndex == -1) {
                jAlert('Please Select Feed Name.', 'Required Information', '<%=ddlFeedNameStore.ClientID %>');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <table width="100%">
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td>
                                    Store :
                                    <asp:DropDownList ID="ddlStore" Width="200px" runat="server" AutoPostBack="true"
                                        CssClass="order-list" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Feed Name :
                                    <asp:DropDownList ID="ddlFeedName" runat="server" CssClass="order-list" Style="width: 170px !important"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlFeedName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table>
                            <tr>
                                <td valign="middle" style="text-align: left">
                                    Search :&nbsp;
                                    <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Style="width: 90px !important">
                                        <asp:ListItem Text="Field Name" Value="FieldName"></asp:ListItem>
                                        <asp:ListItem Text="Field Type" Value="TypeName"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="100px"></asp:TextBox><br />
                                </td>
                                <td valign="middle" style="padding-right: 0px; line-height: 22px;">
                                    <asp:Button ID="btnSearch" runat="server" OnClientClick="return SearchValidation();"
                                        OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" /><br />
                                    <span style="float: right; padding-right: 5px; font-size: 12px; font-family: Arial,Helvetica,sans-serif;">
                                        <asp:Label ID="lblTotcount" runat="server"></asp:Label>
                                    </span>
                                </td>
                                <td valign="top" align="right" style="vertical-align: top; padding-bottom: 9px">
                                    <a id="imgAddFeedField" runat="server" href="EditFeedFieldMaster.aspx">
                                        <img alt="Add Feed" title="Add Feed" src="/App_Themes/<%=Page.Theme %>/images/create-feed-field.png" /></a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
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
                                                <div class="main-title-left" style="width: 98% !important">
                                                    <img src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png" style="padding-top: 4px"
                                                        alt="Feed Field Master List" title="Feed Field Master List" class="img-left" />
                                                    <h2>
                                                        <span style="line-height: 25px;">Feed Field Master List</span> <span style="float: right;">
                                                            Store From :
                                                            <asp:DropDownList Style="margin-bottom: 2px" CssClass="order-list" Width="180px"
                                                                AutoPostBack="true" runat="server" ID="ddlStoreFrom" OnSelectedIndexChanged="ddlStoreFrom_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            Feed Name :
                                                            <asp:DropDownList ID="ddlFeedNameStore" CssClass="order-list" runat="server" Width="165px"
                                                                AutoPostBack="false">
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btnImportdata" OnClientClick="return storeValidation();" runat="server"
                                                                Text="" Style="width: 119px; height: 25px; vertical-align: bottom" OnClick="btnImportdata_Click" />
                                                        </span>
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <div class="content_box">
                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                        <tbody>
                                                            <tr>
                                                                <td class="border">
                                                                    <asp:GridView ID="gvFeedMaster" runat="server" AutoGenerateColumns="False" DataKeyNames="FeedID"
                                                                        EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                                        BorderWidth="1px" BorderColor="#E7E7E7" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                        ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="30"
                                                                        PagerSettings-Mode="NumericFirstLast" CellPadding="3" CellSpacing="1" OnRowDataBound="gvFeedMaster_RowDataBound"
                                                                        OnPageIndexChanging="gvFeedMaster_PageIndexChanging" OnRowCommand="gvFeedMaster_RowCommand">
                                                                        <Columns>
                                                                            <asp:TemplateField Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFieldID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FieldID") %>'></asp:Label>
                                                                                    <asp:Label ID="lblFeedId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FeedId") %>'></asp:Label>
                                                                                    <asp:Label ID="lblFieldTypeID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FieldTypeID") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Select">
                                                                                <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="Left" />
                                                                                <HeaderTemplate>
                                                                                    Field Name
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldName")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Feed Name
                                                                                </HeaderTemplate>
                                                                                <ItemStyle  HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FeedName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Field Type
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTypeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TypeName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Left" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Field Description
                                                                                </HeaderTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFeedType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldDescription") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Width
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblWidth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldWidth") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Height
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblHeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldHeight") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle HorizontalAlign="Center" BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    &nbsp;Add Value
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <a id="tabaddvalue" runat="server">Add Value</a>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Edit">
                                                                                <HeaderStyle BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    Edit
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="hlEdit" runat="server" ImageUrl="~/Admin/images/edit_icon.gif"
                                                                                        PostBackUrl='<%# "EditFeedFieldMaster.aspx?Mode=edit&ID=" + DataBinder.Eval(Container.DataItem,"FieldID")%>'
                                                                                        ToolTip='<%# "Edit " + DataBinder.Eval(Container.DataItem,"fieldName") %>'></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle BackColor="#E7E7E7" />
                                                                                <HeaderTemplate>
                                                                                    Delete
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FieldID") %>' OnClientClick="javascript:return confirm('Are you sure to delete this record ?');"
                                                                                        CommandName="delMe" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                        <AlternatingRowStyle CssClass="altrow" />
                                                                        <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                    </asp:GridView>
                                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="product_table">
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
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
