<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateProductBrowser.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.UpdateProductBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Browser</title>
    <link href="../../App_Themes/gray/css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

        function onKeyPressBlockNumbersWithoutDot(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 13 || key == 8 || key == 9 || key == 189 || key == 0) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

        function chkselect() {
            var allElts = document.forms[0].elements;
            var i;
            var Chktrue;
            Chktrue = 0;
            var addText = '';

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        addText += '::' + elt.parentNode.title + ',';
                        Chktrue = Chktrue + 1;

                        var qtyBox = document.getElementById('qty' + elt.parentNode.title)
                        var txtPrice = document.getElementById('txtprice' + elt.parentNode.title)
                        var txtWeight = document.getElementById('txtweight' + elt.parentNode.title)
                        qtyBox.style.backgroundColor = '';

                        if (qtyBox.value * 1 == 0 || isNaN(qtyBox.value * 1)) {
                            qtyBox.style.backgroundColor = 'yellow';
                            qtyBox.value = '1';
                            alert('Please enter atleast 1 Quantity!');
                            qtyBox.focus();
                            return false;
                        }

                        addText += qtyBox.value + ',';
                        txtPrice.style.backgroundColor = '';

                        if (txtPrice.value <= 0 || isNaN(txtPrice.value)) {
                            txtPrice.style.backgroundColor = 'yellow';
                            txtPrice.value = '1';
                            alert('Please enter Price!');
                            txtPrice.focus();
                            return false;
                        }

                        addText += txtPrice.value + ',';
                        addText += txtWeight.value + ',';
                    }
                }
            }

            if (Chktrue < 1) {
                alert("Please select atleast one Product!");
                return false;
            }
            else {
                if (window.opener.document.getElementById('hfAddToCart')) {
                    window.opener.document.getElementById('hfAddToCart') = addText;
                    window.opener.focus();
                    window.opener.submit();
                    window.close();
                }
                else if (document.getElementById('hfAddToCart')) {
                    document.getElementById('hfAddToCart').value = addText;
                }
            }
            return true;
        }
    
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table"
        style="padding: 2px;">
        <tbody>
            <tr>
                <td>
                    <img id="imgLogo" runat="server" />
                </td>
                <td style="text-align: right;">
                    <img id="imgMainDiv" runat="server" onclick="javascript:var result=window.opener.location.href;result =result.replace('potab=1','');result =result+'&potab=1'; window.opener.location.href=result; window.close();"
                        class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png"
                        style="cursor: pointer" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px;" colspan="2">
                </td>
            </tr>
            <tr>
                <th colspan="2">
                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;
                        height: 21px;">
                        Search Product(s)
                    </div>
                    <%--  <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>--%>
                </th>
            </tr>
        </tbody>
    </table>
    <table border="0" cellspacing="0" cellpadding="5" width="100%">
        <tr>
            <td class="font" align="left" style="width: 200px; padding-top: 8px;" valign="top">
                Model Number (SKU) / Name :
            </td>
            <td align="left" valign="top">
                <asp:TextBox Style="vertical-align: top; margin: 3px; border: 1px solid #BCC0C1;"
                    ID="txtProduct" runat="server" CssClass="textfield_small" Width="350"></asp:TextBox>
                <%-- <asp:Button CssClass="button" ID="btnBrowse"  runat="server" Text="Search Product"
                    OnClick="btnBrowse_Click" ValidationGroup="Search" />--%>
                <asp:ImageButton ID="btnBrowse" runat="server" Text="Search Product" OnClick="btnBrowse_Click"
                    ValidationGroup="Search" /><br />
                <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="txtProduct"
                    Display="Dynamic" ErrorMessage="Please enter Model Number (SKU) / Name!" SetFocusOnError="true"
                    ValidationGroup="Search"></asp:RequiredFieldValidator>
                <asp:ValidationSummary ID="valSearch" runat="server" ShowMessageBox="true" ShowSummary="false"
                    ValidationGroup="Search" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblMsg" runat="Server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" id="containerSearch" runat="server">
                <table>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:HiddenField ID="hfAddToCart" runat="server" />
                            <asp:HiddenField ID="hfOrderNumber" runat="server" />
                            <asp:HiddenField ID="hfStoreID" runat="server" />
                            <%--   <asp:Button ID="btnAddtoCart1" runat="server" CssClass="button" Text="Add To Cart"
                                Visible="false" OnClick="btnAddtoCart_Click" OnClientClick="return chkselect();" />--%>
                            <asp:ImageButton ID="btnAddtoCart1" runat="server" Visible="false" OnClick="btnAddtoCart_Click"
                                OnClientClick="return chkselect();" />
                        </td>
                    </tr>
                    <tr>
                        <td height="20" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdProducts" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                BorderColor="#E7E7E7" CssClass="content-table" BorderWidth="1px" CellPadding="2"
                                CellSpacing="1" GridLines="None" EmptyDataRowStyle-ForeColor="red" EmptyDataRowStyle-Font-Size="12px"
                                EmptyDataText="No Result(s) found! Try with another Keyword(s)!" Width="100%"
                                AllowPaging="false">
                                <FooterStyle CssClass="content-table" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <strong style="color: #fff;">Select</strong>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" ToolTip='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="5%" />
                                        <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Product&nbsp;Name
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="35%" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem,"Name") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;SKU
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="20%" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem,"SKU") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Weight
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <label>
                                                <%# DataBinder.Eval(Container.DataItem, "Weight")%>
                                            </label>
                                            <span style="display: none">
                                                <%# BindWeight(DataBinder.Eval(Container.DataItem, "Weight").ToString(), DataBinder.Eval(Container.DataItem, "ProductID").ToString())%>
                                            </span>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Inventory
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="center" />
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" Width="12%" />
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem,"Inventory") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Quantity
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10%" />
                                        <ItemTemplate>
                                            <%# BindQuantity(DataBinder.Eval(Container.DataItem, "ProductID").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Price
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10%" />
                                        <ItemTemplate>
                                            <%# BindPrice(DataBinder.Eval(Container.DataItem, "SalePrice").ToString(), DataBinder.Eval(Container.DataItem, "ProductID").ToString())%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <%--    <RowStyle BackColor="White" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                                    CssClass="list_table_cell_link" Height="24px" HorizontalAlign="Center" />
                                <EditRowStyle CssClass="list_table_cell_link" />
                                <PagerStyle CssClass="list_table_cell_link" HorizontalAlign="Right" />
                                <HeaderStyle BackColor="#F2F2F2" CssClass="list-table-title" Height="30px" HorizontalAlign="Center"
                                    Wrap="True" />
                                <AlternatingRowStyle BackColor="#F2F2F2" BorderColor="White" BorderStyle="Solid"
                                    CssClass="list_table_cell_link" ForeColor="#444545" />
                                <PagerSettings Position="TopAndBottom" />--%>
                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td height="20" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <%--     <asp:Button ID="btnAddtoCart2" runat="server" CssClass="button" Text="Add To Cart"
                                Visible="false" OnClick="btnAddtoCart_Click" OnClientClick="return chkselect();" />--%>
                            <asp:ImageButton ID="btnAddtoCart2" runat="server" Visible="false" OnClick="btnAddtoCart_Click"
                                OnClientClick="return chkselect();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
