<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorProducts.aspx.cs"
    Inherits="Solution.UI.Web.VendorProducts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vendor Product</title>
    <script src="js/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function loader() {
            chkHeight();
        }
    </script>
    <style type="text/css">
        body
        {
            background: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2a2a2a;
        }
        a, img
        {
            border: none;
            outline: none;
        }
        .table_border
        {
            border: 1px solid #D5D4D4;
            margin: 0 auto;
        }
        .checkout-red
        {
            color: #fe0000;
        }
        .name-input
        {
            border: 1px solid #E4E4E4;
            height: 18px;
            width: 210px;
            font-size: 12px;
            padding: 1px 5px;
        }
        .logo
        {
            float: left;
            width: 480px;
            background: #fff;
            padding: 10px 0 10px;
            margin: 0;
        }
        .close-bg
        {
            float: right;
            width: 30px;
            padding: 15px 40px 12px 0;
            text-align: left;
            background: #fff;
            margin: 0;
        }
        .select-box
        {
            width: 96px;
            border: 1px solid #dbdbdb;
            font-size: 12px;
            color: #141414;
            line-height: 24px;
            padding: 2px;
            margin: 0 0 0 5px;
        }
        .text-field
        {
            border: 1px solid #DFDFDF;
            color: #141414;
            font-size: 12px;
            height: 18px;
            padding: 1px 5px;
            width: 180px;
            float: left;
            margin: 0 2px 0 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="right-sidebar" style="float: left;">
        <table cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td>
                    <div class="static-main-box" style="width: 969px;">
                        <table cellspacing="0" cellpadding="0" border="0" class="table_border" width="791px">
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td valign="middle">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td class="logo">
                                                                <a href="#" title="">
                                                                    <img alt="logo" border="0" src="" style="float: left; padding: 10px 0 0 10px;" id="imgStoreLogo"
                                                                        runat="server" /></a>
                                                            </td>
                                                            <td class="close-bg">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="msgtr" runat="server">
                                                <td align="center">
                                                    <asp:Label ID="lblmsg" runat="server" Style="color: red; text-align: center; font-weight: bold;
                                                        padding: 10px 50px; display: block;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="gridlisttr" runat="server">
                                                <td align="center" width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr id="trVendordetails" runat="server">
                                                            <td align="left" colspan="3" style="padding-left: 40px; padding-right: 40px" width="100%">
                                                                <table cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #dfdfdf;"
                                                                    width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; background-color: #f3f3f3; line-height: 30px; text-indent: 10px;
                                                                            font-weight: bold; border-bottom: solid 1px #dfdfdf;" colspan="2">
                                                                            Vendor Details :
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="padding-left: 10px">
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                                        Order Number:
                                                                                    </td>
                                                                                    <td align="left" style="line-height: 30px">
                                                                                        <asp:Literal ID="ltOrder" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                                        PO Date:
                                                                                    </td>
                                                                                    <td align="left" style="line-height: 30px">
                                                                                        <asp:Literal ID="ltpodate" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                                        Vendor Name:
                                                                                    </td>
                                                                                    <td align="left" style="line-height: 30px">
                                                                                        <asp:Literal ID="ltvname" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                                        Phone:
                                                                                    </td>
                                                                                    <td align="left" style="line-height: 30px">
                                                                                        <asp:Literal ID="ltvphone" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                                        E-mail:
                                                                                    </td>
                                                                                    <td align="left" style="line-height: 30px">
                                                                                        <asp:Literal ID="ltvemail" runat="server"></asp:Literal>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" style="padding-top: 20px; padding-bottom: 10px;">
                                                    <div id="Gridlist">
                                                        <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                            ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                                            CellPadding="2" CellSpacing="1" GridLines="None" Width="90%" OnRowDataBound="gvVendor_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <HeaderTemplate>
                                                                        Product Name
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderTemplate>
                                                                        SKU
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderTemplate>
                                                                        Quantity
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="center" Width="10%" />
                                                                    <HeaderTemplate>
                                                                        Your Price<br />
                                                                        (per unit)
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="right" />
                                                                    <ItemTemplate>
                                                                        $<asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("{0:0.00}",Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderTemplate>
                                                                        Status
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <input type="hidden" id="availdays" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsShipped") %>' />
                                                                        <input type="hidden" id="hndpaid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Ispaid") %>' />
                                                                        <asp:DropDownList OnSelectedIndexChanged="availdays_SelectedIndexChanged" AutoPostBack="true"
                                                                            CssClass="select-box" ID="drpAvailDays" runat="server">
                                                                            <asp:ListItem Text="Pending" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Shipped" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Paid" Value="2"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderTemplate>
                                                                        Tracking #
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTrackingNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TrackingNumber") %>'
                                                                            Visible="false"></asp:Label>
                                                                        <input type="hidden" id="hdnShippdeOn" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ShippedOn") %>' />
                                                                        <input type="hidden" id="hdnOrderNumber" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>' />
                                                                        <asp:Label ID="lblTracking" runat="server" Text="--" Visible="false"></asp:Label>
                                                                        <asp:TextBox ID="txttracking" runat="server" CssClass="text-field" Width="120px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle HorizontalAlign="center" />
                                                                    <HeaderStyle BackColor="#E7E7E7" />
                                                                    <HeaderTemplate>
                                                                        Shipping Method
                                                                    </HeaderTemplate>
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblShippedVia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedVia") %>'></asp:Label>
                                                                        <asp:DropDownList ID="drpMethod" runat="server" AutoPostBack="false" CssClass="select-box">
                                                                            <asp:ListItem Value="UPS" Selected="True" Text="UPS"></asp:ListItem>
                                                                            <asp:ListItem Value="USPS" Text="USPS"></asp:ListItem>
                                                                            <asp:ListItem Value="FEDEX" Text="FEDEX"></asp:ListItem>
                                                                            <asp:ListItem Value="DHL" Text="DHL"></asp:ListItem>
                                                                            <asp:ListItem Value="Freight" Text="Freight"></asp:ListItem>
                                                                            <asp:ListItem Value="Truck" Text="Truck"></asp:ListItem>
                                                                            <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <AlternatingRowStyle CssClass="altrow" />
                                                            <PagerStyle CssClass="paging" />
                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                            <PagerSettings Position="TopAndBottom" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trNotes" runat="server">
                                                <td align="center" width="100%">
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr id="tr2" runat="server">
                                                            <td align="left" colspan="3" style="padding-left: 40px; padding-right: 40px" width="100%">
                                                                <table cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #dfdfdf;"
                                                                    width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; background-color: #f3f3f3; line-height: 30px; text-indent: 10px;
                                                                            font-weight: bold; border-bottom: solid 1px #dfdfdf;" colspan="2">
                                                                            Notes
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" valign="top" style="padding: 10px;">
                                                                            <asp:TextBox ID="txtNotes" runat="server" class="text-field" TextMode="MultiLine"
                                                                                Width="700px" Height="75px">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="padding-top: 10px; padding-bottom: 10px;">
                                                    <asp:Label ID="lblMsgUpdated" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" OnClientClick="loader();"
                                                        ImageUrl="/images/finish.jpg" OnClick="btnsubmit_Click" /><br />
                                                    <br />
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
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%">
            <tr>
                <td align="center" style="color: #fff;">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
