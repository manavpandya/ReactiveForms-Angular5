<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="StoreCreditList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.StoreCreditList" %>
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
                $(document).ready(function () { jAlert('Check at least One Store Credit Option !', 'Message'); });
                return false;
            }
            else {
                return checkaa();
            }
            return false;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Store Credit ?', 'Confirmation', function (r) {
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
    <style type="text/css">
        .style1
        {
            width: 754px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                <table style="display:none;">
                    <tr>
                        <td>
                            Store :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpstore" runat="server" Width="170px" OnSelectedIndexChanged="drpstore_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="order-list">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                href="/Admin/configuration/StoreCredit.aspx">
                <img alt="Add Store Credit" title="Add Store Credit" src="/App_Themes/<%=Page.Theme %>/images/add-store-credit.png" /></a></span>
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
                                                <img class="img-left" title="Store Credit Option" alt="Store Credit Option" src="/App_Themes/<%=Page.Theme %>/Images/payment-option-icon.png" />
                                                <h2>
                                                    Store Credit List</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <table width="100%" style="display:none;">
                                                <tr>
                                                    <td align="right" valign="middle" style="padding-right: 0px;width: 94%;">
                                                        Search&nbsp;&nbsp;:
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
                                           
                                            <asp:HiddenField ID="hdnStoreCredit" runat="server" />
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grdStoreCredit" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                CellSpacing="1" BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" PagerSettings-Mode="NumericFirstLast"
                                                CellPadding="2"  OnRowDataBound="_StoreCreditgridview_RowDataBound"
                                                OnRowCommand="_StoreCreditgridview_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="15%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdnStoreCreditid" runat="server" Value='<%#Eval("ID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Credit Type" HeaderStyle-Width="25%">
                                                        <HeaderTemplate>
                                                            StoreCredit
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbStoreCredit" runat="server" Text='<%# bind("StoreCredit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Credit Amount" HeaderStyle-Width="25%">
                                                        <HeaderTemplate>
                                                          StoreCredit Amount
                                                            
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                        <HeaderStyle HorizontalAlign="left" />
                                                        <ItemTemplate>
                                                           
                                                            <asp:Label ID="lbstorename" runat="server" Text='   <%#  Math.Round((Convert.ToDecimal(Eval("StoreCreditAmount"))),2)%>'></asp:Label>
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
                                                                CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                                                            <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteStoreCredit"
                                                                CommandArgument='<%# Eval("ID") %>' message="Are you sure want to delete current StoreCredit  ?"
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
                                                        <asp:Button ID="btndelete" runat="server" ToolTip="Delete" OnClientClick='return checkCount()'
                                                            OnClick="btndelete_Click" Width="58px" />
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
