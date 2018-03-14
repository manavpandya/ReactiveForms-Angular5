<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderStatus.aspx.cs" Inherits="Solution.UI.Web.OrderStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function checkfieldsforOrderStatus() {


            if (document.getElementById('<%=txtOrderNumber.ClientID %>').value == '') {
                alert('Please enter Order Number.');
                document.getElementById('<%=txtOrderNumber.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>').value == '') {
                alert('Please enter email address.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
                alert('Please enter valid email address.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            return true;
        }

        function isNumberKeyOrderNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))

                return false;


            return true;
        }
    </script>
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
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="ltbrTitle" runat="server" Text="Order Status"></asp:Literal></span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>
                <asp:Literal ID="ltTitle" runat="server" Text="Order status"></asp:Literal></span>
        </div>
        <div class="static-big-main" style="min-height: 200px;">
            <table border="0" align="center" width="50%" id="tblOrderDetail" runat="server">
                <tr>
                    <td>
                        <asp:Panel ID="pnlOrderStatus" runat="server" DefaultButton="btnSubmit">
                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td width="33%" align="left" valign="top" class="td-broder">
                                            <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0" class="border-none">
                                                <tr>
                                                    <th colspan="2">
                                                        Order Detail
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Order Number :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOrderNumber" MaxLength="50" EnableViewState="false" runat="server"
                                                            onkeypress="return isNumberKeyOrderNumber(event)" CssClass="login-field"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Email :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEmail" MaxLength="100" EnableViewState="false" runat="server"
                                                            CssClass="login-field"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnSubmit" runat="server" alt="Submit" title="Submit" ImageUrl="~/images/submit.png"
                                                            OnClientClick="return checkfieldsforOrderStatus();" OnClick="btnSubmit_Click" />
                                                        &nbsp;&nbsp;
                                                        <asp:ImageButton ID="btnCancel" OnClick="btnCancel_Click" runat="server" alt="Cancel"
                                                            title="Cancel" ImageUrl="~/images/cancel.png" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Label ID="lblMsg" runat="server" Style="color: #ff0000; font-size: 12px; font-weight: bold;"></asp:Label></center>
            <asp:Label ID="lblTable" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
