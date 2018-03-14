<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="QuotedPurchaseOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.QuotedPurchaseOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function checkvendor() {
            if (document.getElementById('ContentPlaceHolder1_ddlVendor') && document.getElementById('ContentPlaceHolder1_ddlVendor').options[document.getElementById('ContentPlaceHolder1_ddlVendor').selectedIndex].value == 0) {
                alert("Please select Vendor for Preview.");
                document.getElementById('ContentPlaceHolder1_ddlVendor').focus();
                return false;
            }
            return true;
        }
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);

            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;

            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 104)
                return true;
            return false;
        }

        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                                        style="padding: 2px;">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 0px;">
                                                        Generate Warehouse Purchase Order
                                                    </div>
                                                    <div class="main-title-right">
                                                        &nbsp;
                                                    </div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; text-align: right;">
                                                    <a href="WareHousePO.aspx" style="margin-right: 30px;">
                                                        <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Warehouse PO"
                                                            title="Go to Warehouse PO" />
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px">
                                                    <div id="rdolist" style="border: 5px sollid #e7e7e7;">
                                                        <asp:GridView ID="grdCart" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            class="order-table" Style="border: solid 1px #e7e7e7">
                                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                                            <EmptyDataTemplate>
                                                                <label style="color: Red; text-align: center;">
                                                                    No Record(s) Found !</label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        <asp:Label ID="lblProductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                        <asp:Label ID="lblProductOption" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductOption") %>'></asp:Label>
                                                                        <asp:Label ID="lblVendorQuoteID" runat="server" Text='<%#Eval("RequestQuoteId") %>'></asp:Label>
                                                                        <asp:Label ID="lblVendorQuoteReqDetailsID" runat="server" Text='<%#Eval("RequestQuoteDetailId") %>'></asp:Label>
                                                                        <asp:Label ID="lblQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderTemplate>
                                                                        &nbsp;Name
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="50%" HorizontalAlign="left" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderTemplate>
                                                                        &nbsp; SKU
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <ItemStyle />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQuantity" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'
                                                                            runat="server" Width="70px" Visible="false" Style="text-align: center" MaxLength="6"
                                                                            onkeypress="return keyRestrict(event,'0123456789');" class="order-textfield"></asp:TextBox>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price">
                                                                    <HeaderTemplate>
                                                                        Price&nbsp;
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        $<%# string.Format("{0:F}", DataBinder.Eval(Container.DataItem, "Price"))%>
                                                                        <asp:Label ID="lblPrice" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="10%" HorizontalAlign="right" />
                                                                    <ItemStyle HorizontalAlign="right" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                            <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                                                    <table cellpadding="1" cellspacing="2" width="100%">
                                                        <tr>
                                                            <td style="width: 100px;">
                                                                Vendor :
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="order-list" Width="250px"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblmailtemplate" runat="server" Text="Mail Template :"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMailTemplate" runat="server" CssClass="order-list" Width="250px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblspeicalinstructions" runat="server" Text="Special Instructions:"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td class="ckeditor-table">
                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                Rows="10" Columns="60" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblLinks" runat="server" CssClass="error" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-bottom: 10px;">
                                                                <asp:ImageButton ID="btnPreview" runat="server" OnClientClick="return checkvendor();"
                                                                    OnClick="btnPreview_Click" />
                                                                <asp:ImageButton ID="btnGeneratePDFPO" runat="server" AlternateText="Generate Purchase Order"
                                                                    OnClick="btnGeneratePDFPO_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblstaticpdfcollection" runat="server" Text="PDF Files"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
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
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
