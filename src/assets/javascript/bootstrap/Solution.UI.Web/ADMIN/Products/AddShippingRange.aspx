<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddShippingRange.aspx.cs" MasterPageFile="~/ADMIN/Admin.Master" Inherits="Solution.UI.Web.ADMIN.Products.AddShippingRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript">
        function Checkfields() {
            var fromprice = document.getElementById('ContentPlaceHolder1_txtfromprice').value;
            var toprice = document.getElementById('ContentPlaceHolder1_txttoprice').value;
            if (document.getElementById('ContentPlaceHolder1_txtfromprice').value == "") {
                jAlert('Please enter From Price.', 'Message', 'ContentPlaceHolder1_txtfromprice');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txttoprice').value == "") {
                jAlert('Please enter To Price.', 'Message', 'ContentPlaceHolder1_txttoprice');
                return false;
            }
            else if (parseFloat(fromprice) > parseFloat(toprice)) {
                jAlert('Please enter TO Price bigger than From Price', 'Message', 'ContentPlaceHolder1_txttoprice');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtprice').value == "") {
                jAlert('Please enter Discount Price.', 'Message', 'ContentPlaceHolder1_txtprice');
                return false;
            }
            //else {

            //    $.ajax({
            //        async: false,
            //        url: '/ADMIN/Products/AddShippingRange.aspx/checkdata',
            //        type: 'POST',
            //        dataType: 'json',
            //        data: "{fromprice: '" + document.getElementById("ContentPlaceHolder1_txtfromprice").value + "',toprice: '" + document.getElementById("ContentPlaceHolder1_txttoprice").value + "'}",
            //        contentType: 'application/json; charset=utf-8',
            //        success: function (fromnotexist) {
            //            alert(d.fromnotexist);
            //            return false;
            //        }
            //    });
            //}
            //return false;
        }
        $(function () {
            $('#ContentPlaceHolder1_txtfromprice').keydown(function (e) {
                if (e.shiftKey || e.ctrlKey || e.altKey) {
                    e.preventDefault();
                } else {
                    var key = e.keyCode;
                    if (!((key == 8) || (key == 9) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key == 190) || (key == 110))) {
                        e.preventDefault();
                    }
                }
            });
            $('#ContentPlaceHolder1_txttoprice').keydown(function (e) {
                if (e.shiftKey || e.ctrlKey || e.altKey) {
                    e.preventDefault();
                } else {
                    var key = e.keyCode;
                    if (!((key == 8) || (key == 9) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key == 190) || (key == 110))) {
                        e.preventDefault();
                    }
                }
            });



        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
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
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Add Shipping Range" alt="Add Shipping Range" src="/App_Themes/<%=Page.Theme %>/Images/shipping-services-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Add Shipping Price Range" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                        </th>
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
                                                    <td><span class="star">*</span>$ From Price:</td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtfromprice" CssClass="order-textfield"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">*</span>$ To Price:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txttoprice" CssClass="order-textfield"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td>
                                                        <span class="star">*</span>$ Price (Discount) :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtprice" CssClass="order-textfield"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Is Active:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chactive" runat="server" />
                                                    </td>
                                                </tr>


                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClientClick="return Checkfields();" OnClick="imgSave_Click" />
                                                        &nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel" OnClick="imgCancel_Click" />
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
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
