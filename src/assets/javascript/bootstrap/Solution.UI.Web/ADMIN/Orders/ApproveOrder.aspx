<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.ApproveOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/App_Themes/<%=Page.Theme %>/css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            margin: 5px 8px 5px 8px;
            padding: 0;
            background: #fff;
            color: #000000;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
        }
        .list_table_cell_link
        {
            background-color: #fafafa;
            line-height: 20px;
        }
        .list_table_cell_link td
        {
            width: 300px;
        }
    </style>
    <script type="text/javascript">

        function ConfirmDelete(strmsg, cntrlnm) {
            jConfirmDynemicButton(strmsg, 'Confirmation', 'Yes', 'No', function (r) {
                if (r == true) {
                    document.getElementById(cntrlnm).onclick = function () { return true; };
                    //__doPostBack(cntrlnm, '');
                    document.getElementById(cntrlnm).click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
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
        function numbersonly(e) {

            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
                if (unicode < 48 || unicode > 57) //if not a number
                    return false //disable key press
            }
        }

        function OpenInventory(PID) {
            window.open('ProductSearch.aspx?upgrade=1&pid=' + PID, '', 'width=900,height=700,scrollbars=1');
        }
        function OpenInventoryForSKU(PID, SID, CID) {
            window.open('ProductSearch.aspx?SKU=1&pid=' + PID + '&sid=' + SID + '&cid=' + CID, '', 'width=900,height=700,scrollbars=1');
        }
        function OpenUpdateProductBrowser(StoreID, OrderedShoppingCartID, Ordernum) {
            window.open('UpdateProductBrowser.aspx?store=' + StoreID + '&Ono=' + Ordernum + '&OSCID=' + OrderedShoppingCartID + '', '', 'width=800,height=800,scrollbars=1');
        }
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
        <table width="100%">
            <tr>
                <td style="display: none">
                    <img id="imgLogo" runat="server" src="/Admin/Images/logo_white_bg.jpg" alt="" />
                </td>
                <td align="right">
                    <img src="/Admin/Images/back.jpg" style="cursor: pointer; display: none;" alt="Back"
                        onclick="history.back()" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="2" align="Right">
                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="/Admin/Images/update-orde.gif"
                        AlternateText="Approve Order" OnClick="btnApproveupdate_Click" />&nbsp;<asp:ImageButton
                            ID="ImageButton1" runat="server" ImageUrl="/Admin/Images/approve-order.jpg" AlternateText="Approve Order"
                            OnClick="btnApprove_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="table_border">
                        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="false" Width="100%"  
                            CssClass="table-noneforOrder" CellSpacing="5" CellPadding="5" OnRowCommand="gvProducts_RowCommand" 
                            OnRowDataBound="gvProducts_RowDataBound" >
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                        <asp:Label ID="lblCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")).ToString("f2") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblVariantname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Product Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Product Name") %>'></asp:Label>
                                        <%--  <%# BindVariant(DataBinder.Eval(Container.DataItem,"VariantNames").ToString(),DataBinder.Eval(Container.DataItem,"VariantValues").ToString()) %>--%>
                                        <div style="padding-left: 20px;">
                                            <asp:Label ID="lblAssambly" runat="server"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SKU
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>&nbsp;
                                        <asp:ImageButton ID="btnEditsku" CommandName="EditSKu" runat="server" ImageUrl="/App_Themes/Gray/Images/edit-price.gif" /><br />
                                        <asp:DropDownList ID="ddlupgradesku" CssClass="order-list" runat="server" Visible="false"></asp:DropDownList>
                                        <asp:TextBox ID="txtEditSku" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SKU") %>' Visible="false"></asp:TextBox>
                                        <asp:Label ID="lblSKUupgrade" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"SKUupgrade") %>'></asp:Label>
                                        
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Ordered
                                        <br />
                                        Quantity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Ordered Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        Available
                                        <br />
                                        Inventory
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Available Inventory") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Lock
                                        <br />
                                        Quantity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLockQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Lock Quantity") %>'
                                            Visible="false"></asp:Label>
                                        <asp:TextBox onkeypress='return onKeyPressBlockNumbers(event)' ReadOnly="true" Style="text-align: center;"
                                            Width="50px" ID="txtLockQty" MaxLength="4" CssClass="from-textfield" runat="server"
                                            Text='<%#DataBinder.Eval(Container.DataItem,"Lock Quantity") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Shipped
                                        <br />
                                        Quantity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblShippedQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Shipped Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        Upgrade
                                        <br />
                                        SKU
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpgradeSKU" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Upgrade SKU") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        Available
                                        <br />
                                        Inventory
                                        <br />
                                        for Upgrade
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpgradeInventory" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Available Upgrade Quantity") %>'></asp:Label>
                                        <!--<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Available Upgrade Quantity").ToString().Trim())? "-":"" %>-->
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        Upgrade
                                        <br />
                                        Quantity
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUpgradeQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Upgrade Lock Quantity") %>'
                                            Visible="false"></asp:Label>
                                        <%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Upgrade SKU").ToString().Trim())? "-":"" %>
                                        <asp:TextBox MaxLength="4" onkeypress="return onKeyPressBlockNumbers(event)" Style="text-align: center;"
                                            Visible='<%# !String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem,"Upgrade SKU").ToString().Trim())?true:false%>'
                                            CssClass="from-textfield" Width="50px" ID="txtUpgradeQty" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Upgrade Lock Quantity") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Price
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        $<asp:Label ID="lblUpgradePrice" runat="server" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"price")).ToString("f2") %>'></asp:Label>
                                        <asp:ImageButton ID="btnEditprice" CommandName="EditPrice" runat="server" ImageUrl="/App_Themes/Gray/Images/edit-price.gif" />
                                        <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="text-align: center;
                                           " visible="false"
                                            CssClass="from-textfield" Width="50px" ID="txtUpgradePrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"price")).ToString("f2") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="true">
                                    <HeaderTemplate>
                                        Discount Price
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        $<asp:Label ID="lblUpgradeDiscountPrice" runat="server" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"DiscountPrice")).ToString("f2") %>'></asp:Label>
                                        <asp:ImageButton ID="btnEditdiscountprice" CommandName="EditDiscountPrice" runat="server" ImageUrl="/App_Themes/Gray/Images/edit-price.gif" />
                                        <asp:TextBox onkeypress="return onKeyPressBlockNumbers(event)" Style="text-align: center;
                                           " visible="false"
                                            CssClass="from-textfield" Width="50px" ID="txtUpgradeDiscountPrice" runat="server" Text='<%#Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"DiscountPrice")).ToString("f2") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        Acknowledgement
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlacknowledgement" runat="server" CssClass="order-list" AutoPostBack="false">
                                            <asp:ListItem Value="0">Accepted</asp:ListItem>
                                            <asp:ListItem Value="1">Rejected</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblaccepted" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsAccepted") %>'></asp:Label>
                                        <asp:Label ID="OrderItemID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderItemID") %>'></asp:Label>
                                        <asp:Label ID="lblIsProductType" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsProductType") %>'></asp:Label>
                                        <asp:Label ID="lblRelatedproductID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RelatedproductID") %>'></asp:Label>
                                        <input type="hidden" id="hdncustom" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                 <asp:TemplateField Visible="true">
                                    <HeaderTemplate>
                                        Sub Total
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        $<asp:Label ID="lblSubtotalrow" runat="server" Text='0.00'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="lblAddProduct" Visible="false" runat="server"></asp:Label>
                </td>
                <td align="right">
                 
                <asp:ImageButton ID="imgupdatesku" runat="server" Visible="false"
                        AlternateText="Update SKU" OnClick="imgupdatesku_Click" />&nbsp;
                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="/Admin/Images/update-orde.gif"
                        AlternateText="Approve Order" OnClick="btnApproveupdate_Click" Visible="false" />&nbsp;<asp:ImageButton
                            ID="btnApprove" runat="server" ImageUrl="/Admin/Images/approve-order.jpg" AlternateText="Approve Order"
                            OnClick="btnApprove_Click" Visible="false" />
                    <asp:ImageButton ID="imgprocessOrder" runat="server" OnClientClick="javascript:return ConfirmDelete('Are you sure want to processing this order?','imgprocessOrder');"
                        ImageUrl="" AlternateText="Processing Order" OnClick="imgprocessOrder_Click"
                        Visible="false" />
                    <asp:ImageButton ID="imgshortshiplineOrder" OnClientClick="javascript:return ConfirmDelete('Are you sure want to short ship line this order?','imgshortshiplineOrder');"
                        runat="server" ImageUrl="/Admin/Images/update-orde.gif" AlternateText="Shortship Line"
                        OnClick="imgshortshiplineOrder_Click" Visible="false" />
                </td>
            </tr>
            <tr style="display: none;">
                <td align="center" style="height: 19px" width="100%" colspan="2">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 19px">
                                <asp:Literal ID="ltCart" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td align="right" colspan="5" style="height: 21px; text-align: right" valign="top">
                                &nbsp;Quantity Discount:
                            </td>
                            <td style="width: 15%; height: 21px; text-align: right" valign="top">
                                $<asp:Label ID="lblQuantity" runat="server" CssClass="font-black04">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="height: 21px; text-align: right">
                                Original Subtotal:
                            </td>
                            <td style="text-align: right; height: 21px; width: 15%;" valign="top">
                                $<asp:Label ID="lblOrgSubTotal" runat="server" CssClass="font-black04">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="height: 21px; text-align: right">
                                New Subtotal:
                            </td>
                            <td style="text-align: right; height: 21px; width: 15%;" valign="top">
                                $<asp:Label ID="lblSubTotal" runat="server" CssClass="font-black04">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr align="right" valign="top" style="height: 21px; text-align: right" id="TrGiftCard"
                            runat="server" visible="false">
                            <td colspan="5" align="right" valign="top" style="height: 21px; text-align: right">
                                Gift Card Discount Amount:
                            </td>
                            <td style="text-align: right; height: 21px; width: 15%;" valign="top">
                                $<asp:Label ID="lblGiftCardDiscount" runat="server">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="height: 21px; text-align: right">
                                Shipping:
                            </td>
                            <td style="text-align: right; height: 21px; width: 15%;" valign="top">
                                $<asp:Label ID="lblShoppingCost" runat="server" CssClass="font-black04">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" style="height: 21px; text-align: right" valign="top">
                                Order&nbsp; Tax:
                            </td>
                            <td style="width: 15%; text-align: right; height: 21px;" valign="top">
                                $<asp:Label ID="lblOrderTax" runat="server" CssClass="font-black04"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="text-align: right; height: 21px;">
                                Discount Amount:
                            </td>
                            <td style="text-align: right; width: 15%; height: 21px;" valign="top">
                                $<asp:TextBox ID="txtcustomdis" runat="server" Text="0.00" CssClass="font-black04"
                                    Style="text-align: right; height: 15px; font-family: Arial; font-size: 12px"
                                    onkeypress="return onKeyPressBlockNumbers(event)" Width="30px">
                                </asp:TextBox>
                                <asp:Label ID="lblAmount" runat="server" CssClass="font-black04" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trReturnedStarck" runat="server">
                            <td class="style3" colspan="5" align="right" valign="top" style="text-align: right;">
                                Return Item:
                            </td>
                            <td class="style3" style="text-align: right; width: 15%;" valign="top">
                                $<asp:Label ID="lblReturnedStarck" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trReturnFee" runat="Server">
                            <td class="style3" colspan="5" align="right" valign="top" style="text-align: right;">
                                Return/Restocking Fee/Misc charge increase:
                            </td>
                            <td class="style3" style="text-align: right; width: 15%;" valign="top">
                                $<asp:Label ID="lblReturnFee" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="height: 21px; text-align: right">
                                Adjustments:
                            </td>
                            <td style="text-align: right; height: 21px; width: 15%;" valign="top">
                                $<asp:Label ID="lblAdjustments" runat="server" CssClass="font-black04">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right" valign="top" style="text-align: right; height: 21px;">
                                Total :
                            </td>
                            <td style="text-align: right; width: 15%; height: 21px;" valign="top">
                                $<asp:Label ID="lblTotal" runat="server" CssClass="font-black04"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
