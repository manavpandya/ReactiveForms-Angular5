<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorderAddress.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.EditorderAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
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
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function Emailvalidation() {
            if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Order Email.", "Required Information", "txtEmail");
                $('html, body').animate({ scrollTop: $('#txtEmail').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value != '' && !checkemail1(document.getElementById("txtEmail").value)) {

                jAlert("Please Enter valid Order Email.", "Required Information", "txtEmail");
                $('html, body').animate({ scrollTop: $('#txtEmail').offset().top }, 'slow');
                return false;

            }
            chkHeight();
            return true;
        }

        function validationForm() {
//            if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value.replace(/^\s+|\s+$/g, "") == '') {

//                jAlert("Please Enter Order Email.", "Required Information", "txtEmail");
//                $('html, body').animate({ scrollTop: $('#txtEmail').offset().top }, 'slow');
//                return false;
//            }
//            else if (document.getElementById("txtEmail") != null && document.getElementById("txtEmail").value != '' && !checkemail1(document.getElementById("txtEmail").value)) {

//                jAlert("Please Enter valid Order Email.", "Required Information", "txtEmail");
//                $('html, body').animate({ scrollTop: $('#txtEmail').offset().top }, 'slow');
//                return false;
//            }


            if (document.getElementById("txtFirstname") != null && document.getElementById("txtFirstname").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter First Name.", "Required Information", "txtFirstname");
                $('html, body').animate({ scrollTop: $('#txtFirstname').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("txtLastname") != null && document.getElementById("txtLastname").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Last Name.", "Required Information", "txtLastname");
                $('html, body').animate({ scrollTop: $('#txtLastname').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("txtAddress1") != null && document.getElementById("txtAddress1").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Address Line 1.", "Required Information", "txtAddress1");
                $('html, body').animate({ scrollTop: $('#txtAddress1').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("txtCity") != null && document.getElementById("txtCity").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter City.", "Required Information", "txtCity");
                $('html, body').animate({ scrollTop: $('#txtCity').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("ddlCountry") != null && document.getElementById("ddlCountry").selectedIndex == 0) {

                jAlert("Please Select Country.", "Required Information", "ddlCountry");
                $('html, body').animate({ scrollTop: $('#ddlCountry').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("ddlState") != null && document.getElementById("ddlState").selectedIndex == 0) {

                jAlert("Please Select State.", "Required Information", "ddlState");
                $('html, body').animate({ scrollTop: $('#ddlState').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("ddlState") != null && document.getElementById("ddlState").options[document.getElementById("ddlState").selectedIndex].value == '-11') {

                if (document.getElementById("txtOther") != null && document.getElementById("txtOther").value.replace(/^\s+|\s+$/g, "") == '') {

                    jAlert("Please Enter Other State.", "Required Information", "txtOther");
                    $('html, body').animate({ scrollTop: $('#txtOther').offset().top }, 'slow');

                    return false;
                }
                else if (document.getElementById("txtZipcode") != null && document.getElementById("txtZipcode").value.replace(/^\s+|\s+$/g, "") == '') {

                    jAlert("Please Enter Zip Code.", "Required Information", "txtZipcode");
                    $('html, body').animate({ scrollTop: $('#txtZipcode').offset().top }, 'slow');

                    return false;
                }
                else if (document.getElementById("txtPhone") != null && document.getElementById("txtPhone").value.replace(/^\s+|\s+$/g, "") == '') {

                    jAlert("Please Enter Phone.", "Required Information", "txtPhone");
                    $('html, body').animate({ scrollTop: $('#txtPhone').offset().top }, 'slow');

                    return false;
                }
                else if (!(document.getElementById('txtPhone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
                    jAlert("Please enter valid Phone.", "Required Information", "txtPhone");
                    $('html, body').animate({ scrollTop: $('#txtPhone').offset().top }, 'slow');

                    return false;
                }
                else if (document.getElementById("txtEmailAdd") != null && document.getElementById("txtEmailAdd").value.replace(/^\s+|\s+$/g, "") == '') {

                    jAlert("Please Enter Email.", "Required Information", "txtEmailAdd");
                    $('html, body').animate({ scrollTop: $('#txtEmailAdd').offset().top }, 'slow');
                    return false;
                }
                else if (document.getElementById("txtEmailAdd") != null && document.getElementById("txtEmailAdd").value != '' && !checkemail1(document.getElementById("txtEmailAdd").value)) {

                    jAlert("Please Enter valid Email.", "Required Information", "txtEmailAdd");
                    $('html, body').animate({ scrollTop: $('#txtEmailAdd').offset().top }, 'slow');
                    return false;
                }

            }
            else if (document.getElementById("txtZipcode") != null && document.getElementById("txtZipcode").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Zip Code.", "Required Information", "txtZipcode");
                $('html, body').animate({ scrollTop: $('#txtZipcode').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("txtPhone") != null && document.getElementById("txtPhone").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Phone.", "Required Information", "txtPhone");
                $('html, body').animate({ scrollTop: $('#txtPhone').offset().top }, 'slow');

                return false;
            }
            else if (!(document.getElementById('txtPhone').value).match('^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$')) {
                jAlert("Please enter valid Phone.", "Required Information", "txtPhone");
                $('html, body').animate({ scrollTop: $('#txtPhone').offset().top }, 'slow');

                return false;
            }
            else if (document.getElementById("txtEmailAdd") != null && document.getElementById("txtEmailAdd").value.replace(/^\s+|\s+$/g, "") == '') {

                jAlert("Please Enter Email.", "Required Information", "txtEmailAdd");
                $('html, body').animate({ scrollTop: $('#txtEmailAdd').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("txtEmailAdd") != null && document.getElementById("txtEmailAdd").value != '' && !checkemail1(document.getElementById("txtEmailAdd").value)) {

                jAlert("Please Enter valid Email.", "Required Information", "txtEmailAdd");
                $('html, body').animate({ scrollTop: $('#txtEmailAdd').offset().top }, 'slow');
                return false;
            }
            chkHeight();
            return true;
        }

        function OtherState() {

            if (document.getElementById("ddlState") != null && document.getElementById("ddlState").options[document.getElementById("ddlState").selectedIndex].value == '-11') {
                if (document.getElementById('Stateotherid')) { document.getElementById('Stateotherid').style.display = ''; }
            }
            else {
                if (document.getElementById('Stateotherid')) { document.getElementById('Stateotherid').style.display = 'none'; }
            }
        }
    </script>
    <style type="text/css">
        .required-red
        {
            color: red;
        }
    </style>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="3" style="font-size: 12px;">
            <tr style="background-color: #444; height: 25px;">
                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                    font-weight: bold;">
                    &nbsp;<%=AddressType %>
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="imgClosee" OnClientClick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose1').click();return false;"
                        ImageUrl="~/images/cancel.png" runat="server" ToolTip="Close"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" width="100%">
                    <span class="required-red">*</span>Required Field
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table border="0" style="padding-top: 5px" cellpadding="5" cellspacing="0">
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Order Email
                            </td>
                            <td>
                                :
                            </td>
                            <td >
                                <asp:TextBox ID="txtEmail" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                            <td colspan="3">
                                <asp:ImageButton ID="btnUpdateEmail" runat="server" AlternateText="Change E-mail"
                                    OnClientClick="return Emailvalidation();" OnClick="btnUpdateEmail_Click" />
                            </td>

                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>First Name
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFirstname" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Last Name
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtLastname" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">&nbsp;</span>Company
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCompany" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Address Line 1
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAddress1" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">&nbsp;</span>Address Line 2
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAddress2" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">&nbsp;</span>Apt/Suite #
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSuite" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>City
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCity" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Country
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlCountry" onchange="chkHeight();" runat="server" CssClass="order-list"
                                    Style="width: 180px; font-size: 12px;" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>State/Province
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlState" onchange="OtherState();" runat="server" CssClass="order-list"
                                    Style="width: 180px; font-size: 12px;">
                                </asp:DropDownList>
                                <br />
                                <div id="Stateotherid" style="display: none; margin-top: 5px;">
                                    <span class="required-red">*</span>Other :&nbsp;<asp:TextBox ID="txtOther" Style="font-size: 12px;"
                                        CssClass="order-textfield" Width="133px" runat="server"></asp:TextBox>
                                </div>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Zip Code
                            </td>
                            <td valign="top">
                                :
                            </td>
                            <td colspan="2" valign="top">
                                <asp:TextBox ID="txtZipcode" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>Phone
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPhone" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180" MaxLength="20"></asp:TextBox>
                            </td>
                            <td valign="top" style="font-size: 12px;">
                                <span class="required-red">*</span>E-Mail Address
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtEmailAdd" runat="server" Style="font-size: 12px;" CssClass="order-textfield"
                                    Width="180"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            
                        </tr>
                        <tr>
                            <td colspan="8">
                                &nbsp;
                            </td>
                        </tr>
                        <tr class="oddrow">
                            <td colspan="2">
                            </td>
                            <td colspan="6" align="center">
                                <asp:ImageButton ID="btnSave" OnClick="btnSave_Click" OnClientClick="return validationForm();"
                                    runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" />&nbsp;<asp:ImageButton ID="btnReset" runat="server" OnClientClick="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/reser-filter.gif"
                                        OnClick="btnReset_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 20%;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
