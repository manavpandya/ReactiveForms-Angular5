<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Testimonials.aspx.cs" Inherits="Solution.UI.Web.Testimonials" %>

<%@ Register Assembly="Castle.Web.Controls.Rater" Namespace="Castle.Web.Controls"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .customer-name
        {
            color: #000000;
            float: left;
            padding-left: 8px;
            padding-right: 8px;
            text-align: left;
            width: 25%;
        }
        .customer-txt
        {
            float: left;
            padding-right: 10px;
            text-align: left;
            width: 70%;
        }
        .customer-comments-div
        {
            float: left;
            padding: 10px 0 5px;
            width: 100%;
        }
    </style>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="litBreadTitle" runat="server" Text="Testimonials"></asp:Literal></span>
    </div>
    <div class="content-main">
        <div class="static-title">
            <span>Testimonials</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <asp:DataList DataKeyField="TestimonialsID" runat="server" ID="dtlTestimonial" OnItemCommand="dtlTestimonial_ItemCommand1"
                    OnItemDataBound="dtlTestimonial_ItemDataBound1" Width="100%">
                    <ItemTemplate>
                        <div class="customer-rating-band" style="padding: 5px;">
                            <cc1:Rater ID="Rater" runat="server" AutoPostback="True" designtimedragdrop="128"
                                Enabled="False" ScriptUrl="/App_Themes/<%=Page.Theme %>/js/CastleRater.js" Value='<%#Bind("Rating") %>'></cc1:Rater>
                        </div>
                        <div class="customer-comments-div">
                            <div class="customer-name">
                                <b>Written by
                                    <asp:Label ID="hdnId" Visible="false" runat="server" Text='<%#Bind("TestimonialsID")%>'></asp:Label>
                                    <asp:Literal ID="ltrName" runat="server" Text='<%#Bind("Name") %>'></asp:Literal>
                                    from
                                    <asp:Literal ID="ltrCity" runat="server" Text='<%#Bind("City") %>'></asp:Literal>
                                </b>
                                <br>
                                <asp:Literal ID="ltrDate" runat="server" Text='<%#String.Format("{0:MMMM&nbsp;dd&nbsp;yyyy&nbsp;}",Convert.ToDateTime(Eval("CreatedOn")))%>'></asp:Literal></div>
                            <div class="customer-txt">
                                <asp:Literal ID="ltrComments" runat="server" Text='<%#Bind("Message") %>'></asp:Literal>
                            </div>
                        </div>
                        <div id="divusefulcnt" runat="server" style="padding-top: 5px; margin-top: 0px; padding-bottom: 15px;
                            border-bottom: 1px #ccc solid" class="customer-comments-div">
                            <div class="customer-name" id="divReview" runat="server">
                                <span class="poll">
                                    <asp:Literal ID="ltrYesCount" runat="server" Text='<%#Bind("YesCount") %>'></asp:Literal>
                                    <asp:Literal ID="ltrNoCount" runat="server" Text='<%#Bind("NoCount") %>' Visible="false"></asp:Literal>
                                    out of
                                    <asp:Literal ID="ltrtotalCount" runat="server"></asp:Literal>
                                    users found this review helpful.</span>
                            </div>
                            <div style="text-align: right; color: #B92127; float: right;" class="customer-txt">
                                Was this review helpful to you? <a id="aYes" runat="server" onclick="javascript:__doPostBack('ctl00$ContentPlaceHolder1$dtlTestimonial$ctl00$lnkyes','');"
                                    href="javascript:void(0);">
                                    <img src="/images/yes.gif" alt="Yes" /></a> <a id="aNo" runat="server" onclick="javascript:__doPostBack('ctl00$ContentPlaceHolder1$dtlTestimonial$ctl00$lnkno','');"
                                        href="javascript:void(0);">
                                        <img src="/images/no.gif" alt="No" /></a>
                                <asp:LinkButton ID="lnkyes" runat="server" Text="Yes" Visible="false" CommandArgument='<%#Bind("TestimonialsID") %>'
                                    CommandName="Yes"></asp:LinkButton>
                                <asp:LinkButton ID="lnkno" runat="server" Text="No" Visible="false" CommandArgument='<%#Bind("TestimonialsID") %>'
                                    CommandName="No"></asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="static-title" style="border: none;">
                <span>Write Testimonials</span>
            </div>
            <div class="static-main">
                <div class="static-main-box">
                    <table width="100%" style="float: left; margin-top: 10px;">
                        <tr>
                            <td style="width: 20%">
                                Name :
                            </td>
                            <td>
                                <asp:TextBox ID="txtname" runat="server" CssClass="contact-fild"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City :
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="contact-fild"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Country :
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="contact-fild"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="contact-fild"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Comment :
                            </td>
                            <td>
                                <asp:TextBox ID="txtcomment" runat="server" CssClass="register-textaria" style="resize:none;width:255px;" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rate this Site :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlrating" runat="server" CssClass="select-box" Width="69px"
                                    Style="margin-left: 0px;" AutoPostBack="false" onchange="ratingImage();">
                                    <asp:ListItem Value="0" Text="Rating"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                </asp:DropDownList>
                                <img src="/images/star-form1.jpg" id="img1" style="vertical-align: middle" />
                                <img src="/images/star-form1.jpg" id="img2" style="vertical-align: middle" />
                                <img src="/images/star-form1.jpg" id="img3" style="vertical-align: middle" />
                                <img src="/images/star-form1.jpg" id="img4" style="vertical-align: middle" />
                                <img src="/images/star-form1.jpg" id="img5" style="vertical-align: middle" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="margin-left: 40px;">
                                <asp:ImageButton ID="btnsubmit" style="margin-top:10px;" OnClientClick="return CheckExits();" runat="server"
                                    alt="SUBMIT" title="SUBMIT" ImageUrl="~/images/submit.png" OnClick="btnsubmit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%-- <uc1:leftmenu ID="leftmenu" runat="server" />--%>
    <script type="text/javascript">
        function ratingImage() {
            var indx = document.getElementById("<%=ddlrating.ClientID %>").selectedIndex;
            if (indx == 0) {
                document.getElementById("img1").src = '/images/star-form1.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';
            }
            else if (indx == 1) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form1.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 2) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form1.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 3) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form1.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 4) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form1.jpg';

            }
            else if (indx == 5) {
                document.getElementById("img1").src = '/images/star-form.jpg';
                document.getElementById("img2").src = '/images/star-form.jpg';
                document.getElementById("img3").src = '/images/star-form.jpg';
                document.getElementById("img4").src = '/images/star-form.jpg';
                document.getElementById("img5").src = '/images/star-form.jpg';

            }
        }

        function CheckExits() {

            if (document.getElementById('ContentPlaceHolder1_txtname').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter Name.');
                document.getElementById('ContentPlaceHolder1_txtname').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtCity').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter City.');
                document.getElementById('ContentPlaceHolder1_txtCity').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtCountry').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter Country.');
                document.getElementById('ContentPlaceHolder1_txtCountry').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter Email Address.');
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtEmail').value.replace(/^\s+|\s+$/g, ""))) {
                alert('Please Enter Valid Email Address.');
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtcomment').value.replace(/^\s+|\s+$/g, "") == '') {
                alert('Please Enter Comment');
                document.getElementById('ContentPlaceHolder1_txtcomment').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {
                alert('Please Select Rating.');
                document.getElementById('ContentPlaceHolder1_ddlrating').focus();
                return false;
            }
            document.getElementById('prepage').style.display = '';

            return true;
        }
        var Emailresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                Emailresults = true
            else {
                Emailresults = false
            }
            return (Emailresults)
        }
    </script>
</asp:Content>
