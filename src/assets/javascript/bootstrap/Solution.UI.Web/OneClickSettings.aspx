<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OneClickSettings.aspx.cs" Inherits="Solution.UI.Web.oneclicksettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" src="/js/EditAccountValidate.js" type="text/javascript"></script>
    <script language="javascript" src="/js/OneClickSettingValidation.js" type="text/javascript"></script>
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
                            title="BACK"><img title="BACK" id="imgback" runat="server" src="/images/back.png" /></a></span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center" class="table_none">
                    <tbody>
                        <tr>
                            <td>
                                <strong>Select Your 1-Click Default Billing Address and Shipping Address</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <span class="required-red">*</span>Required Fields
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td style="width: 18%">
                                                <span class="required-red">*</span>Billing Address :&nbsp;
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBillingAddress" runat="server" CssClass="select-box" Style="width: 460px;"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlBillingAddress_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>Shipping Address :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlShippingAddress" runat="server" CssClass="select-box" Style="width: 460px;"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlShippingAddress_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr id="pnlAddressDetails" runat="server">
                            <td>
                                <asp:Panel runat="server">
                                    <script type="text/javascript">
                                        window.onload = function () { enableTooltips('mainTable') };
                                    </script>
                                    <table id="mainTable" cellspacing="0" cellpadding="0" border="0" width="100%" class="table_none">
                                        <tr>
                                            <td colspan="6" style="background-color: #F1F1F1; color: #000000; font-weight: bold;">
                                                <span class="img-left">
                                                    <asp:Label ID="lblAddressTitle" runat="server"></asp:Label></span>
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
                                                <asp:TextBox ID="txtFirstname" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                                    Width="200px" TabIndex="1"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <span class="required-red">*</span>Country
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlcountry" runat="server" CssClass="select-box" Style="width: 212px;"
                                                    OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged" AutoPostBack="true"
                                                    TabIndex="8">
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
                                                <asp:TextBox ID="txtLastname" runat="server" CssClass="checkout-textfild" Width="200px"
                                                    TabIndex="2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span class="required-red">*</span>State/Province
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:DropDownList ID="ddlstate" runat="server" CssClass="select-box" Style="width: 212px;"
                                                    onchange="MakeOtherVisible();" TabIndex="9">
                                                </asp:DropDownList>
                                                <div id="DIVOther" style="display: none; padding-top: 5px">
                                                    <span class="required-red">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                                    <asp:TextBox ID="txtOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                        runat="server" CssClass="checkout-textfild" Width="78px"></asp:TextBox>
                                                    <asp:Label ID="lblBRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;&nbsp;</span>Company
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtCompany" MaxLength="100" runat="server" CssClass="checkout-text-add"
                                                    Width="200px" TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <span class="required-red">*</span>Zip Code
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="checkout-textfild" Width="200px"
                                                    TabIndex="10" MaxLength="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>Address Line 1
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtaddressLine1" MaxLength="500" runat="server" CssClass="checkout-text-add"
                                                    Width="200px" TabIndex="4"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <span class="required-red">*</span>Phone
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
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
                                            <td valign="top">
                                                <asp:TextBox ID="txtAddressLine2" MaxLength="500" runat="server" CssClass="checkout-text-add"
                                                    Width="200px" TabIndex="5"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Fax
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtFax" MaxLength="20" runat="server" CssClass="checkout-text-add"
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
                                            <td valign="top">
                                                <asp:TextBox ID="txtsuite" runat="server" MaxLength="100" CssClass="checkout-text"
                                                    TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td valign="middle">
                                                <span class="required-red">*</span>E-Mail Address
                                            </td>
                                            <td valign="middle" align="center">
                                                :
                                            </td>
                                            <td valign="middle">
                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="checkout-text-phone"
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
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="checkout-textfild" Width="200px"
                                                    TabIndex="7"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td valign="middle" align="left" colspan="4">
                                                <asp:ImageButton ID="btnAddressSubmit" runat="server" alt="SUBMIT" title="SUBMIT"
                                                    ImageUrl="~/images/submit.png" OnClientClick="return ValidatePage();"
                                                    OnClick="btnAddressSubmit_Click" TabIndex="14" />
                                                &nbsp;&nbsp;
                                                <asp:ImageButton ID="btnAddressCancel" runat="server" alt="CANCEL" title="CANCEL"
                                                    ImageUrl="~/images/cancel.png" OnClick="btnAddressCancel_Click" TabIndex="15" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td style="width: 18%">
                                                Payment Method :
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="select-box" Style="width: 212px;"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlCreditCard" runat="server">
                                                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table_none">
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="3" style="background-color: #F1F1F1; color: #000000; font-weight: bold;">
                                                                    <span class="img-left">Credit Card Details</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="3">
                                                                    <span class="required-red">*</span>Required Fields
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="25%">
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
                                                                    <%--      <img src="" alt="" style="position: absolute; display: none; border: 1px solid #E5E5E5;
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
                                                                    <asp:DropDownList ID="ddlmonth" runat="server" CssClass="select-box" Style="width: 130px;">
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
                                                                    <asp:DropDownList ID="ddlyear" runat="server" CssClass="select-box" Style="width: 130px;">
                                                                        <asp:ListItem Value="0">Year</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center" colspan="2">
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="middle" align="left">
                                                                    <asp:ImageButton ID="btnCardSubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                                                        OnClientClick="return ValidateCreditCard();" OnClick="btnCardSubmit_Click" />
                                                                    &nbsp;&nbsp;
                                                                    <asp:ImageButton ID="btnCardCancel" runat="server" alt="CANCEL" title="CANCEL" ImageUrl="/images/cancel.png"
                                                                        OnClick="btnCardCancel_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="3">
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td valign="middle" align="center">
                                                <div class="order-submit">
                                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                                        OnClick="btnsubmit_Click" OnClientClick="return ValidateOneClickPage();" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
