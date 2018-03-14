<%@ Page Title="" Language="C#" MasterPageFile="~/RMA/OverStock/RMAOverStock.Master" AutoEventWireup="true" CodeBehind="ReturnMerchandise.aspx.cs" Inherits="Solution.UI.Web.RMA.OverStock.ReturnMerchandise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" src="/js/ReturnMerchandise.js" type="text/javascript"></script>
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
    </script>

    <div class="breadcrumbs">
        <span>Return Merchandise</span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>Return Merchandise</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                    <tbody>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <span class="required-red">*</span> Required Fields
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td style="width: 20%;">
                                                <span class="required-red">*</span>Order Number
                                            </td>
                                            <td style="width: 3%;">:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrderNumber" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                <asp:HiddenField ID="hdReturnFee" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">&nbsp;</span>Customer Name
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>E-Mail
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span>Invoice Date
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr id="trRreturnProduct" runat="server" visible="false">
                            <td>
                                <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table_none">
                                    <tbody>
                                        <tr>
                                            <th style="padding: 2px 3px; padding-left: 10px; height: 20px" colspan="2" align="left">Return Item Details
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="width: 145px;" valign="top">&nbsp;&nbsp; ProductName :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblProductName" runat="server"></asp:Label>
                                                <br />
                                                <asp:Literal ID="ltrVariant" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;&nbsp; SKU :
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblProductSKU" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: Red;">*</span> Quantity :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtProductQty" Style="text-align: center;" onkeypress="return onKeyPressPhone(event);"
                                                    runat="server" CssClass="aspNetDisabled checkout-textfild" MaxLength="2" Width="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr id="trReturnProductAll" runat="server">
                            <td align="left" colspan="4">
                                <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                    <tbody>
                                        <tr>
                                            <th align="left">
                                                <span class="required-red">*</span>Items to be Returned
                                            </th>
                                            <th align="left">
                                                <span class="required-red">*</span>SKU
                                            </th>
                                            <th align="left" valign="middle">
                                                <span class="required-red">*</span> Quantity
                                            </th>
                                            <th align="center">Item in Mail
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemReturned1" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemCode1" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtQuantity1" runat="server" CssClass="advance-quantity"></asp:TextBox>
                                                <asp:TextBox ID="txtproductid1" runat="server" CssClass="register_fild" Visible="false"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:CheckBox ID="chkItem1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemReturned2" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemCode2" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtQuantity2" runat="server" CssClass="advance-quantity"></asp:TextBox>
                                                <asp:TextBox ID="txtproductid2" runat="server" CssClass="register_fild" Visible="false"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:CheckBox ID="chkItem2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemReturned3" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemCode3" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtQuantity3" runat="server" CssClass="advance-quantity"></asp:TextBox>
                                                <asp:TextBox ID="txtproductid3" runat="server" CssClass="register_fild" Visible="false"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:CheckBox ID="chkItem3" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemReturned4" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtItemCode4" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtQuantity4" runat="server" CssClass="advance-quantity"></asp:TextBox>
                                                <asp:TextBox ID="txtproductid4" runat="server" CssClass="register_fild" Visible="false"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:CheckBox ID="chkItem4" runat="server" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                                    <tbody>
                                        <tr>
                                            <th align="left" class="table">Return Reason
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="required-red">*</span><span style="color: Red;" id="ctl00_wpm_SugestaProduct_ctl05_DefMessage">
                                                    All defective products will be tested, if the result shows passed. A
                                                    <asp:Literal ID="ltReturnFee" runat="server"></asp:Literal>
                                                    restocking fee may be applied to the credit.
                                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" style="width: 100%;">
                                                    <tbody>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rbtReason1" runat="server" GroupName="ReturnReason" onclick="javascript:document.getElementById('DIVWrongItem').style.display='none';" />
                                                                <asp:Label ID="lblReason1" runat="server" Text="Refund On Defective Products"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr1" visible="false" runat="server">
                                                            <td>
                                                                <asp:RadioButton ID="rbtReason2" runat="server" GroupName="ReturnReason" onclick="javascript:document.getElementById('DIVWrongItem').style.display='none';" />
                                                                <asp:Label ID="lblReason2" runat="server" Text="Replacement On Defective Products"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButton ID="rbtReason4" runat="server" GroupName="ReturnReason" onclick="javascript:document.getElementById('DIVWrongItem').style.display='none';" /><asp:Label
                                                                    ID="lblReason4" runat="server" Text=" Order Error. 20% restocking fee will be applied. Shipping Fee is not refundable."></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="float: left; padding: 0px;">
                                                                <table style="float: left;" width="100%">
                                                                    <tr>
                                                                        <td style="width: 435px;">
                                                                            <asp:RadioButton ID="rbtReason3" runat="server" GroupName="ReturnReason" onclick="javascript:document.getElementById('DIVWrongItem').style.display='block';" />
                                                                            <asp:Label ID="lblReason3" runat="server" Text="Received Wrong Items. Please state item you received here "></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <div id="DIVWrongItem" style="float: right; padding-right: 160px; display: none;">
                                                                                <asp:TextBox ID="txtWrongItem" runat="server" CssClass="register_fild" Width="130px"></asp:TextBox>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-none">
                                    <tbody>
                                        <tr>
                                            <th align="left">Any Additional Information
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtAdditionalInformation" runat="server" TextMode="MultiLine" CssClass="texfild-new-return"
                                                    Rows="5" cols="82" Style="width: 920px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="border-none table-none">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <ul>
                                                    <li><span style="color: #000000;"></span>&nbsp;Products must be Sealed, Unopened, and
                                                        in Original Packaging.</li>
                                                    <li><span style="color: #000000;"></span>&nbsp;Please CLEARLY
                                                            MARK YOUR RMA NUMBER OUTSIDE of EVERY BOX you send to
                                                            <%=  Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName")%>
                                                    </li>
                                                    <li><span style="color: #000000;"></span>&nbsp;All RMA numbers are valid for 15 days
                                                        from the date of issuance. </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td width="24%" align="left" valign="middle">
                                                <span class="required-red">&nbsp;</span>Verification
                                            </td>
                                            <td width="3%" align="left" valign="middle">:
                                            </td>
                                            <td width="75%" align="left">
                                                <img width="150px" height="40px" class="img-left" id="imgcapcha" alt="" src="/JpegImage.aspx?id=343343" />
                                                <input tabindex="33" type="button" value="" title="Reload" id="btnreload" style="background: url(/images/reload-icon.png) no-repeat transparent; width: 31px; height: 29px; border: none; cursor: pointer; margin: 8px 0 0 5px;"
                                                    onclick="ReloadCapthca();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="required-red">*</span>Enter the code shown
                                            </td>
                                            <td align="left">:
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCodeshow" runat="server" Width="60px" CssClass="advance-quantity"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4">
                                <div id="divreturn" runat="server" style="height: 70px; width: 100%; overflow: auto; border: 1px solid #DDDDDD; padding: 2px;">
                                    <asp:Label ID="lblReturn" runat="server"></asp:Label>
                                </div>
                                <br />
                                <asp:CheckBox ID="chkreturnpolicy" runat="server" />
                                I have read and understood the return police of halfpricedraps.com
                                <input type="hidden" id="hdnProductId" runat="server" value="" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="btnsubmit" runat="server" alt="FINISH" title="FINISH" OnClientClick="return ValidatePage()"
                                    ImageUrl="/images/finish.png" OnClick="btnsubmit_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <%--commente--%>
    <div class="static-main-box" style="width: 969px;">
    </div>

</asp:Content>
