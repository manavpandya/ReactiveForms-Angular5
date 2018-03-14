<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="NewsLetterSubscription.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Content.NewsLetterSubscription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <script language="javascript" type="text/javascript">
        function ShowModelCredit() {
          
            document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '500px';
            document.getElementById('frmdisplay1').width = '509px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 0px; padding: 0px;width:510px;height:217px;");
            document.getElementById('popupContact1').style.width = '510px';
            document.getElementById('popupContact1').style.height = '217px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = '/ADMIN/customers/ExportCustomerVarification.aspx';
        }
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
                $(document).ready(function () { jAlert('Select at least one News Subscription!', 'Message'); });
                return false;
            }
            else {
                return ConfirmDelete();
            }
        }

        function ConfirmDelete() {
            jConfirm('Are you sure want to delete all selected News Subscription(s) ?', 'Confirmation', function (r) {
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
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-top: 5px; text-align: center; float: left;">
                    <table>
                        <tr>
                            <td style="width: 50%" align="left">
                                Store :&nbsp;
                                <asp:DropDownList ID="ddlStore" runat="server" Width="180px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="order-list">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span>
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
                                                    <img class="img-left" title="News Letter Subscription" alt="News Letter Subscription" src="/App_Themes/<%=Page.Theme %>/images/topic-list-icon.png">
                                                    <h2>
                                                        News Letter Subscription</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">
                                                <span class="star" style="vertical-align: middle; text-align: center; padding-right: 20%;">
                                                    <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.NewsSubscribtionComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreID" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtSearch" DbType="String" Name="SearchValue" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="right" style="width: 50%; padding-right: 0px;">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-right: 0px; width: 91%" valign="middle" align="right">
                                                                        Search :
                                                                        <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 3%;">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                                    </td>
                                                                    <td style="width: 3%;">
                                                                        <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                                    </td>
                                                                    <td style="width: 3%;">
                                                                        <asp:Button ID="btnExport" runat="server" ToolTip="Export" Style="background: url(/App_Themes/<%= Page.Theme.ToString()%>/images/export.gif) no-repeat transparent;
                                                                            width: 67px; height: 23px; border: none; cursor: pointer;" OnClientClick="ShowModelCredit();return false;" />
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
                                                <asp:GridView ID="grdNewsSubScription" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    DataSourceID="_gridObjectDataSource" OnRowDataBound="grdNewsSubScription_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnNewsSubscriptionID" runat="server" Value='<%#Eval("NewsSubscriptionID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" Width="7%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Email ID
                                                                <asp:ImageButton ID="btnEmailID" runat="server" CommandArgument="DESC" CommandName="Email"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Store Name">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Store Name
                                                                <asp:ImageButton ID="btnStoreName" runat="server" CommandArgument="DESC" CommandName="StoreName"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblConfigStoreName" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Created On">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Bind("CreatedOn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="16%" />
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
                                                        <asp:Button ID="btnDelete" runat="server" OnClientClick="return checkCount();" OnClick="btnDelete_Click"
                                                            CommandName="DeleteMultiple" />
                                                        <div style="display: none">
                                                            <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" />
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
         <div style="display: none">
        <input type="button" id="btnreadmore" />
        <input type="button" id="btnhelpdescri" />
        <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
            ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
            <asp:Button ID="btntemp" runat="server" OnClick="btnExport_Click" />
    </div>
    </div>
      <div id="popupContact1" style="z-index: 1000001; width: 750px; height: 350px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="350px" width="750" scrolling="auto">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
