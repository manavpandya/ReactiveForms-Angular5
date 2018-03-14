<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomQuoteSize.aspx.cs"
    Inherits="Solution.UI.Web.CustomQuoteSize" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body {
            background: #fff;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: #2a2a2a;
        }

        a, img {
            border: none;
            outline: none;
        }

        .table_border {
            border: 1px solid #D5D4D4;
            margin: 0 auto;
        }

        .checkout-red {
            color: #fe0000;
        }

        .name-input {
            border: 1px solid #E4E4E4;
            height: 18px;
            width: 210px;
            font-size: 12px;
            padding: 1px 5px;
        }

        .logo {
            float: left;
            width: 480px;
            background: #fff;
            padding: 0px 0 0px;
            margin: 0;
        }

        .close-bg {
            float: right;
            width: 30px;
            padding: 15px 40px 12px 0;
            text-align: left;
            background: #fff;
            margin: 0;
        }

        .option1 {
            border: 1px solid #DDDDDD;
            color: #848383;
            float: left;
            font-size: 12px;
            height: 20px;
            line-height: 20px;
            padding: 1px;
        }
    </style>
    <script src="js/popup.js" type="text/javascript"></script>
    <script src="/js/jquery-1.3.2.js" type="text/javascript"> </script>
    <script type="text/javascript" src="/js/jquery-alerts.js"></script>
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
    </script>
    <script type="text/javascript">
        function checkfields() {

            if (document.getElementById('<%=txtFirstName.ClientID %>') != null && document.getElementById('<%=txtFirstName.ClientID %>').value.trim() == '') {
                alert('Please enter First Name.');
                document.getElementById('<%=txtFirstName.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtLastName.ClientID %>') != null && document.getElementById('<%=txtLastName.ClientID %>').value.trim() == '') {
                alert("Please enter Last Name.");
                document.getElementById('<%=txtLastName.ClientID %>').focus();
                return false;
            }
           <%-- if (document.getElementById('<%=txtAddress.ClientID %>') != null && document.getElementById('<%=txtAddress.ClientID %>').value.trim() == '') {
                alert("Please enter Address.");
                document.getElementById('<%=txtAddress.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtCity.ClientID %>') != null && document.getElementById('<%=txtCity.ClientID %>').value.trim() == '') {
                alert("Please enter City.");
                document.getElementById('<%=txtCity.ClientID %>').focus();
                return false;
            }--%>
            //            if ((document.getElementById('ContentPlaceHolder1_ddlState').options[document.getElementById('ContentPlaceHolder1_ddlState').selectedIndex]).text == 'Select State') {
            //                 alert("Please select state.");
            //                 document.getElementById('ContentPlaceHolder1_ddlState').focus();
            //                 return false;
            //            }
           <%-- if (document.getElementById('<%=txtZip.ClientID %>') != null && document.getElementById('<%=txtZip.ClientID %>').value.trim() == '') {
                alert("Please enter Zip Code.");
                document.getElementById('<%=txtZip.ClientID %>').focus();
                return false;
            }--%>
            if (document.getElementById('<%=txttelephone.ClientID %>') != null && document.getElementById('<%=txttelephone.ClientID %>').value.trim() == '') {
                alert("Please enter phone Number.");
                document.getElementById('<%=txttelephone.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value.trim() == '') {
                alert("Please enter Email.");
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value.trim())) {
                alert('Please enter valid Email.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
        document.getElementById("prepage").style.display = '';
        return true;
    }

    </script>
    <script type="text/javascript">
        function clearfields() {
            document.getElementById('<%=txtFirstName.ClientID %>').value = '';
            document.getElementById('<%=txtLastName.ClientID %>').value = '';
            document.getElementById('<%=txtAddress.ClientID %>').value = '';
            document.getElementById('<%=txtCity.ClientID %>').value = '';
            document.getElementById('<%=txtZip.ClientID %>').value = '';
            document.getElementById('<%=txtInstruction.ClientID %>').value = '';
            document.getElementById('<%=txttelephone.ClientID %>').value = '';
            document.getElementById('<%=txtEmail.ClientID %>').value = '';
            document.getElementById('<%=txtFirstName.ClientID %>').focus();
            return false;
        }
    </script>
    <script type="text/javascript">
        function onKeyPressPhone(e) {
            var key = window.event ? window.event.keyCode : e.which;

            if (key == 32 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
                return key;
            }

            var keychar = String.fromCharCode(key);

            var reg = /\d/;
            if (window.event)
                return event.returnValue = reg.test(keychar);
            else
                return reg.test(keychar);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="hidden" id="hdnheaderqoute" runat="server" value="" />
            <input type="hidden" id="hdnwidthqoute" runat="server" value="" />
            <input type="hidden" id="hdnlengthqoute" runat="server" value="" />
            <input type="hidden" id="hdnoptionhqoute" runat="server" value="" />
            <input type="hidden" id="hdnquantityqoute" runat="server" value="0" />
            <input type="hidden" id="hdncord" runat="server" value="" />
        </div>
        <table width="610" border="0" cellpadding="0" cellspacing="0" class="table_border">
            <%--  <tr>
            <td colspan="3" valign="middle">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="logo">
                            <a href="#" title="Cash Register">
                                <img src="/images/logo.png" style="float: left; padding: 10px 0 0 10px;" /></a>
                        </td>
                        <td class="close-bg">
                            <a onclick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
                                href="javascript:void(0);" title="Close" class="close">
                                <img src="/images/Close.png" alt="Close" style="border: 0px" title="Close" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>--%>
            <tr>
                <td colspan="2" align="left" style="color: #000000; height: 28px; background: #F1F1f1; border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px; font-size: 12px; font-weight: bold;">
                    <strong style="float: left; width: 572px;">DESIGN INQUIRY</strong> <a onclick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
                        href="javascript:void(0);" title="Close" class="close" style="border: 0px; float: right; position: absolute; top: 12px; display: none;">
                        <img src="/images/reset_search_all.png" alt="Close" style="border: 0px;" title="Close" /></a>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table width="100%" border="0" cellpadding="4" cellspacing="0" style="width: 705px">
                        <tr id="trContent" runat="server" visible="false">
                            <td colspan="3">
                                <asp:Literal ID="ltrCustomContent" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <span class="checkout-red">*</span>Required Fields
                            </td>
                        </tr>


                        <tr>
                            <td width="32%">Number of Windows </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtnowindows" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Purpose of Drapery</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlpurposeofdrapery" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="Decorative Only" Value="Decorative Only"></asp:ListItem>
                                    <asp:ListItem Text="Functioning" Value="Functioning"></asp:ListItem>
                                </asp:DropDownList>


                            </td>
                        </tr>
                        <tr>
                            <td>If Functioning</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfunctioning" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="One Way Draw" Value="One Way Draw"></asp:ListItem>
                                    <asp:ListItem Text="Center Draw" Value="Center Draw"></asp:ListItem>
                                </asp:DropDownList>


                            </td>
                        </tr>
                        <tr>
                            <td>Window Width </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtwindowwidth" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                                (Outside of Molding to Outside of Molding)
                            </td>
                        </tr>

                        <tr>
                            <td>Top of Window to Floor</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtwindowtofloor" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                                (Top of Molding to Floor)
                            </td>
                        </tr>
                        <tr>
                            <td>Ceiling Height</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtCeilingheight" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>


                        <tr>
                            <td>Drapery Style</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldraperystyle" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="Rod Pocket" Value="Rod Pocket"></asp:ListItem>
                                    <asp:ListItem Text="Pleated" Value="Pleated"></asp:ListItem>
                                    <asp:ListItem Text="Grommet" Value="Grommet"></asp:ListItem>
                                    <asp:ListItem Text="Back Tab" Value="Back Tab"></asp:ListItem>
                                </asp:DropDownList>


                            </td>
                        </tr>

                        <tr>
                            <td>Lining Option</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlliningoption" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="Lining" Value="Lining"></asp:ListItem>
                                    <asp:ListItem Text="Interlining" Value="Interlining"></asp:ListItem>
                                    <asp:ListItem Text="Black Out" Value="Black Out"></asp:ListItem>
                                </asp:DropDownList>


                            </td>
                        </tr>


                        <tr>
                            <td>Is Your Rod Already in Place?</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlisreadyrod" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:DropDownList>


                            </td>
                        </tr>

                        <tr>
                            <td>Have you ordered with us before?</td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlhaveuordered" runat="server" CssClass="option1" Width="221px">
                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">&nbsp;</span>Any further comments?
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstruction" TextMode="MultiLine" Columns="45" Rows="3" runat="server"
                                    CssClass="contact-textaria" MaxLength="100" Style="width: 217px; resize: none; height:30px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 120px;">
                                <span class="checkout-red">*</span>First Name
                            </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="name-input" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">*</span>Last Name
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">&nbsp;</span>Address
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" TextMode="MultiLine" Columns="45" Rows="2" runat="server"
                                    CssClass="contact-textaria" MaxLength="100" Style="width: 210px; resize: none;height:30px;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">&nbsp;</span>City
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">&nbsp;</span>State
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstate" runat="server" CssClass="option1" Style="width: 212px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">&nbsp;</span>Zip Code
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtZip" runat="server" CssClass="name-input" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">*</span>Phone
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txttelephone" runat="server" CssClass="name-input" MaxLength="20"
                                    onkeypress="return onKeyPressPhone(event);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="checkout-red">*</span>Email
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="name-input" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">&nbsp;
                            </td>
                            <td>
                                <asp:ImageButton ID="btnSend" Style="margin: 0px 10px 0px 0px; border: 0px" runat="server"
                                    alt="SEND" title="SEND" ImageUrl="/images/send.png" OnClientClick="return checkfields();"
                                    OnClick="btnSend_Click" />
                                <asp:ImageButton ID="btnReset" Style="border: 0px" runat="server" alt="RESET" title="RESET"
                                    ImageUrl="/images/reset.png" OnClientClick="return clearfields();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
            <table width="100%" style="padding-top: 400px;" align="center">
                <tr>
                    <td align="center" style="color: #fff;" valign="middle">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
