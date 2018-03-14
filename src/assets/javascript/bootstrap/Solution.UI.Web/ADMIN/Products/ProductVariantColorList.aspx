<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductVariantColorList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductVariantColorList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkondelete(id) {
            jConfirm('Are you sure want to delete selected Color ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById("ContentPlaceHolder1_hdnColorDelete").value = id;
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
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrl = "../Images/active.gif";
            }
            else
            {
                _ReturnUrl = "../Images/in-active.gif";
            }
            return _ReturnUrl;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin-right: 3px; float: right;"><a href="/Admin/Products/AddProductVariantColor.aspx">
                    <img alt="Add Product Color" title="Add Product Color" src="/App_Themes/<%=Page.Theme %>/images/add-prodctcolor-vatiant.png" /></a></span>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                            <th colspan="4">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Product Color List" alt="Product Color List" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png" />
                                                    <h2>
                                                        Product Color List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right" style="width: 100%;">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td style="width: 70%">
                                                        </td>
                                                        <td align="right" style="width: 10%">
                                                            Search&nbsp;:
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"
                                                                Style="vertical-align: middle; float: right"></asp:TextBox>
                                                        </td>
                                                        <td align="right" style="width: 5%;">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                                        </td>
                                                        <td align="right" style="width: 5%;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="4">
                                                <asp:GridView ID="grdSearchProductType" runat="server" AutoGenerateColumns="False"
                                                    DataKeyNames="ColorId" EmptyDataText="No Record(s) Found." AllowSorting="false"
                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" OnRowCommand="grdSearchProductType_RowCommand" Width="100%"
                                                    GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast"
                                                    OnRowDataBound="grdSearchProductType_RowDataBound" OnPageIndexChanging="grdSearchProductType_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnCustListID" runat="server" Value='<%#Eval("ColorID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                            <HeaderTemplate>
                                                                Color ID
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustID" runat="server" Text='<%# Eval("ColorID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Color Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("ColorName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Image
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblImagePath" runat="server" Visible="false" Text='<%# Eval("ImagePath") %>'></asp:Label>
                                                                <asp:Literal ID="ltrImagePath" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status">
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemTemplate>
                                                                <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                    alt="" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("ColorID") %>'></asp:ImageButton>
                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="delete"
                                                                    message='<%# Eval("ColorID") %>' CommandArgument='<%# Eval("ColorID") %>' OnClientClick='return checkondelete(this.getAttribute("message"));'>
                                                                </asp:ImageButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <asp:Button ID="btnhdnDelete" runat="server" Text="Button" OnClick="btnhdnDelete_Click"
                                                    Style="display: none;" />
                                                <asp:HiddenField ID="hdnColorDelete" runat="server" Value="0" />
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
