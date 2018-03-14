<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AmazonContactus.aspx.cs" Inherits="Solution.UI.Web.AmazonContactus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            if (document.getElementById('name').value == "") {
                flag = false;
                name = 'Name';
                document.getElementById('name').focus();
            }
            else if ((document.getElementById('Email').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'E-Mail Address';
                document.getElementById('Email').focus();
            }
            else if ((document.getElementById('Email').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('Email').value)) {
                flag = false;
                name = 'valid E-Mail Address';
                document.getElementById('Email').focus();
            }
            else if ((document.getElementById('ContentPlaceHolder1_ddlcountry').options[document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex]).text == 'Select Country') {
                ValidationMsg = 'Please Select Country.';
                alert('Please select Country.');
                document.getElementById('ContentPlaceHolder1_ddlcountry').focus();
                return false;
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtzipcode').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'Zip Code';
                document.getElementById('ContentPlaceHolder1_txtzipcode').focus();
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtzipcode').value).replace(/^\s*\s*$/g, '') != '' && (document.getElementById('ContentPlaceHolder1_ddlcountry').options[document.getElementById('ContentPlaceHolder1_ddlcountry').selectedIndex]).text == 'United States' && (document.getElementById('ContentPlaceHolder1_txtzipcode').value.length != 5 || isNaN(document.getElementById('ContentPlaceHolder1_txtzipcode').value))) {
                flag = false;
                alert('Zip Code must be 5 digit long and Numeric.');
                document.getElementById('ContentPlaceHolder1_txtzipcode').focus();
                return false;
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtinformation').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'Specific Question/Comment';
                document.getElementById('ContentPlaceHolder1_txtinformation').focus();
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtShowcode').value).replace(/^\s*\s*$/g, '') == '') {
                flag = false;
                name = 'Code Shown';
                document.getElementById('ContentPlaceHolder1_txtShowcode').focus();
            }



            else {
                flag = true;
            }

            if (flag == false) {
                alert('Please enter ' + name + '.');
            }
            return flag;
        }


    </script>
    <asp:Panel ID="pnlcontactus" runat="server" DefaultButton="btnsubmit">
        <div class="content-main">
            <div class="static-title">
                <span>Contact Us </span>
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
                                <td width="30%">
                                    <span class="required-red">*</span>Name
                                </td>
                                <td width="3%">
                                    :
                                </td>
                                <td width="67%">
                                    <input type="text" id="name" name="name" class="contact-fild" onkeypress="return isNumberKey(event)" /><%--
                                    <asp:TextBox ID="txtname" onkeypress="return isNumberKey(event)" runat="server" CssClass="contact-fild"></asp:TextBox>--%>
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
                                    <input type="text" id="Email" name="Email" class="contact-fild" value="" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <span class="required-red">&nbsp;</span>Address
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                     <input type="text" id="Address" name="Address" TextMode="MultiLine" class="contact-fild"  Columns="45" Rows="5" value="" />
                                     
                                </td>
                            </tr>
                            <tr>
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
                            <tr>
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
                            <tr>
                                <td>
                                    <span style="height: 30px;"><span class="required-red">&nbsp;</span>State/Province
                                    </span>
                                </td>
                                <td>
                                    :
                                </td>
                                <td style="padding-left: 0px">
                                    <table>
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
                            <tr>
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
                                <td valign="top" align="left">
                                    <span class="required-red">*</span>Please enter specific<br />
                                    &nbsp;&nbsp;&nbsp;information that may help<br />
                                    &nbsp;&nbsp;&nbsp;us to serve you better<br />
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtinformation" TextMode="MultiLine" runat="server" CssClass="contact-textaria"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
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
                            <tr>
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
                                        <%--OnClick="btnsubmit_Click"--%>
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
    <%--<fieldset>
          <legend>Contact Us</legend>
          <div>
            <label for="Name">Name:</label>
            <input type="text" name="Name" value="" />
          </div>
          <div>
            <label for="Email">Email:</label>
            <input type="text" name="Email" value="" />
          </div>
          <div>
            <label for="City">City:</label>
            <input type="text" name="City" value="" />
          </div>
          <div>
            <label for="Address">Address:</label>
            <input type="text" name="Address" value="" />
          </div>
          <div>
            <label for="Country">Country:</label>
            <input type="text" name="Country" value="" />
          </div>
          <div>
            <label for="STORENAME">STORENAME:</label>
            <input type="hidden" name="STORENAME" value="" />
          </div>
          
          <div>
            <label>&nbsp;</label>
            <input type="submit" value="Submit" class="submit" />
          </div>
        </fieldset>--%>
</asp:Content>
