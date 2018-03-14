<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexPageConfigPopup.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Settings.featureCategory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Tahoma;
            font-size: 12px;
        }
    </style>
</head>
<body style="background: none">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <div id="dvCategory" runat="server" style="display: none;">
        <table style="width: 100%" cellpadding="2" cellspacing="2" class="content-table border-td">
            <tr>
                <td style="width: 10%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtsearch" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 30%">
                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="~/App_Themes/Gray/images/search.gif"
                        OnClick="ibtnSearch_Click" OnClientClick="return validation();" />&nbsp;
                    <asp:ImageButton ID="ibtnShowall" runat="server" ImageUrl="~/App_Themes/Gray/images/showall.png"
                        OnClick="ibtnShowall_Click" />
                </td>
                <td style="width: 40%" align="center">
                    <asp:ImageButton ID="ibtnAddtoselect" runat="server" ImageUrl="~/App_Themes/Gray/images/add-to-selection-list.png"
                        OnClick="ibtnAddtoselect_Click" OnClientClick="return checkCount();" />&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="~/App_Themes/Gray/images/pclose.png"
                        OnClientClick="return closeWin();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    (Max. 6 Category Select as Feature Category)
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="grdCategory" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="grdCategory_PageIndexChanging"
                        PageSize="20" OnRowDataBound="grdCategory_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                <strong>Select</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="False" ToolTip='<%#Eval("CategoryID") %>'
                                        Checked='<%# bool.Parse(Eval("IsFeatured").ToString() == "True" ? "True": "False") %>' />
                                    <asp:HiddenField ID="hdnCategoryid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Category Name">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parent Category Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblParent" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>
                    <asp:HiddenField ID="hiddenCatIDs" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dvProduct" runat="server">
        <table style="width: 100%" cellpadding="3" cellspacing="3" class="content-table border-td">
            <tr>
                <td style="width: 10%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtFeaturesystem" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 30%">
                    <asp:ImageButton ID="ibtnFeaturesystemsearch" runat="server" ImageUrl="~/App_Themes/Gray/images/search.gif"
                        OnClientClick="return fvalidation();" OnClick="ibtnFeaturesystemsearch_Click" />&nbsp;
                    <asp:ImageButton ID="ibtnfeaturesystemshowall" runat="server" ImageUrl="~/App_Themes/Gray/images/showall.png"
                        OnClick="ibtnfeaturesystemshowall_Click" />
                </td>
                <td style="width: 40%" align="center">
                    <asp:ImageButton ID="ibtnFeaturesystemaddtoselectionlist" runat="server" ImageUrl="~/App_Themes/Gray/images/add-to-selection-list.png"
                        OnClientClick="return fcheckCount();" OnClick="ibtnFeaturesystemaddtoselectionlist_Click" />&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnFeaturesystemclose" runat="server" ImageUrl="~/App_Themes/Gray/images/pclose.png"
                        OnClientClick="return closeWin();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    (Max. 18 Product(s) to Select)
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblProducterror" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <input type="hidden" id="hdnTotFeaturecnt1" runat="server" value="0" />
                    <asp:GridView ID="grdFeaturesystem" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="false" PageSize="20"
                        OnPageIndexChanging="grdFeaturesystem_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                <strong>Select</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%#Eval("SKU") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <div id="dvBestseller" runat="server">
        <table style="width: 100%" cellpadding="3" cellspacing="3" class="content-table border-td">
            <tr>
                <td style="width: 10%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtBestkey" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 30%">
                    <asp:ImageButton ID="ibtnbestSearch" runat="server" ImageUrl="~/App_Themes/Gray/images/search.gif"
                        OnClientClick="return bvalidation();" OnClick="ibtnbestSearch_Click" />&nbsp;
                    <asp:ImageButton ID="ibtnbestshowall" runat="server" ImageUrl="~/App_Themes/Gray/images/showall.png"
                        OnClick="ibtnbestshowall_Click" />
                </td>
                <td style="width: 40%" align="center">
                    <asp:ImageButton ID="ibtnBestAddtoselection" runat="server" ImageUrl="~/App_Themes/Gray/images/add-to-selection-list.png"
                        OnClientClick="return bcheckCount();" OnClick="ibtnBestAddtoselection_Click" />&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnbestclose" runat="server" ImageUrl="~/App_Themes/Gray/images/pclose.png"
                        OnClientClick="return closeWin();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    (Max. 12 Product Select)
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblBesterror" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="grdBestproduct" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="True" PageSize="20" OnPageIndexChanging="grdBestproduct_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                <strong>Select</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("IsBestSeller").ToString() == "True" ? "True": "False") %>' />
                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%#Eval("SKU") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    <div id="dvNewArrival" runat="server">
        <table style="width: 100%" cellpadding="3" cellspacing="3" class="content-table border-td">
            <tr>
                <td style="width: 10%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtNewarrival" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 30%">
                    <asp:ImageButton ID="ibtnNewarrivalsearch" runat="server" ImageUrl="~/App_Themes/Gray/images/search.gif"
                        OnClientClick="return nvalidation();" OnClick="ibtnNewarrivalsearch_Click" />&nbsp;
                    <asp:ImageButton ID="ibtnNewarrivalshow" runat="server" ImageUrl="~/App_Themes/Gray/images/showall.png"
                        OnClick="ibtnNewarrivalshow_Click" />
                </td>
                <td style="width: 40%" align="center">
                    <asp:ImageButton ID="ibtnNewaddtoselection" runat="server" ImageUrl="~/App_Themes/Gray/images/add-to-selection-list.png"
                        OnClientClick="return ncheckCount();" OnClick="ibtnNewaddtoselection_Click" />&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnNewarrivalclose" runat="server" ImageUrl="~/App_Themes/Gray/images/pclose.png"
                        OnClientClick="return closeWin();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblNewarrivalerror" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="grdNewArrival" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="True" OnPageIndexChanging="grdNewArrival_PageIndexChanging"
                        PageSize="20">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                <strong>Select</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("IsNewArrival").ToString() == "True" ? "True": "False") %>' />
                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%#Eval("SKU") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
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
                $(document).ready(function () { jAlert('Check at least One Record!', 'Message'); });
                return false;
            }
            else if (count > 6) {
                $(document).ready(function () { jAlert('Check Max. Six Record!', 'Message'); });
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function validation() {
            var a = document.getElementById('<%=txtsearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Category!', 'Message'); });
                return false;
            }
            return true;
        }
        function closeWin() {
            window.close();
        }
    </script>
    <script type="text/javascript" language="javascript">
        function fvalidation() {
            var a = document.getElementById('<%=txtFeaturesystem.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message', '<%=txtFeaturesystem.ClientID %>'); });
                return false;
            }
            return true;
        }
         
    </script>
    <script type="text/javascript" language="javascript">
        function bvalidation() {
            //debugger;
            var a = document.getElementById('<%=txtNewarrival.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message', '<%=txtNewarrival.ClientID %>'); });
                return false;
            }
            return true;
        }
         
    </script>
    <script type="text/javascript" language="javascript">
        function nvalidation() {
            var a = document.getElementById('<%=txtBestkey.ClientID %>').value;
            if (a == "") {
                //$(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message'); });
                alert('Enter Keyword to Search Product!');
                return false;
            }
            return true;
        }
         
    </script>
    <script language="javascript" type="text/javascript">
        function fcheckCount() {

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
                $(document).ready(function () { jAlert('Check at least One Record!', 'Message'); });
                return false;
            }
            else if (count > 18) {
                $(document).ready(function () { jAlert('You can not select more than 18 Product(s)!', 'Message'); });
                return false;
            }
            CntTotalFeature();
            return true;
        }

        function CntTotalFeature() {
            var fcnt = window.opener.document.getElementById('ContentPlaceHolder1_hdnTotFeaturecnt').value;
            if (parseInt(fcnt) > 0) {
                document.getElementById('hdnTotFeaturecnt1').value = fcnt;
            }
        }

    </script>
    <script language="javascript" type="text/javascript">
        function bcheckCount() {

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
                $(document).ready(function () { jAlert('Check at least One Record!', 'Message'); });
                return false;
            }
            else if (count > 12) {
                $(document).ready(function () { jAlert('Check Max. Twelve Record!', 'Message'); });
                return false;
            }
            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function ncheckCount() {

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
                $(document).ready(function () { jAlert('Check at least One Record!', 'Message'); });
                return false;
            }

            return true;
        }
    </script>
    </form>
</body>
</html>
