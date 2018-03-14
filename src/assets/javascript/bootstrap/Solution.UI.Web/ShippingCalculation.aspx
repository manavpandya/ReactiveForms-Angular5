<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShippingCalculation.aspx.cs"
    Inherits="Solution.UI.Web.ShippingCalculation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <script type="text/javascript" src="/js/jquery-alerts.js"></script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
    <script type="text/javascript">
        function isNumberKeyCard(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script type="text/javascript">
        function checkFields() {

            if (document.getElementById('<%=ddlcountry.ClientID %>') != null && document.getElementById('<%=ddlcountry.ClientID %>').selectedIndex == 0) {
                jAlert('Please select Country.', 'Required Information', '<%=ddlcountry.ClientID %>');
                // document.getElementById('<%=ddlcountry.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtZipCode.ClientID %>') != null && document.getElementById('<%=txtZipCode.ClientID %>').value.replace(/^\s+|\s+$/g, "") == '') {
                jAlert('Please enter Zip Code.', 'Required Information', '<%=txtZipCode.ClientID %>');
                //document.getElementById('<%=txtZipCode.ClientID %>').focus();
                return false;
            }

            document.getElementById("prepage").style.display = 'block';
            return true;
        }

        function getprice() {
            if (window.parent.document.getElementById('subtotal') != null) {
                if (parseFloat(window.parent.document.getElementById('subtotal').innerHTML.replace('$', '').replace(/,/g, '')) > parseFloat(0)) {
                    document.getElementById('hdnprice').value = window.parent.document.getElementById('subtotal').innerHTML.toString().replace('$', '').replace(/,/g, '');
                }
                else {
                    if (window.parent.document.getElementById('spnYourPrice') != null) {
                        document.getElementById('hdnprice').value = window.parent.document.getElementById('spnYourPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
                    }
                    else if (window.parent.document.getElementById('spnRegularPrice') != null) {
                        document.getElementById('hdnprice').value = window.parent.document.getElementById('spnRegularPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
                    }
                }
            }
            else {
                if (window.parent.document.getElementById('spnYourPrice') != null) {
                    document.getElementById('hdnprice').value = window.parent.document.getElementById('spnYourPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
                }
                else if (window.opener.document.getElementById('spnRegularPrice') != null) {
                    document.getElementById('hdnprice').value = window.parent.document.getElementById('spnRegularPrice').innerHTML.toString().replace('$', '').replace(/,/g, '');
                }
            }

        }
    </script>
</head>
<body onload="getprice();" style="background: none;">
    <form id="form1" runat="server">
    <div class="td-broder" style="vertical-align: top; margin: 10px; margin-bottom: 5px;">
        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="border-none">
            <tbody>
                <tr>
                    <th align="left" colspan="3" style="color: #000000; background-color: #F1F1F1">
                        Calculate Shipping <span style="float: right; text-decoration: none; height: 24px;
                            margin-top: 2px;"><a style="text-decoration: none;" href="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();">
                                <img alt="Close" title="Close" src="/images/remove-minicart.png" border="0" /></a></span>
                    </th>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <span class="required-red">*</span>Required Fields
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span class="required-red">*</span>Country
                    </td>
                    <td align="left" style="padding: 0pt;">
                        :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlcountry" runat="server" CssClass="select-box" Style="width: 212px;
                            background: none; color: black">
                            <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <span class="required-red">*</span>Zip Code
                    </td>
                    <td align="left" style="padding: 0pt;">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="promo-code-texfild" Style="background: none"
                            MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        &nbsp;
                    </td>
                    <td align="left">
                        <asp:ImageButton ID="btnSubmit" runat="server" alt="SUBMIT" title="Submit" ImageUrl="/images/submit-button.jpg"
                            OnClientClick="return checkFields();" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left">
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Literal ID="ltrshipping" runat="server"></asp:Literal>
                    </td>
                </tr>
            </tbody>
        </table>
        <%--  <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
            top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
            height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%" style="padding-top: 80px;" align="center">
                <tr>
                    <td align="center" style="color: #fff;" valign="middle">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>--%>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
            top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
            height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%" style="padding-top: 80px;" align="center">
                <tr>
                    <td align="center" style="color: #fff;" valign="middle">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <input id="hdnprice" runat="server" value="0" />
        </div>
    </div>
    </form>
</body>
</html>
