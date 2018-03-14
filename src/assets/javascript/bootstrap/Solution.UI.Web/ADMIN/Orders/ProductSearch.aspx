<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductSearch.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.ProductSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Search</title>
    <link href="../../App_Themes/gray/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
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
        
    </script>
    <script type="text/javascript">
        function chkselect(ids) {
            var allElts = document.getElementById("rdolist").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.id == ids) {

                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }

            return true;

        }
        function chkselect1(ids) {
            var allElts = document.getElementById("Div1").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.id == ids) {

                        elt.checked = true;
                    }
                    else {
                        elt.checked = false;
                    }
                }
            }

            return true;

        }
        function chkselectProduct() {
            var allElts = document.getElementById("rdolist").getElementsByTagName('input');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "radio") {
                    if (elt.checked == true) {

                        Chktrue++;
                    }

                }
            }

            if (Chktrue == 0) {
                alert('Please select product')
                return false;
            }
            return true;

        }
        function checkSearch() {

            if (document.getElementById("txtSearch").value == '') {
                alert('Please enter search keyword');
                document.getElementById("txtSearch").focus();
                return false;

            }
            return true;
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
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
                    <td colspan="2" style="padding-top: 5px;">
                    </td>
                </tr>
                <tr>
                    <th colspan="2">
                        <div class="main-title-left" style="color: #fff; text-align: left;">
                            Search Product(s)
                        </div>
                        <%--  <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>--%>
                    </th>
                </tr>
                <tr>
                    <td>
                        <div class="slidingDivImage" style="padding-top: 1px">
                            <table width="100%" style="width: 100%">
                                <tr>
                                    <td colspan="2" style="width: 100%">
                                        <table width="100%" cellspacing="0" cellpadding="0" class="table_border" border="0"
                                            bgcolor="#ffffff">
                                            <tbody>
                                                <tr>
                                                    <td class="border">
                                                        <div class="title">
                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                                <tbody>
                                                                    <tr>
                                                                        <td valign="top" align="left" class="order_bg">
                                                                            <div class="multiple_updates">
                                                                            </div>
                                                                            <div class="order_list_bg">
                                                                                <asp:Label ID="lblTitle" runat="server" Style="color: #ffffff; height: 21px;">Search Product</asp:Label>
                                                                                <asp:LinkButton ID="btnAddVariantProduct" runat="server" ForeColor="White" OnClick="btnAddVariantProduct_Click"
                                                                                    Style="float: right;" Text="Add Option Product"></asp:LinkButton>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%" class="content-table">
                                                            <tr>
                                                                <td align="left" colspan="3" style="padding-left: 20px">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="left" height="30" valign="middle">
                                                                                <span style="color: #ff0033; vertical-align: top;"></span>Search By :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list " Width="150px">
                                                                                    <asp:ListItem Value="Name" Selected="True">Product Name</asp:ListItem>
                                                                                    <asp:ListItem Value="SKU">SKU</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtSearch" runat="server" Style="width: 280px; vertical-align: top;
                                                                                    height: 19px; border: 1px solid #BCC0C1;"></asp:TextBox>&nbsp;
                                                                                <asp:ImageButton ID="btnGo" runat="server" OnClientClick="return checkSearch();"
                                                                                    OnClick="btnGo_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr class="table_bg">
                                                                <td align="center" colspan="3" valign="middle" style="text-align: center; height: 19px;
                                                                    color: red">
                                                                    <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <div id="rdolist" style="border: 1px;">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Panel Width="100%" ID="pnlAddpro" Visible="false" runat="server">
                                                                                        <table width="100%" cellpadding="0" border="0" cellspacing="0" class="table">
                                                                                            <tr style="background-color: rgb(231, 231, 231);">
                                                                                                <td colspan="2" style="text-align: left;">
                                                                                                    Add Option Product
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    Product Name :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox Style="vertical-align: top; border: 1px solid #BCC0C1;" ID="txtProductName"
                                                                                                        runat="server" Height="19px" Width="407px" MaxLength="200"></asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="rfvproname" runat="server" ControlToValidate="txtProductName"
                                                                                                        ErrorMessage="*" ValidationGroup="Product"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    Price :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="width: 120px;
                                                                                                        vertical-align: top; height: 19px; border: 1px solid #BCC0C1;" ID="txtprice"
                                                                                                        runat="server"></asp:TextBox>
                                                                                                    <asp:RequiredFieldValidator ID="rfvprice" runat="server" ControlToValidate="txtprice"
                                                                                                        ErrorMessage="*" ValidationGroup="Product"></asp:RequiredFieldValidator>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    SKU :
                                                                                                </td>
                                                                                                <td>
                                                                                                    RSC-<%=Request.QueryString["VVID"].ToString() %>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="display: none;">
                                                                                                <td>
                                                                                                    Sale Price :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="width: 120px;
                                                                                                        vertical-align: top; height: 19px; border: 1px solid #BCC0C1;" ID="txtsaleprice"
                                                                                                        runat="server"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="display: none;">
                                                                                                <td>
                                                                                                    Inventory :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="width: 120px;
                                                                                                        vertical-align: top; height: 19px; border: 1px solid #BCC0C1;" ID="txtInventory"
                                                                                                        runat="server"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="display: none;">
                                                                                                <td>
                                                                                                    Weight :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="width: 120px;
                                                                                                        vertical-align: top; height: 19px; border: 1px solid #BCC0C1;" ID="txtweight"
                                                                                                        runat="server"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="display: none;">
                                                                                                <td>
                                                                                                    Status :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:CheckBox ID="chkstatus" runat="server" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr style="display: none;">
                                                                                                <td>
                                                                                                    Description :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox TextMode="multiLine" ID="txtDescription" Rows="10" Columns="80" runat="server"></asp:TextBox>
                                                                                                    <script type="text/javascript">
                                                                                                        CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                                                                        CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                                            if (ev.data.name == 'image') {
                                                                                                                var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                btn.hidden = false;
                                                                                                                btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                            }
                                                                                                            if (ev.data.name == 'link') {
                                                                                                                var btn = ev.data.definition.getContents('info').get('browse');
                                                                                                                btn.hidden = false;
                                                                                                                btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                                            }
                                                                                                        });
                                                                                                    </script>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="center">
                                                                                                <td colspan="2" align="center" style="text-align: center;">
                                                                                                    <asp:ImageButton Height="29px" Width="87px" ID="btnVarProsave" runat="server" ValidationGroup="Product"
                                                                                                        OnClick="ImageButton2_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" />
                                                                                                    <asp:ImageButton Height="29px" Width="87px" ID="btnupdate" runat="server" ValidationGroup="Product"
                                                                                                        OnClick="btnupdate_Click" Visible="false" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" />
                                                                                                    <asp:ImageButton Height="29px" Width="87px" ID="btnCancel" runat="server" CssClass="button"
                                                                                                        CausesValidation="False" OnClick="btnCancel_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/images/cancel.gif" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="padding-bottom: 20px;">
                                                                                    <asp:GridView ID="GVVariantProduct" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                        BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                                                        AllowPaging="true" PageSize="10" OnPageIndexChanging="GVVariantProduct_PageIndexChanging"
                                                                                        BorderStyle="Solid">
                                                                                        <FooterStyle CssClass="content-table" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Select</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <input type="radio" name="rdchecknm" id="rdcheckid" runat="server" onclick="return chkselect(this.id);"
                                                                                                        enableviewstate="true" />
                                                                                                    <asp:Label ID="ProductId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                        Visible="false"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Product Name</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblPOName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">SKU</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblVName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Inventory</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblACost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Price</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblPrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="center" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                                                        <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="gvOldPOrder" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                        BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                                                        GridLines="None" AllowPaging="true" PageSize="10" OnPageIndexChanging="gvOldPOrder_PageIndexChanging">
                                                                                        <FooterStyle CssClass="content-table" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Select</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <input type="radio" name="rdchecknm" id="rdcheckid" runat="server" onclick="return chkselect(this.id);"
                                                                                                        enableviewstate="true" />
                                                                                                    <asp:Label ID="ProductId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'
                                                                                                        Visible="false"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="Center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Product Name</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblPOName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" ForeColor="#fff" Height="15px" HorizontalAlign="left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">SKU</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblVName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="left" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Inventory</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblACost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="center" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderTemplate>
                                                                                                    <strong style="color: #fff;">Price</strong>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <%--<asp:Label ID="lblPrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>--%>
                                                                                                    <asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("${0:0.00}", Eval("Price"))%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                                <HeaderStyle BackColor="#e7e7e7" Height="15px" HorizontalAlign="Right" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                        <%--  <HeaderStyle CssClass="list-table-title" BackColor="#F2F2F2" Height="30px" Font-Bold="true" />
                                                            <HeaderStyle BackColor="#F2F2F2" ForeColor="#384557" />
                                                            <PagerStyle HorizontalAlign="Right" />
                                                            <PagerSettings Mode="Numeric" Position="TopAndBottom" />--%>
                                                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                                                        <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                                                        <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                        <%--  <HeaderStyle BackColor="#e7e7e7" Width="56px" Height="40px" Font-Bold="true" />
                                                                                        <AlternatingRowStyle CssClass="gridalt" />
                                                                                        <PagerStyle CssClass="paging" HorizontalAlign="Right" BackColor="#f3f3f3" />
                                                                                        <PagerSettings Position="TopAndBottom" Mode="Numeric" />--%>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="4" style="padding-top: 20px">
                                                                    <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return chkselectProduct();"
                                                                        Visible="false" OnClick="btnSave_Click" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" />
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div>
    </div>
    </form>
</body>
</html>
