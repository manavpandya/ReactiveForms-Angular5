<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckOutCustomerQuote.aspx.cs"
    Inherits="Solution.UI.Web.CheckOutCustomerQuote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/general.js" type="text/javascript"></script>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="js/BubbleToolTips.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OpenCenterWindow(pid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(pid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
    </script>
    <script src="js/jquery-1.3.2.js" type="text/javascript" language="javascript"> </script>
    <script src="js/CheckOutValidation.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtCardNumber').bind('copy paste', function (e) {
                e.preventDefault();
            });

        });

    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div id="doc-width">
        <div id="content-width" style="padding-top: 5px;">
            <div class="title-box">
                <h1 class="new">
                    Checkout </span>
                </h1>
            </div>
            <div class="checkout-content-left">
                <%--  <div class="checkout-content-title">
                    <p>
                        Checkout
                    </p>
                </div>--%>
                <div class="checkout-content-box">
                    <table id="mainTable" width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                    id="tblBillAddEntry" runat="server">
                                    <tr>
                                        <th colspan="3">
                                            <span class="img-left">Billing Address</span> <strong class="required-fields"><span
                                                class="required-red">*</span>Required Fields</strong>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td height="35" valign="middle">
                                            <span class="required-red">*</span>First Name
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="middle">
                                            <span class="img-left">
                                                <asp:TextBox ID="txtBillFirstname" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                            <span class="img-right"><span class="required-red">*</span>Last Name &nbsp;&nbsp; :
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtBillLastname" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtBillAddressLine1" runat="server" CssClass="checkout-text-add"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="required-red">&nbsp;</span>Address Line 2
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtBillAddressLine2" runat="server" CssClass="checkout-text-add"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="top">
                                            <span class="img-left">
                                                <asp:TextBox ID="txtBillSuite" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                            <span class="img-right"><span class="required-red">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                <asp:TextBox ID="txtBillCity" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <span class="required-red">*</span>Country
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="middle">
                                            <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="select-box" Style="width: 185px;"
                                                OnSelectedIndexChanged="ddlBillcountry_SelectedIndexChanged" onchange="javascript:document.getElementById('prepage').style.display = '';"
                                                AutoPostBack="true">
                                                <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <span class="required-red">*</span>State/Province
                                        </td>
                                        <td valign="top" align="right">
                                            :
                                        </td>
                                        <td valign="middle" style="padding: 0px;">
                                            <table>
                                                <tr>
                                                    <td style="padding: 0px;">
                                                        <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="select-box" Style="width: 185px;"
                                                            AutoPostBack="true" onchange="javascript:document.getElementById('prepage').style.display = ''; MakeBillingOtherVisible();"
                                                            OnSelectedIndexChanged="ddlBillstate_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 0px;">
                                                        <div id="DIVBillingOther" style="display: none;">
                                                            <span class="required-red">*</span>If Others, Specify&nbsp;<asp:TextBox ID="txtBillother"
                                                                runat="server" CssClass="checkout-text"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <span class="required-red">*</span>Zip Code
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtBillZipCode" runat="server" CssClass="checkout-text" onchange="javascript:document.getElementById('prepage').style.display = '';"
                                                OnTextChanged="txtBillZipCode_TextChanged" AutoPostBack="true" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <span class="required-red">*</span>Phone
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtBillphone" runat="server" CssClass="checkout-text-phone" MaxLength="20"></asp:TextBox>
                                            <img src="/images/help.jpg" id="imgPhone" title="Your phone number is needed in case we need to contact you about your order like 123-456-7890."
                                                class="img-left" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <span class="required-red">*</span>E-Mail Address
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="middle">
                                            <asp:TextBox ID="txtBillEmail" runat="server" CssClass="checkout-text-phone"></asp:TextBox>
                                            <img src="/images/help.jpg" id="imgEmail" title="Your E-Mail address will never be sold or given to other companies."
                                                class="img-left" />
                                        </td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                    id="tblBillAddress" runat="server" visible="false">
                                    <tr>
                                        <th colspan="3">
                                            <span class="img-left">Billing Address</span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td valign="middle" colspan="4">
                                            <asp:Literal ID="ltrBillAdd" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                                id="tblShippAddressEntry" runat="server">
                                                <tr>
                                                    <th colspan="3">
                                                        Shipping Address
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="UseShippingAddress" runat="server" onchange="javascript:SetBillingShippingVisible(true);"
                                                            onclick="javascript:SetBillingShippingVisible(true);" AutoPostBack="false" Text="&nbsp;Ship to a different address"
                                                            TextAlign="Right" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 0px; padding-right: 0px;" colspan="3">
                                                        <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none-shipp"
                                                            id="pnlShippingDetails" runat="server">
                                                            <tr>
                                                                <td height="35" valign="middle">
                                                                    <span class="required-red">*</span>First Name
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:TextBox ID="txtShipFirstName" runat="server" CssClass="checkout-text img-left"></asp:TextBox>
                                                                    <span class="img-right"><span class="required-red">*</span>Last Name &nbsp;&nbsp; :
                                                                        &nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtShipLastName" runat="server" CssClass="checkout-text"></asp:TextBox>
                                                                    </span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="required-red">*</span>Address&nbsp;Line&nbsp;1
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtshipAddressLine1" runat="server" CssClass="checkout-text-add"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span class="required-red">&nbsp;</span>Address Line 2
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtshipAddressLine2" runat="server" CssClass="checkout-text-add"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtShipSuite" runat="server" CssClass="checkout-text img-left"></asp:TextBox>
                                                                    <span class="img-right"><span class="required-red">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtShipCity" runat="server" CssClass="checkout-text"></asp:TextBox></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>Country
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:DropDownList ID="ddlShipCounry" runat="server" CssClass="select-box" Style="width: 185px;"
                                                                        OnSelectedIndexChanged="ddlShipCounry_SelectedIndexChanged" onchange="javascript:document.getElementById('prepage').style.display = '';"
                                                                        AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <span class="required-red">*</span>State/Province
                                                                </td>
                                                                <td valign="top" align="right">
                                                                    :
                                                                </td>
                                                                <td valign="middle" style="padding: 0px;">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="padding: 0px;">
                                                                                <asp:DropDownList ID="ddlShipState" runat="server" CssClass="select-box" AutoPostBack="true"
                                                                                    Style="width: 185px;" onchange="javascript:document.getElementById('prepage').style.display = ''; MakeShippingOtherVisible();"
                                                                                    OnSelectedIndexChanged="ddlShipState_SelectedIndexChanged">
                                                                                    <asp:ListItem Text="Select State/Province " Value="Select State/Province ">Select State/Province </asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding: 0px;">
                                                                                <div id="DIVShippingOther" style="display: none; padding-top: 7px;">
                                                                                    <span class="required-red">*</span>If Others, Specify&nbsp;
                                                                                    <asp:TextBox ID="txtShipOther" runat="server" CssClass="checkout-text"></asp:TextBox>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>Zip Code
                                                                </td>
                                                                <td>
                                                                    :
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtShipZipCode" runat="server" CssClass="checkout-text" onchange="javascript:document.getElementById('prepage').style.display = '';"
                                                                        OnTextChanged="txtShipZipCode_TextChanged" AutoPostBack="true" MaxLength="15"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>Phone
                                                                </td>
                                                                <td>
                                                                    :
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtShipPhone" runat="server" CssClass="checkout-text-phone" MaxLength="20"></asp:TextBox>
                                                                    <img src="/images/help.jpg" title="Your phone number is needed in case we need to contact you about your order like 123-456-7890."
                                                                        class="img-left" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>E-Mail Address
                                                                </td>
                                                                <td valign="middle" align="center">
                                                                    :
                                                                </td>
                                                                <td valign="middle">
                                                                    <asp:TextBox ID="txtShipEmailAddress" runat="server" CssClass="checkout-text-phone"></asp:TextBox>
                                                                    <img src="/images/help.jpg" title="Your E-Mail address will never be sold or given to other companies."
                                                                        class="img-left" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none"
                                                id="tblShipAddress" runat="server" visible="false">
                                                <tr>
                                                    <th colspan="3">
                                                        Shipping Address
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Literal ID="ltrShipAdd" runat="server"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="99.5%" class="table-none">
                                                <tbody>
                                                    <tr>
                                                        <th>
                                                            Shipping Method
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                                            <asp:RadioButtonList ID="rdoShippingMethod" onchange="javascript:document.getElementById('prepage').style.display = '';"
                                                                runat="server" AutoPostBack="true" CssClass="shippingradio" OnSelectedIndexChanged="rdoShippingMethod_SelectedIndexChanged">
                                                                <asp:ListItem Text="UPS Ground 3-5 Days($12.61)" Value="UPS Ground 3-5 Days($12.61)"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" width="99%" class="table-none">
                                                <tbody>
                                                    <tr>
                                                        <th colspan="4" align="left">
                                                            Payment Method :
                                                            <asp:Literal ID="ltrMethodName" runat="server"></asp:Literal>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <table cellspacing="0" cellpadding="0" border="0" width="99%" id="tblpayment" runat="server">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <span class="required-red">*</span>Name on Card
                                                                    </td>
                                                                    <td>
                                                                        :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="checkout-card"></asp:TextBox>
                                                                    </td>
                                                                    <td width="23%" rowspan="2" align="center" style="padding: 0;">
                                                                        <img src="/images/card-new.png" alt="" title="" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <span class="required-red">*</span>Card Type
                                                                    </td>
                                                                    <td>
                                                                        :
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:DropDownList ID="ddlCardType" runat="server" CssClass="select-box" Style="width: 172px;">
                                                                            <asp:ListItem Text="Select Card Type" Value="Select Card Type">Select Card Type</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="37%" valign="top">
                                                                        <span class="required-red">*</span>Card Number
                                                                    </td>
                                                                    <td width="2%">
                                                                        :
                                                                    </td>
                                                                    <td width="61%" colspan="2">
                                                                        <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="16" CssClass="checkout-card"
                                                                            onkeypress="return isNumberKeyCard(event)"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <span class="required-red">*</span>Expiration Date
                                                                    </td>
                                                                    <td>
                                                                        :
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="select-box" Style="width: 75px;">
                                                                            <asp:ListItem Value="0">Month</asp:ListItem>
                                                                            <asp:ListItem Value="1">Jan(01)</asp:ListItem>
                                                                            <asp:ListItem Value="2">Feb(02)</asp:ListItem>
                                                                            <asp:ListItem Value="3">Mar(03)</asp:ListItem>
                                                                            <asp:ListItem Value="4">Apr(04)</asp:ListItem>
                                                                            <asp:ListItem Value="5">May(05)</asp:ListItem>
                                                                            <asp:ListItem Value="6">June(06)</asp:ListItem>
                                                                            <asp:ListItem Value="7">July(07)</asp:ListItem>
                                                                            <asp:ListItem Value="8">Aug(08)</asp:ListItem>
                                                                            <asp:ListItem Value="9">Sept(09)</asp:ListItem>
                                                                            <asp:ListItem Value="10">Oct(10)</asp:ListItem>
                                                                            <asp:ListItem Value="11">Nov(11)</asp:ListItem>
                                                                            <asp:ListItem Value="12">Dec(12)</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <span style="float: left">&nbsp;</span>
                                                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="select-box" Style="width: 70px;">
                                                                            <asp:ListItem Text="Year" Value="Year">Year</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <span class="required-red">*</span>Card&nbsp;security&nbsp;code&nbsp;(CSC)
                                                                    </td>
                                                                    <td>
                                                                        :
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:TextBox ID="txtCSC" runat="server" CssClass="checkout-card" Style="width: 40px;"
                                                                            onkeypress="return isNumberKeyCard(event)" MaxLength="4"></asp:TextBox>
                                                                        <a href="javascript:void(0);" class="required-red" onclick="javascript:document.getElementById('CVCImage').style.display=''; $('html, body').animate({ scrollTop: $('#footer').offset().top }, 'slow'); "
                                                                            title="What's this?">(What's this?)</a>
                                                                        <div id="CVCImage" style="position: absolute; display: none; z-index: 1; background-color: #fff;
                                                                            border: solid 1px #e0dfdf; padding-top: 5px; padding-bottom: 20px; padding-left: 20px;
                                                                            width: 460px; padding-right: 20px;">
                                                                            <div style="float: right;">
                                                                                <a href="javascript:void(0);" onclick="javascript:document.getElementById('CVCImage').style.display='none';">
                                                                                    Close</a></div>
                                                                            <br />
                                                                            <img style="z-index: 1;" src="/images/verificationnumber.gif" alt="" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                                <tbody>
                                                    <tr>
                                                        <th>
                                                            Order Notes (Optional)
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtOrderNotes" runat="server" TextMode="MultiLine" CssClass="order-review-text-box"
                                                                cols="25" Rows="5"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 980px">
                        <tr>
                            <td style="width: 490px">
                                <div class="place-order-box-left">
                                    <asp:ImageButton ID="btnPlaceOrder" runat="server" alt="PLACE ORDER" title="PLACE ORDER"
                                        ImageUrl="/images/place-order.jpg" OnClick="btnPlaceOrder_Click" CssClass="img-right" /></div>
                            </td>
                            <td style="width: 490px">
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="checkout-content-right">
                <%--  <div class="checkout-content-right-title">
                    <h2>
                        Order Summary</h2>
                </div>--%>
                <div class="checkout-right-box">
                    <asp:Literal ID="ltrCart" runat="server"></asp:Literal>
                </div>
            </div>
            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
                top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
                height: 100%; width: 100%; z-index: 1000; display: none;">
                <table width="100%" style="padding-top: 400px;" align="center">
                    <tr>
                        <td align="center" style="color: #fff;" valign="middle">
                            <img alt="" src="/images/loding.png" /><br />
                            <b>Loading ... ... Please wait!</b>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="display: none;">
                <input type="hidden" id="hdncountry" runat="server" value="" />
                <input type="hidden" id="hdnState" runat="server" value="" />
                <input type="hidden" id="hdnZipCode" runat="server" value="" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
