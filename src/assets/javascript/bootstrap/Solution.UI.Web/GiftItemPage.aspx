<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GiftItemPage.aspx.cs" Inherits="Solution.UI.Web.GiftItemPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function onKeyPressBlockNumbers1(e) {
            var key = window.event ? window.event.keyCode : e.which;
            if (key != 46) {
                if (key == 32 || key == 39 || key == 37 || key == 46 || key == 8 || key == 9 || key == 189 || key == 0) {
                    return key;
                }
            }
            if (key == 13) {
                if (document.getElementById('ContentPlaceHolder1_btnAddToCart') != null) {
                    document.getElementById('ContentPlaceHolder1_btnAddToCart').click();
                }
            }
            var keychar = String.fromCharCode(key);
            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }

        function checkValidation() {

            if ((document.getElementById("ContentPlaceHolder1_txtQty").value == '') || (document.getElementById("ContentPlaceHolder1_txtQty").value <= 0) || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value)) {

                //jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty");
                alert("Please enter valid digits only !!!");
                document.getElementById("ContentPlaceHolder1_txtQty").value = 1;
                document.getElementById("ContentPlaceHolder1_txtQty").focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtRecipientName') != null && document.getElementById("ContentPlaceHolder1_txtRecipientName").value.replace(/^\s+|\s+$/g, "") == '') {
                //jAlert("Please Enter Recipient's Name", "Message", "ContentPlaceHolder1_txtRecipientName");
                alert("Please Enter Recipient's Name");
                document.getElementById('ContentPlaceHolder1_txtRecipientName').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtRecipientEmail') != null && document.getElementById("ContentPlaceHolder1_txtRecipientEmail").value.replace(/^\s+|\s+$/g, "") == '') {
                //jAlert("Please Enter Recipient's Email", "Message", "ContentPlaceHolder1_txtRecipientEmail");
                alert("Please Enter Recipient's Email");
                document.getElementById('ContentPlaceHolder1_txtRecipientEmail').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtRecipientEmail') != null && document.getElementById("ContentPlaceHolder1_txtRecipientEmail").value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtRecipientEmail").value)) {
                //jAlert("Enter Valid Recipient's Email", "Message", "ContentPlaceHolder1_txtRecipientEmail");
                alert("Please Enter Recipient's Email");
                document.getElementById('ContentPlaceHolder1_txtRecipientEmail').focus();
                return false;
            }

            else if (document.getElementById('ContentPlaceHolder1_txtMessage') != null && document.getElementById("ContentPlaceHolder1_txtMessage").value.replace(/^\s+|\s+$/g, "") == '') {
                //jAlert("Please Enter Message for Recipient", "Message", "ContentPlaceHolder1_txtMessage");
                alert("Please Enter Message for Recipient");
                document.getElementById('ContentPlaceHolder1_txtMessage').focus();
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


        function checkquantity() {
            if ((document.getElementById("ContentPlaceHolder1_txtQty").value == '') || (document.getElementById("ContentPlaceHolder1_txtQty").value <= 0) || isNaN(document.getElementById('ContentPlaceHolder1_txtQty').value)) {
                //jAlert("Please enter valid digits only !!!", "Message", "ContentPlaceHolder1_txtQty");
                alert("Please enter valid digits only !!!");
                document.getElementById('ContentPlaceHolder1_txtQty').focus();
                document.getElementById("ContentPlaceHolder1_txtQty").value = 1; return false;
            }
            var TotQty = 0;
            var check = false;

            if (check == false) {
                document.getElementById("ContentPlaceHolder1_hdnQuantity").value = TotQty;
                return true;
            }
            else
            { return true; }
        }

    </script>
    <div class="right-sidebar">
        <div class="breadcrumb">
            <a title='Home' href='/index.aspx'>Home </a>> <a title='Gift Certificate' href='/GiftCertificate.aspx'>
                Gift Certificate </a>> <span>
                    <asp:Literal ID="ltbreadcrmbs" runat="server"></asp:Literal></span>
        </div>
        <div class="title-box1">
            <h1>
                <asp:Literal ID="ltTitle" runat="server" Text="Gift Certificate"></asp:Literal></h1>
        </div>
        <div class="cat-row1" id="topMiddle" runat="server">
            <center>
                <span style="color: #7D888F; text-align: center;">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label></span></center>
            <input type="hidden" id="hdnprice" runat="server" value="0" />
            <table cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td align="right" valign="top" colspan="3">
                        <span class="required-red">*</span>Required Fields
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <div style="padding: 5px;">
                            <div>
                                <asp:Literal ID="litProductMainImage" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </td>
                    <td colspan="2" valign="top">
                        <table cellpadding="4" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 120px;">
                                    <span class="required-red">&nbsp;&nbsp;</span> Price :
                                </td>
                                <td>
                                    <span style="color: #B01230;">$<asp:Literal ID="litSalePrice" runat="server"></asp:Literal></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">&nbsp;&nbsp;</span> Quantity :
                                </td>
                                <td>
                                    <input type="hidden" id="hdnQuantity" runat="server" value="0" />
                                    <asp:TextBox ID="txtQty" class="contact-fild" Width="50px" Style="text-align: center;
                                        text-indent: 2px;" onkeypress="return onKeyPressBlockNumbers1(event);" MaxLength="3"
                                        runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trRecipientName" runat="server">
                                <td>
                                    <span class="required-red">*</span> Recipient's Name :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRecipientName" class="contact-fild" Style="text-indent: 2px;"
                                        Width="200px" MaxLength="50" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trRecipientEmail" runat="server">
                                <td>
                                    <span class="required-red">*</span> Recipient's Email :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRecipientEmail" class="contact-fild" Style="text-indent: 2px;"
                                        Width="200px" MaxLength="50" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trRecipientMessage" runat="server">
                                <td>
                                    <span class="required-red">*</span> Message :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMessage" class="contact-fild" Style="text-indent: 2px; resize: none;"
                                        Height="70px" Width="200px" TextMode="MultiLine" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span class="required-red">&nbsp;</span>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnAddToCart" runat="server" ToolTip="ADD TO CART" ImageUrl="/images/add-to-cart.jpg"
                                        OnClientClick="return checkValidation(); return checkquantity();" AlternateText="Add To Cart"
                                        OnClick="btnAddToCart_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
