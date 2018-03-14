<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreditCardDetail.aspx.cs" Inherits="Solution.UI.Web.CreditCardDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/js/CreditCardDetailValidation.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtCardNumber').bind('copy paste', function (e) {
                e.preventDefault();
            });
        });

    </script>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> > <span>
                <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <table style="width: 100%;">
                <tr>
                    <td style="float: left; width: 94%;">
                        <span>
                            <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
                        </span>
                    </td>
                    <td style="float: right; width: 5%;">
                        <span><a href="MyAccount.aspx" style="color: #B92127; text-decoration: underline;"
                           title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a> </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div class="static-main-box" style="min-height: 200px;">
                <asp:Panel runat="server" ID="pnlViewDetails" DefaultButton="btnsubmit">
                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table_none">
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lnkbtnAddNew" Style="text-decoration: underline;" runat="server"
                                    OnClick="lnkbtnAddNew_Click" Text="Add New"></asp:LinkButton>
                            </td>
                        </tr>
                        <asp:Literal ID="ltrViewDetails" runat="server"></asp:Literal>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlAddEdit" DefaultButton="btnsubmit">
                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table_none">
                        <tbody>
                            <tr>
                                <td align="right" colspan="3">
                                    <span class="required-red">*</span>Required Fields
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                    <span class="required-red">*</span>Name on Card
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="77%">
                                    <asp:TextBox ID="txtcardName" MaxLength="100" runat="server" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Card Type
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="select-box" Style="width: 262px;">
                                        <asp:ListItem Text="Select Card Type" Value="Select Card Type">Select Card Type</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Card Number
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardNumber" onkeypress="return isNumberKeyCard(event)" MaxLength="16"
                                        runat="server" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <span class="required-red">*</span>Card Varification Code
                                    <%--  <img src="" alt="" style="position: absolute; display: none; border: 1px solid #E5E5E5;
                                            left: 475px; z-index: 100; background-color: #fff; border: solid 1px #e0dfdf;
                                            padding: 5px 20px 20px 20px;" id="CVCImage" />--%>
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
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardVarificationCode" onkeypress="return isNumberKeyCard(event)"
                                        MaxLength="4" runat="server" CssClass="contact-fild" Style="width: 150px;"></asp:TextBox>
                                    &nbsp;&nbsp; &nbsp;&nbsp; (<a href="javascript:void(0)" class="text_link" onmouseout="javascript:document.getElementById('CVCImage').style.display='none';"
                                        onmouseover="javascript:document.getElementById('CVCImage').style.display=''; ">What's
                                        This</a>)
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <span class="required-red">*</span>Expiration Date
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlmonth" runat="server" CssClass="select-box" Style="width: 85px;">
                                        <asp:ListItem Value="0">Month</asp:ListItem>
                                        <asp:ListItem Value="01">Jan-01</asp:ListItem>
                                        <asp:ListItem Value="02">Feb-02</asp:ListItem>
                                        <asp:ListItem Value="03">Mar-03</asp:ListItem>
                                        <asp:ListItem Value="04">Apr-04</asp:ListItem>
                                        <asp:ListItem Value="05">May-05</asp:ListItem>
                                        <asp:ListItem Value="06">June-06</asp:ListItem>
                                        <asp:ListItem Value="07">July-07</asp:ListItem>
                                        <asp:ListItem Value="08">Aug-08</asp:ListItem>
                                        <asp:ListItem Value="09">Sept-09</asp:ListItem>
                                        <asp:ListItem Value="10">Oct-10</asp:ListItem>
                                        <asp:ListItem Value="11">Nov-11</asp:ListItem>
                                        <asp:ListItem Value="12">Dec-12</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlyear" runat="server" CssClass="select-box" Style="width:80px;">
                                        <asp:ListItem Value="0">Year</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    &nbsp;&nbsp;<asp:CheckBox ID="chkDefaultCreditCardID" runat="server" />
                                    Make this my default 1-Click credit card
                                </td>
                            </tr>
                            <tr id="trmsgdefault" visible="false" runat="server">
                                <td valign="top" align="left" colspan="3">
                                    <asp:Label ID="lblmsgdefault" Font-Bold="true" runat="server">
                                                               &nbsp;&nbsp;If You do not want to set this address as default Address ,Please select another address as<br />&nbsp; Default Address from the Address Book</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    <span class="required-red">*</span><strong>Please select your billing address from address
                                        book</strong>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    &nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlBillingAddress" runat="server" CssClass="select-box" Style="width: 460px;"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlBillingAddress_SelectedIndexChanged">
                                        <asp:ListItem Text="Select Billing Address" Value="Select Billing Address">Select Billing Address</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    &nbsp;
                                </td>
                                <td valign="middle" align="left">
                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                        OnClientClick="return ValidatePage();" OnClick="btnsubmit_Click" />
                                    &nbsp;&nbsp; <a href="/creditcarddetail.aspx?Type=1" title="CANCEL">
                                        <img src="/images/cancel.png" alt="" /></a>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="3">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlBillingDetails" DefaultButton="btnsubmit">
                    <script type="text/javascript">
                        window.onload = function () { enableTooltips('mainTable') };
                    </script>
                    <table id="mainTable" cellspacing="0" cellpadding="0" border="0" width="100%" class="table_none">
                        <tr>
                            <td colspan="6" style="background-color: #F1F1F1; color: #000000; font-weight: bold;">
                                <span class="img-left">Billing Address</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="right">
                                <span class="required-red">*</span> Required Fields
                            </td>
                        </tr>
                        <tr>
                            <td height="35" valign="middle">
                                <span class="required-red">*</span>First Name
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillFirstname" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                    Width="200px" TabIndex="1"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <span class="required-red">*</span>Country
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="select-box" Style="width: 212px;"
                                    OnSelectedIndexChanged="ddlBillcountry_SelectedIndexChanged" AutoPostBack="true"
                                    TabIndex="8">
                                    <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>Last Name
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillLastname" runat="server" CssClass="checkout-textfild" Width="200px"
                                    TabIndex="2"></asp:TextBox>
                            </td>
                            <td>
                                <span class="required-red">*</span>State/Province
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="select-box" Style="width: 212px;"
                                    onchange="MakeBillingOtherVisible();" TabIndex="9">
                                </asp:DropDownList>
                                <div id="DIVBillingOther" style="display: none; padding-top: 5px">
                                    <span class="required-red">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                    <asp:TextBox ID="txtBillingOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                        runat="server" CssClass="checkout-textfild" Width="78px"></asp:TextBox>
                                    <asp:Label ID="lblBRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">&nbsp;</span>Company
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtBillingCompany" MaxLength="100" runat="server" CssClass="checkout-text-add"
                                    Width="200px" TabIndex="3"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <span class="required-red">*</span>Zip Code
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillZipCode" runat="server" CssClass="checkout-textfild" Width="200px"
                                    TabIndex="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>Address Line 1
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBilladdressLine1" MaxLength="500" runat="server" CssClass="checkout-text-add"
                                    Width="200px" TabIndex="4"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <span class="required-red">*</span>Phone
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                    runat="server" CssClass="checkout-text-phone" Width="200px" TabIndex="11"></asp:TextBox>
                                <img src="/images/help.jpg" alt="help" title="Your phone number is needed in case we need to contact
                                                    you about your order like (123-456-7890)." class="img-left" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">&nbsp;</span>Address Line 2
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillAddressLine2" MaxLength="500" runat="server" CssClass="checkout-text-add"
                                    Width="200px" TabIndex="5"></asp:TextBox>
                            </td>
                            <td>
                                <span class="required-red">&nbsp;</span>Fax
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillingFax" MaxLength="20" runat="server" CssClass="checkout-text-add"
                                    Width="200px" TabIndex="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <span class="required-red">&nbsp;&nbsp;</span>Apt/Suite #
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillsuite" runat="server" MaxLength="100" CssClass="checkout-text"
                                    TabIndex="6"></asp:TextBox>
                            </td>
                            <td valign="middle">
                                <span class="required-red">*</span>E-Mail Address
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtBillEmail" runat="server" MaxLength="100" CssClass="checkout-text-phone"
                                    Width="200px" TabIndex="13"></asp:TextBox>
                                <img src="/images/help.jpg" alt="Your E-Mail address will never be sold or given to other companies."
                                    title="Your E-Mail address will never be sold or given to other companies." class="img-left" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>City
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top" colspan="4">
                                <asp:TextBox ID="txtBillCity" runat="server" CssClass="checkout-textfild" Width="200px"
                                    TabIndex="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td valign="middle" align="left" colspan="4">
                                <asp:ImageButton ID="btnFinish" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="~/images/submit.png"
                                    OnClientClick="return ValidateBillingFields();" OnClick="btnFinish_Click" TabIndex="14" />
                                &nbsp;&nbsp;
                                <asp:ImageButton ID="btnBillingCancel" runat="server" alt="CANCEL" title="CANCEL"
                                    ImageUrl="~/images/cancel.png" OnClick="btnBillingCancel_Click" TabIndex="15" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
