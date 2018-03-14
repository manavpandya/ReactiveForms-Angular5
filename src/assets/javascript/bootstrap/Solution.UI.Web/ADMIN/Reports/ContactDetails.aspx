<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactDetails.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Reports.ContactDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_border">
        <tr>
            <td colspan="3" valign="middle">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="logo" width="80%">
                            <a href="#" title="Cash Register">
                                <img src="/App_Themes/<%=Page.Theme %>/images/logo.png" 
                                    style="float: left; padding: 10px 0 0 10px;" /></a>
                        </td>
                        <td align="right" width="20%" valign="top" style="padding-right: 10px; padding-top: 10px;">
                            <a href="javascript:window.close();" title="Close" class="close">
                                <img src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" alt="Close" style="border: 0px"
                                    title="Close" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left" style="color: #fff; height: 28px; background: #7d7d7d;
                border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                font-size: 12px; font-weight: bold;">
                <strong>Contact Inquiry Details</strong>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <table width="100%" border="0" cellpadding="4" cellspacing="0" style="padding-top: 10px;
                    line-height: 20px; padding-left: 10px;">
                    <tr>
                        <td align="left" style="width: 10%;">
                            <b>Name : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltname" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <b>Email : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltemail" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trAddress" runat="server" style="display:none;">
                        <td align="left">
                            <b>Address : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltAddress" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trCity" runat="server" style="display:none;">
                        <td align="left">
                            <b>City : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltCity" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td align="left">
                            <b>Country : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltCountry" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trState" runat="server" style="display:none;">
                        <td align="left">
                            <b>State : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltState" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td align="left">
                            <b>Zip Code : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltZipCode" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trPhone" runat="server" >
                        <td align="left">
                            <b>Phone : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trFax" runat="server" style="display:none;">
                        <td align="left">
                            <b>Fax : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltFax" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                         <td align="left" valign="top">
                            <b>Subject : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="LtSubjectStatus" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <b>Message : </b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="ltMessage" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
