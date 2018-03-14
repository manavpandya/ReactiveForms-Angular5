<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="AdminRights.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.AdminRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">

        function checkCount() {
           // var ddlid = document.getElementById('ContentPlaceHolder1_ddlStore');
           // var ddl = ddlid.options[ddlid.selectedIndex].value;
          //  alert(ddl);  if (ddl < 0) {
           //     jAlert('Please select at least one Store...', 'Message', '');
              
            //    return false;
           // }
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Please select at least one Right...', 'Message', ''); });
                return false;
            }
        }
        function checkAllfalse() {

            var inputs = document.getElementsByTagName('input');
            var checkboxes = [];
            for (var i = 0; i < inputs.length; i++) {

                if (inputs[i].type == 'checkbox') {
                    inputs[i].checked = false;
                }
            }
        }

    </script>
    <style type="text/css">
        #ContentPlaceHolder1_chklrights input
        {
            margin: 0 5px 0 0;
            float: left;
        }
    </style>
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
                                            <th colspan="4">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Administrator Roles" alt="Administrator Roles" src="/App_Themes/<%=Page.Theme %>/Images/admin-rights-icon.png">
                                                    <h2>
                                                        Admin Rights</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" style="padding: 10px; width: 7%">
                                                Users :
                                            </td>
                                            <td align="left" style="padding: 10px;" colspan="3">
                                                <asp:DropDownList ID="ddlAdmins" runat="server" Width="300px" CssClass="order-list"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlAdmins_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="altrow" style="display:none;">
                                            <td align="left" style="padding: 10px; width: 11%">
                                                Store List :
                                            </td>
                                            <td align="left" style="padding: 10px;" colspan="3">
                                                <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list"   Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" style="padding: 10px; width: 11%">
                                                Template List :
                                            </td>
                                            <td align="left" style="padding: 10px;" colspan="3">
                                                <asp:DropDownList ID="ddltemplatename" runat="server" Width="300px" CssClass="order-list"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddltemplatename_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="4">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #DADADA;
                                                    border-collapse: collapse;">
                                                    <tr style="height: 27px;">
                                                        <td colspan="2" align="center" style="border: 1px solid #DADADA; background: #e7e7e7;
                                                            font-weight: bold;">
                                                            Tab Rights
                                                        </td>
                                                        <td colspan="2" align="center" style="border: 1px solid #DADADA; background: #e7e7e7;
                                                            font-weight: bold;">
                                                            Page Rights
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="vertical-align: top; padding: 10px; width: 7%;">
                                                        </td>
                                                        <td align="left" style="padding: 10px; width: 20%; vertical-align: top; border-right: solid 1px #DADADA;">
                                                            <asp:CheckBoxList ID="chklrights" runat="server">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td align="left" style="vertical-align: top; padding: 10px; width: 7%;">
                                                        </td>
                                                        <td align="left" style="vertical-align: top; padding: 10px">
                                                            <asp:GridView ID="gvAdminPageRights" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                ViewStateMode="Enabled" Width="70%" BackColor="White" GridLines="None" CellPadding="4"
                                                                CellSpacing="1" Style="background: none; border: 1px solid #DADADA;" OnPageIndexChanging="gvAdminPageRights_PageIndexChanging"
                                                                AllowPaging="true" PageSize="20">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select" Visible="false">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label>
                                                                            <asp:Label ID="lblCompareAdminID" runat="server" Text='<%#Eval("AdminID") %>'></asp:Label>
                                                                            <asp:Label ID="lblInnerRightsID" runat="server" Text='<%#Eval("InnerRightsID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tab Name" ItemStyle-Width="20%">
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label runat="server" ID="lblTabname" Text='<%#Eval("TabName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Section Name" ItemStyle-Width="70%">
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemTemplate>
                                                                            &nbsp;<asp:Label runat="server" ID="lblsectionname" Text='<%#Eval("PageName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Is Listed" ItemStyle-Width="10%">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkIsListed" runat="server" Checked='<%# Convert.ToBoolean(Eval("isListed"))%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle HorizontalAlign="Center" />
                                                                <AlternatingRowStyle CssClass="altrow" />
                                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left" style="padding-left: 15px; border-right: 1px solid #DADADA;">
                                                            <asp:Button ID="btnUpdateRights" runat="server" AlternateText="Update Admin Rights"
                                                                ToolTip="Update Admin Rights" OnClientClick="return checkCount();" OnClick="btnUpdateRights_Click"
                                                                Visible="false" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td align="left" style="padding-left: 15px;">
                                                            <asp:Button ID="btnUpdatePageRight" runat="server" AlternateText="Update" ToolTip="Update"
                                                                OnClientClick="return checkCount();" OnClick="btnUpdatePageRight_Click" />
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
            </table>
        </div>
    </div>
</asp:Content>
