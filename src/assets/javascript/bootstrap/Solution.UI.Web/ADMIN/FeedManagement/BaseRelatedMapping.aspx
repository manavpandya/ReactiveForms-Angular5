<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="BaseRelatedMapping.aspx.cs" Inherits="Solution.UI.Web.ADMIN.FeedManagement.BaseRelatedMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function ConfirmRemove() {
            return confirm('Are you sure you want to Remove Mapping?');
        }
    </script>
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
                                                    <img class="img-left" title="Base - Related Mapping" alt="Base - Related Mapping" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Base - Related Mapping" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="altrow">
                                                        <td align="Center" colspan="2">
                                                            <asp:Label ID="lblMsg" runat="server" Style="color: red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Choose Name:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlStore" class="order-list" runat="server" AutoPostBack="True"
                                                                Width="180px" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Base Feed :
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:Label ID="lblBaseFeed" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Related Feed:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRelatedFeed" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlRelatedFeed_SelectedIndexChanged"
                                                                Width="220px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td align="left" style="height: 30px; width: 234px;" valign="middle">
                                                                        <b>Product Schema</b>
                                                                    </td>
                                                                    <td align="left" class="dark_bg">
                                                                        <b>Base Schema</b>
                                                                    </td>
                                                                    <td>
                                                                        <b>Fields Mapping</b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <asp:ListBox ID="lbBaseSchema" runat="server" Height="500px" Width="300px"></asp:ListBox>
                                                                        <br />
                                                                        <asp:RequiredFieldValidator ID="rfvBaseSchema" runat="server" ControlToValidate="lbBaseSchema"
                                                                            ErrorMessage="Select atleast one field" SetFocusOnError="true" ValidationGroup="schema"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td align="left" style="height: 30px;" valign="top">
                                                                        <asp:ListBox ID="lbRelatedSchema" runat="server" Height="500px" Width="300px"></asp:ListBox>
                                                                        <br />
                                                                        <asp:RequiredFieldValidator ID="rfvProductSchema" runat="server" ControlToValidate="lbRelatedSchema"
                                                                            ErrorMessage="Select atleast one field" ValidationGroup="schema" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td align="center" valign="top">
                                                                        <asp:GridView ID="grdMappedFields" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                                                            CellSpacing="1" EmptyDataRowStyle-ForeColor="Red" BorderWidth="1px" BorderColor="#E7E7E7"
                                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" EmptyDataText="No Mapping Exists!"
                                                                            PageSize="<%$ appSettings:GridPageSize %>" GridLines="None" OnRowCommand="grdMappedFields_RowCommand"
                                                                            PagerSettings-Mode="NumericFirstLast" Width="100%" OnRowDataBound="grdMappedFields_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        <strong>Base Field</strong>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBaseField" Font-Size="12px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"BaseFieldName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        <strong>Related Field</strong>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRelatedField" Font-Size="12px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RelatedFieldName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        <strong>Remove Mapping</strong>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnMappingID" runat="server" Value='<%#Eval("MappingID") %>' />
                                                                                        <div style="text-align: center">
                                                                                            <asp:ImageButton ID="btnRemove" runat="server" ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                                CommandArgument='<%#Eval("MappingID") %>' CommandName="Remove" OnClientClick="return ConfirmRemove()" />
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="center" Width="10%" />
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
                                        <tr class="oddrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClick="btnSave_Click" />
                                                        <asp:ImageButton ID="btncancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="btncancel_Click" />
                                                    </td>
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
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
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
