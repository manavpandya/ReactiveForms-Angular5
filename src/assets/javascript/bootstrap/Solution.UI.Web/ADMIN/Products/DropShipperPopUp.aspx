<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DropShipperPopUp.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.DropShipperPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none">
    <script type="text/javascript">
        function loadVendorsku() {
            var id = window.opener.document.getElementById('<%=Request.QueryString["mode"]%>').value;
            var allSKu = ',' + id + ',';
            document.getElementById('<%= txtvendorsku.ClientID %>').value = ',' + id + ',';
            var allCheckbox = document.getElementById('grdProducts').getElementsByTagName('input');
            for (var i = 0; i < allCheckbox.length; i++) {
                var allExists = allCheckbox[i];
                if (allExists.id.indexOf('_hdnVendorSKUID1_') > -1 && allSKu.indexOf(',' + allExists.value.replace(/^\s*\s*$/g, '') + ',') > -1) {
                    var checkboxid = allExists.id.replace('_hdnVendorSKUID1_', '_chkSelect_');

                    document.getElementById(checkboxid).checked = true;
                }

            }
        }
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/css/style.css"></script>
    <div id="dvProduct" runat="server">
        <table width="100%" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" class="order-table1">
            <tr>
                <td style="width: 12%">
                    Search :
                </td>
                <td style="width: 20%">
                    <asp:TextBox ID="txtFeaturesystem" CssClass="order-textfield" runat="server" Width="100%"></asp:TextBox>
                </td>
                <td style="width: 28%">
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
                <td colspan="3">
                    <asp:UpdatePanel ID="upProductTypeDelivery" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            DropShipper:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddldropshippersku" runat="server" OnSelectedIndexChanged="ddldropshippersku_SelectedIndexChanged"
                                class="product-type" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    (Max. 3 Dropshipper SKU Select)
                </td>
                <td colspan="1" align="left">
                    <asp:Label ID="lblProducterror" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:UpdatePanel ID="updategrdDropShip" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="false" Width="100%"
                                EmptyDataText="No Record Found!" EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="True"
                                PageSize="20" OnPageIndexChanging="grdProducts_PageIndexChanging" OnRowDataBound="grdProducts_RowDataBound"
                                CellPadding="2" CellSpacing="1" BorderStyle="solid" BorderWidth="1" BorderColor="#e7e7e7">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                        <HeaderTemplate>
                                            <%-- <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">--%>
                                            <strong>Select</strong>
                                            <%--</td>
                                        </tr>
                                    </table>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# bool.Parse(Eval("VendorSKUID").ToString() == "True" ? "True": "False") %>' />
                                            <asp:HiddenField ID="hdnVendorSKUID" runat="server" Value='<%#Eval("VendorSKUID") %>' />
                                            <asp:HiddenField ID="hdnVendorSKUID1" runat="server" Value='<%#Eval("VendorSKU") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product Name">
                                        <ItemTemplate>
                                            <%#Eval("ProductName")%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SKU">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"VendorSKU") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                                <PagerSettings Position="TopAndBottom" />
                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                <AlternatingRowStyle CssClass="altrow" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="display: none;">
                        <asp:TextBox ID="txtvendorsku" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        function fvalidation() {
            var a = document.getElementById('<%=txtFeaturesystem.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message', '<%=txtFeaturesystem.ClientID %>'); });
                return false;
            }
            return true;
        }
        function closeWin() {
            window.close();
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
            else if (count > 3) {
                $(document).ready(function () { jAlert('Check Max. Three Record!', 'Message'); });
                return false;
            }
            return true;
        }
    </script>
    </form>
</body>
</html>
