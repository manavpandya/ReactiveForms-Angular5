<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditAddress.aspx.cs" Inherits="Solution.UI.Web.EditAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="js/EditAccountValidate.js"></script>
    <script type="text/javascript">
        window.onload = function () { enableTooltips('mainTable') };
    </script>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <a href="/myaccount.aspx" title="My Account">
            My Account</a> > <a href="/Addressbook.aspx" title="Address Book">Address Book</a>
        > <span>
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
            <div class="static-main-box">
                <asp:Panel ID="pnlEditAddr" runat="server" DefaultButton="btnFinish">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none"
                        id="mainTable">
                        <tr>
                            <td colspan="3" style="text-align: right; float: right;" valign="middle">
                                <span class="required-red">* </span>Required Fields
                            </td>
                        </tr>
                        <tr>
                            <td height="35" valign="middle" width="20%">
                                <span class="required-red">*</span>First Name
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtFirstname" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                    Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>Last Name
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtLastname" runat="server" CssClass="checkout-textfild" Width="200px"></asp:TextBox>
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
                                <asp:TextBox ID="txtCompany" MaxLength="100" runat="server" CssClass="checkout-textfild"
                                    Width="200px"></asp:TextBox>
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
                                <asp:TextBox ID="txtaddressLine1" MaxLength="500" runat="server" CssClass="checkout-textfild"
                                    Width="200px"></asp:TextBox>
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
                                <asp:TextBox ID="txtAddressLine2" MaxLength="500" runat="server" CssClass="checkout-textfild"
                                    Width="200px"></asp:TextBox>
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
                                <asp:TextBox ID="txtsuite" runat="server" MaxLength="100" CssClass="checkout-text"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>City
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtCity" runat="server" CssClass="checkout-textfild" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <span class="required-red">*</span>Country
                            </td>
                            <td valign="top" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:DropDownList ID="ddlcountry" runat="server" CssClass="select-box" Style="width: 212px;"
                                    OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Select Country" Value="Select Country">Select Country</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">*</span>State/Province
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlstate" runat="server" CssClass="select-box" Style="width: 212px;"
                                    onchange="MakeOtherVisible();">
                                </asp:DropDownList>
                                <div id="DIVOther" style="display: none; padding-top: 5px;">
                                    <span class="required-red">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                    <asp:TextBox ID="txtOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                        runat="server" CssClass="checkout-textfild" Width="78px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <span class="required-red">*</span>Zip Code
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="checkout-textfild" Width="200px"
                                    MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <span class="required-red">*</span>Phone
                            </td>
                            <td align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                    runat="server" CssClass="checkout-text-phone" Width="200px"></asp:TextBox>
                                <img src="/images/help.jpg" alt="help" title="Your phone number is needed in case we need to contact
                                                    you about your order like (123-456-7890)." class="img-left" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="required-red">&nbsp;</span>Fax
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtFax" MaxLength="20" runat="server" CssClass="checkout-textfild"
                                    Width="200px"></asp:TextBox>
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
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" CssClass="checkout-text-phone"
                                    Width="200px"></asp:TextBox>
                                <img src="/images/help.jpg" alt="help" title="Your E-Mail address will never be sold or given to other
                                                    companies." class="img-left" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                <span class="required-red">&nbsp;</span>Is Default Address
                            </td>
                            <td valign="middle" align="center">
                                :
                            </td>
                            <td valign="middle">
                                <asp:CheckBox ID="chkDefaultAddress" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td valign="middle" align="left">
                                <asp:Label ID="lblmsgdefault" Visible="false" runat="server">
                                                              If You do not want to set this address as default Address ,Please select another address as Default Address from the Address Book</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                            <td valign="middle" align="left">
                                <asp:ImageButton ID="btnFinish" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                    OnClientClick="return ValidatePage();" OnClick="btnFinish_Click" />
                                &nbsp;&nbsp; <a href="/Addressbook.aspx" title="CANCEL">
                                    <img src="/images/cancel.png" alt="" /></a>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
