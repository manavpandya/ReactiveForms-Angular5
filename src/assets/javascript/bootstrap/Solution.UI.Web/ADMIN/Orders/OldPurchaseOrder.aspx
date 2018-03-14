<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldPurchaseOrder.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.OldPurchaseOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../js/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
            $('#txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function PrintPO() {
            w = window.open('', 'Print', 'directories=no, location=no, menubar=no, status=no,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=5,Width=5,left=0,top=0,visible=false,alwaysLowered=yes');
            w.document.write(document.getElementById("poorderprint").innerHTML);
            w.document.close();
            w.print();
            w.close();
        }

            function ValidDate() {
                if (document.getElementById('txtFromDate') != null && document.getElementById('txtFromDate').value.replace(/^\s+|\s+$/g, "") == '') {
                    jAlert('Please Enter From Date.', 'Message', 'txtFromDate');
                    return false;
                }
                if (document.getElementById('txtToDate') != null && document.getElementById('txtToDate').value.replace(/^\s+|\s+$/g, "") == '') {
                    jAlert('Please Enter To Date.', 'Message', 'txtToDate');
                    return false;
                }
                return true;
            }
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        } 
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="../../App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Purchase Order Details
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" runat="server" id="btnClose"
                                onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <b style="text-decoration: underline;">
                                        <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
                                    </b>
                                </td>
                                <td align="right" style="padding-top: 8px;">
                                    <a href="#" runat="server" id="GeneratePO">
                                        <img class="printbtnPo" src="/App_Themes/<%=Page.Theme %>/images/generate-purchase-order.png"
                                            style="border: none;" />
                                    </a>&nbsp;<a onclick="javascript:window.location.href=window.location.href;" href="javascript:void(0);"
                                        target="_parent">
                                        <img id="img1" title="Refresh Purchase Order" alt="Refresh Purchase Order" src="/App_Themes/<%=Page.Theme %>/images/refresh-purchase-order.png" />
                                    </a>&nbsp;&nbsp; <a id="printPOid" runat="server" href="javascript:void(0)" onclick="PrintPO();"
                                        style="color: #000; text-decoration: none; font-weight: bold">
                                        <img src="/App_Themes/<%=Page.Theme %>/images/print.png" alt="Print" title="Print" />
                                    </a>&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="datetr" runat="server">
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                            <tr>
                                <td align="left">
                                    From Date :
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtFromDate" CssClass="order-textfield" Width="86px"></asp:TextBox>
                                </td>
                                <td align="left">
                                    To Date :
                                </td>
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtToDate" CssClass="order-textfield" Width="86px"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnSubmit" runat="server" OnClientClick="return ValidDate();"
                                        OnClick="btnSubmit_Click" />
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="btnShowAll" Style="vertical-align: middle" runat="server" OnClientClick="if (document.getElementById('txtFromDate') != null) {document.getElementById('txtFromDate').value = '';}if (document.getElementById('txtToDate') != null) {document.getElementById('txtToDate').value = '';}"
                                        OnClick="btnShowAll_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="ltDetails" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 2px">
                        <div id="poorderprint" style="border: 5px sollid #e7e7e7; overflow-y: scroll; height: 450px;
                            padding-top: 2px;">
                            <asp:GridView ID="gvOldVendorPO" runat="server" AutoGenerateColumns="False" Width="100%"
                                class="order-table" Style="border: solid 1px #e7e7e7" ShowFooter="true" OnRowDataBound="gvOldVendorPO_RowDataBound">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <label style="color: Red">
                                        No records found ...</label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <a id="lnkPo" runat="server" style="color: #FF0000; text-decoration: underline;"
                                                href='javascript:void(0);'>PO-<asp:Label ID="lblPOName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>'></asp:Label>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Order Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>
                                            <asp:Label ID="lblOrdernumber" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Additional Cost
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblACost" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"AdditionalCost") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Sale Tax
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Tax") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Shipping
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblShipping" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"Shipping") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Adjustments
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdjust" runat="server" Text='<%# MakePositive(DataBinder.Eval(Container.DataItem,"Adjustments").ToString())   %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Amount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOAmt" runat="server" Text='<%# "$"+ DataBinder.Eval(Container.DataItem,"PoAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Date
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PODate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass=".order-table td" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr id="trTotal" runat="server">
                    <td style="height: 35px;">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                            style="padding: 2px;">
                            <tr>
                                <td style="padding: 2px; width: 150px;">
                                    <asp:Literal ID="ltClearPOAmt" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 2px">
                                    <asp:Literal ID="ltUnClearPOAmt" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 2px">
                                    <asp:Literal ID="ltTotalPOAmt" runat="server"></asp:Literal>
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
