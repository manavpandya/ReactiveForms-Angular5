<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="LatestProductInquiryList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.LatestProductInquiryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">
        $(function () {

            $('#ContentPlaceHolder1_txtDateFrom').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
        $(function () {

            $('#ContentPlaceHolder1_txtDateTo').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtDateFrom.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtDateFrom.ClientID %>'); });
                return false;
            }
            a = document.getElementById('<%=txtDateTo.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtDateTo.ClientID %>'); });
                return false;
            }

            var startDate = new Date(document.getElementById('<%=txtDateFrom.ClientID %>').value);
            var endDate = new Date(document.getElementById('<%=txtDateTo.ClientID %>').value);
            if (startDate > endDate) {
                jAlert("Please enter 'Date To' grater than 'Date From' ", "Message", "<%=txtDateTo.ClientID %>");
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
    <%--<script language="javascript" type="text/javascript">
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
                $(document).ready(function () { jAlert('Check at least one Product Inquiry!', 'Message', ''); });
                return false;
            }
            else {
                return DeleteConfirm();
            }
            return false;
        }
        function DeleteConfirm() {
            jConfirm('Are you sure want to delete all selected Product Inquiries ?', 'Confirmation', function (r) {
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
    </script>--%>
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
                $(document).ready(function () { jAlert('Check at least one Product Inquiry!', 'Message', ''); });
                return false;
            }
            else {
                return DeleteConfirm();
            }
            return false;
        }
        function DeleteConfirm() {
            jConfirm('Are you sure want to delete all selected Product Inquiries ?', 'Confirmation', function (r) {
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
        function checkCountSendMail() {
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
                $(document).ready(function () { jAlert('Check at least one Product Inquiry!', 'Message', ''); });
                return false;
            }
            return true;
        }
    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
                <span style="vertical-align: middle; margin-top: 4px; margin-right: 3px; float: left;">
                    <table>
                        <tr>
                            <td align="right">
                                Store :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStore" runat="server" Width="175px" AutoPostBack="true"
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
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Latest Product Inquiry" alt="Latest Product Inquiry"
                                                        src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                                                    <h2>
                                                        Latest Product Inquiry</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                            </td>
                                            <td style="padding-right: 0px;" align="right">
                                                <span class="star" style="vertical-align: middle; text-align: center; padding-right: 20%;">
                                                    <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                                                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                    TypeName="Solution.Bussines.Components.ConfigurationComponent" SelectMethod="GetDataByFilter"
                                                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                    SelectCountMethod="GetCount">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlStore" DbType="Int32" Name="StoreId" DefaultValue="" />
                                                        <asp:ControlParameter ControlID="txtDateFrom" DbType="String" Name="SearchBy" />
                                                        <asp:ControlParameter ControlID="txtDateTo" DbType="String" Name="SearchValue" DefaultValue="" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                                <table align="right" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td valign="middle" align="right">
                                                            From Date:
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="from-textfield" Width="70px"
                                                                Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td valign="middle" align="right">
                                                            To Date:&nbsp;
                                                            <asp:TextBox ID="txtDateTo" ReadOnly="false" runat="server" CssClass="from-textfield"
                                                                Width="70px" Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                        <td align="left" style="width: 50px;">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return validation();" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnShowall" runat="server" OnClick="btnShowall_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="2">
                                                <asp:GridView ID="grdLatestProductInquiry" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                                                    OnPageIndexChanging="grdLatestProductInquiry_PageIndexChanging" OnRowCommand="grdLatestProductInquiry_RowCommand"
                                                    OnRowDataBound="grdLatestProductInquiry_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAvailabilityID" runat="server" Text='<%#Eval("AvailabilityID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-VerticalAlign="Middle">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <HeaderStyle />
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" />
                                                                <asp:HiddenField ID="hdnCustomerName" runat="server" Value='<%#Eval("CustomerName") %>' />
                                                                <asp:HiddenField ID="hdnProductID" runat="server" Value='<%#Eval("ProductID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle VerticalAlign="Top" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Name" SortExpression="ConfigName" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                SKU
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblSKU" runat="server" Text='<%# Bind("SKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                                Subject
                                                                <asp:ImageButton ID="btnSubject" runat="server" CommandArgument="DESC" CommandName="subject"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubject" runat="server" Text='<%# Bind("subject") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Email" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <HeaderTemplate>
                                                                Email
                                                                <asp:ImageButton ID="btnEmail" runat="server" CommandArgument="DESC" CommandName="Email"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IPAddress">
                                                            <HeaderTemplate>
                                                                IPAddress
                                                                <asp:ImageButton ID="btnIpAddress" runat="server" CommandArgument="DESC" CommandName="IPAddress"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIpAddress" runat="server" Text='<%# Bind("IPAddress") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SentOn" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                SentOn
                                                                <asp:ImageButton ID="btnSentOn" runat="server" CommandArgument="DESC" CommandName="mailDate"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSentOn" runat="server" Text='<%# Bind("mailDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Inventory" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                Inventory
                                                                <asp:ImageButton ID="btnInventory" runat="server" CommandArgument="DESC" CommandName="Inventory"
                                                                    AlternateText="Ascending Order" OnClick="Sorting" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInventory" runat="server" Text='<%# Bind("Inventory") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <HeaderTemplate>
                                                                Active
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <img src="<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"MailSent"))) %>" />
                                                            </ItemTemplate>
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
                                            <td colspan="2">
                                                <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                    href="javascript:selectAll(false);">Clear All</a> </span><span style="float: right;
                                                        padding-right: 0px;"></span><span style="float: right; padding-right: 0px;">
                                                            <asp:ImageButton ID="btnSendMail" AlternateText="SendMail" runat="server" OnClick="btnSendMail_Click"
                                                                OnClientClick="return checkCountSendMail();" />
                                                            <asp:Button ID="btnDelete" runat="server" OnClientClick="return checkCount();" CommandName="DeleteMultiple"
                                                                OnClick="btnDelete_Click" /></span>
                                                <div style="display: none">
                                                    <asp:Button ID="btnDeleteTemp" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" />
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
