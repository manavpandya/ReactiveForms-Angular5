<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="HeaderLinkList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.HeaderLinkList" %>

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
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {
                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }

        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkCount() {
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
                $(document).ready(function () { jAlert('Select at least One Header Link !', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Header Link ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
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
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%">
            <span style="vertical-align: middle; margin-top: 4px; margin-right: 3px; float: left;">
                <table>
                    <tr>
                        <td>
                            Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstore" runat="server" CssClass="order-list" Width="170px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                href="/Admin/Settings/HeaderLink.aspx">
                <img alt="Add Header Link" title="Add Header Link" src="/App_Themes/<%=Page.Theme %>/images/add-header-link.png" /></a></span>
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
                                        <th colspan="3">
                                            <div class="main-title-left">
                                                <img class="img-left" title="Header Links" alt="Header Links" src="/App_Themes/<%=Page.Theme %>/Images/header-links-icon.png" />
                                                <h2>
                                                    Header Links</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right" style="padding-right: 0px;">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" valign="middle" style="padding-right: 0px;width: 90%">
                                                        Search :&nbsp;&nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield"
                                                            Width="124px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        <asp:Button ID="btnSearch" runat="server" OnClientClick='return validation();' OnClick="btnSearch_Click" />
                                                    </td>
                                                    <td style="width: 5%">
                                                        <asp:Button ID="btnshowall" runat="server" CommandName="ShowAll" OnClick="btnshowall_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                TypeName="Solution.Bussines.Components.HeaderlinkComponent" SelectMethod="GetDataByFilter"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnheaderlink" DbType="String" DefaultValue="PageId"
                                                        Name="CName" />
                                                    <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                    <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hdnheaderlink" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td colspan="3">
                                            <asp:GridView ID="grdheaderlink" runat="server" AutoGenerateColumns="False" DataKeyNames="PageId"
                                                CellSpacing="1" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridObjectDataSource"
                                                OnRowDataBound="grdheaderlink_RowDataBound" OnRowCommand="grdheaderlink_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdnheaderlinkid" runat="server" Value='<%#Eval("PageId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Header Name" ItemStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbheadername" runat="server" Text='<%# bind("PageName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Header Link" ItemStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbheaderlink" runat="server" Text='<%# bind("PageURL") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Display Order" ItemStyle-Width="15%">
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbdisplayorder" runat="server" Text='<%# bind("DisplayOrder") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" ItemStyle-Width="25%">
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbstorename" runat="server" Text='<%# bind("StoreName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="lnkedit" ToolTip="Edit" CommandName="edit" CommandArgument='<%# Eval("PageId") %>'>
                                                            </asp:ImageButton>
                                                        </ItemTemplate>
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
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr id="trBottom" runat="server">
                                                    <td class="style1">
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                            Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 10%; padding-right: 0px;">
                                                        <asp:ImageButton ID="btndelete" runat="server" ToolTip="Delete" OnClientClick='return checkCount()'
                                                            OnClick="btndelete_Click" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btndelete1_Click" />
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
