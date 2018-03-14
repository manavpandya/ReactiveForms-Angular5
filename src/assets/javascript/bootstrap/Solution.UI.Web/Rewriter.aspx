<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Rewriter.aspx.cs" Inherits="Solution.UI.Web.Rewriter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //for newsletter
        function ValidNewSletter() {
            var element; if (document.getElementById('txtSubscriber'))
            { element = document.getElementById('txtSubscriber'); }
            if (document.getElementById('txtSubscriber'))
            { element = document.getElementById('txtSubscriber'); }
            if (element.value == '') {
                alert('Please enter your E-mail Address.'); if (document.getElementById('txtSubscriber'))
                { document.getElementById('txtSubscriber').focus(); }
                if (document.getElementById('txtSubscriber'))
                { document.getElementById('txtSubscriber').focus(); }
                return false;
            }
            else if (element.value == 'Enter your E-Mail Address') {
                alert('Please enter your E-Mail Address.'); if (document.getElementById('txtSubscriber'))
                { document.getElementById('txtSubscriber').focus(); }
                if (document.getElementById('txtSubscriber'))
                { document.getElementById('txtSubscriber').focus(); }
                return false;
            }
            else {
                var testresults; var str = element.value; var filter = /^.+@.+\..{2,3}$/; if (filter.test(str))
                { return true; }
                else {
                    alert("Please enter valid E-Mail Address.")
                    if (document.getElementById('txtSubscriber'))
                    { document.getElementById('txtSubscriber').focus(); }
                    if (document.getElementById('txtSubscriber'))
                    { document.getElementById('txtSubscriber').focus(); }
                    return false;
                }
            }
        }
        function clear_text() {
            if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
            { document.getElementById('txtSubscriber').value = ""; }
            if (document.getElementById('txtSubscriber') && document.getElementById('txtSubscriber').value == "Enter your E-Mail Address")
            { document.getElementById('txtSubscriber').value = ""; }
            return false;
        }
        function clear_NewsLetter(myControl) {
            if (myControl && myControl.value == "Enter your E-Mail Address")
                myControl.value = "";
        }
        function ChangeNewsLetter(myControl) {
            if (myControl != null && myControl.value == '')
                myControl.value = "Enter your E-Mail Address";
        }
        function Change() {
            if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
                document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; if (document.getElementById('txtSubscriber') != null && document.getElementById('txtSubscriber').value == '')
                document.getElementById('txtSubscriber').value = "Enter your E-Mail Address"; return false;
        }


        //For Search
        function clear_Search(myControl) {
            if (myControl && myControl.value == "Search by Keyword")
                myControl.value = "";
        }
        function ChangeSearch(myControl) {
            if (myControl != null && myControl.value == '')
                myControl.value = "Search by Keyword";
        }


        function ValidSearch() {
            var myControl;

            if (document.getElementById('txtSearch')) {
                myControl = document.getElementById('txtSearch');
            }



            if (myControl.value == '' || myControl.value == 'Search by Keyword') {
                alert("Please enter something to search");

                if (document.getElementById('txtSearch')) {
                    document.getElementById('txtSearch').focus();
                }


                return false;
            }

            if (myControl.value.length < 3) {
                alert("Please enter at least 3 characters to search");
                myControl.focus();
                return false;
            }
            return true;
        }

    </script>
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span>
            <asp:Literal ID="ltcontent" runat="server" Text="Content Not Found"></asp:Literal></span></div>
    <div class="content-main">
        <div class="static-title">
            <span>Content Not Found</span>
        </div>
        <div class="static-main" style="min-height: 200px;">
            <div class="static-main-box">
                Looking for something?<br />
                <br />
                We're sorry. The Web address you entered is not a working page on our site.
                <br />
                Please try our <a style="color: #B92127; font-weight: bold; text-decoration: underline;"
                    href="/search.aspx">Search</a> option to find the product.
            </div>
        </div>
    </div>
</asp:Content>
