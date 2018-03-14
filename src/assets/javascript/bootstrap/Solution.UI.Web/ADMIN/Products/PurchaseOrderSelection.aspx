<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderSelection.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Products.PurchaseOrderSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function selectAll() {
            var tables = document.getElementById('dvgrid').getElementsByTagName("table");
            var totalPOAmount = 0;
            var totalRemaining = 0;
            var totalPaid = 0;
            var trs = tables[0].getElementsByTagName('tr');

            for (var j = 0; j < trs.length - 1; j++) {
                var theCells = trs[j].getElementsByTagName("td");
                if (theCells.length > 0) {
                    var chkc = theCells[0].getElementsByTagName("input")[0];
                    var POAmount = theCells[3].getElementsByTagName("span")[0];
                    var Remaining = theCells[5].getElementsByTagName("input")[0];
                    var paid = theCells[4].getElementsByTagName("span")[0];
                    if (chkc.checked) {
                        if (parseFloat(POAmount.innerHTML) < (parseFloat(Remaining.value) + parseFloat(paid.innerHTML))) {
                            alert("Paid Amount should be less than PO Amount");
                            var RemainBalance = POAmount.innerHTML - parseFloat(paid.innerHTML).toFixed(2);
                            Remaining.value = parseFloat(RemainBalance).toFixed(2);
                            Remaining.focus();
                            return false;
                        }
                        totalPOAmount = parseFloat(totalPOAmount) + parseFloat(POAmount.innerHTML);
                        totalRemaining = parseFloat(totalRemaining) + parseFloat(Remaining.value);
                        totalPaid = parseFloat(totalPaid) + parseFloat(paid.innerHTML);
                    }
                }
            }
            var theCells = trs[trs.length - 1].getElementsByTagName("td");
            if (theCells.length > 0) {
                tPOAmount = theCells[3].getElementsByTagName("span")[0];
                tRemaining = theCells[5].getElementsByTagName("span")[0];
                tPaid = theCells[4].getElementsByTagName("span")[0];
                tPOAmount.innerHTML = String(totalPOAmount.toFixed(2));
                tRemaining.innerHTML = String(totalRemaining.toFixed(2));
                tPaid.innerHTML = String(totalPaid.toFixed(2));
                document.getElementById("lblCost").innerHTML = "$" + tPOAmount.innerHTML;
                document.getElementById("lblReAmount").innerHTML = "$" + String(totalRemaining.toFixed(2));
                document.getElementById("lblPaid").innerHTML = "$" + totalPaid.toFixed(2);
            }
        }
        function onKeyPressBlockNumbers(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0) {
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
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                            Search Purchase Order(s)
                        </div>
                        <div class="main-title-right">
                            <asp:ImageButton ID="btnClose" OnClick="btnclose_Click" runat="server"></asp:ImageButton>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td style="padding: 2px">
                        <div id="dvgrid" style="border: 5px sollid #e7e7e7; overflow-y: scroll; height: 400px;">
                            <asp:GridView ID="GridVenPruchase" runat="server" AutoGenerateColumns="False" Width="100%"
                                class="order-table" Style="border: solid 1px #e7e7e7" ShowFooter="true" OnRowDataBound="GridVenPruchase_RowDataBound">
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <label style="color: Red">
                                        No records found ...</label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPoNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            Select
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox onclick="selectAll();" ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="7%" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONum" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PONumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Order Number
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblONumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Amount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            $<asp:Label ID="lblPAmount" runat="server" Text='<%# Math.Round(Convert.ToDecimal( DataBinder.Eval(Container.DataItem, "POAmount").ToString()),2) %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <b>$<asp:Label ID="lblTotalPaidAmount" runat="server"></asp:Label>
                                            </b>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Paid Amount
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            $<asp:Label ID="lblPaidAmount" runat="server" Text='<%# Math.Round(Convert.ToDecimal( DataBinder.Eval(Container.DataItem, "PaidAmount").ToString()),2) %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <b>$<asp:Label ID="lblTPAmount" runat="server"></asp:Label>
                                            </b>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Remaining
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            $<asp:TextBox AutoCompleted="true" onblur="selectAll();" onkeypress="return onKeyPressBlockNumbers(event)"
                                                Width="60px" MaxLength="8" CssClass="order-textfield" Style="text-align: center;"
                                                ID="lblRemaingAmount" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <b>$<asp:Label ID="lblTotalRemaining" runat="server"></asp:Label>
                                            </b>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            Additional Cost
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            $<asp:Label ID="lblAddCost" runat="server" Text='<%#Math.Round(Convert.ToDecimal( DataBinder.Eval(Container.DataItem, "AdditionalCost").ToString()),2) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="left" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            PO Date
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PODate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="order-table td" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr id="trAddSeletedItems" runat="server">
                    <td style="height: 30px; padding: 2px; padding-right: 4px;">
                        <asp:ImageButton ID="btnAddSelectedItems" OnClientClick="selectAll();" runat="server"
                            OnClick="btnAddSelectedItems_Click" />
                    </td>
                </tr>
                <tr id="trTotal" runat="server">
                    <td style="height: 35px;">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                            style="padding: 2px;">
                            <tr>
                                <td style="padding: 2px; width: 150px;">
                                    <b>Total PO Amount : </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblCost" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 2px">
                                    <b>Previously Paid Amount : </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblPaid" runat="server">0.00</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 2px">
                                    <b>Paid Amount : </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblReAmount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 2px">
                                    <b>Selected PO Number : </b>
                                </td>
                                <td>
                                    <asp:Label ID="lblorder" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; height: 30px; padding: 2px; padding-right: 4px;">
                        <asp:ImageButton ID="btnSaveChanges" Visible="false" runat="server" Style="vertical-align: middle"
                            OnClientClick="return checkCount();" OnClick="btnSaveChanges_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
