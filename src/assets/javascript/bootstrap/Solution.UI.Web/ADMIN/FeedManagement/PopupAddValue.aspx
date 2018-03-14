<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupAddValue.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.FeedManagement.PopupAddValue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Field Values</title>
    <script type="text/javascript">
        function onKeyPressPhone(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
                return key;
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
        function CheckValidations() {
            if ((document.getElementById('txtValue').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Field Value', 'Message', 'txtValue');
                return false;
            }
            if ((document.getElementById('txtDisplayOrder').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Display Order', 'Message', 'txtDisplayOrder');
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Field Value(s)
                            <asp:HiddenField ID="hdnValueId" Value="0" runat="server" />
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; padding: 5px;
                            width: 100%; margin-bottom: 13px;">
                            <tr>
                                <td style="width: 120px;">
                                    <span style="color: Red">*</span> Value :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValue" CssClass="textboxcommonstyle" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: Red">*</span> Display Order :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayOrder" CssClass="textboxcommonstyle" onkeypress="return onKeyPressPhone(event);"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnSubmit" runat="server" OnClientClick="return CheckValidations();"
                                        OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="3" cellspacing="0" style="border: 1px solid gray; width: 100%;
                            margin-bottom: 13px;">
                            <tr>
                                <td>
                                    <div>
                                        <asp:GridView ID="grdSelected" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                            AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" BorderWidth="1px" BorderColor="#E7E7E7"
                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                            GridLines="None" AllowPaging="True" PageSize="30" PagerSettings-Mode="NumericFirstLast"
                                            CellPadding="3" CellSpacing="1" OnRowDataBound="grdSelected_RowDataBound" OnPageIndexChanging="grdSelected_PageIndexChanging"
                                            OnRowCommand="grdSelected_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Field Name</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        &nbsp;
                                                        <asp:Label ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Field Type</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFieldType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TypeName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Field Value</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFieldValues" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldValues") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%"  HorizontalAlign="Left"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Display Order</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisplayOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Edit</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEdit" runat="server" Height="20px" ImageUrl="~/Admin/images/edit_icon.gif"
                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ValueId") %>' CommandName="EditMe" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Delete</HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ValueId") %>' OnClientClick="javascript:return confirm('Are you sure to delete this record ?');"
                                                            CommandName="delMe" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
