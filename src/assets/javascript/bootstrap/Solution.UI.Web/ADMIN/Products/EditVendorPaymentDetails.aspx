<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditVendorPaymentDetails.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.EditVendorPaymentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">

        $(function () {
            $('#ContentPlaceHolder1_txtTransactionDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false
            });
        });

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function inputOnlyNumbers(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;
            if (charCode < 31 || (charCode > 47 && charCode < 58) || charCode == 46) {
                return true;
            }
            return false;
        }
        function RadioValidation() {
            var listItemArray = document.getElementById('ContentPlaceHolder1_rdiocouponvalidfor').getElementsByTagName('INPUT');
            var isItemChecked = false;
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    isItemChecked = true;
                }
            }
            if (isItemChecked == false) {
                jAlert('Please select a option  for Coupon Validity', 'Message');
                return false;
            }
            else {
                return true;
            }
        }

        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <asp:HiddenField ID="hfPA" Value="0" runat="server" />
        <asp:HiddenField ID="hfVPID" Value="0" runat="server" />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Vendor/DropShipper Payment" alt="Add Vendor/DropShipper Payment" src="/App_Themes/<%=Page.Theme %>/Images/coupon-discount-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Vendor/DropShipper Payment" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="even-row">
                                        <td align="center" valign="middle" style="text-align: center">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                            <span class="star">*</span>Required Field
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">*</span>Vendor/DropShipper Name :
                                                    </td>
                                                    <td style="width: 80%" colspan="2">
                                                        <asp:DropDownList ID="ddlVendorName" runat="server" CssClass="order-list" Width="226px"
                                                            AutoPostBack="True" OnSelectedIndexChanged="ddlVendorName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Paid By :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPaidBy" CssClass="order-textfield" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Transaction Reference :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtTransactionRef" CssClass="order-textfield" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">*</span>Transaction Date :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtTransactionDate" CssClass="order-textfield" Width="86px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Purchase Orders :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox Style="display: none;" ID="txtPurchaseorder" runat="server" CssClass="order-textfield"
                                                            TextMode="MultiLine" Width="293px" Height="43px"></asp:TextBox>
                                                        <asp:Label Width="250px" CssClass="textfild" Style="border: 0px;font-weight:bold;" ID="lblPurchaseorder"
                                                            runat="Server"></asp:Label> &nbsp;
                                                        <asp:ImageButton ID="btnAddPurchaseOrder" runat="server" AlternateText="Select Purchase Order"
                                                            ToolTip="Select Purchase Order" />
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Note :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtNote" Width="400px" CssClass="order-textfield"
                                                            TextMode="MultiLine" Height="50px" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>PO Amount($) :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label Width="100px" Text="0.00" CssClass="textfild" Style="border: 0px;" ID="lblPOAmount"
                                                            runat="Server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Previously Paid Amount($) :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblPreviousPaid" runat="server" Text="0.00"></asp:Label>
                                                        <asp:HiddenField ID="hfPrevPaid" runat="server" Value="0.00" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Paid Amount($) :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblPaidAmount" runat="server" Text="0.00"></asp:Label>
                                                        <asp:HiddenField ID="hPaidAmount" runat="server" Value="0.00" />
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Payment Status :
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rdpayment" runat="Server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Full Payment" Value="1" Selected="True" />
                                                            <asp:ListItem Text="Partial Payment" Value="0" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            CausesValidation="true" OnClientClick="return Checkfields();" OnClick="btnSave_Click" />
                                                        &nbsp;<asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="btnCancel_Click" />
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
                <td style="height: 10px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
