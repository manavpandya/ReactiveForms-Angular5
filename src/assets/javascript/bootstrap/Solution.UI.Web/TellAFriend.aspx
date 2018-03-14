<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TellAFriend.aspx.cs" Inherits="Solution.UI.Web.TellAFriend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/js/jquery.min.js" type="text/javascript"></script>
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
        function chkHeight() {

            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>
    <script type="text/javascript">
        function checkfields() {

            if (document.getElementById('<%=txtname.ClientID %>') != null && document.getElementById('<%=txtname.ClientID %>').value == '') {
                alert('Please enter your name.');
                document.getElementById('<%=txtname.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value == '') {
                alert("Please enter your email");
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail.ClientID %>').value)) {
                alert('Please enter valid email.');
                document.getElementById('<%=txtEmail.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtReEmail.ClientID %>') != null && document.getElementById('<%=txtReEmail.ClientID %>').value == '') {
                alert("Please enter re-enter email.");
                document.getElementById('<%=txtReEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtReEmail.ClientID %>') != null && document.getElementById('<%=txtReEmail.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtReEmail.ClientID %>').value)) {
                alert('Please enter valid re-enter email.');
                document.getElementById('<%=txtReEmail.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtReEmail.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value != document.getElementById('<%=txtReEmail.ClientID %>').value) {
                alert('Re-enter email must be same with your email.');
                document.getElementById('<%=txtReEmail.ClientID %>').focus();
                return false;
            }

            if (document.getElementById('<%=txtEmail1.ClientID %>') != null && document.getElementById('<%=txtEmail1.ClientID %>').value == '') {
                alert("Please enter Email1");
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail1.ClientID %>') != null && document.getElementById('<%=txtEmail1.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail1.ClientID %>').value)) {
                alert('Please enter valid Email1.');
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtEmail.ClientID %>') != null && document.getElementById('<%=txtEmail1.ClientID %>') != null && document.getElementById('<%=txtEmail.ClientID %>').value == document.getElementById('<%=txtEmail1.ClientID %>').value) {
                alert('Your email and Email1 can not be same.');
                document.getElementById('<%=txtEmail1.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail2.ClientID %>') != null && document.getElementById('<%=txtEmail2.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail2.ClientID %>').value)) {
                alert('Please enter valid Email2.');
                document.getElementById('<%=txtEmail2.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtEmail3.ClientID %>') != null && document.getElementById('<%=txtEmail3.ClientID %>').value != '' && !checkemail1(document.getElementById('<%=txtEmail3.ClientID %>').value)) {
                alert('Please enter valid Email3.');
                document.getElementById('<%=txtEmail3.ClientID %>').focus();
                return false;
            }

            chkHeight();
            return true;
        }

    </script>
    <script type="text/javascript">
        function clearfields() {
            if (document.getElementById('<%=txtname.ClientID %>') != null) { document.getElementById('<%=txtname.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtEmail.ClientID %>') != null) { document.getElementById('<%=txtEmail.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtReEmail.ClientID %>') != null) { document.getElementById('<%=txtReEmail.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtEmail1.ClientID %>') != null) { document.getElementById('<%=txtEmail1.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtEmail2.ClientID %>') != null) { document.getElementById('<%=txtEmail2.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtEmail3.ClientID %>') != null) { document.getElementById('<%=txtEmail3.ClientID %>').value = ''; }
            if (document.getElementById('<%=txtMessage.ClientID %>') != null) { document.getElementById('<%=txtMessage.ClientID %>').value = 'I thought you would be interested in this item at <%=Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName") %> Enjoy!'; }
            if (document.getElementById('<%=txtname.ClientID %>') != null) { document.getElementById('<%=txtname.ClientID %>').focus(); }
            return false;
        }
    </script>
    <div class="breadcrumbs">
        <a title="Home" href="/index.aspx">Home </a>> <span>Email A Friend</span></div>
    <div class="content-main">
        <div class="static-title">
            <span>Email A Friend</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <asp:Panel ID="pnlTellafrnd" runat="server" DefaultButton="btnSend">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table-none">
                        <tbody>
                            <tr>
                                <td>
                                    <table cellspacing="1" cellpadding="6" border="0" width="100%">
                                        <tbody>
                                            <tr>
                                                <td align="left" rowspan="11" valign="top" style="width: 30%">
                                                    <span style="float: left; margin-left: 0px;">
                                                        <asp:Label ID="lblProductName" runat="server" Font-Size="13px"></asp:Label></span><br />
                                                    <br />
                                                    <asp:Image runat="server" ID="imgProduct" />
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td style="text-align: right; float: right; width: 70%;">
                                                    <span class="required-red">*</span>Required
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 121px;">
                                                    <span class="required-red">*</span>Your Name
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtname" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">*</span>Your Email
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">*</span>Re-enter&nbsp;Email
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReEmail" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span class="required-red">&nbsp;</span>Please enter your friend's email addresses
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">*</span>Email 1
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail1" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">&nbsp;</span>Email 2
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail2" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="required-red">&nbsp;</span>Email 3
                                                </td>
                                                <td>
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmail3" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <span class="required-red">&nbsp;</span>Your Message
                                                </td>
                                                <td valign="top">
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMessage" TextMode="MultiLine" Style="width: 224px; resize: none;"
                                                        Columns="25" Rows="5" runat="server" CssClass="register-textaria"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <span class="required-red">&nbsp;</span>The email that will be sent will content
                                                    your name
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <!---->
                                                    <%-- <asp:ImageButton ID="btnSend" runat="server" alt="SEND" title="SEND" OnClientClick="var i= checkfields();if(i==true){javascript:document.getElementById('prepage').style.display = '';}"
                                                    ImageUrl="/images/send.jpg" OnClick="btnSend_Click" />--%>
                                                    <asp:ImageButton ID="btnSend" runat="server" alt="SEND" title="SEND" OnClientClick="return checkfields();"
                                                        ImageUrl="/images/send.png" OnClick="btnSend_Click" />
                                                    &nbsp;&nbsp;
                                                    <asp:ImageButton ID="btnReset" runat="server" alt="RESET" title="RESET" OnClientClick="return clearfields();"
                                                        ImageUrl="/images/reset.png" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                   
                </asp:Panel>
            </div>
        </div>
    </div>
    <%--  <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>--%>
     <div id="prepage" style="position: absolute; font-family: arial; font-size: 16;
                        left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70);
                        layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
                        <div style="border: 1px solid #ccc;">
                            <table width="100%" style="position: fixed; top: 50%; left: 50%; margin: -50px 0 0 -100px;">
                                <tr>
                                    <td>
                                        <div style="background: none repeat scroll 0 0 rgba(0, 0,0, 0.9) !important; border: 1px solid #ccc;
                                            width: 10%; height: 3%; padding: 20px; -webkit-border-radius: 10px; -moz-border-radius: 10px;
                                            border-radius: 10px;">
                                            <center>
                                                <img alt="" src="/images/loding.png" style="text-align: center;" /><br />
                                                <b style="color: #fff;">Loading ... ... Please wait!</b></center>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
</asp:Content>
