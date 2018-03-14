<%@ Page Title="CreateAccount" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateAccount.aspx.cs" Inherits="Solution.UI.Web.CreateAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <script type="text/javascript" src="/js/jquery-alerts-main.js"></script>
    <link rel="stylesheet" type="text/css" href="/css/jquery.alerts.css" />
    <script src="/js/jquery-alerts.js" type="text/javascript"></script>
    <script language="javascript" src="js/CreateAccountValidate.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>
    <script type="text/javascript">
        function ReloadCapthca() {
            document.getElementById('ContentPlaceHolder1_txtCodeshow').value = '';

            var chars = "0123456789";
            var string_length = 8;
            var randomstring = '';
            for (var i = 0; i < string_length; i++) {
                var rnum = Math.floor(Math.random() * chars.length);
                randomstring += chars.substring(rnum, rnum + 1);
            }

            document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
            document.getElementById('ContentPlaceHolder1_txtCodeshow').focus();
        }

        function keyRestrict(e, validchars) {

            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                return true;
            return false;
        }
        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }
    </script>

    <script type="text/javascript">
        function checkEmailAd() {
            document.getElementById('popup_container').style.visibility = 'visible';
        }
        function checyes() {
            document.getElementById("ContentPlaceHolder1_hdnyes").value = '1';
            document.getElementById("ContentPlaceHolder1_Button2").click();
        }


    </script>
    <div id="popup_container" style="position: fixed; visibility: hidden; z-index: 1000000; padding: 0px; margin: 0px; min-width: 604px; max-width: 604px; top: 275px; left: 487.5px;">
        <h1 id="popup_title">Confirmation</h1>
        <div id="popup_content" class="confirm2">
            <div id="popup_message" style="color:#641114">This Email address is available in our records. Please click on below button to get new password.</div>
            <div id="popup_panel"><a href="javascript:void(0);" id="popup_ok" onclick="javascript:checyes();" style="margin-left: 0;"><strong><span style="margin-left: 15px; margin-right: 15px;">Reset Password</span></strong></a> &nbsp;&nbsp;&nbsp; </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hdnyes" runat="server" value="0" />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
    </div>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span id="spanbreadcrmbs" runat="server">Register</span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span id="h1tag" runat="server">Register </span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <asp:Panel ID="pblcontactus" runat="server" DefaultButton="btnFinish">
                    <table id="mainTable" cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td colspan="3" valign="top">
                                    <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                        <tbody>
                                            <tr>
                                                <th colspan="5">Email &amp; Password
                                                </th>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <span class="required-red">*</span>Email
                                                </td>
                                                <td valign="middle" align="center">:
                                                </td>
                                                <td style="width: 260px">
                                                    <asp:TextBox ID="txtUsername" TabIndex="1" runat="server" CssClass="checkout-textfild"
                                                        MaxLength="100" Width="200px"></asp:TextBox>
                                                    <a title="" href="#">
                                                        <img src="/images/help.jpg" alt="help" title="Special characters such as * &amp; ! are not allowed."
                                                            align="absmiddle" />
                                                    </a>

                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgbtnavaibility" runat="server" alt="Check Avaibility"
                                                        title="Check Avaibility" ImageUrl="/images/check-availability.png" OnClientClick="return ValidatePageUser();"
                                                        TabIndex="35" OnClick="imgbtnavaibility_Click" />
                                                </td>
                                                <td width="24%" align="right">
                                                    <span class="required-red">*</span>Required Fields
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">*</span>Password
                                                </td>
                                                <td valign="middle" align="center">:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtpassword" TabIndex="2" TextMode="Password" OnPreRender="txtpassword_PreRender"
                                                        MaxLength="10" runat="server" CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                                                    <a title="help" href="#">
                                                        <img src="/images/help.jpg" alt="help" title="Password must be at least 6 characters."
                                                            align="absmiddle" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">*</span>Confirm Password
                                                </td>
                                                <td valign="middle" align="center">:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtconfirmpassword" TabIndex="3" TextMode="Password" OnPreRender="txtconfirmpassword_PreRender"
                                                        MaxLength="10" runat="server" CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none">
                                        <tr>
                                            <th colspan="3">
                                                <span class="img-left">Billing Address</span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td height="35" valign="middle" width="20%">
                                                <span class="required-red">*</span>First Name
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="middle">
                                                <span style="float: left;">
                                                    <asp:TextBox ID="txtBillFirstname" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                                        Width="200px" TabIndex="4"></asp:TextBox></span><%-- <span class="register-right-field"
                                                            style="padding: 0 5px 0 0"><span class="required-red">*</span>Last Name &nbsp;&nbsp;
                                                            : &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtBillLastname" runat="server" CssClass="checkout-textfild" Width="200px"
                                                                TabIndex="5"></asp:TextBox>
                                                        </span>--%>
                                                <span class="register-right-field" style="padding: 0 5px 0 0px; margin-left: 0px;"><span
                                                    class="required-red" style="padding-left: 5px;">*</span><font style="margin-right: 15px">Last
                                                            Name</font>: &nbsp;
                                                        <asp:TextBox ID="txtBillLastname" runat="server" onkeypress="return isNumberKey(event)"
                                                            CssClass="checkout-textfild" Width="200px" TabIndex="5" Style="margin-left: 10px"></asp:TextBox>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Company
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBillingCompany" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                                    Width="200px" TabIndex="6"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>Address Line 1
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBilladdressLine1" MaxLength="500" runat="server" CssClass="checkout-textfild"
                                                    Width="200px" TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Address Line 2
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBillAddressLine2" MaxLength="500" runat="server" CssClass="checkout-textfild"
                                                    Width="200px" TabIndex="8"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="top">
                                                <span style="float: left;">
                                                    <asp:TextBox ID="txtBillsuite" runat="server" MaxLength="100" CssClass="checkout-text"
                                                        TabIndex="9"></asp:TextBox></span>
                                                <%--<span class="register-right-field" style="padding: 0 5px 0 0;
                                                            margin-left: 100px;"><span class="required-red">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtBillCity" runat="server" onkeypress="return isNumberKey(event)"
                                                                CssClass="checkout-textfild" Style="margin-left: 45px;" Width="200px" TabIndex="11"></asp:TextBox>
                                                        </span>--%>
                                                <span class="register-right-field" style="padding: 0 5px 0 5px; margin-left: 106px;">
                                                    <span class="required-red">*</span><font style="margin-right: 62px">City</font>:
                                                        &nbsp;
                                                        <asp:TextBox ID="txtBillCity" Style="margin-left: 10px" runat="server" onkeypress="return isNumberKey(event)"
                                                            CssClass="checkout-textfild" Width="200px" TabIndex="10"></asp:TextBox>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <span class="required-red">*</span>Country
                                            </td>
                                            <td valign="top" align="right">:
                                            </td>
                                            <td valign="middle">
                                                <span style="float: left;">
                                                    <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="select-box" Style="width: 212px;"
                                                        OnSelectedIndexChanged="ddlBillcountry_SelectedIndexChanged" AutoPostBack="true"
                                                        TabIndex="11">
                                                        <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <%--<span class="register-right-field" style="padding: 0 5px 0 0"><span class="required-red">
                                                    *</span>State/Province &nbsp;&nbsp; : &nbsp;&nbsp;
                                                    <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="select-box1" Style="width: 208px;"
                                                        onchange="MakeBillingOtherVisible();" TabIndex="13">
                                                    </asp:DropDownList>
                                                </span>--%>
                                                <span class="register-right-field" style="padding: 0 5px 0 0px"><span class="required-red">*</span>State/Province&nbsp; : &nbsp;
                                                        <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="select-box" Style="width: 211px; margin-left: 8px; float: none"
                                                            onchange="MakeBillingOtherVisible();" TabIndex="12">
                                                        </asp:DropDownList>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <span class="required-red">*</span>Zip Code
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtBillZipCode" runat="server" CssClass="checkout-textfild" Width="200px"
                                                    TabIndex="13" MaxLength="15"></asp:TextBox>
                                                <span class="register-right-field" style="padding: 0pt 13px 0pt 0pt; float: right; text-align: left; margin-right: 41px;">
                                                    <div id="DIVBillingOther" style="display: none;">
                                                        <span class="required-red">*</span>If Others, Specify&nbsp;:
                                                            <asp:TextBox ID="txtBillingOtherState" TabIndex="14" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                                runat="server" CssClass="checkout-textfild" Width="78px"></asp:TextBox>
                                                        <asp:Label ID="lblBRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                    </div>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <span class="required-red">*</span>Phone
                                            </td>
                                            <td>:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBillphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                    runat="server" CssClass="checkout-text-phone" Width="200px" TabIndex="15"></asp:TextBox>
                                                <img src="/images/help.jpg" alt="help" title="Your phone number is needed in case we need to contact
                                                    you about your order like 123-456-7890."
                                                    class="img-left" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Fax
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtBillingFax" MaxLength="20" runat="server" CssClass="checkout-textfild"
                                                    onkeypress="return onKeyPressPhone(event);" Width="200px" TabIndex="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <span class="required-red">*</span>E-Mail Address
                                            </td>
                                            <td valign="middle" align="center">:
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtBillEmail" runat="server" MaxLength="100" CssClass="checkout-text-phone"
                                                    Width="200px" TabIndex="17"></asp:TextBox>
                                                <img src="/images/help.jpg" alt="help" title="Your E-Mail address will never be sold or given to other companies."
                                                    class="img-left" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none">
                                        <tr>
                                            <th>Shipping Address
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="UseShippingAddress" runat="server" EnableViewState="false" TabIndex="18"
                                                    onclick="javascript:SetBillingShippingVisible();" Checked="false" />
                                                <asp:Label ID="UseShippingAddressLabel" runat="server" AssociatedControlID="UseShippingAddress"
                                                    Text="Ship to a different address" EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="pnlShippingDetails" style="display: none;" runat="server" cellpadding="0"
                                                    cellspacing="0" width="100%">
                                                    <tr>
                                                        <td width="20%" valign="middle">
                                                            <span class="required-red">*</span>First Name
                                                        </td>
                                                        <td align="center" valign="middle">:
                                                        </td>
                                                        <td valign="middle" style="width: 90%;">
                                                            <span style="float: left;">
                                                                <asp:TextBox ID="txtShipFirstname" TabIndex="19" MaxLength="50" runat="server" CssClass="checkout-textfild"
                                                                    Width="200px"></asp:TextBox></span><%-- <span class="register-right-field" style="padding: 0px">
                                                                        <span class="required-red">*</span>Last Name &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtShipLastname" MaxLength="50" runat="server" CssClass="checkout-textfild"
                                                                            Width="200px"></asp:TextBox>
                                                                    </span>--%>
                                                            <span class="register-right-field" style="padding: 0 5px 0 0px; margin-left: 3px;"><span
                                                                class="required-red" style="padding-left: 5px;">*</span><font style="margin-right: 15px">Last
                                                                        Name</font>: &nbsp;
                                                                    <asp:TextBox ID="txtShipLastname" runat="server" onkeypress="return isNumberKey(event)"
                                                                        CssClass="checkout-textfild" Width="200px" TabIndex="20"></asp:TextBox>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="required-red">&nbsp;</span>Company
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:TextBox ID="txtShippingCompany" TabIndex="21" MaxLength="100" runat="server"
                                                                CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="required-red">*</span>Address Line 1
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:TextBox ID="txtShipAddressLine1" TabIndex="22" MaxLength="500" runat="server"
                                                                CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="required-red">&nbsp;</span>Address Line 2
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:TextBox ID="txtshipAddressLine2" TabIndex="23" MaxLength="500" runat="server"
                                                                CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="top">
                                                            <span style="float: left;">
                                                                <asp:TextBox ID="txtShipSuite" MaxLength="100" TabIndex="24" runat="server" CssClass="checkout-text img-left"></asp:TextBox></span>
                                                            <%-- <span class="register-right-field" style="padding: 0px"><span class="required-red"
                                                                style="margin-left: 100px;">*</span>City &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtShipCity" runat="server" onkeypress="return isNumberKey(event)"
                                                                    CssClass="checkout-textfild" Style="margin-left: 45px;" Width="200px"></asp:TextBox>
                                                            </span>--%>
                                                            <span class="register-right-field" style="padding: 0 5px 0 12px; margin-left: 106px;">
                                                                <span class="required-red">*</span><font style="margin-right: 62px">City</font>:
                                                                    &nbsp;
                                                                    <asp:TextBox ID="txtShipCity" Style="margin-left: 0px;" runat="server" onkeypress="return isNumberKey(event)"
                                                                        CssClass="checkout-textfild" Width="200px" TabIndex="25"></asp:TextBox>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <span class="required-red">*</span>Country
                                                        </td>
                                                        <td valign="top" align="right">:
                                                        </td>
                                                        <td valign="middle">
                                                            <span style="float: left;">
                                                                <asp:DropDownList ID="ddlShipCounry" TabIndex="26" runat="server" CssClass="select-box"
                                                                    Style="width: 212px;" OnSelectedIndexChanged="ddlShipCounry_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                    <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </span>
                                                            <%--<span class="register-right-field" style="padding: 0px;"><span class="required-red">
                                                                *</span>State/Province &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                <asp:DropDownList ID="ddlShipState" runat="server" CssClass="select-box1" Style="width: 208px;"
                                                                    onchange="MakeShippingOtherVisible()">
                                                                </asp:DropDownList>
                                                            </span>--%>
                                                            <span class="register-right-field" style="padding: 0 5px 0 6px"><span class="required-red">*</span>State/Province&nbsp; : &nbsp;
                                                                    <asp:DropDownList ID="ddlShipState" runat="server" CssClass="select-box" Style="width: 211px; margin: 0px; float: none"
                                                                        onchange="MakeShippingOtherVisible();" TabIndex="27">
                                                                    </asp:DropDownList>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <span class="required-red">*</span>Zip Code
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:TextBox ID="txtShipZipCode" TabIndex="28" runat="server" CssClass="checkout-textfild"
                                                                Width="200px" MaxLength="15"></asp:TextBox>
                                                            <span class="register-right-field" style="padding: 0 13px 0 0; float: right; text-align: left; margin-right: 41px;">
                                                                <div id="DIVShippingOther" style="display: none;">
                                                                    <span class="required-red">*</span>If Others, Specify :
                                                                        <asp:TextBox ID="txtShippingOtherState" TabIndex="29" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                                            runat="server" CssClass="checkout-textfild" Width="78px"></asp:TextBox>
                                                                    <asp:Label ID="lblSRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                                </div>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <span class="required-red">*</span>Phone
                                                        </td>
                                                        <td>:
                                                        </td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtShipPhone" TabIndex="30" MaxLength="20" runat="server" onkeypress="return onKeyPressPhone(event);"
                                                                CssClass="checkout-text-phone" Width="200px"></asp:TextBox>
                                                            <img src="/images/help.jpg" alt="help" title="Your phone number is needed in case we need to contact
                                                    you about your order like (123-456-7890)."
                                                                class="img-left" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="required-red">&nbsp;</span>Fax
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txtShippingFax" TabIndex="31" MaxLength="20" runat="server" CssClass="checkout-textfild"
                                                                onkeypress="return onKeyPressPhone(event);" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle">
                                                            <span class="required-red">*</span>E-Mail Address
                                                        </td>
                                                        <td valign="middle" align="center">:
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:TextBox ID="txtShipEmailAddress" TabIndex="32" MaxLength="100" runat="server"
                                                                CssClass="checkout-text-phone" Width="200px"></asp:TextBox>
                                                            <img src="/images/help.jpg" alt="help" title="Your E-Mail address will never be sold or given to other
                                                    companies."
                                                                class="img-left" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" valign="top">
                                    <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                        <tbody>
                                            <tr>
                                                <th colspan="3" align="left">Verification
                                                </th>
                                            </tr>
                                            <tr>
                                                <td width="20%" align="left" valign="middle">&nbsp;&nbsp;Verification
                                                </td>
                                                <td align="left" valign="middle">:
                                                </td>
                                                <td align="left">
                                                    <img width="150px" height="40px" class="img-left" id="imgcapcha" alt="" src="/JpegImage.aspx?id=343343" />
                                                    <%-- <a title="Reload" href="#" class="reload-icon" onclick="ReloadCapthca();">
                                                    <img class="reload-btn" title="Reload" alt="Reload" src="/images/reload-icon.png" /></a>--%>
                                                    <input tabindex="33" type="button" value="" title="Reload" id="btnreload" style="background: url(/images/reload-icon.png) no-repeat transparent; width: 31px; height: 29px; border: none; cursor: pointer; margin: 8px 0 0 5px;"
                                                        onclick="ReloadCapthca();" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span class="required-red">*</span>Enter the code shown
                                                </td>
                                                <td align="left">:
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCodeshow" runat="server" CssClass="checkout-textfild" Style="width: 118px;"
                                                        TabIndex="34"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;
                                                </td>
                                                <td valign="middle" align="left">
                                                    <asp:ImageButton ID="btnContinueCheckout" runat="server" alt="CONTINUE CHECKOUT"
                                                        title="CONTINUE CHECKOUT" ImageUrl="/images/continue-checkout.png" OnClientClick="return ValidatePage();"
                                                        TabIndex="35" OnClick="btnContinueCheckout_Click" />
                                                    <asp:ImageButton ID="btnFinish" runat="server" alt="FINISH" title="FINISH" ImageUrl="/images/finish.png"
                                                        OnClientClick="return ValidatePage();" OnClick="btnFinish_Click" TabIndex="36" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>

</asp:Content>
