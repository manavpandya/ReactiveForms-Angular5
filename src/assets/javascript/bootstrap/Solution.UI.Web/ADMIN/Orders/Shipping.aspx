<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shipping.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.Shipping" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Gray/css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css" media="print">
        .PrintShippingDetail
        {
            display: none;
        }
    </style>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">
        function CheckValidation() {
            //            if (document.getElementById("ddlWareHouse") != null && document.getElementById("ddlWareHouse").selectedIndex == 0) {
            //                jAlert('Please Select Warehouse', 'Message', 'ddlWareHouse');
            //                return false;
            //}
            return true;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            for (var i = 0; i < 100; i++) {
                if (document.getElementById('grdShipping_txtShippedOn2_' + i) != null) {
                    $('#grdShipping_txtShippedOn2_' + i).datetimepicker({
                        showButtonPanel: true, ampm: false,
                        showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                        buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                }
            }

            if (document.getElementById('txtGeneralShippedOn') != null) {
                $('#txtGeneralShippedOn').datetimepicker({
                    showButtonPanel: true, ampm: false,
                    showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                    buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
                    });
                }
        });
    </script>
    <script type="text/javascript">
        function PrintShipping() {
            //            w = window.open('', 'Print', 'directories=no, location=no, menubar=no, status=no,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=5,Width=5,left=0,top=0,visible=false,alwaysLowered=yes');
            //            //w.document.write($('.report_left_inner').html());

            //            w.document.write(document.getElementById("shipdiv").innerHTML);
            //            w.document.close();

            //            w.print();

            //            w.close();
            window.print();
        }
    </script>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr class="PrintShippingDetail">
                <td class="border-td-sub">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                        <tbody>
                            <tr>
                                <th>
                                    <div class="main-title-left">
                                        <h2 style="padding-left: 0px;">
                                            Shipping</h2>
                                    </div>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="content_box_border">
                        <table width="100%" cellspacing="0" class="add-product" cellpadding="0" border="0">
                            <tr>
                                <td align="center" class="border">
                                    <table width="100%" cellpadding="0" cellspacing="0" style="padding: 10px">
                                        <tr class="PrintShippingDetail" tyle="border:1px solid #DFDFDF;">
                                            <td colspan="2">
                                                <table width="100%" style="border: 1px solid #DFDFDF;" cellpadding="0" cellspacing="0"
                                                    align="center">
                                                    <tr valign="top" class="altrow">
                                                        <td style="padding-left: 5px; width: 6%;">
                                                            Ship To:
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Literal ID="ltShippingTo" runat="server"></asp:Literal>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="PrintShippingDetail">
                                            <td align="left">
                                                <b style="text-decoration: underline; font-size: 10pt; padding-left: 5px">Shipped Items:</b>
                                            </td>
                                            <td align="right">
                                                <a id="A2" href="javascript:void(0)" onclick="PrintShipping();" style="color: #000;
                                                    text-decoration: underline; font-weight: bold; font-size: 10pt; padding-right: 20px">
                                                    <img src="/App_Themes/<%=Page.Theme %>/images/print.png" alt="Print" title="Print"
                                                        style="float: right;" />
                                                </a>
                                            </td>
                                        </tr>
                                        <tr valign="top" class="oddrow">
                                            <td style="padding-left: 5px; padding-top: 5px;" align="left" >
                                                <b>Warehouse </b>: &nbsp;<asp:DropDownList ID="ddlWareHouse" runat="server" class="order-list"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlWareHouse_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            <Table>
                                                <tr>
                                            <td style="padding-left: 5px; padding-top: 5px;" align="left" colspan="2">
                                                <b>Tracking Number </b>:<asp:TextBox ID="txtGeneralTrackingNumber"  Text="0" runat="server"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="ReqTracking" runat="server" Text="*" ForeColor="Red"
                                                                        ControlToValidate="txtGeneralTrackingNumber" ValidationGroup="UpdateGeneralShipping" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="padding-left: 5px; padding-top: 5px;" align="left" colspan="2">
                                                <b>Shipped On</b>:   <asp:TextBox ID="txtGeneralShippedOn" Text="01/01/2000" runat="server"  Width="70px"
                                                                        tyle="margin-right: 3px;" CssClass="from-textfield" Visible="True"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqGeneralDate" runat="server" Text="*" ForeColor="Red"
                                                                        ControlToValidate="txtGeneralShippedOn" ValidationGroup="UpdateGeneralShipping"></asp:RequiredFieldValidator>
                                                                    <%-- <asp:RangeValidator Type="Date" ID="rangeDate" MaximumValue="12/31/2999" MinimumValue="01/01/2000" runat="server" ControlToValidate="txtShippedOn" Text="Invalid" ValidationGroup="UpdateShipping"></asp:RangeValidator>--%>
                                                                    <asp:CompareValidator Type="Date" ID="compGeneralDate" runat="server" ControlToValidate="txtGeneralShippedOn"
                                                                        ValidationGroup="UpdateGeneralShipping" Text="Invalid Date" Operator="dataTypeCheck"></asp:CompareValidator>
                                                 <!--img class="ui-datepicker-trigger" src="/App_Themes/Gray/images/date-icon.png" alt="" title=""-->
                                                                   
                                            </td>
                                            <td style="padding-left: 5px; padding-top: 5px;" align="left" colspan="2">
                                                <b>Shipped Via</b>:  <asp:DropDownList ID="ddlGeneralShippedVIA" runat="server" Visible="True" CssClass="order-list"
                                                                        Width="80px">
                                                                        <asp:ListItem Value="FedEx" Selected="True" Text="FedEx"></asp:ListItem>
                                                                        <asp:ListItem Value="UPS" Text="UPS"></asp:ListItem>
                                                                        <asp:ListItem Value="USPS" Text="USPS"></asp:ListItem>
                                                                        <asp:ListItem Value="Freight" Text="Freight"></asp:ListItem>
                                                                        <%--<asp:ListItem Value="DHL" Text="DHL"></asp:ListItem>
                                                                            <asp:ListItem Value="Truck" Text="Truck"></asp:ListItem>--%>
                                                                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                                                    </asp:DropDownList>
                                            </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnGeneralUpdate" runat="server" ValidationGroup="UpdateGeneralShipping" AlternateText="Update" ImageUrl="~/App_Themes/Gray/images/save.gif" OnClick="btnGeneralUpdate_Click"/>
                                                    </td>
                                                    </tr>
                                                </Table>
                                                </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <div id="shipdiv">
                                                    <asp:GridView ID="grdShipping" runat="server" Width="100%" AutoGenerateColumns="false"
                                                        OnRowDataBound="grdShipping_RowDataBound" ShowFooter="false" OnRowCommand="grdShipping_RowCommand"
                                                        CssClass="table-noneforOrder" EmptyDataText="No Record Found!" CellSpacing="0"
                                                        CellPadding="5" border="1" Style="width: 100%; border-collapse: collapse;">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RefProductID") %>'></asp:Label>
                                                                    <asp:Label ID="lblCustomCartID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'></asp:Label>
                                                                    <asp:Label ID="lblVariantNames" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantNames") %>'></asp:Label>
                                                                    <asp:Label ID="lblVariantValues" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VariantValues") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Product Name
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="25%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    SKU
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Quantity
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                    <asp:Label ID="lblOldQty" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedQty") %>'></asp:Label>
                                                                    <asp:Label ID="lblOldWarehouseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"WarehouseId") %>'></asp:Label>
                                                                    <asp:Label ID="lblInventoryupdated" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Inventoryupdated") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <HeaderTemplate>
                                                                    Available Qty
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblavailQuantity" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                <FooterStyle HorizontalAlign="center" />
                                                                <HeaderStyle Width="30px" HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Shipped&nbsp;Items
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShippedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedQty") %>'></asp:Label>
                                                                    <asp:TextBox Visible="False" ID="txtShippedQty" runat="server" CssClass="textfield_small"
                                                                        Width="50" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedQty") %>' Style="text-align: center;"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="ReqShipQty" runat="server" Text="*" ControlToValidate="txtShippedQty"
                                                                        ValidationGroup="UpdateShipping"></asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Tracking Number
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="15%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTrackingNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TrackingNumber") %>'></asp:Label>
                                                                    <asp:TextBox Visible="False" ID="txtTrackingNumber" runat="server" Text="0" Width="150"
                                                                        CssClass="textfield_small"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="ReqTracking" runat="server" Text="*" ForeColor="Red"
                                                                        ControlToValidate="txtTrackingNumber" ValidationGroup="UpdateShipping" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Shipped On
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShippedOn" runat="server" Text='<%# SetShortDate(Convert.ToString(DataBinder.Eval(Container.DataItem,"ShippedOn"))) %>'></asp:Label>
                                                                    <asp:TextBox Visible="False" ID="txtShippedOn1" runat="server" CssClass="textfield_small"
                                                                        Width="100" Text="01/01/2000"></asp:TextBox>
                                                                    <asp:TextBox ID="txtShippedOn2" runat="server" CssClass="from-textfield" Width="70px"
                                                                        tyle="margin-right: 3px;" Visible="false"></asp:TextBox>
                                                                   
                                                                    <asp:RequiredFieldValidator ID="reqDate" runat="server" Text="*" ForeColor="Red"
                                                                        ControlToValidate="txtShippedOn2" ValidationGroup="UpdateShipping"></asp:RequiredFieldValidator>
                                                                    <%-- <asp:RangeValidator Type="Date" ID="rangeDate" MaximumValue="12/31/2999" MinimumValue="01/01/2000" runat="server" ControlToValidate="txtShippedOn" Text="Invalid" ValidationGroup="UpdateShipping"></asp:RangeValidator>--%>
                                                                    <asp:CompareValidator Type="Date" ID="compDate" runat="server" ControlToValidate="txtShippedOn2"
                                                                        ValidationGroup="UpdateShipping" Text="Invalid Date" Operator="dataTypeCheck"></asp:CompareValidator>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Shipped Note
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShippedNote" runat="server" Text='<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"ShippedNote")) %>'></asp:Label>
                                                                    <asp:TextBox Visible="False" ID="txtShippedNote" runat="server" CssClass="textfield_small"
                                                                        Width="180px" Height="35px" TextMode="MultiLine" Style="resize: none"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Shipped Via
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10%" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShippedVia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedVia") %>'></asp:Label>
                                                                    <asp:DropDownList ID="ddlShippedVIA" runat="server" Visible="false" CssClass="order-list"
                                                                        Width="80px">
                                                                        <asp:ListItem Value="FedEx" Selected="True" Text="FedEx"></asp:ListItem>
                                                                        <asp:ListItem Value="UPS" Text="UPS"></asp:ListItem>
                                                                        <asp:ListItem Value="USPS" Text="USPS"></asp:ListItem>
                                                                        <asp:ListItem Value="Freight" Text="Freight"></asp:ListItem>
                                                                        <%--<asp:ListItem Value="DHL" Text="DHL"></asp:ListItem>
                                                                            <asp:ListItem Value="Truck" Text="Truck"></asp:ListItem>--%>
                                                                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Shipped
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="7%" />
                                                                <ItemTemplate>
                                                                    <img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Shipped"))) %>' />
                                                                    <asp:CheckBox ID="chkShipped" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Shipped")) %>'
                                                                        Visible="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Edit
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" ToolTip="Edit" CommandName="CustomEdit"
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'
                                                                        OnClientClick="return CheckValidation();" />
                                                                    <asp:ImageButton ID="btnSave" runat="server" ToolTip="Save" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>'
                                                                        CommandName="CustomSave" Visible="False" ValidationGroup="UpdateShipping" />
                                                                    <asp:ImageButton ID="btnCancel" runat="server" ToolTip="Cancel" CommandName="CustomCancel"
                                                                        Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"OrderedCustomCartID") %>' />
                                                                    <input type="hidden" id="hdnshipped" runat="server" value='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Shipped")) %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="5%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                                        <EditRowStyle CssClass="altrow" />
                                                        <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                                        <HeaderStyle Height="20px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                            BackColor="gray" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <PagerSettings Position="TopAndBottom" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trMessage" runat="server" visible="false" class="PrintShippingDetail">
                                            <td align="center" colspan="2">
                                                <asp:Label ID="lblmailmsg" runat="server" Text="" ForeColor="red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="PrintShippingDetail">
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnItemShippingUpdate" Style="width: 140px; height: 20px; border: 0;
                                                    cursor: pointer;" runat="server" OnClick="ShippingUpdate_Click" />
                                            </td>
                                        </tr>
                                        <tr class="PrintShippingDetail">
                                            <td align="center" colspan="2">
                                                <asp:Literal ID="ltmanualShipping" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
