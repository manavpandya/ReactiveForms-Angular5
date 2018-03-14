<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OnlineApplicationForm.aspx.cs" Inherits="Solution.UI.Web.OnlineApplicationForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .membership-box
        {
            border: 1px solid #E6E6E6;
            float: left;
            margin: 5px 0;
            padding: 0 5px;
            width: 728px;
        }
        .membership-box-bt1
        {
            border-right: 1px solid #E6E6E6;
            float: left;
            margin: 0 9px 0 0;
            padding: 5px 0;
            width: 236px;
        }
        .membership-box-bt1-row
        {
            float: left;
            padding: 3px 0;
        }
        .membership-box-bt1-row span
        {
            color: #6C6D71;
            float: left;
            font-size: 12px;
            line-height: 20px;
            width: 86px;
        }
        .membership-box-textbox
        {
            border: 1px solid #CDC8C4;
            float: left;
            height: 18px;
            width: 140px;
            color: #6C6D71;
        }
        .membership-box-bt2
        {
            float: left;
            margin: 0;
            padding: 5px 0;
            width: 236px;
        }
        .static_content p
        {
            color: #3B3B3B;
            float: left;
            font-size: 12px;
            line-height: 18px;
            padding: 5px 0;
            width: 940px;
        }
        .static_content ul
        {
            float: left;
            list-style: none outside none;
            margin: 0;
            padding: 0 10px;
            width: 920px;
        }
        .static_content ul li
        {
            background: url("../images/bullet-features.gif") no-repeat scroll left top transparent;
            float: left;
            font-size: 12px;
            list-style: none outside none;
            margin: 0;
            padding: 2px 15px;
            width: 890px;
        }
    </style>
    <script type="text/javascript">
        function checkfileupload() {
            if (document.getElementById('ContentPlaceHolder1_fuUplodDoc')) {
                var fup = document.getElementById('ContentPlaceHolder1_fuUplodDoc');
                var fileName = fup.value;
                if (fileName == '') {
                    alert('Please Select File.', 'Message');
                    return false;
                }

            }
            return true;
        }
    </script>
    <script language="javascript" src="js/ContactUSValidate.js" type="text/javascript"></script>
    <asp:Panel ID="pnlcontactus" runat="server" DefaultButton="btnsubmit">
        <div class="breadcrumbs">
            <a href="/index.aspx" title="Home">Home </a>> <span>
                <asp:Literal ID="ltbrTitle" runat="server"></asp:Literal></span>
        </div>
        <div class="content-main">
            <div class="static-title">
                <span>
                    <asp:Literal ID="ltTitle" runat="server"></asp:Literal></span>
            </div>
            <div class="static-main">
                <div class="static-main-box">
                    <div style="padding-bottom: 5px; text-align: center;">
                        <center>
                            <asp:Label ID="lblMsg" runat="server" Style="color: Red;" CssClass="checkout-red"></asp:Label></center>
                    </div>
                    <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table-none">
                        <tbody>
                            <tr>
                                <td align="right" colspan="3">
                                    <span class="required-red">*</span>Required Fields
                                </td>
                            </tr>
                            <tr>
                                <td width="15%">
                                    <span class="required-red">*</span>Company Name
                                </td>
                                <td width="2%">
                                    :
                                </td>
                                <td width="67%">
                                    <asp:TextBox ID="txtCompany" onkeypress="return isNumberKey(event)" runat="server"
                                        CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <b>Business Address</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Street
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStreet" runat="server" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>City
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCity" runat="server" onkeypress="return isNumberKey(event)" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">&nbsp;</span>State/Province
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtState" runat="server" onkeypress="return isNumberKey(event)"
                                        CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Zip/Postal Code
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtzipcode" runat="server" CssClass="zipcode-fild" MaxLength="15"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Website
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtWebSite" runat="server" CssClass="contact-fild" MaxLength="45"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <b>Contact Name/s</b> (please list each person that will use this account)
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="membership-box">
                                        <div class="membership-box-bt1">
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red">*</font>First Name:</span>
                                                <asp:TextBox ID="txtM1FirstName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red">*</font>Last Name:</span>
                                                <asp:TextBox ID="txtM1LastName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red">*</font>Title:</span>
                                                <asp:TextBox ID="txtM1Title" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red">*</font>Email:</span>
                                                <asp:TextBox ID="txtM1Email" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red">*</font>Phone:</span>
                                                <asp:TextBox ID="txtM1Phone" runat="server" onkeypress="return onKeyPressPhone(event);"
                                                    MaxLength="20" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span><font class="required-red" style="padding-right: 6px;"></font>Fax:</span>
                                                <asp:TextBox ID="txtM1Fax" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="membership-box-bt1">
                                            <div class="membership-box-bt1-row">
                                                <span>First Name:</span>
                                                <asp:TextBox ID="txtM2FirstName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Last Name:</span>
                                                <asp:TextBox ID="txtM2LastName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Title:</span>
                                                <asp:TextBox ID="txtM2Title" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Email:</span>
                                                <asp:TextBox ID="TextBox4" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Phone:</span>
                                                <asp:TextBox ID="txtM2Email" runat="server" onkeypress="return onKeyPressPhone(event);"
                                                    MaxLength="20" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Fax:</span>
                                                <asp:TextBox ID="txtM2Fax" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="membership-box-bt2">
                                            <div class="membership-box-bt1-row">
                                                <span>First Name:</span>
                                                <asp:TextBox ID="txtM3FirstName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Last Name:</span>
                                                <asp:TextBox ID="txtM3LastName" onkeypress="return isNumberKey(event)" runat="server"
                                                    CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Title:</span>
                                                <asp:TextBox ID="txtM3Title" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Email:</span>
                                                <asp:TextBox ID="txtM3Email" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Phone:</span>
                                                <asp:TextBox ID="txtM3Phone" runat="server" onkeypress="return onKeyPressPhone(event);"
                                                    MaxLength="20" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                            <div class="membership-box-bt1-row">
                                                <span>Fax:</span>
                                                <asp:TextBox ID="txtM3Fax" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td width="30%">
                                    <span class="required-red">*</span>Name
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="67%">
                                    <asp:TextBox ID="txtname" onkeypress="return isNumberKey(event)" runat="server" CssClass="contact-fild"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <span class="required-red">*</span>Number of Employees in your company
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoOfEmployee" onkeypress="return onKeyPressPhone(event);" runat="server" CssClass="membership-box-textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <span class="required-red">*</span>Primary Project Type:
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="select-box">
                                        <asp:ListItem Value="0" Text="Project Type">Project Type</asp:ListItem>
                                        <asp:ListItem Value="1" Text="Residential">Residential</asp:ListItem>
                                        <asp:ListItem Value="2" Text="Commercial">Commercial</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   Choose Document&nbsp;:&nbsp;<asp:FileUpload ID="fuUplodDoc"  runat="server" /></td>
                                <td>
                                <asp:ImageButton Style="vertical-align: middle" ID="btnUpload" runat="server" AlternateText="Upload"
                              OnClientClick="return checkfileupload();" OnClick="btnUpload_Click" ImageUrl="~/App_Themes/Gray/images/upload.gif" />
                                </td>
                                <td>
                                    <asp:Label ID ="lblFileName" runat="server"></asp:Label>&nbsp;<asp:ImageButton Style="vertical-align: middle" ID="imgdeletebtn" runat="server" Visible="false" AlternateText="Delete"
                                OnClick="imgdeletebtn_Click" ImageUrl="~/App_Themes/Gray/images/delet.gif" />                          </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span class="required-red">&nbsp;</span><b>Business Documents</b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="static_content">
                                        <p>
                                            Please attach at least one of the following</p>
                                        <ul>
                                            <li>Current Business or State Profession License, in a Residential or Commercial Design-based
                                                business, or the Hospitality industry</li>
                                            <li>Proof of current AI or IDI provincial registration</li>
                                            <li>Business ID number</li>
                                            <li>Proof of current ASID membership</li>
                                            <li>Interior design certification (e.g. NCIDQ, CCIDC)</li>
                                            <li>W9, Federal ID form, or EIN number</li>
                                            <li>Resale or Sales Tax Certificate</li>
                                        </ul>
                                       <%-- <p>
                                            If you intend to purchase merchandise for resale, you will be required to supply
                                            a Resale or Sales Tax Certificate. Without this documentation, sales tax will be
                                            applied to all orders.</p>--%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    &nbsp;
                                </td>
                                <td valign="middle" align="left">
                                    <a href="#" title="submit">
                                        <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                             OnClick="btnsubmit_Click" OnClientClick="return ValidateOnlineForm();"/>
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="3">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
