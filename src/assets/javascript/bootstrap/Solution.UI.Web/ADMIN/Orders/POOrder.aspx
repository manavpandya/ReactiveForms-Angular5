<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POOrder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.POOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function ConfirmDelete() {
            var answer = confirm("Are you sure want to delete this record?")
            if (answer)
                return true;
            else
                return false;
        }
        function PrintPO() {
            w = window.open('', 'Print', 'directories=no, location=no, menubar=no, status=no,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=5,Width=5,left=0,top=0,visible=false,alwaysLowered=yes');
            w.document.write(document.getElementById("poorderprint").innerHTML + '<br/>' + document.getElementById("printpurchaselog").innerHTML);
            w.document.close();
            w.print();
            w.close();
        }
    </script>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr id="tr1" runat="server">
                <td align="right" style="padding-bottom: 10px; font-weight: bold;">
                    <asp:ImageButton ID="btnRefreshPO" runat="server" AlternateText="Refresh Purchase Order"
                        OnClick="btnRefreshPO_Click" />
                </td>
            </tr>
            <tr id="trOldPO" runat="server">
                <td>
                    <asp:Label ID="lblPoOrdrs" Text="Existing Purchase Orders" runat="server" Style="font-size: 12px;
                        font-weight: bold; color: #212121;"></asp:Label>
                    <br />
                    <br />
                    <asp:GridView ID="gvOldPOrder" runat="server" AutoGenerateColumns="False" Width="99%"
                        BorderWidth="0" CssClass="table-noneforOrder" OnRowEditing="gvOldPOrder_RowEditing"
                        OnRowDataBound="gvOldPOrder_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    PONumber
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPoNum" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber").ToString() %>'></asp:Label>
                                    <a id="atagOrderNumber" runat="server" style="text-decoration: underline;">PO-<asp:Label
                                        ID="lblPOName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>'></asp:Label>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Vendor Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                    <asp:Label ID="lblAdjust" runat="server" Text='<%# MakePositive(DataBinder.Eval(Container.DataItem,"Adjustments").ToString())   %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblTax" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Tax") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblACost" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"AdditionalCost") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblShipping" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Shipping") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblPOAmt" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"PoAmount") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    PODate
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbldate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PODate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Change Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#  CheckPending(DataBinder.Eval(Container.DataItem, "PONumber").ToString())%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Resend
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResend" runat="server" Text="" CommandName="Edit"><%#  CheckResend(DataBinder.Eval(Container.DataItem, "PONumber").ToString())%></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" Font-Bold="True" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="list-table-title" BackColor="#F2F2F2" Height="30px" Font-Bold="true" />
                        <HeaderStyle BackColor="#F2F2F2" ForeColor="#384557" />
                    </asp:GridView>
                    <br />
                    <asp:Literal ID="ltClearPOAmt" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltUnClearPOAmt" runat="server" Visible="false"></asp:Literal>
                    <asp:Literal ID="ltTotalPOAmt" runat="server" Visible="false"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="Label1" Text="Purchase Orders" runat="server" Style="font-size: 12px;
                                    font-weight: bold; color: #212121;"></asp:Label>
                            </td>
                            <td align="right">
                                <a id="printPOid" runat="server" href="javascript:void(0)" onclick="PrintPO();" style="color: #000;
                                    text-decoration: underline; font-weight: bold">
                                    <img src="/App_Themes/<%=Page.Theme %>/images/print.png" alt="Print" title="Print" />
                                </a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center">
                                <asp:Literal ID="ltCart" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px 20px 7px 0; text-align: right; float: right;">
                                <asp:ImageButton ID="btnGeneratePO" runat="server" AlternateText="Generate Purchase Order"
                                    OnClick="btnGeneratePO_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div id="poorderprint">
                        <asp:GridView ID="gvVendor" runat="server" Font-Names="Arial" Font-Size="Medium"
                            CssClass="table-noneforOrder" GridLines="Both" RowStyle-BorderStyle="Solid" AutoGenerateColumns="False"
                            ForeColor="#384557" Width="99%" RowStyle-BorderWidth="1px" BorderStyle="Solid"
                            AllowPaging="false" BorderColor="#f3f3f3" BorderWidth="1px" OnRowDataBound="gvVendor_RowDataBound"
                            OnRowDeleting="gvVendor_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="PO Number">
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle ForeColor="#384557" />
                                    <HeaderTemplate>
                                        PO Number
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPonumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PoNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor Name">
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Vendor Name
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" ForeColor="#384557" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblVName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Vname") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        Product Name
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" ForeColor="#384557" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"productName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            SKU
                                        </table>
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" ForeColor="#384557" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="center" />
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderTemplate>
                                        Quantity
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="center" ForeColor="#384557" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                        <input type="hidden" id="hdnNotes" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Notes") %>' />
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("{0:0.00}",DataBinder.Eval(Container.DataItem,"Price"))%>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="center" />
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderTemplate>
                                        Status
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="center" ForeColor="#384557" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IsShipped") %>'></asp:Label>
                                        <input type="hidden" id="hdnpaid" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsPaid") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="center" />
                                    <HeaderStyle BackColor="#DDE0E5" ForeColor="#384557" />
                                    <HeaderTemplate>
                                        Delete
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbldelete" runat="server" Text="Delete" CommandName="Delete"
                                            OnClientClick="return ConfirmDelete();"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                    <HeaderStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle CssClass="gridalt" />
                            <PagerStyle CssClass="paging" />
                            <PagerSettings Position="TopAndBottom" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" width="99%">
                    <div id="printpurchaselog" style="padding-top: 10px">
                        <asp:Literal ID="postatus" runat="server"></asp:Literal>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
