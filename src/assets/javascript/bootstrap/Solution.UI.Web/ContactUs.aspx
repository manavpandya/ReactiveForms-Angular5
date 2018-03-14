﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ContactUs.aspx.cs" Inherits="Solution.UI.Web.ContactUs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<script language="javascript" src="js/ContactUSValidate.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function ReloadCapthca() {
            document.getElementById('ContentPlaceHolder1_txtShowcode').value = '';

            var chars = "0123456789";
            var string_length = 8;
            var randomstring = '';
            for (var i = 0; i < string_length; i++) {
                var rnum = Math.floor(Math.random() * chars.length);
                randomstring += chars.substring(rnum, rnum + 1);
            }
            document.getElementById('imgcapcha').src = '/JpegImage.aspx?id=' + randomstring;
            document.getElementById('ContentPlaceHolder1_txtShowcode').focus();
        }

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
        function ValidatePage() {
            var flag = false;
            var name;
            if ((document.getElementById('ContentPlaceHolder1_txtname').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'Name';
                document.getElementById('ContentPlaceHolder1_txtname').focus();
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'E-Mail Address';
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtEmail').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value)) {
                flag = false;
                name = 'valid E-Mail Address';
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
            }
               
            else if ((document.getElementById('ContentPlaceHolder1_txtinformation').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'Message';
                document.getElementById('ContentPlaceHolder1_txtinformation').focus();
            }
               
            else {
                var windowHeight = 0;
                windowHeight = $(document).height(); //window.innerHeight;
                document.getElementById('prepage').style.height = windowHeight + 'px';
                document.getElementById('prepage').style.display = '';
                flag = true;
            }

            if (flag == false) {
                alert('Please enter ' + name + '.');
            }

            return flag;
        }
        
    </script>
    <asp:Panel ID="pnlcontactus" runat="server" DefaultButton="btnsubmit">
        <div class="breadcrumbs">
            <a href="/index.aspx" title="Home">Home </a> > <span>
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
                    <table width="100%" cellspacing="0" cellpadding="3" border="0">
                        <tr>
                            <td style="width: 50%;" valign="top">
                                <asp:Literal ID="ltContactusContent" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellspacing="0" cellpadding="3" border="0" class="table-none">
                                    <tbody>
                                        <tr>
                                            <td align="right" colspan="3">
                                                <span class="required-red">*</span>Required Fields
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                <span class="required-red">*</span>FirstName
                                            </td>
                                            <td width="3%">
                                                :
                                            </td>
                                            <td width="67%">
                                                <asp:TextBox ID="txtname" onkeypress="return isNumberKey(event)" runat="server" CssClass="contact-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td width="30%">
                                                <span class="required-red">&nbsp;</span>LastName
                                            </td>
                                            <td width="3%">
                                                :
                                            </td>
                                            <td width="67%">
                                                <asp:TextBox ID="txtlastname" onkeypress="return isNumberKey(event)" runat="server" CssClass="contact-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>Email
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="contact-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Phone
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhone" runat="server" onkeypress="return onKeyPressPhone(event);"
                                                    MaxLength="20" CssClass="contact-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Subject
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpSubject"  Style="width: 212px; height: 21px;" runat="server" CssClass="select-box"></asp:DropDownList>
                                            </td>
                                        </tr>
                                         

                                        <tr style="display:none;">

                                            <td valign="top" align="left">
                                                <span class="required-red">&nbsp;</span>Address
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="contact-textaria"
                                                    Columns="45" Rows="5"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td>
                                                <span class="required-red">&nbsp;</span>City
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCity" runat="server" onkeypress="return isNumberKey(event)" CssClass="contact-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td style="">
                                                <span class="required-red">*</span>Country
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcountry" runat="server" AutoPostBack="true" CssClass="select-box"
                                                    Style="width: 212px; height: 21px;" OnSelectedIndexChanged="ddlcountry_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td>
                                                <span style="height: 30px;"><span class="required-red">&nbsp;</span>State/Province
                                                </span>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td style="padding-left: 0px">
                                                <table style="display:none;">
                                                    <tr>
                                                        <td style="padding: 0 0 0 7px;">
                                                            <asp:DropDownList ID="ddlstate" onchange="vitxt();" runat="server" CssClass="select-box"
                                                                Style="width: 212px;">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 0 0 0 7px;">
                                                            <div id="divother" runat="Server" style="display: none; float: left;">
                                                                <asp:Label ID="lt_other" runat="server" Text="If Others, Specify  "></asp:Label>
                                                                <asp:TextBox ID="txtother" onkeypress="return isNumberKey(event)" runat="server"
                                                                    CssClass="specify-fild" Width="100px" MaxLength="70" Style="margin-top: 5px;
                                                                    margin-left: 5px;"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td>
                                                <span class="required-red">*</span>Zip Code
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                             <asp:TextBox ID="txtzipcode" runat="server" CssClass="zipcode-fild" MaxLength="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td valign="top" align="left">
                                                <span class="required-red">*</span>Message
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtinformation" TextMode="MultiLine" runat="server" CssClass="contact-textaria"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td valign="middle">
                                                <span class="required-red">&nbsp;</span>Verification
                                            </td>
                                            <td valign="middle">
                                                :
                                            </td>
                                            <td>
                                                <img width="150px" height="40px" class="img-left" id="imgcapcha" alt="" src="/JpegImage.aspx?id=343343" />
                                                <input type="button" value="" title="Reload" id="btnreload" style="background: url(/images/reload-icon.png) no-repeat transparent;
                                                    width: 31px; height: 29px; border: none; cursor: pointer; margin: 8px 0 0 5px;"
                                                    onclick="ReloadCapthca();" />
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td>
                                                <span class="required-red">*</span>Enter the code shown
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtShowcode" runat="server" CssClass="zipcode-fild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center" colspan="2">
                                                &nbsp;
                                            </td>
                                            <td valign="middle" align="left">
                                                <a href="#" title="submit">
                                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" ImageUrl="/images/submit.png"
                                                        OnClientClick="return ValidatePage();" OnClick="btnsubmit_Click" />
                                                </a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="3">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 100px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
