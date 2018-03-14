<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddnewAddress.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.AddnewAddress1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
            <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
             <script type="text/javascript" src="/js/BubbleTooltips.js"></script>
         
            <script type="text/javascript">
                window.onload = function () { enableTooltips('mainTable') };


                // JScript File

                /* Start - General Function for restriction */

                var testresults
                function checkemail1(str) {
                    var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
                    if (filter.test(str))
                        testresults = true
                    else {
                        testresults = false
                    }
                    return (testresults)
                }

                function onKeyPressPhone(e) {


                    var key = window.event ? window.event.keyCode : e.which;

                    if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45 || key == 88 || key == 17 || key == 120) {
                        return key;
                    }

                    var keychar = String.fromCharCode(key);

                    var reg = /\d/;
                    if (window.event)
                        return event.returnValue = reg.test(keychar);
                    else
                        return reg.test(keychar);

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

                function keyRestrict(e, validchars) {
                    var key = '', keychar = '';
                    key = getKeyCode(e);

                    if (key == null) return true;
                    keychar = String.fromCharCode(key);
                    keychar = keychar.toLowerCase();
                    validchars = validchars.toLowerCase();
                    if (validchars.indexOf(keychar) != -1)
                        return true;

                    if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 104)
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

                function isNumberKey(event) {
                    var retval = false;
                    var charCode = (event.keyCode) ? event.keyCode : event.which;
                    if (charCode > 31 && (charCode < 33 || charCode > 64) && (charCode < 91 || charCode > 96) && (charCode < 123 || charCode > 126))
                        retval = true;
                    if (charCode == 8 || (charCode > 35 && charCode < 41) || charCode == 46 || charCode == 9)
                        retval = true;
                    if (navigator.appName.indexOf('Microsoft') != -1)
                        window.event.returnValue = retval;
                    return retval;
                }

                /* End -  General Function for restriction */

                /* Start - All fields of Page Validation function */
                function ValidatePage() {


                    // For Edit Account Details validation

                    if (document.getElementById("txtFirstname") != null && document.getElementById("txtFirstname").value == '') {

                       jAlert("Please enter First Name.");
                        document.getElementById("txtFirstname").focus();
                        return false;
                    }
                    else if (document.getElementById("txtLastname") != null && document.getElementById("txtLastname").value == '') {

                       jAlert("Please enter Last Name.");
                        document.getElementById("txtLastname").focus();
                        return false;
                    }
                    else if (document.getElementById("txtaddressLine1") != null && document.getElementById("txtaddressLine1").value == '') {

                       jAlert("Please enter Address Line 1.");
                        document.getElementById("txtaddressLine1").focus();
                        return false;
                    }
                    else if (document.getElementById("txtCity") != null && document.getElementById("txtCity").value == '') {

                       jAlert("Please enter City.");
                        document.getElementById("txtCity").focus();
                        return false;
                    }
                    else if (document.getElementById("ddlcountry") != null && document.getElementById("ddlcountry").selectedIndex == 0) {

                       jAlert("Please select Country.");
                        document.getElementById("ddlcountry").focus();
                        return false;
                    }
                    else if (document.getElementById("ddlstate") != null && document.getElementById("ddlstate").selectedIndex == 0) {

                       jAlert("Please select State.");
                        document.getElementById("ddlstate").focus();
                        return false;
                    }
                    else if (document.getElementById("ddlstate") != null && document.getElementById("ddlstate").options[document.getElementById("ddlstate").selectedIndex].value == '-11') {

                        if (document.getElementById("txtOtherState") != null && document.getElementById("txtOtherState").value == '') {

                           jAlert("Please enter Other State.");
                            document.getElementById("txtOtherState").focus();
                            return false;
                        }
                    }
                    else if (document.getElementById("txtZipCode") != null && document.getElementById("txtZipCode").value == '') {

                       jAlert("Please enter Zip Code.");
                        document.getElementById("txtZipCode").focus();
                        return false;
                    }
                    else if (document.getElementById("txtphone") != null && document.getElementById("txtphone").value == '') {

                       jAlert("Please enter Phone.");
                        document.getElementById("txtphone").focus();
                        return false;
                    }
                    else if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value == '') {

                       jAlert("Please enter Email.");
                        document.getElementById("txtEmail").focus();
                        return false;
                    }

                    else if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value != '' && !checkemail1(document.getElementById("txtEmail").value)) {

                       jAlert("Please enter valid Email.");
                        document.getElementById("txtEmail").focus();
                        return false;
                    }
                    return true;
                }

                /* End - All fields of Page Validation function */


                /* Start -  Make Other State Field visible True or false according to condition*/
                function SetOtherVisible(IsVisible) {
                    if (IsVisible) {
                        if (document.getElementById('DIVOther') != null) { document.getElementById('DIVOther').style.display = 'block'; }
                    }
                    else {

                        if (document.getElementById('ctl00_txtOtherState') != null) { document.getElementById('ctl00_txtOtherState').value = ''; }
                        if (document.getElementById('DIVOther') != null) { document.getElementById('DIVOther').style.display = 'none'; }

                    }
                }

                function MakeOtherVisible() {
                    if (document.getElementById('ddlstate') != null && document.getElementById('ddlstate').options[document.getElementById('ddlstate').selectedIndex].value == '-11')
                        SetOtherVisible(true);
                    else
                        SetOtherVisible(false);
                }


                /* End -  Make Other State Field visible True or false according to condition*/


            </script>


            <div id="content-width">

                <div class="content-row2">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">


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
                                                            <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                            <h2>
                                                                <asp:Label ID="lblHeader" runat="server" Text="Customer"></asp:Label></h2>
                                                        </div>
                                                        <div style="float: right; margin-right: 20px;">


                                                            <asp:ImageButton ID="imgClosee" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();return false;"
                                                                ImageUrl="~/images/Close.png" runat="server" ToolTip="Close"></asp:ImageButton>



                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr class="altrow">
                                                    <td align="right">
                                                        <span class="star">*</span>Required Field
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td style="background-color: #E3E3E3; font-weight: bold;">Security Information
                                                    </td>
                                                </tr>
                                                <tr class="even-row">
                                                    <td>

                                                        <table width="100%" border="0" style="padding-left: 10px;" cellpadding="0" cellspacing="0"
                                                            id="mainTable">

                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="required-red">*</span>First Name
                                                                </td>

                                                                <td>
                                                                    <asp:TextBox ID="txtFirstname" MaxLength="100" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td>
                                                                    <span class="required-red">*</span>Last Name
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtLastname" runat="server" CssClass="order-textfield" Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="required-red">&nbsp;</span>Company
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtCompany" MaxLength="100" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td>
                                                                    <span class="required-red">*</span>Address Line 1
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtaddressLine1" MaxLength="500" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="required-red">&nbsp;</span>Address Line 2
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtAddressLine2" MaxLength="500" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td valign="middle">
                                                                    <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtsuite" runat="server" Width="200px" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="required-red">*</span>City
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtCity" runat="server" CssClass="order-textfield" Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td valign="top">
                                                                    <span class="required-red">*</span>Country
                                                                </td>

                                                                <td valign="middle">
                                                                    <asp:DropDownList ID="ddlcountry" runat="server" CssClass="select-box" Style="width: 212px;"
                                                                        OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>
                                                                    <span class="required-red">*</span>State/Province
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:DropDownList ID="ddlstate" runat="server" CssClass="select-box" Style="width: 212px;"
                                                                        onchange="MakeOtherVisible();">
                                                                    </asp:DropDownList>
                                                                    <div id="DIVOther" style="display: none; padding-top: 5px;">
                                                                        <span class="required-red">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                    <asp:TextBox ID="txtOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                        runat="server" CssClass="order-textfield" Width="78px"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>Zip Code
                                                                </td>

                                                                <td valign="middle">
                                                                    <asp:TextBox ID="txtZipCode" runat="server" CssClass="order-textfield" Width="200px"
                                                                        MaxLength="15"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>Phone
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                                        runat="server" CssClass="order-textfield" Width="200px"></asp:TextBox>
                                                                    <img src="/images/help.jpg" alt="help" title="Your phone number is needed in case we need to contact
                                                    you about your order like (123-456-7890)."
                                                                        />
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td>
                                                                    <span class="required-red">&nbsp;</span>Fax
                                                                </td>

                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtFax" MaxLength="20" runat="server" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td valign="middle">
                                                                    <span class="required-red">*</span>E-Mail Address
                                                                </td>

                                                                <td valign="middle">
                                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="order-textfield"
                                                                        Width="200px"></asp:TextBox>
                                                                    
                                                                    <img src="/images/help.jpg" alt="help" title="Your E-Mail address will never be sold or given to other
                                                    companies."
                                                                         />
                                                                       
                                                                </td>
                                                            </tr>
                                                            <tr class="oddrow">
                                                                <td valign="middle">
                                                                    <span class="required-red">&nbsp;</span>Is Default Address
                                                                </td>

                                                                <td valign="middle">
                                                                    <asp:CheckBox ID="chkDefaultAddress" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td>&nbsp;
                                                                </td>
                                                                <td valign="middle" align="left">
                                                                    <asp:Label ID="lblmsgdefault" Visible="false" runat="server">
                                                              If You do not want to set this address as default Address ,Please select another address as Default Address from the Address Book</asp:Label>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td align="center" colspan="2">
                                                        <asp:Panel ID="pnlEditAddr" runat="server" DefaultButton="imgSave">

                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="btnFinish_Click" OnClientClick="return ValidatePage();" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" CausesValidation="false" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();return false;"/>


                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>


                </div>
            </div>
        </div>
    </form>
</body>
</html>
