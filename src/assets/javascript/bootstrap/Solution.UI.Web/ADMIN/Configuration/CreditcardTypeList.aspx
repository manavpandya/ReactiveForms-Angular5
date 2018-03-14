<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CreditcardTypeList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.CreditcardTypeList" %>

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
                $(document).ready(function () { jAlert('Check at least One Credit Card Type !', 'Message'); });
                return false;
            }
            else {

                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Credit Card Type ?', 'Confirmation', function (r) {
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
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                <table>
                    <tr>
                        <td style="padding-left: 0px;">
                            Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstore" runat="server" CssClass="order-list" Width="170px"
                                OnSelectedIndexChanged="ddlstore_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                href="/Admin/configuration/CreditCardType.aspx">
                <img alt="Add Credit Card Type" title="Add Credit Card Type" src="/App_Themes/<%=Page.Theme %>/images/add-credit-card-type.png" /></a></span>
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
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Credit Card List" alt="Credit Card List" src="/App_Themes/<%=Page.Theme %>/Images/credit-card-list-icon.png" />
                                                <h2>
                                                    Credit Card List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" valign="middle" style="padding-right: 0px;width: 94%;">
                                                        Search : &nbsp;
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="160px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 3%;">
                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick='return validation();' />
                                                    </td>
                                                    <td style="width: 3%;">
                                                        <asp:Button ID="btnshowall" runat="server" OnClick="btnshowall_Click" CommandName="ShowAll" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                TypeName="Solution.Bussines.Components.CreditcardComponent" SelectMethod="GetDataByFilter"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdncreditcard" DbType="String" DefaultValue="CardTypeID"
                                                        Name="CName" />
                                                    <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                    <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="pSearchValue" DefaultValue="" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hdncreditcard" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grdcreditcard" runat="server" AutoGenerateColumns="False" DataKeyNames="CardTypeID"
                                                BorderStyle="Solid" BorderWidth="1" CellSpacing="1" BorderColor="#E7E7E7" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridObjectDataSource"
                                                OnRowDataBound="grdcreditcard_RowDataBound" OnRowCommand="grdcreditcard_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="15%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdncreditcardid" runat="server" Value='<%#Eval("CardTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Credit Card Type" HeaderStyle-Width="25%">
                                                        <HeaderTemplate>
                                                            Credit Card Type
                                                            <asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="CardType"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcreditcard" runat="server" Text='<%# bind("CardType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" HeaderStyle-Width="25%">
                                                        <HeaderTemplate>
                                                            Store Name
                                                            <asp:ImageButton ID="lbstore" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbstorename" runat="server" Text='<%# bind("StoreName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                            <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Operations" HeaderStyle-Width="15%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                CommandArgument='<%# Eval("CardTypeID") %>'></asp:ImageButton>
                                                            <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteCredit"
                                                                CommandArgument='<%# Eval("CardTypeID") %>' message="Are you sure want to delete current Credit Card Type ?"
                                                                OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
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
                                                    <td>
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">
                                                            Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 10%; padding-right: 0px;">
                                                        <asp:Button runat="server" ID="btndelete" ToolTip="Delete" OnClientClick='return checkCount()'
                                                            OnClick="btndelete_Click" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btndelete_Click" />
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
