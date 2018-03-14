<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorPOProblem.aspx.cs"
    Inherits="Solution.UI.Web.VendorPOProblem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
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
    </style>
    <script type="text/javascript">
        function CheckSelection() {

            if (document.getElementById("rdocheck_0").checked == false && document.getElementById("rdocheck_1").checked == false && document.getElementById("rdocheck_2").checked == false) {
                alert('Please choose option.');
                return false;
            }
            if (document.getElementById("rdocheck_2").checked == true) {
                if (document.getElementById("txtOther").value == '') {
                    alert('Please write text.');
                    document.getElementById("txtOther").focus();
                    return false;
                }

            }
            return true;
        }
    
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div class="right-sidebar" style="float: left;">
        <table cellpadding="0" cellspacing="0" align="center">
            <tr>
                <td>
                    <div class="static-main-box" style="width: 969px;">
                        <table cellspacing="0" cellpadding="0" border="0" class="table_border" width="791px">
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="float: left;">
                                        <tr>
                                            <td valign="middle">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="logo">
                                                            <a href="#">
                                                                <img src="/images/logo.png" style="float: left; padding: 10px 0 0 10px;" /></a>
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
                                                    <%--<tr style="height: 25px">
                                                        <td style="width: 100%; background-color: #E7E7E7; line-height: 20px; text-indent: 10px;
                                                            font-weight: bold; border-bottom: solid 1px #dfdfdf;">
                                                            Vendor Po Problems
                                                        </td>   
                                                    </tr>--%>
                                                    <tr id="trVendordetails" runat="server">
                                                        <td align="left" style="padding-left: 0px; padding-right: 0px" width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #dfdfdf;"
                                                                width="100%" class="table-none-border">
                                                                <tr>
                                                                    <td style="width: 100%; background-color: #E7E7E7; line-height: 20px; text-indent: 10px;
                                                                        font-weight: bold; border-bottom: solid 1px #dfdfdf;">
                                                                        Vendor Information :
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="padding-left: 10px">
                                                                        <table>
                                                                            <tr id="ordertr" runat="server">
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    Order Number:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
                                                                                    <asp:Literal ID="ltOrder" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    PO Number:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
                                                                                    <asp:Literal ID="ltPONumber" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    PO Date:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
                                                                                    <asp:Literal ID="ltpodate" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    Vendor Name:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
                                                                                    <asp:Literal ID="ltvname" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    Phone:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
                                                                                    <asp:Literal ID="ltvphone" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="line-height: 20px; font-weight: bold; font-size: 11px;">
                                                                                    E-mail:
                                                                                </td>
                                                                                <td align="left" style="line-height: 20px; font-size: 11px;">
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
                                            <td align="left" valign="top" style="padding-top: 10px; padding-bottom: 10px;">
                                                <div id="divGridlist" runat="server">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #dfdfdf;"
                                                        width="100%">
                                                        <tr>
                                                            <td style="width: 100%; background-color: #f3f3f3; line-height: 30px; text-indent: 10px;
                                                                font-weight: bold; border-bottom: solid 1px #dfdfdf;" colspan="2">
                                                                Choose Option :
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-left: 10px">
                                                                <table>
                                                                    <tr>
                                                                        <td align="left" style="font-weight: bold;">
                                                                            <asp:RadioButtonList ID="rdocheck" runat="server">
                                                                                <asp:ListItem Value="Item Discontinued">&nbsp;Item Discontinued</asp:ListItem>
                                                                                <asp:ListItem Value="Out of stock">&nbsp;Out of stock</asp:ListItem>
                                                                                <asp:ListItem Value="Other">&nbsp;Other</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="font-weight: normal; padding-left: 20px;">
                                                                            <asp:TextBox ID="txtOther" runat="server" TextMode="MultiLine" Width="450" Height="60px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="display: none">
                                                                        <td>
                                                                            <asp:GridView ID="gvVendor" runat="server" class="table" GridLines="None" RowStyle-BorderStyle="None"
                                                                                AutoGenerateColumns="False" Width="90%" RowStyle-BorderWidth="0px" BorderStyle="None"
                                                                                BackColor="white" BorderWidth="0px">
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
                                                                                            <strong>Product Name</strong>
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
                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                <strong>SKU</strong>
                                                                                            </table>
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
                                                                                            <strong>Quantity</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Your Price<br />
                                                                                                (per unit)</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="right" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPrice" runat="server" Text='<%# Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price"))) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Tracking #</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTrackingNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"TrackingNumber") %>'
                                                                                                Visible="false"></asp:Label>
                                                                                            <input type="hidden" id="hdnShippdeOn" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ShippedOn") %>' />
                                                                                            <input type="hidden" id="hdnOrderNumber" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>' />
                                                                                            <asp:Label ID="lblTracking" runat="server" Text="--" Visible="false"></asp:Label>
                                                                                            <asp:TextBox ID="txttracking" runat="server" CssClass="contact_fild" Width="50px"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderStyle HorizontalAlign="center" />
                                                                                        <HeaderStyle BackColor="#E7E7E7" />
                                                                                        <HeaderTemplate>
                                                                                            <strong>Shipping Method</strong>
                                                                                        </HeaderTemplate>
                                                                                        <ItemStyle HorizontalAlign="center" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblShippedVia" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippedVia") %>'></asp:Label>
                                                                                            <asp:DropDownList ID="drpMethod" runat="server" AutoPostBack="false" CssClass="listmenu">
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
                                                                                <AlternatingRowStyle CssClass="gridalt" />
                                                                                <PagerStyle CssClass="paging" />
                                                                                <PagerSettings Position="TopAndBottom" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td align="center" style="padding-top: 10px; padding-bottom: 10px;">
                                                <asp:Label ID="lblMsgUpdated" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="padding-bottom: 5px;">
                                                <asp:ImageButton ID="btnSubmit" ToolTip="Submit" runat="server" Visible="false" OnClientClick="return CheckSelection();"
                                                    ImageUrl="/images/finish.jpg" OnClick="btnSubmit_Click" /><input type="hidden" id="hdsecond"
                                                        runat="server" value="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
