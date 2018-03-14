<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderReceived.aspx.cs" Inherits="Solution.UI.Web.OrderReceived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/js/jquery-1.3.2.js" type="text/javascript"></script>
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
        function validationUser() {
            if (document.getElementById("ContentPlaceHolder1_txtEmailAddress") != null && document.getElementById("ContentPlaceHolder1_txtEmailAddress").value == '') {
                alert("Please Enter Email Address.");
                document.getElementById("ContentPlaceHolder1_txtEmailAddress").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtEmailAddress").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtEmailAddress").value)) {

                alert("Please Enter valid Email Address.");

                document.getElementById("ContentPlaceHolder1_txtEmailAddress").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtPassword") != null && document.getElementById("ContentPlaceHolder1_txtPassword").value == '') {

                alert("Please Enter Password.");
                document.getElementById("ContentPlaceHolder1_txtPassword").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtPassword") != null && document.getElementById("ContentPlaceHolder1_txtPassword").value.length < 6) {

                alert("Password must be at least 6 characters long.");

                document.getElementById("ContentPlaceHolder1_txtPassword").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtConfirmPassword") != null && document.getElementById("ContentPlaceHolder1_txtConfirmPassword").value == '') {

                alert("Please Enter Confirm Password.");

                document.getElementById("ContentPlaceHolder1_txtConfirmPassword").focus();
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtPassword").value != document.getElementById("ContentPlaceHolder1_txtConfirmPassword").value) {

                alert("Confirm Password must be match with Password.");

                document.getElementById("ContentPlaceHolder1_txtConfirmPassword").focus();
                return false;
            }
            if (document.getElementById("prepage") != null) { document.getElementById("prepage").style.display = ''; }

            return true;
        }
    </script>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span>Order Received</span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>Order Received</span>
        </div>
        <div class="static-big-main">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" style="float: left;">
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" border="0" class="table-none" width="100%">
                            <tbody>
                                <tr>
                                    <td align="left">
                                        Your order number is : <strong class="required-red" style="font-size: 21px;">
                                            <asp:Literal ID="ltrorderNumber" runat="server"></asp:Literal>
                                        </strong>
                                        <br />
                                        <br />
                                        Your order has been received and is being processed pending payment approval. You
                                        will receive email confirmation when your order has been approved and sent to our
                                        warehouse for shipment. We have emailed a summary of this order to your email address.<br />
                                        <br />
                                        You can check the status of your order by entering the order number on our Order
                                        Detail Page.<br />
                                        <br />
                                        <a href="javascript:void(0);" id="aPrint" target="_blank" runat="server">
                                            <img src="/images/print-full-receipt.png" alt="PRINT FULL RECEIPT" /></a> &nbsp;
                                        &nbsp;
                                        <asp:ImageButton ID="imgDownload" runat="server" ImageUrl="/images/download-pdf-receipt.png"
                                            OnClick="imgDownload_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnllogin" runat="server" DefaultButton="btnCreateAccount">
                            <table cellspacing="0" cellpadding="0" border="0" class="table-none" style="width: 100%;"
                                id="tblLogin" runat="server">
                                <tbody>
                                    <tr>
                                        <th colspan="3">
                                            Create an Account
                                        </th>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            Create an account by choosing a e-mail and Password. By creating an account you
                                            will have the ability to :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="18%" valign="middle">
                                            E-Mail Address
                                        </td>
                                        <td width="2%" align="center" valign="middle">
                                            :
                                        </td>
                                        <td width="80%" valign="middle">
                                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="checkout-textfild"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            Password
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="middle">
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="checkout-textfild"
                                                OnPreRender="txtPassword_PreRender"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            Confirm Password
                                        </td>
                                        <td valign="middle" align="center">
                                            :
                                        </td>
                                        <td valign="middle">
                                            <asp:TextBox ID="txtConfirmPassword" TextMode="Password" runat="server" CssClass="checkout-textfild"
                                                OnPreRender="txtConfirmPassword_PreRender"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            &nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnCreateAccount" OnClientClick="return validationUser();" runat="server"
                                                alt="CREATE AN ACCOUNT" title="CREATE AN ACCOUNT" ImageUrl="/images/create-an-account.png"
                                                OnClick="btnCreateAccount_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <strong>Note</strong> : for your convenience, your E-Mail Address has filled for
                                            you. Special characters as * &amp; ! are not allowed. Your personal password must
                                            be at least 6 characters long.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <%--         <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
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
            </div>--%>
            <div id="prepage" style="position: absolute; font-family: arial; font-size: 16px; left: 0px;
                top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
                height: 100%; width: 100%; z-index: 1000; display: none;">
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
        </div>
    </div>
  <%--   <%=strCode%>
    <%=strcretio%> 
    <%=strgoogletrack %>  
    <%=strfacebook %>
      <%=strgoogletrustedstore %>
        --%>
    

    <script type="text/javascript" charset="utf-8">var ju_num = "1291E94D-66CC-4905-A45E-E98999D45F25"; var asset_host = '//d2j3qa5nc37287.cloudfront.net/'; document.write(unescape("%3Cscript src='" + asset_host + "coupon_conversion.js' type='text/javascript'%3E%3C/script%3E"));</script>
    <div style="float:left;width:100%;">
       <asp:Literal ID="ltshareasale" runat="server"></asp:Literal>
    </div>
    <script type="text/javascript"> if (!window.mstag) mstag = { loadTag: function () { }, time: (new Date()).getTime() };</script> <script id="mstag_tops" type="text/javascript" src="//flex.msn.com/mstag/site/e3fdeb46-2e31-46e9-8118-25d2802cd816/mstag.js"></script> <script type="text/javascript">        mstag.loadTag("analytics", { dedup: "1", domainId: "928730", type: "1", revenue: "", actionid: "24932" })</script> <noscript> <iframe src="//flex.msn.com/mstag/tag/e3fdeb46-2e31-46e9-8118-25d2802cd816/analytics.html?dedup=1&domainId=928730&type=1&revenue=&actionid=24932" frameborder="0" scrolling="no" width="1" height="1" style="visibility:hidden;display:none"> </iframe> </noscript>
</asp:Content>
