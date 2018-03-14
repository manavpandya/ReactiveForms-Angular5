<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="HeaderTextList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.HeaderTextList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message'); });
                document.getElementById('<%=txtSearch.ClientID %>').focus();
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
                $(document).ready(function () { jAlert('Select at least one Header!', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected Header(s) ?', 'Confirmation', function (r) {
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
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 4px; float: left;display:none;">
                        <tr>
                            <td style="align="right">
                                Store :
                            </td>
                            <td >
                                <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    href="/Admin/Settings/HeaderText.aspx">
                    <img alt="Add Header" title="Add Header" src="/App_Themes/<%=Page.Theme %>/images/add-Header.png" /></a></span>
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
                                                    <img class="img-left" title="Header Text" alt="Header Text" src="/App_Themes/<%=Page.Theme %>/images/Header-list-icon.png">
                                                    <h2>
                                                        Header Text List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right:0px;">
                                                <span class="star" style="vertical-align: middle; text-align: center; padding-right: 20%;">
                                                    <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                                                
                                                <table style="width: 100%;display:none;" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 75%">
                                                        </td>
                                                        <td style="width: 10%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td style="width: 5%; padding-right:0px;">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="grdHeader" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                    AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                                    CellPadding="2" CellSpacing="1" GridLines="None" Width="100%" AllowPaging="True"
                                                    
                                                     OnRowCommand="grdHeader_RowCommand" OnRowDataBound="grdHeader_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                                <strong>Select </strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnHeaderID" runat="server" Value='<%#Eval("HeaderID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" Width="7%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Header Text
                                                                <%--<asp:ImageButton ID="btnHeaderName" runat="server" CommandArgument="DESC" CommandName="HeaderName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblHeaderText" runat="server" Text='<%# Bind("HeaderText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Active/In-Active">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Active/In-Active
                                                               <%-- <asp:ImageButton ID="btnStoreName" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActive" runat="server" Text='<%# Bind("Active") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DisplayOrder">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDisplayOrder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="16%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                    CommandArgument='<%# Eval("HeaderID") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow" runat="server" id="trBottom">
                                            <td>
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                        padding-right: 0px;">
                                                        <asp:Button ID="btnDeleteHeader" runat="server" OnClientClick="return checkCount();"
                                                            OnClick="btnDeleteHeader_Click" CommandName="DeleteMultiple" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDeleteHeader_Click" />
                                                        </div>
                                                    </span>
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