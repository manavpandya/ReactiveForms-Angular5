<%@ Page Title="" Language="C#" MasterPageFile="~/RMA/HPDAmazon/HPDAmazon.Master"
    AutoEventWireup="true" CodeBehind="ReturnItem.aspx.cs" Inherits="Solution.UI.Web.RMA.HPDAmazon.ReturnItem" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        function ValidatePage() {
            if ((document.getElementById('ContentPlaceHolder1_txtOrderNumber').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Order Number');
                document.getElementById('ContentPlaceHolder1_txtOrderNumber').focus();
                return false;
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtZipCode').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Zip Code');
                document.getElementById('ContentPlaceHolder1_txtZipCode').focus();
                return false;
            }

            else if ((document.getElementById('ContentPlaceHolder1_txtCodeshow').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Code Shown');
                document.getElementById('ContentPlaceHolder1_txtCodeshow').focus();
                return false;
            }
            return true;
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
    </script>
    <div class="breadcrumbs">
        <span>Return Order</span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>Return Order </span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td colspan="3" align="right">
                                            <span class="required-red">*</span> Required Fields
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="14%" align="left" valign="middle">
                                            <span class="required-red">*</span>Order Number
                                        </td>
                                        <td width="3%" align="left" valign="middle">
                                            :
                                        </td>
                                        <td width="75%" align="left" valign="middle">
                                            <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="checkout-textfild"></asp:TextBox>
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
                                            <asp:TextBox ID="txtZipCode" runat="server" Width="75px" CssClass="checkout-textfild"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="required-red">&nbsp;&nbsp;</span>Verification
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <img width="150px" height="40px" class="img-left" id="imgcapcha" alt="" src="/JpegImage.aspx?id=343343" />
                                            <input tabindex="33" type="button" value="" title="Reload" id="btnreload" style="background: url(/images/reload-icon.png) no-repeat transparent;
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
                                            <asp:TextBox ID="txtCodeshow" Width="75px" runat="server" CssClass="advance-quantity"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:ImageButton ID="btnsubmit" runat="server" alt="FINISH" title="FINISH" OnClientClick="return ValidatePage()"
                                                ImageUrl="/images/finish.png" OnClick="btnsubmit_Click" />
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
</asp:Content>
