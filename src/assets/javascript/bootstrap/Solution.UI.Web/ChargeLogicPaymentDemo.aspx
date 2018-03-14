<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChargeLogicPaymentDemo.aspx.cs" Inherits="Solution.UI.Web.ChargeLogicPaymentDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript" src="https://connect.chargelogic.com/ChargeLogicMerchant.js"></script> 
    <script src="https://connect.chargelogic.com/ChargeLogicConnectEmbed.js" type="text/javascript"></script>
    <script type="text/javascript">

        function iframerealod(id)
        {
            document.getElementById('iframeId').src = 'https://connect.chargelogic.com/?HostedPaymentID=' + id;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="border:solid 1px #d7d7d7">
                <tr style="background-color:#d7d7d7;font-weight:bold;">
                    <td style="align: left;">Credential
                    </td>
                </tr>
                <tr>
                    <td style="align: left;">
                        <table>
                            <tr>
                                <td>StoreNo :</td>
                                <td>
                                    <asp:TextBox ID="txtStoreno" runat="server" Text="CHARGELOGIC\EFFTEST"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>APIKey :</td>
                                <td>
                                    <asp:TextBox ID="txtAPIKey" runat="server" TextMode="Password"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>ApplicationNo :</td>
                                <td>
                                    <asp:TextBox ID="txtApplicationNo" runat="server" Text="EFFWEB"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>ApplicationVersion :</td>
                                <td>
                                    <asp:TextBox ID="txtApplicationVersion" runat="server" Text="4.00.04"></asp:TextBox>&nbsp;(4.00.04\4040)</td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr style="background-color:#d7d7d7;font-weight:bold;">
                    <td style="align: left;">Transaction
                    </td>
                </tr>
                <tr>
                    <td style="align: left;">
                        <table>
                            <tr>
                                <td>Currency :</td>
                                <td>
                                    <asp:TextBox ID="txtCurrency" runat="server" Text="USD"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Amount :</td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" Text="1.00"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>ExternalReferenceNumber :</td>
                                <td>
                                    <asp:TextBox ID="txtExternalReferenceNumber" runat="server" Text="989890"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>ConfirmationID :</td>
                                <td>
                                    <asp:TextBox ID="txtConfirmationID" runat="server" Text="989890"></asp:TextBox></td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr style="background-color:#d7d7d7;font-weight:bold;">
                    <td style="align: left;">Billing Address
                    </td>
                </tr>
                <tr>
                    <td style="align: left;">
                        <table>
                            <tr>
                                <td>Name :</td>
                                <td>
                                    <asp:TextBox ID="txtNamebill" runat="server" Text="Hetal Patel"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>StreetAddress :</td>
                                <td>
                                    <asp:TextBox ID="txtStreetAddressbill" runat="server" Text="356 street"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>City :</td>
                                <td>
                                    <asp:TextBox ID="txtCitybill" runat="server" Text="waltham"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>State :</td>
                                <td>
                                    <asp:TextBox ID="txtStatebill" runat="server" Text="Massachusetts"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>PostCode :</td>
                                <td>
                                    <asp:TextBox ID="txtPostCodebill" runat="server" Text="02451"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Country :</td>
                                <td>
                                    <asp:TextBox ID="txtCountrybill" runat="server" Text="US"></asp:TextBox>&nbsp;(US\USA)</td>
                            </tr>
                            <tr>
                                <td>PhoneNumber :</td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNumberbill" runat="server" Text="111-111-1111"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Email :</td>
                                <td>
                                    <asp:TextBox ID="txtEmailbill" runat="server" Text="hetal@kaushalam.com"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
              <tr style="background-color:#d7d7d7;font-weight:bold;">
                    <td style="align: left;">Shipping Address
                    </td>
                </tr>

                <tr>
                    <td style="align: left;">
                        <table>
                            <tr>
                                <td>Name :</td>
                                <td>
                                    <asp:TextBox ID="txtNameshipp" runat="server" Text="Hetal Patel"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>StreetAddress :</td>
                                <td>
                                    <asp:TextBox ID="txtStreetAddressshipp" runat="server" Text="356 street"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>City :</td>
                                <td>
                                    <asp:TextBox ID="txtCityshipp" runat="server" Text="waltham"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>State :</td>
                                <td>
                                    <asp:TextBox ID="txtStateshipp" runat="server" Text="Massachusetts"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>PostCode :</td>
                                <td>
                                    <asp:TextBox ID="txtPostCodeshipp" runat="server" Text="02451"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Country :</td>
                                <td>
                                    <asp:TextBox ID="txtCountryshipp" runat="server" Text="US"></asp:TextBox>&nbsp;(US\USA)</td>
                            </tr>
                            <tr>
                                <td>PhoneNumber :</td>
                                <td>
                                    <asp:TextBox ID="txtPhoneNumbershipp" runat="server" Text="111-111-1111"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Email :</td>
                                <td>
                                    <asp:TextBox ID="txtEmailshipp" runat="server" Text="hetal@kaushalam.com"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="background-color:#d7d7d7;font-weight:bold;">
                    <td style="align: left;">Hosted Payment
                    </td>
                </tr>
                               <tr>
                    <td style="align: left;">
                        <table>
                            <tr>
                                <td>RequireCVV :</td>
                                <td>
                                    <asp:TextBox ID="txtRequireCVV" runat="server" Text="Yes" ></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>ReturnURL :</td>
                                <td>
                                    <asp:TextBox ID="txtReturnURL" runat="server" Text="http://www.halfpricedrapes.com/ChargeLogicPaymentDemo.aspx"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Language :</td>
                                <td>
                                    <asp:TextBox ID="txtLanguage" runat="server" Text="EN"></asp:TextBox>&nbsp;EN\ENG</td>
                            </tr>
                            <tr>
                                <td>ConfirmationID :</td>
                                <td>
                                    <asp:TextBox ID="txtConfirmationIDhoste" runat="server" Text="989890"></asp:TextBox></td>
                            </tr>
                             
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="align: left;"><asp:Button ID="btnCheck" runat="server" Text="Request" OnClick="btnCheck_Click" />
                    </td>
                </tr>
            </table>

        </div>
        <div style="width:100%;">
             <iframe  height="500" scrolling="no" id="iframeId" runat="server"   style="border: none;width:100%"></iframe>
        </div>
        <div id="divId"></div>
        <div style="float:left">
            <asp:Button ID="btnrequest" runat="server" Text="Save" OnClientClick="submitPayment('iframeId', 'divId'); return false;" />


        </div>
        <%--ED44D0E3-22EC-4824-971C-3F259500086D--%>
        <%--6B402AF5-DD9B-42A4-AC75-3205920575EB--%>
         <%--src="https://connect.chargelogic.com/?HostedPaymentID=7191DFCA-3421-4A8A-8DF1-92A033D2946E"--%>
    </form>
</body>
</html>
