<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldWareHousePOOrderCart.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OldWareHousePOOrderCart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function CheckValid() {
            if (document.getElementById('txtNotesMain') != null && (document.getElementById('txtNotesMain').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Additional Note');
                document.getElementById('txtNotesMain').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div id="divOrder" runat="server" style="width:720px;padding:5px;">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            PO Details
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <asp:Label ID="lblMsg" runat="server"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="color: #4D4D4C; text-align: left;" colspan="2">
                        <table cellspacing="1" cellpadding="2" width="700px" class="table_none">
                            <tr>
                                <td style="padding-bottom: 10px; padding-left: 10px">
                                    <asp:Literal ID="ltOrderNo" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-bottom: 10px; padding-right: 10px;" align="right">
                                    <asp:Literal ID="ltDate" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 10px;" colspan="2">
                                    <asp:Literal ID="ltVendorName" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 10px;">
                                </td>
                            </tr>
                            <tr style="padding-top: 20px;">
                                <td style="padding-left: 10px;" colspan="2">
                                    <div style="float: left;">
                                        <asp:Literal ID="litProducts" runat="Server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table cellspacing="1" cellpadding="2" width="700px">
                                        <tr id="trsubtotal" runat="server" visible="false">
                                            <td style="width: 150px; padding-left: 10px;">
                                                <b>Subtotal</b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltSubtotal" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="width: 108px; padding-left: 10px;" align="right">
                                                <b>Ship To Address </b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litShipToAddr" runat="Server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <asp:Literal ID="ltPoStatus" runat="Server" Visible="false"></asp:Literal>
                                        </tr>
                                        <tr valign="top">
                                            <td style="padding-left: 10px;" align="right" valign="top">
                                                <b>Notes </b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litNotes" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <asp:Literal ID="ltstatus" runat="Server" Visible="false"></asp:Literal>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="width: 108px; padding-left: 10px;">
                                                <b>Additional Cost</b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="litAddCost" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="width: 108px; padding-left: 10px;">
                                                <b>Sale Tax</b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltTax" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="padding-left: 10px;">
                                                <b>Shipping</b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltShipping" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="padding-left: 10px;">
                                                <b>Adjustments Cost</b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="ltAdjustmetns" runat="Server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="padding-left: 10px;">
                                                <b>PO Amount </b>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lbPOAmount" runat="Server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left" width="100%">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:Literal ID="Ordernotesid" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 5px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 120px;padding-left:10px;" valign="top">
                                                <b>Add&nbsp;Additional&nbsp;Note&nbsp;:</b>&nbsp;
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtNotesMain" Text="" Width="425px" Height="50px" TextMode="MultiLine"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left" valign="top">
                                    <%--<asp:ImageButton ID="btnSave" runat="server" OnClientClick="return CheckValid();"
                                        ImageUrl="~/Admin/images/save.jpg" OnClick="btnSave_Click" />--%>
                                    <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return CheckValid();"
                                        ImageUrl="~/Admin/images/save.jpg" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
